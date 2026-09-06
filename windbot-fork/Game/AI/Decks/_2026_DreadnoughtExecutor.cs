// ============================================================
// CARD AUDIT โ€” 2026_Dreadnought
// ============================================================
// | Card Name           | Type       | OPT? | HOPT? | Cost | Effect Summary           | Activate When                        | NEVER Activate When                          | Can Be Negated By |
// |---------------------|------------|------|-------|------|--------------------------|--------------------------------------|----------------------------------------------|-------------------|
// | Ash Blossom         | Hand Trap  | Yes  | Yes   | None | Negate search/SS from deck| Opponent activates search/SS         | Board is strong; save for key opponent play  | Called By, Crossout|
// | Fuwalos             | Hand Trap  | Yes  | Yes   | None | Draw on opponent SS      | Opponent summons, empty field        | Already have enough cards/field is full      | Called By, Ash    |
// | Sabatiel            | Spell      | Yes  | No    | None | Search Fusion spell      | Hand has no Fusion spell             | LP <= 2000 or already used                   | Ash Blossom       |
// | ROTA                | Spell      | Yes  | No    | None | Search Level 4 Warrior   | Need combo starter (Vyon/Mist)       | Already have all starters                    | Ash Blossom       |
// | Fusion Destiny      | Spell      | Yes  | Yes   | None | Deck Fusion DPE/Dominance| Special summon not blocked           | Bosses already on field                      | Ash Blossom       |
// | Polymerization      | Spell      | No   | No    | None | Fusion summon            | Hand has materials                   | Special summon blocked                       | Ash Blossom       |
// | Mask Change         | Quick Spell| No   | No    | None | Tribute DARK for Dark Law| Control face-up DARK HERO            | Special summon blocked                       | Ash Blossom       |
// | Dreadnought Servant | Monster    | Yes  | Yes   | None | Search Poly/SS from hand | Control Destiny HERO or Clock Tower  | Already have Poly in hand                    | Ash, Veiler, Imperm|
// | Doom Liege          | Monster    | Yes  | Yes   | None | Search Clock Tower/Send  | Special Summoned                     | Already have all pieces                      | Ash, Veiler, Imperm|
// | Death Dogma         | Monster    | Yes  | Yes   | None | Banish 3 GY for SS       | Banishing 3 Warrior/DARK from GY     | Need materials in GY for Fusion              | Crow, Belle       |
// | Dreadnought         | Monster    | No   | No    | None | Send 2 HEROs from deck   | Special Summoned                     | Deck has no targets                          | Ash, Veiler       |
// | Plasma              | Monster    | No   | No    | None | Negate face-up enemy effs| Control 3 tribute monsters           | Opponent controls no face-up monsters        | Veiler, Imperm    |
// | DPE                 | Monster    | Yes  | Yes   | None | Destroy 1 our + 1 enemy  | Opponent controls cards              | Board is clean                               | Called By, Crow   |
// | Solemn Report       | CounterTrap| No   | No    | LP   | Negate Spell/Trap        | Opponent activates Spell/Trap        | Opponent controls no key targets             | Counter Traps     |
// | Dominus Spark       | Trap       | Yes  | Yes   | None | Banish enemy monster     | Opponent activated monster in hand/GY| We have Earth/Water/Fire/Wind in hand to use | Trap negators     |
// | D-Burst             | Spell      | Yes  | Yes   | None | Destroy 1 our to draw 1  | Control face-up monster              | No expendable monsters on field              | Spell negators    |
// ============================================================
// ACE CARDS (MUST PROTECT):
//   Primary  : Destiny HERO - Destroyer Phoenix Enforcer โ€” quick-play recursive destruction
//   Secondary: Destiny HERO - Plasma โ€” negates opponent monster effects on field
//   Tertiary : Destiny HERO - Dreadnought โ€” deck-dumping extender and huge ATK body
// 
// COMBO STARTERS (Priority Order):
//   1. Vision HERO Vyon โ€” sends Malicious, searches Polymerization
//   2. Elemental HERO Shadow Mist โ€” searches Mask Change on special summon, searches HERO on GY send
//   3. Destiny HERO - Doom Liege โ€” searches Clock Tower and special summons a DARK HERO
//
// EXTENDERS:
//   - Destiny HERO - Malicious โ€” banishes to Special Summon another copy
//   - Destiny HERO - Denier โ€” Special Summons itself and recycles banished Malicious
//   - Destiny HERO - Dreadnought Servant โ€” Special Summons itself from hand
// ============================================================

