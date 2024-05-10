using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    //点击开始游戏UI显示
    public TextMeshProUGUI toPlay;
    //闪烁间隔
    public float blinkCold { get; private set; }
    //闪烁计时
    public float blinkColdCount { get; private set; }
    //音源组件
    public AudioSource audioSource;

    private void Awake()
    {
        //获取音源组件
        audioSource = this.gameObject.GetComponent<AudioSource>();
        //播放
        audioSource.Play();
        //循环
        audioSource.loop = true;
        blinkCold = 0.2f;
        blinkColdCount = 0f;
    }
    private void Update()
    {
        LoadGameScene();
        TimeCount();
        TextBlink();
    }

    public void LoadGameScene()
    {
        if (Input.anyKeyDown)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                SceneManager.LoadScene("Playing");
            }
        }
    }

    public void TextBlink()
    {
        if (blinkColdCount >= blinkCold)
        {
            toPlay.alpha = 255;
        }
        else
        {
            toPlay.alpha = 0;
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
