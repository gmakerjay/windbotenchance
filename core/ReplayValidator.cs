using System;
using System.Collections.Generic;

namespace YgoAiPlatform.Core
{
    public class ReplayValidator
    {
        // บันทึกความสัมพันธ์ระหว่าง GameState Hash และ Move ที่ถูกเลือก
        private readonly Dictionary<string, Move> _history = new Dictionary<string, Move>();
        private readonly object _lockObj = new object();

        /// <summary>
        /// ล้างประวัติการจำลองเดิม
        /// </summary>
        public void Clear()
        {
            lock (_lockObj)
            {
                _history.Clear();
            }
        }

        /// <summary>
        /// ตรวจทวนสอบว่า Action ที่เลือกในปัจจุบัน ตรงกันกับ Action ในอดีตเมื่อส่งสถานะบอร์ดแบบเดิมเข้าไปหรือไม่ (Determinism Check)
        /// </summary>
        public bool Validate(GameState state, Move actualMove)
        {
            if (state == null || actualMove == null) return false;

            string stateHash = state.GetStateHash();

            lock (_lockObj)
            {
                if (_history.TryGetValue(stateHash, out var expectedMove))
                {
                    // มีประวัติสถานะนี้อยู่แล้ว ตรวจสอบว่า Move ตรงกันหรือไม่
                    bool matches = actualMove.IsEqualTo(expectedMove);
                    
                    if (!matches)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[ReplayValidator] Determinism Mismatch!");
                        Console.WriteLine($"  State Hash: {stateHash}");
                        Console.WriteLine($"  Expected: {expectedMove.Action} {expectedMove.CardName} (Location: {expectedMove.TargetLocation})");
                        Console.WriteLine($"  Actual  : {actualMove.Action} {actualMove.CardName} (Location: {actualMove.TargetLocation})");
                        Console.ResetColor();
                    }
                    return matches;
                }
                else
                {
                    // ไม่พบประวัติสถานะนี้ บันทึกเข้าไปเพื่อใช้ทวนสอบในอนาคต
                    _history[stateHash] = actualMove;
                    return true;
                }
            }
        }

        /// <summary>
        /// ดึงจำนวนสถานะในประวัติการทวนสอบทั้งหมด
        /// </summary>
        public int GetHistoryCount()
        {
            lock (_lockObj)
            {
                return _history.Count;
            }
        }
    }
}
