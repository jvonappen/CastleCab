using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using URNTS;

public class InputManager : MonoBehaviour
{
    [SerializeField] TrafficManager m_trafficManager;
    List<PlayerInput> m_players = new();

    public void OnPlayerJoined(PlayerInput _player)
    {
        m_players.Add(_player);

        foreach (PlayerInput player in m_players) player.GetComponent<PlayerMovement>().OnPlayerJoined(_player, m_players);

        if (m_trafficManager) m_trafficManager.AddPlayer(_player.GetComponent<PlayerMovement>().horse);
    }
}
