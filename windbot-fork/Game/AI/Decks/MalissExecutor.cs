using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugins;

namespace WindBot.Game.AI.Decks
{
    // =========================================================================
    // CARD AUDIT — Special_Maliss (Cyberse / DARK / Banish, Link Climb & Disruption)
    // =========================================================================
    // | Card Name                                 | ID        | Type    | OPT? | Cost            | Role / Effect Summary                                 |
    // |-------------------------------------------|-----------|---------|------|-----------------|-------------------------------------------------------|
    // | Maliss <P> Dormouse                       | 32061192  | Monster | Yes  | None            | Starter: Banish Maliss from Deck, ATK +600            |
    // |                                           |           |         | Yes  | 300 LP          | Banished: SS itself, lock Extra to Links              |
    // | Maliss <P> White Rabbit                   | 69272449  | Monster | Yes  | None            | Summon: Set 1 Maliss Trap from Deck                   |
    // |                                           |           |         | Yes  | 300 LP          | Banished: SS itself, lock Extra to Links              |
    // | Maliss <P> Chessy Cat                     | 96676583  | Monster | Yes  | Banish Maliss H | Main: Banish Maliss from hand, draw 2                 |
    // |                                           |           |         | Yes  | 300 LP          | Banished: SS itself, lock Extra to Links              |
    // | Maliss <P> March Hare                     | 20938824  | Monster | Yes  | Banish Maliss   | Hand (Quick): Banish Maliss H/GY, SS itself           |
    // |                                           |           |         | Yes  | 300 LP          | Banished: Pay 300 LP, add banished Maliss monster     |
    // | Wizard @Ignister                          | 3723262   | Monster | Yes  | None            | Hand: Target DARK Cyberse in GY, SS both              |
    // | Backup @Ignister                          | 30118811  | Monster | Yes  | None            | Hand: SS if control ED Cyberse; Search DARK Cyberse   |
    // | The Phantom Knights of Doomed Soleret     | 83566725  | Monster | Yes  | None            | Hand: SS if empty field; Summon: Set Umbrage Veil     |
    // | Santa Claws                               | 46565218  | Monster | No   | Tribute enemy   | Board Breaker: Tribute enemy problem monster to DEF   |
    // | Bystial Magnamhut                         | 33854624  | Monster | Yes  | Banish L/D GY   | Disruption: Quick SS by banishing L/D from either GY  |
    // | Bystial Druiswurm                         | 6637331   | Monster | Yes  | Banish L/D GY   | Disruption: Quick SS; Leaves field -> send enemy SS   |
    // | Bystial Baldrake                          | 72656408  | Monster | Yes  | Banish L/D GY   | Disruption: Quick SS; Opponent ED summon -> Banish it |
    // | Ash Blossom & Joyous Spring               | 14558127  | Monster | Yes  | Discard         | Handtrap: Negate search / deck special summon / mill  |
    // | Maxx "C"                                  | 23434538  | Monster | Yes  | Send to GY      | Handtrap: Draw on every opponent Special Summon       |
    // | Droll & Lock Bird                         | 94145021  | Monster | Yes  | Send to GY      | Handtrap: Lock searching for the rest of turn         |
    // | Mulcharmy Fuwalos                         | 42141493  | Monster | Yes  | Discard         | Handtrap: Draw when opp summons from Deck / Extra     |
    // | Mulcharmy Purulia                         | 84192580  | Monster | Yes  | Discard         | Handtrap: Draw when opp summons from Hand             |
    // | Dimension Shifter                         | 91800273  | Monster | Yes  | Send to GY      | Handtrap: Macro Cosmos for 2 turns (NEVER turn 1 T1!) |
    // | Allure of Darkness                        | 1475311   | Spell   | No   | Banish DARK H   | Draw 2, banish 1 DARK (triggers Maliss banished!)     |
    // | Called by the Grave                       | 24224830  | Spell   | No   | Banish opp GY   | Negate opponent handtrap or GY monster effect         |
    // | Triple Tactics Talent                     | 25311006  | Spell   | Yes  | None            | Draw 2 / Steal monster / Hand rip                     |
    // | Maliss in Underground                     | 68337209  | Spell   | Yes  | Banish 1 Maliss | Field: Banish Maliss hand/deck/GY; Link ATK +3000     |
    // | Terraforming                              | 73628505  | Spell   | No   | None            | Search Maliss in Underground                          |
    // | Gold Sarcophagus                          | 75500286  | Spell   | No   | None            | Banish Dormouse / Rabbit from Deck (Triggers SS!)     |
    // | Maliss in the Mirror                      | 93453053  | Spell   | Yes  | Banish 1 Maliss | Quick Negate; Banished: search Maliss card            |
    // | Pot of Sloth                              | 98476659  | Spell   | Yes  | None            | Board Break: Draw = opp cards, bottom deck draw-1     |
    // | Dominus Spark                             | 6325660   | Trap    | Yes  | None            | Banish opp monster (NEVER activate from hand!)        |
    // | Dominus Impulse                           | 40366667  | Trap    | Yes  | None            | Negate special summon effect (hand only if no Ace!)   |
    // | Solemn Judgment                           | 41420027  | Trap    | No   | Half LP         | Counter Trap: Negate summon or Spell/Trap activation  |
    // | Solemn Accusation                         | 78114463  | Trap    | No   | 1500 / 3000 LP  | Counter Trap: Negate Spell/Trap, lock/banish copies   |
    // | Maliss <C> GWC-06                         | 20726052  | Trap    | Yes  | Banish Maliss f | Fast Trap: SS banished/GY Maliss, gain LP = ATK      |
    // | Maliss <C> TB-11                          | 57111661  | Trap    | Yes  | Banish Maliss f | Fast Trap: SS Maliss from Deck or Extra Deck          |
    // | Maliss <C> MTP-07                         | 94722358  | Trap    | Yes  | Banish Maliss f | Fast Trap: Search Maliss, banish 1 card on field      |
    // | The Phantom Knights of Umbrage Veil       | 85257384  | Trap    | Yes  | None            | Fast Trap: SS as Level 3 Normal Monster for Link     |
    // | Haggard Lizardose                         | 9763474   | Link-2  | Yes  | Banish <=2000   | Trigger Maliss banish SS, draw if Reptile             |
    // | Maliss <Q> Hearts Crypter (ACE)           | 21848500  | Link-3  | Yes  | Shuffle banish  | Quick Banish 1 card (UNNEGATABLE if points to mon)    |
    // |                                           |           |         | Yes  | 900 LP          | Banished: SS itself with 5000 ATK!                    |
    // | Maliss <Q> Red Ransom                     | 68059897  | Link-3  | Yes  | None            | SS: Search Maliss Spell; Switch opp ATK/DEF           |
    // |                                           |           |         | Yes  | 900 LP          | Banished: SS itself, banish Cyberse from Deck         |
    // | Maliss <Q> White Binder                   | 95454996  | Link-3  | Yes  | None            | SS: Banish 3 in GYs; Main: Set Maliss Trap Deck/GY    |
    // |                                           |           |         | Yes  | 900 LP          | Banished: SS itself, draw 1 card                     |
    // | Allied Code Talker @Ignister (ACE)        | 39138610  | Link-5  | Yes  | Tribute Link    | SS 2300 ATK Cyberse from GY; Quick Omni-Negate/Banish |
    // | Accesscode Talker (ACE)                   | 86066372  | Link-4  | No   | Banish Link GY  | 5300 ATK OTK, unchainable multi-pops                  |
    // | Knightmare Gryphon (ACE)                  | 65330383  | Link-4  | Yes  | Discard 1       | Floodgate: Unlinked monsters cannot activate; Set ST  |
    // | Transcode Talker                          | 46947713  | Link-3  | Yes  | None            | Revive Link-3 Cyberse from GY; Co-link +500/untarget  |
    // | Splash Mage                               | 59859086  | Link-2  | Yes  | None            | Revive Cyberse from GY in Defense                    |
    // | Cyberse Wicckid                           | 52698008  | Link-2  | Yes  | Banish GY       | SS to pointed: Banish Cyberse, search Backup @Ignister|
    // | Cherubini, Ebon Angel of Burning Abyss    | 58699500  | Link-2  | Yes  | Send Lv3 cost   | Mill any Level 3 (Dormouse/Backup) from Deck as cost |
    // | Link Decoder                              | 30342076  | Link-1  | Yes  | None            | Material for >=2300 ATK Cyberse Link -> SS itself!    |
    // | Linguriboh                                | 24842059  | Link-1  | Yes  | Tribute self    | Quick Negate opponent Trap and banish it              |
    // | Link Spider                               | 98978921  | Link-1  | Yes  | None            | 1 Normal Monster (Umbrage Veil) -> Cyberse material   |
    // | Salamangreat Almiraj                      | 60303245  | Link-1  | Yes  | Tribute self    | Turn <=1000 ATK normal summon into Link / protect mon |
    // =========================================================================

