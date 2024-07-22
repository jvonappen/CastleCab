using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    [SerializeField] float m_timeToFade = 1.5f;
    TextMeshProUGUI m_display;

    public void DisplayText(string _text)
    {
        m_display = m_display != null ? m_display : GetComponent<TextMeshProUGUI>();

        m_display.text = _text;
        m_display.gameObject.SetActive(true);
        SimpleFadeTMPUGUI.Begin(gameObject, m_timeToFade, FadeEndAction.Inactive);
    }
}
