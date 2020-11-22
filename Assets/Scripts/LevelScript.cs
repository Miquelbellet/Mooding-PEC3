using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour
{
    public Vector2 levelPosInit;
    public GameObject emptyPrefab;
    public GameObject wallPrefab;
    public GameObject boxPrefab;
    public GameObject boxPlacedPrefab;
    public GameObject targetPrefab;
    public GameObject editablePrefab;
    public GameObject playerPrefab;

    private GameObject gameController;
    private int[,] levelCells;
    private int targetsCount;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        if(gameController.GetComponent<UIScriptController>()) gameController.GetComponent<UIScriptController>().SetLevelNumber(PlayerPrefs.GetInt("Level", 0));
    }

    public void DrawEditLevel(int[,] cells)
    {
        if(transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        levelCells = cells;
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
                else if (cells[i, n] == 5) {
                    var player = Instantiate(playerPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform);
                    player.GetComponent<PlayerController>().activatePlayer = false;
                }
            }
        }
        transform.position = new Vector2(levelPosInit.x, levelPosInit.y);
    }

    public void DrawLevelToPlay(int[,] cells)
    {
        levelCells = cells;
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
        transform.position = new Vector2(levelPosInit.x, levelPosInit.y);
    }

    private void DrawLevelToPlayWithoutPlayer(int[,] cells)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.tag != "Player") Destroy(transform.GetChild(i).gameObject);
        }
        transform.position = new Vector2(0, 0);
        targetsCount = 0;
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int n = cells.GetLength(1) - 1; n >= 0; n--)
            {
                if (cells[i, n] == 0) { Instantiate(wallPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if (cells[i, n] == 1) { Instantiate(boxPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if (cells[i, n] == 2) { Instantiate(targetPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); targetsCount++; }
                else if (cells[i, n] == 3) { Instantiate(emptyPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if (cells[i, n] == 4) { Instantiate(emptyPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
                else if (cells[i, n] == 6) { Instantiate(boxPlacedPrefab, new Vector3(i, n, 0), Quaternion.Euler(0, 0, 0), transform); }
            }
        }
        transform.position = new Vector2(levelPosInit.x, levelPosInit.y);

        if (targetsCount == 0) LevelComplete();
    }

    public bool MovePlayer(Vector3 oldPos, Vector3 newPos)
    {
        var posPlayer = new Vector2(newPos.x - levelPosInit.x, newPos.y - levelPosInit.y);
        var dir = oldPos - newPos;
        var forwardCell = new Vector2(posPlayer.x - dir.x, posPlayer.y - dir.y);
        try
        {
            if (levelCells[(int)posPlayer.x, (int)posPlayer.y] == 0)
            {
                return false;
            }
            else if (levelCells[(int)posPlayer.x, (int)posPlayer.y] == 1)
            {
                if(levelCells[(int)forwardCell.x, (int)forwardCell.y] == 0)
                {
                    return false;
                }
                else if (levelCells[(int)forwardCell.x, (int)forwardCell.y] == 1)
                {
                    if(levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] == 0)
                    {
                        return false;
                    }
                    else if (levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] == 1)
                    {
                        if (levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] == 0)
                        {
                            return false;
                        }
                        else if (levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] == 1)
                        {
                            if (levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y - (int)dir.y] == 0)
                            {
                                return false;
                            }
                            else
                            {
                                gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                                gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                                return true;
                            }
                        }
                        else if (levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] == 3 || levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] == 4)
                        {
                            levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] = 1;
                            levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] = 1;
                            levelCells[(int)forwardCell.x, (int)forwardCell.y] = 1;
                            levelCells[(int)posPlayer.x, (int)posPlayer.y] = 3;
                            DrawLevelToPlayWithoutPlayer(levelCells);
                            gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                            gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                            return true;
                        }
                        else if (levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] == 2)
                        {
                            levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] = 6;
                            levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] = 1;
                            levelCells[(int)forwardCell.x, (int)forwardCell.y] = 1;
                            levelCells[(int)posPlayer.x, (int)posPlayer.y] = 3;
                            DrawLevelToPlayWithoutPlayer(levelCells);
                            gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                            gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                            return true;
                        }
                        else if (levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] == 6)
                        {
                            levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] = 1;
                            levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] = 6;
                            levelCells[(int)forwardCell.x, (int)forwardCell.y] = 1;
                            levelCells[(int)posPlayer.x, (int)posPlayer.y] = 3;
                            DrawLevelToPlayWithoutPlayer(levelCells);
                            gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                            gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                            return true;
                        }
                    }
                    else if (levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] == 3 || levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] == 4)
                    {
                        levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] = 1;
                        levelCells[(int)forwardCell.x, (int)forwardCell.y] = 1;
                        levelCells[(int)posPlayer.x, (int)posPlayer.y] = 3;
                        DrawLevelToPlayWithoutPlayer(levelCells);
                        gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                        gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                        return true;
                    }
                    else if (levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] == 2)
                    {
                        levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] = 6;
                        levelCells[(int)forwardCell.x, (int)forwardCell.y] = 1;
                        levelCells[(int)posPlayer.x, (int)posPlayer.y] = 3;
                        DrawLevelToPlayWithoutPlayer(levelCells);
                        gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                        gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                        return true;
                    }
                    else if (levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] == 6)
                    {
                        if (levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] == 0) return false;
                        else
                        {
                            levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] = 1;
                            levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] = 6;
                            levelCells[(int)forwardCell.x, (int)forwardCell.y] = 1;
                            levelCells[(int)posPlayer.x, (int)posPlayer.y] = 3;
                            DrawLevelToPlayWithoutPlayer(levelCells);
                            gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                            gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                            return true;
                        }
                    }
                    gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                    gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                    return true;
                }
                else if (levelCells[(int)forwardCell.x, (int)forwardCell.y] == 2)
                {
                    levelCells[(int)forwardCell.x, (int)forwardCell.y] = 6;
                    levelCells[(int)posPlayer.x, (int)posPlayer.y] = 3;
                    DrawLevelToPlayWithoutPlayer(levelCells);
                    gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                    gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                    return true;
                }
                else if (levelCells[(int)forwardCell.x, (int)forwardCell.y] == 6)
                {
                    if (levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] == 0) return false;
                    else
                    {
                        levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] = 1;
                        levelCells[(int)forwardCell.x, (int)forwardCell.y] = 6;
                        levelCells[(int)posPlayer.x, (int)posPlayer.y] = 3;
                        DrawLevelToPlayWithoutPlayer(levelCells);
                        gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                        gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                        return true;
                    }
                }
                else
                {
                    levelCells[(int)forwardCell.x, (int)forwardCell.y] = 1;
                    levelCells[(int)posPlayer.x, (int)posPlayer.y] = 3;
                    DrawLevelToPlayWithoutPlayer(levelCells);
                    gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                    gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                    return true;
                }
            }
            else if (levelCells[(int)posPlayer.x, (int)posPlayer.y] == 6)
            {
                if (levelCells[(int)forwardCell.x, (int)forwardCell.y] == 0) return false;
                else if (levelCells[(int)forwardCell.x, (int)forwardCell.y] == 3 || levelCells[(int)forwardCell.x, (int)forwardCell.y] == 4)
                {
                    levelCells[(int)forwardCell.x, (int)forwardCell.y] = 1;
                    levelCells[(int)posPlayer.x, (int)posPlayer.y] = 2;
                    DrawLevelToPlayWithoutPlayer(levelCells);
                    gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                    gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                    return true;
                }
                else if (levelCells[(int)forwardCell.x, (int)forwardCell.y] == 1)
                {
                    if (levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] == 0) return false;
                    else
                    {
                        levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] = 1;
                        levelCells[(int)forwardCell.x, (int)forwardCell.y] = 1;
                        levelCells[(int)posPlayer.x, (int)posPlayer.y] = 2;
                        DrawLevelToPlayWithoutPlayer(levelCells);
                        gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                        gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                        return true;
                    }
                }
                else if (levelCells[(int)forwardCell.x, (int)forwardCell.y] == 2)
                {
                    levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] = 3;
                    levelCells[(int)forwardCell.x, (int)forwardCell.y] = 6;
                    levelCells[(int)posPlayer.x, (int)posPlayer.y] = 2;
                    DrawLevelToPlayWithoutPlayer(levelCells);
                    gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                    gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                    return true;
                }
                else if (levelCells[(int)forwardCell.x, (int)forwardCell.y] == 6)
                {
                    if (levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] == 0) return false;
                    else
                    {
                        levelCells[(int)forwardCell.x - (int)dir.x - (int)dir.x, (int)forwardCell.y - (int)dir.y - (int)dir.y] = 3;
                        levelCells[(int)forwardCell.x - (int)dir.x, (int)forwardCell.y - (int)dir.y] = 6;
                        levelCells[(int)forwardCell.x, (int)forwardCell.y] = 2;
                        levelCells[(int)posPlayer.x, (int)posPlayer.y] = 3;
                        DrawLevelToPlayWithoutPlayer(levelCells);
                        gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                        gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                        return true;
                    }
                }
                gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                gameController.GetComponent<UIScriptController>().PlusPlayerPushes();
                return true;
            }
            else
            {
                DrawLevelToPlayWithoutPlayer(levelCells);
                gameController.GetComponent<UIScriptController>().PlusPlayerMove();
                return true;
            }
        }
        catch (System.Exception)
        {
            return false;
            throw;
        }
    }

    private void LevelComplete()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().activatePlayer = false;

        string path = "Assets/Resources/Level" + PlayerPrefs.GetInt("Level", 0) + ".txt";

        StreamReader reader = new StreamReader(path);
        var completed = reader.ReadLine();
        var Rows = int.Parse(reader.ReadLine());
        var Cols = int.Parse(reader.ReadLine());
        var cells = new int[Rows, Cols];
        for (int i = 0; i < Rows; i++)
        {
            for (int n = 0; n < Cols; n++)
            {
                cells[i, n] = int.Parse(reader.ReadLine());
            }
        }
        reader.Close();

        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine("Completed");
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

        gameController.GetComponent<UIScriptController>().GameCompleted();
        AssetDatabase.ImportAsset(path);
        AssetDatabase.Refresh();
        Invoke("GoToMenu", 2f);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }
}
