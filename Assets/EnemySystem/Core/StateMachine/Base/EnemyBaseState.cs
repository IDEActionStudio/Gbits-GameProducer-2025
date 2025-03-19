using EnemySystem.Core.AI;
using EnemySystem.Core.Interfaces;
using EnemySystem.Core.StateMachine.Interfaces;
using EnemySystem.Data.Enums;

namespace EnemySystem.Core.StateMachine.Base
{
    /// <summary>
    /// 敌人AI状态基类（双接口体系实现）
    /// 设计目的：
    /// 1. 提供状态通用模板，规范状态生命周期
    /// 2. 封装上下文访问，实现接口隔离
    /// 3. 统一状态转换检查机制
    /// 核心特性：
    /// - 显式接口实现：对外满足IEnemyState契约
    /// - 模板方法模式：定义状态生命周期框架
    /// - 上下文封装：通过依赖注入保障数据安全
    /// </summary>
    public abstract class EnemyBaseState : IEnemyState
    {
        /// <summary>
        /// 受保护的上下文访问器（只读）
        /// 子类通过该属性访问共享的AI上下文数据
        /// </summary>
        protected EnemyBrain Context { get; private set; }

        /// <summary>
        /// 抽象状态类型标识符
        /// 强制要求每个具体状态必须声明自己的类型
        /// </summary>
        public abstract EnemyStateType Type { get; }

        /// <summary>
        /// 基类构造函数：依赖注入上下文
        /// </summary>
        /// <param name="context">敌人AI的运行时上下文</param>
        /// <remarks>
        /// 上下文包含：
        /// - 移动系统
        /// - 感知系统
        /// - 战斗系统
        /// - 动画控制器
        /// - 配置参数
        /// </remarks>
        protected EnemyBaseState(EnemyBrain context)
        {
            Context = context;
        }

        public EnemyStateType HandleEvent(EnemyEventType eventType, EnemyBrain context)
        {
            throw new System.NotImplementedException();
        }


        #region 显式接口实现（面向状态机）
        /// <summary>
        /// 显式接口实现：状态进入逻辑
        /// </summary>
        /// <param name="context">通过接口传入的上下文</param>
        /// <remarks>
        /// 设计要点：
        /// 1. 验证上下文一致性（当前实现隐式保证）
        /// 2. 转接到受保护的无参数方法
        /// 3. 避免子类直接处理上下文参数
        /// </remarks>
        void IEnemyState.Enter(EnemyBrain context) => Enter();

        /// <summary>
        /// 显式接口实现：状态更新逻辑
        /// </summary>
        void IEnemyState.Update(EnemyBrain context) => Update();

        /// <summary>
        /// 显式接口实现：状态退出逻辑
        /// </summary>
        void IEnemyState.Exit(EnemyBrain context) => Exit();

        /// <summary>
        /// 显式接口实现：状态转换检查
        /// </summary>
        /// <returns>建议转换的目标状态类型</returns>
        EnemyStateType IEnemyState.CheckTransition(EnemyBrain context) => CheckTransition();
        #endregion

        #region 子类可重写的模板方法（面向具体状态）
        /// <summary>
        /// 状态进入时的扩展点（无参数版本）
        /// </summary>
        /// <example>
        /// 典型使用场景：
        /// - 初始化状态参数
        /// - 播放入场动画
        /// - 重置计时器
        /// </example>
        protected virtual void Enter() { }

        /// <summary>
        /// 每帧更新逻辑的扩展点
        /// </summary>
        /// <remarks>
        /// 调用频率：与MonoBehaviour.Update()同步
        /// 注意：避免在此处执行高开销操作
        /// </remarks>
        protected virtual void Update() { }

        /// <summary>
        /// 状态退出时的扩展点
        /// </summary>
        /// <example>
        /// 典型使用场景：
        /// - 清理临时数据
        /// - 停止正在进行的协程
        /// - 取消状态特效
        /// </example>
        protected virtual void Exit() { }

        /// <summary>
        /// 状态转换检查的扩展点
        /// </summary>
        /// <returns>
        /// 默认返回当前状态类型，表示不主动切换
        /// 子类可重写为具体的转换逻辑
        /// </returns>
        /// <example>
        /// 重写示例：
        /// protected override EnemyStateType CheckTransition()
        /// {
        ///     if(Context.Health.IsDead) 
        ///         return EnemyStateType.Death;
        ///     return base.CheckTransition();
        /// }
        /// </example>
        protected virtual EnemyStateType CheckTransition() => Type;
        #endregion

        /* 设计模式解析：
        1. 模板方法模式：
           - 定义状态生命周期的算法骨架（Enter→Update→CheckTransition→Exit）
           - 将具体实现延迟到子类

        2. 双接口体系：
           - 对外：严格实现IEnemyState接口
           - 对内：提供简化版的无参数方法供子类继承

        3. 里氏替换原则：
           - 所有子类均可替代基类被状态机使用
           - 通过抽象属性强制类型声明保障类型安全

        安全机制：
        - 上下文通过构造函数一次性注入，保障生命周期内一致性
        - 显式接口实现阻止直接调用接口方法
        - 受保护的虚方法限制外部访问
        */
    }
}