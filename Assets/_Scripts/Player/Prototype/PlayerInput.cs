using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerControls m_playerControls { get; private set; }

    private void Awake()
    {
        m_playerControls = new PlayerControls();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable() => m_playerControls.Enable();
}
