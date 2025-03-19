using EnemySystem.Core.Managers;
using EnemySystem.Utilities.Pooling;
using UnityEngine;
using UnityEngine.Events;

namespace EnemySystem.Modules.Combat
{
    [System.Serializable]
    public class HealthSystem : MonoBehaviour
    {
        [Header("Base Settings")]
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private bool _invincible = false;

        [Header("Events")]
        public UnityEvent<int> OnHealthChanged;
        public UnityEvent OnDeath;
        public UnityEvent OnReset;

        private int _currentHealth;
        private bool _isDead;

        // 初始化时自动重置
        private void Awake() => Reset();

        public void Initialize(int maxHealth)
        {
            _maxHealth = maxHealth;
            Reset();
        }

        public void ApplyDamage(int damage)
        {
            if (_invincible || _isDead) return;

            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            OnHealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0)
                TriggerDeath();
        }

        public void Heal(int amount)
        {
            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
            OnHealthChanged?.Invoke(_currentHealth);
        }

        public void Reset()
        {
            _currentHealth = _maxHealth;
            _isDead = false;
            OnReset?.Invoke();
            OnHealthChanged?.Invoke(_currentHealth);
        }

        public void SetInvincible(bool state) => _invincible = state;

        private void TriggerDeath()
        {
            _isDead = true;
            OnDeath?.Invoke();
            
            // 自动回收到对象池
            // if (TryGetComponent<EnemyPool.EnemyPoolItem>(out var poolItem))
            // {
            //     EnemyPoolCoordinator.ReturnEnemy(gameObject);
            // }
        }

        // 属性暴露
        public int CurrentHealth => _currentHealth;
        public float HealthPercentage => (float)_currentHealth / _maxHealth;
        public bool IsDead => _isDead;
    }
}