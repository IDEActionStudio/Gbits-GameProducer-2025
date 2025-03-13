using EnemySystem.Core.StateMachine;
using EnemySystem.Data.Configs;
using EnemySystem.Modules.Combat;
using EnemySystem.Modules.Movement;
using EnemySystem.Modules.Perception;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySystem.Core.AI
{
    /// <summary>
    /// 敌人AI主控制器
    /// 集成导航、状态机和模块化组件
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyBrain : MonoBehaviour
    {
        [Header("核心配置")]
        [SerializeField] private EnemyConfig config;

        [Header("模块引用")]
        [SerializeField] private PerceptionModule perception;
        [SerializeField] private MovementModule movement;
        [SerializeField] private CombatModule combat;

        private NavMeshAgent agent;
        private EnemyStateMachine stateMachine;

        public EnemyConfig Config => config;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            InitializeModules();
        }

        /// <summary>
        /// 初始化敌人实例
        /// </summary>
        public void Initialize(EnemyConfig config, Vector3 spawnPoint)
        {
            this.config = config;
            ConfigureNavAgent();
            movement.InitializePatrol(spawnPoint, config.patrolRadius);
            stateMachine = new EnemyStateMachine(this);
        }

        private void ConfigureNavAgent()
        {
            agent.speed = config.moveSpeed;
            agent.angularSpeed = config.rotationSpeed;
            agent.stoppingDistance = config.attackRange * 0.8f;
        }

        private void InitializeModules()
        {
            perception.Initialize(config.detectionRadius);
            combat.Initialize(config.attackDamage);
        }
    }
}