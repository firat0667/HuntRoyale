using UnityEngine;

public class CombatPerception : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float scanInterval = 0.15f;

    private StatsComponent _stats;
    private float _scanTimer;

    private readonly Collider[] _hitsBuffer = new Collider[16];

    public Transform CurrentTarget { get; private set; }
    public float CurrentTargetSqrDistance { get; private set; }

    private void Awake()
    {
        _stats = GetComponent<StatsComponent>();
        _scanTimer = Random.Range(0f, scanInterval);
    }

    private void Update()
    {
        _scanTimer -= Time.deltaTime;
        if (_scanTimer <= 0f)
        {
            _scanTimer = scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        CurrentTarget = null;
        CurrentTargetSqrDistance = float.MaxValue;

        float maxScanRange = Mathf.Max(_stats.AttackRange, _stats.DetectionRange);

        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            maxScanRange,
            _hitsBuffer,
            targetLayer
        );

        for (int i = 0; i < hitCount; i++)
        {
            var hit = _hitsBuffer[i];
            if (!hit) continue;

            float sqrDist =
                (hit.transform.position - transform.position).sqrMagnitude;

            if (sqrDist < CurrentTargetSqrDistance)
            {
                CurrentTargetSqrDistance = sqrDist;
                CurrentTarget = hit.transform;
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_stats == null)
            _stats = GetComponent<StatsComponent>();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _stats.DetectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stats.AttackRange);

        if (CurrentTarget != null)
        {
            bool inAttackRange =
                CurrentTargetSqrDistance <= _stats.AttackRange * _stats.AttackRange;

            Gizmos.color = inAttackRange ? Color.red : Color.cyan;

            Gizmos.DrawLine(transform.position, CurrentTarget.position);
            Gizmos.DrawSphere(CurrentTarget.position, 0.15f);
        }
    }
#endif
}
