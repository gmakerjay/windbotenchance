// ============================================================
// CARD AUDIT โ€” 2026_FairyTailPure
// | Card Name           | Type    | OPT? | Cost      | Effect              | Activate When        | NEVER When           |
// |---------------------|---------|------|-----------|---------------------|----------------------|----------------------|
// | FairyTailLuna       | Monster | HOPT | None      | Search and bounce   | Setup, disrupt opponent| Opponent has no target|
// | FairyTailSnow       | Monster | HOPT | Banish 7  | SS itself & book    | Disrupt, block attack| Banish resources critical|
// ACE CARDS: Primary: FairyTailSnow / Secondary: FairyTailLuna
// COMBO STARTERS: 1. FairyTailLuna 2. Terraforming
// CHOKEPOINTS: Luna normal summon negated
// WIN CONDITION: Disrupt opponent using Snow and Luna bounces, winning by attrition
// GOING 1ST END BOARD: Luna on field + Snow in GY
// GOING 2ND GAMEPLAN: Bounce enemy negates with Luna, special summon Snow to secure board state
// ============================================================

// ============================================================
// COMBO DRAFT โ€” 2026_FairyTailPure
// ============================================================
// === COMBO LINE 1: Control Setup (Starter: Luna) ===
// HAND REQUIRED: FairyTailLuna
// STEP 1: Normal Summon FairyTailLuna
// STEP 2: Luna Effect: Search FairyTailSnow or Rochka
// STEP 3: Set Book of Moon / traps to protect Luna
// END BOARD: Luna on field + disruptions
// === COMBO LINE 2: Snow GY Loop ===
// STEP 1: Foolish/discard Snow to GY
// STEP 2: When opponent attacks or activates effect, banish 7 to SS Snow
// STEP 3: Snow Effect: Flip opponent's monster face-down
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
    [Deck("2026_FairyTailPure", "2026_FairyTailPure")]
    public class _2026_FairyTailPureExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int FairyTailLuna = 86937530;
            public const int FairyTailRochka = 65953423;
            public const int FairyTailSnow = 55623480;
            public const int FairyTailSleeper = 42921475;
            public const int FairyTailMatchgiru = 19144622;
            public const int GeniaOfTheRing = 99745551;
            public const int OnceUponAFairyTail = 19326613;
            public const int FairyTailTales = 91957038;
            public const int TailsOfTheFairyTails = 82119326;
            public const int FairyTailBall = 56725612;
            public const int FairyPrince = 10000120;
            public const int PairBearScare = 21501961;
            public const int TheFallenAndTheVirtuous = 30271097;
            public const int Terraforming = 73628505;
            public const int Metamorphosis = 46411259;
            public const int BookOfMoon = 14087893;
            public const int ForbiddenDroplet = 24299458;
            public const int SuperPolymerization = 48130397;
            public const int InstantFusion = 1845204;

            // Extra Deck
            public const int WeaverOfFairyTails = 78021082;
            public const int TellerOfFairyTails = 4026187;
            public const int MagistusChorozo = 66532962;
            public const int SecreterionDragon = 89851827;
            public const int PanzerDragon = 72959823;
            public const int SeaMonsterOfTheseus = 96334243;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int MudragonOfTheSwamp = 54757758;
            public const int AlbionTheBrandedDragon = 87746184;
            public const int TheDragonThatDevoursTheDogma = 76666602;
            public const int FairyTailWiccat = 27632520;
        }

        private bool _lunaSearched = false;
        private bool _snowSummonedThisTurn = false;
        private bool _tailsUsed = false;
        private bool _onceUponUsed = false;

        public _2026_FairyTailPureExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            // Register Ace Cards
            HeuristicGuard.RegisterAceCards(
                CardId.WeaverOfFairyTails,
                CardId.TellerOfFairyTails,
                CardId.SecreterionDragon,
                CardId.TheDragonThatDevoursTheDogma,
                CardId.FairyTailLuna,
                CardId.FairyTailSnow
            );
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Combo-1",
                RequiredCards = new List<int> { CardId.FairyTailLuna, CardId.FairyTailSnow },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.FairyTailLuna, ActionType = ExecutorType.Activate, Description = "HAND REQUIRED: FairyTailLuna" },
                    new() { CardId = CardId.FairyTailSnow, ActionType = ExecutorType.Activate, Description = "STEP 2: Luna Effect: Search FairyTailSnow or Rochka" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.FairyTailLuna, CardId.Terraforming);
            BaitPlanner.RegisterBaitCards(CardId.Terraforming);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.FairyTailLuna);


            // โ•โ•โ• TIER 1: Reactive Negations & Hand Traps โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, ForbiddenDropletEffect);
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonEffect);

            // โ•โ•โ• TIER 2: Main Combos & Spells โ•โ•โ•
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingEffect);
            AddExecutor(ExecutorType.Activate, CardId.OnceUponAFairyTail, OnceUponAFairyTailEffect);
            AddExecutor(ExecutorType.Activate, CardId.TailsOfTheFairyTails, TailsEffect);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailTales, FairyTailTalesEffect);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailBall, FairyTailBallEffect);
            AddExecutor(ExecutorType.Activate, CardId.PairBearScare, PairBearScareEffect);
            AddExecutor(ExecutorType.Activate, CardId.TheFallenAndTheVirtuous, TheFallenAndTheVirtuousEffect);
            AddExecutor(ExecutorType.Activate, CardId.InstantFusion, InstantFusionEffect);
            AddExecutor(ExecutorType.Activate, CardId.Metamorphosis, MetamorphosisEffect);

            // โ•โ•โ• TIER 3: Monster Summons & Field Effects โ•โ•โ•
            AddExecutor(ExecutorType.SpSummon, CardId.FairyTailSnow, SnowSpSummonEffect);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailSnow, SnowFieldEffect);

            AddExecutor(ExecutorType.Summon, CardId.FairyTailLuna, LunaSummon);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailLuna, LunaEffect);

            AddExecutor(ExecutorType.Summon, CardId.FairyTailRochka, RochkaSummon);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailRochka, RochkaEffect);

            AddExecutor(ExecutorType.Summon, CardId.FairyTailSleeper, SleeperSummon);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailSleeper, SleeperEffect);

            AddExecutor(ExecutorType.Summon, CardId.FairyTailMatchgiru, MatchgiruSummon);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailMatchgiru, MatchgiruEffect);

            AddExecutor(ExecutorType.Summon, CardId.GeniaOfTheRing, GeniaSummon);
            AddExecutor(ExecutorType.Activate, CardId.GeniaOfTheRing, GeniaEffect);

            // โ•โ•โ• TIER 4: Extra Deck Summons โ•โ•โ•
            AddExecutor(ExecutorType.SpSummon, CardId.FairyTailWiccat, WiccatSummon);
            AddExecutor(ExecutorType.Activate, CardId.FairyTailWiccat);

            AddExecutor(ExecutorType.SpSummon, CardId.WeaverOfFairyTails, WeaverSummon);
            AddExecutor(ExecutorType.Activate, CardId.WeaverOfFairyTails);

            AddExecutor(ExecutorType.SpSummon, CardId.TellerOfFairyTails, TellerSummon);
            AddExecutor(ExecutorType.Activate, CardId.TellerOfFairyTails);

            AddExecutor(ExecutorType.SpSummon, CardId.MagistusChorozo, ChorozoSummon);
            AddExecutor(ExecutorType.Activate, CardId.MagistusChorozo);

            AddExecutor(ExecutorType.SpSummon, CardId.SecreterionDragon, SecreterionSummon);
            AddExecutor(ExecutorType.Activate, CardId.SecreterionDragon);

            // โ•โ•โ• Traps & Sets โ•โ•โ•
            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnSelectHand()
        {
            return true; // Go first
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _lunaSearched = false;
            _snowSummonedThisTurn = false;
            _tailsUsed = false;
            _onceUponUsed = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over setup โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
                // (Fairy Tail relies on Luna/Snow for going-second disruption)
            }
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // When selecting fusion/effect targets, prefer opponent's monsters
            if (hint == 502 || hint == 503 || hint == 504 || hint == 505 || hint == 508 || hint == 514 || hint == 551 || hint == 575 || hint == 577 || hint == 507)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count >= min)
                {
                    return enemyCards.Take(Math.Max(min, Math.Min(max, enemyCards.Count))).ToList();
                }
            }

            // For fusion material selection, prefer opponent's monsters first
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

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            return 100;
        }

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.IsCode(
                CardId.WeaverOfFairyTails,
                CardId.TellerOfFairyTails,
                CardId.SecreterionDragon,
                CardId.TheDragonThatDevoursTheDogma,
                CardId.FairyTailLuna,
                CardId.FairyTailSnow
            );
        }

        protected override bool IsBoardStrongEnough()
        {
            if (BoardScore() >= 15) return true;
            return base.IsBoardStrongEnough();
        }

        protected override bool ShouldStopExtending()
        {
            if (BoardScore() >= 20) return true;
            return base.ShouldStopExtending();
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 1: Negations & Reactive Hand Traps โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool ForbiddenDropletEffect()
        {
            if (Duel.LastChainPlayer == 1)
            {
                var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && !m.IsDisabled() && IsViableEffectTarget(m));
                if (target != null)
                {
                    // Cost selection: discard spell/trap we don't need or duplicates
                    var cost = Bot.Hand.Concat(Bot.GetSpells()).FirstOrDefault(c => c != null && c.Id != CardId.ForbiddenDroplet);
                    if (cost != null)
                    {
                        AI.SelectCard(cost);
                        AI.SelectNextCard(target);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SuperPolymerizationEffect()
        {
            if (Bot.Hand.Count < (Card.Location == CardLocation.Hand ? 2 : 1)) return false;

            var allMonsters = Bot.GetMonsters().Concat(Enemy.GetMonsters()).Where(c => c != null && c.IsFaceup()).ToList();
            if (allMonsters.Count < 2) return false;

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
                var discard = Bot.Hand.FirstOrDefault(c => c != Card);
                if (discard != null)
                {
                    AI.SelectCard(discard);
                    AI.SelectNextCard(new[] {
                        CardId.GaruraWingsOfResonantLife,
                        CardId.MudragonOfTheSwamp
                    });
                    return true;
                }
            }
            return false;
        }

        private bool BookOfMoonEffect()
        {
            if (Duel.Player == 1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle)
            {
                var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsViableEffectTarget(m) && m.IsAttack());
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 2: Spells & Field Actions โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool TerraformingEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Bot.HasInHand(CardId.OnceUponAFairyTail) || Bot.HasInSpellZone(CardId.OnceUponAFairyTail)) return false;
            AI.SelectCard(CardId.OnceUponAFairyTail);
            return true;
        }

        private bool OnceUponAFairyTailEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.OnceUponAFairyTail)) return false;
                return true;
            }

            if (Card.Location == CardLocation.SpellZone)
            {
                if (Card.IsFacedown())
                {
                    return true;
                }

                if (_onceUponUsed) return false;
                bool controlFairyTail = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                    m.IsCode(CardId.FairyTailLuna, CardId.FairyTailSnow, CardId.FairyTailRochka, CardId.FairyTailSleeper, CardId.FairyTailMatchgiru));
                if (controlFairyTail)
                {
                    _onceUponUsed = true;
                    return true;
                }
            }
            return false;
        }

        private bool TailsEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (_tailsUsed) return false;

            // Tales of Fairy Tail โ€” targets 1 monster. Pick the best target.
            // Priority: opponent's threat > our strongest monster > any valid target
            var enemyTarget = Enemy.GetMonsters()
                .Where(m => m != null && m.IsFaceup() && IsViableEffectTarget(m))
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (enemyTarget != null)
            {
                AI.SelectCard(enemyTarget);
                _tailsUsed = true;
                return true;
            }

            var ourTarget = Bot.GetMonsters()
                .Where(m => m != null && m.IsFaceup())
                .OrderByDescending(m => m.Attack)
                .FirstOrDefault();

            if (ourTarget != null && ourTarget.Attack >= 1800)
            {
                AI.SelectCard(ourTarget);
                _tailsUsed = true;
                return true;
            }

            // Don't waste the effect on weak monsters
            return false;
        }

        private bool FairyTailTalesEffect()
        {
            // Fairy Tail Tales: equip Fairy Tail from hand/Deck to Spellcaster, or GY revive
            if (ShouldSkipCombo()) return false;
            if (Card.Location == CardLocation.Hand)
            {
                // Need a Spellcaster on field and a Fairy Tail to equip
                bool hasSpellcaster = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasRace(CardRace.SpellCaster));
                bool hasFairyTail = Bot.Hand.Any(c => c != null && c != Card &&
                    c.IsCode(CardId.FairyTailLuna, CardId.FairyTailRochka, CardId.FairyTailSnow, CardId.FairyTailSleeper, CardId.FairyTailMatchgiru))
                    || Bot.Deck.Any(c => c != null &&
                        c.IsCode(CardId.FairyTailLuna, CardId.FairyTailRochka, CardId.FairyTailSnow, CardId.FairyTailSleeper, CardId.FairyTailMatchgiru));
                return hasSpellcaster && hasFairyTail;
            }
            if (Card.Location == CardLocation.Grave)
            {
                // GY banish to SS Fairy Tail โ€” best when we need a body
                bool hasDeckTarget = Bot.Deck.Any(c => c != null &&
                    c.IsCode(CardId.FairyTailLuna, CardId.FairyTailRochka, CardId.FairyTailSnow, CardId.FairyTailSleeper, CardId.FairyTailMatchgiru));
                return hasDeckTarget && Bot.GetMonsterCount() < 4;
            }
            return true; // face-up on field, activate
        }

        private bool FairyTailBallEffect()
        {
            var target = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.IsCode(CardId.FairyTailLuna, CardId.FairyTailSnow));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool PairBearScareEffect()
        {
            return true;
        }

        private bool TheFallenAndTheVirtuousEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFaceup()))
            {
                // Send Albaz monster from Extra Deck to GY to destroy a face-up card
                var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsViableEffectTarget(m))
                             ?? Enemy.GetSpells().FirstOrDefault(s => s != null && s.IsFaceup());
                if (target != null)
                {
                    AI.SelectCard(CardId.AlbionTheBrandedDragon, CardId.TheDragonThatDevoursTheDogma);
                    AI.SelectNextCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool InstantFusionEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Summons Panzer Dragon or Sea Monster of Theseus
            AI.SelectCard(CardId.PanzerDragon, CardId.SeaMonsterOfTheseus);
            return true;
        }

        private bool MetamorphosisEffect()
        {
            if (IsSpecialSummonBlocked()) return false;
            // Tribute a level 4 monster to summon Panzer Dragon or Mudragon from Extra Deck
            var mat = Bot.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Level == 4);
            if (mat != null)
            {
                AI.SelectCard(mat);
                AI.SelectNextCard(CardId.PanzerDragon, CardId.MudragonOfTheSwamp);
                return true;
            }
            return false;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 3: Monster Summons & Disruptions โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool SnowSpSummonEffect()
        {
            if (Card.Location != CardLocation.Grave) return false;
            if (_snowSummonedThisTurn) return false;

            bool oppTurnPlay = Duel.Player == 1 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.Main2);
            bool endPhasePlay = Duel.Phase == DuelPhase.End;
            bool weNeedLethal = Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) && Bot.GetMonsterCount() < 4;

            if (oppTurnPlay || endPhasePlay || weNeedLethal)
            {
                var gyPool = Bot.Graveyard.Where(c => c != null && c != Card)
                    .OrderBy(c => {
                        if (c.IsCode(CardId.TailsOfTheFairyTails)) return 100;
                        if (c.IsCode(CardId.OnceUponAFairyTail)) return -10;
                        if (c.IsCode(CardId.FairyTailSnow)) return 50;
                        if (c.IsMonster()) return 5;
                        return 10;
                    })
                    .ToList();
                var handPool = Bot.Hand.Where(c => c != null && c != Card).ToList();
                var banishPool = gyPool.Concat(handPool).ToList();

                if (banishPool.Count >= 7)
                {
                    AI.SelectCard(banishPool.Take(7).ToList());
                    _snowSummonedThisTurn = true;
                    return true;
                }
            }
            return false;
        }

        private bool SnowFieldEffect()
        {
            if (Card.Location != CardLocation.MonsterZone) return false;

            var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && IsViableEffectTarget(m));
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool LunaSummon()
        {
            if (ShouldSkipCombo()) return false;
            // Only summon Luna if we can use her search effect
            return !Bot.HasInHand(CardId.FairyTailSnow) && !Bot.HasInMonstersZone(CardId.FairyTailSnow)
                || !Bot.HasInHand(CardId.FairyTailRochka) && !Bot.HasInMonstersZone(CardId.FairyTailRochka);
        }

        private bool LunaEffect()
        {
            if (Card.Location == CardLocation.MonsterZone)
            {
                if (ActivateDescription == Util.GetStringId(CardId.FairyTailLuna, 0))
                {
                    if (!_lunaSearched)
                    {
                        AI.SelectCard(CardId.FairyTailSnow, CardId.FairyTailRochka, CardId.FairyTailMatchgiru);
                        _lunaSearched = true;
                        return true;
                    }
                }
                else
                {
                    var target = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && (m.IsExtraCard() || m.IsSpecialSummoned) && IsViableEffectTarget(m))
                        ?? Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Attack >= 1500 && IsViableEffectTarget(m));
                    if (target != null)
                    {
                        AI.SelectCard(target);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool RochkaSummon()
        {
            return true;
        }

        private bool RochkaEffect()
        {
            return true;
        }

        private bool SleeperSummon()
        {
            return true;
        }

        private bool SleeperEffect()
        {
            return true;
        }

        private bool MatchgiruSummon()
        {
            if (ShouldSkipCombo()) return false;
            // Only summon Matchlille if we can use her search effect (need targets in deck)
            bool hasSearchTarget = GetRemainingCount(CardId.FairyTailLuna) > 0
                || GetRemainingCount(CardId.FairyTailRochka) > 0
                || GetRemainingCount(CardId.FairyTailSleeper) > 0
                || GetRemainingCount(CardId.FairyTailSnow) > 0;
            return hasSearchTarget;
        }

        private bool MatchgiruEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            // Search priority: Luna (starter) > Snow (disruption) > Rochka > Sleeper
            if (!Bot.HasInHand(CardId.FairyTailLuna) && !Bot.HasInMonstersZone(CardId.FairyTailLuna) && GetRemainingCount(CardId.FairyTailLuna) > 0)
            {
                AI.SelectCard(CardId.FairyTailLuna);
                return true;
            }
            if (!Bot.HasInHand(CardId.FairyTailSnow) && !Bot.HasInMonstersZone(CardId.FairyTailSnow) && GetRemainingCount(CardId.FairyTailSnow) > 0)
            {
                AI.SelectCard(CardId.FairyTailSnow);
                return true;
            }
            if (GetRemainingCount(CardId.FairyTailRochka) > 0)
            {
                AI.SelectCard(CardId.FairyTailRochka);
                return true;
            }
            if (GetRemainingCount(CardId.FairyTailSleeper) > 0)
            {
                AI.SelectCard(CardId.FairyTailSleeper);
                return true;
            }
            return false;
        }

        private bool GeniaSummon()
        {
            return true;
        }

        private bool GeniaEffect()
        {
            return true;
        }

        // โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ• TIER 4: Extra Deck Summons โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•โ•

        private bool WiccatSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool WeaverSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool TellerSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool ChorozoSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        private bool SecreterionSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }
    }
}
