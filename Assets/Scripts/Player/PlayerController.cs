using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; // 移动速度
    public float dashSpeed; // 角色移动速度
    private bool canDash;
    public GameObject mouseTargetIndicator; // 鼠标落点指示器
    private Image imageMouse;
    public Image lineImage; // 拖入一个UI Image
    private float rayDistance = 1f;
    private Vector3 targetPosition; // 目标位置
    public bool isMoving; // 是否正在移动
    private Vector3 mouseScreenPos;
    private CharacterController characterController;
    private Animator animator;

    private void Start()
    {
        canDash = true;
        imageMouse = mouseTargetIndicator.GetComponent<Image>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 获取输入方向
        float horizontal = Input.GetAxis("Horizontal"); // A/D 或 左右箭头
        float vertical = Input.GetAxis("Vertical");     // W/S 或 上下箭头

        // 创建输入方向向量
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        // 如果玩家有输入
        if (inputDirection != Vector3.zero)
        {
            // 将输入方向旋转45度
            Vector3 rotatedDirection = RotateVector(inputDirection, -45f);
            Vector3 movement=rotatedDirection * moveSpeed * Time.deltaTime;
            // 移动玩家
            if(!isMoving)
                characterController.Move(movement);
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
        }
        // 检测鼠标左键点击
            // 从摄像机发射一条射线到鼠标点击的位置
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // 检测射线是否击中了某个物体
            if (Physics.Raycast(ray, out hit))
            {
                // 更新鼠标落点指示器的位置
                DrawLineToTarget(hit.point);
                mouseScreenPos = Camera.main.WorldToScreenPoint(hit.point);
                mouseTargetIndicator.transform.position = mouseScreenPos;
                // 检查击中的物体是否属于CanGo图层
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("CanGo")
                    ||hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy")
                    ||hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactive"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (canDash)
                        {
                            targetPosition = hit.point;
                            isMoving = true;
                            canDash = false;
                            StartCoroutine(triggerDash(1));
                        }
                        
                    }
                }
            }
        // 如果正在移动，向目标位置移动
        if (isMoving)
        {
            MoveToTarget();
        }
    }

    private IEnumerator triggerDash(float duration)
    {
        yield return new WaitForSeconds(duration);
        canDash = true;
    }
    

    void MoveToTarget()
    {
        // 计算移动方向
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (CheckForObstacles(direction))
        {
            // 如果检测到障碍物，停止移动
            isMoving = false;
            return;
        }
        // 移动角色
        characterController.Move(direction * dashSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 1.3f)
        {
            isMoving = false;
        }
    }

    
    
    bool CheckPathForObstacles(Vector3 target)
    {
        // 计算从当前位置到目标位置的方向和距离
        Vector3 direction = (target - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target);

        // 合并需要检测的图层
        int obstacleLayerMask = (1 << LayerMask.NameToLayer("Buildings")) |
                                (1 << LayerMask.NameToLayer("Vehicles")) |
                                (1 << LayerMask.NameToLayer("Props"));

        // 发射射线，检测路径上是否有障碍物
        if (Physics.Raycast(transform.position, direction, distance, obstacleLayerMask))
        {
            // 如果检测到障碍物，返回true
            return true;
        }

        // 没有检测到障碍物，返回false
        return false;
    }

    bool CheckForObstacles(Vector3 direction)
    {
        // 射线长度
        
        // 合并需要检测的图层
        int obstacleLayerMask = (1 << LayerMask.NameToLayer("Buildings")) |
                                (1 << LayerMask.NameToLayer("Vehicles")) |
                                (1 << LayerMask.NameToLayer("Props"));

        // 发射射线，检测前方是否有障碍物
        if (Physics.Raycast(transform.position, direction, rayDistance, obstacleLayerMask))
        {
            // 如果检测到障碍物，返回true
            return true;
        }

        // 没有检测到障碍物，返回false
        return false;
    }
    
    

    void DrawLineToTarget(Vector3 target)
    {
        // 将世界坐标转换为屏幕坐标
        Vector3 startScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 endScreenPos = Camera.main.WorldToScreenPoint(target);

        // 计算线的长度和角度
        float length = Vector3.Distance(startScreenPos, endScreenPos);
        float angle = Mathf.Atan2(endScreenPos.y - startScreenPos.y, endScreenPos.x - startScreenPos.x) * Mathf.Rad2Deg;

        // 设置线的位置、旋转和大小
        lineImage.rectTransform.position = (startScreenPos + endScreenPos) / 2;
        lineImage.rectTransform.sizeDelta = new Vector2(length, lineImage.rectTransform.sizeDelta.y);
        lineImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        // 检测路径上是否有障碍物
        if (CheckPathForObstacles(target))
        {
            lineImage.gameObject.SetActive(true);
            lineImage.color = Color.red; // 线变红
            imageMouse.color = Color.red;
        }
        else
        {
            lineImage.gameObject.SetActive(false);
            imageMouse.color = Color.white;
        }
    }

    // 旋转向量的辅助函数
    private Vector3 RotateVector(Vector3 vector, float angle)
    {
        // 将角度转换为弧度
        float radians = angle * Mathf.Deg2Rad;

        // 计算旋转后的X和Z分量
        float x = vector.x * Mathf.Cos(radians) - vector.z * Mathf.Sin(radians);
        float z = vector.x * Mathf.Sin(radians) + vector.z * Mathf.Cos(radians);

        // 返回旋转后的向量
        return new Vector3(x, 0, z).normalized;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (isMoving)
            {
                other.gameObject.GetComponent<Enemy>().OnHurt.Invoke();
                canDash = true;
            }
        }
    }
}