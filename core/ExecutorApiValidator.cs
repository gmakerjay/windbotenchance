using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace YgoAiPlatform.Core
{
    /// <summary>
    /// ตรวจสอบและวิเคราะห์ API calls ใน executor ของเด็คเพื่อหา function ที่ไม่มีจริง
    /// ช่วยจับปัญหาที่ AI เดาเขียน function calls ที่ไม่ถูกต้อง
    /// </summary>
    public class ExecutorApiValidator
    {
        private readonly HashSet<string> _validApis;
        private readonly Dictionary<string, List<string>> _apiCategories;
        private readonly List<ApiValidationError> _errors;

        public class ApiValidationError
        {
            public string FilePath { get; set; } = "";
            public int LineNumber { get; set; }
            public string ApiName { get; set; } = "";
            public string Context { get; set; } = "";
            public string Suggestion { get; set; } = "";
            public ApiErrorSeverity Severity { get; set; }
        }

        public enum ApiErrorSeverity
        {
            Critical,  // Function ไม่มีจริงแน่นอน - จะทำให้ crash
            High,      // Function อาจสะกดผิดหรือใช้ผิด
            Medium,    // Function ที่ deprecated หรือไม่แนะนำ
            Low        // Warning เตือนเล็กน้อย
        }

        public ExecutorApiValidator()
        {
            _validApis = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _apiCategories = new Dictionary<string, List<string>>();
            _errors = new List<ApiValidationError>();
            InitializeKnownApis();
        }

        private void InitializeKnownApis()
        {
            // ========================================
            // ClientField APIs (กลุ่ม Field/Board APIs)
            // ========================================
            var fieldApis = new List<string>
            {
                // Monster zones
                "GetMonsters", "GetMonsterCount", "HasMonster",
                "GetMonstersInMainMonsterZone", "GetMonstersInExtraMonsterZone",
                
                // Spell/Trap zones
                "GetSpells", "GetSpellCount", "HasSpell",
                "GetSpellsAndTraps",
                
                // Hand
                "GetHand", "GetHandCount", "HasInHand",
                
                // Graveyard
                "GetGraveyardMonsters", "GetGraveyardSpells", "GetGraveyardTraps",
                "GetGraveyard", "GetGraveyardCount", "HasInGraveyard",
                
                // Banished
                "GetBanishedMonsters", "GetBanishedSpells", "GetBanishedTraps",
                "GetBanished", "GetBanishedCount", "HasInBanished",
                
                // Deck
                "GetDeck", "GetDeckCount", "HasInDeck",
                
                // Extra deck
                "GetExtraDeck", "GetExtraDeckCount", "HasInExtra",
                
                // Utilities
                "GetFieldCard", "GetFieldSpellCard",
                "GetMonsterInZone", "GetSpellInZone"
            };

            // ========================================
            // ClientCard APIs (กลุ่ม Card Properties)
            // ========================================
            var cardApis = new List<string>
            {
                // Card identity
                "Id", "Alias", "Code", "Name",
                
                // Card types
                "IsCode", "HasType", "HasAttribute", "HasRace", "HasSetcode",
                "IsMonster", "IsSpell", "IsTrap",
                "IsMonsterInvincible", "IsMonsterHasPreventActivationEffectInBattle",
                
                // Card stats
                "Attack", "Defense", "Level", "Rank", "Link", "LScale", "RScale",
                "Attribute", "Race", "Type",
                
                // Card status
                "Location", "Controller", "Overlays", "Position",
                "IsFaceup", "IsFacedown", "IsAttack", "IsDefense",
                "IsDisabled", "IsShouldNotBeTarget", "IsShouldNotBeSpellTrapTarget",
                "IsShouldNotBeMonsterTarget",
                
                // Card state checks
                "IsOriginalCode", "HasPosition", "HasLocation",
                "Attacked", "IsAttacking", "IsDefending",
                "RealPower", "GetDefensePower", "GetLinkedZones"
            };

            // ========================================
            // AI.Utils APIs (กลุ่ม Utility Functions)
            // ========================================
            var utilsApis = new List<string>
            {
                // Card utilities
                "GetOneEnemyBetterThanMyBest", "GetProblematicCard",
                "GetBestEnemyMonster", "GetBestEnemyCard",
                "GetWorstEnemyMonster", "GetWorstBotMonster",
                "IsOneEnemyBetterThanValue", "GetBestAttack", "GetBestPower",
                "GetTotalAttackingMonsterAttack",
                
                // Deck/Location utilities  
                "GetCardCountWithId", "GetCardWithId",
                "GetLastChainCard", "ChainContainsCard", "ChainContainPlayer",
                "IsChainTarget", "IsChainTargetOnly",
                
                // Battle utilities
                "IsAllEnemyBetterThanValue", "CompareCardAttack",
                "CanDirectAttack", "GetBattlingMonster",
                
                // Zone utilities
                "SelectPreferredZone", "GetAvailableMonsterZones",
                "GetLinkedZoneCount", "GetAvailableZone"
            };

            // ========================================
            // Executor Main APIs (กลุ่ม Main Execution)
            // ========================================
            var executorApis = new List<string>
            {
                // Chain/Activation
                "OnChainActivation", "OnCardActivation", "OnChainEnd",
                
                // Battle phase
                "OnBattleStart", "OnDirectAttack", "OnBattle",
                
                // Main phase actions
                "OnNormalSummon", "OnSpecialSummon", "OnTribute",
                "OnSummon", "OnMonsterSet", "OnSpellSet",
                "OnRepos", "OnMonsterRepos",
                
                // Generic handlers
                "OnSelectCard", "OnSelectChain", "OnSelectPlace",
                "OnSelectPosition", "OnSelectOption", "OnSelectEffectYn",
                "OnSelectYesNo", "OnSelectBattleReplay",
                
                // Special executors
                "MonsterRepos", "DefaultMonsterRepos",
                "DefaultSpellSet", "DefaultSpellWillBeNegated",
                "DefaultTrap", "DefaultField",
                
                // Card-specific executors (format: CardId + "Activate", "Summon", etc)
                "CardIdActivate", "CardIdSummon", "CardIdEffect",
                "CardIdSet", "CardIdRepos"
            };

            // ========================================
            // Duel APIs (กลุ่ม Duel State)
            // ========================================
            var duelApis = new List<string>
            {
                // Game state
                "Turn", "Phase", "Player", "Enemy",
                "LifePoints", "Duel.LifePoints",
                "CurrentChain", "LastChainPlayer",
                
                // Phase checks
                "MainPhase", "BattlePhase", "EndPhase"
            };

            // Register all APIs
            _apiCategories["Field"] = fieldApis;
            _apiCategories["Card"] = cardApis;
            _apiCategories["Utils"] = utilsApis;
            _apiCategories["Executor"] = executorApis;
            _apiCategories["Duel"] = duelApis;

            // Flatten to valid APIs set
            foreach (var category in _apiCategories.Values)
            {
                foreach (var api in category)
                {
                    _validApis.Add(api);
                }
            }
        }

        /// <summary>
        /// วิเคราะห์ไฟล์ executor เพื่อหา API calls ที่ไม่ถูกต้อง
        /// </summary>
        public List<ApiValidationError> ValidateExecutorFile(string filePath)
        {
            _errors.Clear();

            if (!File.Exists(filePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Executor file not found: {filePath}");
                Console.ResetColor();
                return _errors;
            }

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    int lineNum = i + 1;

                    // Skip comments and empty lines
                    if (string.IsNullOrWhiteSpace(line) || 
                        line.StartsWith("//") || 
                        line.StartsWith("/*") ||
                        line.StartsWith("*"))
                        continue;

                    // Extract API calls from line
                    ExtractAndValidateApiCalls(filePath, lineNum, line);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Failed to validate executor file {filePath}: {ex.Message}");
                Console.ResetColor();
            }

            return _errors;
        }

        private void ExtractAndValidateApiCalls(string filePath, int lineNum, string line)
        {
            // Pattern 1: Method calls - Object.Method() or Method()
            var methodPattern = @"\b([A-Za-z_][A-Za-z0-9_]*)\s*\(";
            var methodMatches = Regex.Matches(line, methodPattern);

            foreach (Match match in methodMatches)
            {
                string methodName = match.Groups[1].Value;
                
                // Skip common C# keywords and built-in methods
                if (IsBuiltInMethod(methodName))
                    continue;

                // Check if this is a known API
                if (!_validApis.Contains(methodName))
                {
                    // Try to find similar API names (typo detection)
                    var suggestions = FindSimilarApiNames(methodName);
                    
                    var error = new ApiValidationError
                    {
                        FilePath = filePath,
                        LineNumber = lineNum,
                        ApiName = methodName,
                        Context = line.Length > 80 ? line.Substring(0, 77) + "..." : line,
                        Suggestion = suggestions.Any() 
                            ? $"คุณอาจหมายถึง: {string.Join(", ", suggestions.Take(3))}" 
                            : "ไม่พบ API นี้ในระบบ - ตรวจสอบ spelling หรือ documentation",
                        Severity = suggestions.Any() ? ApiErrorSeverity.High : ApiErrorSeverity.Critical
                    };
                    
                    _errors.Add(error);
                }
            }

            // Pattern 2: Property access - Object.Property (not followed by ()
            var propertyPattern = @"\.([A-Za-z_][A-Za-z0-9_]*)\b(?!\s*\()";
            var propertyMatches = Regex.Matches(line, propertyPattern);

            foreach (Match match in propertyMatches)
            {
                string propertyName = match.Groups[1].Value;
                
                // Skip common properties
                if (IsBuiltInProperty(propertyName))
                    continue;

                if (!_validApis.Contains(propertyName))
                {
                    var suggestions = FindSimilarApiNames(propertyName);
                    
                    var error = new ApiValidationError
                    {
                        FilePath = filePath,
                        LineNumber = lineNum,
                        ApiName = propertyName,
                        Context = line.Length > 80 ? line.Substring(0, 77) + "..." : line,
                        Suggestion = suggestions.Any() 
                            ? $"คุณอาจหมายถึง: {string.Join(", ", suggestions.Take(3))}" 
                            : "ไม่พบ Property นี้ในระบบ",
                        Severity = suggestions.Any() ? ApiErrorSeverity.Medium : ApiErrorSeverity.High
                    };
                    
                    _errors.Add(error);
                }
            }
        }

        private bool IsBuiltInMethod(string name)
        {
            var builtIn = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Add", "Remove", "Contains", "Clear", "Count", "Any", "All", "First", "Last",
                "Where", "Select", "OrderBy", "Sum", "Max", "Min", "Average",
                "ToString", "Equals", "GetHashCode", "GetType",
                "foreach", "if", "else", "while", "for", "return", "new", "var", "int", "bool", "string",
                "private", "public", "protected", "override", "virtual", "abstract", "static"
            };
            return builtIn.Contains(name);
        }

        private bool IsBuiltInProperty(string name)
        {
            var builtIn = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Length", "Count", "Value", "Key", "Item", "this"
            };
            return builtIn.Contains(name);
        }

        private List<string> FindSimilarApiNames(string target)
        {
            var similar = new List<(string api, int distance)>();

            foreach (var api in _validApis)
            {
                int distance = LevenshteinDistance(target.ToLower(), api.ToLower());
                
                // Consider similar if distance is small relative to length
                if (distance <= 3 || (target.Length > 5 && distance <= target.Length / 3))
                {
                    similar.Add((api, distance));
                }
            }

            return similar.OrderBy(x => x.distance).Select(x => x.api).ToList();
        }

        private int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }

        /// <summary>
        /// วิเคราะห์ทั้งโฟลเดอร์ executors
        /// </summary>
        public Dictionary<string, List<ApiValidationError>> ValidateExecutorDirectory(string dirPath)
        {
            var results = new Dictionary<string, List<ApiValidationError>>();

            if (!Directory.Exists(dirPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Executor directory not found: {dirPath}");
                Console.ResetColor();
                return results;
            }

            var csFiles = Directory.GetFiles(dirPath, "*.cs", SearchOption.AllDirectories);
            
            Console.WriteLine($"\n[Info] Validating {csFiles.Length} executor files in {dirPath}");

            foreach (var file in csFiles)
            {
                var errors = ValidateExecutorFile(file);
                if (errors.Any())
                {
                    results[file] = errors;
                }
            }

            return results;
        }

        /// <summary>
        /// พิมพ์รายงานผลการตรวจสอบ
        /// </summary>
        public void PrintValidationReport(Dictionary<string, List<ApiValidationError>> results)
        {
            if (!results.Any())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[✓] ไม่พบ API errors ในทุกไฟล์ executor!");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║     EXECUTOR API VALIDATION REPORT                             ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            int totalErrors = 0;
            var severityCounts = new Dictionary<ApiErrorSeverity, int>();

            foreach (var kvp in results)
            {
                string fileName = Path.GetFileName(kvp.Key);
                var errors = kvp.Value;
                totalErrors += errors.Count;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n📄 ไฟล์: {fileName}");
                Console.ResetColor();
                Console.WriteLine($"   พบ {errors.Count} API issues:");

                foreach (var error in errors.OrderBy(e => e.Severity).ThenBy(e => e.LineNumber))
                {
                    severityCounts.TryGetValue(error.Severity, out int count);
                    severityCounts[error.Severity] = count + 1;

                    var color = error.Severity switch
                    {
                        ApiErrorSeverity.Critical => ConsoleColor.Red,
                        ApiErrorSeverity.High => ConsoleColor.Yellow,
                        ApiErrorSeverity.Medium => ConsoleColor.DarkYellow,
                        _ => ConsoleColor.Gray
                    };

                    Console.ForegroundColor = color;
                    Console.Write($"   [{error.Severity}] ");
                    Console.ResetColor();
                    
                    Console.WriteLine($"Line {error.LineNumber}: '{error.ApiName}'");
                    Console.WriteLine($"           {error.Suggestion}");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"           Context: {error.Context}");
                    Console.ResetColor();
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + new string('─', 64));
            Console.WriteLine($"สรุป: พบ {totalErrors} API issues ใน {results.Count} ไฟล์");
            foreach (var kvp in severityCounts.OrderBy(k => k.Key))
            {
                Console.WriteLine($"  - {kvp.Key}: {kvp.Value} issues");
            }
            Console.WriteLine(new string('─', 64));
            Console.ResetColor();
        }

        /// <summary>
        /// บันทึกรายงานเป็นไฟล์
        /// </summary>
        public void SaveReport(Dictionary<string, List<ApiValidationError>> results, string outputPath)
        {
            try
            {
                using var writer = new StreamWriter(outputPath, false, System.Text.Encoding.UTF8);
                
                writer.WriteLine("EXECUTOR API VALIDATION REPORT");
                writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine(new string('=', 80));
                writer.WriteLine();

                if (!results.Any())
                {
                    writer.WriteLine("[✓] ไม่พบ API errors ในทุกไฟล์ executor!");
                    return;
                }

                int totalErrors = 0;
                var severityCounts = new Dictionary<ApiErrorSeverity, int>();

                foreach (var kvp in results)
                {
                    var errors = kvp.Value;
                    totalErrors += errors.Count;

                    writer.WriteLine($"File: {kvp.Key}");
                    writer.WriteLine($"Issues: {errors.Count}");
                    writer.WriteLine(new string('-', 80));

                    foreach (var error in errors.OrderBy(e => e.LineNumber))
                    {
                        severityCounts.TryGetValue(error.Severity, out int count);
                        severityCounts[error.Severity] = count + 1;

                        writer.WriteLine($"  [{error.Severity}] Line {error.LineNumber}: '{error.ApiName}'");
                        writer.WriteLine($"    {error.Suggestion}");
                        writer.WriteLine($"    Context: {error.Context}");
                        writer.WriteLine();
                    }
                    writer.WriteLine();
                }

                writer.WriteLine(new string('=', 80));
                writer.WriteLine($"SUMMARY: {totalErrors} API issues found in {results.Count} files");
                foreach (var kvp in severityCounts.OrderBy(k => k.Key))
                {
                    writer.WriteLine($"  {kvp.Key}: {kvp.Value} issues");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[✓] Validation report saved to: {outputPath}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Failed to save validation report: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
