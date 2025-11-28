using UnityEngine;

public class MovementPhysicsCore : CoreComponent
{
    [SerializeField] private float m_speed = 5f;
    private Rigidbody m_rb;
    protected override void Awake()
    {
        base.Awake();
        m_rb = GetComponentInParent<Rigidbody>();
    }

    public void Move(Vector3 input)
    {
        if (m_rb == null) return;
        if (input.sqrMagnitude < 0.001f) return;

        Vector3 nextPos = m_rb.position + input * m_speed * Time.deltaTime;
        m_rb.MovePosition(nextPos);
    }

}
