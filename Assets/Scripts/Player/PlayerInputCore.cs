using Managers.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Subsystems.CoreComponents
{
    public class PlayerInputCore : CoreComponent
    {
        public Vector2 MoveInput { get; private set; }

        private Vector2 actionInput;
        private Joystick mobileJoystick;

        private void Start()
        {
            mobileJoystick = HUDManager.Instance.MovementJoyStick;
        }
        public void OnMove(InputAction.CallbackContext ctx)
        {
            actionInput = ctx.ReadValue<Vector2>();
        }

        public override void LogicUpdate()
        {
            if (mobileJoystick != null)
            {
                Vector2 joy = new Vector2(
                    mobileJoystick.Horizontal,
                    mobileJoystick.Vertical
                );

                if (joy.sqrMagnitude > 0.01f)
                {
                    MoveInput = joy;
                    return;
                }
            }

            MoveInput = actionInput;
        }
    }
}
