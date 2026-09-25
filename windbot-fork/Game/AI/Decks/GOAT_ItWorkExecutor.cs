// ============================================================================
// CARD AUDIT — GOAT_ItWork (Solar Flare Stall / Burn)
// ============================================================================
// | Card Name                   | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |-----------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Solar Flare Dragon          | Monster L4   | No   | No    | None    | Cannot attack if another Pyro; 500 burn in EP | Summon to board; Burn 500 in EP               | Overextending into Torrential Tribute       |
// | Stealth Bird                | Monster L3   | Yes  | No    | None    | Flip: 1000 burn; Ignition: flip face-down     | Flip summon for 1000; flip down in MP1/MP2    | Face-down with already flipped this turn   |
// | Raging Flame Sprite         | Monster L3   | No   | No    | None    | Attack directly; +1000 ATK on direct damage   | Normal summon when stall is active            | Opponent has big attacker and no stall      |
// | UFO Turtle                  | Monster L4   | No   | No    | None    | Battle destroyed: SS FIRE <=1500 ATK from deck| Search Solar Flare Dragon / Raging Flame      | No targets left in deck                     |
// | Morphing Jar                | Monster L2   | No   | No    | None    | FLIP: Both players discard hand, draw 5 cards | Set face-down; flip when hand is small (<=2)  | Hand has 4+ good cards                      |
// | Needle Worm                 | Monster L2   | No   | No    | None    | FLIP: Send top 5 cards of opponent deck to GY | Set face-down for mill disruption             | Opponent has strong GY triggers             |
// | Level Limit - Area B        | Spell Cont   | No   | No    | None    | Level 4+ monsters changed to Defense Position | Setup stall lock against beatsticks           | Already active on field                     |
// | Gravity Bind                | Trap Cont    | No   | No    | None    | Level 4+ monsters cannot attack               | Activate to stop attacks                      | Already active on field                     |
// | Chain Energy                | Spell Cont   | No   | No    | 500 LP  | Players pay 500 LP to summon/set/play cards   | Play when our LP is high, burns opp plays     | Our LP <= 1500 or already active            |
// | Swords of Revealing Light   | Spell Normal | No   | No    | None    | Opponent cannot attack for 3 turns; flip face | Play to stall and reveal face-downs           | Already under stall lock and safe           |
// | Ojama Trio                  | Trap Normal  | No   | No    | None    | SS 3 Ojama Tokens (0/1000) to opp field; 300  | Opponent has <= 2 monsters, clog board & burn | Opponent has 3+ monsters (cannot summon 3)  |
// | Just Desserts               | Trap Normal  | No   | No    | None    | Inflict 500 damage per opponent monster       | Opp has 3+ monsters (or 1+ for lethal)        | Opp has 0 monsters                          |
// | Secret Barrel               | Trap Normal  | No   | No    | None    | Inflict 200 damage per card in opp hand/field | Opp cards >= 6 (or for lethal burn)          | Opp has small hand and board                |
// | Ceasefire                   | Trap Normal  | No   | No    | None    | Flips face-down monsters; 500 burn per effect | 3+ effect monsters or facedowns on field      | Less than 2 effect monsters                 |
// | Judgment of Anubis          | Trap Counter | No   | No    | Discard | Negate S/T destroy spell -> pop 1 mon & burn  | Opp activates Heavy Storm / MST / Trunade     | No cards in hand to discard                 |
// | Curse of Anubis             | Trap Normal  | No   | No    | None    | Change all effect monsters to DEF, DEF becomes 0| Opponent declares battle, stops attacks     | Not in battle phase or our turn             |
// | Magic Cylinder              | Trap Normal  | No   | No    | None    | Negate attack and inflict damage equal to ATK | High ATK attack or lethal burn                | Opponent attacking with negligible ATK       |
// | Ring of Destruction         | Trap Normal  | No   | No    | None    | Destroy face-up monster; both take damage     | Remove dangerous monster or lethal burn       | Burn would kill us (ATK >= our LP)          |
// | Scapegoat                   | Spell Quick  | No   | No    | None    | SS 4 Sheep Tokens in DEF                      | Opponent attacks or end of opponent's turn    | Our turn (blocks our normal summons)        |
// | Creature Swap               | Spell Normal | No   | No    | None    | Each player swaps 1 monster (no targeting)    | Give Token/Turtle, steal big enemy monster    | We have only Solar Flare / Stealth Bird     |
// | Brain Control               | Spell Normal | No   | No    | 800 LP  | Take control of 1 face-up monster until EP    | Take enemy beater to attack or clear board    | Opponent has no face-up monsters            |
// | Book of Moon                | Spell Quick  | No   | No    | None    | Change 1 face-up monster to face-down DEF     | Stop enemy attacker / reuse flip effect       | No valid targets                            |
// | Heavy Storm                 | Spell Normal | No   | No    | None    | Destroy all Spells and Traps on field         | Opp has >= 2 S/T and we have no critical stall| We rely on Gravity Bind/Level Limit to live |
// | Giant Trunade               | Spell Normal | No   | No    | None    | Return all Spells and Traps on field to hand  | Clear backrow for lethal push or reuse locks  | No advantage gained                         |
// | Pot of Greed                | Spell Normal | No   | No    | None    | Draw 2 cards                                  | Always activate when available                | Never                                       |
// | Upstart Goblin              | Spell Normal | No   | No    | None    | Draw 1 card, opponent gains 1000 LP           | Draw deck thinner if looking for combo piece  | Opponent LP close to lethal threshold       |
// | Delinquent Duo              | Spell Normal | No   | No    | 1000 LP | Opponent discards 1 random and 1 choice card  | Early game hand rip (Opp hand >= 2)           | Bot LP <= 1000                              |
// | Mystical Space Typhoon      | Spell Quick  | No   | No    | None    | Destroy 1 Spell/Trap on field                 | Remove dangerous continuous/field/backrow     | No targets                                  |
// | Dust Tornado                | Trap Normal  | No   | No    | None    | Destroy 1 Spell/Trap on field; set 1 S/T      | End Phase removal of opponent backrow         | No targets                                  |
// | Torrential Tribute          | Trap Normal  | No   | No    | None    | Destroy all monsters on field on summon       | Opponent summons boss / swarms board          | We have established lock and opp has weak mon|
// | Mirror Force                | Trap Normal  | No   | No    | None    | Destroy all Attack Position opponent monsters | Opponent attacks with big board / beater      | Weak single attack when we have stall       |
// | Bottomless Trap Hole        | Trap Normal  | No   | No    | None    | Destroy and banish summoned monster >=1500 ATK| High ATK summon disruption                    | Weak monsters                               |
// | Trap Dustshoot              | Trap Normal  | No   | No    | None    | Standby: Reveal opp hand, return 1 mon to deck| Opponent has >= 4 cards in hand in Standby    | Opp hand < 4 or not Standby Phase           |
// | Compulsory Evac Device      | Trap Normal  | No   | No    | None    | Return 1 monster on field to hand             | Bounce boss / extra deck / attacker           | Target is token or useless                  |
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
    [Deck("GOAT_ItWork", "GOAT_ItWork")]
    public class GOAT_ItWorkExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Monsters
            public const int MorphingJar = 33508719;
            public const int SolarFlareDragon = 45985838;
            public const int UfoTurtle = 60806437;
            public const int RagingFlameSprite = 90810762;
            public const int StealthBird = 3510565;
            public const int NeedleWorm = 81843628;

            // Spells
            public const int PotOfGreed = 55144522;
            public const int UpstartGoblin = 70368879;
            public const int DelinquentDuo = 44763025;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int HeavyStorm = 19613556;
            public const int GiantTrunade = 42703248;
            public const int SwordsOfRevealingLight = 72302403;
            public const int LevelLimitAreaB = 3136426;
            public const int ChainEnergy = 79323590;
            public const int BrainControl = 87910978;
            public const int CreatureSwap = 31036355;
            public const int BookOfMoon = 14087893;
            public const int Scapegoat = 73915051;

            // Traps
            public const int JudgmentOfAnubis = 55256016;
            public const int TrapDustshoot = 64697231;
            public const int BottomlessTrapHole = 29401950;
            public const int RingOfDestruction = 83555666;
            public const int OjamaTrio = 29843091;
            public const int MagicCylinder = 62279055;
            public const int JustDesserts = 24068492;
            public const int SecretBarrel = 27053506;
            public const int CurseOfAnubis = 66742250;
            public const int CompulsoryEvacuationDevice = 94192409;
            public const int DustTornado = 60082869;
            public const int Ceasefire = 36468556;
            public const int TorrentialTribute = 53582587;
            public const int MirrorForce = 44095762;
            public const int GravityBind = 85742772;

            // Tokens
            public const int OjamaToken = 29843092;
            public const int SheepToken = 73915052;
        }

        private static readonly int[] ProtectAces =
        {
            CardId.SolarFlareDragon,
            CardId.StealthBird
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && ProtectAces.Contains(card.Id);
        }

        public GOAT_ItWorkExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Counter Traps & Hand Traps ──
            AddExecutor(ExecutorType.Activate, CardId.JudgmentOfAnubis, JudgmentOfAnubisEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrapDustshoot, TrapDustshootEffect);

            // ── 2. Draw & Hand Disruption ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.UpstartGoblin, UpstartGoblinEffect);
            AddExecutor(ExecutorType.Activate, CardId.DelinquentDuo, DelinquentDuoEffect);

            // ── 3. Backrow Clearance ──
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.DustTornado, DustTornadoEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.GiantTrunade, GiantTrunadeEffect);

            // ── 4. Stall Foundation ──
            AddExecutor(ExecutorType.Activate, CardId.SwordsOfRevealingLight);
            AddExecutor(ExecutorType.Activate, CardId.LevelLimitAreaB, LevelLimitAreaBEffect);
            AddExecutor(ExecutorType.Activate, CardId.GravityBind, GravityBindEffect);
            AddExecutor(ExecutorType.Activate, CardId.ChainEnergy, ChainEnergyEffect);

            // ── 5. Field Manipulation & Control Spells ──
            AddExecutor(ExecutorType.Activate, CardId.BrainControl, BrainControlEffect);
            AddExecutor(ExecutorType.Activate, CardId.CreatureSwap, CreatureSwapEffect);
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonEffect);

            // ── 6. Monster Effects & Flip Loops ──
            // Stealth Bird: Flip effect triggers burn; Ignition effect flips itself back face-down
            AddExecutor(ExecutorType.Activate, CardId.StealthBird, StealthBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.MorphingJar);
            AddExecutor(ExecutorType.Activate, CardId.NeedleWorm);
            AddExecutor(ExecutorType.Activate, CardId.RagingFlameSprite);
            AddExecutor(ExecutorType.Activate, CardId.SolarFlareDragon);
            AddExecutor(ExecutorType.Activate, CardId.UfoTurtle, UfoTurtleEffect);

            // ── 7. Normal Summons & Sets ──
            // Set Stealth Bird first if we have it in hand
            AddExecutor(ExecutorType.MonsterSet, CardId.StealthBird);
            AddExecutor(ExecutorType.MonsterSet, CardId.MorphingJar);
            AddExecutor(ExecutorType.MonsterSet, CardId.NeedleWorm);
            AddExecutor(ExecutorType.MonsterSet, CardId.UfoTurtle);

            // Summons
            AddExecutor(ExecutorType.Summon, CardId.SolarFlareDragon, SolarFlareDragonSummon);
            AddExecutor(ExecutorType.Summon, CardId.RagingFlameSprite, RagingFlameSpriteSummon);
            AddExecutor(ExecutorType.Summon, CardId.UfoTurtle);

            // ── 8. Reactive & Disruption Traps ──
            AddExecutor(ExecutorType.Activate, CardId.OjamaTrio, OjamaTrioEffect);
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, DefaultTorrentialTribute);
            AddExecutor(ExecutorType.Activate, CardId.MagicCylinder, MagicCylinderEffect);
            AddExecutor(ExecutorType.Activate, CardId.RingOfDestruction, RingOfDestructionEffect);
            AddExecutor(ExecutorType.Activate, CardId.BottomlessTrapHole);
            AddExecutor(ExecutorType.Activate, CardId.CompulsoryEvacuationDevice, DefaultCompulsoryEvacuationDevice);
            AddExecutor(ExecutorType.Activate, CardId.CurseOfAnubis, CurseOfAnubisEffect);
            AddExecutor(ExecutorType.Activate, CardId.Scapegoat, ScapegoatEffect);

            // ── 9. Direct Burn Traps ──
            AddExecutor(ExecutorType.Activate, CardId.Ceasefire, CeasefireEffect);
            AddExecutor(ExecutorType.Activate, CardId.JustDesserts, JustDessertsEffect);
            AddExecutor(ExecutorType.Activate, CardId.SecretBarrel, SecretBarrelEffect);

            // ── 10. Spell Sets & Repositions ──
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
            const long HINTMSG_SPSUMMON = 509;
            const long HINTMSG_CONTROL = 520;

            // 1. Tribute / Release selection (Creature Swap / Tribute): Protect Solar Flare & Stealth Bird
            if (hint == HINTMSG_RELEASE)
            {
                var candidates = cards.Where(c => c.Controller == 0).OrderBy(c =>
                {
                    if (c.IsCode(CardId.SheepToken) || c.IsCode(CardId.OjamaToken)) return 1;
                    if (c.IsCode(CardId.UfoTurtle)) return 2;
                    if (c.IsCode(CardId.NeedleWorm) || c.IsCode(CardId.MorphingJar)) return 3;
                    if (IsAceCard(c)) return 99;
                    return 10;
                }).ToList();

                if (candidates.Count >= min)
                    return candidates.Take(min).ToList();
            }

            // 2. Discard Cost (Judgment of Anubis): Discard expendable cards
            if (hint == HINTMSG_DISCARD)
            {
                var discards = cards.Where(c => c.Controller == 0).OrderBy(c =>
                {
                    if (c.IsCode(CardId.NeedleWorm)) return 1;
                    if (c.IsCode(CardId.UfoTurtle)) return 2;
                    if (c.IsCode(CardId.UpstartGoblin)) return 3;
                    if (c.IsCode(CardId.ChainEnergy) && Bot.HasInSpellZone(CardId.ChainEnergy)) return 4;
                    if (IsAceCard(c)) return 90;
                    return 10;
                }).ToList();

                if (discards.Count >= min)
                    return discards.Take(min).ToList();
            }

            // 3. Special Summon (UFO Turtle): Solar Flare Dragon > Raging Flame Sprite > UFO Turtle
            if (hint == HINTMSG_SPSUMMON)
            {
                var targets = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.SolarFlareDragon)) return 1;
                    if (c.IsCode(CardId.RagingFlameSprite) && HasStallLock()) return 2;
                    if (c.IsCode(CardId.UfoTurtle)) return 3;
                    return 10;
                }).ToList();

                if (targets.Count >= min)
                    return targets.Take(min).ToList();
            }

            // 4. Creature Swap Control selection: Give token/turtle, take strongest enemy monster
            if (hint == HINTMSG_CONTROL)
            {
                var ownTokens = cards.Where(c => c.Controller == 0).OrderBy(c =>
                {
                    if (c.IsCode(CardId.SheepToken)) return 1;
                    if (c.IsCode(CardId.UfoTurtle)) return 2;
                    if (c.IsCode(CardId.NeedleWorm)) return 3;
                    return 10;
                }).FirstOrDefault();

                if (ownTokens != null) return new List<ClientCard> { ownTokens };
            }

            // 5. Target destruction (Judgment of Anubis): Pop highest ATK enemy monster
            if (hint == HINTMSG_DESTROY)
            {
                var enemyMonsters = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.MonsterZone)
                    .OrderByDescending(c => c.Attack).ToList();
                if (enemyMonsters.Count >= min)
                    return enemyMonsters.Take(min).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.StealthBird || cardId == CardId.MorphingJar || cardId == CardId.NeedleWorm)
            {
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
            }

            if (cardId == CardId.RagingFlameSprite)
            {
                if (positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
            }

            if (cardId == CardId.SolarFlareDragon)
            {
                // If protected by another Pyro, stay in Attack; otherwise Defense if vulnerable
                bool hasAnotherPyro = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasRace(CardRace.Pyro) && m.Id != cardId);
                if (hasAnotherPyro && positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
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

        private bool JudgmentOfAnubisEffect()
        {
            if (Bot.Hand.Count == 0) return false;
            var lastCard = Util.GetLastChainCard();
            if (lastCard != null && lastCard.Controller == 1)
            {
                if (lastCard.IsCode(CardId.HeavyStorm) || lastCard.IsCode(CardId.MysticalSpaceTyphoon) ||
                    lastCard.IsCode(CardId.GiantTrunade) || lastCard.IsCode(CardId.DustTornado))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TrapDustshootEffect()
        {
            return Enemy.Hand.Count >= 4 && Duel.Phase == DuelPhase.Standby;
        }

        private bool UpstartGoblinEffect()
        {
            // Do not heal opponent if they are already within lethal burn range
            int estimatedBurn = (Bot.HasInHand(CardId.JustDesserts) ? Enemy.GetMonsterCount() * 500 : 0) +
                                (Bot.HasInHand(CardId.SecretBarrel) ? (Enemy.Hand.Count + Enemy.GetMonsterCount() + Enemy.GetSpellCount()) * 200 : 0);
            if (estimatedBurn >= Enemy.LifePoints) return false;
            return true;
        }

        private bool DelinquentDuoEffect()
        {
            return Bot.LifePoints > 1000 && Enemy.Hand.Count >= 2;
        }

        private bool HeavyStormEffect()
        {
            // Never destroy our own active stall locks if they are shielding us from lethal/attackers
            if (HasStallLock() && Enemy.GetMonsterCount() > 0) return false;
            return Enemy.GetSpellCount() > Bot.GetSpellCount() && Enemy.GetSpellCount() >= 2;
        }

        private bool GiantTrunadeEffect()
        {
            // Clear enemy backrow when pushing with Raging Flame Sprite or for lethal burn
            if (Enemy.GetSpellCount() >= 2) return true;
            if (Enemy.GetSpellCount() >= 1 && Enemy.LifePoints <= 2000) return true;
            return false;
        }

        private bool LevelLimitAreaBEffect()
        {
            return !Bot.HasInSpellZone(CardId.LevelLimitAreaB);
        }

        private bool GravityBindEffect()
        {
            return !Bot.HasInSpellZone(CardId.GravityBind);
        }

        private bool ChainEnergyEffect()
        {
            if (Bot.LifePoints <= 1500) return false;
            return !Bot.HasInSpellZone(CardId.ChainEnergy);
        }

        private bool BrainControlEffect()
        {
            if (Bot.LifePoints <= 800) return false;
            ClientCard best = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                .OrderByDescending(c => c.Attack).FirstOrDefault();
            if (best != null)
            {
                AI.SelectCard(best);
                return true;
            }
            return false;
        }

        private bool CreatureSwapEffect()
        {
            if (Enemy.GetMonsterCount() == 0) return false;
            var tokenOrFodder = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                (c.IsCode(CardId.SheepToken) || c.IsCode(CardId.OjamaToken) || c.IsCode(CardId.UfoTurtle)));
            if (tokenOrFodder == null) return false;

            var enemyTarget = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Attack >= 1500).FirstOrDefault();
            return enemyTarget != null;
        }

        private bool BookOfMoonEffect()
        {
            // 1. Flip down opponent attacker in battle phase
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep))
            {
                ClientCard attacker = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack >= 1400)
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (attacker != null)
                {
                    AI.SelectCard(attacker);
                    return true;
                }
            }

            // 2. Flip down our Flip monsters to reuse (Stealth Bird, Morphing Jar, Needle Worm)
            ClientCard flipTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                (c.IsCode(CardId.MorphingJar) || c.IsCode(CardId.NeedleWorm)));
            if (flipTarget != null)
            {
                AI.SelectCard(flipTarget);
                return true;
            }

            return false;
        }

        private bool StealthBirdEffect()
        {
            // Ignition effect: If face-up in Main Phase, flip back to face-down Defense Position!
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() &&
                (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                return true;
            }
            // Trigger effect on flip summon
            return Card.IsFaceup();
        }

        private bool UfoTurtleEffect()
        {
            return true;
        }

        private bool SolarFlareDragonSummon()
        {
            // Great summon if we already have a Pyro monster (creates attack lock) or under stall
            bool hasPyro = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Pyro));
            if (hasPyro || HasStallLock()) return true;
            // Otherwise summon if opponent has no strong monsters
            return Enemy.GetMonsters().All(c => c == null || c.Attack < 1500);
        }

        private bool RagingFlameSpriteSummon()
        {
            // Only summon if stall is active or opponent controls no monsters
            return HasStallLock() || Enemy.GetMonsterCount() == 0;
        }

        private bool OjamaTrioEffect()
        {
            int emptyZones = 5 - Enemy.GetMonsterCount();
            if (emptyZones < 3) return false;

            // Activate if opponent has few monsters or we have burn traps ready
            bool hasBurn = Bot.HasInHand(CardId.JustDesserts) || Bot.HasInHand(CardId.SecretBarrel) ||
                           Bot.GetSpells().Any(c => c != null && (c.IsCode(CardId.JustDesserts) || c.IsCode(CardId.SecretBarrel)));
            return hasBurn || Enemy.GetMonsterCount() <= 1;
        }

        private bool MagicCylinderEffect()
        {
            if (Duel.Player != 1 || Enemy.BattlingMonster == null) return false;
            // Lethal burn
            if (Enemy.BattlingMonster.Attack >= Enemy.LifePoints) return true;
            // Significant attacker
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

        private bool CurseOfAnubisEffect()
        {
            return Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep);
        }

        private bool ScapegoatEffect()
        {
            if (Duel.Player == 0) return false; // Opponent turn only to preserve Normal Summon
            return DefaultScapegoat();
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

        private bool DustTornadoEffect()
        {
            return DefaultMysticalSpaceTyphoon();
        }

        private bool MirrorForceEffect()
        {
            if (Duel.Player != 1) return false;
            if (Enemy.BattlingMonster != null && Enemy.BattlingMonster.Attack >= 1200) return true;
            return Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsAttack()) >= 2;
        }

        private bool SetTrapCondition()
        {
            if (Card.HasType(CardType.Field)) return false;
            return Card.HasType(CardType.Trap) || (Card.HasType(CardType.Spell) && Card.HasType(CardType.QuickPlay));
        }

        private bool MonsterRepos()
        {
            bool isLocked = HasStallLock();

            // Raging Flame Sprite: Always Attack (attacks directly)
            if (Card.IsCode(CardId.RagingFlameSprite) && Card.IsFaceup())
            {
                if (Card.IsDefense()) return true;
                return false;
            }

            // Stealth Bird: Flip summon if face-down to inflict 1000 burn
            if (Card.IsCode(CardId.StealthBird) && Card.IsFacedown())
            {
                return true;
            }

            // If locked by Gravity Bind/Level Limit: switch Level 4+ to DEF
            if (isLocked && Card.Level >= 4 && Card.IsAttack())
            {
                return true;
            }

            return DefaultMonsterRepos();
        }
    }
}
