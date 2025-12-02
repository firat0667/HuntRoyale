using UnityEngine;

public class PlayerIdleState : IState
{
    private Player _player;
    private PlayerInputSubsystem _input;

    public PlayerIdleState(Player player)
    {
        _player = player;
        _input = player.GetSubsystem<PlayerInputSubsystem>();
    }

    public void Enter()
    {
        Debug.Log("Entered Idle State");
    }

    public void LogicUpdate()
    {
        if (_input.MoveInput.sqrMagnitude > 0.1f)
        {
            _player.SM.ChangeState(_player.MoveState);
        }
    }

    public void PhysicsUpdate() { }
    public void Exit() { }
}
