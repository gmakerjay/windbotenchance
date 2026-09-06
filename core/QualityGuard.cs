using System;
using System.Collections.Generic;
using System.Linq;

namespace YgoAiPlatform.Core
{
    /// <summary>
    /// 🛡️ QualityGuard — คะแนนคุณภาพการตัดสินใจแบบ Multi-Signal Weighted
    /// 
    /// หลักการ: ไม่ใช้ Binary Filter (เก็บ/ทิ้ง) แต่ให้ "คะแนนคุณภาพ" กับทุกการตัดสินใจ
    /// → ตัวอย่างคุณภาพสูงได้น้ำหนักมากตอนเทรน
    /// → ตัวอย่างคุณภาพต่ำก็ยังอยู่ (น้ำหนักน้อย) เพื่อกัน overfitting และ generalization
    /// 
    /// Quality Score = (0.35 × Confidence) + (0.25 × Impact) + (0.25 × Outcome) + (0.15 × Novelty)
    /// 
    /// Integration points (all real APIs):
    ///   NeuralExecutor.OnSelectIdleCmd  → QualityGuard.ScoreMainPhase(...)
    ///   NeuralExecutor.OnBattle         → QualityGuard.ScoreBattle(...)
    ///   NeuralExecutor.FinalizeAndFlushSamples → QualityGuard.ScoreOutcome(...)
    ///   BatchTrainer.ConvertToTrainingSample    → reads quality_score from DatasetSample
    /// </summary>
    public static class QualityGuard
    {
        /// <summary>
        /// Feature index → weight contribution for confidence gap calculation.
        /// Maps to NeuralEvaluator.FEATURE_COUNT = 36.
        /// </summary>
        private static readonly double[] ConfidenceWeights = new double[36]
        {
            0.5, 0.5, 0.3, 0.3, 0.3,  // features 0-4: LP, hand, field (moderate signal)
            0.2, 0.2, 0.2, 0.2, 0.2,  // features 5-9: action type one-hot (weak — these are what's being chosen)
            0.0,                        // feature 10: baseScore (excluded — this is the bias we want to overcome)
            0.4, 0.4, 0.4,            // features 11-13: ATK, DEF, Level (strong signal — card stats matter)
            0.5, 0.4, 0.4,            // features 14-16: Monster/Spell/Trap flags
            0.5, 0.4, 0.3, 0.4,       // features 17-20: Fusion/Link/Xyz/Synchro
            0.3, 0.3, 0.2, 0.2,      // features 21-24: GY, Banished, ExtraDeck
            0.4, 0.3, 0.3,            // features 25-27: LP adv, Turn, backrow
            0.3, 0.2, 0.1, 0.2,      // features 28-31: SpSummon, Repos, reserved, cardId
            0.2, 0.3, 0.4, 0.5        // features 32-35: GY spells, field spell, chokepoint, counter
        };

        /// <summary>
        /// คำนวณ Confidence Gap — ความมั่นใจในการเลือก move นี้เทียบกับ alternatives
        /// 
        /// ใช้ NeuralEvaluator.ComputeScore (API จริง บรรทัด 174 NeuralEvaluator.cs)
        /// → score แต่ละ candidate → softmax → gap ระหว่าง chosen กับ runner-up
        /// 
        /// HIGH confidence = ชัดเจนว่าอันนี้ดีที่สุด → quality สูง (0.8-1.0)
        /// LOW confidence = มีหลายตัวเลือกพอๆ กัน → quality ต่ำ (0.2-0.5)
        /// </summary>
        /// <param name="scores">Scores for each candidate (caller computes via NeuralEvaluator.ComputeScore)</param>
        /// <param name="chosenIndex">Index of the chosen action in the scores array</param>
        /// <returns>Confidence score 0.0-1.0. Higher = more decisive decision.</returns>
        public static double ComputeConfidenceGap(double[] scores, int chosenIndex)
        {
            if (scores == null || scores.Length < 2) return 0.5;
            if (chosenIndex < 0 || chosenIndex >= scores.Length) return 0.3;

            // Softmax all scores
            double maxScore = scores.Max();
            double sum = 0.0;
            double[] probs = new double[scores.Length];
            for (int i = 0; i < scores.Length; i++)
            {
                probs[i] = Math.Exp(scores[i] - maxScore);
                sum += probs[i];
            }
            for (int i = 0; i < scores.Length; i++)
                probs[i] /= sum;

            double chosenProb = probs[chosenIndex];

            // Find the best alternative (runner-up)
            double bestAlternative = 0.0;
            for (int i = 0; i < probs.Length; i++)
            {
                if (i != chosenIndex && probs[i] > bestAlternative)
                    bestAlternative = probs[i];
            }

            // Confidence gap: chosen vs runner-up
            double gap = chosenProb - bestAlternative;

            // Normalize: gap of 0 = random choice, gap of 1.0 = certain choice
            // Clip to [0, 1] range
            double confidence = Math.Max(0.0, Math.Min(1.0, gap * 2.0)); // scale up since gaps are usually small
            return confidence;
        }

