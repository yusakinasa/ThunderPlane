using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    //速度
    public int speed { get; private set; }

    //存储地图最大Y值
    public float mapMaxY { get; private set; }
    //存储地图最小Y值
    public float mapMinY { get; private set; }

    //当前Buff名
    public string buffName { get; private set; }

    //初始化
    private void Start()
    {
        this.buffName = this.gameObject.tag;
        SetSpeed(4);
        //拿到地图的最大和最小Y值
        mapMaxY = FindObjectOfType<Utils>().getMapMaxY();
        mapMinY = FindObjectOfType<Utils>().getMapMinY();
    }

    //修改更新
    private void FixedUpdate()
    {
        BuffMove();
        //如果Buff坐标超过最大或者最小Y值±修正值，即超出地图外，执行摧毁
        if (transform.position.y > mapMaxY + 0.5 || transform.position.y < mapMinY - 0.5)
        {
            BuffDestroy();
        }
    }

    //Buff移动
    private void BuffMove()
    {
        transform.Translate(-transform.up * speed * Time.deltaTime);
    }

    //摧毁Buff
    private void BuffDestroy()
    {
        Destroy(this.gameObject);
    }

    //设定Buff移动速度
    private void SetSpeed(int speed)
    {
        this.speed = speed;
    }

    //buff类型
    public void BuffIt()
    {
        switch (buffName)
        {
            case "BuffPower":
                FindObjectOfType<Player>().FirePowerUpgrade();
                break;
            case "BuffSpeed":
                FindObjectOfType<Player>().FireSpeedUpgrade();
                break;
            case "BuffHealth":
                FindObjectOfType<PlayerHP>().PlayerHPRec();
                break;
        }
    }

    //buff生效
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            BuffIt();
            BuffDestroy();
        }
    }
}
