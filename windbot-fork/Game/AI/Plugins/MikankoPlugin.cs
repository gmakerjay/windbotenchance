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
internal class MikankoPlugin
    {
        public MikankoExecutor Executor { get; }
        public MikankoEquipAdvisor EquipAdvisor { get; }
        public MikankoDamageReflectPlanner ReflectPlanner { get; }
        public MikankoMaterialScorer MaterialScorer { get; }
        public MikankoBoardAssessor BoardAssessor { get; }

        public MikankoPlugin(MikankoExecutor executor)
        {
            Executor = executor;
            EquipAdvisor = new MikankoEquipAdvisor(executor);
            ReflectPlanner = new MikankoDamageReflectPlanner(executor);
            MaterialScorer = new MikankoMaterialScorer(executor);
            BoardAssessor = new MikankoBoardAssessor(executor);
        }
    }

    internal class MikankoEquipAdvisor
    {
        private readonly MikankoExecutor _executor;

        public MikankoEquipAdvisor(MikankoExecutor executor)
        {
            _executor = executor;
        }

        public bool HasArabesqueRemovalTarget()
        {
            return _executor.Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled());
        }

        public bool HasDoubleEdgedLethal()
        {
            // If enemy controls a Kaiju (3000+ ATK) and we have Double-Edged Sword -> 10k reflect OTK
            return _executor.Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 3000);
        }
    }

    internal class MikankoDamageReflectPlanner
    {
        private readonly MikankoExecutor _executor;

        public MikankoDamageReflectPlanner(MikankoExecutor executor)
        {
            _executor = executor;
        }

        public int GetMaxEnemyAttack()
        {
            var enemies = _executor.Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            return enemies.Count > 0 ? enemies.Max(m => m.Attack) : 0;
        }
    }

    internal class MikankoMaterialScorer
    {
        private readonly MikankoExecutor _executor;

        public MikankoMaterialScorer(MikankoExecutor executor)
        {
            _executor = executor;
        }

        public int GetScore(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == MikankoExecutor.CardId.OhimeTheManifestedMikanko) return 10000;
            if (card.Id == MikankoExecutor.CardId.HuLiTheJewelMikanko) return 8000;
            if (card.Id == MikankoExecutor.CardId.HaReTheSwordMikanko) return 6000;
            return 1000;
        }
    }

    internal class MikankoBoardAssessor
    {
        private readonly MikankoExecutor _executor;

        public MikankoBoardAssessor(MikankoExecutor executor)
        {
            _executor = executor;
        }

        public bool HasUntargetableMikankoLock()
        {
            var huli = _executor.Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Id == MikankoExecutor.CardId.HuLiTheJewelMikanko);
            return huli != null && huli.EquipCards.Count > 0;
        }
    }
}
