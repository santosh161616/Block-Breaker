using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ball")
        {
            collision.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }        
        GameSession.Instance.EnableGameOverPnl();
    }
}
