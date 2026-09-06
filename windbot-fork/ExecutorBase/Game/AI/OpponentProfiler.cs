using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using YGOSharp.OCGWrapper.Enums;
using WindBot.Game;

namespace WindBot.Game.AI
{
    /// <summary>
    /// OpponentProfiler — Tracks and infers opponent deck archetype, likely hand contents,
    /// and predicted next-turn plays.
    /// 
    /// All archetype data is loaded from external JSON (archetypes.json)
    /// so it can be updated when the meta shifts without recompiling.
    /// 
    /// Key capabilities:
    ///   1. Deck Identification: Watch opponent's cards → determine archetype + confidence
    ///   2. Hand Inference: Track draws/searches → estimate hand trap / combo piece ratio
    ///   3. Threat Forecasting: Based on archetype + board state → predict opponent actions
    /// 
    /// Connected to AIBrain — updates automatically from game events.
    /// </summary>
    public class OpponentProfiler
    {
        // ═══════════════════════════════════════
        //  ARCHETYPE DATABASE (data-driven)
        // ═══════════════════════════════════════

        /// <summary>
        /// Archetype definition — loaded from JSON.
        /// </summary>
        public class ArchetypeProfile
        {
            public string Name { get; set; }
            public string[] CardKeywords { get; set; }
            public int[] KnownCardIds { get; set; }
            public string PlayStyle { get; set; } // "Combo", "Control", "Midrange", "OTK"
            public double HandTrapDensity { get; set; } // Estimated % of deck that's hand traps
            public string[] LikelyComboStarters { get; set; }
            public string[] LikelyEndBoard { get; set; }
        }

        /// <summary>
        /// JSON schema for archetypes.json
        /// </summary>
        public class ArchetypesConfig
        {
            /// <summary>List of archetype profiles.</summary>
            public ArchetypeProfile[] Archetypes { get; set; } = Array.Empty<ArchetypeProfile>();

            /// <summary>Default hand trap density when archetype is unknown.</summary>
            public double DefaultHandTrapDensity { get; set; } = 0.15;
        }

        private List<ArchetypeProfile> _archetypes = new List<ArchetypeProfile>();
        private double _defaultHandTrapDensity = 0.15;

        // ═══════════════════════════════════════
        //  TRACKED STATE
        // ═══════════════════════════════════════

        /// <summary>Detected archetype name. "Unknown" if not yet identified.</summary>
        public string DetectedArchetype { get; private set; } = "Unknown";

        /// <summary>Confidence in the detected archetype (0.0-1.0).</summary>
        public double ArchetypeConfidence { get; private set; } = 0.0;

        /// <summary>The detected archetype profile, or null.</summary>
        public ArchetypeProfile DetectedProfile { get; private set; }

        /// <summary>All card IDs seen from the opponent so far.</summary>
        private readonly HashSet<int> _seenCardIds = new HashSet<int>();
        private readonly HashSet<string> _seenCardNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> _archetypeMatchCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Number of cards opponent drew beyond normal draw this game.</summary>
        public int OpponentExtraDraws { get; private set; } = 0;

        /// <summary>Number of searches opponent performed this game.</summary>
        public int OpponentSearches { get; private set; } = 0;

        /// <summary>Estimated hand trap count remaining in opponent's hand.</summary>
        public double EstimatedHandTraps { get; private set; } = 0.0;

        // ═══════════════════════════════════════
        //  CONSTRUCTOR & CONFIG LOADING
        // ═══════════════════════════════════════

        public OpponentProfiler()
        {
        }

