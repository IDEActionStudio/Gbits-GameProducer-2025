using EnemySystem.Core.Interfaces;
using UnityEngine;

namespace EnemySystem.Modules.Combat
{
    /// <summary>
    /// 投射物控制系统
    /// </summary>
    public class ProjectileBase : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 3f;
        [SerializeField] private GameObject impactEffect;

        private float _speed;
        private float _damage;
        private float _spawnTime;

        public void Initialize(float speed, float damage)
        {
            _speed = speed;
            _damage = damage;
            _spawnTime = Time.time;
        }

        private void Update()
        {
            transform.position += transform.forward * (_speed * Time.deltaTime);
            
            if (Time.time - _spawnTime > lifeTime)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(_damage);
                Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}