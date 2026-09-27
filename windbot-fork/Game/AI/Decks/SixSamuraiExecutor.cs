// ============================================================================
// 2026_SixSamurai — DECOUPLED DECK PLUGIN AI ARCHITECTURE (v11.0 Audited)
// ============================================================================
// Architecture:
//   Layer 1: WindBot Core (Protocol, Game State, OCGCore Messages)
//   Layer 2: Generic AI (DecisionContext, ThreatAnalyzer, BoardScorer, BeliefState)
//   Layer 3: SixSamuraiPlugin (Deck-Specific Domain Logic & Sub-Helpers)
//            ├─ SixSamStrategy (Going 1st/2nd Phased Game Plan & Routing)
//            ├─ SixSamCounterEconomy (Bushido Counter Arithmetic & Loop Manager)
//            ├─ SixSamKizaruResolver (Dynamic Attribute-Aware Search Filter)
//            ├─ SixSamMaterialScorer (Ace Monster Preservation & Fodder Priority)
//            ├─ SixSamActionScorer (Value Vector Evaluation for Candidate Actions)
//            ├─ SixSamRecoveryPlanner (Branch Switching when Negated/Interrupted)
//            └─ SixSamBoardAssessor (Threat & Lethal OTK Window Evaluation)
//   Layer 4: DecisionTracer (Real-Time Explainable Decision Logging)
//   Layer 5: Executor Pipeline (Executes Highest Evaluated Action)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;

namespace WindBot.Game.AI.Decks
{
    [Deck("SixSamurai", "2026_SixSamurai")]
    [Deck("2026_SixSamurai", "2026_SixSamurai")]
    public class _2026_SixSamuraiExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Monsters
            public const int Kizan = 49721904;
            public const int Kageki = 2511717;
            public const int TacticalTrainer = 16968936;
            public const int AnarchistMonk = 80570228;
            public const int Fuma = 71207871;
            public const int Hatsume = 44686185;
            public const int Kizaru = 6579928;
            public const int Grandmaster = 83039729;
            public const int GreatShogunShien = 63176202;
            public const int Mizuho = 74094021;
            public const int Shinai = 48505422;
            public const int Genba = 7291576;

            // Spells
            public const int GatewayOfTheSix = 27970830;
            public const int ShiensSmokeSignal = 54031490;
            public const int ShiensDojo = 47436247;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int AsceticismOfTheSixSamurai = 27821104;
            public const int SixStrikeDoubleAssault = 28273805;
            public const int CunningOfTheSixSamurai = 27178262;

            // Handtraps & Defensive Staples
            public const int InfiniteImpermanence = 10045474;
            public const int AshBlossom = 14558127;
            public const int EffectVeiler = 97268402;
            public const int Nibiru = 27204311;
            public const int GhostBelle = 73642296;
            public const int DrollAndLock = 94145021;
            public const int HarpiesFeatherDuster = 18144506;
            public const int CrossoutDesignator = 65681983;

