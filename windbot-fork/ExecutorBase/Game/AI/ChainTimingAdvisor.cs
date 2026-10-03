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
        private readonly HashSet<int> _potAndBaitCards = new HashSet<int>();
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
                97268402,          // Effect Veiler
                59438930,          // Ghost Ogre
                73642296,          // Ghost Belle
                10045474,          // Infinite Impermanence
                42141493,          // Mulcharmy Fuwalos
                84192580,          // Mulcharmy Purulia
                27204311, 27204313,// Nibiru, the Primal Being
                24224830,          // Called by the Grave
                65681983,          // Crossout Designator
            });

            // Built-in Universal Meta & Legacy Chokepoints (Starters & Searchers)
            _comboStarters.UnionWith(new[] {
                // Ryzeal (Meta 2024-2026)
                8633261,  // Ice Ryzeal
                7511613,  // Ryzeal Duo Drive
                34022970, // Ext Ryzeal
                72238166, // Node Ryzeal
                35844557, // Sword Ryzeal
                61116514, // Palm Ryzeal
                34909328, // Ryzeal Detonator
                6798031,  // Ryzeal Cross
                60394026, // Ryzeal Plugin
                // Maliss (Meta 2024-2026)
                69272449, // Maliss <P> White Rabbit
                32061192, // Maliss <P> Dormouse
                96676583, // Maliss <P> Chessy Cat
                68337209, 68337210, // Maliss in Underground
                68059897, // Maliss <Q> Red Ransom
                21848500, // Maliss <Q> Hearts Crypter
                95454996, // Maliss <Q> White Binder
                // Yubel & Fiendsmith
                60764609, // Fiendsmith Engraver
                93729896, // Nightmare Throne
                80453041, // Phantom of Yubel
                62318994, // Samsara D Lotus
                // Tenpai Dragon
                39931513, // Tenpai Dragon Paidra
                91810826, // Tenpai Dragon Chundra
                66730191, // Sangen Kaimen
                // Voiceless Voice
                25801745, // Lo, the Prayers of the Voiceless Voice
                98477480, // Barrier of the Voiceless Voice
                // Centur-Ion
                15005145, // Centur-Ion Primera
                41371602, // Stand Up Centur-Ion!
                8841431,  // Centur-Ion Primera Primus
                // Orcust
                57835716, // Orcust Harp Horror
                30741503, // Galatea, the Orcust Automaton
                69811710, // Girsu, the Orcust Mekk-Knight
                703897,   // Orcust Crescendo
                // Branded / Despia
                44362883, // Branded Fusion
                62962630, // Aluber the Jester of Despia
                68468459, // Fallen of Albaz
                45883110, // Guiding Quem, the Virtuous
                // Snake-Eye / Fire King
                9674034, // Snake-Eye Ash
                90241276, // Snake-Eyes Poplar
                80845034, // WANTED: Seeker of Sinful Spoils
                89023486, // Original Sinful Spoils - Snake-Eye
                90681088,   // Legendary Fire King Ponix
                // ABC
                66399653, // Union Hangar
                77411244, // B-Buster Drake
                12524259, // Unauthorized Reactivation
                // Altergeist
                42790071, // Altergeist Multifaker
                25533642, // Altergeist Meluseek
                53143898, // Altergeist Marionetter
                // Dark Magician
                47222536, // Dark Magical Circle
                7084129, // Magician's Rod
                48680970, // Eternal Soul
                // Blue-Eyes
                8240199,  // Sage with Eyes of Blue
                88241506, // Maiden with Eyes of Blue
                48800175, // The Melody of Awakening Dragon
                // Labrynth
                1225009,  // Arianna the Labrynth Servant
                5380979,  // Welcome Labrynth
                92714517, // Big Welcome Labrynth
                // Spright / Runick / Kashtira / Tearlaments / Cyberse
                15443125, // Spright Starter
                76145933, // Spright Blue
                13533678, // Spright Jet
                31562086, // Runick Tip
                92107604, // Runick Fountain
                32909498, // Kashtira Fenrir
                68304193,  // Kashtira Unicorn
                71832012, // Pressured Planet Wraitsoth
                73956664, // Tearlaments Reinoheart
                77103950, // Primeval Planet Perlereino
                36521307, // Mathmech Circular
                57160136,  // Cynet Mining
                // Generic Searchers & Power Cards
                32807846, // Reinforcement of the Army
                73628505, // Terraforming
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
                71197066, // Gizmek Orochi, the Serpentron Sky Slasher
            });

            // Handtrap Budgeting: Pot & General Bait Cards (Strict Section 5.3)
            _potAndBaitCards.UnionWith(new[] {
                70368879, // Upstart Goblin
                93946239, // Into the Void
                74117290, // Dark World Dealings
                74519184, // Hand Destruction
                67616300, // Chicken Game
                84211599, // Pot of Prosperity
                35261759, // Pot of Desires
                49238328, // Pot of Extravagance
                49238328, // Pot of Extravagance (alt)
                49238329, // Pot of Extravagance (alt 2)
                98645731, // Pot of Duality
                55144522, // Pot of Greed
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
            int altCode = targetCard.GetNonAltartCode();

            // ── Section 5.3: Handtrap Budgeting & Anti-Bait Guard ──
            bool isPotOrBait = _potAndBaitCards.Contains(targetId) || _potAndBaitCards.Contains(altCode)
                || _lowValueTargets.Contains(targetId) || _deckSpecificLowValue.Contains(targetId);

            if (isPotOrBait)
            {
                // Strict Rule: If we only have 1 interactive card (e.g. 1 Ash in hand),
                // NEVER waste it on Pot of Extravagance / Duality / Upstart / Prosperity!
                // Reserve it for the true chokepoint (Branded Fusion, Normal Summon starter, etc.).
                if (ourInteractiveCount <= 1)
                {
                    return 15; // Threshold is 45 -> ALWAYS HOLD
                }
                // With 2+ handtraps in hand, allow interrupting Pot cards
                score = 48;
            }
            else if (_deckSpecificHighValue.Contains(targetId) || _comboStarters.Contains(targetId) ||
                CardIntelligence.IsHighThreatChokepoint(targetId) || CardIntelligence.IsHighThreatChokepoint(altCode)
                || CardIntelligence.IsKnownNegator(targetId) || CardIntelligence.IsKnownNegator(altCode))
            {
                score += 40; // Critical starter or negator — definitely negate (score 90)
            }
            else if (isChokepoint || CardIntelligence.IsFloodgate(targetId) || CardIntelligence.IsFloodgate(altCode))
            {
                score += 35; // Known chokepoint or floodgate from intelligence (score 85)
            }
            else if (_comboExtenders.Contains(targetId) || _comboExtenders.Contains(altCode))
            {
                score += 15; // Worth negating but not critical
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
                // [Core v0.094] Read the card text: search / SS from Deck-Extra-GY / draw = engine piece.
                // This is the generic signal that lets the bot Ash/Veiler unknown starters like a human would.
                try
                {
                    if (CardTextSemantics.IsEngineEffect(targetCard))
                        score += 15;
                }
                catch { }
            }

            // ── 2. Combo Stage (opponent summon count heuristic) ──
            // [Core v0.094] The first engine activation of a turn is usually THE starter — holding against it
            // (old -10 for every non-critical target) let unknown decks combo freely. Only non-engine,
            // low-value activations get a small early hesitation now.
            if (opponentSummonCount <= 1)
            {
                if (score < 60)
                    score -= 5;
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
                if (_handTrapIds.Contains(c.Id) || _handTrapIds.Contains(c.GetNonAltartCode())
                    || CardIntelligence.IsHandtrap(c.Id) || CardIntelligence.IsHandtrap(c.GetNonAltartCode()))
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

        /// <summary>
        /// Check if a card ID is a known bait or low-value card (e.g. Pot cards, Upstart).
        /// </summary>
        public bool IsBaitOrLowValue(int cardId)
        {
            return _potAndBaitCards.Contains(cardId) || _lowValueTargets.Contains(cardId) || _deckSpecificLowValue.Contains(cardId);
        }

        /// <summary>
        /// Check if a card ID is a known critical combo starter or searcher.
        /// </summary>
        public bool IsComboStarter(int cardId)
        {
            return _comboStarters.Contains(cardId) || _deckSpecificHighValue.Contains(cardId);
        }
    }
}
