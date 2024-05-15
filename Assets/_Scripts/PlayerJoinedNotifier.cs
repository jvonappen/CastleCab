using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinedNotifier : MonoBehaviour
{
    protected PlayerInputManager m_playerInputManager;
    public virtual void Awake() => m_playerInputManager = FindObjectOfType<PlayerInputManager>();
    public virtual void OnEnable()
    {
        InputManager.onPlayerJoined += OnPlayerJoined;
        InputManager.onPlayerLeft += OnPlayerLeft;

        OnPlayerUpdated();
    }

    private void OnDisable()
    {
        InputManager.onPlayerJoined -= OnPlayerJoined;
        InputManager.onPlayerLeft -= OnPlayerLeft;
    }

    public virtual void OnPlayerJoined(PlayerInput _player, List<PlayerInput> _players) => OnPlayerUpdated();
    public virtual void OnPlayerLeft(PlayerInput _player, List<PlayerInput> _players) => OnPlayerUpdated();
    public virtual void OnPlayerUpdated() { }
}
