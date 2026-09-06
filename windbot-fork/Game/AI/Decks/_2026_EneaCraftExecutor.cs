using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // CARD AUDIT — EneaCraft (Enneacraft & Mega-Zaborg Extra Nuke)
    // ============================================================
    // | Card Name                                      | Type    | OPT? | Cost               | Effect Summary                                | Activate When                     | NEVER Activate When                    |
    // |------------------------------------------------|---------|------|--------------------|-----------------------------------------------|-----------------------------------|----------------------------------------|
    // | Zaborg the Mega Monarch (87602890)             | Monster | None | Tribute 2 (LIGHT)  | Destroy 1 monster; if LIGHT, send 8 ED each   | Tribute Summoned w/ LIGHT material| No Extra Deck targets                  |
    // | The Winged Dragon of Ra - Sphere Mode (10000080)| Monster | None | Tribute 3 opp      | Tribute 3 opponent monsters to summon on field | Opponent has 3+ face-up monsters  | We have full lethal board              |
    // | Proto Enneacraft - "orgIA" (56187077)          | Pend/M  | HOPT | None               | Hand: SS L9 LIGHT face-down; Flip: Pop monster| Hand setup / Flipped face-up       | No enemy monster                       |
    // | Deftero Enneacraft - "alazoneIA" (92171126)    | Pend/M  | HOPT | None               | Hand: SS L9 LIGHT face-down; Flip: Banish hand| Hand setup / Flipped face-up       | Opponent hand empty                    |
    // | Ekto Enneacraft - "tromarIA" (29570824)        | Pend/M  | HOPT | None               | Hand: Equip (banish batt); Flip: Revive GY def| Hand equip / Flipped face-up      | No Enneacraft on field                 |
    // | Enato Enneacraft - "oknirIA" (55965529)        | Pend/M  | HOPT | None               | Hand: SS L9 LIGHT face-down; Flip: Shuffle GY | Hand setup / Flipped face-up       | Enemy GY empty                         |
    // | Trito Enneacraft - "exapatisIA" (44716748)     | Pend/M  | HOPT | None               | Hand: SS L9 LIGHT face-down; Flip: Pop S/T    | Hand setup / Flipped face-up       | No enemy S/T                           |
    // | Enneacraft - Aiza.LEON (82359538)              | Pend/M  | HOPT | None               | Quick: Bounce up to 3 cards; Flip: Opp LP burn| Opponent chains / Flipped face-up  | No opponent cards                      |
    // | Enneacraft - Asta.PIXEA (28454232)             | Pend/M  | HOPT | None               | Hand: Equip (double attack); Flip: Pop 1 card | Hand equip / Flipped face-up      | No Enneacraft on field                 |
    // | Enneacraft - Atori.MAR (54842941)              | Pend/M  | HOPT | None               | Quick: Negate monster effect / Book all opp   | Opponent chains / Battle Phase     | No opponent monster                     |
    // | Enneacraft - Archa.TAIL (81237046)             | Pend/M  | HOPT | None               | Quick: Protection shield; Flip: Opp LP burn   | Opponent destruction response     | No destruction threat                  |
    // | Enneacraft - Atil.SPIA (71801447)              | Pend/M  | HOPT | None               | Quick: Negate CL3+; Flip: Salvage from GY     | Chain Link 3+ / Flipped face-up    | Chain Link < 3                         |
    // | Enneacraft Release (54020393)                  | Spell   | HOPT | None               | Hand: Set Scale in P-Zone; GY: Salvage ED     | Main Phase / Scale incomplete      | Scales full                            |
    // | Enneacraft Reverth (80015408)                  | Spell QP| HOPT | None               | Hand: Refresh; GY: Opp summon -> Flip/SS face-down| Opponent summons monster      | No face-down Enneacraft / hand empty   |
    // | Enneacraft Reset (19504025)                    | Spell C | HOPT | None               | Hand: Place Enneapolis; GY: Flip our mons face-down| Opponent effect / End Phase  | No face-up Enneacrafts                 |
    // | Enneapolis (17621695)                          | Field Sp| HOPT | None               | Search Enneacraft; Bounce face-up Pendulum    | Main Phase                         | Already active                         |
    // | Sol and Luna (19739265)                        | Spell QP| None | None               | Quick: Flip 1 our + 1 opp face-up/down        | Opponent threat / Battle Phase     | No valid monsters on both sides        |
    // | Fossil Warrior Skull Knight (59531356)         | Fusion  | None | Banish from GY     | GY: Destroy 1 monster                         | Main Phase, opponent has monster   | No monster target                      |
    // | Fossil Machine Skull Wagon (83656563)          | Fusion  | None | Banish from GY     | GY: Destroy 1 Spell/Trap                      | Main Phase, opponent has S/T       | No S/T target                          |
    // | Golden Cloud Beast - Malong (93125329)         | Synchro | None | Sent to GY         | GY: Bounce 1 face-up card to hand             | When sent to GY (Zaborg dump)      | No face-up card                        |
    // | Wind Pegasus @Ignister (98506199)              | Synchro | None | Banish from GY     | GY: Shuffle 1 card when our card destroyed    | Our card destroyed                 | Enemy field empty                      |
    // | Jurrac Meteor (17548456)                       | Synchro | None | None               | On Summon: DESTROY ALL CARDS ON FIELD         | Special Summoned via Astero        | Self-wipe when board is winning        |
    // | Jurrac Astero (52553102)                       | Synchro | HOPT | Banish self+Dino   | GY Quick: SS Jurrac Meteor from Extra Deck    | Opponent's turn during extension   | Our board is winning                   |
    // | Tri-Brigade Ferrijit (26847978)                | Link-2  | HOPT | None               | GY: Draw 1 card, put 1 card on bottom of Deck | When sent to GY (Zaborg dump)      | Empty hand                             |
    // | Tri-Brigade Arms Mouser (33781156)             | Link-5  | HOPT | None               | GY: Send 1 Beast/Beast-Warrior/Winged Beast   | When sent to GY                    | No target in ED                        |
    // ============================================================
    // ACE CARDS: Primary: Enneacraft - Atori.MAR, Enneacraft - Aiza.LEON / Secondary: Zaborg the Mega Monarch, Enneacraft - Archa.TAIL, Enneacraft - Atil.SPIA, Enneacraft - Asta.PIXEA
    // COMBO STARTERS: 1. Enneacraft Release 2. Proto Enneacraft - "orgIA" 3. Zaborg the Mega Monarch
    // CHOKEPOINTS: Tribute summon of Zaborg gets negated (Solemn/etc.); Enneacraft face-down removal before flip.
    // WIN CONDITION: Nuke opponent's Extra Deck (8 cards) using Zaborg, trigger GY disruptions (Skull Knight, Skull Wagon, Malong, Ferrijit, Astero/Meteor), flip Atori.MAR / Aiza.LEON to control the board, then OTK with Level 9 Bosses (Asta.PIXEA double attack).
    // GOING 1ST END BOARD: 2-3 face-down Enneacraft Bosses + Sol and Luna + Enneacraft Reset/Reverth in GY + Extra Deck Nuke active (4-6 disruptions).
    // GOING 2ND GAMEPLAN: Clear board with Sphere Mode / Zaborg / Jurrac Meteor, then OTK with Atori.MAR + Asta.PIXEA (double attack 3000x2 = 6000+).
    // ============================================================

    // ============================================================
    // COMBO DRAFT — EneaCraft
    // ============================================================
    //
    // === COMBO LINE 1: Zaborg Extra Deck Nuke (Primary Line) ===
    // HAND REQUIRED: Zaborg the Mega Monarch + any Enneacraft Level 1 / Level 9 + another monster (or hand SS)
    // STEP 1: Use Enneacraft monster hand effect to Special Summon a Level 9 LIGHT Boss (e.g. Atori.MAR / Aiza.LEON) face-down.
    // STEP 2: Normal Summon a Level 1 Enneacraft (or another monster) to have 2 Tributes.
    // STEP 3: Tribute Summon Zaborg the Mega Monarch using the LIGHT Level 9 Boss + other monster.
    // STEP 4: Zaborg effect triggers -> Target self to destroy -> Send 8 cards from opponent's Extra Deck (we choose!) and 8 from ours.
    // STEP 5: Our GY triggers fire: Ferrijit draws 1 & bottoms 1, Malong bounces threat, Skull Knight/Wagon ready to pop, Astero ready for Meteor wipe.
    // END BOARD: Opponent's Extra Deck gutted (no bosses left), GY triggers primed, face-down bosses ready.
    // DISRUPTION COUNT: 5-7 (Zaborg ED strip + Astero/Meteor wipe + Skull Knight pop + Malong bounce + Set Spells).
    //
    // === COMBO LINE 2: Atori & Aiza Flip Control Line ===
    // HAND REQUIRED: Enneacraft Release + Enneacraft monster + Sol and Luna
    // STEP 1: Activate Enneacraft Release to place Scale 10 (Atori.MAR) in Pendulum Zone.
    // STEP 2: Hand effect of Enneacraft monster -> Special Summon Aiza.LEON or Atori.MAR face-down.
    // STEP 3: Set Sol and Luna and Enneacraft Reset.
    // STEP 4: Opponent's turn: Activate Sol and Luna -> Flip Atori.MAR face-up (book all opponent monsters) & flip opponent's attacker face-down!
    // STEP 5: Opponent chain -> Aiza.LEON bounces up to 3 cards!
    // END BOARD: 2 face-up/face-down bosses + P-Zone + Set Spells.
    //
    // === COMBO LINE 3: Going 2nd Sphere Mode & OTK Push ===
    // HAND REQUIRED: The Winged Dragon of Ra - Sphere Mode + Enneacraft Bosses + Asta.PIXEA
    // STEP 1: Tribute 3 opponent threat monsters to Special Summon Sphere Mode on their field.
    // STEP 2: Special Summon Enneacraft - Atori.MAR (3000 ATK) or Aiza.LEON (3000 ATK).
    // STEP 3: Equip Enneacraft - Asta.PIXEA to grant double attack.
    // STEP 4: Attack and deal over 8,000 damage for OTK!
    //
    // === COMBO LINE 4: Fallback / Survival Defensive Line ===
    // HAND REQUIRED: Level 1 Enneacrafts + Backrow
    // STEP 1: Normal Set Level 1 Enneacraft (Proto / Trito / Deftero / Enato) in DEF (2000+ DEF).
    // STEP 2: Set Sol and Luna, Infinite Impermanence, or Enneacraft Reset.
    // STEP 3: Survive opponent turn and prepare follow-up.
    // ============================================================

    [Deck("2026_EneaCraft", "2026_EneaCraft")]
    public class _2026_EneaCraftExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int ZaborgTheMegaMonarch = 87602890;
            public const int TheWingedDragonOfRaSphereMode = 10000080;
            public const int ProtoEnneacraftOrgIA = 56187077;
            public const int DefteroEnneacraftAlazoneIA = 92171126;
            public const int EktoEnneacraftTromarIA = 29570824;
            public const int EnatoEnneacraftOknirIA = 55965529;
            public const int TritoEnneacraftExapatisIA = 44716748;
            public const int EnneacraftAizaLEON = 82359538;
            public const int EnneacraftAstaPIXEA = 28454232;
            public const int EnneacraftAtoriMAR = 54842941;
            public const int EnneacraftArchaTAIL = 81237046;
            public const int EnneacraftAtilSPIA = 71801447;

            // Hand Traps & Staples
            public const int MaxxC = 23434538;
            public const int AshBlossom = 14558127;
            public const int InfiniteImpermanence = 10045474;
            public const int CalledByTheGrave = 24224830;

            // Spells
            public const int EnneacraftRelease = 54020393;
            public const int EnneacraftReverth = 80015408;
            public const int EnneacraftReset = 19504025;
            public const int Enneapolis = 17621695;
            public const int SolAndLuna = 19739265;

            // Extra Deck
            public const int FossilWarriorSkullKnight = 59531356;
            public const int FossilMachineSkullWagon = 83656563;
            public const int GoldenCloudBeastMalong = 93125329;
            public const int JurracGiganoto = 80032567;
            public const int WindPegasusIgnister = 98506199;
            public const int JurracMeteor = 17548456;
            public const int JurracAstero = 52553102;
            public const int TriBrigadeFerrijitTheBarrenBlossom = 26847978;
            public const int TriBrigadeArmsMouser = 33781156;

            // Other Monsters & Threats
            public const int BBusterDrake = 77411244;
            public const int AAssaultCore = 30012506;
            public const int CCrushWyvern = 3405259;
            public const int UnionDriver = 99249638;
            public const int ABCDragonBuster = 1561110;
            public const int UnionHangar = 66399653;
            public const int CyberDragonInfinity = 10443957;
            public const int CyberDragonNova = 58069384;
            public const int Apollousa = 4280258;
            public const int BaronneDeFleur = 84815190;
            public const int CrystalWing = 50954680;
            public const int AltergeistHexstia = 1508649;
            public const int WitchcrafterMadameVerre = 21522601;
            public const int EternalSoul = 48680970;
            public const int DarkMagicalCircle = 47222536;
            public const int RunickFountain = 70828912;
            public const int UnionCarrier = 83152482;
            public const int IPMasquerena = 65741786;
            public const int CrusadiaAvramax = 21887175;
            public const int KnightmareUnicorn = 38342335;
        }

        private static readonly int[] MachineUnions = {
            CardId.BBusterDrake,
            CardId.AAssaultCore,
            CardId.CCrushWyvern,
            CardId.UnionDriver
        };

        private static readonly int[] EnneacraftMonsters = {
            CardId.ProtoEnneacraftOrgIA,
            CardId.DefteroEnneacraftAlazoneIA,
            CardId.EktoEnneacraftTromarIA,
            CardId.EnatoEnneacraftOknirIA,
            CardId.TritoEnneacraftExapatisIA,
            CardId.EnneacraftAizaLEON,
            CardId.EnneacraftAstaPIXEA,
            CardId.EnneacraftAtoriMAR,
            CardId.EnneacraftArchaTAIL,
            CardId.EnneacraftAtilSPIA
        };

        // Level 9 LIGHT Bosses — used for Zaborg tribute and disruption
        private static readonly int[] LightEnneacrafts = {
            CardId.EnneacraftAizaLEON,
            CardId.EnneacraftAstaPIXEA,
            CardId.EnneacraftAtoriMAR,
            CardId.EnneacraftArchaTAIL,
            CardId.EnneacraftAtilSPIA
        };

        // Level 1 Enneacraft monsters — used for flip effect and set strategy
        private static readonly int[] Level1Enneacrafts = {
            CardId.ProtoEnneacraftOrgIA,
            CardId.DefteroEnneacraftAlazoneIA,
            CardId.EktoEnneacraftTromarIA,
            CardId.EnatoEnneacraftOknirIA,
            CardId.TritoEnneacraftExapatisIA
        };

        private static readonly int[] EnneacraftSpells = {
            CardId.EnneacraftRelease,
            CardId.EnneacraftReverth,
            CardId.EnneacraftReset,
            CardId.Enneapolis
        };

        // Once-Per-Turn Flags
        private bool _releaseUsed = false;
        private bool _reverthUsed = false;
        private bool _resetUsed = false;
        private bool _solLunaUsed = false;
        private bool _zaborgUsed = false;
        private bool _protoEffectUsed = false;
        private bool _defteroEffectUsed = false;
        private bool _enatoEffectUsed = false;
        private bool _tritoEffectUsed = false;
        private bool _atoriMarEffectUsed = false;
        private bool _aizaLeonEffectUsed = false;
        private bool _archaTailEffectUsed = false;
        private bool _atilSpiaEffectUsed = false;
        private bool _astaPixeaEffectUsed = false;
        private bool _tromarIAEffectUsed = false;
        private bool _ferrijitEffectUsed = false;
        private bool _mouserEffectUsed = false;

        // Hand Once-Per-Turn Flags
        private bool _protoHandUsed = false;
        private bool _defteroHandUsed = false;
        private bool _enatoHandUsed = false;
        private bool _tritoHandUsed = false;
        private bool _atoriMarHandUsed = false;
        private bool _aizaLeonHandUsed = false;
        private bool _archaTailHandUsed = false;

        public _2026_EneaCraftExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards for HeuristicGuard
            HeuristicGuard.RegisterAceCards(
                CardId.EnneacraftAtoriMAR,
                CardId.EnneacraftAizaLEON,
                CardId.EnneacraftArchaTAIL,
                CardId.EnneacraftAtilSPIA,
                CardId.EnneacraftAstaPIXEA
            );

            // ── Combo Router: 4 Strategic Lines ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Zaborg-ExtraDeck-Nuke",
                RequiredCards = new List<int> { CardId.ProtoEnneacraftOrgIA, CardId.ZaborgTheMegaMonarch },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ProtoEnneacraftOrgIA, ActionType = ExecutorType.Activate, Description = "Hand effect SS Level 9 LIGHT face-down" },
                    new() { CardId = CardId.ZaborgTheMegaMonarch, ActionType = ExecutorType.Summon, Description = "Tribute Summon Zaborg using LIGHT boss" },
                    new() { CardId = CardId.ZaborgTheMegaMonarch, ActionType = ExecutorType.Activate, Description = "Nuke 8 cards from opponent Extra Deck" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Atori-Aiza-Flip-Control",
                RequiredCards = new List<int> { CardId.EnneacraftAtoriMAR, CardId.EnneacraftRelease },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.EnneacraftRelease, ActionType = ExecutorType.Activate, Description = "Place Atori.MAR in Pendulum Zone" },
                    new() { CardId = CardId.EnneacraftAtoriMAR, ActionType = ExecutorType.SpSummon, Description = "SS Atori / Aiza face-down" },
                    new() { CardId = CardId.SolAndLuna, ActionType = ExecutorType.Activate, Description = "Sol and Luna: Flip Atori face-up & book opponent" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Going2nd-SphereMode-OTK",
                RequiredCards = new List<int> { CardId.TheWingedDragonOfRaSphereMode },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.TheWingedDragonOfRaSphereMode, ActionType = ExecutorType.Summon, Description = "Tribute 3 opponent monsters with Sphere Mode" },
                    new() { CardId = CardId.EnneacraftAstaPIXEA, ActionType = ExecutorType.Activate, Description = "Equip Asta.PIXEA for double attack OTK" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Fallback-Set-Pass",
                RequiredCards = new List<int> { CardId.ProtoEnneacraftOrgIA },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ProtoEnneacraftOrgIA, ActionType = ExecutorType.MonsterSet, Description = "Set Level 1 Enneacraft in DEF" }
                },
                EndBoardScore = 60
            });

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(CardId.ProtoEnneacraftOrgIA, CardId.EnneacraftRelease, CardId.ZaborgTheMegaMonarch);
            BaitPlanner.RegisterBaitCards(CardId.EnneacraftRelease);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.ZaborgTheMegaMonarch, CardId.EnneacraftAtoriMAR);


            // ═══ TIER 0: Hand Traps & Staples (HIGHEST PRIORITY) ═══
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceEffect);

            // ═══ TIER 1: Extra Deck GY Triggers ═══
            AddExecutor(ExecutorType.Summon, CardId.TheWingedDragonOfRaSphereMode, SphereModeSummon);
            AddExecutor(ExecutorType.Activate, CardId.TriBrigadeFerrijitTheBarrenBlossom, FerrijitEffect);
            AddExecutor(ExecutorType.Activate, CardId.TriBrigadeArmsMouser, MouserEffect);
            AddExecutor(ExecutorType.Activate, CardId.JurracAstero, JurracAsteroGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.FossilWarriorSkullKnight, FossilWarriorEffect);
            AddExecutor(ExecutorType.Activate, CardId.FossilMachineSkullWagon, FossilMachineEffect);
            AddExecutor(ExecutorType.Activate, CardId.WindPegasusIgnister, WindPegasusEffect);
            AddExecutor(ExecutorType.Activate, CardId.GoldenCloudBeastMalong, MalongEffect);

            // ═══ TIER 2: Hand/Field Quick Responses & Bosses ═══
            AddExecutor(ExecutorType.Activate, CardId.EnneacraftAtoriMAR, AtoriMarEffect);
            AddExecutor(ExecutorType.Activate, CardId.EnneacraftAizaLEON, AizaLeonEffect);
            AddExecutor(ExecutorType.Activate, CardId.EnneacraftArchaTAIL, ArchaTailEffect);

            // ═══ TIER 3: Enneacraft Level 1 Quick-Flipped Effects ═══
            AddExecutor(ExecutorType.Activate, CardId.ProtoEnneacraftOrgIA, ProtoEffect);
            AddExecutor(ExecutorType.Activate, CardId.DefteroEnneacraftAlazoneIA, DefteroEffect);
            AddExecutor(ExecutorType.Activate, CardId.EnatoEnneacraftOknirIA, EnatoEffect);
            AddExecutor(ExecutorType.Activate, CardId.TritoEnneacraftExapatisIA, TritoEffect);

            // ═══ TIER 4: Zaborg the Mega Monarch ═══
            AddExecutor(ExecutorType.Activate, CardId.ZaborgTheMegaMonarch, ZaborgEffect);
            AddExecutor(ExecutorType.Summon, CardId.ZaborgTheMegaMonarch, ZaborgSummon);

            // ═══ TIER 5: Spells / Setup ═══
            AddExecutor(ExecutorType.Activate, CardId.SolAndLuna, SolAndLunaEffect);
            AddExecutor(ExecutorType.Activate, CardId.EnneacraftRelease, EnneacraftReleaseEffect);
            AddExecutor(ExecutorType.Activate, CardId.EnneacraftReverth, EnneacraftReverthEffect);
            AddExecutor(ExecutorType.Activate, CardId.EnneacraftReset, EnneacraftResetEffect);
            AddExecutor(ExecutorType.Activate, CardId.Enneapolis, EnneapolisEffect);

            // ═══ TIER 6: Equips (tromarIA, PIXEA and SPIA) ═══
            AddExecutor(ExecutorType.Activate, CardId.EktoEnneacraftTromarIA, TromarIAEffect);
            AddExecutor(ExecutorType.Activate, CardId.EnneacraftAstaPIXEA, AstaPixeaEffect);
            AddExecutor(ExecutorType.Activate, CardId.EnneacraftAtilSPIA, AtilSpiaEffect);

            // ═══ TIER 7: Normal Summons & Sets ═══
            AddExecutor(ExecutorType.Summon, CardId.ProtoEnneacraftOrgIA, LowLevelSummon);
            AddExecutor(ExecutorType.Summon, CardId.DefteroEnneacraftAlazoneIA, LowLevelSummon);
            AddExecutor(ExecutorType.Summon, CardId.EnatoEnneacraftOknirIA, LowLevelSummon);
            AddExecutor(ExecutorType.Summon, CardId.TritoEnneacraftExapatisIA, LowLevelSummon);
            AddExecutor(ExecutorType.MonsterSet, CardId.ProtoEnneacraftOrgIA);
            AddExecutor(ExecutorType.MonsterSet, CardId.DefteroEnneacraftAlazoneIA);
            AddExecutor(ExecutorType.MonsterSet, CardId.EnatoEnneacraftOknirIA);
            AddExecutor(ExecutorType.MonsterSet, CardId.TritoEnneacraftExapatisIA);

            // ═══ TIER 8: Spell/Trap Sets ═══
            AddExecutor(ExecutorType.SpellSet, CardId.SolAndLuna);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.EnneacraftReset);
            AddExecutor(ExecutorType.SpellSet, CardId.EnneacraftReverth);

            // ═══ TIER 9: Reposition ═══
            AddExecutor(ExecutorType.Repos, EnneacraftMonsterRepos);
        }

        public override bool OnSelectHand() => true; // Prefer going first for setup

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _releaseUsed = false;
            _reverthUsed = false;
            _resetUsed = false;
            _solLunaUsed = false;
            _zaborgUsed = false;
            _protoEffectUsed = false;
            _defteroEffectUsed = false;
            _enatoEffectUsed = false;
            _tritoEffectUsed = false;
            _atoriMarEffectUsed = false;
            _aizaLeonEffectUsed = false;
            _archaTailEffectUsed = false;
            _atilSpiaEffectUsed = false;
            _astaPixeaEffectUsed = false;
            _tromarIAEffectUsed = false;
            _ferrijitEffectUsed = false;
            _mouserEffectUsed = false;

            _protoHandUsed = false;
            _defteroHandUsed = false;
            _enatoHandUsed = false;
            _tritoHandUsed = false;
            _atoriMarHandUsed = false;
            _aizaLeonHandUsed = false;
            _archaTailHandUsed = false;
            
            if (ShouldGoBreakBoard)
            {
                _enatoEffectUsed = false;
                _atilSpiaEffectUsed = false;
            }
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                int id = card.Id;
                bool fromHand = card.Location == CardLocation.Hand;

                if (id == CardId.ProtoEnneacraftOrgIA)
                {
                    if (fromHand) _protoHandUsed = true;
                    else _protoEffectUsed = true;
                }
                else if (id == CardId.DefteroEnneacraftAlazoneIA)
                {
                    if (fromHand) _defteroHandUsed = true;
                    else _defteroEffectUsed = true;
                }
                else if (id == CardId.EnatoEnneacraftOknirIA)
                {
                    if (fromHand) _enatoHandUsed = true;
                    else _enatoEffectUsed = true;
                }
                else if (id == CardId.TritoEnneacraftExapatisIA)
                {
                    if (fromHand) _tritoHandUsed = true;
                    else _tritoEffectUsed = true;
                }
                else if (id == CardId.EnneacraftAtoriMAR)
                {
                    if (fromHand) _atoriMarHandUsed = true;
                    else _atoriMarEffectUsed = true;
                }
                else if (id == CardId.EnneacraftAizaLEON)
                {
                    if (fromHand) _aizaLeonHandUsed = true;
                    else _aizaLeonEffectUsed = true;
                }
                else if (id == CardId.EnneacraftArchaTAIL)
                {
                    if (fromHand) _archaTailHandUsed = true;
                    else _archaTailEffectUsed = true;
                }
                else if (id == CardId.EnneacraftAtilSPIA) _atilSpiaEffectUsed = true;
                else if (id == CardId.EnneacraftAstaPIXEA) _astaPixeaEffectUsed = true;
                else if (id == CardId.EktoEnneacraftTromarIA) _tromarIAEffectUsed = true;
                else if (id == CardId.EnneacraftRelease) _releaseUsed = true;
                else if (id == CardId.EnneacraftReverth) _reverthUsed = true;
                else if (id == CardId.EnneacraftReset) _resetUsed = true;
                else if (id == CardId.SolAndLuna) _solLunaUsed = true;
                else if (id == CardId.TriBrigadeFerrijitTheBarrenBlossom) _ferrijitEffectUsed = true;
                else if (id == CardId.TriBrigadeArmsMouser) _mouserEffectUsed = true;
            }
        }

        // ============================================================
        // TIER 0: Hand Traps & Staples
        // ============================================================

        private bool MaxxCEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && DefaultMaxxC();
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain == null || lastChain.Controller != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool CalledByTheGraveEffect()
        {
            if (DefaultCalledByTheGrave()) return true;

            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.Controller == 1 && lastChain.Location == CardLocation.Grave)
            {
                if (!lastChain.IsShouldNotBeTarget() && !lastChain.IsShouldNotBeSpellTrapTarget())
                {
                    AI.SelectCard(lastChain);
                    DecisionTracer.TraceActivate("CalledByTheGraveEffect", $"Negating GY activation of {lastChain.Name}");
                    return true;
                }
            }

            if (Duel.LastChainPlayer != 0)
            {
                int[] gyTargets = {
                    CardId.AshBlossom,
                    CardId.MaxxC,
                    CardId.BBusterDrake,
                    CardId.AAssaultCore,
                    CardId.CCrushWyvern,
                    CardId.ABCDragonBuster,
                };
                var target = Enemy.Graveyard.FirstOrDefault(c => c != null && gyTargets.Contains(c.Id));
                if (target != null && !target.IsShouldNotBeTarget() && !target.IsShouldNotBeSpellTrapTarget())
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("CalledByTheGraveEffect", $"Preemptive banish of {target.Name} from GY");
                    return true;
                }
            }

            return false;
        }

        private bool ImpermanenceEffect()
        {
            if (!SmartHandTrapChain()) return false;
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.Controller == 0) return false;

            if (Duel.Player == 1)
            {
                int[] threatIds = {
                    CardId.BaronneDeFleur,
                    CardId.Apollousa,
                    CardId.CyberDragonInfinity,
                    CardId.CrystalWing,
                    CardId.AltergeistHexstia,
                    CardId.ABCDragonBuster,
                    CardId.WitchcrafterMadameVerre,
                };
                var preemptive = Enemy.GetMonsters().FirstOrDefault(c =>
                    c != null && c.IsFaceup() && !c.IsDisabled()
                    && threatIds.Contains(c.Id)
                    && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
                if (preemptive != null)
                {
                    AI.SelectCard(preemptive);
                    DecisionTracer.TraceActivate("ImpermanenceEffect", $"Preemptive negate on {preemptive.Name}");
                    return true;
                }
            }

            return DefaultInfiniteImpermanence();
        }

        // ============================================================
        // TIER 1: Extra Deck GY Triggers
        // ============================================================

        private bool FerrijitEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_ferrijitEffectUsed) return false;

            _ferrijitEffectUsed = true;
            DecisionTracer.TraceActivate("FerrijitEffect", "Tri-Brigade Ferrijit GY filter: draw 1 card, bottom 1");
            return true;
        }

        private bool MouserEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_mouserEffectUsed) return false;

            _mouserEffectUsed = true;
            DecisionTracer.TraceActivate("MouserEffect", "Tri-Brigade Mouser GY dump trigger");
            return true;
        }

        private bool JurracAsteroGYEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Grave) return false;
            if (Duel.Player != 1) return false; // Opponent's turn only

            bool hasMeteor = GetRemainingCount(CardId.JurracMeteor) > 0;
            if (!hasMeteor)
            {
                DecisionTracer.TraceSkip("JurracAsteroGYEffect", "Jurrac Meteor not in Extra Deck");
                return false;
            }

            bool hasDino = Bot.Graveyard.Any(c => c != null && c != Card && c.HasRace(CardRace.Dinosaur));
            if (!hasDino)
            {
                DecisionTracer.TraceSkip("JurracAsteroGYEffect", "No other Dinosaur material in GY");
                return false;
            }

            // Avoid nuking if we already have a strong established board and enemy has nothing
            int ourBosses = Bot.GetMonsters().Count(c => c != null && LightEnneacrafts.Contains(c.Id));
            if (ourBosses >= 2 && Enemy.GetMonsterCount() == 0)
            {
                DecisionTracer.TraceSkip("JurracAsteroGYEffect", "We have established bosses, skipping wipe");
                return false;
            }

            // Trigger when opponent establishes monsters or during their battle phase
            if (Enemy.GetMonsterCount() >= 1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
            {
                DecisionTracer.TraceActivate("JurracAsteroGYEffect", "Wiping opponent field via Jurrac Meteor");
                return true;
            }

            return false;
        }

        private bool FossilWarriorEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Grave) return false;
            
            ClientCard target = GetBestEnemyMonsterByThreat(canBeTarget: true);
            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("FossilWarriorEffect", $"Targeting {target.Name} to destroy");
                return true;
            }
            return false;
        }

        private ClientCard GetBestSpellTrapTarget()
        {
            var spells = Enemy.GetSpells();
            if (spells.Count == 0) return null;

            int[] highThreatSpellTraps = {
                CardId.EternalSoul,
                CardId.DarkMagicalCircle,
                CardId.UnionHangar,
                CardId.RunickFountain,
            };

            var threatSpell = spells.FirstOrDefault(c => c != null && c.IsFaceup() && highThreatSpellTraps.Contains(c.Id));
            if (threatSpell != null) return threatSpell;

            var floodgate = spells.FirstOrDefault(c => c != null && c.IsFaceup() && c.IsFloodgate());
            if (floodgate != null) return floodgate;

            var continuous = spells.FirstOrDefault(c => c != null && c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field) || c.HasType(CardType.Equip) || c.HasType(CardType.Pendulum)));
            if (continuous != null) return continuous;

            var setSpell = spells.FirstOrDefault(c => c != null && c.IsFacedown());
            if (setSpell != null) return setSpell;

            return spells.FirstOrDefault(c => c != null && c.IsFaceup());
        }

        private bool FossilMachineEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Grave) return false;

            ClientCard target = GetBestSpellTrapTarget();
            if (target != null && IsViableEffectTarget(target))
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("FossilMachineEffect", $"Targeting {target.Name} to destroy");
                return true;
            }
            return false;
        }

        private bool WindPegasusEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;

            ClientCard target = GetBestSpellTrapTarget();
            if (target == null || (!target.IsCode(CardId.EternalSoul) && !target.IsCode(CardId.DarkMagicalCircle)))
            {
                var bestMonster = GetBestEnemyMonsterByThreat(canBeTarget: true);
                if (bestMonster != null)
                {
                    int monsterThreat = EvaluateMonsterThreat(bestMonster);
                    if (monsterThreat >= 400 || target == null)
                    {
                        target = bestMonster;
                    }
                }
            }

            if (target != null && IsViableEffectTarget(target))
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("WindPegasusEffect", $"Targeting {target.Name} to spin to deck");
                return true;
            }
            return false;
        }

        private bool MalongEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;

            ClientCard target = GetBestEnemyMonsterByThreat(canBeTarget: true);
            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("MalongEffect", $"Targeting {target.Name} to bounce to hand");
                return true;
            }
            return false;
        }

        // ============================================================
        // TIER 2: Level 9 Enneacraft Boss Effects
        // ============================================================

        private bool AtoriMarEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_atoriMarHandUsed) return false;
                return EnneacraftHandEffect(CardId.EnneacraftAtoriMAR);
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_atoriMarEffectUsed) return false;

                // Face-Up: Quick Effect responses
                if (Card.IsFaceup())
                {
                    if (ShouldRespondToOpponentChain())
                    {
                        DecisionTracer.TraceActivate("AtoriMarEffect", "Negating opponent monster activation");
                        return true;
                    }

                    if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle))
                    {
                        int faceUpEnemyMonsters = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup());
                        if (faceUpEnemyMonsters >= 1)
                        {
                            DecisionTracer.TraceActivate("AtoriMarEffect", $"Battle Phase — book all {faceUpEnemyMonsters} opponent monsters face-down");
                            return true;
                        }
                    }

                    if (Duel.Player == 1 && Duel.Phase == DuelPhase.Main1)
                    {
                        bool hasDangerousMonster = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                            (c.Attack >= 2500 || c.HasType(CardType.Link) || c.HasType(CardType.Xyz) || c.HasType(CardType.Synchro) ||
                             c.IsCode(CardId.Apollousa, CardId.BaronneDeFleur, CardId.CyberDragonInfinity,
                                      CardId.ABCDragonBuster, CardId.CrusadiaAvramax, CardId.KnightmareUnicorn)));
                        int faceUpCount = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup());
                        if (hasDangerousMonster && faceUpCount >= 1)
                        {
                            DecisionTracer.TraceActivate("AtoriMarEffect", $"Proactive — book {faceUpCount} dangerous monsters");
                            return true;
                        }
                    }
                }

                // FLIP Effect: book all opponent monsters
                if (Card.IsFacedown())
                {
                    int faceUpEnemyMonsters = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup());
                    if (faceUpEnemyMonsters > 0)
                    {
                        DecisionTracer.TraceActivate("AtoriMarEffect", $"FLIP — book all {faceUpEnemyMonsters} opponent monsters");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool AtilSpiaEffect()
        {
            if (_atilSpiaEffectUsed) return false;

            if (Card.Location == CardLocation.Hand)
            {
                var target = GetBestEquipTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("AtilSpiaEffect", $"Equipping to {target.Name}");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.CurrentChain.Count >= 2 && ShouldRespondToOpponentChain())
                {
                    DecisionTracer.TraceActivate("AtilSpiaEffect", "Negating chain link 3+");
                    return true;
                }

                var gyCard = Bot.Graveyard.FirstOrDefault(c => c != null && (EnneacraftMonsters.Contains(c.Id) || EnneacraftSpells.Contains(c.Id)));
                if (gyCard != null)
                {
                    AI.SelectCard(gyCard);
                    DecisionTracer.TraceActivate("AtilSpiaEffect", $"FLIP — Salvaged {gyCard.Name} from GY");
                    return true;
                }
            }
            return false;
        }

        private bool AizaLeonEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_aizaLeonHandUsed) return false;
                return EnneacraftHandEffect(CardId.EnneacraftAizaLEON);
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_aizaLeonEffectUsed) return false;

                if (ShouldRespondToOpponentChain())
                {
                    int[] highThreatIds = {
                        CardId.UnionHangar,
                        CardId.CyberDragonInfinity,
                        CardId.ABCDragonBuster,
                        CardId.BBusterDrake,
                        CardId.AAssaultCore,
                        CardId.CCrushWyvern,
                        CardId.UnionCarrier,
                        CardId.IPMasquerena,
                        CardId.CrusadiaAvramax,
                        CardId.KnightmareUnicorn,
                    };

                    var targets = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                        .Where(c => c != null && IsViableEffectTarget(c))
                        .OrderByDescending(c =>
                        {
                            if (c.IsMonster()) return EvaluateMonsterThreat(c);
                            if (highThreatIds.Contains(c.Id)) return 10000;
                            if (c.EquipCards != null && c.EquipCards.Count > 0) return 9000;
                            return 500;
                        })
                        .Take(3)
                        .ToList();
                    if (targets.Count > 0)
                    {
                        AI.SelectCard(targets);
                        DecisionTracer.TraceActivate("AizaLeonEffect", $"Bouncing {targets.Count} opponent cards");
                        return true;
                    }
                }

                if (Enemy.Hand.Count > 0)
                {
                    DecisionTracer.TraceActivate("AizaLeonEffect", "FLIP — Opponent loses LP");
                    return true;
                }
            }
            return false;
        }

        private bool ArchaTailEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_archaTailHandUsed) return false;
                return EnneacraftHandEffect(CardId.EnneacraftArchaTAIL);
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_archaTailEffectUsed) return false;

                if (ShouldRespondToOpponentChain())
                {
                    DecisionTracer.TraceActivate("ArchaTailEffect", "Activating protection shield");
                    return true;
                }

                if (Enemy.Graveyard.Count > 0)
                {
                    DecisionTracer.TraceActivate("ArchaTailEffect", "FLIP — Opponent loses LP");
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        // TIER 3: Level 1 Enneacraft Monster Effects
        // ============================================================

        private bool ProtoEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_protoHandUsed) return false;
                return EnneacraftHandEffect(CardId.ProtoEnneacraftOrgIA);
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_protoEffectUsed) return false;

                if (Card.IsFacedown() && ShouldRespondToOpponentChain())
                {
                    DecisionTracer.TraceActivate("ProtoEffect", "Quick Effect — search on hand trigger");
                    return true;
                }
                
                if (Card.IsFaceup())
                {
                    ClientCard target = Enemy.GetMonsters()
                        .Where(c => c != null && IsViableEffectTarget(c))
                        .OrderByDescending(c => {
                            int score = EvaluateMonsterThreat(c);
                            if (c.IsFaceup()) score += 500;
                            return score;
                        })
                        .FirstOrDefault();
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        DecisionTracer.TraceActivate("ProtoEffect", $"FLIP — Pop monster {target.Name}");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DefteroEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_defteroHandUsed) return false;
                return EnneacraftHandEffect(CardId.DefteroEnneacraftAlazoneIA);
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_defteroEffectUsed) return false;

                if (Card.IsFacedown() && ShouldRespondToOpponentChain())
                {
                    DecisionTracer.TraceActivate("DefteroEffect", "Quick Effect — search on add hand trigger");
                    return true;
                }

                if (Card.IsFaceup() && Enemy.Hand.Count > 0)
                {
                    DecisionTracer.TraceActivate("DefteroEffect", "FLIP — Banish 1 random card from opponent hand");
                    return true;
                }
            }
            return false;
        }

        private bool EnatoEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_enatoHandUsed) return false;
                return EnneacraftHandEffect(CardId.EnatoEnneacraftOknirIA);
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_enatoEffectUsed) return false;

                if (Card.IsFacedown() && ShouldRespondToOpponentChain())
                {
                    DecisionTracer.TraceActivate("EnatoEffect", "Quick Effect — search on GY trigger");
                    return true;
                }

                if (Card.IsFaceup())
                {
                    int[] highPriorityShuffles = {
                        CardId.BBusterDrake,
                        CardId.AAssaultCore,
                        CardId.CCrushWyvern,
                        CardId.ABCDragonBuster,
                        CardId.CyberDragonNova,
                        CardId.CyberDragonInfinity,
                    };

                    var targets = Enemy.Graveyard.Concat(Enemy.Banished)
                        .Where(c => c != null)
                        .OrderByDescending(c => highPriorityShuffles.Contains(c.Id) ? 10 : (c.IsMonster() ? 5 : 1))
                        .ThenByDescending(c => c.Attack)
                        .Take(3)
                        .ToList();
                    if (targets.Count > 0)
                    {
                        AI.SelectCard(targets);
                        DecisionTracer.TraceActivate("EnatoEffect", $"FLIP — shuffle {targets.Count} GY/banished cards");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TritoEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_tritoHandUsed) return false;
                return EnneacraftHandEffect(CardId.TritoEnneacraftExapatisIA);
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_tritoEffectUsed) return false;

                if (Card.IsFacedown() && ShouldRespondToOpponentChain())
                {
                    DecisionTracer.TraceActivate("TritoEffect", "Quick Effect — search on Set trigger");
                    return true;
                }

                if (Card.IsFaceup())
                {
                    ClientCard target = GetBestSpellTrapTarget();
                    if (target != null && IsViableEffectTarget(target))
                    {
                        AI.SelectCard(target);
                        DecisionTracer.TraceActivate("TritoEffect", $"FLIP — Pop Spell/Trap {target.Name}");
                        return true;
                    }
                }
            }
            return false;
        }

        // ============================================================
        // Quick Effect Gate
        // ============================================================

        private int EvaluateChainThreat(ClientCard chainCard)
        {
            if (chainCard == null) return 0;
            int score = 0;

            if (chainCard.IsMonster())
            {
                score += 50;
                if (chainCard.Attack >= 2500) score += 30;
                if (chainCard.HasType(CardType.Link)) score += 20;

                if (chainCard.IsCode(
                    CardId.Apollousa,
                    CardId.BaronneDeFleur,
                    CardId.CyberDragonInfinity,
                    CardId.ABCDragonBuster,
                    CardId.CrusadiaAvramax,
                    CardId.KnightmareUnicorn))
                {
                    score += 100;
                }
            }
            else
            {
                if (chainCard.IsCode(
                    CardId.UnionHangar,
                    CardId.EternalSoul,
                    CardId.DarkMagicalCircle,
                    CardId.RunickFountain,
                    12580477, // Raigeki
                    18144506, // Harpie's Feather Duster
                    14532163, // Lightning Storm
                    32807846, // Evenly Matched
                    5318639,  // Dark Hole
                    24224830, // Called by the Grave
                    10045474  // Infinite Impermanence
                ))
                {
                    score += 100;
                }

                if (chainCard.IsFaceup() && (chainCard.HasType(CardType.Continuous) || chainCard.HasType(CardType.Field) || chainCard.HasType(CardType.Equip) || chainCard.HasType(CardType.Pendulum)))
                {
                    score += 30;
                }
            }

            return score;
        }

        private bool ShouldRespondToOpponentChain()
        {
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain == null) return false;
            if (lastChain.Controller != 1) return false;
            return EvaluateChainThreat(lastChain) >= 30;
        }

        private int EvaluateBoardState(bool isBot)
        {
            var player = isBot ? Bot : Enemy;
            int score = 0;

            var monsters = player.GetMonsters();
            foreach (var m in monsters)
            {
                if (m == null) continue;
                if (m.IsFaceup())
                {
                    score += m.Attack;
                    if (isBot)
                    {
                        if (LightEnneacrafts.Contains(m.Id)) score += 1000;
                    }
                    else
                    {
                        if (m.IsCode(CardId.Apollousa, CardId.BaronneDeFleur, CardId.CyberDragonInfinity, CardId.ABCDragonBuster, CardId.CrusadiaAvramax, CardId.KnightmareUnicorn))
                            score += 2500;
                        else if (m.Attack >= 2500)
                            score += 1000;
                    }
                }
                else if (isBot && EnneacraftMonsters.Contains(m.Id))
                {
                    score += 800;
                }
            }

            int spellCount = player.GetSpells().Count(c => c != null);
            score += spellCount * 500;
            score += player.Hand.Count * 400;
            score += player.LifePoints / 4;

            return score;
        }

        private bool IsDesperateMode()
        {
            int myScore = EvaluateBoardState(true);
            int enemyScore = EvaluateBoardState(false);
            return enemyScore > myScore * 2;
        }

        private int EvaluateMonsterThreat(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;

            score += card.Attack / 10;
            if (card.HasType(CardType.Link)) score += 100;
            if (card.HasType(CardType.Synchro)) score += 80;
            if (card.HasType(CardType.Xyz)) score += 80;
            if (card.HasType(CardType.Fusion)) score += 60;

            if (card.IsCode(CardId.Apollousa)) score += 1000;
            if (card.IsCode(CardId.BaronneDeFleur)) score += 900;
            if (card.IsCode(CardId.CyberDragonInfinity)) score += 850;
            if (card.IsCode(CardId.UnionCarrier)) score += 800;
            if (card.IsCode(CardId.IPMasquerena)) score += 750;
            if (card.IsCode(CardId.CrusadiaAvramax)) score += 700;
            if (card.IsCode(CardId.KnightmareUnicorn)) score += 650;

            if (card.IsCode(CardId.ABCDragonBuster))
            {
                score += (Duel.Player == 0) ? 950 : 300;
            }

            if (MachineUnions.Contains(card.Id)) score += 500;
            if (card.EquipCards != null && card.EquipCards.Count > 0) score += 600;
            if (!card.IsDisabled()) score += 200;

            return score;
        }

        private ClientCard GetBestEnemyMonsterByThreat(bool canBeTarget = true)
        {
            return Enemy.GetMonsters()
                .Where(c => c != null && (!canBeTarget || IsViableEffectTarget(c)))
                .OrderByDescending(c => EvaluateMonsterThreat(c))
                .FirstOrDefault();
        }

        private bool EnemyUsesExtraDeck()
        {
            if (Enemy.ExtraDeck.Count < 5) return false;

            int[] nonEdCardIds = {
                82331575, 65004735, 37376378, 28189874, 16674826, 52758156, // Floowandereeze
                68482979, 32800889, 73384260, 61845184, 80112101, 29334547, 65002047, 81373516, // True Draco
                78884013 // Domain of the True Monarchs
            };

            bool hasNonEdIndicators = Enemy.GetMonsters().Any(c => c != null && nonEdCardIds.Contains(c.Id))
                || Enemy.GetSpells().Any(c => c != null && nonEdCardIds.Contains(c.Id))
                || Enemy.Graveyard.Any(c => c != null && nonEdCardIds.Contains(c.Id))
                || Enemy.Banished.Any(c => c != null && nonEdCardIds.Contains(c.Id));

            if (hasNonEdIndicators)
                return false;

            return true;
        }

        private double GetExpectedInteractionScore()
        {
            double score = 0.0;

            foreach (var m in Bot.GetMonsters())
            {
                if (m == null) continue;

                if (m.IsCode(CardId.EnneacraftAtoriMAR))
                {
                    if (m.IsFaceup() && !_atoriMarEffectUsed)
                        score += 3.0;
                    else if (m.IsFacedown() && !_atoriMarEffectUsed)
                        score += 2.0;
                }
                else if (m.IsCode(CardId.EnneacraftAizaLEON) && !_aizaLeonEffectUsed)
                {
                    score += 2.0;
                }
                else if (m.IsCode(CardId.ProtoEnneacraftOrgIA) && !_protoEffectUsed && m.IsFacedown())
                {
                    score += 1.5;
                }
                else if (m.IsCode(CardId.TritoEnneacraftExapatisIA) && !_tritoEffectUsed && m.IsFacedown())
                {
                    score += 1.0;
                }
                else if (m.IsCode(CardId.DefteroEnneacraftAlazoneIA) && !_defteroEffectUsed && m.IsFacedown())
                {
                    score += 1.0;
                }
                else if (m.IsCode(CardId.EnatoEnneacraftOknirIA) && !_enatoEffectUsed && m.IsFacedown())
                {
                    score += 1.0;
                }
            }

            bool hasSolLunaSet = Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.SolAndLuna));
            if (hasSolLunaSet && !_solLunaUsed) score += 1.0;

            bool hasImpermSet = Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.InfiniteImpermanence));
            if (hasImpermSet) score += 2.0;

            bool hasCalledBySet = Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.CalledByTheGrave));
            if (hasCalledBySet) score += 1.5;

            if (Bot.Hand.Any(c => c != null && c.Id == CardId.AshBlossom)) score += 2.0;
            if (Bot.Hand.Any(c => c != null && c.Id == CardId.MaxxC)) score += 2.5;
            if (Bot.Hand.Any(c => c != null && c.Id == CardId.InfiniteImpermanence)) score += 2.0;

            return score;
        }

        // ============================================================
        // TIER 4: Zaborg the Mega Monarch & Sphere Mode Summons
        // ============================================================

        private bool ZaborgSummon()
        {
            int monsterCount = Bot.GetMonsterCount();
            if (monsterCount < 2) return false;

            if (!IsDesperateMode() && (!EnemyUsesExtraDeck() || Enemy.ExtraDeck.Count < 5))
            {
                DecisionTracer.TraceSkip("ZaborgSummon", "Opponent doesn't rely on Extra Deck or it's empty");
                return false;
            }

            bool hasLightMaterial = Bot.GetMonsters().Any(c => c != null && LightEnneacrafts.Contains(c.Id));
            if (!hasLightMaterial)
            {
                int ourBosses = Bot.GetMonsters().Count(c => c != null && LightEnneacrafts.Contains(c.Id));
                if (ourBosses == 0)
                {
                    DecisionTracer.TraceActivate("ZaborgSummon", "Tribute Summoning Zaborg without LIGHT material");
                    return true;
                }
                DecisionTracer.TraceSkip("ZaborgSummon", "No LIGHT monster on field and we have boss(es) set up");
                return false;
            }

            DecisionTracer.TraceActivate("ZaborgSummon", "Tribute Summoning Zaborg the Mega Monarch");
            return true;
        }

        private bool ZaborgEffect()
        {
            if (_zaborgUsed) return false;

            AI.SelectCard(Card);
            _zaborgUsed = true;
            DecisionTracer.TraceActivate("ZaborgEffect", "Zaborg destroying itself to nuke Extra Decks");
            return true;
        }

        private bool SphereModeSummon()
        {
            int enemyMonsters = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup());
            if (enemyMonsters >= 3)
            {
                var targets = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup())
                    .OrderByDescending(c => c.Attack)
                    .Take(3)
                    .ToList();
                AI.SelectCard(targets);
                DecisionTracer.TraceActivate("SphereModeSummon", "Sphere Mode tributing 3 opponent monsters");
                return true;
            }
            return false;
        }

        // ============================================================
        // TIER 5: Spell Cards
        // ============================================================

        private bool SolAndLunaEffect()
        {
            if (_solLunaUsed) return false;

            var ourMonsters = Bot.GetMonsters().Where(c => c != null).ToList();
            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();

            if (ourMonsters.Count == 0 || oppMonsters.Count == 0) return false;

            var faceDownCandidates = ourMonsters.Where(c => c.IsFacedown() && EnneacraftMonsters.Contains(c.Id)).ToList();
            var faceUpUsedCandidates = ourMonsters.Where(c => c.IsFaceup() && EnneacraftMonsters.Contains(c.Id) && HasUsedFlipEffect(c)).ToList();
            var faceUpAnyCandidates = ourMonsters.Where(c => c.IsFaceup() && EnneacraftMonsters.Contains(c.Id)).ToList();

            var allCandidates = faceDownCandidates.Concat(faceUpUsedCandidates).Concat(faceUpAnyCandidates).Distinct().ToList();
            if (allCandidates.Count == 0) return false;

            if (Duel.Player == 0)
            {
                bool goingSecond = _isGoingSecond;
                bool enemyHasBigThreat = oppMonsters.Any(c => c.Attack >= 2500 ||
                    c.IsCode(CardId.Apollousa, CardId.BaronneDeFleur, CardId.CyberDragonInfinity,
                             CardId.ABCDragonBuster, CardId.CrusadiaAvramax, CardId.KnightmareUnicorn));
                bool isMP2 = Duel.Phase == DuelPhase.Main2;

                if (!goingSecond && !isMP2)
                {
                    DecisionTracer.TraceSkip("SolAndLunaEffect", "Saving Sol & Luna for opponent's turn");
                    return false;
                }
                if (goingSecond && !enemyHasBigThreat && !isMP2)
                {
                    DecisionTracer.TraceSkip("SolAndLunaEffect", "No big threat to justify own-turn activation");
                    return false;
                }
            }
            else
            {
                bool isBattlePhase = Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle;
                bool isChainResponse = Duel.LastChainPlayer == 1;
                bool isEndPhase = Duel.Phase == DuelPhase.End;

                if (!isBattlePhase && !isChainResponse && !isEndPhase)
                {
                    bool opponentHasThreat = oppMonsters.Any(c =>
                        c.Attack >= 2000 ||
                        MachineUnions.Contains(c.Id) ||
                        c.IsCode(CardId.ABCDragonBuster, CardId.CyberDragonInfinity, CardId.Apollousa,
                                 CardId.UnionCarrier, CardId.IPMasquerena, CardId.CrusadiaAvramax, CardId.KnightmareUnicorn) ||
                        (c.EquipCards != null && c.EquipCards.Count > 0) ||
                        c.HasType(CardType.Link) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz)
                    );

                    if (!opponentHasThreat)
                    {
                        DecisionTracer.TraceSkip("SolAndLunaEffect", "No threat or critical phase, saving Sol & Luna");
                        return false;
                    }
                }
            }

            ClientCard ourMonster = null;

            if (faceDownCandidates.Count > 0)
            {
                if (oppMonsters.Count >= 2)
                    ourMonster = faceDownCandidates.FirstOrDefault(c => c.IsCode(CardId.EnneacraftAtoriMAR));
                if (ourMonster == null && oppMonsters.Count > 0)
                    ourMonster = faceDownCandidates.FirstOrDefault(c => c.IsCode(CardId.ProtoEnneacraftOrgIA));
                if (ourMonster == null && Enemy.GetSpells().Count > 0)
                    ourMonster = faceDownCandidates.FirstOrDefault(c => c.IsCode(CardId.TritoEnneacraftExapatisIA));
                if (ourMonster == null && oppMonsters.Count > 0)
                    ourMonster = faceDownCandidates.FirstOrDefault(c => c.IsCode(CardId.EnneacraftAtoriMAR));
                if (ourMonster == null && Enemy.Hand.Count > 0)
                    ourMonster = faceDownCandidates.FirstOrDefault(c => c.IsCode(CardId.DefteroEnneacraftAlazoneIA));
                if (ourMonster == null && (Enemy.Graveyard.Count > 0 || Enemy.Banished.Count > 0))
                    ourMonster = faceDownCandidates.FirstOrDefault(c => c.IsCode(CardId.EnatoEnneacraftOknirIA));
                if (ourMonster == null)
                    ourMonster = faceDownCandidates.FirstOrDefault();
            }

            if (ourMonster == null && faceUpUsedCandidates.Count > 0)
            {
                ourMonster = faceUpUsedCandidates.FirstOrDefault();
            }
            if (ourMonster == null && faceUpAnyCandidates.Count > 0)
            {
                ourMonster = faceUpAnyCandidates.FirstOrDefault();
            }

            var oppThreat = GetBestFlipDownTarget(oppMonsters);

            if (ourMonster != null && oppThreat != null)
            {
                AI.SelectCard(ourMonster);
                AI.SelectNextCard(oppThreat);
                _solLunaUsed = true;
                string flipDir = ourMonster.IsFacedown() ? "face-up" : "face-down";
                DecisionTracer.TraceActivate("SolAndLunaEffect", $"Flipping our {ourMonster.Name} {flipDir} and {oppThreat.Name} face-down");
                return true;
            }

            return false;
        }

        private bool HasUsedFlipEffect(ClientCard card)
        {
            if (card == null) return false;
            if (card.IsCode(CardId.EnneacraftAtoriMAR)) return _atoriMarEffectUsed;
            if (card.IsCode(CardId.ProtoEnneacraftOrgIA)) return _protoEffectUsed;
            if (card.IsCode(CardId.DefteroEnneacraftAlazoneIA)) return _defteroEffectUsed;
            if (card.IsCode(CardId.EnatoEnneacraftOknirIA)) return _enatoEffectUsed;
            if (card.IsCode(CardId.TritoEnneacraftExapatisIA)) return _tritoEffectUsed;
            if (card.IsCode(CardId.EnneacraftAizaLEON)) return _aizaLeonEffectUsed;
            if (card.IsCode(CardId.EnneacraftArchaTAIL)) return _archaTailEffectUsed;
            if (card.IsCode(CardId.EnneacraftAtilSPIA)) return _atilSpiaEffectUsed;
            if (card.IsCode(CardId.EnneacraftAstaPIXEA)) return _astaPixeaEffectUsed;
            return false;
        }

        private ClientCard GetBestFlipDownTarget(List<ClientCard> oppMonsters)
        {
            if (oppMonsters == null || oppMonsters.Count == 0) return null;

            return oppMonsters
                .Where(c => c != null && c.IsFaceup())
                .OrderByDescending(c => {
                    int score = 0;
                    if (c.EquipCards != null && c.EquipCards.Count > 0) score += 3000;
                    if (c.IsCode(CardId.Apollousa, CardId.BaronneDeFleur, CardId.CyberDragonInfinity))
                        score += 2500;
                    if (c.IsCode(CardId.ABCDragonBuster, CardId.CrusadiaAvramax, CardId.KnightmareUnicorn))
                        score += 2000;
                    if (c.HasType(CardType.Link)) score += 1500;
                    if (c.HasType(CardType.Xyz)) score += 1200;
                    if (c.HasType(CardType.Synchro)) score += 1000;
                    if (c.HasType(CardType.Fusion)) score += 800;
                    if (!c.IsDisabled() && c.HasType(CardType.Effect)) score += 500;
                    score += c.Attack / 2;
                    return score;
                })
                .FirstOrDefault();
        }

        private bool EnneacraftReleaseEffect()
        {
            if (_releaseUsed) return false;

            if (Card.Location == CardLocation.Hand)
            {
                var left = Util.GetPZone(0, 0);
                var right = Util.GetPZone(0, 1);
                if (left == null || right == null)
                {
                    _releaseUsed = true;
                    DecisionTracer.TraceActivate("EnneacraftReleaseEffect", "Activating Release from hand to set scale");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                var edPendulums = Bot.ExtraDeck.Where(c => c != null && c.IsFaceup() && c.IsMonster()).ToList();
                if (edPendulums.Count > 0)
                {
                    AI.SelectCard(edPendulums.FirstOrDefault());
                    DecisionTracer.TraceActivate("EnneacraftReleaseEffect", "GY Banish recovery");
                    return true;
                }
            }

            return false;
        }

        private bool EnneacraftReverthEffect()
        {
            if (_reverthUsed) return false;

            if (Card.Location == CardLocation.Hand)
            {
                int bossInHand = Bot.Hand.Count(c => c != null && LightEnneacrafts.Contains(c.Id));
                if (bossInHand >= 2 && Bot.Hand.Count >= 4)
                {
                    var toShuffle = Bot.Hand.Where(c => c != null && LightEnneacrafts.Contains(c.Id)).Take(bossInHand - 1).ToList();
                    AI.SelectCard(toShuffle);
                    _reverthUsed = true;
                    DecisionTracer.TraceActivate("EnneacraftReverthEffect", "Shuffling redundant hand cards");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                var setMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFacedown() && EnneacraftMonsters.Contains(c.Id)).ToList();
                if (setMonsters.Count > 0)
                {
                    if (Duel.Player == 1)
                    {
                        bool isThreatSummoned = false;
                        int[] threatBosses = { 
                            CardId.ABCDragonBuster, 
                            CardId.CyberDragonInfinity, 
                            CardId.Apollousa, 
                            CardId.UnionCarrier, 
                            CardId.IPMasquerena, 
                            CardId.CrusadiaAvramax, 
                            CardId.KnightmareUnicorn 
                        };

                        foreach (var c in Duel.LastSummonedCards)
                        {
                            if (c != null && c.Controller == 1)
                            {
                                if (MachineUnions.Contains(c.Id) || threatBosses.Contains(c.Id) || c.Attack >= 2000)
                                {
                                    isThreatSummoned = true;
                                    break;
                                }
                            }
                        }

                        if (!isThreatSummoned)
                        {
                            DecisionTracer.TraceSkip("EnneacraftReverthEffect", "Opponent summoned a non-threat monster, saving Reverth");
                            return false;
                        }
                    }

                    ClientCard target = PickBestFlipTarget(setMonsters);

                    AI.SelectOption(1);
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("EnneacraftReverthEffect", $"GY Banish to flip {target.Name} face-up");
                    return true;
                }

                var handMonsters = Bot.Hand.Where(c => c != null && c.IsMonster()).ToList();
                if (handMonsters.Count > 0)
                {
                    var bestSS = handMonsters.FirstOrDefault(c => LightEnneacrafts.Contains(c.Id))
                        ?? handMonsters.FirstOrDefault();
                    AI.SelectOption(0);
                    AI.SelectCard(bestSS);
                    DecisionTracer.TraceActivate("EnneacraftReverthEffect", $"GY Banish to Special Summon {bestSS.Name} face-down");
                    return true;
                }
            }

            return false;
        }

        private bool EnneacraftResetEffect()
        {
            if (_resetUsed) return false;

            if (Card.Location == CardLocation.Hand)
            {
                _resetUsed = true;
                DecisionTracer.TraceActivate("EnneacraftResetEffect", "Activating Reset from hand");
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                var faceUpUsedMonsters = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && EnneacraftMonsters.Contains(c.Id) && HasUsedFlipEffect(c))
                    .ToList();
                var faceUpMonsters = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && EnneacraftMonsters.Contains(c.Id))
                    .ToList();

                if (faceUpUsedMonsters.Count > 0 && Duel.LastChainPlayer == 1)
                {
                    AI.SelectCard(faceUpUsedMonsters);
                    DecisionTracer.TraceActivate("EnneacraftResetEffect", $"GY Banish to flip {faceUpUsedMonsters.Count} used-FLIP monsters face-down (reset)");
                    return true;
                }

                if (Duel.Phase == DuelPhase.End && faceUpUsedMonsters.Count > 0)
                {
                    AI.SelectCard(faceUpUsedMonsters);
                    DecisionTracer.TraceActivate("EnneacraftResetEffect", $"End Phase — resetting {faceUpUsedMonsters.Count} face-up monsters for next turn");
                    return true;
                }

                if (faceUpMonsters.Count > 0 && Duel.LastChainPlayer == 1)
                {
                    var lastChain = Util.GetLastChainCard();
                    if (lastChain != null && lastChain.Controller == 1)
                    {
                        int threat = EvaluateChainThreat(lastChain);
                        if (threat >= 80)
                        {
                            AI.SelectCard(faceUpMonsters);
                            DecisionTracer.TraceActivate("EnneacraftResetEffect", "Emergency dodge — flipping monsters face-down to avoid destruction");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool EnneapolisEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                DecisionTracer.TraceActivate("EnneapolisEffect", "Activating Enneapolis Field Spell");
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                var faceUpEnneacrafts = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && LightEnneacrafts.Contains(c.Id)).ToList();
                if (faceUpEnneacrafts.Count > 0)
                {
                    AI.SelectCard(faceUpEnneacrafts.FirstOrDefault());
                    DecisionTracer.TraceActivate("EnneapolisEffect", "Bouncing face-up Enneacraft to hand for re-SS");
                    return true;
                }
            }

            return false;
        }

        // ============================================================
        // TIER 6: Equips (TromarIA, PIXEA, SPIA)
        // ============================================================

        private ClientCard GetBestEquipTarget()
        {
            var fieldMonsters = Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && EnneacraftMonsters.Contains(c.Id))
                .ToList();
            if (fieldMonsters.Count == 0) return null;

            var boss = fieldMonsters.FirstOrDefault(c => LightEnneacrafts.Contains(c.Id));
            return boss ?? fieldMonsters.FirstOrDefault();
        }

        private bool TromarIAEffect()
        {
            if (_tromarIAEffectUsed) return false;

            if (Card.Location == CardLocation.Hand)
            {
                var target = GetBestEquipTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("TromarIAEffect", $"Equipping to {target.Name}");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && EnneacraftMonsters.Contains(c.Id));
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("TromarIAEffect", $"FLIP — Special Summon {target.Name} face-down from GY");
                    return true;
                }
            }
            return false;
        }

        private bool AstaPixeaEffect()
        {
            if (_astaPixeaEffectUsed) return false;

            if (Card.Location == CardLocation.Hand)
            {
                var target = GetBestEquipTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("AstaPixeaEffect", $"Equipping to {target.Name} (double attack)");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target = Util.GetBestEnemyCard(canBeTarget: true);
                if (target != null && IsViableEffectTarget(target))
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("AstaPixeaEffect", $"FLIP — destroy card {target.Name}");
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        // TIER 7: Low Level Normal Summons & Hand Special Summons
        // ============================================================

        private bool LowLevelSummon()
        {
            bool haveZaborg = Bot.HasInHand(CardId.ZaborgTheMegaMonarch);
            if (haveZaborg && Bot.GetMonsterCount() < 2)
                return true;

            return Bot.GetMonsterCount() < 2;
        }

        private bool EnneacraftHandEffect(int callerCardId)
        {
            if (IsSpecialSummonBlocked())
            {
                DecisionTracer.TraceSkip("EnneacraftHandEffect", "Special Summon is blocked by floodgate");
                return false;
            }

            bool haveZaborg = Bot.HasInHand(CardId.ZaborgTheMegaMonarch);
            int monsterCount = Bot.GetMonsterCount();

            // 1. Zaborg setup — need tribute fodder
            if (haveZaborg && monsterCount < 2)
            {
                if (LightEnneacrafts.Contains(callerCardId))
                {
                    DecisionTracer.TraceActivate("EnneacraftHandEffect", "SS LIGHT monster face-down for Zaborg tribute");
                    return true;
                }
                if (monsterCount == 0)
                {
                    DecisionTracer.TraceActivate("EnneacraftHandEffect", "SS face-down for Zaborg tribute (any)");
                    return true;
                }
            }

            // 2. Going first: set up disruption board
            if (Duel.Turn == 1)
            {
                if (LightEnneacrafts.Contains(callerCardId) && monsterCount < 3)
                {
                    DecisionTracer.TraceActivate("EnneacraftHandEffect", "Going 1st — SS boss face-down for disruption");
                    return true;
                }
                if (Level1Enneacrafts.Contains(callerCardId) && monsterCount < 2)
                {
                    DecisionTracer.TraceActivate("EnneacraftHandEffect", "Going 1st — SS Level 1 face-down for flip trap");
                    return true;
                }
            }

            // 3. Going second: aggressive setup
            if (_isGoingSecond && Duel.Phase == DuelPhase.Main1)
            {
                if (LightEnneacrafts.Contains(callerCardId) && monsterCount < 3)
                {
                    DecisionTracer.TraceActivate("EnneacraftHandEffect", "Going 2nd — SS boss for OTK setup");
                    return true;
                }
            }

            // 4. MP2 defensive setup
            if (Duel.Phase == DuelPhase.Main2 && monsterCount < 3)
            {
                DecisionTracer.TraceActivate("EnneacraftHandEffect", "MP2 defensive set");
                return true;
            }

            // 5. Low board presence fallback
            if (NeedsBoardPresence())
            {
                DecisionTracer.TraceActivate("EnneacraftHandEffect", "Low board presence fallback");
                return true;
            }

            return false;
        }

        private ClientCard PickBestFlipTarget(List<ClientCard> setMonsters)
        {
            ClientCard target = null;

            if (Enemy.GetMonsterCount() >= 1)
            {
                target = setMonsters.FirstOrDefault(c => c.IsCode(CardId.EnneacraftAtoriMAR) && !_atoriMarEffectUsed);
            }

            if (target == null && Enemy.GetMonsterCount() > 0)
            {
                target = setMonsters.FirstOrDefault(c => c.IsCode(CardId.ProtoEnneacraftOrgIA) && !_protoEffectUsed);
            }

            if (target == null && Enemy.GetSpells().Count > 0)
            {
                target = setMonsters.FirstOrDefault(c => c.IsCode(CardId.TritoEnneacraftExapatisIA) && !_tritoEffectUsed);
            }

            if (target == null && Enemy.GetMonsterCount() > 0)
            {
                target = setMonsters.FirstOrDefault(c => c.IsCode(CardId.EnneacraftAtoriMAR) && !_atoriMarEffectUsed);
            }

            if (target == null && Enemy.Hand.Count > 0)
            {
                target = setMonsters.FirstOrDefault(c => c.IsCode(CardId.DefteroEnneacraftAlazoneIA) && !_defteroEffectUsed);
            }

            if (target == null && (Enemy.Graveyard.Count > 0 || Enemy.Banished.Count > 0))
            {
                target = setMonsters.FirstOrDefault(c => c.IsCode(CardId.EnatoEnneacraftOknirIA) && !_enatoEffectUsed);
            }

            if (target == null)
            {
                target = setMonsters.FirstOrDefault();
            }

            return target;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(
                CardId.EnneacraftAtoriMAR,
                CardId.EnneacraftAizaLEON,
                CardId.EnneacraftArchaTAIL,
                CardId.EnneacraftAtilSPIA,
                CardId.EnneacraftAstaPIXEA
            );
        }

        protected override bool ShouldStopExtending()
        {
            int faceDownEnneacrafts = Bot.GetMonsters().Count(c => c != null && c.IsFacedown() && EnneacraftMonsters.Contains(c.Id));
            if (faceDownEnneacrafts >= 3)
                return true;
            bool hasAtorifaceDown = Bot.GetMonsters().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.EnneacraftAtoriMAR));
            if (hasAtorifaceDown && faceDownEnneacrafts >= 2)
                return true;
            return false;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (EnneacraftMonsters.Contains(c.Id)) return 50;
            return 100;
        }

        // ============================================================
        // Custom Monster Reposition
        // ============================================================

        private bool EnneacraftMonsterRepos()
        {
            if (Card == null) return false;
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;

            if (EnneacraftMonsters.Contains(Card.Id))
            {
                if (Card.IsAttack() && Card.Attack < 1000)
                {
                    if (enemyEmpty && Card.Attack >= 2000)
                        return false;
                    return true;
                }

                if (Card.IsDefense() && enemyEmpty && Card.Attack >= 1500)
                {
                    return true;
                }

                return false;
            }

            return DefaultMonsterRepos();
        }

        protected override bool NeedsBoardPresence()
        {
            int ourFaceup = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());
            int ourFacedown = Bot.GetMonsters().Count(c => c != null && c.IsFacedown() && EnneacraftMonsters.Contains(c.Id));
            int totalPresence = ourFaceup + ourFacedown;

            return totalPresence == 0 || (totalPresence <= 1 && Enemy.GetMonsterCount() >= 2);
        }

        protected override bool IsBoardStrongEnough()
        {
            return GetExpectedInteractionScore() >= 3.5;
        }

        // ============================================================
        // Hint Card Selection — OnSelectCard
        // ============================================================

        public override IList<ClientCard> OnSelectCard(
            IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Check if Zaborg's effect triggered this selection
            bool isZaborgGYSend = false;
            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.IsCode(CardId.ZaborgTheMegaMonarch))
            {
                isZaborgGYSend = true;
            }

            if (isZaborgGYSend)
            {
                List<ClientCard> selected = new List<ClientCard>();

                // Select from our Extra Deck
                var ourExtra = cards.Where(c => c.Controller == 0 && c.Location == CardLocation.Extra).ToList();
                if (ourExtra.Count > 0)
                {
                    var priorityList = new List<int>
                    {
                        CardId.JurracAstero,
                        CardId.JurracGiganoto,
                        CardId.TriBrigadeFerrijitTheBarrenBlossom,
                        CardId.TriBrigadeArmsMouser,
                        CardId.GoldenCloudBeastMalong,
                        CardId.FossilWarriorSkullKnight,
                        CardId.FossilMachineSkullWagon,
                        CardId.WindPegasusIgnister,
                        CardId.JurracMeteor
                    };

                    foreach (int id in priorityList)
                    {
                        if (id == CardId.JurracMeteor)
                        {
                            // Never send Jurrac Meteor if it would leave us with 0 copies of Jurrac Meteor in the Extra Deck
                            int remainingMeteor = ourExtra.Count(c => c.IsCode(CardId.JurracMeteor) && !selected.Contains(c));
                            if (remainingMeteor <= 1) continue;
                        }

                        var match = ourExtra.FirstOrDefault(c => c.IsCode(id) && !selected.Contains(c));
                        if (match != null)
                        {
                            selected.Add(match);
                            if (selected.Count >= max) break;
                        }
                    }

                    if (selected.Count < min)
                    {
                        foreach (var c in ourExtra)
                        {
                            if (!selected.Contains(c))
                            {
                                selected.Add(c);
                                if (selected.Count >= min) break;
                            }
                        }
                    }

                    if (selected.Count < min)
                    {
                        foreach (var c in cards)
                        {
                            if (!selected.Contains(c))
                            {
                                selected.Add(c);
                                if (selected.Count >= min) break;
                            }
                        }
                    }

                    DecisionTracer.Trace("OnSelectCard", $"Zaborg: Selected {selected.Count} cards from our Extra Deck");
                    return selected;
                }

                // Select from opponent's Extra Deck
                var oppExtra = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.Extra).ToList();
                if (oppExtra.Count > 0)
                {
                    var sortedOpp = oppExtra
                        .OrderByDescending(c =>
                        {
                            if (c.HasType(CardType.Fusion)) return 4;
                            if (c.HasType(CardType.Synchro)) return 3;
                            if (c.HasType(CardType.Xyz)) return 2;
                            if (c.HasType(CardType.Link)) return 1;
                            return 0;
                        })
                        .ThenByDescending(c => c.Level)
                        .ThenByDescending(c => c.Attack)
                        .ToList();

                    foreach (var c in sortedOpp)
                    {
                        selected.Add(c);
                        if (selected.Count >= max) break;
                    }

                    if (selected.Count < min)
                    {
                        foreach (var c in oppExtra)
                        {
                            if (!selected.Contains(c))
                            {
                                selected.Add(c);
                                if (selected.Count >= min) break;
                            }
                        }
                    }

                    if (selected.Count < min)
                    {
                        foreach (var c in cards)
                        {
                            if (!selected.Contains(c))
                            {
                                selected.Add(c);
                                if (selected.Count >= min) break;
                            }
                        }
                    }

                    DecisionTracer.Trace("OnSelectCard", $"Zaborg: Selected {selected.Count} cards from opponent's Extra Deck");
                    return selected;
                }
            }

            // Location Filtering on hint 509 (Special Summon) to prevent crashes
            if (hint == 509)
            {
                bool hasDeck = cards.Any(c => c.Location == CardLocation.Deck);
                if (hasDeck && cards.Any(c => c.Location == CardLocation.Hand))
                {
                    var targets = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    if (targets.Count >= min)
                    {
                        DecisionTracer.Trace("OnSelectCard", "Hint 509 — filtering for Deck location");
                        return targets;
                    }
                }
            }

            // Search Spells (hint 506)
            if (hint == 506)
            {
                List<ClientCard> selected = new List<ClientCard>();
                var deckEnneacrafts = cards.Where(c => c.Location == CardLocation.Deck && (EnneacraftMonsters.Contains(c.Id) || EnneacraftSpells.Contains(c.Id))).ToList();
                if (deckEnneacrafts.Count > 0)
                {
                    var handIds = new HashSet<int>(Bot.Hand.Where(c => c != null).Select(c => c.Id));
                    var prioritySearch = new List<int>();

                    if (Duel.Turn <= 2 && !_isGoingSecond)
                    {
                        if (!handIds.Contains(CardId.EnneacraftRelease))
                            prioritySearch.Add(CardId.EnneacraftRelease);
                        if (!handIds.Contains(CardId.EnneacraftAtoriMAR))
                            prioritySearch.Add(CardId.EnneacraftAtoriMAR);
                        if (!handIds.Contains(CardId.EnneacraftAizaLEON))
                            prioritySearch.Add(CardId.EnneacraftAizaLEON);
                        if (!handIds.Contains(CardId.ProtoEnneacraftOrgIA))
                            prioritySearch.Add(CardId.ProtoEnneacraftOrgIA);
                        if (!handIds.Contains(CardId.EnneacraftReset))
                            prioritySearch.Add(CardId.EnneacraftReset);
                        if (!handIds.Contains(CardId.EnneacraftReverth))
                            prioritySearch.Add(CardId.EnneacraftReverth);
                    }
                    else
                    {
                        if (!handIds.Contains(CardId.EnneacraftAtoriMAR))
                            prioritySearch.Add(CardId.EnneacraftAtoriMAR);
                        if (!handIds.Contains(CardId.EnneacraftRelease))
                            prioritySearch.Add(CardId.EnneacraftRelease);
                        if (!handIds.Contains(CardId.ProtoEnneacraftOrgIA))
                            prioritySearch.Add(CardId.ProtoEnneacraftOrgIA);
                        if (!handIds.Contains(CardId.EnneacraftAizaLEON))
                            prioritySearch.Add(CardId.EnneacraftAizaLEON);
                        if (!handIds.Contains(CardId.EnneacraftAstaPIXEA))
                            prioritySearch.Add(CardId.EnneacraftAstaPIXEA);
                        if (!handIds.Contains(CardId.DefteroEnneacraftAlazoneIA))
                            prioritySearch.Add(CardId.DefteroEnneacraftAlazoneIA);
                        if (!handIds.Contains(CardId.EnneacraftReset))
                            prioritySearch.Add(CardId.EnneacraftReset);
                        if (!handIds.Contains(CardId.EnneacraftReverth))
                            prioritySearch.Add(CardId.EnneacraftReverth);
                    }

                    var fallbacks = new[] {
                        CardId.EnneacraftRelease,
                        CardId.EnneacraftAtoriMAR,
                        CardId.ProtoEnneacraftOrgIA,
                        CardId.EnneacraftAizaLEON,
                        CardId.DefteroEnneacraftAlazoneIA,
                        CardId.EnneacraftReset,
                        CardId.EnneacraftReverth
                    };
                    foreach (int id in fallbacks)
                    {
                        if (!prioritySearch.Contains(id))
                            prioritySearch.Add(id);
                    }

                    foreach (int id in prioritySearch)
                    {
                        var match = deckEnneacrafts.FirstOrDefault(c => c.IsCode(id));
                        if (match != null)
                        {
                            selected.Add(match);
                            if (selected.Count >= max) break;
                        }
                    }

                    if (selected.Count < min)
                    {
                        foreach (var c in deckEnneacrafts)
                        {
                            if (!selected.Contains(c))
                            {
                                selected.Add(c);
                                if (selected.Count >= min) break;
                            }
                        }
                    }

                    if (selected.Count < min)
                    {
                        foreach (var c in cards)
                        {
                            if (!selected.Contains(c))
                            {
                                selected.Add(c);
                                if (selected.Count >= min) break;
                            }
                        }
                    }

                    return selected;
                }
            }

            if (Card == null)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (EnneacraftMonsters.Contains(cardId))
            {
                if (positions.Contains(CardPosition.FaceDownDefence))
                    return CardPosition.FaceDownDefence;
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
        }
    }
}
