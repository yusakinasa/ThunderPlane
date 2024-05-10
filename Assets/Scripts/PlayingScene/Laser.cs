using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//激光
public class Laser : MonoBehaviour
{
    public SpriteRenderer spr;
    public Sprite[] Lasers;

    public int laserCount { get; private set; }
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        this.laserCount = FindObjectOfType<BossHP>().laserCount;
        ChangeSprite(laserCount);
        Invoke(nameof(LaserDestroy), 3.0f);
    }

    //激光摧毁
    public void LaserDestroy()
    {
        FindObjectOfType<Boss>().IsMove(true);
        Destroy(this.gameObject);
    }

    //改变精灵
    public void ChangeSprite(int num)
    {
        spr.sprite = Lasers[num - 1];
    }

    //激光命中
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.SendMessage("GotShot");
        }
    }
}
