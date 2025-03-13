using EnemySystem.Core.AI;
using EnemySystem.Data.Configs.Enums;

namespace EnemySystem.Core.Interfaces
{
    /// <summary>
    /// 敌人状态接口
    /// </summary>
    public interface IEnemyState
    {
        EnemyStateType Type { get; }
        void Enter(EnemyBrain context);
        void Update(EnemyBrain context);
        void Exit(EnemyBrain context);
        EnemyStateType CheckTransition(EnemyBrain context);
    }
}