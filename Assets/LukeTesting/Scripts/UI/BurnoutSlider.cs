using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BurnoutSlider : MonoBehaviour
{
    [SerializeField] private RectTransform _slider;
    [SerializeField] private float _fillAmount;
    [SerializeField] private float _sliderValue;
    [SerializeField] private Vector2 _fill;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Camera _camera;
    private bool _burnout = false;
    private bool _pulse = false;
    private Tween _pulseTween;

    private void Awake()
    {
        _slider = GetComponent<RectTransform>();
        _fillAmount = _slider.rect.width;
        _slider.sizeDelta = new Vector2(0, _slider.rect.height);
        _canvas = GetComponentInParent<Canvas>();
        _camera = FindObjectOfType<Camera>();
        _pulseTween = _canvas.gameObject.transform.DOScale(_canvas.transform.localScale * 1.5f, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        //constanlt have burnout guage look at player
        if (_burnout)
        {
            _canvas.gameObject.transform.LookAt(_camera.gameObject.transform);
            _canvas.transform.rotation = Quaternion.LookRotation(_canvas.transform.position - _camera.gameObject.transform.position);
        }
    }

    public void BurnoutCharge(float value)
    {
        _burnout = true;
        _canvas.gameObject.SetActive(true);
        _sliderValue = Mathf.InverseLerp(10, 3, value); //magic math to get percentage of 
        _sliderValue *= _fillAmount;
        _fill = new Vector2(_sliderValue, _slider.rect.height) ;
        _slider.sizeDelta = _fill;
        if (_sliderValue == _fillAmount) //start tween when guage is full
        {
            SliderPulse();
        }
    }

    public void ResetSlider()
    {
        _pulseTween.Kill(); //kill tween
        _canvas.gameObject.transform.localScale = Vector3.one; //reset canvas scale
        _burnout = false;
        _fill = new Vector2(0, _slider.rect.height);
        _slider.sizeDelta = _fill;
        if (_canvas.gameObject.activeSelf == true) _canvas.gameObject.SetActive(false);
    }

    private void SliderPulse()
    {
        if (_pulseTween.IsPlaying()) return;
        _pulseTween = _canvas.gameObject.transform.DOScale(_canvas.transform.localScale * 1.5f, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo); //set tween
        _pulseTween.Play();
    }
}
