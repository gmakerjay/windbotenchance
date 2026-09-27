using System.Collections.Generic;
using System.Linq;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Enums;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    // CARD AUDIT — ExodiaRaHorus (3 Engines, 2 Gods: Millennium Exodia, Horus, and Sun God Marik Ra)
    // 100% verified against cards.cdb and ExodiaRaHorus.ydk
    // ====================================================================================================
    // | Card Name                          | Type           | Key Interaction                                         |
    // |------------------------------------|----------------|---------------------------------------------------------|
    // | Exodia / Forbidden One (5 pieces)  | Normal/Effect  | Win condition / Material revealed by Millennium Ankh    |
    // | The Winged Dragon of Ra            | Divine-Beast   | Dumped by Sun God Continuous Spell -> Revived by Spell  |
    // | Imsety, Glory of Horus             | Spellcaster/8  | Discards 1 -> Searches King's Sarcophagus + Draws 1     |
    // | King's Sarcophagus                 | Continuous Sp  | Sends Horus from Deck to GY; revives all Horus Level 8s |
    // | The Sun God Leading Down           | Continuous Sp  | Sends Ra from Deck to GY -> Searches Sun God monster    |
    // | Makyura, Destructor of Sun God     | Warrior/4      | When sent to GY: Sets Sun God Trap/Quick Spell playable |
    // | Helpoemer, Chanter of Sun God      | Fiend/5        | Discards 1 to SS -> Searches Sun God card               |
    // | The Immortal Sun God               | Quick-Play Sp  | Revives Ra -> Cheats out Sun God of Darkness from Extra |
    // | Sun God of Darkness (Ra Fusion)    | Divine-Beast/10| Unaffected by card effects; Quick send opp monsters     |
    // | Lava Golem, Steelcage of Sun God   | Fiend/8/Fusion | Contact Fusion on opp field: 1 Sun God + 2 opp monsters |
    // | Millennium Ankh                    | Normal Spell   | Reveals 5 Forbidden Ones -> SS Exodia Incarnate         |
    // | The Unstoppable Exodia Incarnate   | Spellcaster/10 | Cannot be destroyed; gains LP as ATK; negates Sp/Tr     |
    // | Coach King Giantrainer             | Rank 8 Xyz     | 3 Level 8s -> Draws 3 cards + Burns 800 per monster     |
    // | Number 90 / Number 38              | Rank 8 Xyz     | Monster / Spell negation boss monsters                  |
    // ====================================================================================================

    [Deck("ExodiaRaHorus", "ExodiaRaHorus")]
    public class ExodiaRaHorusExecutor : ModernExecutor
    {
        public class CardId
        {
            // --- EXODIA / FORBIDDEN ONE ---
            public const int LeftArm = 7902349;
            public const int LeftLeg = 44519536;
            public const int RightArm = 70903634;
            public const int RightLeg = 8124921;
            public const int ExodiaTheForbiddenOne = 33396948;
            public const int TheUnstoppableExodiaIncarnate = 83257450;
            public const int MillenniumAnkh = 37613663;
            public const int Obliterate = 64043465;

            // --- HORUS ENGINE ---
            public const int ImsetyGloryOfHorus = 84941194;
            public const int QebehsenuefProtectionOfHorus = 74725513;
            public const int HapiGuidanceOfHorus = 47330808;
            public const int DuamutefBlessingOfHorus = 11335209;
            public const int KingsSarcophagus = 16528181;

            // --- SUN GOD / RA ENGINE ---
            public const int TheWingedDragonOfRa = 10000010;
            public const int RaSunGodOfDarkness = 101403030;
            public const int LavaGolemSunGod = 101403031;
            public const int HelpoemerChanter = 101403001;
            public const int MakyuraDestructor = 101403002;
            public const int GilGarthTerrorMachine = 101403003;
            public const int ViserDesShock = 101403004;
            public const int TheImmortalSunGod = 101403051;
            public const int TheSunGodLeadingDownIntoDarkness = 101403052;
            public const int SunGodDomination = 101403072;
            public const int EgyptianGodSlimeSunGod = 101403073;
            public const int TheTrueSunGod = 11587414;

            // --- MILLENNIUM ENGINE ---
            public const int SengenjinMillennium = 38775407;
            public const int GolemMillennium = 74169516;
            public const int ShieldMillennium = 1164211;
            public const int WedjuTemple = 63017368;
            public const int HeartOfTheBlueEyes = 54475145;

            // --- HANDTRAPS & STAPLES ---
            public const int AshBlossom = 14558127;
            public const int DrollAndLockBird = 94145021;
            public const int EffectVeiler = 97268402;
            public const int InfiniteImpermanence = 10045474;
            public const int GalaxyCyclone = 5133471;

            // --- EXTRA DECK ---
            public const int CoachKingGiantrainer = 30741334;
            public const int Number90PhotonLord = 8165596;
            public const int Number38HopeHarbinger = 63767246;
            public const int Number23Lancelot = 66547759;
            public const int Number60Dugares = 66011101;
            public const int Number41Bagooska = 90590303;
            public const int Dingirsu = 93717133;
            public const int TYPHON = 93039339;
            public const int AAZeus = 90448279;
            public const int SPLittleKnight = 29301450;
            public const int IPMasquerena = 65741786;
        }

        private readonly ExodiaRaHorusPlugin _plugin;
        private bool _ankhActivatedThisTurn;

        public ExodiaRaHorusExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _plugin = new ExodiaRaHorusPlugin(this);
            DeckPlugin = _plugin;

            HeuristicGuard.RegisterAceCards(
                CardId.TheUnstoppableExodiaIncarnate,
                CardId.RaSunGodOfDarkness,
                CardId.CoachKingGiantrainer,
                CardId.Number90PhotonLord,
                CardId.Number38HopeHarbinger
            );

            // ==========================================
            // PRIORITY 1: HANDTRAPS & NEGATIONS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DefaultDrollAndLockBird);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, DefaultEffectVeiler);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, DefaultInfiniteImpermanence);

            // ==========================================
            // PRIORITY 2: BOSS QUICK EFFECTS & INTERRUPTIONS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.Number38HopeHarbinger, HopeHarbingerEffect);
            AddExecutor(ExecutorType.Activate, CardId.Number90PhotonLord, PhotonLordEffect);
            AddExecutor(ExecutorType.Activate, CardId.RaSunGodOfDarkness, RaDarknessQuickEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheUnstoppableExodiaIncarnate, ExodiaIncarnateNegate);
            AddExecutor(ExecutorType.Activate, CardId.Obliterate, ObliterateEffect);

            // ==========================================
            // PRIORITY 3: BOARD BREAKING (Lava Golem Contact Fusion)
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.LavaGolemSunGod, LavaGolemContactFusion);

            // ==========================================
            // PRIORITY 4: SUN GOD SETUP & SEARCH (Pre-Ankh)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.TheTrueSunGod, TheTrueSunGodEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheSunGodLeadingDownIntoDarkness, SunGodLeadingEffect);
            AddExecutor(ExecutorType.Activate, CardId.MakyuraDestructor, MakyuraEffect);
            AddExecutor(ExecutorType.Activate, CardId.HelpoemerChanter, HelpoemerEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheImmortalSunGod, ImmortalSunGodEffect);
            AddExecutor(ExecutorType.Activate, CardId.EgyptianGodSlimeSunGod, GodSlimeTrapEffect);
            AddExecutor(ExecutorType.Activate, CardId.SunGodDomination, SunGodDominationEffect);

            // ==========================================
            // PRIORITY 5: HORUS ENGINE (Pre-Ankh)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.ImsetyGloryOfHorus, ImsetyHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.KingsSarcophagus, KingsSarcophagusEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ImsetyGloryOfHorus, HorusReviveEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.HapiGuidanceOfHorus, HorusReviveEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.QebehsenuefProtectionOfHorus, HorusReviveEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DuamutefBlessingOfHorus, HorusReviveEffect);

            // ==========================================
            // PRIORITY 6: RANK 8 XYZ SUMMONS (Pre-Ankh)
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.CoachKingGiantrainer, CoachKingSummonCheck);
            AddExecutor(ExecutorType.Activate, CardId.CoachKingGiantrainer, CoachKingDrawEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Number90PhotonLord, PhotonLordSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.Number38HopeHarbinger, HopeHarbingerSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.Number23Lancelot, LancelotSummonCheck);

            // ==========================================
            // PRIORITY 7: MILLENNIUM SETUP & EXODIA INCARNATE (Last)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.WedjuTemple, WedjuTempleEffect);
            AddExecutor(ExecutorType.Activate, CardId.SengenjinMillennium, MillenniumPlaceEffect);
            AddExecutor(ExecutorType.Activate, CardId.GolemMillennium, MillenniumPlaceEffect);
            AddExecutor(ExecutorType.Activate, CardId.ShieldMillennium, MillenniumPlaceEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SengenjinMillennium, MillenniumSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.GolemMillennium, MillenniumSummonCheck);

            // Millennium Ankh: Finale Play
            AddExecutor(ExecutorType.Activate, CardId.MillenniumAnkh, MillenniumAnkhEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.TheUnstoppableExodiaIncarnate, ExodiaSummonCheck);

            // ==========================================
            // PRIORITY 8: SET BACKROW & CLEANUP
            // ==========================================
            AddExecutor(ExecutorType.SpellSet, CardId.SunGodDomination);
            AddExecutor(ExecutorType.SpellSet, CardId.EgyptianGodSlimeSunGod);
            AddExecutor(ExecutorType.SpellSet, CardId.TheImmortalSunGod);
            AddExecutor(ExecutorType.SpellSet, CardId.Obliterate);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);

            // Link Recovery / Board Control
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummonCheck);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
        }

        public override void OnNewTurn()
        {
            _ankhActivatedThisTurn = false;
            _plugin.StrategyImpl.AnkhActivatedThisTurn = false;
            base.OnNewTurn();
        }

        // ============================================================
        // EXECUTION CONDITIONS & SEQUENCING
        // ============================================================

        private bool SunGodLeadingEffect()
        {
            // Send Ra from Deck to GY to search Sun God monster
            return true;
        }

        private bool TheTrueSunGodEffect()
        {
            return true;
        }

        private bool MakyuraEffect()
        {
            // When sent to GY, sets Sun God Spell/Trap ready to activate
            return true;
        }

        private bool HelpoemerEffect()
        {
            // Discard 1 card to Special Summon Helpoemer and search Sun God
            return Bot.Hand.Count >= 2;
        }

        private bool ImmortalSunGodEffect()
        {
            // Revive Ra from GY and cheat out Sun God of Darkness!
            return Bot.HasInGraveyard(CardId.TheWingedDragonOfRa);
        }

        private bool RaDarknessQuickEffect()
        {
            // Quick Effect during Main Phase: Discard Sun God cards to send opp monsters to GY
            if (Duel.Player == 1 && Enemy.GetMonsterCount() > 0)
                return true;

            // Attack declaration LP payment for game
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
            {
                if (Bot.LifePoints > 1000 && Enemy.GetMonsterCount() == 0)
                    return true;
            }

            return false;
        }

        private bool LavaGolemContactFusion()
        {
            // Go-Second Board Breaking: 1 Sun God we control + 2 face-up monsters on the field
            return Enemy.GetMonsterCount() >= 2 && Bot.GetMonsters().Any(m => m != null && m.HasSetcode(0x1E6));
        }

        private bool GodSlimeTrapEffect()
        {
            return true;
        }

        private bool SunGodDominationEffect()
        {
            // Steal all opponent monsters if Ra is on field
            return Bot.HasInMonstersZone(CardId.TheWingedDragonOfRa) || Bot.HasInMonstersZone(CardId.RaSunGodOfDarkness);
        }

        private bool ImsetyHandEffect()
        {
            // Discard itself + 1 card from hand to get King's Sarcophagus
            return Bot.Hand.Count >= 2;
        }

        private bool KingsSarcophagusEffect()
        {
            // Discard 1 to send Horus from deck to GY
            var horusInDeck = Bot.Deck.Any(c => c != null && (c.Id == CardId.ImsetyGloryOfHorus || c.Id == CardId.HapiGuidanceOfHorus || c.Id == CardId.QebehsenuefProtectionOfHorus || c.Id == CardId.DuamutefBlessingOfHorus));
            return horusInDeck && Bot.Hand.Count >= 1;
        }

        private bool HorusReviveEffect()
        {
            // If King's Sarcophagus is on field and not locked by Ankh, revive Level 8s
            return !_ankhActivatedThisTurn && Bot.HasInSpellZone(CardId.KingsSarcophagus);
        }

        private bool CoachKingSummonCheck()
        {
            // 3 Level 8 monsters -> Draw 3 cards!
            if (_ankhActivatedThisTurn) return false;
            return Bot.GetMonsters().Count(m => m != null && m.Level == 8 && m.IsFaceup()) >= 3;
        }

        private bool CoachKingDrawEffect()
        {
            return true;
        }

        private bool PhotonLordSummonCheck()
        {
            if (_ankhActivatedThisTurn) return false;
            return Bot.GetMonsters().Count(m => m != null && m.Level == 8 && m.IsFaceup()) >= 2;
        }

        private bool HopeHarbingerSummonCheck()
        {
            if (_ankhActivatedThisTurn) return false;
            return Bot.GetMonsters().Count(m => m != null && m.Level == 8 && m.IsFaceup()) >= 2;
        }

        private bool LancelotSummonCheck()
        {
            if (_ankhActivatedThisTurn) return false;
            return Bot.GetMonsters().Count(m => m != null && m.Level == 8 && m.IsFaceup()) >= 2;
        }

        private bool WedjuTempleEffect()
        {
            return true;
        }

        private bool MillenniumPlaceEffect()
        {
            // Only place if we have healthy LP (> 3000)
            return Bot.LifePoints > 3000;
        }

        private bool MillenniumSummonCheck()
        {
            return true;
        }

        private bool MillenniumAnkhEffect()
        {
            // Activate Millennium Ankh as finale: drops 4000+ ATK unaffected Exodia
            // Best executed AFTER Horus XYZ and Sun God setups are completed!
            _ankhActivatedThisTurn = true;
            _plugin.StrategyImpl.AnkhActivatedThisTurn = true;
            return true;
        }

        private bool ExodiaSummonCheck()
        {
            return true;
        }

        private bool ExodiaIncarnateNegate()
        {
            // Spell/Trap negation by setting Obliterate from Deck
            return Duel.LastChainPlayer == 1;
        }

        private bool ObliterateEffect()
        {
            // Bounce opponent monster
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

        private bool SPLittleKnightSummonCheck()
        {
            if (_ankhActivatedThisTurn) return false;
            return Bot.GetMonsterCount() >= 2 && (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) > 0;
        }

        private bool SPLittleKnightEffect()
        {
            return true;
        }

        // ============================================================
        // SMART TARGET SELECTION & HINT RESOLUTION
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
                // If selecting Ra to send to GY for The Sun God Leading Down
                var ra = cards.FirstOrDefault(c => c != null && c.Id == CardId.TheWingedDragonOfRa);
                if (ra != null) return new List<ClientCard> { ra };

                // Horus sending from deck to GY
                var imsety = cards.FirstOrDefault(c => c != null && c.Id == CardId.ImsetyGloryOfHorus && c.Location == CardLocation.Deck);
                if (imsety != null) return new List<ClientCard> { imsety };

                var hapi = cards.FirstOrDefault(c => c != null && c.Id == CardId.HapiGuidanceOfHorus && c.Location == CardLocation.Deck);
                if (hapi != null) return new List<ClientCard> { hapi };
            }

            // 5. Destruction / Removal targeting opponent (Hint 502 / 503 / 504)
            if (hint == 502 || hint == 503 || hint == 504)
            {
                var enemyCards = cards.Where(c => c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    return enemyCards.OrderByDescending(c => _plugin.ThreatImpl.EvaluateThreatScore(c)).Take(max).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            // Rule 14: Reject opponent effects by default
            if (card != null && card.Controller == 1)
                return false;

            return base.OnSelectEffectYn(card, desc);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
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
