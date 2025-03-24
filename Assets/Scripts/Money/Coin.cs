using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    // 移动速度
    private float moveSpeed = 5f;
    private float flySpeed = 20f;
    // 随机点的位置
    private Vector3 randomPoint;

    // 是否已经到达随机点
    private bool reachedRandomPoint = false;

    // 玩家的Transform组件
    private Transform player;

    void Start()
    {
        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
        // 查找玩家对象（假设玩家对象的标签是 "Player"）
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 如果没有找到玩家对象，输出警告
        if (player == null)
        {
            Debug.LogWarning("Player not found! Make sure the player has the tag 'Player'.");
        }

        // 生成随机点
        GenerateRandomPoint();
    }

    void Update()
    {
        if (!reachedRandomPoint)
        {
            // 向随机点移动
            MoveToRandomPoint();
        }
        else
        {
            // 向玩家移动
            StartCoroutine(ExecuteAfterDelay(1));
        }
    }

    // 生成随机点
    void GenerateRandomPoint()
    {
        // 随机生成一个角度（0到360度）
        float angle = Random.Range(0f, 360f);

        // 将角度转换为弧度
        float radians = angle * Mathf.Deg2Rad;

        // 计算随机点的位置
        randomPoint = transform.position + new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians)) *Random.Range(0.8f, 4f);
    }

    // 向随机点移动
    void MoveToRandomPoint()
    {
        // 计算移动方向
        Vector3 direction = (randomPoint - transform.position).normalized;

        // 移动物体
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 检查是否到达随机点
        if (Vector3.Distance(transform.position, randomPoint) < 0.1f)
        {
            reachedRandomPoint = true;
        }
    }

    private IEnumerator ExecuteAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        MoveToPlayer();
    }

    // 向玩家移动
    void MoveToPlayer()
    {
        if (player != null)
        {
            // 计算移动方向
            Vector3 direction = (player.position - transform.position).normalized;

            // 移动物体
            transform.position += direction * flySpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            other.gameObject.GetComponent<PlayerCharacter>().money += 1;
            Destroy(gameObject);
        }
    }
}
