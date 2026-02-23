using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Leaderboard
{
    public class LeaderboardRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_nameText;
        [SerializeField] private TMP_Text m_scoreText;

        public void SetName(string name)
        {
            m_nameText.text = name;
        }

        public void SetScore(int score)
        {
            m_scoreText.text = score.ToString();
        }
    }
}
