using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class DyeCollection : MonoBehaviour
{
    public MultiplayerEventSystem m_eventSystem;

    [SerializeField] GameObject m_firstSelected;

    [SerializeField] GameObject m_buttonPrefab;
    [SerializeField] List<SO_Dye> m_dyes;

    List<Button> m_buttons = new();

    DyeSlot m_dyeSlot;

    [SerializeField] UnityEvent onDyesSelected;

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

    public void OnSlotClicked(DyeSlot _slot)
    {
        if (_slot == m_dyeSlot) m_eventSystem.SetSelectedGameObject(m_firstSelected);
    }

    public void SetSlot(DyeSlot _slot) => m_dyeSlot = _slot;

    public void SelectDye(SO_Dye _dye)
    {
        m_dyeSlot.SetDye(_dye);

        if (m_dyeSlot.m_buttonToSelect) m_eventSystem.SetSelectedGameObject(m_dyeSlot.m_buttonToSelect);
        if (m_dyeSlot.m_nextSlot) m_dyeSlot.SelectNextSlot();
        else
        {
            CustomButton thisButton = m_dyeSlot.GetComponent<CustomButton>();
            thisButton.Deselect();
            thisButton.SetInteractable(false);
        }
    }

    public void SelectEraser() => SelectDye(null);

    public void SetInteraction(bool _canInteract)
    {
        foreach (Button button in m_buttons) button.interactable = _canInteract;
    }

    public void OnSelect() => onDyesSelected?.Invoke();
}
