using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AttributePointDisplay : MonoBehaviour
{
    [SerializeField] PlayerUpgrades m_playerUpgrades;

    [SerializeField] string m_displayTextPrefix = "Attribute Points: ";
    [SerializeField] string m_thousandSeparationText = ",";

    TextMeshProUGUI m_display;

    void Start()
    {
        m_display = GetComponent<TextMeshProUGUI>();

        m_playerUpgrades.onAttributePointsChanged += UpdateDisplay;
        UpdateDisplay();
    }

    void UpdateDisplay(int _oldVal, int _newVal) => UpdateDisplay();
    public void UpdateDisplay() => m_display.text = m_displayTextPrefix + GetAttributePointsDisplayString();
    string GetAttributePointsDisplayString()
    {
        string str = m_playerUpgrades.attributePoints.ToString();
        int spaces = (str.Length - 1) / 3;

        int index = str.Length;
        for (int i = 0; i < spaces; i++)
        {
            index -= 3;
            str = str.Insert(index, m_thousandSeparationText);
        }

        return str;
    }
}