using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    public EnemyState currentState = EnemyState.Patrol;
    public float detectionRange = 15f; // 检测范围
    public float attackRange = 2f;    // 攻击范围
    private Transform player;
    private float lastAttackTime;
    private float attackCooldown=1f;
    private Vector3 lastKnownPlayerPosition;
    private NavMeshAgent agent;

    private int item01Count;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // 禁止自动旋转
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        Item01Effect.OnItem01Effect += AddItem01Count;
    }

    private void OnDisable()
    {
        Item01Effect.OnItem01Effect -= AddItem01Count;
    }

    
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 状态切换逻辑
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (distanceToPlayer <= detectionRange )
           // && distanceToPlayer >= attackRange
        {
            currentState = EnemyState.Chase;
        }
        else if (currentState != EnemyState.Patrol)
        {
            currentState = EnemyState.Patrol;
        }

        // 状态执行逻辑
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    void Patrol()
    {
        
    }
    
    void Chase()
    {
        if (player != null)
        {
            if (Time.frameCount % 10 == 0)
            agent.SetDestination(player.position); // 设置目标为玩家的位置
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Enemy Attacked!");
            player.GetComponent<PlayerCharacter>().OnHurt.Invoke();
            lastAttackTime = Time.time;
        }
    }

    void OnDrawGizmos()
    {
        // 在场景视图中绘制射线
        if (currentState == EnemyState.Chase)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (player.position - transform.position).normalized * detectionRange);
        }
    }
    
    public void CommonStrip()
    {
        agent.speed *= 0.25f;
    }
    public void StopCommonStrip()
    {
        agent.speed = 3.5f;
    }
    

    public void Strip()
    {
        for(int i=0;i<item01Count;i++)
            agent.speed *= 0.75f;
    }

    public void StopStrip()
    {
        agent.speed = 3.5f;
    }
    
    private void AddItem01Count()
    {
        item01Count++;
    }
}