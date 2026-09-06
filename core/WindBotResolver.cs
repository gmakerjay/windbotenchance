using System;
using System.IO;

namespace YgoAiPlatform.Core
{
    public static class WindBotResolver
    {
        public static string ResolveProjectRoot(string? startDir = null)
        {
            string dir = Path.GetFullPath(startDir ?? AppContext.BaseDirectory);
            for (int i = 0; i < 10; i++)
            {
                if (File.Exists(Path.Combine(dir, "config.json")))
                    return dir;
                string? parent = Directory.GetParent(dir)?.FullName;
                if (parent == null) break;
                dir = parent;
            }

            // Fallback checking for YGO_AI_PLATFORM or windbot-fork directory
            dir = Path.GetFullPath(startDir ?? AppContext.BaseDirectory);
            for (int i = 0; i < 10; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "YGO_AI_PLATFORM")) || Directory.Exists(Path.Combine(dir, "windbot-fork")))
                {
                    if (Directory.Exists(Path.Combine(dir, "YGO_AI_PLATFORM")))
                    {
                        return Path.Combine(dir, "YGO_AI_PLATFORM");
                    }
                    return dir;
                }
                string? parent = Directory.GetParent(dir)?.FullName;
                if (parent == null) break;
                dir = parent;
            }
            return Path.GetFullPath(startDir ?? AppContext.BaseDirectory);
        }

        private static string GetGameDir(string workspaceRoot)
        {
            string gameEdoProDir = Path.Combine(workspaceRoot, "Game_EDOPro");
            if (Directory.Exists(gameEdoProDir))
                return gameEdoProDir;
            
            string edoGameDir = Path.Combine(workspaceRoot, "EdoGame");
            if (Directory.Exists(edoGameDir))
                return edoGameDir;

            return gameEdoProDir; // Fallback to original name
        }

        public static string ResolveYgoproExe(string projectRoot)
        {
            string workspaceRoot = Path.GetDirectoryName(projectRoot) ?? projectRoot;

            // PRIORITY 1: Game_EDOPro/EdoGame directory (single source of truth for all binaries)
            string gameEdoProDir = GetGameDir(workspaceRoot);
            
            if (Directory.Exists(gameEdoProDir))
            {
                // HEADLESS SERVER: ._cache_ygopro.exe is the original YGOPro build that supports
                // -s PORT for headless server mode. ygopro.exe / ygoprodll.exe are EDOPro GUI builds
                // that do NOT support the -s flag for headless server hosting.
                
                // Check for ._cache_ygopro.exe first (original YGOPro - supports headless server)
                string cacheYgopro = Path.Combine(gameEdoProDir, "._cache_ygopro.exe");
                if (File.Exists(cacheYgopro))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[Info] Using headless server binary: {cacheYgopro}");
                    Console.ResetColor();
                    return Path.GetFullPath(cacheYgopro);
                }

                // Check for ._cache_ygoprodll.exe (alternative headless build)
                string cacheDll = Path.Combine(gameEdoProDir, "._cache_ygoprodll.exe");
                if (File.Exists(cacheDll))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[Info] Using headless server binary: {cacheDll}");
                    Console.ResetColor();
                    return Path.GetFullPath(cacheDll);
                }

                // Fallback to ygoprodll.exe (EDOPro GUI - may NOT support headless server)
                string ygoproDll = Path.Combine(gameEdoProDir, "ygoprodll.exe");
                if (File.Exists(ygoproDll))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[Warning] Using EDOPro GUI binary: {ygoproDll} (may not support headless server mode)");
                    Console.ResetColor();
                    return Path.GetFullPath(ygoproDll);
                }

                // Fallback to ygopro.exe (EDOPro GUI version)
                string ygopro = Path.Combine(gameEdoProDir, "ygopro.exe");
                if (File.Exists(ygopro))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[Warning] Using EDOPro GUI binary: {ygopro} (may not support headless server mode)");
                    Console.ResetColor();
                    return Path.GetFullPath(ygopro);
                }
            }

            // PRIORITY 2: Fallback to old resolution logic (not recommended)
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Error] Could not find ygopro binaries in game directory: {gameEdoProDir}!");
            Console.WriteLine("        Please ensure it contains ygopro.exe or ._cache_ygopro.exe");
            Console.ResetColor();
            
            throw new FileNotFoundException($"Could not resolve ygopro.exe in game directory: {gameEdoProDir}");
        }

        public static string ResolveServerWorkingDir(string projectRoot)
        {
            string workspaceRoot = Path.GetDirectoryName(projectRoot) ?? projectRoot;
            string gameEdoProDir = GetGameDir(workspaceRoot);
            if (Directory.Exists(gameEdoProDir))
            {
                return Path.GetFullPath(gameEdoProDir);
            }
            return projectRoot;
        }

        public static string ResolveWindBotDll(string projectRoot)
        {
            string workspaceRoot = Path.GetDirectoryName(projectRoot) ?? projectRoot;
            
            // PRIORITY 1: Game_EDOPro/EdoGame WindBot directory (deployed/unified location)
            string gameEdoProDir = GetGameDir(workspaceRoot);
            string gameEdoProWindBot = Path.Combine(gameEdoProDir, "WindBot", "WindBot.dll");
            if (File.Exists(gameEdoProWindBot))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[Info] Using unified WindBot.dll: {gameEdoProWindBot}");
                Console.ResetColor();
                EnsureCardsCdb(Path.GetDirectoryName(gameEdoProWindBot)!, workspaceRoot, projectRoot);
                return Path.GetFullPath(gameEdoProWindBot);
            }

            // PRIORITY 2: Build output directories
            string[] candidates = new[]
            {
                Path.Combine(projectRoot, "windbot-fork", "bin", "x86", "Release", "net10.0", "WindBot.dll"),
                Path.Combine(projectRoot, "windbot-fork", "bin", "Release", "net10.0", "WindBot.dll"),
                Path.Combine(projectRoot, "windbot-fork", "bin", "x86", "Debug", "net10.0", "WindBot.dll"),
                Path.Combine(projectRoot, "windbot-fork", "bin", "Debug", "net10.0", "WindBot.dll"),
                Path.Combine(projectRoot, "WindBot", "WindBot.dll"),
                Path.Combine(AppContext.BaseDirectory, "windbot-fork", "bin", "x86", "Release", "net10.0", "WindBot.dll"),
                Path.Combine(AppContext.BaseDirectory, "windbot-fork", "bin", "Release", "net10.0", "WindBot.dll"),
                Path.Combine(AppContext.BaseDirectory, "windbot-fork", "bin", "x86", "Debug", "net10.0", "WindBot.dll"),
                Path.Combine(AppContext.BaseDirectory, "windbot-fork", "bin", "Debug", "net10.0", "WindBot.dll"),
                Path.Combine(AppContext.BaseDirectory, "WindBot", "WindBot.dll")
            };

            foreach (var path in candidates)
            {
                string fullPath = Path.GetFullPath(path);
                if (File.Exists(fullPath))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[Warning] Using build output WindBot.dll: {fullPath}");
                    Console.WriteLine("          Consider copying to Game_EDOPro/WindBot for unified deployment");
                    Console.ResetColor();
                    
                    string targetDir = Path.GetDirectoryName(fullPath) ?? "";
                    if (!string.IsNullOrEmpty(targetDir))
                    {
                        EnsureCardsCdb(targetDir, workspaceRoot, projectRoot);
                    }
                    return fullPath;
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Could not find WindBot.dll!");
            Console.WriteLine($"        Expected location: {Path.Combine(gameEdoProDir, "WindBot", "WindBot.dll")}");
            Console.WriteLine("        Please build the project or copy WindBot.dll to game directory.");
            Console.ResetColor();
            
            throw new FileNotFoundException($"Could not resolve WindBot.dll location in: {Path.Combine(gameEdoProDir, "WindBot")}");
        }

        private static void EnsureCardsCdb(string targetDir, string workspaceRoot, string projectRoot)
        {
            string targetCdb = Path.Combine(targetDir, "cards.cdb");
            if (File.Exists(targetCdb)) return;

            // PRIORITY: Always copy from Game_EDOPro / EdoGame (single source of truth)
            string gameEdoProDir = GetGameDir(workspaceRoot);
            string gameEdoProCdb = Path.Combine(gameEdoProDir, "cards.cdb");
            if (File.Exists(gameEdoProCdb))
            {
                try
                {
                    Console.WriteLine($"[Info] Auto-copying cards.cdb from Game_EDOPro to {targetCdb}");
                    File.Copy(gameEdoProCdb, targetCdb, true);
                    return;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[Warning] Failed to auto-copy cards.cdb: {ex.Message}");
                    Console.ResetColor();
                }
            }

            // Fallback to other locations
            string[] sourceCandidates = new[]
            {
                Path.Combine(projectRoot, "windbot-fork", "cards.cdb"),
                Path.Combine(projectRoot, "WindBot", "cards.cdb")
            };
            
            foreach (var src in sourceCandidates)
            {
                string fullSrc = Path.GetFullPath(src);
                if (File.Exists(fullSrc))
                {
                    try
                    {
                        Console.WriteLine($"[Info] Auto-copying cards.cdb from {fullSrc} to {targetCdb}");
                        File.Copy(fullSrc, targetCdb, true);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[Warning] Failed to auto-copy cards.cdb: {ex.Message}");
                        Console.ResetColor();
                    }
                }
            }
        }
    }
}
