using CoreScripts.ObjectPool;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        private Vector3 m_dir;
        private ComponentPool<Projectile> m_ownerPool;
        private IAttackContext m_context;

        public void Init(
            float damage,
            List<Transform> targets,
            Transform source,
            float speed,
            ComponentPool<Projectile> ownerPool,
            IAttackContext context)
        {
            m_damage = damage;
            m_targets = targets;
            m_speed = speed;
            m_source = source;
            m_index = 0;
            m_ownerPool = ownerPool;
            m_context = context;
        }

        private void Update()
        {
            if (m_targets == null || m_targets.Count == 0)
            {
                Despawn();
                return;
            }

            if (m_index >= m_targets.Count)
            {
                Despawn();
                return;
            }

            Transform target = m_targets[m_index];
            if (target == null)
            {
                m_index++;
                return;
            }

            Vector3 dir = target.position - transform.position;
            dir.y = 0f;

            transform.position += dir.normalized * m_speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_index >= m_targets.Count)
                return;

            if (other.transform != m_targets[m_index])
                return;

            var entity = other.GetComponent<BaseEntity>();
            if (entity == null)
                return;

            m_context.ApplyDamage(entity, (int)m_damage);

            m_index++;
        }

        private void Despawn()
        {
            m_ownerPool.Return(this); 
        }
    }
}