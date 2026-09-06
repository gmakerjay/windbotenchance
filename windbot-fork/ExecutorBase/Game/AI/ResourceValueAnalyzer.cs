namespace WindBot.Game.AI
{
    public class ResourceValueAnalyzer : IBoardAnalyzer
    {
        public void Analyze(BoardAnalysisContext context, AnalysisScoreVector score)
        {
            if (context == null || context.Bot == null || context.Enemy == null) return;
            int ours = context.Bot.Hand.Count + context.Bot.GetMonsterCount() + context.Bot.GetSpellCount();
            int theirs = context.Enemy.Hand.Count + context.Enemy.GetMonsterCount() + context.Enemy.GetSpellCount();
            score.ResourceAdvantage = ours - theirs;
        }
    }
}
