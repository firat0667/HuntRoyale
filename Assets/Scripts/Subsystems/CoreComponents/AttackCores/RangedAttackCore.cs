using System.Collections.Generic;
using UnityEngine;

namespace Subsystems.CoreComponents.AttackCores
{
    public class RangedAttackCore : AttackCore
    {
        [SerializeField] private Transform m_projectileSpawnPoint;
        private const float FLAT_Y = 0f;

        public override void OnAttackHit()
        {
            if (context == null)
                return;

            var perception = context.Perception;
            if (perception == null)
                return;

            var stats = context.Stats;
            if (stats.ProjectilePrefab == null)
                return;

            Vector3 origin = context.OwnerTransform.position;
            Vector3 spawnPos= m_projectileSpawnPoint.position;
            Vector3 forward =
            perception.CurrentTarget.position - origin;
            forward.y = FLAT_Y;
            forward.Normalize();

            var targets = CollectTargets(
            origin,
            forward,
            stats.ProjectileRange,
            stats.AttackAngle,
            perception.TargetLayer
            );

            if (targets.Count == 0)
                return;

            targets.Sort((a, b) =>
                Vector3.Distance(origin, a.position)
                .CompareTo(Vector3.Distance(origin, b.position))
            );

            int hitCount = Mathf.Min(stats.ProjectilePierce, targets.Count);
            var finalTargets = targets.GetRange(0, hitCount);

            Vector3 dir =
                (finalTargets[0].position - origin).normalized;

            var projectile = Instantiate(
                stats.ProjectilePrefab,
                spawnPos,
                Quaternion.LookRotation(dir)
            );

            projectile.Init(
                currentDamage,
                finalTargets,
                transform.root,
                stats.ProjectileSpeed
            );
        }
        private List<Transform> CollectTargets(
           Vector3 origin,
           Vector3 forward,
           float range,
           float angle,
           LayerMask targetLayer)
        {
            var hits = Physics.OverlapSphere(origin, range, targetLayer);
            List<Transform> result = new();

            foreach (var hit in hits)
            {
                Vector3 dir = hit.transform.position - origin;
                dir.y = 0f;

                if (Vector3.Angle(forward, dir) <= angle * 0.5f)
                    result.Add(hit.transform);
            }
            return result;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!isActiveAndEnabled)
                return;
            if (context == null)
                return;

            var perception = context.Perception;
            if (perception == null || perception.CurrentTarget == null)
                return;

            var stats = context.Stats;

            float range = stats.ProjectileRange;
            float coneAngle = stats.AttackAngle;

            Vector3 origin = context.OwnerTransform.position;

            Vector3 forward =
                perception.CurrentTarget.position - origin;
            forward.y = FLAT_Y;
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

         
            Gizmos.color = new Color(0f, 0.6f, 1f, 0.25f);
            Gizmos.DrawWireSphere(origin, range);
        }
#endif


    }

}
