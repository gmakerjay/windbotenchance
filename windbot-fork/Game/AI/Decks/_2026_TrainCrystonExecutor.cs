// ============================================================
// CARD AUDIT โ€” 2026 TrainCryston
// ============================================================
// | Card Name                       | Type    | ATK/DEF | LV | Role                  |
// |---------------------------------|---------|---------|----|----------------------|
// | Crystron Citree                | Tuner   | 500/500 | 2  | KEY โ€” opponent turn SS |
// | Crystron Tristaros             | Tuner   | 800/2000| 2  | Search/Recursion     |
// | Crystron Smiger                | Tuner   | 1000/1800|3 | KEY โ€” search engine  |
// | Crystron Sulfefnir             | Non-Tun | 2000/1500|5 | KEY โ€” SS from hand/GY|
// | Crystron Sulfador              | Non-Tun | 1900/2250|5 | SS + GY dump         |
// | Scrap Recycler                 | Non-Tun | 900/1200| 3  | Machine dump bridge  |
// | Bleu Traveler                  | Non-Tun | 2500/2500|10| KEY โ€” search Engine  |
// | Bullet Train                   | Non-Tun | 3000/0  | 10| Self-SS from hand    |
// | Convex Knight                  | Tuner   | 1800/500| 4  | Search Machine       |
// | Heavy Knight Babel Decker      | Non-Tun | 500/3000| 10| SS from hand + extend|
// | Therion \"King\" Regulus         | Non-Tun | 2800/1600|8| Negate + equip revival|
// | Ash Blossom & Joyous Spring    | Hand Trap|0/1800| 3  | Hand trap            |
// | Mulcharmy Fuwalos              | Hand Trap|100/600| 4  | Hand trap            |
// | Crystron Inclusion             | Field   | -       | -  | KEY โ€” search Crystron |
// | Exceptional Schedule           | Normal  | -       | -  | Search Urgent Schedule|
// | Urgent Schedule                | Quick   | -       | -  | SS 2 Earth Machines   |
// | Revolving Switchyard           | Field   | -       | -  | Search Train          |
// | Crystron Cluster               | Trap    | -       | -  | SS from GY + extend   |
// | Triple Tactics Talent          | Normal  | -       | -  | Draw/Control/Hand rip |
// | Forbidden Crown                | Quick   | -       | -  | Negate monster effect |
// | Forbidden Droplet              | Quick   | -       | -  | Negate + halve ATK    |
// | Called by the Grave            | Quick   | -       | -  | GY negation           |
// | Lightning Storm                | Normal  | -       | -  | Board wipe            |
// | Harpie's Feather Duster        | Normal  | -       | -  | Backrow wipe          |
// | Dark Ruler No More             | Normal  | -       | -  | Mass negate monsters  |
// |-------------------------------|---------|---------|----|----------------------|
// EXTRA DECK:
// | Crystron Eleskeletus          | Synchro | 2600/2100|7 | Recycle banished     |
// | Crystron Quariongandrax       | Synchro | 3000/3000|9 | ACE โ€” banish monsters |
// | Centur-Ion Legatia            | Synchro | 3500/2000|12| ACE โ€” omni-negate + draw|
// | F.A. Dawn Dragster            | Synchro | 0/2000   |7 | ACE โ€” omni-negate    |
// | Infinitrack River Stormer     | Xyz     | 2500/500 |5 | Search + GY setup    |
// | Superdreadnought Gustav Max   | Xyz     | 3000/3000|10| Burn 2000            |
// | Superdreadnought Super Dora   | Xyz     | 3200/4000|10| ACE โ€” targeting prot |
// | Superdreadnought Liebe        | Xyz     | 4000/4000|11| ACE โ€” OTK            |
// | Superdreadnought Flying Launchr| Xyz     | 3800/3000|10| ACE โ€” pop + search   |
// | Superdreadnought Gustav Rocket| Xyz     | 5000/3000|10| ACE โ€” big beater     |
// | Divine Arsenal AA-ZEUS        | Xyz     | 3000/3000|12| ACE โ€” board wipe     |
// | Double Headed Anger Knuckle   | Link    | 1500    | 2  | GY recycle bridge    |
// | Qliphort Genius               | Link    | 1800    | 2  | Search Regulus       |
// | Clockwork Knight               | Link    | 500     | 1  | Bridge โ€” send to GY  |
// ============================================================
// ACE CARDS:
//   Primary: Legatia (negate + draw), Dawn Dragster (omni-negate)
//   Secondary: Juggernaut Liebe (OTK), Super Dora (protect), Zeus (wipe)
// COMBO STARTERS:
//   1. Crystron Inclusion โ’ search Smiger โ’ establish Crystron engine
//   2. Scrap Recycler โ’ dump Sulfefnir โ’ extend
//   3. Bleu Traveler โ’ search Revolving Switchyard
//   4. Exceptional Schedule โ’ search Urgent Schedule
// WIN CONDITION: Dawn Dragster + Legatia + Super Dora = strong defense/OTK
// GOING 1ST END BOARD: Dawn Dragster + Citree (set) + set Cluster/Urgent Schedule
// GOING 2ND GAMEPLAN: Dark Ruler โ’ Lightning Storm โ’ Rank 10 Xyz OTK
// ============================================================
// COMBO DRAFT โ€” 2026 TrainCryston
// ============================================================
// === COMBO LINE 1: Crystron Setup (Turn 1) ===
// HAND: Inclusion + any Crystron
// STEP 1: Activate Inclusion โ’ search Smiger
// STEP 2: NS Smiger โ’ pop Inclusion โ’ search Sulfefnir/Citree
// STEP 3: SS Sulfefnir from hand/GY
// STEP 4: Synchro Citree (in hand) + Sulfefnir = Dawn Dragster (L7)
// OR: Set Citree + pass โ’ opponent turn synchro into Legatia/Dawn Dragster
// === COMBO LINE 2: Train OTK (Turn 2) ===
// HAND: Bleu Traveler + Urgent Schedule + Dark Ruler
// STEP 1: Dark Ruler โ’ negate all opponent monsters
// STEP 2: Lightning Storm โ’ wipe opponent backrow
// STEP 3: NS Bleu Traveler โ’ search Switchyard
// STEP 4: Activate Switchyard โ’ search Bullet Train
// STEP 5: SS Bullet Train (EARTH Machine only)
// STEP 6: Overlay Bleu Traveler + Bullet Train = Gustav Max
// STEP 7: Burn 2000 with Gustav Max
// STEP 8: Overlay Gustav Max โ’ Juggernaut Liebe
// STEP 9: Liebe attack for game (4000+ ATK)
// END BOARD: OTK (8000 damage)
// ============================================================

