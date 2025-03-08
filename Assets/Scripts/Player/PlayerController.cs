using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // 角色移动速度
    private Vector3 targetPosition; // 目标位置
    private bool isMoving = false; // 是否正在移动

    void Update()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 从摄像机发射一条射线到鼠标点击的位置
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // 检测射线是否击中了某个物体
            if (Physics.Raycast(ray, out hit))
            {
                // 检查击中的物体是否属于CanGo图层
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("CanGo"))
                {
                    // 设置目标位置
                    targetPosition = hit.point;
                    isMoving = true;
                }
            }
        }

        // 如果正在移动，向目标位置移动
        if (isMoving)
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        // 计算移动方向
        Vector3 direction = (targetPosition - transform.position).normalized;

        // 移动角色
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 如果角色接近目标位置，停止移动
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }
    /*public float moveSpeed; // 移动速度
    private CharacterController characterController;
    void Start()
    {
        // 获取Character Controller组件
        characterController = GetComponent<CharacterController>();
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

            // 移动玩家
            transform.position += rotatedDirection * moveSpeed * Time.deltaTime;
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
    }*/
}