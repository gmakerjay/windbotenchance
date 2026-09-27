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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: AntiMetaPlugin
    //  6-Dimensional Lockdown & Omni-Disruption Strategy
    // ═══════════════════════════════════════════════════════════════
    public class AntiMetaPlugin : DeckPluginBase
    {
        private readonly AntiMetaExecutor _exec;

        public override string DeckName => "AntiMeta";

        public AntiMetaStrategy StrategyImpl { get; }
        public AntiMetaMaterialEvaluator MaterialImpl { get; }
        public AntiMetaThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public AntiMetaPlugin(AntiMetaExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new AntiMetaStrategy(exec);
            MaterialImpl = new AntiMetaMaterialEvaluator(exec);
            ThreatImpl = new AntiMetaThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class AntiMetaStrategy : IDeckStrategy
    {
        private readonly AntiMetaExecutor _exec;
        public bool MorganiteActive { get; set; } = false;

        public AntiMetaStrategy(AntiMetaExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Pot of Duality selection priority
            // 1. Time-Tearing Morganite (if not active yet)
            if (!MorganiteActive)
            {
                var morganite = candidates.FirstOrDefault(c => c != null && c.Id == AntiMetaExecutor.CardId.TimeTearingMorganite);
                if (morganite != null) return morganite;
            }

            // 2. Moon Mirror Shield (if we have monster but no equip)
            bool hasMonster = _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup());
            bool hasEquip = _exec.Bot.HasInHand(AntiMetaExecutor.CardId.MoonMirrorShield) ||
                            _exec.Bot.GetSpells().Any(s => s != null && s.Id == AntiMetaExecutor.CardId.MoonMirrorShield);
            if (hasMonster && !hasEquip)
            {
                var shield = candidates.FirstOrDefault(c => c != null && c.Id == AntiMetaExecutor.CardId.MoonMirrorShield);
                if (shield != null) return shield;
            }

            // 3. Floodgate Monster based on enemy field
            if (_exec.Bot.GetMonsterCount() == 0)
            {
                var statue = candidates.FirstOrDefault(c => c != null && c.Id == AntiMetaExecutor.CardId.BarrierStatueOfTheHeavens);
                if (statue != null) return statue;

                var boarder = candidates.FirstOrDefault(c => c != null && c.Id == AntiMetaExecutor.CardId.InspectorBoarder);
                if (boarder != null) return boarder;

                var raioh = candidates.FirstOrDefault(c => c != null && c.Id == AntiMetaExecutor.CardId.ThunderKingRaiOh);
                if (raioh != null) return raioh;

                var banisher = candidates.FirstOrDefault(c => c != null && c.Id == AntiMetaExecutor.CardId.BanisherOfTheRadiance);
                if (banisher != null) return banisher;
            }

            // 4. Protection & Wipe Traps
            var judgment = candidates.FirstOrDefault(c => c != null && c.Id == AntiMetaExecutor.CardId.SolemnJudgment);
            if (judgment != null) return judgment;

            var daruma = candidates.FirstOrDefault(c => c != null && c.Id == AntiMetaExecutor.CardId.DestructiveDarumaKarmaCannon);
            if (daruma != null) return daruma;

            var strike = candidates.FirstOrDefault(c => c != null && c.Id == AntiMetaExecutor.CardId.SolemnStrike);
            if (strike != null) return strike;

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.FirstOrDefault(c => c != null);
        }
    }

    public class AntiMetaMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly AntiMetaExecutor _exec;
        public AntiMetaMaterialEvaluator(AntiMetaExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Never sacrifice or use a monster equipped with Moon Mirror Shield
            if (card.EquipCards != null && card.EquipCards.Count > 0)
                return 50000;

            if (_exec.IsAceCard(card))
                return 20000;

            // Stun monsters on field are critical assets
            if (card.Location == CardLocation.MonsterZone && card.IsFaceup())
            {
                if (card.Id == AntiMetaExecutor.CardId.InspectorBoarder) return 8000;
                if (card.Id == AntiMetaExecutor.CardId.BarrierStatueOfTheHeavens) return 7500;
                if (card.Id == AntiMetaExecutor.CardId.ThunderKingRaiOh) return 7000;
                if (card.Id == AntiMetaExecutor.CardId.BanisherOfTheRadiance) return 6500;
            }

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

            // Duplicate spells or conditional traps can be discarded
            var duplicateMorganite = candidates.FirstOrDefault(c => c != null && c.Id == AntiMetaExecutor.CardId.TimeTearingMorganite &&
                _exec.Bot.HasInHand(AntiMetaExecutor.CardId.TimeTearingMorganite));
            if (duplicateMorganite != null) return duplicateMorganite;

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class AntiMetaThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly AntiMetaExecutor _exec;
        public AntiMetaThreatEvaluator(AntiMetaExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;
            int score = 0;

            // Extreme Threat: Mass Backrow Removals (Solemn Judgment must intercept)
            int id = card.Id;
            if (id == 18144506 || id == 12580477 || id == 15693423 || id == 72302403 || id == 43898403)
                score += 15000; // Harpie's Feather Duster, Raigeki, Evenly Matched, Swords of Concealing, Twin Twisters

            if (CardIntelligence.IsFloodgate(id)) score += 8000;
            if (CardIntelligence.IsKnownNegator(id)) score += 7000;

            // Link Monsters are prime targets for Daruma Cannon (cannot flip face-down -> sent to GY)
            if (card.HasType(CardType.Link)) score += 5000;

            if (card.Attack >= 2500) score += 3000;

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            int id = card.Id;
            // Backrow removal or 3000+ ATK
            return id == 18144506 || id == 12580477 || id == 15693423 || card.Attack >= 3000;
        }
    }
}
