using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("RyzealBlitz", "RyzealBlitzExecutor")]
    public class RyzealBlitzExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Spells
            public const int BlitzcliqueSteppleader = 433377;
            public const int UltimateSlayer = 2263869;
            public const int HeavyStorm = 19613556;
            public const int PairBearScare = 21501961;
            public const int CalledByTheGrave = 24224830;
            public const int TripleTacticsTalent = 25311006;
            public const int IllusionGate = 33017964;
            public const int Coulomb = 37654623;
            public const int BlitzcliqueBreakaway = 64049762;
            public const int Terraforming = 73628505;
            public const int PotOfProsperity = 84211599;

            // Main Traps
            public const int BlitzcliqueReturnStroke = 23526128;
            public const int SolemnJudgment = 41420027;
            public const int BlitzcliqueAlternator = 59921227;

            // Main Monsters
            public const int IceRyzeal = 8633261;
            public const int EmiBlitzclique = 11895663;
            public const int AshBlossom = 14558127;
            public const int SurgeBlitzclique = 22912101;
            public const int MaxxC = 23434538;
            public const int BystialMagnamhut = 33854624;
            public const int ExtRyzeal = 34022970;
            public const int ArtifactLancea = 34267821;
            public const int SwordRyzeal = 35844557;
            public const int SantaClaws = 46565218;
            public const int CrackleBlitzclique = 58916810;
            public const int GhostOgre = 59438930;
            public const int FydraulisHarmonia = 70088809;
            public const int DinowrestlerPankratops = 82385847;
            public const int MulcharmyPurulia = 84192580;
            public const int GrainBlitzclique = 84401954;
            public const int WhiskerBlitzclique = 85523502;
            public const int DimensionShifter = 91800273;

            // Extra Deck
            public const int MereologicAggregator = 9940036;
            public const int Garura = 11765832;
            public const int BorreloadSavageDragon = 27548199;
            public const int TopologicBlasterDragon = 30064423;
            public const int RyzealDetonator = 34909328;
            public const int VallonSuperPsy = 40673853;
            public const int Number106GiantHand = 63746411;
            public const int Number60Dugares = 66011101;
            public const int BorrelcodeDragon = 67288539;
            public const int EnigmasterPackbit = 72444406;
            public const int PSYFramelordOmega = 74586817;
            public const int Zeus = 90448279;
            public const int Typhon = 93039339;
            public const int GoldenCloudBeastMalong = 93125329;
            public const int WindPegasus = 98506199;
        }

        private readonly RyzealBlitzPlugin _plugin;

        public RyzealBlitzExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _plugin = new RyzealBlitzPlugin(this);

            // Tier 1: Handtraps & Counter Disruption
            RegisterHandtraps();

            // Tier 2: Board Breakers (Going Second tools)
            RegisterBoardBreakers();

            // Tier 3: Searchers & Setup Spells
            RegisterSearchersAndSpells();

            // Tier 4: Monsters Special & Normal Summons
            RegisterMonsterSummons();

            // Tier 5: Extra Deck Xyz Summons
            RegisterExtraDeckSummons();

            // Tier 6: Field Spells, Traps & Disruption Activations
            RegisterOnFieldDisruptions();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _plugin.ResetTurnState();
        }

        // ═══════════════════════════════════════════════════════════════
        //  REGISTRATION HELPERS
        // ═══════════════════════════════════════════════════════════════
        private void RegisterHandtraps()
        {
            // Maxx "C"
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, () =>
            {
                return Duel.Player == 1;
            });

            // Mulcharmy Purulia
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, () =>
            {
                return Duel.Player == 1 && Bot.GetMonsterCount() == 0;
            });

            // Ash Blossom
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, () => DefaultAshBlossomAndJoyousSpring());

            // Ghost Ogre
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, () =>
            {
                return Duel.LastChainPlayer == 1;
            });

            // Artifact Lancea
            AddExecutor(ExecutorType.Activate, CardId.ArtifactLancea, () =>
            {
                return Duel.Player == 1;
            });

            // Called by the Grave
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, () => DefaultCalledByTheGrave());

            // Solemn Judgment
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, () => DefaultSolemnJudgment());

            // Crackle Blitzclique (Quick hand disruption on opponent's turn)
            AddExecutor(ExecutorType.Activate, CardId.CrackleBlitzclique, () =>
            {
                if (Duel.Player != 1) return false;
                var target = _plugin.ThreatEvaluator.GetBestMonsterDestructionTarget();
                return target != null;
            });
        }

        private void RegisterBoardBreakers()
        {
            // Santa Claws (Tribute opponent boss monster)
            AddExecutor(ExecutorType.SpSummon, CardId.SantaClaws, () =>
            {
                if (Duel.Player == 1) return false;
                var enemyMonsters = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup());
                return enemyMonsters.Any(m => m.Attack >= 2500 || CardIntelligence.IsKnownNegator(m.Id) || CardIntelligence.IsFloodgate(m.Id));
            });

            // Dinowrestler Pankratops
            AddExecutor(ExecutorType.SpSummon, CardId.DinowrestlerPankratops, () =>
            {
                return Enemy.GetMonsterCount() > Bot.GetMonsterCount();
            });

            AddExecutor(ExecutorType.Activate, CardId.DinowrestlerPankratops, () =>
            {
                var target = _plugin.ThreatEvaluator.GetBestDestructionTarget();
                return target != null;
            });

            // Heavy Storm
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, () =>
            {
                return Enemy.GetSpellCount() >= 2 || (Enemy.GetSpellCount() >= 1 && Bot.GetSpellCount() == 0);
            });

            // Ultimate Slayer
            AddExecutor(ExecutorType.Activate, CardId.UltimateSlayer, () =>
            {
                if (Enemy.GetMonsterCount() == 0) return false;
                var enemyMonsters = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
                // Check if we have matching extra deck card type
                return enemyMonsters.Any(m => m.HasType(CardType.Xyz) || m.HasType(CardType.Fusion) || m.HasType(CardType.Synchro) || m.HasType(CardType.Link));
            });

            // Illusion Gate
            AddExecutor(ExecutorType.Activate, CardId.IllusionGate, () =>
            {
                return Enemy.GetMonsterCount() >= 2 && Bot.LifePoints > 2000;
            });

            // Triple Tactics Talent
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, () =>
            {
                return true;
            });
        }

        private void RegisterSearchersAndSpells()
        {
            // Terraforming -> Coulomb
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, () =>
            {
                return !Bot.HasInHand(CardId.Coulomb);
            });

            // Pot of Prosperity
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, () =>
            {
                return Bot.Hand.Count <= 4;
            });

            // Hideout in the Sky, Coulomb (Field Spell)
            AddExecutor(ExecutorType.Activate, CardId.Coulomb, () =>
            {
                // Can activate to summon token to enemy field and search Blitzclique
                return Bot.GetMonsterCount() < 5;
            });

            // Blitzclique - Steppleader (Continuous Spell)
            AddExecutor(ExecutorType.Activate, CardId.BlitzcliqueSteppleader, () =>
            {
                // Can summon Thunder from hand then self-destructs
                return Bot.Hand.Any(c => c != null && (c.Race & (int)CardRace.Thunder) != 0 && c.Id != CardId.EmiBlitzclique);
            });

            // Blitzclique - Breakaway
            AddExecutor(ExecutorType.Activate, CardId.BlitzcliqueBreakaway, () =>
            {
                // Place Return Stroke or Alternator if not on field, or pop
                if (!Bot.HasInSpellZone(CardId.BlitzcliqueReturnStroke)) return true;
                return _plugin.ThreatEvaluator.GetBestDestructionTarget() != null && _plugin.Strategy.HasThunderOnField();
            });

            // Pair Bear Scare!!
            AddExecutor(ExecutorType.Activate, CardId.PairBearScare, () =>
            {
                return Enemy.GetMonsterCount() > 0;
            });
        }

        private void RegisterMonsterSummons()
        {
            // Surge Blitzclique (hand reveal pop & SS)
            AddExecutor(ExecutorType.Activate, CardId.SurgeBlitzclique, () =>
            {
                if (Card.Location == CardLocation.Hand)
                {
                    // Target a monster on field to destroy and SS Thunder from hand
                    var target = _plugin.ThreatEvaluator.GetBestMonsterDestructionTarget();
                    if (target != null) return true;
                    // Can also pop enemy Coulomb token
                    return Enemy.GetMonsters().Any(m => m != null && m.IsFaceup());
                }
                // Field trigger when another card is destroyed: search Blitzclique
                return true;
            });

            // Grain Blitzclique (hand reveal pop spell & SS)
            AddExecutor(ExecutorType.Activate, CardId.GrainBlitzclique, () =>
            {
                if (Card.Location == CardLocation.Hand)
                {
                    var target = _plugin.ThreatEvaluator.GetBestSpellTrapDestructionTarget();
                    return target != null;
                }
                return true;
            });

            // Whisker Blitzclique
            AddExecutor(ExecutorType.Activate, CardId.WhiskerBlitzclique, () =>
            {
                if (Card.Location == CardLocation.Hand)
                {
                    // Quick SS up to 3 Thunder from hand and pop
                    return Bot.Hand.Count(c => c != null && (c.Race & (int)CardRace.Thunder) != 0) >= 2;
                }
                // On field quick negate monster effect
                if (Duel.LastChainPlayer == 1)
                {
                    return _plugin.Strategy.HasThunderOnField();
                }
                return false;
            });

            // Emi Blitzclique
            AddExecutor(ExecutorType.Activate, CardId.EmiBlitzclique, () =>
            {
                return true;
            });

            // Ice Ryzeal (Normal Summon Priority 1)
            AddExecutor(ExecutorType.Summon, CardId.IceRyzeal, () =>
            {
                return true;
            });

            AddExecutor(ExecutorType.Activate, CardId.IceRyzeal, () =>
            {
                // When Normal Summoned: SS Ryzeal from deck
                // From hand: send 1 card to GY to SS
                return true;
            });

            // Ext Ryzeal (Send Xyz from Extra to GY to SS)
            AddExecutor(ExecutorType.Activate, CardId.ExtRyzeal, () =>
            {
                return true;
            });

            AddExecutor(ExecutorType.Summon, CardId.ExtRyzeal, () =>
            {
                return !_plugin.BoardAssessor.HasDetonatorOnField();
            });

            // Sword Ryzeal
            AddExecutor(ExecutorType.Activate, CardId.SwordRyzeal, () =>
            {
                return true;
            });

            AddExecutor(ExecutorType.Summon, CardId.SwordRyzeal, () =>
            {
                return true;
            });

            // Surge Blitzclique Normal Summon (if no Ryzeal)
            AddExecutor(ExecutorType.Summon, CardId.SurgeBlitzclique, () =>
            {
                return Bot.GetMonsterCount() < 5;
            });

            // Grain Blitzclique Normal Summon
            AddExecutor(ExecutorType.Summon, CardId.GrainBlitzclique, () =>
            {
                return Bot.GetMonsterCount() < 5 && !Bot.HasInHand(CardId.IceRyzeal);
            });

            // Bystial Magnamhut
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, () =>
            {
                return Enemy.Graveyard.Any(c => c != null && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
            });
        }

        private void RegisterExtraDeckSummons()
        {
            // Super Starslayer TY-PHON (Overlay on high threat)
            AddExecutor(ExecutorType.SpSummon, CardId.Typhon, () =>
            {
                return Enemy.GetMonsters().Any(m => m != null && m.Attack >= 3000);
            });

            AddExecutor(ExecutorType.Activate, CardId.Typhon, () =>
            {
                return Enemy.GetMonsterCount() > 0;
            });

            // Divine Arsenal AA-ZEUS (MP2 wipe)
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, () =>
            {
                return Duel.Turn > 1 && Bot.GetMonsters().Any(m => m != null && m.HasType(CardType.Xyz));
            });

            AddExecutor(ExecutorType.Activate, CardId.Zeus, () =>
            {
                return (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) >= 2;
            });

            // Ryzeal Detonator (Rank 4 Ace Boss!)
            AddExecutor(ExecutorType.SpSummon, CardId.RyzealDetonator, () =>
            {
                return !_plugin.BoardAssessor.HasDetonatorOnField();
            });

            AddExecutor(ExecutorType.Activate, CardId.RyzealDetonator, () =>
            {
                // When Special Summoned: attach 1 monster from GY
                if (Card.Location == CardLocation.MonsterZone)
                {
                    // Quick pop on opponent activation or attach
                    if (Duel.LastChainPlayer == 1)
                    {
                        var target = _plugin.ThreatEvaluator.GetBestDestructionTarget();
                        return target != null;
                    }
                    return true;
                }
                return false;
            });

            // Number 106: Giant Hand (Rank 4 Monster Negate)
            AddExecutor(ExecutorType.SpSummon, CardId.Number106GiantHand, () =>
            {
                return _plugin.BoardAssessor.HasDetonatorOnField() && !_plugin.BoardAssessor.HasGiantHandOnField();
            });

            AddExecutor(ExecutorType.Activate, CardId.Number106GiantHand, () =>
            {
                return Duel.LastChainPlayer == 1 && Enemy.GetMonsters().Any(m => m != null && m.IsFaceup());
            });

            // Number 60: Dugares the Timeless (Draw or Double ATK)
            AddExecutor(ExecutorType.SpSummon, CardId.Number60Dugares, () =>
            {
                return _plugin.BoardAssessor.CanOTK() || Bot.Hand.Count <= 2;
            });

            AddExecutor(ExecutorType.Activate, CardId.Number60Dugares, () =>
            {
                return true;
            });
        }

        private void RegisterOnFieldDisruptions()
        {
            // Blitzclique Return Stroke (Continuous Trap)
            AddExecutor(ExecutorType.Activate, CardId.BlitzcliqueReturnStroke, () =>
            {
                if (Card.Location == CardLocation.SpellZone)
                {
                    // Quick negate Spell when opponent activates
                    if (Duel.LastChainPlayer == 1)
                    {
                        return _plugin.Strategy.HasThunderOnField();
                    }
                    return true; // Flip face up
                }
                if (Card.Location == CardLocation.Grave)
                {
                    // Banish to return Blitzclique to hand
                    return true;
                }
                return false;
            });

            // Blitzclique Alternator (Continuous Trap)
            AddExecutor(ExecutorType.Activate, CardId.BlitzcliqueAlternator, () =>
            {
                return true;
            });

            // Mereologic Aggregator (GY negate triggered by Ext Ryzeal or Ultimate Slayer)
            AddExecutor(ExecutorType.Activate, CardId.MereologicAggregator, () =>
            {
                return Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup());
            });

            // Golden Cloud Beast - Malong (GY bounce)
            AddExecutor(ExecutorType.Activate, CardId.GoldenCloudBeastMalong, () =>
            {
                return Enemy.GetMonsters().Concat(Enemy.GetSpells()).Any(c => c != null && c.IsFaceup());
            });

            // Garura (GY draw)
            AddExecutor(ExecutorType.Activate, CardId.Garura, () =>
            {
                return true;
            });

            // Wind Pegasus (GY bounce)
            AddExecutor(ExecutorType.Activate, CardId.WindPegasus, () =>
            {
                return (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) > 0;
            });

            // Spells/Traps to set
            AddExecutor(ExecutorType.SpellSet, CardId.BlitzcliqueReturnStroke);
            AddExecutor(ExecutorType.SpellSet, CardId.BlitzcliqueAlternator);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.BlitzcliqueBreakaway);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.PairBearScare);
        }

        // ═══════════════════════════════════════════════════════════════
        //  PRECISE CARD & OPTION SELECTION OVERRIDES
        // ═══════════════════════════════════════════════════════════════
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Hint 502 / 503 / 504: Destroy / Removal target -> Target enemy's highest threat
            if (hint == 502 || hint == 503 || hint == 504 || hint == 505 || hint == 507)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Any())
                {
                    return enemyCards
                        .OrderByDescending(c => _plugin.ThreatEvaluator.EvaluateTargetPriority(c))
                        .Take(max)
                        .ToList();
                }
            }

            // Hint: Tribute for Santa Claws
            var enemyBoss = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (enemyBoss != null && Card?.Id == CardId.SantaClaws)
            {
                return new List<ClientCard> { enemyBoss };
            }

            // Send from Extra Deck for Ext Ryzeal or Ultimate Slayer
            if (cards.All(c => c != null && c.Location == CardLocation.Extra))
            {
                // If Mereologic Aggregator available, send it to negate an enemy card
                var aggregator = cards.FirstOrDefault(c => c.Id == CardId.MereologicAggregator);
                if (aggregator != null) return new List<ClientCard> { aggregator };

                // If Garura available, send it to draw
                var garura = cards.FirstOrDefault(c => c.Id == CardId.Garura);
                if (garura != null) return new List<ClientCard> { garura };

                // Malong for bounce
                var malong = cards.FirstOrDefault(c => c.Id == CardId.GoldenCloudBeastMalong);
                if (malong != null) return new List<ClientCard> { malong };

                var omega = cards.FirstOrDefault(c => c.Id == CardId.PSYFramelordOmega);
                if (omega != null) return new List<ClientCard> { omega };
            }

            // Detach Xyz Material
            if (cards.All(c => c != null && c.Location == CardLocation.Overlay))
            {
                return cards
                    .OrderByDescending(c => _plugin.MaterialScorer.GetDetachPriority(c))
                    .Take(min)
                    .ToList();
            }

            // Discard to GY
            if (hint == 501)
            {
                return cards
                    .OrderBy(c => _plugin.MaterialScorer.GetHandDiscardPriority(c))
                    .Take(min)
                    .ToList();
            }

            // Search to Hand (Hint 506)
            if (hint == 506)
            {
                // Coulomb search: prioritize Whisker > Surge > Crackle > Grain
                var whisker = cards.FirstOrDefault(c => c.Id == CardId.WhiskerBlitzclique);
                if (whisker != null && !Bot.HasInHand(CardId.WhiskerBlitzclique)) return new List<ClientCard> { whisker };

                var surge = cards.FirstOrDefault(c => c.Id == CardId.SurgeBlitzclique);
                if (surge != null && !Bot.HasInHand(CardId.SurgeBlitzclique)) return new List<ClientCard> { surge };

                var crackle = cards.FirstOrDefault(c => c.Id == CardId.CrackleBlitzclique);
                if (crackle != null && !Bot.HasInHand(CardId.CrackleBlitzclique)) return new List<ClientCard> { crackle };

                // Ryzeal search: prioritize Ice > Sword > Ext
                var ice = cards.FirstOrDefault(c => c.Id == CardId.IceRyzeal);
                if (ice != null && !Bot.HasInHand(CardId.IceRyzeal)) return new List<ClientCard> { ice };

                var sword = cards.FirstOrDefault(c => c.Id == CardId.SwordRyzeal);
                if (sword != null && !Bot.HasInHand(CardId.SwordRyzeal)) return new List<ClientCard> { sword };

                var ext = cards.FirstOrDefault(c => c.Id == CardId.ExtRyzeal);
                if (ext != null && !Bot.HasInHand(CardId.ExtRyzeal)) return new List<ClientCard> { ext };
            }

            // Special Summon from Deck (Hint 509)
            if (hint == 509)
            {
                // From Ice Ryzeal: SS Sword or Ext
                var sword = cards.FirstOrDefault(c => c.Id == CardId.SwordRyzeal);
                if (sword != null) return new List<ClientCard> { sword };

                var ext = cards.FirstOrDefault(c => c.Id == CardId.ExtRyzeal);
                if (ext != null) return new List<ClientCard> { ext };
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0)
                return base.OnSelectOption(options);

            // Dugares the Timeless:
            // Option 0: Skip Draw Phase, Draw 2 Discard 1
            // Option 1: Skip Main Phase 1, SS from GY
            // Option 2: Skip Battle Phase, Double ATK
            if (Card?.Id == CardId.Number60Dugares)
            {
                if (_plugin.BoardAssessor.CanOTK() && options.Count > 2) return 2; // Double ATK
                return 0; // Draw 2 discard 1
            }

            // Blitzclique - Breakaway:
            // Option 0: Place Continuous Trap face-up
            // Option 1: Bounce Thunder and destroy
            if (Card?.Id == CardId.BlitzcliqueBreakaway)
            {
                if (!Bot.HasInSpellZone(CardId.BlitzcliqueReturnStroke)) return 0;
                return 1;
            }

            return base.OnSelectOption(options);
        }
    }
}
