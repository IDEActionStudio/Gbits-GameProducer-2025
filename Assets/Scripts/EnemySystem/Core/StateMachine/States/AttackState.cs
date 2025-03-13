using EnemySystem.Core.AI;
using EnemySystem.Core.StateMachine.BaseStates;
using EnemySystem.Data.Configs.Enums;

namespace EnemySystem.Core.StateMachine.States
{
    /// <summary>
    /// 攻击状态实现
    /// </summary>
    public class AttackState : EnemyBaseState
    {
        public override EnemyStateType Type => EnemyStateType.Attack;

        public AttackState(EnemyBrain context) : base(context) { }

        protected override void Enter()
        {
            Context.Combat.StartAttackSequence();
        }

        protected override void Update()
        {
            if (!Context.Combat.IsInAttackRange)
            {
                Context.StateMachine.TransitionTo(EnemyStateType.Chase);
            }
        }
    }
}