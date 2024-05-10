using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包含：
/// 1.移动
/// 2.开火
/// 3.摧毁本对象
/// 4.与玩家发生碰撞
/// </summary>
public class Enemy : MonoBehaviour
{
    //移动速度
    public int speed { get; private set; }

    //拿到子弹预制体
    public GameObject Bullet;
    //开火冷却
    public float fireCold { get; private set; }

    //存储地图最大Y值
    public float mapMaxY { get; private set; }
    //存储地图最小Y值
    public float mapMinY { get; private set; }

    //是否无敌状态
    public bool unHurt { get; private set; }

    //拿到爆炸预制体
    public GameObject EnemyBoom;

    //拿到Buff预制体
    public GameObject[] BuffList;
    //出现的Buff数量
    public int buffNum { get; private set; }

    //分数
    public int score { get; private set; }

    private void Awake()
    {
        SetSpeed(8);
        SetFireCold(1.2f);
        mapMaxY = FindObjectOfType<Utils>().getMapMaxY();
        mapMinY = FindObjectOfType<Utils>().getMapMinY();
        unHurt = true;
        score = 1;
        buffNum = 1;
    }

    //敌机地图限制
    private void FixedUpdate()
    {
        EnemyMove();
        FireCold();
        //敌方超出地图范围也将其摧毁，但是不给算分
        if (transform.position.y < mapMinY)
        {
            Destroy(this.gameObject);
        }
    }

    //移动
    private void EnemyMove()
    {
        transform.Translate(-transform.up*speed*Time.deltaTime);
    }

    //发射子弹
    private void EnemyFire()
    {
        Instantiate(Bullet, transform.position, transform.rotation);
    }

    //子弹冷却器，冷却时间到才能发射子弹
    private void FireCold()
    {
        if (fireCold >= 1.2f)
        {
            EnemyFire();
            fireCold = 0;
        }
        else
        {
            fireCold += Time.deltaTime;
        }
    }

    //设定移动速度
    private void SetSpeed(int speed)
    {
        this.speed = speed;
    }

    //设定开火冷却
    private void SetFireCold(float firecold)
    {
        this.fireCold = firecold;
    }

    //被击中摧毁
    private void EnemyDestroy()
    {
        //敌方没有无敌状态才能将其摧毁
        if (!isUnHurt())
        {
            FindObjectOfType<PlayingManager>().ScoreRise(score);//加分
            Instantiate(EnemyBoom, transform.position, transform.rotation);//爆炸动画
            //掉落Buff
            BuffDrop();
            Destroy(this.gameObject);//摧毁自己
        }
    }

    //敌方是否无敌
    private bool isUnHurt()
    {
        //还未到达指定高度时
        if (transform.position.y > mapMaxY - 0.3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //与玩家发生碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            //玩家受击，摧毁敌人对象
            collision.collider.SendMessage("GotShot");
            EnemyDestroy();
        }
    }

    //掉落Buff
    private void BuffDrop()
    {
        if (!FindObjectOfType<PlayingManager>().isBossSpawn)
        {
            if (Random.Range(1, 3) == 1 || (this.score / this.buffNum > 30))//将近三分之一的概率,或者分数和buff数比例超过30 : 1(倒霉蛋)
            {
                buffNum += 1;
                //生命值不满的情况下只会掉落回血Buff
                if (FindObjectOfType<PlayerHP>().playerHP != 3)
                {
                    Instantiate(BuffList[2], transform.position, transform.rotation);
                }
                //随机生成三种道具
                else
                {
                    Instantiate(BuffList[Random.Range(0, 2)], transform.position, transform.rotation);
                }
            }
        }
        else
        {
            return;
        }
    }
}
