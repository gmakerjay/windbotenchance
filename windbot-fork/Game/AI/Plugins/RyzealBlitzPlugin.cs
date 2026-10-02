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
    //  MASTER DECK PLUGIN: RyzealBlitzPlugin
    // ═══════════════════════════════════════════════════════════════
    internal class RyzealBlitzPlugin
    {
        public RyzealBlitzExecutor Executor { get; }
        public RyzealBlitzStrategy Strategy { get; }
        public RyzealBlitzThreatEvaluator ThreatEvaluator { get; }
        public RyzealBlitzMaterialScorer MaterialScorer { get; }
        public RyzealBlitzBoardAssessor BoardAssessor { get; }

        public RyzealBlitzPlugin(RyzealBlitzExecutor executor)
        {
            Executor = executor;
            Strategy = new RyzealBlitzStrategy(executor);
            ThreatEvaluator = new RyzealBlitzThreatEvaluator(executor);
            MaterialScorer = new RyzealBlitzMaterialScorer(executor);
            BoardAssessor = new RyzealBlitzBoardAssessor(executor);
        }

        public void ResetTurnState()
        {
            Strategy.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 1: RyzealBlitzStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class RyzealBlitzStrategy
    {
        private readonly RyzealBlitzExecutor _exec;

        public bool IceRyzealSummonedThisTurn { get; set; }
        public bool SwordRyzealSummonedThisTurn { get; set; }
        public bool ExtRyzealSummonedThisTurn { get; set; }
        public bool CoulombActivatedThisTurn { get; set; }
        public bool SteppleaderUsedThisTurn { get; set; }
        public bool SurgePopUsedThisTurn { get; set; }
        public bool GrainPopUsedThisTurn { get; set; }
        public bool CrackleUsedThisTurn { get; set; }
        public bool WhiskerNegateUsedThisTurn { get; set; }
        public bool BreakawayUsedThisTurn { get; set; }
        public bool DetonatorPopUsedThisTurn { get; set; }
        public bool ReturnStrokeNegateUsedThisTurn { get; set; }
        public bool SlayerUsedThisTurn { get; set; }
        public bool DugaresUsedThisTurn { get; set; }
        public bool GiantHandUsedThisTurn { get; set; }

        public RyzealBlitzStrategy(RyzealBlitzExecutor exec) => _exec = exec;

        public void Reset()
        {
            IceRyzealSummonedThisTurn = false;
            SwordRyzealSummonedThisTurn = false;
            ExtRyzealSummonedThisTurn = false;
            CoulombActivatedThisTurn = false;
            SteppleaderUsedThisTurn = false;
            SurgePopUsedThisTurn = false;
            GrainPopUsedThisTurn = false;
            CrackleUsedThisTurn = false;
            WhiskerNegateUsedThisTurn = false;
            BreakawayUsedThisTurn = false;
            DetonatorPopUsedThisTurn = false;
            ReturnStrokeNegateUsedThisTurn = false;
            SlayerUsedThisTurn = false;
            DugaresUsedThisTurn = false;
            GiantHandUsedThisTurn = false;
        }

        public bool HasThunderOnField()
        {
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.Race & (int)CardRace.Thunder) != 0);
        }

        public bool HasRyzealOnFieldOrGY()
        {
            return _exec.Bot.GetMonsters().Any(m => m != null && IsRyzeal(m)) ||
                   _exec.Bot.Graveyard.Any(c => c != null && IsRyzeal(c));
        }

        public static bool IsRyzeal(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == RyzealBlitzExecutor.CardId.IceRyzeal ||
                   card.Id == RyzealBlitzExecutor.CardId.ExtRyzeal ||
                   card.Id == RyzealBlitzExecutor.CardId.SwordRyzeal ||
                   card.Id == RyzealBlitzExecutor.CardId.RyzealDetonator;
        }

        public static bool IsBlitzclique(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == RyzealBlitzExecutor.CardId.SurgeBlitzclique ||
                   card.Id == RyzealBlitzExecutor.CardId.GrainBlitzclique ||
                   card.Id == RyzealBlitzExecutor.CardId.CrackleBlitzclique ||
                   card.Id == RyzealBlitzExecutor.CardId.WhiskerBlitzclique ||
                   card.Id == RyzealBlitzExecutor.CardId.EmiBlitzclique ||
                   card.Id == RyzealBlitzExecutor.CardId.BlitzcliqueSteppleader ||
                   card.Id == RyzealBlitzExecutor.CardId.BlitzcliqueBreakaway ||
                   card.Id == RyzealBlitzExecutor.CardId.BlitzcliqueReturnStroke ||
                   card.Id == RyzealBlitzExecutor.CardId.BlitzcliqueAlternator;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 2: RyzealBlitzThreatEvaluator
    // ═══════════════════════════════════════════════════════════════
    internal class RyzealBlitzThreatEvaluator
    {
        private readonly RyzealBlitzExecutor _exec;
        public RyzealBlitzThreatEvaluator(RyzealBlitzExecutor exec) => _exec = exec;

        public ClientCard GetBestDestructionTarget()
        {
            var enemyCards = _exec.Enemy.GetMonsters().Concat(_exec.Enemy.GetSpells()).ToList();
            if (!enemyCards.Any()) return null;

            // Prioritize dangerous enemy cards
            return enemyCards
                .OrderByDescending(c => EvaluateTargetPriority(c))
                .FirstOrDefault();
        }

        public ClientCard GetBestMonsterDestructionTarget()
        {
            var enemyMonsters = _exec.Enemy.GetMonsters().Where(m => m != null && m.IsFaceup());
            if (!enemyMonsters.Any()) return null;

            return enemyMonsters
                .OrderByDescending(c => EvaluateTargetPriority(c))
                .FirstOrDefault();
        }

        public ClientCard GetBestSpellTrapDestructionTarget()
        {
            var enemySpells = _exec.Enemy.GetSpells();
            if (!enemySpells.Any()) return null;

            return enemySpells
                .OrderByDescending(c => c.IsFaceup() ? 100 : 50)
                .FirstOrDefault();
        }

        public int EvaluateTargetPriority(ClientCard card)
        {
            if (card == null) return 0;
            int score = 10;

            if (CardIntelligence.IsFloodgate(card.Id)) score += 500;
            if (CardIntelligence.IsKnownNegator(card.Id)) score += 300;
            if (CardIntelligence.IsHighThreatChokepoint(card.Id)) score += 200;

            if (card.HasType(CardType.Monster))
            {
                score += card.Attack / 50;
                if (card.HasType(CardType.Fusion) || card.HasType(CardType.Synchro) || card.HasType(CardType.Xyz) || card.HasType(CardType.Link)) score += 80;
            }
            else
            {
                if (card.HasType(CardType.Continuous) || card.HasType(CardType.Field)) score += 120;
            }

            return score;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 3: RyzealBlitzMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class RyzealBlitzMaterialScorer
    {
        private readonly RyzealBlitzExecutor _exec;
        public RyzealBlitzMaterialScorer(RyzealBlitzExecutor exec) => _exec = exec;

        public int GetDetachPriority(ClientCard card)
        {
            if (card == null) return 0;
            // Detach spells or non-essential monsters first
            if (card.HasType(CardType.Spell) || card.HasType(CardType.Trap)) return 100;
            if (card.Id == RyzealBlitzExecutor.CardId.IceRyzeal) return 80;
            if (card.Id == RyzealBlitzExecutor.CardId.SwordRyzeal) return 70;
            if (card.Id == RyzealBlitzExecutor.CardId.ExtRyzeal) return 60;
            return 50;
        }

        public int GetHandDiscardPriority(ClientCard card)
        {
            if (card == null) return 1000;
            // Cards that like being in GY
            if (card.Id == RyzealBlitzExecutor.CardId.MereologicAggregator) return 10;
            if (card.Id == RyzealBlitzExecutor.CardId.Garura) return 15;
            if (card.Id == RyzealBlitzExecutor.CardId.BlitzcliqueBreakaway) return 20;
            if (card.Id == RyzealBlitzExecutor.CardId.Coulomb) return 25;
            if (card.Id == RyzealBlitzExecutor.CardId.BlitzcliqueAlternator) return 30;
            if (card.Id == RyzealBlitzExecutor.CardId.BlitzcliqueReturnStroke) return 35;
            return 100;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 4: RyzealBlitzBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class RyzealBlitzBoardAssessor
    {
        private readonly RyzealBlitzExecutor _exec;
        public RyzealBlitzBoardAssessor(RyzealBlitzExecutor exec) => _exec = exec;

        public int CountAvailableLevel4Monsters()
        {
            return _exec.Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.Level == 4);
        }

        public bool CanXyzSummonRank4()
        {
            return CountAvailableLevel4Monsters() >= 2;
        }

        public bool HasDetonatorOnField()
        {
            return _exec.Bot.HasInMonstersZone(RyzealBlitzExecutor.CardId.RyzealDetonator);
        }

        public bool HasGiantHandOnField()
        {
            return _exec.Bot.HasInMonstersZone(RyzealBlitzExecutor.CardId.Number106GiantHand);
        }

        public int TotalBotATK()
        {
            return _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
        }

        public bool CanOTK()
        {
            return TotalBotATK() >= _exec.Enemy.LifePoints &&
                   (!_exec.Enemy.GetMonsters().Any(m => m != null && m.IsFaceup()) || TotalBotATK() >= _exec.Enemy.LifePoints + 3000);
        }
    }
}
