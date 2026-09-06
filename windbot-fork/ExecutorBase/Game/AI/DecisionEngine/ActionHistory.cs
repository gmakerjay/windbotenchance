using System;
using System.Collections.Generic;
using System.Linq;

namespace WindBot.Game.AI.DecisionEngine
{
    /// <summary>
    /// Tracks a rolling log of recent player and opponent game actions.
    /// Provides context on which once-per-turn effects have been spent
    /// and detects baiting patterns.
    /// </summary>
    public class ActionHistory
    {
        public class GameAction
        {
            public int Player { get; set; } // 0 = Bot, 1 = Opponent
            public int CardId { get; set; }
            public string ActionType { get; set; } // "Summon", "Activate", "Set", "Search", "Draw"
            public int Turn { get; set; }
            public string Phase { get; set; }
            public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        }

        private readonly List<GameAction> _history = new List<GameAction>();
        private const int MaxHistorySize = 50;

        /// <summary>
        /// Record a new action into the rolling history.
        /// </summary>
        public void Record(int player, int cardId, string actionType, int turn, string phase)
        {
            var action = new GameAction
            {
                Player = player,
                CardId = cardId,
                ActionType = actionType,
                Turn = turn,
                Phase = phase
            };

            _history.Add(action);

            if (_history.Count > MaxHistorySize)
            {
                _history.RemoveAt(0);
            }
        }

        /// <summary>
        /// Reset history for a new duel.
        /// </summary>
        public void Reset()
        {
            _history.Clear();
        }

        /// <summary>
        /// Check if a specific card has been activated by a player during the current turn.
        /// Useful for tracking once-per-turn restrictions.
        /// </summary>
        public bool HasBeenActivatedThisTurn(int player, int cardId, int currentTurn)
        {
            return _history.Any(a => 
                a.Player == player && 
                a.CardId == cardId && 
                a.ActionType == "Activate" && 
                a.Turn == currentTurn);
        }

        /// <summary>
        /// Get the last N actions recorded.
        /// </summary>
        public IEnumerable<GameAction> GetRecentActions(int count)
        {
            return _history.AsEnumerable().Reverse().Take(count);
        }

        /// <summary>
        /// Heuristically determine if the bot is currently in a bait phase 
        /// (e.g., played low-value bait cards recently in order to draw out a hand trap).
        /// </summary>
        public bool IsBaitPhaseActive(int currentTurn)
        {
            // If the last action was a known bait card (e.g., Pot spell, or minor effect)
            // and we haven't activated our main starter yet.
            var lastAction = _history.LastOrDefault(a => a.Player == 0 && a.Turn == currentTurn);
            if (lastAction == null) return false;

            // Common bait cards
            int[] baitIds = {
                84211599, // Pot of Prosperity
                35261759, // Pot of Desires
                55144522, // Pot of Extravagance
                25311006, // Triple Tactics Talent
            };

            return baitIds.Contains(lastAction.CardId) && lastAction.ActionType == "Activate";
        }
    }
}
