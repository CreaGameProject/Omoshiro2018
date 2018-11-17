using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

    [SerializeField] private Text[] textPlayer;//タイトル画面の各プレイヤーのテキスト

    private bool[] isReady;

    private float waitTime;//両方のプレイヤーの準備が完了してからテクスチャ反映画面に映るまでの時間
    private float startTime;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip titleME;


    void Start () {
        isReady = new bool[2];
        waitTime = 1;
        foreach (Text t in textPlayer)
            t.text = "Aボタンをおしてね";
        audioPlayer.PlayOneShot(titleME);
	}


    void Update () {
        //Escでいつでもゲーム終了
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //両方のプレイヤーの準備が完了しているとき
		if(isReady[0] && isReady[1])
        {
            //1秒経ったらテクスチャ反映画面へ（テクスチャ反映画面の内容が不明なので、
            //ゲーム画面へ移行）
            if(Time.time - startTime > waitTime)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
            }
        }

        //1P
        if (!isReady[0])
        {
            //1Pが〇ボタンを押したら
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetReady(0);
            }
        }

        //2P
        if (!isReady[1])
        {
            //2Pが〇ボタンを押したら
            if (Input.GetKeyDown(KeyCode.Joystick2Button0) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetReady(1);
            }
        }
    }

    //待機状態に移す
    private void SetReady(int side)
    {
        isReady[side] = true;
        textPlayer[side].text = side + 1 + "P　OK";
        if (isReady[0] && isReady[1])
            startTime = Time.time;
    }
}
