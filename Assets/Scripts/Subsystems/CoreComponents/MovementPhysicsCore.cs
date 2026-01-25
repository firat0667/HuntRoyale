using UnityEngine;

namespace Subsystems.CoreComponents
{
    public class MovementPhysicsCore : CoreComponent
    {
        private Rigidbody m_rb;

        public Vector3 Velocity { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            m_rb = GetComponentInParent<Rigidbody>();
        }

        public void Move(Vector3 input, float speed)
        {
            if (m_rb == null)
            {
                Velocity = Vector3.zero;
                return;
            }

            if (input.sqrMagnitude < 0.001f)
            {
                Velocity = Vector3.zero;
                return;
            }

            Vector3 moveDir = input.normalized;
            Velocity = moveDir * speed;

            Vector3 nextPos = m_rb.position + Velocity * Time.deltaTime;
            m_rb.MovePosition(nextPos);
        }
    }
}

