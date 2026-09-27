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
internal class TenpaiPlugin
    {
        public TenpaiExecutor Executor { get; }
        public TenpaiBattleOTKPlanner OTKPlanner { get; }
        public TenpaiMaterialScorer MaterialScorer { get; }
        public TenpaiBoardAssessor BoardAssessor { get; }

        public TenpaiPlugin(TenpaiExecutor executor)
        {
            Executor = executor;
            OTKPlanner = new TenpaiBattleOTKPlanner(executor);
            MaterialScorer = new TenpaiMaterialScorer(executor);
            BoardAssessor = new TenpaiBoardAssessor(executor);
        }
    }

    internal class TenpaiBattleOTKPlanner
    {
        private readonly TenpaiExecutor _executor;

        public TenpaiBattleOTKPlanner(TenpaiExecutor executor)
        {
            _executor = executor;
        }

        public bool HasBattleLockdown()
        {
            return _executor.Bot.HasInMonstersZone(TenpaiExecutor.CardId.SangenpaiTranscendentDragion);
        }

        public bool CanExecuteTridentTripleAttack()
        {
            return _executor.Bot.HasInMonstersZone(TenpaiExecutor.CardId.TridentDragion) &&
                   _executor.Bot.GetSpellCount() + _executor.Bot.GetMonsterCount() >= 2;
        }
    }

    internal class TenpaiMaterialScorer
    {
        private readonly TenpaiExecutor _executor;

        public TenpaiMaterialScorer(TenpaiExecutor executor)
        {
            _executor = executor;
        }

        public int GetScore(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == TenpaiExecutor.CardId.TridentDragion) return 10000;
            if (card.Id == TenpaiExecutor.CardId.SangenpaiTranscendentDragion) return 8000;
            if (card.Id == TenpaiExecutor.CardId.SangenpaiBidentDragion) return 5000;
            return 1000;
        }
    }

    internal class TenpaiBoardAssessor
    {
        private readonly TenpaiExecutor _executor;

        public TenpaiBoardAssessor(TenpaiExecutor executor)
        {
            _executor = executor;
        }

        public int CalculateFieldAttack()
        {
            return _executor.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
        }

        public bool HasLethalBoard()
        {
            return CalculateFieldAttack() >= _executor.Enemy.LifePoints;
        }
    }
}