            // Extra Deck
            public const int BattleShogun = 74752631;
            public const int LegendaryShiEn = 29981921;
            public const int LegendaryLordShiEn = 34235530;
            public const int LegendaryLordEnishi = 70634245;
            public const int LegendaryLordKizan = 42209438;
            public const int NaturiaBeast = 33198837;
            public const int NaturiaBarkion = 2956282;
            public const int FADawnDragster = 33158448;
            public const int TornadoDragon = 6983839;
            public const int IPMasquerena = 65741786;
            public const int SPLittleKnight = 29301450;
            public const int SaryujaSkullDread = 74997493;
        }

        // Audited OCGCore Hint Message Constants (100% verified from constant.lua)
        private const long HINT_RELEASE = 500;
        private const long HINT_DISCARD = 501;
        private const long HINT_DESTROY = 502;
        private const long HINT_REMOVE = 503;
        private const long HINT_TOGRAVE = 504;
        private const long HINT_RTOHAND = 505;
        private const long HINT_ATOHAND = 506; // Audited Search/Add to Hand
        private const long HINT_SUMMON = 508;
        private const long HINT_SPSUMMON = 509;
        private const long HINT_FMATERIAL = 511;
        private const long HINT_SMATERIAL = 512;
        private const long HINT_XMATERIAL = 513;
        private const long HINT_LMATERIAL = 533;
        private const long HINT_TARGET = 551;
        private const long HINT_TOZONE = 571;
        private const long HINT_COUNTER = 572;
        private const long HINT_NEGATE = 575;
        private const long HINT_ATTACH = 578;

        // Central Domain Plugin Coordinator
        internal SixSamuraiPlugin Plugin { get; private set; }
        public ClientCard CurrentLastChainCard => LastChainCard;

        public _2026_SixSamuraiExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // Instantiate Master Domain Plugin
            Plugin = new SixSamuraiPlugin(this);
            DeckPlugin = Plugin;

            // Register Strategic Assets with Core Framework
            ResourcePlan.RegisterAceCards(
                CardId.LegendaryLordShiEn,
                CardId.LegendaryShiEn,
                CardId.LegendaryLordKizan,
                CardId.GreatShogunShien,
                CardId.NaturiaBeast,
                CardId.NaturiaBarkion,
                CardId.TornadoDragon,
                CardId.IPMasquerena,
                CardId.SPLittleKnight
            );

            BaitPlanner.RegisterComboStarters(
                CardId.ShiensSmokeSignal,
                CardId.ReinforcementOfTheArmy,
                CardId.ShiensDojo,
                CardId.Kageki
            );

            ChainAdvisor.RegisterHighValueTargets(
                CardId.BattleShogun,
                CardId.GatewayOfTheSix,
                CardId.LegendaryLordShiEn,
                CardId.LegendaryShiEn,
                CardId.LegendaryLordKizan
            );

            RegisterOptionalFieldRemovalCards(CardId.Mizuho);

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTOR PIPELINE (Prioritized Action Tree via Deck Plugin)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Absolute Counter Responses, Negations & Handtraps ──
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutEffect);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, NibiruEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLock, DrollEffect);

            // Board Breakers
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, HarpiesFeatherDusterEffect);

            // On-Field Quick Negations / Interruptions
            AddExecutor(ExecutorType.Activate, CardId.LegendaryLordShiEn, LordShiEnNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.LegendaryShiEn, ShiEnSpellTrapNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.LegendaryLordKizan, LordKizanEffect);
            AddExecutor(ExecutorType.Activate, CardId.TornadoDragon, TornadoDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, IPMasquerenaEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.NaturiaBeast, NaturiaBeastEffect);
            AddExecutor(ExecutorType.Activate, CardId.NaturiaBarkion, NaturiaBarkionEffect);
            AddExecutor(ExecutorType.Activate, CardId.FADawnDragster, DawnDragsterEffect);

            // ── Tier 1: Tactical Board Removal (Going 2nd Break & Targeted Pops) ──
            AddExecutor(ExecutorType.Activate, CardId.LegendaryLordEnishi, LordEnishiBounceEffect);
            AddExecutor(ExecutorType.Activate, CardId.Mizuho, MizuhoDestructionEffect);

            // ── Tier 2: Bushido Engine Anchors & Spell Search Routing ──
            AddExecutor(ExecutorType.Activate, CardId.GatewayOfTheSix, GatewayEffect);
            AddExecutor(ExecutorType.Activate, CardId.ShiensDojo, ShiensDojoEffect);
            AddExecutor(ExecutorType.Activate, CardId.ShiensSmokeSignal, SmokeSignalEffect);
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, ROTAEffect);
            AddExecutor(ExecutorType.Activate, CardId.AsceticismOfTheSixSamurai, AsceticismEffect);
            AddExecutor(ExecutorType.Activate, CardId.SixStrikeDoubleAssault, DoubleAssaultEffect);
            AddExecutor(ExecutorType.Activate, CardId.CunningOfTheSixSamurai, CunningEffect);

            // ── Tier 3: Triggered Searchers & Graveyard Floaters ──
            AddExecutor(ExecutorType.Activate, CardId.BattleShogun, BattleShogunLinkEffect);
            AddExecutor(ExecutorType.Activate, CardId.Kizaru, KizaruSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.TacticalTrainer, TacticalTrainerGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.AnarchistMonk, AnarchistMonkGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.LegendaryLordShiEn, LordShiEnSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.Hatsume, HatsumeReviveEffect);

            // ── Tier 4: Normal Summons (Starters & Baits) ──
            AddExecutor(ExecutorType.Summon, CardId.Kageki, KagekiSummon);
            AddExecutor(ExecutorType.Activate, CardId.Kageki, KagekiEffect);
            AddExecutor(ExecutorType.Summon, CardId.Kizaru, KizaruNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.TacticalTrainer, TrainerNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.AnarchistMonk, MonkNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Fuma, FumaNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.Kizan, KizanNormalSummon);

            // ── Tier 5: Extenders (Special Summons from Hand) ──
            AddExecutor(ExecutorType.SpSummon, CardId.TacticalTrainer, TacticalTrainerSpecialSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AnarchistMonk, AnarchistMonkSpecialSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Kizan, KizanSpecialSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Grandmaster, GrandmasterSpecialSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GreatShogunShien, GreatShogunSpecialSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Shinai, ShinaiSpecialSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Mizuho, MizuhoSpecialSummon);

            // ── Tier 6: Extra Deck Boss Summon Pipeline ──
            AddExecutor(ExecutorType.SpSummon, CardId.BattleShogun, BattleShogunSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LegendaryShiEn, ShiEnSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LegendaryLordShiEn, LordShiEnSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LegendaryLordKizan, LordKizanSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LegendaryLordEnishi, LordEnishiSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.NaturiaBarkion, NaturiaBarkionSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.NaturiaBeast, NaturiaBeastSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FADawnDragster, DawnDragsterSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TornadoDragon, TornadoDragonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SaryujaSkullDread, SaryujaSummon);

            // ── Tier 7: Backrow Setting ──
            AddExecutor(ExecutorType.SpellSet, CardId.SixStrikeDoubleAssault, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CrossoutDesignator, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CunningOfTheSixSamurai, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet);

            // ── Tier 8: Smart Monster Repositioning (Prevent 0/1800 Handtrap/Wall Beatdown Suicide) ──
            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            Plugin.ResetTurnState();
        }

        // ═══════════════════════════════════════════════════════════════
        //  ENGINE CALLBACKS: Delegated to SixSamuraiPlugin
        // ═══════════════════════════════════════════════════════════════

        public override IList<int> OnSelectCounter(int type, int quantity, IList<ClientCard> cards, IList<int> counters)
        {
            return Plugin?.CounterEconomy?.SelectCounters(quantity, cards, counters)
                ?? base.OnSelectCounter(type, quantity, cards, counters);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Material Selection (Link / Synchro / Xyz): Strict Ace Preservation & Fodder Selection
            if (hint == HINT_LMATERIAL || hint == HINT_SMATERIAL || hint == HINT_XMATERIAL)
            {
                var scored = Plugin.MaterialScorer.SortMaterials(cards, min);
                if (scored != null && scored.Count >= min)
                {
                    DecisionTracer.TraceSelect("MaterialScorer", "Selected Material Fodder", scored[0]);
                    return scored.Take(min).ToList();
                }
            }

            // 2. Discard Cost for Battle Shogun: Discard Shinai, Fuma, or duplicate cards
            if (hint == HINT_DISCARD)
            {
                var discardTarget = Plugin.MaterialScorer.PickDiscardTarget(cards, min);
                if (discardTarget != null && discardTarget.Count >= min)
                    return discardTarget;
            }

            // 3. Search to Hand (Audited Hint 506): Contextual Search via Kizaru / Gateway / Smoke Signal
            if (hint == HINT_ATOHAND)
            {
                var searchTarget = Plugin.KizaruResolver.PickSearchTarget(cards, LastChainCard);
                if (searchTarget != null && searchTarget.Count >= min)
                    return searchTarget.Take(Math.Min(max, searchTarget.Count)).ToList();
            }

            // 4. Special Summon Targets (Dojo / Double Assault / Hatsume)
            if (hint == HINT_SPSUMMON)
            {
                var ssTarget = Plugin.Strategy.PickSpecialSummonTarget(cards);
                if (ssTarget != null && ssTarget.Count >= min)
                    return ssTarget.Take(Math.Min(max, ssTarget.Count)).ToList();
            }

            // 5. Destruction Selection (Hint 502): Rule 1 Strict Separation
            if (hint == HINT_DESTROY)
            {
                // Target enemy cards if present
                if (cards.Any(c => c.Controller == 1))
                {
                    var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                    return Util.CheckSelectCount(enemyTargets, cards, min, max);
                }
                // Shi En / Lord Shi En Self-Destruction Replacement (only our own cards)
                if (cards.All(c => c.Controller == 0))
                {
                    var safeFodder = Plugin.MaterialScorer.PickDestructionSubstitute(cards, min);
                    if (safeFodder != null && safeFodder.Count >= min)
                        return safeFodder;
                }
            }

            // 6. Banish/Removal Selection (Hint 503):
            if (hint == HINT_REMOVE)
            {
                // Priority: Enemy cards if targeted removal
                if (cards.Any(c => c.Controller == 1))
                {
                    var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                    return Util.CheckSelectCount(enemyTargets, cards, min, max);
                }
                // Cost removal: Banish Six Strike Double Assault from GY for Lord Kizan
                var sixStrike = cards.FirstOrDefault(c => c.Id == CardId.SixStrikeDoubleAssault && c.Location == CardLocation.Grave);
                if (sixStrike != null)
                    return new List<ClientCard> { sixStrike };
            }

            // 7. Bounce Selection (Hint 505):
            if (hint == HINT_RTOHAND)
            {
                if (cards.Any(c => c.Controller == 1))
                {
                    var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                    return Util.CheckSelectCount(enemyTargets, cards, min, max);
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ═══════════════════════════════════════════════════════════════
        //  CARD EXECUTION METHODS (Driven by Strategy & Action Scorer)
        // ═══════════════════════════════════════════════════════════════

        private bool CrossoutEffect() => DefaultCrossoutDesignator();
        private bool AshBlossomEffect() => DefaultAshBlossomAndJoyousSpring();
        private bool ImpermanenceEffect() => DefaultInfiniteImpermanence();
        private bool EffectVeilerEffect() => DefaultEffectVeiler();
        private bool NibiruEffect() => DefaultNibiru();
        private bool GhostBelleEffect() => DefaultGhostBelleAndHauntedMansion();
        private bool DrollEffect() => DefaultDrollAndLockBird();
        private bool HarpiesFeatherDusterEffect() => DefaultHarpiesFeatherDusterFirst();

        private bool LordShiEnNegateEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            bool shouldNegate = LastChainCard != null && LastChainCard.HasType(CardType.Monster);
            if (shouldNegate)
                DecisionTracer.TraceActivate("LordShiEn", $"Negating opponent monster effect: {LastChainCard.Name ?? LastChainCard.Id.ToString()}");
            return shouldNegate;
        }

        private bool ShiEnSpellTrapNegateEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            bool shouldNegate = LastChainCard != null && (LastChainCard.HasType(CardType.Spell) || LastChainCard.HasType(CardType.Trap));
            if (shouldNegate)
                DecisionTracer.TraceActivate("LegendaryShiEn", $"Negating opponent S/T: {LastChainCard.Name ?? LastChainCard.Id.ToString()}");
            return shouldNegate;
        }

        private bool LordKizanEffect()
        {
            // Effect 1 (On Field Quick Effect): Banish Six Strike to destroy 1 card during opponent's Main Phase
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                {
                    bool hasSixStrikeCost = Bot.Graveyard.Any(c => c.Id == CardId.SixStrikeDoubleAssault) ||
                                            Bot.SpellZone.Any(c => c != null && c.Id == CardId.SixStrikeDoubleAssault);
                    if (hasSixStrikeCost && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0))
                    {
                        DecisionTracer.TraceActivate("LordKizan", "Quick Destroy: Banishing Six Strike to destroy opponent card");
                        return true;
                    }
                }
                return false;
            }

            // Effect 2 (In Graveyard): Self Special Summon if controlling 2+ Six Samurai
            if (Card.Location == CardLocation.Grave)
            {
                if (Bot.GetMonsters().Count < 5 && Bot.GetMonsters().Count(m => m.IsFaceup() && IsSixSamurai(m)) >= 2)
                {
                    DecisionTracer.TraceActivate("LordKizan", "Reviving Lord Kizan from Graveyard");
                    return true;
                }
            }

            return false;
        }

        private bool TornadoDragonEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.HasXyzMaterial())
            {
                var targets = Enemy.GetSpells().Where(s => s != null && (s.IsFacedown() || s.HasType(CardType.Continuous) || s.HasType(CardType.Field) || s.HasType(CardType.Equip))).ToList();
                return targets.Count > 0;
            }
            return false;
        }

        private bool IPMasquerenaEffect()
        {
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                return Bot.ExtraDeck.Any(c => c.Id == CardId.SPLittleKnight);
            }
            return false;
        }

        private bool NaturiaBeastEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return LastChainCard != null && LastChainCard.HasType(CardType.Spell) && Bot.Deck.Count >= 2;
        }

        private bool NaturiaBarkionEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return LastChainCard != null && LastChainCard.HasType(CardType.Trap) && Bot.Graveyard.Count >= 2;
        }

        private bool DawnDragsterEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return LastChainCard != null && (LastChainCard.HasType(CardType.Spell) || LastChainCard.HasType(CardType.Trap));
        }

        private bool SPLittleKnightEffect()
        {
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool LordEnishiBounceEffect()
        {
            if (Enemy.GetMonsterCount() == 0) return false;
            var gySixSam = Bot.Graveyard.Where(c => IsSixSamurai(c)).ToList();
            if (gySixSam.Count == 0) return false;
            DecisionTracer.TraceActivate("LordEnishi", $"Bouncing opponent monsters using {gySixSam.Count} banished Six Sams");
            return true;
        }

        private bool MizuhoDestructionEffect()
        {
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;
            return Plugin.MaterialScorer.HasExpendableFodder();
        }

        private bool GatewayEffect()
        {
            var card = Card;
            if (card.Location == CardLocation.Hand)
            {
                DecisionTracer.TraceActivate("GatewayOfTheSix", "Placing Gateway onto field from Hand");
                return true;
            }

            long descSearch = Util.GetStringId(CardId.GatewayOfTheSix, 1);
            long descAtk = Util.GetStringId(CardId.GatewayOfTheSix, 0);
            long descRevive = Util.GetStringId(CardId.GatewayOfTheSix, 2);

            // Effect 1: Search Six Samurai (Cost: 4 Bushido Counters) — HIGHEST ENGINE PRIORITY
            if (ActivateDescription == descSearch || (ActivateDescription == -1 && card.Location == CardLocation.SpellZone))
            {
                return Plugin.CounterEconomy.EvaluateGatewaySearch();
            }

            // Effect 0: ATK +500 (Cost: 2 Bushido Counters) — NEVER activate in Main Phase 1 setup (wastes search counters)
            if (ActivateDescription == descAtk)
            {
                if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
                {
                    if (Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.Attacked))
                    {
                        DecisionTracer.TraceActivate("GatewayOfTheSix", "Boosting Six Samurai ATK by 500 for battle");
                        return true;
                    }
                }
                return false;
            }

            // Effect 2: Special Summon Shien from GY (Cost: 6 Bushido Counters)
            if (ActivateDescription == descRevive)
            {
                bool hasTarget = Bot.Graveyard.Any(c => c.Id == CardId.LegendaryShiEn || 
                                                        c.Id == CardId.LegendaryLordShiEn || 
                                                        c.Id == CardId.GreatShogunShien);
                if (hasTarget && Bot.GetMonsters().Count < 5)
                {
                    DecisionTracer.TraceActivate("GatewayOfTheSix", "Reviving Shien from GY with 6 Bushido counters");
                    return true;
                }
                return false;
            }

            return false;
        }

        private bool ShiensDojoEffect()
        {
            var card = Card;
            if (card.Location == CardLocation.Hand) return true;

            // Send to GY to Special Summon from Deck if we need a key body
            if (Bot.GetMonsters().Count < 5 && Bot.Deck.Any(c => IsSixSamurai(c)))
            {
                DecisionTracer.TraceActivate("ShiensDojo", "Sending Dojo to GY to Special Summon Six Samurai from Deck");
                return true;
            }
            return false;
        }

        private bool SmokeSignalEffect()
        {
            return Bot.Deck.Any(c => IsSixSamurai(c) && c.Level <= 3);
        }

        private bool ROTAEffect()
        {
            return Bot.Deck.Any(c => c.HasRace(CardRace.Warrior) && c.Level <= 4);
        }

        private bool AsceticismEffect()
        {
            return Plugin.Strategy.EvaluateAsceticismTarget() != null;
        }

        private bool DoubleAssaultEffect()
        {
            if (Card.Location == CardLocation.Grave)
                return Bot.Graveyard.Any(c => IsSixSamurai(c));

            return Bot.GetMonsters().Count < 5 && (Bot.Hand.Any(c => IsSixSamurai(c) && c.Attack <= 2000) || Bot.Graveyard.Any(c => IsSixSamurai(c) && c.Attack <= 2000));
        }

        private bool CunningEffect()
        {
            if (!Bot.Graveyard.Any(c => IsSixSamurai(c))) return false;
            return Plugin.MaterialScorer.HasExpendableFodder();
        }

        private bool BattleShogunLinkEffect()
        {
            if (Plugin.Strategy.BattleShogunSearchUsed) return false;
            if (Bot.Hand.Count == 0) return false;
            Plugin.Strategy.BattleShogunSearchUsed = true;
            DecisionTracer.TraceActivate("BattleShogun", "Searching Bushido counter engine (Gateway/Dojo)");
            return true;
        }

        private bool KizaruSearchEffect() => true;
        private bool TacticalTrainerGYEffect() => true;
        private bool AnarchistMonkGYEffect() => true;

        private bool LordShiEnSearchEffect()
        {
            if (Plugin.Strategy.LordShiEnSearchUsed) return false;
            Plugin.Strategy.LordShiEnSearchUsed = true;
            DecisionTracer.TraceActivate("LordShiEn", "Triggering Synchro search for Six Samurai / Shien card");
            return true;
        }

        private bool HatsumeReviveEffect()
        {
            if (Bot.GetMonsters().Count >= 5) return false;
            var gySixSam = Bot.Graveyard.Where(c => IsSixSamurai(c)).ToList();
            return gySixSam.Count >= 2;
        }

        private bool KagekiSummon()
        {
            return Plugin.ActionScorer.ShouldSummonKageki();
        }

        private bool KagekiEffect() => true;

        private bool KizaruNormalSummon()
        {
            return Bot.GetMonsters().Count == 0 || Bot.HasInSpellZone(CardId.GatewayOfTheSix);
        }

        private bool TrainerNormalSummon() => Bot.GetMonsters().Count == 0;
        private bool MonkNormalSummon() => Bot.GetMonsters().Count == 0;
        private bool FumaNormalSummon() => Bot.GetMonsters().Count == 0;
        private bool KizanNormalSummon() => Bot.GetMonsters().Count == 0;

        private bool TacticalTrainerSpecialSummon()
        {
            if (Plugin.Strategy.TrainerSSUsed) return false;
            if (Bot.GetMonsters().Any(m => m.IsFaceup() && IsSixSamurai(m) && m.Id != CardId.TacticalTrainer))
            {
                Plugin.Strategy.TrainerSSUsed = true;
                DecisionTracer.TraceActivate("TacticalTrainer", "Special Summoning Lv2 Tuner from Hand");
                return true;
            }
            return false;
        }

        private bool AnarchistMonkSpecialSummon()
        {
            if (Plugin.Strategy.MonkSSUsed) return false;
            if (Bot.GetMonsters().Any(m => m.IsFaceup() && IsSixSamurai(m) && m.Id != CardId.AnarchistMonk))
            {
                Plugin.Strategy.MonkSSUsed = true;
                DecisionTracer.TraceActivate("AnarchistMonk", "Special Summoning Lv3 Tuner from Hand");
                return true;
            }
            return false;
        }

        private bool KizanSpecialSummon()
        {
            if (Bot.GetMonsters().Count >= 5) return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup() && IsSixSamurai(m));
        }

        private bool GrandmasterSpecialSummon()
        {
            if (Bot.GetMonsters().Count >= 5) return false;
            if (Bot.GetMonsters().Any(m => m.Id == CardId.Grandmaster)) return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup() && IsSixSamurai(m));
        }

        private bool GreatShogunSpecialSummon()
        {
            if (Bot.GetMonsters().Count >= 5) return false;
            int sixSamCount = Bot.GetMonsters().Count(m => m.IsFaceup() && IsSixSamurai(m));
            return sixSamCount >= 2;
        }

        private bool ShinaiSpecialSummon() => Bot.GetMonsters().Count < 5 && Bot.GetMonsters().Any(m => m.Id == CardId.Mizuho);
        private bool MizuhoSpecialSummon() => Bot.GetMonsters().Count < 5 && Bot.GetMonsters().Any(m => m.Id == CardId.Shinai);

        private bool BattleShogunSummon()
        {
            return Plugin.Strategy.ShouldSummonBattleShogun();
        }

        private bool ShiEnSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.LegendaryShiEn)) return false;
            return HasSynchroMaterials(5, true);
        }

        private bool LordShiEnSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.LegendaryLordShiEn)) return false;
            return HasSynchroMaterials(6, true);
        }

        private bool LordKizanSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.LegendaryLordKizan)) return false;
            return HasSynchroMaterials(6, false);
        }

        private bool LordEnishiSummon()
        {
            if (Enemy.GetMonsterCount() == 0) return false;
            return HasSynchroMaterials(6, false);
        }

        private bool NaturiaBarkionSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.NaturiaBarkion)) return false;
            // Requires EARTH Tuner (Tactical Trainer) + EARTH Non-Tuner (Kizan / Kizaru) = 6
            var earthTuners = Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasType(CardType.Tuner) && m.HasAttribute(CardAttribute.Earth) && !IsAceCard(m)).ToList();
            var earthNonTuners = Bot.GetMonsters().Where(m => m.IsFaceup() && !m.HasType(CardType.Tuner) && m.HasAttribute(CardAttribute.Earth) && !IsAceCard(m)).ToList();
            return earthTuners.Any(t => earthNonTuners.Any(nt => t.Level + nt.Level == 6));
        }

        private bool NaturiaBeastSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.NaturiaBeast)) return false;
            var earthTuners = Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasType(CardType.Tuner) && m.HasAttribute(CardAttribute.Earth) && !IsAceCard(m)).ToList();
            var earthNonTuners = Bot.GetMonsters().Where(m => m.IsFaceup() && !m.HasType(CardType.Tuner) && m.HasAttribute(CardAttribute.Earth) && !IsAceCard(m)).ToList();
            return earthTuners.Any(t => earthNonTuners.Any(nt => t.Level + nt.Level == 5));
        }

        private bool DawnDragsterSummon() => HasSynchroMaterials(7, false);

        private bool TornadoDragonSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.TornadoDragon)) return false;
            var lv4 = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && !IsAceCard(m)).ToList();
            return lv4.Count >= 2 && (Enemy.GetSpellCount() > 0 || Duel.Player == 1 || Duel.Phase == DuelPhase.Main2);
        }

        private bool IPMasquerenaSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.IPMasquerena)) return false;
            var nonLink = Bot.GetMonsters().Where(m => m.IsFaceup() && !m.HasType(CardType.Link) && !IsAceCard(m)).ToList();
            return nonLink.Count >= 2 && Bot.ExtraDeck.Any(c => c.Id == CardId.SPLittleKnight);
        }

        private bool SPLittleKnightSummon()
        {
            var nonAce = Bot.GetMonsters().Where(m => m.IsFaceup() && !IsAceCard(m)).ToList();
            return nonAce.Count >= 2 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        private bool SaryujaSummon()
        {
            var nonAce = Bot.GetMonsters().Where(m => m.IsFaceup() && !IsAceCard(m)).ToList();
            return nonAce.Count >= 4 && Bot.Hand.Count <= 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  UTILITY METHODS
        // ═══════════════════════════════════════════════════════════════

        public static bool IsSixSamurai(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.Kizan || card.Id == CardId.Kageki || card.Id == CardId.TacticalTrainer ||
                   card.Id == CardId.AnarchistMonk || card.Id == CardId.Fuma || card.Id == CardId.Hatsume ||
                   card.Id == CardId.Kizaru || card.Id == CardId.Grandmaster || card.Id == CardId.Mizuho ||
                   card.Id == CardId.Shinai || card.Id == CardId.Genba || card.Id == CardId.BattleShogun ||
                   card.Id == CardId.LegendaryShiEn || card.Id == CardId.LegendaryLordShiEn ||
                   card.Id == CardId.LegendaryLordEnishi || card.Id == CardId.LegendaryLordKizan;
        }

        private bool HasSynchroMaterials(int targetLevel, bool warriorTunerRequired)
        {
            var tuners = Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasType(CardType.Tuner) && (!warriorTunerRequired || m.HasRace(CardRace.Warrior)) && !IsAceCard(m)).ToList();
            var nonTuners = Bot.GetMonsters().Where(m => m.IsFaceup() && !m.HasType(CardType.Tuner) && !IsAceCard(m)).ToList();

            foreach (var t in tuners)
            {
                foreach (var nt in nonTuners)
                {
                    if (t.Level + nt.Level == targetLevel)
                        return true;
                }
            }
            return false;
        }

        private bool SmartMonsterRepos()
        {
            if (Card == null) return false;
            // 1. High DEF survival monsters / Handtraps stuck in Attack position
            if (Card.IsAttack() && (Card.Attack == 0 || (Card.Defense > Card.Attack && Card.Defense >= 1800)))
                return true;
            // 2. High ATK monsters accidentally in Defense when we can push for damage
            if (Card.IsDefense() && Card.Attack > Card.Defense && Card.Attack >= 1800 && Duel.Turn > 1)
                return true;
            return DefaultMonsterRepos();
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            var cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                // Link Monsters cannot be in Defense
                if (cardData.HasType(CardType.Link))
                    return CardPosition.FaceUpAttack;

                // 1. Handtraps (Ash 0/1800, Belle 0/1800, Veiler 0/0) or 0 ATK -> 100% Defense
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 2. High DEF / Wall (DEF > ATK and ATK < 1800, e.g. Fuma 200/1800) -> Defense
                if (cardData.Defense > cardData.Attack && cardData.Attack < 1800)
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 3. Boss / High ATK (ATK >= 1800) -> Attack
                if (cardData.Attack >= 1800 && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }

    // ═══════════════════════════════════════════════════════════════
}
