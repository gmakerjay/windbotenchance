using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YgoAiPlatform.Core
{
    /// <summary>
    /// ข้อมูลการ์ดแบบมีโครงสร้าง — ใช้แทน string ธรรมดาเพื่อเก็บรายละเอียดครบ
    /// </summary>
    public class CardInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("atk")]
        public int Attack { get; set; }

        [JsonPropertyName("def")]
        public int Defense { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("type")]
        public int CardType { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("link_markers")]
        public int LinkMarkers { get; set; }

        [JsonPropertyName("counters")]
        public List<int> Counters { get; set; } = new();
    }

    /// <summary>
    /// สถานะกระดานฝั่งผู้เล่น — ใช้ใน Dataset JSONL export
    /// หมายเหตุ: BoardState เป็น serialization-friendly subset ของ GameState
    /// </summary>
    public class BoardState
    {
        [JsonPropertyName("hand")]
        public List<string> Hand { get; set; } = new();

        [JsonPropertyName("field")]
        public List<string> Field { get; set; } = new();

        [JsonPropertyName("graveyard")]
        public List<string> Graveyard { get; set; } = new();

        [JsonPropertyName("banished")]
        public List<string> Banished { get; set; } = new();

        [JsonPropertyName("lp")]
        public int Lp { get; set; }

        [JsonPropertyName("hand_cards")]
        public List<CardInfo> HandCards { get; set; } = new();

        [JsonPropertyName("field_cards")]
        public List<CardInfo> FieldCards { get; set; } = new();

        [JsonPropertyName("graveyard_cards")]
        public List<CardInfo> GraveyardCards { get; set; } = new();

        [JsonPropertyName("banished_cards")]
        public List<CardInfo> BanishedCards { get; set; } = new();

        [JsonPropertyName("extra_deck_cards")]
        public List<CardInfo> ExtraDeckCards { get; set; } = new();

        /// <summary>
        /// สร้าง BoardState จาก GameState
        /// </summary>
        public static BoardState FromGameState(GameState state)
        {
            return new BoardState
            {
                Hand = new List<string>(state.Hand),
                Field = new List<string>(state.Field),
                Graveyard = new List<string>(state.Graveyard),
                Banished = new List<string>(state.Banished),
                Lp = state.LifePoints
            };
        }
    }

    /// <summary>
    /// สถานะกระดานฝั่งตรงข้าม (เห็นได้เพียงบางส่วน)
    /// </summary>
    public class OpponentState
    {
        [JsonPropertyName("hand_count")]
        public int HandCount { get; set; }

        [JsonPropertyName("field")]
        public List<string> Field { get; set; } = new();

        [JsonPropertyName("field_cards")]
        public List<CardInfo> FieldCards { get; set; } = new();

        [JsonPropertyName("graveyard")]
        public List<string> Graveyard { get; set; } = new();

        [JsonPropertyName("graveyard_cards")]
        public List<CardInfo> GraveyardCards { get; set; } = new();

        [JsonPropertyName("banished_cards")]
        public List<CardInfo> BanishedCards { get; set; } = new();

        [JsonPropertyName("lp")]
        public int Lp { get; set; }

        [JsonPropertyName("opponent_chokepoint_active")]
        public bool OpponentChokepointActive { get; set; }
    }

    /// <summary>
    /// ตัวอย่าง 1 แถวข้อมูลใน Dataset JSONL — แทนสถานะบอร์ดและการตัดสินใจ 1 จุด
    /// </summary>
    public class DatasetSample
    {
        [JsonPropertyName("duel_id")]
        public string DuelId { get; set; } = string.Empty;

        [JsonPropertyName("turn")]
        public int Turn { get; set; }

        [JsonPropertyName("phase")]
        public string Phase { get; set; } = string.Empty;

        [JsonPropertyName("board_state")]
        public BoardState BoardState { get; set; } = new();

        [JsonPropertyName("opponent_state")]
        public OpponentState OpponentState { get; set; } = new();

        [JsonPropertyName("legal_actions")]
        public List<string> LegalActions { get; set; } = new();

        [JsonPropertyName("chosen_action")]
        public string ChosenAction { get; set; } = string.Empty;

        [JsonPropertyName("action_reason")]
        public List<string> ActionReason { get; set; } = new();

        [JsonPropertyName("board_score")]
        public double BoardScore { get; set; }

        [JsonPropertyName("reward")]
        public double Reward { get; set; }

        [JsonPropertyName("result")]
        public string? Result { get; set; }

        /// <summary>
        /// Quality score (0.0-1.0) จาก QualityGuard.ComputeQualityScore()
        /// เพิ่มใน v3.0 — backward compatible: ค่า default = 1.0 (samples เก่าได้น้ำหนักเต็ม)
        /// </summary>
        [JsonPropertyName("quality_score")]
        public double QualityScore { get; set; } = 1.0;

        /// <summary>
        /// Quality tier (S/A/B/C) จาก QualityGuard
        /// </summary>
        [JsonPropertyName("quality_tier")]
        public string QualityTier { get; set; } = "S";

        /// <summary>
        /// เหตุผลที่ได้คะแนนคุณภาพเท่านี้ (สำหรับ debugging)
        /// </summary>
        [JsonPropertyName("quality_reasons")]
        public List<string> QualityReasons { get; set; } = new();

        /// <summary>
        /// Opponent deck identity — ชื่อเด็คที่กำลังสู้ด้วย
        /// ใช้สำหรับ generalization tracking และ adaptive weighting
        /// </summary>
        [JsonPropertyName("opponent_deck")]
        public string OpponentDeck { get; set; } = string.Empty;

        /// <summary>
        /// State hash จาก GameState.GetStateHash() — ใช้กับ DiversityTracker
        /// </summary>
        [JsonPropertyName("state_hash")]
        public string StateHash { get; set; } = string.Empty;
    }

    /// <summary>
    /// ผลการตรวจสอบความถูกต้องของไฟล์ Dataset
    /// </summary>
    public class ValidationReport
    {
        public int TotalSamples { get; set; }
        public int CorruptSamples { get; set; }
        public List<string> SchemaErrors { get; set; } = new();
        public bool IsValid => CorruptSamples == 0 && SchemaErrors.Count == 0;
    }
}
