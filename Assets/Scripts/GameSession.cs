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
    public TMP_Text adLoadTimer;

    [SerializeField] private Button _retryBtn, _resumeBtn, _pauseBtn;

    //RewardedAds adsInstance;
    [SerializeField] public GameObject gameOverPanel;

    FirebaseApp app;

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

    private void OnEnable()
    {
        StartGameAction += InitFirebase;
        StartGameAction += AddingListeners;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartGameAction?.Invoke();
        
        highScoreText.text = PlayerPrefs.GetInt(StaticUrlScript.highScore).ToString();
        LevelUpdateEvent.AddListener(LevelUpdate);

        /// <summary>
        /// Firebase Start Game Event
        /// </summary>
        FirebaseAnalytics.LogEvent(StaticUrlScript.StartGame_Firebase);
    }

    void InitFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

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
    void AddingListeners()
    {

        _retryBtn.onClick.AddListener(() =>
        {
            ResetGameLevel();
            Utility.myLog("Retry button listener added");
        });


        _resumeBtn.onClick.AddListener(() =>
        {
            ResumeGame();
        });


        _pauseBtn.onClick.AddListener(() =>
        {
            PauseGame();
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

    /// <summary>
    /// Updating Level counter on every time LevelUpdateEvent Fires
    /// </summary>
    private void LevelUpdate()
    {
        displayLevel.text = "Level: " + PlayerPrefs.GetInt(StaticUrlScript.currentLevel).ToString();
    }

    /// <summary>
    /// Simple Resume Game if user is died b/w game.
    /// </summary>
    public void ResumeGame()
    {
        gameOverPanel?.SetActive(false);
        Ball.instance.HasStarted = false;
        if (LoseCollider.checkResumeEligiblity)
        {
            Utility.myLog("Resume Game");
            LoseCollider.checkResumeEligiblity = false;
        }
    }

    /// <summary>
    /// Reset Game level.
    /// </summary>
    public void ResetGameLevel()
    {
        Utility.myLog("Game Level  Resetted!");
        gameOverPanel?.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LoseCollider.checkResumeEligiblity = true;
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
        gameOverPanel?.SetActive(true);
    }

    private void OnDisable()
    {
        StartGameAction -= AddingListeners;
        StartGameAction -= InitFirebase;
    }

}
