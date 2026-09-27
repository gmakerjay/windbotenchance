namespace WindBot.Game.AI.Plugin
{
    /// <summary>
    /// Abstract base class for decoupled Deck Plugins with default implementations.
    /// Simpler decks only need to override the specific evaluators they require.
    /// </summary>
    public abstract class DeckPluginBase : IDeckPlugin
    {
        public abstract string DeckName { get; }

        public virtual IDeckStrategy Strategy => null;
        public virtual IDeckResourceEvaluator ResourceEvaluator => null;
        public virtual IDeckMaterialEvaluator MaterialEvaluator => null;
        public virtual IDeckActionScorer ActionScorer => null;
        public virtual IDeckThreatEvaluator ThreatEvaluator => null;
        public virtual IDeckScaleResolver ScaleResolver => null;

        public virtual void ResetTurnState()
        {
            Strategy?.Reset();
            ResourceEvaluator?.Reset();
        }
    }
}
