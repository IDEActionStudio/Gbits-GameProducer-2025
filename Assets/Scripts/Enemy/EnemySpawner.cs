using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public int maxEnemies; // 最大敌人数
    private float maxMapSize=380f;

    void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // 从对象池中获取敌人
        // 设置敌人的对象池引用
        GameObject enemy= MyPooler.ObjectPooler.Instance.GetFromPool("CommonEnemy",GetRandomPosition(),Quaternion.identity);
        //enemy.transform.position = GetRandomPosition();
        enemy.SetActive(true);
    }
    Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(0,maxMapSize), 0, Random.Range(0,maxMapSize));
        return randomPosition;
    }
    
}