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
    //  DECOUPLED DOMAIN PLUGIN ARCHITECTURE: Anime_SayerPlugin
    //  Decouples Domain Rules, Strategy, and Material Evaluation
    //  for Sayer's Ultimate Psychic Synchro Battlebox Engine
    // ═══════════════════════════════════════════════════════════════
    public class Anime_SayerPlugin : DeckPluginBase
    {
        private readonly Anime_SayerExecutor _exec;

        public override string DeckName => "Anime_Sayer";

        public Anime_SayerStrategy StrategyImpl { get; }
        public Anime_SayerMaterialEvaluator MaterialImpl { get; }
        public Anime_SayerThreatEvaluator ThreatImpl { get; }

        public override IDeckStrategy Strategy => StrategyImpl;
        public override IDeckMaterialEvaluator MaterialEvaluator => MaterialImpl;
        public override IDeckThreatEvaluator ThreatEvaluator => ThreatImpl;

        public Anime_SayerPlugin(Anime_SayerExecutor exec)
        {
            _exec = exec;
            StrategyImpl = new Anime_SayerStrategy(exec);
            MaterialImpl = new Anime_SayerMaterialEvaluator(exec);
            ThreatImpl = new Anime_SayerThreatEvaluator(exec);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            StrategyImpl.Reset();
        }
    }

    public class Anime_SayerStrategy : IDeckStrategy
    {
        private readonly Anime_SayerExecutor _exec;

        public Anime_SayerStrategy(Anime_SayerExecutor exec)
        {
            _exec = exec;
        }

        public void Reset()
        {
        }

        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            int botTunerCount = _exec.Bot.GetMonsters().Count(m => m.IsFaceup() && m.IsTuner());
            int botNonTunerCount = _exec.Bot.GetMonsters().Count(m => m.IsFaceup() && !m.IsTuner());

            // 1. Synchro Boss Priority
            int[] bossPriority = {
                Anime_SayerExecutor.CardId.PsychicEndPunisher,
                Anime_SayerExecutor.CardId.PsychicBlasterMkII,
                Anime_SayerExecutor.CardId.ThoughtRulerArchfiend,
                Anime_SayerExecutor.CardId.PSYFramelordOmega,
                Anime_SayerExecutor.CardId.HyperPsychicRiser,
                Anime_SayerExecutor.CardId.PsychicOmnibuster,
                Anime_SayerExecutor.CardId.HyperPsychicBlaster,
                Anime_SayerExecutor.CardId.OvermindArchfiend,
                Anime_SayerExecutor.CardId.PsychicLifetrancer,
                Anime_SayerExecutor.CardId.HTSPsyhemuth,
                Anime_SayerExecutor.CardId.MindCastlin,
                Anime_SayerExecutor.CardId.SerenePsychicSorceress,
                Anime_SayerExecutor.CardId.MagicalAndroid
            };

            foreach (int id in bossPriority)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            // 2. Main Deck Tuner vs Non-Tuner balance
            if (botTunerCount == 0 && botNonTunerCount > 0)
            {
                int[] tunerOrder = {
                    Anime_SayerExecutor.CardId.PsychicWheeleder,
                    Anime_SayerExecutor.CardId.GhostOgreAndSnowRabbit,
                    Anime_SayerExecutor.CardId.PsychicCommander,
                    Anime_SayerExecutor.CardId.Krebons,
                    Anime_SayerExecutor.CardId.SerenePsychicGirl
                };
                foreach (int id in tunerOrder)
                {
                    var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                    if (match != null) return match;
                }
            }
            else if (botNonTunerCount == 0 && botTunerCount > 0)
            {
                int[] nonTunerOrder = {
                    Anime_SayerExecutor.CardId.PsychicTracker,
                    Anime_SayerExecutor.CardId.HushedPsychicMinister,
                    Anime_SayerExecutor.CardId.SilentPsychicWizard,
                    Anime_SayerExecutor.CardId.ArmoredAxonKicker
                };
                foreach (int id in nonTunerOrder)
                {
                    var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                    if (match != null) return match;
                }
            }

            return candidates.FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Context: Terraforming -> Brain Research Lab
            if (context != null && context.IsCode(Anime_SayerExecutor.CardId.Terraforming))
            {
                var lab = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_SayerExecutor.CardId.BrainResearchLab));
                if (lab != null) return lab;
            }

            // Context: Hushed Psychic Minister (Add Level 3 or lower Psychic)
            if (context != null && context.IsCode(Anime_SayerExecutor.CardId.HushedPsychicMinister))
            {
                bool hasWheeleder = _exec.Bot.Hand.Any(c => c != null && c.IsCode(Anime_SayerExecutor.CardId.PsychicWheeleder));
                bool hasTracker = _exec.Bot.Hand.Any(c => c != null && c.IsCode(Anime_SayerExecutor.CardId.PsychicTracker));

                if (!hasWheeleder)
                {
                    var wheeleder = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_SayerExecutor.CardId.PsychicWheeleder));
                    if (wheeleder != null) return wheeleder;
                }
                if (!hasTracker)
                {
                    var tracker = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_SayerExecutor.CardId.PsychicTracker));
                    if (tracker != null) return tracker;
                }

                // Default search: Ghost Ogre (Disruption) or Wheeleder
                var ogre = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_SayerExecutor.CardId.GhostOgreAndSnowRabbit));
                if (ogre != null) return ogre;
            }

            // Context: Mind Procedure (Excavated 5 cards)
            if (context != null && context.IsCode(Anime_SayerExecutor.CardId.MindProcedure))
            {
                int[] excavationPriority = {
                    Anime_SayerExecutor.CardId.OverdriveTeleporter,
                    Anime_SayerExecutor.CardId.PsychicWheeleder,
                    Anime_SayerExecutor.CardId.PsychicTracker,
                    Anime_SayerExecutor.CardId.GhostOgreAndSnowRabbit,
                    Anime_SayerExecutor.CardId.SilentPsychicWizard,
                    Anime_SayerExecutor.CardId.HushedPsychicMinister,
                    Anime_SayerExecutor.CardId.ArmoredAxonKicker
                };
                foreach (int id in excavationPriority)
                {
                    var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                    if (match != null) return match;
                }
            }

            // General Priority
            int[] generalPriority = {
                Anime_SayerExecutor.CardId.BrainResearchLab,
                Anime_SayerExecutor.CardId.PsychicWheeleder,
                Anime_SayerExecutor.CardId.PsychicTracker,
                Anime_SayerExecutor.CardId.GhostOgreAndSnowRabbit,
                Anime_SayerExecutor.CardId.OverdriveTeleporter,
                Anime_SayerExecutor.CardId.SilentPsychicWizard
            };

            foreach (int id in generalPriority)
            {
                var match = candidates.FirstOrDefault(c => c != null && c.IsCode(id));
                if (match != null) return match;
            }

            return candidates.FirstOrDefault();
        }
    }

    public class Anime_SayerMaterialEvaluator : IDeckMaterialEvaluator
    {
        private readonly Anime_SayerExecutor _exec;

        public Anime_SayerMaterialEvaluator(Anime_SayerExecutor exec)
        {
            _exec = exec;
        }

        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 0;

            // 1. Ace Bosses & Floodgates: Highest Cost (DO NOT SACRIFICE / TRIBUTE)
            if (card.IsCode(Anime_SayerExecutor.CardId.PsychicEndPunisher)) return 100;
            if (card.IsCode(Anime_SayerExecutor.CardId.ThoughtRulerArchfiend)) return 85;
            if (card.IsCode(Anime_SayerExecutor.CardId.PSYFramelordOmega)) return 85;
            if (card.IsCode(Anime_SayerExecutor.CardId.PsychicBlasterMkII)) return 85;
            if (card.IsCode(Anime_SayerExecutor.CardId.HyperPsychicRiser)) return 80;
            if (card.IsCode(Anime_SayerExecutor.CardId.HyperPsychicBlaster)) return 75;
            if (card.IsCode(Anime_SayerExecutor.CardId.OvermindArchfiend)) return 75;

            // 2. High-value main deck monsters
            if (card.IsCode(Anime_SayerExecutor.CardId.MasterGig)) return 60;
            if (card.IsCode(Anime_SayerExecutor.CardId.OverdriveTeleporter)) return 55;
            if (card.IsCode(Anime_SayerExecutor.CardId.ArmoredAxonKicker)) return 40;

            // 3. Handtrap: Ghost Ogre (preserve in hand)
            if (card.IsCode(Anime_SayerExecutor.CardId.GhostOgreAndSnowRabbit) && card.Location == CardLocation.Hand) return 50;

            // 4. Graveyard Floaters & Fodder (Low Cost - safe to use/discard)
            if (card.IsCode(Anime_SayerExecutor.CardId.HushedPsychicMinister)) return 10;
            if (card.IsCode(Anime_SayerExecutor.CardId.SerenePsychicGirl)) return 8;
            if (card.IsCode(Anime_SayerExecutor.CardId.Krebons)) return 8;
            if (card.IsCode(Anime_SayerExecutor.CardId.PsychicCommander)) return 12;
            if (card.IsCode(Anime_SayerExecutor.CardId.PsychicWheeleder)) return 15;
            if (card.IsCode(Anime_SayerExecutor.CardId.PsychicTracker)) return 15;
            if (card.IsCode(Anime_SayerExecutor.CardId.SilentPsychicWizard)) return 18;

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

            // Prefer discarding cards with GY effects
            var minister = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_SayerExecutor.CardId.HushedPsychicMinister));
            if (minister != null) return minister;

            var girl = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_SayerExecutor.CardId.SerenePsychicGirl));
            if (girl != null) return girl;

            var wizard = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_SayerExecutor.CardId.SilentPsychicWizard));
            if (wizard != null) return wizard;

            var omnibuster = candidates.FirstOrDefault(c => c != null && c.IsCode(Anime_SayerExecutor.CardId.PsychicOmnibuster));
            if (omnibuster != null) return omnibuster;

            // Sort by lowest material cost
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderBy(c => GetMaterialCost(c)).FirstOrDefault();
        }
    }

    public class Anime_SayerThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly Anime_SayerExecutor _exec;

        public Anime_SayerThreatEvaluator(Anime_SayerExecutor exec)
        {
            _exec = exec;
        }

        public int EvaluateThreatScore(ClientCard card)
        {
            if (card == null || card.Controller == 0) return 0;

            int score = 0;

            // Anti-Spell / Anti-Synchro / Floodgates that shut down Psychic decks
            int[] antiPsychicCards = {
                51452091, // Imperial Order
                82044279, // Skill Drain
                40605147, // Solemn Strike
                84749824, // Solemn Warning
                41420027, // Solemn Judgment
                58851034, // Anti-Spell Fragrance
                10833828, // Dimensional Barrier
                53347303, // There Can Be Only One
                15693423  // Summon Limit
            };

            if (antiPsychicCards.Contains(card.Id))
            {
                score += 50;
            }

            // Big beatsticks with ATK > 2800
            if (card.IsFaceup() && card.Attack >= 2800)
            {
                score += 30;
            }

            return score;
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null || card.Controller == 0) return false;

            // Direct game-ending floodgates
            if (card.IsFaceup() && (card.Id == 82044279 || card.Id == 53347303 || card.Id == 15693423))
                return true;

            return false;
        }
    }
}
