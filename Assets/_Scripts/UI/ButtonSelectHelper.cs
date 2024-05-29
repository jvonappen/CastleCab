using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelectHelper : MonoBehaviour
{
    [SerializeField] List<CustomButton> m_buttons;

    public void UpdateButtonInteractions()
    {
        for (int i = 0; i < m_buttons.Count; i++) m_buttons[i].SetInteractable(m_buttons[i].isClickSelected);
    }

    public void EnableButtonInteractions()
    {
        for (int i = 0; i < m_buttons.Count; i++) m_buttons[i].SetInteractable(true);
    }

    public void SelectButtonByIndex(int _index) => SelectButton(m_buttons[_index]);
    public void SelectButton(CustomButton _button)
    {
        DeselectAll();
        _button.Select();

        //if (m_restrainSelection) UpdateButtonInteractions();
    }

    public void DeselectAll()
    {
        for (int i = 0; i < m_buttons.Count; i++)
        {
            if (m_buttons[i].isClickSelected) m_buttons[i].Deselect();
        }
    }
}
