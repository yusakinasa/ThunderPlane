using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pointer : MonoBehaviour
{
    //物体跟随鼠标指针
 
    //获取到鼠标的屏幕坐标
    Vector3 mousePositionOnScreen;
    //将物体从世界坐标转换为的屏幕坐标
    Vector3 screenPosition;
    //将鼠标的屏幕坐标转换为的世界坐标
    Vector3 mousePositionInWorld;

    //存储地图最大Y值
    public float mapMaxY { get; private set; }
    //存储地图最小Y值
    public float mapMinY { get; private set; }    
    //存储地图最大X值
    public float mapMaxX { get; private set; }
    //存储地图最小X值
    public float mapMinX { get; private set; }

    private void Awake()
    {
        //隐藏鼠标本身光标
        Cursor.visible = false;

        if (SceneManager.GetActiveScene().name == "Playing")
        {
            //拿到地图的最大最小值
            mapMaxY = FindObjectOfType<Utils>().getMapMaxY();
            mapMinY = FindObjectOfType<Utils>().getMapMinY();
            mapMaxX = FindObjectOfType<Utils>().getMapMaxX();
            mapMinX = FindObjectOfType<Utils>().getMapMinX();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Playing")
        {
            //调用自制指针跟随鼠标指针方法
            CrossFollowMouse();
        }
    }

    //自制指针跟随鼠标指针方法
    private void CrossFollowMouse()
    {
        //获取鼠标在场景中坐标
        mousePositionOnScreen = Input.mousePosition;

        //获取物体在世界坐标中的位置，并转换为屏幕坐标；
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        //让鼠标坐标的Z轴坐标 等于 场景中物体的Z轴坐标
        mousePositionOnScreen.z = screenPosition.z;
        //将鼠标的屏幕坐标转化为世界坐标
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);

        //如果鼠标超出地图边界，就把最大值给到他，使得玩家不能超出边界
        if (mousePositionInWorld.x > mapMaxX)
        {
            mousePositionInWorld.x = mapMaxX;
        }
        if (mousePositionInWorld.x < mapMinX)
        {
            mousePositionInWorld.x = mapMinX;
        }
        if (mousePositionInWorld.y > mapMaxY)
        {
            mousePositionInWorld.y = mapMaxY;
        }
        if (mousePositionInWorld.y < mapMinY)
        {
            mousePositionInWorld.y = mapMinY;
        }

        //将物体的坐标改为鼠标的世界坐标，物体跟随鼠标移动
        transform.position = mousePositionInWorld;

    }

}
