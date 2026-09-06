using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace YgoAiPlatform.Core
{
    /// <summary>
    /// ตัวเขียน Dataset แบบ Non-blocking ผ่าน Channel + Background Task
    /// รองรับ .jsonl และ .jsonl.gz (Gzip compression)
    /// </summary>
    public class DatasetBuilder : IAsyncDisposable
    {
        private readonly string _filePath;
        private readonly Channel<DatasetSample> _channel;
        private readonly Task _writeTask;
        private readonly bool _useCompression;
        private volatile bool _isCompleted = false;

        public DatasetBuilder(string filePath)
        {
            _filePath = Path.GetFullPath(filePath);
            _useCompression = _filePath.EndsWith(".gz", StringComparison.OrdinalIgnoreCase);

            string? dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // Create bounded channel for backpressure safety
            _channel = Channel.CreateBounded<DatasetSample>(new BoundedChannelOptions(5000)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = false
            });

            _writeTask = Task.Run(WriteLoopAsync);
        }

        /// <summary>
        /// Synchronous write — blocks until sample is accepted (prevents silent data loss).
        /// Use only from non-critical paths; for hot paths, prefer AppendSampleAsync.
        /// </summary>
        public bool AppendSample(DatasetSample sample)
        {
            if (_isCompleted) return false;
            // Block until channel has room — prevents silent drops from TryWrite
            if (!_channel.Writer.TryWrite(sample))
            {
                _channel.Writer.WriteAsync(sample).AsTask().GetAwaiter().GetResult();
            }
            return true;
        }

        public async Task AppendSampleAsync(DatasetSample sample)
        {
            if (_isCompleted) return;
            await _channel.Writer.WriteAsync(sample);
        }

        private async Task WriteLoopAsync()
        {
            Stream? fileStream = null;
            Stream? writeStream = null;
            StreamWriter? writer = null;

            try
            {
                fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: true);
                if (_useCompression)
                {
                    writeStream = new GZipStream(fileStream, CompressionLevel.Optimal, leaveOpen: true);
                    writer = new StreamWriter(writeStream, System.Text.Encoding.UTF8);
                }
                else
                {
                    writeStream = fileStream;
                    writer = new StreamWriter(writeStream, System.Text.Encoding.UTF8);
                }

                while (await _channel.Reader.WaitToReadAsync())
                {
                    while (_channel.Reader.TryRead(out var sample))
                    {
                        string json = JsonSerializer.Serialize(sample);
                        await writer.WriteLineAsync(json);
                    }
                    await writer.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DatasetBuilder Error] Exception in write loop: {ex.Message}");
            }
            finally
            {
                if (writer != null) await writer.DisposeAsync();
                if (_useCompression && writeStream != null) await writeStream.DisposeAsync();
                if (fileStream != null) await fileStream.DisposeAsync();
            }
        }

        public static ValidationReport ValidateFile(string filePath)
        {
            var report = new ValidationReport();
            if (!File.Exists(filePath))
            {
                report.CorruptSamples = 1;
                report.SchemaErrors.Add($"File does not exist: {filePath}");
                return report;
            }

            bool isCompressed = filePath.EndsWith(".gz", StringComparison.OrdinalIgnoreCase);

            try
            {
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using Stream readStream = isCompressed ? new GZipStream(fileStream, CompressionMode.Decompress) : fileStream;
                using var reader = new StreamReader(readStream, System.Text.Encoding.UTF8);

                string? line;
                int lineNumber = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    report.TotalSamples++;

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        report.CorruptSamples++;
                        report.SchemaErrors.Add($"Line {lineNumber}: Empty or whitespace line.");
                        continue;
                    }

                    try
                    {
                        var sample = JsonSerializer.Deserialize<DatasetSample>(line);
                        if (sample == null)
                        {
                            report.CorruptSamples++;
                            report.SchemaErrors.Add($"Line {lineNumber}: Deserialization returned null.");
                            continue;
                        }

                        // Schema checking
                        if (string.IsNullOrEmpty(sample.DuelId))
                        {
                            report.SchemaErrors.Add($"Line {lineNumber}: Missing 'duel_id'.");
                        }
                        if (sample.Turn <= 0)
                        {
                            report.SchemaErrors.Add($"Line {lineNumber}: Invalid 'turn' number: {sample.Turn}.");
                        }
                        if (string.IsNullOrEmpty(sample.Phase))
                        {
                            report.SchemaErrors.Add($"Line {lineNumber}: Missing 'phase'.");
                        }
                        if (sample.BoardState == null)
                        {
                            report.SchemaErrors.Add($"Line {lineNumber}: Missing 'board_state'.");
                        }
                        if (sample.OpponentState == null)
                        {
                            report.SchemaErrors.Add($"Line {lineNumber}: Missing 'opponent_state'.");
                        }
                    }
                    catch (JsonException ex)
                    {
                        report.CorruptSamples++;
                        report.SchemaErrors.Add($"Line {lineNumber}: JSON Parsing error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                report.CorruptSamples++;
                report.SchemaErrors.Add($"General error reading file: {ex.Message}");
            }

            return report;
        }

        public async ValueTask DisposeAsync()
        {
            _isCompleted = true;
            _channel.Writer.Complete();

            try
            {
                await _writeTask;
            }
            catch {}
        }
    }
}
