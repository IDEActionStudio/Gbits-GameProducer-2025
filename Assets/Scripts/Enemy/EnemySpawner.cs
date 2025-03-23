using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPool enemyPool; // 敌人对象池
    public int maxEnemies = 10; // 最大敌人数

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
        GameObject enemy = enemyPool.GetObject();
        enemy.transform.position = GetRandomPosition();
        enemy.SetActive(true);

        // 设置敌人的对象池引用
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetPool(enemyPool);
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(0,380), 0, Random.Range(0,380));
        return randomPosition;
    }
}