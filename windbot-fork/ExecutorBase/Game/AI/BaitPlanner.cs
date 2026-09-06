using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot.Game;

namespace WindBot.Game.AI
{
    /// <summary>
    /// BaitPlanner — Hand trap bait system.
    /// A skilled player baits the opponent's hand traps with less critical cards
    /// before committing their combo starter. This module replicates that behavior.
    /// 
    /// How it works:
    ///   1. Deck executor registers "combo starters" (the cards that MUST resolve)
    ///   2. Before each Main Phase action, BaitPlanner checks:
    ///      - Does the opponent likely have hand traps?
    ///      - Is the next card in queue a combo starter?
    ///      - Do we have a "bait card" we can play first?
    ///   3. If yes to all → recommend playing the bait card first.
    /// 
    /// Integrates into ModernExecutor — Legacy decks unaffected.
    /// </summary>
    public class BaitPlanner
    {
        // ═══════════════════════════════════════
        //  CONFIGURATION
        // ═══════════════════════════════════════

        /// <summary>If true, the bait system is active.</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>Hand trap likelihood threshold (0.0-1.0) to trigger baiting.</summary>
        public double BaitThreshold { get; set; } = 0.4;

        // ═══════════════════════════════════════
        //  REGISTRATION
        // ═══════════════════════════════════════

        /// <summary>Card IDs that are critical combo starters — must not be negated.</summary>
        private readonly HashSet<int> _comboStarters = new HashSet<int>();

        /// <summary>Card IDs that make good bait — trigger opponent response but recoverable.</summary>
        private readonly HashSet<int> _goodBaitCards = new HashSet<int>();

        /// <summary>Card IDs that should never be used as bait (too valuable).</summary>
        private readonly HashSet<int> _neverBait = new HashSet<int>();

        /// <summary>Track if we already baited this turn (don't double-bait).</summary>
        private bool _baitedThisTurn = false;

        /// <summary>Track if opponent already responded to our bait this turn.</summary>
        private bool _opponentRespondedThisTurn = false;

        /// <summary>
        /// Register a card as a critical combo starter that must resolve.
        /// BaitPlanner will try to protect these by playing bait first.
        /// </summary>
        public void RegisterComboStarter(int cardId)
        {
            _comboStarters.Add(cardId);
        }

        /// <summary>
        /// Register multiple combo starters at once.
        /// </summary>
        public void RegisterComboStarters(params int[] cardIds)
        {
            foreach (int id in cardIds)
                _comboStarters.Add(id);
        }

        /// <summary>
        /// Register a card as a good bait candidate — triggers chain but recoverable if negated.
        /// </summary>
        public void RegisterBaitCard(int cardId)
        {
            _goodBaitCards.Add(cardId);
        }

        /// <summary>
        /// Register multiple bait cards at once.
        /// </summary>
        public void RegisterBaitCards(params int[] cardIds)
        {
            foreach (int id in cardIds)
                _goodBaitCards.Add(id);
        }

        /// <summary>
        /// Register a card that should never be used as bait.
        /// </summary>
        public void RegisterNeverBait(int cardId)
        {
            _neverBait.Add(cardId);
        }

        // ═══════════════════════════════════════
        //  LIFECYCLE
        // ═══════════════════════════════════════

        /// <summary>Reset per-turn state. Call from OnNewTurn().</summary>
        public void OnNewTurn()
        {
            _baitedThisTurn = false;
            _opponentRespondedThisTurn = false;
        }

        /// <summary>Notify that opponent responded to our chain (used a hand trap).</summary>
        public void OnOpponentChainResponse()
        {
            _opponentRespondedThisTurn = true;
        }

        // ═══════════════════════════════════════
        //  HAND TRAP LIKELIHOOD ESTIMATION
        // ═══════════════════════════════════════

        /// <summary>
        /// Estimate the probability that the opponent has at least one hand trap.
        /// Based on: hand size, game phase, whether they've responded yet.
        /// </summary>
        public double EstimateHandTrapLikelihood(
            int opponentHandCount,
            int turn,
            bool opponentHasChainedThisTurn,
            bool isGoingFirst)
        {
            if (opponentHandCount == 0) return 0.0;

            // Base probability scales with hand size
            // In a typical 40-card deck with ~6 hand traps: P(≥1 in 5 cards) ≈ 57%
            double baseProbability;
            if (opponentHandCount >= 6) baseProbability = 0.75;
            else if (opponentHandCount >= 5) baseProbability = 0.60;
            else if (opponentHandCount >= 4) baseProbability = 0.50;
            else if (opponentHandCount >= 3) baseProbability = 0.35;
            else if (opponentHandCount >= 2) baseProbability = 0.20;
            else baseProbability = 0.10;

            // Adjustments
            // Turn 1 going first → opponent hasn't drawn yet, but has opening hand
            if (isGoingFirst && turn <= 1)
                baseProbability *= 0.7; // Lower — they might not have started yet

            // If opponent already responded (chained something), they burned one
            if (opponentHasChainedThisTurn)
                baseProbability *= 0.5; // They already used one — less likely to have more

            // Late game (turn 5+) → hand is likely combo pieces, not hand traps
            if (turn >= 5)
                baseProbability *= 0.6;

            return Math.Max(0.0, Math.Min(1.0, baseProbability));
        }

