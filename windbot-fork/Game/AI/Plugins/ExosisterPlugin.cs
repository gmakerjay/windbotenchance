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
    // Exosister Domain Plugin Classes
    // ==========================================
    public class ExosisterPlugin
    {
        public ExosisterPairAdvisor PairAdvisor { get; } = new ExosisterPairAdvisor();
        public ExosisterBanishPlanner BanishPlanner { get; } = new ExosisterBanishPlanner();
        public ExosisterMaterialScorer MaterialScorer { get; } = new ExosisterMaterialScorer();
        public ExosisterBoardAssessor BoardAssessor { get; } = new ExosisterBoardAssessor();
    }

    public class ExosisterPairAdvisor
    {
        public bool IsPairedSisterOnField(Duel duel, ClientField bot, int sisterId)
        {
            int partnerId = sisterId switch
            {
                _2026_ExosisterExecutor.CardId.Martha => _2026_ExosisterExecutor.CardId.Elis,
                _2026_ExosisterExecutor.CardId.Elis => _2026_ExosisterExecutor.CardId.Stella,
                _2026_ExosisterExecutor.CardId.Stella => _2026_ExosisterExecutor.CardId.Elis,
                _2026_ExosisterExecutor.CardId.Sophia => _2026_ExosisterExecutor.CardId.Irene,
                _2026_ExosisterExecutor.CardId.Irene => _2026_ExosisterExecutor.CardId.Sophia,
                _ => 0
            };
            return partnerId != 0 && bot.HasInMonstersZone(partnerId);
        }
    }

    public class ExosisterBanishPlanner
    {
        public bool ShouldPreserveTarget(ClientCard card)
        {
            return card != null && card.Controller == 0;
        }
    }

    public class ExosisterMaterialScorer
    {
        public int ScoreExosisterMaterial(ClientCard card)
        {
            if (card == null) return 0;
            if (card.IsCode(_2026_ExosisterExecutor.CardId.Martha)) return 30;
            if (card.IsCode(_2026_ExosisterExecutor.CardId.Elis)) return 20;
            return 10;
        }
    }

    public class ExosisterBoardAssessor
    {
        public bool HasMagnifica(ClientField bot) => bot.HasInMonstersZone(_2026_ExosisterExecutor.CardId.Magnifica);
        public bool HasSetReturnia(ClientField bot) => bot.HasInSpellZone(_2026_ExosisterExecutor.CardId.Returnia);
    }
}
