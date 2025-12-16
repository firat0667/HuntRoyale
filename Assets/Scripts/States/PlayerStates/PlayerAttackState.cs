using UnityEngine;

public class PlayerAttackState : IState
{
    private readonly Player m_player;
    private readonly PlayerInputCore m_input;
    private readonly AttackSubsystem m_attack;

    public PlayerAttackState(Player player)
    {
        m_player = player;
        m_input = player.GetComponentInChildren<PlayerInputCore>();
        m_attack = player.GetSubsystem<AttackSubsystem>();
    }

    public void Enter()
    {
        // İstersen burada attack animasyon trigger’ı atarsın
        // Örn: _animator.SetBool("IsAttacking", true);
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
        // Bu state'te fiziksel hareket yapmıyoruz.
        // Hareket işini MovementSubsystem zaten hallediyor.
    }

    public void Exit()
    {
        // İstersen burada attack animasyon flag’ini kapatırsın
        // Örn: _animator.SetBool("IsAttacking", false);
    }
}
