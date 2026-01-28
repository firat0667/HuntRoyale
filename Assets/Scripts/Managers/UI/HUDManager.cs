using Firat0667.CaseLib.Patterns;
using UnityEngine;

namespace Managers.UI
{
    public class HUDManager : FoundationSingleton<HUDManager>, IFoundationSingleton
    {
        public bool Initialized { get; set; }

        [SerializeField] private Joystick m_movementJoyStick;
        public Joystick MovementJoyStick => m_movementJoyStick;

    }

}




