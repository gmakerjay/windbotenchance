namespace WindBot.Game.AI.Plugin
{
    /// <summary>
    /// Contract for evaluating archetypal threats and emergency responses.
    /// </summary>
    public interface IDeckThreatEvaluator
    {
        int EvaluateThreatScore(ClientCard card);
        bool IsEmergencyThreat(ClientCard card);
    }
}
