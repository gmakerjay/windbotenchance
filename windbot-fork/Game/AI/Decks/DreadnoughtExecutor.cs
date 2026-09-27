// ============================================================
// CARD AUDIT — 2026_Dreadnought (Destiny HERO & Dreadnought Modern AI)
// ============================================================
// | Card Name                                   | Type        | OPT? | HOPT? | Cost            | Effect Summary                                                        |
// |---------------------------------------------|-------------|------|-------|-----------------|-----------------------------------------------------------------------|
// | Winged Kuriboh Sabatiel LV10 (40237839)     | Monster     | Yes  | No    | Reveal, 1/2 LP  | Search "Polymerization" or "Fusion" Spell from Deck                   |
// | Destiny HERO - Dogma (17132130)             | Monster     | No   | No    | Tribute 3       | Cannot Normal. SS by tributing 3 (incl 1 D-HERO). Halves opp LP in SP |
// | Destiny HERO - Death Dogma (101402021)      | Monster     | Yes  | Yes   | Banish 3 GY     | SS from Hand/GY banishing 3 Warrior/DARK. 2000 burn. Quick Fusion eff  |
// | Destiny HERO - Plasma (83965310)            | Monster     | No   | No    | Tribute 3       | SS by tributing 3. Negate opp face-up monster effects. Absorb 1 opp mon|
// | Destiny HERO - Dreadmaster (40591390)       | Monster     | No   | No    | None            | SS via Clock Tower: SS up to 2 D-HERO from GY; D-HEROs immune to dest |
// | Masked HERO Furnace (58288218)              | Monster     | Yes  | Yes   | Reveal, discard | Search Mask Change or Poly, discard 1. SS from GY/Hand on Fusion SS   |
// | Destiny HERO - Malicious (9411399)          | Monster     | No   | No    | Banish self GY  | Special Summon another Malicious from Deck                            |
// | Masked HERO Dusk Crow (10808715)            | Monster     | Yes  | Yes   | Banish 1 GY     | SS from Hand banishing HERO from GY. On summon: search Masked HERO   |
// | Elemental HERO Shadow Mist (50720316)       | Monster     | Yes  | Yes   | None            | SS: search "Change" Quick-Play. Sent to GY: search 1 HERO monster     |
// | Masked HERO Fountain (66206748)             | Monster     | Yes  | Yes   | Reveal in hand  | SS 1 HERO from hand in DEF. Sent to GY: Set Mask Change from Deck/GY  |
// | Vision HERO Vyon (27780618)                 | Monster     | Yes  | No    | Banish 1 GY     | Normal/SS: dump HERO from Deck. Banish HERO from GY: search Poly      |
// | Mulcharmy Fuwalos (42141493)                | Hand Trap   | Yes  | Yes   | Discard from hand| Draw when opponent Special Summons from Deck/Extra Deck               |
// | Destiny HERO - Denier (16605586)            | Monster     | Yes  | Yes   | None            | Normal/SS: recycle banished/GY/Deck D-HERO to top. SS from GY (1/duel)|
// | Destiny HERO - Doom Liege (101402022)       | Monster     | Yes  | Yes   | Send D-HERO deck| Normal/SS: temp banish 1 opp monster. Send D-HERO: search Clock Tower |
// | Destiny HERO - Dreadnought Servant (101402023) Monster    | Yes  | Yes   | Pop 1 your card | SS from hand if D-HERO/Field. Pop 1 card -> search Poly. Spin opp card|
// | Ash Blossom & Joyous Spring (14558127)      | Hand Trap   | Yes  | Yes   | Discard from hand| Negate search / Special Summon from Deck                              |
// | Polymerization (24094653)                   | Spell       | No   | No    | Materials       | Fusion Summon 1 Fusion Monster from Extra Deck                        |
// | Reinforcement of the Army (32807846)        | Spell       | No   | No    | None            | Add 1 Level 4 or lower Warrior monster from Deck to hand               |
// | Fusion Destiny (52947044)                   | Spell       | Yes  | Yes   | None            | Fusion Summon Destiny HERO Fusion using materials from Deck           |
// | Terraforming (73628505)                     | Spell       | No   | No    | None            | Add 1 Field Spell from Deck to hand                                   |
// | Foolish Burial (81439173)                   | Spell       | No   | No    | None            | Send 1 monster from Deck to Graveyard                                 |
// | D - Burst (100456010)                       | Spell       | Yes  | Yes   | Pop 1 faceup Spl| Pop face-up Spell -> draw 1 + SS D-HERO from hand/GY/banished. Attack2|
// | Mask Change (21143940)                      | Quick Spell | No   | No    | Send HERO to GY | Send HERO -> SS Masked HERO with same Attribute (Dark Law)           |
// | Called by the Grave (24224830)              | Quick Spell | No   | No    | Target opp GY   | Banish 1 monster from opp GY and negate its effects                   |
// | D - Force (6186304)                         | Spell       | No   | No    | Top of deck     | Search Plasma. Protect Plasma (untargetable/indestructible + 2x atk)  |
// | Clock Tower Prison City - Dark City (101402062) Field Spell| Yes  | Yes   | None            | On activation: search D-HERO card. When destroyed: SS Dreadmaster!    |
// | Infinite Impermanence (10045474)            | Trap        | No   | No    | None            | Negate 1 face-up monster on field (activatable from hand if field 0)  |
// | Solemn Report (78114463)                    | Counter Trap| No   | No    | 1500 / 3000 LP  | Negate Spell/Trap activation: lock activations or banish all copies   |
// | Destiny HERO - Destroyer Phoenix Enforcer (60461804) Fusion | Yes  | Yes   | Pop 1 our+1 opp | Quick Effect: pop 1 our + 1 opp card. Standby Phase: SS D-HERO from GY|
// | Destiny HERO - Dreadnought (101402037)      | Fusion      | Yes  | Yes   | Tribute Dreadmas| Extra Deck SS by tributing Dreadmaster. On SS: search 2 D-HERO cards! |
// | Vision HERO Trinity (46759931)              | Fusion      | No   | No    | 3 HEROs         | 5000 ATK, 3 attacks on monsters (OTK board breaker)                   |
// | Masked HERO Dark Law (58481572)             | Fusion      | No   | No    | Mask Change     | Macro Cosmos on opp GY; rip random card from opp hand on search       |
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

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
            public const int SolemnReport = 78114463;

            // Extra Deck
            public const int DestinyHERODusktopia = 93657021;
            public const int DestinyHERODominance = 69394324;
            public const int ContrastHEROChaos = 23204029;
            public const int DestinyHERODystopia = 90579153;
            public const int DestinyHERODestroyerPhoenixEnforcer = 60461804;
            public const int VisionHEROTrinity = 46759931; // Correct ID for Trinity (was mislabeled as StarvingVenom)
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
        private bool _dreadnoughtServantHandUsed = false;
        private bool _dreadnoughtServantGYUsed = false;
        private bool _doomLiegeSummonUsed = false;
        private bool _doomLiegeDeckUsed = false;
        private bool _deathDogmaSummonUsed = false;
        private bool _deathDogmaQuickUsed = false;
        private bool _dreadnoughtSummonUsed = false;
        private bool _dreadnoughtSearchUsed = false;
        private bool _dBurstUsed = false;
        private bool _furnaceHandUsed = false;
        private bool _furnaceGYUsed = false;
        private bool _duskCrowHandUsed = false;
        private bool _duskCrowSummonUsed = false;
        private bool _fountainHandUsed = false;
        private bool _fountainGYUsed = false;
        private bool _denierGYUsed = false;
        private bool _crossCrusaderSearchUsed = false;
        private bool _wonderDriverUsed = false;

        public override bool OnSelectHand()
        {
            // Hero beatdown and board break prefer going second if OTK possible, but going first builds an unbreakable DPE+Plasma+Dark Law board
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _sabatielUsed = false;
            _terraformingUsed = false;
            _dForceUsed = false;
            _clockTowerSpellActivated = false;
            _clockTowerEffectUsed = false;
            _dreadnoughtServantHandUsed = false;
            _dreadnoughtServantGYUsed = false;
            _doomLiegeSummonUsed = false;
            _doomLiegeDeckUsed = false;
            _deathDogmaSummonUsed = false;
            _deathDogmaQuickUsed = false;
            _dreadnoughtSummonUsed = false;
            _dreadnoughtSearchUsed = false;
            _dBurstUsed = false;
            _furnaceHandUsed = false;
            _furnaceGYUsed = false;
            _duskCrowHandUsed = false;
            _duskCrowSummonUsed = false;
            _fountainHandUsed = false;
            _fountainGYUsed = false;
            _denierGYUsed = false;
            _crossCrusaderSearchUsed = false;
            _wonderDriverUsed = false;
        }

        private static readonly int[] AceCardIds = {
            CardId.DestinyHERODestroyerPhoenixEnforcer,
            CardId.DestinyHEROPlasma,
            CardId.DestinyHERODreadnought,
            CardId.MaskedHERODarkLaw,
            CardId.VisionHEROTrinity,
            CardId.DestinyHERODeathDogma,
            CardId.DestinyHERODogma,
            CardId.DestinyHERODreadmaster,
            CardId.ContrastHEROChaos,
            CardId.DestinyHERODusktopia,
            CardId.DestinyHERODominance,
            CardId.DestinyHERODystopia
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Any(id => card.IsCode(id));
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;

            if (c.Id == CardId.DestinyHEROMalicious) return 20;
            if (c.Id == CardId.DestinyHERODenier) return 30;
            if (c.Id == CardId.DestinyHERODreadnoughtServant) return 50;
            if (c.Id == CardId.ElementalHEROShadowMist) return 60;
            if (c.Id == CardId.VisionHEROVyon) return 80;
            if (c.Id == CardId.MaskedHEROFountain) return 90;
            if (c.Id == CardId.MaskedHEROFurnace) return 100;
            if (c.Id == CardId.DestinyHERODoomLiege) return 120;
            if (c.Id == CardId.MaskedHERODuskCrow) return 130;

            return base.GetMaterialPriority(c);
        }

        public _2026_DreadnoughtExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // ── Combo Router ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "ClockTower-Dreadnought-Line",
                RequiredCards = new List<int> { CardId.ClockTowerPrisonCityDarkCity, CardId.DestinyHERODreadnoughtServant },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ClockTowerPrisonCityDarkCity, ActionType = ExecutorType.Activate, Description = "Activate Clock Tower City" },
                    new() { CardId = CardId.DestinyHERODreadnoughtServant, ActionType = ExecutorType.Activate, Description = "SS Dreadnought Servant and pop Clock Tower" },
                    new() { CardId = CardId.ClockTowerPrisonCityDarkCity, ActionType = ExecutorType.Activate, Description = "Clock Tower destroyed triggers Dreadmaster from Deck" },
                    new() { CardId = CardId.DestinyHERODreadmaster, ActionType = ExecutorType.Activate, Description = "Dreadmaster revives D-HEROs from GY" },
                    new() { CardId = CardId.DestinyHERODreadnought, ActionType = ExecutorType.SpSummon, Description = "Tribute Dreadmaster for Dreadnought" },
                    new() { CardId = CardId.DestinyHERODreadnought, ActionType = ExecutorType.Activate, Description = "Dreadnought adds 2 D-HERO cards to hand" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Vyon-CrossCrusader-Line",
                RequiredCards = new List<int> { CardId.VisionHEROVyon },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.VisionHEROVyon, ActionType = ExecutorType.Summon, Description = "Normal Summon Vyon" },
                    new() { CardId = CardId.VisionHEROVyon, ActionType = ExecutorType.Activate, Description = "Send Malicious to GY" },
                    new() { CardId = CardId.DestinyHEROMalicious, ActionType = ExecutorType.Activate, Description = "Banish Malicious to SS Malicious" },
                    new() { CardId = CardId.XtraHEROCrossCrusader, ActionType = ExecutorType.SpSummon, Description = "Link Summon Cross Crusader" },
                    new() { CardId = CardId.XtraHEROCrossCrusader, ActionType = ExecutorType.Activate, Description = "Revive D-HERO and search HERO" },
                    new() { CardId = CardId.FusionDestiny, ActionType = ExecutorType.Activate, Description = "Activate Fusion Destiny" },
                    new() { CardId = CardId.DestinyHERODestroyerPhoenixEnforcer, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon DPE" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "FusionDestiny-Line",
                RequiredCards = new List<int> { CardId.FusionDestiny },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.FusionDestiny, ActionType = ExecutorType.Activate, Description = "Activate Fusion Destiny" },
                    new() { CardId = CardId.DestinyHERODestroyerPhoenixEnforcer, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon DPE" }
                },
                EndBoardScore = 80
            });

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(CardId.FusionDestiny, CardId.VisionHEROVyon, CardId.ClockTowerPrisonCityDarkCity);
            BaitPlanner.RegisterBaitCards(CardId.ReinforcementOfTheArmy, CardId.Terraforming, CardId.SabatielThePhilosophersStone, CardId.MaskedHEROFurnace);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.FusionDestiny, CardId.ReinforcementOfTheArmy, CardId.ClockTowerPrisonCityDarkCity);

            // ===== Quick & Interruption Handtraps =====
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, () => SmartHandTrapChain() && DefaultDontChainMyself());
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);

            // ===== Highest Priority Field Disruptions (Opponent Turn & Standby) =====
            AddExecutor(ExecutorType.Activate, CardId.SolemnReport, SolemnReportEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusSpark, DominusSparkEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODestroyerPhoenixEnforcer, DPEEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODeathDogma, DeathDogmaEffect);
            AddExecutor(ExecutorType.Activate, CardId.ContrastHEROChaos, ContrastHEROChaosEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODystopia, DystopiaEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODusktopia, DusktopiaEffect);

            // ===== Field Spells & Pre-Combo Searches =====
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClockTowerPrisonCityDarkCity, ClockTowerEffect);
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, ReinforcementOfTheArmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.FoolishBurial, FoolishBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.SabatielThePhilosophersStone, SabatielEffect);
            AddExecutor(ExecutorType.Activate, CardId.DForce, DForceEffect);

            // ===== Signature Engine: Dreadmaster -> Dreadnought =====
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODreadnoughtServant, DreadnoughtServantEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODreadmaster, DreadmasterEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODreadnought, DreadnoughtSummon);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODreadnought, DreadnoughtFieldEffect);

            // ===== Hand & GY Extenders =====
            AddExecutor(ExecutorType.Activate, CardId.MaskedHEROFurnace, FurnaceEffect);
            AddExecutor(ExecutorType.Activate, CardId.MaskedHERODuskCrow, DuskCrowEffect);
            AddExecutor(ExecutorType.Activate, CardId.MaskedHEROFountain, FountainEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHEROMalicious, MaliciousEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODenier, DenierEffect);

            // ===== Fusion Spells =====
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.FusionDestiny, FusionDestinyEffect);
            AddExecutor(ExecutorType.Activate, CardId.Polymerization, PolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.MaskChange, MaskChangeEffect);
            AddExecutor(ExecutorType.Activate, CardId.DBurst, DBurstEffect);

            // ===== Special Summons (Hand / Bosses) =====
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODeathDogma, DeathDogmaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHEROPlasma, PlasmaSummon);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHEROPlasma, PlasmaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODogma, DogmaSummon);

            // ===== Normal Summons =====
            AddExecutor(ExecutorType.Summon, SafeTributeCheck);
            AddExecutor(ExecutorType.Summon, CardId.VisionHEROVyon, VyonSummon);
            AddExecutor(ExecutorType.Summon, CardId.DestinyHERODoomLiege, DoomLiegeSummon);
            AddExecutor(ExecutorType.Summon, CardId.DestinyHERODreadnoughtServant, DreadnoughtServantSummon);
            AddExecutor(ExecutorType.Summon, CardId.ElementalHEROShadowMist, ShadowMistSummon);
            AddExecutor(ExecutorType.Summon, CardId.MaskedHERODuskCrow, DuskCrowSummon);
            AddExecutor(ExecutorType.Summon, CardId.MaskedHEROFountain);
            AddExecutor(ExecutorType.Summon, CardId.DestinyHERODenier);
            AddExecutor(ExecutorType.Summon, CardId.MaskedHEROFurnace);

            // ===== Monster On-Summon / Field Triggers =====
            AddExecutor(ExecutorType.Activate, CardId.VisionHEROVyon, VyonEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODoomLiege, DoomLiegeEffect);
            AddExecutor(ExecutorType.Activate, CardId.ElementalHEROShadowMist, ShadowMistEffect);

            // ===== Extra Deck: Link Summoning =====
            AddExecutor(ExecutorType.SpSummon, CardId.XtraHEROCrossCrusader, CrossCrusaderSummon);
            AddExecutor(ExecutorType.Activate, CardId.XtraHEROCrossCrusader, CrossCrusaderEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.XtraHEROWonderDriver, WonderDriverSummon);
            AddExecutor(ExecutorType.Activate, CardId.XtraHEROWonderDriver, WonderDriverEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.XtraHERODreadDecimator, DreadDecimatorSummon);
            AddExecutor(ExecutorType.Activate, CardId.XtraHERODreadDecimator, DreadDecimatorEffect);

            // ===== Extra Deck: Fusion Summoning =====
            AddExecutor(ExecutorType.SpSummon, CardId.VisionHEROTrinity, TrinitySummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODestroyerPhoenixEnforcer);
            AddExecutor(ExecutorType.SpSummon, CardId.MaskedHERODarkLaw);
            AddExecutor(ExecutorType.Activate, CardId.MaskedHERODarkLaw, DarkLawEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODominance);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODominance, DominanceEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODystopia);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODusktopia);
            AddExecutor(ExecutorType.SpSummon, CardId.ContrastHEROChaos);
            AddExecutor(ExecutorType.SpSummon, CardId.DestinyHERODangerous);
            AddExecutor(ExecutorType.Activate, CardId.DestinyHERODangerous, DangerousEffect);

            // ===== Trap Sets (End of MP1 / MP2 only) =====
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnReport);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.MaskChange);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, () => Duel.Phase == DuelPhase.Main2 || (Duel.Turn == 1 && Bot.GetMonsterCount() > 0));
            AddExecutor(ExecutorType.SpellSet, CardId.DominusSpark, () => Duel.Phase == DuelPhase.Main2 || Duel.Turn == 1);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        // ── Strategic Rush Mode & Lethal Guard ──
        public bool HasLethalOnBoard()
        {
            if (Duel.Phase >= DuelPhase.Battle) return false;
            int totalAtk = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.Attack > 0).Sum(c => c.Attack);
            if (Enemy.GetMonsterCount() == 0 && totalAtk >= Enemy.LifePoints)
                return true;

            int enemyTotalAtk = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).Sum(c => c.Attack);
            if (Enemy.GetMonsterCount() > 0 && totalAtk - enemyTotalAtk >= Enemy.LifePoints + 1500)
                return true;

            return false;
        }

        private bool ExtraDeckNow()
        {
            if (HasLethalOnBoard()) return false;
            if (Duel.Turn == 1) return true;
            if (Duel.Phase == DuelPhase.Main1 && Bot.HasAttackingMonster() && Enemy.GetMonsterCount() == 0) return false;
            return true;
        }

        private int GetThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;
            if (CardIntelligence.IsKnownNegator(card.Id)) score += 8000;
            if (CardIntelligence.IsFloodgateMonster(card.Id) || CardIntelligence.IsFloodgateSpellTrap(card.Id)) score += 9000;
            if (CardIntelligence.IsHighThreatChokepoint(card.Id)) score += 6000;
            if (card.IsMonster())
            {
                score += 1000;
                score += card.Attack;
                if (card.IsExtraCard()) score += 2000;
                if (card.IsFaceup() && !card.IsDisabled())
                {
                    if (card.Attack >= 2500) score += 3000;
                    if (card.HasType(CardType.Effect)) score += 2000;
                }
            }
            else if (card.IsSpell() || card.IsTrap())
            {
                score += 500;
                if (card.IsFaceup()) score += 2000;
            }
            return score;
        }

        private ClientCard GetPreemptiveImpermTarget()
        {
            return Enemy.MonsterZone.GetMonsters().FirstOrDefault(c =>
                c != null && c.IsFaceup() && !c.IsDisabled() &&
                (CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsHighThreatChokepoint(c.Id) || c.HasType(CardType.Effect)) &&
                IsViableEffectTarget(c));
        }

        // ── Hand Traps ──
        private bool MulcharmyEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return DefaultAshBlossomAndJoyousSpring();
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

        private bool DominusSparkEffect()
        {
            if (Card != null && Card.Location == CardLocation.Hand)
            {
                bool hasRestrictedInHand = Bot.Hand.Any(c => c != null && c.IsMonster()
                    && (c.HasAttribute(CardAttribute.Earth) || c.HasAttribute(CardAttribute.Water)
                     || c.HasAttribute(CardAttribute.Fire) || c.HasAttribute(CardAttribute.Wind)));
                if (hasRestrictedInHand) return false;
            }

            var oppTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsViableEffectTarget(c));
            if (oppTarget == null) return false;
            AI.SelectCard(oppTarget);
            return true;
        }

        private bool SolemnReportEffect()
        {
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            ClientCard last = Util.GetLastChainCard();
            if (last == null || last.Controller != 1) return false;

            // Only negate Spell/Trap activations
            return last.IsSpell() || last.IsTrap();
        }

        // ── Spells: Search & Starters ──
        private bool SabatielEffect()
        {
            if (_sabatielUsed || HasLethalOnBoard()) return false;
            if (Bot.LifePoints <= 2000) return false; // Safety margin
            if (GetRemainingCount(CardId.FusionDestiny) == 0 && GetRemainingCount(CardId.Polymerization) == 0) return false;

            _sabatielUsed = true;
            return true;
        }

        private bool TerraformingEffect()
        {
            if (HasLethalOnBoard() || _terraformingUsed) return false;
            if (Bot.HasInSpellZone(CardId.ClockTowerPrisonCityDarkCity) || Bot.Hand.Any(c => c != null && c.Id == CardId.ClockTowerPrisonCityDarkCity)) return false;
            _terraformingUsed = true;
            AI.SelectCard(CardId.ClockTowerPrisonCityDarkCity);
            return true;
        }

        private bool ClockTowerEffect()
        {
            if (Card == null) return false;

            // 1. Activating Field Spell from Hand
            if (Card.Location == CardLocation.Hand)
            {
                if (_clockTowerSpellActivated) return false;
                if (Bot.HasInSpellZone(CardId.ClockTowerPrisonCityDarkCity)) return false;
                _clockTowerSpellActivated = true;
                return true;
            }

            // 2. Main Phase Ignition Search Effect
            if (Card.Location == CardLocation.SpellZone)
            {
                if (_clockTowerEffectUsed || HasLethalOnBoard()) return false;
                _clockTowerEffectUsed = true;
                return true;
            }

            return false;
        }

        private bool ReinforcementOfTheArmyEffect()
        {
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool FoolishBurialEffect()
        {
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool DForceEffect()
        {
            if (_dForceUsed || HasLethalOnBoard()) return false;
            if (Bot.HasInSpellZone(CardId.DForce)) return false;
            _dForceUsed = true;
            AI.SelectCard(CardId.DestinyHEROPlasma);
            return true;
        }

        private bool DBurstEffect()
        {
            if (Card == null || _dBurstUsed) return false;

            // GY effect: allow Dogma or equipped monster to attack a second time
            if (Card.Location == CardLocation.Grave)
            {
                if (Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Damage) return true;
                return false;
            }

            // Field activation: requires a face-up Spell
            if (HasLethalOnBoard()) return false;
            bool hasTargetSpell = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsSpell());
            if (!hasTargetSpell) return false;

            _dBurstUsed = true;
            return true;
        }

        // ── Dreadmaster & Dreadnought Core Engine ──
        private bool DreadnoughtServantEffect()
        {
            if (Card == null) return false;

            // 1. Special Summon from Hand and pop our card to search Poly
            if (Card.Location == CardLocation.Hand)
            {
                if (_dreadnoughtServantHandUsed) return false;
                bool hasDHero = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0xc008));
                bool hasField = Bot.HasInSpellZone(CardId.ClockTowerPrisonCityDarkCity);
                if (hasDHero || hasField)
                {
                    _dreadnoughtServantHandUsed = true;
                    return true;
                }
                return false;
            }

            // 2. In GY: Trigger when Level 8 D-HERO is Special Summoned to spin 1 opponent card to top of deck
            if (Card.Location == CardLocation.Grave)
            {
                if (_dreadnoughtServantGYUsed) return false;
                if (Enemy.GetFieldCount() == 0) return false;
                _dreadnoughtServantGYUsed = true;
                return true;
            }

            return false;
        }

        private bool DreadmasterEffect()
        {
            // Dreadmaster Special Summoned via Clock Tower: revives up to 2 D-HEROs and protects board
            return true;
        }

        private bool DreadnoughtSummon()
        {
            if (HasLethalOnBoard() || _dreadnoughtSummonUsed) return false;
            // Tribute Dreadmaster to Special Summon Dreadnought from Extra Deck
            if (Bot.HasInMonstersZone(CardId.DestinyHERODreadmaster))
            {
                _dreadnoughtSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool DreadnoughtFieldEffect()
        {
            if (_dreadnoughtSearchUsed) return false;
            _dreadnoughtSearchUsed = true;
            return true;
        }

        // ── Hand / GY Extenders ──
        private bool FurnaceEffect()
        {
            if (Card == null) return false;
            if (Card.Location == CardLocation.Hand)
            {
                if (_furnaceHandUsed || HasLethalOnBoard()) return false;
                _furnaceHandUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (_furnaceGYUsed) return false;
                _furnaceGYUsed = true;
                return true;
            }
            return false;
        }

        private bool DuskCrowEffect()
        {
            if (Card == null) return false;
            if (Card.Location == CardLocation.Hand)
            {
                if (_duskCrowHandUsed || HasLethalOnBoard()) return false;
                bool hasGYHero = Bot.Graveyard.Any(c => c != null && c.HasSetcode(0x8) && c.Id != CardId.DestinyHEROMalicious && c.Id != CardId.DestinyHERODenier);
                if (!hasGYHero) return false;
                _duskCrowHandUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_duskCrowSummonUsed) return false;
                _duskCrowSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool FountainEffect()
        {
            if (Card == null) return false;
            if (Card.Location == CardLocation.Hand)
            {
                if (_fountainHandUsed || HasLethalOnBoard()) return false;
                bool hasHero = Bot.Hand.Any(c => c != null && c != Card && c.HasSetcode(0x8) && c.IsMonster());
                if (!hasHero) return false;
                _fountainHandUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (_fountainGYUsed) return false;
                _fountainGYUsed = true;
                return true;
            }
            return false;
        }

        private bool MaliciousEffect()
        {
            if (GetRemainingCount(CardId.DestinyHEROMalicious) == 0) return false;
            var apollousa = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.Id == 4280258 && c.IsFaceup() && !c.IsDisabled() && c.Attack > 0);
            if (apollousa != null) return false;
            return true;
        }

        private bool DenierEffect()
        {
            if (Card == null) return false;
            if (Card.Location == CardLocation.Grave)
            {
                if (_denierGYUsed) return false;
                bool hasOtherDHero = Bot.GetMonsters().Concat(Bot.Graveyard).Any(c => c != null && c.HasSetcode(0xc008) && c != Card);
                if (!hasOtherDHero) return false;
                _denierGYUsed = true;
                return true;
            }
            return true;
        }

        // ── Fusion Spells ──
        private bool FusionDestinyEffect()
        {
            if (HasLethalOnBoard()) return false;
            if (IsSpecialSummonBlocked()) return false;
            var bait = GetBaitIfNeeded(Card);
            if (bait != null) return false;

            if (GetRemainingCount(CardId.DestinyHERODestroyerPhoenixEnforcer) > 0)
                AI.SelectCard(CardId.DestinyHERODestroyerPhoenixEnforcer);
            else if (GetRemainingCount(CardId.DestinyHERODominance) > 0)
                AI.SelectCard(CardId.DestinyHERODominance);
            else
                AI.SelectCard(CardId.DestinyHERODystopia);

            return true;
        }

        private bool PolymerizationEffect()
        {
            if (IsSpecialSummonBlocked() || HasLethalOnBoard()) return false;

            var mats = Bot.Hand.Concat(Bot.GetMonsters()).Where(c => c != null && c.IsMonster() && !IsAceCard(c)).ToList();
            var dHeroes = mats.Where(c => c.HasSetcode(0xc008)).ToList();
            var allHeroes = mats.Where(c => c.HasSetcode(0x8)).ToList();

            // Trinity OTK check (requires 3 HEROs)
            if (Duel.Player == 0 && Duel.Turn > 1 && Enemy.GetMonsterCount() >= 1 && allHeroes.Count >= 3 && GetRemainingCount(CardId.VisionHEROTrinity) > 0)
            {
                AI.SelectCard(CardId.VisionHEROTrinity);
                return true;
            }

            // DPE check
            bool canSummonDPE = dHeroes.Any() && allHeroes.Any(c => c.Level >= 6 && !dHeroes.Contains(c));
            if (canSummonDPE && GetRemainingCount(CardId.DestinyHERODestroyerPhoenixEnforcer) > 0)
            {
                AI.SelectCard(CardId.DestinyHERODestroyerPhoenixEnforcer);
                return true;
            }

            // Dreadnought check (2 Level 5+ D-HEROs)
            bool canSummonDreadnought = dHeroes.Count(c => c.Level >= 5) >= 2;
            if (canSummonDreadnought && GetRemainingCount(CardId.DestinyHERODreadnought) > 0)
            {
                AI.SelectCard(CardId.DestinyHERODreadnought);
                return true;
            }

            // Dominance check (3 D-HEROs)
            if (dHeroes.Count >= 3 && GetRemainingCount(CardId.DestinyHERODominance) > 0)
            {
                AI.SelectCard(CardId.DestinyHERODominance);
                return true;
            }

            // Dystopia check (2 D-HEROs)
            if (dHeroes.Count >= 2 && GetRemainingCount(CardId.DestinyHERODystopia) > 0)
            {
                AI.SelectCard(CardId.DestinyHERODystopia);
                return true;
            }

            // Contrast HERO Chaos (2 Masked HEROs)
            var maskedHeroes = mats.Where(c => c.HasSetcode(0xa008)).ToList();
            if (maskedHeroes.Count >= 2 && GetRemainingCount(CardId.ContrastHEROChaos) > 0)
            {
                AI.SelectCard(CardId.ContrastHEROChaos);
                return true;
            }

            return false;
        }

        private bool MaskChangeEffect()
        {
            if (IsSpecialSummonBlocked() || HasLethalOnBoard()) return false;
            if (GetRemainingCount(CardId.MaskedHERODarkLaw) == 0) return false;

            var targets = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Dark) && !IsAceCard(c)).ToList();
            if (targets.Count == 0) return false;

            // In Battle Phase: activate after attacking
            if (Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Damage)
            {
                var attacker = targets.FirstOrDefault(c => c.Attacked);
                if (attacker != null)
                {
                    AI.SelectCard(attacker);
                    return true;
                }
                return false;
            }

            var bestTarget = targets.FirstOrDefault(c => c.Id == CardId.ElementalHEROShadowMist)
                          ?? targets.FirstOrDefault(c => c.Id == CardId.VisionHEROVyon)
                          ?? targets.FirstOrDefault(c => c.Id == CardId.DestinyHEROMalicious)
                          ?? targets.FirstOrDefault(c => c.Id == CardId.DestinyHERODenier)
                          ?? targets.FirstOrDefault(c => c.Id == CardId.DestinyHERODreadnoughtServant)
                          ?? targets.FirstOrDefault(c => c.Id == CardId.DestinyHERODoomLiege)
                          ?? targets.FirstOrDefault();

            if (bestTarget != null)
            {
                AI.SelectCard(bestTarget);
                return true;
            }

            return false;
        }

        private bool SuperPolymerizationEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            AI.SelectCard(CardId.MudragonOfTheSwamp, CardId.DestinyHERODestroyerPhoenixEnforcer, CardId.VisionHEROTrinity);
            return true;
        }

        // ── Boss Monsters: Summons & Effects ──
        private bool DeathDogmaSummon()
        {
            if (IsSpecialSummonBlocked() || _deathDogmaSummonUsed || HasLethalOnBoard()) return false;
            var banishCandidates = Bot.Graveyard.Where(c => c != null
                && (c.HasRace(CardRace.Warrior) || c.HasAttribute(CardAttribute.Dark))
                && c.Id != CardId.DestinyHEROMalicious
                && c.Id != CardId.DestinyHERODenier
                && !IsAceCard(c)).ToList();

            if (banishCandidates.Count >= 3)
            {
                _deathDogmaSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool DeathDogmaEffect()
        {
            if (Card == null) return false;
            // Quick Effect during opponent's turn: Fusion Summon by shuffling materials into deck!
            if (Duel.Player == 1 && !_deathDogmaQuickUsed)
            {
                if (Duel.LastChainPlayer == 1)
                {
                    _deathDogmaQuickUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool PlasmaSummon()
        {
            if (IsSpecialSummonBlocked() || HasLethalOnBoard()) return false;
            // Never tribute our Ace cards for Plasma!
            var tributeCandidates = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            return tributeCandidates.Count >= 3;
        }

        private bool PlasmaEffect()
        {
            // Plasma absorb effect: steal highest ATK face-up opponent monster
            var oppMonster = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c))
                                  .OrderByDescending(c => c.Attack).FirstOrDefault();
            if (oppMonster != null)
            {
                AI.SelectCard(oppMonster);
                return true;
            }
            return false;
        }

        private bool DogmaSummon()
        {
            if (IsSpecialSummonBlocked() || HasLethalOnBoard()) return false;
            var mats = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            bool hasDHero = mats.Any(c => c.HasSetcode(0xc008));
            return hasDHero && mats.Count >= 3;
        }

        private bool DPEEffect()
        {
            if (Card == null) return false;
            // In Graveyard during Standby Phase: revive self!
            if (Card.Location == CardLocation.Grave) return true;

            // Field pop effect: pop 1 our + 1 opp card
            if (Enemy.GetFieldCount() == 0) return false;
            if (Duel.LastChainPlayer == 1) return true;
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main2) return true;
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Main1)) return true;

            return false;
        }

        private bool DystopiaEffect()
        {
            if (Card != null && Card.Location == CardLocation.MonsterZone)
            {
                if (Card.Attack != Card.BaseAttack)
                    return Enemy.GetFieldCount() > 0;
            }
            return true;
        }

        private bool DusktopiaEffect()
        {
            // Protect our boss or prevent battle damage
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));
        }

        private bool ContrastHEROChaosEffect()
        {
            // Quick Effect: negate 1 face-up card
            var negTarget = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                                 .FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && IsViableEffectTarget(c));
            if (negTarget != null)
            {
                AI.SelectCard(negTarget);
                return true;
            }
            return false;
        }

        private bool DarkLawEffect() => Enemy.Hand.Count > 0;
        private bool DominanceEffect() => true;

        private bool DangerousEffect()
        {
            if (Duel.Player == 0 && Duel.Phase >= DuelPhase.Battle) return false;
            return Bot.Hand.Any(c => c != null && !IsAceCard(c));
        }

        // ── Normal Summons ──
        private bool SafeTributeCheck()
        {
            if (Card == null || HasLethalOnBoard()) return false;
            int req = Card.Level >= 7 ? 2 : (Card.Level >= 5 ? 1 : 0);
            if (req == 0) return false;
            int availableNonAce = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return availableNonAce >= req;
        }

        private bool VyonSummon() => !HasLethalOnBoard();
        private bool DoomLiegeSummon() => !HasLethalOnBoard();
        private bool DreadnoughtServantSummon() => !HasLethalOnBoard();
        private bool ShadowMistSummon() => !HasLethalOnBoard();
        private bool DuskCrowSummon() => !HasLethalOnBoard();

        // ── Monster Field Effects ──
        private bool VyonEffect()
        {
            if (Card == null) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Dump effect
                return true;
            }
            return true;
        }

        private bool DoomLiegeEffect()
        {
            if (Card == null) return false;
            // Trigger 1: Banish opp monster on summon
            if (!_doomLiegeSummonUsed && Enemy.GetMonsterCount() > 0)
            {
                var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsViableEffectTarget(c));
                if (target != null)
                {
                    _doomLiegeSummonUsed = true;
                    AI.SelectCard(target);
                    return true;
                }
            }
            // Trigger 2: Send D-HERO from Deck to search Clock Tower
            if (!_doomLiegeDeckUsed && !HasLethalOnBoard())
            {
                _doomLiegeDeckUsed = true;
                return true;
            }
            return false;
        }

        private bool ShadowMistEffect() => true;

        // ── Extra Deck: Link & Fusion Summons ──
        private bool CrossCrusaderSummon()
        {
            if (!ExtraDeckNow() || Bot.HasInMonstersZone(CardId.XtraHEROCrossCrusader)) return false;
            var mats = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Warrior) && !IsAceCard(c)).ToList();
            if (mats.Count < 2) return false;

            bool hasGYRevive = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasSetcode(0xc008) && c.IsCanRevive());
            bool matIsDHero = mats.Any(c => c.HasSetcode(0xc008));
            return hasGYRevive || matIsDHero;
        }

        private bool CrossCrusaderEffect()
        {
            if (ActivateDescription == Util.GetStringId(CardId.XtraHEROCrossCrusader, 1))
            {
                // Tributing cost: must have non-Ace D-HERO to tribute
                var dHeroes = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasSetcode(0xc008)).ToList();
                if (dHeroes.All(c => IsAceCard(c))) return false;
                _crossCrusaderSearchUsed = true;
            }
            return true;
        }

        private bool WonderDriverSummon()
        {
            if (!ExtraDeckNow() || Bot.HasInMonstersZone(CardId.XtraHEROWonderDriver)) return false;
            bool hasTargetSpell = Bot.Graveyard.Any(c => c != null && c.IsSpell()
                && (c.Id == CardId.Polymerization || c.Id == CardId.FusionDestiny || c.Id == CardId.MaskChange));
            if (!hasTargetSpell) return false;

            var mats = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasSetcode(0x8) && !IsAceCard(c)).ToList();
            return mats.Count >= 2;
        }

        private bool WonderDriverEffect()
        {
            _wonderDriverUsed = true;
            return true;
        }

        private bool DreadDecimatorSummon()
        {
            if (!ExtraDeckNow() || Bot.HasInMonstersZone(CardId.XtraHERODreadDecimator)) return false;
            var mats = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasSetcode(0x8) && !IsAceCard(c)).ToList();
            return mats.Count >= 2;
        }

        private bool DreadDecimatorEffect() => Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasSetcode(0xc008) && c.IsCanRevive());

        private bool TrinitySummon()
        {
            if (Duel.Turn == 1 || HasLethalOnBoard()) return false;
            // 5000 ATK 3 attacks on monsters: best when opponent has monsters to attack
            return Enemy.GetMonsterCount() >= 1;
        }

        // ── Selection Overrides ──
        private IList<ClientCard> SelectPreferred(IList<ClientCard> cards, int min, int max, params int[] preferredIds)
        {
            var result = new List<ClientCard>();
            foreach (int id in preferredIds)
            {
                var matches = cards.Where(c => c != null && c.Id == id && !result.Contains(c)).ToList();
                foreach (var m in matches)
                {
                    result.Add(m);
                    if (result.Count >= max) break;
                }
                if (result.Count >= max) break;
            }
            if (result.Count < min)
            {
                foreach (var card in cards)
                {
                    if (card != null && !result.Contains(card))
                    {
                        result.Add(card);
                        if (result.Count >= min) break;
                    }
                }
            }
            return result;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // ── Hint 506: Search from Deck to Hand (HINTMSG_ATOHAND) ──
            if (hint == 506)
            {
                if (Card != null && Card.Id == CardId.DestinyHERODreadnought)
                {
                    // Dreadnought searches TWO D-HERO cards to hand!
                    return SelectPreferred(cards, min, max,
                        CardId.FusionDestiny,
                        CardId.DestinyHEROPlasma,
                        CardId.DForce,
                        CardId.DestinyHERODeathDogma,
                        CardId.DestinyHERODreadnoughtServant,
                        CardId.DestinyHERODoomLiege,
                        CardId.DBurst,
                        CardId.DestinyHEROMalicious,
                        CardId.DestinyHERODenier);
                }

                if (Card != null && Card.Id == CardId.ClockTowerPrisonCityDarkCity)
                {
                    // Clock Tower search on field
                    bool hasServant = Bot.Hand.Any(c => c != null && c.Id == CardId.DestinyHERODreadnoughtServant) || Bot.HasInMonstersZone(CardId.DestinyHERODreadnoughtServant);
                    if (!hasServant && cards.Any(c => c.Id == CardId.DestinyHERODreadnoughtServant))
                        return SelectPreferred(cards, min, max, CardId.DestinyHERODreadnoughtServant);

                    return SelectPreferred(cards, min, max,
                        CardId.FusionDestiny,
                        CardId.DestinyHERODoomLiege,
                        CardId.DestinyHEROPlasma,
                        CardId.DestinyHERODeathDogma,
                        CardId.DestinyHERODenier,
                        CardId.DestinyHEROMalicious);
                }

                if (Card != null && Card.Id == CardId.XtraHEROCrossCrusader)
                {
                    bool hasPlasma = Bot.Hand.Any(c => c != null && c.Id == CardId.DestinyHEROPlasma) || Bot.HasInMonstersZone(CardId.DestinyHEROPlasma);
                    bool canPlasma = Bot.GetMonsterCount() >= 3;
                    if (canPlasma && !hasPlasma && cards.Any(c => c.Id == CardId.DestinyHEROPlasma))
                        return SelectPreferred(cards, min, max, CardId.DestinyHEROPlasma);

                    return SelectPreferred(cards, min, max,
                        CardId.DestinyHERODeathDogma,
                        CardId.DestinyHERODoomLiege,
                        CardId.VisionHEROVyon,
                        CardId.DestinyHERODenier,
                        CardId.ElementalHEROShadowMist,
                        CardId.DestinyHERODreadnoughtServant);
                }

                if (Card != null && Card.Id == CardId.ElementalHEROShadowMist)
                {
                    if (cards.Any(c => c.Id == CardId.MaskChange))
                        return SelectPreferred(cards, min, max, CardId.MaskChange);
                    return SelectPreferred(cards, min, max,
                        CardId.DestinyHEROPlasma,
                        CardId.VisionHEROVyon,
                        CardId.DestinyHERODreadnoughtServant,
                        CardId.DestinyHERODoomLiege,
                        CardId.DestinyHERODenier);
                }

                if (Card != null && Card.Id == CardId.SabatielThePhilosophersStone)
                    return SelectPreferred(cards, min, max, CardId.FusionDestiny, CardId.Polymerization, CardId.SuperPolymerization);

                if (Card != null && Card.Id == CardId.MaskedHEROFurnace)
                    return SelectPreferred(cards, min, max, CardId.MaskChange, CardId.Polymerization);

                if (Card != null && Card.Id == CardId.MaskedHERODuskCrow)
                    return SelectPreferred(cards, min, max, CardId.MaskedHEROFurnace, CardId.MaskedHEROFountain);

                if (Card != null && Card.Id == CardId.ReinforcementOfTheArmy)
                {
                    bool hasField = Bot.HasInSpellZone(CardId.ClockTowerPrisonCityDarkCity) || Bot.Hand.Any(c => c != null && c.Id == CardId.ClockTowerPrisonCityDarkCity);
                    bool hasServant = Bot.Hand.Any(c => c != null && c.Id == CardId.DestinyHERODreadnoughtServant) || Bot.HasInMonstersZone(CardId.DestinyHERODreadnoughtServant);
                    if (hasField && !hasServant && cards.Any(c => c.Id == CardId.DestinyHERODreadnoughtServant))
                        return SelectPreferred(cards, min, max, CardId.DestinyHERODreadnoughtServant);

                    return SelectPreferred(cards, min, max,
                        CardId.VisionHEROVyon,
                        CardId.ElementalHEROShadowMist,
                        CardId.DestinyHERODoomLiege,
                        CardId.DestinyHERODreadnoughtServant,
                        CardId.DestinyHERODenier,
                        CardId.MaskedHERODuskCrow);
                }
            }

            // ── Hint 501: Discard / Send from Hand (HINTMSG_DISCARD) ──
            if (hint == 501)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 900;
                    if (c.Id == CardId.DestinyHEROMalicious) return 10;
                    if (c.Id == CardId.ElementalHEROShadowMist) return 20;
                    if (c.Id == CardId.DestinyHERODenier) return 30;
                    if (c.Id == CardId.DestinyHERODreadnoughtServant) return 40;
                    if (c.Id == CardId.MaskedHEROFountain) return 50;
                    if (Bot.Hand.Count(h => h != null && h.Id == c.Id) > 1) return 60; // duplicates
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // ── Hint 502: Destruction (HINTMSG_DESTROY) ──
            if (hint == 502)
            {
                bool isSelfPop = (Card != null && (Card.Id == CardId.DestinyHERODreadnoughtServant || Card.Id == CardId.DBurst))
                                || cards.All(c => c.Controller == 0);

                if (isSelfPop)
                {
                    // Popping our Clock Tower triggers Dreadmaster SS!
                    var clockTower = cards.FirstOrDefault(c => c != null && c.Id == CardId.ClockTowerPrisonCityDarkCity && c.IsFaceup());
                    if (clockTower != null) return new List<ClientCard> { clockTower };

                    // DPE is recursive, so popping DPE is second best
                    var dpe = cards.FirstOrDefault(c => c != null && c.Id == CardId.DestinyHERODestroyerPhoenixEnforcer);
                    if (dpe != null) return new List<ClientCard> { dpe };

                    var safeOurs = cards.Where(c => c != null && c.Controller == 0 && !IsAceCard(c))
                                        .OrderBy(c => GetMaterialPriority(c)).FirstOrDefault();
                    if (safeOurs != null) return new List<ClientCard> { safeOurs };
                }

                // Opponent cards destruction (e.g. DPE pop, Dreadnought Servant, Dystopia)
                var enemyCards = cards.Where(c => c != null && c.Controller == 1 && IsViableEffectTarget(c)).ToList();
                if (enemyCards.Count >= min)
                {
                    var sorted = enemyCards.OrderByDescending(c => GetThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }

                // Self target for DPE (destroy 1 our + 1 opp card)
                var ourPop = cards.Where(c => c != null && c.Controller == 0).ToList();
                if (ourPop.Count > 0)
                {
                    var selfTarget = ourPop.FirstOrDefault(c => c.Id == CardId.DestinyHERODestroyerPhoenixEnforcer)
                                  ?? ourPop.FirstOrDefault(c => c.Id == CardId.ClockTowerPrisonCityDarkCity)
                                  ?? ourPop.FirstOrDefault(c => !IsAceCard(c))
                                  ?? ourPop.OrderBy(c => c.Attack).First();
                    if (selfTarget != null) return new List<ClientCard> { selfTarget };
                }
            }

            // ── Hint 503 / 504: Banish / Remove (HINTMSG_REMOVE) ──
            if (hint == 503 || hint == 504)
            {
                var oppTargets = cards.Where(c => c != null && c.Controller == 1 && IsViableEffectTarget(c)).ToList();
                if (oppTargets.Count >= min)
                {
                    var sorted = oppTargets.OrderByDescending(c => GetThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }

                // Friendly banish for cost (Death Dogma / Dusk Crow / Dreadnought Servant)
                var safeBanish = cards.Where(c => c != null && c.Controller == 0
                    && c.Id != CardId.DestinyHEROMalicious
                    && c.Id != CardId.DestinyHERODenier
                    && !IsAceCard(c)).OrderBy(c => GetMaterialPriority(c)).ToList();

                if (safeBanish.Count >= min) return safeBanish.Take(max).ToList();
            }

            // ── Hint 505: Bounce / Spin / Recycle to Deck (HINTMSG_RTOHAND / HINTMSG_TODECK) ──
            if (hint == 505)
            {
                // Denier returning banished card: recycle Malicious!
                var mal = cards.FirstOrDefault(c => c != null && c.Id == CardId.DestinyHEROMalicious);
                if (mal != null) return new List<ClientCard> { mal };

                var oppTargets = cards.Where(c => c != null && c.Controller == 1 && IsViableEffectTarget(c)).ToList();
                if (oppTargets.Count >= min)
                    return oppTargets.OrderByDescending(c => GetThreatScore(c)).Take(max).ToList();
            }

            // ── Hint 508: Send to GY (HINTMSG_TOGRAVE) ──
            if (hint == 508)
            {
                int malInGY = Bot.Graveyard.Count(c => c != null && c.Id == CardId.DestinyHEROMalicious);
                if (malInGY == 0 && cards.Any(c => c.Id == CardId.DestinyHEROMalicious))
                    return SelectPreferred(cards, min, max, CardId.DestinyHEROMalicious);

                return SelectPreferred(cards, min, max,
                    CardId.ElementalHEROShadowMist,
                    CardId.DestinyHEROMalicious,
                    CardId.DestinyHERODenier,
                    CardId.DestinyHERODeathDogma,
                    CardId.ClockTowerPrisonCityDarkCity);
            }

            // ── Hint 509: Special Summon / Revival (HINTMSG_SPSUMMON) ──
            if (hint == 509)
            {
                // Clock Tower destroyed -> Summon Dreadmaster from Deck!
                if (Card != null && Card.Id == CardId.ClockTowerPrisonCityDarkCity)
                {
                    if (cards.Any(c => c.Id == CardId.DestinyHERODreadmaster))
                        return SelectPreferred(cards, min, max, CardId.DestinyHERODreadmaster, CardId.DestinyHEROPlasma, CardId.DestinyHERODoomLiege);
                }

                // DPE Standby revival -> DPE!
                var dpe = cards.FirstOrDefault(c => c != null && c.Id == CardId.DestinyHERODestroyerPhoenixEnforcer && c.Location == CardLocation.Grave);
                if (dpe != null) return new List<ClientCard> { dpe };

                // Dreadmaster GY revival -> Revive Doom Liege and Denier!
                if (Card != null && Card.Id == CardId.DestinyHERODreadmaster)
                {
                    return SelectPreferred(cards, min, max,
                        CardId.DestinyHERODoomLiege,
                        CardId.DestinyHERODenier,
                        CardId.DestinyHEROMalicious,
                        CardId.DestinyHERODreadnoughtServant);
                }

                // Cross Crusader GY revival -> Revive Malicious or Denier
                if (Card != null && Card.Id == CardId.XtraHEROCrossCrusader)
                {
                    return SelectPreferred(cards, min, max,
                        CardId.DestinyHEROMalicious,
                        CardId.DestinyHERODenier,
                        CardId.DestinyHERODoomLiege,
                        CardId.DestinyHERODreadnoughtServant);
                }

                var sorted = cards.OrderByDescending(c => {
                    if (c == null) return -999;
                    int score = (c.Controller == 0) ? 1000 : 500;
                    if (IsAceCard(c)) score += 5000;
                    if (c.Id == CardId.DestinyHERODestroyerPhoenixEnforcer) score += 4000;
                    if (c.Id == CardId.VisionHEROTrinity) score += 3500;
                    if (c.Id == CardId.MaskedHERODarkLaw) score += 3000;
                    if (c.Id == CardId.DestinyHERODreadnought) score += 2500;
                    if (c.Id == CardId.DestinyHEROPlasma) score += 2000;
                    return score + c.Attack;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // ── Hint 500 / 512 / 513 / 533: Tribute / Material Selection ──
            if (hint == 500 || hint == 512 || hint == 513 || hint == 533)
            {
                var safe = cards.Where(c => c != null && !IsAceCard(c)).OrderBy(c => GetMaterialPriority(c)).ToList();
                if (safe.Count >= min) return safe.Take(max).ToList();
            }

            // Protect our Ace cards from generic selections
            if (cards.Any(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone && IsAceCard(c)))
            {
                var safeCards = cards.Where(c => c == null || c.Controller != 0 || c.Location != CardLocation.MonsterZone || !IsAceCard(c)).ToList();
                if (safeCards.Count >= min)
                    return safeCards.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectFusionMaterial(cards, min, max);

            // Super Polymerization: use opponent monsters first
            if (Card != null && Card.Id == CardId.SuperPolymerization)
            {
                var oppMonsters = cards.Where(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone).ToList();
                var resultSP = oppMonsters.Take(max).ToList();
                if (resultSP.Count < min)
                {
                    var safeOurs = cards.Where(c => c != null && c.Controller == 0 && !IsAceCard(c)).ToList();
                    foreach (var c in safeOurs) { if (!resultSP.Contains(c)) resultSP.Add(c); if (resultSP.Count >= min) break; }
                }
                if (resultSP.Count >= min) return resultSP;
            }

            // Fusion Destiny (Deck / Hand): send Malicious + Denier/Shadow Mist
            bool isDeckFusion = cards.Any(c => c != null && c.Location == CardLocation.Deck);
            if (isDeckFusion || (Card != null && Card.Id == CardId.FusionDestiny))
            {
                var deckHand = cards.Where(c => c != null && (c.Location == CardLocation.Deck || c.Location == CardLocation.Hand)).ToList();
                var resultDF = new List<ClientCard>();
                int malSelected = 0;
                int[] preferredIds = {
                    CardId.DestinyHEROMalicious,
                    CardId.DestinyHERODenier,
                    CardId.ElementalHEROShadowMist,
                    CardId.DestinyHERODoomLiege,
                    CardId.DestinyHERODreadnoughtServant,
                    CardId.DestinyHERODogma,
                    CardId.DestinyHERODreadmaster
                };

                foreach (int id in preferredIds)
                {
                    var matches = deckHand.Where(c => c != null && c.Id == id && !resultDF.Contains(c)).ToList();
                    foreach (var m in matches)
                    {
                        if (id == CardId.DestinyHEROMalicious)
                        {
                            if (malSelected >= 1) continue;
                            malSelected++;
                        }
                        resultDF.Add(m);
                        if (resultDF.Count >= max) break;
                    }
                    if (resultDF.Count >= max) break;
                }
                if (resultDF.Count >= min) return resultDF;
            }

            // Generic Fusion (Hand, GY, Field): protect Ace monsters
            var result = new List<ClientCard>();
            var handMats = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
            var gyMats = cards.Where(c => c != null && c.Location == CardLocation.Grave).ToList();
            var fieldSafe = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && !IsAceCard(c)).ToList();

            foreach (var c in handMats) { if (!result.Contains(c)) result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max) foreach (var c in gyMats) { if (!result.Contains(c)) result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max) foreach (var c in fieldSafe) { if (!result.Contains(c)) result.Add(c); if (result.Count >= max) break; }

            if (result.Count >= min) return result;
            return base.OnSelectFusionMaterial(cards, min, max);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 4;
                if (cardId == 0 && Card != null) cardId = Card.Id;
                long optIndex = options[i] & 0xf;

                // Solemn Report: Option 0 = Pay 1500 (destroy & lock); Option 1 = Pay 3000 (banish all copies from hand & deck)
                if (cardId == CardId.SolemnReport)
                {
                    var targetCard = Util.GetLastChainCard();
                    bool isHighThreat = targetCard != null && (GetThreatScore(targetCard) >= 50 || CardIntelligence.IsHighThreatChokepoint(targetCard.Id));
                    if (Bot.LifePoints >= 4000 && isHighThreat && optIndex == 1)
                        return i;
                    if (optIndex == 0)
                        return i;
                }
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // High ATK Beatsticks & Bosses: FaceUpAttack
            if (cardId == CardId.DestinyHERODreadnought
                || cardId == CardId.DestinyHERODestroyerPhoenixEnforcer
                || cardId == CardId.VisionHEROTrinity
                || cardId == CardId.DestinyHEROPlasma
                || cardId == CardId.DestinyHERODeathDogma
                || cardId == CardId.DestinyHERODogma
                || cardId == CardId.DestinyHERODystopia
                || cardId == CardId.DestinyHERODusktopia
                || cardId == CardId.DestinyHERODominance
                || cardId == CardId.ContrastHEROChaos
                || cardId == CardId.XtraHERODreadDecimator)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            // Low ATK Utility Monsters & Handtraps: FaceUpDefence
            if (cardId == CardId.DestinyHERODreadnoughtServant
                || cardId == CardId.DestinyHERODoomLiege
                || cardId == CardId.DestinyHERODenier
                || cardId == CardId.VisionHEROVyon
                || cardId == CardId.ElementalHEROShadowMist
                || cardId == CardId.MaskedHEROFountain
                || cardId == CardId.AshBlossom
                || cardId == CardId.MulcharmyFuwalos
                || cardId == CardId.XtraHEROCrossCrusader
                || cardId == CardId.XtraHEROWonderDriver)
            {
                if (Duel.Turn == 1 || Duel.Phase != DuelPhase.Main1)
                {
                    if (positions.Contains(CardPosition.FaceUpDefence))
                        return CardPosition.FaceUpDefence;
                }
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            // Column-Safe Placement: Avoid columns with enemy Spell/Trap threats
            if (location == CardLocation.MonsterZone)
            {
                int safeMask = available;
                for (int seq = 0; seq < 5; seq++)
                {
                    if ((available & (1 << seq)) != 0)
                    {
                        var oppSpell = Enemy.SpellZone[4 - seq];
                        if (oppSpell != null && oppSpell.IsFaceup())
                        {
                            safeMask &= ~(1 << seq);
                        }
                    }
                }
                if (safeMask != 0) return safeMask;
            }

            return base.OnSelectPlace(cardId, player, location, available);
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

    [Deck("Expert_2026_Dreadnought", "2026_Dreadnought")]
    public class ExpertDreadnoughtExecutor : _2026_DreadnoughtExecutor
    {
        public ExpertDreadnoughtExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }
}
