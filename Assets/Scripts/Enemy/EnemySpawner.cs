using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public int maxEnemies; // 最大敌人数
    public Vector3 spawnArea;
    public List<GameObject> enemies;

    void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            Instantiate(enemies[Random.Range(0, enemies.Count)], GetRandomPosition(), Quaternion.identity);
            //GameObject commonEnemy= MyPooler.ObjectPooler.Instance.GetFromPool("CommonEnemy",GetRandomPosition() ,Quaternion.identity);
            //GameObject shieldEnemy= MyPooler.ObjectPooler.Instance.GetFromPool("ShieldEnemy",GetRandomPosition() ,Quaternion.identity);
        }
    }
    Vector3 GetRandomPosition()
    {
            Vector3 randomPosition = new Vector3(Random.Range(0, spawnArea.x), 0f, Random.Range(0, spawnArea.z));
            randomPosition += transform.position;
            return randomPosition;
            //Vector3 spawnPosition = new Vector3(200,0,200);
            //return spawnPosition;
    }
}