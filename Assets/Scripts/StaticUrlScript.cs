public static class StaticUrlScript
{
    public static bool isLoggerEnabled = true;

    //PlayerPrefs
    public static string highScore = "HighScore";                               //Highcore InGame
    public static string currentScore = "CurrentScore";                         //Current GameScore
    public static string currentLevel = "CurrentLevel";                         //Current ActiveLevel
    public static string Volume = "Volumne";                                    //InGame Volume

    //GameScene
    public static string GameOver = "GameOver";                                 //
    public static string Dashboard = "01 Start Menu";                           //StartGame Scene

    //Firebase
    public static string GameLaunched_Firebase = "GameLaunched";                //FirstLaunch
    public static string StartGame_Firebase = "GameStarted";                    //GameStart
    public static string GameEnd_Firebase = "GameEnd";                          //GameEnd
    public static string ResetLevel_Firebase = "ResetLevel";                    //ResetGame Level
    public static string NextLevel_Firebase = "NextLevel";                      //NextLevel Achived
    public static string InterstitialAd_Firebase = "InterstitialAd_Served";     //InterstitialAd Served    
    public static string RewardedAd_Firebase = "RewardedAd_Served";             //RewardedAd Served
    public static string GameExit_Firebase = "GameExit";                        //GameExit

    // String Constrants -- UnityAds
    public const string InterstitialAdUnit = "Interstitial_Android";            //Unity InterstitialAd id
    public const string RewardedAdUnitId = "Rewarded_Android";                  //Unity RewardedAd id
}
