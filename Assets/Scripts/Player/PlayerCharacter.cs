using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : MonoBehaviour
{
    public UnityEvent OnHurt;
    public UnityEvent OnDeath;
    //属性
    public int money;
    private int shield;
    private bool canRevive;
    //组件引用
    private Animator animator;
    private void OnEnable()
    {
        Item06Effect.OnItem06Effect += EnableRevive;
        Item05Effect.OnItem05Effect += AddShield;
    }

    private void OnDisable()
    {
        Item06Effect.OnItem06Effect -= EnableRevive;
        Item05Effect.OnItem05Effect -= AddShield;
    }

    public void Reset()
    {
        money = 0;
        shield = 0;
        canRevive = false;
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

    private void AddShield()
    {
        shield++;
    }
}
