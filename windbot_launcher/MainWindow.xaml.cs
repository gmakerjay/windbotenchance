using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using YgoAiPlatform.Core;


namespace WindBotLauncher
{
    public partial class MainWindow : Window
    {
        // Paths
        private string _projectRoot = string.Empty;
        private string _deployTargetDir = string.Empty;
        private string _windbotDllPath = string.Empty;
        private string _dbPath = string.Empty;

        // Bot processes
        private Process? _botAProcess;
        private Process? _botBProcess;
        private CancellationTokenSource? _botCts;

        // Training
        private CancellationTokenSource? _trainCts;

        // Timers
        private System.Windows.Threading.DispatcherTimer _clockTimer = null!;
        private System.Windows.Threading.DispatcherTimer _monitorTimer = null!;

        public MainWindow()
        {
            InitializeComponent();
            ResolvePaths();
            InitTimers();
            PopulateDecks();
            PopulateQuickStats();
        }

        // ========== INIT ==========

        private void ResolvePaths()
        {
            // Strategy 1: exe อยู่ที่ root repo (self-contained publish)
            //   -> YugiohTH/WindBotLauncher.exe  => YGO_AI_PLATFORM อยู่ข้างๆ
            string exeDir = AppContext.BaseDirectory;
            string? rootCandidate = exeDir;

            // Try exe directory itself and walk up to find YGO_AI_PLATFORM
            while (!string.IsNullOrEmpty(rootCandidate))
            {
                // ถ้าอยู่ใน debug/publish bin — หา YGO_AI_PLATFORM จาก parent
                string candidate = Path.Combine(rootCandidate, "YGO_AI_PLATFORM");
                if (Directory.Exists(candidate))
                {
                    _projectRoot = candidate;
                    break;
                }
                // ถ้าตัวเองคือ YGO_AI_PLATFORM
                if (Path.GetFileName(rootCandidate) == "YGO_AI_PLATFORM" && Directory.Exists(rootCandidate))
                {
                    _projectRoot = rootCandidate;
                    break;
                }
                // ถ้ามี config.json อยู่ใน directory นี้
                if (File.Exists(Path.Combine(rootCandidate, "config.json")))
                {
                    _projectRoot = rootCandidate;
                    break;
                }
                rootCandidate = Path.GetDirectoryName(rootCandidate);
            }

            if (string.IsNullOrEmpty(_projectRoot))
            {
                // Final fallback: relative paths from typical Debug bin location
                var candidates = new[]
                {
                    Path.GetFullPath(Path.Combine(exeDir, "..", "..", "..", "..", "YGO_AI_PLATFORM")),
                    Path.GetFullPath(Path.Combine(exeDir, "..", "..", "YGO_AI_PLATFORM")),
                    Path.GetFullPath(Path.Combine(exeDir, "YGO_AI_PLATFORM")),
                    Path.GetFullPath(Path.Combine(exeDir, "..", "..", "..", "..")),
                };
                foreach (var c in candidates)
                {
                    if (Directory.Exists(c)) { _projectRoot = c; break; }
                }
                _projectRoot ??= exeDir;
            }

            _dbPath = Path.Combine(_projectRoot, "logs", "dashboard.db");
            
            string gameEdoProDir = Path.Combine(_projectRoot, "..", "Game_EDOPro");
            if (!Directory.Exists(gameEdoProDir))
            {
                string edoGameDir = Path.Combine(_projectRoot, "..", "EdoGame");
                if (Directory.Exists(edoGameDir))
                {
                    gameEdoProDir = edoGameDir;
                }
            }
            _deployTargetDir = Path.GetFullPath(Path.Combine(gameEdoProDir, "WindBot", "models"));

            // Find WindBot.dll
            string[] dllCandidates = new[]
            {
                Path.Combine(_projectRoot, "windbot-fork", "bin", "x86", "Debug", "net10.0", "WindBot.dll"),
                Path.Combine(_projectRoot, "windbot-fork", "bin", "Debug", "net10.0", "WindBot.dll"),
                Path.Combine(_projectRoot, "windbot-fork", "bin", "x86", "Release", "net10.0", "WindBot.dll"),
                Path.Combine(_projectRoot, "windbot-fork", "bin", "Release", "net10.0", "WindBot.dll")
            };

            _windbotDllPath = string.Empty;
            foreach (var candidate in dllCandidates)
            {
                if (File.Exists(candidate))
                {
                    _windbotDllPath = candidate;
                    break;
                }
            }

            if (string.IsNullOrEmpty(_windbotDllPath))
            {
                _windbotDllPath = Path.Combine(_deployTargetDir, "..", "WindBot.dll");
            }

            Log($"[Init] Project Root: {_projectRoot}");
            Log($"[Init] DB Path: {_dbPath}");
            Log($"[Init] Deploy Target: {_deployTargetDir}");
            Log($"[Init] WindBot DLL: {_windbotDllPath}");
        }

