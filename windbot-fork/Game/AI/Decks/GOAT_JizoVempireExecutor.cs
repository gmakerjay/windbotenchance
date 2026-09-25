// ============================================================================
// CARD AUDIT — GOAT_JizoVempire (Zombie Control & Jinzo)
// ============================================================================
// | Card Name                   | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |-----------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Jinzo                       | Monster L6   | No   | No    | 1 Trib  | Traps cannot be activated, all traps negated  | Tribute summon when opp has backrow or pushing| We rely on our set traps and opp has none   |
// | Ryu Kokki                   | Monster L6   | No   | No    | 1 Trib  | 2400 ATK; destroys Warrior/Spellcaster battling| SS via Pyramid Turtle/Book of Life; tribute  | Tributing our own bosses                    |
// | Vampire Lord                | Monster L5   | Yes  | No    | 1 Trib  | Battle dmg: opp mills type; revives in Standby| SS via Turtle/Book of Life; damage hand rip   | Tributing our own bosses                    |
// | Giant Orc                   | Monster L4   | No   | No    | None    | 2200 ATK beatstick; turns DEF after attack    | Normal summon to attack over strong monsters  | Opponent has lethal attacker next turn      |
// | Breaker the Magical Warrior | Monster L4   | Yes  | No    | 1 Cntr  | 1600+300 ATK; remove counter to pop 1 S/T     | Pop opponent threatening backrow              | Opponent has 0 Spells/Traps                 |
// | Kycoo the Ghost Destroyer   | Monster L4   | No   | No    | None    | 1800 ATK; battle dmg: banish 2 opp GY monsters| Attack to deal dmg and banish GY threats      | Opponent has 0 monsters in GY               |
// | Regenerating Mummy          | Monster L4   | No   | No    | None    | 1800 ATK Zombie; returns to hand if discarded | Normal summon beatstick / discard fodder      | Never                                       |
// | Pyramid Turtle              | Monster L4   | No   | No    | None    | Battle destroyed: SS Zombie <=2000 DEF from DK| Set or ram to tutor Ryu Kokki / Vampire Lord  | Deck has 0 valid Zombies                    |
// | Tribe-Infecting Virus       | Monster L4   | No   | No    | Discard | Declare race: destroy all face-up mon of race | Clear opponent swarm of same race             | Discarding critical win condition           |
// | Sangan                      | Monster L3   | No   | No    | None    | Sent to GY: Add monster <=1500 ATK from deck  | Set or tribute to search Virus / Faith / Reaper| No targets left in deck                     |
// | Spirit Reaper               | Monster L3   | No   | No    | None    | Cannot be destroyed by battle; direct: rip 1  | Set as wall; attack direct to rip card        | Opponent has targeting cards active         |
// | Night Assailant             | Monster L3   | No   | No    | None    | FLIP: pop 1 opp mon; Discard: add Flip from GY| Set for removal; discard for vortex/virus     | No flip monsters in GY                      |
// | Magician of Faith           | Monster L1   | No   | No    | None    | FLIP: Target 1 Spell in GY; add to hand       | Set face-down; recur Pot/Graceful/Snatch Steal| Graveyard has 0 Spells                      |
// | Morphing Jar                | Monster L2   | No   | No    | None    | FLIP: Both players discard hand, draw 5 cards | Set when hand is small (<=2 cards)            | Hand is full of good cards                  |
// | Book of Life                | Spell Normal | No   | No    | None    | SS Zombie from our GY; banish 1 opp GY mon    | Revive Ryu Kokki/Vampire Lord & banish opp card| No Zombie in our GY or no card in opp GY    |
// | Lightning Vortex            | Spell Normal | No   | No    | Discard | Destroy all face-up monsters opponent controls | Opp has 2+ face-up monsters or big threat     | Hand has 0 cards to discard                 |
// | Pot of Greed                | Spell Normal | No   | No    | None    | Draw 2 cards                                  | Always activate                               | Never                                       |
// | Graceful Charity            | Spell Normal | No   | No    | Discard2| Draw 3 cards, then discard 2 cards            | Always activate; pitch Zombies / Night Assail | Hand is empty                               |
// | Snatch Steal                | Spell Equip  | No   | No    | None    | Take control of 1 opponent face-up monster    | Steal biggest enemy monster for attack/tribute | Opponent has 0 face-up monsters             |
// | Mystical Space Typhoon      | Spell Quick  | No   | No    | None    | Destroy 1 Spell/Trap on field                 | Snipe backrow / floodgate                     | No targets                                  |
// | Heavy Storm                 | Spell Normal | No   | No    | None    | Destroy all Spells and Traps on field         | Opp has >= 2 backrow                          | We control face-up Snatch Steal or Jinzo    |
// | Torrential Tribute          | Trap Normal  | No   | No    | None    | Destroy all monsters on field on summon       | Opponent summons big boss or multiple monsters| Jinzo on field (negated) or our board better|
// | Mirror Force                | Trap Normal  | No   | No    | None    | Destroy all Attack Position opponent monsters | Opponent attacks with strong board            | Jinzo on field (negated)                    |
// | Raigeki Break               | Trap Normal  | No   | No    | Discard | Discard 1, target 1 card on field; destroy it | Disrupt opponent key play / remove threat     | Jinzo on field or hand empty                |
// | Magic Drain                 | Trap Counter | No   | No    | None    | Opp must discard Spell or negate their Spell  | Negate opponent key Spell                     | Jinzo on field                              |
// | Seven Tools of the Bandit   | Trap Counter | No   | No    | 1000 LP | Pay 1000 LP, negate Trap and destroy it      | Negate opponent Mirror Force / Torrential / TT| Jinzo on field or LP <= 1000                |
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
    [Deck("GOAT_JizoVempire", "GOAT_JizoVempire")]
    public class GOAT_JizoVempireExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Monsters
            public const int Jinzo = 77585513;
            public const int RyuKokki = 57281778;
            public const int VampireLord = 53839837;
            public const int GiantOrc = 73698349;
            public const int BreakerTheMagicalWarrior = 71413901;
            public const int KycooTheGhostDestroyer = 88240808;
            public const int RegeneratingMummy = 70821187;
            public const int PyramidTurtle = 77044671;
            public const int TribeInfectingVirus = 33184167;
            public const int Sangan = 26202165;
            public const int SpiritReaper = 23205979;
            public const int NightAssailant = 16226786;
            public const int MagicianOfFaith = 31560081;
            public const int MorphingJar = 33508719;

            // Spells
            public const int BookOfLife = 2204140;
            public const int LightningVortex = 69162969;
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int SnatchSteal = 45986603;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int HeavyStorm = 19613556;

            // Traps
            public const int TorrentialTribute = 53582587;
            public const int MirrorForce = 44095762;
            public const int RaigekiBreak = 4178474;
            public const int MagicDrain = 59344077;
            public const int SevenToolsOfTheBandit = 3819470;
        }

        private static readonly int[] BossMonsters =
        {
            CardId.Jinzo,
            CardId.VampireLord,
            CardId.RyuKokki
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public GOAT_JizoVempireExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── 1. Counter Traps (Only if Jinzo is NOT face-up) ──
            AddExecutor(ExecutorType.Activate, CardId.SevenToolsOfTheBandit, SevenToolsEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagicDrain, MagicDrainEffect);

            // ── 2. Draw & Hand Optimization ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);

            // ── 3. Removal & Board Wipe ──
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.LightningVortex, LightningVortexEffect);
            AddExecutor(ExecutorType.Activate, CardId.RaigekiBreak, RaigekiBreakEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);

            // ── 4. Tribe-Infecting Virus Board Wipe ──
            AddExecutor(ExecutorType.Activate, CardId.TribeInfectingVirus, TribeInfectingVirusEffect);

            // ── 5. Zombie Reborn (Book of Life) ──
            AddExecutor(ExecutorType.Activate, CardId.BookOfLife, BookOfLifeEffect);

            // ── 6. Flip Monsters ──
            AddExecutor(ExecutorType.Activate, CardId.MagicianOfFaith, MagicianOfFaithEffect);
            AddExecutor(ExecutorType.Activate, CardId.MorphingJar);
            AddExecutor(ExecutorType.Activate, CardId.NightAssailant, NightAssailantEffect);

            // ── 7. Combat / Trigger Monster Effects ──
            AddExecutor(ExecutorType.Activate, CardId.PyramidTurtle, PyramidTurtleEffect);
            AddExecutor(ExecutorType.Activate, CardId.BreakerTheMagicalWarrior, BreakerEffect);
            AddExecutor(ExecutorType.Activate, CardId.VampireLord, VampireLordEffect);
            AddExecutor(ExecutorType.Activate, CardId.KycooTheGhostDestroyer, KycooEffect);
            AddExecutor(ExecutorType.Activate, CardId.Sangan);

            // ── 8. Tribute Summons (1 Tribute for Level 5/6) ──
            AddExecutor(ExecutorType.Summon, CardId.Jinzo, JinzoSummon);
            AddExecutor(ExecutorType.Summon, CardId.RyuKokki, RyuKokkiSummon);
            AddExecutor(ExecutorType.Summon, CardId.VampireLord, VampireLordSummon);

            // ── 9. Normal Summons (Level 4 Attackers) ──
            AddExecutor(ExecutorType.Summon, CardId.BreakerTheMagicalWarrior);
            AddExecutor(ExecutorType.Summon, CardId.KycooTheGhostDestroyer);
            AddExecutor(ExecutorType.Summon, CardId.GiantOrc);
            AddExecutor(ExecutorType.Summon, CardId.TribeInfectingVirus, TribeInfectingVirusSummon);
            AddExecutor(ExecutorType.Summon, CardId.RegeneratingMummy);

            // ── 10. Monster Sets (Defensive Walls / Floaters / Flips) ──
            AddExecutor(ExecutorType.MonsterSet, CardId.PyramidTurtle);
            AddExecutor(ExecutorType.MonsterSet, CardId.SpiritReaper);
            AddExecutor(ExecutorType.MonsterSet, CardId.Sangan);
            AddExecutor(ExecutorType.MonsterSet, CardId.NightAssailant);
            AddExecutor(ExecutorType.MonsterSet, CardId.MagicianOfFaith);
            AddExecutor(ExecutorType.MonsterSet, CardId.MorphingJar);

            // Fallback Summons
            AddExecutor(ExecutorType.Summon, CardId.PyramidTurtle);
            AddExecutor(ExecutorType.Summon, CardId.SpiritReaper);

            // ── 11. Reactive Traps ──
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, TorrentialTributeEffect);
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);

            // ── 12. Sets & Repositions ──
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

            // 1. Tribute Selection: Sangan (best) > Stolen Monster > Pyramid Turtle > Giant Orc
            if (hint == HINTMSG_RELEASE)
            {
                var tributes = cards.Where(c => c.Controller == 0).OrderBy(c =>
                {
                    if (c.IsCode(CardId.Sangan)) return 1;
                    if (c.EquipCards.Any(e => e.IsCode(CardId.SnatchSteal))) return 2;
                    if (c.IsCode(CardId.PyramidTurtle)) return 3;
                    if (c.IsCode(CardId.GiantOrc)) return 4;
                    if (c.IsCode(CardId.RegeneratingMummy)) return 5;
                    if (IsAceCard(c)) return 99;
                    return 10;
                }).ToList();

                if (tributes.Count >= min)
                    return tributes.Take(min).ToList();
            }

            // 2. Discard Selection: Night Assailant (triggers GY effect!) > Regenerating Mummy > extra Zombies
            if (hint == HINTMSG_DISCARD)
            {
                var discards = cards.Where(c => c.Controller == 0).OrderBy(c =>
                {
                    if (c.IsCode(CardId.NightAssailant)) return 1;
                    if (c.IsCode(CardId.RegeneratingMummy)) return 2;
                    if (c.IsCode(CardId.RyuKokki) || c.IsCode(CardId.VampireLord)) return 3; // Dump for Book of Life
                    if (IsAceCard(c)) return 90;
                    return 10;
                }).ToList();

                if (discards.Count >= min)
                    return discards.Take(min).ToList();
            }

            // 3. Special Summon:
            // Pyramid Turtle: Ryu Kokki (2400) > Vampire Lord (2000) > Spirit Reaper (stall)
            // Book of Life: Ryu Kokki > Vampire Lord > Regenerating Mummy
            if (hint == HINTMSG_SPSUMMON)
            {
                var summonPool = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.RyuKokki)) return 1;
                    if (c.IsCode(CardId.VampireLord)) return 2;
                    if (c.IsCode(CardId.SpiritReaper) && Enemy.GetMonsterCount() > 0) return 3;
                    if (c.IsCode(CardId.RegeneratingMummy)) return 4;
                    return 10;
                }).ToList();

                if (summonPool.Count >= min)
                    return summonPool.Take(min).ToList();
            }

            // 4. Banish selection (Book of Life / Kycoo): Banish highest threat in opponent GY
            if (hint == HINTMSG_REMOVE)
            {
                var oppGyMonsters = cards.Where(c => c.Controller == 1 && c.Location == CardLocation.Grave)
                    .OrderByDescending(c => c.Attack).ToList();
                if (oppGyMonsters.Count >= min)
                    return oppGyMonsters.Take(min).ToList();
            }

            // 5. Search / Add to Hand (Sangan / Night Assailant):
            if (hint == HINTMSG_ATOHAND)
            {
                // Night Assailant recycle: Magician of Faith > Morphing Jar
                var flipRecycle = cards.Where(c => c.HasType(CardType.Flip) && c.Id != CardId.NightAssailant)
                    .OrderBy(c => c.IsCode(CardId.MagicianOfFaith) ? 1 : 2).FirstOrDefault();
                if (flipRecycle != null) return new List<ClientCard> { flipRecycle };

                // Sangan search: Tribe-Infecting Virus > Breaker > Magician of Faith > Spirit Reaper
                var sanganSearch = cards.OrderBy(c =>
                {
                    if (c.IsCode(CardId.TribeInfectingVirus) && Enemy.GetMonsterCount() >= 2) return 1;
                    if (c.IsCode(CardId.BreakerTheMagicalWarrior)) return 2;
                    if (c.IsCode(CardId.TribeInfectingVirus)) return 3;
                    if (c.IsCode(CardId.MagicianOfFaith) && Bot.Graveyard.Any(g => g.IsSpell())) return 4;
                    if (c.IsCode(CardId.SpiritReaper)) return 5;
                    return 10;
                }).ToList();

                if (sanganSearch.Count >= min)
                    return sanganSearch.Take(min).ToList();
            }

            // 6. Target Destruction (Raigeki Break / Night Assailant):
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
            // Vampire Lord mill declaration: Option 1 is Spell (rip Pot, Charity, Snatch Steal, Heavy)
            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 4;
                long optIndex = options[i] & 0xf;
                if (cardId == 0) cardId = options[i] >> 20;

                if (cardId == CardId.VampireLord || (LastChainCard != null && LastChainCard.IsCode(CardId.VampireLord)))
                {
                    if (optIndex == 1) return i; // Declare Spell
                }
            }

            return base.OnSelectOption(options);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.Jinzo || cardId == CardId.RyuKokki || cardId == CardId.GiantOrc ||
                cardId == CardId.BreakerTheMagicalWarrior || cardId == CardId.KycooTheGhostDestroyer)
            {
                if (positions.Contains(CardPosition.Attack)) return CardPosition.Attack;
            }

            if (cardId == CardId.SpiritReaper || cardId == CardId.PyramidTurtle ||
                cardId == CardId.MagicianOfFaith || cardId == CardId.NightAssailant || cardId == CardId.MorphingJar)
            {
                if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                if (positions.Contains(CardPosition.Defence)) return CardPosition.Defence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        // ═══════════════════════════════════════════════════════════════
        //  Strategic Decision Logic
        // ═══════════════════════════════════════════════════════════════

        private bool IsJinzoOnField()
        {
            return Bot.HasInMonstersZone(CardId.Jinzo) || Enemy.HasInMonstersZone(CardId.Jinzo);
        }

        private bool SevenToolsEffect()
        {
            if (IsJinzoOnField() || Bot.LifePoints <= 1000) return false;
            return DefaultTrap();
        }

        private bool MagicDrainEffect()
        {
            if (IsJinzoOnField()) return false;
            return DefaultTrap();
        }

        private bool HeavyStormEffect()
        {
            // Don't blow up Snatch Steal if we control opponent's boss
            if (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SnatchSteal))) return false;
            return Enemy.GetSpellCount() >= 2 || (Enemy.GetSpellCount() >= 1 && Bot.GetSpellCount() <= 1);
        }

        private bool LightningVortexEffect()
        {
            if (DefaultSpellWillBeNegated() || Bot.Hand.Count == 0) return false;
            int faceupEnemies = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsTargetImmune(c));
            return faceupEnemies >= 2 || (faceupEnemies >= 1 && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2400));
        }

        private bool RaigekiBreakEffect()
        {
            if (IsJinzoOnField() || Bot.Hand.Count == 0) return false;
            ClientCard target = Util.GetProblematicEnemyCard(0, true);
            if (target != null)
            {
                AI.SelectNextCard(target);
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

        private bool TribeInfectingVirusSummon()
        {
            return Enemy.GetMonsterCount() >= 2 && Bot.Hand.Any(c => c != null && c.Id != CardId.TribeInfectingVirus);
        }

        private bool TribeInfectingVirusEffect()
        {
            if (Enemy.GetMonsterCount() == 0 || Bot.Hand.Count == 0) return false;
            var faceUp = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c)).ToList();
            if (faceUp.Count == 0) return false;

            var mostCommonRace = faceUp.GroupBy(c => c.Race).OrderByDescending(g => g.Count()).First();
            CardRace targetRace = (CardRace)mostCommonRace.Key;

            AI.SelectRace(targetRace);
            return true;
        }

        private bool BookOfLifeEffect()
        {
            bool hasZombieInGy = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.Zombie) && c.IsCanRevive());
            bool hasOpponentGy = Enemy.Graveyard.Any(c => c != null && c.IsMonster());
            return hasZombieInGy && hasOpponentGy;
        }

        private bool MagicianOfFaithEffect()
        {
            return Bot.Graveyard.Any(c => c != null && c.IsSpell());
        }

        private bool NightAssailantEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Flip) && c.Id != CardId.NightAssailant);
            }
            return Enemy.GetMonsterCount() > 0;
        }

        private bool PyramidTurtleEffect()
        {
            return true;
        }

        private bool VampireLordEffect()
        {
            return true;
        }

        private bool KycooEffect()
        {
            return Enemy.Graveyard.Any(c => c != null && c.IsMonster());
        }

        private bool JinzoSummon()
        {
            // Level 6: Requires 1 tribute!
            if (Bot.GetMonsterCount() < 1) return false;
            // Always tribute summon if opponent has backrow or we have lethal
            return Enemy.GetSpellCount() > 0 || GetTotalFieldATK() >= Enemy.LifePoints;
        }

        private bool RyuKokkiSummon()
        {
            // Level 6: Requires 1 tribute!
            if (Bot.GetMonsterCount() < 1) return false;
            // Only tribute summon if we have tribute fodder (Sangan / Turtle) and opponent has threats
            bool hasFodder = Bot.GetMonsters().Any(c => c != null && (c.IsCode(CardId.Sangan) || c.IsCode(CardId.PyramidTurtle)));
            return hasFodder && Enemy.GetMonsterCount() > 0;
        }

        private bool VampireLordSummon()
        {
            // Level 5: Requires 1 tribute!
            if (Bot.GetMonsterCount() < 1) return false;
            bool hasFodder = Bot.GetMonsters().Any(c => c != null && (c.IsCode(CardId.Sangan) || c.IsCode(CardId.PyramidTurtle)));
            return hasFodder;
        }

        private bool BreakerEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && Card.Attack == 1900)
            {
                return DefaultMysticalSpaceTyphoon();
            }
            return false;
        }

        private bool TorrentialTributeEffect()
        {
            if (IsJinzoOnField()) return false;
            // Don't trigger if our board is superior
            if (Bot.GetMonsters().Any(c => c != null && IsAceCard(c)) && Enemy.GetMonsterCount() <= 1) return false;
            return DefaultTorrentialTribute();
        }

        private bool MirrorForceEffect()
        {
            if (IsJinzoOnField() || Duel.Player != 1) return false;
            if (Enemy.BattlingMonster != null && Enemy.BattlingMonster.Attack >= 1500) return true;
            return Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsAttack()) >= 2;
        }

        private bool SetTrapCondition()
        {
            // Never set traps while Jinzo is face-up on the field
            if (IsJinzoOnField()) return false;
            return Card.HasType(CardType.Trap);
        }

        private bool MonsterRepos()
        {
            if (Card.IsCode(CardId.MagicianOfFaith) && Card.IsFacedown() && Bot.Graveyard.Any(c => c.IsSpell()))
            {
                return true; // Flip summon to retrieve spell
            }
            return DefaultMonsterRepos();
        }
    }
}
