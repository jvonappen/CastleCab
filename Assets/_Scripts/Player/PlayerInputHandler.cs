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
    public PlayerInput playerInput { get { return m_playerInput; } }

    bool m_paired;
    public void SetPaired(bool _isPaired) => m_paired = _isPaired;

    private void Awake()
    {
        m_playerControls = new PlayerControls();
        m_playerInput = GetComponent<PlayerInput>();

        m_playerInput.actions = m_playerControls.asset;
    }
    private void OnEnable() => m_playerControls.Enable();

    public bool PairDevice(InputDevice _device)
    {
        if (m_paired) return false;

        m_paired = true;
        m_playerInput.SwitchCurrentControlScheme(_device);

        if (GameManager.Instance) GameManager.Instance.AddPlayer(gameObject);

        return true;
    }

}
