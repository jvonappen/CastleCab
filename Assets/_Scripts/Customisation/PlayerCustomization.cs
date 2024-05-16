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
        m_playerHorseColourSelector = m_playerInput.GetComponentInChildren<HorseColourSelector>();
    }

    ModelSelector m_customizeModelSelector;
    ModelSelector m_playerModelSelector;

    HorseColourSelector m_horseColourSelector;
    HorseColourSelector m_playerHorseColourSelector;

    PlayerInputHandler m_input;
    private void Awake()
    {
        m_customizeModelSelector = transform.GetComponentInChildren<ModelSelector>();
        m_horseColourSelector = transform.GetComponentInChildren<HorseColourSelector>();

        m_input = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable() => m_input.m_playerControls.UI.Exit.performed += Exit;
    private void OnDisable() => m_input.m_playerControls.UI.Exit.performed -= Exit;

    private void Exit(InputAction.CallbackContext context)
    {
        FindObjectOfType<PlayerInputManager>().EnableJoining();

        PlayerInputManager.instance.splitScreen = false;
        FindObjectOfType<MenuCanvasManager>()?.EnableMenu();

        foreach (PlayerCustomization player in FindObjectsOfType<PlayerCustomization>()) player.SwitchInput();
    }

    public void SwitchInput()
    {
        InputManager.SwitchPlayerInput(m_input.playerInput, m_playerInput);
        m_customizeModelSelector.CopySelectionToSelector(m_playerModelSelector);
        m_horseColourSelector.CopyMatToSelector(m_playerHorseColourSelector);

        StoreCustomizationsToPlayer();

        gameObject.SetActive(false);
    }

    public void StoreCustomizationsToPlayer()
    {
        List<ModelCustomization> modelCustomizations = new();
        foreach (ModelSelector selector in GetComponentsInChildren<ModelSelector>()) modelCustomizations.Add(new(selector));

        // Temp
        //List<ModelCustomization> modelCustomizations = new() { new(m_customizeModelSelector) };

        InputDevice device;
        if (m_playerInput.devices.Count > 0) device = m_playerInput.devices[0];
        else device = GetComponent<PlayerInput>().devices[0];

        PlayerData data = GameManager.Instance.GetPlayerData(device);
        GameManager.Instance.SetPlayerData(device, new(data.player, data.device, modelCustomizations, GetHorseMat()));
    }

    public HorseMatInformation GetHorseMat()
    {
        return new HorseMatInformation(
            m_horseColourSelector.GetDye("Base"),
            m_horseColourSelector.GetDye("Hair"),
            m_horseColourSelector.GetDye("Tail"),
            m_horseColourSelector.GetDye("Nose"),
            m_horseColourSelector.GetDye("Feet"),
            m_horseColourSelector.GetDye("Horse_Pattern"),
            m_horseColourSelector.GetSelectedPattern(),
            m_horseColourSelector.skinSelector.GetSelectedSkin()
            );
    }
}