    [Deck("Special_Maliss", "Special_Maliss")]
    [Deck("Maliss", "Special_Maliss")]
    [Deck("2026_Maliss", "Special_Maliss")]
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
            public const int MalissInUndergroundAlt = 68337210;
            public const int MalissInTheMirror = 93453053;
            public const int MalissCMTP07 = 94722358;
            public const int MalissCTB11 = 57111661;
            public const int MalissCGWC06 = 20726052;

            // Main Deck — Cyberse / Support / PK
            public const int WizardIgnister = 3723262;
            public const int BackupIgnister = 30118811;
            public const int DoomedSoleret = 83566725;
            public const int UmbrageVeil = 85257384;
            public const int SantaClaws = 46565218;

            // Handtraps / Board Breakers
            public const int AshBlossom = 14558128;
            public const int AshBlossomAlt = 14558127;
            public const int MaxxC = 23434538;
            public const int DrollAndLockBird = 94145021;
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int DimensionShifter = 91800273;
            public const int BystialMagnamhut = 33854624;
            public const int BystialDruiswurm = 6637331;
            public const int BystialBaldrake = 72656408;

            // Spells / Traps
            public const int AllureOfDarkness = 1475311;
            public const int CalledByTheGrave = 24224830;
            public const int TripleTacticsTalent = 25311006;
            public const int Terraforming = 73628505;
            public const int GoldSarcophagus = 75500286;
            public const int PotOfSloth = 98476659;
            public const int DominusImpulse = 40366667;
            public const int DominusSpark = 6325660;
            public const int SolemnJudgment = 41420027;
            public const int SolemnAccusation = 78114463;

            // Extra Deck
            public const int HeartsCrypter = 21848500;
            public const int RedRansom = 68059897;
            public const int WhiteBinder = 95454996;
            public const int CyberseWicckid = 52698008;
            public const int SplashMage = 59859086;
            public const int LinkDecoder = 30342076;
            public const int LinkSpider = 98978921;
            public const int Linguriboh = 24842059;
            public const int SalamangreatAlmiraj = 60303245;
            public const int AlliedCodeTalkerIgnister = 39138610;
            public const int AccesscodeTalker = 86066372;
            public const int TranscodeTalker = 46947713;
            public const int Cherubini = 58699500;
            public const int KnightmareGryphon = 65330383;
            public const int HaggardLizardose = 9763474;

