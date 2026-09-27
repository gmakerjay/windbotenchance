using System;
using System.Collections.Generic;
using System.Linq;

namespace WindBot.Game.AI
{
    /// <summary>
    /// DeckProbability — Mathematical Foundation for Yu-Gi-Oh! Deck and In-Game Decision Making.
    /// Implements Hypergeometric Distribution and Multivariate Hypergeometric Analysis
    /// for opening hand consistency, excavation success, banish risk, and handtrap estimation.
    /// Completely rule-based, deterministic, and executes in O(1) time.
    /// </summary>
    public static class DeckProbability
    {
        /// <summary>
        /// Calculates combinations nCr (n choose r).
        /// Safe for all Yu-Gi-Oh! deck sizes (n <= 60).
        /// </summary>
        public static double Combinations(int n, int r)
        {
            if (r < 0 || r > n) return 0.0;
            if (r == 0 || r == n) return 1.0;
            if (r > n / 2) r = n - r;

            double result = 1.0;
            for (int i = 1; i <= r; i++)
            {
                result *= (n - (i - 1));
                result /= i;
            }
            return result;
        }

        /// <summary>
        /// Standard Hypergeometric Distribution:
        /// Calculates the probability of drawing exactly k successes out of n cards drawn
        /// without replacement from a population of size N containing K successes.
        /// P(X = k) = (K choose k) * ((N - K) choose (n - k)) / (N choose n)
        /// </summary>
        /// <param name="N">Total population size (e.g., remaining cards in deck + hand).</param>
        /// <param name="K">Total number of target cards in the population.</param>
        /// <param name="n">Number of cards drawn/excavated.</param>
        /// <param name="k">Target number of successes.</param>
        public static double Hypergeometric(int N, int K, int n, int k)
        {
            if (N <= 0 || n <= 0 || K < 0 || k < 0) return 0.0;
            if (k > K || (n - k) > (N - K) || n > N || k > n) return 0.0;

            double denom = Combinations(N, n);
            if (denom <= 0.0) return 0.0;

            double num = Combinations(K, k) * Combinations(N - K, n - k);
            return Math.Clamp(num / denom, 0.0, 1.0);
        }

        /// <summary>
        /// Calculates the probability of drawing AT LEAST ONE target card (P >= 1).
        /// P(X >= 1) = 1.0 - P(X == 0)
        /// </summary>
        /// <param name="N">Total cards in deck/pool.</param>
        /// <param name="K">Total copies of the target card in the deck/pool.</param>
        /// <param name="n">Number of cards drawn or excavated.</param>
        public static double AtLeastOne(int N, int K, int n)
        {
            if (N <= 0 || n <= 0 || K <= 0) return 0.0;
            if (n >= N || K >= N) return 1.0;
            if (N - K < n) return 1.0;

            double denom = Combinations(N, n);
            if (denom <= 0.0) return 0.0;

            double pZero = Combinations(N - K, n) / denom;
            return Math.Clamp(1.0 - pZero, 0.0, 1.0);
        }

        /// <summary>
        /// Calculates the probability of drawing AT LEAST 'minSuccesses' target cards.
        /// Sum of P(X = k) for k = minSuccesses .. min(n, K).
        /// </summary>
        public static double AtLeast(int N, int K, int n, int minSuccesses)
        {
            if (minSuccesses <= 0) return 1.0;
            if (minSuccesses > K || minSuccesses > n) return 0.0;

            double sum = 0.0;
            int max = Math.Min(n, K);
            for (int k = minSuccesses; k <= max; k++)
            {
                sum += Hypergeometric(N, K, n, k);
            }
            return Math.Clamp(sum, 0.0, 1.0);
        }

        /// <summary>
        /// Multivariate Hypergeometric Distribution:
        /// Probability of simultaneously drawing exact target quantities k_i from multiple card pools K_i.
        /// Useful for evaluating complex multi-card combos (e.g. Starter + Extender without Garnet).
        /// </summary>
        /// <param name="N">Total deck/pool size.</param>
        /// <param name="K_groups">Sizes of each distinct card pool in the deck.</param>
        /// <param name="k_targets">Desired drawn count for each respective card pool.</param>
        /// <param name="n">Total cards drawn (must equal sum of k_targets).</param>
        public static double MultivariateHypergeometric(int N, int[] K_groups, int[] k_targets, int n)
        {
            if (K_groups == null || k_targets == null || K_groups.Length != k_targets.Length)
                return 0.0;

            int sumK = 0;
            int sumKTargets = 0;
            for (int i = 0; i < K_groups.Length; i++)
            {
                if (k_targets[i] < 0 || k_targets[i] > K_groups[i]) return 0.0;
                sumK += K_groups[i];
                sumKTargets += k_targets[i];
            }
            if (sumKTargets != n || sumK > N) return 0.0;

            double denom = Combinations(N, n);
            if (denom <= 0.0) return 0.0;

            double num = 1.0;
            for (int i = 0; i < K_groups.Length; i++)
            {
                num *= Combinations(K_groups[i], k_targets[i]);
            }
            return Math.Clamp(num / denom, 0.0, 1.0);
        }

