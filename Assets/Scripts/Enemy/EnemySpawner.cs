using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public int maxEnemies; // 最大敌人数
    public Vector3 spawnArea;
    public List<GameObject> enemies;

    void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            //Instantiate(enemies[Random.Range(0, enemies.Count)], GetRandomPosition(), Quaternion.identity);
            GameObject commonEnemy= MyPooler.ObjectPooler.Instance.GetFromPool("CommonEnemy", GetRandomPosition(),Quaternion.identity);
            commonEnemy.name = "CommonEnemy"+i.ToString();
            if (commonEnemy.GetComponent<NavMeshAgent>() == null)
                commonEnemy.AddComponent<NavMeshAgent>();
            GameObject shieldEnemy= MyPooler.ObjectPooler.Instance.GetFromPool("ShieldEnemy",GetRandomPosition() ,Quaternion.identity);
            shieldEnemy.name = "ShieldEnemy"+i.ToString();
            if (shieldEnemy.GetComponent<NavMeshAgent>() == null)
                shieldEnemy.AddComponent<NavMeshAgent>();
            GameObject slowEnemy=MyPooler.ObjectPooler.Instance.GetFromPool("SlowEnemy",GetRandomPosition() ,Quaternion.identity);
            slowEnemy.name = "SlowEnemy"+i.ToString();
            if (slowEnemy.GetComponent<NavMeshAgent>() == null)
                slowEnemy.AddComponent<NavMeshAgent>();
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