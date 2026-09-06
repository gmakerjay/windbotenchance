using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ==========================================
    // GOAT_RedEyes (Reasoning Dragon & Metamorphosis)
    // ==========================================
    [Deck("GOAT_RedEyes", "GOAT_RedEyes")]
    public class GOAT_RedEyesExecutor : ModernExecutor
    {
        public class CardId
        {
            public const int RedEyesDarknessDragon = 96561011;
            public const int ArmedDragonLv7 = 73879377;
            public const int MirageDragon = 15960641;
            public const int RedEyesBlackDragon = 74677422;
            public const int ArmedDragonLv5 = 46384672;
            public const int LordOfD = 17985575;
            public const int MaskedDragon = 39191307;
            public const int ElementDragon = 30314994;
            public const int CyberStein = 69015963;
            public const int BlackDragonsChick = 36262024;
            public const int ArmedDragonLv3 = 980973;
            public const int SpiritRyu = 67957315;
            public const int LusterDragon = 11091375;
            public const int MyBodyAsAShield = 69279219;
            public const int TheDragonsBead = 92408984;
            public const int DifferentDimensionCapsule = 11961740;
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int SuperRejuvenation = 27770341;
            public const int MirrorForce = 44095762;
            public const int LightningVortex = 69162969;
            public const int SnatchSteal = 45986603;
            public const int CardDestruction = 72892473;
            public const int DragonsRage = 54178050;
            public const int InfernoFireBlast = 52684508;
            public const int GiantTrunade = 42703248;
            public const int HeavyStorm = 19613556;
            public const int SoulRelease = 5758500;
            public const int Reasoning = 58577036;
            public const int Metamorphosis = 46411259;
            public const int TheFluteOfSummoningDragon = 43973174;
            public const int DimensionFusion = 23557835;

            // Extra Deck
            public const int DarkfireDragon = 17881964;
            public const int FiendSkullDragon = 66235877;
            public const int KingDragun = 13756293;
            public const int GaiaTheDragonChampion = 66889139;
            public const int BlackSkullDragon = 11901678;
            public const int ThousandEyesRestrict = 63519819;
        }

        private static readonly int[] BossMonsters = {
            CardId.RedEyesDarknessDragon,
            CardId.ArmedDragonLv7,
            CardId.KingDragun,
            CardId.BlackSkullDragon,
            CardId.ThousandEyesRestrict
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public GOAT_RedEyesExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── Draw & Search ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);
            AddExecutor(ExecutorType.Activate, CardId.SuperRejuvenation, SuperRejuvenationEffect);
            AddExecutor(ExecutorType.Activate, CardId.DifferentDimensionCapsule, DifferentDimensionCapsuleEffect);

            // ── Backrow removal / Disruption negation ──
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, DefaultHeavyStorm);
            AddExecutor(ExecutorType.Activate, CardId.GiantTrunade, GiantTrunadeEffect);
            AddExecutor(ExecutorType.Activate, CardId.MyBodyAsAShield, MyBodyAsAShieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheDragonsBead);

            // ── Hand Destruction & Soul Release ──
            AddExecutor(ExecutorType.Activate, CardId.CardDestruction, CardDestructionEffect);
            AddExecutor(ExecutorType.Activate, CardId.SoulRelease, SoulReleaseEffect);

            // ── Reasoning (Cheat Summons) ──
            AddExecutor(ExecutorType.Activate, CardId.Reasoning);

            // ── Direct Damage / Buffs ──
            AddExecutor(ExecutorType.Activate, CardId.InfernoFireBlast, InfernoFireBlastEffect);

            // ── Special Summons / Tribute Actions ──
            AddExecutor(ExecutorType.Activate, CardId.BlackDragonsChick, BlackDragonsChickEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.RedEyesDarknessDragon, RedEyesDarknessDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.RedEyesDarknessDragon);

            // ── Cyber-Stein & Dimension Fusion (Big plays) ──
            AddExecutor(ExecutorType.Activate, CardId.CyberStein, CyberSteinEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionFusion, DimensionFusionEffect);

            // ── Metamorphosis ──
            AddExecutor(ExecutorType.Activate, CardId.Metamorphosis, MetamorphosisEffect);

            // ── Flute of Summoning Dragon ──
            AddExecutor(ExecutorType.Activate, CardId.TheFluteOfSummoningDragon, TheFluteOfSummoningDragonEffect);

            // ── Monster Effects ──
            AddExecutor(ExecutorType.Activate, CardId.ArmedDragonLv3, ArmedDragonLv3Effect);
            AddExecutor(ExecutorType.Activate, CardId.ArmedDragonLv5, ArmedDragonLv5Effect);
            AddExecutor(ExecutorType.Activate, CardId.ArmedDragonLv7, ArmedDragonLv7Effect);
            AddExecutor(ExecutorType.Activate, CardId.SpiritRyu, SpiritRyuEffect);
            AddExecutor(ExecutorType.Activate, CardId.KingDragun, KingDragunEffect);
            AddExecutor(ExecutorType.Activate, CardId.ThousandEyesRestrict, ThousandEyesRestrictEffect);

            // ── Direct Damage / Buffs (Remaining) ──
            AddExecutor(ExecutorType.Activate, CardId.DragonsRage);
            AddExecutor(ExecutorType.Activate, CardId.LightningVortex, LightningVortexEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);

            // ── Summons ──
            AddExecutor(ExecutorType.Summon, CardId.LusterDragon);
            AddExecutor(ExecutorType.Summon, CardId.MirageDragon);
            AddExecutor(ExecutorType.Summon, CardId.LordOfD);
            AddExecutor(ExecutorType.Summon, CardId.ElementDragon);
            AddExecutor(ExecutorType.MonsterSet, CardId.MaskedDragon);
            AddExecutor(ExecutorType.Summon, CardId.ArmedDragonLv3);
            AddExecutor(ExecutorType.MonsterSet, CardId.ArmedDragonLv3);
            AddExecutor(ExecutorType.Summon, CardId.BlackDragonsChick);

            // Floating
            AddExecutor(ExecutorType.Activate, CardId.MaskedDragon, MaskedDragonEffect);

            // ── Traps ──
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);

            AddExecutor(ExecutorType.SpellSet, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        private bool MyBodyAsAShieldEffect()
        {
            return Bot.LifePoints > 1500 && Duel.LastChainPlayer == 1;
        }

        private bool LightningVortexEffect()
        {
            if (DefaultSpellWillBeNegated()) return false;
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack >= 1500) && Bot.Hand.Count > 0;
        }

        private bool SnatchStealEffect()
        {
            if (DefaultSpellWillBeNegated()) return false;
            ClientCard target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c)).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool MirrorForceEffect()
        {
            return Duel.Player == 1 && Duel.Phase == DuelPhase.BattleStart;
        }

        private bool SuperRejuvenationEffect()
        {
            // Activate in End Phase if we discarded/tributed Dragons this turn
            return Duel.Phase == DuelPhase.End;
        }

        private bool DifferentDimensionCapsuleEffect()
        {
            // Search BLS or Red-Eyes Darkness Dragon
            AI.SelectCard(CardId.RedEyesDarknessDragon, CardId.DimensionFusion, CardId.Reasoning);
            return true;
        }

        private bool GiantTrunadeEffect()
        {
            return Enemy.GetSpellCount() >= 2 || (Enemy.GetSpellCount() >= 1 && GetTotalFieldATK() >= Enemy.LifePoints);
        }

        private bool CardDestructionEffect()
        {
            // Play if we have multiple Dragons in hand to dump to GY (setup Darkness Dragon)
            int dragonCount = Bot.Hand.Count(c => c != null && c.HasRace(CardRace.Dragon));
            return dragonCount >= 2 || Bot.Hand.Count >= 5;
        }

        private bool SoulReleaseEffect()
        {
            // Banish opponent GY targets, or banish our Dragons if we have Dimension Fusion in hand
            if (Bot.HasInHand(CardId.DimensionFusion) || Bot.GetSpells().Any(c => c != null && c.IsCode(CardId.DimensionFusion)))
            {
                var dragons = Bot.Graveyard.Where(c => c != null && c.HasRace(CardRace.Dragon)).Take(5).ToList();
                if (dragons.Count >= 3)
                {
                    AI.SelectCard(dragons);
                    return true;
                }
            }
            if (Enemy.Graveyard.Count >= 3)
            {
                var targets = Enemy.Graveyard.OrderByDescending(c => c.Attack).Take(5).ToList();
                AI.SelectCard(targets);
                return true;
            }
            return false;
        }

        private bool BlackDragonsChickEffect()
        {
            // Tribute to Special Summon Red-Eyes from hand
            return Bot.Hand.Any(c => c != null && c.IsCode(CardId.RedEyesBlackDragon));
        }

        private bool RedEyesDarknessDragonSummon()
        {
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.RedEyesBlackDragon));
        }

        private bool CyberSteinEffect()
        {
            // Pay 5000 LP to summon Fusion from Extra Deck
            if (Bot.LifePoints <= 5000 || Bot.GetMonsterCount() >= 5) return false;

            if (Enemy.GetMonsterCount() > 0 && Enemy.GetMonsters().Any(c => c != null && c.Attack >= 2500))
            {
                // Don't summon another TER if one is already on field
                if (!Bot.HasInMonstersZone(CardId.ThousandEyesRestrict))
                {
                    AI.SelectCard(CardId.ThousandEyesRestrict);
                    return true;
                }
            }
            if (Bot.Hand.Any(c => c != null && c.HasRace(CardRace.Dragon) && c.Level >= 5))
            {
                AI.SelectCard(CardId.KingDragun);
                return true;
            }
            AI.SelectCard(CardId.BlackSkullDragon);
            return true;
        }

        private bool DimensionFusionEffect()
        {
            // Special Summon banished Dragons
            if (Bot.LifePoints > 2000)
            {
                int count = Bot.Banished.Count(c => c != null && c.IsMonster());
                return count >= 2 || (count >= 1 && GetTotalFieldATK() + 2400 >= Enemy.LifePoints);
            }
            return false;
        }

        private bool MetamorphosisEffect()
        {
            // Tribute Level 1 -> Thousand-Eyes Restrict
            // Tribute Level 7 -> King Dragun (but NOT our boss Armed Dragon LV7)
            // Tribute Level 9 -> Black Skull Dragon
            ClientCard target = Bot.MonsterZone.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && (c.Level == 1 || c.Level == 7 || c.Level == 9) && !IsAceCard(c))
                .OrderBy(c => c.Attack) // Tribute weakest monster of the matching level
                .FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                if (target.Level == 1)
                {
                    // Don't summon TER if already on field
                    if (Bot.HasInMonstersZone(CardId.ThousandEyesRestrict)) return false;
                    AI.SelectNextCard(CardId.ThousandEyesRestrict);
                }
                else if (target.Level == 7) AI.SelectNextCard(CardId.KingDragun);
                else if (target.Level == 9) AI.SelectNextCard(CardId.BlackSkullDragon);
                return true;
            }
            return false;
        }

        private bool TheFluteOfSummoningDragonEffect()
        {
            // Play if Lord of D is face-up and we have Dragons in hand
            bool hasLordOfD = Bot.HasInMonstersZone(CardId.LordOfD);
            bool hasDragons = Bot.Hand.Any(c => c != null && c.HasRace(CardRace.Dragon));
            return hasLordOfD && hasDragons;
        }

        private bool ArmedDragonLv3Effect()
        {
            // Level up to LV5 during End Phase (if LV5 is in deck)
            if (Duel.Phase == DuelPhase.Standby && Duel.Player == 0)
            {
                return Bot.GetRemainingCount(CardId.ArmedDragonLv5, 1) > 0;
            }
            return false;
        }

        private bool ArmedDragonLv5Effect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Send to GY in Standby to summon LV7
                if (Duel.Phase == DuelPhase.Standby && Duel.Player == 0)
                {
                    return true;
                }
                // Discard 1 monster to destroy face-up opponent monster with equal or lower ATK
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attack <= 2400);
                if (target != null && Bot.Hand.Any(c => c != null && c.IsMonster()))
                {
                    ClientCard discard = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster());
                    if (discard != null)
                    {
                        AI.SelectCard(discard);
                        AI.SelectNextCard(target);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ArmedDragonLv7Effect()
        {
            // Discard 1 monster to destroy all face-up opponent monsters with equal or lower ATK
            if (Enemy.GetMonsterCount() >= 2 && Bot.Hand.Any(c => c != null && c.IsMonster()))
            {
                ClientCard discard = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster());
                if (discard != null)
                {
                    AI.SelectCard(discard);
                    return true;
                }
            }
            return false;
        }

        private bool SpiritRyuEffect()
        {
            // Pump during Battle Step by discarding Dragon monsters
            if (Card.Location != CardLocation.MonsterZone || Bot.BattlingMonster != Card) return false;

            var dragons = Bot.Hand.Where(c => c != null && c.HasRace(CardRace.Dragon)).ToList();
            if (dragons.Count == 0) return false;

            ClientCard defender = Enemy.BattlingMonster;
            if (defender == null)
            {
                // Direct attack: only pump if it will help reach lethal
                int currentATK = Card.Attack + (1000 * dragons.Count);
                if (currentATK >= Enemy.LifePoints && Card.Attack < Enemy.LifePoints)
                {
                    AI.SelectCard(dragons.First());
                    return true;
                }
                return false; // Don't waste dragons on non-lethal direct
            }
            else
            {
                // Battle: pump to beat over or survive
                int defPower = defender.GetDefensePower();
                if (Card.Attack < defPower)
                {
                    // Need to pump to win
                    AI.SelectCard(dragons.First());
                    return true;
                }
            }
            return false;
        }

        private bool KingDragunEffect()
        {
            // Special Summon 1 Dragon from hand (need empty zone)
            if (Bot.GetMonsterCount() >= 5) return false;
            var dragon = Bot.Hand.FirstOrDefault(c => c != null && c.HasRace(CardRace.Dragon));
            if (dragon != null)
            {
                AI.SelectCard(dragon);
                return true;
            }
            return false;
        }

        private bool ThousandEyesRestrictEffect()
        {
            // Target face-up opponent monster to absorb (only if not already carrying an absorbed monster)
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && Card.EquipCards.Count == 0)
            {
                ClientCard target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool InfernoFireBlastEffect()
        {
            // Burn opponent for 2400 (Red-Eyes Black Dragon attack)
            return Bot.HasInMonstersZone(CardId.RedEyesBlackDragon);
        }

        private bool MaskedDragonEffect()
        {
            // Summon another Masked Dragon (for deck thinning) or Armed Dragon LV3
            AI.SelectCard(CardId.MaskedDragon, CardId.ArmedDragonLv3);
            return true;
        }

        private bool SetTrapCondition()
        {
            return Card.HasType(CardType.Trap);
        }

        private bool MonsterRepos()
        {
            // Thousand-Eyes Restrict: keep in ATK if absorbed monster gives high ATK, otherwise stay
            if (Card.IsCode(CardId.ThousandEyesRestrict))
            {
                return false; // TER locks all attacks, position doesn't matter much
            }
            // Masked Dragon: keep in DEF to float when destroyed
            if (Card.IsCode(CardId.MaskedDragon))
            {
                if (Card.IsFaceup() && Card.IsAttack()) return true; // Switch to DEF to float
                return false;
            }
            return DefaultMonsterRepos();
        }
    }
}
