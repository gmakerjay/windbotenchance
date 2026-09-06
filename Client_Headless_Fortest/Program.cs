using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using YgoAiPlatform.Core;

namespace ClientHeadlessFortest
{
    class Program
    {
        class DuelRecord
        {
            public int GameIndex { get; set; }
            public string Winner { get; set; } = "Draw";
            public int TurnCount { get; set; }
            public bool Success { get; set; }
            public string ErrorMessage { get; set; } = "";
            public int OcgCoreErrors { get; set; }
            // NEW: Enhanced tracking fields
            public long DurationMs { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string LogFilePath { get; set; } = "";
            public bool TimedOut { get; set; }
        }

        // Known false-positive patterns that should NOT be treated as crashes
        private static readonly string[] ErrorIgnorePatterns = new[]
        {
            "sqlite", "no weights.json", "Exceptional Schedule",
            "OCGCore", "initial_effect", "ocgCoreError", "error_counter",
            "OcgCoreErrors", "ErrorMessage", "ocg_errors"
        };

        // Real crash patterns using word boundaries
        private static readonly Regex CrashRegex = new Regex(
            @"\b(exception|crash|stacktrace|nullreference|NullReferenceException|ArgumentException|InvalidOperationException|IndexOutOfRangeException|StackOverflowException|OutOfMemoryException|System\.Exception)\b",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static int ParseTurnCount(string line)
        {
            if (line == null) return -1;
            int turnIdx = line.IndexOf("Turn", StringComparison.OrdinalIgnoreCase);
            if (turnIdx >= 0)
            {
                string afterTurn = line.Substring(turnIdx + 4);
                if (afterTurn.Length > 0 && char.IsWhiteSpace(afterTurn[0]))
                {
                    afterTurn = afterTurn.TrimStart();
                    string digits = "";
                    foreach (char c in afterTurn)
                    {
                        if (char.IsDigit(c)) digits += c;
                        else break;
                    }
                    if (digits.Length > 0 && int.TryParse(digits, out int t))
                    {
                        return t;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Check if a line is a real crash/error (not a false positive)
        /// </summary>
        private static bool IsRealCrashLine(string origin, string line)
        {
            if (origin == "Server") return false;

            // Check against ignore patterns first
            foreach (var pattern in ErrorIgnorePatterns)
            {
                if (line.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            // Use regex for real crash detection
            return CrashRegex.IsMatch(line);
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine(" YGO AI PLATFORM - E2E VERIFICATION SUITE (STABLE v2)");
            Console.WriteLine("==================================================================");

            string host = "127.0.0.1";
            int basePort = 20000; // Use high port range to avoid conflicts with services
            string testDeck = "Neural";
            string opponentDeck = "Altergeist";
            int totalGames = 10;
            bool forestMode = false;
            bool parallelMode = false;
            int maxWorkers = 5;
            int duelTimeoutSeconds = 120; // Configurable per-duel timeout

            // Parse args
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("--host", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    host = args[i + 1];
                }
                else if (args[i].Equals("--port", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    int.TryParse(args[i + 1], out basePort);
                }
                else if (args[i].Equals("--deck", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    testDeck = args[i + 1];
                }
                else if (args[i].Equals("--opponent", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    opponentDeck = args[i + 1];
                }
                else if (args[i].Equals("--games", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    int.TryParse(args[i + 1], out totalGames);
                }
                else if (args[i].Equals("--forest", StringComparison.OrdinalIgnoreCase))
                {
                    forestMode = true;
                }
                else if (args[i].Equals("--parallel", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    parallelMode = true;
                    int.TryParse(args[i + 1], out maxWorkers);
                    if (maxWorkers < 1) maxWorkers = 1;
                }
                else if (args[i].Equals("--timeout", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    int.TryParse(args[i + 1], out duelTimeoutSeconds);
                    if (duelTimeoutSeconds < 30) duelTimeoutSeconds = 30;
                }
            }

            if (forestMode)
            {
                testDeck = "2026_Dreadnought";
                Console.WriteLine("Forest Mode enabled: Running round-robin tournament for Dreadnought vs other 2026 decks.");
            }
            else
            {
                Console.WriteLine($"Testing Deck : {testDeck} (Bot A)");
                Console.WriteLine($"Opponent Deck: {opponentDeck} (Bot B)");
            }
            Console.WriteLine($"Total Games  : {totalGames}");
            Console.WriteLine($"Base Port    : {basePort}");
            Console.WriteLine($"Parallel     : {(parallelMode ? $"Yes (workers={maxWorkers})" : "No")}");
            Console.WriteLine($"Duel Timeout : {duelTimeoutSeconds}s");
            Console.WriteLine("Press Ctrl+C to cancel the verification.\n");

            // ═══ Pre-cleanup: Kill any zombie ygopro processes ONLY when --clean is specified ═══
            bool cleanZombieMode = args.Any(a => a.Equals("--clean", StringComparison.OrdinalIgnoreCase));
            if (cleanZombieMode)
            {
                Console.WriteLine("[Info] Pre-cleanup: Terminating any leftover ygopro processes (--clean requested)...");
                try
                {
                    foreach (var proc in System.Diagnostics.Process.GetProcessesByName("ygopro"))
                    {
                        try { proc.Kill(); proc.WaitForExit(2000); } catch { }
                    }
                    foreach (var proc in System.Diagnostics.Process.GetProcessesByName("ygoprodll"))
                    {
                        try { proc.Kill(); proc.WaitForExit(2000); } catch { }
                    }
                    foreach (var proc in System.Diagnostics.Process.GetProcessesByName("._cache_ygopro"))
                    {
                        try { proc.Kill(); proc.WaitForExit(2000); } catch { }
                    }
                    Console.WriteLine("[Info] Pre-cleanup complete.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Warning] Pre-cleanup encountered an issue: {ex.Message}");
                }
                Console.WriteLine();
            }

            // Resolve paths - FORCE use Game_EDOPro as the single source of truth
            string projectRootDir = WindBotResolver.ResolveProjectRoot();
            string workspaceRootDir = Path.GetDirectoryName(projectRootDir) ?? projectRootDir;
            
            // Force use Game_EDOPro directory for all binaries, with fallback to EdoGame
            string gameEdoProDir = Path.Combine(workspaceRootDir, "Game_EDOPro");
            if (!Directory.Exists(gameEdoProDir))
            {
                string fallbackGameDir = Path.Combine(workspaceRootDir, "EdoGame");
                string parentGameDir = Path.GetDirectoryName(workspaceRootDir) ?? "";
                if (Directory.Exists(fallbackGameDir))
                {
                    gameEdoProDir = fallbackGameDir;
                }
                else if (File.Exists(Path.Combine(parentGameDir, "cards.cdb")))
                {
                    gameEdoProDir = parentGameDir;
                }
                else if (Directory.Exists(Path.Combine(parentGameDir, "EdoGame")))
                {
                    gameEdoProDir = Path.Combine(parentGameDir, "EdoGame");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Error] game directory (Game_EDOPro/EdoGame) not found at: {workspaceRootDir}");
                    Console.ResetColor();
                    return;
                }
            }
            
            // Use ygopro server binary (with SetWindowPos offscreen mover) to run reliably
            // IMPORTANT: ._cache_ygopro.exe is the YGOPro build that supports `-s PORT` server mode.
            // ygoprodll.exe / ygopro.exe are EDOPro GUI builds that do NOT support headless server mode.
            string ygoproPath = Path.Combine(gameEdoProDir, "._cache_ygopro.exe");
            if (!File.Exists(ygoproPath))
            {
                ygoproPath = Path.Combine(gameEdoProDir, "ygopro.exe");
            }
            if (!File.Exists(ygoproPath))
            {
                ygoproPath = Path.Combine(gameEdoProDir, "._cache_ygoprodll.exe");
            }
            if (!File.Exists(ygoproPath))
            {
                ygoproPath = Path.Combine(gameEdoProDir, "ygoprodll.exe");
            }
            
            string windbotDllPath = Path.Combine(gameEdoProDir, "WindBot", "WindBot.dll");
            
            if (!File.Exists(ygoproPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] ygopro executable not found in: {gameEdoProDir}");
                Console.ResetColor();
                return;
            }
            
            if (!File.Exists(windbotDllPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] WindBot.dll not found at: {windbotDllPath}");
                Console.ResetColor();
                return;
            }
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Info] Using Game_EDOPro binaries:");
            Console.WriteLine($"       ygopro.exe  : {ygoproPath}");
            Console.WriteLine($"       WindBot.dll : {windbotDllPath}");
            Console.WriteLine($"       core.dll    : {Path.Combine(gameEdoProDir, "core.dll")}");
            Console.WriteLine($"       ocgcore.dll : {Path.Combine(gameEdoProDir, "ocgcore.dll")}");
            Console.ResetColor();

            // Verify cards.cdb exists
            string cardsCdb = Path.Combine(gameEdoProDir, "cards.cdb");
            if (!File.Exists(cardsCdb))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[Warning] cards.cdb not found at: {cardsCdb}");
                Console.ResetColor();
            }

            // Create session log directory
            string sessionTimestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string sessionLogDir = Path.Combine(projectRootDir, "logs", "sessions", sessionTimestamp);
            Directory.CreateDirectory(sessionLogDir);

            // JSONL structured log file for this session
            string jsonlPath = Path.Combine(sessionLogDir, "duel_results.jsonl");
            var jsonlLock = new object();

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("\nStopping test execution...");
                cts.Cancel();
                e.Cancel = true;
            };

            var matchups = new List<(string TestDeck, string OpponentDeck)>();
            if (forestMode)
            {
                var other2026Decks = new List<string> { "2026_BrElfnote", "2026_DarkTime", "2026_K9" };
                foreach (var opp in other2026Decks)
                {
                    matchups.Add(("2026_Dreadnought", opp));
                }
            }
            else
            {
                matchups.Add((testDeck, opponentDeck));
            }

            var allMatchupRecords = new Dictionary<string, List<DuelRecord>>();

            foreach (var matchup in matchups)
            {
                string currentTestDeck = matchup.TestDeck;
                string currentOpponentDeck = matchup.OpponentDeck;
                string matchupKey = $"{currentTestDeck}_vs_{currentOpponentDeck}";

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n==================================================================");
                Console.WriteLine($" STARTING MATCHUP: {currentTestDeck} VS {currentOpponentDeck}");
                Console.WriteLine("==================================================================");
                Console.ResetColor();

                var records = new ConcurrentBag<DuelRecord>();

                async Task<DuelRecord> RunSingleDuel(int gameIndex, int port)
                {
                    var duelStopwatch = Stopwatch.StartNew();
                    var duelStartTime = DateTime.Now;

                    // Per-duel log file (no interleaving)
                    string duelLogPath = Path.Combine(sessionLogDir, $"duel_{gameIndex:D3}_{currentTestDeck}_vs_{currentOpponentDeck}.log");
                    var duelLogLines = new ConcurrentQueue<string>();

                    void LogDuel(string line)
                    {
                        string timestamped = $"[{DateTime.Now:HH:mm:ss.fff}] {line}";
                        duelLogLines.Enqueue(timestamped);
                    }

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n>>> STARTING DUEL {gameIndex} OF {totalGames} ON PORT {port}");
                    Console.ResetColor();
                    LogDuel($"=== DUEL {gameIndex} START | {currentTestDeck} vs {currentOpponentDeck} | Port {port} ===");

                    // Per-duel CancellationTokenSource with timeout
                    using var duelCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
                    duelCts.CancelAfter(TimeSpan.FromSeconds(duelTimeoutSeconds));

                    var botA = new HeadlessClientWrapper(windbotDllPath)
                    {
                        Name = $"Test_{currentTestDeck}_{gameIndex}",
                        Deck = currentTestDeck,
                        Host = host,
                        Port = port,
                        DebugMode = true
                    };

                    var botB = new HeadlessClientWrapper(windbotDllPath)
                    {
                        Name = $"Test_Opponent_{currentOpponentDeck}_{gameIndex}",
                        Deck = currentOpponentDeck,
                        Host = host,
                        Port = port,
                        DebugMode = true
                    };

                    bool duelCompleted = false;
                    string winnerName = "Draw";
                    int turnCount = 0;
                    bool hasCrash = false;
                    bool timedOut = false;
                    int ocgCoreErrorCount = 0;
                    var crashDetails = new List<string>();
                    object duelLock = new object();

                    // ═══ Noise filter patterns — always skip console for these ═══
                    // These are written to per-duel log file only.
                    bool IsNoiseLine(string line)
                    {
                        if (line.Contains("Loading extra database")) return true;
                        if (line.Contains("UnKnowCard")) return true;
                        if (line.Contains("Decks initialized")) return true;
                        if (line.Contains("Deck found, loading")) return true;
                        if (line.Contains("WindBot starting")) return true;
                        if (line.Contains("*********Bot Hand*********")) return true;
                        if (line.Contains("*********Bot Spell*********")) return true;
                        if (line.Contains("*********Bot Monster*********")) return true;
                        if (line.Contains("*********Finish*********")) return true;
                        return false;
                    }

                    // ═══ Decision-critical patterns — shown on console in parallel mode ═══
                    bool IsDecisionLine(string line)
                    {
                        if (line.Contains("[TRACE]")) return true;
                        if (line.Contains("[PHASE-GUARD]")) return true;
                        if (line.Contains("[LETHAL-CHECK]")) return true;
                        if (line.Contains("[TURN-SUMMARY]")) return true;
                        if (line.Contains("[BOARD SCORE]")) return true;
                        if (line.Contains("[SNAPSHOT]")) return true;
                        if (line.Contains("[GUARD-SUMMARY]")) return true;
                        if (line.Contains("Duel finished")) return true;
                        if (line.StartsWith("Turn ") || line.Contains("Turn ")) 
                        {
                            // Only show "Turn N" standalone lines, not embedded in other text
                            if (line.TrimStart().StartsWith("Turn ") && line.TrimEnd().Length < 10) return true;
                        }
                        return false;
                    }

                    void HandleOutput(string origin, string line)
                    {
                        // ALWAYS write to per-duel log file (complete record)
                        LogDuel($"[{origin}] {line}");

                        lock (duelLock)
                        {
                            int parsedTurn = ParseTurnCount(line);
                            if (parsedTurn > turnCount) turnCount = parsedTurn;

                            if (line.Contains("(Go to Draw)", StringComparison.OrdinalIgnoreCase) && turnCount == 0)
                                turnCount++;

                            // FIX: Only process the FIRST "Duel finished" signal to prevent winner overwrite race
                            if (!duelCompleted &&
                                ((line.Contains("Duel finished", StringComparison.OrdinalIgnoreCase) && !line.Contains("MsgRetry", StringComparison.OrdinalIgnoreCase)) ||
                                line.Contains("result: Win", StringComparison.OrdinalIgnoreCase) ||
                                line.Contains("result: Lose", StringComparison.OrdinalIgnoreCase) ||
                                line.Contains("result: Draw", StringComparison.OrdinalIgnoreCase)))
                            {
                                if (line.Contains("result: Win", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (origin == botA.Name) winnerName = currentTestDeck;
                                    else if (origin == botB.Name) winnerName = currentOpponentDeck;
                                    else winnerName = origin;
                                }
                                else if (line.Contains("result: Lose", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (origin == botA.Name) winnerName = currentOpponentDeck;
                                    else if (origin == botB.Name) winnerName = currentTestDeck;
                                    else winnerName = "Unknown";
                                }
                                else if (line.Contains("result: Draw", StringComparison.OrdinalIgnoreCase))
                                    winnerName = "Draw";

                                duelCompleted = true;
                                LogDuel($"[SYSTEM] Duel completed. Winner: {winnerName} (reported by {origin})");
                            }

                            if (line.Contains("[OCGCore Log]", StringComparison.OrdinalIgnoreCase) ||
                                line.Contains("initial_effect", StringComparison.OrdinalIgnoreCase))
                                ocgCoreErrorCount++;

                            // FIX: Use smarter crash detection to avoid false positives
                            if (IsRealCrashLine(origin, line))
                            {
                                hasCrash = true;
                                crashDetails.Add($"[{origin}] {line}");
                                LogDuel($"[CRASH-DETECTED] {line}");
                            }
                        }

                        // ═══ Console Output Filtering ═══

                        // Always filter noise (database loading, bot hand dumps, etc.)
                        if (IsNoiseLine(line)) return;

                        // In parallel mode: only show decision-critical lines
                        if (parallelMode && totalGames > 1)
                        {
                            if (!IsDecisionLine(line)) return;
                        }

                        // Print to console
                        Console.WriteLine($"[{origin}] {line}");
                    }

                    botA.OnOutputReceived += (line) => HandleOutput(botA.Name, line);
                    botA.OnErrorReceived += (line) => {
                        lock (duelLock) { hasCrash = true; crashDetails.Add($"[{botA.Name} StdErr] {line}"); }
                        LogDuel($"[{botA.Name} StdErr] {line}");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[{botA.Name} StdErr] {line}");
                        Console.ResetColor();
                    };
                    botB.OnOutputReceived += (line) => HandleOutput(botB.Name, line);
                    botB.OnErrorReceived += (line) => {
                        lock (duelLock) { hasCrash = true; crashDetails.Add($"[{botB.Name} StdErr] {line}"); }
                        LogDuel($"[{botB.Name} StdErr] {line}");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[{botB.Name} StdErr] {line}");
                        Console.ResetColor();
                    };

                    System.Diagnostics.Process? serverProcess = null;
                    try
                    {
                        var serverStartInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = ygoproPath,
                            Arguments = $"-s {port}",
                            WorkingDirectory = gameEdoProDir,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            RedirectStandardOutput = false,
                            RedirectStandardError = false
                        };

                        serverProcess = new System.Diagnostics.Process { StartInfo = serverStartInfo };

                        serverProcess.Start();
                        LogDuel($"[SYSTEM] Server process started (PID: {serverProcess.Id})");
                        Console.WriteLine($"[Info] Server process started (PID: {serverProcess.Id}) for Duel {gameIndex}");

                        // Instantly hide and move window off-screen on Windows to achieve true headless execution
                        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                        {
                            _ = Task.Run(async () =>
                            {
                                try
                                {
                                    for (int i = 0; i < 40; i++)
                                    {
                                        if (serverProcess.HasExited) break;
                                        serverProcess.Refresh();
                                        IntPtr hwnd = serverProcess.MainWindowHandle;
                                        if (hwnd != IntPtr.Zero)
                                        {
                                            SetWindowPos(hwnd, IntPtr.Zero, -32000, -32000, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
                                            break;
                                        }
                                        await Task.Delay(50);
                                    }
                                }
                                catch { }
                            });
                        }

                        // Wait for server to actually start listening on port
                        for (int retry = 0; retry < 60; retry++)
                        {
                            if (serverProcess.HasExited || duelCts.Token.IsCancellationRequested) break;
                            try
                            {
                                using var testSocket = new System.Net.Sockets.TcpClient();
                                var connectTask = testSocket.ConnectAsync(host, port);
                                if (await Task.WhenAny(connectTask, Task.Delay(100, duelCts.Token)) == connectTask && testSocket.Connected)
                                {
                                    break;
                                }
                            }
                            catch { }
                            await Task.Delay(150, duelCts.Token);
                        }
                        await Task.Delay(200, duelCts.Token);

                        botA.Start();
                        await Task.Delay(300, duelCts.Token);
                        botB.Start();

                        int postExitGraceTicks = 0;

                        while (!duelCts.Token.IsCancellationRequested)
                        {
                            bool localCompleted;
                            lock (duelLock) { localCompleted = duelCompleted; }
                            if (localCompleted || (!botA.IsRunning && !botB.IsRunning))
                                break;

                            try { serverProcess.Refresh(); } catch { }
                            if (serverProcess.HasExited)
                            {
                                LogDuel($"[SYSTEM] Server process exited with code {serverProcess.ExitCode}");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"[Warning] Server process exited with code {serverProcess.ExitCode} (Duel {gameIndex})");
                                Console.ResetColor();
                                break;
                            }
                            if (!botA.IsRunning || !botB.IsRunning)
                            {
                                postExitGraceTicks++;
                                if (postExitGraceTicks > 10) break;
                            }
                            await Task.Delay(200, duelCts.Token);
                        }

                        // Check if we exited due to per-duel timeout
                        if (duelCts.Token.IsCancellationRequested && !cts.Token.IsCancellationRequested)
                        {
                            timedOut = true;
                            LogDuel($"[SYSTEM] Duel {gameIndex} TIMED OUT after {duelTimeoutSeconds} seconds.");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"[Warning] Duel {gameIndex} timed out after {duelTimeoutSeconds} seconds.");
                            Console.ResetColor();
                        }
                    }
                    catch (OperationCanceledException) when (!cts.Token.IsCancellationRequested)
                    {
                        // Per-duel timeout - not a global cancel
                        timedOut = true;
                        LogDuel($"[SYSTEM] Duel {gameIndex} TIMED OUT (OperationCanceledException).");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[Warning] Duel {gameIndex} timed out.");
                        Console.ResetColor();
                    }
                    catch (OperationCanceledException)
                    {
                        // Global cancel - user pressed Ctrl+C
                        LogDuel($"[SYSTEM] Duel {gameIndex} cancelled by user.");
                    }
                    catch (Exception ex)
                    {
                        LogDuel($"[ERROR] Exception during Duel {gameIndex}: {ex.Message}");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[Error] Exception during Duel {gameIndex}: {ex.Message}");
                        Console.ResetColor();
                    }
                    finally
                    {
                        // FIX: Stop bots first (they're the most important to clean up)
                        try { botA.Stop(); } catch { }
                        try { botB.Stop(); } catch { }

                        // FIX: Kill server with entireProcessTree and WaitForExit timeout
                        if (serverProcess != null)
                        {
                            try
                            {
                                if (!serverProcess.HasExited)
                                {
                                    LogDuel($"[SYSTEM] Killing server process PID {serverProcess.Id}...");
                                    serverProcess.Kill();
                                    // Wait max 5 seconds for server to actually exit
                                    if (!serverProcess.WaitForExit(5000))
                                    {
                                        LogDuel($"[WARNING] Server process PID {serverProcess.Id} did not exit after Kill+5s. Force disposing.");
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine($"[Warning] Server process PID {serverProcess.Id} did not exit after Kill. Force disposing.");
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        LogDuel($"[SYSTEM] Server process exited successfully.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogDuel($"[WARNING] Failed to kill server process: {ex.Message}");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"[Warning] Failed to kill server process for Duel {gameIndex}: {ex.Message}");
                                Console.ResetColor();
                            }
                            finally
                            {
                                try { serverProcess.Dispose(); } catch { }
                            }
                            // Allow OS to release the port before next duel iteration
                            // Increased from 500ms to 2000ms to ensure TCP TIME_WAIT fully clears
                            await Task.Delay(2000);
                        }
                    }

                    duelStopwatch.Stop();
                    var duelEndTime = DateTime.Now;

                    DuelRecord record;
                    lock (duelLock)
                    {
                        record = new DuelRecord
                        {
                            GameIndex = gameIndex,
                            Winner = winnerName,
                            TurnCount = turnCount,
                            Success = !hasCrash && duelCompleted && !timedOut,
                            OcgCoreErrors = ocgCoreErrorCount,
                            ErrorMessage = hasCrash ? string.Join(" | ", crashDetails) : (timedOut ? "TIMEOUT" : ""),
                            DurationMs = duelStopwatch.ElapsedMilliseconds,
                            StartTime = duelStartTime,
                            EndTime = duelEndTime,
                            LogFilePath = duelLogPath,
                            TimedOut = timedOut
                        };
                    }

                    // Write per-duel log file
                    try
                    {
                        LogDuel($"=== DUEL {gameIndex} END | Winner: {record.Winner} | Turns: {record.TurnCount} | Duration: {record.DurationMs}ms | Success: {record.Success} ===");
                        var allLines = new List<string>();
                        while (duelLogLines.TryDequeue(out var line))
                        {
                            allLines.Add(line);
                        }
                        await File.WriteAllLinesAsync(duelLogPath, allLines);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Warning] Failed to write duel log for game {gameIndex}: {ex.Message}");
                    }

                    // Write JSONL structured log (thread-safe, append)
                    try
                    {
                        var jsonObj = new
                        {
                            game = record.GameIndex,
                            winner = record.Winner,
                            turns = record.TurnCount,
                            success = record.Success,
                            timed_out = record.TimedOut,
                            duration_ms = record.DurationMs,
                            ocg_errors = record.OcgCoreErrors,
                            error = record.ErrorMessage,
                            start_time = record.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            end_time = record.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            log_file = Path.GetFileName(record.LogFilePath),
                            test_deck = currentTestDeck,
                            opponent_deck = currentOpponentDeck
                        };
                        string jsonLine = JsonSerializer.Serialize(jsonObj);
                        lock (jsonlLock)
                        {
                            File.AppendAllText(jsonlPath, jsonLine + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Warning] Failed to write JSONL for game {gameIndex}: {ex.Message}");
                    }

                    // Print result with color coding
                    if (record.Success)
                    {
                        Console.ForegroundColor = record.Winner == currentTestDeck ? ConsoleColor.Green : ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    string statusTag = record.TimedOut ? "TIMEOUT" : (record.Success ? "OK" : "FAILED");
                    Console.WriteLine($"\n>>> DUEL {gameIndex} FINISHED! Winner: {record.Winner} | Turns: {record.TurnCount} | Status: {statusTag} | Duration: {record.DurationMs}ms");
                    Console.ResetColor();

                    return record;
                }

                if (parallelMode && totalGames > 1)
                {
                    var semaphore = new SemaphoreSlim(maxWorkers, maxWorkers);
                    var tasks = new List<Task>();
                    int portCounter = 0;
                    var portLock = new object();

                    // In parallel mode, use wider port gaps (3 per worker) to avoid any conflicts
                    int portGap = Math.Max(3, maxWorkers);

                    for (int g = 1; g <= totalGames; g++)
                    {
                        await semaphore.WaitAsync(cts.Token);
                        int gameIndex = g;
                        tasks.Add(Task.Run(async () =>
                        {
                            try
                            {
                                int port;
                                lock (portLock) { port = basePort + (++portCounter * portGap); }
                                var record = await RunSingleDuel(gameIndex, port);
                                records.Add(record);
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"[Error] Task for Duel {gameIndex} threw: {ex.Message}");
                                Console.ResetColor();
                                // Add a failed record so we don't lose count
                                records.Add(new DuelRecord
                                {
                                    GameIndex = gameIndex,
                                    Success = false,
                                    ErrorMessage = $"Task exception: {ex.Message}"
                                });
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        }));
                    }

                    // FIX: Overall timeout safety net - don't hang forever
                    int overallTimeoutMinutes = Math.Max(5, (totalGames * duelTimeoutSeconds / maxWorkers / 60) + 3);
                    var allComplete = Task.WhenAll(tasks);
                    try
                    {
                        var winner = await Task.WhenAny(allComplete, Task.Delay(TimeSpan.FromMinutes(overallTimeoutMinutes), cts.Token));
                        if (winner != allComplete)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\n[Error] Overall timeout reached ({overallTimeoutMinutes} min). Some duels may have hung. Proceeding with {records.Count} completed records.");
                            Console.ResetColor();

                            try { cts.Cancel(); } catch { }
                            // Wait up to 10 seconds for tasks to clean up their processes
                            await Task.WhenAny(allComplete, Task.Delay(10000));
                        }
                        else
                        {
                            // Propagate any exceptions from the tasks
                            await allComplete;
                        }
                    }
                    catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
                    {
                        Console.WriteLine("\n[Info] Test cancelled by user. Reporting partial results.");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n[Error] Exception during parallel execution: {ex.Message}");
                        Console.ResetColor();
                    }
                }
                else
                {
                    for (int g = 1; g <= totalGames; g++)
                    {
                        if (cts.Token.IsCancellationRequested) break;
                        int port = basePort + g;
                        var record = await RunSingleDuel(g, port);
                        records.Add(record);
                    }
                }

                // Convert to list and sort by game index for consistent reporting
                var sortedRecords = new List<DuelRecord>(records);
                sortedRecords.Sort((a, b) => a.GameIndex.CompareTo(b.GameIndex));

                // Print Matchup E2E Summary Report - ALWAYS print even if some duels hung
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n==================================================================");
                Console.WriteLine($"                  MATCHUP VERIFICATION SUMMARY: {currentTestDeck} vs {currentOpponentDeck}");
                Console.WriteLine("==================================================================");
                Console.ResetColor();

                int testDeckWins = 0;
                int opponentDeckWins = 0;
                int draws = 0;
                int successfulDuels = 0;
                int totalOcgErrors = 0;
                int timedOutDuels = 0;
                long totalDurationMs = 0;

                foreach (var r in sortedRecords)
                {
                    string status = r.TimedOut ? "TIMEOUT" : (r.Success ? "OK" : "FAILED");
                    string ocgTag = r.OcgCoreErrors > 0 ? $" | OCG Errors: {r.OcgCoreErrors}" : "";
                    string durationTag = r.DurationMs > 0 ? $" | {r.DurationMs / 1000.0:F1}s" : "";
                    Console.WriteLine($"  Duel {r.GameIndex:D2} | Winner: {r.Winner,-20} | Turns: {r.TurnCount:D2} | Status: {status}{durationTag}{ocgTag}");
                    totalOcgErrors += r.OcgCoreErrors;
                    totalDurationMs += r.DurationMs;
                    if (r.TimedOut) timedOutDuels++;
                    if (r.Success)
                    {
                        successfulDuels++;
                        if (r.Winner == currentTestDeck) testDeckWins++;
                        else if (r.Winner == currentOpponentDeck) opponentDeckWins++;
                        else draws++;
                    }
                }

                double winrate = successfulDuels > 0 ? (double)testDeckWins / successfulDuels : 0;
                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine($"  Total Duels Run        : {sortedRecords.Count} / {totalGames}");
                Console.WriteLine($"  Total Successful Duels : {successfulDuels} / {sortedRecords.Count}");
                if (timedOutDuels > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  Timed Out              : {timedOutDuels}");
                    Console.ResetColor();
                }
                Console.WriteLine($"  {currentTestDeck} Win Rate     : {testDeckWins} wins ({winrate:P1})");
                Console.WriteLine($"  {currentOpponentDeck} Win Rate    : {opponentDeckWins} wins");
                Console.WriteLine($"  Draws                  : {draws}");
                if (totalDurationMs > 0)
                    Console.WriteLine($"  Total Duration         : {totalDurationMs / 1000.0:F1}s (avg {totalDurationMs / Math.Max(1, sortedRecords.Count) / 1000.0:F1}s/duel)");
                if (totalOcgErrors > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  OCGCore Errors (total) : {totalOcgErrors}");
                    Console.ResetColor();
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  Session Logs           : {sessionLogDir}");
                Console.WriteLine($"  JSONL Results          : {jsonlPath}");
                Console.ResetColor();
                Console.WriteLine("==================================================================\n");

                allMatchupRecords[matchupKey] = sortedRecords;

                // Write JSON summary file
                try
                {
                    var summaryObj = new
                    {
                        matchup = matchupKey,
                        test_deck = currentTestDeck,
                        opponent_deck = currentOpponentDeck,
                        total_games = totalGames,
                        completed = sortedRecords.Count,
                        successful = successfulDuels,
                        timed_out = timedOutDuels,
                        test_deck_wins = testDeckWins,
                        opponent_wins = opponentDeckWins,
                        draws = draws,
                        win_rate = winrate,
                        total_duration_ms = totalDurationMs,
                        avg_duration_ms = sortedRecords.Count > 0 ? totalDurationMs / sortedRecords.Count : 0,
                        ocg_core_errors = totalOcgErrors,
                        timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    string summaryPath = Path.Combine(sessionLogDir, $"summary_{matchupKey}.json");
                    string summaryJson = JsonSerializer.Serialize(summaryObj, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(summaryPath, summaryJson);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[Success] Summary saved to: {summaryPath}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Warning] Failed to write summary JSON: {ex.Message}");
                }

                // Write crash report if any occurred
                bool hasAnyCrash = sortedRecords.Exists(r => !r.Success && !string.IsNullOrEmpty(r.ErrorMessage));
                if (hasAnyCrash)
                {
                    string reportPath = Path.Combine(sessionLogDir, $"crash_report_{matchupKey}.txt");
                    var reportLines = new List<string> { $"=== Crash Report - {matchupKey} ===" };
                    foreach (var r in sortedRecords)
                    {
                        if (!r.Success)
                        {
                            reportLines.Add($"Duel {r.GameIndex}: Winner: {r.Winner} | TimedOut: {r.TimedOut} | Error: {r.ErrorMessage}");
                        }
                    }
                    File.WriteAllLines(reportPath, reportLines);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Warning] Errors detected. Crash report saved to: {reportPath}");
                    Console.ResetColor();
                }
            }

            // Print Final Round-Robin summary
            if (forestMode)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n==================================================================");
                Console.WriteLine("               ROUND-ROBIN TOURNAMENT SUMMARY (FOREST)");
                Console.WriteLine("==================================================================");
                Console.ResetColor();

                foreach (var kvp in allMatchupRecords)
                {
                    string key = kvp.Key;
                    var records = kvp.Value;

                    int testDeckWins = 0;
                    int successfulDuels = 0;
                    foreach (var r in records)
                    {
                        if (r.Success)
                        {
                            successfulDuels++;
                            if (r.Winner == "2026_Dreadnought") testDeckWins++;
                        }
                    }

                    double winrate = successfulDuels > 0 ? (double)testDeckWins / successfulDuels : 0;
                    string opponentName = key.Replace("2026_Dreadnought_vs_", "");
                    Console.WriteLine($"  Dreadnought vs {opponentName,-15} | Successful: {successfulDuels}/{records.Count} | Wins: {testDeckWins} ({winrate:P1})");
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==================================================================\n");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Done] All session logs saved to: {sessionLogDir}");
            Console.ResetColor();
        }
        #region Win32 API for Headless Window Management
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const int SW_HIDE = 0;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOZORDER = 0x0004;
        #endregion
    }
}
