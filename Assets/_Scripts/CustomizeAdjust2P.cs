using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeAdjust2P : PlayerJoinedNotifier
{
    Vector2 m_defaultMin;

    public override void Awake()
    {
        base.Awake();

        m_defaultMin = transform.GetComponent<RectTransform>().offsetMin;
    }

    public override void OnPlayerUpdated() => UpdatePositionAndSize();
    void UpdatePositionAndSize()
    {
        if (m_playerInputManager)
        {
            if (m_playerInputManager.playerCount == 2) transform.GetComponent<RectTransform>().offsetMin = m_defaultMin + new Vector2(Screen.width / 4, 0);
            else transform.GetComponent<RectTransform>().offsetMin = m_defaultMin;
        }
    }
}
