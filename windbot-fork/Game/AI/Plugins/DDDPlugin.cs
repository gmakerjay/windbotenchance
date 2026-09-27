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
    //  MASTER DECK PLUGIN: DDDPlugin (Layer 3 Domain Helpers)
    //  Decouples Domain Rules, Strategy, and Scorer from Engine Core
    // ═══════════════════════════════════════════════════════════════
    internal class DDDPlugin
    {
        private readonly _2026_DDDExecutor _exec;

        public DDDStrategy Strategy { get; }
        public DDDContractBurnManager ContractBurnManager { get; }
        public DDDScaleAndSearchResolver ScaleAndSearchResolver { get; }
        public DDDMaterialScorer MaterialScorer { get; }
        public DDDActionScorer ActionScorer { get; }
        public DDDBoardAssessor BoardAssessor { get; }

        public DDDPlugin(_2026_DDDExecutor exec)
        {
            _exec = exec;
            Strategy = new DDDStrategy(exec);
            ContractBurnManager = new DDDContractBurnManager(exec);
            ScaleAndSearchResolver = new DDDScaleAndSearchResolver(exec);
            MaterialScorer = new DDDMaterialScorer(exec);
            ActionScorer = new DDDActionScorer(exec, this);
            BoardAssessor = new DDDBoardAssessor(exec);
        }

        public void ResetTurnState()
        {
            Strategy.Reset();
            ContractBurnManager.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 1: DDDStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class DDDStrategy
    {
        private readonly _2026_DDDExecutor _exec;

        public bool GateUsed { get; set; }
        public bool SwampKingUsed { get; set; }
        public bool SwirlSlimeHandUsed { get; set; }
        public bool NecroSlimeUsed { get; set; }
        public bool KeplerUsed { get; set; }
        public bool CopernicusUsed { get; set; }
        public bool GryphonUsed { get; set; }
        public bool GilgameshUsed { get; set; }
        public bool MachinexUsed { get; set; }

        public DDDStrategy(_2026_DDDExecutor exec) => _exec = exec;

        public void Reset()
        {
            GateUsed = false;
            SwampKingUsed = false;
            SwirlSlimeHandUsed = false;
            NecroSlimeUsed = false;
            KeplerUsed = false;
            CopernicusUsed = false;
            GryphonUsed = false;
            GilgameshUsed = false;
            MachinexUsed = false;
        }

        public bool HasGateActive()
        {
            return _exec.Bot.HasInSpellZone(_2026_DDDExecutor.CardId.DctGate);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 2: DDDContractBurnManager
    // ═══════════════════════════════════════════════════════════════
    internal class DDDContractBurnManager
    {
        private readonly _2026_DDDExecutor _exec;

        public DDDContractBurnManager(_2026_DDDExecutor exec) => _exec = exec;

        public void Reset() { }

        public int GetActiveContractCount()
        {
            return _exec.Bot.GetSpells().Count(s => s != null && s.IsFaceup() && 
                (s.Id == _2026_DDDExecutor.CardId.DctGate || 
                 s.Id == _2026_DDDExecutor.CardId.DctSwampKing || 
                 s.Id == _2026_DDDExecutor.CardId.DctZeroKing || 
                 s.Id == _2026_DDDExecutor.CardId.DctEternalDarkness));
        }

        public int GetProjectedBurnDamage()
        {
            return GetActiveContractCount() * 1000;
        }

        public bool IsInBurnDangerZone()
        {
            // Threat gate: If next Standby Phase burn could reduce LP to <= 0 or critical
            int burn = GetProjectedBurnDamage();
            return _exec.Bot.LifePoints <= (burn + 1000);
        }

        public bool ShouldEmergencyClearContract()
        {
            // Trigger emergency clearance via Machinex detach or Orthros pop
            return IsInBurnDangerZone();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 3: DDDScaleAndSearchResolver
    // ═══════════════════════════════════════════════════════════════
    internal class DDDScaleAndSearchResolver
    {
        private readonly _2026_DDDExecutor _exec;

        public DDDScaleAndSearchResolver(_2026_DDDExecutor exec) => _exec = exec;

        public ClientCard PickGateSearchTarget(IList<ClientCard> candidates)
        {
            // Priority 1: Kepler for P-Scale setup if not Normal Summoned
            if (!_exec.Bot.GetMonsters().Any(m => m.Id == _2026_DDDExecutor.CardId.Kepler) &&
                !_exec.Bot.Hand.Any(c => c.Id == _2026_DDDExecutor.CardId.Kepler))
            {
                var kepler = candidates.FirstOrDefault(c => c.Id == _2026_DDDExecutor.CardId.Kepler);
                if (kepler != null) return kepler;
            }

            // Priority 2: Swirl Slime for Fusion Line
            if (!_exec.Bot.Hand.Any(c => c.Id == _2026_DDDExecutor.CardId.SwirlSlime))
            {
                var swirl = candidates.FirstOrDefault(c => c.Id == _2026_DDDExecutor.CardId.SwirlSlime);
                if (swirl != null) return swirl;
            }

            // Priority 3: Gryphon for Level 4 body + extra search
            var gryphon = candidates.FirstOrDefault(c => c.Id == _2026_DDDExecutor.CardId.Gryphon);
            if (gryphon != null) return gryphon;

            // Priority 4: Copernicus
            var copernicus = candidates.FirstOrDefault(c => c.Id == _2026_DDDExecutor.CardId.Copernicus);
            if (copernicus != null) return copernicus;

            return candidates.FirstOrDefault();
        }

        public (ClientCard lowScale, ClientCard highScale) PickGilgameshScales(IList<ClientCard> candidates)
        {
            var low = candidates.FirstOrDefault(c => c.Id == _2026_DDDExecutor.CardId.CountSurveyor || 
                                                    c.Id == _2026_DDDExecutor.CardId.Kepler);
            var high = candidates.FirstOrDefault(c => c.Id == _2026_DDDExecutor.CardId.ScaleSurveyor || 
                                                     c.Id == _2026_DDDExecutor.CardId.Orthros || 
                                                     c.Id == _2026_DDDExecutor.CardId.Gryphon);
            return (low, high);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 4: DDDMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class DDDMaterialScorer
    {
        private readonly _2026_DDDExecutor _exec;

        public DDDMaterialScorer(_2026_DDDExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // Absolute Protection: End Board Bosses must never be linked/fused away casually
            if (card.Id == _2026_DDDExecutor.CardId.WaveHighKingCaesar) return 10000;
            if (card.Id == _2026_DDDExecutor.CardId.CursedKingSiegfried) return 9500;
            if (card.Id == _2026_DDDExecutor.CardId.DeusMachinex) return 9000;
            if (card.Id == _2026_DDDExecutor.CardId.DimensionalKingArcCrisis) return 8500;
            if (card.Id == _2026_DDDExecutor.CardId.SkyKingZeusRagnarok) return 8000;
            if (card.Id == _2026_DDDExecutor.CardId.FlameHighKingGenghis) return 7500;

            // Step Bosses (Intended as bridges/overlay bases)
            if (card.Id == _2026_DDDExecutor.CardId.AbyssKingGilgamesh) return 200;
            if (card.Id == _2026_DDDExecutor.CardId.MarksmanKingTell) return 250;
            if (card.Id == _2026_DDDExecutor.CardId.FlameKingGenghis) return 300;

            // Fodder Ranking
            if (card.Id == _2026_DDDExecutor.CardId.SwirlSlime || card.Id == _2026_DDDExecutor.CardId.NecroSlime) return 10;
            if (card.Id == _2026_DDDExecutor.CardId.Lamia) return 15;
            if (card.Id == _2026_DDDExecutor.CardId.Kepler) return 20;
            if (card.Id == _2026_DDDExecutor.CardId.Copernicus) return 25;
            if (card.Id == _2026_DDDExecutor.CardId.Gryphon) return 30;

            return 100;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 5: DDDActionScorer
    // ═══════════════════════════════════════════════════════════════
    internal class DDDActionScorer
    {
        private readonly _2026_DDDExecutor _exec;
        private readonly DDDPlugin _plugin;

        public DDDActionScorer(_2026_DDDExecutor exec, DDDPlugin plugin)
        {
            _exec = exec;
            _plugin = plugin;
        }

        public bool ShouldOverlayMachinex()
        {
            // Machinex can overlay directly on Gilgamesh or Tell/Caesar to provide immediate 2-material monster steal
            return true;
        }

        public bool ShouldSummonGilgamesh()
        {
            // Gilgamesh is vital if scales are not set yet
            return !_exec.HasBothPZonesFilled();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 6: DDDBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class DDDBoardAssessor
    {
        private readonly _2026_DDDExecutor _exec;

        public DDDBoardAssessor(_2026_DDDExecutor exec) => _exec = exec;

        public bool IsLethalAttackAvailable()
        {
            int totalAtk = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            return totalAtk >= _exec.Enemy.LifePoints && _exec.Enemy.GetMonsterCount() == 0;
        }
    }
}
