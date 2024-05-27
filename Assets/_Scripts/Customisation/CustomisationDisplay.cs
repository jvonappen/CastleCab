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
    [SerializeField] MultiplayerEventSystem m_eventSystem;

    [SerializeField] CategorySelector m_categorySelector;

    [SerializeField] GameObject m_selectorBase;
    [SerializeField] GameObject m_categorySelectorBase;

    [SerializeField] TextMeshProUGUI m_menuText;

    GameObject m_selectedDisplay;

    CustomisationSelector m_selector;

    ModelSelector m_currentModelSelector;
    public void SetModelSelector(ModelSelector _modelSelector) => m_currentModelSelector = _modelSelector;

    private void OnEnable() => m_input.m_playerControls.UI.Cancel.performed += Back;
    private void OnDisable() => m_input.m_playerControls.UI.Cancel.performed -= Back;

    public void SetMenu(GameObject _menu)
    {
        m_selectorBase.SetActive(true);
        m_categorySelectorBase.SetActive(false);

        if (m_selectedDisplay) m_selectedDisplay.SetActive(false);
        
        m_selectedDisplay = _menu;
        m_selectedDisplay.SetActive(true);

        if (m_selectedDisplay.TryGetComponent(out SelectorCollection collection)) m_selector = collection.selector;

        SelectButton();
    }

    public void SelectButton()
    {
        m_categorySelector.SetInteraction(false);
        m_categorySelector.SetDyeInteraction(false);

        Debug.Log(m_selectedDisplay);
        GameObject buttonToSelect = m_selectedDisplay.GetComponentInChildren<Button>().gameObject;
        DyeCollection dyeCollection = m_selectedDisplay.GetComponent<DyeCollection>();
        if (dyeCollection) buttonToSelect = dyeCollection.firstSelected;

        m_eventSystem.SetSelectedGameObject(buttonToSelect);
    }

    void Back(InputAction.CallbackContext context) => ExitSelector();

    public void SetSelectedModel() { if (m_currentModelSelector) m_currentModelSelector.SelectSelected(); }

    public void ExitSelector()
    {
        m_selectedDisplay.SetActive(false);
        m_selectedDisplay = null;

        m_selectorBase.SetActive(false);
        m_categorySelectorBase.SetActive(true);

        SetSelectedModel();
        if (m_selector) m_selector.DisplaySelected();

        m_categorySelector.SetInteraction(true);
        if (m_eventSystem) m_eventSystem.SetSelectedGameObject(m_categorySelector.m_selectedObject);
    }
}
