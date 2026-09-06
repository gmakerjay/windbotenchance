using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace YgoAiPlatform.Core
{
    public enum DuelEventType
    {
        Summon,
        SpecialSummon,
        Set,
        Activate,
        ChainStart,
        ChainResolve,
        Negate,
        Attack,
        BattleResult,
        Destroy,
        Search,
        Draw,
        Discard,
        Banish,
        LpChange,
        PhaseChange,
        DuelEnd
    }

    public class DuelEvent
    {
        [JsonPropertyName("event_id")]
        public string EventId { get; set; } = Guid.NewGuid().ToString("D");

        [JsonPropertyName("duel_id")]
        public string DuelId { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o"); // ISO 8601

        [JsonPropertyName("turn")]
        public int Turn { get; set; }

        [JsonPropertyName("phase")]
        public string Phase { get; set; } = string.Empty;

        [JsonPropertyName("player")]
        public string Player { get; set; } = string.Empty; // "A" or "B"

        [JsonPropertyName("card_id")]
        public int CardId { get; set; }

        [JsonPropertyName("card_name")]
        public string CardName { get; set; } = string.Empty;

        [JsonPropertyName("source_location")]
        public string SourceLocation { get; set; } = string.Empty;

        [JsonPropertyName("target_location")]
        public string TargetLocation { get; set; } = string.Empty;

        [JsonPropertyName("chain_index")]
        public int ChainIndex { get; set; }

        [JsonPropertyName("board_score")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? BoardScore { get; set; }

        [JsonPropertyName("ai_reasoning")]
        public EventReasoning AiReasoning { get; set; } = new EventReasoning();

        [JsonPropertyName("event_type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DuelEventType EventType { get; set; }
    }

    public class EventReasoning
    {
        [JsonPropertyName("reason")]
        public List<string> Reason { get; set; } = new List<string>();

        [JsonPropertyName("score")]
        public double Score { get; set; }
    }
}
