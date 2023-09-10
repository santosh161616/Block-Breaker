﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    private int currentLevel;
    public static SceneLoader Instance;
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
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
        GameSession.instance.ResetHighScore();
    }
    public void LoadStartScean()
    {
        SceneManager.LoadScene(1);
       // SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        GameSession.instance.GameOver();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
