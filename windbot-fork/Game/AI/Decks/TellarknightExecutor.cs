// ============================================================
// CARD AUDIT เนโฌโ€ 2026_Tellarknight
// | Card Name           | Type    | OPT? | Cost      | Effect              | Activate When        | NEVER When           |
// |---------------------|---------|------|-----------|---------------------|----------------------|----------------------|
// | Tellarknight Lyran  | Monster | HOPT | None      | Search S/T, SS self | Extend Xyz plays     | No S/T in hand       |
// | Tellarknight Altairan| Monster | HOPT | None      | Pop card, recycle   | Disruption, recovery  | No target on field   |
// ACE CARDS: Primary: Tellarknight Constellar Caduceus / Secondary: Number F0 Utopic Draco Future
// COMBO STARTERS: 1. Constellar Castor 2. Tellarknight Lyran
// CHOKEPOINTS: Lyran normal summon negated
// WIN CONDITION: Xyz summon rank 4 monsters to lock out opponent and build card advantage
// GOING 1ST END BOARD: Tellarknight Constellar Caduceus + Draco Future
// GOING 2ND GAMEPLAN: Xyz Summon Delteros to destroy opponent cards, then attack for lethal
// ============================================================

// ============================================================
// COMBO DRAFT เนโฌโ€ 2026_Tellarknight
// ============================================================
// === COMBO LINE 1: Rank 4 Spam (Starter: Castor) ===
// HAND REQUIRED: Constellar Castor + Tellarknight Lyran
// STEP 1: Normal Summon Constellar Castor (grants additional summon)
// STEP 2: Normal Summon Tellarknight Lyran
// STEP 3: Lyran Effect: Add Constellar Tellarknights spell/trap
// STEP 4: Overlay Castor + Lyran เนยโ€ Xyz Summon Tellarknight Constellar Caduceus
// END BOARD: Caduceus + Draco Future
// === COMBO LINE 2: Going 2nd Removal ===
// STEP 1: Xyz Summon Stellarknight Delteros
// STEP 2: Delteros Effect: Destroy 1 card on the field
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
    [Deck("2026_Tellarknight", "2026_Tellarknight")]
    public class _2026_TellarknightExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Cards
            public const int ConstellarCastor = 33302589;
            public const int TellarknightCygnian = 60700283;
            public const int TellarknightLyran = 79210531;
            public const int TellarknightAltairan = 42822433;
            public const int SatellarknightAltair = 2273734;
            public const int SatellarknightUnukalhai = 1050186;
            public const int ConstellarCaduceusMain = 82913020;
            public const int DodododoWarrior = 62880279;
            public const int Onomatokage = 55088578;
            public const int Sakitama = 67972302;
            public const int MulcharmyFuwalos = 42141493;
            public const int AshBlossom = 14558128;
            public const int GhostOgre = 59438930;
            public const int ConstellarTellarknights = 10125011;
            public const int SatellarknightSkybridge = 18205590;
            public const int StellarnovaBonds = 69678646;
            public const int SeventhTachyon = 7477101;
            public const int PairBearScare = 21501961;
            public const int ForbiddenCrown = 98829635;
            public const int CalledByTheGrave = 24224830;

            // Extra Deck Cards
            public const int TellarknightConstellarCaduceus = 58858807;
            public const int TellarknightConstellarDelteros = 32289031;
            public const int StellarknightDelteros = 56638325;
            public const int TellarknightPtolemaeus = 18326736;
            public const int StellarknightConstellarDiamond = 9272381;
            public const int ExstellarknightConstellarPtolemyOmega7 = 6195332;
            public const int ConstellarPtolemyM7 = 38495396;
            public const int ConstellarPleiades = 73964868;
            public const int NumberF0UtopicDracoFuture = 26973555;
            public const int NumberF0UtopicFuture = 65305468;
            public const int GagagagaMagician = 86331741;
            public const int Number104Masquerade = 2061963;
            public const int EvilswarmExcitonKnight = 46772449;
            public const int ChronomalyVimana = 2609443;
        }

        // OPT State Tracking
        private bool _seventhTachyonUsed = false;
        private bool _pairBearScareUsed = false;
        private bool _stellarnovaBondsUsed = false;
        private bool _constellarTellarknightsUsed = false;
        private bool _tellarknightConstellarCaduceusCopyUsed = false;
        private bool _tellarknightConstellarDelterosSearchUsed = false;
        private bool _exstellarknightPtolemyOmega7Used = false;
        private bool _constellarPtolemyM7Used = false;
        private bool _constellarPleiadesUsed = false;
        private bool _chronomalyVimanaUsed = false;
        private bool _sakitamaUsed = false;
        private bool _onomatokageUsed = false;
        private bool _caduceusSpSummonUsed = false;
        private bool _caduceusSearchUsed = false;
        private bool _lyranSpSummonUsed = false;
        private bool _lyranSearchUsed = false;
        private bool _altairanSearchUsed = false;
        private bool _altairanGyUsed = false;
        private bool _cygnianSearchUsed = false;

        private static readonly HashSet<int> HandTraps = new HashSet<int>
        {
            CardId.MulcharmyFuwalos,
            CardId.AshBlossom,
            CardId.GhostOgre
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(
                CardId.TellarknightConstellarCaduceus,
                CardId.TellarknightConstellarDelteros,
                CardId.StellarknightDelteros,
                CardId.StellarknightConstellarDiamond,
                CardId.ConstellarPleiades,
                CardId.NumberF0UtopicDracoFuture,
                CardId.ChronomalyVimana
            ) || base.IsAceCard(card);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            return 100;
        }

        protected override bool IsBoardStrongEnough()
        {
            // Caduceus + Draco Future = ideal end board (negate lock)
            if (Bot.HasInMonstersZone(CardId.TellarknightConstellarCaduceus)
                && Bot.HasInMonstersZone(CardId.NumberF0UtopicDracoFuture))
                return true;
            // Draco Future alone = strong enough
            if (Bot.HasInMonstersZone(CardId.NumberF0UtopicDracoFuture))
                return true;
            // Pleiades + Diamond = sufficient control
            if (Bot.HasInMonstersZone(CardId.ConstellarPleiades)
                && Bot.HasInMonstersZone(CardId.StellarknightConstellarDiamond))
                return true;
            // 2+ Xyz bosses = sufficient
            int xyzBossCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && IsAceCard(c));
            if (xyzBossCount >= 2)
                return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            // Stop once we have Draco Future on board
            if (Bot.HasInMonstersZone(CardId.NumberF0UtopicDracoFuture))
                return base.ShouldStopExtending();
            // Stop if we have 2 Xyz bosses
            int xyzBossCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && IsAceCard(c));
            if (xyzBossCount >= 2)
                return base.ShouldStopExtending();
            return false;
        }

        public _2026_TellarknightExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards
            HeuristicGuard.RegisterAceCards(
                CardId.TellarknightConstellarCaduceus,
                CardId.TellarknightConstellarDelteros,
                CardId.StellarknightDelteros,
                CardId.StellarknightConstellarDiamond,
                CardId.ConstellarPleiades,
                CardId.NumberF0UtopicDracoFuture,
                CardId.ChronomalyVimana
            );
            ResourcePlan.RegisterAceCards(
                CardId.TellarknightConstellarCaduceus,
                CardId.TellarknightConstellarDelteros,
                CardId.StellarknightDelteros,
                CardId.StellarknightConstellarDiamond,
                CardId.ConstellarPleiades,
                CardId.NumberF0UtopicDracoFuture,
                CardId.ChronomalyVimana
            );
            // เนโ€โฌเนโ€โฌ Combo Router: Sequencing เนโ€โฌเนโ€โฌ
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.ConstellarCastor, CardId.TellarknightAltairan },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ConstellarCastor, ActionType = ExecutorType.Activate, Description = "Play CardId.ConstellarCastor" },
                    new() { CardId = CardId.TellarknightAltairan, ActionType = ExecutorType.Activate, Description = "Extend with CardId.TellarknightAltairan" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Lyran-Search-Xyz",
                RequiredCards = new List<int> { CardId.TellarknightLyran },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.TellarknightLyran, ActionType = ExecutorType.Summon, Description = "Summon Lyran" },
                    new() { CardId = CardId.TellarknightLyran, ActionType = ExecutorType.Activate, Description = "Lyran searches S/T" },
                    new() { CardId = CardId.TellarknightConstellarCaduceus, ActionType = ExecutorType.SpSummon, Description = "Xyz into Caduceus" }
                },
                EndBoardScore = 85
            });

            // เนโ€โฌเนโ€โฌ Bait Planner เนโ€โฌเนโ€โฌ
            BaitPlanner.RegisterComboStarters(CardId.ConstellarCastor, CardId.TellarknightCygnian, CardId.TellarknightLyran);
            BaitPlanner.RegisterBaitCards(CardId.TellarknightCygnian);

            // เนโ€โฌเนโ€โฌ Chain Advisor เนโ€โฌเนโ€โฌ
            ChainAdvisor.RegisterHighValueTargets(CardId.TellarknightLyran);


            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 1: Hand Traps & Chain Negations เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, GhostOgreEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 2: Archetypal Spells & Traps เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.Activate, CardId.SeventhTachyon, SeventhTachyonEffect);
            AddExecutor(ExecutorType.Activate, CardId.StellarnovaBonds, StellarnovaBondsEffect);
            AddExecutor(ExecutorType.Activate, CardId.SatellarknightSkybridge, SkybridgeEffect);
            AddExecutor(ExecutorType.Activate, CardId.ConstellarTellarknights, ConstellarTellarknightsEffect);
            AddExecutor(ExecutorType.Activate, CardId.PairBearScare, PairBearScareEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 3: Main Deck Summoning & Ignition Effects เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.Summon, CardId.ConstellarCastor, CastorSummon);

            AddExecutor(ExecutorType.SpSummon, CardId.ConstellarCaduceusMain, CaduceusSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.ConstellarCaduceusMain, CaduceusSummon);
            AddExecutor(ExecutorType.Activate, CardId.ConstellarCaduceusMain, CaduceusEffect);

            AddExecutor(ExecutorType.Summon, CardId.TellarknightCygnian, CygnianSummon);
            AddExecutor(ExecutorType.Activate, CardId.TellarknightCygnian, CygnianEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.TellarknightLyran, LyranSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.TellarknightLyran, LyranSummon);
            AddExecutor(ExecutorType.Activate, CardId.TellarknightLyran, LyranEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.TellarknightAltairan, AltairanSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.TellarknightAltairan, AltairanSummon);
            AddExecutor(ExecutorType.Activate, CardId.TellarknightAltairan, AltairanEffect);

            AddExecutor(ExecutorType.Summon, CardId.SatellarknightAltair, AltairSummon);
            AddExecutor(ExecutorType.Activate, CardId.SatellarknightAltair, AltairEffect);

            AddExecutor(ExecutorType.Summon, CardId.SatellarknightUnukalhai, UnukalhaiSummon);
            AddExecutor(ExecutorType.Activate, CardId.SatellarknightUnukalhai, UnukalhaiEffect);

            AddExecutor(ExecutorType.Summon, CardId.DodododoWarrior, DodododoWarriorSummon);
            AddExecutor(ExecutorType.Activate, CardId.DodododoWarrior, DodododoWarriorEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.Onomatokage, OnomatokageSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.Onomatokage, OnomatokageSummon);

            AddExecutor(ExecutorType.Activate, CardId.Sakitama, SakitamaHandEffect);
            AddExecutor(ExecutorType.Summon, CardId.Sakitama, SakitamaSummon);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 4: Extra Deck Xyz Summons เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.SpSummon, CardId.TellarknightPtolemaeus, PtolemaeusSummon);
            AddExecutor(ExecutorType.Activate, CardId.TellarknightPtolemaeus, PtolemaeusEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.TellarknightConstellarCaduceus, TellarknightConstellarCaduceusSummon);
            AddExecutor(ExecutorType.Activate, CardId.TellarknightConstellarCaduceus, TellarknightConstellarCaduceusEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.TellarknightConstellarDelteros, TellarknightConstellarDelterosSummon);
            AddExecutor(ExecutorType.Activate, CardId.TellarknightConstellarDelteros, TellarknightConstellarDelterosEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.StellarknightDelteros, StellarknightDelterosSummon);
            AddExecutor(ExecutorType.Activate, CardId.StellarknightDelteros, StellarknightDelterosEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.StellarknightConstellarDiamond, StellarknightConstellarDiamondSummon);
            AddExecutor(ExecutorType.Activate, CardId.StellarknightConstellarDiamond, StellarknightConstellarDiamondEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.ConstellarPleiades, PleiadesSummon);
            AddExecutor(ExecutorType.Activate, CardId.ConstellarPleiades, PleiadesEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.ChronomalyVimana, VimanaSummon);
            AddExecutor(ExecutorType.Activate, CardId.ChronomalyVimana, VimanaEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.ConstellarPtolemyM7, ConstellarPtolemyM7Summon);
            AddExecutor(ExecutorType.Activate, CardId.ConstellarPtolemyM7, ConstellarPtolemyM7Effect);

            AddExecutor(ExecutorType.SpSummon, CardId.ExstellarknightConstellarPtolemyOmega7, ExstellarknightPtolemyOmega7Summon);
            AddExecutor(ExecutorType.Activate, CardId.ExstellarknightConstellarPtolemyOmega7, ExstellarknightPtolemyOmega7Effect);

            AddExecutor(ExecutorType.SpSummon, CardId.GagagagaMagician, GagagagaMagicianSummon);
            AddExecutor(ExecutorType.Activate, CardId.GagagagaMagician, GagagagaMagicianEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.NumberF0UtopicFuture, UtopicFutureSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.NumberF0UtopicDracoFuture, UtopicDracoFutureSummon);
            AddExecutor(ExecutorType.Activate, CardId.NumberF0UtopicDracoFuture, UtopicDracoFutureEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.Number104Masquerade, MasqueradeSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number104Masquerade, MasqueradeEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.EvilswarmExcitonKnight, ExcitonKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.EvilswarmExcitonKnight, ExcitonKnightEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย Fallbacks เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // First turn preferred to establish disruptions (Pleiades, Vimana, Draco Future)
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _seventhTachyonUsed = false;
            _pairBearScareUsed = false;
            _stellarnovaBondsUsed = false;
            _constellarTellarknightsUsed = false;
            _tellarknightConstellarCaduceusCopyUsed = false;
            _tellarknightConstellarDelterosSearchUsed = false;
            _exstellarknightPtolemyOmega7Used = false;
            _constellarPtolemyM7Used = false;
            _constellarPleiadesUsed = false;
            _chronomalyVimanaUsed = false;
            _sakitamaUsed = false;
            _onomatokageUsed = false;
            _caduceusSpSummonUsed = false;
            _caduceusSearchUsed = false;
            _lyranSpSummonUsed = false;
            _lyranSearchUsed = false;
            _altairanSearchUsed = false;
            _altairanGyUsed = false;
            _cygnianSearchUsed = false;

            // เนโ€โฌเนโ€โฌ Going-Second: prioritize board-building for punch-back เนโ€โฌเนโ€โฌ
            if (ShouldGoBreakBoard)
            {
                // Reset key Xyz boss disruptions for aggressive turn-2 plays
                _constellarPleiadesUsed = false;
                _exstellarknightPtolemyOmega7Used = false;
            }
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Hint 500 / 505: Cost / Detach / Tribute selection
            if (hint == 500 || hint == 505)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 900;
                    if (c.IsCode(CardId.SatellarknightAltair) || c.IsCode(CardId.SatellarknightUnukalhai)) return 10;
                    if (c.IsCode(CardId.TellarknightLyran) || c.IsCode(CardId.TellarknightAltairan) || c.IsCode(CardId.TellarknightCygnian)) return 20;
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 501 / 549: Removal / Disruption Target Selection
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

            // Hint 511 / 512 / 513 / 533 / 508 / 504: Xyz Overlay Material Selection
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533 || hint == 508 || hint == 504)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsAceCard(c)) return 900;
                    if (c.IsCode(CardId.MulcharmyFuwalos)) return 800;
                    if (c.IsCode(CardId.SatellarknightAltair) || c.IsCode(CardId.SatellarknightUnukalhai)) return 10;
                    if (c.IsCode(CardId.TellarknightLyran) || c.IsCode(CardId.TellarknightAltairan)) return 20;
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            if (hint == 502 || hint == 503 || hint == 504 || hint == 505 || hint == 508 || hint == 514 || hint == 551 || hint == 575 || hint == 577 || hint == 507)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count >= min)
                {
                    return enemyCards.Take(Math.Max(min, Math.Min(max, enemyCards.Count))).ToList();
                }
            }

            if (hint == 509)
            {
                bool hasDeck = cards.Any(c => c.Location == CardLocation.Deck);
                if (hasDeck && cards.Any(c => c.Location == CardLocation.Hand))
                {
                    var deckCards = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    return deckCards.Take(Math.Max(min, Math.Min(max, deckCards.Count))).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            // Prioritize materials that have already activated their effects
            IList<ClientCard> result = Util.SelectPreferredCards(new[] {
                CardId.SatellarknightUnukalhai,
                CardId.TellarknightCygnian,
                CardId.ConstellarCastor,
                CardId.DodododoWarrior,
                CardId.Onomatokage,
                CardId.Sakitama,
                CardId.TellarknightLyran,
                CardId.TellarknightAltairan,
                CardId.SatellarknightAltair,
                CardId.ConstellarCaduceusMain
            }, cards, min, max);

            var sorted = result.OrderBy(c => {
                if (c == null) return 9999;
                if (c.Controller == 0)
                {
                    if (IsAceCard(c)) return 10000;
                    if (HandTraps.Contains(c.Id)) return 8000;
                }
                return 100;
            }).ToList();

            return Util.CheckSelectCount(sorted, cards, min, max);
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            return SortMaterials(cards, max);
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            return SortMaterials(cards, max);
        }

        private IList<ClientCard> SortMaterials(IList<ClientCard> cards, int max)
        {
            var sorted = cards.OrderBy(c => {
                if (c == null) return 9999;
                if (c.Controller == 0)
                {
                    if (IsAceCard(c)) return 10000;
                    if (HandTraps.Contains(c.Id)) return 8000;
                }
                return 100;
            }).ToList();
            return sorted.Take(max).ToList();
        }

        // เนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขย TIER 1: Hand Traps & Chain Negations เนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขย

        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool GhostOgreEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return DefaultGhostOgreAndSnowRabbit();
        }

        private bool CalledByTheGraveEffect()
        {
            return DefaultCalledByTheGrave();
        }

        private bool ForbiddenCrownEffect()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;

            // 2. Negate opponent dangerous monster activation
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster() && !lastCard.IsDisabled() && IsViableEffectTarget(lastCard))
                {
                    AI.SelectCard(lastCard);
                    return true;
                }
            }

            // 3. Proactive disruption to negate opponent's monsters on field
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() &&
                    (m.IsMonsterDangerous() || m.IsExtraCard()) && IsViableEffectTarget(m));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return false;
        }

        // เนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขย TIER 2: Spells & Traps เนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขย

        private bool SeventhTachyonEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_seventhTachyonUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Reveal Number 104: Masquerade (Rank 4 Spellcaster LIGHT)
            AI.SelectCard(CardId.Number104Masquerade);

            // Add any Level 4 LIGHT monster
            AI.SelectNextCard(
                CardId.TellarknightCygnian,
                CardId.ConstellarCaduceusMain,
                CardId.TellarknightLyran,
                CardId.ConstellarCastor,
                CardId.Sakitama
            );

            // Put a graveyard-beneficial or duplicate card on top of deck
            AI.SelectNextCard(
                CardId.TellarknightAltairan,
                CardId.SatellarknightAltair,
                CardId.SatellarknightUnukalhai
            );

            _seventhTachyonUsed = true;
            return true;
        }

        private bool StellarnovaBondsEffect()
        {
            if (_stellarnovaBondsUsed) return false;
            if (Bot.Hand.Count <= 1) return false;

            // Select discard
            AI.SelectCard(
                CardId.TellarknightAltairan,
                CardId.SatellarknightAltair,
                CardId.SatellarknightUnukalhai
            );

            // Select deck target with a different Type than controlled monsters
            AI.SelectNextCard(
                CardId.TellarknightCygnian,
                CardId.ConstellarCaduceusMain,
                CardId.TellarknightLyran
            );

            _stellarnovaBondsUsed = true;
            return true;
        }

        private bool SkybridgeEffect()
        {
            if (ShouldSkipCombo()) return false;
            var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                (m.IsCode(CardId.TellarknightCygnian, CardId.TellarknightLyran, 
                         CardId.TellarknightAltairan, CardId.SatellarknightAltair, 
                         CardId.SatellarknightUnukalhai)));
            if (target != null)
            {
                AI.SelectCard(target);
                AI.SelectNextCard(
                    CardId.TellarknightCygnian,
                    CardId.TellarknightLyran,
                    CardId.SatellarknightAltair,
                    CardId.TellarknightAltairan
                );
                return true;
            }
            return false;
        }

        private bool ConstellarTellarknightsEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Activation from hand: Special Summon a monster from hand or GY
                bool hasTarget = Bot.Hand.Any(c => c != null && c != Card && 
                    (c.IsCode(CardId.ConstellarCastor, CardId.TellarknightCygnian, CardId.TellarknightLyran, 
                             CardId.TellarknightAltairan, CardId.SatellarknightAltair, CardId.SatellarknightUnukalhai, 
                             CardId.ConstellarCaduceusMain)))
                    || Bot.Graveyard.Any(c => c != null && c.IsCanRevive() && 
                    (c.IsCode(CardId.ConstellarCastor, CardId.TellarknightCygnian, CardId.TellarknightLyran, 
                             CardId.TellarknightAltairan, CardId.SatellarknightAltair, 
                             CardId.SatellarknightUnukalhai, CardId.ConstellarCaduceusMain)));
                if (hasTarget)
                {
                    AI.SelectCard(
                        CardId.TellarknightCygnian,
                        CardId.TellarknightLyran,
                        CardId.ConstellarCaduceusMain,
                        CardId.TellarknightAltairan,
                        CardId.SatellarknightAltair,
                        CardId.SatellarknightUnukalhai
                    );
                    return true;
                }
            }
            else if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Overlay/Rank-up effect
                if (_constellarTellarknightsUsed) return false;
                var targetXyz = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.HasType(CardType.Xyz) &&
                    (m.IsCode(CardId.TellarknightConstellarCaduceus, CardId.TellarknightConstellarDelteros, 
                             CardId.StellarknightDelteros, CardId.TellarknightPtolemaeus)));
                if (targetXyz != null)
                {
                    AI.SelectCard(targetXyz);
                    AI.SelectNextCard(CardId.ConstellarPleiades, CardId.StellarknightConstellarDiamond);
                    _constellarTellarknightsUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool PairBearScareEffect()
        {
            if (_pairBearScareUsed) return false;
            var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsViableEffectTarget(m));
            if (target != null)
            {
                AI.SelectCard(target);
                _pairBearScareUsed = true;
                return true;
            }
            return false;
        }

        // เนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขย TIER 3: Main Deck Monsters เนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขย

        private bool CastorSummon()
        {
            // Castor grants an extra Normal Summon เนโฌโ€ only summon when we have another Level 4 follow-up
            if (ShouldSkipCombo()) return false;
            bool hasFollowUp = Bot.Hand.Any(c => c != null && c != Card &&
                (c.IsCode(CardId.TellarknightCygnian, CardId.TellarknightLyran, 
                         CardId.TellarknightAltairan, CardId.SatellarknightAltair,
                         CardId.SatellarknightUnukalhai, CardId.ConstellarCaduceusMain,
                         CardId.DodododoWarrior, CardId.Sakitama)));
            return hasFollowUp || Bot.HasInHand(CardId.ConstellarTellarknights) || Bot.HasInHand(CardId.StellarnovaBonds);
        }

        private bool CaduceusSpSummon()
        {
            if (_caduceusSpSummonUsed) return false;
            bool controlOtherConstellar = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && 
                m.Id != CardId.ConstellarCaduceusMain &&
                (m.IsCode(CardId.ConstellarCastor, CardId.TellarknightCygnian, CardId.TellarknightLyran, 
                          CardId.TellarknightAltairan, CardId.SatellarknightAltair, CardId.SatellarknightUnukalhai,
                          CardId.TellarknightConstellarCaduceus, CardId.TellarknightConstellarDelteros,
                          CardId.StellarknightDelteros, CardId.StellarknightConstellarDiamond,
                          CardId.ConstellarPleiades, CardId.ConstellarPtolemyM7,
                          CardId.ExstellarknightConstellarPtolemyOmega7)));
            if (controlOtherConstellar)
            {
                _caduceusSpSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool CaduceusSummon()
        {
            return true;
        }

        private bool CaduceusEffect()
        {
            if (_caduceusSearchUsed) return false;
            if (EnemyHasKnownNegate() || OpponentHasActiveNegator(Card)) return false;
            AI.SelectCard(CardId.ConstellarTellarknights);
            _caduceusSearchUsed = true;
            return true;
        }

        private bool CygnianSummon()
        {
            return true;
        }

        private bool CygnianEffect()
        {
            if (_cygnianSearchUsed) return false;
            AI.SelectCard(
                CardId.ConstellarCaduceusMain,
                CardId.TellarknightLyran,
                CardId.Sakitama,
                CardId.SatellarknightAltair,
                CardId.ConstellarCastor
            );
            _cygnianSearchUsed = true;
            return true;
        }

        private bool LyranSpSummon()
        {
            if (_lyranSpSummonUsed) return false;
            _lyranSpSummonUsed = true;
            return true;
        }

        private bool LyranSummon()
        {
            return true;
        }

        private bool LyranEffect()
        {
            if (_lyranSearchUsed) return false;
            if (EnemyHasKnownNegate() || OpponentHasActiveNegator(Card)) return false;
            AI.SelectCard(CardId.ConstellarTellarknights, CardId.StellarnovaBonds, CardId.SatellarknightSkybridge);
            _lyranSearchUsed = true;
            return true;
        }

        private bool AltairanSpSummon()
        {
            if (_altairanGyUsed) return false;
            // Only self-revive from GY when a Constellar/Tellarknight is on field
            if (Card.Location != CardLocation.Grave) return false;
            bool hasTellarknight = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && 
                (m.IsCode(CardId.TellarknightCygnian, CardId.TellarknightLyran, CardId.ConstellarCastor, 
                          CardId.SatellarknightAltair, CardId.SatellarknightUnukalhai, CardId.ConstellarCaduceusMain,
                          CardId.TellarknightConstellarCaduceus, CardId.TellarknightConstellarDelteros)));
            if (!hasTellarknight) return false;
            _altairanGyUsed = true;
            return true;
        }

        private bool AltairanSummon()
        {
            return true;
        }

        private bool AltairanEffect()
        {
            if (ActivateDescription == Util.GetStringId(CardId.TellarknightAltairan, 1) || Card.Location == CardLocation.Grave)
            {
                if (_altairanGyUsed) return false;
                var targets = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.HasType(CardType.Xyz) &&
                    (m.IsCode(CardId.TellarknightConstellarCaduceus, CardId.TellarknightConstellarDelteros, CardId.StellarknightDelteros, CardId.StellarknightConstellarDiamond))).ToList();
                if (targets.Count > 0)
                {
                    AI.SelectCard(targets);
                    _altairanGyUsed = true;
                    return true;
                }
                return false;
            }

            if (ActivateDescription == Util.GetStringId(CardId.TellarknightAltairan, 0) || Card.Location == CardLocation.MonsterZone)
            {
                if (_altairanSearchUsed) return false;
                int xyzCount = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.HasType(CardType.Xyz) && 
                    (m.HasAttribute(CardAttribute.Light) || m.HasAttribute(CardAttribute.Dark)));
                if (xyzCount > 0)
                {
                    var targets = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                        .Where(c => c != null && IsViableEffectTarget(c)).ToList();
                    if (targets.Count > 0)
                    {
                        AI.SelectCard(targets);
                        _altairanSearchUsed = true;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool AltairSummon()
        {
            if (Duel.Phase == DuelPhase.Main1 && Duel.Turn > 1)
            {
                var nonTellarAttackers = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.IsAttack() && !m.Attacked &&
                    !m.IsCode(CardId.SatellarknightAltair, CardId.SatellarknightUnukalhai, CardId.TellarknightCygnian, CardId.TellarknightLyran, CardId.TellarknightAltairan));
                if (nonTellarAttackers.Any(m => m.Attack >= 1500) && CanDealLethal()) return false;
            }
            // Summon Altair if we have a target to revive
            return Bot.Graveyard.Any(c => c != null && c.IsCanRevive() && 
                (c.IsCode(CardId.TellarknightCygnian, CardId.TellarknightLyran, CardId.TellarknightAltairan, CardId.SatellarknightUnukalhai)));
        }

        private bool AltairEffect()
        {
            var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCanRevive() && 
                (c.IsCode(CardId.TellarknightCygnian, CardId.TellarknightLyran, CardId.TellarknightAltairan, CardId.SatellarknightUnukalhai)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool UnukalhaiSummon()
        {
            return true;
        }

        private bool UnukalhaiEffect()
        {
            if (ShouldSkipCombo()) return false;
            // Send Altairan to set up GY revival, or Altair/Lyran
            AI.SelectCard(CardId.TellarknightAltairan, CardId.SatellarknightAltair, CardId.TellarknightLyran);
            return true;
        }

        private bool DodododoWarriorSummon()
        {
            AI.SelectOption(1); // Normal summon without tribute
            return true;
        }

        private bool DodododoWarriorEffect()
        {
            return true; // Make its Level 4
        }

        private bool OnomatokageSpSummon()
        {
            if (_onomatokageUsed) return false;
            bool hasDododo = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.DodododoWarrior));
            if (hasDododo)
            {
                _onomatokageUsed = true;
                return true;
            }
            return false;
        }

        private bool OnomatokageSummon()
        {
            return true;
        }

        private bool SakitamaHandEffect()
        {
            if (_sakitamaUsed) return false;
            _sakitamaUsed = true;
            return true;
        }

        private bool SakitamaSummon()
        {
            return true;
        }

        // เนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขย TIER 4: Extra Deck Monsters เนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขยเนโ€ขย

        private bool PtolemaeusSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool PtolemaeusEffect()
        {
            if (ActivateDescription == Util.GetStringId(CardId.TellarknightPtolemaeus, 1))
            {
                var targets = Bot.ExtraDeck.Where(c => c != null && 
                    (c.IsCode(CardId.StellarknightDelteros, CardId.TellarknightConstellarDelteros, CardId.TellarknightConstellarCaduceus))).ToList();
                if (targets.Count > 0)
                {
                    AI.SelectCard(targets[0]);
                    return true;
                }
                return false;
            }

            if (ActivateDescription == Util.GetStringId(CardId.TellarknightPtolemaeus, 0) || Card.Overlays.Count >= 3)
            {
                if (Duel.Player == 1)
                {
                    if (Enemy.GetMonsterCount() > 0 || Duel.CurrentChain.Count > 0)
                    {
                        AI.SelectCard(CardId.ConstellarPleiades, CardId.ChronomalyVimana);
                        return true;
                    }
                }
                else if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1 && ShouldRushAttack)
                {
                    AI.SelectCard(CardId.ConstellarPleiades, CardId.ChronomalyVimana);
                    return true;
                }
            }
            return false;
        }

        private bool TellarknightConstellarCaduceusSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool TellarknightConstellarCaduceusEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ActivateDescription == Util.GetStringId(CardId.TellarknightConstellarCaduceus, 0))
                {
                    AI.SelectCard(
                        CardId.TellarknightLyran,
                        CardId.TellarknightCygnian,
                        CardId.ConstellarCaduceusMain,
                        CardId.SatellarknightAltair
                    );
                    return true;
                }
                else
                {
                    if (_tellarknightConstellarCaduceusCopyUsed) return false;
                    
                    bool hasGyTarget = Bot.Graveyard.Any(c => c != null && c.IsCanRevive() && 
                        c.IsCode(CardId.TellarknightCygnian, CardId.TellarknightLyran, CardId.TellarknightAltairan, CardId.SatellarknightUnukalhai));
                    
                    if (hasGyTarget)
                    {
                        // Copy Altair to revive
                        AI.SelectCard(CardId.SatellarknightAltair, CardId.TellarknightCygnian, CardId.TellarknightLyran);
                        AI.SelectNextCard(
                            CardId.TellarknightLyran,
                            CardId.TellarknightCygnian,
                            CardId.TellarknightAltairan,
                            CardId.SatellarknightUnukalhai
                        );
                    }
                    else
                    {
                        // Copy Cygnian to dump Unukalhai/Altairan
                        AI.SelectCard(CardId.TellarknightCygnian, CardId.TellarknightLyran, CardId.SatellarknightAltair);
                        AI.SelectNextCard(CardId.TellarknightAltairan, CardId.SatellarknightAltair, CardId.TellarknightLyran);
                    }
                    
                    _tellarknightConstellarCaduceusCopyUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool TellarknightConstellarDelterosSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool TellarknightConstellarDelterosEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(
                    CardId.TellarknightLyran,
                    CardId.TellarknightCygnian,
                    CardId.ConstellarCaduceusMain,
                    CardId.SatellarknightAltair,
                    CardId.TellarknightAltairan,
                    CardId.SatellarknightUnukalhai
                );
                return true;
            }

            if (ActivateDescription == Util.GetStringId(CardId.TellarknightConstellarDelteros, 0))
            {
                if (_tellarknightConstellarDelterosSearchUsed) return false;
                AI.SelectCard(
                    CardId.SatellarknightAltair,
                    CardId.TellarknightCygnian,
                    CardId.ConstellarCaduceusMain,
                    CardId.TellarknightLyran,
                    CardId.ConstellarCastor
                );
                _tellarknightConstellarDelterosSearchUsed = true;
                return true;
            }
            
            // Destroy effect
            var target = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .FirstOrDefault(c => c != null && IsViableEffectTarget(c));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }

            return false;
        }

        private bool StellarknightDelterosSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool StellarknightDelterosEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(
                    CardId.TellarknightLyran,
                    CardId.TellarknightCygnian,
                    CardId.SatellarknightAltair,
                    CardId.TellarknightAltairan,
                    CardId.SatellarknightUnukalhai
                );
                return true;
            }

            var target = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .FirstOrDefault(c => c != null && IsViableEffectTarget(c));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool StellarknightConstellarDiamondSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool enemyHasDark = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasAttribute(CardAttribute.Dark)) ||
                                Enemy.Graveyard.Any(m => m != null && m.HasAttribute(CardAttribute.Dark));
            if (!enemyHasDark && Duel.Phase != DuelPhase.Main2) return false;

            var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                (m.IsCode(CardId.TellarknightPtolemaeus, CardId.TellarknightConstellarCaduceus,
                         CardId.TellarknightConstellarDelteros, CardId.StellarknightDelteros)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool StellarknightConstellarDiamondEffect()
        {
            if (Duel.CurrentChain.Count > 0 && Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && lastCard.HasAttribute(CardAttribute.Dark))
                {
                    return true;
                }
            }
            return false;
        }

        private bool PleiadesSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool PleiadesEffect()
        {
            if (_constellarPleiadesUsed) return false;
            if (Duel.Player == 1 || Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                var target = Util.GetProblematicEnemyMonster(0, true);
                if (target == null)
                    target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsViableEffectTarget(m));
                if (target == null)
                    target = Enemy.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() && IsViableEffectTarget(s));

                if (target != null)
                {
                    AI.SelectCard(target);
                    _constellarPleiadesUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool VimanaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool VimanaEffect()
        {
            if (Duel.CurrentChain.Count > 0 && Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster() && !lastCard.IsDisabled())
                {
                    return true;
                }
            }

            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (_chronomalyVimanaUsed) return false;
                var fieldTarget = Card;
                var gyTarget = Bot.Graveyard.Concat(Enemy.Graveyard)
                    .Where(c => c != null && c.HasType(CardType.Xyz)).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (fieldTarget != null && gyTarget != null)
                {
                    AI.SelectCard(fieldTarget);
                    AI.SelectNextCard(gyTarget);
                    _chronomalyVimanaUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool ConstellarPtolemyM7Summon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() &&
                (m.IsCode(CardId.ConstellarPleiades, CardId.TellarknightConstellarCaduceus,
                         CardId.TellarknightConstellarDelteros, CardId.StellarknightConstellarDiamond)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ConstellarPtolemyM7Effect()
        {
            if (_constellarPtolemyM7Used) return false;

            var opponentMonster = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsViableEffectTarget(m));
            if (opponentMonster != null)
            {
                AI.SelectCard(opponentMonster);
                _constellarPtolemyM7Used = true;
                return true;
            }

            var ourGyMonster = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && 
                (c.IsCode(CardId.ConstellarCaduceusMain, CardId.TellarknightLyran, CardId.TellarknightCygnian, CardId.Sakitama)));
            if (ourGyMonster != null)
            {
                AI.SelectCard(ourGyMonster);
                _constellarPtolemyM7Used = true;
                return true;
            }

            return false;
        }

        private bool ExstellarknightPtolemyOmega7Summon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Duel.Phase != DuelPhase.Main2) return false;

            var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && 
                (m.IsCode(CardId.TellarknightConstellarCaduceus, CardId.TellarknightConstellarDelteros,
                         CardId.StellarknightDelteros, CardId.TellarknightPtolemaeus,
                         CardId.ConstellarPleiades, CardId.StellarknightConstellarDiamond)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ExstellarknightPtolemyOmega7Effect()
        {
            if (_exstellarknightPtolemyOmega7Used) return false;

            var targets = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && IsViableEffectTarget(m)).ToList();
            if (targets.Count > 0)
            {
                AI.SelectCard(targets);
                _exstellarknightPtolemyOmega7Used = true;
                return true;
            }
            return false;
        }

        private bool GagagagaMagicianSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Xyz) && c.Id != CardId.GagagagaMagician);
        }

        private bool GagagagaMagicianEffect()
        {
            var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.HasType(CardType.Xyz) && c.Id != CardId.GagagagaMagician);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool UtopicFutureSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool UtopicDracoFutureSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.IsCode(CardId.NumberF0UtopicFuture));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool UtopicDracoFutureEffect()
        {
            if (Duel.CurrentChain.Count > 0 && Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster() && !lastCard.IsDisabled())
                {
                    return true;
                }
            }
            return false;
        }

        private bool MasqueradeSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool MasqueradeEffect()
        {
            if (Duel.CurrentChain.Count > 0 && Duel.LastChainPlayer == 1)
            {
                return true;
            }
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                return true;
            }
            return false;
        }

        private bool ExcitonKnightSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int ourCount = Bot.Hand.Count + Bot.GetMonsterCount() + Bot.GetSpellCount();
            int enemyCount = Enemy.Hand.Count + Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            return enemyCount > ourCount && Enemy.GetMonsterCount() >= 2;
        }

        private bool ExcitonKnightEffect()
        {
            int ourCount = Bot.Hand.Count + Bot.GetMonsterCount() + Bot.GetSpellCount();
            int enemyCount = Enemy.Hand.Count + Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            return enemyCount > ourCount;
        }
    }

    [Deck("Expert_2026_Tellarknight", "2026_Tellarknight")]
    public class ExpertTellarknightExecutor : _2026_TellarknightExecutor
    {
        private string _duelId;
        public ExpertTellarknightExecutor(GameAI ai, Duel duel) : base(ai, duel)
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
