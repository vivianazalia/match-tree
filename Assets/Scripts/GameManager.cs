using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //bug game ini : 
    //1. sebelah kiri, object hanya bisa menukar/swipe dari bawah ke atas. bagian atas, sekitar 2 baris dari atas juga ada yang 
    //   hanya bisa menukar/swipe kanan ke kiri.
    //2. terkadang ada dua object saja yang sudah match.
    //3. saat di awal ada object yang sudah match, object tersebut tidak akan hilang sampai dilakukan match pertama manual.
    //4. pada saat tertentu, object diinstantiate tidak pada tempatnya (menumpuk pada object lain) karena queue sudah full dan masing-masing prefab object sudah menempati posisi masing-masing.
    //   jadi, ketika dipangggil lagi maka object tersebut akan menempati tempat yang sebelumnya, dimana bisa jadi tempat sebelumnya sudah ditempati object lain.

    //materi terakhir yang dipraktekkan di dilo academy class
    //Achievement System & Observer Pattern bagian Tambahan 
    //masih ada error di jalannya program yang tidak sesuai, tidak memunculkan achievement message saat kondisi terpenuhi. 
    //curiga ke override OnMatch() yang belum dijelaskan perilakunya. 

    public static GameManager instance;
    private int playerScore;
    public Text scoreText;

    void Start()
    {
        //singleton pattern -> membuat instance dari sebuah class hanya satu saja
        if (instance == null)
        {
            instance = this;
        } 
        else if(instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GetScore(int point)
    {
        playerScore += point;
        scoreText.text = playerScore.ToString();
    }
}
