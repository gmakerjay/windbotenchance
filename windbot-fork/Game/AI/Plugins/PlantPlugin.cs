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
public class PlantPlugin
    {
        private readonly _2026_PlantExecutor _exec;
        public PlantPlugin(_2026_PlantExecutor exec) => _exec = exec;

        public bool HasKonkonActive => _exec.Bot.HasInSpellZone(_2026_PlantExecutor.CardId.RikkaKonkon);
        public bool HasRegulusOnField => _exec.Bot.HasInMonstersZone(_2026_PlantExecutor.CardId.TherionKingRegulus);
        public bool HasTeardropOnField => _exec.Bot.HasInMonstersZone(_2026_PlantExecutor.CardId.TeardropTheRikkaQueen);
    }

    public class PlantComboAdvisor
    {
        private readonly _2026_PlantExecutor _exec;
        public PlantComboAdvisor(_2026_PlantExecutor exec) => _exec = exec;

        public bool CanStartSunavalonLine()
        {
            return _exec.Bot.HasInHand(_2026_PlantExecutor.CardId.SunseedGeniusLoci) &&
                   !_exec.IsSpSummonBlocked();
        }

        public bool CanStartRikkaLine()
        {
            return (_exec.Bot.HasInHand(_2026_PlantExecutor.CardId.RikkaPetal) ||
                    _exec.Bot.HasInHand(_2026_PlantExecutor.CardId.RikkaGlamour)) &&
                   !_exec.IsSpSummonBlocked();
        }
    }

    public class PlantTributeAdvisor
    {
        private readonly _2026_PlantExecutor _exec;
        public PlantTributeAdvisor(_2026_PlantExecutor exec) => _exec = exec;

        public ClientCard BestTributeCostTarget()
        {
            // If Konkon is active, tribute opponent monster if possible
            if (_exec.Bot.HasInSpellZone(_2026_PlantExecutor.CardId.RikkaKonkon))
            {
                var oppMonster = _exec.Enemy.GetMonsters().OrderByDescending(c => c.Attack).FirstOrDefault();
                if (oppMonster != null) return oppMonster;
            }
            return _exec.Bot.GetMonsters()
                .Where(c => c != null && c.IsFaceup() && !_exec.IsAceCard(c))
                .OrderBy(c => _exec.GetMaterialPriority(c))
                .FirstOrDefault();
        }
    }

    public class PlantMaterialScorer
    {
        public static int GetScore(ClientCard c)
        {
            if (c == null) return 999;
            if (c.Id == _2026_PlantExecutor.CardId.TeardropTheRikkaQueen ||
                c.Id == _2026_PlantExecutor.CardId.TherionKingRegulus ||
                c.Id == _2026_PlantExecutor.CardId.SacredTreeBeastHyperyton)
                return 950;
            if (c.Id == _2026_PlantExecutor.CardId.RikkaQueenStrenna)
                return 750;
            if (c.Id == _2026_PlantExecutor.CardId.SunseedGeniusLoci || c.Id == _2026_PlantExecutor.CardId.SunseedTwin)
                return 200;
            return 500;
        }
    }

    public class PlantBoardAssessor
    {
        private readonly _2026_PlantExecutor _exec;
        public PlantBoardAssessor(_2026_PlantExecutor exec) => _exec = exec;

        public int EvaluatePlantBoardScore()
        {
            int score = 0;
            if (_exec.Bot.HasInMonstersZone(_2026_PlantExecutor.CardId.TeardropTheRikkaQueen)) score += 40;
            if (_exec.Bot.HasInMonstersZone(_2026_PlantExecutor.CardId.TherionKingRegulus)) score += 40;
            if (_exec.Bot.HasInMonstersZone(_2026_PlantExecutor.CardId.SacredTreeBeastHyperyton)) score += 35;
            if (_exec.Bot.HasInSpellZone(_2026_PlantExecutor.CardId.RikkaKonkon)) score += 25;
            if (_exec.Bot.HasInSpellZone(_2026_PlantExecutor.CardId.RikkaSheet)) score += 20;
            if (_exec.Bot.HasInHand(_2026_PlantExecutor.CardId.RikkaPrincess) || _exec.Bot.HasInGraveyard(_2026_PlantExecutor.CardId.RikkaPrincess)) score += 20;
            return score;
        }
    }
}
