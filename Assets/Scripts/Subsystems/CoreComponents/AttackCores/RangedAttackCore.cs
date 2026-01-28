using Subsystems.CoreComponents;
using UnityEngine;

namespace Subsystems.CoreComponents.AttackCores
{
    public class RangedAttackCore : AttackCore
    {
        [SerializeField] private GameObject m_projectilePrefab;
        [SerializeField] private Transform m_projectileSpawnPoint;

        private CombatPerception _perception;

        private void Awake()
        {
            _perception = GetComponentInParent<CombatPerception>();
        }

        public override void OnAttackHit()
        {
            if (_perception == null)
                return;

            var target = _perception.CurrentTarget;
            if (target == null)
                return;

            Vector3 dir =
                (target.position - m_projectileSpawnPoint.position).normalized;

            // use object pooler in future
            var proj = Instantiate(
                m_projectilePrefab,
                m_projectileSpawnPoint.position,
                Quaternion.LookRotation(dir)
            );

            if (proj.TryGetComponent<Projectile>(out var projectile))
            {
                projectile.Init(currentDamage, dir);
            }
        }
    }
}
