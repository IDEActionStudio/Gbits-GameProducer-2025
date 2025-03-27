using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public string poolTag;
    
    private float maxMapSize=380f;
    
    public UnityEvent OnHurt;
    public UnityEvent OnDie;
    public ItemEffect itemEffect;
    private bool enable09;
    
    public int moneyAmount;
    public GameObject moneyPrefab;
    private Quaternion moneyRotation = new Quaternion(0.65f, 0.27f, -0.27f, 0.65f);
    
    private bool enable01;
    public GameObject blockingStripPrefab;

    protected virtual void OnEnable()
    {
        Item01Effect.OnItem01Effect += TriggerItem01Effect;
        Item02Effect.OnItem02Effect += IncreaseMoneyDrop;
        Item09Effect.OnItem09Effect += TriggerItem09Effect;
    }

    protected virtual void OnDisable()
    {
        Item01Effect.OnItem01Effect -= TriggerItem01Effect;
        Item02Effect.OnItem02Effect -= IncreaseMoneyDrop;
        Item09Effect.OnItem09Effect -= TriggerItem09Effect;
    }

    public virtual void TakeDamage()
    {
        OnDie.Invoke();
    }
    
    private Vector3 offset=new Vector3(0f,3.5f,0f);
    // 敌人死亡时调用
    public void Die()
    {
        for (int i = 0; i < moneyAmount; i++)
        {
            Debug.Log(transform.position);
            Instantiate(moneyPrefab, transform.position, moneyRotation);
        }
        // 触发死亡动画或效果
        Debug.Log("Enemy Died!");
        if (enable01)
        {
            Instantiate(blockingStripPrefab, transform.position,Quaternion.identity);
        }
        if (enable09)
        {
            itemEffect.ApplyEffect();
            enable09 = false;
        }
        // 回收敌人到对象池
        MyPooler.ObjectPooler.Instance.ReturnToPool(poolTag,gameObject);
        //Destroy(gameObject);
    }

    private void TriggerItem01Effect()
    {
        enable01 = true;
    }
    
    private void TriggerItem09Effect()
    {
        enable09 = true;
    }

    private void IncreaseMoneyDrop()
    {
        moneyAmount += 2;
    }
}