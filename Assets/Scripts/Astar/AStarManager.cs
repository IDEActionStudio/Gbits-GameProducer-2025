using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager
{
    private static AStarManager instance;

    public static AStarManager Instance
    {
        get
        {
            if(instance == null)
                instance = new AStarManager();
            return instance;
        }
    }

    private int mapW;
    private int mapH;
    private AStarNode[,] nodes;
    private List<AStarNode> openList = new List<AStarNode>();
    private List<AStarNode> closedList = new List<AStarNode>();

    //初始化地图信息
    public void InitMapInfo(int mapW, int mapH)
    {
        //根据宽高 创建格子 阻挡的问题 可以随机阻挡
    }

    //寻路方法
    public List<AStarNode> FindPath(Vector3 startPos, Vector3 endPos)
    {
        //先判断传入的两个点是否合法
        //首先要在地图范围内
        //要不是阻挡
        //如果不合法则返回null，意味着不能寻路
        //应该得到起点和终点对应的格子
        //从起点开始找周围的点并放入开启列表中
        //判断这些点是否是边界、阻挡、是否已经在开启或关闭列表中，如果都不是，才放入开启列表
        //选出开启列表中，寻路消耗最小的点
        //放日关闭列表中，然后再从开启列表中移除
        //如果这个点已经是终点，则得到最终结果返回出去
        //如果这个点不是终点，那么继续寻路
        return null;
    }
}
