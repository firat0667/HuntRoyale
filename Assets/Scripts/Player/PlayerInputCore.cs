using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputCore : CoreComponent
{
    public Vector2 MoveInput { get; private set; }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

}