        private void InitTimers()
        {
            _clockTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _clockTimer.Tick += (s, e) =>
            {
                TxtClock.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            _clockTimer.Start();

            _monitorTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            _monitorTimer.Tick += (s, e) => MonitorBotProcesses();
            _monitorTimer.Start();
        }

        private void PopulateDecks()
        {
            var decks = new List<string> {
                "Neural",
                "Altergeist", "BlueEyes", "DarkMagician", "Salamangreat",
                "SkyStriker", "ABC", "Burn", "Horus",
                "2026_AncientG", "2026_DarkTime", "2026_DarkWorld", "2026_DDD", "2026_Doomz", "2026_Dracotail",
                "2026_Dreadnought", "2026_EvilTwin", "2026_EyeInside", "2026_Fireking", "2026_Goldlord",
                "2026_Hecahand", "2026_Invoke", "2026_K9", "2026_Kwtune", "2026_Labrynth",
                "2026_Luna", "2026_Magnet", "2026_Maliss", "2026_Radiant", "2026_RaiOh",
                "2026_RyuGe", "2026_SkyStriker", "2026_Solfachord", "2026_TrueDraco", "2026_Archfiend",
                "2026_Purrely", "2026_Regenesis", "2026_Yummy"
            };

            // Also scan Decks directory for .ydk files
            string decksDir = Path.Combine(_projectRoot, "windbot-fork", "Decks");
            if (Directory.Exists(decksDir))
            {
                foreach (var file in Directory.GetFiles(decksDir, "*.ydk"))
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    if (!decks.Contains(name))
                        decks.Add(name);
                }
            }

            foreach (var d in decks.OrderBy(x => x))
            {
                CbBotDeck.Items.Add(d);
                CbOpponentDeck.Items.Add(d);
                
                // Populate CbTrainDeck with Neural_ and Expert_ versions of 2026 decks
                if (d.StartsWith("2026_"))
                {
                    CbTrainDeck.Items.Add(new ComboBoxItem { Content = "Neural_" + d });
                    CbTrainDeck.Items.Add(new ComboBoxItem { Content = "Expert_" + d });
                }
            }

            if (CbBotDeck.Items.Count > 0) CbBotDeck.SelectedIndex = 0;
            if (CbOpponentDeck.Items.Count > 0) CbOpponentDeck.SelectedIndex = 1;
        }

        // ========== PLAY TAB ==========

        private void RbTwoBots_Checked(object sender, RoutedEventArgs e)
        {
            LblOpponentDeckTitle.Visibility = Visibility.Visible;
            CbOpponentDeck.Visibility = Visibility.Visible;
        }

        private void RbTwoBots_Unchecked(object sender, RoutedEventArgs e)
        {
            LblOpponentDeckTitle.Visibility = Visibility.Collapsed;
            CbOpponentDeck.Visibility = Visibility.Collapsed;
        }

