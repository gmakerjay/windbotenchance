using System;
using System.Collections.Generic;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ==========================================
    // Legacy Decks — no custom combo logic,
    // use DefaultExecutor's built-in actions
    // ==========================================
    [Deck("Cyberse")]
    public class CyberseExecutor : DefaultExecutor
    {
        public CyberseExecutor(GameAI ai, Duel duel) : base(ai, duel) { }
    }

    [Deck("Gren Maju Stun")]
    public class GrenMajuStunExecutor : DefaultExecutor
    {
        public GrenMajuStunExecutor(GameAI ai, Duel duel) : base(ai, duel) { }
    }

    [Deck("Normal Monster Mash")]
    public class NormalMonsterMashExecutor : DefaultExecutor
    {
        public NormalMonsterMashExecutor(GameAI ai, Duel duel) : base(ai, duel) { }
    }

    [Deck("Normal Monster Mash II")]
    public class NormalMonsterMashIIExecutor : DefaultExecutor
    {
        public NormalMonsterMashIIExecutor(GameAI ai, Duel duel) : base(ai, duel) { }
    }

    [Deck("R5NK")]
    public class R5NKExecutor : DefaultExecutor
    {
        public R5NKExecutor(GameAI ai, Duel duel) : base(ai, duel) { }
    }

    [Deck("Rose Scrap Synchro")]
    public class RoseScrapSynchroExecutor : DefaultExecutor
    {
        public RoseScrapSynchroExecutor(GameAI ai, Duel duel) : base(ai, duel) { }
    }

    [Deck("Windwitch Gusto")]
    public class WindwitchGustoExecutor : DefaultExecutor
    {
        public WindwitchGustoExecutor(GameAI ai, Duel duel) : base(ai, duel) { }
    }

}
