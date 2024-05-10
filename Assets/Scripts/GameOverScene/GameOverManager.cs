using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    //胜利或失败UI
    public TextMeshProUGUI[] WinOrLose;

    //Boss是否死亡
    public bool isBossDead { get; private set; }

    //难度等级UI
    public TextMeshProUGUI levelText;
    //难度等级
    public int level { get; private set; }

    //分数UI
    public TextMeshProUGUI scoreText;
    //分数
    public int score { get; private set; }

    //游戏时长UI
    public TextMeshProUGUI gameDurationText;
    //游戏时长
    public int gameDuration { get; private set; }

    //音源组件
    public AudioSource audioSource;
    //声音数组，0为胜利，1为失败
    public AudioClip[] audioClipList;

    //点击开始游戏UI显示
    public TextMeshProUGUI playAgain;
    //闪烁间隔
    public float blinkCold { get; private set; }
    //闪烁计时
    public float blinkColdCount { get; private set; }

    private void Start()
    {
        //获取音源组件
        audioSource = this.gameObject.GetComponent<AudioSource>();

        //从PlayerPrefs拿到游戏数据
        this.score = PlayerPrefs.GetInt("Score");
        this.level = PlayerPrefs.GetInt("Level");
        this.isBossDead = PlayerPrefs.GetInt("IsBossDead") == 1 ? true : false;
        this.gameDuration = PlayerPrefs.GetInt("GameDuration");
        
        MissionState();
        SetText();

        //播放
        audioSource.Play();
        //循环
        audioSource.loop = true;

        blinkCold = 0.2f;
        blinkColdCount = 0f;
    }

    private void FixedUpdate()
    {
        LoadGameScene();

        TimeCount();
        TextBlink();
    }

    //显示胜利还是失败
    public void MissionState()
    {
        if (isBossDead)
        {
            WinOrLose[0].gameObject.SetActive(false);
            audioSource.clip = audioClipList[0];
        }
        else {
            WinOrLose[1].gameObject.SetActive(false);
            audioSource.clip = audioClipList[1];
        }
    }

    //显示游戏数据UI
    public void SetText()
    {
        levelText.text = "Level: " + level.ToString();
        scoreText.text = "Score: " + score.ToString();
        gameDurationText.text = "Time: " + gameDuration.ToString() + " s";
    }

    //加载游戏场景
    public void LoadGameScene()
    {
        if (Input.anyKeyDown)
        {
            if (SceneManager.GetActiveScene().name == "GameOver")
            {
                SceneManager.LoadScene("Playing");
            }
        }
    }

    public void TextBlink()
    {
        if (blinkColdCount >= blinkCold)
        {
            playAgain.alpha = 255;
        }
        else
        {
            playAgain.alpha = 0;
        }
    }

    public void TimeCount()
    {
        blinkColdCount += Time.deltaTime;
        if (Mathf.FloorToInt(blinkColdCount) == 1)
        {
            blinkColdCount = 0f;
        }
    }
}