using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_TrainCryston", "2026_TrainCryston")]
    public class _2026_TrainCrystonExecutor : ModernExecutor
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

            // Hand Traps & Staples
            public const int AshBlossom = 14558128;
            public const int Fuwalos = 42141493;
            public const int CalledByTheGrave = 24224830;
            public const int ForbiddenDroplet = 24299458;
            public const int TTT = 25311006;
            public const int ForbiddenCrown = 98829635;
            public const int LightningStorm = 14532163;
            public const int HarpiesFeatherDuster = 18144507;
            public const int DarkRulerNoMore = 54693926;

            // Archetype Spells & Traps
            public const int Inclusion = 31552317;
            public const int ExceptionalSchedule = 52782439;
            public const int UrgentSchedule = 25274141;
            public const int Switchyard = 76136345;
            public const int Cluster = 53829527;

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
            public const int AngerKnuckle = 146746;
            public const int QliphortGenius = 22423493;
            public const int ClockworkKnight = 41739381;

            // Side
            public const int Droll = 55063751;
            public const int Purulia = 84192580;
            public const int Pankratops = 87074380;
            public const int Gameciel = 84636823;
            public const int TTTThrust = 35269904;
            public const int EvenlyMatched = 31849106;
        }

        // โ”€โ”€ Ace/Boss Monsters โ”€โ”€
        private static readonly int[] AceCardIds = {
            CardId.Legatia,
            CardId.DawnDragster,
            CardId.Quariongandrax,
            CardId.Eleskeletus,
            CardId.GustavMax,
            CardId.SuperDora,
            CardId.JuggernautLiebe,
            CardId.FlyingLauncher,
            CardId.GustavRocket,
            CardId.Zeus,
            CardId.Regulus,
        };

        // โ”€โ”€ Crystron cards for engine detection โ”€โ”€
        private static readonly int[] CrystronMonsters = {
            CardId.Citree, CardId.Tristaros, CardId.Smiger,
            CardId.Sulfefnir, CardId.Sulfador,
        };

        // โ”€โ”€ OPT Tracking โ”€โ”€
        private bool _exceptionalScheduleActivated = false;
        private bool _switchyardUsed = false;
        private bool _citreeUsed = false;
        private bool _smigerSearchUsed = false;
        private bool _clusterUsed = false;
        private bool _sulfefnirUsed = false;
        private bool _sulfadorUsed = false;
        private bool _bleuTravelerUsed = false;
        private bool _convexKnightUsed = false;
        private bool _babelDeckerUsed = false;
        private bool _gustavMaxBurned = false;
        private bool _riverStormerSearched = false;
        private bool _geniusSearched = false;
        private bool _opponentActivatedMonsterEffect = false;
        private bool _tristarosUsed = false;

        // โ”€โ”€ Lock tracking โ”€โ”€
        private bool _hasMachineSynchroLock = false;
        private bool _hasMachineExtraDeckLock = false;

        public _2026_TrainCrystonExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            ResourcePlan.RegisterAceCards(AceCardIds);
            HeuristicGuard.RegisterAceCards(AceCardIds);
            // โ”€โ”€ Combo Router โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Crystron-Start",
                RequiredCards = new List<int> { CardId.Inclusion, CardId.Smiger },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Inclusion, ActionType = ExecutorType.Activate, Description = "Search Smiger via Inclusion" },
                    new() { CardId = CardId.Smiger, ActionType = ExecutorType.Activate, Description = "NS Smiger, pop & search" },
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "BleuTraveler-Start",
                RequiredCards = new List<int> { CardId.BleuTraveler },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.BleuTraveler, ActionType = ExecutorType.Activate, Description = "Search Switchyard" },
                },
                EndBoardScore = 60
            });

            BaitPlanner.RegisterComboStarters(CardId.Inclusion, CardId.BleuTraveler, CardId.ScrapRecycler);
            BaitPlanner.RegisterBaitCards(CardId.ForbiddenCrown, CardId.TTT);
            ChainAdvisor.RegisterHighValueTargets(CardId.Inclusion, CardId.Citree, CardId.ScrapRecycler);

            // TIER 1: Hand Traps & Counters
            AddExecutor(ExecutorType.Activate, CardId.Fuwalos, FuwalosActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, SmartAshBlossom);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, DropletActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownActivate);
            AddExecutor(ExecutorType.Activate, CardId.TTT, TTTActivate);
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerActivate);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormActivate);
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, DefaultHarpiesFeatherDusterFirst);

            // TIER 2: Field & Search Spells
            AddExecutor(ExecutorType.Activate, CardId.Inclusion, InclusionActivate);
            AddExecutor(ExecutorType.Activate, CardId.Switchyard, SwitchyardActivate);
            AddExecutor(ExecutorType.Activate, CardId.ExceptionalSchedule, ExceptionalScheduleActivate);
            AddExecutor(ExecutorType.Activate, CardId.UrgentSchedule, UrgentScheduleActivate);

            // TIER 3: Crystron Engine โ€” Starter Monsters (Normal Summon)
            AddExecutor(ExecutorType.Summon, CardId.Smiger, SmigerSummon);
            AddExecutor(ExecutorType.Summon, CardId.Citree, CitreeSummon);
            AddExecutor(ExecutorType.Summon, CardId.ScrapRecycler, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.ConvexKnight, SimpleSummon);
            AddExecutor(ExecutorType.Summon, CardId.Tristaros, TristarosSummon);

            // TIER 4: Crystron Engine โ€” Activations
            AddExecutor(ExecutorType.Activate, CardId.Smiger, SmigerFieldEffect);
            AddExecutor(ExecutorType.Activate, CardId.Smiger, SmigerGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.Smiger, SmigerHandEffect);
            AddExecutor(ExecutorType.Activate, CardId.Sulfefnir, SulfefnirEffect);
            AddExecutor(ExecutorType.Activate, CardId.Sulfador, SulfadorEffect);
            AddExecutor(ExecutorType.Activate, CardId.Tristaros, TristarosEffect);
            AddExecutor(ExecutorType.Activate, CardId.ScrapRecycler, ScrapRecyclerEffect);

            // TIER 5: Train / Earth Machine Engine
            AddExecutor(ExecutorType.Activate, CardId.BleuTraveler, BleuTravelerEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BulletTrain, BulletTrainSpSummon);
            AddExecutor(ExecutorType.Summon, CardId.BabelDecker, SimpleSummon);
            AddExecutor(ExecutorType.Activate, CardId.BabelDecker, BabelDeckerEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ConvexKnight, ConvexKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.ConvexKnight, ConvexKnightEffect);

            // TIER 6: Link Bridges
            AddExecutor(ExecutorType.SpSummon, CardId.ClockworkKnight, ClockworkKnightSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AngerKnuckle, AngerKnuckleSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.AngerKnuckle, AngerKnuckleEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.QliphortGenius, GeniusSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.QliphortGenius, GeniusEffect);

            // TIER 7: Synchro Bosses
            AddExecutor(ExecutorType.SpSummon, CardId.Legatia, LegatiaSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.DawnDragster, DawnDragsterSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Eleskeletus, EleskeletusSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Quariongandrax, QuariongandraxSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RiverStormer, RiverStormerSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.RiverStormer, RiverStormerEffect);

            // TIER 8: Rank 10/11 Xyz Bosses
            AddExecutor(ExecutorType.SpSummon, CardId.FlyingLauncher, FlyingLauncherSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.FlyingLauncher, FlyingLauncherEffect);
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

            // TIER 9: Regulus
            AddExecutor(ExecutorType.SpSummon, CardId.Regulus, RegulusSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.Regulus, RegulusEffect);

            // TIER 10: Cluster Trap
            AddExecutor(ExecutorType.Activate, CardId.Cluster, ClusterEffect);
            AddExecutor(ExecutorType.SpellSet, CardId.Cluster, DefensiveSetCheck);

            // TIER 11: Sets for reactive cards
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave, DefensiveSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.UrgentSchedule, DefensiveSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.Inclusion, DefensiveSetCheck);
            AddExecutor(ExecutorType.SpellSet, CardId.Switchyard, DefensiveSetCheck);

            // TIER 12: Repos
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand() => true; // Go first for Citree opponent-turn Synchro

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            ResetOPTFlags();
            
            if (ShouldGoBreakBoard)
            {
                // Going second TrainCryston: prioritize Dark Ruler + Lightning Storm โ’ Rank 10 OTK
                _gustavMaxBurned = false;
                _sulfefnirUsed = false;
            }
        }

        private void ResetOPTFlags()
        {
            _exceptionalScheduleActivated = false;
            _switchyardUsed = false;
            _citreeUsed = false;
            _smigerSearchUsed = false;
            _clusterUsed = false;
            _sulfefnirUsed = false;
            _sulfadorUsed = false;
            _bleuTravelerUsed = false;
            _convexKnightUsed = false;
            _babelDeckerUsed = false;
            _gustavMaxBurned = false;
            _riverStormerSearched = false;
            _geniusSearched = false;
            _opponentActivatedMonsterEffect = false;
            _tristarosUsed = false;
            _hasMachineSynchroLock = false;
            _hasMachineExtraDeckLock = false;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 1 && card != null && card.HasType(CardType.Monster) && Duel.Player == 0)
                _opponentActivatedMonsterEffect = true;
            
            // Track our own activations for OPT flags
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.Inclusion) _smigerSearchUsed = true;
                if (card.Id == CardId.Switchyard) _switchyardUsed = true;
                if (card.Id == CardId.Sulfefnir) _sulfefnirUsed = true;
                if (card.Id == CardId.Sulfador) _sulfadorUsed = true;
                if (card.Id == CardId.BleuTraveler && card.Location == CardLocation.Hand) _bleuTravelerUsed = true;
                if (card.Id == CardId.Citree) _citreeUsed = true;
                if (card.Id == CardId.Cluster) _clusterUsed = true;
                if (card.Id == CardId.ConvexKnight) _convexKnightUsed = true;
                if (card.Id == CardId.Tristaros) _tristarosUsed = true;
                if (card.Id == CardId.BabelDecker) _babelDeckerUsed = true;
            }
        }

        // โ•โ•โ• Ace & Priority โ•โ•โ•

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.AshBlossom, CardId.Fuwalos, CardId.ForbiddenDroplet, CardId.ForbiddenCrown))
                return 800;
            // Preserve key combo pieces
            if (c.IsCode(CardId.ScrapRecycler)) return 200;
            if (c.IsCode(CardId.Citree)) return 250;
            if (c.IsCode(CrystronMonsters)) return 300;
            if (c.IsCode(CardId.BulletTrain)) return 150;
            if (c.IsCode(CardId.ConvexKnight)) return 100;
            if (c.IsCode(CardId.BabelDecker)) return 120;
            return 50;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.DawnDragster) || Bot.HasInMonstersZone(CardId.Legatia))
                return true;
            if (Bot.HasInMonstersZone(CardId.SuperDora) || Bot.HasInMonstersZone(CardId.JuggernautLiebe))
                return true;
            if (Bot.HasInMonstersZone(CardId.GustavRocket) || Bot.HasInMonstersZone(CardId.FlyingLauncher))
                return true;
            if (Bot.HasInMonstersZone(CardId.Citree) && Bot.GetMonsterCount() >= 2)
                return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        // โ•โ•โ• TIER 1: Hand Traps & Counters โ•โ•โ•

        private bool FuwalosActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool SmartAshBlossom()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool DropletActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Player != 1 && Duel.Player != 0) return false;

            // Need cost cards to send
            bool hasCost = Bot.Hand.Any(c => c != null && c != Card)
                || Bot.GetMonsters().Any(c => c != null);
            if (!hasCost) return false;

            // Target the most threatening monster
            var target = Util.GetProblematicEnemyCard();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            // If going second and opp has big monsters
            if (Duel.Player == 0 && Duel.Turn == 2)
            {
                var bigMonster = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attack >= 2500);
                if (bigMonster != null)
                {
                    AI.SelectCard(bigMonster);
                    return true;
                }
            }
            return false;
        }

        private bool ForbiddenCrownActivate()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;
            if (LastChainCard.IsMonster())
            {
                AI.SelectCard(LastChainCard);
                return true;
            }
            return false;
        }

        private bool TTTActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Player == 0 && _opponentActivatedMonsterEffect
                && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                return true;
            return false;
        }

        private bool DarkRulerActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Player != 0) return false;
            if (Duel.Phase != DuelPhase.Main1) return false;
            // Going second board breaker โ€” negate all opponent monsters
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled()))
                return true;
            return false;
        }

        private bool LightningStormActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Duel.Player != 0) return false;
            if (Duel.Phase != DuelPhase.Main1) return false;

            // Need no face-up cards to activate from hand
            bool hasFaceup = Bot.GetMonsters().Any(c => c != null && c.IsFaceup())
                || Bot.GetSpells().Any(c => c != null && c.IsFaceup());
            if (hasFaceup) return false;

            // Prioritize destroying backrow
            if (Enemy.GetSpellCount() >= 2)
            {
                AI.SelectOption(1); // Destroy all Spells/Traps
                return true;
            }
            if (Enemy.GetMonsterCount() >= 2)
            {
                AI.SelectOption(0); // Destroy all monsters
                return true;
            }
            return false;
        }

        // โ•โ•โ• TIER 2: Field & Search Spells โ•โ•โ•

        private bool InclusionActivate()
        {
            if (ShouldSkipCombo()) return false;

            if (Card.Location == CardLocation.Hand)
            {
                // Already have one on field?
                if (Bot.HasInSpellZone(CardId.Inclusion)) return false;
                return true;
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Search effect โ€” search Smiger for engine start
                if (!_smigerSearchUsed && GetRemainingCount(CardId.Smiger) > 0)
                {
                    AI.SelectCard(CardId.Smiger);
                    _smigerSearchUsed = true;
                    return true;
                }
                // Search Sulfefnir if we already have Smiger
                if (!_smigerSearchUsed && GetRemainingCount(CardId.Sulfefnir) > 0)
                {
                    AI.SelectCard(CardId.Sulfefnir);
                    _smigerSearchUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool SwitchyardActivate()
        {
            if (_switchyardUsed) return false;
            if (ShouldSkipCombo()) return false;

            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.Switchyard)) return false;
                return true;
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                // Search Train card
                if (GetRemainingCount(CardId.BulletTrain) > 0 && !Bot.HasInHand(CardId.BulletTrain))
                {
                    AI.SelectCard(CardId.BulletTrain);
                }
                else if (GetRemainingCount(CardId.BleuTraveler) > 0 && !Bot.HasInHand(CardId.BleuTraveler))
                {
                    AI.SelectCard(CardId.BleuTraveler);
                }
                else
                    return false;
                _switchyardUsed = true;
                return true;
            }
            return false;
        }

        private bool ExceptionalScheduleActivate()
        {
            if (_exceptionalScheduleActivated) return false;
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;

            // Search Urgent Schedule
            if (GetRemainingCount(CardId.UrgentSchedule) > 0)
            {
                _exceptionalScheduleActivated = true;
                return true;
            }
            return false;
        }

        private bool UrgentScheduleActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Can be activated during opponent's turn
                if (Duel.Player == 1 && Enemy.GetMonsterCount() > Bot.GetMonsterCount())
                    return true;
                // During our turn: also usable if we need bodies
                if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                    return true;
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                // Quick-play from set: during opponent's turn
                if (Duel.Player == 1 && Enemy.GetMonsterCount() > Bot.GetMonsterCount())
                    return true;
            }
            return false;
        }

        // โ•โ•โ• Normal Summon Conditions โ•โ•โ•

        private bool SmigerSummon()
        {
            if (ShouldPrioritizeTrains()) return false;
            return true;
        }

        private bool CitreeSummon()
        {
            if (ShouldPrioritizeTrains()) return false;
            // Only normal summon Citree if we have a GY target for opponent-turn synchro
            if (Duel.Turn == 1)
            {
                bool hasGyTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && !c.HasType(CardType.Tuner) &&
                    (c.Level == 10 || c.Level == 5));
                if (!hasGyTarget) return false;
            }
            return true;
        }

        private bool TristarosSummon()
        {
            if (ShouldPrioritizeTrains()) return false;
            // Only if we have no better normal summon
            if (Bot.HasInHand(CardId.Smiger) || Bot.HasInHand(CardId.Citree)) return false;
            return true;
        }

        private bool SimpleSummon()
        {
            if (ShouldSkipCombo()) return false;
            return true;
        }

        // โ•โ•โ• Crystron Engine Activations โ•โ•โ•

        private bool SmigerFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Pop a card we control to search Crystron from deck
            bool hasTarget = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                (c.IsCode(CardId.Inclusion) && c.Location == CardLocation.SpellZone // This won't be in monster zone...
                 || CrystronMonsters.Contains(c.Id) && c.Id != CardId.Citree));
            var spellTarget = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.Inclusion));

            if (spellTarget != null)
            {
                _hasMachineSynchroLock = true;
                AI.SelectCard(spellTarget);
                return true;
            }
            var monsterTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() &&
                c.Id != CardId.Citree && CrystronMonsters.Contains(c.Id));
            if (monsterTarget != null)
            {
                _hasMachineSynchroLock = true;
                AI.SelectCard(monsterTarget);
                return true;
            }

            // Self-pop as last resort
            AI.SelectCard(Card);
            _hasMachineSynchroLock = true;
            return true;
        }

        private bool SmigerGraveEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;

            // Search Crystron S/T from deck
            if (HasRemainingCrystronCard())
            {
                // Priority: Inclusion > Cluster
                if (GetRemainingCount(CardId.Inclusion) > 0 && !Bot.HasInHand(CardId.Inclusion) && !Bot.HasInSpellZone(CardId.Inclusion))
                    AI.SelectCard(CardId.Inclusion);
                else if (GetRemainingCount(CardId.Cluster) > 0 && !Bot.HasInHand(CardId.Cluster) && !Bot.HasInSpellZone(CardId.Cluster))
                    AI.SelectCard(CardId.Cluster);
                else
                    return false;
                return true;
            }
            return false;
        }

        private bool SmigerHandEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Util.ChainContainsCard(CardId.Smiger)) return false;

            // Reveal 1 Crystron monster from hand to search Inclusion/Cluster
            bool hasCrystronToReveal = Bot.Hand.Any(c => c != null && c != Card && CrystronMonsters.Contains(c.Id));
            if (!hasCrystronToReveal) return false;

            bool hasTarget = GetRemainingCount(CardId.Inclusion) > 0 || GetRemainingCount(CardId.Cluster) > 0;
            if (!hasTarget) return false;

            // Don't search if we already have Inclusion active
            if (Bot.HasInSpellZone(CardId.Inclusion) && GetRemainingCount(CardId.Cluster) == 0) return false;

            return true;
        }

        private bool SulfefnirEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                if (_sulfefnirUsed) return false;
                // Discard 1 Crystron card to SS itself
                bool hasDiscard = Bot.Hand.Any(c => c != null && c != Card && CrystronMonsters.Contains(c.Id));
                if (!hasDiscard) return false;

                _sulfefnirUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Self-destruct to search Crystron monster from deck
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
                if (_sulfadorUsed) return false;
                // Must have card on field to destroy
                if (Bot.GetMonsterCount() + Bot.GetSpellCount() == 0) return false;

                bool hasCrystronInDeck = HasRemainingCrystronMonster();
                if (!hasCrystronInDeck) return false;

                // Select target to destroy
                ClientCard target = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.Inclusion));
                if (target == null)
                    target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && CrystronMonsters.Contains(c.Id) && c.Id != CardId.Citree);
                if (target == null)
                    target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());

                if (target != null)
                {
                    _hasMachineExtraDeckLock = true;
                    AI.SelectCard(target);
                    _sulfadorUsed = true;
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool TristarosEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (_tristarosUsed) return false;
                bool hasSynchro = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Synchro));
                bool hasDeckTargets = HasRemainingCrystronMonster();
                if (!hasSynchro || !hasDeckTargets) return false;

                _hasMachineExtraDeckLock = true;
                _tristarosUsed = true;
                return true;
            }
            // On-field quick effect
            if (Card.Location == CardLocation.MonsterZone)
            {
                bool hasTarget = HasRemainingCrystronMonster();
                return hasTarget;
            }
            return false;
        }

        private bool ScrapRecyclerEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // Send 1 Machine from deck to GY
            bool hasSulfefnir = GetRemainingCount(CardId.Sulfefnir) > 0 && !Bot.HasInGraveyard(CardId.Sulfefnir);
            bool hasBleuTraveler = GetRemainingCount(CardId.BleuTraveler) > 0;
            bool hasSulfador = GetRemainingCount(CardId.Sulfador) > 0;

            if (hasSulfefnir) return true;
            if (hasBleuTraveler && !Bot.HasInHand(CardId.BleuTraveler) && !Bot.HasInGraveyard(CardId.BleuTraveler))
                return true;
            if (hasSulfador) return true;
            return false;
        }

        // โ•โ•โ• Train / Earth Machine Engine โ•โ•โ•

        private bool BleuTravelerEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_bleuTravelerUsed) return false;
                if (ShouldSkipCombo()) return false;

                // Reveal to search Switchyard or Train Connection
                bool hasTarget = GetRemainingCount(CardId.Switchyard) > 0;
                if (!hasTarget) return false;

                _bleuTravelerUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Banish to revive EARTH Machine from GY
                bool hasOtherEarthMachine = Bot.Graveyard.Any(c => c != null && c != Card && c.IsMonster()
                    && c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth));
                if (!hasOtherEarthMachine) return false;

                _hasMachineExtraDeckLock = true;
                return true;
            }
            return false;
        }

        private bool BulletTrainSpSummon()
        {
            // Must control 1+ monster, all our monsters must be EARTH Machine
            int count = Bot.GetMonsterCount();
            if (count == 0) return false;
            return Bot.GetMonsters().All(c => c == null || (c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth)));
        }

        private bool BabelDeckerEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // On summon: SS 1 EARTH Machine from hand
            bool hasHandTarget = Bot.Hand.Any(c => c != null && c.IsMonster() &&
                c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth) && c != Card);
            return hasHandTarget;
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
                // SS from hand
                bool hasMachineOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Machine))
                                      || Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Machine));
                return hasMachineOnField;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Search EARTH Machine
                bool hasDeckTarget = GetRemainingCount(CardId.ScrapRecycler) > 0
                    || GetRemainingCount(CardId.BleuTraveler) > 0
                    || GetRemainingCount(CardId.ConvexKnight) > 0;
                return hasDeckTarget;
            }
            return false;
        }

        // โ•โ•โ• Link Bridges โ•โ•โ•

        private bool ClockworkKnightSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // Send Scrap Recycler or Citree to GY for follow-up plays
            bool hasScrapRecycler = Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.ScrapRecycler));
            bool hasCitree = Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.Citree));

            if (hasScrapRecycler || hasCitree) return true;
            return false;
        }

        private bool AngerKnuckleSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            int monsterCount = Bot.GetMonsterCount();
            return monsterCount >= 2 && Duel.Phase == DuelPhase.Main2;
        }

        private bool AngerKnuckleEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                bool hasLv10MachineInGY = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Level == 10
                    && c.HasRace(CardRace.Machine));
                if (!hasLv10MachineInGY) return false;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                bool canSend = Bot.Hand.Count > 0 || Bot.GetMonsters().Count > 0;
                return canSend;
            }
            return false;
        }

        private bool GeniusSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            return Bot.GetMonsterCount() >= 2;
        }

        private bool GeniusEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_geniusSearched) return false;
            // Search Regulus
            if (GetRemainingCount(CardId.Regulus) > 0)
            {
                _geniusSearched = true;
                return true;
            }
            return false;
        }

        // โ•โ•โ• Synchro Bosses โ•โ•โ•

        private bool LegatiaSpSummon()
        {
            if (_hasMachineExtraDeckLock) return false;
            if (ShouldSkipCombo()) return false;
            // LV12 Machine Synchro โ€” summon when we have strong board need
            if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
            return !_hasMachineExtraDeckLock;
        }

        private bool DawnDragsterSpSummon()
        {
            if (_hasMachineExtraDeckLock) return false;
            if (ShouldSkipCombo()) return false;
            // LV7 Machine Synchro โ€” omni-negate
            if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
            return !_hasMachineExtraDeckLock;
        }

        private bool EleskeletusSpSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (_hasMachineExtraDeckLock) return false;
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            return !_hasMachineExtraDeckLock;
        }

        private bool QuariongandraxSpSummon()
        {
            if (_hasMachineExtraDeckLock) return false;
            // LV9 Machine โ€” banish opponent monsters
            if (!OpponentHasThreateningMonster() && Enemy.GetMonsterCount() < 2) return false;
            return !_hasMachineExtraDeckLock;
        }

        private bool RiverStormerSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // Requires 2 Level 5 monsters
            return Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 5 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool RiverStormerEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_riverStormerSearched) return false;
            // Search EARTH Machine from deck
            bool hasTarget = GetRemainingCount(CardId.ScrapRecycler) > 0
                || GetRemainingCount(CardId.BleuTraveler) > 0
                || GetRemainingCount(CardId.ConvexKnight) > 0;
            if (hasTarget)
            {
                _riverStormerSearched = true;
                return true;
            }
            return false;
        }

        // โ•โ•โ• Rank 10/11 Xyz Bosses โ•โ•โ•

        private bool FlyingLauncherSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // 2 Level 10 monsters
            return Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool FlyingLauncherEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // Search + pop effect
            if (Card.Overlays.Count > 0 && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup()))
                return true;
            return false;
        }

        private bool GustavMaxSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            if (ShouldSkipCombo()) return false;
            if (Duel.Turn == 1 && Duel.Player == 0) return false;

            // 2 Level 10 EARTH Machines
            bool hasMats = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2;
            // Only summon if we can burn or make Liebe
            bool canBurn = Enemy.LifePoints <= 2000;
            bool canMakeLiebe = GetRemainingCount(CardId.JuggernautLiebe) > 0;
            return hasMats && (canBurn || canMakeLiebe);
        }

        private bool GustavMaxEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_gustavMaxBurned) return false;
            // Burn 2000 โ€” go for game
            if (Enemy.LifePoints <= 2000) return true;
            // Setup for Liebe OTK
            if (Enemy.LifePoints <= 4000 && GetRemainingCount(CardId.JuggernautLiebe) > 0)
                return true;
            return false;
        }

        private bool SuperDoraSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // 2 Level 10 EARTH Machines โ€” defensive play
            return Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool SuperDoraEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // Protect our strongest monster
            var target = Bot.GetMonsters().Where(c => c != null && c.IsFaceup())
                .OrderByDescending(c => c.Attack).FirstOrDefault();
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
            // Overlay on Gustav Max or Super Dora
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup()
                && (c.IsCode(CardId.GustavMax) || c.IsCode(CardId.SuperDora)));
        }

        private bool JuggernautLiebeEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // ATK pump โ€” only activate in MP2 after attacking or when no other attackers
            if (Duel.Phase == DuelPhase.Main1)
            {
                bool hasOtherAttackers = Bot.GetMonsters().Any(m => m != null && m.Id != Card.Id
                    && m.IsFaceup() && m.IsAttack() && !m.Attacked);
                if (hasOtherAttackers) return false;
            }
            return true;
        }

        private bool GustavRocketSpSummon()
        {
            if (_hasMachineSynchroLock) return false;
            // 2 Level 10 non-Xyz monsters
            return Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10 && !c.HasType(CardType.Xyz)) >= 2;
        }

        private bool GustavRocketEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // Pop face-up cards โ€” useful for clearing threats
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 1000))
                return true;
            return false;
        }

        private bool ZeusSpSummon()
        {
            // Zeus requires no locks
            if (_hasMachineSynchroLock || _hasMachineExtraDeckLock) return false;
            return Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Xyz) && c.Attacked);
        }

        private bool ZeusEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // Chain if targeted or opponent has overwhelming board
            if (Util.IsChainTarget(Card)) return true;

            int ourCards = Bot.GetMonsterCount() + Bot.GetSpellCount();
            int enemyCards = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
            return enemyCards >= 3 && enemyCards > ourCards;
        }

        // โ•โ•โ• Regulus โ•โ•โ•

        private bool RegulusSpSummon()
        {
            // SS by equipping Machine from GY
            if (Bot.GetSpellCount() >= 5) return false;
            if (!Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.HasRace(CardRace.Machine)))
                return false;
            return true;
        }

        private bool RegulusEffect()
        {
            if (Duel.Player == 1) return true; // Opponent's turn โ€” negate freely
            if (Duel.Player == 0 && OpponentHasThreateningMonster()) return true;
            return false;
        }

        // โ•โ•โ• Cluster โ•โ•โ•

        private bool ClusterEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.SpellZone)
            {
                if (_clusterUsed) return false;

                // SS 1 Crystron from hand/GY, then destroy 1 card we control
                bool hasSummonTarget = Bot.Hand.Concat(Bot.Graveyard)
                    .Any(c => c != null && c.IsMonster() && CrystronMonsters.Contains(c.Id));
                if (!hasSummonTarget) return false;

                bool hasPopTarget = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && CrystronMonsters.Contains(c.Id))
                    || Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.Inclusion) && c != Card);

                if (!hasPopTarget) return false;

                _clusterUsed = true;
                return true;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // Banish to dump 1 Crystron from deck to GY
                bool hasFaceupCrystron = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && CrystronMonsters.Contains(c.Id));
                if (!hasFaceupCrystron) return false;
                return HasRemainingCrystronMonster();
            }
            return false;
        }

        // โ•โ•โ• Engine Priority Detection โ•โ•โ•

        private enum EnginePriority { Crystron, Trains, Both, None }

        private EnginePriority GetPreferredEngine()
        {
            bool hasCrystronStarter = Bot.Hand.Any(c => c != null && c.IsCode(CardId.Citree, CardId.Smiger, CardId.Sulfefnir));
            bool hasCrystronField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && CrystronMonsters.Contains(c.Id));
            bool hasCrystronInDeck = HasRemainingCrystronMonster();

            bool hasTrainStarter = Bot.Hand.Any(c => c != null && c.IsCode(CardId.Switchyard, CardId.ExceptionalSchedule,
                CardId.ScrapRecycler, CardId.UrgentSchedule, CardId.BleuTraveler));
            bool hasTrainField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && !CrystronMonsters.Contains(c.Id)
                && c.HasRace(CardRace.Machine) && c.HasAttribute(CardAttribute.Earth));

            bool hasCrystron = (hasCrystronStarter || hasCrystronField) && hasCrystronInDeck;
            bool hasTrains = hasTrainStarter || hasTrainField;

            // Turn 1: prefer Crystron for defensive Citree setup, but fall back to Trains
            if (Duel.Turn == 1)
            {
                // Only force Crystron if we actually have a starter in hand
                bool hasCrystronInHand = Bot.Hand.Any(c => c != null && 
                    c.IsCode(CardId.Citree, CardId.Smiger, CardId.Inclusion, CardId.Sulfefnir, CardId.Sulfador, CardId.Tristaros));
                if (hasCrystron && hasCrystronInHand) return EnginePriority.Crystron;
                if (hasTrains) return EnginePriority.Trains;
                if (hasCrystronInHand) return EnginePriority.Crystron;
                return EnginePriority.None;
            }

            // Going second: prefer Trains for OTK
            if (_isGoingSecond && Duel.Phase == DuelPhase.Main1)
            {
                if (hasTrains && Enemy.LifePoints <= 8000) return EnginePriority.Trains;
                if (hasCrystron) return EnginePriority.Crystron;
                return hasTrains ? EnginePriority.Trains : EnginePriority.None;
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

        private bool OpponentHasThreateningMonster()
        {
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 2500 || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
        }

        // โ•โ•โ• Utility โ•โ•โ•

        private bool DefensiveSetCheck()
        {
            if (Duel.Player != 0) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (Bot.GetSpellCountWithoutField() >= 5) return false;
            if (Duel.Phase == DuelPhase.Main2) return true;
            // Only set in MP1 if no other actions
            if (Main != null && (Main.ActivableCards.Count > 0 || Main.SummonableCards.Count > 0
                || Main.SpecialSummonableCards.Count > 0))
                return false;
            return true;
        }

        private bool HasRemainingCrystronMonster()
        {
            return CrystronMonsters.Any(id => GetRemainingCount(id) > 0)
                || HasRemainingMonsterWithSetcode(0xea);
        }

        private bool HasRemainingCrystronCard()
        {
            return HasRemainingCrystronMonster()
                || GetRemainingCount(CardId.Inclusion) > 0
                || GetRemainingCount(CardId.Cluster) > 0
                || HasRemainingCardWithSetcode(0xea);
        }

        private static bool IsCrystronCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(CrystronMonsters)
                || card.IsCode(CardId.Inclusion, CardId.Cluster)
                || card.HasSetcode(0xea);
        }

        // โ•โ•โ• OnSelectCard โ•โ•โ•

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (Card != null)
            {
                // Scrap Recycler dump targets
                if (Card.Id == CardId.ScrapRecycler)
                {
                    EnginePriority engine = GetPreferredEngine();
                    ClientCard target = null;

                    if (engine == EnginePriority.Crystron || engine == EnginePriority.Both)
                        target = cards.FirstOrDefault(c => c.Id == CardId.Sulfefnir);
                    if (target == null)
                        target = cards.FirstOrDefault(c => c.Id == CardId.Sulfador);
                    if (target == null)
                        target = cards.FirstOrDefault(c => c.Id == CardId.BleuTraveler);
                    if (target == null)
                        target = cards.FirstOrDefault(c => c.IsMonster());

                    if (target != null) return new List<ClientCard> { target };
                }

                // Switchyard search
                if (Card.Id == CardId.Switchyard && cards.Any(c => c.Location == CardLocation.Deck))
                {
                    ClientCard target = cards.FirstOrDefault(c => c.Id == CardId.BulletTrain);
                    if (target == null)
                        target = cards.FirstOrDefault(c => c.Id == CardId.BleuTraveler);
                    if (target != null) return new List<ClientCard> { target };
                }

                // Inclusion search
                if (Card.Id == CardId.Inclusion && cards.Any(c => c.Location == CardLocation.Deck))
                {
                    ClientCard target = cards.FirstOrDefault(c => c.Id == CardId.Smiger);
                    if (target == null)
                        target = cards.FirstOrDefault(c => c.Id == CardId.Sulfefnir);
                    if (target != null) return new List<ClientCard> { target };
                }

                // Urgent Schedule targets
                if (Card.Id == CardId.UrgentSchedule)
                {
                    var result = new List<ClientCard>();
                    ClientCard lvl4 = cards.FirstOrDefault(c => c.IsMonster() && c.Level <= 4
                        && c.HasAttribute(CardAttribute.Earth) && c.HasRace(CardRace.Machine));
                    ClientCard lvl5 = cards.FirstOrDefault(c => c.IsMonster() && c.Level >= 5
                        && c.HasAttribute(CardAttribute.Earth) && c.HasRace(CardRace.Machine));

                    if (lvl4 != null) result.Add(lvl4);
                    if (lvl5 != null) result.Add(lvl5);
                    if (result.Count >= min && result.Count <= max) return result;
                }

                // Qliphort Genius search
                if (Card.Id == CardId.QliphortGenius)
                {
                    ClientCard target = cards.FirstOrDefault(c => c.Id == CardId.Regulus);
                    if (target != null) return new List<ClientCard> { target };
                }

                // Smiger hand effect: search Inclusion or Cluster
                if (Card.Id == CardId.Smiger && cards.Any(c => c.Location == CardLocation.Deck))
                {
                    bool hasInclusionOnField = Bot.HasInSpellZone(CardId.Inclusion);
                    bool hasInclusionInHand = Bot.HasInHand(CardId.Inclusion);
                    if (!hasInclusionOnField && !hasInclusionInHand)
                    {
                        ClientCard target = cards.FirstOrDefault(c => c.Id == CardId.Inclusion);
                        if (target != null) return new List<ClientCard> { target };
                    }
                    ClientCard cluster = cards.FirstOrDefault(c => c.Id == CardId.Cluster);
                    if (cluster != null) return new List<ClientCard> { cluster };
                }

                // Regulus equip target
                if (Card.Id == CardId.Regulus)
                {
                    ClientCard target = cards.FirstOrDefault(c => IsCrystronCard(c) && c.IsMonster());
                    if (target == null)
                        target = cards.FirstOrDefault(c => c.IsMonster());
                    if (target != null) return new List<ClientCard> { target };
                }
            }

            // Generic: sort by material priority
            if (max > 0)
            {
                var sorted = cards.OrderBy(GetMaterialPriority).ToList();
                var result = sorted.Take(Math.Min(max, sorted.Count)).ToList();
                if (result.Count >= min) return result;
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            var sorted = cards.Where(c => c != null)
                .OrderBy(c => GetMaterialPriority(c)).ToList();
            var safe = sorted.Where(c => !IsAceCard(c)).ToList();
            if (safe.Count >= min)
                return Util.CheckSelectCount(safe, cards, min, max);
            return base.OnSelectSynchroMaterial(cards, sum, min, max);
        }

        public override bool OnSelectYesNo(long desc)
        {
            if (desc == CardId.Inclusion || desc == Util.GetStringId(CardId.Inclusion, 0))
                return true;
            return base.OnSelectYesNo(desc);
        }

        public override int OnSelectOption(IList<long> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                long cardId = options[i] >> 20;
                long optIndex = options[i] & 0xfffff;

                if (cardId == CardId.LightningStorm)
                {
                    // Option 0 = destroy monsters, Option 1 = destroy spells/traps
                    if (Enemy.GetSpellCount() >= 2 && optIndex == 1) return i;
                    if (Enemy.GetMonsterCount() >= 2 && optIndex == 0) return i;
                }
                if (cardId == CardId.TTT)
                {
                    // Option 0 = draw, Option 1 = control, Option 2 = hand rip
                    if (Bot.Hand.Count <= 2 && optIndex == 0) return i;
                    if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500) && optIndex == 1) return i;
                    if (optIndex == 0) return i;
                }
            }
            return 0;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (AceCardIds.Contains(cardId) && positions.Contains(CardPosition.FaceUpAttack))
                return CardPosition.FaceUpAttack;
            return base.OnSelectPosition(cardId, positions);
        }

        // โ•โ•โ• Repos โ•โ•โ•

        private bool MonsterRepos()
        {
            if (Card == null) return false;
            if (IsAceCard(Card))
            {
                if (Card.IsDefense()) return true;
                return false;
            }
            bool enemyEmpty = Enemy.GetMonsterCount() == 0;
            if (Card.IsAttack())
            {
                if (!enemyEmpty && Card.Attack <= 1000 && Card.Defense > 0)
                    return true;
            }
            else
            {
                if (enemyEmpty || Card.Attack >= 1500)
                    return true;
            }
            return false;
        }
    }
}
