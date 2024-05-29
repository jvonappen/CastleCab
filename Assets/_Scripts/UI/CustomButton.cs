using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    [SerializeField] ButtonSelectHelper m_selectHelper;

    bool m_isClickSelected;
    public bool isClickSelected { get { return m_isClickSelected; } }

    public UnityEvent onOtherButtonClicked;
    public UnityEvent onButtonClicked;

    public void Deselect()
    {
        if (m_isClickSelected)
        {
            m_isClickSelected = false;
            onOtherButtonClicked?.Invoke();
        }
    }

    public void Select()
    {
        if (!m_isClickSelected)
        {
            m_isClickSelected = true;
            m_selectHelper.UpdateButtonInteractions();
            onButtonClicked?.Invoke();
        }
    }

    public void SetInteractable(bool _canInteract) => GetComponent<Button>().interactable = _canInteract;
}
