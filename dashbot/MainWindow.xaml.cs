using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using YgoAiPlatform.Core;

namespace dashbot
{
    public class DeckItem : INotifyPropertyChanged
    {
        public string FileName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Category { get; set; } = "Modern";
        public string CategoryTagText { get; set; } = "Modern";
        public string CategoryTagBg { get; set; } = "#D97706";

        private bool _isBot1Selected;
        public bool IsBot1Selected
        {
            get => _isBot1Selected;
            set
            {
                if (_isBot1Selected != value)
                {
                    _isBot1Selected = value;
                    NotifyVisualChanges();
                }
            }
        }

        private bool _isBot2Selected;
        public bool IsBot2Selected
        {
            get => _isBot2Selected;
            set
            {
                if (_isBot2Selected != value)
                {
                    _isBot2Selected = value;
                    NotifyVisualChanges();
                }
            }
        }

        public static bool IsBotVsBotActive { get; set; } = false;

        // Clean WinForm Light Styling
        public string CardBackground
        {
            get
            {
                if (IsBotVsBotActive)
                {
                    if (IsBot1Selected && IsBot2Selected) return "#F5F3FF";
                    if (IsBot1Selected) return "#F0F9FF";
                    if (IsBot2Selected) return "#ECFDF5";
                    return "#FFFFFF";
                }
                return IsBot1Selected ? "#F0F9FF" : "#FFFFFF";
            }
        }

        public string BorderBrushColor
        {
            get
            {
                if (IsBotVsBotActive)
                {
                    if (IsBot1Selected && IsBot2Selected) return "#7C3AED";
                    if (IsBot1Selected) return "#0284C7";
                    if (IsBot2Selected) return "#059669";
                    return "#CBD5E1";
                }
                return IsBot1Selected ? "#0284C7" : "#CBD5E1";
            }
        }

        public string BorderThicknessValue => IsBotVsBotActive ? ((IsBot1Selected || IsBot2Selected) ? "1.5" : "1") : (IsBot1Selected ? "1.5" : "1");

        public string TextColor => "#111111";

