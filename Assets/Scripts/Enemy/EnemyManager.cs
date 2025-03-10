using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int enemyDensity;
    public bool canSpawn;
    public List<string> enemyPrefabNames; //敌人预制体名字
    public List<GameObject> enemys; //生成的怪物列表
    private void OnEnable()
    {
        canSpawn = true;
    }
    protected void Update()
    {
        if (canSpawn)
        {
            SpawnEnemys();
        }
    }
    private void SpawnEnemys() 
    {
        for (int i = 0; i < enemyDensity; i++)
            enemys.Add(SpawnEnemy(Random.Range(0,enemyPrefabNames.Count)));
        canSpawn = false;
    }
    private GameObject SpawnEnemy(int index)
    {

        // 随机生成敌人位置的逻辑
        //Instantiate(enemyPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity);
        //int index = Random.Range(0,enemyPrefabNames.Count);
        //int index = Random.Range(0, test);
        return ObjectPool.GetInstance().GetObj(enemyPrefabNames[index], new Vector3(Random.Range(0, 180), 0,Random.Range(0, 180)), Quaternion.identity);
        //currentEnemyCount++;  // 更新当前敌人数
    }
    public void DestoryEnemy(int score)
    {
        
        if (enemys.Count == 0)
        {
            canSpawn = true;
        }
    }
    public void ClearEnemy()
    {
        //if (enemys.Count > 0)
        //{
        //    foreach (GameObject enemy in enemys)
        //    {
        //        if (enemy != null) ObjectPool.GetInstance().RecycleObj(enemy);
        //    }
        //}
        //销毁所有有关的敌人、子弹
        GameObject[] Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < Enemy.Length; i++)
        {
            ObjectPool.GetInstance().RecycleObj(Enemy[i]);
        }
        GameObject[] EnemyBullet = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < EnemyBullet.Length; i++)
        {
            ObjectPool.GetInstance().RecycleObj(EnemyBullet[i]);
        }
        GameObject[] Bullet = GameObject.FindGameObjectsWithTag("Bullet");
        for (int i = 0; i < Bullet.Length; i++)
        {
            ObjectPool.GetInstance().RecycleObj(Bullet[i]);
        }
    }
}
