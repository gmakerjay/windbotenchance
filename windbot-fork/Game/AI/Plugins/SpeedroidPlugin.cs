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
public class SpeedroidPlugin
    {
        private readonly ModernExecutor _exec;
        public SpeedroidPlugin(ModernExecutor exec) => _exec = exec;

        public bool HasBossOnField => _exec.Bot.HasInMonstersZone(_2026_SpeedroidExecutor.CardId.DragunityKnightAreadbhair) ||
                                      _exec.Bot.HasInMonstersZone(_2026_SpeedroidExecutor.CardId.DragunityLordGeorgius);
    }

    public class SpeedroidSynchroAdvisor
    {
        private readonly ModernExecutor _exec;
        public SpeedroidSynchroAdvisor(ModernExecutor exec) => _exec = exec;

        public bool CanSynchroSummon(int targetLevel)
        {
            var faceup = _exec.Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            bool hasTuner = faceup.Any(c => c.IsTuner());
            bool hasNonTuner = faceup.Any(c => !c.IsTuner());
            return hasTuner && hasNonTuner;
        }
    }

    public class SpeedroidWindLockAdvisor
    {
        private readonly ModernExecutor _exec;
        public SpeedroidWindLockAdvisor(ModernExecutor exec) => _exec = exec;

        public bool IsWindOnlyLocked => _exec.Bot.GetMonsters().Any(c => c != null && c.Id == _2026_SpeedroidExecutor.CardId.RubberBandShooter);
    }

    public class SpeedroidMaterialScorer
    {
        public static int GetScore(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Id == _2026_SpeedroidExecutor.CardId.DragunityKnightAreadbhair ||
                c.Id == _2026_SpeedroidExecutor.CardId.DragunityLordGeorgius ||
                c.Id == _2026_SpeedroidExecutor.CardId.Typhon)
                return 950;
            if (c.Id == _2026_SpeedroidExecutor.CardId.SpeedroidFukiModoshiPiper ||
                c.Id == _2026_SpeedroidExecutor.CardId.SpeedroidDenDenDaikoDuke)
                return 200;
            return 500;
        }
    }

    public class SpeedroidBoardAssessor
    {
        private readonly ModernExecutor _exec;
        public SpeedroidBoardAssessor(ModernExecutor exec) => _exec = exec;

        public int EvaluateWindBoardScore()
        {
            int score = 0;
            if (_exec.Bot.HasInMonstersZone(_2026_SpeedroidExecutor.CardId.DragunityKnightAreadbhair)) score += 40;
            if (_exec.Bot.HasInMonstersZone(_2026_SpeedroidExecutor.CardId.DragunityLordGeorgius)) score += 35;
            if (_exec.Bot.HasInMonstersZone(_2026_SpeedroidExecutor.CardId.TotemBird)) score += 25;
            if (_exec.Bot.HasInMonstersZone(_2026_SpeedroidExecutor.CardId.LinkVaruroon)) score += 25;
            if (_exec.Bot.HasInMonstersZone(_2026_SpeedroidExecutor.CardId.VibrantVortex)) score += 20;
            return score;
        }
    }
}
