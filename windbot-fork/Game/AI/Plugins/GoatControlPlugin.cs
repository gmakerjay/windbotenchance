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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: GoatControlPlugin
    //  Classic 2005 GOAT Format Control Strategy
    // ═══════════════════════════════════════════════════════════════
    public class GoatControlPlugin : DeckPluginBase
    {
        private readonly GoatControlExecutor _exec;

        public override string DeckName => "GoatControl";

        public GoatControlStrategy StrategyImpl { get; }
        public GoatControlMaterialEvaluator MaterialImpl { get; }
        public GoatControlThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public GoatControlPlugin(GoatControlExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new GoatControlStrategy(exec);
            MaterialImpl = new GoatControlMaterialEvaluator(exec);
            ThreatImpl = new GoatControlThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class GoatControlStrategy : IDeckStrategy
    {
        private readonly GoatControlExecutor _exec;

        public GoatControlStrategy(GoatControlExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Sangan search priority:
            // 1. Sinister Serpent (infinite card advantage & discard/fusion fodder)
            if (!_exec.Bot.HasInHand(GoatControlExecutor.CardId.SinisterSerpent) &&
                !_exec.Bot.HasInGraveyard(GoatControlExecutor.CardId.SinisterSerpent))
            {
                var sinister = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.SinisterSerpent);
                if (sinister != null) return sinister;
            }

            // 2. Tsukuyomi (if we have TER on field or Flip monsters to reuse)
            bool hasTerOnField = _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == GoatControlExecutor.CardId.ThousandEyesRestrict);
            if (hasTerOnField && !_exec.Bot.HasInHand(GoatControlExecutor.CardId.Tsukuyomi))
            {
                var tsukuyomi = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.Tsukuyomi);
                if (tsukuyomi != null) return tsukuyomi;
            }

            // 3. Magician of Faith (if high-value spell in GY like Pot, Charity, Duo, Snatch)
            bool hasPowerSpellInGy = _exec.Bot.Graveyard.Any(c => c != null &&
                (c.Id == GoatControlExecutor.CardId.PotOfGreed ||
                 c.Id == GoatControlExecutor.CardId.GracefulCharity ||
                 c.Id == GoatControlExecutor.CardId.DelinquentDuo ||
                 c.Id == GoatControlExecutor.CardId.SnatchSteal ||
                 c.Id == GoatControlExecutor.CardId.HeavyStorm));
            if (hasPowerSpellInGy && !_exec.Bot.HasInHand(GoatControlExecutor.CardId.MagicianOfFaith))
            {
                var faith = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.MagicianOfFaith);
                if (faith != null) return faith;
            }

            // 4. D.D. Warrior Lady (universal board-breaker / boss out)
            var ddwl = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.DdWarriorLady);
            if (ddwl != null) return ddwl;

            // 5. Tribe-Infecting Virus (swarm clear)
            var virus = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.TribeInfectingVirus);
            if (virus != null) return virus;

            // 6. Tsukuyomi general
            var generalTsukuyomi = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.Tsukuyomi);
            if (generalTsukuyomi != null) return generalTsukuyomi;

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Reborn / Call of the Haunted priority
            var bls = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning);
            if (bls != null) return bls;

            var jinzo = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.Jinzo);
            if (jinzo != null) return jinzo;

            var airknight = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.AirknightParshath);
            if (airknight != null) return airknight;

            var breaker = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.BreakerTheMagicalWarrior);
            if (breaker != null) return breaker;

            var ddwl = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.DdWarriorLady);
            if (ddwl != null) return ddwl;

            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class GoatControlMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly GoatControlExecutor _exec;
        public GoatControlMaterialEvaluator(GoatControlExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Bosses & Key Negators
            if (card.Id == GoatControlExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning) return 50000;
            if (card.Id == GoatControlExecutor.CardId.Jinzo) return 30000;
            if (card.Id == GoatControlExecutor.CardId.AirknightParshath) return 20000;

            // Thousand-Eyes Restrict with an equipped monster should NOT be tributed lightly unless Tsukuyomi reset is needed
            if (card.Id == GoatControlExecutor.CardId.ThousandEyesRestrict && card.EquipCards != null && card.EquipCards.Count > 0)
                return 15000;

            // Sangan floats when sent from field to GY
            if (card.Id == GoatControlExecutor.CardId.Sangan) return 50;

            // Sinister Serpent is infinite recursion
            if (card.Id == GoatControlExecutor.CardId.SinisterSerpent) return 10;

            // Scapegoat Tokens are ideal tribute fodder
            if (card.Id == GoatControlExecutor.CardId.SheepToken) return 5;

            // Magician of Faith after being flipped face-up has 300 ATK and is prime tribute fodder
            if (card.Id == GoatControlExecutor.CardId.MagicianOfFaith && card.IsFaceup()) return 20;

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

            // Graceful Charity / Tribe-Infecting Virus discard priority:
            // 1. Sinister Serpent (returns to hand next Standby)
            var sinister = candidates.FirstOrDefault(c => c != null && c.Id == GoatControlExecutor.CardId.SinisterSerpent);
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

    public class GoatControlThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly GoatControlExecutor _exec;
        public GoatControlThreatEvaluator(GoatControlExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;

            // High ATK beaters in GOAT (Jinzo, BLS, Chaos Sorcerer, Airknight)
            if (card.Attack >= 2400) score += 500;
            if (card.Id == GoatControlExecutor.CardId.Jinzo) score += 600; // Shuts down our 6 Traps
            if (card.Id == GoatControlExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning) score += 800;
            if (card.Id == GoatControlExecutor.CardId.ThousandEyesRestrict) score += 700; // Sucks our monsters

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == GoatControlExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning ||
                   (card.IsFaceup() && card.Attack >= 2800);
        }
    }
}
