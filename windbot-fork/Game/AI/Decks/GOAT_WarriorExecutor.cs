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
    // GOAT_Warrior (Chaos Warrior Control)
    // ==========================================
    [Deck("GOAT_Warrior", "GOAT_Warrior")]
    public class GOAT_WarriorExecutor : ModernExecutor
    {
        public class CardId
        {
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
            public const int CallOfTheHaunted = 97077563;
            public const int MirrorForce = 44095762;
            public const int TorrentialTribute = 53582587;
            public const int RingOfDestruction = 83555666;
            public const int SakuretsuArmor = 56120475;
            public const int DustTornado = 60082869;
            public const int SolemnJudgment = 41420027;
            public const int TrapDustshoot = 64697231;
        }

        private static readonly int[] BossMonsters = {
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
            // ── Disruption / Trap Negators ──
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrapDustshoot, TrapDustshootEffect);

            // ── Draw & Search ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);
            AddExecutor(ExecutorType.Activate, CardId.DelinquentDuo, DelinquentDuoEffect);

            // ── Backrow removal & Spot removal ──
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, DefaultHeavyStorm);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.DustTornado, DustTornadoEffect);
            AddExecutor(ExecutorType.Activate, CardId.NoblemanOfCrossout, NoblemanOfCrossoutEffect);
            AddExecutor(ExecutorType.Activate, CardId.SmashingGround, DefaultSmashingGround);
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);
            AddExecutor(ExecutorType.Activate, CardId.ExiledForce, ExiledForceEffect);

            // ── Reinforcement of the Army (Search) ──
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotAEffect);

            // ── Tribe-Infecting Virus ──
            AddExecutor(ExecutorType.Activate, CardId.TribeInfectingVirus, TribeInfectingVirusEffect);

            // ── Spells / Book of Moon ──
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonEffect);

            // ── Chaos Summon (BLS) ──
            AddExecutor(ExecutorType.SpSummon, CardId.BlackLusterSoldierEnvoyOfTheBeginning, BlsSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlackLusterSoldierEnvoyOfTheBeginning, BlsEffect);

            // ── Reborn / Premature ──
            AddExecutor(ExecutorType.Activate, CardId.PrematureBurial, PrematureBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.CallOfTheHaunted, DefaultCallOfTheHaunted);

            // ── Monster Actions ──
            AddExecutor(ExecutorType.Activate, CardId.BreakerTheMagicalWarrior, BreakerEffect);
            AddExecutor(ExecutorType.Activate, CardId.DdWarriorLady, DdWarriorLadyEffect);
            AddExecutor(ExecutorType.Activate, CardId.DonZaloog, DonZaloogEffect);
            AddExecutor(ExecutorType.Activate, CardId.KycooTheGhostDestroyer, KycooEffect);

            // ── Summons ──
            AddExecutor(ExecutorType.Summon, CardId.BladeKnight);
            AddExecutor(ExecutorType.Summon, CardId.KycooTheGhostDestroyer);
            AddExecutor(ExecutorType.Summon, CardId.BreakerTheMagicalWarrior);
            AddExecutor(ExecutorType.Summon, CardId.DdWarriorLady);
            AddExecutor(ExecutorType.Summon, CardId.DonZaloog);
            AddExecutor(ExecutorType.Summon, CardId.NinjaGrandmasterSasuke);
            AddExecutor(ExecutorType.Summon, CardId.ZombyraTheDark);
            AddExecutor(ExecutorType.Summon, CardId.ExiledForce, ExiledForceSummon);
            
            // Set weak / floaters
            AddExecutor(ExecutorType.MonsterSet, CardId.MysticSwordsmanLv2);
            AddExecutor(ExecutorType.Summon, CardId.MysticSwordsmanLv2);

            // ── Traps ──
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, DefaultTorrentialTribute);
            AddExecutor(ExecutorType.Activate, CardId.RingOfDestruction, RingOfDestructionEffect);
            AddExecutor(ExecutorType.Activate, CardId.SakuretsuArmor, SakuretsuArmorEffect);

            AddExecutor(ExecutorType.SpellSet, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
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

        private bool BreakerEffect()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && Card.Attack == 1900)
            {
                return DefaultMysticalSpaceTyphoon();
            }
            return false;
        }

        private bool MirrorForceEffect()
        {
            if (Duel.Player != 1) return false;
            // Only use on meaningful attacks
            if (Enemy.BattlingMonster != null && Enemy.BattlingMonster.Attack >= 1000) return true;
            if (Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsAttack()) >= 2) return true;
            return false;
        }

        private bool SakuretsuArmorEffect()
        {
            if (Duel.Player != 1) return false;
            // Save for meaningful attacks
            return Enemy.BattlingMonster != null && Enemy.BattlingMonster.Attack >= 1500;
        }

        private bool SolemnJudgmentEffect()
        {
            if (Util.GetLastChainCard() == null) return false;
            if (Util.GetLastChainCard().Controller == 0) return false;
            return Util.GetLastChainCard().Id == CardId.HeavyStorm || Util.GetLastChainCard().Id == CardId.SnatchSteal || Util.GetLastChainCard().Id == CardId.BlackLusterSoldierEnvoyOfTheBeginning;
        }

        private bool TrapDustshootEffect()
        {
            return Enemy.Hand.Count >= 4 && Duel.Phase == DuelPhase.Standby;
        }

        private bool RotAEffect()
        {
            // Search D.D. Warrior Lady (versatile removal) or Blade Knight (highest ATK) or Sasuke (to beat DEF)
            if (Enemy.GetMonsterCount() > 0 && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2000))
            {
                if (Bot.GetRemainingCount(CardId.DdWarriorLady, 1) > 0)
                    AI.SelectCard(CardId.DdWarriorLady, CardId.BladeKnight, CardId.DonZaloog);
                else
                    AI.SelectCard(CardId.BladeKnight, CardId.DonZaloog, CardId.NinjaGrandmasterSasuke);
            }
            else if (Enemy.GetMonsters().Any(c => c != null && c.IsFacedown()))
            {
                if (Bot.GetRemainingCount(CardId.MysticSwordsmanLv2, 1) > 0)
                    AI.SelectCard(CardId.MysticSwordsmanLv2, CardId.DdWarriorLady, CardId.BladeKnight);
                else
                    AI.SelectCard(CardId.DdWarriorLady, CardId.BladeKnight, CardId.NinjaGrandmasterSasuke);
            }
            else
            {
                if (Bot.GetRemainingCount(CardId.BladeKnight, 1) > 0)
                    AI.SelectCard(CardId.BladeKnight, CardId.DdWarriorLady, CardId.DonZaloog);
                else
                    AI.SelectCard(CardId.DdWarriorLady, CardId.DonZaloog, CardId.KycooTheGhostDestroyer);
            }
            return true;
        }

        private bool TribeInfectingVirusEffect()
        {
            if (Enemy.GetMonsterCount() == 0 || Bot.Hand.Count <= 1) return false;
            var faceUpMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            if (faceUpMonsters.Count == 0) return false;

            // Find the most common race among face-up enemy monsters
            var raceGroups = faceUpMonsters.GroupBy(c => c.Race).OrderByDescending(g => g.Count()).First();
            CardRace bestRace = (CardRace)raceGroups.Key;

            // Don't discard our last card if it's critical
            ClientCard discard = Bot.Hand
                .Where(c => c != null && c.Id != CardId.TribeInfectingVirus && !IsAceCard(c))
                .OrderBy(c => c.Attack)
                .FirstOrDefault();
            if (discard != null)
            {
                AI.SelectCard(discard);
                AI.SelectRace(bestRace);
                return true;
            }
            return false;
        }

        private bool BookOfMoonEffect()
        {
            // Negate attack by flipping attacker down, or flip face-down defense target to attack with Mystic Swordsman
            ClientCard enemyAttacker = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack >= 1500);
            if (enemyAttacker != null)
            {
                AI.SelectCard(enemyAttacker);
                return true;
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

            // During Battle Phase: choose double attack
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep || Duel.Phase == DuelPhase.Damage)
            {
                AI.SelectOption(1); // Option 1: Double attack
                return true;
            }

            // Check if attack is locked by Gravity Bind or Level Limit Area B
            bool isLocked = Bot.HasInSpellZone(85742772) || Bot.HasInSpellZone(3136426) || Enemy.HasInSpellZone(85742772) || Enemy.HasInSpellZone(3136426);

            if (isLocked)
            {
                // Can't attack, so banish instead
                ClientCard target = Enemy.GetMonsters().Where(c => c != null).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectOption(0); // Option 0: Banish
                    AI.SelectCard(target);
                    return true;
                }
            }

            // Banish if opponent has a giant threat we can't beat over
            ClientCard threat = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.Attack >= 3000);
            if (threat != null)
            {
                AI.SelectOption(0);
                AI.SelectCard(threat);
                return true;
            }

            // Banish a face-down monster that might be a flip effect threat
            ClientCard facedown = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFacedown());
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
            // Try to revive best monsters in priority order
            int[] reviveTargets = {
                CardId.BlackLusterSoldierEnvoyOfTheBeginning,
                CardId.BladeKnight,
                CardId.DdWarriorLady,
                CardId.KycooTheGhostDestroyer,
                CardId.BreakerTheMagicalWarrior,
                CardId.DonZaloog
            };
            ClientCard target = Bot.Graveyard.FirstOrDefault(c => c != null && reviveTargets.Contains(c.Id) && c.IsCanRevive());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool DdWarriorLadyEffect()
        {
            // Banish D.D. Warrior Lady and target if the target has higher ATK than D.D. Warrior Lady or is a big threat
            if (Card.Location == CardLocation.MonsterZone && Bot.BattlingMonster == Card)
            {
                ClientCard opponentMonster = Enemy.BattlingMonster;
                if (opponentMonster != null && (opponentMonster.Attack > 1500 || IsAceCard(opponentMonster)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool DonZaloogEffect()
        {
            // Only trigger after dealing battle damage
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Option 0: Discard 1 random card from opponent's hand
            // Option 1: Send top 2 cards of opponent's deck to GY
            if (Enemy.Hand.Count >= 2)
            {
                AI.SelectOption(0); // Hand disruption is stronger
            }
            else
            {
                AI.SelectOption(1); // Mill if hand is small
            }
            return true;
        }

        private bool KycooEffect()
        {
            if (Enemy.Graveyard.Count > 0)
            {
                var targets = Enemy.Graveyard.OrderByDescending(c => c.Attack).Take(2).ToList();
                AI.SelectCard(targets);
                return true;
            }
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
            return Card.HasType(CardType.Trap);
        }

        private bool MonsterRepos()
        {
            return DefaultMonsterRepos();
        }

        private bool IsLight(int cardId)
        {
            return cardId == CardId.BladeKnight || cardId == CardId.DdWarriorLady || cardId == CardId.BlackLusterSoldierEnvoyOfTheBeginning || cardId == CardId.NinjaGrandmasterSasuke;
        }

        private bool IsDark(int cardId)
        {
            return cardId == CardId.ZombyraTheDark || cardId == CardId.KycooTheGhostDestroyer || cardId == CardId.DonZaloog || cardId == CardId.BreakerTheMagicalWarrior;
        }
    }
}
