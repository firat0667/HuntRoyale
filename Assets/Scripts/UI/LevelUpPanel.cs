using Combat.Upgrades.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Firat0667.WesternRoyaleLib.Key;
using Game.UI;
using Upgrades;

public class LevelUpPanel : MonoBehaviour
{
    [SerializeField] private UpgradeCardUI[] m_cards;
    [SerializeField] private GameObject m_root;

    private IUpgradeable m_entity;

    private BasicSignal<UpgradeSO> m_onUpgradeSelectedSignal
        = new BasicSignal<UpgradeSO>();

    private void Awake()
    {
        m_onUpgradeSelectedSignal.Connect(OnUpgradeSelected);
    }

    public void Show(List<UpgradeSO> upgrades, IUpgradeable entity)
    {
        m_entity = entity;
        m_root.SetActive(true);
        Time.timeScale = 0;

        for (int i = 0; i < m_cards.Length; i++)
        {
            if (i < upgrades.Count)
            {
                m_cards[i].gameObject.SetActive(true);
                m_cards[i].Setup(upgrades[i], m_onUpgradeSelectedSignal);
            }
            else
            {
                m_cards[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnUpgradeSelected(UpgradeSO upgrade)
    {
        m_entity.UpgradeSubsystem.ApplyUpgrade(upgrade);

        m_root.SetActive(false);
        Time.timeScale = 1;
    }
}