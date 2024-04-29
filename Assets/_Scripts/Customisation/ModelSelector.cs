using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSelector : MonoBehaviour
{
    [Tooltip("Index to differentiate different types e.g. hats, wheels, wagons")] public int m_typeIndex;
    //string m_selectorType;

    List<GameObject> m_selectionlist = new();

    GameObject m_selectedObject;
    public GameObject selectedObject { get { return m_selectedObject; } }

    private void Awake()
    {
        foreach (Transform child in transform) m_selectionlist.Add(child.gameObject);
    }

    public Material GetMat() => selectedObject.GetComponent<Renderer>().sharedMaterial;
    public Material InstanceMat() => new(GetMat());
    public void SetMat(Material _mat) => selectedObject.GetComponent<Renderer>().sharedMaterial = _mat;

    public void SelectObjectByIndex(int _index) => SelectObject(transform.GetChild(_index).gameObject);

    public void SelectObject(GameObject _obj)
    {
        DeselectAll();
        _obj.SetActive(true);

        m_selectedObject = _obj;

        ModelSettings settings = _obj.GetComponent<ModelSettings>();
        if (settings)
        {
            Material mat = InstanceMat();
            SetMat(mat);

            mat.SetTexture("_Base_Colour", settings.baseColour);
            mat.SetTexture("_Mask_Map", settings.maskMap);
        }
    }

    public void DeselectAll()
    {
        foreach (GameObject obj in m_selectionlist) obj.SetActive(false);
        m_selectedObject = null;
    }

    public void CopySelectionToSelector(ModelSelector _modelSelector)
    {
        if (selectedObject)
        {
            _modelSelector.SelectObjectByIndex(selectedObject.transform.GetSiblingIndex());
            _modelSelector.SetMat(GetMat());
        }
        else _modelSelector.DeselectAll();
    }
}
