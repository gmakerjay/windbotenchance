using System;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.DecisionEngine
{
    /// <summary>
    /// Captures a structured, high-level representation (State Representation) 
    /// of the current game state, summarizing strategic variables (LP, hand sizes,
    /// floodgates, active negates, and threats) for decision-making.
    /// </summary>
    public class StateRepresentation
    {
        public int Turn { get; private set; }
        public DuelPhase Phase { get; private set; }
        public int BotLP { get; private set; }
        public int OpponentLP { get; private set; }
        public int BotHandCount { get; private set; }
        public int OpponentHandCount { get; private set; }
        public int OpponentSummonsThisTurn { get; private set; }
        public bool NibiruThreatActive { get; private set; }
        public bool IsSpellFloodgated { get; private set; }
        public bool IsMonsterFloodgated { get; private set; }
        public int DisruptionsCount { get; private set; }
        public bool IsGrindGame { get; private set; }
        public double OpponentThreatLevel { get; private set; }
        public bool OpponentHasNegate { get; private set; }

        /// <summary>
        /// Captures and rebuilds the state representation from the executor context.
        /// </summary>
        public void Capture(
            Executor executor, 
            OpponentProfiler profiler, 
            ThreatAnalyzer threatAnalyzer,
            int ownSummonCount,
            int opponentSummonCount
        )
        {
            if (executor == null) return;

            Turn = executor.Duel.Turn;
            Phase = executor.Duel.Phase;

            BotLP = executor.Bot.LifePoints;
            OpponentLP = executor.Enemy.LifePoints;

            BotHandCount = executor.Bot.Hand.Count;
            OpponentHandCount = executor.Enemy.Hand.Count;

            OpponentSummonsThisTurn = opponentSummonCount;
            NibiruThreatActive = (ownSummonCount + opponentSummonCount) >= 4;

            // Floodgate Checks
            IsSpellFloodgated = executor.Enemy.HasInSpellZone(58921041, true, true)  // Anti-Spell
                || executor.Enemy.HasInSpellZone(68462976, true, true)               // Secret Village
                || executor.Enemy.HasInSpellZone(61740673, true, true);              // Imperial Order

            IsMonsterFloodgated = executor.Enemy.HasInSpellZone(82732705, true, true); // Skill Drain

            // Grind game check: turn count is high, or hand sizes are low
            IsGrindGame = Turn >= 6 || (BotHandCount <= 1 && executor.Bot.GetMonsterCount() <= 1);

            // Compute opponent threats
            if (threatAnalyzer != null)
            {
                var highestMonster = threatAnalyzer.GetHighestThreatMonster(executor.Enemy.GetMonsters());
                OpponentThreatLevel = highestMonster != null ? threatAnalyzer.GetThreatScore(highestMonster) : 0.0;
            }

            // Negate check
            OpponentHasNegate = executor.Enemy.GetMonsters().Any(m => 
                m != null && m.IsFaceup() && !m.IsDisabled() && 
                (m.Id == 84815190 || m.Id == 4280258 || m.Id == 57793869 || m.Id == 10443957));

            // Disruptions Count
            DisruptionsCount = CalculateDisruptions(executor);
        }

        private int CalculateDisruptions(Executor executor)
        {
            int count = 0;

            // Face-up active negators we control
            foreach (var m in executor.Bot.GetMonsters())
            {
                if (m != null && m.IsFaceup() && !m.IsDisabled())
                {
                    int[] negators = { 84815190, 4280258, 57793869, 10443957 };
                    if (negators.Contains(m.Id)) count += 2;
                }
            }

            // Set cards in spell zone
            count += executor.Bot.GetSpells().Count(c => c != null && c.IsFacedown());

            // Hand traps in hand
            int[] handTraps = { 14558127, 23434538, 97268402, 63845230, 10045474 };
            count += executor.Bot.Hand.Count(c => c != null && (handTraps.Contains(c.Id) || CardIntelligence.IsHandtrap(c.Id)));

            return count;
        }
    }
}
