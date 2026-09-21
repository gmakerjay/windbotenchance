using System;
using System.Collections.Generic;
using System.Linq;
using WindBot.Game;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot
{
    /// <summary>
    /// Layer 2: Heuristic Violation Guard
    /// Post-decision validation that detects logical errors.
    /// Uses injectable delegates for output (injected by host at startup).
    /// Does NOT block decisions — observation only.
    /// </summary>
    public static class HeuristicGuard
    {
        /// <summary>
        /// Master switch.
        /// </summary>
        public static bool Enabled { get; set; } = true;

        /// <summary>
        /// Running count of violations/warnings detected in this duel session.
        /// </summary>
        public static int ViolationCount { get; private set; } = 0;
        public static int WarningCount { get; private set; } = 0;

        /// <summary>
        /// Output delegates — injected by the host (WindBot.Program) at startup.
        /// </summary>
        public static Action<string> WriteTrace { get; set; }
        public static Action<string> WriteError { get; set; }

        // Ace/Boss card IDs registered by executor
        private static readonly HashSet<int> _aceCardIds = new HashSet<int>();

        // Well-known hint constants from YGOPro protocol
        private const long HINTMSG_NEGATE = 575;
        private const long HINTMSG_FMATERIAL = 511;
        private const long HINTMSG_SMATERIAL = 512;
        private const long HINTMSG_XMATERIAL = 513;
        private const long HINTMSG_LMATERIAL = 533;
        private const long HINTMSG_REMOVE = 504;
        private const long HINTMSG_TOGRAVE = 508;
        private const long HINTMSG_DESTROY = 502;
        private const long HINTMSG_TARGET = 551;

        /// <summary>
        /// Register boss/ace card IDs that should trigger warnings when used as material.
        /// Call from executor constructor.
        /// </summary>
        public static void RegisterAceCards(params int[] cardIds)
        {
            foreach (int id in cardIds)
                _aceCardIds.Add(id);
        }

        /// <summary>
        /// Reset counters at the start of a new duel session.
        /// </summary>
        public static void ResetSession()
        {
            ViolationCount = 0;
            WarningCount = 0;
        }

        /// <summary>
        /// Validate a card selection made by the AI.
        /// Called from GameAI.OnSelectCard() before returning.
        /// </summary>
        public static void ValidateSelection(
            IList<ClientCard> selected, long hint, int turn,
            ClientField bot, ClientField enemy)
        {
            if (!Enabled || selected == null || selected.Count == 0) return;

            try
            {
                // Rule 1: Self-Negate
                if (hint == HINTMSG_NEGATE)
                {
                    // EXEMPTION: Buio the Dawn's Light (19000848) negates our own Fiend Effect monsters to summon itself.
                    var ownCards = selected.Where(c => c != null && c.Controller == 0 && !(c.HasRace(CardRace.Fiend) && c.HasType(CardType.Effect))).ToList();
                    if (ownCards.Count > 0)
                    {
                        string cardNames = string.Join(", ", ownCards.Select(c => $"{c.Name ?? "?"} ({c.Id})"));
                        LogViolation(turn, $"Self-Negate: AI negating own card(s): {cardNames}");
                        GameStateSnapshot.DumpToLog(turn, "?", "Self-Negate Violation", bot, enemy);
                    }
                }

                // Rule 2: Ace-as-Material
                if (hint == HINTMSG_FMATERIAL || hint == HINTMSG_SMATERIAL ||
                    hint == HINTMSG_XMATERIAL || hint == HINTMSG_LMATERIAL)
                {
                    var aceCards = selected.Where(c => c != null && _aceCardIds.Contains(c.Id)).ToList();
                    if (aceCards.Count > 0)
                    {
                        string materialType = "Unknown";
                        if (hint == HINTMSG_FMATERIAL) materialType = "Fusion";
                        else if (hint == HINTMSG_SMATERIAL) materialType = "Synchro";
                        else if (hint == HINTMSG_XMATERIAL) materialType = "Xyz";
                        else if (hint == HINTMSG_LMATERIAL) materialType = "Link";

                        string cardNames = string.Join(", ", aceCards.Select(c => $"{c.Name ?? "?"} ({c.Id})"));
                        LogWarning(turn, $"Ace-as-Material: Boss monster used as {materialType} material: {cardNames}");
                    }
                }

                // Rule 3: Self-Target (destruction) when enemy has targets
                if (hint == HINTMSG_DESTROY)
                {
                    var ownCards = selected.Where(c => c != null && c.Controller == 0 &&
                        (c.Location == CardLocation.MonsterZone || c.Location == CardLocation.SpellZone)).ToList();
                    bool enemyHasTargets = enemy.GetMonsters().Any(c => c != null && c.IsFaceup()) ||
                                           enemy.GetSpells().Any(c => c != null);
                    if (ownCards.Count == selected.Count && ownCards.Count > 0 && enemyHasTargets)
                    {
                        string cardNames = string.Join(", ", ownCards.Select(c => $"{c.Name ?? "?"} ({c.Id})"));
                        LogWarning(turn, $"Self-Target: AI targeting own card(s) while enemy has targets: {cardNames}");
                    }
                }

                // Rule 4: Sending own ace to GY/Banish
                if (hint == HINTMSG_TOGRAVE || hint == HINTMSG_REMOVE)
                {
                    var ownFieldCards = selected.Where(c => c != null && c.Controller == 0 &&
                        c.Location == CardLocation.MonsterZone).ToList();
                    if (ownFieldCards.Count > 0)
                    {
                        var aceInField = ownFieldCards.Where(c => _aceCardIds.Contains(c.Id)).ToList();
                        if (aceInField.Count > 0)
                        {
                            string cardNames = string.Join(", ", aceInField.Select(c => $"{c.Name ?? "?"} ({c.Id})"));
                            string action = hint == HINTMSG_TOGRAVE ? "sending to GY" : "banishing";
                            LogWarning(turn, $"Ace-Removal: AI {action} boss monster(s): {cardNames}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError?.Invoke($"[HeuristicGuard] Validation error: {ex.Message}");
            }
        }

        private static void LogViolation(int turn, string message)
        {
            ViolationCount++;
            WriteTrace?.Invoke($"[VIOLATION][Turn {turn}] {message}");
            WriteError?.Invoke($"[VIOLATION][Turn {turn}] {message}");
        }

        private static void LogWarning(int turn, string message)
        {
            WarningCount++;
            WriteTrace?.Invoke($"[WARNING][Turn {turn}] {message}");
        }

        /// <summary>
        /// Get summary string for end-of-duel report.
        /// </summary>
        public static string GetSessionSummary()
        {
            return $"Violations: {ViolationCount}, Warnings: {WarningCount}";
        }
    }
}
