using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSelector : MonoBehaviour
{
    List<GameObject> m_selectionlist = new();

    GameObject m_selectedObject;
    public GameObject selectedObject { get { return m_selectedObject; } }

    private void Awake()
    {
        foreach (Transform child in transform) m_selectionlist.Add(child.gameObject);
    }

    public void SelectObject(GameObject _obj)
    {
        DeselectAll();
        _obj.SetActive(true);

        m_selectedObject = _obj;
    }

    public void DeselectAll()
    {
        foreach (GameObject obj in m_selectionlist) obj.SetActive(false);
        m_selectedObject = null;
    }
}
