namespace WindBot.Game.AI.Plugin
{
    /// <summary>
    /// Contract for Pendulum scales and field zone resolutions.
    /// </summary>
    public interface IDeckScaleResolver
    {
        ClientCard PickLowScale();
        ClientCard PickHighScale();
        ClientCard PickScalePopTarget();
    }
}
