// ====================================================================================================
// CARD AUDIT — 2026 Kaiju Crusadia Comprehensive AI Executor
// ====================================================================================================
// Goal: Go 2nd, break board with Kaijus/Slumber/Twin Twisters, OTK with Equimax + Maximus + Leonis.
// Uses local dynamic evaluation and turn objectives.
// ====================================================================================================

using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_KaijuCrusadia", "2026_KaijuCrusadia")]
    public class _2026_KaijuCrusadiaExecutor : ModernExecutor
    {
        // ================================================================================================
        // Card ID Constants Definition
        // ================================================================================================
        public class CardId
        {
            // Crusadia Monsters
            public const int CrusadiaReclusia = 55241609;
            public const int CrusadiaArboria = 91646304;
            public const int CrusadiaLeonis = 28031913;
            public const int CrusadiaDraco = 54525057;
            public const int CrusadiaMaximus = 81524756;

            // Mekk-Knight Monsters & World Crown
            public const int MekkKnightPurple = 28692962;
            public const int MekkKnightBlue = 20537097;
            public const int MekkKnightIndigo = 92204263;
            public const int WorldCrown = 27918365;

            // Kaiju Monsters
            public const int Gameciel = 55063751;
            public const int Kumongous = 29726552;
            public const int Gadarla = 36956512;
            public const int Radian = 28674152;

            // Spells
            public const int InterruptedKaijuSlumber = 99330325;
            public const int WorldLegacySuccession = 99674361;
            public const int TwinTwisters = 43898403;
            public const int CalledByTheGrave = 24224830;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int MonsterReborn = 83764719;
            public const int CosmicCyclone = 8267140;
            public const int CrusadiaPower = 96434581;
            public const int CrusadiaRevival = 69039982;
            public const int CrusadiaTestament = 87497553;

            // Handtraps & Traps
            public const int AshBlossom = 14558127;
            public const int EffectVeiler = 97268402;
            public const int ArtifactLancea = 34267821;
            public const int EvenlyMatched = 15693423;
            public const int MaxxC = 23434538;

            // Extra Deck Link Monsters
            public const int CrusadiaMagius = 72228247;
            public const int CrusadiaRegulex = 9617996;
            public const int CrusadiaSpatha = 39528955;
            public const int CrusadiaEquimax = 45002991;
            public const int Avramax = 21887175;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareCerberus = 75452921;
            public const int KnightmareUnicorn = 38342335;
            public const int BorrelswordDragon = 85289965;
            public const int TopologicTrisbaena = 72529749;
            public const int SaryujaSkullDread = 74997493;
        }

        // ================================================================================================
        // State Variable & List Initializations
        // ================================================================================================
        private static readonly int[] AceCardIds = {
            CardId.CrusadiaEquimax, CardId.Avramax, CardId.BorrelswordDragon, CardId.SaryujaSkullDread
        };

        private static readonly int[] CrusadiaMainMonsters = {
            CardId.CrusadiaReclusia, CardId.CrusadiaArboria, CardId.CrusadiaLeonis,
            CardId.CrusadiaDraco, CardId.CrusadiaMaximus
        };

        private static readonly int[] KaijuMonsters = {
            CardId.Gameciel, CardId.Kumongous, CardId.Gadarla, CardId.Radian
        };

        private static readonly int[] MekkKnightMonsters = {
            CardId.MekkKnightPurple, CardId.MekkKnightBlue, CardId.MekkKnightIndigo
        };

        private enum TurnObjective { EstablishBoard, BreakBoard, PushLethal, Survive }
        private TurnObjective _objective;

        private bool _normalSummonedCrusadia = false;
        private bool _slumberUsed = false;
        private bool _maximusUsed = false;
        private bool _leonisUsed = false;
        private bool _revivalUsed = false;

        // Target state memory to fix asynchronous selection leaks during Kaiju summons
        private ClientCard _kaijuTributeTarget = null;

        // ================================================================================================
        // Main Constructor: Setup Strategy & Register Executors
        // ================================================================================================
        public _2026_KaijuCrusadiaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            AI?.Log(LogLevel.Info, "[INIT] Initializing 2026 Kaiju Crusadia Executor...");

            // 1. Register Ace Cards under the Resource Planner module
            ResourcePlan.RegisterAceCards(AceCardIds);

            // 2. Register combo lines with fallback strategies
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Crusadia-Climb",
                RequiredCards = new List<int> { }, // Any 2 Crusadias in hand dynamically
                FallbackLineName = "Kaiju-Beatdown",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { ActionType = ExecutorType.Summon, Description = "Normal Summon Crusadia Starter" },
                    new() { CardId = CardId.CrusadiaMagius, ActionType = ExecutorType.SpSummon, Description = "Link into Magius" },
                    new() { ActionType = ExecutorType.SpSummon, Description = "SS Crusadia to Magius pointer zone" },
                    new() { CardId = CardId.CrusadiaRegulex, ActionType = ExecutorType.SpSummon, Description = "Link into Regulex" },
                    new() { ActionType = ExecutorType.SpSummon, Description = "SS Crusadia to Regulex pointer zone" },
                    new() { CardId = CardId.CrusadiaEquimax, ActionType = ExecutorType.SpSummon, Description = "Link into Equimax" }
                },
                EndBoardScore = 100
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Kaiju-Beatdown",
                RequiredCards = new List<int> { }, 
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { ActionType = ExecutorType.SpSummon, Description = "Fallback: Tribute enemy with Kaiju and attack directly" }
                },
                EndBoardScore = 40,
                Condition = () => Bot.Hand.Any(c => KaijuMonsters.Contains(c.Id))
            });

            // 3. Register Starter and Bait cards with the Bait Planner
            BaitPlanner.RegisterComboStarters(
                CardId.CrusadiaMagius, CardId.CrusadiaRegulex, CardId.InterruptedKaijuSlumber
            );
            BaitPlanner.RegisterBaitCards(
                CardId.TwinTwisters, CardId.CrusadiaTestament, CardId.CosmicCyclone
            );

            // ============================================================================================
            // Register Executors (Hierarchical Tiers)
            // ============================================================================================

            // TIER 1: Hand Traps, Direct Countering & Called By The Grave
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, EffectVeilerActivate);
            AddExecutor(ExecutorType.Activate, CardId.ArtifactLancea, LanceaActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);

            // TIER 2: Board-Breaking Spells & Traps
            AddExecutor(ExecutorType.Activate, CardId.TwinTwisters, TwinTwistersActivate);
            AddExecutor(ExecutorType.Activate, CardId.InterruptedKaijuSlumber, SlumberActivate);
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotaActivate);
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedActivate);
            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, CosmicCycloneActivate);
            AddExecutor(ExecutorType.Activate, CardId.WorldLegacySuccession, SuccessionActivate);

            // TIER 3: Kaiju Tributes
            AddExecutor(ExecutorType.SpSummon, KaijuSummon);

            // TIER 4: Link Climbs (Magius, Regulex, Spatha)
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaMagius, MagiusSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaRegulex, RegulexSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaSpatha, SpathaSummon);

            // TIER 5: Knightmares & Generic Extra Deck Support
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, KnightmarePhoenixSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareCerberus, KnightmareCerberusSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, KnightmareUnicornSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TopologicTrisbaena, TrisbaenaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SaryujaSkullDread, SaryujaSummon);

            // TIER 6: Boss Monsters (Equimax, Avramax, Borrelsword)
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaEquimax, EquimaxSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Avramax, AvramaxSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BorrelswordDragon, BorrelswordSummon);

            // TIER 7: Crusadia Special Summons from Hand (Extenders)
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaReclusia, CrusadiaHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaArboria, CrusadiaHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaLeonis, CrusadiaHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaDraco, CrusadiaHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaMaximus, CrusadiaHandSpSummon);

            // Mekk-Knights & World Crown
            AddExecutor(ExecutorType.SpSummon, CardId.MekkKnightPurple);
            AddExecutor(ExecutorType.SpSummon, CardId.MekkKnightBlue);
            AddExecutor(ExecutorType.SpSummon, CardId.MekkKnightIndigo);
            AddExecutor(ExecutorType.SpSummon, CardId.WorldCrown);

            AddExecutor(ExecutorType.Activate, CardId.MekkKnightPurple, PurpleActivate);
            AddExecutor(ExecutorType.Activate, CardId.MekkKnightBlue, BlueActivate);
            AddExecutor(ExecutorType.Activate, CardId.MekkKnightIndigo, IndigoActivate);

            AddExecutor(ExecutorType.Activate, CardId.CrusadiaReclusia, ReclusiaActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaArboria, ArboriaActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaLeonis, LeonisActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaDraco, DracoActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaMaximus, MaximusActivate);

            // TIER 8: Recovery, Protect & Damage Boosters
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, MonsterRebornActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaPower, PowerActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaRevival, RevivalActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaTestament, TestamentActivate);

            // TIER 9: Normal Summon
            AddExecutor(ExecutorType.Summon, CrusadiaNormalSummon);

            // Spell/Trap Sets and Repositioning
            AddExecutor(ExecutorType.SpellSet, SpellSetLogic);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        // ================================================================================================
        // Duel Setup and Turn Operations
        // ================================================================================================
        public override bool OnSelectHand() => false; // We want to go second for OTK

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _normalSummonedCrusadia = false;
            _slumberUsed = false;
            _maximusUsed = false;
            _leonisUsed = false;
            _revivalUsed = false;
            _kaijuTributeTarget = null;

            // ── Going-Second BreakBoard: prioritize disruption over combo ──
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
                _slumberUsed = false;
                _maximusUsed = false;
            }

            DetermineObjective();
            LogCombatState();
        }

        private void DetermineObjective()
        {
            if (Duel.Turn <= 1)
            {
                _objective = TurnObjective.EstablishBoard;
                return;
            }

            if (OTKDamageCalculator.EvaluateLethal(Bot, Enemy))
            {
                _objective = TurnObjective.PushLethal;
                return;
            }

            if (Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0)
            {
                _objective = TurnObjective.BreakBoard;
                return;
            }

            if (Bot.LifePoints < 2000)
            {
                _objective = TurnObjective.Survive;
                return;
            }

            _objective = TurnObjective.EstablishBoard;
        }

        // ================================================================================================
        // Dynamic Local Decision Engine (Decision Before Execution)
        // ================================================================================================
        private class ActionCandidate
        {
            public MainPhaseAction Action;
            public ClientCard Card;
            public int Score;
            public string Description;
        }

        public override MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            DetermineObjective();
            DynamicLethalCheck();

            List<ActionCandidate> candidates = new List<ActionCandidate>();

            // 1. Evaluate Activable Cards
            for (int i = 0; i < main.ActivableCards.Count; ++i)
            {
                var card = main.ActivableCards[i];
                if (card == null) continue;
                int score = ScoreActivableCard(card, main.ActivableDescs[i]);
                if (score > 0)
                {
                    candidates.Add(new ActionCandidate
                    {
                        Action = new MainPhaseAction(MainPhaseAction.MainAction.Activate, card.ActionActivateIndex[main.ActivableDescs[i]]),
                        Card = card,
                        Score = score,
                        Description = $"Activate {card.Name} (Score: {score})"
                    });
                }
            }

            // 2. Evaluate Normal Summons
            foreach (var card in main.SummonableCards)
            {
                if (card == null) continue;
                int score = ScoreSummonCard(card);
                if (score > 0)
                {
                    candidates.Add(new ActionCandidate
                    {
                        Action = new MainPhaseAction(MainPhaseAction.MainAction.Summon, card.ActionIndex),
                        Card = card,
                        Score = score,
                        Description = $"Summon {card.Name} (Score: {score})"
                    });
                }
            }

            // 3. Evaluate Special Summons
            foreach (var card in main.SpecialSummonableCards)
            {
                if (card == null) continue;
                int score = ScoreSpecialSummonCard(card);
                if (score > 0)
                {
                    candidates.Add(new ActionCandidate
                    {
                        Action = new MainPhaseAction(MainPhaseAction.MainAction.SpSummon, card.ActionIndex),
                        Card = card,
                        Score = score,
                        Description = $"SpSummon {card.Name} (Score: {score})"
                    });
                }
            }

            // Execute best candidate if score > 0
            if (candidates.Count > 0)
            {
                var best = candidates.OrderByDescending(c => c.Score).First();
                AI?.Log(LogLevel.Info, $"[DECISION-ENGINE] Selected Action: {best.Description} (Objective: {_objective})");

                if (best.Card != null)
                {
                    if (best.Card.Id == CardId.CrusadiaMaximus && best.Action.Action == MainPhaseAction.MainAction.SpSummon)
                        _maximusUsed = true;
                    if (best.Card.Id == CardId.CrusadiaLeonis && best.Action.Action == MainPhaseAction.MainAction.SpSummon)
                        _leonisUsed = true;
                }

                return best.Action;
            }

            return base.OnSelectIdleCmd(main);
        }

        // ================================================================================================
        // Local Evaluators
        // ================================================================================================
        private int ScoreActivableCard(ClientCard card, long desc)
        {
            if (card.Id == CardId.InterruptedKaijuSlumber)
            {
                if (_slumberUsed) return 0;
                if (Enemy.GetMonsterCount() == 0) return 0;
                if (OpponentThreatEvaluator.HasNegatorOrFloodgate(Enemy)) return 95;
                return 75;
            }

            if (card.Id == CardId.TwinTwisters)
            {
                int enemyBackrow = Enemy.GetSpellCount();
                if (enemyBackrow == 0) return 0;
                if (enemyBackrow >= 2) return 80;
                return 60;
            }

            if (card.Id == CardId.ReinforcementOfTheArmy)
            {
                return 70;
            }

            if (card.Id == CardId.WorldLegacySuccession)
            {
                int pointedZones = 0;
                for (int i = 0; i < 5; ++i)
                {
                    if (IsZonePointedToByAnyLink(i))
                        pointedZones |= (1 << i);
                }
                if ((GetEmptyZones() & pointedZones) > 0 && Bot.Graveyard.Any(c => CrusadiaMainMonsters.Contains(c.Id)))
                {
                    return 85;
                }
                return 0;
            }

            if (card.Id == CardId.MonsterReborn)
            {
                if (Bot.Graveyard.Any(c => c.Id == CardId.CrusadiaEquimax)) return 80;
                if (Bot.Graveyard.Any(c => CrusadiaMainMonsters.Contains(c.Id))) return 70;
                return 20;
            }

            if (card.Id == CardId.CrusadiaTestament)
            {
                if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup())) return 78;
                return 40;
            }

            if (card.Id == CardId.CrusadiaRevival)
            {
                if (card.Location == CardLocation.SpellZone && card.IsFaceup())
                {
                    if (_revivalUsed) return 0;
                    if (Bot.MonsterZone.Any(c => c != null && c.Id == CardId.CrusadiaEquimax) && _objective == TurnObjective.PushLethal)
                    {
                        return 90;
                    }
                    return 0;
                }
                if (card.Location == CardLocation.Hand)
                {
                    if (Bot.SpellZone[5] == null) return 82;
                    return 10;
                }
                return 0;
            }

            if (card.Id == CardId.CosmicCyclone)
            {
                if (Enemy.GetSpellCount() > 0) return 65;
                return 0;
            }

            if (card.Id == CardId.CrusadiaPower)
            {
                if (Bot.MonsterZone.Any(c => c != null && c.Id == CardId.CrusadiaEquimax) && _objective == TurnObjective.PushLethal)
                {
                    return 30;
                }
                return 5;
            }

            if (card.Id == CardId.CrusadiaReclusia)
            {
                if (Util.GetProblematicEnemyCard() != null) return 80;
                return 0;
            }

            if (card.Id == CardId.CrusadiaLeonis)
            {
                if (_leonisUsed) return 0;
                if (Bot.MonsterZone.Any(c => c != null && c.Id == CardId.CrusadiaEquimax) && _objective == TurnObjective.PushLethal)
                    return 85;
                return 0;
            }

            if (card.Id == CardId.CrusadiaMaximus)
            {
                if (_maximusUsed) return 0;
                if (Bot.MonsterZone.Any(c => c != null && c.Id == CardId.CrusadiaEquimax) && _objective == TurnObjective.PushLethal)
                    return 92;
                return 0;
            }

            if (card.Id == CardId.CrusadiaDraco)
            {
                if (Bot.Graveyard.Any(c => CrusadiaMainMonsters.Contains(c.Id) && c.Id != CardId.CrusadiaDraco))
                    return 86;
                return 0;
            }

            return 0;
        }

        private int ScoreSummonCard(ClientCard card)
        {
            if (CrusadiaMainMonsters.Contains(card.Id))
            {
                if (!_normalSummonedCrusadia)
                {
                    if (Bot.GetMonsterCount() == 0) return 90;
                    return 50;
                }
            }
            return 0;
        }

        private int ScoreSpecialSummonCard(ClientCard card)
        {
            if (card.Location == CardLocation.Extra)
            {
                if (card.Id == CardId.CrusadiaMagius || card.Id == CardId.CrusadiaRegulex || card.Id == CardId.CrusadiaSpatha)
                {
                    if (Bot.HasInMonstersZone(CardId.CrusadiaEquimax) || Bot.HasInMonstersZone(CardId.Avramax))
                        return 0;
                }

                if (card.Id == CardId.CrusadiaMagius)
                {
                    if (Bot.HasInMonstersZone(CardId.CrusadiaMagius)) return 0;
                    if (Bot.GetMonsters().Any(c => CrusadiaMainMonsters.Contains(c.Id))) return 85;
                    return 0;
                }

                if (card.Id == CardId.CrusadiaRegulex)
                {
                    if (Bot.HasInMonstersZone(CardId.CrusadiaRegulex)) return 0;
                    if (Bot.HasInMonstersZone(CardId.CrusadiaMagius) && Bot.GetMonsterCount() >= 2) return 90;
                    return 0;
                }

                if (card.Id == CardId.CrusadiaSpatha)
                {
                    if (Bot.HasInMonstersZone(CardId.CrusadiaSpatha)) return 0;
                    if (Bot.HasInMonstersZone(CardId.CrusadiaMagius) && Bot.GetMonsterCount() >= 2) return 88;
                    return 0;
                }

                if (card.Id == CardId.CrusadiaEquimax)
                {
                    if (Bot.HasInMonstersZone(CardId.CrusadiaEquimax)) return 0;
                    bool hasMaterialLink = Bot.MonsterZone.Any(c => c != null && (c.Id == CardId.CrusadiaRegulex || c.Id == CardId.CrusadiaSpatha));
                    if (hasMaterialLink && Bot.GetMonsterCount() >= 2) return 95;
                    return 0;
                }

                if (card.Id == CardId.Avramax)
                {
                    if (Bot.HasInMonstersZone(CardId.Avramax)) return 0;
                    if (Bot.HasInMonstersZone(CardId.CrusadiaEquimax) && _objective != TurnObjective.PushLethal) return 75;
                    return 0;
                }

                if (card.Id == CardId.BorrelswordDragon)
                {
                    if (Bot.GetMonsterCount() >= 3 && _objective == TurnObjective.PushLethal) return 80;
                    return 0;
                }

                if (card.Id == CardId.SaryujaSkullDread)
                {
                    if (Bot.GetMonsterCount() >= 4) return 70;
                    return 0;
                }
            }

            if (card.Location == CardLocation.Hand)
            {
                if (CrusadiaMainMonsters.Contains(card.Id))
                {
                    int pointedZones = 0;
                    for (int i = 0; i < 5; ++i)
                    {
                        if (IsZonePointedToByAnyLink(i))
                            pointedZones |= (1 << i);
                    }
                    int targetZone = GetEmptyZones() & pointedZones;
                    if (targetZone > 0)
                    {
                        if (card.Id == CardId.CrusadiaDraco) return 88;
                        if (card.Id == CardId.CrusadiaMaximus) return 87;
                        if (card.Id == CardId.CrusadiaArboria) return 84;
                        if (card.Id == CardId.CrusadiaLeonis) return 82;
                        if (card.Id == CardId.CrusadiaReclusia) return 80;
                        return 75;
                    }
                }

                if (MekkKnightMonsters.Contains(card.Id))
                {
                    if (card.Id == CardId.MekkKnightPurple) return 75;
                    if (card.Id == CardId.MekkKnightBlue) return 74;
                    if (card.Id == CardId.MekkKnightIndigo) return 70;
                    return 60;
                }

                if (card.Id == CardId.WorldCrown)
                {
                    int pointedZones = 0;
                    for (int i = 0; i < 5; ++i)
                    {
                        if (IsZonePointedToByAnyLink(i))
                            pointedZones |= (1 << i);
                    }
                    if ((GetEmptyZones() & pointedZones) > 0) return 72;
                }

                if (KaijuMonsters.Contains(card.Id))
                {
                    if (OpponentThreatEvaluator.HasNegatorOrFloodgate(Enemy)) return 96;
                    if (_isGoingSecond && Enemy.GetMonsterCount() > 0) return 60;
                    return 10;
                }
            }

            return 0;
        }

        // ================================================================================================
        // Helper: Calculate Empty Monster Zones as bitmask
        // ================================================================================================
        private int GetEmptyZones()
        {
            int zones = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (Bot.MonsterZone[i] == null)
                    zones |= (1 << i);
            }
            return zones;
        }

        // ================================================================================================
        // Link Pointer Engine & Zone Logic
        // ================================================================================================
        public bool IsZonePointedToByAnyLink(int zone)
        {
            var BotMZone = Bot.MonsterZone;
            var EnemyMZone = Enemy.MonsterZone;

            switch (zone)
            {
                case 0:
                    return (BotMZone[1]?.HasLinkMarker(CardLinkMarker.Left) ?? false) ||
                           (BotMZone[5]?.HasLinkMarker(CardLinkMarker.BottomLeft) ?? false) ||
                           (EnemyMZone[6]?.HasLinkMarker(CardLinkMarker.TopRight) ?? false);
                case 1:
                    return (BotMZone[0]?.HasLinkMarker(CardLinkMarker.Right) ?? false) ||
                           (BotMZone[2]?.HasLinkMarker(CardLinkMarker.Left) ?? false) ||
                           (BotMZone[5]?.HasLinkMarker(CardLinkMarker.Bottom) ?? false) ||
                           (EnemyMZone[6]?.HasLinkMarker(CardLinkMarker.Top) ?? false);
                case 2:
                    return (BotMZone[1]?.HasLinkMarker(CardLinkMarker.Right) ?? false) ||
                           (BotMZone[3]?.HasLinkMarker(CardLinkMarker.Left) ?? false) ||
                           (BotMZone[5]?.HasLinkMarker(CardLinkMarker.BottomRight) ?? false) ||
                           (EnemyMZone[6]?.HasLinkMarker(CardLinkMarker.TopLeft) ?? false) ||
                           (BotMZone[6]?.HasLinkMarker(CardLinkMarker.BottomLeft) ?? false) ||
                           (EnemyMZone[5]?.HasLinkMarker(CardLinkMarker.TopRight) ?? false);
                case 3:
                    return (BotMZone[2]?.HasLinkMarker(CardLinkMarker.Right) ?? false) ||
                           (BotMZone[4]?.HasLinkMarker(CardLinkMarker.Left) ?? false) ||
                           (BotMZone[6]?.HasLinkMarker(CardLinkMarker.Bottom) ?? false) ||
                           (EnemyMZone[5]?.HasLinkMarker(CardLinkMarker.Top) ?? false);
                case 4:
                    return (BotMZone[3]?.HasLinkMarker(CardLinkMarker.Right) ?? false) ||
                           (BotMZone[6]?.HasLinkMarker(CardLinkMarker.BottomRight) ?? false) ||
                           (EnemyMZone[5]?.HasLinkMarker(CardLinkMarker.TopLeft) ?? false);
            }
            return false;
        }

        private int GetIdealCrusadiaZone(int available)
        {
            var magius = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaMagius);
            var regulex = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaRegulex);
            var spatha = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaSpatha);
            var equimax = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);

            if (magius != null)
            {
                int targetZone = (magius.Sequence == 5) ? 1 : 3;
                if ((available & (1 << targetZone)) > 0) return 1 << targetZone;
            }

            if (regulex != null)
            {
                int targetZone = (regulex.Sequence == 5) ? 1 : 3;
                if ((available & (1 << targetZone)) > 0) return 1 << targetZone;
            }

            if (spatha != null)
            {
                int targetZone = (spatha.Sequence == 5) ? 1 : 3;
                if ((available & (1 << targetZone)) > 0) return 1 << targetZone;
            }

            if (equimax != null)
            {
                int z1 = (equimax.Sequence == 5) ? 0 : 2;
                int z2 = (equimax.Sequence == 5) ? 2 : 4;
                if ((available & (1 << z1)) > 0) return 1 << z1;
                if ((available & (1 << z2)) > 0) return 1 << z2;
            }

            return 0;
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            if (player == 1 && location == CardLocation.MonsterZone)
            {
                var equimax = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
                if (equimax != null)
                {
                    int opponentZones = (equimax.GetLinkedZones() >> 16) & 0x1F;
                    int intersection = available & opponentZones;
                    if (intersection > 0)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if ((intersection & (1 << i)) > 0)
                            {
                                AI?.Log(LogLevel.Info, $"[KAIJU-ZONE] Placing Kaiju in Equimax pointer zone: opponent zone {i}");
                                return 1 << i;
                            }
                        }
                    }
                }
                else
                {
                    if ((available & (1 << 3)) > 0)
                    {
                        AI?.Log(LogLevel.Info, "[KAIJU-ZONE] Placing Kaiju in opponent zone 3 (corresponds to left EMZ pointer)");
                        return 1 << 3;
                    }
                    if ((available & (1 << 1)) > 0)
                    {
                        AI?.Log(LogLevel.Info, "[KAIJU-ZONE] Placing Kaiju in opponent zone 1 (corresponds to right EMZ pointer)");
                        return 1 << 1;
                    }
                }
            }

            if (player == 0 && location == CardLocation.MonsterZone)
            {
                if (CrusadiaMainMonsters.Contains((int)cardId))
                {
                    int idealZone = GetIdealCrusadiaZone(available);
                    if (idealZone > 0)
                    {
                        AI?.Log(LogLevel.Info, $"[LINK-ZONE] Placing Crusadia {cardId} in ideal pointer zone.");
                        return idealZone;
                    }

                    int pointedZones = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (IsZonePointedToByAnyLink(i))
                            pointedZones |= (1 << i);
                    }

                    int emptyPointed = GetEmptyZones() & pointedZones;
                    int intersection = available & emptyPointed;
                    if (intersection > 0)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if ((intersection & (1 << i)) > 0)
                            {
                                AI?.Log(LogLevel.Info, $"[LINK-ZONE] Placing Crusadia {cardId} in pointed MMZ {i}");
                                return 1 << i;
                            }
                        }
                    }
                }

                if (cardId == CardId.CrusadiaMagius || cardId == CardId.CrusadiaRegulex || 
                    cardId == CardId.CrusadiaSpatha || cardId == CardId.CrusadiaEquimax || 
                    cardId == CardId.Avramax || cardId == CardId.SaryujaSkullDread)
                {
                    int emzAvailable = available & (Zones.z5 | Zones.z6);
                    if (emzAvailable > 0)
                    {
                        if ((emzAvailable & Zones.z5) > 0) return Zones.z5;
                        if ((emzAvailable & Zones.z6) > 0) return Zones.z6;
                    }
                }

                int mmzAvailable = available & Zones.MainMonsterZones;
                if (mmzAvailable > 0)
                {
                    int[] preferences = { 2, 1, 3, 0, 4 };
                    foreach (int z in preferences)
                    {
                        if ((mmzAvailable & (1 << z)) > 0)
                            return 1 << z;
                    }
                }
            }
            return base.OnSelectPlace(cardId, player, location, available);
        }

        // ================================================================================================
        // Helper Class: Opponent Threat Evaluator
        // ================================================================================================
        public class OpponentThreatEvaluator
        {
            public static bool HasNegatorOrFloodgate(ClientField enemy)
            {
                foreach (var card in enemy.MonsterZone)
                {
                    if (card != null && card.IsFaceup() && !card.IsDisabled())
                    {
                        if (card.IsMonsterShouldBeDisabledBeforeItUseEffect() || card.Attack > 3000)
                            return true;
                    }
                }
                foreach (var card in enemy.SpellZone)
                {
                    if (card != null && card.IsFaceup() && !card.IsDisabled())
                    {
                        if (card.HasType(CardType.Continuous) || card.HasType(CardType.Field))
                            return true;
                    }
                }
                return false;
            }

            public static int CountThreats(ClientField enemy)
            {
                int threatCount = 0;
                foreach (var card in enemy.MonsterZone)
                {
                    if (card != null && card.IsFaceup() && card.Attack >= 2500)
                        threatCount++;
                }
                return threatCount;
            }
        }

        // ================================================================================================
        // Helper Class: Detailed OTK Calculator
        // ================================================================================================
        public class OTKDamageCalculator
        {
            public static bool EvaluateLethal(ClientField bot, ClientField enemy)
            {
                int totalPower = 0;
                bool hasEquimax = bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
                bool hasMaximus = bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaMaximus);

                if (hasEquimax)
                {
                    var equimax = bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
                    if (equimax != null)
                    {
                        int equimaxAtk = equimax.Attack;
                        foreach (var m in bot.MonsterZone)
                        {
                            if (m != null && m.IsFaceup() && m != equimax)
                            {
                                equimaxAtk += m.Attack;
                            }
                        }
                        int baseDamage = equimaxAtk;
                        if (enemy.MonsterZone.Any(m => m != null && m.IsFaceup()))
                        {
                            int highestEnemyAtk = enemy.MonsterZone.GetMonsters().Max(m => m.Attack);
                            baseDamage = equimaxAtk - highestEnemyAtk;
                        }
                        if (baseDamage < 0) baseDamage = 0;

                        if (hasMaximus)
                        {
                            totalPower += baseDamage * 2;
                        }
                        else
                        {
                            totalPower += baseDamage;
                        }
                    }
                }
                else
                {
                    totalPower += bot.MonsterZone.GetMonsters().Where(m => m.IsAttack()).Sum(m => m.Attack);
                }

                return totalPower >= enemy.LifePoints;
            }
        }

        // ================================================================================================
        // TIER 1: Hand Trap handlers
        // ================================================================================================
        private bool AshActivate()
        {
            AI?.Log(LogLevel.Info, "[HANDTRAP] Ash Blossom activation check...");
            if (ChainAdvisor.ShouldHoldResponseWithProfile(Card, LastChainCard, OpponentProfile, 
                Enemy.Hand.Count, Enemy.GetMonsterCount(), Bot.Hand.Count(c => c.IsCode(CardId.AshBlossom, CardId.MaxxC)), false))
            {
                AI?.Log(LogLevel.Info, "[HANDTRAP] Holding Ash Blossom response based on Advisor.");
                return false;
            }
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool MaxxCActivate()
        {
            AI?.Log(LogLevel.Info, "[HANDTRAP] Maxx \"C\" activation check...");
            return DefaultMaxxC();
        }

        private bool EffectVeilerActivate()
        {
            AI?.Log(LogLevel.Info, "[HANDTRAP] Effect Veiler activation check...");
            return DefaultEffectVeiler();
        }

        private bool LanceaActivate()
        {
            AI?.Log(LogLevel.Info, "[HANDTRAP] Artifact Lancea activation check...");
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                DecisionTracer.TraceActivate("LanceaActivate", "Activating Lancea to block opponent's banishing plays.");
                return true;
            }
            return false;
        }

        // ================================================================================================
        // TIER 2: Board Breakers
        // ================================================================================================
        private bool TwinTwistersActivate()
        {
            AI?.Log(LogLevel.Info, "[SPELL] Twin Twisters activation check...");
            int enemyBackrow = Enemy.GetSpellCount();
            if (enemyBackrow == 0) return false;

            var discardTargets = new[] { CardId.CrusadiaDraco, CardId.CrusadiaReclusia, CardId.CrusadiaArboria };
            AI.SelectCard(discardTargets);

            var targets = Enemy.GetSpells().Where(c => c.IsFaceup() || c.IsFacedown()).ToList();
            if (targets.Count > 0)
            {
                AI.SelectNextCard(targets);
                return true;
            }
            return false;
        }

        private bool SlumberActivate()
        {
            AI?.Log(LogLevel.Info, "[SPELL] Interrupted Kaiju Slumber activation check...");
            if (_slumberUsed) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            if (Bot.Hand.Any(c => KaijuMonsters.Contains(c.Id)) && OpponentThreatEvaluator.HasNegatorOrFloodgate(Enemy))
            {
                AI?.Log(LogLevel.Info, "[SPELL] Holding Interrupted Kaiju Slumber to let Kaiju tribute opponent's negator first.");
                return false;
            }

            if (OpponentThreatEvaluator.HasNegatorOrFloodgate(Enemy))
            {
                DecisionTracer.TraceActivate("SlumberActivate", "Slumbering to clear opponent threat board");
                _slumberUsed = true;
                return true;
            }

            _slumberUsed = true;
            return true;
        }

        private bool RotaActivate()
        {
            if (ShouldSkipCombo()) return false;
            AI?.Log(LogLevel.Info, "[SPELL] ROTA activation check...");
            return true;
        }

        private bool CosmicCycloneActivate()
        {
            AI?.Log(LogLevel.Info, "[SPELL] Cosmic Cyclone activation check...");
            var problematicBackrow = Util.GetProblematicEnemySpell();
            if (problematicBackrow != null)
            {
                AI.SelectCard(problematicBackrow);
                return true;
            }
            
            var targets = Enemy.GetSpells().Where(c => c.IsFaceup() || c.IsFacedown()).ToList();
            if (targets.Count > 0)
            {
                AI.SelectCard(targets[0]);
                return true;
            }
            return false;
        }

        private bool EvenlyMatchedActivate()
        {
            AI?.Log(LogLevel.Info, "[TRAP] Evenly Matched activation check...");
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Battle)
            {
                int botCount = Bot.MonsterZone.Count(c => c != null) + Bot.SpellZone.Count(c => c != null);
                int enemyCount = Enemy.MonsterZone.Count(c => c != null) + Enemy.SpellZone.Count(c => c != null);
                if (enemyCount > botCount + 1)
                {
                    DecisionTracer.TraceActivate("EvenlyMatched", "Playing Evenly Matched at end of battle phase.");
                    return true;
                }
            }
            return false;
        }

        private bool SuccessionActivate()
        {
            if (ShouldSkipCombo()) return false;
            AI?.Log(LogLevel.Info, "[SPELL] World Legacy Succession activation check...");
            int pointedZones = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (IsZonePointedToByAnyLink(i))
                    pointedZones |= (1 << i);
            }
            int targetZone = GetEmptyZones() & pointedZones;
            if (targetZone > 0)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.CrusadiaEquimax);
                if (target == null)
                    target = Bot.Graveyard.FirstOrDefault(c => CrusadiaMainMonsters.Contains(c.Id));

                if (target != null)
                {
                    AI.SelectCard(target);
                    AI.SelectPlace(targetZone);
                    return true;
                }
            }
            return false;
        }

        // ================================================================================================
        // TIER 3: Kaiju Summon Logic
        // ================================================================================================
        private bool KaijuSummon()
        {
            AI?.Log(LogLevel.Info, "[KAIJU] Evaluating Kaiju summon...");
            if (!KaijuMonsters.Contains(Card.Id)) return false;

            if (Enemy.HasInMonstersZone(KaijuMonsters))
            {
                bool hasEquimax = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
                if (hasEquimax || Bot.GetMonsterCount() == 0)
                {
                    return Bot.GetMonsterCount() < 5;
                }
                return false;
            }

            ClientCard target = null;
            if (OpponentThreatEvaluator.HasNegatorOrFloodgate(Enemy))
            {
                target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.IsMonsterShouldBeDisabledBeforeItUseEffect());
            }

            if (target == null)
            {
                target = Util.GetProblematicEnemyCard();
            }

            if (target == null)
            {
                target = Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault(c => c.IsFaceup());
            }

            if (target != null)
            {
                _kaijuTributeTarget = target;
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("KaijuSummon", $"Tributing opponent's {target.Name} (ID: {target.Id})");
                return true;
            }
            return false;
        }

        // ================================================================================================
        // TIER 4: Link Climbs (Magius, Regulex, Spatha)
        // ================================================================================================
        private bool MagiusSummon()
        {
            AI?.Log(LogLevel.Info, "[LINK-SUMMON] Magius summon check...");
            if (Bot.HasInMonstersZone(CardId.CrusadiaMagius)) return false;

            var crusadias = Bot.GetMonsters().Where(c => CrusadiaMainMonsters.Contains(c.Id)).ToList();
            if (crusadias.Count == 0) return false;

            bool hasExtender = Bot.Hand.Any(c => CrusadiaMainMonsters.Contains(c.Id)) || Bot.Graveyard.Any(c => c.Id == CardId.MonsterReborn);
            return hasExtender;
        }

        private bool RegulexSummon()
        {
            AI?.Log(LogLevel.Info, "[LINK-SUMMON] Regulex summon check...");
            if (Bot.HasInMonstersZone(CardId.CrusadiaRegulex)) return false;
            
            bool hasMagius = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaMagius);
            return hasMagius;
        }

        private bool SpathaSummon()
        {
            AI?.Log(LogLevel.Info, "[LINK-SUMMON] Spatha summon check...");
            if (Bot.HasInMonstersZone(CardId.CrusadiaSpatha)) return false;
            
            bool hasMagius = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaMagius);
            return hasMagius;
        }

        // ================================================================================================
        // TIER 5: Generic Utility Extra Deck Summons
        // ================================================================================================
        private bool KnightmarePhoenixSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            AI?.Log(LogLevel.Info, "[LINK-SUMMON] Knightmare Phoenix summon check...");
            return Enemy.GetSpellCount() > 0 && Bot.GetMonsterCount() >= 2;
        }

        private bool KnightmareCerberusSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            AI?.Log(LogLevel.Info, "[LINK-SUMMON] Knightmare Cerberus summon check...");
            return Enemy.GetMonsters().Any(c => c.IsFaceup() && c.IsAttack() && c.IsSpecialSummoned) && Bot.GetMonsterCount() >= 2;
        }

        private bool KnightmareUnicornSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            AI?.Log(LogLevel.Info, "[LINK-SUMMON] Knightmare Unicorn summon check...");
            return Util.GetProblematicEnemyCard() != null && Bot.GetMonsterCount() >= 3;
        }

        private bool TrisbaenaSummon()
        {
            AI?.Log(LogLevel.Info, "[LINK-SUMMON] Topologic Trisbaena summon check...");
            return Enemy.GetSpellCount() >= 2 && Bot.GetMonsterCount() >= 3;
        }

        private bool SaryujaSummon()
        {
            AI?.Log(LogLevel.Info, "[LINK-SUMMON] Saryuja Skull Dread summon check...");
            return Bot.GetMonsterCount() >= 4;
        }

        // ================================================================================================
        // TIER 6: Boss Monsters (Equimax, Avramax, Borrelsword)
        // ================================================================================================
        private bool EquimaxSummon()
        {
            AI?.Log(LogLevel.Info, "[LINK-SUMMON] Equimax summon check...");
            if (Bot.HasInMonstersZone(CardId.CrusadiaEquimax)) return false;
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && (c.Id == CardId.CrusadiaRegulex || c.Id == CardId.CrusadiaSpatha));
        }

        private bool AvramaxSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            AI?.Log(LogLevel.Info, "[LINK-SUMMON] Mekk-Knight Crusadia Avramax summon check...");
            if (Bot.HasInMonstersZone(CardId.Avramax)) return false;

            bool hasEquimax = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
            return hasEquimax || Enemy.GetMonsters().Any(c => c.Attack > 3000);
        }

        private bool BorrelswordSummon()
        {
            AI?.Log(LogLevel.Info, "[LINK-SUMMON] Borrelsword Dragon summon check...");
            return Bot.GetMonsterCount() >= 3 && Enemy.GetMonsters().Any(m => m.IsAttack() && m.IsFaceup());
        }

        // ================================================================================================
        // TIER 7: Crusadia Special Summons & Main Deck Effects
        // ================================================================================================
        private bool CrusadiaHandSpSummon()
        {
            if (ShouldSkipCombo()) return false;
            AI?.Log(LogLevel.Info, $"[EXTENDER-SUMMON] Crusadia {Card.Name}: Evaluating hand SpSummon...");
            if (Card.Location == CardLocation.Hand)
            {
                int pointedZones = 0;
                for (int i = 0; i < 5; ++i)
                {
                    if (IsZonePointedToByAnyLink(i))
                        pointedZones |= (1 << i);
                }
                int targetZone = GetEmptyZones() & pointedZones;
                if (targetZone > 0)
                {
                    AI?.Log(LogLevel.Info, $"[EXTENDER-SUMMON] Special Summoning {Card.Name} to pointed zone.");
                    return true;
                }
            }
            return false;
        }

        private bool PurpleActivate()
        {
            AI?.Log(LogLevel.Info, "[MONSTER-EFFECT] Mekk-Knight Purple Nightfall: Checking activation...");
            var target = Bot.GetMonsters().FirstOrDefault(c => MekkKnightMonsters.Contains(c.Id) && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                AI.SelectNextCard(new[] { CardId.MekkKnightBlue, CardId.MekkKnightIndigo });
                return true;
            }
            return false;
        }

        private bool BlueActivate()
        {
            AI?.Log(LogLevel.Info, "[MONSTER-EFFECT] Mekk-Knight Blue Sky: Checking activation...");
            AI.SelectCard(new[] { CardId.MekkKnightPurple, CardId.MekkKnightIndigo });
            return true;
        }

        private bool IndigoActivate()
        {
            AI?.Log(LogLevel.Info, "[MONSTER-EFFECT] Mekk-Knight Indigo Eclipse: Checking activation...");
            var target = Bot.GetMonsters().FirstOrDefault(c => c.Id == CardId.MekkKnightIndigo && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                int emptyZones = GetEmptyZones();
                if (emptyZones > 0)
                {
                    if ((emptyZones & (1 << 0)) > 0) AI.SelectPlace(1 << 0);
                    else if ((emptyZones & (1 << 4)) > 0) AI.SelectPlace(1 << 4);
                    else AI.SelectPlace(emptyZones);
                    return true;
                }
            }
            return false;
        }

        private bool ReclusiaActivate()
        {
            AI?.Log(LogLevel.Info, "[MONSTER-EFFECT] Crusadia Reclusia: Checking activation...");
            if (Card.Location == CardLocation.MonsterZone)
            {
                var enemyTarget = Util.GetProblematicEnemyCard();
                if (enemyTarget != null)
                {
                    AI?.Log(LogLevel.Info, $"[MONSTER-EFFECT] Crusadia Reclusia: Targeting enemy card {enemyTarget.Name} and ourselves for destruction.");
                    AI.SelectCard(Card);
                    AI.SelectNextCard(enemyTarget);
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool ArboriaActivate()
        {
            return false;
        }

        private bool LeonisActivate()
        {
            AI?.Log(LogLevel.Info, "[MONSTER-EFFECT] Crusadia Leonis: Checking activation...");
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_leonisUsed) return false;

                bool goingForGame = OTKDamageCalculator.EvaluateLethal(Bot, Enemy);
                var equimax = Bot.GetMonsters().FirstOrDefault(c => c.Id == CardId.CrusadiaEquimax);
                if (equimax != null && goingForGame)
                {
                    AI?.Log(LogLevel.Info, "[MONSTER-EFFECT] Crusadia Leonis: Granting piercing effect to Equimax.");
                    AI.SelectCard(equimax);
                    _leonisUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool DracoActivate()
        {
            AI?.Log(LogLevel.Info, "[MONSTER-EFFECT] Crusadia Draco: Checking activation...");
            if (Card.Location == CardLocation.MonsterZone)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => CrusadiaMainMonsters.Contains(c.Id) && c.Id != CardId.CrusadiaDraco);
                if (target != null)
                {
                    AI?.Log(LogLevel.Info, $"[MONSTER-EFFECT] Crusadia Draco: Retrieving {target.Name} from Graveyard.");
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool MaximusActivate()
        {
            AI?.Log(LogLevel.Info, "[MONSTER-EFFECT] Crusadia Maximus: Checking activation...");
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_maximusUsed) return false;

                bool goingForGame = OTKDamageCalculator.EvaluateLethal(Bot, Enemy);
                var equimax = Bot.GetMonsters().FirstOrDefault(c => c.Id == CardId.CrusadiaEquimax);
                if (equimax != null && goingForGame)
                {
                    AI?.Log(LogLevel.Info, "[MONSTER-EFFECT] Crusadia Maximus: Granting double battle damage to Equimax.");
                    AI.SelectCard(equimax);
                    _maximusUsed = true;
                    return true;
                }
            }
            return false;
        }

        // ================================================================================================
        // TIER 8: Spells
        // ================================================================================================
        private bool MonsterRebornActivate()
        {
            AI?.Log(LogLevel.Info, "[SPELL] Monster Reborn activation check...");
            var target = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.CrusadiaEquimax);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }

            var material = Bot.Graveyard.FirstOrDefault(c => CrusadiaMainMonsters.Contains(c.Id));
            if (material != null)
            {
                AI.SelectCard(material);
                return true;
            }
            return false;
        }

        private bool PowerActivate()
        {
            AI?.Log(LogLevel.Info, "[SPELL] Crusadia Power activation check...");
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                var target = Bot.GetMonsters().FirstOrDefault(c => AceCardIds.Contains(c.Id));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool RevivalActivate()
        {
            AI?.Log(LogLevel.Info, "[SPELL] Crusadia Revival activation check...");
            if (Card.Location == CardLocation.Hand) return true;

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_revivalUsed) return false;

                bool goingForGame = OTKDamageCalculator.EvaluateLethal(Bot, Enemy);
                var equimax = Bot.GetMonsters().FirstOrDefault(c => c.Id == CardId.CrusadiaEquimax);
                if (equimax != null && goingForGame)
                {
                    AI.SelectCard(equimax);
                    _revivalUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool TestamentActivate()
        {
            AI?.Log(LogLevel.Info, "[SPELL] Crusadia Testament activation check...");
            if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
            {
                var equimax = Bot.GetMonsters().FirstOrDefault(c => c.Id == CardId.CrusadiaEquimax);
                if (equimax != null)
                {
                    AI.SelectCard(equimax);
                    return true;
                }
            }
            return false;
        }

        // ================================================================================================
        // TIER 9: Normal Summon
        // ================================================================================================
        private bool CrusadiaNormalSummon()
        {
            AI?.Log(LogLevel.Info, "[SUMMON] Normal Summon Crusadia check...");
            if (!CrusadiaMainMonsters.Contains(Card.Id)) return false;

            if (Bot.Hand.Any(c => c.IsCode(CardId.InterruptedKaijuSlumber)) && !_slumberUsed && Enemy.GetMonsterCount() > 0)
            {
                return false;
            }

            _normalSummonedCrusadia = true;
            return true;
        }

        // ================================================================================================
        // Card Selection Logic (OnSelectCard) Detailed Rankings
        // ================================================================================================
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            AI?.Log(LogLevel.Info, $"[SELECT-CARD] min={min}, max={max}, hint={hint}");

            if (_kaijuTributeTarget != null && cards.Contains(_kaijuTributeTarget))
            {
                var target = _kaijuTributeTarget;
                _kaijuTributeTarget = null;
                AI?.Log(LogLevel.Info, $"[SELECT-CARD] Routing selection to saved Kaiju tribute target: {target.Name} (ID: {target.Id})");
                return new List<ClientCard> { target };
            }
            
            if (hint == 509) // Special Summoning from Deck (Slumber)
            {
                bool isSlumber = cards.Any(c => c != null && KaijuMonsters.Contains(c.Id) && c.Location == CardLocation.Deck);
                if (isSlumber)
                {
                    var result = new List<ClientCard>();
                    var weakest = cards.Where(c => c != null && KaijuMonsters.Contains(c.Id)).OrderBy(c => c.Attack).FirstOrDefault();
                    var strongest = cards.Where(c => c != null && KaijuMonsters.Contains(c.Id) && c != weakest).OrderByDescending(c => c.Attack).FirstOrDefault();

                    if (weakest != null) result.Add(weakest);
                    if (strongest != null) result.Add(strongest);
                    return result;
                }
            }

            if (hint == 506) // Searching Crusadia cards
            {
                if (cards.Any(c => c != null && CrusadiaMainMonsters.Contains(c.Id)))
                {
                    var preferred = new[] { CardId.CrusadiaMaximus, CardId.CrusadiaDraco, CardId.CrusadiaArboria, CardId.CrusadiaLeonis, CardId.CrusadiaReclusia };
                    foreach (int id in preferred)
                    {
                        var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                        if (match != null) return new List<ClientCard> { match };
                    }
                }
            }

            if (hint == 533 || hint == 513) // Selection for Link materials
            {
                var sorted = cards.Where(c => c != null).OrderBy(c => c.Level).ToList();
                return sorted.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.CrusadiaEquimax || cardId == CardId.Avramax || cardId == CardId.BorrelswordDragon)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (attacker != null && attacker.Id == CardId.CrusadiaEquimax)
            {
                var bestTarget = defenders.Where(d => d != null && d.IsFaceup())
                                          .OrderByDescending(d => d.Attack)
                                          .FirstOrDefault();
                if (bestTarget != null)
                {
                    return AI.Attack(attacker, bestTarget);
                }
            }
            return base.OnSelectAttackTarget(attacker, defenders);
        }

        private bool SpellSetLogic()
        {
            if (Card.IsCode(CardId.CrusadiaPower, CardId.CalledByTheGrave, CardId.CosmicCyclone)) return true;
            return false;
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;
            if (AceCardIds.Contains(Card.Id)) return false;
            return base.DefaultMonsterRepos();
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id) || base.IsAceCard(card);
        }

        protected override bool IsBoardStrongEnough()
        {
            // Equimax on field = primary OTK enabler
            if (Bot.HasInMonstersZone(CardId.CrusadiaEquimax))
                return true;
            // Avramax = defensive boss
            if (Bot.HasInMonstersZone(CardId.Avramax))
                return true;
            // Borrelsword = OTK backup
            if (Bot.HasInMonstersZone(CardId.BorrelswordDragon))
                return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            // Stop once we have Equimax or Avramax
            if (Bot.HasInMonstersZone(CardId.CrusadiaEquimax)
                || Bot.HasInMonstersZone(CardId.Avramax)
                || Bot.HasInMonstersZone(CardId.BorrelswordDragon))
                return base.ShouldStopExtending();
            return false;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (AceCardIds.Contains(c.Id)) return 900;
            if (CrusadiaMainMonsters.Contains(c.Id)) return 50;
            if (KaijuMonsters.Contains(c.Id)) return 30;
            return 100;
        }

        private void LogCombatState()
        {
            AI?.Log(LogLevel.Info, $"[STRATEGY] Normal Summoned Crusadia: {_normalSummonedCrusadia}");
            int botCount = Bot.MonsterZone.Count(c => c != null);
            int enemyCount = Enemy.MonsterZone.Count(c => c != null);
            AI?.Log(LogLevel.Info, $"[STRATEGY] Bot Monsters: {botCount}, Enemy Monsters: {enemyCount}");
        }
    }
}
