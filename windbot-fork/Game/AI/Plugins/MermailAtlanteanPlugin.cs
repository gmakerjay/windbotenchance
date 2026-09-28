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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: MermailAtlanteanPlugin
    //  Decouples Domain Rules, Strategy, and Material Evaluation
    //  for Modern Mermail / Atlantean Sea Serpent Synchro & Xyz
    // ═══════════════════════════════════════════════════════════════
    public class MermailAtlanteanPlugin : DeckPluginBase
    {
        private readonly MermailAtlanteanExecutor _exec;

        public override string DeckName => "MermailAtlantean";

        public MermailAtlanteanStrategy StrategyImpl { get; }
        public MermailAtlanteanMaterialEvaluator MaterialImpl { get; }
        public MermailAtlanteanThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public MermailAtlanteanPlugin(MermailAtlanteanExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new MermailAtlanteanStrategy(exec);
            MaterialImpl = new MermailAtlanteanMaterialEvaluator(exec);
            ThreatImpl = new MermailAtlanteanThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class MermailAtlanteanStrategy : IDeckStrategy
    {
        private readonly MermailAtlanteanExecutor _exec;
        public MermailAtlanteanStrategy(MermailAtlanteanExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            int[] priorities = {
                MermailAtlanteanExecutor.CardId.PoseidraAbyssTheAtlanteanDragonLord,
                MermailAtlanteanExecutor.CardId.MermailAbyssgaios,
                MermailAtlanteanExecutor.CardId.AdamancipatorRisenDragite,
                MermailAtlanteanExecutor.CardId.IcejadeGymirAegirine,
                MermailAtlanteanExecutor.CardId.SwordsoulSupremeSovereignChengying,
                MermailAtlanteanExecutor.CardId.TrishulaDragonOfTheIceBarrier,
                MermailAtlanteanExecutor.CardId.MoulinglaciaTheElementalLord,
                MermailAtlanteanExecutor.CardId.AbysstriteTheAtlanteanSpirit,
                MermailAtlanteanExecutor.CardId.MermailKingNeptabyss,
                MermailAtlanteanExecutor.CardId.MarincessCoralAnemone,
                MermailAtlanteanExecutor.CardId.MermailAbyssteus,
                MermailAtlanteanExecutor.CardId.NeptabyssTheAtlanteanPrince,
                MermailAtlanteanExecutor.CardId.AbyssrhineTheAtlanteanSpirit,
                MermailAtlanteanExecutor.CardId.PoseidraTheStormingAtlantean,
                MermailAtlanteanExecutor.CardId.AtlanteanDragoons,
                MermailAtlanteanExecutor.CardId.MermailShadowSquad,
                MermailAtlanteanExecutor.CardId.MermailAbysspike,
                MermailAtlanteanExecutor.CardId.SuperancientDeepseaKingCoelacanth
            };

            foreach (int id in priorities)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            return candidates.FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Context 1: Atlantean Dragoons (Searches ANY Sea Serpent)
            if (context != null && context.IsCode(MermailAtlanteanExecutor.CardId.AtlanteanDragoons))
            {
                // Check if we can summon Moulinglacia (exactly 5 WATER in GY)
                int waterInGy = _exec.Bot.Graveyard.Count(c => c != null && c.HasAttribute(CardAttribute.Water));
                if (waterInGy == 4 || waterInGy == 5)
                {
                    var moulin = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.MoulinglaciaTheElementalLord));
                    if (moulin != null && !_exec.Bot.Hand.Any(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.MoulinglaciaTheElementalLord)))
                        return moulin;
                }

                // If don't have Abyssteus -> search Abyssteus
                bool hasAbyssteus = _exec.Bot.Hand.Any(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.MermailAbyssteus));
                if (!hasAbyssteus)
                {
                    var teus = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.MermailAbyssteus));
                    if (teus != null) return teus;
                }

                // If don't have Abyssrhine -> search Abyssrhine
                bool hasRhine = _exec.Bot.Hand.Any(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.AbyssrhineTheAtlanteanSpirit));
                if (!hasRhine)
                {
                    var rhine = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.AbyssrhineTheAtlanteanSpirit));
                    if (rhine != null) return rhine;
                }

                // Search Neptabyss
                var nept = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.NeptabyssTheAtlanteanPrince));
                if (nept != null) return nept;

                // Search Poseidra
                var pos = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.PoseidraTheStormingAtlantean));
                if (pos != null) return pos;
            }

            // Context 2: Neptabyss the Atlantean Prince (Searches Atlantean card)
            if (context != null && context.IsCode(MermailAtlanteanExecutor.CardId.NeptabyssTheAtlanteanPrince))
            {
                var dragoons = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.AtlanteanDragoons));
                if (dragoons != null) return dragoons;

                var infantry = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.AtlanteanHeavyInfantry));
                if (infantry != null) return infantry;

                var poseidra = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.PoseidraTheStormingAtlantean));
                if (poseidra != null) return poseidra;
            }

            // Context 3: Mermail Abyssteus (Searches Level 4 or lower Mermail)
            if (context != null && context.IsCode(MermailAtlanteanExecutor.CardId.MermailAbyssteus))
            {
                bool hasSpike = _exec.Bot.Hand.Any(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.MermailAbysspike));
                if (!hasSpike)
                {
                    var spike = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.MermailAbysspike));
                    if (spike != null) return spike;
                }

                var squad = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.MermailShadowSquad));
                if (squad != null) return squad;

                var ocea = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.MermailAbyssocea));
                if (ocea != null) return ocea;
            }

            // Context 4: Abyssrhine (Adds/Summons Level 7 Fish, Sea Serpent, Aqua)
            if (context != null && context.IsCode(MermailAtlanteanExecutor.CardId.AbyssrhineTheAtlanteanSpirit))
            {
                var teus = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.MermailAbyssteus));
                if (teus != null) return teus;

                var poseidra = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.PoseidraTheStormingAtlantean));
                if (poseidra != null) return poseidra;

                var coel = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.SuperancientDeepseaKingCoelacanth));
                if (coel != null) return coel;
            }

            // General Priority Fallback
            int[] generalPriorities = {
                MermailAtlanteanExecutor.CardId.AtlanteanDragoons,
                MermailAtlanteanExecutor.CardId.NeptabyssTheAtlanteanPrince,
                MermailAtlanteanExecutor.CardId.MermailAbyssteus,
                MermailAtlanteanExecutor.CardId.AbyssrhineTheAtlanteanSpirit,
                MermailAtlanteanExecutor.CardId.MermailShadowSquad,
                MermailAtlanteanExecutor.CardId.MermailAbysspike,
                MermailAtlanteanExecutor.CardId.AtlanteanHeavyInfantry,
                MermailAtlanteanExecutor.CardId.MoulinglaciaTheElementalLord,
                MermailAtlanteanExecutor.CardId.PoseidraTheStormingAtlantean
            };

            foreach (int id in generalPriorities)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            return candidates.FirstOrDefault();
        }
    }

    public class MermailAtlanteanMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly MermailAtlanteanExecutor _exec;
        public MermailAtlanteanMaterialEvaluator(MermailAtlanteanExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // 1. Absolute Boss Protection
            if (card.IsCode(MermailAtlanteanExecutor.CardId.PoseidraAbyssTheAtlanteanDragonLord,
                            MermailAtlanteanExecutor.CardId.MermailAbyssgaios,
                            MermailAtlanteanExecutor.CardId.AdamancipatorRisenDragite,
                            MermailAtlanteanExecutor.CardId.IcejadeGymirAegirine,
                            MermailAtlanteanExecutor.CardId.SwordsoulSupremeSovereignChengying,
                            MermailAtlanteanExecutor.CardId.TrishulaDragonOfTheIceBarrier,
                            MermailAtlanteanExecutor.CardId.MoulinglaciaTheElementalLord))
            {
                return 1000;
            }

            // 2. High-Value Starters on Field
            if (card.IsCode(MermailAtlanteanExecutor.CardId.NeptabyssTheAtlanteanPrince,
                            MermailAtlanteanExecutor.CardId.MermailKingNeptabyss))
            {
                return 500;
            }

            // 3. Atlantean Dragoons (WANTS to be discarded / sent to GY for cost!)
            if (card.IsCode(MermailAtlanteanExecutor.CardId.AtlanteanDragoons))
            {
                return 1; // Lowest cost = Highest priority to send!
            }

            // 4. Mermail Shadow Squad (triggers on sent for WATER effect)
            if (card.IsCode(MermailAtlanteanExecutor.CardId.MermailShadowSquad))
            {
                return 5;
            }

            // 5. Atlantean Heavy Infantry (triggers on sent for WATER effect to pop card)
            if (card.IsCode(MermailAtlanteanExecutor.CardId.AtlanteanHeavyInfantry))
            {
                return 10;
            }

            // 6. Generic WATER monsters
            if (card.HasAttribute(CardAttribute.Water))
            {
                return 40;
            }

            return 80;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Prioritize Dragoons (activates search)
            var dragoons = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.AtlanteanDragoons));
            if (dragoons != null) return dragoons;

            // Prioritize Shadow Squad (activates deck summon)
            var squad = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.MermailShadowSquad));
            if (squad != null) return squad;

            // Prioritize Heavy Infantry (pops face-up card)
            var infantry = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.AtlanteanHeavyInfantry));
            if (infantry != null) return infantry;

            // Prioritize First Penguin (GY recursion)
            var penguin = candidates.FirstOrDefault(c => c != null && c.IsCode(MermailAtlanteanExecutor.CardId.FirstPenguin));
            if (penguin != null) return penguin;

            // Prioritize duplicate cards
            var duplicate = candidates.GroupBy(c => c.Id).FirstOrDefault(g => g.Count() > 1)?.FirstOrDefault();
            if (duplicate != null) return duplicate;

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class MermailAtlanteanThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly MermailAtlanteanExecutor _exec;
        public MermailAtlanteanThreatEvaluator(MermailAtlanteanExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null) return 0;
            return CardIntelligence.GetCardThreatScore(card);
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return CardIntelligence.GetCardThreatScore(card) >= 9000 || (card.IsMonster() && card.Attack >= 3000);
        }
    }
}
