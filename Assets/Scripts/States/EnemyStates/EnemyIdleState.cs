using Subsystems;
using Subsystems.Ai;
using UnityEngine;

namespace States.EnemyStates
{
    public class EnemyIdleState : IState
    {
        private readonly Enemy m_enemy;
        private readonly AINavigationSubsystem m_navigation;

        public EnemyIdleState(Enemy enemy)
        {
            m_enemy = enemy;
            m_navigation=enemy.Navigation;
        }

        public void Enter()
        {
            m_navigation.Stop();
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
