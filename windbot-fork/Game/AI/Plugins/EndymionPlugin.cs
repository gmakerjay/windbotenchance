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
    //  MASTER DECK PLUGIN: EndymionPlugin (Layer 3 Domain Helpers)
    //  Decouples Domain Rules, Strategy, and Scorer from Engine Core
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionPlugin : IDeckPlugin
    {
        private readonly _2026_EndymionExecutor _exec;

        public string DeckName => "Endymion";

        public EndymionStrategy Strategy { get; }
        public EndymionCounterEconomy CounterEconomy { get; }
        public EndymionScaleResolver ScaleResolver { get; }
        public EndymionMaterialScorer MaterialScorer { get; }
        public EndymionActionScorer ActionScorer { get; }
        public EndymionThreatEvaluator ThreatEvaluator { get; }
        public EndymionBoardAssessor BoardAssessor { get; }

        IDeckStrategy IDeckPlugin.Strategy => Strategy;
        IDeckResourceEvaluator IDeckPlugin.ResourceEvaluator => CounterEconomy;
        IDeckMaterialEvaluator IDeckPlugin.MaterialEvaluator => MaterialScorer;
        IDeckActionScorer IDeckPlugin.ActionScorer => ActionScorer;
        IDeckThreatEvaluator IDeckPlugin.ThreatEvaluator => ThreatEvaluator;
        IDeckScaleResolver IDeckPlugin.ScaleResolver => ScaleResolver;

        public EndymionPlugin(_2026_EndymionExecutor exec)
        {
            _exec = exec;
            Strategy = new EndymionStrategy(exec);
            CounterEconomy = new EndymionCounterEconomy(exec);
            ScaleResolver = new EndymionScaleResolver(exec);
            MaterialScorer = new EndymionMaterialScorer(exec);
            ActionScorer = new EndymionActionScorer(exec, this);
            ThreatEvaluator = new EndymionThreatEvaluator(exec);
            BoardAssessor = new EndymionBoardAssessor(exec);
        }

        public void ResetTurnState()
        {
            Strategy.Reset();
            CounterEconomy.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 1: EndymionStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionStrategy : IDeckStrategy
    {
        private readonly _2026_EndymionExecutor _exec;

        public bool ServantUsed { get; set; }
        public bool MagisterUsed { get; set; }
        public bool ReflectionUsed { get; set; }
        public bool MightyMasterUsed { get; set; }
        public bool MasterCerberusPZoneUsed { get; set; }
        public bool MasterCerberusMZoneUsed { get; set; }
        public bool JackalKingNegateUsed { get; set; }
        public bool ElectrumiteSendUsed { get; set; }
        public bool ElectrumitePopUsed { get; set; }
        public bool SeleneUsed { get; set; }
        public bool SpellPowerMasteryUsed { get; set; }
        public bool KnowledgeUsed { get; set; }
        public bool SecretsUsed { get; set; }
        public bool AbsoluteUsed { get; set; }
        public bool CrowleyUsed { get; set; }
        public bool GarudaUsed { get; set; }

        public EndymionStrategy(_2026_EndymionExecutor exec) => _exec = exec;

        public void Reset()
        {
            ServantUsed = false;
            MagisterUsed = false;
            ReflectionUsed = false;
            MightyMasterUsed = false;
            MasterCerberusPZoneUsed = false;
            MasterCerberusMZoneUsed = false;
            JackalKingNegateUsed = false;
            ElectrumiteSendUsed = false;
            ElectrumitePopUsed = false;
            SeleneUsed = false;
            SpellPowerMasteryUsed = false;
            KnowledgeUsed = false;
            SecretsUsed = false;
            AbsoluteUsed = false;
            CrowleyUsed = false;
            GarudaUsed = false;
        }

        public bool ShouldSummonElectrumite()
        {
            if (_exec.Bot.GetMonsters().Any(m => m.Id == _2026_EndymionExecutor.CardId.HeavymetalfoesElectrumite)) return false;
            var pendMonsters = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.HasType(CardType.Pendulum) && !_exec.IsAceCard(m)).ToList();
            return pendMonsters.Count >= 2;
        }

        public bool ShouldSummonSelene()
        {
            if (_exec.Bot.GetMonsters().Any(m => m.Id == _2026_EndymionExecutor.CardId.SeleneQueenOfTheMasterMagicians)) return false;
            var spellcasters = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.HasRace(CardRace.SpellCaster)).ToList();
            return spellcasters.Count >= 2;
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderByDescending(c => {
                if (c.Id == _2026_EndymionExecutor.CardId.MythicalBeastJackalKing) return 1000;
                if (c.Id == _2026_EndymionExecutor.CardId.EndymionMightyMaster) return 900;
                if (c.Id == _2026_EndymionExecutor.CardId.OddEyesVortexDragon) return 850;
                if (c.Id == _2026_EndymionExecutor.CardId.SeleneQueenOfTheMasterMagicians) return 820;
                if (c.Id == _2026_EndymionExecutor.CardId.AstrographSorcerer) return 800;
                if (c.Id == _2026_EndymionExecutor.CardId.MagisterOfEndymion) return 600;
                if (c.Id == _2026_EndymionExecutor.CardId.ServantOfEndymion) return 500;
                return 100;
            }).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderByDescending(c => {
                if (c.Id == _2026_EndymionExecutor.CardId.ServantOfEndymion) return 1000;
                if (c.Id == _2026_EndymionExecutor.CardId.EndymionMightyMaster) return 900;
                if (c.Id == _2026_EndymionExecutor.CardId.MythicalBeastJackalKing) return 850;
                if (c.Id == _2026_EndymionExecutor.CardId.MagisterOfEndymion) return 800;
                if (c.Id == _2026_EndymionExecutor.CardId.SpellPowerMastery) return 700;
                return 100;
            }).FirstOrDefault();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 2: EndymionCounterEconomy
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionCounterEconomy : IDeckResourceEvaluator
    {
        private readonly _2026_EndymionExecutor _exec;
        private readonly Dictionary<ClientCard, int> _trackedCounters = new Dictionary<ClientCard, int>();

        public EndymionCounterEconomy(_2026_EndymionExecutor exec) => _exec = exec;

        public void Reset()
        {
            var validCards = new HashSet<ClientCard>(_exec.Bot.GetMonsters().Concat(_exec.Bot.GetSpells()));
            var keysToRemove = _trackedCounters.Keys.Where(k => !validCards.Contains(k)).ToList();
            foreach (var k in keysToRemove)
                _trackedCounters.Remove(k);
        }

        public int GetCounters(ClientCard card)
        {
            if (card == null) return 0;
            return _trackedCounters.TryGetValue(card, out int count) ? count : 0;
        }

        public void SetCounters(ClientCard card, int count)
        {
            if (card != null) _trackedCounters[card] = Math.Max(0, count);
        }

        public void AddCounters(ClientCard card, int count)
        {
            if (card != null) _trackedCounters[card] = GetCounters(card) + count;
        }

        public int GetTotalCountersOnField()
        {
            return _exec.Bot.GetMonsters().Concat(_exec.Bot.GetSpells())
                .Where(c => c != null && c.IsFaceup())
                .Sum(c => GetCounters(c));
        }

        public int GetAvailableResourceCount() => GetTotalCountersOnField();

        public bool CanSafelySpendResource(int cost) => CanSafelySpendCounters(cost);

        public _2026_EndymionExecutor.CounterLevel GetCounterLevel()
        {
            int total = GetTotalCountersOnField();
            if (total <= 1) return _2026_EndymionExecutor.CounterLevel.Critical;
            if (total <= 3) return _2026_EndymionExecutor.CounterLevel.Low;
            if (total <= 5) return _2026_EndymionExecutor.CounterLevel.Ready;
            if (total <= 7) return _2026_EndymionExecutor.CounterLevel.ComboReady;
            return _2026_EndymionExecutor.CounterLevel.Surplus;
        }

        public bool CanSafelySpendCounters(int cost)
        {
            int total = GetTotalCountersOnField();
            if (total < cost) return false;
            // Jackal King reserve budget: Always preserve 2 counters if opponent has monsters to negate
            bool hasJackal = _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == _2026_EndymionExecutor.CardId.MythicalBeastJackalKing);
            int reserve = hasJackal ? 2 : 0;
            return (total - cost) >= reserve;
        }

        public IList<int> SelectCounters(int quantity, IList<ClientCard> cards, IList<int> counters)
        {
            if (cards == null || counters == null || cards.Count != counters.Count)
                return null;

            int[] used = new int[counters.Count];
            int needed = quantity;

            // Sync tracked counters with exact OCGCore engine data
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null)
                    SetCounters(cards[i], counters[i]);
            }

            // Priority 1: Magical Citadel of Endymion (Global replacement fuel)
            for (int i = 0; i < cards.Count && needed > 0; i++)
            {
                if (cards[i] != null && cards[i].Id == _2026_EndymionExecutor.CardId.MagicalCitadel)
                {
                    int take = Math.Min(counters[i], needed);
                    used[i] += take;
                    needed -= take;
                    DecisionTracer.Trace("EndymionCounterEconomy", $"Spent {take} counters from Magical Citadel");
                }
            }

            // Priority 2: Mythical Institution & Surplus continuous cards
            for (int i = 0; i < cards.Count && needed > 0; i++)
            {
                if (cards[i] != null && cards[i].Id == _2026_EndymionExecutor.CardId.MythicalInstitution)
                {
                    int take = Math.Min(counters[i] - used[i], needed);
                    used[i] += take;
                    needed -= take;
                    DecisionTracer.Trace("EndymionCounterEconomy", $"Spent {take} counters from Mythical Institution");
                }
            }

            // Priority 3: Non-negator monsters & cards with surplus counters (> 2 on Jackal King)
            for (int i = 0; i < cards.Count && needed > 0; i++)
            {
                if (cards[i] != null && 
                    cards[i].Id != _2026_EndymionExecutor.CardId.ServantOfEndymion && 
                    cards[i].Id != _2026_EndymionExecutor.CardId.MagisterOfEndymion)
                {
                    int avail = counters[i] - used[i];
                    // If Jackal King, preserve at least 2 counters for monster negate if possible
                    if (cards[i].Id == _2026_EndymionExecutor.CardId.MythicalBeastJackalKing)
                    {
                        avail = Math.Max(0, avail - 2);
                    }
                    if (avail > 0)
                    {
                        int take = Math.Min(avail, needed);
                        used[i] += take;
                        needed -= take;
                        DecisionTracer.Trace("EndymionCounterEconomy", $"Spent {take} surplus counters from {cards[i].Name ?? cards[i].Id.ToString()}");
                    }
                }
            }

            // Priority 4: Safe fallback across ANY remaining counters to strictly guarantee sum(used) == quantity
            for (int i = 0; i < cards.Count && needed > 0; i++)
            {
                int avail = counters[i] - used[i];
                if (avail > 0)
                {
                    int take = Math.Min(avail, needed);
                    used[i] += take;
                    needed -= take;
                    DecisionTracer.Trace("EndymionCounterEconomy", $"Spent {take} fallback counters from {cards[i]?.Name ?? cards[i]?.Id.ToString()}");
                }
            }

            // Update remaining internal counters
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null)
                    SetCounters(cards[i], Math.Max(0, counters[i] - used[i]));
            }

            return used;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 3: EndymionScaleResolver
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionScaleResolver : IDeckScaleResolver
    {
        private readonly _2026_EndymionExecutor _exec;

        public EndymionScaleResolver(_2026_EndymionExecutor exec) => _exec = exec;

        public ClientCard PickLowScale()
        {
            // Low scale: Scale 2 (Servant, Reflection)
            return _exec.Bot.Hand.FirstOrDefault(c => 
                c.Id == _2026_EndymionExecutor.CardId.ServantOfEndymion || 
                c.Id == _2026_EndymionExecutor.CardId.ReflectionOfEndymion);
        }

        public ClientCard PickHighScale()
        {
            // High scale: Scale 8 (Magister, Mighty Master)
            return _exec.Bot.Hand.FirstOrDefault(c => 
                c.Id == _2026_EndymionExecutor.CardId.MagisterOfEndymion || 
                c.Id == _2026_EndymionExecutor.CardId.EndymionMightyMaster);
        }

        public ClientCard PickElectrumitePopTarget()
        {
            // Prioritize popping continuous cards that triggered or low-value scales to trigger Astrograph
            var target = _exec.Bot.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() && 
                (s.Id == _2026_EndymionExecutor.CardId.MagisterOfEndymion || 
                 s.Id == _2026_EndymionExecutor.CardId.MythicalInstitution));
            if (target != null) return target;

            return _exec.Bot.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() && 
                s.Id != _2026_EndymionExecutor.CardId.MagicalCitadel);
        }

        public ClientCard PickScalePopTarget() => PickElectrumitePopTarget();
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 4: EndymionMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionMaterialScorer : IDeckMaterialEvaluator
    {
        private readonly _2026_EndymionExecutor _exec;

        public EndymionMaterialScorer(_2026_EndymionExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // Absolute Protection: End Board Ace Bosses must never be linked away casually
            if (card.Id == _2026_EndymionExecutor.CardId.EndymionMightyMaster) return 10000;
            if (card.Id == _2026_EndymionExecutor.CardId.MythicalBeastJackalKing) return 9500;
            if (card.Id == _2026_EndymionExecutor.CardId.OddEyesVortexDragon) return 9000;
            if (card.Id == _2026_EndymionExecutor.CardId.ApollousaBowOfTheGoddess) return 8500;
            if (card.Id == _2026_EndymionExecutor.CardId.AccesscodeTalker) return 8000;
            if (card.Id == _2026_EndymionExecutor.CardId.SeleneQueenOfTheMasterMagicians) return 7500;

            // Fodder Ranking: Spent low-stat monsters
            if (card.Id == _2026_EndymionExecutor.CardId.SpellbookMagician) return 10;
            if (card.Id == _2026_EndymionExecutor.CardId.ReflectionOfEndymion) return 20;
            if (card.Id == _2026_EndymionExecutor.CardId.MagisterOfEndymion) return 25;
            if (card.Id == _2026_EndymionExecutor.CardId.ServantOfEndymion) return 30;

            return 100;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 5: EndymionActionScorer
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionActionScorer : IDeckActionScorer
    {
        private readonly _2026_EndymionExecutor _exec;
        private readonly EndymionPlugin _plugin;

        public EndymionActionScorer(_2026_EndymionExecutor exec, EndymionPlugin plugin)
        {
            _exec = exec;
            _plugin = plugin;
        }

        public bool ShouldActivateMightyMasterWipe()
        {
            // Evaluate Mighty Master 6-counter wipe:
            // Score = Opponent Monster Count * 30 + Opponent Spell Count * 20 - 60 (Cost)
            int enemyCards = _exec.Enemy.GetMonsterCount() + _exec.Enemy.GetSpellCount();
            if (enemyCards < 2) return false;
            return _plugin.CounterEconomy.GetTotalCountersOnField() >= 6;
        }

        public bool ShouldActivateServantSummon()
        {
            // Servant 3 counters SS is highest priority combo starter
            return true;
        }

        public int CalculateActionScore(string actionName, int boardImpact, int netGain, int cost)
        {
            return (boardImpact * 10) + (netGain * 15) - (cost * 10);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 6: EndymionThreatEvaluator
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly _2026_EndymionExecutor _exec;

        public EndymionThreatEvaluator(_2026_EndymionExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;
            if (CardIntelligence.IsFloodgate(card.Id)) score += 1000;
            if (CardIntelligence.IsKnownNegator(card.Id)) score += 800;
            if (CardIntelligence.IsHighThreatChokepoint(card.Id)) score += 600;
            score += card.Attack;
            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return CardIntelligence.IsFloodgate(card.Id) || card.Attack >= 2500;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 7: EndymionBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class EndymionBoardAssessor
    {
        private readonly _2026_EndymionExecutor _exec;

        public EndymionBoardAssessor(_2026_EndymionExecutor exec) => _exec = exec;

        public bool IsLethalSecured()
        {
            int botAtk = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            return botAtk >= _exec.Enemy.LifePoints && _exec.Enemy.GetMonsterCount() == 0;
        }

        public bool IsOpponentBackrowDangerous()
        {
            return _exec.Enemy.GetSpellCount() >= 2;
        }
    }
}
