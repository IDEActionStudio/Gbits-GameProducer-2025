using System.Collections;
using EnemySystem.Data.Configs;
using EnemySystem.Data.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySystem.Modules.Movement.Pathfinding
{
    // [RequireComponent(typeof(NavMeshAgent))]
    // public class NavMeshAdapter : MonoBehaviour
    // {
    //     private NavMeshAgent _agent;
    //     private Vector3 _currentDestination;
    //     private bool _isPathPending;
    //
    //     // 暴露给MovementModule的关键参数
    //     public float RemainingDistance => _agent.remainingDistance;
    //     public bool HasPath => _agent.hasPath;
    //     public bool PathPending => _agent.pathPending;
    //
    //     private void Awake()
    //     {
    //         _agent = GetComponent<NavMeshAgent>();
    //         _agent.updateRotation = false; // 由MovementModule控制旋转
    //     }
    //
    //     /// <summary>
    //     /// 设置导航目标点（包含多层校验）
    //     /// </summary>
    //     public bool SetDestination(Vector3 target)
    //     {
    //         if (!NavMesh.SamplePosition(target, out var hit, 1.0f, NavMesh.AllAreas))
    //         {
    //             Debug.LogWarning($"Invalid destination: {target}");
    //             return false;
    //         }
    //
    //         _currentDestination = hit.position;
    //         return _agent.SetDestination(_currentDestination);
    //     }
    //
    //     /// <summary>
    //     /// 动态更新路径（适用于追逐场景）
    //     /// </summary>
    //     public void UpdatePathWithPriority(Vector3 newTarget, float recalcInterval = 0.5f)
    //     {
    //         if (Time.frameCount % Mathf.FloorToInt(recalcInterval / Time.deltaTime) != 0) return;
    //         
    //         if (Vector3.Distance(newTarget, _currentDestination) > _agent.stoppingDistance)
    //         {
    //             SetDestination(newTarget);
    //         }
    //     }
    //
    //     /// <summary>
    //     /// 停止导航并清除路径
    //     /// </summary>
    //     public void StopNavigation()
    //     {
    //         _agent.isStopped = true;
    //         _agent.ResetPath();
    //         _currentDestination = Vector3.negativeInfinity;
    //     }
    //
    //     /// <summary>
    //     /// 配置导航参数（从EnemyConfig同步）
    //     /// </summary>
    //     public void ConfigureAgent(EnemyConfig config)
    //     {
    //         _agent.speed = config.MoveSpeed;
    //         _agent.angularSpeed = config.RotationSpeed;
    //         _agent.acceleration = config.Acceleration;
    //         _agent.stoppingDistance = config.StoppingDistance;
    //         
    //         // 根据敌人类型设置导航区域掩码
    //         _agent.areaMask = config.EnemyType switch {
    //             EnemyType.Flying => NavMesh.GetAreaFromName("Air"),
    //             EnemyType.Aquatic => NavMesh.GetAreaFromName("Water"),
    //             _ => NavMesh.AllAreas
    //         };
    //     }
    //
    //     /// <summary>
    //     /// 异步等待路径计算（配合协程使用）
    //     /// </summary>
    //     public IEnumerator WaitForPathCalculation()
    //     {
    //         while (_agent.pathPending)
    //         {
    //             yield return null;
    //         }
    //         
    //         if (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
    //         {
    //             EnemyEventSystem.TriggerEvent(EnemyEventType.PathfindingFailed);
    //         }
    //     }
    // }
}