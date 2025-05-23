using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
public class CityGenerator : MonoBehaviour
{
    public GameObject straightRoadPrefab; // 直路
    public GameObject cornerRoadPrefab;   // 拐角
    public GameObject tJunctionRoadPrefab; // 三向路口
    public GameObject crossRoadPrefab;    // 十字路口
    
    public GameObject[] buildingPrefabs;  // 楼房模型
    private string buildingFolderPath = "Prefabs/Buildings"; // 道具预制体的路径
    public GameObject[] carPrefabs;       // 汽车模型
    private string carFolderPath = "Prefabs/Vehicles"; // 道具预制体的路径
    public GameObject[] propPrefabs;         // 小物件模型
    private string PropsFolderPath = "Prefabs/Props"; // 道具预制体的路径
    public GameObject[] weedPrefabs;//杂草模型
    private string weedFolderPath = "Prefabs/Grass"; // 道具预制体的路径
    public GameObject[] flowerPrefabs;//花朵模型
    private string flowersFolderPath = "Prefabs/Flowers";
    public GameObject[] crackSprites;
    
    public float buildingSpacing = 25f;   // 楼房间隔
    public int carsPerRoad = 2;           // 每条道路上生成的汽车数量
    public int crackPerRoad = 1;           // 每条道路上生成的汽车数量
    public float propDensity = 1f;      // 小物件生成密度（0 到 1）
    public float weedDensity = 1f;//杂草生成密度
    public float weedPerBlock = 40f;
    public float flowerDensity = 1f; //花朵生成密度
    public float flowerPerBlock = 10f;
    public float minScale;
    public float maxScale;
    public float MapSize;

    public List<GameObject> roads = new List<GameObject>(); // 存储生成的道路
    public List<GameObject> cars = new List<GameObject>();  // 存储生成的汽车
    public List<GameObject> buildings = new List<GameObject>();
    
    private NavMeshSurface surface;
    private LayerMask collisionMask;

