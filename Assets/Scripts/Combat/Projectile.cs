using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        private float m_damage;
        private float m_speed;
        private Transform m_source;
        private List<Transform> m_targets;
        private int m_index;

        public void Init(
            float damage,
            List<Transform> targets,
            Transform source,
            float speed)
        {
            m_damage = damage;
            m_targets = targets;
            m_speed = speed;
            m_source = source;
            m_index = 0;
        }

        private void Update()
        {
            if (m_index >= m_targets.Count)
            {
                Destroy(gameObject);
                return;
            }

            Transform target = m_targets[m_index];
            if (target == null)
            {
                m_index++;
                return;
            }

            Vector3 dir = (target.position - transform.position);
            dir.y = 0f;

            transform.position += dir.normalized * m_speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_index >= m_targets.Count)
                return;
            Transform hitRoot = other.transform;
            if (hitRoot != m_targets[m_index])
                return;
            var dmg = hitRoot.GetComponentInChildren<IDamageable>();
            if (dmg == null)
                return;

            dmg.TakeDamage((int)m_damage, m_source);

            m_index++; 
        }

    }

}
