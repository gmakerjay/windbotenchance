using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_CrystronTrains", "2026_CrystronTrains")]
    public class _2026_CrystronTrainsExecutor : ModernExecutor
    {
        public class CardId
        {
            // Crystron Monsters
            public const int Citree = 20050865;
            public const int Tristaros = 99471856;
            public const int Smiger = 83443619;
            public const int Sulfefnir = 3422200;
            public const int Sulfador = 25865565;

            // Train / Earth Machine Monsters
            public const int ScrapRecycler = 4334811;
            public const int BleuTraveler = 81101309;
            public const int BulletTrain = 52481437;
            public const int ConvexKnight = 43471513;
            public const int BabelDecker = 45116390;
            public const int Regulus = 10604644;

            // Spells & Traps
            public const int ExceptionalSchedule = 52782439;
            public const int UrgentSchedule = 25274141;
            public const int Switchyard = 76136345;
            public const int Inclusion = 31552317;
            public const int Cluster = 53829527;
            public const int TrainConnection = 60879050;
            public const int BarrageBlast = 51369889;
            public const int CalledByTheGrave = 24224830;
            public const int ForbiddenDroplet = 24299458;
            public const int AshBlossom = 14558127;
            public const int Imperm = 10045474;
            public const int TripleTacticsTalent = 25311006;
            public const int DrollAndLock = 94145021;
            public const int MulcharmyFuwalos = 42141493;

            // Extra Deck
            public const int Eleskeletus = 47736165;
            public const int Quariongandrax = 13455674;
            public const int Legatia = 15982593;
            public const int DawnDragster = 33158448;
            public const int RiverStormer = 24701066;
            public const int GustavMax = 56910167;
            public const int SuperDora = 49032236;
            public const int JuggernautLiebe = 26096328;
            public const int FlyingLauncher = 38354018;
            public const int GustavRocket = 92359409;
            public const int Zeus = 90448279;
            public const int ClockworkKnight = 41739381;
            public const int AngerKnuckle = 146746;
            public const int QliphortGenius = 22423493;
        }

        // Lock tracking flags
        private bool _hasMachineSynchroLock = false;
        private bool _hasMachineExtraDeckLock = false;
        private bool _opponentActivatedMonsterEffect = false;
        private bool _exceptionalScheduleActivatedThisTurn = false;

        public _2026_CrystronTrainsExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.Citree, CardId.Smiger },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Citree, ActionType = ExecutorType.Activate, Description = "Play CardId.Citree" },
                    new() { CardId = CardId.Smiger, ActionType = ExecutorType.Activate, Description = "Extend with CardId.Smiger" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.Citree, CardId.Tristaros);
            BaitPlanner.RegisterBaitCards(CardId.Tristaros);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.Citree, CardId.Tristaros, CardId.Imperm);

            // 1. Hand traps & Counters (Highest priority)
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLock, DrollAndLockEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, () => SmartHandTrapChain() && DefaultAshBlossomAndJoyousSpring());
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.Imperm, ImpermEffect);

            // 2. Field and Search Spells (Continuous/Field setup)
            AddExecutor(ExecutorType.Activate, CardId.Inclusion, InclusionActivate);
            AddExecutor(ExecutorType.Activate, CardId.Switchyard, SwitchyardActivate);

            // 3. Main Starters & Summon Spells
            AddExecutor(ExecutorType.Activate, CardId.ExceptionalSchedule, ExceptionalScheduleEffect);
            AddExecutor(ExecutorType.Activate, CardId.UrgentSchedule, UrgentScheduleEffect);
            AddExecutor(ExecutorType.Summon, CardId.ScrapRecycler, ScrapRecyclerSummon);
            AddExecutor(ExecutorType.Activate, CardId.ScrapRecycler, ScrapRecyclerEffect);
            AddExecutor(ExecutorType.Summon, CardId.Smiger, SmigerSummon);
            AddExecutor(ExecutorType.Summon, CardId.Tristaros, TristarosSummon);
            AddExecutor(ExecutorType.Summon, CardId.ConvexKnight, ConvexKnightSummon);
            AddExecutor(ExecutorType.Summon, CardId.Citree, CitreeSummon);

            // 4. Crystron Engine Moves (Locks applied on field ignition)
            AddExecutor(ExecutorType.Activate, CardId.Sulfefnir, SulfefnirEffect);
            AddExecutor(ExecutorType.Activate, CardId.Sulfador, SulfadorEffect);
            AddExecutor(ExecutorType.Activate, CardId.Smiger, SmigerFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.Smiger, SmigerGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.Smiger, SmigerHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.Tristaros, TristarosEffect);

            // 5. Train / Earth Machine Engine Moves
            AddExecutor(ExecutorType.Activate, CardId.BleuTraveler, BleuTravelerEffect);
            AddExecutor(ExecutorType.Summon, CardId.BabelDecker, BabelDeckerSummon);
            AddExecutor(ExecutorType.Activate, CardId.BabelDecker, BabelDeckerEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BulletTrain, BulletTrainSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ConvexKnight, ConvexKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.ConvexKnight, ConvexKnightEffect);

            // 6. Link Bridges & Qliphort Genius
            AddExecutor(ExecutorType.SpSummon, CardId.ClockworkKnight, ClockworkKnightSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.QliphortGenius, GeniusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AngerKnuckle, AngerKnuckleSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AngerKnuckle, AngerKnuckleEffect);

            // 7. Extra Deck Synchro & Xyz Boss Monsters (Lock-Sensitive)
            AddExecutor(ExecutorType.SpSummon, CardId.Legatia, LegatiaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DawnDragster, DawnDragsterSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Eleskeletus, EleskeletusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Quariongandrax, QuariongandraxSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RiverStormer, RiverStormerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RiverStormer, RiverStormerEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.FlyingLauncher, FlyingLauncherSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.FlyingLauncher, FlyingLauncherEffect);
            
            // Rank 10 / 11 Xyz OTK moves
            AddExecutor(ExecutorType.SpSummon, CardId.GustavMax, GustavMaxSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.GustavMax, GustavMaxEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperDora, SuperDoraSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperDora, SuperDoraEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.JuggernautLiebe, JuggernautLiebeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.JuggernautLiebe, JuggernautLiebeEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.GustavRocket, GustavRocketSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.GustavRocket, GustavRocketEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, ZeusSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Zeus, ZeusEffect);

            // 8. Therion & Traps
            AddExecutor(ExecutorType.SpSummon, CardId.Regulus, RegulusSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Regulus, RegulusEffect);
            AddExecutor(ExecutorType.Activate, CardId.Cluster, ClusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.BarrageBlast, BarrageBlastEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.Cluster, DefensiveSpellSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, DefensiveSpellSetCheck);
            // ForbiddenDroplet and Imperm are NOT set: they need to be in hand for preemptive targeting
            AddExecutor(ExecutorType.SpellSet, CardId.BarrageBlast, DefensiveSpellSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.UrgentSchedule, DefensiveSpellSetCheck);

            // Handtrap Setting (lowest priority - only if we have nothing else to summon/play and board is empty)
            AddExecutor(ExecutorType.MonsterSet, CardId.AshBlossom, HandtrapSetCheck);
            AddExecutor(ExecutorType.MonsterSet, CardId.DrollAndLock, HandtrapSetCheck);
            AddExecutor(ExecutorType.MonsterSet, CardId.MulcharmyFuwalos, HandtrapSetCheck);

            // 9. Reactives (Citree opponent-turn Synchro)
            AddExecutor(ExecutorType.Activate, CardId.Citree, CitreeOpponentTurnSynchro);
        }

        public override bool OnSelectHand()
        {
            // Crystron/Trains hybrid โ€” prefer going first for Citree opponent-turn Synchro
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _hasMachineSynchroLock = false;
            _hasMachineExtraDeckLock = false;
            _opponentActivatedMonsterEffect = false;
            _exceptionalScheduleActivatedThisTurn = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        private static readonly int[] AceCardIds = {
            CardId.Eleskeletus,
            CardId.Quariongandrax,
            CardId.Legatia,
            CardId.DawnDragster,
            CardId.GustavMax,
            CardId.SuperDora,
            CardId.JuggernautLiebe,
            CardId.FlyingLauncher,
            CardId.GustavRocket,
            CardId.Zeus,
            CardId.Regulus
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Any(id => card.IsCode(id));
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;

            if (c.Id == CardId.ConvexKnight) return 50;
            if (c.Id == CardId.BabelDecker) return 60;
            if (c.Id == CardId.BulletTrain) return 100;
            if (c.Id == CardId.ScrapRecycler) return 150;
            if (c.Id == CardId.Citree) return 200;
            if (c.Id == CardId.Tristaros) return 250;
            if (c.Id == CardId.Smiger) return 250;
            if (c.Id == CardId.Sulfefnir) return 300;
            if (c.Id == CardId.Sulfador) return 300;

            return base.GetMaterialPriority(c);
        }

        public override void OnChaining(int player, ClientCard card)
        {
            if (player == 1 && card != null && card.HasType(CardType.Monster) && Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                _opponentActivatedMonsterEffect = true;
            }
            base.OnChaining(player, card);
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

        private bool ImpermEffect()
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

        // --- Hardcore Hand Trap & Utility Logic ---

        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            // Activate Fuwalos if the opponent is doing special summons from deck/extra deck.
            return Duel.Player == 1 && Bot.GetFieldCount() == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
        }

        private bool DrollAndLockEffect()
        {
            if (!SmartHandTrapChain()) return false;
            // Activate Droll to stop searching.
            return Duel.Player == 1;
        }

        private bool ForbiddenDropletEffect()
        {
            // Droplet should only be activated to negate crucial opponent boss monsters.
            if (Duel.Player == 1)
            {
                // Check if we have at least one card to send as cost (excluding Droplet itself)
                bool hasCost = Bot.Hand.Count(c => c != Card) + Bot.MonsterZone.Count(c => c != null) + Bot.SpellZone.Count(c => c != null && c != Card) > 0;
                if (!hasCost) return false;

                return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.Level >= 5);
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            // TTT is active during our turn if opponent has activated a monster effect in our Main Phase.
            return Duel.Player == 0 && _opponentActivatedMonsterEffect && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
        }

        // --- Starters & Main Combo Spells ---

        private bool InclusionActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (ShouldDeferEngineSearch()) return false;

            // Guard: เธ–เนเธฒเธกเธต Boss Monster เธเธเธชเธเธฒเธกเนเธฅเธฐเธชเธฒเธกเธฒเธฃเธ– OTK เนเธ”เน เนเธกเนเธ•เนเธญเธเธเนเธเธซเธฒเน€เธเธดเนเธก
            if (Duel.MainPhase != null && Duel.MainPhase.CanBattlePhase)
            {
                int totalAtk = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && !c.IsDisabled())
                    .Sum(c => c.Attack);
                if (totalAtk >= Enemy.LifePoints) return false;
            }
            return true;
        }

        private bool SwitchyardActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (ShouldDeferEngineSearch()) return false;

            // Guard: เธ–เนเธฒเธกเธต Level 10 EARTH Machine Boss เธเธเธชเธเธฒเธกเนเธฅเธฐเธเธฃเนเธญเธก OTK เนเธกเนเธ•เนเธญเธเธเนเธเธซเธฒเน€เธเธดเนเธก
            if (Duel.MainPhase != null && Duel.MainPhase.CanBattlePhase)
            {
                int totalAtk = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && !c.IsDisabled())
                    .Sum(c => c.Attack);
                if (totalAtk >= Enemy.LifePoints) return false;
            }
            bool hasBoss = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() &&
                (c.IsCode(CardId.GustavMax) || c.IsCode(CardId.JuggernautLiebe) ||
                 c.IsCode(CardId.GustavRocket) || c.IsCode(CardId.SuperDora) ||
                 c.IsCode(CardId.FlyingLauncher)));
            if (hasBoss && Bot.GetMonsterCount() >= 3) return false;
            return true;
        }

        private bool ExceptionalScheduleEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card == null) return false;

            // Guard 1: เธญเธขเนเธฒ chain เธเนเธณเนเธ chain เน€เธ”เธตเธขเธงเธเธฑเธ
            if (Util.ChainContainsCard(CardId.ExceptionalSchedule)) return false;

            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (_exceptionalScheduleActivatedThisTurn) return false;
                if (ShouldDeferEngineSearch()) return false;

                // เธ•เนเธญเธเธกเธต Urgent Schedule เนเธ Deck เธ–เธถเธเธเธฐเธเธธเนเธกเธเนเธฒ activate
                bool hasTarget = GetRemainingCount(CardId.UrgentSchedule) > 0;
                if (!hasTarget) return false;

                // เธ–เนเธฒ OTK เนเธ”เนเนเธฅเนเธง เนเธกเนเธ•เนเธญเธเธเนเธเธซเธฒเน€เธเธดเนเธก
                int totalAtk = Bot.MonsterZone.Where(c => c != null && c.IsFaceup() && c.IsAttack() && !c.Attacked).Sum(c => c.Attack);
                if (Duel.MainPhase != null && Duel.MainPhase.CanBattlePhase && totalAtk >= Enemy.LifePoints) return false;

                _exceptionalScheduleActivatedThisTurn = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // เน€เธญเธเน€เธเธเธ•เนเนเธเธชเธธเธชเธฒเธ (Banish เน€เธเธทเนเธญเน€เธเนเธเธกเธญเธเธชเน€เธ•เธญเธฃเนเน€เธเธฃเธทเนเธญเธเธเธฑเธเธฃเน€เธฅเน€เธงเธฅ 10)
                // เธ•เนเธญเธเธกเธตเธกเธญเธเธชเน€เธ•เธญเธฃเนเน€เธฅเน€เธงเธฅ 10 เน€เธเนเธฒเน€เธเธฃเธทเนเธญเธเธเธฑเธเธฃเนเธเธชเธธเธชเธฒเธเนเธซเนเน€เธฅเธทเธญเธเธเธฃเธดเธ เน€เธเธทเนเธญเธเนเธญเธเธเธฑเธเธเธฒเธฃเน€เธชเธเธเนเธณเนเธกเนเธชเธดเนเธเธชเธธเธ”
                bool hasLv10MachineInGY = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Level == 10 && c.HasRace(CardRace.Machine));
                if (!hasLv10MachineInGY) return false;

                return true;
            }

            return false;
        }

        private bool UrgentScheduleEffect()
        {
            // Urgent Schedule requires the opponent to control more monsters.
            return Enemy.GetMonsterCount() > Bot.GetMonsterCount();
        }

        private bool ScrapRecyclerSummon()
        {
            // Recycler is versatile enough to always be useful
            return true;
        }

        private bool ScrapRecyclerEffect()
        {
            // Recycler dumps machine monsters to GY.
            // Only activate if there's at least one remaining target in deck that we want
            bool hasSulfefnir = GetRemainingCount(CardId.Sulfefnir) > 0;
            bool hasBleuTraveler = GetRemainingCount(CardId.BleuTraveler) > 0;
            bool hasSulfador = GetRemainingCount(CardId.Sulfador) > 0;
            bool hasSmiger = GetRemainingCount(CardId.Smiger) > 0;

            // If we already have good cards in GY, skip
            bool hasCrystronInGy = Bot.Graveyard.Any(c => c != null && IsCrystronCard(c) && c.IsMonster());

            // Prioritize based on engine priority
            EnginePriority engine = GetPreferredEngine();
            if (engine == EnginePriority.Crystron && hasSulfefnir) return true;
            if (engine == EnginePriority.Trains && hasBleuTraveler) return true;

            // Default: dump Sulfefnir if available, otherwise BleuTraveler
            if (hasSulfefnir || hasBleuTraveler || hasSulfador || hasSmiger) return true;

            return false;
        }

        // --- Summon Conditions ---

        private bool SmigerSummon()
        {
            // Smiger is a key Crystron starter โ€” always worth normal summoning if we have Crystron plays
            if (ShouldPrioritizeTrains()) return false;
            return true;
        }

        private bool TristarosSummon()
        {
            // Tristaros is mainly GY fuel โ€” only summon if we need a body and have no better normal
            if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup())) return false; // Already have a monster
            if (ShouldPrioritizeTrains()) return false;
            return true;
        }

        private bool ConvexKnightSummon()
        {
            // ConvexKnight is a Tuner that searches on summon โ€” good for Crystron or general use
            if (ShouldPrioritizeTrains())
            {
                // Need EARTH Machine in Deck to search
                return GetRemainingCount(CardId.ScrapRecycler) > 0 ||
                       GetRemainingCount(CardId.BleuTraveler) > 0 ||
                       GetRemainingCount(CardId.BulletTrain) > 0 ||
                       GetRemainingCount(CardId.BabelDecker) > 0;
            }
            return true;
        }

        private bool CitreeSummon()
        {
            // Citree is the key Crystron tuner for opponent-turn synchro plays
            // Only normal summon if we have a valid target in GY for opponent turn synchro
            bool hasGyTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && !c.HasType(CardType.Tuner) &&
                (c.Level == 10 || c.Level == 5));
            if (!hasGyTarget && Duel.Player == 0) return false; // No GY target for synchro
            if (ShouldPrioritizeTrains()) return false;
            return true;
        }

        // --- Crystron Engine Movements ---

        private bool SulfefnirEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                // Requires at least 1 other Crystron card in hand to discard
                return Bot.Hand.Any(c => c != null && c != Card && IsCrystronCard(c));
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // BUG FIX: เธ•เธฃเธงเธเธงเนเธฒเธกเธต Crystron monster เนเธ Deck เธเนเธญเธ Self-pop
                // เธ–เนเธฒ Deck เธงเนเธฒเธเธเธฒเธ Crystron เธเธฒเธฃ self-pop เธเธฐเน€เธเนเธเธเธฒเธฃเน€เธชเธตเธขเธเธฒเธฃเนเธ”เธเธฃเธตเน
                bool hasCrystronInDeck = HasRemainingCrystronMonster();
                if (!hasCrystronInDeck) return false;
                
                AI.SelectCard(Card.Id);
                return true;
            }
            return false;
        }

        private bool SulfadorEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                // Must have a card on field to destroy
                if (Bot.GetMonsterCount() + Bot.GetSpellCount() == 0) return false;

                // BUG FIX: เธ•เธฃเธงเธเธงเนเธฒเธกเธต Crystron monster เนเธ Deck เนเธซเน Special Summon เธเนเธญเธเธ—เธณเธฅเธฒเธขเธเธฒเธฃเนเธ”เธ•เธฑเธงเน€เธญเธ
                bool hasCrystronInDeck = HasRemainingCrystronMonster();
                if (!hasCrystronInDeck) return false;

                // Select target to destroy: prioritize face-up Inclusion, then other face-up Crystron cards, then Scrap Recycler, then any card
                ClientCard target = Bot.SpellZone.FirstOrDefault(c => c != null && c.IsCode(CardId.Inclusion) && c.IsFaceup());
                if (target == null) target = Bot.MonsterZone.FirstOrDefault(c => c != null && IsCrystronCard(c) && c.IsFaceup() && c.Id != CardId.Citree);
                if (target == null) target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsCode(CardId.ScrapRecycler) && c.IsFaceup());
                if (target == null) target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup());
                if (target == null) target = Bot.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup() && c != Card);

                if (target != null)
                {
                    _hasMachineExtraDeckLock = true;
                    AI.SelectCard(target);
                    return true;
                }
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                return true; // Trigger on-summon deck dump
            }
            return false;
        }

        private bool SmigerFieldEffect()
        {
            // BUG FIX: เธ•เนเธญเธเน€เธเนเธ Location เธเธฑเธ”เน€เธเธเน€เธเธทเนเธญเธเนเธญเธเธเธฑเธ Double-Chain
            // เธ–เนเธฒ Smiger 2 เนเธ (Field + Grave) bot เธญเธฒเธ activate เธ—เธฑเนเธเธชเธญเธเนเธเธเนเธงเธ Chain เน€เธ”เธตเธขเธงเธเธฑเธ
            if (Card.Location != CardLocation.MonsterZone) return false;
            
            // BUG FIX: เธ•เนเธญเธเธกเธต Crystron เนเธ Deck เธ—เธตเน Synchro เนเธ”เนเธเนเธญเธเธ—เธณเธฅเธฒเธขเธเธฒเธฃเนเธ”เธ•เธฑเธงเน€เธญเธ
            bool canSynchro = HasRemainingCrystronMonster();
            if (!canSynchro) return false;
            
            // Select target to destroy: prioritize face-up Inclusion, then other face-up Crystron cards, then itself
            ClientCard target = Bot.SpellZone.FirstOrDefault(c => c != null && c.IsCode(CardId.Inclusion) && c.IsFaceup());
            if (target == null) target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.Id != CardId.Citree && IsCrystronCard(c) && c.IsFaceup());
            if (target == null) target = Card;
            
            if (target != null)
            {
                _hasMachineSynchroLock = true;
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SmigerGraveEffect()
        {
            // BUG FIX: เธ•เนเธญเธเน€เธเนเธ Location เธเธฑเธ”เน€เธเธเน€เธเธทเนเธญเธเนเธญเธเธเธฑเธ Double-Chain
            if (Card.Location != CardLocation.Grave) return false;

            // Check if there is a Crystron Spell/Trap in the Deck to search
            bool hasTarget = HasRemainingCrystronCard();
            if (!hasTarget) return false;

            // Don't search if we're going for Train OTK and already have our pieces
            if (ShouldPrioritizeTrains() && IsBoardStrongEnough()) return false;

            return true;
        }

        private bool TristarosEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                bool hasSynchro = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Synchro));
                int crystronCountInDeck = GetRemainingCount(CardId.Citree) +
                                         GetRemainingCount(CardId.Smiger) +
                                         GetRemainingCount(CardId.Sulfefnir) +
                                         GetRemainingCount(CardId.Sulfador);
                bool hasDeckTargets = crystronCountInDeck >= 2;
                if (!hasSynchro || !hasDeckTargets) return false;

                _hasMachineExtraDeckLock = true;
                return true;
            }

            // On-field quick effect: check if there's a valid Crystron target in the Deck to summon
            bool hasTarget = HasRemainingCrystronMonster();
            if (!hasTarget) return false;

            return true;
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        // --- Train / Earth Machine Moves ---

        private bool BleuTravelerEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (ShouldDeferEngineSearch()) return false;
                bool hasTarget = (GetRemainingCount(CardId.Switchyard) > 0 || GetRemainingCount(CardId.TrainConnection) > 0)
                              || Bot.Graveyard.Any(c => c != null && (c.IsCode(CardId.Switchyard) || c.IsCode(CardId.TrainConnection)));
                if (!hasTarget) return false;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                bool hasOtherEarthMachine = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth) && c.Id != Card.Id);
                if (!hasOtherEarthMachine) return false;

                _hasMachineExtraDeckLock = true;
                return true;
            }
            return true;
        }

        private bool BabelDeckerSummon()
        {
            return true;
        }

        private bool BabelDeckerEffect()
        {
            // On summon: Special Summon 1 EARTH Machine from hand
            bool hasHandTarget = Bot.Hand.Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth));
            // During Main Phase (Rank 10 Xyz summon using this card):
            bool hasExtraTarget = false; // ED pre-check skipped โ€” rank/race/attribute not serialized for face-down ED
            
            if (!hasHandTarget && !hasExtraTarget) return false;
            return true;
        }

        private bool BulletTrainSpSummon()
        {
            // Must control at least 1 monster, and all monsters we control must be EARTH Machine
            int count = Bot.MonsterZone.Count(c => c != null);
            if (count == 0) return false;
            return Bot.MonsterZone.All(c => c == null || (c.Attribute == (int)CardAttribute.Earth && c.Race == (int)CardRace.Machine));
        }

        private bool ConvexKnightSpSummon()
        {
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Machine))
                || Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Machine));
        }

        private bool ConvexKnightEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                bool hasMachineOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Machine))
                                      || Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Machine));
                if (!hasMachineOnField) return false;
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                bool hasDeckTarget = GetRemainingCount(CardId.ScrapRecycler) > 0 ||
                                     GetRemainingCount(CardId.BleuTraveler) > 0 ||
                                     GetRemainingCount(CardId.BulletTrain) > 0 ||
                                     GetRemainingCount(CardId.ConvexKnight) > 0 ||
                                     GetRemainingCount(CardId.BabelDecker) > 0;
                if (!hasDeckTarget) return false;
                return true;
            }
            return true;
        }

        // --- Crystron Hand Effect (after field effect, to prevent double activation) ---

        private bool SmigerHandEffect()
        {
            // Smiger's hand effect: reveal 1 Crystron monster to search Inclusion/Cluster
            if (Card.Location != CardLocation.Hand) return false;
            if (Util.ChainContainsCard(CardId.Smiger)) return false;

            bool hasCrystronToReveal = Bot.Hand.Any(c => c != null && c != Card && IsCrystronCard(c) && c.IsMonster());
            if (!hasCrystronToReveal) return false;

            bool hasTarget = GetRemainingCount(CardId.Inclusion) > 0 || GetRemainingCount(CardId.Cluster) > 0;
            if (!hasTarget) return false;

            // Don't search if we already have the card or if inclusion is already on field
            if (Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.Inclusion) && c.IsFaceup()))
            {
                // Only search Cluster if we don't have one
                if (GetRemainingCount(CardId.Cluster) == 0) return false;
            }

            return true;
        }

        // --- Link Bridges & Placements (Hardcore Logic) ---

        private bool ClockworkKnightSpSummon()
        {
            if (_hasMachineSynchroLock) return false;

            // Clockwork Knight is a crucial bridge to put Scrap Recycler or Citree in the GY to enable triggers!
            bool hasScrapRecyclerOnField = Bot.MonsterZone.Any(c => c != null && c.IsCode(CardId.ScrapRecycler));
            bool hasCitreeOnField = Bot.MonsterZone.Any(c => c != null && c.IsCode(CardId.Citree));

            if (hasScrapRecyclerOnField || hasCitreeOnField)
            {
                // Case 1: Bleu Traveler is in GY, but we have NO other EARTH Machine in GY to revive.
                // We need to send Scrap Recycler to the GY to activate Bleu Traveler!
                bool hasBleuTravelerInGY = Bot.Graveyard.Any(c => c.IsCode(CardId.BleuTraveler));
                bool hasOtherEarthMachineInGY = Bot.Graveyard.Any(c => c.IsMonster() && c.Attribute == (int)CardAttribute.Earth && c.Id != CardId.BleuTraveler);
                if (hasBleuTravelerInGY && !hasOtherEarthMachineInGY && hasScrapRecyclerOnField)
                {
                    return true;
                }

                // Case 2: Regulus is in hand, but we have NO Machine in the GY to equip.
                // We send Recycler or Citree to GY to enable Regulus.
                bool hasRegulusInHand = Bot.Hand.Any(c => c.IsCode(CardId.Regulus));
                bool hasMachineInGY = Bot.Graveyard.Any(c => c.IsMonster() && c.Race == (int)CardRace.Machine);
                if (hasRegulusInHand && !hasMachineInGY)
                {
                    return true;
                }
            }
            return false;
        }

        private bool GeniusSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // Link summon Genius if we have at least 2 machines on field.
            return Bot.MonsterZone.Count(c => c != null && c.IsMonster()) >= 2;
        }

        private bool AngerKnuckleSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // Anger Knuckle is good for graveyard recycling. Summon if we have excess materials.
            return Bot.MonsterZone.Count(c => c != null && c.IsMonster()) >= 2 && Duel.Phase == DuelPhase.Main2;
        }

        private bool AngerKnuckleEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                bool hasLv10MachineInGY = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Level == 10 && c.HasRace(CardRace.Machine));
                bool canSend = Bot.Hand.Any(c => c != Card) || Bot.GetMonsters().Any(c => c != Card);
                if (!hasLv10MachineInGY || !canSend) return false;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                bool canSend = Bot.Hand.Count > 0 || Bot.GetMonsters().Count > 0 || Bot.GetSpells().Count > 0;
                if (!canSend) return false;
                return true;
            }
            return true;
        }

        // --- Extra Deck Synchro & Xyz Boss Monsters ---

        private bool LegatiaSpSummon()
        {
            // Legatia (Level 12, Machine Synchro) โ€” big negate + destroy
            // Only summon when opponent has a threat or we need the destruction
            if (ShouldPrioritizeTrains() && !_hasMachineExtraDeckLock) return false; // Save materials for Train Xyz
            if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
            return !_hasMachineExtraDeckLock;
        }

        private bool DawnDragsterSpSummon()
        {
            // Dawn Dragster (Level 7, Machine Synchro) โ€” omni-negate
            // Summon when we need omni-negate or opponent has threatening spells
            if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;

            if (ShouldPrioritizeTrains() && Bot.MonsterZone.Count(c => c != null && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2)
                return false; // Save materials for Train Xyz

            return !_hasMachineExtraDeckLock;
        }

        private bool EleskeletusSpSummon()
        {
            // Eleskeletus (Level 7, Machine Synchro) โ€” recycle banished Crystrons
            // Only blocked by Machine Extra Deck lock (NOT by Machine Synchro lock, since Eleskeletus IS a Machine Synchro)
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            if (ShouldPrioritizeTrains()) return false;
            return !_hasMachineExtraDeckLock;
        }

        private bool QuariongandraxSpSummon()
        {
            // Quariongandrax (Level 9, Machine Synchro) โ€” banish opponent monsters
            // Only summon if opponent has meaningful monsters to banish
            if (!OpponentHasThreateningMonster() && Enemy.GetMonsterCount() < 2) return false;
            if (ShouldPrioritizeTrains()) return false;
            return !_hasMachineExtraDeckLock;
        }

        private bool RiverStormerSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // Summon River Stormer if we have two Level 5 monsters (e.g. Sulfefnir, Sulfador).
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 5 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool RiverStormerEffect()
        {
            // Search/Dump Earth Machine โ€” only activate if we need the search target
            EnginePriority engine = GetPreferredEngine();
            if (engine == EnginePriority.Crystron)
            {
                // Search Crystron cards
                return HasRemainingCrystronMonster() || HasRemainingCrystronCard();
            }
            if (engine == EnginePriority.Trains)
            {
                // Search Train/Earth Machine cards
                return GetRemainingCount(CardId.ScrapRecycler) > 0 ||
                       GetRemainingCount(CardId.BleuTraveler) > 0 ||
                       GetRemainingCount(CardId.BulletTrain) > 0 ||
                       GetRemainingCount(CardId.ConvexKnight) > 0 ||
                       GetRemainingCount(CardId.BabelDecker) > 0;
            }
            return true;
        }

        private bool FlyingLauncherSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool FlyingLauncherEffect()
        {
            // Search effect on Xyz Summon
            bool hasSearchTarget = GetRemainingCount(CardId.BarrageBlast) > 0 ||
                                   GetRemainingCount(CardId.ScrapRecycler) > 0 ||
                                   GetRemainingCount(CardId.BleuTraveler) > 0 ||
                                   GetRemainingCount(CardId.BulletTrain) > 0 ||
                                   GetRemainingCount(CardId.ConvexKnight) > 0 ||
                                   GetRemainingCount(CardId.BabelDecker) > 0;
            // Detach and destroy Spell/Trap
            bool hasSpellTrapOnField = Bot.GetSpells().Any() || Enemy.GetSpells().Any();
            bool hasMaterials = Card.Overlays.Count > 0;
            
            if (!hasSearchTarget && !(hasSpellTrapOnField && hasMaterials)) return false;
            return true;
        }

        private bool GustavMaxSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // Do not summon Gustav Max on Turn 1 (since we cannot attack)
            if (Duel.Turn == 1) return false;

            // Only summon if we can burn for game or overlay into Juggernaut Liebe to OTK.
            bool enemyHasFewLP = Enemy.LifePoints <= 2000;
            bool canSummonLiebe = GetRemainingCount(CardId.JuggernautLiebe) > 0;
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2 && (enemyHasFewLP || canSummonLiebe);
        }

        private bool GustavMaxEffect()
        {
            // Detach to burn 2000 โ€” only burn if going for game or clearing LP for Liebe
            if (Enemy.LifePoints <= 2000) return true; // Win the game
            if (Enemy.LifePoints <= 4000 && GetRemainingCount(CardId.JuggernautLiebe) > 0) return true; // Burn + Liebe OTK
            return false;
        }

        private bool SuperDoraSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // Summon Dora for defensive plays during turn 1 or if we cannot OTK.
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool SuperDoraEffect()
        {
            // Protect our strongest monster on the field.
            ClientCard target = Bot.MonsterZone.Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool JuggernautLiebeSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // Liebe overlays on top of Gustav Max or Super Dora.
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.GustavMax) || c.IsCode(CardId.SuperDora)));
        }

        private bool JuggernautLiebeEffect()
        {
            // Liebe's ATK pump locks other monsters from declaring attacks.
            // Only activate if we are in Main Phase 2 (already attacked) or if we have no other ready attackers.
            if (Duel.Phase == DuelPhase.Main1)
            {
                bool hasOtherAttackers = Bot.GetMonsters().Any(m => m != null && m.Id != CardId.JuggernautLiebe && m.IsFaceup() && m.IsAttack() && !m.Attacked);
                if (hasOtherAttackers) return false;
            }
            return true;
        }

        private bool GustavRocketSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // Requires 2 Level 10 non-Xyz monsters on the field
            return Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool GustavRocketEffect()
        {
            // Only activate if we can clear the field for OTK
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 1000 && IsTargetable(c)))
                return true;
            return false;
        }

        private bool ZeusSpSummon()
        {
            // Zeus requires NO locks of either type.
            return !_hasMachineSynchroLock && !_hasMachineExtraDeckLock && 
                   Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.Attacked);
        }

        private bool ZeusEffect()
        {
            // Always chain Zeus if it is being targeted or threatened by an opponent's card effect
            if (Util.IsChainTarget(Card)) return true;
            
            // Otherwise, only wipe if opponent has a significant presence, and they have more cards than us, 
            // OR if the opponent has threatening cards and we don't mind losing our field.
            int ourCards = Bot.GetMonsterCount() + Bot.GetSpellCount();
            int enemyCards = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            return enemyCards >= 3 && enemyCards > ourCards;
        }

        private bool RegulusSpSummon()
        {
            // Summon Regulus by targeting a Machine in GY.
            // Requires an open Spell/Trap zone to equip the target.
            return Bot.GetSpellCount() < 5 && Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Race == (int)CardRace.Machine);
        }

        private bool RegulusEffect()
        {
            // Only negate on opponent's turn or if opponent has a threatening monster
            if (Duel.Player == 1) return true; // Opponent's turn โ€” negate freely
            if (Duel.Player == 0 && OpponentHasThreateningMonster()) return true;
            return false;
        }

        // --- Board State Evaluation ---

        protected override bool IsBoardStrongEnough()
        {
            // Have strong boss monsters on field
            if (Bot.HasInMonstersZone(CardId.SuperDora) || Bot.HasInMonstersZone(CardId.JuggernautLiebe) ||
                Bot.HasInMonstersZone(CardId.GustavRocket) || Bot.HasInMonstersZone(CardId.FlyingLauncher) ||
                Bot.HasInMonstersZone(CardId.DawnDragster))
                return true;

            // Have Citree on field for opponent turn synchro play + other disruption
            if (Bot.HasInMonstersZone(CardId.Citree) && Bot.GetMonsterCount() >= 2)
                return true;

            // Multiple Xyz monsters + protection
            int xyzCount = Bot.MonsterZone.Count(c => c != null && c.HasType(CardType.Xyz));
            if (xyzCount >= 2) return true;

            return false;
        }

        protected override bool IsInGrindGame()
        {
            int handCount = Bot.GetHandCount();
            int monsterCount = Bot.GetMonsterCount();
            int backrowCount = Bot.GetSpells().Count(c => c != null && c.IsFacedown());
            return handCount <= 1 && monsterCount <= 1 && backrowCount == 0;
        }

        protected override bool NeedsBoardPresence()
        {
            int ourFaceup = Bot.MonsterZone.Count(c => c != null && c.IsFaceup());
            int enemyMonsters = Enemy.GetMonsterCount();
            return ourFaceup == 0 || (ourFaceup <= 1 && enemyMonsters >= 2);
        }

        private bool OpponentHasThreateningMonster()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 3000 || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
        }

        // --- Engine Priority Detection ---

        private enum EnginePriority
        {
            Crystron,  // Better when we have Crystron starters (Smiger, Sulfefnir, Citree)
            Trains,    // Better when we have Train/Earth Machine starters (Switchyard, ScrapRecycler, BleuTraveler)
            Both,      // Equal availability
            None       // No clear engine available
        }

        private EnginePriority GetPreferredEngine()
        {
            bool hasCrystronStarter = Bot.Hand.Any(c => c != null && IsCrystronCard(c) && c.IsMonster() &&
                !c.IsCode(CardId.Tristaros)); // Tristaros needs a Synchro target first
            bool hasCrystronField = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && IsCrystronCard(c));
            bool hasCrystronInDeck = HasRemainingCrystronMonster();

            bool hasTrainStarter = Bot.Hand.Any(c => c != null && (
                c.IsCode(CardId.Switchyard) || c.IsCode(CardId.ExceptionalSchedule) ||
                c.IsCode(CardId.ScrapRecycler) || c.IsCode(CardId.UrgentSchedule)));
            bool hasTrainField = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && !IsCrystronCard(c) &&
                c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth));

            bool hasCrystron = (hasCrystronStarter || hasCrystronField) && hasCrystronInDeck;
            bool hasTrains = hasTrainStarter || hasTrainField;

            // Turn 1: prefer Crystron for defensive opponent-turn synchro setup
            if (Duel.Turn == 1 && Duel.Player == 0)
            {
                if (hasCrystron) return EnginePriority.Crystron;
                if (hasTrains) return EnginePriority.Trains;
                return EnginePriority.None;
            }

            // Going for OTK: prefer Trains (GustavMax burn + Liebe attack)
            if (Duel.MainPhase != null && Duel.MainPhase.CanBattlePhase)
            {
                int totalAtk = Bot.MonsterZone.Where(c => c != null && c.IsFaceup() && c.IsAttack() && !c.Attacked)
                    .Sum(c => c.Attack);
                if (totalAtk >= Enemy.LifePoints) return EnginePriority.None; // Already lethal

                if (hasTrains && Enemy.LifePoints <= 8000) return EnginePriority.Trains;
            }

            if (hasCrystron && hasTrains) return EnginePriority.Both;
            if (hasCrystron) return EnginePriority.Crystron;
            if (hasTrains) return EnginePriority.Trains;
            return EnginePriority.None;
        }

        private bool ShouldPrioritizeCrystron()
        {
            EnginePriority engine = GetPreferredEngine();
            return engine == EnginePriority.Crystron || engine == EnginePriority.Both;
        }

        private bool ShouldPrioritizeTrains()
        {
            EnginePriority engine = GetPreferredEngine();
            return engine == EnginePriority.Trains;
        }

        private bool ClusterEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                // Field effect: Summon 1 Crystron from hand/GY, then destroy 1 card we control
                bool hasSummonTarget = Bot.Hand.Concat(Bot.Graveyard).Any(c => c != null && IsCrystronCard(c) && c.IsMonster());
                if (!hasSummonTarget) return false;

                // Make sure we have a card we are willing to destroy (excluding Cluster itself)
                bool hasPopTarget = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && IsCrystronCard(c)) 
                                 || Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Inclusion) && c != Card);
                return hasPopTarget;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // GY effect: Banish, target 1 Crystron we control, dump 1 Crystron with different level from Deck
                bool hasFaceupCrystron = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && IsCrystronCard(c));
                if (!hasFaceupCrystron) return false;

                bool hasDeckTarget = HasRemainingCrystronMonster();
                return hasDeckTarget;
            }
            return false;
        }

        // --- Citree Opponent Turn Synchro Logic ---

        private bool CitreeOpponentTurnSynchro()
        {
            if (Duel.Player == 0) return false; // Opponent turn only
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2 && Duel.Phase != DuelPhase.Battle) return false;

            // Legatia (Level 12) setup: Citree (2) + Level 10 non-Tuner = Legatia (12)
            ClientCard lvl10 = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.Level == 10 && !c.HasType(CardType.Tuner));
            if (lvl10 != null && GetRemainingCount(CardId.Legatia) > 0)
            {
                AI.SelectCard(lvl10);
                return true;
            }

            // Note: Quariongandrax (Level 9) requires 2+ Tuners, so it cannot be Synchro Summoned using only Citree (1 Tuner) and a Level 7 target.
            // Since we have no Level 7 monsters in the Main Deck, the Level 7/Quariongandrax setup has been removed.

            // Eleskeletus / Dawn Dragster (Level 7) setup: Citree (2) + Level 5 non-Tuner = Level 7 Machine Synchro
            ClientCard lvl5 = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.Level == 5 && !c.HasType(CardType.Tuner));
            if (lvl5 != null && (GetRemainingCount(CardId.DawnDragster) > 0 || GetRemainingCount(CardId.Eleskeletus) > 0))
            {
                AI.SelectCard(lvl5);
                return true;
            }

            return false;
        }

        private bool HandtrapSetCheck()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool DefensiveSpellSetCheck()
        {
            if (Duel.Player != 0) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (Bot.GetSpellCountWithoutField() >= 5) return false;

            // Keep reactive cards live, but put them on board before ending so they don't clog hand.
            if (Duel.Phase == DuelPhase.Main2) return true;

            bool hasEngineAction = Duel.MainPhase != null &&
                (Duel.MainPhase.SummonableCards.Any(c => c != null && !IsReactiveHandTrap(c)) ||
                 Duel.MainPhase.SpecialSummonableCards.Any(c => c != null) ||
                 Duel.MainPhase.ActivableCards.Any(c => c != null && c != Card && !IsReactiveOnlySpell(c)));
            if (hasEngineAction) return false;

            return Duel.MainPhase == null || !Duel.MainPhase.CanBattlePhase || !Bot.HasAttackingMonster();
        }

        private bool ShouldDeferEngineSearch()
        {
            if (Scorer != null && Scorer.HasLethal()) return true;
            return false;
        }

        private static bool IsReactiveHandTrap(ClientCard card)
        {
            return card.IsCode(CardId.AshBlossom) || card.IsCode(CardId.DrollAndLock) || card.IsCode(CardId.MulcharmyFuwalos);
        }

        private static bool IsReactiveOnlySpell(ClientCard card)
        {
            return card.IsCode(CardId.CalledByTheGrave) || card.IsCode(CardId.ForbiddenDroplet) ||
                   card.IsCode(CardId.Imperm) || card.IsCode(CardId.Cluster) || card.IsCode(CardId.BarrageBlast);
        }

        // --- Targeting Selection Override ---

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (Card != null)
            {
                // 1. Scrap Recycler dump targets (engine-priority based)
                if (Card.Id == CardId.ScrapRecycler)
                {
                    EnginePriority engine = GetPreferredEngine();
                    ClientCard target = null;

                    if (engine == EnginePriority.Crystron || engine == EnginePriority.Both)
                    {
                        // Crystron priority: Sulfefnir -> Smiger -> Sulfador -> BleuTraveler
                        target = cards.FirstOrDefault(c => c.Id == CardId.Sulfefnir);
                        if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.Smiger);
                        if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.Sulfador);
                        if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.BleuTraveler);
                    }
                    else
                    {
                        // Train priority: BleuTraveler -> Sulfefnir (fodder) -> Smiger -> Sulfador
                        target = cards.FirstOrDefault(c => c.Id == CardId.BleuTraveler);
                        if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.Sulfefnir);
                        if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.Smiger);
                        if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.Sulfador);
                    }

                    // Fallback: any Monster card in the selection
                    if (target == null) target = cards.FirstOrDefault(c => c.IsMonster());
                    if (target != null) return new List<ClientCard> { target };
                }

                // 2. Revolving Switchyard discard search targets (engine-priority based)
                if (Card.Id == CardId.Switchyard && cards.Any(c => c.Location == CardLocation.Deck))
                {
                    EnginePriority engine = GetPreferredEngine();
                    ClientCard target = null;

                    if (engine == EnginePriority.Crystron || engine == EnginePriority.Both)
                    {
                        // For Crystron: prefer a Level 5 non-Tuner Earth Machine (for Citree synchro into Level 7)
                        // BleuTraveler (Lv5) is ideal for Citree synchro
                        target = cards.FirstOrDefault(c => c.Id == CardId.BleuTraveler);
                    }
                    else
                    {
                        // For Trains: prefer BulletTrain (Lv10, self-SS) for Rank 10 Xyz
                        target = cards.FirstOrDefault(c => c.Id == CardId.BulletTrain);
                        if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.BleuTraveler);
                    }

                    // Fallback
                    if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.BabelDecker);
                    if (target != null) return new List<ClientCard> { target };
                }

                // 3. Urgent Schedule targets from deck
                if (Card.Id == CardId.UrgentSchedule)
                {
                    var result = new List<ClientCard>();
                    // Select one Level 4 or lower EARTH Machine
                    ClientCard lvl4 = cards.FirstOrDefault(c => c.IsMonster() && c.Level <= 4 && c.Attribute == (int)CardAttribute.Earth);
                    // Select one Level 5 or higher EARTH Machine
                    ClientCard lvl5 = cards.FirstOrDefault(c => c.IsMonster() && c.Level >= 5 && c.Attribute == (int)CardAttribute.Earth);

                    if (lvl4 != null) result.Add(lvl4);
                    if (lvl5 != null) result.Add(lvl5);
                    if (result.Count >= min && result.Count <= max) return result;
                }

                // 4. Qliphort Genius search targets
                if (Card.Id == CardId.QliphortGenius)
                {
                    // Search Regulus
                    ClientCard target = cards.FirstOrDefault(c => c.Id == CardId.Regulus);
                    if (target != null) return new List<ClientCard> { target };
                }

                // 4.1. Smiger hand effect: search Inclusion or Cluster
                if (Card.Id == CardId.Smiger && cards.Any(c => c.Location == CardLocation.Deck))
                {
                    // Priority: Inclusion (if we don't have it) -> Cluster
                    bool hasInclusionOnField = Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.Inclusion));
                    bool hasInclusionInHand = Bot.Hand.Any(c => c.IsCode(CardId.Inclusion) && c != Card);
                    
                    if (!hasInclusionOnField && !hasInclusionInHand)
                    {
                        ClientCard target = cards.FirstOrDefault(c => c.Id == CardId.Inclusion);
                        if (target != null) return new List<ClientCard> { target };
                    }
                    
                    ClientCard clusterTarget = cards.FirstOrDefault(c => c.Id == CardId.Cluster);
                    if (clusterTarget != null) return new List<ClientCard> { clusterTarget };
                }

                // 4.2. Inclusion search targets
                if (Card.Id == CardId.Inclusion && cards.Any(c => c.Location == CardLocation.Deck))
                {
                    EnginePriority engine = GetPreferredEngine();
                    if (engine == EnginePriority.Crystron || engine == EnginePriority.Both)
                    {
                        ClientCard target = cards.FirstOrDefault(c => c.Id == CardId.Smiger);
                        if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.Sulfefnir);
                        if (target != null) return new List<ClientCard> { target };
                    }
                    if (engine == EnginePriority.Trains)
                    {
                        ClientCard target = cards.FirstOrDefault(c => c.Id == CardId.ScrapRecycler);
                        if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.ConvexKnight);
                        if (target != null) return new List<ClientCard> { target };
                    }
                }

                // 4.5. Therion "King" Regulus equip targets in GY
                if (Card.Id == CardId.Regulus)
                {
                    // Prioritize equipping Crystron monsters so they can be popped by Sulfador/Smiger
                    ClientCard target = cards.FirstOrDefault(c => IsCrystronCard(c) && c.IsMonster());
                    if (target == null) target = cards.FirstOrDefault(c => c.IsMonster());
                    if (target != null) return new List<ClientCard> { target };
                }

                // 6. Sulfefnir self-destruction target selection
                if (Card.Id == CardId.Sulfefnir && hint == 502)
                {
                    // Prioritize face-up Inclusion
                    ClientCard target = cards.FirstOrDefault(c => c.Id == CardId.Inclusion && c.Location == CardLocation.SpellZone && c.IsFaceup());
                    // Then other face-up Crystron cards (except Citree which we want to keep for opponent turn synchro)
                    if (target == null) target = cards.FirstOrDefault(c => IsCrystronCard(c) && c.Location == CardLocation.MonsterZone && c.IsFaceup() && c.Id != CardId.Citree && c.Id != CardId.Sulfefnir);
                    // Then Scrap Recycler
                    if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.ScrapRecycler && c.Location == CardLocation.MonsterZone && c.IsFaceup());
                    // Fallback to Sulfefnir itself
                    if (target == null) target = cards.FirstOrDefault(c => c.Id == CardId.Sulfefnir && c.Location == CardLocation.MonsterZone);
                    // Absolute fallback to any card in the selection list
                    if (target == null) target = cards.FirstOrDefault();

                    if (target != null) return new List<ClientCard> { target };
                }

                // 5. Eleskeletus recycle targets
                if (Card.Id == CardId.Eleskeletus)
                {
                    // Target face-up banished Crystron Inclusion
                    ClientCard target = cards.FirstOrDefault(c => c.Id == CardId.Inclusion);
                    if (target != null) return new List<ClientCard> { target };
                }
            }

            // Generic fallback sorting for discard / send-to-GY costs
            if (max > 0 && cards.All(c => c.Location == CardLocation.Hand || c.Location == CardLocation.MonsterZone || c.Location == CardLocation.SpellZone))
            {
                var sorted = cards.OrderBy(GetDiscardPriority).ToList();
                var result = new List<ClientCard>();
                for (int i = 0; i < Math.Min(max, sorted.Count); i++)
                {
                    result.Add(sorted[i]);
                }
                if (result.Count >= min)
                {
                    return result;
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool OnSelectYesNo(long desc)
        {
            if (desc == CardId.Inclusion || desc == Util.GetStringId(CardId.Inclusion, 0))
            {
                return true; // Yes to search on activation
            }
            return base.OnSelectYesNo(desc);
        }

        private bool IsTargetable(ClientCard card)
        {
            if (card == null) return false;
            return !card.IsShouldNotBeTarget();
        }

        private bool BarrageBlastEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: only activate if opponent has a target worth destroying
                if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2000)) return true;
                if (Enemy.GetSpells().Any(c => c != null && c.IsFaceup())) return true;
                return false;
            }

            if (Card.Location == CardLocation.SpellZone)
            {
                if (Card.IsFacedown())
                {
                    return true;
                }

                var xyzMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.HasRace(CardRace.Machine) && c.Overlays.Count > 0).ToList();
                if (xyzMonsters.Count == 0) return false;

                var enemyTargets = Enemy.GetMonsters().Concat(Enemy.GetSpells()).Where(c => c != null && IsTargetable(c)).ToList();
                if (enemyTargets.Count == 0) return false;

                ClientCard target = enemyTargets.FirstOrDefault(c => c.IsFaceup() && c.IsFloodgate())
                    ?? enemyTargets.FirstOrDefault(c => c.IsFaceup() && c.Attack >= 2500)
                    ?? enemyTargets.FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return false;
        }

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0)
                return 0;

            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 20;
                if (cardId == 0 && Card != null)
                {
                    cardId = Card.Id;
                }
                long optIndex = options[i] & 0xfffff;

                if (cardId == CardId.TripleTacticsTalent)
                {
                    if (Bot.GetHandCount() <= 2)
                    {
                        if (optIndex == 0) return i;
                    }
                    if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500))
                    {
                        if (optIndex == 1) return i;
                    }
                    if (optIndex == 0) return i;
                }
            }

            return base.OnSelectOption(options);
        }

        // --- Helper functions for robust Crystron checks and discard sorting ---

        private static bool IsCrystronCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.Citree ||
                   card.Id == CardId.Tristaros ||
                   card.Id == CardId.Smiger ||
                   card.Id == CardId.Sulfefnir ||
                   card.Id == CardId.Sulfador ||
                   card.Id == CardId.Cluster ||
                   card.Id == CardId.Inclusion ||
                   card.HasSetcode(0xea);
        }

        private bool HasRemainingCrystronMonster()
        {
            return GetRemainingCount(CardId.Citree) > 0 ||
                   GetRemainingCount(CardId.Tristaros) > 0 ||
                   GetRemainingCount(CardId.Smiger) > 0 ||
                   GetRemainingCount(CardId.Sulfefnir) > 0 ||
                   GetRemainingCount(CardId.Sulfador) > 0 ||
                   HasRemainingMonsterWithSetcode(0xea);
        }

        private bool HasRemainingCrystronCard()
        {
            return GetRemainingCount(CardId.Citree) > 0 ||
                   GetRemainingCount(CardId.Tristaros) > 0 ||
                   GetRemainingCount(CardId.Smiger) > 0 ||
                   GetRemainingCount(CardId.Sulfefnir) > 0 ||
                   GetRemainingCount(CardId.Sulfador) > 0 ||
                   GetRemainingCount(CardId.Cluster) > 0 ||
                   GetRemainingCount(CardId.Inclusion) > 0 ||
                   HasRemainingCardWithSetcode(0xea);
        }

        private int GetDiscardPriority(ClientCard card)
        {
            if (card == null) return int.MaxValue;
            
            // Highest priority: cards we explicitly want in the GY
            if (card.Id == CardId.BleuTraveler && card.Location == CardLocation.Hand) return 1;
            if (card.Id == CardId.Sulfefnir && card.Location == CardLocation.Hand) return 2;
            if (card.Id == CardId.Smiger && card.Location == CardLocation.Hand) return 3;
            if (card.Id == CardId.Sulfador && card.Location == CardLocation.Hand) return 4;
            if (card.Id == CardId.Tristaros && card.Location == CardLocation.Hand) return 5;
            if (card.Id == CardId.BulletTrain && card.Location == CardLocation.Hand) return 6;
            
            // Medium priority: other monsters or spells we don't mind discarding
            if (card.Id == CardId.Cluster) return 10;
            if (card.Id == CardId.Inclusion && Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.Inclusion))) return 11; // extra copy
            if (card.Id == CardId.Switchyard && Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.Switchyard))) return 12; // extra copy
            if (card.Id == CardId.ConvexKnight) return 15;
            
            // Low priority: key combo pieces we'd rather summon/activate
            if (card.Id == CardId.ScrapRecycler) return 50;
            if (card.Id == CardId.Citree) return 60;
            
            // Lowest priority: reactive handtraps and crucial defense spells/traps
            if (card.Id == CardId.CalledByTheGrave) return 100;
            if (card.Id == CardId.ForbiddenDroplet) return 101;
            if (card.Id == CardId.AshBlossom) return 102;
            if (card.Id == CardId.MulcharmyFuwalos) return 103;
            if (card.Id == CardId.DrollAndLock) return 104;
            if (card.Id == CardId.Imperm) return 105;
            
            return 80; // default for other cards
        }
    }
}
