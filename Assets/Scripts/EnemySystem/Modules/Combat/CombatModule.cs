using EnemySystem.Core.Interfaces;
using EnemySystem.Data.Configs;
using UnityEngine;

namespace EnemySystem.Modules.Combat
{
    /// <summary>
    /// 战斗管理系统 - 处理攻击逻辑
    /// </summary>
    public class CombatModule : MonoBehaviour
    {
        [Header("攻击参数")]
        [SerializeField] private Transform attackPoint;
        [SerializeField] private GameObject projectilePrefab;

        private float _lastAttackTime;
        private int _currentComboStep;

        /// <summary>
        /// 执行攻击序列
        /// </summary>
        public void PerformAttack(EnemyConfig config)
        {
            if (Time.time - _lastAttackTime < config.attackInterval) return;

            switch (config.attackType)
            {
                case AttackType.Melee:
                    ExecuteMeleeAttack(config);
                    break;
                case AttackType.Ranged:
                    LaunchProjectile(config);
                    break;
            }

            _lastAttackTime = Time.time;
            _currentComboStep = (_currentComboStep + 1) % config.maxComboSteps;
        }

        private void ExecuteMeleeAttack(EnemyConfig config)
        {
            var hitColliders = Physics.OverlapSphere(attackPoint.position, 
                config.attackRange, config.targetLayer);
            
            foreach (var collider in hitColliders)
            {
                if (collider.TryGetComponent<IDamageable>(out var target))
                {
                    target.TakeDamage(config.attackDamage);
                }
            }
        }

        private void LaunchProjectile(EnemyConfig config)
        {
            var projectile = Instantiate(projectilePrefab, 
                attackPoint.position, 
                Quaternion.LookRotation(transform.forward));
            
            projectile.GetComponent<Projectile>().Initialize(
                config.projectileSpeed,
                config.attackDamage);
        }
    }

    public enum AttackType { Melee, Ranged }
}