        private async void BtnStartBot_Click(object sender, RoutedEventArgs e)
        {
            string host = TxtHost.Text.Trim();
            string portStr = TxtPort.Text.Trim();
            string botDeck = CbBotDeck.Text.Trim();
            bool twoBots = RbTwoBots.IsChecked == true;
            string oppDeck = CbOpponentDeck.Text.Trim(); // Read on UI thread before Task.Run

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(portStr) || !int.TryParse(portStr, out int port))
            {
                MessageBox.Show("กรุณากรอก Host/Port ให้ถูกต้อง", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!File.Exists(_windbotDllPath))
            {
                MessageBox.Show($"ไม่พบ WindBot.dll ที่: {_windbotDllPath}\nกรุณา build windbot-fork ก่อน",
                    "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            BtnStartBot.IsEnabled = false;
            BtnStopBot.IsEnabled = true;
            TxtStatus.Text = "🟡 กำลังรันบอท...";
            TxtStatus.Foreground = FindResource("WarningColor") as System.Windows.Media.Brush;

            _botCts = new CancellationTokenSource();
            var token = _botCts.Token;

            try
            {
                await Task.Run(() =>
                {
                    // Start Bot A
                    _botAProcess = LaunchWindBot(botDeck, $"BotA_{botDeck}", host, port);
                    Dispatcher.Invoke(() =>
                    {
                        Log($"[BotA] {botDeck} started (PID: {_botAProcess?.Id})");
                        TxtBotStatus.Text = $"Bot A: {botDeck} — PID {_botAProcess?.Id}\nเชื่อมต่อ {host}:{port}";
                    });

                    if (twoBots)
                    {
                        Thread.Sleep(1000);
                        string botBName = botDeck == oppDeck ? $"{oppDeck}_B" : oppDeck;
                        _botBProcess = LaunchWindBot(oppDeck, botBName, host, port);
                        Dispatcher.Invoke(() =>
                        {
                            Log($"[BotB] {oppDeck} started (PID: {_botBProcess?.Id})");
                            TxtBotStatus.Text += $"\nBot B: {oppDeck} — PID {_botBProcess?.Id}";
                        });
                    }

                    // Monitor until cancellation
                    while (!token.IsCancellationRequested)
                    {
                        if ((_botAProcess?.HasExited ?? true) &&
                            (_botBProcess == null || _botBProcess.HasExited))
                            break;
                        Thread.Sleep(500);
                    }
                }, token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Log($"[ERROR] {ex.Message}");
            }
            finally
            {
                StopAllBots();
                Dispatcher.Invoke(() =>
                {
                    BtnStartBot.IsEnabled = true;
                    BtnStopBot.IsEnabled = false;
                    TxtStatus.Text = "🟢 พร้อม";
                    TxtStatus.Foreground = FindResource("SuccessColor") as System.Windows.Media.Brush;
                });
            }
        }

        private Process LaunchWindBot(string deck, string name, string host, int port)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"\"{_windbotDllPath}\" Name={name} Deck={deck} Host={host} Port={port}",
                WorkingDirectory = Path.GetDirectoryName(_windbotDllPath),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            var process = new Process { StartInfo = psi };
            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null)
                    Dispatcher.Invoke(() => Log($"[{name}] {e.Data}"));
            };
            process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null)
                    Dispatcher.Invoke(() => Log($"[{name} ERR] {e.Data}"));
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            return process;
        }

        private void BtnStopBot_Click(object sender, RoutedEventArgs e)
        {
            _botCts?.Cancel();
            StopAllBots();
            BtnStartBot.IsEnabled = true;
            BtnStopBot.IsEnabled = false;
            Log("[System] Bot stopped by user");
        }

        private void StopAllBots()
        {
            KillProcess(_botAProcess);
            KillProcess(_botBProcess);
            _botAProcess = null;
            _botBProcess = null;
        }

        private void MonitorBotProcesses()
        {
            if (_botAProcess != null && _botAProcess.HasExited)
            {
                Log($"[BotA] Process exited (code: {_botAProcess.ExitCode})");
                _botAProcess?.Dispose();
                _botAProcess = null;
            }
            if (_botBProcess != null && _botBProcess.HasExited)
            {
                Log($"[BotB] Process exited (code: {_botBProcess.ExitCode})");
                _botBProcess?.Dispose();
                _botBProcess = null;
            }
            if (_botAProcess == null && _botBProcess == null && BtnStartBot != null && !BtnStartBot.IsEnabled)
            {
                Dispatcher.Invoke(() =>
                {
                    BtnStartBot.IsEnabled = true;
                    BtnStopBot.IsEnabled = false;
                    TxtStatus.Text = "🟢 พร้อม";
                    TxtStatus.Foreground = FindResource("SuccessColor") as System.Windows.Media.Brush;
                    Log("[System] All bots finished");
                });
            }
        }

        // ========== TRAIN TAB ==========

        private async void BtnStartTrain_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ระบบเทรน AI ถูกลบออกแล้ว", "ไม่พร้อมใช้งาน", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnStopTrain_Click(object sender, RoutedEventArgs e)
        {
            _trainCts?.Cancel();
            BtnStartTrain.IsEnabled = true;
            BtnStopTrain.IsEnabled = false;
            PbTrainProgress.Visibility = Visibility.Collapsed;
            TxtTrainProgress.Visibility = Visibility.Collapsed;
            TxtStatus.Text = "🟢 พร้อม";
            LogTrain("⏹ Training stopped by user");
        }

        // ========== DASHBOARD TAB ==========

        private void BtnRefreshDashboard_Click(object sender, RoutedEventArgs e)
        {
            RefreshDashboard();
        }

