using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainGameUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text pauseScoreText;

    public GameObject pauseButton;
    public GameObject pauseMenu;

    public void updateScoreText(int score, int bestScore)
    {
        scoreText.text = "SCORE: " + score.ToString();
        scoreText.text += "\nBEST: " + bestScore.ToString();
    }

    public void pauseMenuOn()
    {
        Time.timeScale = 0f;
        pauseScoreText.text = "BEST: " + SaveScript.bestScore.ToString();
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
    }


    public void pauseMenuOff()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }
}
