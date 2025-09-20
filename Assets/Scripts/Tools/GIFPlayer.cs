using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UniGif;

public class GIFPlayer : MonoBehaviour
{
    public RawImage TarGetImg;
    public string GifPath="";
    List<UniGif.GifTexture> GifTextures = new List<GifTexture>();
    int index = -1;
    float timer = 0;
    bool isGifLoaded;


    private void Awake()
    {
        TarGetImg = GetComponent<RawImage>();

    }
    void Start()
    {
        string path = Application.dataPath + GifPath;
        // 使用Using确保Stream被正确释放
        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            byte[] bt = new byte[stream.Length];
            stream.Read(bt, 0, (int)stream.Length);
            StartCoroutine(UniGif.GetTextureListCoroutine(bt, LoadingIfPic));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGifLoaded || GifTextures.Count == 0) return;

        timer += Time.deltaTime; // 累计时间

        // 获取当前帧的延迟时间（以秒为单位）
        float currentFrameDelay = GifTextures[index].m_delaySec;

        // 当累计时间超过当前帧的延迟时间时，切换到下一帧
        if (timer >= currentFrameDelay)
        {
            timer = 0f; // 重置计时器
            index = (index + 1) % GifTextures.Count; // 循环索引
            TarGetImg.texture = GifTextures[index].m_texture2d; // 更新纹理
        }
    }

    public void LoadingIfPic(List<GifTexture> list, int count, int width, int hight)
    {
        GifTextures = list;
        isGifLoaded = true; // 标记加载完成
        index = 0;
        // 立即显示第一帧
        if (GifTextures.Count > 0)
        {
            TarGetImg.texture = GifTextures[0].m_texture2d;
        }
    }
}
