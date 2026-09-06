using System;
using System.Linq;
using System.Collections.Generic;

namespace YgoAiPlatform.Core
{
    public class ReasoningContext
    {
        public Move? SelectedMove { get; set; }
        public string WhySelected { get; set; } = string.Empty;
        
        public Dictionary<string, string> RejectedMoves { get; set; } = new();
        public List<Move> Candidates { get; set; } = new();
        
        public double BoardScore { get; set; }
        public double ThreatScore { get; set; }
        public double ComboScore { get; set; }
        public double ResourceScore { get; set; }
        public string ComboBranch { get; set; } = string.Empty;
    }

    public class WindBotExecutorWrapper
    {
        private bool _debugMode = false;

        // CB-FIX: Epsilon-greedy exploration สำหรับ RL training
        // epsilon=0.0 → pure greedy (inference), epsilon=0.1 → 10% random exploration (training)
        public double Epsilon { get; set; } = 0.0;

        private readonly Random _rng = new Random();

        public void SetDebugMode(bool enabled)
        {
            _debugMode = enabled;
        }

        public double EvaluateBoard(GameState state)
        {
            // ให้คะแนนเชิงบวกตามทรัพยากรบนฟิลด์และในมือเรา
            double score = (state.Field.Count * 2.0) + (state.Hand.Count * 1.5) + (state.LifePoints / 8000.0 * 5.0);
            
            if (_debugMode)
            {
                Console.WriteLine($"[Debug] EvaluateBoard: {score:F2} (Field: {state.Field.Count}, Hand: {state.Hand.Count})");
            }
            return score;
        }

        public double EvaluateThreat(GameState state)
        {
            // คะแนนภัยคุกคามอิงตามบอร์ดฝั่งตรงข้าม และพลังชีวิตเราที่เสียไป
            double lostLp = 8000 - state.LifePoints;
            double score = (state.OpponentField.Count * 2.5) + (lostLp / 1000.0 * 1.2);
            
            if (_debugMode)
            {
                Console.WriteLine($"[Debug] EvaluateThreat: {score:F2} (OpponentField: {state.OpponentField.Count}, Lost LP: {lostLp})");
            }
            return score;
        }

        public double EvaluateCombo(List<string> hand, List<string> field)
        {
            double score = 0.0;
            
            // เช็คการ์ดคอมโบหลักใน Yu-Gi-Oh เช่น Kashtira Fenrir หรือ Tearlaments
            bool hasFenrir = hand.Any(c => c.Contains("Fenrir")) || field.Any(c => c.Contains("Fenrir"));
            bool hasScheiren = hand.Any(c => c.Contains("Scheiren")) || field.Any(c => c.Contains("Scheiren"));
            
            if (hasFenrir) score += 3.0;
            if (hasScheiren) score += 2.5;
            
            // หากมีคอมโบพิเศษ Fenrir + Scheiren ให้โบนัส
            if (hasFenrir && hasScheiren)
            {
                score += 5.0; // คอมโบพิเศษ
            }

            if (_debugMode)
            {
                Console.WriteLine($"[Debug] EvaluateCombo: {score:F2} (Fenrir: {hasFenrir}, Scheiren: {hasScheiren})");
            }
            return score;
        }

        public double EvaluateLethal(GameState state)
        {
            // ตรวจหาความเป็นไปได้ที่จะจบเกม
            // จำลองว่าเราสามารถทำดาเมจได้เท่ากับแต้มโจมตีรวมของการ์ดเราบนฟิลด์
            int totalAtk = 0;
            foreach (var card in state.Field)
            {
                if (card.Contains("Fenrir")) totalAtk += 2400;
                else if (card.Contains("Scheiren")) totalAtk += 1600;
                else totalAtk += 1000;
            }

            double lethalScore = totalAtk >= state.OpponentLifePoints ? 1.0 : 0.0;
            
            if (_debugMode)
            {
                Console.WriteLine($"[Debug] EvaluateLethal: {lethalScore:F2} (Total ATK: {totalAtk} vs Opponent LP: {state.OpponentLifePoints})");
            }
            return lethalScore;
        }

