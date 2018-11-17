using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerParams : MonoBehaviour {

    private bool isLost;//負けたらtrueになる
    [HideInInspector]public bool IsLost { get { return isLost; } }

    [SerializeField] private int id;//1Pか2Pかの識別
    [SerializeField] private GameObject resetPositionObject;//ゲームをやり直したときに移動先となるオブジェクト
    private Quaternion firstRotation;//初期の角度を記録
    [SerializeField] private Material modelMaterial;
    private Texture2D modelTexture;
    private GameManager gameManager;
    private Rigidbody rigidbody;
    private float massInc;



    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        firstRotation = transform.rotation;
        rigidbody = GetComponent<Rigidbody>();
        //----ここからテクスチャの適用
        if(id == 1)
        {
            modelTexture = GetTexture("Player1.png");
        }
        else
        {
            modelTexture = GetTexture("Player2.png");
        }
        //modelTexture = GetTexture();
        modelMaterial.mainTexture = modelTexture;
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material = modelMaterial;

        //----ここまでテクスチャの適用

        massInc = 0.1f;

    }

    private void Update()
    {
        //Debug.Log(transform.rotation.y);
        if (gameManager.isGameOver) return;
        if (!gameManager.IsStarted) return;
        if (isLost)
        {
            //isLost = false;
            if(id == 1)
            {
                gameManager.End(2);
            }
            else
            {
                gameManager.End(1);
            }
        }
        else
        {
            IsAlive();
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                rigidbody.mass += massInc;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                rigidbody.mass -= massInc;
            }
        }
    }

    //テクスチャを作成して、読み込んだバイトデータを適用する
    private Texture2D GetTexture(string fileName)
    {
        byte[] readData = readPNGFile(fileName);

        int pos = 16;
        int width = 0;
        for(int i = 0; i < 4; i++)
        {
            width = width * 255 + readData[pos++];
        }

        int height = 0;
        for(int i = 0; i < 4; i++)
        {
            height = height * 256 + readData[pos++];
        }

        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(readData);
        return texture;
    }

    //画像ファイルをバイトに変換して読み込む
    private byte[] readPNGFile(string fileName)
    {
        FileStream fileStream = new FileStream(Application.dataPath + "/" + fileName, FileMode.Open, FileAccess.Read);
        BinaryReader binaryReader = new BinaryReader(fileStream);
        byte[] datas = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
        binaryReader.Close();
        return datas;
    }

    //初期位置に戻す
    public void ResetPosition()
    {
        transform.position = resetPositionObject.transform.position;
        transform.rotation = firstRotation;
        rigidbody.velocity = Vector3.zero;
    }

    //勝ち負け判定メソッド　現状は、X軸方向の角度とZ軸方向の角度で判定
    private void IsAlive()
    {
        //Debug.Log(Mathf.Sin(transform.eulerAngles.z));
        if (( 45 < transform.eulerAngles.z && transform.eulerAngles.z < 315) 
            || (45 < transform.eulerAngles.x && transform.eulerAngles.x < 315))
        {
            isLost = true;
            //Debug.Log("call");
        }
    }
}
