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
    public class Rank5Plugin : DeckPluginBase
    {
        private readonly Rank5Executor _executor;

        public override string DeckName => "Rank5";

        public Rank5Strategy StrategyImpl { get; }
        public Rank5MaterialEvaluator MaterialImpl { get; }
        public Rank5ThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public Rank5Plugin(Rank5Executor executor)
        {
            _executor = executor;
            StrategyImpl = new Rank5Strategy(executor);
            MaterialImpl = new Rank5MaterialEvaluator(executor);
            ThreatImpl = new Rank5ThreatEvaluator(executor);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }

        public IList<ClientCard> SelectCardLogic(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return null;

            // 1. Xyz Materials selection (Hint 513 = HINTMSG_XMATERIAL)
            if (hint == 513)
            {
                var preferred = cards
                    .OrderBy(c => MaterialImpl.GetMaterialCost(c))
                    .Take(max)
                    .ToList();
                if (preferred.Count >= min)
                    return preferred;
            }

            // 2. Removal / Destruction (Hint 502 = HINTMSG_DESTROY, Hint 503 = HINTMSG_REMOVE)
            // Rule 1 & 15: Always target opponent's cards first!
            if (hint == 502 || hint == 503 || hint == 504)
            {
                var enemyCards = cards.Where(c => c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var dangerous = enemyCards.OrderByDescending(c => ThreatImpl.EvaluateThreatScore(c)).Take(max).ToList();
                    if (dangerous.Count >= min)
                        return dangerous;
                }
            }

            // 3. Cyber Dragon Infinity absorption target (Hint 505 = HINTMSG_ATTACH / target)
            if (hint == 505)
            {
                var enemyMonsters = cards.Where(c => c.Controller == 1 && c.IsFaceup()).ToList();
                if (enemyMonsters.Count > 0)
                {
                    var best = enemyMonsters.OrderByDescending(c => c.Attack).Take(max).ToList();
                    if (best.Count >= min)
                        return best;
                }
            }

            // 4. Quickdraw Synchron discard fodder
            if (cards.All(c => c.Location == CardLocation.Hand))
            {
                var discardTarget = cards
                    .OrderBy(c => c.Id == Rank5Executor.CardId.ZWEagleClaw ? 0 :
                                  c.Id == Rank5Executor.CardId.SolarWindJammer && _executor.Bot.GetMonsterCount() > 0 ? 1 :
                                  c.Id == Rank5Executor.CardId.MistArchfiend ? 2 :
                                  c.Id == Rank5Executor.CardId.CyberDragon ? 3 : 10)
                    .Take(max)
                    .ToList();
                if (discardTarget.Count >= min)
                    return discardTarget;
            }

            return null;
        }
    }

    public class Rank5Strategy : IDeckStrategy
    {
        private readonly Rank5Executor _exec;
        public Rank5Strategy(Rank5Executor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Xyz Reborn targets: Cyber Dragon Infinity > Nova > Volcasaurus > Tiras
            var infinity = candidates.FirstOrDefault(c => c.Id == Rank5Executor.CardId.CyberDragonInfinity);
            if (infinity != null) return infinity;

            var nova = candidates.FirstOrDefault(c => c.Id == Rank5Executor.CardId.CyberDragonNova);
            if (nova != null) return nova;

            var volca = candidates.FirstOrDefault(c => c.Id == Rank5Executor.CardId.Number61Volcasaurus);
            if (volca != null) return volca;

            var tiras = candidates.FirstOrDefault(c => c.Id == Rank5Executor.CardId.TirasKeeperOfGenesis);
            if (tiras != null) return tiras;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard contextCard)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }
    }

    public class Rank5MaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly Rank5Executor _exec;
        public Rank5MaterialEvaluator(Rank5Executor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 0;

            // Star Drawing gives +1 card draw when detached from Xyz monster! Best material
            if (c.Id == Rank5Executor.CardId.StarDrawing) return 1;

            // Panzer Dragon (summoned by Instant Fusion, dies at end phase)
            if (c.Id == Rank5Executor.CardId.PanzerDragon) return 2;

            // Mist Archfiend (summoned without tribute, dies at end phase)
            if (c.Id == Rank5Executor.CardId.MistArchfiend) return 3;

            // Solar Wind Jammer (normal level 5 wall)
            if (c.Id == Rank5Executor.CardId.SolarWindJammer) return 4;

            // Wind-Up Soldier / Golden Jet
            if (c.Id == Rank5Executor.CardId.WindUpSoldier || c.Id == Rank5Executor.CardId.ChronomalyGoldenJet) return 5;

            // Cyber Dragon
            if (c.Id == Rank5Executor.CardId.CyberDragon) return 6;

            // Cyber Dragon Nova is meant to rank up into Infinity
            if (c.Id == Rank5Executor.CardId.CyberDragonNova) return 1;

            // NEVER detach from Cyber Dragon Infinity or use Infinity as fodder!
            if (c.Id == Rank5Executor.CardId.CyberDragonInfinity) return 99999;

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

    public class Rank5ThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly Rank5Executor _exec;
        public Rank5ThreatEvaluator(Rank5Executor exec) => _exec = exec;

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
