using EnemySystem.Core.StateMachine;
using EnemySystem.Data.Configs;
using EnemySystem.Data.Enums;
using EnemySystem.Modules.Combat;
using EnemySystem.Modules.Movement;
using EnemySystem.Modules.Perception;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySystem.Core.AI
{
    /// <summary>
    /// 敌人AI主控制器 - 作为状态机的核心上下文(Context)
    /// 实现模块化设计，提供以下核心功能：
    /// 1. 导航系统配置管理
    /// 2. 模块间通信枢纽
    /// 3. 状态机上下文数据提供
    /// 4. 生命周期管理
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [DisallowMultipleComponent] // 防止重复添加组件
    public class EnemyBrain : MonoBehaviour
    {
        [Header("核心配置")]
        [Tooltip("敌人基础配置(攻击范围/移动速度等)")]
        [SerializeField] private EnemyConfig config;

        [Header("模块引用")]
        [Tooltip("感知系统(视野/听觉检测)")]
        [SerializeField] private PerceptionModule perception;
        [Tooltip("移动控制系统(巡逻/追击)")] 
        [SerializeField] private MovementModule movement;
        [Tooltip("战斗系统(攻击/防御)")]
        [SerializeField] private CombatModule combat;

        // 上下文核心组件
        private NavMeshAgent agent;
        private EnemyStateMachine stateMachine;
        
        #region 上下文属性 (供状态机访问)
        /// <summary>
        /// 导航代理接口（状态机通过此属性控制移动）
        /// </summary>
        public NavMeshAgent Agent => agent;
        
        /// <summary>
        /// 当前配置参数（只读访问）
        /// </summary>
        public EnemyConfig Config => config;
        
        /// <summary>
        /// 感知系统接口（状态机通过此访问检测数据）
        /// </summary>
        public PerceptionModule Perception => perception;
        
        /// <summary>
        /// 移动系统接口（状态机控制移动模式）
        /// </summary>
        public MovementModule Movement => movement;
        
        /// <summary>
        /// 战斗系统接口（状态机触发攻击行为）
        /// </summary>
        public CombatModule Combat => combat;

        /// <summary>
        /// 战斗系统接口（状态机触发攻击行为）
        /// </summary>
        public EnemyStateMachine StateMachine => stateMachine;
        public EnemyStateType CurrentState => stateMachine.CurrentState;

        #endregion

        private void Awake()
        {
            // 初始化导航系统
            agent = GetComponent<NavMeshAgent>();
            ValidateComponents();
            InitializeModules();
        }

        /// <summary>
        /// 敌人实例初始化（对象池或生成点调用）
        /// </summary>
        /// <param name="config">敌人配置参数</param>
        /// <param name="spawnPoint">生成位置</param>
        public void Initialize(EnemyConfig config, Vector3 spawnPoint)
        {
            this.config = config;
            
            // 配置导航参数
            ConfigureNavAgent();
            
            // 初始化移动系统（生成巡逻路径）
            movement.InitializePatrol(spawnPoint, config.patrolRadius);
            
            // 创建状态机并注入上下文
            stateMachine = new EnemyStateMachine(this);
            
            // 注册模块事件
            RegisterModuleEvents();
        }

        /// <summary>
        /// 配置导航代理参数
        /// </summary>
        private void ConfigureNavAgent()
        {
            agent.speed = config.moveSpeed;
            agent.angularSpeed = config.rotationSpeed;
            // agent.acceleration = config.acceleration; // 新增加速度参数
            agent.stoppingDistance = config.attackRange * 0.8f;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            // agent.avoidancePriority = config.aiPriority; // 新增优先级参数
        }

        /// <summary>
        /// 初始化各功能模块
        /// </summary>
        private void InitializeModules()
        {
            // 初始化感知系统（添加检测层级过滤）
            perception.Initialize(config.detectionRadius);
            // perception.Initialize(config.detectionRadius, LayerMask.GetMask("Player", "Ally"));
            
            // 初始化战斗系统（添加攻击冷却时间）
            combat.Initialize(config.attackDamage, config.attackType, config.attackInterval, config.attackRange);
            
            // 初始化移动系统（设置路径更新频率）
            movement.Initialize(config.moveSpeed, config.accelerationSpeed);
        }

        /// <summary>
        /// 注册模块事件监听
        /// </summary>
        private void RegisterModuleEvents()
        {
            // 当感知到目标时通知状态机
            perception.OnTargetDetected += (target) => 
                stateMachine.HandleEvent(EnemyEventType.TargetDetected);
            
            // 当丢失目标时通知状态机
            perception.OnTargetLost += () => 
                stateMachine.HandleEvent(EnemyEventType.TargetLost);
            
            // 当进入攻击范围时触发状态转换
            combat.OnEnterAttackRange += () => 
                stateMachine.HandleEvent(EnemyEventType.ReadyToAttack);
        }

        /// <summary>
        /// 组件完整性校验（开发期错误预防）
        /// </summary>
        private void ValidateComponents()
        {
            if (perception == null)
                Debug.LogError("PerceptionModule 未分配！", this);
            if (movement == null)
                Debug.LogError("MovementModule 未分配！", this);
            if (combat == null)
                Debug.LogError("CombatModule 未分配！", this);
        }

        #if UNITY_EDITOR
        /// <summary>
        /// 编辑器调试可视化
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            // 绘制感知范围
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, config.detectionRadius);
            
            // 绘制攻击范围
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, config.attackRange);
            
            // 绘制当前路径
            if (agent != null && agent.hasPath)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, agent.destination);
            }
        }
        #endif
    }
}