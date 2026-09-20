// ============================================================================
// CARD AUDIT — Anime_Yusei (Yusei Fudo's Ultimate Synchro Battlebox)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Stardust Synchron                  | Monster L4 T | Yes  | Yes   | Tribute | SS from hand/GY by tributing; search S/T      | In hand/GY, fodder available; on SS search    | Field full / no tribute fodder              |
// | Junk Meister                       | Monster L4   | Yes  | Yes   | None    | SS from hand if control Junk/Synchron; draw 1 | Control Junk/Synchron; Synchro material       | No Junk/Synchron on field                   |
// | Junk Synchron                      | Monster L3 T | No   | No    | None    | On NS: Revive Level 2 or lower from GY        | Normal Summoned with L<=2 target in GY        | GY has no L<=2 targets                      |
// | Starjunk Synchron                  | Monster L3 T | Yes  | Yes   | None    | SS from hand if control Synchron/Warrior      | Control Synchron or Warrior                  | No Synchron/Warrior on field                |
// | Full-Speed Warrior                 | Monster L2   | Yes  | Yes   | None    | SS from hand if control Warrior/Synchron      | Control Warrior or Synchron                  | No Warrior/Synchron on field                |
// | Anchorbolt Hedgehog                | Monster L2   | Yes  | Yes   | None    | GY Quick: SS if control Junk Warrior mention  | In GY, control Junk Warrior/mention          | Already SS this turn                        |
// | Assault Synchron                   | Monster L2 T | Yes  | Yes   | 700 LP  | SS from hand taking 700 damage                | Main Phase to extend Synchro climbing        | LP <= 700                                   |
// | Junk Converter                     | Monster L2   | Yes  | Yes   | Discard | Discard w/ Tuner -> Search Synchron; revive   | In hand with Tuner; sent as Synchro material | No Tuner in hand                            |
// | Jet Synchron                       | Monster L1 T | Yes  | Yes   | Discard | Search Junk on send; discard to revive from GY| Sent to GY; in GY needing L1 Tuner           | Hand empty for revival                      |
// | Scrap Synchron                     | Monster L1 T | Yes  | Yes   | None    | SS if control Scrap; pop card on Synchro send | Control Scrap; sent as Synchro material       | No Scrap on field                           |
// | Ash Blossom & Joyous Spring        | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent activates deck-interacting effect    | Bot's own turn without threats              |
// | Ghost Belle & Haunted Mansion      | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate GY move/banish/revive        | Opponent activates GY effect                  | Bot's own turn without threats              |
// | Ghost Mourner & Moonlit Chill      | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate face-up monster SS & burn    | Opponent Special Summons face-up monster      | Target already negated                      |
// | Ghost Ogre & Snow Rabbit           | Monster L3 T | Yes  | Yes   | Send    | Handtrap: Destroy card activating effect      | Opp monster or cont S/T activates on field    | Card cannot be destroyed                    |
// | Effect Veiler                      | Monster L1 T | Yes  | Yes   | Send    | Handtrap: Negate opp monster during opp MP    | Opponent Main Phase threat monster            | Already negated or not in opp MP            |
// | Stardust Dragon-Victim Sanctuary   | Monster L8   | Yes  | Yes   | Send    | Hand Quick: Negate opp effect targeting mon   | Opponent targets bot monster on field         | Opponent does not target                    |
// | Synchro Fellowship                 | Spell Normal | Yes  | Yes   | Discard | Add Junk Synchron + Junk/Stardust mention mon | Main Phase starter; GY banish extra NS        | Already used this turn                      |
// | Tuning                             | Spell Normal | No   | No    | Mill 1  | Add 1 "Synchron" Tuner from Deck to hand      | Main Phase early search starter               | Deck has no Synchron Tuners                 |
// | Junk Signal                        | Spell Quick  | Yes  | Yes   | Tribute | SS Stardust/Junk from Dk/GY OR negate chain   | Opp chains to Synchro OR tribute to cheat SS  | Already used this turn                      |
// | Scrap-Iron Sacred Statue           | Trap Normal  | Yes  | Yes   | Target  | Revive Dragon Synchro L7/8, resets itself     | GY has Dragon Synchro L7/8                    | GY empty                                    |
// | Majestic Mirage                    | Trap Cont    | Yes  | Yes   | None    | Protects Stardust / Stardust dragon effects   | Control Stardust Dragon / Synchro boss        | No Stardust monsters on field               |
// | Junk Speeder                       | Synchro L5   | Yes  | Yes   | None    | On SS: Summon multiple Synchron tuners from Dk| Synchro Summoned (primary combo engine)       | Monster zones full                          |
// | Accel Synchro Stardust Dragon      | Synchro L8   | Yes  | Yes   | Tribute | Revive L<=2 Tuner; Quick: SS Stardust & climb | Synchro Summoned; Main Phase Quick Synchro    | Opponent unaffected                         |
// | Shooting Quasar Dragon             | Synchro L12  | Yes  | Yes   | None    | 4000 ATK multi-attack, Omni-negate + destroy  | Opponent activates card/eff; battle push      | Already negated this turn                   |
// | Crimson Dragon                     | Synchro L12  | Yes  | Yes   | Tag-out | Search S/T; Quick: tag out into same Lvl Drag | Target L7+ Dragon on field to cheat boss      | No L7+ Dragon on field                      |
// | Stardust Warrior                   | Synchro L10  | Yes  | Yes   | Tribute | Quick: Negate opp SS & destroy, recurses EP   | Opponent Special Summons monster              | Already used this turn                      |
// | Satellite Warrior                  | Synchro L10  | Yes  | Yes   | None    | Pop opp cards up to Synchros in GY, +1000 ATK | Board breaking / going second OTK             | Opponent has no cards                       |
// | Stardust Dragon                    | Synchro L8   | Yes  | Yes   | Tribute | Negate card/effect that destroys on field     | Opponent effect would destroy card(s)         | No destruction effect                       |
// | Stardust Charge Warrior            | Synchro L6   | No   | No    | None    | Draw 1 card on Synchro Summon; attack all mon | Synchro Summoned (draw engine)                | Deck empty                                  |
// | Formula Synchron                   | Synchro L2 T | No   | No    | None    | Draw 1 card on Synchro Summon; Quick Synchro  | Synchro Summoned; Opp turn Quick Synchro      | Deck empty                                  |
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
            public const int MajesticStarDragon = 7841112;
        }

        public Anime_YuseiExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterExecutors();
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High-Priority Handtraps & Interruptions (Enemy / Chain)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.ShootingQuasarDragon, ShootingQuasarNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.StardustWarrior, StardustWarriorNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.StardustDragonVictimSanctuary, VictimSanctuaryActivate);
            AddExecutor(ExecutorType.Activate, CardId.StardustDragon, StardustDragonNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.JunkSignal, JunkSignalActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostMourner, GhostMournerActivate);
            AddExecutor(ExecutorType.Activate, CardId.GhostOgre, GhostOgreActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerActivate);

            // -------------------------------------------------------------
            // 2. Main Phase 1 Starters & Searches
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.SynchroFellowship, SynchroFellowshipActivate);
            AddExecutor(ExecutorType.Activate, CardId.Tuning, TuningActivate);
            AddExecutor(ExecutorType.Activate, CardId.JunkConverter, JunkConverterActivate);

            // -------------------------------------------------------------
            // 3. Main Phase Monster Special Summons (Extenders)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.StardustSynchron, StardustSynchronEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.JunkMeister, JunkMeisterSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.StarjunkSynchron, StarjunkSynchronSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FullSpeedWarrior, FullSpeedWarriorSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AssaultSynchron, AssaultSynchronSummon);
            AddExecutor(ExecutorType.Activate, CardId.AnchorboltHedgehog, AnchorboltHedgehogEffect);
            AddExecutor(ExecutorType.Activate, CardId.JetSynchron, JetSynchronEffect);

            // -------------------------------------------------------------
            // 4. Normal Summons
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Summon, CardId.JunkSynchron, JunkSynchronSummon);
            AddExecutor(ExecutorType.Activate, CardId.JunkSynchron, JunkSynchronEffect);
            AddExecutor(ExecutorType.Summon, CardId.StardustSynchron, StardustSynchronSummon);
            AddExecutor(ExecutorType.Summon, CardId.JunkConverter, JunkConverterSummon);
            AddExecutor(ExecutorType.Summon, CardId.AssaultSynchron, AssaultSynchronNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.JetSynchron, JetSynchronNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.ScrapSynchron, ScrapSynchronNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.StarjunkSynchron, StarjunkSynchronNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.JunkMeister, JunkMeisterNormalSummon);

            // -------------------------------------------------------------
            // 5. Extra Deck Synchro Summons (Hierarchy & Laddering)
            // -------------------------------------------------------------
            // Step 1: Junk Speeder (L5 Engine)
            AddExecutor(ExecutorType.SpSummon, CardId.JunkSpeeder, JunkSpeederSummon);
            AddExecutor(ExecutorType.Activate, CardId.JunkSpeeder, JunkSpeederEffect);

            // Step 2: Intermediate Laddering (L2 / L3 / L6)
            AddExecutor(ExecutorType.SpSummon, CardId.FormulaSynchron, FormulaSynchronSummon);
            AddExecutor(ExecutorType.Activate, CardId.FormulaSynchron, FormulaSynchronEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.StardustChargeWarrior, StardustChargeWarriorSummon);
            AddExecutor(ExecutorType.Activate, CardId.StardustChargeWarrior, StardustChargeWarriorEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ScrapWarrior, ScrapWarriorSummon);

            // Step 3: Level 8 Synchros (Accel Stardust & Stardust)
            AddExecutor(ExecutorType.SpSummon, CardId.AccelSynchroStardustDragon, AccelStardustSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccelSynchroStardustDragon, AccelStardustEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.StardustDragon, StardustDragonSummon);

            // Step 4: Level 10 Bosses (Satellite Warrior / Stardust Warrior)
            AddExecutor(ExecutorType.SpSummon, CardId.SatelliteWarrior, SatelliteWarriorSummon);
            AddExecutor(ExecutorType.Activate, CardId.SatelliteWarrior, SatelliteWarriorEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.StardustWarrior, StardustWarriorSummon);

            // Step 5: Level 12 Ultimate Bosses (Shooting Quasar Dragon / Crimson Dragon)
            AddExecutor(ExecutorType.SpSummon, CardId.ShootingQuasarDragon, ShootingQuasarSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrimsonDragon, CrimsonDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.CrimsonDragon, CrimsonDragonEffect);
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
        // EXECUTION IMPLEMENTATIONS
        // =================================================================

        private bool ShootingQuasarNegateActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            return Duel.LastChainPlayer != 0;
        }

        private bool StardustWarriorNegateActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            // Negate opponent Special Summon
            return Duel.LastSummonPlayer != 0;
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
                // Tribute monster to SS Stardust or Junk Warrior from deck/GY
                if (Bot.GetMonsters().Any(m => m.Level <= 2))
                {
                    AI.SelectOption(0);
                    AI.SelectCard(new[] {
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
            return true;
        }

        private bool GhostBelleActivate()
        {
            if (Duel.LastChainPlayer == 0) return false;
            return true;
        }

        private bool GhostMournerActivate()
        {
            if (Duel.LastChainPlayer == 0) return false;
            ClientCard oppMonster = Enemy.GetMonsters().LastOrDefault(m => m.IsFaceup() && !m.IsDisabled());
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
            return true;
        }

        private bool EffectVeilerActivate()
        {
            if (Duel.LastChainPlayer == 0) return false;
            if (Duel.Player != 1) return false; // Opponent's turn
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && (m.Attack >= 2000 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true;
        }

        private bool SynchroFellowshipActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Add Junk Synchron + 1 Stardust/Junk mention (Stardust Synchron or Junk Meister), then discard 1
                AI.SelectCard(new[] {
                    CardId.JunkSynchron,
                    CardId.StardustSynchron,
                    CardId.JunkMeister
                });
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Banish from GY to reduce Synchro level by 1 and gain extra Normal Summon
                ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.HasType(CardType.Synchro) && m.Level >= 5);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool TuningActivate()
        {
            // Search Synchron Tuner (Junk Synchron > Stardust Synchron > Assault Synchron)
            AI.SelectCard(new[] {
                CardId.JunkSynchron,
                CardId.StardustSynchron,
                CardId.AssaultSynchron,
                CardId.JetSynchron
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
                    CardId.AssaultSynchron
                });
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // On sent as material: revive Tuner
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

        private bool StardustSynchronEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                // Tribute monster to SS itself
                ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m.Level <= 2 && !m.HasType(CardType.Synchro));
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
            // SS from hand if controlling Junk or Synchron
            return Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Name.Contains("Junk") || m.Name.Contains("Synchron")));
        }

        private bool StarjunkSynchronSummon()
        {
            // SS from hand if controlling Synchron or Warrior
            return Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Name.Contains("Synchron") || m.HasRace(CardRace.Warrior)));
        }

        private bool FullSpeedWarriorSummon()
        {
            // SS from hand if controlling Warrior or Synchron
            return Bot.GetMonsters().Any(m => m.IsFaceup() && (m.HasRace(CardRace.Warrior) || m.Name.Contains("Synchron")));
        }

        private bool AssaultSynchronSummon()
        {
            // SS taking 700 damage
            return Bot.LifePoints > 700;
        }

        private bool AnchorboltHedgehogEffect()
        {
            // Revive from GY if controlling monster mentioning Junk Warrior
            return Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Name.Contains("Junk") || m.Name.Contains("Synchron")));
        }

        private bool JetSynchronEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Discard 1 to revive from GY
                if (Bot.Hand.Count > 0 && !Bot.GetMonsters().Any(m => m.Id == CardId.JetSynchron))
                {
                    return true;
                }
            }
            return false;
        }

        private bool JunkSynchronSummon()
        {
            return true;
        }

        private bool JunkSynchronEffect()
        {
            // On NS: revive L<=2 monster from GY (Converter, Hedgehog, Assault Synchron, Jet Synchron)
            AI.SelectCard(new[] {
                CardId.JunkConverter,
                CardId.AnchorboltHedgehog,
                CardId.AssaultSynchron,
                CardId.FullSpeedWarrior,
                CardId.JetSynchron
            });
            return true;
        }

        private bool StardustSynchronSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool JunkConverterSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool AssaultSynchronNormalSummon()
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

        private bool StarjunkSynchronNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool JunkMeisterNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool JunkSpeederSummon()
        {
            return true;
        }

        private bool JunkSpeederEffect()
        {
            // Summon as many Synchron tuners with different levels from Deck!
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
            return true;
        }

        private bool FormulaSynchronEffect()
        {
            return true;
        }

        private bool StardustChargeWarriorSummon()
        {
            return true;
        }

        private bool StardustChargeWarriorEffect()
        {
            return true;
        }

        private bool ScrapWarriorSummon()
        {
            return true;
        }

        private bool AccelStardustSummon()
        {
            return true;
        }

        private bool AccelStardustEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On summon: revive L<=2 tuner from GY
                AI.SelectCard(new[] {
                    CardId.AssaultSynchron,
                    CardId.JetSynchron,
                    CardId.FormulaSynchron
                });
                return true;
            }
            return false;
        }

        private bool StardustDragonSummon()
        {
            return true;
        }

        private bool SatelliteWarriorSummon()
        {
            // Primary board breaker when going second or opponent has cards
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0;
        }

        private bool SatelliteWarriorEffect()
        {
            // Destroy opponent cards up to Synchros in GY
            List<ClientCard> targets = Enemy.GetMonsters().OrderByDescending(m => m.Attack).Concat(Enemy.GetSpells()).ToList();
            if (targets.Count > 0)
            {
                AI.SelectCard(targets);
            }
            return true;
        }

        private bool StardustWarriorSummon()
        {
            return true;
        }

        private bool ShootingQuasarSummon()
        {
            // Ultimate Omni-Negate & multi-attack boss
            return true;
        }

        private bool CrimsonDragonSummon()
        {
            return true;
        }

        private bool CrimsonDragonEffect()
        {
            // Tag out into same level Dragon Synchro (e.g. Quasar or Stardust Dragon)
            ClientCard dragonTarget = Bot.GetMonsters().FirstOrDefault(m => m.HasType(CardType.Synchro) && m.HasRace(CardRace.Dragon) && m.Id != CardId.CrimsonDragon);
            if (dragonTarget != null)
            {
                AI.SelectCard(dragonTarget);
                return true;
            }
            return false;
        }

        private bool QuetzacoatlSummon()
        {
            return true;
        }

        private bool QuetzacoatlEffect()
        {
            // Add Dragon monster on summon or cancel attack
            return true;
        }

        private bool MajesticMirageActivate()
        {
            return Bot.GetMonsters().Any(m => m.HasType(CardType.Synchro) && m.Name.Contains("Stardust"));
        }

        private bool ScrapIronStatueActivate()
        {
            // Revive L7/8 Dragon Synchro from GY/banish
            ClientCard target = Bot.Graveyard.FirstOrDefault(c => c.HasType(CardType.Synchro) && c.HasRace(CardRace.Dragon) && (c.Level == 7 || c.Level == 8));
            if (target != null)
            {
                AI.SelectCard(target);
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
            if (Card.Attack < 1500 && Card.IsAttack())
            {
                return true;
            }
            if (Card.Attack >= 2000 && Card.IsDefense())
            {
                return true;
            }
            return false;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
