namespace EnemySystem.Data.Enums
{
    /// <summary>
    /// 敌人AI状态机事件类型（驱动状态转换的核心触发器）
    /// </summary>
    public enum EnemyEventType 
    {
        /// <summary>
        /// 感知系统检测到有效目标
        /// 触发状态：巡逻/闲置 -> 追击
        /// </summary>
        TargetDetected,

        /// <summary>
        /// 丢失目标持续超时
        /// 触发状态：追击 -> 返回巡逻
        /// </summary>
        TargetLost,

        /// <summary>
        /// 进入有效攻击范围
        /// 触发状态：追击 -> 攻击准备
        /// </summary>
        ReadyToAttack,

        /// <summary>
        /// 完成攻击动作
        /// 触发状态：攻击 -> 追击/后撤
        /// </summary>
        AttackCompleted,

        /// <summary>
        /// 到达巡逻路径点
        /// 触发状态：生成新巡逻路径
        /// </summary>
        PatrolPointReached,

        /// <summary>
        /// 受到玩家攻击伤害
        /// 触发状态：进入防御/闪避模式
        /// </summary>
        TakeDamage,

        /// <summary>
        /// 血量耗尽
        /// 触发状态：任何状态 -> 死亡
        /// </summary>
        HealthDepleted,

        /// <summary>
        /// 导航路径计算失败
        /// 触发状态：异常处理/重置路径
        /// </summary>
        NavigationFailed
    }
}