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
    /// ChainTimingAdvisor — Smart chain response timing.
    /// Instead of "see chain → always chain", evaluates whether the current
    /// chain target is worth spending our limited interactive resources on.
    /// 
    /// All card data is loaded from external JSON configs (chain_targets.json)
    /// so it can be updated without recompiling when the meta changes.
    /// 
    /// Key concepts:
    ///   1. Target Value: Is the opponent's activating card a combo starter,
    ///      extender, or utility?  Higher value = more worth negating.
    ///   2. Combo Stage: Early (1-2 summons) vs Mid (3-4) vs Late (5+).
    ///      Early negate = prevent combo; Late negate = less impactful.
    ///   3. Resource Budget: How many interactive cards do we have?
    ///      If only 1 negate → save for the best target.
    ///   4. Chokepoint Awareness: Use existing ChokepointConfig to
    ///      auto-detect high-value negate targets.
    /// 
    /// Integrates into ModernExecutor — Legacy decks unaffected.
    /// </summary>
    public class ChainTimingAdvisor
    {
        // ═══════════════════════════════════════
        //  CONFIGURATION
        // ═══════════════════════════════════════

        /// <summary>Minimum chain value score (0-100) to approve a chain response.</summary>
        public int ChainThreshold { get; set; } = 45;

        /// <summary>If true, the advisor is active. Set false to disable and revert to always-chain.</summary>
        public bool Enabled { get; set; } = true;

        // ═══════════════════════════════════════
        //  DATA-DRIVEN TARGET CLASSIFICATION
        //  Loaded from JSON — no hardcoded card IDs
        // ═══════════════════════════════════════

        private readonly HashSet<int> _comboStarters = new HashSet<int>();
        private readonly HashSet<int> _comboExtenders = new HashSet<int>();
        private readonly HashSet<int> _lowValueTargets = new HashSet<int>();
        private readonly HashSet<int> _handTrapIds = new HashSet<int>();

        // Deck-specific overrides (registered at runtime by each executor)
        private readonly HashSet<int> _deckSpecificHighValue = new HashSet<int>();
        private readonly HashSet<int> _deckSpecificLowValue = new HashSet<int>();

        // ═══════════════════════════════════════
        //  JSON CONFIG MODEL
        // ═══════════════════════════════════════

        /// <summary>
        /// JSON schema for chain_targets.json
        /// </summary>
        public class ChainTargetsConfig
        {
            /// <summary>Card IDs that are critical combo starters — highest negate priority.</summary>
            public int[] ComboStarters { get; set; } = Array.Empty<int>();

            /// <summary>Card IDs that are combo extenders — worth negating but less critical.</summary>
            public int[] ComboExtenders { get; set; } = Array.Empty<int>();

            /// <summary>Card IDs that are low-value — don't waste negation on these.</summary>
            public int[] LowValueTargets { get; set; } = Array.Empty<int>();

            /// <summary>Card IDs that are hand traps / interactive cards.</summary>
            public int[] HandTraps { get; set; } = Array.Empty<int>();

            /// <summary>Minimum chain value score threshold (default 45).</summary>
            public int ChainThreshold { get; set; } = 45;
        }

        // ═══════════════════════════════════════
        //  CONSTRUCTOR & LOADING
        // ═══════════════════════════════════════

        public ChainTimingAdvisor()
        {
            // Populate minimal fallback hand traps (these are truly universal)
            _handTrapIds.UnionWith(new[] {
                14558127, 14558128, // Ash Blossom (both IDs)
                23434538,          // Maxx "C"
                94145021,          // Droll & Lock Bird
                63845230,          // Effect Veiler
                59438930,          // Ghost Ogre
                73642296,          // Ghost Belle
                10045474,          // Infinite Impermanence
                42141493,          // Mulcharmy Fuwalos
                84192580,          // Mulcharmy Purulia
                24224830,          // Called by the Grave
                65681983,          // Crossout Designator
            });

            // Built-in Universal Meta & Legacy Chokepoints (Starters & Searchers)
            _comboStarters.UnionWith(new[] {
                // Branded / Despia
                44362883, // Branded Fusion
                62962630, // Aluber the Jester of Despia
                73819701, // Fallen of Albaz
                45883110, // Guiding Quem, the Virtuous
                // Snake-Eye / Fire King
                27381364, // Snake-Eye Ash
                60953949, // Snake-Eyes Poplar
                68468459, // WANTED: Seeker of Sinful Spoils
                49868263, // Original Sinful Spoils - Snake-Eye
                368382,   // Legendary Fire King Ponix
                // Tenpai Dragon
                45533023, // Tenpai Dragon Paidra
                71983925, // Tenpai Dragon Chundra
                84749824, // Sangen Kaimen
                // ABC
                66399653, // Union Hangar
                77411244, // B-Buster Drake
                12524259, // Unauthorized Reactivation
                // Altergeist
                42790071, // Altergeist Multifaker
                53143898, // Altergeist Meluseek
                99111728, // Altergeist Marionetter
                // Dark Magician
                38033121, // Dark Magical Circle
                70781052, // Magician's Rod
                48680970, // Eternal Soul
                // Blue-Eyes
                8240199,  // Sage with Eyes of Blue
                71039903, // Maiden with Eyes of Blue
                99789342, // The Melody of Awakening Dragon
                // Labrynth
                1225009,  // Arianna the Labrynth Servant
                2347656,  // Welcome Labrynth
                78231355, // Big Welcome Labrynth
                // Spright / Runick / Kashtira / Tearlaments / Cyberse
                39477584, // Spright Starter
                54498517, // Spright Blue
                80036531, // Spright Jet
                15394972, // Runick Tip
                38009249, // Runick Fountain
                32909498, // Kashtira Fenrir
                8809344,  // Kashtira Unicorn
                68823957, // Pressured Planet Wraitsoth
                74063034, // Tearlaments Reinoheart
                42386471, // Primeval Planet Perlereino
                74064212, // Mathmech Circular
                5043010,  // Cynet Mining
                // Generic Searchers & Power Cards
                32807846, // Reinforcement of the Army
                73628505, // Terraforming
                84211599, // Pot of Prosperity
                35261759, // Pot of Desires
            });

            // Built-in Extenders
            _comboExtenders.UnionWith(new[] {
                70534340, // Lubellion the Searing Dragon
                87746184, // Albion the Branded Dragon
                41373230, // Titaniklad the Ash Dragon
                25451383, // Albion the Shrouded Dragon
                3405259,  // C-Crush Wyvern
                30012506, // A-Assault Core
                43147039, // Photon Vanisher
            });

            // Built-in Low-Value / Bait Targets (Avoid wasting high-impact negates)
            _lowValueTargets.UnionWith(new[] {
                70368879, // Upstart Goblin
                93946239, // Into the Void
                74117290, // Dark World Dealings
                74519184, // Hand Destruction
                43218406, // Gizmek Orochi
            });
        }

        /// <summary>
        /// Load target classification data from a JSON file.
        /// Call from executor initialization. If file doesn't exist, uses empty defaults
        /// (the system still works via deck-specific registration + heuristics).
        /// </summary>
        public void LoadConfig(string configPath)
        {
            try
            {
                if (!File.Exists(configPath))
                {
                    // Try common search paths
                    string[] searchPaths = {
                        Path.Combine(AppContext.BaseDirectory, "models", "chain_targets.json"),
                        Path.Combine(AppContext.BaseDirectory, "chain_targets.json"),
                        "models/chain_targets.json",
                        "chain_targets.json"
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
                    var config = JsonSerializer.Deserialize<ChainTargetsConfig>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (config != null)
                    {
                        if (config.ComboStarters != null)
                            _comboStarters.UnionWith(config.ComboStarters);
                        if (config.ComboExtenders != null)
                            _comboExtenders.UnionWith(config.ComboExtenders);
                        if (config.LowValueTargets != null)
                            _lowValueTargets.UnionWith(config.LowValueTargets);
                        if (config.HandTraps != null)
                            _handTrapIds.UnionWith(config.HandTraps);
                        if (config.ChainThreshold > 0)
                            ChainThreshold = config.ChainThreshold;

                        System.Diagnostics.Debug.WriteLine($"[ChainTimingAdvisor] Loaded config: {_comboStarters.Count} starters, " +
                            $"{_comboExtenders.Count} extenders, {_lowValueTargets.Count} low-value, " +
                            $"{_handTrapIds.Count} hand traps from {configPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ChainTimingAdvisor] Config load failed: {ex.Message} — using defaults");
            }
        }

        // ═══════════════════════════════════════
        //  DECK-SPECIFIC REGISTRATION
        //  (Runtime — called by each deck executor)
        // ═══════════════════════════════════════

        /// <summary>
        /// Register a card as high-value negate target (deck-specific).
        /// These override the JSON config — useful for matchup-specific knowledge.
        /// </summary>
        public void RegisterHighValueTarget(int cardId)
        {
            _deckSpecificHighValue.Add(cardId);
        }

        /// <summary>Register multiple high-value targets at once.</summary>
        public void RegisterHighValueTargets(params int[] cardIds)
        {
            foreach (int id in cardIds)
                _deckSpecificHighValue.Add(id);
        }

        /// <summary>Register a card as low-value target (not worth negating).</summary>
        public void RegisterLowValueTarget(int cardId)
        {
            _deckSpecificLowValue.Add(cardId);
        }

        // ═══════════════════════════════════════
        //  CORE EVALUATION
        // ═══════════════════════════════════════

        /// <summary>
        /// Evaluate whether we should chain our interactive card to the current chain.
        /// Returns a score from 0-100. Higher = more worth chaining.
        /// 
        /// Parameters:
        ///   ourCard: The card we'd use to respond (Ash, Veiler, Imperm, etc.)
        ///   targetCard: The opponent's card that's activating (last chain card)
        ///   opponentHandCount: Number of cards in opponent's hand
        ///   opponentSummonCount: How many monsters opponent summoned this turn
        ///   ourInteractiveCount: How many interactive cards we have left
        ///   isChokepoint: Whether the target is in the ChokepointConfig
        /// </summary>
        public int EvaluateChainValue(
            ClientCard ourCard,
            ClientCard targetCard,
            int opponentHandCount,
            int opponentSummonCount,
            int ourInteractiveCount,
            bool isChokepoint = false)
        {
            if (!Enabled || targetCard == null || ourCard == null)
                return 100; // Disabled or no info → always chain (safe fallback)

            int score = 50; // Neutral baseline

            int targetId = targetCard.Id;

            if (_deckSpecificHighValue.Contains(targetId) || _comboStarters.Contains(targetId) ||
                CardIntelligence.IsHighThreatChokepoint(targetId) || CardIntelligence.IsKnownNegator(targetId))
            {
                score += 35; // Critical target — definitely negate
            }
            else if (isChokepoint || CardIntelligence.IsFloodgate(targetId))
            {
                score += 30; // Known chokepoint or floodgate from intelligence
            }
            else if (_comboExtenders.Contains(targetId))
            {
                score += 15; // Worth negating but not critical
            }
            else if (_deckSpecificLowValue.Contains(targetId) || _lowValueTargets.Contains(targetId))
            {
                score -= 30; // Not worth spending resources on
            }
            else
            {
                // Unknown card — use type-based heuristics
                // (This is the fallback that works even without JSON config)
                if (targetCard.HasType(CardType.Spell) && !targetCard.HasType(CardType.QuickPlay)
                    && !targetCard.HasType(CardType.Continuous) && !targetCard.HasType(CardType.Field))
                {
                    // Normal spell from hand = likely important (combo starter)
                    score += 10;
                }
                if (targetCard.IsExtraCard())
                {
                    // Extra Deck monster effect = likely important
                    score += 10;
                }
                // High-level monsters activating on field = likely boss effect
                if (targetCard.Level >= 7 && targetCard.Location == CardLocation.MonsterZone)
                {
                    score += 8;
                }
            }

            // ── 2. Combo Stage (opponent summon count heuristic) ──
            if (opponentSummonCount <= 1)
            {
                // Early combo — might have follow-ups. But starters are still critical.
                if (score < 70)
                    score -= 10;
            }
            else if (opponentSummonCount >= 2 && opponentSummonCount <= 3)
            {
                // Mid combo — good time to negate key pieces
                score += 5;
            }
            else if (opponentSummonCount >= 4)
            {
                // Late combo — less impact (board mostly built)
                score -= 5;
            }

            // ── 3. Resource Budget ──
            if (ourInteractiveCount <= 1)
            {
                // Last interactive card — only use on high-value targets
                if (score < 65)
                    score -= 15;
            }
            else if (ourInteractiveCount >= 3)
            {
                // Plenty of interaction — be more aggressive
                score += 5;
            }

            // ── 4. Opponent Hand Size Context ──
            if (opponentHandCount <= 2 && opponentSummonCount == 0)
            {
                // Low hand + no summons = this might be their only play → negate it
                score += 10;
            }

            return Math.Max(0, Math.Min(100, score));
        }

        /// <summary>
        /// Quick decision: should we hold our response and NOT chain?
        /// Returns true if the chain value is below threshold.
        /// </summary>
        public bool ShouldHoldResponse(
            ClientCard ourCard,
            ClientCard targetCard,
            int opponentHandCount,
            int opponentSummonCount,
            int ourInteractiveCount,
            bool isChokepoint = false)
        {
            int value = EvaluateChainValue(ourCard, targetCard,
                opponentHandCount, opponentSummonCount,
                ourInteractiveCount, isChokepoint);
            return value < ChainThreshold;
        }

        /// <summary>
        /// Decide whether to hold response, integrating the OpponentProfiler.
        /// </summary>
        public bool ShouldHoldResponseWithProfile(
            ClientCard ourCard,
            ClientCard targetCard,
            OpponentProfiler profile,
            int opponentHandCount,
            int opponentSummonCount,
            int ourInteractiveCount,
            bool isChokepoint = false)
        {
            bool baseHold = ShouldHoldResponse(ourCard, targetCard, opponentHandCount,
                opponentSummonCount, ourInteractiveCount, isChokepoint);

            if (profile == null) return baseHold;

            // If control deck → keep interactions longer (be conservative)
            if (profile.ShouldPlayConservative() && ourInteractiveCount <= 1)
                return true;

            // If OTK/Combo deck → rush setup, don't hold unnecessarily
            if (profile.ShouldRushSetup() && !baseHold)
                return false;

            return baseHold;
        }

        /// <summary>
        /// Count our available interactive cards (hand traps + face-down negates).
        /// </summary>
        public int CountInteractiveCards(ClientField bot)
        {
            if (bot == null) return 0;
            int count = 0;

            foreach (var c in bot.Hand)
            {
                if (c == null) continue;
                if (_handTrapIds.Contains(c.Id))
                    count++;
            }

            // Face-down backrow = potential negates
            count += bot.GetSpells().Count(c => c != null && c.IsFacedown());

            return count;
        }

        /// <summary>
        /// Check if a card ID is a known hand trap.
        /// </summary>
        public bool IsHandTrap(int cardId)
        {
            return _handTrapIds.Contains(cardId);
        }
    }
}
