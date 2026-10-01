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
    public class PureWindsPlugin : DeckPluginBase
    {
        private readonly PureWindsExecutor _executor;

        public override string DeckName => "PureWinds";

        public PureWindsStrategy StrategyImpl { get; }
        public PureWindsMaterialEvaluator MaterialImpl { get; }
        public PureWindsThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public PureWindsPlugin(PureWindsExecutor executor)
        {
            _executor = executor;
            StrategyImpl = new PureWindsStrategy(executor);
            MaterialImpl = new PureWindsMaterialEvaluator(executor);
            ThreatImpl = new PureWindsThreatEvaluator(executor);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }

        public IList<ClientCard> SelectCardLogic(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return null;

            // 1. Hint 502 = HINTMSG_DESTROY, Hint 503 = HINTMSG_REMOVE (Target enemy cards first)
            if (hint == 502 || hint == 503 || hint == 504)
            {
                var enemyCards = cards.Where(c => c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var highThreat = enemyCards.OrderByDescending(c => ThreatImpl.EvaluateThreatScore(c)).Take(max).ToList();
                    if (highThreat.Count >= min)
                        return highThreat;
                }
            }

            // 2. Hint 506 = HINTMSG_ATOHAND (Searchers: Glass Bell / Terrortop / Sphreez)
            if (hint == 506)
            {
                var target = StrategyImpl.PickSearchTarget(cards, null);
                if (target != null)
                    return new List<ClientCard> { target };
            }

            // 3. Hint 500 = HINTMSG_SPSUMMON (Ice Bell / Pilica / Taketomborg)
            if (hint == 500)
            {
                var target = StrategyImpl.PickSpecialSummonTarget(cards);
                if (target != null)
                    return new List<ClientCard> { target };
            }

            return null;
        }
    }

    public class PureWindsStrategy : IDeckStrategy
    {
        private readonly PureWindsExecutor _exec;
        public PureWindsStrategy(PureWindsExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Ice Bell Special Summon from Deck -> Glass Bell
            var glass = candidates.FirstOrDefault(c => c.Id == PureWindsExecutor.CardId.WindwitchGlassBell);
            if (glass != null) return glass;

            // Taketomborg Special Summon from Deck -> Red-Eyed Dice
            var dice = candidates.FirstOrDefault(c => c.Id == PureWindsExecutor.CardId.SpeedroidRedEyedDice);
            if (dice != null) return dice;

            // Pilica GY revival targets
            var gulldo = candidates.FirstOrDefault(c => c.Id == PureWindsExecutor.CardId.GustoGulldo);
            if (gulldo != null) return gulldo;

            var egul = candidates.FirstOrDefault(c => c.Id == PureWindsExecutor.CardId.GustoEgul);
            if (egul != null) return egul;

            // Monster Reborn revival
            var crystal = candidates.FirstOrDefault(c => c.Id == PureWindsExecutor.CardId.CrystalWingSynchroDragon);
            if (crystal != null) return crystal;

            var clear = candidates.FirstOrDefault(c => c.Id == PureWindsExecutor.CardId.ClearWingSynchroDragon);
            if (clear != null) return clear;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard contextCard)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Glass Bell searches Snow Bell
            var snow = candidates.FirstOrDefault(c => c.Id == PureWindsExecutor.CardId.WindwitchSnowBell);
            if (snow != null) return snow;

            // Terrortop searches Taketomborg
            var take = candidates.FirstOrDefault(c => c.Id == PureWindsExecutor.CardId.SpeedroidTaketomborg);
            if (take != null) return take;

            // Ice Bell if not in hand
            var ice = candidates.FirstOrDefault(c => c.Id == PureWindsExecutor.CardId.WindwitchIceBell);
            if (ice != null && !_exec.Bot.HasInHand(PureWindsExecutor.CardId.WindwitchIceBell)) return ice;

            // Daigusto Sphreez GY recovery: Gusto monster back to hand
            var pilica = candidates.FirstOrDefault(c => c.Id == PureWindsExecutor.CardId.PilicaDescendantOfGusto);
            if (pilica != null) return pilica;

            var winda = candidates.FirstOrDefault(c => c.Id == PureWindsExecutor.CardId.WindaPriestessOfGusto);
            if (winda != null) return winda;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }
    }

    public class PureWindsMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly PureWindsExecutor _exec;
        public PureWindsMaterialEvaluator(PureWindsExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 0;

            // Keep Synchro Bosses
            if (c.Id == PureWindsExecutor.CardId.CrystalWingSynchroDragon) return 99999;
            if (c.Id == PureWindsExecutor.CardId.ClearWingSynchroDragon) return 90000;
            if (c.Id == PureWindsExecutor.CardId.DaigustoSphreez) return 85000;

            // Stepping stones
            if (c.Id == PureWindsExecutor.CardId.WindwitchWinterBell) return 100;
            if (c.Id == PureWindsExecutor.CardId.StardustChargeWarrior) return 100;

            // Main Deck materials
            if (c.Id == PureWindsExecutor.CardId.WindwitchSnowBell) return 10;
            if (c.Id == PureWindsExecutor.CardId.SpeedroidRedEyedDice) return 10;
            if (c.Id == PureWindsExecutor.CardId.WindwitchGlassBell) return 20;
            if (c.Id == PureWindsExecutor.CardId.WindwitchIceBell) return 20;
            if (c.Id == PureWindsExecutor.CardId.GustoGulldo) return 15;
            if (c.Id == PureWindsExecutor.CardId.GustoEgul) return 15;

            return 50;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class PureWindsThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly PureWindsExecutor _exec;
        public PureWindsThreatEvaluator(PureWindsExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;
            if (card.IsFaceup())
            {
                if (card.HasType(CardType.Fusion) || card.HasType(CardType.Synchro) || card.HasType(CardType.Xyz) || card.HasType(CardType.Link)) score += 50;
                if (card.Attack >= 2500) score += 40;
                else if (card.Attack >= 2000) score += 20;
                if (card.IsMonster() && card.HasType(CardType.Effect)) score += 30;
                if (card.HasType(CardType.Spell) || card.HasType(CardType.Trap)) score += 25;
            }
            else
            {
                score += 15;
            }
            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return card.Attack >= 3000 || (card.IsFaceup() && (card.HasType(CardType.Fusion) || card.HasType(CardType.Synchro) || card.HasType(CardType.Xyz) || card.HasType(CardType.Link)));
        }
    }
}
