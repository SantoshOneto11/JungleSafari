using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    private Text TimerText;
    private TextMeshProUGUI TimerTextMesh;
    private float nextTime = 0;
    //public float disabledTime = 0;   // Store the time when the object was disabled
    public long TimeInSeconds;
    public UnityEvent TimerEndEvent = new UnityEvent();
    public UnityEvent OnResetEvent = new UnityEvent();
    [SerializeField] private int interval = 1;

    [SerializeField] private long timeRemaining;

    public bool wasDisabled = false;
    private void Awake()
    {
        TimerText = GetComponent<Text>();
        TimerTextMesh = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        ResetTimer();
    }

    void Update()
    {
        if (timeRemaining >= 0)
        {
            if (Time.time >= nextTime)
            {
                nextTime += interval;
                timeRemaining -= 1;
                TimeSpan duration = new TimeSpan(0, 0, 0, (int)timeRemaining);
                if (TimerText != null)
                    TimerText.text = duration.TotalSeconds > 0 ? duration.ToString(@"mm\:ss") : "00:00";
                if (TimerTextMesh != null)
                    TimerTextMesh.text = duration.TotalSeconds > 0 ? duration.ToString(@"mm\:ss") : "00:00";
                if (duration.TotalSeconds < 0)
                {
                    if (TimerText != null)
                        TimerText.text = "";
                    if (TimerTextMesh != null)
                        TimerTextMesh.text = "";
                    Utility.myLog("Finshed -------------");
                    TimerEndEvent.Invoke();
                }
            }
        }
        
    }
    public void ResetTimer()
    {
        OnResetEvent?.Invoke();
        Utility.myLog("Reset Time");
        timeRemaining = TimeInSeconds;
        nextTime = Time.time;
    }
}
