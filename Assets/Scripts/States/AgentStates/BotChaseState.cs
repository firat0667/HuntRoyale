using Subsystems.Ai;
using Subsystems;
using UnityEngine;
using AI.Agents;

namespace States.AgentStates
{
    public class BotChaseState : IState
    {
        private Agent m_agent;
        private AINavigationSubsystem m_navigation;
        private AttackSubsystem m_attack;
        private readonly MovementSubsystem m_move;
        public BotChaseState(Agent agent)
        {
            m_agent = agent;
            m_navigation = agent.Navigation;
            m_attack = agent.Attack;
            m_move = agent.Movement;
        }
        public void Enter()
        {
            Debug.Log("Enter Chase State");

            Transform target = m_attack.CurrentTarget;
            if (target == null) return;
            m_navigation.SetTarget(target);
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

            if (m_agent.Brain.InAttackRange)
            {
                m_agent.SM.ChangeState(m_agent.AttackState);
            }
        }
        public void PhysicsUpdate()
        {
        }
    }
}
