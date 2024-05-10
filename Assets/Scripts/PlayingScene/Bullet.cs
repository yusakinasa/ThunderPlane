using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包含：
/// 1.移动
/// 2.击中物体
/// 3.摧毁本对象
/// </summary>
public class Bullet : MonoBehaviour
{
    //速度
    public int speed { get; private set; }

    //存储地图最大Y值
    public float mapMaxY { get; private set; }
    //存储地图最小Y值
    public float mapMinY { get; private set; }

    //是否是玩家射出的子弹
    public bool isPlayerBullet;

    private void Start()
    {
        SetSpeed(10);
        //拿到地图的最大和最小Y值
        mapMaxY = FindObjectOfType<Utils>().getMapMaxY();
        mapMinY = FindObjectOfType<Utils>().getMapMinY();
    }

    private void FixedUpdate()
    {
        BulletMove();
        //如果子弹坐标超过最大或者最小Y值±修正值，即超出地图外，执行摧毁
        if (transform.position.y > mapMaxY + 0.5 || transform.position.y < mapMinY - 0.5)
        {
            BulletDestroy();
        }
    }

    //子弹移动
    private void BulletMove()
    {
        //玩家子弹向上移动
        if(isPlayerBullet)
        {
            transform.Translate(transform.up * speed * Time.deltaTime);
        }
        //敌方子弹向下移动
        else
        {
            transform.Translate(-transform.up * speed * Time.deltaTime);
        }
    }

    //子弹击中物体
    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "Player":
                //敌方子弹
                if (!isPlayerBullet)
                {
                    collider.SendMessage("GotShot");
                    BulletDestroy();
                }
                break;
            case "Enemy":
                //玩家子弹
                if (isPlayerBullet)
                {
                    collider.SendMessage("EnemyDestroy");
                    BulletDestroy();
                }
                break;
            case "Boss":
                //玩家子弹
                if (isPlayerBullet)
                {
                    collider.SendMessage("GotShot");
                    BulletDestroy();
                }
                break;
        }

    }

    //摧毁子弹
    private void BulletDestroy()
    {
        Destroy(this.gameObject);
    }

    private void SetSpeed(int speed)
    {
        this.speed = speed;
    }
}
