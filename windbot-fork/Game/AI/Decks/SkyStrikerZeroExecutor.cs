using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.DecisionEngine;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("SkyStrikerZero", "SkyStrikerZero")]
    public class SkyStrikerZeroExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int Raye = 26077387;
            public const int Roze = 37351133;
            public const int AshBlossom = 14558127;
            public const int AshBlossomAlt = 14558128;
            public const int GhostBelle = 73642296;
            public const int DrollAndLockBird = 94145021;
            public const int MaxxC = 23434538;

            // Spells
            public const int MysticalSpaceTyphoon = 5318639;
            public const int Linkage = 9726840;
            public const int RadiantTyphoonVision = 20508881;
            public const int CalledByTheGrave = 24224830;
            public const int TripleTacticsTalent = 25311006;
            public const int TheFallenAndTheVirtuous = 30271097;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int Lemnisgate = 34433770;
            public const int PotOfDesires = 35261759;
            public const int HornetDrones = 52340444;
            public const int Token = 52340445;
            public const int Engage = 63166095;
            public const int WidowAnchor = 98338152;
            public const int ForbiddenCrown = 98829635;

            // Extra Deck
            public const int Hayate = 8491308;
            public const int Kaina = 12421694;
            public const int SPLittleKnight = 29301450;
            public const int Kagari = 63288573;
            public const int Zeke = 75147529;
            public const int Zero = 76072561;
            public const int EcclesiaAndTheDarkDragon = 78397661;
            public const int AlbionTheBrandedDragon = 87746184;
            public const int Shizuku = 90673288;
            public const int SuperStarslayerTYPHON = 93039339;
            public const int Azalea = 98462037;
        }

        private static readonly int[] SkyStrikerLinks = {
            CardId.Kagari, CardId.Shizuku, CardId.Hayate, CardId.Kaina,
            CardId.Zero, CardId.Zeke, CardId.Azalea, CardId.SPLittleKnight,
            CardId.SuperStarslayerTYPHON
        };

        internal SkyStrikerZeroPlugin Plugin { get; private set; }

        private bool _hasOpponentActivatedMonsterEffect = false;
        private bool _crownUsed = false;
        private bool _kagariUsedThisTurn = false;

        public SkyStrikerZeroExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new SkyStrikerZeroPlugin(this);
            DeckPlugin = Plugin;

            RegisterHelperModules();
            RegisterComboLines();
            RegisterExecutors();
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _hasOpponentActivatedMonsterEffect = false;
            _crownUsed = false;
            _kagariUsedThisTurn = false;
            Plugin?.ResetTurnState();
        }

        private void RegisterHelperModules()
        {
            // 1. Central Core Ace Card Protection
            ResourcePlan.RegisterAceCards(SkyStrikerLinks);
            HeuristicGuard.RegisterAceCards(SkyStrikerLinks);

            // 2. Bait Planner & Combo Starters
            BaitPlanner.RegisterComboStarters(CardId.Engage, CardId.Linkage, CardId.HornetDrones, CardId.Raye);
            BaitPlanner.RegisterBaitCards(
                CardId.ReinforcementOfTheArmy,
                CardId.PotOfDesires,
                CardId.ForbiddenCrown,
                CardId.TheFallenAndTheVirtuous,
                CardId.RadiantTyphoonVision,
                CardId.MysticalSpaceTyphoon
            );

            // 3. High Value Chain Targets
            ChainAdvisor.RegisterHighValueTargets(
                CardId.Engage,
                CardId.Kagari,
                CardId.Linkage,
                CardId.WidowAnchor,
                CardId.Zero,
                CardId.SPLittleKnight
            );
        }

        private void RegisterComboLines()
        {
            // Route 1: Engage - Zero / Kagari Advantage Loop
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Engage-Zero-Loop",
                RequiredCards = new List<int> { CardId.Engage },
                FallbackLineName = "Raye-Hayate-Dump",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Engage, ActionType = ExecutorType.Activate, Description = "Activate Engage to search Linkage or Widow Anchor" },
                    new() { CardId = CardId.Kagari, ActionType = ExecutorType.SpSummon, Description = "Link summon Kagari" },
                    new() { CardId = CardId.Kagari, ActionType = ExecutorType.Activate, Description = "Kagari retrieve Engage from GY" },
                    new() { CardId = CardId.Engage, ActionType = ExecutorType.Activate, Description = "Re-activate Engage with 3+ Spells for draw + search" }
                },
                EndBoardScore = 95
            });

            // Route 2: Raye -> Hayate Direct Attack & GY Dump
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Raye-Hayate-Dump",
                RequiredCards = new List<int> { CardId.Raye },
                FallbackLineName = "HornetDrones-Starter",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Raye, ActionType = ExecutorType.Summon, Description = "Normal Summon Raye" },
                    new() { CardId = CardId.Hayate, ActionType = ExecutorType.SpSummon, Description = "Link summon Hayate" },
                    new() { CardId = CardId.Hayate, ActionType = ExecutorType.Activate, Description = "Hayate direct attack & dump Engage/Raye to GY" },
                    new() { CardId = CardId.Shizuku, ActionType = ExecutorType.SpSummon, Description = "Link Shizuku in Main 2 for End Phase search" }
                },
                EndBoardScore = 90
            });

            // Route 3: Hornet Drones 1-Card Starter
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "HornetDrones-Starter",
                RequiredCards = new List<int> { CardId.HornetDrones },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.HornetDrones, ActionType = ExecutorType.Activate, Description = "Activate Hornet Drones for Token" },
                    new() { CardId = CardId.Kagari, ActionType = ExecutorType.SpSummon, Description = "Link Kagari using Token" },
                    new() { CardId = CardId.Kagari, ActionType = ExecutorType.Activate, Description = "Kagari retrieve Engage / Widow Anchor" }
                },
                EndBoardScore = 85
            });

            // Route 4: Stolen Monster Zeke / Azalea Link Push
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "WidowAnchor-Zeke-Removal",
                RequiredCards = new List<int> { CardId.WidowAnchor },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.WidowAnchor, ActionType = ExecutorType.Activate, Description = "Steal opponent threat monster" },
                    new() { CardId = CardId.Zeke, ActionType = ExecutorType.SpSummon, Description = "Link stolen monster into Zeke" },
                    new() { CardId = CardId.Zeke, ActionType = ExecutorType.Activate, Description = "Zeke banish another enemy threat" }
                },
                EndBoardScore = 92,
                Condition = () => Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled())
            });
        }

        private void RegisterExecutors()
        {
            // --- I. Hand Traps & Fast Interruptions ---
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);

            // Preemptive quick responses
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownPreemptiveEffect);
            AddExecutor(ExecutorType.Activate, CardId.WidowAnchor, WidowAnchorPreemptiveEffect);

            // --- II. Board Breakers, Starters & Draw Spells ---
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.MysticalSpaceTyphoon, MysticalSpaceTyphoonEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfDesires, PotOfDesiresEffect);
            AddExecutor(ExecutorType.Activate, CardId.RadiantTyphoonVision, RadiantTyphoonVisionEffect);
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotAEffect);
            AddExecutor(ExecutorType.Activate, CardId.HornetDrones, HornetDronesEffect);
            AddExecutor(ExecutorType.Activate, CardId.Engage, EngageEffect);
            AddExecutor(ExecutorType.SpellSet, SpellSetForLinkage);
            AddExecutor(ExecutorType.Activate, CardId.Linkage, LinkageEffect);
            AddExecutor(ExecutorType.Activate, CardId.Lemnisgate, LemnisgateEffect);

            // --- III. Main Deck Monsters ---
            AddExecutor(ExecutorType.Summon, CardId.Raye, RayeSummon);
            AddExecutor(ExecutorType.Activate, CardId.Raye, RayeEffect);
            AddExecutor(ExecutorType.Summon, CardId.Roze, RozeSummon);
            AddExecutor(ExecutorType.Activate, CardId.Roze, RozeEffect);

            // --- IV. Extra Deck Bosses & Links ---
            // Board Breaker: TY-PHON
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHON, TYPHONSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperStarslayerTYPHON, TYPHONEffect);

            // Sky Striker Ace = Zero (Link-2 Core Boss)
            AddExecutor(ExecutorType.SpSummon, CardId.Zero, ZeroSummon);
            AddExecutor(ExecutorType.Activate, CardId.Zero, ZeroEffect);

            // Generic Link Disruption: S:P Little Knight
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPEffect);

            // Link-2 Removal: Zeke & Azalea
            AddExecutor(ExecutorType.SpSummon, CardId.Zeke, ZekeSummon);
            AddExecutor(ExecutorType.Activate, CardId.Zeke, ZekeEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Azalea, AzaleaSummon);
            AddExecutor(ExecutorType.Activate, CardId.Azalea, AzaleaEffect);

            // Link-1 Aces
            AddExecutor(ExecutorType.SpSummon, CardId.Kagari, KagariSummon);
            AddExecutor(ExecutorType.Activate, CardId.Kagari, KagariEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Shizuku, ShizukuSummon);
            AddExecutor(ExecutorType.Activate, CardId.Shizuku, ShizukuEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Hayate, HayateSummon);
            AddExecutor(ExecutorType.Activate, CardId.Hayate, HayateEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Kaina, KainaSummon);
            AddExecutor(ExecutorType.Activate, CardId.Kaina, KainaEffect);

            // --- V. Traps / Set Quick-Plays ---
            AddExecutor(ExecutorType.SpellSet, CardId.WidowAnchor);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenCrown);
            AddExecutor(ExecutorType.SpellSet, CardId.Lemnisgate);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.SpellSet, CardId.TheFallenAndTheVirtuous);
            AddExecutor(ExecutorType.SpellSet, CardId.MysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, CardId.WidowAnchor, WidowAnchorEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownEffect);

            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 1. Material Protection & Positioning Safeguards
        // ═══════════════════════════════════════════════════════════════

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            // Sky Striker Ace monsters are ALWAYS allowed
            if (IsAceCard(card)) return true;

            // Generic extra deck summons (S:P, TY-PHON)
            if (card.HasType(CardType.Link))
            {
                var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
                if (activeAces.Count > 0)
                {
                    if (CanDealLethal() || OpponentHasActiveNegator() || Enemy.GetMonsterCount() >= 2)
                        return true;

                    DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning generic Link {card.Name} skipped to protect active Ace {activeAces.First().Name}");
                    return false;
                }
            }
            return true;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.AshBlossom || cardId == CardId.GhostBelle || cardId == CardId.DrollAndLockBird || cardId == CardId.MaxxC)
            {
                if (positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
            }

            if (cardId == CardId.SuperStarslayerTYPHON)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            if (IsAceCard(Bot.Hand.FirstOrDefault(c => c != null && c.Id == cardId) ?? Bot.MonsterZone.FirstOrDefault(c => c != null && c.Id == cardId)))
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            if (cardId == CardId.Raye || cardId == CardId.Roze)
            {
                if (Util.IsTurn1OrMain2() && positions.Contains(CardPosition.FaceUpDefence))
                    return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;

            if (Card.IsCode(CardId.Raye, CardId.Roze))
            {
                if (Card.IsDefense() && (CanDealLethal() || Enemy.GetMonsterCount() == 0))
                    return true;
                if (Card.IsAttack() && Util.IsTurn1OrMain2() && Enemy.GetMonsterCount() > 0)
                    return true;
            }

            return base.DefaultMonsterRepos();
        }

        protected int GetSpellCountInGrave() =>
            Bot.Graveyard.Count(c => c != null && c.IsSpell());

        protected bool HasThreeOrMoreSpellsInGrave() =>
            GetSpellCountInGrave() >= 3;

        protected bool EmptyMainMonsterZone()
        {
            for (int i = 0; i < 5; i++)
            {
                if (Bot.MonsterZone[i] != null)
                    return false;
            }
            return true;
        }

        protected int MainMonsterZoneCount()
        {
            int count = 0;
            for (int i = 0; i < 5; i++)
            {
                if (Bot.MonsterZone[i] != null) count++;
            }
            return count;
        }

        protected override bool IsBoardStrongEnough()
        {
            int disruptionCount = 0;
            bool hasSkyStrikerLink = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));
            if (hasSkyStrikerLink) disruptionCount++;

            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.WidowAnchor))) disruptionCount += 2;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.ForbiddenCrown))) disruptionCount += 2;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.Lemnisgate))) disruptionCount++;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.TheFallenAndTheVirtuous))) disruptionCount++;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.CalledByTheGrave))) disruptionCount++;

            if (Bot.HasInHand(CardId.AshBlossom) || Bot.HasInHand(CardId.GhostBelle) || Bot.HasInHand(CardId.DrollAndLockBird)) disruptionCount++;

            return disruptionCount >= 3;
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
            {
                if (ResourcePlan.NibiruCheckpoint(Duel.Turn, Enemy.Hand.Count, Bot.GetMonsterCount(), HasNegateOnField()) || OpponentHasActiveNegator())
                    return true;
            }
            return base.ShouldStopExtending();
        }

        protected override bool NeedsBoardPresence() =>
            !Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));

        public override void OnChaining(int player, ClientCard card)
        {
            if (player == 1 && card != null && card.IsMonster() && Duel.Player == 0)
                _hasOpponentActivatedMonsterEffect = true;

            if (player == 0 && card != null && card.Id == CardId.Kagari)
                _kagariUsedThisTurn = true;

            base.OnChaining(player, card);
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 2. Event Callbacks & Decision Handlers
        // ═══════════════════════════════════════════════════════════════

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            // Rule 14: Default to false for opponent cards
            if (card == null || card.Controller == 1) return false;

            if (card.IsCode(CardId.Zero)) return true;
            if (card.IsCode(CardId.Kagari)) return true;
            if (card.IsCode(CardId.Shizuku)) return true;
            if (card.IsCode(CardId.Hayate)) return true;
            if (card.IsCode(CardId.Raye)) return true;
            if (card.IsCode(CardId.Roze)) return true;
            if (card.IsCode(CardId.AlbionTheBrandedDragon)) return true;
            if (card.IsCode(CardId.Lemnisgate)) return true;

            return base.OnSelectEffectYn(card, desc);
        }

        private long StringId(int cardId, int optionIndex)
        {
            return (long)cardId * 16 + optionIndex;
        }

        public override int OnSelectOption(IList<long> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                long option = options[i];

                // Triple Tactics Talent
                if (option == StringId(CardId.TripleTacticsTalent, 0)) // Draw 2
                {
                    if (Bot.Hand.Count <= 3 || Util.IsTurn1OrMain2()) return i;
                }
                if (option == StringId(CardId.TripleTacticsTalent, 1)) // Take control
                {
                    if (Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled())) return i;
                }
                if (option == StringId(CardId.TripleTacticsTalent, 2)) // Look hand & rip
                {
                    if (Duel.Turn <= 2 && Enemy.Hand.Count >= 4) return i;
                }

                // Sky Striker Ace = Zero
                if (option == StringId(CardId.Zero, 0)) // Search Sky Striker Spell
                {
                    return i;
                }
                if (option == StringId(CardId.Zero, 1)) // Quick Effect: Tribute & summon Raye/Roze + destroy
                {
                    if (Duel.Player == 1 || Duel.Phase == DuelPhase.Battle) return i;
                }

                // Radiant Typhoon Vision
                if (option == StringId(CardId.RadiantTyphoonVision, 2) || option == 0) // Draw 2 & discard Quick-Play
                {
                    bool hasQuickPlay = Bot.Hand.Any(c => c != null && c.IsSpell() && c.HasType(CardType.QuickPlay) && c.Id != CardId.RadiantTyphoonVision);
                    if (hasQuickPlay) return i;
                }
                if (option == StringId(CardId.RadiantTyphoonVision, 3) || option == 1) // Search MST
                {
                    bool enemyHasBackrow = Enemy.GetSpells().Any(c => c != null && c.IsFaceup());
                    if (enemyHasBackrow) return i;
                }
            }

            return base.OnSelectOption(options);
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 3. Main Spells & Board Breakers
        // ═══════════════════════════════════════════════════════════════

        private bool EngageEffect()
        {
            if (OpponentHasActiveNegator()) return false;
            if (IsMain1SearchDeferred()) return false;
            if (!EmptyMainMonsterZone()) return false;

            bool hasMonster = Bot.Hand.Any(c => c != null && c.IsMonster() && (c.Id == CardId.Raye || c.Id == CardId.Roze))
                              || Bot.GetMonsters().Any(c => c != null);

            if (!hasMonster)
            {
                AI.SelectCard(CardId.Raye, CardId.HornetDrones, CardId.Linkage, CardId.WidowAnchor);
            }
            else
            {
                AI.SelectCard(CardId.Linkage, CardId.WidowAnchor, CardId.Lemnisgate, CardId.HornetDrones, CardId.Raye, CardId.Roze);
            }
            return true;
        }

        private bool HornetDronesEffect()
        {
            if (OpponentHasActiveNegator()) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (IsMain1SearchDeferred()) return false;
            return EmptyMainMonsterZone();
        }

        private bool LinkageEffect()
        {
            if (OpponentHasActiveNegator()) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (!CanActivateLinkage()) return false;

            bool hasSSMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));
            var targetedAce = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsAceCard(c) && Util.IsChainTarget(c));

            // 1. Chain Dodging: If opponent targets an Ace on chain, dodge with Linkage!
            if (Duel.CurrentChain.Any(c => c != null && c.Controller == 1) && hasSSMonster)
            {
                if (targetedAce != null)
                {
                    AI.SelectCard(targetedAce);
                    if (!_kagariUsedThisTurn && (Bot.HasInGraveyard(CardId.Engage) || Bot.HasInGraveyard(CardId.WidowAnchor) || Bot.HasInGraveyard(CardId.Linkage)))
                        AI.SelectNextCard(CardId.Kagari, CardId.Zero, CardId.Shizuku, CardId.Hayate);
                    else
                        AI.SelectNextCard(CardId.Zero, CardId.Shizuku, CardId.Kagari, CardId.Hayate);
                    return true;
                }
            }

            // 2. Battle Phase Extra Attack / Lethal Push
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
            {
                if (hasSSMonster)
                {
                    if (!_kagariUsedThisTurn && (Bot.HasInGraveyard(CardId.Engage) || Bot.HasInGraveyard(CardId.WidowAnchor)))
                        AI.SelectCard(CardId.Kagari, CardId.Zero, CardId.Shizuku);
                    else
                        AI.SelectCard(CardId.Zero, CardId.Kagari, CardId.Shizuku);
                    return true;
                }
            }

            // 3. Main Phase extender / Setup
            if (EmptyMainMonsterZone() || Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Hayate)))
            {
                if (!_kagariUsedThisTurn && (Bot.HasInGraveyard(CardId.Engage) || Bot.HasInGraveyard(CardId.WidowAnchor) || Bot.HasInGraveyard(CardId.Linkage)))
                    AI.SelectCard(CardId.Kagari, CardId.Zero, CardId.Shizuku);
                else
                    AI.SelectCard(CardId.Zero, CardId.Shizuku, CardId.Kagari);
                return true;
            }

            return false;
        }

        private bool RotAEffect()
        {
            if (OpponentHasActiveNegator()) return false;
            if (IsMain1SearchDeferred()) return false;
            AI.SelectCard(CardId.Raye, CardId.Roze);
            return true;
        }

        private bool TheFallenAndTheVirtuousEffect()
        {
            if (OpponentHasActiveNegator()) return false;

            // Target Verification Safeguard (Rule 15):
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
            if (target == null)
                target = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());

            // Effect 1: Send Albion (87746184) from Extra Deck to destroy 1 face-up enemy card!
            if (target != null && (Bot.HasInExtra(CardId.AlbionTheBrandedDragon) || Bot.HasInExtra(CardId.EcclesiaAndTheDarkDragon)))
            {
                AI.SelectCard(CardId.AlbionTheBrandedDragon, CardId.EcclesiaAndTheDarkDragon);
                AI.SelectNextCard(target);
                return true;
            }

            // Effect 2: If Ecclesia is in field/GY, revive 1 monster from either GY
            bool hasEcclesia = Bot.HasInMonstersZone(CardId.EcclesiaAndTheDarkDragon) || Bot.HasInGraveyard(CardId.EcclesiaAndTheDarkDragon);
            if (hasEcclesia && !IsSpecialSummonBlocked())
            {
                ClientCard targetSummon = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && c.Attack >= 2000)
                                       ?? Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());
                if (targetSummon != null)
                {
                    AI.SelectCard(targetSummon);
                    return true;
                }
            }

            return false;
        }

        private bool RadiantTyphoonVisionEffect()
        {
            if (OpponentHasActiveNegator()) return false;
            if (IsMain1SearchDeferred()) return false;

            bool hasQuickPlayInHand = Bot.Hand.Any(c => c != null && c != Card && c.IsSpell() && c.HasType(CardType.QuickPlay));
            if (hasQuickPlayInHand || Bot.Hand.Count <= 2)
            {
                AI.SelectOption(0); // Draw 2
                return true;
            }

            // Otherwise, search MST if we don't have MST in hand
            if (!Bot.HasInHand(CardId.MysticalSpaceTyphoon) && (Bot.Deck.Any(c => c != null && c.Id == CardId.MysticalSpaceTyphoon) || Bot.HasInGraveyard(CardId.MysticalSpaceTyphoon)))
            {
                AI.SelectOption(1); // Search MST
                AI.SelectCard(CardId.MysticalSpaceTyphoon);
                return true;
            }

            return false;
        }

        private bool MysticalSpaceTyphoonEffect()
        {
            // Rule 15: Check enemy spell/trap exists before returning true!
            ClientCard stTarget = Enemy.GetSpells()
                .Where(c => c != null && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                .OrderByDescending(c => c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field)) ? 100 : 10)
                .FirstOrDefault();

            if (stTarget != null)
            {
                AI.SelectCard(stTarget);
                return true;
            }
            return false;
        }

        private bool PotOfDesiresEffect()
        {
            if (Bot.Deck.Count < 12) return false;
            bool hasPlaymaker = Bot.Hand.Any(c => c != null &&
                (c.Id == CardId.Raye || c.Id == CardId.Roze ||
                 c.Id == CardId.Engage || c.Id == CardId.HornetDrones ||
                 c.Id == CardId.ReinforcementOfTheArmy || c.Id == CardId.Linkage));

            bool hasMonsterOnField = Bot.GetMonsters().Any(c => c != null);
            if (hasPlaymaker || hasMonsterOnField) return false;
            return Bot.Hand.Count <= 2;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (ShouldSkipCombo()) return false;
            return Duel.Player == 0 && _hasOpponentActivatedMonsterEffect;
        }

        private bool LemnisgateEffect()
        {
            // In GY trigger: when Sky Striker summoned, Link Summon
            if (Card.Location == CardLocation.Grave)
            {
                if (Duel.Player == 1) return true;
                if (Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Main2) return true;
                if (Duel.CurrentChain.Any(c => c != null && c.Controller == 1)) return true;
                return false;
            }

            if (OpponentHasActiveNegator()) return false;
            if (Card.Location == CardLocation.Hand && Bot.HasInSpellZone(CardId.Lemnisgate, faceUp: true))
                return false;

            var monsters = Bot.GetGraveyardMonsters()
                .Where(c => c.IsCode(CardId.Raye, CardId.Roze, CardId.Kagari, CardId.Shizuku, CardId.Hayate, CardId.Zero))
                .OrderBy(c => {
                    if (c.IsCode(CardId.Kagari)) return 1;
                    if (c.IsCode(CardId.Zero)) return 2;
                    if (c.IsCode(CardId.Shizuku)) return 3;
                    return 10;
                })
                .ToList();

            var spells = Bot.GetGraveyardSpells()
                .Where(c => c.Name != null && c.Name.Contains("Sky Striker"))
                .OrderBy(c => {
                    if (c.IsCode(CardId.Engage)) return 1;
                    if (c.IsCode(CardId.Linkage)) return 2;
                    if (c.IsCode(CardId.WidowAnchor)) return 3;
                    return 10;
                })
                .ToList();

            if (monsters.Count > 0 && spells.Count > 0)
            {
                int countToReturn = Math.Min(monsters.Count, spells.Count);
                var targetMonsters = monsters.Take(countToReturn).ToList();
                var targetSpells = spells.Take(countToReturn).ToList();
                var allTargets = new List<ClientCard>();
                allTargets.AddRange(targetMonsters);
                allTargets.AddRange(targetSpells);
                AI.SelectCard(allTargets);
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 4. Fast Interruptions & Hand Traps
        // ═══════════════════════════════════════════════════════════════

        private bool ForbiddenCrownPreemptiveEffect()
        {
            if (_crownUsed) return false;
            if (Duel.LastChainPlayer == 0) return false;

            if (Duel.LastChainPlayer == 1 && LastChainCard != null && LastChainCard.Location == CardLocation.MonsterZone &&
                !LastChainCard.IsDisabled() && !LastChainCard.IsShouldNotBeTarget() && !LastChainCard.IsShouldNotBeSpellTrapTarget())
            {
                AI.SelectCard(LastChainCard);
                _crownUsed = true;
                return true;
            }

            if (Duel.LastChainPlayer == -1 || Duel.Player == 1)
            {
                ClientCard target = Enemy.MonsterZone.GetMonsters()
                    .Where(c => c != null && IsThreatMonster(c) && !c.IsDisabled() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (target != null)
                {
                    AI.SelectCard(target);
                    _crownUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool ForbiddenCrownEffect() => ForbiddenCrownPreemptiveEffect();

        private bool WidowAnchorPreemptiveEffect()
        {
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;
            if (OpponentHasActiveNegator()) return false;

            ClientCard target = Enemy.MonsterZone.GetMonsters()
                .Where(c => c != null && IsThreatMonster(c) && !c.IsDisabled() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault() ?? Util.GetProblematicEnemyMonster(0, true);

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool WidowAnchorEffect()
        {
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            if (Duel.LastChainPlayer == 0) return false;

            if (Duel.LastChainPlayer == 1 && LastChainCard != null && LastChainCard.Location == CardLocation.MonsterZone &&
                !LastChainCard.IsDisabled() && !LastChainCard.IsShouldNotBeTarget() && !LastChainCard.IsShouldNotBeSpellTrapTarget())
            {
                AI.SelectCard(LastChainCard);
                return true;
            }

            return WidowAnchorPreemptiveEffect();
        }

        private bool MaxxCEffect()
        {
            if (Duel.LastChainPlayer == 0) return false;
            if (!SmartHandTrapChain()) return false;
            return Card.Location == CardLocation.Hand;
        }

        private bool AshBlossomEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (!SmartHandTrapChain()) return false;
            ClientCard lastCard = Util.GetLastChainCard();
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool GhostBelleEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (!SmartHandTrapChain()) return false;
            ClientCard lastCard = Util.GetLastChainCard();
            return lastCard != null && lastCard.Controller == 1;
        }

        private bool CalledByTheGraveEffect()
        {
            return Duel.LastChainPlayer == 1 && DefaultCalledByTheGrave();
        }

        private bool DrollAndLockBirdEffect()
        {
            if (Duel.Player != 1 || Duel.LastChainPlayer != 1) return false;
            return SmartHandTrapChain();
        }

        private bool IsThreatMonster(ClientCard c)
        {
            if (c == null || !c.IsFaceup() || c.IsDisabled()) return false;
            if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) return true;
            if (c.Attack >= 2000) return true;
            if (c.HasType(CardType.Effect) && (c.Level >= 5 || c.Rank >= 5)) return true;
            return false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 5. Extra Deck & Monster Logic
        // ═══════════════════════════════════════════════════════════════

        private bool RayeSummon() => EmptyMainMonsterZone();

        private bool RayeEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location == CardLocation.Grave)
                return true;

            if (Card.IsDisabled()) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2 && Duel.Phase != DuelPhase.Battle) return false;

            bool isChainTarget = Util.IsChainTarget(Card);
            bool shouldActivate = false;

            if (Duel.Player == 0)
            {
                if (isChainTarget) shouldActivate = true;
                else if (Duel.Phase == DuelPhase.Main2) shouldActivate = true;
                else if (Duel.Phase == DuelPhase.Battle && Bot.BattlingMonster == Card) shouldActivate = true;
            }
            else
            {
                if (isChainTarget || Enemy.GetMonsterCount() > 0 || Duel.Phase == DuelPhase.Main2) shouldActivate = true;
            }

            if (!shouldActivate) return false;

            var targets = new List<int>();
            if (Duel.Player == 1)
            {
                if (!Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Kaina)))
                    targets.Add(CardId.Kaina);
                targets.Add(CardId.Shizuku);
                targets.Add(CardId.Zero);
                targets.Add(CardId.Kagari);
            }
            else
            {
                targets.Add(CardId.Shizuku);
                targets.Add(CardId.Kagari);
                targets.Add(CardId.Zero);
                targets.Add(CardId.Hayate);
            }

            AI.SelectCard(targets);
            return true;
        }

        private bool RozeSummon() => EmptyMainMonsterZone();
        private bool RozeEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave;
        }

        private bool TYPHONSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Duel.Phase == DuelPhase.Main2 && Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.Attack >= 2500 || m.IsExtraCard()));
        }

        private bool TYPHONEffect()
        {
            ClientCard target = Enemy.GetMonsters().OrderByDescending(m => m.Attack).FirstOrDefault(m => m != null && m.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ZeroSummon() => !IsSpecialSummonBlocked();
        private bool ZeroEffect()
        {
            int opt = (int)ActivateDescription;

            // Trigger on summon: Search Sky Striker Spell from Deck or GY
            if (opt == StringId(CardId.Zero, 0) || opt == -1 || opt == 0)
            {
                return true;
            }

            // Quick Effect: Tribute self -> summon Raye + Roze -> destroy 1 card
            if (opt == StringId(CardId.Zero, 1))
            {
                if (IsSpecialSummonBlocked()) return false;

                // Rule 15 Target verification
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
                AI.SelectCard(CardId.Zero, CardId.Raye, CardId.Roze);
                AI.SelectNextCard(CardId.Kagari, CardId.Shizuku, CardId.Hayate, CardId.Raye, CardId.Roze);
                if (target != null)
                {
                    AI.SelectNextCard(target);
                }
                return true;
            }
            return false;
        }

        private bool SPSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var monsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            if (monsters.Count < 2) return false;
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool SPEffect()
        {
            ClientCard target = Util.GetProblematicEnemyCard()
                ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup())
                ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ZekeSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Best to summon Zeke using a stolen enemy monster or Roze
            return Bot.GetMonsters().Any(card => card != null && (card.Controller == 1 || card.IsCode(CardId.Roze) || card.IsCode(CardId.Token)));
        }

        private bool ZekeEffect()
        {
            ClientCard enemyTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            if (enemyTarget != null)
            {
                AI.SelectCard(enemyTarget);
                return true;
            }
            return false;
        }

        private bool AzaleaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool AzaleaEffect()
        {
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                                ?? Enemy.GetSpells().FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool KagariSummon()
        {
            if (IsSpecialSummonBlocked() || _kagariUsedThisTurn) return false;
            return Bot.Graveyard.Any(c => c != null && c.IsSpell() &&
                (c.IsCode(CardId.Engage, CardId.Linkage, CardId.WidowAnchor, CardId.HornetDrones, CardId.Lemnisgate)));
        }

        private bool KagariEffect()
        {
            AI.SelectCard(CardId.Engage, CardId.Linkage, CardId.WidowAnchor, CardId.HornetDrones, CardId.Lemnisgate);
            return true;
        }

        private bool ShizukuSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Util.IsTurn1OrMain2() && !Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Shizuku));
        }

        private bool ShizukuEffect()
        {
            var targets = new List<int> { CardId.Engage, CardId.Linkage, CardId.WidowAnchor, CardId.HornetDrones, CardId.Lemnisgate };
            var missingFromGrave = targets.Where(id => !Bot.HasInGraveyard(id)).ToList();
            if (missingFromGrave.Count > 0)
                AI.SelectCard(missingFromGrave);
            else
                AI.SelectCard(targets);
            return true;
        }

        private bool HayateSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return !Util.IsTurn1OrMain2() && !Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Hayate));
        }

        private bool HayateEffect()
        {
            if (!Bot.HasInGraveyard(CardId.Raye))
                AI.SelectCard(CardId.Raye);
            else
                AI.SelectCard(CardId.Engage, CardId.Linkage, CardId.WidowAnchor);
            return true;
        }

        private bool KainaSummon() => false;
        private bool KainaEffect()
        {
            ClientCard target = Enemy.MonsterZone.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SpellSetForLinkage()
        {
            if (Bot.GetFieldCount() > 0) return false;
            bool hasLinkageInHand = Bot.Hand.Any(c => c != null && c.Id == CardId.Linkage);
            if (!hasLinkageInHand) return false;

            ClientCard cardToSet = Bot.Hand.FirstOrDefault(c => c != null && c.IsSpell() && !c.HasType(CardType.QuickPlay) && c.Id != CardId.Linkage)
                                ?? Bot.Hand.FirstOrDefault(c => c != null && c.IsSpell() && !c.HasType(CardType.QuickPlay));
            if (cardToSet != null)
            {
                AI.SelectCard(cardToSet);
                return true;
            }
            return false;
        }

        private bool CanActivateLinkage()
        {
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;

            bool isBattlePhase = Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle;
            bool hasEMZMonster = Bot.MonsterZone[5] != null || Bot.MonsterZone[6] != null;
            bool hasSTCard = Bot.GetSpellCount() >= 1;

            if (isBattlePhase && (hasEMZMonster || Bot.GetMonsterCount() > 0))
            {
                ClientCard attackingMonster = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsAceCard(m));
                if (attackingMonster != null)
                {
                    AI.SelectCard(attackingMonster);
                    return true;
                }
            }

            int mmzCount = MainMonsterZoneCount();
            if (mmzCount > 0) return false;

            return hasEMZMonster || hasSTCard;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 6. Selection Overrides & Universal Protections
        // ═══════════════════════════════════════════════════════════════

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Rule 1: Search card to hand
            if (hint == 506 || (cards != null && cards.Count > 0 && cards.All(c => c != null && c.Location == CardLocation.Deck)))
            {
                bool hasMonsterOnField = Bot.GetMonsters().Any(c => c != null);
                bool hasStarterInHand = Bot.Hand.Any(c => c != null && (c.Id == CardId.Raye || c.Id == CardId.Roze || c.Id == CardId.HornetDrones));

                if (!hasMonsterOnField && !hasStarterInHand)
                {
                    var starter = cards.FirstOrDefault(c => c != null && (c.Id == CardId.Raye || c.Id == CardId.HornetDrones || c.Id == CardId.Roze));
                    if (starter != null) return new List<ClientCard> { starter };
                }

                var priority = new[] { CardId.Engage, CardId.Linkage, CardId.WidowAnchor, CardId.HornetDrones, CardId.Raye, CardId.Lemnisgate };
                foreach (int id in priority)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                    if (match != null) return new List<ClientCard> { match };
                }
            }

            // Rule 1 & Anti-Pattern 1: Removal / Destruction must prioritize enemy cards!
            if (hint == 502 || hint == 503)
            {
                var enemyTargets = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyTargets.Count >= min)
                    return enemyTargets.Take(max).ToList();
            }

            // Extra Deck Send cost for The Fallen & The Virtuous
            if (hint == 504 && cards.Any(c => c != null && c.Location == CardLocation.Extra))
            {
                var albion = cards.FirstOrDefault(c => c != null && c.Id == CardId.AlbionTheBrandedDragon);
                if (albion != null) return new List<ClientCard> { albion };
                var ecclesia = cards.FirstOrDefault(c => c != null && c.Id == CardId.EcclesiaAndTheDarkDragon);
                if (ecclesia != null) return new List<ClientCard> { ecclesia };
            }

            // Zero search
            if (Card != null && Card.Id == CardId.Zero)
            {
                if (cards.Any(c => c != null && c.IsSpell()))
                {
                    var priority = new[] { CardId.Engage, CardId.Linkage, CardId.WidowAnchor, CardId.HornetDrones, CardId.Lemnisgate };
                    foreach (int id in priority)
                    {
                        var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                        if (match != null) return new List<ClientCard> { match };
                    }
                }
            }

            // Linkage selection
            if (Card != null && Card.Id == CardId.Linkage)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Extra))
                {
                    bool canUseKagari = !_kagariUsedThisTurn && Bot.Graveyard.Any(sp => sp != null && sp.IsSpell() && (sp.Id == CardId.Engage || sp.Id == CardId.Linkage || sp.Id == CardId.WidowAnchor));
                    if (canUseKagari)
                    {
                        var kagari = cards.FirstOrDefault(c => c != null && c.Id == CardId.Kagari);
                        if (kagari != null) return new List<ClientCard> { kagari };
                    }

                    var priorityAces = new[] { CardId.Zero, CardId.Kagari, CardId.Hayate, CardId.Shizuku };
                    foreach (int id in priorityAces)
                    {
                        var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                        if (match != null) return new List<ClientCard> { match };
                    }
                }
            }

            // Radiant Typhoon Vision discard
            if (Card != null && Card.Id == CardId.RadiantTyphoonVision)
            {
                var qps = cards.Where(c => c.IsSpell() && c.HasType(CardType.QuickPlay)).OrderBy(c => {
                    if (c.IsCode(CardId.Lemnisgate)) return 1;
                    if (c.IsCode(CardId.WidowAnchor)) return 2;
                    return 10;
                }).ToList();
                if (qps.Count > 0) return new List<ClientCard> { qps.First() };
            }

            // Material selection: preserve Ace cards
            if (hint == 511 || hint == 513 || hint == 533)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                if (sorted.Count >= min)
                    return Util.CheckSelectCount(sorted.Take(max).ToList(), cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(CardId.Kagari) ||
                   card.IsCode(CardId.Shizuku) ||
                   card.IsCode(CardId.Hayate) ||
                   card.IsCode(CardId.Kaina) ||
                   card.IsCode(CardId.Zero) ||
                   card.IsCode(CardId.Zeke) ||
                   card.IsCode(CardId.Azalea) ||
                   card.IsCode(CardId.SPLittleKnight) ||
                   card.IsCode(CardId.SuperStarslayerTYPHON);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            // Stolen monster: Use FIRST!
            if (c.Controller == 1) return 1;
            if (c.IsCode(CardId.Token)) return 5;
            if (c.IsCode(CardId.Roze)) return 20;
            if (c.IsCode(CardId.Raye)) return 30;
            if (IsAceCard(c)) return 950;
            return 100;
        }

        public override BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            if (attacker == null || defenders == null || defenders.Count == 0) return AI.Attack(attacker, null);

            int GetDefenseValue(ClientCard c)
            {
                if (c == null) return 0;
                return c.IsDefense() ? c.Defense : c.Attack;
            }

            if (attacker.IsCode(CardId.Hayate))
            {
                var weak = defenders.FirstOrDefault(d => d != null && d.IsMonster() && attacker.Attack > GetDefenseValue(d));
                return AI.Attack(attacker, weak);
            }

            var beatableDefender = defenders.Where(d => d != null && d.IsMonster() && attacker.Attack > GetDefenseValue(d))
                                            .OrderBy(d => GetDefenseValue(d)).FirstOrDefault();
            if (beatableDefender != null) return AI.Attack(attacker, beatableDefender);

            return null;
        }
    }
}
