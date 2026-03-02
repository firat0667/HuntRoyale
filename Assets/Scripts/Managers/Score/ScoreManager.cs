using Firat0667.WesternRoyaleLib.Key;
using Firat0667.WesternRoyaleLib.Patterns;
using System.Collections.Generic;
using System.Linq;
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
        public BasicSignal OnRankingChanged;

        private readonly Dictionary<int, int> _deathOrder = new();
        private int _deathCounter = 0;

        private void Awake()
        {
            OnScoreChanged = new BasicSignal<int, int>();
            OnRankingChanged = new BasicSignal();
        }

        public void ResetAll()
        {
            _scores.Clear();
            _names.Clear();
            _deadStates.Clear();

            _deathOrder.Clear();     
            _deathCounter = 0;       
        }

        public void AddScore(BaseEntity entity, int amount)
        {
            if (GameStateManager.Instance.GetCurrentState() != GameState.Playing) return;

            if (entity == null) return;

            EnsureEntry(entity);

            int id = entity.GetInstanceID();
            _scores.TryGetValue(id, out int current);
            current += Mathf.Max(0, amount);
            _scores[id] = current;

            OnScoreChanged.Emit(id, current);
        }

        public void MarkDead(BaseEntity entity)
        {
            if (GameStateManager.Instance.GetCurrentState() != GameState.Playing)
                return;
            if (entity == null) return;

            EnsureEntry(entity);

            int id = entity.GetInstanceID();
            _deadStates[id] = true;

            _deathCounter++;
            _deathOrder[id] = _deathCounter;
            OnRankingChanged.Emit();
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
            var list = new List<RankingEntry>(_names.Count);

            foreach (var kv in _names)
            {
                int id = kv.Key;

                _scores.TryGetValue(id, out int score);
                _deadStates.TryGetValue(id, out bool dead);

                list.Add(new RankingEntry
                {
                    Id = id,
                    Name = kv.Value,
                    Score = score,
                    IsDead = dead
                });
            }

            return list
                .OrderBy(x => x.IsDead ? 1 : 0)
                .ThenByDescending(x => !x.IsDead ? x.Score : 0)
                .ThenByDescending(x => x.IsDead && _deathOrder.ContainsKey(x.Id)
                    ? _deathOrder[x.Id]
                    : int.MinValue)
                .ToList();
        }
        public void RegisterParticipant(BaseEntity entity)
        {
            if (entity == null) return;

            int id = entity.GetInstanceID();

            _names[id] = entity.name;

            if (!_scores.ContainsKey(id))
                _scores[id] = 0;
            if (!_deadStates.ContainsKey(id))
                _deadStates[id] = entity.IsDead;
        }

        private void EnsureEntry(BaseEntity entity)
        {
            if (entity == null) return;
            RegisterParticipant(entity);
        }
        public int GetScore(BaseEntity entity)
        {
            if (entity == null) return 0;

            int id = entity.GetInstanceID();
            _scores.TryGetValue(id, out int score);
            return score;
        }
        public int GetPlayerRank(BaseEntity player)
        {
            var ranking = GetRanking();
            int playerId = player.GetInstanceID();

            for (int i = 0; i < ranking.Count; i++)
            {
                if (ranking[i].Id == playerId)
                    return i + 1;
            }

            return -1;
        }
    }
}