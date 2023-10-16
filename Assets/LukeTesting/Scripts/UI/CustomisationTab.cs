using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomisationTab : MonoBehaviour
{
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private GameObject _horseHat;
    [SerializeField] private GameObject _cartModel;
    [SerializeField] private Tab _tabs;
    [SerializeField] private int index = 0;

    private void Awake()
    {
        _tabs = GetComponent<Tab>();   
        _leftButton.onClick.AddListener(OnLeftButtonClicked);
        _rightButton.onClick.AddListener(OnRightButtonClicked);
        _text.text = _tabs.tabOption[index].ToString();
    }

    private void OnLeftButtonClicked()
    {
        Debug.Log("LeftClicked");
        if (index == 0) index = _tabs.tabOption.Count - 1;
        else index -= 1;
        _text.text = _tabs.tabOption[index].ToString();
        if (_meshRenderer != null) _meshRenderer.material = _tabs.colorOption[index];
        if (_skinnedMeshRenderer != null) _skinnedMeshRenderer.material.SetTexture("_BaseMap", _tabs.texture2D[index]);
        //if (_tabs.modelOption != null) //set new cart mesh or hat mesh
    }

    private void OnRightButtonClicked()
    {
        Debug.Log("RightClicked");
        if (index == _tabs.tabOption.Count - 1) index = 0;
        else index += 1;
        _text.text = _tabs.tabOption[index].ToString();
        if (_meshRenderer != null) _meshRenderer.material = _tabs.colorOption[index];
        if (_skinnedMeshRenderer != null) _skinnedMeshRenderer.material.SetTexture("_BaseMap", _tabs.texture2D[index]);
        //if (_tabs.modelOption != null) //set new cart mesh or hat mesh
    }
}
