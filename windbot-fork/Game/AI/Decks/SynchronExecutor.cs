// ============================================================================
// CARD AUDIT — Synchron / Junk / Stardust (Competitive Tier-1 Modern Synchro Engine)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Junk Synchron                      | Monster L3 T | No   | No    | None    | NS: Revive L2 or lower monster from GY        | Main Phase 1: Revive Lv 2 to make Lv 5 Speeder| No Lv 2 in GY                               |
// | Junk Converter                     | Monster L2   | Yes  | Yes   | Discard | Hand: Discard + Tuner -> Search Synchron; Mat -> Revive Tuner | Hand: Search Junk Synchron; Mat: Revive Tuner| No Tuner in hand                            |
// | Full-Speed Warrior                 | Monster L2   | Yes  | Yes   | None    | NS/SS: Search Junk Synchron or S/T mentioning Junk Warrior | NS/SS: Search Synchro Fellowship or Junk Synchron | Already searched this turn          |
// | Doppelwarrior                      | Monster L2   | No   | No    | None    | SS from hand when monster revived from GY; Mat -> 2 L1 Tokens | Swarm 2 non-Tuner tokens for Synchro climbing| Field full                                  |
// | Stardust Trail                     | Monster L4   | Yes  | Yes   | None    | SS from hand/GY when monster tributed; Mat -> L1 Dragon Token | Key non-Tuner Dragon extender                | Already used this turn                      |
// | Starjunk Synchron                  | Monster L3 T | Yes  | Yes   | None    | Hand: Target L2 in GY -> SS self + that mon   | Hand: Free Lv 5 Synchro without NS           | No Lv 2 in GY or field full                 |
// | Stardust Synchron                  | Monster L4 T | Yes  | Yes   | Tribute | Hand/GY: Tribute 1 mon -> SS self; NS/SS: Search Stardust S/T | Hand/GY extender; Search Fellowship/Illumination | No tribute fodder or field full        |
// | Assault Synchron                   | Monster L2 T | Yes  | Yes   | 700 LP  | Hand: SS self; GY: Revive Dragon Synchro      | Hand extender; GY follow-up                  | LP <= 700                                   |
// | Scrap Synchron                     | Monster L1 T | Yes  | Yes   | None    | Hand Synchro mat for Synchron-mentioning card | Hand / Field: Synchro Material; GY protect   | Not needed                                  |
// | Jet Synchron                       | Monster L1 T | Yes  | Yes   | Discard | GY: Discard 1 -> SS self; Mat: Search Junk    | GY extender                                  | No discard fodder                           |
// | Cosmic Blazar Dragon               | Synchro L12  | Yes  | No    | Banish  | Omni-Negate (Activation/Summon/Attack) -> End | Opponent card / summon / attack              | Already banished                            |
// | Bystial Dis Pater                  | Synchro L10  | Yes  | Yes   | None    | Revive banished LIGHT/DARK; Shuffle banished -> Negate/Destroy | Extend combos / Opponent monster activation | No banished cards                           |
// | Satellite Warrior                  | Synchro L10  | Yes  | Yes   | Target  | Quick pop cards = # Synchros in GY + 1000 ATK per pop | Quick Synchro on opponent turn to board wipe | Opponent has no cards                       |
// | Crimson Dragon                     | Synchro L12  | Yes  | Yes   | Tag out | Tag out Lv 8 into Crystal Wing or Victim Sanctuary | Opponent turn: Tag into Crystal Wing / Victim Sanctuary | No Lv 8 Synchro to target           |
// | Stardust Warrior                   | Synchro L10  | Yes  | No    | Tribute | Special Summon Negation; Float into Warrior   | Opponent Special Summon                      | Opponent not special summoning              |
// | Crystal Wing Synchro Dragon        | Synchro L8   | Yes  | No    | None    | Monster Negate + ATK gain                     | Opponent monster effect                      | Already negated this turn                   |
// | Stardust Dragon - Victim Sanctuary | Synchro L8   | Yes  | Yes   | Tribute | Negate opp response to our effect; GY: Tag Stardust | Opponent activates card in response to us    | No opp response                              |
// | Junk Speeder                       | Synchro L5   | Yes  | Yes   | None    | SS as many Synchron Tuners with different Lv  | Core engine: Swarm 4-5 Tuners onto board     | Field already full or effect negated        |
// | T.G. Hyper Librarian               | Synchro L5   | No   | No    | None    | Draw 1 card when a monster is Synchro Summoned| Keep on field during Synchro climbing        | Do not use as material prematurely          |
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Synchron", "Synchron")]
    public class SynchronExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int JetSynchron = 9742784;
            public const int JunkConverter = 11069680;
            public const int StarjunkSynchron = 13021682;
            public const int ScrapSynchron = 16449363;
            public const int FullSpeedWarrior = 17201951;
            public const int Nibiru = 27204311;
            public const int StardustSynchron = 37799519;
            public const int MulcharmyFuwalos = 42141493;
            public const int Doppelwarrior = 53855409;
            public const int StardustTrail = 63184227;
            public const int JunkSynchron = 63977008;
            public const int AssaultSynchron = 77202120;
            public const int EffectVeiler = 97268402;

            // Spells
            public const int ForbiddenDroplet = 24299458;
            public const int JunkSignal = 26387390;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int StardustIllumination = 37750912;
            public const int SynchroFellowship = 43834302;
            public const int SynchroRumble = 88901994;
            public const int Tuning = 96363153;

            // Traps
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int CosmicBlazarDragon = 21123811;
            public const int BystialDisPater = 27572350;
            public const int AccelSynchroStardustDragon = 30983281;
            public const int ScrapWarrior = 42711820;
            public const int StardustDragon = 44508094;
            public const int FormulaSynchron = 50091196;
            public const int CrystalWingSynchroDragon = 50954680;
            public const int SatelliteWarrior = 60465049;
            public const int CrimsonDragon = 63436931;
            public const int BlackRoseDragon = 73580471;
            public const int StardustWarrior = 74892653;
            public const int StardustDragonVictimSanctuary = 76636978;
            public const int JunkSpeeder = 77075360;
            public const int TGHyperLibrarian = 90953320;
            public const int RedSupernovaDragon = 99585850;
        }

        internal SynchronPlugin Plugin { get; private set; }

        public SynchronExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new SynchronPlugin(this);
            DeckPlugin = Plugin;

            RegisterHelperModules();
            RegisterComboLines();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            Plugin?.ResetTurnState();
        }

        private void RegisterHelperModules()
        {
            // 1. Layer 2 Central Core Ace Card Protection
            ResourcePlan.RegisterAceCards(
                CardId.CosmicBlazarDragon,
                CardId.RedSupernovaDragon,
                CardId.BystialDisPater,
                CardId.CrystalWingSynchroDragon,
                CardId.CrimsonDragon,
                CardId.SatelliteWarrior,
                CardId.StardustWarrior,
                CardId.StardustDragonVictimSanctuary,
                CardId.TGHyperLibrarian,
                CardId.ScrapWarrior
            );
            HeuristicGuard.RegisterAceCards(
                CardId.CosmicBlazarDragon,
                CardId.RedSupernovaDragon,
                CardId.BystialDisPater,
                CardId.CrystalWingSynchroDragon,
                CardId.CrimsonDragon,
                CardId.SatelliteWarrior,
                CardId.StardustWarrior,
                CardId.StardustDragonVictimSanctuary,
                CardId.TGHyperLibrarian,
                CardId.ScrapWarrior
            );

            // 2. Layer 2 Handtrap Bait & Combo Starters
            BaitPlanner.RegisterComboStarters(
                CardId.SynchroFellowship,
                CardId.JunkSynchron,
                CardId.FullSpeedWarrior,
                CardId.JunkConverter,
                CardId.Tuning,
                CardId.ReinforcementOfTheArmy
            );
            BaitPlanner.RegisterBaitCards(
                CardId.Tuning,
                CardId.ReinforcementOfTheArmy
            );

            // 3. Layer 2 High Value Chain Targets
            ChainAdvisor.RegisterHighValueTargets(
                CardId.JunkSpeeder,
                CardId.JunkSynchron,
                CardId.SynchroFellowship,
                CardId.AccelSynchroStardustDragon,
                CardId.CrimsonDragon,
                CardId.CosmicBlazarDragon,
                CardId.BystialDisPater,
                CardId.CrystalWingSynchroDragon
            );
        }

        private void RegisterComboLines()
        {
            // Main Route: Synchro Fellowship Starter Line
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Synchron-Fellowship-Starter",
                RequiredCards = new List<int> { CardId.SynchroFellowship },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.SynchroFellowship, ActionType = ExecutorType.Activate, Description = "Activate Synchro Fellowship" },
                    new() { CardId = CardId.JunkSynchron, ActionType = ExecutorType.Summon, Description = "Normal Summon Junk Synchron" },
                    new() { CardId = CardId.JunkSpeeder, ActionType = ExecutorType.SpSummon, Description = "Synchro Summon Junk Speeder" }
                },
                Priority = 100
            });

            // Route 2: Junk Synchron + Lv 2 Starter Line
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Synchron-JunkSynchron-Starter",
                RequiredCards = new List<int> { CardId.JunkSynchron },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.JunkSynchron, ActionType = ExecutorType.Summon, Description = "Normal Summon Junk Synchron" },
                    new() { CardId = CardId.JunkSpeeder, ActionType = ExecutorType.SpSummon, Description = "Synchro Summon Junk Speeder" }
                },
                Priority = 90
            });

            // Route 3: Full-Speed Warrior Starter Line
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Synchron-FullSpeed-Starter",
                RequiredCards = new List<int> { CardId.FullSpeedWarrior },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.FullSpeedWarrior, ActionType = ExecutorType.Summon, Description = "Normal Summon Full-Speed Warrior" },
                    new() { CardId = CardId.SynchroFellowship, ActionType = ExecutorType.Activate, Description = "Activate Synchro Fellowship" },
                    new() { CardId = CardId.JunkSynchron, ActionType = ExecutorType.Summon, Description = "Normal Summon Junk Synchron" },
                    new() { CardId = CardId.JunkSpeeder, ActionType = ExecutorType.SpSummon, Description = "Synchro Summon Junk Speeder" }
                },
                Priority = 85
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  HANDTRAPS & COUNTERS (Highest Chain Priority)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, ImpermanenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, NibiruEffect);

            // ═══════════════════════════════════════════════════════════════
            //  ON-FIELD ACE MONSTER QUICK DISRUPTIONS & TAG-OUTS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.CosmicBlazarDragon, CosmicBlazarEffect);
            AddExecutor(ExecutorType.Activate, CardId.RedSupernovaDragon, RedSupernovaEffect);
            AddExecutor(ExecutorType.Activate, CardId.BystialDisPater, BystialDisPaterEffect);
            AddExecutor(ExecutorType.Activate, CardId.StardustDragonVictimSanctuary, VictimSanctuaryEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrystalWingSynchroDragon, CrystalWingEffect);
            AddExecutor(ExecutorType.Activate, CardId.StardustWarrior, StardustWarriorEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrimsonDragon, CrimsonDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.StardustDragon, StardustDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.JunkSignal, JunkSignalEffect);

            // ═══════════════════════════════════════════════════════════════
            //  IN-ENGINE SEARCH & GRAVEYARD SETUP SPELLS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotaEffect);
            AddExecutor(ExecutorType.Activate, CardId.Tuning, TuningEffect);
            AddExecutor(ExecutorType.Activate, CardId.SynchroFellowship, SynchroFellowshipEffect);
            AddExecutor(ExecutorType.Activate, CardId.JunkConverter, JunkConverterHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.StardustIllumination, StardustIlluminationEffect);
            AddExecutor(ExecutorType.Activate, CardId.SynchroRumble, SynchroRumbleEffect);

            // ═══════════════════════════════════════════════════════════════
            //  IN-ENGINE MONSTER SUMMONS & SPECIAL SUMMONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.Doppelwarrior, DoppelwarriorEffect);
            AddExecutor(ExecutorType.Activate, CardId.StardustTrail, StardustTrailEffect);
            AddExecutor(ExecutorType.Activate, CardId.StarjunkSynchron, StarjunkSynchronEffect);
            AddExecutor(ExecutorType.Activate, CardId.AssaultSynchron, AssaultSynchronEffect);
            AddExecutor(ExecutorType.Activate, CardId.StardustSynchron, StardustSynchronEffect);
            AddExecutor(ExecutorType.Activate, CardId.JetSynchron, JetSynchronEffect);

            AddExecutor(ExecutorType.Summon, CardId.FullSpeedWarrior, FullSpeedWarriorSummon);
            AddExecutor(ExecutorType.Activate, CardId.FullSpeedWarrior, FullSpeedWarriorEffect);

            AddExecutor(ExecutorType.Summon, CardId.JunkSynchron, JunkSynchronSummon);
            AddExecutor(ExecutorType.Activate, CardId.JunkSynchron, JunkSynchronEffect);

            AddExecutor(ExecutorType.Summon, CardId.Doppelwarrior, FallbackNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.StarjunkSynchron, FallbackNormalSummon);

            // ═══════════════════════════════════════════════════════════════
            //  EXTRA DECK SYNCHRO PROGRESSION
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpSummon, CardId.ScrapWarrior, ScrapWarriorSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.ScrapWarrior, ScrapWarriorEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.JunkSpeeder, JunkSpeederSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.JunkSpeeder, JunkSpeederEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.TGHyperLibrarian, LibrarianSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TGHyperLibrarian, LibrarianEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.AccelSynchroStardustDragon, AccelSynchroSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccelSynchroStardustDragon, AccelSynchroEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.BlackRoseDragon, BlackRoseDragonSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlackRoseDragon, BlackRoseDragonEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.CrystalWingSynchroDragon, CrystalWingSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.StardustDragonVictimSanctuary, VictimSanctuarySpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialDisPater, DisPaterSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.StardustWarrior, StardustWarriorSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SatelliteWarrior, SatelliteWarriorSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SatelliteWarrior, SatelliteWarriorEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.CosmicBlazarDragon, CosmicBlazarSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RedSupernovaDragon, RedSupernovaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrimsonDragon, CrimsonDragonSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FormulaSynchron, FormulaSynchronSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.FormulaSynchron, FormulaSynchronEffect);

            // ═══════════════════════════════════════════════════════════════
            //  BACKROW SET (MP2 / End of Turn)
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.JunkSignal);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
        }

        public override bool OnSelectHand()
        {
            // Tier-1 Synchro combo excels going First to build multi-negate board
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  HANDTRAP & INTERRUPTION IMPLEMENTATIONS
        // ═══════════════════════════════════════════════════════════════

        private bool ForbiddenDropletEffect()
        {
            if (!Enemy.GetMonsters().Any(m => m.IsFaceup() && !m.IsDisabled())) return false;

            var targets = Enemy.GetMonsters().Where(m => m.IsFaceup() && !m.IsDisabled()).OrderByDescending(m => m.Attack).ToList();
            if (targets.Count > 0)
            {
                AI.SelectCard(targets);
                return true;
            }
            return false;
        }

        private bool ImpermanenceEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(LastChainCard);
                return true;
            }
            var target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool EffectVeilerEffect()
        {
            if (Duel.Player == 0) return false;
            if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(LastChainCard);
                return true;
            }
            var target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool MulcharmyFuwalosEffect()
        {
            return Duel.Player == 1 && Bot.GetMonsterCount() == 0;
        }

        private bool NibiruEffect()
        {
            return Duel.Player == 1 && Enemy.GetMonsterCount() >= 2;
        }

        // ═══════════════════════════════════════════════════════════════
        //  ACE MONSTER DISRUPTIONS & IN-ENGINE PROTECTION
        // ═══════════════════════════════════════════════════════════════

        private bool CosmicBlazarEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Option 0: Negate activation of card/effect
                // Option 1: Negate summon
                // Option 2: Negate attack and end Battle Phase
                if (LastChainCard != null && LastChainCard.Controller == 1)
                {
                    AI.SelectOption(0);
                    return true;
                }
                if (Duel.Player == 1 && Duel.Phase == DuelPhase.BattleStart)
                {
                    AI.SelectOption(2);
                    return true;
                }
            }
            return false;
        }

        private bool RedSupernovaEffect()
        {
            // Banish all opponent cards when opponent monster activates effect or declares attack
            return LastChainCard != null && LastChainCard.Controller == 1;
        }

        private bool BystialDisPaterEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Disruption: When opponent activates monster effect -> shuffle banished card to destroy/negate
                if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.Location == CardLocation.MonsterZone)
                {
                    var banished = Bot.Banished.Concat(Enemy.Banished).FirstOrDefault();
                    if (banished != null)
                    {
                        AI.SelectCard(banished);
                        return true;
                    }
                }

                // Ignition: Revive banished LIGHT or DARK monster
                if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
                {
                    var target = Bot.Banished.FirstOrDefault(c => c != null && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
                    if (target != null && Bot.GetMonsterCount() < 5)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool VictimSanctuaryEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (LastChainCard != null && LastChainCard.Controller == 1)
                {
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                return Bot.GetMonsterCount() < 5;
            }
            return false;
        }

        private bool CrystalWingEffect()
        {
            return LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.Location == CardLocation.MonsterZone;
        }

        private bool StardustWarriorEffect()
        {
            return true;
        }

        private bool SatelliteWarriorEffect()
        {
            // Pop cards up to Synchros in GY
            var enemyCards = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Where(c => c != null && !c.IsDisabled()).ToList();
            if (enemyCards.Count > 0)
            {
                AI.SelectCard(enemyCards);
                return true;
            }
            return false;
        }

        private bool CrimsonDragonEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On summon: search Synchro Rumble
                if (Duel.LastSummonedCards.Contains(Card))
                {
                    AI.SelectCard(CardId.SynchroRumble);
                    return true;
                }

                // Opponent turn: Tag out Lv 8 Synchro into Crystal Wing or Victim Sanctuary, or Lv 12 into Cosmic Blazar!
                if (Duel.Player == 1)
                {
                    var lv12 = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Level == 12 && m != Card);
                    if (lv12 != null)
                    {
                        AI.SelectCard(lv12);
                        AI.SelectNextCard(CardId.CosmicBlazarDragon, CardId.RedSupernovaDragon);
                        return true;
                    }

                    var lv8 = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.Level == 8 && m != Card);
                    if (lv8 != null)
                    {
                        AI.SelectCard(lv8);
                        AI.SelectNextCard(CardId.CrystalWingSynchroDragon, CardId.StardustDragonVictimSanctuary);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool StardustDragonEffect()
        {
            return LastChainCard != null && LastChainCard.Controller == 1;
        }

        private bool JunkSignalEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                AI.SelectOption(1); // Negate opponent effect
                return true;
            }

            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                var fodder = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.JetSynchron || m.Id == CardId.ScrapSynchron || m.Id == CardId.AssaultSynchron);
                if (fodder != null && Bot.GetMonsterCount() < 5)
                {
                    AI.SelectOption(0); // Tribute to Special Summon
                    AI.SelectCard(fodder);
                    AI.SelectNextCard(CardId.StardustDragonVictimSanctuary, CardId.StardustSynchron);
                    return true;
                }
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SEARCH & CONSISTENCY SPELLS
        // ═══════════════════════════════════════════════════════════════

        private bool RotaEffect()
        {
            AI.SelectCard(CardId.FullSpeedWarrior, CardId.Doppelwarrior, CardId.JunkConverter, CardId.JunkSynchron, CardId.StarjunkSynchron);
            return true;
        }

        private bool TuningEffect()
        {
            AI.SelectCard(CardId.JunkSynchron, CardId.StardustSynchron, CardId.StarjunkSynchron, CardId.AssaultSynchron, CardId.JetSynchron);
            return true;
        }

        private bool SynchroFellowshipEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                AI.SelectCard(CardId.JunkSynchron, CardId.FullSpeedWarrior, CardId.StardustSynchron);
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                var target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasType(CardType.Synchro) && m.Level >= 4);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return true;
        }

        private bool JunkConverterHandEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                var tuner = Bot.Hand.FirstOrDefault(h => h != Card && h.HasType(CardType.Tuner));
                if (tuner != null)
                {
                    AI.SelectCard(tuner);
                    AI.SelectNextCard(CardId.JunkSynchron);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                return Bot.GetMonsterCount() < 5;
            }
            return false;
        }

        private bool StardustIlluminationEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Dump Stardust Trail or Stardust Synchron to GY
                AI.SelectCard(CardId.StardustTrail, CardId.StardustSynchron);
                return true;
            }
            return false;
        }

        private bool SynchroRumbleEffect()
        {
            AI.SelectCard(
                CardId.CosmicBlazarDragon,
                CardId.BystialDisPater,
                CardId.CrystalWingSynchroDragon,
                CardId.StardustDragonVictimSanctuary,
                CardId.StardustDragon,
                CardId.AccelSynchroStardustDragon,
                CardId.StardustSynchron,
                CardId.JunkSynchron
            );
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  MONSTER EXTENDERS & SUMMONS
        // ═══════════════════════════════════════════════════════════════

        private bool DoppelwarriorEffect()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool StardustTrailEffect()
        {
            return Bot.GetMonsterCount() < 5;
        }

        private bool StarjunkSynchronEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c.Level == 2 && c.IsMonster());
                if (target != null && Bot.GetMonsterCount() <= 3)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool AssaultSynchronEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return Bot.GetMonsterCount() < 5 && Bot.LifePoints > 700;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                return Bot.GetMonsterCount() < 5;
            }
            return false;
        }

        private bool StardustSynchronEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                var fodder = Bot.GetMonsters().FirstOrDefault(m => m.Id == CardId.JetSynchron || m.Id == CardId.ScrapSynchron || m.Id == CardId.AssaultSynchron);
                if (fodder != null && Bot.GetMonsterCount() < 5)
                {
                    AI.SelectCard(fodder);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.StardustIllumination, CardId.SynchroFellowship, CardId.JunkSignal);
                return true;
            }
            return false;
        }

        private bool JetSynchronEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                var discard = Plugin.MaterialImpl.PickDiscardTarget(Bot.Hand.ToList());
                if (discard != null && Bot.GetMonsterCount() < 5)
                {
                    AI.SelectCard(discard);
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(CardId.JunkSynchron, CardId.JunkConverter);
                return true;
            }
            return false;
        }

        private bool FullSpeedWarriorSummon()
        {
            return true;
        }

        private bool FullSpeedWarriorEffect()
        {
            AI.SelectCard(CardId.SynchroFellowship, CardId.JunkSynchron, CardId.JunkSignal);
            return true;
        }

        private bool JunkSynchronSummon()
        {
            return true;
        }

        private bool JunkSynchronEffect()
        {
            var target = Bot.Graveyard.FirstOrDefault(c => c.Level == 2 && c.IsMonster());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true;
        }

        private bool FallbackNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXTRA DECK SYNCHROS
        // ═══════════════════════════════════════════════════════════════

        private bool ScrapWarriorSpSummon()
        {
            return true;
        }

        private bool ScrapWarriorEffect()
        {
            AI.SelectCard(CardId.JunkSynchron, CardId.FullSpeedWarrior);
            return true;
        }

        private bool JunkSpeederSpSummon()
        {
            return true;
        }

        private bool JunkSpeederEffect()
        {
            AI.SelectCard(
                CardId.JetSynchron,         // Lv 1
                CardId.AssaultSynchron,     // Lv 2
                CardId.StarjunkSynchron,    // Lv 3
                CardId.StardustSynchron     // Lv 4
            );
            return true;
        }

        private bool LibrarianSpSummon()
        {
            return true;
        }

        private bool LibrarianEffect()
        {
            return true;
        }

        private bool AccelSynchroSpSummon()
        {
            return true;
        }

        private bool AccelSynchroEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // In opponent turn: Tribute self -> SS Stardust Dragon, then Quick Synchro into Satellite Warrior or Cosmic Blazar!
                if (Duel.Player == 1)
                {
                    return true;
                }

                // In our turn: Revive Lv 2 or lower Tuner
                AI.SelectCard(CardId.AssaultSynchron, CardId.JetSynchron, CardId.ScrapSynchron);
                return true;
            }
            return true;
        }

        private bool BlackRoseDragonSpSummon()
        {
            return Duel.Player == 0 && (Enemy.GetMonsterCount() >= 2 || Enemy.GetSpellCount() >= 2);
        }

        private bool BlackRoseDragonEffect()
        {
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        private bool CrystalWingSpSummon()
        {
            return true;
        }

        private bool VictimSanctuarySpSummon()
        {
            return true;
        }

        private bool DisPaterSpSummon()
        {
            return true;
        }

        private bool StardustWarriorSpSummon()
        {
            return true;
        }

        private bool SatelliteWarriorSpSummon()
        {
            return true;
        }

        private bool CosmicBlazarSpSummon()
        {
            return true;
        }

        private bool RedSupernovaSpSummon()
        {
            return true;
        }

        private bool CrimsonDragonSpSummon()
        {
            return true;
        }

        private bool FormulaSynchronSpSummon()
        {
            return true;
        }

        private bool FormulaSynchronEffect()
        {
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  DECISION OVERRIDES (0 Violations / Anti-Pattern Safeguards)
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards != null && cards.Count > 0)
            {
                // 1. Deck Search (hint 506 [HINTMSG_ATOHAND] or cards all from Deck)
                if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
                {
                    var preferred = cards.Where(c =>
                        c.Id == CardId.SynchroFellowship ||
                        c.Id == CardId.JunkSynchron ||
                        c.Id == CardId.FullSpeedWarrior ||
                        c.Id == CardId.Doppelwarrior ||
                        c.Id == CardId.StardustSynchron ||
                        c.Id == CardId.StardustTrail ||
                        c.Id == CardId.JunkConverter ||
                        c.Id == CardId.StarjunkSynchron ||
                        c.Id == CardId.AssaultSynchron ||
                        c.Id == CardId.JetSynchron ||
                        c.Id == CardId.JunkSignal
                    ).ToList();

                    if (preferred.Count >= min)
                    {
                        return preferred.Take(max).ToList();
                    }
                }

                // 2. Removal (hint 503 [REMOVE], hint 502 [DESTROY], hint 504 [TOGRAVE], hint 505 [RTOHAND]): ALWAYS target Enemy cards!
                if (hint == 502 || hint == 503 || hint == 504 || hint == 505)
                {
                    var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                    if (enemyTargets.Count >= min)
                    {
                        return enemyTargets.OrderByDescending(c => c.Attack).Take(max).ToList();
                    }
                }

                // 3. Discard selection (hint 501 [HINTMSG_DISCARD])
                if (hint == 501)
                {
                    var target = Plugin.MaterialImpl.PickDiscardTarget(cards, min);
                    if (target != null)
                    {
                        return new List<ClientCard> { target };
                    }
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            // CRITICAL ANTI-PATTERN: Never accept opponent optional effects
            if (card != null && card.Controller == 1)
            {
                return false;
            }
            return base.OnSelectEffectYn(card, desc);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            var cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                // Handtraps (0 ATK) -> DEFENSE
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // Low ATK Tuners / Extenders (ATK < 1500 && DEF >= ATK) -> DEFENSE
                if (cardData.Defense >= cardData.Attack && cardData.Attack < 1500)
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                }

                // Bosses & High ATK (ATK >= 1800) -> ATTACK
                if (cardData.Attack >= 1800 && positions.Contains(CardPosition.FaceUpAttack))
                {
                    return CardPosition.FaceUpAttack;
                }
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }
}
