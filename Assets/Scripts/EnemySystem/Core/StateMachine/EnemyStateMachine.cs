using System.Collections.Generic;
using EnemySystem.Core.AI;
using EnemySystem.Core.Interfaces;
using EnemySystem.Data.Configs.Enums;

namespace EnemySystem.Core.StateMachine
{
    /// <summary>
    /// 状态机控制器
    /// 管理状态堆栈和转换逻辑
    /// </summary>
    public class EnemyStateMachine
    {
        private IEnemyState _currentState;
        private readonly EnemyStateFactory _factory;

        public EnemyStateMachine(EnemyBrain context)
        {
            _factory = new EnemyStateFactory(context);
            TransitionTo(EnemyStateType.Patrol);
        }

        public void TransitionTo(EnemyStateType type)
        {
            _currentState?.Exit(Context);  // 保持接口调用规范
            _currentState = _factory.GetState(type);
            _currentState.Enter(Context);
        }

        public void Update()
        {
            _currentState.Update(Context);
            var nextState = _currentState.CheckTransition(Context);
            if (nextState != _currentState.Type)
                TransitionTo(nextState);
        }
    }
}