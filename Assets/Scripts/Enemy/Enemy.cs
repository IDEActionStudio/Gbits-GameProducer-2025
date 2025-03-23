using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private ObjectPool pool;
    
    public int moneyAmount;
    public GameObject moneyPrefab;
    private Quaternion moneyRotation = new Quaternion(0.65f, 0.27f, -0.27f, 0.65f);
    

    // 设置对象池引用
    public void SetPool(ObjectPool pool)
    {
        this.pool = pool;
    }

    // 敌人死亡时调用
    public void Die()
    {
        for (int i = 0; i < moneyAmount; i++)
        {
            Instantiate(moneyPrefab, transform.position, moneyRotation);
        }
        // 触发死亡动画或效果
        Debug.Log("Enemy Died!");

        // 回收敌人到对象池
        if (pool != null)
        {
            pool.ReturnObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}