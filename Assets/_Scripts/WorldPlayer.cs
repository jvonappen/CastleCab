using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldPlayer : MonoBehaviour
{
    PlayerInput m_playerInput;
    public PlayerInput playerInput { get { return m_playerInput; } }
    public void SetOtherPlayerInput(PlayerInput _input) => m_playerInput = _input;

    PlayerInputHandler m_input;
    private void Awake()
    {
        m_input = GetComponent<PlayerInputHandler>();
        m_input.m_playerControls.UI.Exit.performed += Exit;
    }

    private void Exit(InputAction.CallbackContext context)
    {
        PlayerInputManager.instance.splitScreen = false;
        FindObjectOfType<MenuCanvasManager>()?.EnableMenu();

        foreach (WorldPlayer player in FindObjectsOfType<WorldPlayer>()) player.SwitchInput();
    }

    public void SwitchInput()
    {
        InputManager.SwitchPlayerInput(m_input.playerInput, m_playerInput);
        gameObject.SetActive(false);
    }
}
