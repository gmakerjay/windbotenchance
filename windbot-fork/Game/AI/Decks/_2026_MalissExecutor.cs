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
    // CARD AUDIT — 2026_Maliss (Cyberse / DARK / Banish & Revive)
    // ============================================================
    // | Card Name                                 | Type    | OPT? | Cost            | Effect Summary                                | Activate When                     | NEVER Activate When                    |
    // |-------------------------------------------|---------|------|-----------------|-----------------------------------------------|-----------------------------------|----------------------------------------|
    // | Maliss <P> Dormouse (32061192)            | Monster | Yes  | None            | Field: Banish Maliss from Deck, ATK +600      | Main Phase (Starter)              | No Maliss in Deck                      |
    // |                                           |         | Yes  | 300 LP          | Banished: SS itself, lock Extra to Links      | Banished                          | SS blocked / LP <= 1000                |
    // | Maliss <P> White Rabbit (69272449)        | Monster | Yes  | None            | Summon: Set 1 Maliss Trap from Deck           | Normal/Special Summoned           | GY has same Trap / no target in Deck   |
    // |                                           |         | Yes  | 300 LP          | Banished: SS itself, lock Extra to Links      | Banished                          | SS blocked / LP <= 1000                |
    // | Maliss <P> Cheshire Cat (96676583)        | Monster | Yes  | None            | Field: Draw 1, then banish 1 from hand        | Main Phase (Hand >= 1)            | Hand is empty                          |
    // |                                           |         | Yes  | 300 LP          | Banished: SS itself, lock Extra to Links      | Banished                          | SS blocked / LP <= 1000                |
    // | Maliss <P> March Hare (20938824)          | Monster | Yes  | None            | Hand: SS if control Maliss                    | Control Maliss monster            | SS blocked                             |
    // |                                           |         | Yes  | Banish self GY  | GY: SS 1 banished Maliss                      | Have banished Maliss monster      | SS blocked / effect used               |
    // | Wizard @Ignister (3723262)                | Monster | Yes  | Discard self    | Hand: Search Backup @Ignister                 | Main Phase                        | No Backup @Ignister in Deck            |
    // | Backup @Ignister (30118811)               | Monster | Yes  | None            | Hand: SS if control @Ignister                 | Control @Ignister monster         | SS blocked                             |
    // |                                           |         | Yes  | Banish self GY  | GY: SS 1 Level 4/lower @Ignister from hand    | Have @Ignister in hand            | SS blocked                             |
    // | Bystial Magnamhut (33854624)              | Monster | Yes  | Banish L/D GY   | Hand: SS by banishing Light/Dark from GY      | Opponent turn / disruption        | Our turn 1 / disrupts Cyberse links    |
    // | Bystial Druiswurm (6637331)               | Monster | Yes  | Banish L/D GY   | Hand: SS by banishing Light/Dark from GY      | Opponent turn / disruption        | Our turn 1 / disrupts Cyberse links    |
    // | Bystial Baldrake (72656408)               | Monster | Yes  | Banish L/D GY   | Hand: SS by banishing Light/Dark from GY      | Opponent turn / disruption        | Our turn 1 / disrupts Cyberse links    |
    // | Maliss <Q> Red Ransom (68059897)          | Link-2  | Yes  | None            | Summon: Search 1 Maliss Spell                 | Special Summoned                  | No Spell left in Deck                  |
    // |                                           |         | Yes  | 900 LP          | Banished: SS itself, banish Cyberse from Deck  | Banished                          | SS blocked / LP <= 1000                |
    // | Maliss <Q> Hearts Crypter (21848500)      | Link-3  | Yes  | Shuffle banish  | Field: (Quick) Shuffle banished -> banish fld | Any time disruption               | No banished Maliss / no target field   |
    // |                                           |         | Yes  | 900 LP          | Banished: SS itself, double its ATK           | Banished                          | SS blocked / LP <= 1000                |
    // | Cyberse Wicckid (52698008)                | Link-2  | Yes  | Banish GY       | Pointed zone SS: search Backup @Ignister      | Monster summoned to pointed zone  | No Cyberse in GY / no Tuner in Deck    |
    // | Haggard Lizardose (9763474)               | Link-2  | Yes  | Banish <=2000   | Field: Banish <=2000 ATK -> change ATK / draw | Need to trigger Maliss banish SS  | No valid banish target                 |
    // | Maliss <C> MTP-07 (94722358)              | Trap    | Yes  | Banish 1 Maliss | Banish Maliss -> search Maliss; pop if opp 3+  | Main Phase / opp turn             | No Maliss on field / target not in Deck|
    // | Maliss <C> TB-11 (57111661)               | Trap    | Yes  | Banish 1 Maliss | Banish Maliss -> SS Maliss Deck/ED if opp 3+  | Main Phase / opp turn             | No Maliss on field / SS blocked        |
    // | Maliss in Underground (68337209)          | Spell   | Yes  | Banish 1 Maliss | Banish 1 Maliss hand/deck/GY; Link ATK +3000  | Main Phase                        | Already active / no Maliss to banish   |
    // | Maliss in the Mirror (93453053)           | Spell   | Yes  | Banish 1 Maliss | Field: Negate monster; Banished: GY banish/add| Chain negate / Banished           | No target on field / GY empty          |
    // ============================================================
    // ACE CARDS: Primary: Hearts Crypter / Secondary: Firewall Dragon / Tertiary: Accesscode Talker
    // COMBO STARTERS: 1. Dormouse 2. Gold Sarcophagus 3. Maliss in Underground 4. Allure of Darkness
    // CHOKEPOINTS: Dormouse Normal Summon negated. Wicckid search negated.
    // ============================================================

    [Deck("2026_Maliss", "2026_Maliss")]
    public class _2026_MalissExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck — Maliss
            public const int Dormouse = 32061192;
            public const int WhiteRabbit = 69272449;
            public const int CheshireCat = 96676583;
            public const int MarchHare = 20938824;
            public const int MalissInUnderground = 68337209;
            public const int MalissInTheMirror = 93453053;
            public const int MalissCMTP07 = 94722358;
            public const int MalissCTB11 = 57111661;

            // Main Deck — Cyberse / Support
            public const int WizardIgnister = 3723262;
            public const int BackupIgnister = 30118811;
            public const int DimensionShifter = 91800273;
            public const int AshBlossom = 14558128;
            public const int AshBlossomAlt = 14558127;
            public const int DrollAndLockBird = 94145021;
            public const int BystialMagnamhut = 33854624;
            public const int BystialDruiswurm = 6637331;
            public const int BystialBaldrake = 72656408;
            public const int AllureOfDarkness = 1475311;
            public const int CalledByTheGrave = 24224830;
            public const int Terraforming = 73628505;
            public const int GoldSarcophagus = 75500286;
            public const int InfiniteImpermanence = 10045474;
            public const int DominusImpulse = 40366667;
            public const int DominusSpark = 6325660;

            // Extra Deck
            public const int HeartsCrypter = 21848500;
            public const int RedRansom = 68059897;
            public const int CyberseWicckid = 52698008;
            public const int LinkDisciple = 32995276;
            public const int FirewallDragon = 5043010;
            public const int LinkSpider = 98978921;
            public const int SalamangreatAlmiraj = 60303245;
            public const int Linguriboh = 24842059;
            public const int AlliedCodeTalkerIgnister = 39138610;
            public const int AccesscodeTalker = 86066372;
            public const int TranscodeTalker = 46947713;
            public const int Heatsoul = 61245672;
            public const int DharcCharmer = 8264361;
            public const int SPLittleKnight = 29301450;
            public const int HaggardLizardose = 9763474;

            // Side Deck (Staples)
            public const int GhostBelle = 73642296;
            public const int GhostOgre = 59438930;
        }

        // Dominus locks tracking
        private bool _dominusSparkHandLocked = false;
        private bool _dominusImpulseHandLocked = false;

        // Once-per-turn trackers
        private bool _dormouseBanishUsed = false;
        private bool _dormouseSSUsed = false;
        private bool _rabbitSetUsed = false;
        private bool _rabbitSSUsed = false;
        private bool _cheshireDrawUsed = false;
        private bool _cheshireSSUsed = false;
        private bool _marchHareSSUsed = false;
        private bool _wizardDiscardUsed = false;
        private bool _backupSSUsed = false;
        private bool _backupGYSSUsed = false;
        private bool _bystialMagnamhutUsed = false;
        private bool _bystialDruiswurmUsed = false;
        private bool _bystialBaldrakeUsed = false;
        private bool _redRansomSearchUsed = false;
        private bool _redRansomSSUsed = false;
        private bool _heartsCrypterShuffleUsed = false;
        private bool _heartsCrypterSSUsed = false;
        private bool _undergroundUsed = false;
        private bool _mirrorUsed = false;
        private bool _mirrorBanishUsed = false;
        private bool _mtp07Used = false;
        private bool _tb11Used = false;
        private bool _wicckidSearchUsed = false;
        private bool _heatsoulDrawUsed = false;
        private bool _alliedCodeTalkerNegateUsed = false;
        private bool _haggardBanishUsed = false;
        private bool _transcodeSSUsed = false;
        private bool _firewallBounceUsed = false;
        private bool _spLittleKnightBanishUsed = false;
        private bool _spLittleKnightQuickBanishUsed = false;

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.HeartsCrypter
                || card.Id == CardId.FirewallDragon
                || card.Id == CardId.AccesscodeTalker
                || card.Id == CardId.AlliedCodeTalkerIgnister
                || card.Id == CardId.Heatsoul
                || card.Id == CardId.TranscodeTalker
                || card.Id == CardId.SPLittleKnight;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.HeartsCrypter) && (Bot.HasInMonstersZone(CardId.FirewallDragon) || Bot.HasInMonstersZone(CardId.AlliedCodeTalkerIgnister) || Bot.HasInSpellZone(CardId.MalissCTB11) || Bot.HasInSpellZone(CardId.MalissCMTP07)))
                return true;
            if (Bot.HasInMonstersZone(CardId.AccesscodeTalker)) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (Bot.HasInMonstersZone(CardId.HeartsCrypter) && Bot.HasInMonstersZone(CardId.FirewallDragon))
                return true;
            if (Bot.HasInMonstersZone(CardId.HeartsCrypter) && Bot.HasInMonstersZone(CardId.AccesscodeTalker))
                return true;
            return base.ShouldStopExtending();
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.AshBlossom || c.Id == CardId.AshBlossomAlt || c.Id == CardId.DrollAndLockBird || c.Id == CardId.DimensionShifter)
                return 850;
            if (c.Id == CardId.MarchHare && !_marchHareSSUsed) return 700;
            if (c.Id == CardId.Dormouse && _dormouseBanishUsed) return 100;
            if (c.Id == CardId.WhiteRabbit && _rabbitSetUsed) return 100;
            if (c.Id == CardId.CheshireCat && _cheshireDrawUsed) return 100;
            if (c.Id == CardId.RedRansom && _redRansomSearchUsed) return 120;
            if (c.Id == CardId.CyberseWicckid && _wicckidSearchUsed) return 130;
            return base.GetMaterialPriority(c);
        }

        public _2026_MalissExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards
            HeuristicGuard.RegisterAceCards(
                CardId.HeartsCrypter,
                CardId.FirewallDragon,
                CardId.AccesscodeTalker,
                CardId.AlliedCodeTalkerIgnister,
                CardId.Heatsoul,
                CardId.TranscodeTalker,
                CardId.SPLittleKnight
            );
            ResourcePlan.RegisterAceCards(
                CardId.HeartsCrypter,
                CardId.FirewallDragon,
                CardId.AccesscodeTalker,
                CardId.AlliedCodeTalkerIgnister,
                CardId.Heatsoul,
                CardId.TranscodeTalker,
                CardId.SPLittleKnight
            );

            // ── Combo Router: Multi-branch sequencing ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Dormouse-Full-Combo",
                RequiredCards = new List<int> { CardId.Dormouse },
                FallbackLineName = "GoldSarc-Starter",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Dormouse, ActionType = ExecutorType.Summon, Description = "Normal Summon Dormouse" },
                    new() { CardId = CardId.Dormouse, ActionType = ExecutorType.Activate, Description = "Dormouse banish White Rabbit from Deck" },
                    new() { CardId = CardId.WhiteRabbit, ActionType = ExecutorType.Activate, Description = "White Rabbit SS from banish" },
                    new() { CardId = CardId.WhiteRabbit, ActionType = ExecutorType.Activate, Description = "White Rabbit Set Maliss Trap" },
                    new() { CardId = CardId.RedRansom, ActionType = ExecutorType.SpSummon, Description = "Link-2 Red Ransom" },
                    new() { CardId = CardId.RedRansom, ActionType = ExecutorType.Activate, Description = "Red Ransom search Underground" },
                    new() { CardId = CardId.MalissInUnderground, ActionType = ExecutorType.Activate, Description = "Activate Underground banish Cheshire" },
                    new() { CardId = CardId.CheshireCat, ActionType = ExecutorType.Activate, Description = "Cheshire Cat SS from banish" },
                    new() { CardId = CardId.HeartsCrypter, ActionType = ExecutorType.SpSummon, Description = "Link-3 Hearts Crypter" }
                },
                EndBoardScore = 95,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.Dormouse)
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "GoldSarc-Starter",
                RequiredCards = new List<int> { CardId.GoldSarcophagus },
                FallbackLineName = "Underground-Starter",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.GoldSarcophagus, ActionType = ExecutorType.Activate, Description = "Gold Sarcophagus banish Dormouse" },
                    new() { CardId = CardId.Dormouse, ActionType = ExecutorType.Activate, Description = "Dormouse SS from banish" },
                    new() { CardId = CardId.RedRansom, ActionType = ExecutorType.SpSummon, Description = "Link-2 Red Ransom" }
                },
                EndBoardScore = 90,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.GoldSarcophagus)
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Underground-Starter",
                RequiredCards = new List<int> { CardId.MalissInUnderground },
                FallbackLineName = "Cheshire-Starter",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.MalissInUnderground, ActionType = ExecutorType.Activate, Description = "Underground banish Dormouse" },
                    new() { CardId = CardId.Dormouse, ActionType = ExecutorType.Activate, Description = "Dormouse SS from banish" },
                    new() { CardId = CardId.RedRansom, ActionType = ExecutorType.SpSummon, Description = "Link-2 Red Ransom" }
                },
                EndBoardScore = 85,
                Condition = () => Bot.Hand.Any(c => c != null && (c.Id == CardId.MalissInUnderground || c.Id == CardId.Terraforming))
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Cheshire-Starter",
                RequiredCards = new List<int> { CardId.CheshireCat },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.CheshireCat, ActionType = ExecutorType.Summon, Description = "Normal Summon Cheshire Cat" },
                    new() { CardId = CardId.CheshireCat, ActionType = ExecutorType.Activate, Description = "Cheshire Cat draw & banish" }
                },
                EndBoardScore = 75,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.CheshireCat) && Bot.Hand.Count >= 2
            });

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(CardId.Dormouse, CardId.GoldSarcophagus, CardId.AllureOfDarkness, CardId.MalissInUnderground);
            BaitPlanner.RegisterBaitCards(CardId.Terraforming, CardId.GoldSarcophagus, CardId.AllureOfDarkness);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.Dormouse, CardId.CyberseWicckid, CardId.BackupIgnister, CardId.WizardIgnister, CardId.RedRansom);

            // ============================================================
            // TIER 1: Hand Traps & Reactive Negations
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionShifter, DimensionShifterEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusSpark, DominusSparkEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceEffect);

            // ============================================================
            // TIER 2: Quick Effects & Disruptions
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.HeartsCrypter, HeartsCrypterQuickBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.FirewallDragon, FirewallDragonBounceEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlliedCodeTalkerIgnister, AlliedCodeTalkerNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.Linguriboh, LinguribohTrapNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialDruiswurm, BystialQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialBaldrake, BystialQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.MalissInTheMirror, MirrorFieldEffect);

            // ============================================================
            // TIER 3: Setup Spells & Pre-Summon Starters
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingEffect);
            AddExecutor(ExecutorType.Activate, CardId.GoldSarcophagus, GoldSarcophagusEffect);
            AddExecutor(ExecutorType.Activate, CardId.AllureOfDarkness, AllureOfDarknessEffect);
            AddExecutor(ExecutorType.Activate, CardId.MalissInUnderground, UndergroundEffect);
            AddExecutor(ExecutorType.Activate, CardId.WizardIgnister, WizardDiscardEffect);

            // ============================================================
            // TIER 4: Normal Summons
            // ============================================================
            AddExecutor(ExecutorType.Summon, CardId.Dormouse, ShouldSummonDormouse);
            AddExecutor(ExecutorType.Summon, CardId.WhiteRabbit, ShouldSummonWhiteRabbit);
            AddExecutor(ExecutorType.Summon, CardId.CheshireCat, ShouldSummonCheshireCat);
            AddExecutor(ExecutorType.Summon, CardId.WizardIgnister, ShouldSummonWizardIgnister);
            AddExecutor(ExecutorType.Summon, CardId.MarchHare, ShouldSummonMarchHare);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.Dormouse);
            AddExecutor(ExecutorType.Summon, CardId.WhiteRabbit);
            AddExecutor(ExecutorType.Summon, CardId.CheshireCat);
            AddExecutor(ExecutorType.Summon, CardId.MarchHare);

            // ============================================================
            // TIER 5: Monster Effects, Banished Triggers & Fast Traps
            // ============================================================
            // Banished Trigger Effects
            AddExecutor(ExecutorType.Activate, CardId.Dormouse, DormouseSSFromBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.WhiteRabbit, WhiteRabbitSSFromBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.CheshireCat, CheshireCatSSFromBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.RedRansom, RedRansomSSFromBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeartsCrypter, HeartsCrypterSSFromBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.MalissInTheMirror, MirrorBanishEffect);

            // Field / Hand / GY Effects & Fast Traps
            AddExecutor(ExecutorType.Activate, CardId.Dormouse, DormouseBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.WhiteRabbit, WhiteRabbitSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.CheshireCat, CheshireCatDrawEffect);
            AddExecutor(ExecutorType.Activate, CardId.RedRansom, RedRansomSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.CyberseWicckid, WicckidEffect);
            AddExecutor(ExecutorType.Activate, CardId.MarchHare, MarchHareHandSSEffect);
            AddExecutor(ExecutorType.Activate, CardId.MarchHare, MarchHareGYBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.BackupIgnister, BackupSSEffect);
            AddExecutor(ExecutorType.Activate, CardId.BackupIgnister, BackupGYSSEffect);
            AddExecutor(ExecutorType.Activate, CardId.MalissCTB11, Tb11Effect);
            AddExecutor(ExecutorType.Activate, CardId.MalissCMTP07, Mtp07Effect);
            AddExecutor(ExecutorType.Activate, CardId.TranscodeTalker, TranscodeReviveEffect);
            AddExecutor(ExecutorType.Activate, CardId.Heatsoul, HeatsoulDrawEffect);
            AddExecutor(ExecutorType.Activate, CardId.HaggardLizardose, HaggardBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodePopEffect);

            // ============================================================
            // TIER 6: Special Summons (Hand / GY)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.MarchHare);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialMagnamhut);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialDruiswurm);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialBaldrake);

            // ============================================================
            // TIER 7: Extra Deck Summons (Climbing Order)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.CyberseWicckid, WicckidSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RedRansom, RedRansomSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TranscodeTalker, TranscodeSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HeartsCrypter, HeartsCrypterSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeTalkerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FirewallDragon, FirewallDragonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AlliedCodeTalkerIgnister, AlliedCodeTalkerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Heatsoul, HeatsoulSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Linguriboh, LinguribohSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HaggardLizardose, HaggardLizardoseSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DharcCharmer, DharcCharmerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SalamangreatAlmiraj, AlmirajSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LinkDisciple, LinkDiscipleSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LinkSpider, LinkSpiderSummon);

            // ============================================================
            // TIER 8: Traps & Sets
            // ============================================================
            AddExecutor(ExecutorType.SpellSet, CardId.MalissCMTP07);
            AddExecutor(ExecutorType.SpellSet, CardId.MalissCTB11);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusSpark);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusImpulse);

            // ============================================================
            // TIER 9: Monster Position
            // ============================================================
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Cyberse control/combo prefers going first
            return true;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            if (card != null && card.Controller == 0 && card.Location == CardLocation.Hand)
            {
                if (card.Id == CardId.DominusSpark)
                    _dominusSparkHandLocked = true;
                if (card.Id == CardId.DominusImpulse)
                    _dominusImpulseHandLocked = true;
            }
            base.OnChaining(player, card);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            
            _dormouseBanishUsed = false;
            _dormouseSSUsed = false;
            _rabbitSetUsed = false;
            _rabbitSSUsed = false;
            _cheshireDrawUsed = false;
            _cheshireSSUsed = false;
            _marchHareSSUsed = false;
            _wizardDiscardUsed = false;
            _backupSSUsed = false;
            _backupGYSSUsed = false;
            _bystialMagnamhutUsed = false;
            _bystialDruiswurmUsed = false;
            _bystialBaldrakeUsed = false;
            _redRansomSearchUsed = false;
            _redRansomSSUsed = false;
            _heartsCrypterShuffleUsed = false;
            _heartsCrypterSSUsed = false;
            _undergroundUsed = false;
            _mirrorUsed = false;
            _mirrorBanishUsed = false;
            _mtp07Used = false;
            _tb11Used = false;
            _wicckidSearchUsed = false;
            _heatsoulDrawUsed = false;
            _alliedCodeTalkerNegateUsed = false;
            _haggardBanishUsed = false;
            _transcodeSSUsed = false;
            _firewallBounceUsed = false;
            _spLittleKnightBanishUsed = false;
            _spLittleKnightQuickBanishUsed = false;
            _dominusSparkHandLocked = false;
            _dominusImpulseHandLocked = false;
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            if (card.HasType(CardType.Link))
            {
                var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
                if (activeAces.Count > 0)
                {
                    if (card.Id == CardId.AccesscodeTalker && (CanDealLethal() || OpponentHasActiveNegator() || Enemy.GetMonsterCount() > 0))
                        return true;

                    if (card.Id == CardId.HeartsCrypter && !Bot.HasInMonstersZone(CardId.HeartsCrypter))
                        return true;

                    var nonAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
                    int reqMats = card.LinkCount;
                    if (nonAces.Count < reqMats && !CanDealLethal())
                    {
                        DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} would consume Ace card without lethal");
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsSafeToAttack(ClientCard attacker)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (!enemy.IsDisabled())
                {
                    if (enemy.Id == 21887175 && attacker.IsSpecialSummoned) return false;
                    if (enemy.Id == 50954680 && attacker.Level >= 5) return false;
                }
                if (enemy.IsAttack())
                {
                    if (enemy.Attack > attacker.Attack) return false;
                }
                if (enemy.IsDefense() && attacker.Attack <= enemy.Defense) return false;
            }
            return true;
        }

        private bool IsSafeToDefend(ClientCard monster)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.Attack > monster.Defense) return false;
            }
            return true;
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty && !IsSafeToAttack(Card) && IsSafeToDefend(Card)) return true;
            }
            else
            {
                if (enemyEmpty || IsSafeToAttack(Card)) return true;
            }
            return false;
        }

        private int GetFreeMonsterZoneCount()
        {
            int count = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (Bot.MonsterZone[i] == null) count++;
            }
            return count;
        }

        // ============================================================
        // TIER 1: Hand Traps & Negations
        // ============================================================

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (_dominusSparkHandLocked) return false;
            return Duel.LastChainPlayer == 1 && DefaultAshBlossomAndJoyousSpring();
        }

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (_dominusSparkHandLocked || _dominusImpulseHandLocked) return false;
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool DimensionShifterEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            // CRITICAL: NEVER activate Dimension Shifter on our own Turn 1 going first! It blocks GY costs for Wicckid/Wizard!
            if (Duel.Player == 0 && Duel.Turn == 1) return false;
            return Bot.Graveyard.Count == 0;
        }

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            bool opponentHandTrapChained = LastChainCard != null && 
                (LastChainCard.Id == CardId.AshBlossom || 
                 LastChainCard.Id == CardId.AshBlossomAlt ||
                 LastChainCard.Id == CardId.DrollAndLockBird ||
                 LastChainCard.Id == CardId.GhostBelle ||
                 LastChainCard.Id == CardId.GhostOgre);

            if (opponentHandTrapChained)
                return DefaultCalledByTheGrave();

            if (LastChainCard != null && LastChainCard.Location == CardLocation.Grave)
                return DefaultCalledByTheGrave();

            return false;
        }

        private bool DominusSparkEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            // STRICT: NEVER activate Dominus Spark from Hand in Maliss deck! Hand activation permanently locks DARK effects!
            if (Card.Location == CardLocation.Hand) return false;

            return true;
        }

        private bool DominusImpulseEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            if (Card.Location == CardLocation.Hand)
            {
                // Only use on opponent's turn if we have no Ace monster on field
                if (Duel.Player == 0) return false;
                if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c))) return false;
            }

            return true;
        }

        private bool ImpermanenceEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && DefaultInfiniteImpermanence();
        }

        // ============================================================
        // TIER 2: Quick Effects & Disruptions
        // ============================================================

        private bool HeartsCrypterQuickBanishEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_heartsCrypterShuffleUsed) return false;

            bool hasBanishedMaliss = Bot.Banished.Any(c => c != null && c.HasSetcode(0x1b9));
            if (!hasBanishedMaliss) return false;

            bool hasEnemyTarget = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && IsViableEffectTarget(c))
                || Enemy.GetSpells().Any(c => c != null && IsViableEffectTarget(c));
            if (!hasEnemyTarget) return false;

            _heartsCrypterShuffleUsed = true;
            return true;
        }

        private bool SPLittleKnightEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;

            if (LastChainCard != null && LastChainCard.Controller == 1 && !_spLittleKnightQuickBanishUsed)
            {
                var ourMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
                var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c)).ToList();

                if (ourMonsters.Count > 0 && oppMonsters.Count > 0)
                {
                    _spLittleKnightQuickBanishUsed = true;
                    return true;
                }
            }

            if (!_spLittleKnightBanishUsed)
            {
                bool hasTarget = Enemy.GetMonsters().Any(c => c != null && IsViableEffectTarget(c))
                    || Enemy.Graveyard.Any(c => c != null && c.IsMonster())
                    || Enemy.GetSpells().Any(c => c != null && IsViableEffectTarget(c));
                if (hasTarget)
                {
                    _spLittleKnightBanishUsed = true;
                    return true;
                }
            }

            return false;
        }

        private bool FirewallDragonBounceEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_firewallBounceUsed) return false;
            if (_dominusImpulseHandLocked) return false;

            if (Duel.Player == 1 && Duel.LastChainPlayer == 1)
            {
                bool hasEnemyMonster = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && IsViableEffectTarget(c));
                if (hasEnemyMonster)
                {
                    _firewallBounceUsed = true;
                    return true;
                }
            }

            return false;
        }

        private bool AlliedCodeTalkerNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_alliedCodeTalkerNegateUsed) return false;

            if (Duel.LastChainPlayer == 1 && LastChainCard != null)
            {
                bool hasLinkTribute = Bot.GetMonsters().Any(c => c != null && c != Card && c.IsFaceup() && c.HasType(CardType.Link) && !IsAceCard(c));
                if (hasLinkTribute)
                {
                    _alliedCodeTalkerNegateUsed = true;
                    return true;
                }
            }

            return false;
        }

        private bool LinguribohTrapNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            return Duel.LastChainPlayer == 1 && LastChainCard != null && LastChainCard.HasType(CardType.Trap);
        }

        private bool BystialQuickEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool isMagnamhut = (Card.Id == CardId.BystialMagnamhut && !_bystialMagnamhutUsed);
            bool isDruiswurm = (Card.Id == CardId.BystialDruiswurm && !_bystialDruiswurmUsed);
            bool isBaldrake = (Card.Id == CardId.BystialBaldrake && !_bystialBaldrakeUsed);

            if (!isMagnamhut && !isDruiswurm && !isBaldrake) return false;

            bool oppHasTarget = Enemy.Graveyard.Any(c => c != null && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
            bool selfHasTarget = Bot.Graveyard.Any(c => c != null && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)) && !c.HasSetcode(0x1b9));

            // Only summon Bystials on opponent's turn (as disruption) or on Turn 2+ to break board / lethal
            if (Duel.Player == 1)
            {
                if (oppHasTarget)
                {
                    if (isMagnamhut) _bystialMagnamhutUsed = true;
                    if (isDruiswurm) _bystialDruiswurmUsed = true;
                    if (isBaldrake) _bystialBaldrakeUsed = true;
                    return true;
                }
            }
            else
            {
                if (Duel.Turn > 1 && (oppHasTarget || ShouldGoBreakBoard))
                {
                    if (isMagnamhut) _bystialMagnamhutUsed = true;
                    if (isDruiswurm) _bystialDruiswurmUsed = true;
                    if (isBaldrake) _bystialBaldrakeUsed = true;
                    return true;
                }
            }

            return false;
        }

        private bool MirrorFieldEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (_mirrorUsed) return false;

                bool hasEnemyFaceup = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && IsViableEffectTarget(c) && !c.IsDisabled());
                bool hasBanishSource = Bot.Hand.Any(c => c != null && c.HasSetcode(0x1b9) && c != Card)
                    || Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1b9));

                if (hasEnemyFaceup && hasBanishSource)
                {
                    _mirrorUsed = true;
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        // TIER 3: Setup Spells & Pre-Summon Effects
        // ============================================================

        private bool GoldSarcophagusEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasBanishTarget = GetRemainingCount(CardId.Dormouse) > 0
                || GetRemainingCount(CardId.WhiteRabbit) > 0
                || GetRemainingCount(CardId.CheshireCat) > 0;
            return hasBanishTarget;
        }

        private bool TerraformingEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            bool hasField = GetRemainingCount(CardId.MalissInUnderground) > 0;
            bool alreadyHasFieldActive = Bot.HasInSpellZone(CardId.MalissInUnderground);
            return hasField && !alreadyHasFieldActive;
        }

        private bool AllureOfDarknessEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            bool hasDark = Bot.Hand.Any(c => c != null && c != Card && (c.HasAttribute(CardAttribute.Dark) || c.HasSetcode(0x1b9)));
            if (hasDark) return true;
            bool handStuck = Bot.GetMonsterCount() == 0 && Bot.Hand.Count >= 3;
            return handStuck;
        }

        private bool UndergroundEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (_undergroundUsed) return false;
                if (Bot.HasInSpellZone(CardId.MalissInUnderground)) return false;

                bool hasBanishSource = Bot.Hand.Any(c => c != null && c.HasSetcode(0x1b9) && c != Card)
                    || HasRemainingCardWithSetcode(0x1b9)
                    || Bot.Graveyard.Any(c => c != null && c.HasSetcode(0x1b9));

                if (hasBanishSource)
                {
                    _undergroundUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool WizardDiscardEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_wizardDiscardUsed) return false;

            bool hasBackup = GetRemainingCount(CardId.BackupIgnister) > 0;
            if (hasBackup)
            {
                _wizardDiscardUsed = true;
                return true;
            }
            return false;
        }

        // ============================================================
        // TIER 4: Normal Summon Conditions
        // ============================================================

        private bool ShouldSummonDormouse()
        {
            bool hasDeckTarget = HasRemainingCardWithSetcode(0x1b9);
            return hasDeckTarget;
        }

        private bool ShouldSummonWhiteRabbit()
        {
            bool hasMTP = GetRemainingCount(CardId.MalissCMTP07) > 0
                && !Bot.Graveyard.Any(c => c != null && c.Id == CardId.MalissCMTP07);
            bool hasTB = GetRemainingCount(CardId.MalissCTB11) > 0
                && !Bot.Graveyard.Any(c => c != null && c.Id == CardId.MalissCTB11);
            return hasMTP || hasTB;
        }

        private bool ShouldSummonCheshireCat()
        {
            return Bot.Hand.Count >= 2;
        }

        private bool ShouldSummonWizardIgnister()
        {
            bool hasMalissInHand = Bot.Hand.Any(c => c != null && c.HasSetcode(0x1b9) && c.IsMonster());
            if (hasMalissInHand) return false;
            return true;
        }

        private bool ShouldSummonMarchHare()
        {
            bool hasOtherNS = Bot.Hand.Any(c => c != null && c != Card && (c.Id == CardId.Dormouse || c.Id == CardId.WhiteRabbit || c.Id == CardId.CheshireCat || c.Id == CardId.WizardIgnister));
            if (hasOtherNS) return false;
            return true;
        }

        // ============================================================
        // TIER 5: Monster Effects, Banished Triggers & Fast Traps
        // ============================================================

        private bool DormouseBanishEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_dormouseBanishUsed) return false;

            bool hasDeckTarget = HasRemainingCardWithSetcode(0x1b9);
            if (hasDeckTarget)
            {
                _dormouseBanishUsed = true;
                return true;
            }
            return false;
        }

        private bool DormouseSSFromBanishEffect()
        {
            if (Card.Location != CardLocation.Removed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_dormouseSSUsed) return false;
            if (Bot.LifePoints <= 1000) return false;

            _dormouseSSUsed = true;
            return true;
        }

        private bool WhiteRabbitSummonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_rabbitSetUsed) return false;

            bool hasMTP = GetRemainingCount(CardId.MalissCMTP07) > 0 && !Bot.Graveyard.Any(c => c != null && c.Id == CardId.MalissCMTP07);
            bool hasTB = GetRemainingCount(CardId.MalissCTB11) > 0 && !Bot.Graveyard.Any(c => c != null && c.Id == CardId.MalissCTB11);

            if (hasMTP || hasTB)
            {
                _rabbitSetUsed = true;
                return true;
            }
            return false;
        }

        private bool WhiteRabbitSSFromBanishEffect()
        {
            if (Card.Location != CardLocation.Removed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_rabbitSSUsed) return false;
            if (Bot.LifePoints <= 1000) return false;

            _rabbitSSUsed = true;
            return true;
        }

        private bool CheshireCatDrawEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_cheshireDrawUsed) return false;

            bool hasBanishSource = Bot.Hand.Any(c => c != null && c != Card);
            if (hasBanishSource)
            {
                _cheshireDrawUsed = true;
                return true;
            }
            return false;
        }

        private bool CheshireCatSSFromBanishEffect()
        {
            if (Card.Location != CardLocation.Removed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_cheshireSSUsed) return false;
            if (Bot.LifePoints <= 1000) return false;

            _cheshireSSUsed = true;
            return true;
        }

        private bool MarchHareHandSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_marchHareSSUsed) return false;
            if (GetFreeMonsterZoneCount() == 0) return false;

            bool hasMaliss = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1b9));
            if (hasMaliss)
            {
                _marchHareSSUsed = true;
                return true;
            }
            return false;
        }

        private bool MarchHareGYBanishEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_marchHareSSUsed) return false;

            bool hasBanishTarget = Bot.Banished.Any(c => c != null && c.HasSetcode(0x1b9) && c.IsCanRevive());
            if (hasBanishTarget && GetFreeMonsterZoneCount() > 0)
            {
                _marchHareSSUsed = true;
                return true;
            }
            return false;
        }

        private bool BackupSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_backupSSUsed) return false;

            bool hasIgnister = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.WizardIgnister || c.Id == CardId.BackupIgnister || c.Id == CardId.AlliedCodeTalkerIgnister || c.Id == CardId.CyberseWicckid));
            if (hasIgnister && GetFreeMonsterZoneCount() > 0)
            {
                _backupSSUsed = true;
                return true;
            }
            return false;
        }

        private bool BackupGYSSEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_backupGYSSUsed) return false;

            bool hasTarget = Bot.Hand.Any(c => c != null && (c.Id == CardId.WizardIgnister || c.Id == CardId.BackupIgnister));
            if (hasTarget && GetFreeMonsterZoneCount() > 0)
            {
                _backupGYSSUsed = true;
                return true;
            }
            return false;
        }

        private bool WicckidEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_wicckidSearchUsed) return false;

            bool hasGYBanish = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.Cyberse));
            bool hasTunerInDeck = GetRemainingCount(CardId.BackupIgnister) > 0;

            if (hasGYBanish && hasTunerInDeck)
            {
                _wicckidSearchUsed = true;
                return true;
            }
            return false;
        }

        private bool TranscodeReviveEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_transcodeSSUsed) return false;
            if (_dominusSparkHandLocked || _dominusImpulseHandLocked) return false;

            bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Link) && c.LinkCount <= 3 && c.IsCanRevive() && c.Id != CardId.TranscodeTalker);
            if (hasTarget && GetFreeMonsterZoneCount() > 0)
            {
                _transcodeSSUsed = true;
                return true;
            }
            return false;
        }

        private bool HeatsoulDrawEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_heatsoulDrawUsed) return false;
            if (_dominusSparkHandLocked) return false;
            if (Bot.LifePoints <= 1000) return false;

            _heatsoulDrawUsed = true;
            return true;
        }

        private bool HaggardBanishEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_haggardBanishUsed) return false;
            if (_dominusSparkHandLocked) return false;

            bool hasBanishTarget = Bot.GetMonsters().Concat(Bot.Graveyard)
                .Any(c => c != null && c.IsMonster() && c.HasSetcode(0x1b9) && c.Attack <= 2000);
            if (hasBanishTarget)
            {
                _haggardBanishUsed = true;
                return true;
            }
            return false;
        }

        private bool AccesscodePopEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;

            bool hasEnemyTarget = Enemy.GetMonsters().Any(c => c != null && IsViableEffectTarget(c))
                || Enemy.GetSpells().Any(c => c != null && IsViableEffectTarget(c));
            bool hasBanishCost = Bot.Graveyard.Concat(Bot.GetMonsters())
                .Any(c => c != null && c != Card && c.HasType(CardType.Link));

            return hasEnemyTarget && hasBanishCost;
        }

        private bool RedRansomSummonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_redRansomSearchUsed) return false;

            bool hasSpell = GetRemainingCount(CardId.MalissInUnderground) > 0 || GetRemainingCount(CardId.MalissInTheMirror) > 0;
            if (hasSpell)
            {
                _redRansomSearchUsed = true;
                return true;
            }
            return false;
        }

        private bool RedRansomSSFromBanishEffect()
        {
            if (Card.Location != CardLocation.Removed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_redRansomSSUsed) return false;
            if (Bot.LifePoints <= 1000) return false;

            _redRansomSSUsed = true;
            return true;
        }

        private bool HeartsCrypterSSFromBanishEffect()
        {
            if (Card.Location != CardLocation.Removed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_heartsCrypterSSUsed) return false;
            if (Bot.LifePoints <= 1000) return false;

            _heartsCrypterSSUsed = true;
            return true;
        }

        private bool MirrorBanishEffect()
        {
            if (Card.Location != CardLocation.Removed) return false;
            if (_mirrorBanishUsed) return false;

            bool hasGYMaliss = Bot.Graveyard.Any(c => c != null && c.HasSetcode(0x1b9));
            if (hasGYMaliss)
            {
                _mirrorBanishUsed = true;
                return true;
            }
            return false;
        }

        private bool Mtp07Effect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (_mtp07Used) return false;

                bool hasMaliss = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1b9));
                bool hasDeckSearch = HasRemainingMonsterWithSetcode(0x1b9);

                if (hasMaliss && hasDeckSearch)
                {
                    _mtp07Used = true;
                    return true;
                }
            }
            return false;
        }

        private bool Tb11Effect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (_tb11Used) return false;
                if (IsSpecialSummonBlocked()) return false;

                bool hasMaliss = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1b9));
                bool hasDeckTarget = HasRemainingMonsterWithSetcode(0x1b9);

                if (hasMaliss && hasDeckTarget)
                {
                    _tb11Used = true;
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        // TIER 7: Extra Deck Summons
        // ============================================================

        private bool IsMonsterExpendable(ClientCard c)
        {
            if (c == null) return false;
            if (c.IsDisabled()) return true;
            if (c.Id == CardId.Dormouse && _dormouseBanishUsed) return true;
            if (c.Id == CardId.WhiteRabbit && _rabbitSetUsed) return true;
            if (c.Id == CardId.CheshireCat && _cheshireDrawUsed) return true;
            if (c.Id == CardId.RedRansom && _redRansomSearchUsed) return true;
            if (c.Id == CardId.CyberseWicckid && _wicckidSearchUsed) return true;
            return false;
        }

        private bool WicckidSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.CyberseWicckid)) return false;
            if (GetRemainingCount(CardId.BackupIgnister) == 0) return false;
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Cyberse) && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool RedRansomSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.RedRansom)) return false;
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Cyberse) && !IsAceCard(c)).ToList();
            bool hasMaliss = materials.Any(c => c.HasSetcode(0x1b9));
            return materials.Count >= 2 && hasMaliss;
        }

        private bool TranscodeSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_dominusSparkHandLocked || _dominusImpulseHandLocked) return false;
            if (Bot.HasInMonstersZone(CardId.TranscodeTalker)) return false;

            bool hasReviveTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasType(CardType.Link) && c.LinkCount <= 3 && c.IsCanRevive());
            if (!hasReviveTarget && !CanDealLethal()) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool HeartsCrypterSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.HeartsCrypter)) return false;
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Cyberse) && !IsAceCard(c)).ToList();
            bool hasMaliss = materials.Any(c => c.HasSetcode(0x1b9));
            if (!hasMaliss) return false;

            bool hasLink2 = materials.Any(c => c.HasType(CardType.Link) && c.LinkCount == 2);
            if (hasLink2 && materials.Count >= 2) return true;
            return materials.Count >= 3;
        }

        private bool AccesscodeTalkerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;

            // Prioritize Accesscode on turn 2+ for OTK / board break
            if (Duel.Turn == 1 && !OpponentHasActiveNegator()) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && c.Id != CardId.AccesscodeTalker).ToList();
            bool hasLink3OrHigher = materials.Any(c => c.HasType(CardType.Link) && c.LinkCount >= 3);
            bool hasLink2AndOthers = materials.Any(c => c.HasType(CardType.Link) && c.LinkCount == 2) && materials.Count >= 3;
            return hasLink3OrHigher || hasLink2AndOthers || materials.Count >= 4;
        }

        private bool FirewallDragonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.FirewallDragon)) return false;
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Id != CardId.AccesscodeTalker && c.Id != CardId.HeartsCrypter).ToList();
            return materials.Count >= 3 || (materials.Count >= 2 && materials.Any(c => c.HasType(CardType.Link) && c.LinkCount >= 2));
        }

        private bool AlliedCodeTalkerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.AlliedCodeTalkerIgnister)) return false;
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Cyberse) && !IsAceCard(c)).ToList();
            return materials.Count >= 3 || (materials.Count >= 2 && materials.Any(c => c.HasType(CardType.Link) && c.LinkCount >= 2));
        }

        private bool SPLittleKnightSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            if (Bot.HasInMonstersZone(CardId.SPLittleKnight)) return false;

            bool needDisruption = Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
            if (!needDisruption && Duel.Turn == 1) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool HeatsoulSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_dominusSparkHandLocked) return false;
            if (Bot.HasInMonstersZone(CardId.Heatsoul)) return false;

            var attrs = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Cyberse) && !IsAceCard(c))
                .Select(c => c.Attribute).Distinct().ToList();
            return attrs.Count >= 2;
        }

        private bool LinguribohSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.Linguriboh)) return false;
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level <= 4 && !c.IsExtraCard() && c.HasRace(CardRace.Cyberse) && !IsAceCard(c)).ToList();
            if (materials.Count == 0) return false;
            if (materials.Count == 1 && !IsMonsterExpendable(materials[0])) return false;
            return true;
        }

        private bool HaggardLizardoseSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.HaggardLizardose)) return false;

            bool hasMalissToTrigger = Bot.Graveyard.Any(c => c != null && c.HasSetcode(0x1b9) && ((c.Id == CardId.RedRansom && !_redRansomSSUsed) || (c.Id == CardId.Dormouse && !_dormouseSSUsed) || (c.Id == CardId.WhiteRabbit && !_rabbitSSUsed) || (c.Id == CardId.CheshireCat && !_cheshireSSUsed)));
            if (!hasMalissToTrigger) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool DharcCharmerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.DharcCharmer)) return false;
            bool enemyHasDark = Enemy.Graveyard.Any(c => c != null && c.IsMonster() && c.HasAttribute(CardAttribute.Dark));
            if (!enemyHasDark) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            return materials.Count >= 2 && materials.Any(c => c.HasAttribute(CardAttribute.Dark));
        }

        private bool AlmirajSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_dominusSparkHandLocked) return false;
            if (Bot.HasInMonstersZone(CardId.SalamangreatAlmiraj)) return false;
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Attack <= 1000 && !c.IsSpecialSummoned && !IsAceCard(c)).ToList();
            if (materials.Count == 0) return false;
            if (materials.Count == 1 && materials[0].HasSetcode(0x1b9) && !IsMonsterExpendable(materials[0])) return false;
            return true;
        }

        private bool LinkDiscipleSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.LinkDisciple)) return false;
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level <= 4 && !c.IsExtraCard() && c.HasRace(CardRace.Cyberse) && !IsAceCard(c)).ToList();
            if (materials.Count == 0) return false;
            if (materials.Count == 1 && !IsMonsterExpendable(materials[0])) return false;
            return true;
        }

        private bool LinkSpiderSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.IsMonster() && c.HasType(CardType.Normal) && !IsAceCard(c)).ToList();
            return materials.Count > 0;
        }

        // ============================================================
        // Selection & Card / Option / Placement Hooks
        // ============================================================

        public override int OnSelectOption(IList<long> options)
        {
            return base.OnSelectOption(options);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            if (player == 0 && location == CardLocation.MonsterZone)
            {
                if (cardId == CardId.CyberseWicckid)
                {
                    int emzMask = (1 << 5) | (1 << 6);
                    int validEMZ = available & emzMask;
                    if (validEMZ > 0) return validEMZ;
                }

                var wicckid = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CyberseWicckid);
                if (wicckid != null)
                {
                    int wicckidIndex = Array.IndexOf(Bot.MonsterZone, wicckid);
                    if (wicckidIndex == 5)
                    {
                        int targetMask = (1 << 1) | (1 << 2);
                        int valid = available & targetMask;
                        if (valid > 0) return valid;
                    }
                    else if (wicckidIndex == 6)
                    {
                        int targetMask = (1 << 3) | (1 << 4);
                        int valid = available & targetMask;
                        if (valid > 0) return valid;
                    }
                }
            }
            return base.OnSelectPlace(cardId, player, location, available);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. If selecting opponent cards (banish / destroy / bounce / negate target)
            var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
            if (oppCards.Count > 0 && oppCards.Count == cards.Count)
            {
                var problem = oppCards.FirstOrDefault(c => c == Util.GetProblematicEnemyMonster() || c == Util.GetProblematicEnemySpell());
                if (problem != null) return Util.CheckSelectCount(new[] { problem }, cards, min, max);
                var sortedOpp = oppCards.OrderByDescending(c => c.Attack).ToList();
                return Util.CheckSelectCount(sortedOpp, cards, min, max);
            }

            // 2. If selecting Maliss Traps to Set (White Rabbit effect) (hint 510 or 509 or 506)
            var malissTrapsInDeck = cards.Where(c => c != null && c.Location == CardLocation.Deck && c.HasSetcode(0x1b9) && c.HasType(CardType.Trap)).ToList();
            if (malissTrapsInDeck.Count > 0 && malissTrapsInDeck.Count == cards.Count)
            {
                var tb = malissTrapsInDeck.FirstOrDefault(c => c.Id == CardId.MalissCTB11 && !Bot.Graveyard.Any(g => g != null && g.Id == CardId.MalissCTB11));
                if (tb != null) return Util.CheckSelectCount(new[] { tb }, cards, min, max);
                var mtp = malissTrapsInDeck.FirstOrDefault(c => c.Id == CardId.MalissCMTP07);
                if (mtp != null) return Util.CheckSelectCount(new[] { mtp }, cards, min, max);
                return Util.CheckSelectCount(malissTrapsInDeck, cards, min, max);
            }

            // 3. If selecting from Deck (Banish / Search / Summon)
            var deckCards = cards.Where(c => c != null && c.Location == CardLocation.Deck).ToList();
            if (deckCards.Count > 0)
            {
                // Banish from Deck (Underground / Dormouse / Gold Sarc / Red Ransom)
                if (hint == 503 || hint == 502 || hint == 504 || hint == 500 || (LastChainCard != null && (LastChainCard.Id == CardId.MalissInUnderground || LastChainCard.Id == CardId.Dormouse || LastChainCard.Id == CardId.GoldSarcophagus || LastChainCard.Id == CardId.RedRansom)))
                {
                    var deckMalissMonsters = deckCards.Where(c => c.HasSetcode(0x1b9) && c.IsMonster()).ToList();
                    if (deckMalissMonsters.Count > 0)
                    {
                        var bestBanish = deckMalissMonsters.FirstOrDefault(c => c.Id == CardId.Dormouse && !_dormouseSSUsed)
                            ?? deckMalissMonsters.FirstOrDefault(c => c.Id == CardId.WhiteRabbit && !_rabbitSSUsed)
                            ?? deckMalissMonsters.FirstOrDefault(c => c.Id == CardId.CheshireCat && !_cheshireSSUsed)
                            ?? deckMalissMonsters.FirstOrDefault(c => c.Id == CardId.MarchHare)
                            ?? deckMalissMonsters[0];
                        return Util.CheckSelectCount(new[] { bestBanish }, cards, min, max);
                    }
                }

                // Search Maliss Spells (Red Ransom)
                if (deckCards.Any(c => c.Id == CardId.MalissInUnderground || c.Id == CardId.MalissInTheMirror))
                {
                    bool hasField = Bot.HasInSpellZone(CardId.MalissInUnderground) || Bot.Hand.Any(c => c != null && c.Id == CardId.MalissInUnderground);
                    if (!hasField)
                    {
                        var field = deckCards.FirstOrDefault(c => c.Id == CardId.MalissInUnderground);
                        if (field != null) return Util.CheckSelectCount(new[] { field }, cards, min, max);
                    }
                    var mirror = deckCards.FirstOrDefault(c => c.Id == CardId.MalissInTheMirror);
                    if (mirror != null) return Util.CheckSelectCount(new[] { mirror }, cards, min, max);
                }

                // Search Backup @Ignister (Wicckid / Wizard)
                if (deckCards.Any(c => c.Id == CardId.BackupIgnister))
                {
                    var backup = deckCards.FirstOrDefault(c => c.Id == CardId.BackupIgnister);
                    if (backup != null) return Util.CheckSelectCount(new[] { backup }, cards, min, max);
                }

                // Search / Summon Maliss Monsters (MTP-07 / TB-11)
                var deckMalissSearch = deckCards.Where(c => c.HasSetcode(0x1b9) && c.IsMonster()).ToList();
                if (deckMalissSearch.Count > 0)
                {
                    var bestSearch = deckMalissSearch.FirstOrDefault(c => c.Id == CardId.Dormouse && !_dormouseBanishUsed)
                        ?? deckMalissSearch.FirstOrDefault(c => c.Id == CardId.MarchHare && !_marchHareSSUsed)
                        ?? deckMalissSearch.FirstOrDefault(c => c.Id == CardId.WhiteRabbit && !_rabbitSetUsed)
                        ?? deckMalissSearch.FirstOrDefault(c => c.Id == CardId.CheshireCat && !_cheshireDrawUsed)
                        ?? deckMalissSearch[0];
                    return Util.CheckSelectCount(new[] { bestSearch }, cards, min, max);
                }
            }

            // 4. If selecting face-up Maliss monsters on Field as Cost to Banish (MTP-07 / TB-11 / Mirror)
            var fieldMaliss = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && c.HasSetcode(0x1b9)).ToList();
            if (fieldMaliss.Count > 0 && (hint == 502 || hint == 503 || hint == 504 || (LastChainCard != null && (LastChainCard.Id == CardId.MalissCMTP07 || LastChainCard.Id == CardId.MalissCTB11 || LastChainCard.Id == CardId.MalissInTheMirror))))
            {
                var bestToBanish = fieldMaliss.FirstOrDefault(c => c.Id == CardId.RedRansom && !_redRansomSSUsed)
                    ?? fieldMaliss.FirstOrDefault(c => c.Id == CardId.WhiteRabbit && !_rabbitSSUsed)
                    ?? fieldMaliss.FirstOrDefault(c => c.Id == CardId.CheshireCat && !_cheshireSSUsed)
                    ?? fieldMaliss.FirstOrDefault(c => c.Id == CardId.Dormouse && !_dormouseSSUsed)
                    ?? fieldMaliss.OrderBy(c => c.Attack).First();
                return Util.CheckSelectCount(new[] { bestToBanish }, cards, min, max);
            }

            // 5. If selecting from Hand to Banish (Allure / Cheshire Cat / Underground)
            var handCards = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
            if (handCards.Count > 0 && (hint == 502 || hint == 503 || hint == 504 || (LastChainCard != null && (LastChainCard.Id == CardId.AllureOfDarkness || LastChainCard.Id == CardId.CheshireCat || LastChainCard.Id == CardId.MalissInUnderground))))
            {
                var lv3Maliss = handCards.Where(c => c.HasSetcode(0x1b9) && c.Level == 3).ToList();
                if (lv3Maliss.Count > 0)
                {
                    var bestHandBanish = lv3Maliss.FirstOrDefault(c => c.Id == CardId.Dormouse && !_dormouseSSUsed)
                        ?? lv3Maliss.FirstOrDefault(c => c.Id == CardId.WhiteRabbit && !_rabbitSSUsed)
                        ?? lv3Maliss.FirstOrDefault(c => c.Id == CardId.CheshireCat && !_cheshireSSUsed)
                        ?? lv3Maliss[0];
                    return Util.CheckSelectCount(new[] { bestHandBanish }, cards, min, max);
                }
                var otherDark = handCards.Where(c => c.HasAttribute(CardAttribute.Dark)).ToList();
                if (otherDark.Count > 0) return Util.CheckSelectCount(new[] { otherDark[0] }, cards, min, max);
            }

            // 6. If selecting from Graveyard (Wicckid cost / Haggard Lizardose / Transcode / March Hare)
            var gyCards = cards.Where(c => c != null && c.Location == CardLocation.Grave).ToList();
            if (gyCards.Count > 0)
            {
                // Banishing from GY (Wicckid / Haggard / Mirror)
                if (hint == 502 || hint == 503 || hint == 504 || (LastChainCard != null && (LastChainCard.Id == CardId.CyberseWicckid || LastChainCard.Id == CardId.HaggardLizardose)))
                {
                    var gyCyberse = gyCards.Where(c => c.HasRace(CardRace.Cyberse)).ToList();
                    if (gyCyberse.Count > 0)
                    {
                        var target = gyCyberse.FirstOrDefault(c => c.Id == CardId.RedRansom && !_redRansomSSUsed)
                            ?? gyCyberse.FirstOrDefault(c => c.Id == CardId.WizardIgnister)
                            ?? gyCyberse.FirstOrDefault(c => c.Id == CardId.Linguriboh || c.Id == CardId.LinkDisciple)
                            ?? gyCyberse.FirstOrDefault(c => c.Id == CardId.Dormouse && !_dormouseSSUsed)
                            ?? gyCyberse.FirstOrDefault(c => c.Id == CardId.WhiteRabbit && !_rabbitSSUsed)
                            ?? gyCyberse.FirstOrDefault(c => c.Id == CardId.CheshireCat && !_cheshireSSUsed)
                            ?? gyCyberse[0];
                        return Util.CheckSelectCount(new[] { target }, cards, min, max);
                    }
                }

                // Reviving from GY (Transcode Talker)
                var linkRevive = gyCards.Where(c => c.HasType(CardType.Link)).ToList();
                if (linkRevive.Count > 0)
                {
                    var bestRevive = linkRevive.FirstOrDefault(c => c.Id == CardId.HeartsCrypter)
                        ?? linkRevive.FirstOrDefault(c => c.Id == CardId.RedRansom)
                        ?? linkRevive.FirstOrDefault(c => c.Id == CardId.CyberseWicckid)
                        ?? linkRevive.OrderByDescending(c => c.LinkCount).First();
                    return Util.CheckSelectCount(new[] { bestRevive }, cards, min, max);
                }
            }

            // 7. If selecting from Banished Zone (Hearts Crypter shuffle / March Hare revival)
            var banishCards = cards.Where(c => c != null && c.Location == CardLocation.Removed).ToList();
            if (banishCards.Count > 0)
            {
                // Shuffle into deck (Hearts Crypter hint 512)
                if (hint == 512 || (LastChainCard != null && LastChainCard.Id == CardId.HeartsCrypter))
                {
                    var banishTraps = banishCards.Where(c => c.HasSetcode(0x1b9) && c.HasType(CardType.Trap)).ToList();
                    if (banishTraps.Count > 0) return Util.CheckSelectCount(new[] { banishTraps[0] }, cards, min, max);
                    var banishMaliss = banishCards.Where(c => c.HasSetcode(0x1b9)).ToList();
                    if (banishMaliss.Count > 0) return Util.CheckSelectCount(new[] { banishMaliss[0] }, cards, min, max);
                }

                // Special Summon from Banish (March Hare hint 509)
                var banishMonsters = banishCards.Where(c => c.HasSetcode(0x1b9) && c.IsMonster()).ToList();
                if (banishMonsters.Count > 0)
                {
                    var target = banishMonsters.FirstOrDefault(c => c.Id == CardId.HeartsCrypter)
                        ?? banishMonsters.FirstOrDefault(c => c.Id == CardId.RedRansom)
                        ?? banishMonsters.FirstOrDefault(c => c.Id == CardId.Dormouse)
                        ?? banishMonsters.FirstOrDefault(c => c.Id == CardId.WhiteRabbit)
                        ?? banishMonsters.FirstOrDefault(c => c.Id == CardId.CheshireCat)
                        ?? banishMonsters[0];
                    return Util.CheckSelectCount(new[] { target }, cards, min, max);
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.Dormouse || cardId == CardId.WhiteRabbit || cardId == CardId.CheshireCat || cardId == CardId.MarchHare)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }
    }

    [Deck("Expert_2026_Maliss", "2026_Maliss")]
    public class ExpertMalissExecutor : _2026_MalissExecutor
    {
        public ExpertMalissExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }

    [Deck("Neural_2026_Maliss", "2026_Maliss")]
    public class NeuralMalissExecutor : _2026_MalissExecutor
    {
        public NeuralMalissExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }
}
