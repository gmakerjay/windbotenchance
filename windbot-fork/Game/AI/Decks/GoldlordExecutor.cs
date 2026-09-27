// ============================================================
// CARD AUDIT โ€” 2026 Goldlord (Eldlich + Zombie World Control)
// ============================================================
// | Card Name              | Type       | OPT? | Cost        | Effect Summary                         | Activate When                     | NEVER When                           |
// |------------------------|------------|------|-------------|----------------------------------------|-----------------------------------|--------------------------------------|
// | Eldlich the Golden Lord| Monster    | HOPT | Send 1 S/T  | Hand: Send card on field to GY / GY: SS| Removal / 3500 ATK recur boss     | Hand empty / No S/T to send          |
// | Lord of Heav. Prison   | Monster    | HOPT | Reveal/SS   | Protect set cards, SS & set S/T from dk| Main Phase reveal / Set activated | Already summoned this turn           |
// | Doomking Balerdroch    | Monster    | HOPT | None        | Standby GY SS; Quick Negate / Banish   | Standby Phase / Zombie eff chained| Target own cards                     |
// | Uni-Zombie             | Monster    | HOPT | Discard/Send| Deck dump Zombie / Discard to level mod| GY setup (Banshee/Doomking/Eldlich| No targets in Deck                   |
// | Necroworld Banshee     | Monster    | HOPT | Banish self | Activate Zombie World from Deck/Hand   | Zombie World not on field         | Zombie World already active          |
// | Zombie World           | Field Spell| No   | None        | All monsters on field/GY become Zombie | Field setup / Floodgate           | Already active on field              |
// | Scarlet Sanguine       | Normal Trap| HOPT | None        | SS Zombie from Deck/GY; GY: Set GL Trap| Need Eldlich/Doomking; GY EndPhase| Already used this turn               |
// | Black Awakening        | Normal Sp. | HOPT | None        | SS Zombie from Deck/Hand; GY: Set GL Tr| Main Phase summon / GY End Phase  | Already used this turn               |
// | White Destiny          | Quick-Play | HOPT | None        | SS Zombie from Hand/GY; GY: Set GL Trap| Quick SS / GY End Phase           | Already used this turn               |
// | Conquistador of GL     | Normal Trap| HOPT | None        | SS Level 5 Trap Monster + Destroy 1 cd | Opponent threat; GY End Phase     | Monster zone full (>=5)              |
// | Huaquero of GL         | Normal Trap| HOPT | None        | SS Level 5 Trap Monster + Banish 1 GY  | Opponent GY play; GY End Phase    | Monster zone full (>=5)              |
// | Golden Land Forever!   | Counter Tr.| HOPT | Trib 1 Zomb | Negate S/T/Monster effect & destroy    | High-threat enemy activation      | No Zombie to tribute                 |
// | Glorious Eldlixir      | Normal Trap| HOPT | Pay 800 LP  | SS Lv10 Trap Monster + Bounce / Set ban| Threat monster / Recycle banished | Monster zone full / LP <= 800        |
// | Torrential Tribute     | Normal Trap| No   | None        | Destroy all monsters on field          | Monster summoned; Board wipe      | Wipes own board without protection   |
// | Fallen Angel of GL     | Fusion/Sp  | HOPT | Trib LIGHT  | Contact SS from Extra; floats Eldlich  | Free 2500 body -> Link/Chaos -> Eld| No LIGHT Zombie on field             |
// | Chaos Angel            | Synchro 10 | HOPT | None        | Treat LIGHT/DARK as Tuner; Banish 1 cd | Level 5+5 Trap monsters / Uni-Zomb| No valid materials                   |
// | Eldlich Mad Golden Lord| Fusion     | HOPT | Trib 1 Zomb | Unaffected battle/eff; steal opp mon   | Steal enemy boss monster          | No Zombie to tribute                 |
// | S:P Little Knight      | Link 2     | HOPT | None        | Banish card on summon; Quick banish 2  | Enemy threat / Dodge removal      | Sacrificing Ace cards                |
// ============================================================
// ACE CARDS:
//   Primary Bosses: Eldlich the Golden Lord (95440946), Doomking Balerdroch (39185163), Eldlich the Mad Golden Lord (74889525)
//   Secondary Bosses: Chaos Angel (22850702), Fallen Angel of the Golden Land (43143567)
// COMBO STARTERS:
//   1. Uni-Zombie (49959355) โ€” Dumps Banshee (Zombie World) + Doomking
//   2. Necroworld Banshee (66570171) โ€” Direct Zombie World activation
//   3. Scarlet Sanguine (20612097) / Black Awakening (68829754) โ€” SS Eldlich from Deck
// WIN CONDITION: Zombie World + Doomking loop (1 Negate + 1 Non-targeting Banish per turn) + Protected Backrow Disruptions + 3500 ATK Beatdown
// GOING 1ST END BOARD: Zombie World + Doomking in GY + Eldlich on field + Lord of Heavenly Prison revealed + Set Conquistador + Set Forever! + Set Sanguine
// GOING 2ND GAMEPLAN: Lava Golem tributes -> Eldlich hand send -> Fallen Angel contact -> Chaos Angel banish -> 3500+ ATK OTK
// CHOKEPOINTS: Uni-Zombie normal summon negate -> Fallback to Eldlixirs / Lord of the Heavenly Prison
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
    [Deck("2026_Goldlord", "2026_Goldlord")]
    public class _2026_GoldlordExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int EldlichTheGoldenLord = 95440946;
            public const int LordOfTheHeavenlyPrison = 9822220;
            public const int UniZombie = 49959355;
            public const int NecroworldBanshee = 66570171;
            public const int DoomkingBalerdroch = 39185163;
            public const int LavaGolem = 102380;

            // Spells & Traps
            public const int ZombieWorld = 4064256;
            public const int EldlixirOfScarletSanguine = 20612097;
            public const int GoldenLandForever = 56984514;
            public const int ConquistadorOfTheGoldenLand = 20590515;
            public const int HuaqueroOfTheGoldenLand = 93191801;
            public const int EldlixirOfTheGloriousGoldenLand = 22669793;
            public const int EldlixirOfBlackAwakening = 68829754;
            public const int EldlixirOfWhiteDestiny = 94224458;
            public const int TorrentialTribute = 53582587;
            public const int InfiniteImpermanence = 10045474;

            // Hand Traps & Staples
            public const int AshBlossom = 14558127;
            public const int MulcharmyFuwalos = 42141493;
            public const int DrollAndLockBird = 94145021;

            // Extra Deck
            public const int EldlichTheMadGoldenLord = 74889525;
            public const int FallenAngelOfTheGoldenLand = 43143567;
            public const int ChaosAngel = 22850702;
            public const int SPLittleKnight = 29301450;
            public const int MudragonOfTheSwamp = 54757758;
        }

        // Ace Cards
        private static readonly int[] AceCardIds = {
            CardId.EldlichTheGoldenLord, CardId.EldlichTheMadGoldenLord,
            CardId.DoomkingBalerdroch, CardId.ChaosAngel
        };

        // Granular OPT & State Tracking
        private bool _eldlichHandUsed = false;
        private bool _eldlichGyUsed = false;
        private int _lordRevealedTurn = -1;
        private bool _lordSummonedThisTurn = false;
        private bool _uniZombieDeckSendUsed = false;
        private bool _uniZombieDiscardUsed = false;
        private bool _bansheeUsed = false;
        private bool _doomkingBanishUsed = false;
        private bool _doomkingNegateUsed = false;
        private bool _gloriousEldlixirUsed = false;
        private bool _blackAwakeningUsed = false;
        private bool _whiteDestinyUsed = false;
        private bool _scarletSanguineUsed = false;
        private bool _conquistadorUsed = false;
        private bool _huaqueroUsed = false;

        private bool IsLordRevealed => _lordRevealedTurn != -1 && (Duel.Turn == _lordRevealedTurn || Duel.Turn == _lordRevealedTurn + 1);

        private bool HasEldlichOnField()
        {
            return Bot.HasInMonstersZone(CardId.EldlichTheGoldenLord) || Bot.HasInMonstersZone(CardId.EldlichTheMadGoldenLord);
        }

        private bool HasEldlichInFieldOrGy()
        {
            return HasEldlichOnField() || Bot.HasInGraveyard(CardId.EldlichTheGoldenLord) || Bot.HasInGraveyard(CardId.EldlichTheMadGoldenLord);
        }

        private bool IsZombieWorldActive()
        {
            return Bot.HasInSpellZone(CardId.ZombieWorld) || Enemy.HasInSpellZone(CardId.ZombieWorld);
        }

        public _2026_GoldlordExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            ResourcePlan.RegisterAceCards(AceCardIds);
            HeuristicGuard.RegisterAceCards(AceCardIds);

            // โ”€โ”€ Combo Router โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "UniZombie-Standard-Setup",
                RequiredCards = new List<int> { CardId.UniZombie },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.UniZombie, ActionType = ExecutorType.Summon, Description = "Normal Summon Uni-Zombie" },
                    new() { CardId = CardId.UniZombie, ActionType = ExecutorType.Activate, Description = "Send Banshee -> Activate Zombie World" },
                    new() { CardId = CardId.UniZombie, ActionType = ExecutorType.Activate, Description = "Discard card -> Send Doomking" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Sanguine-Starter",
                RequiredCards = new List<int> { CardId.EldlixirOfScarletSanguine },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.EldlixirOfScarletSanguine, ActionType = ExecutorType.Activate, Description = "SS Eldlich from Deck" }
                },
                EndBoardScore = 80
            });

            BaitPlanner.RegisterComboStarters(CardId.UniZombie, CardId.EldlixirOfBlackAwakening, CardId.EldlixirOfScarletSanguine);
            BaitPlanner.RegisterBaitCards(CardId.LordOfTheHeavenlyPrison, CardId.EldlixirOfWhiteDestiny);
            ChainAdvisor.RegisterHighValueTargets(CardId.EldlichTheGoldenLord, CardId.DoomkingBalerdroch, CardId.LordOfTheHeavenlyPrison, CardId.GoldenLandForever);

            // ==========================================
            // EXECUTOR REGISTRATION (Priority Driven)
            // ==========================================

            // TIER 1: Hand Traps & Fast Negation
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceActivate);
            AddExecutor(ExecutorType.Activate, CardId.GoldenLandForever, GoldenLandForeverActivate);

            // TIER 2: Board Breakers (Going 2nd)
            AddExecutor(ExecutorType.SpSummon, CardId.LavaGolem, LavaGolemSummon);

            // TIER 3: Standby Phase Recursion (Doomking Balerdroch CRITICAL FIX)
            AddExecutor(ExecutorType.Activate, CardId.DoomkingBalerdroch, DoomkingBalerdrochActivate);

            // TIER 4: Reactive Traps (Disruptions & SS)
            AddExecutor(ExecutorType.Activate, CardId.ConquistadorOfTheGoldenLand, ConquistadorActivate);
            AddExecutor(ExecutorType.Activate, CardId.HuaqueroOfTheGoldenLand, HuaqueroActivate);
            AddExecutor(ExecutorType.Activate, CardId.EldlixirOfScarletSanguine, ScarletSanguineActivate);
            AddExecutor(ExecutorType.Activate, CardId.EldlixirOfTheGloriousGoldenLand, GloriousEldlixirActivate);
            AddExecutor(ExecutorType.Activate, CardId.TorrentialTribute, TorrentialTributeActivate);

            // TIER 5: Starters, Field Spells & Lord of Heavenly Prison
            AddExecutor(ExecutorType.Activate, CardId.ZombieWorld, ZombieWorldActivate);
            AddExecutor(ExecutorType.Activate, CardId.NecroworldBanshee, NecroworldBansheeActivate);
            AddExecutor(ExecutorType.Activate, CardId.LordOfTheHeavenlyPrison, LordOfTheHeavenlyPrisonActivate);
            AddExecutor(ExecutorType.Activate, CardId.UniZombie, UniZombieActivate);
            AddExecutor(ExecutorType.Summon, CardId.UniZombie, UniZombieSummon);
            AddExecutor(ExecutorType.Summon, CardId.NecroworldBanshee, NecroworldBansheeSummon);

            // TIER 6: Eldlich Engine Spells & Boss Hand/GY Activations
            AddExecutor(ExecutorType.Activate, CardId.EldlixirOfBlackAwakening, BlackAwakeningActivate);
            AddExecutor(ExecutorType.Activate, CardId.EldlixirOfWhiteDestiny, WhiteDestinyActivate);
            AddExecutor(ExecutorType.Activate, CardId.EldlichTheGoldenLord, EldlichGoldenLordActivate);

            // TIER 7: Extra Deck Summons
            AddExecutor(ExecutorType.SpSummon, CardId.FallenAngelOfTheGoldenLand, FallenAngelSpSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.FallenAngelOfTheGoldenLand, FallenAngelActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, ChaosAngelSpSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.ChaosAngel, ChaosAngelActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.EldlichTheMadGoldenLord);
            AddExecutor(ExecutorType.Activate, CardId.EldlichTheMadGoldenLord, EldlichMadGoldenLordActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.MudragonOfTheSwamp);
            AddExecutor(ExecutorType.Activate, CardId.MudragonOfTheSwamp, MudragonActivate);

            // TIER 8: Backrow Sets (MP2 / End of Turn)
            AddExecutor(ExecutorType.SpellSet, CardId.GoldenLandForever);
            AddExecutor(ExecutorType.SpellSet, CardId.EldlixirOfScarletSanguine);
            AddExecutor(ExecutorType.SpellSet, CardId.ConquistadorOfTheGoldenLand);
            AddExecutor(ExecutorType.SpellSet, CardId.HuaqueroOfTheGoldenLand);
            AddExecutor(ExecutorType.SpellSet, CardId.EldlixirOfTheGloriousGoldenLand);
            AddExecutor(ExecutorType.SpellSet, CardId.TorrentialTribute);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.EldlixirOfWhiteDestiny, () => Util.IsTurn1OrMain2());

            // LAST: Reposition
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand() => true; // Always choose Going First for full Zombie World + Backrow setup

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _eldlichHandUsed = false;
            _eldlichGyUsed = false;
            _uniZombieDeckSendUsed = false;
            _uniZombieDiscardUsed = false;
            _bansheeUsed = false;
            _doomkingBanishUsed = false;
            _doomkingNegateUsed = false;
            _gloriousEldlixirUsed = false;
            _blackAwakeningUsed = false;
            _whiteDestinyUsed = false;
            _scarletSanguineUsed = false;
            _conquistadorUsed = false;
            _huaqueroUsed = false;
            _lordSummonedThisTurn = false;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.AshBlossom, CardId.MulcharmyFuwalos, CardId.DrollAndLockBird)) return 800;
            if (c.IsCode(CardId.LordOfTheHeavenlyPrison)) return 700;
            if (c.IsCode(CardId.FallenAngelOfTheGoldenLand)) return 100; // Floats on sent to GY
            if (c.IsCode(CardId.ConquistadorOfTheGoldenLand, CardId.HuaqueroOfTheGoldenLand)) return 200;
            return 300;
        }

        protected override bool IsBoardStrongEnough()
        {
            int disruptionCount = 0;

            if (Bot.HasInMonstersZone(CardId.DoomkingBalerdroch) && IsZombieWorldActive())
                disruptionCount += 2;

            if (Bot.HasInMonstersZone(CardId.EldlichTheGoldenLord))
                disruptionCount++;

            if (Bot.HasInMonstersZone(CardId.EldlichTheMadGoldenLord))
                disruptionCount += 2;

            if (Bot.HasInMonstersZone(CardId.ChaosAngel))
                disruptionCount += 2;

            int setBackrow = Bot.GetSpells().Count(c => c != null && c.IsFacedown());
            disruptionCount += setBackrow;

            if (Bot.HasInHand(CardId.AshBlossom) || Bot.HasInHand(CardId.InfiniteImpermanence))
                disruptionCount++;

            return disruptionCount >= 4 && (Bot.GetMonsterCount() >= 2 || Bot.HasInMonstersZone(CardId.DoomkingBalerdroch));
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        protected override bool IsInGrindGame()
        {
            int handCount = Bot.GetHandCount();
            int monsterCount = Bot.GetMonsterCount();
            int setCount = Bot.GetSpells().Count(c => c != null && c.IsFacedown());

            bool lowResources = (handCount <= 1 && monsterCount <= 1 && setCount <= 1);
            return Duel.Turn >= 6 || lowResources;
        }

        protected override bool CanDealLethal()
        {
            if (Scorer != null && Scorer.HasLethal()) return true;
            int total = 0;
            foreach (ClientCard card in Bot.MonsterZone)
            {
                if (card != null && card.IsFaceup() && !card.Attacked)
                    total += card.Attack;
            }
            return total >= Enemy.LifePoints;
        }

        protected override bool ShouldSkipCombo() => Duel.Phase == DuelPhase.Main1 && CanDealLethal();

        // ==========================================
        // TIER 1 & 2: Hand Traps & Board Breakers
        // ==========================================

        private bool AshBlossomActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool MulcharmyFuwalosActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (Bot.GetMonsterCount() > 0 || Bot.GetSpellCount() > 0) return false;
            if (Duel.Player != 1) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return true;
        }

        private bool DrollAndLockBirdActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player != 1) return false;
            return true;
        }

        private bool InfiniteImpermanenceActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (Duel.Player == 0 && Duel.IsMainPhase())
            {
                var target = Util.GetProblematicEnemyMonster(0, true);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return DefaultInfiniteImpermanence();
        }

        private bool GoldenLandForeverActivate()
        {
            if (!HasEldlichOnField()) return false;
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;

            // Prioritize tribute: Conquistador > Huaquero > Non-Ace Zombie > Doomking (last resort)
            ClientCard tribute = Bot.GetMonsters()
                .Where(c => c != null && c.IsMonster() && c.HasRace(CardRace.Zombie))
                .OrderBy(c => {
                    if (c.IsCode(CardId.ConquistadorOfTheGoldenLand, CardId.HuaqueroOfTheGoldenLand)) return 0;
                    if (c.IsCode(CardId.FallenAngelOfTheGoldenLand)) return 1;
                    if (!IsAceCard(c) && !c.IsCode(CardId.DoomkingBalerdroch)) return 2;
                    if (c.IsCode(CardId.DoomkingBalerdroch)) return 3; // Will revive in Standby
                    return 4;
                })
                .FirstOrDefault();

            if (tribute == null) return false;

            AI.SelectCard(tribute);
            DecisionTracer.TraceActivate("GoldenLandForeverActivate", $"Negated {LastChainCard.Name} by tributing {tribute.Name}");
            return true;
        }

        private bool LavaGolemSummon()
        {
            if (Bot.LifePoints <= 1000) return false;
            if (Enemy.GetMonsterCount() < 2) return false;

            // If we have overwhelming board with Zombie World + Doomking, keep it
            if (Bot.HasInMonstersZone(CardId.DoomkingBalerdroch) && IsZombieWorldActive() && Enemy.GetMonsterCount() < 3)
                return false;

            return true;
        }

        // ==========================================
        // TIER 3: Standby Phase Recursion (CRITICAL BUG FIX)
        // ==========================================

        private bool DoomkingBalerdrochActivate()
        {
            // 1. GY Standby Phase Revival (Revives whenever Field Spell is active!)
            if (Card.Location == CardLocation.Grave)
            {
                if (!IsZombieWorldActive()) return false;
                // CRITICAL FIX: Always revive in Standby Phase! Do NOT check enemy monster count!
                DecisionTracer.TraceActivate("DoomkingBalerdrochActivate", "Revive Doomking Balerdroch in Standby Phase under Zombie World");
                return true;
            }

            // 2. Monster Zone Quick Effect triggers (When a Zombie effect is activated)
            if (Card.Location == CardLocation.MonsterZone && !Card.IsDisabled())
            {
                int opt = (int)ActivateDescription;

                // Effect 0: Negate that effect
                if (opt == Util.GetStringId(CardId.DoomkingBalerdroch, 0) || !_doomkingNegateUsed)
                {
                    if (LastChainCard != null && LastChainCard.Controller == 1)
                    {
                        _doomkingNegateUsed = true;
                        DecisionTracer.TraceActivate("DoomkingBalerdrochActivate", $"Negate chaining effect of {LastChainCard.Name}");
                        return true;
                    }
                }

                // Effect 1: Banish 1 monster from field or GY
                if (opt == Util.GetStringId(CardId.DoomkingBalerdroch, 1) || !_doomkingBanishUsed)
                {
                    ClientCard target = Util.GetProblematicEnemyMonster(0, true)
                        ?? Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c)).OrderByDescending(c => c.Attack).FirstOrDefault()
                        ?? Enemy.Graveyard.Where(c => c != null && c.IsMonster() && c.IsCanRevive()).OrderByDescending(c => c.Attack).FirstOrDefault()
                        ?? Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());

                    if (target != null)
                    {
                        _doomkingBanishUsed = true;
                        AI.SelectCard(target);
                        DecisionTracer.TraceActivate("DoomkingBalerdrochActivate", $"Banish monster target: {target.Name} (Loc: {target.Location})");
                        return true;
                    }
                }
            }

            return false;
        }

        // ==========================================
        // TIER 4: Reactive Traps (Disruptions & SS)
        // ==========================================

        private bool ConquistadorActivate()
        {
            if (_conquistadorUsed) return false;

            // GY End Phase Search Effect: Banish to set Eldlixir from Deck
            if (Card.Location == CardLocation.Grave)
            {
                int remainingEldlixir = Bot.GetRemainingCount(CardId.EldlixirOfScarletSanguine, 3) +
                                        Bot.GetRemainingCount(CardId.EldlixirOfBlackAwakening, 2) +
                                        Bot.GetRemainingCount(CardId.EldlixirOfWhiteDestiny, 1);
                if (remainingEldlixir == 0) return false;

                AI.SelectCard(new[] {
                    CardId.EldlixirOfScarletSanguine,
                    CardId.EldlixirOfBlackAwakening,
                    CardId.EldlixirOfWhiteDestiny
                });
                _conquistadorUsed = true;
                DecisionTracer.TraceActivate("ConquistadorActivate", "GY effect: Set Eldlixir from Deck");
                return true;
            }

            // Field Activation: Special Summon as Level 5 Monster + Pop 1 face-up card if Eldlich present
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                if (Bot.GetMonsterCount() >= 5) return false;

                if (HasEldlichOnField())
                {
                    var target = Util.GetProblematicEnemyCard()
                        ?? Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c)).OrderByDescending(c => c.Attack).FirstOrDefault()
                        ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled());

                    if (target != null)
                    {
                        AI.SelectCard(target);
                        _conquistadorUsed = true;
                        DecisionTracer.TraceActivate("ConquistadorActivate", $"SS Conquistador and destroy {target.Name}");
                        return true;
                    }
                }

                // End Phase: SS for board presence
                if (Duel.Phase == DuelPhase.End)
                {
                    _conquistadorUsed = true;
                    return true;
                }
            }

            return false;
        }

        private bool HuaqueroActivate()
        {
            if (_huaqueroUsed) return false;

            // GY End Phase Search Effect: Banish to set Eldlixir from Deck
            if (Card.Location == CardLocation.Grave)
            {
                int remainingEldlixir = Bot.GetRemainingCount(CardId.EldlixirOfScarletSanguine, 3) +
                                        Bot.GetRemainingCount(CardId.EldlixirOfBlackAwakening, 2) +
                                        Bot.GetRemainingCount(CardId.EldlixirOfWhiteDestiny, 1);
                if (remainingEldlixir == 0) return false;

                AI.SelectCard(new[] {
                    CardId.EldlixirOfScarletSanguine,
                    CardId.EldlixirOfBlackAwakening,
                    CardId.EldlixirOfWhiteDestiny
                });
                _huaqueroUsed = true;
                DecisionTracer.TraceActivate("HuaqueroActivate", "GY effect: Set Eldlixir from Deck");
                return true;
            }

            // Field Activation: Special Summon as Level 5 Monster + Banish 1 GY card if Eldlich present
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                if (Bot.GetMonsterCount() >= 5) return false;

                if (HasEldlichOnField())
                {
                    var target = Enemy.Graveyard.Where(c => c != null && c.IsMonster() && c.IsCanRevive()).OrderByDescending(c => c.Attack).FirstOrDefault()
                        ?? Enemy.Graveyard.FirstOrDefault(c => c != null && !c.IsDisabled());

                    if (target != null)
                    {
                        AI.SelectCard(target);
                        _huaqueroUsed = true;
                        DecisionTracer.TraceActivate("HuaqueroActivate", $"SS Huaquero and banish {target.Name} from enemy GY");
                        return true;
                    }
                }

                // End Phase: SS for board presence
                if (Duel.Phase == DuelPhase.End)
                {
                    _huaqueroUsed = true;
                    return true;
                }
            }

            return false;
        }

        private bool ScarletSanguineActivate()
        {
            if (_scarletSanguineUsed) return false;

            // Field Activation: Special Summon Zombie from Deck or GY
            if (Card.Location == CardLocation.SpellZone && (Card.IsFacedown() || Duel.Player == 0))
            {
                if (Bot.GetMonsterCount() >= 5) return false;

                int targetId;
                if (!HasEldlichOnField())
                {
                    targetId = CardId.EldlichTheGoldenLord;
                }
                else if (IsZombieWorldActive() && !Bot.HasInGraveyard(CardId.DoomkingBalerdroch) && !Bot.HasInMonstersZone(CardId.DoomkingBalerdroch) && Bot.GetRemainingCount(CardId.DoomkingBalerdroch, 1) > 0)
                {
                    targetId = CardId.DoomkingBalerdroch;
                }
                else
                {
                    targetId = CardId.EldlichTheGoldenLord;
                }

                AI.SelectCard(targetId);
                _scarletSanguineUsed = true;
                DecisionTracer.TraceActivate("ScarletSanguineActivate", $"SS Zombie target {targetId} from Deck/GY");
                return true;
            }

            // GY End Phase Search Effect: Banish to set Golden Land S/T from Deck
            if (Card.Location == CardLocation.Grave)
            {
                int remainingGL = Bot.GetRemainingCount(CardId.ConquistadorOfTheGoldenLand, 3) +
                                  Bot.GetRemainingCount(CardId.HuaqueroOfTheGoldenLand, 2) +
                                  Bot.GetRemainingCount(CardId.GoldenLandForever, 2);
                if (remainingGL == 0) return false;

                AI.SelectCard(new[] {
                    CardId.GoldenLandForever,
                    CardId.ConquistadorOfTheGoldenLand,
                    CardId.HuaqueroOfTheGoldenLand
                });
                _scarletSanguineUsed = true;
                DecisionTracer.TraceActivate("ScarletSanguineActivate", "GY effect: Set Golden Land S/T from Deck");
                return true;
            }

            return false;
        }

        private bool GloriousEldlixirActivate()
        {
            if (_gloriousEldlixirUsed) return false;
            if (Bot.LifePoints <= 800) return false;

            int opt = (int)ActivateDescription;

            // Effect 0: SS Level 10 Trap Monster (1500/2800) + Bounce 1 monster if Eldlich present
            if (opt == Util.GetStringId(CardId.EldlixirOfTheGloriousGoldenLand, 0) || Card.Location == CardLocation.SpellZone)
            {
                if (Bot.GetMonsterCount() >= 5) return false;

                if (HasEldlichOnField())
                {
                    var target = Util.GetProblematicEnemyMonster(0, true)
                        ?? Enemy.MonsterZone.GetHighestAttackMonster(true);
                    if (target != null)
                    {
                        AI.SelectCard(target);
                    }
                }
                _gloriousEldlixirUsed = true;
                DecisionTracer.TraceActivate("GloriousEldlixirActivate", "SS Glorious as Level 10 Trap Monster");
                return true;
            }

            // Effect 1: Recycle a banished Golden Land or Eldlixir card
            if (opt == Util.GetStringId(CardId.EldlixirOfTheGloriousGoldenLand, 1) || Card.Location == CardLocation.Grave)
            {
                var target = Bot.Banished.FirstOrDefault(c => c != null && 
                    (c.IsCode(CardId.ConquistadorOfTheGoldenLand, CardId.HuaqueroOfTheGoldenLand, CardId.EldlixirOfScarletSanguine)));
                if (target != null)
                {
                    AI.SelectCard(target);
                    _gloriousEldlixirUsed = true;
                    DecisionTracer.TraceActivate("GloriousEldlixirActivate", $"Recycle banished card: {target.Name}");
                    return true;
                }
            }

            return false;
        }

        private bool TorrentialTributeActivate()
        {
            if (IsLordRevealed) return true; // Our set cards are completely protected by Lord!

            // If we have Mad Golden Lord or Eldlich with destruction immunity, it's safe
            if (Bot.HasInMonstersZone(CardId.EldlichTheMadGoldenLord)) return true;

            int enemyMonsters = Enemy.GetMonsterCount();
            int botDestroyable = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));

            if (enemyMonsters >= 2 && enemyMonsters > botDestroyable)
            {
                DecisionTracer.TraceActivate("TorrentialTributeActivate", "Wipe field via Torrential Tribute");
                return true;
            }

            return false;
        }

        // ==========================================
        // TIER 5: Starters & Field Spells
        // ==========================================

        private bool ZombieWorldActivate()
        {
            if (IsZombieWorldActive()) return false;
            return true;
        }

        private bool NecroworldBansheeActivate()
        {
            if (_bansheeUsed) return false;
            if (IsZombieWorldActive()) return false;
            if (Bot.GetRemainingCount(CardId.ZombieWorld, 2) == 0) return false;

            _bansheeUsed = true;
            DecisionTracer.TraceActivate("NecroworldBansheeActivate", "Banish Banshee to activate Zombie World directly from Deck");
            return true;
        }

        private bool NecroworldBansheeSummon()
        {
            if (!IsZombieWorldActive() && Bot.GetMonsterCount() == 0) return true;
            return false;
        }

        private bool UniZombieSummon()
        {
            if (ShouldSkipCombo()) return false;
            return true;
        }

        private bool UniZombieActivate()
        {
            if (ShouldSkipCombo()) return false;
            int opt = (int)ActivateDescription;

            // Effect 2: Send Zombie from Deck to GY (Priority: Banshee -> Doomking -> Eldlich)
            if (opt == Util.GetStringId(CardId.UniZombie, 1) || !_uniZombieDeckSendUsed)
            {
                // Priority 1: Zombie World via Banshee
                if (!IsZombieWorldActive() && !Bot.HasInGraveyard(CardId.NecroworldBanshee) && Bot.GetRemainingCount(CardId.NecroworldBanshee, 2) > 0)
                {
                    AI.SelectCard(CardId.UniZombie);
                    AI.SelectNextCard(CardId.NecroworldBanshee);
                    _uniZombieDeckSendUsed = true;
                    DecisionTracer.TraceActivate("UniZombieActivate", "Deck send Banshee -> Activate Zombie World");
                    return true;
                }

                // Priority 2: Doomking Balerdroch
                if (!Bot.HasInGraveyard(CardId.DoomkingBalerdroch) && Bot.GetRemainingCount(CardId.DoomkingBalerdroch, 1) > 0)
                {
                    AI.SelectCard(CardId.UniZombie);
                    AI.SelectNextCard(CardId.DoomkingBalerdroch);
                    _uniZombieDeckSendUsed = true;
                    DecisionTracer.TraceActivate("UniZombieActivate", "Deck send Doomking Balerdroch");
                    return true;
                }

                // Priority 3: Eldlich the Golden Lord
                if (!Bot.HasInGraveyard(CardId.EldlichTheGoldenLord) && Bot.GetRemainingCount(CardId.EldlichTheGoldenLord, 2) > 0)
                {
                    AI.SelectCard(CardId.UniZombie);
                    AI.SelectNextCard(CardId.EldlichTheGoldenLord);
                    _uniZombieDeckSendUsed = true;
                    DecisionTracer.TraceActivate("UniZombieActivate", "Deck send Eldlich the Golden Lord");
                    return true;
                }
            }

            // Effect 1: Discard 1 card from hand (Level modulation)
            if (opt == Util.GetStringId(CardId.UniZombie, 0) || !_uniZombieDiscardUsed)
            {
                if (Bot.GetHandCount() == 0) return false;

                var discardCard = Bot.Hand.FirstOrDefault(c => c != null && 
                    (c.IsCode(CardId.DoomkingBalerdroch, CardId.NecroworldBanshee, CardId.EldlichTheGoldenLord,
                              CardId.ConquistadorOfTheGoldenLand, CardId.HuaqueroOfTheGoldenLand, CardId.EldlixirOfScarletSanguine)))
                    ?? (Bot.GetHandCount() > 2 ? Bot.Hand.FirstOrDefault(c => c != null && c.Id != CardId.UniZombie) : null);

                if (discardCard != null)
                {
                    AI.SelectCard(CardId.UniZombie);
                    AI.SelectNextCard(discardCard);
                    _uniZombieDiscardUsed = true;
                    DecisionTracer.TraceActivate("UniZombieActivate", $"Discard {discardCard.Name} for level mod");
                    return true;
                }
            }

            return false;
        }

        private bool LordOfTheHeavenlyPrisonActivate()
        {
            // Hand Reveal: During Main Phase
            if (Card.Location == CardLocation.Hand && !IsLordRevealed && Duel.IsMainPhase())
            {
                _lordRevealedTurn = Duel.Turn;
                DecisionTracer.TraceActivate("LordOfTheHeavenlyPrisonActivate", "Reveal Lord of the Heavenly Prison: Protect Set cards");
                return true;
            }

            // Trigger SS when a Set S/T is activated
            if (Card.Location == CardLocation.Hand && LastChainCard != null && !_lordSummonedThisTurn)
            {
                _lordSummonedThisTurn = true;
                if (IsLordRevealed)
                {
                    _lordRevealedTurn = -1;
                    AI.SelectCard(
                        CardId.EldlixirOfScarletSanguine,
                        CardId.GoldenLandForever,
                        CardId.ConquistadorOfTheGoldenLand,
                        CardId.TorrentialTribute,
                        CardId.ZombieWorld
                    );
                }
                DecisionTracer.TraceActivate("LordOfTheHeavenlyPrisonActivate", "SS Lord of the Heavenly Prison + Set S/T from Deck");
                return true;
            }

            return false;
        }

        // ==========================================
        // TIER 6: Eldlich Engine Spells & Boss Hand/GY
        // ==========================================

        private bool BlackAwakeningActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (_blackAwakeningUsed) return false;

            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (Bot.GetMonsterCount() >= 5) return false;
                int target = !HasEldlichOnField() ? CardId.EldlichTheGoldenLord : CardId.DoomkingBalerdroch;
                AI.SelectCard(target);
                _blackAwakeningUsed = true;
                DecisionTracer.TraceActivate("BlackAwakeningActivate", $"SS {target} from Deck/Hand");
                return true;
            }

            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(new[] { CardId.GoldenLandForever, CardId.ConquistadorOfTheGoldenLand, CardId.HuaqueroOfTheGoldenLand });
                _blackAwakeningUsed = true;
                return true;
            }

            return false;
        }

        private bool WhiteDestinyActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (_whiteDestinyUsed) return false;

            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (Bot.GetMonsterCount() >= 5) return false;
                int target = !HasEldlichOnField() ? CardId.EldlichTheGoldenLord : CardId.DoomkingBalerdroch;
                AI.SelectCard(target);
                _whiteDestinyUsed = true;
                DecisionTracer.TraceActivate("WhiteDestinyActivate", $"SS {target} from Hand/GY");
                return true;
            }

            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(new[] { CardId.GoldenLandForever, CardId.ConquistadorOfTheGoldenLand, CardId.HuaqueroOfTheGoldenLand });
                _whiteDestinyUsed = true;
                return true;
            }

            return false;
        }

        private bool EldlichGoldenLordActivate()
        {
            // Hand Effect: Send self + 1 S/T from hand -> Send 1 card on field to GY (Non-destruction removal)
            if (Card.Location == CardLocation.Hand)
            {
                if (_eldlichHandUsed) return false;

                var target = Util.GetProblematicEnemyCard()
                    ?? Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault()
                    ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());

                if (target != null && target.Controller == 1)
                {
                    // Cost priority: spent Eldlixirs/Golden Land spells > non-Zombie World S/T
                    var costCard = Bot.Hand.FirstOrDefault(c => c != null && (c.IsSpell() || c.IsTrap()) && c.Id != CardId.ZombieWorld);
                    if (costCard != null)
                    {
                        AI.SelectCard(costCard);
                        AI.SelectNextCard(target);
                        _eldlichHandUsed = true;
                        DecisionTracer.TraceActivate("EldlichGoldenLordActivate", $"Send {target.Name} to GY via Eldlich hand effect");
                        return true;
                    }
                }
            }

            // GY Effect: Send 1 S/T on field to GY -> Add to hand, SS 1 Zombie with +1000 ATK & destruction immunity (3500 ATK Boss!)
            if (Card.Location == CardLocation.Grave)
            {
                if (_eldlichGyUsed) return false;
                if (Bot.GetMonsterCount() >= 5) return false;

                // Priority cost on field: spent continuous traps or Eldlixirs
                var fieldCost = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && 
                    (c.IsCode(CardId.ConquistadorOfTheGoldenLand, CardId.HuaqueroOfTheGoldenLand, CardId.EldlixirOfScarletSanguine, CardId.EldlixirOfTheGloriousGoldenLand)))
                    ?? Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsCode(CardId.ZombieWorld));

                if (fieldCost != null)
                {
                    AI.SelectCard(fieldCost);
                    _eldlichGyUsed = true;
                    DecisionTracer.TraceActivate("EldlichGoldenLordActivate", "Recur Eldlich from GY with +1000 ATK & destruction immunity");
                    return true;
                }
            }

            return false;
        }

        // ==========================================
        // TIER 7: Extra Deck Summons
        // ==========================================

        private bool FallenAngelSpSummonCondition()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (!HasEldlichInFieldOrGy()) return false;

            // Tribute 1 LIGHT Zombie monster on field (Conquistador/Huaquero Level 5 Trap Monster or extra Eldlich)
            var lightZombie = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && 
                (c.IsCode(CardId.ConquistadorOfTheGoldenLand, CardId.HuaqueroOfTheGoldenLand) || 
                (c.HasRace(CardRace.Zombie) && c.HasAttribute(CardAttribute.Light) && !IsAceCard(c))));

            return lightZombie != null;
        }

        private bool FallenAngelActivate()
        {
            // When sent to GY -> SS 1 Eldlich monster from Deck/Extra/GY!
            if (Card.Location == CardLocation.Grave)
            {
                AI.SelectCard(CardId.EldlichTheGoldenLord);
                DecisionTracer.TraceActivate("FallenAngelActivate", "Float into Eldlich the Golden Lord from Deck/Extra/GY");
                return true;
            }
            return false;
        }

        private bool ChaosAngelSpSummonCondition()
        {
            if (IsSpecialSummonBlocked()) return false;

            // Level 5 Conquistador + Level 5 Huaquero = Level 10 (treats 1 LIGHT as Tuner)
            int lightZombiesLv5 = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 5 && 
                (c.IsCode(CardId.ConquistadorOfTheGoldenLand, CardId.HuaqueroOfTheGoldenLand)));

            if (lightZombiesLv5 >= 2) return true;

            // Uni-Zombie + Level 5 Trap Monster + 2 Level ups
            if (Bot.HasInMonstersZone(CardId.UniZombie) && lightZombiesLv5 >= 1) return true;

            return false;
        }

        private bool ChaosAngelActivate()
        {
            var target = Util.GetProblematicEnemyCard()
                ?? Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c)).OrderByDescending(c => c.Attack).FirstOrDefault()
                ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());

            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("ChaosAngelActivate", $"Banish card on summon: {target.Name}");
                return true;
            }
            return false;
        }

        private bool EldlichMadGoldenLordActivate()
        {
            // Quick Effect: Tribute 1 Zombie -> Take control of 1 face-up enemy monster
            var tribute = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Zombie) && !c.IsCode(CardId.EldlichTheMadGoldenLord));
            if (tribute == null) return false;

            var target = Util.GetProblematicEnemyMonster(0, true)
                ?? Enemy.MonsterZone.GetHighestAttackMonster(true);

            if (target != null && !IsTargetImmune(target))
            {
                AI.SelectCard(tribute);
                AI.SelectNextCard(target);
                DecisionTracer.TraceActivate("EldlichMadGoldenLordActivate", $"Take control of {target.Name} by tributing {tribute.Name}");
                return true;
            }

            return false;
        }

        private bool SPLittleKnightSpSummonCondition()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Only summon S:P if we have Fallen Angel or Trap monsters to link off
            int linkable = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && 
                (c.IsCode(CardId.FallenAngelOfTheGoldenLand, CardId.ConquistadorOfTheGoldenLand, CardId.HuaqueroOfTheGoldenLand)));
            return linkable >= 2;
        }

        private bool SPLittleKnightActivate()
        {
            var target = Util.GetProblematicEnemyCard() ?? Util.GetProblematicEnemyMonster(0, true);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool MudragonActivate()
        {
            AI.SelectAttribute(CardAttribute.Light);
            return true;
        }

        // ==========================================
        // Helper Methods: Selections & Reposition
        // ==========================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (hint == 502 || hint == 504 || hint == 511 || hint == 513)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            if (hint == 506) // Search from Deck
            {
                var selected = new List<ClientCard>();
                int[] targets = {
                    CardId.EldlichTheGoldenLord,
                    CardId.DoomkingBalerdroch,
                    CardId.NecroworldBanshee,
                    CardId.ZombieWorld,
                    CardId.EldlixirOfScarletSanguine,
                    CardId.GoldenLandForever,
                    CardId.ConquistadorOfTheGoldenLand,
                    CardId.HuaqueroOfTheGoldenLand
                };

                foreach (int id in targets)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == id && !selected.Contains(c));
                    if (match != null)
                    {
                        selected.Add(match);
                        if (selected.Count == max) return selected;
                    }
                }

                if (selected.Count < min)
                {
                    foreach (var c in cards)
                    {
                        if (c != null && !selected.Contains(c))
                        {
                            selected.Add(c);
                            if (selected.Count == min) break;
                        }
                    }
                }

                if (selected.Count >= min) return selected;
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.EldlichTheGoldenLord || cardId == CardId.ChaosAngel || cardId == CardId.LordOfTheHeavenlyPrison)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }
            if (cardId == CardId.DoomkingBalerdroch && Card != null && Card.Location == CardLocation.Grave)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;

            if (IsAceCard(Card))
            {
                if (Card.IsDefense() && Card.Attack > 0) return true;
                return false;
            }

            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty && !IsSafeToAttack(Card) && IsSafeToDefend(Card)) return true;
            }
            else
            {
                if (enemyEmpty || IsSafeToAttack(Card)) return true;
            }
            return false;
        }

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

        private bool IsSafeToDefend(ClientCard monster)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.Attack > monster.Defense) return false;
            }
            return true;
        }
    }

    [Deck("Expert_2026_Goldlord", "2026_Goldlord")]
    public class ExpertGoldlordExecutor : _2026_GoldlordExecutor
    {
        private string _duelId;
        public ExpertGoldlordExecutor(GameAI ai, Duel duel) : base(ai, duel)
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
