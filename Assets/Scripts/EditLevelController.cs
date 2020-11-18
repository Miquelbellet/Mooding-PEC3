using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class EditLevelController : MonoBehaviour
{
    public GameObject selectedItemPrefab;
    public Sprite wallSprite;
    public Sprite boxSprite;
    public Sprite targetSprite;
    public Sprite playerSprite;


    private LevelScript levelScript;
    private int Rows = 12;
    private int Cols = 8;
    int[,] cells;

    private GameObject selectedItemObject;
    private bool settingWall;
    private bool settingBox;
    private bool settingTarget;
    private bool settingPlayer;
    
    void Start()
    {
        levelScript = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelScript>();
        string path = "Assets/Resources/Level" + PlayerPrefs.GetInt("Level", 0) + ".txt";
        if (File.Exists(path))
        {
            ReadLevelFile(path);
            levelScript.DrawLevel(cells);
        }
        else DrawEmptyLevel();
    }

    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var xPos = Mathf.Round(mousePos.x + 4);
        var yPos = Mathf.Round(mousePos.y + 4);
        if (Input.GetMouseButtonDown(1))
        {
            try
            {
                cells[(int)xPos, (int)yPos] = 3;
                levelScript.DrawLevel(cells);
            }
            catch { }
        }
        if (settingWall)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedItemObject.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
            if (Input.GetMouseButtonDown(0))
            {
                settingWall = false;
                selectedItemObject.SetActive(false);
                Destroy(selectedItemObject);
                try
                {
                    cells[(int)xPos, (int)yPos] = 0;
                    levelScript.DrawLevel(cells);
                }
                catch { }
            }
        }
        else if (settingBox)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedItemObject.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
            if (Input.GetMouseButtonDown(0))
            {
                settingBox = false;
                selectedItemObject.SetActive(false);
                Destroy(selectedItemObject);
                try
                {
                    cells[(int)xPos, (int)yPos] = 1;
                    levelScript.DrawLevel(cells);
                }
                catch { }
            }
        }
        else if (settingTarget)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedItemObject.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
            if (Input.GetMouseButtonDown(0))
            {
                settingTarget = false;
                selectedItemObject.SetActive(false);
                Destroy(selectedItemObject);
                try
                {
                    cells[(int)xPos, (int)yPos] = 2;
                    levelScript.DrawLevel(cells);
                }
                catch { }
            }
        }
        else if (settingPlayer)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedItemObject.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
            if (Input.GetMouseButtonDown(0))
            {
                settingPlayer = false;
                selectedItemObject.SetActive(false);
                Destroy(selectedItemObject);
                try
                {
                    for (int i = 0; i < Rows; i++)
                    {
                        for (int n = Cols - 1; n >= 0; n--)
                        {
                            if (cells[i, n] == 5) cells[i, n] = 3;
                        }
                    }
                    cells[(int)xPos, (int)yPos] = 5;
                    levelScript.DrawLevel(cells);
                }
                catch { }
            }
        }
    }

    private void DrawEmptyLevel()
    {
        cells = new int[Rows, Cols];
        for (int i = 0; i < Rows; i++)
        {
            for (int n = Cols - 1; n >= 0; n--)
            {
                cells[i, n] = 3;
            }
        }
        levelScript.DrawLevel(cells);
    }

    private void ReadLevelFile(string path)
    {
        StreamReader reader = new StreamReader(path);
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

    public void SetWall()
    {
        settingWall = true;
        selectedItemObject = Instantiate(selectedItemPrefab);
        selectedItemObject.SetActive(true);
        selectedItemObject.GetComponent<SpriteRenderer>().sprite = wallSprite;
    }

    public void SetBox()
    {
        settingBox = true;
        selectedItemObject = Instantiate(selectedItemPrefab);
        selectedItemObject.SetActive(true);
        selectedItemObject.GetComponent<SpriteRenderer>().sprite = boxSprite;
    }

    public void SetTarget()
    {
        settingTarget = true;
        selectedItemObject = Instantiate(selectedItemPrefab);
        selectedItemObject.SetActive(true);
        selectedItemObject.GetComponent<SpriteRenderer>().sprite = targetSprite;
    }

    public void SetPlayer()
    {
        settingPlayer = true;
        selectedItemObject = Instantiate(selectedItemPrefab);
        selectedItemObject.SetActive(true);
        selectedItemObject.GetComponent<SpriteRenderer>().sprite = playerSprite;
    }

    public void TestLevel()
    {

    }

    public void SaveLevel()
    {
        string path = "Assets/Resources/Level" + PlayerPrefs.GetInt("Level", 0) + ".txt";
        if (!File.Exists(path))
        {
            string path2 = "Assets/Resources/LevelsCount.txt";
            StreamWriter writer2 = new StreamWriter(path2, false);
            writer2.WriteLine(PlayerPrefs.GetInt("Level", 0));
            writer2.Close();
        }

        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(Rows);
        writer.WriteLine(Cols);
        for (int i = 0; i < Rows; i++)
        {
            for (int n = 0; n < Cols; n++)
            {
                writer.WriteLine(cells[i, n]);
            }
        }
        writer.Close();

        AssetDatabase.ImportAsset(path);
        AssetDatabase.Refresh();
        SceneManager.LoadScene("EditMenu");
    }

    public void BackBtn()
    {
        SceneManager.LoadScene("EditMenu");
    }
}
