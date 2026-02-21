using Subsystems.Ai;
using Subsystems;
using UnityEngine;
using AI.Agents;

namespace States.AgentStates
{
    public class BotAttackState : IState
    {
        private Agent m_agent;
        private readonly AINavigationSubsystem m_navigation;
        private readonly AttackSubsystem m_attack;
        private readonly MovementSubsystem m_movement;
        public BotAttackState(Agent agent)
        {
            m_agent = agent;
            m_navigation = agent.Navigation;
            m_attack = agent.Attack;
            m_movement = agent.Movement;
        }
        public void Enter()
        {
            m_navigation.Stop();
            Debug.Log("Enter Attack State");
        }

        public void Exit()
        {
        }
        public void LogicUpdate()
        {
            if (m_agent.Brain.ShouldHeal)
            {
                m_agent.SM.ChangeState(m_agent.HealState);
                return;
            }

            if (!m_agent.Brain.HasTarget)
            {
                m_agent.SM.ChangeState(m_agent.IdleState);
                return;
            }

            if (!m_agent.Brain.InAttackRange)
            {
                m_agent.SM.ChangeState(m_agent.ChaseState);
            }
            if (m_attack.TryAttack())
                m_agent.AnimatorBridge.TriggerAttack();

            Transform target = m_agent.Brain.CurrentTarget;
            Vector3 dir = target.position - m_agent.transform.position;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.001f)
                m_movement.RotateTowards(dir);
        }


        public void PhysicsUpdate()
        {
        }
    }
}