        /// <summary>
        /// คำนวณ State Impact — การตัดสินใจนี้เปลี่ยนเกมมากแค่ไหน?
        /// 
        /// เรียกจาก NeuralExecutor.OnSelectIdleCmd หรือ OnBattle
        /// โดยใช้ prev state (ก่อนตัดสินใจ) และ current state (หลังตัดสินใจ)
        /// 
        /// HIGH impact = Summon บอส, Activate board wipe, Attack กำจัด threat
        /// LOW impact = Pass เฉยๆ, Set การ์ดแบบไม่มีผลทันที
        /// </summary>
        /// <param name="actionName">"Summon" / "Activate" / "Attack" / "SpSummon" / "Set" / "Repos" / "Pass"</param>
        /// <param name="card">การ์ดที่ใช้ — null ถ้าเป็น Pass</param>
        /// <param name="lpChange">LP เปลี่ยนไปเท่าไหร่ (current - prev, normalized by 8000)</param>
        /// <param name="fieldChange">จำนวน monster บน field เปลี่ยนไปเท่าไหร่</param>
        /// <returns>Impact score 0.0-1.0</returns>
        public static double ComputeStateImpact(string actionName, object? card, double lpChange = 0, int fieldChange = 0)
        {
            double impact = 0.0;

            // Base impact by action type (sourced from NeuralExecutor.cs hardcoded BaseScore domain knowledge)
            switch (actionName?.ToLower())
            {
                case "spsummon":
                    impact = 0.9; // Special summons are typically combo pieces or bosses
                    break;
                case "summon":
                    impact = 0.7; // Normal summon is a key resource
                    break;
                case "activate":
                    impact = 0.8; // Activating effects = highest impact
                    break;
                case "attack":
                    impact = 0.6; // Attacking can swing LP
                    break;
                case "set":
                    impact = 0.3; // Setting is defensive/setup
                    break;
                case "repos":
                    impact = 0.2; // Position change is minor
                    break;
                case "pass":
                    impact = 0.05; // Passing = minimal impact
                    break;
                default:
                    impact = 0.3;
                    break;
            }

            // LP change adjustment: big LP swings = high impact
            double lpImpact = Math.Abs(lpChange) * 0.2;
            impact = Math.Min(1.0, impact + lpImpact);

            // Field change: gaining board presence = higher impact
            if (fieldChange > 0) impact = Math.Min(1.0, impact + 0.1);
            if (fieldChange < 0) impact = Math.Max(0.0, impact - 0.1);

            // Pass with available resources = wasted opportunity, negative impact signal
            if (actionName?.Equals("Pass", StringComparison.OrdinalIgnoreCase) == true)
            {
                impact = 0.05; // Always very low for pass
            }

            return Math.Max(0.0, Math.Min(1.0, impact));
        }

        /// <summary>
        /// Outcome Alignment — เกมที่ชนะ → การตัดสินใจนี้ "น่าจะดี"
        /// 
        /// เรียกจาก NeuralExecutor.FinalizeAndFlushSamples พร้อม duelResult
        /// 
        /// Win  = high alignment (0.8-1.0)
        /// Lose = low alignment but still worth SOME learning (0.1-0.4)
        /// </summary>
        /// <param name="duelResult">"Win" or "Lose"</param>
        /// <param name="turn">Turn number when this decision was made</param>
        /// <param name="maxTurn">Total turns in the duel (or high estimate)</param>
        /// <returns>Alignment score 0.0-1.0</returns>
        public static double ComputeOutcomeAlignment(string? duelResult, int turn, int maxTurn)
        {
            if (string.IsNullOrEmpty(duelResult))
                return 0.3; // Unknown result = neutral

            bool isWin = duelResult.Equals("Win", StringComparison.OrdinalIgnoreCase);

            if (isWin)
            {
                // Early game decisions in a win = good setup. Late game = finishing blow.
                // Both are valuable, but early game is more transferable.
                double earlyBonus = turn < maxTurn * 0.3 ? 0.15 : 0.0;
                return Math.Min(1.0, 0.75 + earlyBonus);
            }
            else
            {
                // Loss: decisions still have value as "what NOT to do"
                // Early game decisions in a loss = may still be correct (lost later). Higher value.
                // Late game decisions = probably bad. Lower value.
                double position = (double)turn / Math.Max(1, maxTurn);
                double alignment = 0.4 * (1.0 - position); // decays from 0.4 → 0.0 as game progresses
                return Math.Max(0.05, alignment);
            }
        }

