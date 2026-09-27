// ====================================================================================================
// CARD AUDIT — 2026 Kaiju Crusadia Comprehensive AI Executor
// ====================================================================================================
// Strategy:
// Go 2nd: Break opponent board with Kaiju / Slumber / Twin Twisters.
// Link climb: Normal Summon -> Magius (search Draco) -> Regulex (search Revival/Power, Draco retrieves Maximus)
//             -> Equimax -> Special Summon Maximus & Extenders to Equimax pointers.
// OTK: Equimax points to Kaiju on enemy field + Maximus doubles battle damage + Leonis grants piercing
//      + Revival allows attacking all monsters. Result: 8000+ to 15000+ damage OTK.
// Go 1st: End on Equimax (quick-effect negate) + Avramax + Crusadia Power (unaffected) protection.
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
        public class CardId
        {
            // Crusadia Main Monsters
            public const int CrusadiaMaximus = 81524756;
            public const int CrusadiaDraco = 54525057;
            public const int CrusadiaArboria = 91646304;
            public const int CrusadiaReclusia = 55241609;
            public const int CrusadiaLeonis = 28031913;

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

            // Spells & Traps
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

            // Handtraps & Disruptions
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

        private static readonly int[] AceCardIds = {
            CardId.CrusadiaEquimax, CardId.Avramax, CardId.BorrelswordDragon, CardId.SaryujaSkullDread
        };

        private static readonly int[] CrusadiaMainMonsters = {
            CardId.CrusadiaMaximus, CardId.CrusadiaDraco, CardId.CrusadiaArboria,
            CardId.CrusadiaReclusia, CardId.CrusadiaLeonis
        };

        private static readonly int[] KaijuMonsters = {
            CardId.Gameciel, CardId.Kumongous, CardId.Gadarla, CardId.Radian
        };

        private static readonly int[] MekkKnightMonsters = {
            CardId.MekkKnightPurple, CardId.MekkKnightBlue, CardId.MekkKnightIndigo
        };

        // Turn Flags
        private bool _normalSummonedCrusadia = false;
        private bool _slumberUsed = false;
        private bool _maximusBuffUsed = false;
        private bool _leonisBuffUsed = false;
        private bool _revivalUsed = false;
        private ClientCard _kaijuTributeTarget = null;

        public _2026_KaijuCrusadiaExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            ResourcePlan.RegisterAceCards(AceCardIds);

            // ── Combo Router Routes ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Going2nd-Equimax-Maximus-OTK",
                RequiredCards = new List<int> { CardId.CrusadiaMaximus },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.CrusadiaMagius, ActionType = ExecutorType.SpSummon, Description = "Link 1 Magius" },
                    new() { CardId = CardId.CrusadiaRegulex, ActionType = ExecutorType.SpSummon, Description = "Link 2 Regulex" },
                    new() { CardId = CardId.CrusadiaEquimax, ActionType = ExecutorType.SpSummon, Description = "Link 3 Equimax" },
                    new() { CardId = CardId.CrusadiaMaximus, ActionType = ExecutorType.Activate, Description = "Maximus double damage" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Going1st-Equimax-Avramax",
                RequiredCards = new List<int> { CardId.CrusadiaArboria },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.CrusadiaMagius, ActionType = ExecutorType.SpSummon, Description = "Link 1 Magius" },
                    new() { CardId = CardId.CrusadiaRegulex, ActionType = ExecutorType.SpSummon, Description = "Link 2 Regulex" },
                    new() { CardId = CardId.CrusadiaEquimax, ActionType = ExecutorType.SpSummon, Description = "Link 3 Equimax" }
                },
                EndBoardScore = 80
            });

            BaitPlanner.RegisterComboStarters(CardId.CrusadiaMaximus, CardId.CrusadiaDraco, CardId.CrusadiaArboria);
            ChainAdvisor.RegisterHighValueTargets(CardId.CrusadiaEquimax, CardId.Avramax, CardId.CrusadiaRegulex, CardId.CrusadiaMagius);

            // ============================================================================================
            // Register Executors in Strategic Priority Order
            // ============================================================================================

            // TIER 1: Handtraps & Direct Response Counters
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, DefaultMaxxC);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, DefaultEffectVeiler);
            AddExecutor(ExecutorType.Activate, CardId.ArtifactLancea, LanceaActivate);

            // TIER 2: Quick Disruption & Negation (Equimax Quick Effect)
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaEquimax, EquimaxNegateActivate);

            // TIER 3: Protection (Crusadia Power & Arboria Grave Shield)
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaPower, PowerActivate);

            // TIER 4: Board Breakers (Twin Twisters, Cosmic Cyclone, Slumber, Evenly Matched)
            AddExecutor(ExecutorType.Activate, CardId.TwinTwisters, TwinTwistersActivate);
            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, CosmicCycloneActivate);
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedActivate);
            AddExecutor(ExecutorType.Activate, CardId.InterruptedKaijuSlumber, SlumberActivate);

            // TIER 5: Kaiju Tributes (Tribute opponent's threat before committing combo)
            AddExecutor(ExecutorType.SpSummon, KaijuSummon);

            // TIER 6: Search & Link Effects (Magius, Regulex, Draco, Reclusia)
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaMagius, MagiusActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaRegulex, RegulexActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaDraco, DracoActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaReclusia, ReclusiaActivate);

            // TIER 7: OTK Buff Effects (Maximus, Leonis, Revival)
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaMaximus, MaximusActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaLeonis, LeonisActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaRevival, RevivalActivate);
            AddExecutor(ExecutorType.Activate, CardId.CrusadiaTestament, TestamentActivate);

            // TIER 8: Extender Spells (ROTA, Succession, Reborn)
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotaActivate);
            AddExecutor(ExecutorType.Activate, CardId.WorldLegacySuccession, SuccessionActivate);
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, MonsterRebornActivate);

            // TIER 9: Extender Special Summons from Hand (into pointed zones)
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaDraco, CrusadiaHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaArboria, CrusadiaHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaLeonis, CrusadiaHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaReclusia, CrusadiaHandSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.WorldCrown, WorldCrownSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaMaximus, MaximusHandSpSummon);

            // TIER 10: Link Climbs: Equimax (Link 3) -> Regulex (Link 2) -> Spatha -> Magius (Link 1)
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaEquimax, EquimaxSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaRegulex, RegulexSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaSpatha, SpathaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.CrusadiaMagius, MagiusSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Avramax, AvramaxSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.BorrelswordDragon, BorrelswordSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmarePhoenix, KnightmarePhoenixSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareCerberus, KnightmareCerberusSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.KnightmareUnicorn, KnightmareUnicornSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TopologicTrisbaena, TrisbaenaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SaryujaSkullDread, SaryujaSummon);

            // TIER 11: Mekk-Knight Summons & Effects
            AddExecutor(ExecutorType.SpSummon, CardId.MekkKnightPurple, MekkKnightSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MekkKnightBlue, MekkKnightSpSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MekkKnightIndigo, MekkKnightSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.MekkKnightPurple, PurpleActivate);
            AddExecutor(ExecutorType.Activate, CardId.MekkKnightBlue, BlueActivate);
            AddExecutor(ExecutorType.Activate, CardId.MekkKnightIndigo, IndigoActivate);

            // TIER 12: Starter Normal Summon
            AddExecutor(ExecutorType.Summon, CrusadiaNormalSummon);

            // TIER 13: Repos & Spell/Trap Setting
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
            AddExecutor(ExecutorType.SpellSet, SpellSetLogic);
        }

        public override bool OnSelectHand() => false; // Choose Second for OTK

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _normalSummonedCrusadia = false;
            _slumberUsed = false;
            _maximusBuffUsed = false;
            _leonisBuffUsed = false;
            _revivalUsed = false;
            _kaijuTributeTarget = null;
        }

        protected override bool ShouldStopExtending() => false; // Pure OTK deck must commit to full combo

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        protected override bool IsBoardStrongEnough()
        {
            bool hasEquimax = Bot.HasInMonstersZone(CardId.CrusadiaEquimax);
            bool hasAvramax = Bot.HasInMonstersZone(CardId.Avramax);
            if (hasEquimax && hasAvramax) return true;
            if (hasEquimax && Bot.HasInMonstersZone(CardId.CrusadiaArboria)) return true;
            return base.IsBoardStrongEnough();
        }


        // ================================================================================================
        // TIER 1 & 2: Handtraps, Negates & Protection
        // ================================================================================================
        private bool AshActivate()
        {
            if (ChainAdvisor.ShouldHoldResponseWithProfile(Card, LastChainCard, OpponentProfile,
                Enemy.Hand.Count, Enemy.GetMonsterCount(), Bot.Hand.Count(c => c.IsCode(CardId.AshBlossom, CardId.MaxxC)), false))
            {
                return false;
            }
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool LanceaActivate()
        {
            return Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2);
        }

        private bool EquimaxNegateActivate()
        {
            // Equimax quick effect: tribute 1 Crusadia/World Legacy monster it points to, negate 1 face-up card on field
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Find a monster Equimax points to to tribute (prefer tributing extenders, NOT Equimax itself)
            var tributeCandidates = Bot.GetMonsters().Where(m => m != null && m != Card && IsEquimaxPointingTo(Card, m)).ToList();
            if (tributeCandidates.Count == 0) return false;

            // Find high-priority enemy target to negate
            ClientCard target = Util.GetProblematicEnemyCard();
            if (target == null && LastChainCard != null && LastChainCard.Controller == 1)
            {
                target = LastChainCard;
            }
            if (target == null)
            {
                target = Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup() && !m.IsDisabled() && (CardIntelligence.IsHighThreatChokepoint(m.Id) || m.IsMonsterShouldBeDisabledBeforeItUseEffect()));
            }
            if (target == null)
            {
                target = Enemy.GetSpells().FirstOrDefault(s => s.IsFaceup() && !s.IsDisabled() && CardIntelligence.IsFloodgate(s.Id));
            }

            if (target != null && target.IsFaceup() && !target.IsDisabled())
            {
                // Select tribute candidate first
                var tribute = tributeCandidates.OrderBy(c => GetMaterialPriority(c)).First();
                AI.SelectCard(tribute);
                AI.SelectNextCard(target);
                return true;
            }
            return false;
        }

        private bool PowerActivate()
        {
            // Crusadia Power: Target 1 Crusadia monster, unaffected by other card effects this turn
            var target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
            if (target == null)
                target = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == CardId.CrusadiaRegulex || c.Id == CardId.CrusadiaMagius));

            if (target != null)
            {
                if (Duel.LastChainPlayer == 1 || Duel.Phase == DuelPhase.Battle)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        // ================================================================================================
        // TIER 4: Board Breakers
        // ================================================================================================
        private bool TwinTwistersActivate()
        {
            int enemyBackrow = Enemy.GetSpellCount();
            if (enemyBackrow == 0) return false;

            // Discard priority: Draco / Reclusia / extra Kaiju
            var discardTarget = Bot.Hand.FirstOrDefault(c => c != null && c != Card &&
                (c.Id == CardId.CrusadiaDraco || c.Id == CardId.CrusadiaReclusia || c.Id == CardId.CrusadiaArboria));
            if (discardTarget == null)
                discardTarget = Bot.Hand.FirstOrDefault(c => c != null && c != Card && KaijuMonsters.Contains(c.Id));
            if (discardTarget == null)
                discardTarget = Bot.Hand.FirstOrDefault(c => c != null && c != Card && !AceCardIds.Contains(c.Id));

            if (discardTarget == null) return false;

            var targets = Enemy.GetSpells().Where(c => c.IsFaceup() || c.IsFacedown()).ToList();
            if (targets.Count > 0)
            {
                AI.SelectCard(discardTarget);
                AI.SelectNextCard(targets);
                return true;
            }
            return false;
        }

        private bool CosmicCycloneActivate()
        {
            var problematic = Util.GetProblematicEnemySpell();
            if (problematic != null)
            {
                AI.SelectCard(problematic);
                return true;
            }
            var target = Enemy.GetSpells().FirstOrDefault(c => c.IsFaceup() || c.IsFacedown());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool EvenlyMatchedActivate()
        {
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Battle)
            {
                int botCards = Bot.GetMonsterCount() + Bot.GetSpellCount();
                int enemyCards = Enemy.GetMonsterCount() + Enemy.GetSpellCount();
                if (enemyCards > botCards + 1)
                {
                    return true;
                }
            }
            return false;
        }

        private bool SlumberActivate()
        {
            // CRITICAL SELF-HARM PREVENTION:
            // Only activate Slumber if Bot controls ZERO monsters, and Enemy has monsters!
            if (_slumberUsed) return false;
            if (Bot.GetMonsterCount() > 0) return false;
            if (Enemy.GetMonsterCount() == 0) return false;

            _slumberUsed = true;
            return true;
        }

        // ================================================================================================
        // TIER 5: Kaiju Summon Logic
        // ================================================================================================
        private bool KaijuSummon()
        {
            if (!KaijuMonsters.Contains(Card.Id)) return false;

            // If opponent already has a Kaiju, can only summon to our field if Equimax exists or field is empty
            if (Enemy.HasInMonstersZone(KaijuMonsters))
            {
                return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax) && Bot.GetMonsterCount() < 5;
            }

            // CRITICAL SAFEGUARD: Do not summon a Kaiju to opponent if Bot cannot make any plays this turn!
            // Bot must have at least 1 monster on field or 1 Crusadia/Mekk-Knight extender in hand to build towards Equimax/Avramax
            bool canCombo = Bot.GetMonsterCount() > 0 ||
                            Bot.Hand.Any(c => c != null && (CrusadiaMainMonsters.Contains(c.Id) || MekkKnightMonsters.Contains(c.Id) || c.Id == CardId.ReinforcementOfTheArmy));
            if (!canCombo) return false;

            // If Bot holds Gameciel (lowest ATK Kaiju: 2200), prefer summoning Gameciel to opponent rather than higher ATK Kaijus
            if (Card.Id != CardId.Gameciel && Bot.HasInHand(CardId.Gameciel))
            {
                return false; // Wait for Gameciel to be evaluated
            }

            // Target selection for tributing opponent's monster:
            // 1. Chokepoints / Negators / Floodgates
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() &&
                (CardIntelligence.IsHighThreatChokepoint(m.Id) || m.IsMonsterShouldBeDisabledBeforeItUseEffect()));

            // 2. Problematic monster
            if (target == null) target = Util.GetProblematicEnemyMonster();

            // 3. Enemy monster with ATK >= 2000 (only Kaiju real threats!)
            if (target == null)
            {
                target = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && m.Attack >= 2000).OrderByDescending(m => m.Attack).FirstOrDefault();
            }

            if (target != null)
            {
                _kaijuTributeTarget = target;
                AI.SelectCard(target);
                return true;
            }

            return false;
        }

        // ================================================================================================
        // TIER 6 & 7: Search, Triggers & OTK Buffs
        // ================================================================================================
        private bool MagiusActivate()
        {
            // Magius searches 1 Crusadia monster
            return true;
        }

        private bool RegulexActivate()
        {
            // Regulex searches 1 Crusadia Spell/Trap
            return true;
        }

        private bool DracoActivate()
        {
            // Draco retrieves 1 Crusadia monster from GY
            var target = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.CrusadiaMaximus);
            if (target == null) target = Bot.Graveyard.FirstOrDefault(c => CrusadiaMainMonsters.Contains(c.Id) && c.Id != CardId.CrusadiaDraco);

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return true;
        }

        private bool ReclusiaActivate()
        {
            // Pop 1 Crusadia card + 1 opponent card
            if (Card.Location == CardLocation.MonsterZone)
            {
                var enemyTarget = Util.GetProblematicEnemyCard() ?? Enemy.GetMonsters().FirstOrDefault(m => m.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault();
                if (enemyTarget != null)
                {
                    AI.SelectCard(Card);
                    AI.SelectNextCard(enemyTarget);
                    return true;
                }
            }
            return false;
        }

        private bool MaximusActivate()
        {
            // Double battle damage for Equimax!
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_maximusBuffUsed) return false;
                var equimax = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
                if (equimax != null)
                {
                    AI.SelectCard(equimax);
                    _maximusBuffUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool LeonisActivate()
        {
            // Piercing damage for Equimax!
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_leonisBuffUsed) return false;
                var equimax = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
                if (equimax != null)
                {
                    AI.SelectCard(equimax);
                    _leonisBuffUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool RevivalActivate()
        {
            // Field spell: activate from hand, or activate on field to grant multi-attack to Equimax!
            if (Card.Location == CardLocation.Hand) return true;

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                if (_revivalUsed) return false;
                var equimax = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
                if (equimax != null && Enemy.GetMonsterCount() > 0)
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
            var equimax = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
            if (equimax != null)
            {
                AI.SelectCard(equimax);
                return true;
            }
            return false;
        }

        // ================================================================================================
        // TIER 8 & 9: Extenders & Spells
        // ================================================================================================
        private bool RotaActivate()
        {
            return true; // Searches Arboria
        }

        private bool SuccessionActivate()
        {
            if (!HasEmptyPointedZone()) return false;
            // Reborn a monster to a zone a link monster points to
            var link = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.HasType(CardType.Link));
            if (link == null) return false;

            var target = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.CrusadiaEquimax)
                      ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.CrusadiaMaximus)
                      ?? Bot.Graveyard.FirstOrDefault(c => CrusadiaMainMonsters.Contains(c.Id));

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool MonsterRebornActivate()
        {
            var target = Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.CrusadiaEquimax)
                      ?? Bot.Graveyard.FirstOrDefault(c => c.Id == CardId.Avramax)
                      ?? Bot.Graveyard.FirstOrDefault(c => CrusadiaMainMonsters.Contains(c.Id))
                      ?? Enemy.Graveyard.FirstOrDefault(c => c.Attack >= 2500);

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CrusadiaHandSpSummon()
        {
            // Only summon if we have a Link monster pointing to an available zone
            return HasEmptyPointedZone();
        }

        private bool MaximusHandSpSummon()
        {
            if (!HasEmptyPointedZone()) return false;
            // If Equimax is on field, ALWAYS summon Maximus under Equimax for OTK!
            if (Bot.HasInMonstersZone(CardId.CrusadiaEquimax)) return true;

            // If we have other extenders in hand that can be summoned instead under Magius/Regulex, hold Maximus!
            bool hasOtherExtender = Bot.Hand.Any(c => c != null && (c.Id == CardId.CrusadiaArboria || c.Id == CardId.CrusadiaDraco || c.Id == CardId.CrusadiaLeonis || c.Id == CardId.CrusadiaReclusia || c.Id == CardId.WorldCrown));
            if (hasOtherExtender) return false;

            // If no other extenders, summon Maximus to keep the climb going
            return true;
        }

        private bool WorldCrownSpSummon()
        {
            return HasEmptyPointedZone();
        }

        // ================================================================================================
        // TIER 10: Link Climbs (Magius -> Regulex -> Equimax -> Avramax)
        // ================================================================================================
        private bool MagiusSummon()
        {
            if (Bot.HasInMonstersZone(CardId.CrusadiaMagius)) return false;
            // CRITICAL: NEVER downgrade if we already have a Link-2 or Link-3 Crusadia boss on field!
            if (Bot.HasInMonstersZone(CardId.CrusadiaRegulex) ||
                Bot.HasInMonstersZone(CardId.CrusadiaSpatha) ||
                Bot.HasInMonstersZone(CardId.CrusadiaEquimax) ||
                Bot.HasInMonstersZone(CardId.Avramax)) return false;

            // Need 1 non-Magius Crusadia on field
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && CrusadiaMainMonsters.Contains(c.Id) && c.Id != CardId.CrusadiaMagius);
        }

        private bool RegulexSummon()
        {
            if (Bot.HasInMonstersZone(CardId.CrusadiaRegulex)) return false;
            // CRITICAL: NEVER downgrade if we already have Equimax or Avramax on field!
            if (Bot.HasInMonstersZone(CardId.CrusadiaEquimax)) return false;
            if (Bot.HasInMonstersZone(CardId.Avramax)) return false;

            // Requires Magius + 1 effect monster
            return Bot.HasInMonstersZone(CardId.CrusadiaMagius) && Bot.GetMonsterCount() >= 2;
        }

        private bool SpathaSummon()
        {
            if (Bot.HasInMonstersZone(CardId.CrusadiaSpatha)) return false;
            if (Bot.HasInMonstersZone(CardId.CrusadiaEquimax)) return false;
            if (Bot.HasInMonstersZone(CardId.Avramax)) return false;

            // Backup if Regulex is unavailable
            return !Bot.ExtraDeck.Any(c => c.Id == CardId.CrusadiaRegulex) && Bot.HasInMonstersZone(CardId.CrusadiaMagius) && Bot.GetMonsterCount() >= 2;
        }

        private bool EquimaxSummon()
        {
            if (Bot.HasInMonstersZone(CardId.CrusadiaEquimax)) return false;
            // Option 1: Regulex/Spatha (Link 2) + 1 monster (total >= 2 monsters)
            bool hasLink2 = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && (c.Id == CardId.CrusadiaRegulex || c.Id == CardId.CrusadiaSpatha));
            if (hasLink2 && Bot.GetMonsterCount() >= 2) return true;

            // Option 2: Any Link monster (e.g. Magius) + 2 effect monsters (total >= 3 monsters)
            bool hasLink1 = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Link));
            if (hasLink1 && Bot.GetMonsterCount() >= 3) return true;

            return false;
        }

        private bool AvramaxSummon()
        {
            if (Bot.HasInMonstersZone(CardId.Avramax)) return false;
            // Summon Avramax if going 1st (control boss) or if Equimax already attacked or if needed for defense
            if (Duel.Turn <= 1)
            {
                return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax) && Bot.GetMonsterCount() >= 2;
            }
            return false; // In Turn 2+, prefer staying on Equimax for OTK
        }

        private bool BorrelswordSummon()
        {
            if (Bot.HasInMonstersZone(CardId.BorrelswordDragon)) return false;
            return !Bot.ExtraDeck.Any(c => c.Id == CardId.CrusadiaEquimax) && Bot.GetMonsterCount() >= 3;
        }

        private bool KnightmarePhoenixSummon()
        {
            return Enemy.GetSpellCount() > 0 && Bot.GetMonsterCount() >= 2 && !Bot.HasInMonstersZone(CardId.CrusadiaEquimax);
        }

        private bool KnightmareCerberusSummon()
        {
            return Enemy.GetMonsters().Any(c => c.IsSpecialSummoned && c.IsFaceup()) && Bot.GetMonsterCount() >= 2 && !Bot.HasInMonstersZone(CardId.CrusadiaEquimax);
        }

        private bool KnightmareUnicornSummon()
        {
            return Util.GetProblematicEnemyCard() != null && Bot.GetMonsterCount() >= 3 && !Bot.HasInMonstersZone(CardId.CrusadiaEquimax);
        }

        private bool TrisbaenaSummon()
        {
            return Enemy.GetSpellCount() >= 2 && Bot.GetMonsterCount() >= 3 && !Bot.HasInMonstersZone(CardId.CrusadiaEquimax);
        }

        private bool SaryujaSummon()
        {
            return Bot.GetMonsterCount() >= 4 && !Bot.HasInMonstersZone(CardId.CrusadiaEquimax);
        }

        // ================================================================================================
        // TIER 11 & 12: Mekk-Knights & Starter Normal Summon
        // ================================================================================================
        private bool MekkKnightSpSummon()
        {
            // Only summon Mekk-Knights if there is a column with 2+ cards
            return HasColumnWithTwoCards();
        }

        private bool PurpleActivate()
        {
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
            AI.SelectCard(new[] { CardId.MekkKnightPurple, CardId.MekkKnightIndigo });
            return true;
        }

        private bool IndigoActivate()
        {
            var target = Bot.GetMonsters().FirstOrDefault(c => c.Id == CardId.MekkKnightIndigo && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CrusadiaNormalSummon()
        {
            if (!CrusadiaMainMonsters.Contains(Card.Id)) return false;

            // If we have Slumber and opponent has monsters, hold Normal Summon to let Slumber clear first!
            if (Bot.Hand.Any(c => c.Id == CardId.InterruptedKaijuSlumber) && !_slumberUsed && Enemy.GetMonsterCount() > 0)
            {
                return false;
            }

            _normalSummonedCrusadia = true;
            return true;
        }

        private bool SpellSetLogic()
        {
            // Set Quick-Play / Traps at end of Main 2 or if needed for Mekk-Knight column
            if (Card.IsCode(CardId.CrusadiaPower, CardId.CalledByTheGrave, CardId.CosmicCyclone))
            {
                return Duel.Phase == DuelPhase.Main2 || Duel.Turn <= 1;
            }
            return false;
        }

        // ================================================================================================
        // Precise Placement Engine (OnSelectPlace)
        // ================================================================================================
        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            if (available <= 0) return 0;
            AI?.Log(LogLevel.Info, $"[SELECT-PLACE-IN] cardId={cardId}, player={player}, loc={location}, available=0x{available:X}");

            // 1. Placing Kaiju on Opponent's Field (player == 1, MonsterZone)
            if (player == 1 && location == CardLocation.MonsterZone)
            {
                var equimax = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
                if (equimax != null)
                {
                    // Opponent zone directly opposite to Equimax top pointer:
                    // If Equimax is in EMZ 5 (left) -> Points to Opponent MMZ 3
                    // If Equimax is in EMZ 6 (right) -> Points to Opponent MMZ 1
                    int targetOppZone = (equimax.Sequence == 5) ? 3 : 1;
                    if ((available & (1 << targetOppZone)) > 0)
                    {
                        AI?.Log(LogLevel.Info, $"[SELECT-PLACE-OUT] Kaiju OppZone: {targetOppZone}");
                        return 1 << targetOppZone;
                    }
                }
                else
                {
                    // Default Kaiju placement opposite to EMZ 5 (Opponent MMZ 3)
                    if ((available & (1 << 3)) > 0) { AI?.Log(LogLevel.Info, "[SELECT-PLACE-OUT] Kaiju default OppZone 3"); return 1 << 3; }
                    if ((available & (1 << 1)) > 0) { AI?.Log(LogLevel.Info, "[SELECT-PLACE-OUT] Kaiju default OppZone 1"); return 1 << 1; }
                }
                int fallbackOpp = base.OnSelectPlace(cardId, player, location, available);
                AI?.Log(LogLevel.Info, $"[SELECT-PLACE-OUT] Kaiju fallback 0x{fallbackOpp:X}");
                return fallbackOpp;
            }

            // 2. Placing Bot's Monsters (player == 0, MonsterZone)
            if (player == 0 && location == CardLocation.MonsterZone)
            {
                // Extra Monster Zone check (Summoning Link Monster from Extra Deck)
                int emzAvailable = available & (Zones.z5 | Zones.z6);
                if (emzAvailable > 0)
                {
                    // Always prefer EMZ 5 (Left EMZ) so pointer alignments are 100% consistent
                    if ((emzAvailable & Zones.z5) > 0) { AI?.Log(LogLevel.Info, "[SELECT-PLACE-OUT] EMZ z5 (0x20)"); return Zones.z5; }
                    if ((emzAvailable & Zones.z6) > 0) { AI?.Log(LogLevel.Info, "[SELECT-PLACE-OUT] EMZ z6 (0x40)"); return Zones.z6; }
                }

                // If Bot already has Magius / Regulex on field:
                var magius = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaMagius);
                if (magius != null)
                {
                    // Magius points directly DOWN:
                    // EMZ 5 points to MMZ 1
                    // EMZ 6 points to MMZ 3
                    int targetZone = (magius.Sequence == 5) ? 1 : 3;
                    if ((available & (1 << targetZone)) > 0)
                    {
                        AI?.Log(LogLevel.Info, $"[SELECT-PLACE-OUT] Under Magius (Seq={magius.Sequence}) -> MMZ {targetZone}");
                        return 1 << targetZone;
                    }
                }

                var regulex = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaRegulex);
                if (regulex != null)
                {
                    int targetZone = (regulex.Sequence == 5) ? 1 : 3;
                    if ((available & (1 << targetZone)) > 0)
                    {
                        AI?.Log(LogLevel.Info, $"[SELECT-PLACE-OUT] Under Regulex (Seq={regulex.Sequence}) -> MMZ {targetZone}");
                        return 1 << targetZone;
                    }
                }

                var equimax = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
                if (equimax != null)
                {
                    int zLeft = (equimax.Sequence == 5) ? 0 : 2;
                    int zRight = (equimax.Sequence == 5) ? 2 : 4;

                    if ((available & (1 << zLeft)) > 0) { AI?.Log(LogLevel.Info, $"[SELECT-PLACE-OUT] Under Equimax -> MMZ {zLeft}"); return 1 << zLeft; }
                    if ((available & (1 << zRight)) > 0) { AI?.Log(LogLevel.Info, $"[SELECT-PLACE-OUT] Under Equimax -> MMZ {zRight}"); return 1 << zRight; }
                }

                // If Normal Summoning Starter (no Link monster on field yet):
                // Place into MMZ 2 (Center) so MMZ 1 (under EMZ 5) and MMZ 3 (under EMZ 6) stay FREE for Link pointers!
                if ((available & Zones.z2) > 0) { AI?.Log(LogLevel.Info, "[SELECT-PLACE-OUT] Starter MMZ 2"); return Zones.z2; }
                if ((available & Zones.z0) > 0) { AI?.Log(LogLevel.Info, "[SELECT-PLACE-OUT] MMZ 0"); return Zones.z0; }
                if ((available & Zones.z4) > 0) { AI?.Log(LogLevel.Info, "[SELECT-PLACE-OUT] MMZ 4"); return Zones.z4; }
                if ((available & Zones.z1) > 0) { AI?.Log(LogLevel.Info, "[SELECT-PLACE-OUT] MMZ 1"); return Zones.z1; }
                if ((available & Zones.z3) > 0) { AI?.Log(LogLevel.Info, "[SELECT-PLACE-OUT] MMZ 3"); return Zones.z3; }
            }

            int defPlace = base.OnSelectPlace(cardId, player, location, available);
            AI?.Log(LogLevel.Info, $"[SELECT-PLACE-OUT] base.OnSelectPlace 0x{defPlace:X}");
            return defPlace;
        }

        // ================================================================================================
        // Card Selection (OnSelectCard) Handler
        // ================================================================================================
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Saved Kaiju Tribute Target
            if (_kaijuTributeTarget != null && cards.Contains(_kaijuTributeTarget))
            {
                var target = _kaijuTributeTarget;
                _kaijuTributeTarget = null;
                return new List<ClientCard> { target };
            }

            // 2. Searching Crusadia cards (Hint 506 - HINTMSG_ATOHAND or Deck search)
            if (hint == 506 || hint == 505)
            {
                // Magius Search:
                // MAXIMUS IS THE ABSOLUTE OTK CORE!
                // Prioritize Maximus FIRST if not already in Hand or MonsterZone!
                // Only search Draco if Maximus is already secured!
                if (cards.Any(c => c.Location == CardLocation.Deck && CrusadiaMainMonsters.Contains(c.Id)))
                {
                    bool hasMaximus = Bot.Hand.Any(c => c.Id == CardId.CrusadiaMaximus) || Bot.MonsterZone.Any(c => c != null && c.Id == CardId.CrusadiaMaximus);
                    bool hasDraco = Bot.Hand.Any(c => c.Id == CardId.CrusadiaDraco) || Bot.MonsterZone.Any(c => c != null && c.Id == CardId.CrusadiaDraco);

                    int targetId = !hasMaximus ? CardId.CrusadiaMaximus
                                 : !hasDraco ? CardId.CrusadiaDraco
                                 : CardId.CrusadiaArboria;

                    var match = cards.FirstOrDefault(c => c.Id == targetId) ?? cards.FirstOrDefault(c => CrusadiaMainMonsters.Contains(c.Id));
                    if (match != null) return new List<ClientCard> { match };
                }

                // Regulex Search:
                // Prioritize Crusadia Revival (for multi-attack OTK), then Crusadia Power, then Testament
                if (cards.Any(c => c.Location == CardLocation.Deck && (c.Id == CardId.CrusadiaRevival || c.Id == CardId.CrusadiaPower || c.Id == CardId.CrusadiaTestament)))
                {
                    bool hasRevival = Bot.Hand.Any(c => c.Id == CardId.CrusadiaRevival) || Bot.SpellZone.Any(c => c != null && c.Id == CardId.CrusadiaRevival);
                    int targetSpellId = !hasRevival ? CardId.CrusadiaRevival : CardId.CrusadiaPower;

                    var match = cards.FirstOrDefault(c => c.Id == targetSpellId)
                             ?? cards.FirstOrDefault(c => c.Id == CardId.CrusadiaRevival || c.Id == CardId.CrusadiaPower || c.Id == CardId.CrusadiaTestament);
                    if (match != null) return new List<ClientCard> { match };
                }

                // ROTA Search:
                if (cards.Any(c => c.Location == CardLocation.Deck && c.Id == CardId.CrusadiaArboria))
                {
                    var arboria = cards.FirstOrDefault(c => c.Id == CardId.CrusadiaArboria);
                    if (arboria != null) return new List<ClientCard> { arboria };
                }
            }

            // 3. Draco Retrieval from Graveyard (Hint 505 - HINTMSG_RTOHAND)
            if (cards.Any(c => c.Location == CardLocation.Grave && CrusadiaMainMonsters.Contains(c.Id)))
            {
                var preferred = new[] { CardId.CrusadiaMaximus, CardId.CrusadiaArboria, CardId.CrusadiaLeonis, CardId.CrusadiaReclusia };
                foreach (int id in preferred)
                {
                    var match = cards.FirstOrDefault(c => c.Id == id);
                    if (match != null) return new List<ClientCard> { match };
                }
            }

            // 4. Slumber Summons from Deck (Hint 509 - HINTMSG_SPSUMMON)
            // One Kaiju to Bot field, one to Enemy field:
            // Bot gets Gameciel (2200 ATK, turtle) or strongest Kaiju depending on whether it summons to our field first
            if (hint == 509 && cards.Any(c => c.Location == CardLocation.Deck && KaijuMonsters.Contains(c.Id)))
            {
                var kaijus = cards.Where(c => KaijuMonsters.Contains(c.Id)).ToList();
                if (kaijus.Count >= min)
                {
                    // If summoning to enemy field: give them the one with LOWER ATK so Equimax can deal more damage, or give them Kumongous/Gameciel
                    return kaijus.OrderBy(c => c.Attack).Take(min).ToList();
                }
            }

            // 5. Link Materials Selection (Hint 533 or 513)
            if (hint == 533 || hint == 513)
            {
                // Never send Equimax or Avramax to grave as link material unless upgrading to Avramax
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).Take(max).ToList();
                return sorted;
            }

            // 6. Equimax Tribute for Negate (Hint 500 - HINTMSG_RELEASE)
            if (hint == 500)
            {
                // Select an extender in MMZ, NEVER Equimax itself!
                var tribute = cards.Where(c => c != null && c.Id != CardId.CrusadiaEquimax).OrderBy(c => GetMaterialPriority(c)).FirstOrDefault();
                if (tribute != null) return new List<ClientCard> { tribute };
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
            // Crusadia main deck extenders summon in Defense Position by card effect
            if (positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (attacker != null && attacker.Id == CardId.CrusadiaEquimax)
            {
                // Equimax wants to attack the monster it points to (for max damage or piercing)
                var pointedEnemy = defenders.FirstOrDefault(d => d != null && d.IsFaceup() && IsEquimaxPointingTo(attacker, d));
                if (pointedEnemy != null)
                {
                    return AI.Attack(attacker, pointedEnemy);
                }

                // Otherwise attack lowest ATK or attack position monster for maximum lethal damage
                var bestTarget = defenders.Where(d => d != null && d.IsFaceup())
                                          .OrderBy(d => d.Attack)
                                          .FirstOrDefault();
                if (bestTarget != null)
                {
                    return AI.Attack(attacker, bestTarget);
                }
            }
            return base.OnSelectAttackTarget(attacker, defenders);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Id == CardId.CrusadiaEquimax) return 1000;
            if (c.Id == CardId.Avramax) return 950;
            if (c.Id == CardId.CrusadiaMaximus) return 800; // Keep Maximus for ATK boost & double damage
            if (c.Id == CardId.CrusadiaRegulex) return 200;
            if (c.Id == CardId.CrusadiaMagius) return 100;
            if (CrusadiaMainMonsters.Contains(c.Id)) return 50;
            if (c.Id == CardId.WorldCrown) return 40;
            return 100;
        }

        // ================================================================================================
        // Link Pointer & Zone Verification Helpers
        // ================================================================================================
        private bool HasEmptyPointedZone()
        {
            var magius = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaMagius);
            if (magius != null)
            {
                int targetZone = (magius.Sequence == 5) ? 1 : 3;
                if (Bot.MonsterZone[targetZone] == null) return true;
            }

            var regulex = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaRegulex);
            if (regulex != null)
            {
                int targetZone = (regulex.Sequence == 5) ? 1 : 3;
                if (Bot.MonsterZone[targetZone] == null) return true;
            }

            var equimax = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && c.Id == CardId.CrusadiaEquimax);
            if (equimax != null)
            {
                int zLeft = (equimax.Sequence == 5) ? 0 : 2;
                int zRight = (equimax.Sequence == 5) ? 2 : 4;
                if (Bot.MonsterZone[zLeft] == null || Bot.MonsterZone[zRight] == null) return true;
            }

            return false;
        }

        private bool IsEquimaxPointingTo(ClientCard equimax, ClientCard target)
        {
            if (equimax == null || target == null) return false;

            if (target.Controller == 0) // Bot monster
            {
                int zLeft = (equimax.Sequence == 5) ? 0 : 2;
                int zRight = (equimax.Sequence == 5) ? 2 : 4;
                return target.Sequence == zLeft || target.Sequence == zRight;
            }
            else // Enemy monster
            {
                int targetOppZone = (equimax.Sequence == 5) ? 3 : 1;
                return target.Sequence == targetOppZone;
            }
        }

        private bool HasColumnWithTwoCards()
        {
            // Mekk-Knights summon condition: same column has 2 or more cards
            for (int col = 0; col < 5; col++)
            {
                int count = 0;
                if (Bot.MonsterZone[col] != null) count++;
                if (Bot.SpellZone[col] != null) count++;
                int oppCol = 4 - col;
                if (Enemy.MonsterZone[oppCol] != null) count++;
                if (Enemy.SpellZone[oppCol] != null) count++;

                // EMZ columns
                if (col == 1)
                {
                    if (Bot.MonsterZone[5] != null || Enemy.MonsterZone[6] != null) count++;
                }
                else if (col == 3)
                {
                    if (Bot.MonsterZone[6] != null || Enemy.MonsterZone[5] != null) count++;
                }

                if (count >= 2 && Bot.MonsterZone[col] == null)
                    return true;
            }
            return false;
        }
    }
}
