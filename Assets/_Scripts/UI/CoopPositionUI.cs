using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (m_transform)
            {
                if (m_playerInputManager.playerCount > 1) m_transform.anchoredPosition = new Vector3((Screen.width / 2) * m_coopPos.x, (Screen.height / 2) * m_coopPos.y, m_coopPos.z);
                else m_transform.anchoredPosition = m_originalPos;
            }
        }
    }

}
