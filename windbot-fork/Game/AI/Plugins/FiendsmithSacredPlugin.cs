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
    //  MASTER DECK PLUGIN: FiendsmithSacredPlugin
    // ═══════════════════════════════════════════════════════════════
    internal class FiendsmithSacredPlugin
    {
        public FiendsmithSacredExecutor Executor { get; }
        public FiendsmithSacredStrategy Strategy { get; }
        public FiendsmithSacredThreatEvaluator ThreatEvaluator { get; }
        public FiendsmithSacredMaterialScorer MaterialScorer { get; }
        public FiendsmithSacredBoardAssessor BoardAssessor { get; }

        public FiendsmithSacredPlugin(FiendsmithSacredExecutor executor)
        {
            Executor = executor;
            Strategy = new FiendsmithSacredStrategy(executor);
            ThreatEvaluator = new FiendsmithSacredThreatEvaluator(executor);
            MaterialScorer = new FiendsmithSacredMaterialScorer(executor);
            BoardAssessor = new FiendsmithSacredBoardAssessor(executor);
        }

        public void ResetTurnState()
        {
            Strategy.Reset();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 1: FiendsmithSacredStrategy
    // ═══════════════════════════════════════════════════════════════
    internal class FiendsmithSacredStrategy
    {
        private readonly FiendsmithSacredExecutor _exec;

        // Hard Once-Per-Turn Triggers
        public bool EngraverHandUsed { get; set; }
        public bool EngraverGYUsed { get; set; }
        public bool TractUsed { get; set; }
        public bool LacrimaSummonUsed { get; set; }
        public bool RequiemTributeUsed { get; set; }
        public bool RequiemEquipUsed { get; set; }
        public bool SequenceFusionUsed { get; set; }
        public bool SequenceEquipUsed { get; set; }
        public bool FiendsmithLacrimaUsed { get; set; }
        public bool AgnumdayUsed { get; set; }
        public bool DesiraeUsed { get; set; }
        public bool CaesarUsedCount { get; set; } // up to 2 times
        public int CaesarNegatesCount { get; set; }

        // Sacred Beasts OPTs
        public bool HamonHandUsed { get; set; }
        public bool RavielHandUsed { get; set; }
        public bool UriaHandUsed { get; set; }
        public bool SummonerHandUsed { get; set; }
        public bool SummonerFieldUsed { get; set; }
        public bool SummonerGYUsed { get; set; }
        public bool MartyrSummonUsed { get; set; }
        public bool MartyrArmyUsed { get; set; }
        public bool ReleasedUsed { get; set; }
        public bool ThunderclapUsed { get; set; }
        public bool FallenParadiseDrawUsed { get; set; }
        public int FallenParadiseSSCount { get; set; } // up to 3 times
        public bool CombinedAssaultUsed { get; set; }
        public bool VarudrasUsed { get; set; }
        public int ChaoticPhantasmalNegateCount { get; set; } // up to 3 times
        public bool ColossusSummonedThisTurn { get; set; }
        public bool ThunderActivatedInHandThisTurn { get; set; }

        public FiendsmithSacredStrategy(FiendsmithSacredExecutor exec) => _exec = exec;

        public void Reset()
        {
            EngraverHandUsed = false;
            EngraverGYUsed = false;
            TractUsed = false;
            LacrimaSummonUsed = false;
            RequiemTributeUsed = false;
            RequiemEquipUsed = false;
            SequenceFusionUsed = false;
            SequenceEquipUsed = false;
            FiendsmithLacrimaUsed = false;
            AgnumdayUsed = false;
            DesiraeUsed = false;
            CaesarUsedCount = false;
            CaesarNegatesCount = 0;

            HamonHandUsed = false;
            RavielHandUsed = false;
            UriaHandUsed = false;
            SummonerHandUsed = false;
            SummonerFieldUsed = false;
            SummonerGYUsed = false;
            MartyrSummonUsed = false;
            MartyrArmyUsed = false;
            ReleasedUsed = false;
            ThunderclapUsed = false;
            FallenParadiseDrawUsed = false;
            FallenParadiseSSCount = 0;
            CombinedAssaultUsed = false;
            VarudrasUsed = false;
            ChaoticPhantasmalNegateCount = 0;
            ColossusSummonedThisTurn = false;
            ThunderActivatedInHandThisTurn = false;
        }

        public bool HasSacredBeastOnField()
        {
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && IsSacredBeastMonster(m));
        }

        public bool HasLevel10SacredBeastOnField()
        {
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && IsLevel10SacredBeast(m));
        }

        public int CountFaceupLevel10SacredBeasts()
        {
            return _exec.Bot.GetMonsters().Count(m => m != null && m.IsFaceup() && IsLevel10SacredBeast(m));
        }

        public bool HasThunderOnField()
        {
            return _exec.Bot.GetMonsters().Any(m => m != null && m.IsFaceup() && (m.Race & (int)CardRace.Thunder) != 0);
        }

        public static bool IsSacredBeastMonster(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == FiendsmithSacredExecutor.CardId.CalamityHamon ||
                   card.Id == FiendsmithSacredExecutor.CardId.InfinityRaviel ||
                   card.Id == FiendsmithSacredExecutor.CardId.InfernoUria ||
                   card.Id == FiendsmithSacredExecutor.CardId.SummonerOfTheSacredBeasts ||
                   card.Id == FiendsmithSacredExecutor.CardId.MartyrOfTheSacredBeasts ||
                   card.Id == FiendsmithSacredExecutor.CardId.TheChaoticPhantasmalSacredBeasts;
        }

        public static bool IsLevel10SacredBeast(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == FiendsmithSacredExecutor.CardId.CalamityHamon ||
                   card.Id == FiendsmithSacredExecutor.CardId.InfinityRaviel ||
                   card.Id == FiendsmithSacredExecutor.CardId.InfernoUria ||
                   card.Id == FiendsmithSacredExecutor.CardId.TheChaoticPhantasmalSacredBeasts;
        }

        public static bool IsFiendsmithCard(ClientCard card)
        {
            if (card == null) return false;
            return card.Id == FiendsmithSacredExecutor.CardId.FiendsmithEngraver ||
                   card.Id == FiendsmithSacredExecutor.CardId.FiendsmithsTract ||
                   card.Id == FiendsmithSacredExecutor.CardId.LacrimaTheCrimsonTears ||
                   card.Id == FiendsmithSacredExecutor.CardId.FiendsmithsRequiem ||
                   card.Id == FiendsmithSacredExecutor.CardId.FiendsmithsSequence ||
                   card.Id == FiendsmithSacredExecutor.CardId.FiendsmithsLacrima ||
                   card.Id == FiendsmithSacredExecutor.CardId.FiendsmithsAgnumday ||
                   card.Id == FiendsmithSacredExecutor.CardId.FiendsmithsDesirae;
        }

        public static bool IsLightFiend(ClientCard card)
        {
            if (card == null) return false;
            return (card.Race & (int)CardRace.Fiend) != 0 && (card.Attribute & (int)CardAttribute.Light) != 0;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 2: FiendsmithSacredThreatEvaluator
    // ═══════════════════════════════════════════════════════════════
    internal class FiendsmithSacredThreatEvaluator
    {
        private readonly FiendsmithSacredExecutor _exec;
        public FiendsmithSacredThreatEvaluator(FiendsmithSacredExecutor exec) => _exec = exec;

        public ClientCard GetBestRemovalTarget()
        {
            var targets = _exec.Enemy.GetMonsters().Concat(_exec.Enemy.GetSpells()).Where(c => c != null).ToList();
            if (!targets.Any()) return null;

            return targets
                .OrderByDescending(c => EvaluateTargetPriority(c))
                .FirstOrDefault();
        }

        public ClientCard GetBestMonsterRemovalTarget()
        {
            var targets = _exec.Enemy.GetMonsters().Where(m => m != null && m.IsFaceup()).ToList();
            if (!targets.Any()) return null;

            return targets
                .OrderByDescending(c => EvaluateTargetPriority(c))
                .FirstOrDefault();
        }

        public ClientCard GetBestSpellTrapRemovalTarget()
        {
            var targets = _exec.Enemy.GetSpells().Where(s => s != null).ToList();
            if (!targets.Any()) return null;

            return targets
                .OrderByDescending(s => s.IsFaceup() ? 200 : 80)
                .FirstOrDefault();
        }

        public int EvaluateTargetPriority(ClientCard card)
        {
            if (card == null) return 0;
            int score = 10;

            // Eternal Soul wipe condition: Destroying it wipes opponent's entire monster field!
            if (card.Id == 48680970) return 2500;

            if (CardIntelligence.IsFloodgate(card.Id)) score += 600;
            if (CardIntelligence.IsKnownNegator(card.Id)) score += 400;
            if (CardIntelligence.IsHighThreatChokepoint(card.Id)) score += 250;

            if (card.HasType(CardType.Monster))
            {
                score += card.Attack / 50;
                if (card.HasType(CardType.Fusion) || card.HasType(CardType.Synchro) ||
                    card.HasType(CardType.Xyz) || card.HasType(CardType.Link))
                {
                    score += 100;
                }
            }
            else
            {
                if (card.HasType(CardType.Continuous) || card.HasType(CardType.Field))
                    score += 150;
            }

            return score;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 3: FiendsmithSacredMaterialScorer
    // ═══════════════════════════════════════════════════════════════
    internal class FiendsmithSacredMaterialScorer
    {
        private readonly FiendsmithSacredExecutor _exec;
        public FiendsmithSacredMaterialScorer(FiendsmithSacredExecutor exec) => _exec = exec;

        public int ScoreSendCost(ClientCard card)
        {
            if (card == null) return 0;
            if (IsProtectedBoss(card)) return -99999;

            // Face-up Thunderclap on field was placed specifically to pay cost!
            if (card.Id == FiendsmithSacredExecutor.CardId.SacredBeastsThunderclap && card.Location == CardLocation.SpellZone)
                return 3000;

            if (card.HasType(CardType.Token)) return 2500;
            if (card.Id == FiendsmithSacredExecutor.CardId.FabledLurrie) return 2200;
            if (card.Id == FiendsmithSacredExecutor.CardId.MartyrOfTheSacredBeasts) return 2000;
            if (card.Id == FiendsmithSacredExecutor.CardId.MoonOfTheClosedHeaven) return 1800;
            if (card.Id == FiendsmithSacredExecutor.CardId.FiendsmithsRequiem) return 1600;
            if (card.Id == FiendsmithSacredExecutor.CardId.FiendsmithsSequence) return 1400;
            if (card.Id == FiendsmithSacredExecutor.CardId.LacrimaTheCrimsonTears) return 1000;

            return ScoreDiscardMaterial(card);
        }

        private static readonly HashSet<int> IrreplaceableBosses = new HashSet<int>
        {
            FiendsmithSacredExecutor.CardId.DDDWaveHighKingCaesar,
            FiendsmithSacredExecutor.CardId.Varudras,
            FiendsmithSacredExecutor.CardId.TheChaoticPhantasmalSacredBeasts,
            FiendsmithSacredExecutor.CardId.ThunderDragonColossus,
            FiendsmithSacredExecutor.CardId.FiendsmithsDesirae,
            FiendsmithSacredExecutor.CardId.SPLittleKnight
        };

        public bool IsProtectedBoss(ClientCard card)
        {
            if (card == null) return false;
            return IrreplaceableBosses.Contains(card.Id);
        }

        public int ScoreTributeOrCostMaterial(ClientCard card)
        {
            if (card == null) return 0;
            if (IsProtectedBoss(card)) return -99999; // NEVER sacrifice protected bosses!

            // Tokens and expendable fodder have top priority to be tributed/used
            if (card.HasType(CardType.Token)) return 1000;
            if (card.Id == FiendsmithSacredExecutor.CardId.FabledLurrie) return 900;
            if (card.Id == FiendsmithSacredExecutor.CardId.MartyrOfTheSacredBeasts) return 800;
            if (card.Id == FiendsmithSacredExecutor.CardId.MoonOfTheClosedHeaven) return 700;
            if (card.Id == FiendsmithSacredExecutor.CardId.FiendsmithsRequiem) return 600;
            if (card.Id == FiendsmithSacredExecutor.CardId.FiendsmithsSequence) return 500;
            if (card.Id == FiendsmithSacredExecutor.CardId.FiendsmithsLacrima) return 400;
            if (card.Id == FiendsmithSacredExecutor.CardId.LacrimaTheCrimsonTears) return 300;
            if (card.Id == FiendsmithSacredExecutor.CardId.SummonerOfTheSacredBeasts) return 200;

            return 50;
        }

        public int ScoreDiscardMaterial(ClientCard card)
        {
            if (card == null) return 0;

            // Fabled Lurrie: Triggers immediately when discarded! TOP DISCARD TARGET!
            if (card.Id == FiendsmithSacredExecutor.CardId.FabledLurrie) return 2000;

            // Fiendsmith Engraver: Has GY effect to revive self!
            if (card.Id == FiendsmithSacredExecutor.CardId.FiendsmithEngraver) return 1500;

            // Sacred Beasts Released: Has GY effect to banish and search Lv10!
            if (card.Id == FiendsmithSacredExecutor.CardId.SacredBeastsReleased) return 1200;

            // Summoner of Sacred Beasts: Has GY banish revival effect!
            if (card.Id == FiendsmithSacredExecutor.CardId.SummonerOfTheSacredBeasts) return 1000;

            // Duplicate Lv10 Sacred Beasts (Hamon/Raviel/Uria) if we already have copies
            if (card.Id == FiendsmithSacredExecutor.CardId.CalamityHamon ||
                card.Id == FiendsmithSacredExecutor.CardId.InfinityRaviel ||
                card.Id == FiendsmithSacredExecutor.CardId.InfernoUria)
            {
                int inHand = _exec.Bot.Hand.Count(c => c.Id == card.Id);
                return inHand > 1 ? 800 : 300;
            }

            // Martyr: Can be retrieved in EP if Lv10 in GY
            if (card.Id == FiendsmithSacredExecutor.CardId.MartyrOfTheSacredBeasts) return 600;

            // Duplicate Thunderclap
            if (card.Id == FiendsmithSacredExecutor.CardId.SacredBeastsThunderclap) return 500;

            // Kashtira Fenrir (if we already have one)
            if (card.Id == FiendsmithSacredExecutor.CardId.KashtiraFenrir) return 100;

            return 50;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  DOMAIN SUB-HELPER 4: FiendsmithSacredBoardAssessor
    // ═══════════════════════════════════════════════════════════════
    internal class FiendsmithSacredBoardAssessor
    {
        private readonly FiendsmithSacredExecutor _exec;
        public FiendsmithSacredBoardAssessor(FiendsmithSacredExecutor exec) => _exec = exec;

        public bool CanConfirmLethal()
        {
            if (_exec.Duel.Turn == 1) return false;
            int totalAttack = _exec.Bot.GetMonsters().Where(m => m != null && m.IsFaceup()).Sum(m => m.Attack);
            if (_exec.Enemy.GetMonsterCount() == 0 && totalAttack >= _exec.Enemy.LifePoints) return true;
            return false;
        }

        public int CountAvailableDisruptions()
        {
            int count = 0;
            if (_exec.Bot.HasInMonstersZone(FiendsmithSacredExecutor.CardId.DDDWaveHighKingCaesar)) count += 2;
            if (_exec.Bot.HasInMonstersZone(FiendsmithSacredExecutor.CardId.Varudras)) count += 1;
            if (_exec.Bot.HasInMonstersZone(FiendsmithSacredExecutor.CardId.TheChaoticPhantasmalSacredBeasts)) count += 2;
            if (_exec.Bot.HasInMonstersZone(FiendsmithSacredExecutor.CardId.ThunderDragonColossus)) count += 2;
            if (_exec.Bot.HasInMonstersZone(FiendsmithSacredExecutor.CardId.FiendsmithsDesirae)) count += 1;
            if (_exec.Bot.HasInMonstersZone(FiendsmithSacredExecutor.CardId.SPLittleKnight)) count += 1;
            if (_exec.Bot.HasInMonstersZone(FiendsmithSacredExecutor.CardId.IPMasquerena)) count += 1;
            if (_exec.Bot.HasInSpellZone(FiendsmithSacredExecutor.CardId.SacredBeastsCombinedAssault)) count += 1;

            count += _exec.Bot.Hand.Count(c => c.Id == FiendsmithSacredExecutor.CardId.AshBlossom ||
                                              c.Id == FiendsmithSacredExecutor.CardId.InfiniteImpermanence ||
                                              c.Id == FiendsmithSacredExecutor.CardId.GhostBelle ||
                                              c.Id == FiendsmithSacredExecutor.CardId.GhostOgre);
            return count;
        }
    }
}
