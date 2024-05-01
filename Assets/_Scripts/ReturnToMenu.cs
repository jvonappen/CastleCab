using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReturnToMenu : MonoBehaviour
{
    PlayerControls m_playerControls;
    private void Awake() => m_playerControls = GetComponent<PlayerInputHandler>().m_playerControls;

    private void OnEnable() => m_playerControls.Controls.ReturnMenu.performed += OpenMenu;
    private void OnDisable() => m_playerControls.Controls.ReturnMenu.performed -= OpenMenu;

    void OpenMenu(InputAction.CallbackContext context)
    {
        GameManager manager = GameManager.Instance;

        GameManager.SetCustomizing(false);
        manager.ClearPlayers();
        manager.LoadScene("StartMenu");
    }
}