        public List<Move> GetCandidateMoves(GameState state)
        {
            var candidates = new List<Move>();

            bool mmzOccupied = state.Field.Any(card => 
                !card.Contains("Kagari") && 
                !card.Contains("Shizuku") && 
                !card.Contains("Hayate") && 
                !card.Contains("Zeke") && 
                !card.Contains("Azalea") && 
                !card.Contains("Zero"));

            // 1. จำลองท่าจากในมือ (Hand)
            foreach (var card in state.Hand)
            {
                if (card == "Sky Striker Mobilize - Engage!")
                {
                    candidates.Add(new Move
                    {
                        CardName = card,
                        Action = "Activate",
                        BaseScore = mmzOccupied ? 0.0 : 10.0,
                        TargetLocation = "Graveyard",
                        Description = "Search Sky Striker card"
                    });
                }
                else if (card == "Sky Striker Airspace - Area Zero")
                {
                    candidates.Add(new Move
                    {
                        CardName = card,
                        Action = "Activate",
                        BaseScore = mmzOccupied ? 7.0 : 4.0,
                        TargetLocation = "Field",
                        Description = "Target a card to clear zone"
                    });
                }
                else if (card == "Reinforcement of the Army")
                {
                    candidates.Add(new Move
                    {
                        CardName = card,
                        Action = "Activate",
                        BaseScore = 6.0,
                        TargetLocation = "Graveyard",
                        Description = "Search Warrior"
                    });
                }
                else if (card == "Sky Striker Ace - Raye")
                {
                    candidates.Add(new Move
                    {
                        CardName = card,
                        Action = "Summon",
                        BaseScore = 5.0,
                        TargetLocation = "Field",
                        Description = "Summon Raye"
                    });
                }
                else if (card == "Radiant Typhoon Vision")
                {
                    bool hasQuickPlay = state.Hand.Any(c => c.Contains("Linkage") || c.Contains("Anchor") || c.Contains("Shark"));
                    if (hasQuickPlay)
                    {
                        candidates.Add(new Move
                        {
                            CardName = card,
                            Action = "Activate",
                            BaseScore = 8.0,
                            TargetLocation = "Graveyard",
                            Description = "Draw 2 and discard Quick-Play"
                        });
                    }
                }
                else if (card == "Sky Striker Mobilize - Linkage!")
                {
                    bool hasHayate = state.Field.Any(c => c.Contains("Hayate"));
                    if (hasHayate)
                    {
                        candidates.Add(new Move
                        {
                            CardName = card,
                            Action = "Activate",
                            BaseScore = 9.8,
                            TargetLocation = "Field",
                            Description = "Linkage chain attack"
                        });
                    }
                }
                else if (card.Contains("Fenrir"))
                {
                    candidates.Add(new Move
                    {
                        CardName = card,
                        Action = "Summon",
                        BaseScore = 8.5,
                        TargetLocation = "Field",
                        Description = "Summon Kashtira Fenrir to establish board presence and search"
                    });
                }
                else if (card.Contains("Scheiren"))
                {
                    candidates.Add(new Move
                    {
                        CardName = card,
                        Action = "Activate",
                        BaseScore = 7.5,
                        TargetLocation = "Field",
                        Description = "Activate Tearlaments Scheiren to special summon and mill"
                    });
                }
                else
                {
                    candidates.Add(new Move
                    {
                        CardName = card,
                        Action = "Set",
                        BaseScore = 4.0,
                        TargetLocation = "SpellTrapZone",
                        Description = $"Set {card}"
                    });
                }
            }

            // 2. จำลองท่าโจมตีหรือเอฟเฟกต์จากฟิลด์
            bool canAttack = state.Turn > 1 || state.OpponentField.Count > 0;
            foreach (var card in state.Field)
            {
                if (card == "Sky Striker Ace = Zero")
                {
                    bool opponentHasCard = state.OpponentField.Count > 0;
                    if (opponentHasCard)
                    {
                        candidates.Add(new Move
                        {
                            CardName = card,
                            Action = "Activate",
                            BaseScore = 9.5,
                            TargetLocation = "Field",
                            Description = "Tribute Zero to summon Raye/Roze and destroy card"
                        });
                    }
                }

                if (canAttack)
                {
                    candidates.Add(new Move
                    {
                        CardName = card,
                        Action = "Attack",
                        BaseScore = 9.0, // การโจมตีได้แต้มความสำคัญสูง
                        TargetLocation = "Opponent",
                        Description = $"Attack with {card} to reduce opponent LP"
                    });
                }
            }

            // เพิ่มท่า Pass เสมอเผื่อความปลอดภัย
            candidates.Add(new Move
            {
                CardName = "None",
                Action = "Pass",
                BaseScore = 1.0,
                TargetLocation = "None",
                Description = "End turn without actions"
            });

            // เรียงลำดับตามคะแนนความคุ้มค่าสูงสุดก่อน
            return candidates.OrderByDescending(m => m.BaseScore).ToList();
        }

