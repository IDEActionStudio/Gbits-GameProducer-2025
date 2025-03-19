using EnemySystem.Core.AI;
using EnemySystem.Core.StateMachine.Factories;
using EnemySystem.Core.StateMachine.Interfaces;
using EnemySystem.Data.Enums;

namespace EnemySystem.Core.StateMachine
{
    /// <summary>
    /// 敌人AI状态机控制器
    /// 核心职责：管理状态堆栈，处理状态间的转换逻辑，驱动状态行为更新
    /// 设计模式：基于有限状态机（FSM）模式实现
    /// </summary>
    public class EnemyStateMachine
    {
        // 添加上下文引用
        private readonly EnemyBrain _context;
        
        // 当前运行的状态对象
        private IEnemyState _currentState;
        
        // 状态工厂：负责具体状态对象的创建与管理
        private readonly EnemyStateFactory _factory;

        // 通过属性暴露上下文（可选）
        private EnemyBrain Context => _context;
        
        public EnemyStateType CurrentState => _currentState.Type;
        
        /// <summary>
        /// 构造函数：初始化状态机
        /// </summary>
        /// <param name="context">敌人AI的上下文环境（EnemyBrain），包含所有状态需要的共享数据</param>
        public EnemyStateMachine(EnemyBrain context)
        {
            _context = context; // 存储上下文引用
            _factory = new EnemyStateFactory(_context); // 将上下文传递给工厂
        
            // 初始化状态时使用存储的上下文
            TransitionTo(EnemyStateType.Patrol);
        }

        
        
        /// <summary>
        /// 执行状态转换的核心方法
        /// </summary>
        /// <param name="type">目标状态类型</param>
        /// <remarks>
        /// 1. 退出当前状态
        /// 2. 从工厂获取新状态实例
        /// 3. 进入新状态
        /// </remarks>
        public void TransitionTo(EnemyStateType type)
        {
            _currentState?.Exit(_context); // 使用存储的上下文
        
            _currentState = _factory.GetState(type);
            _currentState.Enter(_context); // 传递存储的上下文
        }

        /// <summary>
        /// 每帧更新驱动方法
        /// </summary>
        /// <remarks>
        /// 1. 更新当前状态逻辑
        /// 2. 检查状态转换条件
        /// 3. 执行必要的状态转换
        /// </remarks>
        public void Update()
        {
            _currentState.Update(_context);
            var nextState = _currentState.CheckTransition(_context);
        
            if (nextState != _currentState.Type)
                TransitionTo(nextState);
        }

        // 新增的事件处理方法（根据之前需求）
        public void HandleEvent(EnemyEventType eventType)
        {
            var targetState = _currentState.HandleEvent(eventType, _context);
            if (targetState != _currentState.Type)
                TransitionTo(targetState);
        }



        // 注意：原代码中缺少Context属性的定义，此处应有上下文对象的访问方式
        // 可能的实现方式（需根据实际项目补充）：
        // private EnemyBrain Context => _factory.Context;
    }
}