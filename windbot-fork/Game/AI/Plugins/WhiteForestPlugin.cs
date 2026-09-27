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
    // White Forest Domain Plugin Classes
    // ==========================================
    public class WhiteForestPlugin
    {
        public WhiteForestSynchroAdvisor SynchroAdvisor { get; } = new WhiteForestSynchroAdvisor();
        public WhiteForestToyEngine ToyEngine { get; } = new WhiteForestToyEngine();
        public WhiteForestMaterialScorer MaterialScorer { get; } = new WhiteForestMaterialScorer();
        public WhiteForestBoardAssessor BoardAssessor { get; } = new WhiteForestBoardAssessor();
    }

    public class WhiteForestSynchroAdvisor
    {
        public bool CanSynchroLadder(ClientField bot)
        {
            return bot.HasInMonstersZone(WhiteForestExecutor.CardId.RcielaSinisterSoulOfTheWhiteForest) ||
                   bot.HasInMonstersZone(WhiteForestExecutor.CardId.SilveraWolfTamerOfTheWhiteForest);
        }
    }

    public class WhiteForestToyEngine
    {
        public bool HasToyBoxInSpellZone(ClientField bot) =>
            bot.HasInSpellZone(WhiteForestExecutor.CardId.ToyBox);
    }

    public class WhiteForestMaterialScorer
    {
        public int ScoreMaterial(ClientCard card)
        {
            if (card == null) return 0;
            if (card.IsCode(WhiteForestExecutor.CardId.AstellarOfTheWhiteForest)) return 30;
            if (card.IsCode(WhiteForestExecutor.CardId.ElzetteOfTheWhiteForest)) return 20;
            return 10;
        }
    }

    public class WhiteForestBoardAssessor
    {
        public bool HasQueen(ClientField bot) =>
            bot.HasInMonstersZone(WhiteForestExecutor.CardId.DiabellQueenOfTheWhiteForest);

        public bool HasDiabellze(ClientField bot) =>
            bot.HasInMonstersZone(WhiteForestExecutor.CardId.DiabellzeTheWhiteWitch);
    }
}
