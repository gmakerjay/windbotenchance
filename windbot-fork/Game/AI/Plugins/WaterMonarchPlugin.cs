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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: WaterMonarchPlugin
    //  Classic 2005 GOAT Format Water Monarch Control Strategy
    // ═══════════════════════════════════════════════════════════════
    public class WaterMonarchPlugin : DeckPluginBase
    {
        private readonly WaterMonarchExecutor _exec;

        public override string DeckName => "WaterMonarch";

        public WaterMonarchStrategy StrategyImpl { get; }
        public WaterMonarchMaterialEvaluator MaterialImpl { get; }
        public WaterMonarchThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public WaterMonarchPlugin(WaterMonarchExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new WaterMonarchStrategy(exec);
            MaterialImpl = new WaterMonarchMaterialEvaluator(exec);
            ThreatImpl = new WaterMonarchThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class WaterMonarchStrategy : IDeckStrategy
    {
        private readonly WaterMonarchExecutor _exec;

        public WaterMonarchStrategy(WaterMonarchExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Sinister Serpent (infinite card advantage, discard fuel for Tribe/Graceful, Metamorphosis fuel)
            if (!_exec.Bot.HasInHand(WaterMonarchExecutor.CardId.SinisterSerpent) &&
                !_exec.Bot.HasInGraveyard(WaterMonarchExecutor.CardId.SinisterSerpent))
            {
                var sinister = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.SinisterSerpent);
                if (sinister != null) return sinister;
            }

            // 2. Tsukuyomi (if we have TER on field or Flip monsters to reuse)
            bool hasTerOnField = _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == WaterMonarchExecutor.CardId.ThousandEyesRestrict);
            bool hasFaceupFaith = _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == WaterMonarchExecutor.CardId.MagicianOfFaith);
            if ((hasTerOnField || hasFaceupFaith) && !_exec.Bot.HasInHand(WaterMonarchExecutor.CardId.Tsukuyomi))
            {
                var tsukuyomi = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.Tsukuyomi);
                if (tsukuyomi != null) return tsukuyomi;
            }

            // 3. Magician of Faith (if high-value spell in GY like Pot, Charity, Duo, Snatch, Storm)
            bool hasPowerSpellInGy = _exec.Bot.Graveyard.Any(c => c != null &&
                (c.Id == WaterMonarchExecutor.CardId.PotOfGreed ||
                 c.Id == WaterMonarchExecutor.CardId.GracefulCharity ||
                 c.Id == WaterMonarchExecutor.CardId.DelinquentDuo ||
                 c.Id == WaterMonarchExecutor.CardId.SnatchSteal ||
                 c.Id == WaterMonarchExecutor.CardId.HeavyStorm));
            if (hasPowerSpellInGy && !_exec.Bot.HasInHand(WaterMonarchExecutor.CardId.MagicianOfFaith))
            {
                var faith = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.MagicianOfFaith);
                if (faith != null) return faith;
            }

            // 4. Yomi Ship (if opponent has threatening beaters and we need battle deterrent)
            bool oppHasBeater = _exec.Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 1800);
            if (oppHasBeater)
            {
                var yomi = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.YomiShip);
                if (yomi != null) return yomi;
            }

            // 5. Tribe-Infecting Virus (swarm clear)
            var virus = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.TribeInfectingVirus);
            if (virus != null) return virus;

            // 6. D.D. Assailant (universal 1-for-1 banish beater)
            var assailant = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.DdAssailant);
            if (assailant != null) return assailant;

            // 7. Tsukuyomi general
            var generalTsukuyomi = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.Tsukuyomi);
            if (generalTsukuyomi != null) return generalTsukuyomi;

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Mother Grizzly float check (summoning from Deck)
            if (candidates.Any(c => c != null && c.Location == CardLocation.Deck))
            {
                return PickGrizzlyTarget(candidates);
            }

            // Reborn / Call of the Haunted priority
            var mobius = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.MobiusTheFrostMonarch);
            if (mobius != null) return mobius;

            var zaborg = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.ZaborgTheThunderMonarch);
            if (zaborg != null) return zaborg;

            var assailant = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.DdAssailant);
            if (assailant != null) return assailant;

            var breaker = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.BreakerTheMagicalWarrior);
            if (breaker != null) return breaker;

            var yomi = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.YomiShip);
            if (yomi != null) return yomi;

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickGrizzlyTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Sinister Serpent if not in hand or GY (key engine piece)
            if (!_exec.Bot.HasInHand(WaterMonarchExecutor.CardId.SinisterSerpent) &&
                !_exec.Bot.HasInGraveyard(WaterMonarchExecutor.CardId.SinisterSerpent))
            {
                var sinister = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.SinisterSerpent);
                if (sinister != null) return sinister;
            }

            // 2. Yomi Ship if opponent still has attacking monsters
            bool oppHasAttackers = _exec.Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsAttack());
            if (oppHasAttackers)
            {
                var yomi = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.YomiShip);
                if (yomi != null) return yomi;
            }

            // 3. Mother Grizzly (chain stall float)
            var nextGrizzly = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.MotherGrizzly);
            if (nextGrizzly != null) return nextGrizzly;

            // 4. Sinister Serpent
            var anySinister = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.SinisterSerpent);
            if (anySinister != null) return anySinister;

            // 5. Yomi Ship
            var anyYomi = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.YomiShip);
            if (anyYomi != null) return anyYomi;

            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class WaterMonarchMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly WaterMonarchExecutor _exec;
        public WaterMonarchMaterialEvaluator(WaterMonarchExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Bosses & Key Monarchs (never tribute lightly!)
            if (card.Id == WaterMonarchExecutor.CardId.MobiusTheFrostMonarch) return 40000;
            if (card.Id == WaterMonarchExecutor.CardId.ZaborgTheThunderMonarch) return 40000;

            // Thousand-Eyes Restrict with an equipped monster should NOT be tributed
            if (card.Id == WaterMonarchExecutor.CardId.ThousandEyesRestrict && card.EquipCards != null && card.EquipCards.Count > 0)
                return 30000;

            // Sheep Tokens are prime tribute fodder (Level 1)
            if (card.Id == WaterMonarchExecutor.CardId.SheepToken) return 5;

            // Sinister Serpent is infinite recursion
            if (card.Id == WaterMonarchExecutor.CardId.SinisterSerpent) return 10;

            // Magician of Faith after being flipped face-up has 300 ATK and is prime tribute fodder
            if (card.Id == WaterMonarchExecutor.CardId.MagicianOfFaith && card.IsFaceup()) return 20;

            // Gravekeeper's Spy after flipping: high DEF body served its purpose, great for Tribute
            if (card.Id == WaterMonarchExecutor.CardId.GravekeepersSpy && card.IsFaceup()) return 30;

            // Sangan floats when sent to GY
            if (card.Id == WaterMonarchExecutor.CardId.Sangan) return 50;

            // Mother Grizzly on field
            if (card.Id == WaterMonarchExecutor.CardId.MotherGrizzly) return 60;

            // Yomi Ship
            if (card.Id == WaterMonarchExecutor.CardId.YomiShip) return 70;

            // D.D. Assailant (valuable 1700 beater)
            if (card.Id == WaterMonarchExecutor.CardId.DdAssailant) return 150;

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

            // 1. Sinister Serpent (returns to hand next Standby)
            var sinister = candidates.FirstOrDefault(c => c != null && c.Id == WaterMonarchExecutor.CardId.SinisterSerpent);
            if (sinister != null) return sinister;

            // 2. Extra Spells / Traps that are unplayable or redundant
            // 3. Lowest value monsters
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault(c => c != null);
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault(c => c != null);
        }
    }

    public class WaterMonarchThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly WaterMonarchExecutor _exec;
        public WaterMonarchThreatEvaluator(WaterMonarchExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;

            // High ATK beaters in GOAT (Jinzo, BLS, Chaos Sorcerer, Monarchs)
            if (card.Attack >= 2400) score += 500;
            if (card.Id == 77585513) score += 600; // Jinzo (shuts down Traps)
            if (card.Id == 72989439) score += 800; // BLS
            if (card.Id == WaterMonarchExecutor.CardId.ThousandEyesRestrict) score += 700;

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == 72989439 || // BLS
                   (card.IsFaceup() && card.Attack >= 2800);
        }
    }
}
