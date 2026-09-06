using System;
using System.IO;
using System.Collections.Generic;
#if LIBWINDBOT
using Android.Util;
#endif

namespace WindBot
{
    public static class Logger
    {
        private static string _currentLogDir = null;

        /// <summary>
        /// Public accessor for the current log directory (used by GameStateSnapshot).
        /// </summary>
        public static string GetCurrentLogDir()
        {
            return _currentLogDir;
        }
        private static int _currentTurn = 0;
        private static List<string> _turnSummary = new List<string>();
        private static DateTime _startTime;
        private static string _name0;
        private static string _name1;
        private static string _botName;
        private static int _errorCount = 0;
        private static object _logLock = new object();
        private static StreamWriter _duelLogWriter = null;

        private static string CleanFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "Unknown";
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name.Replace(" ", "_");
        }

        private static string FindRecentSessionDir(string logsDir, string cleanName0, string cleanName1)
        {
            try
            {
                if (!Directory.Exists(logsDir)) return null;

                string pattern1 = $"{cleanName0}_vs_{cleanName1}_";
                string pattern2 = $"{cleanName1}_vs_{cleanName0}_";

                foreach (string dir in Directory.GetDirectories(logsDir))
                {
                    string dirName = Path.GetFileName(dir);
                    if (dirName.StartsWith(pattern1) || dirName.StartsWith(pattern2))
                    {
                        DateTime creationTime = Directory.GetCreationTime(dir);
                        if ((DateTime.Now - creationTime).TotalSeconds < 15)
                        {
                            return dir;
                        }
                    }
                }
            }
            catch {}
            return null;
        }

