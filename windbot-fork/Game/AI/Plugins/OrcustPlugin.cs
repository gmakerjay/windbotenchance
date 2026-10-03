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
    //  MASTER DECK PLUGIN: OrcustPlugin (Layer 3 Decoupled Plugin)
    // ═══════════════════════════════════════════════════════════════
    public class OrcustPlugin : DeckPluginBase
    {
        public override string DeckName => "OrcustWCQ";

        public OrcustWCQExecutor Executor { get; }
        public OrcustStrategy OrcustStrat { get; }
        public OrcustThreatEvaluator OrcustThreat { get; }
        public OrcustMaterialScorer OrcustMat { get; }
        public OrcustBoardAssessor BoardAssessor { get; }

        public override IDeckStrategy Strategy => OrcustStrat;
        public override IDeckThreatEvaluator ThreatEvaluator => OrcustThreat;
        public override IDeckMaterialEvaluator MaterialEvaluator => OrcustMat;

        public OrcustPlugin(OrcustWCQExecutor executor)
        {
            Executor = executor;
            OrcustStrat = new OrcustStrategy(executor);
            OrcustThreat = new OrcustThreatEvaluator(executor);
            OrcustMat = new OrcustMaterialScorer(executor);
            BoardAssessor = new OrcustBoardAssessor(executor, OrcustStrat, OrcustMat);
        }

        public override void ResetTurnState()
        {
            base.ResetTurnState();
            OrcustStrat.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 1: OrcustStrategy
    // ═══════════════════════════════════════════════════════════════
    public class OrcustStrategy : IDeckStrategy
    {
        private readonly OrcustWCQExecutor _exec;

        // Once-Per-Turn Triggers
        public bool GirsuSummonUsed { get; set; }
        public bool GirsuTokenUsed { get; set; }
        public bool ScrapRecyclerUsed { get; set; }
        public bool ArmageddonKnightUsed { get; set; }
        public bool DarkGrepherSpSummonUsed { get; set; }
        public bool DarkGrepherDumpUsed { get; set; }
        public bool RotAUsed { get; set; }
        public bool HarpHorrorUsed { get; set; }
        public bool CymbalSkeletonUsed { get; set; }
        public bool OrcustKnightmareUsed { get; set; }
        public bool WorldWandUsed { get; set; }
        public bool BrassBombardUsed { get; set; }
        public bool GalateaUsed { get; set; }
        public bool GalateaISearchUsed { get; set; }
        public bool GalateaIGYUsed { get; set; }
        public bool DingirsuSummonedThisTurn { get; set; }
        public bool LongirsuUsed { get; set; }
        public bool EnlilgirsuHandUsed { get; set; }
        public bool EnlilgirsuGYUsed { get; set; }
        public bool OrcustratedReturnUsed { get; set; }
        public bool CrescendoGYUsed { get; set; }
        public bool TripleTacticsTalentUsed { get; set; }
        public bool IsDarkLockedThisTurn { get; set; }

        public OrcustStrategy(OrcustWCQExecutor executor)
        {
            _exec = executor;
        }

        public void Reset()
        {
            GirsuSummonUsed = false;
            GirsuTokenUsed = false;
            ScrapRecyclerUsed = false;
            ArmageddonKnightUsed = false;
            DarkGrepherSpSummonUsed = false;
            DarkGrepherDumpUsed = false;
            RotAUsed = false;
            HarpHorrorUsed = false;
            CymbalSkeletonUsed = false;
            OrcustKnightmareUsed = false;
            WorldWandUsed = false;
            BrassBombardUsed = false;
            GalateaUsed = false;
            GalateaISearchUsed = false;
            GalateaIGYUsed = false;
            DingirsuSummonedThisTurn = false;
            LongirsuUsed = false;
            EnlilgirsuHandUsed = false;
            EnlilgirsuGYUsed = false;
            OrcustratedReturnUsed = false;
            CrescendoGYUsed = false;
            TripleTacticsTalentUsed = false;
            IsDarkLockedThisTurn = false;
        }

        public bool HasBabelOnField()
        {
            return _exec.Bot.SpellZone.Any(s => s != null && s.IsFaceup() && s.Id == OrcustWCQExecutor.CardId.OrcustratedBabel);
        }

        public bool HasDingirsuInGY()
        {
            return _exec.Bot.Graveyard.Any(c => c.Id == OrcustWCQExecutor.CardId.Dingirsu || c.Id == OrcustWCQExecutor.CardId.DingirsuAlt);
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

        public bool HasOrcustLinkOnField()
        {
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && m.HasType(CardType.Link) && IsOrcustCard(m));
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
                   c.Id == OrcustWCQExecutor.CardId.DingirsuAlt ||
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
                   c.Id == OrcustWCQExecutor.CardId.Dingirsu ||
                   c.Id == OrcustWCQExecutor.CardId.DingirsuAlt;
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

        // IDeckStrategy implementation
        public ClientCard PickSpecialSummonTarget(IList<ClientCard> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;

            // Prioritize Dingirsu if available
            var dingirsu = candidates.FirstOrDefault(c => c.Id == OrcustWCQExecutor.CardId.Dingirsu || c.Id == OrcustWCQExecutor.CardId.DingirsuAlt);
            if (dingirsu != null) return dingirsu;

            var galatea = candidates.FirstOrDefault(c => c.Id == OrcustWCQExecutor.CardId.Galatea);
            if (galatea != null) return galatea;

            var girsu = candidates.FirstOrDefault(c => c.Id == OrcustWCQExecutor.CardId.Girsu);
            if (girsu != null) return girsu;

            var cymbal = candidates.FirstOrDefault(c => c.Id == OrcustWCQExecutor.CardId.OrcustCymbalSkeleton);
            if (cymbal != null) return cymbal;

            return candidates.FirstOrDefault();
        }

        public ClientCard PickSearchTarget(IList<ClientCard> candidates, ClientCard context)
        {
            if (candidates == null || candidates.Count == 0) return null;

            if (!HasBabelOnField() && !_exec.Bot.HasInHand(OrcustWCQExecutor.CardId.OrcustratedBabel))
            {
                var babel = candidates.FirstOrDefault(c => c.Id == OrcustWCQExecutor.CardId.OrcustratedBabel);
                if (babel != null) return babel;
            }

            var crescendo = candidates.FirstOrDefault(c => c.Id == OrcustWCQExecutor.CardId.OrcustCrescendo);
            if (crescendo != null) return crescendo;

            var arma = candidates.FirstOrDefault(c => c.Id == OrcustWCQExecutor.CardId.ArmageddonKnight);
            if (arma != null) return arma;

            var girsu = candidates.FirstOrDefault(c => c.Id == OrcustWCQExecutor.CardId.Girsu);
            if (girsu != null) return girsu;

            return candidates.FirstOrDefault();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 2: OrcustThreatEvaluator
    // ═══════════════════════════════════════════════════════════════
    public class OrcustThreatEvaluator : IDeckThreatEvaluator
    {
        private readonly OrcustWCQExecutor _exec;

        public OrcustThreatEvaluator(OrcustWCQExecutor executor)
        {
            _exec = executor;
        }

        public int EvaluateTargetPriority(ClientCard card)
        {
            if (card == null) return -9999;
            if (card.Controller == 0) return -10000;

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

        public int EvaluateThreatScore(ClientCard card)
        {
            return EvaluateTargetPriority(card);
        }

        public bool IsEmergencyThreat(ClientCard card)
        {
            if (card == null) return false;
            return card.Attack >= 3000 || card.Id == 82828051 || card.Id == 48680970;
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
    public class OrcustMaterialScorer : IDeckMaterialEvaluator
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
            if (card.Id == OrcustWCQExecutor.CardId.OrcustCymbalSkeleton) return 2400;
            if (card.Id == OrcustWCQExecutor.CardId.WorldWand) return 2300;
            if (card.Id == OrcustWCQExecutor.CardId.OrcustKnightmare) return 2200;
            if (card.Id == OrcustWCQExecutor.CardId.OrcustBrassBombard) return 2100;
            if (card.Id == OrcustWCQExecutor.CardId.OrcustCrescendo) return 1800;
            if (card.Id == OrcustWCQExecutor.CardId.GalateaI) return 1600;

            // Extra duplicate copies
            if (_exec.Bot.Hand.Count(c => c.Id == card.Id) > 1) return 1200;

            // Spells/Traps without immediate use
            if (card.IsSpell() && card.Id != OrcustWCQExecutor.CardId.OrcustratedBabel && card.Id != OrcustWCQExecutor.CardId.FoolishBurial) return 800;

            // Handtraps: save for interruption
            if (card.Id == OrcustWCQExecutor.CardId.AshBlossom || card.Id == OrcustWCQExecutor.CardId.InfiniteImpermanence) return 100;

            return 50;
        }

        public int ScoreTributeOrCostMaterial(ClientCard card)
        {
            if (card == null) return -9999;
            if (IsProtectedBoss(card)) return -9999;

            // Tokens or expendable materials
            if (card.HasType(CardType.Token)) return 3000;
            if (card.Id == OrcustWCQExecutor.CardId.ScrapRecycler && card.Location == CardLocation.MonsterZone) return 1900;
            if (card.Id == OrcustWCQExecutor.CardId.ArmageddonKnight && card.Location == CardLocation.MonsterZone) return 1900;
            if (card.Id == OrcustWCQExecutor.CardId.DarkGrepher && card.Location == CardLocation.MonsterZone) return 1800;
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
                   card.Id == OrcustWCQExecutor.CardId.DingirsuAlt ||
                   card.Id == OrcustWCQExecutor.CardId.Galatea ||
                   card.Id == OrcustWCQExecutor.CardId.Longirsu ||
                   card.Id == OrcustWCQExecutor.CardId.Enlilgirsu ||
                   card.Id == OrcustWCQExecutor.CardId.AccesscodeTalker ||
                   card.Id == OrcustWCQExecutor.CardId.SPLittleKnight ||
                   card.Id == OrcustWCQExecutor.CardId.IPMasquerena;
        }

        // IDeckMaterialEvaluator implementation
        public int GetMaterialCost(ClientCard card)
        {
            if (card == null) return 9999;
            if (IsProtectedBoss(card)) return 9999;
            return 1000 - ScoreTributeOrCostMaterial(card);
        }

        public IList<ClientCard> SortMaterials(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null) return new List<ClientCard>();
            return candidates.OrderByDescending(ScoreTributeOrCostMaterial).Take(min).ToList();
        }

        public ClientCard PickDiscardTarget(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            return candidates.OrderByDescending(ScoreDiscardMaterial).FirstOrDefault();
        }

        public ClientCard PickDestructionSubstitute(IList<ClientCard> candidates, int min = 1)
        {
            if (candidates == null || candidates.Count == 0) return null;
            // Dingirsu detach protection or token destroy
            var token = candidates.FirstOrDefault(c => c.HasType(CardType.Token));
            if (token != null) return token;
            return candidates.OrderByDescending(ScoreTributeOrCostMaterial).FirstOrDefault();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 4: OrcustBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    public class OrcustBoardAssessor
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
            if (_strategy.DingirsuSummonedThisTurn) return false;

            // Overlay 1 Orcust Link monster (Galatea, Longirsu, Enlilgirsu — NEVER Galatea-i!)
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                (m.Id == OrcustWCQExecutor.CardId.Galatea || m.Id == OrcustWCQExecutor.CardId.Longirsu || m.Id == OrcustWCQExecutor.CardId.Enlilgirsu));
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
