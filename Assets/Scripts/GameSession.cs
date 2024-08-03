using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

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
        highScoreText.text = PlayerPrefs.GetInt(StaticUrlScript.highScore).ToString();

        //adsInstance = FindObjectOfType<RewardedAds>();
        LevelUpdateEvent.AddListener(LevelUpdate);
        AddingListeners();

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

    private void LevelUpdate()
    {
        displayLevel.text = "Level: " + PlayerPrefs.GetInt(StaticUrlScript.currentLevel).ToString();
    }
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

    public void ResetGameLevel()
    {
        Utility.myLog("Game Level  Resetted!");
        gameOverPanel?.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LoseCollider.checkResumeEligiblity = true;
        Ball.hasStarted = false;
    }
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

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(StaticUrlScript.highScore);
        highScoreText.text = "0";
    }

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
