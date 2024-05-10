using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 包含：
/// 1.受到攻击
/// 2.血条不同阶段变色
/// 3.通过血量判断游戏是否需要结束
/// </summary>
public class BossHP : MonoBehaviour
{
    //血量
    public int bossHP { get; private set; }

    public Slider bossHPBar;

    public TextMeshProUGUI bossHPNum;

    private Image image;

    //玩家Buff情况
    //火力等级
    public int playerFirePowerLevel { get; private set; }
    //射速
    public float playerFireCold { get; private set; }

    //发射激光计数器
    public int laserCount { get; private set; }

    private void Awake()
    {

        //获取玩家当前Buff情况
        this.playerFirePowerLevel = FindObjectOfType<Player>().firePowerLevel;
        this.playerFireCold = FindObjectOfType<Player>().fireCold;

        SetBossHP();

        bossHPBar.maxValue = bossHP;
        bossHPBar.value = bossHP;

        //获取Slider内部Fill子对象的Image组件
        image = GameObject.Find("Fill").GetComponent<Image>();

        laserCount = 0;
    }

    //当前血量
    private void Update()
    {
        bossHPBar.value = bossHP;
        bossHPNum.text = bossHP.ToString();

        SetSliderColor();
        if (bossHP <= 0)
        {
            this.gameObject.SetActive(false);
            BossDead();
        }
    }

    public void GotShot()
    {
        bossHP--;
    }

    //修改进度条颜色
    private void SetSliderColor()
    {

        //计算Slider的比例（值/最大值）
        float progress = bossHPBar.value / bossHPBar.maxValue;

        if (progress < 0.66 && progress > 0.33)
        {
            //如果比例在 0.33 到 0.66 之间，则将Slider的颜色设置为黄色（3个参数分别为红绿黄）
            Color yellow = new Color(1f, 1f, 0f);
            image.color = yellow;
            if(laserCount == 0)
            {
                Lasing();
            }
        }
        else if (progress < 0.33 && progress > 0)
        {
            //如果比例在 0 到 0.33 之间，则将Slider的颜色设置为红色
            Color red = new Color(1f, 0f, 0f);
            image.color = red;
            if (laserCount == 1)
            {
                Lasing();
            }
        }
    }

    //设置Boss血量
    public void SetBossHP()
    {
        //根据玩家的射速和火力两种buff来确定Boss的血量，确保不管玩家是什么状态，Boss都可以存活60秒以上
        bossHP = Mathf.FloorToInt(playerFirePowerLevel * 60f / playerFireCold);
    }

    //通过Boss生命值判断游戏是否需要结束
    private void BossDead()
    {
        FindObjectOfType<PlayingManager>().GameOver(true);
    }

    //判断发射激光
    public void Lasing()
    {
        laserCount += 1;
        FindObjectOfType<Boss>().IsMove(false);
        FindObjectOfType<Boss>().Lasing();
    }
}
