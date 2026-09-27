namespace WindBot.Game.AI.Plugin
{
    /// <summary>
    /// Contract for dynamic scoring of actions and combos before execution.
    /// </summary>
    public interface IDeckActionScorer
    {
        int CalculateActionScore(string actionName, int boardImpact, int netGain, int cost);
    }
}
