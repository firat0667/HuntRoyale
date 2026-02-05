using System.Collections.Generic;
using UnityEngine;
using Firat0667.CaseLib.Game;

namespace Subsystems.CoreComponents.AttackCores
{
    public class RangedAttackCore : AttackCore
    {
        [SerializeField] private Transform m_projectileSpawnPoint;

        private const float FLAT_Y = 0f;

        private ProjectileSpawner ProjectileSpawner =>
            GameRegistry.Instance.Get<ProjectileSpawner>(
                GameRegistryTags.GAME_REGISTRY_SPAWNER_PROJECTILE
            );

        public override void OnAttackHit()
        {
            if (context == null)
                return;

            var perception = context.Perception;
            if (perception == null || perception.CurrentTarget == null)
                return;

            var stats = context.Stats;
            if (stats == null)
                return;

            if (ProjectileSpawner == null)
            {
                Debug.LogError("RangedAttackCore: ProjectileSpawner not found!");
                return;
            }

            Vector3 origin = context.OwnerTransform.position;

            Vector3 attackForward =
                perception.CurrentTarget.position - origin;
            attackForward.y = FLAT_Y;
            attackForward.Normalize();

            var targets = CollectTargets(
                origin,
                attackForward,
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

            ProjectileSpawner.Spawn(
                stats.ProjectileID,
                m_projectileSpawnPoint,
                currentDamage,
                finalTargets,
                context.OwnerTransform,
                stats.ProjectileSpeed
            );
        }

        private List<Transform> CollectTargets(
            Vector3 origin,
            Vector3 forward,
            float range,
            float angle,
            LayerMask targetLayer
        )
        {
            var hits = Physics.OverlapSphere(origin, range, targetLayer);
            List<Transform> result = new();

            foreach (var hit in hits)
            {
                Transform root = hit.transform;
                if (root == context.OwnerTransform)
                    continue;

                Vector3 dir = root.position - origin;
                dir.y = 0f;

                if (Vector3.Angle(forward, dir) <= angle * 0.5f)
                    result.Add(root);
            }

            return result;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!isActiveAndEnabled || context == null)
                return;

            var perception = context.Perception;
            if (perception == null || perception.CurrentTarget == null)
                return;

            var stats = context.Stats;
            Vector3 origin = context.OwnerTransform.position;

            Vector3 forward =
                perception.CurrentTarget.position - origin;
            forward.y = FLAT_Y;
            forward.Normalize();

            float range = stats.ProjectileRange;
            float halfAngle = stats.AttackAngle * 0.5f;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, origin + forward * range);

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