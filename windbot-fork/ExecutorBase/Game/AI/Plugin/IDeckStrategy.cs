using System.Collections.Generic;

namespace WindBot.Game.AI.Plugin
{
    /// <summary>
    /// Contract for deck-specific combo sequencing and high-level target selection.
    /// </summary>
    public interface IDeckStrategy
    {
        void Reset();
        ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates);
        ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context);
        ClientCard PickFoolishGraveTarget(IList<ClientCard> candidates, ClientCard context) => null;
    }
}
