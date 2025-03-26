using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinManger : MonoBehaviour
{
    public static CoinManger instance;// 单例模式，方便全局访问
   [SerializeField] private TextMeshProUGUI coinText;//金币文本
    //private int coins=0;//金币数量
    private PlayerCharacter playerCharacter;
    private const string strCoins= "Coins: ";
    void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        UpdateUI();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        UpdateUI();
    }


    /*public void AddCoins(int amount)//敌人死亡时调用
    {
        coins += amount;
        UpdateUI();
        Debug.Log(strCoins+coins);
    }*/
    private void UpdateUI()//更新UI中金币数量
    {
        coinText.text = strCoins +playerCharacter.money;
         Debug.Log("已更新");

    }

}
