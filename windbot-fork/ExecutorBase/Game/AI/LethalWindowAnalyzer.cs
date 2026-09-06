namespace WindBot.Game.AI
{
    public class LethalWindowAnalyzer : IBoardAnalyzer
    {
        public void Analyze(BoardAnalysisContext context, AnalysisScoreVector score)
        {
            if (context == null || context.Scorer == null) return;
            score.HasLethal = context.Scorer.HasLethal();
            score.HasLethalAfterOneRemoval = context.Scorer.HasLethalAfterRemoval(1);
        }
    }
}
