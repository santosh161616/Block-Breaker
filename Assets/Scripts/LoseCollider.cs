using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{    
    public GameObject enableResumButton;
    public GameObject enableResetButton;
    public static bool checkResumeEligiblity = true;

    Ball ballInstance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (checkResumeEligiblity)
        {
            enableResetButton.SetActive(true);
            enableResumButton.SetActive(true);
            Ball.instance.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        else
        {
            enableResetButton.SetActive(true);
        }
        
        if(!Ball.enableResume)
        {
            SceneManager.LoadScene("03 Game Over");
            Ball.hasStarted = false;
        }                
    }
}
