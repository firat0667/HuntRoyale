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
            if (m_player.AllowWalkAttack)
                m_movement.SetSpeedMultiplier(m_attackMoveMultiply);
            else
                m_movement.SetSpeedMultiplier(1f);
        }
        public void LogicUpdate()
        {
            Vector2 input = m_playerInput.MoveInput;
            bool hasInput = input.sqrMagnitude > 0.1f;

            Vector3 moveDir = hasInput
                ? new Vector3(input.x, 0f, input.y)
                : Vector3.zero;

            m_movement.SetMoveDirection(moveDir);

            var target = m_attack.CurrentTarget;

            if (!m_player.AllowWalkAttack && hasInput)
            {
                m_movement.RotateTowards(moveDir);

                if (!m_player.IsInCombat)
                    m_player.SM.ChangeState(m_player.MoveState);

                return;
            }
            if (target != null)
            {
                Vector3 dir = target.position - m_player.transform.position;
                dir.y = 0f;
                m_movement.RotateTowards(dir);
            }
            else if (hasInput)
            {
                m_movement.RotateTowards(moveDir);
            }

            if (!hasInput || m_player.AllowWalkAttack)
            {
                if (m_attack.TryAttack())
                    m_player.AnimatorBridge.TriggerAttack();
            }

            if (!m_player.IsInCombat)
            {
                if (hasInput)
                    m_player.SM.ChangeState(m_player.MoveState);
                else
                    m_player.SM.ChangeState(m_player.IdleState);
            }
        }

        public void Exit() { m_movement.SetSpeedMultiplier(1f); }
        public void PhysicsUpdate() { }
    }
}
