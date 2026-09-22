using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("ArtMage", "ArtMage")]
    public class ArtMageExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Archetype Monsters
            public const int MediusThePure = 97556336;
            public const int ShadowBeastNervedo = 17473466;
            public const int ArtmagePowerPatron = 23829452;
            public const int ArtmageFinmel = 34541940;
            public const int ArtmageGraflare = 60946049;
            public const int ArtmageLitera = 97434754;

            // Spells & Traps
            public const int Acropolis = 74733322;
            public const int Varnish = 74011784;
            public const int Vandalism = 1122030;
            public const int Masterwork = 37517035;
            public const int Pact = 23599634;
            public const int ImpastoRecapture = 44654994;

            // Extra Deck Fusions
            public const int Nerva = 53589300;
            public const int ArtmageDiactorus = 27184601;
            public const int ArtmageNonFinito = 74631897;

            // Super Poly & Extra Deck Fusions
            public const int MudragonOfTheSwamp = 54757758;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int PredaplantDragostapelia = 69946549;
            public const int EarthGolemIgnister = 62111090;
            public const int StarvingVenomFusionDragon = 41209827;

            // Link Monsters
            public const int SPLittleKnight = 29301450;
            public const int CrossSheep = 50277355;
            public const int KnightmareCerberus = 75452921;
            public const int AccesscodeTalker = 86066372;

            // Handtraps & Staples
            public const int AshBlossom = 14558127;
            public const int InfiniteImpermanence = 10045474;
            public const int SuperPoly = 48130397;
            public const int TripleTacticsTalent = 25311006;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;
        }

        private const int ARTMAGE_SETCODE = 0x1c7;
        private const int POWER_PATRON_SETCODE = 0x1c6;

        // OCGCore Hint Message IDs (audited)
        private const long HINT_SELECT_RELEASE = 500;
        private const long HINT_SELECT_DISCARD = 501;
        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_REMOVE = 503;
        private const long HINT_SELECT_TOGRAVE = 504;
        private const long HINT_SELECT_RTOHAND = 505;
        private const long HINT_SELECT_TOHAND = 506;
        private const long HINT_SELECT_TODECK = 507;
        private const long HINT_SELECT_SUMMON = 508;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_SET = 510;
        private const long HINT_SELECT_FMATERIAL = 511;
        private const long HINT_SELECT_LMATERIAL = 533;
        private const long HINT_SELECT_TARGET = 551;
        private const long HINT_SELECT_TOFIELD = 527;

        private static readonly int[] BossMonsters = {
            CardId.Nerva,
            CardId.ArtmageDiactorus,
            CardId.ArtmageNonFinito,
            CardId.PredaplantDragostapelia,
            CardId.StarvingVenomFusionDragon,
            CardId.SPLittleKnight,
            CardId.AccesscodeTalker
        };

        // Per-turn activation tracking
        private bool _mediusSummonUsed;
        private bool _mediusGYReviveUsed;
        private bool _nervedoBanishUsed;
        private bool _nervedoExtraTriggerUsed;
        private bool _nervedoPendulumNegateUsed;
        private bool _powerPatronFusionUsed;
        private bool _powerPatronGYSearchUsed;
        private bool _finmelHandSSUsed;
        private bool _finmelNegateUsed;
        private bool _graflareHandSSUsed;
        private bool _graflareDestroyUsed;
        private bool _literaHandSSUsed;
        private bool _literaQuickSSUsed;
        private bool _acropolisFieldSearchUsed;
        private bool _varnishUsed;
        private bool _vandalismUsed;
        private bool _masterworkFusionUsed;
        private bool _masterworkGYShuffleUsed;
        private bool _pactUsed;
        private bool _impastoUsed;
        private bool _nonFinitoSetUsed;
        private bool _nonFinitoQuickFusionUsed;
        private bool _diactorusNegateUsed;

        private int _lastAnnouncedId;

        public ArtMageExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            ResourcePlan.RegisterAceCards(BossMonsters);
            HeuristicGuard.RegisterAceCards(BossMonsters);

            // ── Combo Router ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Medius-Nerva-Finmel-Setup",
                RequiredCards = new List<int> { CardId.MediusThePure },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Summon, Description = "Normal Summon Medius -> SS Nervedo from Deck" },
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Activate, Description = "Medius activates (Opt 1) -> SS Nervedo" },
                    new() { CardId = CardId.ShadowBeastNervedo, ActionType = ExecutorType.Activate, Description = "Nervedo banishes 3 face-down -> SS Nerva from Extra" },
                    new() { CardId = CardId.ShadowBeastNervedo, ActionType = ExecutorType.Activate, Description = "Nervedo Extra trigger -> SS Finmel from Deck" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Vandalism-Medius-Starter",
                RequiredCards = new List<int> { CardId.Vandalism },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Vandalism, ActionType = ExecutorType.Activate, Description = "Activate Vandalism -> Search Medius" },
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Summon, Description = "Normal Summon Medius" }
                },
                EndBoardScore = 92
            });

            BaitPlanner.RegisterComboStarters(CardId.Vandalism, CardId.Varnish, CardId.MediusThePure, CardId.Acropolis);
            BaitPlanner.RegisterBaitCards(CardId.Vandalism, CardId.Acropolis, CardId.TripleTacticsTalent);
            ChainAdvisor.RegisterHighValueTargets(CardId.MediusThePure, CardId.ShadowBeastNervedo, CardId.Nerva, CardId.ArtmageDiactorus);

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTOR PIPELINE
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Negates, Blanket Effects & Board Wipes ──
            // Nerva: When our Artmage monster effect is activated -> wipes ALL opponent cards!
            AddExecutor(ExecutorType.Activate, CardId.Nerva, NervaBoardWipeEffect);
            // Diactorus: Omni-Negate any opponent card/effect on field (requires 3+ Types)
            AddExecutor(ExecutorType.Activate, CardId.ArtmageDiactorus, DiactorusNegateEffect);
            // Impasto Recapture: Counter Trap (can activate turn set!)
            AddExecutor(ExecutorType.Activate, CardId.ImpastoRecapture, ImpastoRecaptureEffect);
            // Finmel: Blanket Monster Negate + ATK halve (requires 3+ Types)
            AddExecutor(ExecutorType.Activate, CardId.ArtmageFinmel, FinmelNegateEffect);
            // Nervedo Pendulum Negate (when in Pendulum Zone)
            AddExecutor(ExecutorType.Activate, CardId.ShadowBeastNervedo, NervedoPendulumNegateEffect);

            // Handtraps & Interruptions
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // S:P Little Knight Quick Effect
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightQuickEffect);
            // Litera: Opponent Main Phase Quick SS & bounce
            AddExecutor(ExecutorType.Activate, CardId.ArtmageLitera, LiteraQuickSSEffect);
            // Non-Finito: Opponent turn Quick Fusion
            AddExecutor(ExecutorType.Activate, CardId.ArtmageNonFinito, NonFinitoQuickFusionEffect);
            // Artmage Power Patron: Main Phase Quick Fusion
            AddExecutor(ExecutorType.Activate, CardId.ArtmagePowerPatron, PowerPatronQuickFusionEffect);

            // ── Tier 0.5: Board Breakers ──
            AddExecutor(ExecutorType.Activate, CardId.SuperPoly, SuperPolyEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);

            // ── Tier 1: Search Spells & Field Setup ──
            AddExecutor(ExecutorType.Activate, CardId.Vandalism, VandalismEffect);
            AddExecutor(ExecutorType.Activate, CardId.Varnish, VarnishEffect);
            AddExecutor(ExecutorType.Activate, CardId.Acropolis, AcropolisEffect);

            // ── Tier 2: Monster Starters & Ignition Summons ──
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure, MediusNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusOnSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.ShadowBeastNervedo, NervedoFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.ShadowBeastNervedo, NervedoExtraTriggerEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmagePowerPatron, PowerPatronGYSearchEffect);

            // ── Tier 3: Hand Extenders & Inherent Special Summons ──
            AddExecutor(ExecutorType.Activate, CardId.ArtmageFinmel, FinmelHandSSEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageGraflare, GraflareHandSSEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageLitera, LiteraHandSSEffect);
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusGYReviveEffect);

            // In-archetype Spell/Trap activations
            AddExecutor(ExecutorType.Activate, CardId.Masterwork, MasterworkFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.Masterwork, MasterworkGYShuffleEffect);
            AddExecutor(ExecutorType.Activate, CardId.Pact, PactEffect);
            AddExecutor(ExecutorType.Activate, CardId.Pact, PactGYPopEffect);

            // Secondary Monster Triggers
            AddExecutor(ExecutorType.Activate, CardId.ArtmageGraflare, GraflareDestroyEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageDiactorus, DiactorusPosEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageDiactorus, DiactorusDestroyedEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageNonFinito, NonFinitoOnSummonSetEffect);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.ArtmageLitera, LiteraNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.ShadowBeastNervedo, NervedoNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.ArtmagePowerPatron, PowerPatronNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.ArtmageFinmel, FinmelTributeSummon);
            AddExecutor(ExecutorType.Summon, CardId.ArtmageGraflare, GraflareTributeSummon);

            // ── Tier 4: Extra Deck Summons ──
            AddExecutor(ExecutorType.SpSummon, CardId.ArtmageNonFinito, NonFinitoAlternateSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrossSheep, CrossSheepSummon);
            AddExecutor(ExecutorType.Activate, CardId.CrossSheep, CrossSheepTriggerEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightOnSummonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareCerberus, KnightmareCerberusSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareCerberus, KnightmareCerberusEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeTalkerSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeTalkerEffect);

            // ── Tier 5: Backrow Setting ──
            // Impasto Recapture can be activated the turn it was set! Always set immediately
            AddExecutor(ExecutorType.SpellSet, CardId.ImpastoRecapture);
            AddExecutor(ExecutorType.SpellSet, CardId.Pact, SpellSetInMain2);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SpellSetInMain2);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetInMain2);
            AddExecutor(ExecutorType.SpellSet, CardId.CrossoutDesignator, SpellSetInMain2);

            // ── Tier 6: Reposition ──
            AddExecutor(ExecutorType.Repos, MonsterReposOverride);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _mediusSummonUsed = false;
            _mediusGYReviveUsed = false;
            _nervedoBanishUsed = false;
            _nervedoExtraTriggerUsed = false;
            _nervedoPendulumNegateUsed = false;
            _powerPatronFusionUsed = false;
            _powerPatronGYSearchUsed = false;
            _finmelHandSSUsed = false;
            _finmelNegateUsed = false;
            _graflareHandSSUsed = false;
            _graflareDestroyUsed = false;
            _literaHandSSUsed = false;
            _literaQuickSSUsed = false;
            _acropolisFieldSearchUsed = false;
            _varnishUsed = false;
            _vandalismUsed = false;
            _masterworkFusionUsed = false;
            _masterworkGYShuffleUsed = false;
            _pactUsed = false;
            _impastoUsed = false;
            _nonFinitoSetUsed = false;
            _nonFinitoQuickFusionUsed = false;
            _diactorusNegateUsed = false;
            _lastAnnouncedId = 0;
        }

        private bool SpellSetInMain2()
        {
            return Duel.Phase == DuelPhase.Main2 || (Duel.Phase == DuelPhase.Main1 && Duel.Turn == 1);
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return BossMonsters.Contains(card.Id) || base.IsAceCard(card);
        }

        private int GetDistinctRaceCount()
        {
            return Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).Select(c => c.Race).Distinct().Count();
        }

        // ═══════════════════════════════════════════════════════════════
        //  CORE CALLBACK HANDLERS (Audited & Critical)
        // ═══════════════════════════════════════════════════════════════

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return base.OnSelectOption(options);

            // 1. Medius the Pure: aux.ToHandOrElse
            // Option 0 = Add to Hand, Option 1 = Special Summon it
            if (LastChainCard != null && LastChainCard.Id == CardId.MediusThePure && options.Count >= 2)
            {
                // If monster zone has room, summon Nervedo to field to immediately pop and summon Nerva!
                if (Bot.GetMonsterCount() < 5)
                {
                    return 1; // Special Summon to field!
                }
                return 0; // Add to hand
            }

            // 2. Varnish: aux.ToHandOrElse (when Acropolis already faceup)
            // Option 0 = Add Artmage card to Hand, Option 1 = Place Acropolis
            if (LastChainCard != null && LastChainCard.Id == CardId.Varnish && options.Count >= 2)
            {
                if (Bot.HasInSpellZone(CardId.Acropolis))
                {
                    return 0; // Add to Hand
                }
            }

            // 3. Triple Tactics Talent
            if (LastChainCard != null && LastChainCard.Id == CardId.TripleTacticsTalent && options.Count >= 3)
            {
                if (Enemy.GetMonsterCount() > 0 && Bot.GetMonsterCount() < 5)
                    return 1; // Take control of opponent monster
                return 0; // Draw 2
            }

            return base.OnSelectOption(options);
        }

        public override bool OnSelectYesNo(long desc)
        {
            // Finmel: Draw 1 card? -> Always YES
            // Graflare: Set 1 Artmage Spell from Deck? -> Always YES
            // Litera: Add 1 Artmage card from GY to hand? -> Always YES
            // Vandalism: Add Medius from Deck to hand? -> Always YES
            // Varnish: Special Summon Medius on attack negation? -> YES if Medius in hand
            // Impasto: Bounce all opponent Spells/Traps to hand? -> YES if opponent has Spells/Traps
            // Nervedo in P-Zone: Negate opponent monster effect? -> Always YES
            // Vandalism: Send Vandalism to GY instead of Acropolis being destroyed? -> Always YES
            return true;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.Nerva || cardId == CardId.ArtmageDiactorus || cardId == CardId.ArtmageFinmel ||
                cardId == CardId.AccesscodeTalker || cardId == CardId.SPLittleKnight || cardId == CardId.StarvingVenomFusionDragon)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            if (cardId == CardId.ShadowBeastNervedo || cardId == CardId.ArtmageLitera || cardId == CardId.CrossSheep)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Acropolis announced search target
            if (hint == HINT_SELECT_TOHAND && _lastAnnouncedId != 0)
            {
                var target = cards.FirstOrDefault(c => c.Id == _lastAnnouncedId);
                if (target != null)
                {
                    _lastAnnouncedId = 0;
                    return new[] { target };
                }
            }

            // Hint 509: Special Summon
            if (hint == HINT_SELECT_SPSUMMON)
            {
                // Extra Deck Bosses
                var extraPreferred = new[] { CardId.Nerva, CardId.ArtmageDiactorus, CardId.ArtmageNonFinito, CardId.SPLittleKnight, CardId.CrossSheep };
                foreach (int id in extraPreferred)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                    if (match != null) return new[] { match };
                }

                // Deck Special Summons (e.g. Medius -> Nervedo, Nervedo Extra trigger -> Finmel/Graflare)
                var deckPreferred = new[] {
                    CardId.ShadowBeastNervedo,
                    CardId.ArtmageFinmel,
                    CardId.ArtmageGraflare,
                    CardId.ArtmagePowerPatron,
                    CardId.ArtmageLitera,
                    CardId.MediusThePure
                };
                foreach (int id in deckPreferred)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                    if (match != null) return new[] { match };
                }
            }

            // Hint 506: Add to hand / Search
            if (hint == HINT_SELECT_TOHAND)
            {
                // If searching from Deck
                if (cards.All(c => c.Location == CardLocation.Deck))
                {
                    var preferred = new[] {
                        CardId.MediusThePure,
                        CardId.ImpastoRecapture,
                        CardId.Acropolis,
                        CardId.Varnish,
                        CardId.Vandalism,
                        CardId.Masterwork,
                        CardId.ArtmageFinmel,
                        CardId.Pact
                    };
                    foreach (int id in preferred)
                    {
                        var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                        if (match != null) return new[] { match };
                    }
                }

                // If adding from GY (Litera)
                if (cards.All(c => c.Location == CardLocation.Grave))
                {
                    var gyPreferred = new[] {
                        CardId.ArtmageFinmel,
                        CardId.Masterwork,
                        CardId.ImpastoRecapture,
                        CardId.Varnish,
                        CardId.MediusThePure,
                        CardId.ArtmageGraflare
                    };
                    foreach (int id in gyPreferred)
                    {
                        var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                        if (match != null) return new[] { match };
                    }
                }
            }

            // Hint 510: Set Spell/Trap (Non-Finito / Graflare)
            if (hint == HINT_SELECT_SET)
            {
                var setPreferred = new[] {
                    CardId.ImpastoRecapture, // Can activate turn set!
                    CardId.Masterwork,
                    CardId.Pact,
                    CardId.Varnish,
                    CardId.Vandalism
                };
                foreach (int id in setPreferred)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                    if (match != null) return new[] { match };
                }
            }

            // Hint 511: Fusion Material (Protect Ace monsters!)
            if (hint == HINT_SELECT_FMATERIAL)
            {
                // Prefer opponent cards for Super Poly
                var oppCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (oppCards.Count >= min)
                {
                    return Util.CheckSelectCount(oppCards, cards, min, max);
                }

                // If using our cards, prioritize hand monsters & non-Ace monsters
                var nonAce = cards.Where(c => c != null && c.Controller == 0 && !IsAceCard(c)).OrderBy(c => c.Attack).ToList();
                if (nonAce.Count >= min)
                {
                    return Util.CheckSelectCount(nonAce, cards, min, max);
                }
            }

            // Hint 533: Link Material (Protect Ace monsters!)
            if (hint == HINT_SELECT_LMATERIAL)
            {
                var nonAce = cards.Where(c => c != null && !IsAceCard(c)).OrderBy(c => c.Attack).ToList();
                if (nonAce.Count >= min)
                {
                    return Util.CheckSelectCount(nonAce, cards, min, max);
                }
            }

            // Hint 501: Discard (Acropolis / Super Poly)
            if (hint == HINT_SELECT_DISCARD)
            {
                var safeDiscards = cards.Where(c => c != null && c.Id != CardId.ImpastoRecapture && c.Id != CardId.SuperPoly).ToList();
                var duplicates = safeDiscards.Where(c => Bot.Hand.Count(h => h.Id == c.Id) > 1).ToList();
                if (duplicates.Count >= min)
                {
                    return Util.CheckSelectCount(duplicates, cards, min, max);
                }
                var spellDiscards = safeDiscards.Where(c => c.IsSpell() && c.Id != CardId.Vandalism).ToList();
                if (spellDiscards.Count >= min)
                {
                    return Util.CheckSelectCount(spellDiscards, cards, min, max);
                }
            }

            // Hint 507: Return to Deck (Medius revive / Masterwork GY)
            if (hint == HINT_SELECT_TODECK)
            {
                // If from GY (Masterwork): select 3 different Artmage cards
                if (cards.All(c => c.Location == CardLocation.Grave))
                {
                    var distinct = cards.GroupBy(c => c.Id).Select(g => g.First()).ToList();
                    if (distinct.Count >= min)
                    {
                        return Util.CheckSelectCount(distinct, cards, min, max);
                    }
                }

                // If Medius GY cost: shuffle non-Ace card
                var nonAce = cards.Where(c => c != null && !IsAceCard(c)).OrderBy(c => c.Attack).ToList();
                if (nonAce.Count >= min)
                {
                    return Util.CheckSelectCount(nonAce, cards, min, max);
                }
            }

            // Removal hints: 502 (Destroy), 503 (Remove/Banish), 505 (Bounce)
            if (hint == HINT_SELECT_DESTROY || hint == HINT_SELECT_REMOVE || hint == HINT_SELECT_RTOHAND)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count >= min)
                {
                    var sorted = enemyCards.OrderByDescending(c => c.Attack).ToList();
                    return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 0 & QUICK EFFECTS
        // ═══════════════════════════════════════════════════════════════

        private bool NervaBoardWipeEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            // Opponent must have at least 1 card on field to destroy
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;

            // Nerva triggers when an Artmage monster effect is activated
            if (Duel.LastChainPlayer == 0)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 0 && lastCard.IsMonster() &&
                    (lastCard.HasSetcode(ARTMAGE_SETCODE) || lastCard.Id == CardId.ShadowBeastNervedo))
                {
                    // On opponent's turn: ALWAYS wipe their board!
                    if (Duel.Player == 1) return true;

                    // On our turn: If opponent controls 2+ cards or a boss monster, wipe to clear for OTK!
                    if (Enemy.GetMonsterCount() >= 1 || Enemy.GetSpellCount() >= 2)
                    {
                        // Don't wipe if it's Medius on summon (we want Medius to resolve its summon)
                        if (lastCard.Id == CardId.MediusThePure && !_nervedoBanishUsed) return false;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DiactorusNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_diactorusNegateUsed) return false;

            // Requires 3+ different Monster Types on our field
            if (GetDistinctRaceCount() < 3) return false;

            // Negate opponent card/effect on field
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 &&
                    (lastCard.Location == CardLocation.MonsterZone || lastCard.Location == CardLocation.SpellZone))
                {
                    _diactorusNegateUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool ImpastoRecaptureEffect()
        {
            if (Card.IsDisabled() || _impastoUsed) return false;

            // When opponent activates a monster effect
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster())
                {
                    // Banish 1 Fusion Monster we control until End Phase
                    var fusionTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasType(CardType.Fusion) && c.Id != CardId.Nerva)
                                    ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasType(CardType.Fusion));
                    if (fusionTarget != null)
                    {
                        _impastoUsed = true;
                        AI.SelectCard(fusionTarget);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool FinmelNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_finmelNegateUsed) return false;
            if (!Duel.IsMainPhase()) return false;

            // Requires 3+ different Monster Types
            if (GetDistinctRaceCount() >= 3)
            {
                bool hasUndisabledEnemy = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());
                if (hasUndisabledEnemy)
                {
                    _finmelNegateUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool NervedoPendulumNegateEffect()
        {
            if (Card.Location != CardLocation.PendulumZone || Card.IsDisabled()) return false;
            if (_nervedoPendulumNegateUsed) return false;

            if (Duel.LastChainPlayer == 1)
            {
                _nervedoPendulumNegateUsed = true;
                return true;
            }
            return false;
        }

        private bool AshBlossomEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool CalledByTheGraveEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultCalledByTheGrave();
        }

        private bool CrossoutDesignatorEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            return DefaultCrossoutDesignator();
        }

        private bool SPLittleKnightQuickEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
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

        private bool LiteraQuickSSEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_literaQuickSSUsed) return false;
            if (Duel.Player != 1 || !Duel.IsMainPhase()) return false;

            var target = Bot.Hand.FirstOrDefault(c => c != null && c.HasSetcode(ARTMAGE_SETCODE) && c.Id != CardId.ArtmageLitera)
                      ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.HasSetcode(ARTMAGE_SETCODE) && c.Id != CardId.ArtmageLitera);

            if (target != null)
            {
                _literaQuickSSUsed = true;
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool NonFinitoQuickFusionEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_nonFinitoQuickFusionUsed) return false;
            if (Duel.Player != 1) return false;

            int faceupMonsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c != Card);
            if (faceupMonsters >= 1)
            {
                _nonFinitoQuickFusionUsed = true;
                AI.SelectCard(CardId.ArtmageDiactorus, CardId.Nerva);
                return true;
            }
            return false;
        }

        private bool PowerPatronQuickFusionEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_powerPatronFusionUsed) return false;
            if (!Duel.IsMainPhase()) return false;

            // Can summon Diactorus or Nerva using this card + hand/field
            bool hasMedius = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id == CardId.MediusThePure)
                          || Bot.Hand.Any(c => c != null && c.Id == CardId.MediusThePure);

            if (hasMedius && !Bot.HasInMonstersZone(CardId.ArtmageDiactorus))
            {
                _powerPatronFusionUsed = true;
                AI.SelectCard(CardId.ArtmageDiactorus);
                return true;
            }

            return false;
        }

        private bool SuperPolyEffect()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            if (Bot.Hand.Count(c => c != Card) < 1) return false;

            if (Enemy.GetMonsterCount() >= 2)
            {
                var discard = Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.ArtmageLitera || c.Id == CardId.MediusThePure))
                           ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && !IsAceCard(c) && c.Id != CardId.ImpastoRecapture);
                if (discard != null)
                {
                    AI.SelectCard(discard);
                    return true;
                }
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 1 & SEARCH SPELLS
        // ═══════════════════════════════════════════════════════════════

        private bool VandalismEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_vandalismUsed) return false;
                _vandalismUsed = true;
                AI.SelectCard(CardId.MediusThePure);
                return true;
            }
            return false;
        }

        private bool VarnishEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_varnishUsed) return false;
            _varnishUsed = true;

            if (Bot.HasInSpellZone(CardId.Acropolis))
            {
                AI.SelectCard(CardId.Vandalism, CardId.Masterwork, CardId.ImpastoRecapture, CardId.ArtmageFinmel);
            }
            else
            {
                AI.SelectCard(CardId.Acropolis);
            }
            return true;
        }

        private bool AcropolisEffect()
        {
            // Activate to Field Zone
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (Bot.HasInSpellZone(CardId.Acropolis)) return false;
                return true;
            }

            // On-field search: Discard 1 Spell/Trap -> Announce Artmage monster not on field -> add to hand
            if (Card.Location == CardLocation.SpellZone && !Card.IsFacedown() && !_acropolisFieldSearchUsed)
            {
                var discard = Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.Acropolis || c.Id == CardId.Vandalism || c.Id == CardId.Varnish))
                           ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.IsSpell() || c.IsTrap()) && c.Id != CardId.SuperPoly && c.Id != CardId.ImpastoRecapture);

                if (discard != null)
                {
                    int searchId = 0;
                    if (!Bot.HasInMonstersZone(CardId.ArtmageFinmel) && !Bot.Hand.Any(c => c != null && c.Id == CardId.ArtmageFinmel))
                        searchId = CardId.ArtmageFinmel;
                    else if (!Bot.HasInMonstersZone(CardId.ArtmageGraflare) && !Bot.Hand.Any(c => c != null && c.Id == CardId.ArtmageGraflare))
                        searchId = CardId.ArtmageGraflare;
                    else if (!Bot.HasInMonstersZone(CardId.ArtmagePowerPatron) && !Bot.Hand.Any(c => c != null && c.Id == CardId.ArtmagePowerPatron))
                        searchId = CardId.ArtmagePowerPatron;
                    else
                        searchId = CardId.ArtmageLitera;

                    if (searchId != 0)
                    {
                        AI.SelectCard(discard);
                        AI.SelectAnnounceID(searchId);
                        _lastAnnouncedId = searchId;
                        _acropolisFieldSearchUsed = true;
                        return true;
                    }
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 2 & 3: MONSTERS & SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool MediusNormalSummon()
        {
            return true;
        }

        private bool MediusOnSummonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_mediusSummonUsed) return false;
            _mediusSummonUsed = true;

            // Special Summon Nervedo directly from Deck!
            AI.SelectCard(CardId.ShadowBeastNervedo);
            return true;
        }

        private bool MediusGYReviveEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_mediusGYReviveUsed) return false;

            var shuffleTarget = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.Id != CardId.MediusThePure && !IsAceCard(c))
                             ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Id != CardId.Nerva);

            if (shuffleTarget != null)
            {
                _mediusGYReviveUsed = true;
                AI.SelectCard(shuffleTarget);
                return true;
            }
            return false;
        }

        private bool NervedoFieldEffect()
        {
            // Monster effect on field: banish 3 face-down -> SS Nerva from Extra Deck!
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_nervedoBanishUsed) return false;
                if (Bot.Deck.Count < 3) return false;

                _nervedoBanishUsed = true;
                AI.SelectCard(CardId.Nerva);
                return true;
            }
            return false;
        }

        private bool NervedoExtraTriggerEffect()
        {
            // Trigger when sent to Extra Deck face-up: SS Artmage monster from Deck!
            if (Card.Location == CardLocation.Extra)
            {
                if (_nervedoExtraTriggerUsed) return false;
                _nervedoExtraTriggerUsed = true;
                AI.SelectCard(CardId.ArtmageFinmel, CardId.ArtmageGraflare, CardId.ArtmagePowerPatron, CardId.ArtmageLitera);
                return true;
            }
            return false;
        }

        private bool PowerPatronGYSearchEffect()
        {
            // When sent to GY from hand or field: Add 1 Artmage S/T with different name from GY
            if (Card.Location != CardLocation.Grave) return false;
            if (_powerPatronGYSearchUsed) return false;
            _powerPatronGYSearchUsed = true;

            // Pick an Artmage S/T not in GY
            var gyIds = Bot.Graveyard.Where(c => c != null).Select(c => c.Id).ToHashSet();
            int[] targets = { CardId.ImpastoRecapture, CardId.Masterwork, CardId.Pact, CardId.Varnish, CardId.Vandalism, CardId.Acropolis };
            foreach (int tid in targets)
            {
                if (!gyIds.Contains(tid))
                {
                    AI.SelectCard(tid);
                    return true;
                }
            }

            AI.SelectCard(CardId.ImpastoRecapture, CardId.Masterwork);
            return true;
        }

        private bool FinmelHandSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_finmelHandSSUsed) return false;

            bool hasArtmage = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.HasSetcode(ARTMAGE_SETCODE) || c.Id == CardId.MediusThePure));
            if (hasArtmage)
            {
                _finmelHandSSUsed = true;
                return true;
            }
            return false;
        }

        private bool GraflareHandSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_graflareHandSSUsed) return false;

            bool hasArtmage = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.HasSetcode(ARTMAGE_SETCODE) || c.Id == CardId.MediusThePure));
            if (hasArtmage)
            {
                _graflareHandSSUsed = true;
                AI.SelectCard(CardId.Masterwork, CardId.Varnish, CardId.Vandalism);
                return true;
            }
            return false;
        }

        private bool LiteraHandSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_literaHandSSUsed) return false;

            bool hasArtmage = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.HasSetcode(ARTMAGE_SETCODE) || c.Id == CardId.MediusThePure));
            if (hasArtmage)
            {
                _literaHandSSUsed = true;
                var gyTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.HasSetcode(ARTMAGE_SETCODE));
                if (gyTarget != null) AI.SelectCard(gyTarget);
                return true;
            }
            return false;
        }

        private bool MasterworkFusionEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (_masterworkFusionUsed) return false;

                bool hasArtmage = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(ARTMAGE_SETCODE))
                               || Bot.Hand.Any(c => c != null && c.HasSetcode(ARTMAGE_SETCODE));
                int totalMats = Bot.Hand.Count(c => c != null && c.IsMonster())
                              + Bot.GetMonsters().Count(c => c != null && c.IsFaceup());

                if (hasArtmage && totalMats >= 2)
                {
                    _masterworkFusionUsed = true;
                    AI.SelectCard(CardId.ArtmageDiactorus, CardId.ArtmageNonFinito, CardId.Nerva);
                    return true;
                }
            }
            return false;
        }

        private bool MasterworkGYShuffleEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (_masterworkGYShuffleUsed) return false;
                if (!Duel.IsMainPhase() || Duel.Player != 0) return false;

                var gyCards = Bot.Graveyard.Where(c => c != null && c.HasSetcode(ARTMAGE_SETCODE) && c != Card)
                    .GroupBy(c => c.Id).Select(g => g.First()).Take(3).ToList();
                if (gyCards.Count >= 3)
                {
                    _masterworkGYShuffleUsed = true;
                    AI.SelectCard(gyCards);
                    return true;
                }
            }
            return false;
        }

        private bool PactEffect()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                if (_pactUsed) return false;
                _pactUsed = true;
                AI.SelectCard(CardId.MediusThePure, CardId.ArtmageFinmel, CardId.ArtmageGraflare, CardId.ArtmageLitera);
                return true;
            }
            return false;
        }

        private bool PactGYPopEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var bounceTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasSetcode(ARTMAGE_SETCODE) && !IsAceCard(c));
                var oppTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(c => c != null);

                if (bounceTarget != null && oppTarget != null)
                {
                    AI.SelectCard(bounceTarget);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool GraflareDestroyEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_graflareDestroyUsed) return false;

            var target = Enemy.GetSpells().FirstOrDefault(c => c != null);
            if (target != null)
            {
                _graflareDestroyUsed = true;
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool DiactorusPosEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsAttack() && c.Defense < c.Attack && c.Defense < 2800);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool DiactorusDestroyedEffect()
        {
            AI.SelectCard(CardId.MediusThePure);
            return true;
        }

        private bool NonFinitoOnSummonSetEffect()
        {
            if (_nonFinitoSetUsed) return false;
            _nonFinitoSetUsed = true;
            AI.SelectCard(CardId.ImpastoRecapture, CardId.Masterwork, CardId.Pact);
            return true;
        }

        private bool LiteraNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool NervedoNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool PowerPatronNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool FinmelTributeSummon()
        {
            if (Bot.GetMonsterCount() == 0) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Attack < 1500);
        }

        private bool GraflareTributeSummon()
        {
            if (Bot.GetMonsterCount() == 0) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Attack < 1500);
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 4: EXTRA DECK SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool NonFinitoAlternateSummon()
        {
            if (Bot.HasInMonstersZone(CardId.ArtmageNonFinito)) return false;

            bool hasDiscard = Bot.Hand.Any(c => c != null && (c.IsSpell() || c.IsTrap()) && c.Id != CardId.SuperPoly && c.Id != CardId.ImpastoRecapture);
            var tribute = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasSetcode(ARTMAGE_SETCODE) && c.Level >= 7 && !IsAceCard(c))
                       ?? Bot.Hand.FirstOrDefault(c => c != null && c.HasSetcode(ARTMAGE_SETCODE) && c.Level >= 7);

            return hasDiscard && tribute != null;
        }

        private bool CrossSheepSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 2 && !Bot.HasInMonstersZone(CardId.CrossSheep);
        }

        private bool CrossSheepTriggerEffect()
        {
            // Revive Level 4 or lower monster (Medius the Pure!)
            AI.SelectCard(CardId.MediusThePure, CardId.ArtmageLitera, CardId.ArtmagePowerPatron);
            return true;
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

        private bool KnightmareCerberusSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            bool oppHasSSMonster = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup());
            return mats >= 2 && oppHasSSMonster;
        }

        private bool KnightmareCerberusEffect()
        {
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

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
        //  REPOSITION & COMBAT
        // ═══════════════════════════════════════════════════════════════

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

        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0) return null;

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

            return base.OnBattle(attackers, defenders);
        }
    }
}
