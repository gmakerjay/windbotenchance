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
    //  MASTER DECK PLUGIN: CenturionPlugin (Layer 3 Domain Helpers)
    //  Decouples Domain Rules, Strategy, and Scorer from Engine Core
    // ═══════════════════════════════════════════════════════════════
    internal class CenturionPlugin
    {
        private readonly CenturionExecutor _exec;

        public CenturionStrategy Strategy { get; }
        public CenturionTimingAdvisor TimingAdvisor { get; }
        public CenturionResourceLoop ResourceLoop { get; }
        public CenturionMaterialScorer MaterialScorer { get; }
        public CenturionActionScorer ActionScorer { get; }
        public CenturionBoardAssessor BoardAssessor { get; }

        public CenturionPlugin(CenturionExecutor exec)
        {
            _exec = exec;
            Strategy = new CenturionStrategy(exec);
            TimingAdvisor = new CenturionTimingAdvisor(exec);
            ResourceLoop = new CenturionResourceLoop(exec);
            MaterialScorer = new CenturionMaterialScorer(exec);
            ActionScorer = new CenturionActionScorer(exec, this);
            BoardAssessor = new CenturionBoardAssessor(exec);
        }

        public void ResetTurnState()
        {
            Strategy.Reset();
            TimingAdvisor.Reset();
            ResourceLoop.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 1: CenturionStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class CenturionStrategy
    {
        private readonly CenturionExecutor _exec;

        public bool StandUpUsed { get; set; }
        public bool PrimeraNormalUsed { get; set; }
        public bool TrudeaUsed { get; set; }
        public bool GargoyleSSUsed { get; set; }
        public bool CrimsonDragonUsed { get; set; }
        public bool BlazarUsed { get; set; }
        public bool BondsUsed { get; set; }
        public bool WakeUpUsed { get; set; }
        public bool PhalanxUsed { get; set; }
        public bool TrueAwakeningUsed { get; set; }

        public CenturionStrategy(CenturionExecutor exec) => _exec = exec;

        public void Reset()
        {
            StandUpUsed = false;
            PrimeraNormalUsed = false;
            TrudeaUsed = false;
            GargoyleSSUsed = false;
            CrimsonDragonUsed = false;
            BlazarUsed = false;
            BondsUsed = false;
            WakeUpUsed = false;
            PhalanxUsed = false;
            TrueAwakeningUsed = false;
        }

        public bool HasFieldSpell()
        {
            return _exec.Bot.HasInSpellZone(CenturionExecutor.CardId.StandUpCenturIon);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 2: CenturionTimingAdvisor
    // ═══════════════════════════════════════════════════════════════
    internal class CenturionTimingAdvisor
    {
        private readonly CenturionExecutor _exec;
        private bool _blazarNegatedThisTurn = false;

        public CenturionTimingAdvisor(CenturionExecutor exec) => _exec = exec;

        public void Reset()
        {
            _blazarNegatedThisTurn = false;
        }

        public bool ShouldQuickSynchroOpponentTurn()
        {
            // Stand Up Quick Synchro triggers when opponent performs an action or Battle Phase begins
            if (_exec.Duel.Player != 1) return false;

            // If opponent starts Battle Phase -> must Synchro immediately to block lethal
            if (_exec.Duel.Phase == DuelPhase.BattleStart || _exec.Duel.Phase == DuelPhase.Battle)
                return true;

            // If opponent activated high-threat card or chokepoint
            ClientCard last = _exec.CurrentLastChainCard;
            if (last != null && last.Controller == 1)
            {
                if (CardIntelligence.IsHighThreatChokepoint(last.Id) || CardIntelligence.IsFloodgate(last.Id))
                    return true;
            }

            // If opponent has monster on board and we can summon Legatia to pop it
            if (_exec.Enemy.GetMonsterCount() > 0)
                return true;

            return true;
        }

        public bool ShouldTagOutCrimsonDragon()
        {
            if (_exec.Duel.Player != 1) return false;

            // Target Cosmic Blazar Dragon if available in Extra Deck
            bool hasBlazarInExtra = _exec.Bot.ExtraDeck.Any(c => c.Id == CenturionExecutor.CardId.CosmicBlazarDragon);
            if (!hasBlazarInExtra) return false;

            // Target Lv 12 Dragon on field (Legatia / Auxila)
            bool hasLv12Target = _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && 
                (m.Id == CenturionExecutor.CardId.CenturIonLegatia || m.Id == CenturionExecutor.CardId.CenturIonAuxila) &&
                m.Id != CenturionExecutor.CardId.CrimsonDragon);

            return hasLv12Target;
        }

        public bool ShouldBlazarNegate()
        {
            if (_blazarNegatedThisTurn) return false;

            // Only negate opponent actions (Controller == 1)
            ClientCard last = _exec.CurrentLastChainCard;
            if (last == null || last.Controller != 1)
            {
                // Check if opponent declared attack that threatens LP
                if (_exec.Duel.Phase == DuelPhase.Battle && _exec.Duel.Player == 1)
                {
                    _blazarNegatedThisTurn = true;
                    return true;
                }
                return false;
            }

            // High Threat Gate: Do not waste on low-priority bait
            if (CardIntelligence.IsHighThreatChokepoint(last.Id) ||
                CardIntelligence.IsFloodgate(last.Id) ||
                CardIntelligence.IsKnownNegator(last.Id) ||
                last.IsExtraCard() ||
                last.Attack >= 2500)
            {
                _blazarNegatedThisTurn = true;
                return true;
            }

            _blazarNegatedThisTurn = true;
            return true;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 3: CenturionResourceLoop
    // ═══════════════════════════════════════════════════════════════
    internal class CenturionResourceLoop
    {
        private readonly CenturionExecutor _exec;

        public CenturionResourceLoop(CenturionExecutor exec) => _exec = exec;

        public void Reset() { }

        public bool CanPlaceInSpellTrapZone()
        {
            // Ensure S/T zone is not full (must leave at least 1 open zone for Quick-Play/Counter Trap)
            return _exec.Bot.GetSpellCount() < 4;
        }

        public bool ShouldEndPhaseLoop(ClientCard card)
        {
            if (!CanPlaceInSpellTrapZone()) return false;
            return card.Id == CenturionExecutor.CardId.CenturIonPrimera || 
                   card.Id == CenturionExecutor.CardId.CenturIonTrudea;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 4: CenturionMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class CenturionMaterialScorer
    {
        private readonly CenturionExecutor _exec;

        public CenturionMaterialScorer(CenturionExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // Absolute Protection: Ace Synchro Bosses must never be sent away casually
            if (card.Id == CenturionExecutor.CardId.CosmicBlazarDragon) return 10000;
            if (card.Id == CenturionExecutor.CardId.CenturIonLegatia) return 9000;
            if (card.Id == CenturionExecutor.CardId.CenturIonAuxila) return 8500;
            if (card.Id == CenturionExecutor.CardId.RedSupernovaDragon) return 8000;
            if (card.Id == CenturionExecutor.CardId.TyPhon) return 7500;

            // Crimson Dragon is intended as tag-out bridge
            if (card.Id == CenturionExecutor.CardId.CrimsonDragon) return 200;

            // Synchro Fodder
            if (card.Id == CenturionExecutor.CardId.CenturIonGargoyleII) return 10;
            if (card.Id == CenturionExecutor.CardId.CenturIonEmethVI) return 15;
            if (card.Id == CenturionExecutor.CardId.CenturIonTrudea) return 20;
            if (card.Id == CenturionExecutor.CardId.CenturIonPrimera) return 25;

            return 100;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 5: CenturionActionScorer
    // ═══════════════════════════════════════════════════════════════
    internal class CenturionActionScorer
    {
        private readonly CenturionExecutor _exec;
        private readonly CenturionPlugin _plugin;

        public CenturionActionScorer(CenturionExecutor exec, CenturionPlugin plugin)
        {
            _exec = exec;
            _plugin = plugin;
        }

        public bool ShouldNormalSummonTrudea()
        {
            // Trudea is 1-card starter that places 2 cards in S/T zone
            if (_exec.Bot.HasInSpellZone(CenturionExecutor.CardId.StandUpCenturIon)) return true;
            return true;
        }

        public bool ShouldNormalSummonPrimera()
        {
            // Primera searches Stand-Up if missing
            return !_exec.Bot.HasInSpellZone(CenturionExecutor.CardId.StandUpCenturIon) || 
                   !_exec.Bot.Hand.Any(c => c.Id == CenturionExecutor.CardId.CenturIonTrudea);
        }

        public ClientCard PickStandUpDiscardTarget()
        {
            // Priority 1: Gargoyle II (triggers SS from GY)
            var gargoyle = _exec.Bot.Hand.FirstOrDefault(c => c.Id == CenturionExecutor.CardId.CenturIonGargoyleII);
            if (gargoyle != null) return gargoyle;

            // Priority 2: Duplicate spells / traps
            var duplicate = _exec.Bot.Hand.FirstOrDefault(c => _exec.Bot.Hand.Count(h => h.Id == c.Id) > 1 && !CardIntelligence.IsHandtrap(c.Id));
            if (duplicate != null) return duplicate;

            // Priority 3: Non-handtrap spells
            var spell = _exec.Bot.Hand.FirstOrDefault(c => c.IsSpell() && c.Id != CenturionExecutor.CardId.StandUpCenturIon);
            if (spell != null) return spell;

            return _exec.Bot.Hand.FirstOrDefault(c => !CardIntelligence.IsHandtrap(c.Id));
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 6: CenturionBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class CenturionBoardAssessor
    {
        private readonly CenturionExecutor _exec;

        public CenturionBoardAssessor(CenturionExecutor exec) => _exec = exec;

        public bool IsLethalAttackAvailable()
        {
            int totalAtk = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            return totalAtk >= _exec.Enemy.LifePoints && _exec.Enemy.GetMonsterCount() == 0;
        }
    }
}
