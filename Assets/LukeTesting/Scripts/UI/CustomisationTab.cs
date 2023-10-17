using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering.Universal.ShaderGraph;
using UnityEditor.Rendering.Universal.Toon.ShaderGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomisationTab : MonoBehaviour
{
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private MeshRenderer _cartMeshRenderer;
    [SerializeField] private List<MeshRenderer> _wheelMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer _horseSkinnedMeshRenderer;
    [SerializeField] private GameObject _horseHat;
    [SerializeField] private GameObject _cartModel;
    [SerializeField] private Transform _hatPos;
    [SerializeField] private Tab _tabs;
    [SerializeField] private int index = 0;
    [SerializeField] private string _saveString;

    private void Awake()
    {
        _tabs = GetComponent<Tab>();   
        _leftButton.onClick.AddListener(OnLeftButtonClicked);
        _rightButton.onClick.AddListener(OnRightButtonClicked);
        _saveString = this.gameObject.name;
        LoadData();
        _text.text = _tabs.tabOption[index].ToString();
        ChangeMaterials(index);
    }

    private void OnLeftButtonClicked()
    {
        //set index
        if (index == 0) index = _tabs.tabOption.Count - 1;
        else index -= 1;
        ChangeMaterials(index);
        SaveData();
    }

    private void OnRightButtonClicked()
    {
        //set index
        if (index == _tabs.tabOption.Count - 1) index = 0;
        else index += 1;
        ChangeMaterials(index);
        SaveData();
    }

    private void ChangeMaterials(int index)
    {
        //change text
        _text.text = _tabs.tabOption[index].ToString();

        //change cart material
        if (_cartMeshRenderer != null) _cartMeshRenderer.material = _tabs.colorOption[index];

        //chnage horse colour
        if (_horseSkinnedMeshRenderer != null)
        {
            _horseSkinnedMeshRenderer.material.SetTexture("_1st_ShadeMap", _tabs.texture2D[index]);
            _horseSkinnedMeshRenderer.material.SetTexture("_MainTex", _tabs.texture2D[index]);
        }

        //change wheel colour
        if (_wheelMeshRenderer != null)
        {
            foreach (MeshRenderer wheel in _wheelMeshRenderer)
            {
                wheel.material = _tabs.colorOption[index];
            }
        }

        //spawn hats
        if (_hatPos != null)
        {
            if (_hatPos.childCount == 0 && _tabs.modelOption[index] != null)
            {
                GameObject hat = Instantiate(_tabs.modelOption[index], _hatPos);
                hat.transform.parent = _hatPos.transform;
            }
            else
            {
                if (_hatPos.childCount != 0) Destroy(_hatPos.GetChild(0).gameObject);
                if (_tabs.modelOption[index] != null)
                {
                    GameObject hat = Instantiate(_tabs.modelOption[index], _hatPos);
                    hat.transform.parent = _hatPos.transform;
                }
            }
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(_saveString, index);
        Debug.Log("Save index");
    }

    public void LoadData()
    {
        index = PlayerPrefs.GetInt(_saveString, index);
        Debug.Log("Load index");
    }
}