using System;
using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Dreadnought", "2026_Dreadnought")]
    public class _2026_DreadnoughtExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int SabatielThePhilosophersStone = 40237839;
            public const int DestinyHERODogma = 17132130;
            public const int DestinyHERODeathDogma = 101402021;
            public const int DestinyHEROPlasma = 83965310;
            public const int DestinyHERODreadmaster = 40591390;
            public const int MaskedHEROFurnace = 58288218;
            public const int DestinyHEROMalicious = 9411399;
            public const int MaskedHERODuskCrow = 10808715;
            public const int ElementalHEROShadowMist = 50720316;
            public const int MaskedHEROFountain = 66206748;
            public const int VisionHEROVyon = 27780618;
            public const int MulcharmyFuwalos = 42141493;
            public const int DestinyHERODenier = 16605586;
            public const int DestinyHERODoomLiege = 101402022;
            public const int DestinyHERODreadnoughtServant = 101402023;
            public const int AshBlossom = 14558127;
            public const int Polymerization = 24094653;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int FusionDestiny = 52947044;
            public const int Terraforming = 73628505;
            public const int FoolishBurial = 81439173;
            public const int DBurst = 100456010;
            public const int MaskChange = 21143940;
            public const int CalledByTheGrave = 24224830;
            public const int DForce = 6186304;
            public const int ClockTowerPrisonCityDarkCity = 101402062;
            public const int InfiniteImpermanence = 10045474;
            public const int SolemnAccusation = 78114463;
            // Extra Deck
            public const int DestinyHERODusktopia = 93657021;
            public const int DestinyHERODominance = 69394324;
            public const int ContrastHEROChaos = 23204029;
            public const int DestinyHERODystopia = 90579153;
            public const int DestinyHERODestroyerPhoenixEnforcer = 60461804;
            public const int StarvingVenomFusionDragon = 46759931;
            public const int DestinyHERODreadnought = 101402037;
            public const int MaskedHERODarkLaw = 58481572;
            public const int DestinyHERODangerous = 30757127;
            public const int XtraHERODreadDecimator = 63813056;
            public const int XtraHEROWonderDriver = 1948619;
            public const int XtraHEROCrossCrusader = 58004362;
            // Side Deck
            public const int DrollAndLockBird = 94145021;
            public const int ElementalHEROShiningFlareWingman = 87758525;
            public const int ElementalHEROFlameWingmanInfernalRage = 13243124;
            public const int MudragonOfTheSwamp = 54757758;
            public const int ForbiddenDroplet = 24299458;
            public const int SuperPolymerization = 48130397;
            public const int DominusSpark = 6325660;
        }

        // Once-per-turn tracking
        private bool _sabatielUsed = false;
        private bool _terraformingUsed = false;
        private bool _dForceUsed = false;
        private bool _clockTowerSpellActivated = false;
        private bool _clockTowerEffectUsed = false;

        public override bool OnSelectHand()
        {
            // HERO OTK / DPE โ€” prefer going second to break board and OTK
            return false;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _sabatielUsed = false;
            _terraformingUsed = false;
            _dForceUsed = false;
            _clockTowerSpellActivated = false;
            _clockTowerEffectUsed = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        private static readonly int[] AceCardIds = {
            CardId.DestinyHEROPlasma,
            CardId.DestinyHERODestroyerPhoenixEnforcer,
            CardId.MaskedHERODarkLaw,
            CardId.DestinyHERODreadnought,
            CardId.DestinyHERODreadmaster,
            CardId.DestinyHERODusktopia,
            CardId.ContrastHEROChaos,
            CardId.DestinyHERODominance,
            CardId.DestinyHERODystopia
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Any(id => card.IsCode(id));
        }
        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.DestinyHERODestroyerPhoenixEnforcer)) return true;
            if (Bot.HasInMonstersZone(CardId.DestinyHEROPlasma)) return true;
            if (Bot.HasInMonstersZone(CardId.DestinyHERODreadnought)) return true;
            return base.IsBoardStrongEnough();
        }
        protected override bool ShouldStopExtending()
        {
            if (Bot.HasInMonstersZone(CardId.DestinyHERODestroyerPhoenixEnforcer)
                || Bot.HasInMonstersZone(CardId.DestinyHEROPlasma))
                return base.ShouldStopExtending();
            return false;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;

            if (c.Id == CardId.DestinyHEROMalicious) return 50;
            if (c.Id == CardId.DestinyHERODenier) return 60;
            if (c.Id == CardId.DestinyHERODreadnoughtServant) return 100;
            if (c.Id == CardId.VisionHEROVyon) return 200;
            if (c.Id == CardId.ElementalHEROShadowMist) return 250;

            return base.GetMaterialPriority(c);
        }

        public _2026_DreadnoughtExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Vyon-Play",
                RequiredCards = new List<int> { CardId.VisionHEROVyon },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.VisionHEROVyon, ActionType = ExecutorType.Summon, Description = "Normal Summon Vyon" },
                    new() { CardId = CardId.VisionHEROVyon, ActionType = ExecutorType.Activate, Description = "Activate Vyon effect" },
                    new() { CardId = CardId.DestinyHEROMalicious, ActionType = ExecutorType.Activate, Description = "Banish Malicious to SS" },
                    new() { CardId = CardId.XtraHEROCrossCrusader, ActionType = ExecutorType.SpSummon, Description = "Link Summon Cross Crusader" },
                    new() { CardId = CardId.XtraHEROCrossCrusader, ActionType = ExecutorType.Activate, Description = "Activate Cross Crusader search" },
                    new() { CardId = CardId.FusionDestiny, ActionType = ExecutorType.Activate, Description = "Activate Fusion Destiny" },
                    new() { CardId = CardId.DestinyHERODestroyerPhoenixEnforcer, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon DPE" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "FusionDestiny-Play",
                RequiredCards = new List<int> { CardId.FusionDestiny },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.FusionDestiny, ActionType = ExecutorType.Activate, Description = "Activate Fusion Destiny" },
                    new() { CardId = CardId.DestinyHERODestroyerPhoenixEnforcer, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon DPE" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.FusionDestiny, CardId.VisionHEROVyon);
            BaitPlanner.RegisterBaitCards(CardId.ReinforcementOfTheArmy, CardId.Terraforming, CardId.SabatielThePhilosophersStone, CardId.DestinyHERODreadnoughtServant);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.FusionDestiny, CardId.ReinforcementOfTheArmy);

            // ===== Hand Traps =====
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, () => SmartHandTrapChain() && DefaultDontChainMyself());
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);

            // ===== DPE Quick Effect (highest priority โ€” activate during opponent's turn) =====
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODestroyerPhoenixEnforcer, DPEEffect);

            // ===== Fusion Spells (bring out DPE before searching) =====
            AddExecutor(ExecutorType.Activate, CardId.FusionDestiny, FusionDestinyEffect);
            AddExecutor(ExecutorType.Activate, CardId.Polymerization, PolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.MaskChange, MaskChangeEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);

            // ===== Spells: Search =====
            AddExecutor(ExecutorType.Activate, CardId.SabatielThePhilosophersStone, SabatielEffect);
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingEffect);
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, ReinforcementOfTheArmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClockTowerPrisonCityDarkCity, ClockTowerEffect);
            AddExecutor(ExecutorType.Activate, CardId.DForce, DForceEffect);

            // ===== Monster Effects: Hand/GY =====
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODreadnoughtServant, DreadnoughtServantEffect);
            AddExecutor(ExecutorType.Activate, CardId.MaskedHEROFurnace, FurnaceEffect);
            AddExecutor(ExecutorType.Activate, CardId.MaskedHEROFountain, FountainEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHEROMalicious, MaliciousEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODenier, DenierEffect);

            // ===== Special Summons =====
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODreadnought, DreadnoughtSummon);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODreadnought, DreadnoughtFieldEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHEROPlasma, PlasmaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODeathDogma, DeathDogmaSummon);

            // ===== Normal Summons =====
            AddExecutor(ExecutorType.Summon, SafeTributeCheck);
            AddExecutor(ExecutorType.SummonOrSet, SafeTributeCheck);
            AddExecutor(ExecutorType.Summon, CardId.VisionHEROVyon);
            AddExecutor(ExecutorType.Summon, CardId.DestinyHERODreadnoughtServant);
            AddExecutor(ExecutorType.Summon, CardId.DestinyHERODoomLiege);
            AddExecutor(ExecutorType.Summon, CardId.MaskedHEROFountain);
            AddExecutor(ExecutorType.Summon, CardId.ElementalHEROShadowMist);
            AddExecutor(ExecutorType.Summon, CardId.MaskedHEROFurnace);
            AddExecutor(ExecutorType.Summon, CardId.MaskedHERODuskCrow);
            AddExecutor(ExecutorType.Summon, CardId.DestinyHERODenier);
            AddExecutor(ExecutorType.MonsterSet, CardId.AshBlossom, HandtrapSet);

            // ===== Monster Field Effects =====
            AddExecutor(ExecutorType.Activate, CardId.VisionHEROVyon, VyonEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODoomLiege, DoomLiegeEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElementalHEROShadowMist, ShadowMistEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHEROPlasma, PlasmaEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODeathDogma, DeathDogmaEffect);

            // ===== Extra Deck: Link (MP2 if possible) =====
            AddExecutor(ExecutorType.SpSummon, CardId.XtraHEROCrossCrusader, CrossCrusaderSummon);
            AddExecutor(ExecutorType.Activate, CardId.XtraHEROCrossCrusader, CrossCrusaderEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.XtraHEROWonderDriver, WonderDriverSummon);
            AddExecutor(ExecutorType.Activate, CardId.XtraHEROWonderDriver, WonderDriverEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.XtraHERODreadDecimator, DreadDecimatorSummon);
            AddExecutor(ExecutorType.Activate, CardId.XtraHERODreadDecimator, DreadDecimatorEffect);

            // ===== Extra Deck: Fusions =====
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODestroyerPhoenixEnforcer);
            // DPE Activate is registered above (highest priority)
            AddExecutor(ExecutorType.SpSummon, CardId.MaskedHERODarkLaw);
            AddExecutor(ExecutorType.Activate, CardId.MaskedHERODarkLaw, DarkLawEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODominance);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODominance, DominanceEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODystopia);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODystopia, DystopiaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODusktopia);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODusktopia, DusktopiaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ContrastHEROChaos);
            AddExecutor(ExecutorType.Activate, CardId.ContrastHEROChaos, ContrastHEROChaosEffect);

            // ===== Traps =====
            AddExecutor(ExecutorType.Activate, CardId.DBurst, DBurstEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnAccusation, AccusationEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusSpark, DominusSparkEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnAccusation);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusSpark);
            AddExecutor(ExecutorType.SpellSet, CardId.MaskChange);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        // โ•โ•โ• OTK & Phase Logic โ•โ•โ•

        // FieldGuard inherited: IsSpecialSummonBlocked, CanDealLethal, CanOTK โ’ inherited

        private bool ExtraDeckNow()
        {
            if (Duel.Turn == 1) return true;
            if (CanOTK()) return false; // Attack first!
            if (Duel.Phase == DuelPhase.Main1 && Bot.HasAttackingMonster()) return false;
            return true;
        }

        private ClientCard GetPreemptiveImpermTarget()
        {
            int[] threatIds = {
                21522601, // Witchcrafter Madame Verre
                84523092, // Witchcrafter Haine
                1561110,  // ABC-Dragon Buster
                4280258,  // Apollousa, Bow of the Goddess
                10443957, // Cyber Dragon Infinity
                84815190, // Baronne de Fleur
                1508649   // Altergeist Hexstia
            };

            return Enemy.MonsterZone.GetMonsters().FirstOrDefault(c => 
                c != null && c.IsFaceup() && !c.IsDisabled() && 
                threatIds.Contains(c.Id) && 
                !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
        }

        private bool InfiniteImpermanenceEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = GetPreemptiveImpermTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return DefaultInfiniteImpermanence();
        }

        // โ•โ•โ• Hand Traps โ•โ•โ•

        private bool MulcharmyEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        // โ•โ•โ• Spells โ•โ•โ•

        private bool SabatielEffect()
        {
            if (_sabatielUsed) return false;
            // Don't pay half LP if we're low
            if (Bot.LifePoints <= 2000) return false;
            _sabatielUsed = true;
            return true;
        }

        private bool TerraformingEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_terraformingUsed) return false;
            if (Bot.HasInSpellZone(CardId.ClockTowerPrisonCityDarkCity)) return false;
            _terraformingUsed = true;
            AI.SelectCard(CardId.ClockTowerPrisonCityDarkCity);
            return true;
        }

        private bool ReinforcementOfTheArmyEffect()
        {
            if (ShouldSkipCombo()) return false;
            bool hasField = Bot.Hand.Any(c => c != null && c.Id == CardId.ClockTowerPrisonCityDarkCity) || Bot.HasInSpellZone(CardId.ClockTowerPrisonCityDarkCity);
            bool hasServant = Bot.Hand.Any(c => c != null && c.Id == CardId.DestinyHERODreadnoughtServant);
            if (hasField && !hasServant) { AI.SelectCard(CardId.DestinyHERODreadnoughtServant); return true; }
            AI.SelectCard(CardId.VisionHEROVyon, CardId.ElementalHEROShadowMist, CardId.MaskedHERODuskCrow, CardId.MaskedHEROFountain, CardId.DestinyHERODreadnoughtServant, CardId.DestinyHERODoomLiege, CardId.DestinyHERODenier);
            return true;
        }

        private bool FoolishBurialEffect()
        {
            if (ShouldSkipCombo()) return false;
            int malCount = Bot.Graveyard.Count(c => c != null && c.Id == CardId.DestinyHEROMalicious) + Bot.Hand.Count(c => c != null && c.Id == CardId.DestinyHEROMalicious);
            if (malCount == 0) { AI.SelectCard(CardId.DestinyHEROMalicious); return true; }
            AI.SelectCard(CardId.ElementalHEROShadowMist, CardId.DestinyHERODenier, CardId.DestinyHEROMalicious);
            return true;
        }

        private bool ClockTowerEffect()
        {
            if (Card == null) return false;
            
            // 1. Activating the card from Hand
            if (Card.Location == CardLocation.Hand)
            {
                if (_clockTowerSpellActivated) return false;
                if (Bot.HasInSpellZone(Card.Id)) return false; // Don't activate duplicate
                _clockTowerSpellActivated = true;
                return true;
            }
            
            // 2. Activating the search effect on the Field
            if (Card.Location == CardLocation.SpellZone)
            {
                if (_clockTowerEffectUsed) return false;
                _clockTowerEffectUsed = true;
                AI.SelectCard(CardId.DestinyHERODreadnoughtServant, CardId.DestinyHERODoomLiege, CardId.DestinyHERODeathDogma, CardId.DestinyHERODreadnought);
                return true;
            }
            
            return false;
        }

        private bool DForceEffect()
        {
            if (_dForceUsed) return false;
            if (Bot.HasInSpellZone(CardId.DForce)) return false;
            _dForceUsed = true;
            AI.SelectCard(CardId.DestinyHEROPlasma);
            return true;
        }

        // โ•โ•โ• Fusion Spells โ•โ•โ•

        private bool FusionDestinyEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;
            // Bait check!
            var bait = GetBaitIfNeeded(Card);
            if (bait != null) return false;
            if (GetRemainingCount(CardId.DestinyHERODestroyerPhoenixEnforcer) > 0)
                AI.SelectCard(CardId.DestinyHERODestroyerPhoenixEnforcer);
            else
                AI.SelectCard(CardId.DestinyHERODominance, CardId.DestinyHERODystopia);
            return true;
        }

        private bool PolymerizationEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            
            var availablemats = Bot.Hand.Concat(Bot.GetMonsters()).Where(c => c != null && c.IsMonster()).ToList();
            var safemats = availablemats.Where(c => !IsAceCard(c)).ToList();
            
            bool canSummonDPESafely = false;
            var destinyHeroes = safemats.Where(c => c.HasSetcode(0xc008)).ToList();
            var lvl6Heroes = safemats.Where(c => c.Level >= 6 && c.HasSetcode(0x8)).ToList();
            foreach (var dh in destinyHeroes)
            {
                var other = lvl6Heroes.FirstOrDefault(lh => lh != dh);
                if (other != null)
                {
                    canSummonDPESafely = true;
                    break;
                }
            }
            
            bool canSummonDreadnoughtSafely = safemats.Count(c => c.HasSetcode(0xc008)) >= 2;
            bool canSummonDominanceSafely = safemats.Count(c => c.HasSetcode(0xc008)) >= 3;
            bool canSummonDystopiaSafely = safemats.Count(c => c.HasSetcode(0xc008)) >= 2;
            
            if (canSummonDPESafely && GetRemainingCount(CardId.DestinyHERODestroyerPhoenixEnforcer) > 0)
            {
                AI.SelectCard(CardId.DestinyHERODestroyerPhoenixEnforcer);
                return true;
            }
            if (canSummonDreadnoughtSafely && GetRemainingCount(CardId.DestinyHERODreadnought) > 0)
            {
                AI.SelectCard(CardId.DestinyHERODreadnought);
                return true;
            }
            if (canSummonDominanceSafely && GetRemainingCount(CardId.DestinyHERODominance) > 0)
            {
                AI.SelectCard(CardId.DestinyHERODominance);
                return true;
            }
            if (canSummonDystopiaSafely && GetRemainingCount(CardId.DestinyHERODystopia) > 0)
            {
                AI.SelectCard(CardId.DestinyHERODystopia);
                return true;
            }
            
            return false;
        }

        private bool MaskChangeEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (GetRemainingCount(CardId.MaskedHERODarkLaw) == 0) return false;
            var targets = Bot.GetMonsters().Where(c => c != null && c.HasAttribute(CardAttribute.Dark) && c.IsFaceup()).ToList();
            if (targets.Count == 0) return false;
            var bestTarget = targets.FirstOrDefault(c => c != null && c.Id == CardId.ElementalHEROShadowMist)
                          ?? targets.FirstOrDefault(c => c != null && c.Id == CardId.VisionHEROVyon)
                          ?? targets.FirstOrDefault(c => c != null && c.Id == CardId.DestinyHEROMalicious)
                          ?? targets.FirstOrDefault(c => c != null && c.Id == CardId.DestinyHERODenier)
                          ?? targets.FirstOrDefault(c => c != null && c.Id == CardId.DestinyHERODreadnoughtServant)
                          ?? targets.FirstOrDefault(c => c != null && c.Id == CardId.DestinyHERODoomLiege);
            if (bestTarget != null) 
            { 
                if (CanOTK())
                    AI.SelectPosition(CardPosition.FaceUpAttack);
                else
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                AI.SelectCard(bestTarget); 
                return true; 
            }
            var fallback = targets.FirstOrDefault(c => c != null && !IsAceCard(c));
            if (fallback != null) 
            { 
                if (CanOTK())
                    AI.SelectPosition(CardPosition.FaceUpAttack);
                else
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                AI.SelectCard(fallback); 
                return true; 
            }
            return false;
        }

        private bool SuperPolymerizationEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Enemy.GetMonsterCount() == 0) return false;
            AI.SelectCard(CardId.MudragonOfTheSwamp, CardId.StarvingVenomFusionDragon, CardId.DestinyHERODestroyerPhoenixEnforcer);
            return true;
        }

        // โ•โ•โ• Monster Hand/GY Effects โ•โ•โ•

        private bool DreadnoughtServantEffect()
        {
            if (Card == null) return false;
            
            // 1. If in hand: Special Summon itself
            if (Card.Location == CardLocation.Hand)
            {
                bool hasDestinyHERO = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0xc008));
                bool hasField = Bot.HasInSpellZone(CardId.ClockTowerPrisonCityDarkCity);
                if (hasDestinyHERO || hasField) return true;
                return false;
            }
            
            // 2. If on field or in GY:
            bool hasPolyInDeck = GetRemainingCount(CardId.Polymerization) > 0;
            bool hasLevel8DestinyHERO = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0xc008) && c.Level >= 8);
            bool opponentHasCards = Enemy.GetFieldCount() > 0;
            
            if (hasLevel8DestinyHERO && opponentHasCards)
            {
                return true;
            }
            
            if (hasPolyInDeck)
            {
                AI.SelectCard(CardId.Polymerization);
                return true;
            }
            
            return false;
        }

        private bool FurnaceEffect()
        {
            AI.SelectCard(CardId.MaskChange, CardId.Polymerization);
            return true;
        }

        private bool FountainEffect()
        {
            if (Card != null && Card.Location == CardLocation.Hand)
            {
                var targets = Bot.Hand.Where(c => c != null && c != Card && c.HasSetcode(0x8) && c.IsMonster()).ToList();
                if (targets.Count == 0) return false;
                var best = targets.FirstOrDefault(c => c != null && c.Id == CardId.ElementalHEROShadowMist)
                        ?? targets.FirstOrDefault(c => c != null && c.Id == CardId.VisionHEROVyon)
                        ?? targets.FirstOrDefault(c => c != null && c.Id == CardId.DestinyHERODenier)
                        ?? targets.FirstOrDefault(c => c != null && c.Id == CardId.DestinyHERODoomLiege)
                        ?? targets.FirstOrDefault();
                if (best != null) { AI.SelectCard(best); return true; }
            }
            return true;
        }

        private bool MaliciousEffect()
        {
            if (GetRemainingCount(CardId.DestinyHEROMalicious) == 0) return false;
            // Don't activate into Apollousa โ€” it will just negate and waste our Malicious
            var apollousa = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.Id == 4280258 && c.IsFaceup() && !c.IsDisabled() && c.Attack > 0);
            if (apollousa != null) return false;
            return true;
        }

        private bool DenierEffect()
        {
            AI.SelectCard(CardId.DestinyHEROMalicious);
            return true;
        }

        // โ•โ•โ• Special Summons โ•โ•โ•

        private bool DreadnoughtSummon()
        {
            if (Duel.Phase >= DuelPhase.Battle) return false;
            if (CanOTK()) return false; // Attack first, don't tribute Dreadmaster
            return Bot.HasInMonstersZone(CardId.DestinyHERODreadmaster);
        }

        private bool DreadnoughtFieldEffect()
        {
            if (Duel.Phase >= DuelPhase.Battle) return false;
            if (CanOTK()) return false; // If we can OTK, don't pop โ€” just attack
            return Enemy.GetFieldCount() > 0;
        }

        private bool PlasmaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (CanOTK()) return false;
            return Bot.GetMonsters().Count(c => c != null && !IsAceCard(c)) >= 3;
        }

        private bool DeathDogmaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var banishTargets = Bot.Graveyard.Where(c => c != null 
                && (c.HasRace(CardRace.Warrior) || c.HasAttribute(CardAttribute.Dark)) 
                && c.Id != CardId.DestinyHEROMalicious 
                && c.Id != CardId.DestinyHERODenier 
                && !IsAceCard(c)).ToList();
            return banishTargets.Count >= 3;
        }

        // โ•โ•โ• Tribute Summon Safety โ•โ•โ•

        private bool SafeTributeCheck()
        {
            if (Card == null) return false;
            if (CanOTK()) return false; // Don't tribute attackers if we can OTK
            int reqTributes = Card.Level >= 7 ? 2 : (Card.Level >= 5 ? 1 : 0);
            if (reqTributes == 0) return false;
            var nonBoss = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return nonBoss >= reqTributes;
        }

        private bool HandtrapSet() => Bot.GetMonsterCount() == 0;

        // โ•โ•โ• Monster Field Effects โ•โ•โ•

        private bool VyonEffect()
        {
            if (Card != null && Card.Location == CardLocation.MonsterZone)
            {
                if (Bot.Graveyard.Count(c => c != null && c.Id == CardId.DestinyHEROMalicious) == 0)
                    AI.SelectCard(CardId.DestinyHEROMalicious);
                else
                    AI.SelectCard(CardId.ElementalHEROShadowMist);
            }
            return true;
        }

        private bool DoomLiegeEffect()
        {
            AI.SelectCard(CardId.ClockTowerPrisonCityDarkCity);
            return true;
        }

        private bool ShadowMistEffect()
        {
            AI.SelectCard(CardId.MaskChange, CardId.DestinyHEROPlasma, CardId.DestinyHERODeathDogma);
            return true;
        }

        private bool PlasmaEffect() => true;
        private bool DeathDogmaEffect() => true;

        // โ•โ•โ• Extra Deck โ•โ•โ•

        private bool CrossCrusaderSummon()
        {
            if (!ExtraDeckNow()) return false;
            if (Bot.HasInMonstersZone(CardId.XtraHEROCrossCrusader)) return false;
            
            var materials = Bot.GetMonsters().Where(c => c != null && c.HasRace(CardRace.Warrior) && c.IsFaceup()
                && !IsAceCard(c)).ToList();
            if (materials.Count < 2) return false;
            
            bool hasGyDestinyHERO = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasSetcode(0xc008) && c.IsCanRevive());
            bool materialIsDestinyHERO = materials.Any(c => c.HasSetcode(0xc008));
            
            return hasGyDestinyHERO || materialIsDestinyHERO;
        }

        private bool WonderDriverSummon()
        {
            if (!ExtraDeckNow()) return false;
            if (Bot.HasInMonstersZone(CardId.XtraHEROWonderDriver)) return false;
            
            // Only summon if we have a target spell to recycle in GY
            bool hasTargetSpell = Bot.Graveyard.Any(c => c != null && c.IsSpell() 
                && (c.Id == CardId.Polymerization || c.Id == CardId.FusionDestiny || c.Id == CardId.MaskChange));
            if (!hasTargetSpell) return false;
            
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasSetcode(0x8) && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool DreadDecimatorSummon()
        {
            if (!ExtraDeckNow()) return false;
            if (Bot.HasInMonstersZone(CardId.XtraHERODreadDecimator)) return false;
            
            // Only summon if we are going for game or need to beat over a strong enemy
            bool needBeat = Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Attack > Util.GetBestAttack(Bot));
            if (!CanOTK() && !needBeat && Duel.Turn == 1) return false;
            
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasSetcode(0x8) && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool DreadDecimatorEffect()
        {
            return Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasSetcode(0xc008) && c.IsCanRevive());
        }

        private bool DarkLawEffect()
        {
            return Enemy.Hand.Count > 0;
        }

        private bool DusktopiaEffect()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup());
        }

        private bool ContrastHEROChaosEffect()
        {
            return Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup() && !c.IsDisabled());
        }

        private bool CrossCrusaderEffect()
        {
            if (ActivateDescription == Util.GetStringId(CardId.XtraHEROCrossCrusader, 1))
            {
                var destinyHeroes = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasSetcode(0xc008)).ToList();
                if (destinyHeroes.All(c => IsAceCard(c)))
                    return false;
            }
            AI.SelectCard(CardId.DestinyHEROPlasma, CardId.DestinyHERODeathDogma, CardId.DestinyHERODoomLiege, CardId.DestinyHERODenier);
            return true;
        }

        private bool WonderDriverEffect()
        {
            AI.SelectCard(CardId.FusionDestiny, CardId.MaskChange, CardId.Polymerization);
            return true;
        }

        private bool DPEEffect()
        {
            if (Card == null) return false;
            if (Card.Location == CardLocation.Grave) return true;
            if (Enemy.GetFieldCount() == 0) return false;
            if (Duel.LastChainPlayer == 1) return true;
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1) return true;
            return Duel.Player == 1;
        }

        private bool DominanceEffect() => true;

        private bool DystopiaEffect()
        {
            if (Card != null && Card.Location == CardLocation.MonsterZone)
                if (Card.Attack != Card.BaseAttack)
                    return Enemy.GetFieldCount() > 0;
            return true;
        }

        // โ•โ•โ• Traps โ•โ•โ•

        private bool DBurstEffect()
        {
            AI.SelectCard(CardId.ClockTowerPrisonCityDarkCity);
            return true;
        }

        private bool DominusSparkEffect()
        {
            if (Card != null && Card.Location == CardLocation.Hand)
            {
                // Only prevent activation if we hold Ash/Fuwalos/Droll in hand and want to use them
                bool hasRestrictedInHand = Bot.Hand.Any(c => c != null && c.IsMonster()
                    && (c.HasAttribute(CardAttribute.Earth) || c.HasAttribute(CardAttribute.Water)
                     || c.HasAttribute(CardAttribute.Fire) || c.HasAttribute(CardAttribute.Wind)));
                if (hasRestrictedInHand) return false;
            }
            return true;
        }

        private bool AccusationEffect()
        {
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            ClientCard last = Util.GetLastChainCard();
            return last != null && last.Controller == 1;
        }



        // โ•โ•โ• OnSelectCard โ•โ•โ•

        private IList<ClientCard> SelectPreferred(IList<ClientCard> cards, int min, int max, params int[] preferredIds)
        {
            var result = new List<ClientCard>();
            foreach (int id in preferredIds)
            {
                var matches = cards.Where(c => c != null && c.Id == id && !result.Contains(c)).ToList();
                foreach (var m in matches) { result.Add(m); if (result.Count >= max) break; }
                if (result.Count >= max) break;
            }
            if (result.Count < min)
                foreach (var card in cards) { if (card != null && !result.Contains(card)) result.Add(card); if (result.Count >= min) break; }
            return result;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (hint == 533) // HINTMSG_LMATERIAL
            {
                var safe = cards.Where(c => c != null && !IsAceCard(c)).ToList();
                if (safe.Count >= min) return safe.Take(max).ToList();
            }

            // Handle delayed or general special summon effects where Card might be null (e.g. DPE Standby Phase revival)
            if (hint == 509) // HINTMSG_SPSUMMON
            {
                var dpe = cards.FirstOrDefault(c => c != null && c.Id == CardId.DestinyHERODestroyerPhoenixEnforcer && c.Location == CardLocation.Grave);
                if (dpe != null) return new List<ClientCard> { dpe };
            }

            // Handle destruction selection (e.g. DPE destruction resolving sequentially or globally)
            if (hint == 502) // HINTMSG_DESTROY
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                var ourCards = cards.Where(c => c != null && c.Controller == 0).ToList();

                if (enemyCards.Count > 0)
                {
                    return new List<ClientCard> {
                        enemyCards.OrderByDescending(c => {
                            if (c == null) return 0;
                            if (c.Id == 1561110 || c.Id == 10443957) return 10000; // Buster, Infinity
                            if (c.Id == 66399653) return 9000; // Union Hangar
                            if (c.Id == 65741786 || c.Id == 83152482) return 8000; // I:P, Union Carrier
                            if (c.IsMonster()) return c.Attack;
                            if (c.IsFaceup()) return 500;
                            return 100;
                        }).First()
                    };
                }

                if (ourCards.Count > 0)
                {
                    // DPE revives itself from GY, so it's always the best card to self-pop
                    var target = ourCards.FirstOrDefault(c => c.Id == CardId.DestinyHERODestroyerPhoenixEnforcer)
                              ?? ourCards.FirstOrDefault(c => c.Id == CardId.ClockTowerPrisonCityDarkCity)
                              ?? ourCards.FirstOrDefault(c => c.Id == CardId.DestinyHERODenier)
                              ?? ourCards.FirstOrDefault(c => c.Id == CardId.DestinyHERODreadnoughtServant)
                              ?? ourCards.FirstOrDefault(c => c.Id == CardId.DestinyHEROMalicious)
                              ?? ourCards.OrderBy(c => {
                                  if (IsAceCard(c)) return 1000;
                                  return 10;
                              }).FirstOrDefault();
                    if (target != null) return new List<ClientCard> { target };
                }
            }

            if (Card == null) return base.OnSelectCard(cards, min, max, hint, cancelable);

            if (Card.Id == CardId.SabatielThePhilosophersStone)
                return SelectPreferred(cards, min, max, CardId.FusionDestiny, CardId.SuperPolymerization, CardId.Polymerization);

            if (Card.Id == CardId.DestinyHERODreadnoughtServant)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    // Prioritize destroying opponent's high-threat cards
                    return enemyCards.OrderByDescending(c => {
                        if (c.Id == 1561110 || c.Id == 10443957) return 10000; // ABC-Dragon Buster, Infinity
                        if (c.Id == 66399653) return 9000; // Union Hangar
                        if (c.Id == 65741786 || c.Id == 83152482) return 8000; // I:P, Union Carrier
                        if (c.IsMonster()) return c.Attack;
                        if (c.IsFaceup()) return 500;
                        return 100;
                    }).Take(max).ToList();
                }
                var darkCity = cards.FirstOrDefault(c => c != null && c.Id == CardId.ClockTowerPrisonCityDarkCity && c.IsFaceup());
                if (darkCity != null) return new List<ClientCard> { darkCity };
            }

            if (Card.Id == CardId.DBurst)
            {
                var ourMonsters = cards.Where(c => c != null && c.Controller == 0 && c.IsFaceup()).ToList();
                if (ourMonsters.Count > 0)
                {
                    return ourMonsters.OrderBy(c => {
                        if (c.Id == CardId.DestinyHERODreadnoughtServant) return 1; // Best to destroy!
                        if (c.Id == CardId.DestinyHERODenier) return 2;
                        if (c.Id == CardId.DestinyHEROMalicious) return 3;
                        if (c.Id == CardId.VisionHEROVyon) return 4;
                        if (c.Id == CardId.ElementalHEROShadowMist) return 5;
                        if (c.Id == CardId.DestinyHEROPlasma) return 100; // Never destroy!
                        if (c.Id == CardId.DestinyHERODestroyerPhoenixEnforcer) return 99; // Never destroy!
                        if (c.Id == CardId.MaskedHERODarkLaw) return 98;
                        if (c.Id == CardId.DestinyHERODreadnought) return 97;
                        return 10;
                    }).Take(max).ToList();
                }
                var darkCity = cards.FirstOrDefault(c => c != null && c.Id == CardId.ClockTowerPrisonCityDarkCity && c.IsFaceup());
                if (darkCity != null) return new List<ClientCard> { darkCity };
            }

            if (Card.Id == CardId.ClockTowerPrisonCityDarkCity)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck && c.IsMonster()))
                {
                    // Special Summoning from Deck (when destroyed):
                    // 1. If opponent has face-up monsters and we can negate them: prefer Plasma
                    bool opponentHasMonsters = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());
                    if (opponentHasMonsters)
                        return SelectPreferred(cards, min, max, CardId.DestinyHEROPlasma, CardId.DestinyHERODreadmaster, CardId.DestinyHERODoomLiege);
                    
                    // 2. Otherwise, if we have Destiny HEROs in Graveyard: prefer Dreadmaster to revive them!
                    bool hasGyHEROs = Bot.Graveyard.Any(c => c != null && c.HasSetcode(0xc008) && c.IsMonster());
                    if (hasGyHEROs)
                        return SelectPreferred(cards, min, max, CardId.DestinyHERODreadmaster, CardId.DestinyHEROPlasma, CardId.DestinyHERODoomLiege);
                        
                    // 3. Fallback
                    return SelectPreferred(cards, min, max, CardId.DestinyHEROPlasma, CardId.DestinyHERODreadmaster, CardId.DestinyHERODoomLiege, CardId.DestinyHERODenier, CardId.DestinyHEROMalicious);
                }
                
                // Search from Deck (when activated):
                bool hasFusionDestiny = Bot.Hand.Any(c => c != null && c.Id == CardId.FusionDestiny);
                if (!hasFusionDestiny && cards.Any(c => c != null && c.Id == CardId.FusionDestiny))
                    return SelectPreferred(cards, min, max, CardId.FusionDestiny);
                bool hasPlasma = Bot.Hand.Any(c => c != null && c.Id == CardId.DestinyHEROPlasma) || Bot.HasInMonstersZone(CardId.DestinyHEROPlasma);
                if (!hasPlasma && cards.Any(c => c != null && c.Id == CardId.DestinyHEROPlasma))
                    return SelectPreferred(cards, min, max, CardId.DestinyHEROPlasma);
                return SelectPreferred(cards, min, max, CardId.FusionDestiny, CardId.DestinyHEROPlasma, CardId.DestinyHERODreadnoughtServant, CardId.DestinyHERODoomLiege, CardId.DestinyHERODenier, CardId.DestinyHEROMalicious);
            }

            if (Card.Id == CardId.DestinyHERODoomLiege)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck))
                {
                    // Doom Liege sending from Deck to GY:
                    // Prefer Malicious (if 0 in GY), then ShadowMist, then Denier
                    int malInGrave = Bot.Graveyard.Count(c => c != null && c.Id == CardId.DestinyHEROMalicious);
                    if (malInGrave == 0)
                        return SelectPreferred(cards, min, max, CardId.DestinyHEROMalicious, CardId.ElementalHEROShadowMist, CardId.DestinyHERODenier, CardId.DestinyHERODreadnoughtServant);
                    return SelectPreferred(cards, min, max, CardId.ElementalHEROShadowMist, CardId.DestinyHEROMalicious, CardId.DestinyHERODenier, CardId.DestinyHERODreadnoughtServant);
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Hand))
                {
                    // Doom Liege Special Summoning from Hand:
                    // Prefer Vyon, Shadow Mist, Denier, Dreadnought Servant
                    return SelectPreferred(cards, min, max, CardId.VisionHEROVyon, CardId.ElementalHEROShadowMist, CardId.DestinyHERODenier, CardId.DestinyHERODreadnoughtServant, CardId.DestinyHEROMalicious);
                }
                if (cards.Any(c => c != null && c.Id == CardId.ClockTowerPrisonCityDarkCity))
                {
                    return SelectPreferred(cards, min, max, CardId.ClockTowerPrisonCityDarkCity);
                }
            }

            if (Card.Id == CardId.MaskedHEROFurnace)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck))
                    return SelectPreferred(cards, min, max, CardId.MaskChange, CardId.Polymerization);
                return SelectPreferred(cards, min, max, CardId.DestinyHEROMalicious, CardId.DestinyHERODenier, CardId.ElementalHEROShadowMist, CardId.MaskedHEROFountain, CardId.DestinyHERODoomLiege);
            }

            if (Card.Id == CardId.MaskedHERODuskCrow)
            {
                var safeCards = cards.Where(c => c != null && c.Id != CardId.DestinyHEROMalicious && c.Id != CardId.DestinyHERODenier).ToList();
                if (safeCards.Count >= min)
                    return SelectPreferred(safeCards, min, max, CardId.ElementalHEROShadowMist, CardId.VisionHEROVyon, CardId.MaskedHEROFountain, CardId.MaskedHEROFurnace);
            }

            if (Card.Id == CardId.VisionHEROVyon)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck))
                {
                    // Sending from deck to GY: prefer Malicious first if we have 0 in GY!
                    int malInGrave = Bot.Graveyard.Count(c => c != null && c.Id == CardId.DestinyHEROMalicious);
                    if (malInGrave == 0)
                        return SelectPreferred(cards, min, max, CardId.DestinyHEROMalicious, CardId.ElementalHEROShadowMist, CardId.DestinyHERODenier);
                    return SelectPreferred(cards, min, max, CardId.ElementalHEROShadowMist, CardId.DestinyHEROMalicious, CardId.DestinyHERODenier);
                }
                // Banishing from GY to search Polymerization:
                var safeBanish = cards.Where(c => c != null && c.Id != CardId.DestinyHEROMalicious && c.Id != CardId.DestinyHERODenier).ToList();
                if (safeBanish.Count > 0)
                    return SelectPreferred(safeBanish, min, max, CardId.ElementalHEROShadowMist, CardId.VisionHEROVyon, CardId.MaskedHEROFountain, CardId.MaskedHEROFurnace, CardId.MaskedHERODuskCrow);
            }

            if (Card.Id == CardId.DestinyHERODeathDogma)
            {
                if (Card.Location == CardLocation.Hand)
                {
                    // Banishing 3 from GY to summon Death Dogma
                    // Prioritize non-Ace and non-extender (Malicious/Denier) monsters
                    var nonAceNonExtender = cards.Where(c => c != null 
                        && !IsAceCard(c) 
                        && c.Id != CardId.DestinyHEROMalicious 
                        && c.Id != CardId.DestinyHERODenier).ToList();
                    
                    if (nonAceNonExtender.Count >= min)
                        return nonAceNonExtender.Take(max).ToList();
                        
                    var fallback = cards.Where(c => c != null && !IsAceCard(c)).ToList();
                    if (fallback.Count >= min)
                        return fallback.Take(max).ToList();
                        
                    return base.OnSelectCard(cards, min, max, hint, cancelable);
                }
                else
                {
                    // If on field (e.g. recycling materials back to the deck/extra deck)
                    if (cards.Any(c => c != null && c.Location == CardLocation.Extra))
                        return SelectPreferred(cards, min, max, CardId.DestinyHERODestroyerPhoenixEnforcer, CardId.DestinyHERODreadnought, CardId.DestinyHERODominance, CardId.DestinyHERODystopia);
                    
                    // Shuffling materials back to deck: prefer Malicious and Denier to recycle them!
                    return SelectPreferred(cards, min, max, CardId.DestinyHEROMalicious, CardId.DestinyHERODenier, CardId.ElementalHEROShadowMist, CardId.VisionHEROVyon, CardId.DestinyHERODreadnoughtServant, CardId.DestinyHERODoomLiege, CardId.MaskedHEROFountain, CardId.MaskedHEROFurnace, CardId.MaskedHERODuskCrow);
                }
            }

            if (Card.Id == CardId.DestinyHERODenier)
            {
                var malicious = cards.FirstOrDefault(c => c != null && c.Id == CardId.DestinyHEROMalicious);
                if (malicious != null) return new List<ClientCard> { malicious };
            }

            if (Card.Id == CardId.DestinyHERODreadnought)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.MonsterZone))
                    return SelectPreferred(cards, min, max, CardId.DestinyHERODreadmaster);
                    
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck))
                {
                    // Sending 2 cards from Deck to GY:
                    // Priority 1: Malicious (if 0 in GY/field/hand)
                    // Priority 2: Denier (if 0 in GY/field/hand)
                    // Priority 3: Death Dogma
                    // Priority 4: Clock Tower (to recycle with Doom Liege)
                    // Priority 5: DBurst
                    int malCount = Bot.Graveyard.Count(c => c != null && c.Id == CardId.DestinyHEROMalicious)
                                 + Bot.Hand.Count(c => c != null && c.Id == CardId.DestinyHEROMalicious)
                                 + Bot.GetMonsters().Count(c => c != null && c.Id == CardId.DestinyHEROMalicious);
                    int denierCount = Bot.Graveyard.Count(c => c != null && c.Id == CardId.DestinyHERODenier)
                                    + Bot.Hand.Count(c => c != null && c.Id == CardId.DestinyHERODenier)
                                    + Bot.GetMonsters().Count(c => c != null && c.Id == CardId.DestinyHERODenier);
                                    
                    var preferred = new List<int>();
                    if (malCount == 0) preferred.Add(CardId.DestinyHEROMalicious);
                    if (denierCount == 0) preferred.Add(CardId.DestinyHERODenier);
                    preferred.Add(CardId.DestinyHERODeathDogma);
                    preferred.Add(CardId.ClockTowerPrisonCityDarkCity);
                    preferred.Add(CardId.DBurst);
                    preferred.Add(CardId.DestinyHEROMalicious);
                    preferred.Add(CardId.DestinyHERODenier);
                    
                    return SelectPreferred(cards, min, max, preferred.ToArray());
                }
            }

            if (Card.Id == CardId.DestinyHEROPlasma)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.MonsterZone))
                    return cards.Where(c => c != null).OrderBy(c => {
                        if (c.Id == CardId.DestinyHERODestroyerPhoenixEnforcer) return 10;
                        if (c.Id == CardId.MaskedHERODarkLaw) return 9;
                        if (c.Id == CardId.DestinyHERODreadnought) return 8;
                        if (c.Id == CardId.DestinyHERODreadmaster) return 7;
                        if (c.Id == CardId.XtraHERODreadDecimator) return 6;
                        if (c.Id == CardId.XtraHEROWonderDriver) return 5;
                        if (c.Id == CardId.XtraHEROCrossCrusader) return 4;
                        return 1;
                    }).Take(max).ToList();
                var enemyMonsters = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup() && c.IsMonster()).ToList();
                if (enemyMonsters.Count > 0)
                    return new List<ClientCard> { enemyMonsters.OrderByDescending(c => c.HasType(CardType.Effect)).ThenByDescending(c => c.Attack).First() };
            }

            if (Card.Id == CardId.FusionDestiny)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Extra))
                {
                    return SelectPreferred(cards, min, max, CardId.DestinyHERODestroyerPhoenixEnforcer, CardId.DestinyHERODominance, CardId.DestinyHERODystopia);
                }
                return SelectPreferred(cards, min, max, CardId.DestinyHEROMalicious, CardId.DestinyHERODenier, CardId.ElementalHEROShadowMist, CardId.DestinyHERODoomLiege, CardId.DestinyHERODreadnoughtServant);
            }

            if (Card.Id == CardId.XtraHEROCrossCrusader)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.MonsterZone))
                {
                    // Tributing for cost: tribute low priority monsters first
                    return cards.Where(c => c != null).OrderBy(c => {
                        if (IsAceCard(c)) return 10;
                        if (c.Id == CardId.DestinyHEROMalicious) return 2; // Keep Malicious if we can
                        return 1;
                    }).Take(max).ToList();
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Grave))
                {
                    // Special Summoning from GY: prefer Malicious or Denier
                    return SelectPreferred(cards, min, max, CardId.DestinyHEROMalicious, CardId.DestinyHERODenier, CardId.DestinyHERODoomLiege, CardId.DestinyHERODreadnoughtServant);
                }
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck))
                {
                    // Searching from Deck:
                    // 1. If we can summon Plasma (have 3+ monsters or D-Force) and don't have it, search Plasma
                    bool hasDForce = Bot.Hand.Any(c => c != null && c.Id == CardId.DForce) || Bot.HasInSpellZone(CardId.DForce);
                    bool canSummonPlasma = Bot.GetMonsterCount() >= 3 || hasDForce;
                    bool hasPlasma = Bot.Hand.Any(c => c != null && c.Id == CardId.DestinyHEROPlasma) || Bot.HasInMonstersZone(CardId.DestinyHEROPlasma);
                    
                    if (canSummonPlasma && !hasPlasma)
                        return SelectPreferred(cards, min, max, CardId.DestinyHEROPlasma);
                        
                    // 2. Otherwise, if we don't have Denier: search Denier
                    bool hasDenier = Bot.Hand.Any(c => c != null && c.Id == CardId.DestinyHERODenier) 
                                  || Bot.Graveyard.Any(c => c != null && c.Id == CardId.DestinyHERODenier)
                                  || Bot.HasInMonstersZone(CardId.DestinyHERODenier);
                    if (!hasDenier)
                        return SelectPreferred(cards, min, max, CardId.DestinyHERODenier);
                        
                    // 3. If we have Clock Tower but no Dreadnought Servant: search Dreadnought Servant
                    bool hasClockTower = Bot.Hand.Any(c => c != null && c.Id == CardId.ClockTowerPrisonCityDarkCity) || Bot.HasInSpellZone(CardId.ClockTowerPrisonCityDarkCity);
                    bool hasServant = Bot.Hand.Any(c => c != null && c.Id == CardId.DestinyHERODreadnoughtServant) || Bot.HasInMonstersZone(CardId.DestinyHERODreadnoughtServant);
                    if (hasClockTower && !hasServant)
                        return SelectPreferred(cards, min, max, CardId.DestinyHERODreadnoughtServant);
                        
                    // 4. Default searches
                    return SelectPreferred(cards, min, max, CardId.DestinyHEROPlasma, CardId.DestinyHERODeathDogma, CardId.DestinyHERODoomLiege, CardId.DestinyHEROMalicious, CardId.DestinyHERODenier, CardId.DestinyHERODreadnoughtServant);
                }
            }

            if (Card.Id == CardId.DestinyHERODestroyerPhoenixEnforcer)
            {
                // DPE GY revival: always pick DPE itself
                if (cards.Any(c => c != null && c.Location == CardLocation.Grave))
                    return SelectPreferred(cards, min, max, CardId.DestinyHERODestroyerPhoenixEnforcer);

                // DPE destruction effect: destroy 1 our + 1 enemy
                var ourCards = cards.Where(c => c != null && c.Controller == 0).ToList();
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                var result = new List<ClientCard>();

                // Always add our best self-pop target (DPE itself revives!)
                if (ourCards.Count > 0)
                {
                    var ours = ourCards.FirstOrDefault(c => c.Id == CardId.DestinyHERODestroyerPhoenixEnforcer)
                            ?? ourCards.FirstOrDefault(c => c.Id == CardId.ClockTowerPrisonCityDarkCity)
                            ?? ourCards.FirstOrDefault(c => !IsAceCard(c))
                            ?? ourCards[0];
                    if (ours != null) result.Add(ours);
                }

                // Add best enemy target
                if (enemyCards.Count > 0)
                {
                    var target = enemyCards.OrderByDescending(c => {
                        if (c == null) return 0;
                        if (c.Id == 1561110 || c.Id == 10443957) return 10000; // ABC-Dragon Buster, Infinity
                        if (c.Id == 66399653) return 9000; // Union Hangar
                        if (c.Id == 65741786 || c.Id == 83152482) return 8000; // I:P, Union Carrier
                        if (c.IsMonster()) return c.Attack;
                        if (c.IsFaceup()) return 500;
                        return 100;
                    }).First();
                    result.Add(target);
                }

                // Fallback: fill remaining required slots
                if (result.Count < min)
                    foreach (var c in cards)
                        if (c != null && !result.Contains(c)) { result.Add(c); if (result.Count >= min) break; }
                if (result.Count > max) result = result.Take(max).ToList();
                return result;
            }

            if (Card.Id == CardId.DestinyHERODominance && cards.Any(c => c != null && c.Location == CardLocation.Grave))
                return SelectPreferred(cards, min, max, CardId.DestinyHERODreadmaster, CardId.DestinyHERODoomLiege, CardId.ElementalHEROShadowMist, CardId.DestinyHERODreadnoughtServant, CardId.DestinyHEROMalicious, CardId.DestinyHERODenier);

            if (Card.Id == CardId.DestinyHERODystopia)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Grave)) { var t = cards.Where(c => c != null && c.IsMonster()).OrderByDescending(c => c.Attack).FirstOrDefault(); if (t != null) return new List<ClientCard> { t }; }
                var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (oppCards.Count > 0) { var t = oppCards.OrderByDescending(c => c.IsMonster() ? c.Attack : 0).FirstOrDefault(); if (t != null) return new List<ClientCard> { t }; }
            }

            if (Card.Id == CardId.MaskedHEROFountain) return SelectPreferred(cards, min, max, CardId.MaskChange);

            if (Card.Id == CardId.ElementalHEROShadowMist)
            {
                if (cards.Any(c => c != null && c.IsCode(CardId.MaskChange))) return SelectPreferred(cards, min, max, CardId.MaskChange);
                return SelectPreferred(cards, min, max, CardId.VisionHEROVyon, CardId.DestinyHEROMalicious, CardId.DestinyHERODenier, CardId.DestinyHEROPlasma, CardId.DestinyHERODreadnoughtServant, CardId.MaskedHEROFurnace, CardId.MaskedHERODuskCrow, CardId.MaskedHEROFountain);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            if (Card != null && Card.Id == CardId.SuperPolymerization)
            {
                var oppMonsters = cards.Where(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone).ToList();
                var resultSP = oppMonsters.Take(max).ToList();
                if (resultSP.Count < min)
                {
                    var safeOurs = cards.Where(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone
                        && !IsAceCard(c)).ToList();
                    foreach (var c in safeOurs) { if (!resultSP.Contains(c)) resultSP.Add(c); if (resultSP.Count >= min) break; }
                }
                if (resultSP.Count < min) return base.OnSelectFusionMaterial(cards, min, max);
                return resultSP;
            }
            bool isDeckFusion = cards.Any(c => c != null && c.Location == CardLocation.Deck);
            if (isDeckFusion || (Card != null && Card.Id == CardId.FusionDestiny))
            {
                var deckHand = cards.Where(c => c != null && (c.Location == CardLocation.Deck || c.Location == CardLocation.Hand)).ToList();
                var resultDF = new List<ClientCard>();
                int maliciousSelectedCount = 0;
                int[] preferredIds = {
                    CardId.DestinyHEROMalicious,
                    CardId.DestinyHERODenier,
                    CardId.ElementalHEROShadowMist,
                    CardId.DestinyHERODoomLiege,
                    CardId.DestinyHERODreadnoughtServant,
                    CardId.DestinyHERODogma,
                    CardId.DestinyHEROPlasma,
                    CardId.DestinyHERODreadmaster
                };
                foreach (int id in preferredIds)
                {
                    var matches = deckHand.Where(c => c != null && c.Id == id && !resultDF.Contains(c)).ToList();
                    foreach (var m in matches)
                    {
                        if (id == CardId.DestinyHEROMalicious)
                        {
                            if (maliciousSelectedCount >= 1) continue;
                            maliciousSelectedCount++;
                        }
                        resultDF.Add(m);
                        if (resultDF.Count >= max) break;
                    }
                    if (resultDF.Count >= max) break;
                }
                if (resultDF.Count < min)
                {
                    foreach (var c in deckHand)
                    {
                        if (c != null && !resultDF.Contains(c))
                        {
                            resultDF.Add(c);
                            if (resultDF.Count >= min) break;
                        }
                    }
                }
                return resultDF;
            }
            var result = new List<ClientCard>();
            var handMats = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
            var gyMats = cards.Where(c => c != null && c.Location == CardLocation.Grave).ToList();
            var fieldMats = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone).ToList();
            var safeField = fieldMats.Where(c => c != null && !IsAceCard(c)).ToList();
            foreach (var c in handMats) { if (!result.Contains(c)) result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max) foreach (var c in gyMats) { if (!result.Contains(c)) result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max) foreach (var c in safeField) { if (!result.Contains(c)) result.Add(c); if (result.Count >= max) break; }
            if (result.Count < min) foreach (var c in fieldMats) { if (!result.Contains(c)) result.Add(c); if (result.Count >= min) break; }
            return result;
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (Card != null && Card.Id == CardId.SolemnAccusation && options != null && options.Count > 1)
            {
                if (Bot.LifePoints > 3000)
                {
                    var targetCard = Util.GetLastChainCard();
                    if (targetCard != null)
                    {
                        int targetId = targetCard.Id;
                        if (targetId == 66399653 || targetId == 12524259 || targetId == 73628505 || targetId == 1561110 || targetId == 10443957)
                        {
                            return 1; // Negate and banish option!
                        }
                    }
                }
            }
            return 0;
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            var safe = cards.Where(c => c != null && !IsAceCard(c)).ToList();
            if (safe.Count >= min) return safe.Take(max).ToList();
            return base.OnSelectLinkMaterial(cards, min, max);
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (defender != null && defender.IsFaceup() && (defender.Id == 21887175 || defender.Id == 21887176) && !defender.IsDisabled())
            {
                if (attacker.IsSpecialSummoned)
                    return false;
            }
            return base.OnPreBattleBetween(attacker, defender);
        }
    }
}
