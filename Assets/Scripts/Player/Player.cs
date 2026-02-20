using Combat.Stats.ScriptableObjects;
using Managers.UI;
using Managers.Upgrade;
using States.PlayerStates;
using Subsystems;
using System;
using UnityEngine;
using Upgrades;

public class Player : BaseEntity, IMovableEntity,IUpgradeable
{
    #region States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    #endregion

    #region Subsystems
    private MovementSubsystem m_movement;
    private AttackSubsystem m_attack;
    private ExperienceSubsystem m_experience;
    private UpgradeSubsystem m_upgradeSubsystem;

    #endregion
    #region Animator
    private AnimatorBridge m_animatorBridge;
    public AnimatorBridge AnimatorBridge => m_animatorBridge;
    #endregion

    #region Parameters
    public bool IsInCombat => !IsDead && m_attack.IsTargetInAttackRange && m_attack.CanAttack();
    private bool m_isDead => healthSubsystem.IsDead;

    private bool m_walkAttackEnabled = false;
    public bool AllowWalkAttack => m_walkAttackEnabled;

    public bool IsMoving =>
    m_movement != null &&
    m_movement.Velocity.sqrMagnitude > 0.01f;

    private BaseStatsSO m_baseStatSO;
    public BaseStatsSO BaseStats => m_baseStatSO;

    public UpgradeSubsystem UpgradeSubsystem => m_upgradeSubsystem;


    #endregion


    protected override void Awake()
    {
        base.Awake();

        m_baseStatSO = GetComponent<StatsComponent>().BaseStats;

        m_movement = GetSubsystem<MovementSubsystem>();
        m_attack = GetSubsystem<AttackSubsystem>();
        m_animatorBridge = GetComponent<AnimatorBridge>();

        m_experience = GetSubsystem<ExperienceSubsystem>();
        m_upgradeSubsystem = GetSubsystem<UpgradeSubsystem>();

    }

    protected override void Start()
    {
        base.Start();
        Initialize();
        m_experience.OnLevelUp.Connect(HandleLevelUp);
    }
    protected override void Update()
    {
        if (m_isDead)
            return;

        base.Update(); 
        if (m_movement == null)
            return;


        AnimatorBridge.UpdateMovementAnim(
            m_movement.Velocity,IsInCombat
        );
    }

    protected override void OnDied()
    {
        base.OnDied();
        EventManager.Instance.Trigger(EventTags.EVENT_PLAYER_DIED);
        m_movement.Stop();
        m_animatorBridge.TriggerDead();
        if (m_experience != null)
            m_experience.OnLevelUp.Disconnect(HandleLevelUp);


    }
    private void HandleLevelUp(int level)
    {
        var valid = UpgradeManager.Instance.GetValidUpgrades(this);

        if (valid.Count == 0)
            return;

        var options = UpgradeManager.Instance.GetRandomWeighted(valid, 2);

        HUDManager.Instance.LevelUpPanel.Show(options, this);
    }
    protected override void CreateStates()
    {
        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
        AttackState = new PlayerAttackState(this);
    }
    protected override IState GetEntryState()
    {
        return IdleState;
    }
}
