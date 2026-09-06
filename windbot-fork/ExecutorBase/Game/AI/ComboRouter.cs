using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot.Game;

namespace WindBot.Game.AI
{
    /// <summary>
    /// ComboRouter — Hand-aware combo line selection engine.
    /// Instead of static AddExecutor() ordering, evaluates what combo lines are
    /// available from the current hand and selects the one with the best end board.
    /// 
    /// How it works:
    ///   1. Deck executor registers ComboLine definitions at construction time
    ///   2. Each turn, EvaluateHand() checks which lines are achievable
    ///   3. The best line is activated → GetNextStep() returns the next card to play
    ///   4. Steps are marked complete as they resolve
    /// 
    /// This is OPTIONAL — deck executors that don't register combo lines
    /// fall back to normal AddExecutor() behavior.
    /// Legacy decks (DefaultExecutor) are completely unaffected.
    /// </summary>
    public class ComboRouter
    {
        // ═══════════════════════════════════════
        //  COMBO LINE DEFINITION
        // ═══════════════════════════════════════

        /// <summary>
        /// A single step in a combo line.
        /// </summary>
        public class ComboStep
        {
            /// <summary>Card ID to play in this step.</summary>
            public int CardId { get; set; }

            /// <summary>What action to take (Activate, Summon, SpSummon, SpellSet).</summary>
            public ExecutorType ActionType { get; set; }

            /// <summary>Optional: description for logging.</summary>
            public string Description { get; set; }

            /// <summary>True if this step has been completed this turn.</summary>
            public bool Completed { get; set; }

            /// <summary>True if this step is optional (skip if card not in hand).</summary>
            public bool Optional { get; set; }
        }

        /// <summary>
        /// A complete combo line from hand to end board.
        /// </summary>
        public class ComboLine
        {
            /// <summary>Unique name for this combo line (for logging).</summary>
            public string Name { get; set; }

            /// <summary>Optional fallback combo line name if this line gets negated.</summary>
            public string FallbackLineName { get; set; }

            /// <summary>Ordered list of steps to execute.</summary>
            public List<ComboStep> Steps { get; set; } = new List<ComboStep>();

            /// <summary>Card IDs required in hand to start this line.</summary>
            public List<int> RequiredCards { get; set; } = new List<int>();

            /// <summary>Card IDs that enhance this line (not required, but boost priority).</summary>
            public List<int> DesiredCards { get; set; } = new List<int>();

            /// <summary>
            /// End board quality score (0-100). Higher = better end board.
            /// Used to rank competing combo lines.
            /// </summary>
            public int EndBoardScore { get; set; }

            /// <summary>
            /// Priority when multiple lines have equal score.
            /// Lower = higher priority. Default = 50.
            /// </summary>
            public int Priority { get; set; } = 50;

            /// <summary>
            /// Optional condition function — checked at runtime to see if
            /// this line is viable given the current board state.
            /// Return true if the line is available.
            /// </summary>
            public Func<bool> Condition { get; set; }
        }

        // ═══════════════════════════════════════
        //  STATE
        // ═══════════════════════════════════════

        private readonly List<ComboLine> _registeredLines = new List<ComboLine>();
        private readonly HashSet<string> _failedLinesThisTurn = new HashSet<string>();
        private ComboLine _activeLine = null;
        private int _activeStepIndex = 0;

        /// <summary>True if there's currently an active combo line being executed.</summary>
        public bool HasActiveCombo => _activeLine != null && _activeStepIndex < _activeLine.Steps.Count;

        /// <summary>The name of the current active combo line, or null.</summary>
        public string ActiveComboName => _activeLine?.Name;

        /// <summary>If true, the router is enabled. Set false to disable.</summary>
        public bool Enabled { get; set; } = true;

        // ═══════════════════════════════════════
        //  REGISTRATION (called from deck executor constructor)
        // ═══════════════════════════════════════

        /// <summary>
        /// Register a combo line. Can register multiple lines — router picks the best.
        /// </summary>
        public void RegisterLine(ComboLine line)
        {
            if (line != null)
                _registeredLines.Add(line);
        }

        /// <summary>
        /// Shorthand: register a combo line with just name, required cards, steps, and score.
        /// </summary>
        public void RegisterLine(string name, int[] requiredCards, ComboStep[] steps, int endBoardScore, Func<bool> condition = null)
        {
            RegisterLine(new ComboLine
            {
                Name = name,
                RequiredCards = new List<int>(requiredCards),
                Steps = new List<ComboStep>(steps),
                EndBoardScore = endBoardScore,
                Condition = condition
            });
        }

        // ═══════════════════════════════════════
        //  LIFECYCLE
        // ═══════════════════════════════════════

