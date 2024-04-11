using UnityEngine;

public class UIScale2P : PlayerJoinedNotifier
{
    public override void OnPlayerUpdated() => UpdateScale();

    void UpdateScale()
    {
        if (m_playerInputManager)
        {
            if (m_playerInputManager.playerCount == 2) transform.localScale = Vector3.one * 0.5f;
            else transform.localScale = Vector3.one;
        }
    }
}
