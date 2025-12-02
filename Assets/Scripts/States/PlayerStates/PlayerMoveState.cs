

using UnityEngine;

public class PlayerMoveState : IState
{
    private Player _player;
    private PlayerInputSubsystem _input;

    public PlayerMoveState(Player player)
    {
        _player = player;
        _input = player.GetSubsystem<PlayerInputSubsystem>();
    }

    public void Enter()
    {
      Debug.Log("Entered Move State");
    }

    public void LogicUpdate()
    {
        if (_input.MoveInput.sqrMagnitude < 0.1f)
        {
            _player.SM.ChangeState(_player.IdleState);
            return;
        }
    }

    public void PhysicsUpdate() { } 
    public void Exit() { }
}
