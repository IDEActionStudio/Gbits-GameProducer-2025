using System.Collections.Generic;
using EnemySystem.Core.AI;
using EnemySystem.Core.StateMachine.Interfaces;
using EnemySystem.Core.StateMachine.States;
using EnemySystem.Data.Enums;

namespace EnemySystem.Core.StateMachine.Factories
{
    
    /// <summary>
    /// 状态工厂类
    /// 集中管理状态对象创建
    /// </summary>
    public class EnemyStateFactory
    {
        private Dictionary<EnemyStateType, IEnemyState> states = new();

        public EnemyStateFactory(EnemyBrain context)
        {
            RegisterState(new PatrolState(context));
            RegisterState(new ChaseState(context));
            RegisterState(new AttackState(context));
        }

        public IEnemyState GetState(EnemyStateType type) => states[type];

        private void RegisterState(IEnemyState state)
        {
            states[state.Type] = state;
        }
    }
}