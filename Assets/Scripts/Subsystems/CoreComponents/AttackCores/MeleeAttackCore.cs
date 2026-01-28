using Subsystems.CoreComponents;
using UnityEngine;

namespace Subsystems.CoreComponents.AttackCores
{
    public class MeleeAttackCore : AttackCore
    {
        [SerializeField] private float m_range = 1.2f;
        [SerializeField] private float m_radius = 1.0f;

        private CombatPerception _perception;

        private void Awake()
        {
            _perception = GetComponentInParent<CombatPerception>();
        }

        public override void OnAttackHit()
        {
            var target = _perception.CurrentTarget;
            if (target == null)
                return;

            Vector3 origin = transform.position;

            Vector3 dir = target.position - origin;
            dir.y = 0f;
            dir.Normalize();

            Vector3 center = origin + dir * m_range;

            var hits = Physics.OverlapSphere(
                center,
                m_radius,
                _perception.TargetLayer
            );

            foreach (var hit in hits)
            {
                var dmg = hit.GetComponentInChildren<IDamageable>();
                if (dmg == null) continue;

                dmg.TakeDamage(currentDamage);
                break;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var target = _perception != null ? _perception.CurrentTarget : null;

            Vector3 origin = transform.position;
            Vector3 center;

            if (target != null)
            {
                Vector3 dir = target.position - origin;
                dir.y = 0f;
                dir.Normalize();

                center = origin + dir * m_range;

                Gizmos.color = Color.green; 
                Gizmos.DrawLine(origin, target.position);
            }
            else
            {
                center = origin + transform.forward * m_range;
                Gizmos.color = Color.gray;
            }

            Gizmos.DrawWireSphere(center, m_radius);
        }
#endif

    }
}
