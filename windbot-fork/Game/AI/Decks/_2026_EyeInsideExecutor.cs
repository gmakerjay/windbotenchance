using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ==========================================
    // 2026_EyeInside
    // ==========================================
    // ============================================================
    // CARD AUDIT เนโฌโ€ 2026_EyeInside
    // ============================================================
    // Card Name              | Type       | OPT? | Effect Summary                              | Activate When                            | NEVER Activate When                          |
    // -----------------------|------------|------|---------------------------------------------|------------------------------------------|----------------------------------------------|
    // Gazelle King of M      | Monster    | Yes  | SS Berfomet from Deck/GY (Quick)            | Main Phase / opponent effect             | No Berfomet targets in deck/GY              |
    // Cornfield Coatl        | Monster    | Yes  | Negate 1 face-up card (Quick, from hand)    | Opponent has face-up card to negate       | No opponent face-up cards                   |
    // Mirror Swordknight     | Monster    | Yes  | Revive Gazelle/Berfomet (Quick, from GY)    | Chimera/Berfomet in GY                   | No targets in GY                            |
    // Big-Winged Berfomet   | Monster    | Yes  | SS Illusion from GY when used as Fusion mat | Used as Fusion material                  | No Illusion in GY                           |
    // Nightmare Apprentice   | Monster    | Yes  | Discard to SS self + search Illusion        | Main Phase / Illusion targets in deck     | No Illusion in deck / hand empty            |
    // Chimera Fusion         | Spell      | Yes  | Quick Fusion (Beast+Fiend/Illusion)         | Has materials / control Chimera           | No materials / no valid fusion target       |
    // The Hidden Hecahands   | Spell      | Yes  | Search Hecahands monster + optional Set     | Main Phase / targets in deck             | Hand empty / no targets                     |
    // Hecahands Ibtel        | Monster    | Yes  | SS + add Hecahands S/T on NS/SS; GY revive | Summoned / sent to GY                    | No Hecahands S/T in deck                    |
    // Hecahands Yadel        | Monster    | Yes  | SS + add Hecahands monster on NS/SS         | Summoned / sent to GY                    | No Hecahands monster in deck                |
    // Hecahands Makibel      | Monster    | Yes  | SS + add Illusion on NS/SS                  | Summoned                                  | No Illusion in deck                         |
    // Hecahands Gaigas       | Monster    | Yes  | Excavate to SS or add Hecahand from hand    | Main Phase                                | No Hecahands targets                        |
    // Hecahands Breus        | Monster    | Yes  | Look opponent hand + add Hecahand           | Main Phase                                | No Hecahands targets                        |
    // Hecahands Godos        | Monster    | Yes  | Add Hecahands from Deck/GY when SS'd        | Summoned                                   | No Hecahands in Deck/GY                     |
    // ============================================================
    // ACE CARDS:
    //   Primary  : Chimera the Illusion Beast เนโฌโ€ multi-attack + ATK reset boss
    //   Secondary: Guardian Chimera เนโฌโ€ draw + destroy on fusion
    //   Tertiary : Hecahands Jauzah เนโฌโ€ Hecahands search engine + non-destruction battle
    //   Quatern  : Hecahands Xeno เนโฌโ€ banish disruption + GY revival
    //   Link     : S:P Little Knight เนโฌโ€ banish disruption
    // ============================================================
    // STRATEGY:
    //   Turn 1: Chimera Fusion เนยโ€ Chimera King/Berfomet + Gazelle search loop
    //   Turn 2: Break board with Hecahands Xeno banish + Illusion Beast multi-attack
    //   Engine: Hecahands package for extender plays + recovery
    // ============================================================
    [Deck("2026_EyeInside", "2026_EyeInside")]
    public class _2026_EyeInsideExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters เนโฌโ€ Chimera/Illusion core
            public const int Gazelle = 23076639;
            public const int CornfieldCoatl = 92565383;
            public const int MirrorSwordknight = 28954097;
            public const int BigWingedBerfomet = 55461744;
            public const int NightmareApprentice = 58143852;

            // Hecahands monsters
            public const int HecahandsIbtel = 95365081;
            public const int HecahandsYadel = 32759190;
            public const int HecahandsMakibel = 18321034;
            public const int HecahandsGodos = 68144894;
            public const int HecahandsGaigas = 95132593;
            public const int HecahandsBreus = 21637502;

            // Spells & Traps
            public const int TheHiddenHecahands = 20415050;
            public const int SuperPoly = 48130397;
            public const int ChimeraFusion = 63136489;
            public const int IbalHecahands = 93294363;
            public const int YadalHecahands = 29792472;
            public const int BaytalHecahands = 43932352;

            // Hand Traps & Techs
            public const int MaxxC = 23434538;
            public const int AshBlossom = 14558127;
            public const int CalledByTheGrave = 24224830;
            public const int InfiniteImpermanence = 10045474;

            // Extra Deck
            public const int Mudragon = 54757758;
            public const int Garura = 11765832;
            public const int ChimeraKingOfPhantomBeasts = 1769875;
            public const int BerfometMythicalKing = 69601012;
            public const int EarthGolemIgnister = 62111090;
            public const int HecahandsDandalos = 31411835;
            public const int ChimeraIllusionBeast = 38264974;
            public const int MagnumTheReliever = 43227;
            public const int HecahandsJauzah = 67021206;
            public const int GuardianChimera = 11321089;
            public const int HecahandsXeno = 94410955;
            public const int SPLittleKnight = 29301450;
        }

        // ===== OPT tracking =====
        private bool _coatlUsed = false;
        private bool _swordknightUsed = false;
        private bool _hiddenHecahandsUsed = false;
        private bool _jauzahSearchUsed = false;
        private bool _nightmareSSUsed = false;
        private bool _ibtelSearchUsed = false;
        private bool _yadelSearchUsed = false;
        private bool _makibelSearchUsed = false;
        private bool _godosSearchUsed = false;

        private static readonly int[] AceCardIds = {
            CardId.HecahandsJauzah,
            CardId.HecahandsXeno,
            CardId.ChimeraIllusionBeast,
            CardId.GuardianChimera,
            CardId.SPLittleKnight,
            CardId.BerfometMythicalKing,
            CardId.ChimeraKingOfPhantomBeasts
        };

        // FieldGuard inherited: IsSpecialSummonBlocked, CanDealLethal, CanOTK,
        // ShouldSkipCombo, NeedsBoardPresence, IsInGrindGame, EnemyHasKnownNegate เนยโ€ inherited

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.ChimeraIllusionBeast) && Bot.GetMonsterCount() >= 2)
                return true;
            if (Bot.HasInMonstersZone(CardId.GuardianChimera))
                return true;
            if (Bot.HasInMonstersZone(CardId.SPLittleKnight) && Bot.GetSpellCount() >= 1)
                return true;
            if (Bot.HasInMonstersZone(CardId.ChimeraKingOfPhantomBeasts) && Bot.HasInMonstersZone(CardId.BerfometMythicalKing))
                return true;
            if (Bot.HasInMonstersZone(CardId.HecahandsXeno) && Bot.GetSpellCount() >= 1)
                return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough())
                return base.ShouldStopExtending();
            return false;
        }

        private bool OpponentHasThreateningMonster()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 3000 || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
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
            if (c.IsCode(CardId.AshBlossom, CardId.MaxxC))
                return 800;
            if (c.IsCode(CardId.Gazelle, CardId.MirrorSwordknight, CardId.BigWingedBerfomet, CardId.NightmareApprentice))
                return 100;
            return 200;
        }

        // ===== SelectPreferred helper (Skill v2 เธขเธ7) =====
        private IList<ClientCard> SelectPreferred(
            IList<ClientCard> cards, int min, int max, params int[] preferredIds)
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

        // ===== Helper: check if we control any Hecahand monster =====
        private bool IsHecahandCard(int id)
        {
            return id == CardId.HecahandsIbtel || id == CardId.HecahandsYadel || id == CardId.HecahandsMakibel ||
                   id == CardId.HecahandsGodos || id == CardId.HecahandsGaigas || id == CardId.HecahandsBreus ||
                   id == CardId.HecahandsDandalos || id == CardId.HecahandsJauzah || id == CardId.HecahandsXeno;
        }

        private bool IsIllusionCard(ClientCard card)
            => card != null && card.HasRace((CardRace)0x2000000); // Illusion race

        private bool HasIllusionInDeck()
        {
            return Bot.GetRemainingCount(CardId.Gazelle, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.MirrorSwordknight, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.NightmareApprentice, 3) > 0 ||
                   Bot.GetRemainingCount(CardId.CornfieldCoatl, 3) > 0;
        }

        public _2026_EyeInsideExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // Register Ace Cards to protect them from being used suboptimally
            ResourcePlan.RegisterAceCards(AceCardIds);

            // เนโ€โฌเนโ€โฌ Combo Router: Sequencing เนโ€โฌเนโ€โฌ
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.Gazelle, CardId.MirrorSwordknight },
                FallbackLineName = "Hecahands-Fallback",
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.Gazelle, ActionType = ExecutorType.Activate, Description = "Play CardId.Gazelle" },
                    new() { CardId = CardId.MirrorSwordknight, ActionType = ExecutorType.Activate, Description = "Extend with CardId.MirrorSwordknight" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Hecahands-Fallback",
                RequiredCards = new List<int> { CardId.TheHiddenHecahands },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.TheHiddenHecahands, ActionType = ExecutorType.Activate, Description = "Activate The Hidden Hecahands" }
                },
                EndBoardScore = 60,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.TheHiddenHecahands)
            });

            // เนโ€โฌเนโ€โฌ Bait Planner เนโ€โฌเนโ€โฌ
            BaitPlanner.RegisterComboStarters(CardId.Gazelle, CardId.CornfieldCoatl);
            BaitPlanner.RegisterBaitCards(CardId.CornfieldCoatl);

            // เนโ€โฌเนโ€โฌ Chain Advisor เนโ€โฌเนโ€โฌ
            ChainAdvisor.RegisterHighValueTargets(CardId.Gazelle, CardId.CornfieldCoatl, CardId.InfiniteImpermanence, CardId.SPLittleKnight);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 1: Hand Traps & Quick Disruption เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, () => SmartHandTrapChain() && DefaultMaxxC());
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshBlossomEffect);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, CalledByTheGraveEffect);
            AddExecutor(ExecutorType.Activate, CardId.CornfieldCoatl, CornfieldCoatlEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 2: Quick Effects เนโฌโ€ opponent-turn disruption เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);
            AddExecutor(ExecutorType.Activate, CardId.IbalHecahands, IbalHecahandsEffect);
            AddExecutor(ExecutorType.Activate, CardId.YadalHecahands, YadalHecahandsEffect);
            AddExecutor(ExecutorType.Activate, CardId.MirrorSwordknight, MirrorSwordknightEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 3: Fusion/SS Spells เนโฌโ€ priority over search เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.Activate, CardId.ChimeraFusion, ChimeraFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPoly, SuperPolyEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 4: Search Spells & Setup เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.Activate, CardId.TheHiddenHecahands, TheHiddenHecahandsEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 5: Normal / Special Summons เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.Summon, CardId.MirrorSwordknight, MirrorSwordknightSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.MirrorSwordknight, MirrorSwordknightSummon);
            AddExecutor(ExecutorType.Summon, CardId.Gazelle, GazelleSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Gazelle, GazelleSummon);
            AddExecutor(ExecutorType.Summon, CardId.HecahandsIbtel);
            AddExecutor(ExecutorType.Summon, CardId.HecahandsYadel);
            AddExecutor(ExecutorType.Summon, CardId.HecahandsMakibel);
            AddExecutor(ExecutorType.Summon, CardId.HecahandsGodos);
            AddExecutor(ExecutorType.SpSummon, CardId.NightmareApprentice, NightmareApprenticeSummon);
            AddExecutor(ExecutorType.Activate, CardId.NightmareApprentice, NightmareApprenticeEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 6: Hecahands hand effects / NS/SS triggers / GY effects เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.Activate, CardId.HecahandsIbtel, HecahandsIbtelEffect);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsYadel, HecahandsYadelEffect);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsMakibel, HecahandsMakibelEffect);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsGodos, HecahandsGodosEffect);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsGaigas, HecahandsGaigasEffect);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsBreus, HecahandsBreusEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 7: Gazelle / Berfomet on-summon searches เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.Activate, CardId.Gazelle, GazelleOnSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.BigWingedBerfomet, BerfometOnSummonEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 8: Extra Deck Boss Summons เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.SpSummon, CardId.ChimeraKingOfPhantomBeasts);
            AddExecutor(ExecutorType.Activate, CardId.ChimeraKingOfPhantomBeasts, ChimeraKingEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.BerfometMythicalKing);
            AddExecutor(ExecutorType.Activate, CardId.BerfometMythicalKing, BerfometKingEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.HecahandsJauzah);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsJauzah, HecahandsJauzahEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.HecahandsXeno);
            AddExecutor(ExecutorType.Activate, CardId.HecahandsXeno, HecahandsXenoEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.ChimeraIllusionBeast);
            AddExecutor(ExecutorType.Activate, CardId.ChimeraIllusionBeast, ChimeraIllusionBeastEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.GuardianChimera);
            AddExecutor(ExecutorType.Activate, CardId.GuardianChimera, GuardianChimeraEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 9: Link plays & control เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightEffect);
            AddExecutor(ExecutorType.Activate, CardId.BaytalHecahands, BaytalHecahandsEffect);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 10: Fallback Normal Summon เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.Summon, FallbackNormalSummon);

            // เนโ€ขยเนโ€ขยเนโ€ขย TIER 11: Spell/Trap Sets & Repos (last) เนโ€ขยเนโ€ขยเนโ€ขย
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.ChimeraFusion, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPoly, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.IbalHecahands, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.YadalHecahands, SetTrapCondition);
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Chimera/Illusion Fusion control เนโฌโ€ prefer going first to set up Chimera + disruption
            return true;
        }

        private bool OpponentHasActiveNegator()
        {
            return Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && !c.IsDisabled() &&
                (c.IsFloodgate() || c.Attack >= 2500 || c.HasType(CardType.Link) || c.HasType(CardType.Synchro)));
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            // Link summons (S:P Little Knight)
            if (card.HasType(CardType.Link))
            {
                int reqRating = card.LinkCount;
                if (!IsMaterialSelectionSafeForLink(reqRating))
                {
                    var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
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
                        DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning Link {card.Name} is not safe (would consume Ace card(s) as Link material)");
                        return false;
                    }
                }
            }

            // Fusion summons (Guardian Chimera, Chimera Illusion Beast)
            if (card.HasType(CardType.Fusion))
            {
                // Protect our face-up Ace monsters on the field from being used as fusion materials
                var faceupAcesOnField = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
                if (faceupAcesOnField.Count > 0)
                {
                    // Check if we have other materials in hand or non-Ace field monsters
                    var nonAceMats = Bot.Hand.Concat(Bot.GetMonsters().Where(c => !IsAceCard(c))).ToList();
                    if (nonAceMats.Count < 2) // Typically fusions need at least 2 materials
                    {
                        bool allowed = false;
                        foreach (var mat in faceupAcesOnField)
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
                            DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning Fusion {card.Name} is not safe (would consume Ace card(s) as material)");
                            return false;
                        }
                    }
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

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _coatlUsed = false;
            _swordknightUsed = false;
            _hiddenHecahandsUsed = false;
            _jauzahSearchUsed = false;
            _nightmareSSUsed = false;
            _ibtelSearchUsed = false;
            _yadelSearchUsed = false;
            _makibelSearchUsed = false;
            _godosSearchUsed = false;
            _gaigasHandUsed = false;
            _breusHandUsed = false;
            _gaigasZoneUsed = false;
            _breusZoneUsed = false;

            // เนโ€โฌเนโ€โฌ Going-Second BreakBoard: prioritize disruption over combo เนโ€โฌเนโ€โฌ
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        // =================================================================
        // OnSelectCard เนโฌโ€ Skill v2 เธขเธ1: Hint codes FIRST, then Card context
        // =================================================================
        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // ===== HINT-BASED SELECTIONS (Card may be null here!) =====

            // hint 533 = HINTMSG_LMATERIAL เนโฌโ€ Link Material: protect Ace Cards
            if (hint == 533)
            {
                var safe = cards.Where(c => c != null && !IsAceCard(c)).ToList();
                if (safe.Count >= min) return safe.Take(max).ToList();
            }

            // hint 502 = HINTMSG_DESTROY เนโฌโ€ destroy target selection
            if (hint == 502)
            {
                // Prefer non-ace, non-key cards as destroy targets
                var nonAce = cards.Where(c => c != null && !IsAceCard(c))
                    .OrderByDescending(c => c.Attack).ToList();
                if (nonAce.Count >= min) return nonAce.Take(max).ToList();
            }

            // hint 509 = HINTMSG_SPSUMMON เนโฌโ€ Special Summon target selection
            if (hint == 509)
            {
                // Prioritize Hecahand/Illusion revival targets
                var priority = cards.Where(c => c != null && (IsHecahandCard(c.Id) || IsIllusionCard(c)) && c.IsCanRevive())
                    .OrderByDescending(c => c.Attack).ToList();
                if (priority.Count >= min) return priority.Take(max).ToList();
            }

            // hint 549 = HINTMSG_ATTACKTARGET เนโฌโ€ Battle Phase target (Card context may be wrong!)
            if (hint == 549)
            {
                // Prefer targets our monsters can actually beat เนโฌโ€ avoid suicide attacks
                int ourBestAtk = Bot.GetMonsters()
                    .Where(c => c != null && c.IsFaceup() && c.IsAttack())
                    .Select(c => c.Attack)
                    .DefaultIfEmpty(0).Max();
                var beatable = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone
                    && (c.IsAttack() ? c.Attack < ourBestAtk : c.Defense < ourBestAtk)).ToList();
                if (beatable.Count >= min) return beatable.OrderByDescending(c => c.Attack).Take(max).ToList();
                // Fallback: any valid target (engine may skip battle if unsafe)
                var validTargets = cards.Where(c => c != null && c.IsFaceup() && c.Location == CardLocation.MonsterZone).ToList();
                if (validTargets.Count >= min) return validTargets.Take(max).ToList();
            }

            // ===== CARD-CONTEXT SELECTIONS (safe to check Card now) =====
            if (Card == null)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Ace card protection: avoid using face-up Ace cards as materials/tributes
            if (cards.Any(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone && IsAceCard(c)))
            {
                var safeCards = cards.Where(c => c == null || c.Controller != 0 || c.Location != CardLocation.MonsterZone || !IsAceCard(c)).ToList();
                if (safeCards.Count >= min)
                    return safeCards.Take(max).ToList();
            }

            // Chimera King GY revival เนโฌโ€ prefer Beast/Fiend/Illusion
            if (Card.Id == CardId.ChimeraKingOfPhantomBeasts)
            {
                var revivable = cards.FirstOrDefault(c => c != null && c.IsCanRevive() &&
                    (c.HasRace(CardRace.Beast) || c.HasRace(CardRace.Fiend) || c.HasRace((CardRace)0x2000000)));
                if (revivable != null) return new List<ClientCard> { revivable };
            }

            // Berfomet Mythical King effects
            if (Card.Id == CardId.BerfometMythicalKing)
            {
                if (cards.Any(c => c != null && c.Location == CardLocation.Deck))
                {
                    // On Fusion Summon: send 1 Beast/Fiend/Illusion from Deck to GY (Foolish Burial)
                    // Smart selection: send Gazelle if we don't have it in GY, else MirrorSwordknight
                    if (Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.Gazelle)))
                        return SelectPreferred(cards, min, max,
                            CardId.MirrorSwordknight, CardId.NightmareApprentice,
                            CardId.CornfieldCoatl, CardId.Gazelle);
                    return SelectPreferred(cards, min, max,
                        CardId.Gazelle, CardId.MirrorSwordknight,
                        CardId.NightmareApprentice, CardId.CornfieldCoatl);
                }
                
                // Banish itself from GY to revive a banished monster
                var revivable = cards.FirstOrDefault(c => c != null &&
                    (c.HasRace(CardRace.Beast) || c.HasRace(CardRace.Fiend) || c.HasRace((CardRace)0x2000000)));
                if (revivable != null) return new List<ClientCard> { revivable };
            }

            // Hecahands Ibtel GY revival
            if (Card.Id == CardId.HecahandsIbtel)
            {
                var revivable = cards.FirstOrDefault(c => c != null && c.IsCanRevive() && c.Id != CardId.HecahandsIbtel && c.IsMonster());
                if (revivable != null) return new List<ClientCard> { revivable };
            }

            // Nightmare Apprentice selection เนโฌโ€ hand discard vs summon search
            if (Card.Id == CardId.NightmareApprentice)
            {
                if (Card.Location == CardLocation.Hand)
                {
                    return SelectPreferred(cards, min, max,
                        CardId.HecahandsIbtel, CardId.HecahandsYadel, CardId.HecahandsMakibel,
                        CardId.Gazelle, CardId.CornfieldCoatl);
                }
                else
                {
                    return SelectPreferred(cards, min, max,
                        CardId.MirrorSwordknight, CardId.CornfieldCoatl, CardId.NightmareApprentice);
                }
            }

            // Hecahands Jauzah search เนโฌโ€ prioritize extenders
            if (Card.Id == CardId.HecahandsJauzah)
            {
                return SelectPreferred(cards, min, max,
                    CardId.TheHiddenHecahands, CardId.HecahandsIbtel, CardId.HecahandsYadel);
            }

            // Hecahands Ibtel on-summon search
            if (Card.Id == CardId.HecahandsIbtel)
            {
                return SelectPreferred(cards, min, max,
                    CardId.TheHiddenHecahands, CardId.IbalHecahands, CardId.YadalHecahands);
            }

            // Hecahands Yadel on-summon search
            if (Card.Id == CardId.HecahandsYadel)
            {
                return SelectPreferred(cards, min, max,
                    CardId.HecahandsIbtel, CardId.HecahandsMakibel, CardId.HecahandsGodos);
            }

            // Hecahands Makibel on-summon search
            if (Card.Id == CardId.HecahandsMakibel)
            {
                return SelectPreferred(cards, min, max,
                    CardId.NightmareApprentice, CardId.Gazelle, CardId.MirrorSwordknight);
            }

            // Hecahands Godos on-summon search
            if (Card.Id == CardId.HecahandsGodos)
            {
                return SelectPreferred(cards, min, max,
                    CardId.HecahandsIbtel, CardId.HecahandsYadel, CardId.HecahandsMakibel);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }



        // ===== Fusion Material: prefer hand > expendable field (Skill v2 เธขเธ8) =====
        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            // Deck Fusion (Chimera Fusion with GY material if we control Chimera)
            bool isDeckFusion = cards.Any(c => c != null && c.Location == CardLocation.Deck);
            if (isDeckFusion)
            {
                var deckHand = cards.Where(c => c != null
                    && (c.Location == CardLocation.Deck || c.Location == CardLocation.Hand)
                    && !IsAceCard(c)).ToList();
                if (deckHand.Count >= min) return deckHand.Take(max).ToList();
            }

            // Super Polymerization: use opponent monsters first
            if (Card != null && Card.Id == CardId.SuperPoly)
            {
                var oppMonsters = cards.Where(c => c != null
                    && c.Controller == 1 && c.Location == CardLocation.MonsterZone).ToList();
                if (oppMonsters.Count >= min) return oppMonsters.Take(max).ToList();
            }

            // Regular Fusion: hand materials > field expendable > field non-ace
            var result = new List<ClientCard>();
            var handMats = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
            var safeField = cards.Where(c => c != null
                && c.Location == CardLocation.MonsterZone && !IsAceCard(c)).ToList();
            foreach (var c in handMats) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max)
                foreach (var c in safeField) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count >= min) return result;

            return base.OnSelectFusionMaterial(cards, min, max);
        }

        // ===== Battle Safety Helpers =====


        private bool SetTrapCondition() => Util.IsTurn1OrMain2();

        private bool IsSafeToAttack(ClientCard attacker)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (enemy.IsAttack() && enemy.Attack >= attacker.Attack) return false;
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

        private bool MonsterRepos()
        {
            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster == null || IsAceCard(monster)) continue;
                bool enemyEmpty = Enemy.GetMonsterCount() == 0;
                if (monster.IsAttack())
                {
                    if (!enemyEmpty && !IsSafeToAttack(monster) && IsSafeToDefend(monster))
                        return true;
                }
                else
                {
                    if (enemyEmpty || IsSafeToAttack(monster))
                        return true;
                }
            }
            return false;
        }

        // =================================================================
        // TIER 1 เนโฌโ€ Hand Traps
        // =================================================================
        private bool AshBlossomEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            if (Bot.GetMonsterCount() == 0 && Bot.GetHandCount() <= 2 && Duel.Player == 0)
                return true;
            return DefaultAshBlossomAndJoyousSpring();
        }

        private bool CalledByTheGraveEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return DefaultCalledByTheGrave();
        }

        // Cornfield Coatl: Quick Effect discard to negate face-up card
        private bool CornfieldCoatlEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_coatlUsed) return false;
                // Only activate during opponent's turn or when we need to negate
                if (Duel.Player == 0 && !ShouldSkipCombo()) return false;
                var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled());
                if (target != null)
                {
                    AI.SelectCard(target);
                    _coatlUsed = true;
                    return true;
                }
            }
            return false;
        }

        // =================================================================
        // TIER 2 เนโฌโ€ Quick Effects (opponent-turn disruption)
        // =================================================================
        private ClientCard GetPreemptiveImpermTarget()
        {
            int[] threatIds = {
                21522601, // Witchcrafter Madame Verre
                84523092, // Witchcrafter Haine
                1561110,  // ABC-Dragon Buster
                4280258,  // Apollousa, Bow of the Goddess
                10443957, // Cyber Dragon Infinity
                84815190, // Baronne de Fleur
                1508649   // Altergeist Hexstia
            };

            return Enemy.MonsterZone.GetMonsters().FirstOrDefault(c => 
                c != null && c.IsFaceup() && !c.IsDisabled() && 
                threatIds.Contains(c.Id) && 
                !c.IsShouldNotBeTarget() && !c.IsShouldNotBeSpellTrapTarget());
        }

        private bool InfiniteImpermanenceEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = GetPreemptiveImpermTarget();
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return DefaultInfiniteImpermanence();
        }

        private bool IbalHecahandsEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller != 1 || !LastChainCard.IsMonster()) return false;
            // Don't waste if we already have a strong board
            if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
            bool hasHeca = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsHecahandCard(c.Id));
            return hasHeca;
        }

        private bool YadalHecahandsEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller != 1 || LastChainCard.IsMonster()) return false;
            // Don't waste if we already have a strong board
            if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
            bool hasHeca = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsHecahandCard(c.Id));
            return hasHeca;
        }

        // Mirror Swordknight: Quick Effect tribute to SS Berfomet from Deck/GY
        private bool MirrorSwordknightEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            if (_swordknightUsed) return false;
            bool hasBerfomet = Bot.GetRemainingCount(CardId.BigWingedBerfomet, 2) > 0;
            bool hasGazelle = Bot.GetRemainingCount(CardId.Gazelle, 3) > 0;
            if (!hasBerfomet && !hasGazelle) return false;
            // During opponent's turn = disruption; during our turn = only when needed for combo
            if (Duel.Player == 0 && IsBoardStrongEnough() && !ShouldSkipCombo()) return false;
            AI.SelectCard(CardId.BigWingedBerfomet);
            _swordknightUsed = true;
            return true;
        }

        // =================================================================
        // TIER 3 เนโฌโ€ Fusion/SS Spells (priority over search)
        // =================================================================
        private bool ChimeraFusionEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location == CardLocation.Hand || Card.IsFacedown())
            {
                if (ShouldSkipCombo()) return false;
                // Don't activate if board is already strong and we don't have a threat to deal with
                if (IsBoardStrongEnough() && !OpponentHasThreateningMonster()) return false;
                // Fusion Summon: need Beast + Fiend (or Illusion)
                bool hasBeast = Bot.GetMonsters().Concat(Bot.Hand).Any(c => c != null && c.HasRace(CardRace.Beast));
                bool hasFiendOrIllusion = Bot.GetMonsters().Concat(Bot.Hand).Any(c => c != null
                    && (c.HasRace(CardRace.Fiend) || c.HasRace((CardRace)0x2000000)));
                if (hasBeast && hasFiendOrIllusion)
                {
                    // Smart fusion target selection:
                    // OTK turn เนยโ€ Chimera Illusion Beast (multi-attack)
                    // Opponent has threatening monster เนยโ€ Guardian Chimera (destroy)
                    // Need negate เนยโ€ Chimera King of Phantom Beasts
                    // Default เนยโ€ Chimera Illusion Beast
                    if (CanDealLethal() && GetRemainingCount(CardId.ChimeraIllusionBeast) > 0)
                        AI.SelectCard(CardId.ChimeraIllusionBeast);
                    else if (OpponentHasThreateningMonster() && GetRemainingCount(CardId.GuardianChimera) > 0)
                        AI.SelectCard(CardId.GuardianChimera, CardId.ChimeraIllusionBeast);
                    else if (Bot.GetRemainingCount(CardId.BigWingedBerfomet, 2) > 0 && GetRemainingCount(CardId.BerfometMythicalKing) > 0)
                        AI.SelectCard(CardId.BerfometMythicalKing, CardId.ChimeraIllusionBeast);
                    else
                        AI.SelectCard(
                            CardId.ChimeraIllusionBeast,
                            CardId.GuardianChimera,
                            CardId.BerfometMythicalKing,
                            CardId.ChimeraKingOfPhantomBeasts
                        );
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // GY effect: if Fusion Monster destroyed, banish to revive Gazelle/Berfomet
                bool hasChimera = Bot.GetMonsters().Any(c => c != null && c.IsCode(CardId.ChimeraKingOfPhantomBeasts)) ||
                                   Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.ChimeraKingOfPhantomBeasts));
                if (hasChimera && Bot.GetMonsterCount() < 5)
                {
                    AI.SelectCard(CardId.Gazelle);
                    return true;
                }
            }
            return false;
        }

        private bool SuperPolyEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Enemy.GetMonsterCount() >= 2)
            {
                AI.SelectCard(CardId.Garura, CardId.Mudragon);
                return true;
            }
            return false;
        }

        // =================================================================
        // TIER 4 เนโฌโ€ Search Spells
        // =================================================================
        private bool TheHiddenHecahandsEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_hiddenHecahandsUsed) return false;
            if (Bot.GetRemainingCount(CardId.HecahandsIbtel, 3) == 0 &&
                Bot.GetRemainingCount(CardId.HecahandsYadel, 3) == 0 &&
                Bot.GetRemainingCount(CardId.HecahandsMakibel, 1) == 0 &&
                Bot.GetRemainingCount(CardId.HecahandsGodos, 2) == 0 &&
                Bot.GetRemainingCount(CardId.HecahandsGaigas, 1) == 0 &&
                Bot.GetRemainingCount(CardId.HecahandsBreus, 1) == 0) return false;
            // Prioritize Ibtel (best extender) > Yadel > Makibel
            int searchId = CardId.HecahandsIbtel;
            if (Bot.GetRemainingCount(CardId.HecahandsIbtel, 3) == 0)
            {
                searchId = CardId.HecahandsYadel;
                if (Bot.GetRemainingCount(CardId.HecahandsYadel, 3) == 0)
                    searchId = CardId.HecahandsMakibel;
            }
            AI.SelectCard(searchId);
            _hiddenHecahandsUsed = true;
            return true;
        }

        // =================================================================
        // TIER 5 เนโฌโ€ Normal / Special Summons
        // =================================================================
        private bool MirrorSwordknightSummon()
        {
            if (ShouldSkipCombo()) return false;
            // Don't summon if board is already strong and we have no Gazelle in GY to revive
            if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
            return true;
        }

        private bool GazelleSummon()
        {
            if (ShouldSkipCombo()) return false;
            // Don't waste normal summon if we already have Chimera Fusion in hand
            if (Bot.HasInHand(CardId.ChimeraFusion) && IsBoardStrongEnough()) return false;
            // Don't summon if we already have a monster and DarkTime effect is not active
            if (Bot.GetMonsterCount() >= 2 && !NeedsBoardPresence()) return false;
            return true;
        }

        // Nightmare Apprentice: discard 1 to SS + search Illusion
        private bool NightmareApprenticeSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (_nightmareSSUsed) return false;
            if (Bot.Hand.Count < 2) return false; // need at least 1 card to discard + itself
            if (!HasIllusionInDeck()) return false;
            _nightmareSSUsed = true;
            return true;
        }

        // =================================================================
        // TIER 6 เนโฌโ€ Hecahands hand effects / NS/SS triggers / GY effects
        // =================================================================
        private bool HecahandsIbtelEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (ShouldSkipCombo()) return false;
                if (IsBoardStrongEnough() && !NeedsBoardPresence()) return false;
                bool hasOtherHeca = Bot.Hand.Any(c => c != null && c != Card && IsHecahandCard(c.Id));
                if (!hasOtherHeca) return false;
                // Prefer Yadel (searches Hecahands monsters เนโฌโ€ more value) over Makibel
                AI.SelectCard(CardId.HecahandsYadel, CardId.HecahandsMakibel);
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                // On-summon: search Hecahands S/T
                if (_ibtelSearchUsed) return false;
                if (Bot.GetRemainingCount(CardId.TheHiddenHecahands, 3) == 0 &&
                    Bot.GetRemainingCount(CardId.IbalHecahands, 3) == 0 &&
                    Bot.GetRemainingCount(CardId.YadalHecahands, 3) == 0) return false;
                // The Hidden Hecahands is best เนโฌโ€ searches any Hecahands monster
                if (Bot.GetRemainingCount(CardId.TheHiddenHecahands, 3) > 0)
                    AI.SelectCard(CardId.TheHiddenHecahands);
                else if (Bot.GetRemainingCount(CardId.IbalHecahands, 3) > 0)
                    AI.SelectCard(CardId.IbalHecahands);
                else
                    AI.SelectCard(CardId.YadalHecahands);
                _ibtelSearchUsed = true;
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                // GY revive: only if we need board presence
                if (!NeedsBoardPresence()) return false;
                var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCanRevive() && c.Id != CardId.HecahandsIbtel && c.IsMonster());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool HecahandsYadelEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (ShouldSkipCombo()) return false;
                // Only activate from hand if we need the search
                if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
                if (Bot.GetRemainingCount(CardId.TheHiddenHecahands, 3) == 0) return false;
                AI.SelectCard(CardId.TheHiddenHecahands);
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                // On-summon: search Hecahands monster
                if (_yadelSearchUsed) return false;
                if (Bot.GetRemainingCount(CardId.HecahandsIbtel, 3) == 0 &&
                    Bot.GetRemainingCount(CardId.HecahandsMakibel, 1) == 0) return false;
                int searchId = CardId.HecahandsIbtel;
                if (Bot.GetRemainingCount(CardId.HecahandsIbtel, 3) == 0) searchId = CardId.HecahandsMakibel;
                AI.SelectCard(searchId);
                _yadelSearchUsed = true;
                return true;
            }
            return false;
        }

        private bool HecahandsMakibelEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (ShouldSkipCombo()) return false;
                if (IsSpecialSummonBlocked()) return false;
                // Don't activate if board is already strong
                if (IsBoardStrongEnough() && !NeedsBoardPresence()) return false;
                // Smart fusion target selection:
                bool oppHasThreat = OpponentHasThreateningMonster();
                bool needBody = NeedsBoardPresence();
                if (oppHasThreat && GetRemainingCount(CardId.HecahandsXeno) > 0)
                    AI.SelectCard(CardId.HecahandsXeno, CardId.HecahandsJauzah, CardId.HecahandsDandalos);
                else if (needBody && GetRemainingCount(CardId.HecahandsJauzah) > 0)
                    AI.SelectCard(CardId.HecahandsJauzah, CardId.HecahandsDandalos, CardId.HecahandsXeno);
                else
                    AI.SelectCard(CardId.HecahandsJauzah, CardId.HecahandsXeno, CardId.HecahandsDandalos);
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                // On-summon: search Illusion monster
                if (_makibelSearchUsed) return false;
                if (!HasIllusionInDeck()) return false;
                AI.SelectCard(CardId.NightmareApprentice, CardId.Gazelle);
                _makibelSearchUsed = true;
                return true;
            }
            return false;
        }

        private bool HecahandsGodosEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (ShouldSkipCombo()) return false;
                // Don't waste Godos from hand if we already have a strong board
                if (IsBoardStrongEnough() && !NeedsBoardPresence()) return false;
                return true;
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                // On-summon: search Hecahands monster from Deck/GY
                if (_godosSearchUsed) return false;
                if (Bot.GetRemainingCount(CardId.HecahandsIbtel, 3) == 0 &&
                    Bot.GetRemainingCount(CardId.HecahandsYadel, 3) == 0) return false;
                int searchId = CardId.HecahandsIbtel;
                if (Bot.GetRemainingCount(CardId.HecahandsIbtel, 3) == 0) searchId = CardId.HecahandsYadel;
                AI.SelectCard(searchId);
                _godosSearchUsed = true;
                return true;
            }
            return false;
        }

        // OPT flags for Hecahands hand effects
        private bool _gaigasHandUsed = false;
        private bool _breusHandUsed = false;
        private bool _gaigasZoneUsed = false;
        private bool _breusZoneUsed = false;

        private bool HecahandsGaigasEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_gaigasHandUsed) return false;
                if (EnemyHasKnownNegate()) return false;
                if (Duel.Player == 1)
                {
                    // Opponent's turn: activate only if we need body or they're making plays
                    if (NeedsBoardPresence() || LastChainCard != null)
                    {
                        _gaigasHandUsed = true;
                        return true;
                    }
                    return false;
                }
                if (Bot.GetMonsterCount() == 0 && !ShouldSkipCombo())
                {
                    _gaigasHandUsed = true;
                    return true;
                }
                return false;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_gaigasZoneUsed) return false;
                // Only excavate if we need resources
                if (Bot.GetHandCount() <= 1 || Bot.GetMonsterCount() <= 1)
                {
                    _gaigasZoneUsed = true;
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool HecahandsBreusEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_breusHandUsed) return false;
                if (EnemyHasKnownNegate()) return false;
                if (Duel.Player == 1)
                {
                    // Opponent's turn: look at hand if we suspect threats
                    if (NeedsBoardPresence() || OpponentHasThreateningMonster())
                    {
                        _breusHandUsed = true;
                        return true;
                    }
                    return false;
                }
                if (Bot.GetMonsterCount() == 0 && !ShouldSkipCombo())
                {
                    _breusHandUsed = true;
                    return true;
                }
                return false;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_breusZoneUsed) return false;
                // Only look at opponent's hand if we need info or they have threatening monsters
                if (Bot.GetHandCount() <= 1 || OpponentHasThreateningMonster())
                {
                    _breusZoneUsed = true;
                    return true;
                }
                return false;
            }
            return false;
        }

        // =================================================================
        // TIER 7 เนโฌโ€ Gazelle / Berfomet on-summon searches
        // =================================================================
        private bool GazelleOnSummonEffect()
        {
            AI.SelectCard(CardId.ChimeraFusion);
            return true;
        }

        private bool BerfometOnSummonEffect()
        {
            // Big-Winged Berfomet: search Level 4 Beast + Chimera Fusion
            AI.SelectCard(CardId.ChimeraFusion);
            return true;
        }

        // =================================================================
        // TIER 8 เนโฌโ€ Extra Deck Boss Effects
        // =================================================================
        private bool ChimeraKingEffect()
        {
            // When sent to GY as Fusion mat: SS Berfomet from Deck/GY
            var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCanRevive() &&
                (c.HasRace(CardRace.Beast) || c.HasRace(CardRace.Fiend) || c.HasRace((CardRace)0x2000000)));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

                private bool BerfometKingEffect()
                {
                    if (Card.Location == CardLocation.MonsterZone && Card.IsDisabled()) return false;
                    return true;
                }

        // Hecahands Jauzah: search 1 Hecahands card from Deck/GY (NOT tribute+destroy)
        private bool HecahandsJauzahEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (_jauzahSearchUsed) return false;
                // Don't search if we already have a good hand
                if (IsBoardStrongEnough() && !IsInGrindGame()) return false;
                if (Bot.GetRemainingCount(CardId.TheHiddenHecahands, 3) == 0 &&
                    Bot.GetRemainingCount(CardId.HecahandsIbtel, 3) == 0 &&
                    Bot.GetRemainingCount(CardId.HecahandsYadel, 3) == 0) return false;
                AI.SelectCard(CardId.TheHiddenHecahands, CardId.HecahandsIbtel, CardId.HecahandsYadel);
                _jauzahSearchUsed = true;
                return true;
            }
            return false;
        }

        // Hecahands Xeno: Quick Effect banish 1 opponent card
        private bool HecahandsXenoEffect()
        {
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            // Only activate if there's a meaningful target
            var target = Util.GetProblematicEnemyCard() ??
                         Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && c.Attack >= 2000) ??
                         Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // Chimera Illusion Beast: multi-attack + ATK reset
        private bool ChimeraIllusionBeastEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Only activate ATK reset if opponent has a high-ATK monster
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Attack >= Card.Attack))
                return true;
            // If we're about to enter battle phase and can attack for game
            if (Duel.Phase == DuelPhase.Main1 && CanDealLethal())
                return true;
            return false;
        }

        // Guardian Chimera: draw + destroy on fusion summon
        private bool GuardianChimeraEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;
            // Only activate destroy effect if opponent has meaningful targets
            if (Enemy.GetMonsters().Any(c => c != null && c.IsFaceup()) || Enemy.GetSpells().Any(c => c != null && c.IsFaceup()))
                return true;
            // Draw effect: always ok to draw
            return true;
        }

        // S:P Little Knight: banish disruption
        private bool SPLittleKnightEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            // First effect: On Summon banish (not our turn chain)
            if (LastChainCard == null || LastChainCard.Controller != 0)
            {
                // Only banish if there's a meaningful target
                var target = Util.GetProblematicEnemyCard() ??
                             Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && c.Attack >= 2000) ??
                             Enemy.GetSpells().FirstOrDefault(c => c != null && c.IsFaceup());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
                return false;
            }

            // Second effect: Quick Effect banish 2 monsters (1 ours + 1 opponent's)
            if (LastChainCard != null && LastChainCard.Controller == 1)
            {
                // Only respond if the chain is meaningful or targeting our key cards
                if (!Util.IsChainTarget(Bot.GetMonsters().FirstOrDefault(c => c != null && IsAceCard(c))) &&
                    !OpponentHasThreateningMonster()) return false;
                var ourTarget = Bot.GetMonsters().FirstOrDefault(c => c != null && !IsAceCard(c) && c != Card) ?? Card;
                var enemyTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
                if (ourTarget != null && enemyTarget != null)
                {
                    AI.SelectCard(ourTarget);
                    AI.SelectNextCard(enemyTarget);
                    return true;
                }
            }
            return false;
        }

        private bool BaytalHecahandsEffect()
        {
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                var tribute = Bot.GetMonsters().FirstOrDefault(c => c != null && !IsAceCard(c));
                if (tribute == null)
                {
                    var aceTribute = Bot.GetMonsters().FirstOrDefault(c => c != null && IsAceCard(c));
                    if (aceTribute != null)
                    {
                        var res = ResourcePlan.EvaluateAceUsage(
                            card: aceTribute,
                            hasLethalIfUsed: CanDealLethal(),
                            isOnlyAnswerToThreat: OpponentHasActiveNegator(),
                            haveAlternateWinCon: false
                        );
                        if (res.allowed)
                        {
                            tribute = aceTribute;
                            try { AI?.Log(LogLevel.Info, $"[ACE-ALLOW] Allowing tribute of Ace card {aceTribute.Id} for Bayt'al-Hecahands: {res.reason}"); } catch {}
                        }
                    }
                }
                if (tribute != null)
                {
                    AI.SelectCard(tribute);
                    AI.SelectNextCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool FallbackNormalSummon()
        {
            // Don't waste normal summon on Turn 1 going first เนโฌโ€ nothing to attack
            if (Duel.Turn == 1 && Duel.Player == 0) return false;
            // Only normal summon if we have no face-up monsters to keep pressure
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsAttack())) return false;
            // Summon any available monster as a beater
            return true;
        }

        private bool NightmareApprenticeEffect()
        {
            if (Bot.GetRemainingCount(CardId.MirrorSwordknight, 3) > 0)
                AI.SelectCard(CardId.MirrorSwordknight);
            else if (Bot.GetRemainingCount(CardId.CornfieldCoatl, 3) > 0)
                AI.SelectCard(CardId.CornfieldCoatl);
            else
                AI.SelectCard(CardId.NightmareApprentice);
            return true;
        }
    }

    [Deck("Expert_2026_EyeInside", "2026_EyeInside")]
    public class ExpertEyeInsideExecutor : _2026_EyeInsideExecutor
    {
        private string _duelId;
        public ExpertEyeInsideExecutor(GameAI ai, Duel duel) : base(ai, duel)
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
    }
}
