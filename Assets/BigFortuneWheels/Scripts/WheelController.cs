using System.Collections.Generic;
using UnityEngine;
using Mkey;
using System;
using UnityEngine.Events;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
//using GoogleMobileAds.Api;

namespace MkeyFW // mkey fortune wheel
{
    enum SpinDir { Counter, ClockWise }
    public class WheelController : MonoBehaviour
    {
        [Header("Main references")]
        [Space(16, order = 0)]
        [SerializeField]
        private Transform Reel;
        [SerializeField]
        private Animator pointerAnimator;
        [SerializeField]
        private LampsController lampsController;
        [SerializeField]
        private SceneButton spinButton;
        [SerializeField]
        private SceneButton closeButton;
        [SerializeField]
        private ArrowBeviour arrowBeviour;
        [SerializeField]
        private WinSectorBehavior winSectorPrefab;
        [SerializeField]
        private Transform winSectorParent;

        [Header("Spin options")]
        [Space(16, order = 0)]
        [SerializeField]
        private float inRotTime = 0.2f;
        [SerializeField]
        private float inRotAngle = 5;
        [SerializeField]
        private float mainRotTime = 1.0f;
        [SerializeField]
        private EaseAnim mainRotEase = EaseAnim.EaseLinear;
        [SerializeField]
        private float outRotTime = 0.2f;
        [SerializeField]
        private float outRotAngle = 5;
        [SerializeField]
        private float spinStartDelay = 0;
        [SerializeField]
        private int spinSpeedMultiplier = 1;
        [SerializeField]
        private SpinDir spinDir = SpinDir.Counter;

        [Header("Lamps control")]
        [Space(16, order = 0)]
        [Tooltip("Before spin")]
        [SerializeField]
        private LampsFlash lampsFlashAtStart = LampsFlash.Random;
        [Tooltip("During spin")]
        [SerializeField]
        private LampsFlash lampsFlashDuringSpin = LampsFlash.Sequence;
        [Tooltip("After spin")]
        [SerializeField]
        private LampsFlash lampsFlashEnd = LampsFlash.All;

        [Header("Additional options")]
        [Space(16, order = 0)]
        [Tooltip("Help arrow")]
        [SerializeField]
        private int arrowBlinkCount = 2;
        [SerializeField]
        private AudioClip spinSound;

        [Header("Result event, after spin")]
        [Space(16, order = 0)]
        [SerializeField]
        private UnityEvent resultEvent;

        [Header("Simulation, only for test")]
        [Space(32, order = 0)]
        [SerializeField]
        private bool simulate = false;
        [SerializeField]
        private int simPos = 0;
        [SerializeField]
        private bool debug = false;
        [SerializeField]
        private TextMesh coinsText;
        Queue<GameObject> coinsQueue = new Queue<GameObject>();
        [SerializeField] Transform target, collectedCoinTransform;
        [SerializeField] int maxCoins;
        [SerializeField] public GameObject animatedCoinPrefab;
        [SerializeField] float spread;
        [SerializeField][Range(0.5f, 0.9f)] float minAnimDuration;
        [SerializeField][Range(0.9f, 2f)] float maxAnimDuration;
        [SerializeField] Ease easeType;
        [SerializeField] TextMesh coinUIText;
        [SerializeField] private GameObject _cryEmojiObject;

        [SerializeField] private Button _collectButton, _button_2X, _betterLuckOkButton, _spinButtonNew, _unclaimedButton, _backButton, _messagePanelOkButton;
        [SerializeField] private GameObject _collectPanel, _betterLuckPanel, _unclaimedPanel, _messagePanel;
        [SerializeField] private TextMeshProUGUI _wonCoinsText, _remainingSpinsText, _messageInstructionText;
        [SerializeField] private TextMesh _walletBalanceText;
        [SerializeField] private GameObject _ResultDetailsPanel;
        [SerializeField] private bool walletInfoUpdated;
        [SerializeField] private int walletRetryCount;
        [SerializeField] private ServerLudo.PlayerData playerData;
        [SerializeField] private ServerLudo.SpinWheelData spinData;
        [SerializeField] private string _spinWheelId;
        [SerializeField] private RectSequenceScaler _rectSequenceScaler;

