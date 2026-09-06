namespace WindBot.Game.AI
{
    public class AnalysisScoreVector
    {
        public int BoardPressure { get; set; }
        public int OpponentThreat { get; set; }
        public int BackrowRisk { get; set; }
        public int ResourceAdvantage { get; set; }
        public bool HasLethal { get; set; }
        public bool HasLethalAfterOneRemoval { get; set; }
        public bool PreferBattleBeforeSetting { get; set; }

        // ── Smart Flow v2 fields ──
        /// <summary>True when bot should enter Battle Phase before continuing combo (e.g., empty opponent board).</summary>
        public bool ShouldAttackFirst { get; set; }
        /// <summary>How "complete" our board is (0-100). ≥60 = sufficient, stop extending.</summary>
        public int BoardSufficiency { get; set; }
        /// <summary>How good the attack opportunity is right now (0-100+). ≥60 = should attack.</summary>
        public int AttackOpportunity { get; set; }
        /// <summary>True when the bot should stop extending combos (board is good enough).</summary>
        public bool ShouldStopExtending { get; set; }
        /// <summary>Estimated damage if we enter Battle Phase right now.</summary>
        public int EstimatedDirectDamage { get; set; }
    }
}
