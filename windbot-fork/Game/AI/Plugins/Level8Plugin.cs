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
    public class Level8Plugin : DeckPluginBase
    {
        private readonly Level8Executor _executor;

        public override string DeckName => "Level8";

        public Level8Strategy StrategyImpl { get; }
        public Level8MaterialEvaluator MaterialImpl { get; }
        public Level8ThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public Level8Plugin(Level8Executor executor)
        {
            _executor = executor;
            StrategyImpl = new Level8Strategy(executor);
            MaterialImpl = new Level8MaterialEvaluator(executor);
            ThreatImpl = new Level8ThreatEvaluator(executor);
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

            // 2. Hint 506 = HINTMSG_ATOHAND (Searchers)
            if (hint == 506)
            {
                var target = StrategyImpl.PickSearchTarget(cards, null);
                if (target != null)
                    return new List<ClientCard> { target };
            }

            // 3. Hint 501 = HINTMSG_DISCARD / Hand cost
            if (hint == 501 || cards.All(c => c.Location == CardLocation.Hand))
            {
                var discard = MaterialImpl.PickDiscardTarget(cards, min);
                if (discard != null)
                    return new List<ClientCard> { discard };
            }

            return null;
        }
    }

    public class Level8Strategy : IDeckStrategy
    {
        private readonly Level8Executor _exec;
        public Level8Strategy(Level8Executor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Monster Reborn / Scrap Golem revival targets
            var savage = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.BorreloadSavageDragon);
            if (savage != null) return savage;

            var crystal = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.CrystalWingSynchroDragon);
            if (crystal != null) return crystal;

            var scrapRecycler = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.ScrapRecycler);
            if (scrapRecycler != null) return scrapRecycler;

            var scrapBeast = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.ScrapBeast);
            if (scrapBeast != null) return scrapBeast;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard contextCard)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // ROTA targets: Photon Thrasher or Goblindbergh or Raiden
            var thrasher = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.PhotonThrasher);
            if (thrasher != null && _exec.Bot.GetMonsterCount() == 0) return thrasher;

            var goblind = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.Goblindbergh);
            if (goblind != null) return goblind;

            var raiden = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.RaidenHandofTheLightsworn);
            if (raiden != null) return raiden;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }
    }

    public class Level8MaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly Level8Executor _exec;
        public Level8MaterialEvaluator(Level8Executor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 0;

            // Never sacrifice end boss monsters
            if (c.Id == Level8Executor.CardId.CrystalWingSynchroDragon) return 99999;
            if (c.Id == Level8Executor.CardId.BorreloadSavageDragon) return 99999;
            if (c.Id == Level8Executor.CardId.Number41BagooskaTheTerriblyTiredTapir) return 90000;

            // Tokens and fodder have lowest cost
            if (c.Id == Level8Executor.CardId.MechaPhantomBeastOLionToken) return 5;
            if (c.Id == Level8Executor.CardId.PerformageTrickClown) return 10;
            if (c.Id == Level8Executor.CardId.JetSynchron) return 15;
            if (c.Id == Level8Executor.CardId.MechaPhantomBeastOLion) return 20;
            if (c.Id == Level8Executor.CardId.WorldCarrotweightChampion) return 25;

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

            // Cards that trigger or revive from GY
            var clown = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.PerformageTrickClown);
            if (clown != null) return clown;

            var olion = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.MechaPhantomBeastOLion);
            if (olion != null) return olion;

            var jet = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.JetSynchron);
            if (jet != null) return jet;

            var carrot = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.WorldCarrotweightChampion);
            if (carrot != null) return carrot;

            var golem = candidates.FirstOrDefault(c => c.Id == Level8Executor.CardId.ScrapGolem);
            if (golem != null) return golem;

            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class Level8ThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly Level8Executor _exec;
        public Level8ThreatEvaluator(Level8Executor exec) => _exec = exec;

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
