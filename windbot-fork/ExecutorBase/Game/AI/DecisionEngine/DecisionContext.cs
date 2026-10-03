using System;
using System.Collections.Generic;

namespace WindBot.Game.AI.DecisionEngine
{
    /// <summary>
    /// Serves as the central coordinator (Service Locator / Container) 
    /// for the Windbot Decision Engine, decoupling strategic modules from executors.
    /// </summary>
    public class DecisionContext
    {
        public Executor Executor { get; }
        public StateRepresentation State { get; }
        public BeliefState BeliefState { get; }
        public ThreatAnalyzer ThreatAnalyzer { get; }
        public DynamicValueEvaluator ValueEvaluator { get; }
        public CounterfactualSimulator Simulator { get; }
        public ActionHistory ActionHistory { get; }

        public DecisionContext(Executor executor, OpponentProfiler profiler)
        {
            Executor = executor;
            State = new StateRepresentation();
            BeliefState = new BeliefState(profiler);
            ThreatAnalyzer = new ThreatAnalyzer(profiler);
            ValueEvaluator = new DynamicValueEvaluator(profiler, BeliefState);
            Simulator = new CounterfactualSimulator();
            ActionHistory = new ActionHistory();
        }

        /// <summary>
        /// Recalculates the game state representation. Call this before making any strategic decision.
        /// </summary>
        public void UpdateState(int ownSummons, int opponentSummons)
        {
            State.Capture(Executor, Executor is ModernExecutor modern ? modern.OpponentProfile : null, ThreatAnalyzer, ownSummons, opponentSummons);
        }

        /// <summary>
        /// Resets duel-specific states at the start of a new duel.
        /// </summary>
        public void ResetDuel()
        {
            BeliefState.Reset();
            ActionHistory.Reset();
        }

        /// <summary>
        /// Strategic query: Should the executor activate a card effect based on E(W|Activate) vs E(W|Hold)?
        /// </summary>
        public bool ShouldActivate(ClientCard ourCard, ClientCard targetCard, string eventType)
        {
            if (ourCard == null) return false;

            // [Core v0.094] Refresh the snapshot right before deciding. Previously State was captured only in
            // OnNewTurn (with 0/0 summons), so hand sizes / disruption counts were stale for the whole turn.
            try
            {
                int own = 0, opp = 0;
                if (Executor is ModernExecutor modernExec && modernExec.Brain != null)
                {
                    own = modernExec.Brain.OwnSummons;
                    opp = modernExec.Brain.OpponentSummonCount;
                }
                UpdateState(own, opp);
            }
            catch { }

            // 1. Calculate dynamic card value
            double cardValue = ValueEvaluator.Evaluate(
                ourCard, 
                Executor.Duel.Player == 1, 
                Executor.Duel.Turn, 
                State.OpponentSummonsThisTurn, 
                State.BotHandCount
            );

            // If card value is extremely low in this context, hold it.
            if (cardValue < 15.0) return false;

            // 2. Evaluate target threat if reacting to an opponent card
            // [Core v0.094] Rate the target as an ACTIVATION (search/SS/draw text, Extra Deck…), not only by location.
            double targetThreat = targetCard != null ? ThreatAnalyzer.GetActivationThreatScore(targetCard) : 0.0;

            // 3. Estimate probability of getting countered (using Called by/negators)
            double counterProb = 0.05;
            if (ourCard.Id == BeliefState.AshBlossom || ourCard.Id == BeliefState.EffectVeiler)
            {
                counterProb = BeliefState.GetProbability(BeliefState.InfiniteImpermanence) * 0.5; // proxy for Called by / negate hazard
            }

            // 4. Run Counterfactual Simulation: Action vs Hold win expectation
            bool isFirstAction = opponentSummonsThisTurn() == 0;
            bool hold = Simulator.ShouldHold(
                targetThreat, 
                counterProb, 
                State.DisruptionsCount, 
                State.OpponentHandCount, 
                isFirstAction
            );

            // If simulator advises holding, skip activation.
            if (hold)
            {
                System.Diagnostics.Debug.WriteLine($"[DecisionEngine] HOLD advised for {ourCard.Name} against threat {targetThreat}");
                return false;
            }

            System.Diagnostics.Debug.WriteLine($"[DecisionEngine] ACTIVATE approved for {ourCard.Name}");
            return true;
        }

        private int opponentSummonsThisTurn()
        {
            if (Executor is ModernExecutor modern && modern.Brain != null)
            {
                return modern.Brain.OpponentSummonCount;
            }
            return 0;
        }
    }
}