        [SerializeField] private SpinWheelData _spinWheelData;
        private int _c = 0;
        public int Coins
        {
            get { return _c; }
            set
            {
                _c = value;
                //update UI text whenever "Coins" variable is changed
                if (coinUIText != null) coinUIText.text = Coins.ToString();
            }
        }
        #region events
        public Action<int, string, bool> SpinResultEvent; // spin result event <coins, isBigWin>
        public Action CloseButtonClickEvent;
        #endregion events

        #region properties
        public Sector WinSector { get; private set; }
        #endregion properties

        #region temp vars
        private Sector[] sectors;
        private int rand = 0;
        private int sectorsCount = 0;
        private float angleSpeed = 0;
        private float sectorAngleRad;
        private float sectorAngleDeg;
        private int currSector = 0;
        private int nextSector = 0;
        private TweenSeq tS;
        private AudioSource audioSource;
        private float rotDirF = -0;
        private WinSectorBehavior winSectorBehavior;
        #endregion temp vars

        #region regular
        void OnValidate()
        {
            Validate();
        }

        void Start()
        {
            //Firebase.Analytics.FirebaseAnalytics.LogEvent("Rewards", "SpinWheel", "0");
            GetWalletInfo();
            //RandomizeSpinWheelData(_spinWheelData, 4);
            _spinButtonNew.interactable = false;
            if (Screen.orientation != ScreenOrientation.LandscapeLeft)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }
            sectors = GetComponentsInChildren<Sector>();
            sectorsCount = (sectors != null) ? sectors.Length : 0;
            if (debug) Utility.myLog("sectorsCount: " + sectorsCount);
            if (sectorsCount > 0)
            {
                sectorAngleDeg = 360f / sectorsCount;
                sectorAngleRad = 360f / sectorsCount * Mathf.Deg2Rad;
            }
            if (pointerAnimator)
            {
                pointerAnimator.enabled = false;
                pointerAnimator.speed = 0;
                pointerAnimator.transform.localEulerAngles = Vector3.zero;
            }
            if (lampsController) lampsController.lampFlash = lampsFlashAtStart;
            UpdateRand();
            if (arrowBeviour) arrowBeviour.Show(arrowBlinkCount, 0.1f);
            audioSource = GetComponent<AudioSource>();

            if (closeButton)
            {
                closeButton.clickEvent.RemoveAllListeners();
                closeButton.clickEvent.AddListener(() => { CloseButtonClickEvent?.Invoke(); });
            }
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() =>
            {
                //if (GamesAssistant.gameID == 7)
                //{
                //    NativeFunction.UnloadApp();
                //}
                //else
                {
                    LoadDashboard();
                }

            });

