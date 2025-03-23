using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : Interactive
{
    public ItemEffect itemEffect;
    private float radius = 2f; // 圆的半径
    public float moveSpeed = 2f; // 移动速度

    private Vector3 targetPosition; // 目标位置
    private bool isMoving; // 是否正在移动

    protected virtual void Start()
    {
        base.Start();
        // 初始化目标位置
        SetRandomTargetPosition();
    }

    void Update()
    {
        if (isMoving)
        {
            // 使用 Lerp 平滑移动
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 检查是否接近目标位置
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false; // 停止移动
            }
        }
    }

    void SetRandomTargetPosition()
    {
        // 生成随机角度（0 到 2π）
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // 计算圆上的随机点
        float x = radius * Mathf.Cos(randomAngle);
        float z = radius * Mathf.Sin(randomAngle);

        // 设置目标位置（以自身为圆心）
        targetPosition = transform.position + new Vector3(x, 0, z);

        // 开始移动
        isMoving = true;
    }
    
    protected override void MakeSomeReaction()
    {
        base.MakeSomeReaction();
        itemEffect.ApplyEffect();
        Destroy(gameObject);
    }
}
