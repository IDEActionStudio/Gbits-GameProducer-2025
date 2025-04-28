using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingManger : MonoBehaviour
{
    [SerializeField] private GameObject SettingSysetm;
    private bool isMenuOpen = false;//esc�����������
    void Start()
    {
        SettingSysetm.SetActive(false);//��ʼʱ���ò˵�Ϊ������״̬
    }

    
    void Update()
    {
        ShowSettingSystem();//ÿ֡��⣬��������Ż�
    }
    public void ShowSettingSystem()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //����esc�������ò˵�
        {
            isMenuOpen = !isMenuOpen;//ȷ���ٰ�esc�ܼ�����Ϸ
            SettingSysetm.SetActive(isMenuOpen);
        }
        Time.timeScale = isMenuOpen ? 0 : 1;//ͨ�� isMenuOpen�ж���Ϸ�Ƿ���ͣ
    }
}
