using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// 包含：
/// 1.移动
/// 2.开火
/// 3.被击中时调用PlayerHP类
/// 4.被击中后短暂无敌
/// 5.摧毁本对象
/// </summary>
public class Player : MonoBehaviour
{
    //拿到子弹的预制体
    public GameObject bullet;
    //射击冷却
    public float fireCold { get; private set; }
    //射击冷却计时器
    public float fireColdCount { get; private set; }

    //当前精灵渲染器
    public SpriteRenderer playerSpr { get; private set; }
    //无敌精灵渲染器
    public SpriteRenderer unHurtSpr { get; private set; }

    //是否无敌状态
    public bool unHurt { get; private set; }

    //火力等级
    public int firePowerLevel { get; private set; }
    //射速等级
    public int fireSpeedLevel { get; private set; }

    //音源组件
    private AudioSource audioSource;
    //音效数组 0 Buffed ; 1 UnHurt
    public AudioClip[] audioClips;

    //拿到爆炸预制体
    public GameObject PlayerBoom;

    private void Awake()
    {
        ResetState();
        unHurt = false;
        playerSpr = GetComponent<SpriteRenderer>();
        unHurtSpr = GameObject.Find("UnHurt").GetComponent<SpriteRenderer>();
        //获取音源组件
        audioSource = this.gameObject.GetComponent<AudioSource>();
        //先不播放
        audioSource.Stop();
        //循环
        audioSource.loop = true;
    }

    public void ResetState()
    {
        firePowerLevel = 1;
        fireSpeedLevel = 1;
        fireColdCount = 0f;
        SetFireCold(0.3f);
    }

    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name == "Playing")
        {
            //通过冷却器发射子弹
            FireCold();
        }

        UnHurtAnim();
    }

    //发射子弹
    private void PlayerFire()
    {
        //实例化子弹，参数分别为：实例化的游戏对象，对象位置，对象角度
        if (this.firePowerLevel == 1)
        {
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);//中炮
        }
        else if (this.firePowerLevel == 2)
        {
            Instantiate(bullet, new Vector3(transform.position.x - 0.3f, transform.position.y, transform.position.z), transform.rotation);//左炮
            Instantiate(bullet, new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z), transform.rotation);//右炮
        }
        else if (this.firePowerLevel == 3)
        {
            Instantiate(bullet, new Vector3(transform.position.x - 0.5f, transform.position.y - 0.2f, transform.position.z), transform.rotation);//左炮
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);//中炮
            Instantiate(bullet, new Vector3(transform.position.x + 0.5f, transform.position.y - 0.2f, transform.position.z), transform.rotation);//右炮
        }

    }

    //子弹冷却器，冷却时间到才能发射子弹
    private void FireCold()
    {
        if (fireColdCount >= fireCold)
        {
            PlayerFire();
            fireColdCount = 0;
        }
        else
        {
            fireColdCount += Time.deltaTime;
        }
    }

    //被击中
    private void GotShot()
    {
        //判断当前是否无敌状态，是的话返回，不是就变成是
        if (!unHurt)
        {
            FindObjectOfType<PlayerHP>().GotShot();//先扣血
            OnUnHurt();//变无敌
            Invoke(nameof(OffUnHurt), 1.0f);//3秒后取消无敌
        }
        else
        {
            return;
        }
    }

    //给发射冷却赋值
    public void SetFireCold(float time)
    {
        this.fireCold = time;
    }

    //玩家被摧毁
    public void PlayerDestroy()
    {
        Instantiate(PlayerBoom, transform.position, transform.rotation);//爆炸动画
        this.gameObject.SetActive(false);
    }

    //设定为无敌状态
    public void OnUnHurt()
    {
        this.unHurt = true;

        audioSource.clip = audioClips[1];
        audioSource.loop = true;
        audioSource.Play();
    }

    //取消无敌状态
    public void OffUnHurt()
    {
        this.unHurt = false;
        //停止播放
        audioSource.Stop();
    }

    //根据是否无敌状态播放无敌动画
    public void UnHurtAnim()
    {
        if (unHurt)
        {
            playerSpr.enabled = false;
            unHurtSpr.enabled = true;
        }
        else
        {
            playerSpr.enabled = true;
            unHurtSpr.enabled = false;
        }
    }

    public void FirePowerUpgrade()
    {
        if (this.firePowerLevel < 3)
        {
            firePowerLevel += 1;
            FindObjectOfType<PlayingManager>().SetFPLUI();

            audioSource.clip = audioClips[0];
            audioSource.loop = false;
            audioSource.Play();
        }
        else
        {
            return;
        }
    }

    public void FireSpeedUpgrade()
    {
        if(this.fireSpeedLevel < 3)
        {
            fireSpeedLevel += 1;
            SetFireCold(this.fireCold -0.1f);
            FindObjectOfType<PlayingManager>().SetFSLUI();

            audioSource.clip = audioClips[0];
            audioSource.loop = false;
            audioSource.Play();
        }
        else
        {
            return;
        }
    }
}
