using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class FillInTime : MonoBehaviour
{
    [Header("Auto Reference for getting image component automatically")]
    [SerializeField] private bool _autoReference = true;
    [Space]
    [SerializeField] private Image _imageToFill;
    [SerializeField] private float _timeToFill = 5;
    //[SerializeField] private Button _DashboardBtn;

    // Start is called before the first frame update
    void Start()
    {
        //CheckInternetConnection();       
        
        if (_autoReference) _imageToFill = GetComponent<Image>();
        EnableSliderFilling();
    }

    public void EnableSliderFilling()
    {      
        _imageToFill.DOFillAmount(1, _timeToFill).From(0).OnComplete(() =>
        {
            ShowAd();
            GameSession.Instance.EnableResultPanel(false);
        });
    }

    void ShowAd()
    {
        if (AdsController.Instance.IsInterstitialAdReady)
        {
            AdsController.Instance.OnAdClosedEvent.RemoveAllListeners();
            AdsController.Instance.OnAdClosedEvent.AddListener(() => LoadNextScene());
            AdsController.Instance.OnAdFailedEvent.RemoveAllListeners();
            AdsController.Instance.OnAdFailedEvent.AddListener(() => LoadNextScene());

            AdsController.Instance.ShowAd(StaticUrlScript.InterstitialAdUnit);
        }
        else
        {
            LoadNextScene();
        }
    }


    void LoadNextScene()
    {
        SceneLoader.Instance.LoadNextScene();
    }
    //private void LoadSpinScene()
    //{
    //    SceneManager.LoadSceneAsync("dashboard");
    //}

    //public void CheckInternetConnection()
    //{
    //    Utility.myLog("Internet Closed !!--- >");
    //    if (Application.internetReachability == NetworkReachability.NotReachable)
    //    {
    //        Utility.myError("Internet NOT CONNECTED---- >");
    //        LoaderManager.Instance?.OpenRetryPanel(() => LoadSpinScene());
    //    }       
    //}
}
