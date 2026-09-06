namespace WindBot.Game.AI
{
    public class BoardPressureAnalyzer : IBoardAnalyzer
    {
        public void Analyze(BoardAnalysisContext context, AnalysisScoreVector score)
        {
            if (context == null || context.Bot == null || context.Enemy == null || context.Scorer == null) return;
            score.BoardPressure = context.Scorer.BoardAdvantageScore();
            score.HasLethal = context.Scorer.HasLethal();
            score.HasLethalAfterOneRemoval = context.Scorer.HasLethalAfterRemoval(1);
        }
    }
}
