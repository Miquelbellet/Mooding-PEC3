using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelController : MonoBehaviour
{
    public AudioClip[] songs;

    private LevelScript levelScript;
    private int Rows = 12;
    private int Cols = 8;
    int[,] cells;
    private AudioSource gameAS;

    void Start()
    {
        levelScript = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelScript>();
        gameAS = GetComponent<AudioSource>();
        string path = "Assets/Resources/Level" + PlayerPrefs.GetInt("Level", 0) + ".txt";
        if (File.Exists(path))
        {
            ReadLevelFile(path);
            levelScript.DrawLevelToPlay(cells);
        }
        else SceneManager.LoadScene("GameMenu");
    }

    void Update()
    {
        SetRandomSongs();
    }

    private void ReadLevelFile(string path)
    {
        StreamReader reader = new StreamReader(path);
        var completed = reader.ReadLine();
        Rows = int.Parse(reader.ReadLine());
        Cols = int.Parse(reader.ReadLine());
        cells = new int[Rows, Cols];
        for (int i = 0; i < Rows; i++)
        {
            for (int n = 0; n < Cols; n++)
            {
                cells[i, n] = int.Parse(reader.ReadLine());
            }
        }
        reader.Close();
    }

    private void SetRandomSongs()
    {
        if (!gameAS.isPlaying)
        {
            gameAS.PlayOneShot(songs[Random.Range(0, songs.Length)]);
        }
    }

    public void BackBtn()
    {
        SceneManager.LoadScene("GameMenu");
    }
}
