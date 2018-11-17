using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImportManager : MonoBehaviour
{
    public static Texture2D[] textures = new Texture2D[2];

    [SerializeField] private Image[] select;

    private void Awake()
    {
        textures[0] = GetTexture( "Player1.png" );
        textures[1] = GetTexture( "Player2.png" );
    }

    public void ShowImages()
    {
        for ( int i = 0; i < select.Length; i++ )
        {
            select[i].sprite = Sprite.Create( textures[i], new Rect( 0, 0, textures[0].width, textures[0].height ), Vector2.zero );
            select[i].gameObject.SetActive( true );
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
