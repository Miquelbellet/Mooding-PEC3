using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    [HideInInspector] public int[,] cells = new int[Rows, Cols]; // 0 = empty; 1 = wall; 2 = box; 3 = target
    [HideInInspector] public static int Rows = 8;
    [HideInInspector] public static int Cols = 10;
    [HideInInspector] public int numRows = 8;
    [HideInInspector] public int numCols = 10;

    public GameObject emptyPrefab;
    public GameObject wallPrefab;
    public GameObject boxPrefab;
    public GameObject boxPlacedPrefab;
    public GameObject targetPrefab;

    public void DrawLevel(int[,] cells)
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int n = 0; n < Cols; n++)
            {
                if(cells[i, n] == 0) { Instantiate(emptyPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if(cells[i, n] == 1) { Instantiate(wallPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if(cells[i, n] == 2) { Instantiate(boxPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if(cells[i, n] == 3) { Instantiate(targetPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
            }
        }
    }
}
