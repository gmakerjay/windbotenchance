// ============================================================================
// CARD AUDIT — GOAT_Panda (Panda Ojama OTK / Burn)
// ============================================================================
// | Card Name                   | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |-----------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Gyaku-Gire Panda            | Monster L3   | No   | No    | None    | +500 ATK per opp mon; Piercing battle damage  | Summon to board; attacks under Gravity/Limit  | Opponent has 0 monsters                     |
// | Injection Fairy Lily        | Monster L3   | Yes  | No    | 2000 LP | Damage calculation: +3000 ATK (3400 total)    | Beat high ATK enemy or lethal direct attack   | Bot LP <= 2000 or overkill non-lethal       |
// | Giant Rat                   | Monster L4   | No   | No    | None    | Battle destroyed: SS EARTH <=1500 ATK from DK | Tutor Panda / Lily / Des Koala / Exiled Force | Deck has 0 valid EARTH monsters             |
// | Breaker the Magical Warrior | Monster L4   | Yes  | No    | 1 Cntr  | 1600+300 ATK; remove counter to pop 1 S/T     | Pop threatening backrow before pushing attacks| Opponent has 0 Spells/Traps                 |
// | Exiled Force                | Monster L4   | No   | No    | Tribute | Tribute self to destroy 1 monster on field    | Clear problematic monster or boss             | Opponent has 0 monsters                     |
// | Sangan                      | Monster L3   | No   | No    | None    | Sent to GY: Add monster <=1500 ATK from deck  | Search Panda / Lily / Exiled Force / Koala    | No targets left in deck                     |
// | Des Koala                   | Monster L3   | No   | No    | None    | FLIP: 400 damage per card in opponent hand    | Set face-down; burn when opp has large hand   | Hand has better plays                        |
// | Cyber Jar                   | Monster L3   | No   | No    | None    | FLIP: Destroy all monsters; draw/summon 5     | Set face-down when losing board               | We already have Panda + lethal board        |
// | Morphing Jar                | Monster L2   | No   | No    | None    | FLIP: Both players discard hand, draw 5 cards | Set when hand is small (<=2 cards)            | Hand is full of good cards                  |
// | Pot of Greed                | Spell Normal | No   | No    | None    | Draw 2 cards                                  | Always activate                               | Never                                       |
// | Graceful Charity            | Spell Normal | No   | No    | Discard2| Draw 3 cards, then discard 2 cards            | Always activate; cycle combo pieces           | Hand is empty                               |
// | Heavy Storm                 | Spell Normal | No   | No    | None    | Destroy all Spells and Traps on field         | Clear backrow before battle push              | We rely on Gravity Bind to stay alive       |
// | Giant Trunade               | Spell Normal | No   | No    | None    | Return all S/T to hand                         | Clear backrow for OTK without destroying locks| Opponent has 0 backrow                      |
// | Snatch Steal                | Spell Equip  | No   | No    | None    | Take control of 1 opponent face-up monster    | Steal biggest enemy monster for attack/tribute | Opponent has 0 face-up monsters             |
// | Premature Burial            | Spell Equip  | No   | No    | 800 LP  | Pay 800 LP: Special Summon monster from GY    | Revive Panda or Lily for battle push          | Bot LP <= 800 or GY has no targets          |
// | Swords of Revealing Light   | Spell Normal | No   | No    | None    | Opponent cannot attack for 3 turns; flip face | Stall and protect setup                       | Already under stall lock and safe           |
// | Level Limit - Area B        | Spell Cont   | No   | No    | None    | Level 4+ monsters changed to Defense Position | Setup stall lock (Panda/Lily bypass this!)    | Already active on field                     |
// | Call of the Haunted         | Trap Cont    | No   | No    | None    | Special Summon 1 monster from GY in ATK       | Revive Panda/Lily during Battle or End Phase  | No monsters in GY                           |
// | Magic Cylinder              | Trap Normal  | No   | No    | None    | Negate attack and inflict damage equal to ATK | High ATK attack or lethal burn                | Negligible attack ATK                       |
// | Ring of Destruction         | Trap Normal  | No   | No    | None    | Destroy face-up monster; both take damage     | Remove dangerous monster or lethal burn       | ATK >= our LP                               |
// | Gravity Bind                | Trap Cont    | No   | No    | None    | Level 4+ monsters cannot attack               | Stop opp attacks (Panda/Lily Level 3 bypass!) | Already active on field                     |
// | Ceasefire                   | Trap Normal  | No   | No    | None    | Flips face-down monsters; 500 burn per effect | 3+ effect monsters or facedowns on field      | Less than 2 effect monsters                 |
// | Just Desserts               | Trap Normal  | No   | No    | None    | Inflict 500 damage per opponent monster       | Combos with Ojama Trio (+1500 dmg) or lethal  | Opponent has 0 monsters                     |
// | Secret Barrel               | Trap Normal  | No   | No    | None    | Inflict 200 damage per card in opp hand/field | Opp cards >= 6 (or for lethal burn)          | Opp has small hand and board                |
// | Ojama Trio                  | Trap Normal  | No   | No    | None    | SS 3 Ojama Tokens (0/1000) to opp field; 300  | Combos with Panda (+1500 ATK) & Desserts      | Opponent has 3+ monsters (cannot summon 3)  |
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
    [Deck("GOAT_Panda", "GOAT_Panda")]
    public class GOAT_PandaExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Monsters
            public const int BreakerTheMagicalWarrior = 71413901;
            public const int GiantRat = 97017120;
            public const int GyakuGirePanda = 9817927;
            public const int InjectionFairyLily = 79575620;
            public const int ExiledForce = 74131780;
            public const int Sangan = 26202165;
            public const int DesKoala = 69579761;
            public const int CyberJar = 34124316;
            public const int MorphingJar = 33508719;

            // Spells
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int HeavyStorm = 19613556;
            public const int GiantTrunade = 42703248;
            public const int SnatchSteal = 45986603;
            public const int PrematureBurial = 70828912;
            public const int SwordsOfRevealingLight = 72302403;
            public const int LevelLimitAreaB = 3136426;

            // Traps
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

        private static readonly int[] BossMonsters =
        {
            CardId.GyakuGirePanda,
            CardId.InjectionFairyLily
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public GOAT_PandaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Draw & Search ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);

            // ── 2. Backrow Clearance Before Attacks ──
            AddExecutor(ExecutorType.Activate, CardId.GiantTrunade, GiantTrunadeEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.BreakerTheMagicalWarrior, BreakerEffect);
            AddExecutor(ExecutorType.Activate, CardId.ExiledForce, ExiledForceEffect);

            // ── 3. Revival & Control ──
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrematureBurial, PrematureBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.CallOfTheHaunted, CallOfTheHauntedEffect);

            // ── 4. Injection Fairy Lily Combat Pump ──
            AddExecutor(ExecutorType.Activate, CardId.InjectionFairyLily, InjectionFairyLilyEffect);

            // ── 5. Flip Monsters ──
            AddExecutor(ExecutorType.Activate, CardId.CyberJar);
            AddExecutor(ExecutorType.Activate, CardId.MorphingJar);
            AddExecutor(ExecutorType.Activate, CardId.DesKoala);

            // ── 6. Ojama Trio Board Clogging & Burn Setup ──
            AddExecutor(ExecutorType.Activate, CardId.OjamaTrio, OjamaTrioEffect);

            // ── 7. Stall Setup (Panda & Lily bypass Level Limit/Gravity Bind!) ──
            AddExecutor(ExecutorType.Activate, CardId.SwordsOfRevealingLight);
            AddExecutor(ExecutorType.Activate, CardId.LevelLimitAreaB, LevelLimitAreaBEffect);
            AddExecutor(ExecutorType.Activate, CardId.GravityBind, GravityBindEffect);

            // ── 8. Summons ──
            // Panda summon: prioritized when opponent has monsters or Ojama Trio is ready
            AddExecutor(ExecutorType.Summon, CardId.GyakuGirePanda, GyakuGirePandaSummon);
            AddExecutor(ExecutorType.Summon, CardId.InjectionFairyLily, InjectionFairyLilySummon);
            AddExecutor(ExecutorType.Summon, CardId.BreakerTheMagicalWarrior);
            AddExecutor(ExecutorType.Summon, CardId.ExiledForce, ExiledForceSummon);

            // Defensive Set / Floaters
            AddExecutor(ExecutorType.MonsterSet, CardId.GiantRat);
            AddExecutor(ExecutorType.MonsterSet, CardId.Sangan);
            AddExecutor(ExecutorType.MonsterSet, CardId.DesKoala);
            AddExecutor(ExecutorType.MonsterSet, CardId.CyberJar);
            AddExecutor(ExecutorType.MonsterSet, CardId.MorphingJar);

            // Fallback Summons
            AddExecutor(ExecutorType.Summon, CardId.GiantRat);
            AddExecutor(ExecutorType.Summon, CardId.DesKoala);

            // Floating Trigger
            AddExecutor(ExecutorType.Activate, CardId.GiantRat, GiantRatEffect);
            AddExecutor(ExecutorType.Activate, CardId.Sangan);

            // ── 9. Traps ──
            AddExecutor(ExecutorType.Activate, CardId.MagicCylinder, MagicCylinderEffect);
            AddExecutor(ExecutorType.Activate, CardId.RingOfDestruction, RingOfDestructionEffect);

            // ── 10. Burn Traps ──
            AddExecutor(ExecutorType.Activate, CardId.Ceasefire, CeasefireEffect);
            AddExecutor(ExecutorType.Activate, CardId.JustDesserts, JustDessertsEffect);
            AddExecutor(ExecutorType.Activate, CardId.SecretBarrel, SecretBarrelEffect);

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
            const long HINTMSG_ATOHAND = 506;
            const long HINTMSG_SPSUMMON = 509;

            // 1. Tribute Cost (Exiled Force): Tribute Exiled Force
            if (hint == HINTMSG_RELEASE)
            {
                var exiled = cards.FirstOrDefault(c => c.IsCode(CardId.ExiledForce));
                if (exiled != null) return new List<ClientCard> { exiled };
            }

            // 2. Special Summon Selection:
            // Giant Rat / Premature Burial / Call of the Haunted:
            // Priority: Panda (if opp has >=2 mon) > Lily > Des Koala > Exiled Force
            if (hint == HINTMSG_SPSUMMON)
            {
                var targets = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.GyakuGirePanda) && Enemy.GetMonsterCount() >= 1) return 1;
                    if (c.IsCode(CardId.InjectionFairyLily) && Bot.LifePoints > 2000) return 2;
                    if (c.IsCode(CardId.GyakuGirePanda)) return 3;
                    if (c.IsCode(CardId.DesKoala)) return 4;
                    if (c.IsCode(CardId.ExiledForce)) return 5;
                    return 10;
                }).ToList();

                if (targets.Count >= min)
                    return targets.Take(min).ToList();
            }

            // 3. Search / Add to Hand (Sangan):
            if (hint == HINTMSG_ATOHAND)
            {
                var sanganPicks = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.GyakuGirePanda) && Enemy.GetMonsterCount() >= 2) return 1;
                    if (c.IsCode(CardId.InjectionFairyLily) && Bot.LifePoints > 2000) return 2;
                    if (c.IsCode(CardId.GyakuGirePanda)) return 3;
                    if (c.IsCode(CardId.ExiledForce) && Enemy.GetMonsterCount() > 0) return 4;
                    if (c.IsCode(CardId.DesKoala)) return 5;
                    return 10;
                }).ToList();

                if (sanganPicks.Count >= min)
                    return sanganPicks.Take(min).ToList();
            }

            // 4. Target Destruction (Exiled Force / Ring of Destruction):
            if (hint == HINTMSG_DESTROY)
            {
                var threats = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.MonsterZone && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).ToList();
                if (threats.Count >= min)
                    return threats.Take(min).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Gyaku-Gire Panda & Injection Fairy Lily: Always Attack position (they attack under stall)
            if (cardId == CardId.GyakuGirePanda || cardId == CardId.InjectionFairyLily)
            {
                if (positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
            }

            // Floaters / Flips: Defense position
            if (cardId == CardId.GiantRat || cardId == CardId.DesKoala || cardId == CardId.Sangan ||
                cardId == CardId.CyberJar || cardId == CardId.MorphingJar)
            {
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        // ═══════════════════════════════════════════════════════════════
        //  Strategic Decision Logic
        // ═══════════════════════════════════════════════════════════════

        private bool HasStallLock()
        {
            return Bot.HasInSpellZone(CardId.GravityBind) || Bot.HasInSpellZone(CardId.LevelLimitAreaB) ||
                   Enemy.HasInSpellZone(CardId.GravityBind) || Enemy.HasInSpellZone(CardId.LevelLimitAreaB) ||
                   Bot.HasInSpellZone(CardId.SwordsOfRevealingLight);
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
            ClientCard target = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                .OrderByDescending(c => c.Attack).FirstOrDefault();
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

        private bool GiantTrunadeEffect()
        {
            // Clear enemy backrow when ready to attack with Panda or Lily
            bool hasAttacker = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                (c.IsCode(CardId.GyakuGirePanda) || c.IsCode(CardId.InjectionFairyLily)));
            return hasAttacker && Enemy.GetSpellCount() >= 1;
        }

        private bool HeavyStormEffect()
        {
            // Do not destroy our own stall cards if opponent has beatsticks that would then kill us
            if (HasStallLock() && Enemy.GetMonsterCount() > 0) return false;
            return Enemy.GetSpellCount() > Bot.GetSpellCount() && Enemy.GetSpellCount() >= 2;
        }

        private bool PrematureBurialEffect()
        {
            if (Bot.LifePoints <= 800) return false;
            return Bot.Graveyard.Any(c => c != null && (c.IsCode(CardId.GyakuGirePanda) || c.IsCode(CardId.InjectionFairyLily)) && c.IsCanRevive());
        }

        private bool CallOfTheHauntedEffect()
        {
            return Bot.Graveyard.Any(c => c != null && (c.IsCode(CardId.GyakuGirePanda) || c.IsCode(CardId.InjectionFairyLily)) && c.IsCanRevive());
        }

        private bool InjectionFairyLilyEffect()
        {
            // 2000 LP cost check
            if (Bot.LifePoints <= 2000) return false;
            if (Card.Location != CardLocation.MonsterZone || !Card.IsFaceup()) return false;
            if (Bot.BattlingMonster != Card) return false;

            ClientCard defender = Enemy.BattlingMonster;
            if (defender == null)
            {
                // Direct attack: Only pump if it is lethal
                return 3400 >= Enemy.LifePoints;
            }
            else
            {
                int defPower = defender.GetDefensePower();
                // Pump if we cannot destroy the defender without it
                if (Card.Attack < defPower) return true;
                // Pump if the battle damage dealt will be lethal
                if (3400 - defPower >= Enemy.LifePoints) return true;
                return false;
            }
        }

        private bool OjamaTrioEffect()
        {
            int emptyZones = 5 - Enemy.GetMonsterCount();
            if (emptyZones < 3) return false;
            return true; // Always clog zones to power up Panda (+1500 ATK & 1300 pierce dmg) and burn
        }

        private bool LevelLimitAreaBEffect()
        {
            return !Bot.HasInSpellZone(CardId.LevelLimitAreaB);
        }

        private bool GravityBindEffect()
        {
            return !Bot.HasInSpellZone(CardId.GravityBind);
        }

        private bool GyakuGirePandaSummon()
        {
            // Panda is Level 3 (bypasses stall!) and gains 500 ATK per opponent monster
            if (Enemy.GetMonsterCount() >= 1) return true;
            if (Bot.HasInHand(CardId.OjamaTrio) || Bot.HasInSpellZone(CardId.OjamaTrio)) return true;
            return Bot.GetMonsterCount() == 0;
        }

        private bool InjectionFairyLilySummon()
        {
            // Lily is Level 3 (bypasses stall!)
            if (Enemy.GetMonsterCount() > 0 || (Enemy.LifePoints <= 3400 && Bot.LifePoints > 2000)) return true;
            return Bot.GetMonsterCount() == 0;
        }

        private bool GiantRatEffect()
        {
            return true;
        }

        private bool MagicCylinderEffect()
        {
            if (Duel.Player != 1 || Enemy.BattlingMonster == null) return false;
            if (Enemy.BattlingMonster.Attack >= Enemy.LifePoints) return true;
            return Enemy.BattlingMonster.Attack >= 1500;
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

        private bool CeasefireEffect()
        {
            int count = Bot.GetMonsters().Concat(Enemy.GetMonsters())
                .Count(c => c != null && (c.IsFacedown() || c.HasType(CardType.Effect)));
            if (count * 500 >= Enemy.LifePoints) return true;
            return count >= 3;
        }

        private bool JustDessertsEffect()
        {
            int damage = Enemy.GetMonsterCount() * 500;
            if (damage >= Enemy.LifePoints) return true;
            return Enemy.GetMonsterCount() >= 3;
        }

        private bool SecretBarrelEffect()
        {
            int count = Enemy.Hand.Count + Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            int damage = count * 200;
            if (damage >= Enemy.LifePoints) return true;
            return count >= 6;
        }

        private bool SetTrapCondition()
        {
            return Card.HasType(CardType.Trap);
        }

        private bool MonsterRepos()
        {
            bool isLocked = HasStallLock();

            // Gyaku-Gire Panda is Level 3 (unaffected by Gravity/Level Limit) -> Attack!
            if (Card.IsCode(CardId.GyakuGirePanda) && Card.IsFaceup())
            {
                if (Card.IsDefense()) return true;
                return false;
            }

            // Lily is Level 3 -> Attack if we can afford the pump!
            if (Card.IsCode(CardId.InjectionFairyLily) && Card.IsFaceup())
            {
                if (Card.IsDefense() && Bot.LifePoints > 2000) return true;
                return false;
            }

            // Des Koala should stay in DEF for flip burn
            if (Card.IsCode(CardId.DesKoala))
            {
                if (Card.IsFaceup() && Card.IsAttack()) return true;
                return false;
            }

            // Other Level 4+ monsters under stall -> DEF
            if (isLocked && Card.Level >= 4 && Card.IsAttack())
            {
                return true;
            }

            return DefaultMonsterRepos();
        }
    }
}
