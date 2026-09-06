using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot.Game;

namespace WindBot.Game.AI
{
    /// <summary>
    /// ResourcePlanner — Anti-overextension and Nibiru awareness.
    /// Prevents the bot from blindly summoning everything, and helps
    /// conserve resources when the board is already sufficient.
    /// 
    /// Key capabilities:
    ///   1. Nibiru Checkpoint: Stop summoning when summon count approaches 5
    ///   2. Overextension Risk: Don't flood the board when board wipes are likely
    ///   3. Resource Conservation: Hold resources when winning
    ///   4. Card Economy: Evaluate cost vs impact of each play
    /// 
    /// Integrates into ModernExecutor — Legacy decks unaffected.
    /// </summary>
    public class ResourcePlanner
    {
        // ═══════════════════════════════════════
        //  CONFIGURATION
        // ═══════════════════════════════════════

        /// <summary>If true, the planner is active.</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>Summon count threshold for Nibiru awareness.</summary>
        public int NibiruThreshold { get; set; } = 4;

        /// <summary>Minimum opponent hand size to suspect Nibiru.</summary>
        public int NibiruHandThreshold { get; set; } = 2;

        /// <summary>Monster count threshold for overextension warning.</summary>
        public int OverextensionMonsterThreshold { get; set; } = 4;

        // ═══════════════════════════════════════
        //  NIBIRU CHECKPOINT
        // ═══════════════════════════════════════

        /// <summary>
        /// Should we stop summoning to avoid Nibiru?
        /// Returns true when:
        ///   1. Summon count >= threshold (approaching 5th summon)
        ///   2. Opponent hand is large enough to potentially hold Nibiru
        ///   3. We have at least some board presence already
        /// 
        /// The bot should then set up negation before continuing to summon.
        /// </summary>
        public bool NibiruCheckpoint(
            int summonCountThisTurn,
            int opponentHandCount,
            int ourMonsterCount,
            bool haveNegateOnField)
        {
            if (!Enabled) return false;

            // Not enough summons to worry
            if (summonCountThisTurn < NibiruThreshold) return false;

            // Opponent hand too small for Nibiru
            if (opponentHandCount < NibiruHandThreshold) return false;

            // We don't have board yet — need to keep going
            if (ourMonsterCount <= 1) return false;

            // We have a negate on field — Nibiru can be handled, keep going
            if (haveNegateOnField) return false;

            return true;
        }

        // ═══════════════════════════════════════
        //  OVEREXTENSION RISK
        // ═══════════════════════════════════════

        /// <summary>
        /// Assess overextension risk (0-100). Higher = more risky.
        /// Factors: monster count, opponent backrow, opponent archetype.
        /// </summary>
        public int OverextensionRisk(
            int ourMonsterCount,
            int opponentBackrowCount,
            int opponentMonsterCount,
            bool opponentIsControlDeck = false)
        {
            if (!Enabled) return 0;

            int risk = 0;

            // Monster count factor — more monsters = more vulnerable to board wipes
            if (ourMonsterCount >= 5)
                risk += 40;
            else if (ourMonsterCount >= 4)
                risk += 20;
            else if (ourMonsterCount >= 3)
                risk += 10;

            // Opponent backrow factor — set cards might be Torrential/Mirror Force
            risk += opponentBackrowCount * 10;

            // Opponent is control deck — higher board wipe likelihood
            if (opponentIsControlDeck)
                risk += 15;

            // If opponent has no monsters, they might have used resources on traps
            if (opponentMonsterCount == 0 && opponentBackrowCount >= 2)
                risk += 10;

            return Math.Min(100, risk);
        }

        /// <summary>
        /// Should we stop extending the board?
        /// Combines Nibiru awareness + overextension risk.
        /// </summary>
        public bool ShouldStopExtending(
            int summonCountThisTurn,
            int opponentHandCount,
            int ourMonsterCount,
            int opponentBackrowCount,
            int opponentMonsterCount,
            bool haveNegateOnField,
            bool opponentIsControlDeck = false)
        {
            if (!Enabled) return false;

            // Nibiru checkpoint
            if (NibiruCheckpoint(summonCountThisTurn, opponentHandCount, ourMonsterCount, haveNegateOnField))
                return true;

            // Overextension risk threshold
            int risk = OverextensionRisk(ourMonsterCount, opponentBackrowCount,
                opponentMonsterCount, opponentIsControlDeck);
            return risk >= 50;
        }

