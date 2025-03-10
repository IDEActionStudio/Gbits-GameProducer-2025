using UnityEngine;
using System.Collections;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] cloudSprites; // 云模型的预制体
    public float spawnInterval; // 生成云的间隔时间
    public float minX;
    public float maxX;
    public float minY ; // 云生成的Y轴最小值
    public float maxY ; // 云生成的Y轴最大值
    public float minZ  ;
    public float maxZ ;
    public float minScale;
    public float maxScale;
    public float moveSpeed ; // 云移动的速度
    public int initialCloudCount;
    private Quaternion rotation = Quaternion.identity;

    void Start()
    {
        for (int i = 0; i < initialCloudCount; i++)
        {
            SpawnCloudsInitial();
        }
        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {
        while (true)
        {
            // 随机生成Y轴位置
            float yPos = Random.Range(minY, maxY);
            float zPos = Random.Range(minZ, maxZ);
            Vector3 spawnPosition = new Vector3(0, yPos, zPos);

            // 实例化云模型
            GameObject cloud = Instantiate(cloudSprites[Random.Range(0,cloudSprites.Length)], spawnPosition, rotation);
            // 随机设置云的大小
            float randomScale = Random.Range(minScale, maxScale);
            cloud.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            // 启动云的移动
            StartCoroutine(MoveCloud(cloud));

            // 等待一段时间后再次生成云
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator MoveCloud(GameObject cloud)
    {
        while (cloud != null)
        {
            // 云沿X轴移动
            cloud.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            // 如果云的X坐标超过250，销毁云
            if (cloud.transform.position.x > 250)
            {
                Destroy(cloud);
            }

            yield return null;
        }
    }
    private void SpawnCloudsInitial()
    {
            // 随机生成Y轴位置
            float xPos = Random.Range(minX, maxX);
            float yPos = Random.Range(minY, maxY);
            float zPos = Random.Range(minZ, maxZ);
            Vector3 spawnPosition = new Vector3(xPos, yPos, zPos);

            // 实例化云模型
            GameObject cloud = Instantiate(cloudSprites[Random.Range(0,cloudSprites.Length)], spawnPosition, rotation);

            // 随机设置云的大小
            float randomScale = Random.Range(minScale, maxScale);
            cloud.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            
            // 启动云的移动
            StartCoroutine(MoveCloud(cloud));
    }
}