using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SeCon: MonoBehaviour
{
    public void LoadSceneWithParam(string sceneName, int param)
    {
        // �洢����
        PlayerPrefs.SetInt("Parameter", param);
        // ���س���
        SceneManager.LoadScene(sceneName);
    }
}
