using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {

    [HideInInspector] public bool isGameOver;//勝ち負けが決まったかどうか
    private bool isStarted;//ゲームが始まっているかどうか
    public bool IsStarted { get { return isStarted; } }
    [SerializeField] private GameObject emergency;//ポーズ画面オブジェクト
    [SerializeField] private Button buttonToTitle;//タイトルに戻るボタン
    [SerializeField] private Button buttonResetGame;//ゲームリセットボタン
    [SerializeField] private GameObject countTimer;//カウントダウンタイマー
    [SerializeField] private GameObject resultPanel;//結果表示のパネル
    [SerializeField] private Image resultImagePlayer1;//結果表示イメージ
    [SerializeField] private Image resultImagePlayer2;
    [SerializeField] private Sprite[] resultSprite;//勝敗の画像 0:1PWins 1:2PWins
    [SerializeField] private float resultTime;//結果の表示時間
    public AudioSource sePlayer;
    public AudioSource bgmPlayer;
    [SerializeField] private AudioClip bgm1;
    [SerializeField] private AudioClip bgm2;

    //private ScreenRecorder screenRecorder;
    //private Replayer replayer;


    void Start () {
        isGameOver = false;
        isStarted = false;
        buttonToTitle.onClick.AddListener(() => BackToTitle());
        buttonResetGame.onClick.AddListener(() => ResetGame());
        SwitchMenu(emergency,false);
        SwitchMenu(resultPanel, false);
        StartCoroutine(PlayBGM());
    }



    void Update () {

        //エスケープキーで終了
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //ポーズ画面の表示
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (emergency.activeSelf)
                SwitchMenu(emergency, false);
            else
                SwitchMenu(emergency, true);
        }
    }

    //いちいちコルーチンを呼び出すのが面倒だったので記述
    public void End(int id)
    {
        //StartCoroutine(EndGame(id));
        EndGame(id);
    }

    //ゲーム終了メソッド
    private void EndGame(int id)
    {
        //screenRecorder.RecordControl();
        //1Pが勝利した場合
        if(id == 1)
        {
            //Debug.Log("Win 1P");
            resultImagePlayer1.sprite = resultSprite[0];
            resultImagePlayer2.sprite = resultSprite[1];
        }
        else //2Pが勝利した場合
        {
            //Debug.Log("Win 2P");
            resultImagePlayer1.sprite = resultSprite[1];
            resultImagePlayer2.sprite = resultSprite[0];
        }

        isGameOver = true;
        ShowResult();
        //一定時間待つ
    }

    public void ShowResult()
    {
        resultPanel.SetActive(true);
    }

    //タイトルに戻る
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    //ゲームを最初からやり直す
    private void ResetGame()
    {
        SceneManager.LoadScene( "Game" );

        //isGameOver = false;
        //isStarted = false;
        //ResetPlayer();
        //resultPanel.SetActive(false);
        //countTimer.SetActive(true);
        //StartCoroutine(countTimer.GetComponent<CountTimer>().CountDown(3));
        ////ポーズ画面を非表示にする
        //SwitchMenu(emergency,false);
    }

    //駒の位置と角度をもとに戻す
    private void ResetPlayer()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in objects)
        {
            player.GetComponent<PlayerParams>().ResetPosition();
        }
    }

    //パネル表示非表示の切り替え
    private void SwitchMenu(GameObject obj, bool active)
    {
        obj.SetActive(active);
    }

    //ゲームを開始する
    public void StartGame()
    {
        isStarted = true;
    }

    private IEnumerator PlayBGM()
    {
        while (!isGameOver)
        {
            int choice = Random.Range(0, 2);
            if(choice == 0)
            {
                bgmPlayer.PlayOneShot(bgm1);
            }
            else
            {
                bgmPlayer.PlayOneShot(bgm2);
            }
            while (bgmPlayer.isPlaying)
                yield return null;
        }
    }
}