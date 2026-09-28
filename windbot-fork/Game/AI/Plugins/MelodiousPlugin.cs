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
    // ===================================================================
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: MelodiousPlugin
    //  Decouples Domain Rules, Strategy, and Material Evaluation
    //  for Modern Melodious Fairy Fusion & Pendulum Engine
    // ===================================================================
    public class MelodiousPlugin : DeckPluginBase
    {
        private readonly MelodiousExecutor _exec;

        public override string DeckName => "Melodious";

        public MelodiousStrategy StrategyImpl { get; }
        public MelodiousMaterialEvaluator MaterialImpl { get; }
        public MelodiousThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public MelodiousPlugin(MelodiousExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new MelodiousStrategy(exec);
            MaterialImpl = new MelodiousMaterialEvaluator(exec);
            ThreatImpl = new MelodiousThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class MelodiousStrategy : IDeckStrategy
    {
        private readonly MelodiousExecutor _exec;
        public MelodiousStrategy(MelodiousExecutor exec) => _exec = exec;

        public void Reset() { }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            int[] priorities = {
                MelodiousExecutor.CardId.FloweringEtoileTheMelodiousMagnificat,
                MelodiousExecutor.CardId.BachaTheMelodiousMaestra,
                MelodiousExecutor.CardId.BloomHarmonistTheMelodiousComposer,
                MelodiousExecutor.CardId.BloomDivaTheMelodiousChoir,
                MelodiousExecutor.CardId.SchubertaTheMelodiousMaestra,
                MelodiousExecutor.CardId.AriaTheMelodiousDiva,
                MelodiousExecutor.CardId.ElegyTheMelodiousDiva,
                MelodiousExecutor.CardId.SopranoTheMelodiousSongstress,
                MelodiousExecutor.CardId.RefrainTheMelodiousSongstress,
                MelodiousExecutor.CardId.CoupletTheMelodiousSongstress,
                MelodiousExecutor.CardId.SonataTheMelodiousDiva,
                MelodiousExecutor.CardId.ShopinaTheMelodiousMaestra,
                MelodiousExecutor.CardId.TamtamTheMelodiousDiva,
                MelodiousExecutor.CardId.SPLittleKnight,
                MelodiousExecutor.CardId.HeraldOfMirageLights,
                MelodiousExecutor.CardId.SuperStarslayerTYPHON
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

            // Context 1: Refrain (Searches Melodious Monster)
            if (context != null && context.IsCode(MelodiousExecutor.CardId.RefrainTheMelodiousSongstress))
            {
                bool hasCouplet = _exec.Bot.Hand.Any(c => c != null && c.IsCode(MelodiousExecutor.CardId.CoupletTheMelodiousSongstress))
                    || _exec.Bot.SpellZone.Any(c => c != null && c.IsCode(MelodiousExecutor.CardId.CoupletTheMelodiousSongstress));
                if (!hasCouplet)
                {
                    var couplet = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.CoupletTheMelodiousSongstress));
                    if (couplet != null) return couplet;
                }

                bool hasSoprano = _exec.Bot.Hand.Any(c => c != null && c.IsCode(MelodiousExecutor.CardId.SopranoTheMelodiousSongstress))
                    || _exec.Bot.MonsterZone.Any(c => c != null && c.IsCode(MelodiousExecutor.CardId.SopranoTheMelodiousSongstress));
                if (!hasSoprano)
                {
                    var soprano = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.SopranoTheMelodiousSongstress));
                    if (soprano != null) return soprano;
                }

                var sonata = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.SonataTheMelodiousDiva));
                if (sonata != null) return sonata;

                var score = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.ScoreTheMelodiousDiva));
                if (score != null) return score;
            }

            // Context 2: Couplet (Searches Melodious Spell/Trap)
            if (context != null && context.IsCode(MelodiousExecutor.CardId.CoupletTheMelodiousSongstress))
            {
                bool hasConcerto = _exec.Bot.Hand.Any(c => c != null && c.IsCode(MelodiousExecutor.CardId.MelodiousConcerto))
                    || _exec.Bot.Graveyard.Any(c => c != null && c.IsCode(MelodiousExecutor.CardId.MelodiousConcerto));
                if (!hasConcerto)
                {
                    var concerto = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.MelodiousConcerto));
                    if (concerto != null) return concerto;
                }

                var ostinato = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.Ostinato));
                if (ostinato != null) return ostinato;

                var solo = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.FirstMovementSolo));
                if (solo != null) return solo;
            }

            // Context 3: Soprano (Recovers Melodious Monster from GY)
            if (context != null && context.IsCode(MelodiousExecutor.CardId.SopranoTheMelodiousSongstress))
            {
                int[] recoverPrio = {
                    MelodiousExecutor.CardId.RefrainTheMelodiousSongstress,
                    MelodiousExecutor.CardId.CoupletTheMelodiousSongstress,
                    MelodiousExecutor.CardId.ScoreTheMelodiousDiva,
                    MelodiousExecutor.CardId.SonataTheMelodiousDiva,
                    MelodiousExecutor.CardId.AriaTheMelodiousDiva,
                    MelodiousExecutor.CardId.ElegyTheMelodiousDiva
                };
                foreach (int id in recoverPrio)
                {
                    var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                    if (match != null) return match;
                }
            }

            // Context 4: Tamtam (Searches Polymerization)
            if (context != null && context.IsCode(MelodiousExecutor.CardId.TamtamTheMelodiousDiva))
            {
                var poly = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.Polymerization));
                if (poly != null) return poly;
            }

            return candidates.FirstOrDefault();
        }

        public IList<ClientCard> PickCards(IList<ClientCard> candidates, int count, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return new List<ClientCard>();

            // Context: Bloom Harmonist (Selects 2 Melodious monsters with DIFFERENT Levels from Deck)
            if (context != null && context.IsCode(MelodiousExecutor.CardId.BloomHarmonistTheMelodiousComposer))
            {
                var result = new List<ClientCard>();

                // Choice 1: Melodious Lock (Aria Lv4 + Elegy Lv5)
                var aria = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.AriaTheMelodiousDiva));
                var elegy = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.ElegyTheMelodiousDiva));

                if (aria != null && elegy != null && count >= 2)
                {
                    result.Add(aria);
                    result.Add(elegy);
                    return result;
                }

                // Choice 2: Soprano (Lv4) + Sonata (Lv3) for Fusion extension
                var soprano = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.SopranoTheMelodiousSongstress));
                var sonata = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.SonataTheMelodiousDiva));

                if (soprano != null && sonata != null && count >= 2)
                {
                    result.Add(soprano);
                    result.Add(sonata);
                    return result;
                }

                // Generic pair with different levels
                for (int i = 0; i < candidates.Count; i++)
                {
                    var c1 = candidates[i];
                    if (c1 == null) continue;
                    for (int j = i + 1; j < candidates.Count; j++)
                    {
                        var c2 = candidates[j];
                        if (c2 == null) continue;
                        if (c1.Level != c2.Level)
                        {
                            result.Add(c1);
                            result.Add(c2);
                            return result;
                        }
                    }
                }
            }

            return candidates.Take(count).ToList();
        }

        public bool ShouldGoFirst() => true;
    }

    public class MelodiousMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly MelodiousExecutor _exec;
        public MelodiousMaterialEvaluator(MelodiousExecutor exec) => _exec = exec;

        public bool IsHighValueMaterial(ClientCard card)
        {
            if (card == null) return false;

            // Never discard or sacrifice key active bosses/lock pieces
            if (card.IsCode(MelodiousExecutor.CardId.FloweringEtoileTheMelodiousMagnificat)) return true;
            if (card.IsCode(MelodiousExecutor.CardId.BloomDivaTheMelodiousChoir)) return true;
            if (card.IsCode(MelodiousExecutor.CardId.Apollousa) && card.Attack >= 1600) return true;
            if (card.IsCode(MelodiousExecutor.CardId.SPLittleKnight)) return true;

            // Protect Aria + Elegy Lock pieces if both are active on field
            bool hasAria = _exec.Bot.MonsterZone.Any(c => c != null && c.IsCode(MelodiousExecutor.CardId.AriaTheMelodiousDiva));
            bool hasElegy = _exec.Bot.MonsterZone.Any(c => c != null && c.IsCode(MelodiousExecutor.CardId.ElegyTheMelodiousDiva));
            if (hasAria && hasElegy)
            {
                if (card.IsCode(MelodiousExecutor.CardId.AriaTheMelodiousDiva) || card.IsCode(MelodiousExecutor.CardId.ElegyTheMelodiousDiva))
                    return true;
            }

            return false;
        }

        public int ScoreMaterial(ClientCard card)
        {
            return 1000 - GetMaterialCost(card);
        }

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;
            if (IsHighValueMaterial(card)) return 1000;

            if (card.IsCode(MelodiousExecutor.CardId.BachaTheMelodiousMaestra)) return 10;
            if (card.IsCode(MelodiousExecutor.CardId.RefrainTheMelodiousSongstress)) return 20;
            if (card.IsCode(MelodiousExecutor.CardId.CoupletTheMelodiousSongstress)) return 20;
            if (card.IsCode(MelodiousExecutor.CardId.SonataTheMelodiousDiva)) return 30;
            if (card.IsCode(MelodiousExecutor.CardId.TamtamTheMelodiousDiva)) return 40;
            if (card.IsCode(MelodiousExecutor.CardId.ShopinaTheMelodiousMaestra)) return 50;

            return 100;
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderBy(c => GetMaterialCost(c)).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Prioritize Bacha (floats on send)
            var bacha = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.BachaTheMelodiousMaestra));
            if (bacha != null) return bacha;

            // Prioritize Couplet / Refrain
            var couplet = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.CoupletTheMelodiousSongstress));
            if (couplet != null) return couplet;

            var refrain = candidates.FirstOrDefault(c => c != null && c.IsCode(MelodiousExecutor.CardId.RefrainTheMelodiousSongstress));
            if (refrain != null) return refrain;

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

    public class MelodiousThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly MelodiousExecutor _exec;
        public MelodiousThreatEvaluator(MelodiousExecutor exec) => _exec = exec;

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
