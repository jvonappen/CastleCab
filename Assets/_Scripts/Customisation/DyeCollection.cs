using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class DyeCollection : MonoBehaviour
{
    //[SerializeField] CustomisationDisplay m_customisationDisplay;
    public MultiplayerEventSystem m_eventSystem;
    //[SerializeField] CategorySelector m_categorySelector;
    //public CategorySelector categorySelector { get { return m_categorySelector; } }

    [SerializeField] GameObject m_firstSelected;
    //public GameObject firstSelected { get { return m_firstSelected; } }

    [SerializeField] GameObject m_buttonPrefab;
    [SerializeField] List<SO_Dye> m_dyes;

    List<Button> m_buttons = new();

    DyeSlot m_dyeSlot;

    //SO_Dye m_selectedDye;
    //public SO_Dye selectedDye { get { return m_selectedDye; } set { m_selectedDye = value; } }

    private void Awake()
    {
        DyeButton eraserButton = transform.GetChild(0).GetComponent<DyeButton>();
        eraserButton.Init(this, null);
        m_buttons.Add(eraserButton.GetComponent<Button>());

        foreach (SO_Dye dye in m_dyes)
        {
            GameObject button = Instantiate(m_buttonPrefab);
            button.transform.SetParent(transform, false);

            DyeButton dyeButton = button.GetComponent<DyeButton>();
            dyeButton.Init(this, dye);

            m_buttons.Add(dyeButton.GetComponent<Button>());
        }
    }

    public void SetSlot(DyeSlot _slot) => m_dyeSlot = _slot;

    public void SelectDye(SO_Dye _dye)
    {
        m_dyeSlot.SetDye(_dye);

        if (m_dyeSlot.m_buttonToSelect) m_eventSystem.SetSelectedGameObject(m_dyeSlot.m_buttonToSelect);
        if (m_dyeSlot.m_nextSlot) m_dyeSlot.m_nextSlot.onClick?.Invoke();
        else m_dyeSlot.GetComponent<CustomButton>().Deselect();
    }

    public void SelectEraser() => SelectDye(null);

    public void SetInteraction(bool _canInteract)
    {
        foreach (Button button in m_buttons) button.interactable = _canInteract;
    }
}
