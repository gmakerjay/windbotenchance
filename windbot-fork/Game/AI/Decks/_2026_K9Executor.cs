using System;
using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ==========================================
    // 2026_K9
    // ==========================================
    // ============================================================
    // CARD AUDIT โ€” 2026_K9
    // ============================================================
    // Card Name                  | Type       | OPT? | Effect Summary                              | Activate When                              | NEVER Activate When                          |
    // ---------------------------|------------|------|---------------------------------------------|--------------------------------------------|----------------------------------------------|
    // K9-66a Jokul               | Monster    | Yes  | Reveal + Lv5 K9 โ’ SS both; Field: search K9 | Hand: have Lv5 K9; Field: targets in Deck | No Lv5 K9 / no targets left                 |
    // K9-66b Lantern             | Monster    | Yes  | SS self + Lv5 non-Pyro from GY; Search K9 S/T| Hand: have Lv5 in GY; Field: search       | No GY target / no A Case in deck            |
    // K9-17 Izuna                | Monster    | Yes  | Hand: SS vs opp hand/GY effect; GY: send K9  | Opp activated monster in hand/GY           | No opp activation / no K9 targets            |
    // K9-00 Lupis                | Monster    | No   | Search K9 from Deck on field                  | Need K9 extender                           | No K9 targets left                          |
    // VS Razen                   | Monster    | Yes  | Search VS on summon; Quick reveal attrs       | Summoned / opp turn with attrs             | No targets / no attrs to reveal             |
    // VS Dr. Mad Love            | Monster    | Yes  | Search VS S/T; Quick reveal                   | Summoned / opp turn with attrs             | No S/T targets / no attrs                   |
    // VS Jiaolong                | Monster    | Yes  | Hand: trigger on VS activation; Field: search | VS activated / need search                 | Already used / no targets                   |
    // VS Heavy Borger            | Monster    | Yes  | Bounce VS โ’ SS self; Draw 1 (field)           | Have VS to bounce / DARK in hand           | No VS on field / no DARK                    |
    // VS Hollie Sue              | Monster    | Yes  | Reveal VS in hand โ’ SS self; Search on field  | Have another VS in hand                    | No other VS in hand                         |
    // VS Caesar Valius           | Monster    | Yes  | Bounce non-Dragon VS โ’ SS self; Destroy(field)| Have VS to bounce / EARTH in hand          | No VS on field / no EARTH                   |
    // A Case for K9              | Spell      | Yes  | Search K9 monster; Set from GY later           | Need K9 starter                             | Already face-up / used                      |
    // Stake Your Soul            | Spell      | Yes  | Reveal VS attributes to search pair            | Have attrs to reveal + pair in Deck        | No attrs / no pairs left                    |
    // VS Start                   | Spell      | Yes  | Shuffle VS dupe โ’ draw + add VS from Deck      | Have duplicate VS in hand                  | No duplicates / no targets                  |
    // Reinforcement of Army      | Spell      | No   | Search Warrior (Razen)                          | Need Razen                                  | Already have Razen in hand/field            |
    // Pot of Prosperity          | Spell      | Yes  | Banish 6 for draw 1; Banish 3 for draw 1       | Need cards, have โฅ6 in Extra               | Would lose key Extra monsters               |
    // ============================================================
    // ACE CARDS:
    //   Primary  : Vanquish Soul Caesar Valius โ€” destroy + protection
    //   Secondary: Vanquish Soul Heavy Borger โ€” draw + attribute reveal
    //   Tertiary : K9-17 Ripper โ€” search + negate
    //   Quatern  : Zeus โ€” board wipe after battle
    //   Link     : S:P Little Knight โ€” banish disruption
    // ============================================================
    // STRATEGY:
    //   Turn 1: Chokul โ’ Izuna โ’ K9 Xyz setup + VS attribute setup
    //   Turn 2: Break board with Ripper negate + Zeus board wipe + Caesar destruction
    //   Engine: K9 engine to link into VS; VS for attributes + disruption
    // ============================================================
    [Deck("2026_K9", "2026_K9")]
    public class _2026_K9Executor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck โ€” Vanquish Soul
            public const int VanquishSoulRazen = 29302858;
            public const int VanquishSoulJiaolong = 9091064;
            public const int VanquishSoulDrMadLove = 29280200;
            public const int VanquishSoulHeavyBorger = 92895501;
            public const int VanquishSoulHollieSue = 93156774;
            public const int VanquishSoulCaesarValius = 91073013;

            // Main Deck โ€” K9
            public const int K9_66aJokul = 28642461;
            public const int K9_66bLantern = 55031170;
            public const int K9_00Lupis = 91025875;
            public const int K9_17Izuna = 92248362;

            // Hand Traps & Disruptions
            public const int DrollLockBird = 94145021;
            public const int MulcharmyFuwalos = 42141493;
            public const int GhostBelle = 73642296;
            public const int AshBlossom = 14558127;
            public const int EffectVeiler = 97268402;
            public const int BystialMagnamhut = 33854624;
            public const int DimensionShifter = 91800273;

            // Spells
            public const int StakeYourSoul = 54562327;
            public const int VanquishSoulStart = 35550352;
            public const int ACaseForK9 = 80181649;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int PotOfProsperity = 84211599;
            public const int CalledByTheGrave = 24224830;
            public const int HarpiesFeatherDuster = 18144506;

            // Traps
            public const int VanquishSoulSnowDevil = 60883493;

            // Extra Deck
            public const int Garura = 11765832;
            public const int SeaMonsterOfTheseus = 96334243;
            public const int PanzerDragon = 72959823;
            public const int MudragonOfTheSwamp = 54757758;
            public const int Zeus = 90448279;
            public const int K9_17Ripper = 27420823;
            public const int Vallon = 40673853;
            public const int CXyzNaschKnight = 61374414;
            public const int NashKnight = 34876719;
            public const int NumberC104 = 49456901;
            public const int Number104 = 2061963;
            public const int SPLittleKnight = 29301450;
            public const int RockOfTheVanquisher = 28168628;

            // Side Deck
            public const int RetaliatingC = 46502744;
            public const int MulcharmyPurulia = 84192580;
            public const int SuperPolymerization = 48130397;
            public const int SolemnJudgment = 41420027;
            public const int SolemnAccusation = 78114463;
            public const int SolemnWarning = 84749824;
        }

        // --- GOING FIRST/SECOND TRACKING ---

        // Track once-per-turn activations
        private bool _stakeYourSoulUsed = false;
        private bool _aCaseForK9Used = false;
        private bool _vanquishSoulStartUsed = false;
        private bool _magnamhutUsed = false;
        private bool _lupisUsed = false;

        // โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• โ• 
        //  BOARD STATE EVALUATION โ€” inherits FieldGuard from ModernExecutor
        //  IsSpecialSummonBlocked, CanDealLethal, CanOTK, ShouldSkipCombo,
        //  NeedsBoardPresence, IsInGrindGame, EnemyHasKnownNegate โ’ inherited
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        protected override bool IsBoardStrongEnough()
        {
            // Deck-specific: K9/VS boss configuration checks
            if (Bot.HasInMonstersZone(CardId.VanquishSoulCaesarValius) && Bot.GetMonsterCount() >= 2)
                return true;
            if (Bot.HasInMonstersZone(CardId.VanquishSoulHeavyBorger) && Bot.GetSpellCount() >= 1)
                return true;
            if (Bot.HasInMonstersZone(CardId.K9_17Ripper) && Bot.GetMonsterCount() >= 2)
                return true;
            if (Bot.HasInMonstersZone(CardId.Zeus) && Bot.GetMonsterCount() >= 1)
                return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        private bool OpponentHasThreateningMonster()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 3000 || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
        }

        public _2026_K9Executor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);
            ResourcePlan.RegisterAceCards(AceCardIds);

            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Jokul-Izuna-Play",
                RequiredCards = new List<int> { CardId.K9_66aJokul, CardId.K9_17Izuna },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.K9_66aJokul, ActionType = ExecutorType.Activate, Description = "Activate Jokul from hand" },
                    new() { CardId = CardId.K9_66aJokul, ActionType = ExecutorType.Activate, Description = "Activate Jokul search" },
                    new() { CardId = CardId.K9_17Izuna, ActionType = ExecutorType.Activate, Description = "Activate Izuna GY dump" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "StakeYourSoul-Play",
                RequiredCards = new List<int> { CardId.StakeYourSoul },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.StakeYourSoul, ActionType = ExecutorType.Activate, Description = "Activate Stake Your Soul" },
                    new() { CardId = CardId.VanquishSoulRazen, ActionType = ExecutorType.Activate, Description = "Activate Razen search" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.StakeYourSoul, CardId.K9_66aJokul);
            BaitPlanner.RegisterBaitCards(CardId.ReinforcementOfTheArmy, CardId.PotOfProsperity, CardId.ACaseForK9);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.StakeYourSoul, CardId.ReinforcementOfTheArmy);

            // ===== Priority 1: Board Breakers (before committing monsters) =====
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, DefaultHarpiesFeatherDusterFirst);

            // ===== Priority 2: Search & Draw (before summoning) =====
            AddExecutor(ExecutorType.Activate, CardId.PotOfProsperity, PotOfProsperityEffect);
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, ReinforcementOfTheArmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.StakeYourSoul, StakeYourSoulEffect);
            AddExecutor(ExecutorType.Activate, CardId.VanquishSoulStart, VanquishSoulStartEffect);
            AddExecutor(ExecutorType.Activate, CardId.ACaseForK9, ACaseForK9Effect);

            // ===== Priority 3: Hand Traps (opponent's turn only) =====
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyPurulia, MulcharmyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, () => SmartHandTrapChain() && DefaultGhostBelleAndHauntedMansion());
            AddExecutor(ExecutorType.Activate, CardId.DrollLockBird, () => SmartHandTrapChain() && DefaultDontChainMyself());
            AddExecutor(ExecutorType.Activate, CardId.DimensionShifter, DimensionShifterEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);

            // ===== Priority 4: K9 Monster Effects (hand effects first) =====
            AddExecutor(ExecutorType.Activate, CardId.K9_17Izuna, K9_17IzunaHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_66aJokul, K9_66aJokulHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_66bLantern, K9_66bLanternHandEffect);

            // ===== Priority 5: VS Monster Effects =====
            AddExecutor(ExecutorType.Activate, CardId.VanquishSoulRazen, VanquishSoulRazenEffect);
            AddExecutor(ExecutorType.Activate, CardId.VanquishSoulDrMadLove, VanquishSoulDrMadLoveEffect);
            AddExecutor(ExecutorType.Activate, CardId.VanquishSoulJiaolong, VanquishSoulJiaolongEffect);
            AddExecutor(ExecutorType.Activate, CardId.VanquishSoulHeavyBorger, VanquishSoulHeavyBorgerEffect);
            AddExecutor(ExecutorType.Activate, CardId.VanquishSoulHollieSue, VanquishSoulHollieSueEffect);
            AddExecutor(ExecutorType.Activate, CardId.VanquishSoulCaesarValius, VanquishSoulCaesarValiusEffect);

            // ===== Priority 6: K9 Monster Field Effects & Summons =====
            AddExecutor(ExecutorType.Summon, CardId.K9_66aJokul, K9SummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.K9_17Izuna, K9SummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.K9_66bLantern, K9SummonCheck);

            // K9 field effects (search)
            AddExecutor(ExecutorType.Activate, CardId.K9_66aJokul, K9_66aJokulFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_66bLantern, K9_66bLanternFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_17Izuna, K9_17IzunaFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.K9_00Lupis, K9_00LupisEffect);

            // ===== Priority 7: VS Normal Summons =====
            AddExecutor(ExecutorType.Summon, CardId.VanquishSoulRazen);
            AddExecutor(ExecutorType.Summon, CardId.VanquishSoulDrMadLove);
            AddExecutor(ExecutorType.Summon, CardId.VanquishSoulJiaolong);
            AddExecutor(ExecutorType.Summon, CardId.VanquishSoulHollieSue);
            AddExecutor(ExecutorType.Summon, CardId.VanquishSoulCaesarValius);
            AddExecutor(ExecutorType.Summon, CardId.VanquishSoulHeavyBorger);

            // ===== Priority 8: Bystial Magnamhut =====
            AddExecutor(ExecutorType.SpSummon, CardId.BystialMagnamhut, BystialMagnamhutSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.BystialMagnamhut, BystialMagnamhutEffect);

            // ===== Priority 9: Extra Deck โ€” Link & Xyz (only in MP2 after battle, or if no battle possible) =====
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.RockOfTheVanquisher, RockOfTheVanquisherSummon);
            AddExecutor(ExecutorType.Activate, CardId.RockOfTheVanquisher, RockOfTheVanquisherEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.K9_17Ripper, K9_17RipperSummon);
            AddExecutor(ExecutorType.Activate, CardId.K9_17Ripper, K9_17RipperEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Vallon);
            AddExecutor(ExecutorType.Activate, CardId.Vallon, VallonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.NashKnight);
            AddExecutor(ExecutorType.Activate, CardId.NashKnight, NashKnightEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.CXyzNaschKnight);
            AddExecutor(ExecutorType.Activate, CardId.CXyzNaschKnight, CXyzNaschKnightEffect);
            // Zeus โ€” only after battle!
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, ZeusSummon);
            AddExecutor(ExecutorType.Activate, CardId.Zeus, ZeusEffect);

            // ===== Priority 10: Traps & Counter =====
            AddExecutor(ExecutorType.Activate, CardId.VanquishSoulSnowDevil, DefaultTrap);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, DefaultSolemnJudgment);
            AddExecutor(ExecutorType.Activate, CardId.SolemnWarning, DefaultSolemnWarning);
            AddExecutor(ExecutorType.Activate, CardId.SolemnAccusation, DefaultSolemnWarning);

            // ===== Bottom: Set spells/traps, reposition =====
            AddExecutor(ExecutorType.SpellSet, SpellSetFiltered);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ACE CARDS & MATERIAL PRIORITY
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private static readonly int[] AceCardIds = {
            CardId.VanquishSoulCaesarValius,
            CardId.VanquishSoulHeavyBorger,
            CardId.K9_17Ripper,
            CardId.Zeus,
            CardId.SPLittleKnight,
            CardId.RockOfTheVanquisher
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.K9_00Lupis)) return 50;
            if (c.IsCode(CardId.K9_66aJokul) || c.IsCode(CardId.K9_66bLantern)) return 100;
            if (c.IsCode(CardId.K9_17Izuna)) return 120;
            if (c.IsCode(CardId.VanquishSoulRazen)) return 300;
            if (c.IsCode(CardId.VanquishSoulDrMadLove)) return 280;
            return base.GetMaterialPriority(c);
        }

        public override bool OnSelectHand()
        {
            // K9/VS is a control deck โ€” prefer going first to set up attribute reveals + K9 Xyz
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _stakeYourSoulUsed = false;
            _aCaseForK9Used = false;
            _vanquishSoulStartUsed = false;
            _magnamhutUsed = false;
            _lupisUsed = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  OTK & BATTLE LOGIC
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        /// <summary>
        /// True if we should do extra deck plays now (MP1 with no battle possible, or MP2).
        /// False if we should wait for MP2 after attacking.
        /// </summary>
        private bool ShouldExtraDeckNow()
        {
            // Turn 1: always ok to combo
            if (Duel.Turn == 1) return true;
            // If we have attackers and it's MP1 โ€” wait for MP2
            if (Duel.Phase == DuelPhase.Main1 && Bot.HasAttackingMonster())
                return false;
            // MP2 or no attackers: ok to combo
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  SPELL EFFECTS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool ReinforcementOfTheArmyEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            // Don't search if we already have Razen in hand or field
            if (Bot.HasInMonstersZone(CardId.VanquishSoulRazen) || Bot.HasInHand(CardId.VanquishSoulRazen)) return false;
            AI.SelectCard(CardId.VanquishSoulRazen);
            return true;
        }

        private bool PotOfProsperityEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            // Don't activate if board is already strong and we don't need more cards
            if (IsBoardStrongEnough() && Bot.GetHandCount() >= 3 && !ShouldSkipCombo()) return false;
            // Only activate if we have enough Extra Deck fodder (keep at least key cards)
            int extraCount = Bot.ExtraDeck.Count;
            if (extraCount < 6) return false;
            return true;
        }

        private bool ACaseForK9Effect()
        {
            if (IsMain1SearchDeferred()) return false;
            // Don't activate if already face-up on field
            if (Bot.HasInSpellZone(Card.Id, faceUp: true)) return false;
            // Don't activate duplicate if already used this turn
            if (_aCaseForK9Used) return false;
            _aCaseForK9Used = true;

            AI.SelectCard(CardId.K9_66aJokul, CardId.K9_17Izuna, CardId.K9_66bLantern);
            return true;
        }

        private bool StakeYourSoulEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (_stakeYourSoulUsed) return false;

            // Bait check!
            var bait = GetBaitIfNeeded(Card);
            if (bait != null) return false;

            bool canFire = Bot.Hand.Any(h => h != null && h.IsMonster() && h.HasAttribute(CardAttribute.Fire) &&
                                            HasRemainingVSAttribute(CardAttribute.Fire, h.Id));
            bool canDark = Bot.Hand.Any(h => h != null && h.IsMonster() && h.HasAttribute(CardAttribute.Dark) &&
                                            HasRemainingVSAttribute(CardAttribute.Dark, h.Id));
            bool canEarth = Bot.Hand.Any(h => h != null && h.IsMonster() && h.HasAttribute(CardAttribute.Earth) &&
                                            HasRemainingVSAttribute(CardAttribute.Earth, h.Id));

            if (canFire || canDark || canEarth)
            {
                _stakeYourSoulUsed = true;
                return true;
            }
            return false;
        }

        private bool VanquishSoulStartEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (_vanquishSoulStartUsed) return false;
            bool canUse = Bot.Hand.Any(c => c != null && IsVanquishSoul(c) && GetRemainingCount(c.Id) > 1);
            if (canUse) _vanquishSoulStartUsed = true;
            return canUse;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  HAND TRAPS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool MulcharmyEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool DimensionShifterEffect()
        {
            return Bot.Graveyard.Count == 0;
        }

        private bool CalledByTheGraveEffect()
        {
            int[] targetList =
            {
                CardId.MulcharmyFuwalos, CardId.MulcharmyPurulia,
                CardId.AshBlossom, CardId.GhostBelle,
                CardId.DrollLockBird, CardId.DimensionShifter,
                23434538, // Maxx C
                59438930, // Ghost Ogre
                CardId.EffectVeiler, // Effect Veiler
                34267821  // Artifact Lancea
            };
            if (Duel.LastChainPlayer == 1)
            {
                var lastChainCard = Util.GetLastChainCard();
                if (lastChainCard != null)
                {
                    foreach (int id in targetList)
                    {
                        if (lastChainCard.IsCode(id))
                        {
                            AI.SelectCard(id);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  K9 MONSTER EFFECTS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool K9SummonCheck()
        {
            // Don't summon K9 if opponent has negate available and we need to play through it
            if (EnemyHasKnownNegate() && Bot.GetMonsterCount() == 0) return false;
            if (IsSpecialSummonBlocked()) return false;
            // K9 can be Normal Summoned without Tribute if opponent has 2+ cards in hand
            if (Enemy.Hand.Count >= 2) return true;
            // Fallback: if we have no other play and need a body on field
            if (Bot.GetMonsterCount() == 0 && !Bot.HasInHand(CardId.VanquishSoulRazen) && !Bot.HasInHand(CardId.VanquishSoulDrMadLove))
                return true;
            // Don't waste normal summon if board is already strong
            if (IsBoardStrongEnough()) return false;
            return false;
        }

        private bool K9_66aJokulHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Reveal Jokul + Level 5 K9 โ’ Special Summon both
            return Bot.Hand.Any(c => c != null && c != Card && c.IsMonster() && c.Level == 5 &&
                (c.Id == CardId.K9_17Izuna || c.Id == CardId.K9_66bLantern || c.Id == CardId.K9_00Lupis));
        }

        private bool K9_66aJokulFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Don't search if board is strong and we don't need more setup
            if (IsBoardStrongEnough() && !NeedsBoardPresence()) return false;
            // Search non-Aqua K9 from Deck
            return HasK9DeckTarget(CardId.K9_17Izuna, CardId.K9_66bLantern, CardId.K9_00Lupis);
        }

        private bool K9_66bLanternHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Special Summon itself + Level 5 non-Pyro K9 from GY
            return Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.K9_66aJokul, CardId.K9_17Izuna, CardId.K9_00Lupis));
        }

        private bool K9_66bLanternFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Don't search if we already have A Case for K9 face-up or if board is strong
            if (Bot.HasInSpellZone(CardId.ACaseForK9, faceUp: true)) return false;
            if (IsBoardStrongEnough() && !NeedsBoardPresence()) return false;
            // Search K9 Spell/Trap
            return GetRemainingCount(CardId.ACaseForK9) > 0;
        }

        private bool K9_17IzunaHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            // Special Summon if opponent activated monster effect in hand/GY
            ClientCard last = Util.GetLastChainCard();
            return Duel.LastChainPlayer == 1 &&
                   last != null &&
                   last.Controller == 1 &&
                   last.IsMonster() &&
                   (last.Location == CardLocation.Hand || last.Location == CardLocation.Grave);
        }

        private bool K9_17IzunaFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Don't send if board is strong and we don't need GY setup
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            // Send K9 card from Deck to GY
            return HasK9DeckTarget(CardId.K9_66aJokul, CardId.K9_66bLantern, CardId.K9_00Lupis);
        }

        private bool K9_00LupisEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_lupisUsed) return false;
            if (IsBoardStrongEnough() && !NeedsBoardPresence()) return false;
            if (!HasK9DeckTarget(CardId.K9_66aJokul, CardId.K9_66bLantern, CardId.K9_17Izuna)) return false;
            _lupisUsed = true;
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  VANQUISH SOUL MONSTER EFFECTS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool VanquishSoulRazenEffect()
        {
            // Summon search / Field Quick Effect
            if (Card.Location == CardLocation.Hand) return false; // Hand: no trigger
            if (Duel.Player == 0)
            {
                // Our turn: search if summoned this turn, or reveal
                return true;
            }
            // Opponent's turn: Quick Effect reveal โ€” only if we have attributes to reveal
            return Bot.Hand.Any(c => c != null && (c.HasAttribute(CardAttribute.Fire) || c.HasAttribute(CardAttribute.Dark)));
        }

        private bool VanquishSoulDrMadLoveEffect()
        {
            if (Card.Location == CardLocation.Hand) return false;
            if (Duel.Player == 0)
            {
                return true; // Our turn: search VS spell/trap or reveal
            }
            return Bot.Hand.Any(c => c != null && (c.HasAttribute(CardAttribute.Dark) || c.HasAttribute(CardAttribute.Earth)));
        }

        private bool VanquishSoulJiaolongEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Hand: trigger when VS card/effect is activated
                return true;
            }
            // Field: reveal FIRE x2 to search/disrupt
            return Bot.Hand.Count(c => c != null && c.HasAttribute(CardAttribute.Fire)) >= 1;
        }

        private bool VanquishSoulHeavyBorgerEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Bounce face-up non-Machine VS to SS itself
                return Bot.GetMonsters().Any(c => c != null && IsVanquishSoul(c) && c.Id != CardId.VanquishSoulHeavyBorger && c.IsFaceup());
            }
            // Field: draw 1 (needs DARK) or reveal 3 attributes
            return Bot.Hand.Any(c => c != null && c.HasAttribute(CardAttribute.Dark));
        }

        private bool VanquishSoulHollieSueEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Reveal another VS in hand to SS itself
                return Bot.Hand.Any(c => c != null && IsVanquishSoul(c) && c != Card);
            }
            // Field: skip if board is already strong and no threat to deal with
            if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
            return true;
        }

        private bool VanquishSoulCaesarValiusEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Bounce non-Dragon VS to SS itself
                return Bot.GetMonsters().Any(c => c != null && IsVanquishSoul(c) && c.Id != CardId.VanquishSoulCaesarValius && c.IsFaceup());
            }
            // Field: reveal EARTH to destroy/protect
            // Skip if board is already strong and no threat
            if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
            return Bot.Hand.Any(c => c != null && c.HasAttribute(CardAttribute.Earth));
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  EXTRA DECK
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool SPLittleKnightSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            if (!ShouldExtraDeckNow()) return false;
            // Only summon if there's a target worth banishing on opponent's field
            return Enemy.GetMonsterCount() + Enemy.GetSpellCount() >= 2;
        }

        private bool SPLittleKnightEffect()
        {
            // Only banish if opponent has a real threat
            if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
                return true;
            return false;
        }

        private bool RockOfTheVanquisherSummon()
        {
            if (!ShouldExtraDeckNow()) return false;
            return Bot.HasInMonstersZone(new[]
            {
                CardId.VanquishSoulRazen,
                CardId.VanquishSoulDrMadLove,
                CardId.VanquishSoulJiaolong
            });
        }

        private bool RockOfTheVanquisherEffect()
        {
            return true;
        }

        private bool K9_17RipperSummon()
        {
            if (!ShouldExtraDeckNow()) return false;
            // Don't summon if we need the materials for other plays
            if (ShouldSkipCombo()) return false;
            // Need 2 Level 5 monsters (K9s)
            var level5s = Bot.GetMonsters().Count(c => c != null && c.Level == 5);
            return level5s >= 2;
        }

        private bool K9_17RipperEffect()
        {
            if (Duel.Player == 0) return true; // Detach & search on our turn
            if (Duel.Player == 1 && Duel.LastChainPlayer == 1) return true; // Negate on opponent's turn
            return false;
        }

        private bool ZeusSummon()
        {
            // Only summon Zeus AFTER battle (MP2) and only if we have an Xyz that attacked
            if (Duel.Phase != DuelPhase.Main2) return false;
            return Bot.GetMonsters().Any(c => c != null && c.HasType(CardType.Xyz) && c.Attacked);
        }

        private bool ZeusEffect()
        {
            // Only activate if opponent has 2+ cards on field
            return Enemy.GetFieldCount() >= 2;
        }

        private bool BystialMagnamhutSpSummon()
        {
            if (_magnamhutUsed) return false;
            bool opponentHasTarget = Enemy.Graveyard.Any(c => c != null && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
            if (opponentHasTarget)
            {
                _magnamhutUsed = true;
                return true;
            }

            int k9InGrave = Bot.Graveyard.Count(c => c != null &&
                (c.Id == CardId.K9_66aJokul || c.Id == CardId.K9_66bLantern ||
                 c.Id == CardId.K9_00Lupis || c.Id == CardId.K9_17Izuna));
            if (k9InGrave < 2)
            {
                // Don't waste Magnamhut if no meaningful target
                if (!NeedsBoardPresence() && !IsInGrindGame()) return false;
            }

            bool selfHasTarget = Bot.Graveyard.Any(c => c != null && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark)));
            if (selfHasTarget)
            {
                _magnamhutUsed = true;
                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  SIDE DECK
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool SuperPolymerizationEffect()
        {
            if (Bot.Hand.Count == 0) return false;
            return Bot.GetMonsterCount() + Enemy.GetMonsterCount() >= 2;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  SPELL SET โ€” filtered (user rule: ONE is enough)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool SpellSetFiltered()
        {
            // Never set duplicate continuous/field cards
            if (Card.IsCode(CardId.ACaseForK9) && Bot.HasInSpellZone(Card.Id))
                return false;
            // Once-per-turn spells: don't set multiple copies
            if (Card.IsCode(CardId.StakeYourSoul) && Bot.GetSpellCount() >= 3)
                return false; // already have enough backrow
            if (Card.IsCode(CardId.VanquishSoulStart) && Bot.GetSpellCount() >= 3)
                return false;

            return DefaultSpellSet();
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  HELPERS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool IsVanquishSoul(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.VanquishSoulRazen ||
                   card.Id == CardId.VanquishSoulJiaolong ||
                   card.Id == CardId.VanquishSoulDrMadLove ||
                   card.Id == CardId.VanquishSoulHeavyBorger ||
                   card.Id == CardId.VanquishSoulHollieSue ||
                   card.Id == CardId.VanquishSoulCaesarValius;
        }

        private bool HasK9DeckTarget(params int[] ids)
        {
            foreach (int id in ids)
                if (GetRemainingCount(id) > 0) return true;
            return false;
        }

        /// <summary>
        /// Check if the deck has a Vanquish Soul monster with the given attribute, excluding a specific card.
        /// Uses GetRemainingCount instead of Bot.Deck.Any() โ€” cards in deck are face-down with ID 0.
        /// </summary>
        private bool HasRemainingVSAttribute(CardAttribute attr, int excludeId)
        {
            int[] vsMonsters = { CardId.VanquishSoulRazen, CardId.VanquishSoulJiaolong,
                                 CardId.VanquishSoulDrMadLove, CardId.VanquishSoulHeavyBorger,
                                 CardId.VanquishSoulHollieSue, CardId.VanquishSoulCaesarValius };
            foreach (int id in vsMonsters)
            {
                if (id == excludeId) continue;
                if (GetRemainingCount(id) > 0)
                {
                    var named = YGOSharp.OCGWrapper.NamedCard.Get(id);
                    if (named != null && ((int)named.Attribute & (int)attr) != 0)
                        return true;
                }
            }
            return false;
        }

        // ===== Battle Safety Helpers =====
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

        private bool MonsterRepos()
        {
            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster == null || IsAceCard(monster)) continue;
                bool enemyEmpty = Enemy.GetMonsterCount() == 0;
                if (monster.IsAttack())
                {
                    if (!enemyEmpty && !IsSafeToAttack(monster) && IsSafeToDefend(monster))
                        return true;
                }
                else
                {
                    if (enemyEmpty || IsSafeToAttack(monster))
                        return true;
                }
            }
            return false;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ON SELECT CARD โ€” Intelligent Card Selection
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // ===== HINT-BASED SELECTIONS =====

            // hint 533 = HINTMSG_LMATERIAL โ€” Link Material: protect Ace Cards
            if (hint == 533)
            {
                var safe = cards.Where(c => c != null && !IsAceCard(c)).ToList();
                if (safe.Count >= min) return safe.Take(max).ToList();
            }

            // hint 502 = HINTMSG_DESTROY โ€” destroy target selection
            if (hint == 502)
            {
                var nonAce = cards.Where(c => c != null && !IsAceCard(c))
                    .OrderByDescending(c => c.Attack).ToList();
                if (nonAce.Count >= min) return nonAce.Take(max).ToList();
            }

            // hint 509 = HINTMSG_SPSUMMON โ€” Special Summon target
            if (hint == 509)
            {
                var aceMonsters = cards.Where(c => c != null && IsAceCard(c) && c.IsCanRevive()).ToList();
                if (aceMonsters.Count >= min) return aceMonsters.Take(max).ToList();
            }

            // hint 549 = HINTMSG_ATTACKTARGET โ€” Battle Phase attack target
            if (hint == 549)
            {
                int ourBestAtk = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack())
                    .Select(c => c.Attack)
                    .DefaultIfEmpty(0).Max();
                var beatable = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone
                    && (c.IsAttack() ? c.Attack < ourBestAtk : c.Defense < ourBestAtk)).ToList();
                if (beatable.Count >= min) return beatable.OrderByDescending(c => c.Attack).Take(max).ToList();
                var validTargets = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone).ToList();
                if (validTargets.Count >= min) return validTargets.Take(max).ToList();
            }

            // ===== CARD-CONTEXT SELECTIONS =====
            if (Card == null) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Ace card protection: avoid using face-up Ace cards as materials/tributes
            if (cards.Any(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone && IsAceCard(c)))
            {
                var safeCards = cards.Where(c => c == null || c.Controller != 0 || c.Location != CardLocation.MonsterZone || !IsAceCard(c)).ToList();
                if (safeCards.Count >= min)
                    return safeCards.Take(max).ToList();
            }

            // ROTA โ’ always search Razen
            if (Card.Id == CardId.ReinforcementOfTheArmy)
                return SelectPreferred(cards, min, max, CardId.VanquishSoulRazen);

            // Pot of Prosperity โ’ banish low-priority extras
            if (Card.Id == CardId.PotOfProsperity)
                return SelectPreferred(cards, min, max,
                    CardId.SeaMonsterOfTheseus, CardId.PanzerDragon,
                    CardId.Number104, CardId.NumberC104,
                    CardId.RockOfTheVanquisher, CardId.K9_17Ripper,
                    CardId.Garura, CardId.MudragonOfTheSwamp);

            // A Case for K9 โ’ search Jokul first (starter)
            if (Card.Id == CardId.ACaseForK9)
                return SelectPreferred(cards, min, max,
                    CardId.K9_66aJokul, CardId.K9_17Izuna, CardId.K9_66bLantern, CardId.K9_00Lupis);

            // K9 66a Jokul
            if (Card.Id == CardId.K9_66aJokul)
                return SelectPreferred(cards, min, max,
                    CardId.K9_17Izuna, CardId.K9_66bLantern, CardId.K9_00Lupis);

            // K9 66b Lantern
            if (Card.Id == CardId.K9_66bLantern)
            {
                if (Card.Location == CardLocation.Hand)
                    return SelectPreferred(cards, min, max,
                        CardId.K9_17Izuna, CardId.K9_66aJokul, CardId.K9_00Lupis);
                else
                    return SelectPreferred(cards, min, max, CardId.ACaseForK9);
            }

            // K9 17 Izuna
            if (Card.Id == CardId.K9_17Izuna && Card.Location == CardLocation.MonsterZone)
                return SelectPreferred(cards, min, max,
                    CardId.K9_00Lupis, CardId.K9_66bLantern, CardId.K9_66aJokul);

            // VS Razen โ€” search
            if (Card.Id == CardId.VanquishSoulRazen && Card.Location == CardLocation.MonsterZone && max == 1)
            {
                var searchCandidates = new[] { CardId.VanquishSoulDrMadLove, CardId.VanquishSoulHeavyBorger, CardId.VanquishSoulJiaolong, CardId.VanquishSoulCaesarValius, CardId.VanquishSoulHollieSue };
                int bestSearch = GetVSSearchPriority(Bot.Hand, Bot.GetMonsters(), searchCandidates);
                return SelectPreferred(cards, min, max, bestSearch);
            }

            // VS Dr. Mad Love โ€” search spell/trap
            if (Card.Id == CardId.VanquishSoulDrMadLove && Card.Location == CardLocation.MonsterZone && max == 1)
                return SelectPreferred(cards, min, max, CardId.StakeYourSoul, CardId.VanquishSoulSnowDevil, CardId.VanquishSoulStart);

            // VS Jiaolong โ€” search monster or spell
            if (Card.Id == CardId.VanquishSoulJiaolong && Card.Location == CardLocation.MonsterZone && max == 1)
            {
                var searchCandidates = new[] { CardId.VanquishSoulRazen, CardId.VanquishSoulDrMadLove, CardId.VanquishSoulHeavyBorger, CardId.VanquishSoulCaesarValius, CardId.VanquishSoulHollieSue, CardId.VanquishSoulSnowDevil, CardId.StakeYourSoul };
                int bestSearch = GetVSSearchPriority(Bot.Hand, Bot.GetMonsters(), searchCandidates);
                return SelectPreferred(cards, min, max, bestSearch);
            }

            // VS Heavy Borger โ€” bounce target selection
            if (Card.Id == CardId.VanquishSoulHeavyBorger && Card.Location == CardLocation.Hand)
                return SelectPreferred(cards, min, max, CardId.VanquishSoulDrMadLove, CardId.VanquishSoulRazen, CardId.VanquishSoulJiaolong, CardId.VanquishSoulHollieSue);

            // VS Caesar Valius โ€” bounce target selection
            if (Card.Id == CardId.VanquishSoulCaesarValius && Card.Location == CardLocation.Hand)
                return SelectPreferred(cards, min, max, CardId.VanquishSoulDrMadLove, CardId.VanquishSoulRazen, CardId.VanquishSoulJiaolong, CardId.VanquishSoulHollieSue);

            // VS Hollie Sue โ€” reveal target
            if (Card.Id == CardId.VanquishSoulHollieSue && Card.Location == CardLocation.Hand && max == 1)
                return SelectPreferred(cards, min, max, CardId.VanquishSoulRazen, CardId.VanquishSoulDrMadLove, CardId.VanquishSoulJiaolong, CardId.VanquishSoulHeavyBorger, CardId.VanquishSoulCaesarValius);

            // Rock of the Vanquisher โ€” search
            if (Card.Id == CardId.RockOfTheVanquisher)
            {
                var searchCandidates = new[] { CardId.VanquishSoulRazen, CardId.VanquishSoulDrMadLove, CardId.VanquishSoulHeavyBorger, CardId.VanquishSoulJiaolong, CardId.VanquishSoulCaesarValius, CardId.VanquishSoulHollieSue };
                int bestSearch = GetVSSearchPriority(Bot.Hand, Bot.GetMonsters(), searchCandidates);
                return SelectPreferred(cards, min, max, bestSearch);
            }

            // Stake Your Soul โ€” select reveal target
            if (Card.Id == CardId.StakeYourSoul)
            {
                var priority = new[] { CardId.VanquishSoulJiaolong, CardId.VanquishSoulHeavyBorger, CardId.VanquishSoulCaesarValius, CardId.VanquishSoulDrMadLove, CardId.VanquishSoulRazen, CardId.VanquishSoulHollieSue };
                foreach (int id in priority)
                {
                    var card = cards.FirstOrDefault(c => c != null && c.Id == id);
                    if (card != null)
                    {
                        // Check the pair exists in deck using GetRemainingCount
                        bool pairExists = false;
                        if (id == CardId.VanquishSoulRazen) pairExists = GetRemainingCount(CardId.VanquishSoulJiaolong) > 0;
                        else if (id == CardId.VanquishSoulJiaolong) pairExists = GetRemainingCount(CardId.VanquishSoulRazen) > 0;
                        else if (id == CardId.VanquishSoulDrMadLove) pairExists = GetRemainingCount(CardId.VanquishSoulHeavyBorger) > 0;
                        else if (id == CardId.VanquishSoulHeavyBorger) pairExists = GetRemainingCount(CardId.VanquishSoulDrMadLove) > 0;
                        else if (id == CardId.VanquishSoulHollieSue) pairExists = GetRemainingCount(CardId.VanquishSoulCaesarValius) > 0;
                        else if (id == CardId.VanquishSoulCaesarValius) pairExists = GetRemainingCount(CardId.VanquishSoulHollieSue) > 0;
                        if (pairExists) return new List<ClientCard> { card };
                    }
                }
            }

            // VS Start โ€” select monster to shuffle
            if (Card.Id == CardId.VanquishSoulStart)
            {
                var target = cards.FirstOrDefault(c => c != null && GetRemainingCount(c.Id) > 1);
                if (target != null) return new List<ClientCard> { target };
            }

            // Bystial Magnamhut โ€” banish opponent's card first
            if (Card.Id == CardId.BystialMagnamhut)
            {
                var target = cards.FirstOrDefault(c => c != null && c.Controller == 1) ?? cards.FirstOrDefault(c => c != null);
                if (target != null) return new List<ClientCard> { target };
            }

            // Super Poly โ€” use opponent's monsters
            if (Card.Id == CardId.SuperPolymerization)
            {
                var opponentMonsters = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (opponentMonsters.Count >= min) return opponentMonsters.Take(max).ToList();
            }

            // K9 17 Ripper โ€” detach priority
            if (Card.Id == CardId.K9_17Ripper)
                return SelectPreferred(cards, min, max, CardId.ACaseForK9, CardId.K9_66aJokul, CardId.K9_66bLantern, CardId.K9_17Izuna, CardId.K9_00Lupis);

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ===== Fusion Material: protect Ace =====
        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            // Super Polymerization: use opponent monsters first
            if (Card != null && Card.Id == CardId.SuperPolymerization)
            {
                var oppMonsters = cards.Where(c => c != null
                    && c.Controller == 1 && c.Location == CardLocation.MonsterZone).ToList();
                if (oppMonsters.Count >= min) return oppMonsters.Take(max).ToList();
            }

            // Regular: hand > field expendable > field non-Ace
            var result = new List<ClientCard>();
            var handMats = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
            var safeField = cards.Where(c => c != null
                && c.Location == CardLocation.MonsterZone && !IsAceCard(c)).ToList();
            foreach (var c in handMats) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max)
                foreach (var c in safeField) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count >= min) return result;

            return base.OnSelectFusionMaterial(cards, min, max);
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ON SELECT OPTION
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            // Rock of the Vanquisher: Option 0 = SS from hand, Option 1 = Add from GY
            if (Card != null && Card.Id == CardId.RockOfTheVanquisher)
            {
                bool hasRazenOrMadLoveInGY = Bot.Graveyard.Any(c => c != null && (c.Id == CardId.VanquishSoulRazen || c.Id == CardId.VanquishSoulDrMadLove));
                if (hasRazenOrMadLoveInGY && options.Count > 1) return 1;
                return 0;
            }
            return 0;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ON SELECT XYZ / LINK MATERIAL
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            var nonNullCards = cards.Where(c => c != null).ToList();
            IList<ClientCard> result = Util.SelectPreferredCards(new[] {
                CardId.K9_00Lupis, CardId.K9_66aJokul,
                CardId.K9_17Izuna, CardId.K9_66bLantern
            }, nonNullCards, min, max);
            return Util.CheckSelectCount(result, nonNullCards, min, max);
        }



        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  HELPERS
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private IList<ClientCard> SelectPreferred(IList<ClientCard> cards, int min, int max, params int[] preferredIds)
        {
            var result = new List<ClientCard>();
            foreach (int id in preferredIds)
            {
                var matches = cards.Where(c => c != null && c.Id == id && !result.Contains(c)).ToList();
                foreach (var m in matches)
                {
                    result.Add(m);
                    if (result.Count >= max) break;
                }
                if (result.Count >= max) break;
            }
            if (result.Count < min)
            {
                foreach (var card in cards)
                {
                    if (card != null && !result.Contains(card)) result.Add(card);
                    if (result.Count >= min) break;
                }
            }
            return result;
        }

        private int GetVSSearchPriority(IList<ClientCard> hand, IList<ClientCard> monstersOnField, int[] candidateIds)
        {
            int fireInHand = hand.Count(c => c != null && c.HasAttribute(CardAttribute.Fire));
            int darkInHand = hand.Count(c => c != null && c.HasAttribute(CardAttribute.Dark));
            int earthInHand = hand.Count(c => c != null && c.HasAttribute(CardAttribute.Earth));
            int fireNeeded = 0, darkNeeded = 0, earthNeeded = 0;

            foreach (var m in monstersOnField)
            {
                if (m == null || !m.IsFaceup()) continue;
                if (m.Id == CardId.VanquishSoulRazen) { fireNeeded = Math.Max(fireNeeded, 1); darkNeeded = Math.Max(darkNeeded, 1); }
                else if (m.Id == CardId.VanquishSoulDrMadLove) { darkNeeded = Math.Max(darkNeeded, 1); earthNeeded = Math.Max(earthNeeded, 1); }
                else if (m.Id == CardId.VanquishSoulJiaolong) { fireNeeded = Math.Max(fireNeeded, 2); }
                else if (m.Id == CardId.VanquishSoulHeavyBorger) { darkNeeded = Math.Max(darkNeeded, 1); earthNeeded = Math.Max(earthNeeded, 1); fireNeeded = Math.Max(fireNeeded, 1); }
                else if (m.Id == CardId.VanquishSoulCaesarValius) { earthNeeded = Math.Max(earthNeeded, 1); fireNeeded = Math.Max(fireNeeded, 1); darkNeeded = Math.Max(darkNeeded, 1); }
                else if (m.Id == CardId.VanquishSoulHollieSue) { earthNeeded = Math.Max(earthNeeded, 1); fireNeeded = Math.Max(fireNeeded, 1); darkNeeded = Math.Max(darkNeeded, 1); }
            }

            int fireDeficit = Math.Max(0, fireNeeded - fireInHand);
            int darkDeficit = Math.Max(0, darkNeeded - darkInHand);
            int earthDeficit = Math.Max(0, earthNeeded - earthInHand);

            var sorted = candidateIds.OrderByDescending(id =>
            {
                var attr = GetVSAttribute(id);
                if (attr == CardAttribute.Fire) return fireDeficit;
                if (attr == CardAttribute.Dark) return darkDeficit;
                if (attr == CardAttribute.Earth) return earthDeficit;
                return 0;
            }).ToList();

            return sorted.FirstOrDefault();
        }

        private CardAttribute GetVSAttribute(int id)
        {
            if (id == CardId.VanquishSoulRazen || id == CardId.VanquishSoulJiaolong) return CardAttribute.Fire;
            if (id == CardId.VanquishSoulDrMadLove || id == CardId.VanquishSoulHeavyBorger) return CardAttribute.Dark;
            if (id == CardId.VanquishSoulCaesarValius || id == CardId.VanquishSoulHollieSue) return CardAttribute.Earth;
            return 0;
        }

        private bool BystialMagnamhutEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return BystialMagnamhutSpSummon();
            }
            return true;
        }

        private bool VallonEffect()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup());
        }

        private bool NashKnightEffect()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup());
        }

        private bool CXyzNaschKnightEffect()
        {
            return (GetRemainingCount(CardId.Number104) > 0 || GetRemainingCount(CardId.NumberC104) > 0);
        }
    }
}
