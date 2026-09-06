using System.Linq;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI
{
    /// <summary>
    /// Smart Flow v2: Analyzes whether the bot should attack before continuing combos.
    /// Sets ShouldAttackFirst = true when:
    ///   - MP1 + enemy board is empty + we have attackers
    ///   - MP1 + all enemy monsters are beatable + low backrow risk
    /// </summary>
    public class AttackOpportunityAnalyzer : IBoardAnalyzer
    {
        public void Analyze(BoardAnalysisContext context, AnalysisScoreVector score)
        {
            if (context == null || context.Bot == null || context.Enemy == null || context.Scorer == null) return;
            if (context.Duel == null) return;

            // Only relevant during our MP1
            if (context.Duel.Player != 0 || context.Duel.Phase != DuelPhase.Main1) return;

            int attackOpp = context.Scorer.AttackOpportunityScore();
            score.AttackOpportunity = attackOpp;
            score.EstimatedDirectDamage = context.Scorer.EstimateDamageIfAttackNow();

            // Should attack first when:
            //   1. Attack opportunity is high (≥60) — open board or all beatable
            //   2. We actually have attacking monsters
            //   3. Not Turn 1 (can't attack Turn 1)
            bool hasAttackers = context.Bot.HasAttackingMonster();
            bool notFirstTurn = context.Duel.Turn > 1;

            score.ShouldAttackFirst = hasAttackers && notFirstTurn && attackOpp >= 60;
        }
    }
}
