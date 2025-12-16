using UnityEngine;

public class PlayerIdleState : IState
{
    private Player m_player;
    private PlayerInputSubsystem m_input;

    public PlayerIdleState(Player player)
    {
        m_player = player;
        m_input = player.GetSubsystem<PlayerInputSubsystem>();
    }

    public void Enter()
    {
        Debug.Log("Entered Idle State");
    }

    public void LogicUpdate()
    {
        if (m_input.MoveInput.sqrMagnitude > 0.1f)
        {
            m_player.SM.ChangeState(m_player.MoveState);
            Debug.Log("Transitioning to Move State from Idle State");
        }
    }

    public void PhysicsUpdate() { }
    public void Exit() { }
}
