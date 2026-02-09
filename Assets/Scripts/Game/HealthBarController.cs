using Subsystems;
using UnityEngine;


namespace Game
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] private LongIconBarTool m_barTool;

        [SerializeField] private HealthSubsystem m_healthSubsystem;

        private void OnEnable()
        {
            m_healthSubsystem.OnHealthChanged.Connect(OnHealChanged);
        }
        private void OnDisable()
        {
            m_healthSubsystem.OnHealthChanged.Disconnect(OnHealChanged);
        }

        private void OnHealChanged(int currentHealth, int maxHealth)
        {
            m_barTool.SetMaxValue(maxHealth);
            m_barTool.SetNowValue(currentHealth);
            if(currentHealth <= 0)
            {
                m_barTool.gameObject.SetActive(false);
            }
             else
            {
                m_barTool.gameObject.SetActive(true);
            }
        }
    }
}

