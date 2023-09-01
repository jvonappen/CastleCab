using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DollarDisplay : MonoBehaviour
{
    public static int dollarValue = 0;

    private TextMeshProUGUI _dollarDisplay;
    [SerializeField] private int startingDollars;

    private void Awake()
    {
        _dollarDisplay = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        dollarValue = startingDollars;
        _dollarDisplay.text = dollarValue.ToString();
    }

    private void FixedUpdate()
    {
        UpdateDollarDisplay();
    }
    public void UpdateDollarDisplay()
    {
        _dollarDisplay.text = dollarValue.ToString();
    }

    
}
