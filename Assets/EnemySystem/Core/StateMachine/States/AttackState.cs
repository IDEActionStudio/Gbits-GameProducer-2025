using EnemySystem.Core.AI;
using EnemySystem.Core.StateMachine.Base;
using EnemySystem.Data.Enums;

namespace EnemySystem.Core.StateMachine.States
{
    /// <summary>
    /// 敌人攻击状态实现类
    /// 核心职责：处理敌人进入攻击状态后的所有行为逻辑
    /// 主要功能：
    /// 1. 初始化攻击序列
    /// 2. 维持攻击行为
    /// 3. 检测攻击条件失效时的状态转换
    /// </summary>
    public class AttackState : EnemyBaseState
    {
        /// <summary>
        /// 状态类型标识符（只读属性）
        /// 对应枚举值：EnemyStateType.Attack
        /// </summary>
        public override EnemyStateType Type => EnemyStateType.Attack;

        /// <summary>
        /// 构造函数：注入敌人AI上下文
        /// </summary>
        /// <param name="context">敌人AI的运行时上下文（包含感知系统、战斗组件等）</param>
        /// <remarks>
        /// 通过基类构造函数初始化公共字段，确保上下文数据在基类中可用
        /// </remarks>
        public AttackState(EnemyBrain context) : base(context) { }

        /// <summary>
        /// 状态进入时执行的方法（重写自基类）
        /// 触发攻击启动逻辑
        /// </summary>
        /// <remarks>
        /// 典型执行流程：
        /// 1. 播放攻击准备动画
        /// 2. 锁定攻击目标
        /// 3. 启动攻击冷却计时
        /// 4. 调用战斗系统的攻击逻辑
        /// </remarks>
        protected override void Enter()
        {
            // 通过上下文获取战斗组件，启动攻击序列
            Context.Combat.StartAttackSequence();
            
            // 可以在此处添加：
            // - 攻击动画触发
            // - 特效播放
            // - 音效播放
            // - AI行为记录（用于调试）
        }

        /// <summary>
        /// 每帧更新逻辑（重写自基类）
        /// 主要职责：
        /// 1. 维持攻击行为
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
            // 核心条件检测：敌人是否脱离攻击范围
            if (!Context.Combat.IsInAttackRange)
            {
                // 通过上下文访问状态机，触发向追击状态的转换
                Context.StateMachine.TransitionTo(EnemyStateType.Chase);
                
                // 注意：实际项目中可能需要添加：
                // 1. 平滑过渡处理（如收招动画）
                // 2. 攻击中断回调
                // 3. 目标丢失处理
            }
            
            // 典型可扩展功能：
            // - 攻击冷却计时
            // - 连击次数判断
            // - 攻击命中检测
            // - 特殊状态抵抗（如被击晕时强制退出攻击）
        }
    }
}