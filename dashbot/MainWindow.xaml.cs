using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using YgoAiPlatform.Core;

namespace dashbot
{
    public partial class MainWindow : Window
    {
        private string _decksDir = string.Empty;
        private string _windbotDllPath = string.Empty;
        private readonly System.Windows.Threading.DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            ResolvePaths();

            // Real-time clock updater
            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => TxtTime.Text = DateTime.Now.ToString("HH:mm:ss");
            _timer.Start();

            TxtConsole.Text = "=== YGO Standalone Bot Launcher v2.3 (dashbot) ===\n" +
                              "Rule-based Bot Execution Engine\n" +
                              "---------------------------------------------------\n" +
                              $"Decks Directory: {_decksDir}\n" +
                              $"WindBot Engine: {_windbotDllPath}\n" +
                              "---------------------------------------------------\n" +
                              "Select a deck and host settings on the left, then click 'Start & Connect Bot to Room'.\n" +
                              "For training: Enable 'Spawn 2 Bots' for automatic self-play duels.\n\n";

            PopulateDecksComboBox();
        }

        private void ResolvePaths()
        {
            // Try deployed path first (when running inside Game_EDOPro/)
            string deployedDecks = Path.Combine(AppContext.BaseDirectory, "WindBot", "Decks");
            string deployedDll = Path.Combine(AppContext.BaseDirectory, "WindBot", "WindBot.dll");

            if (Directory.Exists(deployedDecks) && File.Exists(deployedDll))
            {
                _decksDir = deployedDecks;
                _windbotDllPath = deployedDll;
                return;
            }

            // Fallback: search up for dev environment (running from YGO_AI_PLATFORM/)
            string? searchDir = AppContext.BaseDirectory;
            while (!string.IsNullOrEmpty(searchDir))
            {
                string devPath = Path.Combine(searchDir, "YGO_AI_PLATFORM");
                if (Directory.Exists(devPath))
                {
                    string devDecks = Path.Combine(devPath, "windbot-fork", "Decks");
                    if (Directory.Exists(devDecks))
                    {
                        _decksDir = devDecks;
                        string[] candidates = new[]
                        {
                            Path.Combine(devPath, "windbot-fork", "bin", "x86", "Debug", "net10.0", "WindBot.dll"),
                            Path.Combine(devPath, "windbot-fork", "bin", "Debug", "net10.0", "WindBot.dll"),
                            Path.Combine(devPath, "windbot-fork", "bin", "x86", "Release", "net10.0", "WindBot.dll"),
                            Path.Combine(devPath, "windbot-fork", "bin", "Release", "net10.0", "WindBot.dll")
                        };
                        foreach (var path in candidates)
                        {
                            if (File.Exists(path))
                            {
                                _windbotDllPath = path;
                                return;
                            }
                        }
                    }
                }

                string rootPath = Path.Combine(searchDir, "config.json");
                if (File.Exists(rootPath))
                {
                    string devDecks = Path.Combine(searchDir, "windbot-fork", "Decks");
                    if (Directory.Exists(devDecks))
                    {
                        _decksDir = devDecks;
                        string[] candidates = new[]
                        {
                            Path.Combine(searchDir, "windbot-fork", "bin", "x86", "Debug", "net10.0", "WindBot.dll"),
                            Path.Combine(searchDir, "windbot-fork", "bin", "Debug", "net10.0", "WindBot.dll"),
                            Path.Combine(searchDir, "windbot-fork", "bin", "x86", "Release", "net10.0", "WindBot.dll"),
                            Path.Combine(searchDir, "windbot-fork", "bin", "Release", "net10.0", "WindBot.dll")
                        };
                        foreach (var path in candidates)
                        {
                            if (File.Exists(path))
                            {
                                _windbotDllPath = path;
                                return;
                            }
                        }
                    }
                }

                searchDir = Path.GetDirectoryName(searchDir);
            }

            // Ultimate fallback
            _decksDir = Path.Combine(AppContext.BaseDirectory, "WindBot", "Decks");
            _windbotDllPath = Path.Combine(AppContext.BaseDirectory, "WindBot", "WindBot.dll");
        }

