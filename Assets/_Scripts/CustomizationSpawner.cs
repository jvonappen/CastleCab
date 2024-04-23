using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomizationSpawner : MonoBehaviour
{
    [SerializeField] GameObject m_customizePlayerPrefab;
    GameObject m_customizationMenu;
    public GameObject customizationMenu { get { return m_customizationMenu; } }

    PlayerInput m_playerInput;
    PlayerInput m_customizationInput;

    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();

        m_customizationMenu = Instantiate(m_customizePlayerPrefab);
        m_customizationMenu.transform.position = transform.position - Vector3.up * 1000;
        m_customizationMenu.GetComponent<WorldPlayer>().SetOtherPlayerInput(m_playerInput);
        m_customizationMenu.SetActive(false);

        m_customizationInput = m_customizationMenu.GetComponent<PlayerInput>();
    }

    public void StartCustomization()
    {
        m_customizationMenu.SetActive(true);
        InputManager.SwitchPlayerInput(m_playerInput, m_customizationInput);
    }
}
