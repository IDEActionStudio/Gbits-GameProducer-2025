using UnityEngine;

namespace EnemySystem.Modules.Perception
{
    /// <summary>
    /// 高级感知系统 - 处理视觉和听觉检测
    /// 版本2.0 支持多目标检测和调试可视化
    /// </summary>
    public class PerceptionModule : MonoBehaviour
    {
        [Header("检测参数")]
        [SerializeField] private LayerMask targetLayer;    // 目标层级（如玩家）
        [SerializeField] private LayerMask obstacleLayers; // 障碍物层级
        [SerializeField] [Range(0.1f, 1f)] private float visionCheckInterval = 0.2f;
        [SerializeField] private float visionAngle = 120f; // 视野角度
        
        [Header("调试")]
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private Color detectionColor = Color.yellow;

        private float _detectionRadius;
        private Transform _mainTarget;
        private Collider[] _detectedColliders = new Collider[5];

        /// <summary>
        /// 当前主要威胁目标
        /// </summary>
        public Transform PrimaryTarget => _mainTarget;

        /// <summary>
        /// 初始化感知系统
        /// </summary>
        /// <param name="detectionRadius">检测半径</param>
        public void Initialize(float detectionRadius)
        {
            _detectionRadius = detectionRadius;
            InvokeRepeating(nameof(PerformDetection), 0, visionCheckInterval);
        }

        private void PerformDetection()
        {
            int found = Physics.OverlapSphereNonAlloc(
                transform.position,
                _detectionRadius,
                _detectedColliders,
                targetLayer
            );

            _mainTarget = null;
            
            for (int i = 0; i < found; i++)
            {
                if (IsTargetValid(_detectedColliders[i].transform))
                {
                    _mainTarget = _detectedColliders[i].transform;
                    break; // 优先选择第一个有效目标
                }
            }
        }

        private bool IsTargetValid(Transform target)
        {
            // 视线检测
            Vector3 direction = target.position - transform.position;
            if (Vector3.Angle(transform.forward, direction) > visionAngle / 2) 
                return false;

            // 障碍物检测
            return !Physics.Raycast(
                transform.position,
                direction.normalized,
                direction.magnitude,
                obstacleLayers
            );
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;

            Gizmos.color = detectionColor;
            // 绘制检测球体
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
            
            // 绘制视野扇形
            Vector3 leftBound = Quaternion.Euler(0, -visionAngle/2, 0) * transform.forward * _detectionRadius;
            Vector3 rightBound = Quaternion.Euler(0, visionAngle/2, 0) * transform.forward * _detectionRadius;
            
            Gizmos.DrawLine(transform.position, transform.position + leftBound);
            Gizmos.DrawLine(transform.position, transform.position + rightBound);
            Gizmos.DrawLine(transform.position + leftBound, transform.position + rightBound);
        }
    }
}