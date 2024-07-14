using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{    
    public static bool checkResumeEligiblity = true;

    Ball ballInstance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (checkResumeEligiblity)
        {
            GameSession.instance.enableResume.SetActive(true);
            GameSession.instance.tryAgain.SetActive(true);

            Ball.instance.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        
        if(!Ball.enableResume)
        {
            SceneManager.LoadScene("03 Game Over");
            Ball.hasStarted = false;
        }                
    }
}
