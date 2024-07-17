using System;
using UnityEngine;
public static class Utility
{
    public static void myLog(string dataToShow)
    {
        if (StaticUrlScript.isLoggerEnabled)
            Debug.Log(DateTime.Now.ToLongTimeString() + " " + dataToShow);
    }

    public static void myWarnning(string dataToShow)
    {
        if(StaticUrlScript.isLoggerEnabled)
            Debug.LogWarning(DateTime.Now.ToLongTimeString()+ " " + dataToShow);
    }

    public static void myError(string dataToShow)
    {
        if (StaticUrlScript.isLoggerEnabled)
            Debug.LogError(DateTime.Now.ToLongTimeString()+" "+dataToShow);
    }

    public static void myLog(int dataToShow)
    {
        if (StaticUrlScript.isLoggerEnabled)
            Debug.Log(DateTime.Now.ToLongTimeString() + " " + dataToShow);
    }
}
