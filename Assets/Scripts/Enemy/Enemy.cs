using UnityEngine;

public class Enemy : MonoBehaviour
{
    private ObjectPool pool;

    // 设置对象池引用
    public void SetPool(ObjectPool pool)
    {
        this.pool = pool;
    }

    // 敌人死亡时调用
    public void Die()
    {
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