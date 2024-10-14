using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStatus : MonoBehaviour
{
    public bool playable;

    public TMP_Text scoreText, timeText, lifeText;
    public GameObject gameOverPanel;

    private int score = 0;
    public float time = 90.0f;
    public int lives = 5;

    public float totalTime;

    MainGameUI mainGameUI;

    private void Awake()
    {
        scoreText.text = "Score: " + score.ToString();
        timeText.text = timeFormat(time);
        lifeText.text = lives.ToString();
        playable = true;
    }
    void Start()
    {
        mainGameUI = GameObject.Find("MainGameUI").GetComponent<MainGameUI>();
        totalTime = 0f;
    }

    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            totalTime += Time.deltaTime;
            timeText.text = timeFormat(time);
        }
        else
        {
            time = 0;
            if (playable)
            {
                GameOver();
            }
            timeText.text = timeFormat(time);
        }
    }

    private string timeFormat(float time)
    {
        return time.ToString("F1");
    }

    public void timeUpdate(float extraTime)
    {
        time += extraTime;
        timeText.text = timeFormat(time);
    }

    public void scoreUpdate(int scoreChange)
    {
        score += scoreChange;
        scoreText.text = "Score: " + score.ToString();
    }

    public void liveUpdate(int lifeChange)
    {
        lives += lifeChange;
        lifeText.text = lives.ToString();
        if (lives == 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        playable = false;
        gameOverPanel.SetActive(true);
        mainGameUI.updateScoreText(score, SaveScript.bestScore);
        mainGameUI.pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void updateBestScore()
    {
        SaveScript saveScriptInstance = GameObject.Find("SaveData").GetComponent<SaveScript>();
        saveScriptInstance.UpdateTop10Scores(score);
        if (score > SaveScript.bestScore)
        {
            SaveScript.bestScore = score;
            saveScriptInstance.SaveData();
        }
    }
}
