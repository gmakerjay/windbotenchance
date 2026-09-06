using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ============================================================
    // CARD AUDIT โ€” 2026_Runick (Runick Stun)
    // ============================================================
    // | Card Name              | Type    | OPT? | Cost            | Effect Summary                                | Activate When                     | NEVER Activate When                    |
    // |------------------------|---------|------|-----------------|-----------------------------------------------|-----------------------------------|----------------------------------------|
    // | Inspector Boarder      | Monster | No   | None            | Disable monster effects activation            | Normal Summoned                   | Controls monsters                      |
    // | Thunder King Rai-Oh    | Monster | No   | Send to GY      | Negate Special Summon, prevent deck adding    | Normal Summoned                   | Controls monsters / Need to search     |
    // | Runick Fountain        | Spell   | Yes  | Return GY spells| Place up to 3 GY spells at deck bottom, draw  | On field after activation         | Already face-up                        |
    // | Runick Tip             | Spell   | Yes  | None            | Search Runick card + banish 1 opponent deck   | Play                              | Rai-Oh active                          |
    // | Runick Freezing Curses | Spell   | Yes  | None            | Negate monster effect + banish 3              | Opponent controls effect monster  | No opponent monster                    |
    // | Runick Destruction     | Spell   | Yes  | None            | Destroy Spell/Trap + banish 4                 | Opponent controls Spell/Trap      | No opponent Spell/Trap                 |
    // | Runick Flashing Fire   | Spell   | Yes  | None            | Destroy Special Summoned monster + banish 2   | Opponent controls SS monster      | No opponent SS monster                 |
    // | Runick Slumber         | Spell   | Yes  | None            | Protect monster + banish 3                    | Targetable monster on field       | No monster                             |
    // | Runick Golden Droplet  | Spell   | Yes  | None            | Opponent draws 1, banish 4                    | Fountain active / decking out     | Opponent deck < 4                      |
    // | Runick Smiting Storm   | Spell   | Yes  | None            | Banish up to cards opponent controls          | Opponent controls cards           | Opponent controls 0 cards              |
    // | Radiant Typhoon Vision | Spell   | Yes  | None            | Draw 2, discard 1 / Search MST                | Play                              | No discard target                      |
    // | Mystical Space Typhoon | Spell   | No   | None            | Destroy Spell/Trap                            | Opponent controls S/T             | No S/T                                 |
    // | Super Polymerization   | Spell   | No   | Discard 1       | Fusion Summon using field materials           | Opponent controls fusion materials| No materials                           |
    // | Dimensional Fissure    | Spell   | No   | None            | Banishes monsters sent to GY                  | Play                              | Already active                         |
    // | Messenger of Peace     | Spell   | No   | 100 LP          | Stall 1500+ ATK attacks                       | Play                              | Already active                         |
    // | Card Scanner           | Spell   | Yes  | None            | Guess bottom deck card                        | Play                              | Already active                         |
    // | Pot of Duality         | Spell   | Yes  | None            | Dig 3, select 1                               | End of turn, no more SS needed    | Need to SS this turn                   |
    // | There Can Be Only One  | Trap    | No   | None            | Limit to 1 monster per Type                   | Set / Activate                    | Already active                         |
    // | Rivalry of Warlords    | Trap    | No   | None            | Limit to 1 monster Type                       | Set / Activate                    | Already active                         |
    // | Skill Drain            | Trap    | No   | 1000 LP         | Negate monster effects on field               | Set / Activate                    | Already active                         |
    // ============================================================
    // ACE CARDS: Primary: Runick Fountain (92107604) / Secondary: Hugin the Runick Wings (55990317)
    // COMBO STARTERS: 1. Runick Tip (31562086) 2. Runick Fountain (92107604) 3. Hugin the Runick Wings (55990317)
    // CHOKEPOINTS: Hugin search negated, or Fountain popped when no other spells in hand.
    // WIN CONDITION: Deck out the opponent via Runick spell banish effects while stalling with stun cards.
    // ============================================================

    // ============================================================
    // COMBO DRAFT โ€” 2026_Runick
    // ============================================================
    // === COMBO LINE 1: Get Fountain Active (Going 1st) ===
    // HAND REQUIRED: Any Runick Spell + 1 card to discard
    // STEP 1: Activate Runick Spell to Special Summon Hugin the Runick Wings (Option 1)
    // STEP 2: Hugin trigger effect: discard 1 card, search Runick Fountain
    // STEP 3: Activate Runick Fountain from hand
    // END BOARD: Runick Fountain active + Hugin on field
    //
    // === COMBO LINE 2: Disruption & Recycling ===
    // HAND REQUIRED: Runick Spells in hand + Runick Fountain active
    // STEP 1: Wait for opponent to play cards
    // STEP 2: Activate Runick Spells from hand (Option 0) to negate/destroy/disrupt their cards
    // STEP 3: Runick Fountain trigger effect: target up to 3 Runick spells in GY, return to bottom of deck, draw cards!
    //
    // === COMBO LINE 3: Super Poly Board Break ===
    // HAND REQUIRED: Super Polymerization + 1 discard
    // STEP 1: Evaluate opponent's field for Fusion materials (Garura / Mudragon)
    // STEP 2: Activate Super Poly, discarding 1, Fusion Summon Garura or Mudragon using opponent's monsters.
    // ============================================================

    [Deck("2026_Runick", "2026_Runick")]
    public class _2026_RunickExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int InspectorBoarder = 15397015;
            public const int ThunderKingRaiOh = 71564252;

            // Spells
            public const int RunickFountain = 92107604;
            public const int RunickDestruction = 94445733;
            public const int RunickFlashingFire = 68957034;
            public const int RunickFreezingCurses = 30430448;
            public const int RunickGoldenDroplet = 20618850;
            public const int RunickSlumber = 67835547;
            public const int RunickSmitingStorm = 93229151;
            public const int RunickTip = 31562086;
            public const int RadiantTyphoonVision = 20508881;
            public const int MysticalSpaceTyphoon = 5318639;
            public const int SuperPolymerization = 48130397;
            public const int DimensionalFissure = 81674782;
            public const int MessengerOfPeace = 44656491;
            public const int CardScanner = 77066768;
            public const int PotOfDuality = 98645731;

            // Traps
            public const int ThereCanBeOnlyOne = 24207889;
            public const int RivalryOfWarlords = 90846359;
            public const int SkillDrain = 82732705;

            // Extra Deck
            public const int HuginTheRunickWings = 55990317;
            public const int MuninTheRunickWings = 92385016;
            public const int GeriTheRunickFangs = 28373620;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int MudragonOfTheSwamp = 54757758;
            public const int PredaplantDragostapelia = 69946549;
            public const int StarvingVenomFusionDragon = 41209827;
            public const int SuperStarslayerTyphon = 93039339;
            public const int Number41Bagooska = 90590303;
            public const int SpLittleKnight = 29301450;

            // Side Deck (if encountered/drawn)
            public const int LavaGolem = 102380;
            public const int Pankratops = 82385847;
            public const int DenkoSekka = 13974207;
            public const int CosmicCyclone = 8267140;
            public const int GravekeepersInscription = 59494222;
            public const int EvenlyMatched = 15693423;
            public const int SolemnJudgment = 41420027;
            public const int AntiSpellFragrance = 58921041;
        }

        private static readonly int[] RunickSpells = new[]
        {
            CardId.RunickTip, CardId.RunickFreezingCurses, CardId.RunickDestruction,
            CardId.RunickFlashingFire, CardId.RunickSlumber, CardId.RunickGoldenDroplet,
            CardId.RunickSmitingStorm
        };

        private static readonly int[] StunMonsters = new[]
        {
            CardId.InspectorBoarder, CardId.ThunderKingRaiOh
        };

        private static readonly int[] BossMonsters = new[]
        {
            CardId.HuginTheRunickWings, CardId.MuninTheRunickWings, CardId.GeriTheRunickFangs,
            CardId.Number41Bagooska, CardId.SuperStarslayerTyphon, CardId.SpLittleKnight
        };

        // OPT Tracking for Runick spells (all HOPT)
        private bool _tipUsed = false;
        private bool _freezingUsed = false;
        private bool _destructionUsed = false;
        private bool _flashingFireUsed = false;
        private bool _slumberUsed = false;
        private bool _goldenDropletUsed = false;
        private bool _smitingUsed = false;
        private bool _fountainDrawUsed = false;
        private int _runickSpellsUsedThisTurn = 0;

        /// <summary>Estimated opponent deck size tracking (starts at 40, decreases with banish).</summary>
        private int _estimatedOpponentDeckSize = 40;

        public _2026_RunickExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(BossMonsters);
            ResourcePlan.RegisterAceCards(BossMonsters);

            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Get-Fountain",
                RequiredCards = new List<int> { CardId.RunickTip },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.RunickTip, ActionType = ExecutorType.Activate, Description = "SS Hugin with Runick Tip" },
                    new() { CardId = CardId.HuginTheRunickWings, ActionType = ExecutorType.Activate, Description = "Hugin discard and search Fountain" },
                    new() { CardId = CardId.RunickFountain, ActionType = ExecutorType.Activate, Description = "Activate Runick Fountain" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Disruption-And-Recycle",
                RequiredCards = new List<int> { CardId.RunickFountain },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.RunickFreezingCurses, ActionType = ExecutorType.Activate, Description = "Negate opponent monster + banish" },
                    new() { CardId = CardId.RunickFountain, ActionType = ExecutorType.Activate, Description = "Fountain recycles spells + draw" }
                },
                EndBoardScore = 85
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.RunickTip);
            BaitPlanner.RegisterBaitCards(CardId.PotOfDuality);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.RunickFountain, CardId.HuginTheRunickWings);

            // โ•โ•โ• TIER 1: Staples / Hand Traps / Board Breakers โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedActivate);
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentActivate);
            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, CosmicCycloneActivate);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationActivate);

            // โ•โ•โ• TIER 2: Runick Spells & Custom Spells โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.RunickTip, RunickTipActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickFreezingCurses, RunickFreezingCursesActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickDestruction, RunickDestructionActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickFlashingFire, RunickFlashingFireActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickSlumber, RunickSlumberActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickGoldenDroplet, RunickGoldenDropletActivate);
            AddExecutor(ExecutorType.Activate, CardId.RunickSmitingStorm, RunickSmitingStormActivate);
            AddExecutor(ExecutorType.Activate, CardId.RadiantTyphoonVision, RadiantTyphoonVisionActivate);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, MysticalSpaceTyphoonActivate);

            // โ•โ•โ• TIER 3: Setup Spells & Draw Spells โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.RunickFountain, RunickFountainActivate);
            AddExecutor(ExecutorType.Activate, CardId.PotOfDuality, PotOfDualityActivate);

            // โ•โ•โ• TIER 4: Stun / Continuous Spells/Traps โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.DimensionalFissure, DimensionalFissureActivate);
            AddExecutor(ExecutorType.Activate, CardId.MessengerOfPeace, MessengerOfPeaceActivate);
            AddExecutor(ExecutorType.Activate, CardId.CardScanner);
            AddExecutor(ExecutorType.Activate, CardId.ThereCanBeOnlyOne, ThereCanBeOnlyOneActivate);
            AddExecutor(ExecutorType.Activate, CardId.RivalryOfWarlords, RivalryActivate);
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainActivate);
            AddExecutor(ExecutorType.Activate, CardId.AntiSpellFragrance, AntiSpellActivate);

            // โ•โ•โ• TIER 5: Monsters (Summons & Effects) โ•โ•โ•
            AddExecutor(ExecutorType.Summon, CardId.InspectorBoarder, InspectorBoarderSummon);
            AddExecutor(ExecutorType.Summon, CardId.ThunderKingRaiOh, ThunderKingRaiOhSummon);
            AddExecutor(ExecutorType.Summon, CardId.DenkoSekka, DenkoSekkaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LavaGolem, LavaGolemSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Pankratops, PankratopsSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Pankratops, PankratopsActivate);
            AddExecutor(ExecutorType.Activate, CardId.GravekeepersInscription);

            // โ•โ•โ• TIER 6: Extra Deck Fusions & Xyz Summons โ•โ•โ•
            AddExecutor(ExecutorType.SpSummon, CardId.HuginTheRunickWings, HuginSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.HuginTheRunickWings, HuginActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.MuninTheRunickWings, MuninSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.MuninTheRunickWings, MuninActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.GeriTheRunickFangs, GeriSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.GeriTheRunickFangs, GeriActivate);

            AddExecutor(ExecutorType.SpSummon, CardId.SpLittleKnight, LinkSummonCheck);
            AddExecutor(ExecutorType.Activate, CardId.SpLittleKnight);
            AddExecutor(ExecutorType.SpSummon, CardId.Number41Bagooska, XyzSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTyphon, XyzSummonCheck);

            // โ•โ•โ• TIER 7: Spell/Trap Sets & Repos โ•โ•โ•
            AddExecutor(ExecutorType.SpellSet, SpellSetCheck);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _tipUsed = false;
            _freezingUsed = false;
            _destructionUsed = false;
            _flashingFireUsed = false;
            _slumberUsed = false;
            _goldenDropletUsed = false;
            _smitingUsed = false;
            _fountainDrawUsed = false;
            _runickSpellsUsedThisTurn = 0;
            // Only reset opponent deck size at start of duel, not every turn
            if (Duel.Turn <= 1)
                _estimatedOpponentDeckSize = 40;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        public override bool OnSelectHand()
        {
            // Runick Stun prefers going first to establish floodgates
            return true;
        }

        private static bool IsRunickSpell(int id)
        {
            return RunickSpells.Contains(id);
        }

        private bool IsSearchBlocked()
        {
            // Rai-Oh blocks adding cards from deck to hand (except by drawing)
            bool raiOhActive = Bot.GetMonsters().Concat(Enemy.GetMonsters())
                .Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.IsCode(CardId.ThunderKingRaiOh));
            return raiOhActive;
        }

        /// <summary>Should we hold Runick spells for opponent's turn?</summary>
        private bool ShouldHoldRunickSpell()
        {
            // If it's opponent's turn, always use spells (disruption)
            if (Duel.Player == 1) return false;
            // On Turn 1 going first, never hold โ€” need to find and activate Fountain ASAP
            if (Duel.Turn <= 1) return false;
            // If Fountain is active and we're going first, hold spells for opponent's turn
            bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain);
            if (hasFountain && _runickSpellsUsedThisTurn >= 2) return true;
            // If no Fountain yet, allow using spells to find it
            if (!hasFountain) return false;
            // If we have 3+ Runick spells in hand, hold some for opponent's turn
            int runickInHand = Bot.Hand.Count(c => c != null && IsRunickSpell(c.Id));
            if (runickInHand >= 3 && _runickSpellsUsedThisTurn >= 1) return true;
            return false;
        }

        /// <summary>Is opponent close to decking out?</summary>
        private bool IsOpponentNearDeckOut()
        {
            return _estimatedOpponentDeckSize <= 10;
        }

        /// <summary>Track banish from Runick spell activation.</summary>
        private void TrackBanish(int banishCount)
        {
            _estimatedOpponentDeckSize = Math.Max(0, _estimatedOpponentDeckSize - banishCount);
        }

        private bool IsSkillDrainActive()
        {
            return Bot.GetSpells().Concat(Enemy.GetSpells())
                .Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.IsCode(CardId.SkillDrain));
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 1. OnSelectOption Override (Intelligent choice selection)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private long StringId(int cardId, int optionIndex)
        {
            return (optionIndex & 0xfffffL) | ((long)cardId << 20);
        }

        private int ScoreOption(int cardId, int optIndex)
        {
            if (optIndex == 1) // Special Summon
            {
                // Prioritize summoning Hugin to get Fountain if not active
                bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain) || Bot.HasInHand(CardId.RunickFountain);
                if (!hasFountain && GetRemainingCount(CardId.HuginTheRunickWings) > 0 && !IsSearchBlocked() && !IsSkillDrainActive())
                {
                    return 25; // Highest priority to search Fountain!
                }

                // If Fountain is in GY and not on field/hand, summon Geri to retrieve it!
                bool fountainInGY = Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.RunickFountain));
                if (!hasFountain && fountainInGY && GetRemainingCount(CardId.GeriTheRunickFangs) > 0)
                {
                    return 22; // Summon Geri to retrieve Fountain
                }

                // Don't summon multiple Runick fusions if we already have one on field
                if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.HuginTheRunickWings) || c.IsCode(CardId.GeriTheRunickFangs) || c.IsCode(CardId.MuninTheRunickWings))))
                {
                    return 0;
                }

                // Block attacks if zone is empty and opponent's turn
                if (Duel.Player == 1 && Bot.GetMonsterCount() == 0)
                {
                    return 12;
                }

                // Near deck-out: deprioritize SS, focus on banish
                if (IsOpponentNearDeckOut())
                {
                    return 2;
                }

                return 4;
            }
            else // Spell effect (Option 0)
            {
                // Near deck-out: ALL banish effects become highest priority
                bool nearDeckOut = IsOpponentNearDeckOut();

                if (cardId == CardId.RunickTip)
                {
                    if (IsSearchBlocked()) return 0;
                    return nearDeckOut ? 14 : 10; // Tip banishes 1 + searches
                }
                if (cardId == CardId.RunickDestruction)
                {
                    bool hasTarget = Enemy.GetSpells().Any(c => c != null);
                    if (!hasTarget && !nearDeckOut) return 0;
                    // Higher score for Continuous/Field spells
                    bool hasHighValueTarget = Enemy.GetSpells().Any(c => c != null && c.IsFaceup() &&
                        (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)));
                    int baseScore = hasHighValueTarget ? 12 : (hasTarget ? 8 : 0);
                    return nearDeckOut ? Math.Max(baseScore, 13) : baseScore; // Banishes 4
                }
                if (cardId == CardId.RunickFlashingFire)
                {
                    bool hasTarget = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsSpecialSummoned);
                    if (!hasTarget && !nearDeckOut) return 0;
                    // Higher score for boss monsters
                    bool hasBoss = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsSpecialSummoned && c.Attack >= 2500);
                    int baseScore = hasBoss ? 11 : (hasTarget ? 8 : 0);
                    return nearDeckOut ? Math.Max(baseScore, 11) : baseScore; // Banishes 2
                }
                if (cardId == CardId.RunickFreezingCurses)
                {
                    bool hasTarget = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !c.IsDisabled());
                    if (!hasTarget && !nearDeckOut) return 0;
                    // Negate + banish 3 โ€” very valuable vs effect monsters
                    int baseScore = hasTarget ? 10 : 0;
                    return nearDeckOut ? Math.Max(baseScore, 12) : baseScore; // Banishes 3
                }
                if (cardId == CardId.RunickSlumber)
                {
                    bool hasTarget = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Any(c => c != null && c.IsFaceup());
                    if (!hasTarget) return 0;
                    return nearDeckOut ? 11 : 7; // Banishes 3, but protection-focused
                }
                if (cardId == CardId.RunickGoldenDroplet)
                {
                    // Cautious: opponent draws 1, so only use when deck-out is near or Fountain active
                    if (nearDeckOut) return 15; // Maximum priority โ€” banishes 4, game-ending
                    if (!Bot.HasInSpellZone(CardId.RunickFountain)) return 0; // No Fountain = no value
                    return 5; // Low priority โ€” opponent gets a free draw
                }
                if (cardId == CardId.RunickSmitingStorm)
                {
                    bool hasTarget = Enemy.GetFieldCount() > 0;
                    if (!hasTarget) return 0;
                    int enemyCards = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
                    return nearDeckOut ? 13 : (enemyCards >= 3 ? 9 : 7); // Banishes up to N
                }
            }
            return 1;
        }

        public override int OnSelectOption(IList<long> options)
        {
            int bestIndex = 0;
            int bestScore = -1;

            var active = Card ?? LastChainCard;
            int fallbackCardId = active?.Id ?? 0;

            for (int i = 0; i < options.Count; ++i)
            {
                long option = options[i];
                int cardId = (int)(option >> 4);
                int optIndex = (int)(option & 0xf);

                if (!IsRunickSpell(cardId) && cardId != CardId.RadiantTyphoonVision)
                {
                    if (IsRunickSpell(fallbackCardId))
                    {
                        cardId = fallbackCardId;
                        optIndex = i;
                    }
                }

                int score = 0;
                if (IsRunickSpell(cardId))
                {
                    score = ScoreOption(cardId, optIndex);
                }
                else if (cardId == CardId.RadiantTyphoonVision)
                {
                    // Option 0: Draw 2, discard 1. Option 1: add MST.
                    score = (optIndex == 0) ? 8 : 6;
                }
                else
                {
                    score = 1;
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestIndex = i;
                }
            }

            return bestIndex;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 2. OnSelectCard Override (Fountain recycling & disacrds)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private IList<ClientCard> GetDiscardCandidates(IList<ClientCard> cards)
        {
            return cards.OrderBy(c => {
                if (c == null) return 999;
                if (c.IsCode(CardId.RunickFountain)) return 1000;
                
                int dupCount = Bot.Hand.Count(h => h != null && h.Id == c.Id);
                if (dupCount > 1) return 1; // Discard duplicate first
                
                if (c.IsCode(CardId.RadiantTyphoonVision)) return 5;
                if (c.IsCode(CardId.MysticalSpaceTyphoon)) return 10;
                if (c.HasType(CardType.QuickPlay) && IsRunickSpell(c.Id))
                {
                    // If we have 3+ Runick spells, discarding one is fine to get Fountain
                    int runickCount = Bot.Hand.Count(h => h != null && IsRunickSpell(h.Id));
                    return runickCount >= 3 ? 15 : 40;
                }
                if (StunMonsters.Contains(c.Id)) return 80;
                
                return 50;
            }).ToList();
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Fountain target recycling
            if (Card != null && Card.IsCode(CardId.RunickFountain))
            {
                var runickSpells = cards.Where(c => IsRunickSpell(c.Id)).ToList();
                if (runickSpells.Count > 0)
                {
                    return runickSpells.Take(max).ToList();
                }
            }

            // Hugin discard cost or general discard
            if (hint == 501)
            {
                var candidates = GetDiscardCandidates(cards);
                if (candidates.Count >= min)
                {
                    return candidates.Take(max).ToList();
                }
            }

            // Geri target retrieve
            if (Card != null && Card.IsCode(CardId.GeriTheRunickFangs))
            {
                var fountain = cards.FirstOrDefault(c => c.IsCode(CardId.RunickFountain));
                if (fountain != null)
                {
                    return new[] { fountain };
                }
            }

            // Protect valuable monsters from being used as material if possible
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533 || hint == 508 || hint == 504)
            {
                var sorted = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (IsValuableMonster(c)) return 100;
                    return 0;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // Extra Deck Summon: prefer Hugin
            if (cards.Any(c => c != null && c.Location == CardLocation.Extra))
            {
                var hugin = cards.FirstOrDefault(c => c.Id == CardId.HuginTheRunickWings);
                if (hugin != null && !Bot.HasInMonstersZone(CardId.HuginTheRunickWings))
                    return new List<ClientCard> { hugin };
                var geri = cards.FirstOrDefault(c => c.Id == CardId.GeriTheRunickFangs);
                if (geri != null && !Bot.HasInMonstersZone(CardId.GeriTheRunickFangs))
                    return new List<ClientCard> { geri };
                var munin = cards.FirstOrDefault(c => c.Id == CardId.MuninTheRunickWings);
                if (munin != null)
                    return new List<ClientCard> { munin };
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 3. Tier 1 Activators
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool EvenlyMatchedActivate()
        {
            // Only worth it if opponent has 2+ cards
            int enemyFieldCount = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            if (enemyFieldCount < 2) return false;
            return Bot.GetFieldCount() == 0 && Duel.Phase == DuelPhase.Battle;
        }

        private bool SolemnJudgmentActivate()
        {
            // Pay half LP to negate summon/S/T activation
            return Bot.LifePoints > 1000;
        }

        private bool CosmicCycloneActivate()
        {
            if (EnemyHasKnownNegate()) return false;
            var target = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && 
                (c.HasType(CardType.Continuous) || c.HasType(CardType.Field) || c.HasType(CardType.Equip)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SuperPolymerizationActivate()
        {
            if (Bot.Hand.Count == 0 && Card.Location == CardLocation.Hand) return false;

            var allMonsters = Bot.GetMonsters().Concat(Enemy.GetMonsters())
                .Where(c => c != null && c.IsFaceup()).ToList();

            if (allMonsters.Count < 2) return false;

            // Check Mudragon
            bool canMudragon = false;
            for (int i = 0; i < allMonsters.Count; ++i)
            {
                for (int j = i + 1; j < allMonsters.Count; ++j)
                {
                    var m1 = allMonsters[i];
                    var m2 = allMonsters[j];
                    if (m1.Attribute == m2.Attribute && m1.Race != m2.Race)
                    {
                        canMudragon = true;
                        break;
                    }
                }
                if (canMudragon) break;
            }
            if (canMudragon && GetRemainingCount(CardId.MudragonOfTheSwamp) > 0) return true;

            // Check Garura
            bool canGarura = false;
            for (int i = 0; i < allMonsters.Count; ++i)
            {
                for (int j = i + 1; j < allMonsters.Count; ++j)
                {
                    var m1 = allMonsters[i];
                    var m2 = allMonsters[j];
                    if (m1.Attribute == m2.Attribute && m1.Race == m2.Race && m1.Id != m2.Id)
                    {
                        canGarura = true;
                        break;
                    }
                }
                if (canGarura) break;
            }
            if (canGarura && GetRemainingCount(CardId.GaruraWingsOfResonantLife) > 0) return true;

            // Check Starving Venom
            bool canStarving = false;
            int darkCount = allMonsters.Count(c => c.HasAttribute(CardAttribute.Dark) && !c.HasType(CardType.Token));
            if (darkCount >= 2) canStarving = true;
            if (canStarving && GetRemainingCount(CardId.StarvingVenomFusionDragon) > 0) return true;

            // Check Dragostapelia
            bool canDrago = allMonsters.Any(c => c.HasType(CardType.Fusion)) && allMonsters.Any(c => c.HasAttribute(CardAttribute.Dark));
            if (canDrago && GetRemainingCount(CardId.PredaplantDragostapelia) > 0) return true;

            return false;
        }

        // ─────────────────────────────────────────────────────────────
        //  § 4. Tier 2 Activators (Runick & Custom Spells)
        // ─────────────────────────────────────────────────────────────

        private bool NeedsFountainSearch()
        {
            bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain) || Bot.HasInHand(CardId.RunickFountain);
            if (hasFountain) return false;
            if (IsSpecialSummonBlocked() || IsSearchBlocked() || IsSkillDrainActive()) return false;
            // Check if monster zone available
            if (Bot.GetMonsterCount() >= 5) return false;
            // Check if we don't already have a Runick fusion monster on field
            bool hasRunickFusion = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.IsCode(CardId.HuginTheRunickWings) || c.IsCode(CardId.GeriTheRunickFangs) || c.IsCode(CardId.MuninTheRunickWings)));
            if (hasRunickFusion) return false;
            return GetRemainingCount(CardId.HuginTheRunickWings) > 0;
        }

        private bool CanSpecialSummonHugin()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.GetMonsterCount() >= 5) return false;
            return !Bot.HasInMonstersZone(CardId.HuginTheRunickWings) && GetRemainingCount(CardId.HuginTheRunickWings) > 0;
        }

        private bool IsRunickQuickPlay(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.RunickTip ||
                   card.Id == CardId.RunickFreezingCurses ||
                   card.Id == CardId.RunickDestruction ||
                   card.Id == CardId.RunickFlashingFire ||
                   card.Id == CardId.RunickSlumber ||
                   card.Id == CardId.RunickGoldenDroplet ||
                   card.Id == CardId.RunickSmitingStorm;
        }

        private bool RunickTipActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (_tipUsed) return false;
            if (EnemyHasKnownNegate()) return false;
            if (NeedsFountainSearch())
            {
                _tipUsed = true;
                _runickSpellsUsedThisTurn++;
                return true;
            }
            if (ShouldHoldRunickSpell()) return false;
            _tipUsed = true;
            _runickSpellsUsedThisTurn++;
            TrackBanish(1);
            return true;
        }

        private bool RunickFreezingCursesActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (_freezingUsed) return false;
            if (EnemyHasKnownNegate()) return false;
            if (NeedsFountainSearch())
            {
                _freezingUsed = true;
                _runickSpellsUsedThisTurn++;
                return true;
            }
            // Only useful if opponent controls an effect monster to negate
            bool hasEffectMonster = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !c.IsDisabled());
            // On opponent's turn: always use if they have effect monster (disruption)
            if (Duel.Player == 1 && hasEffectMonster)
            {
                _freezingUsed = true;
                _runickSpellsUsedThisTurn++;
                TrackBanish(3);
                return true;
            }
            // On our turn: hold unless we have no monster and need the SS, or near deck-out
            if (!hasEffectMonster && Bot.GetMonsterCount() > 0 && !IsOpponentNearDeckOut()) return false;
            if (ShouldHoldRunickSpell() && !IsOpponentNearDeckOut()) return false;
            _freezingUsed = true;
            _runickSpellsUsedThisTurn++;
            TrackBanish(3);
            return true;
        }

        private bool RunickDestructionActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (_destructionUsed) return false;
            if (EnemyHasKnownNegate()) return false;
            if (NeedsFountainSearch())
            {
                _destructionUsed = true;
                _runickSpellsUsedThisTurn++;
                return true;
            }
            // Need an S/T on opponent field to destroy
            bool hasTarget = Enemy.GetSpells().Any(c => c != null);
            // Without a target, only proceed if near deck-out (banish 4 is game-ending)
            if (!hasTarget && !IsOpponentNearDeckOut()) return false;
            if (ShouldHoldRunickSpell() && !hasTarget && !IsOpponentNearDeckOut()) return false;
            _destructionUsed = true;
            _runickSpellsUsedThisTurn++;
            TrackBanish(4);
            if (hasTarget)
            {
                // Target the most impactful S/T
                var target = Enemy.GetSpells()
                    .Where(c => c != null && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => {
                        if (c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) return 100;
                        if (c.IsFacedown()) return 50;
                        return 10;
                    })
                    .FirstOrDefault();
                if (target != null) AI.SelectCard(target);
            }
            return true;
        }

        private bool RunickFlashingFireActivate()
        {
            if (_flashingFireUsed) return false;
            if (EnemyHasKnownNegate()) return false;
            if (NeedsFountainSearch())
            {
                _flashingFireUsed = true;
                _runickSpellsUsedThisTurn++;
                return true;
            }
            // Need a Special Summoned monster on opponent's field
            bool hasTarget = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsSpecialSummoned);
            // Without a target, only proceed if near deck-out (banish 2 still valuable)
            if (!hasTarget && !IsOpponentNearDeckOut()) return false;
            if (ShouldHoldRunickSpell() && !hasTarget && !IsOpponentNearDeckOut()) return false;
            _flashingFireUsed = true;
            _runickSpellsUsedThisTurn++;
            TrackBanish(2);
            if (hasTarget)
            {
                var target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsSpecialSummoned && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (target != null) AI.SelectCard(target);
            }
            return true;
        }

        private bool RunickSlumberActivate()
        {
            if (_slumberUsed) return false;
            if (EnemyHasKnownNegate()) return false;
            if (NeedsFountainSearch())
            {
                _slumberUsed = true;
                _runickSpellsUsedThisTurn++;
                return true;
            }
            // Need a face-up monster on either field to protect
            bool hasOurMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup());
            bool hasEnemyMonster = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup());
            if (!hasOurMonster && !hasEnemyMonster) return false;
            // Best used on opponent's turn to protect our monster from attack/removal
            if (Duel.Player == 0 && !IsOpponentNearDeckOut() && ShouldHoldRunickSpell()) return false;
            _slumberUsed = true;
            _runickSpellsUsedThisTurn++;
            TrackBanish(3);
            // Prefer protecting our own boss monsters
            var protectTarget = Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup())
                .OrderByDescending(c => {
                    if (c.IsCode(CardId.HuginTheRunickWings, CardId.MuninTheRunickWings, CardId.GeriTheRunickFangs)) return c.Attack + 5000;
                    if (c.IsCode(CardId.InspectorBoarder, CardId.ThunderKingRaiOh)) return c.Attack + 3000;
                    return c.Attack;
                })
                .FirstOrDefault();
            if (protectTarget != null) AI.SelectCard(protectTarget);
            return true;
        }

        private bool RunickGoldenDropletActivate()
        {
            if (_goldenDropletUsed) return false;
            if (EnemyHasKnownNegate()) return false;
            if (NeedsFountainSearch())
            {
                _goldenDropletUsed = true;
                _runickSpellsUsedThisTurn++;
                return true;
            }

            // CAUTION: Golden Droplet lets opponent draw 1 card!
            // Only use when the banish-4 is worth more than opponent's free draw

            // Near deck-out: ALWAYS use — game-ending banish 4
            if (IsOpponentNearDeckOut())
            {
                _goldenDropletUsed = true;
                _runickSpellsUsedThisTurn++;
                TrackBanish(4);
                return true;
            }

            // Only play if Fountain is active (for draw value to offset opponent's draw)
            if (!Bot.HasInSpellZone(CardId.RunickFountain)) return false;

            // Don't use if opponent has a very small deck (< 8) but not near enough to deck out
            // (giving them draw with small deck risk = dangerous)
            if (_estimatedOpponentDeckSize < 8 && _estimatedOpponentDeckSize > 4) return false;

            // Don't use early game when opponent has lots of cards (free draw too costly)
            if (Duel.Turn <= 4 && _estimatedOpponentDeckSize > 25) return false;

            _goldenDropletUsed = true;
            _runickSpellsUsedThisTurn++;
            TrackBanish(4);
            return true;
        }

        private bool RunickSmitingStormActivate()
        {
            if (_smitingUsed) return false;
            if (EnemyHasKnownNegate()) return false;
            if (NeedsFountainSearch())
            {
                _smitingUsed = true;
                _runickSpellsUsedThisTurn++;
                return true;
            }
            // Need cards on opponent's field to banish
            if (Enemy.GetFieldCount() == 0 && Bot.GetMonsterCount() > 0 && !IsOpponentNearDeckOut()) return false;
            if (ShouldHoldRunickSpell() && Enemy.GetFieldCount() < 2 && !IsOpponentNearDeckOut()) return false;
            _smitingUsed = true;
            _runickSpellsUsedThisTurn++;
            return true;
        }

        private bool RadiantTyphoonVisionActivate()
        {
            if (EnemyHasKnownNegate()) return false;
            // Draw 2 requires hand presence or quick plays
            bool hasQuickPlayInHand = Bot.Hand.Any(c => c != null && c != Card && c.IsSpell() && c.HasType(CardType.QuickPlay));
            return hasQuickPlayInHand || Bot.Hand.Count <= 2;
        }

        private bool MysticalSpaceTyphoonActivate()
        {
            var target = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && 
                (c.HasType(CardType.Continuous) || c.HasType(CardType.Field) || c.HasType(CardType.Equip)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            
            if (Duel.Player == 1 || Duel.Phase == DuelPhase.End)
            {
                var setTarget = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFacedown());
                if (setTarget != null)
                {
                    AI.SelectCard(setTarget);
                    return true;
                }
            }

            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 5. Tier 3 Activators
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool RunickFountainActivate()
        {
            if (Card.Location == CardLocation.Hand || Card.IsFacedown())
            {
                // Play Fountain if not already on field โ€” highest priority
                return !Bot.HasInSpellZone(CardId.RunickFountain);
            }

            // Fountain's recycle/draw effect
            if (_fountainDrawUsed) return false;
            // Only activate if we have โฅ2 Runick spells in GY (return 2+ to deck, draw cards)
            int runickInGY = Bot.Graveyard.Count(c => c != null && IsRunickSpell(c.Id));
            if (runickInGY < 2) return false;
            // Don't draw if hand is already full
            if (Bot.Hand.Count >= 6) return false;
            _fountainDrawUsed = true;
            return true;
        }

        private bool PotOfDualityActivate()
        {
            // Pot of Duality blocks Special Summoning for the rest of the turn.
            // NEVER use if we still need to SS this turn (Hugin from Runick spells).
            bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain) || Bot.HasInHand(CardId.RunickFountain);
            // If we don't have Fountain yet and have Runick spells to SS Hugin โ’ don't use Duality
            if (!hasFountain && Bot.Hand.Any(c => c != null && IsRunickSpell(c.Id))) return false;
            // If we have any Runick spell we want to SS from this turn โ’ don't use
            if (_runickSpellsUsedThisTurn == 0 && Bot.Hand.Any(c => c != null && IsRunickSpell(c.Id)))
                return false;
            // Don't use if we already have a good hand
            if (Bot.Hand.Count >= 5) return false;
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 6. Tier 4 Activators
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool MessengerOfPeaceActivate()
        {
            // Pay 100 maintenance cost unless dangerously low on LP
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                return Bot.LifePoints > 1000;
            }
            // Don't activate if already have one active
            if (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c != Card && c.IsCode(CardId.MessengerOfPeace)))
                return false;
            return true;
        }

        // Continuous S/T duplicate checks
        private bool DimensionalFissureActivate()
        {
            // Don't activate if already face-up on field
            if (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.DimensionalFissure)))
                return false;
            return true;
        }

        private bool ThereCanBeOnlyOneActivate()
        {
            // Don't activate if already face-up
            if (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.ThereCanBeOnlyOne)))
                return false;
            // Only activate during opponent's turn
            if (Duel.Player != 1) return false;
            // Best timing: chain to opponent's summon when they have monsters
            // (this locks them from summoning different Types)
            if (Enemy.GetMonsterCount() > 0) return true;
            // If opponent has no monsters but is in Main Phase, still activate preemptively
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) return true;
            return false;
        }

        private bool RivalryActivate()
        {
            if (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.RivalryOfWarlords)))
                return false;
            if (Duel.Player != 1) return false;
            // Activate when opponent has monsters (locks their Type diversity)
            if (Enemy.GetMonsterCount() > 0) return true;
            // Preemptive activation during opponent's Main Phase
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) return true;
            return false;
        }

        private bool SkillDrainActivate()
        {
            if (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.SkillDrain)))
                return false;
            if (Bot.LifePoints <= 1000) return false;
            // Best timing: chain to opponent's monster effect activation
            if (Duel.LastChainPlayer == 1)
            {
                var lastChain = Util.GetLastChainCard();
                if (lastChain != null && lastChain.IsMonster() && lastChain.Location == CardLocation.MonsterZone)
                    return true;
            }
            // Proactive: activate when opponent has effect monsters on field
            if (Duel.Player == 1 && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !c.IsDisabled()))
                return true;
            return false;
        }

        private bool AntiSpellActivate()
        {
            if (Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.AntiSpellFragrance)))
                return false;
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 7. Tier 5 Activators (Stun Monsters)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool InspectorBoarderSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool ThunderKingRaiOhSummon()
        {
            // If Fountain is not active/in hand, prioritize searching it via Quick-Play -> Hugin first
            bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain) || Bot.HasInHand(CardId.RunickFountain);
            if (!hasFountain)
            {
                // If we can search Fountain with Hugin this turn, wait for that first
                if (NeedsFountainSearch() && Bot.Hand.Any(c => c != null && IsRunickSpell(c.Id)))
                {
                    return false;
                }
            }
            return Bot.GetMonsterCount() == 0;
        }

        private bool DenkoSekkaSummon()
        {
            // Summon Denko Sekka only if we have no Set S/T to avoid locking ourselves
            int setSpells = Bot.GetSpells().Count(c => c != null && c.IsFacedown());
            return setSpells == 0 && Bot.GetMonsterCount() == 0;
        }

        private bool LavaGolemSpSummon()
        {
            // Release 2 opponent monsters to summon Lava Golem
            if (Enemy.GetMonsterCount() < 2) return false;
            // Only worth it if opponent has threatening monsters
            int totalEnemyATK = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup())
                .Sum(c => c.Attack);
            bool hasThreat = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                (c.Attack >= 2500 || (!c.IsDisabled() && c.HasType(CardType.Effect))));
            if (totalEnemyATK < 3000 && !hasThreat) return false;
            return true;
        }

        private bool PankratopsSpSummon()
        {
            return Enemy.GetMonsterCount() > Bot.GetMonsterCount();
        }

        private bool PankratopsActivate()
        {
            // Pankratops quick effect target pop
            var target = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                .FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•
        //  ยง 8. Tier 6 Activators (Extra Deck)
        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool HuginSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Prevent duplicate Hugin on field unless needed
            return !Bot.HasInMonstersZone(CardId.HuginTheRunickWings);
        }

        private bool HuginActivate()
        {
            if (Card == null) return true;
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Discard 1 to search Fountain
                bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain) || Bot.HasInHand(CardId.RunickFountain);
                if (!hasFountain)
                {
                    if (IsSearchBlocked()) return false;
                    return Bot.Hand.Count > 0 && Bot.GetRemainingCount(CardId.RunickFountain, 2) > 0;
                }
                
                // Substitution protect effect
                return true;
            }
            return true;
        }

        private bool MuninSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return !Bot.HasInMonstersZone(CardId.MuninTheRunickWings);
        }

        private bool MuninActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && !Card.IsDisabled())
            {
                // Discard search effect (no target in deck)
                if (ActivateDescription == Util.GetStringId(CardId.MuninTheRunickWings, 0))
                {
                    return false;
                }
                
                // Banish negate effect
                if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard() != null)
                {
                    return true;
                }
            }
            return false;
        }

        private bool GeriSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return !Bot.HasInMonstersZone(CardId.GeriTheRunickFangs);
        }

        private bool GeriActivate()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Retrieve Fountain from GY
                if (ActivateDescription == Util.GetStringId(CardId.GeriTheRunickFangs, 0))
                {
                    return Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.RunickFountain));
                }
            }
            return true;
        }

        private bool IsValuableMonster(ClientCard card)
        {
            if (card == null) return false;
            if (StunMonsters.Contains(card.Id)) return true;
            if (card.IsFaceup() && (card.Attack >= 1500 || card.HasType(CardType.Fusion) || card.HasType(CardType.Xyz))) return true;
            return false;
        }

        private bool CanSummonWithoutValuableMaterials(int targetCardId)
        {
            var ownMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            var availableMaterials = ownMonsters.Where(c => !IsValuableMonster(c)).ToList();

            int requiredCount = 2; // Default for Link-2, Synchros, Xyz, etc.
            if (targetCardId == CardId.HuginTheRunickWings || targetCardId == CardId.MuninTheRunickWings || targetCardId == CardId.GeriTheRunickFangs)
            {
                return true;
            }

            return availableMaterials.Count >= requiredCount;
        }

        private bool LinkSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            if (Card != null && !CanSummonWithoutValuableMaterials(Card.Id)) return false;
            return true;
        }

        private bool XyzSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Card != null && Card.IsCode(CardId.SuperStarslayerTyphon)) return true;
            if (Card != null && !CanSummonWithoutValuableMaterials(Card.Id)) return false;
            return true;
        }

        private bool SpellSetCheck()
        {
            if (Card == null) return false;
            // Never set Continuous Spells/Traps or Field Spells (activate them directly)
            if (Card.HasType(CardType.Continuous) || Card.HasType(CardType.Field)) return false;

            bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain);
            // If Fountain is active on field, keep Runick Quick-Play Spells in hand (Fountain allows activating from hand)
            if (hasFountain && IsRunickSpell(Card.Id)) return false;

            // If Fountain is NOT active on field, set Runick Quick-Plays so they can be activated on opponent's turn!
            return (Card.IsTrap() || Card.HasType(CardType.QuickPlay))
                && Util.IsTurn1OrMain2() 
                && Bot.GetSpellCountWithoutField() < 4;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return BossMonsters.Contains(card.Id) || StunMonsters.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            return 100;
        }

        protected override bool IsBoardStrongEnough()
        {
            // Fountain active = core engine online
            bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain);
            if (!hasFountain) return false;
            // Fountain + stun monster (Boarder/Rai-Oh) = full lock
            bool hasStun = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && StunMonsters.Contains(c.Id));
            if (hasStun) return true;
            // Fountain + floodgate trap face-up = strong stall
            bool hasFloodgate = Bot.GetSpells().Any(c => c != null && c.IsFaceup() &&
                c.IsCode(CardId.SkillDrain, CardId.ThereCanBeOnlyOne, CardId.RivalryOfWarlords));
            if (hasFloodgate) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            // Runick doesn't extend traditionally; stop summoning once we have
            // Fountain active + stun/floodgate. Win condition is mill, not board presence.
            bool hasFountain = Bot.HasInSpellZone(CardId.RunickFountain);
            bool hasStun = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && StunMonsters.Contains(c.Id));
            if (hasFountain && hasStun) return true;
            return false;
        }
    }

    [Deck("Expert_2026_Runick", "2026_Runick")]
    public class ExpertRunickExecutor : _2026_RunickExecutor
    {
        public ExpertRunickExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }
}

