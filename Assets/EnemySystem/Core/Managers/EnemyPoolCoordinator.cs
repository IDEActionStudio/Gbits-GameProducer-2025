namespace EnemySystem.Core.Managers
{
    using UnityEngine;
using System.Collections.Generic;

public class EnemyPoolCoordinator : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string key;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private List<Pool> pools = new List<Pool>();
    
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<GameObject, string> instanceToKeyMap;
    private Transform objectPoolParent;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        instanceToKeyMap = new Dictionary<GameObject, string>();
        
        // 创建父对象用于组织对象池对象
        objectPoolParent = new GameObject("ObjectPool").transform;
        objectPoolParent.SetParent(transform);

        foreach (var pool in pools)
        {
            // 为每个池类型创建父对象
            var poolParent = new GameObject(pool.key + "Pool").transform;
            poolParent.SetParent(objectPoolParent);
            
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, poolParent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                instanceToKeyMap.Add(obj, pool.key);
            }
            
            poolDictionary.Add(pool.key, objectPool);
        }
    }

    public T Get<T>(string key) where T : Component
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogError($"Pool with key {key} doesn't exist");
            return null;
        }

        // 获取或创建新实例
        GameObject obj = GetOrCreateInstance(key);
        obj.SetActive(true);
        return obj.GetComponent<T>();
    }

    private GameObject GetOrCreateInstance(string key)
    {
        Queue<GameObject> poolQueue = poolDictionary[key];
        
        // 当池中有可用对象时
        if (poolQueue.Count > 0)
        {
            return poolQueue.Dequeue();
        }

        // 池空时动态扩容
        Pool poolConfig = pools.Find(p => p.key == key);
        if (poolConfig == null)
        {
            Debug.LogError($"No config found for key: {key}");
            return null;
        }

        GameObject newObj = Instantiate(poolConfig.prefab, 
            objectPoolParent.Find(key + "Pool"));
        instanceToKeyMap.Add(newObj, key);
        return newObj;
    }

    public void Return(GameObject objToReturn)
    {
        if (!instanceToKeyMap.TryGetValue(objToReturn, out string key))
        {
            Debug.LogWarning($"Trying to return unmanaged object: {objToReturn.name}");
            Destroy(objToReturn);
            return;
        }

        objToReturn.SetActive(false);
        poolDictionary[key].Enqueue(objToReturn);
        
        // 重置对象状态
        objToReturn.transform.localPosition = Vector3.zero;
        objToReturn.transform.localRotation = Quaternion.identity;
    }
}
}