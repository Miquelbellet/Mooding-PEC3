using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditLevelController : MonoBehaviour
{
    public GameObject selectedItemPrefab;
    public Sprite wallSprite;
    public Sprite boxSprite;
    public Sprite targetSprite;


    private LevelScript levelScript;
    private int Cols = 8;
    private int Rows = 12;
    int[,] cells;

    private GameObject selectedItemObject;
    private bool settingWall;
    private bool settingBox;
    private bool settingTarget;
    void Start()
    {
        levelScript = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelScript>();
        //if (PlayerPrefs.GetInt("EditLevel", 0) > 0) DrawSelectedLevel();
        //else DrawEmptyLevel();
        DrawSelectedLevel();
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
    }

    private void DrawSelectedLevel()
    {
        cells = new int[Rows, Cols];
        for (int i = 0; i < Rows; i++)
        {
            for (int n = Cols - 1; n >= 0; n--)
            {
                cells[i, n] = Random.Range(0, 4);
            }
        }
        levelScript.DrawLevel(cells);
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
}
