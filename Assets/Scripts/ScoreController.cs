using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController
{
    public const int  gameDuration = 15;

    public int redTeamScore;
    public int blueTeamScore;
    public float gameStart; 
    public int secondsRemaining;

    private TextMeshProUGUI timerDisplay;
    private TextMeshProUGUI redScoreDisplay;
    private TextMeshProUGUI blueScoreDisplay;

    public ScoreController()
    {
        ServicesLocator.eventManager.Register<GoalScored>(IncrementTeamScore);
        timerDisplay = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
        redScoreDisplay = GameObject.Find("Red Team Score").GetComponent<TextMeshProUGUI>();
        blueScoreDisplay = GameObject.Find("Blue Team Score").GetComponent<TextMeshProUGUI>();
    }



    public void ShowScores()
    {
        redScoreDisplay.gameObject.SetActive(true);
        blueScoreDisplay.gameObject.SetActive(true);
        timerDisplay.gameObject.SetActive(true);
    }

    public void UpdateDisplays()
    {
        timerDisplay.text = secondsRemaining.ToString();
        redScoreDisplay.text = redTeamScore.ToString();
        blueScoreDisplay.text = blueTeamScore.ToString();

    }


    public void IncrementTeamScore(AGPEvent e)
    {
        GoalScored goalScoreEvent = (GoalScored)e;
        if (goalScoreEvent.didRedTeamScore) redTeamScore++;
        else blueTeamScore++;
    }


}
