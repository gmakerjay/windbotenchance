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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: GrenMajuPlugin
    //  Danger! Kaiju Level 8 Turbo & Numeron Dragon Plan B OTK
    // ═══════════════════════════════════════════════════════════════
    public class GrenMajuPlugin : DeckPluginBase
    {
        private readonly GrenMajuExecutor _exec;

        public override string DeckName => "GrenMaju";

        public GrenMajuStrategy StrategyImpl { get; }
        public GrenMajuMaterialEvaluator MaterialImpl { get; }
        public GrenMajuThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public GrenMajuPlugin(GrenMajuExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new GrenMajuStrategy(exec);
            MaterialImpl = new GrenMajuMaterialEvaluator(exec);
            ThreatImpl = new GrenMajuThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class GrenMajuStrategy : IDeckStrategy
    {
        private readonly GrenMajuExecutor _exec;

        public GrenMajuStrategy(GrenMajuExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Draglubion special summons Numeron Dragon
            var numeron = candidates.FirstOrDefault(c => c != null && c.Id == GrenMajuExecutor.CardId.NumeronDragon);
            if (numeron != null) return numeron;

            // Interrupted Kaiju Slumber:
            // Bot's side -> Dogoran (3000 ATK beater)
            // Enemy's side -> Gameciel (2200 ATK weak target to punch over)
            var dogoran = candidates.FirstOrDefault(c => c != null && c.Id == GrenMajuExecutor.CardId.DogoranTheMadFlameKaiju);
            if (dogoran != null) return dogoran;

            var gameciel = candidates.FirstOrDefault(c => c != null && c.Id == GrenMajuExecutor.CardId.GamecielTheSeaTurtleKaiju);
            if (gameciel != null) return gameciel;

            return candidates.FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Kaiju Slumber GY search
            var gameciel = candidates.FirstOrDefault(c => c != null && c.Id == GrenMajuExecutor.CardId.GamecielTheSeaTurtleKaiju);
            if (gameciel != null) return gameciel;

            var dogoran = candidates.FirstOrDefault(c => c != null && c.Id == GrenMajuExecutor.CardId.DogoranTheMadFlameKaiju);
            if (dogoran != null) return dogoran;

            return candidates.FirstOrDefault();
        }

        public IList<ClientCard> PickKaijuTributeTargets(IList<ClientCard> candidates, int count)
        {
            if (candidates == null || candidates.Count == 0) return new List<ClientCard>();

            var enemyMonsters = candidates.Where(c => c != null && c.Controller == 1 && c.IsFaceup()).ToList();
            if (enemyMonsters.Count < count)
            {
                enemyMonsters = candidates.Where(c => c != null && c.Controller == 1).ToList();
            }

            if (enemyMonsters.Count == 0)
            {
                return candidates.Take(count).ToList();
            }

            // Prioritize highest threat score / known negators / Extra Deck bosses
            return enemyMonsters
                .OrderByDescending(c => _exec.Plugin.ThreatImpl.EvaluateThreatScore(c))
                .ThenByDescending(c => c.Attack)
                .Take(count)
                .ToList();
        }

        public IList<ClientCard> PickExtraDeckBanishTargets(IList<ClientCard> candidates, int count)
        {
            if (candidates == null || candidates.Count == 0) return new List<ClientCard>();

            // Strictly banish sacrificial fodder first (Fusion / Link), preserving Rank 8 Plan B combo!
            return candidates
                .Where(c => c != null)
                .OrderBy(c => _exec.Plugin.MaterialImpl.GetMaterialCost(c))
                .Take(count)
                .ToList();
        }

        public ClientCard PickDangerDestroyTarget(IList<ClientCard> candidates, bool targetSet)
        {
            if (candidates == null || candidates.Count == 0) return null;

            var enemyTargets = candidates.Where(c => c != null && c.Controller == 1).ToList();
            if (enemyTargets.Count == 0) return null; // NEVER pop own cards!

            if (targetSet)
            {
                // Thunderbird targets Set cards
                var setCard = enemyTargets.FirstOrDefault(c => c.IsFacedown());
                if (setCard != null) return setCard;
            }
            else
            {
                // Bigfoot targets face-up cards
                var faceup = enemyTargets.Where(c => c.IsFaceup()).ToList();
                if (faceup.Count > 0)
                {
                    // Prioritize Floodgates / Known Negators first
                    var danger = faceup.FirstOrDefault(c => CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsFloodgate(c.Id));
                    if (danger != null) return danger;

                    return faceup.OrderByDescending(c => c.Attack).FirstOrDefault();
                }
            }

            return enemyTargets.FirstOrDefault();
        }

        public ClientCard PickAlphaBounceTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Step 1: Select our Beast monster to return to hand (Alpha itself)
            var ourAlpha = candidates.FirstOrDefault(c => c != null && c.Controller == 0 && c.Id == GrenMajuExecutor.CardId.AlphaTheMasterOfBeasts);
            if (ourAlpha != null) return ourAlpha;

            // Step 2: Select enemy face-up monster to bounce back to hand/extra deck
            var enemyFaceup = candidates.Where(c => c != null && c.Controller == 1 && c.IsFaceup()).ToList();
            if (enemyFaceup.Count > 0)
            {
                var boss = enemyFaceup.FirstOrDefault(c => CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsFloodgate(c.Id) || c.IsExtraCard());
                if (boss != null) return boss;

                return enemyFaceup.OrderByDescending(c => c.Attack).FirstOrDefault();
            }

            return candidates.FirstOrDefault(c => c != null && c.Controller == 1);
        }

        public ClientCard PickDingirsuTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Dingirsu sends 1 card opponent controls to GY without targeting!
            var enemyCards = candidates.Where(c => c != null && c.Controller == 1).ToList();
            if (enemyCards.Count == 0) return null;

            var danger = enemyCards.FirstOrDefault(c => c.IsFaceup() && (CardIntelligence.IsKnownNegator(c.Id) || CardIntelligence.IsFloodgate(c.Id)));
            if (danger != null) return danger;

            return enemyCards.OrderByDescending(c => c.Attack).FirstOrDefault();
        }

        public IList<ClientCard> PickDraglubionOverlayTargets(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return new List<ClientCard>();

            // Draglubion needs 2 Number dragon monsters with different names from Extra Deck:
            // 1. Numeron Dragon (57314798)
            // 2. Hope Harbinger (63767246)
            var numeron = candidates.FirstOrDefault(c => c != null && c.Id == GrenMajuExecutor.CardId.NumeronDragon);
            var hope = candidates.FirstOrDefault(c => c != null && c.Id == GrenMajuExecutor.CardId.HopeHarbinger);

            var result = new List<ClientCard>();
            if (numeron != null) result.Add(numeron);
            if (hope != null) result.Add(hope);

            if (result.Count < 2)
            {
                foreach (var c in candidates)
                {
                    if (!result.Contains(c))
                    {
                        result.Add(c);
                        if (result.Count == 2) break;
                    }
                }
            }

            return result;
        }
    }

    public class GrenMajuMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly GrenMajuExecutor _exec;
        public GrenMajuMaterialEvaluator(GrenMajuExecutor exec) => _exec = exec;

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 999;

            // NEVER sacrifice Gren Maju Da Eiza or core Plan B OTK bosses
            if (card.Id == GrenMajuExecutor.CardId.GrenMajuDaEiza) return 99999;
            if (card.Id == GrenMajuExecutor.CardId.NumeronDragon) return 99998;
            if (card.Id == GrenMajuExecutor.CardId.Draglubion) return 99997;
            if (card.Id == GrenMajuExecutor.CardId.HopeHarbinger) return 99996;
            if (card.Id == GrenMajuExecutor.CardId.Dingirsu) return 90000;
            if (card.Id == GrenMajuExecutor.CardId.AAZeus) return 80000;
            if (card.Id == GrenMajuExecutor.CardId.TYPHON) return 70000;

            // Extra Deck targets - prioritize sacrificial fodder first (Fusion / Link)
            if (card.Location == CardLocation.Extra)
            {
                if (card.Id == GrenMajuExecutor.CardId.EarthGolemIgnister) return 10;
                if (card.Id == GrenMajuExecutor.CardId.MudragonOfTheSwamp) return 20;
                if (card.Id == GrenMajuExecutor.CardId.Garura) return 30;
                if (card.Id == GrenMajuExecutor.CardId.PredaplantDragostapelia) return 40;
                if (card.Id == GrenMajuExecutor.CardId.StarvingVenomFusionDragon) return 50;
                if (card.Id == GrenMajuExecutor.CardId.SPLittleKnight) return 60;
                return 100;
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

            // For Trade-In discard:
            // 1. Danger! Bigfoot! (triggers pop face-up)
            var bigfoot = candidates.FirstOrDefault(c => c != null && c.Id == GrenMajuExecutor.CardId.DangerBigfoot);
            if (bigfoot != null) return bigfoot;

            // 2. Danger! Thunderbird! (triggers pop set)
            var thunderbird = candidates.FirstOrDefault(c => c != null && c.Id == GrenMajuExecutor.CardId.DangerThunderbird);
            if (thunderbird != null) return thunderbird;

            // 3. Gizmek Orochi (summons from GY!)
            var orochi = candidates.FirstOrDefault(c => c != null && c.Id == GrenMajuExecutor.CardId.GizmekOrochi);
            if (orochi != null) return orochi;

            // 4. Alpha or Kaijus
            var alpha = candidates.FirstOrDefault(c => c != null && c.Id == GrenMajuExecutor.CardId.AlphaTheMasterOfBeasts);
            if (alpha != null) return alpha;

            var kaiju = candidates.FirstOrDefault(c => c != null && (c.Id == GrenMajuExecutor.CardId.DogoranTheMadFlameKaiju || c.Id == GrenMajuExecutor.CardId.GamecielTheSeaTurtleKaiju));
            if (kaiju != null) return kaiju;

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class GrenMajuThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly GrenMajuExecutor _exec;
        public GrenMajuThreatEvaluator(GrenMajuExecutor exec) => _exec = exec;

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;
            int score = 0;

            int id = card.Id;
            if (CardIntelligence.IsKnownNegator(id)) score += 10000;
            if (CardIntelligence.IsFloodgate(id)) score += 8000;

            if (card.HasType(CardType.Fusion | CardType.Synchro | CardType.Xyz | CardType.Link)) score += 5000;
            if (card.Attack >= 3000) score += 4000;
            else if (card.Attack >= 2500) score += 3000;
            else if (card.Attack >= 2000) score += 2000;

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;
            return card.Attack >= 3000 || CardIntelligence.IsKnownNegator(card.Id);
        }
    }
}
