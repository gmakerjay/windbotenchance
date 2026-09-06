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
    // GOAT_SkillDrian (Skill Drain Beatdown)
    // ==========================================
    [Deck("GOAT_SkillDrian", "GOAT_SkillDrian")]
    public class GOAT_SkillDrianExecutor : ModernExecutor
    {
        public class CardId
        {
            public const int GiantOrc = 73698349;
            public const int GoblinAttackForce = 78658564;
            public const int BerserkGorilla = 39168895;
            public const int ZombyraTheDark = 88472456;
            public const int FusilierDragonTheDualModeBeast = 51632798;
            public const int JiraiGumo = 94773007;
            public const int BreakerTheMagicalWarrior = 71413901;
            public const int ExiledForce = 74131780;
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int DelinquentDuo = 44763025;
            public const int PrematureBurial = 70828912;
            public const int SnatchSteal = 45986603;
            public const int NoblemanOfCrossout = 71044499;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int SmashingGround = 97169186;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int FinalAttackOrders = 52503575;
            public const int DustTornado = 60082869;
            public const int SkillDrain = 82732705;
            public const int SolemnJudgment = 41420027;
            public const int MirrorForce = 44095762;
            public const int RingOfDestruction = 83555666;
            public const int TorrentialTribute = 53582587;
            public const int CallOfTheHaunted = 97077563;
            
            // Add missing GOAT cards
            public const int HeavyStorm = 19613556;
            public const int GiantTrunade = 42703248;
        }

        private static readonly int[] BossMonsters = {
            CardId.FusilierDragonTheDualModeBeast,
            CardId.GoblinAttackForce,
            CardId.GiantOrc
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public GOAT_SkillDrianExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── Staples & Negations ──
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);
            AddExecutor(ExecutorType.Activate, CardId.DelinquentDuo, DelinquentDuoEffect);

            // ── Backrow removal & Spot removal ──
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.DustTornado, DustTornadoEffect);
            AddExecutor(ExecutorType.Activate, CardId.NoblemanOfCrossout, NoblemanOfCrossoutEffect);
            AddExecutor(ExecutorType.Activate, CardId.SmashingGround, DefaultSmashingGround);
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);
            AddExecutor(ExecutorType.Activate, CardId.ExiledForce, ExiledForceEffect);

            // ── Reinforcement of the Army (Search) ──
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotAEffect);

            // ── Skill Drain & Final Attack Orders ──
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainEffect);
            AddExecutor(ExecutorType.Activate, CardId.FinalAttackOrders, FinalAttackOrdersEffect);

            // ── Summons ──
            // Normal Summon Fusilier without tribute (he will gain full stats if Skill Drain is active)
            AddExecutor(ExecutorType.Summon, CardId.FusilierDragonTheDualModeBeast, FusilierSummon);
            AddExecutor(ExecutorType.Summon, CardId.GoblinAttackForce);
            AddExecutor(ExecutorType.Summon, CardId.GiantOrc);
            AddExecutor(ExecutorType.Summon, CardId.BerserkGorilla);
            AddExecutor(ExecutorType.Summon, CardId.ZombyraTheDark);
            AddExecutor(ExecutorType.Summon, CardId.JiraiGumo);
            AddExecutor(ExecutorType.Summon, CardId.BreakerTheMagicalWarrior);
            AddExecutor(ExecutorType.Summon, CardId.ExiledForce, ExiledForceSummon);

            // Reborn / Premature
            AddExecutor(ExecutorType.Activate, CardId.PrematureBurial, PrematureBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.CallOfTheHaunted, DefaultCallOfTheHaunted);

            // ── Traps ──
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, DefaultTorrentialTribute);
            AddExecutor(ExecutorType.Activate, CardId.RingOfDestruction, RingOfDestructionEffect);

            // ── Backrow Clearance ──
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.GiantTrunade, GiantTrunadeEffect);

            AddExecutor(ExecutorType.SpellSet, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        private bool SolemnJudgmentEffect()
        {
            // Protect Skill Drain or our big beatsticks, or negate game-winning opponent plays
            if (Util.GetLastChainCard() == null) return false;
            if (Util.GetLastChainCard().Controller == 0) return false; // Don't negate ourselves
            
            // Prioritize protecting Skill Drain
            bool isSkillDrainTarget = Util.GetLastChainCard().IsSpell() || Util.GetLastChainCard().IsTrap();
            if (isSkillDrainTarget && Bot.HasInSpellZone(CardId.SkillDrain))
            {
                return true;
            }

            // Negate big summons or spells/traps
            return Util.GetLastChainCard().Id == CardId.HeavyStorm ||
                Util.GetLastChainCard().Id == CardId.GiantTrunade ||
                Util.GetLastChainCard().Id == CardId.SnatchSteal ||
                Util.GetLastChainCard().Id == CardId.TorrentialTribute;
        }

        private bool DelinquentDuoEffect()
        {
            return Bot.LifePoints > 1000 && Enemy.Hand.Count >= 2;
        }

        private bool DustTornadoEffect()
        {
            return DefaultMysticalSpaceTyphoon();
        }

        private bool NoblemanOfCrossoutEffect()
        {
            if (DefaultSpellWillBeNegated()) return false;
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFacedown());
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

        private bool RotAEffect()
        {
            // Search the best warrior for the situation
            if (Enemy.GetMonsterCount() > 0 && !Bot.HasInSpellZone(CardId.SkillDrain))
            {
                // Need removal — search Exiled Force if available
                if (Bot.GetRemainingCount(CardId.ExiledForce, 1) > 0)
                    AI.SelectCard(CardId.ExiledForce, CardId.GoblinAttackForce, CardId.ZombyraTheDark);
                else
                    AI.SelectCard(CardId.GoblinAttackForce, CardId.ZombyraTheDark, CardId.GiantOrc);
            }
            else
            {
                // Get biggest beater
                if (Bot.GetRemainingCount(CardId.GoblinAttackForce, 1) > 0)
                    AI.SelectCard(CardId.GoblinAttackForce, CardId.ZombyraTheDark, CardId.ExiledForce);
                else
                    AI.SelectCard(CardId.ZombyraTheDark, CardId.GiantOrc, CardId.ExiledForce);
            }
            return true;
        }

        private bool SkillDrainEffect()
        {
            // Always activate as soon as possible if we control our beatsticks or if opponent relies on monster effects
            return !Bot.HasInSpellZone(CardId.SkillDrain) && Bot.LifePoints > 1000;
        }

        private bool FinalAttackOrdersEffect()
        {
            // Only activate if Skill Drain is also active (otherwise our Goblin/Orc suffer)
            if (!Bot.HasInSpellZone(CardId.SkillDrain)) return false;
            return !Bot.HasInSpellZone(CardId.FinalAttackOrders);
        }

        private bool FusilierSummon()
        {
            // Without tribute, Fusilier loses half stats (1400/1200)
            // Only summon without tribute if Skill Drain is active (restores full 2800/2000)
            bool skillDrainActive = Bot.HasInSpellZone(CardId.SkillDrain);
            if (skillDrainActive) return true;
            // Without Skill Drain: only summon if we have no other monsters to play
            bool hasOtherMonster = Bot.Hand.Any(c => c != null && c.Id != Card.Id && c.IsMonster() && c.Level <= 4 && c.Attack >= 1800);
            return !hasOtherMonster && Bot.GetMonsterCount() == 0;
        }

        private bool PrematureBurialEffect()
        {
            // Summon Fusilier (2800 ATK) or Goblin (2300 ATK) or Giant Orc (2200 ATK)
            if (Bot.LifePoints > 800)
            {
                ClientCard target = Bot.Graveyard.FirstOrDefault(c => c != null && (c.IsCode(CardId.FusilierDragonTheDualModeBeast) || c.IsCode(CardId.GoblinAttackForce) || c.IsCode(CardId.GiantOrc)) && c.IsCanRevive());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool MirrorForceEffect()
        {
            if (Duel.Player != 1) return false;
            // Only use on meaningful attacks
            if (Enemy.BattlingMonster != null && Enemy.BattlingMonster.Attack >= 1500) return true;
            if (Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsAttack()) >= 2) return true;
            return false;
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

        private bool SetTrapCondition()
        {
            // Set traps including Skill Drain and Final Attack Orders
            return Card.HasType(CardType.Trap);
        }

        private bool MonsterRepos()
        {
            bool skillDrainActive = Bot.HasInSpellZone(CardId.SkillDrain);
            bool finalAttackActive = Bot.HasInSpellZone(CardId.FinalAttackOrders);

            // With Skill Drain active: Goblin/Orc/Fusilier won't switch to DEF, so keep ATK
            if (skillDrainActive || finalAttackActive)
            {
                if (Card.IsDefense() && Card.IsFaceup()) return true; // Switch to ATK
                return false;
            }

            // Without Skill Drain: use default smart repos
            // Fusilier at half stats (1400 ATK) — switch to DEF if enemy has anything stronger
            if (Card.IsCode(CardId.FusilierDragonTheDualModeBeast) && Card.IsFaceup() && Card.Attack <= 1400)
            {
                if (Card.IsAttack() && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack > 1400))
                    return true; // Switch to DEF
                return false;
            }

            return DefaultMonsterRepos();
        }

        private bool HeavyStormEffect()
        {
            // Don't destroy our own Skill Drain
            if (Bot.HasInSpellZone(CardId.SkillDrain)) return false;
            return Enemy.GetSpellCount() > Bot.GetSpellCount() && Enemy.GetSpellCount() >= 2;
        }

        private bool GiantTrunadeEffect()
        {
            // Bounce everything — use before Skill Drain is set, or to clear opponent backrow for lethal
            if (Bot.HasInSpellZone(CardId.SkillDrain)) return false; // Don't bounce our Skill Drain
            return Enemy.GetSpellCount() >= 2 || (Enemy.GetSpellCount() >= 1 && GetTotalFieldATK() >= Enemy.LifePoints);
        }
    }
}
