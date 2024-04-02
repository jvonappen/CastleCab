using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using URNTS;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject m_playerPrefab;
    bool m_firstPlayerSpawned;

    public static Action<PlayerInput, List<PlayerInput>> onPlayerJoined;
    public static Action<PlayerInput, List<PlayerInput>> onPlayerLeft;

    [SerializeField] TrafficManager m_trafficManager;
    List<PlayerInput> m_players = new();

    public void OnPlayerJoined(PlayerInput _player)
    {
        m_players.Add(_player);

        onPlayerJoined?.Invoke(_player, m_players);

        if (m_trafficManager) m_trafficManager.AddPlayer(_player.GetComponent<PlayerMovement>().horse);
    }

    public void OnPlayerLeft(PlayerInput _player)
    {
        m_players.Remove(_player);
    
        onPlayerLeft?.Invoke(_player, m_players);
    
        if (m_trafficManager) m_trafficManager.RemovePlayer(_player.GetComponent<PlayerMovement>().horse);
    }

    private void Awake()
    {
        InputUser.onUnpairedDeviceUsed += UnpairedDeviceUsed;
    }

    public void UnpairedDeviceUsed(InputControl _inputControl, InputEventPtr _inputEventPtr)
    {
        if (!m_firstPlayerSpawned) m_firstPlayerSpawned = true;
        else Instantiate(m_playerPrefab);

        foreach (PlayerInputHandler playerInput in FindObjectsOfType<PlayerInputHandler>())
        {
            playerInput.UnpairedDeviceUsed(_inputControl, _inputEventPtr);
        }
    }
}
