// ============================================================================
// CARD AUDIT — 2026_CrystronTrains (Crystron & Heavy Industrial Earth Machine Hybrid)
// ============================================================================
// | Card Name                          | Type         | OPT? | HOPT? | Cost    | Effect Summary                                | Activate When                                | NEVER Activate When                         |
// |------------------------------------|--------------|------|-------|---------|-----------------------------------------------|----------------------------------------------|---------------------------------------------|
// | Crystron Citree                    | Monster L2 T | Yes  | Yes   | Target  | Opponent turn: Target non-Tuner in GY, Synchro| Opponent MP/BP, valid non-Tuner in GY        | Bot turn without targets                    |
// | Crystron Tristaros                 | Monster L2 T | Yes  | Yes   | None    | Quick SS Crystron from Dk; GY float on Synchro| Opponent turn or field setup                 | Deck empty of Crystrons                     |
// | Crystron Smiger                    | Monster L3   | Yes  | Yes   | Pop/Ban | Pop face-up card -> SS Tuner; GY search S/T   | Have card to pop; in GY to search S/T        | No Crystron Tuner in deck                   |
// | Crystron Sulfefnir                 | Monster L5   | Yes  | Yes   | Discard | Discard Crystron -> SS hand/GY; self-pop float| In hand/GY with discard fodder; float on pop | No Crystron in deck                         |
// | Crystron Sulfador                  | Monster L5   | Yes  | Yes   | Pop     | Pop card to SS from hand/GY; dump Crystron Dk | Have card to pop and targets in deck         | No targets in deck                          |
// | Scrap Recycler                     | Monster L3   | No   | No    | None    | On NS/SS: Dump 1 Machine from Deck to GY      | Normal or Special Summoned                   | Deck empty of target Machines               |
// | Noctilucent Train Bleu Traveler    | Monster L10  | Yes  | Yes   | Discard | Discard -> Search Switchyard; GY SS 2 Machines| In hand to search; in GY to revive 2 L10s     | Once per duel revival already used          |
// | Super Express Bullet Train         | Monster L10  | Yes  | Yes   | None    | SS if control only EARTH Machine; End recycle | Have EARTH Machine on field; GY recycle EP    | Non-EARTH Machine on field                  |
// | Convex Knight                      | Monster L4   | Yes  | Yes   | None    | SS in DEF if Machine on field; dump EARTH Mach| Machine on field; Main Phase dump extender   | No EARTH Machines in deck                   |
// | Heavy Knight Babel Decker          | Monster L10  | Yes  | Yes   | None    | NS w/o tribute; SS EARTH Mach; 1-card Rank 10 | Hand extender; Rank 10 Xyz if opp used effect | No targets in hand/ED                       |
// | Therion "King" Regulus             | Monster L8   | Yes  | Yes   | Equip   | SS by equipping Machine; Quick Omni-negate    | Machine in GY; Quick negate opponent effect   | No Machine in GY                            |
// | Mulcharmy Fuwalos                  | Monster L4   | Yes  | Yes   | Discard | Handtrap: Draw on opp Deck/Extra Deck SS      | Opponent turn, bot controls 0 cards          | Bot controls cards                          |
// | Ash Blossom & Joyous Spring        | Monster L3 T | Yes  | Yes   | Discard | Handtrap: Negate deck search/dump/SS          | Opponent activates deck-interacting effect    | Bot's own turn without threats              |
// | Droll & Lock Bird                  | Monster L1   | Yes  | Yes   | Send    | Handtrap: Prevent adding cards from Deck      | Opponent added cards from Deck                | Hand already locked                         |
// | Forbidden Droplet                  | Spell Quick  | No   | No    | Send    | Send cards to negate opp monsters & halve ATK | Opponent high-threat monster activates/battles| No expendable cost cards                    |
// | Called by the Grave                | Spell Quick  | Yes  | Yes   | Target  | Banish monster from opp GY & negate it        | Opponent handtrap or GY effect activates      | Target already negated                      |
// | Triple Tactics Talent              | Spell Normal | Yes  | Yes   | None    | Opp used monster eff in MP: Draw 2/Steal/Rip  | Opponent activated monster effect in bot MP   | Opponent did not activate monster eff       |
// | Exceptional Schedule               | Spell Normal | Yes  | Yes   | None    | Search Urgent Schedule + Token to opp; GY SS  | Need Urgent Schedule; in GY to revive L10     | Already used this turn                      |
// | Urgent Schedule                    | Spell Quick  | Yes  | Yes   | None    | Opp controls more mon: SS 1 L<=4 + 1 L>=5 Mach| Opp controls more monsters                    | Bot controls more monsters                  |
// | Revolving Switchyard               | Spell Field  | Yes  | Yes   | Discard | Discard 1 -> Search L10 Mach; SS L4 as L10    | Need L10 Machine starter; on L10 summon       | Already used this turn                      |
// | Crystron Inclusion                 | Spell Cont   | Yes  | Yes   | None    | On activation: Search Crystron; protect destr | Main Phase early search; pop fodder           | Already active on field                     |
// | Crystron Cluster                   | Trap Cont    | Yes  | Yes   | Pop/Ban | SS Crystron & pop 1; GY: dump Crystron from Dk| Need interruption or GY dump                  | No Crystron in deck/GY                      |
// | F.A. Dawn Dragster                 | Synchro L7   | Yes  | Yes   | Level-2 | Quick: Negate opp Spell/Trap & reduce Level 2 | Opponent activates Spell/Trap                 | Level <= 2                                  |
// | Crystron Eleskeletus               | Synchro L7   | Yes  | Yes   | None    | Opp monsters -500 ATK/DEF; recycle Crystron   | Synchro Summoned; floats on destruction       | No Crystron in GY/banish                    |
// | Crystron Quariongandrax            | Synchro L9   | Yes  | Yes   | None    | On Synchro: Banish opp mon up to materials    | Opponent has monsters on field/GY             | Opponent has 0 monsters                     |
// | Centur-Ion Legatia                 | Synchro L12  | Yes  | Yes   | None    | On SS: Draw 1 & pop highest ATK opp monster   | Synchro Summoned (Citree + L10 on opp turn)   | Opponent unaffected                         |
// | Infinitrack River Stormer          | Xyz Rank 5   | Yes  | Yes   | Detach  | Detach 1: Search or dump EARTH Machine        | Main Phase rank 5 engine play                 | No targets in deck                          |
// | Superdreadnought Gustav Max        | Xyz Rank 10  | Yes  | Yes   | Detach  | Detach 1: Inflict 2000 damage to opponent     | Main Phase burn push / preparing Liebe OTK    | Turn 1 or opponent unaffected               |
// | Number 81: Super Dora              | Xyz Rank 10  | Yes  | Yes   | Detach  | Quick: Make 1 face-up monster immune to card  | Target high-value boss for protection         | Already immune                              |
// | Juggernaut Liebe                   | Xyz Rank 11  | Yes  | Yes   | Detach  | Overlay on R10; +2000 ATK/DEF, multi-attack   | Battle Phase OTK beatdown                     | Bot already has lethal                      |
// | Flying Launcher                    | Xyz Rank 10  | Yes  | Yes   | Detach  | Extra NS Machine; Search EARTH Mach; pop S/T  | Xyz Summoned; clear opponent backrow          | No backrow to pop                           |
// | Gustav Rocket                      | Xyz Rank 10  | Yes  | Yes   | Detach  | Overlay on Gustav; Quick monster negate+1000  | Opponent activates monster effect             | Already negated this turn                   |
// | Divine Arsenal AA-ZEUS             | Xyz Rank 12  | Yes  | No    | Detach  | Quick: Detach 2 to send all other cards to GY | After Xyz battled, clear overwhelming board   | Bot winning board established               |
// | Clockwork Knight                   | Link-1       | Yes  | Yes   | Tribute | Send <=1000 ATK Machine to GY; revive Machine | Put Recycler/Citree in GY; revive <=1000 ATK | No valid targets                            |
// | Double Headed Anger Knuckle        | Link-2       | Yes  | Yes   | Send    | Quick: Send 1 from hand/field to revive L10   | Main Phase 2 or opponent turn recursion       | No L10 Machine in GY                        |
// | Qliphort Genius                    | Link-2       | Yes  | Yes   | None    | Unaffected by S/T; Search Regulus on dual SS  | 2 Machines on field; trigger dual SS search   | No Regulus in deck                          |
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
            public const int MulcharmyFuwalos = 42141493;
            public const int AshBlossom = 14558127;
            public const int DrollAndLock = 94145021;
            public const int ForbiddenDroplet = 24299458;
            public const int CalledByTheGrave = 24224830;
            public const int TripleTacticsTalent = 25311006;
            public const int ExceptionalSchedule = 52782439;
            public const int UrgentSchedule = 25274141;
            public const int Switchyard = 76136345;
            public const int Inclusion = 31552317;
            public const int Cluster = 53829527;

            // Extra Deck
            public const int DawnDragster = 33158448;
            public const int Eleskeletus = 47736165;
            public const int Quariongandrax = 13455674;
            public const int Legatia = 15982593;
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
            RegisterExecutors();
        }

        private void RegisterExecutors()
        {
            // -------------------------------------------------------------
            // 1. High-Priority Handtraps & Interruptions (Chain / Enemy Turn)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLock, DrollAndLockEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, () => SmartHandTrapChain() && DefaultAshBlossomAndJoyousSpring());
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);

            // Boss Quick Negates & Protections
            AddExecutor(ExecutorType.Activate, CardId.GustavRocket, GustavRocketEffect);
            AddExecutor(ExecutorType.Activate, CardId.Regulus, RegulusEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperDora, SuperDoraEffect);
            AddExecutor(ExecutorType.Activate, CardId.DawnDragster, DawnDragsterEffect);
            AddExecutor(ExecutorType.Activate, CardId.Zeus, ZeusEffect);

            // Reactive Crystron Moves
            AddExecutor(ExecutorType.Activate, CardId.Citree, CitreeOpponentTurnSynchro);
            AddExecutor(ExecutorType.Activate, CardId.Cluster, ClusterEffect);
            AddExecutor(ExecutorType.Activate, CardId.UrgentSchedule, UrgentScheduleEffect);

            // -------------------------------------------------------------
            // 2. Continuous & Field Spells (Setup)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.Inclusion, InclusionActivate);
            AddExecutor(ExecutorType.Activate, CardId.Switchyard, SwitchyardActivate);

            // -------------------------------------------------------------
            // 3. Main Phase 1 Starters & Summon Spells
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.ExceptionalSchedule, ExceptionalScheduleEffect);
            AddExecutor(ExecutorType.Summon, CardId.ScrapRecycler, ScrapRecyclerSummon);
            AddExecutor(ExecutorType.Activate, CardId.ScrapRecycler, ScrapRecyclerEffect);
            AddExecutor(ExecutorType.Summon, CardId.Smiger, SmigerSummon);
            AddExecutor(ExecutorType.Summon, CardId.Tristaros, TristarosSummon);
            AddExecutor(ExecutorType.Summon, CardId.ConvexKnight, ConvexKnightSummon);
            AddExecutor(ExecutorType.Summon, CardId.Citree, CitreeSummon);
            AddExecutor(ExecutorType.Summon, CardId.BabelDecker, BabelDeckerSummon);

            // -------------------------------------------------------------
            // 4. Crystron Engine Movements (Locks applied on field ignition)
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.Sulfefnir, SulfefnirEffect);
            AddExecutor(ExecutorType.Activate, CardId.Sulfador, SulfadorEffect);
            AddExecutor(ExecutorType.Activate, CardId.Smiger, SmigerFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.Smiger, SmigerGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.Smiger, SmigerHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.Tristaros, TristarosEffect);

            // -------------------------------------------------------------
            // 5. Train & Earth Machine Engine Movements
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.Activate, CardId.BleuTraveler, BleuTravelerEffect);
            AddExecutor(ExecutorType.Activate, CardId.BabelDecker, BabelDeckerEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BulletTrain, BulletTrainSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ConvexKnight, ConvexKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.ConvexKnight, ConvexKnightEffect);

            // -------------------------------------------------------------
            // 6. Link Bridges & Qliphort Genius
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpSummon, CardId.ClockworkKnight, ClockworkKnightSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.QliphortGenius, GeniusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AngerKnuckle, AngerKnuckleSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AngerKnuckle, AngerKnuckleEffect);

            // -------------------------------------------------------------
            // 7. Extra Deck Synchro & Xyz Boss Monsters
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpSummon, CardId.Legatia, LegatiaSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Legatia, LegatiaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.DawnDragster, DawnDragsterSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Eleskeletus, EleskeletusSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Eleskeletus, EleskeletusEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Quariongandrax, QuariongandraxSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Quariongandrax, QuariongandraxEffect);

            // Rank 5 & Rank 10 Engines
            AddExecutor(ExecutorType.SpSummon, CardId.RiverStormer, RiverStormerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RiverStormer, RiverStormerEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.FlyingLauncher, FlyingLauncherSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.FlyingLauncher, FlyingLauncherEffect);

            // Rank 10 / 11 OTK & Burn Bosses
            AddExecutor(ExecutorType.SpSummon, CardId.GustavMax, GustavMaxSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.GustavMax, GustavMaxEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperDora, SuperDoraSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.GustavRocket, GustavRocketSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.JuggernautLiebe, JuggernautLiebeSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.JuggernautLiebe, JuggernautLiebeEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Zeus, ZeusSpSummon);

            // -------------------------------------------------------------
            // 8. Therion & Backrow Setting
            // -------------------------------------------------------------
            AddExecutor(ExecutorType.SpSummon, CardId.Regulus, RegulusSpSummon);
            AddExecutor(ExecutorType.SpellSet, CardId.Cluster, DefensiveSpellSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, DefensiveSpellSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.UrgentSchedule, DefensiveSpellSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, DefensiveSpellSetCheck);
        }

        public override bool OnSelectHand()
        {
            // Prefer going first for defensive Citree opponent-turn Synchro setup
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
        }

        public override void OnChaining(int player, ClientCard card)
        {
            if (player == 1 && card != null && card.HasType(CardType.Monster) && Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                _opponentActivatedMonsterEffect = true;
            }
            base.OnChaining(player, card);
        }

        // =================================================================
        // DISRUPTIONS & HANDTRAPS
        // =================================================================

        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
        }

        private bool DrollAndLockEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1;
        }

        private bool ForbiddenDropletEffect()
        {
            if (Duel.Player == 1)
            {
                bool hasCost = Bot.Hand.Count(c => c != Card) + Bot.GetMonsters().Count + Bot.GetSpells().Count(c => c != Card) > 0;
                if (!hasCost) return false;
                return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() && (c.Level >= 5 || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)));
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            return Duel.Player == 0 && _opponentActivatedMonsterEffect && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
        }

        private bool GustavRocketEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            // Quick Effect: Negate opponent monster effect, destroy it, and burn 1000
            return Duel.LastChainPlayer != 0;
        }

        private bool DawnDragsterEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown()) return false;
            // Quick Effect: Negate opponent Spell/Trap card or effect
            return Duel.LastChainPlayer != 0 && Card.Level > 2;
        }

        private bool RegulusEffect()
        {
            if (Duel.Player == 1) return true; // Opponent turn: negate freely
            if (Duel.Player == 0 && OpponentHasThreateningMonster()) return true;
            return false;
        }

        private bool SuperDoraEffect()
        {
            // Protect our strongest monster on the field
            ClientCard target = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ZeusEffect()
        {
            if (Util.IsChainTarget(Card)) return true;
            int ourCards = Bot.GetMonsterCount() + Bot.GetSpellCount();
            int enemyCards = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            return enemyCards >= 3 && enemyCards > ourCards;
        }

        private bool UrgentScheduleEffect()
        {
            // Requires opponent to control more monsters
            return Enemy.GetMonsterCount() > Bot.GetMonsterCount();
        }

        // =================================================================
        // MAIN PHASE STARTERS & CONTINUOUS SPELLS
        // =================================================================

        private bool InclusionActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (HasLethalOnBoard()) return false;
            return true;
        }

        private bool SwitchyardActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (HasLethalOnBoard()) return false;
            bool hasBoss = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() &&
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

            if (Util.ChainContainsCard(CardId.ExceptionalSchedule)) return false;

            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (_exceptionalScheduleActivatedThisTurn) return false;
                if (HasLethalOnBoard()) return false;

                bool hasTarget = GetRemainingCount(CardId.UrgentSchedule) > 0;
                if (!hasTarget) return false;

                _exceptionalScheduleActivatedThisTurn = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // Banish from GY to Special Summon 1 Level 10 Machine from GY/banish
                bool hasLv10 = Bot.Graveyard.Concat(Bot.Banished).Any(c => c != null && c.IsMonster() && c.Level == 10 && c.HasRace(CardRace.Machine));
                if (!hasLv10) return false;
                return Bot.GetMonsterCount() + Bot.GetSpellCount() > 0;
            }
            return false;
        }

        private bool ScrapRecyclerSummon()
        {
            return true;
        }

        private bool ScrapRecyclerEffect()
        {
            // Dump machine monsters to GY
            bool hasSulfefnir = GetRemainingCount(CardId.Sulfefnir) > 0;
            bool hasBleuTraveler = GetRemainingCount(CardId.BleuTraveler) > 0;
            bool hasSulfador = GetRemainingCount(CardId.Sulfador) > 0;
            bool hasSmiger = GetRemainingCount(CardId.Smiger) > 0;

            EnginePriority engine = GetPreferredEngine();
            if (engine == EnginePriority.Crystron && hasSulfefnir) return true;
            if (engine == EnginePriority.Trains && hasBleuTraveler) return true;

            return hasSulfefnir || hasBleuTraveler || hasSulfador || hasSmiger;
        }

        private bool SmigerSummon()
        {
            if (ShouldPrioritizeTrains()) return false;
            return true;
        }

        private bool TristarosSummon()
        {
            if (Bot.GetMonsterCount() > 0) return false;
            if (ShouldPrioritizeTrains()) return false;
            return true;
        }

        private bool ConvexKnightSummon()
        {
            if (ShouldPrioritizeTrains())
            {
                return GetRemainingCount(CardId.ScrapRecycler) > 0 ||
                       GetRemainingCount(CardId.BleuTraveler) > 0 ||
                       GetRemainingCount(CardId.BulletTrain) > 0 ||
                       GetRemainingCount(CardId.BabelDecker) > 0;
            }
            return true;
        }

        private bool CitreeSummon()
        {
            bool hasGyTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && !c.HasType(CardType.Tuner) && (c.Level == 10 || c.Level == 5));
            if (!hasGyTarget && Duel.Player == 0) return false;
            if (ShouldPrioritizeTrains()) return false;
            return true;
        }

        private bool BabelDeckerSummon()
        {
            // Normal summon without tribute
            return true;
        }

        // =================================================================
        // CRYSTRON MOVEMENTS
        // =================================================================

        private bool SulfefnirEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                return Bot.Hand.Any(c => c != null && c != Card && IsCrystronCard(c));
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Must have Crystron in Deck before self-popping
                if (!HasRemainingCrystronMonster()) return false;
                AI.SelectCard(Card.Id);
                return true;
            }
            return false;
        }

        private bool SulfadorEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                if (Bot.GetMonsterCount() + Bot.GetSpellCount() == 0) return false;
                if (!HasRemainingCrystronMonster()) return false;

                ClientCard target = Bot.SpellZone.FirstOrDefault(c => c != null && c.IsCode(CardId.Inclusion) && c.IsFaceup());
                if (target == null) target = Bot.GetMonsters().FirstOrDefault(c => c != null && IsCrystronCard(c) && c.IsFaceup() && c.Id != CardId.Citree);
                if (target == null) target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsCode(CardId.ScrapRecycler) && c.IsFaceup());
                if (target == null) target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c));

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
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (!HasRemainingCrystronMonster()) return false;

            ClientCard target = Bot.SpellZone.FirstOrDefault(c => c != null && c.IsCode(CardId.Inclusion) && c.IsFaceup());
            if (target == null) target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.Id != CardId.Citree && IsCrystronCard(c) && c.IsFaceup());
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
            if (Card.Location != CardLocation.Grave) return false;
            if (!HasRemainingCrystronCard()) return false;
            if (ShouldPrioritizeTrains() && IsBoardStrongEnough()) return false;
            return true;
        }

        private bool SmigerHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Util.ChainContainsCard(CardId.Smiger)) return false;

            bool hasCrystronToReveal = Bot.Hand.Any(c => c != null && c != Card && IsCrystronCard(c) && c.IsMonster());
            if (!hasCrystronToReveal) return false;

            bool hasTarget = GetRemainingCount(CardId.Inclusion) > 0 || GetRemainingCount(CardId.Cluster) > 0;
            if (!hasTarget) return false;

            if (Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.Inclusion) && c.IsFaceup()))
            {
                if (GetRemainingCount(CardId.Cluster) == 0) return false;
            }
            return true;
        }

        private bool TristarosEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                bool hasSynchro = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Synchro));
                int crystronCountInDeck = GetRemainingCount(CardId.Citree) + GetRemainingCount(CardId.Smiger) +
                                         GetRemainingCount(CardId.Sulfefnir) + GetRemainingCount(CardId.Sulfador);
                if (!hasSynchro || crystronCountInDeck < 2) return false;

                _hasMachineExtraDeckLock = true;
                return true;
            }
            return HasRemainingCrystronMonster();
        }

        // =================================================================
        // TRAIN & EARTH MACHINE MOVEMENTS
        // =================================================================

        private bool BleuTravelerEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (HasLethalOnBoard()) return false;
                bool hasTarget = GetRemainingCount(CardId.Switchyard) > 0 || Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.Switchyard));
                return hasTarget;
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

        private bool BabelDeckerEffect()
        {
            bool hasHandTarget = Bot.Hand.Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth));
            return hasHandTarget || _opponentActivatedMonsterEffect;
        }

        private bool BulletTrainSpSummon()
        {
            int count = Bot.GetMonsterCount();
            if (count == 0) return false;
            return Bot.GetMonsters().All(c => c == null || (c.Attribute == (int)CardAttribute.Earth && c.Race == (int)CardRace.Machine));
        }

        private bool ConvexKnightSpSummon()
        {
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Machine))
                || Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Machine));
        }

        private bool ConvexKnightEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                return GetRemainingCount(CardId.ScrapRecycler) > 0 ||
                       GetRemainingCount(CardId.BleuTraveler) > 0 ||
                       GetRemainingCount(CardId.BulletTrain) > 0 ||
                       GetRemainingCount(CardId.BabelDecker) > 0 ||
                       GetRemainingCount(CardId.Sulfefnir) > 0;
            }
            return true;
        }

        private bool ClockworkKnightSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            bool hasScrapRecyclerOnField = Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.ScrapRecycler));
            bool hasCitreeOnField = Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.Citree));

            if (hasScrapRecyclerOnField || hasCitreeOnField)
            {
                bool hasBleuInGY = Bot.Graveyard.Any(c => c.IsCode(CardId.BleuTraveler));
                bool hasOtherInGY = Bot.Graveyard.Any(c => c.IsMonster() && c.Attribute == (int)CardAttribute.Earth && c.Id != CardId.BleuTraveler);
                if (hasBleuInGY && !hasOtherInGY && hasScrapRecyclerOnField) return true;

                bool hasRegulusInHand = Bot.Hand.Any(c => c.IsCode(CardId.Regulus));
                bool hasMachineInGY = Bot.Graveyard.Any(c => c.IsMonster() && c.Race == (int)CardRace.Machine);
                if (hasRegulusInHand && !hasMachineInGY) return true;
            }
            return false;
        }

        private bool GeniusSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            return Bot.GetMonsters().Count(c => c != null && c.IsMonster()) >= 2;
        }

        private bool AngerKnuckleSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            return Bot.GetMonsters().Count(c => c != null && c.IsMonster()) >= 2 && Duel.Phase == DuelPhase.Main2;
        }

        private bool AngerKnuckleEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                bool hasLv10 = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Level == 10 && c.HasRace(CardRace.Machine));
                bool canSend = Bot.Hand.Any(c => c != Card) || Bot.GetMonsters().Any(c => c != Card);
                return hasLv10 && canSend;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                return Bot.Hand.Count > 0 || Bot.GetMonsterCount() > 0 || Bot.GetSpellCount() > 0;
            }
            return true;
        }

        // =================================================================
        // EXTRA DECK BOSS SUMMONS & EFFECTS
        // =================================================================

        private bool LegatiaSpSummon()
        {
            if (ShouldPrioritizeTrains() && !_hasMachineExtraDeckLock) return false;
            if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
            return !_hasMachineExtraDeckLock;
        }

        private bool LegatiaEffect()
        {
            // Draw 1 card and pop highest ATK opponent monster
            return true;
        }

        private bool DawnDragsterSpSummon()
        {
            if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
            if (ShouldPrioritizeTrains() && Bot.GetMonsters().Count(c => c != null && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2)
                return false;
            return !_hasMachineExtraDeckLock;
        }

        private bool EleskeletusSpSummon()
        {
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            if (ShouldPrioritizeTrains()) return false;
            return !_hasMachineExtraDeckLock;
        }

        private bool EleskeletusEffect()
        {
            // Recycle Crystron from GY/banished
            return true;
        }

        private bool QuariongandraxSpSummon()
        {
            if (!OpponentHasThreateningMonster() && Enemy.GetMonsterCount() < 2) return false;
            if (ShouldPrioritizeTrains()) return false;
            return !_hasMachineExtraDeckLock;
        }

        private bool QuariongandraxEffect()
        {
            // Banish opponent monsters up to materials
            return true;
        }

        private bool RiverStormerSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            return Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 5 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool RiverStormerEffect()
        {
            return true;
        }

        private bool FlyingLauncherSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            return Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool FlyingLauncherEffect()
        {
            return true;
        }

        private bool GustavMaxSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            if (Duel.Turn == 1) return false;
            return Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool GustavMaxEffect()
        {
            if (Enemy.LifePoints <= 2000) return true;
            if (Enemy.LifePoints <= 4000 && GetRemainingCount(CardId.JuggernautLiebe) > 0) return true;
            return Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2;
        }

        private bool SuperDoraSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            return Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool GustavRocketSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // Overlays on top of Gustav Max by discarding 1 card
            bool hasGustav = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.GustavMax));
            return hasGustav && Bot.Hand.Count > 0;
        }

        private bool JuggernautLiebeSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            if (HasLethalOnBoard()) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.IsCode(CardId.GustavMax) || c.IsCode(CardId.SuperDora) || c.IsCode(CardId.FlyingLauncher)));
        }

        private bool JuggernautLiebeEffect()
        {
            if (Duel.Phase == DuelPhase.Main1)
            {
                bool hasOtherAttackers = Bot.GetMonsters().Any(m => m != null && m.Id != CardId.JuggernautLiebe && m.IsFaceup() && m.IsAttack() && !m.Attacked);
                if (hasOtherAttackers) return false;
            }
            return true;
        }

        private bool ZeusSpSummon()
        {
            return !_hasMachineSynchroLock && !_hasMachineExtraDeckLock &&
                   Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.Attacked);
        }

        private bool RegulusSpSummon()
        {
            return Bot.GetSpellCount() < 5 && Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Race == (int)CardRace.Machine);
        }

        private bool ClusterEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                bool hasSummonTarget = Bot.Hand.Concat(Bot.Graveyard).Any(c => c != null && IsCrystronCard(c) && c.IsMonster());
                if (!hasSummonTarget) return false;

                bool hasPopTarget = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsCrystronCard(c))
                                 || Bot.SpellZone.Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Inclusion) && c != Card);
                return hasPopTarget;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                bool hasFaceupCrystron = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsCrystronCard(c));
                return hasFaceupCrystron && HasRemainingCrystronMonster();
            }
            return false;
        }

        private bool CitreeOpponentTurnSynchro()
        {
            if (Duel.Player == 0) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2 && Duel.Phase != DuelPhase.Battle) return false;

            // Legatia (Level 12): Citree (2) + Level 10 non-Tuner = Legatia (12)
            ClientCard lvl10 = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.Level == 10 && !c.HasType(CardType.Tuner));
            if (lvl10 != null && GetRemainingCount(CardId.Legatia) > 0)
            {
                AI.SelectCard(lvl10);
                return true;
            }

            // Dawn Dragster / Eleskeletus (Level 7): Citree (2) + Level 5 non-Tuner
            ClientCard lvl5 = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.Level == 5 && !c.HasType(CardType.Tuner));
            if (lvl5 != null && (GetRemainingCount(CardId.DawnDragster) > 0 || GetRemainingCount(CardId.Eleskeletus) > 0))
            {
                AI.SelectCard(lvl5);
                return true;
            }

            return false;
        }

        private bool DefensiveSpellSetCheck()
        {
            if (Duel.Player != 0) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (Bot.GetSpellCountWithoutField() >= 5) return false;
            if (Duel.Phase == DuelPhase.Main2) return true;

            return Duel.MainPhase == null || !Duel.MainPhase.CanBattlePhase || !Bot.HasAttackingMonster();
        }

        // =================================================================
        // BOARD EVALUATION & HELPERS
        // =================================================================

        private bool HasLethalOnBoard()
        {
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            int totalAtk = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.IsAttack() && !c.Attacked).Sum(c => c.Attack);
            return totalAtk >= Enemy.LifePoints;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.SuperDora) || Bot.HasInMonstersZone(CardId.JuggernautLiebe) ||
                Bot.HasInMonstersZone(CardId.GustavRocket) || Bot.HasInMonstersZone(CardId.FlyingLauncher) ||
                Bot.HasInMonstersZone(CardId.DawnDragster) || Bot.HasInMonstersZone(CardId.Legatia))
                return true;

            if (Bot.HasInMonstersZone(CardId.Citree) && Bot.GetMonsterCount() >= 2)
                return true;

            int xyzCount = Bot.GetMonsters().Count(c => c != null && c.HasType(CardType.Xyz));
            return xyzCount >= 2;
        }

        protected override bool IsInGrindGame()
        {
            return Bot.GetHandCount() <= 1 && Bot.GetMonsterCount() <= 1 && Bot.GetSpells().Count(c => c != null && c.IsFacedown()) == 0;
        }

        private bool OpponentHasThreateningMonster()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 2800 || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
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
            return HasRemainingCrystronMonster() ||
                   GetRemainingCount(CardId.Cluster) > 0 ||
                   GetRemainingCount(CardId.Inclusion) > 0 ||
                   HasRemainingCardWithSetcode(0xea);
        }

        private enum EnginePriority
        {
            Crystron,
            Trains,
            Both,
            None
        }

        private EnginePriority GetPreferredEngine()
        {
            bool hasCrystronStarter = Bot.Hand.Any(c => c != null && IsCrystronCard(c) && c.IsMonster() && !c.IsCode(CardId.Tristaros));
            bool hasCrystronField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsCrystronCard(c));
            bool hasCrystronInDeck = HasRemainingCrystronMonster();

            bool hasTrainStarter = Bot.Hand.Any(c => c != null && (
                c.IsCode(CardId.Switchyard) || c.IsCode(CardId.ExceptionalSchedule) ||
                c.IsCode(CardId.ScrapRecycler) || c.IsCode(CardId.UrgentSchedule)));
            bool hasTrainField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !IsCrystronCard(c) &&
                c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth));

            bool hasCrystron = (hasCrystronStarter || hasCrystronField) && hasCrystronInDeck;
            bool hasTrains = hasTrainStarter || hasTrainField;

            if (Duel.Turn == 1 && Duel.Player == 0)
            {
                if (hasCrystron) return EnginePriority.Crystron;
                if (hasTrains) return EnginePriority.Trains;
                return EnginePriority.None;
            }

            if (hasCrystron && hasTrains) return EnginePriority.Both;
            if (hasCrystron) return EnginePriority.Crystron;
            if (hasTrains) return EnginePriority.Trains;
            return EnginePriority.None;
        }

        private bool ShouldPrioritizeTrains()
        {
            return GetPreferredEngine() == EnginePriority.Trains;
        }

        private int GetDiscardPriority(ClientCard card)
        {
            if (card == null) return int.MaxValue;

            // Preferred graveyard fodder
            if (card.Id == CardId.BleuTraveler && card.Location == CardLocation.Hand) return 1;
            if (card.Id == CardId.Sulfefnir && card.Location == CardLocation.Hand) return 2;
            if (card.Id == CardId.Smiger && card.Location == CardLocation.Hand) return 3;
            if (card.Id == CardId.Sulfador && card.Location == CardLocation.Hand) return 4;
            if (card.Id == CardId.Tristaros && card.Location == CardLocation.Hand) return 5;
            if (card.Id == CardId.BulletTrain && card.Location == CardLocation.Hand) return 6;

            if (card.Id == CardId.Cluster) return 10;
            if (card.Id == CardId.Inclusion && Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.Inclusion))) return 11;
            if (card.Id == CardId.Switchyard && Bot.SpellZone.Any(c => c != null && c.IsCode(CardId.Switchyard))) return 12;
            if (card.Id == CardId.ConvexKnight) return 15;

            if (card.Id == CardId.ScrapRecycler) return 50;
            if (card.Id == CardId.Citree) return 60;

            // Protect handtraps & bosses
            if (card.Id == CardId.CalledByTheGrave) return 100;
            if (card.Id == CardId.ForbiddenDroplet) return 101;
            if (card.Id == CardId.AshBlossom) return 102;
            if (card.Id == CardId.MulcharmyFuwalos) return 103;
            if (card.Id == CardId.DrollAndLock) return 104;

            return 80;
        }

        // =================================================================
        // SELECTION OVERRIDES (OnSelectOption, OnSelectCard, OnSelectYesNo)
        // =================================================================

        public override int OnSelectOption(IList<long> options)
        {
            if (options == null || options.Count == 0)
                return 0;

            for (int i = 0; i < options.Count; i++)
            {
                // Bitshift fix: (id << 4) | opt
                long cardId = options[i] >> 4;
                if (cardId == 0 && Card != null)
                {
                    cardId = Card.Id;
                }
                long optIndex = options[i] & 0xf;

                if (cardId == CardId.TripleTacticsTalent)
                {
                    // Draw 2 cards if hand is low
                    if (Bot.GetHandCount() <= 2 && optIndex == 0) return i;
                    // Take control of high threat monster
                    if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && (c.Attack >= 2500 || c.IsFloodgate())) && optIndex == 1) return i;
                    // Default to draw 2
                    if (optIndex == 0) return i;
                }
            }

            return base.OnSelectOption(options);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Search / Add to hand (HINTMSG_ATOHAND = 506 / 505)
            if (hint == 506 || hint == 505)
            {
                var searchPreferred = new List<int>
                {
                    CardId.Inclusion,
                    CardId.ExceptionalSchedule,
                    CardId.Switchyard,
                    CardId.UrgentSchedule,
                    CardId.ScrapRecycler,
                    CardId.BleuTraveler,
                    CardId.Smiger,
                    CardId.Sulfefnir,
                    CardId.BulletTrain,
                    CardId.Regulus,
                    CardId.BabelDecker,
                    CardId.ConvexKnight,
                    CardId.Cluster
                };

                var matches = cards.Where(c => searchPreferred.Contains(c.Id))
                                   .OrderBy(c => searchPreferred.IndexOf(c.Id))
                                   .ToList();
                if (matches.Count >= min)
                    return matches.Take(max).ToList();
            }

            // 2. Removal / Destruction (HINTMSG_DESTROY = 502, HINTMSG_REMOVE = 503 / 504)
            if (hint == 502 || hint == 503 || hint == 504)
            {
                // If cards contain opponent targets: strictly target opponent cards
                var enemyTargets = cards.Where(c => c.Controller == 1 && !IsTargetImmune(c)).ToList();
                if (enemyTargets.Count >= min)
                {
                    return enemyTargets.OrderByDescending(c => GetCardThreatScore(c)).Take(max).ToList();
                }

                // If this is Crystron self-destruction (all cards belong to bot)
                if (cards.All(c => c.Controller == 0))
                {
                    var popPriority = new List<ClientCard>();
                    // 1. Face-up Inclusion (already searched)
                    var inc = cards.FirstOrDefault(c => c.IsCode(CardId.Inclusion) && c.IsFaceup());
                    if (inc != null) popPriority.Add(inc);
                    // 2. Scrap Recycler on field
                    var rec = cards.FirstOrDefault(c => c.IsCode(CardId.ScrapRecycler) && c.IsFaceup());
                    if (rec != null) popPriority.Add(rec);
                    // 3. Sulfefnir itself (floats into Crystron from deck)
                    var sul = cards.FirstOrDefault(c => c.IsCode(CardId.Sulfefnir) && c.Location == CardLocation.MonsterZone);
                    if (sul != null) popPriority.Add(sul);
                    // 4. Other non-Ace Crystrons
                    var otherCrystron = cards.Where(c => IsCrystronCard(c) && !IsAceCard(c) && c.Id != CardId.Citree).ToList();
                    popPriority.AddRange(otherCrystron);

                    // Exclude Ace Bosses
                    var nonAceCards = cards.Where(c => !IsAceCard(c)).ToList();
                    popPriority.AddRange(nonAceCards);

                    var distinctPops = popPriority.Distinct().ToList();
                    if (distinctPops.Count >= min)
                        return distinctPops.Take(max).ToList();
                }
            }

            // 3. Discard / Cost (HINTMSG_DISCARD = 501)
            if (hint == 501)
            {
                var sorted = cards.OrderBy(GetDiscardPriority).ToList();
                if (sorted.Count >= min)
                    return sorted.Take(max).ToList();
            }

            // 4. Send to GY (HINTMSG_TOGRAVE = 508)
            if (hint == 508)
            {
                var dumpPreferred = new List<int>
                {
                    CardId.Sulfefnir,
                    CardId.BleuTraveler,
                    CardId.Smiger,
                    CardId.Sulfador,
                    CardId.BulletTrain,
                    CardId.ScrapRecycler
                };
                var matches = cards.Where(c => dumpPreferred.Contains(c.Id))
                                   .OrderBy(c => dumpPreferred.IndexOf(c.Id))
                                   .ToList();
                if (matches.Count >= min)
                    return matches.Take(max).ToList();
            }

            // 5. Special Summon (HINTMSG_SPSUMMON = 509)
            if (hint == 509)
            {
                // Urgent Schedule: select 1 L<=4 and 1 L>=5
                if (Card != null && Card.Id == CardId.UrgentSchedule)
                {
                    var result = new List<ClientCard>();
                    var lvl4 = cards.FirstOrDefault(c => c.IsMonster() && c.Level <= 4 && c.Attribute == (int)CardAttribute.Earth);
                    var lvl5 = cards.FirstOrDefault(c => c.IsMonster() && c.Level >= 5 && c.Attribute == (int)CardAttribute.Earth);
                    if (lvl4 != null) result.Add(lvl4);
                    if (lvl5 != null) result.Add(lvl5);
                    if (result.Count >= min && result.Count <= max) return result;
                }

                // General revival priority
                var ssPreferred = new List<int>
                {
                    CardId.BulletTrain,
                    CardId.BleuTraveler,
                    CardId.BabelDecker,
                    CardId.Smiger,
                    CardId.Citree,
                    CardId.Tristaros,
                    CardId.Sulfefnir
                };
                var ssMatches = cards.Where(c => ssPreferred.Contains(c.Id))
                                     .OrderBy(c => ssPreferred.IndexOf(c.Id))
                                     .ToList();
                if (ssMatches.Count >= min)
                    return ssMatches.Take(max).ToList();
            }

            // 6. Equip (HINTMSG_EQUIP = 507)
            if (hint == 507 && Card != null && Card.Id == CardId.Regulus)
            {
                ClientCard target = cards.FirstOrDefault(c => IsCrystronCard(c) && c.IsMonster());
                if (target == null) target = cards.FirstOrDefault(c => c.IsMonster());
                if (target != null) return new List<ClientCard> { target };
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

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // High ATK Xyz and Bosses: always Attack
            if (cardId == CardId.JuggernautLiebe ||
                cardId == CardId.GustavMax ||
                cardId == CardId.GustavRocket ||
                cardId == CardId.SuperDora ||
                cardId == CardId.FlyingLauncher ||
                cardId == CardId.Legatia ||
                cardId == CardId.Quariongandrax ||
                cardId == CardId.Zeus ||
                cardId == CardId.Regulus ||
                cardId == CardId.BleuTraveler ||
                cardId == CardId.BulletTrain)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            // Low ATK / Utility monsters: always Defense
            if (cardId == CardId.Citree ||
                cardId == CardId.Tristaros ||
                cardId == CardId.ScrapRecycler ||
                cardId == CardId.ConvexKnight ||
                cardId == CardId.BabelDecker ||
                cardId == CardId.ClockworkKnight)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }
    }
}
