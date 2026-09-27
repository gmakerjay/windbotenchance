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
internal class MalissPlugin
    {
        public _2026_MalissExecutor Executor { get; }
        public MalissBanishManager BanishManager { get; }
        public MalissLinkAdvisor LinkAdvisor { get; }
        public MalissMaterialScorer MaterialScorer { get; }
        public MalissBoardAssessor BoardAssessor { get; }

        public MalissPlugin(_2026_MalissExecutor executor)
        {
            Executor = executor;
            BanishManager = new MalissBanishManager(executor);
            LinkAdvisor = new MalissLinkAdvisor(executor);
            MaterialScorer = new MalissMaterialScorer(executor);
            BoardAssessor = new MalissBoardAssessor(executor);
        }
    }

    internal class MalissBanishManager
    {
        private readonly _2026_MalissExecutor _executor;
        public MalissBanishManager(_2026_MalissExecutor executor) => _executor = executor;

        public bool CanTriggerBanishSummon()
        {
            return _executor.Bot.LifePoints > 1000 && _executor.Bot.GetMonsterCount() < 5;
        }

        public int CountBanishedMaliss()
        {
            return _executor.Bot.Banished.Count(c => c != null && c.HasSetcode(0x1b9));
        }
    }

    internal class MalissLinkAdvisor
    {
        private readonly _2026_MalissExecutor _executor;
        public MalissLinkAdvisor(_2026_MalissExecutor executor) => _executor = executor;

        public bool ShouldClimbToHeartsCrypter()
        {
            return !_executor.Bot.HasInMonstersZone(_2026_MalissExecutor.CardId.HeartsCrypter) &&
                   _executor.Bot.GetMonsters().Count(m => m != null && m.IsFaceup()) >= 3;
        }
    }

    internal class MalissMaterialScorer
    {
        private readonly _2026_MalissExecutor _executor;
        public MalissMaterialScorer(_2026_MalissExecutor executor) => _executor = executor;

        public int Score(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == _2026_MalissExecutor.CardId.Linguriboh || card.Id == _2026_MalissExecutor.CardId.LinkDisciple) return 100;
            if (card.Id == _2026_MalissExecutor.CardId.WizardIgnister || card.Id == _2026_MalissExecutor.CardId.BackupIgnister) return 90;
            if (card.Id == _2026_MalissExecutor.CardId.HeartsCrypter) return -500;
            return 10;
        }
    }

    internal class MalissBoardAssessor
    {
        private readonly _2026_MalissExecutor _executor;
        public MalissBoardAssessor(_2026_MalissExecutor executor) => _executor = executor;

        public bool HasStrongBoard()
        {
            return _executor.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                (m.Id == _2026_MalissExecutor.CardId.HeartsCrypter || m.Id == _2026_MalissExecutor.CardId.FirewallDragon));
        }
    }
}
