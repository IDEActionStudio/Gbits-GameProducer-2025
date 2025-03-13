using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;  // 敌人基础移动速度
    public string targetTag = "Player"; //当前目标Tag
    public float shootInterval = 2f;  // 发射时间间隔
    public float bulletInterval = 0.5f; //多颗子弹发射时的间隔
    public int bulletsPerShot = 1;  // 每次发射的子弹数量
    public float directionChangeInterval = 3f;  // 敌人改变移动方向的时间间隔
    public LayerMask platformLayer;  // 平台的图层（用于碰撞检测）

    protected Transform target;  // 引用玩家位置
    protected float shootTimer = 0f;  // 射击计时器
    protected bool canAttack;  // 控制敌人是否可以攻击
    protected Vector3 moveDirection;  // 敌人当前移动方向
    protected float directionChangeTimer = 0f;  // 改变移动方向的计时器
    protected Camera mainCamera;  // 引用主摄像机
    
    private Animator animator;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        //if (target == null)
        //{
        //    Debug.LogError("Player not found! Make sure your player object has the 'Player' tag.");
        //    return;
        //}

        //if (customTimeManager == null)
        //{
        //    Debug.LogError("CustomTimeManager not found! Make sure it is present in the scene.");
        //    return;
        //}


        //if (mainCamera == null)
        //{
        //    Debug.LogError("Main camera not found!");
        //    return;
        //}

        //if (enemyCollider == null)
        //{
        //    Debug.LogError("Enemy Collider2D not found!");
        //    return;
        //}
    }

    protected virtual void Update()
    {
        MoveEnemy();  // 控制敌人移动
        CheckDirection();
        if(Vector3.Distance(transform.position, target.position) < 0.1f)
            EnableAttack();
        //UpdateTarget(targetTag);
        if (canAttack)
        {
            AttackAtPlayer();  // 攻击逻辑
        }
        
    }
    private void CheckDirection()
    {
        if (target != null)
        {
            float distance = target.position.x - transform.position.x;
            if (distance > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); 
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    private void UpdateTarget(string s)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(s);
        if (targets.Length == 0)
        {
            target = null;
        }
        else
        {
            target = targets[Random.Range(0, targets.Length)].transform;

        }
    }

    /*private void SetRandomSpawnPosition()
    {
        UpdateTarget(targetTag);
        Vector3 cameraMin = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 cameraMax = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        bool positionFound = false;
        Vector3 randomPosition = Vector3.zero;

        while (!positionFound)
        {
            float xPosition = Random.Range(cameraMin.x, cameraMax.x);
            float yPosition = Random.Range(cameraMin.y, cameraMax.y);
            randomPosition = new Vector3(xPosition, yPosition, 0);

            if (Vector3.Distance(randomPosition, target.position) < spawnDistanceFromPlayer)
            {
                continue;
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPosition, 0.5f);
            bool overlapWithPlatform = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Platform"))
                {
                    overlapWithPlatform = true;
                    break;
                }
            }

            if (!overlapWithPlatform)
            {
                positionFound = true;
            }
        }
        transform.position = randomPosition;
    }*/

    private void SetRandomMoveDirection()
    {
        UpdateTarget(targetTag);
        if (target == null) return;
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        moveDirection = new Vector3(transform.position.x - target.position.x, transform.position.y - target.position.y).normalized * 0.3f + randomDirection * 0.7f;
        moveDirection.Normalize();
    }

    protected virtual void MoveEnemy()
    {

        if (directionChangeTimer >= directionChangeInterval)
        {
            SetRandomMoveDirection();
            directionChangeTimer = 0f;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 1f, platformLayer);
        if (hit.collider != null)
        {
            Vector3 collisionNormal = hit.normal;
            Vector3 newDirection = Vector3.Reflect(moveDirection, collisionNormal);
            moveDirection = newDirection.normalized;
        }

        Vector3 newPosition = transform.position + moveDirection * moveSpeed;
        if (IsPositionWithinCameraBounds(newPosition))
        {
            transform.position = newPosition;
        }
        else
        {
            SetRandomMoveDirection();
        }
    }

    private bool IsPositionWithinCameraBounds(Vector3 position)
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(position);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }

    private void EnableAttack()
    {
        canAttack = true;
    }

    protected void AttackAtPlayer()
    {
        if (shootTimer >= shootInterval)
        {
            StartCoroutine(Attacks());
            shootTimer = 0f;
        }
    }

    IEnumerator Attacks()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Attack();
            yield return new WaitForSeconds(bulletInterval);
        }
    }

    private void Attack()
    {
        
        UpdateTarget(targetTag);
        //敌人的攻击还没做
        animator.Play("Attack");
    }

    public virtual void TakeDamage()
    {
        Die();
    }
    
    protected virtual void Die()
    {
        //FindObjectOfType<EnemyManager>()?.OnEnemyDestroyed();
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager.enemys.Contains(gameObject))
        {
            enemyManager.enemys.Remove(gameObject);
        }
        ObjectPool.GetInstance().RecycleObj(gameObject);
    }
    protected virtual void OnEnable()
    {
        UpdateTarget("Player");
        mainCamera = Camera.main;
        
    }
    protected virtual void OnDisable()
    {
        StopAllCoroutines();
        ObjectPool.GetInstance().RecycleObj(gameObject);
    }
}
