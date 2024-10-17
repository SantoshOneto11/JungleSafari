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
    [SerializeField] private float _timeToFill = 3;

    // Start is called before the first frame update
    void Start()
    {
        if (_autoReference) _imageToFill = GetComponent<Image>();
        EnableSliderFilling();
        //AdsController.Instance.OnRewardAdCloseEvent.RemoveAllListeners();
        //AdsController.Instance.OnRewardAdCloseEvent.AddListener(() => { UnityMainThreadDispatcher.Instance.Enqueue(() => { LoadSpinWheelScene(); }); });
        //AdsController.Instance.OnRewardAdFailedEvent.RemoveAllListeners();
        //AdsController.Instance.OnRewardAdFailedEvent.AddListener(() => { UnityMainThreadDispatcher.Instance.Enqueue(() => { LoadSpinWheelScene(); }); });\
    }

    public void EnableSliderFilling()
    {
        _imageToFill.DOFillAmount(1, _timeToFill).From(0).OnComplete(() =>
        {
            LoadSpinWheelScene();
        });

    }

    private static void LoadSpinWheelScene()
    {
        // Firebase.Analytics.FirebaseAnalytics.LogEvent(StringConstants.Firebase_GameSpinWheelEvent);
        SceneManager.LoadSceneAsync(StringConstants.SpinWheel);
    }
}
