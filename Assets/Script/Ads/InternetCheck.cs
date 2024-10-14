using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InternetCheck : MonoBehaviour
{
    public GameObject noInternetPanel;
    public Button retryButton;

    private bool isInternetAvailable = false;

    private void Start()
    {
        noInternetPanel.SetActive(false);

        CheckInternetConnection();

        retryButton.onClick.AddListener(CheckInternetConnection);
    }

    private void CheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {   
            isInternetAvailable = false;
            noInternetPanel.SetActive(true);
        }
        else
        {
            isInternetAvailable = true;
            noInternetPanel.SetActive(false);
        }
    }

    public void StartApp()
    {
        if (isInternetAvailable)
        {
            SceneManager.LoadScene("TopMenu");
        }
    }
}