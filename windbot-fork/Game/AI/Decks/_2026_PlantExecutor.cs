// =========================================================================================
// CARD AUDIT — 2026 Plant (Sunavalon / Rikka / Therion / Aroma Tier-1 Metagame Engine)
// | Card Name                          | Type    | OPT? | Cost   | Effect Summary                                                   |
// | :--------------------------------- | :-----: | :--: | :----: | :--------------------------------------------------------------- |
// | Sunseed Genius Loci (27520594)     | Normal  | No   | None   | 1-Card starter, Link-1 material for Dryas                        |
// | Sunseed Twin (66407907)            | Monster | HOPT | None   | On Normal/SS revives Genius Loci from GY                         |
// | Sunavalon Dryas (93896655)         | Link-1  | -    | None   | Link-1 (Loci). On-summon searches Sunavalon Sowing. Gain LP/SS   |
// | Sunavalon Sowing (53286626)        | Spell   | HOPT | 1000LP | SS Sunseed Twin from Deck, takes 1000 dmg (triggers Dryas LP gain)|
// | Sunavalon Melias (44478599)        | Link-3  | HOPT | None   | Link-3. On Link Summon revives Genius Loci from GY               |
// | Aromaseraphy Jasmine (21200905)    | Link-2  | HOPT | Tribute| Tribute pointed monster -> SS Plant from Deck; Search on LP gain |
// | Therion "Lily" Borea (83610035)    | Monster | HOPT | Send 1 | SS self from hand; Send 1 card to GY -> Search Discolosseum/Regulus|
// | Therion Discolosseum (84792926)    | Field   | HOPT | None   | Field Spell. On activation searches Therion King Regulus          |
// | Therion "King" Regulus (10604644)  | Monster | HOPT | EquipGY| 2800 ATK. Quick Effect Omni-Negate any card or effect             |
// | Rikka Petal (71734607)             | Monster | HOPT | None   | On-summon searches Rikka monster; GY revives in opp End Phase     |
// | Rikka Mudan (71002019)             | Monster | HOPT | Tribute| Hand SS by tributing Plant; On SS searches Rikka S/T (Konkon)    |
// | Rikka Konkon (76869711)            | Field   | HOPT | OppTrib| Set Rikka S/T from Deck; TRIBUTE 1 OPPONENT MONSTER FOR COST!    |
// | Rikka Glamour (69164989)           | Spell   | HOPT | Tribute| Search 1 Rikka (+ 1 same Level Plant if tributed via Konkon/cost) |
// | Rikka Princess (132308)            | Monster | HOPT | Tribute| Hand SS; Quick Effect Monster Negate from Hand/GY by tributing   |
// | Primula the Rikka Fairy (8129306)  | Monster | HOPT | None   | Hand SS on tribute; Increases Levels of 2 Plants by 2             |
// | Snowdrop the Rikka Fairy (33491462)| Monster | HOPT | Tribute| Hand SS self + Plant; Modulates all Plants to matching Level     |
// | Rikka Sheet (68941332)             | Trap    | HOPT | Tribute| Monster negate + TAKE CONTROL if tributed via Konkon/Plant        |
// | Rikka Queen Strenna (3828844)      | Xyz(R4) | HOPT | Detach | Detach to recycle Plant; If tributed -> FLOATING SS TEARDROP!    |
// | Teardrop the Rikka Queen (33779875)| Xyz(R8) | HOPT | Detach | 2800 ATK. Quick Effect TRIBUTE 1 MONSTER on field (Bypasses towers)|
// | Sacred Tree Hyperyton (9349094)    | Xyz(R9) | HOPT | Detach | 2600 ATK. Quick Effect Spell/Monster Negate                      |
// | Sunavalon Dryatrentiay (92770064)  | Link-4  | HOPT | None   | Link-4. Searches Sunavalon Trap / Multi-pop                      |
// | S:P Little Knight (29301450)       | Link-2  | HOPT | None   | Link-2. Banish on summon + Quick double banish                   |
// | Accesscode Talker (86066372)       | Link-4  | None | None   | 5300 ATK OTK finisher with non-target non-respondable pops       |
// | Garura, Subversion of Wings        | Fusion  | None | None   | Super Polymerization target against same Type/Attribute monsters |
// =========================================================================================
// ACE CARDS: Teardrop the Rikka Queen, Therion King Regulus, Sacred Tree Beast Hyperyton, Rikka Queen Strenna, S:P Little Knight, Accesscode Talker
// PRIMARY FULL COMBO: Genius Loci -> Dryas (Search Sowing) -> Sowing (SS Twin, dmg triggers Dryas LP gain) -> Twin revives Loci -> Melias (revives Loci) -> Jasmine (Search Lily Borea + Tribute pointed to SS Mudan) -> Mudan (Search Konkon) -> Konkon (Set Glamour) -> Lily Borea (Search Discolosseum -> Regulus SS) -> Strenna -> Tribute Strenna via Glamour/Konkon -> Strenna floating SS Teardrop!
// END BOARD: Teardrop (Quick Tribute) + Regulus (Omni-Negate) + Konkon (Opponent Tribute) + Princess (Monster Negate) + Sheet (Steal/Negate) -> 5-6 High-Impact Disruptions!
// =========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using static WindBot.Game.AI.ComboRouter;

namespace WindBot.Game.AI.Decks
{
    [Deck("2026_Plant", "2026_Plant")]
    public class _2026_PlantExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Engine
            public const int SunseedGeniusLoci = 27520594;
            public const int SunseedTwin = 66407907;
            public const int RikkaPetal = 71734607;
            public const int RikkaPrincess = 132308;
            public const int RikkaMudan = 71002019;
            public const int PrimulaTheRikkaFairy = 8129306;
            public const int SnowdropTheRikkaFairy = 33491462;
            public const int TherionLilyBorea = 83610035;
            public const int TherionKingRegulus = 10604644;
            public const int Spore = 11747708;

            // Spells
            public const int RikkaGlamour = 69164989;
            public const int RikkaKonkon = 76869711;
            public const int SunavalonSowing = 53286626;
            public const int TherionDiscolosseum = 84792926;
            public const int Terraforming = 73628505;

            // Traps
            public const int RikkaSheet = 68941332;

            // Extra Deck Engine
            public const int SunavalonDryas = 93896655;
            public const int SunavalonMelias = 44478599;
            public const int SunavalonDryatrentiay = 92770064;
            public const int AromaseraphyJasmine = 21200905;
            public const int TeardropTheRikkaQueen = 33779875;
            public const int RikkaQueenStrenna = 3828844;
            public const int SacredTreeBeastHyperyton = 9349094;

            // Staples & Board Breakers
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int CalledByTheGrave = 24224830;
            public const int BookOfMoon = 14087893;
            public const int SuperPolymerization = 48130397;
            public const int ForbiddenDroplet = 24299458;

