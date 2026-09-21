// ============================================================================
// CARD AUDIT — Anime_JoeyWheeler (Joey Wheeler's ADO Deck: Flame Swordsman & Dark Time Wizard Engine)
// ============================================================================
// | Card Name                              | Type          | OPT? | HOPT? | Cost/Req  | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |----------------------------------------|---------------|------|-------|-----------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Swift Panther Warrior                  | Monster L4    | Yes  | Yes   | Tribute   | Quick tribute 1 mon: SS DarkTime mon or dump  | Need Alligator/DarkTime setup, have fodder   | No fodder / only Ace monsters on field      |
// | Alligator's Sword Dragon Knight        | Monster L5    | Yes  | Yes   | Reveal/Dis| Reveal DarkTime to SS; On SS search 2 S/T     | Have DarkTime in hand / On summon trigger     | Already searched / no discard fodder        |
// | Fisherman, Legend of the Sea           | Monster L4    | Yes  | Yes   | Target    | Quick SS from hand on opp effect/atk; pop mon | Opponent activates mon / declares attack     | Opponent has no target / target immune      |
// | Gearfried the Steel Knight             | Monster L4    | Yes  | Yes   | Equip/Des | Quick SS if mon destroyed; equip GY mon; pop  | Monster destroyed this turn / have equip mon  | No equips / already used this turn          |
// | Gilford the Lightning, Warrior Thunder | Monster L8    | Yes  | Yes   | Tribute 3 | SS by tributing 3; opp sends all mons to GY   | Opponent has 2+ monsters / threat board      | Opponent board empty / need normal beatdown |
// | Fighting Flame Swordsman               | Monster L4    | Yes  | Yes   | Search/GY | Search Flame Swords S/T; GY dump Flame mon    | Normal/Special Summon; sent to GY            | Already searched / no targets in deck       |
// | Salamandra, Flying Flame Dragon        | Monster L2    | Yes  | Yes   | Equip/GY  | Equip to Warrior (+700 ATK); GY search Salam  | Have Warrior on field; sent to GY            | No Warrior target / already equipped        |
// | Dark Time Wizard                       | Quick Spell   | Yes  | Yes   | Choice    | Opt 1: Search DarkTime S/T + EP recycle; Opt2 | Need search (Opt 1) / Board wipe (Opt 2)      | Already used / no targets in deck           |
// | Fighting Flame Sword                   | Quick Spell   | Yes  | Yes   | Choice    | Opt 1: Search Flame card; Opt2: Negate target | Need Flame search / protect Flame Swordsman  | Target already protected                    |
// | Fire Formation - Tenki                 | Cont Spell    | Yes  | Yes   | None      | Search Beast-Warrior (Swift Panther Warrior)  | Main Phase starter                            | Already have Panther in hand                |
// | Flame Swordsrealm                      | Cont Spell    | Yes  | No    | Send 1    | Send mon -> SS Flame Swordsman from Extra     | Have fodder / need Fusion presence           | No Extra Deck Flame Swordsman targets       |
// | Graceful Skull Dice                    | Quick Spell   | Yes  | Yes   | Roll/Ban  | Field: stat swing; GY: banish to pop summon   | Battle phase or opponent summons monster     | Opponent monster already destroyed          |
// | Salamandra Fusion                      | Equip Spell   | Yes  | Yes   | Equip/Rel | Equip +700 ATK; Send to SS Ultimate Swordsman | Have Flame Swordsman / need Ultimate boss     | No Fusion target on field                   |
// | Sleeping Scapegoats                    | Quick Spell   | No   | No    | None      | SS up to 4 Tokens; SS Panther if opp has mon  | Opponent turn End Phase / need defense/fodder| Board full / would block important zones    |
// | Super Critical                         | Cont Spell    | Yes  | Yes   | None      | Search die-roll card; trigger on 1 rolled     | Main Phase 1 setup                           | Already active on field                     |
// | Foolish Graverobber                    | Normal Trap   | Yes  | Yes   | Dump/GY   | Dump DarkTime S/T -> SS mon from either GY    | Need disruption / revive key boss            | Both GYs empty of valid monsters            |
// | Reversal Box                           | Cont Trap     | Yes  | No    | Counter   | Standby counters; coin flip to negate & ATK=0 | Opponent activates mon / declares attack     | No counters left                            |
// | Salamandra with Chain                  | Normal Trap   | Yes  | Yes   | Equip/GY  | Equip +700 & flip face-down; GY Fusion summon | Opponent summons threat / Need GY Fusion     | Target already face-down                    |
// | Fighting Flame Dragon                  | Fusion L5     | Yes  | Yes   | GY Equip  | GY equip to Warrior Fusion (+700, 2nd attack) | Have Warrior Fusion on field / in battle     | Already equipped                            |
// | Flame Swordsman                        | Fusion L5     | No   | No    | None      | Bridge monster summoned via Swordsrealm       | Material for Ultimate Flame Swordsman        | None                                        |
// | Red-Eyes Black Dragon Exceed           | Fusion L9     | Yes  | Yes   | Tribute 1 | SS by tributing mon after DarkTime wipe; un-af| DarkTime destroyed mon / need unaffected Ace | Conditions not met                          |
// | Ultimate Flame Swordsman               | Fusion L8     | Yes  | Yes   | Quick Pop | Quick destroy 1 opp mon + 500 burn; double ATK| Opponent has threat mon; Damage step lethal  | Target immune / already destroyed           |
// | Ferocious Flame Swordsman              | Link-2        | Yes  | Yes   | None      | Boost Warriors +500 ATK; float revive on pop  | Have 2 monsters / need ATK boost push        | Need monsters for other plays               |
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
    [Deck("Anime_JoeyWheeler", "Anime_JoeyWheeler")]
    public class Anime_JoeyWheelerExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int SwiftPantherWarrior = 101402001;
            public const int AlligatorsSwordDragonKnight = 101402002;
            public const int FishermanLegendOfTheSea = 101402004;
            public const int GearfriedTheSteelKnight = 100459023;
            public const int GilfordTheLightningWarriorOfThunderbolts = 100459008;
            public const int FightingFlameSwordsman = 1047075;
            public const int SalamandraTheFlyingFlameDragon = 37531679;

            // Spells
            public const int DarkTimeWizard = 101402052;
            public const int FightingFlameSword = 35697544;
            public const int FireFormationTenki = 57103969;
            public const int FlameSwordsrealm = 73714736;
            public const int GracefulSkullDice = 101402053;
            public const int SalamandraFusion = 9102835;
            public const int SleepingScapegoats = 101402054;
            public const int SuperCritical = 100459015;

            // Traps
            public const int FoolishGraverobber = 101402070;
            public const int ReversalBox = 101402071;
            public const int SalamandraWithChain = 62091148;

            // Extra Deck
            public const int FightingFlameDragon = 36319131;
            public const int FlameSwordsman = 45231177;
            public const int RedEyesBlackDragonExceed = 101402036;
            public const int UltimateFlameSwordsman = 324483;
            public const int FerociousFlameSwordsman = 98642179;

            // Tokens
            public const int ScapegoatToken = 101402154;
        }

        private static readonly int[] DarkTimeCards = {
            CardId.SwiftPantherWarrior,
            CardId.AlligatorsSwordDragonKnight,
            CardId.FishermanLegendOfTheSea,
            CardId.DarkTimeWizard,
            CardId.GracefulSkullDice,
            CardId.SleepingScapegoats,
            CardId.FoolishGraverobber,
            CardId.ReversalBox
        };

        private static readonly int[] FlameSwordsmanCards = {
            CardId.FightingFlameSwordsman,
            CardId.SalamandraTheFlyingFlameDragon,
            CardId.FightingFlameSword,
            CardId.FlameSwordsrealm,
            CardId.SalamandraFusion,
            CardId.SalamandraWithChain,
            CardId.FightingFlameDragon,
            CardId.FlameSwordsman,
            CardId.UltimateFlameSwordsman,
            CardId.FerociousFlameSwordsman
        };

        private static readonly int[] AceCardIds = {
            CardId.RedEyesBlackDragonExceed,
            CardId.UltimateFlameSwordsman,
            CardId.GilfordTheLightningWarriorOfThunderbolts,
            CardId.FerociousFlameSwordsman,
            CardId.FlameSwordsman
        };

        private bool _darkTimeDestroyedMonThisTurn = false;
        private bool _sleepingScapegoatsUsedThisTurn = false;

        public Anime_JoeyWheelerExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            RegisterExecutors();
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High Priority Negation & Targeting Protection (Quick)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.FightingFlameSword, ShouldFightingFlameSwordNegate);

            // -------------------------------------------------------------
            // 2. Quick Disruptions & Removal (Opponent Turn / Free Chain)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.UltimateFlameSwordsman, ShouldUltimateFlameSwordsmanDestroy);
            AddExecutor(ExecutorType.Activate, CardId.FishermanLegendOfTheSea, ShouldFishermanSpecialSummon);
            AddExecutor(ExecutorType.Activate, CardId.ReversalBox, ShouldReversalBoxActivate);
            AddExecutor(ExecutorType.Activate, CardId.SalamandraWithChain, ShouldSalamandraWithChainBookOfMoon);
            AddExecutor(ExecutorType.Activate, CardId.GracefulSkullDice, ShouldGracefulSkullDiceActivate);

            // -------------------------------------------------------------
            // 3. GY Triggers & Recursion (Delays / Chains)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.FightingFlameDragon, ShouldFightingFlameDragonGYEquip);
            AddExecutor(ExecutorType.Activate, CardId.SalamandraTheFlyingFlameDragon, ShouldSalamandraGYSearch);
            AddExecutor(ExecutorType.Activate, CardId.SalamandraWithChain, ShouldSalamandraWithChainGYFusion);
            AddExecutor(ExecutorType.Activate, CardId.FoolishGraverobber, ShouldFoolishGraverobberActivate);

            // -------------------------------------------------------------
            // 4. Board Breakers & Contact / Special Invocations
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpSummon, CardId.GilfordTheLightningWarriorOfThunderbolts, ShouldGilfordTributeSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RedEyesBlackDragonExceed, ShouldRedEyesExceedSummon);
            AddExecutor(ExecutorType.Activate, CardId.RedEyesBlackDragonExceed, ShouldRedEyesExceedRevive);
            AddExecutor(ExecutorType.Activate, CardId.GilfordTheLightningWarriorOfThunderbolts, ShouldGilfordWipeTrigger);

            // -------------------------------------------------------------
            // 5. Starters & Engine Searchers (Main Phase 1)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.FireFormationTenki, ShouldTenkiActivate);
            AddExecutor(ExecutorType.Activate, CardId.SuperCritical, ShouldSuperCriticalActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarkTimeWizard, ShouldDarkTimeWizardActivate);
            AddExecutor(ExecutorType.Activate, CardId.FightingFlameSword, ShouldFightingFlameSwordSearch);

            // -------------------------------------------------------------
            // 6. Normal Summons
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Summon, CardId.FightingFlameSwordsman, ShouldFightingFlameSwordsmanSummon);
            AddExecutor(ExecutorType.Summon, CardId.SwiftPantherWarrior, ShouldSwiftPantherWarriorSummon);
            AddExecutor(ExecutorType.Summon, CardId.GearfriedTheSteelKnight, ShouldGearfriedSummon);
            AddExecutor(ExecutorType.Summon, CardId.FishermanLegendOfTheSea, ShouldFishermanNormalSummon);

            // -------------------------------------------------------------
            // 7. Hand Extenders & Swarm
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.AlligatorsSwordDragonKnight, ShouldAlligatorActivate);
            AddExecutor(ExecutorType.Activate, CardId.SwiftPantherWarrior, ShouldSwiftPantherWarriorActivate);
            AddExecutor(ExecutorType.Activate, CardId.GearfriedTheSteelKnight, ShouldGearfriedActivate);
            AddExecutor(ExecutorType.Activate, CardId.FightingFlameSwordsman, ShouldFightingFlameSwordsmanTrigger);
            AddExecutor(ExecutorType.Activate, CardId.SleepingScapegoats, ShouldSleepingScapegoatsActivate);

            // -------------------------------------------------------------
            // 8. Fusion & Equip Plays
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.FlameSwordsrealm, ShouldFlameSwordsrealmActivate);
            AddExecutor(ExecutorType.Activate, CardId.SalamandraFusion, ShouldSalamandraFusionActivate);
            AddExecutor(ExecutorType.Activate, CardId.SalamandraTheFlyingFlameDragon, ShouldSalamandraEquipField);

            // -------------------------------------------------------------
            // 9. Extra Deck Summons (Link & Combat Boosts)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpSummon, CardId.FerociousFlameSwordsman, ShouldFerociousFlameSwordsmanSummon);
            AddExecutor(ExecutorType.Activate, CardId.FerociousFlameSwordsman, ShouldFerociousFlameSwordsmanTrigger);
            AddExecutor(ExecutorType.Activate, CardId.UltimateFlameSwordsman, ShouldUltimateFlameSwordsmanDoubleAtk);

            // -------------------------------------------------------------
            // 10. Traps & Spells Set (End of Turn Planning)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpellSet, CardId.ReversalBox, ShouldSetTrap);
            AddExecutor(ExecutorType.SpellSet, CardId.FoolishGraverobber, ShouldSetTrap);
            AddExecutor(ExecutorType.SpellSet, CardId.SalamandraWithChain, ShouldSetTrap);
            AddExecutor(ExecutorType.SpellSet, CardId.SleepingScapegoats, ShouldSetQuickSpell);
            AddExecutor(ExecutorType.SpellSet, CardId.DarkTimeWizard, ShouldSetQuickSpell);
            AddExecutor(ExecutorType.SpellSet, CardId.GracefulSkullDice, ShouldSetQuickSpell);
            AddExecutor(ExecutorType.SpellSet, CardId.FightingFlameSword, ShouldSetQuickSpell);
        }

        // =========================================================================
        // IMPLEMENTATION METHODS
        // =========================================================================

        #region 1. Quick Negation & Protection
        private bool ShouldFightingFlameSwordNegate()
        {
            if (Duel.CurrentChain.Count == 0) return false;
            // Check if opponent targeted a Flame Swordsman or monster mentioning it
            ClientCard target = LastChainCard;
            if (target != null && target.Controller == 1)
            {
                foreach (ClientCard card in Bot.GetMonsters())
                {
                    if (IsFlameSwordsmanRelated(card) && Util.IsChainTarget(card))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 2. Quick Disruptions
        private bool ShouldUltimateFlameSwordsmanDestroy()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Only destroy when opponent has face-up monsters
                ClientCard target = Util.GetProblematicEnemyMonster(0, true);
                if (target != null && !IsTargetImmune(target))
                {
                    AI.SelectCard(target);
                    return true;
                }
                var oppMons = Enemy.GetMonsters().Where(m => m.IsFaceup() && !IsTargetImmune(m)).OrderByDescending(m => m.Attack).ToList();
                if (oppMons.Count > 0)
                {
                    AI.SelectCard(oppMons[0]);
                    return true;
                }
            }
            return false;
        }

        private bool ShouldFishermanSpecialSummon()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Condition: opponent activated monster effect or declared attack, and we have a DarkTime card or Umi
                bool hasDarkTime = Bot.GetMonsters().Any(m => m.IsFaceup() && DarkTimeCards.Contains(m.Id))
                    || Bot.GetSpells().Any(s => s.IsFaceup() && DarkTimeCards.Contains(s.Id));
                if (!hasDarkTime) return false;

                // Make sure enemy has a target to destroy
                ClientCard target = Util.GetProblematicEnemyMonster(0, true);
                return target != null || Enemy.GetMonsters().Any(m => m.IsFaceup());
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On SS trigger: destroy 1 opponent monster
                ClientCard target = Util.GetProblematicEnemyMonster(0, true);
                if (target != null && !IsTargetImmune(target))
                {
                    AI.SelectCard(target);
                    return true;
                }
                var oppMons = Enemy.GetMonsters().Where(m => !IsTargetImmune(m)).OrderByDescending(m => m.Attack).ToList();
                if (oppMons.Count > 0)
                {
                    AI.SelectCard(oppMons[0]);
                    return true;
                }
            }
            return false;
        }

        private bool ShouldReversalBoxActivate()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                if (!Card.IsFaceup())
                {
                    // Activate face-down Reversal Box in opponent turn or Standby Phase
                    return Duel.Turn > 1 || Duel.Player == 1;
                }
                // When face-up effect triggers on opponent monster effect/attack:
                return true;
            }
            return false;
        }

        private bool ShouldSalamandraWithChainBookOfMoon()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                // Must have a FIRE monster to equip to
                ClientCard fireMon = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasAttribute(CardAttribute.Fire));
                if (fireMon == null) return false;

                // Disruption: turn 1 dangerous opponent effect monster to face-down DEF
                ClientCard target = Util.GetProblematicEnemyMonster(0, true);
                if (target != null && target.IsFaceup() && !target.IsDisabled() && !IsTargetImmune(target))
                {
                    AI.SelectCard(fireMon); // Equip target
                    AI.SelectNextCard(target); // Book of moon target
                    return true;
                }
                // Or disrupt during opponent Main Phase
                if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                {
                    var oppMon = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasType(CardType.Effect) && !IsTargetImmune(m));
                    if (oppMon != null)
                    {
                        AI.SelectCard(fireMon);
                        AI.SelectNextCard(oppMon);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ShouldGracefulSkullDiceActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY banish when opponent summons a monster
                return true;
            }
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                // Activate during Battle Phase or when opponent has monsters to swing stats
                if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep)
                {
                    return Bot.GetMonsters().Any(m => m.IsFaceup() && DarkTimeCards.Contains(m.Id))
                        || Enemy.GetMonsters().Any(m => m.IsFaceup());
                }
            }
            return false;
        }
        #endregion

        #region 3. GY Triggers & Equip
        private bool ShouldFightingFlameDragonGYEquip()
        {
            // Equip to a Warrior Fusion Monster (Flame Swordsman or Ultimate Flame Swordsman)
            ClientCard target = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasType(CardType.Fusion) && m.HasRace(CardRace.Warrior));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ShouldSalamandraGYSearch()
        {
            return true;
        }

        private bool ShouldSalamandraWithChainGYFusion()
        {
            // Banish from GY to Fusion Summon Fighting Flame Dragon or Ultimate Flame Swordsman
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                return Bot.ExtraDeck.Any(c => c.Id == CardId.FightingFlameDragon || c.Id == CardId.UltimateFlameSwordsman);
            }
            return false;
        }

        private bool ShouldFoolishGraverobberActivate()
        {
            if (Card.Location == CardLocation.SpellZone)
            {
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Banish to set a Dark Time S/T or opponent S/T
                return true;
            }
            return false;
        }
        #endregion

        #region 4. Board Breakers & Boss Summoning
        private bool ShouldGilfordTributeSummon()
        {
            // Tribute 3 monsters (tokens or spare low atk monsters) to wipe all opponent monsters
            var fodder = Bot.GetMonsters().Where(m => !AceCardIds.Contains(m.Id) || m.Id == CardId.ScapegoatToken).ToList();
            if (fodder.Count >= 3)
            {
                // Worth summoning if opponent has 1+ monsters
                if (Enemy.GetMonsters().Count(m => m.IsFaceup()) >= 1)
                {
                    AI.SelectCard(fodder);
                    return true;
                }
                // Or if going for game (Gilford is 2800 ATK)
                if (Util.GetTotalAttackingMonsterAttack(0) + 2800 >= Enemy.LifePoints)
                {
                    AI.SelectCard(fodder);
                    return true;
                }
            }
            return false;
        }

        private bool ShouldGilfordWipeTrigger()
        {
            // Trigger wipe or search on sent to GY
            return true;
        }

        private bool ShouldRedEyesExceedSummon()
        {
            // Special Summon from Extra Deck by tributing 1 face-up monster if a monster was destroyed by DarkTimeWizard this turn
            if (_darkTimeDestroyedMonThisTurn)
            {
                // Tribute opponent monster if possible, else tribute lowest atk fodder
                var oppMon = Enemy.GetMonsters().Where(m => m.IsFaceup() && !IsTargetImmune(m)).OrderByDescending(m => m.Attack).FirstOrDefault();
                if (oppMon != null)
                {
                    AI.SelectCard(oppMon);
                    return true;
                }
                var botFodder = Bot.GetMonsters().Where(m => !AceCardIds.Contains(m.Id)).OrderBy(m => m.Attack).FirstOrDefault();
                if (botFodder != null)
                {
                    AI.SelectCard(botFodder);
                    return true;
                }
            }
            return false;
        }

        private bool ShouldRedEyesExceedRevive()
        {
            // SS 1 Level 8 or lower monster from hand or GY
            return true;
        }
        #endregion

        #region 5. Starters & Searches
        private bool ShouldTenkiActivate()
        {
            return !Bot.HasInHand(CardId.SwiftPantherWarrior);
        }

        private bool ShouldSuperCriticalActivate()
        {
            return true;
        }

        private bool ShouldDarkTimeWizardActivate()
        {
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                // Choice 1: Always use Option 1 (search) unless opponent has massive board and we have lethal/survival necessity
                if (Enemy.GetMonsterCount() >= 3 && Bot.GetMonsterCount() <= 1 && Duel.Player == 0 && _isGoingSecond)
                {
                    // Desperation wipe
                    _darkTimeDestroyedMonThisTurn = true;
                    return true;
                }
                // Safe search option
                return true;
            }
            return false;
        }

        private bool ShouldFightingFlameSwordSearch()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Duel.Phase == DuelPhase.Main1))
            {
                // Search Fighting Flame Swordsman or Flame Swordsrealm
                return !Bot.HasInHand(CardId.FightingFlameSwordsman) || !Bot.HasInHandOrInSpellZone(CardId.FlameSwordsrealm);
            }
            return false;
        }
        #endregion

        #region 6. Normal Summons
        private bool ShouldFightingFlameSwordsmanSummon()
        {
            return true;
        }

        private bool ShouldSwiftPantherWarriorSummon()
        {
            return true;
        }

        private bool ShouldGearfriedSummon()
        {
            return Bot.GetMonsters().Count == 0;
        }

        private bool ShouldFishermanNormalSummon()
        {
            return Bot.GetMonsters().Count == 0;
        }
        #endregion

        #region 7. Hand Extenders & Swarm
        private bool ShouldAlligatorActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Reveal another DarkTime card to SS
                return Bot.Hand.Any(c => c != Card && DarkTimeCards.Contains(c.Id));
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // On summon: search 2 DarkTime S/T and discard 1
                return true;
            }
            return false;
        }

        private bool ShouldSwiftPantherWarriorActivate()
        {
            // Tribute 1 monster from hand or field to SS from Deck or dump S/T
            var fodder = Bot.GetMonsters().Where(m => !AceCardIds.Contains(m.Id) && m != Card).ToList();
            if (fodder.Count > 0)
            {
                return true;
            }
            if (Bot.Hand.Any(c => c != Card && !AceCardIds.Contains(c.Id) && c.IsMonster()))
            {
                return true;
            }
            return false;
        }

        private bool ShouldGearfriedActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Quick SS if monster destroyed this turn
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Pop equipped cards to search
                return Card.EquipCards != null && Card.EquipCards.Count > 0;
            }
            return false;
        }

        private bool ShouldFightingFlameSwordsmanTrigger()
        {
            return true;
        }

        private bool ShouldSleepingScapegoatsActivate()
        {
            // Activate in opponent End Phase, or during Battle Phase, or when field empty
            if (Duel.Player == 1)
            {
                bool act = Duel.Phase == DuelPhase.End || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.BattleStep;
                if (act) _sleepingScapegoatsUsedThisTurn = true;
                return act;
            }
            // On our turn: activate if we need tribute fodder for Panther or Gilford
            if (Bot.GetMonsters().Count <= 1 && Bot.HasInHand(CardId.GilfordTheLightningWarriorOfThunderbolts))
            {
                _sleepingScapegoatsUsedThisTurn = true;
                return true;
            }
            return false;
        }
        #endregion

        #region 8. Fusion & Equip Plays
        private bool ShouldFlameSwordsrealmActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Send 1 monster to SS Flame Swordsman from Extra Deck
                // Prioritize Fighting Flame Swordsman (triggers GY dump of Fighting Flame Dragon!)
                var fodder = Bot.GetMonsters().Where(m => !AceCardIds.Contains(m.Id))
                                             .OrderByDescending(m => m.Id == CardId.FightingFlameSwordsman ? 100 : 0)
                                             .ThenByDescending(m => m.Id == CardId.SalamandraTheFlyingFlameDragon ? 80 : 0)
                                             .ThenBy(m => m.Attack).ToList();
                if (fodder.Count > 0 && Bot.ExtraDeck.Any(c => c.Id == CardId.FlameSwordsman))
                {
                    AI.SelectCard(fodder[0]);
                    return true;
                }
                if (Bot.Hand.Any(c => c.IsMonster() && !AceCardIds.Contains(c.Id)) && Bot.ExtraDeck.Any(c => c.Id == CardId.FlameSwordsman))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ShouldSalamandraFusionActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Equip to a FIRE Warrior
                ClientCard fireWarrior = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasAttribute(CardAttribute.Fire) && m.HasRace(CardRace.Warrior));
                if (fireWarrior != null)
                {
                    AI.SelectCard(fireWarrior);
                    return true;
                }
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Send equipped Fusion monster to SS Ultimate Flame Swordsman (must be equipped to a Fusion monster!)
                ClientCard equipTarget = Card.EquipTarget;
                if (equipTarget != null && equipTarget.HasType(CardType.Fusion) && Bot.ExtraDeck.Any(c => c.Id == CardId.UltimateFlameSwordsman))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ShouldSalamandraEquipField()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                ClientCard warrior = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.HasRace(CardRace.Warrior));
                if (warrior != null)
                {
                    AI.SelectCard(warrior);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 9. Extra Deck Summons
        private bool ShouldFerociousFlameSwordsmanSummon()
        {
            // Cannot Link Summon if Sleeping Scapegoats was used this turn (locks Extra Deck to Fusion only)
            if (_sleepingScapegoatsUsedThisTurn) return false;

            // Link-2 using 2 monsters with different names
            var materials = Bot.GetMonsters().Where(m => m.IsFaceup() && !AceCardIds.Contains(m.Id)).ToList();
            if (materials.Count >= 2 && materials[0].Id != materials[1].Id)
            {
                AI.SelectMaterials(materials.Take(2).ToList());
                return true;
            }
            return false;
        }

        private bool ShouldFerociousFlameSwordsmanTrigger()
        {
            return true;
        }

        private bool ShouldUltimateFlameSwordsmanDoubleAtk()
        {
            // Double ATK to 5600 during damage step
            return true;
        }
        #endregion

        #region 10. Backrow Setting
        private bool ShouldSetTrap()
        {
            return Duel.Phase == DuelPhase.Main2 || (Duel.Phase == DuelPhase.Main1 && Duel.Player == 0);
        }

        private bool ShouldSetQuickSpell()
        {
            return Duel.Phase == DuelPhase.Main2;
        }
        #endregion

        #region Helper Functions
        private bool IsFlameSwordsmanRelated(ClientCard card)
        {
            return card != null && FlameSwordsmanCards.Contains(card.Id);
        }

        public override void OnNewTurn()
        {
            _darkTimeDestroyedMonThisTurn = false;
            _sleepingScapegoatsUsedThisTurn = false;
            base.OnNewTurn();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Hint 500: Release / Tribute
            if (hint == 500)
            {
                // Prioritize opponent monsters if selectable by effect / Gilford
                // Next prioritize Scapegoat tokens, then lowest ATK non-ace monsters
                var sorted = cards.OrderByDescending(c => c.Controller == 1 ? 150 : 0)
                                  .ThenByDescending(c => c.Id == CardId.ScapegoatToken ? 100 : 0)
                                  .ThenBy(c => AceCardIds.Contains(c.Id) ? 100 : 0)
                                  .ThenBy(c => c.Attack).ToList();
                return sorted.Take(min).ToList();
            }

            // Hint 501: Discard
            if (hint == 501)
            {
                // Prefer cards that trigger or provide recursion in GY
                var discardSorted = cards.OrderByDescending(c => c.Id == CardId.SalamandraTheFlyingFlameDragon ? 100 : 0)
                                         .ThenByDescending(c => c.Id == CardId.FoolishGraverobber ? 80 : 0)
                                         .ThenByDescending(c => c.Id == CardId.GracefulSkullDice ? 70 : 0)
                                         .ThenByDescending(c => c.Id == CardId.FightingFlameDragon ? 60 : 0)
                                         .ThenBy(c => AceCardIds.Contains(c.Id) ? 100 : 0)
                                         .ToList();
                return discardSorted.Take(min).ToList();
            }

            // Hint 502: Destroy
            if (hint == 502)
            {
                // If targeting enemy cards, pick highest threat/chokepoint
                // If selecting own token for Sleeping Scapegoats replace protection:
                var destSorted = cards.OrderByDescending(c => c.Controller == 1 ? (c.Attack + (CardIntelligence.IsKnownNegator(c.Id) ? 2000 : 0)) : 0)
                                      .ThenByDescending(c => c.Id == CardId.ScapegoatToken ? 100 : 0)
                                      .ThenBy(c => c.Controller == 0 ? 100 : 0)
                                      .ToList();
                return destSorted.Take(min).ToList();
            }

            // Hint 505: To hand (Search)
            if (hint == 505)
            {
                // Strategy Priority:
                // 1. Fighting Flame Swordsman / Swordsrealm if missing starter
                // 2. Alligator / Sleeping Scapegoats / Reversal Box for Dark Time line
                var searchSorted = cards.OrderByDescending(c => c.Id == CardId.FightingFlameSwordsman && !Bot.HasInHand(CardId.FightingFlameSwordsman) ? 120 : 0)
                                        .ThenByDescending(c => c.Id == CardId.FlameSwordsrealm && !Bot.HasInHandOrInSpellZone(CardId.FlameSwordsrealm) ? 110 : 0)
                                        .ThenByDescending(c => c.Id == CardId.AlligatorsSwordDragonKnight && !Bot.HasInHand(CardId.AlligatorsSwordDragonKnight) ? 100 : 0)
                                        .ThenByDescending(c => c.Id == CardId.SleepingScapegoats && !Bot.HasInHand(CardId.SleepingScapegoats) ? 90 : 0)
                                        .ThenByDescending(c => c.Id == CardId.ReversalBox && !Bot.HasInHandOrInSpellZone(CardId.ReversalBox) ? 80 : 0)
                                        .ThenByDescending(c => c.Id == CardId.DarkTimeWizard && !Bot.HasInHand(CardId.DarkTimeWizard) ? 70 : 0)
                                        .ThenByDescending(c => c.Id == CardId.FightingFlameSword && !Bot.HasInHand(CardId.FightingFlameSword) ? 65 : 0)
                                        .ThenByDescending(c => c.Id == CardId.SalamandraFusion && !Bot.HasInHand(CardId.SalamandraFusion) ? 60 : 0)
                                        .ThenByDescending(c => c.Id == CardId.GilfordTheLightningWarriorOfThunderbolts && !Bot.HasInHand(CardId.GilfordTheLightningWarriorOfThunderbolts) ? 50 : 0)
                                        .ToList();
                return searchSorted.Take(min).ToList();
            }

            // Hint 507: Equip target
            if (hint == 507)
            {
                var equipSorted = cards.OrderByDescending(c => c.Id == CardId.UltimateFlameSwordsman ? 100 : 0)
                                       .ThenByDescending(c => c.Id == CardId.FlameSwordsman ? 80 : 0)
                                       .ThenByDescending(c => c.Attack).ToList();
                return equipSorted.Take(min).ToList();
            }

            // Hint 508: Send to GY (Fighting Flame Swordsman death trigger, Foolish Graverobber)
            if (hint == 508)
            {
                // Extra Deck dump: Fighting Flame Dragon (equips to Fusion Warrior for +700 ATK & 2nd attack)
                // Deck dump: Salamandra (searches Salamandra card) or Graceful Skull Dice (disruption from GY)
                var toGraveSorted = cards.OrderByDescending(c => c.Id == CardId.FightingFlameDragon ? 120 : 0)
                                         .ThenByDescending(c => c.Id == CardId.SalamandraTheFlyingFlameDragon ? 100 : 0)
                                         .ThenByDescending(c => c.Id == CardId.GracefulSkullDice ? 90 : 0)
                                         .ThenByDescending(c => c.Id == CardId.SleepingScapegoats ? 70 : 0)
                                         .ThenByDescending(c => c.Id == CardId.DarkTimeWizard ? 60 : 0)
                                         .ThenBy(c => AceCardIds.Contains(c.Id) ? 100 : 0)
                                         .ToList();
                return toGraveSorted.Take(min).ToList();
            }

            // Hint 509: Special Summon (Foolish Graverobber revival, Sleeping Scapegoats Panther summon)
            if (hint == 509)
            {
                var spSorted = cards.OrderByDescending(c => c.Id == CardId.UltimateFlameSwordsman ? 100 : 0)
                                    .ThenByDescending(c => c.Id == CardId.GilfordTheLightningWarriorOfThunderbolts ? 95 : 0)
                                    .ThenByDescending(c => c.Id == CardId.RedEyesBlackDragonExceed ? 90 : 0)
                                    .ThenByDescending(c => c.Id == CardId.SwiftPantherWarrior ? 85 : 0)
                                    .ThenByDescending(c => c.Id == CardId.FightingFlameSwordsman ? 80 : 0)
                                    .ThenByDescending(c => c.Attack).ToList();
                return spSorted.Take(min).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int OnSelectOption(IList<long> options)
        {
            // For cards with Option 1 (search) vs Option 2 (gamble/wipe)
            // By default choose Option 1 (index 0) for safe resource generation
            return 0;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.ScapegoatToken || cardId == CardId.FishermanLegendOfTheSea)
            {
                return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
        }
        #endregion
    }
}
