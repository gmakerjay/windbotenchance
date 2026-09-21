// ============================================================================
// Yubel2Executor.cs — YugiohTH Rule-Based ModernExecutor (Yubel-Fiendsmith-SacredBeast)
// Architecture: ModernExecutor v10.0 (O(1) Central Intelligence, ComboRouter, Hint Tables)
// Strategy: Nightmare Pain Reflection OTK + Samsara Lotus Negate + Loving Defender Board Wipe
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Yubel2", "Yubel2")]
    public class Yubel2Executor : ModernExecutor
    {
        public class CardId
        {
            // --- Yubel Archetype ---
            public const int ElementalHERONeos = 89943723;
            public const int YubelTheUltimateNightmare = 31764700;
            public const int YubelTerrorIncarnate = 4779091;
            public const int SpiritOfYubel = 90829280;
            public const int Yubel = 78371393;
            public const int GeistgrinderGolem = 26913989;
            public const int GruesomeGraveSquirmer = 24215921;
            public const int SamsaraDLotus = 62318994;
            public const int NightmarePain = 65261141;
            public const int NightmareThrone = 93729896;
            public const int EternalFavorite = 87532344;
            public const int YubelTheLovingDefenderForever = 47172959;
            public const int ElementalHERONeosKluger = 90307498;

            // --- Fiendsmith Engine ---
            public const int FiendsmithEngraver = 60764609;
            public const int LacrimaTheCrimsonTears = 28803166;
            public const int FabledLurrie = 97651498;
            public const int FiendsmithsTract = 98567237;
            public const int FiendsmithsDesirae = 82135803;
            public const int FiendsmithsLacrima = 46640168;

            // --- Sacred Beast Engine ---
            public const int DarkBeckoningBeast = 81034083;
            public const int OpeningOfTheSpiritGates = 80312545;

            // --- Board Breakers, Fusions & Spells ---
            public const int DarkHole = 53129443;
            public const int FusionDeployment = 6498706;
            public const int MutinyInTheSky = 71593652;
            public const int FinalBringerOfTheEndTimes = 54261514;
            public const int ForbiddenDroplet = 24299458;
            public const int SuperPolymerization = 48130397;

            // --- Extra Deck Bosses & Utilities ---
            public const int LuceTheDusksDark = 45409943;
            public const int StarvingVenomFusionDragon = 41209827;
            public const int AerialEater = 28143384;
            public const int TheDukeOfDemise = 45445571;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int ChaosAngel = 22850702;
            public const int SuperdreadnoughtRailCannonSuperDora = 49032236;
            public const int SuperdreadnoughtRailCannonGustavMax = 56910167;
        }

        // Ace cards that must never be casually sacrificed or used as generic material
        private static readonly int[] AceCardIds = {
            CardId.YubelTheLovingDefenderForever,
            CardId.ElementalHERONeosKluger,
            CardId.YubelTerrorIncarnate,
            CardId.YubelTheUltimateNightmare,
            CardId.Yubel,
            CardId.ChaosAngel,
            CardId.SuperdreadnoughtRailCannonGustavMax,
            CardId.SuperdreadnoughtRailCannonSuperDora
        };

        // Yubel family cards for floating, reflection, and material checks
        private static readonly int[] YubelMonsters = {
            CardId.Yubel,
            CardId.SpiritOfYubel,
            CardId.YubelTerrorIncarnate,
            CardId.YubelTheUltimateNightmare,
            CardId.YubelTheLovingDefenderForever
        };

        // Once-per-turn activation guards
        private bool _throneUsed = false;
        private bool _painUsed = false;
        private bool _samsaraUsed = false;
        private bool _squirmerHandUsed = false;
        private bool _squirmerGraveUsed = false;
        private bool _gatesHandUsed = false;
        private bool _gatesFieldUsed = false;
        private bool _beckoningUsed = false;
        private bool _engraverHandUsed = false;
        private bool _engraverGraveUsed = false;
        private bool _tractHandUsed = false;
        private bool _tractGraveUsed = false;
        private bool _favoriteUsed = false;
        private bool _deploymentUsed = false;
        private bool _mutinyUsed = false;
        private bool _finalBringerUsed = false;
        private bool _geistgrinderHandUsed = false;

        private int GetFreeMonsterZoneCount()
        {
            int count = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (Bot.MonsterZone[i] == null) count++;
            }
            return count;
        }

        public Yubel2Executor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // 1. Central Resource & Ace Protections
            ResourcePlan.RegisterAceCards(AceCardIds);
            HeuristicGuard.RegisterAceCards(AceCardIds);

            // 2. Combo Router Registrations
            // Route A: Primary Yubel Engine (Lotus / Throne -> Spirit of Yubel -> Pain -> Yubel + Lotus loop)
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Yubel-Main-Lotus-Loop",
                RequiredCards = new List<int> { CardId.SamsaraDLotus },
                FallbackLineName = "Sacred-Beast-Setup",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.SamsaraDLotus, ActionType = ExecutorType.Summon, Description = "Summon Samsara Lotus" },
                    new() { CardId = CardId.SamsaraDLotus, ActionType = ExecutorType.Activate, Description = "Tribute Lotus to SS Spirit of Yubel" },
                    new() { CardId = CardId.SpiritOfYubel, ActionType = ExecutorType.Activate, Description = "Set Nightmare Pain directly" },
                    new() { CardId = CardId.NightmarePain, ActionType = ExecutorType.Activate, Description = "Destroy Spirit to search Squirmer/Favorite & float into Yubel" }
                },
                EndBoardScore = 95
            });

            // Route B: Sacred Beast Searcher Setup (Dark Beckoning Beast -> Gates -> Lotus extra summon)
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Sacred-Beast-Setup",
                RequiredCards = new List<int> { CardId.DarkBeckoningBeast },
                FallbackLineName = "Fiendsmith-Engraver-Line",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.DarkBeckoningBeast, ActionType = ExecutorType.Summon, Description = "NS Beckoning, search Gates" },
                    new() { CardId = CardId.OpeningOfTheSpiritGates, ActionType = ExecutorType.Activate, Description = "Activate Gates, search Lotus" },
                    new() { CardId = CardId.SamsaraDLotus, ActionType = ExecutorType.Summon, Description = "Extra NS Samsara Lotus" }
                },
                EndBoardScore = 90
            });

            // Route C: Fiendsmith Extender Line
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Fiendsmith-Engraver-Line",
                RequiredCards = new List<int> { CardId.FiendsmithEngraver },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.FiendsmithEngraver, ActionType = ExecutorType.Activate, Description = "Discard Engraver to search Tract" },
                    new() { CardId = CardId.FiendsmithsTract, ActionType = ExecutorType.Activate, Description = "Search Lurrie, discard to SS Lurrie" }
                },
                EndBoardScore = 70,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.FiendsmithEngraver)
            });

            // Route D: Geistgrinder Golem Reflection OTK
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Geistgrinder-Reflection-OTK",
                RequiredCards = new List<int> { CardId.GeistgrinderGolem, CardId.NightmarePain },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.NightmarePain, ActionType = ExecutorType.Activate, Description = "Activate Nightmare Pain for reflection damage" },
                    new() { CardId = CardId.GeistgrinderGolem, ActionType = ExecutorType.Activate, Description = "Give 3000 ATK Golem to enemy, SS Yubel to our field" }
                },
                EndBoardScore = 100,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.GeistgrinderGolem) &&
                                  Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != null && YubelMonsters.Contains(c.Id))
            });

            // 3. Register Starters & Baits
            BaitPlanner.RegisterComboStarters(CardId.NightmareThrone, CardId.SamsaraDLotus, CardId.DarkBeckoningBeast, CardId.OpeningOfTheSpiritGates);
            BaitPlanner.RegisterBaitCards(CardId.DarkHole, CardId.ForbiddenDroplet, CardId.FusionDeployment);
            ChainAdvisor.RegisterHighValueTargets(CardId.NightmarePain, CardId.OpeningOfTheSpiritGates, CardId.NightmareThrone, CardId.SamsaraDLotus);

            // ========================================================================
            // TIER 1: Quick-Play Spells & Interruption Traps (Highest Priority)
            // ========================================================================
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, DropletActivate);
            AddExecutor(ExecutorType.Activate, CardId.EternalFavorite, EternalFavoriteActivate);
            AddExecutor(ExecutorType.Activate, CardId.FinalBringerOfTheEndTimes, FinalBringerActivate);

            // ========================================================================
            // TIER 2: Opponent Turn Disruptions (Samsara Negate & Quick Effects)
            // ========================================================================
            AddExecutor(ExecutorType.Activate, CardId.SamsaraDLotus, LotusOpponentNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.SpiritOfYubel, SpiritOfYubelAttackResponse);
            AddExecutor(ExecutorType.Activate, CardId.LacrimaTheCrimsonTears, LacrimaGraveQuickActivate);

            // ========================================================================
            // TIER 3: Field Spells & Main Combo Starters (Turn 1 / Main Phase)
            // ========================================================================
            AddExecutor(ExecutorType.Activate, CardId.NightmareThrone, ThroneActivate);
            AddExecutor(ExecutorType.Activate, CardId.OpeningOfTheSpiritGates, GatesActivate);
            AddExecutor(ExecutorType.Activate, CardId.NightmarePain, NightmarePainActivate);
            AddExecutor(ExecutorType.Activate, CardId.FusionDeployment, FusionDeploymentActivate);

            // ========================================================================
            // TIER 4: Yubel Engine Core Plays & Floats
            // ========================================================================
            AddExecutor(ExecutorType.Activate, CardId.SamsaraDLotus, LotusMainPhaseActivate);
            AddExecutor(ExecutorType.Activate, CardId.GruesomeGraveSquirmer, GraveSquirmerActivate);
            AddExecutor(ExecutorType.Activate, CardId.SpiritOfYubel, SpiritOfYubelOnSummonOrFloatActivate);
            AddExecutor(ExecutorType.Activate, CardId.Yubel, YubelFloatActivate);
            AddExecutor(ExecutorType.Activate, CardId.YubelTerrorIncarnate, YubelTerrorActivate);

            // ========================================================================
            // TIER 5: Fiendsmith & Board Breaker Spells
            // ========================================================================
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, EngraverActivate);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsTract, TractActivate);
            AddExecutor(ExecutorType.Activate, CardId.LacrimaTheCrimsonTears, LacrimaOnSummonActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarkHole, DarkHoleActivate);
            AddExecutor(ExecutorType.Activate, CardId.MutinyInTheSky, MutinyActivate);

            // ========================================================================
            // TIER 6: Normal Summons & Extra Summons
            // ========================================================================
            AddExecutor(ExecutorType.Summon, CardId.DarkBeckoningBeast, BeckoningBeastSummon);
            AddExecutor(ExecutorType.Summon, CardId.SamsaraDLotus, LotusSummon);
            AddExecutor(ExecutorType.Summon, CardId.GruesomeGraveSquirmer, GenericNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.LacrimaTheCrimsonTears, GenericNormalSummon);

            // ========================================================================
            // TIER 7: Special Summons (Contact Fusion, Extra Deck, Geistgrinder)
            // ========================================================================
            AddExecutor(ExecutorType.Activate, CardId.GeistgrinderGolem, GeistgrinderActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.YubelTheLovingDefenderForever, LovingDefenderSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, ChaosAngelSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LuceTheDusksDark, LuceContactSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsLacrima, LacrimaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AerialEater, AerialEaterSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TheDukeOfDemise, DukeOfDemiseSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ElementalHERONeosKluger, NeosKlugerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperdreadnoughtRailCannonGustavMax, GustavMaxSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperdreadnoughtRailCannonGustavMax, GustavMaxBurnActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperdreadnoughtRailCannonSuperDora, SuperDoraSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperdreadnoughtRailCannonSuperDora, SuperDoraProtectActivate);

            // ========================================================================
            // TIER 8: End Phase Loops & Safe Spell/Trap Sets
            // ========================================================================
            AddExecutor(ExecutorType.Activate, CardId.SamsaraDLotus, LotusEndPhaseReviveActivate);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.EternalFavorite, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.FinalBringerOfTheEndTimes, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Yubel excels going first to establish Lotus negate, Nightmare Pain, and Eternal Favorite
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _throneUsed = false;
            _painUsed = false;
            _samsaraUsed = false;
            _squirmerHandUsed = false;
            _squirmerGraveUsed = false;
            _gatesHandUsed = false;
            _gatesFieldUsed = false;
            _beckoningUsed = false;
            _engraverHandUsed = false;
            _engraverGraveUsed = false;
            _tractHandUsed = false;
            _tractGraveUsed = false;
            _favoriteUsed = false;
            _deploymentUsed = false;
            _mutinyUsed = false;
            _finalBringerUsed = false;
            _geistgrinderHandUsed = false;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.NightmareThrone && card.Location == CardLocation.Hand) _throneUsed = true;
                if (card.Id == CardId.NightmarePain && card.Location == CardLocation.SpellZone) _painUsed = true;
                if (card.Id == CardId.SamsaraDLotus) _samsaraUsed = true;
                if (card.Id == CardId.GruesomeGraveSquirmer)
                {
                    if (card.Location == CardLocation.Hand) _squirmerHandUsed = true;
                    if (card.Location == CardLocation.Grave) _squirmerGraveUsed = true;
                }
                if (card.Id == CardId.OpeningOfTheSpiritGates)
                {
                    if (card.Location == CardLocation.Hand) _gatesHandUsed = true;
                    if (card.Location == CardLocation.SpellZone) _gatesFieldUsed = true;
                }
                if (card.Id == CardId.DarkBeckoningBeast) _beckoningUsed = true;
                if (card.Id == CardId.FiendsmithEngraver)
                {
                    if (card.Location == CardLocation.Hand) _engraverHandUsed = true;
                    if (card.Location == CardLocation.Grave) _engraverGraveUsed = true;
                }
                if (card.Id == CardId.FiendsmithsTract)
                {
                    if (card.Location == CardLocation.Hand) _tractHandUsed = true;
                    if (card.Location == CardLocation.Grave) _tractGraveUsed = true;
                }
                if (card.Id == CardId.EternalFavorite) _favoriteUsed = true;
                if (card.Id == CardId.FusionDeployment) _deploymentUsed = true;
                if (card.Id == CardId.MutinyInTheSky) _mutinyUsed = true;
                if (card.Id == CardId.FinalBringerOfTheEndTimes) _finalBringerUsed = true;
            }
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 950;
            if (c.Id == CardId.ElementalHERONeos) return 800;
            if (c.Id == CardId.FabledLurrie) return 100;
            if (c.Id == CardId.DarkBeckoningBeast) return 200;
            if (c.Id == CardId.LacrimaTheCrimsonTears) return 250;
            if (c.Id == CardId.GruesomeGraveSquirmer) return 300;
            if (c.Id == CardId.SamsaraDLotus) return 350;
            return 500;
        }

        private bool HasLethalOnBoard()
        {
            if (Enemy.LifePoints <= 0) return true;
            bool hasPain = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == CardId.NightmarePain);
            if (hasPain)
            {
                // Under Nightmare Pain, any Yubel monster attacking an enemy monster deals that enemy's ATK as damage to opponent!
                int totalReflectDamage = 0;
                var enemyAtkMonsters = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && m.IsAttack()).OrderByDescending(m => m.Attack).ToList();
                var yubelAttackers = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && YubelMonsters.Contains(m.Id)).ToList();

                if (enemyAtkMonsters.Count > 0 && yubelAttackers.Count > 0)
                {
                    for (int i = 0; i < yubelAttackers.Count; i++)
                    {
                        var enemyTarget = enemyAtkMonsters[Math.Min(i, enemyAtkMonsters.Count - 1)];
                        totalReflectDamage += enemyTarget.Attack;
                        if (yubelAttackers[i].Id == CardId.YubelTheLovingDefenderForever)
                        {
                            totalReflectDamage += enemyTarget.Attack; // Loving Defender deals double damage (battle + burn)
                        }
                    }
                    if (totalReflectDamage >= Enemy.LifePoints) return true;
                }
            }

            int directAtk = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.IsAttack()).Sum(m => m.Attack);
            if (Enemy.GetMonsterCount() == 0 && directAtk >= Enemy.LifePoints) return true;
            return false;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (HasLethalOnBoard()) return true;

            int score = 0;
            if (Bot.HasInMonstersZone(CardId.YubelTheLovingDefenderForever)) score += 3;
            if (Bot.HasInMonstersZone(CardId.YubelTerrorIncarnate)) score += 2;
            if (Bot.HasInMonstersZone(CardId.Yubel) || Bot.HasInMonstersZone(CardId.SpiritOfYubel)) score += 2;
            if (Bot.HasInMonstersZone(CardId.SamsaraDLotus)) score += 2;
            if (Bot.HasInSpellZone(CardId.NightmarePain)) score += 2;
            if (Bot.GetSpells().Any(c => c != null && (c.IsFacedown() || c.IsFaceup()) && c.Id == CardId.EternalFavorite)) score += 2;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.Id == CardId.SuperPolymerization)) score += 2;
            if (Bot.HasInMonstersZone(CardId.ChaosAngel)) score += 2;

            return score >= 5;
        }

        protected override bool ShouldStopExtending()
        {
            if (HasLethalOnBoard()) return true;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return true;
            return base.ShouldStopExtending();
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            // Prevent Xyz summons (Gustav Max / Dora) from casually consuming Yubel / Spirit / Loving Defender unless for lethal
            if (card.HasType(CardType.Xyz))
            {
                if (Enemy.LifePoints > 2000 && !HasLethalOnBoard())
                {
                    return false;
                }
            }
            return true;
        }

        // ========================================================================
        // TIER 1 & 2: QUICK-PLAY SPELLS, TRAPS & INTERRUPTIONS
        // ========================================================================

        private bool SuperPolymerizationActivate()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            // Scenario 1: Loving Defender Forever (1 Yubel monster + 1+ effect monsters on field)
            bool hasYubel = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Any(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id));
            int effectMonsters = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Count(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect));
            int enemyEffectMonsters = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect));

            if (hasYubel && enemyEffectMonsters >= 1 && effectMonsters >= 2)
            {
                DecisionTracer.Trace("SuperPolymerizationActivate", "Super Poly into Loving Defender to clear enemy board!");
                return true;
            }

            // Scenario 2: Starving Venom (2 DARK monsters on field, except tokens)
            int darkMonsters = Bot.GetMonsters().Concat(Enemy.GetMonsters())
                .Count(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Dark) && !c.HasType(CardType.Token));
            int enemyDarkMonsters = Enemy.GetMonsters()
                .Count(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Dark) && !c.HasType(CardType.Token));
            if (enemyDarkMonsters >= 1 && darkMonsters >= 2)
            {
                DecisionTracer.Trace("SuperPolymerizationActivate", "Super Poly into Starving Venom");
                return true;
            }

            // Scenario 3: Garura (2 monsters with same type/attribute, different names)
            if (Enemy.GetMonsterCount() >= 2)
            {
                DecisionTracer.Trace("SuperPolymerizationActivate", "Super Poly into Garura");
                return true;
            }

            // Scenario 4: Fiendsmith's Lacrima (2 LIGHT Fiends)
            int lightFiends = Bot.GetMonsters().Concat(Enemy.GetMonsters())
                .Count(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend));
            if (lightFiends >= 2 && Enemy.GetMonsterCount() > 0)
            {
                DecisionTracer.Trace("SuperPolymerizationActivate", "Super Poly into Fiendsmith's Lacrima");
                return true;
            }

            return false;
        }

        private bool DropletActivate()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            var problematic = Util.GetProblematicEnemyMonster();
            if (problematic != null && !problematic.IsDisabled() && IsViableEffectTarget(problematic))
            {
                AI.SelectCard(problematic);
                DecisionTracer.Trace("DropletActivate", $"Droplet targeting {problematic.Name}");
                return true;
            }
            return false;
        }

        private bool EternalFavoriteActivate()
        {
            if (_favoriteUsed) return false;

            // If facedown on our turn, do not trigger prematurely
            if (Card.IsFacedown() && Duel.Player == 0) return false;

            bool hasYubelOnField = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Any(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id));
            int enemyMonsters = Enemy.GetMonsterCount();

            // Mode 1: Fusion Summon (wipe enemy field) when opponent controls monsters and we have discard
            if (hasYubelOnField && enemyMonsters >= 1 && Bot.Hand.Count > 0 && !IsSpecialSummonBlocked())
            {
                AI.SelectOption(1);
                _favoriteUsed = true;
                DecisionTracer.Trace("EternalFavoriteActivate", "Fusion Summon via Eternal Favorite using enemy monsters!");
                return true;
            }

            // Mode 0: Special Summon Yubel from GY or banished
            bool hasGYBanishYubel = Bot.Graveyard.Concat(Bot.Banished).Any(c => c != null && YubelMonsters.Contains(c.Id));
            if (hasGYBanishYubel && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
            {
                AI.SelectOption(0);
                _favoriteUsed = true;
                DecisionTracer.Trace("EternalFavoriteActivate", "Revive Yubel from GY/Banished via Eternal Favorite");
                return true;
            }

            return false;
        }

        private bool FinalBringerActivate()
        {
            if (_finalBringerUsed) return false;

            // Quick-play: Destroy 1 monster we control and 1 card enemy controls
            var enemyTarget = Util.GetProblematicEnemyCard();
            if (enemyTarget == null)
            {
                enemyTarget = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && (c.IsCode(48680970) || c.IsCode(47222536) || c.IsCode(68462976))) // Eternal Soul / Circle / Secret Village
                           ?? Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c != null && c.IsFaceup());
            }

            if (enemyTarget != null)
            {
                // Target our Spirit of Yubel or Yubel (they FLOAT when destroyed!)
                var ourTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.SpiritOfYubel)
                             ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.Yubel)
                             ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id));

                if (ourTarget != null)
                {
                    AI.SelectCard(ourTarget);
                    AI.SelectNextCard(enemyTarget);
                    _finalBringerUsed = true;
                    DecisionTracer.Trace("FinalBringerActivate", $"Destroy our {ourTarget.Name} (floats!) and enemy {enemyTarget.Name}");
                    return true;
                }
            }
            return false;
        }

        // ========================================================================
        // SAMSARA D LOTUS — OPPONENT TURN INTERRUPTION (CRITICAL WIN-CON)
        // ========================================================================

        private bool LotusOpponentNegateActivate()
        {
            // Only triggers on opponent's turn when a monster effect is activated
            if (Duel.Player != 1) return false;
            if (Card.Location != CardLocation.MonsterZone || !Card.IsFaceup()) return false;

            ClientCard last = Util.GetLastChainCard();
            if (last != null && last.Controller == 1 && (last.IsMonster() || last.HasType(CardType.Monster)))
            {
                bool controlYubel = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id) && c != Card);
                if (controlYubel)
                {
                    DecisionTracer.Trace("LotusOpponentNegateActivate", $"Tribute Samsara Lotus to negate/change {last.Name}'s effect into destroying Yubel!");
                    return true;
                }
            }
            return false;
        }

        private bool SpiritOfYubelAttackResponse()
        {
            // Quick effect: SS from hand when opponent declares an attack
            if (Card.Location == CardLocation.Hand && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle))
            {
                if (GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                {
                    DecisionTracer.Trace("SpiritOfYubelAttackResponse", "SS Spirit of Yubel from hand upon attack declaration!");
                    return true;
                }
            }
            return false;
        }

        private bool LacrimaGraveQuickActivate()
        {
            // Opponent turn quick effect: shuffle self into deck to revive a Fiendsmith Link (or Fiendsmith monster)
            if (Duel.Player == 1 && Card.Location == CardLocation.Grave)
            {
                bool hasTarget = Bot.Graveyard.Any(c => c != null && (c.Id == CardId.FiendsmithEngraver || c.Id == CardId.FiendsmithsLacrima || c.Id == CardId.FiendsmithsDesirae));
                if (hasTarget && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                {
                    DecisionTracer.Trace("LacrimaGraveQuickActivate", "Shuffle Lacrima to revive Fiendsmith monster");
                    return true;
                }
            }
            return false;
        }

        // ========================================================================
        // TIER 3: FIELD SPELLS, CONTINUOUS SPELLS & STARTERS
        // ========================================================================

        private bool ThroneActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_throneUsed) return false;
                if (ShouldSkipCombo()) return false;
                _throneUsed = true;
                DecisionTracer.Trace("ThroneActivate", "Activate Nightmare Throne from hand");
                return true;
            }

            // Float effect in SpellZone when a Yubel monster leaves field
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                DecisionTracer.Trace("ThroneActivate", "Trigger float effect of Nightmare Throne");
                return true;
            }

            return false;
        }

        private bool GatesActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_gatesHandUsed) return false;
                if (Bot.HasInSpellZone(CardId.OpeningOfTheSpiritGates)) return false;
                if (ShouldSkipCombo()) return false;
                _gatesHandUsed = true;
                DecisionTracer.Trace("GatesActivate", "Activate Gates from hand, search Beckoning Beast");
                return true;
            }

            // On-field ignition effect: Discard 1 to revive a 0 ATK/DEF Fiend from GY
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup() && Duel.Player == 0)
            {
                if (_gatesFieldUsed) return false;
                bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Attack == 0 && c.Defense == 0);
                bool hasDiscard = Bot.Hand.Count > 0;
                if (hasTarget && hasDiscard && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                {
                    _gatesFieldUsed = true;
                    DecisionTracer.Trace("GatesActivate", "Discard 1 to revive 0 ATK/DEF Fiend from GY");
                    return true;
                }
            }

            return false;
        }

        private bool NightmarePainActivate()
        {
            // Activate from hand to place face-up in SpellZone
            if (Card.Location == CardLocation.Hand)
            {
                if (_painUsed) return false;
                if (Bot.HasInSpellZone(CardId.NightmarePain)) return false;
                _painUsed = true;
                DecisionTracer.Trace("NightmarePainActivate", "Place Nightmare Pain face-up in SpellZone");
                return true;
            }

            // On-field ignition effect: Destroy 1 DARK monster in hand/field to search Yubel card
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup() && Duel.Player == 0)
            {
                if (_painUsed) return false;

                var darkMonsters = Bot.Hand.Where(c => c != null && c.HasAttribute(CardAttribute.Dark) && c.IsMonster())
                    .Concat(Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Dark)))
                    .ToList();

                if (darkMonsters.Count > 0)
                {
                    // Prioritize destroying Spirit of Yubel (floats immediately!) > Squirmer > Samsara > Yubel
                    var bestTarget = darkMonsters.OrderBy(c =>
                        c.Id == CardId.SpiritOfYubel ? 0 :
                        c.Id == CardId.GruesomeGraveSquirmer ? 1 :
                        c.Id == CardId.SamsaraDLotus ? 2 :
                        c.Id == CardId.Yubel ? 3 : 4
                    ).FirstOrDefault();

                    if (bestTarget != null)
                    {
                        AI.SelectCard(bestTarget);
                        _painUsed = true;
                        DecisionTracer.Trace("NightmarePainActivate", $"Destroy {bestTarget.Name} to search Yubel card");
                        return true;
                    }
                }
            }

            return false;
        }

        private bool FusionDeploymentActivate()
        {
            if (_deploymentUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;

            // Reveal Neos Kluger to SS Neos or Yubel from Deck
            bool hasTarget = GetRemainingCount(CardId.ElementalHERONeos) > 0 || GetRemainingCount(CardId.Yubel) > 0;
            if (hasTarget && GetFreeMonsterZoneCount() > 0)
            {
                AI.SelectCard(CardId.ElementalHERONeosKluger);
                _deploymentUsed = true;
                DecisionTracer.Trace("FusionDeploymentActivate", "Deploy Neos / Yubel from deck");
                return true;
            }
            return false;
        }

        // ========================================================================
        // TIER 4: YUBEL ENGINE CORE PLAYS & FLOATS
        // ========================================================================

        private bool LotusMainPhaseActivate()
        {
            // Main Phase: Tribute self to Special Summon Spirit of Yubel from deck
            if (Duel.Player == 0 && Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                if (_samsaraUsed) return false;
                if (IsSpecialSummonBlocked()) return false;

                bool hasTarget = GetRemainingCount(CardId.SpiritOfYubel) > 0 || GetRemainingCount(CardId.Yubel) > 0;
                if (hasTarget)
                {
                    AI.SelectCard(CardId.SpiritOfYubel, CardId.Yubel);
                    _samsaraUsed = true;
                    DecisionTracer.Trace("LotusMainPhaseActivate", "Tribute Lotus to SS Spirit of Yubel from Deck");
                    return true;
                }
            }
            return false;
        }

        private bool GraveSquirmerActivate()
        {
            if (IsSpecialSummonBlocked()) return false;

            // Hand Quick Effect: SS self if control a Fiend monster
            if (Card.Location == CardLocation.Hand && Duel.Player == 0)
            {
                if (_squirmerHandUsed) return false;
                bool controlFiend = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Fiend));
                if (controlFiend && GetFreeMonsterZoneCount() > 0)
                {
                    _squirmerHandUsed = true;
                    DecisionTracer.Trace("GraveSquirmerActivate", "SS Gruesome Grave Squirmer from Hand");
                    return true;
                }
            }

            // GY effect: Banish self to SS 1 Fiend with 0 ATK/DEF from hand or GY (except Squirmer)
            if (Card.Location == CardLocation.Grave && Duel.Player == 0)
            {
                if (_squirmerGraveUsed) return false;
                bool hasTarget = Bot.Graveyard.Concat(Bot.Hand).Any(c => c != null && c.IsMonster() && c.Attack == 0 && c.Defense == 0 && c.Id != CardId.GruesomeGraveSquirmer);
                if (hasTarget && GetFreeMonsterZoneCount() > 0)
                {
                    _squirmerGraveUsed = true;
                    DecisionTracer.Trace("GraveSquirmerActivate", "Banish Squirmer from GY to SS 0 ATK/DEF Fiend");
                    return true;
                }
            }

            return false;
        }

        private bool SpiritOfYubelOnSummonOrFloatActivate()
        {
            // On Special Summon: Set Nightmare Pain directly or add to hand
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                DecisionTracer.Trace("SpiritOfYubelOnSummonOrFloatActivate", "Spirit of Yubel SS trigger: Set/Search Yubel S/T");
                return true;
            }

            // If destroyed: Float into Yubel from Hand/Deck/GY/Banished
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                DecisionTracer.Trace("SpiritOfYubelOnSummonOrFloatActivate", "Spirit of Yubel destroyed: Float into Yubel!");
                return true;
            }

            return false;
        }

        private bool YubelFloatActivate()
        {
            // Yubel destroyed: Float into Terror Incarnate!
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                DecisionTracer.Trace("YubelFloatActivate", "Yubel destroyed! Float into Yubel - Terror Incarnate!");
                return true;
            }
            return false;
        }

        private bool YubelTerrorActivate()
        {
            // Terror Incarnate destroyed/leaves: Float into Ultimate Nightmare!
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                DecisionTracer.Trace("YubelTerrorActivate", "Terror Incarnate left! Float into Ultimate Nightmare!");
                return true;
            }

            // End Phase board wipe: destroy all other monsters on field
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && Duel.Phase == DuelPhase.End)
            {
                DecisionTracer.Trace("YubelTerrorActivate", "Terror Incarnate End Phase board wipe!");
                return true;
            }

            return false;
        }

        // ========================================================================
        // TIER 5: FIENDSMITH & BOARD BREAKERS
        // ========================================================================

        private bool EngraverActivate()
        {
            if (ShouldSkipCombo()) return false;

            // Hand: Discard to search Tract
            if (Card.Location == CardLocation.Hand)
            {
                if (_engraverHandUsed) return false;
                _engraverHandUsed = true;
                DecisionTracer.Trace("EngraverActivate", "Discard Engraver to search Fiendsmith's Tract");
                return true;
            }

            // GY: Shuffle 1 other LIGHT Fiend from GY into Deck -> SS self
            if (Card.Location == CardLocation.Grave && Duel.Player == 0)
            {
                if (_engraverGraveUsed) return false;
                var otherLightFiends = Bot.Graveyard.Where(c => c != null && c.HasRace(CardRace.Fiend) && c.HasAttribute(CardAttribute.Light) && c != Card).ToList();
                if (otherLightFiends.Count > 0 && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                {
                    _engraverGraveUsed = true;
                    DecisionTracer.Trace("EngraverActivate", "SS Engraver from GY by recycling LIGHT Fiend");
                    return true;
                }
            }

            return false;
        }

        private bool TractActivate()
        {
            if (ShouldSkipCombo()) return false;

            // Hand: Search LIGHT Fiend (Lurrie), then discard 1
            if (Card.Location == CardLocation.Hand)
            {
                if (_tractHandUsed) return false;
                _tractHandUsed = true;
                DecisionTracer.Trace("TractActivate", "Search LIGHT Fiend via Tract");
                return true;
            }

            // GY: Banish to Fusion Summon Fiendsmith Fusion
            if (Card.Location == CardLocation.Grave && Duel.Player == 0)
            {
                if (_tractGraveUsed) return false;
                bool canFuse = Bot.Hand.Concat(Bot.GetMonsters()).Count(c => c != null && c.HasRace(CardRace.Fiend) && c.HasAttribute(CardAttribute.Light)) >= 2;
                if (canFuse && !IsSpecialSummonBlocked())
                {
                    _tractGraveUsed = true;
                    DecisionTracer.Trace("TractActivate", "GY Fusion via Fiendsmith's Tract");
                    return true;
                }
            }

            return false;
        }

        private bool LacrimaOnSummonActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                DecisionTracer.Trace("LacrimaOnSummonActivate", "Dump Fiendsmith card from deck to GY");
                return true;
            }
            return false;
        }

        private bool DarkHoleActivate()
        {
            // Dark Hole wipes opponent monsters, while triggering our Yubel floating!
            int enemyMonsters = Enemy.GetMonsterCount();
            bool hasYubelOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.SpiritOfYubel || c.Id == CardId.Yubel));

            if (enemyMonsters >= 2 || (enemyMonsters >= 1 && hasYubelOnField) || (enemyMonsters >= 1 && Util.GetProblematicEnemyMonster() != null))
            {
                DecisionTracer.Trace("DarkHoleActivate", "Dark Hole board wipe!");
                return true;
            }
            return false;
        }

        private bool MutinyActivate()
        {
            if (_mutinyUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Shuffles Fiend/Fairy from GY into Deck to Fusion Summon
            int gyFiendFairy = Bot.Graveyard.Count(c => c != null && (c.HasRace(CardRace.Fiend) || c.HasRace(CardRace.Fairy)));
            if (gyFiendFairy >= 2 && GetFreeMonsterZoneCount() > 0)
            {
                _mutinyUsed = true;
                DecisionTracer.Trace("MutinyActivate", "Shuffle GY Fiends/Fairies to Fusion Summon");
                return true;
            }
            return false;
        }

        // ========================================================================
        // TIER 6: NORMAL SUMMONS
        // ========================================================================

        private bool BeckoningBeastSummon()
        {
            if (_beckoningUsed) return false;
            return true;
        }

        private bool LotusSummon()
        {
            if (_samsaraUsed) return false;
            return true;
        }

        private bool GenericNormalSummon()
        {
            // Only summon if we have extra summon from Beckoning Beast or no other monsters
            if (Bot.MonsterZone.Count(c => c != null) == 0) return true;
            return false;
        }

        // ========================================================================
        // TIER 7: SPECIAL SUMMONS & GEISTGRINDER OTK
        // ========================================================================

        private bool GeistgrinderActivate()
        {
            if (Card.Location == CardLocation.Hand && Duel.Player == 0)
            {
                if (_geistgrinderHandUsed) return false;
                // Reveal 1 Yubel monster in hand to SS Geistgrinder to opponent's field, SS Yubel to our field
                bool hasYubelInHand = Bot.Hand.Any(c => c != null && YubelMonsters.Contains(c.Id) && c != Card);
                if (hasYubelInHand && Enemy.GetMonsterCount() < 5 && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                {
                    _geistgrinderHandUsed = true;
                    DecisionTracer.Trace("GeistgrinderActivate", "Give Geistgrinder Golem (3000 ATK) to opponent, SS Yubel to our field!");
                    return true;
                }
            }

            // In GY: When Yubel is Special Summoned, SS Geistgrinder to opponent's field
            if (Card.Location == CardLocation.Grave)
            {
                if (Enemy.GetMonsterCount() < 5 && !IsSpecialSummonBlocked())
                {
                    DecisionTracer.Trace("GeistgrinderActivate", "Revive Geistgrinder to enemy field as battle target!");
                    return true;
                }
            }

            return false;
        }

        private bool LovingDefenderSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            // Contact Fusion: 1 Yubel + 1+ Effect Monsters on field (send to GY)
            bool hasYubelOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id));
            int enemyEffectMonsters = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect));

            // Contact summon if opponent has 2+ effect monsters or to deal game-winning damage!
            if (hasYubelOnField && (enemyEffectMonsters >= 2 || HasLethalOnBoard() || Enemy.LifePoints <= 3000))
            {
                DecisionTracer.Trace("LovingDefenderSummon", "Contact Fuse enemy board into Loving Defender Forever!");
                return true;
            }
            return false;
        }

        private bool ChaosAngelSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            var monsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark))).ToList();
            if (monsters.Count < 2) return false;

            // Exclude Ace cards
            var safeMonsters = monsters.Where(c => !IsAceCard(c)).ToList();
            for (int i = 0; i < safeMonsters.Count; i++)
            {
                for (int j = i + 1; j < safeMonsters.Count; j++)
                {
                    if (safeMonsters[i].Level + safeMonsters[j].Level == 10)
                    {
                        AI.SelectCard(new[] { safeMonsters[i], safeMonsters[j] });
                        DecisionTracer.Trace("ChaosAngelSummon", "Synchro Summon Chaos Angel (Banish on summon + Double Protection)");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool LuceContactSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int count = Bot.Graveyard.Count(c => c != null && (c.HasRace(CardRace.Fiend) || c.HasRace(CardRace.Fairy)));
            if (count >= 3 && GetFreeMonsterZoneCount() > 0)
            {
                DecisionTracer.Trace("LuceContactSummon", "Contact Summon Luce (3500 ATK) by shuffling 3 GY Fiend/Fairy");
                return true;
            }
            return false;
        }

        private bool LacrimaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasGYTarget = Bot.Graveyard.Any(c => c != null && c.HasAttribute(CardAttribute.Light) && c.HasRace(CardRace.Fiend));
            return hasGYTarget;
        }

        private bool AerialEaterSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.Deck.Any(c => c != null && c.HasRace(CardRace.Fiend));
        }

        private bool DukeOfDemiseSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return false; // Prefer Aerial Eater or Lacrima
        }

        private bool NeosKlugerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool hasNeos = Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != null && c.Id == CardId.ElementalHERONeos);
            bool hasYubel = Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != null && c.Id == CardId.Yubel);
            return hasNeos && hasYubel;
        }

        private bool GustavMaxSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int level10Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10);
            if (level10Count >= 2 && (Enemy.LifePoints <= 2000 || HasLethalOnBoard()))
            {
                DecisionTracer.Trace("GustavMaxSummon", "Xyz Summon Gustav Max for 2000 lethal burn!");
                return true;
            }
            return false;
        }

        private bool GustavMaxBurnActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                DecisionTracer.Trace("GustavMaxBurnActivate", "Detach to burn 2000 LP!");
                return true;
            }
            return false;
        }

        private bool SuperDoraSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            int level10Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10);
            return level10Count >= 2 && HasLethalOnBoard();
        }

        private bool SuperDoraProtectActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                var boss = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsAceCard(c));
                if (boss != null) AI.SelectCard(boss);
                return true;
            }
            return false;
        }

        // ========================================================================
        // TIER 8: END PHASE SAMSARA LOTUS REVIVE LOOP
        // ========================================================================

        private bool LotusEndPhaseReviveActivate()
        {
            // In our End Phase, if we control Yubel and Lotus is in GY -> SS itself!
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.End && Card.Location == CardLocation.Grave)
            {
                bool controlYubel = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id));
                if (controlYubel && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                {
                    DecisionTracer.Trace("LotusEndPhaseReviveActivate", "Revive Samsara Lotus from GY in End Phase (ready for opponent turn negate!)");
                    return true;
                }
            }
            return false;
        }

        // ========================================================================
        // OCGCORE 64-BIT OPTION SELECTION OVERRIDE
        // ========================================================================

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 4;
                if (cardId == 0 && Card != null) cardId = Card.Id;
                long optIndex = options[i] & 0xf;

                // 1. Samsara D Lotus (62318994)
                // Option 0: Add to Hand, Option 1: Special Summon to field
                if (cardId == CardId.SamsaraDLotus)
                {
                    if (optIndex == 1 && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Samsara Lotus: Option {optIndex} (Special Summon to field)");
                        return i;
                    }
                    if (optIndex == 0) return i;
                }

                // 2. Spirit of Yubel (90829280)
                // Option 0: Add to Hand, Option 1: Set directly to field
                if (cardId == CardId.SpiritOfYubel)
                {
                    // Setting directly to field avoids Droll and plays around Imperm
                    if (optIndex == 1)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Spirit of Yubel: Option {optIndex} (Set directly to field)");
                        return i;
                    }
                    if (optIndex == 0) return i;
                }

                // 3. Nightmare Throne (93729896)
                // Activation: Option 0: Add to Hand, Option 1: Destroy from Deck
                // Float: Option 0: Add to Hand, Option 1: Special Summon
                if (cardId == CardId.NightmareThrone)
                {
                    bool hasLotus = Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != null && c.Id == CardId.SamsaraDLotus);
                    if (hasLotus && optIndex == 1)
                    {
                        // Destroy Spirit of Yubel from deck to immediately float into Yubel!
                        DecisionTracer.Trace("OnSelectOption", $"Nightmare Throne: Option {optIndex} (Destroy Spirit of Yubel to float)");
                        return i;
                    }
                    if (!hasLotus && optIndex == 0)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Nightmare Throne: Option {optIndex} (Add Samsara Lotus to hand)");
                        return i;
                    }
                    if (optIndex == 1 && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                        return i;
                }

                // 4. Eternal Favorite (87532344)
                // Option 0: Special Summon Yubel, Option 1: Fusion Summon using enemy monsters
                if (cardId == CardId.EternalFavorite)
                {
                    bool hasEnemyMonsters = Enemy.GetMonsterCount() > 0;
                    if (hasEnemyMonsters && Bot.Hand.Count > 0 && optIndex == 1 && !IsSpecialSummonBlocked())
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Eternal Favorite: Option {optIndex} (Fusion Summon using enemy board)");
                        return i;
                    }
                    if (optIndex == 0)
                    {
                        DecisionTracer.Trace("OnSelectOption", $"Eternal Favorite: Option {optIndex} (Special Summon Yubel)");
                        return i;
                    }
                }

                // 5. Fiendsmith's Lacrima (46640168)
                // Option 0: Add to Hand, Option 1: Special Summon
                if (cardId == CardId.FiendsmithsLacrima)
                {
                    if (optIndex == 1 && GetFreeMonsterZoneCount() > 0 && !IsSpecialSummonBlocked())
                        return i;
                    if (optIndex == 0) return i;
                }
            }

            return base.OnSelectOption(options);
        }

        // ========================================================================
        // HINT-SPECIFIC CARD SELECTION (OnSelectCard)
        // Strictly adheres to Section 7.3 Hint Table
        // ========================================================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Hint 500: HINTMSG_RELEASE (Tribute fodder/tokens first, never sacrifice Ace)
            if (hint == 500)
            {
                var sorted = cards.OrderBy(c => c.Controller == 1 ? 0 : GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 501: HINTMSG_DISCARD (Discard GY triggers or duplicates, protect starters)
            if (hint == 501 || (cards.All(c => c != null && c.Location == CardLocation.Hand) && hint != 502))
            {
                var lurrie = cards.FirstOrDefault(c => c != null && c.Id == CardId.FabledLurrie);
                if (lurrie != null) return new List<ClientCard> { lurrie }; // Triggers self Special Summon!

                var redundantYubels = cards.Where(c => c != null && YubelMonsters.Contains(c.Id)).ToList();
                if (redundantYubels.Count > 1) return new List<ClientCard> { redundantYubels[0] };

                var engraver = cards.FirstOrDefault(c => c != null && c.Id == CardId.FiendsmithEngraver);
                if (engraver != null) return new List<ClientCard> { engraver };

                var neos = cards.FirstOrDefault(c => c != null && c.Id == CardId.ElementalHERONeos);
                if (neos != null) return new List<ClientCard> { neos };

                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 502: HINTMSG_DESTROY (Self-pops trigger floats; Enemy-pops target highest threat)
            if (hint == 502)
            {
                // Self destruction (Nightmare Pain / Final Bringer / Throne / Lotus trigger)
                var friendlyTargets = cards.Where(c => c != null && c.Controller == 0).ToList();
                if (friendlyTargets.Count > 0)
                {
                    // Prioritize destroying Spirit of Yubel (triggers float into Yubel!)
                    var spirit = friendlyTargets.FirstOrDefault(c => c.Id == CardId.SpiritOfYubel);
                    if (spirit != null) return new List<ClientCard> { spirit };

                    var yubel = friendlyTargets.FirstOrDefault(c => c.Id == CardId.Yubel);
                    if (yubel != null) return new List<ClientCard> { yubel };

                    var squirmer = friendlyTargets.FirstOrDefault(c => c.Id == CardId.GruesomeGraveSquirmer);
                    if (squirmer != null) return new List<ClientCard> { squirmer };

                    var terror = friendlyTargets.FirstOrDefault(c => c.Id == CardId.YubelTerrorIncarnate);
                    if (terror != null) return new List<ClientCard> { terror };
                }

                // Enemy destruction (Target highest ThreatScore, avoid destruction immune / cards that want to be destroyed)
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyTargets.Count > 0)
                {
                    var validTargets = enemyTargets.Where(c => !IsDestructionImmune(c)).ToList();
                    var pool = validTargets.Count > 0 ? validTargets : enemyTargets;
                    var sorted = pool.OrderByDescending(c => GetCardThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }
            }

            // Hint 504 / 503: HINTMSG_REMOVE (Banish highest threat, skip target-immune)
            if (hint == 504 || hint == 503)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyTargets.Count > 0)
                {
                    var validTargets = enemyTargets.Where(c => IsViableEffectTarget(c)).ToList();
                    var pool = validTargets.Count > 0 ? validTargets : enemyTargets;
                    var sorted = pool.OrderByDescending(c => GetCardThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }

                // Friendly banish costs (Grave Squirmer / Tract in GY)
                var squirmerGY = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Grave && c.Id == CardId.GruesomeGraveSquirmer);
                if (squirmerGY != null) return new List<ClientCard> { squirmerGY };

                var tractGY = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.Grave && c.Id == CardId.FiendsmithsTract);
                if (tractGY != null) return new List<ClientCard> { tractGY };
            }

            // Hint 505: HINTMSG_ATOHAND / RTOHAND (Search to hand or bounce enemy threat)
            // Hint 506: HINTMSG_TODECK (Spin to deck or recycle GY materials)
            if (hint == 505 || hint == 506 || cards.Any(c => c != null && c.Location == CardLocation.Deck))
            {
                // If bouncing or spinning enemy cards:
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    var sorted = enemyCards.OrderByDescending(c => GetCardThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }

                // Tract search priority: Lurrie first for free summon
                if (Card != null && Card.Id == CardId.FiendsmithsTract)
                {
                    var lurrie = cards.FirstOrDefault(c => c != null && c.Id == CardId.FabledLurrie);
                    if (lurrie != null) return new List<ClientCard> { lurrie };
                }

                bool hasLotus = Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != null && c.Id == CardId.SamsaraDLotus);
                bool hasPain = Bot.Hand.Concat(Bot.GetSpells()).Any(c => c != null && c.IsFaceup() && c.Id == CardId.NightmarePain);
                bool hasBeckoning = Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != null && c.Id == CardId.DarkBeckoningBeast);

                var searchPriority = new List<int>();
                if (!hasLotus) searchPriority.Add(CardId.SamsaraDLotus);
                if (!hasPain) searchPriority.Add(CardId.NightmarePain);
                if (!hasBeckoning) searchPriority.Add(CardId.DarkBeckoningBeast);
                searchPriority.Add(CardId.SpiritOfYubel);
                searchPriority.Add(CardId.GruesomeGraveSquirmer);
                searchPriority.Add(CardId.EternalFavorite);
                searchPriority.Add(CardId.Yubel);
                searchPriority.Add(CardId.FiendsmithEngraver);
                searchPriority.Add(CardId.OpeningOfTheSpiritGates);
                searchPriority.Add(CardId.FabledLurrie);

                foreach (int sid in searchPriority)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == sid);
                    if (match != null) return new List<ClientCard> { match };
                }
            }

            // Hint 507: HINTMSG_EQUIP (Equip to best boss monster)
            if (hint == 507)
            {
                var bosses = cards.Where(c => c != null && c.Controller == 0 && c.IsFaceup()).OrderByDescending(c => c.Attack).ToList();
                if (bosses.Count > 0) return bosses.Take(max).ToList();
            }

            // Hint 508: HINTMSG_TOGRAVE (Send combo extenders / triggers to GY)
            if (hint == 508)
            {
                var dumpPriority = new[] {
                    CardId.FabledLurrie,
                    CardId.FiendsmithEngraver,
                    CardId.LacrimaTheCrimsonTears,
                    CardId.GruesomeGraveSquirmer,
                    CardId.FiendsmithsTract
                };

                foreach (int did in dumpPriority)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == did);
                    if (match != null) return new List<ClientCard> { match };
                }
            }

            // Hint 509: HINTMSG_SPSUMMON (Special Summon Ace / Negator / Key Extender)
            if (hint == 509)
            {
                var preferred = new[] {
                    CardId.SpiritOfYubel,
                    CardId.YubelTheLovingDefenderForever,
                    CardId.YubelTerrorIncarnate,
                    CardId.Yubel,
                    CardId.SamsaraDLotus,
                    CardId.FiendsmithEngraver,
                    CardId.FiendsmithsLacrima,
                    CardId.GruesomeGraveSquirmer,
                    CardId.FabledLurrie
                };

                foreach (int pid in preferred)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == pid);
                    if (match != null) return new List<ClientCard> { match };
                }
            }

            // Hint 518: HINTMSG_POSCHANGE (Stat-Aware position change)
            if (hint == 518)
            {
                var sorted = cards.OrderBy(c => c.Attack > c.Defense ? 0 : 1).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 519: HINTMSG_XMATERIAL (Detach non-Ace / fodder materials first)
            if (hint == 519)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            // Hint 552 / 572: HINTMSG_DISABLE / NEGATE (Target key chokepoint or negator, not the first card seen)
            if (hint == 552 || hint == 572)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyTargets.Count > 0)
                {
                    var sorted = enemyTargets.OrderByDescending(c => 
                        CardIntelligence.IsHighThreatChokepoint(c.Id) ? 1000 :
                        CardIntelligence.IsKnownNegator(c.Id) ? 900 :
                        GetCardThreatScore(c)).ToList();
                    return sorted.Take(max).ToList();
                }
            }

            // Hint 511 / 513 / 533: Fusion, Contact Fusion, Ritual, Release materials
            if (hint == 511 || hint == 513 || hint == 533)
            {
                // Prioritize absorbing opponent monsters for Loving Defender or Super Poly
                var sorted = cards.OrderBy(c => c.Controller == 1 ? 0 : GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ========================================================================
        // POSITIONING & BATTLE
        // ========================================================================

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            bool hasPain = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == CardId.NightmarePain);

            // Yubel forms should be in FaceUpAttack if Nightmare Pain is active to reflect damage!
            if (YubelMonsters.Contains(cardId))
            {
                if (hasPain || cardId == CardId.YubelTheLovingDefenderForever || cardId == CardId.ElementalHERONeosKluger)
                {
                    if (positions.Contains(CardPosition.FaceUpAttack))
                        return CardPosition.FaceUpAttack;
                }
                else
                {
                    // Otherwise defense is safe
                    if (positions.Contains(CardPosition.FaceUpDefence))
                        return CardPosition.FaceUpDefence;
                }
            }

            // Low ATK / utility monsters -> FaceUpDefence (Stat-Aware)
            if (cardId == CardId.SamsaraDLotus || cardId == CardId.DarkBeckoningBeast ||
                cardId == CardId.FabledLurrie || cardId == CardId.GruesomeGraveSquirmer)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            // High ATK beaters
            if (cardId == CardId.ChaosAngel || cardId == CardId.LuceTheDusksDark ||
                cardId == CardId.FiendsmithEngraver || cardId == CardId.StarvingVenomFusionDragon)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;

            bool hasPain = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == CardId.NightmarePain);
            bool hasEnemyMonstersWithAtk = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack > 0);

            if (YubelMonsters.Contains(Card.Id))
            {
                if (hasPain && hasEnemyMonstersWithAtk)
                {
                    // We want Attack position to declare attacks and reflect damage!
                    if (Card.IsDefense()) return true;
                    return false;
                }
                else if (!hasEnemyMonstersWithAtk)
                {
                    // If no enemy monsters to punch, stay in defense
                    if (Card.IsAttack() && Card.Attack == 0) return true;
                }
            }

            return false;
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (attacker != null && YubelMonsters.Contains(attacker.Id))
            {
                bool hasPain = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.Id == CardId.NightmarePain);
                // If Nightmare Pain is on field, OR if attacker is Loving Defender / Neos Kluger:
                // Punching higher ATK deals MORE damage to opponent!
                if (hasPain || attacker.Id == CardId.YubelTheLovingDefenderForever ||
                    attacker.Id == CardId.YubelTheUltimateNightmare || attacker.Id == CardId.ElementalHERONeosKluger)
                {
                    var bestTarget = defenders.Where(d => d != null && d.IsFaceup() && d.IsAttack())
                                              .OrderByDescending(d => d.Attack)
                                              .FirstOrDefault();
                    if (bestTarget != null && bestTarget.Attack > 0)
                    {
                        return AI.Attack(attacker, bestTarget);
                    }
                }
            }
            return base.OnSelectAttackTarget(attacker, defenders);
        }
    }
}
