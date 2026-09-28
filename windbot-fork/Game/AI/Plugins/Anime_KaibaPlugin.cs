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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: Anime_KaibaPlugin
    //  Decouples Domain Rules, Strategy, and Material Evaluation
    //  for Seto Kaiba's Blue-Eyes Jet & Ultimate Dragon Engine
    // ═══════════════════════════════════════════════════════════════
    public class Anime_KaibaPlugin : DeckPluginBase
    {
        private readonly Anime_KaibaExecutor _exec;

        public override string DeckName => "Anime_Kaiba";

        public Anime_KaibaStrategy StrategyImpl { get; }
        public Anime_KaibaMaterialEvaluator MaterialImpl { get; }
        public Anime_KaibaThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public Anime_KaibaPlugin(Anime_KaibaExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new Anime_KaibaStrategy(exec);
            MaterialImpl = new Anime_KaibaMaterialEvaluator(exec);
            ThreatImpl = new Anime_KaibaThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class Anime_KaibaStrategy : IDeckStrategy
    {
        private readonly Anime_KaibaExecutor _exec;

        public Anime_KaibaStrategy(Anime_KaibaExecutor exec)
        {
            _exec = exec;
        }

        public void Reset()
        {
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // 1. Extra Deck Boss Priorities
            int[] extraBossPriority = {
                Anime_KaibaExecutor.CardId.Number100NumeronDragon,
                Anime_KaibaExecutor.CardId.Number97Draglubion,
                Anime_KaibaExecutor.CardId.Number38HopeHarbinger,
                Anime_KaibaExecutor.CardId.Number90PhotonLord,
                Anime_KaibaExecutor.CardId.BlueEyesSpiritDragon,
                Anime_KaibaExecutor.CardId.NeoBlueEyesUltimateDragon,
                Anime_KaibaExecutor.CardId.BlueEyesTyrantDragon,
                Anime_KaibaExecutor.CardId.AzureEyesSilverDragon,
                Anime_KaibaExecutor.CardId.BlackRoseMoonlightDragon,
                Anime_KaibaExecutor.CardId.MichaelTheArchLightsworn,
                Anime_KaibaExecutor.CardId.BlueEyesTwinBurstDragon
            };

            foreach (int id in extraBossPriority)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            // 2. Main Deck Dragons (Revives / Summons from Deck)
            // Prioritize Jet Dragon if not on field, then Abyss, then Alternative, then BEWD
            bool hasJetOnField = _exec.Bot.GetMonsters().Any(m => m.IsCode(Anime_KaibaExecutor.CardId.BlueEyesJetDragon));
            if (!hasJetOnField)
            {
                var jet = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesJetDragon));
                if (jet != null) return jet;
            }

            var abyss = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesAbyssDragon));
            if (abyss != null) return abyss;

            var bewd = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesWhiteDragon));
            if (bewd != null) return bewd;

            var alt = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesAlternativeWhiteDragon));
            if (alt != null) return alt;

            return candidates.FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Context: Melody of Awakening Dragon
            if (context != null && context.IsCode(Anime_KaibaExecutor.CardId.TheMelodyOfAwakeningDragon))
            {
                bool hasBewdInHand = _exec.Bot.Hand.Any(c => c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesWhiteDragon));
                bool hasAltInHand = _exec.Bot.Hand.Any(c => c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesAlternativeWhiteDragon));

                if (!hasBewdInHand)
                {
                    var bewd = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesWhiteDragon));
                    if (bewd != null) return bewd;
                }
                if (!hasAltInHand)
                {
                    var alt = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesAlternativeWhiteDragon));
                    if (alt != null) return alt;
                }

                var jet = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesJetDragon));
                if (jet != null) return jet;

                var abyss = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesAbyssDragon));
                if (abyss != null) return abyss;
            }

            // Context: Sage with Eyes of Blue (Search Level 1 LIGHT Tuner)
            if (context != null && context.IsCode(Anime_KaibaExecutor.CardId.SageWithEyesOfBlue))
            {
                // If we need GY setup/end phase summon: White Stone of Ancients
                var ancients = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.TheWhiteStoneOfAncients));
                if (ancients != null) return ancients;

                // If going second or already have stone: Effect Veiler (Disruption handtrap)
                var veiler = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.EffectVeiler));
                if (veiler != null) return veiler;

                var legend = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.TheWhiteStoneOfLegend));
                if (legend != null) return legend;
            }

            // Context: Abyss Dragon / True Light (Search Ultimate Fusion)
            if (context != null && (context.IsCode(Anime_KaibaExecutor.CardId.BlueEyesAbyssDragon) || context.IsCode(Anime_KaibaExecutor.CardId.TrueLight)))
            {
                var fusion = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.UltimateFusion));
                if (fusion != null) return fusion;

                var bewd = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesWhiteDragon));
                if (bewd != null) return bewd;
            }

            // General Priorities
            int[] generalPriority = {
                Anime_KaibaExecutor.CardId.TheMelodyOfAwakeningDragon,
                Anime_KaibaExecutor.CardId.BlueEyesAlternativeWhiteDragon,
                Anime_KaibaExecutor.CardId.BlueEyesJetDragon,
                Anime_KaibaExecutor.CardId.UltimateFusion,
                Anime_KaibaExecutor.CardId.BlueEyesWhiteDragon,
                Anime_KaibaExecutor.CardId.TheWhiteStoneOfAncients,
                Anime_KaibaExecutor.CardId.DictatorOfD,
                Anime_KaibaExecutor.CardId.SageWithEyesOfBlue,
                Anime_KaibaExecutor.CardId.EffectVeiler
            };

            foreach (int id in generalPriority)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            return candidates.FirstOrDefault();
        }
    }

    public class Anime_KaibaMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly Anime_KaibaExecutor _exec;

        public Anime_KaibaMaterialEvaluator(Anime_KaibaExecutor exec)
        {
            _exec = exec;
        }

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // 1. Ultra High Cost: Xyz, Synchro & Fusion Bosses (NEVER link away / sacrifice)
            if (card.IsCode(Anime_KaibaExecutor.CardId.Number100NumeronDragon)) return 100;
            if (card.IsCode(Anime_KaibaExecutor.CardId.Number97Draglubion)) return 95;
            if (card.IsCode(Anime_KaibaExecutor.CardId.Number38HopeHarbinger)) return 95;
            if (card.IsCode(Anime_KaibaExecutor.CardId.Number90PhotonLord)) return 95;
            if (card.IsCode(Anime_KaibaExecutor.CardId.NeoBlueEyesUltimateDragon)) return 95;
            if (card.IsCode(Anime_KaibaExecutor.CardId.BlueEyesTyrantDragon)) return 90;
            if (card.IsCode(Anime_KaibaExecutor.CardId.BlueEyesSpiritDragon)) return 90;
            if (card.IsCode(Anime_KaibaExecutor.CardId.AzureEyesSilverDragon)) return 85;
            if (card.IsCode(Anime_KaibaExecutor.CardId.BlueEyesTwinBurstDragon)) return 85;

            // 2. High Cost: Main Deck Bosses
            if (card.IsCode(Anime_KaibaExecutor.CardId.BlueEyesJetDragon)) return 80;
            if (card.IsCode(Anime_KaibaExecutor.CardId.BlueEyesAlternativeWhiteDragon)) return 70;
            if (card.IsCode(Anime_KaibaExecutor.CardId.BlueEyesWhiteDragon)) return 65;
            if (card.IsCode(Anime_KaibaExecutor.CardId.BlueEyesAbyssDragon)) return 60;

            // 3. Handtraps in Hand
            if (card.Location == CardLocation.Hand &&
                (card.IsCode(Anime_KaibaExecutor.CardId.AshBlossom) ||
                 card.IsCode(Anime_KaibaExecutor.CardId.EffectVeiler) ||
                 card.IsCode(Anime_KaibaExecutor.CardId.Nibiru)))
            {
                return 50;
            }

            // 4. Low Cost / Fodder: Stones, Sage, Dictator (SAFE to link / contact fuse / send)
            if (card.IsCode(Anime_KaibaExecutor.CardId.TheWhiteStoneOfLegend)) return 5;
            if (card.IsCode(Anime_KaibaExecutor.CardId.TheWhiteStoneOfAncients)) return 5;
            if (card.IsCode(Anime_KaibaExecutor.CardId.SageWithEyesOfBlue)) return 8;
            if (card.IsCode(Anime_KaibaExecutor.CardId.RelinquishedAnima)) return 10;
            if (card.IsCode(Anime_KaibaExecutor.CardId.DictatorOfD)) return 15;

            if (card.Attack >= 2500) return 60;
            if (card.Attack >= 2000) return 40;

            return 20;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Priority 1: White Stone of Ancients (triggers in End Phase to SS Blue-Eyes!)
            var ancients = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.TheWhiteStoneOfAncients));
            if (ancients != null) return ancients;

            // Priority 2: White Stone of Legend (triggers to add BEWD to hand!)
            var legend = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.TheWhiteStoneOfLegend));
            if (legend != null) return legend;

            // Priority 3: Blue-Eyes White Dragon if we have multiple in hand
            if (_exec.Bot.Hand.Count(c => c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesWhiteDragon)) > 1)
            {
                var bewd = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_KaibaExecutor.CardId.BlueEyesWhiteDragon));
                if (bewd != null) return bewd;
            }

            // Priority 4: Excess Level 8 or low-value cards
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class Anime_KaibaThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly Anime_KaibaExecutor _exec;

        public Anime_KaibaThreatEvaluator(Anime_KaibaExecutor exec)
        {
            _exec = exec;
        }

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;

            int score = 0;

            // S/T board wipes that destroy True Light (which would blow up our entire field!)
            int[] backrowThreats = {
                18144506, // Harpie's Feather Duster
                9952083,  // Lightning Storm
                53582587, // Heavy Storm
                15693423, // Summon Limit
                82044279, // Skill Drain
                41420027  // Solemn Judgment
            };

            if (backrowThreats.Contains(card.Id))
            {
                score += 60;
            }

            // Floodgate / Towers monsters
            if (card.IsFaceup() && card.Attack >= 3000)
            {
                score += 35;
            }

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;

            // Immediate field wipe spells or game-ending floodgates
            if (card.Id == 18144506 || card.Id == 9952083 || card.Id == 82044279)
                return true;

            return false;
        }
    }
}
