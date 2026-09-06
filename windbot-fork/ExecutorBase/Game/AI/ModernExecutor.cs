using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot.Game.AI.DecisionEngine;

namespace WindBot.Game.AI
{
    // ═══════════════════════════════════════════════════════════════
    //  Action Priority Classification — Smart Flow v2
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Classifies how urgent/important an action is for phase planning.
    /// </summary>
    public enum ActionPriority
    {
        /// <summary>ATK boost, removal, must do before battle.</summary>
        CombatEssential,
        /// <summary>Normal summon, key combo piece — always MP1.</summary>
        ComboStarter,
        /// <summary>SS from hand/GY, Link/Xyz summon — MP1 if board needs work.</summary>
        ComboExtender,
        /// <summary>Search, draw, setup spells — can defer to MP2 if attack available.</summary>
        ResourceGain,
        /// <summary>Backrow set, field spell — should defer to MP2 when possible.</summary>
        Deferrable
    }

    /// <summary>
    /// Base executor for 2026+ decks. Legacy decks should keep inheriting DefaultExecutor.
    /// Adds conservative Analysis-based fallbacks without changing old deck behavior.
    /// 
    /// ═══ FieldGuard Shared Utility Layer ═══
    /// Centralizes common game-awareness logic used by all 2026 Executors:
    ///   - IsSpecialSummonBlocked()   : Floodgate detection (Winda, Fossil Dyna, etc.)
    ///   - OpponentHasActiveNegator() : Negate monster detection (Baronne, Apollousa, etc.)
    ///   - IsTargetImmune()           : Card immunity detection (Chaos MAX, Eternal Soul, etc.)
    ///   - NeedsBoardPresence()       : Do we need to build board?
    ///   - IsInGrindGame()            : Is the game in grind phase?
    ///   - CountDisruptions()         : How many disruptions do we have?
    ///
    /// ═══ Smart Flow v2 Layer ═══
    /// Adds intelligent action pacing and MP1/MP2 awareness:
    ///   - DynamicLethalCheck()        : Re-evaluate lethal mid-turn
    ///   - ShouldAttackBeforeCombo()   : Attack first when opponent board is open
    ///   - ClassifyAction()            : Categorize actions by priority
    ///   - ShouldStopExtending()       : Stop combo when board is sufficient
    ///
    /// All methods are protected virtual — deck-specific executors can override as needed.
    /// Legacy executors (inheriting DefaultExecutor) are completely unaffected.
    /// </summary>
    public abstract class ModernExecutor : DefaultExecutor
    {
        protected bool _isGoingSecond
        {
            get => IsGoingSecond;
            set
            {
                if (!IsMatchDesignationSet)
                {
                    IsGoingSecond = value;
                    IsGoingFirst = !value;
                    IsMatchDesignationSet = true;
                }
            }
        }
        protected ClientCard LastChainCard => Util.GetLastChainCard();

        private bool _comboStepNegatedThisChain = false;
        private int _negatedComboStepCardId = -1;

        // ═══════════════════════════════════════════════════════════════
        //  AI ENHANCEMENT MODULES (2026+)
        //  All opt-in — deck executors register combo lines/bait cards.
        //  Without registration, behavior falls back to existing logic.
        // ═══════════════════════════════════════════════════════════════

        /// <summary>Hand-aware combo line selection engine.</summary>
        protected ComboRouter ComboRouter { get; private set; }

        /// <summary>Hand trap bait system — plays expendable cards before combo starters.</summary>
        protected BaitPlanner BaitPlanner { get; private set; }

        /// <summary>Smart chain response timing — evaluates when to use hand traps.</summary>
        protected ChainTimingAdvisor ChainAdvisor { get; private set; }

        /// <summary>Opponent deck/hand inference engine.</summary>
        public OpponentProfiler OpponentProfile { get; private set; }

        /// <summary>Anti-overextension and Nibiru awareness.</summary>
        protected ResourcePlanner ResourcePlan { get; private set; }

        /// <summary>Decoupled Decision Engine context container.</summary>
        protected DecisionContext AIContext { get; private set; }

        // ═══════════════════════════════════════════════════════════════
        //  FLOODGATE IDs — monsters that block Special Summoning
        //  Compiled superset from all 2026 executor implementations
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> _spSummonBlockMonsters = new HashSet<int>
        {
            // --- Commonly seen across all executors ---
            42009023,  // Fossil Dyna Pachycephalo
            7902349,   // Jowgen the Spiritualist
            15397015,  // Inspect Boarder
            19261966,  // El Shaddoll Winda (1 SS per turn)
            78193831,  // Vanity's Fiend
            67922702,  // Archlord Kristya
            96015934,  // Vanity's Ruler
            14212200,  // Amano-Iwato
            86325573,  // Barrier Statue (multiple, check by effect)
            3717252,   // Koa'ki Meiru Drago
            85359414,  // Number 41: Bagooska (face-down floodgate mode)

            // --- Extended from Dreadnought/Branded/other variants ---
            42009836,  // Fossil Dyna (alt ID variant)
            47084486,  // Majesty's Fiend
            72634965,  // Denko Sekka (own-only check needed)
            59509952,  // Lose 1 Turn (monster)
            94977269,  // Naturia Exterio

            // --- Barrier Statues (full set) ---
            10963799,  // Barrier Statue of the Stormwinds
            19740112,  // Barrier Statue of the Drought
            47961808,  // Barrier Statue of the Inferno
            73356503,  // Barrier Statue of the Abyss
            84478195,  // Barrier Statue of the Torrent
        };

        private static readonly HashSet<int> _spSummonBlockSpells = new HashSet<int>
        {
            5851097,   // Vanity's Emptiness
            4514109,   // Kaiser Colosseum
            22046459,  // Rivalry of Warlords
            34487429,  // Gozen Match
            2429943,   // There Can Be Only One
            3188710,   // Summon Breaker
            81674782,  // Dimensional Fissure (indirect block in some combos)
            47355498,  // Summon Limit
            83326048,  // Dimensional Barrier (when active)
            92746535,  // Summon Limit (alt)
            82732047,  // Skill Drain (blocks effects used to SS)
            30241314,  // Macro Cosmos
            53334641,  // Gozen Match (alt)
            90845713,  // Rivalry of Warlords (alt)
        };

        // ═══════════════════════════════════════════════════════════════
        //  NEGATE MONSTER IDs — monsters that can negate activations
        // ═══════════════════════════════════════════════════════════════
        private static readonly HashSet<int> _negateMonsters = new HashSet<int>
        {
            84815190,  // Baronne de Fleur (negate any 1 activation)
            4280258,   // Apollousa, Bow of the Goddess (negate monster effect; ATK check)
            50954680,  // Crystal Wing Synchro Dragon (negate Lv5+ monster effects)
            10443957,  // Cyber Dragon Infinity (absorb + negate)
            86066372,  // Herald of Ultimateness / Perfection
            31801517,  // Evolzar Dolkka (negate monster effects)
            57793869,  // Borreload Savage Dragon (negate any 1 activation)
            17330115,  // Hot Red Dragon Archfiend Abyss (negate S/T)
            21522601,  // Witchcrafter Madame Verre (negate monster effects)
            84523092,  // Witchcrafter Haine (can destroy + immune)
            1508649,   // Altergeist Hexstia (negate S/T activation)
            1561110,   // ABC-Dragon Buster (banish from field)
            63767246,  // Number 38: Hope Harbinger Dragon Titanic Galaxy (negate Spell)
        };

        // ═══════════════════════════════════════════════════════════════
        //  WELL-KNOWN IMMUNITY CARDS
        // ═══════════════════════════════════════════════════════════════
        private const int BlueEyesChaosMAX = 55410871;
        private const int AzureEyesSilverDragon = 40908371;
        private const int EternalSoul = 48680970;
        private const int DarkMagician = 46986414;
        private const int DarkMagicianTheDragonKnight = 41721210;

        protected ModernExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Initialize enhancement modules
            ComboRouter = new ComboRouter();
            BaitPlanner = new BaitPlanner();
            ChainAdvisor = new ChainTimingAdvisor();
            OpponentProfile = new OpponentProfiler();
            ResourcePlan = new ResourcePlanner();
            AIContext = new DecisionContext(this, OpponentProfile);

            // Load external JSON configs (data-driven — edit JSON to update, no recompile)
            try
            {
                string configDir = FindConfigDirectory();
                if (configDir != null)
                {
                    ChainAdvisor.LoadConfig(Path.Combine(configDir, "chain_targets.json"));
                    OpponentProfile.LoadConfig(Path.Combine(configDir, "archetypes.json"));
                }
            }
            catch { /* Config loading is optional — fallback to heuristics */ }

            // ── Board-Completion Pass-Turn System (all 2026 executors) ──
            // When ShouldPassTurn() returns true, the bot immediately goes to EndPhase
            // instead of activating wasteful effects after the board is complete.
            AddExecutor(ExecutorType.GoToEndPhase, ShouldPassTurn);
        }

