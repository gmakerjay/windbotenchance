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
public class SprightPlugin
    {
        public bool StarterUsed { get; set; }
        public bool GiganticUsed { get; set; }
        public bool ElfReviveUsed { get; set; }
        public bool SprindDumpUsed { get; set; }
        public bool BlueSearchUsed { get; set; }
        public bool JetSearchUsed { get; set; }

        public void ResetTurn()
        {
            StarterUsed = false;
            GiganticUsed = false;
            ElfReviveUsed = false;
            SprindDumpUsed = false;
            BlueSearchUsed = false;
            JetSearchUsed = false;
        }
    }

    public static class SprightLevel2Engine
    {
        public static bool IsLevel2(ClientCard card)
        {
            if (card == null) return false;
            return card.Level == 2 || card.Rank == 2 || (card.HasType(CardType.Link) && card.LinkMarker == 2);
        }

        public static int CountLevel2OnField(ClientField field)
        {
            return field.GetMonsters().Count(m => m != null && m.IsFaceup() && IsLevel2(m));
        }
    }

    public static class SprightNegateAdvisor
    {
        public static ClientCard SelectTributeForNegate(ClientField bot, int executorId)
        {
            var monsters = bot.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            var expendable = monsters.FirstOrDefault(m => m.IsCode(_2026_SprightExecutor.CardId.DupeFrog, _2026_SprightExecutor.CardId.NimbleBeaver, _2026_SprightExecutor.CardId.Ronintoadin));
            if (expendable != null) return expendable;
            return monsters.FirstOrDefault(m => m.IsCode(executorId));
        }
    }

    public static class SprightMaterialScorer
    {
        public static int ScoreForMaterial(ClientCard card)
        {
            if (card == null) return 999;
            if (card.IsCode(_2026_SprightExecutor.CardId.NimbleBeaver, _2026_SprightExecutor.CardId.Ronintoadin)) return 10;
            if (card.IsCode(_2026_SprightExecutor.CardId.SwapFrog, _2026_SprightExecutor.CardId.DupeFrog)) return 20;
            if (card.IsCode(_2026_SprightExecutor.CardId.SprightBlue, _2026_SprightExecutor.CardId.SprightJet)) return 30;
            if (card.IsCode(_2026_SprightExecutor.CardId.SprightRed, _2026_SprightExecutor.CardId.SprightCarrot)) return 50;
            if (card.IsCode(_2026_SprightExecutor.CardId.SprightElf, _2026_SprightExecutor.CardId.SprightSprind)) return 80;
            if (card.IsCode(_2026_SprightExecutor.CardId.GiganticSpright, _2026_SprightExecutor.CardId.ToadallyAwesome)) return 100;
            return 40;
        }
    }

    public static class SprightBoardAssessor
    {
        public static bool CanOTKWithGammaBurst(ClientField bot, ClientField enemy)
        {
            int l2Count = SprightLevel2Engine.CountLevel2OnField(bot);
            if (l2Count == 0) return false;
            int totalAtk = bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack + 1400);
            return totalAtk >= enemy.LifePoints && enemy.GetMonsterCount() == 0;
        }
    }
}
