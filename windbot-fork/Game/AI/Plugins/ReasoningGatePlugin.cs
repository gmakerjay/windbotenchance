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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: ReasoningGatePlugin
    //  Reasoning & Monster Gate Excavation Engine with Dimension OTK
    // ═══════════════════════════════════════════════════════════════
    public class ReasoningGatePlugin : DeckPluginBase
    {
        private readonly ReasoningGateExecutor _exec;

        public override string DeckName => "ReasoningGate";

        public ReasoningGateStrategy StrategyImpl { get; }
        public ReasoningGateMaterialEvaluator MaterialImpl { get; }
        public ReasoningGateThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public ReasoningGatePlugin(ReasoningGateExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new ReasoningGateStrategy(exec);
            MaterialImpl = new ReasoningGateMaterialEvaluator(exec);
            ThreatImpl = new ReasoningGateThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class ReasoningGateStrategy : IDeckStrategy
    {
        private readonly ReasoningGateExecutor _exec;

        public ReasoningGateStrategy(ReasoningGateExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Dark Magician of Chaos Spell Retrieval
            if (context != null && context.Id == ReasoningGateExecutor.CardId.DarkMagicianOfChaos)
            {
                // If we have powerful banished monsters and LP > 2000, grab Dimension Fusion!
                int banishedMonsters = _exec.Bot.Banished.Count(c => c != null && c.IsMonster());
                if (banishedMonsters >= 2 && _exec.Bot.LifePoints > 2000)
                {
                    var dimFusion = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.DimensionFusion);
                    if (dimFusion != null) return dimFusion;
                }

                // If opponent has backrow and we don't have removal, grab Trunade / Heavy Storm
                if (_exec.Enemy.GetSpellCount() >= 2)
                {
                    var storm = candidates.FirstOrDefault(c => c != null && (c.Id == ReasoningGateExecutor.CardId.HeavyStorm || c.Id == ReasoningGateExecutor.CardId.GiantTrunade));
                    if (storm != null) return storm;
                }

                // Pot of Greed / Graceful Charity
                var pot = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.PotOfGreed);
                if (pot != null) return pot;

                var charity = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.GracefulCharity);
                if (charity != null) return charity;

                // Monster Gate / Reasoning to continue excavation engine
                var gate = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.MonsterGate);
                if (gate != null) return gate;

                var reasoning = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.Reasoning);
                if (reasoning != null) return reasoning;

                // Snatch Steal
                var snatch = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.SnatchSteal);
                if (snatch != null) return snatch;
            }

            // 2. Sangan search
            if (context != null && context.Id == ReasoningGateExecutor.CardId.Sangan)
            {
                var sinister = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.SinisterSerpent);
                if (sinister != null && !_exec.Bot.HasInHand(ReasoningGateExecutor.CardId.SinisterSerpent))
                    return sinister;

                var ddwl = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.DdWarriorLady);
                if (ddwl != null) return ddwl;

                var faith = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.MagicianOfFaith);
                if (faith != null) return faith;
            }

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Dimension Fusion / Premature / Call of the Haunted:
            var dmoc = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.DarkMagicianOfChaos);
            if (dmoc != null) return dmoc;

            var jinzo = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.Jinzo);
            if (jinzo != null) return jinzo;

            var bls = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning);
            if (bls != null) return bls;

            var crane = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.SacredCrane);
            if (crane != null) return crane;

            var airknight = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.AirknightParshath);
            if (airknight != null) return airknight;

            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class ReasoningGateMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly ReasoningGateExecutor _exec;
        public ReasoningGateMaterialEvaluator(ReasoningGateExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Protect our powerhouse Bosses from being sacrificed to Monster Gate!
            if (card.Id == ReasoningGateExecutor.CardId.DarkMagicianOfChaos) return 60000;
            if (card.Id == ReasoningGateExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning) return 50000;
            if (card.Id == ReasoningGateExecutor.CardId.Jinzo) return 40000;
            if (card.Id == ReasoningGateExecutor.CardId.AirknightParshath) return 20000;

            // Ideal Monster Gate tributes:
            if (card.Id == ReasoningGateExecutor.CardId.SheepToken) return 5;
            if (card.Id == ReasoningGateExecutor.CardId.SinisterSerpent) return 10;
            if (card.Id == ReasoningGateExecutor.CardId.MagicianOfFaith) return 20;
            if (card.Id == ReasoningGateExecutor.CardId.Sangan) return 30; // Floats upon tribute!
            if (card.Id == ReasoningGateExecutor.CardId.SacredCrane && card.IsFaceup()) return 50; // Already drew 1 card

            return 100;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.Where(c => c != null).OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Graceful Charity / Spell Reproduction discard:
            // 1. Sinister Serpent
            var sinister = candidates.FirstOrDefault(c => c != null && c.Id == ReasoningGateExecutor.CardId.SinisterSerpent);
            if (sinister != null) return sinister;

            // 2. Extra Spells for Spell Reproduction
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault(c => c != null);
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault(c => c != null);
        }
    }

    public class ReasoningGateThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly ReasoningGateExecutor _exec;
        public ReasoningGateThreatEvaluator(ReasoningGateExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;

            if (card.Id == ReasoningGateExecutor.CardId.Jinzo) score += 600;
            if (card.Id == ReasoningGateExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning) score += 800;
            if (card.Attack >= 2400) score += 400;

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == ReasoningGateExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning;
        }
    }
}
