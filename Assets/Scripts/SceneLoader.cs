using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
public class SceneLoader : MonoBehaviour
{
    GameSession gameSession;
    private int currentLevel;

    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
    }
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        currentLevel = currentSceneIndex + 1;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
    }
    public void ScoreReset()
    {
        gameSession.ResetHighScore();
    }
    public void LoadStartScean()
    {   
        // SceneManager.LoadScene(01);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        gameSession.GameOver();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
