

using UnityEngine;

public class PlayerMoveState : IState
{
    private readonly Player m_player;
    private readonly PlayerInputSubsystem m_input;
    private readonly MovementSubsystem m_movement;
    private readonly AimSubsystem m_aim;

    public PlayerMoveState(Player player)
    {
        m_player = player;
        m_input = player.GetSubsystem<PlayerInputSubsystem>();
        m_aim = player.GetSubsystem<AimSubsystem>();
        m_movement = player.GetSubsystem<MovementSubsystem>();
    }

    public void Enter()
    {
        m_player.AnimatorBridge.SetSpeed(1f);
    }

    public void LogicUpdate()
    {
        Vector3 moveDir = m_input.MoveInput;
        if (moveDir.sqrMagnitude < 0.1f)
        {
            m_player.SM.ChangeState(m_player.IdleState);
            return;
        }
        if (m_movement.Velocity.sqrMagnitude > 0.001f)
            m_movement.RotateTowards(m_movement.Velocity);
        m_aim.SetAim(moveDir);
    }

    public void PhysicsUpdate() { } 
    public void Exit() { }
}
