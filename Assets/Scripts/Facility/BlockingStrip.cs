using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingStrip : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.magenta);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<EnemyAI>().CommonStrip();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<EnemyAI>().StopCommonStrip();
        }
    }
}
