using System.Collections.Generic;
using System.Linq;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    // ============================================================================
    // DECOUPLED DOMAIN PLUGIN ARCHITECTURE: HorusExodiaPlugin
    // ============================================================================
    public class HorusExodiaPlugin : DeckPluginBase
    {
        private readonly HorusExodiaExecutor _exec;

        public override string DeckName => "HorusExodia";

        public HorusExodiaStrategy StrategyImpl { get; }
        public HorusExodiaMaterialEvaluator MaterialImpl { get; }
        public HorusExodiaThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public HorusExodiaPlugin(HorusExodiaExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new HorusExodiaStrategy(exec);
            MaterialImpl = new HorusExodiaMaterialEvaluator(exec);
            ThreatImpl = new HorusExodiaThreatEvaluator(exec);
        }
    }

    public class HorusExodiaStrategy : IDeckStrategy
    {
        private readonly HorusExodiaExecutor _exec;
        public bool AnkhActivatedThisTurn { get; set; }

        public HorusExodiaStrategy(HorusExodiaExecutor exec) => _exec = exec;

        public void Reset()
        {
            AnkhActivatedThisTurn = false;
        }

        public int GetTurnPlayRoute()
        {
            return _exec.Duel.Turn == 1 ? 1 : 2;
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context = null)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. King's Sarcophagus (if not on field or in hand)
            if (!_exec.Bot.HasInSpellZone(HorusExodiaExecutor.CardId.KingsSarcophagus) &&
                !_exec.Bot.HasInHand(HorusExodiaExecutor.CardId.KingsSarcophagus))
            {
                var sarc = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.KingsSarcophagus);
                if (sarc != null) return sarc;
            }

            // 2. Millennium Ankh (Key starter for Exodia Incarnate)
            if (!_exec.Bot.HasInHand(HorusExodiaExecutor.CardId.MillenniumAnkh))
            {
                var ankh = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.MillenniumAnkh);
                if (ankh != null) return ankh;
            }

            // 3. Wedju Temple (Field setup)
            if (!_exec.Bot.HasInSpellZone(HorusExodiaExecutor.CardId.WedjuTemple) &&
                !_exec.Bot.HasInHand(HorusExodiaExecutor.CardId.WedjuTemple))
            {
                var temple = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.WedjuTemple);
                if (temple != null) return temple;
            }

            // 4. Shield of the Millennium Dynasty (Searches Ankh upon SS)
            if (!_exec.Bot.HasInHand(HorusExodiaExecutor.CardId.ShieldMillenniumDynasty) &&
                !_exec.Bot.HasInSpellZone(HorusExodiaExecutor.CardId.ShieldMillenniumDynasty))
            {
                var shield = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.ShieldMillenniumDynasty);
                if (shield != null) return shield;
            }

            // 5. Sengenjin Wakes from a Millennium (Level 8 body + searches Millennium)
            var sengenjin = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.SengenjinMillennium);
            if (sengenjin != null && !_exec.Bot.HasInHand(HorusExodiaExecutor.CardId.SengenjinMillennium)) return sengenjin;

            // 6. Golem That Guards the Millennium Treasures
            var golem = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.GolemMillennium);
            if (golem != null && !_exec.Bot.HasInHand(HorusExodiaExecutor.CardId.GolemMillennium)) return golem;

            // 7. Imsety, Glory of Horus
            var imsety = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.ImsetyGloryOfHorus);
            if (imsety != null && !_exec.Bot.HasInHand(HorusExodiaExecutor.CardId.ImsetyGloryOfHorus)) return imsety;

            // 8. Obliterate!!!
            var obliterate = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.Obliterate);
            if (obliterate != null && !_exec.Bot.HasInHand(HorusExodiaExecutor.CardId.Obliterate) && !_exec.Bot.HasInSpellZone(HorusExodiaExecutor.CardId.Obliterate)) return obliterate;

            // 9. Forbidden One pieces
            var pieces = candidates.Where(c => c != null &&
                (c.Id == HorusExodiaExecutor.CardId.ExodiaForbiddenOne ||
                 c.Id == HorusExodiaExecutor.CardId.LeftArmForbiddenOne ||
                 c.Id == HorusExodiaExecutor.CardId.LeftLegForbiddenOne ||
                 c.Id == HorusExodiaExecutor.CardId.RightArmForbiddenOne ||
                 c.Id == HorusExodiaExecutor.CardId.RightLegForbiddenOne)).ToList();
            if (pieces.Count > 0) return pieces.First();

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Ace Fusion: The Unstoppable Exodia Incarnate
            var exodia = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.TheUnstoppableExodiaIncarnate);
            if (exodia != null) return exodia;

            // 2. Millennium monsters from S/T Zone
            var sengenjin = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.SengenjinMillennium);
            if (sengenjin != null) return sengenjin;

            var shield = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.ShieldMillenniumDynasty);
            if (shield != null) return shield;

            var golem = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.GolemMillennium);
            if (golem != null) return golem;

            var maiden = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.MaidenMillenniumMoon);
            if (maiden != null) return maiden;

            // 3. Horus Core Revives
            var imsety = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.ImsetyGloryOfHorus);
            if (imsety != null) return imsety;

            var hapi = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.HapiGuidanceOfHorus);
            if (hapi != null) return hapi;

            var duamutef = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.DuamutefBlessingOfHorus);
            if (duamutef != null) return duamutef;

            var qebeh = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.QebehsenuefProtectionOfHorus);
            if (qebeh != null) return qebeh;

            // 4. Rank 8 Xyz Bosses
            var coach = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.CoachKingGiantrainer);
            if (coach != null) return coach;

            var photonLord = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.Number90GalaxyEyesPhotonLord);
            if (photonLord != null) return photonLord;

            var hope = candidates.FirstOrDefault(c => c != null && c.Id == HorusExodiaExecutor.CardId.Number38HopeHarbinger);
            if (hope != null) return hope;

            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class HorusExodiaMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly HorusExodiaExecutor _exec;

        public HorusExodiaMaterialEvaluator(HorusExodiaExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Never sacrifice or link away boss monsters
            if (card.Id == HorusExodiaExecutor.CardId.TheUnstoppableExodiaIncarnate) return 60000;
            if (card.Id == HorusExodiaExecutor.CardId.Number90GalaxyEyesPhotonLord) return 45000;
            if (card.Id == HorusExodiaExecutor.CardId.Number38HopeHarbinger) return 45000;
            if (card.Id == HorusExodiaExecutor.CardId.CoachKingGiantrainer) return 35000;
            if (card.Id == HorusExodiaExecutor.CardId.Number23Lancelot) return 35000;
            if (card.Id == HorusExodiaExecutor.CardId.DingirsuTheOrcust) return 30000;

            if (_exec.IsAceCard(card))
                return 25000;

            // High-power monsters on field (ATK >= 2000) should attack rather than be linked away
            if (card.Location == CardLocation.MonsterZone && card.Attack >= 2000)
                return 5000;

            // Discard priority from hand (Hint 501 / Costs):
            // Horus pieces in hand want to be discarded for Sarcophagus
            if (card.Location == CardLocation.Hand &&
                (card.Id == HorusExodiaExecutor.CardId.HapiGuidanceOfHorus ||
                 card.Id == HorusExodiaExecutor.CardId.DuamutefBlessingOfHorus ||
                 card.Id == HorusExodiaExecutor.CardId.QebehsenuefProtectionOfHorus))
                return 10;

            // Vanilla Exodia pieces are highly discardable (Ankh reveals from GY too! Obliterate can recycle them)
            if (card.Location == CardLocation.Hand &&
                (card.Id == HorusExodiaExecutor.CardId.LeftArmForbiddenOne ||
                 card.Id == HorusExodiaExecutor.CardId.LeftLegForbiddenOne ||
                 card.Id == HorusExodiaExecutor.CardId.RightArmForbiddenOne ||
                 card.Id == HorusExodiaExecutor.CardId.RightLegForbiddenOne ||
                 card.Id == HorusExodiaExecutor.CardId.ExodiaForbiddenOne))
                return 15;

            // Duplicate spells in hand
            if (card.Location == CardLocation.Hand &&
                _exec.Bot.Hand.Count(c => c != null && c.Id == card.Id) > 1)
                return 25;

            return 100;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.Where(c => c != null).OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class HorusExodiaThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly HorusExodiaExecutor _exec;

        public HorusExodiaThreatEvaluator(HorusExodiaExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;

            if (card.HasType(CardType.Monster))
            {
                if (card.Attack >= 3000) score += 3500;
                else if (card.Attack >= 2000) score += 2000;
                if (card.HasType(CardType.Fusion | CardType.Synchro | CardType.Xyz | CardType.Link)) score += 2500;
                if (card.IsFaceup()) score += 1000;
            }

            if (card.HasType(CardType.Spell | CardType.Trap))
            {
                // Board wipe cards
                if (card.Id == 18144506 || card.Id == 12580477 || card.Id == 15693423 || card.Id == 53582587) // Feather Duster, Lightning Storm, Evenly Matched, Torrential
                    score += 6000;
                else if (card.HasType(CardType.Continuous | CardType.Field))
                    score += 2500;
                else
                    score += 1500;
            }

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return EvaluateThreatScore(card) >= 4000;
        }

        public ClientCard PickHighestThreat(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderByDescending(c => EvaluateThreatScore(c)).FirstOrDefault();
        }
    }
}
