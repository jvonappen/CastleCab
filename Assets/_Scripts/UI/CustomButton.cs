using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomButton : MonoBehaviour
{
    [SerializeField] bool m_selectOnEnable;
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
            onButtonClicked?.Invoke();
        }
        
    }
}
