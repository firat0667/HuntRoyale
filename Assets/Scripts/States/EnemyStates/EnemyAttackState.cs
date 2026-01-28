using Subsystems;
using UnityEngine;


namespace States.EnemyStates
{
    public class EnemyAttackState : IState
    {
        private readonly Enemy m_enemy;
        private readonly MovementSubsystem m_movement;
        private readonly AttackSubsystem m_attack;

        public EnemyAttackState(Enemy enemy)
        {
            m_enemy = enemy;
            m_movement = enemy.Movement;
            m_attack = enemy.Attack;
        }
        public void Enter()
        {
            m_movement.Stop();
        }

        public void Exit()
        {

        }

        public void LogicUpdate()
        {
            if (!m_enemy.HasTarget)
            {
                m_enemy.SM.ChangeState(m_enemy.IdleState);
                return;
            }

            if (!m_enemy.IsTargetInAttackRange)
            {
                m_enemy.SM.ChangeState(m_enemy.FollowState);
                return;
            }

            Transform target = m_attack.CurrentTarget;
            Vector3 dir = target.position - m_enemy.transform.position;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.001f)
                m_movement.RotateTowards(dir);

            if (m_attack.TryAttack())
                m_enemy.AnimatorBridge.TriggerAttack();
        }

        public void PhysicsUpdate()
        {

        }
    }
}
