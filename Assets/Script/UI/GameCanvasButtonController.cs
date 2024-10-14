using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCanvasButtonController : MonoBehaviour
{
    private GameStatus gameStatus;

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Main")
        {
            gameStatus = GameObject.Find("Canvas/GameStatus").GetComponent<GameStatus>();
        }
    }

    public void Retry()
    {
        gameStatus.updateBestScore();
        string currentScene = SceneManager.GetActiveScene().name;
        Transition.instance.SceneTransition(currentScene);
    }

    public void ToMainMenu()
    {
        gameStatus.updateBestScore();
        AppUsageTimer.Instance.playInterstitial();
        Transition.instance.SceneTransition("TopMenu");
    }   

    public void PlayGame()
    {
        Transition.instance.SceneTransition("Main");
    }

    public void AttemptRevive()
    {
        // Trigger a rewarded ad to revive the player
        UnityAdsManager.Instance.ShowRewardedAdForRevive();
    }

    public void Revive()
    {
        gameStatus.playable = true;
        gameStatus.time = Mathf.Max(gameStatus.time, 30f);
        if (gameStatus.lives < 2)
        {
            gameStatus.liveUpdate(2 - gameStatus.lives);
        }
    
        gameStatus.gameOverPanel.SetActive(false);
        GameObject[] marbles = GameObject.FindGameObjectsWithTag("Marble");
        foreach (GameObject marble in marbles)
        {
            Destroy(marble);
        }
        Time.timeScale = 1f;
    }

}