            // Generic Extra Deck
            public const int SPLittleKnight = 29301450;
            public const int AccesscodeTalker = 86066372;
            public const int Garura = 11765832;
        }

        private static readonly int[] AceCardIds = {
            CardId.TeardropTheRikkaQueen,
            CardId.TherionKingRegulus,
            CardId.SacredTreeBeastHyperyton,
            CardId.RikkaQueenStrenna,
            CardId.SunavalonDryatrentiay,
            CardId.SPLittleKnight,
            CardId.AccesscodeTalker
        };

        private static readonly int[] PlantMonsters = {
            CardId.SunseedGeniusLoci,
            CardId.SunseedTwin,
            CardId.RikkaPetal,
            CardId.RikkaPrincess,
            CardId.RikkaMudan,
            CardId.PrimulaTheRikkaFairy,
            CardId.SnowdropTheRikkaFairy,
            CardId.TherionLilyBorea,
            CardId.Spore,
            CardId.SunavalonDryas,
            CardId.SunavalonMelias,
            CardId.SunavalonDryatrentiay,
            CardId.AromaseraphyJasmine,
            CardId.TeardropTheRikkaQueen,
            CardId.RikkaQueenStrenna,
            CardId.SacredTreeBeastHyperyton
        };

        // Activation Tracking Flags
        private bool _normalSummonUsed = false;
        private bool _dryasSearchUsed = false;
        private bool _sowingUsed = false;
        private bool _twinReviveUsed = false;
        private bool _meliasReviveUsed = false;
        private bool _jasmineSSUsed = false;
        private bool _jasmineSearchUsed = false;
        private bool _lilyBoreaSSUsed = false;
        private bool _lilyBoreaSearchUsed = false;
        private bool _discolosseumUsed = false;
        private bool _regulusSSUsed = false;
        private bool _regulusNegateUsed = false;
        private bool _petalSearchUsed = false;
        private bool _mudanSSUsed = false;
        private bool _mudanSearchUsed = false;
        private bool _konkonSetUsed = false;
        private bool _konkonTributeUsed = false;
        private bool _glamourUsed = false;
        private bool _princessSSUsed = false;
        private bool _princessNegateUsed = false;
        private bool _primulaSSUsed = false;
        private bool _snowdropSSUsed = false;
        private bool _snowdropModulateUsed = false;
        private bool _strennaDetachUsed = false;
        private bool _strennaTributeTriggerUsed = false;
        private bool _teardropUsed = false;
        private bool _hyperytonNegateUsed = false;
        private bool _sheetUsed = false;
        private bool _sporeUsed = false;
        private int _handTrapsUsedThisTurn = 0;

        public override bool IsAceCard(ClientCard card)
        {
            return card != null && AceCardIds.Contains(card.Id);
        }

        protected override bool IsBoardStrongEnough()
        {
            bool hasTeardrop = Bot.HasInMonstersZone(CardId.TeardropTheRikkaQueen);
            bool hasRegulus = Bot.HasInMonstersZone(CardId.TherionKingRegulus);
            bool hasHyperyton = Bot.HasInMonstersZone(CardId.SacredTreeBeastHyperyton);
            bool hasKonkon = Bot.HasInSpellZone(CardId.RikkaKonkon);
            bool hasPrincess = Bot.HasInHand(CardId.RikkaPrincess) || Bot.HasInGraveyard(CardId.RikkaPrincess);
            bool hasSheet = Bot.HasInSpellZone(CardId.RikkaSheet);

            int disruptionCount = 0;
            if (hasTeardrop) disruptionCount += 2;
            if (hasRegulus) disruptionCount += 2;
            if (hasHyperyton) disruptionCount += 1;
            if (hasKonkon && hasPrincess) disruptionCount += 2;
            if (hasSheet) disruptionCount += 1;

            if (disruptionCount >= 4) return true;

            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (Duel.Turn == 1 || (Duel.Player == 0 && Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0))
            {
                if (IsBoardStrongEnough())
                {
                    return true;
                }
            }

            if (CanDealLethal())
            {
                return true;
            }

            return false;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c))
            {
                // Strenna with material is allowed to be tributed to cheat out Teardrop!
                if (c.Id == CardId.RikkaQueenStrenna && c.HasXyzMaterial())
                    return 10;
                if (c.Location == CardLocation.MonsterZone && c.IsFaceup())
                    return 10000;
                return 900;
            }
            if (c.IsCode(CardId.AshBlossom, CardId.MaxxC)) return 800;

            // Preferred Link / Xyz Materials
            if (c.IsCode(CardId.SunseedGeniusLoci)) return 20;
            if (c.IsCode(CardId.SunseedTwin)) return 25;
            if (c.IsCode(CardId.SunavalonDryas)) return 30;
            if (c.IsCode(CardId.SunavalonMelias)) return 35;
            if (c.IsCode(CardId.Spore)) return 40;
            if (c.IsCode(CardId.RikkaPetal)) return 45;
            if (c.IsCode(CardId.PrimulaTheRikkaFairy)) return 50;
            if (c.IsCode(CardId.RikkaPrincess)) return 60;
            if (c.IsCode(CardId.TherionLilyBorea)) return 70;
            if (c.IsCode(CardId.RikkaMudan)) return 80;

            return 100;
        }

        public override IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            var protectedCards = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c) && !(c.Id == CardId.RikkaQueenStrenna && c.HasXyzMaterial()))).ToList();
            if (protectedCards.Count >= min)
            {
                var sorted = protectedCards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }
            return cards.OrderBy(c => GetMaterialPriority(c)).Take(max).ToList();
        }

        public override IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            var protectedCards = cards.Where(c => c != null && !(c.Location == CardLocation.MonsterZone && IsAceCard(c))).ToList();
            if (protectedCards.Count >= min)
            {
                var sorted = protectedCards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }
            return base.OnSelectXyzMaterial(cards, min, max);
        }

        public _2026_PlantExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);
            ResourcePlan.RegisterAceCards(AceCardIds);

            // Register Strategic Combo Lines in ComboRouter
            ComboRouter.RegisterLine(new ComboLine
            {
                Name = "Plant-Loci-Sunavalon-Full-Combo",
                RequiredCards = new List<int> { CardId.SunseedGeniusLoci },
                EndBoardScore = 95,
                Steps = new List<ComboStep>
                {
                    new() { CardId = CardId.SunseedGeniusLoci, ActionType = ExecutorType.Summon, Description = "Normal Summon Genius Loci" },
                    new() { CardId = CardId.SunavalonDryas, ActionType = ExecutorType.SpSummon, Description = "Link Summon Dryas using Loci" },
                    new() { CardId = CardId.SunavalonDryas, ActionType = ExecutorType.Activate, Description = "Dryas search Sunavalon Sowing" },
                    new() { CardId = CardId.SunavalonSowing, ActionType = ExecutorType.Activate, Description = "Sowing SS Sunseed Twin from Deck" },
                    new() { CardId = CardId.SunseedTwin, ActionType = ExecutorType.Activate, Description = "Twin revives Genius Loci from GY" },
                    new() { CardId = CardId.SunavalonMelias, ActionType = ExecutorType.SpSummon, Description = "Link Summon Melias" },
                    new() { CardId = CardId.SunavalonMelias, ActionType = ExecutorType.Activate, Description = "Melias revives Genius Loci from GY" },
                    new() { CardId = CardId.AromaseraphyJasmine, ActionType = ExecutorType.SpSummon, Description = "Link Summon Aromaseraphy Jasmine" },
                    new() { CardId = CardId.AromaseraphyJasmine, ActionType = ExecutorType.Activate, Description = "Jasmine tributes monster to SS Mudan" },
                    new() { CardId = CardId.RikkaMudan, ActionType = ExecutorType.Activate, Description = "Mudan search Konkon" },
                    new() { CardId = CardId.RikkaKonkon, ActionType = ExecutorType.Activate, Description = "Konkon sets Rikka Sheet/Glamour" },
                    new() { CardId = CardId.RikkaQueenStrenna, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Rikka Queen Strenna" }
                }
            });

            ComboRouter.RegisterLine(new ComboLine
            {
                Name = "Plant-Petal-Rikka-Setup",
                RequiredCards = new List<int> { CardId.RikkaPetal },
                EndBoardScore = 85,
                Steps = new List<ComboStep>
                {
                    new() { CardId = CardId.RikkaPetal, ActionType = ExecutorType.Summon, Description = "Normal Summon Rikka Petal" },
                    new() { CardId = CardId.RikkaPetal, ActionType = ExecutorType.Activate, Description = "Petal searches Mudan or Princess" },
                    new() { CardId = CardId.RikkaMudan, ActionType = ExecutorType.Activate, Description = "SS Mudan searching Konkon" },
                    new() { CardId = CardId.RikkaKonkon, ActionType = ExecutorType.Activate, Description = "Activate Konkon and set Sheet" },
                    new() { CardId = CardId.RikkaQueenStrenna, ActionType = ExecutorType.SpSummon, Description = "Xyz Summon Strenna" }
                }
            });

            ComboRouter.RegisterLine(new ComboLine
            {
                Name = "Plant-Going2nd-BoardBreak",
                RequiredCards = new List<int> { CardId.SuperPolymerization },
                EndBoardScore = 80,
                Steps = new List<ComboStep>
                {
                    new() { CardId = CardId.SuperPolymerization, ActionType = ExecutorType.Activate, Description = "Fuse opponent monsters into Garura" },
                    new() { CardId = CardId.SunseedGeniusLoci, ActionType = ExecutorType.Summon, Description = "Normal Summon Genius Loci" },
                    new() { CardId = CardId.SunavalonDryas, ActionType = ExecutorType.SpSummon, Description = "Link Summon Dryas" }
                }
            });

            // ============================================================
            // TIER 1: Hand Traps & Fast Disruptions (Both Turns)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCActivate);
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);

            // ============================================================
            // TIER 2: Board Breakers (Going 2nd Priority)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolyActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, DropletActivate);
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonActivate);

            // ============================================================
            // TIER 3: Boss Quick Effects & Negates (Opponent Turn & Chains)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.TherionKingRegulus, RegulusNegate);
            AddExecutor(ExecutorType.Activate, CardId.TeardropTheRikkaQueen, TeardropActivate);
            AddExecutor(ExecutorType.Activate, CardId.SacredTreeBeastHyperyton, HyperytonNegate);
            AddExecutor(ExecutorType.Activate, CardId.RikkaPrincess, PrincessNegate);
            AddExecutor(ExecutorType.Activate, CardId.RikkaSheet, SheetActivate);
            AddExecutor(ExecutorType.Activate, CardId.RikkaQueenStrenna, StrennaActivate);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, LittleKnightActivate);

            // ============================================================
            // TIER 3.5: Instant Link-1 Evolution (Loci -> Dryas immediately!)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.SunavalonDryas, DryasSummon);

            // ============================================================
            // TIER 4: Field & Search Spells (Setup Engines)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingActivate);
            AddExecutor(ExecutorType.Activate, CardId.TherionDiscolosseum, DiscolosseumActivate);
            AddExecutor(ExecutorType.Activate, CardId.RikkaKonkon, KonkonActivate);
            AddExecutor(ExecutorType.Activate, CardId.SunavalonSowing, SowingActivate);
            AddExecutor(ExecutorType.Activate, CardId.RikkaGlamour, GlamourActivate);

            // ============================================================
            // TIER 5: Monster Effects (Hand / Field / GY)
            // ============================================================
            AddExecutor(ExecutorType.Activate, CardId.TherionLilyBorea, LilyBoreaActivate);
            AddExecutor(ExecutorType.Activate, CardId.TherionKingRegulus, RegulusSS);
            AddExecutor(ExecutorType.Activate, CardId.RikkaMudan, MudanActivate);
            AddExecutor(ExecutorType.Activate, CardId.RikkaPetal, PetalActivate);
            AddExecutor(ExecutorType.Activate, CardId.PrimulaTheRikkaFairy, PrimulaActivate);
            AddExecutor(ExecutorType.Activate, CardId.SnowdropTheRikkaFairy, SnowdropActivate);
            AddExecutor(ExecutorType.Activate, CardId.RikkaPrincess, PrincessSS);
            AddExecutor(ExecutorType.Activate, CardId.AromaseraphyJasmine, JasmineActivate);
            AddExecutor(ExecutorType.Activate, CardId.SunavalonDryas, DryasActivate);
            AddExecutor(ExecutorType.Activate, CardId.SunseedTwin, TwinActivate);
            AddExecutor(ExecutorType.Activate, CardId.SunavalonMelias, MeliasActivate);
            AddExecutor(ExecutorType.Activate, CardId.Spore, SporeActivate);

            // ============================================================
            // TIER 6: Normal Summons (Starters)
            // ============================================================
            AddExecutor(ExecutorType.Summon, CardId.SunseedGeniusLoci, GeniusSummon);
            AddExecutor(ExecutorType.Summon, CardId.RikkaPetal, GenericPlantSummon);
            AddExecutor(ExecutorType.Summon, CardId.RikkaPrincess, GenericPlantSummon);
            AddExecutor(ExecutorType.Summon, CardId.SunseedTwin, GenericPlantSummon);
            AddExecutor(ExecutorType.Summon, CardId.Spore, GenericPlantSummon);

            // ============================================================
            // TIER 7: Extra Deck Summons (Climbing -> Bosses)
            // ============================================================
            AddExecutor(ExecutorType.SpSummon, CardId.SunavalonMelias, MeliasSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AromaseraphyJasmine, JasmineSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.RikkaQueenStrenna, StrennaSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.TeardropTheRikkaQueen, TeardropSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SacredTreeBeastHyperyton, HyperytonSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SunavalonDryatrentiay, DryatrentiaySummon);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.AccesscodeTalker, AccesscodeSummon);

            // ============================================================
            // TIER 8: Sets & Position
            // ============================================================
            AddExecutor(ExecutorType.SpellSet, CardId.RikkaSheet, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.BookOfMoon, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, () => Util.IsTurn1OrMain2());

            AddExecutor(ExecutorType.Repos, PlantMonsterRepos);
        }

        public override bool OnSelectHand() => true;

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _normalSummonUsed = false;
            _dryasSearchUsed = false;
            _sowingUsed = false;
            _twinReviveUsed = false;
            _meliasReviveUsed = false;
            _jasmineSSUsed = false;
            _jasmineSearchUsed = false;
            _lilyBoreaSSUsed = false;
            _lilyBoreaSearchUsed = false;
            _discolosseumUsed = false;
            _regulusSSUsed = false;
            _regulusNegateUsed = false;
            _petalSearchUsed = false;
            _mudanSSUsed = false;
            _mudanSearchUsed = false;
            _konkonSetUsed = false;
            _konkonTributeUsed = false;
            _glamourUsed = false;
            _princessSSUsed = false;
            _princessNegateUsed = false;
            _primulaSSUsed = false;
            _snowdropSSUsed = false;
            _snowdropModulateUsed = false;
            _strennaDetachUsed = false;
            _strennaTributeTriggerUsed = false;
            _teardropUsed = false;
            _hyperytonNegateUsed = false;
            _sheetUsed = false;
            _sporeUsed = false;
            _handTrapsUsedThisTurn = 0;
        }

        // ============================================================
        // CALLBAKS & OVERRIDES (OnSelectCard / YesNo / Option)
        // ============================================================

        public override bool OnSelectYesNo(long desc)
        {
            // Agree to optional tribute for Rikka Glamour double search
            if (LastChainCard != null && LastChainCard.Id == CardId.RikkaGlamour)
            {
                DecisionTracer.TraceSelect("OnSelectYesNo", "Agreeing to optional tribute for Rikka Glamour double search", null);
                return true;
            }
            // Agree to attach Strenna as material when cheated out
            if (LastChainCard != null && LastChainCard.Id == CardId.RikkaQueenStrenna)
            {
                return true;
            }
            // Sunavalon Dryas gain LP and SS link
            if (LastChainCard != null && LastChainCard.Id == CardId.SunavalonDryas)
            {
                return true;
            }
            // Rikka Sheet optional tribute to take control
            if (LastChainCard != null && LastChainCard.Id == CardId.RikkaSheet)
            {
                return true;
            }
            return base.OnSelectYesNo(desc);
        }

        public override int OnSelectOption(IList<long> options)
        {
            // Rikka Petal: Option 0 = Add to hand, Option 1 = Send to GY
            if (LastChainCard != null && LastChainCard.Id == CardId.RikkaPetal)
            {
                return 0;
            }
            return base.OnSelectOption(options);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // 1. Rikka Konkon Tribute Cost Substitution (Tribute opponent's face-up monster!)
            if (Bot.HasInSpellZone(CardId.RikkaKonkon) && !_konkonTributeUsed)
            {
                var oppMonsters = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup())
                    .OrderByDescending(c => {
                        int score = 10000;
                        if (c.Attack >= 2500) score += 5000;
                        if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) score += 3000;
                        return score + c.Attack;
                    })
                    .ToList();
                if (oppMonsters.Count > 0)
                {
                    _konkonTributeUsed = true;
                    DecisionTracer.TraceSelect("OnSelectCard", "Tributing opponent monster via Rikka Konkon cost substitution", oppMonsters.First());
                    return new List<ClientCard> { oppMonsters.First() };
                }
            }

            // 2. Strenna Tribute for Floating Effect: If Strenna is available as tribute cost and has material, tribute Strenna!
            var strennaWithMat = cards.FirstOrDefault(c => c != null && c.Controller == 0 && c.Id == CardId.RikkaQueenStrenna && c.HasXyzMaterial());
            if (strennaWithMat != null && (LastChainCard == null || LastChainCard.Id != CardId.RikkaQueenStrenna))
            {
                DecisionTracer.TraceSelect("OnSelectCard", "Tributing Rikka Queen Strenna to trigger floating Boss SS", strennaWithMat);
                return new List<ClientCard> { strennaWithMat };
            }

            // 3. Search Deck (Hint 506 = HINTMSG_ATOHAND)
            if (hint == 506)
            {
                bool needKonkon = !Bot.HasInHand(CardId.RikkaKonkon) && !Bot.HasInSpellZone(CardId.RikkaKonkon);
                if (needKonkon && cards.Any(c => c.Id == CardId.RikkaKonkon))
                    return new List<ClientCard> { cards.First(c => c.Id == CardId.RikkaKonkon) };

                var preferred = cards.OrderBy(c => {
                    if (c == null) return 999;
                    if (c.Id == CardId.RikkaKonkon) return 1;
                    if (c.Id == CardId.TherionDiscolosseum && !Bot.HasInSpellZone(CardId.TherionDiscolosseum)) return 2;
                    if (c.Id == CardId.TherionKingRegulus && !Bot.HasInHand(CardId.TherionKingRegulus)) return 3;
                    if (c.Id == CardId.TherionLilyBorea && !Bot.HasInHand(CardId.TherionLilyBorea)) return 4;
                    if (c.Id == CardId.RikkaGlamour && !_glamourUsed) return 5;
                    if (c.Id == CardId.RikkaMudan && !Bot.HasInHand(CardId.RikkaMudan)) return 6;
                    if (c.Id == CardId.RikkaPrincess && !Bot.HasInHand(CardId.RikkaPrincess)) return 7;
                    if (c.Id == CardId.SnowdropTheRikkaFairy && !Bot.HasInHand(CardId.SnowdropTheRikkaFairy)) return 8;
                    if (c.Id == CardId.PrimulaTheRikkaFairy && !Bot.HasInHand(CardId.PrimulaTheRikkaFairy)) return 9;
                    if (c.Id == CardId.RikkaPetal) return 10;
                    if (c.Id == CardId.RikkaSheet && !Bot.HasInSpellZone(CardId.RikkaSheet)) return 11;
                    return 50;
                }).ToList();
                return Util.CheckSelectCount(preferred, cards, min, max);
            }

            // 4. Special Summon from Deck / Extra Deck / GY (Hint 509 / 503)
            if (hint == 509 || hint == 503)
            {
                // Jasmine summon from Deck: prioritize Mudan or Snowdrop or Lily Borea
                var deckCards = cards.Where(c => c != null && c.Location == CardLocation.Deck).ToList();
                if (deckCards.Count > 0)
                {
                    var preferred = deckCards.OrderBy(c => {
                        if (c.Id == CardId.RikkaMudan && !Bot.HasInSpellZone(CardId.RikkaKonkon)) return 1;
                        if (c.Id == CardId.TherionLilyBorea && !Bot.HasInHand(CardId.TherionKingRegulus)) return 2;
                        if (c.Id == CardId.SnowdropTheRikkaFairy) return 3;
                        if (c.Id == CardId.RikkaPrincess) return 4;
                        if (c.Id == CardId.SunseedTwin) return 5;
                        if (c.Id == CardId.RikkaPetal) return 6;
                        if (c.Id == CardId.SunseedGeniusLoci) return 7;
                        if (c.Id == CardId.Spore) return 8;
                        return 20;
                    }).ToList();
                    return Util.CheckSelectCount(preferred, cards, min, max);
                }

                // Strenna floating summon from Extra Deck -> Teardrop or Hyperyton!
                var extraCards = cards.Where(c => c != null && c.Location == CardLocation.Extra).ToList();
                if (extraCards.Count > 0)
                {
                    var teardrop = extraCards.FirstOrDefault(c => c.Id == CardId.TeardropTheRikkaQueen)
                        ?? extraCards.FirstOrDefault(c => c.Id == CardId.SacredTreeBeastHyperyton);
                    if (teardrop != null) return new List<ClientCard> { teardrop };
                }

                // Twin / Melias revive target from GY -> Sunseed Genius Loci
                var gyCards = cards.Where(c => c != null && c.Location == CardLocation.Grave).ToList();
                if (gyCards.Count > 0)
                {
                    var loci = gyCards.FirstOrDefault(c => c.Id == CardId.SunseedGeniusLoci);
                    if (loci != null) return new List<ClientCard> { loci };
                }
            }

            // 5. Target / Destruction / Banish / Quick Tribute (Hint 502 / 504 / 551 / 575)
            if (hint == 502 || hint == 504 || hint == 551 || hint == 575)
            {
                var oppCards = cards.Where(c => c != null && c.Controller == 1).OrderByDescending(c => {
                    int score = 10000;
                    if (c.IsMonster())
                    {
                        if (c.IsFaceup() && !c.IsDisabled())
                        {
                            if (c.Attack >= 2500) score += 5000;
                            if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) score += 2000;
                        }
                        return score + c.Attack;
                    }
                    if (c.IsSpell() || c.IsTrap())
                    {
                        if (c.IsFaceup() && (c.HasType(CardType.Continuous) || c.HasType(CardType.Field))) score += 6000;
                        else score += 2000;
                        return score;
                    }
                    return score;
                }).ToList();

                if (oppCards.Count >= min)
                    return oppCards.Take(max).ToList();
            }

            // 6. Material selection (Protect Ace cards)
            if (hint == 511 || hint == 512 || hint == 513 || hint == 533 || hint == 508)
            {
                var sorted = cards.Where(c => c != null)
                    .OrderBy(c => GetMaterialPriority(c))
                    .ToList();

                if (cancelable)
                {
                    var nonFieldAces = sorted.Where(c => !(c.Location == CardLocation.MonsterZone && IsAceCard(c) && !(c.Id == CardId.RikkaQueenStrenna && c.HasXyzMaterial()))).ToList();
                    if (nonFieldAces.Count < min) return null;
                    return Util.CheckSelectCount(nonFieldAces, cards, min, max);
                }
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        // ============================================================
        // HAND TRAPS & REACTIVE DISRUPTIONS
        // ============================================================

        private bool AshActivate()
        {
            if (Util.GetLastChainCard()?.Controller == 0) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;
            if (DefaultAshBlossomAndJoyousSpring())
            {
                _handTrapsUsedThisTurn++;
                DecisionTracer.TraceActivate("AshBlossom", "Negating search/SS from deck");
                return true;
            }
            return false;
        }

        private bool MaxxCActivate()
        {
            if (Duel.Player == 0) return false;
            if (_handTrapsUsedThisTurn >= 2) return false;
            if (DefaultMaxxC())
            {
                _handTrapsUsedThisTurn++;
                DecisionTracer.TraceActivate("MaxxC", "Chaining Maxx C to opponent SS");
                return true;
            }
            return false;
        }

        // ============================================================
        // BOARD BREAKERS & STAPLES
        // ============================================================

        private bool SuperPolyActivate()
        {
            if (IsSpecialSummonBlocked()) return false;
            var faceupEnemies = Enemy.MonsterZone.Where(c => c != null && c.IsFaceup()).ToList();
            if (faceupEnemies.Count >= 2)
            {
                for (int i = 0; i < faceupEnemies.Count; i++)
                {
                    for (int j = i + 1; j < faceupEnemies.Count; j++)
                    {
                        var a = faceupEnemies[i];
                        var b = faceupEnemies[j];
                        // Garura check: same Type & Attribute, different names
                        if (a.Race == b.Race && a.Attribute == b.Attribute && a.Id != b.Id)
                        {
                            DecisionTracer.TraceActivate("SuperPolymerization", $"Fusing {a.Name} and {b.Name} into Garura!");
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool DropletActivate()
        {
            if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard()?.Controller == 1)
            {
                var sendTarget = Bot.Hand.FirstOrDefault(c => c != null && c.Id == CardId.Spore || c.Id == CardId.Terraforming)
                    ?? Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c));
                if (sendTarget != null) AI.SelectCard(sendTarget);
                return true;
            }
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
            {
                var enemyBoss = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && (c.Attack >= 2500 || OpponentHasActiveNegator()));
                if (enemyBoss != null)
                {
                    var sendTarget = Bot.Hand.FirstOrDefault(c => c != null && (c.Id == CardId.Spore || c.Id == CardId.Terraforming))
                        ?? Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && !IsAceCard(c));
                    if (sendTarget != null)
                    {
                        AI.SelectCard(sendTarget);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool BookOfMoonActivate()
        {
            if (Duel.LastChainPlayer == 1 && Util.GetLastChainCard()?.Controller == 1)
            {
                var target = Util.GetLastChainCard();
                if (target != null && target.Location == CardLocation.MonsterZone && target.IsFaceup() && !target.IsShouldNotBeTarget())
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            var oppThreat = Enemy.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget() && !c.HasType(CardType.Link))
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (oppThreat != null)
            {
                AI.SelectCard(oppThreat);
                return true;
            }
            return false;
        }

        // ============================================================
        // BOSS QUICK EFFECTS & DISRUPTIONS
        // ============================================================

        private bool RegulusNegate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (_regulusNegateUsed) return false;
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;

            _regulusNegateUsed = true;
            DecisionTracer.TraceActivate("RegulusNegate", $"Omni-negating opponent activation {LastChainCard.Name}");
            return true;
        }

        private bool TeardropActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (!Card.HasXyzMaterial()) return false;
            if (_teardropUsed) return false;

            // In Opponent turn: Tribute opponent monster when they summon or start activating effects
            if (Duel.Player == 1)
            {
                var target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => {
                        int score = 10000;
                        if (c.Attack >= 2500) score += 5000;
                        if (c.HasType(CardType.Fusion) || c.HasType(CardType.Synchro) || c.HasType(CardType.Xyz) || c.HasType(CardType.Link)) score += 2000;
                        return score + c.Attack;
                    })
                    .FirstOrDefault();

                if (target != null)
                {
                    _teardropUsed = true;
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("TeardropActivate", $"Quick tributing opponent threat {target.Name}");
                    return true;
                }
            }

            // In our turn: Clear biggest opponent threat before attacking
            if (Duel.Player == 0)
            {
                var target = Enemy.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget())
                    .OrderByDescending(c => c.Attack)
                    .FirstOrDefault();

                if (target != null)
                {
                    _teardropUsed = true;
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("TeardropActivate", $"Tributing opponent monster {target.Name}");
                    return true;
                }
            }

            return false;
        }

        private bool HyperytonNegate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (!Card.HasXyzMaterial()) return false;
            if (_hyperytonNegateUsed) return false;
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;

            _hyperytonNegateUsed = true;
            DecisionTracer.TraceActivate("HyperytonNegate", $"Negating opponent activation {LastChainCard.Name}");
            return true;
        }

        private bool PrincessNegate()
        {
            if (_princessNegateUsed) return false;
            if (LastChainCard == null || LastChainCard.Controller == 0 || !LastChainCard.IsMonster()) return false;

            bool controlsRikka = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && IsRikkaMonster(c));
            if (!controlsRikka) return false;

            _princessNegateUsed = true;
            DecisionTracer.TraceActivate("PrincessNegate", $"Shuffling Princess to negate opponent monster {LastChainCard.Name}");
            return true;
        }

        private bool SheetActivate()
        {
            if (_sheetUsed) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            var target = Enemy.MonsterZone.Where(c => c != null && c.IsFaceup() && !c.IsDisabled() && !c.IsShouldNotBeTarget())
                .OrderByDescending(c => c.Attack)
                .FirstOrDefault();

            if (target != null)
            {
                _sheetUsed = true;
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("RikkaSheet", $"Negating and stealing opponent monster {target.Name}");
                return true;
            }
            return false;
        }

        private bool StrennaActivate()
        {
            // Effect 1: Detach to recycle Plant / Rikka card
            if (Card.Location == CardLocation.MonsterZone && Card.HasXyzMaterial() && !_strennaDetachUsed)
            {
                var recycleTarget = Bot.Graveyard.FirstOrDefault(c => c != null && (c.Id == CardId.RikkaPrincess || c.Id == CardId.TherionKingRegulus || c.Id == CardId.TherionLilyBorea || c.Id == CardId.RikkaKonkon));
                if (recycleTarget != null)
                {
                    _strennaDetachUsed = true;
                    AI.SelectCard(recycleTarget);
                    DecisionTracer.TraceActivate("StrennaActivate", $"Detaching material to recycle {recycleTarget.Name}");
                    return true;
                }
            }

            // Effect 2: On-Tribute Floating Trigger in GY -> Special Summon Teardrop from Extra Deck!
            if (Card.Location == CardLocation.Grave && !_strennaTributeTriggerUsed)
            {
                _strennaTributeTriggerUsed = true;
                AI.SelectCard(CardId.TeardropTheRikkaQueen, CardId.SacredTreeBeastHyperyton);
                DecisionTracer.TraceActivate("StrennaActivate", "Strenna tributed: Special Summoning Teardrop the Rikka Queen from Extra Deck!");
                return true;
            }

            return false;
        }

        private bool LittleKnightActivate()
        {
            var target = Enemy.MonsterZone.Where(c => c != null && c.IsFaceup() && !c.IsShouldNotBeTarget()).OrderByDescending(c => c.Attack).FirstOrDefault()
                ?? Enemy.SpellZone.FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // ============================================================
        // FIELD & SEARCH SPELLS
        // ============================================================

        private bool TerraformingActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (Bot.HasInSpellZone(CardId.RikkaKonkon) || Bot.HasInHand(CardId.RikkaKonkon))
                return false;
            return GetRemainingCount(CardId.RikkaKonkon) > 0;
        }

        private bool DiscolosseumActivate()
        {
            if (_discolosseumUsed || ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.TherionDiscolosseum)) return false;
                _discolosseumUsed = true;
                AI.SelectCard(CardId.TherionKingRegulus);
                DecisionTracer.TraceActivate("TherionDiscolosseum", "Search Therion King Regulus");
                return true;
            }
            return false;
        }

        private bool KonkonActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.RikkaKonkon)) return false;
                DecisionTracer.TraceActivate("RikkaKonkon", "Activating Rikka Konkon from hand");
                return true;
            }
            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup() && Duel.Player == 0)
            {
                if (_konkonSetUsed) return false;
                bool hasRikka = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && IsRikkaMonster(c));
                if (hasRikka)
                {
                    _konkonSetUsed = true;
                    if (GetRemainingCount(CardId.RikkaGlamour) > 0 && !_glamourUsed)
                        AI.SelectCard(CardId.RikkaGlamour);
                    else
                        AI.SelectCard(CardId.RikkaSheet);

                    DecisionTracer.TraceActivate("RikkaKonkon", "Setting Rikka Spell/Trap directly from Deck");
                    return true;
                }
            }
            return false;
        }

        private bool SowingActivate()
        {
            if (_sowingUsed || ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.Hand) return false;

            if (GetRemainingCount(CardId.SunseedTwin) > 0 || GetRemainingCount(CardId.SunseedGeniusLoci) > 0)
            {
                _sowingUsed = true;
                AI.SelectCard(CardId.SunseedTwin, CardId.SunseedGeniusLoci);
                DecisionTracer.TraceActivate("SunavalonSowing", "SS Sunseed Twin/Loci from Deck");
                return true;
            }
            return false;
        }

        private bool GlamourActivate()
        {
            if (_glamourUsed || ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Hand && Card.Location != CardLocation.SpellZone) return false;

            _glamourUsed = true;
            DecisionTracer.TraceActivate("RikkaGlamour", "Search Rikka Princess / Primula / Mudan");
            return true;
        }

        // ============================================================
        // MONSTER EFFECTS & EXTENSIONS
        // ============================================================

        private bool LilyBoreaActivate()
        {
            // Effect 1: Hand Special Summon
            if (Card.Location == CardLocation.Hand && !_lilyBoreaSSUsed && !IsSpecialSummonBlocked())
            {
                var equipTarget = Bot.Graveyard.FirstOrDefault(c => c != null && (PlantMonsters.Contains(c.Id) || c.Id == CardId.TherionKingRegulus));
                if (equipTarget != null)
                {
                    _lilyBoreaSSUsed = true;
                    AI.SelectCard(equipTarget);
                    DecisionTracer.TraceActivate("TherionLilyBorea", $"SS Lily Borea equipping {equipTarget.Name} from GY");
                    return true;
                }
            }

            // Effect 2: On field search Discolosseum or Regulus
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && !_lilyBoreaSearchUsed)
            {
                _lilyBoreaSearchUsed = true;
                var sendTarget = Bot.MonsterZone.FirstOrDefault(c => c != null && c.IsFaceup() && (c.Id == CardId.SunavalonDryas || c.Id == CardId.SunavalonMelias))
                    ?? Card;
                AI.SelectCard(sendTarget);
                DecisionTracer.TraceActivate("TherionLilyBorea", $"Lily Borea sending {sendTarget.Name} to search Therion Discolosseum");
                return true;
            }

            return false;
        }

        private bool RegulusSS()
        {
            if (_regulusSSUsed || ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.Hand) return false;

            var therionTarget = Bot.Graveyard.FirstOrDefault(c => c != null && (c.Id == CardId.TherionLilyBorea || c.Id == CardId.TherionKingRegulus || c.Race == (int)CardRace.Machine));
            if (therionTarget != null)
            {
                _regulusSSUsed = true;
                AI.SelectCard(therionTarget);
                DecisionTracer.TraceActivate("TherionKingRegulus", $"SS Regulus from hand equipping {therionTarget.Name} from GY");
                return true;
            }
            return false;
        }

        private bool MudanActivate()
        {
            if (Card.Location == CardLocation.Hand && !_mudanSSUsed && !IsSpecialSummonBlocked())
            {
                bool hasTribute = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && PlantMonsters.Contains(c.Id) && !IsAceCard(c) && c.Id != CardId.SunseedGeniusLoci)
                    || (Bot.HasInSpellZone(CardId.RikkaKonkon) && Enemy.MonsterZone.Any(c => c != null && c.IsFaceup()));
                if (hasTribute)
                {
                    _mudanSSUsed = true;
                    DecisionTracer.TraceActivate("RikkaMudan", "Tributing Plant (or opponent monster via Konkon) to SS Mudan from hand");
                    return true;
                }
            }

            if (Card.Location == CardLocation.MonsterZone && !_mudanSearchUsed)
            {
                _mudanSearchUsed = true;
                if (!Bot.HasInSpellZone(CardId.RikkaKonkon) && !Bot.HasInHand(CardId.RikkaKonkon) && GetRemainingCount(CardId.RikkaKonkon) > 0)
                    AI.SelectCard(CardId.RikkaKonkon);
                else
                    AI.SelectCard(CardId.RikkaGlamour, CardId.RikkaSheet);

                DecisionTracer.TraceActivate("RikkaMudan", "Mudan on-summon trigger searching Rikka Spell/Trap");
                return true;
            }

            return false;
        }

        private bool PetalActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && !_petalSearchUsed)
            {
                _petalSearchUsed = true;
                AI.SelectCard(CardId.RikkaMudan, CardId.RikkaPrincess, CardId.SnowdropTheRikkaFairy);
                DecisionTracer.TraceActivate("RikkaPetal", "Search Rikka Mudan / Princess from Deck");
                return true;
            }
            if (Card.Location == CardLocation.Grave && Duel.Player == 1 && Duel.Phase == DuelPhase.End)
            {
                bool validBoard = Bot.MonsterZone.All(c => c == null || PlantMonsters.Contains(c.Id));
                if (validBoard)
                {
                    DecisionTracer.TraceActivate("RikkaPetal", "Reviving Rikka Petal from GY in opponent's End Phase");
                    return true;
                }
            }
            return false;
        }

        private bool PrimulaActivate()
        {
            if (Card.Location == CardLocation.Hand && !_primulaSSUsed && !IsSpecialSummonBlocked())
            {
                _primulaSSUsed = true;
                DecisionTracer.TraceActivate("PrimulaTheRikkaFairy", "SS Primula from hand upon monster tribute");
                return true;
            }
            return false;
        }

        private bool SnowdropActivate()
        {
            if (Card.Location == CardLocation.Hand && !_snowdropSSUsed && !IsSpecialSummonBlocked())
            {
                bool hasTribute = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && PlantMonsters.Contains(c.Id) && !IsAceCard(c))
                    || (Bot.HasInSpellZone(CardId.RikkaKonkon) && Enemy.MonsterZone.Any(c => c != null && c.IsFaceup()));
                if (hasTribute)
                {
                    _snowdropSSUsed = true;
                    DecisionTracer.TraceActivate("SnowdropTheRikkaFairy", "Snowdrop tribute to SS self and Plant from hand");
                    return true;
                }
            }

            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup() && !_snowdropModulateUsed)
            {
                _snowdropModulateUsed = true;
                AI.SelectCard(Card);
                DecisionTracer.TraceActivate("SnowdropTheRikkaFairy", "Modulating all Plants to Level 8 for Teardrop Xyz");
                return true;
            }

            return false;
        }

        private bool PrincessSS()
        {
            if (_princessSSUsed || ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.Hand) return false;

            bool hasOtherMonster = Bot.MonsterZone.Any(c => c != null && c.IsFaceup());
            bool validBoard = Bot.MonsterZone.All(c => c == null || PlantMonsters.Contains(c.Id));
            if (validBoard && (hasOtherMonster || !Bot.HasInHand(CardId.SunseedGeniusLoci)))
            {
                _princessSSUsed = true;
                DecisionTracer.TraceActivate("RikkaPrincess", "SS Rikka Princess from hand");
                return true;
            }
            return false;
        }

        private bool JasmineActivate()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                // Tribute pointed monster -> SS Plant from Deck
                if (!_jasmineSSUsed)
                {
                    var pointedMonsters = Bot.MonsterZone.Where(c => c != null && c.IsFaceup() && c.Id != CardId.AromaseraphyJasmine
                        && (!IsAceCard(c) || (c.Id == CardId.RikkaQueenStrenna && c.HasXyzMaterial()))
                        && c.Id != CardId.SunseedGeniusLoci).ToList();
                    if (pointedMonsters.Count > 0)
                    {
                        _jasmineSSUsed = true;
                        AI.SelectCard(pointedMonsters.First());
                        DecisionTracer.TraceActivate("AromaseraphyJasmine", $"Tributing {pointedMonsters.First().Name} to SS Plant from Deck");
                        return true;
                    }
                }

                // Trigger Effect: Search Plant on gaining LP
                if (!_jasmineSearchUsed)
                {
                    _jasmineSearchUsed = true;
                    AI.SelectCard(CardId.TherionLilyBorea, CardId.RikkaMudan, CardId.SnowdropTheRikkaFairy, CardId.RikkaPrincess);
                    DecisionTracer.TraceActivate("AromaseraphyJasmine", "LP gain triggered: Search Plant monster from Deck");
                    return true;
                }
            }
            return false;
        }

        private bool DryasActivate()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // Search Sowing on summon
            if (!_dryasSearchUsed && !Bot.HasInHand(CardId.SunavalonSowing) && GetRemainingCount(CardId.SunavalonSowing) > 0)
            {
                _dryasSearchUsed = true;
                AI.SelectCard(CardId.SunavalonSowing);
                DecisionTracer.TraceActivate("SunavalonDryas", "Search Sunavalon Sowing on Link Summon");
                return true;
            }

            // Damage response trigger: gain LP and trigger Jasmine search!
            return true;
        }

        private bool TwinActivate()
        {
            if (_twinReviveUsed || ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            _twinReviveUsed = true;
            AI.SelectCard(CardId.SunseedGeniusLoci);
            DecisionTracer.TraceActivate("SunseedTwin", "Reviving Sunseed Genius Loci from GY");
            return true;
        }

        private bool MeliasActivate()
        {
            if (_meliasReviveUsed || ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            _meliasReviveUsed = true;
            AI.SelectCard(CardId.SunseedGeniusLoci);
            DecisionTracer.TraceActivate("SunavalonMelias", "Melias reviving Sunseed Genius Loci from GY");
            return true;
        }

        private bool SporeActivate()
        {
            if (_sporeUsed || ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.Grave) return false;

            var banishTarget = Bot.Graveyard.FirstOrDefault(c => c != null && PlantMonsters.Contains(c.Id) && c.Id != CardId.Spore);
            if (banishTarget != null)
            {
                _sporeUsed = true;
                AI.SelectCard(banishTarget);
                DecisionTracer.TraceActivate("Spore", $"Banish {banishTarget.Name} from GY to SS Spore");
                return true;
            }
            return false;
        }

        // ============================================================
        // NORMAL SUMMONS
        // ============================================================

        private bool GeniusSummon()
        {
            if (_normalSummonUsed || ShouldSkipCombo()) return false;
            _normalSummonUsed = true;
            DecisionTracer.TraceActivate("GeniusSummon", "Normal Summoning Sunseed Genius Loci (Starter)");
            return true;
        }

        private bool GenericPlantSummon()
        {
            if (_normalSummonUsed || ShouldSkipCombo()) return false;
            _normalSummonUsed = true;
            return true;
        }

        // ============================================================
        // EXTRA DECK SUMMONS
        // ============================================================

        private bool DryasSummon()
        {
            if (ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            return Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.SunseedGeniusLoci);
        }

        private bool MeliasSummon()
        {
            if (ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            int plantCount = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && PlantMonsters.Contains(c.Id));
            bool hasLink = Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.HasType(CardType.Link) && PlantMonsters.Contains(c.Id));
            return plantCount >= 2 && hasLink;
        }

        private bool JasmineSummon()
        {
            if (ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            int plantCount = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && PlantMonsters.Contains(c.Id) && !IsAceCard(c));
            return plantCount >= 2;
        }

        private bool StrennaSummon()
        {
            if (ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            int lv4Count = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 4);
            return lv4Count >= 2;
        }

        private bool TeardropSummon()
        {
            if (ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            int lv8Count = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 8);
            return lv8Count >= 2;
        }

        private bool HyperytonSummon()
        {
            if (ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            int lv9Count = Bot.MonsterZone.Count(c => c != null && c.IsFaceup() && c.Level == 9);
            return lv9Count >= 2;
        }

        private bool DryatrentiaySummon()
        {
            if (ShouldSkipCombo() || IsSpecialSummonBlocked()) return false;
            int linkRating = Bot.MonsterZone.Where(c => c != null && c.IsFaceup() && c.HasType(CardType.Link)).Sum(c => c.LinkCount);
            return linkRating >= 4;
        }

        private bool SPSummon()
        {
            if (IsSpecialSummonBlocked() || ShouldSkipCombo()) return false;
            if (ShouldAvoidGenericExtraDeckSummon(2)) return false;
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup());
        }

        private bool AccesscodeSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return CanDealLethal() || Enemy.GetMonsterCount() >= 2;
        }

        // ============================================================
        // COMBAT & REPOSITION
        // ============================================================

        private bool PlantMonsterRepos()
        {
            if (Card == null || !Card.IsFaceup()) return false;

            // In MP1 or Battle Phase: Switch high ATK monsters to Attack
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Battle))
            {
                if (Card.IsDefense())
                {
                    if (Card.Attack >= 1500 || Enemy.GetMonsterCount() == 0 || !Util.IsAllEnemyBetter(true))
                    {
                        return true;
                    }
                }
            }

            // In MP2: Switch weak monsters to Defense
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main2)
            {
                if (Card.IsAttack() && Util.IsAllEnemyBetter(true) && Card.Attack < 1500)
                {
                    return true;
                }
            }

            return DefaultMonsterRepos();
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            int[] lowStatMonsters = {
                CardId.SunseedGeniusLoci, CardId.SunseedTwin,
                CardId.RikkaPetal, CardId.Spore,
                CardId.MaxxC, CardId.AshBlossom
            };

            if (Duel.Turn == 1 && lowStatMonsters.Contains(cardId) && positions.Contains(CardPosition.FaceUpDefence))
            {
                return CardPosition.FaceUpDefence;
            }

            // Boss monsters & high ATK monsters always summon in Attack Position
            if (positions.Contains(CardPosition.FaceUpAttack))
            {
                int[] highAtkMonsters = {
                    CardId.TeardropTheRikkaQueen, CardId.TherionKingRegulus,
                    CardId.SacredTreeBeastHyperyton, CardId.AccesscodeTalker,
                    CardId.Garura, CardId.TherionLilyBorea,
                    CardId.RikkaMudan, CardId.SnowdropTheRikkaFairy
                };
                if (highAtkMonsters.Contains(cardId))
                {
                    return CardPosition.FaceUpAttack;
                }
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card == null) return null;
            if (card.IsCode(CardId.SunavalonDryas, CardId.RikkaQueenStrenna, CardId.AromaseraphyJasmine, 
                            CardId.RikkaPrincess, CardId.SunseedTwin, CardId.SunavalonMelias, 
                            CardId.TherionKingRegulus, CardId.SacredTreeBeastHyperyton, CardId.TeardropTheRikkaQueen))
                return true;
            return base.OnSelectEffectYn(card, desc);
        }

        private bool IsRikkaMonster(ClientCard c)
        {
            if (c == null) return false;
            return c.Id == CardId.RikkaPetal || c.Id == CardId.RikkaPrincess || c.Id == CardId.RikkaMudan
                || c.Id == CardId.PrimulaTheRikkaFairy || c.Id == CardId.SnowdropTheRikkaFairy
                || c.Id == CardId.RikkaQueenStrenna || c.Id == CardId.TeardropTheRikkaQueen;
        }
    }

    [Deck("Expert_2026_Plant", "2026_Plant")]
    public class ExpertPlantExecutor : _2026_PlantExecutor
    {
        public ExpertPlantExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
        }
    }
}
