using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace YgoAiPlatform.Core
{
    public class LogEntry
    {
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o"); // ISO 8601

        [JsonPropertyName("duel_id")]
        public string DuelId { get; set; } = string.Empty;

        [JsonPropertyName("turn")]
        public int Turn { get; set; }

        [JsonPropertyName("phase")]
        public string Phase { get; set; } = string.Empty;

        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("card")]
        public string Card { get; set; } = string.Empty;

        [JsonPropertyName("atk")]
        public int Atk { get; set; }

        [JsonPropertyName("def")]
        public int Def { get; set; }

        [JsonPropertyName("reason")]
        public List<string> Reason { get; set; } = new List<string>();

        [JsonPropertyName("threat_score")]
        public double ThreatScore { get; set; }

        [JsonPropertyName("board_score")]
        public double BoardScore { get; set; }

        [JsonPropertyName("resource_advantage")]
        public int ResourceAdvantage { get; set; }

        [JsonPropertyName("combo_branch")]
        public string ComboBranch { get; set; } = string.Empty;

        // ฟิลด์เพิ่มเติมกรณี verbose / developer log
        [JsonPropertyName("candidates")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<CandidateAction>? Candidates { get; set; }

        [JsonPropertyName("rejected_reason")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string>? RejectedReason { get; set; }
    }

    public class CandidateAction
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public double Score { get; set; }
    }
}
