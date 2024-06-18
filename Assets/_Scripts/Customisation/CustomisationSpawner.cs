using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomisationSpawner : MonoBehaviour
{
    [SerializeField] GameObject m_customizePlayerPrefab;
    GameObject m_customizationMenu;
    public GameObject customizationMenu { get { return m_customizationMenu; } }

    PlayerInputHandler m_playerInputHandler;
    PlayerInput m_customizationInput;

    private void OnEnable()
    {
        m_playerInputHandler = GetComponent<PlayerInputHandler>();
        m_playerInputHandler.onPaired += OnPlayerPaired;
    }
    private void OnDisable() => m_playerInputHandler.onPaired -= OnPlayerPaired;

    private void Start()
    {
        m_customizationMenu = Instantiate(m_customizePlayerPrefab);
        m_customizationMenu.name = gameObject.name + " Customiser";

        m_customizationMenu.transform.position = transform.position - Vector3.up * 1000;

        PlayerCustomization customisation = m_customizationMenu.GetComponent<PlayerCustomization>();
        customisation.SetOtherPlayerInput(m_playerInputHandler.playerInput);
        m_customizationMenu.SetActive(false);

        PlayerInputHandler customisationInputHandler = m_customizationMenu.GetComponent<PlayerInputHandler>();
        customisationInputHandler.SetPaired(true);
        m_customizationInput = customisationInputHandler.playerInput;

    }

    void OnPlayerPaired()
    {
        //PlayerCustomization.StoreCustomizationsToPlayer(m_customizationInput, gameObject);
    }

    public void StartCustomization()
    {
        if (m_customizationMenu)
        {
            m_customizationMenu.SetActive(true);
            InputManager.SwitchPlayerInput(m_playerInputHandler.playerInput, m_customizationInput);

            m_customizationMenu.GetComponent<SwitchUI2P>().OpenDefaultMenu();
        }
    }
}
