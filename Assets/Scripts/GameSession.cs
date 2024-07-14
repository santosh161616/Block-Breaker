using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    //State variables.
    public static GameSession instance;

    RewardedAds adsInstance;
    public GameObject enableResume;
    public GameObject tryAgain;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        int gameStatusCount = FindObjectsOfType<GameSession>().Length;
        if (gameStatusCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        displayLevel.text = "Level: " + PlayerPrefs.GetInt("CurrentLevel").ToString();
        adsInstance = FindObjectOfType<RewardedAds>();
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

    public void ResumeGame()
    {
        if (LoseCollider.checkResumeEligiblity)
        {
            Ball.instance.StartCoroutine(AdLoadTimer());
            adsInstance.ShowAd();
            LoseCollider.checkResumeEligiblity = false;
        }
    }

    public void ResetGameLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LoseCollider.checkResumeEligiblity = true;
        Ball.hasStarted = false;
    }
    public void AddToScore()
    {
        currentScore += pointPerBlockDestroyed;
        currentScoreText.text = currentScore.ToString();
        if (currentScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }

        //        scoreText.text = currentScore.ToString();
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
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
}