        /// <summary>
        /// Novelty Score — สถานการณ์นี้แปลกใหม่แค่ไหน?
        /// 
        /// เทียบ state hash กับ history → ถ้าไม่เคยเจอ → novel = high value
        /// ช่วยให้ข้อมูลมีความหลากหลาย → generalization ดีขึ้น
        /// 
        /// เรียกจาก DiversityTracker.GetNoveltyScore(stateHash)
        /// API จริง: GameState.GetStateHash() (บรรทัด 37, GameState.cs)
        /// </summary>
        public static double ComputeNoveltyScore(double similarityToClosestMatch)
        {
            // similarity 0.0 = completely novel → score 1.0
            // similarity 1.0 = exact duplicate → score 0.05
            double novelty = 1.0 - similarityToClosestMatch;
            return Math.Max(0.05, novelty);
        }

        /// <summary>
        /// Aggregate Quality Score — รวมทุกมิติเป็นคะแนนเดียว
        /// 
        /// นี่คือ function หลักที่ NeuralExecutor เรียกใช้ตอนบันทึกตัวอย่าง
        /// </summary>
        public static QualityResult ComputeQualityScore(
            double confidenceGap,
            double stateImpact,
            double outcomeAlignment,
            double noveltyScore)
        {
            double score = 0.35 * confidenceGap
                         + 0.25 * stateImpact
                         + 0.25 * outcomeAlignment
                         + 0.15 * noveltyScore;

            score = Math.Max(0.0, Math.Min(1.0, score));

            // Classification
            string tier = score >= 0.7 ? "S"   // Excellent — learn from this!
                        : score >= 0.5 ? "A"    // Good
                        : score >= 0.3 ? "B"    // OK — weight lower
                        : "C";                   // Poor — minimal weight, but still record

            var reasons = new List<string>();
            if (confidenceGap > 0.6) reasons.Add($"Clear decision (gap={confidenceGap:F2})");
            if (stateImpact > 0.6) reasons.Add($"High impact move (impact={stateImpact:F2})");
            if (outcomeAlignment > 0.7) reasons.Add($"Winning game (outcome={outcomeAlignment:F2})");
            if (noveltyScore > 0.5) reasons.Add($"Novel situation (novelty={noveltyScore:F2})");

            return new QualityResult
            {
                Score = score,
                Tier = tier,
                ConfidenceGap = confidenceGap,
                StateImpact = stateImpact,
                OutcomeAlignment = outcomeAlignment,
                NoveltyScore = noveltyScore,
                Reasons = reasons
            };
        }
    }

    /// <summary>
    /// ผลการประเมินคุณภาพ — แนบไปกับ DatasetSample
    /// </summary>
    public class QualityResult
    {
        /// <summary>Overall quality score 0.0-1.0</summary>
        public double Score { get; set; }

        /// <summary>S/A/B/C tier classification</summary>
        public string Tier { get; set; } = "B";

        // Component scores for transparency
        public double ConfidenceGap { get; set; }
        public double StateImpact { get; set; }
        public double OutcomeAlignment { get; set; }
        public double NoveltyScore { get; set; }

        /// <summary>Human-readable reasons for the score</summary>
        public List<string> Reasons { get; set; } = new();

        /// <summary>
        /// แปลงเป็น training weight สำหรับ BatchTrainer
        /// S-tier = full weight, C-tier = minimal weight
        /// </summary>
        public double GetTrainingWeight()
        {
            return Tier switch
            {
                "S" => 1.0,
                "A" => 0.75,
                "B" => 0.4,
                "C" => 0.1,
                _ => 0.5
            };
        }
    }
}
