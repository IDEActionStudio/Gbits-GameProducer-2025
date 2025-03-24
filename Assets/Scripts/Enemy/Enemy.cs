using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public UnityEvent OnHurt;
    public ItemEffect itemEffect;
    private bool enable09;
    
    private ObjectPool pool;
    
    public int moneyAmount;
    public GameObject moneyPrefab;
    private Quaternion moneyRotation = new Quaternion(0.65f, 0.27f, -0.27f, 0.65f);

    private void OnEnable()
    {
        Item09Effect.OnItem09Effect += TriggerItem09Effect;
    }

    private void OnDisable()
    {
        Item09Effect.OnItem09Effect -= TriggerItem09Effect;
    }

    // 设置对象池引用
    public void SetPool(ObjectPool pool)
    {
        this.pool = pool;
    }

    public virtual void TakeDamage()
    {
        Die();
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
        if (enable09)
        {
            itemEffect.ApplyEffect();
        }
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
    
    private void TriggerItem09Effect()
    {
        enable09 = true;
    }
}