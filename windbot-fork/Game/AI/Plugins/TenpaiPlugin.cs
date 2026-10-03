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
    // DECOUPLED DOMAIN PLUGIN ARCHITECTURE: TenpaiPlugin
    // Implements Strategy, MaterialEvaluator, ThreatEvaluator & OTKPlanner
    // ═══════════════════════════════════════════════════════════════════════════
    public class TenpaiPlugin : DeckPluginBase
    {
        private readonly TenpaiExecutor _exec;

        public override string DeckName => "Tenpai";

        public TenpaiStrategy StrategyImpl { get; }
        public TenpaiMaterialEvaluator MaterialImpl { get; }
        public TenpaiThreatEvaluator ThreatImpl { get; }
        public TenpaiBattleOTKPlanner OTKPlanner { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public TenpaiPlugin(TenpaiExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new TenpaiStrategy(exec);
            MaterialImpl = new TenpaiMaterialEvaluator(exec);
            ThreatImpl = new TenpaiThreatEvaluator(exec);
            OTKPlanner = new TenpaiBattleOTKPlanner(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // STRATEGY: Domain combo paths, search priorities, and summon sequencing
    // ═══════════════════════════════════════════════════════════════════════════
    public class TenpaiStrategy : IDeckStrategy
    {
        private readonly TenpaiExecutor _exec;

        public TenpaiStrategy(TenpaiExecutor exec)
        {
            _exec = exec;
        }

        public void Reset()
        {
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. If reviving via Bident Dragion (Level 7 Tuner):
            // Priority: Paidra (Level 3 non-tuner) to enable 7 + 3 = 10 Synchro (Trident or Transcendent)!
            if (candidates.Any(c => c.Location == CardLocation.Grave))
            {
                var paidra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonPaidra);
                if (paidra != null) return paidra;

                var fadra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonFadra);
                if (fadra != null) return fadra;

                var chundra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonChundra);
                if (chundra != null) return chundra;
            }

            // 2. If Special Summoning from Deck (Chundra at Battle Start or Genroku tribute):
            if (candidates.Any(c => c.Location == CardLocation.Deck))
            {
                // If we don't have Fadra on field, get Fadra (Fadra immediately revives another dragon from GY!)
                if (!_exec.Bot.HasInMonstersZone(TenpaiExecutor.CardId.TenpaiDragonFadra))
                {
                    var fadra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonFadra);
                    if (fadra != null) return fadra;
                }

                // If we need Tuner, get Chundra
                if (!_exec.Bot.HasInMonstersZone(TenpaiExecutor.CardId.TenpaiDragonChundra))
                {
                    var chundra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonChundra);
                    if (chundra != null) return chundra;
                }

                // Else Paidra
                var paidra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonPaidra);
                if (paidra != null) return paidra;
            }

            // Fallback: highest ATK
            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Context: Paidra searching Sangen Spell/Trap
            if (context != null && context.Id == TenpaiExecutor.CardId.TenpaiDragonPaidra)
            {
                // Priority 1: Sangen Summoning (Field Spell) - provides complete blanket MP1 immunity
                if (!_exec.Bot.HasInSpellZone(TenpaiExecutor.CardId.SangenSummoning) && !_exec.Bot.HasInHand(TenpaiExecutor.CardId.SangenSummoning))
                {
                    var field = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.SangenSummoning);
                    if (field != null) return field;
                }

                // Priority 2: Sangen Kaimen (Quick-Play searcher / swarm)
                var kaimen = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.SangenKaimen);
                if (kaimen != null && !_exec.Bot.HasInHand(TenpaiExecutor.CardId.SangenKaimen)) return kaimen;

                // Priority 3: Sangen Kaiho (Trap) - especially if Going First
                var kaiho = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.SangenKaiho);
                if (kaiho != null) return kaiho;

                // Priority 4: Sangen Furo (Continuous Spell)
                var furo = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.SangenFuro);
                if (furo != null) return furo;
            }

            // Context: Sangen Summoning searching Tenpai Monster
            if (context != null && context.Id == TenpaiExecutor.CardId.SangenSummoning)
            {
                // If no Tuner, search Chundra (Level 4 Tuner)
                if (!_exec.Bot.HasInHand(TenpaiExecutor.CardId.TenpaiDragonChundra) && !_exec.Bot.HasInMonstersZone(TenpaiExecutor.CardId.TenpaiDragonChundra))
                {
                    var chundra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonChundra);
                    if (chundra != null) return chundra;
                }

                // If no Paidra, search Paidra
                if (!_exec.Bot.HasInHand(TenpaiExecutor.CardId.TenpaiDragonPaidra) && !_exec.Bot.HasInMonstersZone(TenpaiExecutor.CardId.TenpaiDragonPaidra))
                {
                    var paidra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonPaidra);
                    if (paidra != null) return paidra;
                }

                // Search Genroku (Extender: can Special Summon self when added to hand)
                var genroku = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonGenroku);
                if (genroku != null) return genroku;

                // Fallback: Fadra
                var fadra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonFadra);
                if (fadra != null) return fadra;
            }

            // Context: Sangen Kaimen searching Level 4 or lower FIRE Dragon
            if (context != null && context.Id == TenpaiExecutor.CardId.SangenKaimen)
            {
                if (!_exec.Bot.HasInHand(TenpaiExecutor.CardId.TenpaiDragonChundra) && !_exec.Bot.HasInMonstersZone(TenpaiExecutor.CardId.TenpaiDragonChundra))
                {
                    var chundra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonChundra);
                    if (chundra != null) return chundra;
                }
                if (!_exec.Bot.HasInHand(TenpaiExecutor.CardId.TenpaiDragonPaidra) && !_exec.Bot.HasInMonstersZone(TenpaiExecutor.CardId.TenpaiDragonPaidra))
                {
                    var paidra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonPaidra);
                    if (paidra != null) return paidra;
                }
                var genroku = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonGenroku);
                if (genroku != null) return genroku;
            }

            return candidates.FirstOrDefault();
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // MATERIAL EVALUATOR: Discard priorities, destruction targets for Trident
    // ═══════════════════════════════════════════════════════════════════════════
    public class TenpaiMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly TenpaiExecutor _exec;

        public TenpaiMaterialEvaluator(TenpaiExecutor exec)
        {
            _exec = exec;
        }

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == TenpaiExecutor.CardId.TridentDragion) return 10000;
            if (card.Id == TenpaiExecutor.CardId.SangenpaiTranscendentDragion) return 8500;
            if (card.Id == TenpaiExecutor.CardId.SangenpaiBidentDragion) return 5000;
            if (card.Id == TenpaiExecutor.CardId.TenpaiDragonChundra) return 2500;
            if (card.Id == TenpaiExecutor.CardId.TenpaiDragonPaidra) return 2000;
            if (card.Id == TenpaiExecutor.CardId.TenpaiDragonFadra) return 1500;
            if (card.Id == TenpaiExecutor.CardId.TenpaiDragonGenroku) return 800;
            return 1000;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(GetMaterialCost).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Priority 1: Sangen Kaiho (has powerful GY effect to banish and draw 1 + SS dragons)
            var kaiho = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.SangenKaiho);
            if (kaiho != null) return kaiho;

            // Priority 2: Duplicate Spell cards
            var duplicateSpells = candidates.Where(c => c.IsSpell() && candidates.Count(x => x.Id == c.Id) > 1).FirstOrDefault();
            if (duplicateSpells != null) return duplicateSpells;

            // Priority 3: Dead Going-Second cards if board already broken (Dark Ruler, Lightning Storm)
            var deadStorm = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.LightningStorm && (_exec.Bot.GetMonsterCount() > 0 || _exec.Bot.GetSpellCount() > 0));
            if (deadStorm != null) return deadStorm;

            // Priority 4: Sangen Furo
            var furo = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.SangenFuro);
            if (furo != null) return furo;

            // Priority 5: Fadra (revivable by Bident or Chundra)
            var fadra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonFadra);
            if (fadra != null) return fadra;

            // Fallback: lowest material cost that is NOT Sangen Summoning or Chundra
            return candidates
                .Where(c => c.Id != TenpaiExecutor.CardId.SangenSummoning && c.Id != TenpaiExecutor.CardId.TenpaiDragonChundra)
                .OrderBy(GetMaterialCost)
                .FirstOrDefault() ?? candidates.First();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // For Trident Dragion (Destroy up to 2 friendly cards to attack 3 times):
            // CRITICAL: Destroying Sangen Summoning in Battle Phase triggers its effect to DOUBLE Trident Dragion's ATK to 6000!
            var sangenSummoning = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.SangenSummoning);
            if (sangenSummoning != null) return sangenSummoning;

            // Second target: Sangen Furo, or expendable dragon that already attacked
            var furo = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.SangenFuro);
            if (furo != null) return furo;

            var fadra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonFadra);
            if (fadra != null) return fadra;

            var paidra = candidates.FirstOrDefault(c => c.Id == TenpaiExecutor.CardId.TenpaiDragonPaidra);
            if (paidra != null) return paidra;

            // Never destroy Trident Dragion itself!
            return candidates.Where(c => c.Id != TenpaiExecutor.CardId.TridentDragion).OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // THREAT EVALUATOR: Threat assessment & priority targeting
    // ═══════════════════════════════════════════════════════════════════════════
    public class TenpaiThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly TenpaiExecutor _exec;

        public TenpaiThreatEvaluator(TenpaiExecutor exec)
        {
            _exec = exec;
        }

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;

            int score = 0;
            if (card.IsFaceup())
            {
                if (card.HasType(CardType.Monster))
                {
                    if (card.Attack >= 3000) score += 40;
                    else if (card.Attack >= 2500) score += 30;
                    if (card.HasType(CardType.Effect)) score += 20;
                }
                if (card.HasType(CardType.Continuous) || card.HasType(CardType.Field))
                {
                    score += 25; // Continuous floodgates
                }
            }
            else
            {
                // Face-down backrow
                if (card.IsFacedown() && (card.IsSpell() || card.IsTrap())) score += 35;
            }
            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            return EvaluateThreatScore(card) >= 50;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // BATTLE OTK PLANNER: Calculates lethal damage, attack declarations, locks
    // ═══════════════════════════════════════════════════════════════════════════
    public class TenpaiBattleOTKPlanner
    {
        private readonly TenpaiExecutor _exec;

        public TenpaiBattleOTKPlanner(TenpaiExecutor exec)
        {
            _exec = exec;
        }

        public bool HasBattleLockdown()
        {
            return _exec.Bot.HasInMonstersZone(TenpaiExecutor.CardId.SangenpaiTranscendentDragion);
        }

        public bool CanExecuteTridentTripleAttack()
        {
            return _exec.Bot.HasInMonstersZone(TenpaiExecutor.CardId.TridentDragion) &&
                   _exec.Bot.GetSpellCount() + _exec.Bot.GetMonsterCount() >= 2;
        }

        public int CalculateFieldAttack()
        {
            int total = 0;
            foreach (var m in _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()))
            {
                if (m.Id == TenpaiExecutor.CardId.TridentDragion)
                {
                    // If Sangen Summoning destroyed, Trident is 6000 ATK x 3 = 18000
                    int atk = m.Attack;
                    total += atk * 3;
                }
                else
                {
                    total += m.Attack;
                }
            }
            return total;
        }

        public bool HasLethalBoard()
        {
            return CalculateFieldAttack() >= _exec.Enemy.LifePoints;
        }
    }
}
