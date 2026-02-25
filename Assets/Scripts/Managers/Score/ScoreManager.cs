using Firat0667.WesternRoyaleLib.Key;
using Firat0667.WesternRoyaleLib.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Score
{
    public class ScoreManager : FoundationSingleton<ScoreManager>, IFoundationSingleton
    {
        public bool Initialized { get; set; }

        private readonly Dictionary<int, int> _scores = new();
        private readonly Dictionary<int, string> _names = new();
        private readonly Dictionary<int, bool> _deadStates = new();

        public BasicSignal<int, int> OnScoreChanged;

        private void Awake()
        {
            OnScoreChanged = new BasicSignal<int, int>();
        }

        public void ResetAll()
        {
            _scores.Clear();
            _names.Clear();
            _deadStates.Clear();
        }

        public void AddScore(BaseEntity entity, int amount)
        {
            if (entity == null) return;

            int id = entity.GetInstanceID();

            _names[id] = entity.name;
            _deadStates[id] = entity.IsDead;

            _scores.TryGetValue(id, out int current);
            current += Mathf.Max(0, amount);
            _scores[id] = current;

            OnScoreChanged.Emit(id, current);
        }

        public void MarkDead(BaseEntity entity)
        {
            if (entity == null) return;

            int id = entity.GetInstanceID();
            _deadStates[id] = true;
        }

        public struct RankingEntry
        {
            public int Id;
            public string Name;
            public int Score;
            public bool IsDead;
        }

        public List<RankingEntry> GetRanking()
        {
            var list = new List<RankingEntry>(_scores.Count);

            foreach (var kv in _scores)
            {
                int id = kv.Key;
                int score = kv.Value;

                _names.TryGetValue(id, out var name);
                _deadStates.TryGetValue(id, out var dead);

                list.Add(new RankingEntry
                {
                    Id = id,
                    Name = string.IsNullOrEmpty(name) ? $"Player {id}" : name,
                    Score = score,
                    IsDead = dead
                });
            }

            list.Sort((a, b) => b.Score.CompareTo(a.Score));

            return list;
        }
        public int GetScore(BaseEntity entity)
        {
            if (entity == null) return 0;

            int id = entity.GetInstanceID();
            _scores.TryGetValue(id, out int score);
            return score;
        }
    }
}