using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    // ═══════════════════════════════════════════════════════════════
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: GravePlugin
    // ═══════════════════════════════════════════════════════════════
    public class GravePlugin : DeckPluginBase
    {
        private readonly _2026_GraveExecutor _exec;

        public override string DeckName => "Grave";

        public GraveStrategy StrategyImpl { get; }
        public GraveMaterialEvaluator MaterialImpl { get; }
        public GraveThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public GravePlugin(_2026_GraveExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new GraveStrategy(exec);
            MaterialImpl = new GraveMaterialEvaluator(exec);
            ThreatImpl = new GraveThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
        }
    }

    public class GraveStrategy : IDeckStrategy
    {
        private readonly _2026_GraveExecutor _exec;
        public GraveStrategy(_2026_GraveExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;
            int[] priorities = {
                _2026_GraveExecutor.CardId.K9_66aJokul,
                _2026_GraveExecutor.CardId.K9_04Noroi,
                _2026_GraveExecutor.CardId.K9_66bLantern,
                _2026_GraveExecutor.CardId.K9_17Izuna,
                _2026_GraveExecutor.CardId.K9_XWerewolf
            };
            foreach (int id in priorities)
            {
                var card = candidates.FirstOrDefault(c => c != null && c.Id == id);
                if (card != null) return card;
            }
            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;
            if (context != null && context.Id == _2026_GraveExecutor.CardId.K9_17Ripper)
            {
                var forced = candidates.FirstOrDefault(c => c.Id == _2026_GraveExecutor.CardId.K9XForcedRelease);
                if (forced != null) return forced;
                var k9Case = candidates.FirstOrDefault(c => c.Id == _2026_GraveExecutor.CardId.ACaseForK9);
                if (k9Case != null) return k9Case;
            }
            if (context != null && context.Id == _2026_GraveExecutor.CardId.ACaseForK9)
            {
                var jokul = candidates.FirstOrDefault(c => c.Id == _2026_GraveExecutor.CardId.K9_66aJokul);
                if (jokul != null) return jokul;
            }
            if (context != null && context.Id == _2026_GraveExecutor.CardId.NecrovalleyThrone)
            {
                var cmd = candidates.FirstOrDefault(c => c.Id == _2026_GraveExecutor.CardId.GravekeeperCommandant);
                if (cmd != null) return cmd;
            }
            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class GraveMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly _2026_GraveExecutor _exec;
        public GraveMaterialEvaluator(_2026_GraveExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;
            if (_exec.IsAceCard(card))
            {
                if (card.Location == CardLocation.MonsterZone && card.IsFaceup())
                    return 10000;
                return 900;
            }
            if (card.IsCode(_2026_GraveExecutor.CardId.AshBlossom, _2026_GraveExecutor.CardId.AshBlossomAlt,
                _2026_GraveExecutor.CardId.MaxxC, _2026_GraveExecutor.CardId.DrollAndLockBird))
                return 950;

            if (card.IsCode(_2026_GraveExecutor.CardId.K9_00Lupis)) return 10;
            if (card.IsCode(_2026_GraveExecutor.CardId.K9_17Izuna)) return 15;
            if (card.IsCode(_2026_GraveExecutor.CardId.K9_66aJokul)) return 20;
            if (card.IsCode(_2026_GraveExecutor.CardId.K9_66bLantern)) return 25;
            if (card.IsCode(_2026_GraveExecutor.CardId.K9_04Noroi)) return 30;
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
            var cmd = candidates.FirstOrDefault(c => c != null && c.Id == _2026_GraveExecutor.CardId.GravekeeperCommandant);
            if (cmd != null) return cmd;

            var gyK9 = candidates.Where(c => c != null && c.Level == 5).OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
            if (gyK9 != null) return gyK9;

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class GraveThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly _2026_GraveExecutor _exec;
        public GraveThreatEvaluator(_2026_GraveExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;
            int score = 0;
            if (CardIntelligence.IsFloodgate(card.Id)) score += 8000;
            if (CardIntelligence.IsKnownNegator(card.Id)) score += 6000;
            if (card.Attack >= 2500) score += 4000;
            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            return CardIntelligence.IsFloodgate(card.Id) || (card.Attack >= 3000);
        }
    }
}
