using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

public class AdsController : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    string _adUnitId;
    private Dictionary<string, bool> _isAdLoaded = new Dictionary<string, bool>();

    // UnityAd Events
    public UnityEvent OnAdClosedEvent, OnAdFailedEvent;

    #region Unity Singleton
    private static AdsController _instance;
    public static AdsController Instance
    {
        get
        {
            if (_instance == null)
                FindObjectOfType<AdsController>();

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region SDK Init
    private Action SDKInitEvent;
    private void Start()
    {
        SDKInitEvent = () =>
        {
            //LoadAd(StaticUrlScript.InterstitialAdUnit);
            LoadAd(StaticUrlScript.RewardedAdUnitId);
        };

        InitializeAds();
    }
    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        SDKInitEvent?.Invoke();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    #endregion


    //void Awake()
    //{
    //    // Get the Ad Unit ID for the current platform:
    //    _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
    //        ? _iOsAdUnitId
    //        : _androidAdUnitId;
    //}

    // Load content to the Ad Unit:
    public void LoadAd(string _adUnitId)
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
        _isAdLoaded[_adUnitId] = false;
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd(string _adUnitId)
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + _adUnitId);
        Advertisement.Show(_adUnitId, this);
        _isAdLoaded[_adUnitId] = false;
    }

    // Checking if Ad is Ready to Show.
    public bool IsInterstitialAdReady
    {
        get
        {
            if (_isAdLoaded.ContainsKey(StaticUrlScript.InterstitialAdUnit))
                return _isAdLoaded[StaticUrlScript.InterstitialAdUnit];
            else
                return false; // Return false if the key doesn't exist
        }
    }

    public bool IsRewardedAdReady
    {
        get
        {
            if (_isAdLoaded.ContainsKey(StaticUrlScript.RewardedAdUnitId))
                return _isAdLoaded[StaticUrlScript.RewardedAdUnitId];
            else
                return false;
        }
    }


    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        _isAdLoaded[adUnitId] = true;
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
        _isAdLoaded[_adUnitId] = false;
        LoadAd(_adUnitId);
        OnAdFailedEvent.Invoke();
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }
    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        _isAdLoaded[_adUnitId] = false;
        LoadAd(_adUnitId);
        OnAdClosedEvent.Invoke();
    }

}
