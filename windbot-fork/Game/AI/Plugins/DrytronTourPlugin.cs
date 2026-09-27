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
    //  MASTER DECK PLUGIN: DrytronTourPlugin
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronTourPlugin
    {
        private readonly DrytronTourExecutor _exec;

        public bool AlphaUsed { get; set; }
        public bool ZetaUsed { get; set; }
        public bool NuUsed { get; set; }
        public bool GammaUsed { get; set; }
        public bool DeltaUsed { get; set; }
        public bool BetaUsed { get; set; }

        public DrytronStrategy Strategy { get; }
        public DrytronTributeManager TributeManager { get; }
        public DrytronRitualAdvisor RitualAdvisor { get; }
        public DrytronMaterialScorer MaterialScorer { get; }
        public DrytronBoardAssessor BoardAssessor { get; }

        public DrytronTourPlugin(DrytronTourExecutor exec)
        {
            _exec = exec;
            Strategy = new DrytronStrategy(exec);
            TributeManager = new DrytronTributeManager(exec);
            RitualAdvisor = new DrytronRitualAdvisor(exec);
            MaterialScorer = new DrytronMaterialScorer(exec);
            BoardAssessor = new DrytronBoardAssessor(exec);
        }

        public void ResetTurnState()
        {
            AlphaUsed = false;
            ZetaUsed = false;
            NuUsed = false;
            GammaUsed = false;
            DeltaUsed = false;
            BetaUsed = false;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: DrytronStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronStrategy
    {
        private readonly DrytronTourExecutor _exec;

        public DrytronStrategy(DrytronTourExecutor exec)
        {
            _exec = exec;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: DrytronTributeManager
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronTributeManager
    {
        private readonly DrytronTourExecutor _exec;

        public DrytronTributeManager(DrytronTourExecutor exec)
        {
            _exec = exec;
        }

        public bool CanTributeForDrytron(ClientCard activator)
        {
            // Can tribute another Drytron or Ritual monster from hand or field
            var tributes = _exec.Bot.Hand.Concat(_exec.Bot.GetMonsters())
                .Where(c => c != activator && (c.HasSetcode(0x154) || c.HasType(CardType.Ritual)) && !_exec.IsAceCard(c))
                .ToList();

            return tributes.Count > 0;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: DrytronRitualAdvisor
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronRitualAdvisor
    {
        private readonly DrytronTourExecutor _exec;

        public DrytronRitualAdvisor(DrytronTourExecutor exec)
        {
            _exec = exec;
        }

        public bool CanRitualSummon()
        {
            bool hasDraconids = _exec.Bot.HasInHand(DrytronTourExecutor.CardId.DrytronMeteonisDADraconids) ||
                                _exec.Bot.HasInGraveyard(DrytronTourExecutor.CardId.DrytronMeteonisDADraconids);

            if (!hasDraconids) return false;

            // DA Draconids requires 5000 ATK tribute
            // Mu Beta with 2+ materials can tribute its materials directly
            var muBeta = _exec.Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Id == DrytronTourExecutor.CardId.DrytronMuBetaFafnir);
            int availableAtk = 0;

            if (muBeta != null)
            {
                availableAtk += muBeta.Overlays.Count * 2000;
            }

            var fieldMachines = _exec.Bot.GetMonsters().Where(m => m != muBeta && m.IsFaceup() && m.HasRace(CardRace.Machine) && !_exec.IsAceCard(m)).ToList();
            availableAtk += fieldMachines.Sum(m => m.Attack);

            var handMachines = _exec.Bot.Hand.Where(c => c.HasRace(CardRace.Machine) && c.Id != DrytronTourExecutor.CardId.DrytronMeteonisDADraconids).ToList();
            availableAtk += handMachines.Sum(c => c.Attack);

            return availableAtk >= 4000;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: DrytronMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronMaterialScorer
    {
        private readonly DrytronTourExecutor _exec;

        public DrytronMaterialScorer(DrytronTourExecutor exec)
        {
            _exec = exec;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: DrytronBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class DrytronBoardAssessor
    {
        private readonly DrytronTourExecutor _exec;

        public DrytronBoardAssessor(DrytronTourExecutor exec)
        {
            _exec = exec;
        }
    }
}
