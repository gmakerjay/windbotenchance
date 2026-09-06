using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace YgoAiPlatform.Core
{
    public class DashboardDb
    {
        private readonly string _dbPath;
        private readonly string _connectionString;

        public DashboardDb(string projectRootDir)
        {
            string logsDir = Path.Combine(Path.GetFullPath(projectRootDir), "logs");
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }
            _dbPath = Path.Combine(logsDir, "dashboard.db");
            _connectionString = $"Data Source={_dbPath}";
            InitializeDb();
        }

        private void InitializeDb()
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                // Enable WAL mode for better concurrent read/write performance
                using (var pragmaCmd = new SqliteCommand("PRAGMA journal_mode=WAL;", conn))
                    pragmaCmd.ExecuteNonQuery();
                // Wait up to 5 seconds when database is locked instead of throwing immediately
                using (var busyCmd = new SqliteCommand("PRAGMA busy_timeout=5000;", conn))
                    busyCmd.ExecuteNonQuery();

                string duelsTable = @"
                    CREATE TABLE IF NOT EXISTS duels (
                        duel_id TEXT PRIMARY KEY,
                        deck_a TEXT,
                        deck_b TEXT,
                        winner TEXT,
                        turns INTEGER,
                        duration INTEGER,
                        timestamp TEXT
                    );";

                string actionsTable = @"
                    CREATE TABLE IF NOT EXISTS actions (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        duel_id TEXT,
                        turn INTEGER,
                        phase TEXT,
                        action TEXT,
                        card TEXT,
                        score REAL,
                        reason_json TEXT
                    );";

                string trainingSamplesTable = @"
                    CREATE TABLE IF NOT EXISTS training_samples (
                        sample_id TEXT PRIMARY KEY,
                        duel_id TEXT,
                        reward REAL,
                        encoded_state_blob TEXT
                    );";

                string eloHistoryTable = @"
                    CREATE TABLE IF NOT EXISTS elo_history (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        deck_id TEXT,
                        elo REAL,
                        timestamp TEXT,
                        episode INTEGER
                    );";

                string trainingRunsTable = @"
                    CREATE TABLE IF NOT EXISTS training_runs (
                        run_id TEXT PRIMARY KEY,
                        deck_name TEXT NOT NULL,
                        started_at TEXT NOT NULL,
                        finished_at TEXT,
                        total_games INTEGER DEFAULT 0,
                        wins INTEGER DEFAULT 0,
                        losses INTEGER DEFAULT 0,
                        draws INTEGER DEFAULT 0,
                        epochs INTEGER DEFAULT 0,
                        initial_loss REAL DEFAULT 0,
                        final_loss REAL DEFAULT 0,
                        initial_elo REAL DEFAULT 1500,
                        final_elo REAL DEFAULT 1500,
                        workers_used INTEGER DEFAULT 1,
                        samples_collected INTEGER DEFAULT 0,
                        samples_filtered INTEGER DEFAULT 0,
                        status TEXT DEFAULT 'running'
                    );";

                string deckMatchupsTable = @"
                    CREATE TABLE IF NOT EXISTS deck_matchups (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        deck_a TEXT NOT NULL,
                        deck_b TEXT NOT NULL,
                        winner TEXT,
                        turns INTEGER DEFAULT 0,
                        winner_lp INTEGER DEFAULT 0,
                        loser_lp INTEGER DEFAULT 0,
                        duration_seconds REAL DEFAULT 0,
                        training_run_id TEXT,
                        timestamp TEXT NOT NULL
                    );";

                using (var cmd = new SqliteCommand(duelsTable, conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqliteCommand(actionsTable, conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqliteCommand(trainingSamplesTable, conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqliteCommand(eloHistoryTable, conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqliteCommand(trainingRunsTable, conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqliteCommand(deckMatchupsTable, conn)) cmd.ExecuteNonQuery();
            }
        }

        // Methods to save data
        public void SaveDuel(string duelId, string deckA, string deckB, string winner, int turns, int duration)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO duels (duel_id, deck_a, deck_b, winner, turns, duration, timestamp)
                    VALUES (@id, @deckA, @deckB, @winner, @turns, @duration, @timestamp)
                    ON CONFLICT(duel_id) DO UPDATE SET
                        winner=excluded.winner,
                        turns=excluded.turns,
                        duration=excluded.duration,
                        timestamp=excluded.timestamp;";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", duelId);
                    cmd.Parameters.AddWithValue("@deckA", deckA);
                    cmd.Parameters.AddWithValue("@deckB", deckB);
                    cmd.Parameters.AddWithValue("@winner", winner);
                    cmd.Parameters.AddWithValue("@turns", turns);
                    cmd.Parameters.AddWithValue("@duration", duration);
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveAction(string duelId, int turn, string phase, string action, string card, double score, string reasonJson)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO actions (duel_id, turn, phase, action, card, score, reason_json)
                    VALUES (@duelId, @turn, @phase, @action, @card, @score, @reasonJson);";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@duelId", duelId);
                    cmd.Parameters.AddWithValue("@turn", turn);
                    cmd.Parameters.AddWithValue("@phase", phase);
                    cmd.Parameters.AddWithValue("@action", action);
                    cmd.Parameters.AddWithValue("@card", card);
                    cmd.Parameters.AddWithValue("@score", score);
                    cmd.Parameters.AddWithValue("@reasonJson", reasonJson);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveElo(string deckId, double elo, int episode)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO elo_history (deck_id, elo, timestamp, episode)
                    VALUES (@deckId, @elo, @timestamp, @episode);";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@deckId", deckId);
                    cmd.Parameters.AddWithValue("@elo", elo);
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.UtcNow.ToString("o"));
                    cmd.Parameters.AddWithValue("@episode", episode);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Queries
        public List<Dictionary<string, object>> GetDuels()
        {
            var list = new List<Dictionary<string, object>>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM duels ORDER BY timestamp DESC;";
                using (var cmd = new SqliteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>
                        {
                            { "duel_id", reader["duel_id"] },
                            { "deck_a", reader["deck_a"] },
                            { "deck_b", reader["deck_b"] },
                            { "winner", reader["winner"] },
                            { "turns", Convert.ToInt32(reader["turns"]) },
                            { "duration", Convert.ToInt32(reader["duration"]) },
                            { "timestamp", reader["timestamp"] }
                        };
                        list.Add(row);
                    }
                }
            }
            return list;
        }

        public List<Dictionary<string, object>> GetEloHistory(string deckId)
        {
            var list = new List<Dictionary<string, object>>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM elo_history WHERE deck_id = @deckId ORDER BY episode ASC;";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@deckId", deckId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>
                            {
                                { "id", Convert.ToInt32(reader["id"]) },
                                { "deck_id", reader["deck_id"] },
                                { "elo", Convert.ToDouble(reader["elo"]) },
                                { "timestamp", reader["timestamp"] },
                                { "episode", Convert.ToInt32(reader["episode"]) }
                            };
                            list.Add(row);
                        }
                    }
                }
            }
            return list;
        }

        public List<Dictionary<string, object>> GetDeckPerformance()
        {
            var list = new List<Dictionary<string, object>>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                // Query unique decks and calculate ELO and winrates
                string sql = @"
                    SELECT 
                        deck,
                        COUNT(*) as total_matches,
                        SUM(CASE WHEN winner = deck THEN 1 ELSE 0 END) as wins,
                        (SELECT elo FROM elo_history WHERE deck_id = deck ORDER BY episode DESC LIMIT 1) as current_elo
                    FROM (
                        SELECT deck_a as deck, winner FROM duels
                        UNION ALL
                        SELECT deck_b as deck, winner FROM duels
                    )
                    GROUP BY deck;";

                using (var cmd = new SqliteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        double elo = reader["current_elo"] != DBNull.Value ? Convert.ToDouble(reader["current_elo"]) : 1500.0;
                        int total = Convert.ToInt32(reader["total_matches"]);
                        int wins = Convert.ToInt32(reader["wins"]);
                        double winrate = total > 0 ? (double)wins / total : 0.0;

                        var row = new Dictionary<string, object>
                        {
                            { "deck", reader["deck"] },
                            { "matches", total },
                            { "wins", wins },
                            { "losses", total - wins },
                            { "winrate", winrate },
                            { "elo", elo }
                        };
                        list.Add(row);
                    }
                }
            }
            return list;
        }

        // --- Training Runs ---

        public string CreateTrainingRun(string deckName, int workers)
        {
            string runId = Guid.NewGuid().ToString("N")[..12];
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO training_runs (run_id, deck_name, started_at, workers_used, status)
                    VALUES (@runId, @deckName, @startedAt, @workers, 'running');";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@runId", runId);
                    cmd.Parameters.AddWithValue("@deckName", deckName);
                    cmd.Parameters.AddWithValue("@startedAt", DateTime.UtcNow.ToString("o"));
                    cmd.Parameters.AddWithValue("@workers", workers);
                    cmd.ExecuteNonQuery();
                }
            }
            return runId;
        }

        public void UpdateTrainingRun(string runId, int games, int wins, int losses, int draws,
            int epochs, double initialLoss, double finalLoss, double initialElo, double finalElo,
            int samplesCollected, int samplesFiltered, string status)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    UPDATE training_runs SET
                        total_games = @games,
                        wins = @wins,
                        losses = @losses,
                        draws = @draws,
                        epochs = @epochs,
                        initial_loss = @initialLoss,
                        final_loss = @finalLoss,
                        initial_elo = @initialElo,
                        final_elo = @finalElo,
                        samples_collected = @samplesCollected,
                        samples_filtered = @samplesFiltered,
                        status = @status,
                        finished_at = CASE WHEN @status IN ('completed','failed','cancelled') THEN @finishedAt ELSE finished_at END
                    WHERE run_id = @runId;";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@runId", runId);
                    cmd.Parameters.AddWithValue("@games", games);
                    cmd.Parameters.AddWithValue("@wins", wins);
                    cmd.Parameters.AddWithValue("@losses", losses);
                    cmd.Parameters.AddWithValue("@draws", draws);
                    cmd.Parameters.AddWithValue("@epochs", epochs);
                    cmd.Parameters.AddWithValue("@initialLoss", initialLoss);
                    cmd.Parameters.AddWithValue("@finalLoss", finalLoss);
                    cmd.Parameters.AddWithValue("@initialElo", initialElo);
                    cmd.Parameters.AddWithValue("@finalElo", finalElo);
                    cmd.Parameters.AddWithValue("@samplesCollected", samplesCollected);
                    cmd.Parameters.AddWithValue("@samplesFiltered", samplesFiltered);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@finishedAt", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // --- Deck Matchups ---

        public void SaveMatchup(string deckA, string deckB, string winner, int turns,
            int winnerLp, int loserLp, double durationSeconds, string? trainingRunId)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO deck_matchups (deck_a, deck_b, winner, turns, winner_lp, loser_lp, duration_seconds, training_run_id, timestamp)
                    VALUES (@deckA, @deckB, @winner, @turns, @winnerLp, @loserLp, @duration, @trainingRunId, @timestamp);";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@deckA", deckA);
                    cmd.Parameters.AddWithValue("@deckB", deckB);
                    cmd.Parameters.AddWithValue("@winner", winner);
                    cmd.Parameters.AddWithValue("@turns", turns);
                    cmd.Parameters.AddWithValue("@winnerLp", winnerLp);
                    cmd.Parameters.AddWithValue("@loserLp", loserLp);
                    cmd.Parameters.AddWithValue("@duration", durationSeconds);
                    cmd.Parameters.AddWithValue("@trainingRunId", (object?)trainingRunId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<DeckMatchupStats> GetDeckMatchupMatrix()
        {
            var list = new List<DeckMatchupStats>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    SELECT
                        deck_a,
                        deck_b,
                        SUM(CASE WHEN winner = 'a' THEN 1 ELSE 0 END) as wins_a,
                        SUM(CASE WHEN winner = 'b' THEN 1 ELSE 0 END) as wins_b,
                        COUNT(*) as total
                    FROM deck_matchups
                    GROUP BY deck_a, deck_b;";

                using (var cmd = new SqliteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int winsA = Convert.ToInt32(reader["wins_a"]);
                        int winsB = Convert.ToInt32(reader["wins_b"]);
                        int total = Convert.ToInt32(reader["total"]);
                        list.Add(new DeckMatchupStats
                        {
                            DeckA = reader["deck_a"].ToString()!,
                            DeckB = reader["deck_b"].ToString()!,
                            Wins = winsA,
                            Losses = winsB,
                            WinRate = total > 0 ? (double)winsA / total : 0.0
                        });
                    }
                }
            }
            return list;
        }

        public List<TrainingRunRecord> GetTrainingRuns(string? deckName = null)
        {
            var list = new List<TrainingRunRecord>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = deckName != null
                    ? "SELECT * FROM training_runs WHERE deck_name = @deckName ORDER BY started_at DESC;"
                    : "SELECT * FROM training_runs ORDER BY started_at DESC;";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    if (deckName != null)
                        cmd.Parameters.AddWithValue("@deckName", deckName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new TrainingRunRecord
                            {
                                RunId = reader["run_id"].ToString()!,
                                DeckName = reader["deck_name"].ToString()!,
                                StartedAt = reader["started_at"].ToString()!,
                                FinishedAt = reader["finished_at"] == DBNull.Value ? null : reader["finished_at"].ToString(),
                                TotalGames = Convert.ToInt32(reader["total_games"]),
                                Wins = Convert.ToInt32(reader["wins"]),
                                Losses = Convert.ToInt32(reader["losses"]),
                                Draws = Convert.ToInt32(reader["draws"]),
                                Epochs = Convert.ToInt32(reader["epochs"]),
                                InitialLoss = Convert.ToDouble(reader["initial_loss"]),
                                FinalLoss = Convert.ToDouble(reader["final_loss"]),
                                InitialElo = Convert.ToDouble(reader["initial_elo"]),
                                FinalElo = Convert.ToDouble(reader["final_elo"]),
                                WorkersUsed = Convert.ToInt32(reader["workers_used"]),
                                SamplesCollected = Convert.ToInt32(reader["samples_collected"]),
                                SamplesFiltered = Convert.ToInt32(reader["samples_filtered"]),
                                Status = reader["status"].ToString()!
                            });
                        }
                    }
                }
            }
            return list;
        }
    }

    // --- Data Classes ---

    public class TrainingRunRecord
    {
        public string RunId { get; set; } = "";
        public string DeckName { get; set; } = "";
        public string StartedAt { get; set; } = "";
        public string? FinishedAt { get; set; }
        public int TotalGames { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int Epochs { get; set; }
        public double InitialLoss { get; set; }
        public double FinalLoss { get; set; }
        public double InitialElo { get; set; } = 1500;
        public double FinalElo { get; set; } = 1500;
        public int WorkersUsed { get; set; } = 1;
        public int SamplesCollected { get; set; }
        public int SamplesFiltered { get; set; }
        public string Status { get; set; } = "running";
    }

    public class DeckMatchupStats
    {
        public string DeckA { get; set; } = "";
        public string DeckB { get; set; } = "";
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double WinRate { get; set; }
    }
}
