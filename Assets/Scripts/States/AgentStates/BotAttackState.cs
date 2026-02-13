using UnityEngine;

namespace States.AgentStates
{
    public class BotAttackState : IState
    {
        private Agent m_agent;

        public BotAttackState(Agent agent)
        {
            m_agent = agent;
        }
        public void Enter()
        {
            m_agent.Input.SetAttack(true);
            m_agent.Animator.TriggerAttack();
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
        }


        public void PhysicsUpdate()
        {
        }
    }
}
