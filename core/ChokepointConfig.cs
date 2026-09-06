using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YgoAiPlatform.Core
{
    public class ChokepointConfig
    {
        [JsonPropertyName("chokepoints")]
        public List<string> Chokepoints { get; set; } = new();

        [JsonPropertyName("counters")]
        public List<string> Counters { get; set; } = new();

        private HashSet<string> _chokepointSet = new(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _counterSet = new(StringComparer.OrdinalIgnoreCase);

        public void Initialize()
        {
            _chokepointSet = new HashSet<string>(Chokepoints ?? new List<string>(), StringComparer.OrdinalIgnoreCase);
            _counterSet = new HashSet<string>(Counters ?? new List<string>(), StringComparer.OrdinalIgnoreCase);
        }

        public bool IsChokepoint(string? name, int id)
        {
            if (!string.IsNullOrEmpty(name) && _chokepointSet.Contains(name)) return true;
            if (_chokepointSet.Contains(id.ToString())) return true;
            return false;
        }

        public bool IsCounter(string? name, int id)
        {
            if (!string.IsNullOrEmpty(name) && _counterSet.Contains(name)) return true;
            if (_counterSet.Contains(id.ToString())) return true;
            return false;
        }

        public static ChokepointConfig Load(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    var config = JsonSerializer.Deserialize<ChokepointConfig>(json);
                    if (config != null)
                    {
                        config.Initialize();
                        return config;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChokepointConfig] Failed to load config from {path}: {ex.Message}");
            }

            var fallback = new ChokepointConfig();
            fallback.Initialize();
            return fallback;
        }
    }
}
