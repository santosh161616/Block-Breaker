using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class LoaderManager : MonoBehaviour
{
    public GameObject GameStartLoader;
    public Slider StartGameSlider;
    public GameObject LoaderObject, confirmationPanel, RetryPanel, noAdsPanel;
    public TMP_Text headingTxt, subHeadingTxt, btnText, infoTxt;
    public Image headerImage, RetryImage, GameLogo;
    public Button RetryButton, buyBtn;
    public TMP_Text RetryTextHeading, RetryText;
    [SerializeField] private TextMeshProUGUI loadingTxt, RetryButtonText;

    #region Unity SingleTon
    private static LoaderManager instance;
    public static LoaderManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoaderManager>();

                // If no LoaderManager is found, you can optionally log an error or instantiate it here
                if (instance == null)
                {
                    Debug.LogError("No LoaderManager instance found in the scene.");
                }
            }
            return instance;
        }
        private set { instance = value; } // Setter is private to avoid external modification
    }

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy this instance if it’s not the singleton
            return; // Ensure we don't call DontDestroyOnLoad on this duplicate instance
        }

        DontDestroyOnLoad(gameObject); // Persist this instance across scenes
        LoaderObject.gameObject.SetActive(false); // Assuming LoaderObject is defined elsewhere
    }
    #endregion

    private void Start()
    {
        //RandomColor();
        GameStartLoader?.SetActive(true);
        GameLogo.transform.DOScale(1.5f, 5f);
        StartCoroutine(Loading());
        StartGameSlider.DOValue(1f, 5f).SetEase(DG.Tweening.Ease.Linear).OnComplete(() =>
        {
            if (GameStartLoader != null)
                GameStartLoader.SetActive(false);
        });
    }

    private void RandomColor()
    {
        Color color = Random.ColorHSV(0, 1, 0.5f, 1, 1, 1);
        GameStartLoader.GetComponent<Image>().color = color + Color.gray;
    }

    bool loadingIsDone = false;
    IEnumerator Loading()
    {
        TextMeshProUGUI loadingText = loadingTxt;
        WaitForSeconds delay = new WaitForSeconds(0.25f);
        while (!loadingIsDone)
        {
            loadingText.text = "LOADING";
            yield return delay;

            loadingText.text = "LOADING.";
            yield return delay;

            loadingText.text = "LOADING..";
            yield return delay;

            loadingText.text = "LOADING ...";
            yield return delay;
        }
    }
    public void EnableLoader()
    {
        if (LoaderObject.activeInHierarchy)
        {
            return;
        }
        LoaderObject?.SetActive(true);
        Utility.myLog("Loader Enabled--- >");
    }

    public void DisableLoader()
    {
        LoaderObject?.GetComponent<Image>().DOFade(0, 0.7f).OnComplete(() => { LoaderObject.SetActive(false); LoaderObject.GetComponent<Image>().DOFade(0.75f, 0.1f); });
        Utility.myLog("Loader DISEnabled--- >");
    }

    public void OpenConfirmationPanel(UnityAction actionToPerform, Image item, string headerTxt = null, string subHeaderTxt = null, string btnTxt = null)
    {
        confirmationPanel?.SetActive(true);
        buyBtn?.onClick.RemoveAllListeners();
        buyBtn?.onClick.AddListener(ClosePanels);
        buyBtn?.onClick.AddListener(actionToPerform);

        Utility.myLog("Listener Added ->" + actionToPerform);
        headerImage.sprite = item.sprite;
        headingTxt.text = (headerTxt == null) ? "Want to Buy?" : headerTxt;
        subHeadingTxt.text = (subHeaderTxt == null) ? "buy to get this item" : subHeaderTxt;
        btnText.text = (btnTxt == null) ? "Buy" : btnTxt;
    }


    public void OpenRetryPanel(UnityAction ActionToPerform, string retryHeadingText = null, string retryText = null, string retryButtonText = null, int Index = -1)
    {
        Utility.myLog(nameof(OpenRetryPanel) + "-->>" + retryHeadingText + retryText + retryButtonText);
        RetryPanel?.SetActive(true);
        //if (Index == -1)
        //{
        //    RetryImage.sprite = RetrySprite;
        //}
        //else
        //{
        //    RetryImage.sprite = /*StoredSprite[Index];*/RetrySprite;
        //}
        RetryTextHeading.text = (retryHeadingText == null) ? "Weak or No Internet " : retryHeadingText;
        RetryText.text = (retryText == null) ? "Please move to a strong \n network area." : retryText;
        RetryButtonText.text = (retryButtonText == null) ? "Retry" : retryButtonText;
        RetryButton?.onClick.RemoveAllListeners();
        RetryButton?.onClick.AddListener(CloseRetry);
        RetryButton?.onClick.AddListener(ActionToPerform);
    }


    public void OpenNoAdsPanel(bool enable)
    {
        noAdsPanel.SetActive(enable);
        DisableLoader();
    }

    public void ClosePanels()
    {
        confirmationPanel?.SetActive(false);
    }
    public void CloseRetry()
    {
        RetryPanel?.SetActive(false);
        //ConfirmationPanel?.SetActive(false);
    }

}