using CoreScripts.ObjectPool;
using Pathfinding;
using States.EnemyStates;
using States.SummonMinionStates;
using Subsystems;
using Subsystems.Ai;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class SummonMinion : BaseEntity
    {
        #region Subsystem
        public AINavigationSubsystem Navigation { get; private set; }
        public MovementSubsystem Movement { get; private set; }
        #endregion


        #region Navmesh
        private AIPath m_aiPath;
        public AIPath AIPath => m_aiPath;

        private AIDestinationSetter m_destinationSetter;

        #endregion

        #region States
        public SummonIdleState IdleState { get; private set; }
        public SummonFollowState FollowState { get; private set; }
        #endregion

        #region Animations
        private AnimatorBridge m_animatorBridge;
        public AnimatorBridge AnimatorBridge => m_animatorBridge;
        #endregion

        #region properties
        private float m_damage;
        private float m_explosionRadius;
        private float m_explosionTriggerDistance;

        private Transform m_owner;
        private Transform m_currentTarget;

        private LayerMask m_targetLayer;

        private ComponentPool<SummonMinion> m_ownerPool;
        public Transform Owner => m_owner;
        public Transform CurrentTarget => m_currentTarget;
        public float ExplosionTriggerDistance => m_explosionTriggerDistance;
        public void ExplodeNow() => Explode();

        private IMovableEntity m_ownerEntity;
        public IMovableEntity OwnerEntity
        {
            get
            {
                if (m_ownerEntity == null && m_owner != null)
                    m_ownerEntity = m_owner.GetComponent<IMovableEntity>();
                return m_ownerEntity;
            }
        }
        #endregion
        protected override void Awake()
        {
            base.Awake();
            Navigation =GetSubsystem<AINavigationSubsystem>();
            m_aiPath = GetComponent<AIPath>();
            m_destinationSetter = GetComponent<AIDestinationSetter>();

            Movement = GetSubsystem<MovementSubsystem>();
            m_animatorBridge = GetComponent<AnimatorBridge>();

        }
        public void Init(
            float damage,
            Transform target,
            Transform owner,
            float explosionRadius,
            float explosionTriggerDistance,
            LayerMask targetLayer,
            ComponentPool<SummonMinion> pool
        )
        {
            m_damage = damage;
            m_currentTarget = target;
            m_owner = owner;
            m_explosionRadius = explosionRadius;
            m_explosionTriggerDistance= explosionTriggerDistance;
            m_targetLayer = targetLayer;
            m_ownerPool = pool;
            m_ownerEntity = owner.GetComponent<IMovableEntity>();
            Initialize();
            if (m_owner != null)
            {
                var perception = m_owner.GetComponent<CombatPerception>();
                if (perception != null)
                {
                    perception.OnTargetChanged.Connect(OnOwnerTargetChanged);
                }
            }
        }
        private void OnOwnerTargetChanged(Transform newTarget)
        {
            m_currentTarget = newTarget;

            if (m_currentTarget != null)
                SM.ChangeState(FollowState);
            else
               SM.ChangeState(IdleState);
        }
        protected override void Update()
        {
            base.Update();

            if (m_owner == null)
            {
                ReturnToPool();
                return;
            }

            Vector3 velocity = m_aiPath.canMove
            ? m_aiPath.velocity
            : Vector3.zero;

            m_animatorBridge.UpdateMovementAnim(
                velocity,
                false
            );
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            if (m_owner != null)
            {
                var perception = m_owner.GetComponent<CombatPerception>();
                if (perception != null)
                {
                    perception.OnTargetChanged.Disconnect(OnOwnerTargetChanged);
                }
            }
        }

        private void Explode()
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                m_explosionRadius,
                m_targetLayer

            );

            foreach (var hit in hits)
            {
                var damageable = hit.GetComponentInChildren<IDamageable>();
                if (damageable != null)
                    damageable.TakeDamage((int)m_damage, m_owner);
                OnDied();
            }
          

        }
        protected override void OnDied()
        {
            base.OnDied();
            ReturnToPool();

        }
        protected override void CreateStates()
        {
            FollowState = new SummonFollowState(this);
            IdleState = new SummonIdleState(this);
        }

        protected override IState GetEntryState()
        {
            return FollowState;
        }
        private void ReturnToPool()
        {
            m_ownerPool.Return(this);
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.4f, 0f, 0.25f);
            Gizmos.DrawWireSphere(transform.position, m_explosionRadius);

            Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
            Gizmos.DrawWireSphere(transform.position, m_explosionTriggerDistance);
        }
#endif
    }
}