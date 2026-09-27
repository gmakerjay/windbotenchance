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
internal class BrandedPlugin
    {
        public _2026_BrandedExecutor Executor { get; }
        public BrandedFusionPlanner FusionPlanner { get; }
        public BrandedResourceCycle ResourceCycle { get; }
        public BrandedMaterialScorer MaterialScorer { get; }
        public BrandedBoardAssessor BoardAssessor { get; }

        public BrandedPlugin(_2026_BrandedExecutor executor)
        {
            Executor = executor;
            FusionPlanner = new BrandedFusionPlanner(executor);
            ResourceCycle = new BrandedResourceCycle(executor);
            MaterialScorer = new BrandedMaterialScorer(executor);
            BoardAssessor = new BrandedBoardAssessor(executor);
        }
    }

    internal class BrandedFusionPlanner
    {
        private readonly _2026_BrandedExecutor _executor;
        public BrandedFusionPlanner(_2026_BrandedExecutor executor) => _executor = executor;

        public bool CanFuseMirrorjade()
        {
            return !_executor.Bot.HasInMonstersZone(_2026_BrandedExecutor.CardId.MirrorjadeTheIcebladeDragon) &&
                   (_executor.Bot.HasInExtra(_2026_BrandedExecutor.CardId.MirrorjadeTheIcebladeDragon));
        }

        public bool HasAlbazAccess()
        {
            return _executor.Bot.HasInHandOrInMonstersZoneOrInGraveyard(_2026_BrandedExecutor.CardId.FallenOfAlbaz) ||
                   _executor.Bot.Deck.Any(c => c != null && c.Id == _2026_BrandedExecutor.CardId.FallenOfAlbaz);
        }
    }

    internal class BrandedResourceCycle
    {
        private readonly _2026_BrandedExecutor _executor;
        public BrandedResourceCycle(_2026_BrandedExecutor executor) => _executor = executor;

        public int CountBrandedSpellsInGY()
        {
            return _executor.Bot.Graveyard.Count(c => c != null && c.HasSetcode(0x15d) && c.IsSpell());
        }

        public bool NeedsEndPhaseRecycle()
        {
            return _executor.Bot.Graveyard.Any(c => c != null &&
                (c.Id == _2026_BrandedExecutor.CardId.AlbionTheBrandedDragon ||
                 c.Id == _2026_BrandedExecutor.CardId.TitanikladTheAshDragon));
        }
    }

    internal class BrandedMaterialScorer
    {
        private readonly _2026_BrandedExecutor _executor;
        public BrandedMaterialScorer(_2026_BrandedExecutor executor) => _executor = executor;

        public int Score(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Id == _2026_BrandedExecutor.CardId.TheGoldenSwordsoul) return 100;
            if (card.Id == _2026_BrandedExecutor.CardId.TriBrigadeSpringansKitt) return 90;
            if (card.Id == _2026_BrandedExecutor.CardId.FallenOfAlbaz) return 80;
            if (_executor.IsAceCard(card)) return -500;
            return 10;
        }
    }

    internal class BrandedBoardAssessor
    {
        private readonly _2026_BrandedExecutor _executor;
        public BrandedBoardAssessor(_2026_BrandedExecutor executor) => _executor = executor;

        public bool HasOptimalEndBoard()
        {
            bool hasMirrorjade = _executor.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                m.Id == _2026_BrandedExecutor.CardId.MirrorjadeTheIcebladeDragon);
            bool hasRindbrummOrGranguignol = _executor.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() &&
                (m.Id == _2026_BrandedExecutor.CardId.RindbrummTheStrikingDragon ||
                 m.Id == _2026_BrandedExecutor.CardId.GranguignolTheDuskDragon));
            return hasMirrorjade && hasRindbrummOrGranguignol;
        }
    }
}
