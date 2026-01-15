using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    private AttackSubsystem _attack;
    private Player m_player;
    private void Awake()
    {
        m_player = transform.root.GetComponent<Player>();
        _attack = m_player.GetComponentInChildren<AttackSubsystem>();
    }

    public void OnAttackHit()
    {
        _attack?.TryAttack();
    }
}
