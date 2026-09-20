// ============================================================
// CARD AUDIT — 2026_Tearla (Fiendsmith-Brilliant-Tearlaments)
// ============================================================
// | Card Name                     | Type       | OPT? | HOPT? | Cost | Effect Summary                              | Activate When                               | NEVER Activate When                           |
// |-------------------------------|------------|------|-------|------|---------------------------------------------|---------------------------------------------|-----------------------------------------------|
// | Tearlaments Reinoheart        | Monster L4 | Yes  | Yes   | None | NS/SS: Dump Tear; GY self-revive + discard | Main Phase starter                          | Already used HOPT                             |
// | Tearlaments Scheiren          | Monster L4 | Yes  | Yes   | Send | Hand SS + send 1 + mill 3; GY Fusion summon | Main Phase starter / GY fusion              | No discardable fodder                         |
// | Tearlaments Havnis            | Monster L3 | Yes  | Yes   | None | Opp field eff: Hand SS + mill 3; GY Fusion  | Opponent activates eff on field / GY fusion | Already used HOPT                             |
// | Tearlaments Kashtira          | Monster L7 | Yes  | Yes   | Banish| Hand Quick SS; NS/SS mill 3; GY mill 2    | Main Phase extend / Mill                    | Already used HOPT                             |
// | Tearlaments Kitkallos         | Fusion L5  | Yes  | Yes   | None | On SS: search/dump Tear; Target pop -> SS; GY mill 5 | On Fusion / Main Phase setup       | Already used HOPT                             |
// | Tearlaments Rulkallos         | Fusion L8  | Yes  | Yes   | None | SS negate Quick effect; GY self-revive      | Opponent Special Summons                    | Out of resources                              |
// | Tearlaments Kaleido-Heart     | Fusion L9  | Yes  | Yes   | None | On SS/Aqua dump: Spin 1 card; GY revive     | On Summon / Aqua sent to GY / Sent to GY    | Already used HOPT                             |
// | Fiendsmith Engraver           | Monster L6 | Yes  | Yes   | Disc | Discard -> Search Tract; GY Shuf LIGHT SS   | In Hand / In GY                             | No LIGHT Fiend to shuffle                     |
// | Fiendsmith's Tract            | Spell      | Yes  | Yes   | Disc | Search LIGHT Fiend + Discard; GY Fusion     | Main Phase Fiendsmith starter               | Already used HOPT                             |
// | Lacrima the Crimson Tears     | Monster L4 | Yes  | Yes   | None | NS/SS: Dump Fiendsmith; Opp turn revive Link| On Summon / In GY                           | Already used HOPT                             |
// | Fabled Lurrie                 | Monster L1 | No   | No    | None | Discarded to GY -> Special Summon self      | Discarded by Tract / Scheiren               | Zone full                                     |
// | Fiendsmith's Requiem          | Link 1     | Yes  | Yes   | Trib | Tribute self -> SS Fiendsmith from Deck     | On Field (LIGHT Fiend used as mat)          | Already used HOPT                             |
// | Fiendsmith's Sequence         | Link 2     | Yes  | Yes   | Shuf | Shuffle GY mats -> Fusion Summon Fiend      | Main Phase (Lacrima + Engraver in GY)       | Materials not in GY                           |
// | Fiendsmith's Lacrima          | Fusion L6  | Yes  | Yes   | None | On Fusion: Revive Engraver (Rank 6 setup)   | On Fusion Summon                            | Engraver not in GY                            |
// | Fiendsmith's Desirae          | Fusion L9  | Yes  | Yes   | Detach| Send equipped Link -> Negate cards up to Link rating | On Field / Quick negate          | No equipped card                              |
// | Brilliant Fusion              | ContSpell  | Yes  | Yes   | Send | Send Gem-Knight + Aqua from Deck to fuse    | Main Phase ignition                         | Already used HOPT                             |
// | Gem-Knight Quartz             | Monster L4 | Yes  | Yes   | Disc | Discard -> Set Continuous Fusion Spell from Deck | In Hand, Brilliant Fusion in Deck    | Already used HOPT                             |
// | Gem-Knight Nepyrim            | Monster L4 | Yes  | Yes   | None | Sent to GY -> Add Gem-Knight card from Deck/GY | Sent to GY by Brilliant Fusion           | Already used HOPT                             |
// | Gem-Knight Amethyst           | Fusion L7  | No   | No    | None | Sent from field to GY -> Bounce all set S/T | Fusion target for Brilliant Fusion          | Field empty                                   |
// | Pilgrim Reaper                | Xyz R6     | Yes  | No    | Detach| Detach 1 -> Both players mill 5 cards       | Main Phase mill ignition                    | No materials                                  |
// | Number 60: Dugares            | Xyz R4     | Yes  | Yes   | Detach 2| Draw 2 discard 1 / Revive monster         | Main Phase extension                        | No materials                                  |
// | Fairy Tail - Snow             | Monster L4 | No   | No    | Banish 7| Quick SS from GY; On SS Book of Moon       | Opponent turn disruption / Main Phase extend| Fewer than 7 cards to banish                  |
// | Keldo the Sacred Protector    | Monster L4 | Yes  | Yes   | Banish| Quick GY effect: Shuffle up to 3 cards from GY into Deck | Opponent GY setup / Self recycle | Already used HOPT                             |
// | Mudora the Sword Oracle       | Monster L4 | Yes  | Yes   | Banish| Quick GY effect: Shuffle up to 3 cards from GY into Deck | Opponent GY setup / Self recycle | Already used HOPT                             |
// | Kashtira Fenrir               | Monster L7 | Yes  | Yes   | None | Inherent SS; Search Kashtira; Banish 1 face-down on opp eff | Main Phase / Opp eff response | Already used HOPT                             |
// | Tearlaments Scream            | ContSpell  | Yes  | Yes   | None | On Summon mill 3 + -500 ATK; GY search Trap | Monster summoned / Sent to GY                | Already used HOPT                             |
// | Tearlaments Sulliek           | ContTrap   | Yes  | Yes   | Send | Negate monster eff + send Tear; GY search Tear | Opp monster eff / Sent to GY              | No Tear to send / Already used HOPT           |
// | Tearlaments Cryme             | CounterTr  | Yes  | Yes   | Send | Omni-negate and shuffle + send mon; GY recycle banished Tear | Opp card/eff activation   | No mon in hand / No Tear on field             |
// | S:P Little Knight             | Link 2     | Yes  | Yes   | None | On Link: Banish 1 card; Quick double banish | On Link Summon / Opponent activates effect  | Already used HOPT                             |
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
    [Deck("2026_Tearla", "2026_Tearla")]
    public class _2026_TearlaExecutor : ModernExecutor
    {
        public class CardId
        {
            // Tearlaments Core
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

            // Fiendsmith Engine
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

            // Gem-Knight Engine
            public const int BrilliantFusion = 7394770;
            public const int GemKnightNepyrim = 51831560;
            public const int GemKnightQuartz = 35622739;
            public const int GemKnightAmethyst = 71616908;

            // Ishizu Shufflers & Techs
            public const int KeldoTheSacredProtector = 63542003;
            public const int MudoraTheSwordOracle = 99937011;
            public const int FairyTailSnow = 55623480;
            public const int KashtiraFenrir = 32909498;
            public const int InstantFusion = 1845204;

            // Extra Deck Xyz & Links
            public const int PilgrimReaper = 45742626;
            public const int Number60Dugares = 66011101;
            public const int EvilswarmExcitonKnight = 46772449;
            public const int TheUndyingLegion = 43355214;
            public const int SPLittleKnight = 29301450;

            // Handtraps
            public const int MaxxC = 23434538;
            public const int AshBlossom = 14558127;
            public const int MulcharmyFuwalos = 42141493;
            public const int DrollAndLockBird = 94145021;
        }

        // Standard OCG Hint IDs
        private const long HINT_SELECT_FACEUP = 500;
        private const long HINT_SELECT_TOGRAVE = 501;
        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_DISCARD = 504;
        private const long HINT_SELECT_TOHAND = 506;
        private const long HINT_SELECT_TODECK = 507;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_FUSION_MAT = 511;
        private const long HINT_SELECT_BANISH = 512;
        private const long HINT_SELECT_DETACH = 519;

        // Turn-state tracking
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
        private bool _scheirenGYFusionUsed = false;
        private bool _havnisGYFusionUsed = false;
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

        public _2026_TearlaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── Combo Router Routes ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Reinoheart-Kitkallos-Fusion",
                RequiredCards = new List<int> { CardId.TearlamentsReinoheart },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.TearlamentsReinoheart, ActionType = ExecutorType.Summon, Description = "Summon Reinoheart" },
                    new() { CardId = CardId.TearlamentsReinoheart, ActionType = ExecutorType.Activate, Description = "Reinoheart send Havnis/Scheiren" },
                    new() { CardId = CardId.TearlamentsKitkallos, ActionType = ExecutorType.SpSummon, Description = "Fusion Kitkallos" },
                    new() { CardId = CardId.TearlamentsKitkallos, ActionType = ExecutorType.Activate, Description = "Kitkallos search/mill" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Fiendsmith-Requiem-Sequence",
                RequiredCards = new List<int> { CardId.FiendsmithEngraver },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.FiendsmithEngraver, ActionType = ExecutorType.Activate, Description = "Discard Engraver to search Tract" },
                    new() { CardId = CardId.FiendsmithsTract, ActionType = ExecutorType.Activate, Description = "Tract search Lurrie" },
                    new() { CardId = CardId.FiendsmithsRequiem, ActionType = ExecutorType.SpSummon, Description = "Link Requiem" },
                    new() { CardId = CardId.FiendsmithsSequence, ActionType = ExecutorType.SpSummon, Description = "Link Sequence" }
                },
                EndBoardScore = 90
            });

            BaitPlanner.RegisterComboStarters(CardId.TearlamentsReinoheart, CardId.BrilliantFusion, CardId.FiendsmithEngraver, CardId.FiendsmithsTract);
            ChainAdvisor.RegisterHighValueTargets(CardId.TearlamentsKitkallos, CardId.TearlamentsRulkallos, CardId.TearlamentsKaleidoHeart, CardId.FiendsmithsDesirae);
            ResourcePlan.RegisterAceCards(CardId.TearlamentsRulkallos, CardId.TearlamentsKaleidoHeart, CardId.FiendsmithsDesirae, CardId.SPLittleKnight, CardId.PilgrimReaper, CardId.KashtiraFenrir);

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE (Tiered Priority)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Negates, Handtraps & Interruptions ──
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);

            // In-Field Disruptions
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

            // ── Tier 1: Primary Starters & Ignition Spells ──
            // Kashtira Fenrir Inherent SS (Priority 1: summon before any other monster on empty field)
            AddExecutor(ExecutorType.SpSummon, CardId.KashtiraFenrir, FenrirSpSummon);

            AddExecutor(ExecutorType.Activate, CardId.InstantFusion, InstantFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.GemKnightQuartz, GemKnightQuartzEffect);
            AddExecutor(ExecutorType.Activate, CardId.BrilliantFusion, BrilliantFusionEffect);

            // Fiendsmith Line 1: Hand Discard -> Search Tract -> Activate Tract -> Lurrie SS
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, EngraverEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsTract, TractEffect);
            AddExecutor(ExecutorType.Activate, CardId.FabledLurrie, LurrieEffect);

            // Tearlaments Reinoheart Normal Summon & Trigger
            AddExecutor(ExecutorType.Summon, CardId.TearlamentsReinoheart, ReinoheartSummon);
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsReinoheart, ReinoheartEffect);

            // Tearlaments Scheiren & Havnis (Hand + GY Fusion)
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsScheiren, ScheirenEffect);
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsHavnis, HavnisEffect);

            // Tearlaments Kashtira Inherent SS & Mill
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsKashtira, TearKashtiraEffect);

            // Continuous Spell: Tearlaments Scream
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsScream, ScreamEffect);
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsHeartbeat, HeartbeatEffect);

            // Normal Summon Lurrie as backup starter into Requiem Link
            AddExecutor(ExecutorType.Summon, CardId.FabledLurrie, LurrieSummon);

            // ── Tier 2: GY Triggers (Tear Fusions, Kitkallos, Gem-Knight Nepyrim, Traps to GY) ──
            AddExecutor(ExecutorType.Activate, CardId.GemKnightNepyrim, NepyrimEffect);
            AddExecutor(ExecutorType.Activate, CardId.TearlamentsKitkallos, KitkallosEffect);

            // ── Tier 3: Extra Deck Link & Fusion Ladder ──
            // Fiendsmith Link/Fusion ladder
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsRequiem, RequiemSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsRequiem, RequiemEffect);
            AddExecutor(ExecutorType.Activate, CardId.LacrimaTheCrimsonTears, LacrimaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.MoonOfTheClosedHeaven, MoonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsSequence, SequenceSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsSequence, SequenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsLacrima, FiendsmithLacrimaEffect);

            // Xyz Ladder: Pilgrim Reaper (Mill 5) & Dugares
            AddExecutor(ExecutorType.SpSummon, CardId.PilgrimReaper, PilgrimReaperSummon);
            AddExecutor(ExecutorType.Activate, CardId.PilgrimReaper, PilgrimReaperEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.Number60Dugares, DugaresSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number60Dugares, DugaresEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.EvilswarmExcitonKnight, ExcitonKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.EvilswarmExcitonKnight, ExcitonKnightEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.TheUndyingLegion, UndyingLegionSummon);
            AddExecutor(ExecutorType.Activate, CardId.TheUndyingLegion, UndyingLegionEffect);

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
            _scheirenGYFusionUsed = false;
            _havnisGYFusionUsed = false;
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
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.TearlamentsRulkallos
                || card.Id == CardId.TearlamentsKaleidoHeart
                || card.Id == CardId.FiendsmithsDesirae
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
            if (c.Id == CardId.TearlamentsKaleidoHeart) return 800;
            if (c.Id == CardId.TearlamentsRulkallos) return 900;
            return base.GetMaterialPriority(c);
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 0: QUICK NEGATES & HANDTRAPS
        // ═══════════════════════════════════════════════════════════════

        private bool MaxxCEffect()
        {
            if (_maxxCActivatedThisTurn) return false;
            if (Duel.LastChainPlayer != 0)
            {
                _maxxCActivatedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool MulcharmyFuwalosEffect()
        {
            if (Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0 && Duel.Player == 1)
            {
                return true;
            }
            return false;
        }

        private bool AshBlossomEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                return true;
            }
            return false;
        }

        private bool DrollAndLockBirdEffect()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool TearlamentsCrymeEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Counter Trap activation
                if (Duel.LastChainPlayer == 1)
                {
                    // Select a monster from hand to send to GY (Tear monster preferred to trigger GY effect)
                    var sendTarget = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && (c.Id == CardId.TearlamentsScheiren || c.Id == CardId.TearlamentsHavnis || c.Id == CardId.TearlamentsReinoheart))
                                  ?? Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster());
                    if (sendTarget != null)
                    {
                        AI.SelectCard(sendTarget);
                        return true;
                    }
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                // If sent to GY, recycle banished Tear
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
                if (Duel.LastChainPlayer == 1)
                {
                    // Send 1 Tearlaments monster from hand or face-up field
                    var fodder = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Id != CardId.TearlamentsRulkallos && (c.Id == CardId.TearlamentsKitkallos || c.Id == CardId.TearlamentsReinoheart))
                              ?? Bot.Hand.FirstOrDefault(c => c != null && c.Id == CardId.TearlamentsScheiren || c.Id == CardId.TearlamentsHavnis)
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
                bool oppHasEternalSoul = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == 48680970);
                var oppTarget = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && (!oppHasEternalSoul || c.Id != 46986414));
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
            bool oppHasEternalSoul = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == 48680970);

            // 1. High Priority Floodgates & Key Spells/Traps (Eternal Soul, Circle, Union Hangar, Altergeist Protocol, Spoofing)
            var keySpells = new HashSet<int> { 48680970, 47222536, 66399653, 27541563, 53936268 };
            var spellTarget = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (keySpells.Contains(c.Id) || CardIntelligence.IsFloodgateSpellTrap(c.Id)));
            if (spellTarget != null) return spellTarget;

            // 2. High Priority Boss Monsters & Threats (Apollousa, ABC-Dragon Buster, Avramax, Spirit Dragon, Hope Harbinger, Azure-Eyes, Hexstia)
            var keyMonsters = new HashSet<int> { 4280258, 1561110, 21887175, 59822133, 63767246, 40908371, 1508649 };
            var threatMonster = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && keyMonsters.Contains(c.Id) && (!oppHasEternalSoul || c.Id != 46986414));
            if (threatMonster != null) return threatMonster;

            // 3. Highest ATK face-up monster (not immune)
            var monsterTarget = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup() && (!oppHasEternalSoul || c.Id != 46986414));
            if (monsterTarget != null) return monsterTarget;

            // 4. Any face-up spell
            var otherSpell = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
            if (otherSpell != null) return otherSpell;

            // 5. Fallback
            return Enemy.GetMonsters().FirstOrDefault(c => c != null && (!oppHasEternalSoul || c.Id != 46986414)) ?? Enemy.GetSpells().FirstOrDefault();
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
                AI.SelectCard(CardId.TearlamentsSulliek, CardId.TearlamentsScream, CardId.TearlamentsReinoheart);
                return true;
            }
            return false;
        }

        private bool KashtiraFenrirEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ActivateDescription == -1 || !_fenrirSearchUsed)
                {
                    _fenrirSearchUsed = true;
                    AI.SelectCard(CardId.TearlamentsKashtira, CardId.KashtiraFenrir);
                    return true;
                }
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
                // Only activate if opponent has a face-up monster threat (>= 2000 ATK) that can be flipped face-down
                var oppThreat = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attack >= 2000);
                if (oppThreat == null) return false;

                if (Duel.Player == 1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
                {
                    if (!_snowSummonedThisTurn)
                    {
                        // Protect our critical combo pieces!
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
                var oppTarget = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup());
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
            var priorityIds = new HashSet<int>
            {
                30012506, 77411244, 3405259, 1561110, 99249638, // ABC pieces, Buster & Driver
                71039903, 79814787, 89631139, 38517737, 6853254, // Blue-Eyes Stones, Dragon, Return
                7922915, 46986414, 2314238, // Dark Magician & Navigation
                42790071, 31444249, 97268402, 62742651 // Altergeist Multifaker, Meluseek, Silquitous, Hexstia
            };
            return Enemy.Graveyard.Where(c => c != null && priorityIds.Contains(c.Id))
                .Concat(Enemy.Graveyard.Where(c => c != null && (c.IsMonster() || c.IsSpell())))
                .Distinct()
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
                // Self recycle if our deck is getting low
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
            }
            return false;
        }

        private bool HavnisEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Opponent activated monster effect on field -> Special summon + mill 3!
                if (Duel.Player == 1 && Duel.LastChainPlayer == 1)
                {
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                // GY Fusion Summon: Priority Kitkallos -> Rulkallos -> Kaleido-Heart
                AI.SelectCard(CardId.TearlamentsKitkallos, CardId.TearlamentsRulkallos, CardId.TearlamentsKaleidoHeart);
                AI.SelectNextCard(CardId.TearlamentsKitkallos, CardId.TearlamentsReinoheart, CardId.TearlamentsScheiren, CardId.TearlamentsKashtira, CardId.GemKnightAmethyst);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 1: PRIMARY STARTERS & COMBO ROUTER
        // ═══════════════════════════════════════════════════════════════

        private bool InstantFusionEffect()
        {
            if (Bot.LifePoints > 1000)
            {
                // Summons Kitkallos!
                AI.SelectCard(CardId.TearlamentsKitkallos);
                return true;
            }
            return false;
        }

        private bool GemKnightQuartzEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_gemKnightQuartzUsed) return false;
                _gemKnightQuartzUsed = true;
                // Sets Brilliant Fusion from Deck
                AI.SelectCard(CardId.BrilliantFusion);
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
                // Sends Gem-Knight Nepyrim + Tearlaments Aqua (Scheiren or Havnis)
                AI.SelectCard(CardId.GemKnightAmethyst);
                AI.SelectNextCard(CardId.GemKnightNepyrim);
                AI.SelectThirdCard(CardId.TearlamentsScheiren, CardId.TearlamentsHavnis);
                return true;
            }
            return false;
        }

        private bool EngraverEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_engraverHandUsed) return false;
                _engraverHandUsed = true;
                AI.SelectCard(CardId.FiendsmithsTract);
                return true;
            }
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

        private bool TractEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_tractUsed) return false;
                _tractUsed = true;
                // Search Fabled Lurrie and discard Lurrie (triggering Lurrie SS!)
                AI.SelectCard(CardId.FabledLurrie, CardId.LacrimaTheCrimsonTears);
                AI.SelectNextCard(CardId.FabledLurrie, CardId.GemKnightNepyrim);
                return true;
            }
            return false;
        }

        private bool LurrieSummon()
        {
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

        private bool ScheirenEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_scheirenHandUsed) return false;
                // Send 1 monster from hand: Lurrie / Nepyrim / Tear monster / Snow / Keldo / Mudora
                var sendTarget = Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.FabledLurrie || c.Id == CardId.FairyTailSnow || c.Id == CardId.KeldoTheSacredProtector || c.Id == CardId.MudoraTheSwordOracle || c.Id == CardId.TearlamentsHavnis || c.Id == CardId.TearlamentsReinoheart))
                              ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && c.IsMonster() && !IsAceCard(c));
                if (sendTarget != null)
                {
                    _scheirenHandUsed = true;
                    AI.SelectCard(sendTarget);
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                // GY Fusion Summon: Priority Kitkallos -> Rulkallos -> Kaleido-Heart
                AI.SelectCard(CardId.TearlamentsKitkallos, CardId.TearlamentsRulkallos, CardId.TearlamentsKaleidoHeart);
                AI.SelectNextCard(CardId.TearlamentsKitkallos, CardId.TearlamentsReinoheart, CardId.TearlamentsHavnis, CardId.TearlamentsKashtira, CardId.GemKnightAmethyst);
                return true;
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
                // On summon: mill 3 cards
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // When sent to GY: mill 2 cards
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
                var oppST = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == 48680970 || c.Id == 47222536 || CardIntelligence.IsFloodgateSpellTrap(c.Id)))
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
                if (ActivateDescription == -1 || !_kitkallosSearchUsed)
                {
                    _kitkallosSearchUsed = true;
                    // Add or send Tearlaments card: Sulliek, Reinoheart, Scream
                    AI.SelectCard(CardId.TearlamentsSulliek, CardId.TearlamentsReinoheart, CardId.TearlamentsScream);
                    return true;
                }
                if (!_kitkallosSelfPopUsed)
                {
                    _kitkallosSelfPopUsed = true;
                    // Target Kitkallos herself to send to GY and SS Tear from hand/GY
                    // Sending Kitkallos to GY triggers her 3rd effect: MILL 5!
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
                // Fuse into Fiendsmith's Desirae or Lacrima
                AI.SelectCard(CardId.FiendsmithsDesirae, CardId.FiendsmithsLacrima);
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
            // Only summon Pilgrim Reaper if we need mills and have at least 20 cards in deck
            if (Bot.Deck.Count < 20) return false;
            // If we already have Rulkallos or Kaleido-Heart, keep Lacrima (2400 ATK) on field
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.TearlamentsRulkallos || c.Id == CardId.TearlamentsKaleidoHeart))) return false;
            int lv6 = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 6 && !IsAceCard(c));
            return lv6 >= 2;
        }

        private bool PilgrimReaperEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_pilgrimReaperUsed || Bot.Deck.Count < 15) return false;
                _pilgrimReaperUsed = true;
                // Detach 1 -> MILL 5 FOR BOTH PLAYERS!
                return true;
            }
            return false;
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

        private bool UndyingLegionSummon()
        {
            int lv7 = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 7);
            return lv7 >= 2;
        }

        private bool UndyingLegionEffect() => true;

        private bool SPLittleKnightSummon()
        {
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.SPLittleKnight)) return false;
            // Summon using monsters that have already activated their effects
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
    }
}
