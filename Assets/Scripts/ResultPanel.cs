using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] TMP_Text m_Score;
    [SerializeField] TMP_Text m_BestScore;

    private void OnEnable()
    {
        m_BestScore.text = PlayerPrefs.GetInt(StaticUrlScript.highScore).ToString();
        m_Score.text = PlayerPrefs.GetInt(StaticUrlScript.currentScore).ToString();
    }
}