        private void RefreshDashboard()
        {
            try
            {
                if (!File.Exists(_dbPath))
                {
                    TxtDashboardLastRefresh.Text = "⚠ ไม่พบ dashboard.db";
                    return;
                }

                using var conn = new SqliteConnection($"Data Source={_dbPath}");
                conn.Open();

                // Training Runs
                var runs = new List<dynamic>();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT run_id, deck_name, total_games, wins, losses, draws, epochs, samples_collected, samples_filtered, initial_loss, final_loss, initial_elo, final_elo, status, started_at FROM training_runs ORDER BY started_at DESC";
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        runs.Add(new
                        {
                            RunId = reader.GetString(0)[..Math.Min(12, reader.GetString(0).Length)],
                            Deck = reader.GetString(1),
                            Games = reader.GetInt32(2),
                            Wins = reader.GetInt32(3),
                            Losses = reader.GetInt32(4),
                            Draws = reader.GetInt32(5),
                            Epochs = reader.GetInt32(6),
                            WinRate = reader.GetInt32(2) > 0 ? $"{reader.GetInt32(3) * 100.0 / reader.GetInt32(2):F1}%" : "N/A",
                            Loss = $"{reader.GetDouble(10):F4} → {reader.GetDouble(11):F4}",
                            Elo = $"{reader.GetDouble(12):F0} → {reader.GetDouble(13):F0}",
                            Status = reader.GetString(14)
                        });
                    }
                }
                DgTrainingRuns.ItemsSource = runs;

