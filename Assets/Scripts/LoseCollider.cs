using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{    
    public GameObject enableResumeButton;
    public GameObject enableResetButton;
    public static bool checkResumeEligiblity = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (checkResumeEligiblity)
        {
            enableResetButton.SetActive(true);
            enableResumeButton.SetActive(true);
        }
        else
        {
            enableResetButton.SetActive(true);
        }
        
        if(!Ball.enableResume)
        {
            SceneManager.LoadScene(53);
            Ball.hasStarted = false;
        }                
    }
}
