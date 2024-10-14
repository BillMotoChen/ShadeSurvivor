using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    private int bestScore;
    public TMP_Text bestScoreText;
    public TMP_Text top10ScoresText;

    // Start is called before the first frame update
    void Start()
    {
        UnityAdsManager.Instance.ShowBannerAd();
        SaveScript.instance.LoadData();
        bestScore = SaveScript.bestScore;
        bestScoreText.text = bestScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateTop10()
    {
        List<int> top10Scores = SaveScript.top10Scores;

        string numList = "";
        foreach (int score in top10Scores)
        {
            numList += score.ToString() + "\n";
        }

        top10ScoresText.text = numList;
    }
}
