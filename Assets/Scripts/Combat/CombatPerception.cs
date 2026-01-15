using UnityEngine;

public class CombatPerception : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;

    private StatsComponent _stats;
    public float DetectRange { get; private set; }
    public bool UseDetectRange { get; private set; }
    public bool HasTargetInDetectRange { get; private set; }
    public bool HasTargetInAttackRange { get; private set; }
    public Transform CurrentTarget { get; private set; }

    #region Scan Interval
    [SerializeField] private float scanInterval = 0.15f;
    private float _scanTimer;

    private readonly Collider[] _hitsBuffer = new Collider[16];
    #endregion

    private void Awake()
    {
        _stats = GetComponent<StatsComponent>();
        _scanTimer = Random.Range(0f, scanInterval);
    }

    public void Initialize(float detectRange = 0, bool useDetectRange=false)
    {
        DetectRange = detectRange;
        UseDetectRange = useDetectRange;
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
        HasTargetInDetectRange = false;
        HasTargetInAttackRange = false;
        CurrentTarget = null;

        float scanRange = UseDetectRange ? DetectRange : _stats.AttackRange;
        float scanRangeSqr = scanRange * scanRange;

        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            scanRange,
            _hitsBuffer,
            targetLayer
        );

        if (hitCount == 0)
            return;

        float minSqrDist = float.MaxValue;

        for (int i = 0; i < hitCount; i++)
        {
            var hit = _hitsBuffer[i];
            if (hit == null) continue;

            float sqrDist =
                (hit.transform.position - transform.position).sqrMagnitude;

            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                CurrentTarget = hit.transform;
            }
        }

        if (CurrentTarget == null)
            return;

        if (UseDetectRange)
            HasTargetInDetectRange = true;

        if (minSqrDist <= _stats.AttackRange * _stats.AttackRange)
            HasTargetInAttackRange = true;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (UseDetectRange)
            Gizmos.DrawWireSphere(transform.position, DetectRange);

        if (_stats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _stats.AttackRange);
        }
        if (CurrentTarget != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, CurrentTarget.position);
        }
    }
#endif

}
