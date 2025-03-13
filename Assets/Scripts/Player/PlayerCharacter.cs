using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : MonoBehaviour
{
    public UnityEvent OnDeath;
    //数值
    private int shield;
    private bool canRevive;
    //组件引用
    private Animator animator;

    private void OnEnable()
    {
        Item06Effect.OnItem06Effect += EnableRevive;
    }

    private void OnDisable()
    {
        Item06Effect.OnItem06Effect -= EnableRevive;
    }

    public void TakeDamage()
    {
        if (shield > 0)
        {
            shield--;
        }
        else
        {
            if (canRevive)
            {
                canRevive = false;
                return;
            }
            else
                OnDeath.Invoke();
        }
    }

    private void EnableRevive()
    {
        canRevive = true;
    }
}
