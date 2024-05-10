using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现原理：
/// 背景图片Wrap Mode设置为 Repeat
/// 新建一个Material材质文件，材质的Shader设置为Texture
/// 把材质给到图片的Sprite Renderer
/// 通过更改材质中的offSet属性实现背景的滚动
/// </summary>
public class BackgroundScroll : MonoBehaviour
{
    [Tooltip("移动速度"), Range(0.01f, 1f)]

    public float moveSpeed;
    private SpriteRenderer render;

    //初始化
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    //初始化
    void Update()
    {
        BgScroll();
    }
    /// <summary>
    /// 单张图片的重复滚动
    /// </summary>
    public void BgScroll()
    {
        //图片模式已设置为repeat，通过代码修改材质中的offset即可实现滚动
        //横向滚动改x值，纵向滚动改y值
        render.material.mainTextureOffset += new Vector2(0, moveSpeed * Time.deltaTime);
    }
}
