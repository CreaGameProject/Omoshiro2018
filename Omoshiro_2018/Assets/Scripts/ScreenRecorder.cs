using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenRecorder : MonoBehaviour
{

    public int skipCount;//スクショを取るフレームの間隔
    [HideInInspector] public int count;
    [HideInInspector] public Dictionary<string, Texture2D> savedTextures;//スクショのテクスチャを保存するディクショナリ
    private Texture2D renderedTexture;//保存するテクスチャの型
    private Rect shotRect;//描画情報の読み込み領域
    private bool isEnd;//録画が終了したかどうか
    private bool isStarted;//録画開始したかどうか
    private string keyStr;

    private Replayer replayer;


    private void Start()
    {
        if (GameObject.Find("Replayer") != null)
            replayer = GameObject.Find("Replayer").GetComponent<Replayer>();
        savedTextures = new Dictionary<string, Texture2D>();
        skipCount = 2;
    }

    private void Update()
    {
        //keyStr = GetKeyCodeStr();

        //if (Input.anyKeyDown)
        //{
        //    switch (keyStr)
        //    {
        //        case "p":
        //            RecordControl();
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }



    //録画をコントロールするメソッド
    public void RecordControl()
    {
        if (!isStarted)
        {
            isStarted = true;
            StartCoroutine(RecordScreen());
        }
        else
        {
            isEnd = true;
        }
    }

    //キーを取得する
    private string GetKeyCodeStr()
    {
        return Input.inputString;
    }

    //指定された時間分スクリーンを録画してディクショナリに登録する
    private IEnumerator RecordScreen()
    {
        count = 0;
        while (true)
        {
            if (isEnd) break;
            if(count % skipCount == 0)
            {
                //string fileName = "ShotImage" + (count / skipCount).ToString("000");
                //yield return new WaitForEndOfFrame();
                //スクリーン情報をリストに登録
                Texture2D texture2D = ScreenCapture.CaptureScreenshotAsTexture();
                //savedTextures.Add(fileName, texture2D);
                replayer.shotImages.Add(texture2D);
            }
            count++;
            yield return null;
        }
    }
}