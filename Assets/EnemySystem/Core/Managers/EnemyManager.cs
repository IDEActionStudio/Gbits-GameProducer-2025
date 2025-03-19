using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnemySystem.Core.AI;
using EnemySystem.Data.Configs;
using EnemySystem.Data.Enums;

namespace EnemySystem.Core.Managers
{
    /// <summary>
    /// 全局敌人管理系统（支持初始生成波次）
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }

        // 修改后（添加必需字段验证）
        [Header("Pool Settings")]
        [SerializeField, Tooltip("必须拖拽场景中的EnemyPoolCoordinator对象")] 
        private EnemyPoolCoordinator _pooler;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                ValidateReferences(); // 新增引用验证
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        [SerializeField] private EnemyConfigTable _configTable;

        [Header("Initial Spawn")]
        [SerializeField] private InitialSpawnWave _initialWave;
        [SerializeField] private float _spawnDelay = 1f;

        private Dictionary<EnemyType, List<EnemyBrain>> _activeEnemies = new();
        private Coroutine _spawnRoutine;


        /// <summary>
        /// 初始化管理系统
        /// </summary>
        private void Initialize()
        {
            if (_initialWave != null && _initialWave.enabled)
            {
                _spawnRoutine = StartCoroutine(SpawnInitialWave());
            }
        }

        // 新增引用验证方法
        private void ValidateReferences()
        {
            if (_pooler == null)
            {
                Debug.LogError("未配置EnemyPoolCoordinator引用!", this);
                enabled = false;
            }

            if (_configTable == null)
            {
                Debug.LogError("未配置EnemyConfigTable!", this);
                enabled = false;
            }
        }
        /// <summary>
        /// 生成初始敌人波次（协程分帧生成）
        /// </summary>
        private IEnumerator SpawnInitialWave()
        {
            yield return new WaitForSeconds(_spawnDelay);

            foreach (var spawnInfo in _initialWave.spawnInfos)
            {
                for (int i = 0; i < spawnInfo.amount; i++)
                {
                    SpawnAtRandomPosition(spawnInfo.type);
                    yield return new WaitForEndOfFrame(); // 分帧生成避免卡顿
                }
            }
        }

        /// <summary>
        /// 在随机位置生成敌人
        /// </summary>
        private void SpawnAtRandomPosition(EnemyType type)
        {
            var spawnPoint = CalculateSpawnPosition();
            SpawnEnemy(type, spawnPoint);
        }

        /// <summary>
        /// 计算安全生成位置（示例逻辑）
        /// </summary>
        private Vector3 CalculateSpawnPosition()
        {
            // 实际项目中应替换为你的生成点逻辑
            return transform.position + Random.insideUnitSphere * 5f;
        }

        
        public EnemyBrain SpawnEnemy(EnemyType type, Vector3 spawnPoint)
        {
            // 获取敌人实例
            var enemy = _pooler.GetEnemy<EnemyBrain>(type, spawnPoint);
    
            // 初始化配置
            var config = _configTable.GetConfig(type);
            if (config != null)
            {
                enemy.Initialize(config, spawnPoint);
                RegisterEnemy(type, enemy);
            }
            else
            {
                Debug.LogError($"找不到 {type} 的配置数据");
                _pooler.ReturnEnemy(enemy.gameObject);
            }
    
            return enemy;
        }

        /// <summary>
        /// 注册活跃敌人
        /// </summary>
        private void RegisterEnemy(EnemyType type, EnemyBrain enemy)
        {
            if (!_activeEnemies.ContainsKey(type))
            {
                _activeEnemies.Add(type, new List<EnemyBrain>());
            }
            _activeEnemies[type].Add(enemy);
        }

        /// <summary>
        /// 回收指定敌人实例
        /// </summary>
        public void RecycleEnemy(EnemyBrain enemy)
        {
            if (_activeEnemies.TryGetValue(enemy.Config.type, out var list))
            {
                list.Remove(enemy);
                // 修改回收方式
                _pooler.ReturnEnemy(enemy.gameObject);
            }
        }

        /// <summary>
        /// 清除所有活跃敌人
        /// </summary>
        public void ClearAllEnemies()
        {
            foreach (var list in _activeEnemies.Values)
            {
                foreach (var enemy in list)
                {
                    _pooler.ReturnEnemy(enemy.gameObject);
                }
                list.Clear();
            }
        }

        /// <summary>
        /// 获取当前活跃敌人列表
        /// </summary>
        public List<EnemyBrain> GetActiveEnemies(EnemyType type)
        {
            return _activeEnemies.ContainsKey(type) ? _activeEnemies[type] : new List<EnemyBrain>();
        }

        #region Editor
#if UNITY_EDITOR
        [System.Serializable]
        public class InitialSpawnWave
        {
            public bool enabled = true;
            public List<SpawnInfo> spawnInfos = new();

            [System.Serializable]
            public class SpawnInfo
            {
                public EnemyType type;
                [Range(1, 20)] public int amount = 3;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_initialWave == null || !_initialWave.enabled) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 5f);
        }
#endif
        #endregion
    }
}