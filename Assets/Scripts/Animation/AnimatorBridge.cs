using System;
using UnityEngine;

public class AnimatorBridge : MonoBehaviour
{
    [SerializeField] private float dampTime = 0.15f;

    private Player m_player;
    private Animator m_animator;
   

    private readonly int MoveXHash = Animator.StringToHash(AnimTag.MoveX_Param);
    private readonly int MoveYHash = Animator.StringToHash(AnimTag.MoveY_Param);
    private readonly int IsMovingHash = Animator.StringToHash(AnimTag.IsMoving_Param);
    private readonly int AttackHash = Animator.StringToHash(AnimTag.Attack_Param);
    private readonly int DeadHash = Animator.StringToHash(AnimTag.isDead_Param);
    private readonly int SpeedHash = Animator.StringToHash(AnimTag.Speed_Param);

    private void Awake()
    {
        m_player = GetComponent<Player>();
        m_animator = m_player.GetComponentInChildren<Animator>();
    }

    public void UpdateMovementAnim(Vector3 worldVelocity)
    {
        if (worldVelocity.sqrMagnitude < 0.001f)
        {
            m_animator.SetBool(IsMovingHash, false);
            m_animator.SetFloat(MoveXHash, 0f, dampTime, Time.deltaTime);
            m_animator.SetFloat(MoveYHash, 0f, dampTime, Time.deltaTime);
            return;
        }

        m_animator.SetBool(IsMovingHash, true);

        Vector3 localDir =
            transform.InverseTransformDirection(worldVelocity.normalized);

        m_animator.SetFloat(MoveXHash, localDir.x, dampTime, Time.deltaTime);
        m_animator.SetFloat(MoveYHash, localDir.z, dampTime, Time.deltaTime);
    }

    // basic movement
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
