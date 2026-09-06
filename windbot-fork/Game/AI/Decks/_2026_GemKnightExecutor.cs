// ============================================================
// CARD AUDIT โ€” 2026 Gem-Knight
// ============================================================
// | Card Name              | Type       | OPT? | Cost        | Effect Summary                         | Activate When                     | NEVER When                           |
// |------------------------|------------|------|-------------|----------------------------------------|-----------------------------------|--------------------------------------|
// | Gem-Knight Garnet      | Monster    | No   | None        | Garnet in deck = Brilliant Fusion cost | Normal summon beatstick           | -                                    |
// | Gem-Knight Lazuli      | Monster    | HOPT| None        | Recovers Gem from GY on Normal Summon  | Normal summoned from hand         | GY has no Gem target                 |
// | Gem-Knight Obsidian    | Monster    | HOPT| None        | Searches Gem on Normal Summon          | Normal summoned from hand         | No target in deck                    |
// | Gem-Knight Alexandrite | Monster    | No   | None        | Normal summon without tribute          | Need normal summon                 | Already have board                   |
// | Gem-Knight Crystal     | Monster    | No   | None        | Protects Gem Fusion spell from negate  | Activating Gem-Knight Fusion      | Not in GY                            |
// | Crystal Rose           | Monster    | No   | None        | SS from hand, dump Gem from deck       | Need fusion setup                  | SS blocked, no target in deck        |
// | Gem-Armadillo          | Monster    | HOPT| None        | Search Gem-Knight monster              | Need search                        | Already searched                     |
// | Brilliant Fusion       | Spell      | HOPT| Pay 2000 LP  | Fusion from deck (send materials)      | Turn 1 setup, need Garnet in deck  | LP <= 2000, already used             |
// | Gem-Knight Fusion      | Spell      | No   | None        | Fusion from hand/field                 | Have materials on field/hand       | No valid fusion target               |
// | Absorb Fusion          | Spell      | No   | None        | Fusion using GY materials              | GY has materials                   | No target                            |
// | Scatter Fusion         | Trap       | HOPT| Set 1 turn  | Fusion during opponent's turn          | Opponent summons multiple          | Not set, no target                   |
// | Gem-Knight Seraphinite | Fusion     | -    | -           | Extra Normal Summon                    | Need 2nd normal summon             | Already have enough                  |
// | Gem-Knight Master Diamond| Fusion   | -    | -           | Pop + burn + protection                | End board boss                     | -                                    |
// | Gem-Knight Phantom Quartz| Fusion  | -    | -           | SS Gem from deck/GY on summon          | Link material maker                | SS blocked                           |
// | Gem-Knight Lady Lapis  | Fusion     | -    | -           | Burn damage (FTK enabler)              | Going 2nd OTK                      | Turn 1 setup                         |
// ============================================================
// ACE CARDS:
//   Primary: Gem-Knight Master Diamond (02444655)
//   Secondary: Gem-Knight Seraphinite (15083648)
// COMBO STARTERS:
//   1. Brilliant Fusion (06346825) โ€” send Garnet + Lazuli โ’ SS Seraphinite
//   2. Crystal Rose (94814324) โ€” SS + dump Gem from deck
// WIN CONDITION: Fusion beatdown + burn
// GOING 1ST END BOARD: Seraphinite + Phantom Quartz + set backrow
// GOING 2ND GAMEPLAN: Brilliant Fusion โ’ OTK with Lady Lapis burn
// CHOKEPOINTS: Brilliant Fusion negated = combo dead
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
    [Deck("2026_GemKnight", "2026_GemKnight")]
    public class _2026_GemKnightExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int Garnet = 91731841;
            public const int Lazuli = 81846636;
            public const int Obsidian = 19163116;
            public const int Alexandrite = 90019393;
            public const int Crystal = 76908448;
            public const int CrystalRose = 79531196;
            public const int GemArmadillo = 27004302;
            public const int Quartz = 35622739;

            // Spells
            public const int GemKnightFusion = 1264319;
            public const int BrilliantFusion = 7394770;
            public const int AbsorbFusion = 71422989;
            public const int ScatterFusion = 40597694;
            public const int GemKnightDispersion = 24220368;

            // Extra Deck
            public const int MasterDiamond = 39512984;
            public const int LadyBrilliantDiamond = 19355597;
            public const int Seraphinite = 3113836;
            public const int LadyLapisLazuli = 47611119;
            public const int PhantomQuartz = 24484270;
            public const int Zirconia = 8692301;
            public const int LadyRoseDiamond = 55610199;

            // Staples
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int EffectVeiler = 97268402;
            public const int CalledByTheGrave = 24224830;
            public const int SuperPolymerization = 48130397;
            public const int BookOfMoon = 14087893;
            public const int Terraforming = 73628505;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int Mudragon = 54757758;
            public const int Garura = 11765832;
            public const int PredaplantVerteAnaconda = 70369116;
            public const int SPLittleKnight = 29301450;
            public const int BorreloadDragon = 31833038;
            public const int AccesscodeTalker = 86066372;
        }

        private static readonly int[] AceCardIds = {
            CardId.MasterDiamond, CardId.Seraphinite, CardId.LadyBrilliantDiamond
        };

        private static readonly int[] GemKnightMonsters = {
            CardId.Garnet, CardId.Lazuli, CardId.Obsidian,
            CardId.Alexandrite, CardId.Crystal, CardId.Quartz
        };

        // OPT Flags
        private bool _lazuliUsed = false;
        private bool _obsidianUsed = false;
        private bool _armadilloUsed = false;
        private bool _brilliantFusionUsed = false;
        private bool _scatterFusionUsed = false;
        private bool _crystalRoseUsed = false;

        public _2026_GemKnightExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // Register Ace Cards to protect them from being used suboptimally
            ResourcePlan.RegisterAceCards(AceCardIds);

            // โ”€โ”€ Combo Router โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Brilliant-Fusion",
                RequiredCards = new List<int> { CardId.BrilliantFusion, CardId.Garnet },
                FallbackLineName = "Absorb-Fusion-Fallback",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.BrilliantFusion, ActionType = ExecutorType.Activate, Description = "Activate Brilliant Fusion" },
                    new() { CardId = CardId.CrystalRose, ActionType = ExecutorType.Activate, Description = "SS Crystal Rose from hand" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Absorb-Fusion-Fallback",
                RequiredCards = new List<int> { CardId.AbsorbFusion },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.AbsorbFusion, ActionType = ExecutorType.Activate, Description = "Activate Absorb Fusion" }
                },
                EndBoardScore = 65,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.AbsorbFusion)
            });

            BaitPlanner.RegisterComboStarters(CardId.BrilliantFusion, CardId.CrystalRose);
            BaitPlanner.RegisterBaitCards(CardId.BookOfMoon);
            ChainAdvisor.RegisterHighValueTargets(CardId.BrilliantFusion, CardId.GemKnightFusion);

            // TIER 1: Hand Traps
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, VeilerActivate);

            // TIER 2: Protection / Quick Effects
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);

            // TIER 3: Setup / Search / Resource Generators
            AddExecutor(ExecutorType.Activate, CardId.BrilliantFusion, BrilliantFusionActivate);
            AddExecutor(ExecutorType.Activate, CardId.Quartz, QuartzActivate);
            AddExecutor(ExecutorType.Activate, CardId.GemKnightDispersion, DispersionActivate);
            AddExecutor(ExecutorType.Activate, CardId.AbsorbFusion, AbsorbFusionActivate);
            AddExecutor(ExecutorType.Activate, CardId.ScatterFusion, ScatterFusionActivate);
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingActivate);
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, ROTAActivate);
            AddExecutor(ExecutorType.Activate, CardId.GemArmadillo, ArmadilloSearch);
            AddExecutor(ExecutorType.Activate, CardId.CrystalRose, CrystalRoseEffect);

            // TIER 4: Fusion Spells (Resource Consumers)
            AddExecutor(ExecutorType.Activate, CardId.GemKnightFusion, GemKnightFusionActivate);

            // TIER 5: Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.GemArmadillo, GenericSummon);
            AddExecutor(ExecutorType.Summon, CardId.Lazuli, LazuliSummon);
            AddExecutor(ExecutorType.Summon, CardId.Obsidian, ObsidianSummon);
            AddExecutor(ExecutorType.Summon, CardId.Alexandrite, GenericSummon);
            AddExecutor(ExecutorType.Summon, CardId.Garnet, GenericSummon);
            AddExecutor(ExecutorType.Summon, CardId.Quartz, GenericSummon);

            // TIER 6: Monster Effects on Field
            AddExecutor(ExecutorType.Activate, CardId.Lazuli, LazuliEffect);
            AddExecutor(ExecutorType.Activate, CardId.Obsidian, ObsidianEffect);

            // TIER 7: Extra Deck
            AddExecutor(ExecutorType.SpSummon, CardId.PhantomQuartz, PhantomQuartzSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Seraphinite, FusionSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.MasterDiamond, FusionSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.LadyBrilliantDiamond, FusionSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.LadyLapisLazuli, FusionSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.Zirconia, FusionSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.PredaplantVerteAnaconda, VerteSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPSummon);

            // TIER 8: Sets
            AddExecutor(ExecutorType.SpellSet, CardId.ScatterFusion, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.BookOfMoon, () => Util.IsTurn1OrMain2());

            // LAST: Repos
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand() => true; // Go first for Fusion setup

        private bool OpponentHasActiveNegator()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 2500 || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            // Extra Deck summons (excluding the Ace card itself on its initial summon)
            // If the summon would consume any face-up Ace card currently on the field as material, evaluate it.
            var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
            if (activeAces.Count > 0)
            {
                // Fusion / Link summons that require materials from field
                // Check if we have enough non-Ace monsters to use as material instead.
                int reqCount = card.HasType(CardType.Link) ? card.LinkCount : 2;
                var nonAceMats = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsAceCard(c)).ToList();
                if (nonAceMats.Count < reqCount)
                {
                    bool allowed = false;
                    foreach (var mat in activeAces)
                    {
                        var res = ResourcePlan.EvaluateAceUsage(
                            card: mat,
                            hasLethalIfUsed: CanDealLethal(),
                            isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                            haveAlternateWinCon: false
                        );
                        if (res.allowed)
                        {
                            allowed = true;
                            break;
                        }
                    }
                    if (!allowed)
                    {
                        DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} is not safe (would consume Ace card(s) as material)");
                        return false;
                    }
                }
            }

            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _lazuliUsed = false;
            _obsidianUsed = false;
            _armadilloUsed = false;
            _brilliantFusionUsed = false;
            _scatterFusionUsed = false;
            _crystalRoseUsed = false;

            // โ”€โ”€ Going-Second BreakBoard: reset Brilliant Fusion for OTK pushes โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                _brilliantFusionUsed = false; // Allow Brilliant Fusion for turn-2 OTK
                _scatterFusionUsed = false;   // Allow quick fusion during opponent's turn response
            }
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.Lazuli) _lazuliUsed = true;
                if (card.Id == CardId.Obsidian) _obsidianUsed = true;
                if (card.Id == CardId.GemArmadillo) _armadilloUsed = true;
                if (card.Id == CardId.BrilliantFusion) _brilliantFusionUsed = true;
                if (card.Id == CardId.ScatterFusion) _scatterFusionUsed = true;
                if (card.Id == CardId.CrystalRose) _crystalRoseUsed = true;
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
            if (c.IsCode(CardId.AshBlossom, CardId.MaxxC, CardId.EffectVeiler))
                return 800;
            if (c.IsCode(CardId.Garnet)) return 700; // Must keep Garnet for Brilliant Fusion
            if (GemKnightMonsters.Contains(c.Id)) return 600;
            return 100;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.MasterDiamond)) return true;
            if (Bot.HasInMonstersZone(CardId.LadyBrilliantDiamond)) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        // โ•โ•โ• Hand Traps โ•โ•โ•

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
            if (Duel.Player != 1) return false;
            return true;
        }

        private bool VeilerActivate()
        {
            if (!SmartHandTrapChain()) return false;
            return DefaultEffectVeiler();
        }

        // โ•โ•โ• Fusion Spells โ•โ•โ•

        private bool BrilliantFusionActivate()
        {
            if (_brilliantFusionUsed) return false;
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.Hand) return false;

            // Must have Garnet in deck
            if (GetRemainingCount(CardId.Garnet) == 0) return false;

            // Prefer Seraphinite as target for extra normal summon
            bool hasSeraphiniteInExtra = GetRemainingCount(CardId.Seraphinite) > 0;
            if (hasSeraphiniteInExtra)
                AI.SelectCard(new[] { CardId.Seraphinite, CardId.MasterDiamond, CardId.LadyBrilliantDiamond });
            else
                AI.SelectCard(new[] { CardId.MasterDiamond, CardId.LadyBrilliantDiamond });

            _brilliantFusionUsed = true;
            DecisionTracer.TraceActivate("BrilliantFusionActivate", "Activate Brilliant Fusion");
            return true;
        }

        private bool GemKnightFusionActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.Hand) return false;

            // Check if we have materials
            bool hasMaterials = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.IsMonster()) ||
                               Bot.Hand.Any(c => c != null && c.IsMonster() && c.Id != CardId.GemKnightFusion);
            if (!hasMaterials) return false;

            // Smart fusion target selection
            int target = GetBestFusionTarget();
            if (target > 0)
            {
                AI.SelectCard(target);
                return true;
            }

            return false;
        }

        private int GetBestFusionTarget()
        {
            // OTK mode: Lady Lapis for burn
            if (_isGoingSecond && CanDealLethal())
                return CardId.LadyLapisLazuli;

            // Need Master Diamond as boss
            if (!Bot.HasInMonstersZone(CardId.MasterDiamond))
                return CardId.MasterDiamond;

            // Need Seraphinite for extra normal
            if (!Bot.HasInMonstersZone(CardId.Seraphinite) && !_brilliantFusionUsed)
                return CardId.Seraphinite;

            // Default
            return CardId.LadyBrilliantDiamond;
        }

        private bool AbsorbFusionActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Search first
            if (GetRemainingCount(CardId.CrystalRose) > 0)
                AI.SelectCard(CardId.CrystalRose);
            else if (GetRemainingCount(CardId.Obsidian) > 0)
                AI.SelectCard(CardId.Obsidian);
            else if (GetRemainingCount(CardId.Quartz) > 0)
                AI.SelectCard(CardId.Quartz);
            else if (GetRemainingCount(CardId.Lazuli) > 0)
                AI.SelectCard(CardId.Lazuli);
            else
                return false;

            // Then optional fusion summon
            int target = GetBestFusionTarget();
            if (target > 0)
            {
                AI.SelectNextCard(target);
            }

            DecisionTracer.TraceActivate("AbsorbFusion", "Activate Absorb Fusion from Hand");
            return true;
        }

        private bool ScatterFusionActivate()
        {
            if (_scatterFusionUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.SpellZone || Card.IsFacedown()) return false;

            // Can be activated during our turn's Main Phase 1 or 2
            if (Duel.Player != 0) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;

            // Must have materials in deck
            if (GetRemainingCount(CardId.Garnet) == 0 && GetRemainingCount(CardId.Lazuli) == 0)
                return false;

            _scatterFusionUsed = true;
            int target = GetBestFusionTarget();
            if (target > 0)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("ScatterFusion", "Activate Scatter Fusion on field");
                return true;
            }
            return false;
        }

        // โ•โ•โ• Search/Setup โ•โ•โ•

        private bool TerraformingActivate()
        {
            if (ShouldSkipCombo()) return false;
            return !Bot.HasInHand(CardId.BrilliantFusion) && GetRemainingCount(CardId.BrilliantFusion) > 0;
        }

        private bool ROTAActivate()
        {
            if (ShouldSkipCombo()) return false;
            return !Bot.HasInHand(CardId.GemArmadillo) && GetRemainingCount(CardId.GemArmadillo) > 0;
        }

        private bool ArmadilloSearch()
        {
            if (_armadilloUsed) return false;
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.MonsterZone) return false;

            _armadilloUsed = true;
            // Search Quartz > Obsidian > Lazuli
            if (GetRemainingCount(CardId.Quartz) > 0)
                AI.SelectCard(CardId.Quartz);
            else if (GetRemainingCount(CardId.Obsidian) > 0)
                AI.SelectCard(CardId.Obsidian);
            else
                AI.SelectCard(CardId.Lazuli);

            DecisionTracer.TraceActivate("ArmadilloSearch", "Search Gem-Knight monster");
            return true;
        }

        private bool CrystalRoseEffect()
        {
            if (ShouldSkipCombo()) return false;

            // 1. Hand Activation: Special Summon itself
            if (Card.Location == CardLocation.Hand)
            {
                if (_crystalRoseUsed) return false;
                if (IsSpecialSummonBlocked()) return false;

                // Must have Gem monster in deck to dump
                if (GetRemainingCount(CardId.Garnet) == 0 && GetRemainingCount(CardId.Lazuli) == 0)
                    return false;

                if (GetRemainingCount(CardId.Garnet) > 0)
                    AI.SelectCard(CardId.Garnet);
                else
                    AI.SelectCard(CardId.Lazuli);

                _crystalRoseUsed = true;
                DecisionTracer.TraceActivate("CrystalRose", "SS Crystal Rose from Hand");
                return true;
            }

            // 2. Field Activation: Copy name/material by dumping a Gem-Knight from deck/hand
            if (Card.Location == CardLocation.MonsterZone)
            {
                bool hasFusionsInExtra = Bot.ExtraDeck.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Fusion))
                    || GetRemainingCount(CardId.Seraphinite) > 0
                    || GetRemainingCount(CardId.MasterDiamond) > 0
                    || GetRemainingCount(CardId.LadyBrilliantDiamond) > 0;
                if (hasFusionsInExtra)
                {
                    if (GetRemainingCount(CardId.Obsidian) > 0)
                        AI.SelectCard(CardId.Obsidian);
                    else if (GetRemainingCount(CardId.Lazuli) > 0)
                        AI.SelectCard(CardId.Lazuli);
                    else if (GetRemainingCount(CardId.Garnet) > 0)
                        AI.SelectCard(CardId.Garnet);
                    else
                        return false;

                    DecisionTracer.TraceActivate("CrystalRose", "Activate Crystal Rose on field to copy material");
                    return true;
                }
                return false;
            }

            // 3. GY Activation: Banish a Fusion monster from GY to Special Summon itself
            if (Card.Location == CardLocation.Grave)
            {
                if (IsSpecialSummonBlocked()) return false;
                var fusionsInGY = Bot.Graveyard.Where(c => c != null && c.HasType(CardType.Fusion)).ToList();
                if (fusionsInGY.Count > 0)
                {
                    var target = fusionsInGY.OrderBy(c => {
                        if (c.Id == CardId.Zirconia) return 1;
                        if (c.Id == CardId.LadyLapisLazuli) return 2;
                        if (c.Id == CardId.Seraphinite) return 3;
                        return 10;
                    }).First();

                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("CrystalRose", "SS Crystal Rose from GY");
                    return true;
                }
            }

            return false;
        }

        private bool DispersionActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (ShouldSkipCombo()) return false;
                if (IsSpecialSummonBlocked()) return false;

                bool gkFusionInGY = Bot.Graveyard.Any(c => c != null && c.Id == CardId.GemKnightFusion);
                if (gkFusionInGY)
                {
                    AI.SelectOption(0); // Fusion Summon
                    AI.SelectCard(new[] { CardId.LadyBrilliantDiamond, CardId.MasterDiamond, CardId.Seraphinite, CardId.Zirconia });
                    DecisionTracer.TraceActivate("Dispersion", "Activate Dispersion from Hand (Fusion Summon)");
                    return true;
                }
                else
                {
                    AI.SelectOption(1); // Search
                    if (GetRemainingCount(CardId.Quartz) > 0)
                        AI.SelectCard(CardId.Quartz);
                    else if (GetRemainingCount(CardId.Obsidian) > 0)
                        AI.SelectCard(CardId.Obsidian);
                    else
                        AI.SelectCard(CardId.Lazuli);

                    DecisionTracer.TraceActivate("Dispersion", "Activate Dispersion from Hand (Search)");
                    return true;
                }
            }

            if (Card.Location == CardLocation.Grave)
            {
                bool hasBanished = Bot.Banished.Any(c => c != null && GemKnightMonsters.Contains(c.Id));
                if (hasBanished)
                {
                    var target = Bot.Banished.FirstOrDefault(c => c != null && GemKnightMonsters.Contains(c.Id));
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool QuartzActivate()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (ShouldSkipCombo()) return false;

            bool hasTarget = GetRemainingCount(CardId.BrilliantFusion) > 0 || GetRemainingCount(CardId.ScatterFusion) > 0;
            if (hasTarget)
            {
                if (GetRemainingCount(CardId.BrilliantFusion) > 0)
                    AI.SelectCard(CardId.BrilliantFusion);
                else
                    AI.SelectCard(CardId.ScatterFusion);

                DecisionTracer.TraceActivate("Quartz", "Discard Quartz to search Fusion spell");
                return true;
            }
            return false;
        }

        // โ•โ•โ• Normal Summons โ•โ•โ•

        private bool GenericSummon()
        {
            if (ShouldSkipCombo()) return false;
            return true;
        }

        private bool LazuliSummon()
        {
            if (ShouldSkipCombo()) return false;
            // Only summon Lazuli if we have a GY target to recover
            return Bot.Graveyard.Any(c => c != null && GemKnightMonsters.Contains(c.Id) && c.Id != CardId.Lazuli);
        }

        private bool ObsidianSummon()
        {
            if (ShouldSkipCombo()) return false;
            // Only summon Obsidian if we have a search target in deck
            return GetRemainingCount(CardId.Garnet) > 0 || GetRemainingCount(CardId.Lazuli) > 0;
        }

        // โ•โ•โ• Monster Effects โ•โ•โ•

        private bool LazuliEffect()
        {
            if (_lazuliUsed) return false;
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            return Bot.Graveyard.Any(c => c != null && GemKnightMonsters.Contains(c.Id) && c.Id != CardId.Lazuli);
        }

        private bool ObsidianEffect()
        {
            if (_obsidianUsed) return false;
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            return GetRemainingCount(CardId.Garnet) > 0 || GetRemainingCount(CardId.Lazuli) > 0;
        }

        // โ•โ•โ• Extra Deck โ•โ•โ•

        private bool FusionSummonCondition()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool PhantomQuartzSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Phantom Quartz SS Gem from deck on summon โ€” useful for Link plays
            bool hasTargetInDeck = GetRemainingCount(CardId.Garnet) > 0 || GetRemainingCount(CardId.Lazuli) > 0;
            return hasTargetInDeck;
        }

        private bool VerteSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;

            // Only summon Verte if we can use it for fusion
            bool hasFusionSpell = Bot.HasInHand(CardId.GemKnightFusion) ||
                                  Bot.HasInHand(CardId.BrilliantFusion) ||
                                  Bot.HasInHand(CardId.AbsorbFusion) ||
                                  Bot.Graveyard.Any(c => c != null &&
                                      (c.Id == CardId.GemKnightFusion || c.Id == CardId.BrilliantFusion));
            // Verte takes 2 materials
            int faceup = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());
            return faceup >= 2 && hasFusionSpell;
        }

        private bool SPSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            if (ShouldSkipCombo()) return false;

            // Only use SP to remove threats
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500);
        }

        // โ•โ•โ• OnSelectCard โ•โ•โ•

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
            if (hint == 511 || hint == 533 || hint == 502 || hint == 504)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            if (hint == 506) // Search
            {
                if (!Bot.HasInHand(CardId.BrilliantFusion) && cards.Any(c => c != null && c.Id == CardId.BrilliantFusion))
                    return new List<ClientCard> { cards.First(c => c.Id == CardId.BrilliantFusion) };
                return SelectPreferred(cards, min, max,
                    CardId.Garnet, CardId.Lazuli, CardId.Obsidian, CardId.CrystalRose);
            }

            if (Card == null) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Brilliant Fusion: select materials
            if (Card.Id == CardId.BrilliantFusion)
            {
                var garnet = cards.FirstOrDefault(c => c != null && c.Id == CardId.Garnet);
                if (garnet != null) return new List<ClientCard> { garnet };
                return SelectPreferred(cards, min, max, CardId.Garnet, CardId.Lazuli);
            }

            // Lady Lapis Lazuli effect: send 1 Gem-Knight from deck/Extra to GY
            if (Card.Id == CardId.LadyLapisLazuli || Card.Id == CardId.MasterDiamond)
            {
                // If it is the burn effect sending target
                if (cards.Any(c => c != null && (c.Location == CardLocation.Deck || c.Location == CardLocation.Extra)))
                {
                    return SelectPreferred(cards, min, max, CardId.Lazuli, CardId.Obsidian, CardId.Garnet);
                }
                
                // If Master Diamond is selecting a fusion monster from GY to banish/copy
                if (Card.Id == CardId.MasterDiamond && cards.Any(c => c != null && c.Location == CardLocation.Grave && c.HasType(CardType.Fusion)))
                {
                    var lapis = cards.FirstOrDefault(c => c != null && c.Id == CardId.LadyLapisLazuli && c.Location == CardLocation.Grave);
                    if (lapis != null) return new List<ClientCard> { lapis };
                }
            }

            // Lady Brilliant Diamond effect: select Fusion monster from Extra Deck to Special Summon
            if (Card.Id == CardId.LadyBrilliantDiamond)
            {
                var lapis = cards.FirstOrDefault(c => c != null && c.Id == CardId.LadyLapisLazuli && c.Location == CardLocation.Extra);
                if (lapis != null) return new List<ClientCard> { lapis };
            }

            // Obsidian / Lazuli GY effects: select Normal monster (Garnet) to revive/add to hand
            if (Card.Id == CardId.Obsidian || Card.Id == CardId.Lazuli)
            {
                var garnet = cards.FirstOrDefault(c => c != null && c.Id == CardId.Garnet && c.Location == CardLocation.Grave);
                if (garnet != null) return new List<ClientCard> { garnet };
            }

            // Crystal Rose GY/Field effects
            if (Card.Id == CardId.CrystalRose)
            {
                if (Card.Location == CardLocation.Grave)
                {
                    // GY effect: banish 1 fusion monster to SS itself (avoid banishing Lady Lapis Lazuli if possible)
                    return SelectPreferred(cards, min, max, CardId.Seraphinite, CardId.Zirconia, CardId.LadyRoseDiamond, CardId.LadyLapisLazuli);
                }
                else
                {
                    // Field effect: send 1 Gem-Knight from deck/hand to copy name
                    return SelectPreferred(cards, min, max, CardId.Lazuli, CardId.Obsidian, CardId.Quartz);
                }
            }

            // Gem-Knight Fusion GY recycle effect: banish 1 Gem-Knight from GY to add to hand
            if (Card.Id == CardId.GemKnightFusion && Card.Location == CardLocation.Grave)
            {
                // Prefer banishing non-critical cards in GY (avoid Garnet and Lady Lapis Lazuli)
                var targets = cards.Where(c => c != null && c.Id != CardId.LadyLapisLazuli && c.Id != CardId.Garnet)
                    .OrderBy(c => GetMaterialPriority(c)).ToList();
                if (targets.Count >= min) return targets.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (AceCardIds.Contains(cardId) || cardId == CardId.Garnet)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }
            if (cardId == CardId.Lazuli || cardId == CardId.Quartz)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }
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
