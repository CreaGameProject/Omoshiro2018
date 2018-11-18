using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImportManager : MonoBehaviour
{
    public static Texture2D[] textures = new Texture2D[2];

    [SerializeField] private Image[] select;

    private bool isSelecting = false;
    private bool isSwap = false;

    private void Awake()
    {
        LoadTexture();
    }

    private void Update()
    {
        GetKey();
    }

    private void GetKey()
    {
        if ( Input.GetKeyDown( KeyCode.O ) )
        {
            if ( !isSelecting )
            {
                ShowSelect();
            }
            else
            {
                HideSelect();
            }
        }

        if ( isSelecting && Input.GetKeyDown( KeyCode.P ) )
        {
            SwapSelect();
        }

        if ( Input.GetKeyDown( KeyCode.R ) )
        {
            LoadTexture();
        }
    }

    private void LoadTexture()
    {
        textures[0] = GetTexture( "Player1.png" );
        textures[1] = GetTexture( "Player2.png" );

        for ( int i = 0; i < select.Length; i++ )
        {
            select[i].sprite = Sprite.Create( textures[i], new Rect( 0, 0, textures[0].width, textures[0].height ), Vector2.zero );
        }
    }

    private void ShowSelect()
    {
        isSelecting = true;
        for ( int i = 0; i < select.Length; i++ )
        {
            select[i].gameObject.SetActive( true );
        }
    }

    private void SwapSelect()
    {
        Vector3 tmp = select[0].transform.position;
        select[0].transform.position = select[1].transform.position;
        select[1].transform.position = tmp;

        isSwap = ( isSwap ) ? false : true;
    }

    private void HideSelect()
    {
        isSelecting = false;
        for ( int i = 0; i < select.Length; i++ )
        {
            select[i].gameObject.SetActive( false );
        }

        if ( isSwap )
        {
            Texture2D tmp = textures[0];
            textures[0] = textures[1];
            textures[1] = tmp;

            isSwap = false;
        }
    }

    //テクスチャを作成して、読み込んだバイトデータを適用する
    private Texture2D GetTexture( string fileName )
    {
        byte[] readData = readPNGFile( fileName );

        int pos = 16;
        int width = 0;
        for ( int i = 0; i < 4; i++ )
        {
            width = width * 255 + readData[pos++];
        }

        int height = 0;
        for ( int i = 0; i < 4; i++ )
        {
            height = height * 256 + readData[pos++];
        }

        Texture2D texture = new Texture2D( width, height );
        texture.LoadImage( readData );
        return texture;
    }

    //画像ファイルをバイトに変換して読み込む
    private byte[] readPNGFile( string fileName )
    {
        FileStream fileStream = new FileStream( Application.dataPath + "/" + fileName, FileMode.Open, FileAccess.Read );
        BinaryReader binaryReader = new BinaryReader( fileStream );
        byte[] datas = binaryReader.ReadBytes( (int)binaryReader.BaseStream.Length );
        binaryReader.Close();
        return datas;
    }
}