        public static void StartDuelSession(string name0, string name1, string botName)
        {
            lock (_logLock)
            {
                try
                {
                    _name0 = string.IsNullOrEmpty(name0) ? "Player" : name0;
                    _name1 = string.IsNullOrEmpty(name1) ? "Opponent" : name1;
                    _botName = string.IsNullOrEmpty(botName) ? "Bot" : botName;

                    string cleanName0 = CleanFileName(_name0);
                    string cleanName1 = CleanFileName(_name1);
                    string cleanBotName = CleanFileName(_botName);

                    _startTime = DateTime.Now;
                    string timestamp = _startTime.ToString("yyyyMMdd_HHmmss");

                    // Search up for logs folder or ygopro.exe relative to AppContext.BaseDirectory
                    string logsParentDir = AppContext.BaseDirectory;
                    for (int i = 0; i < 5; i++)
                    {
                        if (Directory.Exists(Path.Combine(logsParentDir, "logs")) ||
                            File.Exists(Path.Combine(logsParentDir, "ygopro.exe")) ||
                            File.Exists(Path.Combine(logsParentDir, "._cache_ygopro.exe")))
                        {
                            break;
                        }
                        string parent = Directory.GetParent(logsParentDir)?.FullName;
                        if (parent == null) break;
                        logsParentDir = parent;
                    }

                    string logsDir = Path.Combine(logsParentDir, "logs");
                    
                    // Find or create the shared duel folder
                    string sharedDuelFolder;
                    string existingDuelDir = FindRecentSessionDir(logsDir, cleanName0, cleanName1);
                    if (existingDuelDir != null)
                    {
                        sharedDuelFolder = existingDuelDir;
                    }
                    else
                    {
                        sharedDuelFolder = Path.Combine(logsDir, $"{cleanName0}_vs_{cleanName1}_{timestamp}");
                    }

                    // Bot specific directory inside the shared duel directory
                    _currentLogDir = Path.Combine(sharedDuelFolder, cleanBotName);

                    Directory.CreateDirectory(_currentLogDir);
                    try
                    {
                        File.WriteAllText(Path.Combine(_currentLogDir, "error.txt"), "", System.Text.Encoding.UTF8);
                        File.WriteAllText(Path.Combine(_currentLogDir, "errors.txt"), "", System.Text.Encoding.UTF8);
                        _duelLogWriter = new StreamWriter(Path.Combine(_currentLogDir, "duel.log"), false, System.Text.Encoding.UTF8);
                        _duelLogWriter.WriteLine($"=== Duel Log Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
                        _duelLogWriter.WriteLine($"Matchup: {_name0} vs {_name1} | Bot: {_botName}");
                        _duelLogWriter.WriteLine("==================================================");
                        _duelLogWriter.Flush();
                    }
                    catch {}

                    _currentTurn = 0;
                    _errorCount = 0;
                    _turnSummary.Clear();

                    SetTurn(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to start duel session logging: " + ex.Message);
                }
            }
        }

        public static void SetTurn(int turn)
        {
            lock (_logLock)
            {
                try
                {
                    _currentTurn = turn;
                    if (_currentLogDir != null)
                    {
                        string turnDir = Path.Combine(_currentLogDir, "turn");
                        Directory.CreateDirectory(turnDir);

                        string entry = $"Turn {_currentTurn}: turn/{_currentTurn}.txt";
                        if (!_turnSummary.Contains(entry) && _currentTurn > 0)
                        {
                            _turnSummary.Add(entry);
                        }
                    }
                }
                catch {}
            }
        }

        public static void EndDuelSession(string result)
        {
            lock (_logLock)
            {
                try
                {
                    if (_currentLogDir != null)
                    {
                        string summaryPath = Path.Combine(_currentLogDir, "summary.txt");
                        using (StreamWriter sw = new StreamWriter(summaryPath, false, System.Text.Encoding.UTF8))
                        {
                            sw.WriteLine("Simulation Summary");
                            sw.WriteLine("==================");
                            sw.WriteLine();
                            sw.WriteLine($"Matchup: {_name0} vs {_name1}");
                            sw.WriteLine($"Bot Name: {_botName}");
                            sw.WriteLine($"Start Time: {_startTime.ToString("yyyy-MM-dd HH:mm:ss")}");
                            sw.WriteLine($"End Time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                            sw.WriteLine($"Result: {result}");
                            sw.WriteLine($"Total Turns: {_currentTurn}");
                            sw.WriteLine($"Total Errors: {_errorCount}");
                            sw.WriteLine();
                            sw.WriteLine("Turn Breakdown:");
                            foreach (string t in _turnSummary)
                            {
                                sw.WriteLine($"  - {t}");
                            }
                        }
                    }
                }
                catch {}
                finally
                {
                    if (_duelLogWriter != null)
                    {
                        try { _duelLogWriter.WriteLine($"=== Duel Log Finished: {DateTime.Now:yyyy-MM-dd HH:mm:ss} (Result: {result}) ==="); } catch {}
                        try { _duelLogWriter.Dispose(); } catch {}
                        _duelLogWriter = null;
                    }
                    _currentLogDir = null;
                }
            }
        }

        private static void WriteToLogFile(string message)
        {
            lock (_logLock)
            {
                try
                {
                    if (_duelLogWriter != null)
                    {
                        _duelLogWriter.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
                        _duelLogWriter.Flush();
                    }
                    if (_currentLogDir != null)
                    {
                        string turnDir = Path.Combine(_currentLogDir, "turn");
                        Directory.CreateDirectory(turnDir);
                        string logPath = Path.Combine(turnDir, $"{_currentTurn}.txt");
                        using (StreamWriter sw = new StreamWriter(logPath, true, System.Text.Encoding.UTF8))
                        {
                            sw.WriteLine("[" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "] " + message);
                        }
                    }
                }
                catch {}
            }
        }

        private static void WriteErrorToLogFile(string message)
        {
            lock (_logLock)
            {
                try
                {
                    _errorCount++;
                    if (_currentLogDir != null)
                    {
                        string errorPath = Path.Combine(_currentLogDir, "error.txt");
                        using (StreamWriter sw = new StreamWriter(errorPath, true, System.Text.Encoding.UTF8))
                        {
                            sw.WriteLine($"[Turn {_currentTurn}] [{DateTime.Now.ToString("yy-MM-dd HH:mm:ss")}] {message}");
                        }
                        // Also write to errors.txt just in case they expect errors.txt
                        string errorsPath = Path.Combine(_currentLogDir, "errors.txt");
                        using (StreamWriter sw = new StreamWriter(errorsPath, true, System.Text.Encoding.UTF8))
                        {
                            sw.WriteLine($"[Turn {_currentTurn}] [{DateTime.Now.ToString("yy-MM-dd HH:mm:ss")}] {message}");
                        }
                    }
                    WriteToLogFile("[ERROR] " + message);
                }
                catch {}
            }
        }


        public static void WriteLine(string message)
        {
            if (message == null) return;
            if (message.StartsWith("Turn ") && int.TryParse(message.Substring(5), out int t))
            {
                SetTurn(t);
            }

            WriteToLogFile(message);

#if !LIBWINDBOT
            Console.WriteLine("[" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "] " + message);
#else
            Log.Info("Edoprowindbot", "[" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "] " + message);
#endif
        }

        public static void DebugWriteLine(string message)
        {
            if (message == null) return;
#if DEBUG
            WriteToLogFile("[DEBUG] " + message);
#if !LIBWINDBOT
            Console.WriteLine("[" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "] " + message);
#else
            Log.Debug("Edoprowindbot", "[" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "] " + message);
#endif
#endif
        }

        public static void WriteErrorLine(string message)
        {
            if (message == null) return;
            WriteErrorToLogFile(message);

#if !LIBWINDBOT
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Error.WriteLine("[" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "] " + message);
            Console.ResetColor();
#else
            Log.Error("Edoprowindbot", "[" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "] " + message);
#endif
        }

        /// <summary>
        /// Write a trace/audit line to the log file.
        /// Used by DecisionTracer, HeuristicGuard, and GameStateSnapshot.
        /// </summary>
        public static void WriteTraceLine(string message)
        {
            if (message == null) return;
            WriteToLogFile(message);

#if !LIBWINDBOT
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("[" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "] " + message);
            Console.ResetColor();
#else
            Log.Info("Edoprowindbot", "[" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "] " + message);
#endif
        }
    }
}