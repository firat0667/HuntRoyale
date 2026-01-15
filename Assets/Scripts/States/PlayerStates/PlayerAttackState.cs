using UnityEngine;

public class PlayerAttackState : IState
{
    private readonly Player m_player;
    private readonly AttackSubsystem m_attack;
    private readonly MovementSubsystem m_movement;

    private const float ATTACK_MOVE_MULT = 0.5f;

    public PlayerAttackState(Player player)
    {
        m_player = player;
        m_attack = player.GetSubsystem<AttackSubsystem>();
        m_movement = player.GetSubsystem<MovementSubsystem>();
    }

    public void Enter()
    {
        Debug.Log("Entering Player Attack State");
        m_movement.SetSpeedMultiplier(ATTACK_MOVE_MULT);
    }

    public void LogicUpdate()
    {

        var target = m_player.CombatPerception.CurrentTarget;
        if (target != null)
        {
            Vector3 dir = target.position - m_player.transform.position;
            m_movement.RotateTowards(dir);
        }

        if (m_attack.TryAttack())
        {
            m_player.AnimatorBridge.TriggerAttack();
        }
        if (!m_player.IsInCombat)
        {
            if (m_movement.Velocity.sqrMagnitude > 0.01f)
                m_player.SM.ChangeState(m_player.MoveState);
            else
                m_player.SM.ChangeState(m_player.IdleState);
        }
    }

    public void Exit() { m_movement.SetSpeedMultiplier(1f); }
    public void PhysicsUpdate() { }
}
