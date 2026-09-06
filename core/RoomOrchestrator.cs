using System;
using System.IO;
using System.Text.Json;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NetMQ;
using NetMQ.Sockets;

namespace YgoAiPlatform.Core
{
    public class RoomOrchestrator : IDisposable
    {
        private readonly string _projectRootDir;
        private readonly string _logsRootDir;
        private readonly ConcurrentDictionary<string, RoomHandle> _rooms = new();
        private readonly ConcurrentDictionary<string, List<string>> _roomLogs = new();
        private readonly CancellationTokenSource _cts = new();
        private Task? _watchdogTask;
        private volatile bool _isRunning = false;

        // CB-002: กำหนด upper bound เพื่อป้องกัน infinite loop กรณีพอร์ตเต็ม
        public const int MaxPortSearchRange = 200;

        public RoomOrchestrator(string projectRootDir)
        {
            _projectRootDir = Path.GetFullPath(projectRootDir);
            _logsRootDir = Path.Combine(_projectRootDir, "logs");
        }

        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;
            _watchdogTask = Task.Run(() => RunWatchdogAsync(_cts.Token));
            Console.WriteLine("[RoomOrchestrator] Watchdog monitor started.");
        }

        public RoomHandle Spawn(string deckA, string deckB, int? seed = null)
        {
            int reqPort = FindAvailablePort(7900);
            int pubPort = FindAvailablePort(8900);
            string duelId = Guid.NewGuid().ToString("N").Substring(0, 8);
            int finalSeed = seed ?? new Random().Next(100000, 999999);

            var room = new RoomHandle
            {
                DuelId = duelId,
                Port = reqPort,
                PubPort = pubPort,
                Seed = finalSeed,
                DeckA = deckA,
                DeckB = deckB,
                Status = RoomStatus.Starting,
                RestartCount = 0
            };

            _rooms[duelId] = room;
            
            // เปิดโปรเซสย่อย
            StartRoomProcess(room);

            return room;
        }

