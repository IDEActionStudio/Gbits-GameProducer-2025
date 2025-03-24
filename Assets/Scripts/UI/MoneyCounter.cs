using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private PlayerCharacter playerCharacter;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        playerCharacter=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        textMesh.text ="Money:"+ playerCharacter.money.ToString();
    }
}
