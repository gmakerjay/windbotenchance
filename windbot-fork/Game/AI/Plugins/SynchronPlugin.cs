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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: SynchronPlugin
    //  Decouples Domain Rules, Strategy, and Material Evaluation for Synchron / Junk / Stardust
    // ═══════════════════════════════════════════════════════════════
    public class SynchronPlugin : DeckPluginBase
    {
        private readonly SynchronExecutor _exec;

        public override string DeckName => "Synchron";

        public SynchronStrategy StrategyImpl { get; }
        public SynchronMaterialEvaluator MaterialImpl { get; }
        public SynchronThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public SynchronPlugin(SynchronExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new SynchronStrategy(exec);
            MaterialImpl = new SynchronMaterialEvaluator(exec);
            ThreatImpl = new SynchronThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class SynchronStrategy : IDeckStrategy
    {
        private readonly SynchronExecutor _exec;
        public SynchronStrategy(SynchronExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Priority for Special Summons / Revivals (e.g., from Junk Synchron, Junk Converter, Accel Synchro, Synchro Rumble, Dis Pater)
            int[] priorities = {
                SynchronExecutor.CardId.FullSpeedWarrior,
                SynchronExecutor.CardId.Doppelwarrior,
                SynchronExecutor.CardId.JunkConverter,
                SynchronExecutor.CardId.StardustTrail,
                SynchronExecutor.CardId.AssaultSynchron,
                SynchronExecutor.CardId.JetSynchron,
                SynchronExecutor.CardId.StarjunkSynchron,
                SynchronExecutor.CardId.JunkSynchron,
                SynchronExecutor.CardId.StardustSynchron,
                SynchronExecutor.CardId.ScrapSynchron,
                SynchronExecutor.CardId.CosmicBlazarDragon,
                SynchronExecutor.CardId.BystialDisPater,
                SynchronExecutor.CardId.CrystalWingSynchroDragon,
                SynchronExecutor.CardId.StardustDragonVictimSanctuary,
                SynchronExecutor.CardId.StardustDragon,
                SynchronExecutor.CardId.AccelSynchroStardustDragon
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

            int ctxId = context != null ? context.Id : 0;

            // Search from Full-Speed Warrior (Junk Synchron or S/T mentioning Junk Warrior)
            if (ctxId == SynchronExecutor.CardId.FullSpeedWarrior)
            {
                var fellowship = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.SynchroFellowship);
                if (fellowship != null) return fellowship;

                var junkSync = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.JunkSynchron);
                if (junkSync != null) return junkSync;

                var signal = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.JunkSignal);
                if (signal != null) return signal;
            }

            // Search from Stardust Synchron (S/T mentioning Stardust Dragon)
            if (ctxId == SynchronExecutor.CardId.StardustSynchron)
            {
                var illumination = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.StardustIllumination);
                if (illumination != null) return illumination;

                var fellowship = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.SynchroFellowship);
                if (fellowship != null) return fellowship;

                var signal = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.JunkSignal);
                if (signal != null) return signal;
            }

            // Search from Tuning (Synchron Tuner)
            if (ctxId == SynchronExecutor.CardId.Tuning)
            {
                var junkSync = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.JunkSynchron);
                if (junkSync != null && !_exec.Bot.Hand.Any(h => h.Id == SynchronExecutor.CardId.JunkSynchron)) return junkSync;

                var stardustSync = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.StardustSynchron);
                if (stardustSync != null && !_exec.Bot.Hand.Any(h => h.Id == SynchronExecutor.CardId.StardustSynchron)) return stardustSync;

                var starjunk = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.StarjunkSynchron);
                if (starjunk != null) return starjunk;

                var assaultSync = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.AssaultSynchron);
                if (assaultSync != null) return assaultSync;

                var jetSync = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.JetSynchron);
                if (jetSync != null) return jetSync;
            }

            // Search from ROTA (Warrior monster)
            if (ctxId == SynchronExecutor.CardId.ReinforcementOfTheArmy)
            {
                var fullSpeed = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.FullSpeedWarrior);
                if (fullSpeed != null && !_exec.Bot.Hand.Any(h => h.Id == SynchronExecutor.CardId.FullSpeedWarrior)) return fullSpeed;

                var doppel = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.Doppelwarrior);
                if (doppel != null && !_exec.Bot.Hand.Any(h => h.Id == SynchronExecutor.CardId.Doppelwarrior)) return doppel;

                var junkConv = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.JunkConverter);
                if (junkConv != null && !_exec.Bot.Hand.Any(h => h.Id == SynchronExecutor.CardId.JunkConverter)) return junkConv;

                var junkSync = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.JunkSynchron);
                if (junkSync != null && !_exec.Bot.Hand.Any(h => h.Id == SynchronExecutor.CardId.JunkSynchron)) return junkSync;
            }

            // Search from Crimson Dragon (Spell/Trap that mentions Crimson Dragon / Synchro)
            if (ctxId == SynchronExecutor.CardId.CrimsonDragon)
            {
                var rumble = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.SynchroRumble);
                if (rumble != null) return rumble;
            }

            // General search priorities
            int[] generalOrder = {
                SynchronExecutor.CardId.SynchroFellowship,
                SynchronExecutor.CardId.JunkSynchron,
                SynchronExecutor.CardId.FullSpeedWarrior,
                SynchronExecutor.CardId.Doppelwarrior,
                SynchronExecutor.CardId.StardustSynchron,
                SynchronExecutor.CardId.JunkConverter,
                SynchronExecutor.CardId.StarjunkSynchron,
                SynchronExecutor.CardId.AssaultSynchron,
                SynchronExecutor.CardId.JetSynchron,
                SynchronExecutor.CardId.JunkSignal
            };

            foreach (int id in generalOrder)
            {
                var card = candidates.FirstOrDefault(c => c != null && c.Id == id);
                if (card != null) return card;
            }

            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class SynchronMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly SynchronExecutor _exec;
        public SynchronMaterialEvaluator(SynchronExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // Ultimate Ace Bosses & Floodgates / Disruption — DO NOT SACRIFICE OR USE AS MATERIAL
            if (card.Id == SynchronExecutor.CardId.CosmicBlazarDragon) return 100;
            if (card.Id == SynchronExecutor.CardId.RedSupernovaDragon) return 100;
            if (card.Id == SynchronExecutor.CardId.BystialDisPater) return 95;
            if (card.Id == SynchronExecutor.CardId.CrystalWingSynchroDragon) return 95;
            if (card.Id == SynchronExecutor.CardId.CrimsonDragon) return 90;
            if (card.Id == SynchronExecutor.CardId.SatelliteWarrior) return 90;
            if (card.Id == SynchronExecutor.CardId.StardustWarrior) return 90;
            if (card.Id == SynchronExecutor.CardId.StardustDragonVictimSanctuary) return 85;
            if (card.Id == SynchronExecutor.CardId.TGHyperLibrarian) return 70; // Keep on board for continuous draws!
            if (card.Id == SynchronExecutor.CardId.ScrapWarrior) return 65; // Protects Speeder from negation!
            if (card.Id == SynchronExecutor.CardId.StardustDragon) return 50;

            // Mid-tier Synchro stepping stones
            if (card.Id == SynchronExecutor.CardId.AccelSynchroStardustDragon) return 30;
            if (card.Id == SynchronExecutor.CardId.JunkSpeeder) return 15; // Once effect resolved, used as high level non-tuner material

            // Tuner and non-Tuner swarming fodder (Lowest cost = preferred material)
            if (card.Id == SynchronExecutor.CardId.JetSynchron) return 1;
            if (card.Id == SynchronExecutor.CardId.ScrapSynchron) return 1;
            if (card.Id == SynchronExecutor.CardId.AssaultSynchron) return 2;
            if (card.Id == SynchronExecutor.CardId.StarjunkSynchron) return 3;
            if (card.Id == SynchronExecutor.CardId.StardustTrail) return 4;
            if (card.Id == SynchronExecutor.CardId.StardustSynchron) return 4;
            if (card.Id == SynchronExecutor.CardId.FormulaSynchron) return 5;
            if (card.Id == SynchronExecutor.CardId.Doppelwarrior) return 6;
            if (card.Id == SynchronExecutor.CardId.FullSpeedWarrior) return 6;
            if (card.Id == SynchronExecutor.CardId.JunkConverter) return 7;
            if (card.Id == SynchronExecutor.CardId.JunkSynchron) return 9;

            return 10;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Discard priorities (Best cards to send to GY for graveyard triggers or revival)
            int[] discardOrder = {
                SynchronExecutor.CardId.StardustTrail,         // Can Special Summon itself when a monster is tributed!
                SynchronExecutor.CardId.JunkConverter,         // Triggers when sent as material, or fodder in GY for Junk Synchron
                SynchronExecutor.CardId.FullSpeedWarrior,      // Level 2 target for Junk Synchron / Starjunk revival
                SynchronExecutor.CardId.Doppelwarrior,         // Level 2 target for revival
                SynchronExecutor.CardId.JetSynchron,           // Can revive itself from GY by discarding 1
                SynchronExecutor.CardId.StardustSynchron,      // Can tribute from GY to revive self
                SynchronExecutor.CardId.StarjunkSynchron,      // GY protection effect
                SynchronExecutor.CardId.ScrapSynchron,         // GY protection effect
                SynchronExecutor.CardId.AssaultSynchron,       // GY revival for Dragon Synchros
                SynchronExecutor.CardId.StardustIllumination   // GY level modulation
            };

            foreach (int id in discardOrder)
            {
                var card = candidates.FirstOrDefault(c => c != null && c.Id == id);
                if (card != null) return card;
            }

            // Discard duplicate spells or non-vital cards
            var duplicate = candidates.GroupBy(c => c.Id).FirstOrDefault(g => g.Count() > 1)?.FirstOrDefault();
            if (duplicate != null && !CardIntelligence.IsHandtrap(duplicate.Id)) return duplicate;

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            // Prefer banishing Scrap Synchron or Starjunk Synchron from GY as substitute
            var scrap = candidates.FirstOrDefault(c => c.Id == SynchronExecutor.CardId.ScrapSynchron);
            if (scrap != null) return scrap;

            return candidates.FirstOrDefault();
        }
    }

    public class SynchronThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly SynchronExecutor _exec;
        public SynchronThreatEvaluator(SynchronExecutor exec) => _exec = exec;

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
