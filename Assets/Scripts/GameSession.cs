using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
using Firebase.Extensions;
using Firebase;
using Firebase.Analytics;

public class GameSession : MonoBehaviour
{
    [Range(0.1f, 10f)][SerializeField] float gameSpeed = 1f;
    [SerializeField] int pointPerBlockDestroyed = 10;
    [SerializeField] TextMeshProUGUI currentScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;

    [SerializeField] TextMeshProUGUI displayLevel;
    [SerializeField] bool isAutoPlayEnabled;

    [SerializeField] private Sprite[] buttonSprite;
    [SerializeField] private Image toggleImage;
    [SerializeField] int currentScore = 0;

    private bool isPaused = false;
    public TMP_Text _adText;

    [SerializeField] private Button _retryBtn, _resumeBtn, _pauseBtn, _quitBtn;

    //RewardedAds adsInstance;
    [SerializeField] public GameObject gameOverPanel;


    //private bool _isResumeValid = true;
    #region Unity Singleton
    private static GameSession instance;
    public static GameSession Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameSession>();
            }
            return instance;
        }
        set { instance = value; }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        LoaderManager.Instance.DisableLoader();
    }
    #endregion

    #region Unity & Action Events
    public UnityEvent LevelUpdateEvent;

    public Action StartGameAction;
    #endregion

    #region Firebase Init & Messaging
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }

    void InitFirebase(Action OnCompleteEvent)
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                OnCompleteEvent.Invoke();
                Utility.myLog("Firebase Initilized!");
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }

    void FirebaseEvents()
    {
        /// <summary>
        /// Firebase Start Game Event
        /// </summary>
        FirebaseAnalytics.LogEvent(StaticUrlScript.StartGame_Firebase);

        //Firebase Messaging Events
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }
    #endregion

    private void OnEnable()
    {
        StartGameAction += AddingListeners;
        StartGameAction += FirebaseEvents;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitFirebase(StartGameAction);

        highScoreText.text = PlayerPrefs.GetInt(StaticUrlScript.highScore).ToString();
        LevelUpdateEvent.AddListener(LevelUpdate);
    }

    void AddingListeners()
    {

        _retryBtn.onClick.AddListener(() =>
        {
            ResetGameLevel();
            Utility.myLog("Retry button listener added");
        });


        _resumeBtn.onClick.AddListener(() =>
        {
            ShowResumeAd(AdsController.Instance.IsRewardedAdReady, StaticUrlScript.RewardedAdUnitId);
        });


        _pauseBtn.onClick.AddListener(() =>
        {
            PauseGame();
        });

        _quitBtn.onClick.AddListener(() =>
        {
            ExitGame();
        });

        // Optional: Log to confirm listeners are being added
        Debug.Log("Listeners have been added to buttons");
    }
    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            Time.timeScale = gameSpeed;
        }
        else
        {
            Time.timeScale = 0f;
        }

    }

    void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Updating Level counter on every time LevelUpdateEvent Fires
    /// </summary>
    private void LevelUpdate()
    {
        displayLevel.text = "Level: " + PlayerPrefs.GetInt(StaticUrlScript.currentLevel).ToString();
    }

    void ShowResumeAd(bool status, string _adUnitId)
    {
        _resumeBtn.interactable = status;
        Utility.myLog("Status of Ads -" + status);

        if (status)
        {
            AdsController.Instance.OnAdClosedEvent.AddListener(() => { ResumeGame(); });
            AdsController.Instance.OnAdFailedEvent.AddListener(() => { ResumeGame(); });
            _adText.text = "Watch Ad";
            AdsController.Instance.ShowAd(_adUnitId);
        }
        else
        {
            _adText.text = "No Ads";
        }

    }

    /// <summary>
    /// Simple Resume Game if user is died b/w game.
    /// </summary>
    public void ResumeGame()
    {
        if (Ball.instance == null)
        {
            Debug.LogError("Ball.instance is null!");
            return; // Exit if instance is not found
        }

        Ball.instance.HasStarted = false;
        gameOverPanel?.SetActive(false);
        Utility.myLog("Game Resume");
        //if (_isResumeValid)
        //{
        //    _isResumeValid = false;
        //}
    }

    /// <summary>
    /// Reset Game level.
    /// </summary>
    public void ResetGameLevel()
    {
        Utility.myLog("Game Level  Resetted!");
        gameOverPanel?.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //_isResumeValid = true;
        Ball.instance.HasStarted = false;
    }

    /// <summary>
    /// Calculating Game Score
    /// </summary>
    public void AddToScore()
    {
        currentScore += pointPerBlockDestroyed;
        currentScoreText.text = currentScore.ToString();
        if (currentScore > PlayerPrefs.GetInt(StaticUrlScript.highScore))
        {
            PlayerPrefs.SetInt(StaticUrlScript.highScore, currentScore);
        }

        //        scoreText.text = currentScore.ToString();
    }

    /// <summary>
    /// Deletes the highScore playerprefs.
    /// </summary>
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(StaticUrlScript.highScore);
        highScoreText.text = "0";
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Simple pause game and change Sprites
    /// </summary>
    public void PauseGame()
    {
        if (isPaused)
        {
            toggleImage.sprite = buttonSprite[0];
            isPaused = false;
        }
        else
        {
            toggleImage.sprite = buttonSprite[1];
            isPaused = true;
        }
    }

    public bool IsAutoPlayEnabled()
    {
        return isAutoPlayEnabled;
    }

    public void EnableGameOverPnl()
    {
        //if (!_isResumeValid)
        //{
        //    SceneManager.LoadSceneAsync(StaticUrlScript.GameOver);
        //    return;
        //}
        gameOverPanel?.SetActive(true);
        LoaderManager.Instance.DisableLoader();
    }

    private void OnDisable()
    {
        StartGameAction -= AddingListeners;
        StartGameAction -= FirebaseEvents;
        StartGameAction = null;
    }
}
