using System.Collections.Generic;
using Combat.Upgrades.ScriptableObjects;


namespace Subsystems
{
    public class UpgradeSubsystem : Subsystem
    {
        private Dictionary<UpgradeSO, int> m_upgradeStacks = new();

        public bool CanTakeUpgrade(UpgradeSO upgrade)
        {
            if (!m_upgradeStacks.ContainsKey(upgrade))
                return true;

            return m_upgradeStacks[upgrade] < upgrade.maxStack;
        }

        public void ApplyUpgrade(UpgradeSO upgrade)
        {
            if (!m_upgradeStacks.ContainsKey(upgrade))
                m_upgradeStacks[upgrade] = 0;

            m_upgradeStacks[upgrade]++;

            ApplyStat(upgrade);
        }
        private void ApplyStat(UpgradeSO upgrade)
        {
            if (StatsComponent != null)
            {
                StatsComponent.ApplyUpgradeStat(upgrade);
            }
        }
    }
}