using System.Collections.Generic;

namespace WindBot.Game.AI.Plugin
{
    /// <summary>
    /// Contract for decoupled deck-specific domain logic and domain sub-helpers (Layer 3).
    /// Decouples resource economy, material scoring, scale management, action scoring, and strategy from the engine core.
    /// </summary>
    public interface IDeckPlugin
    {
        string DeckName { get; }
        void ResetTurnState();

        IDeckStrategy Strategy { get; }
        IDeckResourceEvaluator ResourceEvaluator { get; }
        IDeckMaterialEvaluator MaterialEvaluator { get; }
        IDeckActionScorer ActionScorer { get; }
        IDeckThreatEvaluator ThreatEvaluator { get; }
        IDeckScaleResolver ScaleResolver { get; }
    }
}
