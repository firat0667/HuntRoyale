using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Subsystem[] _subsystems;

    private HealthSubsystem _healthSubsystem;

    private void Awake()
    {
        _subsystems = GetComponentsInChildren<Subsystem>();

        _healthSubsystem = GetSubsystem<HealthSubsystem>();

        _healthSubsystem.OnDied?.Connect(OnPlayerDied);
    }
    private void OnDisable()
    {
       _healthSubsystem.OnDied?.Disconnect(OnPlayerDied);
    }
    private void Update()
    {
        foreach (var s in _subsystems)
            s.LogicUpdate();
    }

    public T GetSubsystem<T>() where T : Subsystem
    {
        return _subsystems.OfType<T>().FirstOrDefault();
    }
    private void OnPlayerDied()
    {
        EventManager.Instance.Trigger(EventTags.EVENT_PLAYER_DIED);
    }
}
