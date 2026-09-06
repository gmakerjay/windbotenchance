using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace YgoAiPlatform.Core
{
    /// <summary>
    /// 🌐 DiversityTracker — รับประกันว่าข้อมูล Training ครอบคลุมสถานการณ์หลากหลาย
    /// 
    /// ปัญหาที่แก้: ถ้า NeuralExecutor เจอแต่ board state เดิมๆ ซ้ำๆ → overfit
    /// → บอทเก่งเฉพาะสถานการณ์ที่เคยเจอ → Generalization แย่
    /// 
    /// วิธีแก้: เก็บ state hash + opponent deck → วัดความคล้าย → ให้คะแนน Novelty
    /// → novel states ได้ quality bonus → มีน้ำหนักในการเทรนมากกว่า
    /// → duplicate states ถูกลดน้ำหนัก → ประหยัด training time
    /// 
    /// Integration:
    ///   NeuralExecutor → DiversityTracker.RecordState(stateHash, opponentDeck)
    ///   NeuralExecutor → DiversityTracker.GetNoveltyScore(stateHash) ← fed to QualityGuard
    /// 
    /// Uses real API: GameState.GetStateHash() (GameState.cs:37)
    /// </summary>
    public class DiversityTracker
    {
        /// <summary>
        /// Max number of unique state signatures to track per opponent.
        /// LRU eviction when exceeded.
        /// </summary>
        private const int MaxTrackedStates = 5000;

        /// <summary>
        /// How many buckets to divide the state space into (for efficient nearest-neighbor).
        /// Higher = finer granularity but more memory.
        /// </summary>
        private const int HashBucketCount = 256;

        /// <summary>
        /// Per-opponent state buckets. Each bucket contains state hashes.
        /// Key: opponent deck name
        /// Value: array of buckets, each bucket is a list of hashes
        /// </summary>
        private readonly ConcurrentDictionary<string, List<string>[]> _opponentStates = new();

        /// <summary>
        /// Global LRU queue for eviction policy.
        /// </summary>
        private readonly ConcurrentQueue<string> _lruQueue = new();

        /// <summary>
        /// Fast lookup: hash → (opponent, bucketIndex)
        /// </summary>
        private readonly ConcurrentDictionary<string, (string Opponent, int Bucket)> _hashIndex = new();

        private readonly object _evictLock = new();

        private int _totalTracked = 0;

        /// <summary>
        /// Statistics for monitoring data diversity.
        /// </summary>
        public DiversityStats Stats { get; } = new();

        /// <summary>
        /// Record a state observation. Returns true if this is a novel state.
        /// 
        /// Call this from NeuralExecutor.OnSelectIdleCmd / OnBattle 
        /// AFTER building the GameState via ClientCardMapper.BuildGameState().
        /// 
        /// Real API reference:
        ///   GameState state = ClientCardMapper.BuildGameState(Bot, Enemy);  // ClientCardMapper.cs:138
        ///   string hash = state.GetStateHash();                              // GameState.cs:37
        ///   tracker.RecordState(hash, "Altergeist");
        /// </summary>
        /// <param name="stateHash">SHA256 hash from GameState.GetStateHash()</param>
        /// <param name="opponentDeck">Name of the opponent's deck (e.g. "Altergeist", "SkyStriker")</param>
        /// <returns>True if this state was NOT previously seen for this opponent</returns>
        public bool RecordState(string stateHash, string opponentDeck)
        {
            if (string.IsNullOrEmpty(stateHash) || string.IsNullOrEmpty(opponentDeck))
                return true; // Treat unknown as novel

            // Get or create opponent bucket array
            var buckets = _opponentStates.GetOrAdd(opponentDeck, _ =>
            {
                var arr = new List<string>[HashBucketCount];
                for (int i = 0; i < HashBucketCount; i++)
                    arr[i] = new List<string>();
                return arr;
            });

            // Hash the hash to get bucket index
            int bucketIdx = Math.Abs(stateHash.GetHashCode()) % HashBucketCount;
            var bucket = buckets[bucketIdx];

            // Check if this exact state already exists
            bool isNovel = true;
            lock (bucket)
            {
                if (bucket.Contains(stateHash))
                {
                    isNovel = false;
                    Stats.DuplicateStates++;
                }
                else
                {
                    bucket.Add(stateHash);
                    Stats.UniqueStates++;
                    _totalTracked++;
                }
            }

            // Track in global index
            _hashIndex.TryAdd(stateHash, (opponentDeck, bucketIdx));
            _lruQueue.Enqueue(stateHash);

            // LRU eviction if over capacity
            if (_totalTracked > MaxTrackedStates)
            {
                EvictOldest();
            }

            return isNovel;
        }

        /// <summary>
        /// Compute similarity to the closest previously-seen state.
        /// Returns 0.0 = completely novel (no similar state found)
        /// Returns 1.0 = exact duplicate
        /// 
        /// This goes into QualityGuard.ComputeNoveltyScore(similarity)
        /// → novel states get quality bonus → weighted higher in training
        /// </summary>
        public double GetMaxSimilarity(string stateHash, string opponentDeck)
        {
            if (string.IsNullOrEmpty(stateHash) || string.IsNullOrEmpty(opponentDeck))
                return 0.0; // Unknown = treat as novel

            if (!_opponentStates.TryGetValue(opponentDeck, out var buckets))
                return 0.0; // No history for this opponent = novel

            // Check exact match first
            int bucketIdx = Math.Abs(stateHash.GetHashCode()) % HashBucketCount;
            var bucket = buckets[bucketIdx];

            lock (bucket)
            {
                if (bucket.Contains(stateHash))
                    return 1.0; // Exact match
            }

            // Check nearby buckets for partial match (prefix similarity)
            int prefixLen = Math.Min(8, stateHash.Length);
            string prefix = stateHash.Substring(0, prefixLen);

            for (int offset = -2; offset <= 2; offset++)
            {
                int checkIdx = (bucketIdx + offset + HashBucketCount) % HashBucketCount;
                var checkBucket = buckets[checkIdx];
                lock (checkBucket)
                {
                    foreach (var h in checkBucket)
                    {
                        if (h.Length >= prefixLen && h.StartsWith(prefix))
                            return 0.3; // Partial match (same prefix = similar situation)
                    }
                }
            }

            return 0.0; // Completely novel
        }

        /// <summary>
        /// Get the diversity coverage score for an opponent.
        /// High = we've seen many different states → good generalization expected
        /// Low = we've only seen a narrow set of situations → poor generalization
        /// </summary>
        public double GetCoverageScore(string opponentDeck)
        {
            if (!_opponentStates.TryGetValue(opponentDeck, out var buckets))
                return 0.0;

            int totalInBuckets = 0;
            int nonEmptyBuckets = 0;
            for (int i = 0; i < HashBucketCount; i++)
            {
                lock (buckets[i])
                {
                    totalInBuckets += buckets[i].Count;
                    if (buckets[i].Count > 0) nonEmptyBuckets++;
                }
            }

            // Coverage = what fraction of the state space have we sampled?
            // Only count buckets that reached minimum threshold
            double fillRatio = (double)nonEmptyBuckets / HashBucketCount;

            // Also consider density: average states per bucket
            double avgDensity = nonEmptyBuckets > 0
                ? (double)totalInBuckets / nonEmptyBuckets
                : 0;

            // Combined: fill × log(density) → rewards both breadth and depth
            double densityScore = avgDensity > 1 ? Math.Log(avgDensity) / Math.Log(10) : 0;
            return Math.Min(1.0, fillRatio * 0.7 + densityScore * 0.3);
        }

        /// <summary>
        /// Get the list of opponents we need MORE data for (coverage < 0.3).
        /// Used by training planner to schedule additional collection.
        /// </summary>
        public List<string> GetUnderrepresentedOpponents(double threshold = 0.3)
        {
            var underrepresented = new List<string>();
            foreach (var kvp in _opponentStates)
            {
                double coverage = GetCoverageScore(kvp.Key);
                if (coverage < threshold)
                    underrepresented.Add(kvp.Key);
            }
            return underrepresented.OrderBy(GetCoverageScore).ToList();
        }

        /// <summary>
        /// Get recommended opponents for generalization training.
        /// Returns opponents ranked by: (low coverage first, then unused opponents)
        /// </summary>
        public List<string> GetGeneralizationRecommendations(
            IEnumerable<string> allAvailableOpponents,
            int topN = 3)
        {
            var scored = new List<(string Deck, double Priority)>();

            foreach (var opponent in allAvailableOpponents)
            {
                double coverage = GetCoverageScore(opponent);
                // Lower coverage = higher priority
                double priority = 1.0 - coverage;
                scored.Add((opponent, priority));
            }

            return scored
                .OrderByDescending(s => s.Priority)
                .Take(topN)
                .Select(s => s.Deck)
                .ToList();
        }

        private void EvictOldest()
        {
            lock (_evictLock)
            {
                int toEvict = Math.Min(500, _totalTracked - MaxTrackedStates / 2);
                for (int i = 0; i < toEvict; i++)
                {
                    if (!_lruQueue.TryDequeue(out var oldHash)) break;
                    if (!_hashIndex.TryRemove(oldHash, out var entry)) continue;

                    var (opponent, bucketIdx) = entry;
                    if (_opponentStates.TryGetValue(opponent, out var buckets))
                    {
                        lock (buckets[bucketIdx])
                        {
                            buckets[bucketIdx].Remove(oldHash);
                        }
                    }
                    _totalTracked--;
                }
            }
        }
    }

    /// <summary>
    /// Statistics for monitoring data diversity health.
    /// </summary>
    public class DiversityStats
    {
        /// <summary>Total unique states recorded (all opponents combined)</summary>
        public int UniqueStates { get; set; }

        /// <summary>Total duplicate states detected (ignored from uniqueness count)</summary>
        public int DuplicateStates { get; set; }

        /// <summary>Total states processed (unique + duplicate)</summary>
        public int TotalObserved => UniqueStates + DuplicateStates;

        /// <summary>Novelty rate: what fraction of observations were new?</summary>
        public double NoveltyRate => TotalObserved > 0
            ? (double)UniqueStates / TotalObserved
            : 0.0;

        public override string ToString()
        {
            return $"Unique:{UniqueStates} Dup:{DuplicateStates} Novelty:{NoveltyRate:P1}";
        }
    }
}
