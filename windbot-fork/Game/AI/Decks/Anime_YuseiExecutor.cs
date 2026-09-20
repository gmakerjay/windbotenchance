// ============================================================================
// CARD AUDIT — Anime_Yusei (Yusei Fudo's Ultimate Synchro Battlebox)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Wheel Synchron                     | Monster L5 T | Yes  | Yes   | None    | Treated as non-Tuner; extra NS L4-; GY -Lvl 4 | Main Phase extra NS; GY banish level modulate| Field full / no NS targets in hand          |
// | Stardust Synchron                  | Monster L4 T | Yes  | Yes   | Tribute | SS from hand/GY by tributing; search S/T      | In hand/GY, fodder available; on SS search    | Field full / no tribute fodder              |
// | Junk Meister                       | Monster L4   | Yes  | Yes   | None    | SS from hand if control Junk/Synchron; draw 1 | Control Junk/Synchron; Synchro material draw  | No Junk/Synchron on field                   |
// | Junk Synchron                      | Monster L3 T | No   | No    | None    | On NS: Revive Level 2 or lower from GY        | Normal Summoned with L<=2 target in GY        | GY has no L<=2 targets                      |
// | Starjunk Synchron                  | Monster L3 T | Yes  | Yes   | None    | SS from hand if control Synchron/Warrior      | Control Synchron or Warrior                  | No Synchron/Warrior on field                |
// | Full-Speed Warrior                 | Monster L2   | Yes  | Yes   | None    | SS from hand if control Warrior/Synchron      | Control Warrior or Synchron                  | No Warrior/Synchron on field                |
// | Anchorbolt Hedgehog                | Monster L2   | Yes  | Yes   | None    | GY Quick: SS if control Junk Warrior mention  | In GY, control Junk Warrior/mention          | Already SS this turn                        |
// | Assault Synchron                   | Monster L2 T | Yes  | Yes   | 700 LP  | SS taking 700 dmg; GY: revive Dragon Synchro  | Main Phase climb; Dragon Synchro tributed/ban | LP <= 700                                   |
// | Junk Converter                     | Monster L2   | Yes  | Yes   | Discard | Discard w/ Tuner -> Search Synchron; revive   | In hand with Tuner; sent as Synchro material | No Tuner in hand                            |
// | Scrap Synchron                     | Monster L1 T | Yes  | Yes   | None    | SS if control Scrap; pop card on Synchro send | Control Scrap; sent as Synchro material       | No Scrap on field                           |
// | Jet Synchron                       | Monster L1 T | Yes  | Yes   | Discard | Search Junk on send; discard to revive from GY| Sent to GY; in GY needing L1 Tuner           | Hand empty for revival                      |
// | Crossroad Sonic Chick              | Monster L1   | Yes  | Yes   | None    | Level change to 3 or 4 for Synchro material   | On field, need Level 3 or 4 non-Tuner material| Already desired Level                       |
// | Ash Blossom & Joyous Spring        | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent activates deck-interacting effect    | Bot's own turn without threats              |
// | Ghost Belle & Haunted Mansion      | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate GY move/banish/revive        | Opponent activates GY effect                  | Bot's own turn without threats              |
// | Ghost Mourner & Moonlit Chill      | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate face-up monster SS & burn    | Opponent Special Summons face-up monster      | Target already negated                      |
// | Ghost Ogre & Snow Rabbit           | Monster L3 T | Yes  | Yes   | Send    | Handtrap: Destroy card activating effect      | Opp monster or cont S/T activates on field    | Card cannot be destroyed                    |
// | Effect Veiler                      | Monster L1 T | Yes  | Yes   | Send    | Handtrap: Negate opp monster during opp MP    | Opponent Main Phase threat monster            | Already negated or not in opp MP            |
// | Stardust Dragon-Victim Sanctuary   | Monster L8   | Yes  | Yes   | Send    | Hand Quick: Negate opp effect targeting mon   | Opponent targets bot monster on field         | Opponent does not target                    |
// | Synchro Fellowship                 | Spell Normal | Yes  | Yes   | Discard | Add Junk Synchron + Junk/Stardust mention mon | Main Phase starter; GY banish extra NS        | Already used this turn                      |
// | Tuning                             | Spell Normal | No   | No    | Mill 1  | Add 1 "Synchron" Tuner from Deck to hand      | Main Phase early search starter               | Deck has no Synchron Tuners                 |
// | Junk Signal                        | Spell Quick  | Yes  | Yes   | Tribute | SS Stardust/Junk from Dk/GY OR negate chain   | Opp chains to Synchro OR tribute to cheat SS  | Already used this turn                      |
// | Scrap-Iron Sacred Statue           | Trap Normal  | Yes  | Yes   | Target  | Revive Dragon Synchro L7/8, resets itself     | GY/banished Dragon Synchro L7/8               | Target already on field                     |
// | Majestic Mirage                    | Trap Cont    | Yes  | Yes   | None    | Protects Stardust / Stardust dragon effects   | Control Stardust Dragon / Synchro boss        | No Stardust monsters on field               |
// | Junk Speeder                       | Synchro L5   | Yes  | Yes   | None    | On SS: Summon multiple Synchron tuners from Dk| Synchro Summoned (primary combo engine)       | Monster zones full                          |
// | Formula Synchron                   | Synchro L2 T | No   | No    | None    | Draw 1 card on Synchro Summon; Quick Synchro  | Synchro Summoned; Opp turn Quick Synchro      | Deck empty                                  |
// | Scrap Warrior                      | Synchro L3   | Yes  | Yes   | None    | Boost Warrior/Stardust 500 ATK; pop on summon | Synchro Summoned stepping stone               | Opponent unaffected                         |
// | Stardust Charge Warrior            | Synchro L6   | No   | No    | None    | Draw 1 card on Synchro Summon; attack all mon | Synchro Summoned (draw engine / laddering)    | Deck empty                                  |
// | Shooting Riser Dragon              | Synchro L7 T | Yes  | Yes   | Send    | Dump card from Deck to modulate level; Quick  | Synchro Summoned; Opp turn Quick Synchro      | Already modulated                           |
// | Accel Synchro Stardust Dragon      | Synchro L8   | Yes  | Yes   | Tribute | Revive L<=2 Tuner; Quick: SS Stardust & climb | Synchro Summoned; Main Phase Quick Synchro    | Opponent unaffected                         |
// | Stardust Dragon                    | Synchro L8   | Yes  | Yes   | Tribute | Negate card/effect that destroys on field     | Opponent effect would destroy card(s)         | No destruction effect                       |
// | Stardust Warrior                   | Synchro L10  | Yes  | Yes   | Tribute | Quick: Negate opp SS & destroy, recurses EP   | Opponent Special Summons monster              | Already used this turn                      |
// | Satellite Warrior                  | Synchro L10  | Yes  | Yes   | None    | Pop opp cards up to Synchros in GY, +1000 ATK | Board breaking / going second OTK             | Opponent has no cards                       |
// | Shooting Star Dragon               | Synchro L10  | Yes  | Yes   | None    | Multi-attack 5-excavate; negate destruction   | Battle push / destruction protection          | Bot has lethal                              |
// | Crimson Dragon                     | Synchro L12  | Yes  | Yes   | Tag-out | Search S/T; Quick: tag out into same Lvl Drag | Target L7+ Dragon on field to cheat boss      | No matching Dragon in Extra Deck            |
// | Crimson Dragon Quetzacoatl         | Synchro L12  | Yes  | Yes   | Banish  | Quick: Banish until EP to negate attack       | Opponent declares attack                      | Bot has lethal                              |
// | Cosmic Blazar Dragon               | Synchro L12  | Yes  | Yes   | Banish  | 4000 ATK Omni-negate (eff / summon / attack)  | Opponent activates card/eff or summons        | Already used this turn                      |
// | Shooting Quasar Dragon             | Synchro L12  | Yes  | Yes   | None    | 4000 ATK multi-attack, Omni-negate + destroy  | Opponent activates card/eff; battle push      | Already negated this turn                   |
// | Shooting Majestic Star Dragon      | Synchro L11  | Yes  | Yes   | Banish  | 4000 ATK Negate opp monster; Omni-negate & ban| Opponent activates card/eff or monster threat | Already negated this turn                   |
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
    [Deck("Anime_Yusei", "Anime_Yusei")]
    public class Anime_YuseiExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int WheelSynchron = 60283232;
            public const int StardustSynchron = 37799519;
            public const int JunkMeister = 73218792;
            public const int JunkSynchron = 63977008;
            public const int StarjunkSynchron = 13021682;
            public const int AshBlossom = 14558127;
            public const int GhostBelle = 73642296;
            public const int GhostMourner = 52038441;
            public const int GhostOgre = 59438930;
            public const int FullSpeedWarrior = 17201951;
            public const int AnchorboltHedgehog = 54656950;
            public const int AssaultSynchron = 77202120;
            public const int JunkConverter = 11069680;
            public const int ScrapSynchron = 16449363;
            public const int JetSynchron = 9742784;
            public const int CrossroadSonicChick = 80054655;
            public const int EffectVeiler = 97268402;
            public const int StardustDragonVictimSanctuary = 76636978;

            // Spells
            public const int SynchroFellowship = 43834302;
            public const int Tuning = 96363153;
            public const int JunkSignal = 26387390;

            // Traps
            public const int ScrapIronSacredStatue = 85520170;
            public const int MajesticMirage = 98020526;

            // Extra Deck
            public const int CosmicBlazarDragon = 21123811;
            public const int ShootingMajesticStarDragon = 40939228;
            public const int ShootingQuasarDragon = 35952884;
            public const int CrimsonDragon = 63436931;
            public const int CrimsonDragonQuetzacoatl = 29053656;
            public const int ShootingStarDragon = 24696097;
            public const int StardustWarrior = 74892653;
            public const int SatelliteWarrior = 84664085;
            public const int AccelSynchroStardustDragon = 30983281;
            public const int StardustDragon = 44508094;
            public const int ShootingRiserDragon = 68431965;
            public const int StardustChargeWarrior = 64880894;
            public const int JunkSpeeder = 77075360;
            public const int ScrapWarrior = 42711820;
            public const int FormulaSynchron = 50091196;
        }

        public Anime_YuseiExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterExecutors();
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High-Priority Negates & Disruption (Chain / Enemy Turn)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.CosmicBlazarDragon, CosmicBlazarNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.ShootingMajesticStarDragon, ShootingMajesticNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.ShootingQuasarDragon, ShootingQuasarNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.StardustWarrior, StardustWarriorNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.ShootingStarDragon, ShootingStarDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.StardustDragonVictimSanctuary, VictimSanctuaryActivate);
            AddExecutor(ExecutorType.Activate, CardId.StardustDragon, StardustDragonNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.JunkSignal, JunkSignalActivate);

            // Handtraps
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostMourner, GhostMournerActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, GhostOgreActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerActivate);

            // Assault Synchron GY Revival (Trigger when Dragon Synchro is tributed/banished)
            AddExecutor(ExecutorType.Activate, CardId.AssaultSynchron, AssaultSynchronGraveActivate);

            // Scrap Synchron & Scrap Warrior Destructions
            AddExecutor(ExecutorType.Activate, CardId.ScrapSynchron, ScrapSynchronEffect);
            AddExecutor(ExecutorType.Activate, CardId.ScrapWarrior, ScrapWarriorEffect);

            // -------------------------------------------------------------
            // 2. Main Phase 1 Starters & Searches
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.SynchroFellowship, SynchroFellowshipActivate);
            AddExecutor(ExecutorType.Activate, CardId.Tuning, TuningActivate);
            AddExecutor(ExecutorType.Activate, CardId.JunkConverter, JunkConverterActivate);
            AddExecutor(ExecutorType.Activate, CardId.WheelSynchron, WheelSynchronEffect);

            // -------------------------------------------------------------
            // 3. Main Phase Monster Special Summons (Extenders)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.StardustSynchron, StardustSynchronEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.JunkMeister, JunkMeisterSummon);
            AddExecutor(ExecutorType.Activate, CardId.JunkMeister, JunkMeisterEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.StarjunkSynchron, StarjunkSynchronSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FullSpeedWarrior, FullSpeedWarriorSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AssaultSynchron, AssaultSynchronSummon);
            AddExecutor(ExecutorType.Activate, CardId.AnchorboltHedgehog, AnchorboltHedgehogEffect);
            AddExecutor(ExecutorType.Activate, CardId.JetSynchron, JetSynchronEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossroadSonicChick, SonicChickEffect);

            // -------------------------------------------------------------
            // 4. Normal Summons
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Summon, CardId.JunkSynchron, JunkSynchronSummon);
            AddExecutor(ExecutorType.Activate, CardId.JunkSynchron, JunkSynchronEffect);
            AddExecutor(ExecutorType.Summon, CardId.WheelSynchron, WheelSynchronNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.StardustSynchron, StardustSynchronSummon);
            AddExecutor(ExecutorType.Summon, CardId.JunkConverter, JunkConverterSummon);
            AddExecutor(ExecutorType.Summon, CardId.AssaultSynchron, AssaultSynchronNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.StarjunkSynchron, StarjunkSynchronNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.JetSynchron, JetSynchronNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.ScrapSynchron, ScrapSynchronNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.FullSpeedWarrior, FullSpeedWarriorNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.CrossroadSonicChick, SonicChickNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.JunkMeister, JunkMeisterNormalSummon);

            // -------------------------------------------------------------
            // 5. Extra Deck Synchro Summons (Hierarchy & Laddering)
            // -------------------------------------------------------------
            // Step 1: Junk Speeder (L5 Combo Engine)
            AddExecutor(ExecutorType.SpSummon, CardId.JunkSpeeder, JunkSpeederSummon);
            AddExecutor(ExecutorType.Activate, CardId.JunkSpeeder, JunkSpeederEffect);

            // Step 2: Intermediate Laddering (L2 / L3 / L6 / L7)
            AddExecutor(ExecutorType.SpSummon, CardId.FormulaSynchron, FormulaSynchronSummon);
            AddExecutor(ExecutorType.Activate, CardId.FormulaSynchron, FormulaSynchronEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ScrapWarrior, ScrapWarriorSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.StardustChargeWarrior, StardustChargeWarriorSummon);
            AddExecutor(ExecutorType.Activate, CardId.StardustChargeWarrior, StardustChargeWarriorEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ShootingRiserDragon, ShootingRiserSummon);
            AddExecutor(ExecutorType.Activate, CardId.ShootingRiserDragon, ShootingRiserEffect);

            // Step 3: Level 8 Synchros (Accel Stardust & Stardust Dragon)
            AddExecutor(ExecutorType.SpSummon, CardId.AccelSynchroStardustDragon, AccelStardustSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccelSynchroStardustDragon, AccelStardustEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.StardustDragon, StardustDragonSummon);

            // Step 4: Level 10 Bosses (Satellite Warrior / Stardust Warrior / Shooting Star Dragon)
            AddExecutor(ExecutorType.SpSummon, CardId.SatelliteWarrior, SatelliteWarriorSummon);
            AddExecutor(ExecutorType.Activate, CardId.SatelliteWarrior, SatelliteWarriorEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.StardustWarrior, StardustWarriorSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ShootingStarDragon, ShootingStarDragonSummon);

            // Step 5: Level 12 Ultimate Bosses (Cosmic Blazar / Shooting Quasar / Crimson Dragon)
            AddExecutor(ExecutorType.SpSummon, CardId.CosmicBlazarDragon, CosmicBlazarSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ShootingQuasarDragon, ShootingQuasarSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrimsonDragon, CrimsonDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.CrimsonDragon, CrimsonDragonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ShootingMajesticStarDragon, ShootingMajesticSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrimsonDragonQuetzacoatl, QuetzacoatlSummon);
            AddExecutor(ExecutorType.Activate, CardId.CrimsonDragonQuetzacoatl, QuetzacoatlEffect);

            // -------------------------------------------------------------
            // 6. Backrow Support & Spells
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.MajesticMirage, MajesticMirageActivate);
            AddExecutor(ExecutorType.Activate, CardId.ScrapIronSacredStatue, ScrapIronStatueActivate);
            AddExecutor(ExecutorType.SpellSet, SpellSetStrategy);
            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // =================================================================
        // EXECUTION IMPLEMENTATIONS — DISRUPTIONS & HANDTRAPS
        // =================================================================

        private bool CosmicBlazarNegateActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 0) return true;
            if (Duel.LastSummonPlayer != 0) return true;
            if (Duel.Phase == DuelPhase.BattleStart && Enemy.HasAttackingMonster()) return true;
            return false;
        }

        private bool ShootingMajesticNegateActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 0) return true;
            ClientCard oppMonster = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && !IsTargetImmune(m) && (m.Attack >= 2000 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)));
            if (oppMonster != null)
            {
                AI.SelectCard(oppMonster);
                return true;
            }
            return false;
        }

        private bool ShootingQuasarNegateActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                return Duel.LastChainPlayer != 0;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Float into Shooting Star Dragon when leaving field
                AI.SelectCard(CardId.ShootingStarDragon);
                return true;
            }
            return false;
        }

        private bool StardustWarriorNegateActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                // Negate opponent Special Summon
                return Duel.LastSummonPlayer != 0;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Floating when leaving field or End Phase recursion
                AI.SelectCard(new[] {
                    CardId.StardustChargeWarrior,
                    CardId.ScrapWarrior
                });
                return true;
            }
            return false;
        }

        private bool VictimSanctuaryActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Negate effect targeting bot's monster
            return Duel.LastChainPlayer != 0;
        }

        private bool StardustDragonNegateActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            return Duel.LastChainPlayer != 0;
        }

        private bool JunkSignalActivate()
        {
            if (Duel.LastChainPlayer != 0)
            {
                // Negate opponent response to Synchro monster effect
                AI.SelectOption(1);
                return true;
            }
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                if (HasLethalOnBoard()) return false;
                // Tribute small fodder to cheat out Stardust Dragon or Junk/Stardust mention
                ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m.Level <= 2 && !m.HasType(CardType.Synchro));
                if (tribute != null)
                {
                    AI.SelectOption(0);
                    AI.SelectCard(tribute);
                    AI.SelectNextCard(new[] {
                        CardId.StardustDragon,
                        CardId.StardustSynchron,
                        CardId.JunkSynchron
                    });
                    return true;
                }
            }
            return false;
        }

        private bool AshBlossomActivate()
        {
            if (Duel.LastChainPlayer == 0) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool GhostBelleActivate()
        {
            if (Duel.LastChainPlayer == 0) return false;
            return DefaultGhostBelleAndHauntedMansion();
        }

        private bool GhostMournerActivate()
        {
            if (Duel.LastChainPlayer == 0) return false;
            ClientCard oppMonster = Enemy.GetMonsters().LastOrDefault(m => m.IsFaceup() && !m.IsDisabled() && !IsTargetImmune(m));
            if (oppMonster != null)
            {
                AI.SelectCard(oppMonster);
                return true;
            }
            return false;
        }

        private bool GhostOgreActivate()
        {
            if (Duel.LastChainPlayer == 0) return false;
            return DefaultGhostOgreAndSnowRabbit();
        }

        private bool EffectVeilerActivate()
        {
            if (Duel.LastChainPlayer == 0) return false;
            if (Duel.Player != 1) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && !IsTargetImmune(m) && (m.Attack >= 1800 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool AssaultSynchronGraveActivate()
        {
            if (Card.Location != CardLocation.Grave) return false;
            // Target Dragon Synchro that was tributed or banished (e.g. Stardust, Accel Stardust, Blazar)
            ClientCard dragon = Bot.Graveyard.Concat(Bot.Banished).FirstOrDefault(c => c.HasType(CardType.Synchro) && c.HasRace(CardRace.Dragon));
            if (dragon != null)
            {
                AI.SelectCard(dragon);
                return true;
            }
            return false;
        }

        private bool ScrapSynchronEffect()
        {
            // Pop opponent card when sent as Synchro material
            ClientCard target = Enemy.GetMonsters().Where(m => !IsTargetImmune(m)).OrderByDescending(m => GetCardThreatScore(m)).FirstOrDefault();
            if (target == null) target = Enemy.GetSpells().OrderByDescending(s => GetCardThreatScore(s)).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ScrapWarriorEffect()
        {
            // Pop opponent card on summon if summoned using Junk Warrior material
            ClientCard target = Enemy.GetMonsters().Where(m => !IsTargetImmune(m)).OrderByDescending(m => GetCardThreatScore(m)).FirstOrDefault();
            if (target == null) target = Enemy.GetSpells().OrderByDescending(s => GetCardThreatScore(s)).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // =================================================================
        // MAIN PHASE 1 STARTERS & SEARCHES
        // =================================================================

        private bool SynchroFellowshipActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Add 1 Junk Synchron + 1 monster mentioning Junk Warrior or Stardust Dragon (Stardust Synchron or Anchorbolt Hedgehog)
                AI.SelectCard(CardId.JunkSynchron);
                AI.SelectNextCard(CardId.StardustSynchron, CardId.AnchorboltHedgehog);
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Banish from GY to reduce Synchro level by 1 and gain extra Normal Summon of "Synchron"
                ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.HasType(CardType.Synchro) && m.Level >= 5);
                if (target != null && (Bot.Hand.Any(c => c.IsTuner() && c.Name.Contains("Synchron")) || Bot.GetMonsterCount() < 5))
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool TuningActivate()
        {
            // Search Synchron Tuner (Junk Synchron > Stardust Synchron > Assault Synchron > Jet Synchron > Wheel Synchron)
            AI.SelectCard(new[] {
                CardId.JunkSynchron,
                CardId.StardustSynchron,
                CardId.AssaultSynchron,
                CardId.JetSynchron,
                CardId.WheelSynchron,
                CardId.StarjunkSynchron
            });
            return true;
        }

        private bool JunkConverterActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Discard with Tuner to search Synchron
                if (!Bot.Hand.Any(c => c.HasType(CardType.Tuner) && c != Card)) return false;
                AI.SelectCard(new[] {
                    CardId.JunkSynchron,
                    CardId.StardustSynchron,
                    CardId.AssaultSynchron,
                    CardId.WheelSynchron
                });
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // On sent as material: revive Tuner from GY
                AI.SelectCard(new[] {
                    CardId.JunkSynchron,
                    CardId.StardustSynchron,
                    CardId.AssaultSynchron,
                    CardId.JetSynchron
                });
                return true;
            }
            return false;
        }

        private bool WheelSynchronEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Main Phase: Immediately Normal Summon 1 Level 4 or lower monster from hand
                if (Bot.Hand.Any(c => c.Level <= 4 && c.IsMonster() && c != Card))
                {
                    AI.SelectCard(new[] {
                        CardId.JunkSynchron,
                        CardId.StardustSynchron,
                        CardId.JunkConverter,
                        CardId.AssaultSynchron,
                        CardId.JetSynchron
                    });
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                // GY banish: Reduce level of 1 Synchro monster by up to 4
                ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasType(CardType.Synchro) && m.Level >= 6);
                if (target != null && Bot.GetMonsters().Any(m => m.IsTuner()))
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        // =================================================================
        // EXTENDERS & MONSTER SPECIAL SUMMONS
        // =================================================================

        private bool StardustSynchronEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                if (Bot.GetMonsterCount() >= 5) return false;
                // Tribute low value fodder to Special Summon itself
                ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m.Level <= 2 && !m.HasType(CardType.Synchro) && !IsMaterialBossProtected(m));
                if (tribute != null)
                {
                    AI.SelectCard(tribute);
                    return true;
                }
                return false;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On SS: search Spell/Trap mentioning Stardust Dragon
                AI.SelectCard(new[] {
                    CardId.SynchroFellowship,
                    CardId.JunkSignal,
                    CardId.MajesticMirage
                });
                return true;
            }
            return false;
        }

        private bool JunkMeisterSummon()
        {
            if (Bot.GetMonsterCount() >= 5) return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Name.Contains("Junk") || m.Name.Contains("Synchron")));
        }

        private bool JunkMeisterEffect()
        {
            // Draw 1 card when used as Synchro material
            return true;
        }

        private bool StarjunkSynchronSummon()
        {
            if (Bot.GetMonsterCount() >= 5) return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Name.Contains("Synchron") || m.HasRace(CardRace.Warrior)));
        }

        private bool FullSpeedWarriorSummon()
        {
            if (Bot.GetMonsterCount() >= 5) return false;
            return Bot.GetMonsters().Any(m => m.IsFaceup() && (m.HasRace(CardRace.Warrior) || m.Name.Contains("Synchron")));
        }

        private bool AssaultSynchronSummon()
        {
            if (Bot.GetMonsterCount() >= 5) return false;
            return Bot.LifePoints > 1000;
        }

        private bool AnchorboltHedgehogEffect()
        {
            if (Bot.GetMonsterCount() >= 5) return false;
            // Revive if controlling monster mentioning Junk Warrior (Scrap Warrior or Anchorbolt Hedgehog)
            return Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Id == CardId.ScrapWarrior || m.Id == CardId.AnchorboltHedgehog));
        }

        private bool JetSynchronEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Discard 1 to revive from GY
                if (Bot.Hand.Count > 0 && Bot.GetMonsterCount() < 5 && !Bot.GetMonsters().Any(m => m.Id == CardId.JetSynchron))
                {
                    return true;
                }
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Search Junk monster when sent as Synchro material
                AI.SelectCard(new[] {
                    CardId.JunkSynchron,
                    CardId.JunkConverter,
                    CardId.JunkMeister
                });
                return true;
            }
            return false;
        }

        private bool SonicChickEffect()
        {
            // Level modulation to 3 or 4
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectOption(1); // Set level to 4
                return true;
            }
            return false;
        }

        // =================================================================
        // NORMAL SUMMONS
        // =================================================================

        private bool JunkSynchronSummon()
        {
            return true;
        }

        private bool JunkSynchronEffect()
        {
            // Revive Level <= 2 monster from GY
            AI.SelectCard(new[] {
                CardId.JunkConverter,
                CardId.AssaultSynchron,
                CardId.AnchorboltHedgehog,
                CardId.JetSynchron,
                CardId.FullSpeedWarrior
            });
            return true;
        }

        private bool WheelSynchronNormalSummon()
        {
            return Bot.GetMonsterCount() == 0 || Bot.Hand.Any(c => c.Level <= 4 && c.IsMonster() && c != Card);
        }

        private bool StardustSynchronSummon()
        {
            return Bot.GetMonsterCount() == 0 || !Bot.Hand.Any(c => c.Id == CardId.JunkSynchron);
        }

        private bool JunkConverterSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool AssaultSynchronNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool StarjunkSynchronNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool JetSynchronNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool ScrapSynchronNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool FullSpeedWarriorNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool SonicChickNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool JunkMeisterNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        // =================================================================
        // EXTRA DECK SYNCHRO SUMMONS
        // =================================================================

        private bool JunkSpeederSummon()
        {
            if (HasLethalOnBoard()) return false;
            // Primary combo engine: summon only when we have at least 3 free zones
            int freeZones = 5 - Bot.GetMonsterCount();
            return freeZones >= 2 && !Bot.HasInMonstersZone(CardId.JunkSpeeder);
        }

        private bool JunkSpeederEffect()
        {
            // Summon Synchron Tuners with different levels from Deck in Defense
            AI.SelectPosition(CardPosition.FaceUpDefence);
            AI.SelectCard(new[] {
                CardId.StardustSynchron,  // L4
                CardId.JunkSynchron,      // L3
                CardId.AssaultSynchron,   // L2
                CardId.JetSynchron,       // L1
                CardId.WheelSynchron      // L5
            });
            return true;
        }

        private bool FormulaSynchronSummon()
        {
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool FormulaSynchronEffect()
        {
            if (Duel.Player == 0) return true; // Draw 1 on summon
            // Opponent turn Quick Synchro
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                return Bot.GetMonsters().Any(m => m != Card && m.IsFaceup() && !m.IsTuner() && (m.Level == 8 || m.Level == 7));
            }
            return false;
        }

        private bool ScrapWarriorSummon()
        {
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool StardustChargeWarriorSummon()
        {
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool StardustChargeWarriorEffect()
        {
            return true; // Draw 1 on summon
        }

        private bool ShootingRiserSummon()
        {
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool ShootingRiserEffect()
        {
            if (Duel.Player == 0)
            {
                // Dump monster from Deck to modulate level
                AI.SelectCard(new[] {
                    CardId.AnchorboltHedgehog,
                    CardId.AssaultSynchron,
                    CardId.JetSynchron,
                    CardId.FullSpeedWarrior
                });
                return true;
            }
            // Opponent turn Quick Synchro
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                return Bot.GetMonsters().Any(m => m != Card && m.IsFaceup() && !m.IsTuner());
            }
            return false;
        }

        private bool AccelStardustSummon()
        {
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool AccelStardustEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Effect 1: Revive Level <= 2 Tuner on summon
            if (Bot.Graveyard.Any(c => c.IsTuner() && c.Level <= 2))
            {
                AI.SelectCard(new[] {
                    CardId.AssaultSynchron,
                    CardId.FormulaSynchron,
                    CardId.JetSynchron
                });
                return true;
            }

            // Effect 2: Quick Effect to tribute itself for Stardust Dragon + Unaffected Synchro
            if (!Bot.HasInMonstersZone(CardId.CosmicBlazarDragon) && !Bot.HasInMonstersZone(CardId.ShootingQuasarDragon))
            {
                bool hasOtherMonster = Bot.GetMonsters().Any(m => m != Card && m.IsFaceup());
                if (hasOtherMonster && Bot.ExtraDeck.Any(e => e.Id == CardId.StardustDragon))
                {
                    return true;
                }
            }
            return false;
        }

        private bool StardustDragonSummon()
        {
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool SatelliteWarriorSummon()
        {
            if (HasLethalOnBoard()) return false;
            // Primary board breaker when going second or opponent has cards
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0 || Duel.Phase == DuelPhase.Main2;
        }

        private bool SatelliteWarriorEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Target opponent cards up to Synchros in GY
                var targets = Enemy.GetMonsters().Where(m => !IsTargetImmune(m)).OrderByDescending(m => GetCardThreatScore(m))
                    .Concat(Enemy.GetSpells().OrderByDescending(s => GetCardThreatScore(s)))
                    .ToList();
                if (targets.Count > 0)
                {
                    AI.SelectCard(targets);
                }
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Float into up to 3 L<=8 Synchros on destruction
                AI.SelectCard(new[] {
                    CardId.AccelSynchroStardustDragon,
                    CardId.StardustDragon,
                    CardId.StardustChargeWarrior,
                    CardId.ScrapWarrior,
                    CardId.FormulaSynchron
                });
                return true;
            }
            return false;
        }

        private bool StardustWarriorSummon()
        {
            if (HasLethalOnBoard()) return false;
            if (Bot.HasInMonstersZone(CardId.StardustWarrior)) return false;
            return true;
        }

        private bool ShootingStarDragonSummon()
        {
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool ShootingStarDragonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            if (Duel.LastChainPlayer != 0) return true; // Negate destruction
            if (Duel.Phase == DuelPhase.Main1) return true; // Excavate 5 to multi-attack
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle) return true; // Negate attack
            return false;
        }

        private bool CosmicBlazarSummon()
        {
            if (HasLethalOnBoard()) return false;
            if (Bot.HasInMonstersZone(CardId.CosmicBlazarDragon)) return false;
            return true;
        }

        private bool ShootingMajesticSummon()
        {
            if (HasLethalOnBoard()) return false;
            if (Bot.HasInMonstersZone(CardId.ShootingMajesticStarDragon)) return false;
            return true;
        }

        private bool ShootingQuasarSummon()
        {
            if (HasLethalOnBoard()) return false;
            if (Bot.HasInMonstersZone(CardId.ShootingQuasarDragon)) return false;
            return true;
        }

        private bool CrimsonDragonSummon()
        {
            if (HasLethalOnBoard()) return false;
            if (Bot.HasInMonstersZone(CardId.CrimsonDragon)) return false;
            return true;
        }

        private bool CrimsonDragonEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;

            // Search S/T mentioning Crimson Dragon (Scrap-Iron Sacred Statue) on SS
            if (!Bot.HasInHand(CardId.ScrapIronSacredStatue) && Bot.Deck.Any(c => c.Id == CardId.ScrapIronSacredStatue))
            {
                AI.SelectCard(CardId.ScrapIronSacredStatue);
                return true;
            }

            // Quick Tag-out: Target Level 7+ Dragon Synchro on field
            var eligibleTargets = Bot.GetMonsters().Where(m => m != Card && m.IsFaceup() && m.HasType(CardType.Synchro) && m.HasRace(CardRace.Dragon) && m.Level >= 7).ToList();
            foreach (var target in eligibleTargets)
            {
                if (Bot.ExtraDeck.Any(e => e.HasType(CardType.Synchro) && e.HasRace(CardRace.Dragon) && e.Level == target.Level))
                {
                    AI.SelectCard(target);
                    AI.SelectNextCard(new[] {
                        CardId.CosmicBlazarDragon,
                        CardId.ShootingQuasarDragon,
                        CardId.ShootingStarDragon,
                        CardId.AccelSynchroStardustDragon,
                        CardId.StardustDragon
                    });
                    return true;
                }
            }
            return false;
        }

        private bool QuetzacoatlSummon()
        {
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool QuetzacoatlEffect()
        {
            // Negate attack by banishing until End Phase
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
            {
                return true;
            }
            return false;
        }

        // =================================================================
        // BACKROW SUPPORT & POSITIONING
        // =================================================================

        private bool MajesticMirageActivate()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                return Bot.GetMonsters().Any(m => m.HasType(CardType.Synchro) && m.Name.Contains("Stardust"));
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Trigger when Stardust leaves field: Option 0 = SS it back!
                AI.SelectOption(0);
                return true;
            }
            return false;
        }

        private bool ScrapIronStatueActivate()
        {
            // Target 1 Level 7 or 8 Dragon Synchro in GY or banished
            var dragon = Bot.Graveyard.Concat(Bot.Banished).FirstOrDefault(c => c.HasType(CardType.Synchro) && c.HasRace(CardRace.Dragon) && (c.Level == 7 || c.Level == 8));
            if (dragon != null)
            {
                AI.SelectCard(dragon);
                return true;
            }
            // If destroyed while set: SS Crimson Dragon from Extra Deck
            if (Bot.ExtraDeck.Any(e => e.Id == CardId.CrimsonDragon))
            {
                AI.SelectCard(CardId.CrimsonDragon);
                return true;
            }
            return false;
        }

        private bool SpellSetStrategy()
        {
            if (Card.IsTrap()) return true;
            if (Card.IsSpell() && Card.HasType(CardType.QuickPlay))
            {
                return Duel.Phase == DuelPhase.Main2;
            }
            return false;
        }

        private bool RepositionStrategy()
        {
            if (Card.Attack < 1500 && Card.IsAttack()) return true;
            if (Card.Attack >= 2000 && Card.IsDefense()) return true;
            return false;
        }

        // =================================================================
        // STRATEGIC OVERRIDES & ANTI-PATTERN DEFENSES
        // =================================================================

        private bool HasLethalOnBoard()
        {
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (Enemy.GetMonsterCount() > 0)
            {
                int enemyDefAtk = Enemy.GetMonsters().Sum(m => m.IsAttack() ? m.Attack : m.Defense);
                int botAtk = Bot.GetMonsters().Where(m => m.IsFaceup() && m.IsAttack()).Sum(m => m.Attack);
                return botAtk - enemyDefAtk >= Enemy.LifePoints;
            }
            int totalAtk = Bot.GetMonsters().Where(m => m.IsFaceup() && m.IsAttack()).Sum(m => m.Attack);
            return totalAtk >= Enemy.LifePoints;
        }

        private bool IsMaterialBossProtected(ClientCard card)
        {
            if (card == null) return false;
            int id = card.Id;
            return id == CardId.CosmicBlazarDragon ||
                   id == CardId.ShootingQuasarDragon ||
                   id == CardId.ShootingMajesticStarDragon ||
                   id == CardId.StardustWarrior ||
                   id == CardId.SatelliteWarrior;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Rule 1: Search / Add to hand (HINTMSG_ATOHAND = 506 / 505)
            if (hint == 506 || hint == 505)
            {
                var preferred = new List<int>
                {
                    CardId.SynchroFellowship,
                    CardId.Tuning,
                    CardId.JunkSynchron,
                    CardId.StardustSynchron,
                    CardId.JunkConverter,
                    CardId.AssaultSynchron,
                    CardId.JetSynchron,
                    CardId.WheelSynchron,
                    CardId.AnchorboltHedgehog,
                    CardId.ScrapIronSacredStatue,
                    CardId.JunkSignal,
                    CardId.AshBlossom,
                    CardId.EffectVeiler
                };
                var matches = cards.Where(c => preferred.Contains(c.Id))
                                   .OrderBy(c => preferred.IndexOf(c.Id))
                                   .ToList();
                if (matches.Count >= min)
                    return matches.Take(max).ToList();
            }

            // Rule 2: Destruction (502) or Banish (503/504) MUST target opponent cards (c.Controller == 1)
            if (hint == 502 || hint == 503 || hint == 504)
            {
                var enemyTargets = cards.Where(c => c.Controller == 1 && !IsTargetImmune(c)).ToList();
                if (enemyTargets.Count >= min)
                {
                    return enemyTargets.OrderByDescending(c => GetCardThreatScore(c)).Take(max).ToList();
                }
            }

            // Rule 3: Discard / Cost selection (HINTMSG_DISCARD = 501)
            if (hint == 501)
            {
                var discardOrder = new List<int>
                {
                    CardId.JetSynchron,
                    CardId.AnchorboltHedgehog,
                    CardId.JunkConverter,
                    CardId.WheelSynchron,
                    CardId.AssaultSynchron,
                    CardId.FullSpeedWarrior,
                    CardId.StarjunkSynchron,
                    CardId.CrossroadSonicChick
                };
                var fodderMatches = cards.Where(c => discardOrder.Contains(c.Id))
                                         .OrderBy(c => discardOrder.IndexOf(c.Id))
                                         .ToList();
                if (fodderMatches.Count >= min)
                    return fodderMatches.Take(max).ToList();

                // Avoid discarding Junk Synchron or established bosses
                var safeDiscards = cards.Where(c => c.Id != CardId.JunkSynchron && !IsMaterialBossProtected(c)).ToList();
                if (safeDiscards.Count >= min)
                    return safeDiscards.Take(max).ToList();
            }

            // Rule 4: Send to Graveyard (HINTMSG_TOGRAVE = 508)
            if (hint == 508)
            {
                var graveFodder = new List<int>
                {
                    CardId.AnchorboltHedgehog,
                    CardId.JetSynchron,
                    CardId.AssaultSynchron,
                    CardId.WheelSynchron,
                    CardId.JunkConverter
                };
                var matches = cards.Where(c => graveFodder.Contains(c.Id))
                                   .OrderBy(c => graveFodder.IndexOf(c.Id))
                                   .ToList();
                if (matches.Count >= min)
                    return matches.Take(max).ToList();
            }

            // Rule 5: Special Summon (HINTMSG_SPSUMMON = 509)
            if (hint == 509)
            {
                // Junk Speeder multi-tuner summon: pick 1 tuner per distinct level
                var tunerCandidates = cards.Where(c => c.IsTuner() && c.Name.Contains("Synchron")).ToList();
                if (tunerCandidates.Count >= min)
                {
                    var distinctByLevel = new List<ClientCard>();
                    var seenLevels = new HashSet<int>();
                    int[] preferredTuners = {
                        CardId.StardustSynchron,  // L4
                        CardId.JunkSynchron,      // L3
                        CardId.AssaultSynchron,   // L2
                        CardId.JetSynchron,       // L1
                        CardId.WheelSynchron      // L5
                    };
                    foreach (int pid in preferredTuners)
                    {
                        var match = tunerCandidates.FirstOrDefault(c => c.Id == pid && !seenLevels.Contains(c.Level));
                        if (match != null)
                        {
                            seenLevels.Add(match.Level);
                            distinctByLevel.Add(match);
                        }
                    }
                    foreach (var c in tunerCandidates)
                    {
                        if (!seenLevels.Contains(c.Level) && distinctByLevel.Count < max)
                        {
                            seenLevels.Add(c.Level);
                            distinctByLevel.Add(c);
                        }
                    }
                    if (distinctByLevel.Count >= min)
                        return distinctByLevel.Take(max).ToList();
                }

                // General revival priority
                var reviveOrder = new List<int>
                {
                    CardId.JunkConverter,
                    CardId.AssaultSynchron,
                    CardId.FormulaSynchron,
                    CardId.AnchorboltHedgehog,
                    CardId.JetSynchron,
                    CardId.JunkSynchron,
                    CardId.StardustSynchron
                };
                var reviveMatches = cards.Where(c => reviveOrder.Contains(c.Id))
                                         .OrderBy(c => reviveOrder.IndexOf(c.Id))
                                         .ToList();
                if (reviveMatches.Count >= min)
                    return reviveMatches.Take(max).ToList();
            }

            // Rule 6: Release / Tribute (HINTMSG_RELEASE = 500)
            if (hint == 500)
            {
                // Tribute small fodder first, NEVER tribute Boss monsters
                var tributeFodder = cards.Where(c => !IsMaterialBossProtected(c) && !c.HasType(CardType.Synchro))
                                         .OrderBy(c => c.Attack)
                                         .ToList();
                if (tributeFodder.Count >= min)
                    return tributeFodder.Take(max).ToList();
            }

            // Rule 7: Synchro Material Selection (HINTMSG_SMATERIAL = 512)
            if (hint == 512)
            {
                // Protect established boss monsters from being consumed
                var nonBossMaterials = cards.Where(c => !IsMaterialBossProtected(c)).ToList();
                if (nonBossMaterials.Count >= min)
                    return nonBossMaterials.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            // Junk Signal: Option 0 = Tribute SS, Option 1 = Negate opp response
            if (options.Count >= 2 && Duel.LastChainPlayer != 0)
            {
                return 1;
            }
            // Majestic Mirage: Option 0 = Special Summon Stardust, Option 1 = Banish opp monster
            if (options.Count >= 2)
            {
                return 0;
            }
            // Cosmic Blazar Dragon: Option 0 = Negate effect, Option 1 = Negate summon, Option 2 = Negate attack
            if (Duel.LastChainPlayer != 0) return 0;
            if (Duel.LastSummonPlayer != 0) return 1;
            if (Duel.Phase == DuelPhase.BattleStart) return 2;

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // High ATK bosses: always Attack
            if (cardId == CardId.CosmicBlazarDragon ||
                cardId == CardId.ShootingQuasarDragon ||
                cardId == CardId.ShootingMajesticStarDragon ||
                cardId == CardId.ShootingStarDragon ||
                cardId == CardId.StardustWarrior ||
                cardId == CardId.SatelliteWarrior ||
                cardId == CardId.StardustDragon ||
                cardId == CardId.AccelSynchroStardustDragon)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            // Low ATK / Utility monsters: always Defense
            if (cardId == CardId.FormulaSynchron ||
                cardId == CardId.JunkMeister ||
                cardId == CardId.CrossroadSonicChick ||
                cardId == CardId.JunkConverter ||
                cardId == CardId.JetSynchron ||
                cardId == CardId.WheelSynchron ||
                cardId == CardId.AnchorboltHedgehog ||
                cardId == CardId.CrimsonDragon ||
                cardId == CardId.CrimsonDragonQuetzacoatl)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }
}
