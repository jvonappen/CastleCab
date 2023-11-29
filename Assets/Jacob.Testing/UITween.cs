using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class UITween : MonoBehaviour
{
    [SerializeField] private float _fadeTime = 1f;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rectTransform;
    [Space]
    [SerializeField] private List<GameObject> uiBox = new List<GameObject>();
    [SerializeField] private float _boxFadeTime = 1f;
    [SerializeField] private float _betweenPopUpTime = 0.25f;


    [SerializeField] private PlayerInput _playerInput;
    private bool _showUI;

    private void Update()
    {
        if (_playerInput._playerControls.Controls.Achievement.WasPressedThisFrame() /*Input.GetKeyUp(KeyCode.T*/)
        {
            _showUI = !_showUI;
            if (_showUI == true)
            { UIFadeIn(); }
            if (_showUI == false)
            { UIFadeOut(); }
        }
    }
    public void UIFadeIn()
    {
        _canvasGroup.alpha = 0;
        _rectTransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        _rectTransform.DOAnchorPos(new Vector2(0f, 0f), _fadeTime, false).SetEase(Ease.OutElastic);
        _canvasGroup.DOFade(1, _fadeTime);
        StartCoroutine(UIAnimation());

        Debug.Log("Fade in");
    }

    public void UIFadeOut()
    {
        _canvasGroup.alpha = 1;
        _rectTransform.transform.localPosition = new Vector3(0f, 0, 0f);
        _rectTransform.DOAnchorPos(new Vector2(0f, -1000f), _fadeTime, false).SetEase(Ease.OutBounce);
        _canvasGroup.DOFade(0, _fadeTime);

        Debug.Log("Fade Out");
    }

    IEnumerator UIAnimation()
    {
        foreach(var item in uiBox)
        {
            item.transform.localScale = Vector3.zero;
        }
        foreach (var item in uiBox)
        {
            item.transform.DOScale(1f, _boxFadeTime).SetEase(Ease.OutFlash);
            yield return new WaitForSeconds(_betweenPopUpTime);
        }
    }
}
