using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class CustomisationDisplay : MonoBehaviour
{
    [SerializeField] PlayerInputHandler m_input;
    public PlayerInputHandler input { get { return m_input; } }
    [SerializeField] ToggleDisplay m_display;
    [SerializeField] CategorySelector m_categorySelector;
    [SerializeField] GameObject m_modeMenu1, m_modeMenu2;

    [SerializeField] TextMeshProUGUI m_menuText;

    DyeCollection m_dyeCollection;

    bool m_isMode1 = true;

    MultiplayerEventSystem m_eventSystem;

    GameObject m_selectedDisplay;

    ModelSelector m_currentModelSelector;
    public void SetModelSelector(ModelSelector _modelSelector) => m_currentModelSelector = _modelSelector;

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

    private void Start()
    {
        m_eventSystem = m_input.playerInput.uiInputModule.GetComponent<MultiplayerEventSystem>();
        m_dyeCollection = m_modeMenu2.GetComponent<DyeCollection>();

        SelectMenu1();
    }

    void ToggleMode(InputAction.CallbackContext context) => ToggleMode();
    public void ToggleMode()
    {
        if (m_isMode1) SelectMenu2();
        else SelectMenu1();
    }

    void SelectMenu1()
    {
        // TEMPORARY
        m_currentModelSelector = m_modeMenu1.GetComponent<ModelCollection>().modelSelector;

        m_display.Display1();

        m_selectedDisplay = m_modeMenu1;

        m_modeMenu1.SetActive(true);
        m_modeMenu2.SetActive(false);

        m_isMode1 = true;

        ExitSelector();
    }

    void SelectMenu2()
    {
        m_dyeCollection.SetInteraction(true);
        m_currentModelSelector.SelectSelected();

        m_display.Display2();

        m_selectedDisplay = m_modeMenu2;

        m_modeMenu1.SetActive(false);
        m_modeMenu2.SetActive(true);

        m_isMode1 = false;

        SelectButton();
    }

    public void SelectButton()
    {
        m_categorySelector.SetInteraction(false);
        m_categorySelector.SetDyeInteraction(false);

        GameObject buttonToSelect = m_selectedDisplay.GetComponentInChildren<Button>().gameObject;
        if (m_selectedDisplay == m_modeMenu2) buttonToSelect = m_dyeCollection.firstSelected;

        m_eventSystem.SetSelectedGameObject(buttonToSelect);

        GreyOutSelectorMenu(false);
    }

    void Cancel(InputAction.CallbackContext context)
    {
        if (m_categorySelector.canInteractDyes) SelectMenu2();
        else SelectMenu1();
    }
    void ExitSelector()
    {
        m_currentModelSelector.SelectSelected();
        m_categorySelector.SetInteraction(true);
        m_eventSystem.SetSelectedGameObject(m_categorySelector.m_selectedObject);

        GreyOutSelectorMenu(true);
    }

    public void GreyOutSelectorMenu(bool _isGreyedOut)
    {
        if (_isGreyedOut)
        {
            m_menuText.alpha = 125f / 255f;
            foreach (Image image in m_selectedDisplay.GetComponentsInChildren<Image>()) image.color = new(image.color.r, image.color.g, image.color.b, 125f / 255f);
        }
        else
        {
            m_menuText.alpha = 1;
            foreach (Image image in m_selectedDisplay.GetComponentsInChildren<Image>()) image.color = new(image.color.r, image.color.g, image.color.b, 1);
        }
    }
}
