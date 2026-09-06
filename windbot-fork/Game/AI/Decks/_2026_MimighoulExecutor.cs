// ============================================================
// CARD AUDIT โ€” 2026 Mimighoul
// ============================================================
// | Card Name              | Type       | OPT? | Cost        | Effect Summary                         | Activate When                     | NEVER When                           |
// |------------------------|------------|------|-------------|----------------------------------------|-----------------------------------|--------------------------------------|
// | Mimighoul Master       | Monster    | HOPT | None        | Flip-summon engine, search any MGH card| Need setup, have flip targets     | Already used                         |
// | Mimighoul Archfiend    | Flip       | No   | None        | Flip: control opponent monster         | Opponent controls monster         | No enemy monster                     |
// | Mimighoul Armor        | Flip       | No   | None        | Flip: equip to enemy, negate attacks   | Opponent attacks                  | Enemy has no monster                 |
// | Mimighoul Cerberus     | Flip       | No   | None        | Flip: burn + banish                    | Opponent controls monster         | Opponent no monster                  |
// | Mimighoul Dragon       | Flip       | No   | None        | Flip: destroy card + burn              | Opponent has cards                | Opponent empty field                 |
// | Mimighoul Fairy        | Flip       | No   | None        | Flip: gain LP, draw                    | Grind game                        | Not in grind                         |
// | Mimighoul Flower       | Flip       | No   | None        | Flip: SS any MGH from hand/deck        | Need board presence               | SS blocked                           |
// | Mimighoul Slime        | Flip       | No   | None        | Flip: copy any MGH flip effect         | Have target MGH on field          | No target                            |
// | Mimighoul Maker        | Spell      | HOPT | None        | SS 1 MGH from deck, set 1 MGH from deck| Turn 1 setup                      | SS blocked                           |
// | Mimighoul Dungeon      | Field      | No   | None        | Lock opponent normal summons, search   | Setup turn                        | Already active                       |
// | Mimighoul Fork         | Spell      | HOPT | None        | Pop 1, search MGH card                 | Need removal or search            | No target                            |
// | Mimighoul Charm        | Continuous | No   | None        | Protect from battle, draw when flipped | Setup phase                       | Not during BP                        |
// | Mimighoul Room         | Normal Trap| HOPT | None        | Flip 1 MGH + search                    | Opponent turn, have set MGH       | No set MGH                           |
// | Giant Mimighoul        | Xyz (R1)   | -    | Detach 1    | Steal opp monster + attach as material | Opponent has monster              | No monster to steal                  |
// | Mimighoul Throne       | Xyz (R1)   | -    | Detach 1    | Negate opp monster effect + destroy    | Opponent activates monster effect | Own chain                            |
// ============================================================
// ACE CARDS:
//   Primary: Mimighoul Throne (42940335) โ€” negate
//   Secondary: Giant Mimighoul (16955631) โ€” steal
// COMBO STARTERS:
//   1. Mimighoul Maker (76127184) โ€” SS from deck + set
//   2. Mimighoul Master (55537983) โ€” search + flip
// WIN CONDITION: Xyz control + flip disruption
// GOING 1ST END BOARD: Dungeon + set Room + set MGH = 2 disruptions
// GOING 2ND GAMEPLAN: Book flip โ’ steal โ’ beatdown
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
    [Deck("2026_Mimighoul", "2026_Mimighoul")]
    public class _2026_MimighoulExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck
            public const int Master = 55537983;
            public const int Archfiend = 50415441;
            public const int Armor = 11677278;
            public const int Cerberus = 23920796;
            public const int Dragon = 81522098;
            public const int Fairy = 43066927;
            public const int Flower = 82933935;
            public const int Slime = 80551022;

            // Spells/Traps
            public const int Maker = 13204145;
            public const int Dungeon = 86809440;
            public const int Fork = 19338434;
            public const int Charm = 55733143;
            public const int Room = 59293853;

            // Extra Deck
            public const int GiantMimighoul = 16955631;
            public const int Throne = 42940335;

            // Staples
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int CalledByTheGrave = 24224830;
            public const int BookOfMoon = 14087893;
            public const int BookOfEclipse = 35480699;
            public const int SPLittleKnight = 29301450;
            public const int AccesscodeTalker = 86066372;
            public const int Garura = 11765832;
        }

        private static readonly int[] AceCardIds = {
            CardId.Throne, CardId.GiantMimighoul
        };

        private static readonly int[] MimighoulMonsters = {
            CardId.Master, CardId.Archfiend, CardId.Armor,
            CardId.Cerberus, CardId.Dragon, CardId.Fairy,
            CardId.Flower, CardId.Slime
        };

        // OPT Flags
        private bool _masterUsed = false;
        private bool _makerUsed = false;
        private bool _forkUsed = false;
        private bool _roomUsed = false;

        public _2026_MimighoulExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // Register Ace Cards to protect them from being used suboptimally
            ResourcePlan.RegisterAceCards(AceCardIds);

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Maker-Dungeon",
                RequiredCards = new List<int> { CardId.Maker, CardId.Dungeon },
                FallbackLineName = "Mimighoul-Master-Fallback",
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Maker, ActionType = ExecutorType.Activate, Description = "SS MGH from deck" },
                    new() { CardId = CardId.Dungeon, ActionType = ExecutorType.Activate, Description = "Set Dungeon field" }
                },
                EndBoardScore = 75
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Mimighoul-Master-Fallback",
                RequiredCards = new List<int> { CardId.Master },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Master, ActionType = ExecutorType.Summon, Description = "Summon Master as fallback starter" }
                },
                EndBoardScore = 60,
                Condition = () => Bot.Hand.Any(c => c != null && c.Id == CardId.Master)
            });

            BaitPlanner.RegisterComboStarters(CardId.Maker, CardId.Master);
            BaitPlanner.RegisterBaitCards(CardId.BookOfMoon);
            ChainAdvisor.RegisterHighValueTargets(CardId.Maker, CardId.Dungeon, CardId.Fork);

            // TIER 1: Hand Traps
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCActivate);

            // TIER 2: Quick Effects (Throne negate, GiantMimighoul steal)
            AddExecutor(ExecutorType.Activate, CardId.Throne, ThroneNegate);
            AddExecutor(ExecutorType.Activate, CardId.GiantMimighoul, GiantMimighoulEffect);

            // TIER 3: Protection
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);

            // TIER 4: Field / Continuous
            AddExecutor(ExecutorType.Activate, CardId.Dungeon, DungeonActivate);
            AddExecutor(ExecutorType.Activate, CardId.Charm, CharmActivate);
            AddExecutor(ExecutorType.Activate, CardId.Maker, MakerActivate);

            // TIER 5: Spells
            AddExecutor(ExecutorType.Activate, CardId.Fork, ForkActivate);
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookActivate);
            AddExecutor(ExecutorType.Activate, CardId.BookOfEclipse, BookEclipseActivate);

            // TIER 6: Monster Effects
            AddExecutor(ExecutorType.Activate, CardId.Master, MasterActivate);
            AddExecutor(ExecutorType.Activate, CardId.Flower, FlowerFlip);
            AddExecutor(ExecutorType.Activate, CardId.Slime, SlimeFlip);
            AddExecutor(ExecutorType.Activate, CardId.Archfiend, ArchfiendFlip);
            AddExecutor(ExecutorType.Activate, CardId.Cerberus, CerberusFlip);
            AddExecutor(ExecutorType.Activate, CardId.Dragon, DragonFlip);
            AddExecutor(ExecutorType.Activate, CardId.Fairy, FairyFlip);
            AddExecutor(ExecutorType.Activate, CardId.Armor, ArmorFlip);

            // TIER 7: Trap
            AddExecutor(ExecutorType.Activate, CardId.Room, RoomActivate);

            // TIER 8: Extra Deck
            AddExecutor(ExecutorType.SpSummon, CardId.GiantMimighoul, XyzSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.Throne, XyzSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPSummon);

            // TIER 9: Sets
            AddExecutor(ExecutorType.SpellSet, CardId.Room, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.BookOfEclipse, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.BookOfMoon, () => Util.IsTurn1OrMain2());

            // LAST: Normal Summon/Set monsters + Repos
            AddExecutor(ExecutorType.Summon, CardId.Master, NormalSummonCondition);
            AddExecutor(ExecutorType.Summon, CardId.Flower, NormalSummonCondition);
            AddExecutor(ExecutorType.Summon, CardId.Fairy, NormalSummonCondition);
            AddExecutor(ExecutorType.MonsterSet, CardId.Archfiend, SetSummonCondition);
            AddExecutor(ExecutorType.MonsterSet, CardId.Armor, SetSummonCondition);
            AddExecutor(ExecutorType.MonsterSet, CardId.Cerberus, SetSummonCondition);
            AddExecutor(ExecutorType.MonsterSet, CardId.Dragon, SetSummonCondition);
            AddExecutor(ExecutorType.MonsterSet, CardId.Slime, SetSummonCondition);

            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand() => true; // Go first to set up Dungeon

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _masterUsed = false;
            _makerUsed = false;
            _forkUsed = false;
            _roomUsed = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        public override bool ShouldAllowSpSummon(ClientCard card)
        {
            if (!base.ShouldAllowSpSummon(card)) return false;
            if (card == null) return true;

            if (card.HasType(CardType.Link | CardType.Xyz))
            {
                var activeAces = Bot.GetMonsters().Where(c => c != null && c.IsFaceup() && IsAceCard(c)).ToList();
                if (activeAces.Count > 0)
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
                        DecisionTracer.TraceSkip("ShouldAllowSpSummon", $"Summoning {card.Name} is not safe (would consume Ace card(s))");
                        return false;
                    }
                }
            }
            return true;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.Master) _masterUsed = true;
                if (card.Id == CardId.Maker) _makerUsed = true;
                if (card.Id == CardId.Fork) _forkUsed = true;
                if (card.Id == CardId.Room) _roomUsed = true;
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
            if (c.IsCode(CardId.AshBlossom, CardId.MaxxC))
                return 800;
            if (MimighoulMonsters.Contains(c.Id))
                return 600;
            return 100;
        }

        protected override bool IsBoardStrongEnough()
        {
            if (Bot.HasInMonstersZone(CardId.Throne) || Bot.HasInMonstersZone(CardId.GiantMimighoul))
                return true;
            if (Bot.HasInSpellZone(CardId.Dungeon) && Bot.GetSpells().Count(c => c != null && c.IsFacedown()) >= 2)
                return true;
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

        // โ•โ•โ• Quick Effects โ•โ•โ•

        private bool ThroneNegate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (!Card.HasXyzMaterial()) return false;
            if (LastChainCard == null || LastChainCard.Controller == 0) return false;
            if (!LastChainCard.IsMonster()) return false;

            AI.SelectCard(LastChainCard);
            DecisionTracer.TraceActivate("ThroneNegate", $"Negate {LastChainCard.Name}");
            return true;
        }

        private bool GiantMimighoulEffect()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;
            if (!Card.HasXyzMaterial()) return false;

            // Steal opponent monster
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsTargetImmune(c));
            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("GiantMimighoulEffect", $"Steal {target.Name}");
                return true;
            }
            return false;
        }

        // โ•โ•โ• Spells โ•โ•โ•

        private bool DungeonActivate()
        {
            if (Bot.HasInSpellZone(CardId.Dungeon)) return false;
            if (ShouldSkipCombo()) return false;
            return true;
        }

        private bool CharmActivate()
        {
            if (ShouldSkipCombo()) return false;
            if (Card.Location != CardLocation.Hand) return false;
            if (Bot.HasInSpellZone(CardId.Charm)) return false;
            return true;
        }

        private bool MakerActivate()
        {
            if (_makerUsed) return false;
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.Hand) return false;

            // Need target in deck for both SS and set
            bool canSS = GetRemainingCount(CardId.Archfiend) > 0 || GetRemainingCount(CardId.Cerberus) > 0
                || GetRemainingCount(CardId.Dragon) > 0;
            bool canSet = GetRemainingCount(CardId.Flower) > 0 || GetRemainingCount(CardId.Slime) > 0
                || GetRemainingCount(CardId.Fairy) > 0 || GetRemainingCount(CardId.Armor) > 0;

            if (!canSS || !canSet) return false;

            _makerUsed = true;
            DecisionTracer.TraceActivate("MakerActivate", "SS MGH from deck + set 1");
            return true;
        }

        private bool ForkActivate()
        {
            if (_forkUsed) return false;
            if (ShouldSkipCombo()) return false;

            if (Card.Location == CardLocation.Hand)
            {
                // Priority 1: Chain to opponent's activation to destroy a threat
                if (Duel.Player == 1 && LastChainCard != null)
                {
                    var chainTarget = Enemy.GetMonsters()
                        .FirstOrDefault(c => c != null && c.IsFaceup() && !IsTargetImmune(c));
                    if (chainTarget != null)
                    {
                        _forkUsed = true;
                        AI.SelectCard(chainTarget);
                        DecisionTracer.TraceActivate("ForkActivate", $"Chain destroy {chainTarget.Name}");
                        return true;
                    }
                }

                // Priority 2: Need search for Maker
                bool needsSearch = !Bot.HasInHand(CardId.Maker) && GetRemainingCount(CardId.Maker) > 0
                    && !Bot.HasInSpellZone(CardId.Maker);

                // Priority 3: Has target to destroy
                bool hasTarget = Enemy.GetSpells().Any(c => c != null && c.IsFaceup()) ||
                                Enemy.GetMonsters().Any(c => c != null && c.IsFaceup());

                if (needsSearch || hasTarget)
                {
                    _forkUsed = true;
                    // Select target to destroy
                    var destroyTarget = Enemy.GetMonsters()
                        .Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c))
                        .OrderByDescending(c => c.Attack)
                        .FirstOrDefault();
                    if (destroyTarget == null)
                        destroyTarget = Enemy.GetSpells()
                            .Where(c => c != null && c.IsFaceup())
                            .FirstOrDefault();
                    if (destroyTarget != null)
                        AI.SelectCard(destroyTarget);
                    DecisionTracer.TraceActivate("ForkActivate", "Fork for search/removal");
                    return true;
                }
            }
            return false;
        }

        private bool BookActivate()
        {
            if (Card.Location == CardLocation.Hand)
            {
                // Strategy 1: Flip our own facedown MGH to re-use flip effect + setup Xyz
                if (Duel.Player == 0 && Duel.Phase == DuelPhase.Main1)
                {
                    var ownFlipTarget = Bot.GetMonsters()
                        .FirstOrDefault(c => c != null && c.IsFacedown() && MimighoulMonsters.Contains(c.Id));
                    if (ownFlipTarget != null && Bot.GetMonsters().Count(c => c != null && c.IsFaceup()) >= 1)
                    {
                        AI.SelectCard(ownFlipTarget);
                        DecisionTracer.TraceActivate("BookActivate", $"Flip own {ownFlipTarget.Name} for flip effect");
                        return true;
                    }
                }

                // Strategy 2: Flip opponent monster face-down for disruption
                var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsTargetImmune(c));
                if (target != null)
                {
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("BookActivate", $"Flip enemy {target.Name}");
                    return true;
                }
            }
            return false;
        }

        private bool BookEclipseActivate()
        {
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                // Activate during opponent's End Phase to flip all face-up monsters face-down
                // This can disrupt opponent's strategy and save our monsters
                if (Duel.Player == 1 && Duel.Phase == DuelPhase.End &&
                    Enemy.GetMonsters().Any(c => c != null && c.IsFaceup()))
                {
                    DecisionTracer.TraceActivate("BookEclipseActivate", "End Phase flip all");
                    return true;
                }
            }
            return false;
        }

        // โ•โ•โ• Monster Effects โ•โ•โ•

        private bool MasterActivate()
        {
            if (_masterUsed) return false;
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            _masterUsed = true;
            DecisionTracer.TraceActivate("MasterActivate", "Search MGH card");
            return true;
        }

        private bool FlowerFlip()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown() || Card.IsDisabled()) return false;
            if (IsSpecialSummonBlocked()) return false;

            // SS Mimighoul from deck
            if (GetRemainingCount(CardId.Master) > 0 || GetRemainingCount(CardId.Archfiend) > 0)
            {
                DecisionTracer.TraceActivate("FlowerFlip", "SS MGH from deck");
                return true;
            }
            return false;
        }

        private bool SlimeFlip()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown() || Card.IsDisabled()) return false;

            // Copy another MGH flip effect โ€” prefer Archfiend for control
            if (Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.Archfiend))
            {
                DecisionTracer.TraceActivate("SlimeFlip", "Copy Archfiend");
                return true;
            }
            return false;
        }

        private bool ArchfiendFlip()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown() || Card.IsDisabled()) return false;

            // Control opponent monster
            var target = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !IsTargetImmune(c));
            if (target != null)
            {
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("ArchfiendFlip", $"Steal {target.Name}");
                return true;
            }
            return false;
        }

        private bool CerberusFlip()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown() || Card.IsDisabled()) return false;
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup());
        }

        private bool DragonFlip()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown() || Card.IsDisabled()) return false;
            return Enemy.GetSpells().Any(c => c != null && c.IsFaceup()) ||
                   Enemy.GetMonsters().Any(c => c != null && c.IsFaceup());
        }

        private bool FairyFlip()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown() || Card.IsDisabled()) return false;
            return IsInGrindGame();
        }

        private bool ArmorFlip()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsFacedown() || Card.IsDisabled()) return false;
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup());
        }

        // โ•โ•โ• Trap โ•โ•โ•

        private bool RoomActivate()
        {
            if (_roomUsed) return false;
            if (Card.Location != CardLocation.SpellZone || Card.IsFacedown()) return false;

            // Need a facedown MGH to flip face-up
            bool hasFlipedMGH = Bot.GetMonsters().Any(c => c != null && c.IsFacedown() && MimighoulMonsters.Contains(c.Id));
            if (!hasFlipedMGH) return false;

            // Activate during opponent's turn when they have a monster we could steal
            if (Duel.Player == 1)
            {
                DecisionTracer.TraceActivate("RoomActivate", "Flip MGH on opp turn");
                return true;
            }
            return false;
        }

        // โ•โ•โ• Summon Conditions โ•โ•โ•

        private bool NormalSummonCondition()
        {
            if (ShouldSkipCombo()) return false;
            return true;
        }

        private bool SetSummonCondition()
        {
            if (ShouldSkipCombo()) return false;

            // Set MGH flip monsters face-down โ€” allow on Turn 1 MP1 too
            // (EDOPro won't advance to MP2 if all actions are sets)
            return Duel.Turn == 1 || Util.IsTurn1OrMain2();
        }

        private bool XyzSummonCondition()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;

            int faceupMonsters = Bot.GetMonsters().Count(c => c != null && c.IsFaceup());
            return faceupMonsters >= 2;
        }

        private bool SPSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (ShouldSkipCombo()) return false;
            return Enemy.GetMonsters().Any(c => c != null && c.IsFaceup());
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
            if (hint == 511 || hint == 513 || hint == 533 || hint == 502 || hint == 504)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            if (hint == 506) // Search
            {
                return SelectPreferred(cards, min, max,
                    CardId.Maker, CardId.Dungeon, CardId.Master,
                    CardId.Fork, CardId.Archfiend, CardId.Room);
            }

            if (Card == null) return base.OnSelectCard(cards, min, max, hint, cancelable);

            if (Card.Id == CardId.Maker)
            {
                // Maker has two selection steps:
                // Step 1: SS from deck (hint 509) โ€” prefer Archfiend > Cerberus > Dragon > Flower
                if (hint == 509)
                {
                    return SelectPreferred(cards, min, max,
                        CardId.Archfiend, CardId.Cerberus, CardId.Dragon,
                        CardId.Flower, CardId.Fairy);
                }
                // Step 2: Set from deck (hint hints for set is often generic) โ€” prefer Flower > Slime > Armor > Fairy
                if (hint == 502 || hint == 504)
                {
                    return SelectPreferred(cards, min, max,
                        CardId.Flower, CardId.Slime, CardId.Armor, CardId.Fairy);
                }
            }

            if (Card.Id == CardId.Fork && hint == 506)
            {
                // After destroying with Fork, search for a MGH card
                return SelectPreferred(cards, min, max,
                    CardId.Maker, CardId.Dungeon, CardId.Master,
                    CardId.Room, CardId.Archfiend);
            }

            // Fork discard cost (if Fork requires discard)
            if (Card.Id == CardId.Fork && hint == 502)
            {
                return cards.OrderBy(c => GetMaterialPriority(c)).Take(max).ToList();
            }

            // Priority for summon materials
            if (hint == 509 && (Card.Id == CardId.GiantMimighoul || Card.Id == CardId.Throne))
            {
                // For Xyz summon: prefer using non-Mimighoul monsters as materials first
                return cards.OrderBy(c => GetMaterialPriority(c)).Take(max).ToList();
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (AceCardIds.Contains(cardId) || cardId == CardId.Master)
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }
            if (MimighoulMonsters.Contains(cardId) && cardId != CardId.Master)
            {
                if (positions.Contains(CardPosition.FaceDownDefence))
                    return CardPosition.FaceDownDefence;
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
