using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScriptController : MonoBehaviour
{
    public TextMeshProUGUI movesTxt;
    public TextMeshProUGUI pushesTxt;
    public TextMeshProUGUI timerTxt;
    public TextMeshProUGUI levelNumber;
    public GameObject CompletedText;

    private int moves;
    private int pushes;
    private float seconds;
    private int minutes;
    private int hours;

    void Start()
    {
        CompletedText.SetActive(false);
    }

    void Update()
    {
        seconds += Time.deltaTime;
        if (seconds >= 60)
        {
            seconds = 0;
            minutes++;
        }
        if (minutes >= 60)
        {
            minutes = 0;
            hours++;
        }
        timerTxt.text = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("F1");
    }

    public void SetLevelNumber(int levelNum)
    {
        levelNumber.text = levelNum.ToString();
    }

    public void PlusPlayerMove()
    {
        moves++;
        movesTxt.text = "Moves: "+moves.ToString();
    }

    public void PlusPlayerPushes()
    {
        pushes++;
        pushesTxt.text = "Pushes: "+pushes.ToString();
    }

    public void GameCompleted()
    {
        CompletedText.SetActive(true);
    }
}
