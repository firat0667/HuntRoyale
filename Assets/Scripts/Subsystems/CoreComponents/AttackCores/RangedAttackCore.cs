using UnityEngine;

namespace Subsystems.CoreComponents.AttackCores
{
    public class RangedAttackCore : AttackCore
    {
        [SerializeField] private Transform m_projectileSpawnPoint;

        public override void OnAttackHit()
        {
            if (context == null)
                return;

            var perception = context.Perception;
            if (perception == null || perception.CurrentTarget == null)
                return;

            var stats = context.Stats;
            if (stats.ProjectilePrefab == null)
                return;

            Vector3 dir =
                perception.CurrentTarget.position - m_projectileSpawnPoint.position;
            dir.y = 0f;
            dir.Normalize();


            // use object pool later
            var projectile = Instantiate(
                stats.ProjectilePrefab,
                m_projectileSpawnPoint.position,
                Quaternion.LookRotation(dir)
            );

            projectile.Init(
                currentDamage,
                dir,
                stats.ProjectileSpeed,
                stats.ProjectilePierce
            );
        }
    }
}
