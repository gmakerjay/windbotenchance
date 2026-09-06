using System;
using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // CARD AUDIT — 2026_ArtMage
    // ============================================================
    // | Card Name                                 | Type    | OPT? | Cost            | Effect Summary                                | Activate When                     | NEVER Activate When                    |
    // |-------------------------------------------|---------|------|-----------------|-----------------------------------------------|-----------------------------------|----------------------------------------|
    // | Medius the Pure (97556336)                | Monster | Yes  | None            | Summon: Add or SS 1 "Power Patron" from Deck  | Normal/Special Summoned           | No target left in Deck                 |
    // | Shadow Beast Nervedo (17473466)           | Monster | Yes  | Banish 3 top    | Field: SS Nerva from Extra Deck               | Main Phase                        | SS blocked / less than 3 cards in Deck |
    // | Shadow Machine Zegredo (43871165)         | Monster | Yes  | Destroy self+1  | Pendulum: Destroy self + 1 card, pop 1 field  | Main Phase                        | No target to destroy                   |
    // | Artmage Power Patron (23829452)           | Monster | Yes  | Discard self    | Hand: Add 1 "Power Patron" from Deck          | Main Phase                        | No target in Deck                      |
    // | Artmage Finmel (34541940)                 | Monster | Yes  | Tribute self    | Field: SS Level 4 or lower Artmage from Deck   | Main Phase                        | No target in Deck                      |
    // | Artmage Graflare (60946049)               | Monster | Yes  | Tribute self    | Field: SS Level 4 or lower Artmage from Deck   | Main Phase                        | No target in Deck                      |
    // | Artmage Litera (97434754)                 | Monster | Yes  | Return to hand  | Field: Quick SS 1 Artmage from hand/GY        | Opponent's Main Phase             | No target in hand or GY                |
    // | Vidrium (70488851)                        | Monster | Yes  | Banish self GY  | GY: Banish 1 opponent monster                 | Any Phase                         | No enemy target / GY not contains card |
    // | Nerva (53589300)                          | Fusion  | Yes  | None            | Field: (Quick) Change Artmage effect -> pop all| When Artmage effect is activated  | Opponent controls no cards             |
    // | Artmage Diactorus (27184601)              | Fusion  | Yes  | None            | Summon: Add 1 Artmage from Deck to hand       | Normal/Special Summoned           | No target in Deck                      |
    // | Artmage Non-Finito (74631897)             | Fusion  | Yes  | None            | Field: Increase levels of Artmage by 2        | Main Phase                        | Already activated                      |
    // | Acropolis (74733322)                      | Spell   | Yes  | Discard S/T     | Field: Extra Normal Summon Medius, Search Art | Main Phase                        | Already searched / no target in Deck   |
    // | Varnish (74011784)                        | Spell   | Yes  | None            | Play Field from Deck/GY, or search Artmage    | Main Phase                        | Already activated                      |
    // | Pact (23599634)                           | Spell   | Yes  | None            | SS 1 Artmage/Medius from Deck, negate protect  | Main Phase                        | SS blocked                             |
    // | Masterwork (37517035)                     | Spell   | Yes  | None            | Fusion Summon using hand/field               | Main Phase                        | No valid fusion materials              |
    // | Vandalism (1122030)                       | Spell   | Yes  | None            | Add Medius from Deck, treat as Artmage fusion  | Main Phase                        | No Medius in Deck                      |
    // | Portal Terminus (25661743)                | Spell   | Yes  | Send Power GY   | Send Power GY, add DARK Fairy from Deck to hand| Main Phase                        | No target in Deck                      |
    // | Impasto Recapture (44654994)              | Trap    | Yes  | Banish Fusion   | Negate monster effect by banishing fusion    | Opponent monster effect activation| No face-up Fusion monster to banish    |
    // ============================================================
    // ACE CARDS: Primary: Nerva (53589300) / Secondary: Diactorus (27184601) / Tertiary: Non-Finito (74631897)
    // COMBO STARTERS: 1. Vandalism (1122030) 2. Varnish (74011784) 3. Pact (23599634) 4. Medius the Pure (97556336)
    // CHOKEPOINTS: Medius normal summon or Nervedo activation negated.
    // WIN CONDITION: Special Summon Nerva using Nervedo's effect, setup Acropolis, activate an Artmage effect, chain Nerva to trigger board wipe, then push for game.
    // ============================================================

    [Deck("2026_ArtMage", "2026_ArtMage")]
    public class _2026_ArtMageExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Archetype Cards
            public const int MediusThePure = 97556336;
            public const int ShadowBeastNervedo = 17473466;
            public const int ShadowMachineZegredo = 43871165;
            public const int ArtmagePowerPatron = 23829452;
            public const int Vidrium = 70488851;
            public const int ArtmageFinmel = 34541940;
            public const int ArtmageGraflare = 60946049;
            public const int ArtmageLitera = 97434754;

            // Main Deck Archetype Spells & Traps
            public const int Acropolis = 74733322;
            public const int Varnish = 74011784;
            public const int Pact = 23599634;
            public const int Masterwork = 37517035;
            public const int Vandalism = 1122030;
            public const int PortalTerminus = 25661743;
            public const int ImpastoRecapture = 44654994;

            // Supporting Staples
            public const int FallenVirtuous = 30271097;
            public const int SuperPoly = 48130397;
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int Droll = 94145021;
            public const int Imperm = 10045474;
            public const int CalledByTheGrave = 24224830;
            public const int TripleTacticsTalent = 25311006;

            // Extra Deck
            public const int Nerva = 53589300;
            public const int ArtmageDiactorus = 27184601;
            public const int ArtmageNonFinito = 74631897;
            public const int Garura = 11765832;
            public const int Mudragon = 54757758;
            public const int Dragostapelia = 69946549;
            public const int Albion = 87746184;
            public const int EcclesiaDarkDragon = 78397661;
            public const int SeaMonsterTheseus = 96334243;
            public const int FlameWingman = 13243124;
            public const int AzaminaIliaSilvia = 46396218;
            public const int SPLittleKnight = 29301450;
            public const int CrossSheep = 50277355;
        }

        private static readonly int[] BossMonsters = { CardId.Nerva, CardId.ArtmageDiactorus, CardId.ArtmageNonFinito };
        private static readonly int[] HandTraps = { CardId.AshBlossom, CardId.MaxxC, CardId.Droll, CardId.Imperm };

        // OPT trackers
        private bool _acropolisSearchUsed = false;
        private bool _varnishUsed = false;
        private bool _pactUsed = false;
        private bool _masterworkUsed = false;
        private bool _vandalismUsed = false;
        private bool _terminusUsed = false;
        private bool _recaptureUsed = false;
        private bool _mediusSummonSearchUsed = false;
        private bool _mediusGYSSEused = false;
        private bool _nervedoBanishUsed = false;
        private bool _nervedoPendulumNegateUsed = false;
        private bool _artmagePowerPatronSearchUsed = false;
        private bool _artmagePowerPatronSSEused = false;
        private bool _finmelTributeUsed = false;
        private bool _finmelSSEused = false;
        private bool _graflareTributeUsed = false;
        private bool _graflareSSEused = false;
        private bool _literaSSUsed = false;
        private bool _literaQuickSSUsed = false;
        private bool _vidriumSSEused = false;
        private bool _vidriumBanishUsed = false;
        private bool _zegredoPendulumUsed = false;
        private bool _diactorusSearchUsed = false;
        private bool _nonFinitoLevelUsed = false;
        private bool _fallenVirtuousUsed = false;

        private int _lastAnnouncedId = 0;

        public _2026_ArtMageExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(BossMonsters);

            // ── Combo Router ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.Vandalism },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Vandalism, ActionType = ExecutorType.Activate, Description = "Search Medius with Vandalism" },
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Summon, Description = "Summon Medius" },
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Activate, Description = "SS Nervedo with Medius" },
                    new() { CardId = CardId.ShadowBeastNervedo, ActionType = ExecutorType.Activate, Description = "SS Nerva with Nervedo" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Medius-Starter",
                RequiredCards = new List<int> { CardId.MediusThePure },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Summon, Description = "Summon Medius" },
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Activate, Description = "SS Nervedo with Medius" },
                    new() { CardId = CardId.ShadowBeastNervedo, ActionType = ExecutorType.Activate, Description = "SS Nerva with Nervedo" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "OTK-BoardBreak",
                RequiredCards = new List<int> { CardId.SuperPoly },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.SuperPoly, ActionType = ExecutorType.Activate, Description = "Clear board with Super Poly" },
                    new() { CardId = CardId.Pact, ActionType = ExecutorType.Activate, Description = "SS Medius or Litera with Pact" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Fallback-Grind",
                RequiredCards = new List<int> { CardId.ArtmageLitera },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ArtmageLitera, ActionType = ExecutorType.Summon, Description = "Normal Summon Litera" },
                    new() { CardId = CardId.ImpastoRecapture, ActionType = ExecutorType.SpellSet, Description = "Set Impasto Recapture" }
                },
                EndBoardScore = 50
            });

            BaitPlanner.RegisterComboStarters(CardId.Vandalism, CardId.Varnish, CardId.Pact, CardId.MediusThePure);
            BaitPlanner.RegisterBaitCards(CardId.Vandalism, CardId.Varnish, CardId.TripleTacticsTalent);
            ChainAdvisor.RegisterHighValueTargets(CardId.MediusThePure, CardId.ShadowBeastNervedo, CardId.Nerva);

            // TIER 1: Hand Traps & Negates
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.Droll, DrollEffect);
            AddExecutor(ExecutorType.Activate, CardId.Imperm, ImpermEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);

            // TIER 2: Boss / Disruption Quick Effects
            AddExecutor(ExecutorType.Activate, CardId.Nerva, NervaQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.ShadowBeastNervedo, NervedoPendulumNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageLitera, LiteraQuickSSEffect);

            // TIER 3: Board Breakers / Fusion Spells
            AddExecutor(ExecutorType.Activate, CardId.SuperPoly, SuperPolyEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.FallenVirtuous, FallenVirtuousEffect);

            // TIER 4: Setup / Search Spells
            AddExecutor(ExecutorType.Activate, CardId.Vandalism, VandalismEffect);
            AddExecutor(ExecutorType.Activate, CardId.Varnish, VarnishEffect);
            AddExecutor(ExecutorType.Activate, CardId.Acropolis, AcropolisEffect);
            AddExecutor(ExecutorType.Activate, CardId.PortalTerminus, PortalTerminusEffect);
            AddExecutor(ExecutorType.Activate, CardId.Pact, PactEffect);
            AddExecutor(ExecutorType.Activate, CardId.Masterwork, MasterworkEffect);

            // TIER 5: Monster Effects (Field/GY/Hand SS)
            AddExecutor(ExecutorType.Activate, CardId.ShadowBeastNervedo, NervedoBanishEffect);
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusSummonSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusGYReviveEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmagePowerPatron, PowerPatronFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmagePowerPatron, PowerPatronGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageFinmel, FinmelSSEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageFinmel, FinmelNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageGraflare, GraflareSSEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageGraflare, GraflareDestroyEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageLitera, LiteraSSEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Vidrium, VidriumAlternateSummon);
            AddExecutor(ExecutorType.Activate, CardId.Vidrium, VidriumTriggerEffects);
            AddExecutor(ExecutorType.Activate, CardId.ShadowMachineZegredo, ZegredoPendulumEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageDiactorus, DiactorusPosEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageDiactorus, DiactorusNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageDiactorus, DiactorusDestroyedEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ArtmageNonFinito, NonFinitoAlternateSummon);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageNonFinito, NonFinitoSetSpellTrap);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageNonFinito, NonFinitoQuickFusion);

            // TIER 6: Special Summons (Hand/GY/Extra)
            AddExecutor(ExecutorType.SpSummon, CardId.ArtmageFinmel);
            AddExecutor(ExecutorType.SpSummon, CardId.ArtmageGraflare);
            AddExecutor(ExecutorType.SpSummon, CardId.ArtmageLitera);

            // TIER 7: Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure, ShouldSummonMedius);
            AddExecutor(ExecutorType.Summon, CardId.ArtmageLitera);
            AddExecutor(ExecutorType.Summon, CardId.ShadowBeastNervedo);

            // TIER 7b: Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure);
            AddExecutor(ExecutorType.Summon, CardId.ArtmageFinmel);
            AddExecutor(ExecutorType.Summon, CardId.ArtmageGraflare);

            // TIER 8: Extra Deck Summons
            AddExecutor(ExecutorType.SpSummon, CardId.Nerva);
            AddExecutor(ExecutorType.SpSummon, CardId.ArtmageDiactorus);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrossSheep, CrossSheepSummon);

            // TIER 9: Trap sets & activations
            AddExecutor(ExecutorType.Activate, CardId.ImpastoRecapture, ImpastoRecaptureEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.ImpastoRecapture);
            AddExecutor(ExecutorType.SpellSet, CardId.Pact);
            AddExecutor(ExecutorType.SpellSet, CardId.Imperm);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);

            // LAST: Repos
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand() => true;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);

            _acropolisSearchUsed = false;
            _varnishUsed = false;
            _pactUsed = false;
            _masterworkUsed = false;
            _vandalismUsed = false;
            _terminusUsed = false;
            _recaptureUsed = false;
            _mediusSummonSearchUsed = false;
            _mediusGYSSEused = false;
            _nervedoBanishUsed = false;
            _nervedoPendulumNegateUsed = false;
            _artmagePowerPatronSearchUsed = false;
            _artmagePowerPatronSSEused = false;
            _finmelTributeUsed = false;
            _finmelSSEused = false;
            _graflareTributeUsed = false;
            _graflareSSEused = false;
            _literaSSUsed = false;
            _literaQuickSSUsed = false;
            _vidriumSSEused = false;
            _vidriumBanishUsed = false;
            _zegredoPendulumUsed = false;
            _diactorusSearchUsed = false;
            _nonFinitoLevelUsed = false;
            _fallenVirtuousUsed = false;
            _lastAnnouncedId = 0;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return BossMonsters.Contains(card.Id) || card.Id == CardId.SPLittleKnight;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.Nerva)) return true;
            if (Bot.HasInMonstersZone(CardId.ArtmageDiactorus)) return true;
            if (Bot.HasInMonstersZone(CardId.SPLittleKnight)) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (Bot.HasInMonstersZone(CardId.Nerva)
                || Bot.HasInMonstersZone(CardId.ArtmageDiactorus))
                return base.ShouldStopExtending();
            return false;
        }

        private bool IsExpendableSpellTrap(ClientCard c)
        {
            if (c == null || (!c.IsSpell() && !c.IsTrap())) return false;
            if (c.Id == CardId.TripleTacticsTalent || c.Id == CardId.SuperPoly || c.Id == CardId.CalledByTheGrave || c.Id == CardId.Imperm)
                return false;
            if (c.Id == CardId.Acropolis && (Bot.HasInSpellZone(CardId.Acropolis) || Bot.Hand.Count(h => h != null && h.Id == CardId.Acropolis) > 1))
                return true;
            if (c.Id == CardId.Varnish || c.Id == CardId.Masterwork || c.Id == CardId.PortalTerminus || c.Id == CardId.Pact)
                return true;
            return false;
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
            if (HandTraps.Contains(c.Id)) return 900;
            if (c.Id == CardId.CalledByTheGrave || c.Id == CardId.SuperPoly || c.Id == CardId.TripleTacticsTalent) return 850;

            // Key Archetype Bosses / Main Starters & Wincons (ATK 2500 - 2800)
            if (c.Id == CardId.ShadowBeastNervedo) return 800; // 2800 ATK, primary starter into Nerva
            if (c.Id == CardId.ShadowMachineZegredo) return 750; // 2800 ATK, Scale 8 / Pop
            if (c.Id == CardId.Vidrium) return 700; // 2800 ATK, GY banish
            if (c.Id == CardId.ArtmagePowerPatron) return 650; // 2500 ATK, searches Power Patron
            if (c.Id == CardId.MediusThePure) return 500; // 1800 ATK, searches archetype

            // Low-ATK / expendable monsters
            if (c.Id == CardId.ArtmageGraflare) return 150; // 1500 ATK, tributes itself
            if (c.Id == CardId.ArtmageFinmel) return 120; // 1200 ATK, tributes itself
            if (c.Id == CardId.ArtmageLitera) return 100; // 0 ATK, bounces / floats

            // Fallback: Higher attack = higher priority (protect high ATK monsters from being thrown away)
            if (c.IsMonster())
            {
                return 200 + Math.Min(300, c.Attack / 10);
            }

            return base.GetMaterialPriority(c);
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

            if (hint == 506 && _lastAnnouncedId != 0)
            {
                var target = cards.FirstOrDefault(c => c.Id == _lastAnnouncedId);
                if (target != null)
                {
                    _lastAnnouncedId = 0;
                    return new[] { target };
                }
            }

            // HINT_SPSUMMON (509) - e.g. Diactorus destroy revive Medius or Pact
            if (hint == 509)
            {
                bool hasDeck = cards.Any(c => c.Location == CardLocation.Deck);
                if (hasDeck && cards.Any(c => c.Location != CardLocation.Deck))
                {
                    var targets = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    return Util.CheckSelectCount(targets, cards, min, max);
                }
                var sorted = cards.OrderByDescending(c => c.Attack).ToList();
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            // HINT_DESTROY (502) / HINT_REMOVE (504) - Target selection (e.g. Fallen & Virtuous, etc.)
            // Prioritize enemy cards, NEVER target own boss/ace cards if enemy cards exist!
            if (hint == 502 || hint == 504)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count >= min)
                {
                    var sortedEnemy = enemyCards
                        .OrderByDescending(c => c.IsFaceup())
                        .ThenByDescending(c => c.Attack)
                        .ToList();
                    return Util.CheckSelectCount(sortedEnemy, cards, min, max);
                }
            }

            // HINT_TODECK (507) - e.g. Medius GY revive (shuffle 1 monster into deck)
            // Choose expendable / low ATK monsters or extra copies, NEVER shuffle high ATK bosses or field Aces!
            if (hint == 507)
            {
                var nonAce = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
                var sorted = (nonAce.Count >= min ? nonAce : cards.Where(c => c != null).ToList())
                    .OrderBy(c => GetMaterialPriority(c))
                    .ThenBy(c => c.Attack)
                    .ToList();
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            // HINT_TOGRAVE (500) / HINT_DISCARD (501) / Materials (511, 512, 513, 533)
            if (hint == 500 || hint == 501 || hint == 511 || hint == 512 || hint == 513 || hint == 533)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ThenBy(c => c.Attack)
                    .ToList();

                if (cancelable)
                {
                    var nonFieldAces = sorted.Where(c => !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
                    if (nonFieldAces.Count < min) return null;
                    return Util.CheckSelectCount(nonFieldAces, cards, min, max);
                }
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ============================================================
        // Hand Traps & Staples
        // ============================================================

        private bool MaxxCEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            return Duel.LastChainPlayer == 1;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool DrollEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            return Duel.LastChainPlayer == 1;
        }

        private bool ImpermEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            return DefaultInfiniteImpermanence();
        }

        private bool CalledByTheGraveEffect()
        {
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            return DefaultCalledByTheGrave();
        }

        // ============================================================
        // Quick Effects & Disruptions
        // ============================================================

        private bool NervaQuickEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;

            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain == null || lastChain.Controller != 0) return false;

            bool isOurArtmage = lastChain.HasSetcode(0x1c7);

            if (isOurArtmage && Enemy.GetFieldCount() > 0)
            {
                DecisionTracer.TraceActivate("NervaQuickEffect", "Chaining Nerva to trigger board wipe");
                return true;
            }
            return false;
        }

        private bool NervedoPendulumNegateEffect()
        {
            if (Card.Location != CardLocation.PendulumZone) return false;
            if (Card.IsDisabled()) return false;
            if (_nervedoPendulumNegateUsed) return false;

            if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard()?.Controller == 1)
            {
                _nervedoPendulumNegateUsed = true;
                DecisionTracer.TraceActivate("NervedoPendulumNegateEffect", "Negating opponent's response");
                return true;
            }
            return false;
        }

        private bool SPLittleKnightEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;

            if (Duel.LastChainPlayer == 1)
            {
                DecisionTracer.TraceActivate("SPLittleKnightEffect", "Activating S:P Little Knight quick banish");
                return true;
            }
            return false;
        }

        private bool LiteraQuickSSEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_literaQuickSSUsed) return false;
            if (Duel.Player != 1) return false;

            bool hasTarget = Bot.Hand.Any(c => c != null && (c.Id == CardId.MediusThePure || c.HasSetcode(0x1c7)))
                || Bot.Graveyard.Any(c => c != null && (c.Id == CardId.MediusThePure || c.HasSetcode(0x1c7)) && c.IsCanRevive());

            if (hasTarget)
            {
                _literaQuickSSUsed = true;
                DecisionTracer.TraceActivate("LiteraQuickSSEffect", "Returning Litera to hand, SS Artmage/Medius target");
                return true;
            }
            return false;
        }

        // ============================================================
        // Board Breakers
        // ============================================================

        private bool SuperPolyEffect()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            if (Bot.Hand.Count < 1) return false;

            if (Enemy.GetMonsterCount() >= 2)
            {
                DecisionTracer.TraceActivate("SuperPolyEffect", "Activating Super Polymerization");
                return true;
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            DecisionTracer.TraceActivate("TripleTacticsTalentEffect", "Activating Triple Tactics Talent");
            return true;
        }

        private bool FallenVirtuousEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_fallenVirtuousUsed) return false;

            bool hasEcclesia = Bot.HasInMonstersZone(CardId.EcclesiaDarkDragon)
                || Bot.Graveyard.Any(c => c != null && c.Id == CardId.EcclesiaDarkDragon);

            if (hasEcclesia && Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.IsCanRevive()))
            {
                ClientCard reviveTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.IsCanRevive())
                    ?? Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.IsCanRevive());

                if (reviveTarget != null)
                {
                    AI.SelectOption(1);
                    AI.SelectCard(reviveTarget);
                    AI.SelectPosition(CardPosition.FaceUpDefence);
                    _fallenVirtuousUsed = true;
                    DecisionTracer.TraceActivate("FallenVirtuousEffect", $"Reviving monster {reviveTarget.Name} with Fallen & Virtuous");
                    return true;
                }
            }
            else if (GetRemainingCount(CardId.Albion) > 0 || GetRemainingCount(CardId.EcclesiaDarkDragon) > 0)
            {
                var enemyTarget = Enemy.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && IsViableEffectTarget(c))
                    ?? Enemy.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup() && IsViableEffectTarget(c))
                    ?? Enemy.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup())
                    ?? Enemy.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup());

                if (enemyTarget != null)
                {
                    AI.SelectCard(new[] { CardId.Albion, CardId.EcclesiaDarkDragon });
                    AI.SelectNextCard(enemyTarget);
                    AI.SelectOption(0);
                    _fallenVirtuousUsed = true;
                    DecisionTracer.TraceActivate("FallenVirtuousEffect", $"Sending Albion/Ecclesia, destroying enemy card: {enemyTarget.Name}");
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        // Setup & Search Spells
        // ============================================================

        private bool VandalismEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (_vandalismUsed) return false;

            if (GetRemainingCount(CardId.MediusThePure) > 0)
            {
                _vandalismUsed = true;
                DecisionTracer.TraceActivate("VandalismEffect", "Searching Medius the Pure");
                return true;
            }
            return false;
        }

        private bool VarnishEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (_varnishUsed) return false;

            bool hasAcropolis = GetRemainingCount(CardId.Acropolis) > 0 
                || Bot.Graveyard.Any(c => c != null && c.Id == CardId.Acropolis);

            if (hasAcropolis || Bot.HasInSpellZone(CardId.Acropolis))
            {
                _varnishUsed = true;
                DecisionTracer.TraceActivate("VarnishEffect", "Searching or placing Acropolis");
                return true;
            }
            return false;
        }

        private bool AcropolisEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (Bot.HasInSpellZone(CardId.Acropolis)) return false;
                DecisionTracer.TraceActivate("AcropolisEffect", "Placing Acropolis Field Spell");
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && !Card.IsFacedown() && !_acropolisSearchUsed)
            {
                if (IsMain1SearchDeferred()) return false;
                bool hasExpendableDiscard = Bot.Hand.Any(c => c != null && c != Card && IsExpendableSpellTrap(c));
                bool hasAnyDiscard = Bot.Hand.Any(c => c != null && c != Card && (c.IsSpell() || c.IsTrap()) && c.Id != CardId.TripleTacticsTalent && c.Id != CardId.SuperPoly);
                if (hasExpendableDiscard || hasAnyDiscard)
                {
                    int searchId = 0;
                    if (!Bot.HasInMonstersZone(CardId.ArtmageLitera) && GetRemainingCount(CardId.ArtmageLitera) > 0)
                        searchId = CardId.ArtmageLitera;
                    else if (!Bot.HasInMonstersZone(CardId.ArtmagePowerPatron) && GetRemainingCount(CardId.ArtmagePowerPatron) > 0)
                        searchId = CardId.ArtmagePowerPatron;
                    else if (GetRemainingCount(CardId.ArtmageFinmel) > 0)
                        searchId = CardId.ArtmageFinmel;
                    else if (GetRemainingCount(CardId.ArtmageGraflare) > 0)
                        searchId = CardId.ArtmageGraflare;

                    if (searchId != 0)
                    {
                        AI.SelectAnnounceID(searchId);
                        _lastAnnouncedId = searchId;
                        _acropolisSearchUsed = true;
                        DecisionTracer.TraceActivate("AcropolisEffect", $"Searching Artmage monster {searchId} from deck");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool PortalTerminusEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (_terminusUsed) return false;

            bool hasPowerPatronDeck = GetRemainingCount(CardId.ShadowBeastNervedo) > 0 
                || GetRemainingCount(CardId.ShadowMachineZegredo) > 0;

            if (hasPowerPatronDeck)
            {
                _terminusUsed = true;
                DecisionTracer.TraceActivate("PortalTerminusEffect", "Activating Portal Terminus search");
                return true;
            }
            return false;
        }

        private bool PactEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (_pactUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasTarget = GetRemainingCount(CardId.MediusThePure) > 0
                || GetRemainingCount(CardId.ArtmageLitera) > 0;

            if (hasTarget)
            {
                _pactUsed = true;
                DecisionTracer.TraceActivate("PactEffect", "SS Medius or Litera from Deck");
                return true;
            }
            return false;
        }

        private bool MasterworkEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_masterworkUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasArtmage = Bot.Hand.Any(c => c != null && c.IsMonster() && c.HasSetcode(0x1c7))
                || Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1c7));

            int totalMonsters = Bot.Hand.Count(c => c != null && c.IsMonster())
                + Bot.MonsterZone.Count(c => c != null && c.IsFaceup());

            if (hasArtmage && totalMonsters >= 2)
            {
                _masterworkUsed = true;
                DecisionTracer.TraceActivate("MasterworkEffect", "Initiating Fusion Summon");
                return true;
            }
            return false;
        }

        // ============================================================
        // Monster Effects
        // ============================================================

        private bool NervedoBanishEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_nervedoBanishUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.Deck.Count < 3) return false;

            _nervedoBanishUsed = true;
            DecisionTracer.TraceActivate("NervedoBanishEffect", "Banish 3, Summon Nerva");
            return true;
        }

        private bool MediusSummonSearchEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_mediusSummonSearchUsed) return false;

            bool hasPowerPatron = GetRemainingCount(CardId.ShadowBeastNervedo) > 0
                || GetRemainingCount(CardId.Vidrium) > 0;

            if (hasPowerPatron)
            {
                _mediusSummonSearchUsed = true;
                DecisionTracer.TraceActivate("MediusSummonSearchEffect", "Searching Power Patron monster");
                return true;
            }
            return false;
        }

        private bool MediusGYReviveEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_mediusGYSSEused) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasShuffleSource = Bot.Hand.Any(c => c != null && c.IsMonster())
                || Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !IsAceCard(c));

            if (hasShuffleSource)
            {
                _mediusGYSSEused = true;
                DecisionTracer.TraceActivate("MediusGYReviveEffect", "Shuffling monster, SS Medius from GY");
                return true;
            }
            return false;
        }

        private bool PowerPatronFusionEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_artmagePowerPatronSSEused) return false;
            if (!Duel.IsMainPhase()) return false;
            if (IsSpecialSummonBlocked()) return false;

            int totalMonsters = Bot.Hand.Count(c => c != null && c.IsMonster())
                + Bot.MonsterZone.Count(c => c != null && c.IsFaceup());
            if (totalMonsters >= 2)
            {
                _artmagePowerPatronSSEused = true;
                DecisionTracer.TraceActivate("PowerPatronFusionEffect", "Quick Fusion Summon on field");
                return true;
            }
            return false;
        }

        private bool PowerPatronGraveEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            if (Card.Location != CardLocation.Grave) return false;
            if (_artmagePowerPatronSearchUsed) return false;

            var graveNames = Bot.Graveyard.Where(c => c != null).Select(c => c.Id).Distinct().ToList();
            bool hasTarget = GetRemainingCount(CardId.Pact) > 0 && !graveNames.Contains(CardId.Pact)
                || GetRemainingCount(CardId.Varnish) > 0 && !graveNames.Contains(CardId.Varnish)
                || GetRemainingCount(CardId.Vandalism) > 0 && !graveNames.Contains(CardId.Vandalism)
                || GetRemainingCount(CardId.Masterwork) > 0 && !graveNames.Contains(CardId.Masterwork)
                || GetRemainingCount(CardId.ImpastoRecapture) > 0 && !graveNames.Contains(CardId.ImpastoRecapture);

            if (hasTarget)
            {
                _artmagePowerPatronSearchUsed = true;
                DecisionTracer.TraceActivate("PowerPatronGraveEffect", "Searching Artmage S/T from deck");
                return true;
            }
            return false;
        }

        private bool FinmelSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_finmelSSEused) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasArtmage = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1c7));
            if (hasArtmage)
            {
                _finmelSSEused = true;
                DecisionTracer.TraceActivate("FinmelSSEffect", "SS Finmel from hand");
                return true;
            }
            return false;
        }

        private bool FinmelNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_finmelTributeUsed) return false;
            if (!Duel.IsMainPhase()) return false;

            var races = Bot.MonsterZone.Where(c => c != null && c.IsFaceup()).Select(c => c.Race).Distinct().ToList();
            bool has3Races = races.Count >= 3;
            bool hasEnemyMonster = Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled());

            if (has3Races && hasEnemyMonster)
            {
                _finmelTributeUsed = true;
                DecisionTracer.TraceActivate("FinmelNegateEffect", "Negating opponent board using Finmel");
                return true;
            }
            return false;
        }

        private bool GraflareSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_graflareSSEused) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasArtmage = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1c7));
            if (hasArtmage)
            {
                _graflareSSEused = true;
                DecisionTracer.TraceActivate("GraflareSSEffect", "SS Graflare from hand");
                return true;
            }
            return false;
        }

        private bool GraflareDestroyEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_graflareTributeUsed) return false;

            var races = Bot.MonsterZone.Where(c => c != null && c.IsFaceup()).Select(c => c.Race).Distinct().ToList();
            bool isQuick = races.Count >= 3;
            if (!isQuick && Duel.Player != 0) return false;

            var target = Enemy.SpellZone.FirstOrDefault(c => c != null && IsViableEffectTarget(c));
            if (target != null)
            {
                AI.SelectCard(target);
                _graflareTributeUsed = true;
                DecisionTracer.TraceActivate("GraflareDestroyEffect", $"Pops enemy S/T: {target.Name}");
                return true;
            }
            return false;
        }

        private bool LiteraSSEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (_literaSSUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            bool hasArtmage = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1c7))
                || Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.HasSetcode(0x1c7));

            if (hasArtmage)
            {
                _literaSSUsed = true;
                DecisionTracer.TraceActivate("LiteraSSEffect", "SS Litera from hand");
                return true;
            }
            return false;
        }

        private bool VidriumAlternateSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_vidriumSSEused) return false;

            var allCards = Bot.MonsterZone.Where(c => c != null && c.IsFaceup()).Concat(Bot.Graveyard.Where(c => c != null)).ToList();
            bool hasFusion = allCards.Any(c => c.HasType(CardType.Fusion));
            bool hasSynchro = allCards.Any(c => c.HasType(CardType.Synchro));
            bool hasXyz = allCards.Any(c => c.HasType(CardType.Xyz));

            if (hasFusion && hasSynchro && hasXyz)
            {
                _vidriumSSEused = true;
                DecisionTracer.TraceActivate("VidriumAlternateSummon", "Special summon Vidrium by shuffling F/S/X materials back");
                return true;
            }
            return false;
        }

        private bool VidriumTriggerEffects()
        {
            if (Card.Location == CardLocation.Extra)
            {
                if (_vidriumBanishUsed) return false;
                _vidriumBanishUsed = true;
                DecisionTracer.TraceActivate("VidriumTriggerEffects", "Vidrium sent to Extra, searching Power Patron");
                return true;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                DecisionTracer.TraceActivate("VidriumTriggerEffects", "Vidrium summoned, banishing GYs");
                return true;
            }

            return false;
        }

        private bool ZegredoPendulumEffect()
        {
            if (Card.Location != CardLocation.PendulumZone) return false;
            if (_zegredoPendulumUsed) return false;

            bool hasTarget = Bot.Hand.Any(c => c != null && c != Card && c.IsMonster() && (c.HasSetcode(0x1c6) || c.HasSetcode(0x1cb)))
                || Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !IsAceCard(c) && (c.HasSetcode(0x1c6) || c.HasSetcode(0x1cb)));

            bool hasEnemyCard = Enemy.GetFieldCount() > 0;

            if (hasTarget && hasEnemyCard)
            {
                _zegredoPendulumUsed = true;
                DecisionTracer.TraceActivate("ZegredoPendulumEffect", "Activating Zegredo Pendulum Zone destruction");
                return true;
            }
            return false;
        }

        private bool DiactorusPosEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (_diactorusSearchUsed) return false;

            var enemyTarget = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && c.IsAttack() && !c.IsShouldNotBeTarget())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            if (enemyTarget != null)
            {
                _diactorusSearchUsed = true;
                AI.SelectCard(enemyTarget);
                DecisionTracer.TraceActivate("DiactorusPosEffect", $"Switching enemy {enemyTarget.Name} to DEF");
                return true;
            }

            var ownTarget = Bot.GetMonsters()
                .FirstOrDefault(c => c != null && c.IsFaceup() && c.IsDefense() && c.Attack > c.Defense && !IsAceCard(c));
            if (ownTarget != null)
            {
                _diactorusSearchUsed = true;
                AI.SelectCard(ownTarget);
                DecisionTracer.TraceActivate("DiactorusPosEffect", $"Switching own {ownTarget.Name} to ATK");
                return true;
            }
            return false;
        }

        private bool DiactorusNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (Duel.LastChainPlayer != 1) return false;
            if (Util.GetLastChainCard()?.Controller != 1) return false;

            var races = Bot.MonsterZone.Where(c => c != null && c.IsFaceup()).Select(c => c.Race).Distinct().ToList();
            if (races.Count < 3) return false;

            DecisionTracer.TraceActivate("DiactorusNegateEffect", "Negating opponent activation on field with Diactorus");
            return true;
        }

        private bool DiactorusDestroyedEffect()
        {
            if (Card.Location != CardLocation.Grave && Card.Location != CardLocation.Removed) return false;

            bool hasMedius = Bot.Hand.Any(c => c != null && c.Id == CardId.MediusThePure)
                || GetRemainingCount(CardId.MediusThePure) > 0
                || Bot.Banished.Any(c => c != null && c.Id == CardId.MediusThePure);

            if (hasMedius)
            {
                DecisionTracer.TraceActivate("DiactorusDestroyedEffect", "Diactorus destroyed, summoning Medius");
                return true;
            }
            return false;
        }

        private bool NonFinitoAlternateSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (_nonFinitoLevelUsed) return false;

            bool hasExpendableST = Bot.Hand.Any(c => c != null && IsExpendableSpellTrap(c));
            bool hasExpendableMonster = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Level >= 7 && c.HasSetcode(0x1c7) && !IsAceCard(c)
                && c.Id != CardId.ShadowBeastNervedo && c.Id != CardId.ShadowMachineZegredo && c.Id != CardId.Vidrium);

            if (hasExpendableST && hasExpendableMonster)
            {
                _nonFinitoLevelUsed = true;
                DecisionTracer.TraceActivate("NonFinitoAlternateSummon", "SS Non-Finito from Extra by alternate procedure");
                return true;
            }
            return false;
        }

        private bool NonFinitoSetSpellTrap()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;

            DecisionTracer.TraceActivate("NonFinitoSetSpellTrap", "Non-Finito setting Artmage Spell/Trap from Deck");
            return true;
        }

        private bool NonFinitoQuickFusion()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Card.IsDisabled()) return false;
            if (Duel.Player == 0) return false;
            if (IsSpecialSummonBlocked()) return false;

            int fieldCount = Bot.MonsterZone.Count(c => c != null && c.IsFaceup());
            if (fieldCount >= 2)
            {
                DecisionTracer.TraceActivate("NonFinitoQuickFusion", "Quick Fusion during opponent's turn");
                return true;
            }
            return false;
        }

        // ============================================================
        // Normal Summon Conditions
        // ============================================================

        private bool ShouldSummonMedius()
        {
            if (GetRemainingCount(CardId.ShadowBeastNervedo) > 0)
            {
                DecisionTracer.TraceActivate("ShouldSummonMedius", "Summoning Medius to search Nervedo");
                return true;
            }
            return false;
        }

        // ============================================================
        // Extra Deck Summons
        // ============================================================

        private bool SPLittleKnightSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.ArtmagePowerPatron)) return false;

            int nonAceCount = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            if (nonAceCount >= 2 && Enemy.GetFieldCount() > 0)
            {
                DecisionTracer.TraceActivate("SPLittleKnightSummon", "Link summoning S:P Little Knight");
                return true;
            }
            return false;
        }

        private bool CrossSheepSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.ArtmagePowerPatron)) return false;

            int nonAceCount = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            if (nonAceCount >= 2)
            {
                DecisionTracer.TraceActivate("CrossSheepSummon", "Link summoning Cross-Sheep");
                return true;
            }
            return false;
        }

        // ============================================================
        // Spell/Trap Setup & Activations
        // ============================================================

        private bool ImpastoRecaptureEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (Card.IsDisabled()) return false;
            if (_recaptureUsed) return false;
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (Duel.LastChainPlayer != 1) return false;

            bool hasFusion = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Fusion));

            if (hasFusion)
            {
                _recaptureUsed = true;
                DecisionTracer.TraceActivate("ImpastoRecaptureEffect", "Negating opponent effect with Impasto Recapture");
                return true;
            }
            return false;
        }
    }
}
