using System.Collections.Generic;
using UnityEngine;
using Managers.Score;

namespace UI.Leaderboard
{
    public class FinalLeaderboardPresenter : MonoBehaviour
    {
        [SerializeField] private Transform m_content;
        [SerializeField] private LeaderboardRow m_rowPrefab;

        private readonly List<LeaderboardRow> m_rows = new();

        private void OnEnable()
        {
            Build();
        }
        private void Build()
        {
            Clear();

            var ranking = ScoreManager.Instance.GetRanking();

            foreach (var entry in ranking)
            {
                var row = Instantiate(m_rowPrefab, m_content);

                row.Init(entry.Name);
                row.SetScore(entry.Score);

                if (entry.IsDead)
                    row.OnEntityDied();
                else
                    row.SetAliveStyleBackToOriginal();

                m_rows.Add(row); 
            }
        }

        private void Clear()
        {
            foreach (var row in m_rows)
                if (row != null)
                    Destroy(row.gameObject);

            m_rows.Clear();
        }
    }
}