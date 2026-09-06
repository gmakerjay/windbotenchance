using System;
using System.Collections.Generic;
using System.Linq;
using YGOSharp.OCGWrapper;
using YGOSharp.OCGWrapper.Enums;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI
{
    public abstract class Executor
    {
        public string Deck { get; set; }
        public Duel Duel { get; private set; }
        public IList<CardExecutor> Executors { get; private set; }
        public GameAI AI { get; private set; }
        public AIUtil Util { get; private set; }

        protected MainPhase Main { get; private set; }
        protected BattlePhase Battle { get; private set; }

        protected ExecutorType Type { get; private set; }
        protected ClientCard Card { get; private set; }
        protected long ActivateDescription { get; private set; }

        public ClientField Bot { get; private set; }
        public ClientField Enemy { get; private set; }

        public Random Rand;

        // ── Situational Awareness (AIBrain) ──
        public AIBrain Brain { get; private set; }

        // ── Combat/Threat Calculation Engine ──
        public BoardScorer Scorer { get; private set; }

        // ── Optional modern analysis plugin layer ──
        public AiAnalysisSuite Analysis { get; private set; }

        // ── Core Decision Flags (computed in OnNewTurn by DefaultExecutor) ──
        // Set true when board state guarantees lethal this turn — skip combo, push for game.
        public bool ShouldRushAttack { get; set; }
        // Set true when lethal is assured — skip searching/combo extenders.
        public bool SkipComboSearch { get; set; }
        // Set true on Turn 2 going-second when we have break-board tools vs opponent's board.
        public bool ShouldGoBreakBoard { get; set; }
        // True when currently in Main Phase 2 — spells/traps can be safely set.
        public bool InMainPhase2 { get; set; }

        protected Executor(GameAI ai, Duel duel)
        {
            Rand = new Random();
            Duel = duel;
            AI = ai;
            Util = new AIUtil(duel);
            Executors = new List<CardExecutor>();
            Brain = new AIBrain(this);
            Scorer = new BoardScorer();

            Bot = Duel.Fields[0];
            Enemy = Duel.Fields[1];
            Analysis = new AiAnalysisSuite(this);
        }

        public virtual int OnRockPaperScissors()
        {
            return Rand.Next(1, 4);
        }

        public virtual bool OnSelectHand()
        {
            return Rand.Next(2) > 0;
        }

        /// <summary>
        /// Called when the AI has to decide if it should attack
        /// </summary>
        /// <param name="attackers">List of monsters that can attcack.</param>
        /// <param name="defenders">List of monsters of enemy.</param>
        /// <returns>A new BattlePhaseAction containing the action to do.</returns>
        public virtual BattlePhaseAction OnBattle(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            // For overriding
            return null;
        }

        public virtual MainPhaseAction OnSelectIdleCmd(MainPhase main)
        {
            // For overriding
            return null;
        }

        public virtual BattlePhaseAction OnSelectBattleCmd(BattlePhase battle)
        {
            // For overriding
            return null;
        }

        /// <summary>
        /// Called when the AI has to decide which card to attack first
        /// </summary>
        /// <param name="attackers">List of monsters that can attcack.</param>
        /// <param name="defenders">List of monsters of enemy.</param>
        /// <returns>The card to attack first.</returns>
        public virtual ClientCard OnSelectAttacker(IList<ClientCard> attackers, IList<ClientCard> defenders)
        {
            // For overriding
            return null;
        }

        public virtual BattlePhaseAction OnSelectAttackTarget(ClientCard attacker, IList<ClientCard> defenders)
        {
            // Overrided in DefalultExecutor
            return null;
        }

        public virtual bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            // Overrided in DefalultExecutor
            return true;
        }

        public virtual void OnChaining(int player, ClientCard card)
        {
            // For overriding
        }

        public virtual void OnChainSolved(int chainIndex)
        {
            // For overriding
        }

        public virtual void OnChainEnd()
        {
            // For overriding
        }
        public void PreNewTurn()
        {
            Brain?.OnNewTurn();
            Scorer?.Reset(Bot, Enemy, Duel);
            Analysis?.Refresh();

            // ── Reset core decision flags ──
            ShouldRushAttack = false;
            SkipComboSearch = false;
            ShouldGoBreakBoard = false;
            InMainPhase2 = false;

            // ── Compute Easy Lethal (accurate — considers blockers) ──
            if (Duel.Player == 0 && Scorer != null && Scorer.HasLethal())
            {
                ShouldRushAttack = true;
                SkipComboSearch = true;

                // [LETHAL-CHECK] Log the diagnostic values used for this decision
                try
                {
                    int fieldATK = 0;
                    foreach (var m in Bot.GetMonsters())
                    {
                        if (m != null && m.IsFaceup() && m.IsAttack())
                            fieldATK += m.Attack;
                    }
                    AI?.Log(LogLevel.Info,
                        $"[LETHAL-CHECK] ✓ RUSH MODE | Field ATK: {fieldATK} | Enemy LP: {Enemy.LifePoints} | " +
                        $"Enemy Monsters: {Enemy.GetMonsterCount()} | HasLethal(Scorer): {Scorer.HasLethal()}");
                }
                catch { /* logging must never crash the bot */ }
            }

            // ── Compute Going-Second BreakBoard EV ──
            if (Duel.Turn == 2 && Duel.Player == 0 && Enemy?.GetMonsterCount() > 0)
            {
                int enemyDisruption = Enemy.GetMonsterCount()
                    + Enemy.GetSpellCount()
                    + Enemy.GetMonsters().Count(m => m != null && m.IsFaceup() && m.Attack >= 2500);

                int breakers = 0;
                foreach (ClientCard c in Bot.Hand)
                {
                    if (c == null) continue;
                    // Common hand traps (count as breakers for Going-Second assessment)
                    if (c.Id == 23434538       // Maxx "C"
                        || c.Id == 94145021   // Droll & Lock Bird
                        || c.Id == 14558127   // Ash Blossom (original)
                        || c.Id == 14558128   // Ash Blossom (alt art)
                        || c.Id == 42141493   // Mulcharmy Fuwalos
                        || c.Id == 84192580   // Mulcharmy Purulia
                        || c.Id == 73642296   // Ghost Belle
                        || c.Id == 63845230   // Effect Veiler
                        || c.Id == 59438930)  // Ghost Ogre
                        breakers++;
                }

                ShouldGoBreakBoard = (breakers >= 1 && breakers * 2 >= enemyDisruption);
            }
        }

        public void PreNewPhase()
        {
            InMainPhase2 = (Duel.Player == 0 && Duel.Phase == DuelPhase.Main2);
            Brain?.OnNewPhase();
            Analysis?.Refresh();
        }

        public void PreChaining(int player, ClientCard card)
        {
            Brain?.OnChain(player, card);
        }

        public virtual void OnNewPhase()
        {
            // Some AI need do something on new phase
        }
        public virtual void OnNewTurn()
        {
            // Some AI need do something on new turn
        }
		
        public virtual void OnDraw(int player)
        {
            // Some AI need do something on draw
        }

        public virtual IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // For overriding
            return null;
        }

        /// <summary>
        /// Universal intelligent fallback when neither the deck executor nor any CardSelector chose targets.
        /// Replaces legacy blind selection (cards[0]) with heuristic evaluation.
        /// Protects own Ace/high-ATK cards on sacrifices, and prioritizes highest threat targets on enemy cards.
        /// Guaranteed safe: never throws unhandled exceptions.
        /// </summary>
        public virtual IList<ClientCard> FallbackSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0) return new List<ClientCard>();

            try
            {
                var validCards = cards.Where(c => c != null).ToList();
                if (validCards.Count < min) return validCards;

                var enemyCards = validCards.Where(c => c.Controller == 1).ToList();
                var ourCards = validCards.Where(c => c.Controller == 0).ToList();

                // ── Case 1: Enemy Target Selection (Destroy, Banish, Return to hand/deck, Target, Negate, Attack Target) ──
                // Hints: 502 (DESTROY), 504 (REMOVE/BANISH), 505 (RTOHAND), 506 (TODECK), 519 (CONTROL), 549 (ATTACK), 551 (TARGET), 552 (DISABLE), 572 (NEGATE), 575 (FACEUP)
                if (hint == 502 || hint == 504 || hint == 505 || hint == 506 || hint == 519 ||
                    hint == 549 || hint == 551 || hint == 552 || hint == 572 || hint == 575)
                {
                    if (enemyCards.Count >= min)
                    {
                        var targetable = enemyCards.Where(c => !c.IsShouldNotBeTarget()).ToList();
                        var candidates = targetable.Count >= min ? targetable : enemyCards;
                        var sorted = candidates.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : (c.Attack + (c.IsExtraCard() ? 2000 : 0))).ToList();
                        return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                    }
                }

                // ── Case 2: Sacrifice / Cost Selection from our side (Tribute, Discard, Send to GY, Materials) ──
                // Hints: 500 (RELEASE), 501 (DISCARD), 508 (TOGRAVE), 511-513 (MATERIALS), 533 (LMATERIAL)
                if (hint == 500 || hint == 501 || hint == 508 || hint == 511 || hint == 512 || hint == 513 || hint == 533)
                {
                    if (ourCards.Count >= min)
                    {
                        var sorted = ourCards.OrderBy(c => {
                            int cost = 0;
                            if (IsAceCard(c)) cost += 100000;
                            if (c.IsExtraCard() && c.Location == CardLocation.MonsterZone) cost += 50000;
                            cost += c.Attack;
                            return cost;
                        }).ToList();
                        return sorted.Take(min).ToList();
                    }
                }

                // ── Case 3: Positive Selection for our side (Special Summon, Add to Hand, Equip) ──
                // Hints: 509 (SPSUMMON), 507 (EQUIP), or Search where only our cards exist
                if (hint == 509 || hint == 507 || (hint == 505 && enemyCards.Count == 0))
                {
                    if (ourCards.Count >= min)
                    {
                        var sorted = ourCards.OrderByDescending(c => {
                            int value = 0;
                            if (IsAceCard(c)) value += 10000;
                            if (c.IsExtraCard()) value += 5000;
                            if (c.HasType(CardType.Monster)) value += 1000 + c.Attack;
                            return value;
                        }).ToList();
                        return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                    }
                }

                // ── Case 4: General Fallbacks by card ownership ──
                if (enemyCards.Count == validCards.Count)
                {
                    var sorted = enemyCards.OrderByDescending(c => Scorer != null ? Scorer.ThreatScore(c) : c.Attack).ToList();
                    return sorted.Take(Math.Min(max, sorted.Count)).ToList();
                }

                if (ourCards.Count == validCards.Count)
                {
                    if (validCards.Any(c => c.Location == CardLocation.Hand || c.Location == CardLocation.MonsterZone))
                    {
                        var sorted = ourCards.OrderBy(c => IsAceCard(c) ? 100000 : c.Attack).ToList();
                        return sorted.Take(min).ToList();
                    }
                    var searchSorted = ourCards.OrderByDescending(c => (IsAceCard(c) ? 10000 : 0) + c.Attack).ToList();
                    return searchSorted.Take(Math.Min(max, searchSorted.Count)).ToList();
                }
            }
            catch { /* Guard against any engine-level anomaly */ }

            return cards.Take(min).ToList();
        }

        public virtual IList<ClientCard> OnSelectTribute(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // For overriding
            return null;
        }

        public virtual IList<ClientCard> OnSelectSum(IList<ClientCard> cards, int sum, int min, int max, long hint, bool mode)
        {
            // For overriding
            return null;
        }

        public virtual IList<ClientCard> OnSelectSum(IList<ClientCard> cards, IList<ClientCard> mandatoryCards, int sum, int min, int max, long hint, bool exactEqual)
        {
            // For overriding
            return null;
        }

        public virtual IList<ClientCard> OnSelectFusionMaterial(IList<ClientCard> cards, int min, int max)
        {
            // For overriding
            return null;
        }

        public virtual IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, int sum, int min, int max)
        {
            // For overriding
            return null;
        }

        public virtual IList<ClientCard> OnSelectSynchroMaterial(IList<ClientCard> cards, IList<ClientCard> mandatoryCards, int sum, int min, int max)
        {
            // For overriding
            return null;
        }

        public virtual IList<ClientCard> OnSelectXyzMaterial(IList<ClientCard> cards, int min, int max)
        {
            // For overriding
            return null;
        }

        public virtual IList<ClientCard> OnSelectLinkMaterial(IList<ClientCard> cards, int min, int max)
        {
            // For overriding
            return null;
        }

        public virtual IList<ClientCard> OnSelectRitualTribute(IList<ClientCard> cards, int sum, int min, int max)
        {
            // For overriding
            return null;
        }

        public virtual IList<ClientCard> OnSelectRitualTribute(IList<ClientCard> cards, IList<ClientCard> mandatoryCards, int sum, int min, int max, bool exactEqual)
        {
            // For overriding
            return null;
        }

        public virtual IList<ClientCard> OnSelectPendulumSummon(IList<ClientCard> cards, int max)
        {
            // For overriding
            return null;
        }

        public virtual IList<ClientCard> OnCardSorting(IList<ClientCard> cards)
        {
            // For overriding
            return null;
        }

        public virtual void OnSelectChain(IList<ClientCard> cards)
        {
            return;
        }

        public virtual bool OnSelectYesNo(long desc)
        {
            return true;
        }

        public virtual bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            return null;
        }

        public virtual int OnSelectOption(IList<long> options)
        {
            return -1;
        }

        public virtual int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            if (available <= 0) return 0;

            try
            {
                // If only 1 zone available, take it immediately
                if ((available & (available - 1)) == 0)
                    return available;

                if (player == 0 && location == CardLocation.SpellZone)
                {
                    // Evaluate available spell zones (z0..z4 = 0x1, 0x2, 0x4, 0x8, 0x10)
                    // Column safety: Avoid columns where opponent has set spells/traps (Impermanence hazard)
                    int dangerousZones = 0;
                    if (Enemy != null)
                    {
                        var enemySpells = Enemy.GetSpells();
                        for (int s = 0; s < 5; s++)
                        {
                            // If enemy has a set card in spell zone s, the opposite player 0 zone is (4 - s)
                            if (s < enemySpells.Count && enemySpells[s] != null && enemySpells[s].IsFacedown())
                            {
                                dangerousZones |= (1 << (4 - s));
                            }
                        }
                    }

                    int safeAvailable = available & ~dangerousZones;
                    if (safeAvailable > 0)
                    {
                        // Preference order: center (z2=0x4) -> middle-sides (z1=0x2, z3=0x8) -> edges (z0=0x1, z4=0x10)
                        int[] preference = { 0x4, 0x2, 0x8, 0x1, 0x10 };
                        foreach (int z in preference)
                        {
                            if ((safeAvailable & z) > 0)
                                return z;
                        }
                        return safeAvailable;
                    }
                }
                else if (player == 0 && location == CardLocation.MonsterZone)
                {
                    // Extra Monster Zone check: z5 (0x20), z6 (0x40)
                    NamedCard card = NamedCard.Get((int)cardId);
                    bool isExtra = card != null && card.IsExtraCard();

                    if (isExtra && (available & 0x60) > 0)
                    {
                        if ((available & 0x20) > 0) return 0x20;
                        if ((available & 0x40) > 0) return 0x40;
                    }

                    // Main Monster Zones (z0..z4):
                    // Prefer z2 (center=0x4) or edges (z0=0x1, z4=0x10) over columns directly under EMZ (z1=0x2, z3=0x8)
                    int[] mmzPreference = { 0x4, 0x1, 0x10, 0x2, 0x8 };
                    foreach (int z in mmzPreference)
                    {
                        if ((available & z) > 0)
                            return z;
                    }
                }
            }
            catch { /* Guard against any engine-level anomaly */ }

            return 0;
        }

        public virtual uint OnSelectDisfield(long hint, int count, uint available)
        {
            // For overriding
            return 0u;
        }

        public virtual void OnHintZone(int player, int zone)
        {
            // For overriding
        }

        public virtual void OnMove(ClientCard card, int previousControler, int previousLocation, int currentControler, int currentLocation)
        {
            // For overriding
        }

        public virtual void OnSpSummoning()
        {
            // For overriding
        }

        public virtual void OnSummoning()
        {
            // For overriding
        }

        public virtual CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            try
            {
                NamedCard cardData = NamedCard.Get(cardId);
                if (cardData != null)
                {
                    // 0 ATK monsters should always defend if possible
                    if (cardData.Attack == 0 && positions.Contains(CardPosition.FaceUpDefence))
                        return CardPosition.FaceUpDefence;

                    // High ATK (>= 1800) or Extra Deck monsters -> FaceUpAttack
                    if ((cardData.IsExtraCard() || cardData.Attack >= 1800) && positions.Contains(CardPosition.FaceUpAttack))
                        return CardPosition.FaceUpAttack;

                    // Low ATK / combo pieces / hand traps (ATK < 1500 and Defense > 0) -> FaceUpDefence
                    if (cardData.Defense > 0 && cardData.Attack < 1500 && positions.Contains(CardPosition.FaceUpDefence))
                    {
                        // If in Battle Phase pushing for lethal with attack > 0, allow Attack
                        if (Duel != null && (Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.BattleStart) && cardData.Attack > 0)
                            return CardPosition.FaceUpAttack;

                        return CardPosition.FaceUpDefence;
                    }
                }
            }
            catch { /* Guard against any engine-level anomaly */ }

            return positions[0];
        }

        public virtual bool OnSelectBattleReplay()
        {
            // Overrided in DefaultExecutor
            return false;
        }

        /// <summary>
        /// Called when the AI has to decide whether the pending attack should be a direct attack.
        /// </summary>
        /// <param name="attacker">The monster selected to attack.</param>
        /// <param name="preselectedAnswer">Whether the preceding battle decision preselected a direct attack.</param>
        public virtual bool OnSelectBattleDirectAttack(ClientCard attacker, bool preselectedAnswer)
        {
            return preselectedAnswer;
        }

        public void SetMain(MainPhase main)
        {
            Main = main;
            _guardLogDedup.Clear(); // Reset guard dedup for this idle command cycle
        }

        // Dedup set for [PHASE-GUARD] logging — prevents same card being logged
        // multiple times per InternalOnSelectIdleCmd call (due to CardExecutor loop).
        protected readonly System.Collections.Generic.HashSet<string> _guardLogDedup
            = new System.Collections.Generic.HashSet<string>();

        public void SetBattle(BattlePhase battle)
        {
            Battle = battle;
        }

        /// <summary>
        /// Set global variables Type, Card, ActivateDescription for Executor
        /// </summary>
        public void SetCard(ExecutorType type, ClientCard card, long description)
        {
            Type = type;
            Card = card;
            ActivateDescription = description;
        }

        /// <summary>
        /// Do the action for the card if func return true.
        /// </summary>
        public void AddExecutor(ExecutorType type, int cardId, Func<bool> func)
        {
            Executors.Add(new CardExecutor(type, cardId, func));
        }

        /// <summary>
        /// Do the action for the card if available.
        /// </summary>
        public void AddExecutor(ExecutorType type, int cardId)
        {
            Executors.Add(new CardExecutor(type, cardId, null));
        }

        /// <summary>
        /// Do the action for every card if func return true.
        /// </summary>
        public void AddExecutor(ExecutorType type, Func<bool> func)
        {
            Executors.Add(new CardExecutor(type, -1, func));
        }

        /// <summary>
        /// Do the action for every card if no other Executor is added to it.
        /// </summary>
        public void AddExecutor(ExecutorType type)
        {
            Executors.Add(new CardExecutor(type, -1, DefaultNoExecutor));
        }

        private bool DefaultNoExecutor()
        {
            return Executors.All(exec => exec.Type != Type || exec.CardId != Card.Id);
        }

        private Deck _loadedDeck;
        public Deck StartingDeck
        {
            get
            {
                if (_loadedDeck == null && !string.IsNullOrEmpty(Deck))
                {
                    _loadedDeck = WindBot.Game.Deck.Load(Deck);
                }
                return _loadedDeck;
            }
        }

        public int GetRemainingCount(int cardId)
        {
            if (StartingDeck == null) return 0;
            int originId = cardId;
            NamedCard nCard = NamedCard.Get(cardId);
            if (nCard != null && nCard.Alias > 0 && NamedCard.IsAltartAlias(nCard.Id, nCard.Alias))
                originId = nCard.Alias;

            int initialCount = StartingDeck.Cards.Concat(StartingDeck.ExtraCards).Count(id => {
                if (id == originId) return true;
                NamedCard c = NamedCard.Get(id);
                return c != null && c.Alias > 0 && NamedCard.IsAltartAlias(c.Id, c.Alias) && c.Alias == originId;
            });
            return Bot.GetRemainingCount(originId, initialCount);
        }

        public bool CardHasSetcode(YGOSharp.OCGWrapper.Card card, int setcode)
        {
            if (card == null) return false;
            long setcodes = card.Setcode;
            int settype = setcode & 0xfff;
            int setsubtype = setcode & 0xf000;
            while (setcodes > 0)
            {
                long check_setcode = setcodes & 0xffff;
                setcodes >>= 16;
                if ((check_setcode & 0xfff) == settype && (check_setcode & 0xf000 & setsubtype) == setsubtype) return true;
            }
            return false;
        }

        public bool HasRemainingCardWithSetcode(int setcode)
        {
            if (StartingDeck == null) return false;
            foreach (int cardId in StartingDeck.Cards)
            {
                var card = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
                if (card != null && CardHasSetcode(card, setcode))
                {
                    if (GetRemainingCount(cardId) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasRemainingMonsterWithSetcode(int setcode)
        {
            if (StartingDeck == null) return false;
            foreach (int cardId in StartingDeck.Cards)
            {
                var card = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
                if (card != null && card.HasType(CardType.Monster) && CardHasSetcode(card, setcode))
                {
                    if (GetRemainingCount(cardId) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasRemainingRitualMonster()
        {
            if (StartingDeck == null) return false;
            foreach (int cardId in StartingDeck.Cards)
            {
                var card = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
                if (card != null && card.HasType(CardType.Ritual) && card.HasType(CardType.Monster))
                {
                    if (GetRemainingCount(cardId) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasRemainingLevel1Monster()
        {
            if (StartingDeck == null) return false;
            foreach (int cardId in StartingDeck.Cards)
            {
                var card = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
                if (card != null && card.Level == 1 && card.HasType(CardType.Monster))
                {
                    if (GetRemainingCount(cardId) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Override in deck executors to mark boss/ace monsters for protection.
        /// Default: treats monsters with ATK ≥ 2500 or Extra Deck types as ace-worthy.
        /// </summary>
        public virtual bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            if (card.Attack >= 2500) return true;
            if (card.HasType(CardType.Fusion) || card.HasType(CardType.Synchro) ||
                card.HasType(CardType.Xyz) || card.HasType(CardType.Link))
                return true;
            return false;
        }

        /// <summary>
        /// Priority for selecting material (Link/Xyz/Synchro/Tribute).
        /// Lower = more expendable. Decks override to protect their specific ace cards.
        /// </summary>
        public virtual int GetMaterialPriority(ClientCard c)
        {
            if (c == null) return 999;
            if (IsAceCard(c)) return 800;
            if (c.IsExtraCard()) return 100;
            if (c.Level <= 2) return 200;
            return 300;
        }

        // ═══════════════════════════════════════════════════════════════
        //  Smart Phase Strategy — Core Guard Methods
        //  Called by GameAI.InternalOnSelectIdleCmd before each action.
        //  Override in ModernExecutor for phase-aware decision gating.
        //  Legacy executors (DefaultExecutor) are completely unaffected.
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Called before allowing a Special Summon (Extra Deck or Main Deck SS).
        /// Return false to skip (e.g., Extra Deck summon would reduce ATK below lethal).
        /// </summary>
        public virtual bool ShouldAllowSpSummon(ClientCard card) => true;

        /// <summary>
        /// Called before allowing a Spell/Trap Set.
        /// Return false to defer (e.g., should attack first in MP1, set in MP2).
        /// </summary>
        public virtual bool ShouldAllowSpellSet(ClientCard card) => true;

        /// <summary>
        /// Called before allowing a Normal Summon.
        /// Return false to skip (e.g., summoning ATK 300 when lethal is confirmed).
        /// </summary>
        public virtual bool ShouldAllowSummon(ClientCard card) => true;

        /// <summary>
        /// Called before allowing an effect Activation during idle command.
        /// Return false to defer (e.g., non-combat search when lethal is ready).
        /// Note: Hand traps / quick effects during opponent's turn bypass this check.
        /// </summary>
        public virtual bool ShouldAllowActivate(ClientCard card) => true;
    }
}
