using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AttributePointDisplay : MonoBehaviour
{
    [SerializeField] string m_displayTextPrefix = "Attribute Points: ";
    [SerializeField] string m_thousandSeparationText = ",";

    GameManager m_manager;
    TextMeshProUGUI m_display;

    void Start()
    {
        m_manager = GameManager.Instance;
        m_display = GetComponent<TextMeshProUGUI>();

        if (m_manager)
        {
            m_manager.onAttributePointsChanged += UpdateDisplay;
            UpdateDisplay();
        }
        else Debug.Log("GameManager is being referenced but it does not exist");
    }

    void UpdateDisplay(int _oldVal, int _newVal) => UpdateDisplay();
    public void UpdateDisplay() => m_display.text = m_displayTextPrefix + GetAttributePointsDisplayString();
    string GetAttributePointsDisplayString()
    {
        string str = m_manager.attributePoints.ToString();
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