using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    /// <summary>
    /// �����˺�
    /// </summary>
    public int attackDamage = 0;
    /// <summary>
    /// �����ͷż��
    /// </summary>
    public float attackInterval = 5;
    /// <summary>
    /// ���ܸ���
    /// </summary>
    public int attackNum = 4;
    /// <summary>
    /// �����ͷż�ʱ��
    /// </summary>
    protected float currentIntervalTime = 0; 
    /// <summary>
    /// ����ʱ����
    /// </summary>
    protected float currentAttackTime; 
    /// <summary>
    /// ��ǰ�ͷŵļ�������
    /// </summary>
    protected int currentAttackIndex; 
    /// <summary>
    /// �Ƿ����ͷż��ܹ����� 
    /// </summary>
    protected bool isAttack;
    /// <summary>
    /// boss����������
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
            MoveEnemy();  // ���Ƶ����ƶ�
            if (canAttack)
            {
                AttackAtPlayer();  // ����߼�
            }
            //�ͷż���
            CanAttack();
        }


    }
    /// <summary>
    /// �ж��Ƿ�����ͷż���
    /// </summary>
    private void CanAttack()
    {
        if (currentIntervalTime >= attackInterval)
        {
            //��ʼ��
            isAttack = true;
            currentIntervalTime = 0;
            //ѡ��ǰ��Ҫִ�еļ���
            currentAttackIndex = Random.Range(0, attackNum);
        }
        else
        {
            currentIntervalTime += Time.deltaTime;
        }
    }
    /// <summary>
    /// �ͷż���
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
    //����1
    protected virtual void AttackFirst()
    {
    //    Debug.Log("boss的第一种攻击");
    }
    //����2
    protected virtual void AttackSecond()
    {
     //   Debug.Log("boss的第二种攻击");

    }
    //����3
    protected virtual void AttackThird()
    {
        Debug.Log("boss的第三种攻击");

    }
    //����4
    protected virtual void AttackFourth()
    {
        Debug.Log("boss的第四种攻击");
    }
    protected void StopAttack()
    {
        isAttack = false;
        currentAttackTime = 0;
    }
}
