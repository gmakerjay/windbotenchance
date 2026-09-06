// ============================================================
// CARD AUDIT — 2026_AFS (Azamina + Fiendsmith + Snake-Eye Meta Engine)
// ============================================================
// | Card Name                     | Type       | OPT? | HOPT? | Cost | Effect Summary                              | Activate When                               | NEVER Activate When                           |
// |-------------------------------|------------|------|-------|------|---------------------------------------------|---------------------------------------------|-----------------------------------------------|
// | Snake-Eye Ash                 | Monster L1 | Yes  | Yes   | None | NS/SS: Search Lv1 FIRE; Send 2 to SS from Dk| Main Phase starter / Extend                | Already used both HOPTs                       |
// | Snake-Eyes Poplar             | Monster L1 | Yes  | Yes   | None | Hand SS on search; Search S/T; GY S/T Place | Added to hand / Summoned / Sent to GY       | Already used HOPT                             |
// | Snake-Eye Oak                 | Monster L1 | Yes  | Yes   | None | NS/SS: Revive Lv1 FIRE; Send 2 to SS from Dk| Need Flamberge / Extend                     | Already used HOPT                             |
// | Snake-Eyes Flamberge Dragon   | Monster L8 | Yes  | Yes   | None | Place monster in S/T; Opp turn SS; GY SS 2  | On Field / Opp Turn / Sent to GY            | Already used HOPT                             |
// | Snake-Eyes Diabellstar        | Monster L8 | Yes  | Yes   | None | Attack S/T place; S/T SS self + place FIRE  | Battle / S/T Zone activate                  | Already used HOPT                             |
// | Diabellstar the Black Witch   | Monster L7 | Yes  | Yes   | Send | SS by sending 1; Set Sinful Spoils from Dk  | In Hand / On Summon                         | No sendable fodder / Already used HOPT        |
// | WANTED: Seeker of Sinful      | QuickSp    | Yes  | Yes   | None | Add Diabellstar from Deck/GY; GY recycle +1 | Main Phase search / GY draw                 | Already used HOPT                             |
// | Original Sinful Spoils        | Spell      | Yes  | Yes   | Send | Send face-up -> SS Lv1 FIRE from Deck; GY +1| Main Phase starter / Extend                 | Already used HOPT                             |
// | Divine Temple of the Snake-Eye| FieldSp    | Yes  | Yes   | None | Place Snake-Eye in S/T; +1100 ATK; Opp SS SS| Main Phase setup / Opponent summon          | Already active                                |
// | Bonfire                       | Spell      | Yes  | Yes   | None | Search Lv4 or lower Pyro (Snake-Eye Ash)    | Main Phase starter                          | Already used HOPT                             |
// | Deception of the Sinful Spoils| ContSpell  | Yes  | Yes   | Trib | Tribute 1 mon -> Search Azamina card; Burn  | Main Phase Azamina starter                  | No tribute / Already used HOPT                |
// | The Hallowed Azamina          | Spell      | Yes  | Yes   | Send | Reveal Azamina Fusion -> Send Sinful Spoils | In Hand, have Sinful Spoils in hand/field   | No Sinful Spoils fodder                       |
// | Azamina Ilia Silvia           | Fusion L6  | Yes  | Yes   | Trib | Omni-Negate Quick Effect (Tribute self); +2x| Opponent activates ANY card/effect          | No negate target                              |
// | Azamina Mu Rcielago           | Fusion L6  | Yes  | Yes   | None | On Fusion: Search Azamina/Sinful Spoils     | On Fusion Summon                            | Already used HOPT                             |
// | Fiendsmith Engraver           | Monster L6 | Yes  | Yes   | Disc | Discard -> Search Tract; GY Shuf LIGHT SS   | In Hand / In GY (Requiem/Moon in GY)        | No LIGHT Fiend to shuffle                     |
// | Fiendsmith's Tract            | Spell      | Yes  | Yes   | Disc | Search LIGHT Fiend + Discard; GY Fusion     | Main Phase Fiendsmith starter               | Already used HOPT                             |
// | Lacrima the Crimson Tears     | Monster L4 | Yes  | Yes   | None | NS/SS: Dump Fiendsmith; Opp turn revive Link| On Summon / In GY                           | Already used HOPT                             |
// | Fabled Lurrie                 | Monster L1 | No   | No    | None | Discarded to GY -> Special Summon self      | Discarded by Tract / Droplet / Diabellstar  | Zone full                                     |
// | Fiendsmith's Requiem          | Link 1     | Yes  | Yes   | Trib | Tribute self -> SS Fiendsmith from Deck     | On Field (LIGHT Fiend used as mat)          | Already used HOPT                             |
// | Fiendsmith's Sequence         | Link 2     | Yes  | Yes   | Shuf | Shuffle GY mats -> Fusion Summon Fiend      | Main Phase (Lacrima + Engraver in GY)       | Materials not in GY                           |
// | Fiendsmith's Lacrima          | Fusion L6  | Yes  | Yes   | None | On Fusion: Revive Engraver (Rank 6 setup)   | On Fusion Summon                            | Engraver not in GY                            |
// | D/D/D Wave High King Caesar   | Xyz R6     | Yes  | Yes   | Det 1| Double Special Summon Omni-Negate & Destroy | Opponent Special Summons or activates SS eff| Out of Xyz materials                          |
// | Promethean Princess           | Link 3     | Yes  | Yes   | None | SS FIRE from GY; GY Quick Pop + SS self     | Main Phase revive / Opponent Special Summons| FIRE lock active (already finished Caesar)    |
// | S:P Little Knight             | Link 2     | Yes  | Yes   | None | On Link: Banish 1 card; Quick double banish | On Link Summon / Opponent activates effect  | Already used HOPT                             |
// | I:P Masquerena                | Link 2     | Yes  | Yes   | None | Opponent Turn Quick Link into S:P / Access  | Opponent Main Phase                         | Already used HOPT                             |
// | Accesscode Talker             | Link 4     | Yes  | No    | Banish Link | +3000 ATK; Multi non-targeting pop  | Turn 2 board breaking / Lethal OTK push     | Turn 1 setup                                  |
// | Worldsea Dragon Zealantis     | Link 4     | Yes  | Yes   | None | Field wipe + Co-link destroy                | Battle Phase / Board breaking               | Turn 1 setup                                  |
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
    [Deck("2026_AFS", "2026_AFS")]
    public class _2026_AFSExecutor : ModernExecutor
    {
        public class CardId
        {
            // Snake-Eye Engine
            public const int SnakeEyesFlambergeDragon = 48452496;
            public const int SnakeEyesDiabellstar = 27260347;
            public const int DiabellstarTheBlackWitch = 72270339;
            public const int SnakeEyeAsh = 9674034;
            public const int SnakeEyesPoplar = 90241276;
            public const int SnakeEyeOak = 45663742;
            public const int WantedSeekerOfSinfulSpoils = 80845034;
            public const int OriginalSinfulSpoilsSnakeEye = 89023486;
            public const int DivineTempleOfTheSnakeEye = 53639887;
            public const int Bonfire = 85106525;

            // Azamina Engine
            public const int DeceptionOfTheSinfulSpoils = 66328392;
            public const int TheHallowedAzamina = 94845588;
            public const int AzaminaIliaSilvia = 46396218;
            public const int AzaminaMuRcielago = 73391962;

            // Fiendsmith Engine
            public const int FiendsmithEngraver = 60764609;
            public const int LacrimaTheCrimsonTears = 28803166;
            public const int FabledLurrie = 97651498;
            public const int FiendsmithsTract = 98567237;
            public const int FiendsmithsRequiem = 2463794;
            public const int FiendsmithsSequence = 49867899;
            public const int FiendsmithsLacrima = 46640168;
            public const int DDDWaveHighKingCaesar = 79559912;
            public const int MoonOfTheClosedHeaven = 71818935;

            // Handtraps & Staples
            public const int MulcharmyFuwalos = 42141493;
            public const int AshBlossom = 14558127;
            public const int GhostBelle = 73642296;
            public const int MaxxC = 23434538;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;
            public const int ForbiddenDroplet = 24299458;
            public const int TripleTacticsTalent = 25311006;
            public const int InfiniteImpermanence = 10045474;
            public const int SkillDrain = 82732705;
            public const int LightningStorm = 14532163;
            public const int DarkRulerNoMore = 54693926;

            // Extra Deck Link Bosses
            public const int WorldseaDragonZealantis = 45112597;
            public const int SalamangreatRagingPhoenix = 57134592;
            public const int PrometheanPrincess = 2772337;
            public const int AccesscodeTalker = 86066372;
            public const int HiitaTheFireCharmer = 48815792;
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
            public const int Linkuriboh = 41999284;
        }

        // Standard OCG Hint IDs
        private const long HINT_SELECT_FACEUP = 500;
        private const long HINT_SELECT_TOGRAVE = 501;
        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_DISCARD = 504;
        private const long HINT_SELECT_TOHAND = 506;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_FUSION_MAT = 511;
        private const long HINT_SELECT_BANISH = 512;
        private const long HINT_SELECT_SET = 514;
        private const long HINT_SELECT_DETACH = 519;

        // Turn-state tracking
        private bool _wantedUsed = false;
        private bool _bonfireUsed = false;
        private bool _diabellstarSpSummonUsed = false;
        private bool _diabellstarSetUsed = false;
        private bool _snakeEyeAshSearchUsed = false;
        private bool _snakeEyeAshSummonFromDeckUsed = false;
        private bool _snakeEyesPoplarSearchUsed = false;
        private bool _snakeEyeOakSummonUsed = false;
        private bool _flambergeSTPlaceUsed = false;
        private bool _flambergeGYTriggerUsed = false;
        private bool _deceptionUsed = false;
        private bool _hallowedAzaminaUsed = false;
        private bool _engraverHandUsed = false;
        private bool _engraverGYUsed = false;
        private bool _tractUsed = false;
        private bool _lacrimaSummonUsed = false;
        private bool _requiemUsed = false;
        private bool _sequenceUsed = false;
        private bool _fiendsmithLacrimaUsed = false;
        private bool _prometheanPrincessUsed = false;
        private bool _maxxCActivatedThisTurn = false;

        public _2026_AFSExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Register Ace Cards in ResourcePlanner ──
            ResourcePlan.RegisterAceCards(
                CardId.DDDWaveHighKingCaesar,
                CardId.AzaminaIliaSilvia,
                CardId.AzaminaMuRcielago,
                CardId.SnakeEyesFlambergeDragon,
                CardId.PrometheanPrincess,
                CardId.SPLittleKnight,
                CardId.IPMasquerena,
                CardId.AccesscodeTalker,
                CardId.SalamangreatRagingPhoenix,
                CardId.WorldseaDragonZealantis
            );

            // ── 2. Register Starters and Baits in BaitPlanner ──
            BaitPlanner.RegisterComboStarters(
                CardId.SnakeEyeAsh,
                CardId.Bonfire,
                CardId.WantedSeekerOfSinfulSpoils,
                CardId.DiabellstarTheBlackWitch,
                CardId.DeceptionOfTheSinfulSpoils,
                CardId.TheHallowedAzamina,
                CardId.FiendsmithEngraver,
                CardId.FiendsmithsTract,
                CardId.OriginalSinfulSpoilsSnakeEye
            );
            BaitPlanner.RegisterBaitCards(
                CardId.ForbiddenDroplet,
                CardId.TripleTacticsTalent,
                CardId.DivineTempleOfTheSnakeEye
            );

            // ── 3. Register Combo Lines in ComboRouter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "AFS-TripleEngine-OmniBoard",
                RequiredCards = new List<int> { CardId.SnakeEyeAsh },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.SnakeEyeAsh, ActionType = ExecutorType.Summon, Description = "Normal Summon Snake-Eye Ash -> Search Poplar" },
                    new() { CardId = CardId.SnakeEyesPoplar, ActionType = ExecutorType.Activate, Description = "Poplar SS from hand -> Search Original Sinful Spoils / Temple" },
                    new() { CardId = CardId.Linkuriboh, ActionType = ExecutorType.SpSummon, Description = "Link Summon Linkuriboh using Poplar -> Poplar places self in S/T" },
                    new() { CardId = CardId.SnakeEyeAsh, ActionType = ExecutorType.Activate, Description = "Ash sends self + S/T Poplar -> SS Flamberge Dragon from Deck" },
                    new() { CardId = CardId.MoonOfTheClosedHeaven, ActionType = ExecutorType.SpSummon, Description = "Link Summon Moon of the Closed Heaven (LIGHT Fiend bridge)" },
                    new() { CardId = CardId.FiendsmithsRequiem, ActionType = ExecutorType.SpSummon, Description = "Link Summon Fiendsmith Requiem -> SS Lacrima from Deck" },
                    new() { CardId = CardId.FiendsmithsSequence, ActionType = ExecutorType.SpSummon, Description = "Link Summon Sequence -> Fusion Summon Fiendsmith Lacrima" },
                    new() { CardId = CardId.DDDWaveHighKingCaesar, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon D/D/D Wave High King Caesar (Double SS Negate)" },
                    new() { CardId = CardId.TheHallowedAzamina, ActionType = ExecutorType.Activate, Description = "The Hallowed Azamina -> Fusion Summon Azamina Ilia Silvia (Omni-Negate)" }
                },
                FallbackLineName = "Fiendsmith-Caesar-Line"
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Fiendsmith-Caesar-Line",
                RequiredCards = new List<int> { CardId.FiendsmithEngraver },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.FiendsmithEngraver, ActionType = ExecutorType.Activate, Description = "Engraver searches Fiendsmith's Tract" },
                    new() { CardId = CardId.FiendsmithsTract, ActionType = ExecutorType.Activate, Description = "Tract searches and discards Fabled Lurrie -> Lurrie SS" },
                    new() { CardId = CardId.FiendsmithsRequiem, ActionType = ExecutorType.SpSummon, Description = "Lurrie into Requiem -> Requiem SS Lacrima from Deck" },
                    new() { CardId = CardId.DDDWaveHighKingCaesar, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Caesar" }
                }
            });

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE (Tiered Priority)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Negates, Handtraps & Interruptions ──
            AddExecutor(ExecutorType.Activate, CardId.DDDWaveHighKingCaesar, CaesarNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.AzaminaIliaSilvia, SilviaNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrometheanPrincess, PrometheanPrincessGYQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, IPMasquerenaEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainEffect);

            // ── Tier 0.5: Board Breakers (Going 2nd) ──
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreEffect);

            // ── Tier 1: Primary Starters (Bonfire / WANTED / Ash / Engraver / Tract) ──
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.WantedSeekerOfSinfulSpoils, WantedEffect);
            AddExecutor(ExecutorType.Activate, CardId.Bonfire, BonfireEffect);

            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, EngraverEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsTract, TractEffect);
            AddExecutor(ExecutorType.Activate, CardId.FabledLurrie, LurrieEffect);

            AddExecutor(ExecutorType.Summon, CardId.SnakeEyeAsh, SnakeEyeAshSummon);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyeAsh, SnakeEyeAshEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyesPoplar, PoplarEffect);
            AddExecutor(ExecutorType.Summon, CardId.SnakeEyesPoplar, PoplarSummon);

            // Diabellstar Inherent Special Summon & Trigger
            AddExecutor(ExecutorType.SpSummon, CardId.DiabellstarTheBlackWitch, DiabellstarSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DiabellstarTheBlackWitch, DiabellstarSetEffect);
            AddExecutor(ExecutorType.Activate, CardId.DivineTempleOfTheSnakeEye, DivineTempleEffect);

            // ── Tier 2: Azamina Fusion & Sinful Spoils Spells ──
            AddExecutor(ExecutorType.Activate, CardId.TheHallowedAzamina, HallowedAzaminaEffect);
            AddExecutor(ExecutorType.Activate, CardId.DeceptionOfTheSinfulSpoils, DeceptionEffect);
            AddExecutor(ExecutorType.Activate, CardId.AzaminaMuRcielago, MuRcielagoSearchEffect);
            AddExecutor(ExecutorType.Summon, CardId.SnakeEyeOak, SnakeEyeOakSummon);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyeOak, SnakeEyeOakEffect);
            AddExecutor(ExecutorType.Activate, CardId.OriginalSinfulSpoilsSnakeEye, OriginalSinfulSpoilsEffect);

            // ── Tier 3: Fiendsmith Extra Deck Engine (Requiem -> Lacrima -> Sequence -> Caesar) ──
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsRequiem, RequiemSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsRequiem, RequiemEffect);
            AddExecutor(ExecutorType.Activate, CardId.LacrimaTheCrimsonTears, LacrimaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsSequence, SequenceSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsSequence, SequenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsLacrima, FiendsmithLacrimaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DDDWaveHighKingCaesar, CaesarSummon);

            // ── Tier 4: Snake-Eye Flamberge & Promethean Princess Link Engine ──
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyesFlambergeDragon, FlambergeEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnakeEyesDiabellstar, SnakeEyesDiabellstarEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, LinkuribohSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MoonOfTheClosedHeaven, MoonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PrometheanPrincess, PrometheanPrincessSummon);
            AddExecutor(ExecutorType.Activate, CardId.PrometheanPrincess, PrometheanPrincessMainEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.HiitaTheFireCharmer, HiitaSummon);
            AddExecutor(ExecutorType.Activate, CardId.HiitaTheFireCharmer, HiitaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightOnSummonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, IPMasquerenaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SalamangreatRagingPhoenix, RagingPhoenixSummon);
            AddExecutor(ExecutorType.Activate, CardId.SalamangreatRagingPhoenix, RagingPhoenixEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.WorldseaDragonZealantis, ZealantisSummon);
            AddExecutor(ExecutorType.Activate, CardId.WorldseaDragonZealantis, ZealantisEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeTalkerSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeTalkerEffect);

            // ── Tier 5: Spell & Trap Sets ──
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.CrossoutDesignator, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.WantedSeekerOfSinfulSpoils, DefaultSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.SkillDrain, DefaultSpellSet);

            // ── Tier 6: Monster Reposition ──
            AddExecutor(ExecutorType.Repos, MonsterReposOverride);
        }

        public override bool OnSelectHand() => true;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _wantedUsed = false;
            _bonfireUsed = false;
            _diabellstarSpSummonUsed = false;
            _diabellstarSetUsed = false;
            _snakeEyeAshSearchUsed = false;
            _snakeEyeAshSummonFromDeckUsed = false;
            _snakeEyesPoplarSearchUsed = false;
            _snakeEyeOakSummonUsed = false;
            _flambergeSTPlaceUsed = false;
            _flambergeGYTriggerUsed = false;
            _deceptionUsed = false;
            _hallowedAzaminaUsed = false;
            _engraverHandUsed = false;
            _engraverGYUsed = false;
            _tractUsed = false;
            _lacrimaSummonUsed = false;
            _requiemUsed = false;
            _sequenceUsed = false;
            _fiendsmithLacrimaUsed = false;
            _prometheanPrincessUsed = false;
            _maxxCActivatedThisTurn = false;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.DDDWaveHighKingCaesar
                || card.Id == CardId.AzaminaIliaSilvia
                || card.Id == CardId.AzaminaMuRcielago
                || card.Id == CardId.SnakeEyesFlambergeDragon
                || card.Id == CardId.PrometheanPrincess
                || card.Id == CardId.SPLittleKnight
                || card.Id == CardId.IPMasquerena
                || card.Id == CardId.AccesscodeTalker
                || card.Id == CardId.SalamangreatRagingPhoenix
                || card.Id == CardId.WorldseaDragonZealantis;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Controller == 1) return 1; // Opponent monsters first!
            if (c.Id == CardId.FabledLurrie) return 2;
            if (c.Id == CardId.SnakeEyesPoplar) return 3;
            if (c.Id == CardId.Linkuriboh) return 4;
            if (c.Id == CardId.FiendsmithsRequiem) return 5;
            if (c.Id == CardId.MoonOfTheClosedHeaven) return 6;
            if (c.Id == CardId.SnakeEyeOak) return 7;
            if (c.Id == CardId.SnakeEyeAsh) return 8;
            if (c.Id == CardId.LacrimaTheCrimsonTears) return 9;
            if (c.Id == CardId.FiendsmithEngraver) return 10;
            if (c.Id == CardId.FiendsmithsLacrima) return 11;
            if (c.Id == CardId.HiitaTheFireCharmer) return 12;
            if (c.Id == CardId.DiabellstarTheBlackWitch) return 15;
            if (c.Id == CardId.SnakeEyesFlambergeDragon) return 20;
            if (c.Id == CardId.PrometheanPrincess) return 25;
            if (c.Id == CardId.AzaminaMuRcielago) return 30;
            if (c.Id == CardId.AzaminaIliaSilvia) return 800; // Protect Silvia Omni-Negate!
            if (c.Id == CardId.DDDWaveHighKingCaesar) return 900; // Protect Caesar Omni-Negate!
            return base.GetMaterialPriority(c);
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 0: QUICK NEGATES, HANDTRAPS & INTERRUPTIONS
        // ═══════════════════════════════════════════════════════════════

        private bool CaesarNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool SilviaNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool PrometheanPrincessGYQuickEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var ourFire = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Fire && !IsAceCard(c))
                           ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Fire);
                var oppTarget = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup());

                if (ourFire != null && oppTarget != null)
                {
                    AI.SelectCard(ourFire);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool SPLittleKnightQuickEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer == 1)
            {
                var ourTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c != Card && !IsAceCard(c)) ?? Card;
                var oppTarget = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup())
                             ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());

                if (ourTarget != null && oppTarget != null)
                {
                    AI.SelectCard(ourTarget);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool IPMasquerenaEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.Player != 1) return false; // Opponent Turn Link Summon
            if (Bot.GetMonsterCount() >= 2) return true;
            return false;
        }

        private bool AshBlossomEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool GhostBelleEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultGhostBelleAndHauntedMansion();
        }

        private bool MaxxCEffect()
        {
            if (_maxxCActivatedThisTurn) return false;
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                _maxxCActivatedThisTurn = true;
                return true;
            }
            return DefaultMaxxC();
        }

        private bool MulcharmyFuwalosEffect()
        {
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                return true;
            }
            return false;
        }

        private bool CalledByTheGraveEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                return DefaultCalledByTheGrave();
            }
            return false;
        }

        private bool CrossoutDesignatorEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = LastChainCard;
                if (lastCard != null && Bot.Deck.Any(c => c != null && c.Id == lastCard.Id))
                {
                    AI.SelectCard(lastCard.Id);
                    return true;
                }
            }
            return false;
        }

        private bool ForbiddenDropletEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var chainCard = LastChainCard;
                if (chainCard != null && chainCard.IsMonster() && chainCard.IsFaceup() && !chainCard.IsDisabled())
                    return true;
            }
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                var oppEffectMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.HasType(CardType.Effect)).ToList();
                if (oppEffectMonsters.Count > 0 && (Bot.Hand.Count > 1 || Bot.GetMonsterCount() > 2))
                    return true;
            }
            return false;
        }

        private bool SkillDrainEffect()
        {
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c)))
                return false;
            return Enemy.GetMonsterCount() >= 2 && Duel.Player == 1;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 1 & 2: ENGINE SEARCH, FUSION & LEVEL 1 STARTERS
        // ═══════════════════════════════════════════════════════════════

        private bool TripleTacticsTalentEffect()
        {
            if (Enemy.GetMonsterCount() > 0)
            {
                AI.SelectOption(0); // Draw 2 cards
                return true;
            }
            return false;
        }

        private bool WantedEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (_wantedUsed) return false;
                _wantedUsed = true;
                AI.SelectCard(CardId.DiabellstarTheBlackWitch);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true; // Draw 1 card
            }
            return false;
        }

        private bool BonfireEffect()
        {
            if (_bonfireUsed) return false;
            _bonfireUsed = true;
            AI.SelectCard(CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar);
            return true;
        }

        private bool DeceptionEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_deceptionUsed) return false;
                _deceptionUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (!_hallowedAzaminaUsed && !Bot.HasInHand(CardId.TheHallowedAzamina))
                {
                    var tribute = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c));
                    if (tribute != null)
                    {
                        AI.SelectCard(tribute);
                        AI.SelectNextCard(CardId.TheHallowedAzamina);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HallowedAzaminaEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_hallowedAzaminaUsed) return false;
                _hallowedAzaminaUsed = true;

                // Reveal Azamina Ilia Silvia (Omni-Negate) -> send 1 Sinful Spoils -> SS Silvia!
                AI.SelectCard(CardId.AzaminaIliaSilvia);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool MuRcielagoSearchEffect()
        {
            AI.SelectCard(CardId.DeceptionOfTheSinfulSpoils, CardId.WantedSeekerOfSinfulSpoils, CardId.OriginalSinfulSpoilsSnakeEye);
            return true;
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
                AI.SelectCard(CardId.FabledLurrie, CardId.LacrimaTheCrimsonTears);
                return true;
            }
            return false;
        }

        private bool LurrieEffect() => true;

        private bool DiabellstarSpSummon()
        {
            if (_diabellstarSpSummonUsed) return false;
            var fodder = Bot.Hand.FirstOrDefault(c => c != null && c.Id != CardId.DiabellstarTheBlackWitch && c.Id != CardId.SnakeEyeAsh && c.Id != CardId.Bonfire)
                      ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c));

            if (fodder != null)
            {
                _diabellstarSpSummonUsed = true;
                AI.SelectCard(fodder);
                return true;
            }
            return false;
        }

        private bool DiabellstarSetEffect()
        {
            if (_diabellstarSetUsed) return false;
            _diabellstarSetUsed = true;
            AI.SelectCard(CardId.OriginalSinfulSpoilsSnakeEye, CardId.DeceptionOfTheSinfulSpoils, CardId.WantedSeekerOfSinfulSpoils);
            return true;
        }

        private bool DivineTempleEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                AI.SelectCard(CardId.SnakeEyesFlambergeDragon, CardId.SnakeEyesPoplar, CardId.SnakeEyeAsh);
                return true;
            }
            return false;
        }

        private bool SnakeEyeAshSummon() => true;

        private bool SnakeEyeAshEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ActivateDescription == -1 || !_snakeEyeAshSearchUsed)
                {
                    _snakeEyeAshSearchUsed = true;
                    AI.SelectCard(CardId.SnakeEyesPoplar, CardId.SnakeEyeOak);
                    return true;
                }
                if (!_snakeEyeAshSummonFromDeckUsed)
                {
                    _snakeEyeAshSummonFromDeckUsed = true;
                    AI.SelectCard(CardId.SnakeEyesFlambergeDragon, CardId.SnakeEyeOak);
                    return true;
                }
            }
            return false;
        }

        private bool PoplarSummon() => true;

        private bool PoplarEffect()
        {
            if (Card.Location == CardLocation.Hand) return true; // SS from hand
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_snakeEyesPoplarSearchUsed) return false;
                _snakeEyesPoplarSearchUsed = true;
                AI.SelectCard(CardId.DivineTempleOfTheSnakeEye, CardId.OriginalSinfulSpoilsSnakeEye);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool SnakeEyeOakSummon() => true;

        private bool SnakeEyeOakEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_snakeEyeOakSummonUsed) return false;
                _snakeEyeOakSummonUsed = true;
                AI.SelectCard(CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar);
                return true;
            }
            return false;
        }

        private bool OriginalSinfulSpoilsEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                var fodder = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == CardId.SnakeEyesPoplar || c.Id == CardId.DeceptionOfTheSinfulSpoils))
                          ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c));

                if (fodder != null)
                {
                    AI.SelectCard(fodder);
                    AI.SelectNextCard(CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar);
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 3 & 4: EXTRA DECK SUMMONS (CAESAR, PRINCESS, S:P, ACCESSCODE)
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
            return false;
        }

        private bool LacrimaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_lacrimaSummonUsed) return false;
                _lacrimaSummonUsed = true;
                AI.SelectCard(CardId.FiendsmithEngraver);
                return true;
            }
            return false;
        }

        private bool SequenceSummon()
        {
            if (_sequenceUsed) return false;
            int fiends = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Race == (int)CardRace.Fiend);
            return fiends >= 2;
        }

        private bool SequenceEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                _sequenceUsed = true;
                return true;
            }
            return false;
        }

        private bool FiendsmithLacrimaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_fiendsmithLacrimaUsed) return false;
                _fiendsmithLacrimaUsed = true;
                AI.SelectCard(CardId.FiendsmithEngraver);
                return true;
            }
            return false;
        }

        private bool CaesarSummon()
        {
            int lv6Fiends = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 6 && c.Race == (int)CardRace.Fiend);
            return lv6Fiends >= 2;
        }

        private bool FlambergeEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_flambergeSTPlaceUsed)
                {
                    _flambergeSTPlaceUsed = true;
                    var target = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup())
                              ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                if (!_flambergeGYTriggerUsed)
                {
                    _flambergeGYTriggerUsed = true;
                    AI.SelectCard(CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar, CardId.SnakeEyeOak);
                    return true;
                }
            }
            return false;
        }

        private bool SnakeEyesDiabellstarEffect()
        {
            return true;
        }

        private bool LinkuribohSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.SnakeEyesPoplar);
        }

        private bool MoonSummon()
        {
            if (_requiemUsed || Bot.HasInMonstersZone(CardId.DDDWaveHighKingCaesar)) return false;
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 2;
        }

        private bool PrometheanPrincessSummon()
        {
            if (_prometheanPrincessUsed) return false;
            if (!_requiemUsed && !_sequenceUsed && Bot.GetMonsters().Any(c => c != null && c.Attribute == (int)CardAttribute.Light && c.Race == (int)CardRace.Fiend))
                return false;

            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 3 || (mats >= 2 && Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Link)));
        }

        private bool PrometheanPrincessMainEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_prometheanPrincessUsed) return false;
                _prometheanPrincessUsed = true;
                AI.SelectCard(CardId.SnakeEyesFlambergeDragon, CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar);
                return true;
            }
            return false;
        }

        private bool HiitaSummon()
        {
            int fires = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Fire && !IsAceCard(c));
            return fires >= 2;
        }

        private bool HiitaEffect()
        {
            var target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.Attribute == (int)CardAttribute.Fire);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SPLittleKnightSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 2 && !Bot.HasInMonstersZone(CardId.SPLittleKnight);
        }

        private bool SPLittleKnightOnSummonEffect()
        {
            var target = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup())
                      ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup())
                      ?? Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool IPMasquerenaSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 2 && !Bot.HasInMonstersZone(CardId.IPMasquerena) && Duel.Turn == 1;
        }

        private bool RagingPhoenixSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Attribute == (int)CardAttribute.Fire);
            return mats >= 3;
        }

        private bool RagingPhoenixEffect() => true;

        private bool ZealantisSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());
            return mats >= 4 || (mats >= 2 && Bot.GetMonsters().Any(c => c != null && c.HasType(CardType.Link) && c.LinkCount >= 3));
        }

        private bool ZealantisEffect() => true;

        private bool AccesscodeTalkerSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());
            return (mats >= 4 || (mats >= 2 && Bot.GetMonsters().Any(c => c != null && c.HasType(CardType.Link) && c.LinkCount >= 3))) && Duel.Turn > 1;
        }

        private bool AccesscodeTalkerEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(c => c != null);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TACTICAL DECISION OVERRIDES (OnSelectCard / Position)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Diabellstar the Black Witch send cost from hand/field
            if (LastChainCard != null && LastChainCard.Id == CardId.DiabellstarTheBlackWitch && (hint == HINT_SELECT_TOGRAVE || hint == HINT_SELECT_DISCARD))
            {
                return SelectPreferredCard(cards, min, max,
                    CardId.FabledLurrie,
                    CardId.SnakeEyesPoplar,
                    CardId.DeceptionOfTheSinfulSpoils,
                    CardId.WantedSeekerOfSinfulSpoils,
                    CardId.DivineTempleOfTheSnakeEye);
            }

            // Fiendsmith's Tract search & discard
            if (LastChainCard != null && LastChainCard.Id == CardId.FiendsmithsTract)
            {
                if (hint == HINT_SELECT_TOHAND)
                {
                    return SelectPreferredCard(cards, min, max, CardId.FabledLurrie, CardId.LacrimaTheCrimsonTears, CardId.FiendsmithEngraver);
                }
                if (hint == HINT_SELECT_DISCARD || hint == HINT_SELECT_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max, CardId.FabledLurrie, CardId.SnakeEyesPoplar, CardId.LacrimaTheCrimsonTears);
                }
            }

            // The Hallowed Azamina fusion & send cost
            if (LastChainCard != null && LastChainCard.Id == CardId.TheHallowedAzamina)
            {
                if (hint == HINT_SELECT_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.DeceptionOfTheSinfulSpoils,
                        CardId.WantedSeekerOfSinfulSpoils,
                        CardId.OriginalSinfulSpoilsSnakeEye);
                }
            }

            // Snake-Eye Ash & Oak send 2 face-up cards cost
            if (LastChainCard != null && (LastChainCard.Id == CardId.SnakeEyeAsh || LastChainCard.Id == CardId.SnakeEyeOak))
            {
                if (hint == HINT_SELECT_TOGRAVE)
                {
                    return SelectPreferredCard(cards, min, max,
                        CardId.SnakeEyesPoplar,
                        CardId.DeceptionOfTheSinfulSpoils,
                        CardId.DivineTempleOfTheSnakeEye,
                        CardId.SnakeEyeOak,
                        CardId.SnakeEyeAsh);
                }
            }

            // Snake-Eyes Flamberge Dragon revive 2 Level 1 FIRE
            if (LastChainCard != null && LastChainCard.Id == CardId.SnakeEyesFlambergeDragon && (hint == HINT_SELECT_SPSUMMON || hint == 0))
            {
                return SelectPreferredCard(cards, min, max, CardId.SnakeEyeAsh, CardId.SnakeEyesPoplar, CardId.SnakeEyeOak);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.DDDWaveHighKingCaesar || cardId == CardId.AzaminaIliaSilvia || cardId == CardId.SnakeEyesFlambergeDragon || cardId == CardId.AccesscodeTalker || cardId == CardId.SalamangreatRagingPhoenix || cardId == CardId.WorldseaDragonZealantis || cardId == CardId.SPLittleKnight)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            if (cardId == CardId.FabledLurrie || cardId == CardId.LacrimaTheCrimsonTears || cardId == CardId.FiendsmithsLacrima || cardId == CardId.AzaminaMuRcielago || cardId == CardId.SnakeEyesPoplar)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        private IList<ClientCard> SelectPreferredCard(IList<ClientCard> cards, int min, int max, params int[] preferredIds)
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

        private bool MonsterReposOverride()
        {
            if (Card == null) return false;
            if (IsAceCard(Card))
            {
                if (Card.IsAttack()) return false;
                return true;
            }

            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty && Card.Attack < 1200) return true;
            }
            else
            {
                if (enemyEmpty || Card.Defense < Card.Attack) return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  BATTLE & ATTACK LOGIC
        // ═══════════════════════════════════════════════════════════════

        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0)
                return null;

            if (defenders == null || defenders.Count == 0)
            {
                var directAttacker = attackers
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack > 0)
                    .OrderBy(c => IsAceCard(c) ? 0 : 1)
                    .ThenByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (directAttacker != null)
                {
                    return AI.Attack(directAttacker, null);
                }
            }

            foreach (var attacker in attackers.Where(c => c != null && c.IsFaceup() && c.IsAttack()).OrderByDescending(c => c.Attack))
            {
                foreach (var defender in defenders.Where(d => d != null))
                {
                    if (defender.IsFaceup() && defender.IsAttack() && attacker.Attack > defender.Attack)
                        return AI.Attack(attacker, defender);
                    if (defender.IsFaceup() && defender.IsDefense() && attacker.Attack > defender.Defense)
                        return AI.Attack(attacker, defender);
                    if (defender.IsFacedown() && attacker.Attack >= 1800)
                        return AI.Attack(attacker, defender);
                }
            }

            return null;
        }
        // ═══════════════════════════════════════════════════════════════
        //  BOARD BREAKER EFFECTS (Going 2nd)
        // ═══════════════════════════════════════════════════════════════

        private bool LightningStormEffect()
        {
            // Lightning Storm requires no face-up cards on our field
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup()) ||
                Bot.GetSpells().Any(c => c != null && c.IsFaceup()))
                return false;

            // Prioritize destroying monsters if enemy has 2+, otherwise S/T
            if (Enemy.GetMonsterCount() >= 2)
            {
                AI.SelectOption(0); // Destroy all ATK position monsters
                return true;
            }
            if (Enemy.GetSpellCount() >= 2)
            {
                AI.SelectOption(1); // Destroy all Spells/Traps
                return true;
            }
            if (Enemy.GetMonsterCount() > 0)
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

        private bool DarkRulerNoMoreEffect()
        {
            // Use when opponent has 2+ face-up monsters to negate
            return Enemy.GetMonsterCount() >= 2;
        }
    }
}
