// ============================================================================
// CARD AUDIT — GOAT_Warrior (Chaos Warrior Control)
// ============================================================================
// | Card Name                   | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |-----------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | BLS - Envoy of the Beginning| Monster L8   | Yes  | No    | Banish2 | Banish 1 LIGHT + 1 DARK from GY; Banish / x2  | Summon boss; Banish threat or double attack   | Attack locked and no targets to banish      |
// | Blade Knight                | Monster L4   | No   | No    | None    | +400 ATK (2000 total) if hand <=1; negate Flip| Normal summon beater / negate flip monsters   | Opponent has stronger monster               |
// | Ninja Grandmaster Sasuke    | Monster L4   | No   | No    | None    | 1800 ATK; destroys face-up DEF mon at dmg step| Attack face-up Defense monster (Reaper/Tokens)| Opponent has high ATK monsters              |
// | D.D. Warrior Lady           | Monster L4   | Yes  | No    | None    | Battle: banish self and battling monster      | Crash into high ATK boss or dangerous floater | Battling monster <= 1500 ATK with no threat |
// | Zombyra the Dark            | Monster L4   | No   | No    | None    | 2100 ATK; loses 200 ATK when destroys monster | Normal summon beater to clear strong monsters | Opponent field is empty (cannot direct)     |
// | Kycoo the Ghost Destroyer   | Monster L4   | No   | No    | None    | 1800 ATK; battle dmg: banish 2 opp GY monsters| Attack to deal dmg and banish GY threats      | Opponent has 0 monsters in GY               |
// | Breaker the Magical Warrior | Monster L4   | Yes  | No    | 1 Cntr  | 1600+300 ATK; remove counter to pop 1 S/T     | Pop threatening backrow                       | Opponent has 0 Spells/Traps                 |
// | Don Zaloog                  | Monster L4   | Yes  | No    | None    | Battle dmg: rip 1 random card from opp hand   | Attack direct or weak monster to hand rip     | Opponent has strong attackers and no defense|
// | Tribe-Infecting Virus       | Monster L4   | No   | No    | Discard | Declare race: wipe face-up monsters of race   | Board wipe enemy swarm of same race           | Discarding BLS or key card                  |
// | Exiled Force                | Monster L4   | No   | No    | Tribute | Tribute self to pop 1 monster on field        | Pop dangerous boss or floodgate               | Opponent has 0 monsters                     |
// | Mystic Swordsman LV2        | Monster L2   | No   | No    | None    | Destroys face-down DEF monster without flipping| Attack face-down defense monsters safely      | Attacking face-up attackers                 |
// | Reinforcement of the Army   | Spell Normal | No   | No    | None    | Add Level 4 or lower Warrior from deck to hand| Search D.D. Warrior / Blade / Don / Sasuke    | No targets left in deck                     |
// | Pot of Greed                | Spell Normal | No   | No    | None    | Draw 2 cards                                  | Always activate                               | Never                                       |
// | Graceful Charity            | Spell Normal | No   | No    | Discard2| Draw 3 cards, then discard 2 cards            | Always activate; setup LIGHT/DARK in GY       | Hand is empty                               |
// | Delinquent Duo              | Spell Normal | No   | No    | 1000 LP | Pay 1000 LP: Opp discards 2 cards from hand   | Early hand rip (Opp hand >= 2)                | Bot LP <= 1000                              |
// | Premature Burial            | Spell Equip  | No   | No    | 800 LP  | Pay 800 LP: SS monster from GY (BLS 3000 ATK)| Revive BLS or Blade Knight for game push      | Bot LP <= 800 or GY has no targets          |
// | Snatch Steal                | Spell Equip  | No   | No    | None    | Take control of 1 opponent face-up monster    | Steal biggest enemy monster for attack/tribute | Opponent has 0 face-up monsters             |
// | Book of Moon                | Spell Quick  | No   | No    | None    | Change 1 face-up monster to face-down DEF     | Stop enemy attacker / set up Mystic Swordsman | No targets                                  |
// | Nobleman of Crossout        | Spell Normal | No   | No    | None    | Banish 1 face-down monster; strip copies      | Banish opponent face-down defense monster     | Opponent has 0 face-down monsters           |
// | Smashing Ground             | Spell Normal | No   | No    | None    | Destroy 1 face-up monster with highest DEF    | Destroy big enemy monster                     | Opponent has 0 face-up monsters             |
// | Heavy Storm                 | Spell Normal | No   | No    | None    | Destroy all Spells and Traps on field         | Clear opponent backrow                        | We control face-up equips / continuous      |
// | Mystical Space Typhoon      | Spell Quick  | No   | No    | None    | Destroy 1 Spell/Trap on field                 | Remove opponent backrow / floodgate           | No targets                                  |
// | Call of the Haunted         | Trap Cont    | No   | No    | None    | Special Summon 1 monster from GY in ATK       | Revive BLS during Battle/End Phase            | No monsters in GY                           |
// | Mirror Force                | Trap Normal  | No   | No    | None    | Destroy all Attack Position opponent monsters | Opponent attacks with strong board            | Weak single attack                          |
// | Torrential Tribute          | Trap Normal  | No   | No    | None    | Destroy all monsters on field on summon       | Opponent summons big boss or multiple monsters| We control BLS and our board is better      |
// | Ring of Destruction         | Trap Normal  | No   | No    | None    | Destroy face-up monster; both take damage     | Remove dangerous monster or lethal burn       | ATK >= our LP                               |
// | Sakuretsu Armor             | Trap Normal  | No   | No    | None    | Destroy attacking monster                     | Destroy attacker >= 1500 ATK                  | Attacker is weak and harmless               |
// | Dust Tornado                | Trap Normal  | No   | No    | None    | Destroy 1 Spell/Trap on field; set 1 S/T      | End Phase removal of opponent backrow         | No targets                                  |
// | Solemn Judgment             | Trap Counter | No   | No    | Half LP | Pay half LP: Negate Summon or Spell/Trap card | Stop Heavy Storm, Snatch Steal, or enemy boss | Trivial opponent cards                      |
// | Trap Dustshoot              | Trap Normal  | No   | No    | None    | Standby: Reveal opp hand, return 1 mon to deck| Opponent has >= 4 cards in hand in Standby    | Opp hand < 4 or not Standby Phase           |
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
    [Deck("GOAT_Warrior", "GOAT_Warrior")]
    public class GOAT_WarriorExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Monsters
            public const int BlackLusterSoldierEnvoyOfTheBeginning = 72989439;
            public const int BladeKnight = 39507162;
            public const int NinjaGrandmasterSasuke = 4041838;
            public const int DdWarriorLady = 7572887;
            public const int ZombyraTheDark = 88472456;
            public const int KycooTheGhostDestroyer = 88240808;
            public const int BreakerTheMagicalWarrior = 71413901;
            public const int DonZaloog = 76922029;
            public const int TribeInfectingVirus = 33184167;
            public const int ExiledForce = 74131780;
            public const int MysticSwordsmanLv2 = 47507260;

            // Spells
            public const int ReinforcementOfTheArmy = 32807846;
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int DelinquentDuo = 44763025;
            public const int PrematureBurial = 70828912;
            public const int SnatchSteal = 45986603;
            public const int BookOfMoon = 14087893;
            public const int NoblemanOfCrossout = 71044499;
            public const int SmashingGround = 97169186;
            public const int HeavyStorm = 19613556;
            public const int MysticalSpaceTyphoon = 5318639;

            // Traps
            public const int CallOfTheHaunted = 97077563;
            public const int MirrorForce = 44095762;
            public const int TorrentialTribute = 53582587;
            public const int RingOfDestruction = 83555666;
            public const int SakuretsuArmor = 56120475;
            public const int DustTornado = 60082869;
            public const int SolemnJudgment = 41420027;
            public const int TrapDustshoot = 64697231;

            // Stall Locks (constants)
            public const int GravityBind = 85742772;
            public const int LevelLimitAreaB = 3136426;
        }

        private static readonly int[] BossMonsters =
        {
            CardId.BlackLusterSoldierEnvoyOfTheBeginning,
            CardId.BladeKnight,
            CardId.DdWarriorLady
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public GOAT_WarriorExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Counter Traps & Hand Traps ──
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrapDustshoot, TrapDustshootEffect);

            // ── 2. Draw & Hand Disruption ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);
            AddExecutor(ExecutorType.Activate, CardId.DelinquentDuo, DelinquentDuoEffect);

            // ── 3. Backrow Removal & Spot Removal ──
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.DustTornado, DustTornadoEffect);
            AddExecutor(ExecutorType.Activate, CardId.NoblemanOfCrossout, NoblemanOfCrossoutEffect);
            AddExecutor(ExecutorType.Activate, CardId.SmashingGround, DefaultSmashingGround);
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);
            AddExecutor(ExecutorType.Activate, CardId.ExiledForce, ExiledForceEffect);

            // ── 4. Reinforcement of the Army (Search) ──
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotAEffect);

            // ── 5. Tribe-Infecting Virus Board Wipe ──
            AddExecutor(ExecutorType.Activate, CardId.TribeInfectingVirus, TribeInfectingVirusEffect);

            // ── 6. Spells / Book of Moon ──
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonEffect);

            // ── 7. Chaos Boss Summon (BLS - Envoy of the Beginning) ──
            AddExecutor(ExecutorType.SpSummon, CardId.BlackLusterSoldierEnvoyOfTheBeginning, BlsSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlackLusterSoldierEnvoyOfTheBeginning, BlsEffect);

            // ── 8. Reborn / Premature ──
            AddExecutor(ExecutorType.Activate, CardId.PrematureBurial, PrematureBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.CallOfTheHaunted, CallOfTheHauntedEffect);

            // ── 9. Monster Actions & Combat Triggers ──
            AddExecutor(ExecutorType.Activate, CardId.BreakerTheMagicalWarrior, BreakerEffect);
            AddExecutor(ExecutorType.Activate, CardId.DdWarriorLady, DdWarriorLadyEffect);
            AddExecutor(ExecutorType.Activate, CardId.DonZaloog, DonZaloogEffect);
            AddExecutor(ExecutorType.Activate, CardId.KycooTheGhostDestroyer, KycooEffect);

            // ── 10. Normal Summons ──
            // If opponent has face-down: prioritize Mystic Swordsman LV2
            AddExecutor(ExecutorType.Summon, CardId.MysticSwordsmanLv2, MysticSwordsmanLv2Summon);
            AddExecutor(ExecutorType.Summon, CardId.NinjaGrandmasterSasuke, SasukeSummon);
            AddExecutor(ExecutorType.Summon, CardId.BladeKnight);
            AddExecutor(ExecutorType.Summon, CardId.KycooTheGhostDestroyer);
            AddExecutor(ExecutorType.Summon, CardId.BreakerTheMagicalWarrior);
            AddExecutor(ExecutorType.Summon, CardId.DdWarriorLady);
            AddExecutor(ExecutorType.Summon, CardId.DonZaloog);
            AddExecutor(ExecutorType.Summon, CardId.ZombyraTheDark, ZombyraSummon);
            AddExecutor(ExecutorType.Summon, CardId.ExiledForce, ExiledForceSummon);

            // Defensive Set / Floaters
            AddExecutor(ExecutorType.MonsterSet, CardId.MysticSwordsmanLv2);
            AddExecutor(ExecutorType.MonsterSet, CardId.DdWarriorLady);
            AddExecutor(ExecutorType.Summon, CardId.MysticSwordsmanLv2);

            // ── 11. Traps ──
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, TorrentialTributeEffect);
            AddExecutor(ExecutorType.Activate, CardId.SakuretsuArmor, SakuretsuArmorEffect);
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
            const long HINTMSG_DISCARD = 501;
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

            // 2. Discard Selection (Tribe-Infecting Virus): Discard non-BLS monster
            if (hint == HINTMSG_DISCARD)
            {
                var discards = cards.Where(c => c.Controller == 0).OrderBy(c =>
                {
                    if (c.IsCode(CardId.ZombyraTheDark)) return 1;
                    if (c.IsCode(CardId.MysticSwordsmanLv2) && Enemy.GetMonsters().All(m => m.IsFaceup())) return 2;
                    if (IsAceCard(c)) return 99;
                    return 10;
                }).ToList();

                if (discards.Count >= min)
                    return discards.Take(min).ToList();
            }

            // 3. Search / Add to Hand (Reinforcement of the Army):
            if (hint == HINTMSG_ATOHAND)
            {
                var candidates = cards.OrderBy(c =>
                {
                    // Opponent has face-down -> Mystic Swordsman LV2
                    if (c.IsCode(CardId.MysticSwordsmanLv2) && Enemy.GetMonsters().Any(m => m.IsFacedown())) return 1;
                    // Opponent has face-up DEF -> Sasuke
                    if (c.IsCode(CardId.NinjaGrandmasterSasuke) && Enemy.GetMonsters().Any(m => m.IsFaceup() && m.IsDefense())) return 2;
                    // Opponent has huge boss -> D.D. Warrior Lady
                    if (c.IsCode(CardId.DdWarriorLady) && Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 2000)) return 3;
                    // Otherwise Blade Knight (high ATK) or Don Zaloog (hand rip)
                    if (c.IsCode(CardId.BladeKnight)) return 4;
                    if (c.IsCode(CardId.DonZaloog) && Enemy.GetMonsterCount() == 0) return 5;
                    if (c.IsCode(CardId.DdWarriorLady)) return 6;
                    return 10;
                }).ToList();

                if (candidates.Count >= min)
                    return candidates.Take(min).ToList();
            }

            // 4. Special Summon / Banish Cost for BLS:
            if (hint == HINTMSG_REMOVE)
            {
                // If selecting from GY for BLS summon cost (1 LIGHT + 1 DARK)
                if (cards.All(c => c.Location == CardLocation.Grave))
                {
                    var light = cards.FirstOrDefault(c => IsLight(c.Id));
                    var dark = cards.FirstOrDefault(c => IsDark(c.Id));
                    if (light != null && dark != null)
                    {
                        return new List<ClientCard> { light, dark };
                    }
                }

                // If BLS effect banishing a target on field:
                var fieldThreats = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.MonsterZone && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).ToList();
                if (fieldThreats.Count >= min)
                    return fieldThreats.Take(min).ToList();

                // If Kycoo banishing from opponent GY:
                var gyMonsters = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.Grave)
                    .OrderByDescending(c => c.Attack).ToList();
                if (gyMonsters.Count >= min)
                    return gyMonsters.Take(Math.Min(max, gyMonsters.Count)).ToList();
            }

            // 5. Special Summon (Premature Burial / Call of the Haunted):
            if (hint == HINTMSG_SPSUMMON)
            {
                var reviveTargets = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.BlackLusterSoldierEnvoyOfTheBeginning)) return 1;
                    if (c.IsCode(CardId.BladeKnight)) return 2;
                    if (c.IsCode(CardId.DdWarriorLady)) return 3;
                    if (c.IsCode(CardId.KycooTheGhostDestroyer)) return 4;
                    if (c.IsCode(CardId.BreakerTheMagicalWarrior)) return 5;
                    return 10;
                }).ToList();

                if (reviveTargets.Count >= min)
                    return reviveTargets.Take(min).ToList();
            }

            // 6. Target Destruction (Exiled Force / Smashing Ground / Sakuretsu):
            if (hint == HINTMSG_DESTROY)
            {
                var threats = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.MonsterZone && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).ToList();
                if (threats.Count >= min)
                    return threats.Take(min).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 4;
                long optIndex = options[i] & 0xf;
                if (cardId == 0) cardId = options[i] >> 20;

                // Don Zaloog: Option 0 is Discard 1 random card from opponent hand (ALWAYS preferred in GOAT!)
                if (cardId == CardId.DonZaloog || (LastChainCard != null && LastChainCard.IsCode(CardId.DonZaloog)))
                {
                    if (Enemy.Hand.Count > 0 && optIndex == 0) return i;
                    if (Enemy.Hand.Count == 0 && optIndex == 1) return i;
                }

                // BLS: Option 0 is Banish; Option 1 is Double Attack
                if (cardId == CardId.BlackLusterSoldierEnvoyOfTheBeginning ||
                    (LastChainCard != null && LastChainCard.IsCode(CardId.BlackLusterSoldierEnvoyOfTheBeginning)))
                {
                    bool isLocked = Bot.HasInSpellZone(CardId.GravityBind) || Bot.HasInSpellZone(CardId.LevelLimitAreaB) ||
                                   Enemy.HasInSpellZone(CardId.GravityBind) || Enemy.HasInSpellZone(CardId.LevelLimitAreaB);
                    if (isLocked && optIndex == 0) return i; // Cannot attack, so banish!
                    if (Duel.Phase == DuelPhase.Main1 && Enemy.GetMonsters().Any(m => m != null && m.Attack >= 3000) && optIndex == 0) return i;
                    if ((Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Damage) && optIndex == 1) return i;
                }
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.BlackLusterSoldierEnvoyOfTheBeginning || cardId == CardId.BladeKnight ||
                cardId == CardId.KycooTheGhostDestroyer || cardId == CardId.BreakerTheMagicalWarrior ||
                cardId == CardId.ZombyraTheDark || cardId == CardId.NinjaGrandmasterSasuke)
            {
                if (positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
            }

            if (cardId == CardId.MysticSwordsmanLv2 || cardId == CardId.DdWarriorLady)
            {
                if (Enemy.GetMonsterCount() > 0 && positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        // ═══════════════════════════════════════════════════════════════
        //  Strategic Decision Logic
        // ═══════════════════════════════════════════════════════════════

        private bool IsLight(int id)
        {
            return id == CardId.BladeKnight || id == CardId.DdWarriorLady ||
                   id == CardId.NinjaGrandmasterSasuke || id == CardId.BlackLusterSoldierEnvoyOfTheBeginning;
        }

        private bool IsDark(int id)
        {
            return id == CardId.ZombyraTheDark || id == CardId.KycooTheGhostDestroyer ||
                   id == CardId.DonZaloog || id == CardId.BreakerTheMagicalWarrior;
        }

        private bool SolemnJudgmentEffect()
        {
            if (Util.GetLastChainCard() == null || Util.GetLastChainCard().Controller == 0) return false;
            var last = Util.GetLastChainCard();
            return last.Id == CardId.HeavyStorm || last.Id == CardId.SnatchSteal ||
                   last.Id == CardId.BlackLusterSoldierEnvoyOfTheBeginning || last.Id == CardId.TorrentialTribute;
        }

        private bool TrapDustshootEffect()
        {
            return Enemy.Hand.Count >= 4 && Duel.Phase == DuelPhase.Standby;
        }

        private bool DelinquentDuoEffect()
        {
            return Bot.LifePoints > 1000 && Enemy.Hand.Count >= 2;
        }

        private bool HeavyStormEffect()
        {
            return Enemy.GetSpellCount() >= 2 || (Enemy.GetSpellCount() >= 1 && Bot.GetSpellCount() == 0);
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
            return Bot.GetRemainingCount(CardId.BladeKnight, 1) > 0 ||
                   Bot.GetRemainingCount(CardId.DdWarriorLady, 1) > 0 ||
                   Bot.GetRemainingCount(CardId.DonZaloog, 1) > 0 ||
                   Bot.GetRemainingCount(CardId.NinjaGrandmasterSasuke, 1) > 0 ||
                   Bot.GetRemainingCount(CardId.MysticSwordsmanLv2, 1) > 0 ||
                   Bot.GetRemainingCount(CardId.ExiledForce, 1) > 0;
        }

        private bool TribeInfectingVirusEffect()
        {
            if (Enemy.GetMonsterCount() == 0 || Bot.Hand.Count <= 1) return false;
            var faceUp = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c)).ToList();
            if (faceUp.Count == 0) return false;

            var bestRace = faceUp.GroupBy(c => c.Race).OrderByDescending(g => g.Count()).First();
            AI.SelectRace((CardRace)bestRace.Key);
            return true;
        }

        private bool BookOfMoonEffect()
        {
            // Flip down opponent attacker in battle phase
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep))
            {
                ClientCard attacker = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack >= 1500)
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (attacker != null)
                {
                    AI.SelectCard(attacker);
                    return true;
                }
            }

            // Flip down face-up enemy monster for Mystic Swordsman LV2
            if (Duel.Player == 0 && Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.MysticSwordsmanLv2)))
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsTargetImmune(c));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return false;
        }

        private bool BlsSummon()
        {
            // Requires 1 LIGHT and 1 DARK in GY
            bool hasLight = Bot.Graveyard.Any(c => c != null && c.IsMonster() && IsLight(c.Id));
            bool hasDark = Bot.Graveyard.Any(c => c != null && c.IsMonster() && IsDark(c.Id));
            return hasLight && hasDark;
        }

        private bool BlsEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || !Card.IsFaceup()) return false;

            // Battle Phase: double attack!
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Damage)
            {
                AI.SelectOption(1);
                return true;
            }

            // Main Phase: Banish if attack is locked or opponent has big boss / face-down
            bool isLocked = Bot.HasInSpellZone(CardId.GravityBind) || Bot.HasInSpellZone(CardId.LevelLimitAreaB) ||
                           Enemy.HasInSpellZone(CardId.GravityBind) || Enemy.HasInSpellZone(CardId.LevelLimitAreaB);
            if (isLocked)
            {
                ClientCard target = Enemy.GetMonsters().Where(c => c != null && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectOption(0);
                    AI.SelectCard(target);
                    return true;
                }
            }

            ClientCard threat = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.Attack >= 3000 && !IsTargetImmune(c));
            if (threat != null)
            {
                AI.SelectOption(0);
                AI.SelectCard(threat);
                return true;
            }

            ClientCard facedown = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFacedown() && !IsTargetImmune(c));
            if (facedown != null && Enemy.GetMonsterCount() <= 2)
            {
                AI.SelectOption(0);
                AI.SelectCard(facedown);
                return true;
            }

            return false;
        }

        private bool PrematureBurialEffect()
        {
            if (Bot.LifePoints <= 800) return false;
            return Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.IsCanRevive() && c.Attack >= 1500);
        }

        private bool CallOfTheHauntedEffect()
        {
            return Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.IsCanRevive() && c.Attack >= 1500);
        }

        private bool BreakerEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && Card.Attack == 1900)
            {
                return DefaultMysticalSpaceTyphoon();
            }
            return false;
        }

        private bool DdWarriorLadyEffect()
        {
            // Banish D.D. Warrior Lady and target ONLY if opponent monster has > 1500 ATK or is dangerous
            if (Card.Location == CardLocation.MonsterZone && Bot.BattlingMonster == Card)
            {
                ClientCard opp = Enemy.BattlingMonster;
                if (opp != null && (opp.Attack > 1500 || IsAceCard(opp) || opp.HasType(CardType.Flip)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool DonZaloogEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (Enemy.Hand.Count > 0) AI.SelectOption(0); // Discard hand
            else AI.SelectOption(1); // Mill deck
            return true;
        }

        private bool KycooEffect()
        {
            return Enemy.Graveyard.Any(c => c != null && c.IsMonster());
        }

        private bool MysticSwordsmanLv2Summon()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFacedown());
        }

        private bool SasukeSummon()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsDefense());
        }

        private bool ZombyraSummon()
        {
            return Enemy.GetMonsterCount() > 0;
        }

        private bool MirrorForceEffect()
        {
            if (Duel.Player != 1) return false;
            if (Enemy.BattlingMonster != null && Enemy.BattlingMonster.Attack >= 1500) return true;
            return Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsAttack()) >= 2;
        }

        private bool SakuretsuArmorEffect()
        {
            if (Duel.Player != 1 || Enemy.BattlingMonster == null) return false;
            return Enemy.BattlingMonster.Attack >= 1500;
        }

        private bool TorrentialTributeEffect()
        {
            if (Bot.HasInMonstersZone(CardId.BlackLusterSoldierEnvoyOfTheBeginning)) return false;
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
            return DefaultMonsterRepos();
        }
    }
}
