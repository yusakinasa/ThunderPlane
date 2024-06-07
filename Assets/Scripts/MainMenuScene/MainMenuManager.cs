using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    //�����ʼ��ϷUI��ʾ
    public TextMeshProUGUI toPlay;
    //��˸���
    public float blinkCold { get; private set; }
    //��˸��ʱ
    public float blinkColdCount { get; private set; }
    //��Դ���
    public AudioSource audioSource;

    private void Awake()
    {
        //��ȡ��Դ���
        audioSource = this.gameObject.GetComponent<AudioSource>();
        //����
        audioSource.Play();
        //ѭ��
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
