// ============================================================================
// CARD AUDIT — GOAT_SkillDrian (Skill Drain Beatdown)
// ============================================================================
// | Card Name                   | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |-----------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Skill Drain                 | Trap Cont    | No   | No    | 1000 LP | Negates effects of all face-up monsters on fld| Activate ASAP to super-buff beatsticks & stun | Already active on field or LP <= 1000       |
// | Fusilier Dragon             | Monster L7   | No   | No    | None    | NS without tribute; stats reset to 2800/2000! | Normal Summon without tribute                 | We need tributes for some reason            |
// | Goblin Attack Force         | Monster L4   | No   | No    | None    | 2300 ATK; under Skill Drain never switches DEF| Normal summon beatstick                       | Opponent has lethal attacker                |
// | Giant Orc                   | Monster L4   | No   | No    | None    | 2200 ATK; under Skill Drain never switches DEF| Normal summon beatstick                       | Opponent has lethal attacker                |
// | Jirai Gumo                  | Monster L4   | No   | No    | None    | 2200 ATK; under Skill Drain attacks free!     | Normal summon beater                          | LP <= 1000 and Skill Drain not active       |
// | Zombyra the Dark            | Monster L4   | No   | No    | None    | 2100 ATK; under Skill Drain direct attack OK! | Normal summon beater                          | Never                                       |
// | Berserk Gorilla             | Monster L4   | No   | No    | None    | 2000 ATK; under Skill Drain no self-destruct  | Normal summon beater                          | Opponent has stronger monster               |
// | Breaker the Magical Warrior | Monster L4   | Yes  | No    | 1 Cntr  | Pop 1 S/T (activate before flipping Drain)    | Pop threatening backrow before Skill Drain    | Skill Drain already active on field         |
// | Exiled Force                | Monster L4   | No   | No    | Tribute | Tribute self to pop mon (BYPASSES SKILL DRAIN)| Pop opponent threat (works under Skill Drain!)| Opponent has 0 monsters                     |
// | Reinforcement of the Army   | Spell Normal | No   | No    | None    | Add Level 4 or lower Warrior from deck to hand| Search Exiled Force (removal) or Goblin/Zomby | No targets left in deck                     |
// | Nobleman of Crossout        | Spell Normal | No   | No    | None    | Banish 1 face-down monster; strip copies      | Banish opponent face-down defense monster     | Opponent has 0 face-down monsters           |
// | Smashing Ground             | Spell Normal | No   | No    | None    | Destroy 1 face-up monster with highest DEF    | Destroy big enemy monster                     | Opponent has 0 face-up monsters             |
// | Snatch Steal                | Spell Equip  | No   | No    | None    | Take control of 1 opponent face-up monster    | Steal biggest enemy monster for attack/tribute | Opponent has 0 face-up monsters             |
// | Premature Burial            | Spell Equip  | No   | No    | 800 LP  | Pay 800 LP: SS monster from GY (Fusilier 2800)| Revive Fusilier or Goblin for lethal push     | Bot LP <= 800 or GY has no targets          |
// | Call of the Haunted         | Trap Cont    | No   | No    | None    | Special Summon 1 monster from GY in ATK       | Revive Fusilier (2800) during Battle/End Phase| No monsters in GY                           |
// | Final Attack Orders         | Trap Cont    | No   | No    | None    | All monsters to face-up ATK; cannot change pos| Lock opponent in ATK for beatdown             | Skill Drain not active                      |
// | Solemn Judgment             | Trap Counter | No   | No    | Half LP | Pay half LP: Negate Summon or Spell/Trap card | Protect Skill Drain from Heavy/MST or stop boss| Trivial opponent cards                      |
// | Pot of Greed                | Spell Normal | No   | No    | None    | Draw 2 cards                                  | Always activate                               | Never                                       |
// | Graceful Charity            | Spell Normal | No   | No    | Discard2| Draw 3 cards, then discard 2 cards            | Always activate; pitch beatsticks for Reborn  | Hand is empty                               |
// | Delinquent Duo              | Spell Normal | No   | No    | 1000 LP | Pay 1000 LP: Opp discards 2 cards from hand   | Early hand rip (Opp hand >= 2)                | Bot LP <= 1000                              |
// | Mystical Space Typhoon      | Spell Quick  | No   | No    | None    | Destroy 1 Spell/Trap on field                 | Remove opponent backrow / floodgates          | No targets                                  |
// | Dust Tornado                | Trap Normal  | No   | No    | None    | Destroy 1 Spell/Trap on field; set 1 S/T      | End Phase removal of opponent backrow         | No targets                                  |
// | Mirror Force                | Trap Normal  | No   | No    | None    | Destroy all Attack Position opponent monsters | Opponent attacks with strong board            | Weak single attack                          |
// | Torrential Tribute          | Trap Normal  | No   | No    | None    | Destroy all monsters on field on summon       | Opponent summons big boss or multiple monsters| We control Fusilier / superior board        |
// | Ring of Destruction         | Trap Normal  | No   | No    | None    | Destroy face-up monster; both take damage     | Remove dangerous monster or lethal burn       | ATK >= our LP                               |
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
    [Deck("GOAT_SkillDrian", "GOAT_SkillDrian")]
    public class GOAT_SkillDrianExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Monsters
            public const int GiantOrc = 73698349;
            public const int GoblinAttackForce = 78658564;
            public const int BerserkGorilla = 39168895;
            public const int ZombyraTheDark = 88472456;
            public const int FusilierDragonTheDualModeBeast = 51632798;
            public const int JiraiGumo = 94773007;
            public const int BreakerTheMagicalWarrior = 71413901;
            public const int ExiledForce = 74131780;

            // Spells
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int DelinquentDuo = 44763025;
            public const int PrematureBurial = 70828912;
            public const int SnatchSteal = 45986603;
            public const int NoblemanOfCrossout = 71044499;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int SmashingGround = 97169186;
            public const int MysticalSpaceTyphoon = 5318639;

            // Traps
            public const int FinalAttackOrders = 52503575;
            public const int DustTornado = 60082869;
            public const int SkillDrain = 82732705;
            public const int SolemnJudgment = 41420027;
            public const int MirrorForce = 44095762;
            public const int RingOfDestruction = 83555666;
            public const int TorrentialTribute = 53582587;
            public const int CallOfTheHaunted = 97077563;
        }

        private static readonly int[] BossMonsters =
        {
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
            // ── 1. Counter Traps & Hand Traps ──
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);

            // ── 2. Draw & Hand Disruption ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);
            AddExecutor(ExecutorType.Activate, CardId.DelinquentDuo, DelinquentDuoEffect);

            // ── 3. Backrow Removal & Spot Removal ──
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.DustTornado, DustTornadoEffect);
            AddExecutor(ExecutorType.Activate, CardId.NoblemanOfCrossout, NoblemanOfCrossoutEffect);
            AddExecutor(ExecutorType.Activate, CardId.SmashingGround, DefaultSmashingGround);
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);

            // Exiled Force: Tributes for COST in GY, so it bypasses Skill Drain!
            AddExecutor(ExecutorType.Activate, CardId.ExiledForce, ExiledForceEffect);

            // ── 4. Reinforcement of the Army (Search) ──
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotAEffect);

            // ── 5. Skill Drain & Final Attack Orders ──
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainEffect);
            AddExecutor(ExecutorType.Activate, CardId.FinalAttackOrders, FinalAttackOrdersEffect);

            // ── 6. Normal Summons (Beatsticks) ──
            // Fusilier Dragon: Normal Summon without tribute (stats jump to 2800 under Skill Drain!)
            AddExecutor(ExecutorType.Summon, CardId.FusilierDragonTheDualModeBeast, FusilierSummon);
            AddExecutor(ExecutorType.Summon, CardId.GoblinAttackForce);
            AddExecutor(ExecutorType.Summon, CardId.GiantOrc);
            AddExecutor(ExecutorType.Summon, CardId.BerserkGorilla);
            AddExecutor(ExecutorType.Summon, CardId.ZombyraTheDark);
            AddExecutor(ExecutorType.Summon, CardId.JiraiGumo);
            AddExecutor(ExecutorType.Summon, CardId.BreakerTheMagicalWarrior);
            AddExecutor(ExecutorType.Summon, CardId.ExiledForce, ExiledForceSummon);

            // ── 7. Reborn / Premature ──
            AddExecutor(ExecutorType.Activate, CardId.PrematureBurial, PrematureBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.CallOfTheHaunted, CallOfTheHauntedEffect);

            // ── 8. Reactive Traps ──
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, TorrentialTributeEffect);
            AddExecutor(ExecutorType.Activate, CardId.RingOfDestruction, RingOfDestructionEffect);

            AddExecutor(ExecutorType.SpellSet, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        // ═══════════════════════════════════════════════════════════════
        //  OCGCore Callbacks & Hint Handling
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            const long HINTMSG_RELEASE = 500;
            const long HINTMSG_DESTROY = 502;
            const long HINTMSG_REMOVE = 503;
            const long HINTMSG_ATOHAND = 506;
            const long HINTMSG_SPSUMMON = 509;

            // 1. Tribute Cost (Exiled Force): Tribute Exiled Force
            if (hint == HINTMSG_RELEASE)
            {
                var exiled = cards.FirstOrDefault(c => c.IsCode(CardId.ExiledForce));
                if (exiled != null) return new List<ClientCard> { exiled };
            }

            // 2. Search / Add to Hand (Reinforcement of the Army):
            if (hint == HINTMSG_ATOHAND)
            {
                // If opponent has strong monster: search Exiled Force
                if (Enemy.GetMonsterCount() > 0 && cards.Any(c => c.IsCode(CardId.ExiledForce)))
                {
                    var exiled = cards.First(c => c.IsCode(CardId.ExiledForce));
                    return new List<ClientCard> { exiled };
                }

                // Otherwise search highest ATK Warrior
                var bestWarrior = cards.OrderByDescending(c => c.Attack).FirstOrDefault();
                if (bestWarrior != null) return new List<ClientCard> { bestWarrior };
            }

            // 3. Special Summon (Premature Burial / Call of the Haunted):
            // Priority: Fusilier Dragon (2800) > Goblin Attack Force (2300) > Giant Orc (2200) > Zombyra (2100)
            if (hint == HINTMSG_SPSUMMON)
            {
                var reviveTargets = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.FusilierDragonTheDualModeBeast)) return 1;
                    if (c.IsCode(CardId.GoblinAttackForce)) return 2;
                    if (c.IsCode(CardId.GiantOrc)) return 3;
                    if (c.IsCode(CardId.ZombyraTheDark)) return 4;
                    if (c.IsCode(CardId.BerserkGorilla)) return 5;
                    return 10;
                }).ToList();

                if (reviveTargets.Count >= min)
                    return reviveTargets.Take(min).ToList();
            }

            // 4. Target Destruction (Exiled Force / Ring of Destruction):
            if (hint == HINTMSG_DESTROY)
            {
                var threats = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.MonsterZone && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).ToList();
                if (threats.Count >= min)
                    return threats.Take(min).ToList();
            }

            // 5. Target Banish (Nobleman of Crossout):
            if (hint == HINTMSG_REMOVE)
            {
                var facedown = cards.FirstOrDefault(c => c.Controller == 1 && c.IsFacedown());
                if (facedown != null) return new List<ClientCard> { facedown };
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // All beatsticks should be in Attack Position
            if (positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
            if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;

            return base.OnSelectPosition(cardId, positions);
        }

        // ═══════════════════════════════════════════════════════════════
        //  Strategic Decision Logic
        // ═══════════════════════════════════════════════════════════════

        private bool SolemnJudgmentEffect()
        {
            if (Util.GetLastChainCard() == null || Util.GetLastChainCard().Controller == 0) return false;

            // Priority 1: Protect Skill Drain from destruction / bounce
            var last = Util.GetLastChainCard();
            if (Bot.HasInSpellZone(CardId.SkillDrain) &&
                (last.IsCode(CardId.MysticalSpaceTyphoon) || last.IsCode(CardId.DustTornado) ||
                 last.Id == 19613556 /* Heavy Storm */ || last.Id == 42703248 /* Giant Trunade */))
            {
                return true;
            }

            // Priority 2: Negate game-winning opponent plays
            return last.Id == 19613556 /* Heavy Storm */ ||
                   last.Id == CardId.SnatchSteal ||
                   last.Id == CardId.TorrentialTribute ||
                   last.Id == 72989439 /* BLS Envoy */;
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
            ClientCard target = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                .OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target != null && target.Attack >= 1500)
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
            ClientCard target = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                .OrderByDescending(c => c.Attack).FirstOrDefault();
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
            return Bot.GetRemainingCount(CardId.GoblinAttackForce, 1) > 0 ||
                   Bot.GetRemainingCount(CardId.ExiledForce, 1) > 0 ||
                   Bot.GetRemainingCount(CardId.ZombyraTheDark, 1) > 0;
        }

        private bool SkillDrainEffect()
        {
            // Activate Skill Drain as soon as possible if we have beatsticks or if opponent relies on monster effects
            return !Bot.HasInSpellZone(CardId.SkillDrain) && Bot.LifePoints > 1000;
        }

        private bool FinalAttackOrdersEffect()
        {
            // Best activated when Skill Drain is also active or about to be active
            return !Bot.HasInSpellZone(CardId.FinalAttackOrders) &&
                   (Bot.HasInSpellZone(CardId.SkillDrain) || Bot.HasInHand(CardId.SkillDrain));
        }

        private bool FusilierSummon()
        {
            // Without tribute: 1400 ATK normally, but under Skill Drain it has full 2800 ATK!
            if (Bot.HasInSpellZone(CardId.SkillDrain) || Bot.HasInHand(CardId.SkillDrain)) return true;
            // Without Skill Drain: summon if no other monster in hand
            return !Bot.Hand.Any(c => c != null && c.Id != Card.Id && c.IsMonster() && c.Level <= 4 && c.Attack >= 1800);
        }

        private bool PrematureBurialEffect()
        {
            if (Bot.LifePoints <= 800) return false;
            return Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.IsCanRevive() && c.Attack >= 2000);
        }

        private bool CallOfTheHauntedEffect()
        {
            return Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.IsCanRevive() && c.Attack >= 2000);
        }

        private bool MirrorForceEffect()
        {
            if (Duel.Player != 1) return false;
            if (Enemy.BattlingMonster != null && Enemy.BattlingMonster.Attack >= 1500) return true;
            return Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsAttack()) >= 2;
        }

        private bool TorrentialTributeEffect()
        {
            // Don't wipe if we control Fusilier (2800) and opponent controls weaker monsters
            if (Bot.HasInMonstersZone(CardId.FusilierDragonTheDualModeBeast) && Enemy.GetMonsterCount() <= 1) return false;
            return DefaultTorrentialTribute();
        }

        private bool RingOfDestructionEffect()
        {
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            ClientCard target = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack < Bot.LifePoints && !IsTargetImmune(c))
                .OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SetTrapCondition()
        {
            return Card.HasType(CardType.Trap);
        }

        private bool MonsterRepos()
        {
            bool skillDrainActive = Bot.HasInSpellZone(CardId.SkillDrain);
            bool finalAttackActive = Bot.HasInSpellZone(CardId.FinalAttackOrders);

            // With Skill Drain or Final Attack Orders: keep ALL beatsticks in Attack Position
            if (skillDrainActive || finalAttackActive)
            {
                if (Card.IsDefense() && Card.IsFaceup()) return true;
                return false;
            }

            // Without Skill Drain: Fusilier at 1400 ATK switches to DEF if enemy has stronger monster
            if (Card.IsCode(CardId.FusilierDragonTheDualModeBeast) && Card.IsFaceup() && Card.Attack <= 1400)
            {
                if (Card.IsAttack() && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack > 1400))
                    return true;
                return false;
            }

            return DefaultMonsterRepos();
        }
    }
}
