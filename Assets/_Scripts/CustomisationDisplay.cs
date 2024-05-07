using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class CustomisationDisplay : MonoBehaviour
{
    [SerializeField] PlayerInputHandler m_input;
    [SerializeField] CategorySelector m_categorySelector;
    [SerializeField] GameObject m_modeMenu1, m_modeMenu2;

    bool m_isMode1 = true;

    MultiplayerEventSystem m_eventSystem;

    GameObject m_selectedDisplay;

    private void Awake() => m_selectedDisplay = m_modeMenu1;

    private void OnEnable()
    {
        m_input.m_playerControls.UI.ToggleCustomiseMode.performed += ToggleMode;
        m_input.m_playerControls.UI.Cancel.performed += Cancel;
    }
    private void OnDisable()
    {
        m_input.m_playerControls.UI.ToggleCustomiseMode.performed -= ToggleMode;
        m_input.m_playerControls.UI.Cancel.performed -= Cancel;
    }

    private void Start() => m_eventSystem = m_input.playerInput.uiInputModule.GetComponent<MultiplayerEventSystem>();

    void ToggleMode(InputAction.CallbackContext context) => ToggleMode();
    public void ToggleMode()
    {
        if (m_isMode1) SelectMenu2();
        else SelectMenu1();
    }

    void SelectMenu1()
    {
        m_selectedDisplay = m_modeMenu1;

        m_modeMenu1.SetActive(true);
        m_modeMenu2.SetActive(false);

        m_isMode1 = true;

        ExitSelector();
    }

    void SelectMenu2()
    {
        m_selectedDisplay = m_modeMenu2;

        m_modeMenu1.SetActive(false);
        m_modeMenu2.SetActive(true);

        m_isMode1 = false;

        SelectButton();
    }

    public void SelectButton()
    {
        m_categorySelector.SetInteraction(false);
        m_eventSystem.SetSelectedGameObject(m_selectedDisplay.transform.GetComponentInChildren<Button>().gameObject);
    }

    void Cancel(InputAction.CallbackContext context) => SelectMenu1();
    void ExitSelector()
    {
        m_categorySelector.SetInteraction(true);
        m_eventSystem.SetSelectedGameObject(m_categorySelector.m_selectedObject);
    }
}
