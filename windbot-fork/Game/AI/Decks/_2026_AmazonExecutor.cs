// ============================================================
// CARD AUDIT — 2026_Amazon (v2 - AI Module & Guard Upgraded)
// Card Name                  | Type       | OPT? | Effect Summary          | Activate When              | NEVER Activate When
// ---------------------------|------------|------|-------------------------|----------------------------|--------------------
// Amazoness Princess         | Monster    | HOPT | Search Spell/Trap       | Main Phase search          | SS blocked / search deferred
// Amazoness War Chief        | Monster    | HOPT | SS self + set Spell/Trap| Main Phase setup           | SS blocked
// Amazoness Call             | Spell      | HOPT | Search Amazoness card   | Main Phase search          | Search deferred / no target
// Amazoness Secret Arts      | Spell      | HOPT | Fusion summon           | Main Phase for boss        | SS blocked / no material
// Polymerization             | Spell      | No   | Fusion summon           | Main Phase for boss        | SS blocked / materials < 2
// Amazoness Augusta          | Fusion     | HOPT | SS Amazoness from Deck  | Face-up Monster Zone       | No targets in Deck
// Amazoness Pet Liger King   | Fusion     | HOPT | Negate enemy monster    | Opponent activates monster | Immune target
// Amazoness Onslaught        | Trap       | No   | Banish after battle     | Battle Phase control       | Already active
// Ash Blossom                | Monster    | OPT  | Negate search/SS/GY     | Opponent activates         | Chokepoint hold / used
// ============================================================
// ACE CARDS: Primary: Amazoness Augusta | Secondary: Amazoness Empress, Pet Liger King
// COMBO STARTERS: 1. Amazoness Princess 2. Amazoness War Chief 3. Amazoness Call
// CHOKEPOINTS: Princess / Call / Secret Arts search & fusion
// WIN CONDITION: Fusion Summon Amazoness Augusta (attack twice, target protection) + Apollousa / Onslaught
// GOING 1ST END BOARD: Amazoness Augusta + Apollousa / Onslaught
// GOING 2ND GAMEPLAN: Kaiju / Droplet / Storm board break -> Augusta OTK push
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Amazon", "2026_Amazon")]
    public class _2026_AmazonExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck - Amazoness
            public const int AmazonessWarChief = 50486289;
            public const int AmazonessPrincess = 84539520;
            public const int AmazonessBabyTiger = 10928224;
            public const int AmazonessSpiritualist = 97870394;
            public const int AmazonessQueen = 15951532;
            public const int AmazonessScouts = 71209500;
            public const int AmazonessSilverSwordMaster = 24087580;
            public const int AmazonessGoldenWhipMaster = 97692972;

            // Main Deck - Tech / Engine
            public const int AussaTheEarthChanneler = 62803464;
            public const int AshBlossom = 14558127;
            public const int GamecielSeaTurtleKaiju = 55063751;
            public const int AmazonessCall = 57312333;
            public const int AmazonessSecretArts = 86758746;
            public const int Polymerization = 24094653;
            public const int AmazonessVillage = 712559;
            public const int ReinforcementOfTheArmy = 32807846;
            public const int ForbiddenDroplet = 24299458;
            public const int HarpiesFeatherDuster = 18144507;
            public const int LightningStorm = 14532163;
            public const int DarkRulerNoMore = 54693926;
            public const int AmazonessHotSpring = 62480168;
            public const int AmazonessHall = 25396150;
            public const int AmazonessOnslaught = 83407038;

            // Extra Deck - Amazoness
            public const int AmazonessAugusta = 23965033;
            public const int AmazonessPetLigerKing = 59353647;
            public const int AmazonessEmpress = 4591250;
            public const int AmazonessPetLiger = 68507541;

            // Extra Deck - Generic
            public const int ChevreuilScout = 13023431;
            public const int FaisanScout = 86038337;
            public const int AussaTheEarthCharmerImmovable = 97661969;
            public const int ApollousaBowOfTheGoddess = 4280258;
            public const int AccesscodeTalker = 86066372;
        }

        private static readonly int[] AceCardIds = {
            CardId.AmazonessAugusta,
            CardId.AmazonessEmpress,
            CardId.AmazonessPetLigerKing
        };

        // HOPT tracking flags
        private bool _princessUsed = false;
        private bool _warChiefUsed = false;
        private bool _callUsed = false;
        private bool _secretArtsUsed = false;
        private bool _ashBlossomUsed = false;

        // ═══════════════════════════════════════
        //  HAND MANAGEMENT HELPERS
        // ═══════════════════════════════════════
        private bool HandNearFull() => Bot.Hand.Count >= 5;

        private bool SearchWouldOverflow()
        {
            if (!HandNearFull()) return false;
            if (Bot.GetMonsterCount() >= 2) return false;
            return true;
        }

        public _2026_AmazonExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // ── AI Module Registration ──
            HeuristicGuard.RegisterAceCards(AceCardIds);

            // ChainTimingAdvisor: Protect core engine starters from enemy hand traps/negates
            ChainAdvisor.RegisterHighValueTargets(
                CardId.AmazonessPrincess,
                CardId.AmazonessCall,
                CardId.AmazonessWarChief,
                CardId.AmazonessSecretArts
            );

            // ComboRouter: Register 4 hand-aware combo lines
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Princess-Full-Combo",
                RequiredCards = new List<int> { CardId.AmazonessPrincess },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AmazonessPrincess, ActionType = ExecutorType.Summon, Description = "Normal Summon Princess" },
                    new() { CardId = CardId.AmazonessPrincess, ActionType = ExecutorType.Activate, Description = "Princess search Call" },
                    new() { CardId = CardId.AmazonessCall, ActionType = ExecutorType.Activate, Description = "Call search Secret Arts" },
                    new() { CardId = CardId.AmazonessSecretArts, ActionType = ExecutorType.Activate, Description = "Activate Secret Arts" },
                    new() { CardId = CardId.AmazonessAugusta, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Augusta" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "WarChief-To-Augusta",
                RequiredCards = new List<int> { CardId.AmazonessWarChief },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AmazonessWarChief, ActionType = ExecutorType.Activate, Description = "SS War Chief from hand" },
                    new() { CardId = CardId.AmazonessWarChief, ActionType = ExecutorType.Activate, Description = "War Chief sets Secret Arts" },
                    new() { CardId = CardId.AmazonessAugusta, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Augusta" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Going2nd-Kaiju-Augusta-OTK",
                RequiredCards = new List<int> { CardId.GamecielSeaTurtleKaiju, CardId.AmazonessSecretArts },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.GamecielSeaTurtleKaiju, ActionType = ExecutorType.SpSummon, Description = "Remove enemy threat with Kaiju" },
                    new() { CardId = CardId.AmazonessSecretArts, ActionType = ExecutorType.Activate, Description = "Fusion Summon Augusta" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Spiritualist-Poly-Fallback",
                RequiredCards = new List<int> { CardId.AmazonessSpiritualist, CardId.Polymerization },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AmazonessSpiritualist, ActionType = ExecutorType.Summon, Description = "Summon Spiritualist" },
                    new() { CardId = CardId.Polymerization, ActionType = ExecutorType.Activate, Description = "Activate Polymerization" },
                    new() { CardId = CardId.AmazonessEmpress, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Empress" }
                },
                EndBoardScore = 70
            });

            BaitPlanner.RegisterComboStarters(CardId.AmazonessCall, CardId.AmazonessPrincess, CardId.AmazonessWarChief);
            BaitPlanner.RegisterBaitCards(CardId.AussaTheEarthChanneler, CardId.ReinforcementOfTheArmy, CardId.AmazonessVillage);

            // ===== Priority 1: Hand Traps & Disruption Counters =====
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);

            // ===== Priority 2: Quick Effects & Board Breakers =====
            AddExecutor(ExecutorType.SpSummon, CardId.GamecielSeaTurtleKaiju, KaijuSpSummon);
            AddExecutor(ExecutorType.Activate, CardId.LightningStorm, LightningStormEffect);
            AddExecutor(ExecutorType.Activate, CardId.HarpiesFeatherDuster, DefaultHarpiesFeatherDusterFirst);
            AddExecutor(ExecutorType.Activate, CardId.DarkRulerNoMore, DarkRulerNoMoreEffect);

            // ===== Priority 3: Boss Monster Effects =====
            AddExecutor(ExecutorType.Activate, CardId.AmazonessAugusta, AugustaEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessPetLigerKing, PetLigerKingEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessEmpress);
            AddExecutor(ExecutorType.Activate, CardId.ApollousaBowOfTheGoddess, ApollousaEffect);

            // ===== Priority 4: Engine Spells & Search =====
            AddExecutor(ExecutorType.Activate, CardId.ReinforcementOfTheArmy, RotaEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessCall, CallEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessSecretArts, SecretArtsEffect);
            AddExecutor(ExecutorType.Activate, CardId.Polymerization, PolyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessVillage, VillageEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessHotSpring, HotSpringEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessHall, HallEffect);

            // ===== Priority 5: Engine Monsters (Hand / Field) =====
            AddExecutor(ExecutorType.Activate, CardId.AmazonessBabyTiger, BabyTigerEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessScouts, ScoutsEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessPrincess, PrincessEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessWarChief, WarChiefEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessSpiritualist, SpiritualistEffect);
            AddExecutor(ExecutorType.Activate, CardId.AussaTheEarthChanneler, AussaChannelerEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessSilverSwordMaster, SilverSwordMasterEffect);
            AddExecutor(ExecutorType.Activate, CardId.AmazonessGoldenWhipMaster, GoldenWhipMasterEffect);

            // ===== Priority 6: Normal Summons =====
            AddExecutor(ExecutorType.Summon, CardId.AmazonessPrincess);
            AddExecutor(ExecutorType.Summon, CardId.AmazonessWarChief, NormalSummonWarChief);
            AddExecutor(ExecutorType.Summon, CardId.AmazonessSpiritualist);

            // ===== Priority 7: Extra Deck Summons =====
            AddExecutor(ExecutorType.SpSummon, CardId.AmazonessAugusta, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.AmazonessPetLigerKing, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.AmazonessEmpress, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.AmazonessPetLiger, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.ChevreuilScout, LinkSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.FaisanScout, LinkSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.AussaTheEarthCharmerImmovable, LinkSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.ApollousaBowOfTheGoddess, ApollousaLinkSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, LinkSummonCheck);

            // ===== Priority 8: Traps & Repos =====
            AddExecutor(ExecutorType.Activate, CardId.AmazonessOnslaught, OnslaughtEffect);
            AddExecutor(ExecutorType.SpellSet, SpellSetFiltered);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _princessUsed = false;
            _warChiefUsed = false;
            _callUsed = false;
            _secretArtsUsed = false;
            _ashBlossomUsed = false;

            if (ShouldGoBreakBoard)
            {
                _ashBlossomUsed = false;
            }
        }

        public override bool OnSelectHand()
        {
            return true; // Go First for setup
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        protected override bool IsBoardStrongEnough()
        {
            // Augusta on field = core end board
            bool hasAugusta = Bot.HasInMonstersZone(CardId.AmazonessAugusta);
            if (hasAugusta && Bot.GetMonsterCount() >= 2)
                return true;
            // Augusta + Onslaught face-up = ideal end board
            if (hasAugusta && Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.AmazonessOnslaught)))
                return true;
            // Apollousa + any ace = sufficient
            if (Bot.HasInMonstersZone(CardId.ApollousaBowOfTheGoddess) && Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c)))
                return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            bool hasAugusta = Bot.HasInMonstersZone(CardId.AmazonessAugusta);
            if (!hasAugusta) return false;
            return base.ShouldStopExtending();
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            if (card.HasType(CardType.Link))
            {
                int reqRating = card.LinkCount;
                if (!IsMaterialSelectionSafeForLink(reqRating))
                {
                    DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} is not safe (would consume Ace card(s) as Link material)");
                    return false;
                }
            }

            return true;
        }

        private bool IsMaterialSelectionSafeForLink(int requiredRating)
        {
            var faceUpNonAceMonsters = Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !IsAceCard(c))
                .ToList();

            int availableRating = 0;
            foreach (var m in faceUpNonAceMonsters)
            {
                if (m.HasType(CardType.Link))
                    availableRating += m.LinkCount;
                else
                    availableRating += 1;
            }

            return faceUpNonAceMonsters.Count >= requiredRating && availableRating >= requiredRating;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c))
            {
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup())
                    return int.MaxValue;
                return 900;
            }
            return 100;
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Hint 509: Special Summon -> Filter by Location (prefer Deck over Hand/Grave to avoid crashes)
            if (hint == 509)
            {
                var deckCards = cards.Where(c => c != null && c.Location == CardLocation.Deck).ToList();
                if (deckCards.Count > 0 && cards.Any(c => c.Location != CardLocation.Deck))
                {
                    DecisionTracer.Trace("OnSelectCard", "Filtering hint 509 Special Summon to Deck location");
                    return Util.CheckSelectCount(deckCards, cards, min, max);
                }
            }

            // Hints 511/512/513/533: Material selection
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();

                if (cancelable)
                {
                    var nonFieldAceMaterials = sorted.Where(c => !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
                    if (nonFieldAceMaterials.Count < min)
                    {
                        DecisionTracer.Trace("OnSelectCard", "Cancelling material selection to protect field Ace cards");
                        return null;
                    }
                    return Util.CheckSelectCount(nonFieldAceMaterials, cards, min, max);
                }
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            var withoutFieldAces = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (withoutFieldAces.Count >= min)
            {
                var sorted = withoutFieldAces.OrderBy(c => GetMaterialPriority(c)).ToList();
                DecisionTracer.Trace("OnSelectLinkMaterial", $"Using {sorted.Count} non-Ace field monsters for Link material");
                return sorted.Take(max).ToList();
            }

            var allSorted = cards.Where(c => c != null).OrderBy(c => GetMaterialPriority(c)).ToList();
            var fieldAces = allSorted.Where(c => c.Location == CardLocation.MonsterZone && IsAceCard(c)).ToList();
            if (fieldAces.Any())
            {
                DecisionTracer.Trace("OnSelectLinkMaterial", $"WARNING: Forced to use field Ace card {fieldAces.First().Name} as Link material");
            }
            return allSorted.Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            var result = new List<ClientCard>();
            var handMats = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
            var fieldNonAce = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && !IsAceCard(c)).ToList();
            var fieldAce = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && IsAceCard(c)).ToList();

            foreach (var c in handMats) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max)
                foreach (var c in fieldNonAce) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max)
                foreach (var c in fieldAce) { result.Add(c); if (result.Count >= max) break; }

            if (result.Count >= min) return result;
            return base.OnSelectFusionMaterial(cards, min, max);
        }

        // ═══════════════════════════════════════
        //  UTILITY CHECKS
        // ═══════════════════════════════════════

        private bool FusionSummonCheck()
        {
            bool cond = !IsSpecialSummonBlocked();
            if (!cond)
                DecisionTracer.TraceSkip("FusionSummonCheck", "Fusion summon is blocked by floodgate");
            return cond;
        }

        private bool LinkSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;

            bool hasFieldAce = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));
            if (hasFieldAce)
            {
                int faceUpNonAce = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
                int faceUpAce = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && IsAceCard(c));

                if (Card.IsCode(CardId.ChevreuilScout, CardId.FaisanScout))
                {
                    if (faceUpNonAce < 1 && faceUpAce > 0)
                    {
                        DecisionTracer.TraceSkip("LinkSummonCheck", $"Skipping {Card.Name}: would waste Ace as Link-1 material");
                        return false;
                    }
                }
                else if (faceUpNonAce < 2)
                {
                    DecisionTracer.TraceSkip("LinkSummonCheck", $"Skipping {Card.Name}: not enough non-Ace monsters ({faceUpNonAce}) to protect Ace");
                    return false;
                }
            }

            return true;
        }

        // ═══════════════════════════════════════
        //  HAND TRAP: ASH BLOSSOM
        // ═══════════════════════════════════════

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (_ashBlossomUsed) return false;
            if (Card.Location != CardLocation.Hand) return false;

            ClientCard lastChain = Util.GetLastChainCard();
            if (lastChain == null) return false;
            if (lastChain.Controller != 1 || Duel.LastChainPlayer != 1) return false;

            _ashBlossomUsed = true;
            DecisionTracer.TraceActivate("AshBlossom", $"Negating opponent's {lastChain.Name}");
            return true;
        }

        // ═══════════════════════════════════════
        //  BOARD BREAKERS
        // ═══════════════════════════════════════

        private bool ForbiddenDropletEffect()
        {
            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && !c.IsShouldNotBeTarget());
            if (target != null && Bot.Hand.Count > 1)
            {
                var discard = Bot.Hand.FirstOrDefault(c => c != null && c.Id != CardId.ForbiddenDroplet
                    && !IsAceCard(c)
                    && !c.IsCode(CardId.AmazonessCall, CardId.AmazonessSecretArts, CardId.AmazonessPrincess));
                if (discard == null)
                    discard = Bot.Hand.FirstOrDefault(c => c != null && c.Id != CardId.ForbiddenDroplet);

                if (discard != null)
                {
                    AI.SelectCard(discard);
                    AI.SelectNextCard(target);
                    DecisionTracer.TraceActivate("ForbiddenDroplet", $"Negating {target.Name} by discarding {discard.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool KaijuSpSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            ClientCard target = Enemy.GetMonsters()
                .Where(c => c != null && !c.IsShouldNotBeTarget())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("KaijuSpSummon", $"Summoning Kaiju over opponent's {target.Name}");
                return true;
            }
            return false;
        }

        private bool LightningStormEffect()
        {
            if (Bot.GetMonsterCount() == 0 && Bot.GetSpellCount() == 0)
            {
                if (Enemy.GetSpellCount() >= 2)
                {
                    AI.SelectOption(1); // Backrow
                    DecisionTracer.TraceActivate("LightningStorm", "Destroying opponent's backrow");
                    return true;
                }
                if (Enemy.GetMonsterCount() > 0)
                {
                    AI.SelectOption(0); // Monsters
                    DecisionTracer.TraceActivate("LightningStorm", "Destroying opponent's monsters");
                    return true;
                }
            }
            return false;
        }

        private bool DarkRulerNoMoreEffect()
        {
            bool cond = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());
            if (cond)
                DecisionTracer.TraceActivate("DarkRulerNoMore", "Activating Dark Ruler No More");
            return cond;
        }

        // ═══════════════════════════════════════
        //  BOSS MONSTER EFFECTS
        // ═══════════════════════════════════════

        private bool AugustaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Select remaining Amazoness starter from Deck
                int princessRem = GetRemainingCount(CardId.AmazonessPrincess);
                int warChiefRem = GetRemainingCount(CardId.AmazonessWarChief);
                int spiritualistRem = GetRemainingCount(CardId.AmazonessSpiritualist);

                if (princessRem > 0)
                {
                    AI.SelectCard(CardId.AmazonessPrincess);
                }
                else if (warChiefRem > 0)
                {
                    AI.SelectCard(CardId.AmazonessWarChief);
                }
                else if (spiritualistRem > 0)
                {
                    AI.SelectCard(CardId.AmazonessSpiritualist);
                }
                else
                {
                    DecisionTracer.TraceSkip("Augusta", "No target Amazoness monsters left in Deck");
                    return false;
                }

                DecisionTracer.TraceActivate("Augusta", "Summoning Amazoness monster from Deck");
                return true;
            }
            return false;
        }

        private bool PetLigerKingEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            var target = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && !c.IsShouldNotBeTarget())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("PetLigerKing", $"Negating opponent's {target.Name} (ATK: {target.Attack})");
                return true;
            }

            DecisionTracer.TraceSkip("PetLigerKing", "No valid opponent monsters to negate");
            return false;
        }

        private bool ApollousaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                DecisionTracer.TraceActivate("Apollousa", "Negating opponent's monster effect");
                return true;
            }
            return false;
        }

        // ═══════════════════════════════════════
        //  ENGINE SPELLS & SEARCH
        // ═══════════════════════════════════════

        private bool RotaEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (SearchWouldOverflow()) return false;
            if (GetRemainingCount(CardId.AmazonessPrincess) == 0) return false;

            var bait = GetBaitIfNeeded(Card);
            if (bait != null) return false;

            AI.SelectCard(CardId.AmazonessPrincess);
            DecisionTracer.TraceActivate("ReinforcementOfTheArmy", "Searching Amazoness Princess");
            return true;
        }

        private bool CallEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (_callUsed) return false;
            if (SearchWouldOverflow())
            {
                DecisionTracer.TraceSkip("AmazonessCall", "Hand near full — skipping search to avoid overflow");
                return false;
            }

            var targets = new List<int>();
            if (GetRemainingCount(CardId.AmazonessPrincess) > 0) targets.Add(CardId.AmazonessPrincess);
            if (GetRemainingCount(CardId.AmazonessSecretArts) > 0) targets.Add(CardId.AmazonessSecretArts);
            if (GetRemainingCount(CardId.AmazonessWarChief) > 0) targets.Add(CardId.AmazonessWarChief);

            if (targets.Count == 0)
            {
                DecisionTracer.TraceSkip("AmazonessCall", "No valid search targets remaining in Deck");
                return false;
            }

            AI.SelectCard(targets.ToArray());
            _callUsed = true;
            DecisionTracer.TraceActivate("AmazonessCall", "Searching Amazoness card");
            return true;
        }

        private bool SecretArtsEffect()
        {
            if (_secretArtsUsed) return false;
            if (IsSpecialSummonBlocked()) return false;

            AI.SelectCard(new[] {
                CardId.AmazonessAugusta,
                CardId.AmazonessPetLigerKing,
                CardId.AmazonessEmpress
            });
            _secretArtsUsed = true;
            DecisionTracer.TraceActivate("AmazonessSecretArts", "Activating Secret Arts fusion summon");
            return true;
        }

        private bool PolyEffect()
        {
            if (IsSpecialSummonBlocked()) return false;

            int handMonsters = Bot.Hand.Count(c => c != null && c.IsMonster());
            int fieldMonsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());

            if (handMonsters + fieldMonsters < 2)
            {
                DecisionTracer.TraceSkip("Polymerization", "Not enough fusion materials available (< 2)");
                return false;
            }

            DecisionTracer.TraceActivate("Polymerization", "Fusion summoning with Polymerization");
            return true;
        }

        private bool VillageEffect()
        {
            DecisionTracer.TraceActivate("AmazonessVillage", "Activating Amazoness Village field spell");
            return true;
        }

        private bool HotSpringEffect()
        {
            DecisionTracer.TraceActivate("AmazonessHotSpring", "Activating Amazoness Hot Spring");
            return true;
        }

        private bool HallEffect()
        {
            DecisionTracer.TraceActivate("AmazonessHall", "Activating Amazoness Hall");
            return true;
        }

        // ═══════════════════════════════════════
        //  MONSTER EFFECTS (AMAZONESS ENGINE)
        // ═══════════════════════════════════════

        private bool PrincessEffect()
        {
            if (IsMain1SearchDeferred()) return false;
            if (_princessUsed) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (SearchWouldOverflow())
                {
                    DecisionTracer.TraceSkip("AmazonessPrincess", "Hand near full — skipping search to avoid overflow");
                    return false;
                }

                var targets = new List<int>();
                if (GetRemainingCount(CardId.AmazonessCall) > 0) targets.Add(CardId.AmazonessCall);
                if (GetRemainingCount(CardId.AmazonessSecretArts) > 0) targets.Add(CardId.AmazonessSecretArts);
                if (GetRemainingCount(CardId.AmazonessOnslaught) > 0) targets.Add(CardId.AmazonessOnslaught);

                if (targets.Count == 0)
                {
                    DecisionTracer.TraceSkip("AmazonessPrincess", "No search targets remaining in Deck");
                    return false;
                }

                AI.SelectCard(targets.ToArray());
                _princessUsed = true;
                DecisionTracer.TraceActivate("AmazonessPrincess", "Searching Amazoness spell/trap");
                return true;
            }
            return false;
        }

        private bool WarChiefEffect()
        {
            if (_warChiefUsed) return false;
            if (Card.Location == CardLocation.Hand)
            {
                if (!IsSpecialSummonBlocked())
                {
                    DecisionTracer.TraceActivate("AmazonessWarChief", "Special summoning War Chief from hand");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                var targets = new List<int>();
                if (GetRemainingCount(CardId.AmazonessCall) > 0) targets.Add(CardId.AmazonessCall);
                if (GetRemainingCount(CardId.AmazonessSecretArts) > 0) targets.Add(CardId.AmazonessSecretArts);
                if (GetRemainingCount(CardId.Polymerization) > 0) targets.Add(CardId.Polymerization);
                if (GetRemainingCount(CardId.AmazonessOnslaught) > 0) targets.Add(CardId.AmazonessOnslaught);

                if (targets.Count == 0)
                {
                    DecisionTracer.TraceSkip("AmazonessWarChief", "No Spell/Trap targets left to set from Deck");
                    return false;
                }

                AI.SelectCard(targets.ToArray());
                _warChiefUsed = true;
                DecisionTracer.TraceActivate("AmazonessWarChief", "Setting Amazoness Spell/Trap from deck");
                return true;
            }
            return false;
        }

        private bool BabyTigerEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                if (!IsSpecialSummonBlocked())
                {
                    DecisionTracer.TraceActivate("AmazonessBabyTiger", "Special Summoning Baby Tiger from hand/GY");
                    return true;
                }
            }
            return false;
        }

        private bool ScoutsEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                ClientCard lastChain = Util.GetLastChainCard();
                bool opponentMonsterEffectTargeting = lastChain != null && lastChain.Controller == 1
                    && (lastChain.HasType(CardType.Effect) && lastChain.HasType(CardType.Monster));
                if (opponentMonsterEffectTargeting || Duel.Phase == DuelPhase.Battle)
                {
                    DecisionTracer.TraceActivate("AmazonessScouts", "Tributing Scouts to protect Amazoness monsters");
                    return true;
                }
            }
            return false;
        }

        private bool SpiritualistEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                var target = Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsCode(CardId.AmazonessBabyTiger) && c.IsFaceup())
                    ?? Bot.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.AmazonessPrincess, CardId.AmazonessWarChief, CardId.AmazonessSilverSwordMaster, CardId.AmazonessGoldenWhipMaster))
                    ?? Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.IsCode(CardId.AmazonessOnslaught, CardId.AmazonessVillage, CardId.AmazonessHotSpring, CardId.AmazonessHall));

                if (target != null && !IsSpecialSummonBlocked())
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("AmazonessSpiritualist", $"Summoning Spiritualist by returning {target.Name}");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                // Fix: Check remaining count of Polymerization in Deck instead of Bot.Deck.FirstOrDefault
                if (GetRemainingCount(CardId.Polymerization) > 0)
                {
                    DecisionTracer.TraceActivate("AmazonessSpiritualist", "Searching Polymerization from Deck");
                    return true;
                }
            }
            return false;
        }

        private bool AussaChannelerEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                var discard = Bot.Hand.FirstOrDefault(c => c != null && c.Id != CardId.AussaTheEarthChanneler && c.HasAttribute(CardAttribute.Earth));
                if (discard != null)
                {
                    var targets = new List<int>();
                    if (GetRemainingCount(CardId.AmazonessPrincess) > 0) targets.Add(CardId.AmazonessPrincess);
                    if (GetRemainingCount(CardId.AmazonessWarChief) > 0) targets.Add(CardId.AmazonessWarChief);
                    if (GetRemainingCount(CardId.AmazonessSpiritualist) > 0) targets.Add(CardId.AmazonessSpiritualist);

                    if (targets.Count > 0)
                    {
                        AI.SelectCard(discard);
                        AI.SelectNextCard(targets.ToArray());
                        DecisionTracer.TraceActivate("AussaChanneler", "Searching Amazoness Earth monster");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SilverSwordMasterEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                DecisionTracer.TraceActivate("SilverSwordMaster", "Setting Silver Sword Master as Pendulum Scale");
                return true;
            }
            else if (Card.Location == CardLocation.SpellZone)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.AmazonessPrincess, CardId.AmazonessWarChief, CardId.AmazonessSpiritualist));
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("SilverSwordMaster", $"Adding {target.Name} from GY to hand");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.AmazonessCall, CardId.AmazonessSecretArts, CardId.AmazonessOnslaught));
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("SilverSwordMaster", $"Adding Spell/Trap {target.Name} from GY to hand");
                    return true;
                }
            }
            return false;
        }

        private bool GoldenWhipMasterEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                DecisionTracer.TraceActivate("GoldenWhipMaster", "Setting Golden Whip Master as Pendulum Scale");
                return true;
            }
            else if (Card.Location == CardLocation.SpellZone)
            {
                var enemyCont = Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && c.HasType(CardType.Continuous));
                var target = enemyCont ?? Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFacedown())
                    ?? Enemy.GetSpells().FirstOrDefault(c => c != null);
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("GoldenWhipMaster", $"Targeting opponent's {target.Name} to destroy");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave || Card.Location == CardLocation.Removed)
            {
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.AmazonessPrincess, CardId.AmazonessWarChief, CardId.AmazonessSpiritualist) && c.Id != CardId.AmazonessGoldenWhipMaster);
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("GoldenWhipMaster", $"Adding {target.Name} from GY to hand");
                    return true;
                }
            }
            return false;
        }

        // ═══════════════════════════════════════
        //  NORMAL SUMMON CHECKS
        // ═══════════════════════════════════════

        private bool NormalSummonWarChief()
        {
            bool cond = !_warChiefUsed && Bot.Hand.Count > 1;
            if (cond)
                DecisionTracer.TraceActivate("NormalSummonWarChief", "Summoning Amazoness War Chief");
            return cond;
        }

        // ═══════════════════════════════════════
        //  LINK SUMMON CHECKS
        // ═══════════════════════════════════════

        private bool ApollousaLinkSummonCheck()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;

            int faceUpMonsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());
            int faceUpNonAce = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            if (faceUpMonsters < 2)
            {
                DecisionTracer.TraceSkip("ApollousaLinkSummonCheck", "Not enough face-up monsters for Apollousa");
                return false;
            }

            bool hasFieldAce = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));
            if (hasFieldAce && faceUpNonAce < 2)
            {
                DecisionTracer.TraceSkip("ApollousaLinkSummonCheck", "Would sacrifice Ace card — skipping Apollousa");
                return false;
            }

            if (!hasFieldAce)
            {
                if (GetRemainingCount(CardId.AmazonessAugusta) > 0)
                {
                    DecisionTracer.TraceSkip("ApollousaLinkSummonCheck", "Prioritizing Augusta Fusion first");
                    return false;
                }
            }

            DecisionTracer.TraceActivate("ApollousaLinkSummonCheck", "Summoning Apollousa for negate protection");
            return true;
        }

        // ═══════════════════════════════════════
        //  TRAPS & SPELL SET
        // ═══════════════════════════════════════

        private bool OnslaughtEffect()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                DecisionTracer.TraceActivate("AmazonessOnslaught", "Activating Onslaught face-up");
                return true;
            }
            return false;
        }

        private bool SpellSetFiltered()
        {
            if (Card.IsCode(CardId.AmazonessOnslaught, CardId.AmazonessCall, CardId.AmazonessSecretArts))
            {
                bool cond = SetBackrowCondition();
                if (cond)
                    DecisionTracer.TraceActivate("SpellSetFiltered", $"Setting {Card.Name}");
                return cond;
            }
            return false;
        }

        private bool SetBackrowCondition() => Util.IsTurn1OrMain2();
    }
}
