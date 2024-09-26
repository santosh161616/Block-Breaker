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

    [SerializeField] private Button _retryBtn, _resumeBtn, _pauseBtn, _quitBtn, _onYes, _onNo;

    //RewardedAds adsInstance;
    [SerializeField] public GameObject gameOverPanel, resultPanel, gameSessionHead, exitPanel;


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

    public void SetScoreBar()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Index -" + index);
        if (index == 0)
            gameSessionHead.SetActive(false);
        else gameSessionHead.SetActive(true);
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
            LoaderManager.Instance.EnableLoader();
            ExitGame();
            gameOverPanel?.SetActive(false);
        });

        ExitGameAction();
        // Optional: Log to confirm listeners are being added
        Debug.Log("Listeners have been added to buttons");
    }

    void ExitGameAction()
    {
        _onYes.onClick.RemoveAllListeners();
        _onYes.onClick.AddListener(() => { YesBtnTask(); FirebaseAnalytics.LogEvent(StaticUrlScript.GameExit_Firebase); });

        _onNo.onClick.RemoveAllListeners();
        _onNo.onClick.AddListener(() => { ClosePanels(false); });
    }

    void YesBtnTask()
    {
        LoaderManager.Instance.EnableLoader();
        ExitGame();
        ClosePanels(false);
        gameOverPanel.SetActive(false);
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

        //Check for Exit Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            if (index != 0)
            {
                ClosePanels(true);
            }
            else
            {
                //ApplicationC losed
                Utility.myLog(">>>> Application Closed <<<< ");
                Application.Quit();
            }
        }
    }

    void ClosePanels(bool status)
    {
        exitPanel?.SetActive(status);
    }

    void ExitGame()
    {
        SceneManager.LoadSceneAsync(StaticUrlScript.Dashboard);
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
            AdsController.Instance.OnAdClosedEvent.RemoveAllListeners();
            AdsController.Instance.OnAdFailedEvent.RemoveAllListeners();
            AdsController.Instance.OnAdClosedEvent.AddListener(() => { ResumeGame(); });
            AdsController.Instance.OnAdFailedEvent.AddListener(() => { ResumeGame(); });

            FirebaseAnalytics.LogEvent(StaticUrlScript.RewardedAd_Firebase);
            AdsController.Instance.ShowAd(_adUnitId);
        }
        else
        {
            _adText.text = "No Ads";
        }

    }

    private void UpdateResumeButtonStatus()
    {
        bool status = AdsController.Instance.IsRewardedAdReady;
        _resumeBtn.interactable = status;

        if (status)
            _adText.text = "Watch Ad";
        else
            _adText.text = "No Ads";
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
        LoaderManager.Instance.EnableLoader();
        Utility.myLog("Game Level  Resetted!");
        gameOverPanel?.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //_isResumeValid = true;
        Ball.instance.HasStarted = false;
        FirebaseAnalytics.LogEvent(StaticUrlScript.ResetLevel_Firebase);
    }

    /// <summary>
    /// Calculating Game Score
    /// </summary>
    public void AddToScore()
    {
        currentScore += pointPerBlockDestroyed;
        currentScoreText.text = currentScore.ToString();
        PlayerPrefs.SetInt(StaticUrlScript.currentScore, currentScore);
        if (currentScore > PlayerPrefs.GetInt(StaticUrlScript.highScore))
        {
            PlayerPrefs.SetInt(StaticUrlScript.highScore, currentScore);
        }

        //        scoreText.text = currentScore.ToString();
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
        UpdateResumeButtonStatus();
        gameOverPanel?.SetActive(true);
        LoaderManager.Instance?.DisableLoader();
    }

    public void EnableResultPanel(bool status)
    {
        resultPanel?.SetActive(status);
    }

    private void OnDisable()
    {
        StartGameAction -= AddingListeners;
        StartGameAction -= FirebaseEvents;
        StartGameAction = null;
    }
}
