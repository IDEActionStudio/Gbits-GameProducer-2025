using UnityEngine;

namespace EnemySystem.Data.Configs
{
    /// <summary>
    /// BOSS特殊配置数据
    /// </summary>
    [CreateAssetMenu(menuName = "EnemySystem/Configs/BossConfig")]
    public class BossConfig : EnemyConfig
    {
        [Header("特殊技能")] [Tooltip("召唤小兵间隔")] public float summonInterval = 10f;
        [Tooltip("范围攻击半径")] public float aoeRadius = 5f;
        [Tooltip("狂暴模式阈值")] [Range(0, 1)] public float enrageThreshold = 0.3f;
    }
}