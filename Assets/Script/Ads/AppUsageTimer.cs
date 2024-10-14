using UnityEngine;
using UnityEngine.SceneManagement;

public class AppUsageTimer : MonoBehaviour
{
    public static AppUsageTimer Instance { get; private set; }
    private float appUsageTime = 0f;
    private float adInterval = 210f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        appUsageTime += Time.unscaledDeltaTime;
    }

    public void playInterstitial()
    {
        if (appUsageTime >= adInterval)
        {
            appUsageTime = 0f;
            UnityAdsManager.Instance.ShowInterstitialAd();
        }
    }
}