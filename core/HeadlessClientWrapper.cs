using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YgoAiPlatform.Core
{
    public class HeadlessClientWrapper : IDisposable
    {
        private readonly string _projectRootDir;
        private string? _windbotDllPath;
        private Process? _process;
        private readonly object _lock = new();
        private StreamWriter? _logWriter;

        public string Name { get; set; } = "WindBot";
        public string Deck { get; set; } = "BlueEyes";
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 7911;
        public int? Hand { get; set; }
        public int? Version { get; set; }
        public string? Dialog { get; set; }
        public bool DebugMode { get; set; } = false;
        public bool Chat { get; set; } = false;
        public string? ReplayPath { get; set; }
        public string? DuelId { get; set; }

        public bool IsRunning
        {
            get
            {
                if (_process == null) return false;
                try { _process.Refresh(); } catch {}
                return !_process.HasExited;
            }
        }
        public int? ProcessId => _process?.Id;
        public int? ExitCode => _process?.HasExited == true ? _process.ExitCode : null;

        public event Action<string>? OnOutputReceived;
        public event Action<string>? OnErrorReceived;
        public event Action<int>? OnProcessExited;

        public HeadlessClientWrapper(string path)
        {
            if (File.Exists(path) || path.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                _windbotDllPath = Path.GetFullPath(path);
                string? dir = Path.GetDirectoryName(_windbotDllPath);
                _projectRootDir = dir != null ? WindBotResolver.ResolveProjectRoot(dir) : AppContext.BaseDirectory;
            }
            else
            {
                _projectRootDir = Path.GetFullPath(path);
            }
        }

        public void Start()
        {
            lock (_lock)
            {
                if (IsRunning)
                {
                    throw new InvalidOperationException("WindBot client is already running.");
                }

                string windbotDllPath = _windbotDllPath ?? WindBotResolver.ResolveWindBotDll(_projectRootDir);

                if (!File.Exists(windbotDllPath))
                {
                    throw new FileNotFoundException($"Could not find WindBot.dll at: {windbotDllPath}. Please run the build script first.");
                }

                // Initialize client log file
                string headlessLogDir = Path.Combine(_projectRootDir, "logs", "headless");
                Directory.CreateDirectory(headlessLogDir);
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string logPath = Path.Combine(headlessLogDir, $"client_{CleanFileName(Name)}_{timestamp}_{Port}.log");
                try
                {
                    _logWriter = new StreamWriter(logPath, false, Encoding.UTF8);
                    _logWriter.WriteLine($"=== WindBot Headless Client Start: {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
                    _logWriter.WriteLine($"Deck: {Deck} | Host: {Host}:{Port}");
                    _logWriter.WriteLine("==================================================");
                    _logWriter.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Warning] Failed to initialize headless client log file: {ex.Message}");
                }

                var argsBuilder = new StringBuilder();
                argsBuilder.Append($"\"{windbotDllPath}\"");
                // CB-FIX: Quote all string arguments to handle spaces in names/paths
                argsBuilder.Append($" Name=\"{Name}\"");
                argsBuilder.Append($" Deck=\"{Deck}\"");
                argsBuilder.Append($" Host=\"{Host}\"");
                argsBuilder.Append($" Port={Port}");
                argsBuilder.Append($" Chat={(Chat ? "true" : "false")}");
                argsBuilder.Append($" Debug={(DebugMode ? "true" : "false")}");

                if (Hand.HasValue)
                {
                    argsBuilder.Append($" Hand={Hand.Value}");
                }
                if (Version.HasValue)
                {
                    argsBuilder.Append($" Version={Version.Value}");
                }
                if (!string.IsNullOrEmpty(Dialog))
                {
                    argsBuilder.Append($" Dialog=\"{Dialog}\"");
                }
                if (!string.IsNullOrEmpty(ReplayPath))
                {
                    argsBuilder.Append($" ReplayPath=\"{ReplayPath}\"");
                }
                if (!string.IsNullOrEmpty(DuelId))
                {
                    argsBuilder.Append($" DuelId=\"{DuelId}\"");
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = argsBuilder.ToString(),
                    WorkingDirectory = Path.GetDirectoryName(windbotDllPath),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                _process = new Process { StartInfo = startInfo };
                _process.EnableRaisingEvents = true;

                _process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        OnOutputReceived?.Invoke(e.Data);
                        lock (_lock)
                        {
                            try
                            {
                                _logWriter?.WriteLine($"[{DateTime.Now:HH:mm:ss}] {e.Data}");
                                _logWriter?.Flush();
                            }
                            catch {}
                        }
                    }
                };

                _process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        OnErrorReceived?.Invoke(e.Data);
                        lock (_lock)
                        {
                            try
                            {
                                _logWriter?.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERR] {e.Data}");
                                _logWriter?.Flush();
                            }
                            catch {}
                        }
                    }
                };

                _process.Exited += (sender, e) =>
                {
                    int code = _process?.ExitCode ?? -1;
                    OnProcessExited?.Invoke(code);
                    CloseLogWriter(code);
                };

                _process.Start();
                _process.BeginOutputReadLine();
                _process.BeginErrorReadLine();
            }
        }

        public void Stop()
        {
            Process? procToKill = null;
            lock (_lock)
            {
                if (_process == null) return;
                procToKill = _process;
                _process = null;
            }

            if (procToKill != null)
            {
                try
                {
                    procToKill.EnableRaisingEvents = false;
                    if (!procToKill.HasExited)
                    {
                        procToKill.Kill();
                        try { procToKill.WaitForExit(3000); } catch {}
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Warning] Failed to stop WindBot process gracefully: {ex.Message}");
                }
                finally
                {
                    CloseLogWriter(-99);
                    try { procToKill.Dispose(); } catch {}
                }
            }
        }

        public void Dispose()
        {
            Stop();
        }

        private void CloseLogWriter(int exitCode)
        {
            lock (_lock)
            {
                if (_logWriter != null)
                {
                    try
                    {
                        _logWriter.WriteLine($"=== WindBot Headless Client Stopped: {DateTime.Now:yyyy-MM-dd HH:mm:ss} (Exit Code: {exitCode}) ===");
                        _logWriter.Dispose();
                    }
                    catch {}
                    _logWriter = null;
                }
            }
        }

        private static string CleanFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "Unknown";
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name.Replace(" ", "_");
        }
    }
}
