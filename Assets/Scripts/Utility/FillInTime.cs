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
    [SerializeField] private Button _DashboardBtn;

    // Start is called before the first frame update
    void Start()
    {
        CheckInternetConnection();
        _DashboardBtn.interactable = false;
        _DashboardBtn.onClick.AddListener(LoadSpinScene);
        
        if (_autoReference) _imageToFill = GetComponent<Image>();
        EnableSliderFilling();
    }

    public void EnableSliderFilling()
    {
       

        _imageToFill.DOFillAmount(1, _timeToFill).From(0).OnComplete(() =>
        {
            _DashboardBtn.interactable = true;
            
            {
                LoadSpinScene();
            }
        });

    }

    private void LoadSpinScene()
    {
        SceneManager.LoadSceneAsync("dashboard");
    }

    public void CheckInternetConnection()
    {
        Utility.myLog("Internet Closed !!--- >");
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Utility.myError("Internet NOT CONNECTED---- >");
            LoaderManager.Instance?.OpenRetryPanel(() => LoadSpinScene());
        }       
    }
}
