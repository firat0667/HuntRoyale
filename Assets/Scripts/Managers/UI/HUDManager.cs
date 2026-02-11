using DG.Tweening;
using Firat0667.CaseLib.Patterns;
using Game.UI;
using UnityEngine;

namespace Managers.UI
{
    public class HUDManager : FoundationSingleton<HUDManager>, IFoundationSingleton
    {
        public bool Initialized { get; set; }

        [SerializeField] private HUDView m_hudView;
        public Joystick MovementJoyStick => m_hudView.Joystick;

    }

}




