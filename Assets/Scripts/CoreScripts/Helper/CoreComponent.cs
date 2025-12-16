using UnityEngine;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{
    protected Subsystem m_core;

    protected virtual void Awake()
    {
        m_core = GetComponentInParent<Subsystem>();
        if (m_core == null)
        {
            Debug.LogError($"{name} has no Subsystem parent!");
            return;
        }

        m_core.AddComponent(this); 
    }
    public virtual void Initialize(Subsystem subsystem)
    {
        m_core = subsystem;
    }

    public virtual void LogicUpdate() { }
}