        /// <summary>
        /// Search for the configs directory across common project paths.
        /// </summary>
        private static string FindConfigDirectory()
        {
            string[] searchDirs = {
                AppContext.BaseDirectory,
                Path.Combine(AppContext.BaseDirectory, ".."),
                Path.Combine(AppContext.BaseDirectory, "..", ".."),
            };

            foreach (string baseDir in searchDirs)
            {
                string configDir = Path.Combine(baseDir, "configs");
                if (Directory.Exists(configDir)) return configDir;

                // Also check for YGO_AI_PLATFORM/configs
                string altDir = Path.Combine(baseDir, "YGO_AI_PLATFORM", "configs");
                if (Directory.Exists(altDir)) return altDir;
            }

            return null;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 1. IsSpecialSummonBlocked — is Special Summoning blocked?
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Returns true if a floodgate on the field blocks Special Summoning.
        /// Checks both monster and spell/trap floodgates across both fields.
        /// Override in deck executors that have additional checks (e.g., Pot of Duality lock).
        /// </summary>
        protected virtual bool IsSpecialSummonBlocked()
        {
            if (CardIntelligence.IsSpecialSummonBlocked(Enemy, Bot)) return true;

            // Check monster floodgates
            var allMonsters = Enemy.GetMonsters().Concat(Bot.GetMonsters())
                .Where(c => c != null && c.IsFaceup() && !c.IsDisabled());

            foreach (var card in allMonsters)
            {
                if (_spSummonBlockMonsters.Contains(card.Id) || CardIntelligence.IsFloodgateMonster(card.Id))
                {
                    // Denko Sekka only blocks the opponent — skip if it's ours
                    if (card.Id == 72634965 && card.Controller == 0) continue;
                    return true;
                }
            }

            // Check spell/trap floodgates
            var allSpells = Enemy.GetSpells().Concat(Bot.GetSpells())
                .Where(c => c != null && c.IsFaceup() && !c.IsDisabled());

            foreach (var card in allSpells)
            {
                if (_spSummonBlockSpells.Contains(card.Id) || CardIntelligence.IsFloodgateSpellTrap(card.Id))
                    return true;
            }

            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 2. OpponentHasActiveNegator — does enemy have negate?
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Returns true if the opponent controls an active negate monster.
        /// Pass ourCard to check context-specific negation (e.g., Crystal Wing vs Lv5+).
        /// </summary>
        protected virtual bool OpponentHasActiveNegator(ClientCard ourCard = null)
        {
            foreach (var c in Enemy.GetMonsters())
            {
                if (c == null || !c.IsFaceup() || c.IsDisabled()) continue;

                // Crystal Wing: only negates Level 5+ monster effects on field
                if (c.Id == 50954680)
                {
                    if (ourCard != null && ourCard.Location == CardLocation.MonsterZone
                        && ourCard.Level >= 5 && ourCard.IsMonster())
                        return true;
                    continue; // Skip if ourCard doesn't match
                }

                // Apollousa: only negates monster effects; needs ATK >= 800
                if (c.Id == 4280258)
                {
                    if (c.Attack >= 800 && (ourCard == null || ourCard.IsMonster()))
                        return true;
                    continue;
                }

                // All others: generic negate
                if (_negateMonsters.Contains(c.Id) || CardIntelligence.IsKnownNegator(c.Id))
                    return true;
            }

            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 3. Negate Intelligence — should we negate this card?
        //  Adapted from ABCExecutor's battle-tested blacklists.
        //  All 2026+ executors automatically get smart negate decisions.
        // ═══════════════════════════════════════════════════════════════

        /// <summary>Cards that should NEVER be negated — they benefit the opponent or are bait.</summary>
        private static readonly HashSet<int> _negateNever = new HashSet<int>
        {
            70368879,  // Upstart Goblin
            93946239,  // Into the Void
            74117290,  // Dark World Dealings
            74519184,  // Hand Destruction
            73628505,  // Terraforming
            32807846,  // Reinforcement of the Army
            64734921,  // The Agent of Creation - Venus
            34408491,  // Beelze of the Diabolic Dragons
            8763963,   // Beelzeus of the Diabolic Dragons
            74586817,  // PSY-Framelord Omega
            55010259,  // Gold Gadget
            29021114,  // Silver Gadget
            57774843,  // Judgment Dragon
            423585,    // Summoner Monk
            43218406,  // Water Gizmek
            30208479,  // Macro Cosmos
        };

        /// <summary>Equip/Union effects from hand — cost already paid, negate is wasteful.</summary>
        private static readonly HashSet<int> _negateNeverHandEquip = new HashSet<int>
        {
            93969023,  // Black Metal Dragon
            38210374,  // Explossum
            38601126,  // Robot Buster Destruction Sword
            2602411,   // Wizard Buster Destruction Sword
            76218313,  // Dragon Buster Destruction Sword
            94573223,  // Inzektor Giga-Mantis
            21977828,  // Inzektor Giga-Weevil
            89132148,  // Photon Orbital
        };

        /// <summary>
        /// Central negate decision engine. Returns true if this card's effect should be negated.
        /// Override per-deck for archetype-specific logic.
        /// </summary>
        protected virtual bool ShouldNegateTarget(ClientCard lastChainCard)
        {
            if (lastChainCard == null) return true;

            // Rule 1: Never negate cards on the blacklist
            if (_negateNever.Contains(lastChainCard.Id))
                return false;

            // Rule 2: Never negate Danger! hand effects (they'll activate again immediately)
            if (lastChainCard.HasSetcode(0x11e) && lastChainCard.Location == CardLocation.Hand)
                return false;

            // Rule 3: Never negate Superheavy Samurai Soul from hand/field in Main Phase
            if (lastChainCard.HasSetcode(0x109a)
                && (lastChainCard.Location == CardLocation.Hand || lastChainCard.Location == CardLocation.MonsterZone)
                && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                return false;

            // Rule 4: Never negate equip effects from hand
            if (_negateNeverHandEquip.Contains(lastChainCard.Id)
                && lastChainCard.Location == CardLocation.Hand)
                return false;

            // Rule 5: Never negate Judgment Dragon mill in Main Phase
            if (lastChainCard.IsCode(57774843)
                && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                return false;

            // Rule 6: Never negate Summoner Monk's discard-to-summon (cost is paid)
            if (lastChainCard.IsCode(423585)
                && ActivateDescription == Util.GetStringId(423585, 0))
                return false;

            // Default: negate it
            return true;
        }

        /// <summary>
        /// Smart Ash Blossom — uses negate intelligence instead of blind negation.
        /// Hides DefaultExecutor.DefaultAshBlossomAndJoyousSpring for 2026+ decks.
        /// </summary>
        protected new bool DefaultAshBlossomAndJoyousSpring()
        {
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard ash = Bot.Hand.FirstOrDefault(c => c.Id == 14558127 || c.Id == 14558128);
            if (ash == null) return false;
            return AIContext != null && AIContext.ShouldActivate(ash, Util.GetLastChainCard(), "AshBlossom");
        }

        /// <summary>
        /// Smart Effect Veiler — won't waste Veiler on Galaxy Soldier when opponent has 3+ cards.
        /// </summary>
        protected new bool DefaultEffectVeiler()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (Duel.CurrentChain != null && Duel.CurrentChain.Any(c => c != null && c.Controller == 0 && (c.IsCode(10045474) || c.IsCode(97268402) || c.IsCode(24224830) || CardIntelligence.IsKnownNegator(c.Id))))
                return false;

            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain == null || lastChain.Controller != 1 || lastChain.Location != CardLocation.MonsterZone) return false;
            if (lastChain.IsDisabled() || lastChain.IsShouldNotBeTarget() || lastChain.IsShouldNotBeMonsterTarget()) return false;

            ClientCard veiler = Bot.Hand.FirstOrDefault(c => c != null && (c.Id == 97268402 || c.Id == 63845230));
            if (veiler == null) return false;
            return AIContext != null && AIContext.ShouldActivate(veiler, lastChain, "EffectVeiler");
        }

        /// <summary>
        /// Boss negate intelligence (Apollousa, Infinity, Baronne style).
        /// Includes extensive blacklists for cards that shouldn't be negated.
        /// </summary>
        protected virtual bool DefaultBossNegate()
        {
            if (Duel.LastChainPlayer != 1) return false;

            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain == null) return false;

            // Find our active boss monster that triggered this negate check
            ClientCard boss = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() && 
                (_negateMonsters.Contains(m.Id)));
            if (boss == null) return false;

            return AIContext != null && AIContext.ShouldActivate(boss, lastChain, "BossNegate");
        }

        /// <summary>
        /// Shortcut: true if enemy has any known active negate monster (no context filtering).
        /// </summary>
        protected bool EnemyHasKnownNegate()
        {
            return OpponentHasActiveNegator(null);
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 3. IsTargetImmune — is this card immune to targeting/effects?
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Returns true if the given enemy card is immune to targeting or card effects.
        /// Covers: Chaos MAX, Dragoon, Avramax, The Arrival, Ultimate Falcon, Underworld Goddess,
        ///         Azure-Eyes (Dragon protection), Eternal Soul (DM/Dragon Knight unaffected).
        /// </summary>
        protected virtual bool IsTargetImmune(ClientCard card)
        {
            if (card == null) return false;
            if (CardIntelligence.IsTargetImmune(card)) return true;
            if (card.IsShouldNotBeTarget()) return true;

            // Untargetable boss monsters
            if (card.IsCode(BlueEyesChaosMAX)) return true;
            if (card.IsCode(37818794)) return true; // Red-Eyes Dark Dragoon
            if (card.IsCode(21887175)) return true; // Mekk-Knight Crusadia Avramax
            if (card.IsCode(11738489)) return true; // The Arrival Cyberse @Ignister
            if (card.IsCode(86221741)) return true; // Raidraptor - Ultimate Falcon
            if (card.IsCode(98127546)) return true; // Underworld Goddess of the Closed World

            // Azure-Eyes Silver Dragon — Dragon monsters are immune to targeting
            if (card.HasRace(CardRace.Dragon) &&
                Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(AzureEyesSilverDragon)))
                return true;

            // Eternal Soul — Dark Magician and DM Dragon Knight are unaffected
            bool isEternalSoulActive = Enemy.GetSpells().Any(s =>
                s != null && s.IsFaceup() && s.IsCode(EternalSoul) && !s.IsDisabled());
            if (isEternalSoulActive &&
                (card.IsCode(DarkMagician) || card.IsCode(DarkMagicianTheDragonKnight)))
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if the enemy card is immune to destruction by card effects.
        /// </summary>
        protected virtual bool IsDestructionImmune(ClientCard card)
        {
            if (card == null) return false;
            if (card.IsCode(37818794)) return true; // Red-Eyes Dark Dragoon
            if (card.IsCode(BlueEyesChaosMAX)) return true; // Chaos MAX
            if (card.IsCode(11738489)) return true; // The Arrival
            if (card.IsCode(86221741)) return true; // Ultimate Falcon

            bool isEternalSoulActive = Enemy.GetSpells().Any(s =>
                s != null && s.IsFaceup() && s.IsCode(EternalSoul) && !s.IsDisabled());
            if (isEternalSoulActive &&
                (card.IsCode(DarkMagician) || card.IsCode(DarkMagicianTheDragonKnight)))
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if the card is a valid effect target (targetable AND not immune).
        /// </summary>
        protected virtual bool IsViableEffectTarget(ClientCard card)
        {
            if (card == null) return false;
            if (IsTargetImmune(card)) return false;

            // Dark Magician the Dragon Knight protects S/T from targeting
            bool hasDragonKnight = Enemy.GetMonsters().Any(m =>
                m != null && m.IsFaceup() && m.IsCode(DarkMagicianTheDragonKnight) && !m.IsDisabled());
            if (hasDragonKnight && (card.IsSpell() || card.IsTrap()))
                return false;

            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 4. Board State Assessment
        // ═══════════════════════════════════════════════════════════════

        // ═══════════════════════════════════════════════════════════════
        //  § 4c. Board-Completion Pass-Turn System
        //  Detects when the bot has finished setting up its board and
        //  should pass turn instead of activating wasteful effects.
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Should the bot pass turn immediately because board setup is complete?
        /// Returns true when further actions would be wasteful and the board is ready.
        /// 
        /// Registered as GoToEndPhase executor in the constructor.
        /// When true, GameAI immediately goes to EndPhase, skipping all remaining actions.
        /// 
        /// Currently handles:
        ///   - Turn 1 MP1 (no battle available): passes after combo is done and board is strong
        ///   - Override in deck executors for archetype-specific completion conditions
        /// 
        /// DOES NOT handle MP2 (uses ShouldAllowActivate guard for that).
        /// DOES NOT interrupt battle phase sequencing.
        /// </summary>
        protected virtual bool ShouldPassTurn()
        {
            // Only on our turn
            if (Duel.Player != 0) return false;
            if (Main == null) return false;

            // ── MP1: No battle available (Turn 1, can't attack) ──
            if (Duel.Phase == DuelPhase.Main1 && !Main.CanBattlePhase)
            {
                // Still need to set up — don't pass if SS is blocked
                if (IsSpecialSummonBlocked()) return false;

                // Board isn't strong enough — keep playing
                if (!IsBoardStrongEnough()) return false;

                // Check for remaining meaningful plays: combo extenders, summons, AND setting backrow.
                // Setting backrow (traps, field spells) is a critical part of end-board setup on Turn 1.
                bool hasRemainingCombo = Main.ActivableCards.Any(c => c != null &&
                    ClassifyAction(c, ExecutorType.Activate) <= ActionPriority.ComboExtender)
                    || Main.SummonableCards.Count > 0
                    || Main.SpecialSummonableCards.Count > 0
                    || Main.SpellSetableCards.Count > 0
                    || Main.MonsterSetableCards.Count > 0;

                if (hasRemainingCombo) return false;

                try
                {
                    AI?.Log(LogLevel.Info,
                        $"[PASS-TURN] ✓ Board complete (Turn {Duel.Turn}) — passing with {CountDisruptions()} disruptions");
                }
                catch { }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if we need to build board presence (empty field or outnumbered).
        /// </summary>
        protected virtual bool NeedsBoardPresence()
        {
            int ourFaceup = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());
            return ourFaceup == 0 || (ourFaceup <= 1 && Enemy.GetMonsterCount() >= 2);
        }

        /// <summary>
        /// Returns true if the game has entered grind phase (late game, low resources, or high opponent pressure).
        /// </summary>
        protected virtual bool IsInGrindGame()
        {
            int handCount = Bot.Hand.Count;
            int monsterCount = Bot.GetMonsterCount();
            int setCount = Bot.GetSpells().Count(c => c != null && c.IsFacedown());

            bool lowResources = (handCount <= 1 && monsterCount <= 1 && setCount <= 1);
            bool highOpponentPressure = Enemy.GetMonsterCount() >= 3;

            return Duel.Turn >= 6 || lowResources || highOpponentPressure;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 4b. Enhanced Board Assessment (Resource-Aware)
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Check if we have a negate monster face-up on field.
        /// </summary>
        protected virtual bool HasNegateOnField()
        {
            foreach (var m in Bot.GetMonsters())
            {
                if (m == null || !m.IsFaceup() || m.IsDisabled()) continue;
                // Check common negate monster IDs from _negateMonsters set
                if (_negateMonsters.Contains(m.Id)) return true;
            }
            return false;
        }

        /// <summary>
        /// Count the number of disruptions we currently have (field + hand).
        /// Uses static boss/negate ID list + hand trap detection.
        /// </summary>
        protected virtual int CountDisruptions()
        {
            int count = 0;

            // Boss monsters on field that provide disruption
            foreach (var m in Bot.GetMonsters())
            {
                if (m == null || !m.IsFaceup() || m.IsDisabled()) continue;
                if (_negateMonsters.Contains(m.Id)) count += 2;
                else if (m.Attack >= 2500 && m.IsExtraCard()) count++;
            }

            // Set backrow (each face-down S/T = potential disruption)
            count += Bot.GetSpells().Count(c => c != null && c.IsFacedown());

            // Hand traps in hand
            foreach (var c in Bot.Hand)
            {
                if (c == null) continue;
                switch (c.Id)
                {
                    case 14558127:  // Ash Blossom
                    case 23434538:  // Maxx "C"
                    case 94145021:  // Droll & Lock Bird
                    case 97268402:  // Effect Veiler
                    case 63845230:  // Eater of Millions
                    case 59438930:  // Ghost Ogre
                    case 73642296:  // Ghost Belle
                    case 10045474:  // Infinite Impermanence
                    case 42141493:  // Mulcharmy Fuwalos
                    case 84192580:  // Mulcharmy Purulia
                        count++;
                        break;
                }
            }

            return count;
        }

        /// <summary>
        /// Returns true if our board is strong enough (3+ disruptions total).
        /// Useful for deciding whether to extend combo or stop.
        /// </summary>
        protected virtual bool IsBoardStrongEnough()
        {
            return CountDisruptions() >= 3;
        }

        /// <summary>
        /// Compute the current board advantage score using the Scorer.
        /// Positive indicates we have the advantage.
        /// </summary>
        protected virtual int BoardScore()
        {
            if (Scorer != null)
                return Scorer.BoardAdvantageScore();
            return 0;
        }

        public override int OnSelectOption(IList<long> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 20;
                long optIndex = options[i] & 0xfffff;

                if (cardId == 0 && LastChainCard != null)
                {
                    cardId = LastChainCard.Id;
                }

                // 1. Blackwing - Simoon the Poison Wind (81470373)
                // Option index 1 is Normal Summon (Option index 0 is send to GY).
                if (cardId == 81470373)
                {
                    if (optIndex == 1) return i;
                }

                // 2. Pot of Prosperity (84211599)
                // Option index 1 is banish 6 cards (Option index 0 is banish 3).
                if (cardId == 84211599)
                {
                    if (optIndex == 1) return i;
                }

                // 3. Triple Tactics Talent (25311006)
                if (cardId == 25311006)
                {
                    // Draw 2 (optIndex 0) is great if we have 3 or fewer cards in hand
                    if (optIndex == 0 && Bot.Hand.Count <= 4)
                    {
                        return i;
                    }
                    // Take control (optIndex 1) is great if going second and enemy has a monster
                    if (optIndex == 1 && _isGoingSecond && Enemy.GetMonsterCount() > 0)
                    {
                        return i;
                    }
                    // Hand rip (optIndex 2) is great if going first
                    if (optIndex == 2 && !_isGoingSecond)
                    {
                        return i;
                    }
                }

                // 4. True Light (62089826)
                // Option index 0 is Special Summon BEWD (Option index 1 is Search/Set spell/trap).
                if (cardId == 62089826)
                {
                    bool hasBEWD = Bot.Hand.Any(c => c.Id == 89631139) || Bot.Graveyard.Any(c => c.Id == 89631139);
                    if (hasBEWD && optIndex == 0) return i;
                    if (!hasBEWD && optIndex == 1) return i;
                }

                // 5. Souleating Oviraptor (44335251)
                // Option index 0 is Add to hand (Option index 1 is Send to GY).
                if (cardId == 44335251)
                {
                    if (optIndex == 0) return i;
                }
            }

            return base.OnSelectOption(options);
        }


        // ═══════════════════════════════════════════════════════════════
        //  § 5. Combat Helpers
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Returns true if we can potentially deal lethal damage this turn.
        /// Uses Scorer.HasLethal() with fallback heuristic.
        /// </summary>
        protected virtual bool CanDealLethal()
        {
            if (Scorer != null && Scorer.HasLethal()) return true;

            int totalATK = 0;
            foreach (var m in Bot.GetMonsters())
            {
                if (m != null && m.IsFaceup() && m.IsAttack() && !m.Attacked)
                    totalATK += m.Attack;
            }
            return Enemy.GetMonsterCount() == 0 && totalATK >= Enemy.LifePoints;
        }

        /// <summary>
        /// Alias for CanDealLethal — some executors use CanOTK naming.
        /// </summary>
        protected bool CanOTK() => CanDealLethal();

        /// <summary>
        /// Returns true if we should skip combo (already have lethal in Main Phase 1).
        /// </summary>
        protected virtual bool ShouldSkipCombo()
        {
            return Duel.Phase == DuelPhase.Main1 && CanDealLethal();
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 6. Smart Phase Strategy — Link Summon & MP1/MP2 Optimization
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Returns total ATK of all face-up attack-position monsters on our field.
        /// Uses m.Attack which reflects runtime buffs (e.g., Underground +3000).
        /// </summary>
        protected int GetTotalFieldATK()
        {
            int total = 0;
            foreach (var m in Bot.GetMonsters())
            {
                if (m != null && m.IsFaceup() && m.IsAttack())
                    total += m.Attack;
            }
            return total;
        }

        /// <summary>
        /// Ace-as-Material protection logic.
        /// Checks if summoning a generic Extra Deck monster would force using an Ace card
        /// or a high ATK monster (ATK >= 2000) as material.
        /// Returns true if the summon should be avoided.
        /// </summary>
        protected virtual bool ShouldAvoidGenericExtraDeckSummon(int requiredMaterials)
        {
            int nonAceFodderCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Attack < 2000);
            return nonAceFodderCount < requiredMaterials;
        }

        /// <summary>
        /// Returns true if Link Summoning would be suboptimal — i.e., we should
        /// attack directly instead of spending materials on a Link monster.
        /// 
        /// Checks:
        ///   1. ShouldRushAttack flag (lethal already confirmed at turn start)
        ///   2. Current field ATK ≥ enemy LP with empty enemy board (dynamic lethal)
        ///   3. Link result ATK &lt; current field ATK with empty enemy board (ATK downgrade)
        /// 
        /// linkMonsterAtk: The expected ATK of the Link monster being summoned (including
        ///                 any known ATK gain effects, e.g., Accesscode = 5300 with gain).
        ///                 Pass 0 to skip the ATK comparison check.
        /// </summary>
        protected virtual bool ShouldSkipLinkSummon(int linkMonsterAtk = 0)
        {
            if (Duel.Phase != DuelPhase.Main1) return false;

            // Fast path: PreNewTurn already confirmed lethal
            if (ShouldRushAttack) return true;

            int totalFieldATK = GetTotalFieldATK();
            int enemyMonsters = Enemy.GetMonsterCount();
            int enemyLP = Enemy.LifePoints;

            // Condition 1: We can deal lethal by attacking directly — don't waste materials
            if (enemyMonsters == 0 && totalFieldATK >= enemyLP)
                return true;

            // Condition 2: Link would reduce total ATK when opponent board is empty/weak
            // (The Link monster replaces multiple smaller ones but has lower combined ATK)
            if (enemyMonsters == 0 && linkMonsterAtk > 0 && linkMonsterAtk < totalFieldATK)
                return true;

            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 6b. Smart Flow v2 — Dynamic Decision Engine
        //  These functions provide mid-turn intelligence that goes beyond
        //  the static ShouldRushAttack flag set at turn start.
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Re-evaluate lethal mid-turn. If board state changed since turn start
        /// and we now have lethal, upgrade to rush mode.
        /// Called at the top of every OnSelectIdleCmd.
        /// </summary>
        protected virtual void DynamicLethalCheck()
        {
            if (Duel.Phase != DuelPhase.Main1) return;
            if (ShouldRushAttack) return; // Already in rush mode
            if (Duel.Turn <= 1) return;   // Can't attack turn 1

            Scorer?.ClearCache();

            bool directLethal = Enemy.GetMonsterCount() == 0 && GetTotalFieldATK() >= Enemy.LifePoints;

            if ((Scorer != null && Scorer.HasLethal()) || directLethal)
            {
                ShouldRushAttack = true;
                SkipComboSearch = true;
                try
                {
                    AI?.Log(LogLevel.Info,
                        $"[DYNAMIC-LETHAL] ✓ UPGRADED to rush mode mid-turn | " +
                        $"Field ATK: {GetTotalFieldATK()} | Enemy LP: {Enemy.LifePoints} | " +
                        $"Enemy Monsters: {Enemy.GetMonsterCount()}");
                }
                catch { }
            }
        }

        /// <summary>
        /// Should we enter Battle Phase before continuing combo?
        /// Returns true when:
        ///   1. We're in MP1 with battle available
        ///   2. Opponent board is empty or weak (high AttackOpportunity)
        ///   3. We have attack-position monsters with meaningful ATK (≥1000)
        ///   4. There's no combo-critical activation pending
        ///   5. Not first turn
        /// 
        /// The idea: attack first → come back to MP2 → search/set there.
        /// This prevents the classic "search until monsters expire" problem.
        /// 
        /// Anti-timidity: also considers if we have Normal Summon still available
        /// (if so, we can safely attack then NS in MP2).
        /// </summary>
        protected virtual bool ShouldAttackBeforeCombo(MainPhase main)
        {
            if (Duel.Phase != DuelPhase.Main1) return false;
            if (main == null || !main.CanBattlePhase) return false;
            if (!Bot.HasAttackingMonster()) return false;
            if (Duel.Turn <= 1) return false; // Can't attack turn 1

            // Min ATK threshold: don't rush to attack with weak monsters
            // (e.g., a 500 ATK combo piece shouldn't trigger battle)
            int bestATK = 0;
            foreach (var m in Bot.GetMonsters())
            {
                if (m != null && m.IsFaceup() && m.IsAttack() && m.Attack > bestATK)
                    bestATK = m.Attack;
            }
            if (bestATK < 1000) return false; // Don't attack with tiny monsters

            // Use Analysis if available
            if (Analysis?.Current != null && Analysis.Current.ShouldAttackFirst)
            {
                // But don't rush if there's a combo-essential activation on field
                // (e.g., removal effect, ATK boost that must happen before battle)
                bool hasEssentialActivation = main.ActivableCards.Any(c =>
                    c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone
                    && !c.IsDisabled() && ClassifyAction(c, ExecutorType.Activate) == ActionPriority.CombatEssential);

                if (hasEssentialActivation) return false;

                // Don't rush if we have pending summons that would significantly boost ATK
                // (e.g., we could summon a 3000 ATK boss first)
                bool hasBigSummon = main.SpecialSummonableCards.Any(c =>
                    c != null && c.Attack > bestATK + 500);
                if (hasBigSummon) return false;

                try
                {
                    AI?.Log(LogLevel.Info,
                        $"[ATTACK-OPPORTUNITY] ✓ Attack before combo | " +
                        $"Score: {Analysis.Current.AttackOpportunity} | " +
                        $"Est. Damage: {Analysis.Current.EstimatedDirectDamage} | " +
                        $"Enemy Monsters: {Enemy.GetMonsterCount()}");
                }
                catch { }

                return true;
            }

            // Fallback: simple heuristic — enemy board empty + we have attackers
            if (Enemy.GetMonsterCount() == 0 && Bot.HasAttackingMonster())
            {
                // Don't rush if there's a summon/activation that would add significant ATK
                bool hasValueAction = main.ActivableCards.Count > 0
                    || main.SummonableCards.Count > 0
                    || main.SpecialSummonableCards.Count > 0;

                // If we only have deferrable actions (sets, searches), attack first
                if (!hasValueAction)
                    return true;

                // If we have actions but our ATK is already >= enemy LP, attack now
                if (GetTotalFieldATK() >= Enemy.LifePoints)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Classify an action by its priority for phase planning.
        /// Deck executors can override to provide deck-specific classification.
        /// 
        /// Default logic:
        ///   - Field monster activation → CombatEssential (removal/ATK boost)
        ///   - Normal Summon → ComboStarter
        ///   - Extra Deck SS → ComboExtender
        ///   - Hand/GY spell activation → ResourceGain
        ///   - Spell/Trap set → Deferrable
        /// </summary>
        protected virtual ActionPriority ClassifyAction(ClientCard card, ExecutorType actionType)
        {
            if (card == null) return ActionPriority.Deferrable;

            switch (actionType)
            {
                case ExecutorType.SpellSet:
                    return ActionPriority.Deferrable;

                case ExecutorType.Summon:
                case ExecutorType.SummonOrSet:
                    return ActionPriority.ComboStarter;

                case ExecutorType.SpSummon:
                    // Extra Deck summons = combo extender
                    bool isExtraDeck = card.HasType(CardType.Link) || card.HasType(CardType.Fusion)
                        || card.HasType(CardType.Synchro) || card.HasType(CardType.Xyz);
                    return isExtraDeck ? ActionPriority.ComboExtender : ActionPriority.ComboStarter;

                case ExecutorType.Activate:
                    // Field monster activation = usually removal/ATK boost = combat essential
                    if (card.Location == CardLocation.MonsterZone && card.IsFaceup())
                        return ActionPriority.CombatEssential;
                    // Face-up continuous/field spell = already active, could be ATK boost
                    if (card.Location == CardLocation.SpellZone && card.IsFaceup()
                        && card.HasType(CardType.Field))
                        return ActionPriority.CombatEssential;
                    // Face-down backrow = might be trap activation
                    if (card.Location == CardLocation.SpellZone && card.IsFacedown())
                        return ActionPriority.CombatEssential;
                    // [FIX CRITICAL-5] Hand activations: Normal/Quick-Play spells could be
                    // removal (Raigeki, Dark Hole) or combo starters (Poly, ROTA).
                    // Classify as ComboStarter so they're NOT blocked in Rush mode.
                    // Only Continuous/Field/Equip hand spells are ResourceGain (setup cards).
                    if (card.Location == CardLocation.Hand)
                    {
                        if (card.HasType(CardType.Spell) &&
                            (card.HasType(CardType.Continuous) || card.HasType(CardType.Field) || card.HasType(CardType.Equip)))
                            return ActionPriority.ResourceGain;
                        // Normal Spell / Quick-Play / Monster effect from hand = combo starter
                        return ActionPriority.ComboStarter;
                    }
                    // GY/Banished activation = resource gain
                    if (card.Location == CardLocation.Grave || card.Location == CardLocation.Removed)
                        return ActionPriority.ResourceGain;
                    return ActionPriority.ResourceGain;

                case ExecutorType.MonsterSet:
                    return ActionPriority.Deferrable;

                default:
                    return ActionPriority.ResourceGain;
            }
        }

        /// <summary>
        /// Should the bot stop extending combos?
        /// Returns true when the board is "good enough" and further extension risks:
        ///   - Nibiru (5+ summons)
        ///   - Board wipe (overextend into Dark Hole/Raigeki)
        ///   - ATK downgrade (Link summon reduces total damage)
        ///   - Resource waste (using cards that won't improve the board meaningfully)
        /// 
        /// Deck executors can override for deck-specific combo endpoints.
        /// </summary>
        protected virtual bool ShouldStopExtending()
        {
            // Use Analysis if available
            if (Analysis?.Current != null && Analysis.Current.ShouldStopExtending)
            {
                try
                {
                    AI?.Log(LogLevel.Info,
                        $"[COMBO-STOP] ✓ Board sufficient — stop extending | " +
                        $"Sufficiency: {Analysis.Current.BoardSufficiency} | " +
                        $"Disruptions: {CountDisruptions()} | " +
                        $"Monsters: {Bot.GetMonsterCount()}");
                }
                catch { }
                return true;
            }

            // ═══ ResourcePlanner integration: Nibiru + overextension check ═══
            if (ResourcePlan != null && ResourcePlan.Enabled)
            {
                bool shouldStop = ResourcePlan.ShouldStopExtending(
                    summonCountThisTurn: Brain?.OwnSummons ?? 0,
                    opponentHandCount: Enemy.Hand.Count,
                    ourMonsterCount: Bot.GetMonsterCount(),
                    opponentBackrowCount: Enemy.GetSpells().Count(c => c != null && c.IsFacedown()),
                    opponentMonsterCount: Enemy.GetMonsterCount(),
                    haveNegateOnField: HasNegateOnField(),
                    opponentIsControlDeck: OpponentProfile?.IsControlDeck() ?? false
                );
                if (shouldStop)
                {
                    try
                    {
                        AI?.Log(LogLevel.Info,
                            $"[RESOURCE-STOP] ✓ ResourcePlanner says stop | " +
                            $"Summons: {Brain?.OwnSummons ?? 0} | " +
                            $"Monsters: {Bot.GetMonsterCount()} | " +
                            $"Enemy Hand: {Enemy.Hand.Count}");
                    }
                    catch { }
                    return true;
                }
            }

            // Fallback: simple heuristic
            // Don't stop if board is weak
            if (NeedsBoardPresence()) return false;

            // Going second + enemy has monsters = DON'T stop, we need to deal with threats
            if (_isGoingSecond && Enemy.GetMonsterCount() > 0) return false;

            // Stop if we have 3+ disruptions and at least 1 attacker
            if (CountDisruptions() >= 3 && Bot.HasAttackingMonster())
                return true;

            // Stop if hand is nearly empty and we have decent board
            // BUT only if going first or enemy board is cleared
            if (Bot.Hand.Count <= 1 && Bot.GetMonsterCount() >= 3 && Enemy.GetMonsterCount() == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if we should defer setting backrow to Main Phase 2.
        /// Attacking first reveals less information and avoids "set → pass" tempo loss.
        /// Deck executors can override to always set combo-critical traps in MP1.
        /// </summary>
        protected virtual bool ShouldDeferToMP2()
        {
            if (Duel.Phase != DuelPhase.Main1) return false;
            if (Main == null || !Main.CanBattlePhase) return false;
            if (!Bot.HasAttackingMonster()) return false;

            // If we have lethal, don't waste time setting — go kill
            if (ShouldRushAttack) return true;

            // If opponent has no board, attack first then set in MP2
            if (Enemy.GetMonsterCount() == 0) return true;

            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 7. Original ModernExecutor Methods (preserved)
        // ═══════════════════════════════════════════════════════════════

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            var scripted = base.OnSelectAttackTarget(attacker, defenders);
            if (scripted != null) return scripted;

            if (Analysis == null || attacker == null || defenders == null) return null;

            foreach (var defender in defenders.Where(d => d != null).OrderByDescending(d => Scorer.ThreatScore(d)))
            {
                attacker.RealPower = attacker.Attack;
                defender.RealPower = defender.GetDefensePower();

                if (!OnPreBattleBetween(attacker, defender)) continue;
                if (Analysis.ShouldTradeForBoard(attacker, defender))
                    return AI.Attack(attacker, defender);
            }

            return null;
        }

        public override MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            // ═══ Smart Flow v2: Dynamic Lethal Re-evaluation ═══
            // Check if board state changed since turn start and we now have lethal
            DynamicLethalCheck();

            // ═══ ComboRouter: Activate best combo line from hand ═══
            if (ComboRouter != null && ComboRouter.Enabled && Duel.Phase == DuelPhase.Main1)
            {
                ComboRouter.ActivateBestLine(Bot);
            }

            // ═══ ComboRouter: Execute active combo step ═══
            if (ComboRouter != null && ComboRouter.Enabled && ComboRouter.HasActiveCombo)
            {
                int maxIterations = 15;
                while (ComboRouter.HasActiveCombo && --maxIterations > 0)
                {
                    var step = ComboRouter.GetNextStep();
                    if (step == null) break;

                    bool executed = false;
                    MainPhaseAction comboAction = null;

                    if (step.ActionType == ExecutorType.Activate)
                    {
                        for (int i = 0; i < main.ActivableCards.Count; ++i)
                        {
                            var card = main.ActivableCards[i];
                            if (card != null && (card.Id == step.CardId || card.GetNonAltartCode() == step.CardId))
                            {
                                comboAction = new MainPhaseAction(MainPhaseAction.MainAction.Activate, card.ActionActivateIndex[main.ActivableDescs[i]]);
                                executed = true;
                                break;
                            }
                        }
                    }
                    else if (step.ActionType == ExecutorType.Summon)
                    {
                        foreach (var card in main.SummonableCards)
                        {
                            if (card != null && (card.Id == step.CardId || card.GetNonAltartCode() == step.CardId))
                            {
                                comboAction = new MainPhaseAction(MainPhaseAction.MainAction.Summon, card.ActionIndex);
                                executed = true;
                                break;
                            }
                        }
                    }
                    else if (step.ActionType == ExecutorType.SpSummon)
                    {
                        foreach (var card in main.SpecialSummonableCards)
                        {
                            if (card != null && (card.Id == step.CardId || card.GetNonAltartCode() == step.CardId))
                            {
                                comboAction = new MainPhaseAction(MainPhaseAction.MainAction.SpSummon, card.ActionIndex);
                                executed = true;
                                break;
                            }
                        }
                    }
                    else if (step.ActionType == ExecutorType.SpellSet)
                    {
                        foreach (var card in main.SpellSetableCards)
                        {
                            if (card != null && (card.Id == step.CardId || card.GetNonAltartCode() == step.CardId))
                            {
                                comboAction = new MainPhaseAction(MainPhaseAction.MainAction.SetSpell, card.ActionIndex);
                                executed = true;
                                break;
                            }
                        }
                    }

                    if (executed)
                    {
                        ComboRouter.CompleteCurrentStep();
                        try
                        {
                            AI?.Log(LogLevel.Info, $"[COMBO-STEP] Executing: {step.Description ?? step.CardId.ToString()} ({step.ActionType})");
                        }
                        catch { }
                        return comboAction;
                    }
                    else
                    {
                        if (step.Optional)
                        {
                            ComboRouter.SkipCurrentStep(Bot);
                        }
                        else
                        {
                            // Try switching to Plan B (fallback line) before giving up!
                            bool switched = ComboRouter.TrySwitchToFallback(Bot);
                            if (switched)
                            {
                                try
                                {
                                    AI?.Log(LogLevel.Info, $"[COMBO-FALLBACK] Step {step.CardId} failed — switched to fallback combo: {ComboRouter.ActiveComboName}");
                                }
                                catch { }
                                continue;
                            }
                            else
                            {
                                ComboRouter.AbortCombo($"Step card {step.CardId} ({step.ActionType}) not available/playable and no fallback viable");
                                break;
                            }
                        }
                    }
                }
            }

            // ═══ Smart Flow v2: Attack Before Combo ═══
            // [FIX CRITICAL-1] Only attack-first when there are NO valuable main-phase actions remaining.
            // Previously this bypassed the entire CardExecutor loop, skipping combo starters.
            if (ShouldAttackBeforeCombo(main)
                && main.SummonableCards.Count == 0
                && main.SpecialSummonableCards.Count == 0
                && !main.ActivableCards.Any(c => c != null &&
                    ClassifyAction(c, ExecutorType.Activate) <= ActionPriority.ComboStarter))
            {
                return new MainPhaseAction(MainPhaseAction.MainAction.ToBattlePhase);
            }

            // ═══ Lethal Rush: Skip non-essential actions in MP1 ═══
            if (Duel.Phase == DuelPhase.Main1 && main.CanBattlePhase && ShouldRushAttack)
            {
                // [FIX CRITICAL-2 partial] Allow CombatEssential AND ComboStarter activations
                // (e.g., Dark Hole from hand is ComboStarter, must not be blocked)
                bool hasUsefulActivation = main.ActivableCards.Any(c =>
                    c != null && ClassifyAction(c, ExecutorType.Activate) <= ActionPriority.ComboStarter);

                if (!hasUsefulActivation)
                    return new MainPhaseAction(MainPhaseAction.MainAction.ToBattlePhase);
            }

            // ═══ Smart Flow v2: Stop Extending ═══
            // [FIX CRITICAL-4] Also check for pending summons/SpSummons before stopping.
            // Previously only checked activations, causing bot to skip Normal Summon.
            if (Duel.Phase == DuelPhase.Main1 && ShouldStopExtending())
            {
                bool hasEssential = main.ActivableCards.Any(c =>
                    c != null && ClassifyAction(c, ExecutorType.Activate) <= ActionPriority.ComboStarter);
                bool hasPendingSummons = main.SummonableCards.Count > 0
                    || main.SpecialSummonableCards.Count > 0;

                if (!hasEssential && !hasPendingSummons)
                {
                    if (main.CanBattlePhase && Bot.HasAttackingMonster())
                        return new MainPhaseAction(MainPhaseAction.MainAction.ToBattlePhase);
                }
            }

            if (ShouldBattleBeforeSetting(main))
                return new MainPhaseAction(MainPhaseAction.MainAction.ToBattlePhase);

            return base.OnSelectIdleCmd(main);
        }

        protected virtual bool ShouldBattleBeforeSetting(MainPhase main)
        {
            if (main == null || Analysis == null) return false;
            if (!main.CanBattlePhase || !Analysis.Current.PreferBattleBeforeSetting) return false;
            if ((main.MonsterSetableCards.Count + main.SpellSetableCards.Count) == 0) return false;

            // Do not interrupt deck-specific combo/action windows; only defer pure set/pass states.
            if (main.ActivableCards.Count > 0) return false;
            if (main.SummonableCards.Count > 0) return false;
            if (main.SpecialSummonableCards.Count > 0) return false;
            if (main.ReposableCards.Count > 0) return false;

            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 8. Smart Phase Strategy — Guard Overrides
        //  These gate EVERY action in GameAI.InternalOnSelectIdleCmd.
        //  All 2026 executors (inheriting ModernExecutor) benefit automatically.
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Log a phase guard decision for diagnostics.
        /// Output: [PHASE-GUARD] BLOCKED/ALLOWED Type: CardName (CardId) — reason
        /// </summary>
        private void LogPhaseGuard(string action, string type, ClientCard card, string reason)
        {
            try
            {
                int cardId = card?.Id ?? 0;
                string dedupKey = $"{type}:{cardId}";
                if (!_guardLogDedup.Add(dedupKey)) return; // Already logged this card+type this cycle

                string cardName = card?.Name ?? $"ID:{cardId}";
                AI?.Log(LogLevel.Info, $"[PHASE-GUARD] {action} {type}: {cardName} ({cardId}) — {reason}");
            }
            catch { /* logging must never crash the bot */ }
        }

        /// <summary>
        /// Evaluate whether a Special Summon is worth doing right now.
        /// Blocks Extra Deck summons that would reduce total ATK when:
        ///   - Lethal is already available (ShouldRushAttack)
        ///   - Opponent board is empty and combined ATK already exceeds their LP
        ///   - The summon result ATK is lower than current total field ATK
        /// Main Deck SS (hand/GY) is always allowed (adds to board).
        /// </summary>
        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (card == null) return true;
            if (Duel.Phase != DuelPhase.Main1) return true;

            // Main Deck SS (from hand/GY) = adding monsters to board → always OK
            bool isExtraDeckSummon = card.HasType(CardType.Link) || card.HasType(CardType.Fusion)
                || card.HasType(CardType.Synchro) || card.HasType(CardType.Xyz);
            if (!isExtraDeckSummon) return true;

            // Lethal confirmed at turn start → don't spend materials
            if (ShouldRushAttack)
            {
                LogPhaseGuard("BLOCKED", "SpSummon", card, "Rush mode — lethal confirmed, skip ED summon");
                return false;
            }

            int totalFieldATK = GetTotalFieldATK();
            int enemyMonsters = Enemy.GetMonsterCount();

            if (Duel.Turn > 1)
            {
                // Opponent board empty + our field ATK ≥ enemy LP → just attack
                if (enemyMonsters == 0 && totalFieldATK >= Enemy.LifePoints)
                {
                    LogPhaseGuard("BLOCKED", "SpSummon", card, $"Lethal by direct attack — ATK {totalFieldATK} >= LP {Enemy.LifePoints}");
                    return false;
                }

                // [FIX CRITICAL-3] REMOVED: ATK downgrade check (card.Attack < totalFieldATK)
                // This was fundamentally flawed because:
                //   1. card.Attack is the base ATK of the ED monster, not accounting for on-summon effects
                //      (e.g., Accesscode Talker base=2300 but gains +2000-3000 from effect)
                //   2. The comparison assumed the ED monster replaces ALL field monsters,
                //      but Link-2 only uses 2 materials, not all 5.
                //   3. Caused bot to NEVER summon boss monsters when field total was high.
            }

            return true;
        }

        /// <summary>
        /// Evaluate whether setting backrow is worth doing right now.
        /// Defers to MP2 when:
        ///   - Lethal is confirmed (don't waste time)
        ///   - Opponent board is empty and we have attackers (attack first, set after)
        /// </summary>
        public override bool ShouldAllowSpellSet(ClientCard card)
        {
            if (card == null) return true;
            if (Duel.Phase != DuelPhase.Main1) return true;

            // Lethal → skip all setting, rush to battle
            if (ShouldRushAttack)
            {
                LogPhaseGuard("BLOCKED", "SpellSet", card, "Rush mode — defer backrow to MP2");
                return false;
            }

            // Opponent board empty + we have attackers → battle first, set in MP2
            if (Duel.Turn > 1 && Enemy.GetMonsterCount() == 0 && Bot.HasAttackingMonster())
            {
                LogPhaseGuard("BLOCKED", "SpellSet", card, "Opponent board empty — battle first, set in MP2");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Evaluate whether a Normal Summon is worth doing right now.
        /// Blocks when:
        ///   - Lethal is confirmed and this monster doesn't increase damage
        ///   - Monster zones are full (can't summon anyway, but guard)
        /// </summary>
        public override bool ShouldAllowSummon(ClientCard card)
        {
            if (card == null) return true;
            if (Duel.Phase != DuelPhase.Main1) return true;

            // Lethal confirmed → only summon if it increases ATK or provides removal
            if (ShouldRushAttack)
            {
                // Small monsters (ATK ≤ 500) don't help push damage
                if (card.Attack <= 500)
                {
                    LogPhaseGuard("BLOCKED", "Summon", card, $"Rush mode — ATK {card.Attack} too low to help push");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Evaluate whether an effect activation is worth doing right now.
        /// 
        /// Smart Flow v2 enhanced:
        /// 1. LETHAL MODE: Only allow combat-enhancing activations
        /// 2. ATTACK-FIRST MODE: Block ResourceGain/Deferrable when should attack first
        /// 3. STOP-EXTENDING MODE: Block ComboExtender when board is sufficient
        /// 4. Normal: Allow everything
        /// </summary>
        public override bool ShouldAllowActivate(ClientCard card)
        {
            if (card == null) return true;
            if (Duel.Phase != DuelPhase.Main1) return true;

            ActionPriority priority = ClassifyAction(card, ExecutorType.Activate);

            // === LETHAL MODE: Allow combat + combo activations, block only setup ===
            // [FIX CRITICAL-2 partial + CRITICAL-5] Previously blocked everything except
            // CombatEssential, which prevented hand removal spells (Dark Hole, Raigeki)
            // and combo starters from resolving during rush mode.
            if (ShouldRushAttack)
            {
                if (priority == ActionPriority.CombatEssential || priority == ActionPriority.ComboStarter)
                    return true;

                // Block only Deferrable and ComboExtender in rush mode
                // ResourceGain (GY effects, etc.) is still allowed — it might enable the kill
                if (priority == ActionPriority.Deferrable || priority == ActionPriority.ComboExtender)
                {
                    LogPhaseGuard("BLOCKED", "Activate", card,
                        $"Rush mode — skip {priority} activation");
                    return false;
                }
                return true;
            }

            // === ATTACK-FIRST MODE: Defer non-essential when attack opportunity exists ===
            if (Analysis?.Current != null && Analysis.Current.ShouldAttackFirst)
            {
                // Block only Deferrable — allow ResourceGain and ComboStarter
                // This way the bot can still use search spells that are combo-critical,
                // but won't waste time setting backrow before attacking
                if (priority == ActionPriority.Deferrable)
                {
                    LogPhaseGuard("BLOCKED", "Activate", card,
                        $"Attack-first mode — defer {priority} to MP2");
                    return false;
                }
            }

            // === STOP-EXTENDING MODE: Block combo extenders when board is sufficient ===
            if (Analysis?.Current != null && Analysis.Current.ShouldStopExtending)
            {
                // Only block pure ComboExtender — allow ResourceGain (might be removal spells)
                if (priority == ActionPriority.ComboExtender)
                {
                    // Exception: allow if NeedsBoardPresence (we need at least some board)
                    if (!NeedsBoardPresence())
                    {
                        LogPhaseGuard("BLOCKED", "Activate", card,
                            $"Board sufficient — skip {priority} activation");
                        return false;
                    }
                }
            }

            // === MP2 COMPLETION MODE: After battle, skip wasteful activations if board is done ===
            // In MP2, the bot has already attacked. If the board is strong enough,
            // block search/draw/GY-recovery effects that would waste resources.
            // Only allow CombatEssential (field monster effects, trap activations).
            // Setting backrow still happens (SpellSet is a separate check in GameAI).
            if (Duel.Phase == DuelPhase.Main2 && IsBoardStrongEnough() &&
                (priority == ActionPriority.ResourceGain || priority == ActionPriority.ComboStarter))
            {
                LogPhaseGuard("BLOCKED", "Activate", card,
                    "MP2 with strong board — skip activation, pass turn");
                return false;
            }

            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 9. Enhancement Module Lifecycle
        //  Reset and feed data to enhancement modules.
        // ═══════════════════════════════════════════════════════════════

        public override void OnNewTurn()
        {
            base.OnNewTurn();

            _comboStepNegatedThisChain = false;
            _negatedComboStepCardId = -1;

            // ── Activate Core Decision Flags (Easy Lethal, BreakBoard EV, etc.) ──
            PreNewTurn();

            // Reset per-turn state in enhancement modules
            ComboRouter?.OnNewTurn();
            BaitPlanner?.OnNewTurn();

            // Reset profiler for a new duel
            if (Duel.Turn == 1)
            {
                OpponentProfile?.ResetDuel();
                AIContext?.ResetDuel();
            }
            
            AIContext?.UpdateState(0, 0);
        }

        public override void OnDraw(int player)
        {
            base.OnDraw(player);
            if (player == 1)
            {
                // Normal draw phase draw doesn't count as extra draw
                if (Duel.Phase != DuelPhase.Draw)
                {
                    OpponentProfile?.OnOpponentExtraDraw();
                }
            }
            AIContext?.ActionHistory.Record(player, 0, "Draw", Duel.Turn, Duel.Phase.ToString());
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);

            if (card != null)
            {
                AIContext?.ActionHistory.Record(player, card.Id, "Activate", Duel.Turn, Duel.Phase.ToString());
            }

            // Feed opponent card data to the profiler
            if (player == 1 && card != null)
            {
                OpponentProfile?.OnOpponentActivate(card);

                // Track opponent chain response for bait system
                BaitPlanner?.OnOpponentChainResponse();

                // Negation detection for active combo steps
                if (ComboRouter != null && ComboRouter.HasActiveCombo)
                {
                    ClientCard ourCard = Util.GetLastChainCard();
                    if (ourCard != null && ourCard.Controller == 0 && ComboRouter.IsPartOfActiveCombo(ourCard.Id))
                    {
                        if (IsOpponentCardNegator(card))
                        {
                            _comboStepNegatedThisChain = true;
                            _negatedComboStepCardId = ourCard.Id;
                        }
                    }
                }
            }
        }

        public override void OnChainEnd()
        {
            base.OnChainEnd();

            if (_comboStepNegatedThisChain)
            {
                _comboStepNegatedThisChain = false;
                ComboRouter?.NotifyStepNegated(_negatedComboStepCardId);
                _negatedComboStepCardId = -1;
            }
        }

        private bool IsOpponentCardNegator(ClientCard card)
        {
            if (card == null) return false;

            // 1. Check known negate monsters
            if (_negateMonsters.Contains(card.Id)) return true;

            // 2. Check Central CardIntelligence
            if (CardIntelligence.IsKnownNegator(card.Id) || CardIntelligence.IsHandtrap(card.Id)) return true;

            // 3. Check known hand traps / negates by ID
            int[] negateIds = {
                14558127, 14558128, // Ash Blossom
                73642296,          // Ghost Belle
                97268402,          // Effect Veiler
                63845230,          // Eater of Millions
                10045474,          // Infinite Impermanence
                24224830,          // Called by the Grave
                41420027,          // Solemn Judgment
                84256858,          // Solemn Strike
                92584307,          // Solemn Warning
                23002292           // Red Reboot
            };
            if (negateIds.Contains(card.Id)) return true;

            return false;
        }

        public override void OnNewPhase()
        {
            base.OnNewPhase();
            PreNewPhase();  // Sets InMainPhase2 flag

            // Feed opponent summoned monsters to profiler (on phase change,
            // we can scan new monsters that appeared)
            if (OpponentProfile != null)
            {
                foreach (var m in Enemy.GetMonsters())
                {
                    if (m != null && m.IsFaceup())
                        OpponentProfile.OnCardRevealed(m);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 10. Smart Hand Trap Chain Helper
        //  Wraps ChainTimingAdvisor for easy use in deck executors.
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Smart hand trap chain decision — should we use our hand trap on this chain?
        /// Replaces the simple "always chain" default with intelligent timing.
        /// 
        /// Usage in deck executor:
        ///   protected bool SmartAshBlossom() => SmartHandTrapChain();
        ///   AddExecutor(ExecutorType.Activate, _CardId.AshBlossom, SmartAshBlossom);
        /// </summary>
        protected bool SmartHandTrapChain()
        {
            // Fallback: if advisor is disabled or not available, use default behavior
            if (ChainAdvisor == null || !ChainAdvisor.Enabled)
                return Duel.LastChainPlayer == 1;

            // Must be opponent's activation
            if (Duel.LastChainPlayer != 1)
                return false;

            var targetCard = Util.GetLastChainCard();
            if (targetCard == null)
                return true; // No info → chain as fallback

            int interactiveCount = ChainAdvisor.CountInteractiveCards(Bot);
            int opponentSummons = Brain?.OpponentSummonCount ?? 0; // This tracks opponent summons during their turn

            bool shouldHold = ChainAdvisor.ShouldHoldResponse(
                ourCard: Card,
                targetCard: targetCard,
                opponentHandCount: Enemy.Hand.Count,
                opponentSummonCount: opponentSummons,
                ourInteractiveCount: interactiveCount,
                isChokepoint: false // Deck executors can override with chokepoint check
            );

            if (shouldHold)
            {
                try
                {
                    AI?.Log(LogLevel.Info,
                        $"[CHAIN-TIMING] HOLD {Card?.Name ?? "?"} vs {targetCard.Name ?? targetCard.Id.ToString()} — saving for better target");
                }
                catch { }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Enhanced version of SmartHandTrapChain that checks chokepoints.
        /// Pass chokepoint card IDs to boost negate priority for those targets.
        /// </summary>
        protected bool SmartHandTrapChain(params int[] chokepointIds)
        {
            if (ChainAdvisor == null || !ChainAdvisor.Enabled)
                return Duel.LastChainPlayer == 1;

            if (Duel.LastChainPlayer != 1)
                return false;

            var targetCard = Util.GetLastChainCard();
            if (targetCard == null)
                return true;

            bool isChokepoint = chokepointIds != null && chokepointIds.Contains(targetCard.Id);
            int interactiveCount = ChainAdvisor.CountInteractiveCards(Bot);

            bool shouldHold = ChainAdvisor.ShouldHoldResponse(
                ourCard: Card,
                targetCard: targetCard,
                opponentHandCount: Enemy.Hand.Count,
                opponentSummonCount: Brain?.OpponentSummonCount ?? 0,
                ourInteractiveCount: interactiveCount,
                isChokepoint: isChokepoint
            );

            if (shouldHold)
            {
                try
                {
                    AI?.Log(LogLevel.Info,
                        $"[CHAIN-TIMING] HOLD {Card?.Name ?? "?"} vs {targetCard.Name ?? targetCard.Id.ToString()} — saving for better target");
                }
                catch { }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if bait should be played before the intended card.
        /// Returns the bait card to play, or null if no baiting needed.
        /// Deck executors call this at the top of their combo starter logic.
        /// </summary>
        protected ClientCard GetBaitIfNeeded(ClientCard intendedCard)
        {
            if (BaitPlanner == null || !BaitPlanner.Enabled)
                return null;

            bool shouldBait = BaitPlanner.ShouldBaitFirst(
                intendedCard: intendedCard,
                bot: Bot,
                opponentHandCount: Enemy.Hand.Count,
                turn: Duel.Turn,
                opponentHasChainedThisTurn: Brain?.EnemyChainedThisChain ?? false,
                isGoingFirst: !_isGoingSecond
            );

            if (!shouldBait) return null;

            var baitCard = BaitPlanner.GetBaitCard(Bot);
            if (baitCard != null)
            {
                try
                {
                    AI?.Log(LogLevel.Info,
                        $"[BAIT] Playing {baitCard.Name ?? baitCard.Id.ToString()} as bait before {intendedCard?.Name ?? "combo starter"}");
                }
                catch { }
            }
            return baitCard;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 11. Hint-Based Universal Smart Card & Material Selection
        // ═══════════════════════════════════════════════════════════════

        protected virtual int GetCardThreatScore(ClientCard c)
        {
            if (c == null) return 0;
            int score = 0;

            // High-threat floodgates and omni-negates
            if (_negateMonsters.Contains(c.Id)) score += 10000;
            if (_spSummonBlockMonsters.Contains(c.Id)) score += 9500;

            if (c.IsSpell() || c.IsTrap())
            {
                if (c.IsFaceup())
                {
                    if (c.Id == 48680970) score += 12000; // Eternal Soul (destroying wipes monsters!)
                    else if (c.Id == 82732047) score += 11000; // Skill Drain
                    else if (c.Id == 38009249) score += 9500;  // Runick Fountain
                    else if (c.Id == 38033121) score += 9000;  // Dark Magical Circle
                    else if (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)) score += 6000;
                    else score += 3000;
                }
                else
                {
                    score += 4500; // Unknown set backrow
                }
            }
            else if (c.IsMonster())
            {
                if (c.IsExtraCard()) score += 4000;
                score += c.Attack;
            }

            return score;
        }

        protected virtual int GetCardDiscardSacrificeCost(ClientCard c)
        {
            if (c == null) return 999999;
            int cost = 0;

            if (c.HasType(CardType.Token)) return -1000;
            if (c.HasType(CardType.Normal)) return -500;

            // Handtraps & Key Starters are extremely valuable
            if (c.IsCode(14558127, 14558128, 23434538, 10045474, 94145021, 97268402, 63845230, 42141493, 84192580))
                cost += 8000;
            if (_negateMonsters.Contains(c.Id))
                cost += 10000;

            // Prefer discarding duplicates if we have more than 1 copy in hand
            if (Bot.Hand.Count(h => h != null && h.Id == c.Id) > 1)
                cost -= 2000;

            return cost;
        }

        protected virtual int GetMaterialSacrificePriority(ClientCard c)
        {
            if (c == null) return 999999;

            // Enemy cards (via Super Poly or Fallen of Albaz) are #1 priority!
            if (c.Controller == 1) return 0;

            // Hand / Grave materials are preferred over field bosses
            if (c.Location == CardLocation.Grave) return 10;
            if (c.Location == CardLocation.Deck) return 20;
            if (c.Location == CardLocation.Hand) return 50;

            // Field materials
            if (c.Location == CardLocation.MonsterZone)
            {
                if (c.HasType(CardType.Token)) return 100;
                if (c.HasType(CardType.Normal)) return 150;
                if (c.Attack <= 1000) return 200;
                if (c.Attack <= 2000) return 300;

                // Protect active boss monsters with disruptions on field!
                if (_negateMonsters.Contains(c.Id)) return 10000;
                if (c.IsExtraCard() && c.Attack >= 2500) return 5000;

                return 1000;
            }

            return 500;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // S:P Little Knight override
            if (Card != null && Card.Id == 29301450)
            {
                var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (oppCards.Count >= min)
                {
                    var sortedOpp = oppCards.OrderBy(c => {
                        if (c.Location == CardLocation.MonsterZone) return 1;
                        if (c.Location == CardLocation.SpellZone) return 2;
                        return 3;
                    }).ToList();
                    return sortedOpp.Take(max).ToList();
                }
            }

            const long HINTMSG_RELEASE = 500;
            const long HINTMSG_DISCARD = 501;
            const long HINTMSG_DESTROY = 502;
            const long HINTMSG_REMOVE = 504;
            const long HINTMSG_RTOHAND = 505;
            const long HINTMSG_TODECK = 506;
            const long HINTMSG_EQUIP = 507;
            const long HINTMSG_TOGRAVE = 508;
            const long HINTMSG_SPSUMMON = 509;
            const long HINTMSG_CONTROL = 519;
            const long HINTMSG_POSCHANGE = 518;
            const long HINTMSG_TARGET = 551;
            const long HINTMSG_DISABLE = 552;
            const long HINTMSG_NEGATE = 572;
            const long HINTMSG_FACEUP = 575;

            var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
            var ourCards = cards.Where(c => c != null && c.Controller == 0).ToList();

            // ── 1. Removal & Disruption against Enemy Cards ──
            if (hint == HINTMSG_DESTROY || hint == HINTMSG_REMOVE || hint == HINTMSG_RTOHAND ||
                hint == HINTMSG_TODECK || hint == HINTMSG_CONTROL || hint == HINTMSG_TARGET ||
                hint == HINTMSG_DISABLE || hint == HINTMSG_NEGATE || hint == HINTMSG_FACEUP)
            {
                if (enemyCards.Count >= min)
                {
                    var viable = enemyCards.Where(c => !IsTargetImmune(c) && !c.IsShouldNotBeTarget()).ToList();
                    var candidatePool = viable.Count >= min ? viable : enemyCards;
                    var sorted = candidatePool.OrderByDescending(c => GetCardThreatScore(c)).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
            }

            // ── 2. Discard / Send to GY / Tribute / Material from our Hand or Field ──
            if (hint == HINTMSG_DISCARD || hint == HINTMSG_TOGRAVE || hint == HINTMSG_RELEASE)
            {
                if (ourCards.Count >= min)
                {
                    var sorted = ourCards.OrderBy(c => GetCardDiscardSacrificeCost(c)).ToList();
                    return sorted.Take(min).ToList();
                }
            }

            // ── 3. Special Summon (From Extra Deck, GY, Deck, Hand) ──
            if (hint == HINTMSG_SPSUMMON)
            {
                if (ourCards.Count >= min)
                {
                    var sorted = ourCards.OrderByDescending(c => {
                        int score = 0;
                        if (IsAceCard(c)) score += 10000;
                        if (c.IsExtraCard()) score += 5000;
                        if (_negateMonsters.Contains(c.Id)) score += 8000;
                        score += c.Attack;
                        return score;
                    }).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
            }

            // ── 4. Add to Hand / Search (When only our cards are available) ──
            if (hint == HINTMSG_RTOHAND && enemyCards.Count == 0 && ourCards.Count >= min)
            {
                var sorted = ourCards.OrderByDescending(c => {
                    int score = 0;
                    if (IsAceCard(c)) score += 8000;
                    // Handtraps
                    if (c.IsCode(14558127, 14558128, 23434538, 10045474, 94145021, 97268402, 63845230, 42141493, 84192580)) score += 6000;
                    if (c.HasType(CardType.Monster)) score += 3000 + c.Attack;
                    if (c.HasType(CardType.Spell)) score += 2000;
                    return score;
                }).ToList();
                return sorted.Take(Math.Min(max, sorted.Count)).ToList();
            }

            // ── 5. Position Change (e.g. Book of Moon) ──
            if (hint == HINTMSG_POSCHANGE)
            {
                if (enemyCards.Count >= min)
                {
                    var viable = enemyCards.Where(c => !IsTargetImmune(c)).ToList();
                    var candidates = viable.Count >= min ? viable : enemyCards;
                    var sorted = candidates.OrderByDescending(c => c.Attack).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
            }

            // ── 6. Equip Card ──
            if (hint == HINTMSG_EQUIP)
            {
                if (ourCards.Count >= min)
                {
                    var sorted = ourCards.OrderByDescending(c => (IsAceCard(c) ? 10000 : 0) + c.Attack).ToList();
                    return sorted.Take(min).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectFusionMaterial(cards, min, max);
            var sorted = cards.OrderBy(c => GetMaterialSacrificePriority(c)).ToList();
            return sorted.Take(min).ToList();
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectLinkMaterial(cards, min, max);
            var sorted = cards.OrderBy(c => GetMaterialSacrificePriority(c)).ToList();
            return sorted.Take(min).ToList();
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectXyzMaterial(cards, min, max);
            var sorted = cards.OrderBy(c => GetMaterialSacrificePriority(c)).ToList();
            return sorted.Take(min).ToList();
        }

        /// <summary>
        /// Smart default: low ATK monsters (hand traps, combo pieces) → DEF position.
        /// Boss monsters (Extra Deck, high ATK) → ATK position.
        /// Individual executors can override for archetype-specific logic.
        /// </summary>
        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            var card = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (card == null) return base.OnSelectPosition(cardId, positions);

            bool isExtraDeck = card.IsExtraCard();
            int atk = card.Attack;
            int def = card.Defense;

            // Extra Deck monsters / high ATK → ATK position
            if (isExtraDeck || atk >= 2000)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            // Low ATK / hand traps / combo enablers → DEF position
            if (def > 0 && positions.Contains(CardPosition.FaceUpDefence))
                return CardPosition.FaceUpDefence;

            return base.OnSelectPosition(cardId, positions);
        }

        /// <summary>
        /// Universal Yes/No prompt safety guard:
        /// Prevents self-destruction when prompted for optional field removals (destroy/banish)
        /// if the opponent has no valid targets on the field.
        /// </summary>
        public override bool OnSelectYesNo(long desc)
        {
            // Safeguard 1: Dracotail Pan (95232014) optional destroy 1 monster on field
            if (desc == Util.GetStringId(95232014, 2))
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            }

            // Safeguard 2: Dracotail Urgula (95232011) optional destroy 1 spell/trap on field
            if (desc == Util.GetStringId(95232011, 2))
            {
                return Enemy.GetSpells().Any(c => c != null);
            }

            // Safeguard 3: Epurrely Plump (74701381) optional banish 1 monster on field
            if (desc == Util.GetStringId(74701381, 2))
            {
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            }

            // Safeguard 4: Universal empty opponent field trap
            // If enemy has no cards on field, refuse optional removal prompts to avoid destroying own cards
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0)
            {
                long cardIdFromDesc = (desc >> 20);
                long cardIdFromDesc4 = (desc >> 4);
                if (cardIdFromDesc == 95232014 || cardIdFromDesc == 95232011 || cardIdFromDesc == 74701381 ||
                    cardIdFromDesc4 == 95232014 || cardIdFromDesc4 == 95232011 || cardIdFromDesc4 == 74701381)
                {
                    return false;
                }
            }

            return base.OnSelectYesNo(desc);
        }
    }
}
