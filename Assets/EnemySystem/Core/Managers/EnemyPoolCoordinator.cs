using UnityEngine;
using System.Collections.Generic;
using EnemySystem.Data.Enums;
using EnemySystem.Utilities.Pooling;

namespace EnemySystem.Core.Managers
{
    public class EnemyPoolCoordinator : MonoBehaviour
    {
        [System.Serializable]
        public class PoolConfig
        {
            public EnemyType type;
            [Tooltip("必须引用场景中的EnemyPool预制体")] 
            public EnemyPool poolPrefab;
            public int initialSize = 10;
        }

        [Header("池配置")]
        public List<PoolConfig> poolConfigs = new List<PoolConfig>();

        private Dictionary<EnemyType, EnemyPool> _pools = new();

        private void Start()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (var config in poolConfigs)
            {
                // 创建子池管理器
                var pool = Instantiate(config.poolPrefab, transform);
                pool.name = $"{config.type}_Pool";
                pool.Initialize(config.initialSize);
                _pools.Add(config.type, pool);
            }
        }

        public T GetEnemy<T>(EnemyType type, Vector3 position) where T : Component
        {
            if (_pools.TryGetValue(type, out var pool))
            {
                return pool.Get(position).GetComponent<T>();
            }
            Debug.LogError($"未配置 {type} 类型的对象池");
            return null;
        }

        public void ReturnEnemy(GameObject enemy)
        {
            if (enemy.TryGetComponent<EnemyPool.EnemyPoolItem>(out var item))
            {
                if (_pools.TryGetValue(item.enemyType, out var pool))
                {
                    pool.Return(item);
                }
            }
            else
            {
                Destroy(enemy);
            }
        }
    }
}