using Combat.Upgrades.ScriptableObjects;
using Firat0667.WesternRoyaleLib.Key;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UpgradeCardUI : MonoBehaviour
    {
        [SerializeField] private Image m_icon;
        [SerializeField] private TMP_Text m_text;
        [SerializeField] private Button m_button;

        private UpgradeSO m_upgrade;
        private BasicSignal<UpgradeSO> m_onSelectedSignal;

        public void Setup(UpgradeSO upgrade, BasicSignal<UpgradeSO> onSelected)
        {
            m_upgrade = upgrade;
            m_onSelectedSignal = onSelected;

            m_icon.sprite = upgrade.icon;
            m_text.text = BuildText(upgrade);

            m_button.onClick.RemoveAllListeners();
            m_button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            m_onSelectedSignal?.Emit(m_upgrade);
        }

        private string BuildText(UpgradeSO upgrade)
        {
            string name;

            if (upgrade.category == UpgradeCategory.Effect && upgrade.targetEffect != null)
            {
                name = upgrade.targetEffect.name;
            }
            else
            {
                name = upgrade.statType.ToString();
            }

            string value = upgrade.isPercentage
                ? $"+{upgrade.value}%"
                : $"+{upgrade.value}";

            return $"{name} {value}";
        }
    }
}