using EnemySystem.Core.AI;
using EnemySystem.Core.StateMachine.BaseStates;
using EnemySystem.Data.Configs.Enums;

namespace EnemySystem.Core.StateMachine.States
{
    
    /// <summary>
    /// 巡逻状态
    /// 处理敌人巡逻行为
    /// </summary>
    public class PatrolState : EnemyBaseState
    {
        public override EnemyStateType Type => EnemyStateType.Patrol;

        public PatrolState(EnemyBrain context) : base(context) { }

        public override void Enter()
        {
            Context.Movement.StartPatrol();
        }

        public override void Update()
        {
            if (Context.Perception.HasDetectedTarget)
            {
                Context.StateMachine.TransitionTo(EnemyStateType.Chase);
            }
        }
    }
}