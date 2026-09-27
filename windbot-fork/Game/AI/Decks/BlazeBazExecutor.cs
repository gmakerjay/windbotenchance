// ============================================================
// CARD AUDIT — 2026_BlazeBaz
// | Card Name           | Type    | OPT? | Cost      | Effect                         | Activate When               | NEVER When                  |
// |---------------------|---------|------|-----------|--------------------------------|-----------------------------|-----------------------------|
// | Branded Fusion      | Spell   | HOPT | None      | Fusion using materials in deck | Main Phase to start combo   | SS blocked / Search deferred|
// | Aluber the Jester   | Monster | HOPT | None      | Search Branded spell/trap      | Summon to start combo       | Effect negated / Hand full  |
// | Blazing Cartesia    | Monster | HOPT | None      | Quick Fusion from hand/field   | Main Phase or opponent turn | SS blocked                  |
// | Keeper of Dragon    | Monster | OPT  | Discard 1 | Search Fusion spell            | Summon to search Branded F. | No discard fodder in hand   |
// | Springans Kitt      | Monster | HOPT | Return 1  | Search Branded spell           | Summon to search Branded F. | Hand empty                  |
// | Fallen of Albaz     | Monster | No   | Discard 1 | Fuse using opponent monster    | Normal summon vs enemy board| SS blocked                  |
// | Despian Tragedy     | Monster | HOPT | None      | Search Aluber when sent to GY  | Sent to GY / Banished       | Target missing in Deck      |
// | Albion Shrouded Drg | Monster | HOPT | None      | Send Albaz/Retribution to GY   | Main Phase hand/GY          | No target in Deck           |
// | Dark Hole           | Spell   | No   | None      | Wipes all monsters on field    | Going 2nd / empty own board | Own boss on field          |
// | Branded in White    | Quick   | No   | None      | Quick Fusion using hand/field  | Main Phase / Hand overflow  | SS blocked                  |
// | Branded Opening     | Quick   | HOPT | Discard 1 | SS or search Aluber from Deck  | Main Phase starter          | No discard fodder           |
// | Branded in Spirits  | Quick   | HOPT | Discard 1 | Discard Dragon to search Albaz | Main Phase                   | No Dragon cost in hand      |
// | Branded in Red      | Quick   | HOPT | Target GY | Recover Despia/Albaz and fuse  | Main Phase / Opponent turn  | Hand near full              |
// | Branded Retribution | Trap    | HOPT | GY banish | Negate summon or recover spell | Set turn 1 / GY recover     | SS not blocked              |
// ============================================================
// ACE CARDS: Primary: Mirrorjade the Iceblade Dragon / Secondary: Lubellion, Albion
// COMBO STARTERS: 1. Branded Fusion 2. Aluber 3. Branded Opening 4. Keeper of Dragon Magic 5. Springans Kitt
// CHOKEPOINTS: Ash Blossom / Effect Veiler on Branded Fusion or Aluber
// WIN CONDITION: Summon Mirrorjade the Iceblade Dragon and control board with banish + Retribution counter trap.
// GOING 1ST END BOARD: Mirrorjade + set Branded Retribution / backrow
// GOING 2ND GAMEPLAN: Fuse using opponent monsters via Fallen of Albaz or Branded Fusion, then push lethal.
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
    [Deck("2026_BlazeBaz", "2026_BlazeBaz")]
    public class _2026_BlazeBazExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int DespianTragedy = 36577931;
            public const int KeeperOfDragonMagic = 48048590;
            public const int FallenOfAlbaz = 68468459;
            public const int AluberTheJesterOfDespia = 62962630;
            public const int SpringansKitt = 45484331;
            public const int BlazingCartesiaTheVirtuous = 95515789;
            public const int AlbionTheShroudedDragon = 25451383;
            public const int DarkHole = 53129443;
            public const int BrandedInWhite = 34995106;
            public const int BrandedFusion = 44362883;
            public const int ForbiddenLance = 27243130;
            public const int CosmicCyclone = 8267140;
            public const int BrandedOpening = 36637374;
            public const int BrandedInHighSpirits = 29948294;
            public const int BrandedInRed = 82738008;
            public const int BrandedRetribution = 17751597;

            // Extra Deck
            public const int AlbionTheBrandedDragon = 87746184;
            public const int MirrorjadeTheIcebladeDragon = 44146295;
            public const int LubellionTheSearingDragon = 70534340;
        }

        private static readonly int[] AceCardIds = {
            CardId.MirrorjadeTheIcebladeDragon,
            CardId.LubellionTheSearingDragon,
            CardId.AlbionTheBrandedDragon
        };

        private bool _brandedFusionUsed = false;
        private bool _cartesiaUsed = false;
        private bool _mirrorjadeUsed = false;
        private bool _lubellionUsed = false;
        private bool _aluberUsed = false;
        private bool _springansKittUsed = false;
        private bool _keeperUsed = false;
        private bool _brandedOpeningUsed = false;

        // ═══════════════════════════════════════
        //  HAND MANAGEMENT HELPERS
        // ═══════════════════════════════════════
        // Prevents hand overflow: checks if hand is near full and we have no good discard target.
        private bool HandNearFull() => Bot.Hand.Count >= 5;

        // Checks if we have a card we can safely discard (non-critical, non-Ace).
        private bool HasDiscardFodder()
        {
            return Bot.Hand.Any(c => c != null && !IsAceCard(c) &&
                !c.IsCode(CardId.BrandedFusion, CardId.AluberTheJesterOfDespia,
                    CardId.KeeperOfDragonMagic, CardId.SpringansKitt,
                    CardId.BlazingCartesiaTheVirtuous, CardId.FallenOfAlbaz,
                    CardId.BrandedOpening));
        }

        // True if activating a search/draw effect would risk overflow.
        private bool SearchWouldOverflow()
        {
            // If hand is not near full, it's safe
            if (!HandNearFull()) return false;
            // If we have discard fodder, we can manage hand size
            if (HasDiscardFodder()) return false;
            // If we have an active discard outlet (Lubellion on field, Cartesia on field), it's safe
            if (Bot.GetMonsters().Any(c => c != null && c.IsFaceup() &&
                c.IsCode(CardId.LubellionTheSearingDragon, CardId.BlazingCartesiaTheVirtuous)))
                return false;
            return true;
        }

        public _2026_BlazeBazExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            HeuristicGuard.RegisterAceCards(AceCardIds);

            // ── Combo Router ──
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "BrandedFusion-First",
                RequiredCards = new List<int> { CardId.BrandedFusion },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.BrandedFusion, ActionType = ExecutorType.Activate, Description = "Activate Branded Fusion" },
                    new() { CardId = CardId.LubellionTheSearingDragon, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Lubellion" },
                    new() { CardId = CardId.MirrorjadeTheIcebladeDragon, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Mirrorjade" }
                },
                EndBoardScore = 85
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Aluber-To-BrandedFusion",
                RequiredCards = new List<int> { CardId.AluberTheJesterOfDespia },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.AluberTheJesterOfDespia, ActionType = ExecutorType.Summon, Description = "Normal Summon Aluber" },
                    new() { CardId = CardId.AluberTheJesterOfDespia, ActionType = ExecutorType.Activate, Description = "Activate Aluber search" },
                    new() { CardId = CardId.BrandedFusion, ActionType = ExecutorType.Activate, Description = "Activate Branded Fusion" },
                    new() { CardId = CardId.LubellionTheSearingDragon, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Lubellion" },
                    new() { CardId = CardId.MirrorjadeTheIcebladeDragon, ActionType = ExecutorType.SpSummon, Description = "Fusion Summon Mirrorjade" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "BrandedOpening-To-Aluber",
                RequiredCards = new List<int> { CardId.BrandedOpening },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.BrandedOpening, ActionType = ExecutorType.Activate, Description = "Activate Branded Opening" },
                    new() { CardId = CardId.AluberTheJesterOfDespia, ActionType = ExecutorType.Activate, Description = "Activate Aluber search" },
                    new() { CardId = CardId.BrandedFusion, ActionType = ExecutorType.Activate, Description = "Activate Branded Fusion" }
                },
                EndBoardScore = 80
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "FallenOfAlbaz-FuseEnemy",
                RequiredCards = new List<int> { CardId.FallenOfAlbaz },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.FallenOfAlbaz, ActionType = ExecutorType.Summon, Description = "Summon Fallen of Albaz" },
                    new() { CardId = CardId.FallenOfAlbaz, ActionType = ExecutorType.Activate, Description = "Fuse with opponent monster" }
                },
                EndBoardScore = 75,
                Condition = () => Enemy.GetMonsters().Any(m => m != null && m.IsFaceup()) && Duel.Turn > 1
            });

            BaitPlanner.RegisterComboStarters(CardId.BrandedFusion);
            BaitPlanner.RegisterBaitCards(CardId.AluberTheJesterOfDespia, CardId.KeeperOfDragonMagic, CardId.SpringansKitt);
            ChainAdvisor.RegisterHighValueTargets(CardId.BrandedFusion, CardId.AluberTheJesterOfDespia);

            // ===== Priority 1: Hand Traps & Reactive Spells =====
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenLance, ForbiddenLanceEffect);
            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, CosmicCycloneEffect);

            // ===== Priority 2: Quick Effects & Board Breakers =====
            AddExecutor(ExecutorType.Activate, CardId.DarkHole, DarkHoleEffect);

            // ===== Priority 3: Boss Monster Quick Effects =====
            AddExecutor(ExecutorType.Activate, CardId.MirrorjadeTheIcebladeDragon, MirrorjadeEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlazingCartesiaTheVirtuous, BlazingCartesiaEffect);
            AddExecutor(ExecutorType.Activate, CardId.LubellionTheSearingDragon, LubellionEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlbionTheBrandedDragon, AlbionEffect);

            // ===== Priority 4: Engine Spells (Hand-Consuming First to Avoid Overflow) =====
            // Branded in High Spirits: Discard Dragon from hand to search Albaz (net -1 hand) — prioritize when near full
            AddExecutor(ExecutorType.Activate, CardId.BrandedInHighSpirits, BrandedInHighSpiritsEffect);
            // Branded in White: Quick-Play Fusion using hand/field materials — consumes hand cards
            AddExecutor(ExecutorType.Activate, CardId.BrandedInWhite, BrandedInWhiteEffect);
            // Normal fusion spells
            AddExecutor(ExecutorType.Activate, CardId.BrandedFusion, BrandedFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.BrandedOpening, BrandedOpeningEffect);
            // Branded in Red: adds from GY to hand (net +1) — skip when hand is full!
            AddExecutor(ExecutorType.Activate, CardId.BrandedInRed, BrandedInRedEffect);

            // ===== Priority 5: Monster Effects (Hand / Field) =====
            AddExecutor(ExecutorType.Activate, CardId.AluberTheJesterOfDespia, AluberEffect);
            AddExecutor(ExecutorType.Activate, CardId.SpringansKitt, SpringansKittEffect);
            AddExecutor(ExecutorType.Activate, CardId.KeeperOfDragonMagic, KeeperOfDragonMagicEffect);
            AddExecutor(ExecutorType.Activate, CardId.FallenOfAlbaz, FallenOfAlbazEffect);
            AddExecutor(ExecutorType.Activate, CardId.DespianTragedy, DespianTragedyEffect);
            AddExecutor(ExecutorType.Activate, CardId.AlbionTheShroudedDragon, AlbionTheShroudedDragonEffect);

            // ===== Priority 6: Normal Summons =====
            AddExecutor(ExecutorType.Summon, CardId.AluberTheJesterOfDespia, NormalSummonAluber);
            AddExecutor(ExecutorType.Summon, CardId.KeeperOfDragonMagic, NormalSummonKeeper);
            AddExecutor(ExecutorType.Summon, CardId.SpringansKitt, NormalSummonKitt);
            AddExecutor(ExecutorType.Summon, CardId.BlazingCartesiaTheVirtuous);
            AddExecutor(ExecutorType.Summon, CardId.FallenOfAlbaz);

            // ===== Priority 7: Extra Deck Summons =====
            AddExecutor(ExecutorType.SpSummon, CardId.MirrorjadeTheIcebladeDragon, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.LubellionTheSearingDragon, FusionSummonCheck);
            AddExecutor(ExecutorType.SpSummon, CardId.AlbionTheBrandedDragon, FusionSummonCheck);

            // ===== Priority 8: Traps & Repos =====
            AddExecutor(ExecutorType.Activate, CardId.BrandedRetribution, BrandedRetributionEffect);
            AddExecutor(ExecutorType.SpellSet, SpellSetFiltered);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _brandedFusionUsed = false;
            _cartesiaUsed = false;
            _mirrorjadeUsed = false;
            _lubellionUsed = false;
            _aluberUsed = false;
            _springansKittUsed = false;
            _keeperUsed = false;
            _brandedOpeningUsed = false;

            if (ShouldGoBreakBoard)
            {
                // Reset key engine OPTs for aggressive going-second plays
                _brandedFusionUsed = false;
                _cartesiaUsed = false;
            }
        }

        public override bool OnSelectHand()
        {
            return true; // Go First
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return AceCardIds.Contains(card.Id);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Id == CardId.MirrorjadeTheIcebladeDragon) return 980;
            if (c.Id == CardId.LubellionTheSearingDragon) return 920;
            if (c.Id == CardId.AlbionTheBrandedDragon)
            {
                if (Bot.HasInMonstersZone(CardId.MirrorjadeTheIcebladeDragon))
                    return 200;
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
                    var nonAceMaterials = sorted.Where(c => !IsAceCard(c)).ToList();
                    if (nonAceMaterials.Count < min)
                    {
                        DecisionTracer.Trace("OnSelectCard", "Cancelling material selection to protect Ace cards");
                        return null; // Cancel to protect Aces
                    }
                }
                return Util.CheckSelectCount(sorted, cards, min, max);
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            var result = new List<ClientCard>();
            var handMats = cards.Where(c => c != null && c.Location == CardLocation.Hand).ToList();
            var graveMats = cards.Where(c => c != null && c.Location == CardLocation.Grave).ToList();
            var fieldNonAce = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && !IsAceCard(c)).ToList();
            var fieldAce = cards.Where(c => c != null && c.Location == CardLocation.MonsterZone && IsAceCard(c)).ToList();

            foreach (var c in handMats) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max)
                foreach (var c in graveMats) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max)
                foreach (var c in fieldNonAce) { result.Add(c); if (result.Count >= max) break; }
            if (result.Count < max)
                foreach (var c in fieldAce) { result.Add(c); if (result.Count >= max) break; }

            if (result.Count >= min) return result;
            return base.OnSelectFusionMaterial(cards, min, max);
        }

        private bool IsExtraDeckLocked()
        {
            return false;
        }

        private bool FusionSummonCheck()
        {
            if (IsExtraDeckLocked())
            {
                DecisionTracer.TraceSkip("FusionSummonCheck", "Extra Deck is locked");
                return false;
            }
            bool allowed = !IsSpecialSummonBlocked();
            if (!allowed)
                DecisionTracer.TraceSkip("FusionSummonCheck", "Special Summon is blocked by floodgate");
            return allowed;
        }

        protected override bool IsSpecialSummonBlocked()
        {
            return base.IsSpecialSummonBlocked();
        }

        private bool ForbiddenLanceEffect()
        {
            ClientCard target = Util.GetLastChainCard();
            if (target != null && target.Controller == 1 && (target.IsSpell() || target.IsTrap()))
            {
                // Protect our Mirrorjade or face-up monsters
                ClientCard protect = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.IsCode(CardId.MirrorjadeTheIcebladeDragon))
                    ?? Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup());
                if (protect != null)
                {
                    AI.SelectCard(protect);
                    DecisionTracer.TraceActivate("ForbiddenLance", $"Protecting {protect.Name} from opponent's Spell/Trap");
                    return true;
                }
            }
            DecisionTracer.TraceSkip("ForbiddenLance", "No target or incorrect controller");
            return false;
        }

        private bool CosmicCycloneEffect()
        {
            ClientCard target = Enemy.GetSpells().FirstOrDefault(c => c != null && (c.IsFaceup() || c.IsFacedown()));
            if (target != null && Bot.LifePoints > 1000)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("CosmicCyclone", $"Targeting enemy Spell/Trap: {target.Id}");
                return true;
            }
            DecisionTracer.TraceSkip("CosmicCyclone", "No Spell/Trap target or LP too low");
            return false;
        }

        private bool DarkHoleEffect()
        {
            if (Bot.GetMonsterCount() == 0 && Enemy.GetMonsterCount() > 0)
            {
                DecisionTracer.TraceActivate("DarkHole", "Activating Dark Hole: empty field and enemy has monsters");
                return true;
            }
            DecisionTracer.TraceSkip("DarkHole", "Field not empty or enemy has no monsters");
            return false;
        }

        private bool MirrorjadeEffect()
        {
            // Mirrorjade has two effects:
            // 1) Quick Effect (field): Send 1 "Albaz" Fusion from Extra Deck to GY, banish 1 monster on field
            // 2) Automatic trigger (NOT activatable): If Fusion Summoned card leaves field by opponent's card,
            //    destroy all opponent monsters during End Phase. This is handled automatically by the game engine.

            if (Card.Location != CardLocation.MonsterZone)
            {
                // The board wipe is an automatic trigger — we should NOT try to manually activate it.
                DecisionTracer.TraceSkip("Mirrorjade", "Card not on field; board wipe is automatic trigger");
                return false;
            }

            if (_mirrorjadeUsed)
            {
                DecisionTracer.TraceSkip("Mirrorjade", "Banish effect already used this turn");
                return false;
            }

            // Must have an "Albaz" Fusion monster in Extra Deck to send as cost
            bool hasFusionCost = GetRemainingCount(CardId.AlbionTheBrandedDragon) > 0;
            if (!hasFusionCost)
            {
                DecisionTracer.TraceSkip("Mirrorjade", "No Albaz Fusion in Extra Deck to send as cost");
                return false;
            }

            ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup());
            if (target != null)
            {
                AI.SelectCard(CardId.AlbionTheBrandedDragon);
                AI.SelectNextCard(target);
                _mirrorjadeUsed = true;
                DecisionTracer.TraceActivate("Mirrorjade", $"Banish targeting: {target.Id}");
                return true;
            }

            DecisionTracer.TraceSkip("Mirrorjade", "No face-up enemy monster to banish");
            return false;
        }

        private bool BlazingCartesiaEffect()
        {
            if (_cartesiaUsed)
            {
                DecisionTracer.TraceSkip("BlazingCartesia", "Already used this turn");
                return false;
            }

            if (Card.Location == CardLocation.Hand)
            {
                bool hasAlbaz = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.FallenOfAlbaz)) ||
                               Bot.Graveyard.Any(c => c != null && c.IsCode(CardId.FallenOfAlbaz));
                if (hasAlbaz && !IsSpecialSummonBlocked())
                {
                    DecisionTracer.TraceActivate("BlazingCartesia", "Special summoning Cartesia from hand");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                if (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2)
                {
                    // [HAND-FLOW] Cartesia's Quick Fusion on field uses hand/field materials.
                    // When hand is near full, prioritize this to consume cards.
                    bool handNeedsDump = HandNearFull() && !HasDiscardFodder();
                    bool hasAlbazMat = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.FallenOfAlbaz))
                                    || Bot.Hand.Any(c => c != null && c.IsCode(CardId.FallenOfAlbaz));

                    if (hasAlbazMat || handNeedsDump)
                    {
                        _cartesiaUsed = true;
                        DecisionTracer.TraceActivate("BlazingCartesia", "Quick fusion using hand/field materials");
                        return true;
                    }
                }
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (Duel.Phase == DuelPhase.End)
                {
                    DecisionTracer.TraceActivate("BlazingCartesia", "Returning Cartesia from GY to hand");
                    return true;
                }
            }
            return false;
        }

        private bool LubellionEffect()
        {
            if (_lubellionUsed)
            {
                DecisionTracer.TraceSkip("Lubellion", "Already used this turn");
                return false;
            }
            if (Card.Location == CardLocation.MonsterZone)
            {
                // [HAND-FLOW] Lubellion discards 1 card (cost) then fuses.
                // This is a KEY discard outlet. Always activate when:
                // 1) We have Mirrorjade as fusion target (normal case), OR
                // 2) Hand is near-full and we need the discard
                bool canFuseMirrorjade = GetRemainingCount(CardId.MirrorjadeTheIcebladeDragon) > 0;
                bool handNeedsDump = HandNearFull() && !HasDiscardFodder();

                if (!canFuseMirrorjade && !handNeedsDump)
                {
                    DecisionTracer.TraceSkip("Lubellion", "No Mirrorjade target and hand not overflowing");
                    return false;
                }

                // Discard: prefer Traged y (triggers GY effect), then Retribution, then least important
                AI.SelectCard(new[] {
                    CardId.DespianTragedy,
                    CardId.BrandedRetribution,
                    CardId.BrandedInHighSpirits
                });
                // Shuffle back materials to summon Mirrorjade
                AI.SelectNextCard(new[] {
                    CardId.FallenOfAlbaz,
                    CardId.LubellionTheSearingDragon
                });
                _lubellionUsed = true;
                DecisionTracer.TraceActivate("Lubellion", "Activating Fusion Summon Mirrorjade effect");
                return true;
            }
            return false;
        }

        private bool AlbionEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // Fusion summon using materials in hand/field/GY
                _brandedFusionUsed = true;
                DecisionTracer.TraceActivate("Albion", "Activating Fusion Summon effect on summon");
                return true;
            }
            return false;
        }

        private bool BrandedFusionEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (IsMain1SearchDeferred()) return false;
            if (_brandedFusionUsed)
            {
                DecisionTracer.TraceSkip("BrandedFusion", "Already used this turn");
                return false;
            }
            if (IsSpecialSummonBlocked())
            {
                DecisionTracer.TraceSkip("BrandedFusion", "Special summon is blocked by floodgate");
                return false;
            }

            AI.SelectCard(CardId.LubellionTheSearingDragon);
            AI.SelectNextCard(new[] {
                CardId.FallenOfAlbaz,
                CardId.DespianTragedy,
                CardId.BlazingCartesiaTheVirtuous
            });
            _brandedFusionUsed = true;
            DecisionTracer.TraceActivate("BrandedFusion", "Activating Branded Fusion using deck materials");
            return true;
        }

        private bool BrandedOpeningEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_brandedOpeningUsed)
            {
                DecisionTracer.TraceSkip("BrandedOpening", "Already used this turn");
                return false;
            }

            var discard = Bot.Hand.FirstOrDefault(c => c != null && c.IsCode(CardId.DespianTragedy, CardId.BrandedRetribution))
                ?? Bot.Hand.FirstOrDefault(c => c != null && c.Id != CardId.BrandedFusion && c.Id != CardId.BrandedOpening && c.Id != CardId.AluberTheJesterOfDespia)
                ?? Bot.Hand.FirstOrDefault();

            if (discard == null) return false;

            if (IsSpecialSummonBlocked())
            {
                AI.SelectCard(discard);
                AI.SelectNextCard(CardId.AluberTheJesterOfDespia);
                AI.SelectOption(0); // Add to hand
                _brandedOpeningUsed = true;
                DecisionTracer.TraceActivate("BrandedOpening", $"Discarding {discard.Name} to add Aluber to hand (SS blocked)");
                return true;
            }
            else
            {
                AI.SelectCard(discard);
                AI.SelectNextCard(CardId.AluberTheJesterOfDespia);
                AI.SelectOption(1); // Special Summon
                _brandedOpeningUsed = true;
                DecisionTracer.TraceActivate("BrandedOpening", $"Discarding {discard.Name} to Special Summon Aluber from deck");
                return true;
            }
        }

        private bool BrandedInHighSpiritsEffect()
        {
            if (ShouldSkipCombo()) return false;
            // Send 1 Dragon monster from hand to GY (cost), then search Fallen of Albaz or Despia monster.
            // Net hand change: -1 (discard) + 1 (search) = 0. Does not cause overflow.
            // However, if we can't send a Dragon (cost), we can't activate.
            var dragonInHand = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.Race == (int)CardRace.Dragon);
            if (dragonInHand == null)
            {
                // [HAND-FLOW] If hand is near full and we only have non-Dragon monsters,
                // we could still use Albion the Shrouded Dragon if available (it's a Dragon itself).
                // But if no Dragon at all, we can't pay the cost.
                DecisionTracer.TraceSkip("BrandedInHighSpirits", "No Dragon cost in hand");
                return false;
            }

            // If we have multiple Dragons, prefer discarding the least important one.
            var preferredDiscard = Bot.Hand.FirstOrDefault(c => c != null && c.IsMonster() && c.Race == (int)CardRace.Dragon
                && !c.IsCode(CardId.FallenOfAlbaz, CardId.BlazingCartesiaTheVirtuous))
                ?? dragonInHand;

            AI.SelectCard(preferredDiscard);
            AI.SelectNextCard(CardId.FallenOfAlbaz);
            DecisionTracer.TraceActivate("BrandedInHighSpirits", $"Sending {preferredDiscard.Name} to search Fallen of Albaz");
            return true;
        }

        private bool BrandedInRedEffect()
        {
            // [HAND-FLOW] Branded in Red adds 1 card from GY to hand (net +1 to hand).
            // Skip if hand is near full with no discard outlet.
            if (SearchWouldOverflow())
            {
                DecisionTracer.TraceSkip("BrandedInRed", "Hand near full — skipping GY recovery to avoid overflow");
                return false;
            }

            // Add Despia or Albaz from GY to hand, then fuse
            var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.DespianTragedy, CardId.AluberTheJesterOfDespia, CardId.FallenOfAlbaz));
            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("BrandedInRed", $"Adding {target.Name} from GY to hand to fuse");
                return true;
            }
            DecisionTracer.TraceSkip("BrandedInRed", "No target Despia or Albaz in GY");
            return false;
        }

        private bool BrandedInWhiteEffect()
        {
            // Branded in White (Quick-Play Spell): Fusion Summon 1 monster using materials from hand/field.
            // Must use at least 1 Dragon as material.

            if (IsSpecialSummonBlocked())
            {
                DecisionTracer.TraceSkip("BrandedInWhite", "Special Summon is blocked by floodgate");
                return false;
            }

            // Check we have a viable fusion target in Extra Deck
            bool hasFusionTarget = GetRemainingCount(CardId.MirrorjadeTheIcebladeDragon) > 0 ||
                GetRemainingCount(CardId.LubellionTheSearingDragon) > 0 ||
                GetRemainingCount(CardId.AlbionTheBrandedDragon) > 0;

            if (!hasFusionTarget)
            {
                DecisionTracer.TraceSkip("BrandedInWhite", "No valid Fusion target in Extra Deck");
                return false;
            }

            // Check we have materials in hand or field (at least 1 Dragon)
            bool hasDragonMat = Bot.Hand.Any(c => c != null && c.IsMonster() && c.Race == (int)CardRace.Dragon)
                             || Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.Race == (int)CardRace.Dragon);

            bool hasAnyMat = Bot.Hand.Any(c => c != null && c.IsMonster())
                          || Bot.GetMonsters().Any(c => c != null && c.IsFaceup());

            if (!hasDragonMat || !hasAnyMat)
            {
                DecisionTracer.TraceSkip("BrandedInWhite", "Not enough fusion materials (need at least 1 Dragon)");
                return false;
            }

            // [HAND-FLOW] When hand is near-full, strongly prefer using hand materials to fuse
            // to consume extra cards. Branded in White uses hand/field materials.
            if (HandNearFull() && !HasDiscardFodder())
            {
                DecisionTracer.TraceActivate("BrandedInWhite", "Hand near full — fusing to consume hand cards");
                AI.SelectCard(new[] {
                    CardId.LubellionTheSearingDragon,
                    CardId.MirrorjadeTheIcebladeDragon,
                    CardId.AlbionTheBrandedDragon
                });
                return true;
            }

            // Prefer Mirrorjade as fusion target, fall back to Lubellion then Albion
            AI.SelectCard(new[] {
                CardId.MirrorjadeTheIcebladeDragon,
                CardId.LubellionTheSearingDragon,
                CardId.AlbionTheBrandedDragon
            });

            DecisionTracer.TraceActivate("BrandedInWhite", "Activating Branded in White for Fusion Summon");
            return true;
        }

        private bool AluberEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                // [HAND-FLOW] Skip search if hand is near-full and we can't discard.
                // Aluber adds 1 card to hand — this INCREASES hand size and may cause overflow.
                if (SearchWouldOverflow())
                {
                    DecisionTracer.TraceSkip("Aluber", "Hand near full with no discard outlet — skipping search to avoid overflow");
                    return false;
                }

                if (_aluberUsed)
                {
                    DecisionTracer.TraceSkip("Aluber", "Search effect already used");
                    return false;
                }
                AI.SelectCard(new[] {
                    CardId.BrandedFusion,
                    CardId.BrandedOpening,
                    CardId.BrandedInRed,
                    CardId.BrandedRetribution
                });
                _aluberUsed = true;
                DecisionTracer.TraceActivate("Aluber", "Searching Branded spell/trap");
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                ClientCard target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled());
                if (target != null && !IsSpecialSummonBlocked())
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("Aluber", $"Special Summoning Aluber from GY and negating {target.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool SpringansKittEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                bool hasListedFusion = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && c.IsCode(CardId.MirrorjadeTheIcebladeDragon, CardId.LubellionTheSearingDragon, CardId.AlbionTheBrandedDragon));
                if (hasListedFusion && !IsSpecialSummonBlocked())
                {
                    DecisionTracer.TraceActivate("SpringansKitt", "Special Summoning Springans Kitt from hand");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.MonsterZone)
            {
                if (_springansKittUsed)
                {
                    DecisionTracer.TraceSkip("SpringansKitt", "Search effect already used");
                    return false;
                }
                var returnCard = Bot.Hand.FirstOrDefault(c => c != null && c.Id != CardId.BrandedFusion && c.Id != CardId.SpringansKitt) ?? Bot.Hand.FirstOrDefault();
                if (returnCard != null)
                {
                    AI.SelectCard(CardId.BrandedFusion);
                    AI.SelectNextCard(returnCard);
                    _springansKittUsed = true;
                    DecisionTracer.TraceActivate("SpringansKitt", $"Searching Branded Fusion and returning {returnCard.Name} to deck");
                    return true;
                }
            }
            return false;
        }

        private bool KeeperOfDragonMagicEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (!_keeperUsed && Bot.Hand.Count > 1)
                {
                    var discard = Bot.Hand.FirstOrDefault(c => c != null && c.IsCode(CardId.DespianTragedy, CardId.BrandedRetribution))
                        ?? Bot.Hand.FirstOrDefault(c => c != null && c.Id != CardId.BrandedFusion && c.Id != CardId.KeeperOfDragonMagic)
                        ?? Bot.Hand.FirstOrDefault();
                    if (discard != null)
                    {
                        AI.SelectCard(discard);
                        AI.SelectNextCard(CardId.BrandedFusion);
                        _keeperUsed = true;
                        DecisionTracer.TraceActivate("KeeperOfDragonMagic", $"Searching Branded Fusion by discarding {discard.Name}");
                        return true;
                    }
                }
                else
                {
                    var albaz = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.FallenOfAlbaz));
                    if (albaz != null && !IsSpecialSummonBlocked())
                    {
                        AI.SelectCard(CardId.MirrorjadeTheIcebladeDragon);
                        AI.SelectNextCard(albaz);
                        DecisionTracer.TraceActivate("KeeperOfDragonMagic", "Reviving Fallen of Albaz in face-down Defense");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool FallenOfAlbazEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location == CardLocation.MonsterZone)
            {
                AI.SelectCard(new[] {
                    CardId.DespianTragedy,
                    CardId.BrandedRetribution
                });
                DecisionTracer.TraceActivate("FallenOfAlbaz", "Fusing using opponent monsters");
                return true;
            }
            return false;
        }

        private bool DespianTragedyEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) && Duel.CurrentChain.Count == 0)
                {
                    var target = Bot.Graveyard.FirstOrDefault(c => c != null && c.IsCode(CardId.BrandedFusion, CardId.BrandedOpening, CardId.BrandedInRed) && c != Card);
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        DecisionTracer.TraceActivate("DespianTragedy", $"Banishing to set {target.Name} from GY");
                        return true;
                    }
                }
                else
                {
                    AI.SelectCard(CardId.AluberTheJesterOfDespia);
                    DecisionTracer.TraceActivate("DespianTragedy", "Searching Aluber");
                    return true;
                }
            }
            else if (Card.Location == CardLocation.Removed)
            {
                AI.SelectCard(CardId.AluberTheJesterOfDespia);
                DecisionTracer.TraceActivate("DespianTragedy", "Searching Aluber");
                return true;
            }
            return false;
        }

        private bool AlbionTheShroudedDragonEffect()
        {
            if (Card.Location == CardLocation.Hand || Card.Location == CardLocation.Grave)
            {
                // Send Branded Retribution or Fallen of Albaz to GY
                AI.SelectCard(new[] {
                    CardId.BrandedRetribution,
                    CardId.FallenOfAlbaz
                });
                DecisionTracer.TraceActivate("AlbionTheShroudedDragon", "Sending Albaz/Retribution to GY");
                return true;
            }
            return false;
        }

        private bool NormalSummonAluber()
        {
            bool cond = !_aluberUsed;
            if (cond)
                DecisionTracer.TraceActivate("NormalSummonAluber", "Summoning Aluber to search Branded spell/trap");
            return cond;
        }

        private bool NormalSummonKeeper()
        {
            bool cond = !_keeperUsed && Bot.Hand.Count > 1;
            if (cond)
                DecisionTracer.TraceActivate("NormalSummonKeeper", "Summoning Keeper of Dragon Magic");
            return cond;
        }

        private bool NormalSummonKitt()
        {
            bool cond = !_springansKittUsed && Bot.Hand.Count > 1;
            if (cond)
                DecisionTracer.TraceActivate("NormalSummonKitt", "Summoning Springans Kitt");
            return cond;
        }

        private bool BrandedRetributionEffect()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // Banished to add Branded spell/trap from GY to hand
                AI.SelectCard(CardId.BrandedFusion);
                DecisionTracer.TraceActivate("BrandedRetribution", "Banishing to add Branded Fusion from GY");
                return true;
            }
            else if (Card.Location == CardLocation.SpellZone && !Card.IsFacedown())
            {
                // Negate special summon effect
                ClientCard target = Util.GetLastChainCard();
                if (target != null && target.Controller == 1)
                {
                    DecisionTracer.TraceActivate("BrandedRetribution", $"Negating summon effect of {target.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool SpellSetFiltered()
        {
            if (Card.IsCode(CardId.BrandedRetribution, CardId.BrandedInRed, CardId.BrandedOpening))
            {
                bool cond = SetBackrowCondition();
                if (cond)
                    DecisionTracer.TraceActivate("SpellSetFiltered", $"Setting {Card.Name}");
                return cond;
            }
            return false;
        }

        private bool SetBackrowCondition() => Util.IsTurn1OrMain2();
        protected override bool IsBoardStrongEnough()
        {
            bool hasMirrorjade = Bot.HasInMonstersZone(CardId.MirrorjadeTheIcebladeDragon);
            bool hasLubellion = Bot.HasInMonstersZone(CardId.LubellionTheSearingDragon);
            bool hasAlbion = Bot.HasInMonstersZone(CardId.AlbionTheBrandedDragon);
            bool hasRetributionSet = Bot.GetSpells().Any(c => c != null && (c.IsFaceup() || c.IsFacedown()) && c.IsCode(CardId.BrandedRetribution));

            if (hasMirrorjade && (hasRetributionSet || Bot.GetMonsterCount() >= 2)) return true;
            if ((hasLubellion || hasAlbion) && hasRetributionSet) return true;

            int disruption = Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && m.IsMonsterDangerous());
            if (disruption >= 2) return true;
            int atk = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            if (atk >= 5000) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough()) return true;
            return base.ShouldStopExtending();
        }

    }
}
