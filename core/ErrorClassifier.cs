using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YgoAiPlatform.Core
{
    public enum ErrorCategory
    {
        None,
        OcgCoreLuaError,           // OCGCore Lua script errors (card script issues)
        ExecutorException,         // Executor logic errors (deck strategy bugs)
        NetworkTimeout,            // Connection/timeout issues
        ServerCrash,               // Server process crashed
        ClientCrash,               // WindBot client crashed
        DuelStuck,                 // Duel stuck/frozen (no progress)
        InvalidGameState,          // Invalid game state detected
        ResourceExhaustion,        // Memory/resource issues
        UnknownError               // Unclassified errors
    }

    public enum ErrorSeverity
    {
        Info,      // Informational (not really an error)
        Warning,   // Warning (might cause issues)
        Error,     // Error (affects duel outcome)
        Critical   // Critical (prevents duel from running)
    }

    public class ErrorInfo
    {
        public ErrorCategory Category { get; set; }
        public ErrorSeverity Severity { get; set; }
        public string Message { get; set; } = string.Empty;
        public string OriginalLine { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty; // Bot name or "Server"
        public int Turn { get; set; }
        public int DuelIndex { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string StackTrace { get; set; } = string.Empty;
        public Dictionary<string, string> Context { get; set; } = new();
    }

    public static class ErrorClassifier
    {
        private static readonly List<(Regex Pattern, ErrorCategory Category, ErrorSeverity Severity, string Description)> _rules = new()
        {
            // OCGCore Lua Errors - เกิดจาก script ของการ์ดมีปัญหา
            (new Regex(@"\[OCGCore Log\]", RegexOptions.IgnoreCase), 
                ErrorCategory.OcgCoreLuaError, ErrorSeverity.Warning, "OCGCore log message"),
            
            (new Regex(@"initial_effect", RegexOptions.IgnoreCase), 
                ErrorCategory.OcgCoreLuaError, ErrorSeverity.Error, "Card initial_effect error"),
            
            (new Regex(@"lua.*error|script.*error|attempt to.*nil", RegexOptions.IgnoreCase), 
                ErrorCategory.OcgCoreLuaError, ErrorSeverity.Error, "Lua script error"),

            // Executor Exceptions - เกิดจาก AI logic มีปัญหา
            (new Regex(@"Executor.*Exception|Executor.*Error|ExecutorBase", RegexOptions.IgnoreCase), 
                ErrorCategory.ExecutorException, ErrorSeverity.Error, "Executor logic error"),
            
            (new Regex(@"OnSelectCard.*null|OnSelectPosition.*null|InvalidOperationException", RegexOptions.IgnoreCase), 
                ErrorCategory.ExecutorException, ErrorSeverity.Error, "Executor callback error"),

            (new Regex(@"not implemented|NotImplementedException|TODO", RegexOptions.IgnoreCase), 
                ErrorCategory.ExecutorException, ErrorSeverity.Warning, "Unimplemented feature"),

            // Network/Timeout Issues
            (new Regex(@"timeout|timed out|connection.*closed|connection.*reset|connection.*refused", RegexOptions.IgnoreCase), 
                ErrorCategory.NetworkTimeout, ErrorSeverity.Warning, "Network/timeout issue"),

            // Server Crashes
            (new Regex(@"Server.*crash|Server.*exit|Server process.*terminated", RegexOptions.IgnoreCase), 
                ErrorCategory.ServerCrash, ErrorSeverity.Critical, "Server crashed"),

            // Client Crashes
            (new Regex(@"Client.*crash|WindBot.*crash|Process.*terminated unexpectedly", RegexOptions.IgnoreCase), 
                ErrorCategory.ClientCrash, ErrorSeverity.Critical, "Client crashed"),

            (new Regex(@"NullReferenceException|Object reference not set", RegexOptions.IgnoreCase), 
                ErrorCategory.ClientCrash, ErrorSeverity.Error, "Null reference exception"),

            // Duel Stuck
            (new Regex(@"stuck|frozen|no progress|infinite loop|deadlock", RegexOptions.IgnoreCase), 
                ErrorCategory.DuelStuck, ErrorSeverity.Error, "Duel stuck/frozen"),

            // Invalid Game State
            (new Regex(@"invalid.*state|illegal.*move|invalid.*action|assertion.*failed", RegexOptions.IgnoreCase), 
                ErrorCategory.InvalidGameState, ErrorSeverity.Error, "Invalid game state"),

            // Resource Exhaustion
            (new Regex(@"out of memory|OutOfMemoryException|stack overflow|StackOverflowException", RegexOptions.IgnoreCase), 
                ErrorCategory.ResourceExhaustion, ErrorSeverity.Critical, "Resource exhaustion"),

            // Generic Exception/Error
            (new Regex(@"exception|error|crash|failed|stacktrace", RegexOptions.IgnoreCase), 
                ErrorCategory.UnknownError, ErrorSeverity.Warning, "General error detected")
        };

        private static readonly HashSet<string> _ignoredPatterns = new(StringComparer.OrdinalIgnoreCase)
        {
            "sqlite",
            "no weights.json",
            "Exceptional Schedule",
            "Warning: Model file not found",
            "skipping",
            "deprecated"
        };

        public static ErrorInfo? ClassifyError(string line, string source = "", int turn = 0, int duelIndex = 0)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;

            // Check ignored patterns first
            foreach (var ignored in _ignoredPatterns)
            {
                if (line.Contains(ignored, StringComparison.OrdinalIgnoreCase))
                    return null;
            }

            // Try to match against known error patterns
            foreach (var (pattern, category, severity, description) in _rules)
            {
                if (pattern.IsMatch(line))
                {
                    var errorInfo = new ErrorInfo
                    {
                        Category = category,
                        Severity = severity,
                        Message = description,
                        OriginalLine = line,
                        Source = source,
                        Turn = turn,
                        DuelIndex = duelIndex,
                        Timestamp = DateTime.Now
                    };

                    // Extract additional context
                    ExtractContext(line, errorInfo);
                    
                    return errorInfo;
                }
            }

            return null;
        }

        private static void ExtractContext(string line, ErrorInfo errorInfo)
        {
            // Extract card name if present
            var cardMatch = Regex.Match(line, @"Card[:\s]+([^\s,\]]+)");
            if (cardMatch.Success)
            {
                errorInfo.Context["card"] = cardMatch.Groups[1].Value;
            }

            // Extract turn number if present
            var turnMatch = Regex.Match(line, @"Turn[:\s]+(\d+)", RegexOptions.IgnoreCase);
            if (turnMatch.Success)
            {
                errorInfo.Context["turn"] = turnMatch.Groups[1].Value;
                if (int.TryParse(turnMatch.Groups[1].Value, out int t))
                    errorInfo.Turn = t;
            }

            // Extract phase if present
            var phaseMatch = Regex.Match(line, @"(Draw|Standby|Main|Battle|End)\s+Phase", RegexOptions.IgnoreCase);
            if (phaseMatch.Success)
            {
                errorInfo.Context["phase"] = phaseMatch.Groups[1].Value;
            }

            // Extract exception type if present
            var exceptionMatch = Regex.Match(line, @"(\w+Exception)");
            if (exceptionMatch.Success)
            {
                errorInfo.Context["exception_type"] = exceptionMatch.Groups[1].Value;
            }

            // Extract stack trace indicator
            if (line.Contains("at ") && (line.Contains(".cs:") || line.Contains(".dll")))
            {
                errorInfo.StackTrace = line;
            }
        }

        public static string GetErrorCategoryDisplay(ErrorCategory category)
        {
            return category switch
            {
                ErrorCategory.OcgCoreLuaError => "🔧 OCG/Lua",
                ErrorCategory.ExecutorException => "🤖 Executor",
                ErrorCategory.NetworkTimeout => "⏱️ Network",
                ErrorCategory.ServerCrash => "💥 Server",
                ErrorCategory.ClientCrash => "💀 Client",
                ErrorCategory.DuelStuck => "🔒 Stuck",
                ErrorCategory.InvalidGameState => "⚠️ GameState",
                ErrorCategory.ResourceExhaustion => "📊 Resources",
                ErrorCategory.UnknownError => "❓ Unknown",
                _ => "ℹ️ Info"
            };
        }

        public static ConsoleColor GetSeverityColor(ErrorSeverity severity)
        {
            return severity switch
            {
                ErrorSeverity.Info => ConsoleColor.Cyan,
                ErrorSeverity.Warning => ConsoleColor.Yellow,
                ErrorSeverity.Error => ConsoleColor.Red,
                ErrorSeverity.Critical => ConsoleColor.DarkRed,
                _ => ConsoleColor.White
            };
        }

        public static string GetRecommendation(ErrorCategory category)
        {
            return category switch
            {
                ErrorCategory.OcgCoreLuaError => "ตรวจสอบ card script ในโฟลเดอร์ script/ - อาจมีการ์ดใหม่ที่ script ยังไม่ได้อัพเดท",
                ErrorCategory.ExecutorException => "⚠️ ปัญหาที่ Executor logic - ต้องแก้ไขใน Executors/*.cs",
                ErrorCategory.NetworkTimeout => "ตรวจสอบว่า server ทำงานปกติ หรือเพิ่ม timeout duration",
                ErrorCategory.ServerCrash => "Server process ล้ม - ตรวจสอบ ygopro.exe และ core.dll",
                ErrorCategory.ClientCrash => "WindBot client ล้ม - ตรวจสอบ WindBot.dll และ dependencies",
                ErrorCategory.DuelStuck => "Duel ค้าง - อาจเป็น infinite loop ใน executor decision",
                ErrorCategory.InvalidGameState => "Game state ผิดปกติ - ตรวจสอบ executor actions",
                ErrorCategory.ResourceExhaustion => "หน่วยความจำเต็ม - ลด parallel execution หรือเพิ่ม RAM",
                ErrorCategory.UnknownError => "ตรวจสอบ log file เพื่อวิเคราะห์เพิ่มเติม",
                _ => ""
            };
        }
    }
}
