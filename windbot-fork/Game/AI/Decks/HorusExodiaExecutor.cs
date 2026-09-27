using System.Collections.Generic;
using System.Linq;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    // ============================================================================
    // RULE-BASED EXECUTOR: HorusExodiaExecutor
    // ============================================================================
    [Deck("HorusExodia", "AI_HorusExodia", "Normal")]
    public class HorusExodiaExecutor : ModernExecutor
    {
        public class CardId
        {
            // Exodia 5 Pieces
            public const int LeftArmForbiddenOne = 7902349;
            public const int LeftLegForbiddenOne = 44519536;
            public const int RightArmForbiddenOne = 70903634;
            public const int RightLegForbiddenOne = 8124921;
            public const int ExodiaForbiddenOne = 33396948;

            // Millennium Engine
            public const int SengenjinMillennium = 38775407;
            public const int GolemMillennium = 74169516;
            public const int MaidenMillenniumMoon = 37552929;
            public const int ShieldMillenniumDynasty = 1164211;
            public const int MillenniumAnkh = 37613663;
            public const int WedjuTemple = 63017368;
            public const int Obliterate = 64043465;

            // Horus Engine
            public const int ImsetyGloryOfHorus = 84941194;
            public const int HapiGuidanceOfHorus = 47330808;
            public const int DuamutefBlessingOfHorus = 11335209;
            public const int QebehsenuefProtectionOfHorus = 74725513;
            public const int KingsSarcophagus = 16528181;

            // Handtraps & Staples
            public const int AshBlossom = 14558127;
            public const int InfiniteImpermanence = 10045474;
            public const int EffectVeiler = 97268402;
            public const int CalledByTheGrave = 24224830;
            public const int CrossoutDesignator = 65681983;

            // Extra Deck
            public const int TheUnstoppableExodiaIncarnate = 83257450;
            public const int CoachKingGiantrainer = 30741334;
            public const int Number38HopeHarbinger = 63767246;
            public const int Number90GalaxyEyesPhotonLord = 8165596;
            public const int TheZombieVampire = 34086406;
            public const int DingirsuTheOrcust = 93854893;
            public const int Number23Lancelot = 66547759;
            public const int Number68Sanaphond = 43490025;
            public const int Number60Dugares = 66011101;
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
            public const int TYPHONSkyCrisis = 93039339;
            public const int AAZEUS = 90448279;
        }

        private readonly HorusExodiaPlugin _plugin;

        public HorusExodiaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _plugin = new HorusExodiaPlugin(this);
            DeckPlugin = _plugin;

            // Register Ace Bosses
            HeuristicGuard.RegisterAceCards(
                CardId.TheUnstoppableExodiaIncarnate,
                CardId.CoachKingGiantrainer,
                CardId.Number90GalaxyEyesPhotonLord,
                CardId.Number38HopeHarbinger,
                CardId.Number23Lancelot,
                CardId.DingirsuTheOrcust
            );

            // ==========================================
            // PRIORITY 1: HANDTRAPS & UNIVERSAL NEGATIONS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, DefaultCrossoutDesignator);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, DefaultEffectVeiler);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // ==========================================
            // PRIORITY 2: BOSS QUICK EFFECTS & DISRUPTIONS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.TheUnstoppableExodiaIncarnate, ExodiaIncarnateNegate);
            AddExecutor(ExecutorType.Activate, CardId.Obliterate, ObliterateActivate);
            AddExecutor(ExecutorType.Activate, CardId.Number38HopeHarbinger, HopeHarbingerEffect);
            AddExecutor(ExecutorType.Activate, CardId.Number90GalaxyEyesPhotonLord, PhotonLordEffect);
            AddExecutor(ExecutorType.Activate, CardId.Number23Lancelot);

            // ==========================================
            // PRIORITY 3: HORUS ENGINE (Pre-Ankh Setup)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.ImsetyGloryOfHorus, ImsetyActivate);
            AddExecutor(ExecutorType.Activate, CardId.KingsSarcophagus, KingsSarcophagusActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.ImsetyGloryOfHorus, HorusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HapiGuidanceOfHorus, HorusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DuamutefBlessingOfHorus, HorusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.QebehsenuefProtectionOfHorus, HorusSpSummon);

            // ==========================================
            // PRIORITY 4: RANK 8 XYZ SUMMONS (Strictly PRE-ANKH!)
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.CoachKingGiantrainer, CoachKingSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.CoachKingGiantrainer, CoachKingActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Number90GalaxyEyesPhotonLord, Number90SpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number38HopeHarbinger, Number38SpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DingirsuTheOrcust, DingirsuSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DingirsuTheOrcust, DingirsuActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Number23Lancelot, LancelotSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TheZombieVampire, ZombieVampireSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TheZombieVampire, ZombieVampireActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Number68Sanaphond, SanaphondSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number68Sanaphond);

            // Link & Rank 4
            AddExecutor(ExecutorType.SpSummon, CardId.Number60Dugares, DugaresSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number60Dugares);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.TYPHONSkyCrisis, TYPHONSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TYPHONSkyCrisis);

            // ==========================================
            // PRIORITY 5: MILLENNIUM ENGINE SETUP
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.WedjuTemple);
            // Place into Spell/Trap Zone
            AddExecutor(ExecutorType.Activate, CardId.ShieldMillenniumDynasty, ShieldPlaceActivate);
            AddExecutor(ExecutorType.Activate, CardId.SengenjinMillennium, SengenjinPlaceActivate);
            AddExecutor(ExecutorType.Activate, CardId.GolemMillennium, GolemPlaceActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaidenMillenniumMoon, MaidenPlaceActivate);

            // Special Summon from Spell/Trap Zone onto Monster Zone!
            AddExecutor(ExecutorType.SpSummon, CardId.ShieldMillenniumDynasty, MillenniumSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GolemMillennium, MillenniumSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SengenjinMillennium, MillenniumSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MaidenMillenniumMoon, MillenniumSpSummon);

            // ==========================================
            // PRIORITY 6: MILLENNIUM ANKH (The Finale Move!)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.MillenniumAnkh, MillenniumAnkhActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.TheUnstoppableExodiaIncarnate);

            // ==========================================
            // PRIORITY 7: SPELL/TRAP SETS
            // ==========================================
            AddExecutor(ExecutorType.SpellSet, CardId.Obliterate);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
        }

        public override void OnNewTurn()
        {
            _plugin.StrategyImpl.Reset();
            base.OnNewTurn();
        }

        // ============================================================
        // ACTIVATION & SUMMON LOGIC
        // ============================================================

        private bool ImsetyActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.Hand.Count >= 2;
            }
            return true;
        }

        private bool KingsSarcophagusActivate()
        {
            if (Card.Location == CardLocation.Hand) return true;
            // Discard 1 to dump Horus from deck to GY
            var horusInDeck = Bot.Deck.Any(c => c != null && (c.Id == CardId.ImsetyGloryOfHorus ||
                                                              c.Id == CardId.HapiGuidanceOfHorus ||
                                                              c.Id == CardId.DuamutefBlessingOfHorus ||
                                                              c.Id == CardId.QebehsenuefProtectionOfHorus));
            return horusInDeck && Bot.Hand.Count >= 1;
        }

        private bool HorusSpSummon()
        {
            if (_plugin.StrategyImpl.AnkhActivatedThisTurn) return false;
            return Bot.HasInSpellZone(CardId.KingsSarcophagus);
        }

        private bool CoachKingSpSummon()
        {
            if (_plugin.StrategyImpl.AnkhActivatedThisTurn) return false;
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 3;
        }

        private bool CoachKingActivate()
        {
            return true;
        }

        private bool Number90SpSummon()
        {
            if (_plugin.StrategyImpl.AnkhActivatedThisTurn) return false;
            if (Bot.HasInMonstersZone(CardId.Number90GalaxyEyesPhotonLord)) return false;
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 2;
        }

        private bool Number38SpSummon()
        {
            if (_plugin.StrategyImpl.AnkhActivatedThisTurn) return false;
            if (Bot.HasInMonstersZone(CardId.Number38HopeHarbinger)) return false;
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 2;
        }

        private bool DingirsuSpSummon()
        {
            if (_plugin.StrategyImpl.AnkhActivatedThisTurn) return false;
            if (Bot.HasInMonstersZone(CardId.DingirsuTheOrcust)) return false;
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 2 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        private bool DingirsuActivate()
        {
            return true;
        }

        private bool LancelotSpSummon()
        {
            if (_plugin.StrategyImpl.AnkhActivatedThisTurn) return false;
            if (Bot.HasInMonstersZone(CardId.Number23Lancelot)) return false;
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 2;
        }

        private bool ZombieVampireSpSummon()
        {
            if (_plugin.StrategyImpl.AnkhActivatedThisTurn) return false;
            if (Bot.HasInMonstersZone(CardId.TheZombieVampire)) return false;
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 2 && Bot.Deck.Count >= 5;
        }

        private bool ZombieVampireActivate()
        {
            return true;
        }

        private bool SanaphondSpSummon()
        {
            if (_plugin.StrategyImpl.AnkhActivatedThisTurn) return false;
            if (Bot.HasInMonstersZone(CardId.Number68Sanaphond)) return false;
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 2 && Duel.Turn == 1;
        }

        private bool DugaresSpSummon()
        {
            if (_plugin.StrategyImpl.AnkhActivatedThisTurn) return false;
            int lv4Count = Bot.GetMonsters().Count(m => m != null && m.Level == 4 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv4Count >= 2;
        }

        private bool SPLittleKnightSpSummon()
        {
            if (_plugin.StrategyImpl.AnkhActivatedThisTurn) return false;
            // Rule 2: Never summon SP in MP1 if enemy board is empty or direct attack is possible
            if (Duel.Phase == DuelPhase.Main1 && Enemy.GetMonsterCount() == 0)
                return false;

            // Never sacrifice Ace or high ATK monsters (>= 2000) for SP in MP1
            var candidates = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && !IsAceCard(m) && m.Attack < 2000).ToList();
            return candidates.Count >= 2 && (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) > 0;
        }

        private bool SPLittleKnightActivate()
        {
            return true;
        }

        private bool TYPHONSpSummon()
        {
            return Enemy.GetMonsters().Any(m => m != null && m.Attack >= 3000) || Duel.Phase == DuelPhase.Main2;
        }

        private bool ShieldPlaceActivate()
        {
            return Card.Location == CardLocation.Hand;
        }

        private bool SengenjinPlaceActivate()
        {
            if (Card.Location == CardLocation.Hand)
                return Bot.LifePoints > 2000;
            return true;
        }

        private bool GolemPlaceActivate()
        {
            if (Card.Location == CardLocation.Hand)
                return Bot.LifePoints > 2000;
            return true;
        }

        private bool MaidenPlaceActivate()
        {
            return Card.Location == CardLocation.Hand;
        }

        private bool MillenniumSpSummon()
        {
            // Special summon self from Spell/Trap Zone to Monster Zone
            return Card.Location == CardLocation.SpellZone;
        }

        private bool MillenniumAnkhActivate()
        {
            // Millennium Ankh locks subsequent special summons to Millennium/Exodia.
            // Execute as the grand finale after Xyz plays!
            _plugin.StrategyImpl.AnkhActivatedThisTurn = true;
            return true;
        }

        private bool ExodiaIncarnateNegate()
        {
            // Negate Spell/Trap activation and set Obliterate
            return Duel.LastChainPlayer == 1;
        }

        private bool ObliterateActivate()
        {
            // Target 1 monster on the field; send 1 Forbidden One or Exodia from Hand/Deck to GY to bounce it
            return Enemy.GetMonsterCount() > 0;
        }

        private bool HopeHarbingerEffect()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool PhotonLordEffect()
        {
            return Duel.LastChainPlayer == 1;
        }

        // ============================================================
        // HINT RESOLUTION & CALLBACKS
        // ============================================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Search / Add to Hand (Hint 506 / 505)
            if (hint == 506 || hint == 505)
            {
                var target = _plugin.StrategyImpl.PickSearchTarget(cards, Card);
                if (target != null) return new List<ClientCard> { target };
            }

            // 2. Special Summon Target (Hint 509)
            if (hint == 509)
            {
                var target = _plugin.StrategyImpl.PickSpecialSummonTarget(cards);
                if (target != null) return new List<ClientCard> { target };
            }

            // 3. Discard Target (Hint 501)
            if (hint == 501)
            {
                var discardTarget = _plugin.MaterialImpl.PickDiscardTarget(cards, min);
                if (discardTarget != null) return new List<ClientCard> { discardTarget };
            }

            // 4. Send to GY (Hint 508)
            if (hint == 508)
            {
                // King's Sarcophagus dumping Horus
                var imsety = cards.FirstOrDefault(c => c != null && c.Id == CardId.ImsetyGloryOfHorus && c.Location == CardLocation.Deck);
                if (imsety != null) return new List<ClientCard> { imsety };

                var hapi = cards.FirstOrDefault(c => c != null && c.Id == CardId.HapiGuidanceOfHorus && c.Location == CardLocation.Deck);
                if (hapi != null) return new List<ClientCard> { hapi };

                var duamutef = cards.FirstOrDefault(c => c != null && c.Id == CardId.DuamutefBlessingOfHorus && c.Location == CardLocation.Deck);
                if (duamutef != null) return new List<ClientCard> { duamutef };

                var qebeh = cards.FirstOrDefault(c => c != null && c.Id == CardId.QebehsenuefProtectionOfHorus && c.Location == CardLocation.Deck);
                if (qebeh != null) return new List<ClientCard> { qebeh };

                // Obliterate dumping Forbidden One piece from Deck/Hand
                var pieces = cards.Where(c => c != null &&
                    (c.Id == CardId.ExodiaForbiddenOne ||
                     c.Id == CardId.LeftArmForbiddenOne ||
                     c.Id == CardId.LeftLegForbiddenOne ||
                     c.Id == CardId.RightArmForbiddenOne ||
                     c.Id == CardId.RightLegForbiddenOne)).ToList();
                if (pieces.Count > 0) return new List<ClientCard> { pieces.First() };
            }

            // 5. Destruction / Removal / Bounce targeting opponent (Hint 502 / 503 / 504 / 505)
            if (hint == 502 || hint == 503 || hint == 504 || hint == 505)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    return enemyCards.OrderByDescending(c => _plugin.ThreatImpl.EvaluateThreatScore(c)).Take(max).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            // Rule 16: Always reject optional opponent card effects
            if (card != null && card.Controller == 1)
                return false;

            return base.OnSelectEffectYn(card, desc);
        }
    }
}
