using UnityEngine;

public class PlayerAttackState : IState
{
    private readonly Player _player;
    private readonly PlayerInputCore _input;
    private readonly AttackSubsystem _attack;

    public PlayerAttackState(Player player)
    {
        _player = player;
        _input = player.GetComponentInChildren<PlayerInputCore>();
        _attack = player.GetSubsystem<AttackSubsystem>();
    }

    public void Enter()
    {
        // İstersen burada attack animasyon trigger’ı atarsın
        // Örn: _animator.SetBool("IsAttacking", true);
    }

    public void LogicUpdate()
    {
        // 1) Hareket varsa → MoveState'e dön
        if (_input.MoveInput.sqrMagnitude > 0.1f)
        {
            _player.SM.ChangeState(_player.MoveState);
            return;
        }

        // 2) Henüz target sistemi yok → sadece attack çalışsın
        _attack.TryAttack();
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