    public int[,] roadGrid = new int[20, 20]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    };

    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
        Cursor.visible = false;
        buildingPrefabs = Resources.LoadAll<GameObject>(buildingFolderPath);
        carPrefabs = Resources.LoadAll<GameObject>(carFolderPath);
        propPrefabs = Resources.LoadAll<GameObject>(PropsFolderPath);
        weedPrefabs = Resources.LoadAll<GameObject>(weedFolderPath);
        flowerPrefabs=Resources.LoadAll<GameObject>(flowersFolderPath);
        GenerateRoad(roadGrid);
        GenerateBuildings(roadGrid, roads.ToArray());
        GenerateCars(roadGrid);
        GenerateProps(roadGrid);
        GenerateNavMesh();
        GenerateWeeds(roadGrid);
        GenerateFlowers(roadGrid);
        GenerateCrack(roadGrid);
        //enerateEnemyPoint(roadGrid);
    }

    void GenerateRoad(int[,] roadGrid)
    {
        int width = roadGrid.GetLength(0);
        int height = roadGrid.GetLength(1);

        Vector3 roadSize = GetModelSize(straightRoadPrefab); // 获取道路模型的尺寸
        float roadWidth = roadSize.x; // 道路的宽度
        float roadLength = roadSize.z; // 道路的长度

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (roadGrid[x, y] == 1) // 如果是道路
                {
                    Vector3 position = new Vector3(x * roadWidth, 0, y * roadLength);
                    Quaternion rotation = Quaternion.identity;

                    // 检查相邻格子
                    bool hasRoadAbove = y < height - 1 && roadGrid[x, y + 1] == 1;
                    bool hasRoadBelow = y > 0 && roadGrid[x, y - 1] == 1;
                    bool hasRoadLeft = x > 0 && roadGrid[x - 1, y] == 1;
                    bool hasRoadRight = x < width - 1 && roadGrid[x + 1, y] == 1;

                    // 计算相邻道路数量
                    int roadCount = (hasRoadAbove ? 1 : 0) +
                                    (hasRoadBelow ? 1 : 0) +
                                    (hasRoadLeft ? 1 : 0) +
                                    (hasRoadRight ? 1 : 0);

                    // 根据相邻道路数量选择模型
                    GameObject roadPrefab = straightRoadPrefab;
                    if (roadCount == 2) // 双向拐角或直路
                    {
                        if ((hasRoadAbove && hasRoadBelow) || (hasRoadLeft && hasRoadRight))
                        {
                            roadPrefab = straightRoadPrefab; // 直路
                            if (hasRoadAbove && hasRoadBelow)
                                rotation = Quaternion.identity; // 水平道路
                            else if (hasRoadLeft && hasRoadRight)
                                rotation = Quaternion.Euler(0, 90, 0); // 垂直道路
                        }
                        else
                        {
                            roadPrefab = cornerRoadPrefab; // 拐角
                            if (hasRoadAbove && hasRoadRight)
                                rotation = Quaternion.Euler(0, 90, 0); // 右上拐角
                            else if (hasRoadRight && hasRoadBelow)
                                rotation = Quaternion.Euler(0, 180, 0); // 右下拐角
                            else if (hasRoadBelow && hasRoadLeft)
                                rotation = Quaternion.Euler(0, -90, 0); // 左下拐角
                            else if (hasRoadLeft && hasRoadAbove)
                                rotation = Quaternion.Euler(0, 0, 0); // 左上拐角
                        }
                    }
                    else if (roadCount == 3) // 三向路口
                    {
                        roadPrefab = tJunctionRoadPrefab;
                        if (!hasRoadBelow)
                            rotation = Quaternion.Euler(0, 0, 0); // 上、左、右
                        else if (!hasRoadLeft)
                            rotation = Quaternion.Euler(0, 90, 0); // 上、右、下
                        else if (!hasRoadAbove)
                            rotation = Quaternion.Euler(0, 180, 0); // 右、下、左
                        else if (!hasRoadRight)
                            rotation = Quaternion.Euler(0, 270, 0); // 下、左、上
                    }
                    else if (roadCount == 4) // 十字路口
                    {
                        roadPrefab = crossRoadPrefab;
                    }

                    GameObject road = Instantiate(roadPrefab, position, rotation);
                    road.isStatic = true;
                    road.name = x.ToString()+","+y.ToString();
                    roads.Add(road); // 将生成的道路添加到列表中
                }
            }
        }
    }
    void GenerateBuildings(int[,] roadGrid, GameObject[] roads)
    {
        int width = roadGrid.GetLength(0);
        int height = roadGrid.GetLength(1);

        Vector3 buildingSize = GetModelSize(buildingPrefabs[0]); // 获取楼房模型的尺寸
        Vector3 roadSize = GetModelSize(straightRoadPrefab); // 获取道路模型的尺寸

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (roadGrid[x, y] == 0) // 如果是可建区域
                {
                    Vector3 position = new Vector3(x * buildingSpacing, 0, y * buildingSpacing);

                    // 检查是否与道路重叠
                    if (!IsOverlapping(position, buildingSize, roads, roadSize))
                    {
                        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                        GameObject building=Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Length)], position, rotation);
                        building.isStatic = true;
                        buildings.Add(building);
                    }
                }
            }
        }
    }
    
   void GenerateCars(int[,] roadGrid)
    {
        int width = roadGrid.GetLength(0);
        int height = roadGrid.GetLength(1);

        Vector3 roadSize = GetModelSize(straightRoadPrefab); // 获取道路模型的尺寸
        float roadWidth = roadSize.x; // 道路的宽度
        float roadLength = roadSize.z; // 道路的长度
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (roadGrid[x, y] == 1) // 如果是道路
                {
                    if (x != 9 || y != 9)
                    {
                        // 在每条道路上生成指定数量的汽车
                        for (int i = 0; i < carsPerRoad; i++)
                        {
                            // 随机选择一个汽车模型
                            GameObject carPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

                            // 生成汽车的位置和朝向
                            Vector3 carPosition;
                            Quaternion carRotation;
                            bool isColliding;
                            int attempts = 0; // 防止无限循环

                            do
                            {
                                // 随机生成汽车的位置
                                carPosition = new Vector3(
                                    x * roadWidth + Random.Range(-roadWidth / 2f, roadWidth / 2f),
                                    0,
                                    y * roadLength + Random.Range(-roadLength / 2f, roadLength / 2f)
                                );

                                // 随机生成汽车的朝向
                                carRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                                // 检测汽车是否与其他汽车碰撞
                                Vector3 carSize = GetModelSize(carPrefab);
                                isColliding = false;
                                foreach (var car in cars)
                                {
                                    if (Vector3.Distance(carPosition, car.transform.position) < carSize.magnitude)
                                    {
                                        isColliding = true;
                                        break;
                                    }
                                }

                                attempts++;
                                if (attempts > 10) // 防止无限循环
                                {
                                    break;
                                }
                            } while (isColliding);

                            // 生成汽车
                            if (!isColliding)
                            {
                                GameObject car = Instantiate(carPrefab, carPosition, carRotation);
                                car.isStatic = true;
                                cars.Add(car); // 将生成的汽车添加到列表中
                            }
                        }
                    }
                }
            }
        }
    }

    public Vector3 GetModelSize(GameObject model)
    {
        Renderer renderer = model.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.size;
        }
        else
        {
            Debug.LogWarning("模型没有 Renderer 组件");
            return Vector3.zero;
        }
    }

    bool IsOverlapping(Vector3 position, Vector3 buildingSize, GameObject[] roads, Vector3 roadSize)
    {
        foreach (var road in roads)
        {
            Vector3 roadPosition = road.transform.position;
            Vector3 roadBounds = roadSize / 2; // 道路的半个尺寸

            // 检查楼房是否与道路重叠
            if (Mathf.Abs(position.x - roadPosition.x) < (buildingSize.x + roadBounds.x) &&
                Mathf.Abs(position.z - roadPosition.z) < (buildingSize.z + roadBounds.z))
            {
                return true; // 重叠
            }
        }
        return false; // 不重叠
    }
    
    
    
    
    void GenerateProps(int[,] roadGrid)
    {
        int width = roadGrid.GetLength(0);
        int height = roadGrid.GetLength(1);
        
        Vector3 roadSize = GetModelSize(straightRoadPrefab); // 获取道路模型的尺寸
        
        float roadWidth = roadSize.x; // 道路的宽度
        float roadLength = roadSize.z; // 道路的长度

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (roadGrid[x, y] == 0) // 如果是可建区域
                {
                    // 根据杂草生成密度随机生成杂草
                    if (Random.value < propDensity)
                    {
                        // 随机选择一个杂草预制体
                        GameObject propPrefab = propPrefabs[Random.Range(0, propPrefabs.Length)];

                        // 随机生成杂草的位置
                        Vector3 propPosition = new Vector3(
                            x * roadWidth + Random.Range(-roadWidth / 2f, roadWidth / 2f),
                            0,
                            y * roadLength + Random.Range(-roadLength / 2f, roadLength / 2f)
                        );

                        // 随机生成杂草的朝向
                        Quaternion propRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                        // 生成杂草
                        GameObject prop = Instantiate(propPrefab, propPosition, propRotation);
                        prop.isStatic = true;
                    }
                }
            }
        }
    }

    void GenerateWeeds(int[,] roadGrid)
    {
        int width = roadGrid.GetLength(0);
        int height = roadGrid.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (roadGrid[x, y] == 0|| roadGrid[x, y] == 1)
                {
                    for (int i = 0; i < weedPerBlock; i++)
                    {
                        // 根据杂草生成密度随机生成杂草
                        if (Random.value < weedDensity)
                        {
                            // 随机选择一个杂草预制体
                            GameObject weedPrefab = weedPrefabs[Random.Range(0, weedPrefabs.Length)];

                            // 随机生成杂草的位置
                            Vector3 weedPosition = new Vector3(
                                Random.Range(0, MapSize),
                                0,
                                Random.Range(0, MapSize)
                            );

                            // 随机生成杂草的朝向
                            Quaternion weedRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                            // 生成杂草
                            GameObject weed = Instantiate(weedPrefab, weedPosition, weedRotation);
                            float randomScale = Random.Range(minScale, maxScale);
                            weed.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                        }
                    }
                }
            }
        }
    }
    
    void GenerateFlowers(int[,] roadGrid)
    {
        int width = roadGrid.GetLength(0);
        int height = roadGrid.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (roadGrid[x, y] == 0|| roadGrid[x, y] == 1)
                {
                    for (int i = 0; i < flowerPerBlock; i++)
                    {
                        // 根据杂草生成密度随机生成杂草
                        if (Random.value < flowerDensity)
                        {
                            // 随机选择一个杂草预制体
                            GameObject flowerPrefab = flowerPrefabs[Random.Range(0, flowerPrefabs.Length)];

                            // 随机生成杂草的位置
                            Vector3 flowerPosition = new Vector3(
                                Random.Range(0, MapSize),
                                0,
                                Random.Range(0, MapSize)
                            );

                            // 随机生成杂草的朝向
                            Quaternion flowerRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                            // 生成杂草
                            GameObject flower = Instantiate(flowerPrefab, flowerPosition, flowerRotation);
                            float randomScale = Random.Range(minScale, maxScale);
                            flower.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                        }
                    }
                }
            }
        }
    }

    void GenerateCrack(int[,] roadGrid)
    {
        int width = roadGrid.GetLength(0);
        int height = roadGrid.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (roadGrid[x, y] == 1)
                {
                    for (int i = 0; i < crackPerRoad; i++)
                    {
                            // 随机选择一个杂草预制体
                            GameObject crackSprite = crackSprites[Random.Range(0, crackSprites.Length)];

                            // 随机生成杂草的位置
                            Vector3 crackPosition = new Vector3(
                                Random.Range(0, MapSize),
                                0.05f,
                                Random.Range(0, MapSize)
                            );

                            // 随机生成杂草的朝向
                            Quaternion crackRotation = Quaternion.Euler(90, Random.Range(0, 360), 0);
                            // 生成杂草
                            GameObject crack = Instantiate(crackSprite, crackPosition, crackRotation);
                            crack.transform.localScale = new Vector3(Random.Range(2f, 3f), Random.Range(2f, 3f), Random.Range(2f, 3f));
                    }
                }
            }
        }
    }
    
    public void GenerateNavMesh()
    {
        if (surface != null)
        {
            surface.BuildNavMesh(); // 动态烘焙NavMesh
        }
    }
    
    /*public List<GameObject> enemyPoints = new List<GameObject>();
    void GenerateEnemyPoint(int[,] roadGrid)
    {
        int width = roadGrid.GetLength(0);
        int height = roadGrid.GetLength(1);
        
        Vector3 roadSize = GetModelSize(straightRoadPrefab); // 获取道路模型的尺寸
        
        float roadWidth = roadSize.x; // 道路的宽度
        float roadLength = roadSize.z; // 道路的长度

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                    Vector3 enemyPointPosition = new Vector3(
                        x * roadWidth + Random.Range(-roadWidth / 2f, roadWidth / 2f),
                        0,
                        y * roadLength + Random.Range(-roadLength / 2f, roadLength / 2f));

                    // 检测该点是否与模型发生碰撞
                    if (!Physics.CheckSphere(enemyPointPosition, 0.1f, collisionMask))
                    {
                        // 如果没有碰撞，生成一个可视化对象来表示导航点
                        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        GameObject enemyPoint = Instantiate(obj, enemyPointPosition, Quaternion.identity);
                        enemyPoint.name = x + "_" + y + "_";
                        enemyPoint.transform.position = enemyPointPosition;
                        enemyPoints.Add(enemyPoint);
                }
            }
        }
    }*/
}