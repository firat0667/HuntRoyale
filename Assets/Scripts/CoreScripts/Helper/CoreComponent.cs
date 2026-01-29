using UnityEngine;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{
    protected Subsystem m_subsytem;

    protected virtual void Awake()
    {
        m_subsytem = GetComponentInParent<Subsystem>();
        if (m_subsytem == null)
        {
            Debug.LogError($"{name} has no Subsystem parent!");
            return;
        }

        m_subsytem.AddComponent(this); 
    }
    public virtual void Initialize(Subsystem subsystem)
    {
        m_subsytem = subsystem;
    }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() { }
}
