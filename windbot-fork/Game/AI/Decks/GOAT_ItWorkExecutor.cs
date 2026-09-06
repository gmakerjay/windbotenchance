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
    // GOAT_ItWork (Solar Flare Stall/Burn)
    // ==========================================
    [Deck("GOAT_ItWork", "GOAT_ItWork")]
    public class GOAT_ItWorkExecutor : ModernExecutor
    {
        public class CardId
        {
            public const int MorphingJar = 33508719;
            public const int SolarFlareDragon = 45985838;
            public const int UfoTurtle = 60806437;
            public const int RagingFlameSprite = 90810762;
            public const int StealthBird = 3510565;
            public const int NeedleWorm = 81843628;
            public const int GravityBind = 85742772;
            public const int BottomlessTrapHole = 29401950;
            public const int RingOfDestruction = 83555666;
            public const int OjamaTrio = 29843091;
            public const int MagicCylinder = 62279055;
            public const int JustDesserts = 24068492;
            public const int SecretBarrel = 27053506;
            public const int CurseOfAnubis = 66742250;
            public const int JudgmentOfAnubis = 55256016;
            public const int CompulsoryEvacuationDevice = 94192409;
            public const int DustTornado = 60082869;
            public const int Ceasefire = 36468556;
            public const int TorrentialTribute = 53582587;
            public const int MirrorForce = 44095762;
            public const int TrapDustshoot = 64697231;
            public const int HeavyStorm = 19613556;
            public const int CreatureSwap = 31036355;
            public const int PotOfGreed = 55144522;
            public const int GiantTrunade = 42703248;
            public const int ChainEnergy = 79323590;
            public const int Scapegoat = 73915051;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int DelinquentDuo = 44763025;
            public const int UpstartGoblin = 70368879;
            public const int LevelLimitAreaB = 3136426;
            public const int SwordsOfRevealingLight = 72302403;
            public const int BookOfMoon = 14087893;
            public const int BrainControl = 87910978;
        }



        private static readonly int[] BossMonsters = {
            CardId.SolarFlareDragon,
            CardId.StealthBird
        };

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && BossMonsters.Contains(card.Id);
        }

        public GOAT_ItWorkExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── Chain Negations / Hand Traps / Counter Spells ──
            AddExecutor(ExecutorType.Activate, CardId.JudgmentOfAnubis, JudgmentOfAnubisEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrapDustshoot, TrapDustshootEffect);

            // ── Draw & Search ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfGreed);
            AddExecutor(ExecutorType.Activate, CardId.UpstartGoblin);
            AddExecutor(ExecutorType.Activate, CardId.DelinquentDuo, DelinquentDuoEffect);

            // ── Backrow Removal ──
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.DustTornado, DustTornadoEffect);
            AddExecutor(ExecutorType.Activate, CardId.HeavyStorm, HeavyStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.GiantTrunade, GiantTrunadeEffect);

            // ── Board Control / Stall Setup ──
            AddExecutor(ExecutorType.Activate, CardId.SwordsOfRevealingLight);
            AddExecutor(ExecutorType.Activate, CardId.LevelLimitAreaB, LevelLimitAreaBEffect);
            AddExecutor(ExecutorType.Activate, CardId.ChainEnergy, ChainEnergyEffect);

            // ── Spells / Mind Control ──
            AddExecutor(ExecutorType.Activate, CardId.BrainControl, BrainControlEffect);
            AddExecutor(ExecutorType.Activate, CardId.CreatureSwap, CreatureSwapEffect);
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonEffect);

            // ── Monsters / Flip Effects ──
            AddExecutor(ExecutorType.Activate, CardId.StealthBird, StealthBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.MorphingJar);
            AddExecutor(ExecutorType.Activate, CardId.NeedleWorm);
            AddExecutor(ExecutorType.Activate, CardId.RagingFlameSprite);
            AddExecutor(ExecutorType.Activate, CardId.SolarFlareDragon);

            // ── Normal Summons / Set Monsters ──
            AddExecutor(ExecutorType.MonsterSet, CardId.StealthBird);
            AddExecutor(ExecutorType.MonsterSet, CardId.MorphingJar);
            AddExecutor(ExecutorType.MonsterSet, CardId.NeedleWorm);
            AddExecutor(ExecutorType.Summon, CardId.SolarFlareDragon);
            AddExecutor(ExecutorType.Summon, CardId.UfoTurtle);
            AddExecutor(ExecutorType.Summon, CardId.RagingFlameSprite);
            AddExecutor(ExecutorType.MonsterSet, CardId.UfoTurtle);

            // ── Disruption / Burn Traps (Respond on chain/attacks) ──
            AddExecutor(ExecutorType.Activate, CardId.OjamaTrio, OjamaTrioEffect);
            AddExecutor(ExecutorType.Activate, CardId.GravityBind, GravityBindEffect);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, DefaultTorrentialTribute);
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.MagicCylinder);
            AddExecutor(ExecutorType.Activate, CardId.RingOfDestruction, RingOfDestructionEffect);
            AddExecutor(ExecutorType.Activate, CardId.BottomlessTrapHole);
            AddExecutor(ExecutorType.Activate, CardId.CompulsoryEvacuationDevice, DefaultCompulsoryEvacuationDevice);
            AddExecutor(ExecutorType.Activate, CardId.CurseOfAnubis, CurseOfAnubisEffect);
            AddExecutor(ExecutorType.Activate, CardId.Scapegoat, ScapegoatEffect);

            // ── Direct Burn Traps ──
            AddExecutor(ExecutorType.Activate, CardId.Ceasefire, CeasefireEffect);
            AddExecutor(ExecutorType.Activate, CardId.JustDesserts, JustDessertsEffect);
            AddExecutor(ExecutorType.Activate, CardId.SecretBarrel, SecretBarrelEffect);

            // ── Hand / Graveyard / Reposition ──
            AddExecutor(ExecutorType.SpellSet, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
        }

        private bool JudgmentOfAnubisEffect()
        {
            var lastCard = Util.GetLastChainCard();
            if (lastCard?.Controller == 1 && (lastCard.IsCode(CardId.HeavyStorm) || lastCard.IsCode(CardId.MysticalSpaceTyphoon) || lastCard.IsCode(CardId.DustTornado) || lastCard.IsCode(CardId.GiantTrunade)))
            {
                return true;
            }
            return false;
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
            // Don't destroy our own stall cards if they're keeping us alive
            bool haveStallCards = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.GravityBind) || c.IsCode(CardId.LevelLimitAreaB) || c.IsCode(CardId.SwordsOfRevealingLight)));
            int ourImportantCount = Bot.GetSpells().Count(c => c != null && c.IsFaceup() && (c.IsCode(CardId.GravityBind) || c.IsCode(CardId.LevelLimitAreaB) || c.IsCode(CardId.SwordsOfRevealingLight) || c.IsCode(CardId.ChainEnergy)));
            // Only play if opponent has significantly more backrow AND our stall cards aren't critical
            if (haveStallCards && Enemy.GetMonsterCount() > 0) return false; // stall cards are protecting us
            return Enemy.GetSpellCount() > Bot.GetSpellCount() && Enemy.GetSpellCount() >= 2;
        }

        private bool GiantTrunadeEffect()
        {
            // Bounce all backrow to clear for an attack, or to reuse our continuous cards
            return Enemy.GetSpellCount() >= 2 || (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.GravityBind) || c.IsCode(CardId.LevelLimitAreaB))) && Enemy.GetMonsterCount() == 0);
        }

        private bool LevelLimitAreaBEffect()
        {
            return !Bot.HasInSpellZone(CardId.LevelLimitAreaB);
        }

        private bool ChainEnergyEffect()
        {
            return !Bot.HasInSpellZone(CardId.ChainEnergy);
        }

        private bool BrainControlEffect()
        {
            // Take high ATK enemy monster for tribute or attacking
            ClientCard best = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && c.IsAttack()).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (best != null)
            {
                AI.SelectCard(best);
                return true;
            }
            return false;
        }

        private bool CreatureSwapEffect()
        {
            // Give the weakest monster we have and take their strongest
            if (Enemy.GetMonsterCount() == 0) return false;
            bool enemyHasStrong = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 1500);
            if (!enemyHasStrong) return false;
            // Select our weakest face-up monster to give away
            var weakest = Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup())
                .OrderBy(c => c.Attack)
                .FirstOrDefault();
            if (weakest != null && weakest.Attack < 1500)
            {
                AI.SelectCard(weakest);
                return true;
            }
            return false;
        }

        private bool BookOfMoonEffect()
        {
            // Priority 1: Flip down enemy attacker during battle phase
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep))
            {
                ClientCard enemyAttacker = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack() && c.Attack >= 1500)
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (enemyAttacker != null)
                {
                    AI.SelectCard(enemyAttacker);
                    return true;
                }
            }

            // Priority 2: Flip our flip-effect monsters face-down to reuse
            // StealthBird: flip down so it can flip summon again for burn
            // MorphingJar/NeedleWorm: flip down to reuse flip effect
            ClientCard flipTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                (c.IsCode(CardId.MorphingJar) || c.IsCode(CardId.NeedleWorm) || c.IsCode(CardId.StealthBird)));
            if (flipTarget != null)
            {
                AI.SelectCard(flipTarget);
                return true;
            }

            // Priority 3: Flip down a threatening enemy monster in main phase
            ClientCard enemyThreat = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && c.Attack >= 1500)
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            if (enemyThreat != null)
            {
                AI.SelectCard(enemyThreat);
                return true;
            }
            return false;
        }

        private bool StealthBirdEffect()
        {
            return Card.IsFaceup();
        }

        private bool OjamaTrioEffect()
        {
            // Need at least 3 empty zones to summon all 3 tokens
            int emptyZones = 5 - Enemy.GetMonsterCount();
            if (emptyZones < 3) return false;
            // Activate if: 1) We have burn traps to combo, OR 2) Lock their zones (always good for stall)
            bool hasBurnCombo = Bot.HasInHand(CardId.JustDesserts) || Bot.HasInHand(CardId.SecretBarrel) ||
                Bot.GetSpells().Any(c => c != null && (c.IsCode(CardId.JustDesserts) || c.IsCode(CardId.SecretBarrel) || c.IsCode(CardId.Ceasefire)));
            return hasBurnCombo || Enemy.GetMonsterCount() == 0;
        }

        private bool GravityBindEffect()
        {
            return !Bot.HasInSpellZone(CardId.GravityBind);
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

        private bool CurseOfAnubisEffect()
        {
            // Switch all effect monsters to DEF when opponent attacks
            return Duel.Player == 1 && Duel.Phase == DuelPhase.BattleStart;
        }

        private bool ScapegoatEffect()
        {
            // Only activate on opponent's turn (to not block our Normal Summon) or if about to take lethal
            if (Duel.Player == 0) return false; // Don't use on our turn
            return DefaultScapegoat();
        }

        private bool CeasefireEffect()
        {
            // Play if there are multiple face-down monsters on field or many effect monsters
            int count = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Count(c => c != null && (c.IsFacedown() || c.HasType(CardType.Effect)));
            return count >= 3;
        }

        private bool JustDessertsEffect()
        {
            // Burn based on number of opponent's monsters
            return Enemy.GetMonsterCount() >= 3 || (Enemy.GetMonsterCount() >= 1 && Enemy.LifePoints <= Enemy.GetMonsterCount() * 500);
        }

        private bool SecretBarrelEffect()
        {
            // Burn based on opponent's hand + field size
            int count = Enemy.Hand.Count + Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            return count >= 6 || (count >= 1 && Enemy.LifePoints <= count * 200);
        }

        private bool SetTrapCondition()
        {
            if (Card.HasType(CardType.Field)) return false;
            // Set all trap cards and quick-play spells
            return Card.HasType(CardType.Trap) || (Card.HasType(CardType.Spell) && Card.HasType(CardType.QuickPlay));
        }

        private bool MonsterRepos()
        {
            // Lock active check: don't attack if Gravity Bind or Level Limit is active unless it's Raging Flame Sprite (under 1500 ATK or can attack directly)
            bool isLocked = Bot.HasInSpellZone(CardId.GravityBind) || Bot.HasInSpellZone(CardId.LevelLimitAreaB) || Enemy.HasInSpellZone(CardId.GravityBind) || Enemy.HasInSpellZone(CardId.LevelLimitAreaB);
            
            if (Card.IsCode(CardId.RagingFlameSprite) && Card.IsFaceup())
            {
                if (Card.IsDefense()) return true; // Flip to attack
                return false;
            }

            if (Card.IsCode(CardId.StealthBird) && Card.IsFacedown())
            {
                // Flip summon to trigger burn
                return true;
            }

            if (isLocked)
            {
                if (Card.IsAttack() && Card.Level >= 4) return true; // Switch to defense
                return false;
            }

            return DefaultMonsterRepos();
        }

        private bool DustTornadoEffect()
        {
            return DefaultMysticalSpaceTyphoon();
        }

        private bool MirrorForceEffect()
        {
            if (Duel.Player != 1) return false;
            // Only use on meaningful attacks (ATK >= 1000 or multiple attackers)
            if (Enemy.BattlingMonster != null && Enemy.BattlingMonster.Attack >= 1000) return true;
            if (Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsAttack()) >= 2) return true;
            return false;
        }
    }
}
