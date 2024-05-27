using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class DyeCollection : MonoBehaviour
{
    [SerializeField] CustomisationDisplayOld m_customisationDisplay;
    [SerializeField] CategorySelector m_categorySelector;
    public CategorySelector categorySelector { get { return m_categorySelector; } }

    [SerializeField] GameObject m_firstSelected;
    public GameObject firstSelected { get { return m_firstSelected; } }

    [SerializeField] GameObject m_buttonPrefab;
    [SerializeField] List<SO_Dye> m_dyes;

    List<Button> m_buttons = new();

    SO_Dye m_selectedDye;
    public SO_Dye selectedDye { get { return m_selectedDye; } set { m_selectedDye = value; } }

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

    public void SelectDye(SO_Dye _dye)
    {
        selectedDye = _dye;

        GameObject dyeSlotToSelect;// = categorySelector.m_selectedObject.transform.GetChild(2).gameObject;
        if (categorySelector.m_selectedObject.transform.childCount < 3)
        {
            dyeSlotToSelect = categorySelector.selections[0].transform.GetChild(2).gameObject;
            //categorySelector.SelectObject(dyeSlotToSelect);
        }
        else dyeSlotToSelect = categorySelector.m_selectedObject.transform.GetChild(2).gameObject;

        categorySelector.SetDyeInteraction(true);
        m_customisationDisplay.input.playerInput.uiInputModule.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(dyeSlotToSelect);
        SetInteraction(false);

        m_customisationDisplay.GreyOutSelectorMenu(true);
    }

    public void SelectEraser() => SelectDye(null);

    public void SetInteraction(bool _canInteract)
    {
        foreach (Button button in m_buttons) button.interactable = _canInteract;
    }
}
