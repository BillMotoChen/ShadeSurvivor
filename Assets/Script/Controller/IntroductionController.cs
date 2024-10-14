using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionController : MonoBehaviour
{
    public GameObject introduction;
    public GameObject top10;

    public void introductionOn()
    {
        introduction.SetActive(true);
    }

    public void introductionOff()
    {
        introduction.SetActive(false);
    }

    public void top10On()
    {
        top10.SetActive(true);
        MainMenuUI mainMenuUI = GameObject.Find("MainMenuUI").GetComponent<MainMenuUI>();
        mainMenuUI.updateTop10();
    }

    public void top10Off()
    {
        top10.SetActive(false);
    }
}
