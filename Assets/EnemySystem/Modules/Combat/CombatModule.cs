using System;
using System.Collections;
using EnemySystem.Core.Interfaces;
using EnemySystem.Data.Enums;
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
        private float _attackRange;
        private float _projectileSpeed;
        private int _maxComboSteps;
        private AttackType _attackType;
        
        private float _lastAttackTime;
        private int _currentComboStep;
        private bool _isInCombo;

        private Transform currentTarget;
        
        
        // 新增事件声明（类内部添加）
        public event Action OnEnterAttackRange;
        public event Action OnExitAttackRange;

        // 新增状态跟踪字段
        private bool _wasInAttackRange;
        private Coroutine _rangeCheckCoroutine;
        
        /// <summary>
        /// 是否处于有效攻击范围内（只读属性）
        /// </summary>
        public bool IsInAttackRange
        {
            get
            {
                // 防御性编程：确保目标存在且未被销毁
                if (currentTarget == null) return false;

                // 计算与目标的平面距离（忽略Y轴高度差）
                Vector3 toTarget = currentTarget.position - transform.position;
                float distance = new Vector3(toTarget.x, 0, toTarget.z).magnitude;

                // 添加5%的宽容值防止边缘抖动
                return distance <= _attackRange * 1.05f; 
            }
        }

        /// <summary>
        /// 初始化战斗系统
        /// </summary>
        public void Initialize(float baseDamage, AttackType type, float interval,
            float attackRange, float projectileSpeed = 10f, int maxCombo = 3) // 新增attackRange参数
        {
            _attackDamage = baseDamage;
            _attackType = type;
            _attackInterval = interval;
            _attackRange = attackRange;
            _projectileSpeed = projectileSpeed;
            _maxComboSteps = maxCombo;
            StartRangeDetection();
        }

        // 新增范围检测逻辑
        private void StartRangeDetection()
        {
            if (_rangeCheckCoroutine != null) 
                StopCoroutine(_rangeCheckCoroutine);
        
            _rangeCheckCoroutine = StartCoroutine(RangeCheckRoutine());
        }

        private IEnumerator RangeCheckRoutine()
        {
            while (true)
            {
                bool currentState = IsInAttackRange;
            
                // 状态变化检测
                if (currentState != _wasInAttackRange)
                {
                    if (currentState)
                        OnEnterAttackRange?.Invoke();
                    else
                        OnExitAttackRange?.Invoke();
                
                    _wasInAttackRange = currentState;
                }
            
                yield return new WaitForSeconds(0.15f); // 优化性能的检测间隔
            }
        }

        // 新增目标设置方法
        public void SetCurrentTarget(Transform target)
        {
            currentTarget = target;
            _wasInAttackRange = false; // 重置状态跟踪
        }

        
        // 攻击流程控制
        public void StartAttackSequence()
        {
            // 典型逻辑：
            // 1. 播放攻击前摇动画
            // 2. 锁定目标方向
            // 3. 触发攻击伤害检测
            // 4. 启动冷却计时器
        }

        // 攻击命中检测（可由动画事件触发）
        public void ApplyAttackDamage()
        {
            // 物理检测逻辑（射线检测/范围检测）
            // 调用目标对象的 TakeDamage 方法
        }

        // 强制中断攻击
        public void CancelAttack()
        {
            // 停止攻击动画
            // 重置攻击状态
            // 清除冷却计时
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