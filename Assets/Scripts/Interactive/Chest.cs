using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Chest : Interactive
{
    
    private int requireMoney;
    private string itemsFolderPath = "Prefabs/Items"; // 道具预制体的路径
    private bool isOpened; // 宝箱是否已开启
    private int canDouble=0;
    private void OnEnable()
    {
        Item10Effect.OnItem10Effect += AddSpawnItemCount;
    }

    private void OnDisable()
    {
        Item10Effect.OnItem10Effect -= AddSpawnItemCount;
    }

    protected override void Interact()
    {
        // 检查是否按下右键
        if (Input.GetMouseButtonDown(1)&&(!isOpened)&&playerCharacter.money >= requireMoney) 
        {
            //不同交互物要做不同的事情
            MakeSomeReaction();
        }
    }

    protected override void MakeSomeReaction()
    {
        base.MakeSomeReaction();
        playerCharacter.money -= requireMoney;
        SpawnRandomItem();
        isOpened = true;
    }
    
    private void SpawnRandomItem()
    {
        // 从指定文件夹加载所有道具预制体
        GameObject[] itemPrefabs = Resources.LoadAll<GameObject>(itemsFolderPath);

        if (itemPrefabs.Length > 0)
        {
            if (canDouble==0)// 随机选择一个道具，如果有道具10则生成两个
            {
                int randomIndex = Random.Range(0, itemPrefabs.Length);
                GameObject selectedItemPrefab = itemPrefabs[randomIndex];
                //GameObject selectedItemPrefab = itemPrefabs[3];
                // 在宝箱的位置生成道具
                Instantiate(selectedItemPrefab, transform.position, Quaternion.identity);
                Debug.Log("生成道具: " + selectedItemPrefab.name);
            }
            else if (canDouble!=0)
            {
                for (int i = 0; i <= canDouble; i++)
                {
                    int randomIndex = Random.Range(0, itemPrefabs.Length);
                    GameObject selectedItemPrefab = itemPrefabs[randomIndex];
                    //GameObject selectedItemPrefab = itemPrefabs[3];
                    // 在宝箱的位置生成道具
                    Instantiate(selectedItemPrefab, transform.position, Quaternion.identity);
                    Debug.Log("生成道具: " + selectedItemPrefab.name);
                }
                canDouble=0;
            }
        }
        else
        {
            Debug.LogWarning("未找到道具预制体，请检查路径: " + itemsFolderPath);
        }
    }

    private void AddSpawnItemCount()
    {
        canDouble++;
    }
}
