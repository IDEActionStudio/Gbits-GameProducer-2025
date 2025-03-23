using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    private Transform playerTransform; // 玩家的Transform
    private float maxDistance = 5f;    // 最大距离限制

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // 获取鼠标在屏幕上的位置
        Vector3 mouseScreenPosition = Input.mousePosition;

        // 将鼠标的屏幕坐标转换为世界坐标
        // 注意：这里需要传入一个合适的Z值，例如摄像机到物体的距离
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.transform.position.y - transform.position.y));
// 计算鼠标位置与玩家位置的距离
        float distance = Vector3.Distance(playerTransform.position, mouseWorldPosition);

        // 如果距离超过最大距离，限制鼠标位置
        if (distance > maxDistance)
        {
            // 计算从玩家指向鼠标的方向
            Vector3 direction = (mouseWorldPosition - playerTransform.position).normalized;

            // 将鼠标位置限制在最大距离范围内
            mouseWorldPosition = playerTransform.position + direction * maxDistance;
        }
        // 计算玩家和鼠标连线的中点
        Vector3 midpoint = (playerTransform.position + mouseWorldPosition) / 2f;

        // 设置当前物体的位置为中点
        transform.position = midpoint;
    }
}