        /// <summary>
        /// Calculates opening hand consistency (0.0 to 1.0).
        /// Standard competitive benchmark is >= 0.85 (85%).
        /// </summary>
        /// <param name="deckSize">Total cards in deck (typically 40).</param>
        /// <param name="starterCount">Total number of playable starters/engine access cards.</param>
        /// <param name="handSize">Opening hand size (5 for Going 1st, 6 for Going 2nd).</param>
        public static double OpeningConsistency(int deckSize, int starterCount, int handSize = 5)
        {
            return AtLeastOne(deckSize, starterCount, handSize);
        }

        /// <summary>
        /// Calculates the odds of hitting a required card when excavating (e.g. Pot of Prosperity, Reasoning).
        /// </summary>
        public static double ExcavationOdds(int remainingDeck, int targetCopies, int excavateCount)
        {
            return AtLeastOne(remainingDeck, targetCopies, excavateCount);
        }

        /// <summary>
        /// Calculates the risk of losing ALL remaining copies of a critical combo piece or boss
        /// when banishing face-down (e.g., Pot of Desires banishing 10 cards).
        /// </summary>
        /// <param name="remainingDeck">Cards currently in deck before banish.</param>
        /// <param name="criticalCopies">Remaining copies of the critical card in deck.</param>
        /// <param name="banishCount">Number of cards to banish (e.g. 10 for Desires, 3 or 6 for Prosperity/Extravagance).</param>
        /// <returns>Probability (0.0 - 1.0) that ALL copies are banished.</returns>
        public static double BanishLossRisk(int remainingDeck, int criticalCopies, int banishCount)
        {
            if (criticalCopies <= 0) return 1.0; // Already gone
            if (banishCount >= remainingDeck) return 1.0;
            if (banishCount < criticalCopies) return 0.0; // Cannot banish all if banishCount < copies

            // Probability that exactly criticalCopies are among the banished cards
            return Hypergeometric(remainingDeck, criticalCopies, banishCount, criticalCopies);
        }

        /// <summary>
        /// Mathematically computes hand trap likelihood using Hypergeometric base + game state context.
        /// Evaluates the probability that the opponent currently holds at least 1 handtrap.
        /// </summary>
        /// <param name="opponentHandCount">Cards in opponent's hand.</param>
        /// <param name="opponentDeckCount">Cards remaining in opponent's deck.</param>
        /// <param name="estimatedHandtrapsInDeck">Estimated total handtraps run by opponent (typically 9-12 in modern format).</param>
        /// <param name="opponentHasChainedThisTurn">Whether opponent already chained a response this turn.</param>
        /// <param name="isGoingFirst">Whether the bot went first.</param>
        /// <param name="turn">Current turn number.</param>
        public static double EstimateHandTrapLikelihood(
            int opponentHandCount,
            int opponentDeckCount = 35,
            int estimatedHandtrapsInDeck = 9,
            bool opponentHasChainedThisTurn = false,
            bool isGoingFirst = true,
            int turn = 1)
        {
            if (opponentHandCount <= 0) return 0.0;

            int totalKnownPool = opponentDeckCount + opponentHandCount;
            if (totalKnownPool <= 0) totalKnownPool = 40;

            // Base hypergeometric probability that at least 1 handtrap is in opponent's hand
            double baseProb = AtLeastOne(totalKnownPool, estimatedHandtrapsInDeck, opponentHandCount);

            // Context adjustments
            if (isGoingFirst && turn <= 1)
            {
                // Going first turn 1: Opponent has not had their draw phase yet
                baseProb *= 0.85;
            }

            if (opponentHasChainedThisTurn)
            {
                // Opponent already burned one handtrap this turn; calculate odds of holding a SECOND handtrap
                // P(X >= 2 | X >= 1) or discounted probability
                baseProb = AtLeast(totalKnownPool, estimatedHandtrapsInDeck - 1, opponentHandCount, 1) * 0.45;
            }

            if (turn >= 5)
            {
                // Late game: hands are depleted and mostly contain committed combo pieces or topdecks
                baseProb *= 0.55;
            }

            return Math.Clamp(baseProb, 0.0, 1.0);
        }
    }
}
