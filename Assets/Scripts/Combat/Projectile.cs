using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float m_damage;
    private Vector3 m_dir;

    [SerializeField] private float m_speed = 20f;

    public void Init(float damage, Vector3 dir)
    {
        m_damage = damage;
        m_dir = dir.normalized;
    }

    private void Update()
    {
        transform.position += m_dir * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var dmg))
        {
            dmg.TakeDamage((int)m_damage);
            Destroy(gameObject);
        }
    }
}
