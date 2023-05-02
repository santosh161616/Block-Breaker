using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private bool isPaused = false;
   
    //State variables.
    [SerializeField] int currentScore = 0;

    private void Awake()
    {
        int gameStatusCount = FindObjectsOfType<GameSession>().Length;
        if(gameStatusCount > 1)
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
        displayLevel.text = "Level: " +PlayerPrefs.GetInt("CurrentLevel").ToString();
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
        if(isPaused)
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
