using YGOSharp.OCGWrapper.Enums;
using WindBot.Game;

namespace WindBot.Game.AI
{
    /// <summary>
    /// AIBrain — standalone situational-awareness tracker.
    /// Hooks into DefaultExecutor.OnNewTurn / OnChaining / OnNewPhase.
    /// Zero impact on legacy executors. Custom executors read properties directly.
    /// 
    /// Architecture:
    ///   Executor.cs +1 line (constructor: Brain = new AIBrain(this))
    ///   DefaultExecutor.cs +3 lines (calls Brain.Reset/OnChain/OnPhase)
    ///   This file: self-contained, no dependencies beyond Game.AI namespace
    /// </summary>
    public class AIBrain
    {
        private readonly Executor _executor;
        private int _ownSummonCount;
        private int _enemySummonCount;
        private int _ownChainCount;
        private int _enemyChainCount;

        public AIBrain(Executor executor)
        {
            _executor = executor;
        }

        // ═══════════════════════════════════════
        //  SUMMON TRACKING (Nibiru / combo pacing)
        // ═══════════════════════════════════════

        /// <summary>Total Special Summons this turn (own + opponent).</summary>
        public int SummonCountThisTurn => _ownSummonCount + _enemySummonCount;

        /// <summary>Our own Special Summon count this turn.</summary>
        public int OwnSummons => _ownSummonCount;

        /// <summary>Opponent's Special Summon count this turn.</summary>
        public int OpponentSummonCount => _enemySummonCount;

        /// <summary>True if the 5th summon is approaching — Nibiru threat active.</summary>
        public bool NibiruIncoming => SummonCountThisTurn >= 4;

        /// <summary>True if we should stop summoning to avoid Nibiru (≥5 summons).</summary>
        public bool ShouldStopSummoning => SummonCountThisTurn >= 5;

        // ═══════════════════════════════════════
        //  CHAIN TRACKING
        // ═══════════════════════════════════════

        /// <summary>Current chain link depth (1 = first activation, 2+ = responses).</summary>
        public int ChainDepth => _executor.Duel.CurrentChain.Count;

        /// <summary>True if we already chained something this chain — avoid double-chaining.</summary>
        public bool OwnChainedThisChain => _ownChainCount > 0;

        /// <summary>True if opponent chained something this chain.</summary>
        public bool EnemyChainedThisChain => _enemyChainCount > 0;

        // ═══════════════════════════════════════
        //  OPPONENT ACTIVITY (hand trap timing)
        // ═══════════════════════════════════════

        /// <summary>True if opponent added a card from deck to hand this turn (searched/drew extra).</summary>
        public bool OpponentSearched { get; set; }

        /// <summary>True if opponent Special Summoned this turn.</summary>
        public bool OpponentSummoned => _enemySummonCount > 0;

        /// <summary>True if any opponent activity is detectable — chain, summon, or search.</summary>
        public bool OpponentActiveThisTurn => OpponentSearched || _enemySummonCount > 0 || _enemyChainCount > 0;

        // ═══════════════════════════════════════
        //  BOARD STATE SNAPSHOTS (convenience)
        // ═══════════════════════════════════════

        /// <summary>Hand card advantage: positive = we have more cards.</summary>
        public int HandAdvantage => (_executor.Bot?.Hand?.Count ?? 0) - (_executor.Enemy?.Hand?.Count ?? 0);

        /// <summary>Field monster advantage: positive = we have more monsters.</summary>
        public int FieldAdvantage => (_executor.Bot?.GetMonsterCount() ?? 0) - (_executor.Enemy?.GetMonsterCount() ?? 0);

        /// <summary>Total board card advantage (monsters + spells).</summary>
        public int BoardAdvantage => (_executor.Bot?.GetFieldCount() ?? 0) - (_executor.Enemy?.GetFieldCount() ?? 0);

        /// <summary>True if we have more resources than opponent (hand + field).</summary>
        public bool ControllingGame => HandAdvantage + FieldAdvantage >= 2;

        // ═══════════════════════════════════════
        //  FLOODGATE DETECTION
        // ═══════════════════════════════════════

        /// <summary>True if opponent has a spell floodgate active (Anti-Spell, Secret Village, Imperial Order).</summary>
        public bool IsSpellFloodgated
        {
            get
            {
                var enemy = _executor?.Enemy;
                if (enemy == null) return false;
                return enemy.HasInSpellZone(58921041, true, true)  // Anti-Spell Fragrance
                    || enemy.HasInSpellZone(68462976, true, true)  // Secret Village of the Spellcasters
                    || enemy.HasInSpellZone(61740673, true, true);       // Imperial Order
            }
        }

