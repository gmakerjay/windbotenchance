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
        public class CardId
        {
            // Main Deck Archetype Cards
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

            // Extra Deck
            public const int Nerva = 53589300;
            public const int ArtmageDiactorus = 27184601;
            public const int ArtmageNonFinito = 74631897;

            // Super Poly & Extra Deck Staples
            public const int MudragonOfTheSwamp = 54757758;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int PredaplantDragostapelia = 69946549;
            public const int EarthGolemIgnister = 62111090;
            public const int StarvingVenomFusionDragon = 41209827;
            public const int SPLittleKnight = 29301450;
            public const int CrossSheep = 50277355;
            public const int KnightmarePhoenix = 75452921;
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

        private const long HINT_SELECT_DESTROY = 502;
        private const long HINT_SELECT_REMOVE = 503;
        private const long HINT_SELECT_TOGRAVE = 504;
        private const long HINT_SELECT_TOHAND = 506;
        private const long HINT_SELECT_TODECK = 507;
        private const long HINT_SELECT_SPSUMMON = 509;
        private const long HINT_SELECT_DISCARD = 501;

        private static readonly int[] BossMonsters = {
            CardId.Nerva,
            CardId.ArtmageDiactorus,
            CardId.ArtmageNonFinito,
            CardId.PredaplantDragostapelia,
            CardId.SPLittleKnight,
            CardId.AccesscodeTalker
        };

        private bool _nervedoBanishUsed;
        private bool _nervedoPendulumNegateUsed;
        private bool _mediusSummonSearchUsed;
        private bool _mediusGYSSEused;
        private bool _finmelSSEused;
        private bool _finmelNegateUsed;
        private bool _graflareSSEused;
        private bool _graflareDestroyUsed;
        private bool _literaSSUsed;
        private bool _literaQuickSSUsed;
        private bool _vandalismUsed;
        private bool _varnishUsed;
        private bool _acropolisSearchUsed;
        private bool _masterworkUsed;
        private bool _pactUsed;
        private bool _impastoUsed;
        private bool _nonFinitoSetUsed;
        private bool _nonFinitoFusionUsed;
        private bool _powerPatronDiscardUsed;

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
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Activate, Description = "Medius activates -> SS Nervedo" },
                    new() { CardId = CardId.ShadowBeastNervedo, ActionType = ExecutorType.Activate, Description = "Nervedo banishes 3 face-down -> SS Nerva from Extra" },
                    new() { CardId = CardId.ShadowBeastNervedo, ActionType = ExecutorType.Activate, Description = "Nervedo Extra trigger -> SS Finmel from Deck" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Vandalism-Starter",
                RequiredCards = new List<int> { CardId.Vandalism },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Vandalism, ActionType = ExecutorType.Activate, Description = "Activate Vandalism -> Search Medius" },
                    new() { CardId = CardId.MediusThePure, ActionType = ExecutorType.Summon, Description = "Normal Summon Medius" }
                },
                EndBoardScore = 90
            });

            BaitPlanner.RegisterComboStarters(CardId.Vandalism, CardId.Varnish, CardId.MediusThePure, CardId.ArtmagePowerPatron);
            BaitPlanner.RegisterBaitCards(CardId.Vandalism, CardId.Acropolis, CardId.TripleTacticsTalent);
            ChainAdvisor.RegisterHighValueTargets(CardId.MediusThePure, CardId.ShadowBeastNervedo, CardId.Nerva, CardId.ArtmageDiactorus);

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTORS PIPELINE
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Quick Negates, Board Wipes & Interruptions ──
            // Nerva: When our Artmage monster effect is activated -> wipes ALL opponent cards!
            AddExecutor(ExecutorType.Activate, CardId.Nerva, NervaBoardWipeEffect);
            // Diactorus: Omni-Negate any opponent card/effect on field
            AddExecutor(ExecutorType.Activate, CardId.ArtmageDiactorus, DiactorusNegateEffect);
            // Impasto Recapture: Counter Trap (can activate turn set)
            AddExecutor(ExecutorType.Activate, CardId.ImpastoRecapture, ImpastoRecaptureEffect);
            // Finmel: Blanket Monster Negate + ATK halve
            AddExecutor(ExecutorType.Activate, CardId.ArtmageFinmel, FinmelNegateEffect);
            // Nervedo Pendulum Negate
            AddExecutor(ExecutorType.Activate, CardId.ShadowBeastNervedo, NervedoPendulumNegateEffect);
            // Handtraps
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

            // ── Tier 0.5: Board Breakers ──
            AddExecutor(ExecutorType.Activate, CardId.SuperPoly, SuperPolyEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);

            // ── Tier 1: Search Spells & Field Setup ──
            AddExecutor(ExecutorType.Activate, CardId.Vandalism, VandalismEffect);
            AddExecutor(ExecutorType.Activate, CardId.Varnish, VarnishEffect);
            AddExecutor(ExecutorType.Activate, CardId.Acropolis, AcropolisEffect);

            // ── Tier 2: Monster Starters & Discards ──
            AddExecutor(ExecutorType.Activate, CardId.ArtmagePowerPatron, PowerPatronDiscardEffect);
            AddExecutor(ExecutorType.Summon, CardId.MediusThePure, MediusNormalSummon);
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusOnSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.MediusThePure, MediusGYReviveEffect);

            // Nervedo on-field banish 3 to summon Nerva, and Extra Deck trigger
            AddExecutor(ExecutorType.Activate, CardId.ShadowBeastNervedo, NervedoEffect);

            // ── Tier 3: Hand Extenders & Inherent Special Summons ──
            AddExecutor(ExecutorType.Activate, CardId.ArtmageFinmel, FinmelHandSSEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageGraflare, GraflareHandSSEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageLitera, LiteraHandSSEffect);

            // In-archetype Spell/Trap activations
            AddExecutor(ExecutorType.Activate, CardId.Masterwork, MasterworkFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.Pact, PactEffect);

            // In-archetype Monster Secondary Triggers
            AddExecutor(ExecutorType.Activate, CardId.ArtmageGraflare, GraflareDestroyEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageDiactorus, DiactorusPosEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageDiactorus, DiactorusDestroyedEffect);
            AddExecutor(ExecutorType.Activate, CardId.ArtmageNonFinito, NonFinitoOnSummonSetEffect);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.ArtmageLitera, LiteraNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.ShadowBeastNervedo, NervedoNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.ArtmageFinmel, FinmelTributeSummon);
            AddExecutor(ExecutorType.Summon, CardId.ArtmageGraflare, GraflareTributeSummon);

            // ── Tier 4: Extra Deck Summons ──
            AddExecutor(ExecutorType.SpSummon, CardId.ArtmageNonFinito, NonFinitoAlternateSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Nerva, NervaTributeSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightOnSummonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.CrossSheep, CrossSheepSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, KnightmarePhoenixSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmarePhoenix, KnightmarePhoenixEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeTalkerSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeTalkerEffect);

            // ── Tier 5: Backrow Setting ──
            // Impasto Recapture can be activated turn set!
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
            _nervedoBanishUsed = false;
            _nervedoPendulumNegateUsed = false;
            _mediusSummonSearchUsed = false;
            _mediusGYSSEused = false;
            _finmelSSEused = false;
            _finmelNegateUsed = false;
            _graflareSSEused = false;
            _graflareDestroyUsed = false;
            _literaSSUsed = false;
            _literaQuickSSUsed = false;
            _vandalismUsed = false;
            _varnishUsed = false;
            _acropolisSearchUsed = false;
            _masterworkUsed = false;
            _pactUsed = false;
            _impastoUsed = false;
            _nonFinitoSetUsed = false;
            _nonFinitoFusionUsed = false;
            _powerPatronDiscardUsed = false;
            _lastAnnouncedId = 0;
        }

        private bool SpellSetInMain2()
        {
            return Duel.Phase == DuelPhase.Main2 || (Duel.Phase == DuelPhase.Main1 && Duel.Turn == 1);
        }

        private bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return BossMonsters.Contains(card.Id);
        }

        private int GetDistinctRaceCount()
        {
            return Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).Select(c => c.Race).Distinct().Count();
        }

        // ═══════════════════════════════════════════════════════════════
        //  TIER 0 & QUICK EFFECTS
        // ═══════════════════════════════════════════════════════════════

        private bool NervaBoardWipeEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            // Nerva triggers when our Artmage MONSTER effect is activated
            if (Duel.LastChainPlayer == 0)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 0 && lastCard.IsMonster() && (lastCard.HasSetcode(ARTMAGE_SETCODE) || lastCard.Id == CardId.ShadowBeastNervedo))
                {
                    // Only trigger if opponent controls cards to destroy!
                    if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DiactorusNegateEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            // Negate opponent card/effect on field
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && (lastCard.Location == CardLocation.MonsterZone || lastCard.Location == CardLocation.SpellZone))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ImpastoRecaptureEffect()
        {
            if (Card.IsDisabled()) return false;
            if (_impastoUsed) return false;

            // When opponent activates a monster effect
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster())
                {
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
            if (_nonFinitoFusionUsed) return false;
            if (Duel.Player != 1) return false;

            int faceupMonsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c != Card);
            if (faceupMonsters >= 1)
            {
                _nonFinitoFusionUsed = true;
                AI.SelectCard(CardId.ArtmageDiactorus, CardId.Nerva);
                return true;
            }
            return false;
        }

        private bool SuperPolyEffect()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;
            if (Bot.Hand.Count < 1) return false;

            if (Enemy.GetMonsterCount() >= 2)
            {
                var discard = Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.ArtmageLitera || c.Id == CardId.MediusThePure))
                           ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && !IsAceCard(c));
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
            return true;
        }

        private bool AcropolisEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                if (Bot.HasInSpellZone(CardId.Acropolis)) return false;
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && !Card.IsFacedown() && !_acropolisSearchUsed)
            {
                var discard = Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.Id == CardId.Acropolis || c.Id == CardId.Vandalism || c.Id == CardId.Varnish))
                           ?? Bot.Hand.FirstOrDefault(c => c != null && c != Card && (c.IsSpell() || c.IsTrap()) && c.Id != CardId.SuperPoly && c.Id != CardId.ImpastoRecapture);

                if (discard != null)
                {
                    int searchId = 0;
                    if (!Bot.HasInMonstersZone(CardId.MediusThePure) && !Bot.Hand.Any(c => c != null && c.Id == CardId.MediusThePure))
                        searchId = CardId.ArtmagePowerPatron;
                    else if (!Bot.HasInMonstersZone(CardId.ArtmageFinmel) && !Bot.Hand.Any(c => c != null && c.Id == CardId.ArtmageFinmel))
                        searchId = CardId.ArtmageFinmel;
                    else if (!Bot.HasInMonstersZone(CardId.ArtmageGraflare) && !Bot.Hand.Any(c => c != null && c.Id == CardId.ArtmageGraflare))
                        searchId = CardId.ArtmageGraflare;
                    else
                        searchId = CardId.ArtmageLitera;

                    if (searchId != 0)
                    {
                        AI.SelectCard(discard);
                        AI.SelectAnnounceID(searchId);
                        _lastAnnouncedId = searchId;
                        _acropolisSearchUsed = true;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool PowerPatronDiscardEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_powerPatronDiscardUsed) return false;
            _powerPatronDiscardUsed = true;
            AI.SelectCard(CardId.ShadowBeastNervedo);
            return true;
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
            if (_mediusSummonSearchUsed) return false;
            _mediusSummonSearchUsed = true;

            // Special Summon Nervedo directly from Deck!
            AI.SelectCard(CardId.ShadowBeastNervedo);
            return true;
        }

        private bool MediusGYReviveEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_mediusGYSSEused) return false;

            var shuffleTarget = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.Id != CardId.MediusThePure && !IsAceCard(c))
                             ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c) && c.Id != CardId.Nerva);

            if (shuffleTarget != null)
            {
                _mediusGYSSEused = true;
                AI.SelectCard(shuffleTarget);
                return true;
            }
            return false;
        }

        private bool NervedoEffect()
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

            // Trigger when sent to Extra Deck face-up: SS Artmage monster from Deck!
            if (Card.Location == CardLocation.Extra)
            {
                AI.SelectCard(CardId.ArtmageFinmel, CardId.ArtmageGraflare, CardId.ArtmageLitera);
                return true;
            }

            return false;
        }

        private bool FinmelHandSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_finmelSSEused) return false;

            bool hasArtmage = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.HasSetcode(ARTMAGE_SETCODE) || c.Id == CardId.MediusThePure));
            if (hasArtmage)
            {
                _finmelSSEused = true;
                return true;
            }
            return false;
        }

        private bool GraflareHandSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_graflareSSEused) return false;

            bool hasArtmage = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.HasSetcode(ARTMAGE_SETCODE) || c.Id == CardId.MediusThePure));
            if (hasArtmage)
            {
                _graflareSSEused = true;
                AI.SelectCard(CardId.Masterwork, CardId.Varnish, CardId.Vandalism);
                return true;
            }
            return false;
        }

        private bool LiteraHandSSEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_literaSSUsed) return false;

            bool hasArtmage = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.HasSetcode(ARTMAGE_SETCODE) || c.Id == CardId.MediusThePure));
            if (hasArtmage)
            {
                _literaSSUsed = true;
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
                if (_masterworkUsed) return false;

                bool hasArtmage = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasSetcode(ARTMAGE_SETCODE))
                               || Bot.Hand.Any(c => c != null && c.HasSetcode(ARTMAGE_SETCODE));
                int totalMats = Bot.Hand.Count(c => c != null && c.IsMonster())
                              + Bot.GetMonsters().Count(c => c != null && c.IsFaceup());

                if (hasArtmage && totalMats >= 2)
                {
                    _masterworkUsed = true;
                    AI.SelectCard(CardId.ArtmageDiactorus, CardId.ArtmageNonFinito, CardId.Nerva);
                    return true;
                }
            }

            if (Card.Location == CardLocation.Grave)
            {
                var gyCards = Bot.Graveyard.Where(c => c != null && c.HasSetcode(ARTMAGE_SETCODE) && c != Card).Distinct().Take(3).ToList();
                if (gyCards.Count >= 3)
                {
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
                AI.SelectCard(CardId.MediusThePure, CardId.ArtmageFinmel, CardId.ArtmageLitera);
                return true;
            }

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
            AI.SelectCard(CardId.ImpastoRecapture, CardId.Pact, CardId.Masterwork);
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

        private bool NervaTributeSummon()
        {
            if (Bot.HasInMonstersZone(CardId.Nerva)) return false;
            return GetDistinctRaceCount() >= 3 && Bot.GetMonsterCount() >= 3;
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

        private bool CrossSheepSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 2 && !Bot.HasInMonstersZone(CardId.CrossSheep);
        }

        private bool KnightmarePhoenixSummon()
        {
            int mats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return mats >= 2 && Enemy.GetSpellCount() > 0;
        }

        private bool KnightmarePhoenixEffect()
        {
            var target = Enemy.GetSpells().FirstOrDefault(c => c != null);
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
        //  TACTICAL DECISION OVERRIDES
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Announce search target
            if (hint == HINT_SELECT_TOHAND && _lastAnnouncedId != 0)
            {
                var target = cards.FirstOrDefault(c => c.Id == _lastAnnouncedId);
                if (target != null)
                {
                    _lastAnnouncedId = 0;
                    return new[] { target };
                }
            }

            // Removal hints: ONLY target opponent cards
            if (hint == HINT_SELECT_DESTROY || hint == HINT_SELECT_REMOVE || hint == HINT_SELECT_TOGRAVE)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count >= min)
                {
                    var sorted = enemyCards.OrderByDescending(c => c.Attack).ToList();
                    return Util.CheckSelectCount(sorted, cards, min, max);
                }
            }

            // Special Summon: Prioritize Nerva, Diactorus, Finmel, Nervedo
            if (hint == HINT_SELECT_SPSUMMON)
            {
                var preferred = new[] { CardId.Nerva, CardId.ArtmageDiactorus, CardId.ArtmageFinmel, CardId.ShadowBeastNervedo, CardId.MediusThePure };
                foreach (int id in preferred)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                    if (match != null) return new[] { match };
                }
            }

            // Search to hand (506): prioritize Medius, Nervedo, Finmel, Acropolis
            if (hint == HINT_SELECT_TOHAND && cards.All(c => c.Location == CardLocation.Deck))
            {
                var preferred = new[] { CardId.MediusThePure, CardId.ShadowBeastNervedo, CardId.ArtmageFinmel, CardId.Acropolis, CardId.Varnish, CardId.Vandalism };
                foreach (int id in preferred)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                    if (match != null) return new[] { match };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.Nerva || cardId == CardId.ArtmageDiactorus || cardId == CardId.ArtmageFinmel || cardId == CardId.AccesscodeTalker || cardId == CardId.SPLittleKnight)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            if (cardId == CardId.ShadowBeastNervedo || cardId == CardId.ArtmageLitera || cardId == CardId.CrossSheep)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
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
