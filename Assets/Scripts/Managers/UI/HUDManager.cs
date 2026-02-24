using DG.Tweening;
using Firat0667.WesternRoyaleLib.Patterns;
using Game.UI;
using Subsystems;
using UI.Game;
using UnityEngine;

namespace Managers.UI
{
    public class HUDManager : FoundationSingleton<HUDManager>, IFoundationSingleton
    {
        public bool Initialized { get; set; }

        [SerializeField] private HUDView m_hudView;
        public Joystick MovementJoyStick => m_hudView.Joystick;
        public LevelHUD LevelHUD => m_hudView.LevelHUD;
        public LevelUpPanel LevelUpPanel => m_hudView.LevelUpPanel;
        public GameTimer GameTimer => m_hudView.GameTimer;

        private void OnEnable()
        {
            EventManager.Instance.Subscribe(EventTags.EVENT_PLAYER_SPAWNED, OnPlayerSpawned);
        }
        private void OnDisable()
        {
            if (EventManager.Instance != null)
                EventManager.Instance.Unsubscribe(EventTags.EVENT_PLAYER_SPAWNED, OnPlayerSpawned);
        }
        private void OnPlayerSpawned(object obj)
        {
            var player = obj as Player;
            if (player == null) return;

            var exp = player.GetSubsystem<ExperienceSubsystem>();
            if (exp == null) return;

            LevelHUD.Bind(exp);
        }
    }

}




