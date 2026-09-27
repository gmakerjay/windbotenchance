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
    // Sky Striker Domain Plugin Classes
    // ==========================================
    public class SkyStrikerPlugin
    {
        public SkyStrikerSpellCounter SpellCounter { get; } = new SkyStrikerSpellCounter();
        public SkyStrikerAceRouter AceRouter { get; } = new SkyStrikerAceRouter();
        public SkyStrikerMaterialScorer MaterialScorer { get; } = new SkyStrikerMaterialScorer();
        public SkyStrikerBoardAssessor BoardAssessor { get; } = new SkyStrikerBoardAssessor();
    }

    public class SkyStrikerSpellCounter
    {
        public int CountSpellsInGrave(ClientField bot) =>
            bot.Graveyard.Count(c => c != null && c.IsSpell());

        public bool HasThreeSpellsInGrave(ClientField bot) =>
            CountSpellsInGrave(bot) >= 3;
    }

    public class SkyStrikerAceRouter
    {
        public bool HasMainMonsterZoneOccupied(ClientField bot) =>
            bot.GetMonsters().Any(c => c != null && (c.Sequence < 5));
    }

    public class SkyStrikerMaterialScorer
    {
        public int ScoreLinkMaterial(ClientCard card)
        {
            if (card == null) return 0;
            if (card.IsCode(_2026_SkyStrikerExecutor.CardId.Token)) return 100;
            if (card.IsCode(_2026_SkyStrikerExecutor.CardId.Roze)) return 80;
            if (card.IsCode(_2026_SkyStrikerExecutor.CardId.Raye)) return 60;
            return 10;
        }
    }

    public class SkyStrikerBoardAssessor
    {
        public bool HasKagari(ClientField bot) =>
            bot.HasInMonstersZone(_2026_SkyStrikerExecutor.CardId.Kagari);

        public bool HasShizuku(ClientField bot) =>
            bot.HasInMonstersZone(_2026_SkyStrikerExecutor.CardId.Shizuku);
    }
}
