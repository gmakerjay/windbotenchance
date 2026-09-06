using System;
using System.Collections.Generic;

namespace YgoAiPlatform.Core
{
    public class GameState
    {
        public List<string> Hand { get; set; } = new List<string>();
        public List<string> Field { get; set; } = new List<string>();
        public List<string> Graveyard { get; set; } = new List<string>();
        public List<string> Banished { get; set; } = new List<string>();
        
        public int LifePoints { get; set; } = 8000;
        public int OpponentLifePoints { get; set; } = 8000;
        public int OpponentHandCount { get; set; } = 5;
        
        public List<string> OpponentField { get; set; } = new List<string>();

        // Enhanced fields for v2.0 deck-specific training
        public int Turn { get; set; } = 1;
        public List<string> OpponentGraveyard { get; set; } = new List<string>();
        public List<string> ExtraDeck { get; set; } = new List<string>();

        // Features 32-35 properties
        public int GraveyardSpellCount { get; set; }
        public bool IsFieldSpellActive { get; set; }
        public bool OpponentChokepointActive { get; set; }

        // CB-004: Phase & Player tracking — กระทบการตัดสินใจ AI อย่างมาก
        // Phase values: "Draw" | "Standby" | "Main1" | "Battle" | "Main2" | "End"
        public string Phase { get; set; } = "Main1";

        // ใช้แยก going-first (board setup) vs going-second (board break) strategy
        public bool IsFirstPlayer { get; set; } = true;

        // ฟังก์ชันในการสร้าง Checksum หรือ Hash ของสถานะบอร์ดเพื่อใช้เปรียบเทียบใน ReplayValidator
        public string GetStateHash()
        {
            // รวมรายละเอียดของบอร์ดเข้าด้วยกันเพื่อทำ Hash อย่างง่าย
            var parts = new List<string>
            {
                string.Join(",", Hand),
                string.Join(",", Field),
                string.Join(",", Graveyard),
                string.Join(",", Banished),
                LifePoints.ToString(),
                OpponentLifePoints.ToString(),
                OpponentHandCount.ToString(),
                string.Join(",", OpponentField),
                // CB-004: Phase และ IsFirstPlayer ต้องอยู่ใน hash เพื่อให้ ReplayValidator ทำงานถูกต้อง
                Phase,
                IsFirstPlayer.ToString()
            };
            
            string raw = string.Join("|", parts);
            
            // แปลงเป็น Base64 หรือ Hash แบบรวดเร็ว
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(raw);
                byte[] hashBytes = sha.ComputeHash(textBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
