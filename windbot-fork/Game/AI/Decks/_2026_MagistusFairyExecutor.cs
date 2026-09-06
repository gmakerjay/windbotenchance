// ============================================================
// CARD AUDIT โ€” 2026_MagistusFairy
// | Card Name           | Type    | OPT? | Cost      | Effect              | Activate When        | NEVER When           |
// |---------------------|---------|------|-----------|---------------------|----------------------|----------------------|
// | Zoroa the Magistus  | Monster | HOPT | None      | Equip extra deck    | Start combo / equip  | No equip target      |
// | Rilliona the Magistus| Monster | HOPT | None      | Search spell        | Summon to start combo| Effect negated       |
// ACE CARDS: Primary: Zoroa the Magistus Verethragna / Secondary: Selene Queen of the Master Magicians
// COMBO STARTERS: 1. Zoroa the Magistus of Flame 2. Rilliona the Magistus of Verre
// CHOKEPOINTS: Zoroa summon negated
// WIN CONDITION: Control using Magistus equip cards and extra deck summons
// GOING 1ST END BOARD: Zoroa equipped with Magistus monster + 1 Set Spell/Trap
// GOING 2ND GAMEPLAN: Break board with Super Poly, summon Selene and link summon to establish advantage
// ============================================================

// ============================================================
// COMBO DRAFT โ€” 2026_MagistusFairy
// ============================================================
// === COMBO LINE 1: Zoroa Equip Setup (Starter: Zoroa) ===
// HAND REQUIRED: Zoroa the Magistus of Flame
// STEP 1: Normal Summon Zoroa
// STEP 2: Zoroa Effect: Equip 1 Magistus monster from Extra Deck (e.g. Zoroa Verethragna)
// STEP 3: Special Summon Level 4 Spellcaster from hand/GY
// END BOARD: Zoroa + equipped Verethragna + Special Summoned monster
// === COMBO LINE 2: Going 2nd Link Summon ===
// STEP 1: Special Summon/Normal Summon Spellcasters (Rilliona, Crowley)
// STEP 2: Link Summon Selene Queen of the Master Magicians
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
    [Deck("2026_MagistusFairy", "2026_MagistusFairy")]
    public class _2026_MagistusFairyExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int FairyTailLuna = 86937530;
            public const int FairyTailSnow = 55623480;
            public const int FairyTailMatchgiru = 19144622;
            public const int GeniaOfTheRing = 99745551;
            public const int SpentaTheMagistusSealer = 42544773;
            public const int CrowleyTheGiftedMagistus = 875572;
            public const int ZoroaTheMagistusOfFlame = 36099130;
            public const int RillionaTheMagistusOfVerre = 72498838;
            public const int WitchcrafterGenni = 64756282;
            public const int RegulusThePrinceOfEndymion = 96228804;
            public const int MulcharmyFuwalos = 42141493;
            public const int DrollAndLockBird = 94145021;
            public const int AshBlossom = 14558128; // Alt art โ€” deck uses 14558128 not 14558127
            public const int OnceUponAFairyTail = 19326613;
            public const int TailsOfTheFairyTails = 82119326;
            public const int FairyTailBall = 56725612;
            public const int FairyPrince = 10000120;
            public const int VerreMagicLacrimaOfLight = 73664385;
            public const int EndymionEmpire = 34041788;
            public const int ForbiddenCrown = 98829635;
            public const int SuperPolymerization = 48130397;
            public const int InstantFusion = 1845204;
            public const int CalledByTheGrave = 24224830;

            // Extra Deck
            public const int WeaverOfFairyTails = 78021082;
            public const int TellerOfFairyTails = 4026187;
            public const int ZoroaTheMagistusVerethragna = 37260677;
            public const int MagistusChorozo = 66532962;
            public const int InvokedMechaba = 75286621;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int MudragonOfTheSwamp = 54757758;
            public const int SummonSorceress = 61665245;
            public const int SeleneQueenOfTheMasterMagicians = 45819647;
            public const int CharmerQuartetInBloom = 27519978;
            public const int ArtemisTheMagistusMoonMaiden = 34755994;
            public const int EndymionTheCrescentMagistus = 20714553;
            public const int FairyTailWiccat = 27632520;
        }

        private bool _lunaSearched = false;
        private bool _snowSummonedThisTurn = false;
        private bool _lacrimaUsed = false;
        private bool _tailsUsed = false;

        private static readonly HashSet<int> HandTraps = new HashSet<int>
        {
            CardId.MulcharmyFuwalos,
            CardId.DrollAndLockBird,
            CardId.AshBlossom
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(
                CardId.ZoroaTheMagistusVerethragna,
                CardId.WeaverOfFairyTails,
                CardId.TellerOfFairyTails,
                CardId.InvokedMechaba,
                CardId.SeleneQueenOfTheMasterMagicians,
                CardId.CharmerQuartetInBloom
            ) || base.IsAceCard(card);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            return 100;
        }

        protected override bool IsBoardStrongEnough()
        {
            // Verethragna equipped to Zoroa + another monster = control board established
            bool hasVerethragna = Bot.GetSpells().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.ZoroaTheMagistusVerethragna));
            bool hasZoroa = Bot.HasInMonstersZone(CardId.ZoroaTheMagistusOfFlame);
            if (hasVerethragna && hasZoroa && Bot.GetMonsterCount() >= 2)
                return true;
            // Selene on field = strong enough
            if (Bot.HasInMonstersZone(CardId.SeleneQueenOfTheMasterMagicians))
                return true;
            // Charmer Quartet = Link-4 boss = more than enough
            if (Bot.HasInMonstersZone(CardId.CharmerQuartetInBloom))
                return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            // Stop once we have Selene or Charmer Quartet on board
            if (Bot.HasInMonstersZone(CardId.SeleneQueenOfTheMasterMagicians)
                || Bot.HasInMonstersZone(CardId.CharmerQuartetInBloom))
                return base.ShouldStopExtending();
            return false; // Keep going if no Link boss yet
        }

        public _2026_MagistusFairyExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards
            HeuristicGuard.RegisterAceCards(
                CardId.ZoroaTheMagistusVerethragna,
                CardId.WeaverOfFairyTails,
                CardId.TellerOfFairyTails,
                CardId.InvokedMechaba,
                CardId.SeleneQueenOfTheMasterMagicians,
                CardId.CharmerQuartetInBloom
            );
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.ZoroaTheMagistusOfFlame, CardId.FairyTailLuna },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.ZoroaTheMagistusOfFlame, ActionType = ExecutorType.Activate, Description = "Play CardId.ZoroaTheMagistusOfFlame" },
                    new() { CardId = CardId.FairyTailLuna, ActionType = ExecutorType.Activate, Description = "Extend with CardId.FairyTailLuna" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Rilliona-Search-Path",
                RequiredCards = new List<int> { CardId.RillionaTheMagistusOfVerre },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.RillionaTheMagistusOfVerre, ActionType = ExecutorType.Activate, Description = "Rilliona searches Lacrima" },
                    new() { CardId = CardId.VerreMagicLacrimaOfLight, ActionType = ExecutorType.Activate, Description = "Lacrima draws/SS" },
                    new() { CardId = CardId.SeleneQueenOfTheMasterMagicians, ActionType = ExecutorType.SpSummon, Description = "Link into Selene" }
                },
                EndBoardScore = 75
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.ZoroaTheMagistusOfFlame, CardId.GeniaOfTheRing, CardId.SpentaTheMagistusSealer, CardId.RillionaTheMagistusOfVerre);
            BaitPlanner.RegisterBaitCards(CardId.GeniaOfTheRing);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.ZoroaTheMagistusOfFlame, CardId.SummonSorceress);


            // โ•โ•โ• TIER 1: Reactive Negations & Hand Traps โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosEffect);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.DrollAndLockBird, DrollAndLockBirdEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenCrown, ForbiddenCrownEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);

            // โ•โ•โ• TIER 2: Main Combos & Spells โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.InstantFusion, InstantFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.VerreMagicLacrimaOfLight, LacrimaEffect);
            AddExecutor(ExecutorType.Activate, CardId.TailsOfTheFairyTails, TailsEffect);
            AddExecutor(ExecutorType.Activate, CardId.OnceUponAFairyTail, OnceUponAFairyTailEffect);
            AddExecutor(ExecutorType.Activate, CardId.EndymionEmpire, EndymionEmpireEffect);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailBall, FairyTailBallEffect);

            // โ•โ•โ• TIER 3: Monster Summons & Field Effects โ•โ•โ•
            AddExecutor(ExecutorType.SpSummon, CardId.FairyTailSnow, SnowSpSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailSnow, SnowFieldEffect);

            AddExecutor(ExecutorType.Summon, CardId.ZoroaTheMagistusOfFlame, ZoroaSummon);
            AddExecutor(ExecutorType.Activate, CardId.ZoroaTheMagistusOfFlame, ZoroaEffect);

            AddExecutor(ExecutorType.Summon, CardId.RillionaTheMagistusOfVerre, RillionaSummon);
            AddExecutor(ExecutorType.Activate, CardId.RillionaTheMagistusOfVerre, RillionaEffect);

            AddExecutor(ExecutorType.Summon, CardId.CrowleyTheGiftedMagistus, CrowleySummon);
            AddExecutor(ExecutorType.Activate, CardId.CrowleyTheGiftedMagistus, CrowleyEffect);

            AddExecutor(ExecutorType.Summon, CardId.FairyTailLuna, LunaSummon);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailLuna, LunaEffect);

            AddExecutor(ExecutorType.Summon, CardId.SpentaTheMagistusSealer, SpentaSummon);
            AddExecutor(ExecutorType.Activate, CardId.SpentaTheMagistusSealer, SpentaEffect);

            AddExecutor(ExecutorType.Summon, CardId.FairyTailMatchgiru, MatchgiruSummon);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailMatchgiru, MatchgiruEffect);

            AddExecutor(ExecutorType.Summon, CardId.GeniaOfTheRing, GenniSummon);
            AddExecutor(ExecutorType.Activate, CardId.GeniaOfTheRing, GenniEffect);

            AddExecutor(ExecutorType.Summon, CardId.WitchcrafterGenni, GenniSummon);
            AddExecutor(ExecutorType.Activate, CardId.WitchcrafterGenni, GenniEffect);

            AddExecutor(ExecutorType.Summon, CardId.RegulusThePrinceOfEndymion, RegulusSummon);
            AddExecutor(ExecutorType.Activate, CardId.RegulusThePrinceOfEndymion, RegulusEffect);

            // โ•โ•โ• TIER 4: Extra Deck Summons โ•โ•โ•
            AddExecutor(ExecutorType.SpSummon, CardId.ArtemisTheMagistusMoonMaiden, ArtemisSummon);
            AddExecutor(ExecutorType.Activate, CardId.ArtemisTheMagistusMoonMaiden);

            AddExecutor(ExecutorType.SpSummon, CardId.FairyTailWiccat, WiccatSummon);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailWiccat);

            AddExecutor(ExecutorType.SpSummon, CardId.ZoroaTheMagistusVerethragna, VerethragnaSummon);
            AddExecutor(ExecutorType.Activate, CardId.ZoroaTheMagistusVerethragna, VerethragnaEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.MagistusChorozo, ChorozoSummon);
            AddExecutor(ExecutorType.Activate, CardId.MagistusChorozo);

            AddExecutor(ExecutorType.SpSummon, CardId.EndymionTheCrescentMagistus, CrescentSummon);
            AddExecutor(ExecutorType.Activate, CardId.EndymionTheCrescentMagistus);

            AddExecutor(ExecutorType.SpSummon, CardId.InvokedMechaba);
            AddExecutor(ExecutorType.SpSummon, CardId.SeleneQueenOfTheMasterMagicians, SeleneSummon);
            AddExecutor(ExecutorType.Activate, CardId.SeleneQueenOfTheMasterMagicians, SeleneEffect);

            AddExecutor(ExecutorType.SpSummon, CardId.SummonSorceress, SummonSorceressSummon);
            AddExecutor(ExecutorType.Activate, CardId.SummonSorceress);

            AddExecutor(ExecutorType.SpSummon, CardId.WeaverOfFairyTails, WeaverSummon);
            AddExecutor(ExecutorType.Activate, CardId.WeaverOfFairyTails);

            AddExecutor(ExecutorType.SpSummon, CardId.TellerOfFairyTails, TellerSummon);
            AddExecutor(ExecutorType.Activate, CardId.TellerOfFairyTails);

            AddExecutor(ExecutorType.SpSummon, CardId.CharmerQuartetInBloom, Link4Summon);
            AddExecutor(ExecutorType.Activate, CardId.CharmerQuartetInBloom);

            // โ•โ•โ• Traps & Sets โ•โ•โ•
            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            return true; // Go first to set up control board (Luna/Snow/Verethragna)
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _lunaSearched = false;
            _snowSummonedThisTurn = false;
            _lacrimaUsed = false;
            _tailsUsed = false;
            
            if (ShouldGoBreakBoard)
            {
                // Going second: prioritize Super Poly + board-breaking
                _lacrimaUsed = false;
                _tailsUsed = false;
            }
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (hint == 502 || hint == 503 || hint == 504 || hint == 505 || hint == 508 || hint == 514 || hint == 551 || hint == 575 || hint == 577 || hint == 507)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count >= min)
                {
                    return enemyCards.Take(Math.Max(min, Math.Min(max, enemyCards.Count))).ToList();
                }
            }

            if (hint == 533)
            {
                var oppPrinces = cards.Where(c => c != null && c.Controller == 1 && c.IsCode(CardId.FairyPrince)).ToList();
                var oppOthers = cards.Where(c => c != null && c.Controller == 1 && !c.IsCode(CardId.FairyPrince)).ToList();
                var ourCards = cards.Where(c => c != null && c.Controller == 0 && !IsAceCard(c)).ToList();
                var ordered = oppPrinces.Concat(oppOthers).Concat(ourCards).ToList();
                if (ordered.Count >= min)
                {
                    return ordered.Take(Math.Max(min, Math.Min(max, ordered.Count))).ToList();
                }
            }

            if (hint == 509)
            {
                bool hasDeck = cards.Any(c => c.Location == CardLocation.Deck);
                if (hasDeck && cards.Any(c => c.Location == CardLocation.Hand))
                {
                    var deckCards = cards.Where(c => c.Location == CardLocation.Deck).ToList();
                    return deckCards.Take(Math.Max(min, Math.Min(max, deckCards.Count))).ToList();
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            return SortMaterials(cards, max);
        }

        public override IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            return SortMaterials(cards, max);
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            if (max == 1) // Must be Artemis Link 1
            {
                // For Link-1: prefer expendable Magistus (Spenta > Crowley > Rilliona > Zoroa)
                // NEVER send Zoroa if it has an equip (that wastes the equip)
                int[] magistusIds = { CardId.SpentaTheMagistusSealer, CardId.CrowleyTheGiftedMagistus, CardId.RillionaTheMagistusOfVerre, CardId.ZoroaTheMagistusOfFlame };
                foreach (int mid in magistusIds)
                {
                    var mat = cards.FirstOrDefault(c => c != null && c.IsCode(mid) && !HasEquippedMagistus(c));
                    if (mat != null) return new[] { mat };
                }
                // Fallback: any Magistus even with equip
                var anyMagistus = cards.FirstOrDefault(c => c != null && magistusIds.Contains(c.Id));
                if (anyMagistus != null) return new[] { anyMagistus };
            }
            return SortMaterials(cards, max);
        }

        /// <summary>
        /// Check if a monster has a Magistus equip card attached (making it more valuable).
        /// </summary>
        private bool HasEquippedMagistus(ClientCard card)
        {
            if (card == null || card.Location != CardLocation.MonsterZone) return false;
            // Check if any of our S/T zone cards are equips pointing to this monster
            return Bot.GetSpells().Any(c => c != null && c.IsFaceup() &&
                c.IsCode(CardId.ArtemisTheMagistusMoonMaiden, CardId.ZoroaTheMagistusVerethragna,
                         CardId.EndymionTheCrescentMagistus, CardId.MagistusChorozo));
        }

        /// <summary>
        /// Smart material sorting: protects valuable monsters, sacrifices expendables first.
        /// Lower score = picked first as material (more expendable).
        /// </summary>
        private IList<ClientCard> SortMaterials(IList<ClientCard> cards, int max)
        {
            var sorted = cards.OrderBy(c => {
                if (c == null) return 9999;
                if (c.Controller == 0)
                {
                    // โ”€โ”€ NEVER USE โ”€โ”€ (highest protection)
                    if (IsAceCard(c)) return 50000;
                    // Zoroa with equip = DO NOT sacrifice (equip makes it a live threat)
                    if (c.IsCode(CardId.ZoroaTheMagistusOfFlame) && HasEquippedMagistus(c)) return 40000;

                    // โ”€โ”€ PROTECT โ”€โ”€ (high protection, avoid if possible)
                    if (HandTraps.Contains(c.Id)) return 8000;  // Hand traps on field = keep for GY value
                    // Zoroa without equip = still important (combo starter), but less than equipped
                    if (c.IsCode(CardId.ZoroaTheMagistusOfFlame)) return 5000;
                    // Rilliona = searcher, protect if effect unused
                    if (c.IsCode(CardId.RillionaTheMagistusOfVerre)) return 3000;
                    // Luna = bounce disruption, protect on opponent's turn
                    if (c.IsCode(CardId.FairyTailLuna) && Duel.Player == 1) return 4000;

                    // โ”€โ”€ PREFER TO USE โ”€โ”€ (low value as material)
                    // Tokens (Level 0 or non-effect) = best material
                    if (c.Level == 0 || !c.HasType(CardType.Effect)) return 1;
                    // Genni/Witchcrafter = expendable extender
                    if (c.IsCode(CardId.WitchcrafterGenni, CardId.GeniaOfTheRing)) return 5;
                    // Spenta after effect = expendable
                    if (c.IsCode(CardId.SpentaTheMagistusSealer)) return 10;
                    // Crowley = mid-value, can be sacrificed after attribute declaration
                    if (c.IsCode(CardId.CrowleyTheGiftedMagistus)) return 50;
                    // Regulus = extender, moderate value
                    if (c.IsCode(CardId.RegulusThePrinceOfEndymion)) return 80;
                    // Luna on our turn = can be used as material
                    if (c.IsCode(CardId.FairyTailLuna) && Duel.Player == 0) return 60;
                    // Fairy Tail Snow = low priority on field (effect already used)
                    if (c.IsCode(CardId.FairyTailSnow)) return 30;
                    // Matchgiru = expendable after bounce effect
                    if (c.IsCode(CardId.FairyTailMatchgiru)) return 20;
                }
                return 100; // Default: moderate willingness to use
            }).ToList();
            return sorted.Take(max).ToList();
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 1: Negations & Reactive Hand Traps โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool MulcharmyFuwalosEffect()
        {
            if (!SmartHandTrapChain()) return false;
            return Duel.Player == 1 && Bot.GetFieldCount() == 0;
        }

        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (Util.GetLastChainCard() == null || Util.GetLastChainCard().Controller != 1) return false;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool DrollAndLockBirdEffect()
        {
            if (!SmartHandTrapChain()) return false;
            // Activate Droll reactively on opponent's turn when they search/draw
            return Duel.Player == 1;
        }

        private bool CalledByTheGraveEffect()
        {
            return DefaultCalledByTheGrave();
        }

        private bool ForbiddenCrownEffect()
        {
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;

            // Target opponent's dangerous monster to negate
            if (Duel.LastChainPlayer == 1)
            {
                var lastCard = Util.GetLastChainCard();
                if (lastCard != null && lastCard.Controller == 1 && lastCard.IsMonster() && !lastCard.IsDisabled() && lastCard.IsFaceup())
                {
                    AI.SelectCard(lastCard);
                    return true;
                }
            }

            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() && (m.IsMonsterDangerous() || m.IsExtraCard()));
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return false;
        }

        private bool SuperPolymerizationEffect()
        {
            if (Bot.Hand.Count < (Card.Location == CardLocation.Hand ? 2 : 1)) return false;

            // Simple Super Poly check: We need to fuse LIGHT/DARK or attributes matching Garura/Mudragon on field
            var allMonsters = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Where(c => c != null && c.IsFaceup()).ToList();
            if (allMonsters.Count < 2) return false;

            // Garura: 2 monsters with the same Type and Attribute but different names
            bool canGarura = false;
            for (int i = 0; i < allMonsters.Count; i++)
            {
                for (int j = i + 1; j < allMonsters.Count; j++)
                {
                    if (allMonsters[i].Race == allMonsters[j].Race &&
                        allMonsters[i].Attribute == allMonsters[j].Attribute &&
                        allMonsters[i].Id != allMonsters[j].Id)
                    {
                        canGarura = true;
                        break;
                    }
                }
            }

            // Mudragon: 2 monsters with the same Attribute but different Types
            bool canMudragon = false;
            for (int i = 0; i < allMonsters.Count; i++)
            {
                for (int j = i + 1; j < allMonsters.Count; j++)
                {
                    if (allMonsters[i].Attribute == allMonsters[j].Attribute &&
                        allMonsters[i].Race != allMonsters[j].Race)
                    {
                        canMudragon = true;
                        break;
                    }
                }
            }

            if (canGarura || canMudragon)
            {
                // Discard cost
                AI.SelectCard(new[] {
                    CardId.FairyTailSnow,
                    CardId.WitchcrafterGenni,
                    CardId.AshBlossom,
                    CardId.DrollAndLockBird
                });

                // Targets
                AI.SelectNextCard(new[] {
                    CardId.GaruraWingsOfResonantLife,
                    CardId.MudragonOfTheSwamp
                });

                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 2: Spells & Field Actions โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool InstantFusionEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.LifePoints <= 1000) return false; // 1000 LP cost
            AI.SelectCard(CardId.TellerOfFairyTails); // Level 4 Fusion Spellcaster
            return true;
        }

        private bool LacrimaEffect()
        {
            if (_lacrimaUsed) return false;
            // Draw or Special Summon spellcaster
            _lacrimaUsed = true;
            return true;
        }

        private bool TailsEffect()
        {
            if (_tailsUsed) return false;
            _tailsUsed = true;
            return true;
        }

        private bool OnceUponAFairyTailEffect()
        {
            // Search 1 "Fairy Tail" monster from deck, then discard 1
            if (ShouldSkipCombo()) return false;
            // Check we have a target in deck
            bool hasTarget = GetRemainingCount(CardId.FairyTailLuna) > 0 || 
                             GetRemainingCount(CardId.FairyTailSnow) > 0 ||
                             GetRemainingCount(CardId.FairyTailMatchgiru) > 0;
            if (!hasTarget) return false;
            // Need a card to discard
            if (Bot.Hand.Count <= 1) return false;
            return true;
        }

        private bool EndymionEmpireEffect()
        {
            // Field spell โ€” activate if we have Spellcasters to benefit
            if (Card.Location == CardLocation.Hand)
            {
                // Don't activate if we already have it face-up
                if (Bot.HasInSpellZone(CardId.EndymionEmpire)) return false;
                return Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasRace(CardRace.SpellCaster));
            }
            // Other effects of Endymion Empire
            return true;
        }

        private bool FairyTailBallEffect()
        {
            // Equip to Rilliona or Zoroa
            var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && 
                (m.IsCode(CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 3: Monster Summons & Disruptions โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private int GetSnowBanishPriority(ClientCard c)
        {
            if (c == null) return 9999;
            // Best targets: spells/traps in GY
            if (c.Location == CardLocation.Grave && !c.IsMonster())
            {
                if (c.IsCode(CardId.OnceUponAFairyTail, CardId.TailsOfTheFairyTails)) return 10;
                return 5;
            }
            // Next: hand traps or spent monster in GY
            if (c.Location == CardLocation.Grave)
            {
                if (HandTraps.Contains(c.Id)) return 15;
                if (c.IsCode(CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre, CardId.CrowleyTheGiftedMagistus, CardId.SpentaTheMagistusSealer))
                    return 100;
                return 20;
            }
            // Hand/Field targets
            if (c.Location == CardLocation.Hand) return 500;
            return 1000;
        }

        private bool SnowSpSummonEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_snowSummonedThisTurn) return false;

            // React to opponent's summons or attacks, or end phase
            bool oppTurnPlay = Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Main2);
            bool endPhasePlay = Duel.Phase == DuelPhase.End;
            bool weNeedLethal = Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) && Bot.GetMonsterCount() < 4;

            if (oppTurnPlay || endPhasePlay || weNeedLethal)
            {
                var gyPool = Bot.Graveyard.Where(c => c != null && c != Card).ToList();
                var handPool = Bot.Hand.Where(c => c != null && c.Id != CardId.VerreMagicLacrimaOfLight && !HandTraps.Contains(c.Id)).ToList();
                var fieldPool = Bot.GetMonsters().Concat(Bot.GetSpells()).Where(c => c != null && !IsAceCard(c)).ToList();
                var banishPool = gyPool.Concat(handPool).Concat(fieldPool).ToList();
                
                var sortedBanishPool = banishPool.OrderBy(GetSnowBanishPriority).ToList();
                if (sortedBanishPool.Count >= 7)
                {
                    AI.SelectCard(sortedBanishPool.Take(7).ToList());
                    _snowSummonedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool SnowFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Target opponent's face-up monster to book it
            var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsViableEffectTarget(m));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool ZoroaSummon()
        {
            // Zoroa is the primary combo starter โ€” equip from ED, then SS Level 4 Spellcaster
            if (ShouldSkipCombo()) return false;
            bool hasEdTarget = Bot.ExtraDeck.Any(c => c != null &&
                (c.IsCode(CardId.ZoroaTheMagistusVerethragna, CardId.ArtemisTheMagistusMoonMaiden, CardId.EndymionTheCrescentMagistus)));
            return hasEdTarget || Bot.GetMonsterCount() == 0;
        }

        private bool ZoroaEffect()
        {
            if (ActivateDescription == Util.GetStringId(CardId.ZoroaTheMagistusOfFlame, 0))
            {
                // Equip Magistus monster from Extra Deck based on game state
                // Verethragna for battle disruption on opponent's turn, Artemis for draw/search
                if (Duel.Player == 1 || Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 2500))
                    AI.SelectCard(CardId.ZoroaTheMagistusVerethragna, CardId.ArtemisTheMagistusMoonMaiden, CardId.EndymionTheCrescentMagistus);
                else
                    AI.SelectCard(CardId.ArtemisTheMagistusMoonMaiden, CardId.ZoroaTheMagistusVerethragna, CardId.EndymionTheCrescentMagistus);
                return true;
            }
            else if (ActivateDescription == Util.GetStringId(CardId.ZoroaTheMagistusOfFlame, 1))
            {
                // Special summon Level 4 Spellcaster from hand/GY
                var candidates = Bot.Hand.Concat(Bot.Graveyard)
                    .Where(c => c != null && c.Level == 4 && c.Race == (int)CardRace.SpellCaster && c.Id != CardId.ZoroaTheMagistusOfFlame)
                    .ToList();
                if (candidates.Count > 0)
                {
                    AI.SelectCard(candidates.OrderByDescending(c => c.Attack).ToList());
                    return true;
                }
            }
            return false;
        }

        private bool RillionaSummon()
        {
            return true;
        }

        private bool RillionaEffect()
        {
            if (ShouldSkipCombo()) return false;
            // GY effect: banish self to equip a Magistus Extra Deck monster to a Spellcaster on field
            if (ActivateDescription == Util.GetStringId(CardId.RillionaTheMagistusOfVerre, 1) || Card.Location == CardLocation.Grave)
            {
                var fieldTargets = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.IsCode(CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre, CardId.CrowleyTheGiftedMagistus, CardId.SpentaTheMagistusSealer)).ToList();
                if (fieldTargets.Count > 0)
                {
                    AI.SelectCard(fieldTargets);
                    AI.SelectNextCard(CardId.ArtemisTheMagistusMoonMaiden, CardId.ZoroaTheMagistusVerethragna);
                    return true;
                }
                return false;
            }

            if (EnemyHasKnownNegate() || OpponentHasActiveNegator(Card)) return false;
            // Search Lacrima of Light or Endymion Empire
            AI.SelectCard(CardId.VerreMagicLacrimaOfLight, CardId.EndymionEmpire);
            return true;
        }

        private bool CrowleySummon()
        {
            return true;
        }

        private bool CrowleyEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (ActivateDescription == Util.GetStringId(CardId.CrowleyTheGiftedMagistus, 0))
            {
                // Send another spellcaster to GY to Special Summon
                var discard = Bot.Hand.FirstOrDefault(c => c != Card && c.Race == (int)CardRace.SpellCaster);
                if (discard != null)
                {
                    AI.SelectCard(discard);
                    return true;
                }
                return false;
            }
            
            if (ActivateDescription == Util.GetStringId(CardId.CrowleyTheGiftedMagistus, 1))
            {
                // Declare Attribute
                AI.SelectAttribute(CardAttribute.Light);
                return true;
            }

            if (ActivateDescription == Util.GetStringId(CardId.CrowleyTheGiftedMagistus, 2) || Card.Location == CardLocation.Grave)
            {
                // GY effect: banish self to equip 1 "Magistus" monster we control with 1 "Magistus" monster from GY or Extra Deck (except Level 4)
                var fieldTargets = Bot.GetMonsters().Where(m => m != null && m.IsFaceup() && m.IsCode(CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre, CardId.CrowleyTheGiftedMagistus, CardId.SpentaTheMagistusSealer)).ToList();
                if (fieldTargets.Count > 0)
                {
                    AI.SelectCard(fieldTargets);
                    AI.SelectNextCard(CardId.ArtemisTheMagistusMoonMaiden, CardId.ZoroaTheMagistusVerethragna);
                    return true;
                }
            }
            
            return false;
        }

        private bool LunaSummon()
        {
            return true;
        }

        private bool LunaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ActivateDescription == Util.GetStringId(CardId.FairyTailLuna, 0))
                {
                    // Search effect: Search Rilliona or another Spellcaster with 1850 ATK
                    if (!_lunaSearched)
                    {
                        AI.SelectCard(CardId.RillionaTheMagistusOfVerre, CardId.FairyTailSnow, CardId.FairyTailMatchgiru);
                        _lunaSearched = true;
                        return true;
                    }
                }
                else
                {
                    // Quick bounce effect: target opponent's face-up monster
                    if (Duel.Player == 1)
                    {
                        var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Attack >= 2000 && IsViableEffectTarget(m));
                        if (target != null)
                        {
                            AI.SelectCard(target);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool SpentaSummon()
        {
            return true;
        }

        private bool SpentaEffect()
        {
            // Discard 1 card to SS Level 4 "Magistus" from Deck (locks into Magistus ED)
            if (IsSpecialSummonBlocked()) return false;
            // Check we have a target in deck
            bool hasTarget = GetRemainingCount(CardId.ZoroaTheMagistusOfFlame) > 0 ||
                             GetRemainingCount(CardId.RillionaTheMagistusOfVerre) > 0 ||
                             GetRemainingCount(CardId.CrowleyTheGiftedMagistus) > 0;
            if (!hasTarget) return false;
            // Need a card to discard
            if (Bot.Hand.Count <= (Card.Location == CardLocation.Hand ? 2 : 1)) return false;
            // Prefer discarding duplicates or low-value cards
            var discard = Bot.Hand
                .Where(c => c != null && c != Card)
                .OrderBy(c => {
                    if (HandTraps.Contains(c.Id) && Bot.Hand.Count(h => h != null && h.Id == c.Id) == 1) return 100;
                    if (c.IsCode(CardId.VerreMagicLacrimaOfLight) && Bot.Hand.Count(h => h != null && h.Id == CardId.VerreMagicLacrimaOfLight) == 1) return 90;
                    if (c.IsCode(CardId.CalledByTheGrave)) return 80;
                    int dupCount = Bot.Hand.Count(h => h != null && h.Id == c.Id);
                    if (dupCount > 1) return 5;
                    return 50;
                })
                .FirstOrDefault();
            if (discard != null)
            {
                AI.SelectCard(discard);
                AI.SelectNextCard(CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre, CardId.CrowleyTheGiftedMagistus);
                return true;
            }
            return false;
        }

        private bool MatchgiruSummon()
        {
            return true;
        }

        private bool MatchgiruEffect()
        {
            // On summon: bounce 1 opponent monster to hand
            if (Card.Location != CardLocation.MonsterZone) return false;
            var target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && IsViableEffectTarget(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool GenniSummon()
        {
            return true;
        }

        private bool GenniEffect()
        {
            // Quick Effect: Tribute self + discard 1 Spell to SS Witchcrafter from Deck
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Need a Spell to discard
            if (!Bot.Hand.Any(c => c != null && c.IsSpell() && c != Card)) return false;
            // Better during opponent's turn for disruption or end phase
            if (Duel.Player == 1 || Duel.Phase == DuelPhase.End) return true;
            // Also use on our turn if we need to extend
            if (Bot.GetMonsterCount() < 3 && Bot.Hand.Any(c => c != null && c.IsSpell())) return true;
            return false;
        }

        private bool RegulusSummon()
        {
            return true;
        }

        private bool RegulusEffect()
        {
            // Remove 2 Spell Counters to destroy 1 face-up card on field
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Target opponent's threat
            var target = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && IsViableEffectTarget(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault()
                ?? (ClientCard)Enemy.GetSpells()
                .FirstOrDefault(c => c != null && c.IsFaceup() && IsViableEffectTarget(c));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 4: Extra Deck Summons โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool ArtemisSummon()
        {
            // Artemis requires a "Magistus" monster as Link Material (not just any Spellcaster)
            if (IsSpecialSummonBlocked()) return false;
            int[] magistusIds = { CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre, CardId.CrowleyTheGiftedMagistus, CardId.SpentaTheMagistusSealer };
            var mat = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && magistusIds.Contains(m.Id));
            if (mat != null)
            {
                AI.SelectCard(mat);
                return true;
            }
            return false;
        }

        private bool WiccatSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            // Don't summon if it would use Ace cards or equipped Zoroa
            if (!HasExpendableMaterials(1)) return false;
            return true;
        }

        private bool VerethragnaSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Verethragna needs 2 Level 4 Spellcasters โ€” check we have expendable ones
            int expendableLevel4 = Bot.GetMonsters().Count(c => c != null && c.IsFaceup()
                && c.Level == 4 && !IsAceCard(c) && !HasEquippedMagistus(c));
            if (expendableLevel4 < 2) return false;
            return true;
        }

        private ClientCard GetMagistusCardToDestroy()
        {
            // Prioritize equipped Magistus cards in SpellZone (since we can destroy them without losing a monster)
            var equipTarget = Bot.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup() && 
                c.IsCode(CardId.ArtemisTheMagistusMoonMaiden, CardId.ZoroaTheMagistusVerethragna, CardId.EndymionTheCrescentMagistus, CardId.MagistusChorozo));
            if (equipTarget != null) return equipTarget;

            // Otherwise, target a Magistus monster on the field that is not our main attacker/Ace
            var monsterTarget = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && 
                m.IsCode(CardId.RillionaTheMagistusOfVerre, CardId.CrowleyTheGiftedMagistus, CardId.SpentaTheMagistusSealer));
            if (monsterTarget != null) return monsterTarget;

            // Fallback to any Magistus card we control
            return Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.IsCode(CardId.ZoroaTheMagistusOfFlame)) ?? Card;
        }

        private bool VerethragnaEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY self-revive: target a Magistus card we control to destroy
                var destroyTarget = GetMagistusCardToDestroy();
                if (destroyTarget != null)
                {
                    AI.SelectCard(destroyTarget);
                    return true;
                }
                return false;
            }

            // Otherwise, it's the Xyz Summon equip effect
            // Equip Artemis Moon Maiden from Extra Deck or GY
            AI.SelectCard(CardId.ArtemisTheMagistusMoonMaiden);
            return true;
        }

        private bool ChorozoSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (!HasExpendableMaterials(2)) return false;
            return true;
        }

        private bool CrescentSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool SeleneSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            // Need at least 3 expendable monsters for Link-3
            if (!HasExpendableMaterials(3)) return false;
            // Need Spellcasters in GY for counter generation
            int gySpellcasters = Bot.Graveyard.Count(c => c != null && c.HasRace(CardRace.SpellCaster));
            if (gySpellcasters < 1) return false;
            return true;
        }

        private bool SeleneEffect()
        {
            // Special summon a Spellcaster from hand/GY โ€” prefer ones that can extend
            AI.SelectCard(CardId.ZoroaTheMagistusOfFlame, CardId.RillionaTheMagistusOfVerre, CardId.FairyTailLuna, CardId.CrowleyTheGiftedMagistus);
            return true;
        }

        private bool SummonSorceressSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            if (!HasExpendableMaterials(2)) return false;
            return true;
        }

        private bool WeaverSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Fusion โ€” needs specific materials, don't waste Ace cards
            return true;
        }

        private bool TellerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool Link4Summon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipLinkSummon()) return false;
            // Need at least 4 expendable monsters โ€” NEVER sacrifice Ace cards for this
            if (!HasExpendableMaterials(4)) return false;
            return true;
        }

        /// <summary>
        /// Check if we have enough expendable (non-Ace, non-equipped) monsters to use as material.
        /// </summary>
        private bool HasExpendableMaterials(int required)
        {
            int count = Bot.GetMonsters().Count(c => c != null && c.IsFaceup()
                && !IsAceCard(c) && !HasEquippedMagistus(c));
            return count >= required;
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Low-stat monsters should be in DEF to survive
            int[] lowStatMonsters = {
                CardId.RillionaTheMagistusOfVerre,  // 800 ATK
                CardId.CrowleyTheGiftedMagistus,     // 0 ATK
                CardId.SpentaTheMagistusSealer,       // 1000 ATK
                CardId.WitchcrafterGenni,             // 400 ATK
                CardId.FairyTailLuna,                 // 1850 ATK but fragile
                CardId.AshBlossom,                    // 0 ATK
                CardId.MulcharmyFuwalos,              // 0 ATK
                CardId.DrollAndLockBird               // 0 ATK
            };

            if (lowStatMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }

            return base.OnSelectPosition(cardId, positions);
        }

    }
}
