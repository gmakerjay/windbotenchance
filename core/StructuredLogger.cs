using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YgoAiPlatform.Core
{
    public enum LogCategory
    {
        Duel,
        Ai,
        Error,
        Training,
        Replay
    }

    public enum VerbosityLevel
    {
        Minimal,
        Normal,
        Verbose,
        Developer
    }

    public class StructuredLogger : IAsyncDisposable
    {
        private readonly string _logsRootDir;
        private readonly Channel<LogPayload> _channel;
        private readonly Task _writeTask;
        private readonly CancellationTokenSource _cts;
        
        private long _bytesWritten = 0;
        private int _queueSize = 0;
        private readonly ConcurrentDictionary<string, string> _activeFiles = new();
        private readonly ConcurrentDictionary<string, long> _fileSizes = new();
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10MB

        public StructuredLogger(string logsRootDir)
        {
            _logsRootDir = Path.GetFullPath(logsRootDir);
            
            // CB-FIX: ใช้ Bounded channel เพื่อป้องกัน OOM ภายใต้ high-throughput training
            // DropOldest เหมาะสำหรับ logging — เราต้องการ event ล่าสุดมากกว่า event เก่า
            _channel = Channel.CreateBounded<LogPayload>(new BoundedChannelOptions(10000)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                AllowSynchronousContinuations = false
            });

            _cts = new CancellationTokenSource();
            _writeTask = Task.Run(ProcessQueueAsync);
        }

        public void Log(LogCategory category, string duelId, string deckA, string deckB, LogEntry entry, VerbosityLevel systemVerbosity = VerbosityLevel.Verbose)
        {
            if (!ShouldLog(entry, systemVerbosity))
            {
                return;
            }

            Interlocked.Increment(ref _queueSize);
            _channel.Writer.TryWrite(new LogPayload
            {
                Category = category,
                DuelId = duelId,
                DeckA = deckA,
                DeckB = deckB,
                Entry = entry
            });
        }

        private bool ShouldLog(LogEntry entry, VerbosityLevel verbosity)
        {
            switch (verbosity)
            {
                case VerbosityLevel.Minimal:
                    return entry.Action.Equals("duel_end", StringComparison.OrdinalIgnoreCase) || 
                           entry.Action.Equals("win", StringComparison.OrdinalIgnoreCase) ||
                           entry.Action.Equals("loss", StringComparison.OrdinalIgnoreCase);

                case VerbosityLevel.Normal:
                    return !entry.Action.Equals("chain_position", StringComparison.OrdinalIgnoreCase);

                case VerbosityLevel.Verbose:
                    return true;

                case VerbosityLevel.Developer:
                    return true;

                default:
                    return true;
            }
        }

        private async Task ProcessQueueAsync()
        {
            var reader = _channel.Reader;
            var token = _cts.Token;
            
            try
            {
                while (await reader.WaitToReadAsync(token))
                {
                    while (reader.TryRead(out var payload))
                    {
                        Interlocked.Decrement(ref _queueSize);
                        try
                        {
                            await WriteLogToFileAsync(payload);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[Logger Internal Error] Failed to write log: {ex.Message}");
                            Console.ResetColor();
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Graceful shutdown via CancellationToken — ปกติไม่ใช่ Error
            }
        }

        private async Task WriteLogToFileAsync(LogPayload payload)
        {
            string subFolder = payload.Category.ToString().ToLower();
            string targetDir = Path.Combine(_logsRootDir, subFolder);
            
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            string fileKey = $"{payload.Category}_{payload.DuelId}";
            
            if (!_activeFiles.TryGetValue(fileKey, out string? filePath))
            {
                string timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
                string deckA_clean = CleanFilename(payload.DeckA);
                string deckB_clean = CleanFilename(payload.DeckB);
                string baseName = $"[{timestamp}]_{payload.DuelId}_{deckA_clean}_vs_{deckB_clean}";
                
                string candidatePath = Path.Combine(targetDir, $"{baseName}.json");
                
                int counter = 1;
                while (File.Exists(candidatePath))
                {
                    candidatePath = Path.Combine(targetDir, $"{baseName}_{counter}.json");
                    counter++;
                }

                filePath = candidatePath;
                _activeFiles[fileKey] = filePath;
                _fileSizes[filePath] = 0;
            }

            string jsonLine = JsonSerializer.Serialize(payload.Entry, new JsonSerializerOptions
            {
                WriteIndented = false
            }) + Environment.NewLine;

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonLine);
            
            long currentSize = _fileSizes.TryGetValue(filePath, out long size) ? size : 0;
            if (currentSize + bytes.Length > MaxFileSizeBytes)
            {
                string dir = Path.GetDirectoryName(filePath) ?? targetDir;
                string nameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
                
                int part = 2;
                if (nameWithoutExt.Contains("_part"))
                {
                    int idx = nameWithoutExt.LastIndexOf("_part");
                    string partStr = nameWithoutExt.Substring(idx + 5);
                    if (int.TryParse(partStr, out int existingPart))
                    {
                        part = existingPart + 1;
                        nameWithoutExt = nameWithoutExt.Substring(0, idx);
                    }
                }
                
                string newPath = Path.Combine(dir, $"{nameWithoutExt}_part{part}.json");
                while (File.Exists(newPath))
                {
                    part++;
                    newPath = Path.Combine(dir, $"{nameWithoutExt}_part{part}.json");
                }

                filePath = newPath;
                _activeFiles[fileKey] = filePath;
                _fileSizes[filePath] = 0;
                currentSize = 0;
            }

            await File.AppendAllBytesAsync(filePath, bytes);
            
            _fileSizes[filePath] = currentSize + bytes.Length;
            Interlocked.Add(ref _bytesWritten, bytes.Length);

            Console.WriteLine($"[{payload.Category.ToString().ToUpper()}] Turn {payload.Entry.Turn} | Action: {payload.Entry.Action,-15} | Card: {payload.Entry.Card,-20} | Reasons: {string.Join(", ", payload.Entry.Reason)}");
        }

        private string CleanFilename(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name.Replace(' ', '_');
        }

        public LoggerStatus GetStatus()
        {
            return new LoggerStatus
            {
                QueueSize = Volatile.Read(ref _queueSize),
                FilesOpen = _activeFiles.Count,
                BytesWritten = Volatile.Read(ref _bytesWritten)
            };
        }

        public async ValueTask DisposeAsync()
        {
            // Complete the channel first to stop accepting new items
            _channel.Writer.Complete();

            try
            {
                // Wait for the write task to drain remaining items (with a timeout)
                var drainTask = _writeTask;
                if (drainTask != null && !drainTask.IsCompleted)
                {
                    // Give it up to 5 seconds to drain
                    var completed = await Task.WhenAny(drainTask, Task.Delay(5000));
                    if (completed != drainTask)
                    {
                        // Drain timed out, force cancel
                        _cts.Cancel();
                        try { await drainTask; } catch { }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                _cts.Cancel();
                _cts.Dispose();
            }
        }
    }

    public class LogPayload
    {
        public LogCategory Category { get; set; }
        public string DuelId { get; set; } = string.Empty;
        public string DeckA { get; set; } = string.Empty;
        public string DeckB { get; set; } = string.Empty;
        public LogEntry Entry { get; set; } = new();
    }

    public class LoggerStatus
    {
        public int QueueSize { get; set; }
        public int FilesOpen { get; set; }
        public long BytesWritten { get; set; }
    }
}
