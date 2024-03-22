using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using URNTS;

public class InputManager : MonoBehaviour
{
    public static Action<PlayerInput, List<PlayerInput>> onPlayerJoined;
    public static Action<PlayerInput, List<PlayerInput>> onPlayerLeft;

    [SerializeField] TrafficManager m_trafficManager;
    List<PlayerInput> m_players = new();

    public void OnPlayerJoined(PlayerInput _player)
    {
        m_players.Add(_player);

        onPlayerJoined?.Invoke(_player, m_players);
        //foreach (PlayerJoinedNotifier o in FindObjectsOfType<PlayerJoinedNotifier>()) o.OnPlayerJoined(_player);

        if (m_trafficManager) m_trafficManager.AddPlayer(_player.GetComponent<PlayerMovement>().horse);
    }

    public void OnPlayerLeft(PlayerInput _player)
    {
        m_players.Remove(_player);
    
        onPlayerLeft?.Invoke(_player, m_players);
        //foreach (PlayerJoinedNotifier o in FindObjectsOfType<PlayerJoinedNotifier>()) o.OnPlayerJoined(_player);
    
        if (m_trafficManager) m_trafficManager.RemovePlayer(_player.GetComponent<PlayerMovement>().horse);
    }
}
