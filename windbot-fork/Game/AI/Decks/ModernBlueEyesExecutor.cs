// ============================================================
// CARD AUDIT โ€” 2026_BlueEyes
// | Card Name           | Type    | OPT? | Cost      | Effect              | Activate When        | NEVER When           |
// |---------------------|---------|------|-----------|---------------------|----------------------|----------------------|
// | BE White Dragon     | Monster | None | None      | Normal Beatstick    | Material / summon    | Tribute summon       |
// | Sage with Eyes      | Monster | HOPT | Discard   | Search / SS from deck| Normal summon / hand | No target in deck    |
// | Maiden of White     | Monster | HOPT | None      | SS self / SS BEWD   | Targeted / in GY     | SS blocked           |
// | Kaibaman the Legend | Monster | None | Tribute   | SS BEWD from hand   | Summon to trigger    | BEWD not in hand     |
// | BE Jet Dragon       | Monster | HOPT | None      | Protect board / SS  | Card destroyed       | SS blocked           |
// | True Light          | Trap    | HOPT | None      | SS BEWD / search S/T| Opponent turn disrupt| No BEWD in hand/GY   |
// ACE CARDS: Primary: Dragon Master Magia / Secondary: Blue-Eyes Spirit Dragon, Cosmic Blazar Dragon
// COMBO STARTERS: 1. Sage with Eyes of Blue 2. Mausoleum of White 3. Maiden of White
// CHOKEPOINTS: Ash Blossom on Sage search or Mausoleum send
// WIN CONDITION: Establish Blue-Eyes Spirit Dragon to tag out into Crimson Dragon โ’ Cosmic Blazar Dragon, or fuse into Dragon Master Magia.
// GOING 1ST END BOARD: Blue-Eyes Spirit Dragon + Cosmic Blazar Dragon
// GOING 2ND GAMEPLAN: Break board using Ultimate Fusion and Chaos MAX Dragon, push damage.
// ============================================================

