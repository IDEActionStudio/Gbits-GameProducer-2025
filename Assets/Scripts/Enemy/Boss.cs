using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    /// <summary>
    /// 攻击伤害
    /// </summary>
    public int attackDamage = 0;
    
    /// <summary>
    /// 攻击释放间隔
    /// </summary>
    public float attackInterval = 5;
    
    /// <summary>
    /// 技能数量
    /// </summary>
    public int attackNum = 4;
    
    /// <summary>
    /// 当前攻击间隔计时
    /// </summary>
    protected float currentIntervalTime = 0; 
    
    /// <summary>
    /// 攻击计时器
    /// </summary>
    protected float currentAttackTime; 
    
    /// <summary>
    /// 当前释放的技能索引
    /// </summary>
    protected int currentAttackIndex; 
    
    /// <summary>
    /// 是否正在释放技能
    /// </summary>
    protected bool isAttack;
    
    /// <summary>
    /// Boss的初始化方法
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isAttack)
        {
            Attack();
        }
        else
        {
            MoveEnemy();  // 控制敌人的移动
            if (canAttack)
            {
                AttackAtPlayer();  // 攻击逻辑
            }
            // 判断是否可以释放技能
            CanAttack();
        }
    }
    
    /// <summary>
    /// 判断是否可以释放技能
    /// </summary>
    private void CanAttack()
    {
        if (currentIntervalTime >= attackInterval)
        {
            // 开始攻击
            isAttack = true;
            currentIntervalTime = 0;
            // 随机选择当前要执行的技能
            currentAttackIndex = Random.Range(0, attackNum);
        }
        else
        {
            currentIntervalTime += Time.deltaTime;
        }
    }
    
    /// <summary>
    /// 释放技能
    /// </summary>
    private void Attack()
    {
        switch (currentAttackIndex)
        { 
            case 0:
                AttackFirst();
                break;
            case 1:
                AttackSecond();
                break;
            case 2:
                AttackThird();
                break;
            case 3:
                AttackFourth();
                break;
            default:
                break;
        }
        if (!isAttack) return;
        currentAttackTime += Time.deltaTime;
    }
    
    // 技能1
    protected virtual void AttackFirst()
    {
        // Debug.Log("Boss的第一种攻击");
    }
    
    // 技能2
    protected virtual void AttackSecond()
    {
        // Debug.Log("Boss的第二种攻击");
    }
    
    // 技能3
    protected virtual void AttackThird()
    {
        Debug.Log("Boss的第三种攻击");
    }
    
    // 技能4
    protected virtual void AttackFourth()
    {
        Debug.Log("Boss的第四种攻击");
    }
    
    /// <summary>
    /// 停止攻击
    /// </summary>
    protected void StopAttack()
    {
        isAttack = false;
        currentAttackTime = 0;
    }
}