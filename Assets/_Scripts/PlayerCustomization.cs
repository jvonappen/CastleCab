using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCustomization : MonoBehaviour
{
    PlayerInput m_playerInput;
    public PlayerInput playerInput { get { return m_playerInput; } }
    public void SetOtherPlayerInput(PlayerInput _input)
    {
        m_playerInput = _input;
        m_playerModelSelector = m_playerInput.GetComponentInChildren<ModelSelector>();
    }

    ModelSelector m_customizeModelSelector;
    ModelSelector m_playerModelSelector;

    PlayerInputHandler m_input;
    private void Awake()
    {
        m_customizeModelSelector = transform.GetComponentInChildren<ModelSelector>();
        m_input = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable() => m_input.m_playerControls.UI.Exit.performed += Exit;
    private void OnDisable() => m_input.m_playerControls.UI.Exit.performed -= Exit;

    private void Exit(InputAction.CallbackContext context)
    {
        GameManager.SetCustomizing(false);

        PlayerInputManager.instance.splitScreen = false;
        FindObjectOfType<MenuCanvasManager>()?.EnableMenu();

        foreach (PlayerCustomization player in FindObjectsOfType<PlayerCustomization>()) player.SwitchInput();
    }

    public void SwitchInput()
    {
        InputManager.SwitchPlayerInput(m_input.playerInput, m_playerInput);
        m_customizeModelSelector.CopySelectionToSelector(m_playerModelSelector);

        StoreCustomizationsToPlayer();

        gameObject.SetActive(false);
    }

    void StoreCustomizationsToPlayer()
    {
        //List<ModelCustomization> modelCustomizations = new();
        //foreach (ModelSelector in obj) // etc etc

        // Temp
        List<ModelCustomization> modelCustomizations = new() { new(m_customizeModelSelector) };

        PlayerData data = GameManager.Instance.GetPlayerData(m_playerInput.devices[0]);
        GameManager.Instance.SetPlayerData(m_playerInput.devices[0], new(data.player, data.device, modelCustomizations));
    }
}
