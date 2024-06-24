using UnityEngine;

public class UIScale2P : PlayerJoinedNotifier
{
    [SerializeField] Canvas m_canvas;

    public override void OnPlayerUpdated() => UpdateScale();

    void UpdateScale()
    {
        if (m_playerInputManager)
        {
            RectTransform rt = GetComponent<RectTransform>();

            if (m_playerInputManager.playerCount == 2)
            {
                rt.offsetMin = new Vector2(m_canvas.pixelRect.width / 4, rt.offsetMin.y);
                rt.offsetMax = new Vector2(-m_canvas.pixelRect.width / 4, rt.offsetMin.y);
            }
            else
            {
                rt.offsetMin = new Vector2(0, rt.offsetMin.y);
                rt.offsetMax = new Vector2(0, rt.offsetMin.y);
            }
        }
    }
}