        public (Move SelectedMove, ReasoningContext Context) SelectAction(GameState state)
        {
            var candidates = GetCandidateMoves(state);
            
            // ประเมินคะแนนสภาพบอร์ดปัจจุบัน
            double boardScore = EvaluateBoard(state);
            double threatScore = EvaluateThreat(state);
            double comboScore = EvaluateCombo(state.Hand, state.Field);
            double lethalScore = EvaluateLethal(state);
            
            double resourceScore = state.Hand.Count + state.Field.Count - state.OpponentField.Count;

            // ตรวจสอบความถูกต้องว่ามี Candidates หรือไม่
            if (candidates.Count == 0)
            {
                var passMove = new Move { CardName = "None", Action = "Pass", BaseScore = 0.0 };
                return (passMove, new ReasoningContext { SelectedMove = passMove });
            }

            // CB-FIX: Epsilon-greedy exploration
            // Epsilon=0 → greedy (default/inference), Epsilon>0 → mix random for RL training
            Move selected;
            bool isExploring = Epsilon > 0.0 && _rng.NextDouble() < Epsilon;
            if (isExploring)
            {
                selected = candidates[_rng.Next(candidates.Count)];
                if (_debugMode)
                    Console.WriteLine($"[Debug] Epsilon-greedy: EXPLORING — random pick '{selected.CardName}' (ε={Epsilon:F2})");
            }
            else
            {
                selected = candidates[0];
                if (_debugMode)
                    Console.WriteLine($"[Debug] Epsilon-greedy: EXPLOITING — best pick '{selected.CardName}' (score={selected.BaseScore:F2})");
            }

            // สร้างคอมโบสายนั่งอ้างอิง (Combo Branch)
            string comboBranch = "default_line";
            if (candidates.Any(m => m.CardName.Contains("Fenrir") && m.Action == "Summon"))
            {
                comboBranch = "kashtira_line_a";
            }
            else if (candidates.Any(m => m.CardName.Contains("Scheiren") && m.Action == "Activate"))
            {
                comboBranch = "tearlaments_line_b";
            }

            var context = new ReasoningContext
            {
                SelectedMove = selected,
                WhySelected = isExploring
                    ? $"[EXPLORE ε={Epsilon:F2}] Random pick '{selected.Action} {selected.CardName}' (score={selected.BaseScore:F2})"
                    : $"[EXPLOIT] Selected '{selected.Action} {selected.CardName}' — highest score ({selected.BaseScore:F2})",
                Candidates = candidates,
                BoardScore = boardScore,
                ThreatScore = threatScore,
                ComboScore = comboScore,
                ResourceScore = resourceScore,
                ComboBranch = comboBranch
            };

            // จดบันทึก Rejected Moves ทั้งหมดพร้อมเหตุผล
            for (int i = 1; i < candidates.Count; i++)
            {
                var rejected = candidates[i];
                context.RejectedMoves[rejected.CardName + "_" + rejected.Action] = 
                    $"Rejected because score ({rejected.BaseScore:F2}) is lower than selected '{selected.CardName}' ({selected.BaseScore:F2})";
            }

            if (_debugMode)
            {
                Console.WriteLine($"\n[Debug] Selected Action: {selected.Action} {selected.CardName} | Reason: {context.WhySelected}");
                Console.WriteLine($"[Debug] Combo Branch: {context.ComboBranch} | Board Score: {boardScore:F2} | Threat: {threatScore:F2}");
                Console.WriteLine($"[Debug] Rejected Candidates: {context.RejectedMoves.Count}");
            }

            return (selected, context);
        }
    }
}
