using EnemySystem.Core;
using EnemySystem.Core.AI;
using EnemySystem.Core.StateMachine;
using EnemySystem.Core.StateMachine.BaseStates;
using EnemySystem.Data.Configs.Enums;

namespace EnemySystem.Core.StateMachine.States
{
    
    /// <summary>
    /// 追击状态
    /// 处理目标追击逻辑
    /// </summary>
    public class ChaseState : EnemyBaseState
    {
        public override EnemyStateType.EnemyStateType Type => EnemyStateType.EnemyStateType.Chase;

        public ChaseState(EnemyBrain context) : base(context) { }

        public override void Enter()
        {
            Context.Movement.SetSpeedMultiplier(1.5f);
            Context.Movement.SetDestination(Context.Perception.TargetPosition);
        }

        public override void Update()
        {
            if (Context.Combat.IsInAttackRange)
            {
                Context.StateMachine.TransitionTo(EnemyStateType.EnemyStateType.Attack);
            }
        }
    }
}