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
