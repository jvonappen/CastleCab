using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScrollElement : MonoBehaviour
{
    AutoScroll m_autoScroll;

    public void OnSelected()
    {
        m_autoScroll = m_autoScroll != null ? m_autoScroll : GetComponentInParent<AutoScroll>();
        m_autoScroll.SelectObject(gameObject);
    }
}
