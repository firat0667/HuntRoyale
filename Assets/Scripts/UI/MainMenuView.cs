using Game.UI;
using MainMenu.CharacterSelection;
using Managers.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace MainMenu.UI
{
    public class MainMenuView : UIBase
    {
        #region UI
        [SerializeField] private TextMeshProUGUI m_goldText;
        [SerializeField] private Button m_playButton;
        [SerializeField] private Button m_exitButton;
        #endregion

        [SerializeField] private CharacterSelectionManager m_characterSelection;
        private GameLoopController m_gameLoopController;
        private void Start()
        {
            GameManager.Instance.GoldChanged.Connect(GoldChanged);

            m_playButton.onClick.AddListener(OnPlayButton);
            m_exitButton.onClick.AddListener(OnExitButton);
            m_gameLoopController = FindObjectOfType<GameLoopController>();
        }
        private void OnDisable()
        {
            GameManager.Instance.GoldChanged.Disconnect(GoldChanged);
        }
        public void GoldChanged(int currentGold)
        {
            m_goldText.text = currentGold.ToString();
        }
        public void OnPlayButton()
        {
            var selectedData = m_characterSelection.GetSelectedCharacter();

            if (selectedData == null || selectedData.gameplayPrefab == null)
            {
                Debug.LogError("Selected character prefab is null!");
                return;
            }

            m_gameLoopController.SetPlayerPrefab(selectedData.gameplayPrefab);
            m_gameLoopController.StartMatch();
        }
        public void OnExitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }
}

