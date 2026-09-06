using System;

namespace WindBot.Game.AI.DecisionEngine
{
    /// <summary>
    /// Evaluates the counterfactual question: "What happens if we DO NOT activate this card now?"
    /// Simulates expected win-rate outcomes: E(W | Activate) vs E(W | Hold).
    /// </summary>
    public class CounterfactualSimulator
    {
        /// <summary>
        /// Heuristically evaluates whether we should hold our interactive resource (return true)
        /// or activate it (return false) by comparing expected win rates.
        /// </summary>
        public bool ShouldHold(
            double targetThreat,          // Threat score of opponent's target card (0-100)
            double opponentCounterProb,   // Probability opponent has a counter (0-1)
            int ourInteractionsCount,     // How many disruptions we have left
            int opponentHandCount,        // Cards left in opponent's hand
            bool isFirstActionThisTurn     // Is this the opponent's first play this turn?
        )
        {
            double baseWinRate = 50.0;

            // --- 1. Calculate Expected Win Rate if we ACTIVATE ---
            // If the negate succeeds: we stop a threat.
            double winIfSuccess = baseWinRate + (targetThreat * 0.25);
            // If the negate is countered (e.g., Called by / negate monster): we lose resource and threat resolves.
            double winIfCountered = baseWinRate - (targetThreat * 0.20);
            
            // Expected Value (Activate)
            double evActivate = (winIfSuccess * (1.0 - opponentCounterProb)) + (winIfCountered * opponentCounterProb);

            // --- 2. Calculate Expected Win Rate if we HOLD ---
            // If we hold, the current threat resolves (hurts our win rate).
            double winIfResolved = baseWinRate - (targetThreat * 0.35);
            
            // However, we retain the card for a future target.
            // The value of saving the card is higher if the opponent has many cards left in hand
            // (likely has a bigger threat coming) and we have few interactions.
            double holdingPremium = 0.0;
            if (opponentHandCount >= 3)
            {
                // High probability of a better target coming up
                holdingPremium += (100.0 - targetThreat) * 0.30;
            }
            if (ourInteractionsCount <= 1)
            {
                // Last resource — saving it has high value if current target isn't lethal
                holdingPremium += 12.0;
            }

            // Opponent's first action is often bait — premium for holding is higher
            if (isFirstActionThisTurn && targetThreat < 70)
            {
                holdingPremium += 15.0;
            }

            double evHold = winIfResolved + holdingPremium;

            // Output simulation logs
            System.Diagnostics.Debug.WriteLine($"[Counterfactual] TargetThreat={targetThreat:F1}, CounterProb={opponentCounterProb:F2} | EV(Act)={evActivate:F1}%, EV(Hold)={evHold:F1}%");

            // If expected win rate of holding is greater than activating, we hold.
            return evHold > evActivate;
        }
    }
}
