using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����
/// </summary>
public class VaryEnemy : Enemy
{
    //�Ƿ��ڼ���״̬
    private bool isActive;
    public float explosionRadius = 3f; //ը����ը�뾶
    public string explosionEffect = "BoomEffectPrefab"; //��ը��Ч
    public int damage = 2; //��ը�˺�
    public int activeSpeed = 20; //����ʱ���ٶ�
    protected override void Update()
    {
        //FollowHealthBar();
        MoveEnemy();  // ���Ƶ����ƶ�
    }
    protected override void MoveEnemy()
    {
        if (isActive)
        {
            //被打扰后的移动方法
        }
        else
        {
            //被打扰前的移动方法
        }
        
    }
    public override void TakeDamage()
    {
        if(isActive)
            Die();
        else
            StartCoroutine(BeingActive());
    }
    //����
    IEnumerator BeingActive()
    {
        yield return new WaitForFixedUpdate();
        //�������
        moveSpeed = activeSpeed;
        isActive = true;
    }
    // Update is called once per frame

    protected override void OnDisable()
    {
        //�ٶ���0
        isActive = false;
        base.OnDisable();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive)
        {
            if (collision.gameObject.CompareTag(targetTag))
            {
                //��ը
                Boom();
                Die();
            }
        }
    }
    //��ըЧ��
    void Boom()
    {
        //���ɱ�ը��Ч
        if (explosionEffect != null)
        {
            GameObject boomEffect = ObjectPool.GetInstance().GetObj(explosionEffect, transform.position, transform.rotation);
            ObjectPool.GetInstance().RecycleObj(boomEffect, 2f);
        }
        // ��ȡ��Χ�ڵ����е���
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
    }
}
