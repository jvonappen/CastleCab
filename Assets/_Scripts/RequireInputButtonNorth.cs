using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RequireInputButtonNorth : MonoBehaviour
{
    [SerializeField] PlayerInputHandler m_playerInput;
    [SerializeField] MonoBehaviour m_scriptToRequire;

    private void Start()
    {
        m_playerInput.m_playerControls.Controls.StatsMenu.performed += OnPerformed;
        m_playerInput.m_playerControls.Controls.StatsMenu.canceled += OnCanceled;
    }

    void OnPerformed(InputAction.CallbackContext context) => m_scriptToRequire.enabled = true;
    void OnCanceled(InputAction.CallbackContext context) => m_scriptToRequire.enabled = false;
}
