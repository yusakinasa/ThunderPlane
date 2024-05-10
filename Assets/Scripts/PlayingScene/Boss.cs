using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包含：
/// 1.移动
/// 2.开火
/// 3.被击中时调用BossHP类
/// 4.摧毁本对象
/// </summary>
public class Boss : MonoBehaviour
{
    //移动速度
    public float speed { get; private set; }
    //身处高度
    public float siteY { get; private set; }
    //是否移动
    public bool isMove { get; private set; }

    //左右移动方向值
    public int right { get; private set; }
    //是否无敌状态
    public bool unHurt { get; private set; }

    //拿到子弹预制体
    public GameObject bullet;
    //开火冷却
    public float fireCold { get; private set; }

    //拿到爆炸预制体
    public GameObject BossBoom;

    //拿到激光预制体
    public GameObject Laser;

    //分数
    public int score { get; private set; }

    private void Awake()
    {
        speed = 2;
        siteY = 7.0f;
        IsMove(true);
        right = 1;
        score = 100;
        unHurt = true;//开局时Boss尚未就位，血条不出现，为无敌状态
    }

    //初始化
    private void Update()
    {
        if (isMove)
        {
            BossMove();
        }
        FireCold();
    }

    //boss移动
    private void BossMove()
    {
        //还未到达指定高度时先纵向移动
        if (transform.position.y > siteY)
        {
            transform.Translate(-transform.up * speed * Time.deltaTime);
        }
        //到达指定高度后左右横向移动
        else
        {
            unHurt = false;
            FindObjectOfType<PlayingManager>().SetBossHPActive();
            transform.Translate(transform.right * right * speed * Time.deltaTime);
            //到达左右边界后向相反方向移动
            if (Mathf.Abs(transform.position.x) >= 4)
            {
                right = -right;
            }
        }
    }

    //发射子弹
    private void BossFire()
    {
        Instantiate(bullet, new Vector3(transform.position.x - 1.0f, transform.position.y, transform.position.z), transform.rotation);//左炮
        Instantiate(bullet, new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z), transform.rotation);//右炮
    }

    //子弹冷却器，冷却时间到才能发射子弹
    private void FireCold()
    {
        if (fireCold >= 1.2f)
        {
            BossFire();
            fireCold = 0;
        }
        else
        {
            fireCold += Time.deltaTime;
        }
    }

    //Boss被击中
    private void GotShot()
    {
        //如果此时不是无敌状态
        if (!unHurt)
        {
            FindObjectOfType<BossHP>().GotShot();
        }
    }

    //Boss被摧毁
    public void BossDestroy()
    {
        FindObjectOfType<PlayingManager>().ScoreRise(100);
        Instantiate(BossBoom, transform.position, transform.rotation);//爆炸动画
        Destroy(this.gameObject);//摧毁自己
    }

    //设置是否移动
    public void IsMove(bool isMove)
    {
        this.isMove = isMove;
    }

    //发射激光
    public void Lasing()
    {
        Instantiate(Laser, new Vector3(transform.position.x, transform.position.y - 1.1f, transform.position.z), transform.rotation);
    }

    //与玩家发生碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            //玩家受击，Boss受击
            collision.collider.SendMessage("GotShot");
            GotShot();
        }
    }
}
