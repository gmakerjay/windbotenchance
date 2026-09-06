using WindBot.Game;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI
{
    public class PhasePlanAnalyzer : IBoardAnalyzer
    {
        public void Analyze(BoardAnalysisContext context, AnalysisScoreVector score)
        {
            if (context == null || context.Duel == null || context.Bot == null) return;
            score.PreferBattleBeforeSetting = context.Duel.Player == 0 &&
                context.Duel.Phase == DuelPhase.Main1 &&
                context.Bot.HasAttackingMonster();
        }

        public bool ShouldDeferSetUntilMain2(BoardAnalysisContext context, MainPhase main, ClientCard card)
        {
            if (context == null || main == null || card == null) return false;
            if (context.Duel.Phase != DuelPhase.Main1 || !main.CanBattlePhase) return false;
            if (!context.Bot.HasAttackingMonster()) return false;
            if (context.Scorer != null && context.Scorer.HasLethal()) return false;

            return true;
        }
    }
}
