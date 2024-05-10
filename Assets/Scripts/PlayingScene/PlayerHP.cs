using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包含：
/// 1.受到攻击
/// 2.通过血量判断游戏是否需要结束
/// </summary>
public class PlayerHP : MonoBehaviour
{
    //玩家血量
    public int playerHP = 3;
    //玩家最高血量
    private int playerHPMax = 3;

    //当前精灵
    public SpriteRenderer spr;
    //血条精灵数组
    public Sprite[] HPBar;

    //音源组件
    public AudioSource audioSource;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        ResetState();

        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.loop = false;
    }

    //重置状态
    public void ResetState()
    {
        SetPlayerHP(3);
    }

    private void Update()
    {
        UpdateHPBar();
        if (playerHP == 0)
        {
            PlayerDead();
        }
    }

    //设置玩家血量
    public void SetPlayerHP(int HP)
    {
        this.playerHP = HP;
    }

    //生命恢复
    public void PlayerHPRec()
    {
        SetPlayerHP(this.playerHP + 1);
        audioSource.Play();
    }

    //通过玩家生命值更新血条精灵
    private void UpdateHPBar()
    {
        if (spr != null && playerHP >= 0 && playerHP <= playerHPMax)
        {
            switch (playerHP)
            {
                case 0:
                    spr.sprite = HPBar[0];
                    break;
                case 1:
                    spr.sprite = HPBar[1];
                    break;
                case 2:
                    spr.sprite = HPBar[2];
                    break;
                case 3:
                    spr.sprite = HPBar[3];
                    break;
            }
        }
    }

    //被击中
    public void GotShot()
    {
        SetPlayerHP(playerHP-1);
    }

    //通过玩家生命值判断游戏是否需要结束
    private void PlayerDead()
    {
        FindObjectOfType<PlayingManager>().GameOver(false);
    }
}
