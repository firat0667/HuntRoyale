using Pathfinding;
using UnityEngine;
using Game;

namespace Subsystems.Ai
{
    public class AINavigationSubsystem : Subsystem
    {
        private AIPath m_aiPath;
        private AIDestinationSetter m_destinationSetter;
        private Transform m_target;

        private float _stopDistance;

        private MovementSubsystem m_movement;

        protected override void Awake()
        {
            base.Awake();
            m_movement = Entity.GetSubsystem<MovementSubsystem>();
            m_aiPath = Entity.GetComponent<AIPath>();
            m_destinationSetter= Entity.GetComponent<AIDestinationSetter>();
            m_aiPath.gravity = Vector3.zero;
            m_aiPath.canMove = false;
            m_aiPath.canSearch = false;
            m_aiPath.rotationSpeed = StatsComponent.RotationSpeed;
        }
        public void SetTarget(Transform target)
        {
            m_target = target;

            if (m_target == null)
            {
                Stop();
                return;
            }
            m_aiPath.maxSpeed = m_movement.MaxSpeed;
            m_aiPath.canMove = true;
            m_aiPath.canSearch = true;
            m_destinationSetter.target = target;
        }
 
        public void SetStopDistance(float distance)
        {
            _stopDistance = distance;

        }

        public void Stop()
        {
            m_aiPath.canMove = false;
            m_aiPath.canSearch = false;
            m_target = null;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (m_target == null)
                return;

            if (m_movement != null)
            {
                m_aiPath.maxSpeed = m_movement.CurrentSpeed;
            }

            float sqrDist =
                (Entity.transform.position - m_target.position).sqrMagnitude;

            if (sqrDist <= _stopDistance * _stopDistance)
            {
                m_aiPath.canMove = false;
                return;
            }

            m_aiPath.canMove = true;
        }
    }

}
