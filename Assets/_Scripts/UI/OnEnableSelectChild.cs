using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnEnableSelectChild : MonoBehaviour
{
    [SerializeField] EventSystem m_eventSystem;

    private void OnEnable()
    {
        TimerManager.RunAfterTime(() =>
        {
            m_eventSystem.SetSelectedGameObject(transform.GetChild(0).gameObject);
        }, 0.1f);
    }
}
