using UnityEngine;

public class AnimatorBridge : MonoBehaviour
{
    private Animator m_animator;

    private readonly int SpeedHash = Animator.StringToHash(AnimTag.Speed_Param);
    private readonly int AttackHash = Animator.StringToHash(AnimTag.Attack_Param);
    private readonly int DeadHash = Animator.StringToHash(AnimTag.isDead_Param);

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
    }

    public void SetSpeed(float value)
    {
        m_animator.SetFloat(SpeedHash, value);
    }

    public void TriggerAttack()
    {
        m_animator.SetTrigger(AttackHash);
    }

    public void TriggerDead()
    {
        m_animator.SetTrigger(DeadHash);
    }
}
