using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyPooler;

public class GenericObject : MonoBehaviour, IPooledObject
{
    public float defTimerToDestroy = 3f;
    public float timeToDestroy = 3f;
    public string poolTag;
    public bool isActive = false;

    public void OnRequestedFromPool()
    {
        isActive = true;
        timeToDestroy = defTimerToDestroy;
    }

    public void DiscardToPool()
    {
        MyPooler.ObjectPooler.Instance.ReturnToPool(poolTag, this.gameObject);
        isActive = false;
    }

    void Update()
    {
        if (isActive)
        {
            timeToDestroy -= Time.deltaTime;
            if (timeToDestroy <= 0f)
            {
                DiscardToPool();
            }
        }
    }
}
