using Subsystems.CoreComponents;
using UnityEngine;

namespace Subsystems.CoreComponents.AttackCores
{
    public class MeleeAttackCore : AttackCore
    {
        private float m_range;
        private CombatPerception _perception;
        public float m_coneAngle;

        private void Awake()
        {
            _perception = GetComponentInParent<CombatPerception>();
        }
        public override void Initialize(Subsystem subsystem)
        {
            base.Initialize(subsystem);

            var attackSubsystem = subsystem as AttackSubsystem;
            if (attackSubsystem == null)
            {
                Debug.LogError("MeleeAttackCore requires AttackSubsystem");
                return;
            }
            m_range = attackSubsystem.attackHitRange;
            m_coneAngle= attackSubsystem.AttackAngle;
        }

        public override void OnAttackHit()
        {
            var target = _perception.CurrentTarget;
            if (target == null)
                return;

            Vector3 origin = transform.position;
            Vector3 forward = target.position - origin;
            forward.y = 0f;
            forward.Normalize();

            var hits = Physics.OverlapSphere(
                origin,
                m_range,
                _perception.TargetLayer
            );

            float cosHalfAngle =
                Mathf.Cos(m_coneAngle * 0.5f * Mathf.Deg2Rad);

            foreach (var hit in hits)
            {
                Vector3 toEnemy = hit.transform.position - origin;
                toEnemy.y = 0f;

                float sqrDist = toEnemy.sqrMagnitude;
                if (sqrDist > m_range * m_range)
                    continue;

                float dot = Vector3.Dot(forward, toEnemy.normalized);
                if (dot < cosHalfAngle)
                    continue;

                var dmg = hit.GetComponentInChildren<IDamageable>();
                if (dmg == null)
                    continue;

                dmg.TakeDamage(currentDamage);
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_perception == null || _perception.CurrentTarget == null)
                return;

            Vector3 origin = transform.position;

            Vector3 forward =
                _perception.CurrentTarget.position - origin;
            forward.y = 0f;
            forward.Normalize();

            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, origin + forward * m_range);

            float halfAngle = m_coneAngle * 0.5f;

            Vector3 leftDir =
                Quaternion.Euler(0f, -halfAngle, 0f) * forward;
            Vector3 rightDir =
                Quaternion.Euler(0f, halfAngle, 0f) * forward;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origin, origin + leftDir * m_range);
            Gizmos.DrawLine(origin, origin + rightDir * m_range);

            int steps = 12;
            Vector3 prevPoint = origin + leftDir * m_range;

            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;
                float angle = Mathf.Lerp(-halfAngle, halfAngle, t);

                Vector3 dir =
                    Quaternion.Euler(0f, angle, 0f) * forward;

                Vector3 point = origin + dir * m_range;

                Gizmos.DrawLine(prevPoint, point);
                prevPoint = point;
            }
        }
#endif
    }
}
