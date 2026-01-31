using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float m_damage;
    private Vector3 m_dir;

    private float m_speed;

    private int m_projectilePierce;

    public void Init(float damage, Vector3 dir, float speeed,int projectilePierce)
    {
        m_damage = damage;
        m_dir = dir.normalized;
        m_speed = speeed;
        m_projectilePierce= projectilePierce;
    }

    private void Update()
    {
        transform.position += m_dir * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var dmg = other.GetComponentInChildren<IDamageable>();
        if (dmg == null)
            return;

        dmg.TakeDamage((int)m_damage);
        m_projectilePierce--;

        if (m_projectilePierce <= 0)
            Destroy(gameObject); // pool later
    }
}
