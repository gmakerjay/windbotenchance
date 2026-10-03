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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: RaiohPlugin
    //  Anti-Meta Stun & Absolute Floodgate Lockdown Strategy
    // ═══════════════════════════════════════════════════════════════
    public class RaiohPlugin : DeckPluginBase
    {
        private readonly RaiohExecutor _exec;

        public override string DeckName => "Raioh";

        public RaiohStrategy StrategyImpl { get; }
        public RaiohMaterialEvaluator MaterialImpl { get; }
        public RaiohThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public RaiohPlugin(RaiohExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new RaiohStrategy(exec);
            MaterialImpl = new RaiohMaterialEvaluator(exec);
            ThreatImpl = new RaiohThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class RaiohStrategy : IDeckStrategy
    {
        private readonly RaiohExecutor _exec;
        public bool MorganiteActive { get; set; } = false;

        public RaiohStrategy(RaiohExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Pot of Duality excavation priority
            // 1. Time-Tearing Morganite (Double Draw + Double Normal Summon is the key engine)
            if (!MorganiteActive)
            {
                var morganite = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.TimeTearingMorganite);
                if (morganite != null) return morganite;
            }

            // 2. Moon Mirror Shield (Guarantees battle victory for our floodgates)
            bool hasMonster = _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup());
            bool hasEquip = _exec.Bot.HasInHand(RaiohExecutor.CardId.MoonMirrorShield) ||
                            _exec.Bot.GetSpells().Any(s => s != null && s.Id == RaiohExecutor.CardId.MoonMirrorShield);
            if (hasMonster && !hasEquip)
            {
                var shield = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.MoonMirrorShield);
                if (shield != null) return shield;
            }

            // 3. Floodgate Monster if we have none on field
            if (_exec.Bot.GetMonsterCount() == 0)
            {
                var boarder = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.InspectorBoarder);
                if (boarder != null) return boarder;

                var raioh = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.ThunderKingRaiOh);
                if (raioh != null) return raioh;

                var statue = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.BarrierStatueOfTheHeavens);
                if (statue != null) return statue;

                var banisher = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.BanisherOfTheRadiance);
                if (banisher != null) return banisher;
            }

            // 4. Backrow Protection: Solemn Judgment (intercepts Feather Duster / Lightning Storm)
            var judgment = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.SolemnJudgment);
            if (judgment != null) return judgment;

            // 5. Mass Disruption: Destructive Daruma Karma Cannon
            var daruma = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.DestructiveDarumaKarmaCannon);
            if (daruma != null) return daruma;

            // 6. Targeted Negation & Floodgates
            var strike = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.SolemnStrike);
            if (strike != null) return strike;

            var warning = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.SolemnWarning);
            if (warning != null) return warning;

            var skillDrain = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.SkillDrain);
            if (skillDrain != null && !_exec.Bot.HasInSpellZone(RaiohExecutor.CardId.SkillDrain)) return skillDrain;

            var antiSpell = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.AntiSpellFragrance);
            if (antiSpell != null && !_exec.Bot.HasInSpellZone(RaiohExecutor.CardId.AntiSpellFragrance)) return antiSpell;

            var macro = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.MacroCosmos);
            if (macro != null && !_exec.Bot.HasInSpellZone(RaiohExecutor.CardId.MacroCosmos)) return macro;

            var tcboo = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.ThereCanBeOnlyOne);
            if (tcboo != null && !_exec.Bot.HasInSpellZone(RaiohExecutor.CardId.ThereCanBeOnlyOne)) return tcboo;

            var crackdown = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.Crackdown);
            if (crackdown != null) return crackdown;

            var necrovalley = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.Necrovalley);
            if (necrovalley != null && !_exec.Bot.HasInSpellZone(RaiohExecutor.CardId.Necrovalley)) return necrovalley;

            var extrav = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.PotOfExtravagance);
            if (extrav != null) return extrav;

            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.FirstOrDefault(c => c != null);
        }

        public ClientCard PickFoolishGraveTarget(IList<ClientCard> candidates, ClientCard context)
        {
            return null;
        }

        public ClientCard PickEquipTarget(IList<ClientCard> targets)
        {
            if (targets == null || targets.Count == 0) return null;

            // Prioritize equipping monsters without an existing Moon Mirror Shield
            var unequipped = targets.Where(m => m != null && m.IsFaceup() &&
                (m.EquipCards == null || !m.EquipCards.Any(e => e.Id == RaiohExecutor.CardId.MoonMirrorShield))).ToList();

            var pool = unequipped.Count > 0 ? unequipped : targets.Where(m => m != null && m.IsFaceup()).ToList();
            if (pool.Count == 0) return targets.FirstOrDefault();

            // Barrier Statue (1000 ATK - most vulnerable to battle)
            var statue = pool.FirstOrDefault(m => m.Id == RaiohExecutor.CardId.BarrierStatueOfTheHeavens);
            if (statue != null) return statue;

            // Thunder King Rai-Oh (1900 ATK)
            var raioh = pool.FirstOrDefault(m => m.Id == RaiohExecutor.CardId.ThunderKingRaiOh);
            if (raioh != null) return raioh;

            // Banisher of the Radiance (1600 ATK)
            var banisher = pool.FirstOrDefault(m => m.Id == RaiohExecutor.CardId.BanisherOfTheRadiance);
            if (banisher != null) return banisher;

            // Inspector Boarder (2000 ATK)
            var boarder = pool.FirstOrDefault(m => m.Id == RaiohExecutor.CardId.InspectorBoarder);
            if (boarder != null) return boarder;

            return pool.OrderBy(m => m.Attack).FirstOrDefault();
        }
    }

    public class RaiohMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly RaiohExecutor _exec;
        public RaiohMaterialEvaluator(RaiohExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Never sacrifice or use a monster equipped with Moon Mirror Shield
            if (card.EquipCards != null && card.EquipCards.Any(e => e.Id == RaiohExecutor.CardId.MoonMirrorShield))
                return 50000;

            if (_exec.IsAceCard(card))
                return 20000;

            // Stun monsters on field are critical lockdown assets
            if (card.Location == CardLocation.MonsterZone && card.IsFaceup())
            {
                if (card.Id == RaiohExecutor.CardId.InspectorBoarder) return 8000;
                if (card.Id == RaiohExecutor.CardId.BarrierStatueOfTheHeavens) return 7500;
                if (card.Id == RaiohExecutor.CardId.ThunderKingRaiOh) return 7000;
                if (card.Id == RaiohExecutor.CardId.BanisherOfTheRadiance) return 6500;
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

            // Duplicate Morganite is prime discard fodder
            var duplicateMorganite = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.TimeTearingMorganite &&
                _exec.Bot.HasInHand(RaiohExecutor.CardId.TimeTearingMorganite));
            if (duplicateMorganite != null) return duplicateMorganite;

            // Duplicate Field Spell
            var duplicateField = candidates.FirstOrDefault(c => c != null && c.Id == RaiohExecutor.CardId.Necrovalley &&
                _exec.Bot.HasInSpellZone(RaiohExecutor.CardId.Necrovalley));
            if (duplicateField != null) return duplicateField;

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class RaiohThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly RaiohExecutor _exec;
        public RaiohThreatEvaluator(RaiohExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;
            int score = 0;

            int id = card.Id;
            // Extreme Threat: Mass Backrow Removals (Solemn Judgment must intercept immediately)
            if (id == 18144506 || id == 12580477 || id == 15693423 || id == 72302403 || id == 43898403 || id == 14532163 || id == 53129443)
                score += 20000; // Harpie's Feather Duster, Raigeki, Evenly Matched, Swords of Concealing, Twin Twisters, Lightning Storm, Dark Hole

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
            return id == 18144506 || id == 12580477 || id == 15693423 || id == 14532163 || id == 53129443 || card.Attack >= 3000;
        }
    }
}
