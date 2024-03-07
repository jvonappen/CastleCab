using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using URNTS;

public class InputManager : MonoBehaviour
{
    [SerializeField] TrafficManager m_trafficManager;

    public void OnPlayerJoined(PlayerInput _player)
    {
        if (m_trafficManager) m_trafficManager.AddPlayer(_player.GetComponent<PlayerMovement>().horse);
    }
}
