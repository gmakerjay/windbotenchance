using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Plugin;
using WindBot.Game.AI.Plugins;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    // ====================================================================================================
    //  Raioh — ANTI-META STUN & ABSOLUTE OPPONENT LOCKDOWN EXECUTOR
    //  Clean Deck Name: Raioh
    //  Decoupled Plugin: RaiohPlugin
    // ====================================================================================================
    //
    //  Strategy Overview:
    //  1. Multi-Dimensional Lockdown:
    //     - Inspector Boarder (15397015): Freezes all monster effects on field/hand/GY (0 activations).
    //     - Thunder King Rai-Oh (71564252): Complete search denial (no card added from Deck) + inherent SS negate.
    //     - Barrier Statue of the Heavens (46145256): 100% Special Summon lockdown for all non-LIGHT monsters.
    //     - Banisher of the Radiance (94853057): Continuous Macro Cosmos on legs (all cards banished instead of GY).
    //  2. Time-Tearing Morganite Engine:
    //     - Draw 2 cards per turn + conduct TWO Normal Summons per turn with zero downside.
    //     - Sets up multiple oppressive floodgate monsters on Turn 1.
    //  3. Invincible Battle & Floodgate Protection:
    //     - Moon Mirror Shield (19508728): Guarantees our floodgates CANNOT be defeated in battle (ATK/DEF + 100).
    //     - Solemn Judgment (41420027): Omni-negate to protect entire backrow from Feather Duster / Lightning Storm.
    //     - Solemn Strike (40605147) & Solemn Warning (84749824): High-speed counter negations.
    //     - Destructive Daruma Karma Cannon (30748475): Mass face-down flip + removes Links.
    //     - Skill Drain (82732705), Macro Cosmos (30241314), There Can Be Only One (24207889), Necrovalley (47355498).
    // ====================================================================================================

    [Deck("Raioh", "Raioh")]
    [Deck("RaiOh", "Raioh")]
    [Deck("2026_RaiOh", "Raioh")]
    [Deck("Expert_2026_RaiOh", "Raioh")]
    public class RaiohExecutor : ModernExecutor
    {
        public class CardId
        {
            // --- MAIN DECK MONSTERS ---
            public const int InspectorBoarder = 15397015;
            public const int BarrierStatueOfTheHeavens = 46145256;
            public const int ThunderKingRaiOh = 71564252;
            public const int BanisherOfTheRadiance = 94853057;

            // --- SPELLS ---
            public const int TimeTearingMorganite = 19403423;
            public const int MoonMirrorShield = 19508728;
            public const int PotOfDuality = 98645731;
            public const int PotOfExtravagance = 49238328;
            public const int Necrovalley = 47355498;
            public const int DimensionalFissure = 81674782;

            // --- TRAPS ---
            public const int DestructiveDarumaKarmaCannon = 30748475;
            public const int SolemnStrike = 40605147;
            public const int SolemnWarning = 84749824;
            public const int SolemnJudgment = 41420027;
            public const int Crackdown = 36975314;
            public const int SkillDrain = 82732705;
            public const int AntiSpellFragrance = 58921041;
            public const int MacroCosmos = 30241314;
            public const int ThereCanBeOnlyOne = 24207889;

            // --- EXTRA DECK ---
            public const int Garura = 11765832;
            public const int MudragonOfTheSwamp = 54757758;
            public const int SuperStarslayerTYPHON = 93039339;
            public const int StarvingVenom = 41209827;
            public const int PredaplantDragostapelia = 69946549;
            public const int SPLittleKnight = 29301450;
            public const int KnightmarePhoenix = 2857636;
            public const int KnightmareCerberus = 75452921;
        }

        private readonly RaiohPlugin _plugin;
        private int _summonCountThisTurn = 0;
        private bool _extravaganceUsedThisTurn = false;

        public override bool IsAceCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == CardId.InspectorBoarder 
                || card.Id == CardId.ThunderKingRaiOh 
                || card.Id == CardId.BarrierStatueOfTheHeavens
                || card.Id == CardId.BanisherOfTheRadiance;
        }

        public RaiohExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _plugin = new RaiohPlugin(this);
            DeckPlugin = _plugin;

            // Register High Value Targets & Bait Starters
            BaitPlanner.RegisterComboStarters(CardId.InspectorBoarder, CardId.ThunderKingRaiOh, CardId.BarrierStatueOfTheHeavens);
            BaitPlanner.RegisterBaitCards(CardId.PotOfDuality, CardId.PotOfExtravagance);
            ChainAdvisor.RegisterHighValueTargets(CardId.InspectorBoarder, CardId.ThunderKingRaiOh, CardId.BarrierStatueOfTheHeavens);

            // ==========================================
            // PRIORITY 1: HIGH-SPEED COUNTER TRAPS (Spell Speed 3)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, SolemnStrikeEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnWarning, SolemnWarningEffect);

            // ==========================================
            // PRIORITY 2: DISRUPTIONS & INHERENT NEGATES
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.DestructiveDarumaKarmaCannon, DarumaCannonEffect);
            AddExecutor(ExecutorType.Activate, CardId.Crackdown, CrackdownEffect);
            AddExecutor(ExecutorType.Activate, CardId.ThunderKingRaiOh, RaiOhNegateEffect);

            // ==========================================
            // PRIORITY 3: CONTINUOUS FLOODGATE TRAPS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainEffect);
            AddExecutor(ExecutorType.Activate, CardId.AntiSpellFragrance, AntiSpellFragranceEffect);
            AddExecutor(ExecutorType.Activate, CardId.ThereCanBeOnlyOne, TcbooEffect);
            AddExecutor(ExecutorType.Activate, CardId.MacroCosmos, MacroCosmosEffect);

            // ==========================================
            // PRIORITY 4: DRAW & ACCELERATION SPELLS (MP1 START)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance, PotOfExtravaganceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TimeTearingMorganite, MorganiteEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfDuality, PotOfDualityEffect);

            // ==========================================
            // PRIORITY 5: FIELD & CONTINUOUS SPELLS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.DimensionalFissure, DimensionalFissureEffect);
            AddExecutor(ExecutorType.Activate, CardId.Necrovalley, NecrovalleyEffect);

            // ==========================================
            // PRIORITY 6: NORMAL SUMMONS (Strict Sequencing)
            // ==========================================
            AddExecutor(ExecutorType.Summon, CardId.InspectorBoarder, BoarderSummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.ThunderKingRaiOh, RaiOhSummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.BarrierStatueOfTheHeavens, BarrierStatueSummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.BanisherOfTheRadiance, BanisherSummonCheck);

            // Fallback Normal Summons
            AddExecutor(ExecutorType.Summon, CardId.InspectorBoarder);
            AddExecutor(ExecutorType.Summon, CardId.ThunderKingRaiOh);
            AddExecutor(ExecutorType.Summon, CardId.BarrierStatueOfTheHeavens);
            AddExecutor(ExecutorType.Summon, CardId.BanisherOfTheRadiance);

            // ==========================================
            // PRIORITY 7: EQUIP SPELL (Moon Mirror Shield)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.MoonMirrorShield, MoonMirrorShieldEffect);

            // ==========================================
            // PRIORITY 8: EXTRA DECK EMERGENCIES
            // ==========================================
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHON, TyphonSummonCheck);
            AddExecutor(ExecutorType.Activate, CardId.SuperStarslayerTYPHON);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummonCheck);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight);

            // ==========================================
            // PRIORITY 9: SET SPELLS / TRAPS
            // ==========================================
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnStrike);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnWarning);
            AddExecutor(ExecutorType.SpellSet, CardId.DestructiveDarumaKarmaCannon);
            AddExecutor(ExecutorType.SpellSet, CardId.Crackdown);
            AddExecutor(ExecutorType.SpellSet, CardId.SkillDrain);
            AddExecutor(ExecutorType.SpellSet, CardId.AntiSpellFragrance);
            AddExecutor(ExecutorType.SpellSet, CardId.MacroCosmos);
            AddExecutor(ExecutorType.SpellSet, CardId.ThereCanBeOnlyOne);

            // Position & Battle Repositioning
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override bool OnSelectHand()
        {
            // Anti-meta stun — strongly prefer going first to establish oppressive floodgates
            return true;
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _summonCountThisTurn = 0;
            _extravaganceUsedThisTurn = false;

            // Check if Morganite has successfully resolved in previous turns
            if (!_plugin.StrategyImpl.MorganiteActive)
            {
                if (Bot.Graveyard.Any(c => c != null && c.Id == CardId.TimeTearingMorganite) ||
                    Bot.Banished.Any(c => c != null && c.Id == CardId.TimeTearingMorganite))
                {
                    _plugin.StrategyImpl.MorganiteActive = true;
                }
            }
        }

        public override void OnChaining(int player, ClientCard card)
        {
            base.OnChaining(player, card);
            if (player == 0 && card != null)
            {
                if (card.Id == CardId.TimeTearingMorganite && card.Location != CardLocation.Grave && card.Location != CardLocation.Removed)
                {
                    _plugin.StrategyImpl.MorganiteActive = true;
                }
            }
        }

        private bool CanNormalSummon()
        {
            int limit = _plugin.StrategyImpl.MorganiteActive ? 2 : 1;
            return _summonCountThisTurn < limit;
        }

        // ====================================================================================================
        // COUNTER TRAP EXECUTORS
        // ====================================================================================================

        private bool SolemnJudgmentEffect()
        {
            if (LastChainCard != null)
            {
                if (LastChainCard.Controller != 1) return false;
                if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.SolemnJudgment)) return false;

                // Priority intercept: Any Spell/Trap card activation from opponent
                if (LastChainCard.IsSpell() || LastChainCard.IsTrap())
                    return true;

                // Intercept threatening monster effects if life permits
                if (LastChainCard.IsMonster() && _plugin.ThreatImpl.EvaluateThreatScore(LastChainCard) >= 3000)
                    return true;
            }

            // Inherent Summon Negation (Normal or Special Summon by opponent)
            if (Duel.LastSummonPlayer == 1 && Duel.LastChainPlayer == -1)
            {
                if (Duel.CurrentChain.Any(c => c != null && (c.Id == CardId.SolemnJudgment || c.Id == CardId.SolemnWarning || c.Id == CardId.SolemnStrike)))
                    return false;

                if (Duel.SummoningCards != null && Duel.SummoningCards.Count > 0)
                {
                    var m = Duel.SummoningCards[0];
                    if (m != null && m.Attack <= 1000 && !m.IsExtraCard() && m.Level <= 4 && !m.IsSpecialSummoned)
                        return false; // Save Solemn Judgment for threats, not 0 ATK normal summons
                }

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
                return true;
            }

            // Inherent Special Summon Negation
            if (Duel.LastSummonPlayer == 1 && Duel.LastChainPlayer == -1)
            {
                if (Duel.CurrentChain.Any(c => c != null && (c.Id == CardId.SolemnJudgment || c.Id == CardId.SolemnWarning || c.Id == CardId.SolemnStrike)))
                    return false;

                if (Duel.SummoningCards != null && Duel.SummoningCards.Count > 0)
                {
                    var m = Duel.SummoningCards[0];
                    if (m != null && m.Attack <= 1000 && !m.IsExtraCard() && m.Level <= 4 && !m.IsSpecialSummoned)
                        return false;
                }

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

                var card = Duel.GetCurrentSolvingChainCard();
                if (card != null && (card.HasType(CardType.Monster) || card.HasType(CardType.Spell) || card.HasType(CardType.Trap)))
                    return true;
            }

            // Inherent Summon Negation
            if (Duel.LastSummonPlayer == 1 && Duel.LastChainPlayer == -1)
            {
                if (Duel.CurrentChain.Any(c => c != null && (c.Id == CardId.SolemnJudgment || c.Id == CardId.SolemnWarning || c.Id == CardId.SolemnStrike)))
                    return false;

                if (Duel.SummoningCards != null && Duel.SummoningCards.Count > 0)
                {
                    var m = Duel.SummoningCards[0];
                    if (m != null && m.Attack <= 1000 && !m.IsExtraCard() && m.Level <= 4 && !m.IsSpecialSummoned)
                        return false; // Don't waste 2000 LP on a 0 ATK tuner normal summon
                }

                return true;
            }

            return false;
        }

        // ====================================================================================================
        // DISRUPTION TRAPS & MONSTER NEGATE
        // ====================================================================================================

        private bool RaiOhNegateEffect()
        {
            // Send Rai-Oh to GY to negate inherent Special Summon
            if (Duel.LastSummonPlayer == 1 && Duel.LastChainPlayer == -1)
            {
                // If a Counter Trap is already in chain, let the Counter Trap handle it without losing Rai-Oh
                if (Duel.CurrentChain.Any(c => c != null && (c.Id == CardId.SolemnJudgment || c.Id == CardId.SolemnWarning || c.Id == CardId.SolemnStrike)))
                    return false;

                if (Duel.SummoningCards != null && Duel.SummoningCards.Count > 0)
                {
                    var m = Duel.SummoningCards[0];
                    if (m != null && m.Attack < 1800 && !m.IsExtraCard() && !m.IsSpecialSummoned)
                        return false;
                }

                // If Rai-Oh has Moon Mirror Shield equipped, check if we want to preserve it
                if (Card != null && Card.EquipCards != null && Card.EquipCards.Any(e => e.Id == CardId.MoonMirrorShield))
                {
                    // Only tribute if opponent has other monsters or we have no other floodgates
                    if (Bot.GetMonsterCount() > 1) return true;
                    return false;
                }

                return true;
            }
            return false;
        }

        private bool DarumaCannonEffect()
        {
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.DestructiveDarumaKarmaCannon)) return false;

            // Case 1: Opponent has 2+ monsters on field (mass disruption)
            if (Enemy.GetMonsterCount() >= 2) return true;

            // Case 2: Opponent has a Link monster (cannot flip face-down -> sent directly to GY)
            if (Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.Link)))
                return true;

            // Case 3: Opponent enters battle or declares an attack
            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.BattleStep))
            {
                if (Enemy.GetMonsterCount() > 0)
                {
                    // If our monster has Moon Mirror Shield, we easily win the battle without flipping
                    bool ourMonsterHasShield = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                        m.EquipCards != null && m.EquipCards.Any(e => e.Id == CardId.MoonMirrorShield));

                    if (!ourMonsterHasShield || Enemy.GetMonsterCount() >= 2)
                        return true;
                }
            }

            return false;
        }

        private bool CrackdownEffect()
        {
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.Crackdown)) return false;

            var targets = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.HasType(CardType.Token)).ToList();
            if (targets.Count == 0) return false;

            // Steal dangerous high ATK or Extra Deck boss monsters
            var bestTarget = targets.OrderByDescending(m => m.Attack).FirstOrDefault();
            if (bestTarget != null && (bestTarget.Attack >= 1800 || bestTarget.IsExtraCard()))
                return true;

            // Steal combo starter if opponent has only 1 monster on their turn
            if (targets.Count == 1 && Duel.Player == 1)
                return true;

            return false;
        }

        // ====================================================================================================
        // FLOODGATES
        // ====================================================================================================

        private bool SkillDrainEffect()
        {
            if (Bot.LifePoints <= 1000) return false;
            if (Bot.HasInSpellZone(CardId.SkillDrain)) return false;

            // Opponent turn: flip immediately in Draw/Standby/Main Phase
            if (Duel.Player == 1) return true;

            // Chain to opponent monster effect
            if (Duel.LastChainPlayer == 1) return true;

            // Opponent has face-up effect monsters
            if (Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && !m.IsDisabled() && !m.HasType(CardType.Normal)))
                return true;

            return false;
        }

        private bool AntiSpellFragranceEffect()
        {
            if (Bot.HasInSpellZone(CardId.AntiSpellFragrance)) return false;

            // Opponent turn: flip immediately in Draw/Standby Phase to lock all spells from hand!
            if (Duel.Player == 1) return true;

            // Our turn: flip in End Phase before passing turn
            if (Duel.Phase == DuelPhase.End) return true;

            return false;
        }

        private bool TcbooEffect()
        {
            if (Bot.HasInSpellZone(CardId.ThereCanBeOnlyOne)) return false;
            return Enemy.GetMonsterCount() >= 1 || Duel.Player == 1;
        }

        private bool MacroCosmosEffect()
        {
            if (Bot.HasInSpellZone(CardId.MacroCosmos)) return false;
            if (Bot.HasInMonstersZone(CardId.BanisherOfTheRadiance)) return false;
            if (Duel.Player == 1) return true;
            return false;
        }

        private bool DimensionalFissureEffect()
        {
            return !Bot.HasInSpellZone(CardId.DimensionalFissure) && !Bot.HasInMonstersZone(CardId.BanisherOfTheRadiance);
        }

        private bool NecrovalleyEffect()
        {
            return !Bot.HasInSpellZone(CardId.Necrovalley);
        }

        // ====================================================================================================
        // DRAW & SEARCH SPELLS
        // ====================================================================================================

        private bool PotOfExtravaganceEffect()
        {
            if (Duel.Phase != DuelPhase.Main1 || Duel.Player != 0) return false;
            if (_extravaganceUsedThisTurn) return false;
            if (Bot.ExtraDeck.Count < 6) return false;

            _extravaganceUsedThisTurn = true;
            return true;
        }

        private bool MorganiteEffect()
        {
            if (Card.Location == CardLocation.Hand)
            {
                if (_plugin.StrategyImpl.MorganiteActive || Duel.CurrentChain.Any(c => c != null && c.Id == CardId.TimeTearingMorganite))
                    return false;
                return true;
            }
            return false;
        }

        private bool PotOfDualityEffect()
        {
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.PotOfDuality)) return false;
            return true;
        }

        private bool MoonMirrorShieldEffect()
        {
            var monsters = Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (monsters.Count == 0) return false;

            // Priority: unequipped monsters
            var target = monsters.FirstOrDefault(m => m.Id == CardId.BarrierStatueOfTheHeavens && (m.EquipCards == null || m.EquipCards.Count == 0))
                      ?? monsters.FirstOrDefault(m => m.Id == CardId.ThunderKingRaiOh && (m.EquipCards == null || m.EquipCards.Count == 0))
                      ?? monsters.FirstOrDefault(m => m.Id == CardId.BanisherOfTheRadiance && (m.EquipCards == null || m.EquipCards.Count == 0))
                      ?? monsters.FirstOrDefault(m => m.Id == CardId.InspectorBoarder && (m.EquipCards == null || m.EquipCards.Count == 0))
                      ?? monsters.FirstOrDefault(m => (m.EquipCards == null || m.EquipCards.Count == 0))
                      ?? monsters[0];

            return target != null;
        }

        // ====================================================================================================
        // SUMMON CHECKS
        // ====================================================================================================

        private bool BoarderSummonCheck()
        {
            if (!CanNormalSummon()) return false;
            _summonCountThisTurn++;
            return true;
        }

        private bool RaiOhSummonCheck()
        {
            if (!CanNormalSummon()) return false;

            // If we have Boarder in hand and 0 monsters on field, prefer Boarder first
            if (Bot.GetMonsterCount() == 0 && Bot.HasInHand(CardId.InspectorBoarder))
                return false;

            _summonCountThisTurn++;
            return true;
        }

        private bool BarrierStatueSummonCheck()
        {
            if (!CanNormalSummon()) return false;

            // TCBOO check: don't summon Barrier Statue if Banisher is on field (both Fairy)
            if (Bot.HasInSpellZone(CardId.ThereCanBeOnlyOne) && Bot.HasInMonstersZone(CardId.BanisherOfTheRadiance))
                return false;

            _summonCountThisTurn++;
            return true;
        }

        private bool BanisherSummonCheck()
        {
            if (!CanNormalSummon()) return false;

            // TCBOO check: don't summon Banisher if Barrier Statue is on field (both Fairy)
            if (Bot.HasInSpellZone(CardId.ThereCanBeOnlyOne) && Bot.HasInMonstersZone(CardId.BarrierStatueOfTheHeavens))
                return false;

            _summonCountThisTurn++;
            return true;
        }

        private bool TyphonSummonCheck()
        {
            if (Bot.GetMonsterCount() > 0 && Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && IsAceCard(m)))
                return false;

            return Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack >= 3000);
        }

        private bool SPLittleKnightSummonCheck()
        {
            if (Duel.Phase != DuelPhase.Main2) return false;
            return Enemy.GetMonsterCount() > 0 && Bot.GetMonsterCount() >= 2;
        }

        // ====================================================================================================
        // POSITION REPOSITIONING
        // ====================================================================================================

        private bool MonsterRepos()
        {
            if (Card == null) return false;

            bool hasShield = Card.EquipCards != null && Card.EquipCards.Any(e => e.Id == CardId.MoonMirrorShield);
            if (hasShield)
            {
                if (Card.IsDefense()) return true; // Switch to Attack to dominate
                return false;
            }

            if (Card.IsAttack())
            {
                // If enemy has face-up monster with higher ATK, switch to defense if DEF is safer
                var strongerEnemy = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Attack > Card.Attack);
                if (strongerEnemy != null && Card.Defense >= Card.Attack)
                    return true;
            }
            else
            {
                // Switch to Attack if field is clear or monster has high ATK
                if (Enemy.GetMonsterCount() == 0 || Card.Attack >= 1900)
                    return true;
            }

            return false;
        }

        // ====================================================================================================
        // SELECT CARD & EFFECT OVERRIDES
        // ====================================================================================================

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            // Crackdown Target Selection
            if (LastChainCard != null && LastChainCard.Id == CardId.Crackdown)
            {
                var oppTargets = cards.Where(c => c != null && c.Controller == 1 && c.IsFaceup()).OrderByDescending(c => c.Attack).ToList();
                if (oppTargets.Count > 0)
                {
                    var target = oppTargets.First();
                    var result = new List<ClientCard> { target };
                    foreach (var c in cards.Where(c => c != target))
                    {
                        if (result.Count >= max) break;
                        result.Add(c);
                    }
                    return result;
                }
            }

            // Rule 1: Destruction / Banish / Bounce must target ENEMY cards ONLY
            if (hint == 502 || hint == 503 || hint == 504 || hint == 509)
            {
                var enemyCards = cards.Where(c => c != null && c.Controller == 1).ToList();
                if (enemyCards.Count > 0)
                {
                    return enemyCards.OrderByDescending(c => _plugin.ThreatImpl.EvaluateThreatScore(c)).Take(max).ToList();
                }
            }

            // Pot of Duality selection (hint == 506 HINTMSG_ATOHAND)
            if (hint == 506 && cards.All(c => c.Location == CardLocation.Deck))
            {
                var bestCard = _plugin.StrategyImpl.PickSearchTarget(cards, Card);
                if (bestCard != null)
                {
                    return new List<ClientCard> { bestCard };
                }
            }

            // Equip target for Moon Mirror Shield (hint == 518 HINTMSG_EQUIP)
            if (hint == 518 || (Card != null && Card.Id == CardId.MoonMirrorShield))
            {
                var ourMonsters = cards.Where(c => c != null && c.Controller == 0 && c.Location == CardLocation.MonsterZone).ToList();
                if (ourMonsters.Count > 0)
                {
                    var equipTarget = _plugin.StrategyImpl.PickEquipTarget(ourMonsters);
                    if (equipTarget != null)
                    {
                        return new List<ClientCard> { equipTarget };
                    }
                }
            }

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            // Floodgate beatsticks enter in FaceUpAttack
            if (cardId == CardId.InspectorBoarder || cardId == CardId.ThunderKingRaiOh || cardId == CardId.BanisherOfTheRadiance)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            // Barrier Statue: FaceUpAttack if we have Moon Mirror Shield, otherwise FaceUpDefense if enemy has beatsticks
            if (cardId == CardId.BarrierStatueOfTheHeavens)
            {
                bool hasShield = Bot.HasInHand(CardId.MoonMirrorShield) ||
                                 Bot.GetSpells().Any(s => s != null && s.Id == CardId.MoonMirrorShield);
                if (hasShield && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;

                if (Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.Attack > 1000))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                }

                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            // Rule 14: Reject opponent effects by default
            if (card != null && card.Controller == 1)
                return false;

            // Moon Mirror Shield: When sent to GY, pay 500 LP to place on top/bottom of Deck (Always activate!)
            if (card != null && card.Id == CardId.MoonMirrorShield)
                return true;

            return base.OnSelectEffectYn(card, desc);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            // Rule 11: Keep EMZ open, place in Main Monster Zones
            return base.OnSelectPlace(cardId, player, location, available);
        }
    }
}
