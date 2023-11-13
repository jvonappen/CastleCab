using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomisationTab : MonoBehaviour
{
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private MeshRenderer _cartMeshRenderer;
    [SerializeField] private List<MeshRenderer> _wheelMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer _horseSkinnedMeshRenderer;
    [SerializeField] private Transform _hatPos;
    [SerializeField] private SetMaterials _setMaterials;
    [SerializeField] private Tab _tabs;
    [SerializeField] private int index = 0;
    [SerializeField] private string _saveString;
    [SerializeField] private Transform right, left;

    private void Awake()
    {
        _setMaterials = FindObjectOfType<SetMaterials>();
        right = transform.Find("Right");
        left = transform.Find("Left");
        _tabs = GetComponent<Tab>();   
        _leftButton.onClick.AddListener(OnLeftButtonClicked);
        _rightButton.onClick.AddListener(OnRightButtonClicked);
        _saveString = this.gameObject.name; //set string to save index
        LoadData(); //load the saved index 

        //set customisation details to correct index
        _text.text = _tabs.tabOption[index].ToString();
        ChangeMaterials(index);
    }

    private void OnLeftButtonClicked()
    {
        left.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        left.DOScale(1f, 0.3f).SetEase(Ease.OutElastic);

        //set index
        if (index == 0) index = _tabs.tabOption.Count - 1;
        else index -= 1;
        ChangeMaterials(index);
        SaveData();
    }

    private void OnRightButtonClicked()
    {
        right.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        right.DOScale(1f, 0.3f).SetEase(Ease.OutElastic);

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
        if (_cartMeshRenderer != null)
        {
            _cartMeshRenderer.material = _tabs.colorOption[index];
            _setMaterials.SetCartMaterials(_tabs.colorOption[index]);
        }
        
        //chnage horse colour
        if (_horseSkinnedMeshRenderer != null)
        {
            _horseSkinnedMeshRenderer.material.SetTexture("_1st_ShadeMap", _tabs.texture2D[index]);
            _horseSkinnedMeshRenderer.material.SetTexture("_MainTex", _tabs.texture2D[index]);
            _setMaterials.SetHorseMaterials(_tabs.texture2D[index]);
        }

        //change wheel colour
        if (_wheelMeshRenderer != null)
        {
            foreach (MeshRenderer wheel in _wheelMeshRenderer)
            {
                wheel.material = _tabs.colorOption[index];
                _setMaterials.SetWheelsMaterials(_tabs.colorOption[index]);
            }
        }

        //spawn hats
        if (_hatPos != null)
        {
            if (_hatPos.childCount == 0 && _tabs.modelOption[index] != null)
            {
                GameObject hat = Instantiate(_tabs.modelOption[index], _hatPos);
                hat.transform.parent = _hatPos.transform;
                _setMaterials.SetHatObject(_tabs.modelOption[index]);
            }
            else
            {
                if (_hatPos.childCount != 0) Destroy(_hatPos.GetChild(0).gameObject);
                if (_tabs.modelOption[index] != null)
                {
                    GameObject hat = Instantiate(_tabs.modelOption[index], _hatPos);
                    hat.transform.parent = _hatPos.transform;
                    _setMaterials.SetHatObject(_tabs.modelOption[index]);
                }
            }
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(_saveString, index);
    }

    public void LoadData()
    {
        index = PlayerPrefs.GetInt(_saveString, index);
    }

    public void ResetCart()
    {
        PlayerPrefs.DeleteKey(_saveString);
        index = 0;
        _text.text = _tabs.tabOption[index].ToString();
        ChangeMaterials(index);
    }
}
