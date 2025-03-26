using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MyPooler
{
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int amount;
            public bool shouldExpandPool = true;
            public int extensionLimit;
        }

        private static ObjectPooler _instance;
        public static ObjectPooler Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("ObjectPooler");
                    go.AddComponent<ObjectPooler>();
                }
                return _instance;
            }
        }

        void Awake()
        {
            _instance = this;
            if (!shouldDestroyOnLoad)
                DontDestroyOnLoad(this);

            CreatePools();
        }

        public bool isDebug = false;
        public bool shouldDestroyOnLoad = false;
        private int extensionSize;
        public List<Pool> pools;
        public Dictionary<string, Transform> parents;
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        public Dictionary<string, List<GameObject>> activeObjects;
        public UnityAction onResetPools;

        /// <summary>
        /// Get an object from the pool if available
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public GameObject GetFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                if(isDebug)
                    Debug.Log("Pool tag not found!");
                return null;
            }

            GameObject o = null;
            if (poolDictionary[tag].Count > 0)
            {
                o = poolDictionary[tag].Dequeue();
            }
            else
            {
                Pool currentPool = null;
                float extensionLimit = 0f;
                bool shouldExpandPool = false;
                foreach (Pool p in pools)
                {
                    if (p.tag == tag)
                    {
                        currentPool = p;
                        extensionLimit = p.extensionLimit;
                        shouldExpandPool = p.shouldExpandPool;
                        break;
                    }
                }
                if (shouldExpandPool)
                {
                    if (extensionLimit > 0)
                    {
                        if (extensionSize < extensionLimit)
                        {
                            o = IncrementPool(currentPool);
                            if (isDebug)
                                Debug.Log(tag + " pool incremented!");
                            extensionSize++;
                        }
                        else
                        {
                            if (isDebug)
                                Debug.Log("You have no room left for extension on your pool: " + tag + ".");
                            return null;
                        }
                    }
                    else
                    {
                        o = IncrementPool(currentPool);
                        if (isDebug)
                            Debug.Log(tag + " pool incremented!");
                        extensionSize++;
                    }
                }
                else
                {
                    if (isDebug)
                        Debug.Log("You have no object left on your pool: " + tag + ".");
                    return null;
                }
            }
            o.SetActive(true);
            o.transform.position = position;
            o.transform.rotation = rotation;
            IPooledObject pooledObj = o.GetComponent<IPooledObject>();

            if (pooledObj != null)
            {
                pooledObj.OnRequestedFromPool();
                onResetPools += pooledObj.DiscardToPool;
            }

            activeObjects[tag].Add(o);
            return o;
        }

        /// <summary>
        /// Return an object to a pool
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="o"></param>
        public void ReturnToPool(string tag, GameObject o)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                if (isDebug)
                    Debug.Log("Pool tag not found!");
                return;
            }
            activeObjects[tag].Remove(o);
            poolDictionary[tag].Enqueue(o);
            o.SetActive(false);
            if(onResetPools != null) onResetPools -= o.GetComponent<IPooledObject>().DiscardToPool;
        }

        /// <summary>
        /// Reset a entire pool 
        /// </summary>
        /// <param name="tag"></param>
        public void ResetPool(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                if (isDebug)
                    Debug.Log("Couldn't reset the pool '" + tag + "': Pool tag not found!");
                return;
            }

            List<GameObject> currentList = new List<GameObject>(activeObjects[tag]);
            foreach (GameObject go in currentList)
            {
                go.GetComponent<IPooledObject>().DiscardToPool();
            }
            currentList.Clear();
        }

        /// <summary>
        /// Reset all pools
        /// </summary>
        public void ResetAllPools()
        {
            onResetPools?.Invoke();
        }

        void CreatePools()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            parents = new Dictionary<string, Transform>();
            activeObjects = new Dictionary<string, List<GameObject>>();

            foreach (Pool pool in pools)
            {
                GameObject poolObject = new GameObject(pool.tag + "_Pool");
                poolObject.transform.SetParent(this.transform);
                parents.Add(pool.tag, poolObject.transform);
                activeObjects.Add(pool.tag, new List<GameObject>());
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.amount; i++)
                {
                    GameObject o = Instantiate(pool.prefab);
                    o.SetActive(false);
                    objectPool.Enqueue(o);
                    o.transform.SetParent(poolObject.transform);
                }
                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        GameObject IncrementPool(Pool p)
        {
            GameObject objectToIncrement = p.prefab;
            GameObject obj = Instantiate(objectToIncrement);
            obj.transform.SetParent(parents[p.tag]);
            activeObjects[p.tag].Add(obj);
            return obj;
        }
    }
}
