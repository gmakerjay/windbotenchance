using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WindBot.Game.AI;

namespace WindBot.Game
{
    public static class DecksManager
    {
        private class DeckInstance
        {
            public string Deck { get; private set; }
            public Type Type { get; private set; }
            public string Level { get; private set; }

            public DeckInstance(string deck, Type type, string level)
            {
                Deck = deck;
                Type = type;
                Level = level;
            }
        }

        private static Dictionary<string, DeckInstance> _decks;
        private static List<DeckInstance> _list;
        private static Random _rand;

        public static void Init()
        {
            _decks = new Dictionary<string, DeckInstance>();
            _rand = new Random();

            Assembly asm = Assembly.GetExecutingAssembly();
            Type[] types = asm.GetTypes();
            
            foreach (Type type in types)
            {
                MemberInfo info = type;
                object[] attributes = info.GetCustomAttributes(false);
                foreach (object attribute in attributes)
                {
                    if (attribute is DeckAttribute)
                    {
                        DeckAttribute deck = (DeckAttribute)attribute;
                        _decks.Add(deck.Name, new DeckInstance(deck.File, type, deck.Level));
                    }
                }
            }
            try
            {
                if (Directory.Exists("Executors"))
                {
                    string[] files = Directory.GetFiles("Executors", "*.dll", SearchOption.TopDirectoryOnly);
                    foreach (string file in files)
                    {
                        Assembly assembly = Assembly.LoadFrom(file);
                        Type[] types2 = assembly.GetTypes();
                        foreach (Type type in types2)
                        {
                        try
                        {
                            MemberInfo info = type;
                            object[] attributes = info.GetCustomAttributes(false);
                            foreach (object attribute in attributes)
                            {
                                if (attribute is DeckAttribute)
                                {
                                    DeckAttribute deck = (DeckAttribute)attribute;
                                    _decks.Add(deck.Name, new DeckInstance(deck.File, type, deck.Level));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLine("Executor loading (" + file + ") error: " + ex);
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLine(ex.ToString());
            }

            _list = new List<DeckInstance>();
            _list.AddRange(_decks.Values);

            Logger.WriteLine("Decks initialized, " + _decks.Count + " found.");
        }

        private static string NormalizeDeckName(string name)
        {
            if (string.IsNullOrEmpty(name)) return string.Empty;
            return name.ToLowerInvariant().Replace("-", "").Replace("_", "").Replace(" ", "");
        }

        private static string StripPrefix(string name)
        {
            if (string.IsNullOrEmpty(name)) return string.Empty;
            string n = NormalizeDeckName(name);
            if (n.StartsWith("expert2026")) n = n.Substring(10);
            else if (n.StartsWith("neural2026")) n = n.Substring(10);
            else if (n.StartsWith("2026")) n = n.Substring(4);
            else if (n.StartsWith("expert")) n = n.Substring(6);
            else if (n.StartsWith("neural")) n = n.Substring(6);
            else if (n.StartsWith("ai")) n = n.Substring(2);
            else if (n.StartsWith("anime")) n = n.Substring(5);
            else if (n.StartsWith("goat")) n = n.Substring(4);
            return n;
        }

        public static Executor Instantiate(GameAI ai, Duel duel, string deck)
        {
            DeckInstance infos = null;

            if (deck != null && _decks.ContainsKey(deck))
            {
                infos = _decks[deck];
                Logger.WriteLine("Deck found, loading " + infos.Deck);
            }
            else if (deck != null)
            {
                string normalizedSearch = NormalizeDeckName(deck);
                string strippedSearch = StripPrefix(deck);

                // Pass 1: exact normalized match
                foreach (var kvp in _decks)
                {
                    if (NormalizeDeckName(kvp.Key) == normalizedSearch || NormalizeDeckName(kvp.Value.Deck) == normalizedSearch)
                    {
                        infos = kvp.Value;
                        Logger.WriteLine("Deck found (via robust match), loading " + infos.Deck);
                        break;
                    }
                }

                // Pass 2: prefix-stripped match (e.g. 2026_AFS -> AFS, or vice versa)
                if (infos == null && !string.IsNullOrEmpty(strippedSearch))
                {
                    foreach (var kvp in _decks)
                    {
                        if (StripPrefix(kvp.Key) == strippedSearch || StripPrefix(kvp.Value.Deck) == strippedSearch)
                        {
                            infos = kvp.Value;
                            Logger.WriteLine("Deck found (via stripped prefix match), loading " + infos.Deck);
                            break;
                        }
                    }
                }

                // Pass 3: substring contains match
                if (infos == null && normalizedSearch.Length >= 3)
                {
                    foreach (var kvp in _decks)
                    {
                        string kNorm = NormalizeDeckName(kvp.Key);
                        string dNorm = NormalizeDeckName(kvp.Value.Deck);
                        if (normalizedSearch.Contains(kNorm) || kNorm.Contains(normalizedSearch) ||
                            normalizedSearch.Contains(dNorm) || dNorm.Contains(normalizedSearch))
                        {
                            infos = kvp.Value;
                            Logger.WriteLine("Deck found (via contains match), loading " + infos.Deck);
                            break;
                        }
                    }
                }
            }

            if (infos == null)
            {
                do
                {
                    infos = _list[_rand.Next(_list.Count)];
                }
                while (infos.Level != "Normal");
                Logger.WriteLine("Deck not found, loading random: " + infos.Deck);
            }

            Executor executor = (Executor)Activator.CreateInstance(infos.Type, ai, duel);

            // If the user requested a specific deck file and it exists on disk, preserve it!
            if (!string.IsNullOrEmpty(deck))
            {
                string candidate = Path.Combine(BotConfig.AssetPath, "Decks", deck + ".ydk");
                if (File.Exists(candidate))
                {
                    executor.Deck = deck;
                    return executor;
                }
            }

            executor.Deck = infos.Deck;
            return executor;
        }
    }
}
