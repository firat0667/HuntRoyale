using AI.Agents;
using Subsystems;
using Subsystems.Ai;
using UnityEngine;

namespace States.AgentStates
{
    public class BotIdleState : IState
    {
        private Agent m_agent;
        private AINavigationSubsystem m_navigation;
        private AttackSubsystem m_attack;

        public BotIdleState(Agent agent)
        {
            m_agent = agent;
            m_navigation =agent.Navigation;
            m_attack = agent.Attack;
        }
        public void Enter()
        {
            Debug.Log("Enter Idle State");
            m_navigation.Stop();
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

            if (m_agent.Brain.HasTarget)
            {
                m_agent.SM.ChangeState(m_agent.ChaseState);
            }
        }

        public void PhysicsUpdate()
        {
        }

    }

}
