using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPos3pUI : PlayerJoinedNotifier
{
    [SerializeField] Vector2 m_screenCoords;
    Vector2 m_originalPos;

    RectTransform rt;

    public override void Awake()
    {
        base.Awake();

        rt = GetComponent<RectTransform>();
        m_originalPos = rt.anchoredPosition;

        ScreenSize.onWindowResize += WindowResized;
    }

    bool m_windowResize;
    void WindowResized()
    {
        m_windowResize = true;
        OnPlayerUpdated();
    }

    public override void OnPlayerUpdated()
    {
        base.OnPlayerUpdated();

        int requiredPlayers = 2;
        if (m_windowResize == true) requiredPlayers = 3;

        Vector2 newPos = m_originalPos;
        if (GameManager.Instance)
        {
            if (GameManager.Instance.players.Count == requiredPlayers) newPos = new Vector2(Screen.width * m_screenCoords.x, Screen.height * m_screenCoords.y);
        }

        rt.anchoredPosition = newPos;

        m_windowResize = false;
    }
}
