using Game.UI;
using UnityEngine;

namespace Game.UI
{
    public class HUDView : UIBase
    {
        [Header("HUD Parts")]
        //[SerializeField] private LevelHUD m_levelHUD;
        [SerializeField] private Joystick m_joystick;
        //[SerializeField] private LeaderboardHUD m_leaderboard;
        public Joystick Joystick => m_joystick;
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