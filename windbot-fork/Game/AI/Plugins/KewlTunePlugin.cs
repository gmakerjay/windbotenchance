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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: KewlTunePlugin
    // ═══════════════════════════════════════════════════════════════
    public class KewlTunePlugin : DeckPluginBase
    {
        private readonly _2026_KwtuneExecutor _exec;

        public override string DeckName => "Kwtune";

        public KewlTuneStrategy StrategyImpl { get; }
        public KewlTuneMaterialEvaluator MaterialImpl { get; }
        public KewlTuneThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public KewlTunePlugin(_2026_KwtuneExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new KewlTuneStrategy(exec);
            MaterialImpl = new KewlTuneMaterialEvaluator(exec);
            ThreatImpl = new KewlTuneThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
        }
    }

    public class KewlTuneStrategy : IDeckStrategy
    {
        private readonly _2026_KwtuneExecutor _exec;
        public KewlTuneStrategy(_2026_KwtuneExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;
            int[] priorities = {
                _2026_KwtuneExecutor.CardId.KewlTuneRotary,
                _2026_KwtuneExecutor.CardId.KewlTuneMix,
                _2026_KwtuneExecutor.CardId.KewlTuneReco,
                _2026_KwtuneExecutor.CardId.KewlTuneCue,
                _2026_KwtuneExecutor.CardId.GoldenCloudBeastMalong,
                _2026_KwtuneExecutor.CardId.KewlTuneCrackle
            };
            foreach (int id in priorities)
            {
                var card = candidates.FirstOrDefault(c => c != null && c.Id == id);
                if (card != null) return card;
            }
            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;
            if (context != null && context.Id == _2026_KwtuneExecutor.CardId.VisasAmritara)
            {
                var reframing = candidates.FirstOrDefault(c => c.Id == _2026_KwtuneExecutor.CardId.MannadiumReframing);
                if (reframing != null) return reframing;
            }
            if (context != null && context.Id == _2026_KwtuneExecutor.CardId.DuelistGenesis)
            {
                var synchro = candidates.FirstOrDefault(c => c.Id == _2026_KwtuneExecutor.CardId.KewlTuneSynchro);
                if (synchro != null) return synchro;
                var emg = candidates.FirstOrDefault(c => c.Id == _2026_KwtuneExecutor.CardId.SynchroEmergency);
                if (emg != null) return emg;
            }
            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class KewlTuneMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly _2026_KwtuneExecutor _exec;
        public KewlTuneMaterialEvaluator(_2026_KwtuneExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;
            if (_exec.IsAceCard(card))
            {
                if (card.Location == CardLocation.MonsterZone && card.IsFaceup())
                    return 10000;
                return 900;
            }
            if (card.IsCode(_2026_KwtuneExecutor.CardId.AshBlossom, _2026_KwtuneExecutor.CardId.AshBlossomAlt,
                _2026_KwtuneExecutor.CardId.GhostBelle, _2026_KwtuneExecutor.CardId.GhostOgre,
                _2026_KwtuneExecutor.CardId.EffectVeiler, _2026_KwtuneExecutor.CardId.DrollAndLockBird))
                return 950;

            switch (card.Id)
            {
                case _2026_KwtuneExecutor.CardId.KewlTuneClip: return 10;
                case _2026_KwtuneExecutor.CardId.KewlTuneRotary: return 15;
                case _2026_KwtuneExecutor.CardId.KewlTuneMix: return 20;
                case _2026_KwtuneExecutor.CardId.KewlTuneReco: return 25;
                case _2026_KwtuneExecutor.CardId.JetSynchron: return 28;
                case _2026_KwtuneExecutor.CardId.KewlTuneCue: return 30;
                default: return 100;
            }
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.Where(c => c != null).OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            var jet = candidates.FirstOrDefault(c => c != null && c.Id == _2026_KwtuneExecutor.CardId.JetSynchron);
            if (jet != null) return jet;

            var gyTuner = candidates.Where(c => c != null && (c.Id == _2026_KwtuneExecutor.CardId.KewlTuneClip || c.Id == _2026_KwtuneExecutor.CardId.KewlTuneRotary || c.Id == _2026_KwtuneExecutor.CardId.KewlTuneMix || c.Id == _2026_KwtuneExecutor.CardId.KewlTuneReco))
                .OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
            if (gyTuner != null) return gyTuner;

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class KewlTuneThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly _2026_KwtuneExecutor _exec;
        public KewlTuneThreatEvaluator(_2026_KwtuneExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;
            int score = 0;
            if (CardIntelligence.IsFloodgate(card.Id)) score += 8000;
            if (CardIntelligence.IsKnownNegator(card.Id)) score += 6000;
            if (card.Attack >= 2500) score += 4000;
            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            return CardIntelligence.IsFloodgate(card.Id) || (card.Attack >= 3000);
        }
    }
}
