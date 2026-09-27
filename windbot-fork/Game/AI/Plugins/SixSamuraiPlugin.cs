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
    //  MASTER DECK PLUGIN: SixSamuraiPlugin
    //  Decouples Domain Rules, Strategy, and Scorer from Engine Core
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamuraiPlugin : IDeckPlugin
    {
        private readonly _2026_SixSamuraiExecutor _exec;

        public string DeckName => "SixSamurai";

        public SixSamStrategy Strategy { get; }
        public SixSamCounterEconomy CounterEconomy { get; }
        public SixSamKizaruResolver KizaruResolver { get; }
        public SixSamMaterialScorer MaterialScorer { get; }
        public SixSamActionScorer ActionScorer { get; }
        public SixSamRecoveryPlanner RecoveryPlanner { get; }
        public SixSamBoardAssessor BoardAssessor { get; }

        IDeckStrategy IDeckPlugin.Strategy => Strategy;
        IDeckResourceEvaluator IDeckPlugin.ResourceEvaluator => CounterEconomy;
        IDeckMaterialEvaluator IDeckPlugin.MaterialEvaluator => MaterialScorer;
        IDeckActionScorer IDeckPlugin.ActionScorer => ActionScorer;
        IDeckThreatEvaluator IDeckPlugin.ThreatEvaluator => BoardAssessor;
        IDeckScaleResolver IDeckPlugin.ScaleResolver => null;

        public SixSamuraiPlugin(_2026_SixSamuraiExecutor exec)
        {
            _exec = exec;
            Strategy = new SixSamStrategy(exec);
            CounterEconomy = new SixSamCounterEconomy(exec);
            KizaruResolver = new SixSamKizaruResolver(exec);
            MaterialScorer = new SixSamMaterialScorer(exec);
            ActionScorer = new SixSamActionScorer(exec, this);
            RecoveryPlanner = new SixSamRecoveryPlanner(exec);
            BoardAssessor = new SixSamBoardAssessor(exec);
        }

        public void ResetTurnState()
        {
            Strategy.Reset();
            CounterEconomy.Reset();
            RecoveryPlanner.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 1: SixSamStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamStrategy : IDeckStrategy
    {
        private readonly _2026_SixSamuraiExecutor _exec;

        public bool BattleShogunSearchUsed { get; set; }
        public bool LordShiEnSearchUsed { get; set; }
        public bool TrainerSSUsed { get; set; }
        public bool MonkSSUsed { get; set; }

        public SixSamStrategy(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public void Reset()
        {
            BattleShogunSearchUsed = false;
            LordShiEnSearchUsed = false;
            TrainerSSUsed = false;
            MonkSSUsed = false;
        }

        public IList<ClientCard> PickSearchTarget(long hint, IList<ClientCard> candidates)
        {
            return _exec.Plugin?.KizaruResolver?.PickSearchTarget(candidates, _exec.CurrentLastChainCard);
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            return _exec.Plugin?.KizaruResolver?.PickSearchTarget(candidates, context ?? _exec.CurrentLastChainCard)?.FirstOrDefault();
        }

        ClientCard IDeckStrategy.PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            return PickSpecialSummonTarget(candidates)?.FirstOrDefault();
        }

        public bool ShouldSummonBattleShogun()
        {
            if (_exec.Bot.GetMonsters().Any(m => m.Id == _2026_SixSamuraiExecutor.CardId.BattleShogun)) return false;
            if (_exec.Bot.HasInSpellZone(_2026_SixSamuraiExecutor.CardId.GatewayOfTheSix)) return false;

            var warriors = _exec.Bot.GetMonsters().Where(m => m.IsFaceup() && m.HasRace(CardRace.Warrior) && !_exec.IsAceCard(m)).ToList();
            if (warriors.Count < 2) return false;
            return warriors.Any(m => _2026_SixSamuraiExecutor.IsSixSamurai(m));
        }

        public ClientCard EvaluateAsceticismTarget()
        {
            // Asceticism pairings with identical ATK:
            // 200 ATK: Kageki <-> Fuma / Tactical Trainer
            // 500 ATK: Anarchist Monk <-> Genba
            // 1600 ATK: Mizuho <-> Hatsume
            foreach (var monster in _exec.Bot.GetMonsters().Where(m => m.IsFaceup() && _2026_SixSamuraiExecutor.IsSixSamurai(m)))
            {
                if (monster.Attack == 200 && _exec.Bot.Deck.Any(c => (c.Id == _2026_SixSamuraiExecutor.CardId.TacticalTrainer || c.Id == _2026_SixSamuraiExecutor.CardId.Fuma) && c.Id != monster.Id))
                    return monster;
                if (monster.Attack == 500 && _exec.Bot.Deck.Any(c => (c.Id == _2026_SixSamuraiExecutor.CardId.Genba || c.Id == _2026_SixSamuraiExecutor.CardId.AnarchistMonk) && c.Id != monster.Id))
                    return monster;
                if (monster.Attack == 1600 && _exec.Bot.Deck.Any(c => (c.Id == _2026_SixSamuraiExecutor.CardId.Hatsume || c.Id == _2026_SixSamuraiExecutor.CardId.Mizuho) && c.Id != monster.Id))
                    return monster;
            }
            return null;
        }

        public IList<ClientCard> PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            // 1. Missing Tuner for Synchro Line
            bool hasTuner = _exec.Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Tuner));
            if (!hasTuner)
            {
                var tuner = candidates.FirstOrDefault(c => c.HasType(CardType.Tuner));
                if (tuner != null) return new List<ClientCard> { tuner };
            }

            // 2. Kizaru for on-special-summon search trigger
            var kizaru = candidates.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.Kizaru);
            if (kizaru != null) return new List<ClientCard> { kizaru };

            // 3. Kizan as extensible body
            var kizan = candidates.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.Kizan);
            if (kizan != null) return new List<ClientCard> { kizan };

            return candidates.Take(1).ToList();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 2: SixSamCounterEconomy
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamCounterEconomy : IDeckResourceEvaluator
    {
        private readonly _2026_SixSamuraiExecutor _exec;
        private int _loopActivationsThisTurn = 0;
        private bool _loopCutoffReached = false;

        public SixSamCounterEconomy(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public int GetAvailableResourceCount()
        {
            int gatewayCount = _exec.Bot.GetSpells().Count(s => s != null && s.IsFaceup() && s.Id == _2026_SixSamuraiExecutor.CardId.GatewayOfTheSix);
            int dojoCount = _exec.Bot.GetSpells().Count(s => s != null && s.IsFaceup() && s.Id == _2026_SixSamuraiExecutor.CardId.ShiensDojo);
            return gatewayCount * 4 + dojoCount * 2;
        }

        public bool CanSafelySpendResource(int cost)
        {
            return GetAvailableResourceCount() >= cost;
        }

        public int GetAvailableCounters() => GetAvailableResourceCount();

        public bool CanAfford(int cost) => CanSafelySpendResource(cost);

        public void Reset()
        {
            _loopActivationsThisTurn = 0;
            _loopCutoffReached = false;
        }

        public bool EvaluateGatewaySearch()
        {
            if (_loopCutoffReached) return false;

            // Safety Cutoff to prevent infinite engine freeze (max 20 activations per turn)
            if (_loopActivationsThisTurn >= 20)
            {
                _loopCutoffReached = true;
                DecisionTracer.TraceSkip("GatewayLoop", "Loop threshold reached (20 activations). Preserving state.");
                return false;
            }

            // Lethal OTK Cutoff: If lethal is already secured, STOP extending
            if (IsLethalAchieved())
            {
                _loopCutoffReached = true;
                DecisionTracer.TraceSkip("GatewayLoop", "Lethal OTK damage achieved. Halting combo to enter Battle Phase.");
                return false;
            }

            _loopActivationsThisTurn++;
            DecisionTracer.TraceActivate("GatewayLoop", $"Activating Gateway search (Iteration {_loopActivationsThisTurn})");
            return true;
        }

        public IList<int> SelectCounters(int quantity, IList<ClientCard> cards, IList<int> counters)
        {
            if (cards == null || counters == null || cards.Count != counters.Count)
                return null;

            int[] used = new int[counters.Count];
            int needed = quantity;

            // Priority 1: Drain from Battle Shogun first (Monster body is expendable and vulnerable)
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null && cards[i].Id == _2026_SixSamuraiExecutor.CardId.BattleShogun && needed > 0)
                {
                    int take = Math.Min(counters[i], needed);
                    used[i] += take;
                    needed -= take;
                    DecisionTracer.Trace("CounterEconomy", $"Spent {take} Bushido counters from Battle Shogun");
                }
            }

            // Priority 2: Drain from Shien's Dojo next
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null && cards[i].Id == _2026_SixSamuraiExecutor.CardId.ShiensDojo && needed > 0)
                {
                    int take = Math.Min(counters[i] - used[i], needed);
                    used[i] += take;
                    needed -= take;
                    DecisionTracer.Trace("CounterEconomy", $"Spent {take} Bushido counters from Shien's Dojo");
                }
            }

            // Priority 3: Drain from Gateway of the Six last (Core continuous engine)
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null && cards[i].Id == _2026_SixSamuraiExecutor.CardId.GatewayOfTheSix && needed > 0)
                {
                    int remaining = counters[i] - used[i];
                    if (remaining > 0)
                    {
                        int take = Math.Min(remaining, needed);
                        used[i] += take;
                        needed -= take;
                        DecisionTracer.Trace("CounterEconomy", $"Spent {take} Bushido counters from Gateway of the Six");
                    }
                }
            }

            // Priority 4: Safe fallback across any available cards to strictly guarantee sum(used) == quantity
            if (needed > 0)
            {
                for (int i = 0; i < cards.Count && needed > 0; i++)
                {
                    int remaining = counters[i] - used[i];
                    if (remaining > 0)
                    {
                        int take = Math.Min(remaining, needed);
                        used[i] += take;
                        needed -= take;
                        DecisionTracer.Trace("CounterEconomy", $"Spent {take} residual Bushido counters from Card {cards[i]?.Id}");
                    }
                }
            }

            return used;
        }

        public bool IsLoopCutoffReached => _loopCutoffReached;

        public bool IsLethalAchieved()
        {
            int botAtkSum = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            return botAtkSum >= (_exec.Enemy.LifePoints + 1000) && _exec.Enemy.GetMonsterCount() == 0;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 3: SixSamKizaruResolver
    //  Audited against script/c6579928.lua
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamKizaruResolver
    {
        private readonly _2026_SixSamuraiExecutor _exec;

        public SixSamKizaruResolver(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public IList<ClientCard> PickSearchTarget(IList<ClientCard> candidates, ClientCard lastChainCard)
        {
            // If Gateway search (from Deck/GY):
            var gatewayCandidate = candidates.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.GatewayOfTheSix);
            if (gatewayCandidate != null) return new List<ClientCard> { gatewayCandidate };

            // Check if this search is triggered by Kizaru:
            // s.thfilter requires searching a Six Sam with Attribute NOT currently controlled on MZONE
            bool isKizaruSearch = lastChainCard != null && lastChainCard.Id == _2026_SixSamuraiExecutor.CardId.Kizaru;
            var controlledAttrs = _exec.Bot.GetMonsters()
                .Where(m => m != null && m.IsFaceup())
                .Select(m => m.Attribute)
                .ToHashSet();

            var legalPool = isKizaruSearch 
                ? candidates.Where(c => !controlledAttrs.Contains(c.Attribute)).ToList()
                : candidates.ToList();

            if (legalPool.Count == 0)
                legalPool = candidates.ToList(); // Fallback if engine provides filtered list

            // Strategic Priority Ranking:
            // 1. Missing Tuner for Synchro Boss
            bool hasTuner = _exec.Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Tuner)) || _exec.Bot.Hand.Any(h => h.HasType(CardType.Tuner));
            if (!hasTuner)
            {
                var tuner = legalPool.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.TacticalTrainer || 
                                                          c.Id == _2026_SixSamuraiExecutor.CardId.AnarchistMonk || 
                                                          c.Id == _2026_SixSamuraiExecutor.CardId.Fuma);
                if (tuner != null)
                {
                    DecisionTracer.Trace("KizaruResolver", $"Selected Tuner: {tuner.Name}");
                    return new List<ClientCard> { tuner };
                }
            }

            // 2. Great Shogun Shien for S/T Lock (FIRE attribute)
            if (_exec.Bot.GetMonsters().Count >= 2 && 
                !_exec.Bot.GetMonsters().Any(m => m.Id == _2026_SixSamuraiExecutor.CardId.GreatShogunShien) && 
                !_exec.Bot.Hand.Any(h => h.Id == _2026_SixSamuraiExecutor.CardId.GreatShogunShien))
            {
                var shogun = legalPool.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.GreatShogunShien);
                if (shogun != null)
                {
                    DecisionTracer.Trace("KizaruResolver", "Selected Great Shogun Shien for S/T Lock");
                    return new List<ClientCard> { shogun };
                }
            }

            // 3. Extender & Loop Fuel (Kizan - EARTH)
            var kizan = legalPool.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.Kizan);
            if (kizan != null)
            {
                DecisionTracer.Trace("KizaruResolver", "Selected Kizan for free Special Summon extension");
                return new List<ClientCard> { kizan };
            }

            // 4. Starter if empty board (Kageki - WIND)
            var kageki = legalPool.FirstOrDefault(c => c.Id == _2026_SixSamuraiExecutor.CardId.Kageki);
            if (kageki != null) return new List<ClientCard> { kageki };

            // 5. Default legal card
            var fallback = legalPool.FirstOrDefault();
            return fallback != null ? new List<ClientCard> { fallback } : candidates.Take(1).ToList();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 4: SixSamMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamMaterialScorer : IDeckMaterialEvaluator
    {
        private readonly _2026_SixSamuraiExecutor _exec;

        public SixSamMaterialScorer(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public bool IsProtectedAce(ClientCard card) => GetMaterialCost(card) >= 6000;

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }

        ClientCard IDeckMaterialEvaluator.PickDiscardTarget(IList<ClientCard> candidates, int min)
        {
            return PickDiscardTarget(candidates, min)?.FirstOrDefault();
        }

        ClientCard IDeckMaterialEvaluator.PickDestructionSubstitute(IList<ClientCard> candidates, int min)
        {
            return PickDestructionSubstitute(candidates, min)?.FirstOrDefault();
        }

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // Absolute Protection: Ace Monsters must never be sacrificed casually (Penalty: 10,000)
            if (card.Id == _2026_SixSamuraiExecutor.CardId.LegendaryLordShiEn) return 10000;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.LegendaryShiEn) return 9000;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.LegendaryLordKizan) return 8500;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.GreatShogunShien) return 8000;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.NaturiaBeast || card.Id == _2026_SixSamuraiExecutor.CardId.NaturiaBarkion) return 7500;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.TornadoDragon) return 7000;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.IPMasquerena) return 6500;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.SPLittleKnight) return 6000;

            // Tuner Protection: Preserve sole tuner on field
            if (card.HasType(CardType.Tuner) && _exec.Bot.GetMonsters().Count(m => m.HasType(CardType.Tuner)) == 1)
                return 400;

            // Prime Fodder (Low ATK / Already triggered effect)
            if (card.Id == _2026_SixSamuraiExecutor.CardId.Shinai || card.Id == _2026_SixSamuraiExecutor.CardId.Mizuho) return 10;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.Kageki) return 15;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.BattleShogun && _exec.Bot.HasInSpellZone(_2026_SixSamuraiExecutor.CardId.GatewayOfTheSix)) return 25;
            if (card.Id == _2026_SixSamuraiExecutor.CardId.Kizan) return 30;

            return 100;
        }

        public bool HasExpendableFodder()
        {
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && GetMaterialCost(m) < 100);
        }

        public IList<ClientCard> PickDiscardTarget(IList<ClientCard> cards, int min)
        {
            return cards.OrderBy(c => {
                if (c.Id == _2026_SixSamuraiExecutor.CardId.Shinai) return 1;
                if (c.Id == _2026_SixSamuraiExecutor.CardId.Fuma) return 2;
                if (c.Id == _2026_SixSamuraiExecutor.CardId.SixStrikeDoubleAssault) return 3;
                if (_exec.Bot.Hand.Count(h => h.Id == c.Id) > 1) return 4;
                if (c.Id == _2026_SixSamuraiExecutor.CardId.Kizan) return 5;
                if (c.Id == _2026_SixSamuraiExecutor.CardId.AshBlossom ||
                    c.Id == _2026_SixSamuraiExecutor.CardId.InfiniteImpermanence ||
                    c.Id == _2026_SixSamuraiExecutor.CardId.EffectVeiler ||
                    c.Id == _2026_SixSamuraiExecutor.CardId.Nibiru ||
                    c.Id == _2026_SixSamuraiExecutor.CardId.GhostBelle ||
                    c.Id == _2026_SixSamuraiExecutor.CardId.DrollAndLock ||
                    c.Id == _2026_SixSamuraiExecutor.CardId.CrossoutDesignator ||
                    c.Id == _2026_SixSamuraiExecutor.CardId.HarpiesFeatherDuster)
                    return 9999;
                return 100;
            }).Take(min).ToList();
        }

        public IList<ClientCard> PickDestructionSubstitute(IList<ClientCard> cards, int min)
        {
            return cards.Where(c => c.Controller == 0).OrderBy(c => GetMaterialCost(c)).Take(min).ToList();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 5: SixSamActionScorer
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamActionScorer : IDeckActionScorer
    {
        private readonly _2026_SixSamuraiExecutor _exec;
        private readonly SixSamuraiPlugin _plugin;

        public SixSamActionScorer(_2026_SixSamuraiExecutor exec, SixSamuraiPlugin plugin)
        {
            _exec = exec;
            _plugin = plugin;
        }

        public int CalculateActionScore(string actionName, int boardImpact, int netGain, int cost)
        {
            return boardImpact * 10 + netGain * 15 - cost * 5;
        }

        public bool ShouldSummonKageki()
        {
            // Evaluate Normal Summoning Kageki:
            // ActionScore = Base(60) + Extension(+35 if hand has Lv4) + BaitValue(+25 if opp has negator) - Risk(0)
            if (_exec.Bot.GetMonsters().Count >= 5) return false;
            bool hasValidTarget = _exec.Bot.Hand.Any(c => _2026_SixSamuraiExecutor.IsSixSamurai(c) && c.Id != _2026_SixSamuraiExecutor.CardId.Kageki && c.Level <= 4);
            if (!hasValidTarget) return _exec.Bot.GetMonsters().Count == 0;

            DecisionTracer.Trace("ActionScorer", "Kageki Normal Summon scored 95 (Prime Starter & Bait Enabler)");
            return true;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 6: SixSamRecoveryPlanner
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamRecoveryPlanner
    {
        private readonly _2026_SixSamuraiExecutor _exec;
        public bool StarterInterrupted { get; set; }
        public bool ShogunInterrupted { get; set; }

        public SixSamRecoveryPlanner(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public void Reset()
        {
            StarterInterrupted = false;
            ShogunInterrupted = false;
        }

        public bool HasEmergencyExtenderInHand()
        {
            return _exec.Bot.Hand.Any(c => c.Id == _2026_SixSamuraiExecutor.CardId.Kizan || 
                                           c.Id == _2026_SixSamuraiExecutor.CardId.TacticalTrainer || 
                                           c.Id == _2026_SixSamuraiExecutor.CardId.AnarchistMonk);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 7: SixSamBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class SixSamBoardAssessor : IDeckThreatEvaluator
    {
        private readonly _2026_SixSamuraiExecutor _exec;

        public SixSamBoardAssessor(_2026_SixSamuraiExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            if (card.HasType(CardType.Monster))
                return card.Attack >= 2500 ? 80 : 40;
            return 30;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return card.HasType(CardType.Monster) && card.Attack >= 3000;
        }

        public int EvaluateThreat(ClientCard card) => EvaluateThreatScore(card);

        public bool IsEmergency()
        {
            return _exec.Enemy.GetMonsterCount() >= 2 && _exec.Bot.GetMonsterCount() == 0;
        }

        public bool IsOpponentBackrowDangerous()
        {
            return _exec.Enemy.GetSpellCount() >= 2;
        }

        public bool IsLethalThresholdReached()
        {
            int totalAttack = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            return totalAttack >= _exec.Enemy.LifePoints && _exec.Enemy.GetMonsterCount() == 0;
        }
    }
}

