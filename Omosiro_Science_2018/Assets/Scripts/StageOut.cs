using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOut : MonoBehaviour {
    //bool Finish = false;
    [SerializeField]
    Collider[] koma;//コマやぞ
    private GameManager gameManager;
    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
    private void OnTriggerEnter(Collider Col)
    {
        if (!gameManager.isGameOver)
        {
            //1Pが落ちたとき
            if (koma[0] == Col)
                Gameset(false);
            //2Pが落ちたとき
            else if (koma[1] == Col)
                Gameset(true);
        }
    }

    void Gameset(bool win1p)
    {
        //Debug.Log(win1p);
        gameManager.isGameOver = true;
        //1Pが勝利した場合
        if (win1p)
        {
            gameManager.End(1);
        }
        else
        {
            gameManager.End(2);
        }

        foreach (Collider k in koma)
        {
            k.transform.parent.gameObject.SetActive(false);//必要に応じて弄ってくれ
        }
        //ここに処理を書く
    }//決着時の処理　Boolは１PがWinかどうか
}


