using EnemySystem.Core.AI;
using UnityEngine;

namespace EnemySystem.Testing.Debug
{
     /// <summary>
        /// 敌人调试可视化工具
        /// </summary>
        [ExecuteAlways]

    public class DebugEnemy : MonoBehaviour
    {
        [Header("显示设置")] public bool showDetectionRange = true;
        public Color detectionColor = Color.yellow;

        [Header("状态显示")] public bool displayState = true;
        public Vector3 stateLabelOffset = new Vector3(0, 2f, 0);

        private EnemyBrain _brain;

        private void Awake()
        {
            _brain = GetComponent<EnemyBrain>();
        }

        private void OnDrawGizmos()
        {
            if (!_brain || !_brain.Config) return;

            if (showDetectionRange)
            {
                Gizmos.color = detectionColor;
                Gizmos.DrawWireSphere(transform.position, _brain.Config.detectionRadius);
            }
        }

        private void OnGUI()
        {
            if (displayState && _brain != null)
            {
                var screenPos = Camera.main.WorldToScreenPoint(transform.position + stateLabelOffset);
                GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 200, 20),
                    $"State: {_brain.CurrentState}");
            }
        }

    }