using Subsystems;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class HealZone : MonoBehaviour
    {
        [Header("Heal Settings")]
        [SerializeField, Range(0f, 1f)]
        private float m_healPercentPerTick = 0.05f;

        [SerializeField]
        private float m_healTickInterval = 0.5f;

        private float m_tickTimer;

        private readonly HashSet<HealthSubsystem> m_targets = new();

        private void OnTriggerEnter(Collider other)
        {
            var health = other.GetComponentInChildren<HealthSubsystem>();
            if (health == null) return;

            m_targets.Add(health);
            health.SetInHealZone(true);
        }

        private void OnTriggerExit(Collider other)
        {
            var health = other.GetComponentInChildren<HealthSubsystem>();
            if (health == null) return;

            m_targets.Remove(health);
            health.SetInHealZone(false);
        }

        private void Update()
        {
            m_tickTimer += Time.deltaTime;
            if (m_tickTimer < m_healTickInterval)
                return;

            m_tickTimer = 0f;

            foreach (var health in m_targets)
            {
                if (!health.IsHealable)
                    continue;

                int amount = Mathf.CeilToInt(
                    health.MaxHP * m_healPercentPerTick
                );

                health.Heal(amount);
            }
        }
    }
}