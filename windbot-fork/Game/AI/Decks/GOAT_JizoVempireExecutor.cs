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
    // GOAT_JizoVempire (Zombie Control & Jinzo)
    // ==========================================
    [Deck("GOAT_JizoVempire", "GOAT_JizoVempire")]
    public class GOAT_JizoVempireExecutor : ModernExecutor
    {
        public class CardId
        {
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
            public const int BookOfLife = 2204140;
            public const int LightningVortex = 69162969;
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int SnatchSteal = 45986603;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int HeavyStorm = 19613556;
            public const int TorrentialTribute = 53582587;
            public const int MirrorForce = 44095762;
            public const int RaigekiBreak = 4178474;
            public const int MagicDrain = 59344077;
            public const int SevenToolsOfTheBandit = 3819470;
        }

        private static readonly int[] BossMonsters = {
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
            // ── Staples & Counter/Trap Negations ──
            AddExecutor(ExecutorType.Activate, CardId.SevenToolsOfTheBandit, SevenToolsEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagicDrain, SevenToolsEffect);

            // ── Draw & Search ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);

            // ── Removal & Backrow Clear ──
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, DefaultHeavyStorm);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.LightningVortex, LightningVortexEffect);
            AddExecutor(ExecutorType.Activate, CardId.RaigekiBreak, RaigekiBreakEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);

            // ── Tribe-Infecting Virus ──
            AddExecutor(ExecutorType.Activate, CardId.TribeInfectingVirus, TribeInfectingVirusEffect);

            // ── Zombie Reborn (Book of Life) ──
            AddExecutor(ExecutorType.Activate, CardId.BookOfLife, BookOfLifeEffect);

            // ── Flip Monsters ──
            AddExecutor(ExecutorType.Activate, CardId.MagicianOfFaith, MagicianOfFaithEffect);
            AddExecutor(ExecutorType.Activate, CardId.MorphingJar);
            AddExecutor(ExecutorType.Activate, CardId.NightAssailant, NightAssailantEffect);

            // ── Combat/Trigger Monster Effects ──
            AddExecutor(ExecutorType.Activate, CardId.PyramidTurtle, PyramidTurtleEffect);
            AddExecutor(ExecutorType.Activate, CardId.BreakerTheMagicalWarrior, BreakerEffect);
            AddExecutor(ExecutorType.Activate, CardId.VampireLord, VampireLordEffect);
            AddExecutor(ExecutorType.Activate, CardId.KycooTheGhostDestroyer, KycooEffect);

            // ── Summons ──
            // Jinzo tribute summon logic
            AddExecutor(ExecutorType.Summon, CardId.Jinzo, JinzoSummon);
            AddExecutor(ExecutorType.Summon, CardId.RyuKokki, RyuKokkiSummon);
            AddExecutor(ExecutorType.Summon, CardId.VampireLord, VampireLordSummon);

            // Normal summons / Sets
            AddExecutor(ExecutorType.Summon, CardId.KycooTheGhostDestroyer);
            AddExecutor(ExecutorType.Summon, CardId.GiantOrc);
            AddExecutor(ExecutorType.Summon, CardId.BreakerTheMagicalWarrior);
            
            // Set weak/floating monsters
            AddExecutor(ExecutorType.MonsterSet, CardId.PyramidTurtle);
            AddExecutor(ExecutorType.MonsterSet, CardId.SpiritReaper);
            AddExecutor(ExecutorType.MonsterSet, CardId.Sangan);
            AddExecutor(ExecutorType.MonsterSet, CardId.MagicianOfFaith);
            AddExecutor(ExecutorType.MonsterSet, CardId.MorphingJar);
            AddExecutor(ExecutorType.MonsterSet, CardId.NightAssailant);

            // Backup summons
            AddExecutor(ExecutorType.Summon, CardId.PyramidTurtle);
            AddExecutor(ExecutorType.Summon, CardId.RegeneratingMummy);

            // ── Traps ──
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, DefaultTorrentialTribute);
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);

            AddExecutor(ExecutorType.SpellSet, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        private bool TribeInfectingVirusEffect()
        {
            if (Enemy.GetMonsterCount() > 0 && Bot.Hand.Count > 0)
            {
                // Select most common race on opponent's field
                var races = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).Select(c => c.Race).ToList();
                if (races.Count > 0)
                {
                    int bestRace = races.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;
                    // Discard Night Assailant or Regenerating Mummy first (as they trigger recovery)
                    ClientCard discard = Bot.Hand.FirstOrDefault(c => c != null && (c.IsCode(CardId.NightAssailant) || c.IsCode(CardId.RegeneratingMummy)));
                    if (discard == null) discard = Bot.Hand.FirstOrDefault(c => c != null && c.Id != CardId.TribeInfectingVirus);
                    if (discard != null)
                    {
                        AI.SelectCard(discard);
                        AI.SelectRace((CardRace)bestRace);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool BookOfLifeEffect()
        {
            // Must have a zombie in GY and opponent must have a card in GY
            bool hasZombieInGy = Bot.Graveyard.Any(c => c != null && c.IsMonster() && IsZombie(c.Id) && c.IsCanRevive());
            bool hasOpponentGy = Enemy.Graveyard.Any(c => c != null);
            if (hasZombieInGy && hasOpponentGy)
            {
                ClientCard target = Bot.Graveyard.Where(c => c != null && c.IsMonster() && IsZombie(c.Id) && c.IsCanRevive())
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                ClientCard oppTarget = Enemy.Graveyard.Where(c => c != null).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null && oppTarget != null)
                {
                    AI.SelectCard(target);
                    AI.SelectNextCard(oppTarget);
                    return true;
                }
            }
            return false;
        }

        private bool MagicianOfFaithEffect()
        {
            // Grab best spell
            ClientCard spell = Bot.Graveyard.Where(c => c != null && c.IsSpell())
                .OrderByDescending(c => c.Id == CardId.PotOfGreed ? 10 : (c.Id == CardId.GracefulCharity ? 9 : (c.Id == CardId.SnatchSteal ? 8 : 1)))
                .FirstOrDefault();
            if (spell != null)
            {
                AI.SelectCard(spell);
                return true;
            }
            return false;
        }

        private bool NightAssailantEffect()
        {
            // Target face-up enemy monster, or recycle flip monster if discarded
            if (Card.Location == CardLocation.Grave)
            {
                ClientCard flipTarget = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.HasType(CardType.Flip) && c.Id != CardId.NightAssailant);
                if (flipTarget != null)
                {
                    AI.SelectCard(flipTarget);
                    return true;
                }
            }
            else
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool PyramidTurtleEffect()
        {
            // Summon Ryu Kokki (2400 ATK/2000 DEF) or Vampire Lord (2000 ATK/1500 DEF) or Spirit Reaper (for stall)
            AI.SelectCard(CardId.RyuKokki, CardId.VampireLord, CardId.SpiritReaper, CardId.RegeneratingMummy);
            return true;
        }

        private bool VampireLordEffect()
        {
            // Declare Card Type for opponent to send to GY (default: Spell - option index 1)
            AI.SelectOption(1);
            return true;
        }

        private bool KycooEffect()
        {
            // Banish opponent cards from GY when dealing damage
            var targets = Enemy.Graveyard.Where(c => c != null && c.IsMonster()).OrderByDescending(c => c.Attack).Take(2).ToList();
            if (targets.Count > 0)
            {
                AI.SelectCard(targets);
                return true;
            }
            return false;
        }

        private bool JinzoSummon()
        {
            // Only summon if opponent has traps on field or we want to push for game
            return Enemy.GetSpellCount() > 0 || GetTotalFieldATK() >= Enemy.LifePoints;
        }

        private bool RyuKokkiSummon()
        {
            // Requires 2 tributes, only Normal Summon if we have excess tribute material or no other plays
            return Bot.GetMonsterCount() >= 2;
        }

        private bool VampireLordSummon()
        {
            return Bot.GetMonsterCount() >= 1;
        }

        private bool SetTrapCondition()
        {
            // Don't set traps if Jinzo is face-up on our field
            if (Bot.HasInMonstersZone(CardId.Jinzo)) return false;
            return Card.HasType(CardType.Trap);
        }

        private bool IsZombie(int cardId)
        {
            return cardId == CardId.RyuKokki || cardId == CardId.VampireLord || cardId == CardId.PyramidTurtle || cardId == CardId.SpiritReaper || cardId == CardId.RegeneratingMummy;
        }

        private bool SevenToolsEffect()
        {
            return DefaultTrap();
        }

        private bool MirrorForceEffect()
        {
            return Duel.Player == 1 && Duel.Phase == DuelPhase.BattleStart;
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

        private bool LightningVortexEffect()
        {
            if (DefaultSpellWillBeNegated()) return false;
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack >= 1500) && Bot.Hand.Count > 0;
        }

        private bool RaigekiBreakEffect()
        {
            if (Bot.Hand.Count == 0) return false;
            ClientCard target = Util.GetProblematicEnemyCard(0, true);
            if (target != null)
            {
                ClientCard discard = Bot.Hand.FirstOrDefault(c => c != null && c.Id != Card.Id);
                if (discard != null)
                {
                    AI.SelectCard(discard);
                    AI.SelectNextCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool BreakerEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && Card.Attack == 1900)
            {
                return DefaultMysticalSpaceTyphoon();
            }
            return false;
        }

        private bool MonsterRepos()
        {
            if (Card.IsCode(CardId.MagicianOfFaith) && Card.IsFacedown() && Bot.Graveyard.Any(c => c.IsSpell()))
            {
                return true;
            }
            return DefaultMonsterRepos();
        }
    }
}
