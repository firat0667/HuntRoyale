using CoreScripts.ObjectPool;
using Firat0667.WesternRoyaleLib.Key;
using FiratGames.WesternRoyale.Event;
using Managers.VFX;
using Pathfinding;
using States.EnemyStates;
using Subsystems;
using Subsystems.Ai;
using System.Collections;
using UnityEngine;
using AI.Enemies;
using Managers.Enemies;
using Combat.ScriptableObjects;


namespace AI.Enemies
{
    public class Enemy : BaseEntity, IMovableEntity
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
        public EffectSubsystem EffectSubsystem { get; private set; }
        public ExperienceSubsystem ExperienceSubsystem { get; private set; }



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

        public bool IsMoving =>
        Movement != null &&
        Movement.Velocity.sqrMagnitude > 0.01f;


        public bool IsTargetInDetectRange =>
        HasTarget &&
        Attack.Perception.CurrentTargetSqrDistance <=
        Attack.Perception.CurrentDetectionRange * Attack.Perception.CurrentDetectionRange;

        private ComponentPool<Enemy> m_ownerPool;
        private Collider m_collider;
        #endregion

        #region VFX
        [Header("VFX KEY")]
        [SerializeField] private EventKey m_deathVFXKey;
        #endregion

        #region Signal Handlers

        public BasicSignal<Enemy> OnDeath { get; private set; }
        #endregion

        #region Kill Rewards
        [SerializeField] private KillRewardsSO m_killRewards;
        public KillRewardsSO KillRewards => m_killRewards;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            OnDeath = new BasicSignal<Enemy>();

            Attack = GetSubsystem<AttackSubsystem>();
            Movement = GetSubsystem<MovementSubsystem>();
            EffectSubsystem = GetSubsystem<EffectSubsystem>();
            ExperienceSubsystem = GetSubsystem<ExperienceSubsystem>();

            m_animatorBridge = GetComponent<AnimatorBridge>();

            Navigation = GetSubsystem<AINavigationSubsystem>();
            m_aiPath = GetComponent<AIPath>();
            m_destinationSetter = GetComponent<AIDestinationSetter>();
            m_collider = GetComponent<Collider>();

        }
        protected override void Start()
        {
            base.Start();
            healthSubsystem.SetHealable(false);

        }
        protected override void Update()
        {
            if (IsDead)
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
        public void ResetForSpawn(ComponentPool<Enemy> enemyPool)
        {
            Initialize();
            healthSubsystem.ResetHealth();
            m_collider.enabled = true;
            m_aiPath.canMove = true;
            m_ownerPool = enemyPool;
            Attack.Perception.ClearTarget();
            healthSubsystem.OnDamaged.Connect(OnDamaged);
        }
        private void Despawn()
        {
            m_ownerPool.Return(this);
        }
        private IEnumerator DespawnAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            VFXManager.Instance.Play(m_deathVFXKey, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
            Despawn();

        }
        protected override void OnDied()
        {
            base.OnDied();
            OnDeath.Emit(this);
            EnemyManager.Instance.EnemyDiedSignal.Emit(this);
            m_aiPath.canMove = false;
            m_collider.enabled = false;
            m_destinationSetter.target = null;
            m_aiPath.SetPath(null);
            effectSubsystem.RemoveAllEffects();
            healthSubsystem.OnDamaged.Disconnect(OnDamaged);
            m_animatorBridge.TriggerDead();
            Movement.Stop();
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
}
