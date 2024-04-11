using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour
{
    public static Action onWindowResize;

    float m_previousWidth, m_previousHeight;

    private void Awake()
    {
        m_previousWidth = Screen.width;
        m_previousHeight = Screen.height;
    }

    private void Update()
    {
        if (Screen.width != m_previousWidth || Screen.height != m_previousHeight)
        {
            onWindowResize?.Invoke();

            m_previousWidth = Screen.width;
            m_previousHeight = Screen.height;
        }
    }
}
