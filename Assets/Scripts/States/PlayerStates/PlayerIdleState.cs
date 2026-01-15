using System;
using UnityEngine;

public class PlayerIdleState : IState
{
    private readonly Player m_player;
    private readonly PlayerInputSubsystem m_input;
    private readonly StateMachine m_stateMachine;
    public PlayerIdleState(Player player)
    {
        m_player = player;
        m_input = player.GetSubsystem<PlayerInputSubsystem>();
        m_stateMachine = player.SM;
    }

    public void Enter()
    {
        m_player.AnimatorBridge.SetSpeed(0f);
        Debug.Log("Entering Player Idle State");
    }

    public void LogicUpdate()
    {
        if (m_player.CombatPerception.HasTargetInAttackRange)
        {
            m_stateMachine.ChangeState(m_player.AttackState);
            return;
        }
        if (m_input.MoveInput.sqrMagnitude > 0.1f)
        {
            m_player.SM.ChangeState(m_player.MoveState);
            Debug.Log("Transitioning to Move State from Idle State");
        }
    }

    public void PhysicsUpdate() { }
    public void Exit() { }
}
