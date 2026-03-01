using Combat.Stats.ScriptableObjects;
using Combat.Upgrades.ScriptableObjects;
using Firat0667.WesternRoyaleLib.Patterns;
using Managers.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Upgrades;

namespace Managers.Upgrade
{
    public class UpgradeManager : FoundationSingleton<UpgradeManager>, IFoundationSingleton
    {
        public bool Initialized { get ; set ; }

        [SerializeField] private List<UpgradeSO> m_allUpgrades;

        public List<UpgradeSO> GetValidUpgrades(IUpgradeable entity)
        {
            return m_allUpgrades
                .Where(u =>
                    entity.UpgradeSubsystem.CanTakeUpgrade(u) &&
                    IsValidForEntity(u, entity))
                .ToList();
        }
        public List<UpgradeSO> GetRandomWeighted(List<UpgradeSO> list, int count)
        {
            List<UpgradeSO> result = new();
            List<UpgradeSO> pool = new(list);

            for (int i = 0; i < count && pool.Count > 0; i++)
            {
                int totalWeight = pool.Sum(u => u.weight);
                int randomValue = Random.Range(0, totalWeight);

                int cumulative = 0;

                foreach (var upgrade in pool)
                {
                    cumulative += upgrade.weight;

                    if (randomValue < cumulative)
                    {
                        result.Add(upgrade);
                        pool.Remove(upgrade);
                        break;
                    }
                }
            }

            return result;
        }
        private bool IsValidForEntity(UpgradeSO upgrade, IUpgradeable entity)
        {
            var baseStats = entity.BaseStats;

            switch (upgrade.category)
            {
                case UpgradeCategory.Global:
                    return true;

                case UpgradeCategory.Melee:
                    return baseStats.attackType == AttackType.Melee;

                case UpgradeCategory.Ranged:
                    return baseStats.attackType == AttackType.Ranged;

                case UpgradeCategory.Summon:
                    return baseStats.attackType == AttackType.Summoner;

                case UpgradeCategory.Effect:
                    return (baseStats.onHitEffects != null &&
                            baseStats.onHitEffects.Contains(upgrade.targetEffect))
                        ||
                           (baseStats.selfEffects != null &&
                            baseStats.selfEffects.Contains(upgrade.targetEffect));

                default:
                    return false;
            }
        }
    }
}

