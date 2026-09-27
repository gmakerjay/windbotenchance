using System.Collections.Generic;

namespace WindBot.Game.AI.Plugin
{
    /// <summary>
    /// Contract for domain-specific resource economy (e.g. Spell Counters, Bushido Counters, Fog Counters).
    /// </summary>
    public interface IDeckResourceEvaluator
    {
        void Reset();
        int GetAvailableResourceCount();
        bool CanSafelySpendResource(int cost);
        IList<int> SelectCounters(int quantity, IList<ClientCard> cards, IList<int> counters);
    }
}
