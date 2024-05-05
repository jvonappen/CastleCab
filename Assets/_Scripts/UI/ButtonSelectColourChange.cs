using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonSelectColourChange : MonoBehaviour
{
    TextMeshProUGUI m_display;

    [SerializeField] Color m_selectedColour = Color.white;
    Color m_originalColour;

    private void Awake()
    {
        m_display = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        m_originalColour = m_display.color;
    }

    public void ChangeTextColour()
    {
        m_display.color = m_selectedColour;
    }

    public void RevertTextColour()
    {
        m_display.color = m_originalColour;
    }
}
