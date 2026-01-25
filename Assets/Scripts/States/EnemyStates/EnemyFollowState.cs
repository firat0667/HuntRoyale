using Subsystems;
using UnityEngine;


namespace States.EnemyStates
{
    public class EnemyFollowState : IState
    {
        private readonly Enemy m_enemy;
        private readonly MovementSubsystem m_movement;
        private readonly AttackSubsystem m_attack;

        public EnemyFollowState(Enemy enemy)
        {
            m_enemy = enemy;
            m_movement = enemy.Movement;
            m_attack = enemy.Attack;
        }


        public void Enter()
        {
            m_enemy.AnimatorBridge.SetSpeed(1f);
        }

        public void Exit()
        {
            m_movement.Stop();
        }

        public void LogicUpdate()
        {
            Transform target = m_attack.CurrentTarget;

            if (target == null)
            {
                m_enemy.SM.ChangeState(m_enemy.IdleState);
                return;
            }

            Vector3 dir = target.position - m_enemy.transform.position;
            dir.y = 0f;

            float sqrDist = dir.sqrMagnitude;

            if (m_attack.IsTargetInAttackRange)
            {
                m_movement.Stop();
                m_enemy.SM.ChangeState(m_enemy.AttackState);
                return;
            }

            if (sqrDist > 0.01f)
            {
                dir.Normalize();
                m_movement.SetMoveDirection(dir);
                m_movement.RotateTowards(dir);
            }
            else
            {
                m_movement.Stop();
            }
        }

        public void PhysicsUpdate()
        {

        }
    }
}
