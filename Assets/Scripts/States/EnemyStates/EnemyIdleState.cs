using Subsystems;
using UnityEngine;

namespace States.EnemyStates
{
    public class EnemyIdleState : IState
    {
        private readonly Enemy m_enemy;
        private readonly MovementSubsystem m_movement;
        private readonly AttackSubsystem m_attack;

        public EnemyIdleState(Enemy enemy)
        {
            m_enemy = enemy;
            m_movement = enemy.Movement;
            m_attack = enemy.Attack;
        }

        public void Enter()
        {
            m_movement.Stop();
        }

        public void LogicUpdate()
        {
            if (m_enemy.IsTargetInAttackRange)
            {
                m_enemy.SM.ChangeState(m_enemy.AttackState);
                return;
            }

            if (m_enemy.IsTargetInDetectRange)
            {
                m_enemy.SM.ChangeState(m_enemy.FollowState);
                return;
            }
        }

        public void PhysicsUpdate() { }
        public void Exit() { }
    }
}
