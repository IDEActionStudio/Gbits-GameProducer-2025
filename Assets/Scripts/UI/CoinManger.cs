using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinManger : MonoBehaviour
{
    public static CoinManger instance;// ����ģʽ������ȫ�ַ���
   [SerializeField] private TextMeshProUGUI coinText;//����ı�
    //private int coins=0;//�������
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


    /*public void AddCoins(int amount)//��������ʱ����
    {
        coins += amount;
        UpdateUI();
        Debug.Log(strCoins+coins);
    }*/
    private void UpdateUI()//����UI�н������
    {
        coinText.text = strCoins +playerCharacter.money;
         Debug.Log("�Ѹ���");

    }

}
