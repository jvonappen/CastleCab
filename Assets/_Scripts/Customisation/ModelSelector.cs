using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSelector : MonoBehaviour
{
    [Tooltip("Index to differentiate different types e.g. hats, wheels, wagons")] public int m_typeIndex;
    [SerializeField] bool m_indexZeroIsNone;

    [HideInInspector] public MultiColourSelector colourSelector;

    List<GameObject> m_selectionlist = new();

    GameObject m_previewObject;
    public GameObject previewObject { get { return m_previewObject; } }

    GameObject m_selectedObject;
    public GameObject selectedObject { get { return m_selectedObject; } }

    public Action onModelSelect;

    private void Awake()
    {
        colourSelector = GetComponent<MultiColourSelector>();

        foreach (Transform child in transform)
        {
            m_selectionlist.Add(child.gameObject);
            child.gameObject.AddComponent<ModelSettings>();
        }
    }

    public Material GetMat()
    {
        if (previewObject) return previewObject.GetComponent<Renderer>().sharedMaterial;
        else return null;
    }
    public Material InstanceMat() => new(GetMat());
    public void SetMat(Material _mat) => previewObject.GetComponent<Renderer>().sharedMaterial = _mat;

    public void SelectObject() => m_selectedObject = m_previewObject;
    public void SelectSelected()
    {
        if (m_selectedObject) PreviewObject(m_selectedObject);
        else DeselectAll();

        onModelSelect?.Invoke();
    }

    public void PreviewObjectByIndex(int _index)
    {
        if (!m_indexZeroIsNone) _index++;

        if (_index == 0) DeselectAll();
        else PreviewObject(transform.GetChild(_index - 1).gameObject);

        onModelSelect?.Invoke();
    }

    public void PreviewObject(GameObject _obj)
    {
        DeselectAll();
        _obj.SetActive(true);

        m_previewObject = _obj;

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
        m_previewObject = null;
    }

    public void CopySelectionToSelector(ModelSelector _modelSelector)
    {
        if (previewObject)
        {
            _modelSelector.PreviewObjectByIndex(GetSelectedIndex());
            _modelSelector.SetMat(GetMat());
        }
        else _modelSelector.DeselectAll();
    }

    public int GetSelectedIndex()
    {
        if (previewObject) return previewObject.transform.GetSiblingIndex() + 1;
        else return 0;
    }
}
