using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [Header("Interstitial Ad")]
    [SerializeField] string interstitialAndroidAdUnitId = "Interstitial_Android";
    [SerializeField] string interstitialiOSAdUnitId = "Interstitial_iOS";
    private string interstitialAdUnitId;

    [Header("Rewarded Ad")]
    [SerializeField] Button showRewardedAdButton;
    [SerializeField] string rewardedAndroidAdUnitId = "Rewarded_Android";
    [SerializeField] string rewardediOSAdUnitId = "Rewarded_iOS";
    private string rewardedAdUnitId;

    [Header("Banner Ad")]
    [SerializeField] Button loadBannerButton;
    [SerializeField] Button showBannerButton;
    [SerializeField] Button hideBannerButton;
    [SerializeField] BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;
    [SerializeField] string bannerAndroidAdUnitId = "Banner_Android";
    [SerializeField] string banneriOSAdUnitId = "Banner_iOS";
    private string bannerAdUnitId;

    private void Awake()
    {
        // Get Ad Unit IDs based on the current platform:
#if UNITY_IOS
        interstitialAdUnitId = interstitialiOSAdUnitId;
        rewardedAdUnitId = rewardediOSAdUnitId;
        bannerAdUnitId = banneriOSAdUnitId;
#elif UNITY_ANDROID
        interstitialAdUnitId = interstitialAndroidAdUnitId;
        rewardedAdUnitId = rewardedAndroidAdUnitId;
        bannerAdUnitId = bannerAndroidAdUnitId;
#endif

        // Initialize Ads (usually done in another script):
        InitializeAds();
    }

    private void InitializeAds()
    {
        // Initialize Unity Ads here if needed.
        // Advertisement.Initialize(gameId, testMode);
    }

    // Load Interstitial Ad:
    public void LoadInterstitialAd()
    {
        Debug.Log("Loading Interstitial Ad: " + interstitialAdUnitId);
        Advertisement.Load(interstitialAdUnitId, this);
    }

    // Show Interstitial Ad:
    public void ShowInterstitialAd()
    {
        Debug.Log("Showing Interstitial Ad: " + interstitialAdUnitId);
        Advertisement.Show(interstitialAdUnitId, this);
    }

    // Load Rewarded Ad:
    public void LoadRewardedAd()
    {
        Debug.Log("Loading Rewarded Ad: " + rewardedAdUnitId);
        Advertisement.Load(rewardedAdUnitId, this);
    }

    // Show Rewarded Ad:
    public void ShowRewardedAd()
    {
        Debug.Log("Showing Rewarded Ad: " + rewardedAdUnitId);
        Advertisement.Show(rewardedAdUnitId, this);
    }

    // Load Banner Ad:
    public void LoadBannerAd()
    {
        Debug.Log("Loading Banner Ad: " + bannerAdUnitId);

        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(bannerAdUnitId, options);
    }

    // Show Banner Ad:
    public void ShowBannerAd()
    {
        Debug.Log("Showing Banner Ad: " + bannerAdUnitId);

        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        Advertisement.Banner.SetPosition(bannerPosition);
        Advertisement.Banner.Show(bannerAdUnitId, options);
    }

    // Hide Banner Ad:
    public void HideBannerAd()
    {
        Debug.Log("Hiding Banner Ad");
        Advertisement.Banner.Hide();
    }

    // Implement IUnityAdsLoadListener and IUnityAdsShowListener methods:
    public void OnUnityAdsAdLoaded(string adUnitId) { }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) { }
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) { }
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }

    // Implement Banner Ad callbacks:
    private void OnBannerLoaded() { }
    private void OnBannerError(string message) { }
    private void OnBannerClicked() { }
    private void OnBannerShown() { }
    private void OnBannerHidden() { }
}
