using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingManger : MonoBehaviour
{
    [SerializeField] private GameObject SettingSysetm;
    private bool isMenuOpen = false;//esc按键次数监测
    void Start()
    {
        SettingSysetm.SetActive(false);//初始时设置菜单为不激活状态
    }

    
    void Update()
    {
        ShowSettingSystem();//每帧检测，或许可以优化
    }
    public void ShowSettingSystem()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //按下esc进入设置菜单
        {
            isMenuOpen = !isMenuOpen;//确保再按esc能继续游戏
            SettingSysetm.SetActive(isMenuOpen);
        }
        Time.timeScale = isMenuOpen ? 0 : 1;//通过 isMenuOpen判断游戏是否暂停
    }
}
