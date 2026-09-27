using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("ChaosTurbo", "ChaosTurbo")]
    public class ChaosTurboExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Monsters
            public const int BlackLusterSoldierEnvoyOfTheBeginning = 72989439;
            public const int ChaosSorcerer = 9596126;
            public const int Jinzo = 77585513;
            public const int ThunderDragon = 31786629;
            public const int DekoichiTheBattlechantedLocomotive = 87621407;
            public const int NightAssailant = 16226786;
            public const int MagicianOfFaith = 31560081;
            public const int BreakerTheMagicalWarrior = 71413901;
            public const int Sangan = 26202165;
            public const int MorphingJar = 33508719;

            // Spells
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int DelinquentDuo = 44763025;
            public const int SnatchSteal = 45986603;
            public const int PrematureBurial = 70828912;
            public const int HeavyStorm = 19613556;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int NoblemanOfCrossout = 71044499;
            public const int BookOfMoon = 14087893;
            public const int Scapegoat = 73915051;
            public const int Metamorphosis = 46411259;
            public const int UpstartGoblin = 70368879;

            // Traps
            public const int MirrorForce = 44095762;
            public const int TorrentialTribute = 53582587;
            public const int RingOfDestruction = 83555666;
            public const int CallOfTheHaunted = 97077563;
            public const int JarOfGreed = 83968380;
            public const int Ceasefire = 36468556;

            // Extra Deck Fusions
            public const int ThousandEyesRestrict = 63519819;
            public const int DarkBalterTheTerrible = 80071763;
            public const int RyuSenshi = 49868263;
            public const int OjamaKing = 90140980;
            public const int KingDragun = 13756293;
            public const int GatlingDragon = 87751584;

            // Tokens
            public const int SheepToken = 73915052;
        }

        private static readonly int[] BossMonsters =
        {
            CardId.BlackLusterSoldierEnvoyOfTheBeginning,
            CardId.ChaosSorcerer,
            CardId.Jinzo
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public ChaosTurboExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // 0. Install Decoupled Domain Plugin
            DeckPlugin = new ChaosTurboPlugin(this);

            // 1. Turbo Deck-Thinning Engine
            AddExecutor(ExecutorType.Activate, CardId.ThunderDragon, ThunderDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.UpstartGoblin);
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);
            AddExecutor(ExecutorType.Activate, CardId.DelinquentDuo, DelinquentDuoEffect);

            // 2. Removal & Disruption Spells
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.NoblemanOfCrossout, NoblemanOfCrossoutEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrematureBurial, PrematureBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.CallOfTheHaunted, CallOfTheHauntedEffect);

            // 3. Chaos Boss Invasions
            AddExecutor(ExecutorType.SpSummon, CardId.BlackLusterSoldierEnvoyOfTheBeginning, BlsSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlackLusterSoldierEnvoyOfTheBeginning, BlsEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosSorcerer, ChaosSorcererSummon);
            AddExecutor(ExecutorType.Activate, CardId.ChaosSorcerer, ChaosSorcererEffect);

            // 4. Tribute Boss
            AddExecutor(ExecutorType.Summon, CardId.Jinzo, JinzoTributeSummon);

            // 5. Flip & Graveyard Triggers
            AddExecutor(ExecutorType.Activate, CardId.NightAssailant, NightAssailantEffect);
            AddExecutor(ExecutorType.Activate, CardId.DekoichiTheBattlechantedLocomotive);
            AddExecutor(ExecutorType.Activate, CardId.MagicianOfFaith);
            AddExecutor(ExecutorType.Activate, CardId.MorphingJar);

            // 6. Normal Summons / Breaker
            AddExecutor(ExecutorType.Summon, CardId.BreakerTheMagicalWarrior);
            AddExecutor(ExecutorType.Activate, CardId.BreakerTheMagicalWarrior, BreakerEffect);

            // 7. Defensive / Flip Monster Sets
            AddExecutor(ExecutorType.MonsterSet, CardId.DekoichiTheBattlechantedLocomotive);
            AddExecutor(ExecutorType.MonsterSet, CardId.NightAssailant);
            AddExecutor(ExecutorType.MonsterSet, CardId.MagicianOfFaith);
            AddExecutor(ExecutorType.MonsterSet, CardId.MorphingJar);
            AddExecutor(ExecutorType.MonsterSet, CardId.Sangan);

            // 8. Traps & Quick Spells
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonEffect);
            AddExecutor(ExecutorType.Activate, CardId.Metamorphosis, MetamorphosisEffect);
            AddExecutor(ExecutorType.Activate, CardId.Scapegoat, ScapegoatEffect);
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, DefaultTorrentialTribute);
            AddExecutor(ExecutorType.Activate, CardId.RingOfDestruction, RingOfDestructionEffect);
            AddExecutor(ExecutorType.Activate, CardId.Ceasefire, CeasefireEffect);
            AddExecutor(ExecutorType.Activate, CardId.JarOfGreed, JarOfGreedEffect);

            // 9. Backrow Setting
            AddExecutor(ExecutorType.SpellSet, CardId.JarOfGreed);
            AddExecutor(ExecutorType.SpellSet, CardId.Scapegoat);
            AddExecutor(ExecutorType.SpellSet, CardId.BookOfMoon);
            AddExecutor(ExecutorType.SpellSet, CardId.MirrorForce);
            AddExecutor(ExecutorType.SpellSet, CardId.TorrentialTribute);
            AddExecutor(ExecutorType.SpellSet, CardId.RingOfDestruction);
            AddExecutor(ExecutorType.SpellSet, CardId.CallOfTheHaunted);
            AddExecutor(ExecutorType.SpellSet, CardId.Ceasefire);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXECUTION LOGIC
        // ═══════════════════════════════════════════════════════════════

        private bool ThunderDragonEffect()
        {
            // Discard 1 Thunder Dragon from hand to add up to 2 copies from deck to hand
            return Card.Location == CardLocation.Hand;
        }

        private bool DelinquentDuoEffect()
        {
            return Bot.LifePoints > 1000 && Enemy.Hand.Count >= 2;
        }

        private bool HeavyStormEffect()
        {
            int enemySpells = Enemy.GetSpellCount();
            int ourSpells = Bot.GetSpellCount();
            return enemySpells >= 2 && enemySpells > ourSpells;
        }

        private bool NoblemanOfCrossoutEffect()
        {
            var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFacedown());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SnatchStealEffect()
        {
            var target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && !IsTargetImmune(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null && target.Attack >= 1500)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool PrematureBurialEffect()
        {
            if (Bot.LifePoints <= 800) return false;
            var target = DeckPlugin?.Strategy?.PickSpecialSummonTarget(Bot.Graveyard.Where(c => c.IsMonster()).ToList());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CallOfTheHauntedEffect()
        {
            if (Duel.Player == 1 && Duel.Phase != DuelPhase.End) return false;
            var target = DeckPlugin?.Strategy?.PickSpecialSummonTarget(Bot.Graveyard.Where(c => c.IsMonster()).ToList());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool BlsSummon()
        {
            int lightCount = Bot.Graveyard.Count(c => c != null && c.HasAttribute(CardAttribute.Light));
            int darkCount = Bot.Graveyard.Count(c => c != null && c.HasAttribute(CardAttribute.Dark));
            return lightCount >= 1 && darkCount >= 1;
        }

        private bool BlsEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || !Card.IsFaceup()) return false;

            // Check if attack is locked by floodgates (Gravity Bind, Level Limit Area B, etc.)
            bool isAttackLocked = Bot.HasInSpellZone(85742772) || Bot.HasInSpellZone(3136426) ||
                                  Enemy.HasInSpellZone(85742772) || Enemy.HasInSpellZone(3136426);

            // Banish targets: MUST be opponent's card (c.Controller == 1)
            var enemyMonsters = Enemy.GetMonsters().Where(m => m != null && !IsTargetImmune(m)).ToList();
            if (enemyMonsters.Count == 0)
            {
                // CRITICAL FIX: If enemy has NO monsters, NEVER activate BLS banish effect!
                // Otherwise BLS will be forced to banish its own monsters or banish itself!
                return false;
            }

            // If attack is locked, banish any enemy monster
            if (isAttackLocked)
            {
                var target = enemyMonsters.OrderByDescending(m => m.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectOption(0);
                    AI.SelectCard(target);
                    return true;
                }
            }

            // Banish high threats: face-down monsters (flippers/stall), fusions, high ATK (>= 2000), or monsters in defense
            var threat = enemyMonsters
                .Where(m => m.IsFacedown() || m.Attack >= 2000 || m.HasType(CardType.Fusion) || m.IsDefense())
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (threat != null)
            {
                AI.SelectOption(0); // Banish threat
                AI.SelectCard(threat);
                return true;
            }

            // If enemy only has small face-up attack monsters, attack them to trigger Battle Phase double-attack!
            return false;
        }

        private bool ChaosSorcererSummon()
        {
            int lightCount = Bot.Graveyard.Count(c => c != null && c.HasAttribute(CardAttribute.Light));
            int darkCount = Bot.Graveyard.Count(c => c != null && c.HasAttribute(CardAttribute.Dark));
            // Keep at least 1 LIGHT/DARK for BLS if we hold BLS in hand
            if (Bot.HasInHand(CardId.BlackLusterSoldierEnvoyOfTheBeginning))
            {
                return lightCount >= 2 && darkCount >= 2;
            }
            return lightCount >= 1 && darkCount >= 1;
        }

        private bool ChaosSorcererEffect()
        {
            var target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && !IsTargetImmune(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool JinzoTributeSummon()
        {
            var tribute = Bot.GetMonsters().FirstOrDefault(m => m != null && (m.Id == CardId.SheepToken || (m.IsFaceup() && (m.Id == CardId.DekoichiTheBattlechantedLocomotive || m.Id == CardId.MagicianOfFaith))));
            if (tribute != null)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return false;
        }

        private bool NightAssailantEffect()
        {
            // If activated on field (Flip effect): destroy 1 enemy monster
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            // If activated from GY (discarded): return a Flip monster from GY to hand
            return true;
        }

        private bool BreakerEffect()
        {
            var target = Enemy.GetSpells().FirstOrDefault(s => s != null && !IsTargetImmune(s));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool BookOfMoonEffect()
        {
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep))
            {
                var attacker = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack >= 1500 && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack).FirstOrDefault();
                if (attacker != null)
                {
                    AI.SelectCard(attacker);
                    return true;
                }
            }
            return false;
        }

        private bool MetamorphosisEffect()
        {
            var monsters = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (monsters.Count == 0) return false;

            // Lv1 token -> TER
            var lv1 = monsters.FirstOrDefault(m => m.Level == 1 && m.Id == CardId.SheepToken);
            if (lv1 != null && Enemy.GetMonsterCount() > 0)
            {
                AI.SelectCard(lv1);
                AI.SelectNextCard(CardId.ThousandEyesRestrict);
                return true;
            }

            // Lv6 Jinzo / Chaos Sorcerer that already used banish -> Ryu Senshi or Ojama King
            var lv6 = monsters.FirstOrDefault(m => m.Level == 6 && m.Id == CardId.ChaosSorcerer);
            if (lv6 != null && Enemy.GetMonsterCount() == 0)
            {
                AI.SelectCard(lv6);
                AI.SelectNextCard(CardId.RyuSenshi);
                return true;
            }

            return false;
        }

        private bool ScapegoatEffect()
        {
            if (Duel.Player == 0) return false; // In Chaos Turbo, save Scapegoat for opponent's turn
            if (Duel.Phase == DuelPhase.End) return true;
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
            {
                return Bot.GetMonsterCount() == 0 || Enemy.GetMonsters().Any(m => m.IsAttack() && m.Attack >= 1500);
            }
            return false;
        }

        private bool RingOfDestructionEffect()
        {
            var target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && m.Attack < Bot.LifePoints && !IsTargetImmune(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (target != null && (target.Attack >= Enemy.LifePoints || target.Attack >= 1800))
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CeasefireEffect()
        {
            // Activate if lethal or flips face-down monsters
            int effectMonsters = Bot.GetMonsters().Count(m => m.IsFaceup()) + Enemy.GetMonsters().Count(m => m.IsFaceup());
            int burn = effectMonsters * 500;
            if (burn >= Enemy.LifePoints) return true;
            if (Enemy.GetMonsters().Any(m => m.IsFacedown()) && Duel.Player == 0) return true;
            return false;
        }

        private bool JarOfGreedEffect()
        {
            // Chain to destruction or activate at opponent's End Phase
            return Duel.Player == 1 && Duel.Phase == DuelPhase.End;
        }

        private bool MirrorForceEffect()
        {
            if (Duel.Player != 1) return false;
            if (Enemy.BattlingMonster != null && Enemy.BattlingMonster.Attack >= 1500) return true;
            return Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsAttack()) >= 2;
        }

        private bool MonsterRepos()
        {
            return DefaultMonsterRepos();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Enforce Strict Rule 1: NEVER destroy/remove own cards if enemy can be chosen!
            if (hint == 502 || hint == 503 || hint == 505 || hint == 507)
            {
                var enemyTargets = cards.Where(c => c.Controller == 1).ToList();
                if (enemyTargets.Count >= min)
                {
                    return enemyTargets.Take(min).ToList();
                }
            }

            // Discard hint (501: HINTMSG_DISCARD, 504: HINTMSG_TOGRAVE, 508): Delegate to Domain Plugin
            if (hint == 501 || hint == 504 || hint == 508)
            {
                var ourCards = cards.Where(c => c.Controller == 0).ToList();
                if (ourCards.Count >= min)
                {
                    if (min == 1)
                    {
                        var discard = DeckPlugin?.MaterialEvaluator?.PickDiscardTarget(ourCards, min);
                        if (discard != null)
                        {
                            return new List<ClientCard> { discard };
                        }
                    }
                    else
                    {
                        var sorted = DeckPlugin?.MaterialEvaluator?.SortMaterials(ourCards, min);
                        if (sorted != null && sorted.Count >= min)
                        {
                            return sorted.Take(min).ToList();
                        }
                    }
                }
            }

            // Search hint: Delegate to Domain Plugin
            if (hint == 506)
            {
                var target = DeckPlugin?.Strategy?.PickSearchTarget(cards, Card);
                if (target != null)
                {
                    return new List<ClientCard> { target };
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }
}
