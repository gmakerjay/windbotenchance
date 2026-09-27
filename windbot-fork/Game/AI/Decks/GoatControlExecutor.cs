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
    [Deck("GoatControl", "GoatControl")]
    public class GoatControlExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Main Monsters
            public const int BlackLusterSoldierEnvoyOfTheBeginning = 72989439;
            public const int AirknightParshath = 18036057;
            public const int Jinzo = 77585513;
            public const int BreakerTheMagicalWarrior = 71413901;
            public const int DdWarriorLady = 7572887;
            public const int TribeInfectingVirus = 33184167;
            public const int SinisterSerpent = 8131171;
            public const int MagicianOfFaith = 31560081;
            public const int Tsukuyomi = 34853266;
            public const int Sangan = 26202165;
            public const int MorphingJar = 33508719;
            public const int SpiritReaper = 23205979;
            public const int AsuraPriest = 2134346;
            public const int ExiledForce = 74131780;

            // Spells
            public const int PotOfGreed = 55144522;
            public const int GracefulCharity = 79571449;
            public const int DelinquentDuo = 44763025;
            public const int SnatchSteal = 45986603;
            public const int PrematureBurial = 70828912;
            public const int HeavyStorm = 19613556;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int NoblemanOfCrossout = 71044499;
            public const int Scapegoat = 73915051;
            public const int Metamorphosis = 46411259;
            public const int BookOfMoon = 14087893;

            // Traps
            public const int MirrorForce = 44095762;
            public const int TorrentialTribute = 53582587;
            public const int RingOfDestruction = 83555666;
            public const int CallOfTheHaunted = 97077563;
            public const int SakuretsuArmor = 56120475;

            // Extra Deck Fusions
            public const int ThousandEyesRestrict = 63519819;
            public const int DarkBalterTheTerrible = 80071763;
            public const int RyuSenshi = 49868263;
            public const int FiendSkullDragon = 66235877;
            public const int KingDragun = 13756293;
            public const int GatlingDragon = 87751584;
            public const int OjamaKing = 90140980;
            public const int ReaperOnTheNightmare = 85684223;
            public const int DarkFlareKnight = 13722870;
            public const int Sanwitch = 53539634;

            // Tokens
            public const int SheepToken = 73915052;
        }

        private static readonly int[] BossMonsters =
        {
            CardId.BlackLusterSoldierEnvoyOfTheBeginning,
            CardId.Jinzo,
            CardId.ThousandEyesRestrict,
            CardId.AirknightParshath
        };

        private ClientCard _tsukuyomiTarget = null;

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public GoatControlExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // 0. Install Decoupled Domain Plugin
            DeckPlugin = new GoatControlPlugin(this);

            // 1. Hand Disruption & Power Draws
            AddExecutor(ExecutorType.Activate, CardId.SinisterSerpent, SinisterSerpentEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.GracefulCharity);
            AddExecutor(ExecutorType.Activate, CardId.DelinquentDuo, DelinquentDuoEffect);

            // 2. Removal Spells & Board Wipes
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.NoblemanOfCrossout, NoblemanOfCrossoutEffect);
            AddExecutor(ExecutorType.Activate, CardId.SnatchSteal, SnatchStealEffect);
            AddExecutor(ExecutorType.Activate, CardId.PrematureBurial, PrematureBurialEffect);
            AddExecutor(ExecutorType.Activate, CardId.CallOfTheHaunted, CallOfTheHauntedEffect);

            // 3. Thousand-Eyes Restrict & Metamorphosis Engine
            AddExecutor(ExecutorType.Activate, CardId.Metamorphosis, MetamorphosisEffect);
            AddExecutor(ExecutorType.Activate, CardId.ThousandEyesRestrict, TerEffect);

            // 4. Tsukuyomi Loop & Flip Re-activators
            AddExecutor(ExecutorType.Summon, CardId.Tsukuyomi, TsukuyomiSummon);
            AddExecutor(ExecutorType.Activate, CardId.Tsukuyomi, TsukuyomiEffect);

            // 5. BLS Summon & Effects
            AddExecutor(ExecutorType.SpSummon, CardId.BlackLusterSoldierEnvoyOfTheBeginning, BlsSummon);
            AddExecutor(ExecutorType.Activate, CardId.BlackLusterSoldierEnvoyOfTheBeginning, BlsEffect);

            // 6. Tribe-Infecting Virus & Exiled Force
            AddExecutor(ExecutorType.Activate, CardId.ExiledForce, ExiledForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TribeInfectingVirus, TribeInfectingVirusEffect);

            // 7. Tribute Summons
            AddExecutor(ExecutorType.Summon, CardId.Jinzo, JinzoTributeSummon);
            AddExecutor(ExecutorType.Summon, CardId.AirknightParshath, AirknightTributeSummon);

            // 8. Normal Beatdown Summons
            AddExecutor(ExecutorType.Summon, CardId.BreakerTheMagicalWarrior, BreakerSummon);
            AddExecutor(ExecutorType.Activate, CardId.BreakerTheMagicalWarrior, BreakerEffect);
            AddExecutor(ExecutorType.Summon, CardId.DdWarriorLady, DdwlSummon);
            AddExecutor(ExecutorType.Activate, CardId.DdWarriorLady, DdwlEffect);
            AddExecutor(ExecutorType.Summon, CardId.AsuraPriest, AsuraPriestSummon);
            AddExecutor(ExecutorType.Summon, CardId.TribeInfectingVirus, TribeInfectingVirusSummon);

            // 9. Flip Flops & Sets
            AddExecutor(ExecutorType.Activate, CardId.MagicianOfFaith);
            AddExecutor(ExecutorType.Activate, CardId.MorphingJar);
            AddExecutor(ExecutorType.MonsterSet, CardId.MagicianOfFaith);
            AddExecutor(ExecutorType.MonsterSet, CardId.MorphingJar);
            AddExecutor(ExecutorType.MonsterSet, CardId.Sangan);
            AddExecutor(ExecutorType.MonsterSet, CardId.SpiritReaper);
            AddExecutor(ExecutorType.MonsterSet, CardId.SinisterSerpent);

            // 10. Quick-Play & Defensive Traps
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonEffect);
            AddExecutor(ExecutorType.Activate, CardId.Scapegoat, ScapegoatEffect);
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, DefaultTorrentialTribute);
            AddExecutor(ExecutorType.Activate, CardId.RingOfDestruction, RingOfDestructionEffect);
            AddExecutor(ExecutorType.Activate, CardId.SakuretsuArmor, SakuretsuArmorEffect);

            // 11. Spell/Trap Setting
            AddExecutor(ExecutorType.SpellSet, CardId.Scapegoat);
            AddExecutor(ExecutorType.SpellSet, CardId.BookOfMoon);
            AddExecutor(ExecutorType.SpellSet, CardId.MirrorForce);
            AddExecutor(ExecutorType.SpellSet, CardId.TorrentialTribute);
            AddExecutor(ExecutorType.SpellSet, CardId.RingOfDestruction);
            AddExecutor(ExecutorType.SpellSet, CardId.CallOfTheHaunted);
            AddExecutor(ExecutorType.SpellSet, CardId.SakuretsuArmor);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            return true; // First turn advantage in GOAT (Delinquent Duo, Pot of Greed, Set Faith)
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _tsukuyomiTarget = null;
        }

        // ═══════════════════════════════════════════════════════════════
        //  EXECUTION LOGIC
        // ═══════════════════════════════════════════════════════════════

        private bool SinisterSerpentEffect()
        {
            return Card.Location == CardLocation.Grave;
        }

        private bool DelinquentDuoEffect()
        {
            return Bot.LifePoints > 1000 && Enemy.Hand.Count >= 2;
        }

        private bool HeavyStormEffect()
        {
            int enemySpells = Enemy.GetSpellCount();
            int ourSpells = Bot.GetSpellCount();
            // Don't blow up our own Premature Burial / Snatch Steal / face-down traps unnecessarily
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
            // Activate during opponent's End Phase or during our turn for big push
            if (Duel.Player == 1 && Duel.Phase != DuelPhase.End) return false;
            var target = DeckPlugin?.Strategy?.PickSpecialSummonTarget(Bot.Graveyard.Where(c => c.IsMonster()).ToList());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool MetamorphosisEffect()
        {
            var monsters = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (monsters.Count == 0) return false;

            // 1. Level 1 Tribute (Sheep Token or Sinister Serpent) -> Thousand-Eyes Restrict
            var lv1 = monsters.FirstOrDefault(m => m.Level == 1 && (m.Id == CardId.SheepToken || m.Id == CardId.SinisterSerpent));
            if (lv1 != null && !Bot.HasInMonstersZone(CardId.ThousandEyesRestrict) && Enemy.GetMonsterCount() > 0)
            {
                AI.SelectCard(lv1);
                AI.SelectNextCard(CardId.ThousandEyesRestrict);
                return true;
            }

            // 2. Level 5 Tribute (Airknight) -> Dark Balter the Terrible (Spell negate)
            var lv5 = monsters.FirstOrDefault(m => m.Level == 5);
            if (lv5 != null && Enemy.GetSpellCount() >= 2)
            {
                AI.SelectCard(lv5);
                AI.SelectNextCard(CardId.DarkBalterTheTerrible);
                return true;
            }

            // 3. Level 6 Tribute (Jinzo / Sanwitch) -> Ryu Senshi (Trap negate)
            var lv6 = monsters.FirstOrDefault(m => m.Level == 6 && m.Id != CardId.Jinzo);
            if (lv6 != null)
            {
                AI.SelectCard(lv6);
                AI.SelectNextCard(CardId.RyuSenshi);
                return true;
            }

            // 4. Level 8 Tribute (BLS that attacked or is targeted) -> Gatling Dragon
            var lv8 = monsters.FirstOrDefault(m => m.Level == 8);
            if (lv8 != null && Enemy.GetMonsterCount() >= 2)
            {
                AI.SelectCard(lv8);
                AI.SelectNextCard(CardId.GatlingDragon);
                return true;
            }

            return false;
        }

        private bool TerEffect()
        {
            // TER can only have 1 monster equipped at a time
            if (Card.EquipCards != null && Card.EquipCards.Count > 0)
                return false;

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

        private bool TsukuyomiSummon()
        {
            // Loop 1: TER is equipped -> flip it down to destroy the equip card and prepare for another suck
            var equippedTer = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Id == CardId.ThousandEyesRestrict && m.EquipCards != null && m.EquipCards.Count > 0);
            if (equippedTer != null)
            {
                _tsukuyomiTarget = equippedTer;
                return true;
            }

            // Loop 2: Magician of Faith is face-up -> flip it down to reuse its spell recursion
            var faceupFaith = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Id == CardId.MagicianOfFaith);
            if (faceupFaith != null)
            {
                _tsukuyomiTarget = faceupFaith;
                return true;
            }

            // Battle interaction: Enemy monster with high ATK but low DEF (can beat over with 1100 ATK or other monster)
            int ourMaxAtk = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Select(m => m.Attack).DefaultIfEmpty(1100).Max();
            var flippableEnemy = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && m.Defense <= Math.Max(1100, ourMaxAtk) && m.Attack >= 1500 && !IsTargetImmune(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (flippableEnemy != null)
            {
                _tsukuyomiTarget = flippableEnemy;
                return true;
            }

            return false;
        }

        private bool TsukuyomiEffect()
        {
            if (_tsukuyomiTarget != null)
            {
                AI.SelectCard(_tsukuyomiTarget);
                _tsukuyomiTarget = null;
                return true;
            }

            var fallback = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && (m.Id == CardId.ThousandEyesRestrict || m.Id == CardId.MagicianOfFaith))
                           ?? Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).OrderByDescending(m => m.Attack).FirstOrDefault();

            if (fallback != null)
            {
                AI.SelectCard(fallback);
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

        private bool ExiledForceEffect()
        {
            var target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool TribeInfectingVirusSummon()
        {
            return Enemy.GetMonsterCount() >= 2 && Bot.Hand.Count >= 2;
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

        private bool JinzoTributeSummon()
        {
            var tribute = Bot.GetMonsters().FirstOrDefault(m => m != null && (m.Id == CardId.SheepToken || m.Id == CardId.SinisterSerpent || (m.Id == CardId.MagicianOfFaith && m.IsFaceup())));
            if (tribute != null)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return false;
        }

        private bool AirknightTributeSummon()
        {
            var tribute = Bot.GetMonsters().FirstOrDefault(m => m != null && (m.Id == CardId.SheepToken || m.Id == CardId.SinisterSerpent || (m.Id == CardId.MagicianOfFaith && m.IsFaceup())));
            if (tribute != null && Enemy.GetMonsterCount() > 0)
            {
                AI.SelectCard(tribute);
                return true;
            }
            return false;
        }

        private bool BreakerSummon() => true;

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

        private bool DdwlSummon() => true;

        private bool DdwlEffect()
        {
            // Banish self and battling monster if opponent monster is >= 1500 ATK or dangerous boss
            if (Enemy.BattlingMonster != null && (Enemy.BattlingMonster.Attack >= 1500 || IsAceCard(Enemy.BattlingMonster)))
                return true;
            return false;
        }

        private bool AsuraPriestSummon()
        {
            // Summon when opponent has multiple monsters (especially tokens or swarms)
            return Enemy.GetMonsterCount() >= 2 || (Enemy.GetMonsterCount() == 1 && Enemy.GetMonsters().First().Attack < 1700);
        }

        private bool BookOfMoonEffect()
        {
            // Flip down opponent attacker in battle phase
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

            // Flip down TER to reset its suck effect
            var equippedTer = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Id == CardId.ThousandEyesRestrict && m.EquipCards != null && m.EquipCards.Count > 0);
            if (equippedTer != null && Duel.Player == 0)
            {
                AI.SelectCard(equippedTer);
                return true;
            }

            return false;
        }

        private bool ScapegoatEffect()
        {
            // Our turn: Only activate if we have Metamorphosis to summon Thousand-Eyes Restrict!
            if (Duel.Player == 0)
            {
                return Bot.HasInHand(CardId.Metamorphosis) && Bot.GetMonsterCount() <= 1 && Enemy.GetMonsterCount() > 0;
            }

            // Opponent turn: End Phase activation
            if (Duel.Phase == DuelPhase.End)
                return true;

            // Opponent turn: Emergency survival during battle
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

        private bool MonsterRepos()
        {
            return DefaultMonsterRepos();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Enforce Strict Rule 1: NEVER destroy or remove own cards if enemy cards can be chosen!
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
