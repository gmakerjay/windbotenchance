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
public class MimighoulPlugin
    {
        private readonly ModernExecutor _exec;
        public MimighoulPlugin(ModernExecutor exec) => _exec = exec;

        public bool HasDungeonLock => _exec.Bot.HasInSpellZone(_2026_MimighoulExecutor.CardId.Dungeon);
        public bool HasFacedownMimighoul => _exec.Bot.GetMonsters().Any(c => c != null && c.IsFacedown() && _2026_MimighoulExecutor.MimighoulMonsters.Contains(c.Id));
    }

    public class MimighoulFlipAdvisor
    {
        private readonly ModernExecutor _exec;
        public MimighoulFlipAdvisor(ModernExecutor exec) => _exec = exec;

        public ClientCard BestFlipTarget()
        {
            return _exec.Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFacedown() && _2026_MimighoulExecutor.MimighoulMonsters.Contains(c.Id));
        }

        public bool ShouldTriggerFlip()
        {
            return _exec.Enemy.GetMonsterCount() > 0 || _exec.Duel.Player == 1;
        }
    }

    public class MimighoulXyzAdvisor
    {
        private readonly ModernExecutor _exec;
        public MimighoulXyzAdvisor(ModernExecutor exec) => _exec = exec;

        public bool ShouldSummonThrone()
        {
            return !_exec.Bot.HasInMonstersZone(_2026_MimighoulExecutor.CardId.Throne) &&
                   _exec.Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 1) >= 2;
        }

        public bool ShouldSummonGiant()
        {
            return !_exec.Bot.HasInMonstersZone(_2026_MimighoulExecutor.CardId.GiantMimighoul) &&
                   _exec.Enemy.GetMonsters().Any(c => c != null && c.IsFaceup()) &&
                   _exec.Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 1) >= 2;
        }
    }

    public class MimighoulMaterialScorer
    {
        public static int GetScore(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Id == _2026_MimighoulExecutor.CardId.Throne || c.Id == _2026_MimighoulExecutor.CardId.GiantMimighoul)
                return 900;
            if (c.IsCode(_2026_MimighoulExecutor.CardId.AshBlossom, _2026_MimighoulExecutor.CardId.MaxxC))
                return 800;
            if (_2026_MimighoulExecutor.MimighoulMonsters.Contains(c.Id))
                return 600;
            return 100;
        }
    }

    public class MimighoulBoardAssessor
    {
        private readonly ModernExecutor _exec;
        public MimighoulBoardAssessor(ModernExecutor exec) => _exec = exec;

        public int EvaluateControlScore()
        {
            int score = 0;
            if (_exec.Bot.HasInMonstersZone(_2026_MimighoulExecutor.CardId.Throne)) score += 40;
            if (_exec.Bot.HasInMonstersZone(_2026_MimighoulExecutor.CardId.GiantMimighoul)) score += 35;
            if (_exec.Bot.HasInSpellZone(_2026_MimighoulExecutor.CardId.Dungeon)) score += 30;
            score += _exec.Bot.GetMonsters().Count(c => c != null && c.IsFacedown()) * 15;
            score += _exec.Bot.GetSpells().Count(c => c != null && c.IsFacedown()) * 10;
            return score;
        }
    }
}
