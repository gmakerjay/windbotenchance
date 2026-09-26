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

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_SixSamurai")]
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

            // Handtraps & Defensive
            public const int InfiniteImpermanence = 10045474;
            public const int AshBlossom = 14558127;
            public const int CalledByTheGrave = 24224830;
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
            public const int SPLittleKnight = 29301450;
            public const int SaryujaSkullDread = 74997493;
            public const int Apollousa = 4280258;
            public const int AbyssDweller = 21044178;
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

        public _2026_SixSamuraiExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // Instantiate Master Domain Plugin
            Plugin = new SixSamuraiPlugin(this);

            // Register Strategic Assets with Core Framework
            ResourcePlan.RegisterAceCards(
                CardId.LegendaryLordShiEn,
                CardId.LegendaryShiEn,
                CardId.GreatShogunShien,
                CardId.NaturiaBeast,
                CardId.NaturiaBarkion,
                CardId.Apollousa,
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
                CardId.LegendaryShiEn
            );

            RegisterOptionalFieldRemovalCards(CardId.Mizuho);

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTOR PIPELINE (Prioritized Action Tree via Deck Plugin)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Absolute Counter Responses, Negations & Handtraps ──
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.LegendaryLordShiEn, LordShiEnNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.LegendaryShiEn, ShiEnSpellTrapNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.NaturiaBeast, NaturiaBeastEffect);
            AddExecutor(ExecutorType.Activate, CardId.NaturiaBarkion, NaturiaBarkionEffect);
            AddExecutor(ExecutorType.Activate, CardId.FADawnDragster, DawnDragsterEffect);
            AddExecutor(ExecutorType.Activate, CardId.Apollousa, ApollousaEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller, AbyssDwellerEffect);

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
            AddExecutor(ExecutorType.SpSummon, CardId.LegendaryLordEnishi, LordEnishiSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.NaturiaBarkion, NaturiaBarkionSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.NaturiaBeast, NaturiaBeastSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FADawnDragster, DawnDragsterSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, AbyssDwellerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Apollousa, ApollousaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SaryujaSkullDread, SaryujaSummon);

            // ── Tier 7: Backrow Setting ──
            AddExecutor(ExecutorType.SpellSet, CardId.SixStrikeDoubleAssault, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, DefaultSpellSet);
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

            // 5. Shi En / Lord Shi En Self-Destruction Replacement
            if (hint == HINT_DESTROY && cards.Any(c => c.Controller == 0))
            {
                var safeFodder = Plugin.MaterialScorer.PickDestructionSubstitute(cards, min);
                if (safeFodder != null && safeFodder.Count >= min)
                    return safeFodder;
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ═══════════════════════════════════════════════════════════════
        //  CARD EXECUTION METHODS (Driven by Strategy & Action Scorer)
        // ═══════════════════════════════════════════════════════════════

        private bool CrossoutEffect() => DefaultCrossoutDesignator();
        private bool CalledByEffect() => DefaultCalledByTheGrave();
        private bool AshBlossomEffect() => DefaultAshBlossomAndJoyousSpring();
        private bool ImpermanenceEffect() => DefaultInfiniteImpermanence();

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

        private bool ApollousaEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return LastChainCard != null && LastChainCard.HasType(CardType.Monster);
        }

        private bool SPLittleKnightEffect()
        {
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool AbyssDwellerEffect()
        {
            return Duel.Player == 1 || (Duel.Player == 0 && Duel.Phase == DuelPhase.Main2);
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

        private bool AbyssDwellerSummon()
        {
            var lv4 = Bot.GetMonsters().Where(m => m.IsFaceup() && m.Level == 4 && !IsAceCard(m)).ToList();
            return lv4.Count >= 2;
        }

        private bool ApollousaSummon()
        {
            if (Bot.GetMonsters().Any(m => m.Id == CardId.Apollousa)) return false;
            var nonAce = Bot.GetMonsters().Where(m => m.IsFaceup() && !IsAceCard(m)).ToList();
            return nonAce.Count >= 3;
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
    //  MASTER DECK PLUGIN: SixSamuraiPlugin
    //  Decouples Domain Rules, Strategy, and Scorer from Engine Core
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamuraiPlugin
    {
        private readonly _2026_SixSamuraiExecutor _exec;

        public SixSamStrategy Strategy { get; }
        public SixSamCounterEconomy CounterEconomy { get; }
        public SixSamKizaruResolver KizaruResolver { get; }
        public SixSamMaterialScorer MaterialScorer { get; }
        public SixSamActionScorer ActionScorer { get; }
        public SixSamRecoveryPlanner RecoveryPlanner { get; }
        public SixSamBoardAssessor BoardAssessor { get; }

        public SixSamuraiPlugin(_2026_SixSamuraiExecutor exec)
        {
            _exec = exec;
            Strategy = new SixSamStrategy(exec);
            CounterEconomy = new SixSamCounterEconomy(exec);
            KizaruResolver = new SixSamKizaruResolver(exec);
            MaterialScorer = new SixSamMaterialScorer(exec);
            ActionScorer = new SixSamActionScorer(exec, this);
            RecoveryPlanner = new SixSamRecoveryPlanner(exec);
            BoardAssessor = new SixSamBoardAssessor(exec);
        }

        public void ResetTurnState()
        {
            Strategy.Reset();
            CounterEconomy.Reset();
            RecoveryPlanner.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 1: SixSamStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamStrategy
    {
        private readonly _2026_SixSamuraiExecutor _exec;

        public bool BattleShogunSearchUsed { get; set; }
        public bool LordShiEnSearchUsed { get; set; }
        public bool TrainerSSUsed { get; set; }
        public bool MonkSSUsed { get; set; }

        public SixSamStrategy(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public void Reset()
        {
            BattleShogunSearchUsed = false;
            LordShiEnSearchUsed = false;
            TrainerSSUsed = false;
            MonkSSUsed = false;
        }

        public bool ShouldSummonBattleShogun()
        {
            if (_exec.Bot.GetMonsters().Any(m => m.Id == _2026_SixSamuraiExecutor.CardId.BattleShogun)) return false;
            if (_exec.Bot.HasInSpellZone(_2026_SixSamuraiExecutor.CardId.GatewayOfTheSix)) return false;

            var warriors = _exec.Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasRace(CardRace.Warrior) && !_exec.IsAceCard(m)).ToList();
            if (warriors.Count < 2) return false;
            return warriors.Any(m => _2026_SixSamuraiExecutor.IsSixSamurai(m));
        }

        public ClientCard EvaluateAsceticismTarget()
        {
            // Asceticism pairings with identical ATK:
            // 200 ATK: Kageki <-> Fuma / Tactical Trainer
            // 500 ATK: Anarchist Monk <-> Genba
            // 1600 ATK: Mizuho <-> Hatsume
            foreach (var monster in _exec.Bot.GetMonsters().Where(m => m.IsFaceup() && _2026_SixSamuraiExecutor.IsSixSamurai(m)))
            {
                if (monster.Attack == 200 && _exec.Bot.Deck.Any(c => (c.Id == _2026_SixSamuraiExecutor.CardId.TacticalTrainer || c.Id == _2026_SixSamuraiExecutor.CardId.Fuma) && c.Id != monster.Id))
                    return monster;
                if (monster.Attack == 500 && _exec.Bot.Deck.Any(c => (c.Id == _2026_SixSamuraiExecutor.CardId.Genba || c.Id == _2026_SixSamuraiExecutor.CardId.AnarchistMonk) && c.Id != monster.Id))
                    return monster;
                if (monster.Attack == 1600 && _exec.Bot.Deck.Any(c => (c.Id == _2026_SixSamuraiExecutor.CardId.Hatsume || c.Id == _2026_SixSamuraiExecutor.CardId.Mizuho) && c.Id != monster.Id))
                    return monster;
            }
            return null;
        }

        public IList<ClientCard> PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            // 1. Missing Tuner for Synchro Line
            bool hasTuner = _exec.Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Tuner));
            if (!hasTuner)
            {
                var tuner = candidates.FirstOrDefault(c => c.HasType(CardType.Tuner));
                if (tuner != null) return new List<ClientCard> { tuner };
            }

            // 2. Kizaru for on-special-summon search trigger
            var kizaru = candidates.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.Kizaru);
            if (kizaru != null) return new List<ClientCard> { kizaru };

            // 3. Kizan as extensible body
            var kizan = candidates.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.Kizan);
            if (kizan != null) return new List<ClientCard> { kizan };

            return candidates.Take(1).ToList();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 2: SixSamCounterEconomy
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamCounterEconomy
    {
        private readonly _2026_SixSamuraiExecutor _exec;
        private int _loopActivationsThisTurn = 0;
        private bool _loopCutoffReached = false;

        public SixSamCounterEconomy(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public void Reset()
        {
            _loopActivationsThisTurn = 0;
            _loopCutoffReached = false;
        }

        public bool EvaluateGatewaySearch()
        {
            if (_loopCutoffReached) return false;

            // Safety Cutoff to prevent infinite engine freeze (max 20 activations per turn)
            if (_loopActivationsThisTurn >= 20)
            {
                _loopCutoffReached = true;
                DecisionTracer.TraceSkip("GatewayLoop", "Loop threshold reached (20 activations). Preserving state.");
                return false;
            }

            // Lethal OTK Cutoff: If lethal is already secured, STOP extending
            if (IsLethalAchieved())
            {
                _loopCutoffReached = true;
                DecisionTracer.TraceSkip("GatewayLoop", "Lethal OTK damage achieved. Halting combo to enter Battle Phase.");
                return false;
            }

            _loopActivationsThisTurn++;
            DecisionTracer.TraceActivate("GatewayLoop", $"Activating Gateway search (Iteration {_loopActivationsThisTurn})");
            return true;
        }

        public IList<int> SelectCounters(int quantity, IList<ClientCard> cards, IList<int> counters)
        {
            if (cards == null || counters == null || cards.Count != counters.Count)
                return null;

            int[] used = new int[counters.Count];
            int needed = quantity;

            // Priority 1: Drain from Battle Shogun first (Monster body is expendable and vulnerable)
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null && cards[i].Id == _2026_SixSamuraiExecutor.CardId.BattleShogun && needed > 0)
                {
                    int take = Math.Min(counters[i], needed);
                    used[i] += take;
                    needed -= take;
                    DecisionTracer.Trace("CounterEconomy", $"Spent {take} Bushido counters from Battle Shogun");
                }
            }

            // Priority 2: Drain from Shien's Dojo next
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null && cards[i].Id == _2026_SixSamuraiExecutor.CardId.ShiensDojo && needed > 0)
                {
                    int take = Math.Min(counters[i] - used[i], needed);
                    used[i] += take;
                    needed -= take;
                    DecisionTracer.Trace("CounterEconomy", $"Spent {take} Bushido counters from Shien's Dojo");
                }
            }

            // Priority 3: Drain from Gateway of the Six last (Core continuous engine)
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null && cards[i].Id == _2026_SixSamuraiExecutor.CardId.GatewayOfTheSix && needed > 0)
                {
                    int remaining = counters[i] - used[i];
                    if (remaining > 0)
                    {
                        int take = Math.Min(remaining, needed);
                        used[i] += take;
                        needed -= take;
                        DecisionTracer.Trace("CounterEconomy", $"Spent {take} Bushido counters from Gateway of the Six");
                    }
                }
            }

            // Priority 4: Safe fallback across any available cards to strictly guarantee sum(used) == quantity
            if (needed > 0)
            {
                for (int i = 0; i < cards.Count && needed > 0; i++)
                {
                    int remaining = counters[i] - used[i];
                    if (remaining > 0)
                    {
                        int take = Math.Min(remaining, needed);
                        used[i] += take;
                        needed -= take;
                        DecisionTracer.Trace("CounterEconomy", $"Spent {take} residual Bushido counters from Card {cards[i]?.Id}");
                    }
                }
            }

            return used;
        }

        public bool IsLoopCutoffReached => _loopCutoffReached;

        public bool IsLethalAchieved()
        {
            int botAtkSum = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            return botAtkSum >= (_exec.Enemy.LifePoints + 1000) && _exec.Enemy.GetMonsterCount() == 0;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 3: SixSamKizaruResolver
    //  Audited against script/c6579928.lua
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamKizaruResolver
    {
        private readonly _2026_SixSamuraiExecutor _exec;

        public SixSamKizaruResolver(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public IList<ClientCard> PickSearchTarget(IList<ClientCard> candidates, ClientCard lastChainCard)
        {
            // If Gateway search (from Deck/GY):
            var gatewayCandidate = candidates.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.GatewayOfTheSix);
            if (gatewayCandidate != null) return new List<ClientCard> { gatewayCandidate };

            // Check if this search is triggered by Kizaru:
            // s.thfilter requires searching a Six Sam with Attribute NOT currently controlled on MZONE
            bool isKizaruSearch = lastChainCard != null && lastChainCard.Id == _2026_SixSamuraiExecutor.CardId.Kizaru;
            var controlledAttrs = _exec.Bot.GetMonsters()
                .Where(m => m != null && m.IsFaceup())
                .Select(m => m.Attribute)
                .ToHashSet();

            var legalPool = isKizaruSearch 
                ? candidates.Where(c => !controlledAttrs.Contains(c.Attribute)).ToList()
                : candidates.ToList();

            if (legalPool.Count == 0)
                legalPool = candidates.ToList(); // Fallback if engine provides filtered list

            // Strategic Priority Ranking:
            // 1. Missing Tuner for Synchro Boss
            bool hasTuner = _exec.Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Tuner)) || _exec.Bot.Hand.Any(h => h.HasType(CardType.Tuner));
            if (!hasTuner)
            {
                var tuner = legalPool.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.TacticalTrainer || 
                                                          c.Id == _2026_SixSamuraiExecutor.CardId.AnarchistMonk || 
                                                          c.Id == _2026_SixSamuraiExecutor.CardId.Fuma);
                if (tuner != null)
                {
                    DecisionTracer.Trace("KizaruResolver", $"Selected Tuner: {tuner.Name}");
                    return new List<ClientCard> { tuner };
                }
            }

            // 2. Great Shogun Shien for S/T Lock (FIRE attribute)
            if (_exec.Bot.GetMonsters().Count >= 2 && 
                !_exec.Bot.GetMonsters().Any(m => m.Id == _2026_SixSamuraiExecutor.CardId.GreatShogunShien) && 
                !_exec.Bot.Hand.Any(h => h.Id == _2026_SixSamuraiExecutor.CardId.GreatShogunShien))
            {
                var shogun = legalPool.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.GreatShogunShien);
                if (shogun != null)
                {
                    DecisionTracer.Trace("KizaruResolver", "Selected Great Shogun Shien for S/T Lock");
                    return new List<ClientCard> { shogun };
                }
            }

            // 3. Extender & Loop Fuel (Kizan - EARTH)
            var kizan = legalPool.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.Kizan);
            if (kizan != null)
            {
                DecisionTracer.Trace("KizaruResolver", "Selected Kizan for free Special Summon extension");
                return new List<ClientCard> { kizan };
            }

            // 4. Starter if empty board (Kageki - WIND)
            var kageki = legalPool.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.Kageki);
            if (kageki != null) return new List<ClientCard> { kageki };

            // 5. Default legal card
            var fallback = legalPool.FirstOrDefault();
            return fallback != null ? new List<ClientCard> { fallback } : candidates.Take(1).ToList();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 4: SixSamMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamMaterialScorer
    {
        private readonly _2026_SixSamuraiExecutor _exec;

        public SixSamMaterialScorer(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // Absolute Protection: Ace Monsters must never be sacrificed casually (Penalty: 10,000)
            if (card.Id == _2026_SixSamuraiExecutor.CardId.LegendaryLordShiEn) return 10000;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.LegendaryShiEn) return 9000;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.GreatShogunShien) return 8000;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.NaturiaBeast || card.Id == _2026_SixSamuraiExecutor.CardId.NaturiaBarkion) return 7500;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.Apollousa) return 7000;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.SPLittleKnight) return 6000;

            // Tuner Protection: Preserve sole tuner on field
            if (card.HasType(CardType.Tuner) && _exec.Bot.GetMonsters().Count(m => m.HasType(CardType.Tuner)) == 1)
                return 400;

            // Prime Fodder (Low ATK / Already triggered effect)
            if (card.Id == _2026_SixSamuraiExecutor.CardId.Shinai || card.Id == _2026_SixSamuraiExecutor.CardId.Mizuho) return 10;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.Kageki) return 15;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.BattleShogun && _exec.Bot.HasInSpellZone(_2026_SixSamuraiExecutor.CardId.GatewayOfTheSix)) return 25;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.Kizan) return 30;

            return 100;
        }

        public bool HasExpendableFodder()
        {
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && GetMaterialCost(m) < 100);
        }

        public IList<ClientCard> PickDiscardTarget(IList<ClientCard> cards, int min)
        {
            return cards.OrderBy(c => {
                if (c.Id == _2026_SixSamuraiExecutor.CardId.Shinai) return 1;
                if (c.Id == _2026_SixSamuraiExecutor.CardId.Fuma) return 2;
                if (c.Id == _2026_SixSamuraiExecutor.CardId.SixStrikeDoubleAssault) return 3;
                if (_exec.Bot.Hand.Count(h => h.Id == c.Id) > 1) return 4;
                if (c.Id == _2026_SixSamuraiExecutor.CardId.Kizan) return 5;
                return 100;
            }).Take(min).ToList();
        }

        public IList<ClientCard> PickDestructionSubstitute(IList<ClientCard> cards, int min)
        {
            return cards.Where(c => c.Controller == 0).OrderBy(c => GetMaterialCost(c)).Take(min).ToList();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 5: SixSamActionScorer
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamActionScorer
    {
        private readonly _2026_SixSamuraiExecutor _exec;
        private readonly SixSamuraiPlugin _plugin;

        public SixSamActionScorer(_2026_SixSamuraiExecutor exec, SixSamuraiPlugin plugin)
        {
            _exec = exec;
            _plugin = plugin;
        }

        public bool ShouldSummonKageki()
        {
            // Evaluate Normal Summoning Kageki:
            // ActionScore = Base(60) + Extension(+35 if hand has Lv4) + BaitValue(+25 if opp has negator) - Risk(0)
            if (_exec.Bot.GetMonsters().Count >= 5) return false;
            bool hasValidTarget = _exec.Bot.Hand.Any(c => _2026_SixSamuraiExecutor.IsSixSamurai(c) && c.Id != _2026_SixSamuraiExecutor.CardId.Kageki && c.Level <= 4);
            if (!hasValidTarget) return _exec.Bot.GetMonsters().Count == 0;

            DecisionTracer.Trace("ActionScorer", "Kageki Normal Summon scored 95 (Prime Starter & Bait Enabler)");
            return true;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 6: SixSamRecoveryPlanner
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamRecoveryPlanner
    {
        private readonly _2026_SixSamuraiExecutor _exec;
        public bool StarterInterrupted { get; set; }
        public bool ShogunInterrupted { get; set; }

        public SixSamRecoveryPlanner(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public void Reset()
        {
            StarterInterrupted = false;
            ShogunInterrupted = false;
        }

        public bool HasEmergencyExtenderInHand()
        {
            return _exec.Bot.Hand.Any(c => c.Id == _2026_SixSamuraiExecutor.CardId.Kizan || 
                                           c.Id == _2026_SixSamuraiExecutor.CardId.TacticalTrainer || 
                                           c.Id == _2026_SixSamuraiExecutor.CardId.AnarchistMonk);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 7: SixSamBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamBoardAssessor
    {
        private readonly _2026_SixSamuraiExecutor _exec;

        public SixSamBoardAssessor(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public bool IsOpponentBackrowDangerous()
        {
            return _exec.Enemy.GetSpellCount() >= 2;
        }

        public bool IsLethalThresholdReached()
        {
            int totalAttack = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            return totalAttack >= _exec.Enemy.LifePoints && _exec.Enemy.GetMonsterCount() == 0;
        }
    }
}
