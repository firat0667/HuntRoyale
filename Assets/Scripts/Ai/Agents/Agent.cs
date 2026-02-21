using AI.Brain;
using Combat;
using Pathfinding;
using States.AgentStates;
using Subsystems;
using Subsystems.Ai;
using System.Xml;
using UnityEngine;

namespace AI.Agents

{
    public class Agent : BaseEntity, IMovableEntity
    {
        #region Subsystems
        public AINavigationSubsystem Navigation { get; private set; }
        public AttackSubsystem Attack { get; private set; }
        public MovementSubsystem Movement { get; private set; }

        #endregion

        #region Properties
        public BotBrain Brain { get; private set; }
        public AnimatorBridge AnimatorBridge { get; private set; }
        public bool IsInCombat => Attack.IsTargetInAttackRange;

        private AIPath m_aiPath;
        public CombatPerception CombatPerception => Attack.Perception;
        #endregion

        #region States
        public BotIdleState IdleState { get; private set; }
        public BotChaseState ChaseState { get; private set; }
        public BotAttackState AttackState { get; private set; }
        public BotHealState HealState { get; private set; }
        #endregion
        public bool IsMoving =>
        Movement != null &&
        Movement.Velocity.sqrMagnitude > 0.01f;

        protected override void Awake()
        {
            base.Awake();
            Brain = GetComponent<BotBrain>();
            AnimatorBridge = GetComponentInChildren<AnimatorBridge>();
            m_aiPath = GetComponent<AIPath>();
            Movement = GetSubsystem<MovementSubsystem>();
            Navigation = GetSubsystem<AINavigationSubsystem>();
            Attack = GetSubsystem<AttackSubsystem>();
        }
        protected override void Start()
        {
            base.Start();
            Initialize();
        }
        protected override void Update()
        {
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
        protected override void OnDied()
        {
            AnimatorBridge.TriggerDead();
            Brain.ClearTarget();
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
