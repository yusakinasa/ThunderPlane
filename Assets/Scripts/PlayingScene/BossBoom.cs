using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBoom : MonoBehaviour
{
    //音源组件
    private AudioSource audioSource;
    private void Start()
    {
        //获取音源组件
        audioSource = this.gameObject.GetComponent<AudioSource>();
        //播放
        audioSource.Play();
        //不循环
        audioSource.loop = false;
        Invoke(nameof(Destroy), 1.0f);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
