using System;
using System.Collections.Generic;
using System.Linq;

namespace WindBot.Game.AI.DecisionEngine
{
    /// <summary>
    /// Tracks the probability distribution (Belief State) of the opponent holding 
    /// specific hidden cards (such as Ash Blossom, Maxx "C", or Impermanence) in hand.
    /// Updates probabilities dynamically using Bayesian heuristics on game events.
    /// </summary>
    public class BeliefState
    {
        private readonly Dictionary<int, double> _probabilities = new Dictionary<int, double>();
        private readonly OpponentProfiler _profiler;

        // Known hand trap IDs
        public const int AshBlossom = 14558127;
        public const int MaxxC = 23434538;
        public const int EffectVeiler = 63845230;
        public const int InfiniteImpermanence = 10045474;
        public const int Nibiru = 27204311;

        public BeliefState(OpponentProfiler profiler)
        {
            _profiler = profiler;
            InitializeProbabilities();
        }

        /// <summary>
        /// Initialize probabilities based on the opponent's profile or default density.
        /// </summary>
        public void InitializeProbabilities()
        {
            _probabilities.Clear();

            double baseDensity = 0.15; // default 15%
            if (_profiler != null && _profiler.DetectedProfile != null)
            {
                baseDensity = _profiler.DetectedProfile.HandTrapDensity;
            }

            // Estimate hand trap likelihood at start
            _probabilities[AshBlossom] = Math.Min(0.60, baseDensity * 2.5);
            _probabilities[MaxxC] = Math.Min(0.50, baseDensity * 2.2);
            _probabilities[InfiniteImpermanence] = Math.Min(0.45, baseDensity * 2.0);
            _probabilities[EffectVeiler] = Math.Min(0.40, baseDensity * 1.8);
            _probabilities[Nibiru] = Math.Min(0.20, baseDensity * 0.8);
        }

        /// <summary>
        /// Get the estimated probability of the opponent holding a specific card.
        /// </summary>
        public double GetProbability(int cardId)
        {
            if (_probabilities.TryGetValue(cardId, out double p))
            {
                return p;
            }
            return 0.05; // low default
        }

        /// <summary>
        /// Direct override when card is confirmed in hand (e.g. searched or revealed).
        /// </summary>
        public void SetConfirmed(int cardId, bool isConfirmed)
        {
            _probabilities[cardId] = isConfirmed ? 1.0 : 0.0;
        }

        /// <summary>
        /// Called when the opponent has a chance to activate a response but chooses not to (passes priority).
        /// Decreases the probability of relevant hand traps.
        /// </summary>
        public void UpdateOnOpponentPass(int ourCardId, string eventType, bool opponentCanChain)
        {
            if (!opponentCanChain) return;

            // 1. Maxx "C" check: If we Special Summoned and they didn't respond,
            // the probability of them holding Maxx "C" drops significantly.
            if (eventType == "SpecialSummon")
            {
                _probabilities[MaxxC] = Math.Max(0.02, _probabilities[MaxxC] * 0.15);
            }

            // 2. Ash Blossom check: If we searched or drew extra and they didn't Ash,
            // the probability of them holding Ash drops.
            if (eventType == "Search" || eventType == "Draw")
            {
                _probabilities[AshBlossom] = Math.Max(0.05, _probabilities[AshBlossom] * 0.35);
            }

            // 3. Veiler / Impermanence check: If we activated a monster effect on field,
            // and they didn't negate it, their probability drops.
            if (eventType == "MonsterEffectActivation")
            {
                _probabilities[EffectVeiler] = Math.Max(0.05, _probabilities[EffectVeiler] * 0.40);
                _probabilities[InfiniteImpermanence] = Math.Max(0.05, _probabilities[InfiniteImpermanence] * 0.40);
            }

            // 4. Nibiru check: If we did 5+ summons and they didn't Nibiru us,
            // they probably don't have it or can't use it.
            if (eventType == "FiveSummonsReached")
            {
                _probabilities[Nibiru] = Math.Max(0.01, _probabilities[Nibiru] * 0.10);
            }
        }

        /// <summary>
        /// Reset for a new duel.
        /// </summary>
        public void Reset()
        {
            InitializeProbabilities();
        }
    }
}
