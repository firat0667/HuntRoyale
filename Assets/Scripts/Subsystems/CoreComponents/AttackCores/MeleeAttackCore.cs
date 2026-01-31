using UnityEngine;

namespace Subsystems.CoreComponents.AttackCores
{
    public class MeleeAttackCore : AttackCore
    {
        public override void OnAttackHit()
        {
            if (context == null)
                return;

            var perception = context.Perception;
            if (perception == null || perception.CurrentTarget == null)
                return;

            var stats = context.Stats;

            float range = stats.AttackHitRange;
            float coneAngle = stats.AttackAngle;

            Vector3 origin = transform.position;

            Vector3 forward =
                perception.CurrentTarget.position - origin;
            forward.y = 0f;
            forward.Normalize();

            var hits = Physics.OverlapSphere(
                origin,
                range,
                perception.TargetLayer
            );

            float cosHalfAngle =
                Mathf.Cos(coneAngle * 0.5f * Mathf.Deg2Rad);

            foreach (var hit in hits)
            {
                Vector3 toEnemy = hit.transform.position - origin;
                toEnemy.y = 0f;

                if (toEnemy.sqrMagnitude > range * range)
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
            if (context == null)
                return;

            var perception = context.Perception;
            if (perception == null || perception.CurrentTarget == null)
                return;

            var stats = context.Stats;

            float range = stats.AttackHitRange;
            float coneAngle = stats.AttackAngle;

            Vector3 origin = transform.position;

            Vector3 forward =
                perception.CurrentTarget.position - origin;
            forward.y = 0f;
            forward.Normalize();

            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, origin + forward * range);

            float halfAngle = coneAngle * 0.5f;

            Vector3 leftDir =
                Quaternion.Euler(0f, -halfAngle, 0f) * forward;
            Vector3 rightDir =
                Quaternion.Euler(0f, halfAngle, 0f) * forward;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origin, origin + leftDir * range);
            Gizmos.DrawLine(origin, origin + rightDir * range);
        }
#endif
    }
}
