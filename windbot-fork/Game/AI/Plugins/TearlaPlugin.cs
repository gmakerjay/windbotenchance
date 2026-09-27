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
public class TearlaPlugin
    {
        public bool ReinoheartSummonUsed { get; set; }
        public bool ScheirenSpSummonUsed { get; set; }
        public bool KitkallosSearchUsed { get; set; }
        public bool KitkallosSelfPopUsed { get; set; }
        public bool TearKashtiraMillUsed { get; set; }

        public void ResetTurn()
        {
            ReinoheartSummonUsed = false;
            ScheirenSpSummonUsed = false;
            KitkallosSearchUsed = false;
            KitkallosSelfPopUsed = false;
            TearKashtiraMillUsed = false;
        }
    }

    public static class TearlaFusionAdvisor
    {
        public static bool CanFuseFromGY(ClientField bot)
        {
            var tearsInGy = bot.Graveyard.Where(c => c != null && (c.Id == _2026_TearlaExecutor.CardId.TearlamentsScheiren || c.Id == _2026_TearlaExecutor.CardId.TearlamentsHavnis || c.Id == _2026_TearlaExecutor.CardId.TearlamentsKashtira)).ToList();
            return tearsInGy.Count > 0;
        }
    }

    public static class TearlaMillEngine
    {
        public static bool SafeToMill(ClientField bot, int count)
        {
            return bot.Deck.Count >= count + 5;
        }
    }

    public static class TearlaMaterialScorer
    {
        public static int ScoreMaterialForFusion(ClientCard card)
        {
            if (card == null) return 999;
            if (card.Id == _2026_TearlaExecutor.CardId.GemKnightAmethyst) return 1;
            if (card.Id == _2026_TearlaExecutor.CardId.TearlamentsKashtira) return 2;
            if (card.Id == _2026_TearlaExecutor.CardId.TearlamentsHavnis) return 3;
            if (card.Id == _2026_TearlaExecutor.CardId.TearlamentsScheiren) return 4;
            if (card.Id == _2026_TearlaExecutor.CardId.TearlamentsReinoheart) return 5;
            if (card.Id == _2026_TearlaExecutor.CardId.TearlamentsKitkallos) return 6;
            return 50;
        }
    }

    public static class TearlaBoardAssessor
    {
        public static bool HasDisruptionReady(ClientField bot)
        {
            bool hasRulkallos = bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == _2026_TearlaExecutor.CardId.TearlamentsRulkallos);
            bool hasSulliek = bot.GetSpells().Any(s => s != null && s.IsFaceup() && s.Id == _2026_TearlaExecutor.CardId.TearlamentsSulliek);
            bool hasCryme = bot.GetSpells().Any(s => s != null && s.Id == _2026_TearlaExecutor.CardId.TearlamentsCryme);
            return hasRulkallos || hasSulliek || hasCryme;
        }
    }
}
