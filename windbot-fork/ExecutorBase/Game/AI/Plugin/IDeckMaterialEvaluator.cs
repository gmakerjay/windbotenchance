using System.Collections.Generic;

namespace WindBot.Game.AI.Plugin
{
    /// <summary>
    /// Contract for evaluating material costs, protecting boss monsters, and selecting discard/tribute fodder.
    /// </summary>
    public interface IDeckMaterialEvaluator
    {
        int GetMaterialCost(ClientCard card);
        IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1);
        ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1);
        ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1);
    }
}
