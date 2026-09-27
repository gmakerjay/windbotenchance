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
internal class KashtiraPlugin
    {
        public KashtiraExecutor Executor { get; }
        public KashtiraZoneLockManager ZoneLockManager { get; }
        public KashtiraMaterialScorer MaterialScorer { get; }
        public KashtiraBoardAssessor BoardAssessor { get; }

        public KashtiraPlugin(KashtiraExecutor executor)
        {
            Executor = executor;
            ZoneLockManager = new KashtiraZoneLockManager(executor);
            MaterialScorer = new KashtiraMaterialScorer(executor);
            BoardAssessor = new KashtiraBoardAssessor(executor);
        }
    }

    internal class KashtiraZoneLockManager
    {
        private readonly KashtiraExecutor _executor;

        public KashtiraZoneLockManager(KashtiraExecutor executor)
        {
            _executor = executor;
        }

        public int SelectLockZone(int availableZones, CardLocation location)
        {
            // Preference order for locking opponent Monster Zones:
            // Center (Zone 2) -> Left-Center (Zone 1) -> Right-Center (Zone 3) -> Extra Monster Zones (5, 6)
            int[] monsterPref = { 0x4, 0x2, 0x8, 0x1, 0x10, 0x20, 0x40 };
            foreach (int mask in monsterPref)
            {
                if ((availableZones & mask) != 0) return mask;
            }

            // Spell & Trap Zones: Middle columns first
            int[] spellPref = { 0x400, 0x200, 0x800, 0x100, 0x1000 };
            foreach (int mask in spellPref)
            {
                if ((availableZones & mask) != 0) return mask;
            }

            return availableZones;
        }
    }

    internal class KashtiraMaterialScorer
    {
        private readonly KashtiraExecutor _executor;

        public KashtiraMaterialScorer(KashtiraExecutor executor)
        {
            _executor = executor;
        }

        public int GetScore(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == KashtiraExecutor.CardId.KashtiraAriseHeart) return 10000;
            if (card.Id == KashtiraExecutor.CardId.KashtiraShangriIra) return 8000;
            if (card.Id == KashtiraExecutor.CardId.KashtiraFenrir) return 5000;
            if (card.Id == KashtiraExecutor.CardId.KashtiraUnicorn) return 4000;
            return 1000;
        }
    }

    internal class KashtiraBoardAssessor
    {
        private readonly KashtiraExecutor _executor;

        public KashtiraBoardAssessor(KashtiraExecutor executor)
        {
            _executor = executor;
        }

        public bool HasMacroCosmosLock()
        {
            return _executor.Bot.HasInMonstersZone(KashtiraExecutor.CardId.KashtiraAriseHeart);
        }

        public bool HasZoneLockEngine()
        {
            return _executor.Bot.HasInMonstersZone(KashtiraExecutor.CardId.KashtiraShangriIra);
        }
    }
}
