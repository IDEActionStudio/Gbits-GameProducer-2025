using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Node_Type
{
    Walk,//能走的地方
    Stop,//不能走的阻挡
}

public class AStarNode
{
    //格子对象坐标
    public int x;
    public int y;
    //寻路消耗
    public float f;
    //离起点的距离
    public float g;
    //离终点的距离
    public float h;
    //父对象
    public AStarNode father;
    
    public E_Node_Type type;

    public AStarNode(int x,int y,E_Node_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}
