using UnityEngine;

public class PlayerAttackState : IState
{
    private readonly Player m_player;
    private readonly PlayerInputCore m_input;
    private readonly AttackSubsystem m_attack;
    private readonly MovementSubsystem m_movement;

    public PlayerAttackState(Player player)
    {
        m_player = player;
        m_input = player.GetComponentInChildren<PlayerInputCore>();
        m_attack = player.GetSubsystem<AttackSubsystem>();
        m_movement= player.GetSubsystem<MovementSubsystem>();
    }

    public void Enter()
    {
        Debug.Log("Entering Player Attack State");
        m_player.AnimatorBridge.TriggerAttack();
    }

    public void LogicUpdate()
    {
        // 1) Hareket varsa → MoveState'e dön
        if (m_input.MoveInput.sqrMagnitude > 0.1f)
        {
            m_player.SM.ChangeState(m_player.MoveState);
            return;
        }

        // 2) Henüz target sistemi yok → sadece attack çalışsın
        m_attack.TryAttack();

    }

    public void PhysicsUpdate()
    {
        //if (target == null) return;

        //Vector3 dir = target.position - m_player.transform.position;
        //m_movement.RotateTowards(dir);
    }

    public void Exit()
    {
        // İstersen burada attack animasyon flag’ini kapatırsın
        // Örn: _animator.SetBool("IsAttacking", false);
    }
}
