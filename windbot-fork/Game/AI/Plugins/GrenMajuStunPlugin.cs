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
    public class GrenMajuStunPlugin : DeckPluginBase
    {
        private readonly GrenMajuThunderBoarderExecutor _executor;

        public override string DeckName => "GrenMajuThunderBoarder";

        public GrenMajuStunStrategy StrategyImpl { get; }
        public GrenMajuStunMaterialEvaluator MaterialImpl { get; }
        public GrenMajuStunThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public GrenMajuStunPlugin(GrenMajuThunderBoarderExecutor executor)
        {
            _executor = executor;
            StrategyImpl = new GrenMajuStunStrategy(executor);
            MaterialImpl = new GrenMajuStunMaterialEvaluator(executor);
            ThreatImpl = new GrenMajuStunThreatEvaluator(executor);
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

            // 2. Hint 506 = HINTMSG_ATOHAND (Pot of Duality)
            if (hint == 506)
            {
                var target = StrategyImpl.PickSearchTarget(cards, null);
                if (target != null)
                    return new List<ClientCard> { target };
            }

            // 3. Hint 500 = HINTMSG_SPSUMMON (Waking the Dragon)
            if (hint == 500)
            {
                var target = StrategyImpl.PickSpecialSummonTarget(cards);
                if (target != null)
                    return new List<ClientCard> { target };
            }

            // 4. Eater of Millions banish cost from Extra Deck
            if (cards.All(c => c.Location == CardLocation.Extra))
            {
                var banished = cards
                    .OrderBy(c => MaterialImpl.GetMaterialCost(c))
                    .Take(max)
                    .ToList();
                if (banished.Count >= min)
                    return banished;
            }

            // 5. Card of Demise / Discard hand cost
            if (hint == 501 || cards.All(c => c.Location == CardLocation.Hand))
            {
                var discard = MaterialImpl.PickDiscardTarget(cards, min);
                if (discard != null)
                    return new List<ClientCard> { discard };
            }

            return null;
        }
    }

    public class GrenMajuStunStrategy : IDeckStrategy
    {
        private readonly GrenMajuThunderBoarderExecutor _exec;
        public GrenMajuStunStrategy(GrenMajuThunderBoarderExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Waking the Dragon summon target: Raidraptor - Ultimate Falcon (unaffected boss)
            var falcon = candidates.FirstOrDefault(c => c.Id == GrenMajuThunderBoarderExecutor.CardId.RaidraptorUltimateFalcon);
            if (falcon != null) return falcon;

            var borrelsword = candidates.FirstOrDefault(c => c.Id == GrenMajuThunderBoarderExecutor.CardId.BirrelswordDragon);
            if (borrelsword != null) return borrelsword;

            var borreload = candidates.FirstOrDefault(c => c.Id == GrenMajuThunderBoarderExecutor.CardId.BorreloadDragon);
            if (borreload != null) return borreload;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard contextCard)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Pot of Duality selection priority:
            // 1. Inspect Boarder (turn 1 stun king)
            if (_exec.Bot.GetMonsterCount() == 0)
            {
                var boarder = candidates.FirstOrDefault(c => c.Id == GrenMajuThunderBoarderExecutor.CardId.InspectBoarder);
                if (boarder != null) return boarder;
            }

            // 2. Macro Cosmos (banish floodgate)
            var macro = candidates.FirstOrDefault(c => c.Id == GrenMajuThunderBoarderExecutor.CardId.MacroCosmos);
            if (macro != null && !_exec.Bot.HasInSpellZone(GrenMajuThunderBoarderExecutor.CardId.MacroCosmos)) return macro;

            // 3. Pot of Desires (card draw + banish 10 cards to boost Gren Maju)
            var desires = candidates.FirstOrDefault(c => c.Id == GrenMajuThunderBoarderExecutor.CardId.PotOfDesires);
            if (desires != null) return desires;

            // 4. Solemn Judgment / Strike
            var solemn = candidates.FirstOrDefault(c => c.Id == GrenMajuThunderBoarderExecutor.CardId.SolemnJudgment || c.Id == GrenMajuThunderBoarderExecutor.CardId.SolemStrike);
            if (solemn != null) return solemn;

            // 5. Crackdown / Anti-Spell
            var crackdown = candidates.FirstOrDefault(c => c.Id == GrenMajuThunderBoarderExecutor.CardId.Crackdown || c.Id == GrenMajuThunderBoarderExecutor.CardId.AntiSpellFragrance);
            if (crackdown != null) return crackdown;

            // 6. Gren Maju (if already have banished cards)
            if (_exec.Bot.Banished.Count >= 10)
            {
                var gren = candidates.FirstOrDefault(c => c.Id == GrenMajuThunderBoarderExecutor.CardId.GrenMajuDaEizo);
                if (gren != null) return gren;
            }

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }
    }

    public class GrenMajuStunMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly GrenMajuThunderBoarderExecutor _exec;
        public GrenMajuStunMaterialEvaluator(GrenMajuThunderBoarderExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 0;

            // Never banish Ultimate Falcon or Borrelsword for Eater of Millions!
            if (c.Id == GrenMajuThunderBoarderExecutor.CardId.RaidraptorUltimateFalcon) return 99999;
            if (c.Id == GrenMajuThunderBoarderExecutor.CardId.BirrelswordDragon) return 90000;
            if (c.Id == GrenMajuThunderBoarderExecutor.CardId.BorreloadDragon) return 85000;

            // Fodder to banish from Extra Deck
            if (c.Id == GrenMajuThunderBoarderExecutor.CardId.MissusRadiant) return 10;
            if (c.Id == GrenMajuThunderBoarderExecutor.CardId.BrandishMaidenKagari) return 10;
            if (c.Id == GrenMajuThunderBoarderExecutor.CardId.LinkSpider) return 10;
            if (c.Id == GrenMajuThunderBoarderExecutor.CardId.HeavymetalfoesElectrumite) return 15;
            if (c.Id == GrenMajuThunderBoarderExecutor.CardId.CrystronNeedlefiber) return 15;
            if (c.Id == GrenMajuThunderBoarderExecutor.CardId.TopologicTrisbaena) return 20;
            if (c.Id == GrenMajuThunderBoarderExecutor.CardId.NingirsuTheWorldChaliceWarrior) return 20;

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

            // For Card of Demise / Knightmare discard: discard cards we have duplicates of
            var duplicate = candidates.GroupBy(c => c.Id).Where(g => g.Count() > 1).Select(g => g.First()).FirstOrDefault();
            if (duplicate != null) return duplicate;

            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class GrenMajuStunThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly GrenMajuThunderBoarderExecutor _exec;
        public GrenMajuStunThreatEvaluator(GrenMajuThunderBoarderExecutor exec) => _exec = exec;

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
