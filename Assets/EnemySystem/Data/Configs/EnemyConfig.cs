using EnemySystem.Data.Enums;
using UnityEngine;

namespace EnemySystem.Data.Configs
{
    [CreateAssetMenu(menuName = "EnemySystem/Configs/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("基础属性")]
        [Tooltip("敌人类型标识")] 
        public EnemyType type;
        [Tooltip("初始生命值"), Range(1, 1000)] 
        public int maxHealth = 100;
        [Tooltip("移动速度（米/秒）"), Min(0.1f)] 
        public float moveSpeed = 3.5f;
        [Tooltip("转向速度（度/秒）"), Min(1)] 
        public float rotationSpeed = 120f;
        [Tooltip("导航代理加速度")] 
        public float accelerationSpeed = 1.5f;

        
        
        [Header("战斗设置")]
        [Tooltip("近战攻击距离"), Min(0.5f)] 
        public AttackType attackType = AttackType.Melee;
        [Tooltip("近战攻击距离"), Min(0.5f)] 
        public float attackRange = 2f;
        [Tooltip("攻击动作间隔（秒）"), Min(0.1f)] 
        public float attackInterval = 1.5f;
        [Tooltip("单次攻击伤害值"), Min(1)] 
        public int attackDamage = 15;
        [Tooltip("弹道速度（远程类型使用）"), Min(1)] 
        public float projectileSpeed = 10f;

        
        
        [Header("AI行为")] 
        [Tooltip("玩家检测半径"), Min(1f)] 
        public float detectionRadius = 8f;
        [Tooltip("巡逻移动速度（米/秒）"), Min(0.1f)] 
        public float patrolSpeed = 2.5f;
        [Tooltip("追击移动速度（米/秒）"), Min(0.1f)] 
        public float chaseSpeed = 5f;
        [Tooltip("巡逻停止距离阈值"), Range(0.1f, 2f)] 
        public float patrolStoppingDistance = 0.5f;
        [Tooltip("巡逻区域半径"), Min(1f)] 
        public float patrolRadius = 5f;
        [Tooltip("攻击目标层级")] 
        public LayerMask targetLayer;

        // 基础数据验证
        private void OnValidate()
        {
            // 确保远程类型没有近战参数
            if (type == EnemyType.Ranged && attackRange < 3f)
            {
                attackRange = 3f;
                Debug.LogWarning($"远程敌人攻击距离自动调整为最小值3米");
            }
        }
        
    }
}