using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YgoAiPlatform.Core
{
    /// <summary>
    /// Utility to automatically prune and clean up old duel logs and snapshots,
    /// keeping disk usage lightweight and preventing log file accumulation.
    /// </summary>
    public static class LogCleanupUtility
    {
        /// <summary>
        /// Scans known log directories and removes old duel sessions, retaining only the latest N matches.
        /// </summary>
        /// <param name="baseSearchDir">Root or base directory to search for logs folders.</param>
        /// <param name="maxRetainedMatches">Maximum number of recent duel sessions to keep (default: 20).</param>
        /// <returns>Total number of deleted folders and files.</returns>
        public static int CleanupOldLogs(string baseSearchDir, int maxRetainedMatches = 20)
        {
            if (maxRetainedMatches <= 0) maxRetainedMatches = 20;
            int deletedCount = 0;

            try
            {
                var candidateDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                void AddIfFound(string? path)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        try
                        {
                            if (Directory.Exists(path))
                            {
                                candidateDirs.Add(Path.GetFullPath(path));
                            }
                        }
                        catch { }
                    }
                }

                // Add search paths
                AddIfFound(Path.Combine(baseSearchDir, "logs"));
                AddIfFound(Path.Combine(baseSearchDir, "WindBot", "logs"));

                // Also check up to 3 parent directories
                string current = baseSearchDir;
                for (int i = 0; i < 3; i++)
                {
                    var parent = Directory.GetParent(current);
                    if (parent == null) break;
                    AddIfFound(Path.Combine(parent.FullName, "logs"));
                    AddIfFound(Path.Combine(parent.FullName, "WindBot", "logs"));
                    current = parent.FullName;
                }

                foreach (var logsDir in candidateDirs)
                {
                    try
                    {
                        if (!Directory.Exists(logsDir)) continue;

                        var dirInfo = new DirectoryInfo(logsDir);

                        // 1. Cleanup duel session directories (exclude "headless" folder)
                        var duelDirs = dirInfo.GetDirectories()
                            .Where(d => !d.Name.Equals("headless", StringComparison.OrdinalIgnoreCase))
                            .OrderByDescending(d => d.LastWriteTimeUtc)
                            .ToList();

                        if (duelDirs.Count > maxRetainedMatches)
                        {
                            var dirsToDelete = duelDirs.Skip(maxRetainedMatches);
                            foreach (var d in dirsToDelete)
                            {
                                try
                                {
                                    d.Delete(true);
                                    deletedCount++;
                                }
                                catch
                                {
                                    // Skip if locked or in use by active duel
                                }
                            }
                        }

                        // 2. Cleanup headless client logs
                        string headlessDir = Path.Combine(logsDir, "headless");
                        if (Directory.Exists(headlessDir))
                        {
                            var headlessInfo = new DirectoryInfo(headlessDir);
                            var logFiles = headlessInfo.GetFiles("*.log")
                                .OrderByDescending(f => f.LastWriteTimeUtc)
                                .ToList();

                            if (logFiles.Count > maxRetainedMatches)
                            {
                                var filesToDelete = logFiles.Skip(maxRetainedMatches);
                                foreach (var f in filesToDelete)
                                {
                                    try
                                    {
                                        f.Delete();
                                        deletedCount++;
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Safely ignore errors during directory iteration
                    }
                }
            }
            catch
            {
                // Never crash calling application on log cleanup
            }

            return deletedCount;
        }
    }
}