            // Legacy fallbacks / Side staples
            public const int GhostBelle = 73642296;
            public const int GhostOgre = 59438930;
            public const int Nibiru = 46502744;
        }

        private readonly MalissPlugin _plugin;

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
        private bool _marchHareHandSSUsed = false;
        private bool _marchHareBanishSearchUsed = false;
        private bool _wizardSSUsed = false;
        private bool _backupHandSSUsed = false;
        private bool _backupSearchUsed = false;
        private bool _soleretSSUsed = false;
        private bool _soleretSetUsed = false;
        private bool _umbrageSSUsed = false;
        private bool _cherubiniMillUsed = false;
        private bool _linkDecoderSSUsed = false;
        private bool _splashMageReviveUsed = false;
        private bool _whiteBinderBanishUsed = false;
        private bool _whiteBinderSetUsed = false;
        private bool _whiteBinderSSUsed = false;
        private bool _redRansomSearchUsed = false;
        private bool _redRansomSSUsed = false;
        private bool _heartsCrypterShuffleUsed = false;
        private bool _heartsCrypterSSUsed = false;
        private bool _undergroundUsed = false;
        private bool _mirrorNegateUsed = false;
        private bool _mirrorBanishUsed = false;
        private bool _mtp07Used = false;
        private bool _tb11Used = false;
        private bool _gwc06Used = false;
        private bool _wicckidSearchUsed = false;
        private bool _alliedCodeTalkerNegateUsed = false;
        private bool _alliedCodeTalkerReviveUsed = false;
        private bool _haggardBanishUsed = false;
        private bool _transcodeSSUsed = false;
        private bool _gryphonSetUsed = false;
        private bool _bystialMagnamhutUsed = false;
        private bool _bystialDruiswurmUsed = false;
        private bool _bystialBaldrakeUsed = false;
        private bool _potOfSlothUsed = false;
        private bool _tttUsed = false;
        private bool _mulcharmyPuruliaUsed = false;
        private bool _mulcharmyFuwalosUsed = false;

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.HeartsCrypter
                || card.Id == CardId.AlliedCodeTalkerIgnister
                || card.Id == CardId.AccesscodeTalker
                || card.Id == CardId.KnightmareGryphon
                || card.Id == CardId.WhiteBinder
                || card.Id == CardId.TranscodeTalker;
        }

        protected override bool IsBoardStrongEnough()
        {
            return _plugin.BoardAssessor.HasStrongBoard() || base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            bool hasHearts = Bot.HasInMonstersZone(CardId.HeartsCrypter);
            bool hasAllied = Bot.HasInMonstersZone(CardId.AlliedCodeTalkerIgnister);
            bool hasAccess = Bot.HasInMonstersZone(CardId.AccesscodeTalker);
            bool hasGryphon = Bot.HasInMonstersZone(CardId.KnightmareGryphon);

            if (hasHearts && hasAllied) return true;
            if (hasHearts && hasGryphon) return true;
            if (hasHearts && hasAccess) return true;

            return base.ShouldStopExtending();
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.AshBlossom || c.Id == CardId.AshBlossomAlt || c.Id == CardId.DrollAndLockBird || c.Id == CardId.DimensionShifter)
                return 850;

            return _plugin.MaterialScorer.Score(c);
        }

        public _2026_MalissExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            _plugin = new MalissPlugin(this);

            // Register Ace Cards in HeuristicGuard & ResourcePlan
            HeuristicGuard.RegisterAceCards(
                CardId.HeartsCrypter,
                CardId.AlliedCodeTalkerIgnister,
                CardId.AccesscodeTalker,
                CardId.KnightmareGryphon,
                CardId.WhiteBinder,
                CardId.TranscodeTalker
            );
            ResourcePlan.RegisterAceCards(
                CardId.HeartsCrypter,
                CardId.AlliedCodeTalkerIgnister,
                CardId.AccesscodeTalker,
                CardId.KnightmareGryphon,
                CardId.WhiteBinder,
                CardId.TranscodeTalker
            );

            // ── Combo Router: Multi-branch sequencing ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Dormouse-Full-Combo",
                RequiredCards = new List<int> { CardId.Dormouse },
                FallbackLineName = "Soleret-Starter",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Dormouse, ActionType = ExecutorType.Summon, Description = "Normal Summon Dormouse" },
                    new() { CardId = CardId.Dormouse, ActionType = ExecutorType.Activate, Description = "Dormouse banish White Rabbit from Deck" },
                    new() { CardId = CardId.WhiteRabbit, ActionType = ExecutorType.Activate, Description = "White Rabbit SS from banish" },
                    new() { CardId = CardId.WhiteRabbit, ActionType = ExecutorType.Activate, Description = "White Rabbit Set Maliss Trap" },
                    new() { CardId = CardId.LinkDecoder, ActionType = ExecutorType.SpSummon, Description = "Link-1 Link Decoder" },
                    new() { CardId = CardId.RedRansom, ActionType = ExecutorType.SpSummon, Description = "Link-3 Red Ransom" },
                    new() { CardId = CardId.LinkDecoder, ActionType = ExecutorType.Activate, Description = "Link Decoder SS from GY" },
                    new() { CardId = CardId.RedRansom, ActionType = ExecutorType.Activate, Description = "Red Ransom search Underground" },
                    new() { CardId = CardId.MalissInUnderground, ActionType = ExecutorType.Activate, Description = "Activate Underground banish Cheshire" },
                    new() { CardId = CardId.CheshireCat, ActionType = ExecutorType.Activate, Description = "Cheshire Cat SS from banish" },
                    new() { CardId = CardId.HeartsCrypter, ActionType = ExecutorType.SpSummon, Description = "Link-3 Hearts Crypter" }
                },
                EndBoardScore = 95,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.Dormouse)
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Soleret-Starter",
                RequiredCards = new List<int> { CardId.DoomedSoleret },
                FallbackLineName = "GoldSarc-Starter",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.DoomedSoleret, ActionType = ExecutorType.SpSummon, Description = "Special Summon Doomed Soleret" },
                    new() { CardId = CardId.DoomedSoleret, ActionType = ExecutorType.Activate, Description = "Soleret Set Umbrage Veil" },
                    new() { CardId = CardId.UmbrageVeil, ActionType = ExecutorType.Activate, Description = "Activate Umbrage Veil as Normal Monster" },
                    new() { CardId = CardId.LinkSpider, ActionType = ExecutorType.SpSummon, Description = "Link-1 Link Spider" }
                },
                EndBoardScore = 88,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.DoomedSoleret) && Bot.GetMonsterCount() == 0
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "GoldSarc-Starter",
                RequiredCards = new List<int> { CardId.GoldSarcophagus },
                FallbackLineName = "Underground-Starter",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.GoldSarcophagus, ActionType = ExecutorType.Activate, Description = "Gold Sarcophagus banish Dormouse" },
                    new() { CardId = CardId.Dormouse, ActionType = ExecutorType.Activate, Description = "Dormouse SS from banish" },
                    new() { CardId = CardId.RedRansom, ActionType = ExecutorType.SpSummon, Description = "Link-3 Red Ransom" }
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
                    new() { CardId = CardId.RedRansom, ActionType = ExecutorType.SpSummon, Description = "Link-3 Red Ransom" }
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
            BaitPlanner.RegisterComboStarters(CardId.Dormouse, CardId.DoomedSoleret, CardId.GoldSarcophagus, CardId.AllureOfDarkness, CardId.MalissInUnderground);
            BaitPlanner.RegisterBaitCards(CardId.Terraforming, CardId.GoldSarcophagus, CardId.AllureOfDarkness, CardId.PotOfSloth);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.Dormouse, CardId.CyberseWicckid, CardId.BackupIgnister, CardId.WizardIgnister, CardId.RedRansom, CardId.WhiteBinder, CardId.HeartsCrypter, CardId.AlliedCodeTalkerIgnister, CardId.SplashMage);

            // ============================================================
            // TIER 1: Hand Traps & Reactive Negations
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyPuruliaEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionShifter, DimensionShifterEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusSpark, DominusSparkEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnAccusation, SolemnAccusationEffect);

            // ============================================================
            // TIER 2: Quick Effects & Disruptions
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.HeartsCrypter, HeartsCrypterQuickBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlliedCodeTalkerIgnister, AlliedCodeTalkerNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.Linguriboh, LinguribohTrapNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialDruiswurm, BystialQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialBaldrake, BystialQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.MalissInTheMirror, MirrorFieldEffect);

            // ============================================================
            // TIER 3: Setup Spells & Pre-Summon Starters
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.PotOfSloth, PotOfSlothEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingEffect);
            AddExecutor(ExecutorType.Activate, CardId.GoldSarcophagus, GoldSarcophagusEffect);
            AddExecutor(ExecutorType.Activate, CardId.AllureOfDarkness, AllureOfDarknessEffect);
            AddExecutor(ExecutorType.Activate, CardId.MalissInUnderground, UndergroundEffect);
            AddExecutor(ExecutorType.Activate, CardId.MalissInUndergroundAlt, UndergroundEffect);
            AddExecutor(ExecutorType.Activate, CardId.WizardIgnister, WizardDiscardEffect);

            // ============================================================
            // TIER 4: Normal Summons
            // ============================================================
            AddExecutor(ExecutorType.Summon, CardId.Dormouse, ShouldSummonDormouse);
            AddExecutor(ExecutorType.Summon, CardId.WhiteRabbit, ShouldSummonWhiteRabbit);
            AddExecutor(ExecutorType.Summon, CardId.CheshireCat, ShouldSummonCheshireCat);
            AddExecutor(ExecutorType.Summon, CardId.DoomedSoleret, ShouldSummonSoleret);
            AddExecutor(ExecutorType.Summon, CardId.WizardIgnister, ShouldSummonWizardIgnister);
            AddExecutor(ExecutorType.Summon, CardId.BackupIgnister, ShouldSummonBackupIgnister);
            AddExecutor(ExecutorType.Summon, CardId.MarchHare, ShouldSummonMarchHare);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.Dormouse);
            AddExecutor(ExecutorType.Summon, CardId.WhiteRabbit);
            AddExecutor(ExecutorType.Summon, CardId.CheshireCat);
            AddExecutor(ExecutorType.Summon, CardId.DoomedSoleret);
            AddExecutor(ExecutorType.Summon, CardId.MarchHare);

            // ============================================================
            // TIER 5: Monster Effects, Banished Triggers & Fast Traps
            // ============================================================
            // Santa Claws (Tribute opponent boss)
            AddExecutor(ExecutorType.SpSummon, CardId.SantaClaws, SantaClawsSummon);

            // Banished Trigger Effects
            AddExecutor(ExecutorType.Activate, CardId.Dormouse, DormouseSSFromBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.WhiteRabbit, WhiteRabbitSSFromBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.CheshireCat, CheshireCatSSFromBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.MarchHare, MarchHareBanishSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.RedRansom, RedRansomSSFromBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.WhiteBinder, WhiteBinderSSFromBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeartsCrypter, HeartsCrypterSSFromBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.MalissInTheMirror, MirrorBanishEffect);

            // Field / Hand / GY Effects & Fast Traps
            AddExecutor(ExecutorType.Activate, CardId.DoomedSoleret, SoleretOnSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.UmbrageVeil, UmbrageVeilActivateEffect);
            AddExecutor(ExecutorType.Activate, CardId.Dormouse, DormouseBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.WhiteRabbit, WhiteRabbitSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.CheshireCat, CheshireCatDrawEffect);
            AddExecutor(ExecutorType.Activate, CardId.RedRansom, RedRansomSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.WhiteBinder, WhiteBinderSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.WhiteBinder, WhiteBinderSetEffect);
            AddExecutor(ExecutorType.Activate, CardId.CyberseWicckid, WicckidEffect);
            AddExecutor(ExecutorType.Activate, CardId.SplashMage, SplashMageReviveEffect);
            AddExecutor(ExecutorType.Activate, CardId.TranscodeTalker, TranscodeReviveEffect);
            AddExecutor(ExecutorType.Activate, CardId.LinkDecoder, LinkDecoderEffect);
            AddExecutor(ExecutorType.Activate, CardId.MarchHare, MarchHareHandSSEffect);
            AddExecutor(ExecutorType.Activate, CardId.BackupIgnister, BackupSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.Cherubini, CherubiniMillEffect);
            AddExecutor(ExecutorType.Activate, CardId.HaggardLizardose, HaggardBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlliedCodeTalkerIgnister, AlliedCodeTalkerReviveEffect);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodePopEffect);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareGryphon, GryphonSetEffect);
            AddExecutor(ExecutorType.Activate, CardId.MalissCGWC06, Gwc06Effect);
            AddExecutor(ExecutorType.Activate, CardId.MalissCTB11, Tb11Effect);
            AddExecutor(ExecutorType.Activate, CardId.MalissCMTP07, Mtp07Effect);

            // ============================================================
            // TIER 6: Special Summons (Hand / GY)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.DoomedSoleret, SoleretHandSS);
            AddExecutor(ExecutorType.SpSummon, CardId.MarchHare);
            AddExecutor(ExecutorType.SpSummon, CardId.BackupIgnister, BackupHandSS);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialMagnamhut);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialDruiswurm);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialBaldrake);

            // ============================================================
            // TIER 7: Extra Deck Summons (Climbing Order)
            // ============================================================
            // Link-1
            AddExecutor(ExecutorType.SpSummon, CardId.LinkDecoder, LinkDecoderSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Linguriboh, LinguribohSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LinkSpider, LinkSpiderSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SalamangreatAlmiraj, AlmirajSummon);

            // Link-2
            AddExecutor(ExecutorType.SpSummon, CardId.SplashMage, SplashMageSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CyberseWicckid, WicckidSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Cherubini, CherubiniSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HaggardLizardose, HaggardLizardoseSummon);

            // Link-3
            AddExecutor(ExecutorType.SpSummon, CardId.RedRansom, RedRansomSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.WhiteBinder, WhiteBinderSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TranscodeTalker, TranscodeSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HeartsCrypter, HeartsCrypterSummon);

            // Link-4 / Link-5
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeTalkerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareGryphon, KnightmareGryphonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AlliedCodeTalkerIgnister, AlliedCodeTalkerSummon);

            // ============================================================
            // TIER 8: Traps & Sets
            // ============================================================
            AddExecutor(ExecutorType.SpellSet, CardId.MalissCMTP07);
            AddExecutor(ExecutorType.SpellSet, CardId.MalissCTB11);
            AddExecutor(ExecutorType.SpellSet, CardId.MalissCGWC06);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnAccusation);
            AddExecutor(ExecutorType.SpellSet, CardId.UmbrageVeil);
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
            // Maliss preferred Going First to set up Hearts Crypter / Allied Code Talker / Traps
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

            _plugin.ResetTurnState();

            _dormouseBanishUsed = false;
            _dormouseSSUsed = false;
            _rabbitSetUsed = false;
            _rabbitSSUsed = false;
            _cheshireDrawUsed = false;
            _cheshireSSUsed = false;
            _marchHareHandSSUsed = false;
            _marchHareBanishSearchUsed = false;
            _wizardSSUsed = false;
            _backupHandSSUsed = false;
            _backupSearchUsed = false;
            _soleretSSUsed = false;
            _soleretSetUsed = false;
            _umbrageSSUsed = false;
            _cherubiniMillUsed = false;
            _linkDecoderSSUsed = false;
            _splashMageReviveUsed = false;
            _whiteBinderBanishUsed = false;
            _whiteBinderSetUsed = false;
            _whiteBinderSSUsed = false;
            _redRansomSearchUsed = false;
            _redRansomSSUsed = false;
            _heartsCrypterShuffleUsed = false;
            _heartsCrypterSSUsed = false;
            _undergroundUsed = false;
            _mirrorNegateUsed = false;
            _mirrorBanishUsed = false;
            _mtp07Used = false;
            _tb11Used = false;
            _gwc06Used = false;
            _wicckidSearchUsed = false;
            _alliedCodeTalkerNegateUsed = false;
            _alliedCodeTalkerReviveUsed = false;
            _haggardBanishUsed = false;
            _transcodeSSUsed = false;
            _gryphonSetUsed = false;
            _bystialMagnamhutUsed = false;
            _bystialDruiswurmUsed = false;
            _bystialBaldrakeUsed = false;
            _potOfSlothUsed = false;
            _tttUsed = false;
            _mulcharmyPuruliaUsed = false;
            _mulcharmyFuwalosUsed = false;
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

                    if (card.Id == CardId.AlliedCodeTalkerIgnister && !Bot.HasInMonstersZone(CardId.AlliedCodeTalkerIgnister))
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

        private bool MonsterRepos()
        {
            if (Card == null || Card.HasType(CardType.Link)) return false;

            // 1. High ATK Beatsticks (Bystials 2500 ATK, Santa Claws if on our side): maintain FaceUpAttack
            if (Card.Attack >= 2500)
            {
                if (Card.IsDefense() && Card.IsFaceup()) return true;
                return false;
            }

            // 2. High DEF or Utility / Handtraps (Santa Claws 1200/2500, March Hare 600/300, Ash 0/1800, Fuwalos 100/600, Purulia 100/600, Soleret 400/1400)
            if (Card.IsAttack())
            {
                if (Card.Defense > Card.Attack || Card.Attack < 1800 || CardIntelligence.IsHandtrap(Card.Id))
                {
                    return true;
                }
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

        private bool MaxxCEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player == 0) return false;
            return DefaultMaxxC();
        }

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (_dominusSparkHandLocked || _dominusImpulseHandLocked) return false;
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool MulcharmyFuwalosEffect()
        {
            if (Duel.Player == 0) return false;
            if (Bot.GetMonsterCount() > 0 || Bot.GetSpellCount() > 0) return false;
            if (_mulcharmyFuwalosUsed || _mulcharmyPuruliaUsed) return false;

            _mulcharmyFuwalosUsed = true;
            return true;
        }

        private bool MulcharmyPuruliaEffect()
        {
            if (Duel.Player == 0) return false;
            if (Bot.GetMonsterCount() > 0 || Bot.GetSpellCount() > 0) return false;
            if (_mulcharmyPuruliaUsed || _mulcharmyFuwalosUsed) return false;

            _mulcharmyPuruliaUsed = true;
            return true;
        }

        private bool DimensionShifterEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            // CRITICAL: NEVER activate Dimension Shifter on our own Turn 1 going first! It blocks GY costs for Wicckid/Splash/Wizard!
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
                 LastChainCard.Id == CardId.MaxxC ||
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

            // STRICT: NEVER activate Dominus Spark from Hand in Maliss deck! Hand activation permanently locks EARTH/WATER/FIRE/WIND (Splash Mage)!
            if (Card.Location == CardLocation.Hand) return false;

            return true;
        }

        private bool DominusImpulseEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            if (Card.Location == CardLocation.Hand)
            {
                if (Duel.Player == 0) return false;
                if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c))) return false;
            }

            return true;
        }

        private bool SolemnJudgmentEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Bot.LifePoints <= 1000) return false;
            return DefaultSolemnJudgment();
        }

        private bool SolemnAccusationEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (Bot.LifePoints <= 1500) return false;

            return LastChainCard != null && (LastChainCard.HasType(CardType.Spell) || LastChainCard.HasType(CardType.Trap));
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

        private bool AlliedCodeTalkerNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_alliedCodeTalkerNegateUsed) return false;

            if (Duel.LastChainPlayer == 1 && LastChainCard != null)
            {
                bool hasLinkTribute = Bot.GetMonsters().Any(c => c != null && c != Card && c.IsFaceup() && c.HasType(CardType.Link) && c.Id != CardId.HeartsCrypter);
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
                if (_mirrorNegateUsed) return false;

                bool hasEnemyFaceup = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && IsViableEffectTarget(c) && !c.IsDisabled());
                bool hasBanishSource = Bot.Hand.Any(c => c != null && c.HasSetcode(0x1b9) && c != Card)
                    || Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1b9));

                if (hasEnemyFaceup && hasBanishSource)
                {
                    _mirrorNegateUsed = true;
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        // TIER 3: Setup Spells & Pre-Summon Effects
        // ============================================================

        private bool PotOfSlothEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_potOfSlothUsed) return false;

            int oppCards = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            if (oppCards >= 1)
            {
                _potOfSlothUsed = true;
                return true;
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_tttUsed) return false;

            _tttUsed = true;
            return true;
        }

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
            bool hasField = GetRemainingCount(CardId.MalissInUnderground) > 0 || GetRemainingCount(CardId.MalissInUndergroundAlt) > 0;
            bool alreadyHasFieldActive = Bot.HasInSpellZone(CardId.MalissInUnderground) || Bot.HasInSpellZone(CardId.MalissInUndergroundAlt);
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
                if (Bot.HasInSpellZone(CardId.MalissInUnderground) || Bot.HasInSpellZone(CardId.MalissInUndergroundAlt)) return false;

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
            if (_wizardSSUsed) return false;

            bool hasTargetInGY = Bot.Graveyard.Any(c => c != null && c.HasRace(CardRace.Cyberse) && c.HasAttribute(CardAttribute.Dark));
            bool hasEDCyberse = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Cyberse) && c.IsExtraCard());

            if (hasTargetInGY && hasEDCyberse && GetFreeMonsterZoneCount() >= 2)
            {
                _wizardSSUsed = true;
                return true;
            }
            return false;
        }

        // ============================================================
        // TIER 4: Normal Summon Conditions
        // ============================================================

        private bool ShouldSummonDormouse()
        {
            return HasRemainingCardWithSetcode(0x1b9);
        }

        private bool ShouldSummonWhiteRabbit()
        {
            bool hasTrap = (GetRemainingCount(CardId.MalissCMTP07) > 0 && !Bot.Graveyard.Any(c => c != null && c.Id == CardId.MalissCMTP07))
                || (GetRemainingCount(CardId.MalissCTB11) > 0 && !Bot.Graveyard.Any(c => c != null && c.Id == CardId.MalissCTB11))
                || (GetRemainingCount(CardId.MalissCGWC06) > 0 && !Bot.Graveyard.Any(c => c != null && c.Id == CardId.MalissCGWC06));
            return hasTrap;
        }

        private bool ShouldSummonCheshireCat()
        {
            return Bot.Hand.Count >= 2;
        }

        private bool ShouldSummonSoleret()
        {
            bool hasMaliss = Bot.Hand.Any(c => c != null && c.HasSetcode(0x1b9) && c.IsMonster());
            if (hasMaliss) return false;
            return true;
        }

        private bool ShouldSummonWizardIgnister()
        {
            bool hasMalissInHand = Bot.Hand.Any(c => c != null && c.HasSetcode(0x1b9) && c.IsMonster());
            if (hasMalissInHand) return false;
            return true;
        }

        private bool ShouldSummonBackupIgnister()
        {
            return GetRemainingCount(CardId.Dormouse) > 0 || GetRemainingCount(CardId.WhiteRabbit) > 0;
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

        private bool SantaClawsSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            // Target problematic enemy monster: ATK >= 2500, or active negator / floodgate
            var problem = Util.GetProblematicEnemyMonster();
            if (problem != null) return true;

            var bigThreat = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && (m.Attack >= 2500 || m.IsExtraCard()));
            return bigThreat != null;
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

        private bool WhiteRabbitSSFromBanishEffect()
        {
            if (Card.Location != CardLocation.Removed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_rabbitSSUsed) return false;
            if (Bot.LifePoints <= 1000) return false;

            _rabbitSSUsed = true;
            return true;
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

        private bool MarchHareBanishSearchEffect()
        {
            if (Card.Location != CardLocation.Removed) return false;
            if (_marchHareBanishSearchUsed) return false;
            if (Bot.LifePoints <= 1000) return false;

            bool hasBanishedTarget = Bot.Banished.Any(c => c != null && c.HasSetcode(0x1b9) && c.IsMonster() && c != Card);
            if (hasBanishedTarget)
            {
                _marchHareBanishSearchUsed = true;
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

        private bool WhiteBinderSSFromBanishEffect()
        {
            if (Card.Location != CardLocation.Removed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_whiteBinderSSUsed) return false;
            if (Bot.LifePoints <= 1000) return false;

            _whiteBinderSSUsed = true;
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

        private bool SoleretOnSummonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_soleretSetUsed) return false;

            bool hasTrapInDeck = GetRemainingCount(CardId.UmbrageVeil) > 0;
            if (hasTrapInDeck)
            {
                _soleretSetUsed = true;
                return true;
            }
            return false;
        }

        private bool UmbrageVeilActivateEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (_umbrageSSUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasPK = Bot.GetMonsters().Any(c => c != null && c.Id == CardId.DoomedSoleret)
                || Bot.Graveyard.Any(c => c != null && c.Id == CardId.DoomedSoleret);

            if (hasPK && GetFreeMonsterZoneCount() > 0)
            {
                _umbrageSSUsed = true;
                return true;
            }
            return false;
        }

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

        private bool WhiteRabbitSummonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_rabbitSetUsed) return false;

            bool hasMTP = GetRemainingCount(CardId.MalissCMTP07) > 0 && !Bot.Graveyard.Any(c => c != null && c.Id == CardId.MalissCMTP07);
            bool hasTB = GetRemainingCount(CardId.MalissCTB11) > 0 && !Bot.Graveyard.Any(c => c != null && c.Id == CardId.MalissCTB11);
            bool hasGWC = GetRemainingCount(CardId.MalissCGWC06) > 0 && !Bot.Graveyard.Any(c => c != null && c.Id == CardId.MalissCGWC06);

            if (hasMTP || hasTB || hasGWC)
            {
                _rabbitSetUsed = true;
                return true;
            }
            return false;
        }

        private bool CheshireCatDrawEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_cheshireDrawUsed) return false;

            bool hasBanishSource = Bot.Hand.Any(c => c != null && c != Card && c.HasSetcode(0x1b9));
            if (hasBanishSource)
            {
                _cheshireDrawUsed = true;
                return true;
            }
            return false;
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

        private bool WhiteBinderSummonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_whiteBinderBanishUsed) return false;

            bool oppHasGY = Enemy.Graveyard.Count > 0;
            bool ourHasGY = Bot.Graveyard.Any(c => c != null && c.HasSetcode(0x1b9));

            if (oppHasGY || ourHasGY)
            {
                _whiteBinderBanishUsed = true;
                return true;
            }
            return false;
        }

        private bool WhiteBinderSetEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_whiteBinderSetUsed) return false;

            bool hasTrapInDeckOrGY = GetRemainingCount(CardId.MalissCGWC06) > 0
                || GetRemainingCount(CardId.MalissCTB11) > 0
                || GetRemainingCount(CardId.MalissCMTP07) > 0
                || Bot.Graveyard.Any(c => c != null && c.HasSetcode(0x1b9) && c.HasType(CardType.Trap));

            if (hasTrapInDeckOrGY)
            {
                _whiteBinderSetUsed = true;
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

        private bool SplashMageReviveEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_splashMageReviveUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasCyberseInGY = Bot.Graveyard.Any(c => c != null && c.HasRace(CardRace.Cyberse) && c.IsCanRevive());
            if (hasCyberseInGY && GetFreeMonsterZoneCount() > 0)
            {
                _splashMageReviveUsed = true;
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

        private bool LinkDecoderEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_linkDecoderSSUsed) return false;

            _linkDecoderSSUsed = true;
            return true;
        }

        private bool MarchHareHandSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_marchHareHandSSUsed) return false;
            if (GetFreeMonsterZoneCount() == 0) return false;

            bool hasMaliss = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1b9));
            if (hasMaliss)
            {
                _marchHareHandSSUsed = true;
                return true;
            }
            return false;
        }

        private bool BackupSearchEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_backupSearchUsed) return false;

            bool hasDarkCyberse = HasRemainingMonsterWithSetcode(0x1b9) || GetRemainingCount(CardId.WizardIgnister) > 0;
            if (hasDarkCyberse)
            {
                _backupSearchUsed = true;
                return true;
            }
            return false;
        }

        private bool CherubiniMillEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_cherubiniMillUsed) return false;

            bool hasLv3InDeck = GetRemainingCount(CardId.Dormouse) > 0
                || GetRemainingCount(CardId.BackupIgnister) > 0
                || GetRemainingCount(CardId.WhiteRabbit) > 0
                || GetRemainingCount(CardId.MarchHare) > 0
                || GetRemainingCount(CardId.CheshireCat) > 0;

            if (hasLv3InDeck)
            {
                _cherubiniMillUsed = true;
                return true;
            }
            return false;
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

        private bool AlliedCodeTalkerReviveEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_alliedCodeTalkerReviveUsed) return false;

            bool has2300InGY = Bot.Graveyard.Any(c => c != null && c.HasRace(CardRace.Cyberse) && c.Attack == 2300 && c.IsCanRevive());
            if (has2300InGY && GetFreeMonsterZoneCount() > 0)
            {
                _alliedCodeTalkerReviveUsed = true;
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

        private bool GryphonSetEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_gryphonSetUsed) return false;
            if (Bot.Hand.Count == 0) return false;

            bool hasSpellTrapInGY = Bot.Graveyard.Any(c => c != null && (c.HasType(CardType.Spell) || c.HasType(CardType.Trap)));
            if (hasSpellTrapInGY)
            {
                _gryphonSetUsed = true;
                return true;
            }
            return false;
        }

        private bool Gwc06Effect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (_gwc06Used) return false;
                if (IsSpecialSummonBlocked()) return false;

                bool hasMalissOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1b9));
                bool hasTarget = Bot.Banished.Concat(Bot.Graveyard).Any(c => c != null && c.HasSetcode(0x1b9) && c.IsMonster() && c.IsCanRevive());

                if (hasMalissOnField && hasTarget && GetFreeMonsterZoneCount() > 0)
                {
                    _gwc06Used = true;
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

        // ============================================================
        // TIER 6: Special Summons (Hand)
        // ============================================================

        private bool SoleretHandSS()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_soleretSSUsed) return false;
            if (Bot.GetMonsterCount() == 0)
            {
                _soleretSSUsed = true;
                return true;
            }
            return false;
        }

        private bool BackupHandSS()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_backupHandSSUsed) return false;

            bool hasEDCyberse = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Cyberse) && c.IsExtraCard());
            if (hasEDCyberse && GetFreeMonsterZoneCount() > 0)
            {
                _backupHandSSUsed = true;
                return true;
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
            if (c.Id == CardId.LinkDecoder) return true;
            if (c.Id == CardId.DoomedSoleret && _soleretSetUsed) return true;
            if (c.Id == CardId.UmbrageVeil) return true;
            if (c.Id == CardId.Dormouse && _dormouseBanishUsed) return true;
            if (c.Id == CardId.WhiteRabbit && _rabbitSetUsed) return true;
            if (c.Id == CardId.CheshireCat && _cheshireDrawUsed) return true;
            if (c.Id == CardId.RedRansom && _redRansomSearchUsed) return true;
            if (c.Id == CardId.WhiteBinder && _whiteBinderSetUsed) return true;
            if (c.Id == CardId.CyberseWicckid && _wicckidSearchUsed) return true;
            if (c.Id == CardId.SplashMage && _splashMageReviveUsed) return true;
            return false;
        }

        private bool LinkDecoderSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.LinkDecoder)) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level <= 4 && !c.IsExtraCard() && c.HasRace(CardRace.Cyberse) && !IsAceCard(c)).ToList();
            if (materials.Count == 0) return false;
            if (materials.Count == 1 && !IsMonsterExpendable(materials[0])) return false;
            return true;
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

        private bool LinkSpiderSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.IsMonster() && c.HasType(CardType.Normal) && !IsAceCard(c)).ToList();
            return materials.Count > 0;
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

        private bool WicckidSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.CyberseWicckid)) return false;
            if (GetRemainingCount(CardId.BackupIgnister) == 0) return false;

            // Only make Wicckid if we can trigger its search via an extender in hand or banished trigger
            bool canTriggerWicckid = Bot.Hand.Any(c => c != null && (c.Id == CardId.BackupIgnister || c.Id == CardId.DoomedSoleret || c.Id == CardId.MarchHare))
                || Bot.Banished.Any(c => c != null && c.HasSetcode(0x1b9) && c.IsMonster());
            if (!canTriggerWicckid) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Cyberse) && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool SplashMageSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_dominusSparkHandLocked) return false;
            if (Bot.HasInMonstersZone(CardId.SplashMage)) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Cyberse) && !IsAceCard(c)).ToList();
            return materials.Count >= 2;
        }

        private bool CherubiniSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.Cherubini)) return false;

            var lv3s = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 3 && !IsAceCard(c)).ToList();
            return lv3s.Count >= 2;
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

        private bool RedRansomSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.RedRansom)) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Cyberse) && !IsAceCard(c)).ToList();
            bool hasMaliss = materials.Any(c => c.HasSetcode(0x1b9));
            return materials.Count >= 2 && hasMaliss;
        }

        private bool WhiteBinderSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.WhiteBinder)) return false;

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

        private bool KnightmareGryphonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.KnightmareGryphon)) return false;
            if (!_plugin.LinkAdvisor.ShouldSummonKnightmareGryphon()) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            return materials.Count >= 4 || (materials.Count >= 3 && materials.Any(c => c.HasType(CardType.Link) && c.LinkCount >= 2));
        }

        private bool AlliedCodeTalkerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.HasInMonstersZone(CardId.AlliedCodeTalkerIgnister)) return false;

            var materials = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Cyberse) && c.Id != CardId.AccesscodeTalker).ToList();
            return materials.Count >= 3 || (materials.Count >= 2 && materials.Any(c => c.HasType(CardType.Link) && c.LinkCount >= 3));
        }

        // ============================================================
        // Selection & Option & Placement Callbacks
        // ============================================================

        public override int OnSelectOption(IList<long> options)
        {
            // Triple Tactics Talent:
            // 0: Draw 2 cards
            // 1: Take control of opponent's monster
            // 2: Look at opponent hand and shuffle 1
            if (LastChainCard != null && LastChainCard.Id == CardId.TripleTacticsTalent)
            {
                var problem = Util.GetProblematicEnemyMonster();
                if (problem != null && options.Contains(1)) return 1;
                if (options.Contains(0)) return 0;
            }

            // Solemn Accusation:
            // 0: Pay 1500 LP -> Negate and destroy, neither can activate same name
            // 1: Pay 3000 LP -> Negate and banish, opponent banishes all from hand and deck
            if (LastChainCard != null && LastChainCard.Id == CardId.SolemnAccusation)
            {
                if (Bot.LifePoints >= 4000 && options.Contains(1)) return 1;
                if (options.Contains(0)) return 0;
            }

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

            // 1. If selecting opponent cards (banish / destroy / bounce / negate / tribute target)
            var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
            if (oppCards.Count > 0 && oppCards.Count == cards.Count)
            {
                var problem = oppCards.FirstOrDefault(c => c == Util.GetProblematicEnemyMonster() || c == Util.GetProblematicEnemySpell());
                if (problem != null) return Util.CheckSelectCount(new[] { problem }, cards, min, max);
                var sortedOpp = oppCards.OrderByDescending(c => c.Attack).ToList();
                return Util.CheckSelectCount(sortedOpp, cards, min, max);
            }

            // 2. If selecting Maliss Traps to Set (White Rabbit / White Binder)
            var malissTrapsInDeck = cards.Where(c => c != null && (c.Location == CardLocation.Deck || c.Location == CardLocation.Grave) && c.HasSetcode(0x1b9) && c.HasType(CardType.Trap)).ToList();
            if (malissTrapsInDeck.Count > 0 && malissTrapsInDeck.Count == cards.Count)
            {
                if (Bot.LifePoints < 3500)
                {
                    var gwc = malissTrapsInDeck.FirstOrDefault(c => c.Id == CardId.MalissCGWC06 && !Bot.Graveyard.Any(g => g != null && g.Id == CardId.MalissCGWC06));
                    if (gwc != null) return Util.CheckSelectCount(new[] { gwc }, cards, min, max);
                }

                var tb = malissTrapsInDeck.FirstOrDefault(c => c.Id == CardId.MalissCTB11 && !Bot.Graveyard.Any(g => g != null && g.Id == CardId.MalissCTB11));
                if (tb != null) return Util.CheckSelectCount(new[] { tb }, cards, min, max);

                var mtp = malissTrapsInDeck.FirstOrDefault(c => c.Id == CardId.MalissCMTP07);
                if (mtp != null) return Util.CheckSelectCount(new[] { mtp }, cards, min, max);

                var gwcFallback = malissTrapsInDeck.FirstOrDefault(c => c.Id == CardId.MalissCGWC06);
                if (gwcFallback != null) return Util.CheckSelectCount(new[] { gwcFallback }, cards, min, max);

                return Util.CheckSelectCount(malissTrapsInDeck, cards, min, max);
            }

            // 3. If selecting from Deck (Banish / Search / Summon / Mill)
            var deckCards = cards.Where(c => c != null && c.Location == CardLocation.Deck).ToList();
            if (deckCards.Count > 0)
            {
                // Cherubini mill cost (send Lv3 to GY)
                if (LastChainCard != null && LastChainCard.Id == CardId.Cherubini)
                {
                    var lv3 = deckCards.FirstOrDefault(c => c.Id == CardId.Dormouse && !_dormouseSSUsed)
                        ?? deckCards.FirstOrDefault(c => c.Id == CardId.BackupIgnister)
                        ?? deckCards.FirstOrDefault(c => c.Id == CardId.WhiteRabbit && !_rabbitSSUsed)
                        ?? deckCards.FirstOrDefault(c => c.Id == CardId.CheshireCat)
                        ?? deckCards.FirstOrDefault(c => c.Level == 3);
                    if (lv3 != null) return Util.CheckSelectCount(new[] { lv3 }, cards, min, max);
                }

                // Banish from Deck (Underground / Dormouse / Gold Sarc / Red Ransom)
                if (hint == 503 || hint == 502 || hint == 504 || hint == 500 || (LastChainCard != null && (LastChainCard.Id == CardId.MalissInUnderground || LastChainCard.Id == CardId.MalissInUndergroundAlt || LastChainCard.Id == CardId.Dormouse || LastChainCard.Id == CardId.GoldSarcophagus || LastChainCard.Id == CardId.RedRansom)))
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
                if (deckCards.Any(c => c.Id == CardId.MalissInUnderground || c.Id == CardId.MalissInUndergroundAlt || c.Id == CardId.MalissInTheMirror))
                {
                    bool hasField = Bot.HasInSpellZone(CardId.MalissInUnderground) || Bot.HasInSpellZone(CardId.MalissInUndergroundAlt) || Bot.Hand.Any(c => c != null && (c.Id == CardId.MalissInUnderground || c.Id == CardId.MalissInUndergroundAlt));
                    if (!hasField)
                    {
                        var field = deckCards.FirstOrDefault(c => c.Id == CardId.MalissInUnderground || c.Id == CardId.MalissInUndergroundAlt);
                        if (field != null) return Util.CheckSelectCount(new[] { field }, cards, min, max);
                    }
                    var mirror = deckCards.FirstOrDefault(c => c.Id == CardId.MalissInTheMirror);
                    if (mirror != null) return Util.CheckSelectCount(new[] { mirror }, cards, min, max);
                }

                // Search Backup @Ignister (Wicckid)
                if (deckCards.Any(c => c.Id == CardId.BackupIgnister))
                {
                    var backup = deckCards.FirstOrDefault(c => c.Id == CardId.BackupIgnister);
                    if (backup != null) return Util.CheckSelectCount(new[] { backup }, cards, min, max);
                }

                // Set Umbrage Veil from Deck (Soleret)
                if (deckCards.Any(c => c.Id == CardId.UmbrageVeil))
                {
                    var veil = deckCards.FirstOrDefault(c => c.Id == CardId.UmbrageVeil);
                    if (veil != null) return Util.CheckSelectCount(new[] { veil }, cards, min, max);
                }

                // Search / Summon Maliss Monsters (MTP-07 / TB-11 / Backup @Ignister)
                var deckMalissSearch = deckCards.Where(c => c.HasSetcode(0x1b9) && c.IsMonster()).ToList();
                if (deckMalissSearch.Count > 0)
                {
                    var bestSearch = deckMalissSearch.FirstOrDefault(c => c.Id == CardId.Dormouse && !_dormouseBanishUsed)
                        ?? deckMalissSearch.FirstOrDefault(c => c.Id == CardId.MarchHare && !_marchHareHandSSUsed)
                        ?? deckMalissSearch.FirstOrDefault(c => c.Id == CardId.WhiteRabbit && !_rabbitSetUsed)
                        ?? deckMalissSearch.FirstOrDefault(c => c.Id == CardId.CheshireCat && !_cheshireDrawUsed)
                        ?? deckMalissSearch[0];
                    return Util.CheckSelectCount(new[] { bestSearch }, cards, min, max);
                }
            }

            // 4. If selecting face-up Maliss monsters on Field as Cost to Banish (GWC-06 / TB-11 / MTP-07 / Mirror)
            var fieldMaliss = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && c.HasSetcode(0x1b9)).ToList();
            if (fieldMaliss.Count > 0 && (hint == 502 || hint == 503 || hint == 504 || (LastChainCard != null && (LastChainCard.Id == CardId.MalissCGWC06 || LastChainCard.Id == CardId.MalissCMTP07 || LastChainCard.Id == CardId.MalissCTB11 || LastChainCard.Id == CardId.MalissInTheMirror))))
            {
                // Pick one whose banished SS effect has NOT been used this turn yet!
                var bestToBanish = fieldMaliss.FirstOrDefault(c => c.Id == CardId.RedRansom && !_redRansomSSUsed)
                    ?? fieldMaliss.FirstOrDefault(c => c.Id == CardId.WhiteBinder && !_whiteBinderSSUsed)
                    ?? fieldMaliss.FirstOrDefault(c => c.Id == CardId.WhiteRabbit && !_rabbitSSUsed)
                    ?? fieldMaliss.FirstOrDefault(c => c.Id == CardId.CheshireCat && !_cheshireSSUsed)
                    ?? fieldMaliss.FirstOrDefault(c => c.Id == CardId.Dormouse && !_dormouseSSUsed)
                    ?? fieldMaliss.OrderBy(c => c.Attack).First();
                return Util.CheckSelectCount(new[] { bestToBanish }, cards, min, max);
            }

            // 5. If selecting from Hand to Banish / Discard (Allure / Cheshire / March Hare / Underground / Gryphon)
            var handCards = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
            if (handCards.Count > 0 && (hint == 501 || hint == 502 || hint == 503 || hint == 504 || (LastChainCard != null && (LastChainCard.Id == CardId.AllureOfDarkness || LastChainCard.Id == CardId.CheshireCat || LastChainCard.Id == CardId.MalissInUnderground || LastChainCard.Id == CardId.MalissInUndergroundAlt || LastChainCard.Id == CardId.KnightmareGryphon))))
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

            // 6. If selecting from Graveyard (Wicckid cost / Haggard / Splash / Transcode / Wizard / Allied / Gryphon)
            var gyCards = cards.Where(c => c != null && c.Location == CardLocation.Grave).ToList();
            if (gyCards.Count > 0)
            {
                // Wizard @Ignister target: DARK Cyberse
                if (LastChainCard != null && LastChainCard.Id == CardId.WizardIgnister)
                {
                    var target = gyCards.FirstOrDefault(c => c.Id == CardId.HeartsCrypter)
                        ?? gyCards.FirstOrDefault(c => c.Id == CardId.RedRansom)
                        ?? gyCards.FirstOrDefault(c => c.Id == CardId.WhiteBinder)
                        ?? gyCards.FirstOrDefault(c => c.HasRace(CardRace.Cyberse) && c.HasAttribute(CardAttribute.Dark));
                    if (target != null) return Util.CheckSelectCount(new[] { target }, cards, min, max);
                }

                // Gryphon Set Spell/Trap target
                if (LastChainCard != null && LastChainCard.Id == CardId.KnightmareGryphon)
                {
                    var bestTrap = gyCards.FirstOrDefault(c => c.Id == CardId.SolemnJudgment)
                        ?? gyCards.FirstOrDefault(c => c.Id == CardId.SolemnAccusation)
                        ?? gyCards.FirstOrDefault(c => c.Id == CardId.CalledByTheGrave)
                        ?? gyCards.FirstOrDefault(c => c.HasSetcode(0x1b9) && c.HasType(CardType.Trap))
                        ?? gyCards.FirstOrDefault(c => c.HasType(CardType.Spell) || c.HasType(CardType.Trap));
                    if (bestTrap != null) return Util.CheckSelectCount(new[] { bestTrap }, cards, min, max);
                }

                // Allied Code Talker revive targets (2300 ATK Cyberse)
                if (LastChainCard != null && LastChainCard.Id == CardId.AlliedCodeTalkerIgnister)
                {
                    var list2300 = gyCards.Where(c => c.HasRace(CardRace.Cyberse) && c.Attack == 2300).ToList();
                    if (list2300.Count > 0) return Util.CheckSelectCount(list2300, cards, min, max);
                }

                // Splash Mage revive target
                if (LastChainCard != null && LastChainCard.Id == CardId.SplashMage)
                {
                    var revive = gyCards.FirstOrDefault(c => c.Id == CardId.LinkDecoder)
                        ?? gyCards.FirstOrDefault(c => c.Id == CardId.HeartsCrypter)
                        ?? gyCards.FirstOrDefault(c => c.Id == CardId.RedRansom)
                        ?? gyCards.FirstOrDefault(c => c.Id == CardId.WhiteBinder)
                        ?? gyCards.FirstOrDefault(c => c.HasRace(CardRace.Cyberse));
                    if (revive != null) return Util.CheckSelectCount(new[] { revive }, cards, min, max);
                }

                // Transcode Talker revive target (Link-3 or lower)
                if (LastChainCard != null && LastChainCard.Id == CardId.TranscodeTalker)
                {
                    var linkRevive = gyCards.Where(c => c.HasType(CardType.Link) && c.LinkCount <= 3).ToList();
                    if (linkRevive.Count > 0)
                    {
                        var best = linkRevive.FirstOrDefault(c => c.Id == CardId.HeartsCrypter)
                            ?? linkRevive.FirstOrDefault(c => c.Id == CardId.WhiteBinder)
                            ?? linkRevive.FirstOrDefault(c => c.Id == CardId.RedRansom)
                            ?? linkRevive.OrderByDescending(c => c.LinkCount).First();
                        return Util.CheckSelectCount(new[] { best }, cards, min, max);
                    }
                }

                // Banishing from GY (Wicckid / Haggard / Mirror / White Binder)
                if (hint == 503 || hint == 502 || hint == 504 || (LastChainCard != null && (LastChainCard.Id == CardId.CyberseWicckid || LastChainCard.Id == CardId.HaggardLizardose || LastChainCard.Id == CardId.WhiteBinder)))
                {
                    // If White Binder: target opponent's GY cards first!
                    var oppGY = gyCards.Where(c => c.Controller == 1).ToList();
                    if (oppGY.Count > 0)
                    {
                        var sortedOppGY = oppGY.OrderByDescending(c => c.Attack).ToList();
                        return Util.CheckSelectCount(sortedOppGY, cards, min, max);
                    }

                    var gyCyberse = gyCards.Where(c => c.HasRace(CardRace.Cyberse)).ToList();
                    if (gyCyberse.Count > 0)
                    {
                        var target = gyCyberse.FirstOrDefault(c => c.Id == CardId.RedRansom && !_redRansomSSUsed)
                            ?? gyCyberse.FirstOrDefault(c => c.Id == CardId.WhiteBinder && !_whiteBinderSSUsed)
                            ?? gyCyberse.FirstOrDefault(c => c.Id == CardId.Linguriboh || c.Id == CardId.LinkSpider)
                            ?? gyCyberse.FirstOrDefault(c => c.Id == CardId.Dormouse && !_dormouseSSUsed)
                            ?? gyCyberse.FirstOrDefault(c => c.Id == CardId.WhiteRabbit && !_rabbitSSUsed)
                            ?? gyCyberse.FirstOrDefault(c => c.Id == CardId.CheshireCat && !_cheshireSSUsed)
                            ?? gyCyberse[0];
                        return Util.CheckSelectCount(new[] { target }, cards, min, max);
                    }
                }
            }

            // 7. If selecting from Banished Zone (Hearts Crypter shuffle / March Hare add / GWC-06 revive)
            var banishCards = cards.Where(c => c != null && c.Location == CardLocation.Removed).ToList();
            if (banishCards.Count > 0)
            {
                // Hearts Crypter shuffle target into Deck (hint 512 / 507)
                if (hint == 512 || hint == 507 || (LastChainCard != null && LastChainCard.Id == CardId.HeartsCrypter))
                {
                    var banishTraps = banishCards.Where(c => c.HasSetcode(0x1b9) && c.HasType(CardType.Trap)).ToList();
                    if (banishTraps.Count > 0) return Util.CheckSelectCount(new[] { banishTraps[0] }, cards, min, max);
                    var banishMaliss = banishCards.Where(c => c.HasSetcode(0x1b9)).ToList();
                    if (banishMaliss.Count > 0) return Util.CheckSelectCount(new[] { banishMaliss[0] }, cards, min, max);
                }

                // GWC-06 revive target (Special Summon 1 banished or GY Maliss)
                if (LastChainCard != null && LastChainCard.Id == CardId.MalissCGWC06)
                {
                    var bestRevive = banishCards.FirstOrDefault(c => c.Id == CardId.HeartsCrypter)
                        ?? banishCards.FirstOrDefault(c => c.Id == CardId.WhiteBinder)
                        ?? banishCards.FirstOrDefault(c => c.Id == CardId.RedRansom)
                        ?? banishCards.FirstOrDefault(c => c.Id == CardId.Dormouse)
                        ?? banishCards[0];
                    return Util.CheckSelectCount(new[] { bestRevive }, cards, min, max);
                }

                // March Hare add to hand
                var banishMonsters = banishCards.Where(c => c.HasSetcode(0x1b9) && c.IsMonster()).ToList();
                if (banishMonsters.Count > 0)
                {
                    var target = banishMonsters.FirstOrDefault(c => c.Id == CardId.HeartsCrypter)
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
            // Attack position for beaters: Bystials (2500 ATK) & Maliss attackers
            if (cardId == CardId.BystialMagnamhut || cardId == CardId.BystialDruiswurm || cardId == CardId.BystialBaldrake ||
                cardId == CardId.Dormouse || cardId == CardId.WhiteRabbit || cardId == CardId.CheshireCat)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            // Defense position for high DEF / Handtraps / Utility (Santa Claws, March Hare 600/300, Soleret 400/1400, Umbrage Veil 0/300, Ash 0/1800, Fuwalos 100/600, Purulia 100/600, Droll 0/0)
            if (cardId == CardId.SantaClaws || cardId == CardId.MarchHare || cardId == CardId.DoomedSoleret || cardId == CardId.UmbrageVeil ||
                CardIntelligence.IsHandtrap(cardId) || cardId == CardId.WizardIgnister || cardId == CardId.BackupIgnister)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }
}
