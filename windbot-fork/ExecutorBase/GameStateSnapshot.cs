using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot
{
    /// <summary>
    /// Layer 3: Game State Snapshot
    /// Dumps board state when violations occur or when the bot loses.
    /// Uses injectable delegates for output.
    /// </summary>
    public static class GameStateSnapshot
    {
        /// <summary>
        /// Master switch.
        /// </summary>
        public static bool Enabled { get; set; } = true;

        /// <summary>
        /// Output delegates — injected by host at startup.
        /// </summary>
        public static Action<string> WriteTrace { get; set; }
        public static Action<string> WriteError { get; set; }

        /// <summary>
        /// Delegate to get the current log directory path.
        /// Injected by host at startup.
        /// </summary>
        public static Func<string> GetLogDir { get; set; }

        /// <summary>
        /// Dump a compact snapshot to the log.
        /// </summary>
        public static void DumpToLog(int turn, string phase, string reason,
            ClientField bot, ClientField enemy)
        {
            if (!Enabled) return;

            try
            {
                double boardScore = 0.0;
                try
                {
                    var scorer = new BoardScorer();
                    scorer.Reset(bot, enemy);
                    boardScore = scorer.BoardAdvantageScore();
                }
                catch (Exception)
                {
                    // Ignore
                }
                var sb = new StringBuilder();
                sb.AppendLine($"[SNAPSHOT][Turn {turn}][{phase}] Reason: {reason} | Board Score: {boardScore}");

                // Bot state
                sb.AppendLine($"  Bot LP={bot.LifePoints} | Hand: {FormatCards(bot.Hand)}");
                sb.AppendLine($"  Bot Field: {FormatFieldCards(bot.GetMonsters())} | ST: {FormatFieldCards(bot.GetSpells())}");
                sb.AppendLine($"  Bot Grave: {FormatCards(bot.Graveyard)}");
                if (bot.Banished.Count > 0)
                    sb.AppendLine($"  Bot Banished: {FormatCards(bot.Banished)}");

                // Enemy state
                sb.AppendLine($"  Enemy LP={enemy.LifePoints} | Hand: {enemy.Hand.Count} cards");
                sb.AppendLine($"  Enemy Field: {FormatFieldCards(enemy.GetMonsters())} | ST: {FormatFieldCards(enemy.GetSpells())}");
                sb.AppendLine($"  Enemy Grave: {FormatCards(enemy.Graveyard)}");

                WriteTrace?.Invoke(sb.ToString().TrimEnd());
            }
            catch (Exception ex)
            {
                WriteError?.Invoke($"[GameStateSnapshot] DumpToLog error: {ex.Message}");
            }
        }

        /// <summary>
        /// Dump a detailed snapshot as a JSON file in the log directory.
        /// </summary>
        public static void DumpToFile(int turn, string phase, string reason,
            ClientField bot, ClientField enemy)
        {
            if (!Enabled) return;

            try
            {
                string json = BuildJsonSnapshot(turn, phase, reason, bot, enemy);

                string logDir = GetLogDir?.Invoke();
                if (!string.IsNullOrEmpty(logDir))
                {
                    string snapshotDir = Path.Combine(logDir, "snapshots");
                    Directory.CreateDirectory(snapshotDir);

                    string timestamp = DateTime.Now.ToString("HHmmss");
                    string fileName = $"snapshot_turn{turn}_{timestamp}.json";
                    string filePath = Path.Combine(snapshotDir, fileName);

                    File.WriteAllText(filePath, json, Encoding.UTF8);
                    WriteTrace?.Invoke($"[SNAPSHOT] Saved to: {filePath}");
                }

                // Also dump compact version to log
                DumpToLog(turn, phase, reason, bot, enemy);
            }
            catch (Exception ex)
            {
                WriteError?.Invoke($"[GameStateSnapshot] DumpToFile error: {ex.Message}");
            }
        }

        private static string BuildJsonSnapshot(int turn, string phase, string reason,
            ClientField bot, ClientField enemy)
        {
            double boardScore = 0.0;
            try
            {
                var scorer = new BoardScorer();
                scorer.Reset(bot, enemy);
                boardScore = scorer.BoardAdvantageScore();
            }
            catch (Exception)
            {
                // Ignore
            }
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  \"turn\": {turn},");
            sb.AppendLine($"  \"phase\": \"{EscapeJson(phase)}\",");
            sb.AppendLine($"  \"reason\": \"{EscapeJson(reason)}\",");
            sb.AppendLine($"  \"board_score\": {boardScore},");
            sb.AppendLine($"  \"timestamp\": \"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\",");
            sb.AppendLine($"  \"guard_summary\": \"{EscapeJson(HeuristicGuard.GetSessionSummary())}\",");

            // Bot
            sb.AppendLine("  \"bot\": {");
            sb.AppendLine($"    \"lp\": {bot.LifePoints},");
            sb.AppendLine($"    \"hand\": [{FormatCardIds(bot.Hand)}],");
            sb.AppendLine($"    \"field_monsters\": [{FormatCardIds(bot.GetMonsters())}],");
            sb.AppendLine($"    \"field_spells\": [{FormatCardIds(bot.GetSpells())}],");
            sb.AppendLine($"    \"grave\": [{FormatCardIds(bot.Graveyard)}],");
            sb.AppendLine($"    \"banished\": [{FormatCardIds(bot.Banished)}],");
            sb.AppendLine($"    \"deck_count\": {bot.Deck.Count},");
            sb.AppendLine($"    \"extra_count\": {bot.ExtraDeck.Count}");
            sb.AppendLine("  },");

            // Enemy
            sb.AppendLine("  \"enemy\": {");
            sb.AppendLine($"    \"lp\": {enemy.LifePoints},");
            sb.AppendLine($"    \"hand_count\": {enemy.Hand.Count},");
            sb.AppendLine($"    \"field_monsters\": [{FormatCardIds(enemy.GetMonsters())}],");
            sb.AppendLine($"    \"field_spells\": [{FormatCardIds(enemy.GetSpells())}],");
            sb.AppendLine($"    \"grave\": [{FormatCardIds(enemy.Graveyard)}],");
            sb.AppendLine($"    \"banished\": [{FormatCardIds(enemy.Banished)}]");
            sb.AppendLine("  }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string FormatCards(IList<ClientCard> cards)
        {
            if (cards == null || cards.Count == 0) return "(empty)";
            var items = cards.Where(c => c != null)
                .Select(c => $"{c.Name ?? "?"}({c.Id})")
                .ToList();
            return items.Count > 0 ? string.Join(", ", items) : "(empty)";
        }

        private static string FormatFieldCards(IList<ClientCard> cards)
        {
            if (cards == null || cards.Count == 0) return "(empty)";
            var items = cards.Where(c => c != null)
                .Select(c =>
                {
                    string name = c.Name ?? "?";
                    string pos = c.IsFaceup() ? (c.IsAttack() ? "ATK" : "DEF") : "SET";
                    string atk = c.Attack >= 0 ? $"/{c.Attack}" : "";
                    return $"{name}({c.Id})[{pos}{atk}]";
                })
                .ToList();
            return items.Count > 0 ? string.Join(", ", items) : "(empty)";
        }

        private static string FormatCardIds(IList<ClientCard> cards)
        {
            if (cards == null || cards.Count == 0) return "";
            var items = cards.Where(c => c != null)
                .Select(c => $"\"{c.Name ?? "?"}({c.Id})\"");
            return string.Join(", ", items);
        }

        private static string EscapeJson(string s)
        {
            if (s == null) return "";
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "");
        }
    }
}
