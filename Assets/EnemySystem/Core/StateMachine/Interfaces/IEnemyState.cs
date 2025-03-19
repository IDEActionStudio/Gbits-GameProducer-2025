using EnemySystem.Core.AI;
using EnemySystem.Data.Enums;

namespace EnemySystem.Core.StateMachine.Interfaces
{
    /// <summary>
    /// 敌人状态接口
    /// </summary>
    public interface IEnemyState
    {
        EnemyStateType Type { get; }
    
        // 新增事件处理方法
        EnemyStateType HandleEvent(EnemyEventType eventType, EnemyBrain context);
        
        void Enter(EnemyBrain context);
        void Update(EnemyBrain context);
        void Exit(EnemyBrain context);
        EnemyStateType CheckTransition(EnemyBrain context);
    }
}