        // ═══════════════════════════════════════
        //  CORE DECISION
        // ═══════════════════════════════════════

        /// <summary>
        /// Should we bait before playing our combo starter?
        /// Returns true if:
        ///   1. Baiting is enabled and we haven't baited yet
        ///   2. Opponent likely has hand traps
        ///   3. Our next intended action is a combo starter
        ///   4. We have a bait card available in hand
        /// </summary>
        public bool ShouldBaitFirst(
            ClientCard intendedCard,
            ClientField bot,
            int opponentHandCount,
            int turn,
            bool opponentHasChainedThisTurn,
            bool isGoingFirst,
            int minHandSizeToBait = 2)
        {
            if (!Enabled) return false;
            if (_baitedThisTurn) return false;
            if (_opponentRespondedThisTurn) return false; // They already burned a hand trap
            if (intendedCard == null) return false;
            if (bot == null || bot.Hand.Count < minHandSizeToBait) return false;

            // Only trigger baiting if the intended card is a combo starter
            if (!_comboStarters.Contains(intendedCard.Id)) return false;

            // Estimate hand trap likelihood
            double likelihood = EstimateHandTrapLikelihood(
                opponentHandCount, turn, opponentHasChainedThisTurn, isGoingFirst);

            if (likelihood < BaitThreshold) return false;

            // Check if we have a bait card in hand
            bool hasBait = bot.Hand.Any(c => c != null && IsBaitCandidate(c));
            return hasBait;
        }

        /// <summary>
        /// Get the best bait card from our hand.
        /// Prefers registered bait cards; falls back to lowest-impact activatable card.
        /// Returns null if no good bait is available.
        /// </summary>
        public ClientCard GetBaitCard(ClientField bot)
        {
            if (bot == null) return null;

            // Priority 1: Registered good bait cards
            var registeredBait = bot.Hand
                .Where(c => c != null && _goodBaitCards.Contains(c.Id))
                .OrderBy(c => c.Attack) // Prefer lower impact
                .FirstOrDefault();

            if (registeredBait != null)
            {
                _baitedThisTurn = true;
                return registeredBait;
            }

            // Priority 2: Any activatable card that's not a combo starter and not protected
            var fallbackBait = bot.Hand
                .Where(c => c != null && IsBaitCandidate(c))
                .OrderBy(c => GetBaitPriority(c)) // Lower priority = better bait
                .FirstOrDefault();

            if (fallbackBait != null)
            {
                _baitedThisTurn = true;
                return fallbackBait;
            }

            return null;
        }

        /// <summary>
        /// Check if a card is a valid bait candidate.
        /// </summary>
        private bool IsBaitCandidate(ClientCard card)
        {
            if (card == null) return false;
            if (_comboStarters.Contains(card.Id)) return false; // Never bait with combo starters
            if (_neverBait.Contains(card.Id)) return false;     // Explicitly protected

            // Must be an effect monster or spell (something that triggers a chain)
            if (card.HasType(CardType.Monster) && card.HasType(CardType.Effect))
                return true;
            if (card.HasType(CardType.Spell) && !card.HasType(CardType.Continuous)
                && !card.HasType(CardType.Field) && !card.HasType(CardType.Equip))
                return true;

            return false;
        }

        /// <summary>
        /// Lower number = better bait (more expendable / less loss if negated).
        /// </summary>
        private int GetBaitPriority(ClientCard card)
        {
            if (card == null) return 999;

            // Registered bait cards are best
            if (_goodBaitCards.Contains(card.Id)) return 10;

            // Monsters with search effects are good bait (opponent wants to negate searches)
            if (card.HasType(CardType.Monster) && card.HasType(CardType.Effect))
            {
                if (card.Attack <= 1500) return 20; // Small monster = good bait
                return 50;
            }

            // Normal spells that aren't combo starters
            if (card.HasType(CardType.Spell)) return 30;

            return 100; // Default — not great bait
        }

        /// <summary>
        /// Mark that our bait was responded to (opponent chained).
        /// After this, we know it's safer to play our combo starter.
        /// </summary>
        public void MarkBaitSucceeded()
        {
            _opponentRespondedThisTurn = true;
        }

        /// <summary>
        /// Check if a card is a registered combo starter.
        /// </summary>
        public bool IsComboStarter(int cardId)
        {
            return _comboStarters.Contains(cardId);
        }

        /// <summary>
        /// Check if a card is a registered combo starter.
        /// </summary>
        public bool IsComboStarter(ClientCard card)
        {
            return card != null && _comboStarters.Contains(card.Id);
        }
    }
}
