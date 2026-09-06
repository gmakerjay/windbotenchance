using YGOSharp.OCGWrapper.Enums;
using WindBot.Game;

namespace WindBot.Game.AI
{
    public class CombatTradeAnalyzer : IBoardAnalyzer
    {
        public void Analyze(BoardAnalysisContext context, AnalysisScoreVector score)
        {
        }

        public bool ShouldTradeForBoard(BoardAnalysisContext context, ClientCard attacker, ClientCard defender)
        {
            if (context == null || context.Scorer == null || attacker == null || defender == null) return false;
            if (!attacker.IsAttack() || defender.IsFacedown() || !defender.IsAttack()) return false;

            if (defender.IsMonsterDangerous() && !defender.IsDisabled()) return false;
            if (defender.IsMonsterInvincible() && !defender.IsDisabled()) return false;
            if (attacker.Attack != defender.Attack) return false;

            int attackerValue = EstimateCardValue(context, attacker, true);
            int defenderThreat = EstimateCardValue(context, defender, false);

            if (context.Executor != null && context.Executor.IsAceCard(attacker) && defenderThreat < attackerValue + 20)
                return false;

            if (context.Scorer.HasLethalAfterRemoval(1)) return true;
            if (defender.IsFloodgate()) return true;
            if (IsExtraDeckMonster(defender)) return true;
            if (defenderThreat >= attackerValue + 10) return true;

            return defenderThreat >= 45 && attackerValue <= defenderThreat;
        }

        private static int EstimateCardValue(BoardAnalysisContext context, ClientCard card, bool ours)
        {
            int value = context.Scorer.ThreatScore(card);
            if (ours && context.Executor != null && context.Executor.IsAceCard(card)) value += 25;
            if (card.Attack >= 2500) value += 10;
            return value;
        }

        private static bool IsExtraDeckMonster(ClientCard card)
        {
            return card.HasType(CardType.Fusion) || card.HasType(CardType.Synchro) ||
                   card.HasType(CardType.Xyz) || card.HasType(CardType.Link);
        }
    }
}
