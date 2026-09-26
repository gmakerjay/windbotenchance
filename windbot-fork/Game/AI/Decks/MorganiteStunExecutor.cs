// ============================================================================
// MorganiteStunExecutor.cs — Anti-Meta Morganite Stun & Board Break Executor
// Archetype: Time-Tearing Morganite / Vanity's Ruler / Inspector Boarder / Super Poly
// Standard: Decoupled Domain Plugin Architecture (Layer 3 in SKILL.md)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("MorganiteStun")]
    public class MorganiteStunExecutor : ModernExecutor
    {
        public static class CardId
        {
            // Monsters
            public const int InspectorBoarder = 15397015;
            public const int MorganaTheWitchOfEyes = 29439831;
            public const int VanitySRuler = 72634965;
            public const int MajestysFiend = 33746252;

            // Spells
            public const int MonsterReborn = 84211599;
            public const int PotOfExtravagance = 49238328;
            public const int TimeTearingMorganite = 19403423;
            public const int SuccumbingSongMorganite = 81756619;
            public const int GuiltGrippingMorganite = 61822419;
            public const int SeventhTachyon = 7477101;
            public const int SuperPolymerization = 48130397;

            // Traps & Handtraps
            public const int DominusPurge = 97045737;
            public const int DominusImpulse = 40366667;
            public const int SongsOfTheDominators = 58053438;
            public const int SolemnJudgment = 41420027;
            public const int SolemnStrike = 40605147;
            public const int IronThunder = 12682213;
            public const int SkillDrain = 82732705;

            // Extra Deck
            public const int MudragonOfTheSwamp = 54757758;
            public const int GaruraWingsOfResonantLife = 11765832;
            public const int EarthGolemIgnister = 62111090;
            public const int StarvingVenomFusionDragon = 41209827;
            public const int PredaplantDragostapelia = 69946549;
            public const int KhaosStarsourceDragon = 72578374;
            public const int WorldChaliceGuardragonAlmarduke = 95793022;
            public const int PredaplantTriphyoverutum = 79864860;
            public const int FiendsmithSRextremende = 11464648;
            public const int Number104Masquerade = 2061963;
            public const int Number107GalaxyEyesTachyonDragon = 88177324;
            public const int SuperStarslayerTYPHONSkyCrisis = 93039339;
            public const int SPLittleKnight = 29301450;
        }

        // Domain Plugin Coordinator (Layer 3)
        internal MorganiteStunPlugin Plugin { get; private set; }

        public ClientCard CurrentLastChainCard => LastChainCard;

        public MorganiteStunExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Plugin = new MorganiteStunPlugin(this);

            // Register Ace Floodgate Assets
            ResourcePlan.RegisterAceCards(
                CardId.VanitySRuler,
                CardId.InspectorBoarder,
                CardId.MajestysFiend,
                CardId.SkillDrain,
                CardId.SuperStarslayerTYPHONSkyCrisis
            );

            BaitPlanner.RegisterComboStarters(
                CardId.PotOfExtravagance,
                CardId.GuiltGrippingMorganite,
                CardId.SeventhTachyon,
                CardId.TimeTearingMorganite
            );

            ChainAdvisor.RegisterHighValueTargets(
                CardId.VanitySRuler,
                CardId.InspectorBoarder,
                CardId.SkillDrain
            );

            // ═══════════════════════════════════════════════════════════════
            //  EXECUTOR PIPELINE (Anti-Meta Stun Prioritized Action Tree)
            // ═══════════════════════════════════════════════════════════════

            // ── Tier 0: Absolute Counter Responses, Negations & Trap Handtraps ──
            AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, SolemnJudgmentActivate);
            AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, SolemnStrikeActivate);
            AddExecutor(ExecutorType.Activate, CardId.IronThunder, IronThunderActivate);
            AddExecutor(ExecutorType.Activate, CardId.SkillDrain, SkillDrainActivate);
            AddExecutor(ExecutorType.Activate, CardId.SongsOfTheDominators, SongsOfTheDominatorsActivate);
            AddExecutor(ExecutorType.Activate, CardId.DominusPurge, DominusPurgeActivate);
            AddExecutor(ExecutorType.Activate, CardId.DominusImpulse, DominusImpulseActivate);

            // ── Tier 1: Spell Speed 4 Board Clear (Super Polymerization) ──
            AddExecutor(ExecutorType.Activate, CardId.SuperPolymerization, SuperPolymerizationActivate);

            // ── Tier 2: Draw & Search Engine ──
            AddExecutor(ExecutorType.Activate, CardId.PotOfExtravagance, PotOfExtravaganceActivate);
            AddExecutor(ExecutorType.Activate, CardId.GuiltGrippingMorganite, GuiltGrippingMorganiteActivate);
            AddExecutor(ExecutorType.Activate, CardId.TimeTearingMorganite, TimeTearingMorganiteActivate);
            AddExecutor(ExecutorType.Activate, CardId.SuccumbingSongMorganite, SuccumbingSongMorganiteActivate);
            AddExecutor(ExecutorType.Activate, CardId.SeventhTachyon, SeventhTachyonActivate);
            AddExecutor(ExecutorType.Activate, CardId.MonsterReborn, MonsterRebornActivate);

            // ── Tier 3: Boss & Floodgate Normal Summons ──
            AddExecutor(ExecutorType.Summon, CardId.VanitySRuler, VanitySRulerSummon);
            AddExecutor(ExecutorType.Summon, CardId.InspectorBoarder, InspectorBoarderSummon);
            AddExecutor(ExecutorType.Summon, CardId.MajestysFiend, MajestysFiendSummon);
            AddExecutor(ExecutorType.Summon, CardId.MorganaTheWitchOfEyes, MorganaSummon);

            // ── Tier 4: Emergency Extra Deck Options ──
            AddExecutor(ExecutorType.SpSummon, CardId.SuperStarslayerTYPHONSkyCrisis, TyphonSummon);
            AddExecutor(ExecutorType.Activate, CardId.SuperStarslayerTYPHONSkyCrisis, TyphonActivate);
            AddExecutor(ExecutorType.SpSummon, CardId.SPLittleKnight, SPLittleKnightSummon);
            AddExecutor(ExecutorType.Activate, CardId.SPLittleKnight, SPLittleKnightActivate);

            // ── Tier 5: Backrow Setting ──
            AddExecutor(ExecutorType.SpellSet, CardId.SkillDrain, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnJudgment, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.SolemnStrike, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.IronThunder, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.SuperPolymerization, SetQuickPlayCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.SongsOfTheDominators, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusPurge, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet, CardId.DominusImpulse, SetTrapCondition);
            AddExecutor(ExecutorType.SpellSet);

            // ── Tier 6: Emergency Desperation MonsterSet (Face-down Defense Only) ──
            AddExecutor(ExecutorType.MonsterSet, DesperationMonsterSet);

            // ── Tier 7: Smart Monster Repositioning ──
            AddExecutor(ExecutorType.Repos, SmartMonsterRepos);
        }

        public override void OnNewTurn()
        {
            base.OnNewTurn();
            Plugin.ResetTurnState();
        }

        public override bool OnSelectHand()
        {
            // Stun thrives going first to establish locks before opponent plays
            return true;
        }

        // ═══════════════════════════════════════════════════════════════
        //  ACTIVATION & SUMMON LOGIC
        // ═══════════════════════════════════════════════════════════════

        private bool SolemnJudgmentActivate()
        {
            if (Plugin.FloodgateManager.IsGuiltMorganiteActive || Bot.LifePoints > 1500)
                return DefaultTrap();
            return false;
        }

        private bool SolemnStrikeActivate()
        {
            if (Plugin.FloodgateManager.IsGuiltMorganiteActive || Bot.LifePoints > 1500)
                return DefaultTrap();
            return false;
        }

        private bool IronThunderActivate()
        {
            if (Plugin.FloodgateManager.IsGuiltMorganiteActive || Bot.LifePoints > 2000)
                return DefaultTrap();
            return false;
        }

        private bool SkillDrainActivate()
        {
            if (Plugin.FloodgateManager.HasSkillDrain) return false;
            // Always activate if opponent has face-up monsters or going first
            if (Duel.Turn <= 1 || Enemy.GetMonsterCount() > 0)
            {
                if (Plugin.FloodgateManager.IsGuiltMorganiteActive || Bot.LifePoints > 1000)
                    return true;
            }
            return false;
        }

        private bool SongsOfTheDominatorsActivate()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1 || !last.IsMonster()) return false;
            if (last.IsDisabled()) return false;
            return true;
        }

        private bool DominusPurgeActivate()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return false;
            if (last.IsDisabled()) return false;
            return true;
        }

        private bool DominusImpulseActivate()
        {
            ClientCard last = LastChainCard;
            if (last == null || last.Controller != 1) return false;
            if (last.IsDisabled()) return false;
            return true;
        }

        private bool SuperPolymerizationActivate()
        {
            if (Card.Location == CardLocation.Hand && Bot.Hand.Count < 2) return false;
            return Plugin.SuperPolyAdvisor.CanActivateSuperPoly();
        }

        private bool PotOfExtravaganceActivate()
        {
            return Duel.Phase == DuelPhase.Main1 && Bot.Hand.Contains(Card);
        }

        private bool GuiltGrippingMorganiteActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: banish self, reveal Morganite in hand, draw 1
                return Bot.Hand.Any(c => c.Id == CardId.TimeTearingMorganite || c.Id == CardId.SuccumbingSongMorganite || c.Id == CardId.GuiltGrippingMorganite);
            }
            return !Plugin.FloodgateManager.IsGuiltMorganiteActive;
        }

        private bool TimeTearingMorganiteActivate()
        {
            return !Plugin.FloodgateManager.IsTimeMorganiteActive;
        }

        private bool SuccumbingSongMorganiteActivate()
        {
            if (Card.Location == CardLocation.Grave)
            {
                // GY effect: banish self, search Morganite
                return true;
            }
            return true;
        }

        private bool SeventhTachyonActivate()
        {
            // Reveal Number 104 to search Inspector Boarder, or Number 107 to search Vanity's Ruler
            if (Bot.ExtraDeck.Any(c => c.Id == CardId.Number107GalaxyEyesTachyonDragon) &&
                (Plugin.FloodgateManager.IsGuiltMorganiteActive || Bot.GetMonsterCount() > 0))
            {
                return true;
            }
            if (Bot.ExtraDeck.Any(c => c.Id == CardId.Number104Masquerade))
            {
                return true;
            }
            return false;
        }

        private bool MonsterRebornActivate()
        {
            return Bot.Graveyard.Any(c => c.Id == CardId.VanitySRuler || c.Id == CardId.InspectorBoarder || c.Id == CardId.MajestysFiend);
        }

        private bool VanitySRulerSummon()
        {
            if (Plugin.FloodgateManager.HasVanitySRuler) return false;
            // Can be normal summoned without tribute if Guilt-Gripping Morganite is active
            if (Plugin.FloodgateManager.IsGuiltMorganiteActive) return true;
            // Otherwise requires tribute
            var tributes = Bot.GetMonsters().Where(m => m.IsFaceup() && !IsAceCard(m)).ToList();
            return tributes.Count >= 2;
        }

        private bool InspectorBoarderSummon()
        {
            if (Plugin.FloodgateManager.HasInspectorBoarder) return false;
            // Inspector Boarder cannot be normal summoned if we already control a monster (unless empty field or tribute)
            if (Bot.GetMonsterCount() == 0) return true;
            if (Plugin.FloodgateManager.IsTimeMorganiteActive && Bot.GetMonsterCount() == 0) return true;
            return false;
        }

        private bool MajestysFiendSummon()
        {
            if (Plugin.FloodgateManager.HasMajestysFiend) return false;
            if (Plugin.FloodgateManager.IsGuiltMorganiteActive) return true;
            var tributes = Bot.GetMonsters().Where(m => m.IsFaceup() && !IsAceCard(m)).ToList();
            return tributes.Count >= 1;
        }

        private bool MorganaSummon()
        {
            return Bot.GetMonsterCount() == 0;
        }

        private bool TyphonSummon()
        {
            // Can summon TY-PHON if opponent special summoned 2+ monsters from Extra Deck this turn/previous turn
            return Enemy.GetMonsters().Any(m => m.IsFaceup() && m.Attack >= 3000);
        }

        private bool TyphonActivate()
        {
            // Detach 1 to bounce 1 monster
            return Enemy.GetMonsterCount() > 0;
        }

        private bool SPLittleKnightSummon()
        {
            var nonAce = Bot.GetMonsters().Where(m => m.IsFaceup() && !IsAceCard(m)).ToList();
            return nonAce.Count >= 2 && Enemy.GetMonsterCount() > 0;
        }

        private bool SPLittleKnightActivate()
        {
            return Enemy.GetMonsterCount() > 0 || Enemy.GetSpellCount() > 0;
        }

        private bool SetTrapCondition()
        {
            return Duel.Phase == DuelPhase.Main2 || Duel.Turn == 1 || Bot.GetSpellCountWithoutField() < 4;
        }

        private bool SetQuickPlayCondition()
        {
            return Duel.Phase == DuelPhase.Main2 || Duel.Turn == 1;
        }

        private bool DesperationMonsterSet()
        {
            // Only set when our field is under severe threat and we need a survival shield
            if (Bot.GetMonsterCount() > 0) return false;
            if (Duel.Turn <= 1) return false; // Never set on Turn 1
            if (Enemy.GetMonsters().Any(m => m.IsFaceup() && m.IsAttack() && m.Attack >= 1500))
            {
                return Card != null && (Card.Id == CardId.MorganaTheWitchOfEyes || Card.Defense >= 1500);
            }
            return false;
        }

        private bool SmartMonsterRepos()
        {
            if (Card == null) return false;
            // 1. Handtraps or Low ATK / High DEF walls stranded in Attack -> Switch to Defense!
            if (Card.IsAttack() && (Card.Attack == 0 || (Card.Defense > Card.Attack && Card.Defense >= 1800)))
                return true;
            // 2. High ATK monsters accidentally in Defense when we can push for lethal -> Switch to Attack!
            if (Card.IsDefense() && Card.Attack > Card.Defense && Card.Attack >= 1800 && Duel.Turn > 1)
                return true;
            return DefaultMonsterRepos();
        }

        public override CardPosition OnSelectPosition(int cardId, IList<CardPosition> positions)
        {
            if (positions == null || positions.Count == 0) return CardPosition.FaceUpAttack;
            if (positions.Count == 1) return positions[0];

            var cardData = YGOSharp.OCGWrapper.NamedCard.Get(cardId);
            if (cardData != null)
            {
                if (cardData.HasType(CardType.Link))
                    return CardPosition.FaceUpAttack;

                // 1. Handtraps or 0 ATK -> 100% Defense
                if (cardData.Attack == 0 || CardIntelligence.IsHandtrap(cardId))
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 2. High DEF / Wall (DEF > ATK and ATK < 1800) -> Defense
                if (cardData.Defense > cardData.Attack && cardData.Attack < 1800)
                {
                    if (positions.Contains(CardPosition.FaceUpDefence)) return CardPosition.FaceUpDefence;
                    if (positions.Contains(CardPosition.FaceDownDefence)) return CardPosition.FaceDownDefence;
                }

                // 3. Boss / High ATK (ATK >= 1800) -> Attack
                if (cardData.Attack >= 1800 && positions.Contains(CardPosition.FaceUpAttack))
                    return CardPosition.FaceUpAttack;
            }

            return base.OnSelectPosition(cardId, positions);
        }

        public override IList<ClientCard> OnSelectCard(IList<ClientCard> cards, int min, int max, long hint, bool cancelable)
        {
            if (cards == null || cards.Count == 0)
                return base.OnSelectCard(cards, min, max, hint, cancelable);

            // Super Polymerization tribute/material selection: prioritize enemy threats
            if (hint == 511 || hint == 500) // HINTMSG_FMATERIAL or HINTMSG_RELEASE
            {
                var enemies = cards.Where(c => c.Controller == 1).OrderByDescending(c => c.Attack).ToList();
                if (enemies.Count >= min) return enemies.Take(max).ToList();
            }

            // Seventh Tachyon Extra Deck reveal: Number 107 (for Vanity's Ruler) or Number 104 (for Boarder)
            var tachyon107 = cards.FirstOrDefault(c => c.Id == CardId.Number107GalaxyEyesTachyonDragon);
            if (tachyon107 != null && (Plugin.FloodgateManager.IsGuiltMorganiteActive || Bot.GetMonsterCount() > 0))
                return new List<ClientCard> { tachyon107 };

            var tachyon104 = cards.FirstOrDefault(c => c.Id == CardId.Number104Masquerade);
            if (tachyon104 != null)
                return new List<ClientCard> { tachyon104 };

            // Seventh Tachyon deck search: Vanity's Ruler > Inspector Boarder
            var vanity = cards.FirstOrDefault(c => c.Id == CardId.VanitySRuler);
            if (vanity != null && (Plugin.FloodgateManager.IsGuiltMorganiteActive || Bot.GetMonsterCount() > 0))
                return new List<ClientCard> { vanity };

            var boarder = cards.FirstOrDefault(c => c.Id == CardId.InspectorBoarder);
            if (boarder != null)
                return new List<ClientCard> { boarder };

            // Seventh Tachyon top deck placement: place redundant spell or Morganite
            var topDeckFodder = cards.FirstOrDefault(c => c.Id == CardId.SeventhTachyon || c.Id == CardId.SuccumbingSongMorganite);
            if (topDeckFodder != null)
                return new List<ClientCard> { topDeckFodder };

            return base.OnSelectCard(cards, min, max, hint, cancelable);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  MASTER DECK PLUGIN: MorganiteStunPlugin
    // ═══════════════════════════════════════════════════════════════
    internal class MorganiteStunPlugin
    {
        private readonly MorganiteStunExecutor _exec;

        public MorganiteStunStrategy Strategy { get; }
        public MorganiteFloodgateManager FloodgateManager { get; }
        public SuperPolyAdvisor SuperPolyAdvisor { get; }
        public MorganiteMaterialScorer MaterialScorer { get; }
        public MorganiteActionScorer ActionScorer { get; }
        public MorganiteBoardAssessor BoardAssessor { get; }

        public MorganiteStunPlugin(MorganiteStunExecutor exec)
        {
            _exec = exec;
            Strategy = new MorganiteStunStrategy(exec);
            FloodgateManager = new MorganiteFloodgateManager(exec);
            SuperPolyAdvisor = new SuperPolyAdvisor(exec);
            MaterialScorer = new MorganiteMaterialScorer(exec);
            ActionScorer = new MorganiteActionScorer(exec);
            BoardAssessor = new MorganiteBoardAssessor(exec);
        }

        public void ResetTurnState()
        {
            Strategy.Reset();
            FloodgateManager.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: MorganiteStunStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class MorganiteStunStrategy
    {
        private readonly MorganiteStunExecutor _exec;

        public MorganiteStunStrategy(MorganiteStunExecutor exec)
        {
            _exec = exec;
        }

        public void Reset() { }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: MorganiteFloodgateManager
    // ═══════════════════════════════════════════════════════════════
    internal class MorganiteFloodgateManager
    {
        private readonly MorganiteStunExecutor _exec;

        public bool IsGuiltMorganiteActive { get; set; }
        public bool IsTimeMorganiteActive { get; set; }

        public bool HasVanitySRuler => _exec.Bot.GetMonsters().Any(m => m.IsFaceup() && m.Id == MorganiteStunExecutor.CardId.VanitySRuler);
        public bool HasInspectorBoarder => _exec.Bot.GetMonsters().Any(m => m.IsFaceup() && m.Id == MorganiteStunExecutor.CardId.InspectorBoarder);
        public bool HasMajestysFiend => _exec.Bot.GetMonsters().Any(m => m.IsFaceup() && m.Id == MorganiteStunExecutor.CardId.MajestysFiend);
        public bool HasSkillDrain => _exec.Bot.GetSpells().Any(s => s.IsFaceup() && s.Id == MorganiteStunExecutor.CardId.SkillDrain);

        public MorganiteFloodgateManager(MorganiteStunExecutor exec)
        {
            _exec = exec;
        }

        public void Reset()
        {
            // Check graveyard for Morganite continuous flags
            if (_exec.Bot.Graveyard.Any(c => c.Id == MorganiteStunExecutor.CardId.GuiltGrippingMorganite) ||
                _exec.Bot.Banished.Any(c => c.Id == MorganiteStunExecutor.CardId.GuiltGrippingMorganite))
                IsGuiltMorganiteActive = true;

            if (_exec.Bot.Graveyard.Any(c => c.Id == MorganiteStunExecutor.CardId.TimeTearingMorganite) ||
                _exec.Bot.Banished.Any(c => c.Id == MorganiteStunExecutor.CardId.TimeTearingMorganite))
                IsTimeMorganiteActive = true;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: SuperPolyAdvisor
    // ═══════════════════════════════════════════════════════════════
    internal class SuperPolyAdvisor
    {
        private readonly MorganiteStunExecutor _exec;

        public SuperPolyAdvisor(MorganiteStunExecutor exec)
        {
            _exec = exec;
        }

        public bool CanActivateSuperPoly()
        {
            var enemies = _exec.Enemy.GetMonsters().Where(m => m.IsFaceup()).ToList();
            if (enemies.Count < 2) return false;

            // 1. Mudragon of the Swamp: 2 monsters with same Attribute but different Types
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (enemies[i].Attribute == enemies[j].Attribute && enemies[i].Race != enemies[j].Race)
                        return true;
                }
            }

            // 2. Garura, Wings of Resonant Life: 2 monsters with same Type and Attribute, different names
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (enemies[i].Attribute == enemies[j].Attribute && enemies[i].Race == enemies[j].Race && enemies[i].Id != enemies[j].Id)
                        return true;
                }
            }

            // 3. Starving Venom: 2 DARK monsters on field
            var darks = enemies.Where(m => m.HasAttribute(CardAttribute.Dark) && !m.HasType(CardType.Token)).ToList();
            if (darks.Count >= 2) return true;

            // 4. Dragostapelia: 1 Fusion + 1 DARK
            bool hasFusion = enemies.Any(m => m.HasType(CardType.Fusion));
            bool hasDark = enemies.Any(m => m.HasAttribute(CardAttribute.Dark));
            if (hasFusion && hasDark && enemies.Count >= 2) return true;

            // 5. Earth Golem @Ignister: 1 Cyberse + 1 Link
            bool hasCyberse = enemies.Any(m => m.HasRace(CardRace.Cyberse));
            bool hasLink = enemies.Any(m => m.HasType(CardType.Link));
            if (hasCyberse && hasLink && enemies.Count >= 2) return true;

            return false;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: MorganiteMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class MorganiteMaterialScorer
    {
        private readonly MorganiteStunExecutor _exec;

        public MorganiteMaterialScorer(MorganiteStunExecutor exec)
        {
            _exec = exec;
        }

        public int ScoreMonsterAsMaterial(ClientCard card)
        {
            if (card == null) return 0;
            // Severe penalty for sacrificing on-field floodgates
            if (card.Id == MorganiteStunExecutor.CardId.VanitySRuler) return -10000;
            if (card.Id == MorganiteStunExecutor.CardId.InspectorBoarder) return -8000;
            if (card.Id == MorganiteStunExecutor.CardId.MajestysFiend) return -8000;
            return 0;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: MorganiteActionScorer
    // ═══════════════════════════════════════════════════════════════
    internal class MorganiteActionScorer
    {
        private readonly MorganiteStunExecutor _exec;

        public MorganiteActionScorer(MorganiteStunExecutor exec)
        {
            _exec = exec;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN HELPER: MorganiteBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class MorganiteBoardAssessor
    {
        private readonly MorganiteStunExecutor _exec;

        public MorganiteBoardAssessor(MorganiteStunExecutor exec)
        {
            _exec = exec;
        }

        public bool IsOpponentLockedOut()
        {
            return _exec.Plugin.FloodgateManager.HasVanitySRuler ||
                   (_exec.Plugin.FloodgateManager.HasInspectorBoarder && _exec.Plugin.FloodgateManager.HasSkillDrain);
        }
    }
}
