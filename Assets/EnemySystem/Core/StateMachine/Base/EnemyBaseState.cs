using EnemySystem.Core.AI;
using EnemySystem.Core.Interfaces;
using EnemySystem.Data.Configs.Enums;

namespace EnemySystem.Core.StateMachine.BaseStates
{
    /// <summary>
    /// 状态基类（实现双接口体系）
    /// </summary>
    public abstract class EnemyBaseState : IEnemyState
    {
        protected EnemyBrain Context { get; private set; }

        // 必须实现的抽象属性
        public abstract EnemyStateType Type { get; }

        protected EnemyBaseState(EnemyBrain context)
        {
            Context = context;
        }

        // 接口显式实现（保持接口契约）
        void IEnemyState.Enter(EnemyBrain context) => Enter();
        void IEnemyState.Update(EnemyBrain context) => Update();
        void IEnemyState.Exit(EnemyBrain context) => Exit();
        EnemyStateType IEnemyState.CheckTransition(EnemyBrain context) => CheckTransition();

        // 实际供子类重写的方法（无参数版本）
        protected virtual void Enter() { }
        protected virtual void Update() { }
        protected virtual void Exit() { }
        protected virtual EnemyStateType CheckTransition() => Type;
    }
}