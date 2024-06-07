using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SeCon: MonoBehaviour
{
    public void LoadSceneWithParam(string sceneName, int param)
    {
        // 存储参数
        PlayerPrefs.SetInt("Parameter", param);
        // 加载场景
        SceneManager.LoadScene(sceneName);
    }
}
