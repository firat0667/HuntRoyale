using Subsystems;
using Subsystems.Ai;
using UnityEngine;


namespace States.EnemyStates
{
    public class EnemyAttackState : IState
    {
        private readonly Enemy m_enemy;
        private readonly AINavigationSubsystem m_navigation;
        private readonly AttackSubsystem m_attack;

        public EnemyAttackState(Enemy enemy)
        {
            m_enemy = enemy;
            m_navigation = enemy.Navigation;
            m_attack = enemy.Attack;
        }
        public void Enter()
        {
            m_navigation.Stop();
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

            if (m_attack.TryAttack())
                m_enemy.AnimatorBridge.TriggerAttack();
        }

        public void PhysicsUpdate()
        {

        }
    }
}
