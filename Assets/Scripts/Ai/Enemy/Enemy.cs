using CoreScripts.ObjectPool;
using Firat0667.CaseLib.Key;
using FiratGames.CampSimulator.Event;
using Pathfinding;
using States.EnemyStates;
using Subsystems;
using Subsystems.Ai;
using System.Collections;
using UnityEngine;

public class Enemy : BaseEntity
{

    #region States
    public EnemyIdleState IdleState { get; private set; }
    public EnemyFollowState FollowState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }

    #endregion

    #region Subsystems
    public AINavigationSubsystem Navigation { get; private set; }
    public AttackSubsystem Attack { get; private set; }
    public MovementSubsystem Movement { get; private set; }

    #endregion

    #region Animations
    private AnimatorBridge m_animatorBridge;
    public AnimatorBridge AnimatorBridge => m_animatorBridge;
    #endregion

    #region Navmesh
    private AIPath m_aiPath;
    public AIPath AIPath => m_aiPath;

    private AIDestinationSetter m_destinationSetter;


    #endregion

    #region Properties
    public bool IsInCombat => Attack.IsTargetInAttackRange;
    public bool HasTarget => Attack.CurrentTarget != null;
    public bool IsTargetInAttackRange =>
    Attack.IsTargetInAttackRange;

    public bool IsTargetInDetectRange =>
    HasTarget &&
    Attack.Perception.CurrentTargetSqrDistance <=
    Attack.Perception.CurrentDetectionRange * Attack.Perception.CurrentDetectionRange;

    private ComponentPool<Enemy> m_ownerPool;
  

    private LayerMask m_originalLayer;

    #endregion

    #region VFX
    [Header("VFX KEY")]
    [SerializeField] private EventKey m_deathVFXKey;

    #endregion


    #region Signal Handlers

    public BasicSignal<Enemy>OnDeath { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        OnDeath= new BasicSignal<Enemy>();
        Attack = GetSubsystem<AttackSubsystem>();
        Navigation = GetSubsystem<AINavigationSubsystem>();
        Movement = GetSubsystem<MovementSubsystem>();

        m_animatorBridge = GetComponent<AnimatorBridge>();
        m_aiPath = GetComponent<AIPath>();
        m_destinationSetter = GetComponent<AIDestinationSetter>();
        m_originalLayer = gameObject.layer;

    }
    protected override void Start()
    {
        base.Start();
        healthSubsystem.OnDamaged.Connect(OnDamaged);

    }
    protected override void Update()
    {
        if(IsDead)
            return;

        base.Update();

        Vector3 velocity = m_aiPath.canMove
            ? m_aiPath.velocity
            : Vector3.zero;

        m_animatorBridge.UpdateMovementAnim(
            velocity,
            IsInCombat
        );
    }
    private void OnDamaged(Transform source)
    {
        if (source == null)
            return;

        Attack.Perception.SetCurrentTarget(source);
    }
    public void ResetForSpawn(ComponentPool<Enemy>  enemyPool)
    {
        m_destinationSetter.target = null;
        Movement.Stop();
        Navigation.Stop();
        m_ownerPool = enemyPool;
        gameObject.layer = m_originalLayer;

    }
    private void Despawn()
    {
        m_ownerPool.Return(this);
    }
    private IEnumerator DespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        VFXManager.Instance.Play(m_deathVFXKey, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Despawn();
    }
    protected override void OnDied()
    {
        OnDeath.Emit(this);
        gameObject.layer = LayerMask.NameToLayer(LayerTags.Dead_Layer);
        healthSubsystem.OnDamaged.Disconnect(OnDamaged);
        m_animatorBridge.TriggerDead();
        StartCoroutine(DespawnAfterDelay(3f));
        // particle effects, sound effects, etc. can be triggered here
        // xp for who killed the enemy can be awarded here
    }
    protected override void CreateStates()
    {
        IdleState = new EnemyIdleState(this);
        FollowState = new EnemyFollowState(this);
        AttackState = new EnemyAttackState(this);

    }
    protected override IState GetEntryState()
    {
        return IdleState;
    }
}
