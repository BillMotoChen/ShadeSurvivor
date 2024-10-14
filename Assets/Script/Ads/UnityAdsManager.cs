using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour, IUnityAdsShowListener, IUnityAdsLoadListener
{
    public static UnityAdsManager Instance { get; private set; }

    private bool _isReviveRequested = false;

    private string _gameId;
    private string _rewardedAdUnitId;
    private string _interstitialAdUnitId;
    private string _bannerAdUnitId;

    // Ad Unit IDs for Android
    private string _androidGameId = "5712025";
    private string _androidRewardedAdUnitId = "Rewarded_Android";
    private string _androidInterstitialAdUnitId = "Interstitial_Android";
    private string _androidBannerAdUnitId = "Banner_Android";

    // Ad Unit IDs for iOS
    private string _iosGameId = "5712024";
    private string _iosRewardedAdUnitId = "Rewarded_iOS";
    private string _iosInterstitialAdUnitId = "Interstitial_iOS";
    private string _iosBannerAdUnitId = "Banner_iOS";

    private bool _testMode = false;  // Set to false when you are ready to go live

    // Flags to track if ads are loaded
    private bool _isRewardedAdLoaded = false;
    private bool _isInterstitialAdLoaded = false;

    private void Awake()
    {
        // Singleton pattern enforcement
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Configure Game ID and Ad Unit IDs based on the platform
        ConfigurePlatformAds();
    }

    private void ConfigurePlatformAds()
    {
#if UNITY_ANDROID
        _gameId = _androidGameId;
        _rewardedAdUnitId = _androidRewardedAdUnitId;
        _interstitialAdUnitId = _androidInterstitialAdUnitId;
        _bannerAdUnitId = _androidBannerAdUnitId;
#elif UNITY_IOS
        _gameId = _iosGameId;
        _rewardedAdUnitId = _iosRewardedAdUnitId;
        _interstitialAdUnitId = _iosInterstitialAdUnitId;
        _bannerAdUnitId = _iosBannerAdUnitId;
#else
        Debug.LogError("Unsupported platform for Unity Ads.");
#endif
    }

    private void Start()
    {
        // Initialize Unity Ads with the correct game ID based on the platform
        Advertisement.Initialize(_gameId, _testMode);

        // Preload ads on app startup
        LoadInterstitialAd();
        LoadRewardedAd();
    }

    // Preload Interstitial Ad
    private void LoadInterstitialAd()
    {
        _isInterstitialAdLoaded = false;  // Reset flag before loading
        Advertisement.Load(_interstitialAdUnitId, this);
    }

    // Preload Rewarded Ad
    private void LoadRewardedAd()
    {
        _isRewardedAdLoaded = false;  // Reset flag before loading
        Advertisement.Load(_rewardedAdUnitId, this);
    }

    // Show the rewarded ad for reviving the player
    public void ShowRewardedAdForRevive()
    {
        if (_isRewardedAdLoaded)
        {
            _isReviveRequested = true;
            Advertisement.Show(_rewardedAdUnitId, this);
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet. Loading again...");
            LoadRewardedAd(); // Load it again if not ready
        }
    }

    // Handle when the rewarded ad finishes (called by Unity Ads SDK)
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(_rewardedAdUnitId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            if (_isReviveRequested)
            {
                // Player successfully watched the ad for reviving, call Revive
                GameCanvasButtonController controller = GameObject.Find("GameCanvasButtonController").GetComponent<GameCanvasButtonController>();
                controller.Revive();
                _isReviveRequested = false;  // Reset the flag after revive
            }

            // Preload the next rewarded ad
            LoadRewardedAd();
        }
        else if (placementId.Equals(_interstitialAdUnitId))
        {
            Time.timeScale = 1f;
            Debug.Log("Interstitial ad completed.");
            // Preload the next interstitial ad
            LoadInterstitialAd();
        }
    }

    // Show Interstitial Ad
    public void ShowInterstitialAd()
    {
        if (_isInterstitialAdLoaded)
        {
            Time.timeScale = 0f;
            Advertisement.Show(_interstitialAdUnitId, this);
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet. Loading again...");
            LoadInterstitialAd(); // Load it again if not ready
        }
    }

    // Show Banner Ad
    public void ShowBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(_bannerAdUnitId);
    }

    // Hide Banner Ad
    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    // Implement IUnityAdsLoadListener interface methods
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad {placementId} loaded successfully.");
        // Set the flags when the ads are loaded
        if (placementId.Equals(_interstitialAdUnitId))
        {
            _isInterstitialAdLoaded = true;
        }
        else if (placementId.Equals(_rewardedAdUnitId))
        {
            _isRewardedAdLoaded = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load ad {placementId}: {message}");
    }

    // Implement IUnityAdsShowListener interface methods
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Failed to show ad {placementId}: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Ad {placementId} started showing.");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Ad {placementId} was clicked.");
    }
}