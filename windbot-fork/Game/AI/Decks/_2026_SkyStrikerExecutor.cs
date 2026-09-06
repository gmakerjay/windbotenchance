using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.DecisionEngine;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_SkyStriker", "2026_SkyStriker")]
    public class _2026_SkyStrikerExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Engine
            public const int Raye = 26077387;
            public const int Roze = 37351133;
            public const int Kagari = 63288573;
            public const int Shizuku = 90673288;
            public const int Hayate = 8491308;
            public const int Kaina = 12421694;
            public const int Token = 52340445;

            // Spells
            public const int Engage = 63166095;
            public const int HornetDrones = 52340444;
            public const int WidowAnchor = 98338152;
            public const int SharkCannon = 51227866;
            public const int ReinforcementOfTheArmy = 32807846;

            // 2026 Cards & Extra Deck
            public const int Zero = 76072561;
            public const int EngageZero = 17217034;
            public const int Camellia = 63013339;
            public const int Linkage = 9726840;
            public const int RadiantTyphoonVision = 20508881;
            public const int Lemnisgate = 34433770;
            public const int TheFallenAndTheVirtuous = 30271097;
            public const int EcclesiaAndTheDarkDragon = 78397661;
            public const int AlbionTheBrandedDragon = 87746184;
            public const int FallenOfAlbaz = 68468459;
            public const int Zeke = 75147529;
            public const int Azalea = 98462037;
            public const int AzaleaTemperance = 56741506;
            public const int Amatsu = 25072579;

            // Generic Extra Deck Finishers
            public const int AccesscodeTalker = 86066372;
            public const int SPLittleKnight = 29301450;

            // Hand Traps & Staples
            public const int AshBlossom = 14558127;
            public const int AshBlossomAlt = 14558128;
            public const int GhostBelle = 73642296;
            public const int DrollAndLockBird = 94145021;
            public const int CalledByTheGrave = 24224830;
            public const int PotOfDesires = 35261759;
            public const int ForbiddenDroplet = 24299458;
            public const int CrossoutDesignator = 65681983;
            public const int TripleTacticsTalent = 25311006;
            public const int TripleTacticsThrust = 35269904;
            public const int ForbiddenCrown = 98829635;
            public const int DominusImpulse = 40366667;
            public const int MysticalSpaceTyphoon = 5318639;
        }

        private static readonly int[] SkyStrikerLinks = {
            CardId.Kagari, CardId.Shizuku, CardId.Hayate, CardId.Kaina,
            CardId.Zero, CardId.EngageZero, CardId.Camellia, CardId.Zeke,
            CardId.Azalea, CardId.AzaleaTemperance, CardId.Amatsu
        };

        private bool _hasOpponentActivatedMonsterEffect = false;
        private bool _crownUsed = false;
        private bool _dominusImpulseUsed = false;
        private bool _kagariUsedThisTurn = false;

        public _2026_SkyStrikerExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace cards to protect them from generic material consumption
            ResourcePlan.RegisterAceCards(SkyStrikerLinks);

            // ── Combo Router: High-Level Sequencing Lines ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Engage-Kagari-Loop",
                RequiredCards = new List<int> { CardId.Engage },
                FallbackLineName = "Raye-Hayate-Dump",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Engage, ActionType = ExecutorType.Activate, Description = "Activate Engage to search Linkage or Widow Anchor" },
                    new() { CardId = CardId.Kagari, ActionType = ExecutorType.SpSummon, Description = "Link summon Kagari" },
                    new() { CardId = CardId.Kagari, ActionType = ExecutorType.Activate, Description = "Kagari retrieve Engage from GY" },
                    new() { CardId = CardId.Engage, ActionType = ExecutorType.Activate, Description = "Re-activate Engage with 3+ Spells for draw + search" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Raye-Hayate-Dump",
                RequiredCards = new List<int> { CardId.Raye },
                FallbackLineName = "HornetDrones-Starter",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Raye, ActionType = ExecutorType.Summon, Description = "Normal Summon Raye" },
                    new() { CardId = CardId.Hayate, ActionType = ExecutorType.SpSummon, Description = "Link summon Hayate" },
                    new() { CardId = CardId.Hayate, ActionType = ExecutorType.Activate, Description = "Hayate direct attack & dump Raye/Engage to GY" },
                    new() { CardId = CardId.Shizuku, ActionType = ExecutorType.SpSummon, Description = "Link Shizuku in Main 2 for End Phase search" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "HornetDrones-Starter",
                RequiredCards = new List<int> { CardId.HornetDrones },
                FallbackLineName = "Camellia-Raye-Setup",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.HornetDrones, ActionType = ExecutorType.Activate, Description = "Activate Hornet Drones for Token" },
                    new() { CardId = CardId.Kagari, ActionType = ExecutorType.SpSummon, Description = "Link Kagari using Token" },
                    new() { CardId = CardId.Kagari, ActionType = ExecutorType.Activate, Description = "Kagari retrieve Hornet Drones / Engage" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Camellia-Raye-Setup",
                RequiredCards = new List<int> { CardId.Camellia },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Camellia, ActionType = ExecutorType.SpSummon, Description = "Link summon Camellia" },
                    new() { CardId = CardId.Camellia, ActionType = ExecutorType.Activate, Description = "Dump Raye to GY for floating security" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "WidowAnchor-Accesscode-OTK",
                RequiredCards = new List<int> { CardId.WidowAnchor },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.WidowAnchor, ActionType = ExecutorType.Activate, Description = "Steal opponent threat monster" },
                    new() { CardId = CardId.Zeke, ActionType = ExecutorType.SpSummon, Description = "Link stolen monster into Zeke" },
                    new() { CardId = CardId.AccesscodeTalker, ActionType = ExecutorType.SpSummon, Description = "Link Accesscode Talker for 5300 ATK OTK" }
                },
                EndBoardScore = 100,
                Condition = () => Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled())
            });

            // ── Bait Planner & Chain Advisor ──
            BaitPlanner.RegisterComboStarters(CardId.Engage, CardId.Linkage);
            BaitPlanner.RegisterBaitCards(CardId.ReinforcementOfTheArmy, CardId.PotOfDesires, CardId.HornetDrones, CardId.ForbiddenCrown, CardId.TheFallenAndTheVirtuous, CardId.RadiantTyphoonVision);
            ChainAdvisor.RegisterHighValueTargets(CardId.Engage, CardId.Kagari, CardId.Linkage, CardId.WidowAnchor);

            // --- I. Hand Traps & Fast Interruption ---
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossomAlt, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, GhostBelleEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CrossoutDesignator, CrossoutDesignatorEffect);
            AddExecutor(ExecutorType.Activate, CardId.WidowAnchor, WidowAnchorPreemptiveEffect);
            AddExecutor(ExecutorType.Activate, CardId.SharkCannon, SharkCannonEffect);

            // --- II. Board Breakers, Starters & Search Spells ---
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsTalent, TripleTacticsTalentEffect);
            AddExecutor(ExecutorType.Activate, CardId.TripleTacticsThrust, TripleTacticsThrustEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfDesires, PotOfDesiresEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.RadiantTyphoonVision, RadiantTyphoonVisionEffect);
            AddExecutor(ExecutorType.Activate, CardId.Lemnisgate, LemnisgateEffect);
            AddExecutor(ExecutorType.Activate, CardId.HornetDrones, HornetDronesEffect);
            AddExecutor(ExecutorType.Activate, CardId.Engage, EngageEffect);
            AddExecutor(ExecutorType.SpellSet, SpellSetForLinkage);
            AddExecutor(ExecutorType.Activate, CardId.Linkage, LinkageEffect);
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotAEffect);

            // --- III. Main Deck Summons & Monster Effects ---
            AddExecutor(ExecutorType.Summon, CardId.Raye, RayeSummon);
            AddExecutor(ExecutorType.Activate, CardId.Raye, RayeEffect);
            AddExecutor(ExecutorType.Summon, CardId.Roze, RozeSummon);
            AddExecutor(ExecutorType.Activate, CardId.Roze, RozeEffect);

            // --- IV. Generic Finishers (Accesscode & S:P) ---
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeSummon);
            AddExecutor(ExecutorType.Activate, CardId.AccesscodeTalker, AccesscodeEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPEffect);

            // --- V. Extra Deck Link Summons & Effects ---
            AddExecutor(ExecutorType.SpSummon, CardId.Zero, ZeroSummon);
            AddExecutor(ExecutorType.Activate, CardId.Zero, ZeroEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.EngageZero, EngageZeroSummon);
            AddExecutor(ExecutorType.Activate, CardId.EngageZero, EngageZeroEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Camellia, CamelliaSummon);
            AddExecutor(ExecutorType.Activate, CardId.Camellia, CamelliaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.EcclesiaAndTheDarkDragon, EcclesiaAndTheDarkDragonSummon);
            AddExecutor(ExecutorType.Activate, CardId.EcclesiaAndTheDarkDragon, EcclesiaAndTheDarkDragonEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Kagari, KagariSummon);
            AddExecutor(ExecutorType.Activate, CardId.Kagari, KagariEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Shizuku, ShizukuSummon);
            AddExecutor(ExecutorType.Activate, CardId.Shizuku, ShizukuEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Hayate, HayateSummon);
            AddExecutor(ExecutorType.Activate, CardId.Hayate, HayateEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Kaina, KainaSummon);
            AddExecutor(ExecutorType.Activate, CardId.Kaina, KainaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Zeke, ZekeSummon);
            AddExecutor(ExecutorType.Activate, CardId.Zeke, ZekeEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Azalea, AzaleaSummon);
            AddExecutor(ExecutorType.Activate, CardId.Azalea, AzaleaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.AzaleaTemperance, AzaleaTemperanceSummon);
            AddExecutor(ExecutorType.Activate, CardId.AzaleaTemperance, AzaleaTemperanceEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.Amatsu, AmatsuSummon);
            AddExecutor(ExecutorType.Activate, CardId.Amatsu, AmatsuEffect);

            // --- VI. Traps / Set Spells ---
            AddExecutor(ExecutorType.SpellSet, CardId.WidowAnchor);
            AddExecutor(ExecutorType.SpellSet, CardId.SharkCannon);
            AddExecutor(ExecutorType.SpellSet, CardId.Lemnisgate);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenCrown);
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusImpulse);
            AddExecutor(ExecutorType.SpellSet, CardId.CalledByTheGrave);
            AddExecutor(ExecutorType.Activate, CardId.WidowAnchor, WidowAnchorEffect);
            AddExecutor(ExecutorType.Activate, CardId.SharkCannon, SharkCannonEffect);

            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _hasOpponentActivatedMonsterEffect = false;
            _crownUsed = false;
            _dominusImpulseUsed = false;
            _kagariUsedThisTurn = false;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 1. Material Protection & Positioning Safeguards
        // ═══════════════════════════════════════════════════════════════

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            // Archetype Link-1 & Link-2 Sky Striker Ace monsters are ALWAYS allowed
            if (IsAceCard(card)) return true;

            // For non-archetype generic Link monsters (e.g. Accesscode Talker, S:P Little Knight):
            // Check if summoning them would consume an active Ace monster without lethal or threat removal
            if (card.HasType(CardType.Link))
            {
                var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
                if (activeAces.Count > 0)
                {
                    if (CanDealLethal() || OpponentHasActiveNegator() || Enemy.GetMonsterCount() >= 2)
                        return true;

                    DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning generic Link {card.Name} skipped to protect active Sky Striker Ace {activeAces.First().Name}");
                    return false;
                }
            }
            return true;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
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
                    return true; // Shift to attack during lethal push
                if (Card.IsAttack() && Util.IsTurn1OrMain2() && Enemy.GetMonsterCount() > 0)
                    return true; // Shift to defense on turn 1 / setup
            }

            return base.DefaultMonsterRepos();
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 2. Sky Striker Spell & Graveyard Threshold Helpers
        // ═══════════════════════════════════════════════════════════════

        protected int GetSpellCountInGrave()
        {
            return Bot.Graveyard.Count(c => c != null && c.IsSpell());
        }

        protected bool HasThreeOrMoreSpellsInGrave()
        {
            return GetSpellCountInGrave() >= 3;
        }

        protected bool EmptyMainMonsterZone()
        {
            for (int i = 0; i < 5; i++)
            {
                if (Bot.MonsterZone[i] != null)
                    return false;
            }
            return true;
        }

        protected override bool IsBoardStrongEnough()
        {
            int disruptionCount = 0;

            bool hasSkyStrikerLink = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));
            if (hasSkyStrikerLink) disruptionCount++;

            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.WidowAnchor))) disruptionCount += 2;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.SharkCannon))) disruptionCount++;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.Lemnisgate))) disruptionCount++;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.DominusImpulse))) disruptionCount++;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.ForbiddenCrown))) disruptionCount++;
            if (Bot.GetSpells().Any(c => c != null && c.IsFacedown() && c.IsCode(CardId.CalledByTheGrave))) disruptionCount++;

            if (Bot.HasInHand(CardId.AshBlossom) || Bot.HasInHand(CardId.GhostBelle) || Bot.HasInHand(CardId.DrollAndLockBird) || Bot.HasInHand(CardId.DominusImpulse)) disruptionCount++;

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

        protected override bool NeedsBoardPresence()
        {
            return !Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));
        }

        public override void OnChaining(int player, ClientCard card)
        {
            if (player == 1 && card != null && card.IsMonster() && Duel.Player == 0)
                _hasOpponentActivatedMonsterEffect = true;

            if (card != null && card.Id == CardId.DominusImpulse && card.Controller == 0 && card.Location == CardLocation.Hand)
                _dominusImpulseUsed = true;

            if (player == 0 && card != null && card.Id == CardId.Kagari)
                _kagariUsedThisTurn = true;

            base.OnChaining(player, card);
        }

        private long StringId(int cardId, int optionIndex)
        {
            return (optionIndex & 0xfffffL) | ((long)cardId << 20);
        }

        public override int OnSelectOption(IList<long> options)
        {
            for (int i = 0; i < options.Count; ++i)
            {
                long option = options[i];

                if (option == StringId(CardId.TripleTacticsTalent, 0)) // Draw 2
                {
                    if (Bot.GetHandCount() <= 2) return i;
                }
                if (option == StringId(CardId.TripleTacticsTalent, 1)) // Take Control
                {
                    if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= 2500)) return i;
                }
                if (option == StringId(CardId.TripleTacticsTalent, 2)) // Hand Rip
                {
                    return i;
                }

                if (option == StringId(CardId.Zero, 0)) // Search Sky Striker Spell
                {
                    return i;
                }
                if (option == StringId(CardId.Zero, 1)) // Tribute & summon Raye/Roze
                {
                    if (Duel.Player == 1 || Duel.Phase == DuelPhase.Battle) return i;
                }
            }

            return base.OnSelectOption(options);
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 3. Sky Striker Spell Activations & Search Handlers
        // ═══════════════════════════════════════════════════════════════

        private bool EngageEffect()
        {
            if (EnemyHasKnownNegate()) return false;
            if (IsMain1SearchDeferred()) return false;
            if (EmptyMainMonsterZone())
            {
                bool hasMonster = Bot.Hand.Any(c => c != null && c.IsMonster() && (c.Id == CardId.Raye || c.Id == CardId.Roze)) 
                                  || Bot.GetMonsters().Any(c => c != null);

                if (!hasMonster)
                {
                    AI.SelectCard(CardId.Raye, CardId.HornetDrones, CardId.Linkage, CardId.WidowAnchor, CardId.SharkCannon);
                }
                else
                {
                    AI.SelectCard(CardId.Linkage, CardId.WidowAnchor, CardId.HornetDrones, CardId.SharkCannon, CardId.Raye, CardId.Roze, CardId.Lemnisgate);
                }
                return true;
            }
            return false;
        }

        private bool HornetDronesEffect()
        {
            if (EnemyHasKnownNegate()) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (IsMain1SearchDeferred()) return false;
            return EmptyMainMonsterZone();
        }

        private bool LinkageEffect()
        {
            if (EnemyHasKnownNegate()) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (!CanActivateLinkage()) return false;

            bool hasSSMonster = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));
            var targetedAce = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsAceCard(c) && Util.IsChainTarget(c));

            // 1. Chain Dodging: If opponent targets an Ace monster on chain, dodge with Linkage!
            if (Duel.CurrentChain.Any(c => c != null && c.Controller == 1) && hasSSMonster)
            {
                if (targetedAce != null)
                {
                    // Select targetedAce to send to GY
                    AI.SelectCard(targetedAce);
                    // Next select Link target
                    if (!_kagariUsedThisTurn && (Bot.HasInGraveyard(CardId.Engage) || Bot.HasInGraveyard(CardId.WidowAnchor) || Bot.HasInGraveyard(CardId.Linkage)))
                        AI.SelectNextCard(CardId.Kagari, CardId.Zero, CardId.Shizuku, CardId.Camellia);
                    else
                        AI.SelectNextCard(CardId.Zero, CardId.Shizuku, CardId.Camellia, CardId.Kagari);
                    return true;
                }
            }

            // 2. Battle Phase OTK / Extra Attack push
            if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
            {
                if (hasSSMonster)
                {
                    if (!_kagariUsedThisTurn && (Bot.HasInGraveyard(CardId.Engage) || Bot.HasInGraveyard(CardId.WidowAnchor)))
                        AI.SelectCard(CardId.Kagari, CardId.Zero, CardId.Shizuku, CardId.Camellia);
                    else
                        AI.SelectCard(CardId.Zero, CardId.Kagari, CardId.Shizuku, CardId.Camellia);
                    return true;
                }
            }

            // 3. Main Phase extender / after Hayate attack
            if (EmptyMainMonsterZone() || Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Hayate)))
            {
                if (!_kagariUsedThisTurn && (Bot.HasInGraveyard(CardId.Engage) || Bot.HasInGraveyard(CardId.WidowAnchor) || Bot.HasInGraveyard(CardId.Linkage)))
                    AI.SelectCard(CardId.Kagari, CardId.Shizuku, CardId.Zero, CardId.Camellia);
                else
                    AI.SelectCard(CardId.Shizuku, CardId.Zero, CardId.Camellia, CardId.Kagari);
                return true;
            }

            return false;
        }

        private bool RotAEffect()
        {
            if (EnemyHasKnownNegate()) return false;
            if (IsMain1SearchDeferred()) return false;
            AI.SelectCard(CardId.Raye, CardId.Roze);
            return true;
        }

        private bool TheFallenAndTheVirtuousEffect()
        {
            if (EnemyHasKnownNegate()) return false;
            if (IsMain1SearchDeferred()) return false;
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target == null)
                target = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());

            bool hasAlbaz = Bot.HasInMonstersZone(CardId.FallenOfAlbaz) || Bot.HasInGraveyard(CardId.FallenOfAlbaz);
            if (hasAlbaz && target != null && Bot.HasInExtra(CardId.AlbionTheBrandedDragon))
            {
                AI.SelectCard(CardId.AlbionTheBrandedDragon);
                AI.SelectNextCard(target);
                return true;
            }

            bool hasEcclesia = Bot.HasInMonstersZone(CardId.EcclesiaAndTheDarkDragon) || Bot.HasInGraveyard(CardId.EcclesiaAndTheDarkDragon);
            if (hasEcclesia)
            {
                ClientCard targetSummon = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());
                if (targetSummon == null)
                    targetSummon = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());
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
            if (EnemyHasKnownNegate()) return false;
            if (IsMain1SearchDeferred()) return false;

            // Target & Destroy enemy face-up S/T (Skill Drain, Graydle Impact)
            ClientCard stTarget = Enemy.GetSpells()
                .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                .OrderByDescending(c => c.IsCode(82732705) ? 100 : 0)
                .ThenByDescending(c => c.HasType(CardType.Continuous) || c.HasType(CardType.Field) ? 10 : 1)
                .FirstOrDefault();

            if (stTarget != null)
            {
                AI.SelectCard(stTarget);
                return true;
            }

            bool hasQuickPlayInHand = Bot.Hand.Any(c => c != null && c != Card && c.IsSpell() && c.HasType(CardType.QuickPlay));
            if (hasQuickPlayInHand || Bot.Hand.Count <= 2) return true;

            return false;
        }



        private bool LemnisgateEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (Duel.Player == 1) return true;
                if (Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Main2) return true;
                if (Duel.CurrentChain.Any(c => c != null && c.Controller == 1)) return true;
                return false;
            }

            if (EnemyHasKnownNegate()) return false;
            if (IsMain1SearchDeferred()) return false;
            if (Card.Location == CardLocation.Hand && Bot.HasInSpellZone(CardId.Lemnisgate, faceUp: true))
                return false;

            var monsters = Bot.GetGraveyardMonsters()
                .Where(c => c.IsCode(CardId.Raye, CardId.Roze, CardId.Kagari, CardId.Shizuku, CardId.Hayate))
                .OrderBy(c => {
                    if (c.IsCode(CardId.Kagari)) return 1;
                    if (c.IsCode(CardId.Raye)) return 2;
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
        //  § 4. Monster Summons & Extra Deck Link Logic
        // ═══════════════════════════════════════════════════════════════

        private bool RayeSummon() => EmptyMainMonsterZone();

        private bool RayeEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location == CardLocation.Grave)
                return true;

            if (Card.IsDisabled()) return false;
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;

            bool isChainTarget = Util.IsChainTarget(Card);
            bool shouldActivate = false;

            if (Duel.Player == 0) // Our turn
            {
                if (isChainTarget) shouldActivate = true;
                else if (Duel.Phase == DuelPhase.Main2) shouldActivate = true;
            }
            else // Opponent's turn
            {
                if (isChainTarget) shouldActivate = true;
                else if (Enemy.GetMonsterCount() > 0) shouldActivate = true;
                else if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) shouldActivate = true;
            }

            if (!shouldActivate) return false;

            var targets = new List<int>();
            if (Duel.Player == 1)
            {
                if (Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 2000) && !Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Amatsu)))
                {
                    targets.Add(CardId.Amatsu);
                }
                if (!Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Kaina)))
                {
                    targets.Add(CardId.Kaina);
                }
                targets.Add(CardId.Shizuku);
                targets.Add(CardId.Zero);
                targets.Add(CardId.Camellia);
                targets.Add(CardId.Kagari);
                targets.Add(CardId.Hayate);
            }
            else
            {
                targets.Add(CardId.Shizuku);
                targets.Add(CardId.Kagari);
                targets.Add(CardId.Zero);
                targets.Add(CardId.Camellia);
                targets.Add(CardId.Kaina);
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

        private bool ZeroSummon() => !IsSpecialSummonBlocked();
        private bool ZeroEffect()
        {
            int opt = (int)ActivateDescription;
            if (opt == StringId(CardId.Zero, 0) || opt == -1 || opt == 0)
            {
                return true;
            }
            if (opt == StringId(CardId.Zero, 1))
            {
                if (IsSpecialSummonBlocked()) return false;
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
                AI.SelectCard(CardId.Zero, CardId.Raye, CardId.Roze, CardId.Token);
                AI.SelectNextCard(CardId.Kagari, CardId.Shizuku, CardId.Hayate, CardId.Camellia, CardId.Raye, CardId.Roze);
                if (target != null)
                {
                    AI.SelectNextCard(target);
                }
                return true;
            }
            return false;
        }

        private bool EngageZeroSummon() => !IsSpecialSummonBlocked();
        private bool EngageZeroEffect()
        {
            if (Duel.Phase == DuelPhase.Damage || Duel.Phase == DuelPhase.DamageCal)
            {
                return Bot.HasInGraveyard(CardId.Raye) && Bot.HasInGraveyard(CardId.Roze);
            }

            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.Attack >= 2500 && !c.IsDisabled() && !c.IsShouldNotBeTarget());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CamelliaSummon() => !IsSpecialSummonBlocked();
        private bool CamelliaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!Bot.HasInGraveyard(CardId.Raye))
                {
                    AI.SelectCard(CardId.Raye);
                }
                else
                {
                    AI.SelectCard(CardId.Engage, CardId.Linkage, CardId.WidowAnchor, CardId.Roze);
                }
                return true;
            }

            if (Card.Location == CardLocation.Grave)
            {
                ClientCard target = Enemy.GetMonsters()
                    .Where(c => c != null && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool EcclesiaAndTheDarkDragonSummon() => !IsSpecialSummonBlocked();
        private bool EcclesiaAndTheDarkDragonEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (Bot.Deck.Concat(Bot.Graveyard).Any(c => c != null && c.IsCode(CardId.FallenOfAlbaz)))
                {
                    AI.SelectCard(CardId.FallenOfAlbaz);
                    return true;
                }
                return false;
            }

            if (Card.Location == CardLocation.Grave)
            {
                ClientCard fusionTarget = Bot.Graveyard.Concat(Bot.Banished)
                    .FirstOrDefault(c => c != null && c.Level == 8 && c.HasType(CardType.Fusion));
                ClientCard fieldTarget = Enemy.GetMonsters().Concat(Enemy.GetSpells())
                    .FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget());
                if (fusionTarget != null && fieldTarget != null)
                {
                    AI.SelectCard(fusionTarget);
                    AI.SelectNextCard(fieldTarget);
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool HasHighATKAceOnField()
        {
            return Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && IsAceCard(m) && (m.Attack >= 2400 || m.IsCode(CardId.Zero)));
        }

        private bool KagariSummon()
        {
            if (IsSpecialSummonBlocked() || _kagariUsedThisTurn) return false;
            
            // If we have a high ATK Ace on field, only summon Kagari if Engage or Linkage is in GY to gain massive value
            if (HasHighATKAceOnField())
            {
                bool hasHighValueSpellInGY = Bot.Graveyard.Any(c => c != null && (c.Id == CardId.Engage || c.Id == CardId.Linkage));
                if (!hasHighValueSpellInGY) return false;
            }

            return Bot.Graveyard.Any(c => c != null && c.IsSpell() && (c.IsCode(CardId.Engage, CardId.Linkage, CardId.WidowAnchor, CardId.HornetDrones, CardId.SharkCannon, CardId.Lemnisgate)));
        }

        private bool KagariEffect()
        {
            AI.SelectCard(CardId.Engage, CardId.Linkage, CardId.WidowAnchor, CardId.HornetDrones, CardId.SharkCannon, CardId.Lemnisgate);
            return true;
        }

        private bool ShizukuSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            // If we have a high ATK Ace on field, only summon Shizuku if we lack interruptions for opponent turn
            if (HasHighATKAceOnField())
            {
                bool hasInterruption = Bot.Hand.Concat(Bot.GetSpells()).Any(c => c != null && (c.Id == CardId.WidowAnchor || c.Id == CardId.SharkCannon || c.Id == CardId.ForbiddenCrown || c.Id == CardId.DominusImpulse));
                if (hasInterruption) return false;
            }

            return Util.IsTurn1OrMain2() && !Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Shizuku));
        }

        private bool ShizukuEffect()
        {
            var targets = new List<int> { CardId.Engage, CardId.Linkage, CardId.WidowAnchor, CardId.HornetDrones, CardId.SharkCannon, CardId.Lemnisgate };
            var missingFromGrave = targets.Where(id => !Bot.HasInGraveyard(id)).ToList();
            if (missingFromGrave.Count > 0)
            {
                AI.SelectCard(missingFromGrave);
            }
            else
            {
                AI.SelectCard(targets);
            }
            return true;
        }

        private bool HayateSummon()
        {
            if (IsSpecialSummonBlocked()) return false;

            // If we have a high ATK Ace on field, only summon Hayate if enemy has a monster with higher ATK than our current monster
            if (HasHighATKAceOnField())
            {
                int myHighestATK = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Select(m => m.Attack).DefaultIfEmpty(0).Max();
                bool enemyHasHigherATK = Enemy.MonsterZone.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack > myHighestATK);
                if (!enemyHasHigherATK) return false;
            }

            return !Util.IsTurn1OrMain2() && !Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Hayate));
        }

        private bool HayateEffect()
        {
            if (!Bot.HasInGraveyard(CardId.Raye))
            {
                AI.SelectCard(CardId.Raye);
            }
            else
            {
                AI.SelectCard(CardId.Engage, CardId.Linkage, CardId.WidowAnchor);
            }
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

        private bool ZekeSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return Bot.GetMonsters().Any(card => 
                card != null && card.IsFaceup() && 
                (card.IsCode(CardId.Raye, CardId.Roze) || 
                 (card.Name != null && (card.Name.Contains("Sky Striker Ace") || card.Name.Contains("Sky Striker")))));
        }

        private bool ZekeEffect()
        {
            if (ShouldSkipCombo()) return false;
            ClientCard enemyTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            if (enemyTarget != null)
            {
                AI.SelectCard(enemyTarget);
                return true;
            }
            return false;
        }

        private bool AzaleaSummon() => !IsSpecialSummonBlocked();
        private bool AzaleaEffect()
        {
            if (ShouldSkipCombo()) return false;
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                                ?? Enemy.GetSpells().FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool AzaleaTemperanceSummon() => !IsSpecialSummonBlocked();
        private bool AzaleaTemperanceEffect()
        {
            if (ShouldSkipCombo()) return false;
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool AmatsuSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool isEndBoardPhase = Util.IsTurn1OrMain2() || Duel.Phase == DuelPhase.Main2;
            if (!isEndBoardPhase) return false;

            return !Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Shizuku)) 
                   && !Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Kagari)) 
                   && !Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.IsCode(CardId.Amatsu));
        }

        private bool AmatsuEffect()
        {
            if (ShouldSkipCombo()) return false;
            int opt = (int)ActivateDescription;

            if (opt == StringId(CardId.Amatsu, 1))
            {
                if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.Damage)
                {
                    ClientCard myCard = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && IsAceCard(c) && c.Id != CardId.Amatsu);
                    if (myCard == null)
                    {
                        var attackingMonster = Enemy.MonsterZone.GetMonsters().FirstOrDefault(c => c != null && c == Enemy.BattlingMonster);
                        if (attackingMonster != null && !attackingMonster.IsShouldNotBeTarget())
                        {
                            myCard = Card;
                            AI.SelectCard(myCard);
                            AI.SelectNextCard(attackingMonster);
                            return true;
                        }
                    }
                    else
                    {
                        ClientCard enemyCard = Util.GetProblematicEnemyMonster(0, true) 
                            ?? Enemy.GetMonsters().Concat(Enemy.GetSpells()).FirstOrDefault(c => c != null && !c.IsShouldNotBeTarget());
                        if (enemyCard != null)
                        {
                            AI.SelectCard(myCard);
                            AI.SelectNextCard(enemyCard);
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                if (LastChainCard == null || LastChainCard.Controller == 0) return false;
                if (LastChainCard.Location == CardLocation.Grave || LastChainCard.Location == CardLocation.Removed) return false;
                if (!IsThreatMonster(LastChainCard)) return false;

                bool canFloat = Bot.HasInGraveyard(CardId.Raye);
                bool hasOtherLink = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c) && c.Id != CardId.Amatsu);
                bool lowLP = Bot.LifePoints < 3000;

                if (canFloat || hasOtherLink || lowLP) return true;
                return false;
            }
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 5. Generic Finishers (Accesscode Talker & S:P Little Knight)
        // ═══════════════════════════════════════════════════════════════

        private bool SPSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var monsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            if (monsters.Count < 2) return false;

            return Util.GetProblematicEnemyCard() != null || Enemy.GetMonsterCount() > 0;
        }

        private bool SPEffect()
        {
            ClientCard target = Util.GetProblematicEnemyCard() ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool AccesscodeSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            var monsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsFacedown()).ToList();
            if (monsters.Count < 2) return false;

            int totalLinkRating = monsters.Sum(c => c.HasType(CardType.Link) ? c.LinkCount : 1);
            if (totalLinkRating >= 4)
            {
                return CanDealLethal() || Enemy.LifePoints <= 5300 || Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
            }
            return false;
        }

        private bool AccesscodeEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                var linkInGrave = Bot.Graveyard.Where(c => c != null && c.HasType(CardType.Link)).OrderByDescending(c => c.LinkCount).FirstOrDefault();
                if (linkInGrave != null)
                {
                    AI.SelectCard(linkInGrave);
                    return true;
                }
                var targetCard = Util.GetProblematicEnemyCard() ?? Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup()) ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
                if (targetCard != null)
                {
                    AI.SelectCard(targetCard);
                    return true;
                }
            }
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  § 6. Hand Traps & Spells Logic
        // ═══════════════════════════════════════════════════════════════

        private bool IsThreatMonster(ClientCard c)
        {
            if (c == null || !c.IsFaceup() || c.IsDisabled()) return false;
            if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) return true;
            if (c.Attack >= 2000) return true;
            if (c.HasType(CardType.Effect) && (c.Level >= 5 || c.Rank >= 5)) return true;
            return false;
        }

        private bool WidowAnchorPreemptiveEffect()
        {
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;

            // Restrict preemptive Widow Anchor to Main Phase 1 or 2 (prevent premature activation on Draw/Standby Phase)
            if (Duel.Phase != DuelPhase.Main1 && Duel.Phase != DuelPhase.Main2) return false;

            // Don't fire raw if opponent has active negators that can negate quick spells unless we are baiting
            if (OpponentHasActiveNegator()) return false;

            ClientCard target = Enemy.MonsterZone.GetMonsters()
                .Where(c => c != null && IsThreatMonster(c) && !c.IsDisabled() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                .OrderByDescending(c => c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link) ? 1000 : 0)
                .ThenByDescending(c => c.Attack)
                .FirstOrDefault();

            if (target == null) target = Util.GetProblematicEnemyMonster(0, true);

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool CrossoutDesignatorEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;

            int code = LastChainCard.Id;
            int alias = LastChainCard.Alias;
            if (alias != 0 && alias - code < 10) code = alias;
            if (code == 0) return false;

            if (!LastChainCard.IsMonster()) return false;

            if (GetRemainingCount(code) > 0)
            {
                AI.SelectAnnounceID(code);
                return true;
            }
            return false;
        }

        private bool ForbiddenDropletEffect()
        {
            if (ShouldSkipCombo()) return false;
            ClientCard target = Enemy.MonsterZone.GetMonsters()
                .Where(c => c != null && IsThreatMonster(c) && !c.IsDisabled() && !c.IsShouldNotBeTarget())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault() ?? Util.GetProblematicEnemyMonster(0, true);

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool TripleTacticsTalentEffect()
        {
            if (ShouldSkipCombo()) return false;
            return Duel.Player == 0 && _hasOpponentActivatedMonsterEffect;
        }

        private bool TripleTacticsThrustEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Duel.Player != 0 || !_hasOpponentActivatedMonsterEffect) return false;
            
            int searchTarget = CardId.Engage;
            if (Bot.GetRemainingCount(CardId.Engage, 3) == 0)
            {
                searchTarget = CardId.ReinforcementOfTheArmy;
            }

            AI.SelectCard(searchTarget);
            return true;
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

        private bool ForbiddenCrownEffect()
        {
            if (_crownUsed) return false;
            // Strictly prohibit chaining against our own card activations!
            if (Duel.LastChainPlayer == 0) return false;

            // On chain: Disable opponent's activating monster
            if (Duel.LastChainPlayer == 1 && LastChainCard != null && LastChainCard.Location == CardLocation.MonsterZone &&
                !LastChainCard.IsDisabled() && !LastChainCard.IsShouldNotBeTarget() && !LastChainCard.IsShouldNotBeSpellTrapTarget())
            {
                AI.SelectCard(LastChainCard);
                _crownUsed = true;
                return true;
            }

            // Preemptive: Disable high threat face-up monster on opponent turn or outside chain
            if (Duel.LastChainPlayer == -1 || Duel.Player == 1)
            {
                ClientCard target = Enemy.MonsterZone.GetMonsters()
                    .Where(c => c != null && IsThreatMonster(c) && !c.IsDisabled() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                    .OrderByDescending(c => c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link) ? 1000 : 0)
                    .ThenByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (target == null) target = Util.GetProblematicEnemyMonster(0, true);

                if (target != null)
                {
                    AI.SelectCard(target);
                    _crownUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool DominusImpulseEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand) return false;
            if (_dominusImpulseUsed) return false;
            if (Duel.LastChainPlayer != 1 || LastChainCard == null) return false;
            if (LastChainCard.Controller != 1) return false;

            _dominusImpulseUsed = true;
            return true;
        }

        private bool WidowAnchorEffect()
        {
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            // Strictly prohibit chaining against our own card activations!
            if (Duel.LastChainPlayer == 0) return false;

            // React on opponent chain first
            if (Duel.LastChainPlayer == 1 && LastChainCard != null && LastChainCard.Location == CardLocation.MonsterZone &&
                !LastChainCard.IsDisabled() && !LastChainCard.IsShouldNotBeTarget() && !LastChainCard.IsShouldNotBeSpellTrapTarget())
            {
                AI.SelectCard(LastChainCard);
                return true;
            }

            // Preemptive against opponent threat monster
            if (Duel.LastChainPlayer == -1 || Duel.Player == 1)
            {
                ClientCard target = Enemy.MonsterZone.GetMonsters()
                    .Where(c => c != null && IsThreatMonster(c) && !c.IsDisabled() && !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget())
                    .OrderByDescending(c => c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link) ? 1000 : 0)
                    .ThenByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (target == null) target = Util.GetProblematicEnemyMonster(0, true);

                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool SharkCannonEffect()
        {
            if (Card != null && Util.ChainContainsCard(Card.Id)) return false;
            // Strictly prohibit chaining against our own card activations!
            if (Duel.LastChainPlayer == 0) return false;

            // 1. Interrupt opponent targeting in GY on chain
            ClientCard targetedInGrave = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster() && Util.IsChainTarget(c));
            if (targetedInGrave != null)
            {
                AI.SelectCard(targetedInGrave);
                return true;
            }

            // 2. React to opponent monster activating from GY
            if (Duel.LastChainPlayer == 1 && LastChainCard != null && LastChainCard.Location == CardLocation.Grave && LastChainCard.IsMonster())
            {
                AI.SelectCard(LastChainCard);
                return true;
            }

            // 3. Reanimation & Steal: If 3+ Spells in GY, steal high ATK or Extra Deck monster to use as Link Material
            if (HasThreeOrMoreSpellsInGrave())
            {
                ClientCard highValueMonster = Enemy.Graveyard
                    .Where(c => c != null && c.IsMonster() && (c.Attack >= 2000 || c.IsExtraCard()))
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (highValueMonster != null)
                {
                    AI.SelectCard(highValueMonster);
                    if (!IsSpecialSummonBlocked() && MainMonsterZoneCount() < 5)
                        AI.SelectOption(1);
                    else
                        AI.SelectOption(0);
                    return true;
                }
            }

            // 4. Generic GY disruption: Banish key Level 5+ / Extra Deck monsters from opponent's GY
            ClientCard genericTarget = Enemy.Graveyard
                .Where(c => c != null && c.IsMonster() && (c.Level >= 5 || c.Rank >= 5 || c.IsExtraCard() || c.Attack >= 1800))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault() ?? Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());

            if (genericTarget != null)
            {
                AI.SelectCard(genericTarget);
                return true;
            }
            return false;
        }

        private bool SpellSetForLinkage()
        {
            if (Bot.GetFieldCount() > 0) return false;
            bool hasLinkageInHand = Bot.Hand.Any(c => c != null && c.Id == CardId.Linkage);
            if (!hasLinkageInHand) return false;

            // Only set non-QuickPlay Spells (Normal/Continuous/Field Spells)
            // QuickPlay Spells set this turn cannot be sent to GY as cost in YGOPro!
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

            // In Battle Phase: Activate Linkage for multi-attack OTK if we control an Ace monster
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

        private int MainMonsterZoneCount()
        {
            int count = 0;
            for (int i = 0; i < 5; i++)
            {
                if (Bot.MonsterZone[i] != null) count++;
            }
            return count;
        }

        private bool AshBlossomEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (!SmartHandTrapChain()) return false;
            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard != null && lastCard.Controller == 1)
            {
                return true;
            }
            return false;
        }

        private bool GhostBelleEffect()
        {
            if (Duel.LastChainPlayer != 1) return false;
            if (!SmartHandTrapChain()) return false;
            ClientCard lastCard = Util.GetLastChainCard();
            if (lastCard != null && lastCard.Controller == 1)
            {
                return true;
            }
            return false;
        }

        private bool CalledByTheGraveEffect()
        {
            return Duel.LastChainPlayer == 1 && DefaultCalledByTheGrave();
        }

        private bool DrollAndLockBirdEffect()
        {
            if (Duel.Player != 1 || Duel.LastChainPlayer != 1) return false;
            if (!SmartHandTrapChain()) return false;
            return true;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (hint == 506 || (cards != null && cards.Count > 0 && cards.All(c => c != null && c.Location == CardLocation.Deck)))
            {
                bool hasMonsterOnField = Bot.GetMonsters().Any(c => c != null);
                bool hasStarterInHand = Bot.Hand.Any(c => c != null && (c.Id == CardId.Raye || c.Id == CardId.Roze || c.Id == CardId.HornetDrones));

                if (!hasMonsterOnField && !hasStarterInHand)
                {
                    var starter = cards.FirstOrDefault(c => c != null && (c.Id == CardId.Raye || c.Id == CardId.HornetDrones || c.Id == CardId.Roze));
                    if (starter != null) return new List<ClientCard> { starter };
                }

                var priority = new[] { CardId.Linkage, CardId.WidowAnchor, CardId.HornetDrones, CardId.Raye, CardId.Lemnisgate, CardId.SharkCannon };
                foreach (int id in priority)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                    if (match != null) return new List<ClientCard> { match };
                }
            }
            if (Card != null && Card.Id == CardId.ForbiddenDroplet)
            {
                bool oppHasMonsterEffect = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() && m.HasType(CardType.Effect));
                var sortedCosts = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (oppHasMonsterEffect && c.IsMonster() && !IsAceCard(c)) return 0;
                    if (c.IsCode(CardId.ForbiddenCrown, CardId.RadiantTyphoonVision, CardId.TheFallenAndTheVirtuous)) return 1;
                    if (c.IsSpell() && !c.IsCode(CardId.Engage, CardId.Linkage, CardId.WidowAnchor)) return 2;
                    if (c.IsMonster() && !IsAceCard(c)) return 3;
                    if (c.IsSpell()) return 4;
                    return 10;
                }).ToList();
                return sortedCosts.Take(max).ToList();
            }
            if (Card != null && Card.Id == CardId.Engage)
            {
                bool hasMonsterOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup());

                if (!hasMonsterOnField)
                {
                    var starter = cards.FirstOrDefault(c => c != null && (c.Id == CardId.HornetDrones || c.Id == CardId.Linkage || c.Id == CardId.Raye || c.Id == CardId.Roze));
                    if (starter != null) return new List<ClientCard> { starter };
                }

                var priority = new[] { CardId.HornetDrones, CardId.Linkage, CardId.WidowAnchor, CardId.Raye, CardId.Lemnisgate, CardId.SharkCannon };
                foreach (int id in priority)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                    if (match != null) return new List<ClientCard> { match };
                }
            }
            if (Card != null && Card.Id == CardId.Amatsu)
            {
                var ourOtherAce = cards.FirstOrDefault(c => c != null && c.Controller == 0 && IsAceCard(c) && c.Id != CardId.Amatsu);
                if (ourOtherAce != null) return new List<ClientCard> { ourOtherAce };

                var enemyAttacking = cards.FirstOrDefault(c => c != null && c.Controller == 1 && c == Enemy.BattlingMonster);
                if (enemyAttacking != null) return new List<ClientCard> { enemyAttacking };

                var enemyProblematic = cards.Where(c => c != null && c.Controller == 1)
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();
                if (enemyProblematic != null) return new List<ClientCard> { enemyProblematic };

                var ourAmatsu = cards.FirstOrDefault(c => c != null && c.Controller == 0 && c.Id == CardId.Amatsu);
                if (ourAmatsu != null) return new List<ClientCard> { ourAmatsu };
            }
            if (Card != null && Card.Id == CardId.Zero)
            {
                if (cards.Any(c => c != null && c.IsSpell()))
                {
                    var priority = new[] { CardId.Engage, CardId.Linkage, CardId.WidowAnchor, CardId.HornetDrones, CardId.SharkCannon, CardId.Lemnisgate };
                    foreach (int id in priority)
                    {
                        var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                        if (match != null) return new List<ClientCard> { match };
                    }
                }
            }
            if (Card != null && Card.Id == CardId.Lemnisgate)
            {
                if (min == 1 && max == 1)
                {
                    var enemyFaceup = cards.FirstOrDefault(c => c != null && c.Controller == 1 && c.IsFaceup());
                    if (enemyFaceup != null) return new List<ClientCard> { enemyFaceup };
                }
            }
            if (Card != null && Card.Id == CardId.Linkage)
            {
                // Extra Deck selection: Pick highest impact Ace
                if (cards.Any(c => c != null && c.Location == CardLocation.Extra))
                {
                    bool canUseKagari = !_kagariUsedThisTurn && Bot.Graveyard.Any(sp => sp != null && sp.IsSpell() && (sp.Id == CardId.Engage || sp.Id == CardId.Linkage || sp.Id == CardId.WidowAnchor));
                    if (canUseKagari)
                    {
                        var kagari = cards.FirstOrDefault(c => c != null && c.Id == CardId.Kagari);
                        if (kagari != null) return new List<ClientCard> { kagari };
                    }

                    var priorityAces = new[] { CardId.Zero, CardId.Kagari, CardId.Hayate, CardId.Shizuku, CardId.Amatsu, CardId.Camellia };
                    foreach (int id in priorityAces)
                    {
                        var match = cards.FirstOrDefault(c => c != null && c.Id == id);
                        if (match != null) return new List<ClientCard> { match };
                    }
                }

                var targets = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.IsCode(CardId.Hayate)) return 1;
                    if (c.IsCode(CardId.Kagari)) return 2;
                    if (c.IsCode(CardId.Shizuku)) return 3;
                    if (IsAceCard(c)) return 4;
                    if (c.IsMonster()) return 5;
                    if (c.IsSpell() && c.IsFaceup()) return 6;
                    return 100;
                }).ToList();
                if (targets.Count > 0) return new List<ClientCard> { targets.First() };
            }
            if (Card != null && Card.Id == CardId.RadiantTyphoonVision)
            {
                var qps = cards.Where(c => c.IsSpell() && c.HasType(CardType.QuickPlay)).OrderBy(c => {
                    if (c.IsCode(CardId.Lemnisgate)) return 1;
                    if (c.IsCode(CardId.SharkCannon)) return 2;
                    if (c.IsCode(CardId.WidowAnchor)) return 3;
                    return 100;
                }).ToList();
                if (qps.Count > 0) return new List<ClientCard> { qps.First() };
            }

            // Material selection: preserve Ace cards
            if (hint == 511 || hint == 513 || hint == 533 || hint == 502 || hint == 504)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
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
                   card.IsCode(CardId.EngageZero) ||
                   card.IsCode(CardId.Camellia) ||
                   card.IsCode(CardId.Zeke) ||
                   card.IsCode(CardId.Azalea) ||
                   card.IsCode(CardId.AzaleaTemperance) ||
                   card.IsCode(CardId.Amatsu) ||
                   card.IsCode(CardId.AccesscodeTalker);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 950;
            if (c.IsCode(CardId.AshBlossom) || c.IsCode(CardId.GhostBelle) || c.IsCode(CardId.DrollAndLockBird))
                return 800;
            if (c.IsCode(CardId.Token)) return 10;
            if (c.IsCode(CardId.Raye)) return 20;
            if (c.IsCode(CardId.Roze)) return 30;
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

            if (attacker.Attack >= 3000)
            {
                var beatable = defenders.Where(d => d != null && d.IsMonster() && attacker.Attack > GetDefenseValue(d))
                                        .OrderByDescending(d => GetDefenseValue(d)).FirstOrDefault();
                if (beatable != null) return AI.Attack(attacker, beatable);
            }

            var beatableDefender = defenders.Where(d => d != null && d.IsMonster() && attacker.Attack > GetDefenseValue(d))
                                            .OrderBy(d => GetDefenseValue(d)).FirstOrDefault();
            if (beatableDefender != null) return AI.Attack(attacker, beatableDefender);

            return null;
        }
    }

    [Deck("Expert_2026_SkyStriker", "2026_SkyStriker")]
    public class ExpertSkyStrikerExecutor : _2026_SkyStrikerExecutor
    {
        private string _duelId;
        public ExpertSkyStrikerExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _duelId = $"duel_{Guid.NewGuid():N}";
            // [REMOVED-AI-TRAINING] ExpertDataLogger.EnsureInitialized(ExpertDataLogger.FindProjectRoot());
        }
        public override MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            var action = base.OnSelectIdleCmd(main);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogMainPhaseDecision(main, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
        public override BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            var action = base.OnBattle(attackers, defenders);
            // [REMOVED-AI-TRAINING] ExpertDataLogger.LogBattleDecision(attackers, defenders, action, Bot, Enemy, _duelId, Duel.Turn);
            return action;
        }
        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough()) return true;
            return base.ShouldStopExtending();
        }
    }
}
