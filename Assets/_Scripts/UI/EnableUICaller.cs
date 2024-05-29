using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableUICaller : MonoBehaviour
{
    [SerializeField] UnityEvent m_onEnable;
    private void OnEnable() => m_onEnable?.Invoke();
}
