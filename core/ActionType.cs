namespace YgoAiPlatform.Core
{
    /// <summary>
    /// ประเภทของ Action ที่ผู้เล่นหรือ AI สามารถเลือกทำได้ในเทิร์น
    /// ใช้แทน Magic Strings เช่น "Summon", "Activate" ฯลฯ ทั่วโค้ดเบส
    /// </summary>
    public enum ActionType
    {
        Summon,
        Activate,
        Attack,
        Set,
        Pass,
        SpSummon,
        Repos
    }

    public static class ActionTypeExtensions
    {
        public static string ToActionString(this ActionType type)
        {
            return type switch
            {
                ActionType.Summon => "Summon",
                ActionType.Activate => "Activate",
                ActionType.Attack => "Attack",
                ActionType.Set => "Set",
                ActionType.Pass => "Pass",
                ActionType.SpSummon => "SpSummon",
                ActionType.Repos => "Repos",
                _ => "Unknown"
            };
        }

        public static ActionType ParseAction(string action)
        {
            if (string.IsNullOrEmpty(action)) return ActionType.Pass;
            return action.ToLowerInvariant() switch
            {
                "summon" => ActionType.Summon,
                "activate" => ActionType.Activate,
                "attack" => ActionType.Attack,
                "set" => ActionType.Set,
                "setmonster" => ActionType.Set,
                "setspell" => ActionType.Set,
                "pass" => ActionType.Pass,
                "spsummon" => ActionType.SpSummon,
                "repos" => ActionType.Repos,
                _ => ActionType.Pass
            };
        }
    }
}