        /// <summary>True if opponent has a monster-effect floodgate active (Skill Drain).</summary>
        public bool IsMonsterFloodgated
        {
            get
            {
                var enemy = _executor?.Enemy;
                if (enemy == null) return false;
                return enemy.HasInSpellZone(82732705, true, true); // Skill Drain
            }
        }

        /// <summary>True if ANY disruptive floodgate is active against us.</summary>
        public bool IsFloodgated => IsSpellFloodgated || IsMonsterFloodgated;

        /// <summary>True if opponent controls a monster that can negate our effects during our turn.</summary>
        public bool EnemyHasNegate
        {
            get
            {
                var enemy = _executor?.Enemy;
                if (enemy == null) return false;
                return enemy.HasInMonstersZone(33198837, true, false, true)   // Naturia Beast (negate spells)
                    || enemy.HasInMonstersZone(86221708, true, false, true);  // Apollousa, Bow of the Goddess
            }
        }

        // ═══════════════════════════════════════
        //  TURN / PHASE SHORTCUTS
        // ═══════════════════════════════════════

        /// <summary>True if it's our first turn (Turn 1, player 0).</summary>
        public bool IsFirstTurn => _executor?.Duel?.Turn == 1 && _executor?.Duel?.Player == 0;

        /// <summary>True if we are going second on Turn 2.</summary>
        public bool IsGoingSecond => _executor?.Duel?.Turn == 2 && _executor?.Duel?.Player == 0;

        /// <summary>True if we are in Main Phase 1 (combo phase).</summary>
        public bool IsMainPhase1 => _executor?.Duel?.Player == 0 && _executor?.Duel?.Phase == DuelPhase.Main1;

        /// <summary>True if we are in Main Phase 2 (post-battle setup).</summary>
        public bool IsMainPhase2 => _executor?.Duel?.Player == 0 && _executor?.Duel?.Phase == DuelPhase.Main2;

        /// <summary>True if it's opponent's turn.</summary>
        public bool IsOpponentTurn => _executor?.Duel?.Player == 1;

        // ═══════════════════════════════════════
        //  LIFECYCLE (called from DefaultExecutor)
        // ═══════════════════════════════════════

        /// <summary>
        /// Call from DefaultExecutor.OnNewTurn().
        /// Resets all per-turn counters.
        /// </summary>
        public void OnNewTurn()
        {
            _ownSummonCount = 0;
            _enemySummonCount = 0;
            _ownChainCount = 0;
            _enemyChainCount = 0;
            OpponentSearched = false;
        }

        /// <summary>
        /// Call from DefaultExecutor.OnChaining(int player, ClientCard card).
        /// Tracks chain participation.
        /// </summary>
        public void OnChain(int player, ClientCard card)
        {
            if (card == null) return;

            // Track chain participation
            if (player == 0) _ownChainCount++;
            else _enemyChainCount++;
        }

        /// <summary>
        /// Hook to count a monster summon (Normal/Special).
        /// </summary>
        public void OnMonsterSummoned(ClientCard card, bool isSpecial)
        {
            if (card == null) return;
            if (card.Controller == 0)
                _ownSummonCount++;
            else
            {
                _enemySummonCount++;
                if (_executor is ModernExecutor modern)
                {
                    modern.OpponentProfile?.OnOpponentSummon(card);
                }
            }
        }

        /// <summary>
        /// Hook to notify that the opponent searched or drew a card from the deck.
        /// </summary>
        public void OnOpponentSearch()
        {
            OpponentSearched = true;
            if (_executor is ModernExecutor modern)
            {
                modern.OpponentProfile?.OnOpponentSearch();
            }
        }

        /// <summary>
        /// Hook to reset chain counters at the end of a chain.
        /// </summary>
        public void OnChainEnd()
        {
            _ownChainCount = 0;
            _enemyChainCount = 0;
        }

        /// <summary>
        /// Call from DefaultExecutor.OnNewPhase().
        /// Resets chain counters for new phase.
        /// </summary>
        public void OnNewPhase()
        {
            _ownChainCount = 0;
            _enemyChainCount = 0;
        }
    }
}
