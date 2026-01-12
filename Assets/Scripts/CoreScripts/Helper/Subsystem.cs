using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Subsystem : MonoBehaviour
{
    protected BaseEntity Entity;

    private readonly List<CoreComponent> CoreComponents = new List<CoreComponent>();
    protected StatsComponent StatsComponent { get; private set; }

    protected virtual void Awake()
    {
        Entity = GetComponentInParent<BaseEntity>();

        if (Entity == null)
        {
            Debug.LogError($"{name} has no BaseEntity parent!");
        }
        StatsComponent = Entity.GetComponent<StatsComponent>();
    }

    public virtual void LogicUpdate()
    {
        foreach (CoreComponent component in CoreComponents)
        {
            component.LogicUpdate();
        }
    }
    public virtual void PhysicsUpdate()
    {
        foreach (CoreComponent component in CoreComponents)
        {
            component.PhysicsUpdate();
        }
    }
    public void AddComponent(CoreComponent component)
    {
        if (!CoreComponents.Contains(component))
        {
            CoreComponents.Add(component);
        }
    }

    public T GetCoreComponent<T>() where T : CoreComponent
    {
        var comp = CoreComponents.OfType<T>().FirstOrDefault();

        if (comp)
            return comp;

        comp = GetComponentInChildren<T>();

        if (comp)
            return comp;

        Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
        return null;
    }

    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;
    }
}

