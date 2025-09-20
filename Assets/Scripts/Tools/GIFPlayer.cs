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
        // ʹ��Usingȷ��Stream����ȷ�ͷ�
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

        timer += Time.deltaTime; // �ۼ�ʱ��

        // ��ȡ��ǰ֡���ӳ�ʱ�䣨����Ϊ��λ��
        float currentFrameDelay = GifTextures[index].m_delaySec;

        // ���ۼ�ʱ�䳬����ǰ֡���ӳ�ʱ��ʱ���л�����һ֡
        if (timer >= currentFrameDelay)
        {
            timer = 0f; // ���ü�ʱ��
            index = (index + 1) % GifTextures.Count; // ѭ������
            TarGetImg.texture = GifTextures[index].m_texture2d; // ��������
        }
    }

    public void LoadingIfPic(List<GifTexture> list, int count, int width, int hight)
    {
        GifTextures = list;
        isGifLoaded = true; // ��Ǽ������
        index = 0;
        // ������ʾ��һ֡
        if (GifTextures.Count > 0)
        {
            TarGetImg.texture = GifTextures[0].m_texture2d;
        }
    }
}
