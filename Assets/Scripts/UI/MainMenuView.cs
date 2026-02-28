using Game.UI;
using Managers.Game;
using TMPro;
using UnityEngine;


namespace MainMenu.UI
{
    public class MainMenuView : UIBase
    {
        [SerializeField] private TextMeshProUGUI m_goldText;
        private void Start()
        {
            GameManager.Instance.GoldChanged.Connect(GoldChanged);
        }
        private void OnDisable()
        {
            GameManager.Instance.GoldChanged.Disconnect(GoldChanged);
        }
        public void GoldChanged(int currentGold)
        {
            m_goldText.text = currentGold.ToString();
        }

    }
}

