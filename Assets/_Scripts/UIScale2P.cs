using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIScale2P : MonoBehaviour
{
    PlayerInputManager m_playerInputManager;

    private void Awake()
    {
        m_playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable() => UpdateScale();
    public void OnPlayerJoined(PlayerInput _player) => UpdateScale();

    void UpdateScale()
    {
        if (m_playerInputManager.playerCount == 2) transform.localScale = Vector3.one * 0.5f;
        else transform.localScale = Vector3.one;
    }
}
