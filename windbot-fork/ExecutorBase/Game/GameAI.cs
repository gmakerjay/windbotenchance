using System;
using System.Linq;
using System.Collections.Generic;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game
{
    public enum LogLevel : int
    {
        Info,
        Debug,
        Error
    }
    public class GameAI
    {
        public Duel Duel { get; private set; }
        public Executor Executor { get; set; }

        public Dialogs _dialogs;
        private System.Action<string, int> _log;

        // ── Turn-Summary Tracker ──
        private readonly System.Collections.Generic.List<string> _turnActions = new System.Collections.Generic.List<string>();
        private int _lastTrackedTurn = -1;

        public void Log(LogLevel level, string message)
        {
            _log(message, (int)level);
        }

        public GameAI(Duel duel, string dialog, System.Action<string, bool> chat, System.Action<string, int> log, string path)
        {
            Duel = duel;
            _log = log;
            _dialogs = new Dialogs(dialog, chat, path);
        }

        /// <summary>
        /// Called when the AI got the error message.
        /// </summary>
        public void OnRetry()
        {
            _dialogs.SendSorry();
        }

        public void OnDeckError(string card)
        {
            _dialogs.SendDeckSorry(card);
        }

        /// <summary>
        /// Called when the AI join the game.
        /// </summary>
        public void OnJoinGame()
        {
            _dialogs.SendWelcome();
        }

        /// <summary>
        /// Called when the duel starts.
        /// </summary>
        public void OnStart()
        {
            _dialogs.SendDuelStart();
        }

        /// <summary>
        /// Called when the AI do the rock-paper-scissors.
        /// </summary>
        /// <returns>1 for Scissors, 2 for Rock, 3 for Paper.</returns>
        public int OnRockPaperScissors()
        {
            return Executor.OnRockPaperScissors();
        }

        /// <summary>
        /// Called when the AI won the rock-paper-scissors.
        /// </summary>
        /// <returns>True if the AI should begin first, false otherwise.</returns>
        public bool OnSelectHand()
        {
            return Executor.OnSelectHand();
        }

        /// <summary>
        /// Called when any player draw card.
        /// </summary>
        public void OnDraw(int player)
        {
            Executor.OnDraw(player);
        }

        /// <summary>
        /// Called when it's a new turn.
        /// </summary>
        public void OnNewTurn()
        {
            // Emit summary for previous turn if we had actions
            FlushTurnSummary();

            _lastTrackedTurn = Duel.Turn;
            _turnActions.Clear();

            Executor.PreNewTurn();
            if (Executor.Scorer != null)
            {
                int score = Executor.Scorer.BoardAdvantageScore();
                Log(LogLevel.Info, $"[BOARD SCORE] Turn {Duel.Turn} | Score: {score} (Winning: {Executor.Scorer.WeAreWinning()}, Losing: {Executor.Scorer.WeAreLosing()})");
            }
            Executor.OnNewTurn();
        }

        /// <summary>
        /// Emit [TURN-SUMMARY] log line with all actions tracked this turn, then clear.
        /// </summary>
        private void FlushTurnSummary()
        {
            if (_turnActions.Count == 0) return;
            try
            {
                Log(LogLevel.Info, $"[TURN-SUMMARY] Turn {_lastTrackedTurn}: {string.Join(" → ", _turnActions)}");
            }
            catch { }
            _turnActions.Clear();
        }

        /// <summary>
        /// Track an action for the turn summary.
        /// </summary>
        private void TrackAction(string actionTag)
        {
            try { _turnActions.Add(actionTag); } catch { }
        }

        /// <summary>
        /// Called when it's a new phase.
        /// </summary>
        public void OnNewPhase()
        {
            m_selector.Clear();
            m_position.Clear();
            m_selector_pointer = -1;
            m_materialSelector = null;
            m_option = -1;
            m_yesno = -1;
           
            m_place = 0;
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.Draw)
            {
                _dialogs.SendNewTurn();
            }
            Executor.PreNewPhase();
            Executor.OnNewPhase();
        }

        /// <summary>
        /// Called when the AI got attack directly.
        /// </summary>
        public void OnDirectAttack(ClientCard card)
        {
            _dialogs.SendOnDirectAttack(card.Name);
        }

        /// <summary>
        /// Called when a chain is executed.
        /// </summary>
        /// <param name="card">Card who is chained.</param>
        /// <param name="player">Player who is currently chaining.</param>
        public void OnChaining(ClientCard card, int player)
        {
            Executor.PreChaining(player, card);
            Executor.OnChaining(player,card);
        }
        
        /// <summary>
        /// Called when a chain has been solved.
        /// </summary>
        public void OnChainEnd()
        {
            m_selector.Clear();
            m_selector_pointer = -1;
            Executor.OnChainEnd();
        }

        /// <summary>
        /// Called when the AI has to do something during the battle phase.
        /// </summary>
        /// <param name="battle">Informations about usable cards.</param>
        /// <returns>A new BattlePhaseAction containing the action to do.</returns>
        public BattlePhaseAction OnSelectBattleCmd(BattlePhase battle)
        {
            BattlePhaseAction action = InternalOnSelectBattleCmd(battle);
            if (action != null && Executor?.Scorer != null)
            {
                int score = Executor.Scorer.BoardAdvantageScore();
                Log(LogLevel.Info, $"[BOARD SCORE] Battle Command Decision | Score: {score} | Action: {action.Action} (Index: {action.Index})");
            }
            return action;
        }

        private BattlePhaseAction InternalOnSelectBattleCmd(BattlePhase battle)
        {
            Executor?.Scorer?.ClearCache();
            Executor.SetBattle(battle);
            BattlePhaseAction action = Executor.OnSelectBattleCmd(battle);
            if (action != null) return action;

            foreach (CardExecutor exec in Executor.Executors)
            {
                if (exec.Type == ExecutorType.GoToMainPhase2 && battle.CanMainPhaseTwo && exec.Func()) // check if should enter main phase 2 directly
                {
                    return ToMainPhase2();
                }
                if (exec.Type == ExecutorType.GoToEndPhase && battle.CanEndPhase && exec.Func()) // check if should enter end phase directly
                {
                    return ToEndPhase();
                }
                for (int i = 0; i < battle.ActivableCards.Count; ++i)
                {
                    ClientCard card = battle.ActivableCards[i];
                    if (ShouldExecute(exec, card, ExecutorType.Activate, battle.ActivableDescs[i]))
                    {
                        _dialogs.SendChaining(card.Name);
                        return new BattlePhaseAction(BattlePhaseAction.BattleAction.Activate, card.ActionIndex);
                    }
                }
            }

            // Sort the attackers and defenders, make monster with higher attack go first.
            List<ClientCard> attackers = new List<ClientCard>(battle.AttackableCards);
            attackers.Sort(CardContainer.CompareCardAttack);
            attackers.Reverse();

            List<ClientCard> defenders = new List<ClientCard>(Duel.Fields[1].GetMonsters());
            defenders.Sort(CardContainer.CompareDefensePower);
            defenders.Reverse();

            // Let executor decide which card should attack first.
            ClientCard selected = Executor.OnSelectAttacker(attackers, defenders);
            if (selected != null && attackers.Contains(selected))
            {
                attackers.Remove(selected);
                attackers.Insert(0, selected);
            }

            // Check for the executor.
            BattlePhaseAction result = Executor.OnBattle(attackers, defenders);
            if (result != null)
                return result;

            if (attackers.Count == 0)
            {
                if (battle.CanMainPhaseTwo) return ToMainPhase2();
                else if (battle.CanEndPhase) return ToEndPhase();
            }

            if (defenders.Count == 0)
            {
                // [FIX BATTLE-IQ] When opponent controls facedown Spell/Trap and lethal is not assured,
                // attack with lower ATK monster first to bait Mirror Force / battle traps!
                bool oppHasFacedownBackrow = Duel.Fields[1].GetSpells().Any(s => s != null && s.IsFacedown());
                if (oppHasFacedownBackrow && !Executor.ShouldRushAttack && attackers.Count > 1)
                {
                    for (int i = attackers.Count - 1; i >= 0; --i)
                    {
                        ClientCard attacker = attackers[i];
                        if (attacker.Attack > 0)
                            return Attack(attacker, null);
                    }
                }

                // Otherwise, attack with highest ATK first to maximize damage before interruption
                for (int i = 0; i < attackers.Count; ++i)
                {
                    ClientCard attacker = attackers[i];
                    if (attacker.Attack > 0)
                        return Attack(attacker, null);
                }
            }
            else
            {
                for (int k = 0; k < attackers.Count; ++k)
                {
                    ClientCard attacker = attackers[k];
                    attacker.IsLastAttacker = (k == attackers.Count - 1);
                    result = Executor.OnSelectAttackTarget(attacker, defenders);
                    if (result != null)
                        return result;
                }
            }

            if (!battle.CanMainPhaseTwo && !battle.CanEndPhase)
                return Attack(attackers[0], (defenders.Count == 0) ? null : defenders[0]);

            return battle.CanMainPhaseTwo ? ToMainPhase2() : ToEndPhase();
        }

        /// <summary>
        /// Called when the AI has to select one or more cards.
        /// </summary>
        /// <param name="cards">List of available cards.</param>
        /// <param name="min">Minimal quantity.</param>
        /// <param name="max">Maximal quantity.</param>
        /// <param name="hint">The hint message of the select.</param>
        /// <param name="cancelable">True if you can return an empty list.</param>
        /// <returns>A new list containing the selected cards.</returns>
        public IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            Executor?.Scorer?.ClearCache();
            // Local helper: validate & sanitize selection via HeuristicGuard before returning
            IList<ClientCard> ValidateAndReturn(IList<ClientCard> sel, string source)
            {
                try { Log(LogLevel.Info, $"[DEBUG-DECISION] OnSelectCard -> Selected ({source}): " + string.Join(", ", sel.Select(c => c == null ? "null" : $"{c.Id}"))); } catch {}
                try { sel = WindBot.HeuristicGuard.SanitizeSelection(sel, cards, min, max, hint, cancelable, Duel.Turn, Duel.Fields[0], Duel.Fields[1]); } catch {}
                try { WindBot.HeuristicGuard.ValidateSelection(sel, hint, Duel.Turn, Duel.Fields[0], Duel.Fields[1]); } catch {}
                return sel;
            }

            try
            {
                Log(LogLevel.Info, $"[DEBUG-DECISION] OnSelectCard: min={min}, max={max}, hint={hint}, cancelable={cancelable}. Options: " + string.Join(", ", cards.Select(c => c == null ? "null" : $"{c.Id} (Loc={c.Location})")));
            }
            catch {}

            const long HINTMSG_FMATERIAL = 511;
            const long HINTMSG_SMATERIAL = 512;
            const long HINTMSG_XMATERIAL = 513;
            const long HINTMSG_LMATERIAL = 533;
            const long HINTMSG_SPSUMMON = 509;

            // Check for the executor.
            IList<ClientCard> result = Executor.OnSelectCard(cards, min, max, hint, cancelable);
            if (result != null)
            {
                return ValidateAndReturn(result, "Executor");
            }

            if (hint == HINTMSG_SPSUMMON && min == 1 && max > min) // pendulum summon
            {
                result = Executor.OnSelectPendulumSummon(cards, max);
                if (result != null)
                {
                    return ValidateAndReturn(result, "Pendulum");
                }
            }

            CardSelector selector = null;
            if (hint == HINTMSG_FMATERIAL || hint == HINTMSG_SMATERIAL || hint == HINTMSG_XMATERIAL || hint == HINTMSG_LMATERIAL)
            {
                if (m_materialSelector != null)
                {
                    //Logger.DebugWriteLine("m_materialSelector");
                    selector = m_materialSelector;
                }
                else
                {
                    if (hint == HINTMSG_FMATERIAL)
                        result = Executor.OnSelectFusionMaterial(cards, min, max);
                    if (hint == HINTMSG_SMATERIAL)
                        result = Executor.OnSelectSynchroMaterial(cards, 0, min, max);
                    if (hint == HINTMSG_XMATERIAL)
                        result = Executor.OnSelectXyzMaterial(cards, min, max);
                    if (hint == HINTMSG_LMATERIAL)
                        result = Executor.OnSelectLinkMaterial(cards, min, max);

                    if (result != null)
                    {
                        return ValidateAndReturn(result, "Material");
                    }

                    // Update the next selector.
                    selector = GetSelectedCards();
                }
            }
            else
            {
                // Update the next selector.
                selector = GetSelectedCards();
            }

            // If we selected a card, use this card.
            if (selector != null)
            {
                var selResult = selector.Select(cards, min, max);
                return ValidateAndReturn(selResult, "Selector");
            }

            // Centralized smart fallback evaluation before resorting to blind selection
            if (Executor != null)
            {
                try
                {
                    IList<ClientCard> fallbackResult = Executor.FallbackSelectCard(cards, min, max, hint, cancelable);
                    if (fallbackResult != null && fallbackResult.Count >= min)
                    {
                        return ValidateAndReturn(fallbackResult, "Fallback");
                    }
                }
                catch (System.Exception ex)
                {
                    try { Log(LogLevel.Info, $"[FALLBACK-CARD-ERROR] {ex.Message}"); } catch {}
                }
            }

            // Always select the first available cards and choose the minimum if fallback could not decide.
            IList<ClientCard> selected = new List<ClientCard>();

            if (cards.Count >= min)
            {
                for (int i = 0; i < min; ++i)
                    selected.Add(cards[i]);
            }
            return ValidateAndReturn(selected, "Default");
        }

        /// <summary>
        /// Called when the AI can chain (activate) a card.
        /// </summary>
        /// <param name="cards">List of activable cards.</param>
        /// <param name="descs">List of effect descriptions.</param>
        /// <param name="forced">You can't return -1 if this param is true.</param>
        /// <returns>Index of the activated card or -1.</returns>
        public int OnSelectChain(IList<ClientCard> cards, IList<long> descs, bool forced)
        {
            Executor?.Scorer?.ClearCache();

            // 1. Universal Chain Link 3 Defense (Called by the Grave / Crossout vs Handtraps)
            if (Executor != null)
            {
                int cl3Idx = Executor.CheckChainLink3Defense(cards, descs);
                if (cl3Idx >= 0)
                {
                    _dialogs.SendChaining(cards[cl3Idx]?.Name ?? string.Empty);
                    return cl3Idx;
                }

                // 2. Pre-emptive Draw/Standby Phase Floodgates (Skill Drain, D-Barrier, Anti-Spell, etc.)
                int floodIdx = Executor.CheckDrawStandbyFloodgate(cards, descs);
                if (floodIdx >= 0)
                {
                    _dialogs.SendChaining(cards[floodIdx]?.Name ?? string.Empty);
                    return floodIdx;
                }
            }

            foreach (CardExecutor exec in Executor.Executors)
            {
                for (int i = 0; i < cards.Count; ++i)
                {
                    ClientCard card = cards[i];
                    if (ShouldExecute(exec, card, ExecutorType.Activate, descs[i]))
                    {
                        // Universal Safety: If this is a once-per-turn handtrap or response,
                        // ensure we didn't already chain the identical card in this exact chain
                        if (CardIntelligence.IsHandtrap(card.Id) && Duel.CurrentChain != null &&
                            Duel.CurrentChain.Any(c => c != null && c.Controller == 0 && (c.Id == card.Id || c.GetNonAltartCode() == card.GetNonAltartCode())))
                        {
                            continue;
                        }

                        _dialogs.SendChaining(card.Name);
                        return i;
                    }
                }
            }
            // If we're forced to chain, we chain the first card. However don't do anything.
            return forced ? 0 : -1;
        }
        
        /// <summary>
        /// Called when the AI has to use one or more counters.
        /// </summary>
        /// <param name="type">Type of counter to use.</param>
        /// <param name="quantity">Quantity of counter to select.</param>
        /// <param name="cards">List of available cards.</param>
        /// <param name="counters">List of available counters.</param>
        /// <returns>List of used counters.</returns>
        public IList<int> OnSelectCounter(int type, int quantity, IList<ClientCard> cards, IList<int> counters)
        {
            IList<int> custom = Executor?.OnSelectCounter(type, quantity, cards, counters);
            if (custom != null)
                return custom;

            // Always select the first available counters safely without index out of bounds.
            int[] used = new int[counters.Count];
            int needed = quantity;
            for (int i = 0; i < counters.Count && needed > 0; i++)
            {
                int take = Math.Min(counters[i], needed);
                used[i] = take;
                needed -= take;
            }
            return used;
        }

        /// <summary>
        /// Called when the AI has to sort cards.
        /// </summary>
        /// <param name="cards">Cards to sort.</param>
        /// <returns>List of sorted cards.</returns>
        public IList<ClientCard> OnCardSorting(IList<ClientCard> cards)
        {

            IList<ClientCard> result = Executor.OnCardSorting(cards);
            if (result != null)
                return result;
            result = new List<ClientCard>();
            // TODO: use selector
            result = cards.ToList();
            return result;
        }

        /// <summary>
        /// Called when the AI has to choose to activate or not an effect.
        /// </summary>
        /// <param name="card">Card to activate.</param>
        /// <returns>True for yes, false for no.</returns>
        public bool OnSelectEffectYn(ClientCard card, long desc)
        {
            Executor?.Scorer?.ClearCache();
            bool? result = Executor?.OnSelectEffectYn(card, desc);
            if (result.HasValue)
                return result.Value;

            bool hasExecutor = false;
            foreach (CardExecutor exec in Executor.Executors)
            {
                if (exec.Type == ExecutorType.Activate && (exec.CardId == -1 || exec.CardId == card.Id))
                {
                    hasExecutor = true;
                    if (ShouldExecute(exec, card, ExecutorType.Activate, desc))
                        return true;
                }
            }
            if (hasExecutor)
                return false;

            // Universal Safeguard:
            // If the card prompting the question is an OPPONENT card (card.Controller == 1),
            // unhandled optional prompts are dangerous traps/penalties/opponent requests.
            // Decline opponent-initiated effects by default unless explicitly registered.
            if (card != null && card.Controller == 1)
                return false;

            // [FIX MAJOR-2] Default to YES for optional effects.
            // Previously defaulted to false, declining beneficial effects
            // (e.g., "Draw 1 card?", "Add to hand?") that weren't explicitly registered.
            // Most optional effects in YGO are beneficial. Aligns with OnSelectYesNo default (true).
            return true;
        }

        /// <summary>
        /// Called when the AI has to do something during the main phase.
        /// </summary>
        /// <param name="main">A lot of informations about the available actions.</param>
        /// <returns>A new MainPhaseAction containing the action to do.</returns>
        public MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            MainPhaseAction action = InternalOnSelectIdleCmd(main);
            if (action != null && Executor?.Scorer != null)
            {
                int score = Executor.Scorer.BoardAdvantageScore();
                Log(LogLevel.Info, $"[BOARD SCORE] Idle Command Decision | Score: {score} | Action: {action.Action} (Index: {action.Index})");

                // Track for turn summary — emit on phase transition
                if (action.Action == MainPhaseAction.MainAction.ToBattlePhase)
                {
                    TrackAction("→ Battle");
                    FlushTurnSummary();
                }
                else if (action.Action == MainPhaseAction.MainAction.ToEndPhase)
                {
                    TrackAction("→ End");
                    FlushTurnSummary();
                }
            }
            return action;
        }

        private MainPhaseAction InternalOnSelectIdleCmd(MainPhase main)
        {
            Executor?.Scorer?.ClearCache();
            try
            {
                Log(LogLevel.Info, $"[DEBUG-IDLE] Turn={Duel.Turn}, Phase={Duel.Phase}. Activable count: {main.ActivableCards.Count}, Summonable: {main.SummonableCards.Count}, SpSummonable: {main.SpecialSummonableCards.Count}. Options: " +
                    "Activable=[" + string.Join(", ", main.ActivableCards.Select(c => c == null ? "null" : $"{c.Id} ({c.Location})")) + "], " +
                    "Summonable=[" + string.Join(", ", main.SummonableCards.Select(c => c == null ? "null" : $"{c.Id}")) + "], " +
                    "SpSummonable=[" + string.Join(", ", main.SpecialSummonableCards.Select(c => c == null ? "null" : $"{c.Id}")) + "]");
            }
            catch (System.Exception ex)
            {
                Log(LogLevel.Info, $"[DEBUG-IDLE] Logging error: {ex.Message}");
            }

            Executor.SetMain(main);
            MainPhaseAction action = Executor.OnSelectIdleCmd(main);
            if (action != null) return action;

            foreach (CardExecutor exec in Executor.Executors)
            {
            	if (exec.Type == ExecutorType.GoToEndPhase && main.CanEndPhase && exec.Func()) // check if should enter end phase directly
                {
                    _dialogs.SendEndTurn();
                    return new MainPhaseAction(MainPhaseAction.MainAction.ToEndPhase);
                }
                if (exec.Type==ExecutorType.GoToBattlePhase && main.CanBattlePhase && exec.Func()) // check if should enter battle phase directly
                {
                    return new MainPhaseAction(MainPhaseAction.MainAction.ToBattlePhase);
                }
                // NOTICE: GoToBattlePhase and GoToEndPhase has no "card" can be accessed to ShouldExecute(), so instead use exec.Func() to check ...
                // enter end phase and enter battle pahse is in higher priority. 

                for (int i = 0; i < main.ActivableCards.Count; ++i)
                {
                    ClientCard card = main.ActivableCards[i];
                    if (!Executor.ShouldAllowActivate(card)) continue;
                    if (ShouldExecute(exec, card, ExecutorType.Activate, main.ActivableDescs[i]))
                    {
                        _dialogs.SendActivate(card.Name);
                        TrackAction($"Activate({card.Name ?? $"#{card.Id}"})");
                        return new MainPhaseAction(MainPhaseAction.MainAction.Activate, card.ActionActivateIndex[main.ActivableDescs[i]]);
                    }
                }
                foreach (ClientCard card in main.MonsterSetableCards)
                {
                    if (!Executor.ShouldAllowMonsterSet(card)) continue;
                    if (ShouldExecute(exec, card, ExecutorType.MonsterSet))
                    {
                        _dialogs.SendSetMonster();
                        TrackAction($"MSet({card.Name ?? $"#{card.Id}"})");
                        return new MainPhaseAction(MainPhaseAction.MainAction.SetMonster, card.ActionIndex);
                    }
                }
                foreach (ClientCard card in main.ReposableCards)
                {
                    if (!Executor.ShouldAllowRepos(card)) continue;
                    if (ShouldExecute(exec, card, ExecutorType.Repos))
                    {
                        TrackAction($"Repos({card.Name ?? $"#{card.Id}"})");
                        return new MainPhaseAction(MainPhaseAction.MainAction.Repos, card.ActionIndex);
                    }
                }
                foreach (ClientCard card in main.SpecialSummonableCards)
                {
                    if (!Executor.ShouldAllowSpSummon(card)) continue;
                    if (ShouldExecute(exec, card, ExecutorType.SpSummon))
                    {
                        _dialogs.SendSummon(card.Name);
                        TrackAction($"SS({card.Name ?? $"#{card.Id}"})");
                        return new MainPhaseAction(MainPhaseAction.MainAction.SpSummon, card.ActionIndex);
                    }
                }
                foreach (ClientCard card in main.SummonableCards)
                {
                    if (!Executor.ShouldAllowSummon(card)) continue;
                    if (ShouldExecute(exec, card, ExecutorType.Summon))
                    {
                        _dialogs.SendSummon(card.Name);
                        TrackAction($"NS({card.Name ?? $"#{card.Id}"})");
                        return new MainPhaseAction(MainPhaseAction.MainAction.Summon, card.ActionIndex);
                    }
                    if (ShouldExecute(exec, card, ExecutorType.SummonOrSet))
                    {
                        // [FIX MAJOR-3] Don't set Effect monsters face-down.
                        // Effect monsters often have on-summon triggers (search, SS from deck)
                        // that are LOST when set face-down. Only set non-Effect vanilla monsters.
                        if (Executor.Util.IsAllEnemyBetter(true) && Executor.Util.IsAllEnemyBetterThanValue(card.Attack + 300, false) &&
                            main.MonsterSetableCards.Contains(card) && !card.HasType(CardType.Effect))
                        {
                            _dialogs.SendSetMonster();
                            TrackAction($"MSet({card.Name ?? $"#{card.Id}"})");
                            return new MainPhaseAction(MainPhaseAction.MainAction.SetMonster, card.ActionIndex);
                        }
                        _dialogs.SendSummon(card.Name);
                        TrackAction($"NS({card.Name ?? $"#{card.Id}"})");
                        return new MainPhaseAction(MainPhaseAction.MainAction.Summon, card.ActionIndex);
                    }
                }                
                foreach (ClientCard card in main.SpellSetableCards)
                {
                    if (!Executor.ShouldAllowSpellSet(card)) continue;
                    if (ShouldExecute(exec, card, ExecutorType.SpellSet))
                    {
                        TrackAction($"Set({card.Name ?? $"#{card.Id}"})");
                        return new MainPhaseAction(MainPhaseAction.MainAction.SetSpell, card.ActionIndex);
                    }
                }
            }

            if (main.CanBattlePhase && Duel.Fields[0].HasAttackingMonster())
                return new MainPhaseAction(MainPhaseAction.MainAction.ToBattlePhase);

            // [CENTRAL CORE] Universal Fallback Idle Command
            // Before passing the turn, check if the Executor wants to execute any desperation defense,
            // smart repositioning, or backrow setup that wasn't caught by CardExecutors.
            MainPhaseAction fallbackAction = Executor.OnFallbackIdleCmd(main);
            if (fallbackAction != null) return fallbackAction;

            _dialogs.SendEndTurn();
            return new MainPhaseAction(MainPhaseAction.MainAction.ToEndPhase); 
        }

        /// <summary>
        /// Called when the AI has to select an option.
        /// </summary>
        /// <param name="options">List of available options.</param>
        /// <returns>Index of the selected option.</returns>
        public int OnSelectOption(IList<long> options)
        {
            if (m_option != -1 && m_option < options.Count)
                return m_option;

            int result = Executor.OnSelectOption(options);
            if (result != -1)
                return result;

            return 0; // Always select the first option.
        }

        public int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            Executor?.Scorer?.ClearCache();
            int selector_selected = m_place;
            m_place = 0;

            int executor_selected = Executor.OnSelectPlace(cardId, player, location, available);

            if ((executor_selected & available) > 0)
                return executor_selected & available;
            if ((selector_selected & available) > 0)
                return selector_selected & available;

            // TODO: LinkedZones

            return 0;
        }

        /// <summary>
        /// Called when the AI has to select a card position.
        /// </summary>
        /// <param name="cardId">Id of the card to position on the field.</param>
        /// <param name="positions">List of available positions.</param>
        /// <returns>Selected position.</returns>
        public CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            Executor?.Scorer?.ClearCache();
            CardPosition selector_selected = GetSelectedPosition();

            CardPosition executor_selected = Executor.OnSelectPosition(cardId, positions);

            // Selects the selected position if available, the first available otherwise.
            if (positions.Contains(executor_selected))
                return executor_selected;
            if (positions.Contains(selector_selected))
                return selector_selected;

            return positions[0];
        }

        /// <summary>
        /// Called when the AI has to tribute for a synchro monster or ritual monster.
        /// </summary>
        /// <param name="cards">Available cards.</param>
        /// <param name="sum">Result of the operation.</param>
        /// <param name="min">Minimum cards.</param>
        /// <param name="max">Maximum cards.</param>
        /// <param name="mode">True for exact equal.</param>
        /// <returns></returns>
        public IList<ClientCard> OnSelectSum(IList<ClientCard> cards, int sum, int min, int max, long hint, bool mode)
        {
            Executor?.Scorer?.ClearCache();
            const long HINTMSG_RELEASE = 500;
            const long HINTMSG_SMATERIAL = 512;

            IList<ClientCard> selected = Executor.OnSelectSum(cards, sum, min, max, hint, mode);
            if (selected != null)
            {
                return selected;
            }

            if (hint == HINTMSG_RELEASE || hint == HINTMSG_SMATERIAL)
            {
                if (m_materialSelector != null)
                {
                    selected = m_materialSelector.Select(cards, min, max);
                }
                else
                {
                    switch (hint)
                    {
                        case HINTMSG_SMATERIAL:
                            selected = Executor.OnSelectSynchroMaterial(cards, sum, min, max);
                            break;
                        case HINTMSG_RELEASE:
                            selected = Executor.OnSelectRitualTribute(cards, sum, min, max);
                            break;
                    }
                }
                if (selected != null)
                {
                    int s1 = 0, s2 = 0;
                    foreach (ClientCard card in selected)
                    {
                        s1 += card.OpParam1;
                        s2 += (card.OpParam2 != 0) ? card.OpParam2 : card.OpParam1;
                    }
                    if ((mode && (s1 == sum || s2 == sum)) || (!mode && (s1 >= sum || s2 >= sum)))
                    {
                        return selected;
                    }
                }
            }

            if (mode)
            {
                // equal

                if (sum == 0 && min == 0)
                {
                    return new List<ClientCard>();
                }

                if (min <= 1)
                {
                    // try special level first
                    foreach (ClientCard card in cards)
                    {
                        if (card.OpParam2 == sum)
                        {
                            return new[] { card };
                        }
                    }
                    // try level equal
                    foreach (ClientCard card in cards)
                    {
                        if (card.OpParam1 == sum)
                        {
                            return new[] { card };
                        }
                    }
                }

                // try all
                int s1 = 0, s2 = 0;
                foreach (ClientCard card in cards)
                {
                    s1 += card.OpParam1;
                    s2 += (card.OpParam2 != 0) ? card.OpParam2 : card.OpParam1;
                }
                if (s1 == sum || s2 == sum)
                {
                    return cards;
                }

                // try all combinations
                int i = (min <= 1) ? 2 : min;
                while (i <= max && i <= cards.Count)
                {
                    IEnumerable<IEnumerable<ClientCard>> combos = CardContainer.GetCombinations(cards, i);

                    foreach (IEnumerable<ClientCard> combo in combos)
                    {
                        Log(LogLevel.Debug, "--");
                        s1 = 0;
                        s2 = 0;
                        foreach (ClientCard card in combo)
                        {
                            s1 += card.OpParam1;
                            s2 += (card.OpParam2 != 0) ? card.OpParam2 : card.OpParam1;
                        }
                        if (s1 == sum || s2 == sum)
                        {
                            return combo.ToList();
                        }
                    }
                    i++;
                }
            }
            else
            {
                // larger
                if (min <= 1)
                {
                    // try special level first
                    foreach (ClientCard card in cards)
                    {
                        if (card.OpParam2 >= sum)
                        {
                            return new[] { card };
                        }
                    }
                    // try level equal
                    foreach (ClientCard card in cards)
                    {
                        if (card.OpParam1 >= sum)
                        {
                            return new[] { card };
                        }
                    }
                }

                // try all combinations
                int i = (min <= 1) ? 2 : min;
                while (i <= max && i <= cards.Count)
                {
                    IEnumerable<IEnumerable<ClientCard>> combos = CardContainer.GetCombinations(cards, i);

                    foreach (IEnumerable<ClientCard> combo in combos)
                    {
                        Log(LogLevel.Debug, "----");
                        int s1 = 0, s2 = 0;
                        foreach (ClientCard card in combo)
                        {
                            s1 += card.OpParam1;
                            s2 += (card.OpParam2 != 0) ? card.OpParam2 : card.OpParam1;
                        }
                        if (s1 >= sum || s2 >= sum)
                        {
                            return combo.ToList();
                        }
                    }
                    i++;
                }
            }

            Log(LogLevel.Error, "Fail to select sum.");
            return new List<ClientCard>();
        }

        /// <summary>
        /// Called when the AI has to tribute one or more cards.
        /// </summary>
        /// <param name="cards">List of available cards.</param>
        /// <param name="min">Minimal quantity.</param>
        /// <param name="max">Maximal quantity.</param>
        /// <param name="hint">The hint message of the select.</param>
        /// <param name="cancelable">True if you can return an empty list.</param>
        /// <returns>A new list containing the tributed cards.</returns>
        public IList<ClientCard> OnSelectTribute(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            Executor?.Scorer?.ClearCache();
            IList<ClientCard> selected = Executor.OnSelectTribute(cards, min, max, hint, cancelable);
            if (selected != null)
                return selected;

            // Always choose the minimum and lowest atk.
            List<ClientCard> sorted = new List<ClientCard>();
            sorted.AddRange(cards);
            sorted.Sort(CardContainer.CompareCardAttack);

            IList<ClientCard> result = new List<ClientCard>();

            for (int i = 0; i < min && i < sorted.Count; ++i)
                result.Add(sorted[i]);

            return result;
        }

        /// <summary>
        /// Called when the AI has to select yes or no.
        /// </summary>
        /// <param name="desc">Id of the question.</param>
        /// <returns>True for yes, false for no.</returns>
        public bool OnSelectYesNo(long desc)
        {
            Executor?.Scorer?.ClearCache();
            if (m_yesno != -1)
                return m_yesno > 0;
            return Executor.OnSelectYesNo(desc);
        }

        /// <summary>
        /// Called when the AI has to select if to continue attacking when replay.
        /// </summary>
        /// <returns>True for yes, false for no.</returns>
        public bool OnSelectBattleReplay()
        {
            return Executor.OnSelectBattleReplay();
        }

        public bool OnSelectBattleDirectAttack(ClientCard attacker, bool preselectedAnswer)
        {
            return Executor.OnSelectBattleDirectAttack(attacker, preselectedAnswer);
        }

        public void OnHintZone(int player, int zone)
        {
            Executor.OnHintZone(player, zone);
        }

        public void OnMove(ClientCard card, int previousControler, int previousLocation, int currentControler, int currentLocation)
        {
            Executor.OnMove(card, previousControler, previousLocation, currentControler, currentLocation);
        }

        public void OnChainSolved(int chainIndex)
        {
            Executor.OnChainSolved(chainIndex);
        }

        /// <summary>
        /// Called when the AI has to declare a card.
        /// </summary>
        /// <returns>Id of the selected card.</returns>
        public int OnAnnounceCard()
        {
            int announced = m_announce;
            m_announce = 0;

            if (announced > 0)
            {
                NamedCard card = NamedCard.Get(announced);
                if (card != null && card.Alias > 0 && NamedCard.IsAltartAlias(card.Id, card.Alias))
                    return card.Alias;
                return announced;
            }

            // Fallback: If no card announced, choose a remaining card from bot's own deck to satisfy OCGCore filter
            if (Executor != null && Executor.StartingDeck != null && Executor.StartingDeck.Cards != null)
            {
                foreach (int id in Executor.StartingDeck.Cards)
                {
                    if (Executor.GetRemainingCount(id) > 0)
                    {
                        NamedCard card = NamedCard.Get(id);
                        if (card != null && card.Alias > 0 && NamedCard.IsAltartAlias(card.Id, card.Alias))
                            return card.Alias;
                        return id;
                    }
                }
            }

            if (Duel != null && Duel.Fields[0] != null && Duel.Fields[0].Deck.Count > 0)
            {
                var validDeckCard = Duel.Fields[0].Deck.FirstOrDefault(c => c != null && c.Id > 0);
                if (validDeckCard != null)
                    return validDeckCard.GetNonAltartCode();
            }

            return 89631139; // Blue-eyes white dragon
        }

        // _ Others functions _
        // Those functions are used by the AI behavior.

        
        private CardSelector m_materialSelector;
        private int m_place;
        private int m_option;
        private int m_number;
        private int m_announce;
        private int m_yesno;
        private IList<CardAttribute> m_attributes = new List<CardAttribute>();
        private IList<CardSelector> m_selector = new List<CardSelector>();
        private IList<CardPosition> m_position = new List<CardPosition>();
        private int m_selector_pointer = -1;
        private IList<CardRace> m_races = new List<CardRace>();

        public void SelectCard(ClientCard card)
        {
            m_selector_pointer = m_selector.Count();
            m_selector.Add(new CardSelector(card));
        }

        public void SelectCard(IList<ClientCard> cards)
        {
            m_selector_pointer = m_selector.Count();
            m_selector.Add(new CardSelector(cards));
        }

        public void SelectCard(int cardId)
        {
            m_selector_pointer = m_selector.Count();
            m_selector.Add(new CardSelector(cardId));
        }

        public void SelectCard(IList<int> ids)
        {
            m_selector_pointer = m_selector.Count();
            m_selector.Add(new CardSelector(ids));
        }

        public void SelectCard(params int[] ids)
        {
            m_selector_pointer = m_selector.Count();
            m_selector.Add(new CardSelector(ids));
        }

        public void SelectCard(CardLocation loc)
        {
            m_selector_pointer = m_selector.Count();
            m_selector.Add(new CardSelector(loc));
        }

        public void SelectNextCard(ClientCard card)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectNextCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(card));
        }

        public void SelectNextCard(IList<ClientCard> cards)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectNextCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(cards));
        }

        public void SelectNextCard(int cardId)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectNextCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(cardId));
        }

        public void SelectNextCard(IList<int> ids)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectNextCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(ids));
        }

        public void SelectNextCard(params int[] ids)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectNextCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(ids));
        }

        public void SelectNextCard(CardLocation loc)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectNextCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(loc));
        }

        public void SelectThirdCard(ClientCard card)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectThirdCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(card));
        }

        public void SelectThirdCard(IList<ClientCard> cards)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectThirdCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(cards));
        }

        public void SelectThirdCard(int cardId)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectThirdCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(cardId));
        }

        public void SelectThirdCard(IList<int> ids)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectThirdCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(ids));
        }

        public void SelectThirdCard(params int[] ids)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectThirdCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(ids));
        }

        public void SelectThirdCard(CardLocation loc)
        {
            if (m_selector_pointer == -1)
            {
                Log(LogLevel.Error, "Error: Call SelectThirdCard() before SelectCard()");
                m_selector_pointer = 0;
            }
            m_selector.Insert(m_selector_pointer, new CardSelector(loc));
        }

        public void SelectMaterials(ClientCard card)
        {
            m_materialSelector = new CardSelector(card);
        }

        public void SelectMaterials(IList<ClientCard> cards)
        {
            m_materialSelector = new CardSelector(cards);
        }

        public void SelectMaterials(int cardId)
        {
            m_materialSelector = new CardSelector(cardId);
        }

        public void SelectMaterials(IList<int> ids)
        {
            m_materialSelector = new CardSelector(ids);
        }

        public void SelectMaterials(CardLocation loc)
        {
            m_materialSelector = new CardSelector(loc);
        }

        public void CleanSelectMaterials()
        {
            m_materialSelector = null;
        }

        public CardSelector GetSelectedCards()
        {
            CardSelector selected = null;
            if (m_selector.Count > 0)
            {
                selected = m_selector[m_selector.Count - 1];
                m_selector.RemoveAt(m_selector.Count - 1);
            }
            return selected;
        }

        public CardPosition GetSelectedPosition()
        {
            CardPosition selected = CardPosition.FaceUpAttack;
            if (m_position.Count > 0)
            {
                selected = m_position[0];
                m_position.RemoveAt(0);
            }
            return selected;
        }

        public void SelectPosition(CardPosition pos)
        {
            m_position.Add(pos);
        }

        public void SelectPlace(int zones)
        {
            m_place = zones;
        }

        public void SelectOption(int opt)
        {
            m_option = opt;
        }

        public void SelectNumber(int number)
        {
            m_number = number;
        }

        public void SelectAttribute(CardAttribute attribute)
        {
            m_attributes.Clear();
            m_attributes.Add(attribute);
        }

        public void SelectAttributes(CardAttribute[] attributes)
        {
            m_attributes.Clear();
            foreach (CardAttribute attribute in attributes)
                m_attributes.Add(attribute);
        }

        public void SelectRace(CardRace race)
        {
            m_races.Clear();
            m_races.Add(race);
        }

        public void SelectRaces(CardRace[] races)
        {
            m_races.Clear();
            foreach (CardRace race in races)
                m_races.Add(race);
        }

        public void SelectAnnounceID(int id)
        {
            NamedCard card = NamedCard.Get(id);
            if (card != null && card.Alias > 0 && NamedCard.IsAltartAlias(card.Id, card.Alias))
            {
                m_announce = card.Alias;
            }
            else
            {
                m_announce = id;
            }
        }

        public void SelectYesNo(bool opt)
        {
            m_yesno = opt ? 1 : 0;
        }

        /// <summary>
        /// Called when the AI has to declare a number.
        /// </summary>
        /// <param name="numbers">List of available numbers.</param>
        /// <returns>Index of the selected number.</returns>
        public int OnAnnounceNumber(IList<int> numbers)
        {
            if (numbers.Contains(m_number))
                return numbers.IndexOf(m_number);

            return Executor.Rand.Next(0, numbers.Count); // Returns a random number.
        }

        /// <summary>
        /// Called when the AI has to declare one or more attributes.
        /// </summary>
        /// <param name="count">Quantity of attributes to declare.</param>
        /// <param name="attributes">List of available attributes.</param>
        /// <returns>A list of the selected attributes.</returns>
        public virtual IList<CardAttribute> OnAnnounceAttrib(int count, IList<CardAttribute> attributes)
        {
            IList<CardAttribute> foundAttributes = m_attributes.Where(attributes.Contains).ToList();
            if (foundAttributes.Count > 0)
                return foundAttributes;

            return attributes; // Returns the first available Attribute.
        }

        /// <summary>
        /// Called when the AI has to declare one or more races.
        /// </summary>
        /// <param name="count">Quantity of races to declare.</param>
        /// <param name="races">List of available races.</param>
        /// <returns>A list of the selected races.</returns>
        public virtual IList<CardRace> OnAnnounceRace(int count, IList<CardRace> races)
        {
            IList<CardRace> foundRaces = m_races.Where(races.Contains).ToList();
            if (foundRaces.Count > 0)
                return foundRaces;

            return races; // Returns the first available Races.
        }

        public BattlePhaseAction Attack(ClientCard attacker, ClientCard defender)
        {
            Executor.SetCard(0, attacker, -1);
            if (defender != null)
            {
                string cardName = defender.Name ?? "monster";
                attacker.ShouldDirectAttack = false;
                _dialogs.SendAttack(attacker.Name, cardName);
                SelectCard(defender);
            }
            else
            {
                attacker.ShouldDirectAttack = true;
                _dialogs.SendDirectAttack(attacker.Name);
            }
            return new BattlePhaseAction(BattlePhaseAction.BattleAction.Attack, attacker.ActionIndex);
        }

        public BattlePhaseAction ToEndPhase()
        {
            _dialogs.SendEndTurn();
            return new BattlePhaseAction(BattlePhaseAction.BattleAction.ToEndPhase);
        }
        public BattlePhaseAction ToMainPhase2()
        {
            return new BattlePhaseAction(BattlePhaseAction.BattleAction.ToMainPhaseTwo);
        }

        private bool ShouldExecute(CardExecutor exec, ClientCard card, ExecutorType type, long desc = -1)
        {
            Executor.SetCard(type, card, desc);
            if (card == null || exec.Type != type)
                return false;
            if (exec.CardId != -1 && exec.CardId != card.Id)
                return false;

            // Universal Central Core Guard for Counter Traps:
            // Prevents self-negation, duplicate chaining in same chain, and paying LP costs redundantly
            if (type == ExecutorType.Activate && card.HasType(CardType.Counter))
            {
                // 1. Never chain a Counter Trap to our own card's activation
                if (Duel.LastChainPlayer == 0)
                    return false;

                // 2. Never negate our own inherent summon
                if (Duel.LastChainPlayer == -1 && Duel.LastSummonPlayer == 0)
                    return false;

                // 3. Never double-chain the exact same Counter Trap in the same chain (prevents duplicate LP costs)
                if (Duel.CurrentChain != null && Duel.CurrentChain.Count > 0 && Duel.CurrentChain.Any(c => c != null && c.IsCode(card.Id)))
                    return false;

                // 4. Never target our own card on top of the chain
                ClientCard lastCard = Duel.CurrentChain?.LastOrDefault();
                if (lastCard != null && lastCard.Controller == 0)
                    return false;
            }

            // Universal Central Core Guard for Handtraps & Targeted Negators:
            // Prevents duplicate activation of the same handtrap/negator in the same chain (e.g. Imperm -> Imperm, Ash -> Ash)
            // and prevents wasting negations on already disabled targets.
            if (type == ExecutorType.Activate && Duel.CurrentChain != null && Duel.CurrentChain.Count > 0)
            {
                if (card.IsCode(10045474) || card.IsCode(97268402) || CardIntelligence.IsHandtrap(card.Id) || CardIntelligence.IsHandtrap(card.GetNonAltartCode()))
                {
                    // 1. Never activate the same handtrap/negator twice in the same chain
                    if (Duel.CurrentChain.Any(c => c != null && (c.IsCode(card.Id) || c.IsCode(card.GetNonAltartCode()))))
                        return false;

                    // 2. Never chain targeted monster negators (Imperm/Veiler) to our own card's activation
                    if ((card.IsCode(10045474) || card.IsCode(97268402)) && Duel.LastChainPlayer == 0)
                        return false;

                    // 3. Never chain targeted monster negator to an opponent monster that is ALREADY disabled
                    if (card.IsCode(10045474) || card.IsCode(97268402))
                    {
                        ClientCard targetCard = Duel.CurrentChain.LastOrDefault();
                        if (targetCard != null && targetCard.Controller == 1 && targetCard.Location == CardLocation.MonsterZone && targetCard.IsDisabled())
                            return false;
                    }
                }
            }

            bool result = exec.Func == null || exec.Func();

            // Layer 1: Auto-trace every executor decision for all decks
            try
            {
                if (WindBot.DecisionTracer.Enabled && result)
                {
                    string funcName = exec.Func?.Method?.Name ?? "default";
                    string cardName = card.Name ?? $"id:{card.Id}";
                    WindBot.DecisionTracer.WriteTrace?.Invoke($"[TRACE][{type}] ✓ {cardName} ({card.Id}) → {funcName} from {card.Location}");
                }
            }
            catch { }

            return result;
        }
    }
}
