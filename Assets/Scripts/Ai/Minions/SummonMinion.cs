using FiratGames.WesternRoyale.Event;
using Managers.VFX;
using Pathfinding;
using States.SummonMinionStates;
using Subsystems;
using Subsystems.Ai;
using UnityEngine;

namespace Combat
{
    public class SummonMinion : BaseEntity
    {
        #region Subsystems
        public AINavigationSubsystem Navigation { get; private set; }
        public MovementSubsystem Movement { get; private set; }
        #endregion

        #region Navigation
        private AIPath m_aiPath;
        public AIPath AIPath => m_aiPath;

        private AIDestinationSetter m_destinationSetter;
        #endregion

        #region States
        public SummonIdleState IdleState { get; private set; }
        public SummonFollowState FollowState { get; private set; }
        #endregion

        #region Animation
        private AnimatorBridge m_animatorBridge;
        public AnimatorBridge AnimatorBridge => m_animatorBridge;
        #endregion

        #region Runtime Data
        private float m_damage;
        private float m_explosionRadius;
        private float m_explosionTriggerDistance;

        private Transform m_owner;
        private Transform m_currentTarget;
        private LayerMask m_targetLayer;

        private float m_lifeTime;
        private bool m_isReturning;

        private ComponentPool<SummonMinion> m_ownerPool;
        private IAttackContext m_context;
        private BaseEntity m_ownerEntity;
        #endregion

        #region Public Access (State için gerekli)
        public Transform Owner => m_owner;
        public Transform CurrentTarget => m_currentTarget;
        public float ExplosionTriggerDistance => m_explosionTriggerDistance;
        public void ExplodeNow() => Explode();
        #endregion

        #region VFX
        [SerializeField] private EventKey m_deathVFXKey;
        #endregion

        #region Unity

        protected override void Awake()
        {
            base.Awake();

            Navigation = GetSubsystem<AINavigationSubsystem>();
            Movement = GetSubsystem<MovementSubsystem>();

            m_aiPath = GetComponent<AIPath>();
            m_destinationSetter = GetComponent<AIDestinationSetter>();
            m_animatorBridge = GetComponent<AnimatorBridge>();
        }

        protected override void Update()
        {
            base.Update();

            if (m_owner == null || m_ownerEntity == null || m_ownerEntity.IsDead)
            {
                ReturnToPool();
                return;
            }

            Vector3 velocity = m_aiPath.canMove ? m_aiPath.velocity : Vector3.zero;
            m_animatorBridge.UpdateMovementAnim(velocity, false);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            CancelInvoke();
            m_isReturning = false;

            if (m_owner != null)
            {
                var perception = m_owner.GetComponentInParent<CombatPerception>();
                if (perception != null)
                    perception.OnTargetChanged.Disconnect(OnOwnerTargetChanged);
            }
        }

        #endregion

        #region Init

        public void Init(
            float damage,
            Transform target,
            Transform owner,
            float explosionRadius,
            float explosionTriggerDistance,
            LayerMask targetLayer,
            ComponentPool<SummonMinion> pool,
            IAttackContext context
        )
        {
            m_damage = damage;
            m_currentTarget = target;
            m_owner = owner;
            m_explosionRadius = explosionRadius;
            m_explosionTriggerDistance = explosionTriggerDistance;
            m_targetLayer = targetLayer;
            m_ownerPool = pool;
            m_context = context;
            m_ownerEntity = owner.GetComponentInParent<BaseEntity>();

            m_isReturning = false;

            Initialize();

            if (m_owner != null)
            {
                var perception = m_owner.GetComponentInParent<CombatPerception>();
                if (perception != null)
                    perception.OnTargetChanged.Connect(OnOwnerTargetChanged);
            }

            StartLifetime();
        }

        private void StartLifetime()
        {
            m_lifeTime = m_context?.Stats?.SummonLifeTime ?? 5f;

            if (m_lifeTime > 0f)
            {
                CancelInvoke();
                Invoke(nameof(Explode), m_lifeTime);
            }
        }

        #endregion

        #region Behaviour

        private void OnOwnerTargetChanged(Transform newTarget)
        {
            m_currentTarget = newTarget;

            if (m_currentTarget != null)
                SM.ChangeState(FollowState);
            else
                SM.ChangeState(IdleState);
        }

        private void Explode()
        {
            if (m_isReturning) return;
            m_isReturning = true;

            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                m_explosionRadius,
                m_targetLayer
            );

            foreach (var hit in hits)
            {
                var entity = hit.GetComponentInParent<BaseEntity>();
                if (entity == null) continue;

                m_context.ApplyDamage(entity, (int)m_damage);
            }

            VFXManager.Instance.Play(m_deathVFXKey, transform.position, Quaternion.identity);

            ReturnToPoolInternal();
        }

        protected override void OnDied()
        {
            base.OnDied();
            Explode(); 
        }

        #endregion

        #region Pool

        private void ReturnToPool()
        {
            if (m_isReturning) return;

            m_isReturning = true;
            ReturnToPoolInternal();
        }

        private void ReturnToPoolInternal()
        {
            CancelInvoke();
            m_ownerPool.Return(this);
        }

        #endregion

        #region State Setup

        protected override void CreateStates()
        {
            FollowState = new SummonFollowState(this);
            IdleState = new SummonIdleState(this);
        }

        protected override IState GetEntryState()
        {
            return FollowState;
        }

        #endregion
    }
}