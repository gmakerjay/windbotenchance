using System;
using WindBot.Game;

namespace WindBot
{
    /// <summary>
    /// Layer 1: Decision Trace Logger
    /// Records WHY the bot chose each branch inside executor functions.
    /// Uses injectable delegate for output (Logger.WriteTraceLine injected at startup).
    /// Usage: DecisionTracer.Trace("FunctionName", "message");
    /// </summary>
    public static class DecisionTracer
    {
        /// <summary>
        /// Master switch — set to false to disable all trace output.
        /// </summary>
        public static bool Enabled { get; set; } = true;

        /// <summary>
        /// Output delegate — injected by the host (WindBot.Program) at startup.
        /// Signature: (string message) => void
        /// </summary>
        public static Action<string> WriteTrace { get; set; }

        /// <summary>
        /// Error output delegate — injected by the host at startup.
        /// </summary>
        public static Action<string> WriteError { get; set; }

        private static void Write(string message)
        {
            WriteTrace?.Invoke(message);
        }

        /// <summary>
        /// Trace a decision step within an executor function.
        /// Output: [TRACE][FunctionName] message
        /// </summary>
        public static void Trace(string functionName, string message)
        {
            if (!Enabled) return;
            Write($"[TRACE][{functionName}] {message}");
        }

        /// <summary>
        /// Trace when a card is selected (cost, target, material, etc.)
        /// Output: [TRACE][FunctionName] LABEL: CardName (CardId) from Location[Sequence]
        /// </summary>
        public static void TraceSelect(string functionName, string label, ClientCard card)
        {
            if (!Enabled || card == null) return;
            string cardInfo = $"{card.Name ?? "?"} ({card.Id}) from {card.Location}";
            if (card.Sequence >= 0)
                cardInfo += $"[{card.Sequence}]";
            Write($"[TRACE][{functionName}] {label}: {cardInfo}");
        }

        /// <summary>
        /// Trace when a function decides NOT to activate (return false).
        /// Output: [TRACE][FunctionName] SKIP: reason
        /// </summary>
        public static void TraceSkip(string functionName, string reason)
        {
            if (!Enabled) return;
            Write($"[TRACE][{functionName}] SKIP: {reason}");
        }

        /// <summary>
        /// Trace when a function decides to activate (return true).
        /// Output: [TRACE][FunctionName] → ACTIVATE: reason
        /// </summary>
        public static void TraceActivate(string functionName, string reason)
        {
            if (!Enabled) return;
            Write($"[TRACE][{functionName}] → ACTIVATE: {reason}");
        }
    }
}
