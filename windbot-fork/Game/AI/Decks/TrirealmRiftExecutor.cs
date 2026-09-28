using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.DecisionEngine;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("TrirealmRift", "TrirealmRift")]
    public class TrirealmRiftExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Deck Monsters
            public const int TrirealmRiftOfEmptinessGehenna = 100458031; // Lv 1 DARK
            public const int TrirealmRiftOfSkySheol = 100458032;        // Lv 2 LIGHT
            public const int TrirealmRiftOfBlueTuonela = 100458033;     // Lv 3 WATER
            public const int TrirealmRiftOfScarletNaraka = 100458034;   // Lv 4 FIRE
            public const int SkyThunderTrirealmRiftYomi = 100458035;    // Lv 5 WIND (Core Boss / Negate)
            public const int BurialSummitTrirealmRiftHelheim = 100458036; // Lv 6 EARTH (Protection Boss)
            public const int MadTempestTrirealmRiftPloutonion = 100458037; // Lv 7 WATER (Draw & Removal)
            public const int TrirealmRiftDarkness = 100458038;          // Lv 8 DARK (Giant Finisher)

            // Staples / Handtraps
            public const int AshBlossom = 14558127;
            public const int AshBlossomAlt = 14558128;
            public const int InfiniteImpermanence = 10045474;
            public const int CalledByTheGrave = 24224830;
            public const int CalledByTheGraveAlt = 24224831;
            public const int PotOfDesires = 35261759;

            // Spells & Traps
            public const int TrirealmRiftTerritoryValvols = 100458039; // Field Spell
            public const int TrirealmRiftGospel = 100458040;          // Quick-Play Spell
            public const int TrirealmRiftJudgment = 100458041;        // Counter Trap

            // Extra Deck
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareUnicorn = 38342335;
            public const int AccesscodeTalker = 86066372;
            public const int AbyssDweller = 21044178;
            public const int Number41Bagooska = 90590303;
            public const int DivineArsenalAAZEUS = 90448279;
            public const int SuperStarslayerTYPHON = 93039339;
            public const int Linkuriboh = 41999284;
            public const int RelinquishedAnima = 94259633;
            public const int DharcTheDarkCharmer = 8264361;
            public const int LynaTheLightCharmer = 9839945;
            public const int HiitaTheFireCharmer = 48815792;
            public const int WynnTheWindCharmer = 30674956;
        }

        private static readonly int[] TrirealmMonsters = {
            CardId.TrirealmRiftOfEmptinessGehenna,
            CardId.TrirealmRiftOfSkySheol,
            CardId.TrirealmRiftOfBlueTuonela,
            CardId.TrirealmRiftOfScarletNaraka,
            CardId.SkyThunderTrirealmRiftYomi,
            CardId.BurialSummitTrirealmRiftHelheim,
            CardId.MadTempestTrirealmRiftPloutonion,
            CardId.TrirealmRiftDarkness
        };

        internal TrirealmRiftPlugin Plugin { get; private set; }

        private bool _yomiSearchedThisTurn = false;
        private bool _helheimSummonedThisTurn = false;
        private bool _ploutonionDrawnThisTurn = false;
        private bool _valvolsSearchedThisTurn = false;
        private bool _gehennaSearchedThisTurn = false;
        private bool _sheolSearchedThisTurn = false;


        public TrirealmRiftExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // 0. Install Decoupled Domain Plugin
            DeckPlugin = new TrirealmRiftPlugin(this);
            Plugin = (TrirealmRiftPlugin)DeckPlugin;

            // 1. Counter Traps & Quick Negates (Priority 1)
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftJudgment, OnTrirealmJudgment);
            AddExecutor(ExecutorType.Activate, CardId.SkyThunderTrirealmRiftYomi, OnYomiQuickNegate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGraveAlt, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // 2. Opponent Turn Disruptions (Priority 2)
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfScarletNaraka, OnNarakaOpponentSummon);
            AddExecutor(ExecutorType.Activate, CardId.MadTempestTrirealmRiftPloutonion, OnPloutonionQuickSpin);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftDarkness, OnDarknessQuickDestroy);
            AddExecutor(ExecutorType.Activate, CardId.BurialSummitTrirealmRiftHelheim, OnHelheimQuickSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, OnSPLittleKnightActivate);
            AddExecutor(ExecutorType.Activate, CardId.IPMasquerena, OnIPMasquerenaActivate);

            // 3. Field & Hand Draw Engines (Priority 3)
            AddExecutor(ExecutorType.Activate, CardId.PotOfDesires, OnPotOfDesires);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftTerritoryValvols, OnValvolsActivate);
            AddExecutor(ExecutorType.Activate, CardId.MadTempestTrirealmRiftPloutonion, OnPloutonionHandDraw);
            AddExecutor(ExecutorType.Activate, CardId.SkyThunderTrirealmRiftYomi, OnYomiHandSearch);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftGospel, OnGospelActivate);

            // 4. Starters & Special Summons from Hand (Priority 4)
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfEmptinessGehenna, OnGehennaActivate);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfSkySheol, OnSheolActivate);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfBlueTuonela, OnTuonelaActivate);
            AddExecutor(ExecutorType.Activate, CardId.TrirealmRiftOfScarletNaraka, OnNarakaHandSummon);

            // 5. Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfScarletNaraka);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfEmptinessGehenna);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfSkySheol);
            AddExecutor(ExecutorType.Summon, CardId.TrirealmRiftOfBlueTuonela);

            // 6. Extra Deck Toolbox
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, OnAccesscodeSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, OnKnightmareUnicornSummon);
            AddExecutor(ExecutorType.Activate, CardId.KnightmareUnicorn);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, OnSPLittleKnightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number41Bagooska, OnBagooskaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AbyssDweller, OnAbyssDwellerSummon);
            AddExecutor(ExecutorType.Activate, CardId.AbyssDweller);
            AddExecutor(ExecutorType.SpSummon, CardId.IPMasquerena, OnIPMasquerenaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Linkuriboh, OnLinkuribohSummon);
            AddExecutor(ExecutorType.Activate, CardId.Linkuriboh);

            // 7. Spell & Trap Backrow Placement
            AddExecutor(ExecutorType.SpellSet, CardId.TrirealmRiftJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGraveAlt);
            AddExecutor(ExecutorType.SpellSet, CardId.TrirealmRiftGospel);

            // 8. Repositioning
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // First turn preferred to set up face-down banish pool and Yomi negate
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _yomiSearchedThisTurn = false;
            _helheimSummonedThisTurn = false;
            _ploutonionDrawnThisTurn = false;
            _valvolsSearchedThisTurn = false;
            _gehennaSearchedThisTurn = false;
            _sheolSearchedThisTurn = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // DISRUPTIONS & COMBOS
        // ═══════════════════════════════════════════════════════════════

        private bool OnYomiQuickNegate()
        {
            // Only on field
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Duel.LastChainPlayer != 1) return false;

            var lastCard = Util.GetLastChainCard();
            if (lastCard == null || !lastCard.HasType(CardType.Monster)) return false;

            // Check if we have a face-down banished Trirealm Rift monster with the same attribute
            int enemyAttr = (int)lastCard.Attribute;
            var banishedMatch = Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && ((int)c.Attribute & enemyAttr) > 0);
            return banishedMatch;
        }

        private bool OnTrirealmJudgment()
        {
            // Counter Trap: when opponent summons or damage step ATK = 0
            return Duel.LastChainPlayer == 1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Damage;
        }

        private bool OnPloutonionQuickSpin()
        {
            // On field: spin opponent monster with same attribute
            if (Card.Location != CardLocation.MonsterZone) return false;

            var enemyMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()).ToList();
            if (enemyMonsters.Count == 0) return false;

            // Check if we have face-down banished Trirealm monster matching attribute
            foreach (var em in enemyMonsters)
            {
                int attr = (int)em.Attribute;
                if (Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && ((int)c.Attribute & attr) > 0))
                {
                    return true;
                }
            }
            return false;
        }

        private bool OnDarknessQuickDestroy()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // 20+ face-down banished cards required for quick effect destroy
            int facedownCount = Bot.Banished.Count(c => c != null && c.IsFacedown());
            if (facedownCount < 20) return false;

            return Enemy.GetMonsters().Any(c => c != null && !c.IsShouldNotBeTarget())
                || Enemy.GetSpells().Any(c => c != null && !c.IsShouldNotBeTarget());
        }

        private bool OnNarakaOpponentSummon()
        {
            // During opponent's Main Phase, Special Summon Level 5+ from face-down banished
            if (Card.Location == CardLocation.MonsterZone && Duel.Player != 0)
            {
                return Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && c.Level >= 5);
            }
            return false;
        }

        private bool OnHelheimQuickSummon()
        {
            // In Hand: Quick Effect during Main Phase to reveal & banish face-down -> Special Summon Lv 5+
            if (Card.Location == CardLocation.Hand && !_helheimSummonedThisTurn)
            {
                bool hasBanishedTarget = Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && c.Level != 6);
                if (hasBanishedTarget && Bot.GetMonsterCount() < 5)
                {
                    _helheimSummonedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnPotOfDesires()
        {
            // Banishes 10 cards face-down and draws 2 -> Fuels the entire Trirealm Rift engine!
            return Bot.Deck.Count >= 15;
        }

        private bool OnValvolsActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Can activate by banishing 5 cards from GY/top of Deck face-down
                return Bot.Deck.Count + Bot.Graveyard.Count >= 5;
            }
            if (Card.Location == CardLocation.SpellZone && !_valvolsSearchedThisTurn)
            {
                // Search 1 face-down banished Trirealm Rift card
                _valvolsSearchedThisTurn = true;
                return Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealm(c));
            }
            return false;
        }

        private bool OnPloutonionHandDraw()
        {
            // In Hand: reveal Ploutonion + 1 other Trirealm card -> banish both face-down -> draw 2
            if (Card.Location == CardLocation.Hand && !_ploutonionDrawnThisTurn)
            {
                var other = Bot.Hand.FirstOrDefault(c => c != null && c != Card && IsTrirealm(c));
                if (other != null)
                {
                    _ploutonionDrawnThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnYomiHandSearch()
        {
            // In Hand: reveal Yomi -> banish face-down -> add 1 face-down banished Trirealm card (except Lv 5) to hand
            if (Card.Location == CardLocation.Hand && !_yomiSearchedThisTurn)
            {
                bool hasTarget = Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealm(c) && c.Level != 5);
                if (hasTarget)
                {
                    _yomiSearchedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool OnGospelActivate()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                // Banish 5 from GY/Deck -> extra normal summon or convert Lv 4 to Lv 5+
                return Bot.Deck.Count + Bot.Graveyard.Count >= 5;
            }
            return false;
        }

        private bool OnGehennaActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Free Special Summon when 0 monsters
                return Bot.GetMonsterCount() == 0;
            }
            if (Card.Location == CardLocation.MonsterZone && !_gehennaSearchedThisTurn)
            {
                // Add up to 2 face-down banished Trirealm monsters with different names
                _gehennaSearchedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool OnSheolActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Free Special Summon when controlling Trirealm monster
                return Bot.GetMonsters().Any(c => c != null && IsTrirealmMonster(c)) && Bot.GetMonsterCount() < 5;
            }
            if (Card.Location == CardLocation.MonsterZone && !_sheolSearchedThisTurn)
            {
                // Add up to 2 face-down banished Trirealm Spells/Traps
                _sheolSearchedThisTurn = true;
                return true;
            }
            return false;
        }

        private bool OnTuonelaActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Special summon when opponent controls more monsters
                return Enemy.GetMonsterCount() > Bot.GetMonsterCount() && Bot.GetMonsterCount() < 5;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Special summon 1 Lv 5+ face-down banished Trirealm monster
                return Bot.Banished.Any(c => c != null && c.IsFacedown() && IsTrirealmMonster(c) && c.Level >= 5);
            }
            return false;
        }

        private bool OnNarakaHandSummon()
        {
            // When opponent special summons a monster, special summon Naraka
            return Card.Location == CardLocation.Hand && Bot.GetMonsterCount() < 5;
        }

        // Extra Deck Summons
        private bool OnAccesscodeSummon()
        {
            return Util.IsTurn1OrMain2() && Bot.GetMonsterCount() >= 3;
        }

        private bool OnKnightmareUnicornSummon()
        {
            return Enemy.GetMonsterCount() > 0 && Bot.GetMonsterCount() >= 3;
        }

        private bool OnSPLittleKnightSummon()
        {
            return Bot.GetMonsterCount() >= 2 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        private bool OnSPLittleKnightActivate()
        {
            return Card.Location == CardLocation.MonsterZone;
        }

        private bool OnIPMasquerenaSummon()
        {
            return Duel.Turn == 1 && Bot.GetMonsterCount() >= 2;
        }

        private bool OnIPMasquerenaActivate()
        {
            return Duel.Player != 0 && Card.Location == CardLocation.MonsterZone;
        }

        private bool OnBagooskaSummon()
        {
            return Duel.Turn == 1 && Bot.GetMonsters().Count(c => c != null && c.Level == 4) >= 2;
        }

        private bool OnAbyssDwellerSummon()
        {
            return Duel.Turn == 1 && Bot.GetMonsters().Count(c => c != null && c.Level == 4) >= 2;
        }

        private bool OnLinkuribohSummon()
        {
            var lv1 = Bot.GetMonsters().FirstOrDefault(c => c != null && c.Level == 1 && c.IsCode(CardId.TrirealmRiftOfEmptinessGehenna));
            return lv1 != null;
        }

        private bool IsTrirealm(ClientCard card)
        {
            if (card == null) return false;
            return TrirealmMonsters.Contains(card.Id)
                || card.IsCode(CardId.TrirealmRiftTerritoryValvols)
                || card.IsCode(CardId.TrirealmRiftGospel)
                || card.IsCode(CardId.TrirealmRiftJudgment);
        }

        private bool IsTrirealmMonster(ClientCard card)
        {
            if (card == null) return false;
            return TrirealmMonsters.Contains(card.Id);
        }

        // ═══════════════════════════════════════════════════════════════
        // CALLBACK OVERRIDES
        // ═══════════════════════════════════════════════════════════════

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            // 1. High DEF walls & Handtraps -> Defense
            if (cardId == CardId.SkyThunderTrirealmRiftYomi // 1000 ATK / 2500 DEF
                || cardId == CardId.TrirealmRiftOfBlueTuonela // 300 ATK / 2000 DEF
                || cardId == CardId.TrirealmRiftOfEmptinessGehenna // 100 ATK / 1000 DEF
                || cardId == CardId.AshBlossom
                || cardId == CardId.AshBlossomAlt
                || cardId == CardId.Number41Bagooska)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }

            // 2. High ATK Bosses -> Attack
            if (cardId == CardId.TrirealmRiftDarkness // 3000 ATK
                || cardId == CardId.BurialSummitTrirealmRiftHelheim // 2400 ATK
                || cardId == CardId.MadTempestTrirealmRiftPloutonion // 2100 ATK
                || cardId == CardId.AccesscodeTalker)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return new List<ClientCard>();

            // 1. Hint 506: ATOHAND (Search target from banished)
            if (hint == 506)
            {
                var target = Plugin.Strategy.PickSearchTarget(cards, Card);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 2. Hint 509: SPSUMMON (Special Summon target from banished)
            if (hint == 509)
            {
                var target = Plugin.Strategy.PickSpecialSummonTarget(cards);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            // 3. Hint 501 / 504: DISCARD / TOGRAVE
            if (hint == 501 || hint == 504)
            {
                var discardTarget = Plugin.MaterialEvaluator.PickDiscardTarget(cards, min);
                if (discardTarget != null)
                {
                    return new List<ClientCard> { discardTarget };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            // Judgment: Option 0: Add to Hand, Option 1: Special Summon
            // Prioritize Special Summon if monster zone has room!
            if (Card != null && Card.IsCode(CardId.TrirealmRiftJudgment))
            {
                for (int i = 0; i < options.Count; i++)
                {
                    long optIndex = options[i] & 0xf;
                    if (optIndex == 1 && Bot.GetMonsterCount() < 5) return i;
                    if (optIndex == 0) return i;
                }
            }

            return base.OnSelectOption(options);
        }
    }
}
