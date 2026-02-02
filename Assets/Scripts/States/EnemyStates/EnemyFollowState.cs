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
        private readonly MovementSubsystem m_move;

        public EnemyFollowState(Enemy enemy)
        {
            m_enemy = enemy;
            m_attack = enemy.Attack;
            m_navigation = enemy.Navigation;
            m_move=enemy.Movement;
        }
        public void Enter()
        {
            Transform target = m_attack.CurrentTarget;
            if (target == null) return;
            m_navigation.SetTarget(target);
        }


        public void Exit()
        {
            m_navigation.Stop();
            
        }

        public void LogicUpdate()
        {
            if (!m_enemy.IsTargetInDetectRange)
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
