using System;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.AI.Decks;
using WindBot.Game.AI.Plugin;
using YGOSharp.OCGWrapper.Enums;

namespace WindBot.Game.AI.Plugins
{
    // ═══════════════════════════════════════════════════════════════
    //  MASTER DECK PLUGIN: OrcustPlugin
    // ═══════════════════════════════════════════════════════════════
    internal class OrcustPlugin
    {
        public OrcustWCQExecutor Executor { get; }
        public OrcustStrategy Strategy { get; }
        public OrcustThreatEvaluator ThreatEvaluator { get; }
        public OrcustMaterialScorer MaterialScorer { get; }
        public OrcustBoardAssessor BoardAssessor { get; }

        public OrcustPlugin(OrcustWCQExecutor executor)
        {
            Executor = executor;
            Strategy = new OrcustStrategy(executor);
            ThreatEvaluator = new OrcustThreatEvaluator(executor);
            MaterialScorer = new OrcustMaterialScorer(executor);
            BoardAssessor = new OrcustBoardAssessor(executor, Strategy, MaterialScorer);
        }

        public void ResetTurnState()
        {
            Strategy.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 1: OrcustStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class OrcustStrategy
    {
        private readonly OrcustWCQExecutor _exec;

        // Once-Per-Turn Triggers
        public bool GirsuSummonUsed { get; set; }
        public bool GirsuTokenUsed { get; set; }
        public bool HarpHorrorUsed { get; set; }
        public bool CymbalSkeletonUsed { get; set; }
        public bool OrcustKnightmareUsed { get; set; }
        public bool WorldWandUsed { get; set; }
        public bool GalateaUsed { get; set; }
        public bool GalateaISearchUsed { get; set; }
        public bool GalateaIGYUsed { get; set; }
        public bool DingirsuSummonedThisTurn { get; set; }
        public bool LongirsuUsed { get; set; }
        public bool EnlilgirsuHandUsed { get; set; }
        public bool EnlilgirsuGYUsed { get; set; }
        public bool OrcustratedReturnUsed { get; set; }
        public bool CrescendoGYUsed { get; set; }
        public bool BabelActivatedThisTurn { get; set; }
        public bool IsDarkLockedThisTurn { get; set; }

        public OrcustStrategy(OrcustWCQExecutor executor)
        {
            _exec = executor;
        }

        public void Reset()
        {
            GirsuSummonUsed = false;
            GirsuTokenUsed = false;
            HarpHorrorUsed = false;
            CymbalSkeletonUsed = false;
            OrcustKnightmareUsed = false;
            WorldWandUsed = false;
            GalateaUsed = false;
            GalateaISearchUsed = false;
            GalateaIGYUsed = false;
            DingirsuSummonedThisTurn = false;
            LongirsuUsed = false;
            EnlilgirsuHandUsed = false;
            EnlilgirsuGYUsed = false;
            OrcustratedReturnUsed = false;
            CrescendoGYUsed = false;
            BabelActivatedThisTurn = false;
            IsDarkLockedThisTurn = false;
        }

        public bool HasBabelOnField()
        {
            return _exec.Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == OrcustWCQExecutor.CardId.OrcustratedBabel);
        }

        public bool HasDingirsuInGY()
        {
            return _exec.Bot.Graveyard.Any(c => c.Id == OrcustWCQExecutor.CardId.Dingirsu);
        }

        public bool HasCymbalInGY()
        {
            return _exec.Bot.Graveyard.Any(c => c.Id == OrcustWCQExecutor.CardId.OrcustCymbalSkeleton);
        }

        public bool HasHarpInGY()
        {
            return _exec.Bot.Graveyard.Any(c => c.Id == OrcustWCQExecutor.CardId.OrcustHarpHorror);
        }

        public bool HasOrcustKnightmareInGY()
        {
            return _exec.Bot.Graveyard.Any(c => c.Id == OrcustWCQExecutor.CardId.OrcustKnightmare);
        }

        public static bool IsOrcustCard(ClientCard c)
        {
            if (c == null) return false;
            return c.Id == OrcustWCQExecutor.CardId.Girsu ||
                   c.Id == OrcustWCQExecutor.CardId.OrcustHarpHorror ||
                   c.Id == OrcustWCQExecutor.CardId.OrcustCymbalSkeleton ||
                   c.Id == OrcustWCQExecutor.CardId.OrcustKnightmare ||
                   c.Id == OrcustWCQExecutor.CardId.OrcustBrassBombard ||
                   c.Id == OrcustWCQExecutor.CardId.Galatea ||
                   c.Id == OrcustWCQExecutor.CardId.GalateaI ||
                   c.Id == OrcustWCQExecutor.CardId.Longirsu ||
                   c.Id == OrcustWCQExecutor.CardId.Enlilgirsu ||
                   c.Id == OrcustWCQExecutor.CardId.Dingirsu ||
                   c.Id == OrcustWCQExecutor.CardId.OrcustratedBabel ||
                   c.Id == OrcustWCQExecutor.CardId.OrcustratedReturn ||
                   c.Id == OrcustWCQExecutor.CardId.OrcustCrescendo;
        }

        public static bool IsOrcustMonster(ClientCard c)
        {
            if (c == null) return false;
            return c.Id == OrcustWCQExecutor.CardId.Girsu ||
                   c.Id == OrcustWCQExecutor.CardId.OrcustHarpHorror ||
                   c.Id == OrcustWCQExecutor.CardId.OrcustCymbalSkeleton ||
                   c.Id == OrcustWCQExecutor.CardId.OrcustKnightmare ||
                   c.Id == OrcustWCQExecutor.CardId.OrcustBrassBombard ||
                   c.Id == OrcustWCQExecutor.CardId.Galatea ||
                   c.Id == OrcustWCQExecutor.CardId.GalateaI ||
                   c.Id == OrcustWCQExecutor.CardId.Longirsu ||
                   c.Id == OrcustWCQExecutor.CardId.Enlilgirsu ||
                   c.Id == OrcustWCQExecutor.CardId.Dingirsu;
        }

        public static bool IsWorldLegacyCard(ClientCard c)
        {
            if (c == null) return false;
            return c.Id == OrcustWCQExecutor.CardId.WorldWand ||
                   c.Id == OrcustWCQExecutor.CardId.WorldCrown;
        }

        public static bool IsDarkMachine(ClientCard c)
        {
            if (c == null) return false;
            return (c.Race & (int)CardRace.Machine) != 0 && (c.Attribute & (int)CardAttribute.Dark) != 0;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 2: OrcustThreatEvaluator
    // ═══════════════════════════════════════════════════════════════
    internal class OrcustThreatEvaluator
    {
        private readonly OrcustWCQExecutor _exec;

        public OrcustThreatEvaluator(OrcustWCQExecutor executor)
        {
            _exec = executor;
        }

        public int EvaluateTargetPriority(ClientCard card)
        {
            if (card == null) return -9999;
            if (card.Controller == 0) return -10000; // Never target own cards for enemy removal

            int score = 0;

            // 1. Critical S/T Floodgates (Grade S)
            if (card.Id == 48680970) score += 2500; // Eternal Soul
            else if (card.Id == 68462976) score += 2200; // Secret Village of the Spellcasters
            else if (card.Id == 38009249) score += 2000; // Runick Fountain
            else if (card.Id == 66970002) score += 1800; // Union Hangar
            else if (card.Id == 47222536) score += 1700; // Dark Magical Circle
            else if (card.Id == 82828051) score += 2200; // Skill Drain

            // 2. Continuous / Field Spells
            if (card.IsFaceup() && (card.HasType(CardType.Continuous) || card.HasType(CardType.Field)))
            {
                score += 1200;
            }

            // 3. Boss Monsters / Floodgates
            if (card.Location == CardLocation.MonsterZone)
            {
                if (card.Attack >= 3000) score += 1500;
                else if (card.Attack >= 2500) score += 1200;
                else score += card.Attack / 2;

                if (card.HasType(CardType.Fusion) || card.HasType(CardType.Synchro) || card.HasType(CardType.Xyz) || card.HasType(CardType.Link))
                {
                    score += 800;
                }
            }

            // 4. Backrow threats
            if (card.Location == CardLocation.SpellZone && card.IsFacedown())
            {
                score += 700;
            }

            return score;
        }

        public ClientCard GetBestRemovalTarget()
        {
            var enemies = _exec.Enemy.GetMonsters().Concat(_exec.Enemy.GetSpells())
                .Where(c => c != null)
                .OrderByDescending(EvaluateTargetPriority)
                .ToList();

            return enemies.FirstOrDefault();
        }

        public ClientCard GetBestSpellTrapRemovalTarget()
        {
            return _exec.Enemy.GetSpells()
                .Where(s => s != null)
                .OrderByDescending(EvaluateTargetPriority)
                .FirstOrDefault();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 3: OrcustMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class OrcustMaterialScorer
    {
        private readonly OrcustWCQExecutor _exec;

        public OrcustMaterialScorer(OrcustWCQExecutor executor)
        {
            _exec = executor;
        }

        public int ScoreDiscardMaterial(ClientCard card)
        {
            if (card == null) return -9999;
            if (IsProtectedBoss(card)) return -9999;

            // Cards that thrive in GY
            if (card.Id == OrcustWCQExecutor.CardId.OrcustHarpHorror) return 2500;
            if (card.Id == OrcustWCQExecutor.CardId.OrcustCymbalSkeleton) return 2300;
            if (card.Id == OrcustWCQExecutor.CardId.WorldWand) return 2100;
            if (card.Id == OrcustWCQExecutor.CardId.OrcustKnightmare) return 2000;
            if (card.Id == OrcustWCQExecutor.CardId.OrcustCrescendo) return 1800;
            if (card.Id == OrcustWCQExecutor.CardId.TheBlackGoatLaughs) return 1700;
            if (card.Id == OrcustWCQExecutor.CardId.GalateaI) return 1600;

            // Extra duplicate copies
            if (_exec.Bot.Hand.Count(c => c.Id == card.Id) > 1) return 1200;

            // Spells/Traps without immediate use
            if (card.IsSpell() && card.Id != OrcustWCQExecutor.CardId.OrcustratedBabel && card.Id != OrcustWCQExecutor.CardId.FoolishBurial) return 800;

            // Handtraps: save for interruption
            if (card.Id == OrcustWCQExecutor.CardId.AshBlossom || card.Id == OrcustWCQExecutor.CardId.DrollAndLockBird) return 100;

            return 50;
        }

        public int ScoreTributeOrCostMaterial(ClientCard card)
        {
            if (card == null) return -9999;
            if (IsProtectedBoss(card)) return -9999;

            // Tokens or expendable materials
            if (card.HasType(CardType.Token)) return 3000;
            if (card.Id == OrcustWCQExecutor.CardId.Girsu && card.Location == CardLocation.MonsterZone) return 1500;
            if (card.Id == OrcustWCQExecutor.CardId.OrcustHarpHorror && card.Location == CardLocation.MonsterZone) return 1600;
            if (card.Id == OrcustWCQExecutor.CardId.OrcustCymbalSkeleton && card.Location == CardLocation.MonsterZone) return 1600;
            if (card.Id == OrcustWCQExecutor.CardId.GalateaI && card.Location == CardLocation.MonsterZone) return 1800;

            return 100;
        }

        public bool IsProtectedBoss(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == OrcustWCQExecutor.CardId.Dingirsu ||
                   card.Id == OrcustWCQExecutor.CardId.AccesscodeTalker ||
                   card.Id == OrcustWCQExecutor.CardId.SPLittleKnight ||
                   card.Id == OrcustWCQExecutor.CardId.IPMasquerena ||
                   card.Id == OrcustWCQExecutor.CardId.Enlilgirsu;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 4: OrcustBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class OrcustBoardAssessor
    {
        private readonly OrcustWCQExecutor _exec;
        private readonly OrcustStrategy _strategy;
        private readonly OrcustMaterialScorer _materialScorer;

        public OrcustBoardAssessor(OrcustWCQExecutor executor, OrcustStrategy strategy, OrcustMaterialScorer materialScorer)
        {
            _exec = executor;
            _strategy = strategy;
            _materialScorer = materialScorer;
        }

        public bool HasLethalOnBoard()
        {
            int totalAtk = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            return totalAtk >= _exec.Enemy.LifePoints && _exec.Enemy.GetMonsterCount() == 0;
        }

        public bool CanMakeDingirsu()
        {
            if (_exec.Bot.HasInMonstersZone(OrcustWCQExecutor.CardId.Dingirsu)) return false;
            if (_strategy.DingirsuSummonedThisTurn) return false;

            // Overlay 1 Orcust Link monster
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                (m.Id == OrcustWCQExecutor.CardId.Galatea || m.Id == OrcustWCQExecutor.CardId.GalateaI || m.Id == OrcustWCQExecutor.CardId.Longirsu));
        }

        public bool CanMakeGalatea()
        {
            if (_strategy.GalateaUsed) return false;
            int count = _exec.Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && !_materialScorer.IsProtectedBoss(m));
            bool hasOrcust = _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && OrcustStrategy.IsOrcustMonster(m));
            return count >= 2 && hasOrcust;
        }
    }
}
