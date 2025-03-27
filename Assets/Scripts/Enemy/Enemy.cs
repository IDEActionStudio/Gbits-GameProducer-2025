using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private float maxMapSize=380f;
    
    public UnityEvent OnHurt;
    public UnityEvent OnDie;
    public ItemEffect itemEffect;
    private bool enable09;
    
    public int moneyAmount;
    public GameObject moneyPrefab;
    private Quaternion moneyRotation = new Quaternion(0.65f, 0.27f, -0.27f, 0.65f);

    protected virtual void OnEnable()
    {
        Item02Effect.OnItem02Effect += IncreaseMoneyDrop;
        Item09Effect.OnItem09Effect += TriggerItem09Effect;
    }

    protected virtual void OnDisable()
    {
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
        if (enable09)
        {
            itemEffect.ApplyEffect();
            enable09 = false;
        }
        // 回收敌人到对象池
        MyPooler.ObjectPooler.Instance.ReturnToPool("CommonEnemy",gameObject);
        //Destroy(gameObject);
    }
    
    private void TriggerItem09Effect()
    {
        enable09 = true;
    }

    private void IncreaseMoneyDrop()
    {
        moneyAmount += 2;
    }
    
    /*Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(0,maxMapSize), 0, Random.Range(0,maxMapSize));
        return randomPosition;
    }*/
}