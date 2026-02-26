using DG.Tweening;
using Managers.Leaderboard;
using Managers.Score;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Leaderboard
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField] private RectTransform m_contentRoot;
        [SerializeField] private LeaderboardRow m_rowPrefab;
        [SerializeField] private float rowHeight = 70f;

        private Dictionary<int, LeaderboardRow> m_rows = new();
        private Dictionary<int, int> m_previousIndexes = new();

        private void Start()
        {
            var participants = LeaderboardManager.Instance.GetParticipants();
            var sorted = LeaderboardManager.Instance.Build(participants);
            ScoreManager.Instance.OnScoreChanged.Connect(OnScoreChanged);
            UpdateLeaderboard(sorted);
        }

        private void OnAnyEntityDied()
        {
            var participants = LeaderboardManager.Instance.GetParticipants();
            var sorted = LeaderboardManager.Instance.Build(participants);
            UpdateLeaderboard(sorted);
        }
        private void OnDisable()
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.OnScoreChanged.Disconnect(OnScoreChanged);

            var participants = LeaderboardManager.Instance.GetParticipants();

            foreach (var p in participants)
            {
                if (p == null) continue;       
                var health = p.Health;            
                if (health == null) continue;

                health.OnDied?.Disconnect(OnAnyEntityDied);
            }
        }
    
        private void OnScoreChanged(int id, int score)
        {
            var participants = LeaderboardManager.Instance.GetParticipants();
            var sorted = LeaderboardManager.Instance.Build(participants);
            UpdateLeaderboard(sorted);
        }
        public void UpdateLeaderboard(List<LeaderboardManager.LeaderboardEntry> entries)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                int id = entry.Entity.GetInstanceID();

                if (!m_rows.ContainsKey(id))
                {
                    var row = Instantiate(m_rowPrefab, m_contentRoot);
                    m_rows.Add(id, row);
                    row.Init(entry.Name);
                    if (entry.Entity.Health != null)
                        entry.Entity.Health.OnDied.Connect(OnAnyEntityDied);
                }

                var currentRow = m_rows[id];
                currentRow.SetScore(entry.Score);
                if (entry.Entity.IsDead)
                    currentRow.OnEntityDied();
                else
                    currentRow.SetAliveStyleBackToOriginal();
            }

            AnimateReorder(entries);
        }

        private void AnimateReorder(List<LeaderboardManager.LeaderboardEntry> entries)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                int id = entries[i].Entity.GetInstanceID();
                var row = m_rows[id];
                var rect = row.GetComponent<RectTransform>();

                Vector2 targetPos = new Vector2(0, -i * rowHeight);

                int oldIndex = m_previousIndexes.ContainsKey(id) ? m_previousIndexes[id] : i;
                int newIndex = i;
                rect.DOAnchorPos(targetPos, 0.45f)
                    .SetEase(Ease.InOutCubic);

                if (newIndex < oldIndex)
                {
                    Sequence seq = DOTween.Sequence();
                    seq.Append(rect.DOPunchScale(Vector3.one * 0.12f, 0.25f, 1, 0.4f));
                    seq.Join(rect.DOPunchAnchorPos(Vector2.up * 15f, 0.3f, 1, 0.5f));
                }
                else if (newIndex > oldIndex)
                {
                    Sequence seq = DOTween.Sequence();
                    seq.Append(rect.DOScale(0.92f, 0.15f));
                    seq.Append(rect.DOScale(1f, 0.2f));
                }

                m_previousIndexes[id] = newIndex;
            }
        }
    }
}
