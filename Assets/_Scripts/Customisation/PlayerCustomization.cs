using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCustomization : MonoBehaviour
{
    PlayerInput m_playerInput;
    public PlayerInput playerInput { get { return m_playerInput; } }
    public void SetOtherPlayerInput(PlayerInput _input)
    {
        m_playerInput = _input;
    }

    HorseColourSelector m_horseColourSelector;

    PlayerInputHandler m_input;
    private void Awake()
    {
        m_horseColourSelector = transform.GetComponentInChildren<HorseColourSelector>();

        m_input = GetComponent<PlayerInputHandler>();
    }

    private void Start()
    {
        m_horseColourSelector.skinSelector.Init();
        StoreCustomizationsToPlayer(m_input.playerInput, m_playerInput.gameObject, true);
        ApplyCosmeticsToPlayer();
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

        StoreCustomizationsToPlayer(true);

        GameManager.Instance.ApplyCustomisationsToPlayer(m_playerInput.gameObject);

        gameObject.SetActive(false);
    }

    public void ApplyCosmeticsToPlayer()
    {
        HorseColourSelector horseSelector = gameObject.GetComponentInChildren<HorseColourSelector>(true);
        horseSelector.Init();
        horseSelector.GetComponent<SkinSelector>().Init();

        PlayerData data = GameManager.Instance.GetPlayerData(m_playerInput.gameObject);

        foreach (ModelSelector modelSelector in gameObject.GetComponentsInChildren<ModelSelector>(true))
        {
            modelSelector.Init();

            ModelCustomization foundItem = data.modelCustomizations.FirstOrDefault(item => item.typeIndex == modelSelector.m_typeIndex);

            modelSelector.PreviewObjectByIndex(foundItem.index);
            modelSelector.SelectObject();

            if (foundItem.mat.mainDye.colour != null) modelSelector.colourSelector.SetDye("Main", foundItem.mat.mainDye);
            if (foundItem.mat.secondaryDye.colour != null) modelSelector.colourSelector.SetDye("Secondary", foundItem.mat.secondaryDye);
            if (foundItem.mat.tertiaryDye.colour != null) modelSelector.colourSelector.SetDye("Tertiary", foundItem.mat.tertiaryDye);
        }

        horseSelector.SetDyes(data.horseMat);
    }

    public void StoreCustomizationsToPlayer(bool _storeInactive = false) => StoreCustomizationsToPlayer(m_playerInput, gameObject, _storeInactive);

    public static void StoreCustomizationsToPlayer(PlayerInput _input, GameObject _basePlayer, bool _storeInactive = false)
    {
        List<ModelCustomization> modelCustomizations = new();
        foreach (ModelSelector selector in _basePlayer.GetComponentsInChildren<ModelSelector>(_storeInactive))
        {
            selector.Init();
            modelCustomizations.Add(new(selector));
        }

        InputDevice device;
        if (_input.devices.Count > 0) device = _input.devices[0];
        else device = _basePlayer.GetComponent<PlayerInput>().devices[0];

        PlayerData data = GameManager.Instance.GetPlayerData(device);
        GameManager.Instance.SetPlayerData(device, new(data.player, data.device, modelCustomizations, GetHorseMat(_basePlayer.GetComponentInChildren<HorseColourSelector>(true))));
    }

    public static HorseMatInformation GetHorseMat(HorseColourSelector _selector)
    {
        return new HorseMatInformation(
            _selector.GetDye("Base"),
            _selector.GetDye("Hair"),
            _selector.GetDye("Tail"),
            _selector.GetDye("Nose"),
            _selector.GetDye("Feet"),
            _selector.GetDye("Horse_Pattern"),
            _selector.GetSelectedPattern(),
            _selector.skinSelector.GetSelectedSkin()
            );
    }
}
