using System.Linq;

namespace WindBot.Game.AI
{
    public class OpponentThreatAnalyzer : IBoardAnalyzer
    {
        public void Analyze(BoardAnalysisContext context, AnalysisScoreVector score)
        {
            if (context == null || context.Enemy == null || context.Scorer == null) return;
            score.OpponentThreat = context.Enemy.GetMonsters()
                .Where(c => c != null)
                .Sum(c => context.Scorer.ThreatScore(c));
        }
    }
}
