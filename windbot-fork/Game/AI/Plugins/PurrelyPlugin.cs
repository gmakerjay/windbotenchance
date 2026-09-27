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
public class PurrelyPlugin
    {
        public bool NormalSummonUsed { get; set; }
        public bool PurrelylySearchUsed { get; set; }
        public bool PurrelyExcavateUsed { get; set; }
        public bool MyFriendUsed { get; set; }
        public bool StreetAttachUsed { get; set; }

        public void ResetTurn()
        {
            NormalSummonUsed = false;
            PurrelylySearchUsed = false;
            PurrelyExcavateUsed = false;
            MyFriendUsed = false;
            StreetAttachUsed = false;
        }
    }

    public static class PurrelyMemoryManager
    {
        public static int GetMemoryPriority(int cardId, bool isGoingFirst)
        {
            switch (cardId)
            {
                case _2026_PurrelyExecutor.CardId.PurrelySleepyMemory:
                    return isGoingFirst ? 100 : 80;
                case _2026_PurrelyExecutor.CardId.PurrelyDeliciousMemory:
                    return isGoingFirst ? 95 : 95;
                case _2026_PurrelyExecutor.CardId.PurrelyPrettyMemory:
                    return isGoingFirst ? 80 : 90;
                case _2026_PurrelyExecutor.CardId.PurrelyHappyMemory:
                    return isGoingFirst ? 70 : 85;
                default:
                    return 50;
            }
        }
    }

    public static class PurrelyXyzRankUpAdvisor
    {
        public static int GetBestRank2Target(bool goingFirst, bool hasSleepy, bool hasDelicious, bool hasPretty)
        {
            if (goingFirst)
            {
                if (hasDelicious) return _2026_PurrelyExecutor.CardId.EpurrelyPlump;
                if (hasPretty) return _2026_PurrelyExecutor.CardId.EpurrelyBeauty;
                return _2026_PurrelyExecutor.CardId.EpurrelyPlump;
            }
            else
            {
                if (hasDelicious) return _2026_PurrelyExecutor.CardId.EpurrelyPlump;
                if (hasPretty) return _2026_PurrelyExecutor.CardId.EpurrelyBeauty;
                return _2026_PurrelyExecutor.CardId.EpurrelyHappiness;
            }
        }
    }

    public static class PurrelyMaterialScorer
    {
        public static int ScoreMaterialForDetach(ClientCard card)
        {
            if (card == null) return 999;
            if (card.IsCode(_2026_PurrelyExecutor.CardId.Purrely, _2026_PurrelyExecutor.CardId.Purrelyly)) return 5;
            if (card.IsCode(_2026_PurrelyExecutor.CardId.PurrelyHappyMemory)) return 20;
            if (card.IsCode(_2026_PurrelyExecutor.CardId.PurrelyPrettyMemory)) return 30;
            if (card.IsCode(_2026_PurrelyExecutor.CardId.PurrelyDeliciousMemory)) return 80;
            if (card.IsCode(_2026_PurrelyExecutor.CardId.PurrelySleepyMemory)) return 100;
            return 50;
        }
    }

    public static class PurrelyBoardAssessor
    {
        public static bool HasTowerNoir(ClientField bot)
        {
            return bot.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                m.IsCode(_2026_PurrelyExecutor.CardId.ExpurrelyNoir) && m.Overlays.Count >= 5);
        }
    }
}
