using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    private AttackSubsystem _attack;

    private void Awake()
    {
        _attack = GetComponentInParent<AttackSubsystem>();
    }

    public void OnAttackHit()
    {
        _attack?.TryAttack();
    }
}
