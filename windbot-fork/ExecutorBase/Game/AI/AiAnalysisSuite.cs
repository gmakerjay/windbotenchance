using System.Collections.Generic;
using WindBot.Game;

namespace WindBot.Game.AI
{
    public class AiAnalysisSuite
    {
        private readonly Executor _executor;
        private readonly List<IBoardAnalyzer> _analyzers;

        public BoardAnalysisContext Context { get; private set; }
        public AnalysisScoreVector Current { get; private set; }
        public CombatTradeAnalyzer CombatTrade { get; private set; }
        public PhasePlanAnalyzer PhasePlan { get; private set; }

        public AiAnalysisSuite(Executor executor)
        {
            _executor = executor;
            CombatTrade = new CombatTradeAnalyzer();
            PhasePlan = new PhasePlanAnalyzer();
            _analyzers = new List<IBoardAnalyzer>
            {
                new OpponentThreatAnalyzer(),
                new BoardPressureAnalyzer(),
                new BoardClearAnalyzer(),
                new LethalWindowAnalyzer(),
                new BackrowRiskAnalyzer(),
                new ResourceValueAnalyzer(),
                CombatTrade,
                PhasePlan,
                // Smart Flow v2 analyzers
                new AttackOpportunityAnalyzer(),
                new ComboSufficiencyAnalyzer()
            };
            Refresh();
        }

        public void Refresh()
        {
            Context = new BoardAnalysisContext(_executor);
            Current = new AnalysisScoreVector();
            foreach (var analyzer in _analyzers)
            {
                try
                {
                    analyzer.Analyze(Context, Current);
                }
                catch
                {
                    // Some tests construct partial duel fields; analysis is optional and must not break legacy paths.
                }
            }
        }

        public bool ShouldTradeForBoard(ClientCard attacker, ClientCard defender)
        {
            try
            {
                return CombatTrade.ShouldTradeForBoard(Context, attacker, defender);
            }
            catch
            {
                return false;
            }
        }

        public bool ShouldDeferSetUntilMain2(MainPhase main, ClientCard card)
        {
            try
            {
                return PhasePlan.ShouldDeferSetUntilMain2(Context, main, card);
            }
            catch
            {
                return false;
            }
        }
    }
}
