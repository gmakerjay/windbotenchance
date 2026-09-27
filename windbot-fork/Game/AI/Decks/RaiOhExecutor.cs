using YGOSharp.OCGWrapper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    //  2026_RaiOh โ€” ANTI-META STUN STRATEGY & FLOODGATE EXECUTOR
    // ====================================================================================================
    //
    // เธเธฒเธกเน€เธ”เนเธ: 2026_RaiOh (Anti-Meta Stun)
    //
    // เธเธณเธญเธเธดเธเธฒเธขเธเธฅเธขเธธเธ—เธเนเนเธฅเธฐเธเธฒเธฃเธ—เธณเธเธฒเธเธซเธฅเธฑเธ (Strategy Overview):
    //  1. Floodgate Lock (เธเธฒเธฃเธเธดเธ”เธเธฑเนเธเธ”เนเธงเธขเธกเธญเธเธชเน€เธ•เธญเธฃเนเธชเนเธ•เธเธ”เน):
    //     - Inspector Boarder: เธฅเนเธญเธเธเธฒเธฃเน€เธเธดเธ”เนเธเนเธเธฒเธเน€เธญเธเน€เธเธเธ•เนเธกเธญเธเธชเน€เธ•เธญเธฃเน
    //     - Thunder King Rai-Oh: เธเนเธญเธเธเธฑเธเธเธฒเธฃเธเนเธเธซเธฒเธเธฒเธฃเนเธ”เธเธฒเธเน€เธ”เนเธ เนเธฅเธฐเธชเนเธเธชเธธเธชเธฒเธเน€เธเธทเนเธญเธขเธเน€เธฅเธดเธเธเธฒเธฃเธญเธฑเธเน€เธเธดเธเธเธดเน€เธจเธฉ
    //     - Banisher of the Radiance: เธเธณเธเธฒเธฃเนเธ”เธ—เธฑเนเธเธซเธกเธ”เธ—เธตเนเธฅเธเธชเธธเธชเธฒเธเธญเธญเธเธเธญเธเน€เธเธกเนเธ—เธ
    //  2. Time-Tearing Morganite Engine:
    //     - เธเธฑเนเธงเธเธฒเธฃเนเธ” 2 เนเธเธ•เนเธญเน€เธ—เธดเธฃเนเธ เนเธฅเธฐเธชเธฒเธกเธฒเธฃเธ– Normal Summon เนเธ”เน 2 เธเธฃเธฑเนเธเธ•เนเธญเน€เธ—เธดเธฃเนเธ
    //     - AI เธเธฐเธเธฑเธ”เธฅเธณเธ”เธฑเธเธญเธฑเธเน€เธเธดเธ Boarder -> Rai-Oh -> Banisher เน€เธเธทเนเธญเธซเธฅเธตเธเน€เธฅเธตเนเธขเธเธเธฒเธฃเธเธฅเนเธญเธเธเธฑเธเน€เธญเธ
    //  3. Hand-Activated Traps & Negates:
    //     - Dominus Impulse / Purge / Songs of the Dominators เธเธฑเธ”เธเธงเธฒเธเธเธญเธกเนเธเธเธนเนเธ•เนเธญเธชเธนเนเธเธฒเธเธเธเธกเธทเธญ
    //  4. Super Polymerization Board Breaker:
    //     - เธเธฅเธตเธเธเธญเธฃเนเธ”เธเธนเนเธ•เนเธญเธชเธนเนเธ”เนเธงเธขเธเธฒเธฃเธชเนเธเธกเธญเธเธชเน€เธ•เธญเธฃเนเธเธญเธเธเนเธฒเธขเธ•เธฃเธเธเนเธฒเธกเน€เธเนเธเธงเธฑเธ•เธ–เธธเธ”เธดเธเธเธดเธงเธเธฑเธ
    //
    // ====================================================================================================

    [Deck("2026_RaiOh", "2026_RaiOh")]
    public class _2026_RaiOhExecutor : ModernExecutor
    {
        public class CardId
        {
            // --- MAIN DECK FLOODGATES ---
            public const int BanisherOfRadiance = 94853057;
            public const int ThunderKingRaiOh = 71564252;
            public const int InspectorBoarder = 15397015;
            public const int DimensionShifter = 91800273;

            // --- SPELLS & CONTROLS ---
            public const int TimeTearingMorganite = 19403423;
            public const int TreasuresOfTheKings = 69299029;
            public const int DimensionalFissure = 81674782;
            public const int SuperPolymerization = 48130397;

            // --- NORMAL TRAPS & MIRROR FORCES ---
            public const int MirrorForce = 44095762;
            public const int BlazingMirrorForce = 75249652;
            public const int InfiniteImpermanence = 10045474;
            public const int DominusImpulse = 40366667;
            public const int DominusPurge = 97045737;
            public const int SongsOfTheDominators = 58053438;
            public const int Crackdown = 36975314;
            
            // --- APOPHIS ENGINE ---
            public const int ApophisSwampDeity = 85888377;
            public const int ApophisSerpent = 95561146;

            // --- COUNTER TRAPS ---
            public const int SolemnJudgment = 41420027;
            public const int SolemnWarning = 84749824;
            public const int SolemnStrike = 40605147;
            public const int DracoUtopianAura = 9070454;

            // --- EXTRA DECK FUSIONS ---
            public const int MudragonOfTheSwamp = 54757758;
            public const int MagistusChorozo = 66532962;
            public const int SeaMonsterOfTheseus = 96334243;
            public const int Garura = 11765832;
            public const int DivineSerpentApophis = 97800311;
            public const int AceSpadesSpeculation = 10796448;
            public const int StarvingVenom = 41209827;
            public const int PredaplantDragostapelia = 69946549;
            public const int SecreterionDragon = 89851827;
            public const int WorldChaliceAlmarduke = 95793022;
            public const int PredaplantTriphyoverutum = 79864860;
            public const int AxonKickerOracle = 33171768;
            public const int SuperStarslayerTYPHON = 93039339;
        }

        // --- GOING FIRST/SECOND TRACKING ---

        // --- OPT FLAGS & ENGINE STATE ---
        private bool _morganiteUsed = false;
        private int _summonCountThisTurn = 0;
        private bool _treasuresActivatedThisTurn = false;
        private bool _impulseActivatedThisTurn = false;
        private bool _purgeActivatedThisTurn = false;
        private bool _songsActivatedThisTurn = false;
        private bool _apophisSerpentUsedThisTurn = false;

        private static readonly int[] OpponentFloodgateCards = {
            42009023, // Fossil Dyna Pachycephalo
            7902349,  // Jowgen the Spiritualist
            15397015, // Inspect Boarder
            19261966, // El Shaddoll Winda
            82732047, // Skill Drain
            30241314, // Macro Cosmos
            81674782, // Dimensional Fissure
            92746535, // Summon Limit
            53334641, // Gozen Match
            90845713  // Rivalry of Warlords
        };

        private static readonly int[] NegateMonsterIds = {
            4280258,   // Apollousa, Bow of the Goddess
            84815190,  // Baronne de Fleur
            10443957,  // Cyber Dragon Infinity
            86066372,  // Herald of Ultimateness
            31801517   // Evolzar Dolkka
        };

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.InspectorBoarder 
                || card.Id == CardId.ThunderKingRaiOh 
                || card.Id == CardId.BanisherOfRadiance;
        }

        public override int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 900;
            if (c.Id == CardId.ApophisSerpent || c.Id == CardId.ApophisSwampDeity) return 100;
            return base.GetMaterialPriority(c);
        }

        public _2026_RaiOhExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // โ”€โ”€ Combo Router: Sequencing โ”€โ”€
            ComboRouter.RegisterLine(new ComboRouter.ComboLine {
                Name = "Standard-Setup",
                RequiredCards = new List<int> { CardId.BanisherOfRadiance, CardId.InspectorBoarder },
                Steps = new List<ComboRouter.ComboStep> {
                    new() { CardId = CardId.BanisherOfRadiance, ActionType = ExecutorType.Activate, Description = "Play CardId.BanisherOfRadiance" },
                    new() { CardId = CardId.InspectorBoarder, ActionType = ExecutorType.Activate, Description = "Extend with CardId.InspectorBoarder" }
                },
                EndBoardScore = 80
            });

            // โ”€โ”€ Bait Planner โ”€โ”€
            BaitPlanner.RegisterComboStarters(CardId.BanisherOfRadiance, CardId.ThunderKingRaiOh);
            BaitPlanner.RegisterBaitCards(CardId.ThunderKingRaiOh);

            // โ”€โ”€ Chain Advisor โ”€โ”€
            ChainAdvisor.RegisterHighValueTargets(CardId.BanisherOfRadiance, CardId.ThunderKingRaiOh, CardId.InfiniteImpermanence);

            // 1. Hand Traps, Negations & Rai-Oh Negate (Quick Activation)
            AddExecutor(ExecutorType.Activate, CardId.DimensionShifter, DimensionShifterEffect);
            AddExecutor(ExecutorType.Activate, CardId.ThunderKingRaiOh, RaiOhNegateEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusPurge, DominusPurgeEffect);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseEffect);
            AddExecutor(ExecutorType.Activate, CardId.SongsOfTheDominators, SongsOfTheDominatorsEffect);
            AddExecutor(ExecutorType.Activate, CardId.InfiniteImpermanence, InfiniteImpermanenceEffect);

            // 2. Counter Traps & Mirror Forces
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnWarning, SolemnWarningEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, SolemnStrikeEffect);
            AddExecutor(ExecutorType.Activate, CardId.DracoUtopianAura, DracoUtopianAuraEffect);
            AddExecutor(ExecutorType.Activate, CardId.MirrorForce, MirrorForceEffect);
            AddExecutor(ExecutorType.Activate, CardId.BlazingMirrorForce, MirrorForceEffect);

            // 3. Spells (Super Poly & Controls)
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.TimeTearingMorganite, TimeTearingMorganiteEffect);
            AddExecutor(ExecutorType.Activate, CardId.TreasuresOfTheKings, TreasuresOfTheKingsEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionalFissure, DimensionalFissureEffect);

            // 4. Trap Monsters & Swarm Controls
            AddExecutor(ExecutorType.Activate, CardId.ApophisSerpent, ApophisSerpentEffect);
            AddExecutor(ExecutorType.Activate, CardId.ApophisSwampDeity, ApophisSwampDeityEffect);
            AddExecutor(ExecutorType.Activate, CardId.Crackdown, CrackdownEffect);

            // 5. Strict Summon Sequencing (Boarder -> Rai-Oh -> Banisher)
            AddExecutor(ExecutorType.Summon, CardId.InspectorBoarder, BoarderSummon);
            AddExecutor(ExecutorType.Summon, CardId.ThunderKingRaiOh, RaiOhSummon);
            AddExecutor(ExecutorType.Summon, CardId.BanisherOfRadiance, BanisherSummon);

            // 6. Extra Deck Backup
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHON, TyPhonSummon);

            // 7. Spell/Trap Sets
            AddExecutor(ExecutorType.SpellSet, CardId.TreasuresOfTheKings);
            AddExecutor(ExecutorType.SpellSet, CardId.DimensionalFissure);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization);
            AddExecutor(ExecutorType.SpellSet, CardId.ApophisSerpent);
            AddExecutor(ExecutorType.SpellSet, CardId.ApophisSwampDeity);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnWarning);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnStrike);
            AddExecutor(ExecutorType.SpellSet, CardId.DracoUtopianAura);
            AddExecutor(ExecutorType.SpellSet, CardId.MirrorForce);
            AddExecutor(ExecutorType.SpellSet, CardId.BlazingMirrorForce);
            AddExecutor(ExecutorType.SpellSet, CardId.Crackdown);
            AddExecutor(ExecutorType.SpellSet, CardId.InfiniteImpermanence);

            // PositionRepos & Battle Repos
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Anti-meta stun โ€” strongly prefer going first to set floodgates
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _isGoingSecond = (Duel.Turn > 1);
            _summonCountThisTurn = 0;
            _treasuresActivatedThisTurn = false;
            _impulseActivatedThisTurn = false;
            _purgeActivatedThisTurn = false;
            _songsActivatedThisTurn = false;
            _apophisSerpentUsedThisTurn = false;

            // โ”€โ”€ Going-Second BreakBoard: prioritize disruption over combo โ”€โ”€
            if (ShouldGoBreakBoard)
            {
                // Reset board-breaking resources for aggressive turn-2 plays
            }
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.TimeTearingMorganite && card.Location != CardLocation.Grave && card.Location != CardLocation.Removed)
                {
                    _morganiteUsed = true;
                }
                if (card.Id == CardId.TreasuresOfTheKings)
                {
                    _treasuresActivatedThisTurn = true;
                }
                if (card.Id == CardId.DominusImpulse)
                {
                    _impulseActivatedThisTurn = true;
                }
                if (card.Id == CardId.DominusPurge)
                {
                    _purgeActivatedThisTurn = true;
                }
                if (card.Id == CardId.SongsOfTheDominators)
                {
                    _songsActivatedThisTurn = true;
                }
                if (card.Id == CardId.ApophisSerpent)
                {
                    _apophisSerpentUsedThisTurn = true;
                }
            }
        }

        // --- STRATEGIC CHECKS & HELPERS ---
        // IsSpecialSummonBlocked โ’ inherited from ModernExecutor FieldGuard



        private bool CanNormalSummon()
        {
            int limit = _morganiteUsed ? 2 : 1;
            return _summonCountThisTurn < limit;
        }

        private bool IsSafeToAttack(ClientCard attacker)
        {
            foreach (ClientCard enemy in Enemy.MonsterZone)
            {
                if (enemy == null || !enemy.IsFaceup()) continue;
                if (!enemy.IsDisabled())
                {
                    if (enemy.Id == 21887175 && attacker.IsSpecialSummoned) return false; // Avramax
                    if (enemy.Id == 50954680 && attacker.Level >= 5) return false;      // Crystal Wing
                }
                if (enemy.IsAttack())
                {
                    if (enemy.Attack > attacker.Attack) return false;
                    if (enemy.Attack == attacker.Attack && Bot.GetMonsterCount() < Enemy.GetMonsterCount()) return false;
                }
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
            if (Card == null) return false;
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

        // --- HAND TRAPS & SP/TRAP EFFECTS ---

        private bool IsOpponentCardAThreat(ClientCard card)
        {
            if (card == null) return false;

            if (card.IsSpell() || card.IsTrap())
            {
                int[] boardBreakers = {
                    53129443, // Raigeki
                    18144506, // Harpy's Feather Duster
                    15693423, // Evenly Matched
                    14558127, // Lightning Storm
                    82674125, // Cosmic Cyclone
                    43898403, // Twin Twisters
                    27243130, // Imperial Order
                    82732047, // Skill Drain
                    53582587, // Torrential Tribute
                    5818294   // Dark Hole
                };
                
                if (boardBreakers.Contains(card.Id)) return true;

                if (Bot.GetMonsterCount() > 0 || Bot.GetSpellCount() > 0)
                {
                    return true;
                }
            }

            if (card.IsMonster())
            {
                if (card.IsExtraCard() || card.Attack >= 2000 || card.Id == 1561110)
                {
                    return true;
                }
                return true;
            }

            return false;
        }

        private bool DimensionShifterEffect()
        {
            if (_morganiteUsed || Duel.CurrentChain.Any(c => c != null && c.Id == CardId.DimensionShifter)) return false;
            if (Bot.Graveyard.Count > 0) return false;
            return true;
        }

        private bool RaiOhNegateEffect()
        {
            if (Duel.LastChainPlayer == -1 && Duel.LastSummonPlayer == 1)
            {
                if (Duel.CurrentChain.Any(c => c != null && (c.Id == CardId.ThunderKingRaiOh || c.Id == CardId.SolemnWarning || c.Id == CardId.SolemnStrike || c.Id == CardId.SolemnJudgment)))
                    return false;
                return true;
            }
            return false;
        }

        private bool DominusPurgeEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            if (_purgeActivatedThisTurn || Duel.CurrentChain.Any(c => c != null && c.Id == CardId.DominusPurge)) return false;
            return true;
        }

        private bool DominusImpulseEffect()
        {
            if (Card.Location == CardLocation.Hand) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            if (_impulseActivatedThisTurn || Duel.CurrentChain.Any(c => c != null && c.Id == CardId.DominusImpulse)) return false;
            return true;
        }

        private bool SongsOfTheDominatorsEffect()
        {
            if (ShouldSkipCombo()) return false;
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            if (_songsActivatedThisTurn || Duel.CurrentChain.Any(c => c != null && c.Id == CardId.SongsOfTheDominators)) return false;
            return true;
        }

        private bool InfiniteImpermanenceEffect()
        {
            if (!SmartHandTrapChain()) return false;
            if (LastChainCard != null && LastChainCard.Controller == 0) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.InfiniteImpermanence)) return false;

            var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled()).ToList();
            if (targets.Count == 0) return false;

            var target = targets.FirstOrDefault(c => c.IsExtraCard() || NegateMonsterIds.Contains(c.Id) || c.Id == 1561110);
            if (target == null)
            {
                target = targets.OrderByDescending(c => c.Attack).FirstOrDefault();
            }

            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        private bool SolemnJudgmentEffect()
        {
            if (LastChainCard != null)
            {
                if (LastChainCard.Controller != 1) return false;
                if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.SolemnJudgment)) return false;
                return IsOpponentCardAThreat(LastChainCard);
            }
            if (Duel.LastChainPlayer == -1 && Duel.LastSummonPlayer == 1)
            {
                if (Duel.CurrentChain.Any(c => c != null && (c.Id == CardId.SolemnJudgment || c.Id == CardId.SolemnWarning || c.Id == CardId.SolemnStrike || c.Id == CardId.ThunderKingRaiOh)))
                    return false;
                return true;
            }
            return false;
        }

        private bool SolemnWarningEffect()
        {
            if (Bot.LifePoints <= 2000) return false;
            if (LastChainCard != null)
            {
                if (LastChainCard.Controller != 1) return false;
                if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.SolemnWarning)) return false;
                return IsOpponentCardAThreat(LastChainCard);
            }
            if (Duel.LastChainPlayer == -1 && Duel.LastSummonPlayer == 1)
            {
                if (Duel.CurrentChain.Any(c => c != null && (c.Id == CardId.SolemnJudgment || c.Id == CardId.SolemnWarning || c.Id == CardId.SolemnStrike || c.Id == CardId.ThunderKingRaiOh)))
                    return false;
                return true;
            }
            return false;
        }

        private bool SolemnStrikeEffect()
        {
            if (Bot.LifePoints <= 1500) return false;
            if (LastChainCard != null)
            {
                if (LastChainCard.Controller != 1) return false;
                if (!LastChainCard.IsMonster()) return false;
                if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.SolemnStrike)) return false;
                return IsOpponentCardAThreat(LastChainCard);
            }
            if (Duel.LastChainPlayer == -1 && Duel.LastSummonPlayer == 1)
            {
                if (Duel.CurrentChain.Any(c => c != null && (c.Id == CardId.SolemnJudgment || c.Id == CardId.SolemnWarning || c.Id == CardId.SolemnStrike || c.Id == CardId.ThunderKingRaiOh)))
                    return false;
                return true;
            }
            return false;
        }

        private bool DracoUtopianAuraEffect()
        {
            if (LastChainCard == null || LastChainCard.Controller != 1) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.DracoUtopianAura)) return false;
            return true;
        }

        private bool MirrorForceEffect()
        {
            if (Duel.Player != 1) return false;
            if (Duel.CurrentChain.Any(c => c != null && (c.Id == CardId.MirrorForce || c.Id == CardId.BlazingMirrorForce))) return false;
            return true;
        }

        private bool CrackdownEffect()
        {
            if (Duel.Player != 1) return false;
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.Crackdown)) return false;

            var targets = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup() && !c.IsDisabled()).ToList();
            if (targets.Count == 0) return false;
            
            var target = targets.OrderByDescending(c => c.Attack).FirstOrDefault();
            if (target != null)
            {
                AI.SelectCard(target);
                return true;
            }
            return false;
        }

        // --- SPELLS & CONTROLS ---

        private bool SuperPolymerizationEffect()
        {
            if (Bot.Hand.Count == 0 || Duel.CurrentChain.Any(c => c != null && c.Id == CardId.SuperPolymerization)) return false;

            var oppMonsters = Enemy.GetMonsters().Where(c => c != null && c.IsFaceup()).ToList();
            if (oppMonsters.Count < 2) return false;

            // 1. Starving Venom: 2 DARK monsters
            var darkMonsters = oppMonsters.Where(c => c.HasAttribute(CardAttribute.Dark)).ToList();
            if (darkMonsters.Count >= 2) return true;

            // 2. Mudragon of the Swamp: 2 monsters with same Attribute but different Types
            for (int i = 0; i < oppMonsters.Count; i++)
            {
                for (int j = i + 1; j < oppMonsters.Count; j++)
                {
                    var m1 = oppMonsters[i];
                    var m2 = oppMonsters[j];
                    if (m1.Attribute == m2.Attribute && m1.Race != m2.Race)
                        return true;
                }
            }

            // 3. Garura: 2 monsters with same Type and Attribute but different names
            for (int i = 0; i < oppMonsters.Count; i++)
            {
                for (int j = i + 1; j < oppMonsters.Count; j++)
                {
                    var m1 = oppMonsters[i];
                    var m2 = oppMonsters[j];
                    if (m1.Attribute == m2.Attribute && m1.Race == m2.Race && m1.Id != m2.Id)
                        return true;
                }
            }

            return false;
        }

        private bool DimensionalFissureEffect()
        {
            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                bool banishActive = 
                    Bot.SpellZone.Any(c => c != null && c.IsFaceup() && (c.Id == CardId.DimensionalFissure || c.Id == 30241314) && !c.IsDisabled())
                    || Enemy.SpellZone.Any(c => c != null && c.IsFaceup() && (c.Id == CardId.DimensionalFissure || c.Id == 30241314) && !c.IsDisabled())
                    || Bot.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.BanisherOfRadiance && !c.IsDisabled())
                    || Enemy.MonsterZone.Any(c => c != null && c.IsFaceup() && c.Id == CardId.BanisherOfRadiance && !c.IsDisabled());

                bool banishInChain = Duel.CurrentChain.Any(c => c != null && 
                    (c.Id == CardId.DimensionalFissure || c.Id == 30241314 || c.Id == CardId.BanisherOfRadiance));

                if (banishActive || banishInChain) return false;
                return true;
            }
            return false;
        }

        private bool TimeTearingMorganiteEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_morganiteUsed || Duel.CurrentChain.Any(c => c != null && c.Id == CardId.TimeTearingMorganite)) return false;
                
                // Hold if we have Dimension Shifter in hand and can activate it
                bool hasShifter = Bot.Hand.Any(c => c != null && c.Id == CardId.DimensionShifter);
                if (hasShifter && Bot.Graveyard.Count == 0)
                    return false;

                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
                if (!CanNormalSummon()) return false;
                bool hasAnotherInHand = Bot.Hand.Any(c => c != null && c.Id == CardId.TimeTearingMorganite);
                if (!hasAnotherInHand) return false;
                
                bool hasNormalSummonableMonsters = Bot.Hand.Any(c => c != null && c.IsMonster() && 
                    (c.Id == CardId.InspectorBoarder || c.Id == CardId.ThunderKingRaiOh || c.Id == CardId.BanisherOfRadiance));
                if (!hasNormalSummonableMonsters) return false;
                
                return true;
            }
            return false;
        }

        private bool TreasuresOfTheKingsEffect()
        {
            if (_treasuresActivatedThisTurn || Duel.CurrentChain.Any(c => c != null && c.Id == CardId.TreasuresOfTheKings)) return false;

            if (Card.Location == CardLocation.Hand || (Card.Location == CardLocation.SpellZone && Card.IsFacedown()))
            {
                bool hasApophisInDeck = GetRemainingCount(CardId.ApophisSerpent) > 0 || GetRemainingCount(CardId.ApophisSwampDeity) > 0;
                if (!hasApophisInDeck) return false;
                return true;
            }
            return false;
        }

        // --- APOPHIS ENGINE ---

        private bool ApophisSerpentEffect()
        {
            if (_apophisSerpentUsedThisTurn || Duel.CurrentChain.Any(c => c != null && c.Id == CardId.ApophisSerpent)) return false;
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                return Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2;
            }
            return false;
        }

        private bool ApophisSwampDeityEffect()
        {
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.ApophisSwampDeity)) return false;
            if (Card.Location == CardLocation.SpellZone && Card.IsFacedown())
            {
                if (Duel.Player == 0 && (Duel.Phase == DuelPhase.Main1 || Duel.Phase == DuelPhase.Main2))
                    return true;

                if (Duel.Player == 1)
                {
                    int otherContinuousTraps = Bot.SpellZone.Count(c => c != null && c.IsFaceup() && c.IsTrap() && c.HasType(CardType.Continuous));
                    bool oppHasFaceupCards = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && !c.IsDisabled())
                        || Enemy.GetSpells().Any(c => c != null && c.IsFaceup() && !c.IsDisabled());
                    
                    if (otherContinuousTraps > 0 && oppHasFaceupCards)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // --- SUMMONING FLOODGATES ---

        private bool BoarderSummon()
        {
            if (!CanNormalSummon()) return false;
            if (Bot.GetMonsterCount() > 0) return false;

            _summonCountThisTurn++;
            return true;
        }

        private bool RaiOhSummon()
        {
            if (ShouldSkipCombo()) return false;
            if (!CanNormalSummon()) return false;
            
            bool hasBoarder = Bot.Hand.Any(c => c != null && c.Id == CardId.InspectorBoarder);
            if (hasBoarder && Bot.GetMonsterCount() == 0) return false;

            _summonCountThisTurn++;
            return true;
        }

        private bool BanisherSummon()
        {
            if (!CanNormalSummon()) return false;

            bool hasBoarder = Bot.Hand.Any(c => c != null && c.Id == CardId.InspectorBoarder);
            bool hasRaiOh = Bot.Hand.Any(c => c != null && c.Id == CardId.ThunderKingRaiOh);
            if (Bot.GetMonsterCount() == 0 && (hasBoarder || hasRaiOh)) return false;

            _summonCountThisTurn++;
            return true;
        }

        private bool TyPhonSummon()
        {
            if (IsSpecialSummonBlocked()) return false;
            bool oppHasThreat = Enemy.GetMonsters().Any(c => c != null && c.IsFaceup() && 
                (c.Attack >= 3000 || NegateMonsterIds.Contains(c.Id) || c.IsExtraCard()));
            if (!oppHasThreat) return false;

            bool hasOurBoss = Bot.GetMonsters().Any(c => c != null && c.IsFaceup() && IsAceCard(c));
            if (hasOurBoss && Bot.GetMonsterCount() > Enemy.GetMonsterCount()) return false;

            return true;
        }

        // --- SELECT CARD / OPTION OVERRIDES ---

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // 1. Super Polymerization material selection
            if (LastChainCard != null && LastChainCard.Id == CardId.SuperPolymerization)
            {
                var oppMaterials = cards.Where(c => c != null && c.Controller == 1).ToList();
                var ourMaterials = cards.Where(c => c != null && c.Controller == 0).ToList();
                
                var selected = new List<ClientCard>();
                foreach (var c in oppMaterials)
                {
                    selected.Add(c);
                    if (selected.Count >= max) break;
                }
                if (selected.Count < min)
                {
                    foreach (var c in ourMaterials)
                    {
                        selected.Add(c);
                        if (selected.Count >= min) break;
                    }
                }
                return selected;
            }

            // 2. Apophis the Serpent search preferred
            if (LastChainCard != null && LastChainCard.Id == CardId.ApophisSerpent && hint == 510)
            {
                return SelectPreferred(cards, min, max, CardId.ApophisSwampDeity);
            }

            // 3. Treasures of the Kings activation set
            if (LastChainCard != null && LastChainCard.Id == CardId.TreasuresOfTheKings && hint == 510)
            {
                return SelectPreferred(cards, min, max, CardId.ApophisSerpent, CardId.ApophisSwampDeity);
            }

            // 4. Apophis the Swamp Deity negation targets
            if (LastChainCard != null && LastChainCard.Id == CardId.ApophisSwampDeity)
            {
                var oppThreats = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup() && !c.IsDisabled())
                    .OrderByDescending(c => c.Attack)
                    .ToList();
                
                var selected = new List<ClientCard>();
                foreach (var c in oppThreats)
                {
                    selected.Add(c);
                    if (selected.Count >= max) break;
                }
                if (selected.Count < min)
                {
                    foreach (var c in cards)
                    {
                        if (c != null && !selected.Contains(c))
                        {
                            selected.Add(c);
                            if (selected.Count >= min) break;
                        }
                    }
                }
                return selected;
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (cardId == CardId.InspectorBoarder || cardId == CardId.ThunderKingRaiOh || cardId == CardId.BanisherOfRadiance)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }
            return base.OnSelectPosition(cardId, positions);
        }
    }

    [Deck("Expert_2026_RaiOh", "2026_RaiOh")]
    public class ExpertRaiOhExecutor : _2026_RaiOhExecutor
    {
        private string _duelId;
        public ExpertRaiOhExecutor(GameAI ai, Duel duel) : base(ai, duel)
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
