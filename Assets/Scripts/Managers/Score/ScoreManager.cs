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

        public BasicSignal<int, int> OnScoreChanged;
        private void Awake()
        {
            OnScoreChanged = new BasicSignal<int, int>();
        }

        public void ResetAll() => _scores.Clear();

        public void AddScore(BaseEntity entity, int amount)
        {
            if (entity == null) return;

            int id = entity.GetInstanceID();
            _scores.TryGetValue(id, out int current);
            current += Mathf.Max(0, amount);
            _scores[id] = current;

            OnScoreChanged.Emit(id, current);
        }
        public int GetScore(BaseEntity entity)
        {
            if (entity == null) return 0;
            _scores.TryGetValue(entity.GetInstanceID(), out int value);
            return value;
        }
        public IReadOnlyDictionary<int, int> GetAllScores() => _scores;
    }
}
