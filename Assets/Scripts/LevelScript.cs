using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public GameObject emptyPrefab;
    public GameObject wallPrefab;
    public GameObject boxPrefab;
    public GameObject boxPlacedPrefab;
    public GameObject targetPrefab;
    public GameObject editablePrefab;
    public GameObject playerPrefab;

    public void DrawLevel(int[,] cells)
    {
        if(transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        transform.position = new Vector2(0, 0);
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int n = cells.GetLength(1) - 1; n >= 0; n--)
            {
                if(cells[i, n] == 0) { Instantiate(wallPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if(cells[i, n] == 1) { Instantiate(boxPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if(cells[i, n] == 2) { Instantiate(targetPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if(cells[i, n] == 3) { Instantiate(editablePrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if (cells[i, n] == 4) { Instantiate(emptyPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if (cells[i, n] == 5) { Instantiate(playerPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
            }
        }
        transform.position = new Vector2(-4, -4);
    }

    public void DrawLevelToPlay(int[,] cells)
    {
        transform.position = new Vector2(0, 0);
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int n = cells.GetLength(1) - 1; n >= 0; n--)
            {
                if (cells[i, n] == 0) { Instantiate(wallPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if (cells[i, n] == 1) { Instantiate(boxPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if (cells[i, n] == 2) { Instantiate(targetPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if (cells[i, n] == 3) { Instantiate(emptyPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if (cells[i, n] == 4) { Instantiate(emptyPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if (cells[i, n] == 5) { Instantiate(playerPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
            }
        }
        transform.position = new Vector2(-5, -4);
    }
}
