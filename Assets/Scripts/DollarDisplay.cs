using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DollarDisplay : MonoBehaviour
{
    public static int dollarValue = 0;

    [SerializeField] public TextMeshProUGUI dollarDisplay;

    [SerializeField] private int startingDollars;

    private void Start()
    {
        dollarValue = startingDollars;
        dollarDisplay.text = dollarValue.ToString();
    }

    private void FixedUpdate()
    {
        UpdateDollarDisplay();
    }
    public void UpdateDollarDisplay()
    {
        dollarDisplay.text = dollarValue.ToString();
    }

    
}
