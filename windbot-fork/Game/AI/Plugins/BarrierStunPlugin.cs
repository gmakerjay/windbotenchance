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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: BarrierStunPlugin
    //  Elemental Barrier Lock, Graveyard Prohibition & Anti-Meta Stun
    // ═══════════════════════════════════════════════════════════════
    public class BarrierStunPlugin : DeckPluginBase
    {
        private readonly BarrierStunExecutor _exec;

        public override string DeckName => "BarrierStun";

        public BarrierStunStrategy StrategyImpl { get; }
        public BarrierStunMaterialEvaluator MaterialImpl { get; }
        public BarrierStunThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public BarrierStunPlugin(BarrierStunExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new BarrierStunStrategy(exec);
            MaterialImpl = new BarrierStunMaterialEvaluator(exec);
            ThreatImpl = new BarrierStunThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class BarrierStunStrategy : IDeckStrategy
    {
        private readonly BarrierStunExecutor _exec;
        public bool MorganiteActive { get; set; } = false;

        public BarrierStunStrategy(BarrierStunExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            // We rarely/never special summon, but if Extra Deck / Monster Reborn resolves:
            return candidates?.FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Pot of Duality excavation priority
            // 1. Time-Tearing Morganite (Double Draw + Double Normal Summon is our core engine)
            if (!MorganiteActive && !_exec.Bot.HasInHand(BarrierStunExecutor.CardId.TimeTearingMorganite))
            {
                var morganite = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.TimeTearingMorganite);
                if (morganite != null) return morganite;
            }

            // 2. Moon Mirror Shield (Guarantees battle victory for 1000-1800 ATK stun monsters)
            bool hasMonster = _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup());
            bool hasEquip = _exec.Bot.HasInHand(BarrierStunExecutor.CardId.MoonMirrorShield) ||
                            _exec.Bot.GetSpells().Any(s => s != null && s.Id == BarrierStunExecutor.CardId.MoonMirrorShield);
            if (hasMonster && !hasEquip)
            {
                var shield = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.MoonMirrorShield);
                if (shield != null) return shield;
            }

            // 3. Stun Monster if we lack one
            if (!hasMonster && !_exec.Bot.Hand.Any(c => c != null && BarrierStunExecutor.StunMonsters.Contains(c.Id)))
            {
                var boarder = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.InspectorBoarder);
                if (boarder != null) return boarder;

                var abyss = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.BarrierStatueOfTheAbyss);
                if (abyss != null) return abyss;

                var heavens = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.BarrierStatueOfTheHeavens);
                if (heavens != null) return heavens;

                var kycoo = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.KycooTheGhostDestroyer);
                if (kycoo != null) return kycoo;
            }

            // 4. Counter Traps (Omni-Protection & Summon Cancellation)
            var judgment = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.SolemnJudgment);
            if (judgment != null) return judgment;

            var strike = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.SolemnStrike);
            if (strike != null) return strike;

            var warning = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.SolemnWarning);
            if (warning != null) return warning;

            // 5. Board Interaction & Mass Book
            var daruma = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.DestructiveDarumaKarmaCannon);
            if (daruma != null) return daruma;

            var crackdown = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.Crackdown);
            if (crackdown != null) return crackdown;

            // 6. Continuous Floodgates
            var antiSpell = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.AntiSpellFragrance);
            if (antiSpell != null && !_exec.Bot.HasInSpellZone(BarrierStunExecutor.CardId.AntiSpellFragrance)) return antiSpell;

            var skillDrain = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.SkillDrain);
            if (skillDrain != null && !_exec.Bot.HasInSpellZone(BarrierStunExecutor.CardId.SkillDrain)) return skillDrain;

            var macro = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.MacroCosmos);
            if (macro != null && !_exec.Bot.HasInSpellZone(BarrierStunExecutor.CardId.MacroCosmos)) return macro;

            var necro = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.Necrovalley);
            if (necro != null && !_exec.Bot.HasInSpellZone(BarrierStunExecutor.CardId.Necrovalley)) return necro;

            var fissure = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.DimensionalFissure);
            if (fissure != null && !_exec.Bot.HasInSpellZone(BarrierStunExecutor.CardId.DimensionalFissure)) return fissure;

            return candidates.FirstOrDefault();
        }

        public ClientCard PickNormalSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Inspector Boarder if we have none on field
            if (!_exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == BarrierStunExecutor.CardId.InspectorBoarder))
            {
                var boarder = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.InspectorBoarder);
                if (boarder != null) return boarder;
            }

            // 2. Barrier Statue of the Abyss (Blocks non-DARK Special Summons - shuts down BlueEyes, ABC, Altergeist, etc.)
            if (!_exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == BarrierStunExecutor.CardId.BarrierStatueOfTheAbyss))
            {
                var abyss = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.BarrierStatueOfTheAbyss);
                if (abyss != null) return abyss;
            }

            // 3. Barrier Statue of the Heavens (Blocks non-LIGHT Special Summons)
            if (!_exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Id == BarrierStunExecutor.CardId.BarrierStatueOfTheHeavens))
            {
                var heavens = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.BarrierStatueOfTheHeavens);
                if (heavens != null) return heavens;
            }

            // 4. Kycoo the Ghost Destroyer (Blocks banishing from GY, deal damage banishes 2 GY monsters)
            var kycoo = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.KycooTheGhostDestroyer);
            if (kycoo != null) return kycoo;

            return candidates.FirstOrDefault(c => c != null && BarrierStunExecutor.StunMonsters.Contains(c.Id));
        }

        public ClientCard PickEquipTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Prioritize unequipped monsters in our control
            var botMonsters = candidates.Where(c => c != null && c.Controller == 0 && c.IsFaceup()).ToList();
            if (botMonsters.Count == 0) return candidates.FirstOrDefault();

            // Give shield to unequipped monsters first
            var unequipped = botMonsters.Where(c => c.EquipCards == null || c.EquipCards.Count == 0).ToList();
            var targetPool = unequipped.Count > 0 ? unequipped : botMonsters;

            // Target Priority: Statues (low 1000 ATK) > Kycoo (1800 ATK) > Boarder (2000 ATK)
            var abyss = targetPool.FirstOrDefault(c => c.Id == BarrierStunExecutor.CardId.BarrierStatueOfTheAbyss);
            if (abyss != null) return abyss;

            var heavens = targetPool.FirstOrDefault(c => c.Id == BarrierStunExecutor.CardId.BarrierStatueOfTheHeavens);
            if (heavens != null) return heavens;

            var kycoo = targetPool.FirstOrDefault(c => c.Id == BarrierStunExecutor.CardId.KycooTheGhostDestroyer);
            if (kycoo != null) return kycoo;

            var boarder = targetPool.FirstOrDefault(c => c.Id == BarrierStunExecutor.CardId.InspectorBoarder);
            if (boarder != null) return boarder;

            return targetPool.FirstOrDefault();
        }
    }

    public class BarrierStunMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly BarrierStunExecutor _exec;
        public BarrierStunMaterialEvaluator(BarrierStunExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // Never sacrifice or use a monster equipped with Moon Mirror Shield
            if (card.EquipCards != null && card.EquipCards.Any(e => e.Id == BarrierStunExecutor.CardId.MoonMirrorShield))
                return 50000;

            if (_exec.IsAceCard(card))
                return 20000;

            // Stun monsters on field are critical lockdown assets
            if (card.Location == CardLocation.MonsterZone && card.IsFaceup())
            {
                if (card.Id == BarrierStunExecutor.CardId.InspectorBoarder) return 8000;
                if (card.Id == BarrierStunExecutor.CardId.BarrierStatueOfTheAbyss) return 7500;
                if (card.Id == BarrierStunExecutor.CardId.BarrierStatueOfTheHeavens) return 7000;
                if (card.Id == BarrierStunExecutor.CardId.KycooTheGhostDestroyer) return 6500;
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
            var duplicateMorganite = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.TimeTearingMorganite &&
                _exec.Bot.HasInHand(BarrierStunExecutor.CardId.TimeTearingMorganite));
            if (duplicateMorganite != null) return duplicateMorganite;

            // Duplicate Field Spell
            var duplicateField = candidates.FirstOrDefault(c => c != null && c.Id == BarrierStunExecutor.CardId.Necrovalley &&
                _exec.Bot.HasInSpellZone(BarrierStunExecutor.CardId.Necrovalley));
            if (duplicateField != null) return duplicateField;

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class BarrierStunThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly BarrierStunExecutor _exec;
        public BarrierStunThreatEvaluator(BarrierStunExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;
            int score = 0;

            int id = card.Id;
            // Extreme Threat: Mass Backrow Removals (Solemn Judgment must intercept immediately)
            if (id == 18144506 || id == 12580477 || id == 15693423 || id == 72302403 || id == 43898403 || id == 14532163 || id == 53129443)
                score += 20000;

            if (CardIntelligence.IsFloodgate(id)) score += 8000;
            if (CardIntelligence.IsKnownNegator(id)) score += 7000;

            if (card.HasType(CardType.Link)) score += 5000;
            if (card.Attack >= 2500) score += 3000;

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            int id = card.Id;
            return id == 18144506 || id == 12580477 || id == 15693423 || id == 14532163 || id == 53129443 || card.Attack >= 3000;
        }
    }
}
