﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    private int currentLevel;
    public static SceneLoader Instance;
    private async void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        currentLevel = PlayerPrefs.GetInt(StaticUrlScript.currentLevel);
        ///<summary>
        ///Making Event firing wait so Level can be updated.
        ///</summary>
        await Task.Delay(500);
        GameSession.Instance.LevelUpdateEvent.Invoke();
    }
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        currentLevel = currentSceneIndex + 1;
        PlayerPrefs.SetInt(StaticUrlScript.currentLevel, currentLevel);
        GameSession.Instance.LevelUpdateEvent.Invoke();
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadSceneAsync(PlayerPrefs.GetInt(StaticUrlScript.currentLevel, 1));
    }
    public void ScoreReset()
    {
        GameSession.Instance.ResetHighScore();
    }
    public void LoadStartScean()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt(StaticUrlScript.currentLevel));
    }
}
