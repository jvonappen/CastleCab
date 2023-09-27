using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] public float timerValue = 60;
    [SerializeField] private GameObject _failedUI;

    [Header("Debug")]
    [SerializeField] private bool hasFailed = false;
    [SerializeField] private float ticker = 0;
    private float _hideAfter = 3;

    private void Awake()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();


        _failedUI.SetActive(false);
        hasFailed = false;
    }
    void Update()
    {
        if(timerValue > 0)
        {
            timerText.enabled = true;
            timerText.color = Color.white;
            timerValue = timerValue - 1 * Time.deltaTime;
            timerText.text = timerValue.ToString("00");
        }
        if(timerValue <= 0)
        {
            //timerText.color = Color.red;
            //timerText.text = "fail";
            timerText.enabled = false;

            _failedUI.SetActive(true);
            hasFailed = true;
            
        }
        if(hasFailed == true)
        {
            ticker = ticker + 1 * Time.deltaTime;
        }
        if(ticker >= _hideAfter) { hasFailed = false; _failedUI.SetActive(false); ticker = 0; }
        

    }
}
