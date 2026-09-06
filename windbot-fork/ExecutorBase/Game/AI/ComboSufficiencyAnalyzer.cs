using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI
{
    /// <summary>
    /// Smart Flow v2: Analyzes whether the bot has built a sufficient board and should stop extending.
    /// 
    /// Tuned for balanced aggression:
    ///   - Going first:  threshold 55 (need real disruptions before stopping)
    ///   - Going second: threshold 75 (need MORE board because we're pushing)
    ///   - Enemy has threats: DON'T stop (deal with them first)
    ///   - Near-lethal damage: stop and attack (75%+ of LP)
    /// </summary>
    public class ComboSufficiencyAnalyzer : IBoardAnalyzer
    {
        public void Analyze(BoardAnalysisContext context, AnalysisScoreVector score)
        {
            if (context == null || context.Bot == null || context.Enemy == null || context.Scorer == null) return;
            if (context.Duel == null) return;

            // Only relevant during our turn
            if (context.Duel.Player != 0) return;

            int sufficiency = context.Scorer.BoardSufficiencyScore();
            score.BoardSufficiency = sufficiency;

            bool shouldStop = false;
            bool isGoingSecond = context.Duel.Turn >= 2;
            bool enemyHasMonsters = context.Enemy.GetMonsterCount() > 0;

            // ── Going First: Build end board, but don't overextend ──
            if (!isGoingSecond)
            {
                // Need real disruptions (threshold 55, not 45)
                // 55 = e.g., 1 boss negate (20) + 1 mid-range monster (10) + 2 backrow (20) + hand trap (5) = 55
                if (sufficiency >= 55)
                    shouldStop = true;
            }
            // ── Going Second: Much higher threshold — we WANT to push ──
            else
            {
                // Don't stop if enemy still has monsters we haven't dealt with
                if (enemyHasMonsters)
                {
                    // Only stop going-second if board is overwhelmingly strong (75+)
                    // AND we have attackers that can actually push through
                    if (sufficiency >= 75 && context.Bot.HasAttackingMonster())
                        shouldStop = true;
                    // Otherwise keep comboing — we need to deal with their board
                }
                else
                {
                    // Enemy board cleared — moderate threshold since we already won the board
                    if (sufficiency >= 60)
                        shouldStop = true;
                }

                // Near-lethal override: if we can deal 75%+ damage, stop and attack
                if (context.Duel.Phase == DuelPhase.Main1)
                {
                    int estimatedDmg = context.Scorer.EstimateDamageIfAttackNow();
                    int enemyLP = context.Enemy.LifePoints;

                    if (estimatedDmg >= enemyLP * 3 / 4 && sufficiency >= 30)
                        shouldStop = true;
                }
            }

            score.ShouldStopExtending = shouldStop;
        }
    }
}
