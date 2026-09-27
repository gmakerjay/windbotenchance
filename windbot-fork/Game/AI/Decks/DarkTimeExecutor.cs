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
    // 2026_DarkTime
    // ==========================================
    // ============================================================
    // CARD AUDIT โ€” 2026_DarkTime
    // ============================================================
    // Card Name                 | Type       | OPT? | Effect Summary                                   | Activate When                               | NEVER Activate When                          |
    // --------------------------|------------|------|--------------------------------------------------|---------------------------------------------|----------------------------------------------|
    // Swift Panther Warrior     | Monster    | Yes  | Tribute to SS DarkTime from Deck; Quick trib+SS  | Need DarkTime engine / targeted             | No tribute fodder / board already strong    |
    // Alligator's Sword DK      | Monster    | Yes  | Discard DarkTime to SS; On-SS set non-Dice S/T  | Have DarkTime in hand                       | Already have enough backrow                  |
    // Jinzo Energy Shocker     | Monster    | Yes  | Negate all S/T on field (as long as face-up)     | Enemy has spells/traps > 0                 | Enemy has no backrow                        |
    // Fisherman Legend of Sea  | Monster    | Yes  | Quick bounce opponent monster (from hand)        | Opponent has face-up target                 | No DarkTime on field / no target            |
    // Fydraulis Harmonia       | Monster    | Yes  | Quick Synchro from Extra (hand discard)          | Opponent summons monster                   | No Synchro targets / used already           |
    // DarkTime Wizard          | Spell      | Yes  | Search DarkTime card or board wipe               | Main Phase / need setup                     | Used already / enemy board weak             |
    // Pot of Extravagance      | Spell      | Yes  | Draw 2, banish 6 from Extra                         | Need cards, have โฅ6 Extra                  | Banned Extra would lose too many Synchros   |
    // Fire Formation - Tenki   | Spell      | No   | Search Beast-Warrior (Panther)                   | Main Phase / need Panther                   | Already have Panther in hand or field       |
    // Graceful Skull Dice      | Spell/Trap | Yes  | GY banish to negate; Field boost monster ATK     | Battle Phase or opponent summons            | No DarkTime on field / no target            |
    // Sleeping Scapegoats      | Trap       | Yes  | Special Summon 2 tokens (LP โฅ opp)              | End Phase / attack declared / need tribute  | Already have enough tokens                  |
    // Foolish Graverobber      | Spell      | Yes  | Banish face-down from Extra to Foolish any      | Need grave setup / need Jinzo               | No extra fodder / already have setup        |
    // Reversal Box             | Trap       | Yes  | Swap ATK/DEF of 1 monster; 2nd: tribute + banish| Opponent's turn / battle phase              | No target / no DarkTime on field            |
    // ============================================================
    // ACE CARDS:
    //   Primary  : Psychic End Punisher โ€” big beater + damage reflection
    //   Secondary: Favorite Hero Flame Wingman โ€” burn + ATK boost
    //   Tertiary : Jinzo Energy Shocker โ€” S/T lock
    //   Quatern  : Enigmaster Packbit / Wind Pegasus Ignister / Golden Cloud Beast Malong
    // ============================================================
    // STRATEGY:
    //   Turn 1: Search Tenki โ’ Panther โ’ SS Alligator/DarkTime engine
    //   Turn 2: Break board with Harmonia Synchro + Jinzo lock + Reversal Box
    //   Engine: DarkTime Wizard for search; Panther for extension
    // ============================================================
    [Deck("2026_DarkTime", "2026_DarkTime")]
    public class _2026_DarkTimeExecutor : ModernExecutor
    {
        public class CardId
        {
            public const int SwiftPantherWarrior = 101402001;
            public const int AlligatorsSwordDragonKnight = 101402002;
            public const int JinzoEnergyShocker = 101402003;
            public const int FishermanLegendOfTheSea = 101402004;
            public const int FydraulisHarmonia = 70088809;
            public const int MulcharmyFuwalos = 42141493;
            public const int AshBlossom = 14558127;
            public const int GhostBelle = 73642296;
            public const int PotOfExtravagance = 49238328;
            public const int GracefulSkullDice = 101402053;
            public const int SleepingScapegoats = 101402054;
            public const int DarkTimeWizard = 101402052;
            public const int ForbiddenDroplet = 24299458;
            public const int FireFormationTenki = 57103969;
            public const int CalledByTheGrave = 24224830;
            public const int FoolishGraverobber = 101402070;
            public const int InfiniteImpermanence = 10045474;
            public const int ReversalBox = 101402071;
            // Extra
            public const int FavoriteHeroFlameWingman = 13243124;
            public const int Garura = 11765832;
            public const int MudragonOfTheSwamp = 54757758;
            public const int PsychicEndPunisher = 60465049;
            public const int EnigmasterPackbit = 72444406;
            public const int WindPegasusIgnister = 98506199;
            public const int GoldenCloudBeastMalong = 93125329;
            // Side
            public const int MulcharmyPurulia = 84192580;
            public const int SuperPolymerization = 48130397;
            public const int DoubleCyclone = 75652080;
            public const int EvenlyMatched = 15693423;
            public const int SolemnReport = 78114463;
        }

        private static readonly int[] DarkTimeWizardCards = {
            CardId.SwiftPantherWarrior, CardId.AlligatorsSwordDragonKnight,
            CardId.JinzoEnergyShocker, CardId.FishermanLegendOfTheSea,
            CardId.GracefulSkullDice, CardId.SleepingScapegoats,
            CardId.DarkTimeWizard, CardId.FoolishGraverobber, CardId.ReversalBox
        };

        private static readonly int[] AceCardIds = {
            CardId.PsychicEndPunisher,
            CardId.FavoriteHeroFlameWingman,
            CardId.JinzoEnergyShocker,
            CardId.EnigmasterPackbit,
            CardId.WindPegasusIgnister,
            CardId.GoldenCloudBeastMalong,
            CardId.SwiftPantherWarrior,
            CardId.AlligatorsSwordDragonKnight
        };



        // Once per turn / state flags
        private bool _tenkiUsed = false;
        private bool _harmoniaUsed = false;
        private bool _extravaganceUsed = false;
        private bool _pantherUsed = false;
        private bool _alligatorHandUsed = false;
        private bool _alligatorFieldUsed = false;
        private bool _shockerUsed = false;
        private bool _fishermanHandUsed = false;
        private bool _fishermanFieldUsed = false;
        private bool _wizardUsed = false;
        private bool _diceUsed = false;
        private bool _diceGyUsed = false;
        private bool _scapegoatsUsed = false;
        private bool _graverobberUsed = false;
        private bool _graverobberGyUsed = false;
        private bool _dropletUsed = false;
        private bool _polyUsed = false;
        private bool _evenlyUsed = false;
        private bool _cycloneUsed = false;
        private bool _reportUsed = false;

        // FieldGuard inherited: IsSpecialSummonBlocked, CanDealLethal, CanOTK,
        // ShouldSkipCombo, NeedsBoardPresence, IsInGrindGame, EnemyHasKnownNegate

        protected override bool IsBoardStrongEnough()
        {
            // Deck-specific boss configuration checks
            if (Bot.HasInMonstersZone(CardId.PsychicEndPunisher) && Bot.GetMonsterCount() >= 2)
                return true;
            if (Bot.HasInMonstersZone(CardId.JinzoEnergyShocker) && Bot.GetSpellCount() >= 1)
                return true;
            if (Bot.HasInMonstersZone(CardId.FavoriteHeroFlameWingman) && Bot.GetMonsterCount() >= 2)
                return true;
            if (Bot.HasInMonstersZone(CardId.EnigmasterPackbit) && Bot.HasInMonstersZone(CardId.SwiftPantherWarrior))
                return true;
            if (Bot.HasInMonstersZone(CardId.WindPegasusIgnister) && Bot.GetSpellCount() >= 1)
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

        // ===== SelectPreferred helper =====
        private IList<ClientCard> SelectPreferred(
            IList<ClientCard> cards, int min, int max, params int[] preferredIds)
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
                    if (card != null && !result.Contains(card))
                    {
                        result.Add(card);
                        if (result.Count >= min) break;
                    }
                }
            }
            return result;
        }

        public override bool OnSelectHand()
        {
            // DarkTime control/lock: prefer going first to establish Jinzo lock + backrow + Harmonia/Fisherman handtraps
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _tenkiUsed = false;
            _harmoniaUsed = false;
            _extravaganceUsed = false;
            _pantherUsed = false;
            _alligatorHandUsed = false;
            _alligatorFieldUsed = false;
            _shockerUsed = false;
            _fishermanHandUsed = false;
            _fishermanFieldUsed = false;
            _wizardUsed = false;
            _diceUsed = false;
            _diceGyUsed = false;
            _scapegoatsUsed = false;
            _graverobberUsed = false;
            _graverobberGyUsed = false;
            _dropletUsed = false;
            _polyUsed = false;
            _evenlyUsed = false;
            _cycloneUsed = false;
            _reportUsed = false;

            // ── Going-Second BreakBoard: prioritize disruption over combo ──
            if (ShouldGoBreakBoard)
            {
                _dropletUsed = false;
                _evenlyUsed = false;
            }
        }

        public _2026_DarkTimeExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.SwiftPantherWarrior, CardId.JinzoEnergyShocker },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.SwiftPantherWarrior, ActionType = ExecutorType.Activate, Description = "Play CardId.SwiftPantherWarrior" },
                    new() { CardId = CardId.JinzoEnergyShocker, ActionType = ExecutorType.Activate, Description = "Extend with CardId.JinzoEnergyShocker" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.SwiftPantherWarrior, CardId.AlligatorsSwordDragonKnight);
            BaitPlanner.RegisterBaitCards(CardId.PotOfExtravagance, CardId.AlligatorsSwordDragonKnight);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.SwiftPantherWarrior, CardId.AlligatorsSwordDragonKnight, CardId.InfiniteImpermanence);

            // 1. Hand Traps / Negations / Counter-Traps (highest priority)
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnReport, SolemnReportEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);

            // 2. React / Interruption / Graveyard Triggers
            AddExecutor(ExecutorType.Activate, CardId.FydraulisHarmonia, FydraulisHarmoniaEffect);
            AddExecutor(ExecutorType.Activate, CardId.GoldenCloudBeastMalong, GoldenCloudBeastMalongEffect);
            AddExecutor(ExecutorType.Activate, CardId.EnigmasterPackbit, EnigmasterPackbitEffect);
            AddExecutor(ExecutorType.Activate, CardId.WindPegasusIgnister, WindPegasusIgnisterEffect);
            AddExecutor(ExecutorType.Activate, CardId.Garura, GaruraEffect);
            AddExecutor(ExecutorType.Activate, CardId.FishermanLegendOfTheSea, FishermanLegendOfTheSeaEffect);
            AddExecutor(ExecutorType.Activate, CardId.GracefulSkullDice, GracefulSkullDiceEffect);
            AddExecutor(ExecutorType.Activate, CardId.PsychicEndPunisher, PsychicEndPunisherEffect);

            // 3. Spells (search/draw first)
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance, PotOfExtravaganceEffect);
            AddExecutor(ExecutorType.Activate, CardId.FireFormationTenki, FireFormationTenkiEffect);
            AddExecutor(ExecutorType.Activate, CardId.DarkTimeWizard, DarkTimeWizardEffect);
            AddExecutor(ExecutorType.Activate, CardId.FoolishGraverobber, FoolishGraverobberEffect);

            // 4. Special Summons / Monster Effects (In-Hand and Field)
            AddExecutor(ExecutorType.Activate, CardId.AlligatorsSwordDragonKnight, AlligatorsSwordDragonEffect);
            AddExecutor(ExecutorType.Activate, CardId.SwiftPantherWarrior, SwiftPantherWarriorEffect);

            // Synchro Summons (Extra Deck)
            AddExecutor(ExecutorType.SpSummon, CardId.PsychicEndPunisher);
            AddExecutor(ExecutorType.SpSummon, CardId.EnigmasterPackbit);
            AddExecutor(ExecutorType.SpSummon, CardId.WindPegasusIgnister);
            AddExecutor(ExecutorType.SpSummon, CardId.GoldenCloudBeastMalong);

            // 5. Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.SwiftPantherWarrior);
            AddExecutor(ExecutorType.Summon, CardId.AlligatorsSwordDragonKnight);
            AddExecutor(ExecutorType.Summon, CardId.FishermanLegendOfTheSea);
            AddExecutor(ExecutorType.Summon, CardId.JinzoEnergyShocker, JinzoEnergyShockerSummon);

            // Trigger on summon
            AddExecutor(ExecutorType.Activate, CardId.JinzoEnergyShocker, JinzoEnergyShockerEffect);

            // 6. Traps and other spells (react/defense)
            AddExecutor(ExecutorType.Activate, CardId.SleepingScapegoats, SleepingScapegoatsEffect);
            AddExecutor(ExecutorType.Activate, CardId.ReversalBox, ReversalBoxEffect);
            AddExecutor(ExecutorType.Activate, CardId.DoubleCyclone, DoubleCycloneEffect);
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedEffect);

            // Set backrow (Only Quick-Plays / Traps intended for opponent's turn)
            AddExecutor(ExecutorType.SpellSet, CardId.SleepingScapegoats);
            AddExecutor(ExecutorType.SpellSet, CardId.ReversalBox);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.DoubleCyclone);
            AddExecutor(ExecutorType.SpellSet, CardId.GracefulSkullDice);

            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }



        // โ•โ•โ• Hand Traps โ•โ•โ•
        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            // Only activate on opponent's turn when our field is empty (best value)
            if (Duel.Player != 1) return false;
            if (Bot.GetFieldCount() > 0) return false;
            // Don't activate if opponent already has many summons (likely played through hand traps)
            return true;
        }
        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && DefaultAshBlossomAndJoyousSpring();
        }
        private bool GhostBelleEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return DefaultGhostBelleAndHauntedMansion();
        }
        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && DefaultCalledByTheGrave();
        }
        private ClientCard GetPreemptiveImpermTarget()
        {
            int[] threatIds = {
                21522601, // Witchcrafter Madame Verre
                84523092, // Witchcrafter Haine
                1561110,  // ABC-Dragon Buster
                4280258,  // Apollousa, Bow of the Goddess
                10443957, // Cyber Dragon Infinity
                84815190, // Baronne de Fleur
                1508649   // Altergeist Hexstia
            };

            return Enemy.MonsterZone.GetMonsters().FirstOrDefault(c => 
                c != null && c.IsFaceup() && !c.IsDisabled() && 
                threatIds.Contains(c.Id) && 
                !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
        }

        private bool InfiniteImpermanenceEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            // Preemptive negation on our turn against known boss monsters
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) && Duel.LastChainPlayer == -1)
            {
                var target = GetPreemptiveImpermTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            if (!SmartHandTrapChain()) return false;

            return DefaultInfiniteImpermanence();
        }

        // โ•โ•โ• Helpers โ•โ•โ•
        private bool HasAnotherDarkTimeWizardCardInHand(ClientCard excludeCard = null)
        {
            foreach (ClientCard card in Bot.Hand)
                if (card != null && card != excludeCard && card.IsCode(DarkTimeWizardCards))
                    return true;
            return false;
        }

        // โ•โ•โ• Spells โ•โ•โ•
        private bool FireFormationTenkiEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            if (_tenkiUsed) return false;
            if (Bot.HasInSpellZone(CardId.FireFormationTenki)) return false;
            _tenkiUsed = true;
            AI.SelectCard(CardId.SwiftPantherWarrior);
            return true;
        }

        private bool PotOfExtravaganceEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            if (_extravaganceUsed) return false;

            // Only activate if we have at least 6 cards in Extra Deck
            if (Bot.ExtraDeck.Count < 6) return false;

            // Check if we have Harmonia in hand, and keep at least 5 Synchros if possible
            if (Bot.HasInHand(CardId.FydraulisHarmonia))
            {
                int synchroCount = Bot.ExtraDeck.Count(c => c != null && c.HasType(CardType.Synchro));
                if (synchroCount < 8) return false;
            }

            _extravaganceUsed = true;
            return true;
        }

        private bool DarkTimeWizardEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (_wizardUsed) return false;

            _wizardUsed = true;
            // Smart search: prefer Panther (core extender) > Alligator > support cards
            if (ShouldSkipCombo())
            {
                // OTK turn: search cards that help break board
                AI.SelectCard(CardId.SwiftPantherWarrior, CardId.ReversalBox, CardId.JinzoEnergyShocker);
            }
            else
            {
                // Normal setup: search Panther or Alligator for extension
                AI.SelectCard(CardId.SwiftPantherWarrior, CardId.AlligatorsSwordDragonKnight, CardId.SleepingScapegoats, CardId.ReversalBox, CardId.JinzoEnergyShocker);
            }
            return true;
        }

        // โ•โ•โ• Fydraulis Harmonia (Hand Trap) โ•โ•โ•
        private bool FydraulisHarmoniaEffect()
        {
            if (_harmoniaUsed) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1 || LastChainCard.Location != CardLocation.MonsterZone) return false;

            int synchroCount = Bot.ExtraDeck.Count(c => c != null && c.HasType(CardType.Synchro));
            if (synchroCount < 1) return false;

            _harmoniaUsed = true;
            return true;
        }

        // ─── Spells / Traps (Engine & Defense) ───
        private bool SleepingScapegoatsEffect()
        {
            if (_scapegoatsUsed) return false;
            if (Util.ChainContainsCard(CardId.SleepingScapegoats)) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.GetMonsterCount() >= 4) return false;

            // Opponent's turn: activate freely during Main, Battle, or End Phase
            if (Duel.Player == 1)
            {
                // If opponent controls a monster, we get up to 4 tokens + 1 Panther from Deck!
                if (Enemy.GetMonsterCount() > 0 || Duel.Phase == DuelPhase.End || Duel.Phase == DuelPhase.Battle)
                {
                    _scapegoatsUsed = true;
                    return true;
                }
            }

            // Our turn: only if we need board presence/tributes and opponent controls a monster
            if (Duel.Player == 0 && Duel.IsMainPhase())
            {
                if (Enemy.GetMonsterCount() > 0 && Bot.GetMonsterCount() <= 2)
                {
                    _scapegoatsUsed = true;
                    return true;
                }
            }

            return false;
        }

        private bool FoolishGraverobberEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (_graverobberGyUsed) return false;
                if (Bot.GetSpellCountWithoutField() < 5)
                {
                    _graverobberGyUsed = true;
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (_graverobberUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                _graverobberUsed = true;
                return true;
            }

            return false;
        }

        // ─── Monsters ───
        private bool AlligatorsSwordDragonEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_alligatorHandUsed) return false;
                if (IsSpecialSummonBlocked()) return false;
                if (!HasAnotherDarkTimeWizardCardInHand(Card)) return false;
                _alligatorHandUsed = true;
                return true;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_alligatorFieldUsed) return false;
                _alligatorFieldUsed = true;
                return true;
            }

            return false;
        }

        private bool SwiftPantherWarriorEffect()
        {
            if (_pantherUsed) return false;
            if (!Duel.IsMainPhase()) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Check if we have tribute fodder (token or monster != Card)
            bool hasFieldTribute = Bot.GetMonsters().Any(m => m != null && m != Card && !IsAceCard(m));
            bool hasHandTribute = Bot.Hand.Any(c => c != null && c.IsMonster() && c != Card);
            if (!hasFieldTribute && !hasHandTribute) return false;

            _pantherUsed = true;
            return true;
        }

        private bool JinzoEnergyShockerSummon()
        {
            // Jinzo is Lv 7 (requires 2 non-token tributes).
            // Usually summoned via Panther, Graverobber, or Reversal Box.
            var nonTokenMonsters = Bot.GetMonsters().Where(m => m != null && m.Id != 0 && !IsAceCard(m)).ToList();
            if (nonTokenMonsters.Count >= 2 && Enemy.GetSpellCount() > 0)
                return true;
            return false;
        }

        private bool JinzoEnergyShockerEffect()
        {
            if (_shockerUsed) return false;
            _shockerUsed = true;
            return Enemy.GetSpellCount() > 0;
        }

        private bool FishermanLegendOfTheSeaEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_fishermanHandUsed) return false;
                if (IsSpecialSummonBlocked()) return false;

                // In hand: Special Summon when opp monster activates effect or attacks, and we have DarkTime card on field
                bool hasDarkTimeOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(DarkTimeWizardCards) || c.Id == 0)) ||
                                         Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(DarkTimeWizardCards));
                if (!hasDarkTimeOnField) return false;

                if (!Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsShouldNotBeTarget()))
                    return false;

                _fishermanHandUsed = true;
                return true;
            }

            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_fishermanFieldUsed) return false;
                var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsShouldNotBeTarget());
                if (target != null)
                {
                    _fishermanFieldUsed = true;
                    AI.SelectCard(target);
                    return true;
                }
            }

            return false;
        }

        private bool ReversalBoxEffect()
        {
            // Face-down activation: only on opponent's turn (best value)
            if (Card.Location == CardLocation.SpellZone && !Card.IsFaceup())
            {
                if (Duel.Player != 1) return false;
                // Activate during opponent's Turn for accumulation & disruption
                return true;
            }

            // Face-up effect: When opponent monster activates effect or attacks -> toss coin, negate ATK & effects + SS from deck
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (Enemy.GetMonsterCount() == 0) return false;
                return true;
            }

            return false;
        }

        // โ•โ•โ• New Card Executors โ•โ•โ•
        private bool SolemnReportEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;
            if (_reportUsed) return false;
            if (LastChainCard.IsSpell() || LastChainCard.IsTrap())
            {
                if (Bot.LifePoints > 1500)
                {
                    _reportUsed = true;
                    return true;
                }
            }
            return false;
        }

        // ─── Extra Deck Payoffs & Synchros ───
        private bool GoldenCloudBeastMalongEffect()
        {
            var target = Enemy.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()) ??
                         Enemy.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool EnigmasterPackbitEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                return true;
            }
            if (Card.Location == CardLocation.Grave && Bot.Hand.Count > 0)
            {
                var target = Enemy.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool WindPegasusIgnisterEffect()
        {
            var target = Enemy.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()) ??
                         Enemy.SpellZone.FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool GaruraEffect()
        {
            return true;
        }

        private bool PsychicEndPunisherEffect()
        {
            if (Bot.LifePoints <= 1000) return false;
            var oppTarget = Enemy.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()) ??
                            Enemy.SpellZone.FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget());
            if (oppTarget != null)
            {
                var ourTribute = Bot.GetMonsters().FirstOrDefault(c => c != null && !c.IsCode(CardId.PsychicEndPunisher)) ?? Card;
                AI.SelectCard(ourTribute);
                AI.SelectNextCard(oppTarget);
                return true;
            }
            return false;
        }

        private bool ForbiddenDropletEffect()
        {
            if (_dropletUsed) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && !c.IsShouldNotBeTarget()).ToList();
            if (targets.Count == 0) return false;

            // Only use Droplet if there are meaningful threats or on our turn to break board
            if (!OpponentHasThreateningMonster() && targets.Count < 2 && Duel.Player != 0) return false;

            // Check if we have cost available (tokens, spells, non-Ace hand)
            bool hasCost = Bot.Hand.Any(c => c != Card && !IsAceCard(c)) ||
                           Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.FireFormationTenki, CardId.ReversalBox)) ||
                           Bot.GetMonsters().Any(c => c != null && (c.Id == 0 || (!IsAceCard(c) && c.Attack < 1500)));
            if (!hasCost) return false;

            _dropletUsed = true;
            return true;
        }

        private bool SuperPolymerizationEffect()
        {
            if (_polyUsed) return false;
            if (Bot.Hand.Count(c => c != Card) == 0) return false;

            if (Enemy.GetMonsterCount() >= 2)
            {
                _polyUsed = true;
                return true;
            }

            if (LastChainCard != null && LastChainCard.Controller == 1 &&
                Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && Util.IsChainTarget(m)))
            {
                _polyUsed = true;
                return true;
            }

            return false;
        }

        private bool GracefulSkullDiceEffect()
        {
            // GY effect: when opponent Normal or Special Summons a monster, banish to destroy that monster (2d6 >= 6, 72% success)
            if (Card.Location == CardLocation.Grave)
            {
                if (_diceGyUsed) return false;
                _diceGyUsed = true;
                return true;
            }

            // Field / Hand effect: boost ATK of DarkTime monsters (+1400 avg) & debuff enemy (-1400 avg)
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (_diceUsed) return false;
                bool controlDarkTimeMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(DarkTimeWizardCards));
                if (!controlDarkTimeMonster) return false;

                // Battle Phase / Damage Step
                if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.Damage)
                {
                    _diceUsed = true;
                    return true;
                }
            }

            return false;
        }

        private bool DoubleCycloneEffect()
        {
            if (_cycloneUsed) return false;

            var enemyST = Enemy.GetSpells().Where(c => c != null && c.IsFaceup()).ToList();
            if (enemyST.Count == 0) return false;

            // Only activate if enemy has meaningful backrow (not just a set card we can't see)
            // Check face-up Continuous S/T or Field spells as meaningful targets
            bool hasMeaningfulTarget = enemyST.Any(c => c.IsFaceup() && 
                (c.IsCode(57103969) || // Tenki
                 c.HasType(CardType.Continuous) || c.HasType(CardType.Field)));
            // If no face-up continuous S/T, only activate if we have expendable backrow
            if (!hasMeaningfulTarget) return false;

            // Prefer to destroy Tenki as cost (we already searched)
            var ourST = Bot.GetSpells().Where(c => c != null && c.IsCode(CardId.FireFormationTenki)).ToList();
            if (ourST.Count == 0)
            {
                // Can also destroy Reversal Box after using it, or expendable cards
                ourST = Bot.GetSpells().Where(c => c != null && (c.IsCode(CardId.ReversalBox) || c.IsCode(CardId.GracefulSkullDice))).ToList();
                if (ourST.Count == 0) return false;
            }

            _cycloneUsed = true;
            AI.SelectCard(ourST[0]);
            AI.SelectNextCard(enemyST[0]);
            return true;
        }

        private bool EvenlyMatchedEffect()
        {
            if (_evenlyUsed) return false;
            int ourCount = Bot.GetFieldCount();
            if (Card.Location == CardLocation.Hand) ourCount += 1;
            int enemyCount = Enemy.GetFieldCount();
            if (enemyCount <= ourCount) return false;

            // Best value: activate when our field is empty (opponent must keep 1 card)
            if (Bot.GetFieldCount() == 0 && Enemy.GetFieldCount() > 0)
            {
                _evenlyUsed = true;
                return true;
            }

            // Also activate if opponent has significantly more cards and we need to break board
            if (enemyCount >= ourCount + 3 && OpponentHasThreateningMonster())
            {
                _evenlyUsed = true;
                return true;
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

        // โ•โ•โ• Selections and Priorities โ•โ•โ•
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
            if (Card == null)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Ace card protection: avoid using face-up Ace cards as materials/tributes
            if (cards.Any(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone && IsAceCard(c)))
            {
                var safeCards = cards.Where(c => c == null || c.Controller != 0 || c.Location != CardLocation.MonsterZone || !IsAceCard(c)).ToList();
                if (safeCards.Count >= min)
                    return safeCards.Take(max).ToList();
            }

            // ── Card-Specific & Hint Routing ──

            if (Card.Id == CardId.ForbiddenDroplet)
            {
                // Target selection: negate opponent's threatening monsters
                if (cards.Any(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone))
                {
                    var oppThreats = cards.Where(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone && !c.IsDisabled())
                        .OrderByDescending(c =>
                            c.Id == 1561110 ? 100 : // ABC-Dragon Buster
                            c.Id == 10443957 ? 95 : // Cyber Dragon Infinity
                            c.Id == 4280258 ? 90 :  // Apollousa
                            c.Id == 84815190 ? 85 : // Baronne
                            c.IsFloodgate() ? 80 :
                            c.Attack
                        ).ToList();
                    if (oppThreats.Count >= min)
                        return oppThreats.Take(max).ToList();
                }

                // Cost selection: from our hand / field
                var costCandidates = new List<ClientCard>();
                // 1. Scapegoat tokens on field
                costCandidates.AddRange(cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && c.Id == 0));
                // 2. Used face-up Spells on field (Tenki after search)
                costCandidates.AddRange(cards.Where(c => c != null && c.Location == CardLocation.SpellZone && c.IsFaceup() && c.IsCode(CardId.FireFormationTenki)));
                // 3. Graceful Skull Dice in hand (beneficial in GY)
                costCandidates.AddRange(cards.Where(c => c != null && c.Location == CardLocation.Hand && c.IsCode(CardId.GracefulSkullDice)));
                // 4. Other non-Ace hand cards
                costCandidates.AddRange(cards.Where(c => c != null && c.Location == CardLocation.Hand && !IsAceCard(c) && !c.IsCode(CardId.ForbiddenDroplet)));
                // 5. Any safe non-Ace cards
                costCandidates.AddRange(cards.Where(c => c != null && !IsAceCard(c)));

                // CRITICAL FIX: Distinct to prevent duplicate card objects (which causes MSG_RETRY crash)
                var uniqueCosts = costCandidates.Distinct().ToList();
                int needed = Math.Min(max, Math.Max(min, 1));
                if (uniqueCosts.Count >= min)
                    return uniqueCosts.Take(needed).ToList();
            }

            if (Card.Id == CardId.SwiftPantherWarrior)
            {
                // Special Summon target from Deck/Hand
                if (hint == 509 || cards.All(c => c.Location == CardLocation.Deck || c.Location == CardLocation.Hand))
                {
                    int[] summonPriority = (Enemy.GetSpellCount() > 0 || Bot.HasInMonstersZone(CardId.AlligatorsSwordDragonKnight))
                        ? new[] { CardId.JinzoEnergyShocker, CardId.FishermanLegendOfTheSea, CardId.AlligatorsSwordDragonKnight }
                        : new[] { CardId.AlligatorsSwordDragonKnight, CardId.JinzoEnergyShocker, CardId.FishermanLegendOfTheSea };

                    foreach (int id in summonPriority)
                    {
                        var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                        if (match != null) return new List<ClientCard> { match };
                    }
                }

                // Tribute cost selection
                var fieldTokens = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && c.Id == 0).ToList();
                if (fieldTokens.Count >= min) return fieldTokens.Take(max).ToList();

                var fieldMonsters = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && c != Card && !IsAceCard(c)).OrderBy(c => c.Attack).ToList();
                if (fieldMonsters.Count >= min) return fieldMonsters.Take(max).ToList();

                var handMonsters = cards.Where(c => c != null && c.Location == CardLocation.Hand && !IsAceCard(c)).OrderBy(c => c.Attack).ToList();
                if (handMonsters.Count >= min) return handMonsters.Take(max).ToList();
            }

            if (Card.Id == CardId.AlligatorsSwordDragonKnight)
            {
                // 1. In Hand: Reveal 1 other card that mentions Dark Time Wizard
                if (Card.Location == CardLocation.Hand && (hint == 507 || cards.All(c => c.Location == CardLocation.Hand)))
                {
                    var revealTarget = cards.FirstOrDefault(c => c != null && c != Card && c.IsCode(DarkTimeWizardCards));
                    if (revealTarget != null) return new List<ClientCard> { revealTarget };
                }

                // 2. On Summon: Add up to 2 Spells/Traps that mention Dark Time Wizard with different names
                if (hint == 506 || cards.All(c => c.Location == CardLocation.Deck))
                {
                    var selected = new List<ClientCard>();
                    int[] priorityST = {
                        CardId.DarkTimeWizard,
                        CardId.SleepingScapegoats,
                        CardId.ReversalBox,
                        CardId.FoolishGraverobber,
                        CardId.GracefulSkullDice
                    };
                    foreach (int id in priorityST)
                    {
                        var match = cards.FirstOrDefault(c => c != null && c.Id == id && !selected.Any(s => s.Id == id));
                        if (match != null)
                        {
                            selected.Add(match);
                            if (selected.Count >= max) break;
                        }
                    }
                    if (selected.Count >= min) return selected;
                }

                // 3. Discard 1 card
                if (hint == 501 || hint == 504 || cards.All(c => c.Location == CardLocation.Hand))
                {
                    var handCards = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
                    if (handCards.Count > 0)
                    {
                        return handCards.OrderBy(c =>
                            c.Id == CardId.GracefulSkullDice ? 0 :
                            c.Id == CardId.FoolishGraverobber ? 1 :
                            handCards.Count(h => h.Id == c.Id) > 1 ? 2 :
                            c.IsCode(CardId.FireFormationTenki) ? 3 :
                            (c.IsSpell() || c.IsTrap()) ? 4 :
                            IsAceCard(c) ? 10 : 5
                        ).Take(max).ToList();
                    }
                }
            }

            if (Card.Id == CardId.FoolishGraverobber)
            {
                // GY banish effect: Set 1 DarkTime S/T from GY
                if (Card.Location == CardLocation.Grave)
                {
                    var ourST = cards.Where(c => c != null && c.Location == CardLocation.Grave && c.Controller == 0 && c.IsCode(DarkTimeWizardCards)).ToList();
                    var enemyST = cards.Where(c => c != null && c.Location == CardLocation.Grave && c.Controller == 1 && (c.IsSpell() || c.IsTrap())).ToList();

                    if (ourST.Count > 0)
                    {
                        return ourST.OrderByDescending(c => c.Id == CardId.SleepingScapegoats ? 3 : c.Id == CardId.ReversalBox ? 2 : 1).Take(max).ToList();
                    }
                    if (enemyST.Count > 0)
                    {
                        return enemyST.OrderByDescending(c => c.IsCode(CardId.CalledByTheGrave) ? 3 : c.IsCode(CardId.InfiniteImpermanence) ? 2 : 1).Take(max).ToList();
                    }
                }

                // Activation from Hand/Field:
                // Step 1: Send 1 DarkTime card from Deck to GY
                if (cards.All(c => c.Location == CardLocation.Deck))
                {
                    // Prefer sending Jinzo to immediately revive it
                    return cards.OrderBy(c =>
                        c.Id == CardId.JinzoEnergyShocker ? 0 :
                        c.Id == CardId.SwiftPantherWarrior ? 1 :
                        c.Id == CardId.FishermanLegendOfTheSea ? 2 :
                        c.Id == CardId.AlligatorsSwordDragonKnight ? 3 : 4
                    ).Take(max).ToList();
                }

                // Step 2: Special Summon 1 monster from either GY
                if (cards.All(c => c.Location == CardLocation.Grave))
                {
                    return cards.OrderBy(c =>
                        c.Id == CardId.JinzoEnergyShocker ? 0 :
                        c.Id == CardId.SwiftPantherWarrior ? 1 :
                        c.Id == CardId.FishermanLegendOfTheSea ? 2 :
                        c.Id == CardId.AlligatorsSwordDragonKnight ? 3 : 4
                    ).Take(max).ToList();
                }
            }

            if (Card.Id == CardId.SleepingScapegoats)
            {
                // Special Summon Swift Panther Warrior from Deck
                var panther = cards.FirstOrDefault(c => c != null && c.Id == CardId.SwiftPantherWarrior);
                if (panther != null) return new List<ClientCard> { panther };
            }

            if (Card.Id == CardId.FishermanLegendOfTheSea)
            {
                // Target 1 monster opp controls to destroy
                var oppMonsters = cards.Where(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone).ToList();
                if (oppMonsters.Count > 0)
                {
                    return oppMonsters.OrderByDescending(c => c.Attack).Take(max).ToList();
                }
            }

            if (Card.Id == CardId.GracefulSkullDice)
            {
                // Target 1 summoned monster to destroy
                var targets = cards.Where(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone).ToList();
                if (targets.Count > 0)
                {
                    return targets.OrderByDescending(c => c.Attack).Take(max).ToList();
                }
            }

            if (Card.Id == CardId.FydraulisHarmonia)
            {
                // Reveal up to 5 Synchros from Extra Deck
                if (cards.All(c => c != null && c.Location == CardLocation.Extra))
                {
                    var synchros = cards.Where(c => c.HasType(CardType.Synchro)).ToList();
                    return synchros.OrderByDescending(c =>
                        c.Id == CardId.GoldenCloudBeastMalong ? 3 :
                        c.Id == CardId.EnigmasterPackbit ? 2 :
                        c.Id == CardId.WindPegasusIgnister ? 1 : 0
                    ).Take(max).ToList();
                }

                // Send 1 revealed Synchro to GY (Malong bounces card; Packbit traps monster)
                if (hint == 504 || cards.Any(c => c.HasType(CardType.Synchro)))
                {
                    var target = cards.FirstOrDefault(c => c.Id == CardId.GoldenCloudBeastMalong);
                    if (target != null) return new List<ClientCard> { target };

                    target = cards.FirstOrDefault(c => c.Id == CardId.EnigmasterPackbit);
                    if (target != null) return new List<ClientCard> { target };

                    target = cards.FirstOrDefault(c => c.Id == CardId.WindPegasusIgnister);
                    if (target != null) return new List<ClientCard> { target };
                }

                // Destroy 1 monster opponent controls
                var oppTargets = cards.Where(c => c != null && c.Controller == 1 && c.Location == CardLocation.MonsterZone).ToList();
                if (oppTargets.Count > 0)
                {
                    return oppTargets.OrderByDescending(c => c.Attack).Take(max).ToList();
                }
            }

            if (Card.Id == CardId.DarkTimeWizard)
            {
                int[] priorityCards = {
                    CardId.SwiftPantherWarrior,
                    CardId.AlligatorsSwordDragonKnight,
                    CardId.JinzoEnergyShocker,
                    CardId.SleepingScapegoats,
                    CardId.ReversalBox,
                    CardId.FishermanLegendOfTheSea,
                    CardId.FoolishGraverobber,
                    CardId.GracefulSkullDice
                };
                foreach (int id in priorityCards)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                    if (match != null) return new List<ClientCard> { match };
                }
            }

            if (Card.Id == CardId.SuperPolymerization)
            {
                // Use hand monsters first (protect field Ace cards)
                if (cards.All(c => c != null && c.Location == CardLocation.Hand))
                {
                    return cards.OrderBy(c => c.Id == CardId.GracefulSkullDice ? 0 : c.IsMonster() ? 2 : 1).Take(max).ToList();
                }

                var fieldMonsters = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && !IsAceCard(c)).ToList();
                if (fieldMonsters.Count > 0)
                {
                    return fieldMonsters.OrderByDescending(c => c.Controller == 1 ? 1 : 0).Take(max).ToList();
                }
            }

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

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0) return 0;

            // Swift Panther Warrior: Option 0 = Special Summon DarkTime monster from Deck/Hand
            if (Card != null && Card.Id == CardId.SwiftPantherWarrior)
            {
                return 0;
            }

            // Dark Time Wizard: Option 0 = Search 1 DarkTime card + Recycle at End Phase (Always guaranteed +2)
            if (Card != null && Card.Id == CardId.DarkTimeWizard)
            {
                return 0;
            }

            if (Card != null && Card.Id == CardId.FydraulisHarmonia && options.Count > 1)
            {
                return 0;
            }

            if (Card != null && Card.Id == CardId.SolemnReport && options.Count > 1)
            {
                if (Bot.LifePoints > 4000) return 1; // Pay 3000 to banish copies
                return 0; // Pay 1500
            }

            return 0;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(CardId.PsychicEndPunisher) ||
                   card.IsCode(CardId.FavoriteHeroFlameWingman) ||
                   card.IsCode(CardId.JinzoEnergyShocker) ||
                   card.IsCode(CardId.FishermanLegendOfTheSea) ||
                   card.IsCode(CardId.EnigmasterPackbit) ||
                   card.IsCode(CardId.WindPegasusIgnister) ||
                   card.IsCode(CardId.SwiftPantherWarrior) ||
                   card.IsCode(CardId.AlligatorsSwordDragonKnight) ||
                   card.IsCode(CardId.FydraulisHarmonia);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.AshBlossom, CardId.GhostBelle, CardId.MulcharmyFuwalos, CardId.MulcharmyPurulia))
                return 800;
            return 100;
        }
    }
}
