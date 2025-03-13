using UnityEngine;

public class Chest : Interactive
{
    private string itemsFolderPath = "Prefabs/Items"; // 道具预制体的路径
    private bool isOpened; // 宝箱是否已开启

    protected override void Interact()
    {
        // 检查是否按下右键
        if (Input.GetMouseButtonDown(1)&&(!isOpened)) 
        {
            //不同交互物要做不同的事情
            MakeSomeReaction();
        }
    }

    protected override void MakeSomeReaction()
    {
        PlayAnimation();
        PlayAudio();
        SpawnRandomItem();
        isOpened = true;
    }
    
    private void SpawnRandomItem()
    {
        // 从指定文件夹加载所有道具预制体
        GameObject[] itemPrefabs = Resources.LoadAll<GameObject>(itemsFolderPath);

        if (itemPrefabs.Length > 0)
        {
            // 随机选择一个道具
            //int randomIndex = Random.Range(0, itemPrefabs.Length);
            //GameObject selectedItemPrefab = itemPrefabs[randomIndex];
            GameObject selectedItemPrefab = itemPrefabs[9];

            // 在宝箱的位置生成道具
            Instantiate(selectedItemPrefab, transform.position, Quaternion.identity);
            Debug.Log("生成道具: " + selectedItemPrefab.name);
        }
        else
        {
            Debug.LogWarning("未找到道具预制体，请检查路径: " + itemsFolderPath);
        }
    }
}
