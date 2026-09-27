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
internal class LabrynthPlugin
    {
        public _2026_LabrynthExecutor Executor { get; }
        public LabrynthTrapManager TrapManager { get; }
        public LabrynthMaterialScorer MaterialScorer { get; }
        public LabrynthBoardAssessor BoardAssessor { get; }

        public LabrynthPlugin(_2026_LabrynthExecutor executor)
        {
            Executor = executor;
            TrapManager = new LabrynthTrapManager(executor);
            MaterialScorer = new LabrynthMaterialScorer(executor);
            BoardAssessor = new LabrynthBoardAssessor(executor);
        }
    }

    internal class LabrynthTrapManager
    {
        private readonly _2026_LabrynthExecutor _executor;
        public LabrynthTrapManager(_2026_LabrynthExecutor executor) => _executor = executor;

        public bool HasActiveDisruption()
        {
            return _executor.Bot.GetSpells().Any(s => s != null && s.IsFacedown() && s.HasType(CardType.Trap));
        }

        public int CountNormalTrapsInDeck()
        {
            return _executor.Bot.Deck.Count(c => c != null && _executor.IsNormalTrap(c));
        }
    }

    internal class LabrynthMaterialScorer
    {
        private readonly _2026_LabrynthExecutor _executor;
        public LabrynthMaterialScorer(_2026_LabrynthExecutor executor) => _executor = executor;

        public int Score(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == _2026_LabrynthExecutor.CardId.AbsoluteKingBackJack) return 100;
            if (card.Id == _2026_LabrynthExecutor.CardId.LabrynthStovieTorbie) return 90;
            if (card.Id == _2026_LabrynthExecutor.CardId.LabrynthChandraglier) return 85;
            if (_executor.IsAceCard(card)) return -500;
            return 10;
        }
    }

    internal class LabrynthBoardAssessor
    {
        private readonly _2026_LabrynthExecutor _executor;
        public LabrynthBoardAssessor(_2026_LabrynthExecutor executor) => _executor = executor;

        public bool IsLabrynthDominating()
        {
            bool hasLadyOrLovely = _executor.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                (m.Id == _2026_LabrynthExecutor.CardId.LadyLabrynthOfTheSilverCastle ||
                 m.Id == _2026_LabrynthExecutor.CardId.LovelyLabrynthOfTheSilverCastle));
            int setTraps = _executor.Bot.GetSpells().Count(s => s != null && s.IsFacedown());
            return hasLadyOrLovely && setTraps >= 2;
        }
    }
}
