using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEnemy : Enemy
{
    private float playerOriginalSpeed=15f;
    private PlayerController playerController;

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerController.moveSpeed = 8f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerController.moveSpeed = playerOriginalSpeed;
    }
}
