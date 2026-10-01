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
    public class NormalMonsterMashPlugin : DeckPluginBase
    {
        private readonly ModernExecutor _executor;
        private readonly string _deckName;

        public override string DeckName => _deckName;

        public NormalMonsterStrategy StrategyImpl { get; }
        public NormalMonsterMaterialEvaluator MaterialImpl { get; }
        public NormalMonsterThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public NormalMonsterMashPlugin(ModernExecutor executor, string deckName = "NormalMonsterMash")
        {
            _executor = executor;
            _deckName = deckName;
            StrategyImpl = new NormalMonsterStrategy(executor);
            MaterialImpl = new NormalMonsterMaterialEvaluator(executor);
            ThreatImpl = new NormalMonsterThreatEvaluator(executor);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }

        public IList<ClientCard> SelectCardLogic(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return null;

            // Attackers / Summon candidates: pick highest ATK
            if (cards.All(c => c.Location == CardLocation.Hand))
            {
                var bestAtk = cards.OrderByDescending(c => c.Attack).Take(max).ToList();
                if (bestAtk.Count >= min)
                    return bestAtk;
            }

            return null;
        }
    }

    public class NormalMonsterStrategy : IDeckStrategy
    {
        private readonly ModernExecutor _exec;
        public NormalMonsterStrategy(ModernExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard contextCard)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }
    }

    public class NormalMonsterMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly ModernExecutor _exec;
        public NormalMonsterMaterialEvaluator(ModernExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 0;
            return c.Attack;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(c => c.Attack).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => c.Attack).FirstOrDefault();
        }
    }

    public class NormalMonsterThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly ModernExecutor _exec;
        public NormalMonsterThreatEvaluator(ModernExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            return card.Attack;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return card.Attack >= 2500;
        }
    }
}
