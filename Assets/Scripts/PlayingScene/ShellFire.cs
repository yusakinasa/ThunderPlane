using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包含：
/// 1.生成随机横坐标并生成炮弹
/// 2.加速生成
/// 3.生成Boss
/// </summary>
public class ShellFire : MonoBehaviour
{
    //炮弹预制体
    public GameObject Shell;
    //炮弹发射冷却
    public float fireCold { get; private set; }
    //炮弹发射冷却计时器
    public float fireColdCount { get; private set; }

    //存储地图最小X值
    public float mapMinX { get; private set; }
    //存储地图最大X值
    public float mapMaxX { get; private set; }

    //随机横向坐标
    private Vector3 randomX;

    private void Awake()
    {
        //炮弹发射冷却时间
        fireCold = 0.9f;
        fireColdCount = 0.9f;
        //拿到地图横向的最大最小值
        mapMinX = FindObjectOfType<Utils>().getMapMinX();
        mapMaxX = FindObjectOfType<Utils>().getMapMaxX();
    }

    private void FixedUpdate()
    {
        ShellFireCold();
    }

    //发射冷却器
    private void ShellFireCold()
    {
        if (fireColdCount >= fireCold)
        {
            //每次都有三分之一的概率不发射
            if (Random.Range(1,3) > 1)
            {
                Instantiate(Shell, new Vector3(GetRandomX().x, transform.position.y, transform.position.z), transform.rotation);
                fireColdCount = 0;
            }
        }
        else
        {
            fireColdCount += Time.deltaTime;
        }
    }

    //获得一个随机的横向坐标
    private Vector3 GetRandomX()
    {
        randomX.x = Random.Range(mapMinX + 0.3f, mapMaxX - 0.3f);
        return randomX;
    }
}