        /// <summary>
        /// Reset per-turn state. Call from OnNewTurn().
        /// Clears active line, failed lines history, and step completion status.
        /// </summary>
        public void OnNewTurn()
        {
            _activeLine = null;
            _activeStepIndex = 0;
            _failedLinesThisTurn.Clear();
            foreach (var line in _registeredLines)
            {
                foreach (var step in line.Steps)
                    step.Completed = false;
            }
        }

        // ═══════════════════════════════════════
        //  HAND EVALUATION
        // ═══════════════════════════════════════

        /// <summary>
        /// Evaluate which combo lines are achievable from the current hand.
        /// Returns them sorted by EndBoardScore (descending), then Priority (ascending).
        /// </summary>
        public List<ComboLine> GetViableLines(ClientField bot)
        {
            if (!Enabled || _registeredLines.Count == 0 || bot == null)
                return new List<ComboLine>();

            var handIds = new HashSet<int>(bot.Hand.Where(c => c != null).SelectMany(c => new[] { c.Id, c.GetNonAltartCode() }));
            // Also count cards on field and in GY (some combo lines use them)
            var fieldIds = new HashSet<int>(bot.GetMonsters().Where(c => c != null).SelectMany(c => new[] { c.Id, c.GetNonAltartCode() }));
            var gyIds = new HashSet<int>(bot.Graveyard.Where(c => c != null).SelectMany(c => new[] { c.Id, c.GetNonAltartCode() }));

            var allAvailable = new HashSet<int>(handIds);
            allAvailable.UnionWith(fieldIds);
            allAvailable.UnionWith(gyIds);

            var viable = new List<ComboLine>();

            foreach (var line in _registeredLines)
            {
                // Check required cards
                bool hasRequired = line.RequiredCards.All(id => allAvailable.Contains(id));
                if (!hasRequired) continue;

                // Check optional condition
                if (line.Condition != null)
                {
                    try
                    {
                        if (!line.Condition()) continue;
                    }
                    catch
                    {
                        continue; // Condition threw → skip this line
                    }
                }

                viable.Add(line);
            }

            // Sort: highest EndBoardScore first, then lowest Priority
            viable.Sort((a, b) =>
            {
                int scoreComp = b.EndBoardScore.CompareTo(a.EndBoardScore);
                if (scoreComp != 0) return scoreComp;
                return a.Priority.CompareTo(b.Priority);
            });

            return viable;
        }

        /// <summary>
        /// Select the best combo line from hand and activate it.
        /// Call at the start of Main Phase 1 (after OnNewTurn).
        /// Returns true if a combo line was activated.
        /// </summary>
        public bool ActivateBestLine(ClientField bot)
        {
            if (!Enabled) return false;
            if (_activeLine != null) return true; // Already have an active line

            var viable = GetViableLines(bot);
            if (viable.Count == 0) return false;

            _activeLine = viable[0];
            _activeStepIndex = 0;

            System.Diagnostics.Debug.WriteLine($"[ComboRouter] Activated line: {_activeLine.Name} " +
                $"(score={_activeLine.EndBoardScore}, steps={_activeLine.Steps.Count})");
            return true;
        }

        // ═══════════════════════════════════════
        //  STEP MANAGEMENT
        // ═══════════════════════════════════════

        /// <summary>
        /// Get the next step to execute in the active combo line.
        /// Returns null if no active line or all steps completed.
        /// </summary>
        public ComboStep GetNextStep()
        {
            if (_activeLine == null) return null;

            while (_activeStepIndex < _activeLine.Steps.Count)
            {
                var step = _activeLine.Steps[_activeStepIndex];
                if (!step.Completed)
                    return step;
                _activeStepIndex++;
            }

            return null; // All steps done
        }

        /// <summary>
        /// Mark the current step as completed and advance to the next.
        /// </summary>
        public void CompleteCurrentStep()
        {
            if (_activeLine == null) return;
            if (_activeStepIndex < _activeLine.Steps.Count)
            {
                _activeLine.Steps[_activeStepIndex].Completed = true;
                _activeStepIndex++;
            }

            // Check if combo is complete
            if (_activeStepIndex >= _activeLine.Steps.Count)
            {
                System.Diagnostics.Debug.WriteLine($"[ComboRouter] ✓ Combo completed: {_activeLine.Name}");
            }
        }