        private void PopulateDecksComboBox()
        {
            var legacyDecks = new System.Collections.Generic.List<string>();
            var new2026Decks = new System.Collections.Generic.List<string>();
            var otherDecks = new System.Collections.Generic.List<string>();

            try
            {
                if (Directory.Exists(_decksDir))
                {
                    var files = Directory.GetFiles(_decksDir, "*.ydk");
                    
                    foreach (var file in files)
                    {
                        string originalName = Path.GetFileNameWithoutExtension(file);
                        string cleanName = originalName;

                        if (cleanName.StartsWith("AI_"))
                        {
                            cleanName = cleanName.Substring(3); // Remove "AI_" prefix
                        }
                        
                        // Strip Neural_ or Expert_ prefix if they are in the deck filenames
                        if (cleanName.StartsWith("Neural_"))
                        {
                            cleanName = cleanName.Substring(7);
                        }
                        else if (cleanName.StartsWith("Expert_"))
                        {
                            cleanName = cleanName.Substring(7);
                        }

                        // Categorize based on original name prefix
                        if (originalName.StartsWith("2026_") || originalName.StartsWith("Expert_2026_") || originalName.StartsWith("Neural_2026_"))
                        {
                            if (!new2026Decks.Contains(cleanName))
                                new2026Decks.Add(cleanName);
                        }
                        else if (originalName.StartsWith("AI_"))
                        {
                            if (!legacyDecks.Contains(cleanName))
                                legacyDecks.Add(cleanName);
                        }
                        else
                        {
                            if (!otherDecks.Contains(cleanName))
                                otherDecks.Add(cleanName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogToConsole($"Failed to load decks: {ex.Message}");
            }

            // Fallback lists if folders are empty
            if (legacyDecks.Count == 0 && new2026Decks.Count == 0 && otherDecks.Count == 0)
            {
                legacyDecks.AddRange(new[] { "Altergeist", "BlueEyes", "SkyStriker" });
                new2026Decks.AddRange(new[] { "2026_K9" });
                otherDecks.AddRange(new[] { "Tour2025_Drytron", "Tour2024_Dogma" });
            }

            // Sort lists alphabetically
            legacyDecks.Sort(StringComparer.OrdinalIgnoreCase);
            new2026Decks.Sort(StringComparer.OrdinalIgnoreCase);
            otherDecks.Sort(StringComparer.OrdinalIgnoreCase);

            // Clear items
            CbDecksLegacy.Items.Clear();
            CbDecks2026.Items.Clear();
            CbDecksOther.Items.Clear();
            CbOpponentDecksLegacy.Items.Clear();
            CbOpponentDecks2026.Items.Clear();
            CbOpponentDecksOther.Items.Clear();

            // Populate ComboBoxes
            foreach (var deck in legacyDecks)
            {
                CbDecksLegacy.Items.Add(deck);
                CbOpponentDecksLegacy.Items.Add(deck);
            }
            foreach (var deck in new2026Decks)
            {
                CbDecks2026.Items.Add(deck);
                CbOpponentDecks2026.Items.Add(deck);
            }
            foreach (var deck in otherDecks)
            {
                CbDecksOther.Items.Add(deck);
                CbOpponentDecksOther.Items.Add(deck);
            }

            // Set default selections
            if (CbDecksLegacy.Items.Count > 0) CbDecksLegacy.SelectedIndex = 0;
            if (CbDecks2026.Items.Count > 0) CbDecks2026.SelectedIndex = 0;
            if (CbDecksOther.Items.Count > 0) CbDecksOther.SelectedIndex = 0;

            if (CbOpponentDecksLegacy.Items.Count > 0) CbOpponentDecksLegacy.SelectedIndex = 0;
            if (CbOpponentDecks2026.Items.Count > 0) CbOpponentDecks2026.SelectedIndex = 0;
            if (CbOpponentDecksOther.Items.Count > 0) CbOpponentDecksOther.SelectedIndex = 0;

            // Trigger checked states to enable/disable dropdowns
            RbCategory_Checked(null, null);
            RbOpponentCategory_Checked(null, null);
        }

        private string GetSelectedDeck(bool isOpponent)
        {
            if (isOpponent)
            {
                if (RbOpponentLegacy?.IsChecked == true)
                    return CbOpponentDecksLegacy.SelectedItem?.ToString() ?? "";
                if (RbOpponent2026?.IsChecked == true)
                    return CbOpponentDecks2026.SelectedItem?.ToString() ?? "";
                if (RbOpponentOther?.IsChecked == true)
                    return CbOpponentDecksOther.SelectedItem?.ToString() ?? "";
                return "";
            }
            else
            {
                if (RbLegacy?.IsChecked == true)
                    return CbDecksLegacy.SelectedItem?.ToString() ?? "";
                if (Rb2026?.IsChecked == true)
                    return CbDecks2026.SelectedItem?.ToString() ?? "";
                if (RbOther?.IsChecked == true)
                    return CbDecksOther.SelectedItem?.ToString() ?? "";
                return "";
            }
        }

        private void RbCategory_Checked(object? sender, RoutedEventArgs? e)
        {
            if (CbDecksLegacy == null || CbDecks2026 == null || CbDecksOther == null) return;

            CbDecksLegacy.IsEnabled = RbLegacy.IsChecked == true;
            CbDecks2026.IsEnabled = Rb2026.IsChecked == true;
            CbDecksOther.IsEnabled = RbOther.IsChecked == true;
        }

        private void RbOpponentCategory_Checked(object? sender, RoutedEventArgs? e)
        {
            if (CbOpponentDecksLegacy == null || CbOpponentDecks2026 == null || CbOpponentDecksOther == null) return;

            CbOpponentDecksLegacy.IsEnabled = RbOpponentLegacy.IsChecked == true;
            CbOpponentDecks2026.IsEnabled = RbOpponent2026.IsChecked == true;
            CbOpponentDecksOther.IsEnabled = RbOpponentOther.IsChecked == true;
        }

        private void CbDecksLegacy_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void CbDecks2026_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void CbDecksOther_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void CbOpponentDecksLegacy_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void CbOpponentDecks2026_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void CbOpponentDecksOther_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void LogToConsole(string message)
        {
            Dispatcher.Invoke(() =>
            {
                // Prevent freezing by trimming console logs if they exceed limit
                if (TxtConsole.Text.Length > 80000)
                {
                    TxtConsole.Text = TxtConsole.Text.Substring(40000);
                }
                TxtConsole.AppendText(message + "\n");
                TxtConsole.ScrollToEnd();
            });
        }

        private void ChkTwoBots_Click(object sender, RoutedEventArgs e)
        {
            if (OpponentDeckPanel != null)
            {
                OpponentDeckPanel.Visibility = (ChkTwoBots.IsChecked == true) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private async void BtnConnectAi_Click(object sender, RoutedEventArgs e)
        {
            string host = TxtHostIp.Text.Trim();
            string portStr = TxtHostPort.Text.Trim();

            string selectedDeck = GetSelectedDeck(isOpponent: false);
            if (string.IsNullOrEmpty(selectedDeck)) selectedDeck = "BlueEyes";

            string selectedOpponentDeck = GetSelectedDeck(isOpponent: true);
            if (string.IsNullOrEmpty(selectedOpponentDeck)) selectedOpponentDeck = "BlueEyes";

            bool spawnTwo = ChkTwoBots.IsChecked == true;

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(portStr))
            {
                MessageBox.Show("Please specify host IP and port.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(portStr, out int port))
            {
                MessageBox.Show("Invalid port number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LogToConsole($"\n---------------------------------------------------");
            LogToConsole($"Launching WindBot Bot (Bot A): {selectedDeck}");

            if (spawnTwo)
            {
                string botBName = (selectedDeck == selectedOpponentDeck) ? $"{selectedOpponentDeck}_Bot2" : selectedOpponentDeck;
                LogToConsole($"Launching WindBot Bot (Bot B): {botBName}");
            }
            LogToConsole($"Connecting to {host}:{port}...");
            LogToConsole($"---------------------------------------------------\n");

            BtnConnectAi.IsEnabled = false;
            TxtConsoleStatus.Text = "Running WindBot client...";

            await Task.Run(() =>
            {
                try
                {
                    var wrapper1 = new HeadlessClientWrapper(_windbotDllPath)
                    {
                        Name = selectedDeck,
                        Deck = selectedDeck,
                        Host = host,
                        Port = port
                    };

                    wrapper1.OnOutputReceived += (line) => LogToConsole($"[{selectedDeck}] {line}");
                    wrapper1.OnErrorReceived += (line) => LogToConsole($"[{selectedDeck} Warning] {line}");

                    wrapper1.Start();
                    LogToConsole($"Bot {selectedDeck} started successfully (PID: {wrapper1.ProcessId}).");

                    HeadlessClientWrapper? wrapper2 = null;
                    if (spawnTwo)
                    {
                        Thread.Sleep(1000); // Give the first bot a second to connect
                        string botBName = (selectedDeck == selectedOpponentDeck) ? $"{selectedOpponentDeck}_Bot2" : selectedOpponentDeck;
                        wrapper2 = new HeadlessClientWrapper(_windbotDllPath)
                        {
                            Name = botBName,
                            Deck = selectedOpponentDeck,
                            Host = host,
                            Port = port
                        };

                        wrapper2.OnOutputReceived += (line) => LogToConsole($"[{botBName}] {line}");
                        wrapper2.OnErrorReceived += (line) => LogToConsole($"[{botBName} Warning] {line}");

                        wrapper2.Start();
                        LogToConsole($"Bot {botBName} started successfully (PID: {wrapper2.ProcessId}).");
                    }

                    // Monitor the bot processes for output (max 3 minutes or until stopped)
                    int secondsElapsed = 0;
                    while ((wrapper1.IsRunning || (wrapper2 != null && wrapper2.IsRunning)) && secondsElapsed < 180)
                    {
                        Thread.Sleep(1000);
                        secondsElapsed++;
                    }

                    if (wrapper1.IsRunning)
                    {
                        LogToConsole($"Disconnecting / Stopping {wrapper1.Name} process...");
                        wrapper1.Stop();
                    }
                    if (wrapper2 != null && wrapper2.IsRunning)
                    {
                        LogToConsole($"Disconnecting / Stopping {wrapper2.Name} process...");
                        wrapper2.Stop();
                    }
                    LogToConsole("WindBot process stopped.\n");
                }
                catch (Exception ex)
                {
                    LogToConsole($"[Error Running Bot] {ex.Message}");
                }
            });

            BtnConnectAi.IsEnabled = true;
            TxtConsoleStatus.Text = "Ready.";
        }

        private void BtnClearConsole_Click(object sender, RoutedEventArgs e)
        {
            TxtConsole.Text = string.Empty;
        }
    }
}
