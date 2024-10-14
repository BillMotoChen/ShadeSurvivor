using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public static Transition instance;
    public GameObject transition;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SceneTransition(string sceneName)
    {
        StartCoroutine(WaitForTransition(sceneName));
    }

    IEnumerator WaitForTransition(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        transition.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);

        transition.SetActive(false);

        Time.timeScale = 1.0f;
    }
}
