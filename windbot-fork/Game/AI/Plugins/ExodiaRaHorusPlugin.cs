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
    // DECOUPLED DOMAIN PLUGIN ARCHITECTURE: ExodiaRaHorusPlugin
    // ============================================================================
    public class ExodiaRaHorusPlugin : DeckPluginBase
    {
        private readonly ExodiaRaHorusExecutor _exec;

        public override string DeckName => "ExodiaRaHorus";

        public ExodiaRaHorusStrategy StrategyImpl { get; }
        public ExodiaRaHorusMaterialEvaluator MaterialImpl { get; }
        public ExodiaRaHorusThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public ExodiaRaHorusPlugin(ExodiaRaHorusExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new ExodiaRaHorusStrategy(exec);
            MaterialImpl = new ExodiaRaHorusMaterialEvaluator(exec);
            ThreatImpl = new ExodiaRaHorusThreatEvaluator(exec);
        }
    }

    public class ExodiaRaHorusStrategy : IDeckStrategy
    {
        private readonly ExodiaRaHorusExecutor _exec;
        public bool AnkhActivatedThisTurn { get; set; }

        public ExodiaRaHorusStrategy(ExodiaRaHorusExecutor exec) => _exec = exec;

        public void Reset()
        {
            AnkhActivatedThisTurn = false;
        }

        public int GetTurnPlayRoute()
        {
            // Route 1: First Turn -> Set up Sun God of Darkness + Exodia Incarnate + Rank 8 Omni-negates
            // Route 2: Second Turn -> Lava Golem contact fusion + wipe opponent board + Ra OTK
            return _exec.Duel.Turn == 1 ? 1 : 2;
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context = null)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. King's Sarcophagus for Horus engine
            var sarc = candidates.FirstOrDefault(c => c != null && c.Id == ExodiaRaHorusExecutor.CardId.KingsSarcophagus);
            if (sarc != null) return sarc;

            // 2. Sun God search targets
            var darkLeading = candidates.FirstOrDefault(c => c != null && c.Id == ExodiaRaHorusExecutor.CardId.TheSunGodLeadingDownIntoDarkness);
            if (darkLeading != null) return darkLeading;

            var immortal = candidates.FirstOrDefault(c => c != null && c.Id == ExodiaRaHorusExecutor.CardId.TheImmortalSunGod);
            if (immortal != null) return immortal;

            var helpoemer = candidates.FirstOrDefault(c => c != null && c.Id == ExodiaRaHorusExecutor.CardId.HelpoemerChanter);
            if (helpoemer != null) return helpoemer;

            var makyura = candidates.FirstOrDefault(c => c != null && c.Id == ExodiaRaHorusExecutor.CardId.MakyuraDestructor);
            if (makyura != null) return makyura;

            // 3. Millennium search targets
            var ankh = candidates.FirstOrDefault(c => c != null && c.Id == ExodiaRaHorusExecutor.CardId.MillenniumAnkh);
            if (ankh != null) return ankh;

            var sengenjin = candidates.FirstOrDefault(c => c != null && c.Id == ExodiaRaHorusExecutor.CardId.SengenjinMillennium);
            if (sengenjin != null) return sengenjin;

            var golem = candidates.FirstOrDefault(c => c != null && c.Id == ExodiaRaHorusExecutor.CardId.GolemMillennium);
            if (golem != null) return golem;

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Priority: Sun God of Darkness > Exodia Incarnate > Ra > Horus Level 8s
            var darkRa = candidates.FirstOrDefault(c => c != null && c.Id == ExodiaRaHorusExecutor.CardId.RaSunGodOfDarkness);
            if (darkRa != null) return darkRa;

            var exodia = candidates.FirstOrDefault(c => c != null && c.Id == ExodiaRaHorusExecutor.CardId.TheUnstoppableExodiaIncarnate);
            if (exodia != null) return exodia;

            var imsety = candidates.FirstOrDefault(c => c != null && c.Id == ExodiaRaHorusExecutor.CardId.ImsetyGloryOfHorus);
            if (imsety != null) return imsety;

            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class ExodiaRaHorusMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly ExodiaRaHorusExecutor _exec;
        public ExodiaRaHorusMaterialEvaluator(ExodiaRaHorusExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Never sacrifice or use Bosses as material
            if (card.Id == ExodiaRaHorusExecutor.CardId.TheUnstoppableExodiaIncarnate ||
                card.Id == ExodiaRaHorusExecutor.CardId.RaSunGodOfDarkness ||
                card.Id == ExodiaRaHorusExecutor.CardId.CoachKingGiantrainer ||
                card.Id == ExodiaRaHorusExecutor.CardId.Number90PhotonLord ||
                card.Id == ExodiaRaHorusExecutor.CardId.Number38HopeHarbinger)
                return 50000;

            if (_exec.IsAceCard(card))
                return 20000;

            // Discard priority: Makyura triggers on discard from hand!
            if (card.Location == CardLocation.Hand && card.Id == ExodiaRaHorusExecutor.CardId.MakyuraDestructor)
                return 10;

            // Horus pieces in hand want to be discarded for Sarcophagus or Imsety
            if (card.Location == CardLocation.Hand &&
                (card.Id == ExodiaRaHorusExecutor.CardId.HapiGuidanceOfHorus ||
                 card.Id == ExodiaRaHorusExecutor.CardId.QebehsenuefProtectionOfHorus ||
                 card.Id == ExodiaRaHorusExecutor.CardId.DuamutefBlessingOfHorus))
                return 20;

            // Vanilla Exodia pieces are discardable if we have Sarcophagus / Millennium Ankh from Deck
            if (card.Location == CardLocation.Hand &&
                (card.Id == ExodiaRaHorusExecutor.CardId.LeftArm ||
                 card.Id == ExodiaRaHorusExecutor.CardId.LeftLeg ||
                 card.Id == ExodiaRaHorusExecutor.CardId.RightArm ||
                 card.Id == ExodiaRaHorusExecutor.CardId.RightLeg))
                return 30;

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

    public class ExodiaRaHorusThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly ExodiaRaHorusExecutor _exec;
        public ExodiaRaHorusThreatEvaluator(ExodiaRaHorusExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;

            if (card.HasType(CardType.Monster))
            {
                if (card.Attack >= 3000) score += 3000;
                if (card.HasType(CardType.Fusion | CardType.Synchro | CardType.Xyz | CardType.Link)) score += 2000;
                if (card.IsDestructionImmune()) score += 1500;
            }

            if (card.HasType(CardType.Spell | CardType.Trap))
            {
                // Mass removal spells are highest threat
                if (card.Id == 18144506 || card.Id == 12580477 || card.Id == 15693423) // Feather Duster, Lightning Storm, Evenly Matched
                    score += 5000;
                else
                    score += 1000;
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
