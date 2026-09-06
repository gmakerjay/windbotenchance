// ============================================================
// CARD AUDIT โ€” 2026 Radiant
// ============================================================
// | Card Name           | Type       | OPT? | Effect Summary           | Activate When                        | NEVER Activate When                          |
// |---------------------|------------|------|--------------------------|--------------------------------------|----------------------------------------------|
// | Krosea              | Monster    | Yes  | Search Radiant/MST       | Normal/Special Summoned              | WIND lock would disrupt Runick fusions       |
// | Eldam               | Monster    | Yes  | Search Radiant monster   | Normal/Special Summoned              | Already searched this turn                   |
// | Swen                | Monster    | Yes  | Search Radiant S/T       | Normal/Special Summoned              | Already searched this turn                   |
// | Meghala             | Monster    | Yes  | SS from hand / turn Def  | WIND on field / declare attack       | Already used this turn                       |
// | Vibrant Vortex      | Monster    | Yes  | Negate monster / SS      | Opponent monster effect / MST / S/T  | No MST in GY for negation                    |
// | Shiina              | Monster    | Yes  | SS, bounce field cards   | Opponent activates card while WIND   | We do not control WIND monster               |
// | Majesty's Fiend     | Monster    | No   | Negate all monster effs  | Tribute summon setup ready           | We need to activate key monster effects      |
// | Vision              | Spell      | Yes  | Draw 2 when popped       | Destroyed by MST                     | No cards in hand to discard (if draw 2 fails)|
// | Chant               | Spell      | Yes  | Search Lv4- when popped  | Destroyed by MST                     | Already searched this turn                   |
// | Ascendance          | Spell      | Yes  | Revive Lv6- when popped  | Destroyed by MST                     | GY has no valid target                       |
// | Mandate             | Trap       | Yes  | Recycle Spells, Negate   | MST activates / S/T in GY            | GY has no target / already used              |
// | Runick Fountain     | Field Spell| Yes  | Play from hand / Draw    | Runick spell activated               | Already drew this turn                       |
// | MST                 | Spell      | No   | Pop S/T                  | Target own Radiant Spells / enemy S/T| Target our own Runick Fountain               |
// ============================================================
// ACE CARDS:
//   Primary  : Radiant Typhoon Varuroon, the Vibrant Vortex โ€” Boss monster with negates and GY recurrence.
//   Secondary: Majesty's Fiend โ€” Lockout monster that completely shut down opponent monster effects.
// Priority: Win > Attack > Combo > Setup
// ============================================================

