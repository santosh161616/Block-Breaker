﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] int breakableBlocks;

  //  public Text textAd;
    public GameObject enableCanvas;
  //  private AdManager adInstance;

    private void Start()
    {

      //  adInstance = GetComponent<AdManager>();
    }

    public void CountBlocks()
    {
        breakableBlocks++;
    }
    
    public void BlockDestroyed()
    {
        breakableBlocks--;
        if(breakableBlocks <= 0)
        {
            enableCanvas.SetActive(true);
            Ball.hasStarted = false;
            Ball.enableInput = false;
            
            StartCoroutine(LoadingCount());            
        }
    }   
    
    IEnumerator LoadingCount()
    {
        int t = 3;
        while(t > 0)
        {
            t--;
        //    textAd.text = "Hold on: " + t;            
            yield return new WaitForSeconds(1);
        }
        enableCanvas.SetActive(false);        
       // adInstance.DisplayAdNow();
        Ball.enableInput = true;
        SceneLoader.Instance.LoadNextScene();
    }
}
