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
    // ═══════════════════════════════════════════════════════════════
    //  MASTER DECK PLUGIN: MalissPlugin (Special_Maliss Domain Helpers)
    // ═══════════════════════════════════════════════════════════════
    internal class MalissPlugin
    {
        public _2026_MalissExecutor Executor { get; }
        public MalissStrategy Strategy { get; }
        public MalissBanishManager BanishManager { get; }
        public MalissLinkAdvisor LinkAdvisor { get; }
        public MalissMaterialScorer MaterialScorer { get; }
        public MalissBoardAssessor BoardAssessor { get; }
        public MalissThreatEvaluator ThreatEvaluator { get; }

        public MalissPlugin(_2026_MalissExecutor executor)
        {
            Executor = executor;
            Strategy = new MalissStrategy(executor);
            BanishManager = new MalissBanishManager(executor);
            LinkAdvisor = new MalissLinkAdvisor(executor);
            MaterialScorer = new MalissMaterialScorer(executor);
            BoardAssessor = new MalissBoardAssessor(executor);
            ThreatEvaluator = new MalissThreatEvaluator(executor);
        }

        public void ResetTurnState()
        {
            Strategy.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 1: MalissStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class MalissStrategy
    {
        private readonly _2026_MalissExecutor _exec;

        public bool SoleretUsed { get; set; }
        public bool UmbrageUsed { get; set; }
        public bool CherubiniUsed { get; set; }
        public bool DecoderUsed { get; set; }
        public bool SplashMageUsed { get; set; }
        public bool WhiteBinderUsed { get; set; }
        public bool Gwc06Used { get; set; }
        public bool GryphonUsed { get; set; }

        public MalissStrategy(_2026_MalissExecutor exec) => _exec = exec;

        public void Reset()
        {
            SoleretUsed = false;
            UmbrageUsed = false;
            CherubiniUsed = false;
            DecoderUsed = false;
            SplashMageUsed = false;
            WhiteBinderUsed = false;
            Gwc06Used = false;
            GryphonUsed = false;
        }

        public bool CanSafelyPayLP(int cost)
        {
            return _exec.Bot.LifePoints > cost + 500;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 2: MalissBanishManager
    // ═══════════════════════════════════════════════════════════════
    internal class MalissBanishManager
    {
        private readonly _2026_MalissExecutor _executor;
        public MalissBanishManager(_2026_MalissExecutor executor) => _executor = executor;

        public bool CanTriggerBanishSummon(int cost = 300)
        {
            return _executor.Bot.LifePoints > cost && _executor.Bot.GetMonsterCount() < 5;
        }

        public int CountBanishedMaliss()
        {
            return _executor.Bot.Banished.Count(c => c != null && c.HasSetcode(0x1b9));
        }

        public int CountBanishedTraps()
        {
            return _executor.Bot.Banished.Count(c => c != null && c.HasSetcode(0x1b9) && c.HasType(CardType.Trap));
        }

        public bool IsUndergroundAtkBoostActive()
        {
            return CountBanishedTraps() >= 3;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 3: MalissLinkAdvisor
    // ═══════════════════════════════════════════════════════════════
    internal class MalissLinkAdvisor
    {
        private readonly _2026_MalissExecutor _executor;
        public MalissLinkAdvisor(_2026_MalissExecutor executor) => _executor = executor;

        public bool ShouldClimbToHeartsCrypter()
        {
            return !_executor.Bot.HasInMonstersZone(_2026_MalissExecutor.CardId.HeartsCrypter) &&
                   _executor.Bot.GetMonsters().Count(m => m != null && m.IsFaceup()) >= 2;
        }

        public bool ShouldClimbToAlliedCodeTalker()
        {
            if (_executor.Bot.HasInMonstersZone(_2026_MalissExecutor.CardId.AlliedCodeTalkerIgnister))
                return false;

            var monsters = _executor.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            bool hasLink3OrHigher = monsters.Any(m => m.HasType(CardType.Link) && m.LinkCount >= 3);
            return (monsters.Count >= 3 || (hasLink3OrHigher && monsters.Count >= 2));
        }

        public bool ShouldClimbToAccesscode()
        {
            if (_executor.Bot.HasInMonstersZone(_2026_MalissExecutor.CardId.AccesscodeTalker))
                return false;

            return _executor.Duel.Turn > 1 || _executor.Enemy.GetMonsterCount() > 0;
        }

        public bool ShouldSummonKnightmareGryphon()
        {
            if (_executor.Bot.HasInMonstersZone(_2026_MalissExecutor.CardId.KnightmareGryphon))
                return false;

            bool hasGoodTrapInGY = _executor.Bot.Graveyard.Any(c => c != null && (c.Id == _2026_MalissExecutor.CardId.SolemnJudgment || c.Id == _2026_MalissExecutor.CardId.SolemnAccusation || c.HasSetcode(0x1b9)));
            return hasGoodTrapInGY && _executor.Bot.Hand.Count > 0;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 4: MalissMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class MalissMaterialScorer
    {
        private readonly _2026_MalissExecutor _executor;
        public MalissMaterialScorer(_2026_MalissExecutor executor) => _executor = executor;

        public int Score(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == _2026_MalissExecutor.CardId.LinkDecoder) return 120;
            if (card.Id == _2026_MalissExecutor.CardId.Linguriboh || card.Id == _2026_MalissExecutor.CardId.LinkSpider) return 110;
            if (card.Id == _2026_MalissExecutor.CardId.DoomedSoleret || card.Id == _2026_MalissExecutor.CardId.UmbrageVeil) return 105;
            if (card.Id == _2026_MalissExecutor.CardId.WizardIgnister || card.Id == _2026_MalissExecutor.CardId.BackupIgnister) return 95;
            if (card.Id == _2026_MalissExecutor.CardId.HeartsCrypter) return -500;
            if (card.Id == _2026_MalissExecutor.CardId.AlliedCodeTalkerIgnister) return -600;
            if (card.Id == _2026_MalissExecutor.CardId.KnightmareGryphon) return -400;
            return 10;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 5: MalissBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class MalissBoardAssessor
    {
        private readonly _2026_MalissExecutor _executor;
        public MalissBoardAssessor(_2026_MalissExecutor executor) => _executor = executor;

        public bool HasStrongBoard()
        {
            bool hasHearts = _executor.Bot.HasInMonstersZone(_2026_MalissExecutor.CardId.HeartsCrypter);
            bool hasAllied = _executor.Bot.HasInMonstersZone(_2026_MalissExecutor.CardId.AlliedCodeTalkerIgnister);
            bool hasGryphon = _executor.Bot.HasInMonstersZone(_2026_MalissExecutor.CardId.KnightmareGryphon);
            bool hasTraps = _executor.Bot.HasInSpellZone(_2026_MalissExecutor.CardId.MalissCTB11) ||
                            _executor.Bot.HasInSpellZone(_2026_MalissExecutor.CardId.MalissCMTP07) ||
                            _executor.Bot.HasInSpellZone(_2026_MalissExecutor.CardId.MalissCGWC06) ||
                            _executor.Bot.HasInSpellZone(_2026_MalissExecutor.CardId.SolemnJudgment) ||
                            _executor.Bot.HasInSpellZone(_2026_MalissExecutor.CardId.SolemnAccusation);

            return (hasHearts && (hasAllied || hasGryphon || hasTraps)) ||
                   _executor.Bot.HasInMonstersZone(_2026_MalissExecutor.CardId.AccesscodeTalker);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 6: MalissThreatEvaluator
    // ═══════════════════════════════════════════════════════════════
    internal class MalissThreatEvaluator
    {
        private readonly _2026_MalissExecutor _executor;
        public MalissThreatEvaluator(_2026_MalissExecutor executor) => _executor = executor;

        public bool OpponentHasThreatMonster()
        {
            return _executor.Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                (m.Attack >= 2500 || m.IsDisabled() == false && (m.HasType(CardType.Fusion) || m.HasType(CardType.Synchro) || m.HasType(CardType.Xyz) || m.HasType(CardType.Link))));
        }
    }
}
