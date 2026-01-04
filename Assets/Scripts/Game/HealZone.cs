using UnityEngine;

public class HealZone : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float m_healPercentPerSecond = 0.10f;

    private void OnTriggerStay(Collider other)
    {
        var health = other.GetComponentInChildren<HealthSubsystem>();
        if (health == null) return;

        int amount = Mathf.CeilToInt(health.MaxHP * m_healPercentPerSecond * Time.deltaTime);
        health.Heal(amount);
    }
}
