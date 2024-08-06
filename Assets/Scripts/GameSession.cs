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
    private FirebaseApp app;

    public UnityEvent LevelUpdateEvent;
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

    // Start is called before the first frame update
    void Start()
    {
        InitFirebase();
        highScoreText.text = PlayerPrefs.GetInt(StaticUrlScript.highScore).ToString();

        //adsInstance = FindObjectOfType<RewardedAds>();
        LevelUpdateEvent.AddListener(LevelUpdate);
        AddingListeners();

        /// <summary>
        /// Firebase Start Game Event
        /// </summary>
        Firebase.Analytics.FirebaseAnalytics.LogEvent(StaticUrlScript.StartGame_Firebase);
    }

    /// <summary>
    /// Init Firebase 
    /// </summary>
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
        _retryBtn.onClick.RemoveAllListeners();
        _retryBtn.onClick.AddListener(() => { ResetGameLevel(); });

        _resumeBtn.onClick.RemoveAllListeners();
        _resumeBtn.onClick.AddListener(() => { ResumeGame(); });

        _pauseBtn.onClick.RemoveAllListeners();
        _pauseBtn.onClick.AddListener(() => { PauseGame(); });
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

    IEnumerator AdLoadTimer()
    {
        int t = 3;
        while (t > 0)
        {
            t--;
            adLoadTimer.text = t.ToString();
            yield return new WaitForSeconds(1);
        }

        Ball.hasStarted = false;
        // rewarded.ShowAd();
        //  enableButton.SetActive(true);
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
        if (LoseCollider.checkResumeEligiblity)
        {
            Utility.myLog("Resume Game");
            //Ball.instance.StartCoroutine(AdLoadTimer());
            //adsInstance.ShowAd();
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
        Ball.hasStarted = false;
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

    public void GameOver()
    {
        Destroy(gameObject);
    }

    public bool IsAutoPlayEnabled()
    {
        return isAutoPlayEnabled;
    }

    public void EnableGameOverPnl()
    {
        gameOverPanel?.SetActive(true);
    }
}
