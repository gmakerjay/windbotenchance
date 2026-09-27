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
// ==========================================
    // Yubel Domain Plugin Classes
    // ==========================================
    public class YubelPlugin
    {
        public YubelPainReflectAdvisor PainAdvisor { get; } = new YubelPainReflectAdvisor();
        public YubelFusionAdvisor FusionAdvisor { get; } = new YubelFusionAdvisor();
        public YubelMaterialScorer MaterialScorer { get; } = new YubelMaterialScorer();
        public YubelBoardAssessor BoardAssessor { get; } = new YubelBoardAssessor();
    }

    public class YubelPainReflectAdvisor
    {
        public bool IsPainActive(ClientField bot) =>
            bot.HasInSpellZone(Yubel2Executor.CardId.NightmarePain);

        public bool ShouldAttackForReflectDamage(ClientCard attacker, ClientCard defender, bool hasPain)
        {
            if (attacker == null || defender == null) return false;
            if (!defender.IsAttack() || defender.Attack <= 0) return false;
            return hasPain ||
                   attacker.Id == Yubel2Executor.CardId.YubelTheLovingDefenderForever ||
                   attacker.Id == Yubel2Executor.CardId.YubelTheUltimateNightmare ||
                   attacker.Id == Yubel2Executor.CardId.ElementalHERONeosKluger;
        }
    }

    public class YubelFusionAdvisor
    {
        public bool CanSuperPolyLovingDefender(ClientField enemy)
        {
            return enemy.GetMonsterCount() >= 2;
        }
    }

    public class YubelMaterialScorer
    {
        public int ScoreYubelMaterial(ClientCard card)
        {
            if (card == null) return 0;
            if (card.Controller == 1) return 100; // enemy monster is best material
            if (card.IsCode(Yubel2Executor.CardId.FabledLurrie)) return 90;
            if (card.IsCode(Yubel2Executor.CardId.GruesomeGraveSquirmer)) return 80;
            return 20;
        }
    }

    public class YubelBoardAssessor
    {
        public bool HasLovingDefender(ClientField bot) =>
            bot.HasInMonstersZone(Yubel2Executor.CardId.YubelTheLovingDefenderForever);

        public bool HasNightmareThrone(ClientField bot) =>
            bot.HasInSpellZone(Yubel2Executor.CardId.NightmareThrone);
    }
}
