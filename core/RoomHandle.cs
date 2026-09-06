using System;

namespace YgoAiPlatform.Core
{
    public enum RoomStatus
    {
        Starting,
        Running,
        Paused,
        Crashed,
        Terminated
    }

    public class RoomHandle
    {
        public string DuelId { get; set; } = string.Empty;
        public int Port { get; set; }
        public int PubPort { get; set; }
        public int ProcessId { get; set; }
        public int Seed { get; set; }
        
        public string DeckA { get; set; } = string.Empty;
        public string DeckB { get; set; } = string.Empty;
        
        public RoomStatus Status { get; set; } = RoomStatus.Starting;
        public int RestartCount { get; set; } = 0;
        public const int MaxRestartCount = 5;
    }
}
