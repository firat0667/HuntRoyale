using UnityEngine;

public class PlayerInputSubsystem : Subsystem
{
    private PlayerInputCore _core;

    public Vector2 MoveInput => _core.MoveInput;
    private void Start()
    {
        GetCoreComponent(ref _core);
    }
}
