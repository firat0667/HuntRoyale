using DG.Tweening;
using Managers.Score;
using System.Collections.Generic;
using System.Linq;
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
            ScoreManager.Instance.OnScoreChanged.Connect(OnScoreChanged);
            Refresh();
        }

        private void OnDisable()
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.OnScoreChanged.Disconnect(OnScoreChanged);
        }

        private void OnScoreChanged(int id, int score)
        {
            Refresh();
        }

        private void Refresh()
        {
            var ranking = ScoreManager.Instance.GetRanking();
            UpdateFromRanking(ranking);
        }

        private void UpdateFromRanking(List<ScoreManager.RankingEntry> entries)
        {
            var validIds = new HashSet<int>(entries.Select(e => e.Id));
            var keysToRemove = new List<int>();

            foreach (var kvp in m_rows)
            {
                if (!validIds.Contains(kvp.Key))
                {
                    Destroy(kvp.Value.gameObject);
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                m_rows.Remove(key);
                m_previousIndexes.Remove(key);
            }

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                int id = entry.Id;

                if (!m_rows.ContainsKey(id))
                {
                    var row = Instantiate(m_rowPrefab, m_contentRoot);
                    m_rows.Add(id, row);
                    row.Init(entry.Name);
                }

                var currentRow = m_rows[id];
                currentRow.SetScore(entry.Score);

                if (entry.IsDead)
                    currentRow.OnEntityDied();
                else
                    currentRow.SetAliveStyleBackToOriginal();
            }

            AnimateReorder(entries.Select(e => e.Id).ToList());
        }

        private void AnimateReorder(List<int> orderedIds)
        {
            for (int i = 0; i < orderedIds.Count; i++)
            {
                int id = orderedIds[i];

                if (!m_rows.ContainsKey(id))
                    continue;

                var row = m_rows[id];
                var rect = row.GetComponent<RectTransform>();

                Vector2 targetPos = new Vector2(0, -i * rowHeight);

                int oldIndex = m_previousIndexes.ContainsKey(id) ? m_previousIndexes[id] : i;
                int newIndex = i;

                rect.DOKill(true);
                rect.DOAnchorPos(targetPos, 0.45f).SetEase(Ease.InOutCubic);

                if (newIndex < oldIndex)
                {
                    DOTween.Sequence()
                        .Append(rect.DOPunchScale(Vector3.one * 0.12f, 0.25f, 1, 0.4f))
                        .Join(rect.DOPunchAnchorPos(Vector2.up * 15f, 0.3f, 1, 0.5f));
                }
                else if (newIndex > oldIndex)
                {
                    DOTween.Sequence()
                        .Append(rect.DOScale(0.92f, 0.15f))
                        .Append(rect.DOScale(1f, 0.2f));
                }

                m_previousIndexes[id] = newIndex;
            }
        }
    }
}