using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectHelper : MonoBehaviour
{
    [SerializeField] List<CustomButton> m_buttons;

    public void SelectButtonByIndex(int _index)
    {
        DeselectAll();
        m_buttons[_index].Select();
    }

    public void DeselectAll()
    {
        for (int i = 0; i < m_buttons.Count; i++)
        {
            if (m_buttons[i].isClickSelected) m_buttons[i].Deselect();
        }
    }
}
