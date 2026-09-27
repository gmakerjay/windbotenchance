// ============================================================
// CARD AUDIT — 2026 Exodia
// ============================================================
// | Card Name                     | Type       | OPT? | Effect Summary                         | Activate When                         | NEVER Activate When                    |
// |-------------------------------|------------|------|----------------------------------------|---------------------------------------|----------------------------------------|
// | Ash Blossom                   | Hand Trap  | Yes  | Negate search/SS                       | Opponent searches/SS                  | Board is strong                        |
// | Droll & Lock Bird             | Hand Trap  | Yes  | Stop searches                          | Opponent adds card from deck to hand  | Draw Phase                             |
// | Maxx "C"                      | Hand Trap  | Yes  | Draw when opponent SS                  | Opponent SS on their turn             | We control any cards                   |
// | The Unstoppable Exodia        | Fusion     | Yes  | Negate S/T, gain ATK, unaffected       | Opponent activates S/T                | -                                      |
// | Exxod Fires of Rage           | Spell      | Yes  | Field wipe / GY recycle                | We control Exodia Fusion, enemy has fd| Enemy controls no cards                |
// | Millennium Ankh               | Spell      | Yes  | Fusion summon Exodia from Extra Deck   | All 5 Forbidden One parts accessible  | Exodia Fusion already on field         |
// | Sengenjin Wakes               | Monster    | Yes  | S/T zone place, SS self, search Mill.  | Main Phase setup                      | Zone full or HOPT used                 |
// | Shield of Mill. Dynasty       | Monster    | Yes  | S/T zone place, SS self, search Ankh   | Main Phase setup                      | Zone full or HOPT used                 |
// | Golem Guards Mill. Treasures  | Monster    | Yes  | S/T zone place, SS self, search Wedju  | Main Phase setup                      | Zone full or HOPT used                 |
// | Wedju Temple                  | Field      | Yes  | Place hand monster + place deck monster| Main Phase combo setup                | Zone full or HOPT used                 |
// | Heart of the Blue-Eyes        | Monster    | Yes  | Discard to search Millennium Ankh      | We need Millennium Ankh                | Ankh already in hand                   |
// | Gravekeeper's Commandant      | Monster    | Yes  | Discard to search Necrovalley          | Necrovalley not on field              | Macro Cosmos or Fissure active         |
// | Necrovalley Throne            | Spell      | Yes  | Search Commandant                      | Necrovalley not on field              | Necrovalley already active             |
// | Necrovalley                   | Field      | No   | Negate GY cards/movement               | Played to lock opponent               | Duplicate already active               |
// | Macro Cosmos                  | Trap       | No   | Banish all cards sent to GY            | Disrupted opponent, end of turn       | Duplicate already active               |
// | Dimensional Fissure           | Spell      | No   | Banish all monsters sent to GY         | Disrupted opponent                    | Duplicate already active               |
// | Light-Imprisoning Mirror      | Trap       | No   | Negate LIGHT monster effects           | Opponent plays LIGHT                  | Duplicate already active               |
// | Super Polymerization          | Spell      | No   | Board breaker fusion summon            | Opponent has valid fusion targets     | No discard cost in hand                |
// ============================================================
// ACE CARDS:
//   Primary: The Unstoppable Exodia Incarnate (83257450)
// COMBO STARTERS:
//   1. Millennium Ankh (37613663)
//   2. Heart of the Blue-Eyes (54475145)
//   3. Shield of the Millennium Dynasty (1164211)
//   4. Sengenjin Wakes from a Millennium (38775407)
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
    [Deck("2026_Exodia", "2026_Exodia")]
    public class _2026_ExodiaExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int ShieldOfTheMillenniumDynasty = 1164211;
            public const int LeftArmOfTheForbiddenOne = 7902349;
            public const int RightLegOfTheForbiddenOne = 8124921;
            public const int AshBlossom = 14558127;
            public const int GravekeepersCommandant = 17393207;
            public const int MaxxC = 23434538;
            public const int ExxodFiresOfRage = 23617756;
            public const int ExodiaTheForbiddenOne = 33396948;
            public const int NecrovalleyThrone = 37561138;
            public const int MillenniumAnkh = 37613663;
            public const int SengenjinWakesFromAMillennium = 38775407;
            public const int LeftLegOfTheForbiddenOne = 44519536;
            public const int Necrovalley = 47355498;
            public const int SuperPolymerization = 48130397;
            public const int LightImprisoningMirror = 53341729;
            public const int HeartOfTheBlueEyes = 54475145;
            public const int WedjuTemple = 63017368;
            public const int RightArmOfTheForbiddenOne = 70903634;
            public const int SoulOfGaiaTheFierceKnight = 73129314;
            public const int GolemThatGuardsTheMillenniumTreasures = 74169516;
            public const int DimensionalFissure = 81674782;
            public const int MacroCosmos = 30241314;
            public const int DrollAndLockBird = 94145021;

            // Extra Deck
            public const int TheUnstoppableExodiaIncarnate = 83257450;
            public const int MudragonOfTheSwamp = 54757758;
            public const int MagistusChorozo = 66532962;
            public const int SeaMonsterOfTheseus = 96334243;
            public const int Garura = 11765832;
            public const int BorreloadFuriousDragon = 92892239;
            public const int MysterionTheDragonCrown = 13735899;
            public const int MirrorjadeTheIcebladeDragon = 44146295;
            public const int SnakeEyesDoomedDragon = 58071334;
            public const int PredaplantTriphyoverutum = 79864860;
            public const int AxonKickerOracle = 33171768;
            public const int ArtemisTheMagistusMoonMaiden = 34755994;
            public const int SPLittleKnight = 29301450;
            public const int SorcererOfSebek = 39767432;
        }

        private static readonly int[] ForbiddenOneParts = {
            CardId.ExodiaTheForbiddenOne,
            CardId.LeftArmOfTheForbiddenOne,
            CardId.RightArmOfTheForbiddenOne,
            CardId.LeftLegOfTheForbiddenOne,
            CardId.RightLegOfTheForbiddenOne
        };

        // HOPT & State Flags
        private bool _shieldPlacedThisTurn = false;
        private bool _shieldSummonedThisTurn = false;
        private bool _sengenjinPlacedThisTurn = false;
        private bool _sengenjinSummonedThisTurn = false;
        private bool _golemPlacedThisTurn = false;
        private bool _golemSummonedThisTurn = false;
        private bool _wedjuUsedThisTurn = false;
        private bool _heartUsedThisTurn = false;
        private bool _exxodUsedThisTurn = false;
        private bool _ankhUsedThisTurn = false;
        private bool _commandantUsedThisTurn = false;
        private bool _throneUsedThisTurn = false;

        public _2026_ExodiaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // ── Combo Router: Sequencing ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.ShieldOfTheMillenniumDynasty, CardId.LeftArmOfTheForbiddenOne },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ShieldOfTheMillenniumDynasty, ActionType = ExecutorType.Activate, Description = "Play CardId.ShieldOfTheMillenniumDynasty" },
                    new() { CardId = CardId.LeftArmOfTheForbiddenOne, ActionType = ExecutorType.Activate, Description = "Extend with CardId.LeftArmOfTheForbiddenOne" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Heart-Search-Path",
                RequiredCards = new List<int> { CardId.HeartOfTheBlueEyes },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.HeartOfTheBlueEyes, ActionType = ExecutorType.Activate, Description = "Heart searches Millennium Ankh" },
                    new() { CardId = CardId.MillenniumAnkh, ActionType = ExecutorType.Activate, Description = "Ankh Fusion Summons Exodia" },
                    new() { CardId = CardId.TheUnstoppableExodiaIncarnate, ActionType = ExecutorType.SpSummon, Description = "Summon Exodia Incarnate" }
                },
                EndBoardScore = 95
            });

            // ── Bait Planner ──
            BaitPlanner.RegisterComboStarters(CardId.ShieldOfTheMillenniumDynasty, CardId.MillenniumAnkh);
            BaitPlanner.RegisterBaitCards(CardId.MillenniumAnkh);

            // ── Chain Advisor ──
            ChainAdvisor.RegisterHighValueTargets(CardId.ShieldOfTheMillenniumDynasty, CardId.MillenniumAnkh, CardId.SPLittleKnight);

            // TIER 1: Hand Traps (Ash, Maxx "C", Droll)
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomActivate);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCActivate);

            // TIER 2: Quick Effects of Boss (Exodia Incarnate negate, Exxod Fires of Rage field wipe)
            AddExecutor(ExecutorType.Activate, CardId.TheUnstoppableExodiaIncarnate, ExodiaIncarnateActivate);
            AddExecutor(ExecutorType.Activate, CardId.ExxodFiresOfRage, ExxodFiresOfRageActivate);

            // TIER 3: Fusion Summon Spells (Millennium Ankh)
            AddExecutor(ExecutorType.Activate, CardId.MillenniumAnkh, MillenniumAnkhActivate);

            // TIER 4: Search Spells and Starters (Heart of the Blue-Eyes, Wedju Temple, Millennium placement)
            AddExecutor(ExecutorType.Activate, CardId.HeartOfTheBlueEyes, HeartOfTheBlueEyesActivate);
            AddExecutor(ExecutorType.Activate, CardId.WedjuTemple, WedjuTempleActivate);
            AddExecutor(ExecutorType.Activate, CardId.SengenjinWakesFromAMillennium, SengenjinPlaceActivate);
            AddExecutor(ExecutorType.Activate, CardId.ShieldOfTheMillenniumDynasty, ShieldPlaceActivate);
            AddExecutor(ExecutorType.Activate, CardId.GolemThatGuardsTheMillenniumTreasures, GolemPlaceActivate);

            // TIER 5: Special Summoning Millennium monsters from Spell Zone
            AddExecutor(ExecutorType.Activate, CardId.SengenjinWakesFromAMillennium, SengenjinSummonActivate);
            AddExecutor(ExecutorType.Activate, CardId.ShieldOfTheMillenniumDynasty, ShieldSummonActivate);
            AddExecutor(ExecutorType.Activate, CardId.GolemThatGuardsTheMillenniumTreasures, GolemSummonActivate);

            // TIER 6: Stun/Control Spells and Traps (Commandant, Necrovalley, Macro Cosmos, Dimensional Fissure, Mirrors)
            AddExecutor(ExecutorType.Activate, CardId.NecrovalleyThrone, NecrovalleyThroneActivate);
            AddExecutor(ExecutorType.Activate, CardId.GravekeepersCommandant, CommandantActivate);
            AddExecutor(ExecutorType.Activate, CardId.Necrovalley, NecrovalleyActivate);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationActivate);
            AddExecutor(ExecutorType.Activate, CardId.DimensionalFissure, DimensionalFissureActivate);
            AddExecutor(ExecutorType.Activate, CardId.MacroCosmos, MacroCosmosActivate);
            AddExecutor(ExecutorType.Activate, CardId.LightImprisoningMirror, LightImprisoningMirrorActivate);

            // Extra Deck Fusions and links
            AddExecutor(ExecutorType.SpSummon, CardId.TheUnstoppableExodiaIncarnate);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ArtemisTheMagistusMoonMaiden, () => false);
            AddExecutor(ExecutorType.SpSummon, CardId.SorcererOfSebek, () => false);

            // Spell/Trap setting (MP2 optimization)
            AddExecutor(ExecutorType.SpellSet, CardId.MacroCosmos);
            AddExecutor(ExecutorType.SpellSet, CardId.LightImprisoningMirror);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization);
            AddExecutor(ExecutorType.SpellSet, CardId.DimensionalFissure);
            AddExecutor(ExecutorType.SpellSet, CardId.Necrovalley);

            // Position repos
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Prefer going first to set up floodgates and combos
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _shieldPlacedThisTurn = false;
            _shieldSummonedThisTurn = false;
            _sengenjinPlacedThisTurn = false;
            _sengenjinSummonedThisTurn = false;
            _golemPlacedThisTurn = false;
            _golemSummonedThisTurn = false;
            _wedjuUsedThisTurn = false;
            _heartUsedThisTurn = false;
            _exxodUsedThisTurn = false;
            _ankhUsedThisTurn = false;
            _commandantUsedThisTurn = false;
            _throneUsedThisTurn = false;

            // ── Going-Second BreakBoard: prioritize disruption over combo ──
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.ShieldOfTheMillenniumDynasty)
                {
                    if (card.Location == CardLocation.Hand)
                        _shieldPlacedThisTurn = true;
                    else if (card.Location == CardLocation.SpellZone)
                        _shieldSummonedThisTurn = true;
                }
                if (card.Id == CardId.SengenjinWakesFromAMillennium)
                {
                    if (card.Location == CardLocation.Hand)
                        _sengenjinPlacedThisTurn = true;
                    else if (card.Location == CardLocation.SpellZone)
                        _sengenjinSummonedThisTurn = true;
                }
                if (card.Id == CardId.GolemThatGuardsTheMillenniumTreasures)
                {
                    if (card.Location == CardLocation.Hand)
                        _golemPlacedThisTurn = true;
                    else if (card.Location == CardLocation.SpellZone)
                        _golemSummonedThisTurn = true;
                }
                if (card.Id == CardId.WedjuTemple)
                    _wedjuUsedThisTurn = true;
                if (card.Id == CardId.HeartOfTheBlueEyes)
                    _heartUsedThisTurn = true;
                if (card.Id == CardId.ExxodFiresOfRage)
                    _exxodUsedThisTurn = true;
                if (card.Id == CardId.MillenniumAnkh)
                    _ankhUsedThisTurn = true;
                if (card.Id == CardId.GravekeepersCommandant)
                    _commandantUsedThisTurn = true;
                if (card.Id == CardId.NecrovalleyThrone)
                    _throneUsedThisTurn = true;
            }
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.TheUnstoppableExodiaIncarnate;
        }

        protected override bool IsBoardStrongEnough()
        {
            // Exodia Incarnate on field = win condition established
            if (Bot.HasInMonstersZone(CardId.TheUnstoppableExodiaIncarnate))
                return true;
            // Necrovalley + Millennium monsters on field = sufficient stall
            bool hasNecrovalley = Bot.HasInSpellZone(CardId.Necrovalley, faceUp: true);
            bool hasMillenniumMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                (c.Id == CardId.ShieldOfTheMillenniumDynasty || c.Id == CardId.SengenjinWakesFromAMillennium ||
                 c.Id == CardId.GolemThatGuardsTheMillenniumTreasures || c.Id == CardId.SoulOfGaiaTheFierceKnight));
            if (hasNecrovalley && hasMillenniumMonster && Bot.GetMonsterCount() >= 2)
                return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            // Stop once Exodia Incarnate is on the field
            if (Bot.HasInMonstersZone(CardId.TheUnstoppableExodiaIncarnate))
                return true;
            return false;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.AshBlossom || c.Id == CardId.DrollAndLockBird || c.Id == CardId.MaxxC)
                return 800; // Protect hand traps
            if (ForbiddenOneParts.Contains(c.Id))
                return 700; // Protect parts
            if (c.Id == CardId.ShieldOfTheMillenniumDynasty ||
                c.Id == CardId.SengenjinWakesFromAMillennium ||
                c.Id == CardId.GolemThatGuardsTheMillenniumTreasures ||
                c.Id == CardId.SoulOfGaiaTheFierceKnight ||
                c.Id == CardId.HeartOfTheBlueEyes)
                return 600; // Protect Millennium combo pieces
            return 100;
        }

        // --- HAND TRAPS ---
        private bool AshBlossomActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool DrollAndLockBirdActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player != 1) return false;
            if (Duel.LastChainPlayer != 1) return false;
            return true;
        }

        private bool MaxxCActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player != 1) return false;
            return true;
        }

        // --- BOSS EFFECTS & REMOVALS ---
        private bool ExodiaIncarnateActivate()
        {
            // If the last chain card is our own Spell/Trap, do NOT negate it!
            if (LastChainCard != null && LastChainCard.Controller == 0 && (LastChainCard.IsSpell() || LastChainCard.IsTrap()))
            {
                return false;
            }

            // Prevent double chaining negation
            if (LastChainCard != null && (LastChainCard.IsSpell() || LastChainCard.IsTrap()))
            {
                if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.TheUnstoppableExodiaIncarnate))
                    return false;
            }

            // For other activations (ATK gain during battle, End Phase search/set, opponent's S/T negation), always return true
            return true;
        }

        private bool ExxodFiresOfRageActivate()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                // Field wipe effect
                if (_exxodUsedThisTurn) return false;
                if (!Bot.MonsterZone.Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.TheUnstoppableExodiaIncarnate)))
                    return false;
                if (Enemy.GetMonsterCount() + Enemy.GetSpellCount() == 0)
                    return false;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // GY recovery effect
                // Check if any Forbidden One part is in GY or banished
                bool hasPartInGyOrBanish = Bot.Graveyard.Any(c => ForbiddenOneParts.Contains(c.Id)) ||
                                           Bot.Banished.Any(c => ForbiddenOneParts.Contains(c.Id));
                if (hasPartInGyOrBanish)
                {
                    // Select shuffle option (handled in OnSelectCard/Option)
                    return true;
                }
                return false;
            }
            return false;
        }

        // --- COMBO STARTERS & SPELLS ---
        private bool CanActivateMillenniumAnkh()
        {
            if (_ankhUsedThisTurn) return false;
            foreach (int partId in ForbiddenOneParts)
            {
                bool inHand = Bot.Hand.Any(c => c.Id == partId);
                bool inDeck = GetRemainingCount(partId) > 0;
                bool inMonsters = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == partId);
                bool inSpells = Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.Id == partId);
                if (!inHand && !inDeck && !inMonsters && !inSpells)
                    return false;
            }
            return true;
        }

        private bool MillenniumAnkhActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (!CanActivateMillenniumAnkh()) return false;
            if (Bot.HasInMonstersZone(CardId.TheUnstoppableExodiaIncarnate)) return false;
            return true;
        }

        private bool HeartOfTheBlueEyesActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (_heartUsedThisTurn) return false;
            if (Bot.HasInHand(CardId.MillenniumAnkh)) return false;
            return true;
        }

        private bool WedjuTempleActivate()
        {
            if (Bot.HasInSpellZone(CardId.WedjuTemple))
            {
                // Effect activation
                if (_wedjuUsedThisTurn) return false;
                if (Bot.GetSpellCount() >= 4) return false; // Need at least 2 free slots
                if (Bot.GetHandCount() == 0) return false;
                return true;
            }
            // Play from hand
            if (Bot.HasInSpellZone(CardId.WedjuTemple)) return false;
            return true;
        }

        // --- MILLENNIUM MONSTER S/T PLACEMENTS ---
        private bool ShieldPlaceActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_shieldPlacedThisTurn) return false;
            if (Bot.GetSpellCount() >= 5) return false;
            if (Duel.Player != 0) return false;
            return true;
        }

        private bool SengenjinPlaceActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_sengenjinPlacedThisTurn) return false;
            if (Bot.GetSpellCount() >= 5) return false;
            if (Duel.Player != 0) return false;
            return true;
        }

        private bool GolemPlaceActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_golemPlacedThisTurn) return false;
            if (Bot.GetSpellCount() >= 5) return false;
            if (Duel.Player != 0) return false;
            return true;
        }

        // --- MILLENNIUM MONSTER SUMMONS FROM S/T ZONE ---
        private bool ShieldSummonActivate()
        {
            if (Card.Location != CardLocation.SpellZone || Card.IsFacedown()) return false;
            if (_shieldSummonedThisTurn) return false;
            if (Bot.GetMonsterCount() >= 5) return false;
            if (Duel.Player != 0) return false;
            if (Bot.LifePoints <= 2000) return false;

            // Safeguard: block summon if Exodia Incarnate is active, unless we can summon with Millennium Ankh (0 LP cost)
            if (Bot.HasInMonstersZone(CardId.TheUnstoppableExodiaIncarnate) && !Bot.HasInHand(CardId.MillenniumAnkh))
                return false;

            // Shield searches Millennium Ankh, so we can pay 2000 LP if we have > 3000 LP
            if (Bot.LifePoints <= 3000 && !Bot.HasInHand(CardId.MillenniumAnkh))
                return false;

            return true;
        }

        private bool SengenjinSummonActivate()
        {
            if (Card.Location != CardLocation.SpellZone || Card.IsFacedown()) return false;
            if (_sengenjinSummonedThisTurn) return false;
            if (Bot.GetMonsterCount() >= 5) return false;
            if (Duel.Player != 0) return false;
            if (Bot.LifePoints <= 2000) return false;

            // Safeguard: block summon if Exodia Incarnate is active, unless we can summon with Millennium Ankh (0 LP cost)
            if (Bot.HasInMonstersZone(CardId.TheUnstoppableExodiaIncarnate) && !Bot.HasInHand(CardId.MillenniumAnkh))
                return false;

            // Only summon if we have Ankh or > 4000 LP
            if (!Bot.HasInHand(CardId.MillenniumAnkh) && Bot.LifePoints < 4000)
                return false;

            return true;
        }

        private bool GolemSummonActivate()
        {
            if (Card.Location != CardLocation.SpellZone || Card.IsFacedown()) return false;
            if (_golemSummonedThisTurn) return false;
            if (Bot.GetMonsterCount() >= 5) return false;
            if (Duel.Player != 0) return false;
            if (Bot.LifePoints <= 2000) return false;

            // Safeguard: block summon if Exodia Incarnate is active, unless we can summon with Millennium Ankh (0 LP cost)
            if (Bot.HasInMonstersZone(CardId.TheUnstoppableExodiaIncarnate) && !Bot.HasInHand(CardId.MillenniumAnkh))
                return false;

            // Only summon if we have Ankh or > 4000 LP
            if (!Bot.HasInHand(CardId.MillenniumAnkh) && Bot.LifePoints < 4000)
                return false;

            return true;
        }

        // --- STUNS & FLOODGATES ---
        private bool CommandantActivate()
        {
            if (_commandantUsedThisTurn) return false;
            if (Bot.HasInSpellZone(CardId.Necrovalley)) return false;

            // Commandant must discard to GY. Under Macro Cosmos or Dimensional Fissure, it would banish
            // and fail cost requirement.
            if (Bot.HasInSpellZone(CardId.MacroCosmos) || Bot.HasInSpellZone(CardId.DimensionalFissure))
                return false;

            return true;
        }

        private bool NecrovalleyThroneActivate()
        {
            if (_throneUsedThisTurn) return false;
            if (Bot.HasInSpellZone(CardId.Necrovalley)) return false;
            return true;
        }

        private bool NecrovalleyActivate()
        {
            if (!UniqueFaceupSpell()) return false;
            return true;
        }

        private bool SuperPolymerizationActivate()
        {
            if (Bot.Hand.Count == 0) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.SuperPolymerization)) return false;

            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            if (oppMonsters.Count < 2) return false;

            // 1. Garura: 2 monsters with same Type and Attribute but different names
            for (int i = 0; i < oppMonsters.Count; i++)
            {
                for (int j = i + 1; j < oppMonsters.Count; j++)
                {
                    var m1 = oppMonsters[i];
                    var m2 = oppMonsters[j];
                    if (m1.Attribute == m2.Attribute && m1.Race == m2.Race && m1.Id != m2.Id)
                        return true;
                }
            }

            // 2. Mudragon of the Swamp: 2 monsters with same Attribute but different Types
            for (int i = 0; i < oppMonsters.Count; i++)
            {
                for (int j = i + 1; j < oppMonsters.Count; j++)
                {
                    var m1 = oppMonsters[i];
                    var m2 = oppMonsters[j];
                    if (m1.Attribute == m2.Attribute && m1.Race != m2.Race)
                        return true;
                }
            }

            return false;
        }

        private bool DimensionalFissureActivate()
        {
            if (!UniqueFaceupSpell()) return false;

            // Hold DF if we have Commandant in hand and need Necrovalley
            if (Bot.HasInHand(CardId.GravekeepersCommandant) && !Bot.HasInSpellZone(CardId.Necrovalley))
                return false;

            return true;
        }

        private bool MacroCosmosActivate()
        {
            if (!UniqueFaceupSpell()) return false;

            // Activate on opponent's turn or end of our turn
            if (Duel.Player == 0 && Duel.Phase != DuelPhase.Main2 && Duel.Phase != DuelPhase.End)
                return false;

            // Hold Macro if we have Commandant in hand and need Necrovalley
            if (Bot.HasInHand(CardId.GravekeepersCommandant) && !Bot.HasInSpellZone(CardId.Necrovalley))
                return false;

            return true;
        }

        private bool LightImprisoningMirrorActivate()
        {
            if (!UniqueFaceupSpell()) return false;

            // Never activate if we have Exodia Incarnate or plan to summon it
            if (Bot.HasInMonstersZone(CardId.TheUnstoppableExodiaIncarnate)) return false;
            if (Bot.HasInHand(CardId.MillenniumAnkh) || Bot.HasInSpellZone(CardId.MillenniumAnkh)) return false;

            // Check if we have any LIGHT Millennium/Exodia cards on field or hand or ST zone
            bool hasLightComboPieces = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Id == CardId.ShieldOfTheMillenniumDynasty || c.Id == CardId.GolemThatGuardsTheMillenniumTreasures))
                || Bot.GetSpells().Any(c => c != null && (c.Id == CardId.ShieldOfTheMillenniumDynasty || c.Id == CardId.GolemThatGuardsTheMillenniumTreasures))
                || Bot.Hand.Any(c => c != null && (c.Id == CardId.ShieldOfTheMillenniumDynasty || c.Id == CardId.GolemThatGuardsTheMillenniumTreasures || c.Id == CardId.HeartOfTheBlueEyes));

            if (hasLightComboPieces) return false;

            // Activate on opponent's turn or end of our turn
            if (Duel.Player == 0 && Duel.Phase != DuelPhase.Main2 && Duel.Phase != DuelPhase.End)
                return false;

            // Only activate it if the opponent has activated a LIGHT monster effect on the field or GY
            if (LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.IsMonster() && LastChainCard.HasAttribute(CardAttribute.Light))
            {
                return true;
            }

            return false;
        }

        private bool SPLittleKnightSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            // Do not link away Millennium monsters or Exodia Incarnate if we are trying to summon Exodia
            var monstersToKeep = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && 
                (c.Id == CardId.ShieldOfTheMillenniumDynasty || 
                 c.Id == CardId.SengenjinWakesFromAMillennium || 
                 c.Id == CardId.GolemThatGuardsTheMillenniumTreasures || 
                 c.Id == CardId.SoulOfGaiaTheFierceKnight ||
                 c.Id == CardId.TheUnstoppableExodiaIncarnate));
            
            if (monstersToKeep.Any())
            {
                // We want to keep them. SP requires 2 monsters. If we have to use one of these, return false.
                int nonCrucialCount = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && 
                    c.Id != CardId.ShieldOfTheMillenniumDynasty && 
                    c.Id != CardId.SengenjinWakesFromAMillennium && 
                    c.Id != CardId.GolemThatGuardsTheMillenniumTreasures && 
                    c.Id != CardId.SoulOfGaiaTheFierceKnight &&
                    c.Id != CardId.TheUnstoppableExodiaIncarnate);
                
                if (nonCrucialCount < 2)
                    return false;
            }

            bool enemyHasThreat = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup()) || Enemy.GetSpells().Any(c => c != null && c.IsFaceup());
            return enemyHasThreat;
        }

        // --- SELECTION & UTILITIES ---
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
                    if (card != null && !result.Contains(card))
                    {
                        result.Add(card);
                        if (result.Count >= min) break;
                    }
                }
            }
            return result;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // If the hint is a removal/material/discard/send-to-GY/destroy/banish effect, sort by priority (lowest first) to protect valuable cards
            if (hint == 501 || hint == 502 || hint == 503 || hint == 504 || hint == 505 || hint == 508 || 
                hint == 511 || hint == 512 || hint == 513 || hint == 533)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            if (hint == 509) // Special Summon / Placement
            {
                var millenniums = cards.Where(c => c != null && 
                    (c.Id == CardId.SengenjinWakesFromAMillennium || 
                     c.Id == CardId.ShieldOfTheMillenniumDynasty || 
                     c.Id == CardId.GolemThatGuardsTheMillenniumTreasures ||
                     c.Id == CardId.HeartOfTheBlueEyes)).ToList();

                if (millenniums.Count > 0)
                {
                    // Location filter to prevent crashes
                    bool hasDeck = cards.Any(c => c.Location == CardLocation.Deck);
                    if (hasDeck)
                    {
                        var deckMillenniums = millenniums.Where(c => c.Location == CardLocation.Deck).ToList();
                        if (deckMillenniums.Count > 0)
                            return SelectPreferred(deckMillenniums, min, max, CardId.ShieldOfTheMillenniumDynasty, CardId.SengenjinWakesFromAMillennium, CardId.GolemThatGuardsTheMillenniumTreasures);
                    }
                    else
                    {
                        return SelectPreferred(millenniums, min, max, CardId.SengenjinWakesFromAMillennium, CardId.ShieldOfTheMillenniumDynasty, CardId.GolemThatGuardsTheMillenniumTreasures);
                    }
                }
            }

            if (hint == 506) // Search / Add to hand
            {
                if (!Bot.HasInHand(CardId.MillenniumAnkh))
                {
                    var ankh = cards.FirstOrDefault(c => c != null && c.Id == CardId.MillenniumAnkh);
                    if (ankh != null) return new List<ClientCard> { ankh };
                }

                var preferred = SelectPreferred(cards, min, max, 
                    CardId.MillenniumAnkh, 
                    CardId.ShieldOfTheMillenniumDynasty, 
                    CardId.SengenjinWakesFromAMillennium, 
                    CardId.GolemThatGuardsTheMillenniumTreasures,
                    CardId.WedjuTemple);
                if (preferred.Count >= min) return preferred;
            }

            if (LastChainCard != null && LastChainCard.Id == CardId.ExxodFiresOfRage)
            {
                // GY recovery option: select Forbidden One parts
                var parts = cards.Where(c => c != null && ForbiddenOneParts.Contains(c.Id)).ToList();
                if (parts.Count >= min) return parts.Take(max).ToList();
            }

            if (LastChainCard != null && LastChainCard.Id == CardId.SuperPolymerization)
            {
                var oppMaterials = cards.Where(c => c != null && c.Controller == 1).ToList();
                var ourMaterials = cards.Where(c => c != null && c.Controller == 0).ToList();
                
                var selected = new List<ClientCard>();
                foreach (var c in oppMaterials)
                {
                    selected.Add(c);
                    if (selected.Count >= max) break;
                }
                if (selected.Count < min)
                {
                    foreach (var c in ourMaterials)
                    {
                        selected.Add(c);
                        if (selected.Count >= min) break;
                    }
                }
                return selected;
            }

            if (Card == null) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Card specific handlers
            if (Card.Id == CardId.MillenniumAnkh)
            {
                var fusion = cards.FirstOrDefault(c => c != null && c.Id == CardId.TheUnstoppableExodiaIncarnate);
                if (fusion != null) return new List<ClientCard> { fusion };
                return cards.Where(c => c != null && ForbiddenOneParts.Contains(c.Id)).ToList();
            }

            if (Card.Id == CardId.WedjuTemple)
            {
                var handMonsters = cards.Where(c => c != null && c.Location == CardLocation.Hand && c.IsMonster()).ToList();
                if (handMonsters.Count > 0)
                {
                    var pref = SelectPreferred(handMonsters, min, max, 
                        CardId.SengenjinWakesFromAMillennium, 
                        CardId.ShieldOfTheMillenniumDynasty, 
                        CardId.GolemThatGuardsTheMillenniumTreasures);
                    if (pref.Count >= min) return pref;
                    return handMonsters.Take(max).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.TheUnstoppableExodiaIncarnate || 
                cardId == CardId.SengenjinWakesFromAMillennium)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            if (cardId == CardId.ShieldOfTheMillenniumDynasty ||
                cardId == CardId.GolemThatGuardsTheMillenniumTreasures)
            {
                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        // --- BATTLE & REPOS ---
        private bool MonsterRepos()
        {
            if (Card == null) return false;

            // Exodia Incarnate must always be in Attack Position
            if (Card.Id == CardId.TheUnstoppableExodiaIncarnate)
            {
                if (Card.IsDefense()) return true;
                return false;
            }

            // Golem and Shield have 0 ATK, they must always be in Defense Position
            if (Card.Id == CardId.GolemThatGuardsTheMillenniumTreasures || Card.Id == CardId.ShieldOfTheMillenniumDynasty)
            {
                if (Card.IsAttack()) return true;
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
            int atk = attacker.Id == CardId.TheUnstoppableExodiaIncarnate ? Bot.LifePoints : attacker.Attack;
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.IsAttack())
                {
                    if (enemy.Attack > atk) return false;
                }
                if (enemy.IsDefense() && atk <= enemy.Defense) return false;
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

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (attacker != null && attacker.Id == CardId.TheUnstoppableExodiaIncarnate)
            {
                attacker.RealPower = Bot.LifePoints;
            }
            if (defender != null && defender.Id == CardId.TheUnstoppableExodiaIncarnate)
            {
                defender.RealPower = Bot.LifePoints;
            }
            return base.OnPreBattleBetween(attacker, defender);
        }

        public override ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            var exodia = attackers.FirstOrDefault(c => c != null && c.Id == CardId.TheUnstoppableExodiaIncarnate);
            if (exodia != null) return exodia;
            return base.OnSelectAttacker(attackers, defenders);
        }

        public override int OnSelectOption(IList<long> options)
        {
            // Check if one of the options has a description that ends with * 16 + 3 (reveal Millennium Ankh)
            // for Sengenjin (38775407), Shield (1164211), Golem (74169516).
            for (int i = 0; i < options.Count; ++i)
            {
                long desc = options[i];
                long cardId = desc >> 4;
                long optionIndex = desc & 0xf;
                
                if (optionIndex == 3 && (cardId == CardId.SengenjinWakesFromAMillennium || cardId == CardId.ShieldOfTheMillenniumDynasty || cardId == CardId.GolemThatGuardsTheMillenniumTreasures))
                {
                    // Select this option to reveal Millennium Ankh for 0 LP cost!
                    return i;
                }
            }
            
            return base.OnSelectOption(options);
        }
    }
}
