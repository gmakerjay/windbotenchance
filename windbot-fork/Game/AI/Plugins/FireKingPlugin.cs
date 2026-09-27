using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    // ==========================================
    // Fire King Domain Plugin Classes
    // ==========================================
    public class FireKingPlugin
    {
        public FireKingDestructionAdvisor DestructionAdvisor { get; } = new FireKingDestructionAdvisor();
        public FireKingXyzAdvisor XyzAdvisor { get; } = new FireKingXyzAdvisor();
        public FireKingMaterialScorer MaterialScorer { get; } = new FireKingMaterialScorer();
        public FireKingBoardAssessor BoardAssessor { get; } = new FireKingBoardAssessor();
    }

    public class FireKingDestructionAdvisor
    {
        public bool HasDestructionTriggerInHand(ClientField bot) =>
            bot.HasInHand(_2026_FirekingExecutor.CardId.SacredFireKingGarunix) ||
            bot.HasInHand(_2026_FirekingExecutor.CardId.FireKingHighAvatarKirin);
    }

    public class FireKingXyzAdvisor
    {
        public bool CanRank8GarunixEternity(ClientField bot) =>
            bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 8) >= 2;
    }

    public class FireKingMaterialScorer
    {
        public int ScorePopTarget(ClientCard card)
        {
            if (card == null) return 0;
            if (card.IsCode(_2026_FirekingExecutor.CardId.LegendaryFireKingPonix)) return 100;
            if (card.IsCode(_2026_FirekingExecutor.CardId.FireKingCourtierUlcanix)) return 90;
            if (card.IsCode(_2026_FirekingExecutor.CardId.FireKingAvatarKirin)) return 80;
            return 20;
        }
    }

    public class FireKingBoardAssessor
    {
        public bool HasGarunixEternity(ClientField bot) =>
            bot.HasInMonstersZone(_2026_FirekingExecutor.CardId.GarunixEternity);

        public bool HasPrometheanPrincess(ClientField bot) =>
            bot.HasInMonstersZone(_2026_FirekingExecutor.CardId.PrometheanPrincess);
    }
}
