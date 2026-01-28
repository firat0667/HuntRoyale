
using Subsystems;
using UnityEngine;


namespace States.PlayerStates
{
    public class PlayerMoveState : IState
    {
        private readonly Player m_player;
        private readonly PlayerInputSubsystem m_input;
        private readonly MovementSubsystem m_movement;
        private readonly AimSubsystem m_aim;
        private readonly StateMachine m_stateMachine;

        public PlayerMoveState(Player player)
        {
            m_player = player;
            m_input = player.GetSubsystem<PlayerInputSubsystem>();
            m_aim = player.GetSubsystem<AimSubsystem>();
            m_movement = player.GetSubsystem<MovementSubsystem>();
            m_stateMachine = player.SM;
        }

        public void Enter()
        {

        }

        public void LogicUpdate()
        {
            if (m_player.IsInCombat)
            {
                m_stateMachine.ChangeState(m_player.AttackState);
                return;
            }

            Vector2 input = m_input.MoveInput;

            if (input.sqrMagnitude < 0.1f)
            {
                m_movement.Stop();
                m_stateMachine.ChangeState(m_player.IdleState);
                return;
            }

            Vector3 moveDir = new Vector3(input.x, 0f, input.y);

            m_movement.SetMoveDirection(moveDir);

            if (m_movement.Velocity.sqrMagnitude > 0.001f)
                m_movement.RotateTowards(m_movement.Velocity);

            m_aim.SetAim(moveDir);
        }



        public void PhysicsUpdate() { }
        public void Exit() { }
    }
}
