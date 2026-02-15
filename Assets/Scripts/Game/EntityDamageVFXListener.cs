using FiratGames.WesternRoyale.Event;
using Subsystems;
using UnityEngine;

namespace Game 
{
    public class EntityDamageVFXListener : MonoBehaviour
    {
        [Header("Damage VFX")]
        [SerializeField] private EventKey m_damageVFXKey;

        private HealthSubsystem m_health;

        private void Awake()
        {
            m_health = GetComponent<HealthSubsystem>();

            if (m_health != null)
                m_health.OnDamaged.Connect(OnDamaged);
        }

        private void OnDestroy()
        {
            if (m_health != null)
                m_health.OnDamaged.Disconnect(OnDamaged);
        }

        private void OnDamaged(Transform source)
        {
            VFXManager.Instance.Play(
                m_damageVFXKey,
                transform.position,
                Quaternion.identity
            );
        }
    }
}