using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.DecisionEngine;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    // ═══════════════════════════════════════════════════════════════
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: TrirealmRiftPlugin
    //  Decouples Domain Rules, Strategy, and Material Evaluation
    //  for Trirealm Rift (Yomi / Face-Down Banishment) Archetype
    // ═══════════════════════════════════════════════════════════════
    public class TrirealmRiftPlugin : DeckPluginBase
    {
        private readonly TrirealmRiftExecutor _exec;

        public override string DeckName => "TrirealmRift";

        public TrirealmRiftStrategy StrategyImpl { get; }
        public TrirealmRiftMaterialEvaluator MaterialImpl { get; }
        public TrirealmRiftThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public TrirealmRiftPlugin(TrirealmRiftExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new TrirealmRiftStrategy(exec);
            MaterialImpl = new TrirealmRiftMaterialEvaluator(exec);
            ThreatImpl = new TrirealmRiftThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class TrirealmRiftStrategy : IDeckStrategy
    {
        private readonly TrirealmRiftExecutor _exec;
        public TrirealmRiftStrategy(TrirealmRiftExecutor exec) => _exec = exec;

        public void Reset() { }

        /// <summary>
        /// Selects the best Level 5+ or boss monster to Special Summon from face-down banished zone.
        /// </summary>
        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // In Opponent turn: Yomi (Monster Quick Negate) > Helheim (Protection) > Ploutonion (Spin) > Darkness > Naraka
            if (_exec.Duel.Player != 0)
            {
                int[] enemyTurnPriorities = {
                    TrirealmRiftExecutor.CardId.SkyThunderTrirealmRiftYomi,
                    TrirealmRiftExecutor.CardId.BurialSummitTrirealmRiftHelheim,
                    TrirealmRiftExecutor.CardId.MadTempestTrirealmRiftPloutonion,
                    TrirealmRiftExecutor.CardId.TrirealmRiftDarkness,
                    TrirealmRiftExecutor.CardId.TrirealmRiftOfScarletNaraka
                };

                foreach (int id in enemyTurnPriorities)
                {
                    var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                    if (match != null) return match;
                }
            }

            // In Bot turn: Helheim (protects board & other Trirealms) > Darkness (3000 Beatdown) > Yomi > Ploutonion > Gehenna > Sheol
            int[] botTurnPriorities = {
                TrirealmRiftExecutor.CardId.BurialSummitTrirealmRiftHelheim,
                TrirealmRiftExecutor.CardId.TrirealmRiftDarkness,
                TrirealmRiftExecutor.CardId.SkyThunderTrirealmRiftYomi,
                TrirealmRiftExecutor.CardId.MadTempestTrirealmRiftPloutonion,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfSkySheol,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfEmptinessGehenna,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfScarletNaraka,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfBlueTuonela,
                TrirealmRiftExecutor.CardId.SPLittleKnight,
                TrirealmRiftExecutor.CardId.AccesscodeTalker
            };

            foreach (int id in botTurnPriorities)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            return candidates.FirstOrDefault();
        }

        /// <summary>
        /// Selects the best single Trirealm Rift card to add to hand from face-down banished cards.
        /// </summary>
        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            var list = PickSearchTargets(candidates, context, 1);
            return list?.FirstOrDefault();
        }

        /// <summary>
        /// Selects up to `max` cards (supporting Gehenna & Sheol multi-add up to 2 cards with different names).
        /// </summary>
        public IList<ClientCard> PickSearchTargets(IList<ClientCard> candidates, ClientCard context, int max)
        {
            if (candidates == null || candidates.Count == 0) return new List<ClientCard>();

            var result = new List<ClientCard>();
            var usedCodes = new HashSet<int>();

            // Context 1: Sheol (Adds up to 2 Spells/Traps with DIFFERENT names)
            if (context != null && context.IsCode(TrirealmRiftExecutor.CardId.TrirealmRiftOfSkySheol))
            {
                int[] stPriorities = {
                    TrirealmRiftExecutor.CardId.TrirealmRiftGospel,
                    TrirealmRiftExecutor.CardId.TrirealmRiftTerritoryValvols,
                    TrirealmRiftExecutor.CardId.TrirealmRiftJudgment
                };

                foreach (int id in stPriorities)
                {
                    var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id) && !usedCodes.Contains(c.Id));
                    if (match != null)
                    {
                        result.Add(match);
                        usedCodes.Add(match.Id);
                        if (result.Count >= max) return result;
                    }
                }
            }

            // Context 2: Gehenna (Adds up to 2 Monsters with DIFFERENT names)
            if (context != null && context.IsCode(TrirealmRiftExecutor.CardId.TrirealmRiftOfEmptinessGehenna))
            {
                int[] monsterPriorities = {
                    TrirealmRiftExecutor.CardId.TrirealmRiftOfSkySheol,
                    TrirealmRiftExecutor.CardId.BurialSummitTrirealmRiftHelheim,
                    TrirealmRiftExecutor.CardId.SkyThunderTrirealmRiftYomi,
                    TrirealmRiftExecutor.CardId.TrirealmRiftOfScarletNaraka,
                    TrirealmRiftExecutor.CardId.MadTempestTrirealmRiftPloutonion,
                    TrirealmRiftExecutor.CardId.TrirealmRiftDarkness,
                    TrirealmRiftExecutor.CardId.TrirealmRiftOfBlueTuonela
                };

                foreach (int id in monsterPriorities)
                {
                    var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id) && !usedCodes.Contains(c.Id));
                    if (match != null)
                    {
                        result.Add(match);
                        usedCodes.Add(match.Id);
                        if (result.Count >= max) return result;
                    }
                }
            }

            // Context 3: General priorities (Yomi, Valvols, Gospel)
            int[] generalPriorities = {
                TrirealmRiftExecutor.CardId.TrirealmRiftTerritoryValvols,
                TrirealmRiftExecutor.CardId.TrirealmRiftGospel,
                TrirealmRiftExecutor.CardId.BurialSummitTrirealmRiftHelheim,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfSkySheol,
                TrirealmRiftExecutor.CardId.TrirealmRiftJudgment,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfScarletNaraka,
                TrirealmRiftExecutor.CardId.MadTempestTrirealmRiftPloutonion,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfEmptinessGehenna,
                TrirealmRiftExecutor.CardId.TrirealmRiftDarkness
            };

            foreach (int id in generalPriorities)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id) && !usedCodes.Contains(c.Id));
                if (match != null)
                {
                    result.Add(match);
                    usedCodes.Add(match.Id);
                    if (result.Count >= max) return result;
                }
            }

            foreach (var c in candidates)
            {
                if (c != null && !usedCodes.Contains(c.Id))
                {
                    result.Add(c);
                    usedCodes.Add(c.Id);
                    if (result.Count >= max) break;
                }
            }

            return result;
        }
    }

    public class TrirealmRiftMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly TrirealmRiftExecutor _exec;
        public TrirealmRiftMaterialEvaluator(TrirealmRiftExecutor exec) => _exec = exec;

        public void Reset() { }

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // 1. Core Boss Protection
            if (card.IsCode(TrirealmRiftExecutor.CardId.SkyThunderTrirealmRiftYomi,
                            TrirealmRiftExecutor.CardId.BurialSummitTrirealmRiftHelheim,
                            TrirealmRiftExecutor.CardId.TrirealmRiftDarkness,
                            TrirealmRiftExecutor.CardId.MadTempestTrirealmRiftPloutonion))
            {
                return 1000;
            }

            // 2. High Value Backrow Locks
            if (card.IsCode(TrirealmRiftExecutor.CardId.TrirealmRiftTerritoryValvols,
                            TrirealmRiftExecutor.CardId.TrirealmRiftJudgment,
                            TrirealmRiftExecutor.CardId.TrirealmRiftGospel))
            {
                return 800;
            }

            // 3. Low-level fodder
            if (card.IsCode(TrirealmRiftExecutor.CardId.TrirealmRiftOfBlueTuonela,
                            TrirealmRiftExecutor.CardId.TrirealmRiftOfSkySheol,
                            TrirealmRiftExecutor.CardId.TrirealmRiftOfEmptinessGehenna))
            {
                return 20;
            }

            return 50;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int needed = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int count = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            int[] discardPriority = {
                TrirealmRiftExecutor.CardId.GizmekOrochi, // Loves being in GY!
                TrirealmRiftExecutor.CardId.TrirealmRiftOfBlueTuonela,
                TrirealmRiftExecutor.CardId.PotOfDesires,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfSkySheol,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfEmptinessGehenna
            };

            foreach (int id in discardPriority)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickFusionMaterial(IList<ClientCard> candidates) => candidates?.FirstOrDefault();
        public ClientCard PickSynchroMaterial(IList<ClientCard> candidates) => candidates?.FirstOrDefault();
        public ClientCard PickXyzMaterial(IList<ClientCard> candidates) => candidates?.FirstOrDefault();

        public ClientCard PickLinkMaterial(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            int[] linkFodderPriorities = {
                TrirealmRiftExecutor.CardId.TrirealmRiftOfBlueTuonela,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfSkySheol,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfEmptinessGehenna,
                TrirealmRiftExecutor.CardId.TrirealmRiftOfScarletNaraka
            };

            foreach (int id in linkFodderPriorities)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class TrirealmRiftThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly TrirealmRiftExecutor _exec;
        public TrirealmRiftThreatEvaluator(TrirealmRiftExecutor exec) => _exec = exec;

        public void Reset() { }

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

        public ClientCard PickBestRemovalTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            var threat = candidates.FirstOrDefault(c => c != null && c.IsFaceup() && (c.Attack >= 2500 || CardIntelligence.IsFloodgate(c.Id) || CardIntelligence.IsHighThreatChokepoint(c.Id) || CardIntelligence.IsKnownNegator(c.Id)));
            if (threat != null) return threat;

            var backrow = candidates.FirstOrDefault(c => c != null && (c.Location == CardLocation.SpellZone || c.IsFacedown()));
            if (backrow != null) return backrow;

            return candidates.FirstOrDefault();
        }
    }
}
