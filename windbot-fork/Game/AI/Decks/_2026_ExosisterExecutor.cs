// ============================================================
// CARD AUDIT โ€” 2026 Exosister
// ============================================================
// | Card Name              | Type       | OPT? | Cost        | Effect Summary                         | Activate When                     | NEVER When                           |
// |------------------------|------------|------|-------------|----------------------------------------|-----------------------------------|--------------------------------------|
// | Exosister Martha       | Monster    | HOPT | None        | SS self + SS Elis from Deck            | Need 1-card Xyz starter           | Non-Exosister SS lock / SS blocked   |
// | Exosister Elis         | Monster    | HOPT | None        | SS from hand if control Exosister      | Control Exosister, need extender  | Hand SS blocked / No Exosister on bd |
// | Exosister Stella       | Monster    | HOPT | None        | SS 1 Exosister monster from hand       | Have Exosister in hand to extend  | No Exosister in hand to SS           |
// | Exosister Sophia       | Monster    | HOPT | None        | Draw 1 card if control Irene           | Control Irene on field            | No Irene on field                    |
// | Exosister Irene        | Monster    | HOPT | Bottom deck | Place 1 Exo card from hand, draw 1     | Have redundant Exo card in hand   | Hand empty / only critical cards     |
// | Exosister Pax          | Quick-Play | HOPT | Pay 800 LP  | Search Exo card; SS if partner present | Need starter/extender/disruption  | Already used this turn / LP <= 800   |
// | Exosister Arment       | Quick-Play | HOPT | Pay 800 LP  | Rank-up target Exo monster into Exo Xyz| Need instant Xyz / dodge removal  | No Exo monster on field              |
// | Exosister Betrayal     | Continuous | HOPT | Discard 1   | Search 2 Exo monsters, NS mentioned    | Starter / Board extension         | Already used / Hand count < 1        |
// | Exosister Carpedivem   | Continuous | No   | None        | Declare card name lock / pop S/T battle| MP1 setup / Battle phase          | Field spell/cont zone full           |
// | Exosister Returnia     | Normal Trap| HOPT | Pay 800 LP  | Banish 1 card; 2nd banish if Xyz sum.  | Opponent threat / field control   | Control non-Exosister / No Exo on bd |
// | Exosister Vadis        | Normal Trap| HOPT | Pay 800 LP  | SS 2 paired Exosisters from Deck       | Opponent turn setup / GY reaction | SS blocked / Less than 2 targets     |
// | Exosister Mikailis     | Xyz (R4)   | HOPT | Detach 1    | Detach to search S/T; Quick Banish 1   | Search Pax/Returnia / Banish threat| No material / Already used effect    |
// | Exosister Kaspitell    | Xyz (R4)   | HOPT | Detach 1    | Detach to search Monster; Lock GY SS   | Search Martha/Stella / Anti-GY SS | No material / Already used effect    |
// | Exosister Gibrine      | Xyz (R4)   | HOPT | Detach 1    | Quick Negate monster; +800 ATK to Xyz  | Enemy monster threat / Push lethal| No material / Target immune          |
// | Exosister Karmael      | Xyz (R4)   | HOPT | Detach all  | Quick Rewrite enemy effect / Set S/T EP| Enemy activates effect / End Phase| No material                          |
// | Exosister Asophiel     | Xyz (R4)   | HOPT | Detach 1    | Detach to bounce monster; Lock GY eff  | Threat monster on field / Anti-GY | No material / Target immune          |
// | Exosisters Magnifica   | Xyz (R8)   | HOPT | Detach 1    | Quick Banish 1; Quick Tag-Out to R4    | Enemy threat / Chain Tag-Out      | No material                          |
// ============================================================
// ACE CARDS:
//   Primary Boss: Exosisters Magnifica (59242457) โ€” 2800 ATK x 2 attacks, Quick Banish 1, Quick Tag-Out
//   Secondary Bosses: Exosister Mikailis (42741437), Exosister Kaspitell (78135071), Exosister Gibrine (5530780)
// COMBO STARTERS:
//   1. Exosister Martha (37343995) โ€” 1-card combo (SS Martha + SS Elis from Deck)
//   2. Exosister Pax (77913594) โ€” Searches Martha or Returnia
//   3. Exosister Betrayal (52335937) โ€” Searches 2 Exosisters + Normal Summons
// WIN CONDITION: Xyz banish lock, GY lockdown, double-attacking Magnifica OTK (5600+ DMG)
// GOING 1ST END BOARD: Magnifica (2 Mats) + Set Returnia (2 Banishes) + Set Vadis (2 Sisters -> Xyz) + Hand Traps (4-6 Interactions)
// GOING 2ND GAMEPLAN: Board break via Super Poly / Droplet -> Mikailis Banish -> Asophiel Bounce -> Magnifica Double Strike OTK
// CHOKEPOINTS: Martha activation -> Ash Blossom / SS Block
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
    [Deck("2026_Exosister", "2026_Exosister")]
    public class _2026_ExosisterExecutor : ModernExecutor
    {
        public class CardId
        {
            // Main Deck Monsters
            public const int Martha = 37343995;
            public const int Elis = 16474916;
            public const int Sophia = 5352328;
            public const int Stella = 43863925;
            public const int Irene = 79858629;

            // Spells & Traps
            public const int Pax = 77913594;
            public const int Arment = 4408198;
            public const int Betrayal = 52335937;
            public const int Carpedivem = 30802207;
            public const int Returnia = 197042;
            public const int Vadis = 77891946;

            // Extra Deck
            public const int Mikailis = 42741437;
            public const int Kaspitell = 78135071;
            public const int Gibrine = 5530780;
            public const int Karmael = 77675029;
            public const int Asophiel = 41524885;
            public const int Magnifica = 59242457;
            public const int PanzerDragon = 72959823;
            public const int Mudragon = 54757758;
            public const int Garura = 11765832;
            public const int SPLittleKnight = 29301450;

            // Staples
            public const int AshBlossom = 14558127;
            public const int MaxxC = 23434538;
            public const int MulcharmyFuwalos = 42141493;
            public const int InfiniteImpermanence = 10045474;
            public const int EffectVeiler = 97268402;
            public const int CalledByTheGrave = 24224830;
            public const int ForbiddenDroplet = 24299458;
            public const int SuperPolymerization = 48130397;
            public const int BookOfMoon = 14087893;
        }

        // Ace Cards
        private static readonly int[] AceCardIds = {
            CardId.Magnifica, CardId.Mikailis, CardId.Kaspitell,
            CardId.Karmael, CardId.Gibrine, CardId.Asophiel
        };

        // Exosister Main Deck monsters
        private static readonly int[] ExosisterMonsters = {
            CardId.Martha, CardId.Elis, CardId.Sophia,
            CardId.Stella, CardId.Irene
        };

        // Rank 4 Exosister Xyz Monsters
        private static readonly int[] Rank4ExosisterXyz = {
            CardId.Mikailis, CardId.Kaspitell, CardId.Gibrine,
            CardId.Karmael, CardId.Asophiel
        };

        // Granular OPT Flags
        private bool _marthaHandUsed = false;
        private bool _marthaGyTriggerUsed = false;
        private bool _elisHandUsed = false;
        private bool _elisGyTriggerUsed = false;
        private bool _stellaHandUsed = false;
        private bool _stellaGyTriggerUsed = false;
        private bool _sophiaDrawUsed = false;
        private bool _sophiaGyTriggerUsed = false;
        private bool _ireneCycleUsed = false;
        private bool _ireneGyTriggerUsed = false;
        private bool _paxUsed = false;
        private bool _armentUsed = false;
        private bool _betrayalUsed = false;
        private bool _returniaUsed = false;
        private bool _vadisUsed = false;
        private bool _mikailisSearchUsed = false;
        private bool _mikailisBanishUsed = false;
        private bool _kaspitellSearchUsed = false;
        private bool _kaspitellLockUsed = false;
        private bool _gibrineNegateUsed = false;
        private bool _gibrineBoostUsed = false;
        private bool _karmaelRewriteUsed = false;
        private bool _asophielBounceUsed = false;
        private bool _asophielLockUsed = false;
        private bool _magnificaBanishUsed = false;
        private bool _magnificaTagOutUsed = false;
        private bool _xyzSummonedThisTurn = false;

        public _2026_ExosisterExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            ResourcePlan.RegisterAceCards(AceCardIds);
            HeuristicGuard.RegisterAceCards(AceCardIds);

            // โ”€โ”€ Combo Lines โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Martha-1Card-Starter",
                RequiredCards = new List<int> { CardId.Martha },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Martha, ActionType = ExecutorType.Activate, Description = "SS Martha + Elis from Deck" },
                    new() { CardId = CardId.Kaspitell, ActionType = ExecutorType.SpSummon, Description = "Xyz Kaspitell" },
                    new() { CardId = CardId.Kaspitell, ActionType = ExecutorType.Activate, Description = "Search Stella/Extender" },
                    new() { CardId = CardId.Mikailis, ActionType = ExecutorType.SpSummon, Description = "Xyz Mikailis" },
                    new() { CardId = CardId.Mikailis, ActionType = ExecutorType.Activate, Description = "Search Returnia/Vadis" },
                    new() { CardId = CardId.Magnifica, ActionType = ExecutorType.SpSummon, Description = "Xyz Magnifica overlay" }
                },
                EndBoardScore = 95
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Pax-Starter",
                RequiredCards = new List<int> { CardId.Pax },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Pax, ActionType = ExecutorType.Activate, Description = "Search Martha via Pax" },
                    new() { CardId = CardId.Martha, ActionType = ExecutorType.Activate, Description = "Execute Martha line" }
                },
                EndBoardScore = 90
            });

            ComboRouter.RegisterLine(new ComboRouter.ComboLine
            {
                Name = "Betrayal-Starter",
                RequiredCards = new List<int> { CardId.Betrayal },
                Steps = new List<ComboRouter.ComboStep>
                {
                    new() { CardId = CardId.Betrayal, ActionType = ExecutorType.Activate, Description = "Add 2 Exosisters, discard 1" },
                    new() { CardId = CardId.Martha, ActionType = ExecutorType.Activate, Description = "SS Martha + Elis" }
                },
                EndBoardScore = 88
            });

            BaitPlanner.RegisterComboStarters(CardId.Martha, CardId.Pax, CardId.Betrayal);
            BaitPlanner.RegisterBaitCards(CardId.BookOfMoon, CardId.ForbiddenDroplet);
            ChainAdvisor.RegisterHighValueTargets(CardId.Magnifica, CardId.Mikailis, CardId.Martha, CardId.Pax);

            // ==========================================
            // EXECUTOR REGISTRATION (Strategic Priority)
            // ==========================================

            // TIER 1: Hand Traps & Fast Interrupts
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, AshActivate);
            AddExecutor(ExecutorType.Activate, CardId.MulcharmyFuwalos, MulcharmyFuwalosActivate);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceActivate);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, MaxxCActivate);
            AddExecutor(ExecutorType.Activate, CardId.EffectVeiler, VeilerActivate);
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);

            // TIER 2: Board Breakers & Quick Spells
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolyActivate);
            AddExecutor(ExecutorType.Activate, CardId.ForbiddenDroplet, DropletActivate);
            AddExecutor(ExecutorType.Activate, CardId.BookOfMoon, BookOfMoonActivate);
            AddExecutor(ExecutorType.Activate, CardId.Garura, () => true);
            AddExecutor(ExecutorType.Activate, CardId.PanzerDragon, () => true);
            AddExecutor(ExecutorType.Activate, CardId.Mudragon, () => Enemy.GetMonsterCount() > 0);

            // TIER 3: Boss Monster Quick Effects & Tag-Outs
            AddExecutor(ExecutorType.Activate, CardId.Magnifica, MagnificaActivate);
            AddExecutor(ExecutorType.Activate, CardId.Mikailis, MikailisActivate);
            AddExecutor(ExecutorType.Activate, CardId.Karmael, KarmaelActivate);
            AddExecutor(ExecutorType.Activate, CardId.Gibrine, GibrineActivate);
            AddExecutor(ExecutorType.Activate, CardId.Asophiel, AsophielActivate);
            AddExecutor(ExecutorType.Activate, CardId.Kaspitell, KaspitellActivate);

            // TIER 4: Traps (Disruptions & Reaction Transforms)
            AddExecutor(ExecutorType.Activate, CardId.Returnia, ReturniaActivate);
            AddExecutor(ExecutorType.Activate, CardId.Vadis, VadisActivate);

            // TIER 5: Searchers & Starters (Pax, Betrayal, Carpedivem, Arment)
            AddExecutor(ExecutorType.Activate, CardId.Pax, PaxActivate);
            AddExecutor(ExecutorType.Activate, CardId.Betrayal, BetrayalActivate);
            AddExecutor(ExecutorType.Activate, CardId.Carpedivem, CarpedivemActivate);
            AddExecutor(ExecutorType.Activate, CardId.Arment, ArmentActivate);

            // TIER 6: Main Deck Monster Effects (Martha SS, Elis/Stella/Sophia/Irene)
            AddExecutor(ExecutorType.Activate, CardId.Martha, MarthaSS);
            AddExecutor(ExecutorType.Activate, CardId.Elis, ElisSS);
            AddExecutor(ExecutorType.Activate, CardId.Stella, StellaSS);
            AddExecutor(ExecutorType.Activate, CardId.Sophia, SophiaDraw);
            AddExecutor(ExecutorType.Activate, CardId.Irene, IreneCycle);

            // TIER 7: Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.Martha, NormalSummonStarter);
            AddExecutor(ExecutorType.Summon, CardId.Stella, NormalSummonStarter);
            AddExecutor(ExecutorType.Summon, CardId.Elis, NormalSummonStarter);
            AddExecutor(ExecutorType.Summon, CardId.Sophia, NormalSummonStarter);
            AddExecutor(ExecutorType.Summon, CardId.Irene, NormalSummonStarter);

            // TIER 8: Extra Deck Summons
            AddExecutor(ExecutorType.SpSummon, CardId.Magnifica, MagnificaSpSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.Mikailis, XyzSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.Kaspitell, XyzSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.Gibrine, XyzSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.Karmael, XyzSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.Asophiel, XyzSummonCondition);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummonCondition);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);

            // TIER 9: Backrow Sets (MP2 / End of Turn)
            AddExecutor(ExecutorType.SpellSet, CardId.Returnia, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.Vadis, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.BookOfMoon, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.Pax, () => Util.IsTurn1OrMain2() && _paxUsed);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization, () => Util.IsTurn1OrMain2());
            AddExecutor(ExecutorType.SpellSet, CardId.ForbiddenDroplet, () => Util.IsTurn1OrMain2());

            // LAST: Battle Reposition
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand() => true; // Always choose Going First for full interaction setup

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            ResetOPTFlags();
            _xyzSummonedThisTurn = false;
        }

        public override void OnNewPhase()
        {
            base.OnNewPhase();
        }

        private void ResetOPTFlags()
        {
            _marthaHandUsed = false;
            _marthaGyTriggerUsed = false;
            _elisHandUsed = false;
            _elisGyTriggerUsed = false;
            _stellaHandUsed = false;
            _stellaGyTriggerUsed = false;
            _sophiaDrawUsed = false;
            _sophiaGyTriggerUsed = false;
            _ireneCycleUsed = false;
            _ireneGyTriggerUsed = false;
            _paxUsed = false;
            _armentUsed = false;
            _betrayalUsed = false;
            _returniaUsed = false;
            _vadisUsed = false;
            _mikailisSearchUsed = false;
            _mikailisBanishUsed = false;
            _kaspitellSearchUsed = false;
            _kaspitellLockUsed = false;
            _gibrineNegateUsed = false;
            _gibrineBoostUsed = false;
            _karmaelRewriteUsed = false;
            _asophielBounceUsed = false;
            _asophielLockUsed = false;
            _magnificaBanishUsed = false;
            _magnificaTagOutUsed = false;
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.Martha && card.Location == CardLocation.Hand) _marthaHandUsed = true;
                if (card.Id == CardId.Elis && card.Location == CardLocation.Hand) _elisHandUsed = true;
                if (card.Id == CardId.Pax) _paxUsed = true;
                if (card.Id == CardId.Arment) _armentUsed = true;
                if (card.Id == CardId.Betrayal) _betrayalUsed = true;
                if (card.Id == CardId.Returnia) _returniaUsed = true;
                if (card.Id == CardId.Vadis) _vadisUsed = true;
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
            if (c.IsCode(CardId.Magnifica)) return 950;
            if (IsAceCard(c)) return 900;
            if (c.IsCode(CardId.AshBlossom, CardId.MaxxC, CardId.EffectVeiler)) return 800;
            if (c.IsCode(CardId.Martha)) return 300;
            if (c.IsCode(CardId.Elis, CardId.Stella, CardId.Sophia, CardId.Irene)) return 200;
            return 100;
        }

        protected override bool IsBoardStrongEnough()
        {
            int disruptions = CountDisruptions();
            if (Bot.HasInMonstersZone(CardId.Magnifica)) disruptions += 2;
            if (Bot.HasInMonstersZone(CardId.Mikailis)) disruptions += 1;
            if (Bot.HasInMonstersZone(CardId.Gibrine)) disruptions += 1;
            if (Bot.HasInMonstersZone(CardId.Karmael)) disruptions += 1;
            if (Bot.HasInSpellZone(CardId.Returnia)) disruptions += 2;
            if (Bot.HasInSpellZone(CardId.Vadis)) disruptions += 2;

            return disruptions >= 4 && (Bot.HasInMonstersZone(CardId.Magnifica) || Bot.GetMonsterCount() >= 2);
        }

        protected override bool ShouldStopExtending()
        {
            if (IsBoardStrongEnough()) return true;
            return base.ShouldStopExtending();
        }

        // ==========================================
        // TIER 1 & 2: Hand Traps & Board Breakers
        // ==========================================

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

        private bool MulcharmyFuwalosActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (Bot.GetMonsterCount() > 0 || Bot.GetSpellCount() > 0) return false;
            if (Duel.Player != 1) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            return true;
        }

        private bool InfiniteImpermanenceActivate()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;

            if (Duel.Player == 0 && Duel.IsMainPhase())
            {
                var target = Util.GetProblematicEnemyMonster(0, true);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }

            return DefaultInfiniteImpermanence();
        }

        private bool VeilerActivate()
        {
            if (Duel.Player != 1) return false;
            if (!Duel.IsMainPhase()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            
            var target = Util.GetProblematicEnemyMonster(0, true);
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return DefaultEffectVeiler();
        }

        private bool DropletActivate()
        {
            var target = Util.GetProblematicEnemyMonster(0, canBeTarget: true);
            if (target != null && Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled()))
            {
                AI.SelectCard(target);
                return true;
            }
            if (EnemyHasKnownNegate())
            {
                var negateTarget = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled());
                if (negateTarget != null)
                {
                    AI.SelectCard(negateTarget);
                    return true;
                }
            }
            return false;
        }

        private bool BookOfMoonActivate()
        {
            if (Duel.Player == 1)
            {
                var threat = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && 
                    (c.Attack >= 2400 || c.IsFloodgate() || c.HasType(CardType.Synchro | CardType.Xyz | CardType.Link)));
                if (threat != null && !IsTargetImmune(threat))
                {
                    AI.SelectCard(threat);
                    return true;
                }
            }
            if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
            {
                var target = Util.GetProblematicEnemyMonster(0, true);
                if (target != null)
                {
                    AI.SelectCard(target);
                    return true;
                }
            }
            return false;
        }

        private bool SuperPolyActivate()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.SuperPolymerization)) return false;

            var enemyMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            var ourMonsters = Bot.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            var allMonsters = enemyMonsters.Concat(ourMonsters).ToList();

            if (enemyMonsters.Count == 0) return false;

            // 1. Garura check (same Type and Attribute, different names)
            foreach (var enemy in enemyMonsters)
            {
                foreach (var other in allMonsters)
                {
                    if (enemy == other) continue;
                    if (enemy.Attribute == other.Attribute && enemy.Race == other.Race && enemy.Id != other.Id)
                    {
                        if (ourMonsters.Contains(other) && IsAceCard(other)) continue;
                        return true;
                    }
                }
            }

            // 2. Mudragon check (same Attribute, different Types)
            foreach (var enemy in enemyMonsters)
            {
                foreach (var other in allMonsters)
                {
                    if (enemy == other) continue;
                    if (enemy.Attribute == other.Attribute && enemy.Race != other.Race)
                    {
                        if (ourMonsters.Contains(other) && IsAceCard(other)) continue;
                        return true;
                    }
                }
            }

            return false;
        }

        // ==========================================
        // TIER 3: Boss Xyz Quick Effects & Tag-Outs
        // ==========================================

        private bool MagnificaActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            int opt = (int)ActivateDescription;

            // Option 0: Quick Banish (Detach 1 material -> Banish 1 card opponent controls)
            if (opt == Util.GetStringId(CardId.Magnifica, 0) || Card.HasXyzMaterial())
            {
                if (_magnificaBanishUsed) return false;
                var target = GetBestBanishTarget();
                if (target != null && target.Controller == 1)
                {
                    _magnificaBanishUsed = true;
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("MagnificaActivate", $"Quick Banish target: {target.Name}");
                    return true;
                }
            }

            // Option 1: Quick Tag-Out (When opponent activates a card/effect -> Return attached Xyz to Extra, overlay into it)
            if (opt == Util.GetStringId(CardId.Magnifica, 1) || (Duel.LastChainPlayer == 1 && LastChainCard != null))
            {
                if (_magnificaTagOutUsed) return false;
                _magnificaTagOutUsed = true;
                DecisionTracer.TraceActivate("MagnificaActivate", "Quick Tag-Out into attached Xyz Boss");
                return true;
            }

            return false;
        }

        private bool MikailisActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            int opt = (int)ActivateDescription;

            // 1. Search Effect (Ignition, Main Phase, Detach 1)
            if (opt == Util.GetStringId(CardId.Mikailis, 1) || 
               (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) && Card.HasXyzMaterial() && !_mikailisSearchUsed))
            {
                bool hasTargets = GetRemainingCount(CardId.Pax) > 0 ||
                                  GetRemainingCount(CardId.Returnia) > 0 ||
                                  GetRemainingCount(CardId.Vadis) > 0 ||
                                  GetRemainingCount(CardId.Betrayal) > 0;
                if (hasTargets)
                {
                    _mikailisSearchUsed = true;
                    DecisionTracer.TraceActivate("MikailisActivate", "Search Exosister Spell/Trap");
                    return true;
                }
            }

            // 2. Quick Banish Effect (Quick Effect, targets 1 card on opponent's field or GY)
            if (!_mikailisBanishUsed)
            {
                var target = GetBestBanishTarget();
                if (target != null)
                {
                    _mikailisBanishUsed = true;
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("MikailisActivate", $"Quick Banish target: {target.Name} (Loc: {target.Location})");
                    return true;
                }
            }

            return false;
        }

        private bool KaspitellActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            int opt = (int)ActivateDescription;

            // 1. Search Effect (Ignition, Main Phase, Detach 1)
            if (opt == Util.GetStringId(CardId.Kaspitell, 1) || 
               (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2) && Card.HasXyzMaterial() && !_kaspitellSearchUsed))
            {
                bool hasTargets = GetRemainingCount(CardId.Martha) > 0 ||
                                  GetRemainingCount(CardId.Stella) > 0 ||
                                  GetRemainingCount(CardId.Elis) > 0 ||
                                  GetRemainingCount(CardId.Sophia) > 0 ||
                                  GetRemainingCount(CardId.Irene) > 0;
                if (hasTargets)
                {
                    _kaspitellSearchUsed = true;
                    DecisionTracer.TraceActivate("KaspitellActivate", "Search Exosister monster");
                    return true;
                }
            }

            // 2. GY Special Summon Lock (Trigger/Quick on summon)
            if (!_kaspitellLockUsed)
            {
                _kaspitellLockUsed = true;
                DecisionTracer.TraceActivate("KaspitellActivate", "Apply GY Special Summon lock");
                return true;
            }

            return false;
        }

        private bool GibrineActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            // 1. Quick Monster Negation
            if (!_gibrineNegateUsed)
            {
                if (Duel.Player == 1 && LastChainCard != null && LastChainCard.Controller == 1 && LastChainCard.IsMonster() && !IsTargetImmune(LastChainCard))
                {
                    _gibrineNegateUsed = true;
                    AI.SelectCard(LastChainCard);
                    DecisionTracer.TraceActivate("GibrineActivate", $"Negate chaining monster: {LastChainCard.Name}");
                    return true;
                }

                var threat = Enemy.GetMonsters().FirstOrDefault(c => c != null && c.IsFaceup() && !c.IsDisabled() && !IsTargetImmune(c) && (c.Attack >= 2200 || c.IsFloodgate()));
                if (threat != null)
                {
                    _gibrineNegateUsed = true;
                    AI.SelectCard(threat);
                    DecisionTracer.TraceActivate("GibrineActivate", $"Negate threat monster: {threat.Name}");
                    return true;
                }
            }

            // 2. ATK Boost (+800 to all Xyz)
            if (Duel.Player == 0 && Card.HasXyzMaterial() && !_gibrineBoostUsed)
            {
                if (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Main1)
                {
                    _gibrineBoostUsed = true;
                    DecisionTracer.TraceActivate("GibrineActivate", "Boost Xyz ATK by 800");
                    return true;
                }
            }

            return false;
        }

        private bool KarmaelActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            // 1. Quick Effect Rewrite (Detach all -> rewrite opponent effect to 'Add 1 card from opp GY to hand')
            if (Duel.Player == 1 && LastChainCard != null && LastChainCard.Controller == 1 && Card.HasXyzMaterial() && !_karmaelRewriteUsed)
            {
                _karmaelRewriteUsed = true;
                DecisionTracer.TraceActivate("KarmaelActivate", "Rewrite opponent effect and trigger sister transforms");
                return true;
            }

            // 2. End Phase Set effect
            if (Duel.Phase == DuelPhase.End)
            {
                DecisionTracer.TraceActivate("KarmaelActivate", "Set Exosister S/T in End Phase");
                return true;
            }

            return false;
        }

        private bool AsophielActivate()
        {
            if (Card.Location != CardLocation.MonsterZone || Card.IsDisabled()) return false;

            // 1. Bounce monster to hand (Ignition, Detach 1)
            if (Duel.Player == 0 && Card.HasXyzMaterial() && !_asophielBounceUsed)
            {
                var target = Util.GetProblematicEnemyMonster(0, true) ?? Enemy.MonsterZone.GetHighestAttackMonster(true);
                if (target != null && !IsTargetImmune(target))
                {
                    _asophielBounceUsed = true;
                    AI.SelectCard(target);
                    DecisionTracer.TraceActivate("AsophielActivate", $"Bounce monster: {target.Name}");
                    return true;
                }
            }

            // 2. GY Effect Activation Lock (on summon)
            if (!_asophielLockUsed)
            {
                _asophielLockUsed = true;
                DecisionTracer.TraceActivate("AsophielActivate", "Apply GY activation lock");
                return true;
            }

            return false;
        }

        // ==========================================
        // TIER 4: Traps (Disruptions & SS)
        // ==========================================

        private bool ReturniaActivate()
        {
            if (_returniaUsed) return false;
            if (Card.Location != CardLocation.SpellZone || Card.IsFacedown()) return false;
            if (Bot.LifePoints <= 800) return false;

            bool onlyExosisters = Bot.GetMonsters().All(c => c != null && c.IsFaceup() && 
                (ExosisterMonsters.Contains(c.Id) || AceCardIds.Contains(c.Id) || Rank4ExosisterXyz.Contains(c.Id)));
            if (!onlyExosisters || Bot.GetMonsterCount() == 0) return false;

            var target = GetBestBanishTarget();
            if (target != null)
            {
                _returniaUsed = true;
                AI.SelectCard(target);
                DecisionTracer.TraceActivate("ReturniaActivate", $"Banish threat via Returnia: {target.Name}");
                return true;
            }

            return false;
        }

        private bool VadisActivate()
        {
            if (_vadisUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.SpellZone || Card.IsFacedown()) return false;
            if (Bot.LifePoints <= 800) return false;
            if (Bot.GetMonsterCount() >= 4) return false;

            int targetsInDeck = GetRemainingCount(CardId.Martha) + GetRemainingCount(CardId.Elis) +
                               GetRemainingCount(CardId.Stella) + GetRemainingCount(CardId.Sophia) +
                               GetRemainingCount(CardId.Irene);
            if (targetsInDeck < 2) return false;

            // 1. Chain to opponent card/effect activation on their turn
            if (Duel.Player == 1 && Duel.LastChainPlayer == 1)
            {
                _vadisUsed = true;
                DecisionTracer.TraceActivate("VadisActivate", "Chain Vadis to opponent activation");
                return true;
            }

            // 2. Preemptive in opponent's Main Phase
            if (Duel.Player == 1 && Duel.IsMainPhase())
            {
                _vadisUsed = true;
                DecisionTracer.TraceActivate("VadisActivate", "Preemptive Vadis in opponent Main Phase");
                return true;
            }

            // 3. Our turn for extra materials
            if (Duel.Player == 0 && Duel.IsMainPhase() && Bot.GetMonsterCount() <= 2)
            {
                _vadisUsed = true;
                DecisionTracer.TraceActivate("VadisActivate", "Activate Vadis on our turn for combo materials");
                return true;
            }

            return false;
        }

        // ==========================================
        // TIER 5: Searchers & Starters Spells
        // ==========================================

        private bool PaxActivate()
        {
            if (_paxUsed) return false;
            if (ShouldSkipCombo()) return false;
            if (Bot.LifePoints <= 800) return false;

            _paxUsed = true;
            DecisionTracer.TraceActivate("PaxActivate", "Search Exosister card via Pax");
            return true;
        }

        private bool BetrayalActivate()
        {
            if (_betrayalUsed) return false;
            if (ShouldSkipCombo()) return false;

            if (Card.Location == CardLocation.Hand)
            {
                if (Bot.HasInSpellZone(CardId.Betrayal)) return false;
                if (Bot.GetHandCount() < 2) return false; // Need 1 card to discard
                _betrayalUsed = true;
                DecisionTracer.TraceActivate("BetrayalActivate", "Activate Betrayal to search 2 sisters and discard 1");
                return true;
            }

            if (Card.Location == CardLocation.SpellZone && Card.IsFaceup())
            {
                return true; // Trigger normal summon or GY banish effect
            }

            return false;
        }

        private bool CarpedivemActivate()
        {
            if (Bot.HasInSpellZone(CardId.Carpedivem)) return false;
            if (!Duel.IsMainPhase()) return false;
            AI.SelectAnnounceID(CardId.AshBlossom);
            return true;
        }

        private bool ArmentActivate()
        {
            if (_armentUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Bot.LifePoints <= 800) return false;

            bool hasTargetOnField = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                ExosisterMonsters.Contains(c.Id) && !c.IsExtraCard());
            if (!hasTargetOnField) return false;

            bool oppSpSummoned = Duel.Player == 1 && Enemy.GetMonsters().Any(c => c != null && c.IsSpecialSummoned);
            if (Duel.Player == 0 || oppSpSummoned)
            {
                _armentUsed = true;
                DecisionTracer.TraceActivate("ArmentActivate", "Rank-up Exosister into Extra Deck Xyz");
                return true;
            }

            return false;
        }

        // ==========================================
        // TIER 6 & 7: Monster Effects & Normal Summons
        // ==========================================

        private bool MarthaSS()
        {
            // Field trigger: opponent or self moves card from GY -> Xyz summon on self
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                DecisionTracer.TraceActivate("MarthaSS", "Martha field trigger: Transform into Exosister Xyz");
                return true;
            }

            // Hand effect: SS self + SS Elis from Deck
            if (_marthaHandUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.Hand) return false;

            bool canSS = Bot.GetMonsterCount() == 0 || Bot.GetMonsters().All(c => c != null && c.IsFaceup() && c.IsExtraCard());
            if (!canSS) return false;

            _marthaHandUsed = true;
            DecisionTracer.TraceActivate("MarthaSS", "Activate Martha in hand: SS Martha + Elis from Deck");
            return true;
        }

        private bool ElisSS()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                return true; // Transform into Xyz
            }

            if (_elisHandUsed) return false;
            if (IsSpecialSummonBlocked()) return false;
            if (Card.Location != CardLocation.Hand) return false;

            if (Duel.IsMainPhase())
            {
                bool controlExosister = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                    (ExosisterMonsters.Contains(c.Id) || AceCardIds.Contains(c.Id) || Rank4ExosisterXyz.Contains(c.Id)));
                if (controlExosister && Bot.GetMonsterCount() < 5)
                {
                    _elisHandUsed = true;
                    DecisionTracer.TraceActivate("ElisSS", "SS Elis from hand");
                    return true;
                }
            }

            return false;
        }

        private bool StellaSS()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                if (Duel.Player == 0 && Duel.IsMainPhase() && !_stellaHandUsed)
                {
                    bool hasExtenderInHand = Bot.Hand.Any(c => c != null && ExosisterMonsters.Contains(c.Id) && c.Id != CardId.Stella);
                    if (hasExtenderInHand && Bot.GetMonsterCount() < 5)
                    {
                        _stellaHandUsed = true;
                        DecisionTracer.TraceActivate("StellaSS", "Stella on field: SS Exosister from hand");
                        return true;
                    }
                }
                return true; // Transform into Xyz
            }
            return false;
        }

        private bool SophiaDraw()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                if (Duel.Player == 0 && Duel.IsMainPhase() && !_sophiaDrawUsed && Bot.HasInMonstersZone(CardId.Irene))
                {
                    _sophiaDrawUsed = true;
                    DecisionTracer.TraceActivate("SophiaDraw", "Draw 1 card via Sophia (Irene present)");
                    return true;
                }
                return true; // Transform into Xyz
            }
            return false;
        }

        private bool IreneCycle()
        {
            if (Card.Location == CardLocation.MonsterZone && Card.IsFaceup())
            {
                if (Duel.Player == 0 && Duel.IsMainPhase() && !_ireneCycleUsed)
                {
                    bool hasRedundantExoInHand = Bot.Hand.Any(c => c != null && ExosisterMonsters.Contains(c.Id) && c.Id != CardId.Martha);
                    if (hasRedundantExoInHand)
                    {
                        _ireneCycleUsed = true;
                        DecisionTracer.TraceActivate("IreneCycle", "Place 1 Exosister card to bottom of Deck, draw 1");
                        return true;
                    }
                }
                return true; // Transform into Xyz
            }
            return false;
        }

        private bool NormalSummonStarter()
        {
            if (ShouldSkipCombo()) return false;
            if (IsSpecialSummonBlocked()) return false;
            return true;
        }

        // ==========================================
        // TIER 8: Extra Deck Summons
        // ==========================================

        private bool MagnificaSpSummonCondition()
        {
            if (IsSpecialSummonBlocked()) return false;

            int faceupRank4Xyz = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.IsExtraCard() && 
                (Rank4ExosisterXyz.Contains(c.Id) || AceCardIds.Contains(c.Id)));
            
            return faceupRank4Xyz >= 2;
        }

        private bool XyzSummonCondition()
        {
            if (IsSpecialSummonBlocked()) return false;

            int faceupLevel4 = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && c.Level == 4 && !c.IsExtraCard());
            if (faceupLevel4 < 2) return false;

            if (ShouldStopExtending()) return false;

            // Going 2nd Board Breaking: prioritize Mikailis (banish) / Asophiel (bounce) / Gibrine (negate)
            if (ShouldGoBreakBoard && Card != null)
            {
                if (Card.IsCode(CardId.Mikailis) && !_mikailisBanishUsed && Enemy.GetMonsterCount() > 0) return true;
                if (Card.IsCode(CardId.Asophiel) && Enemy.GetMonsterCount() > 0) return true;
                if (Card.IsCode(CardId.Gibrine)) return true;
            }

            // Going 1st Standard: Kaspitell (search) -> Mikailis (search Returnia) -> Magnifica
            if (!ShouldGoBreakBoard && Card != null)
            {
                if (Card.IsCode(CardId.Kaspitell) && !_kaspitellSearchUsed) return true;
                if (Card.IsCode(CardId.Mikailis) && !_mikailisSearchUsed) return true;
                if (Card.IsCode(CardId.Karmael)) return true;
                if (Card.IsCode(CardId.Gibrine)) return true;
            }

            return true;
        }

        private bool SPLittleKnightSummonCondition()
        {
            if (IsSpecialSummonBlocked()) return false;
            if (Enemy.GetMonsterCount() == 0 && Enemy.GetSpellCount() == 0) return false;
            
            // Only make S:P if we have expendable non-Ace monsters
            int expendable = Bot.GetMonsters().Count(c => c != null && c.IsFaceup() && !IsAceCard(c));
            return expendable >= 2;
        }

        private bool SPLittleKnightActivate()
        {
            var target = Util.GetProblematicEnemyMonster(0, true) ?? Util.GetProblematicEnemyCard();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // ==========================================
        // Strategic Selections & Hints
        // ==========================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Material & Discard Hints (502 = Discard/Send to GY, 504 = Release, 511/513 = Xyz Material)
            if (hint == 502 || hint == 504 || hint == 511 || hint == 513 || hint == 533)
            {
                var sorted = cards.OrderBy(c => GetMaterialPriority(c)).ToList();
                return sorted.Take(max).ToList();
            }

            // Special Summon Hint (509)
            if (hint == 509)
            {
                // Martha SS -> Prefer Elis from Deck
                var elis = cards.FirstOrDefault(c => c != null && c.Id == CardId.Elis);
                if (elis != null) return new List<ClientCard> { elis };

                // Vadis SS -> Prefer pairs (Martha + Elis, or Sophia + Irene)
                if (cards.Any(c => c != null && c.Id == CardId.Martha) && cards.Any(c => c != null && c.Id == CardId.Elis))
                {
                    var martha = cards.First(c => c.Id == CardId.Martha);
                    var elisCard = cards.First(c => c.Id == CardId.Elis);
                    return new List<ClientCard> { martha, elisCard }.Take(max).ToList();
                }

                var preferred = cards.Where(c => c != null && AceCardIds.Contains(c.Id)).ToList();
                if (preferred.Count >= min) return preferred.Take(max).ToList();

                preferred = cards.Where(c => c != null && ExosisterMonsters.Contains(c.Id)).ToList();
                if (preferred.Count >= min) return preferred.Take(max).ToList();
            }

            // Search / Select from Deck Hint (506)
            if (hint == 506)
            {
                var selected = new List<ClientCard>();
                int[] targets;

                if (Card != null && Card.Id == CardId.Mikailis)
                {
                    // Mikailis S/T search priority: Returnia > Vadis > Pax > Betrayal > Arment
                    if (!_returniaUsed && GetRemainingCount(CardId.Returnia) > 0 && !Bot.HasInSpellZone(CardId.Returnia))
                        targets = new[] { CardId.Returnia, CardId.Vadis, CardId.Pax, CardId.Betrayal, CardId.Arment };
                    else if (!_paxUsed && !Bot.HasInHand(CardId.Pax))
                        targets = new[] { CardId.Pax, CardId.Vadis, CardId.Returnia, CardId.Betrayal, CardId.Arment };
                    else
                        targets = new[] { CardId.Vadis, CardId.Returnia, CardId.Betrayal, CardId.Pax, CardId.Arment };
                }
                else if (Card != null && Card.Id == CardId.Kaspitell)
                {
                    // Kaspitell Monster search priority: Martha > Stella > Elis > Sophia > Irene
                    if (!Bot.HasInHand(CardId.Martha) && !_marthaHandUsed)
                        targets = new[] { CardId.Martha, CardId.Stella, CardId.Elis, CardId.Sophia, CardId.Irene };
                    else if (!Bot.HasInHand(CardId.Stella) && !_stellaHandUsed)
                        targets = new[] { CardId.Stella, CardId.Elis, CardId.Sophia, CardId.Irene, CardId.Martha };
                    else
                        targets = new[] { CardId.Elis, CardId.Stella, CardId.Sophia, CardId.Irene, CardId.Martha };
                }
                else if (Card != null && Card.Id == CardId.Pax)
                {
                    // Pax search priority: Martha (if not had) > Returnia > Vadis > Stella > Elis
                    if (!Bot.HasInHand(CardId.Martha) && !_marthaHandUsed)
                        targets = new[] { CardId.Martha, CardId.Returnia, CardId.Vadis, CardId.Stella, CardId.Elis };
                    else
                        targets = new[] { CardId.Returnia, CardId.Vadis, CardId.Stella, CardId.Elis, CardId.Sophia };
                }
                else if (Card != null && Card.Id == CardId.Betrayal)
                {
                    targets = new[] { CardId.Martha, CardId.Elis, CardId.Stella, CardId.Sophia, CardId.Irene };
                }
                else
                {
                    targets = new[] { CardId.Martha, CardId.Pax, CardId.Returnia, CardId.Vadis, CardId.Elis, CardId.Stella, CardId.Sophia, CardId.Irene };
                }

                foreach (int id in targets)
                {
                    var match = cards.FirstOrDefault(c => c != null && c.Id == id && !selected.Contains(c));
                    if (match != null)
                    {
                        selected.Add(match);
                        if (selected.Count == max) return selected;
                    }
                }

                if (selected.Count < min)
                {
                    foreach (var c in cards)
                    {
                        if (c != null && !selected.Contains(c))
                        {
                            selected.Add(c);
                            if (selected.Count == min) break;
                        }
                    }
                }

                if (selected.Count >= min) return selected;
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (AceCardIds.Contains(cardId) || Rank4ExosisterXyz.Contains(cardId))
            {
                if (positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }

        public override int OnSelectOption(IList<long> options)
        {
            // For Carpedivem / Returnia effect selection: choose banish / first option
            return 0;
        }

        // ==========================================
        // Helper Methods: Targets & Battle
        // ==========================================

        private ClientCard GetBestBanishTarget()
        {
            var spells = Enemy.GetSpells().Where(c => c != null && c.IsFaceup()).ToList();
            
            // Priority 1: High-threat continuous floodgates & field spells
            var floodgateSpell = spells.FirstOrDefault(c => c.IsFloodgate() || 
                c.IsCode(27541563, 53936268, 48680970, 47222536, 68462976, 82732705, 58921041, 4064256));
            if (floodgateSpell != null) return floodgateSpell;

            // Priority 2: Opponent boss monsters & effect negators on field
            var monsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !IsTargetImmune(c)).ToList();
            var threatMonster = monsters.FirstOrDefault(c => c.Attack >= 2500 || c.IsFloodgate() || c.HasType(CardType.Xyz | CardType.Fusion | CardType.Link | CardType.Synchro))
                ?? monsters.OrderByDescending(c => c.Attack).FirstOrDefault();
            if (threatMonster != null) return threatMonster;

            // Priority 3: Dangerous GY cards (revivers & floaters)
            var gyMonsters = Enemy.Graveyard.Where(c => c != null && c.IsMonster() && c.IsCanRevive()).ToList();
            if (gyMonsters.Count > 0) return gyMonsters.OrderByDescending(c => c.Attack).First();

            var anyGyMonster = Enemy.Graveyard.FirstOrDefault(c => c != null && c.IsMonster());
            if (anyGyMonster != null) return anyGyMonster;

            // Priority 4: Face-down Spells/Traps in End Phase
            if (Duel.Player == 1 && Duel.Phase == DuelPhase.End)
            {
                var faceDownSpells = Enemy.GetSpells().Where(c => c != null && c.IsFacedown()).ToList();
                if (faceDownSpells.Count > 0) return faceDownSpells[0];
            }

            if (spells.Count > 0) return spells[0];
            if (monsters.Count > 0) return monsters[0];

            return null;
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;

            if (IsAceCard(Card))
            {
                if (Card.IsDefense() && Card.Attack > 0) return true;
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

    [Deck("Expert_2026_Exosister", "2026_Exosister")]
    public class ExpertExosisterExecutor : _2026_ExosisterExecutor
    {
        private string _duelId;
        public ExpertExosisterExecutor(GameAI ai, Duel duel) : base(ai, duel)
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
