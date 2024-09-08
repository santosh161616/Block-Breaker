using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] int breakableBlocks;

    public void CountBlocks()
    {
        breakableBlocks++;
    }
    
    public void BlockDestroyed()
    {
        breakableBlocks--;
        if(breakableBlocks <= 0)
        {
            //enableCanvas.SetActive(true);
            Ball.instance.HasStarted = false;
            Ball.instance.EnableInput = false;

            //StartCoroutine(LoadingCount());    //Added it to show Ad.        
            SceneLoader.Instance.LoadNextScene();
        }
    }           
}
