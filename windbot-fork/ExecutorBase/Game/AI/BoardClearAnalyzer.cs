using System.Linq;

namespace WindBot.Game.AI
{
    public class BoardClearAnalyzer : IBoardAnalyzer
    {
        public void Analyze(BoardAnalysisContext context, AnalysisScoreVector score)
        {
            if (context == null || context.Enemy == null || context.Scorer == null) return;
            int clearValue = context.Enemy.GetMonsters().Where(c => c != null).Sum(c => context.Scorer.ThreatScore(c));
            if (clearValue >= 80) score.BoardPressure -= clearValue / 10;
        }
    }
}