        private void StartRoomProcess(RoomHandle room)
        {
            // ค้นหาตำแหน่งไฟล์ launcher.dll
            string launcherDllPath = Path.Combine(AppContext.BaseDirectory, "launcher.dll");
            
            if (!File.Exists(launcherDllPath))
            {
                var candidatePaths = new[]
                {
                    Path.Combine(_projectRootDir, "YGO_AI_PLATFORM", "launcher", "bin", "Release", "net10.0", "launcher.dll"),
                    Path.Combine(_projectRootDir, "YGO_AI_PLATFORM", "launcher", "bin", "Debug", "net10.0", "launcher.dll"),
                    Path.Combine(_projectRootDir, "launcher", "bin", "Release", "net10.0", "launcher.dll"),
                    Path.Combine(_projectRootDir, "launcher", "bin", "Debug", "net10.0", "launcher.dll")
                };

                foreach (var path in candidatePaths)
                {
                    if (File.Exists(path))
                    {
                        launcherDllPath = path;
                        break;
                    }
                }
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"\"{launcherDllPath}\" --worker-mode --port {room.Port} --pub-port {room.PubPort} --duel-id {room.DuelId} --seed {room.Seed} --decka \"{room.DeckA}\" --deckb \"{room.DeckB}\"",
                // CB-FIX: Set WorkingDirectory ไปยังโฟลเดอร์ของ launcher.dll
                // ถ้าไม่ตั้งค่า process ลูกจะ inherit CWD ของ parent ซึ่งทำให้ relative path เสีย
                WorkingDirectory = Path.GetDirectoryName(launcherDllPath) ?? _projectRootDir,
                UseShellExecute = false,
                CreateNoWindow = true, // รันเบื้องหลัง
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = new Process { StartInfo = startInfo };
            
            var outputList = new List<string>();
            _roomLogs[room.DuelId] = outputList;
            var outputLock = new object();

            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null)
                {
                    lock (outputLock)
                    {
                        outputList.Add(e.Data);
                        if (outputList.Count > 200) outputList.RemoveAt(0);
                    }
                }
            };
            process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null)
                {
                    lock (outputLock)
                    {
                        outputList.Add("[ERR] " + e.Data);
                        if (outputList.Count > 200) outputList.RemoveAt(0);
                    }
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            room.ProcessId = process.Id;
            room.Status = RoomStatus.Running;

            Console.WriteLine($"[RoomOrchestrator] Spawned Worker Process for {room.DuelId} (PID: {process.Id}) on REP port {room.Port} | PUB port {room.PubPort}");
            LogIpcMessage(room.Port, $"SPAWN_PROCESS", $"PID: {process.Id}, Args: {startInfo.Arguments}");
        }

        private async Task RunWatchdogAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(5000, token); // วนตรวจสอบสุขภาพของโปรเซสทุก 5 วินาที
                }
                catch (TaskCanceledException)
                {
                    break;
                }

                foreach (var room in _rooms.Values)
                {
                    if (room.Status == RoomStatus.Terminated) continue;

                    bool isAlive = false;
                    try
                    {
                        var process = Process.GetProcessById(room.ProcessId);
                        if (!process.HasExited)
                        {
                            isAlive = true;
                        }
                    }
                    catch
                    {
                        // หากมี Exception แปลว่าโปรเซสสิ้นสุดลงแล้วหรือหาไม่พบ
                    }

                    if (!isAlive)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[RoomOrchestrator] [Watchdog] Detected Crashed Process for {room.DuelId} (PID: {room.ProcessId}) on port {room.Port}!");
                        Console.ResetColor();

                        room.Status = RoomStatus.Crashed;
                        room.RestartCount++;

                        WriteWorkerCrashLog(room);

                        // Enforce max restart limit to prevent infinite restart loops
                        if (room.RestartCount > RoomHandle.MaxRestartCount)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[RoomOrchestrator] [Watchdog] Room {room.DuelId} exceeded max restart limit ({RoomHandle.MaxRestartCount}). Marking as Terminated.");
                            Console.ResetColor();
                            room.Status = RoomStatus.Terminated;
                            LogIpcMessage(room.Port, "WATCHDOG_MAX_RESTART", $"Room {room.DuelId} terminated after {room.RestartCount} restarts.");
                            _roomLogs.TryRemove(room.DuelId, out _);
                            continue;
                        }

                        LogIpcMessage(room.Port, "WATCHDOG_DETECT_CRASH", $"PID {room.ProcessId} exited. Initiating Auto-restart.");

                        // ทำการรีสตาร์ทบอทด้วยข้อมูลเดิม
                        try
                        {
                            StartRoomProcess(room);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[RoomOrchestrator Error] Failed to restart room {room.DuelId}: {ex.Message}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ส่งคำสั่ง IPC ไปยัง Socket REQ/REP ปลายทาง
        /// </summary>
        public string SendCommand(int port, string command)
        {
            try
            {
                using (var socket = new RequestSocket())
                {
                    // ตั้ง timeout เพื่อป้องกันการบล็อกค้างในกรณีโปรเซสล่ม
                    socket.Connect($"tcp://127.0.0.1:{port}");
                    
                    LogIpcMessage(port, $"SEND_COMMAND", command);
                    
                    socket.SendFrame(command);
                    
                    if (socket.TryReceiveFrameString(TimeSpan.FromSeconds(3), out string? response))
                    {
                        LogIpcMessage(port, $"RECV_RESPONSE", response ?? "Null");
                        return response ?? "Empty Response";
                    }
                    else
                    {
                        LogIpcMessage(port, $"RECV_TIMEOUT", "No response in 3s");
                        return "Timeout";
                    }
                }
            }
            catch (Exception ex)
            {
                LogIpcMessage(port, $"IPC_ERROR", ex.Message);
                return $"Error: {ex.Message}";
            }
        }

        public List<RoomHandle> ListRooms()
        {
            return new List<RoomHandle>(_rooms.Values);
        }

        private void LogIpcMessage(int port, string action, string details)
        {
            try
            {
                string ipcLogDir = Path.Combine(_logsRootDir, "ipc");
                if (!Directory.Exists(ipcLogDir))
                {
                    Directory.CreateDirectory(ipcLogDir);
                }

                string today = DateTime.UtcNow.ToString("yyyyMMdd");
                string logFile = Path.Combine(ipcLogDir, $"{today}_{port}.log");
                
                string logLine = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] [{action,-20}] {details}{Environment.NewLine}";
                
                File.AppendAllText(logFile, logLine);
            }
            catch {}
        }

        // CB-002: Private method ที่มี upper bound — ไม่ infinite loop อีกต่อไป
        private int FindAvailablePort(int startPort)
            => FindAvailablePortPublic(startPort, MaxPortSearchRange);

        /// <summary>
        /// ค้นหาพอร์ตว่างถัดไปจาก startPort — expose เป็น public เพื่อ testability
        /// </summary>
        /// <exception cref="InvalidOperationException">เมื่อไม่พบพอร์ตว่างใน range ที่กำหนด</exception>
        public int FindAvailablePortPublic(int startPort, int maxAttempts = MaxPortSearchRange)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                int candidate = startPort + i;
                // ตรวจสอบ valid port range (TCP: 1-65535)
                if (candidate > 65535) break;
                if (IsPortAvailable(candidate)) return candidate;
            }
            throw new InvalidOperationException(
                $"No available port found in range [{startPort}, {startPort + maxAttempts}). " +
                $"All {maxAttempts} ports are in use. Consider reducing concurrent rooms.");
        }

        private bool IsPortAvailable(int port)
        {
            try
            {
                using (var tcp = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, port))
                {
                    tcp.Start();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void WriteWorkerCrashLog(RoomHandle room)
        {
            try
            {
                string errorLogDir = Path.Combine(_logsRootDir, "error");
                Directory.CreateDirectory(errorLogDir);
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string logFilePath = Path.Combine(errorLogDir, $"worker_crash_{room.DuelId}_{timestamp}.log");

                var sb = new System.Text.StringBuilder();
                sb.AppendLine("==================================================================");
                sb.AppendLine($"  WORKER PROCESS CRASH LOG - {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                sb.AppendLine("==================================================================");
                sb.AppendLine($"Duel ID:       {room.DuelId}");
                sb.AppendLine($"Port:          {room.Port}");
                sb.AppendLine($"PubPort:       {room.PubPort}");
                sb.AppendLine($"Seed:          {room.Seed}");
                sb.AppendLine($"Deck A:        {room.DeckA}");
                sb.AppendLine($"Deck B:        {room.DeckB}");
                sb.AppendLine($"PID:           {room.ProcessId}");
                sb.AppendLine($"Restart Count: {room.RestartCount}");
                sb.AppendLine();
                sb.AppendLine("------------------------------------------------------------------");
                sb.AppendLine("  WORKER PROCESS OUTPUT (LAST 200 LINES)");
                sb.AppendLine("------------------------------------------------------------------");
                
                if (_roomLogs.TryGetValue(room.DuelId, out var lines))
                {
                    lock (lines)
                    {
                        foreach (var line in lines)
                        {
                            sb.AppendLine(line);
                        }
                    }
                }
                else
                {
                    sb.AppendLine("[No output captured]");
                }
                sb.AppendLine("==================================================================");

                File.WriteAllText(logFilePath, sb.ToString(), System.Text.Encoding.UTF8);
                Console.WriteLine($"[RoomOrchestrator] Crash log written to: {logFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RoomOrchestrator Error] Failed to write crash log for room {room.DuelId}: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            try
            {
                _watchdogTask?.Wait(2000);
            }
            catch {}

            // ปิดโปรเซสที่ค้างทั้งหมด
            foreach (var room in _rooms.Values)
            {
                if (room.Status != RoomStatus.Terminated)
                {
                    // ส่งสัญญาณ terminate ก่อน
                    SendCommand(room.Port, "terminate");
                    
                    try
                    {
                        var process = Process.GetProcessById(room.ProcessId);
                        if (!process.HasExited)
                        {
                            process.Kill(entireProcessTree: true);
                        }
                    }
                    catch {}
                }
            }
            
            _roomLogs.Clear();
            _cts.Dispose();
            Console.WriteLine("[RoomOrchestrator] Disposed all rooms and watchdog.");
        }
    }
}
