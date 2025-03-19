using EnemySystem.Core.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace EnemySystem.Modules.Movement
{
    /// <summary>
    /// 智能移动控制系统（支持多模式移动）
    /// 功能特性：
    /// 1. 自动导航路径优化
    /// 2. 移动状态事件通知
    /// 3. 多停止条件支持
    /// 4. 移动异常处理
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementModule : MonoBehaviour
    {
        // 移动状态事件
        public event UnityAction OnDestinationReached; // 抵达目的地
        public event UnityAction OnPathInvalid;        // 路径无效

        
        private float _speedMultiplier = 1f;
        
        // 导航相关组件
        private NavMeshAgent _agent;
        private Vector3 _patrolCenter;
        private float _pathUpdateInterval = 0.5f;
        private float _lastPathUpdateTime;

        // 移动模式枚举
        public enum MoveMode
        {
            Idle,     // 静止
            Patrol,   // 巡逻
            Chase,    // 追击
            Retreat   // 撤退
        }
        private MoveMode _currentMode = MoveMode.Idle;

        #region 初始化配置
        /// <summary>
        /// 模块初始化（由EnemyBrain统一调用）
        /// </summary>
        /// <param name="baseSpeed">基础移动速度</param>
        /// <param name="acceleration">导航代理加速度</param>
        /// <param name="updateInterval">路径更新频率（秒）</param>
        public void Initialize(float baseSpeed, float acceleration, float updateInterval = 0.5f)
        {
            ValidateNavAgent();
            
            // 配置导航参数
            _agent.speed = baseSpeed;
            _agent.acceleration = acceleration;
            _pathUpdateInterval = updateInterval;
            
            // 初始化为静止状态
            SetMovementState(MoveMode.Idle);
            
            // 启用自动路径更新
            _agent.autoRepath = true;
            _agent.autoBraking = true;
        }

        /// <summary>
        /// 初始化巡逻系统（与主初始化分离）
        /// </summary>
        public void InitializePatrol(Vector3 center, float radius, float patrolSpeedMultiplier = 1.0f)
        {
            _patrolCenter = center;
            _agent.speed *= patrolSpeedMultiplier;
            SetMovementState(MoveMode.Patrol);
            UpdatePatrolDestination(radius);
        }
        #endregion

        /// <summary>
        /// 启动/重启巡逻行为
        /// 使用条件：需要先调用InitializePatrol完成初始化
        /// </summary>
        public void StartPatrol()
        {
            if (!_agent.isActiveAndEnabled || _currentMode == MoveMode.Patrol) 
                return;

            // 保持原有巡逻参数，仅重置路径
            SetMovementState(MoveMode.Patrol);
            UpdatePatrolDestination(ConfigPatrolRadius());
    
            // 触发首次路径更新
            _lastPathUpdateTime = Time.time - _pathUpdateInterval;
        }

        /// <summary>
        /// 设置速度倍率（影响当前移动模式的速度计算）
        /// </summary>
        /// <param name="multiplier">速度倍率（1.0表示原始速度）</param>
        public void SetSpeedMultiplier(float multiplier)
        {
            _speedMultiplier = Mathf.Max(0.1f, multiplier); // 防止负值
            RefreshMovementSpeed();
        }
        
        
        /// <summary>
        /// 刷新当前移动速度（根据模式和倍率重新计算）
        /// </summary>
        private void RefreshMovementSpeed()
        {
            switch (_currentMode)
            {
                case MoveMode.Patrol:
                    _agent.speed = ConfigPatrolSpeed() * _speedMultiplier;
                    break;
                case MoveMode.Chase:
                    _agent.speed = ConfigChaseSpeed() * _speedMultiplier;
                    break;
                default:
                    _agent.speed = _agent.speed * _speedMultiplier;
                    break;
            }
        }
        
        #region 核心逻辑
        private void Update()
        {
            if (_currentMode == MoveMode.Idle || !_agent.isActiveAndEnabled) return;

            // 定时路径更新检测
            if (Time.time - _lastPathUpdateTime > _pathUpdateInterval)
            {
                ValidateCurrentPath();
                _lastPathUpdateTime = Time.time;
            }

            // 抵达检测（考虑坡度误差）
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.1f)
            {
                OnDestinationReached?.Invoke();
                
                // 巡逻模式自动更新目标
                if (_currentMode == MoveMode.Patrol)
                {
                    UpdatePatrolDestination(ConfigPatrolRadius());
                }
            }
        }

        /// <summary>
        /// 设置导航目标（外部调用入口）
        /// </summary>
        public void SetDestination(Vector3 target, float stoppingDistance = 0.5f)
        {
            if (!_agent.isActiveAndEnabled) return;

            _agent.stoppingDistance = stoppingDistance;
            
            // 高性能路径检查
            if (NavMesh.SamplePosition(target, out var hit, 1.0f, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
            }
            else
            {
                OnPathInvalid?.Invoke();
                Debug.LogWarning($"无效导航目标: {target}", this);
            }
        }
        #endregion

        #region 状态管理
        /// <summary>
        /// 切换移动状态（供状态机调用）
        /// </summary>
        public void SetMovementState(MoveMode newMode)
        {
            _currentMode = newMode;
            
            switch (newMode)
            {
                case MoveMode.Idle:
                    _agent.isStopped = true;
                    break;
                case MoveMode.Patrol:
                    _agent.speed = ConfigPatrolSpeed();
                    _agent.isStopped = false;
                    break;
                case MoveMode.Chase:
                    _agent.speed = ConfigChaseSpeed();
                    _agent.isStopped = false;
                    break;
            }
        }
        #endregion

        #region 工具方法
        private void ValidateNavAgent()
        {
            if (_agent == null) _agent = GetComponent<NavMeshAgent>();
            
            if (!_agent.isOnNavMesh)
            {
                Debug.LogError("导航代理未放置在可行走区域！", this);
                enabled = false;
            }
        }

        private void ValidateCurrentPath()
        {
            if (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                OnPathInvalid?.Invoke();
            }
        }

        private void UpdatePatrolDestination(float radius)
        {
            var point = GenerateValidPatrolPoint(radius);
            SetDestination(point, ConfigPatrolStoppingDistance());
        }

        private Vector3 GenerateValidPatrolPoint(float radius)
        {
            const int maxAttempts = 5;
            for (int i = 0; i < maxAttempts; i++)
            {
                var randomPoint = _patrolCenter + Random.insideUnitSphere * radius;
                if (NavMesh.SamplePosition(randomPoint, out var hit, radius, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return _patrolCenter; // 退回中心点
        }
        #endregion

        #region 配置访问（从EnemyConfig获取）
        private float ConfigPatrolSpeed() => 
            GetComponent<EnemyBrain>().Config.patrolSpeed;

        private float ConfigChaseSpeed() => 
            GetComponent<EnemyBrain>().Config.chaseSpeed;

        private float ConfigPatrolRadius() => 
            GetComponent<EnemyBrain>().Config.patrolRadius;

        private float ConfigPatrolStoppingDistance() => 
            GetComponent<EnemyBrain>().Config.patrolStoppingDistance;
        #endregion

        #region 调试工具
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_currentMode == MoveMode.Patrol && Application.isPlaying)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_patrolCenter, ConfigPatrolRadius());
                Gizmos.DrawLine(transform.position, _agent.destination);
            }
        }
#endif
        #endregion
    }
}