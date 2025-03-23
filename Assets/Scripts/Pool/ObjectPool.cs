using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject[] prefab; // 敌人预制体
    public int initialSize = 10; // 初始池大小

    private Queue<GameObject> pool = new Queue<GameObject>();

    private static ObjectPool instance;

    public static ObjectPool GetInstance()
    {
        if(instance == null)
            instance = new ObjectPool();
        return instance;
    }
    void Start()
    {
        InitializePool();
    }

    // 初始化对象池
    public void InitializePool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab[0]);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // 从池中获取一个对象
    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // 如果池中没有可用对象，创建一个新的
            GameObject obj = Instantiate(prefab[0]);
            return obj;
        }
    }

    // 回收对象到池中
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}