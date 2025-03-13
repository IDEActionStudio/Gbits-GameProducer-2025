using UnityEngine;
using UnityEngine.AI;

namespace EnemySystem.Modules.Movement
{
    /// <summary>
    /// 智能移动控制系统
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementModule : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Vector3 _patrolCenter;

        private void Awake() => _agent = GetComponent<NavMeshAgent>();

        /// <summary>
        /// 初始化巡逻参数
        /// </summary>
        public void InitializePatrol(Vector3 center, float radius)
        {
            _patrolCenter = center;
            SetDestination(GetRandomPatrolPoint(radius));
        }

        /// <summary>
        /// 设置导航目标
        /// </summary>
        public void SetDestination(Vector3 target)
        {
            if (_agent.isActiveAndEnabled)
                _agent.SetDestination(target);
        }

        /// <summary>
        /// 生成随机巡逻点
        /// </summary>
        private Vector3 GetRandomPatrolPoint(float radius)
        {
            var randomPoint = _patrolCenter + Random.insideUnitSphere * radius;
            NavMesh.SamplePosition(randomPoint, out var hit, radius, NavMesh.AllAreas);
            return hit.position;
        }

        /// <summary>
        /// 设置移动速度倍率
        /// </summary>
        public void SetSpeedMultiplier(float multiplier)
        {
            _agent.speed *= multiplier;
        }
    }
}