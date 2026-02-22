using AI.Brain;
using Combat;
using Combat.Stats.ScriptableObjects;
using Managers.Upgrade;
using Pathfinding;
using States.AgentStates;
using Subsystems;
using Subsystems.Ai;
using System.Xml;
using UnityEngine;
using Upgrades;

namespace AI.Agents

{
    public class Agent : BaseEntity, IMovableEntity, IUpgradeable
    {
        #region Subsystems
        public AINavigationSubsystem Navigation { get; private set; }
        public AttackSubsystem Attack { get; private set; }
        public MovementSubsystem Movement { get; private set; }

        private ExperienceSubsystem m_experience;
        private UpgradeSubsystem m_upgrade;

        public UpgradeSubsystem UpgradeSubsystem => m_upgrade;

        #endregion

        #region Properties
        public BotBrain Brain { get; private set; }
        public bool IsInCombat => Attack.IsTargetInAttackRange;
        
        public CombatPerception CombatPerception => Attack.Perception;

        public bool IsMoving =>
        Movement != null &&
        Movement.Velocity.sqrMagnitude > 0.01f;

        private BaseStatsSO m_baseStatSO;
        public BaseStatsSO BaseStats => m_baseStatSO;

        #endregion

        #region States
        public BotIdleState IdleState { get; private set; }
        public BotChaseState ChaseState { get; private set; }
        public BotAttackState AttackState { get; private set; }
        public BotHealState HealState { get; private set; }
        #endregion

        #region Animations
        public AnimatorBridge AnimatorBridge { get; private set; }
        #endregion

        #region Navmesh
        private AIPath m_aiPath;
        private AIDestinationSetter m_destinationSetter;
        #endregion

   

 

        protected override void Awake()
        {
            base.Awake();

            m_baseStatSO= GetComponent<StatsComponent>().BaseStats;

            Brain = GetComponent<BotBrain>();
            AnimatorBridge = GetComponentInChildren<AnimatorBridge>();
            
            Navigation = GetSubsystem<AINavigationSubsystem>();
            m_destinationSetter = GetComponent<AIDestinationSetter>();

            Movement = GetSubsystem<MovementSubsystem>();
            Attack = GetSubsystem<AttackSubsystem>();
            m_experience = GetSubsystem<ExperienceSubsystem>();
            m_upgrade= GetSubsystem<UpgradeSubsystem>();
            m_aiPath = GetComponent<AIPath>();

            
        }
        protected override void Start()
        {
            base.Start();
            Initialize();
            m_experience.OnLevelUp.Connect(OnLevelUp);
        }
        protected override void Update()
        {
            if (IsDead)
                return;
            base.Update();

            if (Movement == null) return;

            Vector3 velocity = m_aiPath.canMove
         ? m_aiPath.velocity
         : Vector3.zero;

            AnimatorBridge.UpdateMovementAnim(
                velocity,
                IsInCombat
            );
        }
        private void OnLevelUp(int level)
        {
            if (m_upgrade == null)
                return;

            var valid = UpgradeManager.Instance.GetValidUpgrades(this);
            if (valid.Count == 0)
                return;

            var options = UpgradeManager.Instance.GetRandomWeighted(valid, 2);
            if (options.Count == 0)
                return;

            var chosen = options[Random.Range(0, options.Count)];

            m_upgrade.ApplyUpgrade(chosen);
        }
        protected override void OnDied()
        {
            AnimatorBridge.TriggerDead();
            m_aiPath.canMove = false;
            m_aiPath.SetPath(null);
            m_destinationSetter.target = null;
            Movement.Stop();
            Destroy(gameObject, 3f);
        }
        protected override void CreateStates()
        {
            IdleState = new BotIdleState(this);
            ChaseState = new BotChaseState(this);
            AttackState = new BotAttackState(this);
            HealState = new BotHealState(this);
        }

        protected override IState GetEntryState()
        {
            return IdleState;
        }
    }
}
