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
    // DECOUPLED DOMAIN PLUGIN ARCHITECTURE: WatenpaiPlugin
    // Implements Strategy, MaterialEvaluator, ThreatEvaluator & DirectDamagePlanner
    // ═══════════════════════════════════════════════════════════════════════════
    public class WatenpaiPlugin : DeckPluginBase
    {
        private readonly WatenpaiExecutor _exec;

        public override string DeckName => "Watenpai";

        public WatenpaiStrategy StrategyImpl { get; }
        public WatenpaiMaterialEvaluator MaterialImpl { get; }
        public WatenpaiThreatEvaluator ThreatImpl { get; }
        public WatenpaiDirectDamagePlanner DamagePlanner { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public WatenpaiPlugin(WatenpaiExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new WatenpaiStrategy(exec);
            MaterialImpl = new WatenpaiMaterialEvaluator(exec);
            ThreatImpl = new WatenpaiThreatEvaluator(exec);
            DamagePlanner = new WatenpaiDirectDamagePlanner(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class WatenpaiStrategy : IDeckStrategy
    {
        private readonly WatenpaiExecutor _exec;

        public WatenpaiStrategy(WatenpaiExecutor exec)
        {
            _exec = exec;
        }

        public void Reset()
        {
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Interrupted Kaiju Slumber:
            // The 1st selection goes to Bot's field: give STRONGEST Kaiju (Thunder King 3300)
            // The 2nd selection goes to Opponent's field: give WEAKEST Kaiju (Kumongous 2400 or Radian 2800)
            if (candidates.Any(c => c.Id == WatenpaiExecutor.CardId.ThunderKingKaiju || c.Id == WatenpaiExecutor.CardId.KumongousKaiju || c.Id == WatenpaiExecutor.CardId.RadianKaiju))
            {
                var thunderKing = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.ThunderKingKaiju);
                if (thunderKing != null && !_exec.Bot.HasInMonstersZone(WatenpaiExecutor.CardId.ThunderKingKaiju))
                {
                    return thunderKing;
                }
                var weakKaiju = candidates.OrderBy(c => c.Attack).FirstOrDefault();
                if (weakKaiju != null) return weakKaiju;
            }

            // 2. Wattuna direct damage release -> SS Watt Synchro from Extra Deck:
            if (candidates.Any(c => c.Id == WatenpaiExecutor.CardId.Wattkyuki || c.Id == WatenpaiExecutor.CardId.Watthydra))
            {
                var wattkyuki = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.Wattkyuki);
                if (wattkyuki != null) return wattkyuki;

                var watthydra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.Watthydra);
                if (watthydra != null) return watthydra;
            }

            // 3. Tenpai Bident Dragion reviving from GY:
            if (candidates.Any(c => c.Location == CardLocation.Grave))
            {
                var paidra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonPaidra);
                if (paidra != null) return paidra;

                var fadra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonFadra);
                if (fadra != null) return fadra;

                var chundra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonChundra);
                if (chundra != null) return chundra;
            }

            return candidates.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Wattcobra direct damage search: Add Wattuna (Tuner enabler!) > Wattcobra > Brohunder
            if (context != null && context.Id == WatenpaiExecutor.CardId.Wattcobra)
            {
                var wattuna = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.Wattuna && !_exec.Bot.HasInHand(WatenpaiExecutor.CardId.Wattuna));
                if (wattuna != null) return wattuna;

                var cobra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.Wattcobra);
                if (cobra != null) return cobra;

                var dragonfly = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.Wattdragonfly);
                if (dragonfly != null) return dragonfly;
            }

            // 2. Brohunder Normal Summon search: Add Level 4 LIGHT Thunder monster
            if (context != null && context.Id == WatenpaiExecutor.CardId.Brohunder)
            {
                var cobra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.Wattcobra && !_exec.Bot.HasInHand(WatenpaiExecutor.CardId.Wattcobra));
                if (cobra != null) return cobra;

                var tuna = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.Wattuna);
                if (tuna != null) return tuna;
            }

            // 3. Dora Dora search: Add Level 4 or lower FIRE Dragon (Paidra > Chundra > Fadra)
            if (context != null && context.Id == WatenpaiExecutor.CardId.DoraDora)
            {
                if (!_exec.Bot.HasInHand(WatenpaiExecutor.CardId.TenpaiDragonPaidra) && !_exec.Bot.HasInMonstersZone(WatenpaiExecutor.CardId.TenpaiDragonPaidra))
                {
                    var paidra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonPaidra);
                    if (paidra != null) return paidra;
                }

                if (!_exec.Bot.HasInHand(WatenpaiExecutor.CardId.TenpaiDragonChundra) && !_exec.Bot.HasInMonstersZone(WatenpaiExecutor.CardId.TenpaiDragonChundra))
                {
                    var chundra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonChundra);
                    if (chundra != null) return chundra;
                }

                var fadra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonFadra);
                if (fadra != null) return fadra;
            }

            // 4. Paidra search: Sangen Summoning > Sangen Kaimen
            if (context != null && context.Id == WatenpaiExecutor.CardId.TenpaiDragonPaidra)
            {
                if (!_exec.Bot.HasInSpellZone(WatenpaiExecutor.CardId.SangenSummoning) && !_exec.Bot.HasInHand(WatenpaiExecutor.CardId.SangenSummoning))
                {
                    var field = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.SangenSummoning);
                    if (field != null) return field;
                }

                var kaimen = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.SangenKaimen);
                if (kaimen != null) return kaimen;
            }

            // 5. Sangen Summoning search: Chundra > Paidra > Genroku
            if (context != null && context.Id == WatenpaiExecutor.CardId.SangenSummoning)
            {
                if (!_exec.Bot.HasInHand(WatenpaiExecutor.CardId.TenpaiDragonChundra) && !_exec.Bot.HasInMonstersZone(WatenpaiExecutor.CardId.TenpaiDragonChundra))
                {
                    var chundra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonChundra);
                    if (chundra != null) return chundra;
                }

                var paidra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonPaidra);
                if (paidra != null) return paidra;

                var genroku = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonGenroku);
                if (genroku != null) return genroku;
            }

            // 6. Sangen Kaimen search: Add Level 4 or lower FIRE Dragon
            if (context != null && context.Id == WatenpaiExecutor.CardId.SangenKaimen)
            {
                // If no Genroku in hand, search Genroku! (Genroku triggers upon being added to hand!)
                if (!_exec.Bot.HasInHand(WatenpaiExecutor.CardId.TenpaiDragonGenroku))
                {
                    var genroku = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonGenroku);
                    if (genroku != null) return genroku;
                }

                // If no Paidra, search Paidra (searches Sangen Summoning!)
                if (!_exec.Bot.HasInHand(WatenpaiExecutor.CardId.TenpaiDragonPaidra) && !_exec.Bot.HasInMonstersZone(WatenpaiExecutor.CardId.TenpaiDragonPaidra))
                {
                    var paidra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonPaidra);
                    if (paidra != null) return paidra;
                }

                var chundra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonChundra);
                if (chundra != null) return chundra;

                var fadra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonFadra);
                if (fadra != null) return fadra;
            }

            return candidates.FirstOrDefault();
        }
    }

    public class WatenpaiMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly WatenpaiExecutor _exec;

        public WatenpaiMaterialEvaluator(WatenpaiExecutor exec)
        {
            _exec = exec;
        }

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == WatenpaiExecutor.CardId.TridentDragion) return 10000;
            if (card.Id == WatenpaiExecutor.CardId.SangenpaiTranscendentDragion) return 9000;
            if (card.Id == WatenpaiExecutor.CardId.ThunderKingKaiju) return 8000;
            if (card.Id == WatenpaiExecutor.CardId.Wattkyuki) return 7000;
            if (card.Id == WatenpaiExecutor.CardId.SangenpaiBidentDragion) return 5000;
            if (card.Id == WatenpaiExecutor.CardId.Wattuna) return 3000;
            if (card.Id == WatenpaiExecutor.CardId.Wattcobra) return 2500;
            if (card.Id == WatenpaiExecutor.CardId.TenpaiDragonChundra) return 2500;
            if (card.Id == WatenpaiExecutor.CardId.TenpaiDragonPaidra) return 2000;
            if (card.Id == WatenpaiExecutor.CardId.Wattdragonfly) return 800;
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

            // Priority 1: Duplicate board breakers (e.g. 2nd Raigeki or 2nd Dark Hole)
            var duplicateSpells = candidates.Where(c => c.IsSpell() && candidates.Count(x => x.Id == c.Id) > 1).FirstOrDefault();
            if (duplicateSpells != null) return duplicateSpells;

            // Priority 2: Blaster, Dragon Ruler of Infernos (can be revived from GY by banishing 2 FIRE)
            var blaster = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.Blaster);
            if (blaster != null) return blaster;

            // Priority 3: Wattdragonfly
            var dragonfly = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.Wattdragonfly);
            if (dragonfly != null) return dragonfly;

            // Priority 4: Fadra (can be revived by Bident/Chundra)
            var fadra = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.TenpaiDragonFadra);
            if (fadra != null) return fadra;

            return candidates
                .Where(c => c.Id != WatenpaiExecutor.CardId.SangenSummoning && c.Id != WatenpaiExecutor.CardId.Wattuna)
                .OrderBy(GetMaterialCost)
                .FirstOrDefault() ?? candidates.First();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // For Trident Dragion: ALWAYS destroy Sangen Summoning to double ATK to 6000!
            var field = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.SangenSummoning);
            if (field != null) return field;

            // Fodder monster
            var dragonfly = candidates.FirstOrDefault(c => c.Id == WatenpaiExecutor.CardId.Wattdragonfly);
            if (dragonfly != null) return dragonfly;

            return candidates.Where(c => c.Id != WatenpaiExecutor.CardId.TridentDragion).OrderBy(GetMaterialCost).FirstOrDefault();
        }
    }

    public class WatenpaiThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly WatenpaiExecutor _exec;

        public WatenpaiThreatEvaluator(WatenpaiExecutor exec)
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
                if (card.HasType(CardType.Continuous) || card.HasType(CardType.Field)) score += 25;
            }
            else
            {
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

    public class WatenpaiDirectDamagePlanner
    {
        private readonly WatenpaiExecutor _exec;

        public WatenpaiDirectDamagePlanner(WatenpaiExecutor exec)
        {
            _exec = exec;
        }

        public int CalculateDirectDamagePotential()
        {
            int total = 0;
            foreach (var m in _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()))
            {
                if (m.Id == WatenpaiExecutor.CardId.Wattcobra) total += 1000;
                else if (m.Id == WatenpaiExecutor.CardId.Wattuna) total += 800;
                else if (m.Id == WatenpaiExecutor.CardId.Wattkyuki) total += 1600;
                else if (m.Id == WatenpaiExecutor.CardId.Watthydra) total += 1500;
            }
            return total;
        }

        public bool HasDirectLethal()
        {
            return CalculateDirectDamagePotential() >= _exec.Enemy.LifePoints;
        }
    }
}
