// =========================================================================================
// CARD AUDIT โ€” 2026_Clown (Mitsurugi & Clown Crew Hybrid Engine)
// | Card Name                          | Type       | OPT? | Cost            | Effect Summary                                                        | Activate When                              | NEVER When                            |
// | :--------------------------------- | :--------: | :--: | :-------------: | :-------------------------------------------------------------------: | :----------------------------------------: | :-----------------------------------: |
// | Clown Crew Biancaviso (82159583)   | Ritual/Mon | HOPT | None / Tribute  | Lv7 2500/2500 Spellcaster. Negate face-up card or Draw 1. Can Tribute Summon | Needs negation/draw or tribute summon boss | Target is already negated             |
// | Clown Crew Flair (42759961)        | Monster    | HOPT | None            | Lv1 500/1000 Spellcaster. Add Clown spell/trap, or flip monster face-down  | Combo setup / face-down disruption        | Skip combo active                     |
// | Futsu no Mitama no Mitsurugi (55397172)| Ritual/Mon | HOPT | None        | Level 8 Reptile. SS Reptile from GY; on tribute/sent add Ritual S/T   | GY revival & Ritual recursion             | SS blocked or board full              |
// | Ame no Murakumo no Mitsurugi (19899073)| Ritual/Mon | HOPT | Tribute Reptile | Level 8 Reptile. Quick pop 2 opp cards; on tribute/sent add Ritual S/T| Disrupt opp cards / removal               | No opp targets or no tribute          |
// | Ame no Habakiri no Mitsurugi (13332685)| Ritual/Mon | HOPT | Tribute Reptile | Lv8 2400/1800 Reptile. SS from hand; on tribute add S/T              | Beatdown / Lethal push / Board presence   | SS blocked or board full              |
// | Mitsurugi no Mikoto Saji (18176525)| Monster    | HOPT | None            | Level 4 Reptile. When tributed: Search Mitsurugi Spell/Trap           | Tributed for ritual / spell cost           | No Mitsurugi S/T left in deck         |
// | Mitsurugi no Mikoto Aramasa (40543231)| Monster  | HOPT | None            | Level 4 Reptile. When tributed: Search Mitsurugi Reptile monster      | Tributed for ritual / spell cost           | No Reptile left in deck               |
// | Mitsurugi no Mikoto Kusanagi (82782870)| Monster | HOPT | None            | Level 4 Reptile. When tributed: Add Mitsurugi Spell or Ritual monster | Tributed for ritual / spell cost           | No targets in deck                    |
// | Mitsurugi no Miko Wousu (76948970) | Monster    | HOPT | None            | Lv8 2000/2000 Reptile. SS by revealing Reptile; in GY add Ritual Spell| Free body / GY Ritual Spell recovery       | Non-Reptiles on field or SS blocked   |
// | Fydraulis Harmonia (70088809)      | Monster    | HOPT | Discard Hand    | Lv7 2500/2000 Handtrap. Opp monster effect -> Send 3 Synchros from ED to GY | Opp monster activates effect in chain      | GY empty or no Synchros in ED         |
// | Dimension Shifter (91800273)       | Monster    | HOPT | Send to GY      | Handtrap. Banish all cards sent to GY for 2 turns                     | Turn 1-2 when GY is empty                  | Bot has cards in GY                   |
// | Clown Crew Rehearsal (20448151)    | Spell      | HOPT | Tribute 1       | Add 1 Clown Crew Monster + 1 Clown Crew S/T; GY: recover Flair        | Search core engine pieces                  | No tribute monster available          |
// | Clown Crew Matinee Operatics (57847269)| Cont Spell | HOPT | None          | Place in S/T zone; Special Summon Clown Crew monster from GY          | Continuous GY revival & board presence    | Monster zone is full                  |
// | Clown Crew Malabarisme (83232904)  | Q-Play Spell| HOPT | Tribute 1       | SS Clown Crew from Deck/GY; GY: Quick Tribute & pop opp card          | Extender / Quick disruption during opp turn| No targets or Ace would be sacrificed |
// | Mitsurugi Prayers (45171524)       | Spell      | HOPT | Tribute Reptile | Search 1 Mitsurugi Ritual + 1 Mitsurugi non-Ritual monster            | Primary combo starter                      | No Reptiles to tribute                |
// | Mitsurugi Ritual (81560239)        | Ritual Spell| HOPT| Tribute Reptile | Ritual Summon from Hand or Deck by tributing Reptiles                 | Core Ritual Summon enabler                 | No valid Ritual target                |
// | Mitsurugi Mirror (49721684)        | Ritual Spell| HOPT| Tribute Reptile | Ritual Summon from Hand/GY; GY: banish to add Mitsurugi Ritual        | Secondary Ritual enabler & GY recursion    | No valid Ritual target                |
// | Pre-Preparation of Rites (13048472)| Spell      | HOPT | None            | Search Mitsurugi Mirror + Ame no Murakumo / Futsu no Mitama           | 1st Action Bait / Combo Starter            | No targets remaining in deck          |
// | Super Polymerization (48130397)    | Q-Play Spell| No   | Discard 1 card  | Fuse using monsters on either field into Diabolo / Garura / Theseus   | Opponent has 2+ fusible monsters           | CanDealLethal without it / no mats    |
// | Forbidden Droplet (24299458)       | Q-Play Spell| No   | Send cards      | Negate opp monsters & halve ATK without triggering responses          | Opp has problematic negates/monsters       | Target is already negated             |
// | Forbidden Crown (98829635)         | Q-Play Spell| HOPT | Target monster  | Negate monster effect & prevent activation                            | Chain to high-threat monster activation    | Monster already negated               |
// | Mitsurugi Great Purification (17954937)| Cont Trap| HOPT | Tribute Lv5+ Reptile| Banish 1 card on field; GY: Tribute & Ritual Summon             | Quick disruption / GY Ritual surprise      | No Lv5+ Reptile to tribute            |
// | Clown Crew Soiree Operations (70058649)| Cont Trap| HOPT| None           | SS Clown Crew from GY/Banished; GY: Tribute Summon Biancaviso        | Disruption / Follow-up / Boss revival      | SS blocked or no targets              |
// | Clown Crew Meteor (93172951)       | Synchro/Mon| HOPT | None            | Lv6 600/1300 Synchro. Quick pop 1 card; GY: Add Clown card to hand    | Boss disruption & GY recursion             | No opp targets                        |
// | Number 23: Lancelot (66547759)     | Xyz/Mon    | OPT  | Detach 1        | Rank 8 Xyz. Direct attack (2000 ATK); Mandatory negate card activation| Going 1st Omni-negate / Lethal direct poke | Self-board harm if forced             |
// | Clown Crew Diabolo (31533473)      | Fusion/Mon | HOPT | 2 Same Attr     | Lv5 200/1800 Fusion. Quick pop 1 card; GY: Recycle Fusions to ED      | Super Poly target / Quick Board Disruption | No opp targets                        |
// | Clown Crew Fiends (91237821)       | Fusion/Mon | HOPT | 2 Spellcasters  | Lv3 0/2000 Fusion. Sent to GY: Set 1 Clown Crew Spell from Deck       | Super Poly target / GY engine searcher     | No spells in deck                     |
// | Golden Cloud Beast - Malong (93125329)| Synchro/Mon| HOPT| Sent to GY     | Level 6 Synchro. Sent to GY: Bounce 1 face-up card opp controls       | Sent via Fydraulis Harmonia / Synchro play | No opp face-up cards                  |
// | Wind Pegasus @Ignister (98506199)  | Synchro/Mon| HOPT | Card destroyed  | Level 7 Synchro. In GY when our card destroyed: Shuffle 1 opp card   | Passive GY disruption via Fydraulis        | No opp cards on field/GY              |
// | S:P Little Knight (29301450)       | Link/Mon   | HOPT | 2 Effect Mons   | Link-2. Banish 1 card on field/GY on summon; Quick banish 2 face-up  | Removal / Dodging removal / Disruption     | Would consume Ace cards as material   |
// =========================================================================================
// ACE CARDS:
// Primary Ace: Ame no Murakumo no Mitsurugi, Clown Crew Biancaviso, Clown Crew Meteor
// Secondary Ace: Futsu no Mitama no Mitsurugi, Ame no Habakiri no Mitsurugi, Clown Crew Diabolo, Number 23: Lancelot
// Critical Resource: Clown Crew Flair, Mitsurugi Prayers, Pre-Preparation of Rites, S:P Little Knight
// =========================================================================================
// WIN CONDITION:
// 1. Going 1st: Ame no Murakumo (Pop 2) + Biancaviso (Negate) + Meteor (Pop 1) + Set Purification/Soiree + Handtraps.
// 2. Going 2nd: Board Break with Super Poly / Droplet / Crown -> Ame no Habakiri (2400) + Biancaviso (2500) + Murakumo (3200) -> 8100+ OTK!
// =========================================================================================