        public Visibility IndicatorVisibility
        {
            get
            {
                if (IsBotVsBotActive)
                    return (IsBot1Selected || IsBot2Selected) ? Visibility.Visible : Visibility.Collapsed;
                return IsBot1Selected ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public string IndicatorBackground
        {
            get
            {
                if (IsBotVsBotActive)
                {
                    if (IsBot1Selected && IsBot2Selected) return "#7C3AED";
                    if (IsBot1Selected) return "#0284C7";
                    if (IsBot2Selected) return "#059669";
                    return "#666666";
                }
                return "#0284C7";
            }
        }

        public string SelectionIndicator
        {
            get
            {
                if (IsBotVsBotActive)
                {
                    if (IsBot1Selected && IsBot2Selected) return "P1/P2";
                    if (IsBot1Selected) return "P1";
                    if (IsBot2Selected) return "P2";
                    return string.Empty;
                }
                return IsBot1Selected ? "P1" : string.Empty;
            }
        }

        public void NotifyVisualChanges()
        {
            OnPropertyChanged(nameof(IsBot1Selected));
            OnPropertyChanged(nameof(IsBot2Selected));
            OnPropertyChanged(nameof(CardBackground));
            OnPropertyChanged(nameof(BorderBrushColor));
            OnPropertyChanged(nameof(BorderThicknessValue));
            OnPropertyChanged(nameof(TextColor));
            OnPropertyChanged(nameof(IndicatorVisibility));
            OnPropertyChanged(nameof(IndicatorBackground));
            OnPropertyChanged(nameof(SelectionIndicator));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public partial class MainWindow : Window
    {
        private string _decksDir = string.Empty;
        private string _windbotDllPath = string.Empty;
        private readonly System.Windows.Threading.DispatcherTimer _timer;

        private readonly List<DeckItem> _allDecks = new();
        private readonly ObservableCollection<DeckItem> _filteredDecks = new();

        private DeckItem? _selectedBot1Deck;
        private DeckItem? _selectedBot2Deck;
        private bool _isAssigningBot2 = false;
        private string _currentCategory = "All";

        public MainWindow()
        {
            InitializeComponent();
            ResolvePaths();

            DeckItemsHost.ItemsSource = _filteredDecks;

            // Real-time clock updater
            _timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) => TxtTime.Text = DateTime.Now.ToString("HH:mm:ss");
            _timer.Start();

            TxtConsole.Text = "=== YGO Standalone Bot Launcher ===\n" +
                              "WindBot by IceYgo | Custom Deck By Jaynesiz\n" +
                              "Rule-based Bot Execution Engine\n" +
                              "---------------------------------------------------\n" +
                              $"Decks Directory: {_decksDir}\n" +
                              $"WindBot Engine: {_windbotDllPath}\n" +
                              "---------------------------------------------------\n" +
                              "Select a deck from the library on the left, then click 'Start & Connect Bot to Room'.\n" +
                              "For Bot vs Bot testing: Enable 'Spawn 2 Bots' and select decks for Bot 1 & Bot 2.\n\n";

            PopulateDeckLibrary();
        }

        private void ResolvePaths()
        {
            // 1. Try deployed path first (when running inside Game_EDOPro/)
            string deployedDecks = Path.Combine(AppContext.BaseDirectory, "WindBot", "Decks");
            string deployedDll = Path.Combine(AppContext.BaseDirectory, "WindBot", "WindBot.dll");

            if (Directory.Exists(deployedDecks) && File.Exists(deployedDll))
            {
                _decksDir = deployedDecks;
                _windbotDllPath = deployedDll;
                return;
            }

            // 2. Dev environment check (running from EdoGame root or YGO_SOURCE_CLEAN/)
            string? searchDir = AppContext.BaseDirectory;
            while (!string.IsNullOrEmpty(searchDir))
            {
                string cleanDecks = Path.Combine(searchDir, "src", "YGO_SOURCE_CLEAN", "windbot-fork", "Decks");
                if (Directory.Exists(cleanDecks))
                {
                    _decksDir = cleanDecks;
                    string[] candidates = new[]
                    {
                        Path.Combine(searchDir, "src", "YGO_SOURCE_CLEAN", "windbot-fork", "bin", "Release", "net10.0-publish", "WindBot.dll"),
                        Path.Combine(searchDir, "WindBot", "WindBot.dll"),
                        Path.Combine(searchDir, "src", "YGO_SOURCE_CLEAN", "windbot-fork", "bin", "x86", "Release", "net10.0", "WindBot.dll"),
                        Path.Combine(searchDir, "src", "YGO_SOURCE_CLEAN", "windbot-fork", "bin", "Release", "net10.0", "WindBot.dll")
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

                string directDecks = Path.Combine(searchDir, "windbot-fork", "Decks");
                if (Directory.Exists(directDecks))
                {
                    _decksDir = directDecks;
                    string[] candidates = new[]
                    {
                        Path.Combine(searchDir, "windbot-fork", "bin", "Release", "net10.0-publish", "WindBot.dll"),
                        Path.Combine(searchDir, "windbot-fork", "bin", "Release", "net10.0", "WindBot.dll"),
                        Path.Combine(searchDir, "windbot-fork", "bin", "x86", "Release", "net10.0", "WindBot.dll")
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

                searchDir = Path.GetDirectoryName(searchDir);
            }

            // Fallback
            _decksDir = Path.Combine(AppContext.BaseDirectory, "WindBot", "Decks");
            _windbotDllPath = Path.Combine(AppContext.BaseDirectory, "WindBot", "WindBot.dll");
        }

        private void PopulateDeckLibrary()
        {
            _allDecks.Clear();

            try
            {
                if (Directory.Exists(_decksDir))
                {
                    var files = Directory.GetFiles(_decksDir, "*.ydk");
                    foreach (var file in files)
                    {
                        string originalName = Path.GetFileNameWithoutExtension(file);
                        var item = ParseDeckItem(originalName);
                        if (!_allDecks.Any(d => d.DisplayName.Equals(item.DisplayName, StringComparison.OrdinalIgnoreCase)))
                        {
                            _allDecks.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogToConsole($"[Error loading decks] {ex.Message}");
            }

            // Fallbacks if directory is empty
            if (_allDecks.Count == 0)
            {
                _allDecks.Add(ParseDeckItem("2026_Branded"));
                _allDecks.Add(ParseDeckItem("Anime_JackAtlas"));
                _allDecks.Add(ParseDeckItem("AI_BlueEyes"));
                _allDecks.Add(ParseDeckItem("AI_DarkMagician"));
                _allDecks.Add(ParseDeckItem("AI_Altergeist"));
            }

            // Sort: Modern first, then Anime, then Legacy, then Special, sorted alphabetically
            _allDecks.Sort((a, b) =>
            {
                int catOrderA = GetCategoryWeight(a.Category);
                int catOrderB = GetCategoryWeight(b.Category);
                if (catOrderA != catOrderB) return catOrderA.CompareTo(catOrderB);
                return string.Compare(a.DisplayName, b.DisplayName, StringComparison.OrdinalIgnoreCase);
            });

            TxtDeckTotalCount.Text = $"({_allDecks.Count})";

            // Default selections
            _selectedBot1Deck = _allDecks.Find(d => d.FileName.Contains("Branded", StringComparison.OrdinalIgnoreCase))
                             ?? _allDecks[0];
            _selectedBot1Deck.IsBot1Selected = true;

            _selectedBot2Deck = _allDecks.Find(d => d.FileName.Contains("DarkMagician", StringComparison.OrdinalIgnoreCase))
                             ?? _allDecks.Find(d => d.FileName.Contains("BlueEyes", StringComparison.OrdinalIgnoreCase))
                             ?? _allDecks[0];
            _selectedBot2Deck.IsBot2Selected = true;

            UpdateMatchupUI();
            ApplyFilter();
        }

        private static int GetCategoryWeight(string cat)
        {
            return cat switch
            {
                "Modern" => 1,
                "Anime" => 2,
                "Legacy" => 3,
                "GOAT" => 4,
                "Special" => 5,
                _ => 6
            };
        }

        private static readonly HashSet<string> ModernArchetypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ADML", "AFS", "ArtMage",
            "Tenpai", "VoicelessVoice", "Centurion", "CenturIon",
            "Branded", "Purrely", "Yummy", "RyuGe", "Runick", "Spright", "WCParisKewlTune", "Kwtune", "KewlTune",
            "SnakeEye", "FireKing", "Tearla", "Tearlaments", "Kashtira", "Labrynth",
            "VanquishSoul", "Unchained", "RescueAce", "Horus", "Memento",
            "WhiteForest", "Fiendsmith", "Azamina", "Maliss", "Raika",
            "Yubel", "Chimera", "GoblinBiker", "GoldPride", "Mikanko",
            "SuperHeavySamurai", "Mathmech", "Cyberse", "Synchron", "Exosister",
            "SacrBeatsMach", "ScarbeatMach", "SacredBeats", "Machina",
            "PhantomKnight", "PhantomKnights"
        };

        private static DeckItem ParseDeckItem(string originalName)
        {
            string cleanName = originalName;
            string category;
            string tagText;
            string tagBg;

            if (originalName.StartsWith("2026_") || originalName.StartsWith("Expert_2026_") || originalName.StartsWith("Neural_2026_") || ModernArchetypes.Contains(cleanName))
            {
                category = "Modern";
                tagText = "Modern";
                tagBg = "#D97706"; // Amber / Gold
                if (cleanName.StartsWith("Expert_2026_")) cleanName = cleanName.Substring(12);
                else if (cleanName.StartsWith("Neural_2026_")) cleanName = cleanName.Substring(12);
                else if (cleanName.StartsWith("2026_")) cleanName = cleanName.Substring(5);
            }
            else if (originalName.StartsWith("Anime_"))
            {
                category = "Anime";
                tagText = "Anime";
                tagBg = "#BE185D"; // Deep Pink / Rose
                cleanName = cleanName.Substring(6);
            }
            else if (originalName.StartsWith("AI_"))
            {
                category = "Legacy";
                tagText = "Legacy";
                tagBg = "#1D4ED8"; // Royal Blue
                cleanName = cleanName.Substring(3);
            }
            else if (originalName.StartsWith("GOAT_"))
            {
                category = "GOAT";
                tagText = "GOAT";
                tagBg = "#047857"; // Emerald Green
                cleanName = cleanName.Substring(5);
            }
            else
            {
                category = "Special";
                tagText = "Special";
                tagBg = "#6D28D9"; // Purple
            }

            cleanName = CleanDeckDisplayName(cleanName);

            return new DeckItem
            {
                FileName = originalName,
                DisplayName = cleanName,
                Category = category,
                CategoryTagText = tagText,
                CategoryTagBg = tagBg
            };
        }

        private static string CleanDeckDisplayName(string name)
        {
            var overrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "AFS", "Azamina Fiendsmith Snake-Eye" },
                { "ArtMage", "Artmage" },
                { "Tenpai", "Tenpai Dragon" },
                { "VoicelessVoice", "Voiceless Voice" },
                { "Centurion", "Centur-Ion" },
                { "WhiteForest", "White Forest" },
                { "Sayer", "Sayer (Psychic)" },
                { "JackAtlas", "Jack Atlas" },
                { "Yugi", "Yugi Muto" },
                { "Yusei", "Yusei Fudo" },
                { "Kaiba", "Seto Kaiba" },
                { "Judai", "Jaden Yuki" },
                { "Zane", "Zane Truesdale" },
                { "Gong", "Gong Strong" },
                { "BlueEyes", "Blue-Eyes" },
                { "BlueEyesMaxDragon", "Blue-Eyes Max" },
                { "DarkMagician", "Dark Magician" },
                { "RedDragon", "Red Dragon Archfiend" },
                { "SkyStriker", "Sky Striker" },
                { "CyberDragon", "Cyber Dragon" },
                { "Blackwings", "Blackwing" },
                { "Blackwing", "Blackwing" },
                { "CrystronTrains", "Crystron Trains" },
                { "TrainCryston", "Train Crystron" },
                { "DarkWorld", "Dark World" },
                { "EvilTwin", "Evil Twin" },
                { "FairyTailPure", "Fairy Tail" },
                { "GemKnight", "Gem-Knight" },
                { "KaijuCrusadia", "Kaiju Crusadia" },
                { "Kwtune", "Kewl Tune" },
                { "RexRaptor", "Rex Raptor" },
                { "TrueDraco", "True Draco" },
                { "ChainBurn", "Chain Burn" },
                { "LightswornShaddoldinosour", "Lightsworn Shaddoll Dino" },
                { "ST1732", "Cyberse Link" },
                { "Demise", "Demise OTK" },
                { "Yubel2", "Yubel" },
                { "DogmaStun", "Dogmatika Stun" },
                { "BrElfnote", "Branded Elfnote" },
                { "EneaCraft", "Enea Craft" },
                { "EyeInside", "Eye Inside" },
                { "Goldlord", "Eldlich Goldlord" },
                { "AncientG", "Ancient Gear" },
                { "DarkTime", "Dark Time" },
                { "MagistusFairy", "Magistus Fairy" },
                { "BlazeBaz", "Blaze Baz" },
                { "PureWinds", "Pure Winds" },
                { "OldSchool", "Old School" },
                { "Timethief", "Time Thief" },
                { "ToadallyAwesome", "Toadally Awesome" },
                { "SacrBeatsMach", "Sacred Beasts Machina (FTK)" },
                { "ScarbeatMach", "Sacred Beasts Machina (FTK)" },
                { "SacredBeats", "Sacred Beasts" },
                { "PhantomKnight", "Phantom Knights" },
                { "PhantomKnights", "Phantom Knights" }
            };

            if (overrides.TryGetValue(name, out var customName))
            {
                return customName;
            }

            string spaced = Regex.Replace(name, "(?<=[a-z])([A-Z])", " $1");
            return spaced.Trim();
        }

        private void ApplyFilter()
        {
            string keyword = TxtSearch?.Text?.Trim() ?? string.Empty;

            _filteredDecks.Clear();
            foreach (var deck in _allDecks)
            {
                if (_currentCategory != "All" && !deck.Category.Equals(_currentCategory, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    bool matchDisplay = deck.DisplayName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool matchFile = deck.FileName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (!matchDisplay && !matchFile)
                    {
                        continue;
                    }
                }

                _filteredDecks.Add(deck);
            }
        }

        private void UpdateMatchupUI()
        {
            string bot1Name = _selectedBot1Deck?.DisplayName ?? "None";
            string bot2Name = _selectedBot2Deck?.DisplayName ?? "None";

            if (TxtSummaryBot1 != null) TxtSummaryBot1.Text = bot1Name;
            if (TxtSummaryBot2 != null) TxtSummaryBot2.Text = bot2Name;
            if (TxtMatchupBot1 != null) TxtMatchupBot1.Text = bot1Name;
            if (TxtMatchupBot2 != null) TxtMatchupBot2.Text = bot2Name;

            bool isBotVsBot = (RbModeDual?.IsChecked == true);
            if (BtnConnectAi != null)
            {
                if (isBotVsBot)
                {
                    BtnConnectAi.Content = $"Start & Connect Both ({bot1Name} vs {bot2Name})";
                }
                else
                {
                    BtnConnectAi.Content = $"Start & Connect Bot ({bot1Name}) to Room";
                }
            }

            UpdateBotSelectionHighlights();
        }

        private void UpdateBotSelectionHighlights()
        {
            if (CardBoxBot1 == null || CardBoxBot2 == null) return;

            bool isBotVsBot = (RbModeDual?.IsChecked == true);

            if (isBotVsBot && _isAssigningBot2)
            {
                CardBoxBot1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                CardBoxBot1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"));
                CardBoxBot1.BorderThickness = new Thickness(1);
                if (TxtIndicatorBot1 != null)
                {
                    TxtIndicatorBot1.Text = "CLICK TO SELECT";
                    TxtIndicatorBot1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#888888"));
                }

                CardBoxBot2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECFDF5"));
                CardBoxBot2.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669"));
                CardBoxBot2.BorderThickness = new Thickness(1.5);
                if (TxtIndicatorBot2 != null)
                {
                    TxtIndicatorBot2.Text = "● ACTIVE";
                    TxtIndicatorBot2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669"));
                }
            }
            else
            {
                CardBoxBot1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EFF6FF"));
                CardBoxBot1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0284C7"));
                CardBoxBot1.BorderThickness = new Thickness(1.5);
                if (TxtIndicatorBot1 != null)
                {
                    TxtIndicatorBot1.Text = "● ACTIVE";
                    TxtIndicatorBot1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0284C7"));
                }

                CardBoxBot2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                CardBoxBot2.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"));
                CardBoxBot2.BorderThickness = new Thickness(1);
                if (TxtIndicatorBot2 != null)
                {
                    TxtIndicatorBot2.Text = "CLICK TO SELECT";
                    TxtIndicatorBot2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#888888"));
                }
            }
        }

        private void DeckCard_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement elem && elem.DataContext is DeckItem clickedDeck)
            {
                bool isBotVsBot = (RbModeDual?.IsChecked == true);
                if (isBotVsBot && _isAssigningBot2)
                {
                    if (_selectedBot2Deck != null) _selectedBot2Deck.IsBot2Selected = false;
                    _selectedBot2Deck = clickedDeck;
                    _selectedBot2Deck.IsBot2Selected = true;
                    LogToConsole($"Assigned Bot 2 (P2): {clickedDeck.DisplayName}");
                }
                else
                {
                    if (_selectedBot1Deck != null) _selectedBot1Deck.IsBot1Selected = false;
                    _selectedBot1Deck = clickedDeck;
                    _selectedBot1Deck.IsBot1Selected = true;
                    LogToConsole(isBotVsBot ? $"Assigned Bot 1 (P1): {clickedDeck.DisplayName}" : $"Selected Bot: {clickedDeck.DisplayName}");
                }

                UpdateMatchupUI();
            }
        }

        private void DeckCard_RightClick(object sender, MouseButtonEventArgs e)
        {
            bool isBotVsBot = (RbModeDual?.IsChecked == true);
            if (!isBotVsBot)
            {
                DeckCard_Click(sender, e);
                return;
            }

            if (sender is FrameworkElement elem && elem.DataContext is DeckItem clickedDeck)
            {
                if (_selectedBot2Deck != null) _selectedBot2Deck.IsBot2Selected = false;
                _selectedBot2Deck = clickedDeck;
                _selectedBot2Deck.IsBot2Selected = true;

                LogToConsole($"[Right-Click] Assigned Bot 2 (P2): {clickedDeck.DisplayName}");
                UpdateMatchupUI();
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtSearchPlaceholder != null)
            {
                TxtSearchPlaceholder.Visibility = string.IsNullOrEmpty(TxtSearch.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
            ApplyFilter();
        }

        private void RbCat_Checked(object sender, RoutedEventArgs e)
        {
            if (RbCatAll?.IsChecked == true) _currentCategory = "All";
            else if (RbCatModern?.IsChecked == true) _currentCategory = "Modern";
            else if (RbCatAnime?.IsChecked == true) _currentCategory = "Anime";
            else if (RbCatLegacy?.IsChecked == true) _currentCategory = "Legacy";
            else if (RbCatGoat?.IsChecked == true) _currentCategory = "GOAT";
            else if (RbCatSpecial?.IsChecked == true) _currentCategory = "Special";

            ApplyFilter();
        }

        private void RbAssignBot_Checked(object sender, RoutedEventArgs e)
        {
            _isAssigningBot2 = (RbAssignBot2?.IsChecked == true);
            UpdateBotSelectionHighlights();
        }

        private void RbDuelMode_Checked(object sender, RoutedEventArgs e)
        {
            bool isBotVsBot = (RbModeDual?.IsChecked == true);
            DeckItem.IsBotVsBotActive = isBotVsBot;

            // Update P2 controls visibility
            if (RbAssignBot2 != null)
                RbAssignBot2.Visibility = isBotVsBot ? Visibility.Visible : Visibility.Collapsed;
            if (VsCircle != null)
                VsCircle.Visibility = isBotVsBot ? Visibility.Visible : Visibility.Collapsed;
            if (CardBoxBot2 != null)
                CardBoxBot2.Visibility = isBotVsBot ? Visibility.Visible : Visibility.Collapsed;
            if (SummaryBot2Panel != null)
                SummaryBot2Panel.Visibility = isBotVsBot ? Visibility.Visible : Visibility.Collapsed;

            if (TxtSummaryBot1Label != null)
                TxtSummaryBot1Label.Text = isBotVsBot ? "Bot 1 (P1):" : "Selected Bot:";

            if (TxtCardHeaderBot1 != null)
                TxtCardHeaderBot1.Text = isBotVsBot ? "BOT 1 (P1)" : "SELECTED BOT";

            if (TxtDeckSelectHint != null)
                TxtDeckSelectHint.Text = isBotVsBot ? "Click deck to assign active | Right-click for P2" : "Click deck to select bot";

            if (RbAssignBot1 != null)
                RbAssignBot1.Content = isBotVsBot ? "P1 (Bot 1)" : "P1 (Bot)";

            // If switching back to single mode, force assign to P1
            if (!isBotVsBot)
            {
                _isAssigningBot2 = false;
                if (RbAssignBot1 != null) RbAssignBot1.IsChecked = true;
            }

            // Refresh all deck cards visually
            foreach (var deck in _allDecks)
            {
                deck.NotifyVisualChanges();
            }

            UpdateMatchupUI();
        }

        private void CardBoxBot1_Click(object sender, MouseButtonEventArgs e)
        {
            _isAssigningBot2 = false;
            if (RbAssignBot1 != null) RbAssignBot1.IsChecked = true;
            UpdateBotSelectionHighlights();
            LogToConsole("Active deck selection target: P1");
        }

        private void CardBoxBot2_Click(object sender, MouseButtonEventArgs e)
        {
            bool isBotVsBot = (RbModeDual?.IsChecked == true);
            if (!isBotVsBot) return;

            _isAssigningBot2 = true;
            if (RbAssignBot2 != null) RbAssignBot2.IsChecked = true;
            UpdateBotSelectionHighlights();
            LogToConsole("Active deck selection target: P2 (Bot 2)");
        }

        private void LogToConsole(string message)
        {
            Dispatcher.Invoke(() =>
            {
                if (TxtConsole.Text.Length > 80000)
                {
                    TxtConsole.Text = TxtConsole.Text.Substring(40000);
                }
                TxtConsole.AppendText(message + "\n");
                TxtConsole.ScrollToEnd();
            });
        }

        private void BtnClearConsole_Click(object sender, RoutedEventArgs e)
        {
            TxtConsole.Text = string.Empty;
        }

        private void BtnCopyConsole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(TxtConsole.Text);
                TxtConsoleStatus.Text = "Logs copied to clipboard.";
            }
            catch
            {
                TxtConsoleStatus.Text = "Copy failed.";
            }
        }

        private void BtnOpenLogs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string baseDir = AppContext.BaseDirectory;
                string logDir = Path.Combine(baseDir, "WindBot", "logs");
                if (!Directory.Exists(logDir))
                {
                    logDir = Path.Combine(baseDir, "logs");
                }
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = logDir,
                    UseShellExecute = true,
                    Verb = "open"
                });
                TxtConsoleStatus.Text = "Logs folder opened.";
            }
            catch (Exception ex)
            {
                TxtConsoleStatus.Text = $"Open logs error: {ex.Message}";
            }
        }

        private void ChkDevMode_Click(object sender, RoutedEventArgs e)
        {
            bool isDev = (ChkDevMode?.IsChecked == true);
            if (isDev)
            {
                LogToConsole("[โหมดนักพัฒนา] เปิดใช้งาน: แสดง Trace ละเอียด และบันทึก Logs ลงไฟล์");
                TxtConsoleStatus.Text = "Dev Mode: ON (Logging)";
            }
            else
            {
                LogToConsole("[โหมดนักพัฒนา] ปิดใช้งาน: ซ่อน Trace ละเอียด และระงับการบันทึกไฟล์ Logs");
                TxtConsoleStatus.Text = "Dev Mode: OFF (Clean)";
            }
        }

        private static bool ShouldShowInCleanMode(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return false;
            // Suppress verbose debug and decision trace internals
            if (line.Contains("[DEBUG]") || line.Contains("[TRACE]") || line.Contains("Candidate card") || line.Contains("Score:"))
                return false;
            return true;
        }

        private async void BtnConnectAi_Click(object sender, RoutedEventArgs e)
        {
            string host = TxtHostIp.Text.Trim();
            string portStr = TxtHostPort.Text.Trim();

            string bot1FileName = _selectedBot1Deck?.FileName ?? "AI_BlueEyes";
            string bot1DisplayName = _selectedBot1Deck?.DisplayName ?? "Blue-Eyes";

            string bot2FileName = _selectedBot2Deck?.FileName ?? "AI_DarkMagician";
            string bot2DisplayName = _selectedBot2Deck?.DisplayName ?? "Dark Magician";

            bool isBotVsBot = (RbModeDual?.IsChecked == true);
            bool isDevMode = (ChkDevMode?.IsChecked == true);

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(portStr))
            {
                MessageBox.Show("Please specify target host IP and port.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(portStr, out int port))
            {
                MessageBox.Show("Invalid port number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LogToConsole($"\n---------------------------------------------------");
            if (isBotVsBot)
            {
                string botBName = (bot1FileName == bot2FileName) ? $"{bot2DisplayName} (P2)" : bot2DisplayName;
                LogToConsole($"Spawning Matchup: Bot vs Bot");
                LogToConsole($"Bot 1 (P1): {bot1DisplayName} [{bot1FileName}]");
                LogToConsole($"Bot 2 (P2): {botBName} [{bot2FileName}]");
            }
            else
            {
                LogToConsole($"Spawning WindBot: {bot1DisplayName} [{bot1FileName}]");
            }
            LogToConsole($"Mode: {(isDevMode ? "โหมดนักพัฒนา (บันทึก Logs)" : "โหมดปกติ (ไม่บันทึก Logs)")}");
            LogToConsole($"Connecting to {host}:{port}...");
            LogToConsole($"---------------------------------------------------\n");

            BtnConnectAi.IsEnabled = false;
            TxtConsoleStatus.Text = "Running WindBot client...";

            await Task.Run(() =>
            {
                try
                {
                    HeadlessClientWrapper? wrapper1 = null;
                    HeadlessClientWrapper? wrapper2 = null;

                    wrapper1 = new HeadlessClientWrapper(_windbotDllPath)
                    {
                        Name = bot1DisplayName,
                        Deck = bot1FileName,
                        Host = host,
                        Port = port,
                        EnableFileLog = isDevMode
                    };

                    wrapper1.OnOutputReceived += (line) =>
                    {
                        if (isDevMode || ShouldShowInCleanMode(line))
                        {
                            LogToConsole($"[{bot1DisplayName}] {line}");
                        }
                    };
                    wrapper1.OnErrorReceived += (line) => LogToConsole($"[{bot1DisplayName} Warning] {line}");

                    wrapper1.Start();
                    LogToConsole($"Bot {bot1DisplayName} started successfully (PID: {wrapper1.ProcessId}).");

                    if (isBotVsBot)
                    {
                        Thread.Sleep(1000);
                        string botBName = (bot1FileName == bot2FileName) ? $"{bot2DisplayName}_P2" : bot2DisplayName;
                        wrapper2 = new HeadlessClientWrapper(_windbotDllPath)
                        {
                            Name = botBName,
                            Deck = bot2FileName,
                            Host = host,
                            Port = port,
                            EnableFileLog = isDevMode
                        };

                        wrapper2.OnOutputReceived += (line) =>
                        {
                            if (isDevMode || ShouldShowInCleanMode(line))
                            {
                                LogToConsole($"[{botBName}] {line}");
                            }
                        };
                        wrapper2.OnErrorReceived += (line) => LogToConsole($"[{botBName} Warning] {line}");

                        wrapper2.Start();
                        LogToConsole($"P2 Bot {botBName} started successfully (PID: {wrapper2.ProcessId}).");
                    }

                    int secondsElapsed = 0;
                    while (((wrapper1 != null && wrapper1.IsRunning) || (wrapper2 != null && wrapper2.IsRunning)) && secondsElapsed < 180)
                    {
                        Thread.Sleep(1000);
                        secondsElapsed++;
                    }

                    if (wrapper1 != null && wrapper1.IsRunning)
                    {
                        LogToConsole($"Disconnecting {wrapper1.Name}...");
                        wrapper1.Stop();
                    }
                    if (wrapper2 != null && wrapper2.IsRunning)
                    {
                        LogToConsole($"Disconnecting {wrapper2.Name}...");
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
            TxtConsoleStatus.Text = isDevMode ? "Ready (Dev Mode)." : "Ready.";
        }
    }
}
