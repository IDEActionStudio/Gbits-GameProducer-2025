using EnemySystem.Core.AI;
using EnemySystem.Core.StateMachine.Base;
using EnemySystem.Data.Enums;

namespace EnemySystem.Core.StateMachine.States
{
    /// <summary>
    /// 敌人追击状态实现类
    /// 核心职责：处理敌人对目标的持续追踪逻辑
    /// 主要功能：
    /// 1. 初始化追击参数
    /// 2. 持续更新移动目标
    /// 3. 监控攻击条件达成
    /// </summary>
    public class ChaseState : EnemyBaseState
    {
        /// <summary>
        /// 状态类型标识符（只读属性）
        /// 对应枚举值：EnemyStateType.Chase
        /// </summary>
        public override EnemyStateType Type => EnemyStateType.Chase;

        /// <summary>
        /// 构造函数：注入敌人AI上下文
        /// </summary>
        /// <param name="context">敌人AI的运行时上下文（包含移动组件、感知系统等）</param>
        /// <remarks>
        /// 通过基类构造函数初始化公共字段，确保上下文数据在基类中可用
        /// </remarks>
        public ChaseState(EnemyBrain context) : base(context) { }

        /// <summary>
        /// 状态进入时执行的方法（重写自基类）
        /// 初始化追击行为参数
        /// </summary>
        /// <remarks>
        /// 典型执行流程：
        /// 1. 设置移动速度加成
        /// 2. 锁定当前感知到的主要目标
        /// 3. 启动导航系统
        /// 4. 播放追击相关动画
        /// </remarks>
        protected override void Enter()
        {
            // 设置移动速度加成（1.5倍基础速度）
            Context.Movement.SetSpeedMultiplier(1.5f);
            
            // 设置导航目标为感知系统检测到的主要目标位置
            Context.Movement.SetDestination(Context.Perception.PrimaryTarget.position);
            
            // 典型扩展点：
            // - 播放追击启动特效
            // - 触发追击状态音效
            // - 记录AI行为日志
        }

        /// <summary>
        /// 每帧更新逻辑（重写自基类）
        /// 主要职责：
        /// 1. 维持追击行为
        /// 2. 实时检测攻击条件
        /// 3. 处理状态转换
        /// </summary>
        /// <remarks>
        /// 执行顺序保证：
        /// 在EnemyStateMachine的Update循环中调用
        /// 先于状态转换检测执行
        /// </remarks>
        protected override void Update()
        {
            // 核心条件检测：是否进入攻击范围
            if (Context.Combat.IsInAttackRange)
            {
                // 通过上下文访问状态机，触发向攻击状态的转换
                Context.StateMachine.TransitionTo(EnemyStateType.Attack);
            }
            
            // 典型可扩展功能：
            // - 实时更新目标位置（见注意事项）
            // - 检测目标丢失情况
            // - 处理路径受阻逻辑
            // - 计算剩余追击距离
        }
    }
}