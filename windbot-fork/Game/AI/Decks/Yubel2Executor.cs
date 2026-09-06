// ============================================================================
// CARD AUDIT — YUBEL-FIENDSMITH (Yubel2)
// ============================================================================
// | Card Name              | Type       | OPT? | Cost        | Effect Summary                         | Activate When                     | NEVER When                           |
// |------------------------|------------|------|-------------|----------------------------------------|-----------------------------------|--------------------------------------|
// | Yubel                  | Monster    | No   | None        | Battle damage reflection, float on dest| Battle Phase block, float trigger | Tribute material without win-con     |
// | Yubel - Terror Incarn. | Monster    | No   | None        | End Phase board wipe, float on leave   | Disrupt opponent field            | If we have cards we need on board    |
// | Yubel - Ult. Nightmare | Monster    | No   | None        | Destroy battle target + reflect damage | Battle Phase pressure             | -                                    |
// | Spirit of Yubel        | Monster    | HOPT | None        | SS on attack, set Yubel spell/trap     | Opponent attack, deck search      | Already used opt effect              |
// | Samsara D Lotus        | Monster    | HOPT | Tribute self| Tribute to SS Yubel monster from deck  | Setup turn, starter play          | Special Summon is blocked            |
// | Nightmare Throne       | Field Spell| HOPT | None        | Search 0 ATK/DEF Fiend, float on leave | Combo starter, recovery play       | Already active                       |
// | Nightmare Pain         | Cont. Spell| HOPT | Destroy Dark| Destroy Dark to search mentioning Yubel| Setup turn, search combo piece    | No Dark monster in hand/field        |
// | Gruesome Grave Squirm. | Monster    | HOPT | None        | SS self, destroy own Yubel to revive   | Extend combo, trigger Yubel float  | No Yubel to destroy or GY empty      |
// | Opening of Spirit Gates| Cont. Spell| HOPT | Discard 1   | Search Beckoning, revive 0 ATK/DEF Fiend| Setup turn, starter/extender      | Hand is empty                        |
// | Dark Beckoning Beast   | Monster    | HOPT | None        | Search Gates, extra 0 ATK/DEF NS       | Extra summon, setup               | Already normal summoned              |
// | Fiendsmith Engraver    | Monster    | HOPT | Discard self| Search Tract, SS itself by sending Fiend| Light Fiend setup, combo starter  | No Light Fiends to send              |
// | Fiendsmith's Tract     | Spell      | HOPT | Discard 1   | Search Light Fiend, GY fusion summon   | Setup turn, Fiendsmith engine     | Hand is empty                        |
// | Lacrima Crimson Tears  | Monster    | HOPT | None        | Send Fiendsmith card, quick GY revive  | Starter, opponent turn disruption | -                                    |
// | Fabled Lurrie          | Monster    | No   | None        | SS itself when discarded to GY         | Discard fodder for Tract/Gates     | Already on field                     |
// | Yubel Loving Defender  | Fusion     | -    | None        | Burn damage, banish battle targets     | Clearing opponent board, lethal   | No opponent monsters to fuse         |
// | Neos Kluger            | Fusion     | -    | None        | Reflect damage, float into Neos        | Battle Phase protection, lethal   | -                                    |
// ============================================================================
// ACE CARDS:
//   Primary: Yubel - The Loving Defender Forever (47172959) — Board wipe + massive burn
//   Secondary: Elemental HERO Neos Kluger (90307498) — Battle float
//   Engine Ace: Yubel (78371393) / Spirit of Yubel (90829280) — Main loops
// COMBO STARTERS:
//   1. Samsara D Lotus (62318994) — SS Spirit of Yubel
//   2. Nightmare Throne (93729896) — Search Samsara or Spirit
//   3. Dark Beckoning Beast (81034083) / Opening of the Spirit Gates (80312545) — Sacred Beast loop
//   4. Fiendsmith Engraver (60764609) — Fiendsmith Fusion plays
// WIN CONDITION: Reflection damage via Nightmare Pain + Yubel, or Loving Defender OTK
// GOING 1ST END BOARD: Yubel - Terror Incarnate + Samsara Lotus loop + Eternal Favorite
// GOING 2ND GAMEPLAN: Super Poly / Loving Defender contact fusion -> direct attack reflection
// CHOKEPOINTS: Samsara Lotus negated / Special Summon blocked
// ============================================================================

