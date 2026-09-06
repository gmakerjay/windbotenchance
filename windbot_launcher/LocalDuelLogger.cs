using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace WindBotLauncher
{
    public class LocalDuelLogger
    {
        private readonly object _lock = new object();
        private string _baseDir = string.Empty;
        private int _currentTurn = 0;
        private string _currentTurnDir = string.Empty;
        private StreamWriter? _currentTurnWriter;
        private StreamWriter? _errorWriter;
        private int _errorCount = 0;
        private readonly List<string> _turnSummary = new List<string>();
        private readonly string _matchupName;
        private readonly string _timestamp;
        
        private static readonly Regex TurnRegex = new Regex(@"\bTurn\s+(\d+)\b", RegexOptions.IgnoreCase);
        private static readonly Regex ErrorRegex = new Regex(@"\b(error|exception|crash|failed|stacktrace|nullreference)\b", RegexOptions.IgnoreCase);

        public string BaseDir => _baseDir;

        public LocalDuelLogger(string matchupName)
        {
            _matchupName = matchupName;
            _timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        }

        public void Init(string projectRoot)
        {
            lock (_lock)
            {
                _baseDir = Path.Combine(projectRoot, "logs", "local_duels", $"{_timestamp}_{_matchupName}");
                
                // turn_00_setup
                _currentTurn = 0;
                _currentTurnDir = Path.Combine(_baseDir, "turn_00_setup");
                Directory.CreateDirectory(_currentTurnDir);
                
                string turnLogPath = Path.Combine(_currentTurnDir, "log.txt");
                _currentTurnWriter = new StreamWriter(turnLogPath, false, Encoding.UTF8);
                
                // errors.txt
                Directory.CreateDirectory(_baseDir);
                string errorLogPath = Path.Combine(_baseDir, "errors.txt");
                _errorWriter = new StreamWriter(errorLogPath, false, Encoding.UTF8);
            }
        }

        public void LogLine(string origin, string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return;
            
            lock (_lock)
            {
                if (_currentTurnWriter == null) return;

                // Format with timestamp and source
                string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{origin}] {line}";
                
                // 1. Write to active turn log
                _currentTurnWriter.WriteLine(logLine);
                _currentTurnWriter.Flush();

                // 2. Check for errors
                if (ErrorRegex.IsMatch(line))
                {
                    if (!line.Contains("sqlite", StringComparison.OrdinalIgnoreCase) &&
                        !line.Contains("no weights.json", StringComparison.OrdinalIgnoreCase) &&
                        !line.Contains("Exceptional Schedule", StringComparison.OrdinalIgnoreCase))
                    {
                        _errorCount++;
                        _errorWriter?.WriteLine($"[Turn {_currentTurn}] {logLine}");
                        _errorWriter?.Flush();
                    }
                }

                // 3. Check for turn changes
                Match match = TurnRegex.Match(line);
                if (match.Success && int.TryParse(match.Groups[1].Value, out int turnNum))
                {
                    if (turnNum > _currentTurn)
                    {
                        // Record summary for previous turn
                        string prevDirName = Path.GetFileName(_currentTurnDir) ?? $"turn_{_currentTurn:02d}";
                        _turnSummary.Add($"Turn {_currentTurn}: {prevDirName}");

                        // Close current writer
                        _currentTurnWriter.Dispose();

                        // Open new turn
                        _currentTurn = turnNum;
                        _currentTurnDir = Path.Combine(_baseDir, $"turn_{_currentTurn:02d}");
                        Directory.CreateDirectory(_currentTurnDir);

                        string turnLogPath = Path.Combine(_currentTurnDir, "log.txt");
                        _currentTurnWriter = new StreamWriter(turnLogPath, false, Encoding.UTF8);

                        // Write turn header to new turn log
                        _currentTurnWriter.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [System] ==================================================");
                        _currentTurnWriter.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [System]   >>> TURN {_currentTurn} STARTED <<<");
                        _currentTurnWriter.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [System] ==================================================");
                        _currentTurnWriter.Flush();
                    }
                }
            }
        }

        public void Close(int exitCodeA, int exitCodeB)
        {
            lock (_lock)
            {
                // Record the final turn summary
                if (!string.IsNullOrEmpty(_currentTurnDir))
                {
                    string finalDirName = Path.GetFileName(_currentTurnDir) ?? $"turn_{_currentTurn:02d}";
                    _turnSummary.Add($"Turn {_currentTurn}: {finalDirName}");
                }

                // Dispose writers
                if (_currentTurnWriter != null)
                {
                    _currentTurnWriter.Dispose();
                    _currentTurnWriter = null;
                }
                if (_errorWriter != null)
                {
                    _errorWriter.Dispose();
                    _errorWriter = null;
                }

                // Write summary.txt
                if (!string.IsNullOrEmpty(_baseDir))
                {
                    string summaryPath = Path.Combine(_baseDir, "summary.txt");
                    try
                    {
                        using (var sw = new StreamWriter(summaryPath, false, Encoding.UTF8))
                        {
                            sw.WriteLine("Local Duel Summary");
                            sw.WriteLine("==================");
                            sw.WriteLine();
                            sw.WriteLine($"Matchup: {_matchupName}");
                            sw.WriteLine($"Timestamp: {_timestamp}");
                            sw.WriteLine($"Total Turns: {_currentTurn}");
                            sw.WriteLine($"Total Errors: {_errorCount}");
                            sw.WriteLine($"Bot A Exit Code: {exitCodeA}");
                            sw.WriteLine($"Bot B Exit Code: {exitCodeB}");
                            sw.WriteLine();
                            sw.WriteLine("Turn Breakdown:");
                            foreach (var entry in _turnSummary)
                            {
                                sw.WriteLine($"  - {entry}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Error] Failed to write summary.txt: {ex.Message}");
                    }
                }
            }
        }
    }
}
