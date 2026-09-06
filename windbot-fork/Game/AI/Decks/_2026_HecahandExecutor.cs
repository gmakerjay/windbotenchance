using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // CARD AUDIT — 2026_Hecahand (Illusion Control & Board Steal Engine)
    // ============================================================
    // | Card Name           | Type       | OPT? | Effect Summary                                                   |
    // |---------------------|------------|------|------------------------------------------------------------------|
    // | Maxx "C"            | Monster    | Yes  | Draw when opponent SS                                            |
    // | Droll & Lock Bird   | Monster    | Yes  | Lock deck searches for rest of turn                              |
    // | Mulcharmy Purulia   | Monster    | Yes  | Draw when opponent Normal/SS from hand                           |
    // | Mulcharmy Fuwalos   | Monster    | Yes  | Draw when opponent SS from Deck/Extra                            |
    // | Nightmare Apprentice| Monster    | Yes  | Discard 1 to SS; On NS/SS search 1 Illusion monster              |
    // | Hecahands Ibtel     | Monster    | Yes  | Reveal & shuffle to SS Hecahands from Deck; GY: revive Hecahands |
    // | Hecahands Yadel     | Monster    | Yes  | Reveal & shuffle to add Hecahands S/T; GY: Set Hecahands S/T   |
    // | Hecahands Makibel   | Monster    | Yes  | Reveal in hand to Fusion summon; GY: recycle to hand on Fusion GY|
    // | Hecahands Gaigas    | Monster    | Yes  | Opponent adds card -> SS from hand; MP: excavate 3 & steal mon   |
    // | Hecahands Breus     | Monster    | Yes  | Opponent adds card -> SS from hand; MP: look opp hand & steal mon|
    // | Lava Golem          | Monster    | No   | Tribute 2 opponent monsters to their field (1000 burn / turn)    |
    // | Gameciel            | Monster    | No   | Tribute 1 opponent monster to their field                        |
    // | Monster Reborn      | Spell      | Yes  | Special Summon 1 monster from either GY                          |
    // | Change of Heart     | Spell      | Yes  | Target & take control of 1 opponent monster                      |
    // | Triple Tactics Talent| Spell     | Yes  | Draw 2 / Steal monster / Look & shuffle 1 hand card              |
    // | Triple Tactics Thrust| Spell     | Yes  | Search / Set Normal Spell/Trap from Deck                         |
    // | Bot Herder          | Spell      | Yes  | Target owned monster / facedown def -> 200 burn + steal ALL mons |
    // | Illusion Gate       | Spell      | Once | Pay half LP: destroy all opp mons + SS 1 mon from opp GY        |
    // | The Hidden Hecahand | Spell      | Yes  | Add Hecahands mon + optional send to Set S/T; GY: opp draw/drop  |
    // | Hecahands Tartaros  | QuickSpell | Yes  | Option 0: Bounce opp S/T; Option 1: Fusion summon from field/GY  |
    // | Hecahands Dandalos  | Fusion Lv7 | Yes  | 2900 ATK; Steal 1 opp monster; Heca Fusions & stolen attack direct!|
    // | Hecahands Jauzah    | Fusion Lv8 | Yes  | 2400 ATK; Contact SS (1 stolen + 1 Illusion); Add Hecahands card |
    // | Hecahands Xeno      | Fusion Lv9 | Yes  | 3400 ATK; Quick SS mon from opp Extra Deck; If destroyed steal all|
    // | Red-Eyes Flare Metal| Xyz Rank 7 | Yes  | 2800 ATK; Indestructible by effects; 500 burn per opp card/effect|
    // ============================================================
    // ACE CARDS: Primary: Hecahands Dandalos / Hecahands Xeno / Red-Eyes Flare Metal Dragon / Hecahands Jauzah
    // PRIORITY: Win > Direct Attack OTK > Board Steal > Fusion Setup > Lockdown
    // ============================================================

    [Deck("2026_Hecahand", "2026_Hecahand")]
    public class _2026_HecahandExecutor : ModernExecutor
    {
        public class CardId
        {
            public const int DrollAndLockBird = 94145021;
            public const int MaxxC = 23434538;
            public const int MulcharmyPurulia = 84192580;
            public const int MulcharmyFuwalos = 42141493;
            public const int NightmareApprentice = 58143852;
            public const int HecahandsIbtel = 95365081;
            public const int HecahandsYadel = 32759190;
            public const int HecahandsMakibel = 18321034;
            public const int HecahandsGaigas = 95132593;
            public const int HecahandsBreus = 21637502;
            public const int LavaGolem = 102380;
            public const int Gameciel = 55063751;
            public const int MonsterReborn = 83764719;
            public const int ChangeOfHeart = 4031928;
            public const int TripleTacticsTalent = 25311006;
            public const int TripleTacticsThrust = 35269904;
            public const int BotHerder = 45951104;
            public const int IllusionGate = 33017964;
            public const int TheHiddenHecahands = 20415050;
            public const int HecahandsTartaros = 57809669;

            public const int HecahandsDandalos = 31411835;
            public const int HecahandsJauzah = 67021206;
            public const int HecahandsXeno = 94410955;
            public const int RedEyesFlareMetalDragon = 44405066;

            public const int AshBlossom = 14558127;
            public const int CalledByTheGrave = 24224830;
            public const int InfiniteImpermanence = 10045474;
        }

        private static readonly int[] AceCardIds = {
            CardId.HecahandsDandalos,
            CardId.HecahandsXeno,
            CardId.HecahandsJauzah,
            CardId.RedEyesFlareMetalDragon
        };

        private bool _hiddenHecahandsUsed;
        private bool _hiddenHecahandsGyUsed;
        private bool _gateUsed;
        private bool _tartarosFusionUsed;
        private bool _tartarosBounceUsed;
        private bool _gaigasHandUsed;
        private bool _breusHandUsed;
        private bool _gaigasZoneUsed;
        private bool _breusZoneUsed;
        private bool _ibtelHandUsed;
        private bool _ibtelGyUsed;
        private bool _yadelHandUsed;
        private bool _yadelGyUsed;
        private bool _makibelHandUsed;
        private bool _makibelGyUsed;
        private bool _jauzahSearchUsed;
        private bool _dandalosStealUsed;
        private bool _xenoExtraUsed;
        private bool _tttUsed;
        private bool _thrustUsed;

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            // Highest priority to use as material: stolen opponent monster!
            if (c.Location == CardLocation.MonsterZone && c.Owner == 1) return 10;
            if (c.IsCode(CardId.HecahandsIbtel, CardId.HecahandsYadel))
            {
                if (c.Location == CardLocation.MonsterZone || c.Location == CardLocation.Hand)
                    return 50;
                return 150;
            }
            if (c.IsCode(CardId.NightmareApprentice)) return 80;
            if (c.IsCode(CardId.HecahandsMakibel)) return 90;
            if (c.IsCode(CardId.HecahandsGaigas, CardId.HecahandsBreus)) return 120;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.AshBlossom, CardId.MaxxC, CardId.DrollAndLockBird)) return 800;
            return 200;
        }

        private bool IsHecahandCard(int id)
        {
            return id == CardId.HecahandsIbtel || id == CardId.HecahandsYadel || id == CardId.HecahandsMakibel ||
                   id == CardId.HecahandsGaigas || id == CardId.HecahandsBreus || id == CardId.HecahandsDandalos ||
                   id == CardId.HecahandsJauzah || id == CardId.HecahandsXeno || id == CardId.TheHiddenHecahands ||
                   id == CardId.HecahandsTartaros;
        }

        private bool IsIllusionMonster(ClientCard c)
        {
            if (c == null) return false;
            return c.IsCode(CardId.NightmareApprentice, CardId.HecahandsIbtel, CardId.HecahandsYadel,
                            CardId.HecahandsMakibel, CardId.HecahandsGaigas, CardId.HecahandsBreus,
                            CardId.HecahandsDandalos, CardId.HecahandsJauzah, CardId.HecahandsXeno);
        }

        public _2026_HecahandExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);

            // Combo Router: Authentic Hecahands sequencing
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Apprentice-Starter",
                RequiredCards = new List<int> { CardId.NightmareApprentice },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.NightmareApprentice, ActionType = ExecutorType.SpSummon, Description = "SS Nightmare Apprentice" },
                    new() { CardId = CardId.NightmareApprentice, ActionType = ExecutorType.Activate, Description = "Search Ibtel" },
                    new() { CardId = CardId.HecahandsIbtel, ActionType = ExecutorType.Activate, Description = "Reveal Ibtel -> SS Gaigas/Breus" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Ibtel-Starter",
                RequiredCards = new List<int> { CardId.HecahandsIbtel },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.HecahandsIbtel, ActionType = ExecutorType.Activate, Description = "Reveal Ibtel -> SS Gaigas/Breus" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Yadel-Hidden-Starter",
                RequiredCards = new List<int> { CardId.HecahandsYadel },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.HecahandsYadel, ActionType = ExecutorType.Activate, Description = "Reveal Yadel -> Search The Hidden Hecahands" },
                    new() { CardId = CardId.TheHiddenHecahands, ActionType = ExecutorType.Activate, Description = "Search Makibel & Set Tartaros" }
                },
                EndBoardScore = 85
            });

            // Bait Planner
            BaitPlanner.RegisterComboStarters(CardId.TheHiddenHecahands, CardId.NightmareApprentice, CardId.HecahandsIbtel, CardId.HecahandsYadel);
            BaitPlanner.RegisterBaitCards(CardId.TripleTacticsTalent, CardId.TripleTacticsThrust, CardId.ChangeOfHeart, CardId.MonsterReborn);

            // Chain Advisor
            ChainAdvisor.RegisterHighValueTargets(CardId.TheHiddenHecahands, CardId.HecahandsMakibel, CardId.HecahandsTartaros, CardId.HecahandsDandalos, CardId.HecahandsXeno);

            // ============================================================
            // TIER 1: Hand Traps (Opponent-Reactive)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, () => SmartHandTrapChain() && DefaultMaxxC());
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyPuruliaEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);

            // ============================================================
            // TIER 2: Quick Effects & Board Disruption (Opponent Turn / Fast Response)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsXeno, HecahandsXenoEffect);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsTartaros, HecahandsTartarosEffect);

            // ============================================================
            // TIER 3: Board-Clearing & Tributes (Kaiju & Lava Golem)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.LavaGolem, LavaGolemSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Gameciel, GamecielSummon);

            // ============================================================
            // TIER 4: Board Control, Steal & Breaker Spells
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.BotHerder, BotHerderEffect);
            AddExecutor(ExecutorType.Activate, CardId.ChangeOfHeart, ChangeOfHeartEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsThrust, TripleTacticsThrustEffect);
            AddExecutor(ExecutorType.Activate, CardId.IllusionGate, IllusionGateEffect);
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, MonsterRebornEffect);

            // ============================================================
            // TIER 5: Engine Searchers & Starters
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.TheHiddenHecahands, TheHiddenHecahandsEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.NightmareApprentice, NightmareApprenticeSummon);
            AddExecutor(ExecutorType.Activate, CardId.NightmareApprentice, NightmareApprenticeEffect);

            // ============================================================
            // TIER 6: Hecahands Hand / GY / Ignition Effects
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.HecahandsIbtel, HecahandsIbtelEffect);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsYadel, HecahandsYadelEffect);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsGaigas, HecahandsGaigasEffect);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsBreus, HecahandsBreusEffect);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsMakibel, HecahandsMakibelEffect);

            // ============================================================
            // TIER 7: Extra Deck Summons & Boss Ignition
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.HecahandsJauzah, HecahandsJauzahSummon);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsJauzah, HecahandsJauzahEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.HecahandsDandalos);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsDandalos, HecahandsDandalosEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.HecahandsXeno);
            AddExecutor(ExecutorType.SpSummon, CardId.RedEyesFlareMetalDragon, RedEyesFlareMetalDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.RedEyesFlareMetalDragon, RedEyesFlareMetalDragonEffect);

            // ============================================================
            // TIER 8: Trap Sets & Battle Positioning
            // ============================================================
            AddExecutor(ExecutorType.SpellSet, CardId.HecahandsTartaros, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);

            // ============================================================
            // TIER 9: Fallback Normal Summon
            // ============================================================
            AddExecutor(ExecutorType.Summon, FallbackNormalSummon);
        }

        public override bool OnSelectHand()
        {
            // Hecahand excels going second with Lava Golem / Bot Herder / Dandalos direct OTK
            return false;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _hiddenHecahandsUsed = false;
            _hiddenHecahandsGyUsed = false;
            _gateUsed = false;
            _tartarosFusionUsed = false;
            _tartarosBounceUsed = false;
            _gaigasHandUsed = false;
            _breusHandUsed = false;
            _gaigasZoneUsed = false;
            _breusZoneUsed = false;
            _ibtelHandUsed = false;
            _ibtelGyUsed = false;
            _yadelHandUsed = false;
            _yadelGyUsed = false;
            _makibelHandUsed = false;
            _makibelGyUsed = false;
            _jauzahSearchUsed = false;
            _dandalosStealUsed = false;
            _xenoExtraUsed = false;
            _tttUsed = false;
            _thrustUsed = false;
        }

        protected override bool IsBoardStrongEnough()
        {
            int disruptionCount = 0;
            if (Bot.HasInMonstersZone(CardId.HecahandsDandalos))
                disruptionCount += 2;
            if (Bot.HasInMonstersZone(CardId.HecahandsXeno))
                disruptionCount += 2;
            if (Bot.HasInMonstersZone(CardId.HecahandsJauzah))
                disruptionCount += 1;
            if (Bot.HasInMonstersZone(CardId.RedEyesFlareMetalDragon))
                disruptionCount += 2;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.HecahandsTartaros)))
                disruptionCount += 1;
            if (Bot.HasInHand(CardId.AshBlossom) || Bot.HasInHand(CardId.InfiniteImpermanence))
                disruptionCount += 1;
            if (Bot.HasInHand(CardId.HecahandsGaigas) || Bot.HasInHand(CardId.HecahandsBreus))
                disruptionCount += 1;

            int stolenCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Owner == 1);
            disruptionCount += stolenCount;

            int requiredDisruptions = 2;
            if (Enemy.Hand.Count >= 5) requiredDisruptions += 1;
            if (Bot.LifePoints <= 2000) requiredDisruptions += 1;

            return disruptionCount >= requiredDisruptions;
        }

        protected override bool ShouldStopExtending()
        {
            if (CanDealLethal()) return true;
            if (IsBoardStrongEnough() && Duel.Turn == 1) return true;
            return false;
        }

        private bool OpponentHasThreateningMonster()
        {
            int ourMaxAtk = Bot.GetMonsters()
                .Where(m => m != null && m.IsFaceup())
                .Select(m => m.Attack)
                .DefaultIfEmpty(0).Max();

            int threshold = ourMaxAtk > 0 ? ourMaxAtk : 2400;

            foreach (ClientCard c in Enemy.MonsterZone)
            {
                if (c == null || !c.IsFaceup() || c.IsDisabled()) continue;
                if (c.Attack >= threshold || c.IsFloodgate() || c.IsExtraCard())
                    return true;
            }
            return false;
        }

        private bool SetTrapCondition() => Util.IsTurn1OrMain2();

        private bool FallbackNormalSummon()
        {
            if (Duel.Turn == 1 && Duel.Player == 0) return false;
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsAttack())) return false;

            var summonable = Bot.Hand.Where(c => c != null && c.IsMonster() && c.Level <= 4).ToList();
            if (summonable.Count == 0) return false;

            if (!CanDealLethal())
            {
                summonable = summonable.Where(c =>
                    !c.IsCode(CardId.AshBlossom, CardId.MaxxC, CardId.DrollAndLockBird) &&
                    !c.IsCode(CardId.MulcharmyFuwalos, CardId.MulcharmyPurulia)
                ).ToList();
            }

            var bestMonster = summonable.OrderByDescending(c => c.Attack).FirstOrDefault();
            if (bestMonster != null)
            {
                AI.SelectCard(bestMonster);
                return true;
            }
            return false;
        }

        private bool MonsterRepos()
        {
            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster == null || IsAceCard(monster)) continue;
                bool enemyEmpty = Enemy.GetMonsterCount() == 0;
                if (monster.IsAttack())
                {
                    if (!enemyEmpty && monster.Attack < 1500)
                        return true;
                }
                else
                {
                    if (enemyEmpty || monster.Attack >= 1800)
                        return true;
                }
            }
            return false;
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            if (attackers == null || attackers.Count == 0) return null;
            // Attack with highest ATK first for maximum immediate damage push
            return attackers.OrderByDescending(a => a.Attack).FirstOrDefault();
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (attacker == null) return null;

            bool hasDandalos = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.HecahandsDandalos));
            bool canAttackDirect = hasDandalos && (attacker.IsCode(CardId.HecahandsDandalos, CardId.HecahandsJauzah, CardId.HecahandsXeno) || attacker.Owner == 1);

            if (canAttackDirect)
            {
                // Direct attack skips through all enemy monsters and strikes player directly!
                return AI.Attack(attacker, null);
            }

            return base.OnSelectAttackTarget(attacker, defenders);
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            // Dandalos allows all Hecahands Fusion monsters and stolen monsters to attack directly!
            bool hasDandalos = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.HecahandsDandalos));
            if (hasDandalos && (attacker.IsCode(CardId.HecahandsDandalos, CardId.HecahandsJauzah, CardId.HecahandsXeno) || attacker.Owner == 1))
            {
                return true;
            }

            // Illusion monster battle protection: neither monster is destroyed by battle
            if (IsIllusionMonster(attacker) && defender != null)
            {
                if (defender.IsFaceup() && !defender.IsDisabled() && defender.IsAttack() && defender.Attack > attacker.Attack)
                    return false;
            }
            return base.OnPreBattleBetween(attacker, defender);
        }

        // ============================================================
        // HAND TRAPS
        // ============================================================
        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool MulcharmyPuruliaEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return DefaultCalledByTheGrave();
        }

        private ClientCard GetPreemptiveImpermTarget()
        {
            return Enemy.MonsterZone.GetMonsters().FirstOrDefault(c =>
                c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() ||
                 AntiFloodgateHelper.NegateMonsterIds.Contains(c.Id) ||
                 (c.IsExtraCard() && c.Attack >= 2000)) &&
                !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
        }

        private bool InfiniteImpermanenceEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = GetPreemptiveImpermTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return DefaultInfiniteImpermanence();
        }

        // ============================================================
        // TRIBUTES & BOARD CLEARERS
        // ============================================================
        private bool LavaGolemSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Tribute 2 opponent monsters (clears negates without activating effects!)
            if (Enemy.GetMonsterCount() < 2) return false;

            // Always summon if enemy has 2+ monsters on our turn
            return true;
        }

        private bool GamecielSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            // Priority: problematic enemy monster
            var target = Util.GetProblematicEnemyMonster(0, canBeTarget: false);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }

            if (OpponentHasThreateningMonster() || Bot.HasInHand(CardId.BotHerder))
            {
                var highestAtk = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault();
                if (highestAtk != null)
                {
                    AI.SelectCard(highestAtk);
                    return true;
                }
            }
            return false;
        }

        // ============================================================
        // CONTROL TAKEOVER SPELLS
        // ============================================================
        private bool BotHerderEffect()
        {
            // Target 1 monster opponent controls that we OWN (Lava Golem/Gameciel) OR face-down defense monster
            var ownedTarget = Enemy.GetMonsters()
                .Where(c => c != null && c.Owner == 0 && (c.IsFaceup() || (c.IsFacedown() && c.IsDefense())) && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                .FirstOrDefault();

            if (ownedTarget != null)
            {
                AI.SelectCard(ownedTarget);
                return true;
            }

            var facedownTarget = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFacedown() && c.IsDefense() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                .FirstOrDefault();

            if (facedownTarget != null && Enemy.GetMonsterCount() >= 2)
            {
                AI.SelectCard(facedownTarget);
                return true;
            }
            return false;
        }

        private bool ChangeOfHeartEffect()
        {
            var target = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                .OrderByDescending(c => {
                    bool isProblematic = c.IsFloodgate() || c.IsExtraCard() || c.Attack >= 2500;
                    return isProblematic ? c.Attack + 10000 : c.Attack;
                })
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (_tttUsed) return false;
            // Option 0: Draw 2
            // Option 1: Take control of 1 opponent monster
            // Option 2: Look at opponent hand and shuffle 1
            if (Enemy.GetMonsterCount() > 0 && (OpponentHasThreateningMonster() || !Bot.HasInMonstersZone(CardId.HecahandsJauzah)))
            {
                AI.SelectOption(1); // Steal monster (can be used for Jauzah contact summon or direct attack!)
            }
            else if (Enemy.Hand.Count >= 3)
            {
                AI.SelectOption(2); // Hand rip
            }
            else
            {
                AI.SelectOption(0); // Draw 2
            }
            _tttUsed = true;
            return true;
        }

        private bool TripleTacticsThrustEffect()
        {
            if (_thrustUsed) return false;
            // Search priority: Bot Herder (if enemy has owned monster or we have Kaiju) > Change of Heart > The Hidden Hecahands > Illusion Gate
            bool enemyHasOwned = Enemy.GetMonsters().Any(c => c != null && c.Owner == 0);
            bool hasKaijuInHand = Bot.HasInHand(CardId.LavaGolem) || Bot.HasInHand(CardId.Gameciel);

            if (enemyHasOwned || hasKaijuInHand)
            {
                AI.SelectCard(CardId.BotHerder, CardId.ChangeOfHeart, CardId.TheHiddenHecahands, CardId.IllusionGate);
            }
            else
            {
                AI.SelectCard(CardId.ChangeOfHeart, CardId.TheHiddenHecahands, CardId.IllusionGate, CardId.BotHerder);
            }
            _thrustUsed = true;
            return true;
        }

        private bool IllusionGateEffect()
        {
            if (_gateUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (!CanDealLethal() && (Bot.LifePoints / 2) < 1500) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            if (Enemy.GetMonsterCount() >= 2 || OpponentHasThreateningMonster())
            {
                _gateUsed = true;
                return true;
            }
            return false;
        }

        private bool MonsterRebornEffect()
        {
            // Priority: Hecahands Fusion boss > Stolen boss > Highest ATK monster
            var target = Bot.Graveyard
                .Where(c => c != null && c.IsCanRevive() && IsAceCard(c))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (target == null)
            {
                target = Enemy.Graveyard
                    .Where(c => c != null && c.IsCanRevive() && (c.IsExtraCard() || c.Attack >= 2500))
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
            }

            if (target == null)
            {
                target = Bot.Graveyard
                    .Where(c => c != null && c.IsCanRevive() && IsHecahandCard(c.Id))
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
            }

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // ============================================================
        // ENGINE SEARCHERS & STARTERS
        // ============================================================
        private bool TheHiddenHecahandsEffect()
        {
            // GY Ignition Effect: Banish from GY -> opponent draws 1, discards 1
            if (Card.Location == CardLocation.Grave)
            {
                if (_hiddenHecahandsGyUsed) return false;
                bool hasGaigasOrBreusInHand = Bot.Hand.Any(c => c != null && (c.Id == CardId.HecahandsGaigas || c.Id == CardId.HecahandsBreus));
                if (hasGaigasOrBreusInHand || Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
                {
                    _hiddenHecahandsGyUsed = true;
                    return true;
                }
                return false;
            }

            // Hand / Field Activation Effect: Add 1 Hecahands monster + optional send card to Set S/T
            if (_hiddenHecahandsUsed) return false;
            if (ShouldSkipCombo()) return false;

            int searchId;
            bool hasIllusionMats = Bot.Hand.Concat(Bot.MonsterZone).Any(c => c != null && (IsHecahandCard(c.Id) || c.Id == CardId.NightmareApprentice));
            bool hasMakibel = Bot.HasInHand(CardId.HecahandsMakibel);
            bool hasIbtel = Bot.HasInHand(CardId.HecahandsIbtel);
            bool hasYadel = Bot.HasInHand(CardId.HecahandsYadel);

            if (hasIllusionMats && !hasMakibel && Bot.GetRemainingCount(CardId.HecahandsMakibel, 1) > 0)
            {
                searchId = CardId.HecahandsMakibel;
            }
            else if (!hasIbtel && Bot.GetRemainingCount(CardId.HecahandsIbtel, 3) > 0)
            {
                searchId = CardId.HecahandsIbtel;
            }
            else if (!hasYadel && Bot.GetRemainingCount(CardId.HecahandsYadel, 2) > 0)
            {
                searchId = CardId.HecahandsYadel;
            }
            else if (!hasMakibel && Bot.GetRemainingCount(CardId.HecahandsMakibel, 1) > 0)
            {
                searchId = CardId.HecahandsMakibel;
            }
            else if (Bot.GetRemainingCount(CardId.HecahandsGaigas, 1) > 0)
            {
                searchId = CardId.HecahandsGaigas;
            }
            else
            {
                searchId = CardId.HecahandsBreus;
            }

            AI.SelectCard(searchId);
            _hiddenHecahandsUsed = true;
            return true;
        }

        private bool NightmareApprenticeSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;
            return Bot.Hand.Count > 1;
        }

        private bool NightmareApprenticeEffect()
        {
            if (ShouldSkipCombo()) return false;
            // Search priority: Ibtel > Makibel > Yadel > Gaigas > Breus
            int searchId;
            if (!Bot.HasInHand(CardId.HecahandsIbtel) && Bot.GetRemainingCount(CardId.HecahandsIbtel, 3) > 0)
                searchId = CardId.HecahandsIbtel;
            else if (!Bot.HasInHand(CardId.HecahandsMakibel) && Bot.GetRemainingCount(CardId.HecahandsMakibel, 1) > 0)
                searchId = CardId.HecahandsMakibel;
            else if (!Bot.HasInHand(CardId.HecahandsYadel) && Bot.GetRemainingCount(CardId.HecahandsYadel, 2) > 0)
                searchId = CardId.HecahandsYadel;
            else
                searchId = CardId.HecahandsGaigas;

            AI.SelectCard(searchId);
            return true;
        }

        // ============================================================
        // HECAHANDS MONSTER EFFECTS
        // ============================================================
        private bool HecahandsIbtelEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_ibtelHandUsed) return false;
                if (ShouldSkipCombo()) return false;
                if (IsSpecialSummonBlocked()) return false;

                // Reveal & shuffle to SS Hecahands from Deck in DEF
                // ALWAYS prefer Gaigas (excavates 3 & steals) > Breus (looks at hand & steals) > Yadel
                if (Bot.GetRemainingCount(CardId.HecahandsGaigas, 1) > 0)
                    AI.SelectCard(CardId.HecahandsGaigas, CardId.HecahandsBreus, CardId.HecahandsYadel);
                else if (Bot.GetRemainingCount(CardId.HecahandsBreus, 1) > 0)
                    AI.SelectCard(CardId.HecahandsBreus, CardId.HecahandsGaigas, CardId.HecahandsYadel);
                else
                    AI.SelectCard(CardId.HecahandsYadel, CardId.HecahandsGaigas);

                _ibtelHandUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_ibtelGyUsed) return false;
                // Target 1 Hecahands in GY except Ibtel -> Special Summon
                var target = Bot.Graveyard
                    .Where(c => c != null && c.IsCanRevive() && c.Id != CardId.HecahandsIbtel && IsHecahandCard(c.Id))
                    .OrderByDescending(c => IsAceCard(c) ? c.Attack + 10000 : c.Attack)
                    .FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    _ibtelGyUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool HecahandsYadelEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_yadelHandUsed) return false;
                if (ShouldSkipCombo()) return false;

                // Reveal & shuffle to add Hecahands S/T from Deck
                bool hasHiddenHeca = Bot.HasInHand(CardId.TheHiddenHecahands);
                bool hasTartaros = Bot.HasInHand(CardId.HecahandsTartaros) || Bot.HasInSpellZone(CardId.HecahandsTartaros);

                if (!hasHiddenHeca && Bot.GetRemainingCount(CardId.TheHiddenHecahands, 3) > 0)
                {
                    AI.SelectCard(CardId.TheHiddenHecahands, CardId.HecahandsTartaros);
                }
                else
                {
                    AI.SelectCard(CardId.HecahandsTartaros, CardId.TheHiddenHecahands);
                }

                _yadelHandUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_yadelGyUsed) return false;
                // Target 1 Hecahands S/T in GY -> Set it
                var target = Bot.Graveyard
                    .Where(c => c != null && (c.Id == CardId.HecahandsTartaros || c.Id == CardId.TheHiddenHecahands))
                    .OrderByDescending(c => c.Id == CardId.HecahandsTartaros ? 1 : 0)
                    .FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    _yadelGyUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool HecahandsGaigasEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_gaigasHandUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (Bot.GetMonsterCount() >= 5) return false;

                // Hand trigger: Opponent added card to hand -> SS free 2800 DEF body!
                _gaigasHandUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                if (_gaigasZoneUsed) return false;
                if (Duel.Player != 0) return false;
                if (Bot.GetMonsterCount() >= 5) return false;

                // Main Phase: Excavate top 3 of opponent deck and SS 1 monster to our field!
                _gaigasZoneUsed = true;
                return true;
            }
            return false;
        }

        private bool HecahandsBreusEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_breusHandUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (Bot.GetMonsterCount() >= 5) return false;

                // Hand trigger: Opponent added card to hand -> SS free 2800 ATK body!
                _breusHandUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                if (_breusZoneUsed) return false;
                if (Duel.Player != 0) return false;
                if (Bot.GetMonsterCount() >= 5) return false;

                // Main Phase: Look at 1 random card in opponent hand, if monster SS to our field!
                _breusZoneUsed = true;
                return true;
            }
            return false;
        }

        private bool HecahandsMakibelEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_makibelHandUsed) return false;
                if (ShouldSkipCombo()) return false;
                if (IsSpecialSummonBlocked()) return false;

                // Count available materials in Hand/Field (excluding Makibel itself)
                int extraMaterials = Bot.Hand.Concat(Bot.MonsterZone)
                    .Count(c => c != null && c != Card && (IsHecahandCard(c.Id) || c.Id == CardId.NightmareApprentice || (c.Location == CardLocation.MonsterZone && c.Owner == 1)));

                if (extraMaterials < 1) return false;

                // Select Fusion Target:
                // #1: Dandalos (2900 ATK, steals 1 monster, ALL fusions/stolen attack directly!)
                // #2: Xeno (if 3 materials & already have Dandalos)
                // #3: Jauzah
                if (!Bot.HasInMonstersZone(CardId.HecahandsDandalos))
                {
                    AI.SelectCard(CardId.HecahandsDandalos, CardId.HecahandsXeno, CardId.HecahandsJauzah);
                }
                else if (extraMaterials >= 2 && !Bot.HasInMonstersZone(CardId.HecahandsXeno))
                {
                    AI.SelectCard(CardId.HecahandsXeno, CardId.HecahandsDandalos, CardId.HecahandsJauzah);
                }
                else
                {
                    AI.SelectCard(CardId.HecahandsJauzah, CardId.HecahandsDandalos, CardId.HecahandsXeno);
                }

                _makibelHandUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (_makibelGyUsed) return false;
                _makibelGyUsed = true;
                return true;
            }
            return false;
        }

        // ============================================================
        // TARTAROS QUICK-PLAY SPELL
        // ============================================================
        private bool HecahandsTartarosEffect()
        {
            if (IsSpecialSummonBlocked()) return false;

            bool controlsHeca = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsHecahandCard(c.Id));

            // On Opponent's Turn:
            if (Duel.Player == 1)
            {
                if (controlsHeca && !_tartarosBounceUsed)
                {
                    var stTarget = Enemy.GetSpells()
                        .Where(c => c != null && c.IsFaceup() && (c.IsFloodgate() || c.Type == (int)CardType.Field || c.Type == (int)CardType.Continuous))
                        .FirstOrDefault();

                    if (stTarget != null)
                    {
                        AI.SelectOption(0);
                        AI.SelectCard(stTarget);
                        _tartarosBounceUsed = true;
                        return true;
                    }
                }

                if (!_tartarosFusionUsed)
                {
                    var mats = Bot.GetMonsters().Concat(Bot.Graveyard)
                        .Where(c => c != null && (IsHecahandCard(c.Id) || (c.Location == CardLocation.MonsterZone && c.Owner == 1)))
                        .ToList();

                    if (mats.Count >= 2)
                    {
                        AI.SelectOption(1);
                        if (!Bot.HasInMonstersZone(CardId.HecahandsDandalos))
                            AI.SelectCard(CardId.HecahandsDandalos, CardId.HecahandsXeno, CardId.HecahandsJauzah);
                        else if (mats.Count >= 3)
                            AI.SelectCard(CardId.HecahandsXeno, CardId.HecahandsDandalos, CardId.HecahandsJauzah);
                        else
                            AI.SelectCard(CardId.HecahandsDandalos, CardId.HecahandsJauzah, CardId.HecahandsXeno);

                        _tartarosFusionUsed = true;
                        return true;
                    }
                }
                return false;
            }

            // On Our Turn:
            if (Duel.Player == 0)
            {
                if (!_tartarosFusionUsed && !ShouldSkipCombo())
                {
                    var mats = Bot.GetMonsters().Concat(Bot.Graveyard)
                        .Where(c => c != null && (IsHecahandCard(c.Id) || (c.Location == CardLocation.MonsterZone && c.Owner == 1)))
                        .ToList();

                    if (mats.Count >= 2)
                    {
                        AI.SelectOption(1);
                        if (!Bot.HasInMonstersZone(CardId.HecahandsDandalos))
                            AI.SelectCard(CardId.HecahandsDandalos, CardId.HecahandsXeno, CardId.HecahandsJauzah);
                        else if (mats.Count >= 3 && !Bot.HasInMonstersZone(CardId.HecahandsXeno))
                            AI.SelectCard(CardId.HecahandsXeno, CardId.HecahandsDandalos, CardId.HecahandsJauzah);
                        else
                            AI.SelectCard(CardId.HecahandsDandalos, CardId.HecahandsJauzah, CardId.HecahandsXeno);

                        _tartarosFusionUsed = true;
                        return true;
                    }
                }

                if (controlsHeca && !_tartarosBounceUsed)
                {
                    var stTarget = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
                    if (stTarget != null)
                    {
                        AI.SelectOption(0);
                        AI.SelectCard(stTarget);
                        _tartarosBounceUsed = true;
                        return true;
                    }
                }
            }
            return false;
        }

        // ============================================================
        // EXTRA DECK SUMMONS & BOSS EFFECTS
        // ============================================================
        private bool HecahandsJauzahSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Contact Summon: Tribute 1 face-up monster owned by opponent + 1 Illusion monster
            bool hasOpponentMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Owner == 1);
            bool hasIllusionMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsIllusionMonster(c) && !IsAceCard(c));

            return hasOpponentMonster && hasIllusionMonster;
        }

        private bool HecahandsJauzahEffect()
        {
            if (_jauzahSearchUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Main Phase: Add 1 Hecahands card from Deck or GY to hand
                bool hasTartaros = Bot.HasInHand(CardId.HecahandsTartaros) || Bot.HasInSpellZone(CardId.HecahandsTartaros);
                bool hasHiddenHeca = Bot.HasInHand(CardId.TheHiddenHecahands);
                bool hasMakibel = Bot.HasInHand(CardId.HecahandsMakibel);

                if (!hasMakibel)
                    AI.SelectCard(CardId.HecahandsMakibel, CardId.HecahandsTartaros, CardId.TheHiddenHecahands, CardId.HecahandsIbtel);
                else if (!hasTartaros)
                    AI.SelectCard(CardId.HecahandsTartaros, CardId.TheHiddenHecahands, CardId.HecahandsIbtel, CardId.HecahandsMakibel);
                else if (!hasHiddenHeca)
                    AI.SelectCard(CardId.TheHiddenHecahands, CardId.HecahandsIbtel, CardId.HecahandsMakibel, CardId.HecahandsTartaros);
                else
                    AI.SelectCard(CardId.HecahandsIbtel, CardId.HecahandsMakibel, CardId.HecahandsTartaros);

                _jauzahSearchUsed = true;
                return true;
            }
            return false;
        }

        private bool HecahandsDandalosEffect()
        {
            if (_dandalosStealUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.IsFloodgate() || c.IsExtraCard() ? c.Attack + 10000 : c.Attack)
                    .FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    _dandalosStealUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool HecahandsXenoEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_xenoExtraUsed) return false;
                if (Bot.GetMonsterCount() < 5)
                {
                    _xenoExtraUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool RedEyesFlareMetalDragonSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;

            var lvl7s = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.Level == 7).ToList();
            if (lvl7s.Count < 2) return false;

            // In Main Phase 1, only make Red-Eyes if we have Gaigas + Breus (or if Dandalos already stole and it's Turn 1)
            if (Bot.HasInMonstersZone(CardId.HecahandsDandalos) && Duel.Phase == DuelPhase.Main1 && Duel.Turn > 1)
                return false;

            return true;
        }

        private bool RedEyesFlareMetalDragonEffect()
        {
            return false;
        }

        // ============================================================
        // ON SELECT CARD — PRECISE HINT HANDLING & MATERIAL SELECTION
        // ============================================================
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Jauzah Contact Summon: 1 Stolen Opponent Monster (Owner == 1) + 1 Illusion Monster
            if (Card != null && Card.Id == CardId.HecahandsJauzah && hint == 500)
            {
                var stolen = cards.FirstOrDefault(c => c != null && c.Location == CardLocation.MonsterZone && c.Owner == 1);
                var illusion = cards.FirstOrDefault(c => c != null && c != stolen && c.Location == CardLocation.MonsterZone && IsIllusionMonster(c) && !IsAceCard(c));
                if (stolen != null && illusion != null)
                {
                    return new List<ClientCard> { stolen, illusion };
                }
            }

            // The Hidden Hecahands Specific Selection
            if (Card != null && Card.Id == CardId.TheHiddenHecahands)
            {
                if (hint == 506) // Search from Deck
                {
                    bool hasIllusionMats = Bot.Hand.Concat(Bot.MonsterZone).Any(c => c != null && (IsHecahandCard(c.Id) || c.Id == CardId.NightmareApprentice));
                    bool hasMakibel = Bot.HasInHand(CardId.HecahandsMakibel);
                    bool hasIbtel = Bot.HasInHand(CardId.HecahandsIbtel);
                    bool hasYadel = Bot.HasInHand(CardId.HecahandsYadel);

                    int targetId;
                    if (hasIllusionMats && !hasMakibel && Bot.GetRemainingCount(CardId.HecahandsMakibel, 1) > 0)
                        targetId = CardId.HecahandsMakibel;
                    else if (!hasIbtel && Bot.GetRemainingCount(CardId.HecahandsIbtel, 3) > 0)
                        targetId = CardId.HecahandsIbtel;
                    else if (!hasYadel && Bot.GetRemainingCount(CardId.HecahandsYadel, 2) > 0)
                        targetId = CardId.HecahandsYadel;
                    else if (!hasMakibel && Bot.GetRemainingCount(CardId.HecahandsMakibel, 1) > 0)
                        targetId = CardId.HecahandsMakibel;
                    else if (Bot.GetRemainingCount(CardId.HecahandsGaigas, 1) > 0)
                        targetId = CardId.HecahandsGaigas;
                    else
                        targetId = CardId.HecahandsBreus;

                    var searchCard = cards.FirstOrDefault(c => c != null && c.Id == targetId);
                    if (searchCard != null) return new List<ClientCard> { searchCard };
                }
                else if (hint == 501 || hint == 504 || hint == 500) // Send other card to GY
                {
                    var bestToSend = cards.Where(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone)
                        .OrderBy(c => {
                            if (c.Id == CardId.HecahandsIbtel) return 1;
                            if (c.Id == CardId.HecahandsYadel) return 2;
                            if (c.Id == CardId.NightmareApprentice) return 3;
                            if (c.Id == CardId.HecahandsMakibel) return 4;
                            if (c.Owner == 1 && !c.IsFloodgate() && !c.IsExtraCard() && c.Attack < 2000) return 5;
                            if (IsAceCard(c)) return 999;
                            if (c.Attack >= 2500 || c.IsFloodgate()) return 800;
                            return 50;
                        }).FirstOrDefault();

                    if (bestToSend != null && !IsAceCard(bestToSend) && bestToSend.Attack < 2500)
                    {
                        return new List<ClientCard> { bestToSend };
                    }
                    else if (cancelable)
                    {
                        return new List<ClientCard>();
                    }
                }
            }

            // Hecahands Ibtel Special Summon from Deck
            if (Card != null && Card.Id == CardId.HecahandsIbtel && hint == 509)
            {
                var summonTarget = cards.FirstOrDefault(c => c != null && c.Id == CardId.HecahandsGaigas)
                                ?? cards.FirstOrDefault(c => c != null && c.Id == CardId.HecahandsBreus)
                                ?? cards.FirstOrDefault(c => c != null && c.Id == CardId.HecahandsYadel);

                if (summonTarget != null) return new List<ClientCard> { summonTarget };
            }

            // Hecahands Ibtel GY Revive
            if (Card != null && Card.Id == CardId.HecahandsIbtel && (hint == 509 || hint == 500))
            {
                var reviveTarget = cards.Where(c => c != null && c.IsCanRevive() && c.Id != CardId.HecahandsIbtel && IsHecahandCard(c.Id))
                    .OrderByDescending(c => IsAceCard(c) ? c.Attack + 10000 : c.Attack)
                    .FirstOrDefault();

                if (reviveTarget != null) return new List<ClientCard> { reviveTarget };
            }

            // Extra Deck Fusion Monster selection: ALWAYS Dandalos first if not on field!
            if (hint == 509 || hint == 570)
            {
                if (!Bot.HasInMonstersZone(CardId.HecahandsDandalos))
                {
                    var dandalos = cards.FirstOrDefault(c => c != null && c.Id == CardId.HecahandsDandalos);
                    if (dandalos != null) return new List<ClientCard> { dandalos };
                }

                if (!Bot.HasInMonstersZone(CardId.HecahandsXeno))
                {
                    var xeno = cards.FirstOrDefault(c => c != null && c.Id == CardId.HecahandsXeno);
                    if (xeno != null) return new List<ClientCard> { xeno };
                }

                var jauzah = cards.FirstOrDefault(c => c != null && c.Id == CardId.HecahandsJauzah);
                if (jauzah != null) return new List<ClientCard> { jauzah };
            }

            // Fusion Material Selection (Makibel / Tartaros)
            if (hint == 511 || hint == 578 || hint == 514)
            {
                var sortedMaterials = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.Id == CardId.HecahandsMakibel) return 1;
                    if (c.Location == CardLocation.MonsterZone && c.Owner == 1) return 2; // Stolen monster
                    if (c.Id == CardId.HecahandsIbtel) return 3; // Triggers GY revive
                    if (c.Id == CardId.HecahandsYadel) return 4; // Triggers GY set S/T
                    if (c.Id == CardId.NightmareApprentice) return 5;
                    if (IsAceCard(c)) return 999;
                    return 50;
                }).ToList();

                if (sortedMaterials.Count >= min)
                    return sortedMaterials.Take(max).ToList();
            }

            // Bot Herder Target Selection
            if (Card != null && Card.Id == CardId.BotHerder)
            {
                var owned = cards.FirstOrDefault(c => c != null && c.Controller == 1 && c.Owner == 0);
                if (owned != null) return new List<ClientCard> { owned };

                var facedownDef = cards.FirstOrDefault(c => c != null && c.Controller == 1 && c.IsFacedown() && c.IsDefense());
                if (facedownDef != null) return new List<ClientCard> { facedownDef };
            }

            // Change of Heart Target Selection
            if (Card != null && Card.Id == CardId.ChangeOfHeart)
            {
                var steal = cards.Where(c => c != null && c.Controller == 1 && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                    .OrderByDescending(c => c.IsFloodgate() || c.IsExtraCard() ? c.Attack + 10000 : c.Attack)
                    .FirstOrDefault();

                if (steal != null) return new List<ClientCard> { steal };
            }

            // Hecahands Dandalos Steal Target Selection
            if (Card != null && Card.Id == CardId.HecahandsDandalos)
            {
                var steal = cards.Where(c => c != null && c.Controller == 1 && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.IsFloodgate() || c.IsExtraCard() ? c.Attack + 10000 : c.Attack)
                    .FirstOrDefault();

                if (steal != null) return new List<ClientCard> { steal };
            }

            // Nightmare Apprentice Cost & Search
            if (Card != null && Card.Id == CardId.NightmareApprentice)
            {
                if (hint == 501 || hint == 504)
                {
                    var discard = cards.OrderBy(c => {
                        if (c == null) return 999;
                        if (c.Id == CardId.HecahandsIbtel) return 1;
                        if (c.Id == CardId.HecahandsYadel) return 2;
                        if (c.Id == CardId.HecahandsMakibel) return 3;
                        if (c.IsCode(CardId.BotHerder) && Enemy.GetMonsterCount() == 0) return 4;
                        if (c.IsCode(CardId.LavaGolem) && Enemy.GetMonsterCount() < 2) return 5;
                        if (!IsAceCard(c) && !c.IsCode(CardId.MaxxC, CardId.AshBlossom)) return 10;
                        return 100;
                    }).FirstOrDefault();

                    if (discard != null) return new List<ClientCard> { discard };
                }
                else if (hint == 506)
                {
                    int searchId;
                    bool hasIbtel = Bot.HasInHand(CardId.HecahandsIbtel);
                    bool hasMakibel = Bot.HasInHand(CardId.HecahandsMakibel);
                    bool hasYadel = Bot.HasInHand(CardId.HecahandsYadel);

                    if (!hasIbtel && Bot.GetRemainingCount(CardId.HecahandsIbtel, 3) > 0)
                        searchId = CardId.HecahandsIbtel;
                    else if (!hasMakibel && Bot.GetRemainingCount(CardId.HecahandsMakibel, 1) > 0)
                        searchId = CardId.HecahandsMakibel;
                    else if (!hasYadel && Bot.GetRemainingCount(CardId.HecahandsYadel, 2) > 0)
                        searchId = CardId.HecahandsYadel;
                    else
                        searchId = CardId.HecahandsGaigas;

                    var target = cards.FirstOrDefault(c => c != null && c.Id == searchId);
                    if (target != null) return new List<ClientCard> { target };
                }
            }

            // Hint 501 / 504: Generic Cost / Discard / Send to GY
            if (hint == 501 || hint == 504)
            {
                var sortedDiscards = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.Id == CardId.HecahandsIbtel) return 1;
                    if (c.Id == CardId.HecahandsYadel) return 2;
                    if (c.Id == CardId.HecahandsMakibel) return 3;
                    if (c.Id == CardId.NightmareApprentice && c.Location == CardLocation.MonsterZone) return 4;
                    if (!IsAceCard(c) && !c.IsCode(CardId.MaxxC, CardId.AshBlossom)) return 10;
                    return 100;
                }).ToList();

                if (sortedDiscards.Count >= min)
                    return sortedDiscards.Take(max).ToList();
            }

            // Hint 506: Generic Search from Deck to Hand
            if (hint == 506)
            {
                var priorityOrder = new[] {
                    CardId.TheHiddenHecahands,
                    CardId.HecahandsMakibel,
                    CardId.HecahandsIbtel,
                    CardId.HecahandsYadel,
                    CardId.HecahandsTartaros,
                    CardId.HecahandsGaigas,
                    CardId.HecahandsBreus
                };

                var searchMatches = cards.OrderBy(c => {
                    int idx = Array.IndexOf(priorityOrder, c.Id);
                    return idx >= 0 ? idx : 999;
                }).ToList();

                if (searchMatches.Count >= min)
                    return searchMatches.Take(max).ToList();
            }

            // Hint 509: Generic Special Summon from Deck / GY / Extra
            if (hint == 509)
            {
                var priority = cards.Where(c => c != null)
                    .OrderByDescending(c => {
                        if (c.Id == CardId.HecahandsDandalos && !Bot.HasInMonstersZone(CardId.HecahandsDandalos)) return 200000;
                        if (IsAceCard(c)) return c.Attack + 10000;
                        if (IsHecahandCard(c.Id)) return c.Attack + 5000;
                        return c.Attack;
                    }).ToList();

                if (priority.Count >= min)
                    return priority.Take(max).ToList();
            }

            // Hint 502 / 533: Generic Release / Tribute / Destroy
            if (hint == 502 || hint == 533)
            {
                var safeTributes = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                if (safeTributes.Count >= min)
                    return safeTributes.Take(max).ToList();
            }

            // Hint 549: Battle Target Selection
            if (hint == 549)
            {
                bool hasDandalos = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.HecahandsDandalos));
                if (hasDandalos)
                {
                    var directOrWeak = cards.OrderBy(c => c.Attack).ToList();
                    if (directOrWeak.Count >= min) return directOrWeak.Take(max).ToList();
                }

                int ourBestAtk = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack())
                    .Select(c => c.Attack)
                    .DefaultIfEmpty(0).Max();

                var beatable = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone
                    && (c.IsAttack() ? c.Attack < ourBestAtk : c.Defense < ourBestAtk)).ToList();

                if (beatable.Count >= min)
                    return beatable.OrderByDescending(c => c.Attack).Take(max).ToList();
            }

            // Protect Ace Cards on our field from accidental selection
            if (cards.Any(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone && IsAceCard(c)))
            {
                var safeCards = cards.Where(c => c == null || c.Controller != 0 || c.Location != CardLocation.MonsterZone || !IsAceCard(c)).ToList();
                if (safeCards.Count >= min)
                    return safeCards.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }

    [Deck("Expert_2026_Hecahand", "2026_Hecahand")]
    public class ExpertHecahandExecutor : _2026_HecahandExecutor
    {
        public ExpertHecahandExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }
}
