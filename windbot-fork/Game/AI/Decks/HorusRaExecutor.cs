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
    // RULE-BASED EXECUTOR: HorusRaExecutor
    // ============================================================================
    [Deck("HorusRa", "AI_HorusRa", "Normal")]
    public class HorusRaExecutor : ModernExecutor
    {
        public class CardId
        {
            // Ra & Sun God Engine
            public const int TheWingedDragonOfRa = 10000010;
            public const int HelpoemerChanter = 101403001;
            public const int MakyuraDestructor = 101403002;
            public const int GilGarthTerrorMachine = 101403003;
            public const int ViserDesShock = 101403004;
            public const int TheImmortalSunGod = 101403051;
            public const int TheSunGodLeadingDownIntoDarkness = 101403052;
            public const int SunGodDomination = 101403072;
            public const int EgyptianGodSlimeProtector = 101403073;
            public const int TheTrueSunGod = 11587414;

            // Horus Engine
            public const int ImsetyGloryOfHorus = 84941194;
            public const int HapiGuidanceOfHorus = 47330808;
            public const int DuamutefBlessingOfHorus = 11335209;
            public const int QebehsenuefProtectionOfHorus = 74725513;
            public const int KingsSarcophagus = 16528181;

            // Staples & Board Breakers
            public const int SuperPolymerization = 48130397;
            public const int AshBlossom = 14558127;
            public const int InfiniteImpermanence = 10045474;
            public const int CalledByTheGrave = 24224830;

            // Extra Deck
            public const int RaSunGodOfDarkness = 101403030;
            public const int LavaGolemSteelcage = 101403031;
            public const int GaruraWings = 11765832;
            public const int MudragonOfTheSwamp = 54757758;
            public const int StarvingVenom = 41209827;
            public const int CoachKingGiantrainer = 30741334;
            public const int Number38HopeHarbinger = 63767246;
            public const int Number90GalaxyEyesPhotonLord = 8165596;
            public const int TheZombieVampire = 34086406;
            public const int DingirsuTheOrcust = 93854893;
            public const int Number68Sanaphond = 43490025;
            public const int SPLittleKnight = 29301450;
            public const int TYPHONSkyCrisis = 93039339;
        }

        private readonly HorusRaPlugin _plugin;

        public HorusRaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _plugin = new HorusRaPlugin(this);
            DeckPlugin = _plugin;

            // Register Ace Bosses for protection
            HeuristicGuard.RegisterAceCards(
                CardId.RaSunGodOfDarkness,
                CardId.TheWingedDragonOfRa,
                CardId.CoachKingGiantrainer,
                CardId.Number90GalaxyEyesPhotonLord,
                CardId.Number38HopeHarbinger,
                CardId.DingirsuTheOrcust
            );

            // ── Register Starters and Baits in BaitPlanner ──
            BaitPlanner.RegisterComboStarters(
                CardId.TheTrueSunGod,
                CardId.ImsetyGloryOfHorus,
                CardId.TheSunGodLeadingDownIntoDarkness,
                CardId.HelpoemerChanter
            );
            BaitPlanner.RegisterBaitCards(
                CardId.KingsSarcophagus,
                CardId.SuperPolymerization,
                CardId.MakyuraDestructor
            );

            // ==========================================
            // PRIORITY 1: HANDTRAPS & UNIVERSAL INTERRUPTIONS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // ==========================================
            // PRIORITY 2: BOSS QUICK EFFECTS & BOARD DISRUPTIONS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.Number38HopeHarbinger, Number38Effect);
            AddExecutor(ExecutorType.Activate, CardId.Number90GalaxyEyesPhotonLord, Number90Effect);
            AddExecutor(ExecutorType.Activate, CardId.RaSunGodOfDarkness, RaDarkSunGodActivate);
            AddExecutor(ExecutorType.Activate, CardId.SunGodDomination, SunGodDominationActivate);
            AddExecutor(ExecutorType.Activate, CardId.EgyptianGodSlimeProtector, EgyptianGodSlimeActivate);

            // ==========================================
            // PRIORITY 3: BOARD BREAKING (Super Poly & Lava Golem Contact Fusion)
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.LavaGolemSteelcage, LavaGolemSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationActivate);

            // ==========================================
            // PRIORITY 4: HORUS ENGINE (Imsety -> Sarcophagus -> Send Horus -> Revive)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.ImsetyGloryOfHorus, ImsetyActivate);
            AddExecutor(ExecutorType.Activate, CardId.KingsSarcophagus, KingsSarcophagusActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.ImsetyGloryOfHorus, HorusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.HapiGuidanceOfHorus, HorusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DuamutefBlessingOfHorus, HorusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.QebehsenuefProtectionOfHorus, HorusSpSummon);

            // ==========================================
            // PRIORITY 5: SUN GOD SETUP & FUSION POWER PLAYS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.TheTrueSunGod, TheTrueSunGodActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheSunGodLeadingDownIntoDarkness, TheSunGodLeadingActivate);
            AddExecutor(ExecutorType.Activate, CardId.HelpoemerChanter, HelpoemerActivate);
            AddExecutor(ExecutorType.Activate, CardId.MakyuraDestructor, MakyuraActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheImmortalSunGod, TheImmortalSunGodActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.RaSunGodOfDarkness);

            // ==========================================
            // PRIORITY 6: RANK 8 XYZ SUMMONS & EFFECTS
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.CoachKingGiantrainer, CoachKingSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.CoachKingGiantrainer, CoachKingActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Number90GalaxyEyesPhotonLord, Number90SpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Number38HopeHarbinger, Number38SpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DingirsuTheOrcust, DingirsuSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DingirsuTheOrcust, DingirsuActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.TheZombieVampire, ZombieVampireSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TheZombieVampire, ZombieVampireActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Number68Sanaphond, SanaphondSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Number68Sanaphond);

            // Link & Comeback Bosses
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.TYPHONSkyCrisis, TYPHONSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TYPHONSkyCrisis);

            // ==========================================
            // PRIORITY 7: SPELL/TRAP SETS
            // ==========================================
            AddExecutor(ExecutorType.SpellSet, CardId.SunGodDomination);
            AddExecutor(ExecutorType.SpellSet, CardId.EgyptianGodSlimeProtector);
            AddExecutor(ExecutorType.SpellSet, CardId.TheImmortalSunGod);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
        }

        // ============================================================
        // ACTIVATION & SUMMON LOGIC
        // ============================================================

        private bool ImsetyActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (BaitPlanner.ShouldBaitFirst(Card, Bot, Enemy.Hand.Count, Duel.Turn, false, Duel.Turn == 1))
                    return false;

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
            return Bot.HasInSpellZone(CardId.KingsSarcophagus);
        }

        private bool TheTrueSunGodActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (BaitPlanner.ShouldBaitFirst(Card, Bot, Enemy.Hand.Count, Duel.Turn, false, Duel.Turn == 1))
                    return false;
            }
            return true;
        }

        private bool TheSunGodLeadingActivate()
        {
            // Dump Ra from Deck to GY to search Sun God monster
            return true;
        }

        private bool HelpoemerActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (BaitPlanner.ShouldBaitFirst(Card, Bot, Enemy.Hand.Count, Duel.Turn, false, Duel.Turn == 1))
                    return false;
            }
            // Discard 1 card to special summon and search
            return Bot.Hand.Count >= 2;
        }

        private bool MakyuraActivate()
        {
            return true;
        }

        private bool TheImmortalSunGodActivate()
        {
            return Bot.HasInGraveyard(CardId.TheWingedDragonOfRa);
        }

        private bool RaDarkSunGodActivate()
        {
            // Quick Effect during Main Phase: Discard Sun God cards to send opp MMZ monsters to GY
            if (Card.Location == CardLocation.MonsterZone && Duel.IsMainPhase())
            {
                bool hasSunGodInHand = Bot.Hand.Any(c => c != null && (c.HasSetcode(0x1E6) || c.Id == CardId.TheWingedDragonOfRa));
                if (hasSunGodInHand && Enemy.GetMonsters().Any(m => m != null && m.Sequence >= 0 && m.Sequence < 5))
                    return true;
            }

            // Attack declaration LP payment for game
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
            {
                if (Bot.LifePoints > 1000 && Card.IsFaceup())
                    return true;
            }

            return false;
        }

        private bool SunGodDominationActivate()
        {
            // Steal all opponent monsters if Ra is on our field
            bool hasRa = Bot.HasInMonstersZone(CardId.TheWingedDragonOfRa) || Bot.HasInMonstersZone(CardId.RaSunGodOfDarkness);
            return hasRa && Enemy.GetMonsterCount() > 0;
        }

        private bool EgyptianGodSlimeActivate()
        {
            return true;
        }

        private bool LavaGolemSpSummon()
        {
            // Go-Second Board Breaking: 1 Sun God we control + 2 face-up monsters on the field
            bool hasSunGod = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.HasSetcode(0x1E6) || m.Id == CardId.TheWingedDragonOfRa));
            int oppFaceupCount = Enemy.GetMonsters().Count(m => m != null && m.IsFaceup());
            return hasSunGod && oppFaceupCount >= 2;
        }

        private bool SuperPolymerizationActivate()
        {
            return Enemy.GetMonsters().Count(m => m != null && m.IsFaceup()) >= 2 && Bot.Hand.Count >= 1;
        }

        private bool Number38Effect()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool Number90Effect()
        {
            return Duel.LastChainPlayer == 1;
        }

        private bool CoachKingSpSummon()
        {
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 3;
        }

        private bool CoachKingActivate()
        {
            return true;
        }

        private bool Number90SpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.Number90GalaxyEyesPhotonLord)) return false;
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 2;
        }

        private bool Number38SpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.Number38HopeHarbinger)) return false;
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 2;
        }

        private bool DingirsuSpSummon()
        {
            if (Bot.HasInMonstersZone(CardId.DingirsuTheOrcust)) return false;
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 2 && (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0);
        }

        private bool DingirsuActivate()
        {
            return true;
        }

        private bool ZombieVampireSpSummon()
        {
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
            if (Bot.HasInMonstersZone(CardId.Number68Sanaphond)) return false;
            int lv8NonXyz = Bot.GetMonsters().Count(m => m != null && m.Level == 8 && !m.HasType(CardType.Xyz) && m.IsFaceup());
            return lv8NonXyz >= 2 && Duel.Turn == 1;
        }

        private bool SPLittleKnightSpSummon()
        {
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
                // The Sun God Leading: Send Ra from Deck to GY
                var ra = cards.FirstOrDefault(c => c != null && c.Id == CardId.TheWingedDragonOfRa);
                if (ra != null) return new List<ClientCard> { ra };

                // King's Sarcophagus dumping Horus
                var imsety = cards.FirstOrDefault(c => c != null && c.Id == CardId.ImsetyGloryOfHorus && c.Location == CardLocation.Deck);
                if (imsety != null) return new List<ClientCard> { imsety };

                var hapi = cards.FirstOrDefault(c => c != null && c.Id == CardId.HapiGuidanceOfHorus && c.Location == CardLocation.Deck);
                if (hapi != null) return new List<ClientCard> { hapi };

                var duamutef = cards.FirstOrDefault(c => c != null && c.Id == CardId.DuamutefBlessingOfHorus && c.Location == CardLocation.Deck);
                if (duamutef != null) return new List<ClientCard> { duamutef };

                var qebeh = cards.FirstOrDefault(c => c != null && c.Id == CardId.QebehsenuefProtectionOfHorus && c.Location == CardLocation.Deck);
                if (qebeh != null) return new List<ClientCard> { qebeh };
            }

            // 5. Destruction / Removal targeting opponent (Hint 502 / 503 / 504)
            if (hint == 502 || hint == 503 || hint == 504)
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

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            // Zone selection for opponent's monsters (Lava Golem contact fusion / Ra Darkness wipe)
            if (location == CardLocation.MonsterZone && player == 1)
            {
                var oppMonsters = Enemy.GetMonsters();
                int targetZones = 0;
                foreach (var m in oppMonsters)
                {
                    if (m != null && m.Sequence >= 0 && m.Sequence < 5)
                    {
                        int zoneBit = 1 << m.Sequence;
                        if ((available & zoneBit) > 0)
                            targetZones |= zoneBit;
                    }
                }
                if (targetZones > 0) return targetZones;
            }
            return base.OnSelectPlace(cardId, player, location, available);
        }
    }
}
