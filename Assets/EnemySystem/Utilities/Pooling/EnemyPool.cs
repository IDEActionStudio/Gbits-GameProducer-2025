using UnityEngine;
using EnemySystem.Core.AI;
using EnemySystem.Data.Enums;
using EnemySystem.Modules.Combat;

namespace EnemySystem.Utilities.Pooling
{
    public class EnemyPool : BasePool<EnemyPool.EnemyPoolItem>
    {
        [System.Serializable]
        public class EnemyPoolItem : MonoBehaviour
        {
            public EnemyType enemyType;
            public EnemyBrain brain; // 核心组件引用
            public HealthSystem health;
            public Rigidbody rb;
        }

        [Header("Enemy Settings")]
        [SerializeField] private Transform activeContainer;
        [SerializeField] private GameObject spawnVFX;

        public void Initialize(int initialSize)
        {
            WarmPool(initialSize);
        }

        // 修改返回类型为具体组件
        public new EnemyPoolItem Get(Vector3 position)
        {
            var instance = base.Get();
            instance.transform.position = position;
            return instance;
        }

        // 添加类型安全的回收方法
        public void Return(EnemyPoolItem enemy)
        {
            base.Return(enemy);
        }

        protected override void OnGet(EnemyPoolItem instance)
        {
            instance.health.Reset();
            // instance.brain.ResetState();
            instance.transform.SetParent(activeContainer);
            
            if(spawnVFX) 
                Instantiate(spawnVFX, instance.transform.position, Quaternion.identity);
        }

        protected override void OnReturn(EnemyPoolItem instance)
        {
            instance.brain.StopAllCoroutines();
            instance.transform.SetParent(PoolRoot);
        }
    }
}