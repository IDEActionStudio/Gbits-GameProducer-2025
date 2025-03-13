using UnityEngine;

namespace EnemySystem.Modules.Perception
{
    /// <summary>
    /// 高级感知系统 - 处理视觉和听觉检测
    /// </summary>
    public class PerceptionModule : MonoBehaviour
    {
        [Header("检测参数")]
        [SerializeField] private float visionCheckInterval = 0.2f;
        [SerializeField] private LayerMask obstacleLayers;

        private float _lastCheckTime;
        private Transform _playerTransform;

        /// <summary>
        /// 当前检测到的目标
        /// </summary>
        public Transform CurrentTarget { get; private set; }

        private void Update()
        {
            if (Time.time - _lastCheckTime >= visionCheckInterval)
            {
                DetectPlayer();
                _lastCheckTime = Time.time;
            }
        }

        /// <summary>
        /// 执行玩家检测逻辑
        /// </summary>
        private void DetectPlayer()
        {
            if (Physics.SphereCast(transform.position, 1f, transform.forward, 
                    out var hit, detectionRange, targetLayer))
            {
                if (hit.transform.CompareTag("Player") && HasLineOfSight(hit.transform))
                {
                    CurrentTarget = hit.transform;
                }
            }
        }

        /// <summary>
        /// 判断是否具有直接视线
        /// </summary>
        private bool HasLineOfSight(Transform target)
        {
            var direction = target.position - transform.position;
            return !Physics.Raycast(transform.position, direction.normalized, 
                direction.magnitude, obstacleLayers);
        }
    }
}