using Game.UI;
using Helper.MatchResults;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameOverView : UIBase
    {
        [SerializeField] private TextMeshProUGUI m_rewardText;
        [SerializeField] private Button m_replayButton;
        [SerializeField] private Button m_mainMenu;

        private GameLoopController m_gameloopController;
        private void OnEnable()
        {
            m_replayButton.onClick.AddListener(Replay);
            m_mainMenu.onClick.AddListener(MainMenu);

            m_gameloopController = FindObjectOfType<GameLoopController>();
            if (m_gameloopController != null)
            {
                var result = m_gameloopController.LastMatchResult;
                m_rewardText.text = $"+{result.Reward}";
            }
        }
        public void Replay()
        {
            m_gameloopController.RestartGame();
        }
        public void MainMenu()
        {
            GameStateManager.Instance.SetState(GameState.MainMenu);
        }
    }
}
