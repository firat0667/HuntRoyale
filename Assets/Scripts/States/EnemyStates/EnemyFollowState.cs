using Subsystems;
using Subsystems.Ai;
using UnityEngine;
using UnityEngine.UI;


namespace States.EnemyStates
{
    public class EnemyFollowState : IState
    {
        private readonly Enemy m_enemy;
        private readonly AINavigationSubsystem m_navigation;
        private readonly AttackSubsystem m_attack;

        public EnemyFollowState(Enemy enemy)
        {
            m_enemy = enemy;
            m_attack = enemy.Attack;
            m_navigation = enemy.Navigation;
        }
        public void Enter()
        {
            Transform target = m_attack.CurrentTarget;
            if (target == null)
                return;

            float engageRange = m_attack.EffectiveAttackRange > 0
            ? m_attack.EffectiveAttackRange
            : m_attack.AttackStartRange;

            m_navigation.SetStopDistance(engageRange);
            m_navigation.SetTarget(target);
        }

        public void Exit()
        {
            m_navigation.Stop();
            
        }

        public void LogicUpdate()
        {
            
            Transform target = m_attack.CurrentTarget;

            if (target == null)
            {
                m_enemy.SM.ChangeState(m_enemy.IdleState);
                return;
            }

            if (m_attack.IsTargetInAttackRange)
            {
                m_enemy.SM.ChangeState(m_enemy.AttackState);
                return;
            }
        }

        public void PhysicsUpdate()
        {

        }
    }
}