// ============================================================
// COMBO DRAFT โ€” 2026_BlueEyes
// ============================================================
// === COMBO LINE 1: Sage + Maiden (Starter: Sage) ===
// HAND REQUIRED: Sage with Eyes of Blue + Maiden of White
// STEP 1: Normal Summon Sage with Eyes of Blue.
//   โ’ Effect: Search another Level 1 Tuner (Wishes for Eyes of Blue or Effect Veiler).
// STEP 2: Use Sage hand effect or target Maiden.
//   โ’ Maiden triggers: SS Blue-Eyes White Dragon.
// STEP 3: Synchro Sage (Level 1) + Blue-Eyes (Level 8) โ’ Blue-Eyes Spirit Dragon (Level 9).
// STEP 4: Tag out Spirit Dragon into Crimson Dragon (Level 9) or Blue-Eyes Ultimate Spirit (Level 12).
// STEP 5: Crimson Dragon targets Level 12 โ’ SS Cosmic Blazar Dragon.
// END BOARD: Cosmic Blazar Dragon (1 omni-negate)
// ============================================================
// === COMBO LINE 2: Alternative + Mausoleum (Starter: Mausoleum) ===
// HAND REQUIRED: Mausoleum of White + 1 lv8 Dragon (BEWD/Alternative)
// STEP 1: Activate Mausoleum โ’ send BEWD โ’ search Roar or Wishes.
// STEP 2: SS Alternative White Dragon by revealing BEWD in hand/GY.
// STEP 3: Normal Summon Sage (searched by Roar) + Alternative โ’ Spirit Dragon.
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_BlueEyes", "2026_BlueEyes")]
    public class _2026_BlueEyesExecutor : ModernExecutor
    {
        public class CardId
        {
            // Blue-Eyes Main Deck
            public const int BEWhiteDragon = 89631139;
            public const int SageWithEyesOfBlue = 8240199;
            public const int MaidenOfWhite = 17947697;
            public const int KaibamanTheLegend = 67768675;
            public const int DeepEyesWhiteDragon = 67886895;
            public const int BEAlternativeWhiteDragon = 30397786;
            public const int BEJetDragon = 30576089;
            public const int BEChaosMaxDragon = 55410871;

            // Spells & Traps
            public const int WishesForEyesOfBlue = 80326401;
            public const int RoarOfTheBEDragons = 17725109;
            public const int UltimateFusion = 71143015;
            public const int MausoleumOfWhite = 24382602;
            public const int TrueLight = 62089826;
            public const int MajestyOfTheWhiteDragons = 43219114;
            public const int MelodyOfAwakeningDragon = 48800175;
            public const int TradeIn = 38120068;
            public const int CardsOfConsonance = 39701395;

            // Primite package
            public const int PrimiteDragonEtherBeryl = 63198739;
            public const int PrimiteLordlyLode = 56506740;
            public const int PrimiteDrillbeam = 29095457;

            // Other staples
            public const int BystialMagnamhut = 33854624;
            public const int SynchroRumble = 88901994;
            public const int AshBlossom = 14558127;
            public const int MulcharmyFuwalos = 42141493;
            public const int EffectVeiler = 97268402;

            // Extra Deck
            public const int SpiritWithEyesOfBlue = 42097666;
            public const int HieraticSeal = 24361622;
            public const int BESpiritDragon = 59822133;
            public const int BEUltimateSpiritDragon = 89604813;
            public const int LightstormDragon = 10515412;
            public const int CrimsonDragon = 63436931;
            public const int CosmicBlazarDragon = 21123811;
            public const int StardustSifrDragon = 26268488;
            public const int AncientFairyLifeDragon = 43321985;
            public const int BETyrantDragon = 11443677;
            public const int BEUltimateDragon = 23995346;
            public const int DragonMasterMagia = 12381100;
            public const int IndigoEyesSilverDragon = 16699558;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ACE CARDS โ€” Cards we must protect at all costs
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private static readonly int[] AceCardIds = {
            CardId.DragonMasterMagia,
            CardId.CosmicBlazarDragon,
            CardId.StardustSifrDragon,
            CardId.BEUltimateSpiritDragon,
            CardId.BESpiritDragon,
            CardId.BETyrantDragon,
            CardId.BEJetDragon,
            CardId.BEChaosMaxDragon,
            CardId.BEAlternativeWhiteDragon,
            CardId.IndigoEyesSilverDragon
        };

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  EXTRA DECK EXTORTION โ€” Cards that mill/force discard from ED
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private static readonly int[] ExtraDeckExtortion = {
            87602890,   // Zaborg, The Mega Monarch
            95679145,   // Maximus Dragma
            82734805,   // Infernoid Tierra
            86062400,   // Xyz Avenger
            63737050,   // Ryu Okami
        };

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  MASS REMOVAL โ€” Board wipes that require Ace protection
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private static readonly int[] BoardWipes = {
            12580477,   // Raigeki
            53129443,   // Dark Hole
            14532163,   // Lightning Storm
            99330325,   // Interrupted Kaiju Slumber
            15693423,   // Evenly Matched
        };

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  KEY NORMAL DRAGONS โ€” Level 8 beaters that can be used for Synchro
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private static readonly int[] Level8Dragons = {
            CardId.BEWhiteDragon,
            CardId.BEAlternativeWhiteDragon,
        };

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  TUENRS โ€” Level 1 Tuners used for Synchro plays
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private static readonly int[] Level1Tuners = {
            CardId.SageWithEyesOfBlue,
            CardId.MaidenOfWhite,
            CardId.EffectVeiler,
        };

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  STATE TRACKING โ€” HOPT Flags & Combo Progression
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        
        // HOPT (Hard Once Per Turn) Flags
        private bool _fusionUsed = false;
        private bool _crimsonUsed = false;
        private bool _trueLightUsed = false;
        private bool _sageEffectUsed = false;     // Covers both field and hand effect
        private bool _mausoleumUsed = false;
        private bool _alternativeUsedEffect = false; // Alternative pop effect
        private bool _maidenEffectUsed = false;
        private bool _jetDragonEffectUsed = false;
        private bool _melodyUsed = false;
        private bool _tradeInUsed = false;
        private bool _roarUsed = false;
        private bool _etherBerylUsed = false;
        private bool _drillbeamUsed = false;

        // Combo Progression State
        private enum ComboPhase
        {
            Init,           // Start of turn
            Searching,      // Searching for key pieces
            Summoning,      // Normal summoning combo starters
            Synchroing,     // Synchro summoning (Spirit Dragon, etc.)
            TaggingOut,     // Spirit Dragon โ’ Crimson Dragon
            Finalizing,     // Crimson Dragon โ’ Cosmic Blazar
            BoardComplete,  // End board is set up
            GoingForOTK,    // Turn 2+ going for lethal
            GrindGame       // Late game resource war
        }
        private ComboPhase _currentPhase = ComboPhase.Init;
        
        // Normal Summon tracking (for Trade-In / Melody sequencing)
        private bool _normalSummonUsed = false;
        
        // Used-Alternative tracking (like the original BlueEyesExecutor)
        // Note: Brain?.OwnSummons is used for Nibiru awareness instead of a manual counter
        private List<ClientCard> _usedAlternativeOrLevel8 = new List<ClientCard>();

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  CONSTRUCTOR โ€” Executor Registrations
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        public _2026_BlueEyesExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);
            ResourcePlan.RegisterAceCards(AceCardIds);

            // ===== Combo Router: Combo Line Registration =====
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Sage-Maiden-Combo",
                RequiredCards = new List<int> { CardId.SageWithEyesOfBlue },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.SageWithEyesOfBlue, ActionType = ExecutorType.Summon, Description = "Normal Summon Sage" }
                },
                EndBoardScore = 90
            });
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Mausoleum-Start",
                RequiredCards = new List<int> { CardId.MausoleumOfWhite },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.MausoleumOfWhite, ActionType = ExecutorType.Activate, Description = "Activate Mausoleum send BEWD" }
                },
                EndBoardScore = 85
            });

            // ===== Bait Planner =====
            BaitPlanner.RegisterComboStarters(CardId.SageWithEyesOfBlue, CardId.MausoleumOfWhite, CardId.PrimiteLordlyLode);
            BaitPlanner.RegisterBaitCards(CardId.PrimiteDrillbeam);

            // ===== Chain Advisor =====
            ChainAdvisor.RegisterHighValueTargets(CardId.SageWithEyesOfBlue, CardId.PrimiteLordlyLode, CardId.WishesForEyesOfBlue, CardId.UltimateFusion);

            // ===== Priority 1: Hand Traps & Staples =====
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, FuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerEffect);

            // ===== Priority 2: Boss Quick Effects (Chainable Negates) =====
            AddExecutor(ExecutorType.Activate, CardId.CosmicBlazarDragon, BlazarEffect);
            AddExecutor(ExecutorType.Activate, CardId.StardustSifrDragon, SifrEffect);
            AddExecutor(ExecutorType.Activate, CardId.DragonMasterMagia, MagiaEffect);
            AddExecutor(ExecutorType.Activate, CardId.BEUltimateSpiritDragon, UltimateSpiritEffect);
            AddExecutor(ExecutorType.Activate, CardId.BESpiritDragon, SpiritDragonEffect);

            // ===== Priority 3: Draw Power & Search Spells (BEFORE summons for max info) =====
            AddExecutor(ExecutorType.Activate, CardId.MelodyOfAwakeningDragon, MelodyEffect);
            AddExecutor(ExecutorType.Activate, CardId.TradeIn, TradeInEffect);
            AddExecutor(ExecutorType.Activate, CardId.CardsOfConsonance, () => false); // Reserved: enable when tuners (White Stone) are added to the deck

            // ===== Priority 4: Field Setup Spells =====
            AddExecutor(ExecutorType.Activate, CardId.MausoleumOfWhite, MausoleumEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrimiteLordlyLode, LordlyLodeEffect);
            AddExecutor(ExecutorType.Activate, CardId.WishesForEyesOfBlue, WishesEffect);
            AddExecutor(ExecutorType.Activate, CardId.RoarOfTheBEDragons, RoarEffect);
            AddExecutor(ExecutorType.Activate, CardId.SynchroRumble, SynchroRumbleEffect);
            AddExecutor(ExecutorType.Activate, CardId.MajestyOfTheWhiteDragons, MajestyEffect);

            // ===== Priority 5: Bystial Magnamhut โ€” Free SS from hand =====
            AddExecutor(ExecutorType.SpSummon, CardId.BystialMagnamhut, BystialSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialEffect);

            // ===== Priority 6: Summons & Main Plays =====
            AddExecutor(ExecutorType.Summon, CardId.SageWithEyesOfBlue, SageSummon);
            AddExecutor(ExecutorType.Activate, CardId.SageWithEyesOfBlue, SageEffect);
            AddExecutor(ExecutorType.Summon, CardId.MaidenOfWhite, MaidenSummon);
            AddExecutor(ExecutorType.Activate, CardId.MaidenOfWhite, MaidenEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BEAlternativeWhiteDragon, AlternativeSummon);
            AddExecutor(ExecutorType.Activate, CardId.BEAlternativeWhiteDragon, AlternativeEffect);
            AddExecutor(ExecutorType.Summon, CardId.KaibamanTheLegend, KaibamanSummon);
            AddExecutor(ExecutorType.Activate, CardId.KaibamanTheLegend, KaibamanEffect);

            // ===== Priority 7: Jet, Deep-Eyes & Primite =====
            AddExecutor(ExecutorType.Activate, CardId.BEJetDragon, JetDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.DeepEyesWhiteDragon, DeepEyesEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrimiteDragonEtherBeryl, EtherBerylEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrimiteDrillbeam, DrillbeamEffect);

            // ===== Priority 8: Extra Deck Summons =====
            AddExecutor(ExecutorType.SpSummon, CardId.BESpiritDragon, SpiritDragonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SpiritWithEyesOfBlue);
            AddExecutor(ExecutorType.SpSummon, CardId.CrimsonDragon, CrimsonDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.CrimsonDragon, CrimsonDragonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BEUltimateSpiritDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.HieraticSeal, HieraticSealSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.IndigoEyesSilverDragon);
            AddExecutor(ExecutorType.Activate, CardId.AncientFairyLifeDragon, () => true);
            AddExecutor(ExecutorType.Activate, CardId.LightstormDragon, () => true);

            // ===== Priority 9: Fusion Plays =====
            AddExecutor(ExecutorType.Activate, CardId.UltimateFusion, UltimateFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.BETyrantDragon, () => true);

            // ===== Priority 10: Traps & Backrow =====
            AddExecutor(ExecutorType.Activate, CardId.TrueLight, TrueLightEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.TrueLight);
            AddExecutor(ExecutorType.SpellSet, SpellSetFiltered);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง NEW TURN โ€” Reset all state flags and evaluate game plan
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            
            // Reset HOPT flags
            _fusionUsed = false;
            _crimsonUsed = false;
            _trueLightUsed = false;
            _sageEffectUsed = false;
            _mausoleumUsed = false;
            _alternativeUsedEffect = false;
            _maidenEffectUsed = false;
            _jetDragonEffectUsed = false;
            _melodyUsed = false;
            _tradeInUsed = false;
            _roarUsed = false;
            _etherBerylUsed = false;
            _drillbeamUsed = false;

            // Reset combo state
            _normalSummonUsed = false;
            _currentPhase = ComboPhase.Init;
            _usedAlternativeOrLevel8.Clear();

            // Evaluate game plan based on turn number and board state
            bool goingSecondEmptyBoard = _isGoingSecond && Bot.GetMonsterCount() == 0;
            if (goingSecondEmptyBoard)
            {
                _currentPhase = ComboPhase.GoingForOTK;
                AI?.Log(LogLevel.Info, "[GAMEPLAN] Going second โ€” OTK mode activated");
            }
            else if (Duel.Turn >= 6 || (Bot.Hand.Count <= 1 && Bot.GetMonsterCount() <= 1))
            {
                _currentPhase = ComboPhase.GrindGame;
                AI?.Log(LogLevel.Info, "[GAMEPLAN] Grind game โ€” resource conservation mode");
            }
            else if (Duel.Turn >= 3 && Bot.GetMonsterCount() == 0 && Enemy.GetMonsterCount() >= 1)
            {
                // We lost our board โ€” recovery mode (prioritize SS and reborn)
                _currentPhase = ComboPhase.GrindGame;
                AI?.Log(LogLevel.Info, "[GAMEPLAN] Board wiped โ€” recovery mode");
            }

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        public override bool OnSelectHand()
        {
            return true; // Go first to set up Spirit Dragon + negation board
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง ACECARD / MATERIAL PRIORITY โ€” Protect key cards
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            
            // Highest: Ace cards already on field (never use as material!)
            if (IsAceCard(c))
            {
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup())
                    return int.MaxValue;
                return 900;
            }
            
            // Combo pieces (Sage, Maiden) โ€” keep for combos
            if (c.Id == CardId.SageWithEyesOfBlue || c.Id == CardId.MaidenOfWhite)
                return 850;
            
            // Hand traps โ€” protect
            if (c.IsCode(CardId.AshBlossom, CardId.EffectVeiler, CardId.MulcharmyFuwalos))
                return 800;
            
            // Tuners are valuable for Synchro plays
            if (c.IsTuner()) return 700;
            
            // High level dragons (BEWD, Alternative) already summoned โ€” can use as material
            if (c.IsCode(Level8Dragons)) return 500;
            
            return 100;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง ON SELECT CARD โ€” Ace Card Protection + Smart Selection
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // โ”€โ”€ CASE 1: Extra Deck Mill Protection (Zaborg, Maximus Dragma) โ”€โ”€
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().IsCode(ExtraDeckExtortion))
            {
                // When opponent makes us send Extra Deck cards to GY,
                // send the least valuable ones first (reverse priority)
                return SelectExtraDeckToMill(cards, min, max);
            }

            // โ”€โ”€ CASE 2: Evenly Matched Protection โ”€โ”€
            if (Util.GetLastChainCard() != null && Util.GetLastChainCard().IsCode(15693423) && Duel.LastChainPlayer != 0)
            {
                // Keep ace cards, send weaker ones
                var keepIds = new HashSet<int> { 
                    CardId.DragonMasterMagia,
                    CardId.CosmicBlazarDragon,
                    CardId.StardustSifrDragon,
                    CardId.BESpiritDragon,
                    CardId.BEChaosMaxDragon
                };
                var result = cards.Where(c => c != null && !keepIds.Contains(c.Id)).Take(max).ToList();
                if (result.Count >= min)
                    return Util.CheckSelectCount(result, cards, min, max);
            }

            // โ”€โ”€ CASE 3: Melody of Awakening Dragon (detect by max==2 and Deck location) โ”€โ”€
            if (max == 2 && cards.Count >= 1 && cards[0].Location == CardLocation.Deck)
            {
                return SelectMelodyTargets(cards, min, max);
            }

            // โ”€โ”€ CASE 4: Search from Deck (hint == 509 for SS/Place, 506 for Add to hand) โ”€โ”€
            if (hint == 509)
            {
                var deckCards = cards.Where(c => c != null && c.Location == CardLocation.Deck).ToList();
                if (deckCards.Count > 0 && cards.Any(c => c.Location != CardLocation.Deck))
                    return Util.CheckSelectCount(deckCards, cards, min, max);
                // If all from same location, prefer combo pieces
                var sorted = cards.Where(c => c != null)
                    .OrderByDescending(c => GetMaterialPriority(c))
                    .ToList();
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            // โ”€โ”€ CASE 5: Material selection (Fusion/Synchro/Xyz/Link) โ”€โ”€
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533)
            {
                // Use already-used Alternative/Level8 as material first
                var preferredUsed = cards.Where(c => c != null && _usedAlternativeOrLevel8.Contains(c)).ToList();
                var sorted = cards.Where(c => c != null && !_usedAlternativeOrLevel8.Contains(c))
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();
                
                var result = new List<ClientCard>();
                foreach (var c in preferredUsed) { result.Add(c); if (result.Count >= max) break; }
                foreach (var c in sorted) { result.Add(c); if (result.Count >= max) break; }
                
                return Util.CheckSelectCount(result, cards, min, max);
            }

            // โ”€โ”€ CASE 6: Discard / Removal effects โ”€โ”€
            if (hint == 501 || hint == 502 || hint == 503 || hint == 504 || hint == 505 || hint == 508)
            {
                // Discard lowest priority first
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        /// <summary>
        /// Select Extra Deck cards to mill โ€” send least valuable first.
        /// </summary>
        private IList<ClientCard> SelectExtraDeckToMill(IList<ClientCard> cards, int min, int max)
        {
            var priority = new List<int> {
                CardId.IndigoEyesSilverDragon,       // Lowest value
                CardId.LightstormDragon,
                CardId.AncientFairyLifeDragon,
                CardId.BEUltimateDragon,
                CardId.SpiritWithEyesOfBlue,
                CardId.BETyrantDragon,               // Can be re-fused if needed
                CardId.BEUltimateSpiritDragon,
                CardId.BESpiritDragon,
                CardId.HieraticSeal,
                CardId.CrimsonDragon,
                CardId.CosmicBlazarDragon,            // High value negate
                CardId.StardustSifrDragon,
                CardId.DragonMasterMagia               // Highest value
            };
            
            var result = new List<ClientCard>();
            foreach (int id in priority)
            {
                var match = cards.FirstOrDefault(c => c != null && c.Id == id && !result.Contains(c));
                if (match != null)
                {
                    result.Add(match);
                    if (result.Count >= max) break;
                }
            }
            // Fill remaining if needed
            foreach (var c in cards)
            {
                if (c != null && !result.Contains(c))
                {
                    result.Add(c);
                    if (result.Count >= max) break;
                }
            }
            return Util.CheckSelectCount(result, cards, min, max);
        }

        /// <summary>
        /// Select Melody of Awakening Dragon targets โ€” prefer BEWD + Alternative.
        /// </summary>
        private IList<ClientCard> SelectMelodyTargets(IList<ClientCard> cards, int min, int max)
        {
            var result = new List<ClientCard>();
            // Always grab Alternative if we don't have it
            if (!Bot.HasInHand(CardId.BEAlternativeWhiteDragon))
            {
                var alt = cards.FirstOrDefault(c => c != null && c.Id == CardId.BEAlternativeWhiteDragon);
                if (alt != null) result.Add(alt);
            }
            // Grab BEWD if we don't have one
            if (!Bot.HasInHand(CardId.BEWhiteDragon) && result.Count < max)
            {
                var bewd = cards.FirstOrDefault(c => c != null && c.Id == CardId.BEWhiteDragon && !result.Contains(c));
                if (bewd != null) result.Add(bewd);
            }
            // Fill remaining with Alternative or BEWD
            foreach (var c in cards)
            {
                if (c != null && !result.Contains(c) && c.IsCode(CardId.BEAlternativeWhiteDragon, CardId.BEWhiteDragon))
                {
                    result.Add(c);
                    if (result.Count >= max) break;
                }
            }
            return Util.CheckSelectCount(result, cards, min, max);
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            // Use already-used Level 8s first (like original BlueEyesExecutor)
            var sorted = cards.Where(c => c != null)
                .OrderBy(c => _usedAlternativeOrLevel8.Contains(c) ? 0 : 1)
                .ThenBy(c => GetMaterialPriority(c))
                .ToList();
            return Util.CheckSelectCount(sorted, cards, min, max);
        }

        /// <summary>
        /// Synchro Material Selection โ€” prefer used-up Alternative/Level 8 as non-tuner material.
        /// This preserves fresh Alternative (with pop effect) and other valuable monsters.
        /// </summary>
        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            // If sum == 9 (Spirit Dragon), we need Level 1 Tuner + Level 8 non-Tuner
            // Prefer used Alternative as the Level 8 material
            foreach (ClientCard used in _usedAlternativeOrLevel8)
            {
                if (cards.Contains(used) && !used.IsTuner())
                {
                    _usedAlternativeOrLevel8.Remove(used);
                    AI?.Log(LogLevel.Info, $"[SYNCHRO-MAT] Using already-popped Level 8 as material");
                    return new[] { used };
                }
            }

            // Fallback: prefer BEWD over fresh Alternative (Alternative still has pop effect)
            var bewd = cards.FirstOrDefault(c => c != null && c.Id == CardId.BEWhiteDragon && !c.IsTuner());
            if (bewd != null)
                return new[] { bewd };

            return null; // Let default logic handle
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง BATTLE PREDICTION โ€” Simulate effect interactions
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            // Dragon Master Magia: ATK = combined ATK of materials used for its Fusion Summon
            // (In EDOPro, this is typically ~5000+ but we handle the high ATK assumption)
            if (attacker != null && attacker.Id == CardId.DragonMasterMagia)
            {
                attacker.RealPower = Math.Max(attacker.Attack, 5000);
            }
            if (defender != null && defender.Id == CardId.DragonMasterMagia)
            {
                defender.RealPower = Math.Max(defender.Attack, 5000);
            }

            // Blue-Eyes Chaos MAX Dragon: double piercing damage when attacking Defense position
            if (attacker != null && attacker.IsCode(CardId.BEChaosMaxDragon) && defender != null && defender.IsDefense())
            {
                attacker.RealPower = attacker.Attack * 2;
            }

            // BE Tyrant Dragon: can attack twice (approximate by doubling power vs empty board)
            if (attacker != null && attacker.Id == CardId.BETyrantDragon && !attacker.IsDisabled()
                && defender != null && defender.IsMonster())
            {
                // Double damage approximation โ€” Tyrant can attack twice
                attacker.RealPower = attacker.Attack * 2;
            }

            return base.OnPreBattleBetween(attacker, defender);
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง COMBO STOP & END BOARD AWARENESS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        protected override bool IsBoardStrongEnough()
        {
            // Blue-Eyes specific board strength check:
            // 1. Negate monster on field (Cosmic Blazar, Stardust Sifr, Spirit Dragon)
            bool hasNegate = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.Id == CardId.CosmicBlazarDragon || c.Id == CardId.StardustSifrDragon ||
                 c.Id == CardId.DragonMasterMagia || c.Id == CardId.BESpiritDragon ||
                 c.Id == CardId.BEUltimateSpiritDragon));
            
            // 2. Spirit Dragon on field (tag-out potential)
            bool hasSpiritDragon = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                c.Id == CardId.BESpiritDragon);

            // 3. Face-down backrow (True Light etc.)
            int backrowCount = Bot.GetSpells().Count(c => c != null && c.IsFacedown());

            // 4. Hand traps available
            int handTraps = Bot.Hand.Count(c => c != null && 
                c.IsCode(CardId.AshBlossom, CardId.EffectVeiler, CardId.MulcharmyFuwalos));

            // Board is strong with either combo piece
            if (hasNegate && (hasSpiritDragon || backrowCount >= 1))
                return true;

            // Fall back to base logic
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            // If board is already strong, stop extending
            if (IsBoardStrongEnough() && _currentPhase == ComboPhase.Synchroing)
            {
                AI?.Log(LogLevel.Info, "[COMBO-STOP] โ“ Board sufficient (Spirit Dragon + negate) โ€” stop extending");
                return true;
            }

            // Nibiru protection: if we've summoned 5+ monsters this turn, stop
            int ownSummons = Brain?.OwnSummons ?? 0;
            if (ownSummons >= 5)
            {
                AI?.Log(LogLevel.Info, $"[NIBIRU-CHECK] โ  {ownSummons} summons โ€” stop to avoid Nibiru");
                _currentPhase = ComboPhase.BoardComplete;
                return true;
            }

            return base.ShouldStopExtending();
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง SHOULD ALLOW โ€” Smart Phase Flow Control
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool SpellSetFiltered()
        {
            return Duel.Phase == DuelPhase.Main2 || !Main.CanBattlePhase;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง HAND TRAPS & NEGATES
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// Smart Ash Blossom โ€” knows chokepoints for Blue-Eyes mirror and common decks.
        /// </summary>
        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain(
                // Blue-Eyes chokepoints
                CardId.SageWithEyesOfBlue,
                CardId.MausoleumOfWhite,
                CardId.MelodyOfAwakeningDragon,
                CardId.PrimiteLordlyLode,
                // Generic chokepoints
                32807846,   // Reinforcement of the Army
                73628505,   // Terraforming
                66399653,   // Union Hangar
                04367828,   // Branded Fusion
                57959858,   // Nadir Servant
                44599277,   // Fossil Dig
                75452921,   // Small World
                73853976    // Pot of Extravagance
            )) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool FuwalosEffect()
        {
            return Duel.Player == 1;
        }

        private bool EffectVeilerEffect()
        {
            if (Duel.Player != 1) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            
            // Target the most threatening monster
            var target = Enemy.MonsterZone
                .Where(c => c != null && c.IsFaceup() && c.IsMonster() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง BOSS QUICK EFFECTS โ€” Chainable Negates
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        /// <summary>
        /// Cosmic Blazar Dragon โ€” omni-negate with threat assessment.
        /// Only negate meaningful opponent activations, not trivial ones.
        /// </summary>
        private bool BlazarEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain == null || lastChain.Controller != 1) return false;

            // Always negate board wipes
            if (lastChain.IsCode(12580477, 53129443, 14532163, 99330325, 15693423)) return true;
            // Always negate removal targeting our monsters
            if (Util.ChainContainsCard(BoardWipes)) return true;
            // Negate S/T activations (likely impactful)
            if (lastChain.IsSpell() || lastChain.IsTrap()) return true;
            // Negate high-ATK monster effects (likely boss effects)
            if (lastChain.IsMonster() && lastChain.Attack >= 2000) return true;
            // Negate if our board has Ace cards worth protecting
            if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && IsAceCard(c)))
                return true;

            return false;
        }

        /// <summary>
        /// Stardust Sifr Dragon โ€” destruction negate with threat assessment.
        /// </summary>
        private bool SifrEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain == null || lastChain.Controller != 1) return false;

            // Always negate board wipes and removal
            if (lastChain.IsCode(12580477, 53129443, 14532163, 18144506)) return true;
            if (Util.ChainContainsCard(BoardWipes)) return true;
            // Negate S/T and high-impact monster effects
            if (lastChain.IsSpell() || lastChain.IsTrap()) return true;
            if (lastChain.IsMonster() && lastChain.Attack >= 2000) return true;
            if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && IsAceCard(c)))
                return true;

            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง DRAW POWER & SEARCH SPELLS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// Melody of Awakening Dragon โ€” search BEWD + Alternative.
        /// Fixed: GY/field presence should NOT block search. We need cards IN HAND
        /// for Alternative reveal and Synchro combos.
        /// </summary>
        private bool MelodyEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_melodyUsed) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.MelodyOfAwakeningDragon)) return false;
            
            // Only skip if we already have BOTH in hand (not GY, not field โ€” HAND)
            bool needBEWDInHand = !Bot.HasInHand(CardId.BEWhiteDragon);
            bool needAltInHand = !Bot.HasInHand(CardId.BEAlternativeWhiteDragon);
            
            // Also search if we need BEWD for Alternative reveal even if Alt is in hand
            bool altNeedsRevealTarget = Bot.HasInHand(CardId.BEAlternativeWhiteDragon) && !Bot.HasInHand(CardId.BEWhiteDragon);
            
            if (!needBEWDInHand && !needAltInHand && !altNeedsRevealTarget) return false;
            
            // Check we have searchable targets left in deck
            bool canSearchBEWD = Bot.GetRemainingCount(CardId.BEWhiteDragon, 3) > 0;
            bool canSearchAlt = Bot.GetRemainingCount(CardId.BEAlternativeWhiteDragon, 1) > 0;
            if (!canSearchBEWD && !canSearchAlt) return false;

            // Cost: discard 1 card โ€” pick the lowest priority card
            var discardTargets = Bot.Hand
                .Where(c => c != null && c.Id != CardId.MelodyOfAwakeningDragon
                    && !c.IsCode(CardId.SageWithEyesOfBlue, CardId.MaidenOfWhite, CardId.MausoleumOfWhite))
                .OrderBy(c => GetMaterialPriority(c))
                .ToList();
            if (discardTargets.Count == 0) return false;

            AI.SelectCard(discardTargets[0]);
            _melodyUsed = true;
            AI?.Log(LogLevel.Info, $"[MELODY] Searching โ€” needBEWD={needBEWDInHand}, needAlt={needAltInHand}");
            return true;
        }

        /// <summary>
        /// Trade-In โ€” discard Level 8 Dragon to draw 2.
        /// Relaxed: Allow discarding last BEWD if we have one in GY or can search another.
        /// BEWD in GY is actually GOOD (enables Fusion, True Light, Wishes targets).
        /// </summary>
        private bool TradeInEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_tradeInUsed) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.TradeIn)) return false;

            var tradeInFodder = Bot.Hand.Where(c => c != null && c.IsCode(Level8Dragons)).ToList();
            if (tradeInFodder.Count == 0) return false;

            // Priority: discard BEWD over Alternative (BEWD in GY enables Fusion/True Light)
            ClientCard toDiscard = null;
            
            // If we have 2+ BEWD, safely discard one
            int bewdInHand = Bot.Hand.Count(c => c != null && c.Id == CardId.BEWhiteDragon);
            if (bewdInHand >= 2)
                toDiscard = tradeInFodder.FirstOrDefault(c => c.Id == CardId.BEWhiteDragon);
            
            // If we have both BEWD and Alternative, discard BEWD (keep Alternative for SS + pop)
            if (toDiscard == null && Bot.HasInHand(CardId.BEWhiteDragon) && Bot.HasInHand(CardId.BEAlternativeWhiteDragon))
                toDiscard = tradeInFodder.FirstOrDefault(c => c.Id == CardId.BEWhiteDragon);
            
            // Last resort: discard whatever we have โ€” BEWD in GY is still useful
            if (toDiscard == null)
            {
                // Only block if we have NO Lv8 on field AND NO Lv8 in GY AND no way to search more
                bool hasLevel8Anywhere = Bot.Graveyard.Any(c => c.IsCode(Level8Dragons))
                    || Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(Level8Dragons));
                bool canSearchMore = Bot.GetRemainingCount(CardId.BEWhiteDragon, 3) > 0
                    || Bot.HasInHand(CardId.MelodyOfAwakeningDragon)
                    || Bot.HasInHand(CardId.RoarOfTheBEDragons);
                    
                if (!hasLevel8Anywhere && !canSearchMore && tradeInFodder.Count <= 1)
                    return false;
                
                toDiscard = tradeInFodder[0];
            }

            AI.SelectCard(toDiscard);
            _tradeInUsed = true;
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง SPELLS & FIELD SETUP
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// Mausoleum of White โ€” send BEWD to search Roar or Wishes.
        /// </summary>
        private bool MausoleumEffect()
        {
            if (Bot.HasInSpellZone(CardId.MausoleumOfWhite) || _mausoleumUsed) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.MausoleumOfWhite)) return false;
            
            // Need a Level 8 Dragon in hand/deck to send
            bool hasBEWDInDeck = Bot.GetRemainingCount(CardId.BEWhiteDragon, 3) > 0;
            bool hasBEWDInHand = Bot.HasInHand(CardId.BEWhiteDragon);
            
            if (!hasBEWDInDeck && !hasBEWDInHand) return false;

            _mausoleumUsed = true;
            _currentPhase = ComboPhase.Searching;

            // Search spell: prefer Roar if we need Sage, else Wishes
            if (!Bot.HasInHand(CardId.SageWithEyesOfBlue) && Bot.GetRemainingCount(CardId.SageWithEyesOfBlue, 3) > 0)
                AI.SelectCard(CardId.RoarOfTheBEDragons); // Roar searches Sage
            else
                AI.SelectCard(CardId.WishesForEyesOfBlue); // Wishes revives tuner

            return true;
        }

        private bool LordlyLodeEffect()
        {
            if (Bot.HasInSpellZone(CardId.PrimiteLordlyLode)) return false;
            return true;
        }

        /// <summary>
        /// Wishes for Eyes of Blue โ€” revive a Level 1 Tuner from GY.
        /// </summary>
        private bool WishesEffect()
        {
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.WishesForEyesOfBlue)) return false;
            
            bool hasTarget = Bot.Graveyard.Any(c => c.IsCode(Level1Tuners) && c.IsCanRevive());
            return hasTarget;
        }

        /// <summary>
        /// Roar of the Blue-Eyed Dragons โ€” search Sage or a normal monster.
        /// </summary>
        private bool RoarEffect()
        {
            if (_roarUsed) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.RoarOfTheBEDragons)) return false;
            
            // Search Sage first
            if (!Bot.HasInHand(CardId.SageWithEyesOfBlue) && Bot.GetRemainingCount(CardId.SageWithEyesOfBlue, 3) > 0)
                AI.SelectCard(CardId.SageWithEyesOfBlue);
            else if (!Bot.HasInHand(CardId.BEWhiteDragon) && Bot.GetRemainingCount(CardId.BEWhiteDragon, 3) > 0)
                AI.SelectCard(CardId.BEWhiteDragon);
            else if (Bot.GetRemainingCount(CardId.BEAlternativeWhiteDragon, 3) > 0)
                AI.SelectCard(CardId.BEAlternativeWhiteDragon);
            else
                return false; // Nothing useful to search
            
            _roarUsed = true;
            return true;
        }

        private bool SynchroRumbleEffect()
        {
            return Bot.Graveyard.Any(c => c.IsMonster() && c.Level == 1 && c.IsTuner() && c.IsCanRevive());
        }

        private bool MajestyEffect()
        {
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.BEWhiteDragon);
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง MONSTER SUMMONS & EFFECTS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// Sage with Eyes of Blue โ€” Normal Summon with combo prioritization.
        /// </summary>
        private bool SageSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_normalSummonUsed) return false;
            
            // Priority 1: If we have Maiden in hand, summon Sage to trigger her
            if (Bot.HasInHand(CardId.MaidenOfWhite))
            {
                _normalSummonUsed = true;
                _currentPhase = ComboPhase.Summoning;
                return true;
            }

            // Priority 2: If we have Mausoleum active, summon Sage to search
            if (Bot.HasInSpellZone(CardId.MausoleumOfWhite))
            {
                _normalSummonUsed = true;
                _currentPhase = ComboPhase.Summoning;
                return true;
            }

            // Priority 3: If we need a tuner for Synchro
            if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(Level8Dragons)))
            {
                _normalSummonUsed = true;
                _currentPhase = ComboPhase.Summoning;
                return true;
            }

            // Otherwise, only summon if we can follow up (have a Level 8 in hand/field)
            if (Bot.HasInHand(CardId.BEWhiteDragon) || Bot.HasInHand(CardId.BEAlternativeWhiteDragon)
                || Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(Level8Dragons)))
            {
                _normalSummonUsed = true;
                _currentPhase = ComboPhase.Summoning;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sage effect handler โ€” both field search and hand SS.
        /// Uses ActivateDescription for multi-effect discrimination.
        /// </summary>
        private bool SageEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_sageEffectUsed) return false;

            if (Card.Location == CardLocation.MonsterZone)
            {
                // โ”€โ”€ Field Effect: Search a Level 1 Tuner โ”€โ”€
                _sageEffectUsed = true;
                
                // Priority: Maiden > Effect Veiler > other tuners
                if (!Bot.HasInHand(CardId.MaidenOfWhite) 
                    && !Bot.MonsterZone.Any(c => c != null && c.Id == CardId.MaidenOfWhite)
                    && Bot.GetRemainingCount(CardId.MaidenOfWhite, 3) > 0)
                {
                    // If we're going first and have no Maiden, search Maiden for combo extension
                    if (Duel.Turn == 1 || Bot.HasInHand(CardId.BEWhiteDragon) || Bot.HasInHand(CardId.BEAlternativeWhiteDragon))
                        AI.SelectCard(CardId.MaidenOfWhite);
                    else
                        AI.SelectCard(CardId.EffectVeiler, CardId.MaidenOfWhite);
                }
                else
                {
                    // Already have Maiden, get Veiler for disruption or another tuner
                    AI.SelectCard(CardId.EffectVeiler, CardId.MaidenOfWhite);
                }
                
                AI?.Log(LogLevel.Info, $"[SAGE] Searching Level 1 Tuner โ€” Maiden={!Bot.HasInHand(CardId.MaidenOfWhite)}");
                return true;
            }
            else if (Card.Location == CardLocation.Hand)
            {
                // โ”€โ”€ Hand Effect: Discard Sage + target own Effect monster โ’ SS BEWD from Deck โ”€โ”€
                // KEY COMBO: If Maiden is on field, target Maiden โ’ she triggers her own effect
                // โ’ SS BEWD from deck (Maiden's trigger) โ’ we ALSO get BEWD from Sage
                // This is a 2-for-1 BEWD generation play
                
                bool canSSBEWD = Bot.GetRemainingCount(CardId.BEWhiteDragon, 3) > 0
                    || Bot.Graveyard.Any(c => c.Id == CardId.BEWhiteDragon);
                if (!canSSBEWD) return false;

                // Priority 1: Target Maiden (triggers her SS BEWD effect โ€” best play)
                ClientCard maiden = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup()
                    && c.Id == CardId.MaidenOfWhite && !c.IsDisabled());
                if (maiden != null)
                {
                    _sageEffectUsed = true;
                    AI.SelectCard(maiden);
                    AI.SelectNextCard(CardId.BEWhiteDragon);
                    AI?.Log(LogLevel.Info, "[SAGE-HAND] Targeting Maiden โ’ triggers her SS BEWD + Sage SS BEWD");
                    return true;
                }

                // Priority 2: Target any non-critical Effect monster
                ClientCard target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup()
                    && !c.IsCode(Level8Dragons) && !c.Id.Equals(CardId.SageWithEyesOfBlue)
                    && !IsAceCard(c));
                
                if (target != null)
                {
                    _sageEffectUsed = true;
                    AI.SelectCard(target);
                    AI.SelectNextCard(CardId.BEWhiteDragon);
                    AI?.Log(LogLevel.Info, $"[SAGE-HAND] Targeting {target.Name} โ’ SS BEWD");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Maiden of White โ€” Normal Summon.
        /// </summary>
        private bool MaidenSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_normalSummonUsed) return false;
            
            // Only summon Maiden if we have Sage in hand to target her
            // or if we have Mausoleum to trigger her
            if (!Bot.HasInHand(CardId.SageWithEyesOfBlue) && !Bot.HasInSpellZone(CardId.MausoleumOfWhite))
                return false;

            _normalSummonUsed = true;
            _currentPhase = ComboPhase.Summoning;
            return true;
        }

        /// <summary>
        /// Maiden of White โ€” effect: SS BEWD when targeted.
        /// </summary>
        private bool MaidenEffect()
        {
            if (_maidenEffectUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            
            // Can only activate when targeted or in GY
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Triggered when targeted by effect
                if (!Util.IsChainTarget(Card)) return false;
            }
            
            _maidenEffectUsed = true;
            AI.SelectCard(CardId.BEWhiteDragon);
            return true;
        }

        /// <summary>
        /// Alternative White Dragon โ€” Special Summon by revealing BEWD in hand.
        /// Fixed: OCG text requires revealing BEWD in HAND only. GY/field does NOT count.
        /// </summary>
        private bool AlternativeSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            
            // OCG: "You can reveal 1 'Blue-Eyes White Dragon' in your hand"
            // ONLY hand counts for the reveal condition
            if (!Bot.HasInHand(CardId.BEWhiteDragon)) return false;
            
            return true;
        }

        /// <summary>
        /// Alternative White Dragon โ€” effect: destroy target monster.
        /// </summary>
        private bool AlternativeEffect()
        {
            if (_alternativeUsedEffect) return false;

            ClientCard target = Enemy.MonsterZone
                .Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            
            if (target != null)
            {
                _alternativeUsedEffect = true;
                // Track that this Alternative has used its effect
                // (so it can be used as Synchro material first)
                if (Card != null && !_usedAlternativeOrLevel8.Contains(Card))
                    _usedAlternativeOrLevel8.Add(Card);
                
                AI.SelectCard(target);
                return true;
            }

            // If no valid target, still usable as beater โ€” just don't activate
            return false;
        }

        private bool KaibamanSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_normalSummonUsed) return false;
            if (!Bot.HasInHand(CardId.BEWhiteDragon)) return false;
            
            _normalSummonUsed = true;
            return true;
        }

        private bool KaibamanEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (!Bot.HasInHand(CardId.BEWhiteDragon)) return false;
            
            AI.SelectCard(CardId.BEWhiteDragon);
            return true;
        }

        /// <summary>
        /// BE Jet Dragon โ€” protection & Special Summon.
        /// </summary>
        private bool JetDragonEffect()
        {
            if (_jetDragonEffectUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            // List of destruction effects that should trigger Jet Dragon's GY SS
            int[] destructionTriggers = {
                12580477,   // Raigeki
                53129443,   // Dark Hole
                14532163,   // Lightning Storm
                53582587,   // Torrential Tribute
                18144506,   // Harpie's Feather Duster
            };
            
            // Jet Dragon can SS from GY when a card is destroyed
            if (Card.Location == CardLocation.Grave)
            {
                // Trigger on any destruction effect, not just board wipes
                if (Util.ChainContainsCard(destructionTriggers) || Duel.LastChainPlayer == 1)
                {
                    _jetDragonEffectUsed = true;
                    return true;
                }
                return false;
            }
            
            // On field effect: protect from targeting
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Util.IsChainTarget(Card))
                    return true;
            }
            
            return false;
        }

        private bool DeepEyesEffect()
        {
            // Deep-Eyes activates when destroyed or when sent to GY
            if (Card.Location == CardLocation.Grave)
                return true;
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง PRIMITE ACTIONS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        private bool EtherBerylEffect()
        {
            if (_etherBerylUsed) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.PrimiteDragonEtherBeryl)) return false;
            _etherBerylUsed = true;
            return true;
        }

        private bool DrillbeamEffect()
        {
            if (_drillbeamUsed) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.PrimiteDrillbeam)) return false;

            ClientCard target = Enemy.MonsterZone
                .Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            
            if (target != null)
            {
                _drillbeamUsed = true;
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง BYSTIAL MAGNAMHUT โ€” Free SS from hand
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// Bystial Magnamhut โ€” SS from hand by banishing 1 LIGHT/DARK from either GY.
        /// Free 2500 ATK body. Excellent going second or in grind game.
        /// </summary>
        private bool BystialSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            // Check if either GY has a LIGHT or DARK monster to banish
            bool hasTargetInOurGY = Bot.Graveyard.Any(c => c != null && c.IsMonster()
                && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
            bool hasTargetInEnemyGY = Enemy.Graveyard.Any(c => c != null && c.IsMonster()
                && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));

            if (!hasTargetInOurGY && !hasTargetInEnemyGY) return false;

            // Prefer banishing from opponent's GY (disrupts their recursion)
            // Don't banish our own BEWD if we need it for Fusion/True Light
            if (hasTargetInEnemyGY)
            {
                AI?.Log(LogLevel.Info, "[BYSTIAL] SS by banishing from opponent GY");
                return true;
            }

            // Banish from our GY only if we have extra copies
            if (hasTargetInOurGY)
            {
                // Don't banish last BEWD from GY if it's needed for Fusion
                int bewdInGY = Bot.Graveyard.Count(c => c.Id == CardId.BEWhiteDragon);
                bool needBEWDforFusion = Bot.HasInHand(CardId.UltimateFusion) && bewdInGY <= 1;
                if (needBEWDforFusion)
                {
                    // Check if there are non-BEWD targets in our GY
                    bool hasOtherTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster()
                        && !c.IsCode(CardId.BEWhiteDragon)
                        && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
                    if (!hasOtherTarget) return false;
                }

                AI?.Log(LogLevel.Info, "[BYSTIAL] SS by banishing from own GY");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Bystial Magnamhut โ€” when SS'd, search a Dragon from deck.
        /// Searches BEWD (for Alternative reveal) or Alternative (for SS + pop).
        /// </summary>
        private bool BystialEffect()
        {
            // Search priority based on what we need in hand
            if (!Bot.HasInHand(CardId.BEWhiteDragon) && Bot.GetRemainingCount(CardId.BEWhiteDragon, 3) > 0)
            {
                AI.SelectCard(CardId.BEWhiteDragon);
                AI?.Log(LogLevel.Info, "[BYSTIAL] Searching BEWD (for Alternative reveal / Synchro material)");
                return true;
            }
            if (!Bot.HasInHand(CardId.BEAlternativeWhiteDragon) && Bot.GetRemainingCount(CardId.BEAlternativeWhiteDragon, 1) > 0)
            {
                AI.SelectCard(CardId.BEAlternativeWhiteDragon);
                AI?.Log(LogLevel.Info, "[BYSTIAL] Searching Alternative (for SS + pop effect)");
                return true;
            }
            // Fallback: search any useful Dragon
            AI.SelectCard(CardId.BEWhiteDragon, CardId.BEAlternativeWhiteDragon, CardId.BEJetDragon);
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง SYNCHRO & EXTRA DECK
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// Blue-Eyes Spirit Dragon โ€” Synchro Summon.
        /// </summary>
        private bool SpiritDragonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            
            // Need a Level 1 Tuner + Level 8 non-Tuner
            bool hasTuner = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Level == 1 && c.IsTuner());
            bool hasNonTuner = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(Level8Dragons));
            
            if (!hasTuner || !hasNonTuner) return false;
            
            if (Duel.Turn == 1)
                _currentPhase = ComboPhase.Synchroing;
            
            return true;
        }

        /// <summary>
        /// Blue-Eyes Spirit Dragon โ€” effect handler (GY negate + Tag out).
        /// Enhanced: Only tag-out on real threats, not every opponent action.
        /// Tag-out target selection improved for end-board optimization.
        /// </summary>
        private bool SpiritDragonEffect()
        {
            // โ”€โ”€ Effect 1: GY Negate (ActivateDescription == -1 or 0) โ”€โ”€
            if (ActivateDescription == -1 || ActivateDescription == Util.GetStringId(CardId.BESpiritDragon, 0))
            {
                // Only negate opponent's GY effects
                if (Duel.CurrentChain.Count > 0)
                {
                    ClientCard lastChain = Util.GetLastChainCard();
                    if (lastChain != null && lastChain.Location == CardLocation.Grave && lastChain.Controller == 1)
                        return true;
                }
                return false;
            }
            
            // โ”€โ”€ Effect 2: Tag Out โ”€โ”€
            // Spirit Dragon can tag out into a LIGHT Dragon Synchro (non-Synchro Summon).
            // Valid targets in our Extra: Azure-Eyes, Ultimate Spirit Dragon.
            
            bool shouldTagOut = false;
            string reason = "";
            
            // Condition A: Combo progression โ€” going first, combo phase wants tag-out
            if (_currentPhase == ComboPhase.Synchroing && Duel.Player == 0)
            {
                shouldTagOut = true;
                reason = "combo progression";
            }
            
            // Condition B: Being targeted by opponent
            if (Util.IsChainTarget(Card) && Duel.LastChainPlayer == 1)
            {
                shouldTagOut = true;
                reason = "dodge targeting";
            }
            
            // Condition C: Board wipe incoming
            if (Util.ChainContainsCard(BoardWipes))
            {
                shouldTagOut = true;
                reason = "dodge board wipe";
            }
            
            // Condition D: Opponent's Battle Phase (avoid being attacked while in DEF)
            if (Duel.Player == 1 && Duel.Phase == DuelPhase.BattleStart)
            {
                shouldTagOut = true;
                reason = "battle phase protection";
            }

            // Condition E: Opponent's End Phase (safe tag for protection next turn)
            if (Duel.Player == 1 && Duel.Phase == DuelPhase.End)
            {
                shouldTagOut = true;
                reason = "end phase safe tag";
            }

            // Condition F: Opponent activated a threatening effect
            if (Duel.Player == 1 && Duel.LastChainPlayer == 1)
            {
                ClientCard lastChain = Util.GetLastChainCard();
                if (lastChain != null && (lastChain.IsSpell() || lastChain.IsTrap()
                    || (lastChain.IsMonster() && lastChain.Attack >= 2500)))
                {
                    shouldTagOut = true;
                    reason = "dodge threatening effect";
                }
            }
            
            if (!shouldTagOut) return false;

            // Choose tag-out target:
            // Ultimate Spirit Dragon โ€” Lv12, has negate effects, largest body (3500 DEF)
            // This also enables Crimson Dragon to target it later for Cosmic Blazar
            int tagTarget;
            if (Bot.HasInExtra(CardId.BEUltimateSpiritDragon))
            {
                tagTarget = CardId.BEUltimateSpiritDragon;
            }
            else
            {
                // Fallback to any available Light Dragon Synchro
                tagTarget = CardId.BEUltimateSpiritDragon; // Default even if not in ED (OCGCore handles)
            }

            AI.SelectCard(tagTarget);
            _currentPhase = ComboPhase.TaggingOut;
            
            AI?.Log(LogLevel.Info, $"[SPIRIT-DRAGON] Tagging out โ’ Ultimate Spirit Dragon (reason: {reason})");
            return true;
        }

        /// <summary>
        /// Crimson Dragon โ€” Synchro Summon using Spirit Dragon.
        /// </summary>
        private bool CrimsonDragonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            
            // Crimson Dragon needs 1 Tuner + 1+ non-Tuner (Dragon type)
            bool hasTuner = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsTuner());
            bool hasLevel8Dragon = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Dragon) && !c.IsTuner());
            
            if (!hasTuner || !hasLevel8Dragon) return false;
            
            return true;
        }

        /// <summary>
        /// Crimson Dragon โ€” effect: SS a Level 9 or 12 Dragon from Extra Deck.
        /// </summary>
        private bool CrimsonDragonEffect()
        {
            if (_crimsonUsed) return false;

            // Find a Level 12 monster on field to target
            ClientCard target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Level == 12);
            if (target != null)
            {
                _crimsonUsed = true;
                AI.SelectCard(target);
                
                // Prefer Cosmic Blazar Dragon (omni-negate) over Stardust Sifr (protection)
                if (Bot.HasInExtra(CardId.CosmicBlazarDragon))
                    AI.SelectNextCard(CardId.CosmicBlazarDragon);
                else
                    AI.SelectNextCard(CardId.StardustSifrDragon);

                _currentPhase = ComboPhase.Finalizing;
                
                AI?.Log(LogLevel.Info, $"[CRIMSON] Targeting Lv12 โ’ Cosmic Blazar Dragon");
                return true;
            }

            return false;
        }

        /// <summary>
        /// BE Ultimate Spirit Dragon โ€” negate effect activation.
        /// </summary>
        private bool UltimateSpiritEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.Controller == 1)
                    return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง FUSION PLAYS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// Ultimate Fusion โ€” intelligent Fusion target selection.
        /// T1: Always Tyrant Dragon (puts Fusion in GY for future Magia).
        /// T2+: Magia if Fusion material in GY, else Tyrant for OTK.
        /// Going second: Tyrant for double attack OTK potential.
        /// </summary>
        private bool UltimateFusionEffect()
        {
            if (_fusionUsed) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.UltimateFusion)) return false;

            // Count BEWD material availability across all zones
            int bewdTotal = Bot.Hand.Count(c => c.Id == CardId.BEWhiteDragon)
                          + Bot.MonsterZone.Count(c => c != null && c.Id == CardId.BEWhiteDragon)
                          + Bot.Graveyard.Count(c => c.Id == CardId.BEWhiteDragon);
            int altTotal = Bot.Hand.Count(c => c.Id == CardId.BEAlternativeWhiteDragon)
                         + Bot.MonsterZone.Count(c => c != null && c.Id == CardId.BEAlternativeWhiteDragon)
                         + Bot.Graveyard.Count(c => c.Id == CardId.BEAlternativeWhiteDragon);
            
            if (bewdTotal + altTotal < 1) return false;

            _fusionUsed = true;

            // Check if Magia is possible (requires a Fusion monster as material)
            bool canMakeMagia = Bot.HasInExtra(CardId.DragonMasterMagia)
                && (Bot.Graveyard.Any(c => c.IsMonster() && c.HasType(CardType.Fusion))
                    || Bot.MonsterZone.Any(c => c != null && c.HasType(CardType.Fusion)));

            // โ”€โ”€ TURN 1: Always Tyrant Dragon โ”€โ”€
            // Tyrant is a 3000 ATK beater that also puts a Fusion monster into GY,
            // enabling Magia on the next turn. Magia is impossible T1 (no Fusion in GY).
            if (Duel.Turn == 1)
            {
                if (Bot.HasInExtra(CardId.BETyrantDragon))
                {
                    AI.SelectCard(CardId.BETyrantDragon);
                    AI?.Log(LogLevel.Info, "[FUSION] T1 โ’ Tyrant Dragon (seeds GY for future Magia)");
                    return true;
                }
                AI.SelectCard(CardId.BEUltimateDragon);
                return true;
            }

            // โ”€โ”€ GOING SECOND / OTK MODE โ”€โ”€
            if (_currentPhase == ComboPhase.GoingForOTK || _isGoingSecond)
            {
                // Tyrant's double attack is better for OTK than Magia's single attack
                if (Bot.HasInExtra(CardId.BETyrantDragon))
                {
                    AI.SelectCard(CardId.BETyrantDragon);
                    AI?.Log(LogLevel.Info, "[FUSION] OTK โ’ Tyrant Dragon (double attack)");
                    return true;
                }
            }

            // โ”€โ”€ TURN 2+: Magia if available (strongest boss) โ”€โ”€
            if (canMakeMagia)
            {
                AI.SelectCard(CardId.DragonMasterMagia);
                AI?.Log(LogLevel.Info, "[FUSION] T2+ โ’ Dragon Master Magia (omni-negate boss)");
                return true;
            }

            // โ”€โ”€ Fallback: Tyrant โ’ Ultimate Dragon โ”€โ”€
            if (Bot.HasInExtra(CardId.BETyrantDragon))
            {
                AI.SelectCard(CardId.BETyrantDragon);
                return true;
            }
            AI.SelectCard(CardId.BEUltimateDragon);
            return true;
        }

        /// <summary>
        /// Dragon Master Magia โ€” negate activation.
        /// </summary>
        private bool MagiaEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                ClientCard lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.Controller == 1)
                    return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง TRAPS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// True Light โ€” SS BEWD from hand/GY or search S/T.
        /// Enhanced: Always prefer SS BEWD during opponent's turn (instant 3000 ATK body).
        /// Grind game: prioritize SS for board presence recovery.
        /// </summary>
        private bool TrueLightEffect()
        {
            if (_trueLightUsed) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.TrueLight)) return false;

            _trueLightUsed = true;
            
            bool hasBEWDinHandOrGY = Bot.Hand.Any(c => c.Id == CardId.BEWhiteDragon)
                || Bot.Graveyard.Any(c => c.Id == CardId.BEWhiteDragon);
            
            // During opponent's turn: ALWAYS SS BEWD if possible (instant disruption body)
            if (Duel.Player == 1 && hasBEWDinHandOrGY)
            {
                AI.SelectOption(0);
                AI.SelectCard(CardId.BEWhiteDragon);
                AI?.Log(LogLevel.Info, "[TRUE-LIGHT] Opponent turn โ’ SS BEWD (3000 ATK disruption body)");
                return true;
            }

            // Grind game or empty board: prioritize SS for board presence
            if (hasBEWDinHandOrGY && (NeedsBoardPresence() || _currentPhase == ComboPhase.GrindGame))
            {
                AI.SelectOption(0);
                AI.SelectCard(CardId.BEWhiteDragon);
                AI?.Log(LogLevel.Info, "[TRUE-LIGHT] Recovery โ’ SS BEWD from hand/GY");
                return true;
            }
            
            if (hasBEWDinHandOrGY)
            {
                AI.SelectOption(0);
                AI.SelectCard(CardId.BEWhiteDragon);
                AI?.Log(LogLevel.Info, "[TRUE-LIGHT] Special Summon BEWD from hand/GY");
            }
            else
            {
                // Option 1: Search/SET a Blue-Eyes S/T
                AI.SelectOption(1);
                
                // Search priority: Roar (searches Sage) > Wishes (revives) > Majesty (protection)
                if (Bot.GetRemainingCount(CardId.RoarOfTheBEDragons, 3) > 0 && !Bot.HasInHand(CardId.RoarOfTheBEDragons))
                    AI.SelectCard(CardId.RoarOfTheBEDragons);
                else if (Bot.GetRemainingCount(CardId.WishesForEyesOfBlue, 3) > 0 && !Bot.HasInHand(CardId.WishesForEyesOfBlue))
                    AI.SelectCard(CardId.WishesForEyesOfBlue);
                else if (Bot.GetRemainingCount(CardId.MajestyOfTheWhiteDragons, 3) > 0)
                    AI.SelectCard(CardId.MajestyOfTheWhiteDragons);
                else
                    AI.SelectCard(CardId.WishesForEyesOfBlue, CardId.RoarOfTheBEDragons, CardId.MajestyOfTheWhiteDragons);
                
                AI?.Log(LogLevel.Info, "[TRUE-LIGHT] Searching Blue-Eyes S/T");
            }

            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง EXTRA DECK SUMMONS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// Hieratic Seal of the Heavenly Spheres โ€” Link Summon.
        /// </summary>
        private bool HieraticSealSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldAvoidGenericExtraDeckSummon(2)) return false;
            
            int dragonCount = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Dragon));
            return dragonCount >= 2;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง ON SELECT POSITION โ€” Smart placement
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Boss monsters โ’ Attack Position
            if (cardId == CardId.DragonMasterMagia || cardId == CardId.CosmicBlazarDragon 
                || cardId == CardId.StardustSifrDragon || cardId == CardId.BETyrantDragon
                || cardId == CardId.BEChaosMaxDragon || cardId == CardId.BEUltimateDragon)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }
            
            // Synchro monsters on turn 1 โ’ Defense (safety)
            if (cardId == CardId.BESpiritDragon && Duel.Turn == 1)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            // Combo pieces / tuners โ’ Defense
            if (cardId == CardId.SageWithEyesOfBlue || cardId == CardId.MaidenOfWhite
                || cardId == CardId.EffectVeiler)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }

    [Deck("Expert_2026_BlueEyes", "2026_BlueEyes")]
    public class ExpertBlueEyesExecutor : _2026_BlueEyesExecutor
    {
        private string _duelId;
        public ExpertBlueEyesExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _duelId = $"duel_{Guid.NewGuid():N}";
            // [REMOVED-AI-TRAINING] ExpertDataLogger.EnsureInitialized(ExpertDataLogger.FindProjectRoot());
        }
        public override MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            var action = base.OnSelectIdleCmd(main);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogMainPhaseDecision(main, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            var action = base.OnBattle(attackers, defenders);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogBattleDecision(attackers, defenders, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
    }
}
