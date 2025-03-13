using EnemySystem.Data.Configs.Enums;
using EnemySystem.Modules.Combat;
using UnityEngine;

namespace EnemySystem.Data.Configs
{
    /// <summary>
    /// 基础敌人配置数据（ScriptableObject）
    /// </summary>
    [CreateAssetMenu(menuName = "EnemySystem/Configs/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("基础属性")]
        [Tooltip("敌人类型")] public EnemyType type; // 添加类型字段
        [Tooltip("最大血量")] public int maxHealth = 100;
        [Tooltip("移动速度")] public float moveSpeed = 3.5f;
        [Tooltip("旋转速度")] public float rotationSpeed = 120f;

        [Header("战斗设置")]
        [Tooltip("攻击距离")] public float attackRange = 2f;
        [Tooltip("攻击间隔")] public float attackInterval = 1.5f;
        [Tooltip("攻击伤害")] public int attackDamage = 15;
        [Tooltip("攻击伤害")] public float projectileSpeed = 10.0f;

        [Header("攻击类型")]
        public AttackType attackType = AttackType.Melee;
        
        [Header("连击设置")] 
        [Tooltip("最大连击段数")] public int maxComboSteps = 3;

        [Header("目标设置")]
        [Tooltip("攻击目标层级")] public LayerMask targetLayer;
        
        [Header("AI行为")]
        [Tooltip("检测半径")] public float detectionRadius = 8f;
        [Tooltip("巡逻半径")] public float patrolRadius = 5f;
    }
}