// ============================================================
// CARD AUDIT — 2026_Branded
// | Card Name                     | Type      | OPT? | Effect                                      | Activate When               | NEVER When            |
// |-------------------------------|-----------|------|---------------------------------------------|-----------------------------|-----------------------|
// | Branded Fusion                | Normal Sp | HOPT | Fusion from Deck using Albaz + LIGHT/DARK   | Main Phase starter combo    | Extra Deck locked     |
// | Aluber the Jester of Despia   | Effect Mn | HOPT | Search Branded S/T / GY negate & SS         | NS/SS to search; GY trigger | Effect negated        |
// | Fallen of the White Dragon    | Effect Mn | HOPT | Send Albaz ED -> SS self; SS Ecclesia       | Hand starter; field trigger | SS locked             |
// | Guiding Quem the Virtuous     | Tuner Lv4 | HOPT | Dump Albaz/support; SS from GY on ED exit   | NS/SS starter; ED trigger   | SS locked             |
// | Blazing Cartesia the Virtuous | Tuner Lv4 | HOPT | SS if Albaz in field/GY; Quick Fusion       | Main Phase / opponent turn  | Board saturated       |
// | Incredible Ecclesia           | Tuner Lv4 | HOPT | SS if opp controls more; tribute -> Albaz   | SS / Synchro material       | Self-harm tribute     |
// | Tri-Brigade Springans Kitt    | Effect Mn | HOPT | GY send trigger: SS Albaz/mention from GY   | Sent to GY from Hand/Deck/ED| No targets in GY      |
// | The Golden Swordsoul          | Tuner Lv4 | HOPT | Attack negate & pop; Banished -> SS LIGHT Sp| Opponent attack / Banish trg| Bad timing            |
// | Tri-Brigade Mercourier        | Effect Mn | HOPT | Handtrap monster negate; Banished -> Search | Opponent monster eff; Banish| No Albaz fusion ctrl  |
// | Albion the Shrouded Dragon    | Effect Mn | HOPT | Dump Branded S/T or Albaz -> draw 1 card    | Hand/GY setup               | Main Phase only       |
// | Nadir Servant                 | Normal Sp | HOPT | Send ED -> Search Dogmatika/Albaz; ED lock  | Hand starter / extender     | Before fusion plays   |
// | Branded in High Spirits       | Quick-Play| HOPT | Reveal matching type -> send Lv8 ED & search| Hand starter / setup        | Invalid reveal target |
// | The Fallen & The Virtuous     | Quick-Play| HOPT | Destroy 1 face-up (send ED) / Revive from GY| Opponent threat / Turn 1 SS | Invalid target        |
// | Super Polymerization          | Quick-Play| None | Unbreakable fusion using field monsters     | Opponent board break / push | Missing discard cost  |
// | Mirrorjade the Iceblade Dragon| Fusion Lv8| SOPT | Quick non-target banish (cost ED); Wipe     | Priority disruption         | Board clear           |
// | The Dragon That Devours Dogma | Fusion Lv8| HOPT | Tower if Ecclesia; Shuffle GY; Search Merc  | Priority boss / Disruption  | SS locked             |
// | Ecclesia and the Dark Dragon  | SynchroLv8| HOPT | Quick banish -> SS Albaz from Deck/GY; Recyc| Opponent Main Phase tag-out | Non-Tuner missing     |
// | Granguignol the Dusk Dragon   | Fusion Lv8| HOPT | Send Lv6+ LIGHT/DARK from ED/Deck; Tag-out  | Fusion setup / Tag Luluwa   | Missing material      |
// | Despian Luluwalilith          | Synchro 12| HOPT | 3000 ATK; ED exit -> Negate face-up; End SS| Tagged by Granguignol / Boss| Unsummonable          |
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
    [Deck("2026_Branded", "2026_Branded")]
    public class _2026_BrandedExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int AluberTheJesterOfDespia = 62962630;
            public const int FallenOfTheWhiteDragon = 73819701;
            public const int FallenOfAlbaz = 68468459;
            public const int BlazingCartesiaTheVirtuous = 95515789;
            public const int GuidingQuemTheVirtuous = 45883110;
            public const int IncredibleEcclesiaTheVirtuous = 55273560;
            public const int TheGoldenSwordsoul = 82489470;
            public const int TriBrigadeSpringansKitt = 19304410;
            public const int TriBrigadeMercourier = 19096726;
            public const int AlbionTheShroudedDragon = 25451383;
            public const int MulcharmyFuwalos = 42141493;
            public const int AshBlossom = 14558128;
            public const int BystialMagnamhut = 33854624;

            // Main Deck Spells & Traps
            public const int BrandedFusion = 44362883;
            public const int BrandedInHighSpirits = 29948294;
            public const int NadirServant = 1984618;
            public const int TheFallenAndTheVirtuous = 30271097;
            public const int ForbiddenDroplet = 24299458;
            public const int SuperPolymerization = 48130397;
            public const int TripleTacticsTalent = 25311006;
            public const int GoldSarcophagus = 75500286;
            public const int BrandedRetribution = 17751597;
            public const int BrandedSword = 81767888;
            public const int CalledByTheGrave = 24224830;

            // Extra Deck
            public const int AlbionTheBrandedDragon = 87746184;
            public const int MirrorjadeTheIcebladeDragon = 44146295;
            public const int LubellionTheSearingDragon = 70534340;
            public const int GranguignolTheDuskDragon = 24915933;
            public const int TitanikladTheAshDragon = 41373230;
            public const int RindbrummTheStrikingDragon = 51409648;
            public const int TheDragonThatDevoursTheDogma = 76666602;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int MudragonOfTheSwamp = 54757758;
            public const int PSYFramelordOmega = 74586817;
            public const int DespianLuluwalilith = 53971455;
            public const int EcclesiaAndTheDarkDragon = 78397661;
            public const int DespianQuaeritis = 72272462;
            public const int AlbaLenatusTheAbyssDragon = 3410461;
            public const int BorreloadFuriousDragon = 92892239;

            // Side Deck Handtraps & Board Breakers
            public const int GhostSisterAndSpookyDogwood = 29726552;
            public const int DrollAndLockBird = 94145021;
            public const int HarpiesFeatherDuster = 18144507;
            public const int LightningStorm = 14532163;
            public const int Raigeki = 12580477;
            public const int DimensionalBarrier = 33017964;
            public const int CosmicCyclone = 35269904;
            public const int RedReboot = 41420027;

            // Threat / Floodgate card references
            public const int EternalSoul = 48680970;
            public const int DarkMagicalCircle = 47222536;
            public const int DarkMagicianDragonKnight = 41721210;
            public const int ABCDragonBuster = 1561110;
            public const int SecretVillageOfSpellcasters = 68462976;
            public const int AltergeistProtocol = 27541563;
            public const int AltergeistHexstia = 1508649;
            public const int PersonalSpoofing = 53936268;
            public const int AltergeistMultifaker = 42790071;
            public const int AltergeistMeluseek = 25533642;
            public const int AltergeistManifestation = 35146019;
            public const int UnionHangar = 66970002;
            public const int BBusterDrake = 77411244;
            public const int UnionDriver = 99249638;
            public const int GalaxySoldier = 46659709;
            public const int MaxxC = 23434538;
            public const int PotOfDesires = 35261759;
            public const int PotOfExtravagance = 72426662;
            public const int ImperialOrder = 61740673;
            public const int AntiSpellFragrance = 58921041;
            public const int NaturiaBeast = 33198837;
            public const int SkillDrain = 82732705;
        }

        private static readonly int[] HighThreatChokepoints = {
            CardId.AltergeistMultifaker,
            CardId.PersonalSpoofing,
            CardId.AltergeistMeluseek,
            CardId.AltergeistHexstia,
            CardId.AltergeistProtocol,
            CardId.AltergeistManifestation,
            CardId.UnionHangar,
            CardId.BBusterDrake,
            CardId.UnionDriver,
            CardId.GalaxySoldier,
            CardId.ABCDragonBuster,
            CardId.EternalSoul,
            CardId.DarkMagicalCircle,
            CardId.DarkMagicianDragonKnight,
            CardId.SkillDrain,
            CardId.MaxxC,
            CardId.PotOfDesires,
            CardId.PotOfExtravagance,
            71039903, // White Stone of Ancients
            79814787, // White Stone of Legend
            8240199,  // Sage with Eyes of Blue
            48800175, // Melody of Awakening Dragon
            43722862, // Windwitch Ice Bell
            71007216, // Windwitch Glass Bell
            89739383, // Spellbook of Secrets
            23314220  // Spellbook of Knowledge
        };

        private static readonly int[] AceCardIds = {
            CardId.MirrorjadeTheIcebladeDragon,
            CardId.TheDragonThatDevoursTheDogma,
            CardId.DespianLuluwalilith,
            CardId.EcclesiaAndTheDarkDragon,
            CardId.GranguignolTheDuskDragon,
            CardId.RindbrummTheStrikingDragon,
            CardId.AlbionTheBrandedDragon,
            CardId.LubellionTheSearingDragon,
            CardId.TitanikladTheAshDragon,
            CardId.PSYFramelordOmega
        };

        private static readonly int[] EcclesiaIds = {
            CardId.IncredibleEcclesiaTheVirtuous,
            CardId.EcclesiaAndTheDarkDragon
        };

        private static readonly int[] ValidReviveIds = {
            CardId.MirrorjadeTheIcebladeDragon,
            CardId.TheDragonThatDevoursTheDogma,
            CardId.DespianLuluwalilith,
            CardId.GranguignolTheDuskDragon,
            CardId.AlbionTheBrandedDragon,
            CardId.LubellionTheSearingDragon,
            CardId.RindbrummTheStrikingDragon,
            CardId.TitanikladTheAshDragon,
            CardId.GuidingQuemTheVirtuous,
            CardId.BlazingCartesiaTheVirtuous,
            CardId.FallenOfTheWhiteDragon,
            CardId.FallenOfAlbaz,
            CardId.IncredibleEcclesiaTheVirtuous,
            CardId.TriBrigadeSpringansKitt,
            CardId.TheGoldenSwordsoul
        };

        // Turn tracking states
        private bool _nadirServantUsed = false;
        private bool _brandedFusionUsed = false;
        private bool _brandedHighSpiritsUsed = false;
        private bool _fallenVirtuousUsed = false;
        private bool _whiteDragonHandUsed = false;
        private bool _whiteDragonFieldUsed = false;
        private bool _cartesiaUsed = false;
        private bool _mirrorjadeUsed = false;
        private bool _lubellionUsed = false;
        private int _handTrapsUsedThisTurn = 0;
        private bool _goldSwordsoulUsed = false;
        private bool _springansKittUsed = false;
        private bool _tttUsed = false;
        private bool _goldSarcUsed = false;

        public _2026_BrandedExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Ace monster protection from link/synchro sacrifice or premature discard
            HeuristicGuard.RegisterAceCards(AceCardIds);
            ResourcePlan.RegisterAceCards(AceCardIds);

            // ── Combo Router: Strategic Multi-Route Sequencing ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "BrandedFusion-Main",
                RequiredCards = new List<int> { CardId.BrandedFusion },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.BrandedFusion, ActionType = ExecutorType.Activate, Description = "Activate Branded Fusion" },
                    new() { CardId = CardId.LubellionTheSearingDragon, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Lubellion" },
                    new() { CardId = CardId.MirrorjadeTheIcebladeDragon, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Mirrorjade" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Aluber-To-BrandedFusion",
                RequiredCards = new List<int> { CardId.AluberTheJesterOfDespia },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AluberTheJesterOfDespia, ActionType = ExecutorType.Summon, Description = "Normal Summon Aluber" },
                    new() { CardId = CardId.AluberTheJesterOfDespia, ActionType = ExecutorType.Activate, Description = "Search Branded Fusion" },
                    new() { CardId = CardId.BrandedFusion, ActionType = ExecutorType.Activate, Description = "Activate Branded Fusion" },
                    new() { CardId = CardId.LubellionTheSearingDragon, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Lubellion" },
                    new() { CardId = CardId.MirrorjadeTheIcebladeDragon, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Mirrorjade" }
                },
                EndBoardScore = 92
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "WhiteDragon-Ecclesia-Synchro",
                RequiredCards = new List<int> { CardId.FallenOfTheWhiteDragon },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.FallenOfTheWhiteDragon, ActionType = ExecutorType.Activate, Description = "Send ED Fusion and SS White Dragon" },
                    new() { CardId = CardId.FallenOfTheWhiteDragon, ActionType = ExecutorType.Activate, Description = "SS Incredible Ecclesia from Deck" },
                    new() { CardId = CardId.EcclesiaAndTheDarkDragon, ActionType = ExecutorType.SpSummon, Description = "Synchro Summon Ecclesia & Dark Dragon" }
                },
                EndBoardScore = 88
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Quem-Cartesia-Granguignol",
                RequiredCards = new List<int> { CardId.GuidingQuemTheVirtuous },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.GuidingQuemTheVirtuous, ActionType = ExecutorType.Summon, Description = "Normal Summon Quem" },
                    new() { CardId = CardId.GuidingQuemTheVirtuous, ActionType = ExecutorType.Activate, Description = "Dump Cartesia or Shrouded Dragon" },
                    new() { CardId = CardId.BlazingCartesiaTheVirtuous, ActionType = ExecutorType.SpSummon, Description = "SS Cartesia from Hand" },
                    new() { CardId = CardId.GranguignolTheDuskDragon, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Granguignol" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "NadirServant-Quem",
                RequiredCards = new List<int> { CardId.NadirServant },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.NadirServant, ActionType = ExecutorType.Activate, Description = "Send Dogma Dragon/Albion & Search Quem" },
                    new() { CardId = CardId.GuidingQuemTheVirtuous, ActionType = ExecutorType.Summon, Description = "Normal Summon Quem" },
                    new() { CardId = CardId.GuidingQuemTheVirtuous, ActionType = ExecutorType.Activate, Description = "Quem dumps Albaz/Cartesia" }
                },
                EndBoardScore = 82
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "GoldSarc-Mercourier",
                RequiredCards = new List<int> { CardId.GoldSarcophagus },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.GoldSarcophagus, ActionType = ExecutorType.Activate, Description = "Banish Mercourier from Deck" },
                    new() { CardId = CardId.TriBrigadeMercourier, ActionType = ExecutorType.Activate, Description = "Search White Dragon or Quem" }
                },
                EndBoardScore = 80
            });

            // ── Bait & Chain Planning ──
            BaitPlanner.RegisterComboStarters(CardId.BrandedFusion);
            BaitPlanner.RegisterBaitCards(CardId.NadirServant, CardId.GoldSarcophagus, CardId.AluberTheJesterOfDespia, CardId.BrandedInHighSpirits);
            ChainAdvisor.RegisterHighValueTargets(HighThreatChokepoints);

            // ==========================================
            // PRIORITY 1: Reactive Handtraps & Protection
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.TriBrigadeMercourier, TriBrigadeMercourierEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheGoldenSwordsoul, TheGoldenSwordsoulEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostSisterAndSpookyDogwood, GhostSisterEffect);

            // ==========================================
            // PRIORITY 2: Quick Board Breakers & Spells
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.Raigeki, RaigekiEffect);
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, DefaultHarpiesFeatherDusterFirst);
            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, CosmicCycloneEffect);

            // ==========================================
            // PRIORITY 3: Boss Quick Effects & Triggers
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.MirrorjadeTheIcebladeDragon, MirrorjadeTheIcebladeDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheDragonThatDevoursTheDogma, TheDragonThatDevoursTheDogmaEffect);
            AddExecutor(ExecutorType.Activate, CardId.RindbrummTheStrikingDragon, RindbrummTheStrikingDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.EcclesiaAndTheDarkDragon, EcclesiaAndTheDarkDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.GranguignolTheDuskDragon, GranguignolTheDuskDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.DespianLuluwalilith, DespianLuluwalilithEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlazingCartesiaTheVirtuous, BlazingCartesiaTheVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.LubellionTheSearingDragon, LubellionTheSearingDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlbionTheBrandedDragon, AlbionTheBrandedDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.TitanikladTheAshDragon, TitanikladTheAshDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.GaruraWingsOfResonantLife);
            AddExecutor(ExecutorType.Activate, CardId.MudragonOfTheSwamp, MudragonOfTheSwampEffect);
            AddExecutor(ExecutorType.Activate, CardId.PSYFramelordOmega, PSYFramelordOmegaEffect);

            // ==========================================
            // PRIORITY 4: Primary Combo Starters & Spells
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.GoldSarcophagus, GoldSarcophagusEffect);
            AddExecutor(ExecutorType.Activate, CardId.BrandedFusion, BrandedFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.BrandedInHighSpirits, BrandedInHighSpiritsEffect);
            AddExecutor(ExecutorType.Activate, CardId.NadirServant, NadirServantEffect);

            // ==========================================
            // PRIORITY 5: Inherent Summons & Monster Effects
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.FallenOfTheWhiteDragon, FallenOfTheWhiteDragonHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.FallenOfTheWhiteDragon, FallenOfTheWhiteDragonFieldEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BlazingCartesiaTheVirtuous, BlazingCartesiaTheVirtuousSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.IncredibleEcclesiaTheVirtuous, IncredibleEcclesiaTheVirtuousSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.IncredibleEcclesiaTheVirtuous, IncredibleEcclesiaTheVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.AluberTheJesterOfDespia, AluberTheJesterOfDespiaEffect);
            AddExecutor(ExecutorType.Activate, CardId.GuidingQuemTheVirtuous, GuidingQuemTheVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.TriBrigadeSpringansKitt, TriBrigadeSpringansKittEffect);
            AddExecutor(ExecutorType.Activate, CardId.FallenOfAlbaz, FallenOfAlbazEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlbionTheShroudedDragon, AlbionTheShroudedDragonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialMagnamhut, BystialMagnamhutSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialMagnamhutEffect);

            // ==========================================
            // PRIORITY 6: Normal Summons (Strategic Order)
            // ==========================================
            AddExecutor(ExecutorType.Summon, CardId.AluberTheJesterOfDespia, NormalSummonAluber);
            AddExecutor(ExecutorType.Summon, CardId.GuidingQuemTheVirtuous, NormalSummonQuem);
            AddExecutor(ExecutorType.Summon, CardId.BlazingCartesiaTheVirtuous, NormalSummonCartesia);
            AddExecutor(ExecutorType.Summon, CardId.IncredibleEcclesiaTheVirtuous, NormalSummonPriorityCheck);
            AddExecutor(ExecutorType.Summon, CardId.FallenOfAlbaz, NormalSummonAlbaz);
            AddExecutor(ExecutorType.Summon, CardId.TriBrigadeSpringansKitt, NormalSummonPriorityCheck);

            // ==========================================
            // PRIORITY 7: Extra Deck Summons
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.MirrorjadeTheIcebladeDragon, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.TheDragonThatDevoursTheDogma, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.EcclesiaAndTheDarkDragon, EcclesiaAndTheDarkDragonSynchroSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.LubellionTheSearingDragon, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.AlbionTheBrandedDragon, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.GranguignolTheDuskDragon, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.RindbrummTheStrikingDragon, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.PSYFramelordOmega, PSYFramelordOmegaSynchroSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.GaruraWingsOfResonantLife, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.MudragonOfTheSwamp, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.TitanikladTheAshDragon, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.AlbaLenatusTheAbyssDragon, FusionSummonCheck);

            // ==========================================
            // PRIORITY 8: Traps & Backrow Set
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.BrandedRetribution, BrandedRetributionEffect);
            AddExecutor(ExecutorType.Activate, CardId.BrandedSword, BrandedSwordEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionalBarrier, DimensionalBarrierEffect);
            AddExecutor(ExecutorType.Activate, CardId.RedReboot, RedRebootEffect);
            AddExecutor(ExecutorType.SpellSet, SpellSetFiltered);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _nadirServantUsed = false;
            _brandedFusionUsed = false;
            _brandedHighSpiritsUsed = false;
            _fallenVirtuousUsed = false;
            _whiteDragonHandUsed = false;
            _whiteDragonFieldUsed = false;
            _cartesiaUsed = false;
            _mirrorjadeUsed = false;
            _lubellionUsed = false;
            _handTrapsUsedThisTurn = 0;
            _goldSwordsoulUsed = false;
            _springansKittUsed = false;
            _tttUsed = false;
            _goldSarcUsed = false;
        }

        public override bool OnSelectHand()
        {
            return true;
        }

        // ==========================================
        // Safety & Board State Checks
        // ==========================================

        private bool IsTargetable(ClientCard card)
        {
            if (card == null) return false;
            return !card.IsShouldNotBeTarget();
        }

        private bool IsExtraDeckLocked()
        {
            return _nadirServantUsed;
        }

        private bool FusionSummonCheck()
        {
            if (IsExtraDeckLocked()) return false;
            return !IsSpecialSummonBlocked();
        }

        private bool SynchroSummonCheck()
        {
            if (IsExtraDeckLocked() || _brandedFusionUsed) return false;
            return !IsSpecialSummonBlocked();
        }

        protected override bool IsBoardStrongEnough()
        {
            if (BoardScore() >= 18) return true;
            if (Bot.HasInMonstersZone(CardId.MirrorjadeTheIcebladeDragon) && Bot.HasInMonstersZone(CardId.TheDragonThatDevoursTheDogma))
                return true;
            if (Bot.HasInMonstersZone(CardId.MirrorjadeTheIcebladeDragon) && Bot.GetMonsterCount() >= 2 && Bot.GetSpellCount() >= 1)
                return true;
            return base.IsBoardStrongEnough();
        }

        private bool OpponentHasThreateningMonster()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 2800 || c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)));
        }

        private bool AreSpellsNegatedOrDisabled()
        {
            if (Bot.GetSpells().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.IsCode(CardId.ImperialOrder, CardId.AntiSpellFragrance)))
                return true;

            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.IsCode(CardId.NaturiaBeast)))
                return true;

            bool enemyHasVillage = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SecretVillageOfSpellcasters) && !c.IsDisabled());
            if (enemyHasVillage)
            {
                bool enemyHasSpellcaster = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Race == (int)CardRace.SpellCaster);
                bool weHaveSpellcaster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Race == (int)CardRace.SpellCaster);
                if (enemyHasSpellcaster && !weHaveSpellcaster)
                    return true;
            }

            return false;
        }

        private bool EnemyHasSpellNegator()
        {
            int[] negators = { 84815190, 27548133, 9753964, 63767246, CardId.DarkMagicianDragonKnight };
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && negators.Contains(c.Id));
        }

        // ==========================================
        // Card Handlers — Spells & Traps
        // ==========================================

        private bool BrandedFusionEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (AreSpellsNegatedOrDisabled()) return false;
            if (_brandedFusionUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            if (EnemyHasSpellNegator() && Bot.Hand.Any(c => c != null && c.IsCode(CardId.GoldSarcophagus, CardId.NadirServant)))
                return false;

            AI.SelectCard(new[] {
                CardId.LubellionTheSearingDragon,
                CardId.AlbionTheBrandedDragon,
                CardId.TheDragonThatDevoursTheDogma,
                CardId.RindbrummTheStrikingDragon
            });

            _brandedFusionUsed = true;
            return true;
        }

        private bool TheFallenAndTheVirtuousEffect()
        {
            if (AreSpellsNegatedOrDisabled()) return false;
            if (_fallenVirtuousUsed) return false;

            bool hasEcclesia = Bot.GetMonsters().Concat(Bot.Graveyard).Any(c => c != null && (EcclesiaIds.Contains(c.Id) || (c.Name != null && c.Name.Contains("Ecclesia"))));
            ClientCard enemyFaceup = Enemy.GetMonsters().Concat(Enemy.GetSpells()).FirstOrDefault(c => c != null && c.IsFaceup() && IsTargetable(c));
            bool hasExtraSend = Bot.ExtraDeck.Any(c => c != null && c.IsCode(
                CardId.TheDragonThatDevoursTheDogma, CardId.AlbionTheBrandedDragon, CardId.TitanikladTheAshDragon, CardId.RindbrummTheStrikingDragon));

            // On Turn 1 going first, if enemy has 0 cards, use Option 1 (Revive) if available
            if (Duel.Turn == 1 && Duel.Player == 0 && enemyFaceup == null)
            {
                if (hasEcclesia)
                {
                    ClientCard reviveTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.IsCanRevive() && ValidReviveIds.Contains(c.Id));
                    if (reviveTarget != null)
                    {
                        AI.SelectOption(1);
                        AI.SelectCard(reviveTarget);
                        AI.SelectPosition(CardPosition.FaceUpDefence);
                        _fallenVirtuousUsed = true;
                        return true;
                    }
                }
                return false;
            }

            // Option 0: Destroy 1 face-up card on field
            if (enemyFaceup != null && hasExtraSend)
            {
                // Extra Deck send priority: Dogma Dragon (searches Mercourier) > Albion > Titaniklad > Rindbrumm
                AI.SelectCard(new[] {
                    CardId.TheDragonThatDevoursTheDogma,
                    CardId.AlbionTheBrandedDragon,
                    CardId.TitanikladTheAshDragon,
                    CardId.RindbrummTheStrikingDragon
                });

                // Target to destroy: Prioritize Eternal Soul, Dark Magical Circle, Altergeist Protocol, Skill Drain
                ClientCard targetToPop = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.EternalSoul, CardId.DarkMagicalCircle, CardId.AltergeistProtocol, CardId.PersonalSpoofing, CardId.SkillDrain, CardId.SecretVillageOfSpellcasters))
                    ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && (c.IsCode(CardId.DarkMagicianDragonKnight, CardId.ABCDragonBuster, CardId.AltergeistHexstia) || c.IsFloodgate() || c.Attack >= 2500) && IsTargetable(c))
                    ?? enemyFaceup;

                AI.SelectNextCard(targetToPop);
                AI.SelectOption(0);
                _fallenVirtuousUsed = true;
                return true;
            }

            // Option 1: Revive
            if (hasEcclesia)
            {
                ClientCard reviveTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.IsCanRevive() && ValidReviveIds.Contains(c.Id))
                    ?? Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.IsCanRevive() && ValidReviveIds.Contains(c.Id));

                if (reviveTarget != null)
                {
                    AI.SelectOption(1);
                    AI.SelectCard(reviveTarget);
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                    _fallenVirtuousUsed = true;
                    return true;
                }
            }

            return false;
        }

        private bool NadirServantEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (AreSpellsNegatedOrDisabled()) return false;
            if (_nadirServantUsed) return false;

            // If we have Branded Fusion or Aluber in hand, execute fusion plays first before locking Extra Deck
            bool canBrandedFusionFirst = !_brandedFusionUsed && Bot.Hand.Any(c => c.IsCode(CardId.BrandedFusion, CardId.AluberTheJesterOfDespia));
            if (canBrandedFusionFirst) return false;

            AI.SelectCard(new[] {
                CardId.TheDragonThatDevoursTheDogma,
                CardId.GranguignolTheDuskDragon,
                CardId.AlbionTheBrandedDragon,
                CardId.TitanikladTheAshDragon,
                CardId.GaruraWingsOfResonantLife
            });

            AI.SelectNextCard(new[] {
                CardId.GuidingQuemTheVirtuous,
                CardId.FallenOfAlbaz
            });

            _nadirServantUsed = true;
            return true;
        }

        private bool BrandedInHighSpiritsEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (AreSpellsNegatedOrDisabled()) return false;
            if (_brandedHighSpiritsUsed) return false;

            bool hasSpellcaster = Bot.Hand.Any(c => c != null && c.IsMonster() && c.Race == (int)CardRace.SpellCaster) &&
                                  Bot.ExtraDeck.Any(c => c != null && c.IsCode(CardId.GranguignolTheDuskDragon));
            bool hasDragon = Bot.Hand.Any(c => c != null && c.IsMonster() && c.Race == (int)CardRace.Dragon) &&
                             Bot.ExtraDeck.Any(c => c != null && c.IsCode(CardId.AlbionTheBrandedDragon, CardId.TheDragonThatDevoursTheDogma, CardId.TitanikladTheAshDragon));
            bool hasWingedBeast = Bot.Hand.Any(c => c != null && c.IsMonster() && c.Race == (int)CardRace.WindBeast) &&
                                  Bot.ExtraDeck.Any(c => c != null && c.IsCode(CardId.RindbrummTheStrikingDragon));

            if (!hasSpellcaster && !hasDragon && !hasWingedBeast) return false;

            ClientCard revealCandidate = null;
            int edTarget = 0;

            if (hasSpellcaster)
            {
                revealCandidate = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.Race == (int)CardRace.SpellCaster &&
                    (c.IsCode(CardId.IncredibleEcclesiaTheVirtuous, CardId.TheGoldenSwordsoul, CardId.BlazingCartesiaTheVirtuous, CardId.GuidingQuemTheVirtuous)));
                edTarget = CardId.GranguignolTheDuskDragon;
            }
            else if (hasDragon)
            {
                revealCandidate = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.Race == (int)CardRace.Dragon &&
                    (c.IsCode(CardId.FallenOfTheWhiteDragon, CardId.AlbionTheShroudedDragon, CardId.FallenOfAlbaz, CardId.BystialMagnamhut)));
                edTarget = CardId.TheDragonThatDevoursTheDogma;
            }
            else if (hasWingedBeast)
            {
                revealCandidate = Bot.Hand.FirstOrDefault(c => c != null && c.IsCode(CardId.TriBrigadeMercourier));
                edTarget = CardId.RindbrummTheStrikingDragon;
            }

            if (revealCandidate == null) return false;

            AI.SelectCard(revealCandidate);
            AI.SelectNextCard(edTarget);
            AI.SelectThirdCard(new[] {
                CardId.FallenOfTheWhiteDragon,
                CardId.GuidingQuemTheVirtuous,
                CardId.BlazingCartesiaTheVirtuous,
                CardId.TriBrigadeMercourier,
                CardId.FallenOfAlbaz
            });

            _brandedHighSpiritsUsed = true;
            return true;
        }

        private bool GoldSarcophagusEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (AreSpellsNegatedOrDisabled()) return false;
            if (_goldSarcUsed) return false;

            if (Bot.Deck.Any(c => c != null && c.IsCode(CardId.TriBrigadeMercourier)))
            {
                AI.SelectCard(CardId.TriBrigadeMercourier);
                _goldSarcUsed = true;
                return true;
            }

            var fallback = Bot.Deck.FirstOrDefault(c => c != null && c.IsCode(CardId.BrandedFusion, CardId.AluberTheJesterOfDespia, CardId.FallenOfTheWhiteDragon));
            if (fallback != null)
            {
                AI.SelectCard(fallback);
                _goldSarcUsed = true;
                return true;
            }

            return false;
        }

        private bool CanSuperPoly()
        {
            var allMonsters = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Where(c => c != null && c.IsFaceup()).ToList();
            if (allMonsters.Count < 2) return false;

            // Check Garura (2 monsters of same Race & Attribute, different names)
            bool canGarura = allMonsters.GroupBy(c => new { c.Race, c.Attribute })
                .Any(g => g.Select(c => c.Id).Distinct().Count() >= 2);
            if (canGarura && Bot.ExtraDeck.Any(c => c.IsCode(CardId.GaruraWingsOfResonantLife))) return true;

            // Check Mudragon (2 monsters of same Attribute, different Races)
            bool canMudragon = allMonsters.GroupBy(c => c.Attribute)
                .Any(g => g.Select(c => c.Race).Distinct().Count() >= 2);
            if (canMudragon && Bot.ExtraDeck.Any(c => c.IsCode(CardId.MudragonOfTheSwamp))) return true;

            // Check The Dragon that Devours the Dogma (Albaz + 1 LIGHT/DARK + 1 Effect)
            bool hasAlbaz = allMonsters.Any(c => c.IsCode(CardId.FallenOfAlbaz, CardId.FallenOfTheWhiteDragon));
            bool hasLightDark = allMonsters.Any(c => (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)) && !c.IsCode(CardId.FallenOfAlbaz, CardId.FallenOfTheWhiteDragon));
            if (hasAlbaz && hasLightDark && allMonsters.Count >= 3 && Bot.ExtraDeck.Any(c => c.IsCode(CardId.TheDragonThatDevoursTheDogma)))
                return true;

            // Check Mirrorjade (Albaz + 1 Fusion/Synchro/Xyz/Link)
            bool hasExtra = allMonsters.Any(c => (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) && !c.IsCode(CardId.FallenOfAlbaz, CardId.FallenOfTheWhiteDragon));
            if (hasAlbaz && hasExtra && Bot.ExtraDeck.Any(c => c.IsCode(CardId.MirrorjadeTheIcebladeDragon)))
                return true;

            return false;
        }

        private bool SuperPolymerizationEffect()
        {
            if (AreSpellsNegatedOrDisabled()) return false;
            if (IsSpecialSummonBlocked()) return false;

            int requiredHand = (Card != null && Card.Location == CardLocation.Hand) ? 2 : 1;
            if (Bot.Hand.Count < requiredHand) return false;

            if (!CanSuperPoly()) return false;
            if (Enemy.GetMonsterCount() == 0 && !CanDealLethal()) return false;

            AI.SelectCard(new[] {
                CardId.TheGoldenSwordsoul,
                CardId.BrandedRetribution,
                CardId.BrandedSword,
                CardId.TriBrigadeSpringansKitt,
                CardId.TriBrigadeMercourier,
                CardId.FallenOfAlbaz,
                CardId.FallenOfTheWhiteDragon
            });

            AI.SelectNextCard(new[] {
                CardId.GaruraWingsOfResonantLife,
                CardId.MudragonOfTheSwamp,
                CardId.TheDragonThatDevoursTheDogma,
                CardId.MirrorjadeTheIcebladeDragon,
                CardId.RindbrummTheStrikingDragon,
                CardId.AlbionTheBrandedDragon,
                CardId.LubellionTheSearingDragon
            });

            return true;
        }

        private bool ForbiddenDropletEffect()
        {
            if (AreSpellsNegatedOrDisabled()) return false;

            var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.HasType(CardType.Effect) && IsTargetable(c)).ToList();
            if (targets.Count == 0) return false;

            bool oppHasThreat = targets.Any(c => c.Attack >= 2500 || c.IsFloodgate() || c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Link));
            if (!oppHasThreat && !ShouldGoBreakBoard) return false;

            AI.SelectCard(new[] {
                CardId.TheGoldenSwordsoul,
                CardId.BrandedRetribution,
                CardId.BrandedSword,
                CardId.TriBrigadeSpringansKitt,
                CardId.TriBrigadeMercourier,
                CardId.FallenOfAlbaz,
                CardId.FallenOfTheWhiteDragon
            });

            AI.SelectNextCard(targets.OrderByDescending(c => c.Attack).ToList());
            return true;
        }

        private bool BrandedRetributionEffect()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                if (Duel.LastChainPlayer != 1) return false;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (AreSpellsNegatedOrDisabled()) return false;

                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.BrandedFusion, CardId.TheFallenAndTheVirtuous, CardId.BrandedInHighSpirits));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool BrandedSwordEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                int[] validRecycle = {
                    CardId.FallenOfAlbaz,
                    CardId.AluberTheJesterOfDespia,
                    CardId.TriBrigadeMercourier,
                    CardId.BlazingCartesiaTheVirtuous,
                    CardId.GuidingQuemTheVirtuous,
                    CardId.FallenOfTheWhiteDragon,
                    CardId.IncredibleEcclesiaTheVirtuous
                };
                ClientCard target = Bot.Banished.FirstOrDefault(c => c != null && c.IsMonster() && validRecycle.Contains(c.Id));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool SpellSetFiltered()
        {
            if (Card == null) return false;
            if (Card.IsCode(CardId.BrandedFusion) && _brandedFusionUsed) return false;

            if (Card.HasType(CardType.QuickPlay) || Card.HasType(CardType.Trap))
            {
                if (Duel.Turn == 1 || Duel.Phase == DuelPhase.Main2)
                {
                    return DefaultSpellSet();
                }
            }

            return DefaultSpellSet();
        }

        // ==========================================
        // Card Handlers — Boss Monsters
        // ==========================================

        private bool MirrorjadeTheIcebladeDragonEffect()
        {
            if (Card != null && (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed))
            {
                return Enemy.GetMonsterCount() > 0;
            }

            if (_mirrorjadeUsed) return false;

            // Mirrorjade banish does NOT target, so it bypasses targeting immunity
            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            if (oppMonsters.Count == 0) return false;

            ClientCard target = oppMonsters.FirstOrDefault(c => HighThreatChokepoints.Contains(c.Id) || c.IsFloodgate())
                ?? oppMonsters.FirstOrDefault(c => c.Attack >= 2500 || c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link))
                ?? oppMonsters.OrderByDescending(c => c.Attack).FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(new[] {
                    CardId.AlbionTheBrandedDragon,
                    CardId.TheDragonThatDevoursTheDogma,
                    CardId.RindbrummTheStrikingDragon,
                    CardId.TitanikladTheAshDragon
                });

                AI.SelectNextCard(target);
                _mirrorjadeUsed = true;
                return true;
            }

            return false;
        }

        private bool TheDragonThatDevoursTheDogmaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var oppTargets = Enemy.Graveyard.Concat(Enemy.Banished)
                    .Where(c => c != null && (c.IsMonster() || c.IsCode(CardId.EternalSoul, CardId.DarkMagicalCircle)))
                    .Take(2).ToList();

                if (oppTargets.Count > 0)
                {
                    AI.SelectCard(oppTargets);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(new[] {
                    CardId.TriBrigadeMercourier,
                    CardId.TheFallenAndTheVirtuous
                });
                return true;
            }
            return false;
        }

        private bool GranguignolTheDuskDragonEffect()
        {
            int opt = (int)ActivateDescription;
            if (Card.Location == CardLocation.MonsterZone && opt == 0)
            {
                AI.SelectCard(new[] {
                    CardId.DespianLuluwalilith,
                    CardId.TheDragonThatDevoursTheDogma,
                    CardId.AlbionTheShroudedDragon,
                    CardId.TitanikladTheAshDragon
                });
                return true;
            }
            else
            {
                if (Duel.Player == 1)
                {
                    AI.SelectCard(CardId.DespianLuluwalilith);
                    AI.SelectPosition(CardPosition.FaceUpAttack);
                    return true;
                }
            }
            return false;
        }

        private bool DespianLuluwalilithEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                    .FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && IsTargetable(c));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(new[] {
                    CardId.GuidingQuemTheVirtuous,
                    CardId.BlazingCartesiaTheVirtuous,
                    CardId.IncredibleEcclesiaTheVirtuous,
                    CardId.TheGoldenSwordsoul
                });
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool RindbrummTheStrikingDragonEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                    (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) && IsTargetable(c));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (Duel.Player == 1 && Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.FallenOfAlbaz) && c.IsCanRevive()))
                {
                    AI.SelectCard(CardId.FallenOfAlbaz);
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                    return true;
                }
            }
            return false;
        }

        private bool EcclesiaAndTheDarkDragonEffect()
        {
            int opt = (int)ActivateDescription;
            if (opt == Util.GetStringId(CardId.EcclesiaAndTheDarkDragon, 0) || Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.Player == 1 || (Duel.Player == 0 && ShouldGoBreakBoard))
                {
                    AI.SelectCard(new[] {
                        CardId.FallenOfAlbaz,
                        CardId.GuidingQuemTheVirtuous,
                        CardId.BlazingCartesiaTheVirtuous,
                        CardId.IncredibleEcclesiaTheVirtuous
                    });
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                var fusionTarget = Bot.Graveyard.Concat(Bot.Banished)
                    .FirstOrDefault(c => c != null && (c.IsCode(CardId.AlbionTheBrandedDragon, CardId.MirrorjadeTheIcebladeDragon, CardId.TheDragonThatDevoursTheDogma)));
                var fieldTarget = Enemy.GetMonsters().Concat(Enemy.GetSpells()).FirstOrDefault(c => c != null && IsTargetable(c));

                if (fusionTarget != null && fieldTarget != null)
                {
                    AI.SelectCard(fusionTarget);
                    AI.SelectNextCard(fieldTarget);
                    return true;
                }
            }
            return false;
        }

        private bool LubellionTheSearingDragonEffect()
        {
            if (_lubellionUsed) return false;
            if (Bot.Hand.Count == 0) return false;

            AI.SelectCard(new[] {
                CardId.TheGoldenSwordsoul,
                CardId.BrandedRetribution,
                CardId.BrandedSword,
                CardId.TriBrigadeSpringansKitt,
                CardId.TriBrigadeMercourier,
                CardId.FallenOfTheWhiteDragon,
                CardId.FallenOfAlbaz
            });

            AI.SelectNextCard(new[] {
                CardId.MirrorjadeTheIcebladeDragon,
                CardId.TheDragonThatDevoursTheDogma,
                CardId.RindbrummTheStrikingDragon,
                CardId.AlbionTheBrandedDragon
            });

            _lubellionUsed = true;
            return true;
        }

        private bool AlbionTheBrandedDragonEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(new[] {
                    CardId.MirrorjadeTheIcebladeDragon,
                    CardId.TheDragonThatDevoursTheDogma,
                    CardId.LubellionTheSearingDragon,
                    CardId.RindbrummTheStrikingDragon
                });
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(new[] {
                    CardId.BrandedFusion,
                    CardId.TheFallenAndTheVirtuous,
                    CardId.BrandedRetribution,
                    CardId.BrandedInHighSpirits
                });
                return true;
            }
            return false;
        }

        private bool TitanikladTheAshDragonEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(new[] {
                    CardId.GuidingQuemTheVirtuous,
                    CardId.FallenOfAlbaz
                });
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        // ==========================================
        // Card Handlers — Starters & Monsters
        // ==========================================

        private bool FallenOfTheWhiteDragonHandEffect()
        {
            if (_whiteDragonHandUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            AI.SelectCard(new[] {
                CardId.TheDragonThatDevoursTheDogma,
                CardId.AlbionTheBrandedDragon,
                CardId.TitanikladTheAshDragon,
                CardId.RindbrummTheStrikingDragon
            });

            _whiteDragonHandUsed = true;
            return true;
        }

        private bool FallenOfTheWhiteDragonFieldEffect()
        {
            if (_whiteDragonFieldUsed) return false;
            if (Card != null && Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.IncredibleEcclesiaTheVirtuous);
                AI.SelectPosition(CardPosition.FaceUpDefence);
                _whiteDragonFieldUsed = true;
                return true;
            }
            return false;
        }

        private bool BlazingCartesiaTheVirtuousSpSummon()
        {
            bool hasAlbaz = Bot.GetMonsters().Concat(Bot.Graveyard).Any(c => c != null && c.IsCode(CardId.FallenOfAlbaz, CardId.FallenOfTheWhiteDragon));
            return hasAlbaz && !IsSpecialSummonBlocked();
        }

        private bool BlazingCartesiaTheVirtuousEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_cartesiaUsed) return false;

                AI.SelectCard(new[] {
                    CardId.GranguignolTheDuskDragon,
                    CardId.TheDragonThatDevoursTheDogma,
                    CardId.MirrorjadeTheIcebladeDragon
                });

                _cartesiaUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool IncredibleEcclesiaTheVirtuousSpSummon()
        {
            AI.SelectPosition(CardPosition.FaceUpDefence);
            return true;
        }

        private bool IncredibleEcclesiaTheVirtuousEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // NEVER tribute self on Turn 1 going first when opponent has 0 monsters
                if (Duel.Turn == 1 && Duel.Player == 0) return false;
                if (Enemy.GetMonsterCount() == 0) return false;

                bool canSynchro = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner() && c.Level == 4);
                if (canSynchro && !_brandedFusionUsed)
                {
                    return false;
                }

                bool hasAlbazInDeck = Bot.Deck.Any(c => c.IsCode(CardId.FallenOfAlbaz)) || Bot.Hand.Any(c => c.IsCode(CardId.FallenOfAlbaz));
                if (hasAlbazInDeck)
                {
                    AI.SelectCard(CardId.FallenOfAlbaz);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                return Duel.Phase == DuelPhase.End;
            }
            return false;
        }

        private bool TriBrigadeSpringansKittEffect()
        {
            if (_springansKittUsed) return false;

            if (Card.Location == CardLocation.Grave)
            {
                int[] reviveTargets = {
                    CardId.MirrorjadeTheIcebladeDragon,
                    CardId.TheDragonThatDevoursTheDogma,
                    CardId.GranguignolTheDuskDragon,
                    CardId.AlbionTheBrandedDragon,
                    CardId.LubellionTheSearingDragon,
                    CardId.RindbrummTheStrikingDragon,
                    CardId.GuidingQuemTheVirtuous,
                    CardId.BlazingCartesiaTheVirtuous,
                    CardId.FallenOfAlbaz,
                    CardId.FallenOfTheWhiteDragon,
                    CardId.IncredibleEcclesiaTheVirtuous
                };

                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.IsCanRevive() && reviveTargets.Contains(c.Id));
                if (target != null)
                {
                    AI.SelectCard(reviveTargets);
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                    _springansKittUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool AluberTheJesterOfDespiaEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var oppTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && IsTargetable(c));
                if (oppTarget != null)
                {
                    AI.SelectCard(oppTarget);
                    return true;
                }
                return false;
            }

            AI.SelectCard(new[] {
                CardId.BrandedFusion,
                CardId.TheFallenAndTheVirtuous,
                CardId.BrandedInHighSpirits,
                CardId.BrandedRetribution,
                CardId.BrandedSword
            });
            return true;
        }

        private bool GuidingQuemTheVirtuousEffect()
        {
            int opt = (int)ActivateDescription;
            if (opt == Util.GetStringId(CardId.GuidingQuemTheVirtuous, 0) || Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(new[] {
                    CardId.AlbionTheShroudedDragon,
                    CardId.BlazingCartesiaTheVirtuous,
                    CardId.FallenOfTheWhiteDragon,
                    CardId.FallenOfAlbaz,
                    CardId.IncredibleEcclesiaTheVirtuous
                });
                return true;
            }
            else
            {
                AI.SelectCard(new[] {
                    CardId.BlazingCartesiaTheVirtuous,
                    CardId.FallenOfAlbaz,
                    CardId.FallenOfTheWhiteDragon,
                    CardId.IncredibleEcclesiaTheVirtuous,
                    CardId.TriBrigadeSpringansKitt
                });
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
        }

        private bool FallenOfAlbazEffect()
        {
            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsTargetable(c)).ToList();
            if (oppMonsters.Count == 0) return false;

            AI.SelectCard(new[] {
                CardId.TheGoldenSwordsoul,
                CardId.BrandedRetribution,
                CardId.BrandedSword,
                CardId.TriBrigadeSpringansKitt,
                CardId.TriBrigadeMercourier,
                CardId.FallenOfTheWhiteDragon
            });

            bool hasDragon = oppMonsters.Any(c => c.Race == (int)CardRace.Dragon);
            bool hasExtra = oppMonsters.Any(c => c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link));
            bool hasBeast = oppMonsters.Any(c => c.Race == (int)CardRace.Beast || c.Race == (int)CardRace.BestWarrior || c.Race == (int)CardRace.WindBeast);
            bool hasLight = oppMonsters.Any(c => c.HasAttribute(CardAttribute.Light));
            bool hasDark = oppMonsters.Any(c => c.HasAttribute(CardAttribute.Dark));

            List<int> fusionPriority = new List<int>();
            if (hasExtra || hasBeast) fusionPriority.Add(CardId.RindbrummTheStrikingDragon);
            if (hasLight) fusionPriority.Add(CardId.AlbionTheBrandedDragon);
            if (hasDark) fusionPriority.Add(CardId.LubellionTheSearingDragon);
            if (hasDragon) fusionPriority.Add(CardId.AlbaLenatusTheAbyssDragon);
            fusionPriority.Add(CardId.MirrorjadeTheIcebladeDragon);
            fusionPriority.Add(CardId.TheDragonThatDevoursTheDogma);
            fusionPriority.Add(CardId.TitanikladTheAshDragon);

            AI.SelectNextCard(fusionPriority);
            return true;
        }

        private bool AlbionTheShroudedDragonEffect()
        {
            AI.SelectCard(new[] {
                CardId.BrandedRetribution,
                CardId.BrandedFusion,
                CardId.TheFallenAndTheVirtuous,
                CardId.FallenOfAlbaz
            });
            return true;
        }

        private bool BystialMagnamhutSpSummon()
        {
            var target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)))
                ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.TriBrigadeMercourier));

            if (target != null)
            {
                AI.SelectCard(target);
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool BystialMagnamhutEffect()
        {
            return true;
        }

        private bool TheGoldenSwordsoulEffect()
        {
            if (_goldSwordsoulUsed) return false;

            if (Card.Location == CardLocation.Hand)
            {
                _goldSwordsoulUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Removed)
            {
                AI.SelectCard(new[] {
                    CardId.GuidingQuemTheVirtuous,
                    CardId.BlazingCartesiaTheVirtuous,
                    CardId.IncredibleEcclesiaTheVirtuous
                });
                AI.SelectPosition(CardPosition.FaceUpDefence);
                _goldSwordsoulUsed = true;
                return true;
            }
            return false;
        }

        private bool TriBrigadeMercourierEffect()
        {
            ClientCard lastChain = Util.GetLastChainCard();
            if (Card.Location == CardLocation.Hand)
            {
                if (lastChain != null && lastChain.Controller == 1 && lastChain.IsMonster())
                {
                    bool controlAlbazFusion = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                        c.IsCode(
                            CardId.MirrorjadeTheIcebladeDragon,
                            CardId.TheDragonThatDevoursTheDogma,
                            CardId.AlbionTheBrandedDragon,
                            CardId.LubellionTheSearingDragon,
                            CardId.RindbrummTheStrikingDragon,
                            CardId.TitanikladTheAshDragon
                        ));
                    if (controlAlbazFusion) return true;
                }
                return false;
            }
            else if (Card.Location == CardLocation.Removed)
            {
                AI.SelectCard(new[] {
                    CardId.FallenOfTheWhiteDragon,
                    CardId.GuidingQuemTheVirtuous,
                    CardId.BlazingCartesiaTheVirtuous,
                    CardId.IncredibleEcclesiaTheVirtuous,
                    CardId.FallenOfAlbaz
                });
                return true;
            }
            return false;
        }

        // ==========================================
        // Normal Summon Priority
        // ==========================================

        private bool NormalSummonAluber()
        {
            if (ShouldSkipCombo()) return false;
            return NormalSummonPriorityCheck();
        }

        private bool NormalSummonQuem()
        {
            if (ShouldSkipCombo()) return false;
            return NormalSummonPriorityCheck();
        }

        private bool NormalSummonCartesia()
        {
            if (ShouldSkipCombo()) return false;
            return NormalSummonPriorityCheck();
        }

        private bool NormalSummonAlbaz()
        {
            bool oppHasMonsters = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && IsTargetable(c));
            if (!oppHasMonsters && Bot.Hand.Count > 1) return false;
            return NormalSummonPriorityCheck();
        }

        private bool NormalSummonPriorityCheck()
        {
            if (EnemyHasKnownNegate() && !CanDealLethal()) return false;
            return true;
        }

        // ==========================================
        // Synchro & Extra Deck Summon Checks
        // ==========================================

        private bool EcclesiaAndTheDarkDragonSynchroSummonCheck()
        {
            if (!SynchroSummonCheck()) return false;
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner() && c.Level == 4);
            bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner() && c.Level == 4);
            return hasTuner && hasNonTuner;
        }

        private bool PSYFramelordOmegaSynchroSummonCheck()
        {
            if (!SynchroSummonCheck()) return false;
            bool hasTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsTuner() && c.Level == 4);
            bool hasNonTuner = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsTuner() && c.Level == 4);
            return hasTuner && hasNonTuner;
        }

        // ==========================================
        // Reactive Handtraps & Board Breakers
        // ==========================================

        private bool MulcharmyEffect()
        {
            if (_handTrapsUsedThisTurn >= 2) return false;
            if (Duel.Player != 1 || Bot.GetFieldCount() > 0) return false;

            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain == null || lastChain.Controller != 1) return false;

            if (HighThreatChokepoints.Contains(lastChain.Id) || lastChain.HasType(CardType.Monster))
            {
                _handTrapsUsedThisTurn++;
                return true;
            }

            if (!SmartHandTrapChain(HighThreatChokepoints)) return false;

            _handTrapsUsedThisTurn++;
            return true;
        }

        private bool AshBlossomEffect()
        {
            if (_handTrapsUsedThisTurn >= 2) return false;
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain == null || lastChain.Controller != 1) return false;

            // Direct must-negate override for critical opponent chokepoints
            if (HighThreatChokepoints.Contains(lastChain.Id))
            {
                _handTrapsUsedThisTurn++;
                return true;
            }

            if (!SmartHandTrapChain(HighThreatChokepoints)) return false;

            _handTrapsUsedThisTurn++;
            return true;
        }

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;
            bool canUse = Duel.Player == 1;
            if (canUse) _handTrapsUsedThisTurn++;
            return canUse;
        }

        private bool GhostSisterEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1;
        }

        private bool CalledByTheGraveEffect()
        {
            if (AreSpellsNegatedOrDisabled()) return false;
            ClientCard lastChain = Util.GetLastChainCard();
            if (Duel.LastChainPlayer == 1 && lastChain != null && lastChain.IsMonster())
            {
                ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsCode(lastChain.Id));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (AreSpellsNegatedOrDisabled()) return false;
            if (Duel.Player != 0 || (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)) return false;
            if (_tttUsed) return false;

            bool canControl = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && IsTargetable(c));
            if (canControl && (CanDealLethal() || OpponentHasThreateningMonster()))
            {
                AI.SelectOption(1);
                AI.SelectCard(Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsTargetable(c)).OrderByDescending(c => c.Attack).ToList());
                _tttUsed = true;
                return true;
            }

            AI.SelectOption(0);
            _tttUsed = true;
            return true;
        }

        private bool LightningStormEffect()
        {
            if (AreSpellsNegatedOrDisabled()) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) || Bot.GetSpells().Any(c => c != null && c.IsFaceup()))
                return false;

            if (Enemy.GetSpellCount() >= 2)
            {
                AI.SelectOption(1);
                return true;
            }
            if (Enemy.GetMonsterCount() >= 2)
            {
                AI.SelectOption(0);
                return true;
            }
            if (Enemy.GetSpellCount() > 0)
            {
                AI.SelectOption(1);
                return true;
            }
            return false;
        }

        private bool RaigekiEffect()
        {
            if (AreSpellsNegatedOrDisabled()) return false;
            return Enemy.GetMonsterCount() >= 1;
        }

        private bool CosmicCycloneEffect()
        {
            if (AreSpellsNegatedOrDisabled()) return false;
            var target = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsCode(CardId.EternalSoul, CardId.DarkMagicalCircle, CardId.AltergeistProtocol, CardId.PersonalSpoofing, CardId.SkillDrain))
                ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && IsTargetable(c))
                ?? Enemy.GetSpells().FirstOrDefault(c => c != null && IsTargetable(c));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool DimensionalBarrierEffect()
        {
            if (Duel.Player == 1)
            {
                AI.SelectOption(0);
                return true;
            }
            return false;
        }

        private bool RedRebootEffect()
        {
            ClientCard lastChain = Util.GetLastChainCard();
            return lastChain != null && lastChain.Controller == 1 && lastChain.IsTrap();
        }

        private bool MudragonOfTheSwampEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectAttribute(CardAttribute.Dark);
                return true;
            }
            return false;
        }

        private bool PSYFramelordOmegaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
                {
                    return Enemy.Hand.Count > 0;
                }
                if (Duel.Phase == DuelPhase.Standby)
                {
                    var target = Bot.Banished.FirstOrDefault(c => c != null && c.IsCode(CardId.AlbionTheBrandedDragon, CardId.MirrorjadeTheIcebladeDragon, CardId.TriBrigadeMercourier));
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.BrandedFusion, CardId.MirrorjadeTheIcebladeDragon));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool MonsterRepos()
        {
            if (Card != null && IsAceCard(Card) && Card.IsDefense() && Duel.Phase == DuelPhase.Main1 && !CanDealLethal())
                return false;
            return DefaultMonsterRepos();
        }

        // ==========================================
        // Decision & Hint Interception (OnSelectCard)
        // ==========================================

        public override IList<ClientCard> OnSelectCard(
            IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Safety & Target Priority Guard (Destruction / Banish / Bounce / Faceup / Target)
            if (hint == 502 || hint == 503 || hint == 505 || hint == 551 || hint == 575)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count >= min)
                {
                    int[] topPriorityTargets = {
                        CardId.EternalSoul,
                        CardId.DarkMagicalCircle,
                        CardId.DarkMagicianDragonKnight,
                        CardId.AltergeistProtocol,
                        CardId.PersonalSpoofing,
                        CardId.AltergeistHexstia,
                        CardId.ABCDragonBuster,
                        CardId.SecretVillageOfSpellcasters,
                        CardId.SkillDrain,
                        CardId.ImperialOrder
                    };

                    var sortedEnemy = enemyCards.OrderBy(c => {
                        if (topPriorityTargets.Contains(c.Id)) return 0;
                        if (c.IsSpell() || c.IsTrap()) return 1;
                        if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Link) || c.HasType(CardType.Xyz)) return 2;
                        if (c.Attack >= 2500) return 3;
                        return 10;
                    }).ToList();

                    return sortedEnemy.Take(Math.Max(min, Math.Min(max, sortedEnemy.Count))).ToList();
                }
            }

            // HINTMSG_TOGRAVE / HINTMSG_DISCARD (501 / 504): Protect key starters from being discarded
            if (hint == 501 || hint == 504)
            {
                var candidates = cards.Where(c => !c.IsCode(CardId.BrandedFusion)).ToList();
                if (candidates.Count >= min)
                {
                    return candidates.OrderBy(c => {
                        if (c.IsCode(CardId.TheGoldenSwordsoul)) return 1;
                        if (c.IsCode(CardId.BrandedRetribution) || c.IsCode(CardId.BrandedSword)) return 2;
                        if (c.IsCode(CardId.TriBrigadeSpringansKitt)) return 3;
                        if (c.IsCode(CardId.TriBrigadeMercourier)) return 4;
                        if (IsAceCard(c)) return 100;
                        return 10;
                    }).Take(max).ToList();
                }
            }

            // HINTMSG_TODECK (507): When shuffling fusion materials for Lubellion
            if (hint == 507)
            {
                var albazAndLubellion = cards.Where(c => c != null && c.IsCode(CardId.FallenOfAlbaz, CardId.LubellionTheSearingDragon)).ToList();
                if (albazAndLubellion.Count >= min)
                {
                    return albazAndLubellion.Take(max).ToList();
                }
            }

            // HINTMSG_FMATERIAL (533): Fusion materials
            if (hint == 533)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                if (sorted.Count >= min)
                {
                    return sorted.Take(max).ToList();
                }
            }

            // HINTMSG_SPSUMMON (509): Prefer Deck over Hand if available
            if (hint == 509)
            {
                if (cards.Any(c => c.Location == CardLocation.Deck))
                {
                    var deckCards = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    if (deckCards.Count >= min)
                    {
                        return deckCards.Take(Math.Max(min, Math.Min(max, deckCards.Count))).ToList();
                    }
                }
            }

            if (Card != null)
            {
                if (Card.IsCode(CardId.BrandedFusion))
                {
                    var ordered = cards.OrderBy(c => {
                        if (c == null) return 999;
                        if (c.IsCode(CardId.FallenOfAlbaz)) return 0;
                        if (c.IsCode(CardId.TheGoldenSwordsoul)) return 1;
                        if (c.IsCode(CardId.TriBrigadeMercourier)) return 2;
                        if (c.IsCode(CardId.AlbionTheShroudedDragon)) return 3;
                        if (c.IsCode(CardId.BlazingCartesiaTheVirtuous)) return 4;
                        if (c.IsCode(CardId.GuidingQuemTheVirtuous)) return 5;
                        return 50;
                    }).ToList();

                    return ordered.Take(Math.Max(min, Math.Min(max, ordered.Count))).ToList();
                }

                if (Card.IsCode(CardId.FallenOfTheWhiteDragon))
                {
                    var costExtra = cards.FirstOrDefault(c => c.Location == CardLocation.Extra &&
                        c.IsCode(CardId.TheDragonThatDevoursTheDogma, CardId.AlbionTheBrandedDragon, CardId.TitanikladTheAshDragon, CardId.RindbrummTheStrikingDragon));
                    if (costExtra != null) return new List<ClientCard> { costExtra };
                }

                if (Card.IsCode(CardId.BystialMagnamhut))
                {
                    var preferredDragons = cards.Where(c => c != null &&
                        c.IsCode(CardId.FallenOfTheWhiteDragon, CardId.AlbionTheShroudedDragon, CardId.FallenOfAlbaz)).ToList();
                    if (preferredDragons.Count >= min)
                    {
                        return preferredDragons.Take(Math.Max(min, Math.Min(max, preferredDragons.Count))).ToList();
                    }
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 20;
                if (cardId == 0 && Card != null) cardId = Card.Id;
                long optIndex = options[i] & 0xfffff;

                if (cardId == CardId.TheFallenAndTheVirtuous)
                {
                    bool hasEnemyFaceup = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup() && IsTargetable(c));
                    bool hasExtraSend = Bot.ExtraDeck.Any(c => c != null && c.IsCode(
                        CardId.TheDragonThatDevoursTheDogma, CardId.AlbionTheBrandedDragon, CardId.TitanikladTheAshDragon, CardId.RindbrummTheStrikingDragon));

                    if (hasEnemyFaceup && hasExtraSend && optIndex == 0)
                    {
                        return i;
                    }

                    bool hasEcclesia = Bot.GetMonsters().Concat(Bot.Graveyard).Any(c => c != null && (EcclesiaIds.Contains(c.Id) || (c.Name != null && c.Name.Contains("Ecclesia"))));
                    bool hasReviveTarget = hasEcclesia && Bot.Graveyard.Concat(Enemy.Graveyard).Any(c => c != null && c.IsMonster() && c.IsCanRevive() && ValidReviveIds.Contains(c.Id));
                    if (hasReviveTarget && optIndex == 1)
                    {
                        return i;
                    }

                    if (hasEnemyFaceup && optIndex == 0)
                    {
                        return i;
                    }
                }
            }

            return base.OnSelectOption(options);
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Controller == 1) return 10;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.MulcharmyFuwalos) || c.IsCode(CardId.AshBlossom) || c.IsCode(CardId.DrollAndLockBird))
                return 800;
            if (c.IsCode(CardId.TheGoldenSwordsoul)) return 80;
            if (c.IsCode(CardId.TriBrigadeSpringansKitt)) return 90;
            if (c.IsCode(CardId.FallenOfAlbaz)) return 100;
            if (c.IsCode(CardId.TriBrigadeMercourier)) return 110;
            if (c.IsCode(CardId.AlbionTheShroudedDragon)) return 120;
            if (c.IsCode(CardId.FallenOfTheWhiteDragon)) return 130;
            if (c.IsCode(CardId.BlazingCartesiaTheVirtuous) || c.IsCode(CardId.GuidingQuemTheVirtuous)) return 140;
            if (c.IsCode(CardId.AluberTheJesterOfDespia)) return 150;
            return base.GetMaterialPriority(c);
        }
    }
}