using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("Yubel2", "Yubel2")]
    public class Yubel2Executor : ModernExecutor
    {
        public class CardId
        {
            // Yubel Main Deck
            public const int ElementalHERONeos = 89943723;
            public const int YubelTheUltimateNightmare = 31764700;
            public const int YubelTerrorIncarnate = 4779091;
            public const int SpiritOfYubel = 90829280;
            public const int Yubel = 78371393;
            public const int GeistgrinderGolem = 26913989;
            public const int GruesomeGraveSquirmer = 24215921;
            public const int SamsaraDLotus = 62318994;

            // Fiendsmith Main Deck
            public const int FiendsmithEngraver = 60764609;
            public const int LacrimaTheCrimsonTears = 28803166;
            public const int FabledLurrie = 97651498;

            // Sacred Beast Engine
            public const int DarkBeckoningBeast = 81034083;

            // Spells & Traps
            public const int NightmarePain = 65261141;
            public const int OpeningOfTheSpiritGates = 80312545;
            public const int NightmareThrone = 93729896;
            public const int DarkHole = 53129443;
            public const int FiendsmithsTract = 98567237;
            public const int FusionDeployment = 6498706;
            public const int MutinyInTheSky = 71593652;
            public const int FinalBringerOfTheEndTimes = 54261514;
            public const int ForbiddenDroplet = 24299458;
            public const int SuperPolymerization = 48130397;
            public const int EternalFavorite = 87532344;

            // Extra Deck
            public const int YubelTheLovingDefenderForever = 47172959;
            public const int ElementalHERONeosKluger = 90307498;
            public const int FiendsmithsDesirae = 82135803;
            public const int LuceTheDusksDark = 45409943;
            public const int StarvingVenomFusionDragon = 41209827;
            public const int FiendsmithsLacrima = 46640168;
            public const int AerialEater = 28143384;
            public const int TheDukeOfDemise = 45445571;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int ChaosAngel = 22850702;
            public const int SuperdreadnoughtRailCannonSuperDora = 49032236;
            public const int SuperdreadnoughtRailCannonGustavMax = 56910167;

            // Side Deck / Custom
            public const int FiendsmithKyrie = 26434972;
            public const int FiendsmithsSanct = 35552985;
            public const int FiendsmithInParadise = 99989863;
            public const int TorrentialTribute = 53582587;
            
            // Generic Staples
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int EffectVeiler = 97268402;
            public const int CalledByTheGrave = 24224830;
        }

        // List of Ace Cards that shouldn't be casually used as Fusion/Link/Synchro materials
        private static readonly int[] AceCardIds = {
            CardId.YubelTheLovingDefenderForever,
            CardId.ElementalHERONeosKluger,
            CardId.FiendsmithsDesirae,
            CardId.LuceTheDusksDark,
            CardId.YubelTerrorIncarnate,
            CardId.YubelTheUltimateNightmare,
            CardId.Yubel
        };

        // Yubel monster IDs for floating/reflective checks
        private static readonly int[] YubelMonsters = {
            CardId.Yubel,
            CardId.SpiritOfYubel,
            CardId.YubelTerrorIncarnate,
            CardId.YubelTheUltimateNightmare,
            CardId.YubelTheLovingDefenderForever
        };

        // Once per turn flags to prevent infinite loops and optimize resource usage
        private bool _throneUsed = false;
        private bool _painUsed = false;
        private bool _samsaraUsed = false;
        private bool _squirmerHandUsed = false;
        private bool _squirmerFieldUsed = false;
        private bool _squirmerGraveUsed = false;
        private bool _gatesHandUsed = false;
        private bool _gatesFieldUsed = false;
        private bool _beckoningUsed = false;
        private bool _engraverHandUsed = false;
        private bool _engraverGraveUsed = false;
        private bool _tractHandUsed = false;
        private bool _tractGraveUsed = false;
        private bool _favoriteUsed = false;

        public Yubel2Executor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // 1. Register Ace Cards to prevent the bot from using them blindly
            ResourcePlan.RegisterAceCards(AceCardIds);

            // 2. Setup Combo Line Routes using ComboRouter
            // Plan A: Main Yubel setup loop
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Yubel-Throne-Pain",
                RequiredCards = new List<int> { CardId.NightmareThrone, CardId.SamsaraDLotus },
                FallbackLineName = "Beckoning-Beast-Lotus",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.NightmareThrone, ActionType = ExecutorType.Activate, Description = "Search Samsara/Spirit" },
                    new() { CardId = CardId.SamsaraDLotus, ActionType = ExecutorType.Summon, Description = "Summon Samsara Lotus" },
                    new() { CardId = CardId.SamsaraDLotus, ActionType = ExecutorType.Activate, Description = "Tribute Samsara to SS Spirit of Yubel" },
                    new() { CardId = CardId.SpiritOfYubel, ActionType = ExecutorType.Activate, Description = "Set Nightmare Pain" }
                },
                EndBoardScore = 90
            });

            // Plan B: Sacred Beast engine setup
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Beckoning-Beast-Lotus",
                RequiredCards = new List<int> { CardId.DarkBeckoningBeast, CardId.OpeningOfTheSpiritGates },
                FallbackLineName = "Fiendsmith-Engraver-Fallback",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.OpeningOfTheSpiritGates, ActionType = ExecutorType.Activate, Description = "Search Beckoning Beast" },
                    new() { CardId = CardId.DarkBeckoningBeast, ActionType = ExecutorType.Summon, Description = "Summon Beckoning, search Gates/Lotus" },
                    new() { CardId = CardId.SamsaraDLotus, ActionType = ExecutorType.Summon, Description = "Use extra Normal Summon for Lotus" }
                },
                EndBoardScore = 75
            });

            // Plan C: Fiendsmith setup when Yubel engine is blocked or negated
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Fiendsmith-Engraver-Fallback",
                RequiredCards = new List<int> { CardId.FiendsmithEngraver },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.FiendsmithEngraver, ActionType = ExecutorType.Activate, Description = "Discard Engraver to search Tract" },
                    new() { CardId = CardId.FiendsmithsTract, ActionType = ExecutorType.Activate, Description = "Search Lurrie, discard to SS Lurrie" }
                },
                EndBoardScore = 60,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.FiendsmithEngraver)
            });

            // 3. Register Starter and Bait Cards
            BaitPlanner.RegisterComboStarters(CardId.NightmareThrone, CardId.SamsaraDLotus, CardId.OpeningOfTheSpiritGates, CardId.DarkBeckoningBeast);
            BaitPlanner.RegisterBaitCards(CardId.DarkHole, CardId.ForbiddenDroplet);
            ChainAdvisor.RegisterHighValueTargets(CardId.NightmarePain, CardId.OpeningOfTheSpiritGates, CardId.NightmareThrone, CardId.SamsaraDLotus);

            // ==========================================
            // TIER 1: Hand Traps & Protection
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, VeilerActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);

            // ==========================================
            // TIER 2: Quick Effects & Board Disruptions
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsDesirae, DesiraeNegate);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, DropletActivate);
            AddExecutor(ExecutorType.Activate, CardId.EternalFavorite, EternalFavoriteActivate);

            // ==========================================
            // TIER 3: Field Spells & Main Combo Starters
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.NightmareThrone, ThroneActivate);
            AddExecutor(ExecutorType.Activate, CardId.OpeningOfTheSpiritGates, GatesActivate);
            AddExecutor(ExecutorType.Activate, CardId.FusionDeployment, FusionDeploymentActivate);

            // ==========================================
            // TIER 4: Yubel Engine Core Activations
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.SamsaraDLotus, LotusActivate);
            AddExecutor(ExecutorType.Activate, CardId.NightmarePain, NightmarePainActivate);
            AddExecutor(ExecutorType.Activate, CardId.GruesomeGraveSquirmer, GraveSquirmerActivate);
            AddExecutor(ExecutorType.Activate, CardId.SpiritOfYubel, SpiritOfYubelActivate);
            AddExecutor(ExecutorType.Activate, CardId.Yubel, YubelFloatActivate);
            AddExecutor(ExecutorType.Activate, CardId.YubelTerrorIncarnate, YubelTerrorActivate);

            // ==========================================
            // TIER 5: Fiendsmith Engine core plays
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithEngraver, EngraverActivate);
            AddExecutor(ExecutorType.Activate, CardId.FiendsmithsTract, TractActivate);
            AddExecutor(ExecutorType.Activate, CardId.LacrimaTheCrimsonTears, LacrimaActivate);
            AddExecutor(ExecutorType.Activate, CardId.MutinyInTheSky, MutinyActivate);
            AddExecutor(ExecutorType.Activate, CardId.FinalBringerOfTheEndTimes, FinalBringerActivate);

            // ==========================================
            // TIER 6: Normal Summons & Extra Summons
            // ==========================================
            AddExecutor(ExecutorType.Summon, CardId.DarkBeckoningBeast, BeckoningBeastSummon);
            AddExecutor(ExecutorType.Summon, CardId.SamsaraDLotus, LotusSummon);
            AddExecutor(ExecutorType.Summon, CardId.GruesomeGraveSquirmer, GenericSummon);
            AddExecutor(ExecutorType.MonsterSet, CardId.SpiritOfYubel, () => Util.IsTurn1OrMain2());

            // ==========================================
            // TIER 7: Special Summons (Link/Fusion/Xyz)
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.YubelTheLovingDefenderForever, LovingDefenderSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.ElementalHERONeosKluger, NeosKlugerSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsDesirae, DesiraeSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LuceTheDusksDark, LuceContactSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.FiendsmithsLacrima, LacrimaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AerialEater, AerialEaterSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TheDukeOfDemise, DukeOfDemiseSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.StarvingVenomFusionDragon, SuperPolymerizationActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.GaruraWingsOfResonantLife, SuperPolymerizationActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.ChaosAngel, ChaosAngelSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperdreadnoughtRailCannonGustavMax, XyzRank10Summon);
            AddExecutor(ExecutorType.SpSummon, CardId.SuperdreadnoughtRailCannonSuperDora, XyzRank10Summon);

            // ==========================================
            // TIER 8: Sets & Pass Strategy
            // ==========================================
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.EternalFavorite, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.FinalBringerOfTheEndTimes, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Yubel performs best going first to establish the destruction loop and set disruptions
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _throneUsed = false;
            _painUsed = false;
            _samsaraUsed = false;
            _squirmerHandUsed = false;
            _squirmerFieldUsed = false;
            _squirmerGraveUsed = false;
            _gatesHandUsed = false;
            _gatesFieldUsed = false;
            _beckoningUsed = false;
            _engraverHandUsed = false;
            _engraverGraveUsed = false;
            _tractHandUsed = false;
            _tractGraveUsed = false;
            _favoriteUsed = false;
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            // Extra Deck monsters that might consume our precious Yubel forms as materials
            if (card.HasType(CardType.Link | CardType.Xyz | CardType.Synchro))
            {
                var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
                if (activeAces.Count > 0)
                {
                    bool allowed = false;
                    foreach (var mat in activeAces)
                    {
                        var res = ResourcePlan.EvaluateAceUsage(
                            card: mat,
                            hasLethalIfUsed: CanDealLethal(),
                            isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                            haveAlternateWinCon: Bot.Hand.Any(c => c != null && c.Id == CardId.OpeningOfTheSpiritGates)
                        );
                        if (res.allowed)
                        {
                            allowed = true;
                            break;
                        }
                    }
                    if (!allowed)
                    {
                        DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} is not safe (would consume Ace card(s))");
                        return false;
                    }
                }
            }
            return true;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.NightmareThrone && card.Location == CardLocation.Hand) _throneUsed = true;
                if (card.Id == CardId.NightmarePain && card.Location == CardLocation.SpellZone) _painUsed = true;
                if (card.Id == CardId.SamsaraDLotus) _samsaraUsed = true;
                if (card.Id == CardId.GruesomeGraveSquirmer)
                {
                    if (card.Location == CardLocation.Hand) _squirmerHandUsed = true;
                    if (card.Location == CardLocation.MonsterZone) _squirmerFieldUsed = true;
                    if (card.Location == CardLocation.Grave) _squirmerGraveUsed = true;
                }
                if (card.Id == CardId.OpeningOfTheSpiritGates)
                {
                    if (card.Location == CardLocation.Hand) _gatesHandUsed = true;
                    if (card.Location == CardLocation.SpellZone) _gatesFieldUsed = true;
                }
                if (card.Id == CardId.DarkBeckoningBeast) _beckoningUsed = true;
                if (card.Id == CardId.FiendsmithEngraver)
                {
                    if (card.Location == CardLocation.Hand) _engraverHandUsed = true;
                    if (card.Location == CardLocation.Grave) _engraverGraveUsed = true;
                }
                if (card.Id == CardId.FiendsmithsTract)
                {
                    if (card.Location == CardLocation.Hand) _tractHandUsed = true;
                    if (card.Location == CardLocation.Grave) _tractGraveUsed = true;
                }
                if (card.Id == CardId.EternalFavorite) _favoriteUsed = true;
            }
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.ElementalHERONeos) return 850;
            if (c.Id == CardId.FabledLurrie) return 100; // Easiest material
            if (c.Id == CardId.DarkBeckoningBeast) return 200;
            if (c.Id == CardId.SamsaraDLotus) return 300;
            if (c.Id == CardId.GruesomeGraveSquirmer) return 400;
            return 500;
        }

        protected override bool IsBoardStrongEnough()
        {
            // We have a solid Yubel loop or a fusion boss
            if (Bot.HasInMonstersZone(CardId.YubelTheLovingDefenderForever) || Bot.HasInMonstersZone(CardId.FiendsmithsDesirae))
                return true;
            if (Bot.HasInMonstersZone(CardId.YubelTerrorIncarnate) && Bot.HasInSpellZone(CardId.NightmarePain))
                return true;
            return base.IsBoardStrongEnough();
        }

        // ==========================================
        // HAND TRAP LOGIC
        // ==========================================

        private bool AshActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool MaxxCActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.Player != 1) return false; // Only use on opponent's turn
            return true;
        }

        private bool VeilerActivate()
        {
            if (Duel.Player != 1) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return true;
        }

        // ==========================================
        // QUICK EFFECTS & DISRUPTIONS
        // ==========================================

        private bool DesiraeNegate()
        {
            // Bypasses ShouldSkipCombo() because negations are critical to disrupt opponent plays
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;

            // Desirae negates face-up cards on field. Prioritize the card that is currently activating an effect!
            ClientCard target = null;
            if (LastChainCard.Location == CardLocation.MonsterZone && LastChainCard.IsFaceup() && !IsTargetImmune(LastChainCard))
            {
                target = LastChainCard;
            }

            if (target == null)
            {
                target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && !IsTargetImmune(c))
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
            }

            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("DesiraeNegate", $"Negating enemy {target.Name}");
                return true;
            }
            return false;
        }

        private bool SuperPolymerizationActivate()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            // We look to fuse away the opponent's board.
            // Loving Defender can absorb all effect monsters on the field!
            int enemyEffectMonsters = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsMonster() && !c.HasType(CardType.Normal));
            bool hasYubelOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id));

            if (hasYubelOnField && enemyEffectMonsters >= 1)
            {
                AI.SelectCard(CardId.YubelTheLovingDefenderForever);
                DecisionTracer.TraceActivate("SuperPolymerizationActivate", "Fusing opponent board into Loving Defender");
                return true;
            }

            // Fallback: Starving Venom (2 Dark monsters)
            int enemyDarkMonsters = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Dark));
            int ourDarkMonsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Dark));
            if (enemyDarkMonsters >= 1 && (enemyDarkMonsters + ourDarkMonsters) >= 2)
            {
                DecisionTracer.TraceActivate("SuperPolymerizationActivate", "Fusing into Starving Venom");
                return true;
            }

            // Fallback 2: Garura (2 monsters of same type/attribute with different names)
            if (Enemy.GetMonsterCount() >= 2)
            {
                DecisionTracer.TraceActivate("SuperPolymerizationActivate", "Fusing into Garura");
                return true;
            }

            return false;
        }

        private bool DropletActivate()
        {
            var target = Util.GetProblematicEnemyCard();
            if (target != null && target.IsMonster())
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool EternalFavoriteActivate()
        {
            if (_favoriteUsed) return false;
            if (Card.IsFacedown() && Duel.Player == 0) return false; // Don't trigger on own turn if set

            // Effect 2: Fusion Summon using either field (requires 1 hand discard as cost)
            // Priority 1: Wipe opponent board via Fusion
            bool hasYubelField = Bot.GetMonsters().Concat(Enemy.GetMonsters())
                .Any(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id));
            bool hasEnemyEffectMonsters = Enemy.GetMonsters()
                .Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect));
            bool canFuseFavorite = hasYubelField && hasEnemyEffectMonsters && Bot.Hand.Count > 0 && !IsSpecialSummonBlocked();

            if (canFuseFavorite)
            {
                AI.SelectOption(1);
                _favoriteUsed = true;
                DecisionTracer.TraceActivate("EternalFavoriteActivate", "Fusion Summon via Eternal Favorite");
                return true;
            }

            // Effect 1: Revive Yubel from GY/banished
            bool canRevive = Bot.Graveyard.Concat(Bot.Banished).Any(c => c != null && c.Id == CardId.Yubel);
            bool hasEmptySlot = Bot.GetMonsterCount() < 5;

            if (canRevive && hasEmptySlot)
            {
                AI.SelectOption(0);
                _favoriteUsed = true;
                DecisionTracer.TraceActivate("EternalFavoriteActivate", "Reviving Yubel");
                return true;
            }

            return false;
        }

        // ==========================================
        // STARTERS & SETUP CARD LOGIC
        // ==========================================

        private bool ThroneActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_throneUsed) return false;
                if (ShouldSkipCombo()) return false;
                
                // When activated, we can add 1 Fiend with 0 ATK/DEF from Deck to Hand or destroy it.
                // We should select either Samsara D Lotus or Spirit of Yubel if we need them.
                var targetId = Bot.Hand.Any(c => c != null && c.Id == CardId.SamsaraDLotus) 
                    ? CardId.SpiritOfYubel 
                    : CardId.SamsaraDLotus;
                
                AI.SelectCard(targetId, CardId.SpiritOfYubel, CardId.GruesomeGraveSquirmer, CardId.Yubel);
                DecisionTracer.TraceActivate("ThroneActivate", "Activate Nightmare Throne field spell from hand");
                return true;
            }

            if (Card.Location == CardLocation.SpellZone)
            {
                // Trigger float effect when Yubel leaves field
                AI.SelectCard(CardId.SpiritOfYubel, CardId.SamsaraDLotus, CardId.GruesomeGraveSquirmer, CardId.Yubel);
                DecisionTracer.TraceActivate("ThroneActivate", "Trigger float effect of Nightmare Throne");
                return true;
            }

            return false;
        }

        private bool GatesActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_gatesHandUsed) return false;
                if (ShouldSkipCombo()) return false;
                if (Bot.HasInSpellZone(CardId.OpeningOfTheSpiritGates)) return false;
                return true;
            }

            // GY Revive effect: discard 1 to revive a 0 ATK/DEF Fiend
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup() && Duel.Player == 0)
            {
                if (_gatesFieldUsed) return false;
                bool hasTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Attack == 0 && c.Defense == 0);
                bool hasDiscard = Bot.Hand.Count > 0;
                if (hasTarget && hasDiscard)
                {
                    DecisionTracer.TraceActivate("GatesActivate", "Discard to revive 0 ATK/DEF Fiend");
                    return true;
                }
            }

            return false;
        }

        private bool FusionDeploymentActivate()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;

            // Reveal Elemental HERO Neos Kluger to Special Summon Neos or Yubel from deck/hand
            bool hasTarget = GetRemainingCount(CardId.ElementalHERONeos) > 0 || GetRemainingCount(CardId.Yubel) > 0;
            if (!hasTarget) return false;

            // If going first on Turn 1, only summon Yubel if we can sustain it or float it
            if (Duel.Turn == 1 && Duel.Player == 0)
            {
                bool hasTribute = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Id != CardId.Yubel);
                bool hasThrone = Bot.HasInSpellZone(CardId.NightmareThrone);
                if (!hasTribute && !hasThrone)
                {
                    return false; // Skip to avoid Yubel self-destroying with no benefit
                }
            }

            DecisionTracer.TraceActivate("FusionDeploymentActivate", "SS Neos/Yubel from deck");
            return true;
        }

        // ==========================================
        // YUBEL SYSTEM LOGIC
        // ==========================================

        private bool LotusActivate()
        {
            if (_samsaraUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Main Phase: tribute itself to SS a Yubel monster from deck
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && Duel.Player == 0)
            {
                bool hasTarget = GetRemainingCount(CardId.SpiritOfYubel) > 0 || GetRemainingCount(CardId.Yubel) > 0;
                if (hasTarget)
                {
                    AI.SelectCard(CardId.SpiritOfYubel, CardId.Yubel);
                    _samsaraUsed = true;
                    DecisionTracer.TraceActivate("LotusActivate", "Tribute Lotus to SS Yubel form");
                    return true;
                }
            }

            // End Phase GY self-revive
            if (Card.Location == CardLocation.Grave && Duel.Phase == DuelPhase.End && Duel.Player == 0)
            {
                bool controlsYubel = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id));
                if (controlsYubel)
                {
                    _samsaraUsed = true;
                    DecisionTracer.TraceActivate("LotusActivate", "Revive Lotus from GY in End Phase");
                    return true;
                }
            }

            return false;
        }

        private bool NightmarePainActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_painUsed) return false;
                if (ShouldSkipCombo()) return false;
                if (Bot.HasInSpellZone(CardId.NightmarePain)) return false;
                return true;
            }

            // Field activation: destroy 1 Dark monster in hand/field to search
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup() && Duel.Player == 0)
            {
                if (_painUsed) return false;
                
                var darkMonsters = Bot.Hand.Where(c => c != null && c.HasAttribute(CardAttribute.Dark))
                    .Concat(Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasAttribute(CardAttribute.Dark)))
                    .ToList();
                
                if (darkMonsters.Count > 0)
                {
                    // Pick the best target to destroy: Spirit of Yubel > Squirmer > Samsara > Yubel
                    var bestTarget = darkMonsters.OrderBy(c => 
                        c.Id == CardId.SpiritOfYubel ? 0 :
                        c.Id == CardId.GruesomeGraveSquirmer ? 1 :
                        c.Id == CardId.SamsaraDLotus ? 2 :
                        YubelMonsters.Contains(c.Id) ? 3 : 4
                    ).FirstOrDefault();

                    if (bestTarget != null)
                    {
                        AI.SelectCard(bestTarget);
                    }
                    
                    DecisionTracer.TraceActivate("NightmarePainActivate", "Destroy Dark monster to search Yubel card");
                    return true;
                }
            }

            return false;
        }

        private bool GraveSquirmerActivate()
        {
            if (ShouldSkipCombo()) return false;

            // SS from hand if we control Yubel
            if (Card.Location == CardLocation.Hand)
            {
                if (_squirmerHandUsed) return false;
                bool controlYubel = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id));
                if (controlYubel && Bot.MonsterZone.Count(c => c != null) < 5)
                {
                    DecisionTracer.TraceActivate("GraveSquirmerActivate", "SS Squirmer from hand");
                    return true;
                }
            }

            // Destroy 1 Yubel to revive a 0 ATK/DEF Fiend
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && Duel.Player == 0)
            {
                if (_squirmerFieldUsed) return false;
                var yubels = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id)).ToList();
                bool hasGYTarget = Bot.Graveyard.Any(c => c != null && c.IsMonster() && c.Attack == 0 && c.Defense == 0);
                if (yubels.Count > 0 && hasGYTarget)
                {
                    // Prioritize destroying Spirit of Yubel to float
                    var bestYubel = yubels.OrderBy(c => c.Id == CardId.SpiritOfYubel ? 0 : c.Id == CardId.Yubel ? 1 : 2).FirstOrDefault();
                    if (bestYubel != null) AI.SelectCard(bestYubel);
                    
                    DecisionTracer.TraceActivate("GraveSquirmerActivate", "Destroy Yubel on field to revive 0 ATK/DEF Fiend");
                    return true;
                }
            }

            // GY effect: Banish to SS Yubel from GY/banished
            if (Card.Location == CardLocation.Grave && Duel.Player == 0)
            {
                if (_squirmerGraveUsed) return false;
                bool hasTarget = Bot.Graveyard.Concat(Bot.Banished).Any(c => c != null && YubelMonsters.Contains(c.Id) && c != Card);
                if (hasTarget && !IsSpecialSummonBlocked() && Bot.MonsterZone.Count(c => c != null) < 5)
                {
                    DecisionTracer.TraceActivate("GraveSquirmerActivate", "Banish Squirmer from GY to SS Yubel");
                    return true;
                }
            }

            return false;
        }

        private bool SpiritOfYubelActivate()
        {
            // Bypasses ShouldSkipCombo() because trigger floating is critical to continue the loop when destroyed
            // Quick Effect: SS on opponent attack
            if (Card.Location == CardLocation.Hand && Duel.Player == 1 && Duel.Phase == DuelPhase.Battle)
            {
                DecisionTracer.TraceActivate("SpiritOfYubelActivate", "SS Spirit of Yubel on attack");
                return true;
            }

            // Search/Set Spell/Trap when SS
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                bool hasPain = Bot.HasInSpellZone(CardId.NightmarePain) || Bot.Hand.Any(c => c != null && c.Id == CardId.NightmarePain);
                int targetId = hasPain ? CardId.EternalFavorite : CardId.NightmarePain;
                AI.SelectCard(targetId, CardId.NightmarePain, CardId.EternalFavorite);
                DecisionTracer.TraceActivate("SpiritOfYubelActivate", "Set Nightmare Pain or Eternal Favorite");
                return true;
            }

            // Float when destroyed (GY or Banished)
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                DecisionTracer.TraceActivate("SpiritOfYubelActivate", "Spirit of Yubel destroyed! Float into Yubel");
                return true;
            }

            return false;
        }

        private bool YubelFloatActivate()
        {
            // Bypasses ShouldSkipCombo() because floating is critical to continue the loop when Yubel is destroyed
            // Floating when destroyed: summon Terror Incarnate
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                DecisionTracer.TraceActivate("YubelFloatActivate", "Yubel destroyed! Float into Terror Incarnate");
                return true;
            }
            return false;
        }

        private bool YubelTerrorActivate()
        {
            // Bypasses ShouldSkipCombo() because floating is critical to continue the loop when Terror Incarnate leaves field
            // Floating when leaves field: summon Ultimate Nightmare
            if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                DecisionTracer.TraceActivate("YubelTerrorActivate", "Terror Incarnate left! Float into Ultimate Nightmare");
                return true;
            }
            return false;
        }

        // ==========================================
        // FIENDSMITH ENGINE LOGIC
        // ==========================================

        private bool EngraverActivate()
        {
            if (ShouldSkipCombo()) return false;

            // Discard to search Tract
            if (Card.Location == CardLocation.Hand)
            {
                if (_engraverHandUsed) return false;
                DecisionTracer.TraceActivate("EngraverActivate", "Discard Engraver to search Tract");
                return true;
            }

            // GY Special Summon: send 1 Light Fiend you control to GY
            if (Card.Location == CardLocation.Grave && Duel.Player == 0)
            {
                if (_engraverGraveUsed) return false;
                var lightFiends = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && c.HasRace(CardRace.Fiend) && c.HasAttribute(CardAttribute.Light)).ToList();
                if (lightFiends.Count > 0 && Bot.MonsterZone.Count(c => c != null) < 5)
                {
                    var bestTarget = lightFiends.OrderBy(c => 
                        c.Id == CardId.FabledLurrie ? 0 : 
                        c.Id == CardId.LacrimaTheCrimsonTears ? 1 : 2).FirstOrDefault();
                    if (bestTarget != null) AI.SelectCard(bestTarget);

                    DecisionTracer.TraceActivate("EngraverActivate", "SS Engraver from GY");
                    return true;
                }
            }

            return false;
        }

        private bool TractActivate()
        {
            if (ShouldSkipCombo()) return false;

            // Normal Spell search
            if (Card.Location == CardLocation.Hand)
            {
                if (_tractHandUsed) return false;
                DecisionTracer.TraceActivate("TractActivate", "Search Light Fiend");
                return true;
            }

            // GY fusion: banish to fusion summon
            if (Card.Location == CardLocation.Grave && Duel.Player == 0)
            {
                if (_tractGraveUsed) return false;
                // Banish materials from GY to fuse
                bool canFuse = Bot.Graveyard.Count(c => c != null && c.HasRace(CardRace.Fiend) && c.HasAttribute(CardAttribute.Light)) >= 2;
                if (canFuse && !IsSpecialSummonBlocked())
                {
                    DecisionTracer.TraceActivate("TractActivate", "GY Fusion via Tract");
                    return true;
                }
            }

            return false;
        }

        private bool LacrimaActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                // Send Fiendsmith card from deck to GY
                DecisionTracer.TraceActivate("LacrimaActivate", "Send Fiendsmith card from deck");
                return true;
            }

            // GY Quick Effect: shuffle back to revive Link monster (opponent's turn)
            if (Card.Location == CardLocation.Grave && Duel.Player == 1)
            {
                bool hasLinkTarget = Bot.Graveyard.Any(c => c != null && c.HasType(CardType.Link) && c.Name.Contains("Fiendsmith"));
                if (hasLinkTarget)
                {
                    DecisionTracer.TraceActivate("LacrimaActivate", "Revive Link Fiendsmith");
                    return true;
                }
            }

            return false;
        }

        private bool MutinyActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Shuffle materials from GY to fuse
            bool hasMaterials = Bot.Graveyard.Count(c => c != null && (c.HasRace(CardRace.Fiend) || c.HasRace(CardRace.Fairy))) >= 2;
            if (hasMaterials)
            {
                DecisionTracer.TraceActivate("MutinyActivate", "Shuffle GY to Fusion Summon");
                return true;
            }
            return false;
        }

        private bool FinalBringerActivate()
        {
            // Destroy 1 monster we control and 1 card enemy controls
            var target = Util.GetProblematicEnemyCard();
            if (target != null)
            {
                // Destroy one of our Yubel monsters to trigger float
                var ourTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id));
                if (ourTarget != null)
                {
                    AI.SelectCard(ourTarget);
                    AI.SelectNextCard(target);
                    DecisionTracer.TraceActivate("FinalBringerActivate", $"Destroy our {ourTarget.Name} and enemy {target.Name}");
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        // SUMMON / EXTRA SUMMON CONDITIONS
        // ==========================================

        private bool BeckoningBeastSummon()
        {
            if (_beckoningUsed) return false;
            if (ShouldSkipCombo()) return false;

            // Search Gates on summon
            return true;
        }

        private bool LotusSummon()
        {
            if (_samsaraUsed) return false;
            if (ShouldSkipCombo()) return false;

            return true;
        }

        private bool GenericSummon()
        {
            if (ShouldSkipCombo()) return false;
            return true;
        }

        // ==========================================
        // EXTRA DECK SPECIAL SUMMONS
        // ==========================================

        private bool LovingDefenderSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            // Fuses 1 Yubel + 1+ effect monsters on the field
            bool hasYubelOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && YubelMonsters.Contains(c.Id));
            int enemyMonsters = Enemy.GetMonsters().Count(c => c != null && c.IsFaceup() && !c.HasType(CardType.Normal));

            // Summon to wipe the opponent's field or deal lethal burn
            if (hasYubelOnField && (enemyMonsters >= 2 || CanDealLethal()))
            {
                DecisionTracer.TraceActivate("LovingDefenderSummon", "Contact fuse field into Loving Defender");
                return true;
            }
            return false;
        }

        private bool NeosKlugerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            // Needs Neos + Yubel
            bool hasNeos = Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != null && c.Id == CardId.ElementalHERONeos);
            bool hasYubel = Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != null && c.Id == CardId.Yubel);

            if (hasNeos && hasYubel)
            {
                DecisionTracer.TraceActivate("NeosKlugerSummon", "Fusion Summon Neos Kluger");
                return true;
            }
            return false;
        }

        private bool LuceContactSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            // Requires 3 Fiend/Fairy in GY, shuffles to deck
            int count = Bot.Graveyard.Count(c => c != null && (c.HasRace(CardRace.Fiend) || c.HasRace(CardRace.Fairy)));
            if (count >= 3 && Bot.MonsterZone.Count(c => c != null) < 5)
            {
                DecisionTracer.TraceActivate("LuceContactSummon", "Contact summon Luce by shuffling GY");
                return true;
            }
            return false;
        }

        private bool DesiraeSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            return true; // Always summon boss if possible
        }

        private bool LacrimaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // Extender: make sure we have a target to revive/add in GY
            bool hasGYTarget = Bot.Graveyard.Any(c => c != null && (c.Id == CardId.FiendsmithEngraver || c.Id == CardId.FiendsmithsTract || c.Id == CardId.LacrimaTheCrimsonTears));
            return hasGYTarget;
        }

        private bool AerialEaterSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // Extender: make sure we have a Fiend target in Deck to dump
            bool hasDeckTarget = Bot.Deck.Any(c => c != null && (c.Id == CardId.GruesomeGraveSquirmer || c.Id == CardId.SamsaraDLotus || c.Id == CardId.Yubel));
            return hasDeckTarget;
        }

        private bool DukeOfDemiseSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            // We only summon Duke of Demise if we need to dump Fiends and cannot make Aerial Eater or Lacrima
            bool canSummonBetter = (Bot.ExtraDeck.Any(c => c != null && c.Id == CardId.FiendsmithsLacrima && !c.IsDisabled()) && LacrimaSummon()) ||
                                  (Bot.ExtraDeck.Any(c => c != null && c.Id == CardId.AerialEater && !c.IsDisabled()) && AerialEaterSummon());
            return !canSummonBetter;
        }

        private bool ChaosAngelSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            // Get all face-up LIGHT/DARK monsters we control
            var monsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && (c.HasAttribute(CardAttribute.Light) || c.HasAttribute(CardAttribute.Dark))).ToList();
            if (monsters.Count < 2) return false;

            // Avoid using Spirit of Yubel or Yubel if possible
            var safeMonsters = monsters.Where(c => c.Id != CardId.SpiritOfYubel && c.Id != CardId.Yubel).ToList();

            bool canSynchro = false;

            // 1. Pair combinations (Safe only)
            for (int i = 0; i < safeMonsters.Count; i++)
            {
                for (int j = i + 1; j < safeMonsters.Count; j++)
                {
                    if (safeMonsters[i].Level + safeMonsters[j].Level == 10)
                    {
                        AI.SelectCard(new[] { safeMonsters[i], safeMonsters[j] });
                        canSynchro = true;
                        break;
                    }
                }
                if (canSynchro) break;
            }

            if (canSynchro)
            {
                DecisionTracer.TraceActivate("ChaosAngelSummon", "Synchro Summon Chaos Angel");
                return true;
            }

            return false;
        }

        private bool XyzRank10Summon()
        {
            if (IsSpecialSummonBlocked()) return false;

            // Rank 10: 2 Level 10 monsters (Yubel, Spirit of Yubel, Chaos Angel level 10)
            int level10Count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 10);
            if (level10Count >= 2)
            {
                // ONLY summon if we can deal lethal or win!
                if (CanDealLethal() || Enemy.LifePoints <= 2000)
                {
                    DecisionTracer.TraceActivate("XyzRank10Summon", "Xyz Summon Rank 10 Boss for lethal");
                    return true;
                }
            }
            return false;
        }

        // ==========================================
        // CARD SELECTION LOGIC (OnSelectCard)
        // ==========================================

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
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // 1. Material selection priority (Fusion, Link, Synchro, Xyz)
            if (hint == 511 || hint == 513 || hint == 533 || hint == 502 || hint == 504)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            // 2. Search from Deck (any selection where candidate is in Deck)
            if (cards.Any(c => c != null && c.Location == CardLocation.Deck))
            {
                // Custom check for Fiendsmith's Tract search
                if (Card != null && Card.Id == CardId.FiendsmithsTract)
                {
                    bool handHasLurrie = Bot.Hand.Any(c => c != null && c.Id == CardId.FabledLurrie);
                    if (!handHasLurrie)
                    {
                        var lurrie = cards.FirstOrDefault(c => c != null && c.Id == CardId.FabledLurrie);
                        if (lurrie != null) return new List<ClientCard> { lurrie };
                    }
                }

                var preferred = new List<int>();
                bool hasLotus = Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != null && c.Id == CardId.SamsaraDLotus);
                bool hasBeckoning = Bot.Hand.Concat(Bot.GetMonsters()).Any(c => c != null && c.Id == CardId.DarkBeckoningBeast);
                bool hasPain = Bot.Hand.Concat(Bot.GetSpells()).Any(c => c != null && c.Id == CardId.NightmarePain);

                if (!hasLotus) preferred.Add(CardId.SamsaraDLotus);
                if (!hasBeckoning) preferred.Add(CardId.DarkBeckoningBeast);
                preferred.Add(CardId.SpiritOfYubel);
                preferred.Add(CardId.GruesomeGraveSquirmer);
                preferred.Add(CardId.Yubel);
                preferred.Add(CardId.FiendsmithEngraver);
                preferred.Add(CardId.OpeningOfTheSpiritGates);
                preferred.Add(CardId.NightmarePain);
                preferred.Add(CardId.FabledLurrie);

                var result = new List<ClientCard>();
                foreach (int id in preferred)
                {
                    var matches = cards.Where(c => c != null && c.Id == id && !result.Contains(c)).ToList();
                    foreach (var m in matches)
                    {
                        result.Add(m);
                        if (result.Count >= max) return result;
                    }
                }

                // Fallback: select whatever is NOT Terror Incarnate/Ultimate Nightmare/Neos first (prevent hand clogging)
                var rest = cards.Where(c => c != null && !result.Contains(c))
                    .OrderBy(c => c.Id == CardId.YubelTerrorIncarnate || c.Id == CardId.YubelTheUltimateNightmare || c.Id == CardId.ElementalHERONeos ? 1 : 0)
                    .ToList();
                foreach (var r in rest)
                {
                    result.Add(r);
                    if (result.Count >= max) return result;
                }
                return result.Take(max).ToList();
            }

            // 3. Selection for Destruction (from hand/field)
            if (Card != null && Card.Id == CardId.NightmarePain)
            {
                var targets = cards.Where(c => c != null && (c.Location == CardLocation.Hand || c.Location == CardLocation.MonsterZone))
                    .OrderBy(c => c.Id == CardId.SpiritOfYubel ? 0 :
                             c.Id == CardId.Yubel ? 1 :
                             c.Id == CardId.SamsaraDLotus ? 2 : 3)
                    .ToList();
                if (targets.Count > 0)
                {
                    return targets.Take(max).ToList();
                }
            }

            // 4. Discard cost (Opening of the Spirit Gates or Fiendsmith's Tract)
            if (cards.Any(c => c != null && c.Location == CardLocation.Hand))
            {
                var lurrie = cards.FirstOrDefault(c => c != null && c.Id == CardId.FabledLurrie);
                if (lurrie != null) return new List<ClientCard> { lurrie };

                var redundantYubels = cards.Where(c => c != null && YubelMonsters.Contains(c.Id)).ToList();
                if (redundantYubels.Count > 1) return new List<ClientCard> { redundantYubels[0] };

                var neos = cards.FirstOrDefault(c => c != null && c.Id == CardId.ElementalHERONeos);
                if (neos != null) return new List<ClientCard> { neos };

                // Avoid discarding Hand Traps! Filter them out if possible.
                var safeToDiscard = cards.Where(c => c != null && 
                    c.Id != CardId.AshBlossom && c.Id != CardId.MaxxC && 
                    c.Id != CardId.EffectVeiler && c.Id != CardId.CalledByTheGrave).ToList();
                
                if (safeToDiscard.Count > 0)
                {
                    var sorted = safeToDiscard.OrderBy(c => GetMaterialPriority(c)).ToList();
                    return sorted.Take(max).ToList();
                }

                var sortedFallback = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sortedFallback.Take(max).ToList();
            }

            // 5. Special Poly or Loving Defender target selections
            if (Card != null && (Card.Id == CardId.SuperPolymerization || Card.Id == CardId.YubelTheLovingDefenderForever))
            {
                var sorted = cards.OrderByDescending(c => c.Controller == 1 ? 100000 + c.Attack : c.Attack).ToList();
                return sorted.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Yubel forms should be in attack position to maximize damage reflection
            if (YubelMonsters.Contains(cardId))
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }
            // Fiendsmith or normal monsters in attack
            if (cardId == CardId.FiendsmithEngraver || cardId == CardId.LacrimaTheCrimsonTears)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        // ==========================================
        // MONSTER REPOSITIONING LOGIC (MonsterRepos)
        // ==========================================

        private bool MonsterRepos()
        {
            if (Card == null) return false;

            // Yubel forms repositioning
            if (YubelMonsters.Contains(Card.Id))
            {
                // If opponent has face-up monsters with ATK, we want to be in Attack position to declare attacks and reflect damage.
                // Otherwise, if opponent board has no monsters, keep/change to Defense position for safety.
                bool hasTarget = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack > 0);
                if (hasTarget)
                {
                    if (Card.IsDefense()) return true; // Change to Attack
                    return false;
                }
                else
                {
                    if (Card.IsAttack()) return true; // Change to Defense
                    return false;
                }
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

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (attacker != null && YubelMonsters.Contains(attacker.Id))
            {
                // Yubel wants to crash into the highest ATK target!
                var bestTarget = defenders.Where(d => d != null && d.IsFaceup() && d.IsAttack())
                                          .OrderByDescending(d => d.Attack)
                                          .FirstOrDefault();
                if (bestTarget != null && bestTarget.Attack >= 0)
                {
                    return AI.Attack(attacker, bestTarget);
                }
            }
            return base.OnSelectAttackTarget(attacker, defenders);
        }

        private bool IsSafeToAttack(ClientCard attacker)
        {
            if (YubelMonsters.Contains(attacker.Id)) return true;

            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.IsAttack() && enemy.Attack > attacker.Attack) return false;
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
    }
}