// =========================================================================================
// COMBO DRAFT โ€” 2026_Clown
// =========================================================================================
// === COMBO LINE 1: Mitsurugi Ritual Primary Line (Pre-Prep / Prayers) ===
// HAND REQUIRED: Pre-Preparation of Rites or Mitsurugi Prayers + 1 Reptile
// STEP 1: Activate Pre-Prep -> Add Mitsurugi Mirror + Ame no Murakumo no Mitsurugi
// STEP 2: Activate Mitsurugi Prayers -> Tribute Reptile (e.g. Saji/Aramasa) -> Search Futsu no Mitama + Aramasa/Saji
// STEP 3: Trigger tributed Reptile effect -> Saji searches Mitsurugi Ritual / Purification; Aramasa searches Wousu
// STEP 4: Activate Mitsurugi Ritual -> Tribute Reptiles -> Ritual Summon Ame no Murakumo (Pop 2 on opp turn)
// STEP 5: Activate Futsu no Mitama -> Revive tributed Reptile for board presence / Link material
// END BOARD: Ame no Murakumo + Futsu no Mitama + Set Great Purification + Set Quick-Plays
//
// === COMBO LINE 2: Clown Crew Hybrid Setup (Flair + Rehearsal) ===
// HAND REQUIRED: Clown Crew Flair + Clown Crew Rehearsal
// STEP 1: Normal Summon Clown Crew Flair -> Add Clown Crew Soiree Operations / Matinee
// STEP 2: Activate Clown Crew Rehearsal -> Tribute 1 monster -> Add Biancaviso + Matinee Operatics
// STEP 3: Tribute Summon / Ritual Summon Clown Crew Biancaviso
// STEP 4: Set Soiree Operations & Quick-Plays -> Pass
// END BOARD: Biancaviso (Negate on field) + Matinee (GY revive) + Soiree Operations + Backrow
//
// === COMBO LINE 3: Going 2nd Board Break & 8600+ Damage OTK ===
// HAND REQUIRED: Super Polymerization / Forbidden Droplet + Ritual Starters
// STEP 1: Clear enemy omni-negates / board threats with Super Poly (Fuse into Diabolo / Garura) or Droplet
// STEP 2: Activate Mitsurugi Ritual / Prayers -> SS Ame no Habakiri (3000 ATK) + Ame no Murakumo (pop remaining backrow)
// STEP 3: Special Summon Biancaviso (2800 ATK) or Synchro into Clown Crew Meteor (2800 ATK)
// STEP 4: Battle Phase: Attack with Diabolo (2800) + Habakiri (3000) + Biancaviso (2800) = 8600 Damage -> OTK!
//
// === COMBO LINE 4: Fallback / Grind Recovery ===
// STEP 1: Use Mitsurugi Mirror GY effect -> Banish Mirror to add Ritual Monster
// STEP 2: Use Wousu in GY -> Add Ritual Spell to hand
// STEP 3: Re-establish Ame no Murakumo or Biancaviso with minimum card investment
// =========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Clown", "2026_Clown")]
    public class _2026_ClownExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int ClownCrewBiancaviso = 82159583;
            public const int ClownCrewFlair = 42759961;
            public const int FutsuNoMitamaNoMitsurugi = 55397172;
            public const int AmeNoMurakumoNoMitsurugi = 19899073;
            public const int AmeNoHabakiriNoMitsurugi = 13332685;
            public const int MitsurugiNoMikotoSaji = 18176525;
            public const int MitsurugiNoMikotoKusanagi = 82782870;
            public const int MitsurugiNoMikotoAramasa = 40543231;
            public const int MitsurugiNoMikoWousu = 76948970;
            public const int FydraulisHarmonia = 70088809;
            public const int DimensionShifter = 91800273;

            // Spells
            public const int ClownCrewMatineeOperatics = 57847269;
            public const int ClownCrewRehearsal = 20448151;
            public const int ClownCrewMalabarisme = 83232904;
            public const int MitsurugiPrayers = 45171524;
            public const int MitsurugiRitual = 81560239;
            public const int MitsurugiMirror = 49721684;
            public const int PrePreparationOfRites = 13048472;
            public const int TripleTacticsTalent = 25311006;
            public const int ForbiddenDroplet = 24299458;
            public const int ForbiddenCrown = 98829635;
            public const int SuperPolymerization = 48130397;
            public const int CalledByTheGrave = 24224830;

            // Traps
            public const int MitsurugiGreatPurification = 17954937;
            public const int ClownCrewSoireeOperations = 70058649;

            // Extra Deck
            public const int ClownCrewMeteor = 93172951;
            public const int GoldenCloudBeastMalong = 93125329;
            public const int WindPegasusIgnister = 98506199;
            public const int Number23Lancelot = 66547759;
            public const int ClownCrewFiends = 91237821;
            public const int ClownCrewDiabolo = 31533473;
            public const int TheDragonThatDevoursTheDogma = 76666602;
            public const int SeaMonsterOfTheseus = 96334243;
            public const int PanzerDragon = 72959823;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int SPLittleKnight = 29301450;

            // Side Deck Staples
            public const int MulcharmyFuwalos = 42141493;
            public const int GhostOgreAndSnowRabbit = 33854624;
            public const int HarpiesFeatherDuster = 18144507;
            public const int LightningStorm = 14532163;
            public const int CosmicCyclone = 54693926;
            public const int AntiSpellFragrance = 58921041;
            public const int EvenlyMatched = 15693423;
            public const int SolemnJudgment = 41420027;
            public const int SolemnStrike = 78114463;
            public const int DifferentDimensionGround = 31849106;
            public const int DrollAndLockBird = 94145021;
            public const int NibiruThePrimalBeing = 27204311;
            public const int DimensionalBarrier = 83326048;
            public const int RedReboot = 23002292;
            public const int SummonLimit = 22888900;
        }

        private static readonly int[] AceCardIds = {
            CardId.ClownCrewBiancaviso,
            CardId.ClownCrewMeteor,
            CardId.ClownCrewDiabolo,
            CardId.FutsuNoMitamaNoMitsurugi,
            CardId.AmeNoMurakumoNoMitsurugi,
            CardId.AmeNoHabakiriNoMitsurugi,
            CardId.Number23Lancelot,
            CardId.SPLittleKnight
        };

        private static readonly int[] BestTributeTargets = {
            CardId.MitsurugiNoMikotoAramasa,
            CardId.MitsurugiNoMikotoSaji,
            CardId.MitsurugiNoMikotoKusanagi,
            CardId.ClownCrewFlair,
            CardId.ClownCrewFiends,
            CardId.MitsurugiNoMikoWousu
        };

        private static readonly int[] ReptileTributeTargets = {
            CardId.MitsurugiNoMikotoAramasa,
            CardId.MitsurugiNoMikotoSaji,
            CardId.MitsurugiNoMikotoKusanagi,
            CardId.MitsurugiNoMikoWousu
        };

        // Once Per Turn (OPT) tracking flags
        private bool _rehearsalUsed = false;
        private bool _rehearsalGyUsed = false;
        private bool _flairUsed = false;
        private bool _flairTributedUsed = false;
        private bool _prayersUsed = false;
        private bool _ritualUsed = false;
        private bool _mirrorUsed = false;
        private bool _mirrorGyUsed = false;
        private bool _fydraulisUsed = false;
        private bool _prePrepUsed = false;
        private bool _tttUsed = false;
        private bool _crownUsed = false;
        private bool _purificationUsed = false;
        private bool _purificationGyUsed = false;
        private bool _soireeUsed = false;
        private bool _soireeGyUsed = false;
        private bool _malabarismeUsed = false;
        private bool _malabarismeGyUsed = false;
        private bool _matineeUsed = false;
        private bool _biancavisoUsed = false;
        private bool _meteorUsed = false;
        private bool _diaboloUsed = false;
        private bool _habakiriUsed = false;
        private bool _wousuSpUsed = false;

        public _2026_ClownExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards to protect them from being used suboptimally
            HeuristicGuard.RegisterAceCards(AceCardIds);
            ResourcePlan.RegisterAceCards(AceCardIds);

            // โ”€โ”€ Combo Router: Strategic Lines โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Mitsurugi-Ritual-Primary",
                RequiredCards = new List<int> { CardId.PrePreparationOfRites },
                FallbackLineName = "Mitsurugi-Prayers-Line",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.PrePreparationOfRites, ActionType = ExecutorType.Activate, Description = "Search Mitsurugi Mirror & Ame no Murakumo" },
                    new() { CardId = CardId.MitsurugiMirror, ActionType = ExecutorType.Activate, Description = "Ritual Summon Ame no Murakumo" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Mitsurugi-Prayers-Line",
                RequiredCards = new List<int> { CardId.MitsurugiPrayers },
                FallbackLineName = "Clown-Flair-Setup",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.MitsurugiPrayers, ActionType = ExecutorType.Activate, Description = "Tribute Reptile & Search Ritual + Monster" },
                    new() { CardId = CardId.MitsurugiRitual, ActionType = ExecutorType.Activate, Description = "Ritual Summon Ame no Murakumo or Futsu no Mitama" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Clown-Flair-Setup",
                RequiredCards = new List<int> { CardId.ClownCrewFlair, CardId.ClownCrewRehearsal },
                FallbackLineName = "Going2nd-SuperPoly-OTK",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ClownCrewFlair, ActionType = ExecutorType.Summon, Description = "Normal Summon Clown Crew Flair" },
                    new() { CardId = CardId.ClownCrewRehearsal, ActionType = ExecutorType.Activate, Description = "Tribute for Biancaviso & Matinee" },
                    new() { CardId = CardId.ClownCrewBiancaviso, ActionType = ExecutorType.Summon, Description = "Tribute Summon Biancaviso" }
                },
                EndBoardScore = 75
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Going2nd-SuperPoly-OTK",
                RequiredCards = new List<int> { CardId.SuperPolymerization },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.SuperPolymerization, ActionType = ExecutorType.Activate, Description = "Fuse opponent monsters into Diabolo / Garura" },
                    new() { CardId = CardId.AmeNoHabakiriNoMitsurugi, ActionType = ExecutorType.Activate, Description = "Special Summon 3000 ATK Habakiri for OTK" }
                },
                EndBoardScore = 95,
                Condition = () => Duel.Turn > 1 && Enemy.GetMonsterCount() >= 2
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.PrePreparationOfRites, CardId.MitsurugiPrayers, CardId.ClownCrewFlair);
            BaitPlanner.RegisterBaitCards(CardId.PrePreparationOfRites, CardId.TripleTacticsTalent);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.AmeNoMurakumoNoMitsurugi, CardId.ClownCrewBiancaviso, CardId.ClownCrewMeteor, CardId.ClownCrewDiabolo, CardId.SPLittleKnight);

            // ===== PRIORITY 1: Hand Traps, Negates & Board Breakers =====
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.DimensionShifter, DimensionShifterEffect);
            AddExecutor(ExecutorType.Activate, CardId.FydraulisHarmonia, FydraulisHarmoniaEffect);
            AddExecutor(ExecutorType.Activate, CardId.GoldenCloudBeastMalong, GoldenCloudBeastMalongEffect);
            AddExecutor(ExecutorType.Activate, CardId.WindPegasusIgnister, WindPegasusIgnisterEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);

            // ===== PRIORITY 2: Core Searchers & Starters =====
            AddExecutor(ExecutorType.Activate, CardId.PrePreparationOfRites, PrePreparationOfRitesEffect);
            AddExecutor(ExecutorType.Activate, CardId.MitsurugiPrayers, MitsurugiPrayersEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClownCrewFlair, ClownCrewFlairEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClownCrewRehearsal, ClownCrewRehearsalEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClownCrewMatineeOperatics, ClownCrewMatineeOperaticsEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClownCrewMalabarisme, ClownCrewMalabarismeEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClownCrewSoireeOperations, ClownCrewSoireeOperationsEffect);

            // ===== PRIORITY 3: Ritual Spells =====
            AddExecutor(ExecutorType.Activate, CardId.MitsurugiRitual, MitsurugiRitualEffect);
            AddExecutor(ExecutorType.Activate, CardId.MitsurugiMirror, MitsurugiMirrorEffect);

            // ===== PRIORITY 4: Mitsurugi Monster Search / GY Triggers =====
            AddExecutor(ExecutorType.Activate, CardId.MitsurugiNoMikotoSaji, MitsurugiNoMikotoSajiEffect);
            AddExecutor(ExecutorType.Activate, CardId.MitsurugiNoMikotoAramasa, MitsurugiNoMikotoAramasaEffect);
            AddExecutor(ExecutorType.Activate, CardId.MitsurugiNoMikotoKusanagi, MitsurugiNoMikotoKusanagiEffect);
            AddExecutor(ExecutorType.Activate, CardId.MitsurugiNoMikoWousu, MitsurugiNoMikoWousuEffect);
            AddExecutor(ExecutorType.Activate, CardId.FutsuNoMitamaNoMitsurugi, FutsuNoMitamaNoMitsurugiEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmeNoMurakumoNoMitsurugi, AmeNoMurakumoNoMitsurugiEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmeNoHabakiriNoMitsurugi, AmeNoHabakiriNoMitsurugiEffect);

            // ===== PRIORITY 5: Extra Deck & Boss Monster Activations =====
            AddExecutor(ExecutorType.Activate, CardId.Number23Lancelot, Number23LancelotEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClownCrewBiancaviso, ClownCrewBiancavisoEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClownCrewMeteor, ClownCrewMeteorEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClownCrewDiabolo, ClownCrewDiaboloEffect);
            AddExecutor(ExecutorType.Activate, CardId.ClownCrewFiends, ClownCrewFiendsEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheDragonThatDevoursTheDogma, TheDragonThatDevoursTheDogmaEffect);

            // ===== PRIORITY 6: Main Deck Summons =====
            AddExecutor(ExecutorType.SpSummon, CardId.MitsurugiNoMikoWousu, MitsurugiNoMikoWousuSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.ClownCrewFlair, ClownCrewFlairSummon);
            AddExecutor(ExecutorType.Summon, CardId.MitsurugiNoMikotoAramasa, MitsurugiNonTunerSummon);
            AddExecutor(ExecutorType.Summon, CardId.MitsurugiNoMikotoSaji, MitsurugiNonTunerSummon);
            AddExecutor(ExecutorType.Summon, CardId.MitsurugiNoMikotoKusanagi, MitsurugiNonTunerSummon);
            AddExecutor(ExecutorType.Summon, CardId.ClownCrewBiancaviso, ClownCrewBiancavisoSummon);

            // ===== PRIORITY 7: Extra Deck Summons =====
            AddExecutor(ExecutorType.SpSummon, CardId.Number23Lancelot, Number23LancelotSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ClownCrewMeteor, ClownCrewMeteorSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ClownCrewDiabolo, ClownCrewDiaboloSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ClownCrewFiends, ClownCrewFiendsSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TheDragonThatDevoursTheDogma, DogmaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GaruraWingsOfResonantLife);
            AddExecutor(ExecutorType.SpSummon, CardId.PanzerDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.SeaMonsterOfTheseus);

            // ===== PRIORITY 8: Traps & Backrow Set =====
            AddExecutor(ExecutorType.Activate, CardId.MitsurugiGreatPurification, MitsurugiGreatPurificationEffect);
            AddExecutor(ExecutorType.SpellSet, SpellSetFiltered);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Prefer going first to set up control & disruption board
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _rehearsalUsed = false;
            _rehearsalGyUsed = false;
            _flairUsed = false;
            _flairTributedUsed = false;
            _prayersUsed = false;
            _ritualUsed = false;
            _mirrorUsed = false;
            _mirrorGyUsed = false;
            _fydraulisUsed = false;
            _prePrepUsed = false;
            _tttUsed = false;
            _crownUsed = false;
            _purificationUsed = false;
            _purificationGyUsed = false;
            _soireeUsed = false;
            _soireeGyUsed = false;
            _malabarismeUsed = false;
            _malabarismeGyUsed = false;
            _matineeUsed = false;
            _biancavisoUsed = false;
            _meteorUsed = false;
            _diaboloUsed = false;
            _habakiriUsed = false;
            _wousuSpUsed = false;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c))
            {
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup())
                    return 950;
                return 800;
            }
            if (BestTributeTargets.Contains(c.Id)) return 50;
            return 200;
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            // Link summons (S:P Little Knight)
            if (card.HasType(CardType.Link))
            {
                int reqRating = card.LinkCount;
                if (!IsMaterialSelectionSafeForLink(reqRating))
                {
                    if (CanDealLethal() || OpponentHasActiveNegator()) return true;
                    DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} skipped: would consume active Ace card(s)");
                    return false;
                }
            }

            // Synchro Summons (ClownCrewMeteor)
            if (card.HasType(CardType.Synchro))
            {
                if (!CanSummonWithoutValuableMaterials(card.Id))
                {
                    if (CanDealLethal() || OpponentHasActiveNegator()) return true;
                    DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning Synchro {card.Name} skipped: would consume active Ace card(s)");
                    return false;
                }
            }

            return true;
        }

        private bool IsMaterialSelectionSafeForLink(int requiredRating)
        {
            var faceUpNonAceMonsters = Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !IsAceCard(c))
                .ToList();

            int availableRating = 0;
            foreach (var m in faceUpNonAceMonsters)
            {
                if (m.HasType(CardType.Link))
                    availableRating += m.LinkCount;
                else
                    availableRating += 1;
            }

            return faceUpNonAceMonsters.Count >= requiredRating && availableRating >= requiredRating;
        }

        private bool CanSummonWithoutValuableMaterials(int targetCardId)
        {
            var ownMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            var availableMaterials = ownMonsters.Where(c => !IsValuableMonster(c)).ToList();
            return availableMaterials.Count >= 2;
        }

        protected override bool IsBoardStrongEnough()
        {
            int disruptions = CountDisruptions();
            bool hasMurakumo = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.AmeNoMurakumoNoMitsurugi));
            bool hasBianca = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.ClownCrewBiancaviso));
            bool hasMeteor = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.ClownCrewMeteor));

            if ((hasMurakumo || hasBianca || hasMeteor) && disruptions >= 2) return true;
            if (disruptions >= 3) return true;

            int totalAtk = GetTotalFieldATK();
            if (totalAtk >= 8000) return true;

            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
            {
                if (ResourcePlan.NibiruCheckpoint(Duel.Turn, Enemy.Hand.Count, Bot.GetMonsterCount(), HasNegateOnField()) || OpponentHasActiveNegator())
                    return true;
            }

            bool isStarterNegated = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.ClownCrewFlair) && m.IsDisabled());
            if (isStarterNegated && Bot.GetMonsterCount() >= 1 && Bot.Hand.Count <= 1)
            {
                DecisionTracer.TraceSkip("ShouldStopExtending", "Starter negated and resources low; conserving for follow-up");
                return true;
            }

            return base.ShouldStopExtending();
        }

        private bool IsTargetable(ClientCard card)
        {
            if (card == null) return false;
            if (card.IsShouldNotBeTarget()) return false;
            return IsViableEffectTarget(card);
        }

        private ClientCard GetHighThreatEnemyCard(bool monstersOnly = false, bool spellsOnly = false)
        {
            var highThreatCardIds = new[] {
                48680970, // Eternal Soul (destroying this nukes all DM monsters!)
                66399653, // Union Hangar
                47222536, // Dark Magical Circle
                10443957, // Cyber Dragon Infinity
                63767246, // Number 38: Hope Harbinger
                1561110,  // ABC-Dragon Buster
                38517737, // Blue-Eyes Alternative White Dragon
                89631139, // Blue-Eyes White Dragon
                46986414, // Dark Magician
                7084129   // Magician's Rod
            };

            var candidates = new List<ClientCard>();
            if (!spellsOnly)
                candidates.AddRange(Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsTargetable(c)));
            if (!monstersOnly)
                candidates.AddRange(Enemy.GetSpells().Where(c => c != null && c.IsFaceup() && IsTargetable(c)));

            var threat = candidates.FirstOrDefault(c => highThreatCardIds.Contains(c.Id));
            if (threat != null) return threat;

            var highestAtk = candidates.Where(c => c.IsMonster()).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (highestAtk != null && highestAtk.Attack >= 2000) return highestAtk;

            return candidates.FirstOrDefault() ?? Util.GetBestEnemyCard(canBeTarget: true);
        }

        // ============================================================
        //  ACTIVATION & SUMMONING EFFECTS
        // ============================================================

        private bool DimensionShifterEffect()
        {
            if (Bot.Graveyard.Count == 0 && (Duel.Player == 1 || Duel.Turn > 1))
            {
                DecisionTracer.TraceActivate("DimensionShifter", "Locking graveyard on opponent turn / going second");
                return true;
            }
            return false;
        }

        private bool FydraulisHarmoniaEffect()
        {
            if (_fydraulisUsed) return false;

            var lastChain = Util.GetLastChainCard();
            if (lastChain != null && lastChain.IsMonster())
            {
                var synchros = Bot.ExtraDeck.Where(c => c != null && c.HasType(CardType.Synchro)).Select(c => c.Id).ToList();
                if (synchros.Count >= 3)
                {
                    _fydraulisUsed = true;
                    int oppFaceupCount = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup()) + Enemy.GetSpells().Count(c => c != null && c.IsFaceup());
                    int malongCount = Bot.ExtraDeck.Count(c => c != null && c.Id == CardId.GoldenCloudBeastMalong);

                    if (oppFaceupCount >= 2 && malongCount >= 2)
                    {
                        DecisionTracer.TraceActivate("FydraulisHarmonia", "Sending 2x Malong (Double Bounce) + Wind Pegasus to GY");
                        AI.SelectCard(CardId.GoldenCloudBeastMalong, CardId.GoldenCloudBeastMalong, CardId.WindPegasusIgnister);
                    }
                    else
                    {
                        DecisionTracer.TraceActivate("FydraulisHarmonia", "Sending Malong + Pegasus + Meteor to GY");
                        AI.SelectCard(CardId.GoldenCloudBeastMalong, CardId.WindPegasusIgnister, CardId.ClownCrewMeteor);
                    }
                    return true;
                }
                else if (synchros.Count >= 1)
                {
                    _fydraulisUsed = true;
                    AI.SelectCard(synchros);
                    return true;
                }
            }
            return false;
        }

        private bool GoldenCloudBeastMalongEffect()
        {
            var target = GetHighThreatEnemyCard();
            if (target != null)
            {
                DecisionTracer.TraceActivate("GoldenCloudBeastMalong", $"Bouncing high-threat card {target.Name}");
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool WindPegasusIgnisterEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            var target = GetHighThreatEnemyCard()
                ?? Enemy.Graveyard.FirstOrDefault(c => c != null);
            if (target != null)
            {
                DecisionTracer.TraceActivate("WindPegasusIgnister", $"Shuffling opponent card {target.Name} into deck");
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CanSuperPoly()
        {
            var allMonsters = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Where(c => c != null && c.IsFaceup()).ToList();
            if (allMonsters.Count < 2) return false;

            // Check Clown Crew Diabolo (2 same Attribute, different Types)
            var attrGroups = allMonsters.GroupBy(c => c.Attribute).Where(g => g.Select(c => c.Race).Distinct().Count() >= 2);
            if (attrGroups.Any())
                return true;

            // Check Garura (2 same Type and Attribute, different names)
            var sameTypeAttr = allMonsters.GroupBy(c => new { c.Race, c.Attribute }).Where(g => g.Select(c => c.Id).Distinct().Count() >= 2);
            if (sameTypeAttr.Any())
                return true;

            // Check Panzer Dragon (1 Machine + 1 Dragon)
            bool hasMachine = allMonsters.Any(c => c.Race == (int)CardRace.Machine);
            bool hasDragon = allMonsters.Any(c => c.Race == (int)CardRace.Dragon);
            if (hasMachine && hasDragon)
                return true;

            // Check Sea Monster of Theseus (2 Tuners)
            int tunersCount = allMonsters.Count(c => c.HasType(CardType.Tuner));
            if (tunersCount >= 2)
                return true;

            return false;
        }

        private bool SuperPolymerizationEffect()
        {
            int requiredHand = (Card != null && Card.Location == CardLocation.Hand) ? 2 : 1;
            if (Bot.Hand.Count < requiredHand) return false;

            if (!CanSuperPoly()) return false;

            var opponentMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()).ToList();
            if (opponentMonsters.Count >= 1)
            {
                var costCards = new[] { CardId.MitsurugiMirror, CardId.MitsurugiGreatPurification, CardId.MitsurugiPrayers, CardId.ClownCrewRehearsal };
                var handCost = Bot.Hand.FirstOrDefault(c => c != null && c != Card && costCards.Contains(c.Id))
                    ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && !IsAceCard(c))
                    ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card);

                if (handCost != null)
                {
                    DecisionTracer.TraceActivate("SuperPolymerization", $"Fusing opponent board threats using cost {handCost.Name}");
                    AI.SelectCard(handCost);
                    AI.SelectNextCard(new[] {
                        CardId.ClownCrewDiabolo,
                        CardId.GaruraWingsOfResonantLife,
                        CardId.PanzerDragon,
                        CardId.SeaMonsterOfTheseus
                    });
                    AI.SelectThirdCard(opponentMonsters);
                    return true;
                }
            }
            return false;
        }

        private bool ForbiddenDropletEffect()
        {
            var targets = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.IsDisabled() && m.HasType(CardType.Effect)).ToList();
            if (targets.Count > 0)
            {
                var costCards = new[] { CardId.MitsurugiMirror, CardId.MitsurugiGreatPurification, CardId.ClownCrewRehearsal };
                var cost = Bot.Hand.Concat(Bot.GetMonsters().Concat(Bot.GetSpells())).Where(c => c != null && c != Card && 
                    (costCards.Contains(c.Id) || (c.IsMonster() && !IsAceCard(c)))).ToList();
                if (cost.Count > 0)
                {
                    DecisionTracer.TraceActivate("ForbiddenDroplet", $"Negating {targets.Count} enemy monsters");
                    AI.SelectCard(cost);
                    AI.SelectNextCard(targets);
                    return true;
                }
            }
            return false;
        }

        private bool ForbiddenCrownEffect()
        {
            if (_crownUsed) return false;
            ClientCard lastChain = Util.GetLastChainCard();

            if (lastChain != null && lastChain.Controller == 1 && lastChain.IsMonster() && lastChain.Location == CardLocation.MonsterZone)
            {
                if (lastChain.IsFaceup() && !lastChain.IsDisabled() && IsTargetable(lastChain))
                {
                    _crownUsed = true;
                    DecisionTracer.TraceActivate("ForbiddenCrown", $"Chaining negate to {lastChain.Name}");
                    AI.SelectCard(lastChain);
                    return true;
                }
            }

            if (Duel.Player == 1 || (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)))
            {
                ClientCard target = GetHighThreatEnemyCard(monstersOnly: true);
                if (target != null && !target.IsDisabled())
                {
                    _crownUsed = true;
                    DecisionTracer.TraceActivate("ForbiddenCrown", $"Negating high-threat {target.Name}");
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (_tttUsed) return false;
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                _tttUsed = true;
                if (Enemy.GetMonsterCount() > 0 && CanDealLethal())
                {
                    DecisionTracer.TraceActivate("TripleTacticsTalent", "Taking control of opponent monster for lethal push");
                    AI.SelectOption(1); // Take control
                    var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsTargetable(m));
                    if (target != null) AI.SelectCard(target);
                }
                else
                {
                    DecisionTracer.TraceActivate("TripleTacticsTalent", "Drawing 2 cards");
                    AI.SelectOption(0); // Draw 2
                }
                return true;
            }
            return false;
        }

        private bool PrePreparationOfRitesEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_prePrepUsed) return false;
            _prePrepUsed = true;
            DecisionTracer.TraceActivate("PrePreparationOfRites", "Searching Mitsurugi Mirror and Ame no Murakumo");
            AI.SelectCard(CardId.MitsurugiMirror);
            AI.SelectNextCard(CardId.AmeNoMurakumoNoMitsurugi, CardId.FutsuNoMitamaNoMitsurugi, CardId.AmeNoHabakiriNoMitsurugi);
            return true;
        }

        private bool ClownCrewFlairEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                if (_flairUsed) return false;
                _flairUsed = true;
                DecisionTracer.TraceActivate("ClownCrewFlair", "Searching Clown Crew Rehearsal/Soiree/Matinee");
                AI.SelectCard(CardId.ClownCrewRehearsal, CardId.ClownCrewSoireeOperations, CardId.ClownCrewMatineeOperatics, CardId.ClownCrewBiancaviso);
                AI.SelectNextCard(CardId.MitsurugiMirror, CardId.MitsurugiGreatPurification, CardId.ClownCrewRehearsal);
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                if (_flairTributedUsed) return false;
                _flairTributedUsed = true;
                if (Enemy.GetMonsterCount() > 0)
                {
                    AI.SelectOption(1); // Option 2 (face-down defense)
                    var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() && m.HasType(CardType.Effect));
                    if (target != null)
                    {
                        DecisionTracer.TraceActivate("ClownCrewFlair", $"Flipping {target.Name} face-down");
                        AI.SelectCard(target);
                    }
                    return true;
                }
                else
                {
                    AI.SelectOption(0); // Option 1 (shuffle Rituals)
                    return true;
                }
            }
            return false;
        }

        private bool ClownCrewRehearsalEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                if (_rehearsalUsed) return false;
                bool hasMonster = Bot.Hand.Any(c => c != null && c.IsMonster() && c != Card) || Bot.GetMonsterCount() > 0;
                if (!hasMonster) return false;

                _rehearsalUsed = true;
                DecisionTracer.TraceActivate("ClownCrewRehearsal", "Tributing monster to add Biancaviso & Matinee");
                AI.SelectCard(BestTributeTargets);
                AI.SelectNextCard(CardId.ClownCrewBiancaviso);
                AI.SelectThirdCard(CardId.ClownCrewMatineeOperatics, CardId.ClownCrewSoireeOperations, CardId.ClownCrewMalabarisme);
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_rehearsalGyUsed) return false;
                bool hasFlair = Bot.Hand.Any(c => c != null && c.IsCode(CardId.ClownCrewFlair));
                if (hasFlair)
                {
                    _rehearsalGyUsed = true;
                    DecisionTracer.TraceActivate("ClownCrewRehearsal", "GY effect: Recovering Flair");
                    AI.SelectCard(CardId.ClownCrewFlair);
                    AI.SelectNextCard(BestTributeTargets);
                    return true;
                }
            }
            return false;
        }

        private bool ClownCrewMatineeOperaticsEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }
            else if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_matineeUsed) return false;
                if (IsSpecialSummonBlocked() || Bot.GetMonsterCount() >= 5) return false;
                _matineeUsed = true;
                DecisionTracer.TraceActivate("ClownCrewMatineeOperatics", "Reviving Clown Crew monster from GY");
                AI.SelectCard(CardId.ClownCrewMeteor, CardId.ClownCrewDiabolo, CardId.ClownCrewFiends, CardId.ClownCrewFlair, CardId.ClownCrewBiancaviso);
                return true;
            }
            return false;
        }

        private bool ClownCrewMalabarismeEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_malabarismeUsed) return false;
                if (IsSpecialSummonBlocked() || Bot.GetMonsterCount() >= 5) return false;
                _malabarismeUsed = true;
                DecisionTracer.TraceActivate("ClownCrewMalabarisme", "Special Summoning Clown Crew from Deck/GY");
                AI.SelectCard(CardId.ClownCrewMeteor, CardId.ClownCrewDiabolo, CardId.ClownCrewFiends, CardId.ClownCrewFlair, CardId.ClownCrewBiancaviso);
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_malabarismeGyUsed) return false;
                var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && IsTargetable(m))
                    ?? Enemy.GetSpells().FirstOrDefault(s => s != null && IsTargetable(s));
                if (target != null)
                {
                    var tribute = Bot.Hand.Concat(Bot.GetMonsters())
                        .Where(m => m != null && m != Card)
                        .OrderBy(m => {
                            if (BestTributeTargets.Contains(m.Id)) return 0;
                            if (IsAceCard(m)) return 2;
                            return 1;
                        }).FirstOrDefault();
                    if (tribute == null) return false;

                    _malabarismeGyUsed = true;
                    DecisionTracer.TraceActivate("ClownCrewMalabarisme", $"GY effect: Tributing {tribute.Name} to destroy {target.Name}");
                    AI.SelectCard(tribute);
                    AI.SelectNextCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool ClownCrewSoireeOperationsEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (_soireeUsed) return false;
                _soireeUsed = true;
                if (Bot.GetMonstersInMainZone().Count < 5 && !IsSpecialSummonBlocked())
                {
                    DecisionTracer.TraceActivate("ClownCrewSoireeOperations", "Special Summoning Clown Crew monster");
                    AI.SelectOption(0);
                    AI.SelectCard(CardId.ClownCrewMeteor, CardId.ClownCrewDiabolo, CardId.ClownCrewFiends, CardId.ClownCrewFlair, CardId.ClownCrewBiancaviso);
                }
                else
                {
                    AI.SelectOption(1);
                    AI.SelectCard(CardId.ClownCrewBiancaviso, CardId.ClownCrewFlair);
                }
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_soireeGyUsed) return false;
                bool hasTribute = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.HasType(CardType.Ritual) || m.IsExtraCard()));
                if (hasTribute && Bot.Hand.Any(c => c != null && c.IsCode(CardId.ClownCrewBiancaviso)))
                {
                    _soireeGyUsed = true;
                    DecisionTracer.TraceActivate("ClownCrewSoireeOperations", "GY effect: Tribute Summoning Biancaviso");
                    AI.SelectCard(CardId.ClownCrewBiancaviso);
                    return true;
                }
            }
            return false;
        }

        private bool MitsurugiPrayersEffect()
        {
            if (_prayersUsed) return false;
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                _prayersUsed = true;
                var tribute = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Race == (int)CardRace.Reptile && !IsAceCard(m))
                    ?? Bot.Hand.FirstOrDefault(m => m != null && m != Card && m.IsMonster() && m.Race == (int)CardRace.Reptile);
                
                if (tribute == null)
                {
                    var aceTribute = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Race == (int)CardRace.Reptile && IsAceCard(m));
                    if (aceTribute != null)
                    {
                        var res = ResourcePlan.EvaluateAceUsage(
                            card: aceTribute,
                            hasLethalIfUsed: CanDealLethal(),
                            isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                            haveAlternateWinCon: false
                        );
                        if (res.allowed)
                        {
                            tribute = aceTribute;
                        }
                    }
                }

                if (tribute != null)
                {
                    DecisionTracer.TraceActivate("MitsurugiPrayers", $"Tributing {tribute.Name} to search Ritual & Monster");
                    AI.SelectCard(tribute);
                    AI.SelectNextCard(CardId.FutsuNoMitamaNoMitsurugi, CardId.AmeNoMurakumoNoMitsurugi, CardId.AmeNoHabakiriNoMitsurugi, CardId.MitsurugiNoMikotoAramasa);
                    AI.SelectThirdCard(CardId.AmeNoMurakumoNoMitsurugi, CardId.FutsuNoMitamaNoMitsurugi, CardId.AmeNoHabakiriNoMitsurugi);
                }
                else
                {
                    AI.SelectCard(CardId.FutsuNoMitamaNoMitsurugi, CardId.AmeNoMurakumoNoMitsurugi, CardId.AmeNoHabakiriNoMitsurugi, CardId.MitsurugiNoMikotoAramasa);
                }
                return true;
            }
            return false;
        }

        private bool MitsurugiRitualEffect()
        {
            if (_ritualUsed) return false;
            _ritualUsed = true;
            bool hasRitualInHand = Bot.Hand.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Ritual) && c.Race == (int)CardRace.Reptile);
            if (hasRitualInHand)
            {
                DecisionTracer.TraceActivate("MitsurugiRitual", "Ritual Summoning from hand by tributing from Deck");
                AI.SelectOption(1); // Ritual Summon from hand by tributing from Deck
                AI.SelectCard(CardId.AmeNoMurakumoNoMitsurugi, CardId.FutsuNoMitamaNoMitsurugi, CardId.AmeNoHabakiriNoMitsurugi);
                AI.SelectNextCard(CardId.MitsurugiNoMikotoAramasa, CardId.MitsurugiNoMikotoSaji, CardId.MitsurugiNoMikotoKusanagi);
                return true;
            }
            else
            {
                DecisionTracer.TraceActivate("MitsurugiRitual", "Ritual Summoning from Deck");
                AI.SelectOption(0); // Ritual Summon from Deck
                AI.SelectCard(CardId.AmeNoMurakumoNoMitsurugi, CardId.FutsuNoMitamaNoMitsurugi, CardId.AmeNoHabakiriNoMitsurugi);
                AI.SelectNextCard(ReptileTributeTargets);
                return true;
            }
        }

        private bool MitsurugiMirrorEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (_mirrorUsed) return false;
                _mirrorUsed = true;
                DecisionTracer.TraceActivate("MitsurugiMirror", "Ritual Summoning Ame no Murakumo / Futsu no Mitama");
                AI.SelectCard(CardId.AmeNoMurakumoNoMitsurugi, CardId.FutsuNoMitamaNoMitsurugi, CardId.AmeNoHabakiriNoMitsurugi);
                AI.SelectNextCard(ReptileTributeTargets);
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_mirrorGyUsed) return false;
                _mirrorGyUsed = true;
                DecisionTracer.TraceActivate("MitsurugiMirror", "GY effect: Banishing to add Ritual Monster");
                return true;
            }
            return false;
        }

        private bool MitsurugiNoMikotoSajiEffect()
        {
            DecisionTracer.TraceActivate("MitsurugiNoMikotoSaji", "Searching Mitsurugi Spell/Trap on tribute");
            AI.SelectCard(CardId.MitsurugiRitual, CardId.MitsurugiPrayers, CardId.MitsurugiMirror, CardId.MitsurugiGreatPurification);
            return true;
        }

        private bool MitsurugiNoMikotoAramasaEffect()
        {
            DecisionTracer.TraceActivate("MitsurugiNoMikotoAramasa", "Searching Mitsurugi Reptile monster on tribute");
            AI.SelectCard(CardId.MitsurugiNoMikoWousu, CardId.FutsuNoMitamaNoMitsurugi, CardId.AmeNoMurakumoNoMitsurugi, CardId.AmeNoHabakiriNoMitsurugi, CardId.MitsurugiNoMikotoSaji);
            return true;
        }

        private bool MitsurugiNoMikotoKusanagiEffect()
        {
            DecisionTracer.TraceActivate("MitsurugiNoMikotoKusanagi", "Searching Mitsurugi Spell/Ritual on tribute");
            AI.SelectCard(CardId.MitsurugiPrayers, CardId.MitsurugiRitual, CardId.FutsuNoMitamaNoMitsurugi);
            return true;
        }

        private bool MitsurugiNoMikoWousuEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (Bot.Hand.Count > 0)
                {
                    DecisionTracer.TraceActivate("MitsurugiNoMikoWousu", "GY effect: Recovering Ritual Spell");
                    AI.SelectCard(CardId.MitsurugiMirror, CardId.MitsurugiGreatPurification);
                    return true;
                }
            }
            return false;
        }

        private bool FutsuNoMitamaNoMitsurugiEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (IsSpecialSummonBlocked() || Bot.GetMonsterCount() >= 5) return false;
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.Race == (int)CardRace.Reptile && c.Id != CardId.FutsuNoMitamaNoMitsurugi);
                if (target != null)
                {
                    DecisionTracer.TraceActivate("FutsuNoMitamaNoMitsurugi", $"Special Summoning {target.Name} from GY");
                    AI.SelectCard(target);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                DecisionTracer.TraceActivate("FutsuNoMitamaNoMitsurugi", "Tribute trigger: Adding Mitsurugi Ritual Spell/Monster");
                AI.SelectCard(CardId.MitsurugiPrayers, CardId.MitsurugiRitual, CardId.MitsurugiNoMikotoAramasa, CardId.MitsurugiNoMikoWousu);
                return true;
            }
            return false;
        }

        private bool AmeNoMurakumoNoMitsurugiEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
                {
                    var oppTargets = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                        .Where(c => c != null && IsTargetable(c))
                        .OrderByDescending(c => {
                            if (c.IsSpell() && c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) return 3500;
                            if (c.IsMonster() && c.IsFaceup()) return 2000 + c.Attack;
                            if (c.IsFacedown()) return 1500;
                            return 1000;
                        })
                        .Take(2)
                        .ToList();

                    if (oppTargets.Count > 0)
                    {
                        DecisionTracer.TraceActivate("AmeNoMurakumoNoMitsurugi", $"Quick Effect: Popping {oppTargets.Count} opponent cards ({string.Join(", ", oppTargets.Select(t => t.Name))})");
                        AI.SelectCard(oppTargets);
                        return true;
                    }
                    return true;
                }
                if (Util.GetLastChainCard() != null && Util.GetLastChainCard().Controller == 1)
                {
                    return true;
                }
                return false;
            }
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                DecisionTracer.TraceActivate("AmeNoMurakumoNoMitsurugi", "Tribute trigger: Adding Mitsurugi Ritual Spell/Monster");
                AI.SelectCard(CardId.MitsurugiPrayers, CardId.MitsurugiRitual, CardId.MitsurugiNoMikotoAramasa, CardId.MitsurugiNoMikoWousu);
                return true;
            }
            return false;
        }

        private bool AmeNoHabakiriNoMitsurugiEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_habakiriUsed) return false;
                if (IsSpecialSummonBlocked() || Bot.GetMonsterCount() >= 5) return false;
                var targets = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.IsCode(
                    CardId.MitsurugiNoMikotoAramasa,
                    CardId.MitsurugiNoMikotoSaji,
                    CardId.MitsurugiNoMikotoKusanagi,
                    CardId.MitsurugiNoMikoWousu,
                    CardId.FutsuNoMitamaNoMitsurugi,
                    CardId.AmeNoMurakumoNoMitsurugi
                )).ToList();
                if (targets.Count > 0)
                {
                    _habakiriUsed = true;
                    DecisionTracer.TraceActivate("AmeNoHabakiriNoMitsurugi", "Special Summoning 3000 ATK Habakiri from hand");
                    AI.SelectCard(targets);
                    return true;
                }
                return false;
            }
            else if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                DecisionTracer.TraceActivate("AmeNoHabakiriNoMitsurugi", "Tribute trigger: Adding Mitsurugi Ritual Spell/Monster");
                AI.SelectCard(CardId.MitsurugiPrayers, CardId.MitsurugiRitual, CardId.MitsurugiNoMikotoAramasa, CardId.MitsurugiNoMikoWousu);
                return true;
            }
            return false;
        }

        private bool Number23LancelotEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                DecisionTracer.TraceActivate("Number23Lancelot", "Mandatory Omni-Negation triggered");
                return true;
            }
            return false;
        }

        private bool ClownCrewBiancavisoEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_biancavisoUsed) return false;
                var oppNegatable = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.IsDisabled() && IsTargetable(m)).Concat(
                    Enemy.GetSpells().Where(s => s != null && s.IsFaceup() && !s.IsDisabled() && IsTargetable(s))
                ).OrderByDescending(c => {
                    if (c.IsSpell() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) return 3000;
                    if (c.IsMonster()) return c.Attack;
                    return 1000;
                }).ToList();

                if (oppNegatable.Count > 0)
                {
                    _biancavisoUsed = true;
                    DecisionTracer.TraceActivate("ClownCrewBiancaviso", $"Negating face-up opponent threat {oppNegatable[0].Name}");
                    AI.SelectOption(1); // Negate
                    AI.SelectCard(oppNegatable);
                    return true;
                }
                else
                {
                    _biancavisoUsed = true;
                    DecisionTracer.TraceActivate("ClownCrewBiancaviso", "Drawing 1 card");
                    AI.SelectOption(0); // Draw
                    return true;
                }
            }
            return false;
        }

        private bool ClownCrewMeteorEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_meteorUsed) return false;
                var target = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                    .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.IsMonster() ? c.Attack : 1500)
                    .FirstOrDefault();
                if (target != null)
                {
                    _meteorUsed = true;
                    DecisionTracer.TraceActivate("ClownCrewMeteor", $"Popping opponent threat {target.Name}");
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }
            if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.HasSetcode(0x1d3) && c.Id != CardId.ClownCrewMeteor);
                if (target != null)
                {
                    DecisionTracer.TraceActivate("ClownCrewMeteor", $"GY effect: Adding {target.Name} to hand");
                    AI.SelectOption(1); // Add Clown Crew card from GY to hand
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool ClownCrewFiendsEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                DecisionTracer.TraceActivate("ClownCrewFiends", "GY effect: Setting Clown Crew Spell from Deck");
                AI.SelectOption(1); // Set 1 Clown Crew spell from Deck
                AI.SelectCard(CardId.ClownCrewRehearsal, CardId.ClownCrewMatineeOperatics, CardId.ClownCrewMalabarisme);
                return true;
            }
            return false;
        }

        private bool ClownCrewDiaboloEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_diaboloUsed) return false;
                var target = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                    .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.IsMonster() ? c.Attack : 2000)
                    .FirstOrDefault();
                if (target != null)
                {
                    _diaboloUsed = true;
                    DecisionTracer.TraceActivate("ClownCrewDiabolo", $"Popping opponent card {target.Name}");
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }
            if (Card.Location == CardLocation.Grave)
            {
                DecisionTracer.TraceActivate("ClownCrewDiabolo", "GY effect: Returning Fusions to Extra Deck");
                AI.SelectOption(0); // Return all Fusion Monsters to Extra Deck
                return true;
            }
            return false;
        }

        private bool SPLittleKnightEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ActivateDescription == Util.GetStringId(CardId.SPLittleKnight, 0) || ActivateDescription == 0)
                {
                    var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && IsTargetable(m))
                        ?? Enemy.GetSpells().FirstOrDefault(s => s != null && IsTargetable(s))
                        ?? Enemy.Graveyard.FirstOrDefault(c => c != null);
                    if (target != null)
                    {
                        DecisionTracer.TraceActivate("SPLittleKnight", $"On-Summon: Banishing {target.Name}");
                        AI.SelectCard(target);
                        return true;
                    }
                }
                else
                {
                    var oppTarget = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsTargetable(m));
                    var ourTarget = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !IsAceCard(m))
                        ?? Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.IsCode(CardId.SPLittleKnight));
                    if (oppTarget != null && ourTarget != null)
                    {
                        DecisionTracer.TraceActivate("SPLittleKnight", $"Quick Effect: Banishing {ourTarget.Name} and {oppTarget.Name} till End Phase");
                        AI.SelectCard(ourTarget);
                        AI.SelectNextCard(oppTarget);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TheDragonThatDevoursTheDogmaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster())
                    ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());
                if (target != null)
                {
                    DecisionTracer.TraceActivate("TheDragonThatDevoursTheDogma", $"Banishing {target.Name} from GY");
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        //  SUMMONING CONDITIONS
        // ============================================================

        private bool MitsurugiNoMikoWousuSpSummon()
        {
            if (_wousuSpUsed) return false;
            if (IsSpecialSummonBlocked() || Bot.GetMonsterCount() >= 5) return false;

            // Delay Wousu if we still have non-Reptile plays in hand or Spellcasters on field
            if (Bot.Hand.Any(c => c != null && c.IsCode(CardId.ClownCrewFlair, CardId.PrePreparationOfRites, CardId.ClownCrewRehearsal, CardId.ClownCrewSoireeOperations))
                || Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Race == (int)CardRace.SpellCaster))
                return false;

            var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup());
            var handReptile = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.Race == (int)CardRace.Reptile && c.Id != CardId.MitsurugiNoMikoWousu);
            if (target != null && handReptile != null)
            {
                _wousuSpUsed = true;
                DecisionTracer.TraceActivate("MitsurugiNoMikoWousu", "Special Summoning Wousu by revealing Reptile");
                AI.SelectCard(handReptile);
                AI.SelectNextCard(target);
                return true;
            }
            return false;
        }

        private bool ClownCrewFlairSummon()
        {
            return !ShouldSkipCombo();
        }

        private bool MitsurugiNonTunerSummon()
        {
            if (ShouldSkipCombo()) return false;
            return Bot.GetMonsterCount() < 4;
        }

        private bool ClownCrewBiancavisoSummon()
        {
            var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !IsAceCard(m));
            if (target != null)
            {
                DecisionTracer.TraceActivate("ClownCrewBiancaviso", $"Tribute Summoning Biancaviso using {target.Name}");
                AI.SelectCard(target);
                return true;
            }

            var aceTarget = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsAceCard(m));
            if (aceTarget != null)
            {
                var res = ResourcePlan.EvaluateAceUsage(
                    card: aceTarget,
                    hasLethalIfUsed: CanDealLethal(),
                    isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                    haveAlternateWinCon: false
                );
                if (res.allowed)
                {
                    DecisionTracer.TraceActivate("ClownCrewBiancaviso", $"Tribute Summoning Biancaviso using Ace {aceTarget.Name} ({res.reason})");
                    AI.SelectCard(aceTarget);
                    return true;
                }
            }
            return false;
        }

        private bool Number23LancelotSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var lv8s = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.Level == 8).ToList();
            if (lv8s.Count >= 2 && (CanDealLethal() || IsBoardStrongEnough() || Duel.Player == 0))
            {
                DecisionTracer.TraceActivate("Number23Lancelot", "Xyz Summoning Rank 8 Number 23: Lancelot");
                return true;
            }
            return false;
        }

        private bool ClownCrewMeteorSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return !ShouldStopExtending();
        }

        private bool ClownCrewDiaboloSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return CanSuperPoly() || CanDealLethal();
        }

        private bool ClownCrewFiendsSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return !ShouldStopExtending();
        }

        private bool SPLittleKnightSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return IsMaterialSelectionSafeForLink(2);
        }

        private bool DogmaSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Enemy.Graveyard.Any(c => c != null && c.IsMonster());
        }

        private bool MitsurugiGreatPurificationEffect()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                if (_purificationUsed) return false;
                var cost = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Level >= 5 && m.Race == (int)CardRace.Reptile && !IsAceCard(m));
                if (cost == null)
                {
                    var aceCost = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Level >= 5 && m.Race == (int)CardRace.Reptile && IsAceCard(m));
                    if (aceCost != null)
                    {
                        var res = ResourcePlan.EvaluateAceUsage(
                            card: aceCost,
                            hasLethalIfUsed: CanDealLethal(),
                            isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                            haveAlternateWinCon: false
                        );
                        if (res.allowed)
                        {
                            cost = aceCost;
                        }
                    }
                }
                if (cost != null)
                {
                    _purificationUsed = true;
                    DecisionTracer.TraceActivate("MitsurugiGreatPurification", $"Tributing {cost.Name} to banish opponent card");
                    AI.SelectCard(cost);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_purificationGyUsed) return false;
                bool hasTribute = Bot.GetMonsterCount() > 0;
                if (hasTribute)
                {
                    _purificationGyUsed = true;
                    DecisionTracer.TraceActivate("MitsurugiGreatPurification", "GY effect: Ritual Summoning Reptile");
                    AI.SelectCard(CardId.AmeNoMurakumoNoMitsurugi, CardId.FutsuNoMitamaNoMitsurugi, CardId.AmeNoHabakiriNoMitsurugi);
                    AI.SelectNextCard(ReptileTributeTargets);
                    return true;
                }
            }
            return false;
        }

        private bool SpellSetFiltered()
        {
            if (Card == null) return false;
            if (Card.IsCode(CardId.MitsurugiGreatPurification, CardId.ClownCrewSoireeOperations, CardId.MitsurugiPrayers, CardId.CalledByTheGrave, CardId.ForbiddenCrown, CardId.ForbiddenDroplet, CardId.SuperPolymerization))
            {
                return true;
            }
            return false;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // โ”€โ”€ Hint 500 / 505: Tribute & Release Selection โ”€โ”€
            // Prioritize tribute triggers: Aramasa (searches monster), Saji (searches S/T), Kusanagi (searches Spell/Ritual), Flair, Wousu
            // Heavily protect Ace cards from being tributed
            if (hint == 500 || hint == 505)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.Id == CardId.MitsurugiNoMikotoAramasa) return 10;
                    if (c.Id == CardId.MitsurugiNoMikotoSaji) return 20;
                    if (c.Id == CardId.MitsurugiNoMikotoKusanagi) return 30;
                    if (c.Id == CardId.ClownCrewFlair) return 40;
                    if (c.Id == CardId.MitsurugiNoMikoWousu) return 50;
                    if (IsAceCard(c)) return 900;
                    if (IsValuableMonster(c)) return 500;
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // โ”€โ”€ Hint 501 / 549: Target Removal / Negation / Disruption โ”€โ”€
            if (hint == 501 || hint == 549)
            {
                var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (oppCards.Count > 0)
                {
                    var sortedOpp = oppCards.OrderByDescending(c => {
                        if (c.IsSpell() && c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) return 4000;
                        if (c.IsMonster() && c.IsFaceup() && !c.IsDisabled()) return 3000 + c.Attack;
                        if (c.IsFacedown()) return 2000;
                        return 1000;
                    }).ToList();
                    return sortedOpp.Take(max).ToList();
                }
            }

            // Protect valuable monsters and Ace Cards from being used as material if possible
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533 || hint == 508 || hint == 504)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 200;
                    if (IsValuableMonster(c)) return 100;
                    return 0;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 506 / Search priority
            if (hint == 506)
            {
                var preferred = new List<int> {
                    CardId.AmeNoMurakumoNoMitsurugi,
                    CardId.FutsuNoMitamaNoMitsurugi,
                    CardId.AmeNoHabakiriNoMitsurugi,
                    CardId.MitsurugiMirror,
                    CardId.MitsurugiPrayers,
                    CardId.MitsurugiRitual,
                    CardId.ClownCrewBiancaviso,
                    CardId.ClownCrewFlair,
                    CardId.ClownCrewRehearsal,
                    CardId.MitsurugiGreatPurification,
                    CardId.ClownCrewSoireeOperations
                };

                var matches = cards.Where(c => c != null && preferred.Contains(c.Id))
                    .OrderBy(c => preferred.IndexOf(c.Id))
                    .ToList();

                if (matches.Count >= min) return matches.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        private bool IsValuableMonster(ClientCard card)
        {
            if (card == null) return false;
            return IsAceCard(card) || card.HasType(CardType.Ritual) || card.IsExtraCard();
        }

        // ============================================================
        // COMBAT LOGIC & ATTACK ROUTING
        // ============================================================

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            YGOSharp.OCGWrapper.NamedCard cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                if (cardId == CardId.AmeNoMurakumoNoMitsurugi && positions.Contains(CardPosition.FaceUpDefence))
                {
                    // Murakumo has 0 ATK and 2000 DEF; prefer defense
                    return CardPosition.FaceUpDefence;
                }
                if (cardData.Attack <= 1000 && cardData.Defense > cardData.Attack && positions.Contains(CardPosition.FaceUpDefence))
                {
                    return CardPosition.FaceUpDefence;
                }
            }
            return base.OnSelectPosition(cardId, positions);
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            var sorted = attackers.Where(c => c != null && !c.Attacked && c.Attack > 0)
                .OrderByDescending(c => c.Attack).ToList();
            return sorted.Count > 0 ? sorted.First() : base.OnSelectAttacker(attackers, defenders);
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (defenders.Count == 0) return AI.Attack(attacker, null);

            int GetDefenseValue(ClientCard c)
            {
                if (c == null) return 0;
                return c.IsDefense() ? c.Defense : c.Attack;
            }

            var defeatable = defenders.Where(d => attacker.Attack > GetDefenseValue(d))
                .OrderByDescending(d => GetDefenseValue(d)).ToList();
            if (defeatable.Count > 0) return AI.Attack(attacker, defeatable.First());

            if (defenders.All(d => d == null)) return AI.Attack(attacker, null);

            var equal = defenders.Where(d => attacker.Attack == GetDefenseValue(d) && d.IsAttack()).ToList();
            if (equal.Count > 0 && !IsAceCard(attacker) && Bot.LifePoints > Enemy.LifePoints)
                return AI.Attack(attacker, equal.First());

            return null;
        }
    }

    [Deck("Expert_2026_Clown", "2026_Clown")]
    public class ExpertClownExecutor : _2026_ClownExecutor
    {
        private string _duelId;
        public ExpertClownExecutor(GameAI ai, Duel duel) : base(ai, duel)
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
