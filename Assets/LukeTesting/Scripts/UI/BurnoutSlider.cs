using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnoutSlider : MonoBehaviour
{
    [SerializeField] private RectTransform _slider;
    [SerializeField] private float _fillAmount;
    private float _sliderValue;
    [SerializeField] private Vector2 _fill;

    private void Awake()
    {
        _slider = GetComponent<RectTransform>();
        _fillAmount = _slider.rect.width;
        _slider.sizeDelta = new Vector2(0, _slider.rect.height);
    }

    public void BurnoutCharge(float value)
    {
        _fill = new Vector2(value, _slider.rect.height);
        _slider.sizeDelta = _fill;

    }
}
