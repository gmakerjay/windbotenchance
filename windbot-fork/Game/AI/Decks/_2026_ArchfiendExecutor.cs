// =========================================================================================
// CARD AUDIT — 2026_Archfiend (Archfiend Ritual & Synchro Hybrid Engine)
// | Card Name                              | Type         | OPT? | Cost             | Effect Summary                                                        | Activate When                              | NEVER When                            |
// | :------------------------------------- | :----------: | :--: | :--------------: | :-------------------------------------------------------------------: | :----------------------------------------: | :-----------------------------------: |
// | Archfiend Matriarch (78744660)     | Pend/Fiend   | HOPT | None             | Lv7 2100/2900 P-Scale 9. P-Effect: recover Archfiend S/T; Mon: Pop S/T | P-Scale setup / GY S/T recovery             | No P-Zone space or S/T in GY          |
// | Archfiend Emperor (48469380)           | Pend/Fiend   | HOPT | Banish Fiend     | Lv7 2500/1200. P-Effect: Pop 1 card; Mon: Banish to pop 1              | P-Scale setup / On-field threat removal    | No banish target / No opp targets     |
// | Matador Archfiend (7622360)            | Ritual/Fiend | HOPT | None             | Lv6 Ritual 0/0. On Summon: Search 2 Archfiend cards                   | Core Ritual Starter / Engine setup         | Skip combo active                     |
// | Royal Archfiend (58769832)             | Monster      | HOPT | None             | Lv7 2400/1000 Fiend. On Summon: Search 1 Archfiend card               | Core Extender / Search Highness or Throne  | Target already in hand / deck empty   |
// | Duke Archfiend (85154941)              | Monster      | HOPT | None             | Lv4 0/2000 Fiend. Level adjust + Mill 1 Archfiend (Heiress)           | Setup Synchro Level + Trigger Heiress mill | No Heiress left in deck               |
// | Archfiend Heiress (66540884)           | Monster      | HOPT | None             | Lv3 1000/0 Fiend. Sent to GY: Search ANY Archfiend card               | Triggers on Mill / Destroy / Card effect   | Hand already has all required pieces  |
// | Highness Archfiend (11248645)          | Monster      | HOPT | Banish GY Fiend  | Lv3 1000/800 Fiend. Banish GY Fiend: Search 2 Archfiend cards          | Extender / Mid-game resource generation    | No Fiends in GY                       |
// | Regenesis Archfiend (95718355)         | Tuner/Fiend  | HOPT | None             | Lv8 2500/2500 Tuner. SS from hand + Search Regenesis Sage              | Synchro Level 7/10 enabler                 | SS blocked                            |
// | Regenesis Sage (22938501)              | Tuner/Fiend  | HOPT | None             | Lv7 2500/2500 Tuner. Extender for Synchro Summoning                    | Synchro material / Search Regenesis        | SS blocked                            |
// | Throne of the Archfiends (63679166)    | Field Spell  | HOPT | None             | Search Archfiend card & enables Ritual / SS                            | Field setup / Search missing engine piece  | Field zone occupied                   |
// | Ritual of the Matador (70105073)       | Ritual Spell | HOPT | Tribute Fiends   | Ritual Summon Matador Archfiend from Hand; GY: Add Fiend from GY to hand| Core Ritual Summon enabler                 | No Matador in hand                    |
// | Archfiend Usurpation (82997779)        | Cont Trap    | HOPT | None             | Set Archfiend Trap from Deck / Quick Ritual Summon on opp turn         | Quick disruption & Trap setup              | Trap zone full                        |
// | Archfiend Strategy (90764871)         | Spell        | HOPT | Send Fiend to GY | Send Fiend from Hand/Deck to GY -> Draw 2 (triggers Heiress!)         | Draw power + Mill trigger Heiress          | Hand is empty                         |
// | Regenesis (31786838)                   | Spell        | HOPT | None             | Revive Regenesis Archfiend / Sage from GY                              | Synchro material recovery                  | No targets in GY                      |
// | Archfiend's Ghastly Glitch (5168381)   | Normal Trap  | HOPT | None             | Pop 1 card on field, then Send 1 Fiend from Deck to GY (Heiress!)      | Core Disruption + Engine Mill              | No Fiends on field / No targets       |
// | Archfiend Playtime (87985506)         | Cont Spell   | HOPT | None             | SS Archfiend from Deck when Archfiend is destroyed/effect activated    | Continuous Board Presence & Swarm          | SS blocked                            |
// | Archfiends' Fervor (13379114)         | Cont Trap    | HOPT | None             | ATK pump + Effect Negation during Battle / Chain                       | Combat dominance & Negation                | No Ritual in EMZ                      |
// | Pre-Preparation of Rites (13048472)   | Spell        | HOPT | None             | Search Ritual of the Matador + Matador Archfiend                       | Primary 1st Turn Starter                   | No targets in deck                    |
// | Odd-Eyes Meteorburst Dragon (80696379) | Synchro/Mon  | HOPT | None             | Level 7 Synchro (4+3). On SS: SS Pendulum Monster from P-Zone!         | Bring Emperor / Regina to Monster Zone     | P-Zone is empty                       |
// | Black Rose Dragon (73580471)          | Synchro/Mon  | HOPT | None             | Level 7 Synchro (4+3). On SS: Destroy all cards on the field           | Emergency Board Clear when falling behind  | Bot has established Boss board        |
// | Ruddy Rose Dragon (40139997)          | Synchro/Mon  | HOPT | None             | Level 10 Synchro (7+3). On SS: Banish ALL cards in ALL Graveyards!     | GY Wipe against GY decks / 3200 ATK Beat   | GY wipe harms bot more than opp       |
// | Bramble Rose Dragon (6560411)         | Synchro/Mon  | HOPT | None             | Level 7 Synchro (4+3). Burn damage on SS / Control                     | Extra Deck extender                        | SS blocked                            |
// | Periallis, Empress of Blossoms (72924435)| Synchro/Mon| HOPT | None             | Level 7 Synchro (4+3). ATK booster & Reviver                           | High ATK push                              | SS blocked                            |
// =========================================================================================
// ACE CARDS:
// Primary Ace: Matador Archfiend, Archfiend Emperor, Doom Regina Archfiend
// Secondary Ace: Ruddy Rose Dragon, Odd-Eyes Meteorburst Dragon, Highness Archfiend
// Critical Resource: Archfiend Heiress, Royal Archfiend, Ritual of the Matador, Pre-Preparation of Rites
// =========================================================================================
// WIN CONDITION:
// 1. Going 1st: Matador Archfiend (Defense / Search) + Doom Regina / Emperor in P-Zone -> Meteorburst SS Boss + Set Ghastly Glitch / Usurpation.
// 2. Going 2nd: Clear board with Black Rose / Ruddy Rose -> Archfiend Emperor (2500) + Archfiend Matriarch (2100) + Royal Archfiend (2400) -> 7000+ Damage + Fervor/Synchro!
// =========================================================================================

