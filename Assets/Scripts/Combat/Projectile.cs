using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        private float _damage;
        private float _speed;

        private List<Transform> _targets;
        private int _index;

        public void Init(
            float damage,
            List<Transform> targets,
            float speed)
        {
            _damage = damage;
            _targets = targets;
            _speed = speed;
            _index = 0;
        }

        private void Update()
        {
            if (_index >= _targets.Count)
            {
                Destroy(gameObject);
                return;
            }

            Transform target = _targets[_index];
            if (target == null)
            {
                _index++;
                return;
            }

            Vector3 dir = (target.position - transform.position);
            dir.y = 0f;

            transform.position += dir.normalized * _speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_index >= _targets.Count)
                return;
            Transform hitRoot = other.transform;
            if (hitRoot != _targets[_index])
                return;
            var dmg = hitRoot.GetComponentInChildren<IDamageable>();
            if (dmg == null)
                return;

            dmg.TakeDamage((int)_damage);

            _index++; 
        }

    }

}
