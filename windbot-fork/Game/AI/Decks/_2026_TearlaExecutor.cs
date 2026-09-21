// =========================================================================================
// CARD AUDIT โ€” 2026_Tearla (Fiendsmith-Brilliant-Tearlaments)
// =========================================================================================
// | Card Name                     | Type        | OPT? | HOPT? | Cost / Target           | Effect Summary                              | Activate When                               | NEVER When / Risks                            |
// |-------------------------------|-------------|------|-------|-------------------------|---------------------------------------------|---------------------------------------------|-----------------------------------------------|
// | Tearlaments Reinoheart        | Monster L4  | Yes  | Yes   | NS/SS: Dump; GY: Disc   | NS/SS: Dump Tear; GY self-revive + discard  | Main Phase starter; Trigger Tear GY Fusion  | Hand empty (cannot discard to revive)         |
// | Tearlaments Scheiren          | Monster L4  | Yes  | Yes   | Send 1 mon from hand    | Hand SS + send 1 + mill 3; GY Fusion summon | Main Phase starter/extender; GY Fusion      | No discard fodder available                   |
// | Tearlaments Havnis            | Monster L3  | Yes  | Yes   | Quick: None             | Opp field eff: Hand SS + mill 3; GY Fusion  | Opp activates eff on field; GY Fusion       | Field full / Already used HOPT                |
// | Tearlaments Kashtira          | Monster L7  | Yes  | Yes   | Quick: Banish 1 Kasht/Tr| Hand Quick SS; NS/SS mill 3; GY mill 2     | Main Phase extend; Mill 3 (Our deck!)       | Banish essential combo piece / Mill opp deck  |
// | Tearlaments Kitkallos         | Fusion L5   | Yes  | Yes   | Field: Pop 1 mon        | On SS: search/dump Tear; Target pop -> SS;  | On Fusion / Main Phase setup & mill 5       | Extra Deck locked / Already used HOPT         |
// |                               |             |      |       |                         | Sent to GY: MILL 5!                         |                                             |                                               |
// | Tearlaments Rulkallos         | Fusion L8   | Yes  | Yes   | Quick: Send Tear mon    | SS negate Quick effect; GY self-revive      | Opponent Special Summons; Float back        | No Tear fodder / Battle phase under 3000 ATK  |
// | Tearlaments Kaleido-Heart     | Fusion L9   | Yes  | Yes   | Target 1 card           | On SS/Aqua dump: Spin 1 card; GY revive     | On Summon / Aqua sent to GY; Spin threat    | Opp field empty / Already used HOPT           |
// | Fiendsmith Engraver           | Monster L6  | Yes  | Yes   | Hand: Disc; GY: Shuf    | Discard -> Search Tract; GY Shuf LIGHT SS   | In Hand / In GY (Rank 6 setup)              | No LIGHT Fiend in GY to shuffle               |
// | Fiendsmith's Tract            | Spell       | Yes  | Yes   | Disc 1 card             | Search LIGHT Fiend + Discard; GY Fusion     | Main Phase Fiendsmith starter (search Lurrie)| No discardable card in hand                   |
// | Lacrima the Crimson Tears     | Monster L4  | Yes  | Yes   | None                    | NS/SS: Dump Fiendsmith; Opp turn revive Link| On Summon / In GY                           | Engraver already in GY                        |
// | Fabled Lurrie                 | Monster L1  | No   | No    | None                    | Discarded to GY -> Special Summon self      | Discarded by Tract / Scheiren -> Link 1     | Monster zones full                            |
// | Fiendsmith's Requiem          | Link 1      | Yes  | Yes   | Tribute self            | Tribute self -> SS Fiendsmith from Deck     | On Field (LIGHT Fiend used as mat)          | Already used HOPT                             |
// | Fiendsmith's Sequence         | Link 2      | Yes  | Yes   | Shuf GY mats            | Shuffle GY mats -> Fusion Summon Fiend      | Main Phase (Lacrima + Engraver in GY)       | Materials not in GY                           |
// | Fiendsmith's Lacrima          | Fusion L6   | Yes  | Yes   | Target LIGHT Fiend      | On Fusion: Revive Engraver (Rank 6 setup!)  | On Fusion Summon -> Rank 6 Pilgrim Reaper   | Engraver not in GY                            |
// | Fiendsmith's Desirae          | Fusion L9   | Yes  | Yes   | Send equipped Link      | Send equipped Link -> Negate up to Link rate| On Field / Quick multi-negate               | No equipped card                              |
// | Fiendsmith's Agnumday         | Link 3      | Yes  | Yes   | Target LIGHT Fiend      | Quick revive non-Link LIGHT Fiend & equip   | Battle / OTK push                           | Target not in GY                              |
// | Moon of the Closed Heaven     | Link 2      | Yes  | Yes   | 2 Effect monsters       | Generic bridge into LIGHT Fiend Link        | When LIGHT Fiend missing for Requiem        | Already have LIGHT Fiend                      |
// | Brilliant Fusion              | ContSpell   | Yes  | Yes   | Send from Deck          | Send Gem-Knight + Aqua from Deck to fuse    | Main Phase ignition -> Amethyst + Tear mill | Already used HOPT                             |
// | Gem-Knight Quartz             | Monster L4  | Yes  | Yes   | Disc (Locks Extra Deck!)| Discard -> Set Continuous Fusion Spell      | Discard fodder only (DO NOT ACTIVATE FROM H)| Locks Extra Deck except Gem-Knights!          |
// | Gem-Knight Nepyrim            | Monster L4  | Yes  | Yes   | None                    | Sent to GY -> Add Gem-Knight card           | Sent to GY by Brilliant Fusion              | Already added                                 |
// | Gem-Knight Amethyst           | Fusion L7   | No   | No    | None                    | Sent field->GY -> Bounce all Set S/T        | Fusion target for Brilliant Fusion; Aqua mat| Opp set S/T empty                             |
// | Pilgrim Reaper                | Xyz R6      | Yes  | No    | Detach 1 material       | Detach 1 -> Both players mill 5 cards       | Main Phase mill ignition -> Trigger Tears   | Deck < 10 cards                               |
// | The Undying Legion            | Xyz R7      | Yes  | No    | Overlay on Rank 6 Zombie| 2700 ATK Boss; Rank-up on Pilgrim Reaper    | After Pilgrim Reaper mills 5 cards          | Pilgrim Reaper not on field                   |
// | Number 60: Dugares            | Xyz R4      | Yes  | Yes   | Detach 2 materials      | Draw 2 discard 1 / Revive monster           | Main Phase extension / hand refresh         | Hand full / Deck low                          |
// | Evilswarm Exciton Knight      | Xyz R4      | No   | No    | Detach 1 material       | Destroy all other cards on field            | Going second when opponent has card advantage| We have advantage                             |
// | Kashtira Fenrir               | Monster L7  | Yes  | Yes   | None                    | Inherent SS; Search Kashtira; Face-down ban | Empty field / Opp monster effect response   | Control monsters                              |
// | Fairy Tail - Snow             | Monster L4  | No   | No    | Banish 7 cards          | Quick SS from GY; On SS Book of Moon        | Opp turn disruption / Battle Phase protect  | Banishing combo pieces (< 7 non-essential)    |
// | Keldo the Sacred Protector    | Monster L4  | Yes  | Yes   | Banish self from GY     | Quick GY: Shuffle up to 3 cards to Deck     | Opp GY chokepoint / Self recycle Kitkallos  | Opp GY empty & deck healthy                   |
// | Mudora the Sword Oracle       | Monster L4  | Yes  | Yes   | Banish self from GY     | Quick GY: Shuffle up to 3 cards to Deck     | Opp GY chokepoint / Self recycle Kitkallos  | Opp GY empty & deck healthy                   |
// | Instant Fusion                | Normal Spell| Yes  | No    | 1000 LP                 | Special Summon Kitkallos from Extra Deck    | Main Phase starter -> Search/Mill 5         | LP <= 1000                                    |
// | Tearlaments Scream            | ContSpell   | Yes  | Yes   | None                    | On Summon mill 3 + -500 ATK; GY search Trap | Monster summoned / Sent to GY               | Deck <= 3 cards                               |
// | Tearlaments Sulliek           | ContTrap    | Yes  | Yes   | Send 1 mon from field   | Negate monster eff + send Tear; GY search   | Opp monster eff / Sent to GY                | No Tear on field / Already used HOPT          |
// | Tearlaments Cryme             | CounterTr   | Yes  | Yes   | Send 1 mon from hand    | Omni-negate & shuffle + send mon; GY recycle| Opp card/eff activation; protect board      | No mon in hand / No Tear on field             |
// | Tearlaments Heartbeat         | Quick-Play  | Yes  | Yes   | Send 1 card from hand   | Spin opp S/T into deck; GY recycle Trap     | Opp backrow removal / Floodgate pop         | Hand empty                                    |
// | S:P Little Knight             | Link 2      | Yes  | Yes   | None                    | On Link: Banish 1 card; Quick double banish | On Link Summon / Opponent activates effect  | Only Ace on board                             |
// | Maxx "C" / Mulcharmy Fuwalos  | Handtraps   | Yes  | Yes   | Discard from hand       | Draw cards on opponent Special Summons      | Opponent turn summon response               | Under Droll / Controlling cards for Fuwalos   |
// | Ash Blossom / Droll & Lock    | Handtraps   | Yes  | Yes   | Discard from hand       | Negate search/draw; Lock deck additions     | Opponent chokepoint activation              | Already resolved / Chain blocked              |
// =========================================================================================
// STRATEGY PLAYBOOK:
// Route A (Reinoheart Starter):
//   Normal Summon Reinoheart -> Dump Scheiren/Havnis -> Fuse Kitkallos -> Search Sulliek ->
//   Kitkallos pops herself to SS Tear from GY -> Mill 5 -> Fuse Rulkallos -> Set Sulliek/Cryme.
// Route B (Fiendsmith Engine):
//   Engraver discards to search Tract -> Tract searches Lurrie & discards Lurrie -> Lurrie SS ->
//   Link Requiem -> Requiem tributes to SS Lacrima -> Lacrima dumps Engraver ->
//   Engraver shuffles Requiem to SS self -> Link Sequence -> Sequence fuses Fiendsmith's Lacrima ->
//   Fiendsmith's Lacrima revives Engraver -> Overlay 2x Lv6 into Pilgrim Reaper -> Detach 1 & Mill 5 ->
//   Rank up into The Undying Legion (2700 ATK)!
// Route C (Brilliant Fusion):
//   Brilliant Fusion dumps Nepyrim + Tear Aqua (Scheiren/Havnis) -> Fuse Gem-Knight Amethyst ->
//   Scheiren/Havnis fuses Kitkallos from GY -> Nepyrim adds Quartz/Brilliant -> Kitkallos pops self mill 5.
// Route D (Instant Fusion):
//   Instant Fusion pays 1000 LP -> SS Kitkallos -> Search Sulliek/Reino -> Pop self mill 5.
// =========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Tearla", "2026_Tearla")]
    public class _2026_TearlaExecutor : ModernExecutor
    {
        public class CardId
        {
            // ── Tearlaments Core ──
            public const int TearlamentsReinoheart = 73956664;
            public const int TearlamentsScheiren = 572850;
            public const int TearlamentsHavnis = 37961969;
            public const int TearlamentsKashtira = 4928565;
            public const int TearlamentsKitkallos = 92731385;
            public const int TearlamentsRulkallos = 84330567;
            public const int TearlamentsKaleidoHeart = 28226490;
            public const int TearlamentsScream = 6767771;
            public const int TearlamentsHeartbeat = 60362066;
            public const int TearlamentsSulliek = 74920585;
            public const int TearlamentsCryme = 1329620;

            // ── Fiendsmith Engine ──
            public const int FiendsmithEngraver = 60764609;
            public const int LacrimaTheCrimsonTears = 28803166;
            public const int FabledLurrie = 97651498;
            public const int FiendsmithsTract = 98567237;
            public const int FiendsmithsRequiem = 2463794;
            public const int FiendsmithsSequence = 49867899;
            public const int FiendsmithsLacrima = 46640168;
            public const int FiendsmithsDesirae = 82135803;
            public const int FiendsmithsAgnumday = 32991300;
            public const int MoonOfTheClosedHeaven = 71818935;

            // ── Gem-Knight Engine ──
            public const int BrilliantFusion = 7394770;
            public const int GemKnightNepyrim = 51831560;
            public const int GemKnightQuartz = 35622739;
            public const int GemKnightAmethyst = 71616908;

            // ── Ishizu Shufflers & Techs ──
            public const int KeldoTheSacredProtector = 63542003;
            public const int MudoraTheSwordOracle = 99937011;
            public const int FairyTailSnow = 55623480;
            public const int KashtiraFenrir = 32909498;
            public const int InstantFusion = 1845204;

            // ── Extra Deck Xyz & Links ──
            public const int PilgrimReaper = 45742626;
            public const int Number60Dugares = 66011101;
            public const int EvilswarmExcitonKnight = 46772449;
            public const int TheUndyingLegion = 43355214;
            public const int SPLittleKnight = 29301450;

            // ── Handtraps ──
            public const int MaxxC = 23434538;
            public const int AshBlossom = 14558127;
            public const int MulcharmyFuwalos = 42141493;
            public const int DrollAndLockBird = 94145021;
        }

        // Standard OCG Hint IDs
        private const long HINT_SELECT_FACEUP = 500;
        private const long HINT_SELECT_TOGRAVE = 501;
        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_REMOVE = 504;
        private const long HINT_SELECT_DISCARD = 504;
        private const long HINT_SELECT_TOHAND = 505;
        private const long HINT_SELECT_TODECK = 506;
        private const long HINT_SELECT_EQUIP = 507;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_FUSION_MAT = 511;
        private const long HINT_SELECT_BANISH = 512;
        private const long HINT_SELECT_POSCHANGE = 518;
        private const long HINT_SELECT_DETACH = 519;
        private const long HINT_SELECT_DISABLE = 552;
        private const long HINT_SELECT_NEGATE = 572;

        // Turn-state tracking (Cleaned & verified HOPTs)
        private bool _engraverHandUsed = false;
        private bool _engraverGYUsed = false;
        private bool _tractUsed = false;
        private bool _requiemUsed = false;
        private bool _sequenceUsed = false;
        private bool _fiendsmithLacrimaUsed = false;
        private bool _lacrimaSummonUsed = false;
        private bool _brilliantFusionUsed = false;
        private bool _gemKnightQuartzUsed = false;
        private bool _reinoheartSummonUsed = false;
        private bool _scheirenHandUsed = false;
        private bool _tearKashtiraSpSummonUsed = false;
        private bool _kitkallosSearchUsed = false;
        private bool _kitkallosSelfPopUsed = false;
        private bool _pilgrimReaperUsed = false;
        private bool _dugaresUsed = false;
        private bool _fenrirSearchUsed = false;
        private bool _keldoUsed = false;
        private bool _mudoraUsed = false;
        private bool _snowSummonedThisTurn = false;
        private bool _maxxCActivatedThisTurn = false;
        private bool _fuwalosActivatedThisTurn = false;

        public _2026_TearlaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ═══════════════════════════════════════════════════════════════
            //  COMBO ROUTER & STRATEGIC REGISTER
            // ═══════════════════════════════════════════════════════════════

            // Route A: Reinoheart -> Kitkallos -> Mill 5 -> Rulkallos
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Reinoheart-Kitkallos-Rulkallos",
                RequiredCards = new List<int> { CardId.TearlamentsReinoheart },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.TearlamentsReinoheart, ActionType = ExecutorType.Summon, Description = "Normal Summon Reinoheart" },
                    new() { CardId = CardId.TearlamentsReinoheart, ActionType = ExecutorType.Activate, Description = "Reinoheart send Tear monster to GY" },
                    new() { CardId = CardId.TearlamentsKitkallos, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Kitkallos" },
                    new() { CardId = CardId.TearlamentsKitkallos, ActionType = ExecutorType.Activate, Description = "Kitkallos search & pop self to mill 5" },
                    new() { CardId = CardId.TearlamentsRulkallos, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Rulkallos Boss" }
                },
                EndBoardScore = 95
            });

            // Route B: Fiendsmith Engine -> Pilgrim Reaper -> Mill 5 -> The Undying Legion
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Fiendsmith-Reaper-Milling",
                RequiredCards = new List<int> { CardId.FiendsmithEngraver },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.FiendsmithEngraver, ActionType = ExecutorType.Activate, Description = "Discard Engraver to search Tract" },
                    new() { CardId = CardId.FiendsmithsTract, ActionType = ExecutorType.Activate, Description = "Tract search Lurrie & discard Lurrie" },
                    new() { CardId = CardId.FabledLurrie, ActionType = ExecutorType.Activate, Description = "Lurrie Special Summon self" },
                    new() { CardId = CardId.FiendsmithsRequiem, ActionType = ExecutorType.SpSummon, Description = "Link Summon Requiem" },
                    new() { CardId = CardId.FiendsmithsRequiem, ActionType = ExecutorType.Activate, Description = "Requiem tribute self to SS Lacrima" },
                    new() { CardId = CardId.LacrimaTheCrimsonTears, ActionType = ExecutorType.Activate, Description = "Lacrima dump Engraver to GY" },
                    new() { CardId = CardId.FiendsmithEngraver, ActionType = ExecutorType.Activate, Description = "Engraver revive from GY" },
                    new() { CardId = CardId.FiendsmithsSequence, ActionType = ExecutorType.SpSummon, Description = "Link Summon Sequence" },
                    new() { CardId = CardId.FiendsmithsSequence, ActionType = ExecutorType.Activate, Description = "Sequence fuse Fiendsmith's Lacrima" },
                    new() { CardId = CardId.FiendsmithsLacrima, ActionType = ExecutorType.Activate, Description = "Fiendsmith's Lacrima revive Engraver" },
                    new() { CardId = CardId.PilgrimReaper, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Pilgrim Reaper (2x Lv6)" },
                    new() { CardId = CardId.PilgrimReaper, ActionType = ExecutorType.Activate, Description = "Pilgrim Reaper detach to Mill 5" },
                    new() { CardId = CardId.TheUndyingLegion, ActionType = ExecutorType.SpSummon, Description = "Rank-up into The Undying Legion (2700 ATK)" }
                },
                EndBoardScore = 98
            });

            // Route C: Brilliant Fusion -> Amethyst + Nepyrim + Tear Aqua -> Kitkallos
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Brilliant-Fusion-Tear-Engine",
                RequiredCards = new List<int> { CardId.BrilliantFusion },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.BrilliantFusion, ActionType = ExecutorType.Activate, Description = "Brilliant Fusion send Nepyrim + Tear Aqua" },
                    new() { CardId = CardId.GemKnightAmethyst, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Amethyst" },
                    new() { CardId = CardId.TearlamentsKitkallos, ActionType = ExecutorType.SpSummon, Description = "GY Fusion Kitkallos" },
                    new() { CardId = CardId.GemKnightNepyrim, ActionType = ExecutorType.Activate, Description = "Nepyrim search Gem-Knight card" },
                    new() { CardId = CardId.TearlamentsKitkallos, ActionType = ExecutorType.Activate, Description = "Kitkallos search & mill 5" }
                },
                EndBoardScore = 92
            });

            // Route D: Instant Fusion -> Free Kitkallos
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Instant-Fusion-Kitkallos",
                RequiredCards = new List<int> { CardId.InstantFusion },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.InstantFusion, ActionType = ExecutorType.Activate, Description = "Instant Fusion pay 1000 LP" },
                    new() { CardId = CardId.TearlamentsKitkallos, ActionType = ExecutorType.SpSummon, Description = "Special Summon Kitkallos" },
                    new() { CardId = CardId.TearlamentsKitkallos, ActionType = ExecutorType.Activate, Description = "Kitkallos search & mill 5" }
                },
                EndBoardScore = 90
            });

            // Planner Registrations
            BaitPlanner.RegisterComboStarters(CardId.TearlamentsReinoheart, CardId.BrilliantFusion, CardId.FiendsmithEngraver, CardId.FiendsmithsTract, CardId.InstantFusion);
            ChainAdvisor.RegisterHighValueTargets(CardId.TearlamentsKitkallos, CardId.TearlamentsRulkallos, CardId.TearlamentsKaleidoHeart, CardId.FiendsmithsDesirae, CardId.PilgrimReaper);
            ResourcePlan.RegisterAceCards(CardId.TearlamentsRulkallos, CardId.TearlamentsKaleidoHeart, CardId.FiendsmithsDesirae, CardId.TheUndyingLegion, CardId.SPLittleKnight, CardId.KashtiraFenrir);

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE (Tiered Priority)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Negates, Handtraps & Interruptions ──
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);

            // In-Field & Trap Disruptions
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsCryme, TearlamentsCrymeEffect);
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsRulkallos, RulkallosNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsSulliek, SulliekActivateOrNegate);
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsKaleidoHeart, KaleidoHeartEffect);
            AddExecutor(ExecutorType.Activate, CardId.KashtiraFenrir, KashtiraFenrirEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsDesirae, DesiraeEffect);

            // Quick GY Disruptions (Snow & Ishizu Shufflers)
            AddExecutor(ExecutorType.Activate, CardId.FairyTailSnow, FairyTailSnowEffect);
            AddExecutor(ExecutorType.Activate, CardId.KeldoTheSacredProtector, KeldoEffect);
            AddExecutor(ExecutorType.Activate, CardId.MudoraTheSwordOracle, MudoraEffect);

            // Havnis Opponent Turn Hand Trigger
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsHavnis, HavnisHandEffect);

            // ── Tier 1: Starters & Ignition Spells ──
            // Kashtira Fenrir Inherent SS (Priority 1 on empty field)
            AddExecutor(ExecutorType.SpSummon, CardId.KashtiraFenrir, FenrirSpSummon);

            // Spells: Instant Fusion & Brilliant Fusion
            AddExecutor(ExecutorType.Activate, CardId.InstantFusion, InstantFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.BrilliantFusion, BrilliantFusionEffect);

            // Fiendsmith Starter Line: Engraver Discard -> Tract -> Lurrie SS
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, EngraverHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsTract, TractEffect);
            AddExecutor(ExecutorType.Activate, CardId.FabledLurrie, LurrieEffect);

            // Tearlaments Reinoheart Normal Summon & Trigger
            AddExecutor(ExecutorType.Summon, CardId.TearlamentsReinoheart, ReinoheartSummon);
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsReinoheart, ReinoheartEffect);

            // Tearlaments Scheiren Hand SS & Mill 3
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsScheiren, ScheirenHandEffect);

            // Tearlaments Kashtira Hand SS & Mill 3
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsKashtira, TearKashtiraEffect);

            // Continuous Spell: Tearlaments Scream
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsScream, ScreamEffect);
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsHeartbeat, HeartbeatEffect);

            // Normal Summon Lurrie as backup starter into Requiem Link
            AddExecutor(ExecutorType.Summon, CardId.FabledLurrie, LurrieSummon);

            // ── Tier 2: GY Triggers & Mill Reactions ──
            AddExecutor(ExecutorType.Activate, CardId.GemKnightNepyrim, NepyrimEffect);
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsKitkallos, KitkallosEffect);

            // GY Fusion triggers for Scheiren & Havnis
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsScheiren, ScheirenGYFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsHavnis, HavnisGYFusionEffect);

            // Engraver GY Revive
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, EngraverGYEffect);

            // ── Tier 3: Extra Deck Link, Fusion & Xyz Ladder ──
            // Fiendsmith Link & Fusion ladder
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsRequiem, RequiemSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsRequiem, RequiemEffect);
            AddExecutor(ExecutorType.Activate, CardId.LacrimaTheCrimsonTears, LacrimaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.MoonOfTheClosedHeaven, MoonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsSequence, SequenceSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsSequence, SequenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsLacrima, FiendsmithLacrimaEffect);

            // Xyz Ladder: Pilgrim Reaper (Mill 5) -> The Undying Legion (2700 ATK Rank-up)
            AddExecutor(ExecutorType.SpSummon, CardId.PilgrimReaper, PilgrimReaperSummon);
            AddExecutor(ExecutorType.Activate, CardId.PilgrimReaper, PilgrimReaperEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.TheUndyingLegion, UndyingLegionSummon);
            AddExecutor(ExecutorType.Activate, CardId.TheUndyingLegion, UndyingLegionEffect);

            // Dugares & Exciton Knight
            AddExecutor(ExecutorType.SpSummon, CardId.Number60Dugares, DugaresSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number60Dugares, DugaresEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.EvilswarmExcitonKnight, ExcitonKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.EvilswarmExcitonKnight, ExcitonKnightEffect);

            // Links: S:P Little Knight & Agnumday
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightOnSummonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsAgnumday, AgnumdaySummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsAgnumday, AgnumdayEffect);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.LacrimaTheCrimsonTears, LacrimaSummon);
            AddExecutor(ExecutorType.Summon, CardId.GemKnightQuartz, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.GemKnightNepyrim, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.TearlamentsScheiren, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.FairyTailSnow, SimpleSummon);

            // Gated Emergency Quartz (NEVER activate from hand in normal play due to Extra Deck lock)
            AddExecutor(ExecutorType.Activate, CardId.GemKnightQuartz, GemKnightQuartzGatedEffect);

            // ── Tier 4: Spell & Trap Sets ──
            AddExecutor(ExecutorType.SpellSet, CardId.TearlamentsSulliek, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.TearlamentsCryme, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.TearlamentsHeartbeat, DefaultSpellSet);

            // ── Tier 5: Repositioning ──
            AddExecutor(ExecutorType.Repos, MonsterReposOverride);
        }

        public override bool OnSelectHand() => true;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _engraverHandUsed = false;
            _engraverGYUsed = false;
            _tractUsed = false;
            _requiemUsed = false;
            _sequenceUsed = false;
            _fiendsmithLacrimaUsed = false;
            _lacrimaSummonUsed = false;
            _brilliantFusionUsed = false;
            _gemKnightQuartzUsed = false;
            _reinoheartSummonUsed = false;
            _scheirenHandUsed = false;
            _tearKashtiraSpSummonUsed = false;
            _kitkallosSearchUsed = false;
            _kitkallosSelfPopUsed = false;
            _pilgrimReaperUsed = false;
            _dugaresUsed = false;
            _fenrirSearchUsed = false;
            _keldoUsed = false;
            _mudoraUsed = false;
            _snowSummonedThisTurn = false;
            _maxxCActivatedThisTurn = false;
            _fuwalosActivatedThisTurn = false;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.TearlamentsRulkallos
                || card.Id == CardId.TearlamentsKaleidoHeart
                || card.Id == CardId.FiendsmithsDesirae
                || card.Id == CardId.TheUndyingLegion
                || card.Id == CardId.SPLittleKnight
                || card.Id == CardId.PilgrimReaper
                || card.Id == CardId.KashtiraFenrir;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Controller == 1) return 1; // Enemy monsters first (super poly / closed heaven)
            if (c.Id == CardId.FabledLurrie) return 2;
            if (c.Id == CardId.GemKnightNepyrim) return 3;
            if (c.Id == CardId.GemKnightQuartz) return 4;
            if (c.Id == CardId.GemKnightAmethyst) return 5;
            if (c.Id == CardId.FiendsmithsRequiem) return 6;
            if (c.Id == CardId.MoonOfTheClosedHeaven) return 7;
            if (c.Id == CardId.LacrimaTheCrimsonTears) return 8;
            if (c.Id == CardId.FiendsmithEngraver) return 9;
            if (c.Id == CardId.FiendsmithsLacrima) return 10;
            if (c.Id == CardId.TearlamentsKitkallos) return 12;
            if (c.Id == CardId.TearlamentsReinoheart) return 15;
            if (c.Id == CardId.FairyTailSnow) return 20;
            if (c.Id == CardId.KashtiraFenrir) return 500;
            if (c.Id == CardId.TheUndyingLegion) return 700;
            if (c.Id == CardId.TearlamentsKaleidoHeart) return 800;
            if (c.Id == CardId.TearlamentsRulkallos) return 900;
            if (c.Id == CardId.FiendsmithsDesirae) return 950;
            return base.GetMaterialPriority(c);
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 0: QUICK NEGATES, HANDTRAPS & DISRUPTIONS
        // ═══════════════════════════════════════════════════════════════

        private bool MaxxCEffect()
        {
            if (_maxxCActivatedThisTurn) return false;
            if (Duel.Player == 1 || Duel.LastChainPlayer == 1)
            {
                _maxxCActivatedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool MulcharmyFuwalosEffect()
        {
            if (_fuwalosActivatedThisTurn) return false;
            // Strict condition: We must control 0 cards, opponent turn
            if (Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0 && Duel.Player == 1)
            {
                _fuwalosActivatedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool AshBlossomEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return CardIntelligence.IsHighThreatChokepoint(LastChainCard?.Id ?? 0)
                || Duel.LastChainPlayer == 1;
        }

        private bool DrollAndLockBirdEffect()
        {
            return Duel.LastChainPlayer == 1 && Duel.Phase != DuelPhase.Draw;
        }

        private bool TearlamentsCrymeEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Counter Trap activation: Negate activation of monster effect or S/T
                if (Duel.LastChainPlayer == 1)
                {
                    // Select a monster from hand to send to GY (Tear monster preferred to trigger GY effect)
                    var sendTarget = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && (c.Id == CardId.TearlamentsScheiren || c.Id == CardId.TearlamentsHavnis || c.Id == CardId.TearlamentsReinoheart))
                                  ?? Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && (c.Id == CardId.FabledLurrie || c.Id == CardId.GemKnightNepyrim || c.Id == CardId.FairyTailSnow))
                                  ?? Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && !IsAceCard(c));
                    if (sendTarget != null)
                    {
                        AI.SelectCard(sendTarget);
                        return true;
                    }
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                // If sent to GY, recycle banished Tear monster to hand
                var banishedTear = Bot.Banished.FirstOrDefault(c => c != null && c.IsMonster() && (c.Id == CardId.TearlamentsScheiren || c.Id == CardId.TearlamentsReinoheart || c.Id == CardId.TearlamentsKashtira));
                if (banishedTear != null)
                {
                    AI.SelectCard(banishedTear);
                    return true;
                }
                return true;
            }
            return false;
        }

        private bool RulkallosNegateEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Quick effect to negate Special Summon effect
                if (Duel.LastChainPlayer == 1)
                {
                    // Fodder to send to GY: Kitkallos / Reinoheart / Scheiren / Havnis
                    var fodder = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Id != CardId.TearlamentsRulkallos && (c.Id == CardId.TearlamentsKitkallos || c.Id == CardId.TearlamentsReinoheart))
                              ?? Bot.Hand.FirstOrDefault(c => c != null && (c.Id == CardId.TearlamentsScheiren || c.Id == CardId.TearlamentsHavnis))
                              ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c))
                              ?? Card;
                    AI.SelectCard(fodder);
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Self-revive when sent to GY by card effect
                return true;
            }
            return false;
        }

        private bool SulliekActivateOrNegate()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                // Face-down Trap: activate when opponent has a face-up monster effect or in opponent turn
                if (Duel.Player == 1 || Enemy.GetMonsters().Any(c => c != null && c.IsFaceup()))
                {
                    return true;
                }
            }
            else if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Monster negate: target opponent face-up monster, then send 1 monster we control
                var oppTarget = GetBestOpponentTarget();
                var ourTear = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == CardId.TearlamentsKitkallos || c.Id == CardId.TearlamentsReinoheart || c.Id == CardId.TearlamentsScheiren || c.Id == CardId.GemKnightAmethyst))
                           ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c));

                if (oppTarget != null && ourTear != null)
                {
                    AI.SelectCard(oppTarget);
                    AI.SelectNextCard(ourTear);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // Sent to GY -> Search Tearlaments monster
                AI.SelectCard(CardId.TearlamentsReinoheart, CardId.TearlamentsScheiren, CardId.TearlamentsKashtira, CardId.TearlamentsHavnis);
                return true;
            }
            return false;
        }

        private ClientCard GetBestOpponentTarget()
        {
            // 1. High Priority Floodgates & Key Spells/Traps from Central Intelligence
            var floodgateST = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && CardIntelligence.IsFloodgateSpellTrap(c.Id) && !CardIntelligence.IsTargetImmune(c));
            if (floodgateST != null) return floodgateST;

            // 2. High Priority Floodgate Monsters & Known Negators
            var threatMonster = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && (CardIntelligence.IsFloodgateMonster(c.Id) || CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsHighThreatChokepoint(c.Id)) && !CardIntelligence.IsTargetImmune(c));
            if (threatMonster != null) return threatMonster;

            // 3. Highest ATK face-up monster (not target-immune)
            var monsterTarget = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !CardIntelligence.IsTargetImmune(c)).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (monsterTarget != null) return monsterTarget;

            // 4. Any face-up spell
            var otherSpell = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && !CardIntelligence.IsTargetImmune(c));
            if (otherSpell != null) return otherSpell;

            // 5. Fallback
            return Enemy.GetMonsters().FirstOrDefault(c => c != null && !CardIntelligence.IsTargetImmune(c)) ?? Enemy.GetSpells().FirstOrDefault();
        }

        private bool KaleidoHeartEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = GetBestOpponentTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Self-revive + send Tearlaments card from Deck to GY
                AI.SelectCard(CardId.TearlamentsSulliek, CardId.TearlamentsScream, CardId.TearlamentsReinoheart);
                return true;
            }
            return false;
        }

        private bool KashtiraFenrirEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                long stringId = ActivateDescription;
                long searchDesc = Util.GetStringId(CardId.KashtiraFenrir, 0);

                // Effect 1: Search Kashtira monster during Main Phase
                if (stringId == searchDesc || !_fenrirSearchUsed)
                {
                    _fenrirSearchUsed = true;
                    AI.SelectCard(CardId.TearlamentsKashtira, CardId.KashtiraFenrir);
                    return true;
                }

                // Effect 2: Banish opponent face-up card face-down when opponent activates monster effect!
                var oppTarget = GetBestOpponentTarget();
                if (oppTarget != null)
                {
                    AI.SelectCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool SPLittleKnightQuickEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer != 1) return false;

            var ourTarget = Card;
            var oppTarget = GetBestOpponentTarget();
            if (oppTarget != null)
            {
                AI.SelectCard(ourTarget);
                AI.SelectNextCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool DesiraeEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer == 1 || Duel.Player == 1)
            {
                var oppTarget = GetBestOpponentTarget();
                var equipToSend = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == CardId.FiendsmithsRequiem || c.Id == CardId.FiendsmithEngraver || c.Id == CardId.FiendsmithsSequence));

                if (oppTarget != null)
                {
                    if (equipToSend != null)
                    {
                        AI.SelectCard(equipToSend);
                        AI.SelectNextCard(oppTarget);
                    }
                    else
                    {
                        AI.SelectCard(oppTarget);
                    }
                    return true;
                }
            }
            return false;
        }

        private bool FairyTailSnowEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Only activate if opponent has a face-up monster threat (>= 2000 ATK or Negator) that can be flipped face-down
                var oppThreat = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Attack >= 2000 || CardIntelligence.IsKnownNegator(c.Id)));
                if (oppThreat == null && Duel.Phase != DuelPhase.BattleStart && Duel.Phase != DuelPhase.Battle) return false;

                if (Duel.Player == 1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
                {
                    if (!_snowSummonedThisTurn)
                    {
                        // Protected cards: NEVER banish active Tear monsters or Ace cards!
                        var protectedIds = new HashSet<int>
                        {
                            CardId.TearlamentsKitkallos, CardId.TearlamentsRulkallos, CardId.TearlamentsKaleidoHeart,
                            CardId.TearlamentsReinoheart, CardId.TearlamentsScheiren, CardId.TearlamentsHavnis, CardId.TearlamentsKashtira,
                            CardId.FiendsmithEngraver, CardId.FiendsmithsSequence, CardId.FiendsmithsDesirae, CardId.FiendsmithsRequiem,
                            CardId.FiendsmithsLacrima, CardId.LacrimaTheCrimsonTears, CardId.KashtiraFenrir,
                            CardId.KeldoTheSacredProtector, CardId.MudoraTheSwordOracle,
                            CardId.TearlamentsSulliek, CardId.TearlamentsCryme, CardId.TearlamentsScream
                        };

                        var banishList = Bot.Graveyard.Where(c => c != null && c != Card && !protectedIds.Contains(c.Id))
                                                      .OrderBy(c => c.IsMonster() ? 2 : 1)
                                                      .Take(7)
                                                      .ToList();
                        if (banishList.Count == 7)
                        {
                            _snowSummonedThisTurn = true;
                            AI.SelectCard(banishList);
                            return true;
                        }
                    }
                }
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On SS: Book of Moon 1 opponent monster
                var oppTarget = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup() && !IsTargetImmune(c));
                if (oppTarget != null)
                {
                    AI.SelectCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private List<ClientCard> GetOppGraveyardChokepoints()
        {
            return Enemy.Graveyard.Where(c => c != null && (CardIntelligence.IsHighThreatChokepoint(c.Id) || c.IsMonster()))
                .OrderByDescending(c => CardIntelligence.IsHighThreatChokepoint(c.Id) ? 100 : c.Attack)
                .Take(3)
                .ToList();
        }

        private bool KeldoEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (_keldoUsed) return false;
                // Disrupt opponent GY or recycle our own
                var oppGY = GetOppGraveyardChokepoints();
                if (oppGY.Count > 0 && (Duel.Player == 1 || Duel.LastChainPlayer == 1 || Enemy.Graveyard.Count >= 3))
                {
                    _keldoUsed = true;
                    AI.SelectCard(oppGY);
                    return true;
                }
                // Self recycle if our deck is getting low (<= 10 cards)
                if (Bot.Deck.Count <= 10)
                {
                    var ourRecycle = Bot.Graveyard.Where(c => c != null && c != Card && (c.Id == CardId.TearlamentsKitkallos || c.Id == CardId.TearlamentsReinoheart || c.Id == CardId.TearlamentsScheiren)).Take(3).ToList();
                    if (ourRecycle.Count > 0)
                    {
                        _keldoUsed = true;
                        AI.SelectCard(ourRecycle);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool MudoraEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (_mudoraUsed) return false;
                var oppGY = GetOppGraveyardChokepoints();
                if (oppGY.Count > 0 && (Duel.Player == 1 || Duel.LastChainPlayer == 1 || Enemy.Graveyard.Count >= 3))
                {
                    _mudoraUsed = true;
                    AI.SelectCard(oppGY);
                    return true;
                }
                if (Bot.Deck.Count <= 10)
                {
                    var ourRecycle = Bot.Graveyard.Where(c => c != null && c != Card && (c.Id == CardId.TearlamentsKitkallos || c.Id == CardId.TearlamentsReinoheart || c.Id == CardId.TearlamentsScheiren)).Take(3).ToList();
                    if (ourRecycle.Count > 0)
                    {
                        _mudoraUsed = true;
                        AI.SelectCard(ourRecycle);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HavnisHandEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Opponent activated monster effect on field -> Special summon + mill 3!
                if (Duel.Player == 1 && Duel.LastChainPlayer == 1)
                {
                    return true;
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 1: PRIMARY STARTERS & COMBO ROUTER
        // ═══════════════════════════════════════════════════════════════

        private bool InstantFusionEffect()
        {
            if (Bot.LifePoints > 1000 && Bot.ExtraDeck.Any(c => c.Id == CardId.TearlamentsKitkallos))
            {
                // Free Kitkallos!
                AI.SelectCard(CardId.TearlamentsKitkallos);
                return true;
            }
            return false;
        }

        private bool BrilliantFusionEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (_brilliantFusionUsed) return false;
                _brilliantFusionUsed = true;

                // Summons Gem-Knight Amethyst!
                // Sends Gem-Knight Nepyrim + Tearlaments Aqua (Scheiren or Havnis) from Deck
                AI.SelectCard(CardId.GemKnightAmethyst);
                AI.SelectNextCard(CardId.GemKnightNepyrim);
                AI.SelectThirdCard(CardId.TearlamentsScheiren, CardId.TearlamentsHavnis);
                return true;
            }
            return false;
        }

        private bool EngraverHandEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_engraverHandUsed) return false;
                _engraverHandUsed = true;
                AI.SelectCard(CardId.FiendsmithsTract);
                return true;
            }
            return false;
        }

        private bool TractEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_tractUsed) return false;
                _tractUsed = true;
                // Search Fabled Lurrie and discard Lurrie (triggers free Lurrie SS!)
                AI.SelectCard(CardId.FabledLurrie, CardId.LacrimaTheCrimsonTears);
                AI.SelectNextCard(CardId.FabledLurrie, CardId.GemKnightNepyrim);
                return true;
            }
            return false;
        }

        private bool LurrieSummon()
        {
            // Normal Summon Lurrie as backup starter if field is empty and we have Requiem in Extra
            return Bot.GetMonsterCount() == 0 && Bot.ExtraDeck.Any(c => c.Id == CardId.FiendsmithsRequiem);
        }

        private bool LurrieEffect() => true;

        private bool FenrirSpSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool ReinoheartSummon()
        {
            if (_reinoheartSummonUsed) return false;
            return true;
        }

        private bool ReinoheartEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_reinoheartSummonUsed) return false;
                _reinoheartSummonUsed = true;
                // Dump Scheiren or Havnis to trigger GY Fusion immediately
                AI.SelectCard(CardId.TearlamentsScheiren, CardId.TearlamentsHavnis, CardId.TearlamentsKashtira);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Special summon self + discard 1 Tear
                var tearFodder = Bot.Hand.FirstOrDefault(c => c != null && (c.Id == CardId.TearlamentsScheiren || c.Id == CardId.TearlamentsHavnis || c.Id == CardId.TearlamentsSulliek || c.Id == CardId.TearlamentsScream));
                if (tearFodder != null)
                {
                    AI.SelectCard(tearFodder);
                    return true;
                }
            }
            return false;
        }

        private bool ScheirenHandEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_scheirenHandUsed) return false;
                // Send 1 monster from hand: Lurrie / Nepyrim / Tear monster / Snow / Keldo / Mudora
                var sendTarget = Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.FabledLurrie || c.Id == CardId.GemKnightNepyrim || c.Id == CardId.FairyTailSnow || c.Id == CardId.KeldoTheSacredProtector || c.Id == CardId.MudoraTheSwordOracle || c.Id == CardId.TearlamentsHavnis || c.Id == CardId.TearlamentsReinoheart))
                              ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && c.IsMonster() && !IsAceCard(c));
                if (sendTarget != null)
                {
                    _scheirenHandUsed = true;
                    AI.SelectCard(sendTarget);
                    return true;
                }
            }
            return false;
        }

        private bool TearKashtiraEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_tearKashtiraSpSummonUsed) return false;
                // Banish 1 Kashtira or Tearlaments card from hand or GY
                var banishFodder = Bot.Graveyard.FirstOrDefault(c => c != null && (c.Id == CardId.TearlamentsScream || c.Id == CardId.TearlamentsHeartbeat || c.Id == CardId.TearlamentsReinoheart))
                                ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.TearlamentsScream || c.Id == CardId.TearlamentsHeartbeat));
                if (banishFodder != null)
                {
                    _tearKashtiraSpSummonUsed = true;
                    AI.SelectCard(banishFodder);
                    return true;
                }
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On summon: mill 3 cards from OUR deck! (Handled in OnSelectOption: Option 0)
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // When sent to GY: mill 2 cards from our deck
                return true;
            }
            return false;
        }

        private bool ScreamEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                return true; // Activate Continuous Spell
            }
            if (Card.Location == CardLocation.Grave)
            {
                // When sent to GY: add 1 Tear Trap (Sulliek or Cryme)
                AI.SelectCard(CardId.TearlamentsSulliek, CardId.TearlamentsCryme);
                return true;
            }
            return false;
        }

        private bool HeartbeatEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                var oppST = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && CardIntelligence.IsFloodgateSpellTrap(c.Id))
                         ?? Enemy.GetSpells().FirstOrDefault(c => c != null);
                if (oppST != null)
                {
                    AI.SelectCard(oppST);
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.TearlamentsSulliek, CardId.TearlamentsCryme);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 2: GY TRIGGERS & MILL REACTIONS
        // ═══════════════════════════════════════════════════════════════

        private bool NepyrimEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Add Gem-Knight card from Deck/GY
                AI.SelectCard(CardId.BrilliantFusion, CardId.GemKnightQuartz);
                return true;
            }
            return false;
        }

        private bool KitkallosEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                long stringId = ActivateDescription;
                long searchDesc = Util.GetStringId(CardId.TearlamentsKitkallos, 0);
                long popDesc = Util.GetStringId(CardId.TearlamentsKitkallos, 1);

                // Effect 1: Search or dump Tearlaments card
                if (stringId == searchDesc || (stringId == -1 && !_kitkallosSearchUsed))
                {
                    _kitkallosSearchUsed = true;
                    // Add Sulliek if not possessed, else add/send Reinoheart or Scream
                    AI.SelectCard(CardId.TearlamentsSulliek, CardId.TearlamentsReinoheart, CardId.TearlamentsScream);
                    return true;
                }

                // Effect 2: Target Kitkallos herself to send to GY and SS Tear from hand/GY
                // Pop self triggers her 3rd effect: MILL 5!
                if (stringId == popDesc || (stringId == -1 && !_kitkallosSelfPopUsed))
                {
                    _kitkallosSelfPopUsed = true;
                    AI.SelectCard(Card);
                    AI.SelectNextCard(CardId.TearlamentsReinoheart, CardId.TearlamentsScheiren, CardId.TearlamentsKashtira);
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                // If sent to GY by card effect: MILL 5!
                return true;
            }
            return false;
        }

        private bool ScheirenGYFusionEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY Fusion Summon: Priority Kitkallos -> Rulkallos -> Kaleido-Heart
                AI.SelectCard(CardId.TearlamentsKitkallos, CardId.TearlamentsRulkallos, CardId.TearlamentsKaleidoHeart);
                AI.SelectNextCard(CardId.TearlamentsKitkallos, CardId.TearlamentsReinoheart, CardId.TearlamentsHavnis, CardId.TearlamentsKashtira, CardId.GemKnightAmethyst);
                return true;
            }
            return false;
        }

        private bool HavnisGYFusionEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY Fusion Summon: Priority Kitkallos -> Rulkallos -> Kaleido-Heart
                AI.SelectCard(CardId.TearlamentsKitkallos, CardId.TearlamentsRulkallos, CardId.TearlamentsKaleidoHeart);
                AI.SelectNextCard(CardId.TearlamentsKitkallos, CardId.TearlamentsReinoheart, CardId.TearlamentsScheiren, CardId.TearlamentsKashtira, CardId.GemKnightAmethyst);
                return true;
            }
            return false;
        }

        private bool EngraverGYEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (_engraverGYUsed) return false;
                var lightFiend = Bot.Graveyard.FirstOrDefault(c => c != null && c != Card && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend);
                if (lightFiend != null)
                {
                    _engraverGYUsed = true;
                    AI.SelectCard(lightFiend);
                    return true;
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 3: EXTRA DECK SUMMONS & LADDERS
        // ═══════════════════════════════════════════════════════════════

        private bool RequiemSummon()
        {
            if (_requiemUsed) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend);
        }

        private bool RequiemEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                _requiemUsed = true;
                AI.SelectCard(CardId.LacrimaTheCrimsonTears, CardId.FiendsmithEngraver);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Equip to non-Link Fiendsmith monster we control (Desirae, Lacrima, Engraver)
                var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == CardId.FiendsmithsDesirae || c.Id == CardId.FiendsmithsLacrima || c.Id == CardId.FiendsmithEngraver));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool LacrimaSummon() => true;

        private bool LacrimaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_lacrimaSummonUsed) return false;
                _lacrimaSummonUsed = true;
                AI.SelectCard(CardId.FiendsmithEngraver);
                return true;
            }
            if (Card.Location == CardLocation.Grave && Duel.Player == 1)
            {
                // Opponent turn: revive Link Fiendsmith
                AI.SelectCard(CardId.FiendsmithsRequiem, CardId.FiendsmithsSequence);
                return true;
            }
            return false;
        }

        private bool MoonSummon()
        {
            if (_sequenceUsed) return false;
            if (!Bot.ExtraDeck.Any(c => c.Id == CardId.FiendsmithsSequence || c.Id == CardId.FiendsmithsRequiem)) return false;
            // Only summon Moon if we do NOT have a LIGHT Fiend yet
            bool hasLightFiend = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend);
            if (hasLightFiend) return false;
            var fodder = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            return fodder.Count >= 2;
        }

        private bool SequenceSummon()
        {
            if (_sequenceUsed) return false;
            // Sequence requires: 2 monsters, including 1 LIGHT Fiend monster
            bool hasLightFiend = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend);
            var fodder = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            return hasLightFiend && fodder.Count >= 2;
        }

        private bool SequenceEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                _sequenceUsed = true;
                // Fuse into Fiendsmith's Lacrima (to set up Rank 6 Reaper) or Desirae
                AI.SelectCard(CardId.FiendsmithsLacrima, CardId.FiendsmithsDesirae);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Equip to non-Link LIGHT Fiend (Desirae, Lacrima, Engraver) to grant target protection!
                var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == CardId.FiendsmithsDesirae || c.Id == CardId.FiendsmithsLacrima || c.Id == CardId.FiendsmithEngraver));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool FiendsmithLacrimaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_fiendsmithLacrimaUsed) return false;
                _fiendsmithLacrimaUsed = true;
                // Revive Engraver for Rank 6 setup!
                AI.SelectCard(CardId.FiendsmithEngraver);
                return true;
            }
            if (Card.Location == CardLocation.Grave && Duel.Player == 1)
            {
                var linkTarget = Bot.Graveyard.FirstOrDefault(c => c != null && (c.Id == CardId.FiendsmithsRequiem || c.Id == CardId.FiendsmithsSequence));
                if (linkTarget != null)
                {
                    AI.SelectCard(linkTarget);
                    return true;
                }
            }
            return false;
        }

        private bool PilgrimReaperSummon()
        {
            if (_pilgrimReaperUsed) return false;
            if (Bot.Deck.Count < 15) return false;
            int lv6 = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 6);
            return lv6 >= 2;
        }

        private bool PilgrimReaperEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_pilgrimReaperUsed || Bot.Deck.Count < 10) return false;
                _pilgrimReaperUsed = true;
                // Detach 1 -> MILL 5 FOR BOTH PLAYERS!
                return true;
            }
            return false;
        }

        private bool UndyingLegionSummon()
        {
            // Rank-up directly onto Pilgrim Reaper after it milled 5!
            var reaper = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.PilgrimReaper);
            if (reaper != null)
            {
                AI.SelectCard(reaper);
                return true;
            }
            int lv7 = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 7 && !IsAceCard(c));
            return lv7 >= 2;
        }

        private bool UndyingLegionEffect()
        {
            // Detach 1 to mill or attach
            return true;
        }

        private bool DugaresSummon()
        {
            if (_dugaresUsed) return false;
            int lv4 = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c));
            return lv4 >= 2 && Bot.Hand.Count <= 3;
        }

        private bool DugaresEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_dugaresUsed) return false;
                _dugaresUsed = true;
                // Option 0: Draw 2 discard 1
                AI.SelectOption(0);
                return true;
            }
            return false;
        }

        private bool ExcitonKnightSummon()
        {
            int ourTotal = Bot.GetMonsterCount() + Bot.GetSpellCount() + Bot.Hand.Count;
            int oppTotal = Enemy.GetMonsterCount() + Enemy.GetSpellCount() + Enemy.Hand.Count;
            return ourTotal < oppTotal && Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !IsAceCard(c)) >= 2;
        }

        private bool ExcitonKnightEffect()
        {
            int ourTotal = Bot.GetMonsterCount() + Bot.GetSpellCount();
            int oppTotal = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            return oppTotal > ourTotal;
        }

        private bool SimpleSummon() => true;

        private bool SPLittleKnightSummon()
        {
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.SPLittleKnight)) return false;
            var fodder = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            return fodder.Count >= 2;
        }

        private bool SPLittleKnightOnSummonEffect()
        {
            var oppTarget = GetBestOpponentTarget() ?? Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool AgnumdaySummon()
        {
            return Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c)) >= 3;
        }

        private bool AgnumdayEffect()
        {
            AI.SelectCard(CardId.FiendsmithEngraver, CardId.FiendsmithsLacrima);
            return true;
        }

        private bool GemKnightQuartzGatedEffect()
        {
            // CRITICAL ANTI-LOCK GATE:
            // Gem-Knight Quartz locks the player into ONLY Gem-Knight Extra Deck monsters for the rest of the turn!
            // NEVER activate from hand unless we have NO other plays and no board presence!
            if (_gemKnightQuartzUsed) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // Only activate in dire emergency when hand has NO other starters and board is empty
                bool hasOtherStarters = Bot.Hand.Any(c => c != null && c != Card && (c.Id == CardId.TearlamentsReinoheart || c.Id == CardId.BrilliantFusion || c.Id == CardId.InstantFusion || c.Id == CardId.FiendsmithEngraver || c.Id == CardId.FiendsmithsTract));
                if (hasOtherStarters || Bot.GetMonsterCount() > 0) return false;

                if (Enemy.GetMonsterCount() > 0)
                {
                    _gemKnightQuartzUsed = true;
                    AI.SelectCard(CardId.BrilliantFusion);
                    return true;
                }
            }
            return false;
        }

        private bool MonsterReposOverride()
        {
            if (Card.Id == CardId.PilgrimReaper && Card.Attack < 2000 && Card.IsAttack())
            {
                return true;
            }
            if (Card.Attack < Card.Defense)
            {
                return DefaultMonsterRepos();
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SPECIALIZED SELECTION OVERRIDES (OnSelectCard / Option / Chain)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // ── Hint 506: HINTMSG_TODECK (Tearlaments GY Fusion Materials / Spin Removal) ──
            if (hint == HINT_SELECT_TODECK)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var sorted = enemyCards.OrderByDescending(c => GetCardThreatScore(c)).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }

                // If selecting our cards to return to deck (Tearlaments Fusion Materials):
                // Priority: return GY monsters (Scheiren, Havnis, Tear Kashtira, Amethyst, Reinoheart)
                // NEVER return active on-field Ace cards if GY materials exist!
                var ourCards = cards.Where(c => c != null && c.Controller == 0).ToList();
                if (ourCards.Count >= min)
                {
                    var sorted = ourCards.OrderBy(c => {
                        // Return GY cards first
                        if (c.Location == CardLocation.Grave)
                        {
                            if (c.Id == CardId.GemKnightAmethyst) return 1;
                            if (c.Id == CardId.TearlamentsKashtira) return 2;
                            if (c.Id == CardId.TearlamentsHavnis) return 3;
                            if (c.Id == CardId.TearlamentsScheiren) return 4;
                            if (c.Id == CardId.TearlamentsReinoheart) return 5;
                            if (c.Id == CardId.TearlamentsKitkallos) return 6;
                            return 10;
                        }
                        // Field cards second (non-ace first)
                        if (c.Location == CardLocation.MonsterZone)
                        {
                            if (!IsAceCard(c)) return 50;
                            return 500;
                        }
                        return 100;
                    }).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
            }

            // ── Hint 507: HINTMSG_EQUIP (Fiendsmith Link Equip to Non-Link LIGHT Fiend) ──
            if (hint == HINT_SELECT_EQUIP)
            {
                var equipTargets = cards.Where(c => c != null && c.Controller == 0 && c.IsFaceup()).OrderBy(c => {
                    if (c.Id == CardId.FiendsmithsDesirae) return 1;
                    if (c.Id == CardId.FiendsmithsLacrima) return 2;
                    if (c.Id == CardId.FiendsmithEngraver) return 3;
                    return 10;
                }).ToList();
                if (equipTargets.Count >= min) return equipTargets.Take(min).ToList();
            }

            // ── Hint 508 / 501: HINTMSG_TOGRAVE / DISCARD ──
            if (hint == HINT_SELECT_TOGRAVE || hint == HINT_SELECT_DISCARD)
            {
                // If Kitkallos self-pop is triggering: send Kitkallos herself!
                var kitkallos = cards.FirstOrDefault(c => c != null && c.Id == CardId.TearlamentsKitkallos && c.Location == CardLocation.MonsterZone);
                if (kitkallos != null && Card != null && Card.Id == CardId.TearlamentsKitkallos)
                {
                    return new List<ClientCard> { kitkallos };
                }

                // If Reinoheart on-summon dump: prioritize Scheiren / Havnis / Tear Kashtira
                if (Card != null && Card.Id == CardId.TearlamentsReinoheart)
                {
                    var dumpTarget = cards.FirstOrDefault(c => c != null && (c.Id == CardId.TearlamentsScheiren || c.Id == CardId.TearlamentsHavnis || c.Id == CardId.TearlamentsKashtira));
                    if (dumpTarget != null) return new List<ClientCard> { dumpTarget };
                }

                // If Lacrima the Crimson Tears on-summon dump: dump Engraver
                if (Card != null && Card.Id == CardId.LacrimaTheCrimsonTears)
                {
                    var engraver = cards.FirstOrDefault(c => c != null && c.Id == CardId.FiendsmithEngraver);
                    if (engraver != null) return new List<ClientCard> { engraver };
                }
            }

            // ── Hint 504 / 512: HINTMSG_REMOVE / BANISH ──
            if (hint == HINT_SELECT_REMOVE || hint == HINT_SELECT_BANISH)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyTargets.Count > 0)
                {
                    var sorted = enemyTargets.OrderByDescending(c => GetCardThreatScore(c)).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }

                // Friendly banish (Fairy Tail - Snow costs or Tear Kashtira cost)
                var friendlyTargets = cards.Where(c => c != null && c.Controller == 0).ToList();
                if (friendlyTargets.Count >= min)
                {
                    var protectedIds = new HashSet<int>
                    {
                        CardId.TearlamentsKitkallos, CardId.TearlamentsRulkallos, CardId.TearlamentsKaleidoHeart,
                        CardId.TearlamentsReinoheart, CardId.TearlamentsScheiren, CardId.TearlamentsHavnis, CardId.TearlamentsKashtira,
                        CardId.FiendsmithEngraver, CardId.FiendsmithsSequence, CardId.FiendsmithsDesirae,
                        CardId.KeldoTheSacredProtector, CardId.MudoraTheSwordOracle
                    };

                    var sorted = friendlyTargets.OrderBy(c => {
                        if (protectedIds.Contains(c.Id)) return 1000;
                        if (c.Location == CardLocation.Grave && (c.IsSpell() || c.IsTrap())) return 1;
                        if (c.Location == CardLocation.Grave && (c.Id == CardId.AshBlossom || c.Id == CardId.MaxxC || c.Id == CardId.MulcharmyFuwalos || c.Id == CardId.DrollAndLockBird)) return 2;
                        if (c.Location == CardLocation.Grave && (c.Id == CardId.FabledLurrie || c.Id == CardId.GemKnightQuartz || c.Id == CardId.GemKnightNepyrim)) return 3;
                        return 100;
                    }).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }
            }

            // ── Hint 509: HINTMSG_SPSUMMON ──
            if (hint == HINT_SELECT_SPSUMMON)
            {
                // Kitkallos Effect 2 Special Summon from GY/Hand: prioritize Reinoheart / Scheiren / Tear Kashtira
                if (Card != null && Card.Id == CardId.TearlamentsKitkallos)
                {
                    var tearTarget = cards.FirstOrDefault(c => c != null && (c.Id == CardId.TearlamentsReinoheart || c.Id == CardId.TearlamentsScheiren || c.Id == CardId.TearlamentsKashtira));
                    if (tearTarget != null) return new List<ClientCard> { tearTarget };
                }

                // Fiendsmith's Lacrima revive Engraver
                if (Card != null && Card.Id == CardId.FiendsmithsLacrima)
                {
                    var engraver = cards.FirstOrDefault(c => c != null && c.Id == CardId.FiendsmithEngraver);
                    if (engraver != null) return new List<ClientCard> { engraver };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return base.OnSelectOption(options);

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 20;
                long optIndex = options[i] & 0xfffff;

                if (cardId == 0 && LastChainCard != null)
                {
                    cardId = LastChainCard.Id;
                }

                // 1. Tearlaments Kashtira (4928565):
                // optIndex 3 (str4) is "Send the top 3 cards of your Deck"
                // optIndex 4 (str5) is "Send the top 3 cards of your opponent's Deck"
                // Rule 4 Anti-Advantage Gate: ALWAYS mill our own deck!
                if (cardId == CardId.TearlamentsKashtira)
                {
                    if (optIndex == 3 || optIndex == 0) return i;
                }

                // 2. Tearlaments Kitkallos (92731385):
                // optIndex 0 is Add to Hand
                // optIndex 1 is Send to GY
                if (cardId == CardId.TearlamentsKitkallos)
                {
                    // If we do not have Sulliek in hand or field: Add to Hand (Option 0)
                    bool hasSulliek = Bot.Hand.Concat(Bot.GetSpells()).Any(c => c != null && c.Id == CardId.TearlamentsSulliek);
                    if (!hasSulliek && optIndex == 0) return i;

                    // If we haven't Normal Summoned Reinoheart: Add to Hand (Option 0)
                    if (!_reinoheartSummonUsed && !Bot.Hand.Any(c => c != null && c.Id == CardId.TearlamentsReinoheart) && optIndex == 0) return i;

                    // Otherwise, send to GY to trigger Fusion immediately (Option 1)
                    if (optIndex == 1) return i;
                    if (optIndex == 0) return i;
                }

                // 3. Number 60: Dugares the Timeless (66011101):
                // optIndex 1 is Draw 2 discard 1
                // optIndex 2 is Special Summon 1 monster from GY
                if (cardId == CardId.Number60Dugares)
                {
                    if (optIndex == 1) return i;
                }
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Low ATK combo pieces should be summoned in Defense Position
            if (cardId == CardId.MulcharmyFuwalos || cardId == CardId.MaxxC || cardId == CardId.DrollAndLockBird
             || cardId == CardId.FabledLurrie || cardId == CardId.GemKnightNepyrim || cardId == CardId.GemKnightQuartz)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }

    [Deck("Expert_2026_Tearla", "2026_Tearla")]
    public class ExpertTearlaExecutor : _2026_TearlaExecutor
    {
        public ExpertTearlaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }
}
