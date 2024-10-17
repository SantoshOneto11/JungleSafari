using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class LoaderManager : MonoBehaviour
{
    public static LoaderManager instance;
    public static LoaderManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoaderManager>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    public Image LoaderBackImage;
    public Image RetryImage, knifeImageDefault;
    public Sprite RetrySprite;
    public Sprite[] StoredSprite;
    public GameObject LoaderObject;
    public GameObject RetryPanel, ConfirmationPanel, NotifyPanel, ConnectingPanel, maximumLimitPanel;
    public Button RetryButton, YesConfirm, NoConfirm, LimitAction;
    public Text RetryTextHeading, RetryText, RetryButtonText, NoOpponentInstruction, NoOpponentButtonText;
    public TMP_Text RetryTitleTxt, _limitMessageTxt;

    public GameObject GameStartLoader;
    public Slider StartGameSlider;
   
    private void Awake()
    {
        // Utility.myLog("Loader Manager ----->");
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(true);
        LoaderObject.SetActive(false);
        CloseRetry();
    }

    public bool IsLoaderCompleted = false;
    private void Start()
    {
        GameStartLoader?.SetActive(true);
        StartGameSlider.DOValue(1f, 5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (GameStartLoader != null)
                GameStartLoader.SetActive(false);

            IsLoaderCompleted = true;
            Utility.myLog("Loader Finished!");
        });
    }

    public void EnableLoader(string data = ""/*bool Darken = false*/)
    {
        Utility.myLog("Logger Enabled---------------");
        LoaderObject?.SetActive(true);
    }

    public void DisableLoader(string data = "")
    {
        Utility.myLog("Loader Disabled---------------  " + data);
        LoaderObject?.GetComponent<Image>().DOFade(0, 0.7f).SetEase(Ease.Linear).OnComplete(() => { LoaderObject.SetActive(false); LoaderObject.GetComponent<Image>().DOFade(0.75f, 0.1f); });
    }

    public void OpenRetryPanel(UnityAction ActionToPerform, string retryHeadingText = null, string retryText = null, string retryButtonText = null, int Index = -1)
    {      
        Utility.myLog(nameof(OpenRetryPanel) + "-->>" + retryHeadingText + retryText + retryButtonText);
        RetryPanel?.SetActive(true);
        if (Index == -1)
        {
            RetryImage.sprite = RetrySprite;
        }
        else
        {
            RetryImage.sprite = StoredSprite[Index];
        }
        RetryTextHeading.text = (retryHeadingText == null) ? "Weak or No Internet " : retryHeadingText;
        RetryText.text = (retryText == null) ? "Please move to a strong \n network area." : retryText;
        RetryButtonText.text = (retryButtonText == null) ? "Retry" : retryButtonText;
        RetryButton?.onClick.RemoveAllListeners();
        RetryButton?.onClick.AddListener(CloseRetry);
        RetryButton?.onClick.AddListener(ActionToPerform);
    }

    public void OpenConfirmationPanel(UnityAction OnYes, UnityAction OnNo, string instructionText = null, string NoOppButtonText = null, Image knifeImage = null)
    {
        DisableLoader();
        Utility.myLog(nameof(OpenConfirmationPanel) + "-->>" + instructionText + NoOppButtonText);
        ConfirmationPanel?.SetActive(true);
        //NoOpponentInstruction.text = (instructionText == null) ? "Opponent Not Found" : instructionText;
        NoOpponentButtonText.text = (NoOppButtonText == null) ? "GO TO LOBBY" : NoOppButtonText;
        RetryTitleTxt.text = (instructionText == null) ? "Not Enough Coins" : instructionText;
        knifeImageDefault.sprite = (knifeImage.sprite == null) ? knifeImageDefault.sprite : knifeImage.sprite;

        YesConfirm?.onClick.RemoveAllListeners();
        YesConfirm?.onClick.AddListener(CloseRetry);
        YesConfirm?.onClick.AddListener(OnYes);
        NoConfirm?.onClick.RemoveAllListeners();
        NoConfirm?.onClick.AddListener(CloseRetry);
        NoConfirm?.onClick.AddListener(OnNo);
    }

    public void CloseRetry()
    {
        RetryPanel?.SetActive(false);
        ConfirmationPanel?.SetActive(false);
    }

    public void ClosePanels(bool affectRetryPanel, bool affectConfirmationPanel, bool affectConnectionPanel = true)
    {
        if (affectRetryPanel)
            RetryPanel?.SetActive(false);
        if (affectConfirmationPanel)
            ConfirmationPanel?.SetActive(false);
        if (affectConnectionPanel)
            ConnectingPanel.SetActive(false);
    }

    public void ClosePanel(bool confirmationPanel = false, bool dnsPanel = false)
    {
        ConfirmationPanel?.SetActive(confirmationPanel);
        maximumLimitPanel?.SetActive(dnsPanel);
    }
    public void CloseApplication()
    {
        Application.Quit();
    }

    
    public void LoadDashboard()
    {
        string scenToLoad = StringConstants.Dashboard;
        SceneManager.LoadSceneAsync(scenToLoad);
    }    

    public void EnableDailyLimitPanel(string msg)
    {
        maximumLimitPanel.SetActive(true);
        _limitMessageTxt.text = msg;
    }

    public void OpenDNSPanel(UnityAction onYes, string msg)
    {
        if (LoaderObject.activeInHierarchy)
            DisableLoader();

        maximumLimitPanel.SetActive(true);
        _limitMessageTxt.text = msg;
        LimitAction.onClick.AddListener(onYes);
    }
    public enum SpinWheelState
    {
        Rewarded,
        LuckySpin,
        GameEndSpin,
        None
    }

    public SpinWheelState SpinState = SpinWheelState.None;
}
