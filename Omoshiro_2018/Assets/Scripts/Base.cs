using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    Rigidbody Rig;
    bool[,] Left = new bool[2, 3];//１ｐ２ｐの十字ボタンの直前に押されてたか判定　Ａｘｉｓ版GetDown
    [SerializeField]
    GameObject[] MasterHund;//うで
    [SerializeField]
    float PushPower;//押す力
    private GameManager gameManager;
    [SerializeField] private AudioClip ton1;
    [SerializeField] private AudioClip ton2;
    [SerializeField] private AudioClip ton3;


    // Use this for initialization
    void Start()
    {
        Rig = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Shake(float x, float y, float Power)
    {

        Rig.AddForceAtPosition(new Vector3(-x, 0, -y), new Vector3(0, 3 * PushPower, 0), ForceMode.Impulse);
        PlaySE();
    }


    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameOver) return;
        if (!gameManager.IsStarted) return;

        Rig.angularVelocity = Vector3.zero;
        //if (!gameManager.isGameOver)
        GetPush();
    }

    void GetPush()
    {
        if (Input.GetButtonDown("RightUp1P") || Input.GetKeyDown(KeyCode.P))
        {
            if (!HundMove[0])
                StartCoroutine(Push(0));
            Shake(-2, -2, 1);
        }
        else if (Input.GetButtonDown("RightUp2P"))
        {
            if (!HundMove[2])
                StartCoroutine(Push(2));
            Shake(2, 2, 1);
        }

        if (Input.GetButtonDown("Right1P"))
        {
            if (!HundMove[0])
                StartCoroutine(Push(0));
            Shake(-2, -2, 1);
        }
        else if (Input.GetButtonDown("Right2P"))
        {
            if (!HundMove[2])
                StartCoroutine(Push(2));
            Shake(2, 2, 1);
        }

        if (Input.GetButtonDown("RightDown1P"))
        {
            if (!HundMove[0])
                StartCoroutine(Push(0));
            Shake(-2, -2, 1);
        }
        else if (Input.GetButtonDown("RightDown2P"))
        {
            if (!HundMove[2])
                StartCoroutine(Push(2));
            Shake(2, 2, 1);
        }

        if (Input.GetAxis("LeftHAxis1P") > 0.8f)
        {
            if (Left[0, 0])
            {
                if (!HundMove[1])
                    StartCoroutine(Push(1));
                Shake(-2, 2, 1);
                Left[0, 0] = false;
            }
        }
        else Left[0, 0] = true;
        if (Input.GetAxis("LeftVAxis1P") > 0.8f)
        {
            if (Left[0, 1])
            {
                if (!HundMove[1])
                    StartCoroutine(Push(1));
                Shake(-2, 2, 1);
                Left[0, 1] = false;
            }
        }
        else Left[0, 1] = true;
        if (Input.GetAxis("LeftVAxis1P") < -0.8f)
        {
            if (Left[0, 2])
            {
                if (!HundMove[1])
                    StartCoroutine(Push(1));
                Shake(-2, 2, 1);
                Left[0, 2] = false;
            }
        }
        else Left[0, 2] = true;

        if (Input.GetAxis("LeftHAxis2P") > 0.8f)
        {
            if (Left[1, 0])
            {
                if (!HundMove[3])
                    StartCoroutine(Push(3));
                Shake(2, -2, 1);
                Left[1, 0] = false;
            }
        }
        else Left[1, 0] = true;
        if (Input.GetAxis("LeftVAxis2P") > 0.8f)
        {
            if (Left[1, 1])
            {
                if (!HundMove[3])
                    StartCoroutine(Push(3));
                Shake(2, -2, 1);
                Left[1, 1] = false;
            }
        }
        else Left[1, 1] = true;
        if (Input.GetAxis("LeftVAxis2P") < -0.8f)
        {
            if (Left[1, 2])
            {
                if (!HundMove[3])
                    StartCoroutine(Push(3));
                Shake(2, -2, 1);
                Left[1, 2] = false;
            }
        }
        else Left[1, 2] = true;
    }
    bool[] HundMove = new bool[4];//４つの手が今動いてるか

    IEnumerator Push(int Hund)
    {//手を動かすんだよ
        HundMove[Hund] = true;
        Transform tf = MasterHund[Hund].transform;
        Vector3 Speed = new Vector3(0, -0.4f, 0);
        Vector3 Rot = new Vector3(3, 0, 0);
        for (int i = 0; i < 5; i++)
        {
            yield return null;
            tf.Translate(Speed, Space.World);
            tf.Rotate(Rot);
        }
        for (int i = 0; i < 5; i++)
        {
            yield return null;
            tf.Translate(Speed * -1, Space.World);
            tf.Rotate(Rot * -1);
        }
        HundMove[Hund] = false;
    }//手を動かすんだよぉ

    private void PlaySE()
    {
        int a = Random.Range(0, 3);
        switch (a)
        {
            case 0:
                gameManager.sePlayer.PlayOneShot(ton1);
                break;
            case 1:
                gameManager.sePlayer.PlayOneShot(ton2);
                break;
            case 2:
                gameManager.sePlayer.PlayOneShot(ton3);
                break;
            default:
                break;
        }
    }
}