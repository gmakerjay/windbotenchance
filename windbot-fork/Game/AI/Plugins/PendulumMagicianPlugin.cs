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
    // ============================================================================
    // PENDULUM MAGICIAN PLUGIN — DECOUPLED DOMAIN ARCHITECTURE (Layer 3)
    // ============================================================================
    internal class PendulumMagicianPlugin : DeckPluginBase
    {
        private readonly PendulumMagicianExecutor _exec;

        public override string DeckName => "PendulumMagician";

        public override IDeckStrategy Strategy { get; }
        public override IDeckScaleResolver ScaleResolver { get; }
        public override IDeckMaterialEvaluator MaterialEvaluator { get; }
        public override IDeckThreatEvaluator ThreatEvaluator { get; }
        public override IDeckActionScorer ActionScorer { get; }

        public PendulumMagicianPlugin(PendulumMagicianExecutor exec)
        {
            _exec = exec;
            Strategy = new PendulumMagicianStrategy(exec);
            ScaleResolver = new PendulumMagicianScaleResolver(exec);
            MaterialEvaluator = new PendulumMagicianMaterialEvaluator(exec);
            ThreatEvaluator = new PendulumMagicianThreatEvaluator(exec);
            ActionScorer = new PendulumMagicianActionScorer(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
        }
    }

    // ============================================================================
    // DOMAIN HELPER 1: STRATEGY (Search Priorities, SS Priorities, Combo Triggers)
    // ============================================================================
    internal class PendulumMagicianStrategy : IDeckStrategy
    {
        private readonly PendulumMagicianExecutor _exec;

        public PendulumMagicianStrategy(PendulumMagicianExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Harmonizing Magician pull from deck
            // If Baronne is in Extra Deck and not on field:
            // Harmonizing (4 Tuner) + Oafdragon (6 non-Tuner Magician) = Baronne de Fleur (Level 10 Omni-Negate)!
            bool needBaronne = _exec.Bot.HasInExtra(PendulumMagicianExecutor.CardId.BaronneDeFleur)
                               && !_exec.Bot.GetMonsters().Any(m => m.Id == PendulumMagicianExecutor.CardId.BaronneDeFleur);

            return candidates.OrderByDescending(c =>
            {
                if (needBaronne && c.Id == PendulumMagicianExecutor.CardId.OafdragonMagician) return 1200;
                if (c.Id == PendulumMagicianExecutor.CardId.PurplePoisonMagician) return 900;
                if (c.Id == PendulumMagicianExecutor.CardId.DoubleIrisMagician) return 850;
                if (c.Id == PendulumMagicianExecutor.CardId.BlackFangMagician) return 800;
                if (c.Id == PendulumMagicianExecutor.CardId.OafdragonMagician) return 780;
                if (c.Id == PendulumMagicianExecutor.CardId.TimegazerMagician) return 750;
                if (c.Id == PendulumMagicianExecutor.CardId.WhiteWingMagician) return 700;
                if (c.Id == PendulumMagicianExecutor.CardId.SupremeKingZARC) return 990;
                return 100;
            }).FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            bool hasLowScale = _exec.HasLowScaleInHandOrPZone();
            bool hasHighScale = _exec.HasHighScaleInHandOrPZone();
            bool hasHarmonizing = _exec.Bot.HasInHand(PendulumMagicianExecutor.CardId.HarmonizingMagician);

            return candidates.OrderByDescending(c =>
            {
                // 1. If missing scales, prioritize Wisdom-Eye or scale pieces
                if (!hasLowScale || !hasHighScale)
                {
                    if (c.Id == PendulumMagicianExecutor.CardId.WisdomEyeMagician) return 1000;
                    if (!hasLowScale && (c.Id == PendulumMagicianExecutor.CardId.PurplePoisonMagician || c.Id == PendulumMagicianExecutor.CardId.WhiteWingMagician)) return 950;
                    if (!hasHighScale && (c.Id == PendulumMagicianExecutor.CardId.DoubleIrisMagician || c.Id == PendulumMagicianExecutor.CardId.BlackFangMagician)) return 940;
                }

                // 2. Combo Starter: Harmonizing Magician (The engine's heart)
                if (!hasHarmonizing && c.Id == PendulumMagicianExecutor.CardId.HarmonizingMagician) return 920;

                // 3. Double Iris for Pendulumgraph search & Dragon material
                if (c.Id == PendulumMagicianExecutor.CardId.DoubleIrisMagician) return 850;

                // 4. Purple Poison for removal & ATK buff
                if (c.Id == PendulumMagicianExecutor.CardId.PurplePoisonMagician) return 820;

                // 5. Astrograph Sorcerer for card advantage & Z-ARC
                if (c.Id == PendulumMagicianExecutor.CardId.AstrographSorcerer) return 800;

                // 6. Spell search: Pendulum Call / Star / Time Pendulumgraph
                if (c.Id == PendulumMagicianExecutor.CardId.PendulumCall) return 780;
                if (c.Id == PendulumMagicianExecutor.CardId.TimePendulumgraph) return 770;
                if (c.Id == PendulumMagicianExecutor.CardId.StarPendulumgraph) return 750;

                // 7. Recovery & Extenders
                if (c.Id == PendulumMagicianExecutor.CardId.OafdragonMagician) return 700;
                if (c.Id == PendulumMagicianExecutor.CardId.BlackFangMagician) return 680;
                if (c.Id == PendulumMagicianExecutor.CardId.ChronographSorcerer) return 650;

                return 100;
            }).FirstOrDefault();
        }
    }

    // ============================================================================
    // DOMAIN HELPER 2: SCALE RESOLVER (Pairing Scale 1 with Scale 8, Anti-Mismatch)
    // ============================================================================
    internal class PendulumMagicianScaleResolver : IDeckScaleResolver
    {
        private readonly PendulumMagicianExecutor _exec;

        public PendulumMagicianScaleResolver(PendulumMagicianExecutor exec) => _exec = exec;

        public ClientCard PickLowScale()
        {
            // Low Scales (Scale 1-2):
            // Priority: Purple Poison (1) > White Wing (1) > Astrograph (1) > Oafdragon (2)
            var hand = _exec.Bot.Hand;

            var purple = hand.FirstOrDefault(c => c.Id == PendulumMagicianExecutor.CardId.PurplePoisonMagician);
            if (purple != null) return purple;

            var white = hand.FirstOrDefault(c => c.Id == PendulumMagicianExecutor.CardId.WhiteWingMagician);
            if (white != null) return white;

            var oaf = hand.FirstOrDefault(c => c.Id == PendulumMagicianExecutor.CardId.OafdragonMagician);
            if (oaf != null) return oaf;

            var astro = hand.FirstOrDefault(c => c.Id == PendulumMagicianExecutor.CardId.AstrographSorcerer);
            if (astro != null) return astro;

            return hand.FirstOrDefault(c => c.HasType(CardType.Pendulum) && c.LScale <= 3);
        }

        public ClientCard PickHighScale()
        {
            // High Scales (Scale 8):
            // Priority: Double Iris (8) > Black Fang (8) > Dragonpit (8) > Chronograph (8) > Skullcrobat (8)
            // STRICT RULE: Harmonizing Magician MUST NOT be picked if other high scales exist!
            var hand = _exec.Bot.Hand;

            var iris = hand.FirstOrDefault(c => c.Id == PendulumMagicianExecutor.CardId.DoubleIrisMagician);
            if (iris != null) return iris;

            var fang = hand.FirstOrDefault(c => c.Id == PendulumMagicianExecutor.CardId.BlackFangMagician);
            if (fang != null) return fang;

            var pit = hand.FirstOrDefault(c => c.Id == PendulumMagicianExecutor.CardId.DragonpitMagician);
            if (pit != null) return pit;

            var chrono = hand.FirstOrDefault(c => c.Id == PendulumMagicianExecutor.CardId.ChronographSorcerer);
            if (chrono != null) return chrono;

            // If Skullcrobat is in hand and we already normal summoned this turn, it can be scale 8
            if (_exec.NormalSummonUsed)
            {
                var skull = hand.FirstOrDefault(c => c.Id == PendulumMagicianExecutor.CardId.PerformapalSkullcrobatJoker);
                if (skull != null) return skull;
            }

            // Fallback to any other Scale 8 (excluding Harmonizing)
            var otherHigh = hand.FirstOrDefault(c => c.HasType(CardType.Pendulum) && c.LScale >= 8 && c.Id != PendulumMagicianExecutor.CardId.HarmonizingMagician);
            if (otherHigh != null) return otherHigh;

            // Absolute desperate fallback: only use Harmonizing if no other high scale exists
            return hand.FirstOrDefault(c => c.Id == PendulumMagicianExecutor.CardId.HarmonizingMagician);
        }

        public ClientCard PickScalePopTarget()
        {
            // For Electrumite pop, Ignister Prominence pop, or Time Pendulumgraph pop:
            // Priority 1: Double Iris Magician (destroys self to search Pendulumgraph)
            var iris = _exec.Bot.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == PendulumMagicianExecutor.CardId.DoubleIrisMagician);
            if (iris != null) return iris;

            // Priority 2: Purple Poison Magician (destroys self to pop an opponent card)
            var poison = _exec.Bot.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == PendulumMagicianExecutor.CardId.PurplePoisonMagician);
            if (poison != null) return poison;

            // Priority 3: Black Fang Magician (revives DARK Spellcaster)
            var fang = _exec.Bot.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == PendulumMagicianExecutor.CardId.BlackFangMagician);
            if (fang != null) return fang;

            // Priority 4: Wisdom-Eye Magician if still in scale
            var wisdom = _exec.Bot.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() && s.Id == PendulumMagicianExecutor.CardId.WisdomEyeMagician);
            if (wisdom != null) return wisdom;

            // Priority 5: Any face-up spell that is not Star/Time Pendulumgraph
            return _exec.Bot.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup() &&
                s.Id != PendulumMagicianExecutor.CardId.StarPendulumgraph &&
                s.Id != PendulumMagicianExecutor.CardId.TimePendulumgraph);
        }
    }

    // ============================================================================
    // DOMAIN HELPER 3: MATERIAL EVALUATOR (Ace Card Guard & Discard/Substitute Optimization)
    // ============================================================================
    internal class PendulumMagicianMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly PendulumMagicianExecutor _exec;

        public PendulumMagicianMaterialEvaluator(PendulumMagicianExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // Tier 1: Absolute Ace Protection (Never link away or tribute)
            if (card.Id == PendulumMagicianExecutor.CardId.SupremeKingZARC) return 10000;
            if (card.Id == PendulumMagicianExecutor.CardId.BaronneDeFleur) return 9500;
            if (card.Id == PendulumMagicianExecutor.CardId.BorreloadSavageDragon) return 9500;
            if (card.Id == PendulumMagicianExecutor.CardId.AccesscodeTalker) return 9500;
            if (card.Id == PendulumMagicianExecutor.CardId.Apollousa) return 8500;
            if (card.Id == PendulumMagicianExecutor.CardId.Number41Bagooska) return 8500;
            if (card.Id == PendulumMagicianExecutor.CardId.TornadoDragon) return 8000;
            if (card.Id == PendulumMagicianExecutor.CardId.TimestarMagician) return 8000;
            if (card.Id == PendulumMagicianExecutor.CardId.AbyssDweller) return 7500;
            if (card.Id == PendulumMagicianExecutor.CardId.SupremeKingDragonClearWing) return 7000;
            if (card.Id == PendulumMagicianExecutor.CardId.SupremeKingDragonDarkRebellion) return 7000;

            // Tier 2: Steal / Fodder protection
            if (card.Controller == 1) return 100; // Opponent card stolen

            // Tier 3: Pendulum Fodder (Cheap materials)
            if (card.Id == PendulumMagicianExecutor.CardId.PerformapalSkullcrobatJoker) return 200;
            if (card.Id == PendulumMagicianExecutor.CardId.TimegazerMagician) return 250;
            if (card.Id == PendulumMagicianExecutor.CardId.WhiteWingMagician) return 300;
            if (card.Id == PendulumMagicianExecutor.CardId.OafdragonMagician) return 350;

            return 500;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // For Pendulum Call discard or costs:
            // Prefer cards that benefit from GY (Black Fang, Purple Poison) or redundant duplicate copies
            return candidates.OrderBy(c =>
            {
                // Never discard Harmonizing Magician or Wisdom-Eye Magician!
                if (c.Id == PendulumMagicianExecutor.CardId.HarmonizingMagician) return 9999;
                if (c.Id == PendulumMagicianExecutor.CardId.WisdomEyeMagician) return 9000;
                if (c.Id == PendulumMagicianExecutor.CardId.DuelistAlliance) return 8500;

                // High priority discards:
                if (c.Id == PendulumMagicianExecutor.CardId.BlackFangMagician) return 100; // Can revive with another Black Fang
                if (c.Id == PendulumMagicianExecutor.CardId.WhiteWingMagician) return 150; // Tuner in GY for Z-ARC
                if (c.Id == PendulumMagicianExecutor.CardId.PurplePoisonMagician) return 200; // Fusion Dragon in GY for Z-ARC
                if (c.Id == PendulumMagicianExecutor.CardId.DragonpitMagician) return 250;

                // Redundant copies
                int countInHand = candidates.Count(x => x.Id == c.Id);
                if (countInHand > 1) return 300;

                return 500;
            }).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // For Timestar Magician sending from Deck to GY to replace destruction:
            // Send the Dragon Magician missing from GY to prepare Supreme King Z-ARC!
            return candidates.OrderByDescending(c =>
            {
                if (c.Id == PendulumMagicianExecutor.CardId.PurplePoisonMagician && !_exec.HasDragonInGraveyard(PendulumMagicianExecutor.CardId.PurplePoisonMagician)) return 1000;
                if (c.Id == PendulumMagicianExecutor.CardId.BlackFangMagician && !_exec.HasDragonInGraveyard(PendulumMagicianExecutor.CardId.BlackFangMagician)) return 950;
                if (c.Id == PendulumMagicianExecutor.CardId.WhiteWingMagician && !_exec.HasDragonInGraveyard(PendulumMagicianExecutor.CardId.WhiteWingMagician)) return 900;
                if (c.Id == PendulumMagicianExecutor.CardId.DoubleIrisMagician && !_exec.HasDragonInGraveyard(PendulumMagicianExecutor.CardId.DoubleIrisMagician)) return 850;
                return 100;
            }).FirstOrDefault();
        }
    }

    // ============================================================================
    // DOMAIN HELPER 4: THREAT EVALUATOR (Prioritizing Removal Targets)
    // ============================================================================
    internal class PendulumMagicianThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly PendulumMagicianExecutor _exec;

        public PendulumMagicianThreatEvaluator(PendulumMagicianExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;

            int score = 0;
            // Eternal Soul: Destroying this card automatically wipes all opponent monsters!
            if (card.Id == 48680970) score += 500;
            // Dark Magical Circle: Banishing removal engine
            if (card.Id == 47222536) score += 200;

            // Anti-Special Summon floodgates (Skill Drain, Anti-Spell Fragrance)
            if (CardIntelligence.IsFloodgate(card.Id) || card.Id == 58921041) // Anti-Spell Fragrance is fatal for Pendulums!
                score += 100;

            if (CardIntelligence.IsKnownNegator(card.Id))
                score += 80;

            if (card.IsMonster() && card.Attack >= 2500)
                score += 50;

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            return CardIntelligence.IsFloodgate(card.Id) || (card.IsMonster() && card.Attack >= 3000);
        }
    }

    // ============================================================================
    // DOMAIN HELPER 5: ACTION SCORER
    // ============================================================================
    internal class PendulumMagicianActionScorer : IDeckActionScorer
    {
        private readonly PendulumMagicianExecutor _exec;

        public PendulumMagicianActionScorer(PendulumMagicianExecutor exec) => _exec = exec;

        public int CalculateActionScore(string actionName, int boardImpact, int netGain, int cost)
        {
            return (boardImpact * 3) + (netGain * 2) - cost;
        }
    }
}
