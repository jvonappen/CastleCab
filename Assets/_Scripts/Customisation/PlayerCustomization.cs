using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCustomization : MonoBehaviour
{
    [SerializeField] Transform m_modelBase;

    PlayerInput m_playerInput;
    public PlayerInput playerInput { get { return m_playerInput; } }
    public void SetOtherPlayerInput(PlayerInput _input) => m_playerInput = _input;

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
        StoreCustomisationsToPlayer(m_input.playerInput, m_playerInput.transform);
        ApplyCosmeticsToPlayer();
    }

    private void OnEnable()
    {
        m_input.m_playerControls.UI.Exit.performed += Exit;
        ApplyCosmeticsToPlayer();
    }
    private void OnDisable() => m_input.m_playerControls.UI.Exit.performed -= Exit;

    private void Exit(InputAction.CallbackContext context)
    {
        //FindObjectOfType<PlayerInputManager>().EnableJoining();
        FindObjectOfType<InputManager>().SetCanJoin(true);

        PlayerInputManager.instance.splitScreen = false;
        FindObjectOfType<MenuCanvasManager>()?.EnableMenu();

        foreach (PlayerCustomization player in FindObjectsOfType<PlayerCustomization>()) player.SwitchInput();
    }

    public void SwitchInput()
    {
        InputManager.SwitchPlayerInput(m_input.playerInput, m_playerInput);

        GameManager.Instance.ApplyCustomisationsToPlayer(m_playerInput.gameObject);

        gameObject.SetActive(false);
    }

    public void ApplyCosmeticsToPlayer()
    {
        if (m_playerInput)
        {
            HorseColourSelector[] horseSelectors = gameObject.GetComponentsInChildren<HorseColourSelector>(true);
            foreach (HorseColourSelector selector in horseSelectors)
            {
                selector.Init();
                selector.GetComponent<SkinSelector>().Init();
            }


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

            foreach (HorseColourSelector selector in horseSelectors) selector.SetDyes(data.horseMat);
        }
    }

    public void StoreCustomisationsToPlayer()
    {
        StoreCustomisationsToPlayer(m_input.playerInput, m_modelBase);
        ApplyCosmeticsToPlayer();
    }
    public static void StoreCustomisationsToPlayer(PlayerInput _input, Transform _baseModel)
    {
        InputDevice device;
        if (_input && _input.devices.Count > 0) device = _input.devices[0];
        else return;

        List<ModelCustomization> modelCustomizations = new();
        foreach (ModelSelector selector in _baseModel.GetComponentsInChildren<ModelSelector>(true))
        {
            selector.Init();
            modelCustomizations.Add(new(selector));
        }

        PlayerData data = GameManager.Instance.GetPlayerData(device);
        GameManager.Instance.SetPlayerData(device, new(data.player, data.device, modelCustomizations, GetHorseMat(_baseModel.GetComponentInChildren<HorseColourSelector>(true)), data.playerUpgradeData));
    }

    public static HorseMatInformation GetHorseMat(HorseColourSelector _selector)
    {
        if (_selector.skinSelector == null) Debug.LogWarning(_selector + " skinSelector does not exist");
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
