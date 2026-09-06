using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;

namespace YgoAiPlatform.Core
{
    public class ReplayParser
    {
        public static List<string> Reconstruct(string datasetPath)
        {
            var logs = new List<string>();
            if (!File.Exists(datasetPath))
            {
                logs.Add($"Error: File not found: {datasetPath}");
                return logs;
            }

            bool isCompressed = datasetPath.EndsWith(".gz", StringComparison.OrdinalIgnoreCase);

            try
            {
                using var fileStream = new FileStream(datasetPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using Stream readStream = isCompressed ? new GZipStream(fileStream, CompressionMode.Decompress) : fileStream;
                using var reader = new StreamReader(readStream, System.Text.Encoding.UTF8);

                string? line;
                int lineNumber = 0;

                logs.Add($"=== Reconstructing Duel Replay from {Path.GetFileName(datasetPath)} ===");

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    try
                    {
                        var sample = JsonSerializer.Deserialize<DatasetSample>(line);
                        if (sample == null) continue;

                        var logMsg = $"Turn {sample.Turn} | Phase {sample.Phase} | Duel ID: {sample.DuelId}\n" +
                                     $"  [Player] LP: {sample.BoardState.Lp} | Hand: {sample.BoardState.Hand.Count} cards ({string.Join(", ", sample.BoardState.Hand)}) | Field: {string.Join(", ", sample.BoardState.Field)}\n" +
                                     $"  [Opponent] LP: {sample.OpponentState.Lp} | Hand Count: {sample.OpponentState.HandCount} | Field: {string.Join(", ", sample.OpponentState.Field)}\n" +
                                     $"  [Action] Chosen: {sample.ChosenAction} | Score: {sample.BoardScore:F2}\n" +
                                     $"  [Reasoning] {string.Join(" & ", sample.ActionReason)}";
                        
                        logs.Add(logMsg);
                    }
                    catch (Exception ex)
                    {
                        logs.Add($"Line {lineNumber}: [Corrupt Sample] {ex.Message}");
                    }
                }
                logs.Add($"=== End of Replay Reconstruction (Total Turns: {lineNumber}) ===");
            }
            catch (Exception ex)
            {
                logs.Add($"Failed to read dataset: {ex.Message}");
            }

            return logs;
        }
    }
}
