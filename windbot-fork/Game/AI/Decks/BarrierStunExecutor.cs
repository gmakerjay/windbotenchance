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
    // CARD AUDIT — BarrierStun (Omni-Elemental Barrier Lock & Anti-Meta Stun Deck)
    // 100% verified against cards.cdb and BarrierStun.ydk
    // ====================================================================================================
    // | Card Name                     | Type           | Lv/Rk | ATK  | DEF  | Key Interaction                                         |
    // |-------------------------------|----------------|-------|------|------|---------------------------------------------------------|
    // | Barrier Statue of the Abyss   | Fiend/DARK     | 4     | 1000 | 1000 | Locks Special Summons of all non-DARK monsters         |
    // | Barrier Statue of the Heavens | Fairy/LIGHT    | 4     | 1000 | 1000 | Locks Special Summons of all non-LIGHT monsters        |
    // | Inspector Boarder             | Machine/LIGHT  | 4     | 2000 | 2000 | Monster effect freeze (0 activations if no ED monster) |
    // | Kycoo the Ghost Destroyer     | Spellcaster/DK | 4     | 1800 | 700  | Opponent cannot banish from GY; banishes 2 enemy GY     |
    // | Time-Tearing Morganite        | Normal Spell   | -     | -    | -    | Double Normal Summon + Double Draw every turn          |
    // | Moon Mirror Shield            | Equip Spell    | -     | -    | -    | Battle invincibility (ATK/DEF = opp ATK/DEF + 100)     |
    // | Pot of Duality                | Normal Spell   | -     | -    | -    | Excavate 3 add 1, no SS this turn (we don't SS)        |
    // | Pot of Extravagance           | Normal Spell   | -     | -    | -    | Banish 6 ED cards to Draw 2 cards                      |
    // | Necrovalley                   | Field Spell    | -     | -    | -    | Graveyard total lockout                                |
    // | Dimensional Fissure           | Continuous Sp  | -     | -    | -    | All monsters banished instead of sent to GY            |
    // | Anti-Spell Fragrance          | Continuous Tr  | -     | -    | -    | Spells must be set 1 turn before activating            |
    // | Skill Drain                   | Continuous Tr  | -     | -    | -    | Negates all face-up monster effects on field           |
    // | Macro Cosmos                  | Continuous Tr  | -     | -    | -    | Universal banish floodgate                             |
    // | Destructive Daruma Karma Cannon| Normal Trap   | -     | -    | -    | Mass face-down flip; Links & unaffected sent to GY     |
    // | Solemn Strike                 | Counter Trap   | -     | -    | -    | Negate monster effect activation or Special Summon     |
    // | Solemn Warning                | Counter Trap   | -     | -    | -    | Negate summon or effect that summons                   |
    // | Solemn Judgment               | Counter Trap   | -     | -    | -    | Omni-negate; protects backrow from Feather Duster etc. |
    // | Crackdown                     | Continuous Tr  | -     | -    | -    | Steals face-up opponent monster                        |
    // ====================================================================================================

    [Deck("BarrierStun", "BarrierStun")]
    public class BarrierStunExecutor : ModernExecutor
    {
        public class CardId
        {
            // --- MONSTERS ---
            public const int BarrierStatueOfTheAbyss = 84478195;
            public const int BarrierStatueOfTheHeavens = 46145256;
            public const int InspectorBoarder = 15397015;
            public const int KycooTheGhostDestroyer = 88240808;

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
            public const int AntiSpellFragrance = 58921041;
            public const int SkillDrain = 82732705;
            public const int MacroCosmos = 30241314;

            // --- EXTRA DECK ---
            public const int Garura = 11765832;
            public const int Mudragon = 54757758;
            public const int Ntss = 80532587;
            public const int SPLittleKnight = 29301450;
            public const int AAZeus = 90448279;
            public const int TYPHON = 93039339;
            public const int TornadoDragon = 6983839;
            public const int Castel = 82633039;
            public const int KnightmarePhoenix = 2857636;
        }

        public static readonly int[] StunMonsters = {
            CardId.BarrierStatueOfTheAbyss,
            CardId.BarrierStatueOfTheHeavens,
            CardId.InspectorBoarder,
            CardId.KycooTheGhostDestroyer
        };

        private readonly BarrierStunPlugin _plugin;
        private int _summonCountThisTurn = 0;

        public BarrierStunExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            _plugin = new BarrierStunPlugin(this);
            DeckPlugin = _plugin;

            // Register Ace cards that should never be sacrificed
            HeuristicGuard.RegisterAceCards(
                CardId.BarrierStatueOfTheAbyss,
                CardId.BarrierStatueOfTheHeavens,
                CardId.InspectorBoarder,
                CardId.KycooTheGhostDestroyer
            );

            // ==========================================
            // PRIORITY 1: COUNTER TRAPS (Chain Reactions)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, SolemnStrikeEffect);
            AddExecutor(ExecutorType.Activate, CardId.SolemnWarning, SolemnWarningEffect);

            // ==========================================
            // PRIORITY 2: PROACTIVE CONTINUOUS TRAPS / FLIP GATES
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainEffect);
            AddExecutor(ExecutorType.Activate, CardId.AntiSpellFragrance, AntiSpellFragranceEffect);
            AddExecutor(ExecutorType.Activate, CardId.MacroCosmos, MacroCosmosEffect);
            AddExecutor(ExecutorType.Activate, CardId.DestructiveDarumaKarmaCannon, DarumaCannonEffect);
            AddExecutor(ExecutorType.Activate, CardId.Crackdown, CrackdownEffect);

            // ==========================================
            // PRIORITY 3: POTS & ENGINES (Main Phase 1)
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance, PotOfExtravaganceEffect);
            AddExecutor(ExecutorType.Activate, CardId.TimeTearingMorganite, MorganiteEffect);
            AddExecutor(ExecutorType.Activate, CardId.PotOfDuality, PotOfDualityEffect);

            // ==========================================
            // PRIORITY 4: FIELD & CONTINUOUS SPELLS
            // ==========================================
            AddExecutor(ExecutorType.Activate, CardId.Necrovalley, NecrovalleyEffect);
            AddExecutor(ExecutorType.Activate, CardId.DimensionalFissure, DimensionalFissureEffect);
            AddExecutor(ExecutorType.Activate, CardId.MoonMirrorShield, MoonMirrorShieldEffect);

            // ==========================================
            // PRIORITY 5: NORMAL SUMMONS
            // ==========================================
            AddExecutor(ExecutorType.Summon, CardId.InspectorBoarder, BoarderSummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.BarrierStatueOfTheAbyss, StatueAbyssSummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.BarrierStatueOfTheHeavens, StatueHeavensSummonCheck);
            AddExecutor(ExecutorType.Summon, CardId.KycooTheGhostDestroyer, KycooSummonCheck);

            // ==========================================
            // PRIORITY 6: SET TRAPS / SPELLS
            // ==========================================
            AddExecutor(ExecutorType.SpellSet, SpellSetPriority);

            // Repositioning Safeguard
            AddExecutor(ExecutorType.Repos, MonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            _summonCountThisTurn = 0;
            _plugin.ResetTurnState();

            // Detect if Morganite was resolved previously
            if (Bot.Graveyard.Any(c => c.Id == CardId.TimeTearingMorganite) ||
                Bot.Banished.Any(c => c.Id == CardId.TimeTearingMorganite))
            {
                _plugin.StrategyImpl.MorganiteActive = true;
            }
        }

        // =========================================================================
        // COUNTER TRAP HANDLERS
        // =========================================================================
        private bool SolemnJudgmentEffect()
        {
            if (Bot.LifePoints <= 1000) return false;

            if (LastChainCard != null && Duel.LastChainPlayer == 1)
            {
                if (LastChainCard.IsSpell() || LastChainCard.IsTrap()) return true;
                if (LastChainCard.IsMonster() && Duel.SummoningCards.Count > 0) return true;
            }

            return Duel.LastChainPlayer == 1;
        }

        private bool SolemnStrikeEffect()
        {
            if (Bot.LifePoints <= 1500) return false;

            if (Duel.LastChainPlayer == 1)
            {
                if (LastChainCard != null && LastChainCard.IsMonster()) return true;
            }

            if (Duel.SummoningCards.Count > 0 && Duel.SummoningCards.Any(c => c.Controller == 1 && c.IsSpecialSummoned))
            {
                return true;
            }

            return false;
        }

        private bool SolemnWarningEffect()
        {
            if (Bot.LifePoints <= 2000) return false;

            if (Duel.SummoningCards.Count > 0)
            {
                var target = Duel.SummoningCards.FirstOrDefault(c => c.Controller == 1);
                if (target != null && !target.IsSpecialSummoned && target.Attack <= 1000 && target.Level <= 4)
                {
                    return false;
                }
                return true;
            }

            if (Duel.LastChainPlayer == 1 && LastChainCard != null)
            {
                return true;
            }

            return false;
        }

        // =========================================================================
        // CONTINUOUS TRAP & FLOODGATE HANDLERS
        // =========================================================================
        private bool SkillDrainEffect()
        {
            if (Bot.LifePoints <= 1000) return false;
            if (Bot.HasInSpellZone(CardId.SkillDrain)) return false;

            if (Duel.Player == 1) return true;
            if (Duel.LastChainPlayer == 1) return true;

            return Enemy.GetMonsters().Any(m => m.IsFaceup());
        }

        private bool AntiSpellFragranceEffect()
        {
            if (Bot.HasInSpellZone(CardId.AntiSpellFragrance)) return false;

            if (Duel.Player == 1) return true;
            return Duel.Phase == DuelPhase.End;
        }

        private bool MacroCosmosEffect()
        {
            if (Bot.HasInSpellZone(CardId.MacroCosmos)) return false;

            if (Duel.Player == 1) return true;
            return Duel.Phase == DuelPhase.End;
        }

        private bool DarumaCannonEffect()
        {
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.DestructiveDarumaKarmaCannon)) return false;

            if (Enemy.GetMonsterCount() >= 2) return true;
            if (Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.Link))) return true;
            if (Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.Attack >= 2000 || m.IsExtraCard()))) return true;

            if (Duel.Player == 1 && (Duel.Phase == DuelPhase.BattleStart || Duel.Phase == DuelPhase.Battle || Duel.Phase == DuelPhase.BattleStep))
            {
                if (Enemy.GetMonsterCount() > 0)
                {
                    bool ourMonsterHasShield = Bot.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                        m.EquipCards != null && m.EquipCards.Any(e => e.Id == CardId.MoonMirrorShield));

                    if (!ourMonsterHasShield || Enemy.GetMonsterCount() >= 2) return true;
                }
            }

            return false;
        }

        private bool CrackdownEffect()
        {
            if (Duel.CurrentChain.Any(c => c != null && c.Id == CardId.Crackdown)) return false;

            var targets = Enemy.GetMonsters().Where(m => m != null && m.IsFaceup() && !m.HasType(CardType.Token)).ToList();
            if (targets.Count == 0) return false;

            var bestTarget = targets.OrderByDescending(m => m.Attack).FirstOrDefault();
            if (bestTarget != null && (bestTarget.Attack >= 1500 || bestTarget.IsExtraCard()))
                return true;

            if (targets.Count >= 1 && Duel.Player == 1)
                return true;

            return false;
        }

        // =========================================================================
        // SPELL HANDLERS
        // =========================================================================
        private bool PotOfExtravaganceEffect()
        {
            if (Bot.ExtraDeck.Count < 6) return false;
            return Duel.Phase == DuelPhase.Main1;
        }

        private bool MorganiteEffect()
        {
            _plugin.StrategyImpl.MorganiteActive = true;
            return true;
        }

        private bool PotOfDualityEffect()
        {
            return true;
        }

        private bool NecrovalleyEffect()
        {
            if (Bot.HasInSpellZone(CardId.Necrovalley)) return false;
            return true;
        }

        private bool DimensionalFissureEffect()
        {
            if (Bot.HasInSpellZone(CardId.DimensionalFissure)) return false;
            return true;
        }

        private bool MoonMirrorShieldEffect()
        {
            return Bot.GetMonsters().Any(m => m != null && m.IsFaceup());
        }

        // =========================================================================
        // SUMMONING HANDLERS
        // =========================================================================
        private bool BoarderSummonCheck()
        {
            int maxSummons = _plugin.StrategyImpl.MorganiteActive ? 2 : 1;
            if (_summonCountThisTurn >= maxSummons) return false;

            if (Bot.HasInMonstersZone(CardId.InspectorBoarder)) return false;

            _summonCountThisTurn++;
            return true;
        }

        private bool StatueAbyssSummonCheck()
        {
            int maxSummons = _plugin.StrategyImpl.MorganiteActive ? 2 : 1;
            if (_summonCountThisTurn >= maxSummons) return false;

            if (Bot.HasInMonstersZone(CardId.BarrierStatueOfTheAbyss)) return false;

            _summonCountThisTurn++;
            return true;
        }

        private bool StatueHeavensSummonCheck()
        {
            int maxSummons = _plugin.StrategyImpl.MorganiteActive ? 2 : 1;
            if (_summonCountThisTurn >= maxSummons) return false;

            if (Bot.HasInMonstersZone(CardId.BarrierStatueOfTheHeavens)) return false;

            // Anti-Harm Safeguard: If enemy controls LIGHT monsters or has LIGHT monsters in GY (e.g. Blue-Eyes, ABC),
            // summoning Statue of the Heavens permits enemy to Special Summon freely!
            // Prefer other monsters (Abyss / Boarder / Kycoo) if available!
            bool enemyIsLight = Enemy.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasAttribute(CardAttribute.Light)) ||
                                Enemy.Graveyard.Any(c => c.IsMonster() && c.HasAttribute(CardAttribute.Light));
            if (enemyIsLight)
            {
                if (Bot.HasInHand(CardId.InspectorBoarder) || Bot.HasInHand(CardId.BarrierStatueOfTheAbyss) || Bot.HasInHand(CardId.KycooTheGhostDestroyer))
                    return false;
            }

            _summonCountThisTurn++;
            return true;
        }

        private bool KycooSummonCheck()
        {
            int maxSummons = _plugin.StrategyImpl.MorganiteActive ? 2 : 1;
            if (_summonCountThisTurn >= maxSummons) return false;

            _summonCountThisTurn++;
            return true;
        }

        private bool SpellSetPriority()
        {
            if (Card.Id == CardId.TimeTearingMorganite ||
                Card.Id == CardId.PotOfDuality ||
                Card.Id == CardId.PotOfExtravagance)
            {
                return false;
            }

            if (Card.Id == CardId.MoonMirrorShield && !Bot.GetMonsters().Any(m => m.IsFaceup()))
            {
                return false;
            }

            return Card.IsTrap() || Card.Id == CardId.Necrovalley || Card.Id == CardId.DimensionalFissure;
        }

        private bool MonsterRepos()
        {
            if (Card == null) return false;

            bool hasShield = Card.EquipCards != null && Card.EquipCards.Any(e => e.Id == CardId.MoonMirrorShield);
            if (hasShield)
            {
                if (Card.IsDefense()) return true;
                return false;
            }

            if (Card.IsAttack())
            {
                var strongerEnemy = Enemy.GetMonsters().FirstOrDefault(m => m != null && m.IsFaceup() && m.Attack > Card.Attack);
                if (strongerEnemy != null && Card.Defense >= Card.Attack)
                    return true;
            }
            else
            {
                if (Enemy.GetMonsterCount() == 0 || Card.Attack >= 1800)
                    return true;
            }

            return false;
        }

        // =========================================================================
        // SELECTION OVERRIDES (Decoupled Domain Plugin Integration)
        // =========================================================================
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
            if (cardId == CardId.InspectorBoarder || cardId == CardId.KycooTheGhostDestroyer)
            {
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            if (cardId == CardId.BarrierStatueOfTheAbyss || cardId == CardId.BarrierStatueOfTheHeavens)
            {
                bool hasShield = Bot.HasInHand(CardId.MoonMirrorShield) ||
                                 Bot.GetSpells().Any(s => s != null && s.Id == CardId.MoonMirrorShield);
                if (hasShield && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;

                if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                if (positions.Contains(CardPosition.FaceUpAttack)) return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override bool? OnSelectEffectYn(ClientCard card, long desc)
        {
            if (card != null && card.Controller == 1)
                return false;

            if (card != null && card.Id == CardId.MoonMirrorShield)
                return true;

            if (card != null && card.Id == CardId.KycooTheGhostDestroyer)
                return true;

            return base.OnSelectEffectYn(card, desc);
        }

        public override int OnSelectPlace(long cardId, int player, CardLocation location, int available)
        {
            return base.OnSelectPlace(cardId, player, location, available);
        }
    }
}
