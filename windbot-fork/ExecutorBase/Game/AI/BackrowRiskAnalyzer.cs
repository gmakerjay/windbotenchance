using System.Linq;

namespace WindBot.Game.AI
{
    public class BackrowRiskAnalyzer : IBoardAnalyzer
    {
        public void Analyze(BoardAnalysisContext context, AnalysisScoreVector score)
        {
            if (context == null || context.Enemy == null) return;
            score.BackrowRisk = context.Enemy.GetSpells().Count(c => c != null && c.IsFacedown()) * 10;
        }
    }
}
