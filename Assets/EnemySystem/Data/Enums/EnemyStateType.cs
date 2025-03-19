namespace EnemySystem.Data.Enums
{
    /// <summary>
    /// 敌人状态类型枚举
    /// </summary>
    public enum EnemyStateType
    {
        Patrol,  // 巡逻状态
        Chase,   // 追击状态
        Attack,  // 攻击状态
        Flee,    // 逃跑状态
        Dead     // 死亡状态
    }
}