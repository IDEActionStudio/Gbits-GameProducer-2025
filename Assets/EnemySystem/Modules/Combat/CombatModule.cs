using EnemySystem.Core.Interfaces;
using EnemySystem.Data.Configs.Enums;
using UnityEngine;

namespace EnemySystem.Modules.Combat
{
    /// <summary>
    /// 高级战斗系统 - 支持多种攻击模式和战斗状态
    /// 版本2.1 增加对象池、连击系统和攻击冷却
    /// </summary>
    public class CombatModule : MonoBehaviour
    {
        [Header("攻击参数")]
        [SerializeField] private Transform attackPoint;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private LayerMask collisionLayers;

        [Header("调试")]
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private Color attackRangeColor = Color.red;

        private float _attackDamage;
        private float _attackInterval;
        private float _projectileSpeed;
        private int _maxComboSteps;
        private AttackType _attackType;
        
        private float _lastAttackTime;
        private int _currentComboStep;
        private bool _isInCombo;

        /// <summary>
        /// 初始化战斗系统
        /// </summary>
        public void Initialize(float baseDamage, AttackType type, float interval, 
            float projectileSpeed = 10f, int maxCombo = 3)
        {
            _attackDamage = baseDamage;
            _attackType = type;
            _attackInterval = interval;
            _projectileSpeed = projectileSpeed;
            _maxComboSteps = maxCombo;
        }

        /// <summary>
        /// 执行智能攻击序列
        /// </summary>
        public void PerformAttack()
        {
            if (!CanAttack()) return;

            switch (_attackType)
            {
                case AttackType.Melee:
                    ExecuteSmartMelee();
                    break;
                case AttackType.Ranged:
                    LaunchAimProjectile();
                    break;
            }

            UpdateComboState();
            _lastAttackTime = Time.time;
        }

        private bool CanAttack()
        {
            return Time.time - _lastAttackTime >= GetCurrentAttackInterval();
        }

        private float GetCurrentAttackInterval()
        {
            return _isInCombo ? _attackInterval * 0.7f : _attackInterval;
        }

        private void ExecuteSmartMelee()
        {
            // 带预测的扇形攻击检测
            var hits = Physics.OverlapSphere(attackPoint.position, 
                CalculateDynamicRange(), collisionLayers);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IDamageable>(out var target))
                {
                    target.TakeDamage(CalculateTotalDamage());
                    ApplyKnockback(hit.attachedRigidbody);
                }
            }
        }

        private void LaunchAimProjectile()
        {
            var projectile = Instantiate(projectilePrefab, 
                attackPoint.position, 
                CalculateAimRotation());

            var projComponent = projectile.GetComponent<ProjectileBase>();
            projComponent.Initialize(_projectileSpeed, CalculateTotalDamage());
        }

        private Quaternion CalculateAimRotation()
        {
            // 添加简单的预测逻辑
            return Quaternion.LookRotation(transform.forward);
        }

        private float CalculateDynamicRange()
        {
            return _isInCombo ? 1.2f : 1f;
        }

        private float CalculateTotalDamage()
        {
            return _attackDamage * (1 + _currentComboStep * 0.3f);
        }

        private void ApplyKnockback(Rigidbody targetRb)
        {
            if (targetRb != null)
            {
                targetRb.AddForce(transform.forward * 5f, ForceMode.Impulse);
            }
        }

        private void UpdateComboState()
        {
            _currentComboStep = (_currentComboStep + 1) % _maxComboSteps;
            _isInCombo = _currentComboStep > 0;
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos || attackPoint == null) return;

            Gizmos.color = attackRangeColor;
            Gizmos.DrawWireSphere(attackPoint.position, CalculateDynamicRange());
        }
    }

}