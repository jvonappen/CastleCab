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

    public Material GetMat()
    {
        if (selectedObject) return selectedObject.GetComponent<Renderer>().sharedMaterial;
        else return null;
    }
    public Material InstanceMat() => new(GetMat());
    public void SetMat(Material _mat) => selectedObject.GetComponent<Renderer>().sharedMaterial = _mat;

    public void SelectObjectByIndex(int _index)
    {
        if (_index == 0) DeselectAll();
        else SelectObject(transform.GetChild(_index - 1).gameObject);
    }

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
            _modelSelector.SelectObjectByIndex(GetSelectedIndex());
            _modelSelector.SetMat(GetMat());
        }
        else _modelSelector.DeselectAll();
    }

    public int GetSelectedIndex()
    {
        if (selectedObject) return selectedObject.transform.GetSiblingIndex() + 1;
        else return 0;
    }
}
