using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerControls m_playerControls { get; private set; }
    PlayerInput m_playerInput;

    private void Awake()
    {
        m_playerControls = new PlayerControls();
        m_playerInput = GetComponent<PlayerInput>();

        m_playerInput.actions = m_playerControls.asset;

        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable() => m_playerControls.Enable();


}
