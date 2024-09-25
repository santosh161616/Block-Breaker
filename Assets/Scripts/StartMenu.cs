using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject _optionsPanel;
    [SerializeField] Slider _sound;
    [SerializeField] Button _resetBest, _resetLevel;

    private void Start()
    {
        _sound.value = PlayerPrefs.GetFloat(StaticUrlScript.Volume, 1);
        AddingListeners();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_optionsPanel.activeInHierarchy)
                _optionsPanel.SetActive(false);
        }
    }
    void AddingListeners()
    {
        _sound.onValueChanged.AddListener(AdjustVolume);

        _resetBest.onClick.RemoveAllListeners();
        _resetBest.onClick.AddListener(() => { ScoreReset(); });

        _resetLevel.onClick.RemoveAllListeners();
        _resetLevel.onClick.AddListener(() => { ResetLevel(); });
    }

    public void ScoreReset()
    {
        PlayerPrefs.DeleteKey(StaticUrlScript.highScore);
        PlayerPrefs.Save();
    }

    public void ResetLevel()
    {
        PlayerPrefs.DeleteKey(StaticUrlScript.currentLevel);
        PlayerPrefs.Save();
    }

    public void AdjustVolume(float value)
    {
        PlayerPrefs.SetFloat(StaticUrlScript.Volume, value);
    }
}
