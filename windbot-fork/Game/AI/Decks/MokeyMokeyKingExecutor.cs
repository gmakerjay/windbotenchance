using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugins;

namespace WindBot.Game.AI.Decks
{
    [Deck("Normal Monster Mash II", "AI_MokeyMokeyKing")]
    [Deck("MokeyMokeyKing", "AI_MokeyMokeyKing", "Easy")]
    public class MokeyMokeyKingExecutor : ModernExecutor
    {
        public ClientCard CurrentExecutingCard => Card;

        public class CardId
        {
            public const int LeoWizard = 4392470;
            public const int Bunilla = 69380702;
        }

        private int RockCount = 0;

        public MokeyMokeyKingExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            DeckPlugin = new NormalMonsterMashPlugin(this, "MokeyMokeyKing");

            AddExecutor(ExecutorType.SpSummon);
            AddExecutor(ExecutorType.SummonOrSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
            AddExecutor(ExecutorType.Activate, DefaultField);
        }

        public override int OnRockPaperScissors()
        {
            RockCount++;
            if (RockCount <= 3)
                return 2;
            else
                return base.OnRockPaperScissors();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (DeckPlugin is NormalMonsterMashPlugin nmPlugin)
            {
                var selected = nmPlugin.SelectCardLogic(cards, min, max, hint, cancelable);
                if (selected != null && selected.Count >= min)
                    return selected;
            }
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}