// =========================================================================================
// COMBO DRAFT — 2026_Archfiend
// =========================================================================================
// === COMBO LINE 1: Matador Ritual Primary Line (Pre-Prep / Ritual) ===
// HAND REQUIRED: Pre-Preparation of Rites or (Ritual of the Matador + Matador Archfiend)
// STEP 1: Activate Pre-Preparation of Rites -> Add Ritual of the Matador + Matador Archfiend
// STEP 2: Activate Ritual of the Matador -> Tribute Fiends (e.g. Heiress) -> Ritual Summon Matador Archfiend
// STEP 3: Trigger Matador Effect -> Search Royal Archfiend + Archfiend Playtime
// STEP 4: Trigger Heiress Effect (if tributed) -> Search Highness Archfiend / Throne of the Archfiends
// STEP 5: Activate Archfiend Playtime -> Normal Summon Royal Archfiend -> Search Duke Archfiend
// STEP 6: Scale Doom Regina & Archfiend Emperor in Pendulum Zones
// STEP 7: Set Archfiend's Ghastly Glitch + Usurpation -> Pass turn in impenetrable control state
// END BOARD: Matador Archfiend (Defense) + Royal Archfiend + Doom Regina & Emperor in P-Zone + Set Glitch
//
// === COMBO LINE 2: Regenesis Synchro Line (Regenesis + Sage) ===
// HAND REQUIRED: Regenesis Archfiend (Lv7 Tuner) + Regenesis Sage (Lv3 Tuner) or Duke (Lv4) + Sage (Lv3)
// STEP 1: Special Summon Regenesis Archfiend -> Search Regenesis Spell
// STEP 2: Normal Summon Duke Archfiend -> Send Archfiend Heiress to GY (Trigger Heiress search!)
// STEP 3: Synchro Summon Level 7: Odd-Eyes Meteorburst Dragon (Duke Lv4 + Sage Lv3)
// STEP 4: Meteorburst Effect triggers: Special Summon Archfiend Emperor (3000 ATK) from Pendulum Zone!
// STEP 5: (Or Synchro Level 10: Ruddy Rose Dragon using Lv7 Regenesis + Lv3 Sage to wipe opp GY)
// END BOARD: Odd-Eyes Meteorburst Dragon + Archfiend Emperor + Matador Archfiend
//
// === COMBO LINE 3: Going 2nd Board Break & 8000+ OTK ===
// HAND REQUIRED: Board Breaker (Evenly / Droplet) + Archfiend Starters
// STEP 1: If opponent has overwhelming field: Synchro Summon Black Rose Dragon -> Nuke entire field!
// STEP 2: Normal/Special Summon Archfiend Emperor (3000 ATK) + Highness Archfiend (2400 ATK) + Doom Regina (2800 ATK)
// STEP 3: Emperor effect: Banish GY Archfiend -> Pop remaining obstacle
// STEP 4: Battle Phase: Attack for 3000 + 2400 + 2800 = 8200 Damage -> OTK!
// =========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.DecisionEngine;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Archfiend", "2026_Archfiend")]
    public class _2026_ArchfiendExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Archetype Cards
            public const int DoomReginaArchfiend = 78744660;
            public const int ArchfiendEmperor = 48469380;
            public const int MatadorArchfiend = 7622360;
            public const int RoyalArchfiend = 58769832;
            public const int DukeArchfiend = 85154941;
            public const int ArchfiendHeiress = 66540884;
            public const int HighnessArchfiend = 11248645;
            public const int RegenesisArchfiend = 95718355;
            public const int RegenesisSage = 22938501;

            // Archetype Spells & Traps
            public const int ThroneOfTheArchfiends = 63679166;
            public const int RitualOfTheMatador = 70105073;
            public const int ArchfiendUsurpation = 82997779;
            public const int ArchfiendStrategy = 90764871;
            public const int Regenesis = 31786838;
            public const int ArchfiendsGhastlyGlitch = 5168381;
            public const int ArchfiendPlaytime = 87985506;
            public const int ArchfiendsFervor = 13379114;

            // Staples / Hand Traps / Spells
            public const int NibiruThePrimalBeing = 27204311;
            public const int MaxxC = 23434538;
            public const int AshBlossomAndJoyousSpring = 14558127;
            public const int DrollAndLockBird = 94145021;
            public const int GhostBelleAndHauntedMansion = 73642296;
            public const int TripleTacticsTalent = 25311006;
            public const int CrossoutDesignator = 65681983;
            public const int CalledByTheGrave = 24224830;
            public const int PotOfExtravagance = 49238328;
            public const int PotOfProsperity = 84211599;
            public const int PrePreparationOfRites = 13048472;

            // Side Deck Techs
            public const int MulcharmyFuwalos = 42141493;
            public const int ForbiddenDroplet = 24299458;
            public const int TripleTacticsThrust = 35269904;
            public const int BookOfEclipse = 35480699;
            public const int EvenlyMatched = 15693423;
            public const int DimensionalBarrier = 83326048;
            public const int AntiSpellFragrance = 58921041;
            public const int RedReboot = 23002292;
            public const int InfiniteImpermanence = 10045474;
            public const int RivalryOfWarlords = 90846359;
            public const int DragonsMind = 85442146;

            // Extra Deck
            public const int OddEyesMeteorburstDragon = 80696379;
            public const int BlackRoseDragon = 73580471;
            public const int BrambleRoseDragon = 6560411;
            public const int RageRoseWitch = 33955120;
            public const int RuddyRoseDragon = 40139997;
            public const int GardenRoseMaiden = 53325667;
            public const int PeriallisEmpressOfBlossoms = 72924435;
            public const int CrossroseDragon = 72218246;
            public const int TopologicTrisbaena = 72529749;
            public const int TripleBurstDragon = 49725936;
            public const int WorldGearsOfTheurlogicalDemiurgy = 57282724;
            public const int TopologicBomberDragon = 5821478;
            public const int TopologicZeroboros = 66403530;
        }

        private static readonly int[] BossMonsters = {
            CardId.DoomReginaArchfiend,
            CardId.ArchfiendEmperor,
            CardId.MatadorArchfiend,
            CardId.RuddyRoseDragon,
            CardId.OddEyesMeteorburstDragon,
            CardId.HighnessArchfiend
        };

        private static readonly int[] NonAceFodder = {
            CardId.ArchfiendHeiress,
            CardId.RoyalArchfiend,
            CardId.DukeArchfiend,
            CardId.RegenesisSage
        };

        // OPT Tracker Flags
        private bool _matadorUsed = false;
        private bool _emperorUsed = false;
        private bool _reginaUsed = false;
        private bool _royalUsed = false;
        private bool _dukeUsed = false;
        private bool _heiressUsed = false;
        private bool _highnessUsed = false;
        private bool _regenesisUsed = false;
        private bool _playtimeUsed = false;
        private bool _glitchUsed = false;
        private bool _fervorUsed = false;
        private bool _strategyUsed = false;

        public _2026_ArchfiendExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards to protect them from being used suboptimally
            HeuristicGuard.RegisterAceCards(BossMonsters);
            ResourcePlan.RegisterAceCards(BossMonsters);

            // ── Combo Router: Strategic Lines ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Matador-Ritual-Primary",
                RequiredCards = new List<int> { CardId.PrePreparationOfRites },
                FallbackLineName = "Heiress-Highness-Extender",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.PrePreparationOfRites, ActionType = ExecutorType.Activate, Description = "Search Matador & Ritual" },
                    new() { CardId = CardId.RitualOfTheMatador, ActionType = ExecutorType.Activate, Description = "Ritual Summon Matador Archfiend" },
                    new() { CardId = CardId.MatadorArchfiend, ActionType = ExecutorType.Activate, Description = "Search Royal & Playtime" },
                    new() { CardId = CardId.ArchfiendPlaytime, ActionType = ExecutorType.Activate, Description = "Activate Playtime to swarm Archfiends" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Heiress-Highness-Extender",
                RequiredCards = new List<int> { CardId.HighnessArchfiend },
                FallbackLineName = "Regenesis-Synchro-Line",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.HighnessArchfiend, ActionType = ExecutorType.Summon, Description = "Normal Summon Highness Archfiend" },
                    new() { CardId = CardId.HighnessArchfiend, ActionType = ExecutorType.Activate, Description = "Banish GY Archfiend & Search 2 Archfiends" }
                },
                EndBoardScore = 80,
                Condition = () => Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasSetcode(0x45))
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Regenesis-Synchro-Line",
                RequiredCards = new List<int> { CardId.RegenesisArchfiend },
                FallbackLineName = "Archfiend-Emperor-Pop-Fallback",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.RegenesisArchfiend, ActionType = ExecutorType.Activate, Description = "SS Regenesis & search Regenesis Sage" },
                    new() { CardId = CardId.RegenesisSage, ActionType = ExecutorType.Summon, Description = "Summon Sage for Synchro setup" }
                },
                EndBoardScore = 75
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Archfiend-Emperor-Pop-Fallback",
                RequiredCards = new List<int> { CardId.ArchfiendEmperor },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.ArchfiendEmperor, ActionType = ExecutorType.SpSummon, Description = "Summon Emperor & Pop Threat" }
                },
                EndBoardScore = 65
            });

            // ── Bait Planner & Chain Advisor ──
            BaitPlanner.RegisterComboStarters(CardId.PrePreparationOfRites, CardId.ThroneOfTheArchfiends, CardId.HighnessArchfiend);
            BaitPlanner.RegisterBaitCards(CardId.PotOfExtravagance, CardId.PotOfProsperity);
            ChainAdvisor.RegisterHighValueTargets(CardId.MatadorArchfiend, CardId.ArchfiendPlaytime, CardId.ArchfiendsGhastlyGlitch, CardId.DoomReginaArchfiend);

            // ===== PRIORITY 1: Hand Traps & Reactive Negates =====
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, DefaultMaxxC);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAndJoyousSpring, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelleAndHauntedMansion, DefaultGhostBelleAndHauntedMansion);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.NibiruThePrimalBeing, NibiruEffect);

            // ===== PRIORITY 2: Draw Pots & Starters (Start of Main 1) =====
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance, PotOfExtravaganceEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrePreparationOfRites, PrePreparationOfRitesEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.BookOfEclipse, BookOfEclipseEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsThrust, TripleTacticsThrustEffect);

            // ===== PRIORITY 3: Main Archetype Monster Activations =====
            AddExecutor(ExecutorType.Activate, CardId.DoomReginaArchfiend, DoomReginaArchfiendEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArchfiendEmperor, ArchfiendEmperorEffect);
            AddExecutor(ExecutorType.Activate, CardId.MatadorArchfiend, MatadorArchfiendEffect);
            AddExecutor(ExecutorType.Activate, CardId.RoyalArchfiend, RoyalArchfiendEffect);
            AddExecutor(ExecutorType.Activate, CardId.DukeArchfiend, DukeArchfiendEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArchfiendHeiress, ArchfiendHeiressEffect);
            AddExecutor(ExecutorType.Activate, CardId.HighnessArchfiend, HighnessArchfiendEffect);
            AddExecutor(ExecutorType.Activate, CardId.RegenesisArchfiend, RegenesisArchfiendEffect);
            AddExecutor(ExecutorType.Activate, CardId.RegenesisSage, RegenesisSageEffect);

            // ===== PRIORITY 4: Main Archetype Spell/Trap Activations =====
            AddExecutor(ExecutorType.Activate, CardId.ThroneOfTheArchfiends, ThroneOfTheArchfiendsEffect);
            AddExecutor(ExecutorType.Activate, CardId.RitualOfTheMatador, RitualOfTheMatadorEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArchfiendUsurpation, ArchfiendUsurpationEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArchfiendStrategy, ArchfiendStrategyEffect);
            AddExecutor(ExecutorType.Activate, CardId.Regenesis, RegenesisEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArchfiendsGhastlyGlitch, ArchfiendsGhastlyGlitchEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArchfiendPlaytime, ArchfiendPlaytimeEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArchfiendsFervor, ArchfiendsFervorEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionalBarrier, DefaultDimensionalBarrier);
            AddExecutor(ExecutorType.Activate, CardId.AntiSpellFragrance, AntiSpellFragranceEffect);
            AddExecutor(ExecutorType.Activate, CardId.RedReboot, RedRebootEffect);
            AddExecutor(ExecutorType.Activate, CardId.RivalryOfWarlords, RivalryOfWarlordsEffect);
            AddExecutor(ExecutorType.Activate, CardId.DragonsMind, DragonsMindEffect);

            // ===== PRIORITY 5: Extra Deck Monster Activations =====
            AddExecutor(ExecutorType.Activate, CardId.OddEyesMeteorburstDragon, OddEyesMeteorburstDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlackRoseDragon, BlackRoseDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.BrambleRoseDragon, BrambleRoseDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.RageRoseWitch, RageRoseWitchEffect);
            AddExecutor(ExecutorType.Activate, CardId.RuddyRoseDragon, RuddyRoseDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.GardenRoseMaiden, GardenRoseMaidenEffect);
            AddExecutor(ExecutorType.Activate, CardId.PeriallisEmpressOfBlossoms, PeriallisEmpressOfBlossomsEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossroseDragon, CrossroseDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.TopologicTrisbaena, TopologicTrisbaenaEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleBurstDragon, TripleBurstDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.WorldGearsOfTheurlogicalDemiurgy, WorldGearsOfTheurlogicalDemiurgyEffect);
            AddExecutor(ExecutorType.Activate, CardId.TopologicBomberDragon, TopologicBomberDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.TopologicZeroboros, TopologicZeroborosEffect);

            // ===== PRIORITY 6: Strategic Extra Deck Summons =====
            AddExecutor(ExecutorType.SpSummon, CardId.OddEyesMeteorburstDragon, OddEyesMeteorburstDragonSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RuddyRoseDragon, RuddyRoseDragonSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BlackRoseDragon, BlackRoseDragonSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BrambleRoseDragon, () => !IsSpecialSummonBlocked() && !ShouldStopExtending());
            AddExecutor(ExecutorType.SpSummon, CardId.RageRoseWitch, () => !IsSpecialSummonBlocked() && !ShouldStopExtending());
            AddExecutor(ExecutorType.SpSummon, CardId.PeriallisEmpressOfBlossoms, () => !IsSpecialSummonBlocked() && !ShouldStopExtending());
            AddExecutor(ExecutorType.SpSummon, CardId.GardenRoseMaiden, () => !IsSpecialSummonBlocked() && !ShouldStopExtending());
            AddExecutor(ExecutorType.SpSummon, CardId.CrossroseDragon, () => !IsSpecialSummonBlocked() && !ShouldStopExtending());
            AddExecutor(ExecutorType.SpSummon, CardId.TopologicTrisbaena, () => !IsSpecialSummonBlocked() && Enemy.GetSpellCount() >= 1);
            AddExecutor(ExecutorType.SpSummon, CardId.TripleBurstDragon, () => !IsSpecialSummonBlocked() && !ShouldStopExtending());
            AddExecutor(ExecutorType.SpSummon, CardId.WorldGearsOfTheurlogicalDemiurgy, () => !IsSpecialSummonBlocked() && Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 3);
            AddExecutor(ExecutorType.SpSummon, CardId.TopologicBomberDragon, () => !IsSpecialSummonBlocked() && Enemy.GetMonsterCount() >= 2);
            AddExecutor(ExecutorType.SpSummon, CardId.TopologicZeroboros, () => !IsSpecialSummonBlocked() && Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 3);

            // ===== PRIORITY 7: Main Deck Special Summons =====
            AddExecutor(ExecutorType.SpSummon, CardId.DoomReginaArchfiend, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.SpSummon, CardId.ArchfiendEmperor, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.SpSummon, CardId.MatadorArchfiend, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.SpSummon, CardId.HighnessArchfiend, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.SpSummon, CardId.RoyalArchfiend, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.SpSummon, CardId.DukeArchfiend, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.SpSummon, CardId.RegenesisArchfiend, () => !IsSpecialSummonBlocked());
            AddExecutor(ExecutorType.SpSummon, CardId.RegenesisSage, () => !IsSpecialSummonBlocked());

            // ===== PRIORITY 8: Normal Summons =====
            AddExecutor(ExecutorType.Summon, CardId.ArchfiendEmperor, ArchfiendEmperorSummon);
            AddExecutor(ExecutorType.Summon, CardId.RoyalArchfiend);
            AddExecutor(ExecutorType.Summon, CardId.HighnessArchfiend);
            AddExecutor(ExecutorType.Summon, CardId.DukeArchfiend);
            AddExecutor(ExecutorType.Summon, CardId.ArchfiendHeiress);
            AddExecutor(ExecutorType.Summon, CardId.RegenesisSage);

            // ===== PRIORITY 9: Backrow Set Commands =====
            AddExecutor(ExecutorType.SpellSet, CardId.ArchfiendUsurpation);
            AddExecutor(ExecutorType.SpellSet, CardId.ArchfiendStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.ArchfiendsGhastlyGlitch);
            AddExecutor(ExecutorType.SpellSet, CardId.ArchfiendsFervor);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);

            // ===== PRIORITY 10: Repositioning =====
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        private bool IsInExtraMZone(ClientCard card)
        {
            return card != null && card.Location == CardLocation.MonsterZone && (card.Sequence == 5 || card.Sequence == 6);
        }

        public override bool OnSelectHand() => true;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _matadorUsed = false;
            _emperorUsed = false;
            _reginaUsed = false;
            _royalUsed = false;
            _dukeUsed = false;
            _heiressUsed = false;
            _highnessUsed = false;
            _regenesisUsed = false;
            _playtimeUsed = false;
            _glitchUsed = false;
            _fervorUsed = false;
            _strategyUsed = false;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.MatadorArchfiend) _matadorUsed = true;
                if (card.Id == CardId.ArchfiendEmperor) _emperorUsed = true;
                if (card.Id == CardId.DoomReginaArchfiend) _reginaUsed = true;
                if (card.Id == CardId.RoyalArchfiend) _royalUsed = true;
                if (card.Id == CardId.DukeArchfiend) _dukeUsed = true;
                if (card.Id == CardId.ArchfiendHeiress) _heiressUsed = true;
                if (card.Id == CardId.HighnessArchfiend) _highnessUsed = true;
                if (card.Id == CardId.RegenesisArchfiend) _regenesisUsed = true;
                if (card.Id == CardId.ArchfiendPlaytime) _playtimeUsed = true;
            }
        }

        protected override bool IsBoardStrongEnough()
        {
            int disruptions = CountDisruptions();
            bool hasBoss = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && BossMonsters.Contains(m.Id));
            if (hasBoss && disruptions >= 2) return true;
            if (disruptions >= 3) return true;

            int atk = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            if (atk >= 6000) return true;

            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
            {
                if (ResourcePlan.NibiruCheckpoint(Duel.Turn, Enemy.Hand.Count, Bot.GetMonsterCount(), HasNegateOnField()) || OpponentHasActiveNegator())
                    return true;
            }
            return base.ShouldStopExtending();
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c))
            {
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup())
                    return int.MaxValue;
                return 950;
            }
            if (c.IsCode(CardId.ArchfiendHeiress)) return 10; // Best send target to trigger search!
            if (c.IsCode(CardId.RegenesisSage)) return 50;
            if (c.IsCode(CardId.DukeArchfiend)) return 60;
            if (c.IsCode(CardId.RoyalArchfiend)) return 80;
            return 100;
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            var withoutFieldAces = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (withoutFieldAces.Count >= min)
            {
                var sorted = withoutFieldAces.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }
            var allSorted = cards.Where(c => c != null).OrderBy(c => GetMaterialPriority(c)).ToList();
            return allSorted.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            var result = new List<ClientCard>();
            var handMats = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
            var fieldNonAce = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && !IsAceCard(c)).ToList();
            var fieldAce = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && IsAceCard(c)).ToList();

            foreach (var c in handMats) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max)
                foreach (var c in fieldNonAce) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max)
                foreach (var c in fieldAce) { result.Add(c); if (result.Count >= max) break; }

            if (result.Count >= min) return result;
            return base.OnSelectFusionMaterial(cards, min, max);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            if (Card != null && Card.Id == CardId.RegenesisArchfiend && min == 0 && cards.Count > 0)
            {
                var bestReveal = cards.FirstOrDefault(c => c.Attack == 2500 || c.Defense == 2500) ?? cards.OrderByDescending(c => c.Attack).First();
                return new[] { bestReveal };
            }

            // Material selection hint (protect field Aces)
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();

                if (cancelable)
                {
                    var nonFieldAces = sorted.Where(c => !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
                    if (nonFieldAces.Count < min) return null;
                    return Util.CheckSelectCount(nonFieldAces, cards, min, max);
                }
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            // Search priority hint
            if (hint == 506)
            {
                var preferred = new List<int> {
                    CardId.MatadorArchfiend,
                    CardId.RitualOfTheMatador,
                    CardId.HighnessArchfiend,
                    CardId.RoyalArchfiend,
                    CardId.DukeArchfiend,
                    CardId.ArchfiendHeiress,
                    CardId.ThroneOfTheArchfiends,
                    CardId.ArchfiendPlaytime,
                    CardId.ArchfiendsGhastlyGlitch,
                    CardId.ArchfiendStrategy,
                    CardId.RegenesisArchfiend
                };

                var matches = cards.Where(c => c != null && preferred.Contains(c.Id))
                    .OrderBy(c => preferred.IndexOf(c.Id))
                    .ToList();

                if (matches.Count >= min) return matches.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
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
                candidates.AddRange(Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)));
            if (!monstersOnly)
                candidates.AddRange(Enemy.GetSpells().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)));

            var threat = candidates.FirstOrDefault(c => highThreatCardIds.Contains(c.Id));
            if (threat != null) return threat;

            var highestAtk = candidates.Where(c => c.IsMonster()).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (highestAtk != null && highestAtk.Attack >= 2000) return highestAtk;

            return candidates.FirstOrDefault() ?? Util.GetBestEnemyCard(canBeTarget: true);
        }

        private bool ArchfiendEmperorSummon()
        {
            if (Duel.Phase == DuelPhase.Main2) return false;
            bool canPop = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasSetcode(0x45) && c.Id != CardId.ArchfiendEmperor) && GetHighThreatEnemyCard() != null;
            if (canPop)
            {
                DecisionTracer.Trace("ArchfiendEmperor", "Normal Summoning without tribute to pop high-threat enemy card");
                return true;
            }
            if (Enemy.GetMonsterCount() > 0 && !Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 2000))
            {
                return true;
            }
            return false;
        }

        // ============================================================
        // ACTIVATED HANDLERS
        // ============================================================

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool CrossoutDesignatorEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            int code = LastChainCard.Id;
            int alias = LastChainCard.Alias;
            if (alias != 0 && alias - code < 10) code = alias;
            if (code == 0) return false;
            if (GetRemainingCount(code) > 0)
            {
                DecisionTracer.TraceActivate("CrossoutDesignator", $"Declaring {code} against opponent chain");
                AI.SelectAnnounceID(code);
                return true;
            }
            return false;
        }

        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool DoomReginaArchfiendEffect()
        {
            if (_reginaUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (IsSpecialSummonBlocked() || Bot.GetMonsterCount() >= 5) return false;
                var target = Bot.Graveyard
                    .Where(c => c != null && c.IsMonster() && c.HasSetcode(0x45) && c.Id != CardId.DoomReginaArchfiend && c.IsCanRevive())
                    .OrderByDescending(c => BossMonsters.Contains(c.Id) ? 2 : 1)
                    .FirstOrDefault();
                if (target != null)
                {
                    _reginaUsed = true;
                    DecisionTracer.TraceActivate("DoomReginaArchfiend", $"Reviving {target.Name} from GY");
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool ArchfiendEmperorEffect()
        {
            if (_emperorUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Banish 1 Archfiend card from hand or GY to destroy 1 card on field
                var banishCost = Bot.Graveyard.Concat(Bot.Hand)
                    .FirstOrDefault(c => c != null && c.HasSetcode(0x45) && c != Card && c.Location != CardLocation.Removed);
                if (banishCost != null)
                {
                    var target = GetHighThreatEnemyCard() ?? Enemy.GetMonsters().Concat(Enemy.GetSpells()).FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget());
                    if (target != null)
                    {
                        _emperorUsed = true;
                        DecisionTracer.TraceActivate("ArchfiendEmperor", $"Banishing {banishCost.Name} to destroy {target.Name}");
                        AI.SelectCard(banishCost);
                        AI.SelectNextCard(target);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool MatadorArchfiendEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            if (_matadorUsed) return false;
            _matadorUsed = true;
            DecisionTracer.TraceActivate("MatadorArchfiend", "On-Summon: Searching Royal Archfiend & Archfiend Playtime");
            AI.SelectCard(new[] {
                CardId.RoyalArchfiend,
                CardId.ArchfiendPlaytime,
                CardId.DukeArchfiend,
                CardId.HighnessArchfiend,
                CardId.ArchfiendHeiress,
                CardId.ThroneOfTheArchfiends,
                CardId.ArchfiendsFervor
            });
            AI.SelectNextCard(new[] {
                CardId.RegenesisArchfiend,
                CardId.RegenesisSage,
                CardId.ArchfiendStrategy,
                CardId.ArchfiendUsurpation
            });
            return true;
        }

        private bool RoyalArchfiendEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (_royalUsed) return false;
            _royalUsed = true;
            DecisionTracer.TraceActivate("RoyalArchfiend", "Searching Highness or Throne");
            if (GetRemainingCount(CardId.HighnessArchfiend) > 0)
            {
                AI.SelectCard(CardId.HighnessArchfiend);
            }
            else if (GetRemainingCount(CardId.ThroneOfTheArchfiends) > 0)
            {
                AI.SelectCard(CardId.ThroneOfTheArchfiends);
            }
            else
            {
                AI.SelectCard(CardId.DukeArchfiend, CardId.ArchfiendHeiress);
            }
            return true;
        }

        private bool DukeArchfiendEffect()
        {
            if (_dukeUsed) return false;
            var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Level > 0 && !IsAceCard(c))
                ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.DukeArchfiend));
            if (target != null)
            {
                _dukeUsed = true;
                DecisionTracer.TraceActivate("DukeArchfiend", "Adjusting level & milling Archfiend Heiress to trigger search");
                AI.SelectCard(target);
                AI.SelectNextCard(CardId.ArchfiendHeiress, CardId.RegenesisSage, CardId.RoyalArchfiend);
                return true;
            }
            return false;
        }

        private bool ArchfiendHeiressEffect()
        {
            if (_heiressUsed) return false;
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Grave) return false;
            _heiressUsed = true;
            DecisionTracer.TraceActivate("ArchfiendHeiress", "GY trigger: Searching best missing Archfiend card");
            if (GetRemainingCount(CardId.MatadorArchfiend) > 0 && Bot.Hand.Any(c => c != null && c.IsCode(CardId.RitualOfTheMatador)))
            {
                AI.SelectCard(CardId.MatadorArchfiend);
            }
            else if (GetRemainingCount(CardId.HighnessArchfiend) > 0)
            {
                AI.SelectCard(CardId.HighnessArchfiend);
            }
            else if (GetRemainingCount(CardId.RoyalArchfiend) > 0)
            {
                AI.SelectCard(CardId.RoyalArchfiend);
            }
            else if (GetRemainingCount(CardId.ThroneOfTheArchfiends) > 0)
            {
                AI.SelectCard(CardId.ThroneOfTheArchfiends);
            }
            else if (GetRemainingCount(CardId.ArchfiendPlaytime) > 0)
            {
                AI.SelectCard(CardId.ArchfiendPlaytime);
            }
            else
            {
                AI.SelectCard(CardId.DukeArchfiend, CardId.ArchfiendsGhastlyGlitch);
            }
            return true;
        }

        private bool HighnessArchfiendEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (_highnessUsed) return false;
            if (Card.Location != CardLocation.MonsterZone) return false;
            bool hasGYTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasSetcode(0x45) && c != Card);
            if (!hasGYTarget) return false;
            
            _highnessUsed = true;
            DecisionTracer.TraceActivate("HighnessArchfiend", "Banishing GY Fiend to search 2 Archfiend cards");
            AI.SelectCard(new[] {
                CardId.RoyalArchfiend,
                CardId.DukeArchfiend,
                CardId.ArchfiendHeiress,
                CardId.ThroneOfTheArchfiends,
                CardId.ArchfiendsFervor
            });
            AI.SelectNextCard(new[] {
                CardId.RegenesisArchfiend,
                CardId.RegenesisSage,
                CardId.ArchfiendStrategy
            });
            return true;
        }

        private bool RegenesisArchfiendEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (_regenesisUsed) return false;
            _regenesisUsed = true;
            DecisionTracer.TraceActivate("RegenesisArchfiend", "Searching Regenesis or Sage");
            if (GetRemainingCount(CardId.Regenesis) > 0)
            {
                AI.SelectCard(CardId.Regenesis);
            }
            else if (GetRemainingCount(CardId.RegenesisSage) > 0)
            {
                AI.SelectCard(CardId.RegenesisSage);
            }
            return true;
        }

        private bool RegenesisSageEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (GetRemainingCount(CardId.RegenesisArchfiend) > 0)
            {
                DecisionTracer.TraceActivate("RegenesisSage", "Searching Regenesis Archfiend");
                AI.SelectCard(CardId.RegenesisArchfiend);
            }
            return true;
        }

        private bool ThroneOfTheArchfiendsEffect()
        {
            bool canRitual = Bot.Hand.Concat(Bot.ExtraDeck).Concat(Bot.Graveyard).Concat(Bot.Banished)
                .Any(c => c != null && c.IsMonster() && c.HasType(CardType.Ritual) && c.HasSetcode(0x45) && (c.Location == CardLocation.Hand || c.Location == CardLocation.Grave || c.Location == CardLocation.Removed || (c.Location == CardLocation.Extra && c.IsFaceup())));

            if (canRitual)
            {
                DecisionTracer.TraceActivate("ThroneOfTheArchfiends", "Activating Field Spell for Ritual & search");
                return true;
            }
            return false;
        }

        private bool RitualOfTheMatadorEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                bool hasHandTarget = Bot.Hand.Any(c => c != null && c.IsMonster() && c.HasSetcode(0x45) && !c.HasType(CardType.Ritual));
                if (hasHandTarget)
                {
                    DecisionTracer.TraceActivate("RitualOfTheMatador", "GY effect: Recovering Fiend to hand");
                    return true;
                }
                return false;
            }
            bool canRitual = Bot.Hand.Any(c => c != null && c.IsCode(CardId.MatadorArchfiend));
            if (canRitual)
            {
                DecisionTracer.TraceActivate("RitualOfTheMatador", "Ritual Summoning Matador Archfiend");
                AI.SelectCard(CardId.MatadorArchfiend);
                AI.SelectNextCard(CardId.ArchfiendHeiress, CardId.RoyalArchfiend, CardId.DukeArchfiend, CardId.RegenesisSage);
                return true;
            }
            return false;
        }

        private bool ArchfiendUsurpationEffect()
        {
            bool canSetTrap = Bot.Deck.Concat(Bot.Graveyard)
                .Any(c => c != null && c.IsTrap() && c.HasSetcode(0x45));

            if (canSetTrap && Duel.Player == 0)
            {
                DecisionTracer.TraceActivate("ArchfiendUsurpation", "Setting Archfiend Trap from Deck");
                AI.SelectCard(CardId.ArchfiendsGhastlyGlitch, CardId.ArchfiendsFervor);
                return true;
            }

            bool canRitual = Bot.Hand.Concat(Bot.ExtraDeck)
                .Any(c => c != null && c.IsMonster() && c.HasType(CardType.Ritual) && c.HasSetcode(0x45) && (c.Location == CardLocation.Hand || (c.Location == CardLocation.Extra && c.IsFaceup())));

            return canRitual;
        }

        private bool ArchfiendStrategyEffect()
        {
            if (_strategyUsed) return false;
            if (Card.Location != CardLocation.Hand) return false;

            bool hasCost = Bot.Hand.Concat(Bot.GetMonsters())
                .Any(c => c != null && (c.HasRace(CardRace.Fiend) || c.HasSetcode(0x45)) && c != Card);

            if (hasCost)
            {
                _strategyUsed = true;
                DecisionTracer.TraceActivate("ArchfiendStrategy", "Sending Fiend (prioritizing Heiress) to Draw 2");
                AI.SelectCard(CardId.ArchfiendHeiress, CardId.RegenesisSage, CardId.RoyalArchfiend, CardId.DukeArchfiend);
                return true;
            }
            return false;
        }

        private bool RegenesisEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                DecisionTracer.TraceActivate("Regenesis", "Reviving / searching Regenesis monsters");
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.IsCode(CardId.RegenesisArchfiend, CardId.RegenesisSage) && c.IsCanRevive());
                if (hasTarget)
                {
                    DecisionTracer.TraceActivate("Regenesis", "GY effect: Reviving Regenesis monster");
                    return true;
                }
            }
            return false;
        }

        private bool ArchfiendsGhastlyGlitchEffect()
        {
            if (_glitchUsed) return false;
            if (Card.Location != CardLocation.SpellZone) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (!Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Fiend))) return false;

            var target = GetHighThreatEnemyCard();
            if (target != null)
            {
                _glitchUsed = true;
                DecisionTracer.TraceActivate("ArchfiendsGhastlyGlitch", $"Popping high-threat {target.Name} & milling Archfiend Heiress");
                AI.SelectCard(target);
                AI.SelectNextCard(CardId.ArchfiendHeiress, CardId.HighnessArchfiend, CardId.RoyalArchfiend);
                return true;
            }
            return false;
        }

        private bool ArchfiendPlaytimeEffect()
        {
            if (IsSpecialSummonBlocked() || _playtimeUsed) return false;
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            
            bool hasTarget = GetRemainingCount(CardId.RoyalArchfiend) > 0
                || GetRemainingCount(CardId.DukeArchfiend) > 0
                || GetRemainingCount(CardId.HighnessArchfiend) > 0
                || GetRemainingCount(CardId.RegenesisArchfiend) > 0
                || GetRemainingCount(CardId.RegenesisSage) > 0;
            if (hasTarget)
            {
                _playtimeUsed = true;
                DecisionTracer.TraceActivate("ArchfiendPlaytime", "Special Summoning Archfiend from Deck");
                AI.SelectCard(CardId.RoyalArchfiend, CardId.HighnessArchfiend, CardId.DukeArchfiend, CardId.RegenesisArchfiend);
                return true;
            }
            return false;
        }

        private bool ArchfiendsFervorEffect()
        {
            if (_fervorUsed) return false;
            if (Card.Location != CardLocation.SpellZone) return false;

            bool hasRitualInEMZ = Bot.GetMonsters()
                .Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Ritual) && c.HasSetcode(0x45) && IsInExtraMZone(c));

            if (!hasRitualInEMZ) return false;

            var oppCards = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && IsViableEffectTarget(c))
                .OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : (c.IsMonster() ? c.Attack : 1000))
                .ToList();

            if (oppCards.Count > 0)
            {
                _fervorUsed = true;
                DecisionTracer.TraceActivate("ArchfiendsFervor", $"Negating {oppCards[0].Name}");
                AI.SelectCard(oppCards[0]);
                return true;
            }
            return false;
        }

        private bool PrePreparationOfRitesEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            bool canSearch = GetRemainingCount(CardId.RitualOfTheMatador) > 0 && GetRemainingCount(CardId.MatadorArchfiend) > 0;
            if (canSearch)
            {
                DecisionTracer.TraceActivate("PrePreparationOfRites", "Searching Ritual of the Matador & Matador Archfiend");
                AI.SelectCard(CardId.RitualOfTheMatador);
                AI.SelectNextCard(CardId.MatadorArchfiend);
                return true;
            }
            return false;
        }

        private bool PotOfExtravaganceEffect()
        {
            if (Duel.Phase != DuelPhase.Main1) return false;
            return Bot.ExtraDeck.Count >= 6;
        }

        private bool PotOfProsperityEffect()
        {
            if (Duel.Phase != DuelPhase.Main1) return false;
            return Bot.ExtraDeck.Count >= 3;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (Duel.Player == 0 && Duel.LastChainPlayer == 1)
            {
                if (Enemy.GetMonsterCount() > 0 && CanDealLethal())
                {
                    DecisionTracer.TraceActivate("TripleTacticsTalent", "Taking control of opponent monster for lethal");
                    AI.SelectOption(1);
                    var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsViableEffectTarget(m));
                    if (target != null) AI.SelectCard(target);
                }
                else
                {
                    DecisionTracer.TraceActivate("TripleTacticsTalent", "Drawing 2 cards");
                    AI.SelectOption(0);
                }
                return true;
            }
            return false;
        }

        private bool TripleTacticsThrustEffect()
        {
            return Duel.Player == 0 && Duel.LastChainPlayer == 1;
        }

        private bool ForbiddenDropletEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1;
        }

        private bool BookOfEclipseEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && Enemy.GetMonsterCount() >= 2;
        }

        private bool EvenlyMatchedEffect()
        {
            int ourCount = Bot.GetFieldCount();
            if (Card.Location == CardLocation.Hand) ourCount += 1;
            int enemyCount = Enemy.GetFieldCount();
            if (enemyCount <= ourCount) return false;

            if (Bot.GetFieldCount() == 0 && Enemy.GetFieldCount() > 0) return true;
            if (enemyCount >= ourCount + 3) return true;

            return false;
        }

        private bool AntiSpellFragranceEffect() => Duel.Player == 1;
        private bool RedRebootEffect() => Duel.LastChainPlayer == 1 && LastChainCard != null && LastChainCard.IsTrap();
        private bool RivalryOfWarlordsEffect() => Duel.Player == 1;

        private bool DragonsMindEffect()
        {
            if (Card.Location == CardLocation.SpellZone) return true;
            if (Card.Location == CardLocation.Grave)
            {
                bool hasMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.BaseAttack == 2500 || c.BaseDefense == 2500));
                return hasMonster && Bot.LifePoints > 3500;
            }
            return false;
        }

        private bool NibiruEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Enemy.GetMonsterCount() >= 2 || Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 2000);
        }

        // ── Extra Deck Effect Handlers ──

        private bool OddEyesMeteorburstDragonEffect()
        {
            var pTarget = Bot.SpellZone.FirstOrDefault(c => c != null && c.Location == CardLocation.PendulumZone && c.IsMonster());
            if (pTarget != null)
            {
                DecisionTracer.TraceActivate("OddEyesMeteorburstDragon", $"SS {pTarget.Name} from Pendulum Zone to Monster Zone");
                AI.SelectCard(pTarget);
                return true;
            }
            return false;
        }

        private bool BlackRoseDragonEffect()
        {
            int ourBossCount = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && IsAceCard(m));
            if (ourBossCount >= 2 && !CanDealLethal()) return false;

            int enemyFieldCount = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            bool hasHighThreat = GetHighThreatEnemyCard() != null;
            if (enemyFieldCount >= 2 || hasHighThreat)
            {
                DecisionTracer.TraceActivate("BlackRoseDragon", "Nuking entire field to clear enemy threats!");
                return true;
            }
            return false;
        }

        private bool BrambleRoseDragonEffect() => Card.Location == CardLocation.MonsterZone;
        private bool RageRoseWitchEffect() => Card.Location == CardLocation.MonsterZone;

        private bool RuddyRoseDragonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Enemy.Graveyard.Count >= 3 || (Duel.Player == 0 && CanDealLethal()))
            {
                DecisionTracer.TraceActivate("RuddyRoseDragon", "Banishing all cards in all Graveyards");
                return true;
            }
            return false;
        }

        private bool GardenRoseMaidenEffect() => Card.Location == CardLocation.MonsterZone;
        private bool PeriallisEmpressOfBlossomsEffect() => Card.Location == CardLocation.MonsterZone;
        private bool CrossroseDragonEffect() => Card.Location == CardLocation.MonsterZone || Card.Location == CardLocation.Grave;

        private bool TopologicTrisbaenaEffect() => Enemy.GetSpellCount() > 0;
        private bool TripleBurstDragonEffect() => Card.Location == CardLocation.MonsterZone;

        private bool WorldGearsOfTheurlogicalDemiurgyEffect()
        {
            int enemyFieldCount = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            return enemyFieldCount >= 3;
        }

        private bool TopologicBomberDragonEffect() => Enemy.GetMonsterCount() >= 2;

        private bool TopologicZeroborosEffect()
        {
            int enemyFieldCount = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            int ourFieldCount = Bot.GetMonsterCount() + Bot.GetSpellCount();
            return enemyFieldCount >= ourFieldCount + 3;
        }

        // ── Extra Deck SpSummon Checks ──

        private bool OddEyesMeteorburstDragonSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasPScale = Bot.SpellZone.Any(c => c != null && c.Location == CardLocation.PendulumZone && c.IsMonster());
            return hasPScale && !ShouldStopExtending();
        }

        private bool RuddyRoseDragonSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return (Enemy.Graveyard.Count >= 3 || CanDealLethal()) && !ShouldStopExtending();
        }

        private bool BlackRoseDragonSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int ourFieldCount = Bot.GetMonsterCount() + Bot.GetSpellCount();
            int enemyFieldCount = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            bool hasOurBoss = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && IsAceCard(m));
            if (hasOurBoss) return false;

            return enemyFieldCount >= ourFieldCount + 2;
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            if (IsAceCard(card)) return true;

            if (card.HasType(CardType.Link | CardType.Synchro | CardType.Xyz))
            {
                var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
                if (activeAces.Count > 0)
                {
                    if (CanDealLethal() || OpponentHasActiveNegator())
                        return true;

                    DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning generic Extra Deck {card.Name} skipped to protect Boss card {activeAces.First().Name}");
                    return false;
                }
            }
            return true;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.MatadorArchfiend)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence; // Matador is battle & effect immune in defense
            }
            if (BossMonsters.Contains(cardId) && cardId != CardId.MatadorArchfiend)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        private new bool DefaultMonsterRepos()
        {
            if (Card == null) return false;
            if (Card.Id == CardId.MatadorArchfiend)
            {
                if (Card.IsDefense() && CanDealLethal()) return true;
                if (Card.IsAttack() && !CanDealLethal()) return true;
            }
            return base.DefaultMonsterRepos();
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
}
