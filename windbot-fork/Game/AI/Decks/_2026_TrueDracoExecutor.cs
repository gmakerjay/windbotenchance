using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_TrueDraco", "2026_TrueDraco")]
    public class _2026_TrueDracoExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int MasterPeaceTheTrueDracoslayingKing = 21377582;
            public const int DinomightKnightTheTrueDracofighter = 58984738;
            public const int IgnisHeatTheTrueDracowarrior = 22499034;
            public const int InspectorBoarder = 15397015;

            // Main Deck Spells
            public const int DragonicDiagram = 13035077;
            public const int DracoAwakening = 66092596;
            public const int TrueDracoHeritage = 49430782;
            public const int DisciplesOfTheTrueDracophoenix = 75425320;
            public const int DomainOfTheTrueMonarchs = 84171830;
            public const int CardOfDemise = 59750328;
            public const int PotOfDuality = 98645731;
            public const int Terraforming = 73628505;

            // Main Deck Traps
            public const int TrueDracoApocalypse = 61529473;
            public const int TrueKingsReturn = 35125879;
            public const int TheMonarchsErupt = 48716527;
            public const int LoseOneTurn = 24348804;
            public const int SkillDrain = 82732705;
            public const int ThereCanBeOnlyOne = 24207889;
            public const int RivalryOfWarlords = 90846359;
            public const int AndTheBandPlayedOn = 47594939;
            public const int ApophisTheSwampDeity = 85888377;

            // Hand Traps / Staples (Main)
            public const int MaxxC = 23434538;
            public const int AshBlossom = 14558127;
            public const int Nibiru = 27204311;
            public const int DrollAndLockBird = 94145021;

            // Side Deck
            public const int SphereMode = 10000080;
            public const int Gameciel = 55063751;
            public const int EvenlyMatched = 15693423;
            public const int GraveOfTheSuperAncientOrganism = 83266092;
            public const int AntiSpellFragrance = 58921041;
        }

        private static readonly int[] TrueDracoMonsters = {
            CardId.MasterPeaceTheTrueDracoslayingKing,
            CardId.DinomightKnightTheTrueDracofighter,
            CardId.IgnisHeatTheTrueDracowarrior
        };

        private static readonly int[] TrueDracoSpellTraps = {
            CardId.DragonicDiagram,
            CardId.TrueDracoHeritage,
            CardId.DracoAwakening,
            CardId.DisciplesOfTheTrueDracophoenix,
            CardId.TrueDracoApocalypse,
            CardId.TrueKingsReturn
        };

        private static readonly int[] FloodgateTraps = {
            CardId.TheMonarchsErupt,
            CardId.LoseOneTurn,
            CardId.SkillDrain,
            CardId.ThereCanBeOnlyOne,
            CardId.RivalryOfWarlords,
            CardId.AndTheBandPlayedOn,
            CardId.AntiSpellFragrance,
            CardId.GraveOfTheSuperAncientOrganism
        };

        private static readonly int[] ContinuousST = {
            CardId.TrueDracoHeritage,
            CardId.DracoAwakening,
            CardId.DisciplesOfTheTrueDracophoenix,
            CardId.TrueDracoApocalypse,
            CardId.TrueKingsReturn,
            CardId.TheMonarchsErupt,
            CardId.LoseOneTurn,
            CardId.SkillDrain,
            CardId.ThereCanBeOnlyOne,
            CardId.RivalryOfWarlords,
            CardId.AndTheBandPlayedOn,
            CardId.ApophisTheSwampDeity,
            CardId.AntiSpellFragrance,
            CardId.GraveOfTheSuperAncientOrganism
        };

        private bool _diagramSearchUsed = false;
        private bool _heritageDrawUsed = false;
        private bool _awakeningUsed = false;
        private bool _demiseUsed = false;
        private bool _kingsReturnUsed = false;
        private bool _masterPeaceUsed = false;
        private bool _dinomightUsed = false;
        private bool _ignisUsed = false;
        private bool _disciplesUsed = false;

        private bool _opponentMonsterEffectActivatedThisTurn = false;
        private bool _haveTributeSummonedThisTurn = false;

        public override bool OnSelectHand()
        {
            return true;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (card != null && card.Controller == 1)
            {
                if (card.IsMonster())
                    _opponentMonsterEffectActivatedThisTurn = true;
            }
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _diagramSearchUsed = false;
            _heritageDrawUsed = false;
            _awakeningUsed = false;
            _demiseUsed = false;
            _kingsReturnUsed = false;
            _masterPeaceUsed = false;
            _dinomightUsed = false;
            _ignisUsed = false;
            _disciplesUsed = false;
            _opponentMonsterEffectActivatedThisTurn = false;
            _haveTributeSummonedThisTurn = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        private bool IsTrueDracoMonster(ClientCard c)
        {
            return c != null && c.IsMonster() && c.IsCode(TrueDracoMonsters);
        }

        private bool IsContinuousSpellOrTrapOnField(ClientCard c)
        {
            if (c == null) return false;
            if (c.Location != CardLocation.SpellZone) return false;
            if (!c.IsFaceup()) return false;
            return c.HasType(CardType.Continuous) || c.HasType(CardType.Field);
        }

        public _2026_TrueDracoExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(
                CardId.MasterPeaceTheTrueDracoslayingKing,
                CardId.DinomightKnightTheTrueDracofighter,
                CardId.InspectorBoarder
            );

            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Diagram-Search",
                RequiredCards = new List<int> { CardId.DragonicDiagram },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.DragonicDiagram, ActionType = ExecutorType.Activate, Description = "Activate Diagram" },
                    new() { CardId = CardId.DragonicDiagram, ActionType = ExecutorType.Activate, Description = "Diagram search effect" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Demise-Draw",
                RequiredCards = new List<int> { CardId.CardOfDemise },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.CardOfDemise, ActionType = ExecutorType.Activate, Description = "Activate Card of Demise to draw" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.DragonicDiagram, CardId.CardOfDemise);
            BaitPlanner.RegisterBaitCards(CardId.Terraforming, CardId.PotOfDuality);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.DragonicDiagram, CardId.CardOfDemise, CardId.MasterPeaceTheTrueDracoslayingKing);

            // TIER 1: Hand Traps
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, () => SmartHandTrapChain() && DefaultMaxxC());
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.Nibiru, NibiruEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollEffect);

            // TIER 2: Field Spell Activation (Diagram / Domain)
            AddExecutor(ExecutorType.Activate, CardId.DragonicDiagram, DiagramActivateEffect);
            AddExecutor(ExecutorType.Activate, CardId.DomainOfTheTrueMonarchs, DomainActivateEffect);

            // TIER 3: Draw Spells (Pot of Duality / Card of Demise)
            AddExecutor(ExecutorType.Activate, CardId.PotOfDuality, PotOfDualityEffect);
            AddExecutor(ExecutorType.Activate, CardId.CardOfDemise, CardOfDemiseEffect);

            // TIER 4: Diagram Search Effect
            AddExecutor(ExecutorType.Activate, CardId.DragonicDiagram, DiagramSearchEffect);

            // TIER 5: Continuous Spell/Trap Activations
            AddExecutor(ExecutorType.Activate, CardId.TrueDracoHeritage, HeritageActivateEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrueDracoApocalypse, ApocalypseActivateEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrueKingsReturn, KingsReturnActivateEffect);
            AddExecutor(ExecutorType.Activate, CardId.DisciplesOfTheTrueDracophoenix, DisciplesActivateEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracoAwakening, AwakeningActivateEffect);

            // TIER 6: Floodgate Trap Activations
            AddExecutor(ExecutorType.Activate, CardId.TheMonarchsErupt, MonarchsEruptEffect);
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainEffect);
            AddExecutor(ExecutorType.Activate, CardId.LoseOneTurn, LoseOneTurnEffect);
            AddExecutor(ExecutorType.Activate, CardId.ThereCanBeOnlyOne, FloodgateEffect);
            AddExecutor(ExecutorType.Activate, CardId.RivalryOfWarlords, FloodgateEffect);
            AddExecutor(ExecutorType.Activate, CardId.AndTheBandPlayedOn, FloodgateEffect);
            AddExecutor(ExecutorType.Activate, CardId.GraveOfTheSuperAncientOrganism, FloodgateEffect);
            AddExecutor(ExecutorType.Activate, CardId.AntiSpellFragrance, AntiSpellFragranceEffect);

            // TIER 7: Continuous S/T Draw/Search Effects (MP activation)
            AddExecutor(ExecutorType.Activate, CardId.TrueDracoHeritage, HeritageDrawEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracoAwakening, AwakeningDrawEffect);
            AddExecutor(ExecutorType.Activate, CardId.DisciplesOfTheTrueDracophoenix, DisciplesDrawEffect);

            // TIER 8: Tribute Summon
            AddExecutor(ExecutorType.Summon, CardId.MasterPeaceTheTrueDracoslayingKing, MasterPeaceSummon);
            AddExecutor(ExecutorType.Summon, CardId.DinomightKnightTheTrueDracofighter, TrueDracoMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.IgnisHeatTheTrueDracowarrior, TrueDracoMonsterSummon);
            AddExecutor(ExecutorType.Summon, CardId.InspectorBoarder, BoarderSummon);

            // TIER 9: Tribute Summon Triggers
            AddExecutor(ExecutorType.Activate, CardId.DinomightKnightTheTrueDracofighter, DinomightSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.IgnisHeatTheTrueDracowarrior, IgnisSearchEffect);
            AddExecutor(ExecutorType.Activate, CardId.MasterPeaceTheTrueDracoslayingKing, MasterPeaceTriggerEffect);

            // TIER 10: GY Floating Effects
            AddExecutor(ExecutorType.Activate, CardId.TrueDracoHeritage, HeritageGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrueDracoApocalypse, ApocalypseGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.TrueKingsReturn, KingsReturnGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracoAwakening, AwakeningGYEffect);
            AddExecutor(ExecutorType.Activate, CardId.DisciplesOfTheTrueDracophoenix, DisciplesGYEffect);

            // TIER 11: Master Peace Field Effects
            AddExecutor(ExecutorType.Activate, CardId.MasterPeaceTheTrueDracoslayingKing, MasterPeaceFieldEffect);

            // TIER 12: Trap Monster Summon (Apophis)
            AddExecutor(ExecutorType.Activate, CardId.ApophisTheSwampDeity, ApophisActivateEffect);

            // TIER 13: Evenly Matched
            AddExecutor(ExecutorType.Activate, CardId.EvenlyMatched, EvenlyMatchedEffect);

            // TIER 14: Sets & Repos
            AddExecutor(ExecutorType.SpellSet, SpellSetEffect);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        // ==========================================
        //  TIER 1: Hand Traps
        // ==========================================

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.LastChainPlayer == 1 && DefaultAshBlossomAndJoyousSpring();
        }

        private bool NibiruEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                if (Enemy.GetMonsterCount() >= 2)
                    return true;
            }
            return false;
        }

        private bool DrollEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return Duel.Player == 1 && Duel.LastChainPlayer == 1;
        }

        // ==========================================
        //  TIER 2: Field Spell Activation
        // ==========================================

        private bool DiagramActivateEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.SpellZone[5] == null || (Bot.SpellZone[5] != null && Bot.SpellZone[5].Id != CardId.DragonicDiagram);
        }

        private bool DomainActivateEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.SpellZone[5] == null || (Bot.SpellZone[5] != null && Bot.SpellZone[5].Id != CardId.DomainOfTheTrueMonarchs);
        }

        // ==========================================
        //  TIER 3: Draw Spells
        // ==========================================

        private bool PotOfDualityEffect()
        {
            if (Duel.Player != 0 || !Duel.IsMainPhase()) return false;
            return Bot.ExtraDeck.Count == 0 || ShouldActivateDrawSpell();
        }

        private bool CardOfDemiseEffect()
        {
            if (_demiseUsed) return false;
            if (Duel.Player != 0 || !Duel.IsMainPhase()) return false;
            _demiseUsed = true;
            return true;
        }

        private bool ShouldActivateDrawSpell()
        {
            if (IsSpecialSummonBlocked()) return true;
            return !_haveTributeSummonedThisTurn && Bot.Hand.Count <= 3;
        }

        // ==========================================
        //  TIER 4: Diagram Search
        // ==========================================

        private bool DiagramSearchEffect()
        {
            if (_diagramSearchUsed) return false;
            if (Card.Location != CardLocation.SpellZone) return false;
            if (ActivateDescription != 0 && ActivateDescription != Util.GetStringId(CardId.DragonicDiagram, 0)) return false;

            _diagramSearchUsed = true;
            if (ShouldAddMonster())
            {
                AI.SelectCard(new[] {
                    CardId.DinomightKnightTheTrueDracofighter,
                    CardId.MasterPeaceTheTrueDracoslayingKing,
                    CardId.IgnisHeatTheTrueDracowarrior
                });
            }
            else if (ShouldAddSpell())
            {
                AI.SelectCard(new[] {
                    CardId.TrueDracoHeritage,
                    CardId.TrueDracoApocalypse,
                    CardId.DisciplesOfTheTrueDracophoenix
                });
            }
            else
            {
                AI.SelectCard(new[] {
                    CardId.TrueDracoHeritage,
                    CardId.TrueDracoApocalypse,
                    CardId.TrueKingsReturn,
                    CardId.TheMonarchsErupt,
                    CardId.SkillDrain
                });
            }
            return true;
        }

        private bool ShouldAddMonster()
        {
            bool hasDinomight = Bot.HasInHand(CardId.DinomightKnightTheTrueDracofighter) || Bot.HasInMonstersZone(CardId.DinomightKnightTheTrueDracofighter);
            bool hasIgnis = Bot.HasInHand(CardId.IgnisHeatTheTrueDracowarrior) || Bot.HasInMonstersZone(CardId.IgnisHeatTheTrueDracowarrior);
            bool hasMasterPeace = Bot.HasInHand(CardId.MasterPeaceTheTrueDracoslayingKing) || Bot.HasInMonstersZone(CardId.MasterPeaceTheTrueDracoslayingKing);

            if (!hasDinomight) return true;
            if (!hasIgnis && _isGoingSecond) return true;
            if (!hasMasterPeace && Bot.GetMonsterCount() >= 2) return true;
            return false;
        }

        private bool ShouldAddSpell()
        {
            bool hasHeritage = Bot.HasInHand(CardId.TrueDracoHeritage) || Bot.HasInSpellZone(CardId.TrueDracoHeritage);
            bool hasApocalypse = Bot.HasInHand(CardId.TrueDracoApocalypse) || Bot.HasInSpellZone(CardId.TrueDracoApocalypse);

            if (!hasHeritage) return true;
            if (!hasApocalypse && _isGoingSecond) return true;
            return false;
        }

        // ==========================================
        //  TIER 5: Continuous S/T Activation
        // ==========================================

        private bool HeritageActivateEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.GetSpellCount() < 4;
        }

        private bool ApocalypseActivateEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.GetSpellCount() < 4;
        }

        private bool KingsReturnActivateEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.GetSpellCount() < 4;
        }

        private bool DisciplesActivateEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.GetSpellCount() < 4;
        }

        private bool AwakeningActivateEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            return Bot.GetSpellCount() < 4;
        }

        // ==========================================
        //  TIER 6: Floodgate Trap Activation
        // ==========================================

        private bool MonarchsEruptEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (Card.IsFacedown()) return false;
            bool hasTributeSummonedMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsTrueDracoMonster(c));
            if (!hasTributeSummonedMonster) return false;
            bool enemyHasMonsterEffects = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect));
            return enemyHasMonsterEffects;
        }

        private bool SkillDrainEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (Card.IsFacedown()) return false;
            return Duel.Player == 1 && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Effect) && c.Attack >= 1500);
        }

        private bool LoseOneTurnEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (Card.IsFacedown()) return false;
            if (ActivateDescription == Util.GetStringId(CardId.LoseOneTurn, 0))
            {
                // Activate when opponent has a newly summoned monster on their turn
                return Duel.Player == 1 && Enemy.GetMonsterCount() > 0;
            }
            return false;
        }

        private bool FloodgateEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (Card.IsFacedown()) return false;
            return Duel.Player == 1 || (Duel.Player == 0 && _opponentMonsterEffectActivatedThisTurn);
        }

        private bool AntiSpellFragranceEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (Card.IsFacedown()) return false;
            return Duel.Player == 1 || (Duel.Player == 0 && Bot.GetMonsterCount() > 0);
        }

        // ==========================================
        //  TIER 7: Continuous S/T Draw/Search (MP)
        // ==========================================

        private bool HeritageDrawEffect()
        {
            if (_heritageDrawUsed) return false;
            if (Card.Location != CardLocation.SpellZone) return false;
            if (!Card.IsFaceup()) return false;
            if (Duel.Player != 0 || !Duel.IsMainPhase()) return false;

            // Only activate if we need cards or have something to send
            if (Bot.Hand.Count <= 4)
            {
                _heritageDrawUsed = true;
                // After draw, send True Draco card from Deck to GY to set up floating
                if (Bot.GetRemainingCount(CardId.TrueDracoHeritage, 3) > 0 ||
                    Bot.GetRemainingCount(CardId.TrueDracoApocalypse, 3) > 0 ||
                    Bot.GetRemainingCount(CardId.TrueKingsReturn, 2) > 0 ||
                    Bot.GetRemainingCount(CardId.DisciplesOfTheTrueDracophoenix, 1) > 0)
                {
                    AI.SelectOption(0);
                }
                else
                {
                    AI.SelectOption(1);
                }
                return true;
            }
            return false;
        }

        private bool AwakeningDrawEffect()
        {
            if (_awakeningUsed) return false;
            if (Card.Location != CardLocation.SpellZone) return false;
            if (!Card.IsFaceup()) return false;
            if (Duel.Player != 0 || !Duel.IsMainPhase()) return false;

            if (Bot.Hand.Count <= 3)
            {
                _awakeningUsed = true;
                return true;
            }
            return false;
        }

        private bool DisciplesDrawEffect()
        {
            if (_disciplesUsed) return false;
            if (Card.Location != CardLocation.SpellZone) return false;
            if (!Card.IsFaceup()) return false;
            if (Duel.Player != 0 || !Duel.IsMainPhase()) return false;

            if (GetFreeMonsterZoneCount() > 0 && Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsTrueDracoMonster(c)))
            {
                _disciplesUsed = true;
                AI.SelectOption(0);
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 8: Tribute Summon
        // ==========================================

        private int GetTributeCount()
        {
            if (Card.Level >= 7) return 2;
            return 1;
        }

        private bool HasEnoughTributes()
        {
            int needed = GetTributeCount();

            // Count face-up Continuous S/T (can be tributed for True Draco summon)
            int continuousST = Bot.GetSpells().Count(c => c != null && c.IsFaceup() &&
                (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)));

            // Count monsters
            int monsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());

            // We can use either monsters OR S/T, or a mix
            // For Master Peace: need 2 tributes FROM either pool
            // For Dinomight/Ignis: need 1 tribute FROM either pool
            return (monsters + continuousST) >= needed;
        }

        private bool MasterPeaceSummon()
        {
            if (Bot.HasInMonstersZone(CardId.MasterPeaceTheTrueDracoslayingKing)) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (!HasEnoughTributes()) return false;
            if (ShouldSkipCombo())
            {
                if (!CanTributeSummonDirectAttack()) return false;
            }
            return true;
        }

        private bool TrueDracoMonsterSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (!HasEnoughTributes()) return false;
            if (ShouldSkipCombo())
            {
                if (!CanTributeSummonDirectAttack()) return false;
            }
            return true;
        }

        private bool BoarderSummon()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (_haveTributeSummonedThisTurn) return false;
            if (Bot.GetMonsterCount() >= 2) return false;
            return Duel.Player == 0 && Duel.Turn <= 2;
        }

        private bool CanTributeSummonDirectAttack()
        {
            return Enemy.GetMonsterCount() == 0;
        }

        // ==========================================
        //  TIER 9: Tribute Summon Triggers
        // ==========================================

        private bool DinomightSearchEffect()
        {
            if (_dinomightUsed) return false;
            _dinomightUsed = true;
            _haveTributeSummonedThisTurn = true;

            if (ShouldAddFloodgate())
            {
                AI.SelectCard(new[] {
                    CardId.TheMonarchsErupt,
                    CardId.SkillDrain,
                    CardId.ThereCanBeOnlyOne,
                    CardId.RivalryOfWarlords,
                    CardId.TrueDracoApocalypse,
                    CardId.TrueKingsReturn
                });
            }
            else
            {
                AI.SelectCard(new[] {
                    CardId.TrueDracoHeritage,
                    CardId.TrueDracoApocalypse,
                    CardId.TrueKingsReturn,
                    CardId.DisciplesOfTheTrueDracophoenix,
                    CardId.DracoAwakening
                });
            }
            return true;
        }

        private bool IgnisSearchEffect()
        {
            if (_ignisUsed) return false;
            _ignisUsed = true;
            _haveTributeSummonedThisTurn = true;

            AI.SelectCard(new[] {
                CardId.TrueDracoHeritage,
                CardId.TrueDracoApocalypse,
                CardId.TrueKingsReturn,
                CardId.DisciplesOfTheTrueDracophoenix,
                CardId.DracoAwakening
            });
            return true;
        }

        private bool ShouldAddFloodgate()
        {
            if (Duel.Turn <= 2 && Duel.Player == 0)
            {
                bool hasErupt = Bot.HasInHand(CardId.TheMonarchsErupt) || Bot.HasInSpellZone(CardId.TheMonarchsErupt);
                bool hasSkillDrain = Bot.HasInHand(CardId.SkillDrain) || Bot.HasInSpellZone(CardId.SkillDrain);
                bool hasTCBOO = Bot.HasInHand(CardId.ThereCanBeOnlyOne) || Bot.HasInSpellZone(CardId.ThereCanBeOnlyOne);
                return !hasErupt || !hasSkillDrain || !hasTCBOO;
            }
            return false;
        }

        private bool MasterPeaceTriggerEffect()
        {
            if (_masterPeaceUsed) return false;
            _masterPeaceUsed = true;
            _haveTributeSummonedThisTurn = true;
            return true;
        }

        // ==========================================
        //  TIER 10: GY Floating Effects
        // ==========================================

        private bool HeritageGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            // Destroy 1 Spell/Trap on field โ€” prioritize highest threat continuous/field first
            var targets = Enemy.GetSpells()
                .Where(c => c != null && IsViableEffectTarget(c))
                .ToList();
            if (targets.Count > 0)
            {
                var priority = targets
                    .OrderByDescending(c =>
                    {
                        int score = Scorer != null ? Scorer.ThreatScore(c) : 0;
                        if (c.IsFaceup() && c.HasType(CardType.Field)) score += 200;
                        else if (c.IsFaceup() && c.HasType(CardType.Continuous)) score += 100;
                        return score;
                    })
                    .First();
                AI.SelectCard(priority);
                return true;
            }
            // Also try to destroy our own S/T to trigger floating (only if no enemy targets)
            var ownTargets = Bot.GetSpells()
                .Where(c => c != null && c.IsFaceup() && c.IsCode(ContinuousST))
                .OrderBy(c =>
                {
                    // Prefer destroying cards with best GY floating
                    if (c.IsCode(CardId.TrueDracoApocalypse)) return 1;
                    if (c.IsCode(CardId.TrueKingsReturn)) return 2;
                    if (c.IsCode(CardId.DisciplesOfTheTrueDracophoenix)) return 3;
                    if (c.IsCode(CardId.TrueDracoHeritage)) return 4;
                    return 10;
                }).FirstOrDefault();
            if (ownTargets != null)
            {
                AI.SelectCard(ownTargets);
                return true;
            }
            return false;
        }

        private bool ApocalypseGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            // Special Summon 1 "True Draco" monster from Deck
            if (GetFreeMonsterZoneCount() > 0)
            {
                AI.SelectCard(new[] {
                    CardId.DinomightKnightTheTrueDracofighter,
                    CardId.MasterPeaceTheTrueDracoslayingKing,
                    CardId.IgnisHeatTheTrueDracowarrior
                });
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        private bool KingsReturnGYEffect()
        {
            if (_kingsReturnUsed) return false;
            if (Card.Location != CardLocation.Grave) return false;
            if (GetFreeMonsterZoneCount() == 0) return false;
            // Banish itself โ’ Special Summon 1 True Draco monster from GY
            var targets = Bot.Graveyard
                .Where(c => c != null && c.IsCode(TrueDracoMonsters) && c != Card && c.IsCanRevive())
                .ToList();
            if (targets.Count > 0)
            {
                _kingsReturnUsed = true;
                // Prefer highest-level (Master Peace) โ’ Dinomight โ’ Ignis
                AI.SelectCard(targets
                    .OrderByDescending(c => c.IsCode(CardId.MasterPeaceTheTrueDracoslayingKing) ? 100 :
                                           c.IsCode(CardId.DinomightKnightTheTrueDracofighter) ? 50 : 10)
                    .First());
                return true;
            }
            return false;
        }

        private bool AwakeningGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            // Add 1 "True Draco" or "True King" card from Deck to hand
            AI.SelectCard(new[] {
                CardId.MasterPeaceTheTrueDracoslayingKing,
                CardId.DinomightKnightTheTrueDracofighter,
                CardId.IgnisHeatTheTrueDracowarrior,
                CardId.TrueDracoHeritage,
                CardId.TrueDracoApocalypse
            });
            return true;
        }

        private bool DisciplesGYEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            // Place 1 "True Draco" or "True King" Continuous Spell/Trap from Deck face-up
            if (Bot.GetSpellCount() < 4)
            {
                AI.SelectCard(new[] {
                    CardId.TrueDracoHeritage,
                    CardId.TrueDracoApocalypse,
                    CardId.TrueKingsReturn,
                    CardId.DracoAwakening
                });
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 11: Master Peace Field Effects
        // ==========================================

        private bool MasterPeaceFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            if (ActivateDescription == Util.GetStringId(CardId.MasterPeaceTheTrueDracoslayingKing, 0))
            {
                // Effect 1: Once per turn (main phase or opponent's turn) โ€” destroy 1 card on field
                if (!Duel.IsMainPhase() && Duel.Player == 0) return false;

                // Priority: highest-threat monster > highest-threat continuous S/T > any face-up S/T
                var monsters = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && IsViableEffectTarget(c))
                    .ToList();
                var spells = Enemy.GetSpells()
                    .Where(c => c != null && IsViableEffectTarget(c))
                    .ToList();

                if (monsters.Count > 0 || spells.Count > 0)
                {
                    // Score all targets together
                    var allTargets = monsters.Concat(spells)
                        .OrderByDescending(c =>
                        {
                            int score = Scorer != null ? Scorer.ThreatScore(c) : (c.IsMonster() ? c.Attack / 100 : 20);
                            if (c.IsFaceup() && c.HasType(CardType.Field)) score += 200;
                            else if (c.IsFaceup() && c.HasType(CardType.Continuous)) score += 100;
                            return score;
                        })
                        .ToList();

                    if (allTargets.Count > 0)
                    {
                        AI.SelectCard(allTargets[0]);
                        return true;
                    }
                }
                return false;
            }
            else if (ActivateDescription == Util.GetStringId(CardId.MasterPeaceTheTrueDracoslayingKing, 1))
            {
                // Effect 2: Negate activation (Quick Effect) โ€” opponent's turn or in response
                if (LastChainCard != null && LastChainCard.Controller == 0) return false; // don't negate ourselves
                if (LastChainCard != null && LastChainCard.Controller == 1)
                    return true;
                return false;
            }
            return false;
        }

        // ==========================================
        //  TIER 12: Apophis / Trap Monster
        // ==========================================

        private bool ApophisActivateEffect()
        {
            if (Card.Location != CardLocation.SpellZone) return false;
            if (Card.IsFacedown()) return false;
            if (GetFreeMonsterZoneCount() == 0) return false;
            if (Duel.Player == 1 && Duel.Phase == DuelPhase.End) return false;

            // Activate if we need tribute fodder or extra monster
            bool needTribute = Bot.Hand.Any(c => c != null && c.IsCode(TrueDracoMonsters) && c.Level >= 5);
            if (needTribute && GetFreeMonsterZoneCount() > 0)
            {
                AI.SelectPosition(CardPosition.FaceUpDefence);
                return true;
            }
            return false;
        }

        // ==========================================
        //  TIER 13: Evenly Matched
        // ==========================================

        private bool EvenlyMatchedEffect()
        {
            if (Duel.Player != 1) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2)
            {
                if (Duel.Phase != DuelPhase.End) return false;
            }
            if (Duel.Phase == DuelPhase.Main1 && !_opponentMonsterEffectActivatedThisTurn) return false;

            // Only activate if opponent has more cards than us
            if (Enemy.GetFieldCount() <= Bot.GetFieldCount()) return false;
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;

            return true;
        }

        // ==========================================
        //  TIER 14: Sets & Repos
        // ==========================================

        private bool SpellSetEffect()
        {
            if (Card.Location != CardLocation.Hand) return false;
            if (Bot.GetSpellCount() >= 4) return false;

            if (Card.IsCode(CardId.TrueDracoApocalypse) ||
                Card.IsCode(CardId.TrueKingsReturn) ||
                Card.IsCode(CardId.TheMonarchsErupt) ||
                Card.IsCode(CardId.LoseOneTurn) ||
                Card.IsCode(CardId.SkillDrain) ||
                Card.IsCode(CardId.ThereCanBeOnlyOne) ||
                Card.IsCode(CardId.RivalryOfWarlords) ||
                Card.IsCode(CardId.AndTheBandPlayedOn) ||
                Card.IsCode(CardId.AntiSpellFragrance) ||
                Card.IsCode(CardId.GraveOfTheSuperAncientOrganism) ||
                Card.IsCode(CardId.EvenlyMatched) ||
                Card.IsCode(CardId.ApophisTheSwampDeity))
            {
                return true;
            }
            return false;
        }

        // ==========================================
        //  OnSelectCard Override
        // ==========================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // โ”€โ”€ STEP 1: Handle HINTS first (Card may be null in these paths) โ”€โ”€

            // Hint 503: Tribute selection for Tribute Summon
            if (hint == 503)
            {
                // Priority: Continuous Spells/Traps > Weak Monsters > Strong Monsters
                var sorted = cards.OrderBy(c =>
                {
                    if (c == null) return 9999;
                    if (c.Location == CardLocation.SpellZone && c.IsFaceup() && c.IsCode(ContinuousST))
                    {
                        // Prefer tribute cards that have floating effects: Heritage, Apocalypse, Awakening
                        if (c.IsCode(CardId.TrueDracoHeritage)) return 1;
                        if (c.IsCode(CardId.TrueDracoApocalypse)) return 2;
                        if (c.IsCode(CardId.DracoAwakening)) return 3;
                        if (c.IsCode(CardId.DisciplesOfTheTrueDracophoenix)) return 4;
                        if (c.IsCode(CardId.TrueKingsReturn)) return 5;
                        return 10;
                    }
                    if (c.Location == CardLocation.MonsterZone)
                    {
                        // Prefer tribute weak monsters, keep strong ones
                        if (c.IsCode(CardId.ApophisTheSwampDeity)) return 6;
                        if (c.IsCode(CardId.InspectorBoarder)) return 7;
                        if (c.HasType(CardType.TrapMonster)) return 8;
                        if (c.Attack < 1500) return 20;
                        if (TrueDracoMonsters.Contains(c.Id)) return 50;
                        return 30;
                    }
                    return 100;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            // โ”€โ”€ STEP 2: Null-safe Card check โ”€โ”€
            if (Card == null) return base.OnSelectCard(cards, min, max, hint, cancelable);

            // โ”€โ”€ STEP 3: Card-specific logic โ”€โ”€

            // Domain of the True Monarchs: select which attribute
            if (Card.IsCode(CardId.DomainOfTheTrueMonarchs))
            {
                if (cards.Count >= min)
                {
                    // Default to EARTH for Master Peace, or match our monster
                    var masterPeace = cards.FirstOrDefault(c => c.IsCode(CardId.MasterPeaceTheTrueDracoslayingKing));
                    if (masterPeace != null) return new List<ClientCard> { masterPeace };
                    return cards.Take(max).ToList();
                }
            }

            // Heritage / Diagram GY effect: select S/T to destroy (highest threat first)
            if (Card.IsCode(CardId.TrueDracoHeritage, CardId.DragonicDiagram) && Card.Location == CardLocation.Grave)
            {
                if (cards.Any(c => c != null && c.Controller == 1))
                {
                    var sorted = cards
                        .Where(c => c != null && c.Controller == 1)
                        .OrderByDescending(c =>
                        {
                            int score = Scorer != null ? Scorer.ThreatScore(c) : 0;
                            if (c.IsFaceup() && c.HasType(CardType.Field)) score += 200;
                            else if (c.IsFaceup() && c.HasType(CardType.Continuous)) score += 100;
                            return score;
                        }).ToList();
                    if (sorted.Count > 0) return sorted.Take(max).ToList();
                }
            }

            // Apocalypse GY effect: summon from Deck
            if (Card.IsCode(CardId.TrueDracoApocalypse) && Card.Location == CardLocation.Grave)
            {
                var masterPeace = cards.FirstOrDefault(c => c.Id == CardId.MasterPeaceTheTrueDracoslayingKing);
                if (masterPeace != null) return new List<ClientCard> { masterPeace };
                var dinomight = cards.FirstOrDefault(c => c.Id == CardId.DinomightKnightTheTrueDracofighter);
                if (dinomight != null) return new List<ClientCard> { dinomight };
                var ignis = cards.FirstOrDefault(c => c.Id == CardId.IgnisHeatTheTrueDracowarrior);
                if (ignis != null) return new List<ClientCard> { ignis };
            }

            // True King's Return GY effect: SS True Draco from GY (prefer highest priority)
            if (Card.IsCode(CardId.TrueKingsReturn) && Card.Location == CardLocation.Grave)
            {
                // SS from GY โ€” pick best available; prefer Master Peace โ’ Dinomight โ’ Ignis
                var masterPeace = cards.FirstOrDefault(c => c != null && c.Id == CardId.MasterPeaceTheTrueDracoslayingKing);
                if (masterPeace != null) return new List<ClientCard> { masterPeace };
                var dinomight = cards.FirstOrDefault(c => c != null && c.Id == CardId.DinomightKnightTheTrueDracofighter);
                if (dinomight != null) return new List<ClientCard> { dinomight };
                var ignis = cards.FirstOrDefault(c => c != null && c.Id == CardId.IgnisHeatTheTrueDracowarrior);
                if (ignis != null) return new List<ClientCard> { ignis };
            }

            // Dragonic Diagram search
            if (Card.IsCode(CardId.DragonicDiagram))
            {
                if (cards.Any(c => c.Location == CardLocation.Deck))
                {
                    var dinomight = cards.FirstOrDefault(c => c.Id == CardId.DinomightKnightTheTrueDracofighter);
                    if (dinomight != null) return new List<ClientCard> { dinomight };
                    var heritage = cards.FirstOrDefault(c => c.Id == CardId.TrueDracoHeritage);
                    if (heritage != null) return new List<ClientCard> { heritage };
                    var apocalypse = cards.FirstOrDefault(c => c.Id == CardId.TrueDracoApocalypse);
                    if (apocalypse != null) return new List<ClientCard> { apocalypse };
                }
            }

            // Card of Demise discard phase
            if (Card.IsCode(CardId.CardOfDemise))
            {
                var sorted = cards.OrderBy(c =>
                {
                    if (c == null) return 9999;
                    if (IsAceCard(c) && c.Controller == 0) return 10000;
                    if (c.IsCode(CardId.InspectorBoarder) && c.Controller == 0) return 9000;
                    if (c.IsCode(TrueDracoMonsters)) return 100;
                    if (c.IsCode(FloodgateTraps)) return 50;
                    if (c.IsCode(TrueDracoSpellTraps)) return 80;
                    return 200;
                }).ToList();
                return sorted.Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ==========================================
        //  Material Selectors (for potential Extra)
        // ==========================================

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 9999;
            if (IsAceCard(c)) return 10000;
            if (c.IsCode(CardId.InspectorBoarder)) return 8000;
            if (c.IsCode(TrueDracoMonsters)) return 1000;
            if (c.IsCode(TrueDracoSpellTraps)) return 100;
            return base.GetMaterialPriority(c);
        }

        // ==========================================
        //  IsAceCard
        // ==========================================

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            if (card.IsCode(
                CardId.MasterPeaceTheTrueDracoslayingKing,
                CardId.DinomightKnightTheTrueDracofighter,
                CardId.InspectorBoarder))
                return true;
            return base.IsAceCard(card);
        }
        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.MasterPeaceTheTrueDracoslayingKing)) return true;
            if (Bot.HasInMonstersZone(CardId.DinomightKnightTheTrueDracofighter)) return true;
            if (Bot.HasInMonstersZone(CardId.InspectorBoarder)) return true;
            return base.IsBoardStrongEnough();
        }
        protected override bool ShouldStopExtending()
        {
            if (Bot.HasInMonstersZone(CardId.MasterPeaceTheTrueDracoslayingKing)
                || Bot.HasInMonstersZone(CardId.DinomightKnightTheTrueDracofighter))
                return base.ShouldStopExtending();
            return false;
        }

        // ==========================================
        //  Helpers
        // ==========================================

        private int GetFreeMonsterZoneCount()
        {
            int count = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (Bot.MonsterZone[i] == null) count++;
            }
            return count;
        }
    }
}
