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
    // ═══════════════════════════════════════════════════════════════════════════
    // DECOUPLED DOMAIN PLUGIN ARCHITECTURE: LairOfDarknessPlugin
    // Implements Strategy, MaterialEvaluator, and ThreatEvaluator for Lair of Darkness
    // ═══════════════════════════════════════════════════════════════════════════
    public class LairOfDarknessPlugin : DeckPluginBase
    {
        private readonly LairOfDarknessExecutor _exec;

        public override string DeckName => "LairOfDarkness";

        public LairOfDarknessStrategy StrategyImpl { get; }
        public LairOfDarknessMaterialEvaluator MaterialImpl { get; }
        public LairOfDarknessThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public LairOfDarknessPlugin(LairOfDarknessExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new LairOfDarknessStrategy(exec);
            MaterialImpl = new LairOfDarknessMaterialEvaluator(exec);
            ThreatImpl = new LairOfDarknessThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class LairOfDarknessStrategy : IDeckStrategy
    {
        private readonly LairOfDarknessExecutor _exec;
        public LairOfDarknessStrategy(LairOfDarknessExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Darkest Diabolos from Hand/GY
            var diabolos = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.DarkestDiabolos);
            if (diabolos != null) return diabolos;

            // 2. Lord of the Heavenly Prison
            var lord = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.LordOfTheHeavenlyPrison);
            if (lord != null) return lord;

            // 3. Super Poly Fusion bosses
            var starving = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.StarvingVenomFusionDragon);
            if (starving != null) return starving;

            var mudragon = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.MudragonOfTheSwamp);
            if (mudragon != null) return mudragon;

            var garura = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.GaruraWingsOfResonantLife);
            if (garura != null) return garura;

            var dragostapelia = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.PredaplantDragostapelia);
            if (dragostapelia != null) return dragostapelia;

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard contextCard)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Ahrima search: Lair of Darkness if not on field/hand
            if (!_exec.Bot.HasInSpellZone(LairOfDarknessExecutor.CardId.LairOfDarkness) &&
                !_exec.Bot.HasInHand(LairOfDarknessExecutor.CardId.LairOfDarkness))
            {
                var lair = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.LairOfDarkness);
                if (lair != null) return lair;
            }

            // 2. Ahrima on-field tribute search: Darkest Diabolos (Level 8 DARK with 2000 DEF)
            var diabolos = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.DarkestDiabolos);
            if (diabolos != null && !_exec.Bot.HasInHand(LairOfDarknessExecutor.CardId.DarkestDiabolos))
                return diabolos;

            // 3. Lilith search: Trap Trick x3 guarantee or Virus
            var trapTrick = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.TrapTrick);
            if (trapTrick != null) return trapTrick;

            var eev = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.EradicatorEpidemicVirus);
            if (eev != null) return eev;

            var ddv = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.DeckDevastationVirus);
            if (ddv != null) return ddv;

            var idp = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.IceDragonsPrison);
            if (idp != null) return idp;

            var trio = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.OjamaTrio);
            if (trio != null) return trio;

            return candidates.FirstOrDefault();
        }
    }

    public class LairOfDarknessMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly LairOfDarknessExecutor _exec;
        public LairOfDarknessMaterialEvaluator(LairOfDarknessExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard c)
        {
            if (c == null) return 0;

            // If card is on opponent's field: ALWAYS prioritize tributing opponent's cards first! (Negative cost)
            if (c.Controller == 1)
            {
                return -100 - _exec.Plugin.ThreatImpl.EvaluateThreatScore(c);
            }

            // Torment Tokens: Zero cost
            if (c.HasType(CardType.Token) || c.Id == LairOfDarknessExecutor.CardId.TormentToken)
                return 1;

            // Malice / Lilith after using effect: Low cost
            if (c.Id == LairOfDarknessExecutor.CardId.LilithLadyOfLament || c.Id == LairOfDarknessExecutor.CardId.MaliceLadyOfLament)
                return 3;

            // Ahrima on field: Medium cost
            if (c.Id == LairOfDarknessExecutor.CardId.AhrimaTheWickedWarden)
                return 4;

            // Darkest Diabolos: High cost on field (3000 ATK boss), but if sent to GY can revive!
            if (c.Id == LairOfDarknessExecutor.CardId.DarkestDiabolos)
                return 20;

            // Lord of the Heavenly Prison: High cost
            if (c.Id == LairOfDarknessExecutor.CardId.LordOfTheHeavenlyPrison)
                return 50;

            // Extra Deck bosses: High cost
            if (c.Id == LairOfDarknessExecutor.CardId.StarvingVenomFusionDragon ||
                c.Id == LairOfDarknessExecutor.CardId.PredaplantDragostapelia ||
                c.Id == LairOfDarknessExecutor.CardId.AccesscodeTalker)
                return 100;

            return 10;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Darkest Diabolos (Floats from GY to field when a DARK monster is tributed!)
            var diabolos = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.DarkestDiabolos);
            if (diabolos != null) return diabolos;

            // 2. Duplicate Lair of Darkness
            var duplicateLair = candidates.FirstOrDefault(c => c.Id == LairOfDarknessExecutor.CardId.LairOfDarkness &&
                _exec.Bot.HasInSpellZone(LairOfDarknessExecutor.CardId.LairOfDarkness));
            if (duplicateLair != null) return duplicateLair;

            // 3. Normal Traps (Can be set back by Malice from GY!)
            var trap = candidates.FirstOrDefault(c => c.HasType(CardType.Trap));
            if (trap != null) return trap;

            return candidates.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates?.OrderBy(GetMaterialCost).FirstOrDefault();
        }

        /// <summary>
        /// Select best tribute target under Lair of Darkness.
        /// If Lair is active and opponent has face-up monsters, tributes the most threatening opponent monster!
        /// </summary>
        public ClientCard PickTributeTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Opponent monster under Lair of Darkness
            var oppMonster = candidates.Where(c => c.Controller == 1)
                .OrderByDescending(c => _exec.Plugin.ThreatImpl.EvaluateThreatScore(c))
                .FirstOrDefault();
            if (oppMonster != null) return oppMonster;

            // 2. Torment Token on our field
            var token = candidates.FirstOrDefault(c => c.Controller == 0 && c.HasType(CardType.Token));
            if (token != null) return token;

            // 3. Lowest cost friendly monster
            return candidates.Where(c => c.Controller == 0).OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class LairOfDarknessThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly LairOfDarknessExecutor _exec;
        public LairOfDarknessThreatEvaluator(LairOfDarknessExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard c)
        {
            if (c == null) return 0;
            int score = 0;

            if (c.Id == 18144506 || c.Id == 14532163 || c.Id == 15693423) score += 90;
            if (c.Id == 82732047 || c.Id == 82732705 || c.Id == 30241314) score += 85;

            if (c.IsFaceup() && c.HasType(CardType.Monster))
            {
                if (c.Attack >= 3000) score += 30;
                if (CardIntelligence.IsKnownNegator(c.Id)) score += 50;
                if (CardIntelligence.IsHighThreatChokepoint(c.Id)) score += 40;
            }

            return score;
        }

        public bool IsEmergencyThreat(ClientCard c)
        {
            if (c == null) return false;
            return EvaluateThreatScore(c) >= 70;
        }
    }
}
