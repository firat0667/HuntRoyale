using Subsystems;
using UnityEngine;


namespace States.PlayerStates
{
    public class PlayerAttackState : IState
    {
        private readonly Player m_player;
        private readonly AttackSubsystem m_attack;
        private readonly MovementSubsystem m_movement;
        private readonly PlayerInputSubsystem m_playerInput;
        private readonly float m_attackMoveMultiply;

        public PlayerAttackState(Player player)
        {
            m_player = player;
            m_attack = player.GetSubsystem<AttackSubsystem>();
            m_movement = player.GetSubsystem<MovementSubsystem>();
            m_playerInput = player.GetSubsystem<PlayerInputSubsystem>();
            m_attackMoveMultiply = m_movement.MoveAttackSpeedMult;
        }

        public void Enter()
        {
            Debug.Log("Entering Player Attack State");
            m_movement.SetSpeedMultiplier(m_attackMoveMultiply);
        }

        public void LogicUpdate()
        {
            Vector2 input = m_playerInput.MoveInput;

            Vector3 moveDir = Vector3.zero;
            if (input.sqrMagnitude > 0.1f)
                moveDir = new Vector3(input.x, 0f, input.y);

            m_movement.SetMoveDirection(moveDir);

            var target = m_attack.CurrentTarget;
            if (target != null)
            {
                Vector3 dir = target.position - m_player.transform.position;
                dir.y = 0f;
                m_movement.RotateTowards(dir);
            }
            else if (moveDir.sqrMagnitude > 0.001f)
            {
                m_movement.RotateTowards(moveDir);
            }
            if (m_attack.TryAttack())
                m_player.AnimatorBridge.TriggerAttack();

            if (!m_player.IsInCombat)
            {
                if (moveDir.sqrMagnitude > 0.01f)
                    m_player.SM.ChangeState(m_player.MoveState);
                else
                    m_player.SM.ChangeState(m_player.IdleState);
            }
        }


        public void Exit() { m_movement.SetSpeedMultiplier(1f); }
        public void PhysicsUpdate() { }
    }
}
