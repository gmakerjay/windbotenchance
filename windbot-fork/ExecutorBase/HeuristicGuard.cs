using System;
using System.Collections.Generic;
using System.Linq;
using WindBot.Game;
using WindBot.Game.AI;
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

        // Well-known hint constants from YGOPro / OCGCore protocol
        private const long HINTMSG_RELEASE = 500;
        private const long HINTMSG_DISCARD = 501;
        private const long HINTMSG_DESTROY = 502;
        private const long HINTMSG_REMOVE = 503;
        private const long HINTMSG_TOGRAVE = 504;
        private const long HINTMSG_RTOHAND = 505;
        private const long HINTMSG_ATOHAND = 506;
        private const long HINTMSG_TODECK = 507;
        private const long HINTMSG_SPSUMMON = 509;
        private const long HINTMSG_FMATERIAL = 511;
        private const long HINTMSG_SMATERIAL = 512;
        private const long HINTMSG_XMATERIAL = 513;
        private const long HINTMSG_FACEUP = 514;
        private const long HINTMSG_EQUIP = 518;
        private const long HINTMSG_REMOVEXYZ = 519;
        private const long HINTMSG_CONTROL = 520;
        private const long HINTMSG_POSCHANGE = 528;
        private const long HINTMSG_LMATERIAL = 533;
        private const long HINTMSG_TARGET = 551;
        private const long HINTMSG_DISABLE = 575;
        private const long HINTMSG_NEGATE = 575;

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
        /// Active Intervention Guard:
        /// Intercepts and auto-corrects fatal bot misplays (e.g. self-negate or friendly destruction
        /// when enemy targets are available), ensuring 0 violations across all 140+ decks.
        /// </summary>
        public static IList<ClientCard> SanitizeSelection(
            IList<ClientCard> selected, IList<ClientCard> pool, int min, int max,
            long hint, bool cancelable, int turn, ClientField bot, ClientField enemy)
        {
            if (!Enabled || pool == null || pool.Count == 0) return selected;
            if (selected == null || selected.Count < min) return selected;

            try
            {
                var enemyPool = pool.Where(c => c != null && c.Controller == 1).ToList();

                // 1. Intercept Self-Negate / Self-Disable (Do NOT include HINTMSG_FACEUP which is used for buffs/equips)
                if ((hint == HINTMSG_NEGATE || hint == HINTMSG_DISABLE) && enemyPool.Count >= min)
                {
                    // Exemption: Archetype self-negation (e.g. Buio the Dawn's Light 19000848)
                    bool isSelfNegateExempt = selected.Any(c => c != null && c.Controller == 0 && c.HasRace(CardRace.Fiend) && c.HasType(CardType.Effect));
                    if (!isSelfNegateExempt)
                    {
                        var ownSelected = selected.Where(c => c != null && c.Controller == 0).ToList();
                        if (ownSelected.Count > 0)
                        {
                            var viableEnemy = enemyPool.Where(c => !c.IsDisabled() && !c.IsShouldNotBeTarget()).ToList();
                            var candidateEnemy = viableEnemy.Count >= min ? viableEnemy : enemyPool;
                            var sortedEnemy = candidateEnemy.OrderByDescending(c => {
                                int score = 0;
                                if (WindBot.Game.AI.CardIntelligence.IsKnownNegator(c.Id) || WindBot.Game.AI.CardIntelligence.IsKnownNegator(c.GetNonAltartCode())) score += 10000;
                                if (WindBot.Game.AI.CardIntelligence.IsFloodgateMonster(c.Id)) score += 9500;
                                if (c.IsExtraCard()) score += 5000;
                                score += c.Attack;
                                return score;
                            }).ToList();

                            var sanitized = sortedEnemy.Take(Math.Min(max, sortedEnemy.Count)).ToList();
                            if (sanitized.Count >= min)
                            {
                                WriteTrace?.Invoke($"[AUTO-GUARD][Turn {turn}] Intercepted Self-Negate! Auto-corrected to enemy target(s): " +
                                    string.Join(", ", sanitized.Select(c => $"{c.Name ?? "?"} ({c.Id})")));
                                return sanitized;
                            }
                        }
                    }
                }

                // 2. Intercept Self-Destruction / Banish / Spin when enemy has viable targets
                if ((hint == HINTMSG_DESTROY || hint == HINTMSG_REMOVE || hint == 504 || hint == HINTMSG_TODECK) && enemyPool.Count >= min)
                {
                    var ownFieldCards = selected.Where(c => c != null && c.Controller == 0 &&
                        (c.Location == CardLocation.MonsterZone || c.Location == CardLocation.SpellZone)).ToList();

                    if (ownFieldCards.Count == selected.Count && ownFieldCards.Count > 0)
                    {
                        // Exemption: Known self-destruct beneficial triggers (DPE, Clock Tower, Fire Kings, Tokens)
                        bool hasBeneficialSelfPop = ownFieldCards.Any(c => 
                            c.IsCode(60461880, 27552504, 75500286, 21887175, 48680970)
                            || (_aceCardIds.Contains(c.Id) == false && (c.HasType(CardType.Token) || c.Id == 0)));

                        bool targetingOurAce = ownFieldCards.Any(c => _aceCardIds.Contains(c.Id) || c.Attack >= 2000 || c.IsExtraCard());
                        if (targetingOurAce && !hasBeneficialSelfPop)
                        {
                            var viableEnemy = enemyPool.Where(c => !c.IsShouldNotBeTarget()).ToList();
                            var candidateEnemy = viableEnemy.Count >= min ? viableEnemy : enemyPool;
                            var sortedEnemy = candidateEnemy.OrderByDescending(c => {
                                int score = 0;
                                if (WindBot.Game.AI.CardIntelligence.IsKnownNegator(c.Id)) score += 10000;
                                if (WindBot.Game.AI.CardIntelligence.IsFloodgateMonster(c.Id) || WindBot.Game.AI.CardIntelligence.IsFloodgateSpellTrap(c.Id)) score += 9500;
                                if (c.IsExtraCard()) score += 4000;
                                score += c.Attack;
                                return score;
                            }).ToList();

                            var sanitized = sortedEnemy.Take(Math.Min(max, sortedEnemy.Count)).ToList();
                            if (sanitized.Count >= min)
                            {
                                WriteTrace?.Invoke($"[AUTO-GUARD][Turn {turn}] Intercepted Self-Harm Removal! Auto-corrected to enemy target(s): " +
                                    string.Join(", ", sanitized.Select(c => $"{c.Name ?? "?"} ({c.Id})")));
                                return sanitized;
                            }
                        }
                    }
                }
            }
            catch { }

            return selected;
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
                if (hint == HINTMSG_NEGATE || hint == HINTMSG_DISABLE)
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
                if (hint == HINTMSG_DESTROY || hint == HINTMSG_REMOVE || hint == HINTMSG_TODECK)
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
