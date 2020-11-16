using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditLevelController : MonoBehaviour
{
    private LevelScript levelScript;

    void Start()
    {
        levelScript = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelScript>();
        var numRows = levelScript.numRows;
        var numCols = levelScript.numCols;
        int[,] cells = new int[numRows, numCols];
        for (int i = 0; i < numRows; i++)
        {
            for (int n = 0; n < numCols; n++)
            {
                cells[i, n] = Random.Range(0, 4);
            }
        }
        levelScript.DrawLevel(cells);
    }

    void Update()
    {
        
    }
}
