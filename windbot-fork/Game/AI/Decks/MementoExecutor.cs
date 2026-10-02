// ============================================================================
// CARD AUDIT — Memento (Mementotlan — Destruction Engine & 5000 ATK Behemoth)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost      | Effect Summary                                 | Activate When                                 | NEVER Activate When                          |
// |------------------------------------|--------------|------|-------|-----------|------------------------------------------------|-----------------------------------------------|----------------------------------------------|
// | Mementotlan Angwitch               | Monster L3   | Yes  | Yes   | None/Pop  | NS/SS search Memento; pop Memento to revive L<=2| Turn 1 starter; search Tatsunootoshigo/Fusion | Already used this turn                       |
// | Mementotlan Dark Blade             | Monster L4   | Yes  | Yes   | Disc/Pop  | Discard to pop S/T; pop Memento to SS L<=3 deck| Turn 1 starter; pop self to SS Angwitch from D | No targets in Deck                           |
// | Mementotlan Tatsunootoshigo        | Monster L5   | Yes  | Yes   | None/Pop  | SS from hand; pop Memento to dump L<=5 Mementos| Extender/starter; dump Shleepy+Ghattic        | Already used this turn                       |
// | Mementotlan Shleepy                | Monster L3   | Yes  | Yes   | None      | Quick SS on destroy; NS/SS Fusion; dump on des | Trigger SS; Quick Fusion; GY dump on pop      | Already used this turn                       |
// | Mementotlan Ghattic                | Monster L2   | Yes  | Yes   | None      | Trigger SS on Memento GY dump; add Memento GY  | Revive immediately on dump; recycle resource  | Already on field                             |
// | Mementotlan Goblin                 | Monster L1   | Yes  | Yes   | Disc/Pop  | Hand: target immune; pop Memento to dump 2 deck| Protection or milling 2 Mementos from Deck    | Already used this turn                       |
// | Mementotlan Mace                   | Monster L1   | Yes  | Yes   | Disc/Pop  | Hand: Quick steal; pop Memento to search card  | Opponent Main Phase steal; Deck searcher       | Opponent has no valid monsters               |
// | Mementotlan Akihiron               | Monster L5   | Yes  | Yes   | None      | Revive on destroy; add GY/banished on destroy  | GY recursion extender                         | Already used this turn                       |
// | Mementotlan-Horned Dragon          | Monster L8   | Yes  | Yes   | None      | SS if 3+ Mementos GY; pop 3 cards on destroy   | Big beatstick / board break pop               | No targets on field                          |
// | Mementoal Tecuhtlica (Combined)    | Monster L11  | No   | No    | Shuffle 5 | 5000/5000; attack all monsters; revive on opp  | Boss monster; OTK win condition               | Cannot meet 5 names cost                     |
// | Mementomictlan (Field Spell)       | Spell Field  | Yes  | Yes   | None      | Lock opp battle S/T; revive L< on pop; EP set  | Field setup; battle protection                | Already active on field                      |
// | Mementotlan Fusion                 | Spell Quick  | Yes  | Yes   | Banish GY | Main Phase Fusion (hand/field/GY); GY searcher | Fusion Creation King/Twin Dragon              | No materials or valid target                 |
// | Mementotlan Bone Party             | Spell Quick  | Yes  | Yes   | Pop Memento| Pop 1 Memento -> search/SS Memento from Deck   | Starter/Extender; GY piercing damage          | No Memento to destroy                        |
// | Creation King                      | Fusion L9    | Yes  | Yes   | Pop/Banish| Dump 3 Memento Deck/ED; Quick pop equal opp;   | Boss Fusion; 3-card dump; Quick disruption    | Opponent controls 0 cards                    |
// | Twin Dragon                        | Fusion L7    | Yes  | Yes   | Pop Memento| Pop Memento -> search 2 Memento; float on des  | Mid-combo bridge; search 2 names              | Deck has no valid Mementos                   |
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Memento", "2026_Memento")]
    [Deck("2026_Memento", "2026_Memento")]
    public class MementoExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int Angwitch = 54550967;
            public const int DarkBlade = 18165869;
            public const int Tatsunootoshigo = 81677154;
            public const int Shleepy = 50042011;
            public const int Ghattic = 52918032;
            public const int Goblin = 17943271;
            public const int Mace = 81945676;
            public const int Akihiron = 54207171;
            public const int HornedDragon = 55272555;
            public const int MementoalTecuhtlica = 23288411; // Combined Creation (5000 ATK)
            public const int MulcharmyFuwalos = 42141493;
            public const int MulcharmyPurulia = 84192580;
            public const int SantaClaws = 46565218;
            public const int DrollAndLockBird = 94145022;
            public const int DrollAndLockBirdAlt = 94145021;

            // Spells & Traps
            public const int Mementomictlan = 43338320; // Field Spell
            public const int MementotlanFusion = 66518509;
            public const int BoneParty = 80722024;
            public const int GoblinBikerGrandBreakout = 29111045;
            public const int OneForOne = 2295441;
            public const int SuperPolymerization = 48130397;
            public const int ForbiddenDroplet = 24299458;
            public const int CalledByTheGrave = 24224831;
            public const int CalledByTheGraveAlt = 24224830;
            public const int TripleTacticsTalent = 25311006;
            public const int PotOfProsperity = 84211599;
            public const int PotOfSloth = 98476659;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int RadiantTyphoonVision = 20508881;
            public const int EvenlyMatched = 15693423;

            // Extra Deck
            public const int CreationKing = 14529511; // Mementomictlan Tecuhtlica - Creation King (3000 ATK)
            public const int TwinDragon = 19181420;   // Mementotlan Twin Dragon (2800 ATK)
            public const int ChimeraKing = 1769875;
            public const int BerfometMythicalKing = 69601012;
            public const int Garura = 11765832;
            public const int MudragonOfTheSwamp = 54757758;
            public const int PredaplantDragostapelia = 69946549;
            public const int FavoriteHeroFlameWingman = 13243125;
            public const int HyperinvokedAeon = 33166263;
            public const int MixousiaTheConfounder = 80843006;
            public const int ProxyFMagician = 12450071;
            public const int SPLittleKnight = 29301451;
            public const int SPLittleKnightAlt = 29301450;
            public const int FiendsmithSequence = 49867899;
            public const int MelomelodyBrassDjinn = 88942504;
        }

        public MementoPlugin Plugin { get; }

        public MementoExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new MementoPlugin(this);

            // Register Combo Starters & Bait Cards
            BaitPlanner.RegisterComboStarters(CardId.Angwitch, CardId.DarkBlade, CardId.Tatsunootoshigo, CardId.BoneParty);
            BaitPlanner.RegisterBaitCards(CardId.PotOfSloth, CardId.PotOfProsperity, CardId.TripleTacticsTalent, CardId.RadiantTyphoonVision);

            RegisterComboLines();
            RegisterExecutors();
        }

        private void RegisterComboLines()
        {
            // ── Line 1: Angwitch 1-Card Starter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Memento-Angwitch-Starter",
                RequiredCards = new List<int> { CardId.Angwitch },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Angwitch, ActionType = ExecutorType.Summon, Description = "Normal Summon Angwitch" }
                },
                FallbackLineName = "Memento-DarkBlade-Starter"
            });

            // ── Line 2: Dark Blade 1-Card Starter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Memento-DarkBlade-Starter",
                RequiredCards = new List<int> { CardId.DarkBlade },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.DarkBlade, ActionType = ExecutorType.Summon, Description = "Normal Summon Dark Blade" },
                    new() { CardId = CardId.DarkBlade, ActionType = ExecutorType.Activate, Description = "Pop self to Special Summon Angwitch from Deck" }
                },
                FallbackLineName = "Memento-Tatsuno-Starter"
            });

            // ── Line 3: Tatsunootoshigo Solo Starter ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Memento-Tatsuno-Starter",
                RequiredCards = new List<int> { CardId.Tatsunootoshigo },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Tatsunootoshigo, ActionType = ExecutorType.SpSummon, Description = "Special Summon Tatsunootoshigo from hand" },
                    new() { CardId = CardId.Tatsunootoshigo, ActionType = ExecutorType.Activate, Description = "Pop self to send Shleepy + Ghattic to GY" }
                }
            });
        }

        private void RegisterExecutors()
        {
            // ═══════════════════════════════════════════════════════════════
            //  TIER 0: BOARD BREAKERS, HANDTRAPS & QUICK DISRUPTIONS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedActivate);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosActivate);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyPuruliaActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBirdAlt, DrollActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGraveAlt, CalledByTheGraveActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletActivate);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolyActivate);

            // Santa Claws: Tribute opponent problematic monsters
            AddExecutor(ExecutorType.SpSummon, CardId.SantaClaws, SantaClawsSpSummon);

            // Mace & Goblin Quick Effects from Hand (When Combined Creation is on field)
            AddExecutor(ExecutorType.Activate, CardId.Mace, MaceHandActivate);
            AddExecutor(ExecutorType.Activate, CardId.Goblin, GoblinHandActivate);

            // Creation King Quick Field Pop Disruption
            AddExecutor(ExecutorType.Activate, CardId.CreationKing, CreationKingActivate);

            // Predaplant Dragostapelia & S:P Little Knight Quick Effects
            AddExecutor(ExecutorType.Activate, CardId.PredaplantDragostapelia, DragostapeliaActivate);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnightAlt, SPLittleKnightActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 1: BOARD SETUP, SEARCHERS & SPELL CARDS
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.Activate, CardId.PotOfSloth, PotOfSlothActivate);
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityActivate);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentActivate);
            AddExecutor(ExecutorType.Activate, CardId.OneForOne, OneForOneActivate);
            AddExecutor(ExecutorType.Activate, CardId.GoblinBikerGrandBreakout, GoblinBikerActivate);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, MSTActivate);
            AddExecutor(ExecutorType.Activate, CardId.RadiantTyphoonVision, RadiantTyphoonActivate);

            // Field Spell Setup
            AddExecutor(ExecutorType.Activate, CardId.Mementomictlan, MementomictlanActivate);

            // Quick-Play Spell: Bone Party
            AddExecutor(ExecutorType.Activate, CardId.BoneParty, BonePartyActivate);

            // Quick-Play Spell: Mementotlan Fusion (Field/Hand or GY Banish)
            AddExecutor(ExecutorType.Activate, CardId.MementotlanFusion, MementotlanFusionActivate);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 2: NORMAL SUMMONS & MONSTER EFFECTS
            // ═══════════════════════════════════════════════════════════════
            // Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.Angwitch, AngwitchSummon);
            AddExecutor(ExecutorType.Summon, CardId.DarkBlade, DarkBladeSummon);
            AddExecutor(ExecutorType.Summon, CardId.Ghattic, SmallMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.Goblin, SmallMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.Mace, SmallMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.Shleepy, SmallMonsterSummon);

            // Field Ignition & Trigger Effects
            AddExecutor(ExecutorType.Activate, CardId.Angwitch, AngwitchEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkBlade, DarkBladeEffect);
            AddExecutor(ExecutorType.Activate, CardId.Tatsunootoshigo, TatsunootoshigoEffect);
            AddExecutor(ExecutorType.Activate, CardId.Ghattic, GhatticEffect);
            AddExecutor(ExecutorType.Activate, CardId.Shleepy, ShleepyEffect);
            AddExecutor(ExecutorType.Activate, CardId.Akihiron, AkihironEffect);
            AddExecutor(ExecutorType.Activate, CardId.HornedDragon, HornedDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.TwinDragon, TwinDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.Goblin, GoblinFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.Mace, MaceFieldEffect);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 3: SPECIAL SUMMONS & BOSS SUMMONS
            // ═══════════════════════════════════════════════════════════════
            // Tatsunootoshigo Special Summon from Hand
            AddExecutor(ExecutorType.SpSummon, CardId.Tatsunootoshigo, TatsunootoshigoSpSummon);

            // Shleepy Quick Special Summon from Hand
            AddExecutor(ExecutorType.SpSummon, CardId.Shleepy);

            // Horned Dragon Special Summon from Hand
            AddExecutor(ExecutorType.SpSummon, CardId.HornedDragon, HornedDragonSpSummon);

            // Boss: Mementoal Tecuhtlica - Combined Creation (5000 ATK)
            AddExecutor(ExecutorType.SpSummon, CardId.MementoalTecuhtlica, CombinedCreationSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.MementoalTecuhtlica, CombinedCreationEffect);

            // Extra Deck Fusion Summons
            AddExecutor(ExecutorType.SpSummon, CardId.CreationKing);
            AddExecutor(ExecutorType.SpSummon, CardId.TwinDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.PredaplantDragostapelia);
            AddExecutor(ExecutorType.SpSummon, CardId.Garura);
            AddExecutor(ExecutorType.SpSummon, CardId.MudragonOfTheSwamp);
            AddExecutor(ExecutorType.SpSummon, CardId.ChimeraKing);
            AddExecutor(ExecutorType.SpSummon, CardId.BerfometMythicalKing);

            // Link & Xyz Summons
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnightAlt, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ProxyFMagician);
            AddExecutor(ExecutorType.SpSummon, CardId.MelomelodyBrassDjinn);

            // ═══════════════════════════════════════════════════════════════
            //  TIER 4: SPELL SETTING & REPOSITIONING
            // ═══════════════════════════════════════════════════════════════
            AddExecutor(ExecutorType.SpellSet, CardId.EvenlyMatched, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGraveAlt, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.BoneParty, SpellSetStrategy);
            AddExecutor(ExecutorType.SpellSet, CardId.MementotlanFusion, SpellSetStrategy);

            AddExecutor(ExecutorType.Repos, RepositionStrategy);
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISRUPTIONS & HANDTRAPS
        // ═══════════════════════════════════════════════════════════════

        private bool EvenlyMatchedActivate()
        {
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
            {
                int myCards = Bot.GetMonsterCount() + Bot.GetSpellCount();
                int enemyCards = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
                return enemyCards > myCards;
            }
            return false;
        }

        private bool MulcharmyFuwalosActivate()
        {
            return Duel.Player == 1 && Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0;
        }

        private bool MulcharmyPuruliaActivate()
        {
            return Duel.Player == 1 && Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0;
        }

        private bool DrollActivate()
        {
            ClientCard lastCard = LastChainCard;
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool CalledByTheGraveActivate()
        {
            ClientCard lastCard = LastChainCard;
            if (lastCard != null && lastCard.Controller == 1)
            {
                ClientCard target = Enemy.Graveyard.FirstOrDefault(c => c.IsMonster() && c.Name == lastCard.Name);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            if (Duel.Player == 1)
            {
                ClientCard target = Enemy.Graveyard.Where(c => c.IsMonster()).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool ForbiddenDropletActivate()
        {
            var faceupEnemies = Enemy.GetMonsters().Where(m => m.IsFaceup() && !m.IsDisabled()).ToList();
            if (faceupEnemies.Count == 0) return false;

            // Only activate if opponent monster is an actual threat:
            // On opponent's turn: activate if opponent activates effect, attacks, or has lethal
            // On our turn: ONLY activate if opponent has a known negator, floodgate, or high ATK monster
            bool isThreat = false;
            if (Duel.Player == 1)
            {
                isThreat = (LastChainCard != null && LastChainCard.Controller == 1) ||
                           Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle ||
                           faceupEnemies.Any(m => CardIntelligence.IsKnownNegator(m.Id) || CardIntelligence.IsFloodgate(m.Id));
            }
            else
            {
                isThreat = faceupEnemies.Any(m => CardIntelligence.IsKnownNegator(m.Id) || CardIntelligence.IsFloodgate(m.Id) || m.Attack >= 2500);
            }

            if (!isThreat) return false;

            // Pick non-essential card to send as cost
            // Prioritize already-used spells or GY triggers (Shleepy, Ghattic, Akihiron)
            var costFromField = Bot.GetSpells().FirstOrDefault(s => s.IsFaceup() && s.IsCode(CardId.Mementomictlan))
                             ?? Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && (m.IsCode(CardId.Shleepy) || m.IsCode(CardId.Ghattic) || m.IsCode(CardId.Akihiron) || m.IsCode(CardId.Goblin) || m.IsCode(CardId.Mace)));

            var costFromHand = Bot.Hand.FirstOrDefault(c => c.IsMonster() && (c.IsCode(CardId.Shleepy) || c.IsCode(CardId.Ghattic) || c.IsCode(CardId.Akihiron)));

            var selectedCost = costFromField ?? costFromHand;

            if (selectedCost != null)
            {
                AI.SelectCard(selectedCost);
                var targetEnemy = faceupEnemies
                    .OrderByDescending(m => CardIntelligence.IsKnownNegator(m.Id) ? 10000 : m.Attack)
                    .First();
                AI.SelectNextCard(targetEnemy);
                return true;
            }
            return false;
        }

        private bool SuperPolyActivate()
        {
            if (Enemy.GetMonsterCount() >= 2)
            {
                // Can fuse Garura (same Type & Attribute, diff names) or Mudragon (same Attribute, diff Types)
                return true;
            }
            return false;
        }

        private bool SantaClawsSpSummon()
        {
            var oppThreats = Enemy.GetMonsters()
                .Where(m => m.IsFaceup())
                .OrderByDescending(m => {
                    int score = m.Attack;
                    if (CardIntelligence.IsFloodgate(m.Id)) score += 10000;
                    if (CardIntelligence.IsKnownNegator(m.Id)) score += 8000;
                    return score;
                }).ToList();

            if (oppThreats.Count > 0 && oppThreats[0].Attack >= 2500)
            {
                AI.SelectCard(oppThreats[0]);
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool MaceHandActivate()
        {
            // Steal 1 face-up monster opponent controls during opp Main Phase
            if (Duel.Player == 1 && Duel.IsMainPhase() && Plugin.BoardAssessor.HasCombinedCreationOnField())
            {
                var target = Plugin.ThreatEvaluator.GetBestStealTarget(Enemy.GetMonsters());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool GoblinHandActivate()
        {
            // Give target immunity to Memento monsters during Main Phase
            if (Duel.IsMainPhase() && Plugin.BoardAssessor.HasCombinedCreationOnField())
            {
                ClientCard lastCard = LastChainCard;
                if (lastCard != null && lastCard.Controller == 1)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CreationKingActivate()
        {
            // Case 1: From GY -> Banish self to search Field Spell Mementomictlan (Option 0)
            if (Card.Location == CardLocation.Grave || ActivateDescription == Util.GetStringId(CardId.CreationKing, 0))
            {
                return !Bot.HasInHand(CardId.Mementomictlan) && !Bot.HasInSpellZone(CardId.Mementomictlan);
            }

            // Case 2: On Fusion Summon -> Dump 3 Memento cards from Deck/ED to GY (Option 1)
            if (ActivateDescription == Util.GetStringId(CardId.CreationKing, 1))
            {
                return true;
            }

            // Case 3: Quick Effect -> Destroy equal number of Mementos and opponent cards (Option 2)
            if (ActivateDescription == Util.GetStringId(CardId.CreationKing, 2) || Card.Location == CardLocation.MonsterZone)
            {
                // CRITICAL SAFETY GUARD: NEVER SACRIFICE CREATION KING OR COMBINED CREATION!
                var sacrificialFodder = Bot.GetMonsters()
                    .Where(m => m.IsFaceup() &&
                                !m.IsCode(CardId.CreationKing) &&
                                !m.IsCode(CardId.MementoalTecuhtlica))
                    .OrderByDescending(m => Plugin.MaterialEvaluator.GetDestructionPriority(m))
                    .ToList();

                var oppTargets = Enemy.GetMonsters().Where(m => m.IsFaceup()).Concat(Enemy.GetSpells()).ToList();

                if (sacrificialFodder.Count > 0 && oppTargets.Count > 0)
                {
                    var bestOpp = Plugin.ThreatEvaluator.GetBestOpponentTarget(oppTargets);
                    if (bestOpp != null)
                    {
                        AI.SelectCard(sacrificialFodder.First());
                        AI.SelectNextCard(bestOpp);
                        return true;
                    }
                }

                // If no safe fodder exists, DO NOT ACTIVATE! (Never destroy Creation King itself!)
                return false;
            }

            return false;
        }

        private bool DragostapeliaActivate()
        {
            var oppTarget = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled());
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool SPLittleKnightActivate()
        {
            var oppTarget = Plugin.ThreatEvaluator.GetBestOpponentTarget(Enemy.GetMonsters().Concat(Enemy.GetSpells()));
            if (oppTarget != null)
            {
                AI.SelectCard(oppTarget);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  SEARCHERS & SPELLS
        // ═══════════════════════════════════════════════════════════════

        private bool PotOfSlothActivate()
        {
            return (Enemy.GetMonsterCount() + Enemy.GetSpellCount()) >= 1;
        }

        private bool PotOfProsperityActivate()
        {
            var banishList = Bot.ExtraDeck.Where(c =>
                c.Id != CardId.CreationKing &&
                c.Id != CardId.TwinDragon &&
                c.Id != CardId.SPLittleKnight &&
                c.Id != CardId.PredaplantDragostapelia
            ).Take(6).ToList();

            if (banishList.Count >= 3)
            {
                AI.SelectCard(banishList);
                return true;
            }
            return false;
        }

        private bool TripleTacticsTalentActivate()
        {
            if (Enemy.GetMonsterCount() > 0)
            {
                AI.SelectOption(1); // Take control
                return true;
            }
            AI.SelectOption(0); // Draw 2
            return true;
        }

        private bool OneForOneActivate()
        {
            var discard = Bot.Hand.FirstOrDefault(c => c.IsMonster() && !c.IsCode(CardId.Angwitch) && !c.IsCode(CardId.MementoalTecuhtlica));
            if (discard != null)
            {
                AI.SelectCard(discard);
                AI.SelectNextCard(CardId.Goblin, CardId.Mace);
                return true;
            }
            return false;
        }

        private bool GoblinBikerActivate()
        {
            var tribute = Bot.GetMonsters().FirstOrDefault(m => !m.IsCode(CardId.MementoalTecuhtlica) && !m.IsCode(CardId.CreationKing));
            if (tribute != null)
            {
                AI.SelectCard(tribute);
                AI.SelectNextCard(CardId.Goblin);
                return true;
            }
            return false;
        }

        private bool MSTActivate()
        {
            var oppSpell = Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup() && CardIntelligence.IsFloodgate(s.Id))
                        ?? Enemy.GetSpells().FirstOrDefault(s => s.IsFacedown());
            if (oppSpell != null)
            {
                AI.SelectCard(oppSpell);
                return true;
            }
            // Can trigger Radiant Typhoon Vision
            var myRadiant = Bot.GetSpells().FirstOrDefault(s => s.IsCode(CardId.RadiantTyphoonVision));
            if (myRadiant != null)
            {
                AI.SelectCard(myRadiant);
                return true;
            }
            return false;
        }

        private bool RadiantTyphoonActivate()
        {
            AI.SelectOption(1); // Add MST from Deck/GY
            return true;
        }

        private bool MementomictlanActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return !Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == CardId.Mementomictlan);
            }
            return true; // GY / End Phase reset effect
        }

        private bool BonePartyActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Banish from GY to grant piercing damage
                var boss = Bot.GetMonsters().FirstOrDefault(m => m.IsCode(CardId.MementoalTecuhtlica))
                        ?? Bot.GetMonsters().FirstOrDefault(m => m.IsCode(CardId.CreationKing));
                if (boss != null)
                {
                    AI.SelectCard(boss);
                    return true;
                }
                return false;
            }

            // Quick-Play activation: destroy 1 Memento -> SS Angwitch or Tatsunootoshigo
            var target = Bot.GetMonsters()
                .Where(m => m.IsFaceup() && !m.IsCode(CardId.MementoalTecuhtlica) && !m.IsCode(CardId.CreationKing))
                .OrderByDescending(m => Plugin.MaterialEvaluator.GetDestructionPriority(m))
                .FirstOrDefault()
                ?? Bot.Hand.FirstOrDefault(c => c.IsMonster() && c.IsCode(CardId.Shleepy) || c.IsCode(CardId.Ghattic) || c.IsCode(CardId.Akihiron));

            if (target != null)
            {
                AI.SelectCard(target);
                AI.SelectNextCard(CardId.Angwitch, CardId.Tatsunootoshigo, CardId.DarkBlade);
                return true;
            }
            return false;
        }

        private bool MementotlanFusionActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Banish self from GY, destroy 1 monster -> search Memento Spell/Trap
                var popTarget = Bot.GetMonsters()
                    .Where(m => m.IsFaceup() && !m.IsCode(CardId.MementoalTecuhtlica) && !m.IsCode(CardId.CreationKing))
                    .OrderByDescending(m => Plugin.MaterialEvaluator.GetDestructionPriority(m))
                    .FirstOrDefault();

                if (popTarget != null)
                {
                    AI.SelectCard(popTarget);
                    AI.SelectNextCard(CardId.BoneParty, CardId.Mementomictlan);
                    return true;
                }
                return false;
            }

            // Fusion Summon Creation King or Twin Dragon
            AI.SelectCard(CardId.CreationKing, CardId.TwinDragon);
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  NORMAL SUMMONS & MONSTER EFFECTS
        // ═══════════════════════════════════════════════════════════════

        private bool AngwitchSummon() => true;
        private bool DarkBladeSummon() => true;
        private bool SmallMonsterSummon() => Bot.GetMonsterCount() == 0;

        private bool AngwitchEffect()
        {
            // Case 1: Search effect on Summon
            AI.SelectCard(CardId.Tatsunootoshigo, CardId.BoneParty, CardId.MementoalTecuhtlica, CardId.MementotlanFusion, CardId.Shleepy);

            // Case 2: Pop Memento to revive Level <= 2 Memento from GY
            var gyRevive = Bot.Graveyard.FirstOrDefault(c => c.IsCode(CardId.Ghattic) || c.IsCode(CardId.Mace) || c.IsCode(CardId.Goblin));
            if (gyRevive != null)
            {
                AI.SelectCard(gyRevive);
                var popTarget = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && m.IsCode(CardId.Angwitch));
                if (popTarget != null)
                {
                    AI.SelectNextCard(popTarget);
                }
            }
            return true;
        }

        private bool DarkBladeEffect()
        {
            // Case 1: On Summon pop opponent Spell/Trap
            var oppSpell = Enemy.GetSpells().FirstOrDefault();
            if (oppSpell != null)
            {
                AI.SelectCard(CardId.Shleepy, CardId.Ghattic, CardId.Akihiron);
                AI.SelectNextCard(oppSpell);
            }

            // Case 2: Pop self to Special Summon Level <= 3 Memento from Deck (Angwitch!)
            AI.SelectCard(Card.Id);
            AI.SelectNextCard(CardId.Angwitch, CardId.Mace, CardId.Goblin);
            return true;
        }

        private bool TatsunootoshigoEffect()
        {
            // Destroy self (Lv5) -> Send Mementos whose total levels <= 5 (Shleepy Lv3 + Ghattic Lv2 = 5)
            AI.SelectCard(Card.Id);
            AI.SelectNextCard(CardId.Shleepy, CardId.Ghattic);
            return true;
        }

        private bool TatsunootoshigoSpSummon()
        {
            return Bot.GetMonsters().All(m => m == null || MementoResourceLoop.IsMementoCard(m));
        }

        private bool GhatticEffect()
        {
            // On Summon add Memento from GY to hand
            AI.SelectCard(CardId.Tatsunootoshigo, CardId.Angwitch, CardId.BoneParty, CardId.MementotlanFusion, CardId.MementoalTecuhtlica);
            return true;
        }

        private bool ShleepyEffect()
        {
            // Fusion Summon on Summon or foolish on destroy
            AI.SelectCard(CardId.CreationKing, CardId.TwinDragon);
            AI.SelectNextCard(CardId.MementoalTecuhtlica, CardId.Ghattic, CardId.Akihiron);
            return true;
        }

        private bool AkihironEffect()
        {
            AI.SelectCard(CardId.Tatsunootoshigo, CardId.Angwitch, CardId.BoneParty);
            return true;
        }

        private bool HornedDragonSpSummon()
        {
            return Plugin.ResourceLoop.CanSpecialSummonHornedDragon();
        }

        private bool HornedDragonEffect()
        {
            // Pop 3 face-up cards (including 1 Memento you control)
            var friendlyPop = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsCode(CardId.MementoalTecuhtlica));
            var oppTargets = Enemy.GetMonsters().Where(m => m.IsFaceup()).Take(2).ToList();
            if (friendlyPop != null && oppTargets.Count > 0)
            {
                AI.SelectCard(friendlyPop);
                AI.SelectNextCard(oppTargets);
                return true;
            }
            return false;
        }

        private bool TwinDragonEffect()
        {
            // Destroy 1 Memento -> search 2 Memento monsters
            var popTarget = Bot.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsCode(CardId.MementoalTecuhtlica))
                         ?? Bot.Hand.FirstOrDefault(c => c.IsMonster() && c.IsCode(CardId.Shleepy));
            if (popTarget != null)
            {
                AI.SelectCard(popTarget);
                AI.SelectNextCard(CardId.MementoalTecuhtlica, CardId.Mace, CardId.Goblin);
                return true;
            }
            return true;
        }

        private bool GoblinFieldEffect()
        {
            AI.SelectCard(Card.Id);
            AI.SelectNextCard(CardId.MementoalTecuhtlica, CardId.Shleepy, CardId.Ghattic);
            return true;
        }

        private bool MaceFieldEffect()
        {
            AI.SelectCard(Card.Id);
            AI.SelectNextCard(CardId.MementotlanFusion, CardId.BoneParty, CardId.Mementomictlan);
            return true;
        }

        private bool CombinedCreationSpSummon()
        {
            return Plugin.ResourceLoop.CanSpecialSummonCombinedCreation();
        }

        private bool CombinedCreationEffect()
        {
            // Opponent activated effect -> revive Memento from GY/hand
            AI.SelectCard(CardId.CreationKing, CardId.TwinDragon, CardId.Angwitch, CardId.Ghattic, CardId.Mace);
            return true;
        }

        private bool SPLittleKnightSpSummon()
        {
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool SpellSetStrategy()
        {
            return Bot.GetSpellCount() < 4;
        }

        private bool RepositionStrategy()
        {
            if (Card.IsCode(CardId.MementoalTecuhtlica) || Card.IsCode(CardId.CreationKing))
            {
                return Card.IsDefense();
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  OVERRIDE: OnSelectCard — Safe Hint & Fodder/Boss Management
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // ── Hint 501: Discard Priority (Tribute / Discard for Cost) ──
            if (hint == 501)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.Shleepy)) return 10;
                    if (c.IsCode(CardId.Ghattic)) return 20;
                    if (c.IsCode(CardId.Akihiron)) return 30;
                    if (c.IsCode(CardId.Goblin)) return 40;
                    if (c.IsCode(CardId.Mace)) return 50;
                    if (CardIntelligence.IsHandtrap(c.Id)) return 5000;
                    if (c.IsCode(CardId.MementoalTecuhtlica) || c.IsCode(CardId.CreationKing)) return 10000;
                    return 100;
                }).ToList();

                if (sorted.Count >= min)
                    return Util.CheckSelectCount(sorted.Take(max).ToList(), cards, min, max);
            }

            // ── Hint 502 / 503: Destroy / Banish Selection ──
            if (hint == 502 || hint == 503)
            {
                // If opponent cards are options, prioritize destroying opponent threats!
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyTargets.Count >= min)
                {
                    var sortedEnemy = enemyTargets.OrderByDescending(c => CardIntelligence.GetCardThreatScore(c, hint)).ToList();
                    return Util.CheckSelectCount(sortedEnemy.Take(Math.Min(max, sortedEnemy.Count)).ToList(), cards, min, max);
                }

                // If destroying friendly cards, STRICTLY PROTECT BOSSES (Creation King & Combined Creation)!
                var friendlyTargets = cards.Where(c => c != null && c.Controller == 0).ToList();
                if (friendlyTargets.Count >= min)
                {
                    // Filter out bosses first!
                    var safeTargets = friendlyTargets
                        .Where(c => !c.IsCode(CardId.CreationKing) && !c.IsCode(CardId.MementoalTecuhtlica))
                        .OrderByDescending(c => Plugin.MaterialEvaluator.GetDestructionPriority(c))
                        .ToList();

                    if (safeTargets.Count >= min)
                    {
                        return Util.CheckSelectCount(safeTargets.Take(Math.Min(max, safeTargets.Count)).ToList(), cards, min, max);
                    }

                    // Only if no safe targets exist (and not cancelable), fallback
                    var sortedFriendly = friendlyTargets.OrderByDescending(c => Plugin.MaterialEvaluator.GetDestructionPriority(c)).ToList();
                    return Util.CheckSelectCount(sortedFriendly.Take(Math.Min(max, sortedFriendly.Count)).ToList(), cards, min, max);
                }
            }

            // ── Hint 504: Send to GY / Dump / Cost ──
            if (hint == 504)
            {
                // Case A: Creation King 3-Card Dump from Deck & Extra Deck
                if (min == 3 && max == 3)
                {
                    var dumpList = new List<ClientCard>();

                    // 1. Combined Creation (Boss to GY so it can be Special Summoned from GY!)
                    var boss = cards.FirstOrDefault(c => c.IsCode(CardId.MementoalTecuhtlica) &&
                                                         !Bot.HasInGraveyard(CardId.MementoalTecuhtlica) &&
                                                         !Bot.HasInHand(CardId.MementoalTecuhtlica));
                    if (boss != null) dumpList.Add(boss);

                    // 2. Ghattic (Self-reviver on dump, adds Memento from GY!)
                    var ghattic = cards.FirstOrDefault(c => c.IsCode(CardId.Ghattic) &&
                                                           !Bot.HasInGraveyard(CardId.Ghattic) &&
                                                           !Bot.HasInMonstersZone(CardId.Ghattic));
                    if (ghattic != null && !dumpList.Any(d => d.Id == ghattic.Id)) dumpList.Add(ghattic);

                    // 3. Twin Dragon from Extra Deck (great name in GY) or Shleepy / Mace / Goblin
                    var candidates = cards
                        .Where(c => !dumpList.Any(d => d.Id == c.Id))
                        .OrderByDescending(c => {
                            int score = 0;
                            if (c.IsCode(CardId.TwinDragon)) score += 800;
                            if (c.IsCode(CardId.Shleepy) && !Bot.HasInGraveyard(CardId.Shleepy)) score += 600;
                            if (c.IsCode(CardId.Mace) && !Bot.HasInGraveyard(CardId.Mace)) score += 500;
                            if (c.IsCode(CardId.Goblin) && !Bot.HasInGraveyard(CardId.Goblin)) score += 400;
                            if (c.IsCode(CardId.Akihiron) && !Bot.HasInGraveyard(CardId.Akihiron)) score += 300;
                            if (c.IsCode(CardId.HornedDragon) && !Bot.HasInGraveyard(CardId.HornedDragon)) score += 200;
                            if (!Bot.HasInGraveyard(c.Id)) score += 100;
                            return score;
                        })
                        .GroupBy(c => c.Id)
                        .Select(g => g.First())
                        .ToList();

                    foreach (var c in candidates)
                    {
                        if (dumpList.Count >= 3) break;
                        dumpList.Add(c);
                    }

                    if (dumpList.Count >= 3)
                        return dumpList.Take(3).ToList();
                }

                // Case B: Deck Dumps (Tatsunootoshigo Lv5 budget dump, Goblin mill 2, Shleepy dump 1)
                if (cards.All(c => c.Location == CardLocation.Deck))
                {
                    var sortedDeck = cards.OrderByDescending(c => {
                        int score = 0;
                        if (c.IsCode(CardId.Ghattic) && !Bot.HasInMonstersZone(CardId.Ghattic)) score += 1000;
                        if (c.IsCode(CardId.Shleepy) && !Bot.HasInGraveyard(CardId.Shleepy)) score += 900;
                        if (c.IsCode(CardId.MementoalTecuhtlica) && !Bot.HasInGraveyard(CardId.MementoalTecuhtlica) && !Bot.HasInHand(CardId.MementoalTecuhtlica)) score += 850;
                        if (c.IsCode(CardId.Mace) && !Bot.HasInGraveyard(CardId.Mace)) score += 700;
                        if (c.IsCode(CardId.Goblin) && !Bot.HasInGraveyard(CardId.Goblin)) score += 600;
                        if (c.IsCode(CardId.Akihiron) && !Bot.HasInGraveyard(CardId.Akihiron)) score += 500;
                        if (c.IsCode(CardId.DarkBlade) && !Bot.HasInGraveyard(CardId.DarkBlade)) score += 400;
                        if (c.IsCode(CardId.Angwitch) && !Bot.HasInGraveyard(CardId.Angwitch)) score += 300;
                        if (!Bot.HasInGraveyard(c.Id)) score += 100;
                        return score;
                    }).ToList();

                    int selectCount = Math.Max(min, Math.Min(max, 1));
                    var picked = sortedDeck.Take(selectCount).ToList();
                    if (picked.Count >= min)
                        return Util.CheckSelectCount(picked, cards, min, max);
                }

                // Case C: Hand / Field Costs (Droplet cost, etc.)
                var sortedCost = cards.OrderByDescending(c => {
                    int score = 0;
                    if (c.Location == CardLocation.SpellZone && c.IsFaceup() && c.IsCode(CardId.Mementomictlan)) score += 1000;
                    if (c.IsCode(CardId.Shleepy)) score += 900;
                    if (c.IsCode(CardId.Ghattic)) score += 800;
                    if (c.IsCode(CardId.Akihiron)) score += 700;
                    if (c.IsCode(CardId.Goblin) || c.IsCode(CardId.Mace)) score += 500;
                    // Protect key starters and boss monsters
                    if (c.IsCode(CardId.Angwitch) || c.IsCode(CardId.DarkBlade)) score -= 2000;
                    if (c.IsCode(CardId.BoneParty) || c.IsCode(CardId.MementotlanFusion)) score -= 3000;
                    if (c.IsCode(CardId.SuperPolymerization) || c.IsCode(CardId.CalledByTheGrave)) score -= 4000;
                    if (c.IsCode(CardId.MementoalTecuhtlica) || c.IsCode(CardId.CreationKing)) score -= 10000;
                    return score;
                }).ToList();

                if (sortedCost.Count >= min)
                    return Util.CheckSelectCount(sortedCost.Take(Math.Min(max, sortedCost.Count)).ToList(), cards, min, max);
            }

            // ── Hint 506: Add to Hand / Search Priority ──
            if (hint == 506)
            {
                var sorted = cards.OrderByDescending(c => {
                    int score = 0;
                    // If we need the Boss and have 4+ Mementos in GY/hand
                    if (c.IsCode(CardId.MementoalTecuhtlica) && !Bot.HasInHand(CardId.MementoalTecuhtlica) && !Bot.HasInGraveyard(CardId.MementoalTecuhtlica))
                        score += 900;
                    if (c.IsCode(CardId.Tatsunootoshigo) && !Bot.HasInHand(CardId.Tatsunootoshigo))
                        score += 850;
                    if (c.IsCode(CardId.BoneParty) && !Bot.HasInHand(CardId.BoneParty))
                        score += 800;
                    if (c.IsCode(CardId.MementotlanFusion) && !Bot.HasInHand(CardId.MementotlanFusion))
                        score += 750;
                    if (c.IsCode(CardId.Mementomictlan) && !Bot.HasInHand(CardId.Mementomictlan) && !Bot.HasInSpellZone(CardId.Mementomictlan))
                        score += 700;
                    if (c.IsCode(CardId.Angwitch) && !Bot.HasInHand(CardId.Angwitch))
                        score += 650;
                    if (c.IsCode(CardId.HornedDragon) && !Bot.HasInHand(CardId.HornedDragon))
                        score += 600;
                    if (c.IsCode(CardId.Mace) && !Bot.HasInHand(CardId.Mace))
                        score += 550;
                    if (c.IsCode(CardId.Goblin) && !Bot.HasInHand(CardId.Goblin))
                        score += 500;
                    return score;
                }).GroupBy(c => c.Id).Select(g => g.First()).ToList();

                if (sorted.Count >= min)
                    return Util.CheckSelectCount(sorted.Take(Math.Min(max, sorted.Count)).ToList(), cards, min, max);
            }

            // ── Hint 507: Return to Deck (Combined Creation Special Summon Procedure) ──
            if (hint == 507)
            {
                // Must select 5 different Memento monsters from hand/GY
                var sorted = cards
                    .Where(c => c != null && c.IsCode(CardId.MementoalTecuhtlica) == false)
                    .GroupBy(c => c.Id)
                    .Select(g => g.First())
                    .OrderByDescending(c => {
                        int score = 0;
                        // Extra Deck monsters return to Extra Deck — excellent to recycle!
                        if (c.IsCode(CardId.TwinDragon) || c.IsCode(CardId.CreationKing)) score += 1000;
                        // Main Deck searchers/extenders replenish deck ammo
                        if (c.IsCode(CardId.Tatsunootoshigo)) score += 500;
                        if (c.IsCode(CardId.DarkBlade)) score += 450;
                        if (c.IsCode(CardId.Shleepy)) score += 400;
                        if (c.IsCode(CardId.Angwitch)) score += 350;
                        if (c.IsCode(CardId.Goblin)) score += 300;
                        if (c.IsCode(CardId.Mace)) score += 250;
                        if (c.IsCode(CardId.Akihiron)) score += 200;
                        // Keep Ghattic in GY if possible to revive
                        if (c.IsCode(CardId.Ghattic)) score += 50;
                        return score;
                    }).ToList();

                if (sorted.Count >= min)
                    return Util.CheckSelectCount(sorted.Take(Math.Min(max, sorted.Count)).ToList(), cards, min, max);
            }

            // ── Hint 509: Special Summon Selection ──
            if (hint == 509)
            {
                // If summoning from Extra Deck
                var extraTargets = cards.Where(c => c.Location == CardLocation.Extra).ToList();
                if (extraTargets.Count >= min)
                {
                    var sortedExtra = extraTargets.OrderByDescending(c => {
                        if (c.IsCode(CardId.CreationKing)) return 2000;
                        if (c.IsCode(CardId.TwinDragon)) return 1500;
                        if (c.IsCode(CardId.SPLittleKnight)) return 1200;
                        return 100;
                    }).ToList();
                    return Util.CheckSelectCount(sortedExtra.Take(Math.Min(max, sortedExtra.Count)).ToList(), cards, min, max);
                }

                // If Special Summoning from GY / Hand (Angwitch revive / Twin Dragon float / Bone Party)
                var sortedSp = cards.OrderByDescending(c => {
                    int score = 0;
                    if (c.IsCode(CardId.Angwitch)) score += 1000;
                    if (c.IsCode(CardId.Tatsunootoshigo)) score += 900;
                    if (c.IsCode(CardId.DarkBlade)) score += 800;
                    if (c.IsCode(CardId.Ghattic)) score += 750;
                    if (c.IsCode(CardId.Shleepy)) score += 700;
                    if (c.IsCode(CardId.HornedDragon)) score += 650;
                    if (c.IsCode(CardId.Mace)) score += 600;
                    if (c.IsCode(CardId.Goblin)) score += 550;
                    return score;
                }).ToList();

                if (sortedSp.Count >= min)
                    return Util.CheckSelectCount(sortedSp.Take(Math.Min(max, sortedSp.Count)).ToList(), cards, min, max);
            }

            // ── Hint 510: Set from GY (Field Spell End Phase effect) ──
            if (hint == 510)
            {
                var sorted = cards.OrderByDescending(c => {
                    if (c.IsCode(CardId.BoneParty)) return 2000;
                    if (c.IsCode(CardId.MementotlanFusion)) return 1500;
                    return 100;
                }).ToList();

                if (sorted.Count >= min)
                    return Util.CheckSelectCount(sorted.Take(Math.Min(max, sorted.Count)).ToList(), cards, min, max);
            }

            // ── Hint 511: Fusion Material Selection ──
            if (hint == 511)
            {
                var sorted = cards.OrderByDescending(c => Plugin.MaterialEvaluator.GetFusionMaterialScore(c)).ToList();
                if (sorted.Count >= min)
                    return Util.CheckSelectCount(sorted.Take(max).ToList(), cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ═══════════════════════════════════════════════════════════════
        //  OVERRIDE: OnSelectOption — Choice Dispatcher
        // ═══════════════════════════════════════════════════════════════

        public override int OnSelectOption(IList<long> options)
        {
            // Default to first option unless specialized
            return 0;
        }
    }
}
