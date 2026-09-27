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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: ChaosTurboPlugin
    //  Lightning-Fast Deck Thinning & Chaos Banish Engine
    // ═══════════════════════════════════════════════════════════════
    public class ChaosTurboPlugin : DeckPluginBase
    {
        private readonly ChaosTurboExecutor _exec;

        public override string DeckName => "ChaosTurbo";

        public ChaosTurboStrategy StrategyImpl { get; }
        public ChaosTurboMaterialEvaluator MaterialImpl { get; }
        public ChaosTurboThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public ChaosTurboPlugin(ChaosTurboExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new ChaosTurboStrategy(exec);
            MaterialImpl = new ChaosTurboMaterialEvaluator(exec);
            ThreatImpl = new ChaosTurboThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class ChaosTurboStrategy : IDeckStrategy
    {
        private readonly ChaosTurboExecutor _exec;

        public ChaosTurboStrategy(ChaosTurboExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Thunder Dragon search (adds copies of Thunder Dragon)
            var td = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.ThunderDragon);
            if (td != null && context != null && context.Id == ChaosTurboExecutor.CardId.ThunderDragon)
                return td;

            // 2. Night Assailant GY salvage (recovers a Flip monster)
            if (context != null && context.Id == ChaosTurboExecutor.CardId.NightAssailant)
            {
                // Prefer Magician of Faith if strong spells in GY, else another Night Assailant or Dekoichi
                var faith = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.MagicianOfFaith);
                if (faith != null && _exec.Bot.Graveyard.Any(c => c != null && (c.Id == ChaosTurboExecutor.CardId.PotOfGreed || c.Id == ChaosTurboExecutor.CardId.GracefulCharity)))
                    return faith;

                var deko = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.DekoichiTheBattlechantedLocomotive);
                if (deko != null) return deko;

                var assailant = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.NightAssailant && c != context);
                if (assailant != null) return assailant;
            }

            // 3. Sangan search priority:
            // Need LIGHT in GY? Magician of Faith
            // Need DARK in GY? Dekoichi, Night Assailant, Breaker
            int lightInGy = _exec.Bot.Graveyard.Count(c => c != null && c.HasAttribute(CardAttribute.Light));
            int darkInGy = _exec.Bot.Graveyard.Count(c => c != null && c.HasAttribute(CardAttribute.Dark));

            if (lightInGy == 0)
            {
                var faith = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.MagicianOfFaith);
                if (faith != null) return faith;
            }

            if (darkInGy == 0)
            {
                var breaker = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.BreakerTheMagicalWarrior);
                if (breaker != null) return breaker;

                var assailant = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.NightAssailant);
                if (assailant != null) return assailant;

                var deko = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.DekoichiTheBattlechantedLocomotive);
                if (deko != null) return deko;
            }

            var generalBreaker = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.BreakerTheMagicalWarrior);
            if (generalBreaker != null) return generalBreaker;

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Premature / Call of the Haunted:
            var bls = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning);
            if (bls != null) return bls;

            var jinzo = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.Jinzo);
            if (jinzo != null) return jinzo;

            var sorcerer = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.ChaosSorcerer);
            if (sorcerer != null) return sorcerer;

            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class ChaosTurboMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly ChaosTurboExecutor _exec;
        public ChaosTurboMaterialEvaluator(ChaosTurboExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Bosses
            if (card.Id == ChaosTurboExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning) return 50000;
            if (card.Id == ChaosTurboExecutor.CardId.Jinzo) return 30000;
            if (card.Id == ChaosTurboExecutor.CardId.ChaosSorcerer) return 25000;

            // Ideal Discard targets from hand:
            if (card.Id == ChaosTurboExecutor.CardId.NightAssailant) return 1; // triggers Flip monster salvage!
            if (card.Id == ChaosTurboExecutor.CardId.ThunderDragon) return 5; // loads LIGHT to GY!

            // Sangan
            if (card.Id == ChaosTurboExecutor.CardId.Sangan) return 40;

            // Flipped Dekoichi / Faith
            if (card.IsFaceup() && (card.Id == ChaosTurboExecutor.CardId.DekoichiTheBattlechantedLocomotive || card.Id == ChaosTurboExecutor.CardId.MagicianOfFaith))
                return 50;

            // Dead or redundant Spells/Traps
            if (card.Id == ChaosTurboExecutor.CardId.NoblemanOfCrossout && _exec.Enemy.GetMonsters().All(m => m == null || m.IsFaceup()))
                return 15;

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

            // Graceful Charity discard priority:
            // 1. Night Assailant (triggers salvage effect!)
            var assailant = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.NightAssailant);
            if (assailant != null) return assailant;

            // 2. Extra Thunder Dragons in hand (already thinned, now feeds LIGHT to GY)
            var td = candidates.FirstOrDefault(c => c != null && c.Id == ChaosTurboExecutor.CardId.ThunderDragon);
            if (td != null) return td;

            // 3. Excess or unneeded cards
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault(c => c != null);
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault(c => c != null);
        }
    }

    public class ChaosTurboThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly ChaosTurboExecutor _exec;
        public ChaosTurboThreatEvaluator(ChaosTurboExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            int score = 0;

            // Kycoo shuts down our GY banish summon mechanism!
            if (card.Id == 88240808) score += 900;
            if (card.Id == ChaosTurboExecutor.CardId.Jinzo) score += 500;
            if (card.Id == ChaosTurboExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning) score += 800;
            if (card.Attack >= 2400) score += 400;

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == 88240808 || card.Id == ChaosTurboExecutor.CardId.BlackLusterSoldierEnvoyOfTheBeginning;
        }
    }
}
