

using UnityEngine;

public class PlayerMoveState : IState
{
    private Player m_player;
    private PlayerInputSubsystem m_input;

    public PlayerMoveState(Player player)
    {
        m_player = player;
        m_input = player.GetSubsystem<PlayerInputSubsystem>();
    }

    public void Enter()
    {
      Debug.Log("Entered Move State");
    }

    public void LogicUpdate()
    {
        if (m_input.MoveInput.sqrMagnitude < 0.1f)
        {
            m_player.SM.ChangeState(m_player.IdleState);
            return;
        }
    }

    public void PhysicsUpdate() { } 
    public void Exit() { }
}
