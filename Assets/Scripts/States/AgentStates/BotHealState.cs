using AI.Agents;
using Subsystems;
using Subsystems.Ai;
using UnityEngine;
using UnityEngine.UI;

namespace States.AgentStates
{
    public class BotHealState : IState
    {
        private Agent m_agent;
        private AINavigationSubsystem m_navigation;
        private AttackSubsystem m_attack;


        public BotHealState(Agent agent)
        {
            m_agent = agent;
            m_navigation = agent.Navigation;
            m_attack = agent.Attack;
        }

        public void Enter()
        {
            Transform healZone = m_agent.Brain.GetHealZone();

            if (healZone != null)
            {
                m_navigation.SetTarget(healZone);
            }
        }

        public void Exit()
        {
        }

        public void LogicUpdate()
        {
            if (!m_agent.Brain.ShouldHeal)
            {
                m_agent.SM.ChangeState(m_agent.IdleState);
                return;
            }
        }

        public void PhysicsUpdate()
        {
        }

    }
}