using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using System;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Radiant", "2026_Radiant")]
    public class _2026_RadiantExecutor : ModernExecutor
    {
        public class CardId
        {
            // Radiant Typhoon Main Deck
            public const int Krosea = 16922142;
            public const int Vision = 20508881;
            public const int Fonix = 85315450;
            public const int Eldam = 54143349;
            public const int Chant = 67115133;
            public const int Ascendance = 25940932;
            public const int Mandate = 53813120;
            public const int VibrantVortex = 53927851;
            public const int Swen = 80538047;
            public const int Meghala = 27755794;

            // Runick Main Deck
            public const int RunickFlashingFire = 68957034;
            public const int RunickFreezingCurses = 30430448;
            public const int RunickSlumber = 67835547;
            public const int RunickTip = 31562086;
            public const int RunickFountain = 92107604;
            public const int RunickDestruction = 94445733;
            public const int RunickDispelling = 66712905;
            public const int RunickSmitingStorm = 93229151;

            // Staples & Hand Traps
            public const int MysticalSpaceTyphoon = 5318639;
            public const int Shiina = 12197223;
            public const int MajestysFiend = 33746252;
            public const int DrollAndLockBird = 94145021;
            public const int MaxxC = 23434538;
            public const int CalledByTheGrave = 24224830;

            // Extra Deck
            public const int HuginTheRunickWings = 55990317;
            public const int GeriTheRunickFangs = 28373620;
            public const int SleipnirTheRunickMane = 74659582;
            public const int LinkVaruroon = 39341885;
            public const int TotemBird = 71068247;
            public const int CicadaKing = 4997565;
            public const int Enterblathnir = 95113856;
            public const int SPLittleKnight = 29301450;
            public const int WynnTheWindCharmerVerdant = 30674956;
            public const int DoomEagle = 49105782;
            public const int Zeus = 90448279;
            public const int Typhon = 93039339;
            public const int GravityController = 23656668;
        }

        // --- GOING FIRST/SECOND TRACKING ---

        // Once Per Turn Flags
        private bool _kroseaHandUsed = false;
        private bool _kroseaSummonUsed = false;
        private bool _vibrantVortexHandUsed = false;
        private bool _vibrantVortexNegateUsed = false;
        private bool _eldamSummonUsed = false;
        private bool _swenSummonUsed = false;
        private bool _meghalaSpUsed = false;
        private bool _visionUsed = false;
        private bool _chantUsed = false;
        private bool _ascendanceUsed = false;
        private bool _mandateUsed = false;
        private bool _fountainUsed = false;
        private bool _linkVaruroonSummonUsed = false;
        private bool _linkVaruroonTrapPlaceUsed = false;

        // WIND lock tracker
        private bool _windLocked = false;

        private static readonly int[] AceCardIds = {
            CardId.VibrantVortex,
            CardId.LinkVaruroon,
            CardId.MajestysFiend
        };

        private static readonly int[] RunickSpellIds = {
            CardId.RunickFlashingFire,
            CardId.RunickFreezingCurses,
            CardId.RunickSlumber,
            CardId.RunickTip,
            CardId.RunickDestruction,
            CardId.RunickDispelling,
            CardId.RunickSmitingStorm
        };

        private static readonly int[] HandTrapIds = {
            CardId.DrollAndLockBird,
            CardId.MaxxC,
            CardId.CalledByTheGrave
        };

        // โ”€โ”€ Strategic Board / Resource Evaluation โ”€โ”€
        protected override bool IsBoardStrongEnough()
        {
            int disruptionCount = 0;

            // Link Varuroon = search + trap placement + board control
            if (Bot.HasInMonstersZone(CardId.LinkVaruroon))
                disruptionCount += 3;

            // Vibrant Vortex = negate (with MST in GY)
            if (Bot.HasInMonstersZone(CardId.VibrantVortex))
            {
                if (Bot.HasInGraveyard(CardId.MysticalSpaceTyphoon))
                    disruptionCount += 2;
                else
                    disruptionCount++;
            }

            // Xyz monsters
            if (Bot.HasInMonstersZone(CardId.TotemBird))
                disruptionCount += 2; // S/T negate
            if (Bot.HasInMonstersZone(CardId.CicadaKing))
                disruptionCount++;
            if (Bot.HasInMonstersZone(CardId.Enterblathnir))
                disruptionCount += 2;

            // Runick fusions
            if (Bot.HasInMonstersZone(CardId.HuginTheRunickWings))
                disruptionCount++;
            if (Bot.HasInMonstersZone(CardId.SleipnirTheRunickMane))
                disruptionCount += 2;

            // Set backrow + hand traps
            int setBackrow = Bot.GetSpells().Count(c => c != null && c.IsFacedown());
            disruptionCount += setBackrow / 2;

            if (Bot.HasInHand(CardId.MaxxC) || Bot.HasInHand(CardId.DrollAndLockBird))
                disruptionCount++;

            // Majesty's Fiend = full monster lock
            if (Bot.HasInMonstersZone(CardId.MajestysFiend))
                disruptionCount += 3;

            return disruptionCount >= 3;
        }

        protected override bool IsInGrindGame()
        {
            int handCount = Bot.GetHandCount();
            int monsterCount = Bot.GetMonsterCount();
            int setCount = Bot.GetSpells().Count(c => c != null && c.IsFacedown());

            bool lowResources = (handCount <= 1 && monsterCount <= 1 && setCount <= 1);
            bool highOpponentPressure = Enemy.GetMonsterCount() >= 3;

            return Duel.Turn >= 8 || lowResources || highOpponentPressure;
        }

        protected override bool NeedsBoardPresence()
        {
            int ourFaceup = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());
            return ourFaceup == 0 || (ourFaceup <= 1 && Enemy.GetMonsterCount() >= 2);
        }

        public _2026_RadiantExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.Krosea, CardId.Fonix },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Krosea, ActionType = ExecutorType.Activate, Description = "Play CardId.Krosea" },
                    new() { CardId = CardId.Fonix, ActionType = ExecutorType.Activate, Description = "Extend with CardId.Fonix" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.Krosea, CardId.Vision);
            BaitPlanner.RegisterBaitCards(CardId.Vision);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.Krosea, CardId.Vision, CardId.SPLittleKnight);

            // 1. Hand Traps & Negations (Tiers 1 & 2)
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.Shiina, ShiinaActivate);
            AddExecutor(ExecutorType.Activate, CardId.VibrantVortex, VibrantVortexActivate);
            AddExecutor(ExecutorType.Activate, CardId.Fonix, FonixActivate);

            // 2. Continuous S/T Activations & Recyclers
            AddExecutor(ExecutorType.Activate, CardId.RunickFountain, RunickFountainActivate);
            AddExecutor(ExecutorType.Activate, CardId.Mandate, MandateActivate);

            // 3. Runick Spells (Tier 3) - Prioritize summon before WIND lock
            AddExecutor(ExecutorType.Activate, CardId.RunickTip, RunickTipActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickFreezingCurses, RunickFreezingCursesActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickFlashingFire, RunickFlashingFireActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickSlumber, RunickSlumberActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickDestruction, RunickDestructionActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickDispelling, RunickDispellingActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickSmitingStorm, RunickSmitingStormActivate);

            // 4. MST Self-destruction (Tier 4)
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, MysticalSpaceTyphoonActivate);

            // 5. Radiant Spells/Traps Triggered by MST (Tier 5)
            AddExecutor(ExecutorType.Activate, CardId.Vision, VisionActivate);
            AddExecutor(ExecutorType.Activate, CardId.Chant, ChantActivate);
            AddExecutor(ExecutorType.Activate, CardId.Ascendance, AscendanceActivate);

            // 6. Normal / Special Summons of Starters (Tier 6)
            AddExecutor(ExecutorType.SpSummon, CardId.Eldam);
            AddExecutor(ExecutorType.SpSummon, CardId.Swen);
            AddExecutor(ExecutorType.SpSummon, CardId.Meghala);
            AddExecutor(ExecutorType.Activate, CardId.Krosea, KroseaActivate);
            AddExecutor(ExecutorType.Summon, CardId.Krosea, KroseaSummon);
            AddExecutor(ExecutorType.Activate, CardId.Eldam, EldamActivate);
            AddExecutor(ExecutorType.Summon, CardId.Eldam, EldamSummon);
            AddExecutor(ExecutorType.Activate, CardId.Swen, SwenActivate);
            AddExecutor(ExecutorType.Summon, CardId.Swen, SwenSummon);
            AddExecutor(ExecutorType.Activate, CardId.Meghala, MeghalaActivate);
            AddExecutor(ExecutorType.Summon, CardId.Meghala);

            // 7. Majesty's Fiend Tribute (Tier 7)
            AddExecutor(ExecutorType.Summon, CardId.MajestysFiend, MajestysFiendSummon);

            // 8. Extra Deck Bosses & Links (Tier 8)
            AddExecutor(ExecutorType.SpSummon, CardId.LinkVaruroon, LinkVaruroonSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.LinkVaruroon, LinkVaruroonActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.TotemBird, TotemBirdSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.TotemBird, TotemBirdActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.CicadaKing, CicadaKingSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.CicadaKing, CicadaKingActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Enterblathnir, EnterblathnirSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Enterblathnir, EnterblathnirActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.WynnTheWindCharmerVerdant, WynnTheWindCharmerVerdantSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.WynnTheWindCharmerVerdant, WynnTheWindCharmerVerdantActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.DoomEagle, DoomEagleSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.DoomEagle, DoomEagleActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Typhon, TyphonSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Typhon, TyphonActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.GravityController, GravityControllerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.GravityController, GravityControllerActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, ZeusSummon);
            AddExecutor(ExecutorType.Activate, CardId.Zeus, ZeusEffect);

            // 9. Sets (Tier 9)
            AddExecutor(ExecutorType.SpellSet, CardId.MysticalSpaceTyphoon, MstSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.Vision, RadiantSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.Chant, RadiantSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.Ascendance, RadiantSpellSet);
            AddExecutor(ExecutorType.SpellSet, CardId.Mandate, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, RunickSpellSet);

            // Always last
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Radiant Typhoon control/combo โ€” prefer going first to set up Link Varuroon + backrow
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _kroseaHandUsed = false;
            _kroseaSummonUsed = false;
            _vibrantVortexHandUsed = false;
            _vibrantVortexNegateUsed = false;
            _eldamSummonUsed = false;
            _swenSummonUsed = false;
            _meghalaSpUsed = false;
            _visionUsed = false;
            _chantUsed = false;
            _ascendanceUsed = false;
            _mandateUsed = false;
            _fountainUsed = false;
            _linkVaruroonSummonUsed = false;
            _linkVaruroonTrapPlaceUsed = false;
            _windLocked = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
                _breakBoardEvaluated = false;
                _breakBoardScore = -1;
            }
        }

        // โ”€โ”€ WIND Lock checks โ”€โ”€
        private void TriggerWindLock()
        {
            _windLocked = true;
        }

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && AceCardIds.Any(id => card.IsCode(id));
        }

        private bool IsHandTrap(ClientCard card)
        {
            return card != null && HandTrapIds.Any(id => card.IsCode(id));
        }

        private bool HasRemainingRadiantOrMst()
        {
            return Bot.GetRemainingCount(CardId.MysticalSpaceTyphoon, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Krosea, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Eldam, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Swen, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Meghala, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.VibrantVortex, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Fonix, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Vision, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Chant, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Ascendance, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.Mandate, 3) > 0;
        }

        private static readonly int[] TargetProtectionIds = {
            40908371,  // Azure-Eyes Silver Dragon
            21887179,  // The Arrival Cyberse @Ignister
            86157908,  // Marincess Bubble Reef
            68957034,  // (placeholder โ€” cards with "unaffected by card effects")
            10000030,  // Blue-Eyes Jet Dragon
            25290459,  // Raidraptor - Ultimate Falcon
            30674956,  // (cards that can't be targeted)
        };

        private bool EnemyHasTargetProtection()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (TargetProtectionIds.Any(id => c.IsCode(id)) || c.IsShouldNotBeTarget()));
        }

        // โ”€โ”€ Lethal Check (delegates to BoardScorer โ€” accurate combat simulation) โ”€โ”€
        protected override bool CanDealLethal()
        {
            return Scorer.HasLethal();
        }

        // โ”€โ”€ Attack/Defense Safety โ”€โ”€
        private bool IsSafeToAttack(ClientCard attacker)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.IsAttack() && enemy.Attack >= attacker.Attack) return false;
                if (enemy.IsDefense() && attacker.Attack <= enemy.Defense) return false;
            }
            return true;
        }

        // โ”€โ”€ Going-Second Break Board Evaluation โ”€โ”€
        private int _breakBoardScore = -1;
        private bool _breakBoardEvaluated = false;

        private bool EvaluateBreakBoard()
        {
            if (_breakBoardEvaluated) return _breakBoardScore > 0;
            _breakBoardEvaluated = true;

            if (Duel.Turn != 2 || Duel.Player != 0) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            int score = 0;
            int enemyDisruption = 0;

            // Count enemy disruptions (face-up monsters with negate/interrupt effects, and set backrow)
            foreach (var m in Enemy.GetMonsters())
            {
                if (m == null || !m.IsFaceup()) continue;
                if (m.Attack >= 2500) enemyDisruption++;
                if (m.HasType(CardType.Xyz) || m.HasType(CardType.Synchro) || m.HasType(CardType.Fusion) || m.HasType(CardType.Link))
                    enemyDisruption++;
            }
            enemyDisruption += Enemy.GetSpellCount(); // backrow as potential disruption

            // Count our break-board tools in hand
            int breakers = 0;
            if (Bot.HasInHand(CardId.Shiina)) breakers += 2;
            if (Bot.HasInHand(CardId.DrollAndLockBird)) breakers += 1;
            if (Bot.HasInHand(CardId.MysticalSpaceTyphoon)) breakers += 1;

            score = breakers - enemyDisruption;
            _breakBoardScore = score;
            return score > 0;
        }

        private bool IsSafeToDefend(ClientCard monster)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.Attack > monster.Defense) return false;
            }
            return true;
        }

        // โ”€โ”€ MonsterRepos โ”€โ”€
        private bool MonsterRepos()
        {
            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster == null) continue;

                // Protect Link monsters (they cannot be in defense position)
                if (monster.HasType(CardType.Link)) continue;

                bool enemyEmpty = Enemy.GetMonsterCount() == 0;

                if (monster.IsAttack())
                {
                    // If opponent controls a monster with higher ATK, switch to defense position to prevent battle damage
                    if (!enemyEmpty)
                    {
                        bool enemyHasStrongerMonster = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack > monster.Attack);
                        if (enemyHasStrongerMonster)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    // Switch to attack if enemy is empty, or if we can safely attack (no enemy monster has higher or equal ATK),
                    // or if our monster has 0 DEF and high ATK (like Vibrant Vortex with 2900 ATK / 0 DEF) and we want to attack or threaten.
                    if (enemyEmpty)
                    {
                        return true;
                    }
                    
                    bool canAttackSafely = !Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= monster.Attack);
                    if (canAttackSafely || (monster.Attack > monster.Defense && monster.Defense == 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // โ”€โ”€ Hand Traps & Negation โ”€โ”€
        private bool DrollAndLockBirdActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player != 1) return false;
            return true;
        }

        private bool MaxxCActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return DefaultMaxxC();
        }

        private bool CalledByTheGraveActivate()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return DefaultCalledByTheGrave();
        }

        // โ”€โ”€ Shiina, Twin Tempests Hand Trap (12197223) โ”€โ”€
        private bool ShiinaActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;

            // Requires control of a WIND monster
            bool controlsWind = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Wind));
            if (!controlsWind) return false;

            // Going-second Break Board: use Shiina aggressively to clear opponent's board
            bool goBreakBoard = EvaluateBreakBoard();

            // If opponent activated a monster effect, verify we aren't wiping our own valuable bosses
            if (LastChainCard.IsMonster())
            {
                if (goBreakBoard) return true;

                bool hasAce = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));
                if (hasAce) return false;

                bool hasXyz = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.TotemBird) || c.IsCode(CardId.CicadaKing)));
                if (hasXyz) return false;

                int ourMonsters = Bot.GetMonsterCount();
                int enemyMonsters = Enemy.GetMonsterCount();
                if (ourMonsters > enemyMonsters && ourMonsters >= 3) return false;
            }

            // If opponent activated a Spell/Trap, be careful not to bounce our own valuable Fountain/Mandate
            if (LastChainCard.IsSpell() || LastChainCard.IsTrap())
            {
                if (goBreakBoard) return true;

                bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain) || Bot.HasInSpellZone(CardId.Mandate);
                if (hasFountain)
                {
                    int ourSpells = Bot.GetSpellCount();
                    int enemySpells = Enemy.GetSpellCount();
                    if (enemySpells <= ourSpells)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // โ”€โ”€ Vibrant Vortex (53927851) โ”€โ”€
        private bool VibrantVortexActivate()
        {
            // In hand: Special Summon on Quick-Play activation
            if (Card.Location == CardLocation.Hand)
            {
                if (_vibrantVortexHandUsed) return false;
                _vibrantVortexHandUsed = true;
                return true;
            }

            // On field: Negate opponent monster effect if MST is in GY
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_vibrantVortexNegateUsed) return false;
                if (LastChainCard == null || LastChainCard.Controller == 0) return false;
                if (!LastChainCard.IsMonster()) return false;
                
                bool hasMst = Bot.HasInGraveyard(CardId.MysticalSpaceTyphoon);
                if (!hasMst) return false;

                _vibrantVortexNegateUsed = true;
                return true;
            }

            // In GY: Special Summon if MST is activated
            if (Card.Location == CardLocation.Grave)
            {
                // ๐ง  Only SS from GY if we need board presence or MST was just used
                if (IsBoardStrongEnough() && !IsInGrindGame())
                    return false;
                return true;
            }

            return false;
        }

        // โ”€โ”€ Fonix, the Great Flame (85315450) โ”€โ”€
        private bool FonixActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // ๐ง  Only SS from hand if we need board presence
                if (IsBoardStrongEnough() && !IsInGrindGame())
                    return false;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // ๐ง  Only SS from GY if we need a body and have summonable target
                if (Bot.GetMonsterCount() >= 3 || (!NeedsBoardPresence() && !IsInGrindGame()))
                    return false;
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard target1 = Scorer.GetHighestThreat();
                if (target1 != null)
                {
                    var targets = new List<ClientCard> { target1 };
                    ClientCard target2 = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !targets.Contains(c))
                                      ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && !targets.Contains(c));
                    if (target2 != null) targets.Add(target2);
                    
                    AI.SelectCard(targets);
                    return true;
                }
            }
            return false;
        }

        // โ”€โ”€ Runick Fountain (92107604) โ”€โ”€
        private bool RunickFountainActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // Do not activate if we already have one face-up
                if (Bot.HasInSpellZone(CardId.RunickFountain)) return false;
                return true;
            }

            // Opponent's turn: Fountain lets us activate Runick Quick-Play Spells from hand
            if (Card.Location == CardLocation.SpellZone && Duel.Player == 1)
            {
                if (_fountainUsed) return false;
                // Check if we have Runick Quick-Play spells in hand to activate
                bool hasRunickInHand = Bot.Hand.Any(c => c != null && c.IsSpell() && c.HasType(CardType.QuickPlay) && c.HasSetcode(0x1183));
                if (!hasRunickInHand) return false;

                _fountainUsed = true;
                return true;
            }

            // Field effect: Shuffle up to 3 Runick spells from GY to draw
            if (Card.Location == CardLocation.SpellZone && Duel.Player == 0)
            {
                if (_fountainUsed) return false;
                var spellsInGy = Bot.Graveyard.Where(c => c != null && c.IsSpell() && c.HasType(CardType.QuickPlay) && c.HasSetcode(0x1183)).ToList();
                if (spellsInGy.Count == 0) return false;

                _fountainUsed = true;
                // Select up to 3 cards
                AI.SelectCard(spellsInGy);
                return true;
            }

            return false;
        }

        // โ”€โ”€ Radiant Typhoon Mandate (53813120) โ”€โ”€
        private bool MandateActivate()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_mandateUsed) return false;

                // Continuous Trap activation: Target 3 Quick-Play Spells in GY (incl 1 Radiant) to recycle & draw
                var quickPlays = Bot.Graveyard.Where(c => c != null && c.IsSpell() && c.HasType(CardType.QuickPlay)).ToList();
                var radiantQuickPlays = quickPlays.Where(c => c.IsCode(CardId.MysticalSpaceTyphoon) || c.IsCode(CardId.Vision) || c.IsCode(CardId.Chant) || c.IsCode(CardId.Ascendance)).ToList();

                if (quickPlays.Count >= 3 && radiantQuickPlays.Count >= 1)
                {
                    _mandateUsed = true;
                    // AI selects 1 radiant and 2 other quickplays
                    var selection = new List<ClientCard>();
                    selection.Add(radiantQuickPlays[0]);
                    foreach (var card in quickPlays)
                    {
                        if (selection.Count >= 3) break;
                        if (!selection.Contains(card)) selection.Add(card);
                    }
                    AI.SelectCard(selection);
                    return true;
                }
            }

            // Trigger when MST is activated: Negate 1 face-up enemy card
            if (Card.Location == CardLocation.SpellZone)
            {
                if (LastChainCard != null && LastChainCard.IsCode(CardId.MysticalSpaceTyphoon))
                {
                    ClientCard target = Scorer.GetHighestThreat(onlyFaceup: true, canBeTarget: true);
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }

            // Trigger Set when popped by MST
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }

            return false;
        }

        // โ”€โ”€ MST (5318639) โ”€โ”€
        private bool MysticalSpaceTyphoonActivate()
        {
            // Do not chain to our own cards (unless we want to pop our set Radiant cards on our own turn)
            if (LastChainCard != null && LastChainCard.Controller == 0 && Duel.CurrentChain.Count > 0)
            {
                return false;
            }

            // If opponent is activating a Spell/Trap card, only chain MST if it is a Continuous/Field/Equip card where destruction disrupts resolution!
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                if (LastChainCard.IsSpell() || LastChainCard.IsTrap())
                {
                    bool isContinuousOrFieldOrEquip = LastChainCard.HasType(CardType.Continuous) ||
                                                      LastChainCard.HasType(CardType.Field) ||
                                                      LastChainCard.HasType(CardType.Equip);
                    if (!isContinuousOrFieldOrEquip && Duel.Phase != DuelPhase.End)
                    {
                        return false;
                    }
                }
            }

            // If we are chaining to an opponent's card, or on opponent's turn, prioritize opponent backrow
            if (Duel.CurrentChain.Count > 0 || Duel.Player == 1)
            {
                ClientCard enemyBackrow = Util.GetBestEnemySpell();
                if (enemyBackrow != null)
                {
                    // Avoid targeting a card that is already targeted in the current chain
                    if (Duel.CurrentChain.Any(c => c.IsCode(CardId.MysticalSpaceTyphoon) && Duel.ChainTargets.Contains(enemyBackrow)))
                    {
                        return false;
                    }

                    AI.SelectCard(enemyBackrow);
                    return true;
                }
            }
            else
            {
                // On our turn, if we are in Main Phase and want to perform our combo:
                if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                {
                    // Find our own face-down set Radiant Spells that have not been used yet
                    var ourSetRadiants = Bot.GetSpells().Where(c => c != null && c.IsFacedown() &&
                        ((c.IsCode(CardId.Vision) && !_visionUsed) ||
                         (c.IsCode(CardId.Chant) && !_chantUsed) ||
                         (c.IsCode(CardId.Ascendance) && !_ascendanceUsed))).ToList();

                    if (ourSetRadiants.Count > 0)
                    {
                        // Prioritize popping Vision first (draw 2), then Chant (search), then Ascendance (revive)
                        var target = ourSetRadiants.OrderBy(c => {
                            if (c.IsCode(CardId.Vision)) return 1;
                            if (c.IsCode(CardId.Chant)) return 2;
                            return 3;
                        }).First();

                        AI.SelectCard(target);
                        return true;
                    }

                    // Fallback to enemy backrow if no set Radiants
                    ClientCard enemyBackrow = Util.GetBestEnemySpell();
                    if (enemyBackrow != null)
                    {
                        AI.SelectCard(enemyBackrow);
                        return true;
                    }
                }
            }

            return false;
        }

        // โ”€โ”€ Radiant Spells Pop Triggers โ”€โ”€
        private bool VisionActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // When destroyed by MST: Draw 2, then discard 1 if holding Radiant/Quick-Play, else discard all.
                // Only activate if we have enough cards in hand to make this worthwhile.
                return Bot.Hand.Count >= 1;
            }
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFaceup()))
            {
                if (_visionUsed) return false;
                _visionUsed = true;
                return true;
            }
            return false;
        }

        private bool ChantActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFaceup()))
            {
                if (_chantUsed) return false;
                
                bool hasTarget = Bot.GetRemainingCount(CardId.Eldam, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Swen, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Meghala, 3) > 0;
                if (!hasTarget) return false;

                _chantUsed = true;
                return true;
            }
            return false;
        }

        private bool AscendanceActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                return true;
            }
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFaceup()))
            {
                if (_ascendanceUsed) return false;
                
                bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Level <= 6 && c.HasAttribute(CardAttribute.Wind) && c.IsCanRevive());
                if (!hasTarget) return false;

                _ascendanceUsed = true;
                return true;
            }
            return false;
        }

        // โ”€โ”€ Starters: Krosea, Eldam, Swen, Meghala โ”€โ”€
        private bool KroseaActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_kroseaHandUsed) return false;
                _kroseaHandUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                if (_kroseaSummonUsed) return false;

                // Verify search target exists in Deck
                if (!HasRemainingRadiantOrMst()) return false;

                _kroseaSummonUsed = true;
                TriggerWindLock();
                return true;
            }
            return false;
        }

        private bool KroseaSummon()
        {
            if (Bot.GetMonsters().Count(c => c.HasAttribute(CardAttribute.Wind)) >= 2)
                return false; // Preserve room
            return true;
        }

        private bool EldamActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_eldamSummonUsed) return false;

                // Verify search target exists in Deck (any Radiant monster except Eldam, or MST)
                bool hasTarget = Bot.GetRemainingCount(CardId.Krosea, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Swen, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Meghala, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.VibrantVortex, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Fonix, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.MysticalSpaceTyphoon, 3) > 0;
                if (!hasTarget) return false;

                _eldamSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool EldamSummon()
        {
            return true;
        }

        private bool SwenActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_swenSummonUsed) return false;

                // Verify search target exists in Deck (any Radiant S/T, or MST)
                bool hasTarget = Bot.GetRemainingCount(CardId.Vision, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Chant, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Ascendance, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Mandate, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.MysticalSpaceTyphoon, 3) > 0;
                if (!hasTarget) return false;

                _swenSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool SwenSummon()
        {
            return true;
        }

        private bool MeghalaActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_meghalaSpUsed) return false;
                
                // Can Special Summon if we control a WIND monster
                bool controlsWind = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Wind));
                if (controlsWind)
                {
                    _meghalaSpUsed = true;
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                // Verify we have a different named Radiant target in Deck
                var controlledIds = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).Select(c => c.Id).ToList();
                int[] allRadiantMonsterIds = {
                    CardId.Krosea,
                    CardId.Eldam,
                    CardId.Swen,
                    CardId.Meghala,
                    CardId.VibrantVortex,
                    CardId.Fonix
                };
                bool hasTarget = false;
                foreach (int id in allRadiantMonsterIds)
                {
                    if (!controlledIds.Contains(id) && Bot.GetRemainingCount(id, 3) > 0)
                    {
                        hasTarget = true;
                        break;
                    }
                }
                if (!hasTarget) return false;

                return true;
            }
            return false;
        }

        // โ”€โ”€ Majesty's Fiend Tribute Summon โ”€โ”€
        private bool MajestysFiendSummon()
        {
            int monsterCount = Bot.GetMonsterCount();
            if (monsterCount >= 1)
            {
                bool opponentHasStrongMonster = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2400 && c.IsAttack());
                if (opponentHasStrongMonster)
                {
                    // If opponent has a strong monster, only summon Majesty's Fiend if we have a way to protect it or destroy their monster,
                    // or if we have another strong monster of our own (like Vibrant Vortex or Sleipnir or Geri)
                    bool hasProtectionOrRemoval = Bot.HasInHand(CardId.RunickSlumber) || Bot.HasInSpellZone(CardId.RunickSlumber) ||
                                                 Bot.HasInHand(CardId.RunickFlashingFire) || Bot.HasInSpellZone(CardId.RunickFlashingFire) ||
                                                 Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500);
                    if (!hasProtectionOrRemoval)
                    {
                        return false;
                    }
                }

                // Choose expendable tribute (avoid tributing our valuable bosses)
                var tribute = Bot.GetMonsters().FirstOrDefault(c => c != null && !IsAceCard(c));
                if (tribute != null)
                {
                    AI.SelectCard(tribute);
                    return true;
                }
            }
            return false;
        }

        // โ”€โ”€ Extra Deck: Link Varuroon (39341885) โ”€โ”€
        private bool LinkVaruroonSpSummon()
        {
            if (CanDealLethal()) return false;
            if (_linkVaruroonSummonUsed) return false;
            // Requires 2 Radiant monsters
            var radiants = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() &&
                (c.IsCode(CardId.Krosea) || c.IsCode(CardId.Eldam) || c.IsCode(CardId.Swen) || c.IsCode(CardId.Meghala))).ToList();

            if (radiants.Count >= 2)
            {
                _linkVaruroonSummonUsed = true;
                return true;
            }
            return false;
        }

        private bool LinkVaruroonActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Link summon effect: Add MST
                if (ActivateDescription == Util.GetStringId(CardId.LinkVaruroon, 0))
                {
                    return true;
                }

                // Place 2 face-up monsters in S/T zones (Ignition)
                if (ActivateDescription == Util.GetStringId(CardId.LinkVaruroon, 1))
                {
                    if (EnemyHasTargetProtection()) return false;

                    var enemyTargets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()).ToList();
                    var ourMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
                    
                    // Do not use this effect if it would leave us with 0 monsters on board to defend ourselves
                    if (ourMonsters.Count == 1 && ourMonsters[0].IsCode(CardId.LinkVaruroon))
                    {
                        return false;
                    }

                    if (enemyTargets.Count > 0 && ourMonsters.Count >= 2)
                    {
                        return true;
                    }
                    return false;
                }

                // Quick Play Spell trigger effect (places Mandate trap)
                if (ActivateDescription == Util.GetStringId(CardId.LinkVaruroon, 2))
                {
                    if (_linkVaruroonTrapPlaceUsed) return false;
                    _linkVaruroonTrapPlaceUsed = true;
                    return true;
                }
            }
            return false;
        }

        // โ”€โ”€ Totem Bird (71068247) โ”€โ”€
        private bool TotemBirdSpSummon()
        {
            if (CanDealLethal()) return false;
            // ๐ง  Don't summon if board is already strong โ€” save materials
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;

            var lv3Winds = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 3 && c.HasAttribute(CardAttribute.Wind)).ToList();
            if (lv3Winds.Count >= 2)
            {
                return true;
            }
            return false;
        }

        private bool TotemBirdActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Negate S/T activation (requires 2 materials)
                if (LastChainCard != null && LastChainCard.Controller == 1 && (LastChainCard.IsSpell() || LastChainCard.IsTrap()))
                {
                    return true;
                }
            }
            return false;
        }

        // โ”€โ”€ Rank 9: Enterblathnir โ”€โ”€
        private bool EnterblathnirSpSummon()
        {
            if (CanDealLethal()) return false;
            var lv9s = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 9).ToList();
            if (lv9s.Count >= 2)
            {
                return true;
            }
            return false;
        }

        private bool EnterblathnirActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Choose first effect (banish opponent card)
                AI.SelectOption(0);
                ClientCard target = Scorer.GetHighestThreat(onlyFaceup: true, canBeTarget: true);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool ShouldSummonRunickFusion()
        {
            if (_windLocked) return false;

            // 1. Blocker Deficit: If under attack and we have no monsters, summon a blocker.
            if (Bot.UnderAttack && Bot.GetMonsterCount() == 0)
            {
                return true;
            }

            // 2. Tribute setup: If we have Majesty's Fiend in hand, but no monsters on field to tribute.
            if (Bot.HasInHand(CardId.MajestysFiend) && Bot.GetMonsterCount() == 0 && Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                return true;
            }

            // 3. Search setup: If we don't have Fountain in hand or field, we must summon Hugin to search it.
            // But only if we have at least 1 other card in hand to discard for Hugin's search effect cost.
            bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain) || Bot.HasInHand(CardId.RunickFountain);
            if (!hasFountain && Bot.Hand.Count > 0) return true;

            return false;
        }

        private bool ActivateRunickSpell(int fusionOption)
        {
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) && CanDealLethal())
            {
                return false;
            }

            // Don't activate Runick spells on our turn during Draw/Standby Phase to avoid wasting Fountain draw opportunities
            if (Duel.Player == 0 && Duel.Phase < DuelPhase.Main1)
            {
                return false;
            }

            if (ShouldSummonRunickFusion())
            {
                if (Duel.Player == 0 && Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)
                    return false;

                AI.SelectOption(1); // Special Summon Runick monster
                return true;
            }

            AI.SelectOption(0); // Activate target effect
            return false;
        }

        private bool RunickTipActivate()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player == 0 && Duel.Phase < DuelPhase.Main1) return false;
            
            bool summon = ActivateRunickSpell(0);
            if (summon) return true;

            // Target effect: Search 1 Runick card
            return true;
        }

        private bool RunickFreezingCursesActivate()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player == 0 && Duel.Phase < DuelPhase.Main1) return false;
            
            bool summon = ActivateRunickSpell(0);
            if (summon) return true;

            // Target effect: Negate monster effect
            ClientCard target = null;
            if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.IsMonster() && LastChainCard.Location == CardLocation.MonsterZone && !LastChainCard.IsDisabled() && !LastChainCard.IsShouldNotBeTarget())
            {
                target = LastChainCard;
            }
            else
            {
                target = Util.GetProblematicEnemyMonster(canBeTarget: true)
                      ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && !c.IsShouldNotBeTarget());
            }

            if (target != null)
            {
                if (Duel.CurrentChain.Any(c => c.IsCode(CardId.RunickFreezingCurses) && Duel.ChainTargets.Contains(target)))
                {
                    return false;
                }
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool RunickFlashingFireActivate()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player == 0 && Duel.Phase < DuelPhase.Main1) return false;
            
            bool summon = ActivateRunickSpell(0);
            if (summon) return true;

            // Target effect: Destroy Special Summoned monster
            ClientCard target = null;
            var candidates = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && c.IsSpecialSummoned && !c.IsShouldNotBeTarget()).ToList();
            if (candidates.Count > 0)
            {
                if (LastChainCard != null && LastChainCard.Controller == 1 && candidates.Contains(LastChainCard))
                {
                    target = LastChainCard;
                }
                else
                {
                    target = candidates.OrderByDescending(c => c.Attack).First();
                }
            }

            if (target != null)
            {
                if (Duel.CurrentChain.Any(c => c.IsCode(CardId.RunickFlashingFire) && Duel.ChainTargets.Contains(target)))
                {
                    return false;
                }
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool RunickSlumberActivate()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player == 0 && Duel.Phase < DuelPhase.Main1) return false;
            
            bool summon = ActivateRunickSpell(0);
            if (summon) return true;

            // Target effect: Protect 1 monster. Prioritize our ace monsters, then any face-up we control.
            ClientCard target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsAceCard(c))
                             ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup())
                             ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool RunickDestructionActivate()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player == 0 && Duel.Phase < DuelPhase.Main1) return false;
            
            bool summon = ActivateRunickSpell(0);
            if (summon) return true;

            // If opponent is activating a normal Spell/Trap, don't chain target effect (unless it is Continuous/Field/Equip)
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                if (LastChainCard.IsSpell() || LastChainCard.IsTrap())
                {
                    bool isContinuousOrFieldOrEquip = LastChainCard.HasType(CardType.Continuous) ||
                                                      LastChainCard.HasType(CardType.Field) ||
                                                      LastChainCard.HasType(CardType.Equip);
                    if (!isContinuousOrFieldOrEquip && Duel.Phase != DuelPhase.End)
                    {
                        return false;
                    }
                }
            }

            // Target effect: Destroy backrow
            ClientCard target = null;
            if (LastChainCard != null && LastChainCard.Controller == 1 && (LastChainCard.IsSpell() || LastChainCard.IsTrap()) &&
                (LastChainCard.HasType(CardType.Continuous) || LastChainCard.HasType(CardType.Field) || LastChainCard.HasType(CardType.Equip)) &&
                !LastChainCard.IsShouldNotBeTarget())
            {
                target = LastChainCard;
            }
            else
            {
                target = Util.GetBestEnemySpell();
            }

            if (target != null)
            {
                if (Duel.CurrentChain.Any(c => c.IsCode(CardId.RunickDestruction) && Duel.ChainTargets.Contains(target)))
                {
                    return false;
                }
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool RunickDispellingActivate()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player == 0 && Duel.Phase < DuelPhase.Main1) return false;
            
            bool summon = ActivateRunickSpell(0);
            if (summon) return true;

            // ๐ง  Target effect (banish up to 3 cards from opponent GY): only activate if opponent has meaningful GY targets
            bool hasMeaningfulGyTarget = Enemy.Graveyard.Any(c => c != null && 
                ((c.IsMonster() && c.IsCanRevive()) ||
                (!c.IsMonster() && !c.IsDisabled())));

            if (!hasMeaningfulGyTarget && Enemy.Graveyard.Count < 2)
                return false;

            return true;
        }

        private bool RunickSmitingStormActivate()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player == 0 && Duel.Phase < DuelPhase.Main1) return false;
            
            bool summon = ActivateRunickSpell(0);
            if (summon) return true;

            // Target effect: Banish cards up to opponent's field count
            if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() > 0)
            {
                return true;
            }
            return false;
        }

        // โ”€โ”€ SpellSets โ”€โ”€
        private bool RadiantSpellSet()
        {
            if (Card == null) return false;
            
            // We need MST to pop it. Check if we have MST in hand or on field
            bool hasMst = Bot.HasInHand(CardId.MysticalSpaceTyphoon) || Bot.HasInSpellZone(CardId.MysticalSpaceTyphoon);
            if (!hasMst) return false;

            if (Card.IsCode(CardId.Vision))
            {
                return !_visionUsed;
            }
            if (Card.IsCode(CardId.Chant))
            {
                bool hasTarget = Bot.GetRemainingCount(CardId.Eldam, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Swen, 3) > 0 ||
                                 Bot.GetRemainingCount(CardId.Meghala, 3) > 0;
                return !_chantUsed && hasTarget;
            }
            if (Card.IsCode(CardId.Ascendance))
            {
                bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Level <= 6 && c.HasAttribute(CardAttribute.Wind) && c.IsCanRevive());
                return !_ascendanceUsed && hasTarget;
            }
            
            return false;
        }

        private bool RunickSpellSet()
        {
            if (Card == null) return false;
            if (!SetTrapCondition()) return false;
            bool isRunickSpell = RunickSpellIds.Contains(Card.Id);
            bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain) || Bot.HasInHand(CardId.RunickFountain);
            return isRunickSpell && !hasFountain && Card.Location == CardLocation.Hand;
        }

        // โ”€โ”€ Extra Deck and SpellSet Helper Conditions โ”€โ”€
        private bool CicadaKingActivate()
        {
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;
            if (!LastChainCard.IsMonster() || LastChainCard.Location != CardLocation.MonsterZone) return false;
            
            // Cicada King targets 1 face-up monster your opponent controls.
            // Let's choose the activating monster if it's face-up, otherwise the best enemy monster.
            ClientCard target = null;
            if (LastChainCard.IsFaceup() && LastChainCard.Controller == 1)
            {
                target = LastChainCard;
            }
            else
            {
                target = Util.GetBestEnemyMonster(onlyFaceup: true, canBeTarget: true);
            }
            
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SPLittleKnightActivate()
        {
            // First effect (on-summon banish 1 card on the field or in GY)
            if (ActivateDescription == Util.GetStringId(CardId.SPLittleKnight, 0) || LastChainCard == null || LastChainCard.Controller == 0)
            {
                ClientCard target = Scorer.GetHighestThreat(canBeTarget: true)
                                 ?? Enemy.Graveyard.FirstOrDefault(c => c != null);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }

            // Second effect (Quick Effect: banish 2 face-up monsters on the field, including ours)
            if (ActivateDescription == Util.GetStringId(CardId.SPLittleKnight, 1) || (LastChainCard != null && LastChainCard.Controller == 1))
            {
                var ourTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c))
                             ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());

                var enemyTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsSpecialSummoned && !c.IsShouldNotBeTarget())
                               ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());

                if (ourTarget != null && enemyTarget != null)
                {
                    AI.SelectCard(new List<ClientCard> { ourTarget, enemyTarget });
                    return true;
                }
            }
            return false;
        }

        private bool DoomEagleActivate()
        {
            if (CanDealLethal()) return false;

            var targets = Enemy.Graveyard.Where(c => c != null).ToList();
            if (targets.Count > 0)
            {
                var preferred = targets.OrderByDescending(c => {
                    if (c.IsMonster() && c.IsSpecialSummoned) return 3;
                    if (c.IsSpell() || c.IsTrap()) return 2;
                    return 1;
                }).Take(3).ToList();
                AI.SelectCard(preferred);
                return true;
            }
            return false;
        }

        private bool WynnTheWindCharmerVerdantActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.HasAttribute(CardAttribute.Wind) && c.IsCanRevive());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }
            
            if (Card.Location == CardLocation.Grave)
            {
                return Bot.GetRemainingCount(CardId.Krosea, 3) > 0 ||
                       Bot.GetRemainingCount(CardId.Eldam, 3) > 0 ||
                       Bot.GetRemainingCount(CardId.Swen, 3) > 0 ||
                       Bot.GetRemainingCount(CardId.Meghala, 3) > 0;
            }
            return false;
        }

        private bool TyphonActivate()
        {
            ClientCard target = Util.GetBestEnemyMonster(canBeTarget: true);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // โ”€โ”€ Gravity Controller (23656668) โ”€โ”€
        private bool GravityControllerActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!Enemy.GetMonstersInExtraZone().Any(c => c != null && c.IsFaceup())) return false;
                return true;
            }
            return false;
        }

        private bool ZeusSummon()
        {
            if (Duel.Phase != DuelPhase.Main2) return false;
            return Bot.GetMonsters().Any(c => c != null && c.HasType(CardType.Xyz) && c.Attacked);
        }

        private bool ZeusEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            
            int enemyCount = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            int ourCount = Bot.GetMonsterCount() + Bot.GetSpellCount();
            return enemyCount >= 2 && enemyCount >= ourCount - 1;
        }

        private bool MstSpellSet()
        {
            return Util.IsTurn1OrMain2();
        }

        private bool HasExpendableMaterials(int count, bool windOnly = false)
        {
            var monsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            if (windOnly)
            {
                monsters = monsters.Where(c => c.HasAttribute(CardAttribute.Wind)).ToList();
            }
            
            var expendable = monsters.Where(c => !IsAceCard(c)).ToList();
            return expendable.Count >= count;
        }

        private bool CicadaKingSpSummon()
        {
            if (CanDealLethal()) return false;
            // ๐ง  Only summon if opponent has a monster to negate
            if (!Enemy.GetMonsters().Any(c => c != null && c.IsFaceup()))
                return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;

            var lv3s = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 3 && !IsAceCard(c)).ToList();
            return lv3s.Count >= 2;
        }

        private bool WynnTheWindCharmerVerdantSpSummon()
        {
            if (CanDealLethal()) return false;
            bool opponentHasWind = Enemy.Graveyard.Any(c => c != null && c.IsMonster() && c.HasAttribute(CardAttribute.Wind) && c.IsCanRevive());
            if (!opponentHasWind) return false;

            return HasExpendableMaterials(2, windOnly: true);
        }

        private bool DoomEagleSpSummon()
        {
            if (CanDealLethal()) return false;
            // ๐ง  Only summon if opponent GY has meaningful targets or we need GY hate
            bool hasGyTarget = Enemy.Graveyard.Any(c => c != null && c.IsMonster() && c.IsCanRevive());
            if (!hasGyTarget && Enemy.Graveyard.Count < 3) return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;

            return HasExpendableMaterials(2, windOnly: true);
        }

        private bool SPLittleKnightSpSummon()
        {
            if (CanDealLethal()) return false;
            // ๐ง  Only summon if opponent has meaningful targets
            if (Enemy.GetMonsters().Count(c => c != null && c.IsFaceup()) + Enemy.GetSpells().Count(c => c != null && c.IsFaceup()) < 2)
                return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;

            var effectMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
            return effectMonsters.Count >= 2;
        }

        private bool TyphonSpSummon()
        {
            if (CanDealLethal()) return false;
            
            // ๐ง  Only summon if opponent has a meaningful threat
            bool hasThreat = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Attack >= 2500 || c.IsFloodgate()));
            if (!hasThreat) return false;

            // ๐ง  Don't waste Extra Deck slot if board is already strong
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;

            if (Duel.Phase == DuelPhase.Main2) return true;
            if (Bot.Hand.Count == 0 && Bot.GetMonsterCount() > 0) return true;
            return false;
        }

        private bool GravityControllerSpSummon()
        {
            if (CanDealLethal()) return false;
            if (!Enemy.GetMonstersInExtraZone().Any(c => c != null && c.IsFaceup())) return false;
            return HasExpendableMaterials(1);
        }

        private bool SetTrapCondition()
        {
            return Util.IsTurn1OrMain2();
        }

        // โ”€โ”€ OnSelectOption Override (Intelligent choice selection) โ”€โ”€
        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;
            if (Card == null) return base.OnSelectOption(options);

            if (Card.IsCode(CardId.Vision) || Card.IsCode(CardId.Chant) || Card.IsCode(CardId.Ascendance))
            {
                bool hasMst = Bot.HasInHand(CardId.MysticalSpaceTyphoon) || Bot.HasInSpellZone(CardId.MysticalSpaceTyphoon);
                if (!hasMst && options.Count > 1)
                {
                    return 1; // Index 1 is Option 2: Add MST from Deck/GY to hand
                }
                return 0; // Index 0 is Option 1: Archetype effect (draw/search/revive)
            }

            if (Card.IsCode(CardId.Enterblathnir))
            {
                return 0; // Banish opponent's card
            }

            return base.OnSelectOption(options);
        }

        // โ”€โ”€ Safe SelectPreferredCards Helper โ”€โ”€
        private IList<ClientCard> SafeSelectPreferredCards(IList<ClientCard> preferred, IList<ClientCard> cards, int min, int max)
        {
            IList<ClientCard> selected = new List<ClientCard>();
            IList<ClientCard> avail = cards.ToList();
            
            foreach (ClientCard card in preferred)
            {
                if (selected.Count >= max) break;
                if (avail.Contains(card))
                {
                    selected.Add(card);
                    avail.Remove(card);
                }
            }

            while (selected.Count < min && avail.Count > 0)
            {
                ClientCard card = avail[0];
                selected.Add(card);
                avail.Remove(card);
            }

            return selected;
        }

        // โ”€โ”€ OnSelectCard Override (Location filtering & material protection) โ”€โ”€
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Handle target selection (hint 503) for Link Varuroon
            if (hint == 503 && Card != null && Card.IsCode(CardId.LinkVaruroon))
            {
                var opponentMonsters = cards.Where(c => c.Controller == 1 && c.IsFaceup()).ToList();
                var ourMonsters = cards.Where(c => c.Controller == 0 && c.IsFaceup() && !IsAceCard(c)).ToList();
                
                if (ourMonsters.Count == 0)
                {
                    ourMonsters = cards.Where(c => c.Controller == 0 && c.IsFaceup()).ToList();
                }

                if (opponentMonsters.Count > 0 && ourMonsters.Count > 0)
                {
                    var selection = new List<ClientCard> { opponentMonsters[0], ourMonsters[0] };
                    return SafeSelectPreferredCards(selection, cards, min, max);
                }

                // Fallback: If we must target our own, sort by priority (lowest first)
                var sortedOurMonsters = cards.Where(c => c.Controller == 0).OrderBy(c => GetMaterialPriority(c)).ToList();
                return SafeSelectPreferredCards(sortedOurMonsters, cards, min, max);
            }

            // Handle Link/Xyz Material (hint 533 / 513) - Protect Aces & Hand Traps
            if (hint == 533 || hint == 513)
            {
                var candidates = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return SafeSelectPreferredCards(candidates, cards, min, max);
            }

            // Handle Special Summon (hint 509) - Location filtering to prevent crash, prioritize best WIND revivals
            if (hint == 509)
            {
                bool hasDeck = cards.Any(c => c.Location == CardLocation.Deck);
                if (hasDeck && cards.Any(c => c.Location == CardLocation.Hand))
                {
                    var deckCards = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    return SafeSelectPreferredCards(deckCards, cards, min, max);
                }

                // Summoning from Extra Deck (Runick fusions)
                var extraTargets = cards.Where(c => c.Location == CardLocation.Extra).ToList();
                if (extraTargets.Count > 0)
                {
                    var preferred = extraTargets.OrderByDescending(c => {
                        // Prioritize Hugin if we don't have Fountain
                        if (c.IsCode(CardId.HuginTheRunickWings))
                        {
                            bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain) || Bot.HasInHand(CardId.RunickFountain);
                            return hasFountain ? 10 : 30;
                        }
                        // Prioritize Sleipnir as a stronger blocker / interruption
                        if (c.IsCode(CardId.SleipnirTheRunickMane)) return 20;
                        // Geri is lowest default unless we need to recover a card
                        if (c.IsCode(CardId.GeriTheRunickFangs)) return 15;
                        return 0;
                    }).ToList();
                    return SafeSelectPreferredCards(preferred, cards, min, max);
                }

                // Reviving from GY: prioritize best WIND monsters that IsCanRevive()
                var gyTargets = cards.Where(c => c.Location == CardLocation.Grave && c.IsCanRevive()).ToList();
                if (gyTargets.Count > 0)
                {
                    var preferred = gyTargets.OrderByDescending(c => {
                        if (c.IsCode(CardId.Krosea)) return 4;
                        if (c.IsCode(CardId.Eldam)) return 3;
                        if (c.IsCode(CardId.Swen)) return 2;
                        if (c.IsCode(CardId.Meghala)) return 1;
                        return 0;
                    }).ToList();
                    return SafeSelectPreferredCards(preferred, cards, min, max);
                }
            }

            // Handle Search / Add to hand (hint 506) - Prioritize MST and key cards
            if (hint == 506)
            {
                bool hasMst = Bot.HasInHand(CardId.MysticalSpaceTyphoon) || Bot.HasInSpellZone(CardId.MysticalSpaceTyphoon);
                bool hasRadiantSpell = Bot.HasInHand(CardId.Vision) || Bot.HasInHand(CardId.Chant) || Bot.HasInHand(CardId.Ascendance) ||
                                      Bot.HasInSpellZone(CardId.Vision) || Bot.HasInSpellZone(CardId.Chant) || Bot.HasInSpellZone(CardId.Ascendance);
                
                var preferred = cards.OrderBy(c => {
                    if (c.IsCode(CardId.MysticalSpaceTyphoon))
                    {
                        return (hasRadiantSpell && !hasMst) ? 1 : 6;
                    }
                    if (c.IsCode(CardId.Krosea)) return 2;
                    if (c.IsCode(CardId.Eldam)) return 3;
                    if (c.IsCode(CardId.Vision)) return 4;
                    if (c.IsCode(CardId.Swen)) return 5;
                    if (c.IsCode(CardId.Meghala)) return 7;
                    return 100;
                }).ToList();
                return SafeSelectPreferredCards(preferred, cards, min, max);
            }

            // Handle Destroy (hint 502) - Target opponent's cards first, only target our own if forced
            if (hint == 502)
            {
                var enemyCards = cards.Where(c => c.Controller == 1).OrderByDescending(c => c.IsMonster() ? c.Attack : 0).ToList();
                if (enemyCards.Count > 0)
                {
                    return SafeSelectPreferredCards(enemyCards, cards, min, max);
                }

                // If forced to target our own, prioritize set radiants that have GY triggers
                var ourSetRadiants = cards.Where(c => c.Controller == 0 && c.IsFacedown() &&
                    (c.IsCode(CardId.Vision) || c.IsCode(CardId.Chant) || c.IsCode(CardId.Ascendance) || c.IsCode(CardId.Mandate))).ToList();

                if (ourSetRadiants.Count > 0)
                {
                    var preferred = ourSetRadiants.OrderBy(c => {
                        if (c.IsCode(CardId.Vision)) return 1;
                        if (c.IsCode(CardId.Chant)) return 2;
                        if (c.IsCode(CardId.Ascendance)) return 3;
                        return 4;
                    }).ToList();
                    return SafeSelectPreferredCards(preferred, cards, min, max);
                }

                // Generic fallback for our own cards (lowest priority first)
                var ourGeneric = cards.Where(c => c.Controller == 0).OrderBy(c => GetMaterialPriority(c)).ToList();
                return SafeSelectPreferredCards(ourGeneric, cards, min, max);
            }

            if (Card == null)
            {
                return base.OnSelectCard(cards, min, max, hint, cancelable);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (IsHandTrap(c)) return 800;

            // Prioritize fusions or tokens
            if (c.IsExtraCard()) return 100;
            if (c.Level == 3) return 200;
            return 300;
        }
    }

    [Deck("Expert_2026_Radiant", "2026_Radiant")]
    public class ExpertRadiantExecutor : _2026_RadiantExecutor
    {
        private string _duelId;
        public ExpertRadiantExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _duelId = $"duel_{Guid.NewGuid():N}";
            // [REMOVED-AI-TRAINING] ExpertDataLogger.EnsureInitialized(ExpertDataLogger.FindProjectRoot());
        }
        public override MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            var action = base.OnSelectIdleCmd(main);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogMainPhaseDecision(main, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            var action = base.OnBattle(attackers, defenders);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogBattleDecision(attackers, defenders, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
    }
}