        /// <summary>
        /// Load archetype data from a JSON file.
        /// If file doesn't exist, the profiler still works using heuristic fallback
        /// (type-based detection: combo vs control based on board patterns).
        /// </summary>
        public void LoadConfig(string configPath)
        {
            try
            {
                if (!File.Exists(configPath))
                {
                    string[] searchPaths = {
                        Path.Combine(AppContext.BaseDirectory, "models", "archetypes.json"),
                        Path.Combine(AppContext.BaseDirectory, "archetypes.json"),
                        "models/archetypes.json",
                        "archetypes.json"
                    };

                    foreach (string path in searchPaths)
                    {
                        if (File.Exists(path))
                        {
                            configPath = path;
                            break;
                        }
                    }
                }

                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    var config = JsonSerializer.Deserialize<ArchetypesConfig>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (config != null)
                    {
                        if (config.Archetypes != null && config.Archetypes.Length > 0)
                            _archetypes = new List<ArchetypeProfile>(config.Archetypes);
                        if (config.DefaultHandTrapDensity > 0)
                            _defaultHandTrapDensity = config.DefaultHandTrapDensity;

                        System.Diagnostics.Debug.WriteLine($"[OpponentProfiler] Loaded {_archetypes.Count} archetypes from {configPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[OpponentProfiler] Config load failed: {ex.Message} — using heuristic fallback");
            }
        }

        // ═══════════════════════════════════════
        //  EVENT HOOKS
        // ═══════════════════════════════════════

        /// <summary>Call when a new opponent card is revealed (field, GY, banished).</summary>
        public void OnCardRevealed(ClientCard card)
        {
            if (card == null) return;
            if (card.Id != 0)
                _seenCardIds.Add(card.Id);
            if (!string.IsNullOrEmpty(card.Name))
                _seenCardNames.Add(card.Name);

            UpdateArchetypeDetection(card);
        }

        /// <summary>Call when opponent searches (deck → hand).</summary>
        public void OnOpponentSearch()
        {
            OpponentSearches++;
        }

        /// <summary>Call when opponent draws (beyond normal draw).</summary>
        public void OnOpponentExtraDraw()
        {
            OpponentExtraDraws++;
        }

        /// <summary>Call when opponent summons a monster.</summary>
        public void OnOpponentSummon(ClientCard card)
        {
            OnCardRevealed(card);
        }

        /// <summary>Call when opponent activates an effect.</summary>
        public void OnOpponentActivate(ClientCard card)
        {
            OnCardRevealed(card);
        }

        /// <summary>Reset for a new duel.</summary>
        public void ResetDuel()
        {
            _seenCardIds.Clear();
            _seenCardNames.Clear();
            _archetypeMatchCounts.Clear();
            DetectedArchetype = "Unknown";
            ArchetypeConfidence = 0.0;
            DetectedProfile = null;
            OpponentExtraDraws = 0;
            OpponentSearches = 0;
            EstimatedHandTraps = 0.0;
        }

        // ═══════════════════════════════════════
        //  ARCHETYPE DETECTION
        // ═══════════════════════════════════════

        private void UpdateArchetypeDetection(ClientCard card)
        {
            if (card == null) return;

            // Only run JSON-based detection if we have archetypes loaded
            if (_archetypes.Count > 0)
            {
                foreach (var archetype in _archetypes)
                {
                    bool matched = false;

                    // Check by card ID
                    if (card.Id != 0 && archetype.KnownCardIds != null)
                    {
                        if (archetype.KnownCardIds.Contains(card.Id))
                            matched = true;
                    }

                    // Check by name keywords
                    if (!matched && !string.IsNullOrEmpty(card.Name) && archetype.CardKeywords != null)
                    {
                        string nameLower = card.Name.ToLower();
                        if (archetype.CardKeywords.Any(kw => nameLower.Contains(kw.ToLower())))
                            matched = true;
                    }

                    if (matched)
                    {
                        if (!_archetypeMatchCounts.ContainsKey(archetype.Name))
                            _archetypeMatchCounts[archetype.Name] = 0;
                        _archetypeMatchCounts[archetype.Name]++;
                    }
                }
            }

            // Update best match
            if (_archetypeMatchCounts.Count > 0)
            {
                var best = _archetypeMatchCounts.OrderByDescending(kv => kv.Value).First();
                DetectedArchetype = best.Key;
                ArchetypeConfidence = Math.Min(1.0, best.Value / 4.0);
                DetectedProfile = _archetypes.FirstOrDefault(a =>
                    a.Name.Equals(best.Key, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                // Heuristic fallback: detect play style from board patterns
                // (works even without JSON config)
                InferPlayStyleFromBoard();
            }
        }

        /// <summary>
        /// Fallback detection when no JSON archetype data is available.
        /// Infers play style from observable patterns.
        /// </summary>
        private void InferPlayStyleFromBoard()
        {
            // If we've seen many Extra Deck monsters → likely Combo deck
            int extraDeckSeen = 0;
            int spellTrapSeen = 0;
            foreach (int id in _seenCardIds)
            {
                var namedCard = YGOSharp.OCGWrapper.NamedCard.Get(id);
                if (namedCard == null) continue;
                if (namedCard.HasType(CardType.Fusion) || namedCard.HasType(CardType.Synchro)
                    || namedCard.HasType(CardType.Xyz) || namedCard.HasType(CardType.Link))
                    extraDeckSeen++;
                if (namedCard.HasType(CardType.Trap)
                    || (namedCard.HasType(CardType.Spell) && namedCard.HasType(CardType.Continuous)))
                    spellTrapSeen++;
            }

            if (extraDeckSeen >= 3)
            {
                // Lots of ED monsters → Combo
                DetectedArchetype = "Unknown-Combo";
                DetectedProfile = new ArchetypeProfile
                {
                    Name = "Unknown-Combo",
                    PlayStyle = "Combo",
                    HandTrapDensity = 0.12
                };
                ArchetypeConfidence = 0.3;
            }
            else if (spellTrapSeen >= 3)
            {
                // Lots of traps/continuous → Control
                DetectedArchetype = "Unknown-Control";
                DetectedProfile = new ArchetypeProfile
                {
                    Name = "Unknown-Control",
                    PlayStyle = "Control",
                    HandTrapDensity = 0.18
                };
                ArchetypeConfidence = 0.3;
            }
        }

        // ═══════════════════════════════════════
        //  HAND INFERENCE
        // ═══════════════════════════════════════

        /// <summary>
        /// Estimate how many hand traps the opponent might have in hand right now.
        /// Based on: archetype hand trap density, hand size, cards spent.
        /// </summary>
        public double EstimateHandTrapsInHand(int currentHandCount)
        {
            if (currentHandCount <= 0) return 0.0;

            double density = _defaultHandTrapDensity;
            if (DetectedProfile != null && DetectedProfile.HandTrapDensity > 0)
                density = DetectedProfile.HandTrapDensity;

            double estimate = currentHandCount * density;

            // Late game adjustment
            if (_seenCardIds.Count > 10)
                estimate *= 0.7;

            EstimatedHandTraps = Math.Max(0.0, estimate);
            return EstimatedHandTraps;
        }

        // ═══════════════════════════════════════
        //  PLAY STYLE QUERIES
        // ═══════════════════════════════════════

        /// <summary>Is the detected opponent a control deck?</summary>
        public bool IsControlDeck()
        {
            return DetectedProfile?.PlayStyle == "Control";
        }

        /// <summary>Is the detected opponent a combo deck?</summary>
        public bool IsComboDeck()
        {
            return DetectedProfile?.PlayStyle == "Combo";
        }

        /// <summary>Is the detected opponent an OTK deck?</summary>
        public bool IsOTKDeck()
        {
            return DetectedProfile?.PlayStyle == "OTK";
        }

        /// <summary>Is the detected opponent a midrange deck?</summary>
        public bool IsMidrangeDeck()
        {
            return DetectedProfile?.PlayStyle == "Midrange";
        }

        /// <summary>
        /// Should we play more conservatively against this opponent?
        /// True for control decks (lots of traps/interaction).
        /// </summary>
        public bool ShouldPlayConservative()
        {
            return IsControlDeck() && ArchetypeConfidence >= 0.5;
        }

        /// <summary>
        /// Should we rush combo fast against this opponent?
        /// True for OTK/combo decks (if we don't set up, we die).
        /// </summary>
        public bool ShouldRushSetup()
        {
            return (IsOTKDeck() || IsComboDeck()) && ArchetypeConfidence >= 0.5;
        }

        /// <summary>
        /// Get the estimated threat level from the opponent on their next turn (0-10).
        /// </summary>
        public double GetNextTurnThreatLevel(ClientField enemy, int turn)
        {
            if (enemy == null) return 5.0;

            double threat = 0.0;

            // Board presence
            int monsterCount = enemy.GetMonsterCount();
            int backrowCount = enemy.GetSpells().Count(c => c != null && c.IsFacedown());
            threat += monsterCount * 1.5;
            threat += backrowCount * 1.0;

            // High ATK monsters
            foreach (var m in enemy.GetMonsters())
            {
                if (m != null && m.IsFaceup() && m.Attack >= 3000)
                    threat += 1.5;
            }

            // Archetype-based adjustment
            if (IsOTKDeck())
                threat += 2.0;
            if (IsControlDeck())
                threat += 1.0;

            // Hand size factor
            int handCount = enemy.Hand.Count;
            threat += handCount * 0.3;

            return Math.Min(10.0, threat);
        }
    }
}
