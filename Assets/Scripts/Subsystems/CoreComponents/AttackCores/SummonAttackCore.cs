using UnityEngine;
using Firat0667.WesternRoyaleLib.Game;

namespace Subsystems.CoreComponents.AttackCores
{
    public class SummonAttackCore : AttackCore
    {
        private SummonSpawner m_summonSpawner =>
        GameRegistry.Instance.Get<SummonSpawner>(
          GameRegistryTags.GAME_REGISTRY_SPAWNER_SUMMON
      );
        public override void OnAttackHit()
        {
            if (context == null)
                return;
            var perception = context.Perception;
            if (perception == null || perception.CurrentTarget == null)
                return;
            var stats = context.Stats;
            if (stats == null && stats == null)
                return;

            if (m_summonSpawner == null)
            {
                Debug.LogError("SummonSpawner not found!");
                return;
            }

            Vector3 forward = context.OwnerTransform.forward;
            forward.y = 0f;
            forward.Normalize();

            for (int i = 0; i < stats.SummonSpawnCount; i++)
            {
                Vector3 randomOffset =
                    Random.insideUnitSphere * 0.3f;

                randomOffset.y = 0f;

                Vector3 spawnPos =
                    context.OwnerTransform.position +
                    forward * 1f +
                    randomOffset;

                m_summonSpawner.Spawn(
                stats.SummonID,
                spawnPos,
                currentDamage,
                perception.CurrentTarget,
                context.OwnerTransform,
                stats.SummonExplosionRadius,
                stats.SummonExplosionTriggerDistance,
                perception.TargetLayer,
                context    
            );
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (context == null)
                return;

            var stats = context.Stats;
            if (stats == null)
                return;

            Vector3 origin = context.OwnerTransform.position;
            Vector3 forward = context.OwnerTransform.forward;
            forward.y = 0f;
            forward.Normalize();

            Gizmos.color = Color.magenta;

            for (int i = 0; i < stats.SummonSpawnCount; i++)
            {
                Vector3 spawnPos =
                    origin +
                    forward * 0.6f;

                Gizmos.DrawSphere(spawnPos, 0.1f);
            }
        }
#endif
    }

}
