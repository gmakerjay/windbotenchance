namespace WindBot.Game.AI
{
    public interface IBoardAnalyzer
    {
        void Analyze(BoardAnalysisContext context, AnalysisScoreVector score);
    }
}
