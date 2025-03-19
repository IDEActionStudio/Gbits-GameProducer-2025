using UnityEngine;
using System.Collections.Generic;
using EnemySystem.Core.AI;
using EnemySystem.Data.Configs;
using EnemySystem.Data.Enums;

namespace EnemySystem.Core.Managers
{
    /// <summary>
    /// 全局敌人管理系统（单例模式）
    /// 负责敌人生成、回收和全局状态管理
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }

        [Header("对象池配置")]
        [SerializeField] private EnemyPoolCoordinator enemyPooler;

        [Header("敌人配置表")] 
        [SerializeField] private EnemyConfigTable configTable;

        private Dictionary<EnemyType, List<EnemyBrain>> activeEnemies = new();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        /// <summary>
        /// 生成指定类型的敌人
        /// </summary>
        /// <param name="type">敌人类型</param>
        /// <param name="spawnPoint">生成位置</param>
        public EnemyBrain SpawnEnemy(EnemyType type, Vector3 spawnPoint)
        {
            var enemy = enemyPooler.Get<EnemyBrain>(type.ToString());
            enemy.Initialize(configTable.GetConfig(type), spawnPoint);
            
            if (!activeEnemies.ContainsKey(type))
                activeEnemies.Add(type, new List<EnemyBrain>());
            
            activeEnemies[type].Add(enemy);
            return enemy;
        }

        /// <summary>
        /// 回收指定敌人实例
        /// </summary>
        public void RecycleEnemy(EnemyBrain enemy)
        {
            if (activeEnemies.TryGetValue(enemy.Config.type, out var list))
            {
                list.Remove(enemy);
                enemyPooler.Return(enemy.gameObject);
            }
        }
    }
}
