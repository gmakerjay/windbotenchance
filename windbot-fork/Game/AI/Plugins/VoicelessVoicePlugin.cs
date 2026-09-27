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
internal class VoicelessVoicePlugin
    {
        public VoicelessVoiceExecutor Executor { get; }
        public VoicelessRitualAdvisor RitualAdvisor { get; }
        public VoicelessResourceLoop ResourceLoop { get; }
        public VoicelessMaterialScorer MaterialScorer { get; }
        public VoicelessBoardAssessor BoardAssessor { get; }

        public VoicelessVoicePlugin(VoicelessVoiceExecutor executor)
        {
            Executor = executor;
            RitualAdvisor = new VoicelessRitualAdvisor(executor);
            ResourceLoop = new VoicelessResourceLoop(executor);
            MaterialScorer = new VoicelessMaterialScorer(executor);
            BoardAssessor = new VoicelessBoardAssessor(executor);
        }
    }

    internal class VoicelessRitualAdvisor
    {
        private readonly VoicelessVoiceExecutor _executor;

        public VoicelessRitualAdvisor(VoicelessVoiceExecutor executor)
        {
            _executor = executor;
        }

        public bool CanNegateWithSkullGuardian()
        {
            // Skull Guardian only negates while Lo is face-up on our field
            return _executor.Bot.HasInMonstersZone(VoicelessVoiceExecutor.CardId.SkullGuardian) &&
                   _executor.Bot.HasInMonstersZone(VoicelessVoiceExecutor.CardId.LoThePrayers);
        }
    }

    internal class VoicelessResourceLoop
    {
        private readonly VoicelessVoiceExecutor _executor;

        public VoicelessResourceLoop(VoicelessVoiceExecutor executor)
        {
            _executor = executor;
        }

        public bool ShouldReviveLo()
        {
            return _executor.Bot.Graveyard.Any(c => c.Id == VoicelessVoiceExecutor.CardId.LoThePrayers) &&
                   !_executor.Bot.HasInMonstersZone(VoicelessVoiceExecutor.CardId.LoThePrayers);
        }
    }

    internal class VoicelessMaterialScorer
    {
        private readonly VoicelessVoiceExecutor _executor;

        public VoicelessMaterialScorer(VoicelessVoiceExecutor executor)
        {
            _executor = executor;
        }

        public int GetScore(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == VoicelessVoiceExecutor.CardId.SkullGuardian) return 10000;
            if (card.Id == VoicelessVoiceExecutor.CardId.LoThePrayers) return 8000;
            if (card.Id == VoicelessVoiceExecutor.CardId.SaffiraDivineDragon) return 6000;
            return 1000;
        }
    }

    internal class VoicelessBoardAssessor
    {
        private readonly VoicelessVoiceExecutor _executor;

        public VoicelessBoardAssessor(VoicelessVoiceExecutor executor)
        {
            _executor = executor;
        }

        public bool HasUntargetableProtection()
        {
            return _executor.Bot.HasInSpellZone(VoicelessVoiceExecutor.CardId.BarrierOfTheVoicelessVoice) &&
                   _executor.Bot.GetMonsters().Any(m => m.IsFaceup() && m.Attribute == (int)CardAttribute.Light);
        }
    }
}