        // ═══════════════════════════════════════
        //  RESOURCE CONSERVATION
        // ═══════════════════════════════════════

        /// <summary>
        /// Should we conserve resources this turn?
        /// True when we're already winning and further plays risk over-committing.
        /// </summary>
        public bool ShouldConserveResources(
            int boardAdvantageScore,
            int ourHandCount,
            int ourDisruptionCount,
            bool isGoingFirst)
        {
            if (!Enabled) return false;

            // Going first turn 1 — never conserve, build board
            if (isGoingFirst) return false;

            // We're behind — don't conserve, push to recover
            if (boardAdvantageScore < 0) return false;

            // We're ahead with sufficient disruptions and hand — conserve
            if (boardAdvantageScore >= 15 && ourDisruptionCount >= 3 && ourHandCount >= 2)
                return true;

            // We have overwhelming advantage — no need to play more
            if (boardAdvantageScore >= 30)
                return true;

            return false;
        }

        // ═══════════════════════════════════════
        //  CARD ECONOMY SCORE
        // ═══════════════════════════════════════

        /// <summary>
        /// Evaluate whether playing a specific card right now is "worth it".
        /// Returns a score (0-100). Higher = more worth playing.
        /// 
        /// Consider:
        ///   - Card's impact on board state
        ///   - Whether we need it for a combo line
        ///   - Resource conservation needs
        /// </summary>
        public int CardEconomyScore(
            ClientCard card,
            ExecutorType actionType,
            int boardAdvantageScore,
            int ourDisruptionCount,
            bool isPartOfCombo)
        {
            if (card == null) return 50;

            int score = 50; // Neutral

            // Part of an active combo line → always play
            if (isPartOfCombo)
                score += 30;

            // Action type weighting
            switch (actionType)
            {
                case ExecutorType.Activate:
                    // Effect activation — usually impactful
                    score += 10;
                    // Removal effects are always valuable
                    if (card.Location == CardLocation.MonsterZone && card.IsFaceup())
                        score += 5;
                    break;

                case ExecutorType.Summon:
                    // Normal summon — important for combo starters
                    score += 5;
                    break;

                case ExecutorType.SpSummon:
                    // Special summon — check if it meaningfully improves board
                    if (card.Attack >= 2500 || card.IsExtraCard())
                        score += 15; // Boss monster
                    break;

                case ExecutorType.SpellSet:
                    // Setting backrow — safe, low commitment
                    score += 5;
                    break;
            }

            // Penalize playing cards when we're already winning
            if (boardAdvantageScore >= 20 && ourDisruptionCount >= 3)
                score -= 20;

            // Bonus for playing cards that add disruption when we have few
            if (ourDisruptionCount <= 1)
                score += 15;

            return Math.Max(0, Math.Min(100, score));
        }

        // ═══════════════════════════════════════
        //  ACE CARD REGISTRY & WIN-CON EVALUATION
        // ═══════════════════════════════════════

        /// <summary>Card IDs that are considered "Ace"/critical deck cards and should not be used as material/tribute suboptimally.</summary>
        private readonly HashSet<int> _aceCards = new HashSet<int>();

        /// <summary>Register Ace/Boss cards of this deck.</summary>
        public void RegisterAceCards(params int[] cardIds)
        {
            foreach (int id in cardIds)
                _aceCards.Add(id);
        }

        /// <summary>
        /// Evaluate if it is acceptable to use an Ace card as tribute/cost/material.
        /// Returns (allowed, reason).
        /// </summary>
        public (bool allowed, string reason) EvaluateAceUsage(
            ClientCard card,
            bool hasLethalIfUsed,
            bool isOnlyAnswerToThreat,
            bool haveAlternateWinCon)
        {
            if (card == null) return (true, "null card — skip check");
            if (!_aceCards.Contains(card.Id)) return (true, "not a registered Ace card");

            if (hasLethalIfUsed)
                return (true, $"Ace {card.Id}: Used because it enables lethal damage this turn");

            if (isOnlyAnswerToThreat)
                return (true, $"Ace {card.Id}: Used because it is the only answer to a current opponent threat");

            if (haveAlternateWinCon)
                return (true, $"Ace {card.Id}: Used because alternate win conditions/materials are secured");

            return (false, $"Ace {card.Id}: Blocked — no valid justification for sacrificing this card");
        }
    }
}
