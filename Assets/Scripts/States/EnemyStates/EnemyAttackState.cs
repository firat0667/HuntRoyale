using Subsystems;
using Subsystems.Ai;
using UnityEngine;
using AI.Enemies;
using Managers.Enemies;
using NUnit.Framework.Internal;

namespace States.EnemyStates
{
    public class EnemyAttackState : IState
    {
        private readonly Enemy m_enemy;
        private readonly AINavigationSubsystem m_navigation;
        private readonly AttackSubsystem m_attack;
        private readonly MovementSubsystem m_movement;

        public EnemyAttackState(Enemy enemy)
        {
            m_enemy = enemy;
            m_navigation = enemy.Navigation;
            m_movement=enemy.Movement;
            m_attack = enemy.Attack;
        }
        public void Enter()
        {
            m_navigation.Stop();
            
        }

        public void Exit()
        {
            m_attack.Perception.ClearTarget();
        }

        public void LogicUpdate()
        {
            if (!m_enemy.HasTarget)
            {
                m_enemy.SM.ChangeState(m_enemy.IdleState);
                return;
            }

            if (!m_enemy.IsTargetInAttackRange || !m_attack.CanAttack())
            {
                m_enemy.SM.ChangeState(m_enemy.FollowState);
                return;
            }


            if (m_attack.TryAttack())
                m_enemy.AnimatorBridge.TriggerAttack();

            Transform target = m_attack.CurrentTarget;
            Vector3 dir = target.position - m_enemy.transform.position;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.001f)
                m_movement.RotateTowards(dir);
        }

        public void PhysicsUpdate()
        {

        }
    }
}
