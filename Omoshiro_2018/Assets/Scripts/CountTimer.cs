using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountTimer : MonoBehaviour {

    private Image countDownImage;//カウントダウンのイメージ
    [SerializeField] private Sprite[] countDownSprites;
    [SerializeField] private int count;
    private GameManager gameManager;

    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        countDownImage = GetComponent<Image>();
        StartCoroutine(CountDown(count));
	}

    //private void ChangeText(int count)
    //{
    //    coutDownText.text = "" + count;
    //}


    private void ChangeImage(int count)
    {
        countDownImage.sprite = countDownSprites[count - 1];
    }



    //引数に指定された時間分カウントダウンを行う
    public IEnumerator CountDown(int count)
    {
        while(count > 0)
        {
            //countDownText.text = "" + count;
            ChangeImage(count);
            count--;
            yield return new WaitForSeconds(1);
        }

        //countDownText.text = "スタート";
        yield return new WaitForSeconds(0.5f);
        //countDownText.text = "";
        gameManager.StartGame();
        //gameObject.SetActive(false);
        countDownImage.gameObject.SetActive(false);
    }
}
