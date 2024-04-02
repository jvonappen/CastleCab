using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenCloseMenu : MonoBehaviour
{
    [SerializeField] PlayerInputHandler m_playerInput;
    [SerializeField] GameObject m_menu;

    private void Start()
    {
        m_playerInput.m_playerControls.Controls.StatsMenu.performed += ToggleMenu;
    }

    void ToggleMenu(InputAction.CallbackContext context) => m_menu.SetActive(!m_menu.activeSelf);
}
