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
    // GOAT_Panda (Panda Ojama OTK / Burn)
    // ==========================================
    [Deck("GOAT_Panda", "GOAT_Panda")]
    public class GOAT_PandaExecutor : ModernExecutor
    {
        public class CardId
        {
            public const int BreakerTheMagicalWarrior = 71413901;
            public const int GiantRat = 97017120;
            public const int GyakuGirePanda = 9817927;
            public const int InjectionFairyLily = 79575620;
            public const int ExiledForce = 74131780;
            public const int Sangan = 26202165;
            public const int DesKoala = 69579761;
            public const int CyberJar = 34124316;
            public const int MorphingJar = 33508719;
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int HeavyStorm = 19613556;
            public const int GiantTrunade = 42703248;
            public const int SnatchSteal = 45986603;
            public const int PrematureBurial = 70828912;
            public const int SwordsOfRevealingLight = 72302403;
            public const int LevelLimitAreaB = 3136426;
            public const int CallOfTheHaunted = 97077563;
            public const int MagicCylinder = 62279055;
            public const int RingOfDestruction = 83555666;
            public const int GravityBind = 85742772;
            public const int Ceasefire = 36468556;
            public const int JustDesserts = 24068492;
            public const int SecretBarrel = 27053506;
            public const int OjamaTrio = 29843091;
            
            // Tokens
            public const int OjamaToken = 29843092;
        }

        private static readonly int[] BossMonsters = {
            CardId.GyakuGirePanda,
            CardId.InjectionFairyLily
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public GOAT_PandaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── Staples & Draw ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);

            // ── Backrow Clearance ──
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.GiantTrunade, GiantTrunadeEffect);
            AddExecutor(ExecutorType.Activate, CardId.BreakerTheMagicalWarrior, BreakerEffect);
            AddExecutor(ExecutorType.Activate, CardId.ExiledForce, ExiledForceEffect);

            // ── Equipment / Reborn ──
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrematureBurial, PrematureBurialEffect);

            // ── Injection Fairy Lily (Activate in battle phase to pump) ──
            AddExecutor(ExecutorType.Activate, CardId.InjectionFairyLily, InjectionFairyLilyEffect);

            // ── Flip Monsters ──
            AddExecutor(ExecutorType.Activate, CardId.CyberJar);
            AddExecutor(ExecutorType.Activate, CardId.MorphingJar);

            // ── Ojama Trio ──
            AddExecutor(ExecutorType.Activate, CardId.OjamaTrio, OjamaTrioEffect);

            // ── Stall Setup ──
            AddExecutor(ExecutorType.Activate, CardId.SwordsOfRevealingLight);
            AddExecutor(ExecutorType.Activate, CardId.LevelLimitAreaB, LevelLimitAreaBEffect);
            
            // ── Summons ──
            AddExecutor(ExecutorType.Summon, CardId.GyakuGirePanda, GyakuGirePandaSummon);
            AddExecutor(ExecutorType.Summon, CardId.InjectionFairyLily, InjectionFairyLilySummon);
            AddExecutor(ExecutorType.Summon, CardId.BreakerTheMagicalWarrior);
            AddExecutor(ExecutorType.Summon, CardId.ExiledForce, ExiledForceSummon);
            
            // Float set
            AddExecutor(ExecutorType.MonsterSet, CardId.GiantRat);
            AddExecutor(ExecutorType.MonsterSet, CardId.Sangan);
            AddExecutor(ExecutorType.MonsterSet, CardId.DesKoala);
            AddExecutor(ExecutorType.MonsterSet, CardId.CyberJar);
            AddExecutor(ExecutorType.MonsterSet, CardId.MorphingJar);

            AddExecutor(ExecutorType.Summon, CardId.GiantRat);
            AddExecutor(ExecutorType.Summon, CardId.DesKoala);

            // ── Floater Trigger ──
            AddExecutor(ExecutorType.Activate, CardId.GiantRat, GiantRatEffect);

            // ── Traps ──
            AddExecutor(ExecutorType.Activate, CardId.GravityBind, GravityBindEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagicCylinder, MagicCylinderEffect);
            AddExecutor(ExecutorType.Activate, CardId.RingOfDestruction, RingOfDestructionEffect);
            AddExecutor(ExecutorType.Activate, CardId.CallOfTheHaunted, DefaultCallOfTheHaunted);

            // ── Burn ──
            AddExecutor(ExecutorType.Activate, CardId.Ceasefire, CeasefireEffect);
            AddExecutor(ExecutorType.Activate, CardId.JustDesserts, JustDessertsEffect);
            AddExecutor(ExecutorType.Activate, CardId.SecretBarrel, SecretBarrelEffect);

            AddExecutor(ExecutorType.SpellSet, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        private bool BreakerEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && Card.Attack == 1900)
            {
                return DefaultMysticalSpaceTyphoon();
            }
            return false;
        }

        private bool ExiledForceSummon()
        {
            return Enemy.GetMonsterCount() > 0;
        }

        private bool ExiledForceEffect()
        {
            ClientCard target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target == null) target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFacedown());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
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

        private bool MagicCylinderEffect()
        {
            if (Duel.Player != 1) return false;
            if (Enemy.BattlingMonster == null) return false;
            // Always activate if it will be lethal
            if (Enemy.BattlingMonster.Attack >= Enemy.LifePoints) return true;
            // Otherwise only activate on meaningful attacks
            return Enemy.BattlingMonster.Attack >= 1500;
        }

        private bool GiantTrunadeEffect()
        {
            // Clear backrow before launching big attacks, or bounce stall cards to enable our attacks
            bool aboutToAttack = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.GyakuGirePanda) || c.IsCode(CardId.InjectionFairyLily)));
            return aboutToAttack && (Enemy.GetSpellCount() > 0 || Bot.GetSpells().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.GravityBind) || c.IsCode(CardId.LevelLimitAreaB))));
        }

        private bool PrematureBurialEffect()
        {
            // Summon Panda or Lily
            if (Bot.LifePoints > 800)
            {
                ClientCard target = Bot.Graveyard.FirstOrDefault(c => c != null && (c.IsCode(CardId.GyakuGirePanda) || c.IsCode(CardId.InjectionFairyLily)) && c.IsCanRevive());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool InjectionFairyLilyEffect()
        {
            // Pump during damage calculation if we can afford the 2000 LP
            if (Bot.LifePoints <= 2000) return false;
            if (Card.Location != CardLocation.MonsterZone || !Card.IsFaceup()) return false;
            if (Bot.BattlingMonster != Card) return false;

            ClientCard defender = Enemy.BattlingMonster;
            if (defender == null)
            {
                // Direct attack: pump only if it will be lethal
                return 3400 >= Enemy.LifePoints;
            }
            else
            {
                int defPower = defender.GetDefensePower();
                // Pump if: we would lose without it, or the extra damage is lethal
                if (Card.Attack < defPower) return true; // Can't win without pump
                if (3400 - defPower >= Enemy.LifePoints) return true; // Pump makes it lethal
                return false; // Don't waste 2000 LP if we'd win anyway
            }
        }

        private bool OjamaTrioEffect()
        {
            // Need at least 3 empty zones for all tokens
            int emptyZones = 5 - Enemy.GetMonsterCount();
            if (emptyZones < 3) return false;
            // Always good to clog zones for Panda pierce and burn
            return true;
        }

        private bool LevelLimitAreaBEffect()
        {
            // Don't activate if we control a face-up level 4 attacker and opponent has no threats
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level >= 4 && c.IsCode(CardId.BreakerTheMagicalWarrior)) && Enemy.GetMonsterCount() == 0)
                return false;
            return !Bot.HasInSpellZone(CardId.LevelLimitAreaB);
        }

        private bool GyakuGirePandaSummon()
        {
            if (Enemy.GetMonsterCount() >= 1) return true;
            if (Bot.HasInHand(CardId.OjamaTrio) || Bot.HasInSpellZone(CardId.OjamaTrio)) return true;
            if (!Bot.Hand.Any(c => c != null && c.Id != Card.Id && c.HasType(CardType.Monster))) return true;
            return false;
        }

        private bool InjectionFairyLilySummon()
        {
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 1500)) return true;
            if (Enemy.LifePoints <= 3400 && Bot.LifePoints > 2000) return true;
            if (!Bot.Hand.Any(c => c != null && c.Id != Card.Id && c.HasType(CardType.Monster))) return true;
            return false;
        }

        private bool GiantRatEffect()
        {
            // Summon Panda (if opponent has monsters) or Lily or Des Koala (earth)
            if (Enemy.GetMonsterCount() >= 2)
            {
                AI.SelectCard(CardId.GyakuGirePanda, CardId.InjectionFairyLily, CardId.DesKoala);
            }
            else
            {
                AI.SelectCard(CardId.InjectionFairyLily, CardId.GyakuGirePanda, CardId.DesKoala);
            }
            return true;
        }

        private bool GravityBindEffect()
        {
            // Don't activate if we need to attack with Level 4+ this turn (Breaker)
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1 &&
                Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level >= 4 && c.IsAttack()))
                return false;
            // Don't activate if already active
            if (Bot.HasInSpellZone(CardId.GravityBind)) return false;
            // Activate during opponent's turn or if opponent has Level 4+ threats
            return Duel.Player == 1 || Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Level >= 4);
        }

        private bool RingOfDestructionEffect()
        {
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            ClientCard target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack < Bot.LifePoints).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CeasefireEffect()
        {
            int count = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Count(c => c != null && (c.IsFacedown() || c.HasType(CardType.Effect)));
            return count >= 3;
        }

        private bool JustDessertsEffect()
        {
            return Enemy.GetMonsterCount() >= 3 || (Enemy.GetMonsterCount() >= 1 && Enemy.LifePoints <= Enemy.GetMonsterCount() * 500);
        }

        private bool SecretBarrelEffect()
        {
            int count = Enemy.Hand.Count + Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            return count >= 6 || (count >= 1 && Enemy.LifePoints <= count * 200);
        }

        private bool SetTrapCondition()
        {
            return Card.HasType(CardType.Trap);
        }

        private bool MonsterRepos()
        {
            bool isLocked = Bot.HasInSpellZone(CardId.GravityBind) || Bot.HasInSpellZone(CardId.LevelLimitAreaB) || Enemy.HasInSpellZone(CardId.GravityBind) || Enemy.HasInSpellZone(CardId.LevelLimitAreaB);
            
            // Gyaku-Gire Panda is Level 3, so it is unaffected by Gravity Bind and Level Limit!
            if (Card.IsCode(CardId.GyakuGirePanda) && Card.IsFaceup())
            {
                if (Card.IsDefense()) return true; // Flip to attack
                return false;
            }

            // Lily is Level 3, so she is also unaffected!
            if (Card.IsCode(CardId.InjectionFairyLily) && Card.IsFaceup())
            {
                if (Card.IsDefense() && Bot.LifePoints > 2000) return true; // Flip to attack
                return false;
            }

            // Des Koala should stay in DEF to trigger flip damage (800 per card in opponent hand)
            if (Card.IsCode(CardId.DesKoala))
            {
                if (Card.IsFaceup() && Card.IsAttack()) return true; // Switch to defense
                return false; // Stay set
            }

            if (isLocked)
            {
                if (Card.IsAttack() && Card.Level >= 4) return true; // Switch to defense
                return false;
            }

            return DefaultMonsterRepos();
        }

        private bool HeavyStormEffect()
        {
            // Don't destroy our own stall cards
            bool haveStallCards = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.GravityBind) || c.IsCode(CardId.LevelLimitAreaB) || c.IsCode(CardId.SwordsOfRevealingLight)));
            if (haveStallCards && Enemy.GetMonsterCount() > 0) return false;
            return Enemy.GetSpellCount() > Bot.GetSpellCount() && Enemy.GetSpellCount() >= 2;
        }
    }
}
