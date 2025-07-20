using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowTimer : MonoBehaviour
{
    // This script is used to show the timer in the UI
    // It will display the time left before night
    public GlobalData globalData; 
    public TMPro.TextMeshProUGUI timerTextTMP; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        globalData.TimeSetDown(Time.deltaTime);

        timerTextTMP.text = $"Time Left: {globalData.secondsLeftBeforeNight}";
        Debug.Log($"secondsLeftBeforeNight: {globalData.secondsLeftBeforeNight}");
    }
}

