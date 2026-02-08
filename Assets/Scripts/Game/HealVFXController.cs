using Subsystems;
using UnityEngine;

public class HealVFXController : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_healVFX;
    private HealthSubsystem m_health;

    private void Awake()
    {
        m_health = GetComponentInChildren<HealthSubsystem>();
    }

    private void Update()
    {
        if (m_health == null || m_healVFX == null)
            return;

        if (m_health.IsHealable)
        {
            if (!m_healVFX.isPlaying)
                m_healVFX.Play();
        }
        else
        {
            if (m_healVFX.isPlaying)
                m_healVFX.Stop();
        }
    }
}