using Subsystems;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    private AttackSubsystem _attack;
    private BaseEntity m_entity;
    private void Awake()
    {
        m_entity = transform.root.GetComponent<BaseEntity>();
        _attack = m_entity.GetComponentInChildren<AttackSubsystem>();
    }

    public void OnAttackHit()
    {
        _attack?.NotifyAttackHit();
    }
}
