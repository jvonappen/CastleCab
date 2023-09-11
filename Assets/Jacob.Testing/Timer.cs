using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timerValue = 60;

    private void Awake()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Update()
    {
        if(timerValue > 0)
        {
            timerValue = timerValue - 1 * Time.deltaTime;
            timerText.text = timerValue.ToString("00");
        }
        if(timerValue <= 0)
        {
            timerText.text = "fail";
        }
        

    }
}