            SetupSpinWheel();
        }

        private void RandomizeSpinWheelData(SpinWheelData _spinData, int randomCount)
        {
            int count = _spinData.spinWheelValues.Count - 1;
            for (int i = 0; i < count; i++)
            {
                if (i < randomCount)
                {
                    int resultId = UnityEngine.Random.Range(11, 22);
                    _spinData.spinWheelValues[resultId].Coins = UnityEngine.Random.Range(0, 7);
                }
                else
                {
                    _spinData.spinWheelValues[i].Coins = 0;
                }

            }
        }
        private void OnEnable()
        {
            CheckInternetConnectivity();
            if (StaticUrlScript.isAdsEnabled)
            {
                //AdsController.Instance.LoadBannerAd(/*AdSize.IABBanner, AdPosition.Bottom*/);
                //IronSourceAdsController.Instance.LoadBannerAd(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM, StringConstants.SpinWheel_Banner_PlacementName);
            }
        }

        private void OnDisable()
        {
            if (StaticUrlScript.isAdsEnabled)
            {
                //AdsController.Instance.DestroyBannerAd();
                //IronSource.Agent.destroyBanner();
            }
        }
        public void CheckInternetConnectivity()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Utility.myLog("Error. Check internet connection!");
                LoaderManager.instance.OpenRetryPanel(LoadDashboard);
            }
        }

        public async void SetupSpinWheel()
        {
            Utility.myLog("Updating SpinWheel Data");
            List<spinWheelValue> sectorsd = _spinWheelData.spinWheelValues;
            await SetData(sectorsd);
            simulate = true;
            int resultId = UnityEngine.Random.Range(0, 12);
            simPos = FindResultIndex(resultId);
        }
        private int FindResultIndex(int resultCoinId)
        {
            for (int i = 0; i < sectors.Length; i++)
            {
                if (sectors[i].Id == resultCoinId)
                {
                    return i;
                }
            }
            return 0;
        }
        public async Task SetData(List<spinWheelValue> responseData)
        {
            //while (!walletInfoUpdated)
            //{
            //    await Task.Yield();
            //}
            for (int i = 0; i < responseData.Count; i++)
            {
                sectors[i].Coins = responseData[i].Coins;
                //sectors[i].BigWin = responseData[i].Id;
                sectors[i].Id = responseData[i].Id;
                sectors[i].IconUrl = responseData[i].Icon;

            }
            for (int i = responseData.Count; i < sectors.Length; i++)
            {
                sectors[i].Coins = 0;
                //sectors[i].BigWin = responseData[i].Id;
                sectors[i].Id = 1;

            }
            _rectSequenceScaler.enabled = true;
            _spinButtonNew.interactable = true;
            Utility.myLog(" _spinButtonNew.interactable" + _spinButtonNew.interactable);
        }

        void Update()
        {
            UpdateRand();
            if (Input.GetKeyDown(KeyCode.Escape) && !_betterLuckPanel.activeInHierarchy && !_collectPanel.activeInHierarchy && !_ResultDetailsPanel.activeInHierarchy && !_messagePanel.activeInHierarchy && _spinButtonNew.interactable)
            {
                LoadDashboard();
            }
        }

        void OnDestroy()
        {
            CancelSpin();
        }
        #endregion regular

        void Awake()
        {
            //prepare pool
            PrepareCoins();
            SpinResultEvent += CollectCoins; //AddCoins;
        }

        private async void CollectCoins(int amount, string spinWheelId, bool isBigWin)
        {
            if (amount == 0)
            {
                await Task.Delay(1000);
                _betterLuckPanel.SetActive(true);
                _betterLuckOkButton.onClick.RemoveAllListeners();
                _betterLuckOkButton.onClick.AddListener(() => { /*EnableCryEmoji();*/ /*_betterLuckOkButton.interactable = false;*/ CollectSpinWheelReward(spinWheelId, false, false, () => { /*NativeFunction.UpdateCoins(Value + amount);*/ LoaderManager.Instance.DisableLoader(); }); });

            }
            else
            {
                _collectPanel.SetActive(true);
                _wonCoinsText.text = amount + " Coins";
                _collectButton.onClick.RemoveAllListeners();
                _collectButton.onClick.AddListener(() => { /*_collectButton.interactable = false;*/ CollectSpinWheelReward(spinWheelId, false, true, () => {/* NativeFunction.UpdateCoins( amount);*/PlayerPrefs.SetInt(StringConstants.Pp_SpinWinCoins, amount); AddCoins(amount, isBigWin); LoaderManager.Instance.DisableLoader(); }); });

                if (StaticUrlScript.isAdsEnabled)
                {
                    if (AdManager.Instance.IsAdLoaded)
                    {
                        _button_2X.gameObject.SetActive(true);
                        _button_2X.onClick.RemoveAllListeners();
                        _button_2X.onClick.AddListener(() => { CollectSpinWheelReward(spinWheelId, true, true, () => { /*NativeFunction.UpdateCoins(amount);*/PlayerPrefs.SetInt(StringConstants.Pp_SpinWinCoins, amount * 2); AddCoins(amount * 2, isBigWin); LoaderManager.Instance.DisableLoader(); }); });                                             
                    }
                    else
                    {
                        _button_2X.gameObject.SetActive(false);
                        _collectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Collect";
                        //_collectButton.image.sprite = _button_2X.image.sprite;
                        _collectButton.transform.position = new Vector3(0, _collectButton.transform.position.y, _collectButton.transform.position.z);
                    }
                }
                else
                {
                    _button_2X.gameObject.SetActive(false);
                    _collectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Collect";
                    //_collectButton.image.sprite = _button_2X.image.sprite;
                    _collectButton.transform.position = new Vector3(0, _collectButton.transform.position.y, _collectButton.transform.position.z);
                }

            }
        }

        private void EnableCryEmoji()
        {
            _betterLuckPanel.SetActive(false);
            _cryEmojiObject.SetActive(true);
            Vector3 originalScale = _cryEmojiObject.transform.localScale;
            _cryEmojiObject.transform.DOPunchScale(new Vector3(2, 2, 2), 2, 0, 0)
                .OnComplete(() =>
                {
                    //_cryEmojiObject.transform.localScale = originalScale;
                    _cryEmojiObject.SetActive(false);
                });
        }

        public void PrepareCoins()
        {
            GameObject coin;
            for (int i = 0; i < maxCoins; i++)
            {
                coin = Instantiate(animatedCoinPrefab);
                coin.transform.parent = transform;
                coin.SetActive(false);
                coinsQueue.Enqueue(coin);
            }
        }
        /// <summary>
        /// Start spin
        /// </summary>
        public void StartSpin(Action completeCallBack)
        {
            _spinButtonNew.interactable = false;
            _backButton.interactable = false;
            WinSector = null;
            if (arrowBeviour) arrowBeviour.CancelTween();
            if (tS != null) return;
            if (debug) Utility.myLog("rand: " + rand);
            nextSector = rand;
            if (spinButton) spinButton.interactable = false;
            CancelSectorWin();

            // spin sound
            if (audioSource) audioSource.Stop();  // stop spin sound
            if (audioSource && spinSound)
            {
                audioSource.clip = spinSound;
                audioSource.Play();
                audioSource.loop = true;
            }

            RotateWheel(() =>
            {
                CheckResult();
                WinSector = sectors[currSector];

                ShowSectorWin();

                if (spinButton) spinButton.interactable = true;
                if (arrowBeviour) arrowBeviour.Show(arrowBlinkCount, 3f);

                if (audioSource) audioSource.Stop();  // stop spin sound

                if (audioSource && sectors[currSector] && sectors[currSector].hitSound) // play hit sound
                {
                    audioSource.clip = sectors[currSector].hitSound;
                    audioSource.Play();
                    audioSource.loop = false;
                }

                bool isBigWin = false;
                int res = GetWin(ref isBigWin);
                if (WinSector.Coins == 0)
                {
                    WinSector.emojiSprite.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 1f, 4);
                }
                resultEvent?.Invoke();
                SpinResultEvent?.Invoke(res, /*playerData.spinWheelData.*/_spinWheelId, isBigWin);
                completeCallBack?.Invoke();
            });
        }

        public void StartSpin()
        {
            StartSpin(null);
        }

        /// <summary>
        /// Async rotate wheel to next sector
        /// </summary>
        private void RotateWheel(Action rotCallBack)
        {
            rotDirF = (spinDir == SpinDir.ClockWise) ? -1f : 1f;
            // validate input
            Validate();

            //change lamps state
            if (lampsController) lampsController.lampFlash = lampsFlashDuringSpin;

            // get next reel position
            nextSector = (!simulate) ? nextSector : simPos;
            if (debug) Utility.myLog("next: " + nextSector + " ;angle: " + GetAngleToNextSector(nextSector));

            // create reel rotation sequence - 4 parts  in - (continuous) - main - out
            float oldVal = 0f;
            tS = new TweenSeq();
            float angleZ = 0;


            tS.Add((callBack) => // in rotation part
            {
                SimpleTween.Value(gameObject, 0f, inRotAngle, inRotTime)
                                  .SetOnUpdate((float val) =>
                                  {
                                      if (Reel) Reel.Rotate(0, 0, (-val + oldVal) * rotDirF);
                                      oldVal = val;
                                  })
                                  .AddCompleteCallBack(() =>
                                  {
                                      callBack?.Invoke();
                                  }).SetDelay(spinStartDelay);
            });

            tS.Add((callBack) =>  // main rotation part
            {
                oldVal = 0f;
                pointerAnimator.enabled = true;
                spinSpeedMultiplier = Mathf.Max(0, spinSpeedMultiplier);
                angleZ = GetAngleToNextSector(nextSector) + 360.0f * spinSpeedMultiplier;
                SimpleTween.Value(gameObject, 0, -(angleZ + outRotAngle + inRotAngle), mainRotTime)
                                  .SetOnUpdate((float val) =>
                                  {
                                      angleSpeed = (-val + oldVal) * rotDirF;
                                      if (Reel) Reel.Rotate(0, 0, angleSpeed);
                                      oldVal = val;
                                      if (pointerAnimator)
                                      {
                                          pointerAnimator.speed = Mathf.Abs(angleSpeed);
                                      }
                                  })
                                  .SetEase(mainRotEase)
                                  .AddCompleteCallBack(() =>
                                  {
                                      if (pointerAnimator)
                                      {
                                          pointerAnimator.enabled = false;
                                          pointerAnimator.speed = 0;
                                          pointerAnimator.transform.localEulerAngles = Vector3.zero;
                                      }
                                      if (lampsController) lampsController.lampFlash = lampsFlashEnd;
                                      callBack?.Invoke();
                                  });
            });

            tS.Add((callBack) =>  // out rotation part
            {
                oldVal = 0f;
                SimpleTween.Value(gameObject, 0, outRotAngle, outRotTime)
                                  .SetOnUpdate((float val) =>
                                  {
                                      if (Reel) Reel.Rotate(0, 0, (-val + oldVal) * rotDirF);
                                      oldVal = val;
                                  })
                                  .AddCompleteCallBack(() =>
                                  {
                                      if (pointerAnimator)
                                      {
                                          pointerAnimator.transform.localEulerAngles = Vector3.zero;
                                      }
                                      currSector = nextSector;
                                      callBack?.Invoke();
                                  });
            });

            tS.Add((callBack) =>
            {
                rotCallBack?.Invoke();
                tS = null;
                callBack?.Invoke();
            });

            tS.Start();
        }

        private void Validate()
        {
            mainRotTime = Mathf.Max(0.1f, mainRotTime);

            inRotTime = Mathf.Clamp(inRotTime, 0, 1f);
            inRotAngle = Mathf.Clamp(inRotAngle, 0, 10);

            outRotTime = Mathf.Clamp(outRotTime, 0, 1f);
            outRotAngle = Mathf.Clamp(outRotAngle, 0, 10);
            spinSpeedMultiplier = Mathf.Max(1, spinSpeedMultiplier);
            spinStartDelay = Mathf.Max(0, spinStartDelay);

            if (simulate)
            {
                sectors = GetComponentsInChildren<Sector>();
                sectorsCount = (sectors != null) ? sectors.Length : 0;
                simPos = Mathf.Clamp(simPos, 0, sectorsCount - 1);
            }
        }

        /// <summary>
        /// Return angle in degree to next symbol position in symbOrder array
        /// </summary>
        /// <param name="nextOrderPosition"></param>
        /// <returns></returns>
        private float GetAngleToNextSector(int nextOrderPosition)
        {
            rotDirF = (spinDir == SpinDir.ClockWise) ? -1f : 1f;
            return (currSector < nextOrderPosition) ? rotDirF * (nextOrderPosition - currSector) * sectorAngleDeg : (sectors.Length - rotDirF * (currSector - nextOrderPosition)) * sectorAngleDeg;
        }

        /// <summary>
        /// Upadate random value rand
        /// </summary>
        private void UpdateRand()
        {
            rand = UnityEngine.Random.Range(0, sectorsCount);
        }

        public void CancelSpin()
        {
            if (this)
            {
                CancelSectorWin();

                if (tS != null)
                {
                    tS.Break();
                    tS = null;
                }

                SimpleTween.Cancel(gameObject, false);
                if (pointerAnimator)
                {
                    pointerAnimator.enabled = false;
                    pointerAnimator.speed = 0;
                    pointerAnimator.transform.localEulerAngles = Vector3.zero;
                }
            }
        }

        #region win
        public void CancelSectorWin()
        {
            if (this && winSectorBehavior)
            {
                //Destroy(winSectorBehavior);
                winSectorBehavior?.gameObject.SetActive(false);
            }
        }

        private void ShowSectorWin()
        {
            if (winSectorPrefab && winSectorParent && !winSectorBehavior) winSectorBehavior = Instantiate(winSectorPrefab, winSectorParent);
            else
            {
                winSectorBehavior?.gameObject.SetActive(true);
            }
        }
        #endregion win


        /// <summary>
        /// Check result and invoke sector hit event
        /// </summary>
        private void CheckResult()
        {
            int coins = 0;
            bool isBigWin = false;

            if (sectors != null && currSector >= 0 && currSector < sectors.Length)
            {
                Sector s = sectors[currSector];
                if (s != null)
                {
                    isBigWin = s.BigWin;
                    coins = s.Coins;
                    s.PlayHit(Reel.position);
                }
            }
            if (debug) Utility.myLog("Coins: " + coins + " ;IsBigWin: " + isBigWin);
        }

        /// <summary>
        /// Return spin result, coins
        /// </summary>
        /// <param name="isBigWin"></param>
        /// <returns></returns>
        public int GetWin(ref bool isBigWin)
        {
            int res = 0;
            isBigWin = false;
            if (sectors != null && currSector >= 0 && currSector < sectors.Length)
            {
                isBigWin = sectors[currSector].BigWin;
                return sectors[currSector].Coins;
            }
            return res;
        }
        private async void Animate(float amount, bool isBigWin)
        {
            for (int i = 0; i < amount; i++)
            {
                //check if there's coins in the pool
                if (coinsQueue.Count <= 0) continue;
                //extract a coin from the pool
                GameObject coin = coinsQueue.Dequeue();
                coin.SetActive(true);

                //move coin to the collected coin pos
                coin.transform.position = collectedCoinTransform.position + new Vector3(UnityEngine.Random.Range(-spread, spread), 0f, 0f);

                //animate coin to target position
                float duration = UnityEngine.Random.Range(minAnimDuration, maxAnimDuration);
                var targetPosition = target.position;
                coin.transform.DOMove(targetPosition, duration)
                    .SetEase(easeType)
                    .OnComplete(() =>
                    {
                        //executes whenever coin reach target position
                        coin.SetActive(false);
                        coinsQueue.Enqueue(coin);
                        //Coins++;
                    });
                await Task.Delay(50);
            }
            Utility.myLog("amount -> " + amount + "    " + 1000 / amount);
            //for (int i = 0; i < amount; i++)
            //{
            //    await Task.Delay(1000 / amount);
            //    Coins++;
            //}
            //UpdateText(amount);
            Value = amount;
            PlayerPrefs.SetFloat(StringConstants.Pp_Balance, (PlayerPrefs.GetFloat(StringConstants.Pp_Balance) + Value));

        }
        public int CountFPS = 30;
        public float Duration = 1f;
        public string NumberFormat = "N0";
        private float _value;
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                UpdateText(_value + value);
                _value = value;
            }
        }
        private Coroutine CountingCoroutine;

        private void UpdateText(float newValue)
        {
            if (CountingCoroutine != null)
            {
                StopCoroutine(CountingCoroutine);
            }

            CountingCoroutine = StartCoroutine(CountText(newValue));
        }

        private IEnumerator CountText(float newValue)
        {
            WaitForSeconds Wait = new WaitForSeconds(1f / CountFPS);
            float previousValue = _value;
            int stepAmount;

            if (newValue - previousValue < 0)
            {
                stepAmount = Mathf.FloorToInt((newValue - previousValue) / (CountFPS * Duration)); // newValue = -20, previousValue = 0. CountFPS = 30, and Duration = 1; (-20- 0) / (30*1) // -0.66667 (ceiltoint)-> 0
            }
            else
            {
                stepAmount = Mathf.CeilToInt((newValue - previousValue) / (CountFPS * Duration)); // newValue = 20, previousValue = 0. CountFPS = 30, and Duration = 1; (20- 0) / (30*1) // 0.66667 (floortoint)-> 0
            }

            if (previousValue < newValue)
            {
                while (previousValue < newValue)
                {
                    previousValue += stepAmount;
                    if (previousValue > newValue)
                    {
                        previousValue = newValue;
                    }

                    //Text.SetText(previousValue.ToString(NumberFormat));
                    _walletBalanceText.text = previousValue.ToString();

                    yield return Wait;
                }
            }
            else
            {
                while (previousValue > newValue)
                {
                    previousValue += stepAmount; // (-20 - 0) / (30 * 1) = -0.66667 -> -1              0 + -1 = -1
                    if (previousValue < newValue)
                    {
                        previousValue = newValue;
                    }

                    //Text.SetText(previousValue.ToString(NumberFormat));
                    _walletBalanceText.text = previousValue.ToString();
                    yield return Wait;
                }
                _walletBalanceText.text = previousValue.ToString();
            }
        }
        public void AddCoins(float amount, bool isBigWin)
        {
            _collectPanel.SetActive(false);
            Animate(amount, isBigWin);
        }

        public async void CollectSpinWheelReward(string spinWheelId, bool watchedAd = false, bool shouldWait = true, UnityAction action = null)
        {         
            Utility.myLog("CollectSpinWheelReward --- > id " + spinWheelId + "    watchedAd-  " + watchedAd + "Spin State>" + LoaderManager.instance.SpinState);

            var data = _spinWheelData;
            //var data = await ApiManager_SocialPe.ClaimSpinWheelReward(spinWheelId, watchedAd);
            if (data != null)
            {
                if (data.spinWheelId != null) // old (data.statuscode == 200)
                {
                    action?.Invoke();
                    if (shouldWait)
                    {
                        await Task.Delay(3000);
                    }
                    if (GamesAssistant.gameID == 7)
                    {
                        Firebase.Analytics.FirebaseAnalytics.LogEvent(StringConstants.Firebase_SpinResultScreenEvent, StringConstants.FirebaseParam_GameId, GamesAssistant.gameID);
                        _ResultDetailsPanel.SetActive(true);
                    }
                    else
                    {
                        //NativeFunction.UnloadApp();
                        LoadDashboard();
                    }
                }
                else
                {
                    _messagePanel.SetActive(true);
                    _messageInstructionText.text = "Something went wrong!".ToString();
                    _messagePanelOkButton.onClick.RemoveAllListeners();
                    _messagePanelOkButton.onClick.AddListener(() =>
                    {
                        //if (GamesAssistant.gameID == 7)
                        //{
                        //    NativeFunction.UnloadApp();
                        //}
                        //else
                        {
                            LoadDashboard();
                        }
                    });
                }
            }
            else
            {
                _messagePanel.SetActive(true);
                _messagePanelOkButton.onClick.RemoveAllListeners();
                _messagePanelOkButton.onClick.AddListener(() =>
                {
                    //if (GamesAssistant.gameID == 7)
                    //{
                    //    NativeFunction.UnloadApp();
                    //}
                    //else
                    {
                        LoadDashboard();
                    }
                });
            }
            LoaderManager.Instance.DisableLoader();
        }

        public void GetWalletInfo()
        {
            walletInfoUpdated = false;
            Value = PlayerPrefs.GetFloat(StringConstants.Pp_Balance, 0);
            Utility.myLog("Wallet Amount ->" + Value);
            walletInfoUpdated = true;
            LoaderManager.Instance.DisableLoader();
        }


        public void LoadDashboard()
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(StringConstants.Firebase_DashboardEvent);
            SceneManager.LoadSceneAsync(StringConstants.Dashboard);
        }
    }
}