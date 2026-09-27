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
    // Runick Domain Plugin Classes
    // ==========================================
    public class RunickPlugin
    {
        public RunickFountainManager FountainManager { get; } = new RunickFountainManager();
        public RunickBanishTracker BanishTracker { get; } = new RunickBanishTracker();
        public RunickStunAdvisor StunAdvisor { get; } = new RunickStunAdvisor();
        public RunickMaterialScorer MaterialScorer { get; } = new RunickMaterialScorer();
        public RunickBoardAssessor BoardAssessor { get; } = new RunickBoardAssessor();
    }

    public class RunickFountainManager
    {
        public bool HasActiveFountain(ClientField bot) => bot.HasInSpellZone(_2026_RunickExecutor.CardId.RunickFountain);
        public int CountRunickSpellsInGrave(ClientField bot) =>
            bot.Graveyard.Count(c => c != null && _2026_RunickExecutor.RunickSpells.Contains(c.Id));
    }

    public class RunickBanishTracker
    {
        public int EstimatedDeckSize { get; set; } = 40;
        public void Deduct(int count) => EstimatedDeckSize = Math.Max(0, EstimatedDeckSize - count);
        public bool IsDeckOutNear() => EstimatedDeckSize <= 10;
    }

    public class RunickStunAdvisor
    {
        public bool IsStunMonster(int cardId) =>
            cardId == _2026_RunickExecutor.CardId.InspectorBoarder ||
            cardId == _2026_RunickExecutor.CardId.ThunderKingRaiOh;
    }

    public class RunickMaterialScorer
    {
        public int ScoreRunickFusion(ClientCard card)
        {
            if (card == null) return 0;
            if (card.IsCode(_2026_RunickExecutor.CardId.HuginTheRunickWings)) return 50;
            if (card.IsCode(_2026_RunickExecutor.CardId.GeriTheRunickFangs)) return 40;
            return 20;
        }
    }

    public class RunickBoardAssessor
    {
        public bool IsOpponentLocked(ClientField enemy, ClientField bot)
        {
            bool hasSkillDrain = bot.HasInSpellZone(_2026_RunickExecutor.CardId.SkillDrain) || enemy.HasInSpellZone(_2026_RunickExecutor.CardId.SkillDrain);
            bool hasBoarder = bot.HasInMonstersZone(_2026_RunickExecutor.CardId.InspectorBoarder);
            return hasSkillDrain || hasBoarder;
        }
    }
}
