using System;
using System.Collections.Generic;
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
    // Evil Twin Domain Plugin Classes
    // ==========================================
    public class EvilTwinPlugin
    {
        public LiveTwinLinkAdvisor LinkAdvisor { get; } = new LiveTwinLinkAdvisor();
        public FiendsmithEngineAdvisor FiendsmithAdvisor { get; } = new FiendsmithEngineAdvisor();
        public EvilTwinMaterialScorer MaterialScorer { get; } = new EvilTwinMaterialScorer();
        public EvilTwinBoardAssessor BoardAssessor { get; } = new EvilTwinBoardAssessor();
    }

    public class LiveTwinLinkAdvisor
    {
        public bool HasBothTwinsInGrave(ClientField bot)
        {
            bool hasKiSikil = bot.Graveyard.Any(c => c != null && (c.Id == _2026_EvilTwinExecutor.CardId.EvilTwinKiSikil || c.Id == _2026_EvilTwinExecutor.CardId.EvilTwinKiSikilDeal));
            bool hasLilla = bot.Graveyard.Any(c => c != null && c.Id == _2026_EvilTwinExecutor.CardId.EvilTwinLilla);
            return hasKiSikil && hasLilla;
        }
    }

    public class FiendsmithEngineAdvisor
    {
        public bool CanTractEngraver(ClientField bot) =>
            bot.HasInHand(_2026_EvilTwinExecutor.CardId.FiendsmithsTract);
    }

    public class EvilTwinMaterialScorer
    {
        public int ScoreMaterial(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == _2026_EvilTwinExecutor.CardId.FabledLurrie) return 50;
            if (card.Id == _2026_EvilTwinExecutor.CardId.LiveTwinKiSikilFrost) return 30;
            return 10;
        }
    }

    public class EvilTwinBoardAssessor
    {
        public bool HasTroubleSunny(ClientField bot) =>
            bot.HasInMonstersZone(_2026_EvilTwinExecutor.CardId.EvilTwinsTroubleSunny);
    }
}
