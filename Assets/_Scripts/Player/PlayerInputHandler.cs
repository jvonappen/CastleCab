using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerControls m_playerControls { get; private set; }
    PlayerInput m_playerInput;

    bool m_paired;

    private void Awake()
    {
        m_playerControls = new PlayerControls();
        m_playerInput = GetComponent<PlayerInput>();

        m_playerInput.actions = m_playerControls.asset;
    }
    private void OnEnable() => m_playerControls.Enable();

    public void UnpairedDeviceUsed(InputControl _inputControl, InputEventPtr _inputEventPtr)
    {
        if (m_paired) return;

        m_paired = true;
        m_playerInput.SwitchCurrentControlScheme(_inputControl.device);

        GameManager.Instance.AddPlayer(gameObject);
    }

}
