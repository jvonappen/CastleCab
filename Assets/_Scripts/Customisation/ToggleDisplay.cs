using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToggleDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_text1, m_text2;
    [SerializeField] RectTransform m_selectorBar;


    public void Display1() => Display(m_text1);
    public void Display2() => Display(m_text2);
    void Display(TextMeshProUGUI _text)
    {
        m_text1.alpha = 100f/255f;
        m_text2.alpha = 100f/255f;

        _text.alpha = 1;

        m_selectorBar.DOAnchorPosX(_text.GetComponent<RectTransform>().anchoredPosition.x, 0.3f).SetEase(Ease.OutExpo);
    }
}
