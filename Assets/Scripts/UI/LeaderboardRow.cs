using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Subsystems; // HealthSubsystem için

namespace UI.Leaderboard
{
    public class LeaderboardRow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_nameText;
        [SerializeField] private TextMeshProUGUI m_scoreText;
        [SerializeField] private Image m_background;

        private BaseEntity m_entity;

        private Color m_nameOriginalColor;
        private Color m_scoreOriginalColor;
        private Color m_bgOriginalColor;

        private void Awake()
        {
            m_nameOriginalColor = m_nameText.color;
            m_scoreOriginalColor = m_scoreText.color;
            if (m_background != null) m_bgOriginalColor = m_background.color;
        }

        public void Init(BaseEntity entity)
        {
            m_entity = entity;
            m_nameText.text = entity.name;

            if (m_entity.Health != null)
                m_entity.Health.OnDied.Connect(OnEntityDied);
        }

        private void OnDisable()
        {
            if (m_entity != null && m_entity.Health != null)
                m_entity.Health.OnDied.Disconnect(OnEntityDied);
        }

        private void OnEntityDied()
        {
            SetDeadStyle();
        }

        public void SetScore(int score)
        {
            m_scoreText.text = score.ToString();
        }

        private void SetDeadStyle()
        {
            m_nameText.color = Color.gray;
            m_scoreText.color = Color.gray;
            if (m_background != null) m_background.color = Color.gray * 0.6f;
        }

        public void SetAliveStyleBackToOriginal()
        {
            m_nameText.color = m_nameOriginalColor;
            m_scoreText.color = m_scoreOriginalColor;
            if (m_background != null) m_background.color = m_bgOriginalColor;
        }
    }
}