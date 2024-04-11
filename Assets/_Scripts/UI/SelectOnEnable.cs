using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnEnable : MonoBehaviour
{
    [SerializeField] GameObject m_button;
    EventSystem m_eventSystem;

    private void Awake()
    {
        m_eventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        m_eventSystem.SetSelectedGameObject(m_button);
    }
}