        /// <summary>
        /// Skip the current step (optional step or couldn't execute).
        /// If a mandatory step fails, attempts to switch to fallback before aborting.
        /// </summary>
        public void SkipCurrentStep(ClientField bot = null)
        {
            if (_activeLine == null) return;
            if (_activeStepIndex < _activeLine.Steps.Count)
            {
                var step = _activeLine.Steps[_activeStepIndex];
                if (step.Optional)
                {
                    step.Completed = true; // Mark as done (skipped)
                    _activeStepIndex++;
                }
                else
                {
                    // Mandatory step failed → try fallback line before giving up
                    System.Diagnostics.Debug.WriteLine($"[ComboRouter] ✗ Mandatory step failed: {step.Description ?? step.CardId.ToString()} — checking fallback");
                    if (!TrySwitchToFallback(bot))
                    {
                        AbortCombo($"Mandatory step failed: {step.Description ?? step.CardId.ToString()}");
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to switch to the registered fallback line (Plan B) or dynamically finds the next best viable line.
        /// Guaranteed not to cycle into lines that have already failed this turn.
        /// </summary>
        public bool TrySwitchToFallback(ClientField bot = null)
        {
            if (_activeLine != null)
            {
                _failedLinesThisTurn.Add(_activeLine.Name);
            }

            string fallbackName = _activeLine?.FallbackLineName;
            string abortedName = _activeLine?.Name;
            _activeLine = null;
            _activeStepIndex = 0;

            if (!string.IsNullOrEmpty(fallbackName) && !_failedLinesThisTurn.Contains(fallbackName))
            {
                var fallback = _registeredLines.FirstOrDefault(l => l.Name == fallbackName);
                if (fallback != null && (fallback.Condition == null || SafeInvoke(fallback.Condition)))
                {
                    _activeLine = fallback;
                    _activeStepIndex = 0;
                    System.Diagnostics.Debug.WriteLine(
                        $"[ComboRouter] ↩ Fallback switched: {abortedName} → {fallback.Name}");
                    return true;
                }
            }

            // If no specific fallback registered, re-evaluate hand for any alternative viable line not already failed
            if (bot != null)
            {
                var viable = GetViableLines(bot)
                    .Where(l => l.Name != abortedName && !_failedLinesThisTurn.Contains(l.Name))
                    .ToList();
                if (viable.Count > 0)
                {
                    _activeLine = viable[0];
                    _activeStepIndex = 0;
                    System.Diagnostics.Debug.WriteLine(
                        $"[ComboRouter] ↩ Auto-switched to next best line: {abortedName} → {viable[0].Name}");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if a specific card ID is the next expected step.
        /// Used by deck executors to verify whether to follow the combo line.
        /// </summary>
        public bool IsNextStep(int cardId)
        {
            var next = GetNextStep();
            return next != null && next.CardId == cardId;
        }

        /// <summary>
        /// Check if a specific card ID is part of the active combo line
        /// (any step, not just the next one).
        /// </summary>
        public bool IsPartOfActiveCombo(int cardId)
        {
            if (_activeLine == null) return false;
            return _activeLine.Steps.Any(s => s.CardId == cardId);
        }

        /// <summary>
        /// Get the priority of a card in the active combo.
        /// Returns step index (lower = should play first). Returns -1 if not part of combo.
        /// </summary>
        public int GetComboOrder(int cardId)
        {
            if (_activeLine == null) return -1;
            for (int i = 0; i < _activeLine.Steps.Count; i++)
            {
                if (_activeLine.Steps[i].CardId == cardId)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Abort the current combo line (e.g., if a key card was negated).
        /// </summary>
        public void AbortCombo(string reason = "")
        {
            if (_activeLine != null)
            {
                _failedLinesThisTurn.Add(_activeLine.Name);
                System.Diagnostics.Debug.WriteLine($"[ComboRouter] ✗ Combo aborted: {_activeLine.Name} — {reason}");
                _activeLine = null;
                _activeStepIndex = 0;
            }
        }

        /// <summary>
        /// Notify that a combo step card was negated. If a fallback line is registered,
        /// switch to it immediately. Otherwise, abort the combo.
        /// </summary>
        public void NotifyStepNegated(int cardId)
        {
            if (_activeLine == null) return;

            string fallbackName = _activeLine.FallbackLineName;
            string abortedName = _activeLine.Name;

            AbortCombo($"Card {cardId} in combo was negated — checking fallback");

            if (!string.IsNullOrEmpty(fallbackName))
            {
                var fallback = _registeredLines.FirstOrDefault(l => l.Name == fallbackName);
                if (fallback != null && (fallback.Condition == null || SafeInvoke(fallback.Condition)))
                {
                    _activeLine = fallback;
                    _activeStepIndex = 0;
                    System.Diagnostics.Debug.WriteLine(
                        $"[ComboRouter] ↩ Fallback: {abortedName} → {fallback.Name} (switched due to negation)");
                }
            }
        }

        private bool SafeInvoke(Func<bool> condition)
        {
            try { return condition(); } catch { return false; }
        }
    }
}
