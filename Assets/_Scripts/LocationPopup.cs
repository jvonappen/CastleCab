using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationPopup : MonoBehaviour
{
    [SerializeField] float m_fadeTime = 3;
    [SerializeField] GameObject m_display;
    TextMeshProUGUI m_displayText;

    BaseAlphaController m_alphaController;
    private void Awake()
    {
        m_displayText = m_display.GetComponentInChildren<TextMeshProUGUI>();
        m_alphaController = m_display.GetComponent<BaseAlphaController>();

        m_display.gameObject.SetActive(false);
    }

    public void Display(string _text)
    {
        m_display.gameObject.SetActive(true);

        m_alphaController.FadeOut(3);
        m_displayText.text = _text;
    }
}
