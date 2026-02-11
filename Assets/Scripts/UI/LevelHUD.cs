using DG.Tweening;
using Subsystems;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class LevelHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_levelText;
        [SerializeField] private LongIconBarTool m_barTool;

        private ExperienceSubsystem m_boundExp;

        public void Bind(ExperienceSubsystem exp)
        {
            if (m_boundExp != null)
            {
                m_boundExp.OnLevelUp.Disconnect(OnLevelUp);
                m_boundExp.OnExpChanged.Disconnect(OnExpChanged);
            }

            m_boundExp = exp;

            UpdateLevel(exp.Core.Level);
            UpdateExp(exp.Core.CurrentExp, exp.Core.RequiredExp);

            exp.OnLevelUp.Connect(OnLevelUp);
            exp.OnExpChanged.Connect(OnExpChanged);
        }

        private void OnLevelUp(int level)
        {
            UpdateLevel(level);
            PlayLevelUpAnimation();
        }

        private void OnExpChanged(int current, int required)
        {
            UpdateExp(current, required);
        }

        private void UpdateExp(int current, int required)
        {
            m_barTool.SetMaxValue(required);
            m_barTool.SetNowValue(current);
        }

        private void UpdateLevel(int level)
        {
            m_levelText.text = $"Lvl {level}";
        }

        private void PlayLevelUpAnimation()
        {
            m_levelText.transform
                .DOPunchScale(Vector3.one * 0.25f, 0.3f, 6, 0.8f);
        }
    }
}

