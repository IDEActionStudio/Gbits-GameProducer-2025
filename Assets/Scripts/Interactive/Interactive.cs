using UnityEngine;
using UnityEngine.AI;

public class Interactive : MonoBehaviour
{
    public float interactionDistance; // 交互距离
    public Animator animator; // 动画组件
    public AudioSource audioSource; // 音效组件
    public string triggerName = "PlayAnimation"; // 动画触发器名称
    private GameObject player; // 玩家对象

    void Start()
    {
        // 假设玩家对象有 "Player" 标签
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // 检查玩家是否在交互距离内
        if (Vector3.Distance(player.transform.position, transform.position) <= interactionDistance)
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {
        // 检查是否按下右键
        if (Input.GetMouseButtonDown(1)) // 1 表示右键
        {
            //不同交互物要做不同的事情
            MakeSomeReaction();
        }
    }

    protected virtual void MakeSomeReaction()
    {
        
    }

    protected void PlayAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    protected void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
