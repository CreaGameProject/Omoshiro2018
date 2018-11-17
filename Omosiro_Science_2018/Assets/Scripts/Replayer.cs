using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;

public class Replayer : MonoBehaviour {

    private List<Sprite> screenImages;
    public List<Texture2D> shotImages;

    [SerializeField] private Image videoPlayer;
    [SerializeField, Range(1, 10)] private int skipFrameCount;
    [SerializeField] private float replayBackTime;
    [SerializeField] private Text messageText;

    private GameManager gameManager;

    private ScreenRecorder screenRecorder;

    void Start () {
        if (GameObject.Find("ScreenRecorder") != null)
            screenRecorder = GameObject.Find("ScreenRecorder").GetComponent<ScreenRecorder>();
        screenImages = new List<Sprite>();
        shotImages = new List<Texture2D>();
        videoPlayer.gameObject.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	void Update () {

    }

    public void VideoControl()
    {
        //messageText.gameObject.SetActive(true);
        messageText.text = "読み込み中";
        StartCoroutine(PlayVideo(screenRecorder.skipCount));
    }

    //保存したスクショの画像のテクスチャ一覧を取得する
    private IEnumerator GetImages()
    {
        int startIndex = GetStartIndex(shotImages.Count);
        //yield return null;

        foreach(Texture2D texture2D in shotImages)
        {
            
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, Screen.width, Screen.height), Vector2.zero);
            //重すぎるので数フレーム待つ
            //for (int i = 0; i < skipFrameCount; i++)
            //{
            yield return null;
            //}
            screenImages.Add(sprite);
        }

        RemoveExtra(screenImages, startIndex);

    }

    //不要な画像を取り除く
    private void RemoveExtra(List<Sprite> list , int removeCount)
    {
        for(int i = 0; i < removeCount; i++)
        {
            list.RemoveAt(0);
        }
    }
    
    //再生の開始位置を返す
    private int GetStartIndex(int endPoint)
    {
        int startIndex = (int)(shotImages.Count - 60 * replayBackTime / screenRecorder.skipCount);
        return startIndex;
    }

    //取得したスクショ画像を切り替えて動画っぽくする
    private IEnumerator PlayVideo(int skip)
    {
        yield return StartCoroutine(GetImages());
        videoPlayer.gameObject.SetActive(true);
        messageText.text = "";
        foreach(Sprite sprite in screenImages)
        {
            videoPlayer.sprite = sprite;
            for(int i=0;i<skip;i++)
                yield return null;
        }
        videoPlayer.gameObject.SetActive(false);
        gameManager.ShowResult();
    }
}
