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
    [Deck("ReasoningGate", "ReasoningGate")]
    public class ReasoningGateExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Monsters
            public const int DarkMagicianOfChaos = 40737112;
            public const int Jinzo = 77585513;
            public const int AirknightParshath = 18036057;
            public const int BlackLusterSoldierEnvoyOfTheBeginning = 72989439;
            public const int ChaosSorcerer = 9596126;
            public const int SacredCrane = 30914564;
            public const int MagicianOfFaith = 31560081;
            public const int SinisterSerpent = 8131171;
            public const int Sangan = 26202165;
            public const int DdWarriorLady = 7572887;

            // Spells
            public const int Reasoning = 58577036;
            public const int MonsterGate = 43040603;
            public const int DimensionFusion = 23557835;
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int DelinquentDuo = 44763025;
            public const int SnatchSteal = 45986603;
            public const int PrematureBurial = 70828912;
            public const int HeavyStorm = 19613556;
            public const int GiantTrunade = 42703248;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int Scapegoat = 73915051;
            public const int BookOfMoon = 14087893;
            public const int NoblemanOfCrossout = 71044499;
            public const int CardDestruction = 72892473;
            public const int Reload = 22589918;
            public const int SpellReproduction = 29228529;

            // Traps
            public const int CallOfTheHaunted = 97077563;
            public const int TorrentialTribute = 53582587;

            // Fusions
            public const int ThousandEyesRestrict = 63519819;
            public const int DarkBalterTheTerrible = 80071763;
            public const int RyuSenshi = 49868263;

            // Tokens
            public const int SheepToken = 73915052;
        }

        private static readonly int[] BossMonsters =
        {
            CardId.DarkMagicianOfChaos,
            CardId.Jinzo,
            CardId.BlackLusterSoldierEnvoyOfTheBeginning,
            CardId.AirknightParshath
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public ReasoningGateExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // 0. Install Decoupled Domain Plugin
            DeckPlugin = new ReasoningGatePlugin(this);

            // 1. Backrow Neutralization (Giant Trunade & Heavy Storm)
            AddExecutor(ExecutorType.Activate, CardId.GiantTrunade, GiantTrunadeEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);

            // 2. High-Octane Draw & Advantage
            AddExecutor(ExecutorType.Activate, CardId.SinisterSerpent, SinisterSerpentEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);
            AddExecutor(ExecutorType.Activate, CardId.DelinquentDuo, DelinquentDuoEffect);
            AddExecutor(ExecutorType.Activate, CardId.CardDestruction, CardDestructionEffect);

            // 3. Token Generation for Monster Gate
            AddExecutor(ExecutorType.Activate, CardId.Scapegoat, ScapegoatEffect);

            // 4. Core Excavation Engines: Reasoning & Monster Gate
            AddExecutor(ExecutorType.Activate, CardId.Reasoning, ReasoningEffect);
            AddExecutor(ExecutorType.Activate, CardId.MonsterGate, MonsterGateEffect);

            // 5. Dimension Fusion Swarm Finisher
            AddExecutor(ExecutorType.Activate, CardId.DimensionFusion, DimensionFusionEffect);

            // 6. Spell Recursion & Reincarnation
            AddExecutor(ExecutorType.Activate, CardId.DarkMagicianOfChaos, DmocEffect);
            AddExecutor(ExecutorType.Activate, CardId.SpellReproduction, SpellReproductionEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrematureBurial, PrematureBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.CallOfTheHaunted, CallOfTheHauntedEffect);

            // 7. Chaos Boss Summons
            AddExecutor(ExecutorType.SpSummon, CardId.BlackLusterSoldierEnvoyOfTheBeginning, BlsSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlackLusterSoldierEnvoyOfTheBeginning, BlsEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosSorcerer, ChaosSorcererSummon);
            AddExecutor(ExecutorType.Activate, CardId.ChaosSorcerer, ChaosSorcererEffect);

            // 8. Sacred Crane Draw Trigger
            AddExecutor(ExecutorType.Activate, CardId.SacredCrane);

            // 9. Board Control / Removal
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);
            AddExecutor(ExecutorType.Activate, CardId.NoblemanOfCrossout, NoblemanOfCrossoutEffect);

            // 10. Normal Summons & Sets
            AddExecutor(ExecutorType.Summon, CardId.SacredCrane);
            AddExecutor(ExecutorType.Summon, CardId.DdWarriorLady);
            AddExecutor(ExecutorType.Activate, CardId.DdWarriorLady, DdwlEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagicianOfFaith);
            AddExecutor(ExecutorType.MonsterSet, CardId.MagicianOfFaith);
            AddExecutor(ExecutorType.MonsterSet, CardId.Sangan);
            AddExecutor(ExecutorType.MonsterSet, CardId.SinisterSerpent);

            // 11. Hand Refresh
            AddExecutor(ExecutorType.Activate, CardId.Reload, ReloadEffect);

            // 12. Traps & Quick Spells
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonEffect);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, DefaultTorrentialTribute);

            // 13. Setting Backrow
            AddExecutor(ExecutorType.SpellSet, CardId.BookOfMoon);
            AddExecutor(ExecutorType.SpellSet, CardId.Scapegoat);
            AddExecutor(ExecutorType.SpellSet, CardId.CallOfTheHaunted);
            AddExecutor(ExecutorType.SpellSet, CardId.TorrentialTribute);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            return true; // Go first to set up board, rip hand, or unleash Reasoning
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXECUTION LOGIC
        // ═══════════════════════════════════════════════════════════════

        private bool GiantTrunadeEffect()
        {
            // Bounce enemy backrow before big push, or bounce our Snatch Steal / Premature Burial
            return Enemy.GetSpellCount() >= 1 || Bot.GetSpells().Any(s => s != null && (s.Id == CardId.PrematureBurial || s.Id == CardId.SnatchSteal));
        }

        private bool HeavyStormEffect()
        {
            int enemySpells = Enemy.GetSpellCount();
            int ourSpells = Bot.GetSpellCount();
            return enemySpells >= 2 && enemySpells > ourSpells;
        }

        private bool SinisterSerpentEffect()
        {
            return Card.Location == CardLocation.Grave;
        }

        private bool DelinquentDuoEffect()
        {
            return Bot.LifePoints > 1000 && Enemy.Hand.Count >= 2;
        }

        private bool CardDestructionEffect()
        {
            // Activate if hand has Sinister Serpent or mostly dead cards and we want fresh fuel
            return Bot.Hand.Count >= 3 && Enemy.Hand.Count >= 2;
        }

        private bool ScapegoatEffect()
        {
            // In Reasoning Gate: activate in our turn if we need tribute fodder for Monster Gate!
            if (Duel.Player == 0)
            {
                return Bot.HasInHand(CardId.MonsterGate) && Bot.GetMonsterCount() == 0;
            }

            if (Duel.Phase == DuelPhase.End) return true;
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
            {
                return Bot.GetMonsterCount() == 0 || Enemy.GetMonsters().Any(m => m.IsAttack() && m.Attack >= 1500);
            }
            return false;
        }

        private bool ReasoningEffect()
        {
            // Activate when we have deck space and monster zone space
            return Bot.GetMonsterCount() < 5;
        }

        private bool MonsterGateEffect()
        {
            if (Bot.GetMonsterCount() == 0 || Bot.GetMonsterCount() >= 5) return false;

            // Pick tribute from field (Tokens > Sinister > Faith > Sangan > Crane)
            var candidates = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (candidates.Count == 0) return false;

            var tribute = DeckPlugin?.MaterialEvaluator?.SortMaterials(candidates).FirstOrDefault();
            if (tribute != null && tribute.Id != CardId.DarkMagicianOfChaos && tribute.Id != CardId.Jinzo && tribute.Id != CardId.BlackLusterSoldierEnvoyOfTheBeginning)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return false;
        }

        private bool DimensionFusionEffect()
        {
            if (Bot.LifePoints <= 2000) return false;

            int ourBanishedMonsters = Bot.Banished.Count(c => c != null && c.IsMonster());
            int enemyBanishedMonsters = Enemy.Banished.Count(c => c != null && c.IsMonster());

            bool hasBoss = Bot.Banished.Any(c => c != null && (c.Id == CardId.DarkMagicianOfChaos || c.Id == CardId.Jinzo || c.Id == CardId.BlackLusterSoldierEnvoyOfTheBeginning));

            return (ourBanishedMonsters >= 2 || hasBoss) && ourBanishedMonsters >= enemyBanishedMonsters;
        }

        private bool DmocEffect()
        {
            var target = DeckPlugin?.Strategy?.PickSearchTarget(Bot.Graveyard.Where(c => c.IsSpell()).ToList(), Card);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SpellReproductionEffect()
        {
            // Only if we have 2 other spells in hand to discard
            if (Bot.Hand.Count(c => c != Card && c.IsSpell()) < 2) return false;

            var target = Bot.Graveyard.FirstOrDefault(c => c != null && (c.Id == CardId.DimensionFusion || c.Id == CardId.PotOfGreed || c.Id == CardId.MonsterGate || c.Id == CardId.GracefulCharity));
            if (target != null)
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

        private bool DdwlEffect()
        {
            if (Enemy.BattlingMonster != null && (Enemy.BattlingMonster.Attack >= 1500 || IsAceCard(Enemy.BattlingMonster)))
                return true;
            return false;
        }

        private bool ReloadEffect()
        {
            // Activate if hand is clogged with unplayable cards
            return Bot.Hand.Count >= 3 && !Bot.HasInHand(CardId.Reasoning) && !Bot.HasInHand(CardId.MonsterGate) && !Bot.HasInHand(CardId.PotOfGreed);
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

            // Search / Retrieval hint (506: HINTMSG_ATOHAND): Delegate to Domain Plugin
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
