using Subsystems;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LvlTextController : MonoBehaviour
    {
        [SerializeField] private Text m_LvlText;

        private ExperienceSubsystem m_experience;

        private void Awake()
        {
            var entity = GetComponentInParent<BaseEntity>();
            if (entity == null)
            {
                Debug.LogError("LvlTextController: BaseEntity yok.");
                return;
            }

            m_experience = entity.GetSubsystem<ExperienceSubsystem>();
            if (m_experience == null)
            {
                Debug.LogError("LvlTextController: ExperienceSubsystem yok.");
                return;
            }
            UpdateLevelText(m_experience.Level);
            m_experience.OnLevelUp.Connect(UpdateLevelText);
        }

        private void OnDestroy()
        {
            if (m_experience != null)
                m_experience.OnLevelUp.Disconnect(UpdateLevelText);
        }

        private void UpdateLevelText(int newLevel)
        {
            m_LvlText.text =newLevel.ToString();
        }
    }
}