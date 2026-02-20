using Game.UI;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Game.UI
{
    public class HUDView : UIBase
    {
        [Header("HUD Parts")]
        [SerializeField] private LevelHUD m_levelHUD;
        [SerializeField] private Joystick m_joystick;
        [SerializeField] private LevelUpPanel m_levelUpPanel;

        public Joystick Joystick => m_joystick;
        public LevelHUD LevelHUD => m_levelHUD;

        public LevelUpPanel LevelUpPanel => m_levelUpPanel;

        protected override void OnAfterShow()
        {
            m_joystick.gameObject.SetActive(true);
        }
        protected override void OnAfterHide()
        {
            m_joystick.gameObject.SetActive(false);
        }
    }
}