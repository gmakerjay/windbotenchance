// ============================================================================
// CARD AUDIT — Anime_JackAtlas (Jack Atlas's Ultimate Resonator Battlebox)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Nibiru, the Primal Being           | Monster L11  | Yes  | Yes   | Tribute | Tribute all monsters on field, SS + Token     | Opponent 5+ summons and has high threat      | Bot has established unbreakable boss board   |
// | The Bystial Lubellion              | Monster L8   | Yes  | Yes   | Discard | Discard to search Bystial; tribute L6+ to SS  | In hand, search Druiswurm/Baldrake; tribute   | No Bystial targets in deck                  |
// | Fidraulis Harmonia                 | Monster L4 T | Yes  | Yes   | None    | Reveal Tuners in deck; SS from Deck           | Hand Quick effect or SS extender             | Already used this turn                      |
// | Bystial Baldrake                   | Monster L6   | Yes  | Yes   | Banish  | Quick: banish LIGHT/DARK from GY, SS; banish  | Opp controls monster or in GY to interrupt   | No valid GY targets                         |
// | Bystial Druiswurm                  | Monster L6   | Yes  | Yes   | Banish  | Quick: banish LIGHT/DARK from GY, SS; pop SS  | Opp controls monster or in GY to interrupt   | No valid GY targets                         |
// | Fiend Piece Golem                  | Monster L4   | Yes  | Yes   | None    | SS if control Fiend Tuner; banish S/T on SS   | Control Fiend Tuner; opp S/T to banish        | No Fiend Tuner on field                     |
// | Power Vice Dragon                  | Monster L5   | Yes  | Yes   | None    | SS if control Dragon; search L5 or lower Drag | Control Dragon; search Dragon extender        | No Dragon on field                          |
// | Bone Archfiend                     | Monster L4   | Yes  | Yes   | Send    | SS from hand/GY by sending 1 card; dump Tuner | In hand/GY; modify level and dump Resonator   | Hand empty and field empty                  |
// | Wandering King Wildwind            | Monster L4   | Yes  | Yes   | None    | SS if control Fiend Tuner <= 1500 ATK; GY srch| Control Soul/Crimson Resonator; GY search     | No Fiend Tuner on field                     |
// | Red Lotus King, Flame Crime        | Monster L3   | Yes  | Yes   | None    | Quick: SS if control Fiend Tuner; dump Trap   | Main Phase, control Fiend Tuner; dump RedReign| Already used this turn                      |
// | Darkness Resonator                 | Monster L3 T | Yes  | Yes   | None    | SS if control Resonator or DARK Dragon Synchro| Control Resonator or RDA Synchro              | No valid monsters on field                  |
// | Soul Resonator                     | Monster L3 T | Yes  | Yes   | None    | On NS/SS: Search L<=4 Fiend (Bone Archfiend)  | Normal/Special Summoned (primary starter)     | Already used this turn                      |
// | Crimson Resonator                  | Monster L2 T | Yes  | Yes   | None    | SS if field empty; SS 2 Resonators from Deck  | Field empty OR control exactly 1 DARK Drag Syn| More than 1 other monster on field          |
// | Vision Resonator                   | Monster L2 T | Yes  | Yes   | None    | SS if control L5+ DARK; search Gaia on GY send| Control L5+ DARK; sent to GY to search Gaia   | Already SS this turn                        |
// | Synkron Resonator                  | Monster L1 T | Yes  | Yes   | None    | SS if control Synchro; recycle Resonator on GY| Control Synchro monster; sent to GY to recycle| Already SS this turn                        |
// | Crimson Call                       | Spell Quick  | Yes  | Yes   | None    | Add L4 or lower Fiend from GY (or Deck if RDA)| Need Bone Archfiend or Soul Resonator         | No targets in GY/Deck                       |
// | Resonator Call                     | Spell Normal | No   | No    | None    | Add 1 "Resonator" monster from Deck to hand   | Main Phase search starter (Soul Resonator)    | Deck has no Resonators                      |
// | Crimson Gaia                       | Spell Cont   | Yes  | Yes   | None    | Search RDA / S/T mention; book opp on attack  | Main Phase search Red Zone/Red Reign/Soul Res | Already used this turn                      |
// | Dominus Impulse                    | Trap Normal  | Yes  | Yes   | None    | Handtrap: Negate opp effect that SS monster   | Opponent activates effect that Special Summons| Bot controls cards and cannot activate hand |
// | Red Reign                          | Trap Normal  | Yes  | Yes   | None    | Banish all monsters except highest level Synch| Control L8+ Synchro and opp controls monsters | Opponent has 0 monsters on field            |
// | Red Dragon Archfiend's Chain       | Trap Normal  | Yes  | Yes   | Reveal  | Reduce opp monsters ATK and negate            | Opponent activates effects / attacks          | No RDA to reveal                            |
// | Red Zone                           | Trap Cont    | Yes  | Yes   | None    | Pop 1 card when RDA eff triggers; revive banis| Opp activates card/eff while control RDA; rev | Opponent has 0 cards on field               |
// | The Ruler's Rumbling               | Trap Normal  | Yes  | Yes   | None    | Backrow and attack disruption                 | Opponent attacking or activating cards        | Opponent controls no cards                  |
// | Red Rising Dragon                  | Synchro L6   | Yes  | Yes   | None    | On Synchro: revive Resonator; GY banish SS 2  | Synchro Summoned (primary climbing ladder)    | No Resonator in GY                          |
// | Scarred Dragon Archfiend           | Synchro L8   | Yes  | Yes   | None    | Treated as RDA; on GY send: SS RDA & wipe atk | Level 8 Synchro stepping stone into Abyss/Bane| Extra Deck has no RDA                       |
// | Red Dragon Archfiend               | Synchro L8   | No   | No    | None    | 3000 ATK Ace; destroy all opp DEF monsters    | Level 8 boss / attacker / stepping stone      | Alone on field with friendly non-attackers  |
// | Hot Red Dragon Archfiend Abyss     | Synchro L9   | Yes  | Yes   | Target  | Quick: Negate 1 face-up opp card; revive Tuner| Opponent activates card/eff or on face-up thrt| Target already disabled                     |
// | Hot Red Dragon Archfiend Bane      | Synchro L10  | Yes  | Yes   | Tribute | Tribute monster to revive RDA; SS 2 Tuners    | Have fodder to tribute and RDA in GY          | No RDA in GY                                |
// | Red Supernova Dragon               | Synchro L12  | Yes  | Yes   | Banish  | 4000+ ATK, indestructible; Quick: banish all  | Opponent activates monster eff or declares atk| Opponent controls 0 cards                   |
// | Red Nova Dragon - Burning Soul     | Synchro L12  | No   | No    | None    | 4500+ ATK Double-Tuner boss; indestructible   | Level 12 boss push                            | Opponent unaffected                         |
// | Hot Red Dragon Archfiend King Calam| Synchro L12  | Yes  | Yes   | None    | On Synchro: Opponent cannot activate cards/eff| Level 12 Synchro climb to lock opponent turn  | Already summoned this turn                  |
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
    [Deck("Anime_JackAtlas", "Anime_JackAtlas")]
    public class Anime_JackAtlasExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int Nibiru = 27204311;
            public const int TheBystialLubellion = 32731036;
            public const int FidraulisHarmonia = 70088809;
            public const int BystialBaldrake = 72656408;
            public const int BystialDruiswurm = 6637331;
            public const int FiendPieceGolem = 56838842;
            public const int PowerViceDragon = 19434243;
            public const int BoneArchfiend = 25784595;
            public const int WanderingKingWildwind = 52589809;
            public const int RedLotusKingFlameCrime = 19299793;
            public const int DarknessResonator = 83445539;
            public const int SoulResonator = 62991792;
            public const int CrimsonResonator = 34761841;
            public const int VisionResonator = 98396890;
            public const int SynkronResonator = 77360173;

            // Spells
            public const int CrimsonCall = 99398682;
            public const int ResonatorCall = 23008320;
            public const int CrimsonGaia = 98173209;

            // Traps
            public const int DominusImpulse = 40366667;
            public const int RedReign = 5376159;
            public const int RedDragonArchfiendsChain = 92936364;
            public const int RedZone = 50056656;
            public const int TheRulersRumbling = 17269895;

            // Extra Deck
            public const int RedHypernovaDragon = 30698243;
            public const int BystialDisPater = 27572350;
            public const int RedSupernovaDragon = 99585850;
            public const int RedNovaDragonBurningSoul = 65541655;
            public const int HotRedDragonArchfiendBane = 36857073;
            public const int HotRedDragonArchfiendAbyss = 9753964;
            public const int RedDragonArchfiend = 70902743;
            public const int ScarredDragonArchfiend = 87451661;
            public const int TheCrimsonKing = 67809530;
            public const int KuibeltTheBladeDragon = 87837090;
            public const int CrimsonBladeDragon = 3294539;
            public const int RedRisingDragon = 66141736;
        }

        public Anime_JackAtlasExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterExecutors();
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High-Priority Handtraps & Quick Disruption (Enemy / Chain)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.HotRedDragonArchfiendAbyss, HotRedAbyssNegateActivate);
            AddExecutor(ExecutorType.Activate, CardId.RedSupernovaDragon, RedSupernovaBanishActivate);
            AddExecutor(ExecutorType.Activate, CardId.RedZone, RedZoneActivate);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseActivate);
            AddExecutor(ExecutorType.Activate, CardId.RedReign, RedReignActivate);
            AddExecutor(ExecutorType.Activate, CardId.RedDragonArchfiendsChain, RedArchfiendChainActivate);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, NibiruActivate);
            AddExecutor(ExecutorType.Activate, CardId.BystialDruiswurm, BystialDruiswurmActivate);
            AddExecutor(ExecutorType.Activate, CardId.BystialBaldrake, BystialBaldrakeActivate);
            AddExecutor(ExecutorType.Activate, CardId.TheBystialLubellion, LubellionActivate);

            // -------------------------------------------------------------
            // 2. Main Phase 1 Starters & Searches
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.ResonatorCall, ResonatorCallActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrimsonGaia, CrimsonGaiaActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrimsonCall, CrimsonCallActivate);

            // -------------------------------------------------------------
            // 3. Main Phase Monster Special Summons (Extenders)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpSummon, CardId.CrimsonResonator, CrimsonResonatorSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.BoneArchfiend, BoneArchfiendEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BoneArchfiend, BoneArchfiendSpecialSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.WanderingKingWildwind, WildwindSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.WanderingKingWildwind, WildwindGraveEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.VisionResonator, VisionResonatorSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.VisionResonator, VisionResonatorGraveEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SynkronResonator, SynkronResonatorSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.SynkronResonator, SynkronResonatorGraveEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DarknessResonator, DarknessResonatorSpecialSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.PowerViceDragon, PowerViceDragonSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.PowerViceDragon, PowerViceDragonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendPieceGolem, FiendPieceGolemSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.FiendPieceGolem, FiendPieceGolemEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.RedLotusKingFlameCrime, FlameCrimeSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.RedLotusKingFlameCrime, FlameCrimeEffect);
            AddExecutor(ExecutorType.Activate, CardId.FidraulisHarmonia, HarmoniaEffect);

            // -------------------------------------------------------------
            // 4. Normal Summons
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Summon, CardId.SoulResonator, SoulResonatorSummon);
            AddExecutor(ExecutorType.Activate, CardId.SoulResonator, SoulResonatorEffect);
            AddExecutor(ExecutorType.Summon, CardId.CrimsonResonator, CrimsonResonatorNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.BoneArchfiend, BoneArchfiendNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.VisionResonator, VisionResonatorNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.SynkronResonator, SynkronResonatorNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.DarknessResonator, DarknessResonatorNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.WanderingKingWildwind, WildwindNormalSummon);
            AddExecutor(ExecutorType.Summon, CardId.RedLotusKingFlameCrime, FlameCrimeNormalSummon);

            // -------------------------------------------------------------
            // 5. Extra Deck Synchro Climbing
            // -------------------------------------------------------------
            // Step 1: Red Rising Dragon (Level 6 Engine)
            AddExecutor(ExecutorType.SpSummon, CardId.RedRisingDragon, RedRisingDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.RedRisingDragon, RedRisingDragonEffect);

            // Step 2: Crimson Resonator Multiple Deck Summon
            AddExecutor(ExecutorType.Activate, CardId.CrimsonResonator, CrimsonResonatorDeckSummon);

            // Step 3: Level 7 Intermediate Synchros
            AddExecutor(ExecutorType.SpSummon, CardId.KuibeltTheBladeDragon, KuibeltSummon);
            AddExecutor(ExecutorType.Activate, CardId.KuibeltTheBladeDragon, KuibeltEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.CrimsonBladeDragon, CrimsonBladeSummon);

            // Step 4: Level 8 Dragon Synchros (Scarred / RDA / Crimson King)
            AddExecutor(ExecutorType.SpSummon, CardId.ScarredDragonArchfiend, ScarredDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.ScarredDragonArchfiend, ScarredDragonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.RedDragonArchfiend, RedDragonArchfiendSummon);
            AddExecutor(ExecutorType.Activate, CardId.RedDragonArchfiend, RedDragonArchfiendEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.TheCrimsonKing, TheCrimsonKingSummon);

            // Step 5: Level 9 Omni-Negate (Hot Red Dragon Archfiend Abyss)
            AddExecutor(ExecutorType.SpSummon, CardId.HotRedDragonArchfiendAbyss, HotRedAbyssSummon);

            // Step 6: Level 10 Revival & Recursion (Hot Red Dragon Archfiend Bane)
            AddExecutor(ExecutorType.SpSummon, CardId.HotRedDragonArchfiendBane, HotRedBaneSummon);
            AddExecutor(ExecutorType.Activate, CardId.HotRedDragonArchfiendBane, HotRedBaneEffect);

            // Step 7: Level 12 Ultimate Bosses (Red Supernova / Red Nova / Calamity / Hypernova)
            AddExecutor(ExecutorType.SpSummon, CardId.RedSupernovaDragon, RedSupernovaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BystialDisPater, DisPaterSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialDisPater, DisPaterEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.RedNovaDragonBurningSoul, RedNovaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RedHypernovaDragon, RedHypernovaSummon);

            // -------------------------------------------------------------
            // 6. Backrow Support & Fallbacks
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpellSet, SpellSetStrategy);
            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // =================================================================
        // EXECUTION LOGIC & METHODS
        // =================================================================

        private bool HotRedAbyssNegateActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            // Quick effect: Target 1 face-up card opponent controls to negate
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && (m.Attack >= 2000 || AntiFloodgateHelper.NegateMonsterIds.Contains(m.Id)));
            if (target == null)
            {
                target = Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup() && !s.IsDisabled());
            }
            if (target == null && Enemy.GetMonsterCount() > 0)
            {
                target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled());
            }
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            // Trigger effect on battle damage: revive Tuner in defense
            if (Duel.Phase == DuelPhase.Damage || Duel.Phase == DuelPhase.DamageCal)
            {
                AI.SelectCard(new[] {
                    CardId.VisionResonator,
                    CardId.SoulResonator,
                    CardId.SynkronResonator,
                    CardId.CrimsonResonator
                });
                return true;
            }
            return false;
        }

        private bool RedSupernovaBanishActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            // Banish all opponent cards if opponent activates a monster effect or attacks
            int oppCards = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            if (oppCards > 0 && (Duel.LastChainPlayer != 0 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep))
            {
                return true;
            }
            return false;
        }

        private bool RedZoneActivate()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Pop 1 card on field when RDA activates effect
                if (Duel.LastChainPlayer == 0)
                {
                    ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                    if (target == null)
                    {
                        target = Enemy.GetSpells().FirstOrDefault();
                    }
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
                // Revive banished DARK Dragon Synchro
                ClientCard banished = Bot.Banished.FirstOrDefault(c => c.HasType(CardType.Synchro) && c.HasRace(CardRace.Dragon));
                if (banished != null)
                {
                    AI.SelectCard(banished);
                    return true;
                }
            }
            return false;
        }

        private bool DominusImpulseActivate()
        {
            if (Duel.LastChainPlayer == 0) return false;
            return true;
        }

        private bool RedReignActivate()
        {
            // Banish all monsters except the highest level Synchro
            if (Bot.GetMonsters().Any(m => m.HasType(CardType.Synchro) && m.Level >= 8))
            {
                if (Enemy.GetMonsterCount() >= 1)
                {
                    return true;
                }
            }
            // GY recursion effect: if DARK Dragon Synchro is Synchro Summoned, add back to hand
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            return false;
        }

        private bool RedArchfiendChainActivate()
        {
            if (Duel.LastChainPlayer == 0) return false;
            ClientCard oppMonster = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled());
            if (oppMonster != null)
            {
                AI.SelectCard(oppMonster);
                return true;
            }
            return false;
        }

        private bool NibiruActivate()
        {
            AI.SelectPosition(CardPosition.FaceUpDefence);
            return DefaultNibiru();
        }

        private bool BystialDruiswurmActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Banish LIGHT or DARK from either GY to SS
                ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark));
                if (target == null && Bot.Graveyard.Count(c => c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)) >= 2)
                {
                    target = Bot.Graveyard.FirstOrDefault(c => (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)) && !c.HasType(CardType.Synchro));
                }
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            if (Card.Location == CardLocation.Grave)
            {
                // On sent from field to GY: send 1 opp Special Summoned monster to GY
                ClientCard oppTarget = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (oppTarget != null)
                {
                    AI.SelectCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool BystialBaldrakeActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark));
                if (target == null && Bot.Graveyard.Count(c => c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)) >= 2)
                {
                    target = Bot.Graveyard.FirstOrDefault(c => (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)) && !c.HasType(CardType.Synchro));
                }
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            if (Card.Location == CardLocation.MonsterZone && Duel.LastSummonPlayer != 0)
            {
                // Tribute other LIGHT/DARK to banish opp SS monster
                ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m != Card && (m.HasAttribute(CardAttribute.Light) || m.HasAttribute(CardAttribute.Dark)) && !m.HasType(CardType.Synchro));
                if (tribute != null)
                {
                    AI.SelectCard(tribute);
                    return true;
                }
            }
            return false;
        }

        private bool LubellionActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Discard to search Bystial Druiswurm or Baldrake
                AI.SelectCard(new[] {
                    CardId.BystialDruiswurm,
                    CardId.BystialBaldrake
                });
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Tribute Level 6+ DARK Dragon to SS
                ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m.HasRace(CardRace.Dragon) && m.HasAttribute(CardAttribute.Dark) && m.Level >= 6 && !m.HasType(CardType.Synchro));
                if (tribute != null)
                {
                    AI.SelectCard(tribute);
                    return true;
                }
            }
            return false;
        }

        private bool ResonatorCallActivate()
        {
            // Search Soul Resonator (primary starter) -> Vision Resonator -> Crimson Resonator -> Synkron Resonator
            AI.SelectCard(new[] {
                CardId.SoulResonator,
                CardId.VisionResonator,
                CardId.CrimsonResonator,
                CardId.SynkronResonator,
                CardId.DarknessResonator
            });
            return true;
        }

        private bool CrimsonGaiaActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }
            if (Card.Location == CardLocation.SpellZone)
            {
                // Search Red Zone > Red Reign > Soul Resonator > Bone Archfiend > Vision Resonator
                AI.SelectCard(new[] {
                    CardId.RedZone,
                    CardId.RedReign,
                    CardId.SoulResonator,
                    CardId.BoneArchfiend,
                    CardId.VisionResonator
                });
                return true;
            }
            return false;
        }

        private bool CrimsonCallActivate()
        {
            // Add Level 4 or lower Fiend from GY or Deck
            AI.SelectCard(new[] {
                CardId.BoneArchfiend,
                CardId.SoulResonator,
                CardId.VisionResonator,
                CardId.CrimsonResonator
            });
            return true;
        }

        private bool CrimsonResonatorSpecialSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool BoneArchfiendSpecialSummon()
        {
            // Discard 1 other card from hand or field to SS Bone Archfiend
            return Bot.Hand.Count >= 2;
        }

        private bool BoneArchfiendEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Dump Fiend Tuner (Vision Resonator / Crimson Resonator) from Deck to adjust Level
                AI.SelectCard(new[] {
                    CardId.VisionResonator,
                    CardId.CrimsonResonator,
                    CardId.SynkronResonator
                });
                return true;
            }
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Hand)
            {
                // Send other card from hand/field to SS
                ClientCard discardTarget = Bot.Hand.FirstOrDefault(c => c != Card && c.Id != CardId.SoulResonator);
                if (discardTarget != null)
                {
                    AI.SelectCard(discardTarget);
                    return true;
                }
            }
            return false;
        }

        private bool WildwindSpecialSummon()
        {
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasRace(CardRace.Fiend) && m.HasType(CardType.Tuner) && m.Attack <= 1500);
        }

        private bool WildwindGraveEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(new[] {
                    CardId.SoulResonator,
                    CardId.CrimsonResonator,
                    CardId.SynkronResonator
                });
                return true;
            }
            return false;
        }

        private bool VisionResonatorSpecialSummon()
        {
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasAttribute(CardAttribute.Dark) && m.Level >= 5);
        }

        private bool VisionResonatorGraveEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Search Crimson Gaia > Red Zone > Red Reign
                AI.SelectCard(new[] {
                    CardId.CrimsonGaia,
                    CardId.RedZone,
                    CardId.RedReign
                });
                return true;
            }
            return false;
        }

        private bool SynkronResonatorSpecialSummon()
        {
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasType(CardType.Synchro));
        }

        private bool SynkronResonatorGraveEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Recycle Resonator from GY to hand
                AI.SelectCard(new[] {
                    CardId.SoulResonator,
                    CardId.VisionResonator,
                    CardId.CrimsonResonator
                });
                return true;
            }
            return false;
        }

        private bool DarknessResonatorSpecialSummon()
        {
            return Bot.GetMonsters().Any(m => m.IsFaceup() && (m.Name.Contains("Resonator") || (m.HasType(CardType.Synchro) && m.HasRace(CardRace.Dragon) && m.HasAttribute(CardAttribute.Dark))));
        }

        private bool PowerViceDragonSpecialSummon()
        {
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasRace(CardRace.Dragon));
        }

        private bool PowerViceDragonEffect()
        {
            AI.SelectCard(new[] {
                CardId.BoneArchfiend,
                CardId.SoulResonator
            });
            return true;
        }

        private bool FiendPieceGolemSpecialSummon()
        {
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasRace(CardRace.Fiend) && m.HasType(CardType.Tuner));
        }

        private bool FiendPieceGolemEffect()
        {
            // Banish opponent Spell/Trap
            ClientCard oppSpell = Enemy.GetSpells().FirstOrDefault();
            if (oppSpell != null)
            {
                AI.SelectCard(oppSpell);
                return true;
            }
            return false;
        }

        private bool FlameCrimeSpecialSummon()
        {
            return Bot.GetMonsters().Any(m => m.IsFaceup() && m.HasRace(CardRace.Fiend) && m.HasType(CardType.Tuner)) || Enemy.GetMonsters().Any(m => m.IsSpecialSummoned);
        }

        private bool FlameCrimeEffect()
        {
            // Dump Red Reign to GY
            AI.SelectCard(new[] {
                CardId.RedReign,
                CardId.RedZone
            });
            return true;
        }

        private bool HarmoniaEffect()
        {
            if (Duel.LastChainPlayer == 0) return false;
            return Bot.ExtraDeck.Any(c => c.HasType(CardType.Synchro));
        }

        private bool SoulResonatorSummon()
        {
            return true;
        }

        private bool SoulResonatorEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Add Bone Archfiend (priority 1 starter) -> Wildwind -> Flame Crime
                AI.SelectCard(new[] {
                    CardId.BoneArchfiend,
                    CardId.WanderingKingWildwind,
                    CardId.RedLotusKingFlameCrime,
                    CardId.FiendPieceGolem
                });
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // GY destruction substitute
                return true;
            }
            return false;
        }

        private bool CrimsonResonatorNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool BoneArchfiendNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool VisionResonatorNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool SynkronResonatorNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool DarknessResonatorNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool WildwindNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool FlameCrimeNormalSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool RedRisingDragonSummon()
        {
            return true;
        }

        private bool RedRisingDragonEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Revive Resonator from GY: Crimson Resonator (for deck summon) > Soul Resonator > Vision Resonator
                AI.SelectCard(new[] {
                    CardId.CrimsonResonator,
                    CardId.SoulResonator,
                    CardId.VisionResonator,
                    CardId.SynkronResonator,
                    CardId.DarknessResonator
                });
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Banish to revive 2 Level 1 Resonators
                return Bot.Graveyard.Count(c => c.Id == CardId.SynkronResonator) >= 2;
            }
            return false;
        }

        private bool CrimsonResonatorDeckSummon()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // SS 2 Resonators from Deck when controlling 1 DARK Dragon Synchro!
                AI.SelectCard(new[] {
                    CardId.VisionResonator,
                    CardId.SynkronResonator,
                    CardId.SoulResonator,
                    CardId.DarknessResonator
                });
                return true;
            }
            return false;
        }

        private bool KuibeltSummon()
        {
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0;
        }

        private bool KuibeltEffect()
        {
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target == null) target = Enemy.GetSpells().FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CrimsonBladeSummon()
        {
            return true;
        }

        private bool ScarredDragonSummon()
        {
            return true;
        }

        private bool ScarredDragonEffect()
        {
            // On sent from Monster Zone to GY: SS Red Dragon Archfiend from Extra Deck
            // If sent as Synchro material for DARK Dragon: destroy all opponent Attack monsters!
            AI.SelectCard(new[] {
                CardId.RedDragonArchfiend
            });
            return true;
        }

        private bool RedDragonArchfiendSummon()
        {
            return true;
        }

        private bool RedDragonArchfiendEffect()
        {
            return true;
        }

        private bool TheCrimsonKingSummon()
        {
            return true;
        }

        private bool HotRedAbyssSummon()
        {
            // Primary Level 9 Omni-negate boss
            return true;
        }

        private bool HotRedBaneSummon()
        {
            return true;
        }

        private bool HotRedBaneEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Tribute monster to revive RDA from GY
                ClientCard tribute = Bot.GetMonsters().FirstOrDefault(m => m != Card && m.Level <= 4 && !m.HasType(CardType.Synchro));
                if (tribute != null && Bot.Graveyard.Any(c => c.Id == CardId.RedDragonArchfiend || c.Id == CardId.ScarredDragonArchfiend || c.Id == CardId.HotRedDragonArchfiendAbyss))
                {
                    AI.SelectCard(tribute);
                    return true;
                }
            }
            return false;
        }

        private bool RedSupernovaSummon()
        {
            // Ultimate Level 12 Triple-Tuner boss (4000+ ATK & full board banish)
            return true;
        }

        private bool DisPaterSummon()
        {
            // Level 10 DARK Dragon Synchro
            return Bot.GetMonsters().Any(m => m.IsTuner()) && Bot.GetMonsters().Any(m => !m.IsTuner() && m.HasRace(CardRace.Dragon));
        }

        private bool DisPaterEffect()
        {
            // Effect 1: Target 1 banished LIGHT/DARK monster to Special Summon
            if (Card.Location == CardLocation.MonsterZone && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var banishedTarget = Bot.Banished.FirstOrDefault(c => (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)) && c.IsMonster());
                if (banishedTarget != null)
                {
                    AI.SelectCard(banishedTarget);
                    return true;
                }
            }
            // Effect 2: Quick Effect when opponent monster effect activates
            if (Duel.LastChainPlayer == 1)
            {
                var target = Bot.Banished.FirstOrDefault() ?? Enemy.Banished.FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool RedNovaSummon()
        {
            return true;
        }

        private bool RedHypernovaSummon()
        {
            return true;
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

        public override bool OnSelectYesNo(long desc)
        {
            return base.OnSelectYesNo(desc);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
