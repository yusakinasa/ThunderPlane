using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 工具类：
/// 1.获得地图最大最小的x、y值
/// </summary>
public class Utils : MonoBehaviour
{
    //地图边界，分别装有地图 右上角 和 左下角 的坐标
    public GameObject[] mapScope;

    //获取地图最大Y值
    public float getMapMaxY()
    {
        return mapScope[0].transform.position.y;
    }

    //获取地图最小Y值
    public float getMapMinY()
    {
        return mapScope[1].transform.position.y;
    }

    //获取地图最大X值
    public float getMapMaxX()
    {
        return mapScope[0].transform.position.x;
    }

    //获取地图最小X值
    public float getMapMinX()
    {
        return mapScope[1].transform.position.x;
    }
}
