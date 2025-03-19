using EnemySystem.Core.AI;
using EnemySystem.Core.StateMachine.Base;
using EnemySystem.Data.Enums;

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

        protected override void Enter()
        {
            Context.Movement.StartPatrol();
        }

        protected override void Update()
        {
            if (Context.Perception.HasDetectedTarget)
            {
                Context.StateMachine.TransitionTo(EnemyStateType.Chase);
            }
        }
    }
}