                // Elo History
                var elo = new List<dynamic>();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT deck_id, elo, episode, timestamp FROM elo_history ORDER BY id DESC LIMIT 50";
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        elo.Add(new { Deck = reader.GetString(0), Elo = reader.GetDouble(1), Episode = reader.GetInt32(2), Timestamp = reader.GetString(3) });
                    }
                }
                DgEloHistory.ItemsSource = elo;

                // Matchups
                var matchups = new List<dynamic>();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT deck_a, deck_b, COUNT(*) as total, 
                        SUM(CASE WHEN winner = 'a' THEN 1 ELSE 0 END) as wins_a,
                        SUM(CASE WHEN winner = 'b' THEN 1 ELSE 0 END) as wins_b
                        FROM deck_matchups GROUP BY deck_a, deck_b ORDER BY total DESC";
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int total = reader.GetInt32(2);
                        int winsA = reader.GetInt32(3);
                        int winsB = reader.GetInt32(4);
                        matchups.Add(new
                        {
                            DeckA = reader.GetString(0),
                            DeckB = reader.GetString(1),
                            Total = total,
                            Wins_A = winsA,
                            Wins_B = winsB,
                            WinRate_A = total > 0 ? $"{winsA * 100.0 / total:F1}%" : "N/A"
                        });
                    }
                }
                DgMatchups.ItemsSource = matchups;

                // Deck Stats
                var stats = new List<dynamic>();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT deck_a as deck, COUNT(*) as matches,
                        SUM(CASE WHEN winner = 'a' THEN 1 ELSE 0 END) as wins
                        FROM deck_matchups GROUP BY deck_a
                        UNION ALL
                        SELECT deck_b as deck, COUNT(*) as matches,
                        SUM(CASE WHEN winner = 'b' THEN 1 ELSE 0 END) as wins
                        FROM deck_matchups GROUP BY deck_b";
                    using var reader = cmd.ExecuteReader();
                    var deckStats = new Dictionary<string, (int matches, int wins)>();
                    while (reader.Read())
                    {
                        string deck = reader.GetString(0);
                        int m = reader.GetInt32(1);
                        int w = reader.GetInt32(2);
                        if (deckStats.ContainsKey(deck))
                        {
                            var existing = deckStats[deck];
                            deckStats[deck] = (existing.matches + m, existing.wins + w);
                        }
                        else
                        {
                            deckStats[deck] = (m, w);
                        }
                    }
                    foreach (var kv in deckStats.OrderByDescending(x => x.Value.wins))
                    {
                        stats.Add(new
                        {
                            Deck = kv.Key,
                            Matches = kv.Value.matches,
                            Wins = kv.Value.wins,
                            Losses = kv.Value.matches - kv.Value.wins,
                            WinRate = kv.Value.matches > 0 ? $"{kv.Value.wins * 100.0 / kv.Value.matches:F1}%" : "N/A"
                        });
                    }
                }
                DgDeckStats.ItemsSource = stats;

                conn.Close();
                TxtDashboardLastRefresh.Text = $"Last refresh: {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                TxtDashboardLastRefresh.Text = $"Error: {ex.Message}";
            }
        }

        // ========== DATASETS TAB ==========

        private void BtnRefreshDatasets_Click(object sender, RoutedEventArgs e)
        {
            RefreshDatasets();
        }

        private void RefreshDatasets()
        {
            LbDatasetDecks.Items.Clear();
            string datasetsDir = Path.Combine(_projectRoot, "datasets");
            if (!Directory.Exists(datasetsDir)) return;

            foreach (var deckDir in Directory.GetDirectories(datasetsDir))
            {
                string deckName = Path.GetFileName(deckDir);
                int fileCount = Directory.GetFiles(deckDir, "*.jsonl").Length;
                long totalSize = Directory.GetFiles(deckDir, "*.jsonl").Sum(f => new FileInfo(f).Length);
                LbDatasetDecks.Items.Add($"{deckName} ({fileCount} files, {FormatBytes(totalSize)})");
            }
        }

        private void LbDatasetDecks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LbDatasetFiles.Items.Clear();
            TxtDatasetPreview.Text = "";
            if (LbDatasetDecks.SelectedItem == null) return;

            string selection = LbDatasetDecks.SelectedItem.ToString()!;
            string deckName = selection.Split('(')[0].Trim();
            string deckDir = Path.Combine(_projectRoot, "datasets", deckName);

            if (!Directory.Exists(deckDir)) return;

            var files = Directory.GetFiles(deckDir, "*.jsonl")
                .OrderByDescending(f => new FileInfo(f).Length)
                .ToList();

            int nonEmpty = files.Count(f => new FileInfo(f).Length > 10);
            long totalSamples = 0;
            foreach (var file in files)
            {
                string fname = Path.GetFileName(file);
                long size = new FileInfo(file).Length;
                if (size > 10)
                {
                    int lines = File.ReadLines(file).Count();
                    totalSamples += lines;
                    LbDatasetFiles.Items.Add($"{fname} — {lines} samples ({FormatBytes(size)})");
                }
                else
                {
                    LbDatasetFiles.Items.Add($"{fname} — EMPTY");
                }
            }

            TxtDatasetInfo.Text = $"{deckName}\n{files.Count} files\n{nonEmpty} non-empty\n{totalSamples} total samples";
            TxtDatasetFileTitle.Text = $"{deckName} — {nonEmpty} files with data";
        }

        private void LbDatasetFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LbDatasetFiles.SelectedItem == null) return;
            string selection = LbDatasetFiles.SelectedItem.ToString()!;
            string fileName = selection.Split(" — ")[0].Trim();

            string deckSelection = LbDatasetDecks.SelectedItem?.ToString() ?? "";
            string deckName = deckSelection.Split('(')[0].Trim();
            string filePath = Path.Combine(_projectRoot, "datasets", deckName, fileName);

            if (!File.Exists(filePath))
            {
                TxtDatasetPreview.Text = $"[File not found: {filePath}]";
                return;
            }

            try
            {
                // Show first 20 samples
                var sb = new StringBuilder();
                int count = 0;
                foreach (var line in File.ReadLines(filePath))
                {
                    if (count >= 20) break;
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        // Pretty-print first 200 chars of each sample
                        string display = line.Length > 500 ? line[..500] + "..." : line;
                        sb.AppendLine($"--- Sample {count + 1} ---");
                        sb.AppendLine(display);
                        sb.AppendLine();
                        count++;
                    }
                }
                sb.AppendLine($"... ({count} samples shown)");
                TxtDatasetPreview.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                TxtDatasetPreview.Text = $"Error reading file: {ex.Message}";
            }
        }

        // ========== MODELS TAB ==========

        private void BtnRefreshModels_Click(object sender, RoutedEventArgs e)
        {
            RefreshModels();
        }

        private void BtnDeployWeights_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ระบบเทรน AI ถูกลบออกแล้ว", "ไม่พร้อมใช้งาน", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshModels()
        {
            // Training models
            string modelsDir = Path.Combine(_projectRoot, "models");
            var trainModels = new List<dynamic>();

            if (Directory.Exists(modelsDir))
            {
                foreach (var file in Directory.GetFiles(modelsDir, "weights*.json"))
                {
                    var fi = new FileInfo(file);
                    int weightCount = 0;
                    try
                    {
                        var json = File.ReadAllText(file);
                        if (json.TrimStart().StartsWith("["))
                        {
                            var arr = JsonSerializer.Deserialize<double[]>(json);
                            weightCount = arr?.Length ?? 0;
                        }
                        else
                        {
                            using var doc = JsonDocument.Parse(json);
                            if (doc.RootElement.TryGetProperty("Weights", out var wProp) ||
                                doc.RootElement.TryGetProperty("weights", out wProp))
                            {
                                weightCount = wProp.GetArrayLength();
                            }
                        }
                    }
                    catch { }

                    trainModels.Add(new
                    {
                        File = fi.Name,
                        Size = FormatBytes(fi.Length),
                        Weights = weightCount,
                        Modified = fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                    });
                }
            }
            DgTrainModels.ItemsSource = trainModels;

            // Deployed models
            var deployedModels = new List<dynamic>();
            if (Directory.Exists(_deployTargetDir))
            {
                foreach (var file in Directory.GetFiles(_deployTargetDir, "weights*.json"))
                {
                    var fi = new FileInfo(file);
                    deployedModels.Add(new
                    {
                        File = fi.Name,
                        Size = FormatBytes(fi.Length),
                        Modified = fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                    });
                }
            }
            DgDeployedModels.ItemsSource = deployedModels;

            TxtWeightPreview.Text = $"Training dir: {modelsDir}\nDeploy dir: {_deployTargetDir}";
        }

        // ========== TOOLS TAB ==========

        private void BtnOpenTrainGui_Click(object sender, RoutedEventArgs e)
        {
            // build + launch train_gui (legacy WPF)
            LogTools("[Tools] กำลัง build และเปิด Train GUI เดิม...");
            Task.Run(() =>
            {
                string projPath = Path.Combine(_projectRoot, "train_gui", "train_gui.csproj");
                if (!File.Exists(projPath))
                {
                    Dispatcher.Invoke(() => LogTools($"[Tools] ❌ ไม่พบ: {projPath}"));
                    return;
                }
                RunShellCommand("dotnet", $"build \"{projPath}\" -v q", _projectRoot,
                    onOutput: line => Dispatcher.Invoke(() => LogTools(line)),
                    onError:  line => Dispatcher.Invoke(() => LogTools("ERR: " + line)),
                    onDone: code =>
                    {
                        if (code != 0) { Dispatcher.Invoke(() => LogTools("[Tools] ❌ Build ล้มเหลว")); return; }
                        string exePath = Path.Combine(_projectRoot, "train_gui", "bin", "Debug", "net10.0-windows", "train_gui.exe");
                        if (!File.Exists(exePath)) { Dispatcher.Invoke(() => LogTools($"[Tools] ❌ ไม่พบ exe: {exePath}")); return; }
                        Process.Start(new ProcessStartInfo(exePath) { UseShellExecute = true });
                        Dispatcher.Invoke(() => LogTools("[Tools] ✅ Train GUI เปิดแล้ว"));
                    });
            });
        }

        private void BtnBuildVps_Click(object sender, RoutedEventArgs e)
        {
            LogTools("[Tools] กำลัง Build & Package VPS...");
            Task.Run(() =>
            {
                string ps1 = Path.Combine(_projectRoot, "vps_package.ps1");
                if (!File.Exists(ps1))
                {
                    Dispatcher.Invoke(() => LogTools($"[Tools] ❌ ไม่พบ: {ps1}"));
                    return;
                }
                RunShellCommand("powershell",
                    $"-ExecutionPolicy Bypass -File \"{ps1}\"",
                    _projectRoot,
                    onOutput: line => Dispatcher.Invoke(() => LogTools(line)),
                    onError:  line => Dispatcher.Invoke(() => LogTools("ERR: " + line)),
                    onDone: code =>
                    {
                        Dispatcher.Invoke(() => LogTools(code == 0
                            ? $"[Tools] ✅ VPS package สำเร็จ → {Path.Combine(_projectRoot, "vps_deploy")}"
                            : "[Tools] ❌ VPS package ล้มเหลว"));
                    });
            });
        }

        private void BtnCopyDesktop_Click(object sender, RoutedEventArgs e)
        {
            string src = Path.Combine(_projectRoot, "vps_deploy");
            string dst = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                "vps_deploy");

            if (!Directory.Exists(src))
            {
                LogTools($"[Tools] ❌ ไม่พบ vps_deploy ที่: {src}  — กรุณา Build VPS ก่อน");
                return;
            }

            LogTools($"[Tools] กำลังคัดลอก → {dst}");
            Task.Run(() =>
            {
                try
                {
                    CopyDirectory(src, dst);
                    Dispatcher.Invoke(() => LogTools($"[Tools] ✅ คัดลอกสำเร็จ → {dst}"));
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => LogTools($"[Tools] ❌ {ex.Message}"));
                }
            });
        }

        private void BtnRunTests_Click(object sender, RoutedEventArgs e)
        {
            LogTools("[Tools] กำลังรัน E2E Tests...");
            Task.Run(() =>
            {
                string testProj = Path.Combine(_projectRoot, "tests", "tests.csproj");
                if (!File.Exists(testProj))
                {
                    Dispatcher.Invoke(() => LogTools($"[Tools] ❌ ไม่พบ: {testProj}"));
                    return;
                }
                RunShellCommand("dotnet", $"test \"{testProj}\" --no-build --logger console",
                    _projectRoot,
                    onOutput: line => Dispatcher.Invoke(() => LogTools(line)),
                    onError:  line => Dispatcher.Invoke(() => LogTools("ERR: " + line)),
                    onDone: code =>
                    {
                        Dispatcher.Invoke(() => LogTools(code == 0
                            ? "[Tools] ✅ Tests PASSED"
                            : $"[Tools] ❌ Tests FAILED (exit {code})"));
                    });
            });
        }

        private void BtnRunHeadless_Click(object sender, RoutedEventArgs e)
        {
            LogTools("[Tools] กำลังรัน Headless Simulation...");
            Task.Run(() =>
            {
                // ลอง run_headless_debug.bat ก่อน ถ้าไม่มีให้ run training/Program.cs
                string batPath = Path.Combine(_projectRoot, "run_headless_debug.bat");
                if (File.Exists(batPath))
                {
                    RunShellCommand("cmd", $"/c \"{batPath}\"",
                        _projectRoot,
                        onOutput: line => Dispatcher.Invoke(() => LogTools(line)),
                        onError:  line => Dispatcher.Invoke(() => LogTools("ERR: " + line)),
                        onDone: code => Dispatcher.Invoke(() => LogTools(
                            code == 0 ? "[Tools] ✅ Headless เสร็จสิ้น" : $"[Tools] ❌ exit {code}")));
                }
                else
                {
                    string trainingProj = Path.Combine(_projectRoot, "training", "training.csproj");
                    RunShellCommand("dotnet", $"run --project \"{trainingProj}\" -- headless",
                        _projectRoot,
                        onOutput: line => Dispatcher.Invoke(() => LogTools(line)),
                        onError:  line => Dispatcher.Invoke(() => LogTools("ERR: " + line)),
                        onDone: code => Dispatcher.Invoke(() => LogTools(
                            code == 0 ? "[Tools] ✅ Headless เสร็จสิ้น" : $"[Tools] ❌ exit {code}")));
                }
            });
        }

        private void BtnDeckAuditor_Click(object sender, RoutedEventArgs e)
        {
            LogTools("[Tools] กำลังรัน Deck Auditor (Python)...");
            Task.Run(() =>
            {
                // ค้นหา deck_auditor.py จาก project root หรือ parent
                string repoRoot = Path.GetFullPath(Path.Combine(_projectRoot, ".."));
                string py = Path.Combine(repoRoot, "deck_auditor.py");
                if (!File.Exists(py)) py = Path.Combine(_projectRoot, "deck_auditor.py");
                if (!File.Exists(py))
                {
                    Dispatcher.Invoke(() => LogTools($"[Tools] ❌ ไม่พบ deck_auditor.py"));
                    return;
                }
                RunShellCommand("python", $"\"{py}\"",
                    Path.GetDirectoryName(py)!,
                    onOutput: line => Dispatcher.Invoke(() => LogTools(line)),
                    onError:  line => Dispatcher.Invoke(() => LogTools("ERR: " + line)),
                    onDone: code => Dispatcher.Invoke(() => LogTools(
                        code == 0 ? "[Tools] ✅ Deck Auditor เสร็จสิ้น" : $"[Tools] ❌ exit {code}")));
            });
        }

        private void BtnCleanData_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "⚠ จะลบโฟลเดอร์ datasets/ และ logs/ ทั้งหมดแล้วสร้างใหม่\nต้องการดำเนินการต่อหรือไม่?",
                "ยืนยันการลบข้อมูล", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            LogTools("[Tools] กำลังล้างข้อมูลชั่วคราว...");
            Task.Run(() =>
            {
                try
                {
                    foreach (var folder in new[] { "datasets", "logs" })
                    {
                        string path = Path.Combine(_projectRoot, folder);
                        if (Directory.Exists(path))
                        {
                            Directory.Delete(path, recursive: true);
                            Dispatcher.Invoke(() => LogTools($"[Tools] ลบ {folder}/ แล้ว"));
                        }
                        Directory.CreateDirectory(path);
                        Dispatcher.Invoke(() => LogTools($"[Tools] สร้าง {folder}/ ใหม่แล้ว"));
                    }
                    Dispatcher.Invoke(() => LogTools("[Tools] ✅ ล้างข้อมูลเรียบร้อย"));
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => LogTools($"[Tools] ❌ {ex.Message}"));
                }
            });
        }

        private void BtnClearTools_Click(object sender, RoutedEventArgs e)
        {
            TxtToolsOutput.Text = "";
        }

        private void LogTools(string message)
        {
            Dispatcher.Invoke(() =>
            {
                string ts = DateTime.Now.ToString("HH:mm:ss");
                TxtToolsOutput.AppendText($"[{ts}] {message}\n");
                TxtToolsOutput.ScrollToEnd();
                // ส่งไปที่ footer ด้วย
                TxtFooter.Text = message.Length > 80 ? message[..80] + "..." : message;
            });
        }

        /// <summary>รัน process แบบ async พร้อม stream output กลับมา</summary>
        private static void RunShellCommand(
            string exe, string args, string workDir,
            Action<string>? onOutput, Action<string>? onError, Action<int>? onDone)
        {
            var psi = new ProcessStartInfo
            {
                FileName = exe,
                Arguments = args,
                WorkingDirectory = workDir,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };
            using var p = new Process { StartInfo = psi };
            p.OutputDataReceived += (_, e) => { if (e.Data != null) onOutput?.Invoke(e.Data); };
            p.ErrorDataReceived  += (_, e) => { if (e.Data != null) onError?.Invoke(e.Data); };
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();
            onDone?.Invoke(p.ExitCode);
        }

        private static void CopyDirectory(string src, string dst)
        {
            Directory.CreateDirectory(dst);
            foreach (var file in Directory.GetFiles(src))
                File.Copy(file, Path.Combine(dst, Path.GetFileName(file)), overwrite: true);
            foreach (var dir in Directory.GetDirectories(src))
                CopyDirectory(dir, Path.Combine(dst, Path.GetFileName(dir)));
        }

        // ========== HELPERS ==========

        private void Log(string message)
        {
            Dispatcher.Invoke(() =>
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                TxtConsole.AppendText($"[{timestamp}] {message}\n");
                TxtConsole.ScrollToEnd();
            });
        }

        private void LogTrain(string message)
        {
            Dispatcher.Invoke(() =>
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                TxtTrainOutput.AppendText($"[{timestamp}] {message}\n");
                TxtTrainOutput.ScrollToEnd();
            });
        }

        private void BtnClearConsole_Click(object sender, RoutedEventArgs e)
        {
            TxtConsole.Text = "";
        }

        private void KillProcess(Process? process)
        {
            if (process == null) return;
            try
            {
                if (!process.HasExited)
                {
                    process.Kill(true);
                    process.WaitForExit(3000);
                }
            }
            catch { }
            finally
            {
                try { process.Dispose(); } catch { }
            }
        }

        private static string FormatBytes(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
            return $"{bytes / (1024.0 * 1024.0):F1} MB";
        }

        /// <summary>
        /// Populate quick stats panel on Play tab with summary of available models/datasets.
        /// </summary>
        private void PopulateQuickStats()
        {
            try
            {
                var sb = new StringBuilder();

                // Count models
                string modelsDir = Path.Combine(_projectRoot, "models");
                int modelCount = Directory.Exists(modelsDir)
                    ? Directory.GetFiles(modelsDir, "weights*.json").Length
                    : 0;
                sb.AppendLine($"📦 Models: {modelCount} weight files");

                // Count datasets
                string datasetsDir = Path.Combine(_projectRoot, "datasets");
                int deckCount = 0;
                int totalFiles = 0;
                if (Directory.Exists(datasetsDir))
                {
                    var deckDirs = Directory.GetDirectories(datasetsDir);
                    deckCount = deckDirs.Length;
                    foreach (var d in deckDirs)
                        totalFiles += Directory.GetFiles(d, "*.jsonl").Length;
                }
                sb.AppendLine($"📂 Datasets: {deckCount} decks, {totalFiles} files");

                // WindBot DLL
                sb.AppendLine(File.Exists(_windbotDllPath) ? "✅ WindBot.dll found" : "❌ WindBot.dll missing");

                // DB status
                sb.AppendLine(File.Exists(_dbPath) ? "✅ Dashboard DB found" : "⚠ Dashboard DB missing");

                TxtQuickStats.Text = sb.ToString().TrimEnd();
            }
            catch
            {
                TxtQuickStats.Text = "⚠ ไม่สามารถโหลดข้อมูลด่วนได้";
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _clockTimer?.Stop();
            _monitorTimer?.Stop();
            _botCts?.Cancel();
            _trainCts?.Cancel();
            StopAllBots();
            base.OnClosed(e);
        }
    }
}
