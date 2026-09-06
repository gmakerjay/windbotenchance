using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.DecisionEngine
{
    /// <summary>
    /// Computes contextual card value using a layered scoring model:
    /// Value = Intrinsic + Board + Combo + Future - Risk.
    /// Helps the decision engine decide whether to activate, hold, or discard.
    /// </summary>
    public class DynamicValueEvaluator
    {
        private readonly OpponentProfiler _profiler;
        private readonly BeliefState _beliefState;

        // Base intrinsic values for popular cards
        private static readonly Dictionary<int, double> IntrinsicValues = new Dictionary<int, double>
        {
            { 14558127, 70 },  // Ash Blossom
            { 23434538, 85 },  // Maxx "C"
            { 63845230, 60 },  // Effect Veiler
            { 10045474, 68 },  // Infinite Impermanence
            { 25311006, 75 },  // Triple Tactics Talent
            { 84211599, 70 },  // Pot of Prosperity
            { 44335251, 65 },  // Souleating Oviraptor
            { 53582565, 30 },  // Raigeki (base is moderate because it's situational)
        };

        public DynamicValueEvaluator(OpponentProfiler profiler, BeliefState beliefState)
        {
            _profiler = profiler;
            _beliefState = beliefState;
        }

        /// <summary>
        /// Calculates the dynamic strategic value of a card in the current state.
        /// </summary>
        public double Evaluate(ClientCard card, bool isOpponentTurn, int currentTurn, int opponentSummons, int ourCardsCount)
        {
            if (card == null) return 0;

            double intrinsic = GetIntrinsicValue(card);
            double board = GetBoardValue(card, isOpponentTurn, opponentSummons);
            double combo = GetComboValue(card, isOpponentTurn);
            double future = GetFutureValue(card, isOpponentTurn, currentTurn);
            double risk = GetRiskValue(card, isOpponentTurn);

            double total = intrinsic + board + combo + future - risk;

            // Optional: output breakdown to diagnostics
            System.Diagnostics.Debug.WriteLine($"[ValueEvaluator] {card.Name} (ID: {card.Id}) -> Intrinsic={intrinsic}, Board={board}, Combo={combo}, Future={future}, Risk={risk} | Total={total}");

            return Math.Max(0, total);
        }

        private double GetIntrinsicValue(ClientCard card)
        {
            if (IntrinsicValues.TryGetValue(card.Id, out double val))
            {
                return val;
            }
            
            // Heuristic fallbacks
            if (card.IsMonster())
            {
                return card.Attack >= 2500 ? 40.0 : 20.0;
            }
            if (card.IsSpell()) return 30.0;
            if (card.IsTrap()) return 25.0;

            return 10.0;
        }

        private double GetBoardValue(ClientCard card, bool isOpponentTurn, int opponentSummons)
        {
            double val = 0;

            // If it's a board clear (like Raigeki) and opponent controls monsters
            if (card.Id == 53582565) // Raigeki
            {
                // Scaled by opponent's monster count
                val += opponentSummons * 15.0;
            }

            // Negation effects on opponent's turn
            if (isOpponentTurn && IsNegator(card))
            {
                val += 15.0;
                if (opponentSummons >= 2) // Opponent is actively comboing
                {
                    val += 20.0;
                }
            }

            return val;
        }

        private double GetComboValue(ClientCard card, bool isOpponentTurn)
        {
            if (isOpponentTurn) return 0; // combos are for our turn

            // Combo starters/extenders
            int[] starters = { 44335251, 84211599 }; // Oviraptor, Prosperity
            if (starters.Contains(card.Id))
            {
                return 35.0; // high combo value
            }

            return 0;
        }

        private double GetFutureValue(ClientCard card, bool isOpponentTurn, int currentTurn)
        {
            double val = 0;

            // Hand traps have high future value early in the game (especially Turn 1/2)
            if (IsHandTrap(card) && currentTurn <= 2)
            {
                val += 40.0;
            }
            else if (IsHandTrap(card) && currentTurn >= 6)
            {
                val += 10.0; // drops late game when opponent resources are low
            }

            return val;
        }

        private double GetRiskValue(ClientCard card, bool isOpponentTurn)
        {
            double risk = 0;

            // Negate risks: if opponent has active negators on board, playing effects is risky
            if (_profiler != null && _profiler.OpponentSearches > 0)
            {
                // Opponent searched a hand trap counter?
                // Add minor risk penalty
                risk += 10.0;
            }

            // Called by the Grave risk: if we activate a hand trap that goes to GY (Ash/Veiler),
            // check if opponent has set cards or known Called by the Grave.
            if ((card.Id == 14558127 || card.Id == 63845230) && isOpponentTurn)
            {
                // Check if opponent has set cards (risk of getting Called by / crossout-ed)
                risk += 12.0;
            }

            return risk;
        }

        private bool IsNegator(ClientCard card)
        {
            int[] negators = { 14558127, 63845230, 10045474 }; // Ash, Veiler, Imperm
            return negators.Contains(card.Id);
        }

        private bool IsHandTrap(ClientCard card)
        {
            int[] handTraps = { 14558127, 23434538, 63845230, 10045474 };
            return handTraps.Contains(card.Id);
        }
    }
}
