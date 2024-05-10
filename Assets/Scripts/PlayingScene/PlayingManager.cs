using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// 包含：
/// 1.分数 击败每个敌人获得相应分数
/// 2.难度等级 每打满10分提升一级 奖励1分
/// </summary>

public class PlayingManager : MonoBehaviour
{
    //玩家和玩家血条，都是场景中的对象
    public Player player;
    public PlayerHP playerHP;

    //玩家分数
    public int score { get; private set; }
    //分数UI显示
    public TextMeshProUGUI scoreUI;

    //难度等级
    public int difficutLevel { get; private set; }
    //难度等级UI显示
    public TextMeshProUGUI diffLevelUI;

    //Boss是否出生
    public bool isBossSpawn { get; private set; }
    //Boss血条
    public BossHP bossHP;

    //游戏结束时Boss是否死亡
    public bool isBossDead { get; private set; }
    //游戏是否已经结束
    public int isGameOver { get; private set; }

    //对局时长
    public float gameDuration { get; private set; }
    //对局时长UI显示
    public TextMeshProUGUI gameDurationUI;

    //音源组件
    public AudioSource audioSource { get; private set; }
    //声音数组，0为普通，1为Boss战
    public AudioClip[] audioClipList;

    //玩家火力等级
    public int firePowerLevel { get; private set; }
    //玩家射速等级
    public int fireSpeedLevel { get; private set; }
    //玩家火力等级UI显示
    public TextMeshProUGUI firePowerLevelUI;
    //玩家射速等级UI显示
    public TextMeshProUGUI fireSpeedLevelUI;

    private void Awake()
    {
        ResetState();
        //获取音源组件
        audioSource = this.gameObject.GetComponent<AudioSource>();
        //获取普通音乐
        audioSource.clip = audioClipList[0];
        //播放
        audioSource.Play();
        //循环
        audioSource.loop = true;
    }

    //重置状态
    public void ResetState()
    {
        SetScore(0);//开局分数为0
        SetDifficutLevel(1);//开局难度等级为1级
        isBossSpawn = false;//Boss还未出生
        isBossDead = false;//Boss还未死亡
        bossHP.gameObject.SetActive(false);//Boss血条不显示
        isGameOver = 0;//游戏未结束
        gameDuration = 0f;

        diffLevelUI.text = this.difficutLevel.ToString();
        scoreUI.text = this.score.ToString();

        FindObjectOfType<PlayerHP>().ResetState();
        FindObjectOfType<Player>().ResetState();

        PlayerPrefs.SetInt("Score", this.score);
        PlayerPrefs.SetInt("Level", this.difficutLevel);
        PlayerPrefs.SetInt("IsBossDead", this.isBossDead ? 1 : 0);
        PlayerPrefs.SetInt("GameDuration", Mathf.FloorToInt(this.gameDuration));
    }

    private void FixedUpdate()
    {
        //分数大于零后，分数每增加10，难度上升一级
        if (score > 0 && score % 10 == 0)
        {
            DifficultRise();
        }
        //计算游戏时长
        if(isGameOver == 0)
        {
            GameDurationRise();
        }
    }

    //设置分数
    private void SetScore(int score)
    {
        this.score = score;
    }

    //分数增加
    public void ScoreRise(int score)
    {
        SetScore(this.score + score);
        scoreUI.text = this.score.ToString();
    }

    //设置难度范围
    private void SetDifficutLevel(int level)
    {
        this.difficutLevel = level;
        diffLevelUI.text = this.difficutLevel.ToString();
    }

    //难度等级 + 1
    public void DifficutLevelRise()
    {
        SetDifficutLevel(this.difficutLevel + 1);
    }

    //每消灭10架敌机，难度上升
    //消灭够100架敌机，Boss出生
    public void DifficultRise()
    {
        FindObjectOfType<EnemySpawn>().SpawnFaster();
        //消灭够100架以上敌机，Boss出生
        if (score >= 100 && !isBossSpawn)
        {
            isBossSpawn = true;

            audioSource.Stop();//先停止播放
            audioSource.clip = audioClipList[1];//切换为Boss战音乐
            audioSource.Play();//播放

            FindObjectOfType<EnemySpawn>().BossSpawn();
        }
        //奖励一分，不然在杀死下一个敌人前，会持续调用这个难度增加方法
        SetScore(this.score + 1);
    }

    //等到Boss移动到指定高度后，再显示Boss血条
    public void SetBossHPActive()
    {
        bossHP.gameObject.SetActive(true);
    }

    //游戏结束，参数为Boss是否死亡，用来区别游戏输赢
    public void GameOver(bool isBossDead)
    {
        //停止播放音乐
        audioSource.Stop();
        //防止重复调用GameOver方法
        this.isGameOver += 1;
        if (isGameOver == 1)//只有第一次调用有用
        {
            this.isBossDead = isBossDead;
            if (isBossDead)//Boss死亡，玩家胜利
            {
                FindObjectOfType<Boss>().BossDestroy();//摧毁Boss对象
            }
            else//Boss存活，玩家死亡
            {
                player.PlayerDestroy();//摧毁玩家对象
            }
            Invoke(nameof(LoadOverScene), 2.0f);
        }
    }

    public void LoadOverScene()
    {
        //判断当前场景是否是游戏场景
        if (SceneManager.GetActiveScene().name == "Playing")
        {
            PlayerPrefs.SetInt("Score", this.score);
            PlayerPrefs.SetInt("Level", this.difficutLevel);
            PlayerPrefs.SetInt("IsBossDead", this.isBossDead ? 1 : 0);
            PlayerPrefs.SetInt("GameDuration", Mathf.FloorToInt(this.gameDuration));
            SceneManager.LoadScene("GameOver");
        }
    }

    public void GameDurationRise()
    {
        this.gameDuration += Time.deltaTime;
        gameDurationUI.text = Mathf.FloorToInt(this.gameDuration).ToString();
    }

    public void SetFPLUI()
    {
        this.firePowerLevel = FindObjectOfType<Player>().firePowerLevel;
        firePowerLevelUI.text = "Lv " + this.firePowerLevel.ToString();
    }

    public void SetFSLUI()
    {
        this.fireSpeedLevel = FindObjectOfType<Player>().fireSpeedLevel;
        fireSpeedLevelUI.text = "Lv " + this.fireSpeedLevel.ToString();
    }

}
