﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    public GameObject buttonsParent;
    public GameObject redButtonPrefab;
    public GameObject greenButtonPrefab;

    void Start()
    {
        string path = "Assets/Resources/LevelsCount.txt";
        if (File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);
            var numOfLevels = int.Parse(reader.ReadLine());
            for (int i = 1; i <= numOfLevels; i++)
            {
                string path2 = "Assets/Resources/Level" + i + ".txt";
                StreamReader reader2 = new StreamReader(path2);
                var completed = reader2.ReadLine();
                if (completed == "NotCompleted") Instantiate(redButtonPrefab, buttonsParent.transform);
                else if (completed == "Completed") Instantiate(greenButtonPrefab, buttonsParent.transform);
                reader2.Close();
            }
        }
        SetAllButtonsListeners();
    }

    void Update()
    {

    }

    private void SetAllButtonsListeners()
    {
        for (int i = 0; i < buttonsParent.transform.childCount; i++)
        {
            int x = i + 1;
            buttonsParent.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = x.ToString();
            buttonsParent.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { SelectedLevel(x); });
        }
    }

    public void SelectedLevel(int level)
    {
        PlayerPrefs.SetInt("Level", level);
        SceneManager.LoadScene("GameLevel");
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
