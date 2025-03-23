using System;
using UnityEngine;
using UnityEngine.AI;

public class Interactive : MonoBehaviour
{
    public Animator animator; // 动画组件
    public AudioSource audioSource; // 音效组件
    public string triggerName = "PlayAnimation"; // 动画触发器名称
    protected GameObject player; // 玩家对象
    protected PlayerCharacter playerCharacter;

    protected virtual void Start()
    {
        // 假设玩家对象有 "Player" 标签
        player = GameObject.FindGameObjectWithTag("Player");
        playerCharacter=player.GetComponent<PlayerCharacter>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") )
            Interact();
    }

    protected virtual void Interact()
    {
        // 检查是否按下右键
        if (Input.GetKeyDown(KeyCode.E) )
        {
            Debug.Log("Interact");
            //不同交互物要做不同的事情
            MakeSomeReaction();
        }
    }

    protected virtual void MakeSomeReaction()
    {
        PlayAnimation();
        PlayAudio();
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
