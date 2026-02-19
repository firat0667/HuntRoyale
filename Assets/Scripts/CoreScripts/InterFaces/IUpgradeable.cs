using Combat.Stats.ScriptableObjects;
using Subsystems;

namespace Upgrades
{
    public interface IUpgradeable
    {
        BaseStatsSO BaseStats { get; }
        UpgradeSubsystem UpgradeSubsystem { get; }
    }
}