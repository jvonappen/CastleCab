using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoopPositionUI : PlayerJoinedNotifier
{
    RectTransform m_transform;

    Vector3 m_originalPos;
    [SerializeField] Vector3 m_coopPos;

    public override void Awake()
    {
        base.Awake();

        m_transform = GetComponent<RectTransform>();
        m_originalPos = m_transform.anchoredPosition;
    }

    public override void OnPlayerUpdated() => UpdatePos();

    public void UpdatePos()
    {
        if (m_playerInputManager)
        {
            if (m_playerInputManager.playerCount > 1) m_transform.anchoredPosition = m_coopPos;
            else m_transform.anchoredPosition = m_originalPos;
        }
    }

}
